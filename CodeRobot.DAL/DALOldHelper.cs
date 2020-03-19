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
    /// <summary>
    /// 数据处理类
    /// </summary>
    public class DALOldHelper
    {

        /// <summary>
        /// 根据表名创建DAL类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateDALClass(string strFilePath,string strProjectName, string strTableName)
        {
            string strClassName = CommonHelper.GetClassName(strTableName);//类名
            strClassName = CommonHelper.GetTableNameUpper(strClassName);

            Directory.CreateDirectory(strFilePath);
            StreamWriter sw = new StreamWriter(strFilePath + "\\" + strClassName + "DAL.cs");
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
            sw.WriteLine("/// <summary>");
            sw.WriteLine("/// 版权所有: Copyright © " + DateTime.Now.Year.ToString() + " Cloudin. 保留所有权利。");
            sw.WriteLine("/// 内容摘要: " + strClassName + "DAL");
            sw.WriteLine("/// 完成日期：" + DateTime.Now.ToString("yyyy年M月d日"));
            sw.WriteLine("/// 版    本：V1.0 ");
            sw.WriteLine("/// 作    者：Cloudin");
            sw.WriteLine("/// </summary>");
            sw.WriteLine("public class " + strClassName + "DAL");
            sw.WriteLine("{");
            sw.WriteLine("/// <summary>");
            sw.WriteLine("/// 定义数据连接");
            sw.WriteLine("/// </summary>");
            sw.WriteLine("private DBMySQLHelper mysqlHelper;");
            sw.WriteLine("");
            sw.WriteLine("/// <summary>");
            sw.WriteLine("/// 构造数据连接");
            sw.WriteLine("/// </summary>");
            sw.WriteLine("public " + strClassName + "DAL()");
            sw.WriteLine("{");
            sw.WriteLine("mysqlHelper = new DBMySQLHelper();");
            sw.WriteLine("}");
            sw.WriteLine("");
            string strFunctionList = GetDALFunctionList(strProjectName, strTableName);
            sw.WriteLine(strFunctionList);
            sw.WriteLine("}");
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
            string strAllColumnNameNotKey = GetAllColumnNameNotKey(strTableName);
            string strAllColumnNameAt = GetAllColumnNameForAt(strTableName);
            //获取表中字段数量
            int nNum = GetColumnNum(strTableName);
            //获取新增数据参数
            string strInsertPara = GetInsertParas(strTableName);
            string strInsertParaKey = GetInsertParas(strTableName,true);

            //1--创建新增数据函数
            string strFunction1 = "\r\n";
            strFunction1 += "/// <summary>\r\n";
            strFunction1 += "/// 新增一条数据\r\n";
            strFunction1 += "/// </summary>\r\n";
            strFunction1 += "/// <param name=\"" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model\">实体类</param>\r\n";
            strFunction1 += "/// <returns></returns>\r\n";
            strFunction1 += "public bool Insert" + strUpperTableName + "(" + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model)\r\n";
            strFunction1 += "{\r\n";
            strFunction1 += "string strSql = \"INSERT INTO " + strTableName + "("+ strAllColumnNameNotKey + ") \" +\r\n";
            strFunction1 += "\"VALUES("+ strAllColumnNameAt + ")\";\r\n";
            strFunction1 += "\r\n";
            strFunction1 += "MySqlParameter[] paraArray = new MySqlParameter["+ nNum + "];//修改时注意下标数要和下面的参数一致\r\n";
            strFunction1 += strInsertPara;
            strFunction1 += "\r\n";
            strFunction1 += "try\r\n";
            strFunction1 += "{\r\n";
            strFunction1 += "if (mysqlHelper.ExecuteNonQuery(strSql, CommandType.Text, paraArray) > 0)\r\n";
            strFunction1 += "{\r\n";
            strFunction1 += "return true;\r\n";
            strFunction1 += "}\r\n";
            strFunction1 += "return false;\r\n";
            strFunction1 += "}\r\n";
            strFunction1 += "catch (Exception ex)\r\n";
            strFunction1 += "{\r\n";
            strFunction1 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"新增一条数据，SQL语句：\" + strSql, \"Insert" + strUpperTableName + "\", true);\r\n";
            strFunction1 += "return false;\r\n";
            strFunction1 += "}\r\n";
            strFunction1 += "}\r\n";

            //2--删除一条数据
            string strFunction2 = "\r\n";
            strFunction2 += "/// <summary>\r\n";
            strFunction2 += "/// 删除一条数据\r\n";
            strFunction2 += "/// </summary>\r\n";
            strFunction2 += "/// <param name=\"" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model\">实体类</param>\r\n";
            strFunction2 += "/// <returns></returns>\r\n";
            strFunction2 += "public bool Delete" + strUpperTableName + "(" + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model)\r\n";
            strFunction2 += "{\r\n";
            strFunction2 += "string strSql = \"DELETE FROM " + strTableName + " WHERE "+ strPrimaryID + " = @" + strPrimaryID + "\";\r\n";
            strFunction2 += "\r\n";
            strFunction2 += "MySqlParameter[] paraArray = new MySqlParameter[1];\r\n";
            strFunction2 += "paraArray[0] = mysqlHelper.InitMySqlParameter(\"@" + strPrimaryID + "\", ParameterDirection.Input, "+ CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model."+ CommonHelper.GetTableNameUpper(strPrimaryID) +");\r\n";
            strFunction2 += "\r\n";
            strFunction2 += "try\r\n";
            strFunction2 += "{\r\n";
            strFunction2 += "if (mysqlHelper.ExecuteNonQuery(strSql, CommandType.Text, paraArray) > 0)\r\n";
            strFunction2 += "{\r\n";
            strFunction2 += "return true;\r\n";
            strFunction2 += "}\r\n";
            strFunction2 += "return false;\r\n";
            strFunction2 += "}\r\n";
            strFunction2 += "catch (Exception ex)\r\n";
            strFunction2 += "{\r\n";
            strFunction2 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"删除一条数据，SQL语句：\" + strSql, \"Delete" + strUpperTableName + "\", true);\r\n";
            strFunction2 += "return false;\r\n";
            strFunction2 += "}\r\n";
            strFunction2 += "}\r\n";

            //3--更新一条数据
            string strGetUpdateColumn = GetUpdateColumnName(strTableName);
            int numKey = nNum + 1;
            string strFunction3 = "\r\n";
            strFunction3 += "/// <summary>\r\n";
            strFunction3 += "/// 更新一条数据\r\n";
            strFunction3 += "/// </summary>\r\n";
            strFunction3 += "/// <param name=\"" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model\">实体类</param>\r\n";
            strFunction3 += "/// <returns></returns>\r\n";
            strFunction3 += "public bool Update" + strUpperTableName + "(" + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model)\r\n";
            strFunction3 += "{\r\n";
            strFunction3 += "string strSql = \"UPDATE  " + strTableName + "  \"+\r\n";
            strFunction3 += "\" SET " + strGetUpdateColumn + " \"+\r\n";
            strFunction3 += "\" WHERE " + strPrimaryID + " = @" + strPrimaryID + "\";\r\n";
            strFunction3 += "\r\n";
            strFunction3 += "MySqlParameter[] paraArray = new MySqlParameter[" + numKey + "];//修改时注意下标数要和下面的参数一致\r\n";
            strFunction3 += strInsertParaKey;
            strFunction3 += "\r\n";
            strFunction3 += "try\r\n";
            strFunction3 += "{\r\n";
            strFunction3 += "if (mysqlHelper.ExecuteNonQuery(strSql, CommandType.Text, paraArray) > 0)\r\n";
            strFunction3 += "{\r\n";
            strFunction3 += "return true;\r\n";
            strFunction3 += "}\r\n";
            strFunction3 += "return false;\r\n";
            strFunction3 += "}\r\n";
            strFunction3 += "catch (Exception ex)\r\n";
            strFunction3 += "{\r\n";
            strFunction3 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"更新一条数据，SQL语句：\" + strSql, \"Update" + strUpperTableName + "\", true);\r\n";
            strFunction3 += "return false;\r\n";
            strFunction3 += "}\r\n";
            strFunction3 += "}\r\n";

            //4--根据主键更新表中任一字段自增1
            string strFunction4 = "\r\n";
            strFunction4 += "/// <summary>\r\n";
            strFunction4 += "/// 根据主键更新表中任一字段自增1\r\n";
            strFunction4 += "/// </summary>\r\n";
            strFunction4 += "/// <param name=\"n" + CommonHelper.GetTableNameUpper(strPrimaryID) + "\">主键ID</param>\r\n";
            strFunction4 += "/// <param name=\"strColumnName\">表中任一字段（必须是整型）</param>\r\n";
            strFunction4 += "/// <returns></returns>\r\n";
            strFunction4 += "public bool UpdateAnyColumnValueIncrement(int n" + CommonHelper.GetTableNameUpper(strPrimaryID) + ", string strColumnName)\r\n";
            strFunction4 += "{\r\n";
            strFunction4 += "string strSql = \"UPDATE " + strTableName + " SET \" + strColumnName + \" = \" + strColumnName + \" + 1 \" +\r\n";
            strFunction4 += "\" WHERE " + strPrimaryID + " = @" + strPrimaryID + "\";\r\n";
            strFunction4 += "\r\n";
            strFunction4 += "MySqlParameter[] paraArray = new MySqlParameter[1];\r\n";
            strFunction4 += "paraArray[0] = mysqlHelper.InitMySqlParameter(\"@" + strPrimaryID + "\", ParameterDirection.Input, n" + CommonHelper.GetTableNameUpper(strPrimaryID) + ");\r\n";
            strFunction4 += "\r\n";
            strFunction4 += "try\r\n";
            strFunction4 += "{\r\n";
            strFunction4 += "if (mysqlHelper.ExecuteNonQuery(strSql, CommandType.Text, paraArray) > 0)\r\n";
            strFunction4 += "{\r\n";
            strFunction4 += "return true;\r\n";
            strFunction4 += "}\r\n";
            strFunction4 += "return false;\r\n";
            strFunction4 += "}\r\n";
            strFunction4 += "catch (Exception ex)\r\n";
            strFunction4 += "{\r\n";
            strFunction4 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据主键更新表中任一字段自增1，SQL语句：\" + strSql, \"UpdateAnyColumnValueIncrement\", true);\r\n";
            strFunction4 += "return false;\r\n";
            strFunction4 += "}\r\n";
            strFunction4 += "}\r\n";

            //5--更新表中任一字段的值
            string strFunction5 = "\r\n";
            strFunction5 += "/// <summary>\r\n";
            strFunction5 += "/// 更新表中任一字段的值\r\n";
            strFunction5 += "/// </summary>\r\n";
            strFunction5 += "/// <param name=\"n" + CommonHelper.GetTableNameUpper(strPrimaryID) + "\">主键ID</param>\r\n";
            strFunction5 += "/// <param name=\"strColumnName\">表中任一字段（必须是整型）</param>\r\n";
            strFunction5 += "/// <param name=\"strColumnValue\">表中任一字段名的值</param>\r\n";
            strFunction5 += "/// <returns></returns>\r\n";
            strFunction5 += "public bool UpdateAnyColumnValue(int n" + CommonHelper.GetTableNameUpper(strPrimaryID) + ", string strColumnName, string strColumnValue)\r\n";
            strFunction5 += "{\r\n";
            strFunction5 += "string strSql = \"UPDATE " + strTableName + " SET \" + strColumnName + \"= \" + strColumnValue + \" \" +\r\n";
            strFunction5 += "\" WHERE " + strPrimaryID + " = @" + strPrimaryID + "\";\r\n";
            strFunction5 += "\r\n";
            strFunction5 += "MySqlParameter[] paraArray = new MySqlParameter[1];\r\n";
            strFunction5 += "paraArray[0] = mysqlHelper.InitMySqlParameter(\"@" + strPrimaryID + "\", ParameterDirection.Input, n" + CommonHelper.GetTableNameUpper(strPrimaryID) + ");\r\n";
            strFunction5 += "\r\n";
            strFunction5 += "try\r\n";
            strFunction5 += "{\r\n";
            strFunction5 += "if (mysqlHelper.ExecuteNonQuery(strSql, CommandType.Text, paraArray) > 0)\r\n";
            strFunction5 += "{\r\n";
            strFunction5 += "return true;\r\n";
            strFunction5 += "}\r\n";
            strFunction5 += "return false;\r\n";
            strFunction5 += "}\r\n";
            strFunction5 += "catch (Exception ex)\r\n";
            strFunction5 += "{\r\n";
            strFunction5 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"更新表中任一字段的值，SQL语句：\" + strSql, \"UpdateAnyColumnValue\", true);\r\n";
            strFunction5 += "return false;\r\n";
            strFunction5 += "}\r\n";
            strFunction5 += "}\r\n";

            //6--查询表中一条数据
            string strFunction6 = "\r\n";
            strFunction6 += "/// <summary>\r\n";
            strFunction6 += "/// 查询表中一条数据\r\n";
            strFunction6 += "/// </summary>\r\n";
            strFunction6 += "/// <param name=\"n" + CommonHelper.GetTableNameUpper(strPrimaryID) + "\">主键ID</param>\r\n";
            strFunction6 += "/// <returns></returns>\r\n";
            strFunction6 += "public " + strUpperTableName + "Model Select" + strUpperTableName + "(int n" + CommonHelper.GetTableNameUpper(strPrimaryID) + ")\r\n";
            strFunction6 += "{\r\n";
            strFunction6 += "string strSql = \"SELECT "+ strAllColumnName + " \" +\r\n";
            strFunction6 += "\" FROM "+strTableName+" \" +\r\n";
            strFunction6 += "\" WHERE " + strPrimaryID + " = @" + strPrimaryID + "\";\r\n";
            strFunction6 += "\r\n";
            strFunction6 += "MySqlParameter[] paraArray = new MySqlParameter[1];\r\n";
            strFunction6 += "paraArray[0] = mysqlHelper.InitMySqlParameter(\"@" + strPrimaryID + "\", ParameterDirection.Input, n" + CommonHelper.GetTableNameUpper(strPrimaryID) + ");\r\n";
            strFunction6 += "\r\n";
            strFunction6 += "" + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model = null;\r\n";
            strFunction6 += "\r\n";
            strFunction6 += "try\r\n";
            strFunction6 += "{\r\n";
            strFunction6 += "using (MySqlDataReader dr = mysqlHelper.ExecuteReader(strSql, CommandType.Text, paraArray))\r\n";
            strFunction6 += "{\r\n";
            strFunction6 += "if (dr.Read())\r\n";
            strFunction6 += "{\r\n";
            strFunction6 += "" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model = new " + strUpperTableName + "Model();\r\n";
            strFunction6 += "\r\n";
            strFunction6 += GetSelectPara(strTableName) + "\r\n";
            strFunction6 += "}\r\n";
            strFunction6 += "}\r\n";
            strFunction6 += "}\r\n";
            strFunction6 += "catch (Exception ex)\r\n";
            strFunction6 += "{\r\n";
            strFunction6 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"查询表中一条数据，SQL语句：\" + strSql, \"Select" + strUpperTableName + "\", true);\r\n";
            strFunction6 += "}\r\n";
            strFunction6 += "\r\n";
            strFunction6 += "return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model;\r\n";
            strFunction6 += "}\r\n";

            //7--查询表中所有数据
            string strFunction7 = "\r\n";
            strFunction7 += "/// <summary>\r\n";
            strFunction7 += "/// 查询表中所有数据\r\n";
            strFunction7 += "/// </summary>\r\n";
            strFunction7 += "/// <param name=\"strWhere\">查询条件</param>\r\n";
            strFunction7 += "/// <returns></returns>\r\n";
            strFunction7 += "public List<"+strUpperTableName+ "Model> SelectAll" + strUpperTableName + "(string strWhere)\r\n";
            strFunction7 += "{\r\n";
            strFunction7 += "string strSql = \"SELECT " + strAllColumnName + " \" +\r\n";
            strFunction7 += "\" FROM " + strTableName + " \" + strWhere;\r\n";
            strFunction7 += "\r\n";
            strFunction7 += "List<" + strUpperTableName + "Model> list = new List<" + strUpperTableName + "Model>();\r\n";
            strFunction7 += "\r\n";
            strFunction7 += "try\r\n";
            strFunction7 += "{\r\n";
            strFunction7 += "using (MySqlDataReader dr = mysqlHelper.ExecuteReader(strSql, CommandType.Text))\r\n";
            strFunction7 += "{\r\n";
            strFunction7 += "while (dr.Read())\r\n";
            strFunction7 += "{\r\n";
            strFunction7 += strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model = new " + strUpperTableName + "Model();\r\n";
            strFunction7 += "\r\n";
            strFunction7 += GetSelectPara(strTableName) + "\r\n";
            strFunction7 += "\r\n";
            strFunction7 += "list.Add("+CommonHelper.GetTableNameFirtWordLower(strTableName)+"Model);\r\n";
            strFunction7 += "}\r\n";
            strFunction7 += "}\r\n";
            strFunction7 += "}\r\n";
            strFunction7 += "catch (Exception ex)\r\n";
            strFunction7 += "{\r\n";
            strFunction7 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"查询表中所有数据，SQL语句：\" + strSql, \"SelectAll\", true);\r\n";
            strFunction7 += "}\r\n";
            strFunction7 += "\r\n";
            strFunction7 += "return list;\r\n";
            strFunction7 += "}\r\n";

            //8--根据主键检测是否该数据已存在
            string strFunction8 = "\r\n";
            strFunction8 += "/// <summary>\r\n";
            strFunction8 += "/// 根据主键检测是否该数据已存在\r\n";
            strFunction8 += "/// </summary>\r\n";
            strFunction8 += "/// <param name=\"n" + CommonHelper.GetTableNameUpper(strPrimaryID) + "\">主键ID</param>\r\n";
            strFunction8 += "/// <returns></returns>\r\n";
            strFunction8 += "public bool Check" + CommonHelper.GetTableNameUpper(strPrimaryID) + "HasExist(int n" + CommonHelper.GetTableNameUpper(strPrimaryID) + ")\r\n";
            strFunction8 += "{\r\n";
            strFunction8 += "string strSql = \"SELECT " + strPrimaryID + " \" +\r\n";
            strFunction8 += "\" FROM "+strTableName+" \" +\r\n";
            strFunction8 += "\" WHERE " + strPrimaryID + " = @" + strPrimaryID + "\";\r\n";
            strFunction8 += "\r\n";
            strFunction8 += "MySqlParameter[] paraArray = new MySqlParameter[1];\r\n";
            strFunction8 += "paraArray[0] = mysqlHelper.InitMySqlParameter(\"@" + strPrimaryID + "\", ParameterDirection.Input, n" + CommonHelper.GetTableNameUpper(strPrimaryID) + ");\r\n";
            strFunction8 += "\r\n";
            strFunction8 += "try\r\n";
            strFunction8 += "{\r\n";
            strFunction8 += "if (mysqlHelper.ExecuteNonQuery(strSql, CommandType.Text, paraArray) > 0)\r\n";
            strFunction8 += "{\r\n";
            strFunction8 += "return true;\r\n";
            strFunction8 += "}\r\n";
            strFunction8 += "return false;\r\n";
            strFunction8 += "}\r\n";
            strFunction8 += "catch (Exception ex)\r\n";
            strFunction8 += "{\r\n";
            strFunction8 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据主键检测是否该数据已存在，SQL语句：\" + strSql, \"UpdateAnyColumnValue\", true);\r\n";
            strFunction8 += "return false;\r\n";
            strFunction8 += "}\r\n";
            strFunction8 += "}\r\n";

            //9--根据表中某个字段，并检测该字段的值是否存在，除了主键
            string strFunction9 = "\r\n";
            strFunction9 += "/// <summary>\r\n";
            strFunction9 += "/// 根据表中某个字段，并检测该字段的值是否存在，除了主键\r\n";
            strFunction9 += "/// </summary>\r\n";
            strFunction9 += "/// <param name=\"strColumnName\">字段名</param>\r\n";
            strFunction9 += "/// <param name=\"strColumnValue\">字段值</param>\r\n";
            strFunction9 += "/// <returns></returns>\r\n";
            strFunction9 += "public bool CheckColumnValueHasExist(string strColumnName, string strColumnValue)\r\n";
            strFunction9 += "{\r\n";
            strFunction9 += "string strSql = \"SELECT " + strPrimaryID + " \" +\r\n";
            strFunction9 += "\" FROM " + strTableName + " \" +\r\n";
            strFunction9 += "\" WHERE \" + strColumnName + \" = @\" + strColumnName + \"\";\r\n";
            strFunction9 += "\r\n";
            strFunction9 += "MySqlParameter[] paraArray = new MySqlParameter[1];\r\n";
            strFunction9 += "paraArray[0] = mysqlHelper.InitMySqlParameter(\"@\" + strColumnName + \"\", ParameterDirection.Input, strColumnValue);\r\n";
            strFunction9 += "\r\n";
            strFunction9 += "try\r\n";
            strFunction9 += "{\r\n";
            strFunction9 += "if (mysqlHelper.ExecuteNonQuery(strSql, CommandType.Text, paraArray) > 0)\r\n";
            strFunction9 += "{\r\n";
            strFunction9 += "return true;\r\n";
            strFunction9 += "}\r\n";
            strFunction9 += "return false;\r\n";
            strFunction9 += "}\r\n";
            strFunction9 += "catch (Exception ex)\r\n";
            strFunction9 += "{\r\n";
            strFunction9 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据表中某个字段，并检测该字段的值是否存在，除了主键，SQL语句：\" + strSql, \"UpdateAnyColumnValue\", true);\r\n";
            strFunction9 += "return false;\r\n";
            strFunction9 += "}\r\n";
            strFunction9 += "}\r\n";

            //10--根据表中主键获取某个字段的值
            string strFunction10 = "\r\n";
            strFunction10 += "/// <summary>\r\n";
            strFunction10 += "/// 根据表中主键获取某个字段的值\r\n";
            strFunction10 += "/// </summary>\r\n";
            strFunction10 += "/// <param name=\"n" + CommonHelper.GetTableNameUpper(strPrimaryID) + "\">主键ID</param>\r\n";
            strFunction10 += "/// <param name=\"strColumnName\">字段名</param>\r\n";
            strFunction10 += "/// <returns></returns>\r\n";
            strFunction10 += "public string GetColumnValueByPrimaryID(int n" + CommonHelper.GetTableNameUpper(strPrimaryID) + ", string strColumnName)\r\n";
            strFunction10 += "{\r\n";
            strFunction10 += "string strReturnValue = \"\";\r\n";
            strFunction10 += "\r\n";
            strFunction10 += "string strSql = \"SELECT \" + strColumnName + \" \" +\r\n";
            strFunction10 += "\" FROM " + strTableName + " \" +\r\n";
            strFunction10 += "\" WHERE " + strPrimaryID + " = @" + strPrimaryID + "\";\r\n";
            strFunction10 += "\r\n";
            strFunction10 += "MySqlParameter[] paraArray = new MySqlParameter[1];\r\n";
            strFunction10 += "paraArray[0] = mysqlHelper.InitMySqlParameter(\"@" + strPrimaryID + "\", ParameterDirection.Input, n" + CommonHelper.GetTableNameUpper(strPrimaryID) + ");\r\n";
            strFunction10 += "\r\n";
            strFunction10 += "try\r\n";
            strFunction10 += "{\r\n";
            strFunction10 += "using (MySqlDataReader dr = mysqlHelper.ExecuteReader(strSql, CommandType.Text, paraArray))\r\n";
            strFunction10 += "{\r\n";
            strFunction10 += " if (dr.Read())\r\n";
            strFunction10 += "{\r\n";
            strFunction10 += "strReturnValue = dr.IsDBNull(0) == true ? String.Empty : dr.GetString(0);\r\n";
            strFunction10 += "}\r\n";
            strFunction10 += "}\r\n";
            strFunction10 += "}\r\n";
            strFunction10 += "catch (Exception ex)\r\n";
            strFunction10 += "{\r\n";
            strFunction10 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据表中主键获取某个字段的值，SQL语句：\" + strSql, \"UpdateAnyColumnValue\", true);\r\n";
            strFunction10 += "}\r\n";
            strFunction10 += "\r\n";
            strFunction10 += "return strReturnValue;\r\n";
            strFunction10 += "}\r\n";

            //11--根据表中任一字段获取其中某个字段的值
            string strFunction11 = "\r\n";
            strFunction11 += "/// <summary>\r\n";
            strFunction11 += "/// 根据表中任一字段获取其中某个字段的值\r\n";
            strFunction11 += "/// </summary>\r\n";
            strFunction11 += "/// <param name=\"strTargetColumnName\">获取目标字段的值</param>\r\n";
            strFunction11 += "/// <param name=\"strColumnName\">字段名</param>\r\n";
            strFunction11 += "/// <param name=\"strAnyColumnValue\">任一字段值</param>\r\n";
            strFunction11 += "/// <returns></returns>\r\n";
            strFunction11 += "public string GetTargetColumnValueByAnyColumnName(string strTargetColumnName, string strAnyColumnName, string strAnyColumnValue)\r\n";
            strFunction11 += "{\r\n";
            strFunction11 += "string strReturnValue = \"\";\r\n";
            strFunction11 += "\r\n";
            strFunction11 += "string strSql = \"SELECT \" + strTargetColumnName + \" \" +\r\n";
            strFunction11 += "\" FROM " + strTableName + " \" +\r\n";
            strFunction11 += "\" WHERE \" + strAnyColumnName + \" = @\" + strAnyColumnName + \"\";\r\n";
            strFunction11 += "\r\n";
            strFunction11 += "MySqlParameter[] paraArray = new MySqlParameter[1];\r\n";
            strFunction11 += "paraArray[0] = mysqlHelper.InitMySqlParameter(\"@" + strPrimaryID + "\", ParameterDirection.Input, strAnyColumnValue);\r\n";
            strFunction11 += "\r\n";
            strFunction11 += "try\r\n";
            strFunction11 += "{\r\n";
            strFunction11 += "using (MySqlDataReader dr = mysqlHelper.ExecuteReader(strSql, CommandType.Text, paraArray))\r\n";
            strFunction11 += "{\r\n";
            strFunction11 += " if (dr.Read())\r\n";
            strFunction11 += "{\r\n";
            strFunction11 += "strReturnValue = dr.IsDBNull(0) == true ? String.Empty : dr.GetString(0);\r\n";
            strFunction11 += "}\r\n";
            strFunction11 += "}\r\n";
            strFunction11 += "}\r\n";
            strFunction11 += "catch (Exception ex)\r\n";
            strFunction11 += "{\r\n";
            strFunction11 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据表中任一字段获取其中某个字段的值，SQL语句：\" + strSql, \"UpdateAnyColumnValue\", true);\r\n";
            strFunction11 += "}\r\n";
            strFunction11 += "\r\n";
            strFunction11 += "return strReturnValue;\r\n";
            strFunction11 += "}\r\n";

            //12--获取表中最大的主键值
            string strFunction12 = "\r\n";
            strFunction12 += "/// <summary>\r\n";
            strFunction12 += "/// 获取表中最大的主键值\r\n";
            strFunction12 += "/// </summary>\r\n";
            strFunction12 += "/// <returns></returns>\r\n";
            strFunction12 += "public string GetMaxPrimaryID()\r\n";
            strFunction12 += "{\r\n";
            strFunction12 += "string strReturnValue = \"\";\r\n";
            strFunction12 += "\r\n";
            strFunction12 += "string strSql = \"SELECT " + strPrimaryID + " \" +\r\n";
            strFunction12 += "\" FROM " + strTableName + " \" +\r\n";
            strFunction12 += "\" ORDER BY " + strPrimaryID + " DESC LIMIT 0,1\";\r\n";
            strFunction12 += "\r\n";
            strFunction12 += "try\r\n";
            strFunction12 += "{\r\n";
            strFunction12 += "using (MySqlDataReader dr = mysqlHelper.ExecuteReader(strSql, CommandType.Text, null))\r\n";
            strFunction12 += "{\r\n";
            strFunction12 += " if (dr.Read())\r\n";
            strFunction12 += "{\r\n";
            strFunction12 += "strReturnValue = dr.IsDBNull(0) == true ? String.Empty : dr.GetString(0);\r\n";
            strFunction12 += "}\r\n";
            strFunction12 += "}\r\n";
            strFunction12 += "}\r\n";
            strFunction12 += "catch (Exception ex)\r\n";
            strFunction12 += "{\r\n";
            strFunction12 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"获取表中最大的主键值，SQL语句：\" + strSql, \"GetMaxPrimaryID\", true);\r\n";
            strFunction12 += "}\r\n";
            strFunction12 += "\r\n";
            strFunction12 += "return strReturnValue;\r\n";
            strFunction12 += "}\r\n";

            //13--根据自定义提交统计数据
            string strFunction13 = "\r\n";
            strFunction13 += "/// <summary>\r\n";
            strFunction13 += "/// 根据自定义提交统计数据\r\n";
            strFunction13 += "/// </summary>\r\n";
            strFunction13 += "/// <param name=\"strWhere\">条件</param>\r\n";
            strFunction13 += "/// <returns></returns>\r\n";
            strFunction13 += "public int CountColumnValueByWhere(string strWhere)\r\n";
            strFunction13 += "{\r\n";
            strFunction13 += "int num = 0;\r\n";
            strFunction13 += "\r\n";
            strFunction13 += "string strSql = \"SELECT count(" + strPrimaryID + ") as num \" +\r\n";
            strFunction13 += "\" FROM " + strTableName + " \" + strWhere;\r\n";
            strFunction13 += "try\r\n";
            strFunction13 += "{\r\n";
            strFunction13 += "using (MySqlDataReader dr = mysqlHelper.ExecuteReader(strSql, CommandType.Text))\r\n";
            strFunction13 += "{\r\n";
            strFunction13 += " if (dr.Read())\r\n";
            strFunction13 += "{\r\n";
            strFunction13 += "num = dr.GetInt32(0);\r\n";
            strFunction13 += "}\r\n";
            strFunction13 += "}\r\n";
            strFunction13 += "}\r\n";
            strFunction13 += "catch (Exception ex)\r\n";
            strFunction13 += "{\r\n";
            strFunction13 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据自定义提交统计数据，SQL语句：\" + strSql, \"CountColumnValueByWhere\", true);\r\n";
            strFunction13 += "}\r\n";
            strFunction13 += "\r\n";
            strFunction13 += "return num;\r\n";
            strFunction13 += "}\r\n";

            //14--根据自定义条件删除某条数据
            string strFunction14 = "\r\n";
            strFunction14 += "/// <summary>\r\n";
            strFunction14 += "/// 根据自定义条件删除某条数据\r\n";
            strFunction14 += "/// </summary>\r\n";
            strFunction14 += "/// <param name=\"strWhere\">条件</param>\r\n";
            strFunction14 += "/// <returns></returns>\r\n";
            strFunction14 += "public bool Delete" + strUpperTableName + "ByWhere(string strWhere)\r\n";
            strFunction14 += "{\r\n";
            strFunction14 += "\r\n";
            strFunction14 += "string strSql = \"DELETE FROM " + strTableName + " \" + strWhere;\r\n";
            strFunction14 += "try\r\n";
            strFunction14 += "{\r\n";
            strFunction14 += "if (mysqlHelper.ExecuteNonQuery(strSql, CommandType.Text, null) > 0)\r\n";
            strFunction14 += "{\r\n";
            strFunction14 += "return true;\r\n";
            strFunction14 += "}\r\n";
            strFunction14 += "return false;\r\n";
            strFunction14 += "}\r\n";
            strFunction14 += "catch (Exception ex)\r\n";
            strFunction14 += "{\r\n";
            strFunction14 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据自定义条件删除某条数据，SQL语句：\" + strSql, \"DeleteFavoritesByWhere\", true);\r\n";
            strFunction14 += "return false;\r\n";
            strFunction14 += "}\r\n";
            strFunction14 += "\r\n";
            strFunction14 += "}\r\n";

            //15--根据自定义条件检测是否存在
            string strFunction15 = "\r\n";
            strFunction15 += "/// <summary>\r\n";
            strFunction15 += "/// 根据自定义条件检测是否存在\r\n";
            strFunction15 += "/// </summary>\r\n";
            strFunction15 += "/// <param name=\"strWhere\">条件</param>\r\n";
            strFunction15 += "/// <returns></returns>\r\n";
            strFunction15 += "public bool CheckColumnValueHasExistByWhere(string strWhere)\r\n";
            strFunction15 += "{\r\n";
            strFunction15 += "\r\n";
            strFunction15 += "string strSql = \"SELECT " + strPrimaryID + " \" +\r\n";
            strFunction15 += "\" FROM " + strTableName + " \" + strWhere;\r\n";
            strFunction15 += "try\r\n";
            strFunction15 += "{\r\n";
            strFunction15 += "using (MySqlDataReader dr = mysqlHelper.ExecuteReader(strSql, CommandType.Text))\r\n";
            strFunction15 += "{\r\n";
            strFunction15 += " if (dr.Read())\r\n";
            strFunction15 += "{\r\n";
            strFunction15 += "return true;\r\n";
            strFunction15 += "}\r\n";
            strFunction15 += "return false;\r\n";
            strFunction15 += "}\r\n";
            strFunction15 += "}\r\n";
            strFunction15 += "catch (Exception ex)\r\n";
            strFunction15 += "{\r\n";
            strFunction15 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据自定义条件检测是否存在，SQL语句：\" + strSql, \"CountColumnValueByWhere\", true);\r\n";
            strFunction15 += "return false;\r\n";
            strFunction15 += "}\r\n";
            strFunction15 += "\r\n";
            strFunction15 += "}\r\n";

            //15--根据自定义条件更新数据
            string strFunction16 = "\r\n";
            strFunction16 += "/// <summary>\r\n";
            strFunction16 += "/// 根据自定义条件更新数据\r\n";
            strFunction16 += "/// </summary>\r\n";
            strFunction16 += "/// <param name=\"" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model\">实体类</param>\r\n";
            strFunction16 += "/// <param name=\"strWhere\">条件</param>\r\n";
            strFunction16 += "/// <returns></returns>\r\n";
            strFunction16 += "public bool Update" + strUpperTableName + "ByWhere(" + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model,string strWhere)\r\n";
            strFunction16 += "{\r\n";
            strFunction16 += "string strSql = \"UPDATE  " + strTableName + "  \"+\r\n";
            strFunction16 += "\" SET " + strGetUpdateColumn + " \"+\r\n";
            strFunction16 += "\" \" + strWhere;\r\n";
            strFunction16 += "\r\n";
            strFunction16 += "MySqlParameter[] paraArray = new MySqlParameter[" + nNum + "];//修改时注意下标数要和下面的参数一致\r\n";
            strFunction16 += strInsertPara;
            strFunction16 += "\r\n";
            strFunction16 += "try\r\n";
            strFunction16 += "{\r\n";
            strFunction16 += "if (mysqlHelper.ExecuteNonQuery(strSql, CommandType.Text, paraArray) > 0)\r\n";
            strFunction16 += "{\r\n";
            strFunction16 += "return true;\r\n";
            strFunction16 += "}\r\n";
            strFunction16 += "return false;\r\n";
            strFunction16 += "}\r\n";
            strFunction16 += "catch (Exception ex)\r\n";
            strFunction16 += "{\r\n";
            strFunction16 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据自定义条件更新数据，SQL语句：\" + strSql, \"Update" + strUpperTableName + "\", true);\r\n";
            strFunction16 += "return false;\r\n";
            strFunction16 += "}\r\n";
            strFunction16 += "}\r\n";


            //17--根据自定义条件查询表中一条数据
            string strFunction17 = "\r\n";
            strFunction17 += "/// <summary>\r\n";
            strFunction17 += "/// 根据自定义条件查询表中一条数据\r\n";
            strFunction17 += "/// </summary>\r\n";
            strFunction17 += "/// <param name=\"strWhere\">条件</param>\r\n";
            strFunction17 += "/// <returns></returns>\r\n";
            strFunction17 += "public " + strUpperTableName + "Model Select" + strUpperTableName + "ByWhere(string strWhere)\r\n";
            strFunction17 += "{\r\n";
            strFunction17 += "string strSql = \"SELECT " + strAllColumnName + " \" +\r\n";
            strFunction17 += "\" FROM " + strTableName + " \" +\r\n";
            strFunction17 += "\" \" + strWhere;\r\n";
            strFunction17 += "\r\n";
            strFunction17 += "" + strUpperTableName + "Model " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model = null;\r\n";
            strFunction17 += "\r\n";
            strFunction17 += "try\r\n";
            strFunction17 += "{\r\n";
            strFunction17 += "using (MySqlDataReader dr = mysqlHelper.ExecuteReader(strSql, CommandType.Text))\r\n";
            strFunction17 += "{\r\n";
            strFunction17 += "if (dr.Read())\r\n";
            strFunction17 += "{\r\n";
            strFunction17 += "" + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model = new " + strUpperTableName + "Model();\r\n";
            strFunction17 += "\r\n";
            strFunction17 += GetSelectPara(strTableName) + "\r\n";
            strFunction17 += "}\r\n";
            strFunction17 += "}\r\n";
            strFunction17 += "}\r\n";
            strFunction17 += "catch (Exception ex)\r\n";
            strFunction17 += "{\r\n";
            strFunction17 += "" + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "DAL), ex, \"根据自定义条件查询表中一条数据，SQL语句：\" + strSql, \"Select" + strUpperTableName + "ByWhere\", true);\r\n";
            strFunction17 += "}\r\n";
            strFunction17 += "\r\n";
            strFunction17 += "return " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model;\r\n";
            strFunction17 += "}\r\n";

            strDALFunctionList = strFunction1 + strFunction2+ strFunction3 + strFunction4 + strFunction5 + strFunction6 + 
                strFunction7 + strFunction8 + strFunction9 + strFunction10 + strFunction11 + strFunction12 + strFunction13 + 
                strFunction14 + strFunction15 + strFunction16 + strFunction17;

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
                    if (strColumnKey != "PRI")
                    {
                        strReturnValue += "paraArray[" + nNum + "] = mysqlHelper.InitMySqlParameter(\"@" + strColumnName + "\", ParameterDirection.Input, " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + ",false);\r\n";
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
        /// 获取新增数据参数:包含主键
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <param name="hasKey">是否包含主键</param>
        /// <returns></returns>
        public static string GetInsertParas(string strTableName,bool hasKey)
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

                    strReturnValue += "paraArray[" + nNum + "] = mysqlHelper.InitMySqlParameter(\"@" + strColumnName + "\", ParameterDirection.Input, " + CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + ",false);\r\n";
                    nNum++;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取新增数据参数:包含主键", "GetInsertParas",false);
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
                    else if (strDataType == "float")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = dr.GetDouble(" + nNum + ");\r\n";
                    }
                    else if (strDataType == "double")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = dr.GetDouble(" + nNum + ");\r\n";
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = dr.GetDouble(" + nNum + ");\r\n";
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = CommonHelper.GetTableNameFirtWordLower(strTableName) + "Model." + CommonHelper.GetTableNameUpper(strColumnName) + " = dr.GetBoolean(" + nNum + ");\r\n";
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
