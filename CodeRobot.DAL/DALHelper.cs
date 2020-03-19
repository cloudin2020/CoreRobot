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
    /// 数据处理类
    /// </summary>
    public class DALHelper
    {

        /// <summary>
        /// 根据表名创建DAL类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateDALClass(string strFilePath,string strProjectName, string strTableName,string strColumnComment)
        {
            //拷贝第三方库文件
            string strTargetDirectory = strFilePath + "\\bin\\Debug";
            if (!Directory.Exists(strTargetDirectory))
            {
                Directory.CreateDirectory(strTargetDirectory);
            }
            File.Copy(Application.StartupPath + "\\copy\\MySql.Data.dll", strTargetDirectory + "\\MySql.Data.dll", true);


            //读取版权信息
            CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
            string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
            string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
            string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
            string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
            string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

            string strClassName = CommonHelper.GetClassName(strTableName);//类名
            strClassName = CommonHelper.GetTableNameUpper(strClassName);

            Directory.CreateDirectory(strFilePath);
            StreamWriter sw = new StreamWriter(strFilePath + "\\" + strClassName + "DAL.cs", false, Encoding.GetEncoding("utf-8"));
            sw.WriteLine("using System;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine("using " + strProjectName + ".Model;");
            sw.WriteLine("using " + strProjectName + ".DBSqlHelper;");
            sw.WriteLine("using System.Data;");
            sw.WriteLine("using MySql.Data.MySqlClient;");
            sw.WriteLine("");
            sw.WriteLine("namespace " + CommonHelper.GetTableNameUpper(strProjectName) + ".DAL");
            sw.WriteLine("{");
            sw.WriteLine("");
            sw.WriteLine("    /// <summary>");
            sw.WriteLine("    /// 版权所有: Copyright © " + DateTime.Now.Year.ToString() + " "+strCompany+". 保留所有权利。");
            sw.WriteLine("    /// 内容摘要: " + strColumnComment);
            sw.WriteLine("    /// 创建日期：" + Convert.ToDateTime(strCreateDate).ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 更新日期：" + DateTime.Now.ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 版    本：V" + strVersion + "." + strCode + " ");
            sw.WriteLine("    /// 作    者：" + strAuthor);
            sw.WriteLine("    /// </summary>");
            sw.WriteLine("    public class " + strClassName + "DAL");
            sw.WriteLine("    {");
            sw.WriteLine("");
            string strFunctionList = GetDALFunctionList(strProjectName, strTableName);
            sw.WriteLine(strFunctionList);
            sw.WriteLine("    }");
            sw.WriteLine("}");
            sw.Close();

        }

        /// <summary>
        /// 根据表名创建DAL类
        /// </summary>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static string GetDALFunctionList(string strProjectName,string strTableName)
        {
            string strDALFunctionList = "";

            string strUpperTableName = CommonHelper.GetClassName(strTableName);
            strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);

            //获取主键ID
            string strPrimaryID = GetPrimaryKey(strTableName);
            //获取表中所有字段
            string strAllColumnName = GetAllColumnName(strTableName);
            string strSpecialColumnName = GetSpecialColumnName(strTableName);
            string strAllColumnNameNotKey = GetAllColumnNameNotKey(strTableName);
            string strSpecialColumnNameNotKey = GetSpecialColumnNameNotKey(strTableName);
            string strAllColumnNameAt = GetAllColumnNameForAt(strTableName);
            string strSpecialColumnNameAt = GetSpecialColumnNameForAt(strTableName);
            //获取表中字段数量
            int nNum = GetColumnNum(strTableName);
            //获取新增数据参数
            string strInsertPara = GetInsertParas(strTableName, false,1);//不包含主键
            string strInsertParaKey = GetInsertParas(strTableName,true, 1);//包含主键
            string strInsertParaForUpdate = GetInsertParas(strTableName, false, 2);

            //1--新增一条数据
            string strFunction1 = "\r\n";
            strFunction1 += "        /// <summary>\r\n";
            strFunction1 += "        /// 新增一条数据\r\n";
            strFunction1 += "        /// </summary>\r\n";
            strFunction1 += "        /// <param name=\"" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model\">实体类</param>\r\n";
            strFunction1 += "        /// <returns>新增成功=True,新增失败=False</returns>\r\n";
            strFunction1 += "        public bool InsertData(" + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model)\r\n";
            strFunction1 += "        {\r\n";
            strFunction1 += "            string strSql = \"INSERT INTO " + strTableName + "(" + strSpecialColumnNameNotKey + ") \" +\r\n";
            strFunction1 += "                \"VALUES(" + strSpecialColumnNameAt + ")\";\r\n";
            strFunction1 += "";
            strFunction1 += "            try\r\n";
            strFunction1 += "            {\r\n";
            strFunction1 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction1 += "                {\r\n";
            strFunction1 += "                    cn.Open();\r\n";
            strFunction1 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction1 += "                    {\r\n";
            strFunction1 += "                        cmd.CommandText = strSql;\r\n";
            strFunction1 += strInsertPara + "\r\n";//参数
            strFunction1 += "                        cmd.ExecuteNonQuery();\r\n";
            strFunction1 += "                    }\r\n";
            strFunction1 += "                }\r\n";
            strFunction1 += "";
            strFunction1 += "                return true;\r\n";
            strFunction1 += "            }\r\n";
            strFunction1 += "            catch (Exception ex)\r\n";
            strFunction1 += "            {\r\n";
            strFunction1 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"新增一条数据，SQL语句：\" + strSql, \"Insert" + strUpperTableName + "\", true);\r\n";
            strFunction1 += "                return false;\r\n";
            strFunction1 += "            }\r\n";
            strFunction1 += "        }\r\n";

            string strFunction11 = "\r\n";
            strFunction11 += "        /// <summary>\r\n";
            strFunction11 += "        /// 新增一条数据 BY SQL\r\n";
            strFunction11 += "        /// </summary>\r\n";
            strFunction11 += "        /// <param name=\"strSql\">SQL语句</param>\r\n";
            strFunction11 += "        /// <param name=\"" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model\">实体类</param>\r\n";
            strFunction11 += "        /// <returns>新增成功=True,新增失败=False</returns>\r\n";
            strFunction11 += "        public bool InsertDataBySQL(string strSql," + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model)\r\n";
            strFunction11 += "        {\r\n";
            strFunction11 += "";
            strFunction11 += "            try\r\n";
            strFunction11 += "            {\r\n";
            strFunction11 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction11 += "                {\r\n";
            strFunction11 += "                    cn.Open();\r\n";
            strFunction11 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction11 += "                    {\r\n";
            strFunction11 += "                        cmd.CommandText = strSql;\r\n";
            strFunction11 += strInsertPara + "\r\n";//参数
            strFunction11 += "                        cmd.ExecuteNonQuery();\r\n";
            strFunction11 += "                    }\r\n";
            strFunction11 += "                }\r\n";
            strFunction11 += "";
            strFunction11 += "                return true;\r\n";
            strFunction11 += "            }\r\n";
            strFunction11 += "            catch (Exception ex)\r\n";
            strFunction11 += "            {\r\n";
            strFunction11 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"新增一条数据，SQL语句：\" + strSql, \"Insert" + strUpperTableName + "\", true);\r\n";
            strFunction11 += "                return false;\r\n";
            strFunction11 += "            }\r\n";
            strFunction11 += "        }\r\n";

            //2--删除一条数据
            string strFunction2 = "\r\n";
            strFunction2 += "        /// <summary>\r\n";
            strFunction2 += "        /// 根据自定义条件删除数据\r\n";
            strFunction2 += "        /// </summary>\r\n";
            strFunction2 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction2 += "        /// <returns>删除成功=True,删除失败=False</returns>\r\n";
            strFunction2 += "        public bool DeleteData(string strWhere)\r\n";
            strFunction2 += "        {\r\n";
            strFunction2 += "            string strSql = \"DELETE FROM " + strTableName  +" \" + strWhere;\r\n";
            strFunction2 += "            //string strSql = \"UPDATE " + strTableName + " SET is_del=1 \" + strWhere;\r\n";
            strFunction2 += "";
            strFunction2 += "            try\r\n";
            strFunction2 += "            {\r\n";
            strFunction2 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction2 += "                {\r\n";
            strFunction2 += "                    cn.Open();\r\n";
            strFunction2 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction2 += "                    {\r\n";
            strFunction2 += "                        cmd.CommandText = strSql;\r\n";
            strFunction2 += "                        cmd.ExecuteNonQuery();\r\n";
            strFunction2 += "                    }\r\n";
            strFunction2 += "                }\r\n";
            strFunction2 += "";
            strFunction2 += "                return true;\r\n";
            strFunction2 += "            }\r\n";
            strFunction2 += "            catch (Exception ex)\r\n";
            strFunction2 += "            {\r\n";
            strFunction2 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据自定义条件删除数据，SQL语句：\" + strSql, \"DeleteData\", true);\r\n";
            strFunction2 += "                return false;\r\n";
            strFunction2 += "            }\r\n";
            strFunction2 += "        }\r\n";


            //3--根据自定义条件更新一条数据
            string strGetUpdateColumn = GetUpdateColumnName(strTableName);
            int numKey = nNum + 1;
            string strFunction3 = "\r\n";
            strFunction3 += "        /// <summary>\r\n";
            strFunction3 += "        /// 根据自定义条件更新一条数据\r\n";
            strFunction3 += "        /// </summary>\r\n";
            strFunction3 += "        /// <param name=\"" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model\">实体类</param>\r\n";
            strFunction3 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction3 += "        /// <returns>更新成功=True,更新失败=False</returns>\r\n";
            strFunction3 += "        public bool UpdateData(" + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model,string strWhere)\r\n";
            strFunction3 += "        {\r\n";
            strFunction3 += "            string strSql = \"UPDATE  " + strTableName + "  \"+\r\n";
            strFunction3 += "            \" SET " + strGetUpdateColumn + " \"+\r\n";
            strFunction3 += "            \" \" + strWhere;\r\n";
            strFunction3 += "";
            strFunction3 += "            try\r\n";
            strFunction3 += "            {\r\n";
            strFunction3 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction3 += "                {\r\n";
            strFunction3 += "                    cn.Open();\r\n";
            strFunction3 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction3 += "                    {\r\n";
            strFunction3 += "                        cmd.CommandText = strSql;\r\n";
            strFunction3 += strInsertParaForUpdate + "\r\n";//参数
            strFunction3 += "                        cmd.ExecuteNonQuery();\r\n";
            strFunction3 += "                    }\r\n";
            strFunction3 += "                }\r\n";
            strFunction3 += "";
            strFunction3 += "                return true;\r\n";
            strFunction3 += "            }\r\n";
            strFunction3 += "            catch (Exception ex)\r\n";
            strFunction3 += "            {\r\n";
            strFunction3 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"更新一条数据，SQL语句：\" + strSql, \"UpdateData\", true);\r\n";
            strFunction3 += "                return false;\r\n";
            strFunction3 += "            }\r\n";
            strFunction3 += "        }\r\n";

            //31--根据SQL语句更新数据
            string strFunction31 = "\r\n";
            strFunction31 += "        /// <summary>\r\n";
            strFunction31 += "        /// 根据SQL语句更新数据\r\n";
            strFunction31 += "        /// </summary>\r\n";
            strFunction31 += "        /// <param name=\"strSql\">SQL语句</param>\r\n";
            strFunction31 += "        /// <returns>更新成功=True,更新失败=False</returns>\r\n";
            strFunction31 += "        public bool UpdateDataBySQL(string strSql)\r\n";
            strFunction31 += "        {\r\n";
            strFunction31 += "";
            strFunction31 += "            try\r\n";
            strFunction31 += "            {\r\n";
            strFunction31 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction31 += "                {\r\n";
            strFunction31 += "                    cn.Open();\r\n";
            strFunction31 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction31 += "                    {\r\n";
            strFunction31 += "                        cmd.CommandText = strSql;\r\n";
            strFunction31 += "                        cmd.ExecuteNonQuery();\r\n";
            strFunction31 += "                    }\r\n";
            strFunction31 += "                }\r\n";
            strFunction31 += "";
            strFunction31 += "                return true;\r\n";
            strFunction31 += "            }\r\n";
            strFunction31 += "            catch (Exception ex)\r\n";
            strFunction31 += "            {\r\n";
            strFunction31 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"更新一条数据，SQL语句：\" + strSql, \"UpdateData\", true);\r\n";
            strFunction31 += "                return false;\r\n";
            strFunction31 += "            }\r\n";
            strFunction31 += "        }\r\n";


            //4--根据自定义条件查询表中一条数据
            string strFunction4 = "\r\n";
            strFunction4 += "        /// <summary>\r\n";
            strFunction4 += "        /// 根据自定义条件查询表中一条数据\r\n";
            strFunction4 += "        /// </summary>\r\n";
            strFunction4 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction4 += "        /// <returns>返回一条数据序列</returns>\r\n";
            strFunction4 += "        public " + strUpperTableName + "Model SelectOneByWhere(string strWhere)\r\n";
            strFunction4 += "        {\r\n";
            strFunction4 += "            string strSql = \"SELECT " + strAllColumnName + " \" +\r\n";
            strFunction4 += "            \" FROM " + strTableName + " \" +\r\n";
            strFunction4 += "            \" \" + strWhere;\r\n";
            strFunction4 += "\r\n";
            strFunction4 += "            " + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model  = new " + strUpperTableName + "Model();\r\n";
            strFunction4 += "\r\n";
            strFunction4 += "            try\r\n";
            strFunction4 += "            {\r\n";
            strFunction4 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction4 += "                {\r\n";
            strFunction4 += "                    cn.Open();\r\n";
            strFunction4 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction4 += "                    {\r\n";
            strFunction4 += "                        cmd.CommandText = strSql;\r\n";
            strFunction4 += "                        using (MySqlDataReader reader = cmd.ExecuteReader())\r\n";
            strFunction4 += "                        {\r\n";
            strFunction4 += "                           if (reader.Read())\r\n";
            strFunction4 += "                           {\r\n";
            strFunction4 += GetSelectPara(strTableName) + "\r\n";//参数
            strFunction4 += "                           }\r\n";
            strFunction4 += "                        }\r\n";
            strFunction4 += "                    }\r\n";
            strFunction4 += "                }\r\n";
            strFunction4 += "";
            strFunction4 += "            }\r\n";
            strFunction4 += "            catch (Exception ex)\r\n";
            strFunction4 += "            {\r\n";
            strFunction4 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据自定义条件查询表中一条数据，SQL语句：\" + strSql, \"SelectOneByWhere\", true);\r\n";
            strFunction4 += "            }\r\n";
            strFunction4 += "                return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model;\r\n";
            strFunction4 += "        }\r\n";

            //14--根据自定义条件查询表中一条数据
            string strFunction14 = "\r\n";
            strFunction14 += "        /// <summary>\r\n";
            strFunction14 += "        /// 根据自定义条件查询表中一条数据 By SQL\r\n";
            strFunction14 += "        /// </summary>\r\n";
            strFunction14 += "        /// <param name=\"strSql\">SQL语句</param>\r\n";
            strFunction14 += "        /// <returns>返回一条数据序列</returns>\r\n";
            strFunction14 += "        public " + strUpperTableName + "Model SelectOneBySQL(string strSql)\r\n";
            strFunction14 += "        {\r\n";
            strFunction14 += "\r\n";
            strFunction14 += "            " + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model  = new " + strUpperTableName + "Model();\r\n";
            strFunction14 += "\r\n";
            strFunction14 += "            try\r\n";
            strFunction14 += "            {\r\n";
            strFunction14 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction14 += "                {\r\n";
            strFunction14 += "                    cn.Open();\r\n";
            strFunction14 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction14 += "                    {\r\n";
            strFunction14 += "                        cmd.CommandText = strSql;\r\n";
            strFunction14 += "                        using (MySqlDataReader reader = cmd.ExecuteReader())\r\n";
            strFunction14 += "                        {\r\n";
            strFunction14 += "                           if (reader.Read())\r\n";
            strFunction14 += "                           {\r\n";
            strFunction14 += GetSelectPara(strTableName) + "\r\n";//参数
            strFunction14 += "                           }\r\n";
            strFunction14 += "                        }\r\n";
            strFunction14 += "                    }\r\n";
            strFunction14 += "                }\r\n";
            strFunction14 += "";
            strFunction14 += "            }\r\n";
            strFunction14 += "            catch (Exception ex)\r\n";
            strFunction14 += "            {\r\n";
            strFunction14 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据自定义条件查询表中一条数据，SQL语句：\" + strSql, \"SelectOneByWhere\", true);\r\n";
            strFunction14 += "            }\r\n";
            strFunction14 += "                return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model;\r\n";
            strFunction14 += "        }\r\n";

            //5--根据自定义条件查询表中所有数据
            string strFunction5 = "\r\n";
            strFunction5 += "        /// <summary>\r\n";
            strFunction5 += "        /// 根据自定义条件查询表中所有数据\r\n";
            strFunction5 += "        /// </summary>\r\n";
            strFunction5 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction5 += "        /// <returns>返回数据集</returns>\r\n";
            strFunction5 += "        public List<" + strUpperTableName + "Model> SelectAllByWhere(string strWhere)\r\n";
            strFunction5 += "        {\r\n";
            strFunction5 += "            string strSql = \"SELECT " + strAllColumnName + " \" +\r\n";
            strFunction5 += "            \" FROM " + strTableName + " \" +\r\n";
            strFunction5 += "            \" \" + strWhere;\r\n";
            strFunction5 += "\r\n";
            strFunction5 += "            List<" + strUpperTableName + "Model> list = new List<" + strUpperTableName + "Model>();\r\n";
            strFunction5 += "\r\n";
            strFunction5 += "            try\r\n";
            strFunction5 += "            {\r\n";
            strFunction5 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction5 += "                {\r\n";
            strFunction5 += "                    cn.Open();\r\n";
            strFunction5 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction5 += "                    {\r\n";
            strFunction5 += "                        cmd.CommandText = strSql;\r\n";
            strFunction5 += "                        using (MySqlDataReader reader = cmd.ExecuteReader())\r\n";
            strFunction5 += "                        {\r\n";
            strFunction5 += "                           while (reader.Read())\r\n";
            strFunction5 += "                           {\r\n";
            strFunction5 += "                                " + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model = new " + strUpperTableName + "Model();\r\n";
            strFunction5 += GetSelectPara(strTableName) + "\r\n";//参数
            strFunction5 += "                               list.Add(" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model);\r\n";
            strFunction5 += "                           }\r\n";
            strFunction5 += "                        }\r\n";
            strFunction5 += "                    }\r\n";
            strFunction5 += "                }\r\n";
            strFunction5 += "";
            strFunction5 += "            }\r\n";
            strFunction5 += "            catch (Exception ex)\r\n";
            strFunction5 += "            {\r\n";
            strFunction5 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据自定义条件查询表中一条数据，SQL语句：\" + strSql, \"SelectOneByWhere\", true);\r\n";
            strFunction5 += "            }\r\n";
            strFunction5 += "            return list;\r\n";
            strFunction5 += "        }\r\n";

            string strFunction15 = "\r\n";
            strFunction15 += "        /// <summary>\r\n";
            strFunction15 += "        /// 根据自定义条件查询表中所有数据 By SQL\r\n";
            strFunction15 += "        /// </summary>\r\n";
            strFunction15 += "        /// <param name=\"strSql\">SQL语句</param>\r\n";
            strFunction15 += "        /// <returns>返回数据集</returns>\r\n";
            strFunction15 += "        public List<" + strUpperTableName + "Model> SelectAllBySQL(string strSql)\r\n";
            strFunction15 += "        {\r\n";
            strFunction15 += "\r\n";
            strFunction15 += "            List<" + strUpperTableName + "Model> list = new List<" + strUpperTableName + "Model>();\r\n";
            strFunction15 += "\r\n";
            strFunction15 += "            try\r\n";
            strFunction15 += "            {\r\n";
            strFunction15 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction15 += "                {\r\n";
            strFunction15 += "                    cn.Open();\r\n";
            strFunction15 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction15 += "                    {\r\n";
            strFunction15 += "                        cmd.CommandText = strSql;\r\n";
            strFunction15 += "                        using (MySqlDataReader reader = cmd.ExecuteReader())\r\n";
            strFunction15 += "                        {\r\n";
            strFunction15 += "                           while (reader.Read())\r\n";
            strFunction15 += "                           {\r\n";
            strFunction15 += "                                " + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model = new " + strUpperTableName + "Model();\r\n";
            strFunction15 += GetSelectPara(strTableName) + "\r\n";//参数
            strFunction15 += "                               list.Add(" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model);\r\n";
            strFunction15 += "                           }\r\n";
            strFunction15 += "                        }\r\n";
            strFunction15 += "                    }\r\n";
            strFunction15 += "                }\r\n";
            strFunction15 += "";
            strFunction15 += "            }\r\n";
            strFunction15 += "            catch (Exception ex)\r\n";
            strFunction15 += "            {\r\n";
            strFunction15 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据自定义条件查询表中一条数据，SQL语句：\" + strSql, \"SelectOneByWhere\", true);\r\n";
            strFunction15 += "            }\r\n";
            strFunction15 += "            return list;\r\n";
            strFunction15 += "        }\r\n";


            //6--根据条件获取表中任一字段的值
            string strFunction6 = "\r\n";
            strFunction6 += "        /// <summary>\r\n";
            strFunction6 += "        /// 根据条件获取表中任一字段的值\r\n";
            strFunction6 += "        /// </summary>\r\n";
            strFunction6 += "        /// <param name=\"strColumnName\">任一字段名</param>\r\n";
            strFunction6 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction6 += "        /// <returns>字段值</returns>\r\n";
            strFunction6 += "        public string GetColumnValueByWhere(string strColumnName,string strWhere)\r\n";
            strFunction6 += "        {\r\n";
            strFunction6 += "            string strReturnValue = \"\";\r\n";
            strFunction6 += "\r\n";
            strFunction6 += "            string strSql = \"SELECT \" + strColumnName + \" \" +\r\n";
            strFunction6 += "            \" FROM " + strTableName + " \" +\r\n";
            strFunction6 += "            \" \" + strWhere;\r\n";
            strFunction6 += "";
            strFunction6 += "            try\r\n";
            strFunction6 += "            {\r\n";
            strFunction6 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction6 += "                {\r\n";
            strFunction6 += "                    cn.Open();\r\n";
            strFunction6 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction6 += "                    {\r\n";
            strFunction6 += "                        cmd.CommandText = strSql;\r\n";
            strFunction6 += "                        using (MySqlDataReader reader = cmd.ExecuteReader())\r\n";
            strFunction6 += "                        {\r\n";
            strFunction6 += "                           if (reader.Read())\r\n";
            strFunction6 += "                           {\r\n";
            strFunction6 += "                               strReturnValue = reader[strColumnName].ToString();\r\n";//参数
            strFunction6 += "                           }\r\n";
            strFunction6 += "                           else\r\n";
            strFunction6 += "                           {\r\n";
            strFunction6 += "                               strReturnValue = \"\";\r\n";
            strFunction6 += "                           }\r\n";
            strFunction6 += "                        }\r\n";
            strFunction6 += "                    }\r\n";
            strFunction6 += "                }\r\n";
            strFunction6 += "\r\n";
            strFunction6 += "            }\r\n";
            strFunction6 += "            catch (Exception ex)\r\n";
            strFunction6 += "            {\r\n";
            strFunction6 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据条件获取表中任一字段的值，SQL语句：\" + strSql, \"GetColumnValueByWhere\", true);\r\n"; 
            strFunction6 += "            }\r\n";
            strFunction6 += "\r\n";
            strFunction6 += "            return strReturnValue;\r\n";
            strFunction6 += "        }\r\n";


            //7--根据条件检测表中任一字段的值是否存在
            string strFunction7 = "\r\n";
            strFunction7 += "        /// <summary>\r\n";
            strFunction7 += "        /// 根据条件检测表中任一字段的值是否存在，如果不限定条件，即CheckColumnValueHasExist(\"\")\r\n";
            strFunction7 += "        /// </summary>\r\n";
            strFunction7 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction7 += "        /// <returns>返回是否存在该值，存在=True,不存在=False</returns>\r\n";
            strFunction7 += "        public bool CheckColumnValueHasExist(string strWhere)\r\n";
            strFunction7 += "        {\r\n";
            strFunction7 += "            string strSql = \"SELECT " + strPrimaryID + " \" +\r\n";
            strFunction7 += "            \" FROM " + strTableName + " \" +\r\n";
            strFunction7 += "            \" \" + strWhere;\r\n";
            strFunction7 += "";
            strFunction7 += "            try\r\n";
            strFunction7 += "            {\r\n";
            strFunction7 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction7 += "                {\r\n";
            strFunction7 += "                    cn.Open();\r\n";
            strFunction7 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction7 += "                    {\r\n";
            strFunction7 += "                        cmd.CommandText = strSql;\r\n";
            strFunction7 += "                        using (MySqlDataReader reader = cmd.ExecuteReader())\r\n";
            strFunction7 += "                        {\r\n";
            strFunction7 += "                           if (reader.Read())\r\n";
            strFunction7 += "                           {\r\n";
            strFunction7 += "                               return true;\r\n";//参数
            strFunction7 += "                           }\r\n";
            strFunction7 += "                           else\r\n";
            strFunction7 += "                           {\r\n";
            strFunction7 += "                               return false;\r\n";
            strFunction7 += "                           }\r\n";
            strFunction7 += "                        }\r\n";
            strFunction7 += "                    }\r\n";
            strFunction7 += "                }\r\n";
            strFunction7 += "";
            strFunction7 += "            }\r\n";
            strFunction7 += "            catch (Exception ex)\r\n";
            strFunction7 += "            {\r\n";
            strFunction7 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据条件检测表中任一字段的值是否存在，SQL语句：\" + strSql, \"CheckColumnValueHasExist\", true);\r\n";
            strFunction7 += "                return false;\r\n";
            strFunction7 += "            }\r\n";
            strFunction7 += "        }\r\n";


            //8--根据条件统计数据字段累加结果
            string strFunction8 = "\r\n";
            strFunction8 += "        /// <summary>\r\n";
            strFunction8 += "        /// 根据条件统计数据字段累加结果\r\n";
            strFunction8 += "        /// </summary>\r\n";
            strFunction8 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction8 += "        /// <returns>返回值</returns>\r\n";
            strFunction8 += "        public int CountNumByWhere(string strWhere)\r\n";
            strFunction8 += "        {\r\n";
            strFunction8 += "            int nNum = 0;\r\n";
            strFunction8 += "\r\n";
            strFunction8 += "            string strSql = \"SELECT count(" + strPrimaryID + ") as num \" +\r\n";
            strFunction8 += "            \" FROM " + strTableName + " \" +\r\n";
            strFunction8 += "            \" \" + strWhere;\r\n";
            strFunction8 += "";
            strFunction8 += "            try\r\n";
            strFunction8 += "            {\r\n";
            strFunction8 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction8 += "                {\r\n";
            strFunction8 += "                    cn.Open();\r\n";
            strFunction8 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction8 += "                    {\r\n";
            strFunction8 += "                        cmd.CommandText = strSql;\r\n";
            strFunction8 += "                        using (MySqlDataReader reader = cmd.ExecuteReader())\r\n";
            strFunction8 += "                        {\r\n";
            strFunction8 += "                           if (reader.Read())\r\n";
            strFunction8 += "                           {\r\n";
            strFunction8 += "                               nNum = Convert.ToInt32(reader[\"num\"]);\r\n";//参数
            strFunction8 += "                           }\r\n";
            strFunction8 += "                           else\r\n";
            strFunction8 += "                           {\r\n";
            strFunction8 += "                               nNum = 0;\r\n";
            strFunction8 += "                           }\r\n";
            strFunction8 += "                        }\r\n";
            strFunction8 += "                    }\r\n";
            strFunction8 += "                }\r\n";
            strFunction8 += "";
            strFunction8 += "            }\r\n";
            strFunction8 += "            catch (Exception ex)\r\n";
            strFunction8 += "            {\r\n";
            strFunction8 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据条件统计数据字段累加结果，SQL语句：\" + strSql, \"CountNumByWhere\", true);\r\n";
            strFunction8 += "            }\r\n";
            strFunction8 += "\r\n";
            strFunction8 += "            return nNum;\r\n";
            strFunction8 += "        }\r\n";

            //81--根据条件统计字段数值相加
            string strFunction81 = "\r\n";
            strFunction81 += "        /// <summary>\r\n";
            strFunction81 += "        /// 根据条件统计字段数值相加\r\n";
            strFunction81 += "        /// </summary>\r\n";
            strFunction81 += "        /// <param name=\"strKeyValue\">需要统计的字段名称</param>\r\n";
            strFunction81 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction81 += "        /// <returns>返回值</returns>\r\n";
            strFunction81 += "        public int SumNumByWhere(string strKeyName,string strWhere)\r\n";
            strFunction81 += "        {\r\n";
            strFunction81 += "            int nNum = 0;\r\n";
            strFunction81 += "\r\n";
            strFunction81 += "            string strSql = \"SELECT sum(\" + strKeyName + \") as num \" +\r\n";
            strFunction81 += "            \" FROM " + strTableName + " \" +\r\n";
            strFunction81 += "            \" \" + strWhere;\r\n";
            strFunction81 += "";
            strFunction81 += "            try\r\n";
            strFunction81 += "            {\r\n";
            strFunction81 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction81 += "                {\r\n";
            strFunction81 += "                    cn.Open();\r\n";
            strFunction81 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction81 += "                    {\r\n";
            strFunction81 += "                        cmd.CommandText = strSql;\r\n";
            strFunction81 += "                        using (MySqlDataReader reader = cmd.ExecuteReader())\r\n";
            strFunction81 += "                        {\r\n";
            strFunction81 += "                           if (reader.Read())\r\n";
            strFunction81 += "                           {\r\n";
            strFunction81 += "                                if (reader[\"num\"] is DBNull)\r\n";
            strFunction81 += "                                {\r\n";
            strFunction81 += "                                    nNum = 0;\r\n";
            strFunction81 += "                                }\r\n";
            strFunction81 += "                                else\r\n";
            strFunction81 += "                                {\r\n";
            strFunction81 += "                                    nNum = Convert.ToInt32(reader[\"num\"]);\r\n";
            strFunction81 += "                                }\r\n";
            strFunction81 += "                           }\r\n";
            strFunction81 += "                           else\r\n";
            strFunction81 += "                           {\r\n";
            strFunction81 += "                               nNum = 0;\r\n";
            strFunction81 += "                           }\r\n";
            strFunction81 += "                        }\r\n";
            strFunction81 += "                    }\r\n";
            strFunction81 += "                }\r\n";
            strFunction81 += "";
            strFunction81 += "            }\r\n";
            strFunction81 += "            catch (Exception ex)\r\n";
            strFunction81 += "            {\r\n";
            strFunction81 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据条件统计字段数值相加，SQL语句：\" + strSql, \"SumNumByWhere\", true);\r\n";
            strFunction81 += "            }\r\n";
            strFunction81 += "\r\n";
            strFunction81 += "            return nNum;\r\n";
            strFunction81 += "        }\r\n";

            //82--根据SQL语句统计某字段的数值相加或累加
            string strFunction82 = "\r\n";
            strFunction82 += "        /// <summary>\r\n";
            strFunction82 += "        /// 根据SQL语句统计某字段的数值相加或累加\r\n";
            strFunction82 += "        /// </summary>\r\n";
            strFunction82 += "        /// <param name=\"strSql\">SQL语句，注意：count或sum语句格式 count(xxx) as num,sum(xxx) as num</param>\r\n";
            strFunction82 += "        /// <returns>返回值</returns>\r\n";
            strFunction82 += "        public int CountOrSumNumBySQL(string strSql)\r\n";
            strFunction82 += "        {\r\n";
            strFunction82 += "            int nNum = 0;\r\n";
            strFunction82 += "\r\n";
            strFunction82 += "";
            strFunction82 += "            try\r\n";
            strFunction82 += "            {\r\n";
            strFunction82 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction82 += "                {\r\n";
            strFunction82 += "                    cn.Open();\r\n";
            strFunction82 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction82 += "                    {\r\n";
            strFunction82 += "                        cmd.CommandText = strSql;\r\n";
            strFunction82 += "                        using (MySqlDataReader reader = cmd.ExecuteReader())\r\n";
            strFunction82 += "                        {\r\n";
            strFunction82 += "                           if (reader.Read())\r\n";
            strFunction82 += "                           {\r\n";
            strFunction82 += "                                if (reader[\"num\"] is DBNull)\r\n";
            strFunction82 += "                                {\r\n";
            strFunction82 += "                                    nNum = 0;\r\n";
            strFunction82 += "                                }\r\n";
            strFunction82 += "                                else\r\n";
            strFunction82 += "                                {\r\n";
            strFunction82 += "                                    nNum = Convert.ToInt32(reader[\"num\"]);\r\n";
            strFunction82 += "                                }\r\n";
            strFunction82 += "                           }\r\n";
            strFunction82 += "                           else\r\n";
            strFunction82 += "                           {\r\n";
            strFunction82 += "                               nNum = 0;\r\n";
            strFunction82 += "                           }\r\n";
            strFunction82 += "                        }\r\n";
            strFunction82 += "                    }\r\n";
            strFunction82 += "                }\r\n";
            strFunction82 += "";
            strFunction82 += "            }\r\n";
            strFunction82 += "            catch (Exception ex)\r\n";
            strFunction82 += "            {\r\n";
            strFunction82 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据SQL语句统计某字段的数值相加或累加，SQL语句：\" + strSql, \"CountNumByWhere\", true);\r\n";
            strFunction82 += "            }\r\n";
            strFunction82 += "\r\n";
            strFunction82 += "            return nNum;\r\n";
            strFunction82 += "        }\r\n";

            //83--根据条件统计字段数值相加
            string strFunction83 = "\r\n";
            strFunction83 += "        /// <summary>\r\n";
            strFunction83 += "        /// 根据条件统计字段数值相加\r\n";
            strFunction83 += "        /// </summary>\r\n";
            strFunction83 += "        /// <param name=\"strKeyValue\">需要统计的字段名称</param>\r\n";
            strFunction83 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction83 += "        /// <returns>返回值</returns>\r\n";
            strFunction83 += "        public double SumDoubleByWhere(string strKeyName,string strWhere)\r\n";
            strFunction83 += "        {\r\n";
            strFunction83 += "             double nNum= 0;\r\n";
            strFunction83 += "\r\n";
            strFunction83 += "            string strSql = \"SELECT sum(\" + strKeyName + \") as num \" +\r\n";
            strFunction83 += "            \" FROM " + strTableName + " \" +\r\n";
            strFunction83 += "            \" \" + strWhere;\r\n";
            strFunction83 += "";
            strFunction83 += "            try\r\n";
            strFunction83 += "            {\r\n";
            strFunction83 += "                using (MySqlConnection cn = new MySqlConnection(" + strProjectName + ".DBSqlHelper.DBMySQLHelper.ConnectMySQL()))\r\n";
            strFunction83 += "                {\r\n";
            strFunction83 += "                    cn.Open();\r\n";
            strFunction83 += "                    using (MySqlCommand cmd = cn.CreateCommand())\r\n";
            strFunction83 += "                    {\r\n";
            strFunction83 += "                        cmd.CommandText = strSql;\r\n";
            strFunction83 += "                        using (MySqlDataReader reader = cmd.ExecuteReader())\r\n";
            strFunction83 += "                        {\r\n";
            strFunction83 += "                           if (reader.Read())\r\n";
            strFunction83 += "                           {\r\n";
            strFunction83 += "                                if (reader[\"num\"] is DBNull)\r\n";
            strFunction83 += "                                {\r\n";
            strFunction83 += "                                    nNum = 0;\r\n";
            strFunction83 += "                                }\r\n";
            strFunction83 += "                                else\r\n";
            strFunction83 += "                                {\r\n";
            strFunction83 += "                                    nNum = Convert.ToDouble(reader[\"num\"]);\r\n";
            strFunction83 += "                                }\r\n";
            strFunction83 += "                           }\r\n";
            strFunction83 += "                           else\r\n";
            strFunction83 += "                           {\r\n";
            strFunction83 += "                               nNum = 0;\r\n";
            strFunction83 += "                           }\r\n";
            strFunction83 += "                        }\r\n";
            strFunction83 += "                    }\r\n";
            strFunction83 += "                }\r\n";
            strFunction83 += "";
            strFunction83 += "            }\r\n";
            strFunction83 += "            catch (Exception ex)\r\n";
            strFunction83 += "            {\r\n";
            strFunction83 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据条件统计字段数值相加，SQL语句：\" + strSql, \"SumNumByWhere\", true);\r\n";
            strFunction83 += "            }\r\n";
            strFunction83 += "\r\n";
            strFunction83 += "            return nNum;\r\n";
            strFunction83 += "        }\r\n";

            strDALFunctionList = strFunction1 + strFunction11 + strFunction2 + strFunction3 + strFunction31 + strFunction4 + strFunction14 + strFunction5 + strFunction15 + strFunction6 +
                strFunction7 + strFunction8 + strFunction81 + strFunction82 + strFunction83;

            return strDALFunctionList;
        }

        /// <summary>
        /// 获取表中的主键ID
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetPrimaryKey(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,DATA_TYPE,COLUMN_TYPE,COLUMN_KEY,COLUMN_COMMENT,EXTRA,COLUMN_DEFAULT,CHARACTER_SET_NAME FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释

                    if(strColumnKey== "PRI")
                    {
                        strReturnValue = strColumnName;
                        break;
                    }
                    
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中的主键ID", "GetPrimaryKey",false);
            }

            return strReturnValue;

        }

        /// <summary>
        /// 获取表中所有字段，不包含主键
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetAllColumnNameNotKey(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    if (strColumnKey != "PRI")
                    {
                        strReturnValue += strColumnName + ",";

                    }
                    
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段", "GetAllColumnName",false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        public static string GetSpecialColumnNameNotKey(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    if (strColumnKey != "PRI")
                    {
                        if (!strColumnComment.Contains("{添加排除}"))
                        {
                            strReturnValue += strColumnName + ",";
                        }

                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段", "GetAllColumnName", false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取表中所有字段
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetAllColumnName(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    strReturnValue += strColumnName + ",";
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段", "GetAllColumnName",false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取表中部分字段，过滤掉排除的
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetSpecialColumnName(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    if (!strColumnComment.Contains("{添加排除}"))
                    {
                        strReturnValue += strColumnName + ",";
                    }
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段", "GetAllColumnName", false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取表中所有字段@
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetAllColumnNameForAt(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    if (strColumnKey != "PRI")
                    {
                        strReturnValue += "@" + strColumnName + ",";
                    }
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段@", "GetAllColumnNameForAt",false);
            }

            if(!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        public static string GetSpecialColumnNameForAt(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    if (strColumnKey != "PRI")
                    {
                        if (!strColumnComment.Contains("{添加排除}"))
                        {
                            strReturnValue += "@" + strColumnName + ",";
                        }
                        
                    }
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段@", "GetAllColumnNameForAt", false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取表中字段数量
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static int GetColumnNum(string strTableName)
        {
            int nNum = 0;

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    nNum++;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中字段数量", "GetColumnNum",false);
            }

            //减去自增主键
            nNum = nNum - 1;

            return nNum;
        }

        /// <summary>
        /// 获取新增数据参数
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetInsertParas(string strTableName, bool hasKey, int nFlag)
        {
            string strReturnValue = "";

            try
            {
                int nNum = 0;
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT,DATA_TYPE FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnNameUpper = CommonHelper.GetTableNameUpper(strColumnName);//如：UserName
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    string strDataType = dr["DATA_TYPE"].ToString();//数据类型
                    strDataType = StringHelper.GetCSharpDBType(strDataType);

                    //获取数据类型
                    string strValue = "";
                    if (strDataType == "int")
                    {
                        strValue = "Int32";
                    }
                    else if (strDataType == "float")
                    {
                        strValue = "Float";
                    }
                    else if (strDataType == "double")
                    {
                        strValue = "Double";
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = "Decimal";
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = "Bit";
                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = "DateTime";
                    }
                    else if (strDataType == "string")
                    {
                        strValue = "VarChar";
                    }
                    else
                    {
                        strValue = "VarChar";
                    }

                    if(hasKey)
                    {
                        //包含主键
                        if (!strColumnComment.Contains("{添加排除}"))
                        {
                            strReturnValue += "                        cmd.Parameters.Add(new MySqlParameter(\"@" + strColumnName + "\", MySqlDbType." + strValue + ") { Value = " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + strColumnNameUpper + " });\r\n";
                        }
                        nNum++;
                    }
                    else
                    {
                        //不包含主键
                        if (strColumnKey != "PRI")
                        {
                            if(nFlag == 2)
                            {
                                //应用到更新数据模块
                                if (strColumnName == "visit_num" || strColumnName == "created_at" || strColumnName == "like_num" || strColumnName == "comment_num" || strColumnName == "favorite_num" || strColumnName == "share_num")
                                {
                                    //
                                }
                                else
                                {
                                    if (!strColumnComment.Contains("{添加排除}"))
                                    {
                                        strReturnValue += "                        cmd.Parameters.Add(new MySqlParameter(\"@" + strColumnName + "\", MySqlDbType." + strValue + ") { Value = " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + strColumnNameUpper + " });\r\n";
                                    }
                                    nNum++;
                                }
                            }
                            else
                            {
                                if (!strColumnComment.Contains("{添加排除}"))
                                {
                                    //strReturnValue += "paraArray[" + nNum + "] = mysqlHelper.InitMySqlParameter(\"@" + strColumnName + "\", ParameterDirection.Input, " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + ");\r\n";
                                    strReturnValue += "                        cmd.Parameters.Add(new MySqlParameter(\"@" + strColumnName + "\", MySqlDbType." + strValue + ") { Value = " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + strColumnNameUpper + " });\r\n";
                                }
                                
                                nNum++;
                            }
                            
                        }
                    }
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取新增数据参数", "GetInsertParas",false);
            }

            return strReturnValue;
        }


        /// <summary>
        /// 获取更新字段
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetUpdateColumnName(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    //不包含主键
                    //排除visit_num,created_at,like_num,comment_num,favorite_num,share_num
                    if (strColumnKey != "PRI")
                    {
                        if(strColumnName == "visit_num" || strColumnName == "created_at" || strColumnName == "like_num" || strColumnName == "comment_num" || strColumnName == "favorite_num" || strColumnName == "share_num" || strColumnName == "last_loginat" || strColumnName == "last_activityat")
                        {
                            //
                        }
                        else
                        {
                            if (!strColumnComment.Contains("{添加排除}"))
                            {
                                strReturnValue += " " + strColumnName + " = @" + strColumnName + ",";
                            }
                        }
                    }
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取更新字段@", "GetUpdateColumnName",false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取查询参数
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetSelectPara(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                string strUpperTableName = CommonHelper.GetClassName(strTableName);
                strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);

                int nNum = 0;
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT,DATA_TYPE FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strDataType = dr["DATA_TYPE"].ToString();//数据类型
                    strDataType = StringHelper.GetCSharpDBType(strDataType);

                    string strValue = "";
                    if(strDataType=="int")
                    {
                        //需要优化，当字段为DBNull时可以，为空时有问题
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = reader[\"" + strColumnName + "\"] is DBNull ? 0 : Convert.ToInt32(reader[\"" + strColumnName + "\"]);\r\n";
                    }
                    else if (strDataType == "float")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = reader[\"" + strColumnName + "\"] is DBNull ? 0 : Convert.ToDouble(reader[\"" + strColumnName + "\"]);\r\n";
                    }
                    else if (strDataType == "double")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = reader[\"" + strColumnName + "\"] is DBNull ? 0 : Convert.ToDouble(reader[\"" + strColumnName + "\"]);\r\n";
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = reader[\"" + strColumnName + "\"] is DBNull ? 0 : Convert.ToDouble(reader[\"" + strColumnName + "\"]);\r\n";
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = reader[\"" + strColumnName + "\"] is DBNull ? false : Convert.ToBoolean(reader[\"" + strColumnName + "\"]);\r\n";
                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = reader[\"" + strColumnName + "\"] is DBNull ? Convert.ToDateTime(\"1900-01-01\") : Convert.ToDateTime(reader[\"" + strColumnName + "\"]);\r\n";
                    }
                    else if (strDataType == "string")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = reader[\"" + strColumnName + "\"] == null ? \"\" : reader[\"" + strColumnName + "\"].ToString();\r\n";
                    }
                    else
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = reader[\"" + strColumnName + "\"] == null ? \"\" : reader[\"" + strColumnName + "\"].ToString();\r\n";
                    }

                    strReturnValue += "                                " + strValue;

                    nNum++;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取查询参数", "GetSelectPara",false);
            }

            return strReturnValue;
        }

    }
}
