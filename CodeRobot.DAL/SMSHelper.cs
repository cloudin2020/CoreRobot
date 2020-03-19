using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CodeRobot.Utility;
using System.Windows.Forms;

namespace CodeRobot.DAL
{
    /// <summary>
    /// 版权所有: Copyright © 2018 Cloudin. 保留所有权利。
    /// 内容摘要: 短信发送封装类
    /// 完成日期：2018年01月15日
    /// 版    本：V1.0 
    /// 作    者：Adin Lee
    public class SMSHelper
    {
        /// <summary>
        /// 根据SMSHelper类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        public static void CreateSMSClass(string strFilePath, string strProjectName)
        {
            //读取版权信息
            CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
            string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
            string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
            string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
            string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
            string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

            Directory.CreateDirectory(strFilePath);
            StreamWriter sw = new StreamWriter(strFilePath + "\\SMSHelper.cs", false, Encoding.GetEncoding("utf-8"));
            sw.WriteLine("using System;");
            sw.WriteLine("using System.Text;");
            sw.WriteLine("using System.IO;");
            sw.WriteLine("using System.Net;");
            sw.WriteLine("");
            sw.WriteLine("namespace " + CommonHelper.GetTableNameUpper(strProjectName) + ".SMS");
            sw.WriteLine("{");
            sw.WriteLine("");
            sw.WriteLine("    /// <summary>");
            sw.WriteLine("    /// 版权所有: Copyright © " + DateTime.Now.Year.ToString() + " " + strCompany + ". 保留所有权利。");
            sw.WriteLine("    /// 内容摘要: 短信发送封装类");
            sw.WriteLine("    /// 完成日期：" + DateTime.Now.ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 版    本：V" + strVersion + "." + strCode + " ");
            sw.WriteLine("    /// 作    者：" + strAuthor);
            sw.WriteLine("    /// </summary>");
            sw.WriteLine("    public class SMSHelper");
            sw.WriteLine("    {");
            sw.WriteLine("        public static string strSMSUID = \"\";");
            sw.WriteLine("        public static string strSMSPassword = \"\";");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 注册发送验证码");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"strMobile\">手机号码</param>");
            sw.WriteLine("        /// <param name=\"strCode\">验证码</param>");
            sw.WriteLine("        /// <returns></returns>");
            sw.WriteLine("        public static string SendSMSForCode(string strMobile, string strCode)");
            sw.WriteLine("        {");
            sw.WriteLine("            string strReturnValue = \"\";");
            sw.WriteLine("            ");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                string sendurl = \"http://api.sms.cn/mt/\";");
            sw.WriteLine("                string strContent = \"验证码：\"+ strCode + \"【乐骑】\";");
            sw.WriteLine("                StringBuilder sbTemp = new StringBuilder();");
            sw.WriteLine("                string strPassword = "+strProjectName+".Utility.CommonHelper.MD5(strSMSPassword + strSMSUID);");
            sw.WriteLine("                sbTemp.Append(\"uid=\" + strSMSUID + \"&pwd=\" + strPassword + \"&mobile=\" + strMobile + \"&content=\" + strContent);");
            sw.WriteLine("                byte[] bTemp = System.Text.Encoding.GetEncoding(\"GBK\").GetBytes(sbTemp.ToString());");
            sw.WriteLine("                strReturnValue = doPostRequest(sendurl, bTemp);");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                strReturnValue = ex.Message;");
            sw.WriteLine("            }");
            sw.WriteLine("            ");
            sw.WriteLine("            return strReturnValue;");
            sw.WriteLine("            ");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// Post方式请求，并返回结果");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"url\">URL地址</param>");
            sw.WriteLine("        /// <param name=\"bData\">提交数据</param>");
            sw.WriteLine("        /// <returns>返回结果</returns>");
            sw.WriteLine("        public static string doPostRequest(string url, byte[] bData)");
            sw.WriteLine("        {");
            sw.WriteLine("            HttpWebRequest hwRequest = null;");
            sw.WriteLine("            HttpWebResponse hwResponse = null;");
            sw.WriteLine("            string strResultString = \"\";");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);");
            sw.WriteLine("                hwRequest.Timeout = 5000;");
            sw.WriteLine("                hwRequest.Method = \"POST\";");
            sw.WriteLine("                hwRequest.ContentType = \"application/x-www-form-urlencoded\";");
            sw.WriteLine("                hwRequest.ContentLength = bData.Length;");
            sw.WriteLine("                System.IO.Stream smWrite = hwRequest.GetRequestStream();");
            sw.WriteLine("                smWrite.Write(bData, 0, bData.Length);");
            sw.WriteLine("                smWrite.Close();");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                "+strProjectName+".Utility.LogHelper.Error(typeof(SMSHelper), ex, \"Post方式请求，并返回结果\", \"doPostRequest\", true);");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                hwResponse = (HttpWebResponse)hwRequest.GetResponse();");
            sw.WriteLine("                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);");
            sw.WriteLine("                strResultString = srReader.ReadToEnd();");
            sw.WriteLine("                srReader.Close();");
            sw.WriteLine("                hwResponse.Close();");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                " + strProjectName + ".Utility.LogHelper.Error(typeof(SMSHelper), ex, \"Post方式请求，并返回结果\", \"doPostRequest\", true);");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            return strResultString;");
            sw.WriteLine("        }");
            sw.WriteLine("    }");
            sw.WriteLine("}");
            sw.Close();

        }

    }
}
