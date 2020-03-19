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
    /// 业务逻辑处理类
    /// </summary>
    public class BLLHelper
    {

        /// <summary>
        /// 根据表名创建DAL类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateBLLClass(string strFilePath,string strProjectName, string strTableName, string strColumnComment)
        {
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
            StreamWriter sw = new StreamWriter(strFilePath + "\\" + strClassName + "BLL.cs", false, Encoding.GetEncoding("utf-8"));
            sw.WriteLine("using System;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine("using " + strProjectName + ".Model;");
            sw.WriteLine("using " + strProjectName + ".DAL;");
            sw.WriteLine("");
            sw.WriteLine("namespace " + CommonHelper.GetTableNameUpper(strProjectName) + ".BLL");
            sw.WriteLine("{");
            sw.WriteLine("");
            sw.WriteLine("    /// <summary>");
            sw.WriteLine("    /// 版权所有: Copyright © " + DateTime.Now.Year.ToString() + " "+strCompany+". 保留所有权利。");
            sw.WriteLine("    /// 内容摘要: " + strClassName + "BLL");
            sw.WriteLine("    /// 创建日期：" + Convert.ToDateTime(strCreateDate).ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 更新日期：" + DateTime.Now.ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 版    本：V" + strVersion + "." + strCode + " ");
            sw.WriteLine("    /// 作    者：" + strAuthor);
            sw.WriteLine("    /// </summary>");
            sw.WriteLine("    public class " + strClassName + "BLL");
            sw.WriteLine("    {");
            string strFunctionList = GetBLLFunctionList(strProjectName, strTableName);
            sw.WriteLine(strFunctionList);
            sw.WriteLine("    }");
            sw.WriteLine("}");
            sw.Close();

        }

        /// <summary>
        /// 根据表名创建BLL类
        /// </summary>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static string GetBLLFunctionList(string strProjectName,string strTableName)
        {
            string strDALFunctionList = "";

            string strUpperTableName = CommonHelper.GetClassName(strTableName);
            strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);

            //获取主键ID
            string strPrimaryID = GetPrimaryKey(strTableName);
            //获取表中所有字段
            string strAllColumnName = GetAllColumnName(strTableName);
            string strAllColumnNameAt = GetAllColumnNameForAt(strTableName);
            //获取表中字段数量
            int nNum = GetColumnNum(strTableName);
            //获取新增数据参数
            string strInsertPara = GetInsertParas(strTableName);

            //1--新增一条数据
            string strFunction1 = "\r\n";
            strFunction1 += "        /// <summary>\r\n";
            strFunction1 += "        /// 新增一条数据\r\n";
            strFunction1 += "        /// </summary>\r\n";
            strFunction1 += "        /// <param name=\"" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model\">实体类</param>\r\n";
            strFunction1 += "        /// <returns>新增成功=True,新增失败=False</returns>\r\n";
            strFunction1 += "        public static bool InsertData(" + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model)\r\n";
            strFunction1 += "        {\r\n";
            strFunction1 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction1 += "\r\n";
            strFunction1 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.InsertData(" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model);\r\n";
            strFunction1 += "        }\r\n";

            string strFunction11 = "\r\n";
            strFunction11 += "        /// <summary>\r\n";
            strFunction11 += "        /// 新增一条数据 By SQL\r\n";
            strFunction11 += "        /// </summary>\r\n";
            strFunction11 += "        /// <param name=\"strSql\">SQL语句</param>\r\n";
            strFunction11 += "        /// <param name=\"" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model\">实体类</param>\r\n";
            strFunction11 += "        /// <returns>新增成功=True,新增失败=False</returns>\r\n";
            strFunction11 += "        public static bool InsertData(string strSql," + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model)\r\n";
            strFunction11 += "        {\r\n";
            strFunction11 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction11 += "\r\n";
            strFunction11 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.InsertDataBySQL(strSql," + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model);\r\n";
            strFunction11 += "        }\r\n";


            //2--删除一条数据
            string strFunction2 = "\r\n";
            strFunction2 += "        /// <summary>\r\n";
            strFunction2 += "        /// 删除一条数据\r\n";
            strFunction2 += "        /// </summary>\r\n";
            strFunction2 += "        /// <param name=\"strWhere\">自定义条件</param>\r\n";
            strFunction2 += "        /// <returns>新增成功=True,新增失败=False</returns>\r\n";
            strFunction2 += "        public static bool DeleteData(string strWhere)\r\n";
            strFunction2 += "        {\r\n";
            strFunction2 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction2 += "\r\n";
            strFunction2 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.DeleteData(strWhere);\r\n";
            strFunction2 += "        }\r\n";

            //3--根据自定义条件更新一条数据
            string strFunction3 = "\r\n";
            strFunction3 += "        /// <summary>\r\n";
            strFunction3 += "        /// 根据自定义条件更新一条数据\r\n";
            strFunction3 += "        /// </summary>\r\n";
            strFunction3 += "        /// <param name=\"" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model\">实体类</param>\r\n";
            strFunction3 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction3 += "        /// <returns>更新成功=True,更新失败=False</returns>\r\n";
            strFunction3 += "        public static bool UpdateData(" + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model,string strWhere)\r\n";
            strFunction3 += "        {\r\n";
            strFunction3 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction3 += "\r\n";
            strFunction3 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.UpdateData(" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model,strWhere);\r\n";
            strFunction3 += "        }\r\n";

            //31--根据SQL语句更新数据
            string strFunction31 = "\r\n";
            strFunction31 += "        /// <summary>\r\n";
            strFunction31 += "        /// 根据SQL语句更新数据\r\n";
            strFunction31 += "        /// </summary>\r\n";
            strFunction31 += "        /// <param name=\"strSql\">SQL语句</param>\r\n";
            strFunction31 += "        /// <returns>更新成功=True,更新失败=False</returns>\r\n";
            strFunction31 += "        public static bool UpdateDataBySQL(string strSql)\r\n";
            strFunction31 += "        {\r\n";
            strFunction31 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction31 += "\r\n";
            strFunction31 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.UpdateDataBySQL(strSql);\r\n";
            strFunction31 += "        }\r\n";

            //4--根据自定义条件查询表中一条数据
            string strFunction4 = "\r\n";
            strFunction4 += "        /// <summary>\r\n";
            strFunction4 += "        /// 根据自定义条件查询表中一条数据\r\n";
            strFunction4 += "        /// </summary>\r\n";
            strFunction4 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction4 += "        /// <returns>返回一条数据序列</returns>\r\n";
            strFunction4 += "        public static  " + strUpperTableName + "Model SelectOneByWhere(string strWhere)\r\n";
            strFunction4 += "        {\r\n";
            strFunction4 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction4 += "\r\n";
            strFunction4 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.SelectOneByWhere(strWhere);\r\n";
            strFunction4 += "        }\r\n";

            string strFunction14 = "\r\n";
            strFunction14 += "        /// <summary>\r\n";
            strFunction14 += "        /// 根据自定义条件查询表中一条数据 By SQL\r\n";
            strFunction14 += "        /// </summary>\r\n";
            strFunction14 += "        /// <param name=\"strSql\">SQL语句</param>\r\n";
            strFunction14 += "        /// <returns>返回一条数据序列</returns>\r\n";
            strFunction14 += "        public static  " + strUpperTableName + "Model SelectOneBySQL(string strSql)\r\n";
            strFunction14 += "        {\r\n";
            strFunction14 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction14 += "\r\n";
            strFunction14 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.SelectOneBySQL(strSql);\r\n";
            strFunction14 += "        }\r\n";

            //5--根据自定义条件查询表中所有数据
            string strFunction5 = "\r\n";
            strFunction5 += "        /// <summary>\r\n";
            strFunction5 += "        /// 根据自定义条件查询表中所有数据\r\n";
            strFunction5 += "        /// </summary>\r\n";
            strFunction5 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction5 += "        /// <returns>返回数据集</returns>\r\n";
            strFunction5 += "        public static List<" + strUpperTableName + "Model> SelectAllByWhere(string strWhere)\r\n";
            strFunction5 += "        {\r\n";
            strFunction5 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction5 += "\r\n";
            strFunction5 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.SelectAllByWhere(strWhere);\r\n";
            strFunction5 += "        }\r\n";

            string strFunction15 = "\r\n";
            strFunction15 += "        /// <summary>\r\n";
            strFunction15 += "        /// 根据自定义条件查询表中所有数据\r\n";
            strFunction15 += "        /// </summary>\r\n";
            strFunction15 += "        /// <param name=\"strSql\">SQL语句</param>\r\n";
            strFunction15 += "        /// <returns>返回数据集</returns>\r\n";
            strFunction15 += "        public static List<" + strUpperTableName + "Model> SelectAllBySQL(string strSql)\r\n";
            strFunction15 += "        {\r\n";
            strFunction15 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction15 += "\r\n";
            strFunction15 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.SelectAllBySQL(strSql);\r\n";
            strFunction15 += "        }\r\n";


            //6--根据条件获取表中任一字段的值
            string strFunction6 = "\r\n";
            strFunction6 += "        /// <summary>\r\n";
            strFunction6 += "        /// 根据条件获取表中任一字段的值\r\n";
            strFunction6 += "        /// </summary>\r\n";
            strFunction6 += "        /// <param name=\"strColumnName\">任一字段名</param>\r\n";
            strFunction6 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction6 += "        /// <returns>字段值</returns>\r\n";
            strFunction6 += "        public static string GetColumnValueByWhere(string strColumnName,string strWhere)\r\n";
            strFunction6 += "        {\r\n";
            strFunction6 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction6 += "\r\n";
            strFunction6 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.GetColumnValueByWhere(strColumnName,strWhere);\r\n";
            strFunction6 += "        }\r\n";


            //7--根据条件检测表中任一字段的值是否存在
            string strFunction7 = "\r\n";
            strFunction7 += "        /// <summary>\r\n";
            strFunction7 += "        /// 根据条件检测表中任一字段的值是否存在，如果不限定条件，即CheckColumnValueHasExist(\"\")\r\n";
            strFunction7 += "        /// </summary>\r\n";
            strFunction7 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction7 += "        /// <returns>返回是否存在该值，存在=True,不存在=False</returns>\r\n";
            strFunction7 += "        public static bool CheckColumnValueHasExist(string strWhere)\r\n";
            strFunction7 += "        {\r\n";
            strFunction7 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction7 += "\r\n";
            strFunction7 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.CheckColumnValueHasExist(strWhere);\r\n";
            strFunction7 += "        }\r\n";

            //8--根据条件统计数据字段累加结果
            string strFunction8 = "\r\n";
            strFunction8 += "        /// <summary>\r\n";
            strFunction8 += "        /// 根据条件统计数据字段累加结果\r\n";
            strFunction8 += "        /// </summary>\r\n";
            strFunction8 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction8 += "        /// <returns>返回值</returns>\r\n";
            strFunction8 += "        public static int CountNumByWhere(string strWhere)\r\n";
            strFunction8 += "        {\r\n";
            strFunction8 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction8 += "\r\n";
            strFunction8 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.CountNumByWhere(strWhere);\r\n";
            strFunction8 += "        }\r\n";

            //81--根据条件统计字段数值相加
            string strFunction81 = "\r\n";
            strFunction81 += "        /// <summary>\r\n";
            strFunction81 += "        /// 根据条件统计字段数值相加\r\n";
            strFunction81 += "        /// </summary>\r\n";
            strFunction81 += "        /// <param name=\"strKeyValue\">需要统计的字段名称</param>\r\n";
            strFunction81 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction81 += "        /// <returns>返回值</returns>\r\n";
            strFunction81 += "        public static int SumNumByWhere(string strKeyName,string strWhere)\r\n";
            strFunction81 += "        {\r\n";
            strFunction81 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction81 += "\r\n";
            strFunction81 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.SumNumByWhere(strKeyName,strWhere);\r\n";
            strFunction81 += "        }\r\n";

            //82--根据SQL语句统计某字段的数值相加或累加
            string strFunction82 = "\r\n";
            strFunction82 += "        /// <summary>\r\n";
            strFunction82 += "        /// 根据SQL语句统计某字段的数值相加或累加\r\n";
            strFunction82 += "        /// </summary>\r\n";
            strFunction82 += "        /// <param name=\"strSql\">SQL语句</param>\r\n";
            strFunction82 += "        /// <returns>返回值</returns>\r\n";
            strFunction82 += "        public static int CountOrSumNumBySQL(string strSql)\r\n";
            strFunction82 += "        {\r\n";
            strFunction82 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction82 += "\r\n";
            strFunction82 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.CountOrSumNumBySQL(strSql);\r\n";
            strFunction82 += "        }\r\n";

            string strFunction83 = "\r\n";
            strFunction83 += "        /// <summary>\r\n";
            strFunction83 += "        /// 根据条件统计字段数值相加\r\n";
            strFunction83 += "        /// </summary>\r\n";
            strFunction83 += "        /// <param name=\"strKeyValue\">需要统计的字段名称</param>\r\n";
            strFunction83 += "        /// <param name=\"strWhere\">条件</param>\r\n";
            strFunction83 += "        /// <returns>返回值</returns>\r\n";
            strFunction83 += "        public static double SumDoubleByWhere(string strKeyName,string strWhere)\r\n";
            strFunction83 += "        {\r\n";
            strFunction83 += "            " + strUpperTableName + "DAL " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL = new " + strUpperTableName + "DAL();\r\n";
            strFunction83 += "\r\n";
            strFunction83 += "            return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "DAL.SumDoubleByWhere(strKeyName,strWhere);\r\n";
            strFunction83 += "        }\r\n";

            strDALFunctionList = strFunction1 + strFunction11 + strFunction2+ strFunction3 + strFunction31 + strFunction4 + strFunction14 + strFunction5 + strFunction15 + strFunction6
                + strFunction7 + strFunction8 + strFunction81 + strFunction82 + strFunction83;

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
                    strColumnComment = CommonHelper.GetColumnKeyComment(strColumnComment);

                    if (strColumnKey== "PRI")
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
        public static string GetInsertParas(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                int nNum = 0;
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    strColumnComment = CommonHelper.GetColumnKeyComment(strColumnComment);

                    if (strColumnKey != "PRI")
                    {
                        strReturnValue += "paraArray[" + nNum + "] = mysqlHelper.InitMySqlParameter(\"@" + strColumnName + "\", ParameterDirection.Input, " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + ");\r\n";
                        nNum++;
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
                    if (strColumnKey != "PRI")
                    {
                        strReturnValue += " "+ strColumnName + " = @" + strColumnName + ",";
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
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName)+"Model."+CommonHelper.GetTableNameUpper(strColumnName)+" = dr.GetInt32(" + nNum + ");\r\n";
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = dr.IsDBNull(" + nNum + ") == true ? true : false;\r\n";
                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = dr.IsDBNull(" + nNum + ") == true ? DateTime.Now : dr.GetDateTime(" + nNum + ");\r\n";
                    }
                    else if (strDataType == "string")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = dr.IsDBNull(" + nNum + ") == true ? String.Empty : dr.GetString(" + nNum + ");\r\n";
                    }
                    else
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = dr.IsDBNull(" + nNum + ") == true ? String.Empty : dr.GetString(" + nNum + ");\r\n";
                    }

                    strReturnValue += strValue;

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
