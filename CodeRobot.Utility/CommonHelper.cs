using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography;

namespace CodeRobot.Utility
{
    /// <summary>
    /// 版权所有：版权所有(C) 2016，Cloudin
    /// 内容摘要：常用工具封装
    /// 完成日期：2016年10月6日
    /// 版    本：V1.0 
    /// 作    者：Adin
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// 加密密钥
        /// </summary>
        public static string cryptographyKey = "CloudinCodeRobot2018";

        //随机字母
        private static char[] arWords = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        //随机数字
        private static char[] arNumeric = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// 获取随机字母
        /// </summary>
        /// <param name="iLength">所需生成随机字母的长度</param>
        /// <example>CommonHelper.GenerateRandomWords(4)，取得随机字母为：edge</example>
        /// <returns>返回随机字母</returns>
        public static string GenerateRandomWords(int iLength)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(26);
            Random rm = new Random();
            for (int i = 0; i < iLength; i++)
            {
                sb.Append(arWords[rm.Next(26)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 0~9数字中生成任意长度数字
        /// </summary>
        /// <param name="iLength"></param>
        /// <example>CommonHelper.GenerateRandomWords(4)，取得随机字母为：edge</example>
        /// <returns>返回随机数字</returns>
        public static string GenerateRandomNum(int iLength)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(10);
            Random rm = new Random();
            for (int i = 0; i < iLength; i++)
            {
                sb.Append(arNumeric[rm.Next(10)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// MD5加密字符串
        /// </summary>
        /// <param name="strContent">字符串</param>
        /// <param name="iCodeNum">可选择16或32，即加密后的密文16位或32位</param>
        /// <returns>返回加密字符串</returns>
        public static string MD5(string strContent, int iCodeNum)
        {
            string strReturnValue = "";

            if (iCodeNum == 16) //16位MD5加密（取32位加密的9~25字符） 
            {
                strReturnValue = Md5Hash(strContent).ToLower().Substring(8, 16);
            }

            if (iCodeNum == 32) //32位加密 
            {
                strReturnValue = Md5Hash(strContent).ToLower();
            }

            return strReturnValue;
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 弹窗消息框，加载当前页面
        /// </summary>
        /// <param name="strPopupMessage">弹窗消息内容</param>
        public static void ReloadPage(string strPopupMessage)
        {
            HttpContext.Current.Response.Write("<script language=javascript>alert('" + strPopupMessage + "');</script> ");
        }

        /// <summary>
        /// 重定向到当前页面，相当于重新打开本页面
        /// </summary>
        /// <param name="strPopupMessage">弹窗消息内容</param>
        public static void ReloadCurrentPage(string strPopupMessage)
        {
            string strUrl = HttpContext.Current.Request.Url.ToString();
            strUrl = strUrl.Replace("'", " ");
            HttpContext.Current.Response.Write("<script language=javascript>alert('" + strPopupMessage + "');window.window.location.href='" + strUrl + "';</script>");
        }

        /// <summary>
        /// 重定向一个指定的页面
        /// </summary>
        /// <param name="strPopupMessage">弹窗消息内容</param>
        /// <param name="strSpecialPageUrl">指定页面地址</param>
        public static void ReloadCurrentPage(string strPopupMessage, string strSpecialPageUrl)
        {
            HttpContext.Current.Response.Write(" <script language=javascript>alert('" + strPopupMessage + "');window.window.location.href='" + strSpecialPageUrl + "';</script> ");
        }

        /// <summary>
        /// 获取当前是星期几（汉字）
        /// </summary>
        /// <returns></returns>
        public static string GetWeekName()
        {
            //获取当前日期是星期几
            string dt = DateTime.Today.DayOfWeek.ToString();
            //根据取得的星期英文单词返回汉字
            string strWeekName = "";
            switch (dt)
            {
                case "Monday":
                    strWeekName = "星期一";
                    break;

                case "Tuesday":
                    strWeekName = "星期二";
                    break;

                case "Wednesday":
                    strWeekName = "星期三";
                    break;

                case "Thu;rsday":
                    strWeekName = "星期四";
                    break;

                case "Friday":
                    strWeekName = "星期五";
                    break;

                case "Saturday":
                    strWeekName = "星期六";
                    break;

                case "Sunday":
                    strWeekName = "星期日";
                    break;

            }

            return strWeekName;
        }

        /// <summary>
        /// 存储Cookie值
        /// </summary>
        /// <param name="strKey">Cookie名称</param>
        /// <param name="strValue">Cookie值</param>
        /// <param name="iExpiresDays">过期时间，天为单位</param>
        public static void SaveCookie(string strKey, string strValue, int iExpiresDays)
        {
            DateTime dt = DateTime.Now.AddDays(iExpiresDays);
            HttpCookie hc = new HttpCookie(strKey);
            //hc.Domain = ".yourdomain.com";
            hc.Value = HttpUtility.UrlEncode(strValue);
            hc.Expires = dt;
            HttpContext.Current.Response.Cookies.Add(hc);
        }


        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="strCookieName">Cookie名</param>
        /// <param name="strCookieValue">Cookie值</param>
        /// <param name="iDayNum">Cookie保存天数，以天为单位</param>
        public static void SetCookie(string strCookieName, string strCookieValue, int iDayNum)
        {
            HttpCookie cookie = new HttpCookie(strCookieName);
            cookie.Value = HttpContext.Current.Server.UrlEncode(strCookieValue);
            //cookie.Domain = ".yourdomain.com";
            DateTime dt = DateTime.Now;
            cookie.Expires = dt.AddDays(iDayNum);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }


        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="strCookieName">Cookie名</param>
        /// <param name="bDecode">是否解密</param>
        /// <returns>返回Cookie值</returns>
        public static String GetCookieString(string strCookieName, bool bDecode)
        {
            if (HttpContext.Current.Request.Cookies[strCookieName] == null)
            {
                return String.Empty;
            }
            try
            {
                string strCookieValue = HttpContext.Current.Request.Cookies[strCookieName].Value.ToString();
                if (bDecode)
                {
                    strCookieValue = HttpContext.Current.Server.UrlDecode(strCookieValue);
                }
                return strCookieValue;
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 从Cookie中获取整型值
        /// </summary>
        /// <param name="strCookieName">Cookie名</param>
        /// <returns>返回整型值</returns>
        public static int GetCookieInt(string strCookieName)
        {
            string strCookieValue= GetCookieString(strCookieName, true);
            if (!string.IsNullOrEmpty(strCookieValue))
            {
                return Convert.ToInt32(strCookieValue);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取Cookie ID
        /// </summary>
        /// <param name="strCookieName">Cookie名</param>
        /// <returns>返回Cookie ID</returns>
        public static string GetCookid(string strCookieName)
        {
            Encoding eg = Encoding.GetEncoding("UTF-8");
            return HttpUtility.UrlDecode(strCookieName, eg);
        }

        /// <summary>
        /// 显示Flash
        /// </summary>
        /// <param name="strSWFURL">SWF文件地址</param>
        /// <param name="iWidth">显示宽度</param>
        /// <param name="iHeight">显示高度</param>
        /// <returns></returns>
        public static string ShowFlash(string strSWFURL, int iWidth, int iHeight)
        {
            string strHtmlString = "<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0\" width=\"" + iWidth + "px\" height=\"" + iHeight + "px\"  id=\"Flash\" align=\"middle\">";
            strHtmlString += "<param name=\"allowScriptAccess\" value=\"sameDomain\" />";
            strHtmlString += "<param name=\"allowFullScreen\" value=\"false\" />";
            strHtmlString += "<param name=\"movie\" value=\"" + strSWFURL + "\" />";
            strHtmlString += "<param name=\"quality\" value=\"high\" />";
            strHtmlString += "<param name=wmode value=transparent>";
            strHtmlString += "<embed src=\"" + strSWFURL + "\" quality=\"high\" width=\"" + iWidth + "px\" height=\"" + iHeight + "px\" align=\"middle\" allowScriptAccess=\"sameDomain\" allowFullScreen=\"false\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" />";
            strHtmlString += "</object>";

            return strHtmlString;
        }

        /// <summary>
        /// 隐藏IP地址最后一位用*号代替
        /// </summary>
        /// <param name="strIpAddress">IP地址如：192.168.1.1</param>
        /// <returns>返回192.168.1.*</returns>
        public static string HidenLastIp(string strIpAddress)
        {
            return strIpAddress.Substring(0, strIpAddress.LastIndexOf(".")) + ".*";
        }

        /// <summary>
        /// 格式化显示价格数据，如：1000000.00 格式化后为：1,000,000.00
        /// </summary>
        /// <param name="strPrice">价格</param>
        /// <returns>返回格式化数据</returns>
        public static string FormatPriceViewData(string strPrice)
        {
            string strReturnValue = "";
            string strNewPrice = strPrice;
            if (!strPrice.Contains("."))
            {
                strPrice = strPrice + ".00";
            }

            if (strPrice.Contains("-"))
            {
                strPrice = strPrice.Replace("-", "");
            }

            string[] splitPrice = strPrice.Split(new char[] { '.' });
            string strFormatPrice = splitPrice[0].ToString();
            int nLength = strFormatPrice.Length;
            string strNewFormatPrice = "";
            switch (nLength)
            {
                case 3: strNewFormatPrice = strFormatPrice;
                    break;
                case 4: strNewFormatPrice = strFormatPrice.Insert(1, ",");
                    break;
                case 5: strNewFormatPrice = strFormatPrice.Insert(2, ",");
                    break;
                case 6: strNewFormatPrice = strFormatPrice.Insert(3, ",");
                    break;
                case 7:
                    strNewFormatPrice = strFormatPrice.Insert(1, ",");
                    strNewFormatPrice = strNewFormatPrice.Insert(5, ",");
                    break;
                case 8:
                    //10000000.00 -> 10,000,000.00
                    strNewFormatPrice = strFormatPrice.Insert(2, ",");
                    strNewFormatPrice = strNewFormatPrice.Insert(6, ",");
                    break;
                case 9:
                    //100000000.00 -> 100,000,000.00
                    strNewFormatPrice = strFormatPrice.Insert(3, ",");
                    strNewFormatPrice = strNewFormatPrice.Insert(7, ",");
                    break;
                case 10:
                    //1000000000.00 -> 1,000,000,000.00
                    strNewFormatPrice = strFormatPrice.Insert(1, ",");
                    strNewFormatPrice = strNewFormatPrice.Insert(5, ",");
                    strNewFormatPrice = strNewFormatPrice.Insert(9, ",");
                    break;
                default:
                    strNewFormatPrice = strFormatPrice;
                    break;

            }

            strReturnValue = strNewFormatPrice + ".00";
            if (strNewPrice.Contains("-"))
            {
                strReturnValue = "-" + strReturnValue;
            }

            return strReturnValue;
        }

        /// <summary>
        /// SQL注入过滤
        /// </summary>
        /// <param name="strInText">要过滤的字符串</param>
        /// <returns>如果参数存在不安全字符，则返回true</returns>
        public static bool SqlFilter2(string strInText)
        {
            string strWord = "and|exec|insert|select|delete|update|chr|mid|master|or|truncate|char|declare|join";
            if (strInText == null)
                return false;
            foreach (string i in strWord.Split('|'))
            {
                if ((strInText.ToLower().IndexOf(i + " ") > -1) || (strInText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 截取内容中的图片
        /// </summary>
        /// <param name="content">文章内容</param>
        /// <returns></returns>
        public static string GetContentImage(string content)
        {
            Regex reg = new Regex(@"img[^>]*?src=""?(?<url>[a-z][^\s""]*)""?", RegexOptions.IgnoreCase);

            Match mr = reg.Match(content);
            string url = "";

            url = mr.Groups[1].ToString();

            return url;
        }

        /// <summary>
        /// 指定内容获取指定的长度，超过指定长度用“..”代替
        /// 通常用于显示文章最新几条，指定了文章字符长度。
        /// </summary>
        /// <param name="content">文章标题</param>
        /// <param name="len">指定长度</param>
        /// <param name="adddot">是否需要用..来代替超长的字符，如果需要True，不需要False</param>
        /// <returns>返回处理后的字符串</returns>
        public static string LimitString(object content, int len, bool adddot)
        {
            string originalNews = content.ToString().Trim();
            System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
            byte[] newsBytes = encoding.GetBytes(originalNews);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            int narrowChars = 0;
            int widthChars = 0;
            for (int i = 1; i < newsBytes.Length; i += 2)
            {
                if (widthChars == len)
                    break;
                byte[] temp = new byte[] { newsBytes[i - 1], newsBytes[i] };
                sb.Append(System.Text.Encoding.Unicode.GetString(temp));
                if ((int)newsBytes[i] == 0)
                {
                    narrowChars++;
                    if (narrowChars == 2)
                    {
                        narrowChars = 0;
                        widthChars++;
                    }
                }
                else
                {
                    widthChars++;
                }
            }
            if (adddot && sb.ToString() != content.ToString())
            {
                return sb.ToString() + "..";
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取表情图片，用于文字间
        /// </summary>
        /// <returns>返回表情图片</returns>
        public static string GetFacePics()
        {
            string[] arrPics = { "[心]", "[太阳]", "[花心]", "[good]", "[ok]", "[来]", "[给力]", "[围观]", "[粉红丝带]", "[爱心传递]", "[鲜花]", "[花]", "[耶]", "[跳舞花]", "[冒个泡]" };
            Random rm = new Random();
            return arrPics[rm.Next(1, 15)].ToString();
        }

        /// <summary>
        /// 获取表情图片，用于句尾
        /// </summary>
        /// <returns>返回表情图片</returns>
        public static string GetFacePicsEnd()
        {
            string[] arrPics = { "[心]", "[太阳]", "[花心]", "[good]", "[ok]", "[来]", "[给力]", "[围观]", "[爱心传递]", "[鲜花]", "[花]", "[耶]", "[跳舞花]" };
            Random rm = new Random();
            return arrPics[rm.Next(1, 13)].ToString();
        }


        /// <summary>
        /// 获取魔法表情，用于微博句末
        /// </summary>
        /// <returns>返回魔法表情</returns>
        public static string GetMagicFace()
        {
            string[] arrPics = { "http://t.cn/GZ7Sw", "http://t.cn/GZPZB", "http://t.cn/bR6v5", "http://t.cn/bR56p", "http://t.cn/GZv51", "http://t.cn/GZzk2 ", "http://t.cn/GU3Ok", "http://t.cn/GZzrp" };
            Random rm = new Random();
            return arrPics[rm.Next(1, 9)].ToString();
        }

        /// <summary>
        /// 清除IE缓存
        /// </summary>
        public static void ClearIECache()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c " + "del /f /s /q \"%userprofile%\\Local Settings\\Temporary Internet Files\\*.*\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// 清除IE Cookie
        /// </summary>
        public static void ClearIECookie()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c " + "del /f /s /q \"%userprofile%\\Cookies\\*.*\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// 关闭IE窗口
        /// </summary>
        public static void KillIEWindows()
        {
            Process[] process = Process.GetProcesses();
            for (int i = 0; i < process.Length; i++)
            {
                if (process[i].ProcessName.ToLower() == "iexplore")
                {
                    try
                    {
                        process[i].Kill();
                        Thread.Sleep(500);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 用于文章显示页面，处理抓取过来文章中多余的字符串元素
        /// </summary>
        /// <param name="strContent">文章内容</param>
        public static string ClearSpecialStringForArticle(string strContent)
        {
            strContent = strContent.Replace(".hx_hidden_tag { display:none; }", "");

            return strContent;
        }

        /// <summary>
        /// 将系统时间转换成UNIX时间戳
        /// </summary>
        /// <returns></returns>
        public static string DateTimeToUnixTimeStamp()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);

            return timeStamp;
        }

        public static string DateTimeToUnixTimeStamp(string strDate)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(strDate);
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);

            return timeStamp;
        }

        /// <summary>
        /// 将系统时间转换成UNIX时间戳
        /// </summary>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(string strTimeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(strTimeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);

            return dtResult;
        }

        #region 防止sql注入式攻击(可用于UI层控制）

        /// 
        /// 判断字符串中是否有SQL攻击代码，by fangbo.yu 2008.07.18
        /// 
        /// 传入用户提交数据
        /// true-安全；false-有注入攻击现有；
        public static bool ProcessSqlStr(string inputString)
        {
            string SqlStr = @"and|or|exec|execute|insert|select|delete|update|alter|create|drop|count|\*|chr|char|asc|mid|substring|master|truncate|declare|xp_cmdshell|restore|backup|net +user|net +localgroup +administrators";
            try
            {
                if ((inputString != null) && (inputString != String.Empty))
                {
                    string str_Regex = @"\b(" + SqlStr + @")\b";

                    Regex Regex = new Regex(str_Regex, RegexOptions.IgnoreCase);
                    if (true == Regex.IsMatch(inputString))
                        return false;

                }
            }
            catch
            {
                return false;
            }
            return true;
        }


        /// 
        /// 处理用户提交的请求，校验sql注入式攻击,在页面装置时候运行
        /// System.Configuration.ConfigurationSettings.AppSettings["ErrorPage"].ToString(); 为用户自定义错误页面提示地址,
        /// 在Web.Config文件时里面添加一个 ErrorPage 即可
        /// 
        ///     
        /// 
        public static void ProcessRequest()
        {
            try
            {
                string getkeys = "";
                string sqlErrorPage = System.Configuration.ConfigurationManager.AppSettings["ErrorPage"].ToString();
                if (System.Web.HttpContext.Current.Request.QueryString != null)
                {

                    for (int i = 0; i < System.Web.HttpContext.Current.Request.QueryString.Count; i++)
                    {
                        getkeys = System.Web.HttpContext.Current.Request.QueryString.Keys[i];
                        if (!ProcessSqlStr(System.Web.HttpContext.Current.Request.QueryString[getkeys]))
                        {
                            System.Web.HttpContext.Current.Response.Redirect(sqlErrorPage + "?errmsg=" + getkeys + "有SQL攻击嫌疑！");
                            System.Web.HttpContext.Current.Response.End();
                        }
                    }
                }
                if (System.Web.HttpContext.Current.Request.Form != null)
                {
                    for (int i = 0; i < System.Web.HttpContext.Current.Request.Form.Count; i++)
                    {
                        getkeys = System.Web.HttpContext.Current.Request.Form.Keys[i];
                        if (!ProcessSqlStr(System.Web.HttpContext.Current.Request.Form[getkeys]))
                        {
                            System.Web.HttpContext.Current.Response.Redirect(sqlErrorPage + "?errmsg=" + getkeys + "有SQL攻击嫌疑！");
                            System.Web.HttpContext.Current.Response.End();
                        }
                    }
                }
            }
            catch
            {
                // 错误处理: 处理用户提交信息!
            }
        }
        #endregion

        #region 转换sql代码（也防止sql注入式攻击，可以用于业务逻辑层，但要求UI层输入数据时候进行解码）
        /// 
        /// 提取字符固定长度，by fangbo.yu 2008.07.18
        /// 
        /// 
        /// 
        /// 
        public static string CheckStringLength(string inputString, Int32 maxLength)
        {
            if ((inputString != null) && (inputString != String.Empty))
            {
                inputString = inputString.Trim();

                if (inputString.Length > maxLength)
                    inputString = inputString.Substring(0, maxLength);
            }
            return inputString;
        }

        /// 
        /// 将输入字符串中的sql敏感字，替换成"[敏感字]"，要求输出时，替换回来，by fangbo.yu 2008.07.21
        /// 
        /// 
        /// 
        public static string MyEncodeInputString(string inputString)
        {
            //要替换的敏感字
            string SqlStr = @"and|or|exec|execute|insert|select|delete|update|alter|create|drop|count|\*|chr|char|asc|mid|substring|master|truncate|declare|xp_cmdshell|restore|backup|net +user|net +localgroup +administrators";
            try
            {
                if ((inputString != null) && (inputString != String.Empty))
                {
                    string str_Regex = @"\b(" + SqlStr + @")\b";

                    Regex Regex = new Regex(str_Regex, RegexOptions.IgnoreCase);
                    //string s = Regex.Match(inputString).Value; 
                    MatchCollection matches = Regex.Matches(inputString);
                    for (int i = 0; i < matches.Count; i++)
                        inputString = inputString.Replace(matches[i].Value, "[" + matches[i].Value + "]");

                }
            }
            catch
            {
                return "";
            }
            return inputString;

        }

        /// 
        /// 将已经替换成的"[敏感字]"，转换回来为"敏感字"，by fangbo.yu 2008.07.21
        /// 
        /// 
        /// 
        public static string MyDecodeOutputString(string outputstring)
        {
            //要替换的敏感字
            string SqlStr = @"and|or|exec|execute|insert|select|delete|update|alter|create|drop|count|\*|chr|char|asc|mid|substring|master|truncate|declare|xp_cmdshell|restore|backup|net +user|net +localgroup +administrators";
            try
            {
                if ((outputstring != null) && (outputstring != String.Empty))
                {
                    string str_Regex = @"\[\b(" + SqlStr + @")\b\]";
                    Regex Regex = new Regex(str_Regex, RegexOptions.IgnoreCase);
                    MatchCollection matches = Regex.Matches(outputstring);
                    for (int i = 0; i < matches.Count; i++)
                        outputstring = outputstring.Replace(matches[i].Value, matches[i].Value.Substring(1, matches[i].Value.Length - 2));

                }
            }
            catch
            {
                return "";
            }
            return outputstring;
        }
        #endregion


        /// <summary>
        /// 返回格式化的日期
        /// </summary>
        /// <param name="strDate1">目标日期</param>
        /// <param name="strDate2">当前日期</param>
        /// <returns></returns>
        public static string FormatDate(string strDate1, string strDate2)
        {
            string strDate = "";

            DateTime dt1 = DateTime.Parse(strDate1);
            DateTime dt2 = DateTime.Parse(strDate2);
            TimeSpan ts = dt2.Subtract(dt1);
            // strDate = ts.Minutes +","+ ts.Hours + "," + ts.Days ;

            if (ts.Days == 0 && ts.Hours == 0 && ts.Minutes == 0)
            {
                strDate = "刚刚";
            }
            else if (ts.Days == 0 && ts.Hours == 0)
            {
                strDate = ts.Minutes + "分钟前";
            }
            else if (ts.Days == 0 && ts.Hours > 0)
            {
                strDate = ts.Hours + "小时前";
            }
            else if (ts.Days > 0 && ts.Days == 1)
            {
                strDate = "昨天";
            }
            else if (ts.Days > 0 && ts.Days == 2)
            {
                strDate = "前天";
            }
            else
            {
                strDate = Convert.ToDateTime(strDate1).ToString("M月d日");
            }

            return strDate;
        }

        public static string GetEmojiICON(string strContent)
        {
            strContent = strContent.Replace("[微笑]", "<img src=\"http://img.fotuozi.cn/emoji/huanglianwx_thumb.gif\">");
            strContent = strContent.Replace("[蜡烛]", "<img src=\"http://img.fotuozi.cn/emoji/lazhuv2_thumb.gif\">");
            strContent = strContent.Replace("[嘻嘻]", "<img src=\"http://img.fotuozi.cn/emoji/tootha_thumb.gif\">");
            strContent = strContent.Replace("[哈哈]", "<img src=\"http://img.fotuozi.cn/emoji/laugh.gif\">");
            strContent = strContent.Replace("[可爱]", "<img src=\"http://img.fotuozi.cn/emoji/tza_thumb.gif\">");
            strContent = strContent.Replace("[祈祷]", "<img src=\"http://img.fotuozi.cn/emoji/bless_org.png\">");
            strContent = strContent.Replace("[鲜花]", "<img src=\"http://img.fotuozi.cn/emoji/flower_org.gif\">");
            strContent = strContent.Replace("[玫瑰]", "<img src=\"http://img.fotuozi.cn/emoji/flower_org.gif\">");
            strContent = strContent.Replace("[威武]", "<img src=\"http://img.fotuozi.cn/emoji/vw_thumb.gif\">");
            strContent = strContent.Replace("[good]", "<img src=\"http://img.fotuozi.cn/emoji/good_thumb.gif\">");
            strContent = strContent.Replace("[心]", "<img src=\"http://img.fotuozi.cn/emoji/hearta_thumb.gif\">");
            strContent = strContent.Replace("[伤心]", "<img src=\"http://img.fotuozi.cn/emoji/unheart.gif\">");
            strContent = strContent.Replace("[给力]", "<img src=\"http://img.fotuozi.cn/emoji/geiliv2_thumb.gif\">");
            strContent = strContent.Replace("[赞]", "<img src=\"http://img.fotuozi.cn/emoji/z2_thumb.gif\">");
            strContent = strContent.Replace("[太开心]", "<img src=\"http://img.fotuozi.cn/emoji/mb_thumb.gif\">");
            strContent = strContent.Replace("[害羞]", "<img src=\"http://img.fotuozi.cn/emoji/shamea_thumb.gif\">");
            strContent = strContent.Replace("[浮云]", "<img src=\"http://img.fotuozi.cn/emoji/fuyun_thumb.gif\">");
            strContent = strContent.Replace("[神马]", "<img src=\"http://img.fotuozi.cn/emoji/horse2_thumb.gif\">");
            strContent = strContent.Replace("[挖鼻]", "<img src=\"http://img.fotuozi.cn/emoji/wabi_thumb.gif\">");

            strContent = strContent.Replace("[月亮]", "<img src=\"http://img.fotuozi.cn/emoji/moon.gif\">");
            strContent = strContent.Replace("[太阳]", "<img src=\"http://img.fotuozi.cn/emoji/sun.gif\">");
            strContent = strContent.Replace("[微风]", "<img src=\"http://img.fotuozi.cn/emoji/wind_org.gif\">");


            return strContent;
        }
    }     
}
