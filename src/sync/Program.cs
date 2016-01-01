using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Data;

namespace SyncLib
{
    struct lib_info
    {
        public int nId;
        public int nPage;
        public double fPrice;
        public string sTitle;
        public string sAuthor;
        public string sPress;
        public string sCurrency;
        public string sISBN;
        public string sSubject;
        public string sType;
        public string sCallNo;
        public string sDesc;
    }
    class Program
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        static readonly string SERVER = "SERVER";
        static readonly string DATABASE = "DATABASE";
        static readonly string USER = "USER";
        static readonly string PASSWD = "PASSWD";

        static int _j = 0;

        /// <summary>
        /// 将提取任务分割为几块
        /// </summary>
        static int _th = 5;

        static void Main(string[] args)
        {
            for (int i = 0; i < _th; i++)
            {
                Thread th = new Thread(Fun);
                th.Start();
            }

            while (_th > 0) ;
        }

        static void Fun()
        {
            Run(_j++);
        }

        /// <summary>
        /// 开始一个提取任务，此处用于多线程调用将任务分成_th块，每块各开一个线程提取
        /// </summary>
        /// <param name="j">任务索引，也用于标记任务起始提取ID</param>
        static void Run(int j)
        {
            int nBase = 500000 / _th;
            Console.WriteLine("Begin " + (nBase * j) + " to " + ((j + 1) * nBase));
            int n = 0;
            for (int i = nBase * j; i < nBase * (j + 1); i++)
            {
                lib_info? info = WriteData(i);
                if (n++ > 1000 && null != info) Console.WriteLine("Sync Data " + "lib_" + (i + (n = 0)) + ".html now : " + info.Value.sTitle);
            }
            _th--;
        }

        /// <summary>
        /// 提取下载的网页中书籍信息
        /// </summary>
        /// <param name="nId">书籍ID</param>
        /// <returns>书籍结构体，若为null，这表示该网页不包含书籍信息</returns>
        static lib_info? WriteData(int nId)
        {
            string sHTML = ReadFile("lib_" + nId + ".html");
            if (string.IsNullOrEmpty(sHTML) || sHTML.IndexOf("打开主参数库错误") >= 0) return null;
            lib_info info = ToInfo(sHTML, nId);
            if (info.sTitle == "") return null;
            if (info.sTitle == "临时") return null;
            WriteSQL(ToSQL(info));
            return info;
        }

