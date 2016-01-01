﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace DownloadLibaray
{
    /// <summary>
    /// 网页爬虫类 by hancel.lin 2014/12/05
    /// </summary>
    static class GvCrawler
    {
        #region 私有变量
        static HttpHeader _header = new HttpHeader();
        static HttpStatusCode _status = HttpStatusCode.OK;
        static CookieContainer _cookies = new CookieContainer();
        #endregion

        #region 获取状态
        /// <summary>
        /// 获取最后抓取网页的状态
        /// </summary>
        /// <returns></returns>
        public static HttpStatusCode GetLastStatus()
        {
            return _status;
        }
        #endregion

        #region Get方法

        /// <summary>
        /// Get方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <returns>网页源码</returns>
        public static string Get(string sWebUrl)
        {
            return Get(sWebUrl, null, true, _cookies, ref _status);
        }

        /// <summary>
        /// Get方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sEncoding">指定网页编码</param>
        /// <returns>网页源码</returns>
        public static string Get(string sWebUrl, string sEncoding)
        {
            return Get(sWebUrl, sEncoding, true, _cookies, ref _status);
        }

        /// <summary>
        /// Get方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="cookieContainer">网页Cookies</param>
        /// <returns>网页源码</returns>
        public static string Get(string sWebUrl, CookieContainer cookieContainer)
        {
            return Get(sWebUrl, null, true, cookieContainer, ref _status);
        }

        /// <summary>
        /// Get方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sEncoding">指定网页编码</param>
        /// <param name="bResetSpace">过滤空白符</param>
        /// <returns>网页源码</returns>
        public static string Get(string sWebUrl, string sEncoding, bool bResetSpace)
        {
            return Get(sWebUrl, sEncoding, bResetSpace, _cookies, ref _status);
        }

        /// <summary>
        /// Get方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sEncoding">指定网页编码</param>
        /// <param name="cookieContainer">网页Cookies</param>
        /// <returns>网页源码</returns>
        public static string Get(string sWebUrl, string sEncoding, CookieContainer cookieContainer)
        {
            return Get(sWebUrl, sEncoding, true, cookieContainer, ref _status);
        }

        /// <summary>
        /// Get方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sEncoding">指定编码</param>
        /// <param name="bResetSpace">是否过滤多空白符</param>
        /// <param name="cookieContainer">网页访问Cookies</param>
        /// <returns>网页源码</returns>
        public static string Get(string sWebUrl, string sEncoding, bool bResetSpace, CookieContainer cookieContainer)
        {
            return Get(sWebUrl, sEncoding, bResetSpace, cookieContainer, ref _status);
        }

        /// <summary>
        /// Get方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sEncoding">指定编码</param>
        /// <param name="bResetSpace">是否过滤多空白符</param>
        /// <param name="cookieContainer">网页访问Cookies</param>
        /// <param name="httpStatus">网页访问状态</param>
        /// <returns>网页源码</returns>
        public static string Get(string sWebUrl, string sEncoding, bool bResetSpace, CookieContainer cookieContainer, ref HttpStatusCode httpStatus)
        {
            return Crawler(sWebUrl, null, sEncoding, bResetSpace, ref cookieContainer, ref httpStatus);

        }

        #endregion

        #region Post方法

        /// <summary>
        /// Post方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sData">Post数据</param>
        /// <returns>网页源码</returns>
        public static string Post(string sWebUrl, string sData)
        {
            return Post(sWebUrl, sData, null, true, ref _cookies, ref _status);
        }

        /// <summary>
        /// Post方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sData">Post数据</param>
        /// <param name="sEncoding">指定编码</param>
        /// <param name="cookieContainer">网页访问Cookies</param>
        /// <returns>网页源码</returns>
        public static string Post(string sWebUrl, string sData, string sEncoding, ref CookieContainer cookieContainer)
        {
            return Post(sWebUrl, sData, sEncoding, true, ref cookieContainer, ref _status);
        }

        /// <summary>
        /// Post方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sData">Post数据</param>
        /// <param name="cookieContainer">网页访问Cookies</param>
        /// <returns>网页源码</returns>
        public static string Post(string sWebUrl, string sData, ref CookieContainer cookieContainer)
        {
            return Post(sWebUrl, sData, null, true, ref cookieContainer, ref _status);
        }

        /// <summary>
        /// Post方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sData">Post数据</param>
        /// <param name="cookieContainer">网页访问Cookies</param>
        /// <returns>网页源码</returns>
        public static string Post(string sWebUrl, string sData, CookieContainer cookieContainer)
        {
            return Post(sWebUrl, sData, null, true, ref cookieContainer, ref _status);
        }

        /// <summary>
        /// Post方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sData">Post数据</param>
        /// <param name="sEncoding">指定编码</param>
        /// <returns>网页源码</returns>
        public static string Post(string sWebUrl, string sData, string sEncoding)
        {
            return Post(sWebUrl, sData, sEncoding, true, ref _cookies, ref _status);
        }

        /// <summary>
        /// Post方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sData">Post数据</param>
        /// <param name="sEncoding">指定编码</param>
        /// <param name="bResetSpace">是否过滤多空白符</param>
        /// <returns>网页源码</returns>
        public static string Post(string sWebUrl, string sData, string sEncoding, bool bResetSpace)
        {
            return Post(sWebUrl, sData, sEncoding, bResetSpace, ref _cookies, ref _status);
        }

        /// <summary>
        /// Post方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sData">Post数据</param>
        /// <param name="sEncoding">指定编码</param>
        /// <param name="bResetSpace">是否过滤多空白符</param>
        /// <param name="cookieContainer">网页访问Cookies</param>
        /// <returns>网页源码</returns>
        public static string Post(string sWebUrl, string sData, string sEncoding, bool bResetSpace, ref CookieContainer cookieContainer)
        {
            return Post(sWebUrl, sData, sEncoding, bResetSpace, ref cookieContainer, ref _status);
        }

        /// <summary>
        /// Post方式获取一个网页源码
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sData">Post数据</param>
        /// <param name="sEncoding">指定编码</param>
        /// <param name="bResetSpace">是否过滤多空白符</param>
        /// <param name="cookieContainer">网页访问Cookies</param>
        /// <param name="httpStatus">网页访问状态</param>
        /// <returns>网页源码</returns>
        public static string Post(string sWebUrl, string sData, string sEncoding, bool bResetSpace, ref CookieContainer cookieContainer, ref HttpStatusCode httpStatus)
        {
            return Crawler(sWebUrl, sData, sEncoding, bResetSpace, ref cookieContainer, ref httpStatus);
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 抓取网页函数
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sData">Post数据</param>
        /// <param name="sEncoding">指定编码</param>
        /// <param name="bResetSpace">是否过滤多空白符</param>
        /// <param name="cookieContainer">网页访问Cookies</param>
        /// <param name="httpStatus">网页访问状态</param>
        /// <returns>网页源码</returns>
        private static string Crawler(string sWebUrl, string sData, string sEncoding, bool bResetSpace, ref CookieContainer cookieContainer, ref HttpStatusCode httpStatus)
        {
            string sWebHtml = "";

            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (!string.IsNullOrEmpty(sWebUrl))
            {
                try
                {
                    httpRequest = (HttpWebRequest)HttpWebRequest.Create(sWebUrl);
                    httpRequest.CookieContainer = cookieContainer;
                    httpRequest.ContentType = _header.contentType;
                    httpRequest.ServicePoint.ConnectionLimit = _header.maxTry;
                    httpRequest.Referer = sWebUrl;
                    httpRequest.Accept = _header.accept;
                    httpRequest.UserAgent = _header.userAgent;

                    if (!string.IsNullOrEmpty(sData)) // Post
                    {
                        byte[] btDataArray = Encoding.UTF8.GetBytes(sData);
                        httpRequest.Method = "POST";
                        httpRequest.ContentLength = btDataArray.Length;

                        Stream dataStream = null;
                        dataStream = httpRequest.GetRequestStream();
                        dataStream.Write(btDataArray, 0, btDataArray.Length);
                        dataStream.Close();

                        httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    }
                    else    //Get
                    {
                        httpRequest.Method = "GET";
                        httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    }

                    httpStatus = httpResponse.StatusCode;

                    Stream resStream = httpResponse.GetResponseStream();
                    byte[] resByte = StreamToBytes(resStream);
                    sWebHtml = string.IsNullOrEmpty(sEncoding) ?
                        CorrectEncode(resByte) : // 校正编码
                        Encoding.GetEncoding(sEncoding).GetString(resByte);
                    if (bResetSpace)
                        sWebHtml = ResetSpace(sWebHtml);
                    httpResponse.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return sWebHtml;
        }

        /// <summary>
        /// 抓取网页函数
        /// </summary>
        /// <param name="sWebUrl">网页地址</param>
        /// <param name="sData">Post数据</param>
        /// <param name="sEncoding">指定编码</param>
        /// <param name="bResetSpace">是否过滤多空白符</param>
        /// <param name="cookieContainer">网页访问Cookies</param>
        /// <param name="httpStatus">网页访问状态</param>
        /// <returns>网页源码</returns>
        public static string Download(string sFile, string sWebUrl, string sData, string sEncoding, ref CookieContainer cookieContainer, ref HttpStatusCode httpStatus)
        {
            string sWebHtml = "";

            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (!string.IsNullOrEmpty(sWebUrl))
            {
                try
                {
                    httpRequest = (HttpWebRequest)HttpWebRequest.Create(sWebUrl);
                    httpRequest.CookieContainer = cookieContainer;
                    httpRequest.ContentType = "application/octet-stream";
                    httpRequest.ServicePoint.ConnectionLimit = _header.maxTry;
                    httpRequest.Referer = sWebUrl;
                    httpRequest.Accept = _header.accept;
                    httpRequest.UserAgent = _header.userAgent;

                    if (!string.IsNullOrEmpty(sData)) // Post
                    {
                        byte[] btDataArray = Encoding.UTF8.GetBytes(sData);
                        httpRequest.Method = "POST";
                        httpRequest.ContentLength = btDataArray.Length;

                        Stream dataStream = null;
                        dataStream = httpRequest.GetRequestStream();
                        dataStream.Write(btDataArray, 0, btDataArray.Length);
                        dataStream.Close();

                        httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    }
                    else    //Get
                    {
                        httpRequest.Method = "GET";
                        httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                    }

                    httpStatus = httpResponse.StatusCode;

                    Stream resStream = httpResponse.GetResponseStream();
                    byte[] resByte = StreamToBytes(resStream);
                    FileStream fs = new FileStream(sFile, FileMode.Create);
                    fs.Write(resByte, 0, resByte.Length);
                    fs.Close();
                    resStream.Close();
                    httpResponse.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return sWebHtml;
        }

        /// <summary>
        /// 校正源码字符编码
        /// </summary>
        /// <param name="sReader">文字流</param>
        /// <returns>转换后的字符串</returns>
        private static string CorrectEncode(byte[] btWebHtml)
        {
            string sWebHtml = Encoding.UTF8.GetString(btWebHtml);
            string pattern = @"(?i)\bcharset=(?<charset>[-a-zA-Z_0-9]+)";
            string sCharset = Regex.Match(sWebHtml, pattern).Groups["charset"].Value;
            return Encoding.GetEncoding(sCharset).GetString(btWebHtml);
        }

        /// <summary>
        /// 过滤多余空格
        /// </summary>
        /// <param name="sWebHtml">网页源码</param>
        /// <returns></returns>
        private static string ResetSpace(string sWebHtml)
        {
            Regex r = new Regex(@"\s+");
            return r.Replace(sWebHtml, " ");
        }

        /// <summary>
        /// 转换Stream为Byte数组
        /// </summary>
        /// <param name="stream">Stream对象</param>
        /// <returns></returns>
        private static byte[] StreamToBytes(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        #endregion

    }

    #region 文件头
    class HttpHeader
    {
        public string contentType;

        public string accept;

        public string userAgent;

        public string method;

        public int maxTry;

        public HttpHeader()
        {
            accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            contentType = "application/x-www-form-urlencoded";
            userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            maxTry = 300;
            method = "GET";
        }
    }
    #endregion

}