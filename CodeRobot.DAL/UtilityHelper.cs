using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CodeRobot.DAL
{
    /// <summary>
    /// 创建常用工具类
    /// </summary>
    public class UtilityHelper
    {
        /// <summary>
        /// 创建Utility项目下封装类文件
        /// </summary>
        /// <param name="strFilePath">文件路径</param>
        /// <param name="strProjectName">项目名称</param>
        public static void CreateUtilityFile(string strFilePath, string strProjectName)
        {
            //拷贝第三方库文件
            string strTargetDirectory = strFilePath + "\\bin\\Debug";
            if (!Directory.Exists(strTargetDirectory))
            {
                Directory.CreateDirectory(strTargetDirectory);
            }
            File.Copy(Application.StartupPath+ "\\copy\\log4net.dll", strTargetDirectory + "\\log4net.dll", true);
            File.Copy(Application.StartupPath + "\\copy\\log4net.xml", strTargetDirectory + "\\log4net.xml", true);

            //1-生成CommonHelper.cs-------------------------------------------1
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            //创建项目文件
            FileStream fs1 = new FileStream(Application.StartupPath + "\\template\\utility\\Template1_CommonHelper.cs", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);
            string strGetCsPageContent1 = sr1.ReadToEnd();
            sr1.Close();
            fs1.Close();

            strGetCsPageContent1 = strGetCsPageContent1.Replace("{project_name}", strProjectName);

            StreamWriter sw1 = new StreamWriter(strFilePath + "\\CommonHelper.cs", false, Encoding.GetEncoding("utf-8"));
            sw1.WriteLine(strGetCsPageContent1);
            sw1.Close();

            //2-生成CommonHelper.cs-------------------------------------------2
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            //创建项目文件
            FileStream fs2 = new FileStream(Application.StartupPath + "\\template\\utility\\Template2_StringHelper.cs", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr2 = new StreamReader(fs2, Encoding.UTF8);
            string strGetCsPageContent2 = sr2.ReadToEnd();
            sr2.Close();
            fs2.Close();

            strGetCsPageContent2 = strGetCsPageContent2.Replace("{project_name}", strProjectName);

            StreamWriter sw2 = new StreamWriter(strFilePath + "\\StringHelper.cs", false, Encoding.GetEncoding("utf-8"));
            sw2.WriteLine(strGetCsPageContent2);
            sw2.Close();

            //3-生成PublicValue.cs-------------------------------------------3
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            //创建项目文件
            FileStream fs3 = new FileStream(Application.StartupPath + "\\template\\utility\\Template3_PublicValue.cs", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr3 = new StreamReader(fs3, Encoding.UTF8);
            string strGetCsPageContent3 = sr3.ReadToEnd();
            sr3.Close();
            fs3.Close();

            strGetCsPageContent3 = strGetCsPageContent3.Replace("{project_name}", strProjectName);

            StreamWriter sw3 = new StreamWriter(strFilePath + "\\PublicValue.cs", false, Encoding.GetEncoding("utf-8"));
            sw3.WriteLine(strGetCsPageContent3);
            sw3.Close();

            //4-生成LogHelper.cs-------------------------------------------4
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            //创建项目文件
            FileStream fs4 = new FileStream(Application.StartupPath + "\\template\\utility\\Template4_LogHelper.cs", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr4 = new StreamReader(fs4, Encoding.UTF8);
            string strGetCsPageContent4 = sr4.ReadToEnd();
            sr4.Close();
            fs4.Close();

            strGetCsPageContent4 = strGetCsPageContent4.Replace("{project_name}", strProjectName);

            StreamWriter sw4 = new StreamWriter(strFilePath + "\\LogHelper.cs", false, Encoding.GetEncoding("utf-8"));
            sw4.WriteLine(strGetCsPageContent4);
            sw4.Close();

            //5-生成ConfigHelper.cs-------------------------------------------5
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            //创建项目文件
            FileStream fs5 = new FileStream(Application.StartupPath + "\\template\\utility\\Template5_ConfigHelper.cs", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr5 = new StreamReader(fs5, Encoding.UTF8);
            string strGetCsPageContent5 = sr5.ReadToEnd();
            sr5.Close();
            fs5.Close();

            strGetCsPageContent5 = strGetCsPageContent5.Replace("{project_name}", strProjectName);

            StreamWriter sw5 = new StreamWriter(strFilePath + "\\ConfigHelper.cs", false, Encoding.GetEncoding("utf-8"));
            sw5.WriteLine(strGetCsPageContent5);
            sw5.Close();

            //6-生成HttpProc.cs-------------------------------------------6
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            //创建项目文件
            FileStream fs6 = new FileStream(Application.StartupPath + "\\template\\utility\\Template6_HttpProc.cs", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr6 = new StreamReader(fs6, Encoding.UTF8);
            string strGetCsPageContent6 = sr6.ReadToEnd();
            sr6.Close();
            fs6.Close();

            strGetCsPageContent6 = strGetCsPageContent6.Replace("{project_name}", strProjectName);

            StreamWriter sw6 = new StreamWriter(strFilePath + "\\HttpProc.cs", false, Encoding.GetEncoding("utf-8"));
            sw6.WriteLine(strGetCsPageContent6);
            sw6.Close();

            //7-生成JSONHelper.cs-------------------------------------------7
            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            //创建项目文件
            FileStream fs7 = new FileStream(Application.StartupPath + "\\template\\utility\\Template7_JSONHelper.cs", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr7 = new StreamReader(fs7, Encoding.UTF8);
            string strGetCsPageContent7 = sr7.ReadToEnd();
            sr7.Close();
            fs7.Close();

            strGetCsPageContent7 = strGetCsPageContent7.Replace("{project_name}", strProjectName);

            StreamWriter sw7 = new StreamWriter(strFilePath + "\\JSONHelper.cs", false, Encoding.GetEncoding("utf-8"));
            sw7.WriteLine(strGetCsPageContent7);
            sw7.Close();
        }

        /// <summary>
        /// 创建日志记录类-作废
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateLogHelperFile(string strFilePath, string strProjectName)
        {
            

            //生成LogHelper.cs
            Directory.CreateDirectory(strFilePath);
            StreamWriter sw = new StreamWriter(strFilePath + "\\LogHelper.cs", false, Encoding.GetEncoding("utf-8"));
            sw.WriteLine("using System;");
            sw.WriteLine("using log4net;");
            sw.WriteLine("");
            sw.WriteLine("namespace " + CommonHelper.GetTableNameUpper(strProjectName) + ".Utility");
            sw.WriteLine("{");
            sw.WriteLine("");
            sw.WriteLine("    /// <summary>");
            sw.WriteLine("    /// 版权所有: Copyright © " + DateTime.Now.Year.ToString() + " Cloudin. 保留所有权利。");
            sw.WriteLine("    /// 内容摘要: 日志记录类(注意：如果是Winform程序，必须把log4net.config拷贝到Debug目录下)");
            sw.WriteLine("    /// 完成日期：" + DateTime.Now.ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 版    本：V1.0 ");
            sw.WriteLine("    /// 作    者：Cloudin");
            sw.WriteLine("    /// </summary>");
            sw.WriteLine("    public class LogHelper");
            sw.WriteLine("    {");
            sw.WriteLine("");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 记录异常日志");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"t\">通常是类名</param>");
            sw.WriteLine("        /// <param name=\"ex\">异常对象</param>");
            sw.WriteLine("        /// <param name=\"strMessageObject\">通常是方法名或指定记录消息类型</param>");
            sw.WriteLine("        /// <param name=\"strFunctionName\">通常是函数名</param>");
            sw.WriteLine("        /// <param name=\"hasWeb\">是否记录网站日志：true=是，false=否（记录记录应用程序日志）</param>");
            sw.WriteLine("        public static void Error(Type t, Exception ex,string strMessageObject,string strFunctionName,bool hasWeb)");
            sw.WriteLine("        {");
            sw.WriteLine("            string strPath = \"\";");
            sw.WriteLine("            if(hasWeb)");
            sw.WriteLine("            {");
            sw.WriteLine("                strPath = System.Web.HttpContext.Current.Server.MapPath(\"//\") + \"log4net.config\";//WEB");
            sw.WriteLine("            }");
            sw.WriteLine("            else");
            sw.WriteLine("            {");
            sw.WriteLine("                strPath = System.Windows.Forms.Application.StartupPath + \"\\\\log4net.config\";//WinForm");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo((strPath)));");
            sw.WriteLine("");
            sw.WriteLine("            ILog ilog = LogManager.GetLogger(t);");
            sw.WriteLine("            if(ilog.IsErrorEnabled)");
            sw.WriteLine("            {");
            sw.WriteLine("                ilog.Error(strMessageObject, ex);");
            sw.WriteLine("            }");
            sw.WriteLine("        }");
            sw.WriteLine("\r\n");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 记录调试日志");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"t\">通常是类名</param>");
            sw.WriteLine("        /// <param name=\"strMessageObject\">通常是方法名或指定记录消息类型</param>");
            sw.WriteLine("        /// <param name=\"strFunctionName\">通常是函数名</param>");
            sw.WriteLine("        /// <param name=\"hasWeb\">是否记录网站日志：true=是，false=否（记录记录应用程序日志）</param>");
            sw.WriteLine("        public static void Debug(Type t, string strMessageObject, string strFunctionName,bool hasWeb)");
            sw.WriteLine("        {");
            sw.WriteLine("            string strPath = \"\";");
            sw.WriteLine("            if(hasWeb)");
            sw.WriteLine("            {");
            sw.WriteLine("                strPath = System.Web.HttpContext.Current.Server.MapPath(\"//\") + \"log4net.config\";//WEB");
            sw.WriteLine("            }");
            sw.WriteLine("            else");
            sw.WriteLine("            {");
            sw.WriteLine("                strPath = System.Windows.Forms.Application.StartupPath + \"\\\\log4net.config\";//WinForm");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo((strPath)));");
            sw.WriteLine("");
            sw.WriteLine("            ILog ilog = LogManager.GetLogger(t);");
            sw.WriteLine("            if(ilog.IsDebugEnabled)");
            sw.WriteLine("            {");
            sw.WriteLine("                ilog.Debug(strMessageObject);");
            sw.WriteLine("            }");
            sw.WriteLine("        }");
            sw.WriteLine("\r\n");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 记录信息日志");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"t\">通常是类名</param>");
            sw.WriteLine("        /// <param name=\"strMessageObject\">通常是方法名或指定记录消息类型</param>");
            sw.WriteLine("        /// <param name=\"strFunctionName\">通常是函数名</param>");
            sw.WriteLine("        /// <param name=\"hasWeb\">是否记录网站日志：true=是，false=否（记录记录应用程序日志）</param>");
            sw.WriteLine("        public static void Info(Type t, string strMessageObject, string strFunctionName,bool hasWeb)");
            sw.WriteLine("        {");
            sw.WriteLine("            string strPath = \"\";");
            sw.WriteLine("            if(hasWeb)");
            sw.WriteLine("            {");
            sw.WriteLine("                strPath = System.Web.HttpContext.Current.Server.MapPath(\"//\") + \"log4net.config\";//WEB");
            sw.WriteLine("            }");
            sw.WriteLine("            else");
            sw.WriteLine("            {");
            sw.WriteLine("                strPath = System.Windows.Forms.Application.StartupPath + \"\\\\log4net.config\";//WinForm");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo((strPath)));");
            sw.WriteLine("");
            sw.WriteLine("            ILog ilog = LogManager.GetLogger(t);");
            sw.WriteLine("            if(ilog.IsInfoEnabled)");
            sw.WriteLine("            {");
            sw.WriteLine("                ilog.Info(strMessageObject);");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("        }");
            sw.WriteLine("\r\n");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 记录警告日志");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"t\">通常是类名</param>");
            sw.WriteLine("        /// <param name=\"strMessageObject\">通常是方法名或指定记录消息类型</param>");
            sw.WriteLine("        /// <param name=\"strFunctionName\">通常是函数名</param>");
            sw.WriteLine("        /// <param name=\"hasWeb\">是否记录网站日志：true=是，false=否（记录记录应用程序日志）</param>");
            sw.WriteLine("        public static void Warn(Type t, string strMessageObject, string strFunctionName,bool hasWeb)");
            sw.WriteLine("        {");
            sw.WriteLine("            string strPath = \"\";");
            sw.WriteLine("            if(hasWeb)");
            sw.WriteLine("            {");
            sw.WriteLine("                strPath = System.Web.HttpContext.Current.Server.MapPath(\"//\") + \"log4net.config\";//WEB");
            sw.WriteLine("            }");
            sw.WriteLine("            else");
            sw.WriteLine("            {");
            sw.WriteLine("                strPath = System.Windows.Forms.Application.StartupPath + \"\\\\log4net.config\";//WinForm");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo((strPath)));");
            sw.WriteLine("");
            sw.WriteLine("            ILog ilog = LogManager.GetLogger(t);");
            sw.WriteLine("            if(ilog.IsWarnEnabled)");
            sw.WriteLine("            {");
            sw.WriteLine("                ilog.Warn(strMessageObject);");
            sw.WriteLine("            }");
            sw.WriteLine("        }");
            sw.WriteLine("\r\n");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 记录严重错误日志");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"t\">通常是类名</param>");
            sw.WriteLine("        /// <param name=\"strMessageObject\">通常是方法名或指定记录消息类型</param>");
            sw.WriteLine("        /// <param name=\"strFunctionName\">通常是函数名</param>");
            sw.WriteLine("        /// <param name=\"hasWeb\">是否记录网站日志：true=是，false=否（记录记录应用程序日志）</param>");
            sw.WriteLine("        public static void Fatal(Type t, string strMessageObject, string strFunctionName,bool hasWeb)");
            sw.WriteLine("        {");
            sw.WriteLine("            string strPath = \"\";");
            sw.WriteLine("            if(hasWeb)");
            sw.WriteLine("            {");
            sw.WriteLine("                strPath = System.Web.HttpContext.Current.Server.MapPath(\"//\") + \"log4net.config\";//WEB");
            sw.WriteLine("            }");
            sw.WriteLine("            else");
            sw.WriteLine("            {");
            sw.WriteLine("                strPath = System.Windows.Forms.Application.StartupPath + \"\\\\log4net.config\";//WinForm");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo((strPath)));");
            sw.WriteLine("");
            sw.WriteLine("            ILog ilog = LogManager.GetLogger(t);");
            sw.WriteLine("            if(ilog.IsFatalEnabled)");
            sw.WriteLine("            {");
            sw.WriteLine("                ilog.Fatal(strMessageObject);");
            sw.WriteLine("            }");
            sw.WriteLine("        }");
            sw.WriteLine("\r\n");
            sw.WriteLine("    }");
            sw.WriteLine("}");
            sw.Close();

        }


    }
}
