using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace DownloadLibaray
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begin");
            for (int i = 0; i < 500000; i++)
            {
                try
                { 
                    Save(i, GvCrawler.Get("http://10.60.20.10/cgi-bin/DispBibDetail?v_recno=" + i + "&v_curdbno=0"));
                    Console.WriteLine("Save " + i);
                    Thread.Sleep(100);
                }
                catch(Exception ex)
                {
                    log(ex.Message + "\t" + i);
                }
            }
        }

        /// <summary>
        /// 保存抓取到的网页
        /// </summary>
        /// <param name="nId">网页ID</param>
        /// <param name="shtml">网页源代码</param>
        /// <returns>保存路径</returns>
        static string Save(int nId, string shtml)
        {
            if (shtml.IndexOf("没有满足条件的记录") > 0
             || shtml.IndexOf("未指定数据库或书目记录号") > 0
             || shtml.IndexOf("打开主参数库错误") > 0)
                return "";
            string sPath = "lib_" + nId + ".html";
            using(StreamWriter wr = new StreamWriter(sPath, false, Encoding.GetEncoding("GB2312")))
            {
                wr.Write(shtml);
            }
            return sPath;
        }

        /// <summary>
        /// 记录Error Log
        /// </summary>
        /// <param name="sErr">Error</param>
        static void log(string sErr)
        {
            using (StreamWriter wr = new StreamWriter("error.log", true))
            {
                wr.WriteLine(sErr);
            }
        }
    }
}
