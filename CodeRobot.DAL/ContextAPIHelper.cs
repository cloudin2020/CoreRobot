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
    /// <summary>
    /// 数据库上下文类
    /// </summary>
    public class ContextAPIHelper
    {

        /// <summary>
        /// 根据表名创建Context类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateContextClass(string strFilePath,string strProjectName, string strContext)
        {
            //读取版权信息
            CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
            string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
            string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
            string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
            string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
            string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");


            Directory.CreateDirectory(strFilePath);
            StreamWriter sw = new StreamWriter(strFilePath + "\\" + strProjectName + "ApiContext.cs", false, Encoding.GetEncoding("utf-8"));
            sw.WriteLine("using System;");
            sw.WriteLine("using Microsoft.EntityFrameworkCore;");
            sw.WriteLine("");
            sw.WriteLine("namespace " + CommonHelper.GetTableNameUpper(strProjectName) + ".API.Data");
            sw.WriteLine("{");
            sw.WriteLine("");
            sw.WriteLine("    /// <summary>");
            sw.WriteLine("    /// 版权所有: Copyright © " + DateTime.Now.Year.ToString() + " "+strCompany+". 保留所有权利。");
            sw.WriteLine("    /// 内容摘要: " + strProjectName + "ApiContext 数据库上下文类,EF Core ");
            sw.WriteLine("    /// 创建日期：" + Convert.ToDateTime(strCreateDate).ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 更新日期：" + DateTime.Now.ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 版    本：V" + strVersion + "." + strCode + " ");
            sw.WriteLine("    /// 作    者：" + strAuthor);
            sw.WriteLine("    /// </summary>");
            sw.WriteLine("    public class " + strProjectName + "ApiContext : DbContext");
            sw.WriteLine("    {");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 数据库上下文");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        public " + strProjectName + "ApiContext(DbContextOptions<" + strProjectName + "ApiContext> options): base(options)");
            sw.WriteLine("        {");
            sw.WriteLine("        }");
            sw.WriteLine("        ");
            sw.WriteLine(strContext);
            sw.WriteLine("    }");
            sw.WriteLine("}");
            sw.Close();

        }

    }
}
