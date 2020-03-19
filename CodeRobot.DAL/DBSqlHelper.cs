using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using CodeRobot.Utility;
using System.Windows.Forms;

namespace CodeRobot.DAL
{
    public class DBSqlHelper
    {

        /// <summary>
        /// 创建数据库连接文件
        /// </summary>
        /// <param name="strFilePath">文件路径</param>
        /// <param name="strProjectName">项目名称</param>
        public static void CreateDBSqlHelperFile(string strFilePath, string strProjectName)
        {
            try
            {
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(System.Windows.Forms.Application.StartupPath + "\\config.ini");

                string strGetServer = iniFile.GetString("BASE", "SERVER", "");//数据库地址
                string strGetDBName = iniFile.GetString("BASE", "DBNAME", "");//数据库名
                string strGetDBUser = iniFile.GetString("BASE", "USERID", "");//登录数据库用户名
                string strGetDBPassword = iniFile.GetString("BASE", "PASSWORD", "");//登录数据库密码
                string strCharset = iniFile.GetString("BASE", "CHARSET", "");//数据库字符集
                string strFramework = iniFile.GetString("BASE", "FRAMEWORK", "");//.net framework版本号
                string strGUID = iniFile.GetString("GUID", "DBSQLHELPER", "");//项目GUID

                //读取版权信息
                string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
                string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
                string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
                string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
                string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

                //strGetServer = CodeRobot.Utility.CryptographyHelper.Decrypt(strGetServer, CodeRobot.Utility.CommonHelper.cryptographyKey);
                //strGetDBName = CodeRobot.Utility.CryptographyHelper.Decrypt(strGetDBName, CodeRobot.Utility.CommonHelper.cryptographyKey);
                //strGetDBUser = CodeRobot.Utility.CryptographyHelper.Decrypt(strGetDBUser, CodeRobot.Utility.CommonHelper.cryptographyKey);
                //strGetDBPassword = CodeRobot.Utility.CryptographyHelper.Decrypt(strGetDBPassword, CodeRobot.Utility.CommonHelper.cryptographyKey);

                Directory.CreateDirectory(strFilePath);
                
                StreamWriter sw = new StreamWriter(strFilePath + "\\DBMySQLHelper.cs");
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                //sw.WriteLine("using System.Data;");
                //sw.WriteLine("using MySql.Data.MySqlClient;");
                sw.WriteLine("");
                sw.WriteLine("namespace " + CommonHelper.GetTableNameUpper(strProjectName) + ".DBSqlHelper");
                sw.WriteLine("{");
                sw.WriteLine("");
                sw.WriteLine("    /// <summary>");
                sw.WriteLine("    /// 版权所有: Copyright © " + DateTime.Now.Year.ToString() + " "+ strCompany + ". 保留所有权利。");
                sw.WriteLine("    /// 内容摘要: 数据库连接底层封装类");
                sw.WriteLine("    /// 完成日期：" + DateTime.Now.ToString("yyyy年M月d日"));
                sw.WriteLine("    /// 版    本：V" + strVersion + "." + strCode);
                sw.WriteLine("    /// 作    者：" + strAuthor);
                sw.WriteLine("    /// </summary>");
                sw.WriteLine("    public class DBMySQLHelper");
                sw.WriteLine("    {");
                sw.WriteLine("        /// <summary>");
                sw.WriteLine("        /// 连接数据库");
                sw.WriteLine("        /// <summary>");
                sw.WriteLine("        /// <returns>返回数据库连接字符串</returns>");
                sw.WriteLine("        public static string ConnectMySQL()");
                sw.WriteLine("        {");
                sw.WriteLine("            return \"Server="+ strGetServer + "; Database=" + strGetDBName + "; Uid=" + strGetDBUser + "; Pwd=" + strGetDBPassword + "; CharSet=" + strCharset + "\";");
                sw.WriteLine("        }");
                sw.WriteLine("    }");
                sw.WriteLine("}");
                sw.Close();

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建数据库连接文件", "CreateDBSqlHelperFile",false);
            }
        }
    }
}