        /// <summary>
        /// 生成写入数据的SQL语句
        /// </summary>
        /// <param name="info">书籍信息结构体</param>
        /// <returns>SQL语句</returns>
        static string ToSQL(lib_info info)
        {
            return @" delete from [dbo].[sxt_lib_info] where [lib_id] = " + info.nId + @"
            INSERT INTO [dbo].[sxt_lib_info]
                       ([lib_id]
                       ,[lib_title]
                       ,[lib_author]
                       ,[lib_press]
                       ,[lib_subject]
                       ,[lib_type]
                       ,[lib_page]
                       ,[lib_price]
                       ,[lib_currency]
                       ,[lib_isbn]
                       ,[lib_callno]
                       ,[lib_desc])
                 VALUES
                       ('" + info.nId + @"'
                       ,'" + info.sTitle.Replace("'", "''") + @"'
                       ,'" + info.sAuthor.Replace("'", "''") + @"'
                       ,'" + info.sPress.Replace("'", "''") + @"'
                       ,'" + info.sSubject.Replace("'", "''") + @"'
                       ,'" + info.sType.Replace("'", "''") + @"'
                       ,'" + info.nPage + @"'
                       ,'" + info.fPrice.ToString("0.000") + @"'
                       ,'" + info.sCurrency.Replace("'", "''") + @"'
                       ,'" + info.sISBN.Replace("'", "''") + @"'
                       ,'" + info.sCallNo.Replace("'", "''") + @"'
                       ,'" + info.sDesc.Replace("'", "''") + @"')";
        }

        /// <summary>
        /// 从网页中提取书籍内容
        /// </summary>
        /// <param name="sHTML">网页源代码</param>
        /// <param name="nId">书籍ID</param>
        /// <returns>书籍内容结构体</returns>
        static lib_info ToInfo(string sHTML, int nId)
        {
            // 获取题名
            string reTitle = @"\('TITLE','(.*?)'\)";
            // 获取作者
            string reAuthor = @"\('AUTHOR','([^']*?)'\)"">[^<]";
            // 出版社
            string rePress = @"<b>出版项:</b></font>(.*?)</td>";
            // 页码
            string rePage = @"<b>页码:&nbsp; </b></font>[^\d]*?(\d*?)[^\d]";
            // 价格
            string rePrice = @"<b>价格:&nbsp; </b></font>[^\d]*?([\d.]*?)[^\d.]";
            // 币别
            string reCurrency = @"<b>价格:&nbsp; </b></font>([^\d]*?)[\d.]*?&nbsp;</td>";
            // ISBN
            string reIsbn = @"<span id=""isbn"">(.*?)</span>";
            // 主题
            string reSubject = @"\('SUBJECT','([^']*?)'\)"">[^<]";
            // 索取号
            string reCallNo = @"\('CALLNO','(.*?)'\)";
            // 摘要
            string reDesc = @"<b>摘要:</b></font> (.*?)</a></td>";
            // 分类
            string reType = @"\('CLASSNO','(.*?)'\)";

            lib_info info = new lib_info();
            info.nId = nId;
            info.sTitle = GetData(Analtytic(reTitle, sHTML));
            info.sAuthor = GetDatas(Analtytic(reAuthor, sHTML));
            info.sPress = GetData(Analtytic(rePress, sHTML));
            try { info.nPage = Convert.ToInt32(GetData(Analtytic(rePage, sHTML))); }
            catch(Exception){info.nPage = -1;}
            try { info.fPrice = Convert.ToDouble(GetData(Analtytic(rePrice, sHTML))); }
            catch (Exception) { info.fPrice = -1; }
            info.sCurrency = GetData(Analtytic(reCurrency, sHTML));
            info.sISBN = GetData(Analtytic(reIsbn, sHTML));
            info.sSubject = GetDatas(Analtytic(reSubject, sHTML));
            info.sCallNo = GetData(Analtytic(reCallNo, sHTML));
            info.sDesc = GetData(Analtytic(reDesc, sHTML));
            info.sType = GetData(Analtytic(reType, sHTML));

            return info;
        }

        /// <summary>
        /// 将匹配对象组合成为字符串，仅取第一个匹配到的数据
        /// </summary>
        /// <param name="match">匹配对象</param>
        /// <returns>转换后的字符串</returns>
        static string GetData(MatchCollection match)
        {
            string sData = "";
            foreach (Match mat in match)
            {
                sData = mat.Groups[1].Value.Trim();
                return sData;
            }
            return sData;
        }

        /// <summary>
        /// 将匹配对象组合成为字符串，有多个匹配值时，逗号隔开
        /// </summary>
        /// <param name="match">匹配对象</param>
        /// <returns>转换后的字符串</returns>
        static string GetDatas(MatchCollection match)
        {
            string sData = ",";
            foreach (Match mat in match)
            {
                if (sData.IndexOf("," + mat.Groups[1].Value.Trim() + ",") < 0)
                    sData += mat.Groups[1].Value.Trim() + ",";
            }
            return sData.Trim(',');
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="sPath">文件路径</param>
        /// <returns>文件内容</returns>
        static string ReadFile(string sPath)
        {
            string sHtml = "";
            sPath = Environment.CurrentDirectory + @"\" + sPath;
            if (!File.Exists(sPath)) return "";
            using (StreamReader r = new StreamReader(sPath, Encoding.GetEncoding("GB2312")))
            {
                sHtml = r.ReadToEnd();
            }
            return sHtml;
        }

        /// <summary>
        /// 执行SQL写入数据
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        static int WriteSQL(string cmdText)
        {
            int nCount = 0;
            SqlConnection sqlConn = new SqlConnection("Data Source=" + SERVER + ";Initial Catalog=" + DATABASE + ";User ID=" + USER + ";Password=" + PASSWD + ";");
            int nConnTimes = 4;

            try
            {
                SqlCommand sqlComm = new SqlCommand(cmdText, sqlConn);
                for (int i = 0; i < nConnTimes; i++)
                {
                    try
                    {
                        sqlComm.Connection.Open();
                        i = nConnTimes;
                    }
                    finally { }
                }
                sqlComm.CommandTimeout = 0;
                nCount = sqlComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log("Error:" + ex.Message + ", SQL: " + cmdText.Replace("\r", "\\r").Replace("\n", "\\n"));
            }
            finally
            {
                sqlConn.Dispose();
            }

            return nCount;
        }

        /// <summary>
        /// 根据正则解析文本
        /// </summary>
        /// <param name="sRegEx">正则表达式</param>
        /// <param name="sHtml">网页源代码</param>
        /// <returns>返回匹配对象</returns>
        static public MatchCollection Analtytic(string sRegEx, string sHtml)
        {
            MatchCollection matches = Regex.Matches(sHtml, sRegEx, RegexOptions.IgnoreCase);
            //Console.WriteLine("Analtytic RegEx: " + sRegEx + " Count:" + matches.Count);
            return matches;
        }

        /// <summary>
        /// 记录Error Log
        /// </summary>
        /// <param name="sErr">Error</param>
        static void log(string sErr)
        {
            Console.WriteLine("###### " + sErr + " ######");
            using (StreamWriter wr = new StreamWriter("error.log", true))
            {
                wr.WriteLine(sErr);
            }
        }

    }
}
