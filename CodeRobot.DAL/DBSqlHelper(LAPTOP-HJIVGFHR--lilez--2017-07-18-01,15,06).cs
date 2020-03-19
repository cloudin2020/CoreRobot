using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using CodeRobot.Utility;

namespace CodeRobot.DAL
{
    public class DBSqlHelper
    {
        /// <summary>
        /// 根据数据库连接类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateDBSqlHelperFile(string strFilePath, string strProjectName)
        {
            try
            {
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(System.Windows.Forms.Application.StartupPath + "\\config.ini");

                string strServer = iniFile.GetString("BASE", "SERVER", "");//数据库地址
                string strDBName = iniFile.GetString("BASE", "DBNAME", "");//数据库名
                string strUserID = iniFile.GetString("BASE", "USERID", "");//登录数据库用户名
                string strPassword = iniFile.GetString("BASE", "PASSWORD", "");//登录数据库密码
                string strCharset = iniFile.GetString("BASE", "CHARSET", "");//数据库字符集

                Directory.CreateDirectory(strFilePath);
                StreamWriter sw = new StreamWriter(strFilePath + "\\DBMySQLHelper.cs");
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                //sw.WriteLine("using System.Data;");
                //sw.WriteLine("using MySql.Data.MySqlClient;");
                sw.WriteLine("");
                sw.WriteLine("namespace " + CodeRobot.Utility.StringHelper.GetTableNameUpper(strProjectName) + ".DBSqlHelper");
                sw.WriteLine("{");
                sw.WriteLine("");
                sw.WriteLine("    /// <summary>");
                sw.WriteLine("    /// 版权所有: Copyright © " + DateTime.Now.Year.ToString() + " Cloudin. 保留所有权利。");
                sw.WriteLine("    /// 内容摘要: DBMySQLHelper");
                sw.WriteLine("    /// 完成日期：" + DateTime.Now.ToString("yyyy年M月d日"));
                sw.WriteLine("    /// 版    本：V1.0 ");
                sw.WriteLine("    /// 作    者：Adin.Lee");
                sw.WriteLine("    /// </summary>");
                sw.WriteLine("    public class DBMySQLHelper");
                sw.WriteLine("    {");
                sw.WriteLine("        /// <summary>");
                sw.WriteLine("        /// 连接数据库");
                sw.WriteLine("        /// <summary>");
                sw.WriteLine("        /// <returns>返回数据库连接字符串</returns>");
                sw.WriteLine("        public static string ConnectMySQL()");
                sw.WriteLine("        {");
                sw.WriteLine("            return \"Server="+ strServer + "; Database=" + strDBName + "; Uid=" + strUserID + "; Pwd=" + strPassword + "; CharSet=" + strCharset + "\";");
                sw.WriteLine("        }");
                sw.WriteLine("    }");
                sw.WriteLine("}");
                sw.Close();


            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建BLL README文件", "CreateBLLFile");
            }
        }
    }
}
