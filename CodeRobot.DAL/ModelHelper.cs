using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace CodeRobot.DAL
{
    public class ModelHelper
    {


        /// <summary>
        /// 根据表名生成实体类-Web或H5
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateModelClass(string strFilePath, string strProjectName, string strTableName, string strFunctionList, string strColumnComment)
        {
            //读取版权信息
            CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
            string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
            string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
            string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
            string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
            string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");
            string strFramework = iniFile.GetString("BASE", "FRAMEWORK", "");//.net framework版本号
            string strGUID = iniFile.GetString("GUID", "DBSQLHELPER", "");//项目GUID

            string strClassName = CommonHelper.GetClassName(strTableName);//类名
            strClassName = CommonHelper.GetTableNameUpper(strClassName);

            Directory.CreateDirectory(strFilePath);
            StreamWriter sw = new StreamWriter(strFilePath + "\\" + strClassName + ".cs", false, Encoding.GetEncoding("utf-8"));
            sw.WriteLine("using System;");
            sw.WriteLine("using System.ComponentModel.DataAnnotations;");
            sw.WriteLine("");
            sw.WriteLine("namespace " + CommonHelper.GetTableNameUpper(strProjectName) + ".Models");
            sw.WriteLine("{");
            sw.WriteLine("");
            sw.WriteLine("    /// <summary>");
            sw.WriteLine("    /// 版权所有: Copyright © " + DateTime.Now.Year.ToString() + " " + strCompany + ". 保留所有权利。");
            sw.WriteLine("    /// 内容摘要: " + strColumnComment + " " + strTableName + " 实体类");
            sw.WriteLine("    /// 创建日期：" + Convert.ToDateTime(strCreateDate).ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 更新日期：" + DateTime.Now.ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 版    本：V" + strVersion + "." + strCode + " ");
            sw.WriteLine("    /// 作    者：" + strAuthor);
            sw.WriteLine("    /// </summary>");
            sw.WriteLine("    public class " + strClassName);
            sw.WriteLine("    {");
            sw.WriteLine(strFunctionList);
            sw.WriteLine("    }");
            sw.WriteLine("}");
            sw.Close();

        }

    }
}
