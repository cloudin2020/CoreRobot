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
    /// Restful API接口类
    /// </summary>
    public class RestAPIHelper
    {

        /// <summary>
        /// 根据表名创建RestAPIHelper类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateRestAPIClass(string strFilePath,string strProjectName, string strTableName,string strTableComment)
        {
            //读取版权信息
            CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
            string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
            string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
            string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
            string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
            string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

            string strClassName = CommonHelper.GetClassName(strTableName);//类名
            strClassName = CommonHelper.GetTableNameUpper(strClassName);//首字母大写

            Directory.CreateDirectory(strFilePath);
            StreamWriter sw = new StreamWriter(strFilePath + "\\" + strClassName + "Controller.cs", false, Encoding.GetEncoding("utf-8"));
            sw.WriteLine("using System;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine("using System.Net.Http;");
            sw.WriteLine("using System.Web.Http;");
            sw.WriteLine("");
            sw.WriteLine("namespace " + CommonHelper.GetTableNameUpper(strProjectName) + ".RestAPI.Controllers");
            sw.WriteLine("{");
            sw.WriteLine("");
            sw.WriteLine("    /// <summary>");
            sw.WriteLine("    /// 版权所有: Copyright © " + DateTime.Now.Year.ToString() + " " + strCompany + ". 保留所有权利。");
            sw.WriteLine("    /// 内容摘要: " + strTableComment);
            sw.WriteLine("    /// 完成日期：" + DateTime.Now.ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 版    本：V" + strVersion + "." + strCode + " ");
            sw.WriteLine("    /// 作    者：" + strAuthor);
            sw.WriteLine("    /// </summary>");
            sw.WriteLine("    //[Authorize]");
            sw.WriteLine("    [RoutePrefix(\"api\")]");
            sw.WriteLine("    public class " + strClassName + "Controller : ApiController");
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
            strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);//首字母大写
            string strRouteDir = strUpperTableName.ToLower();//路由路径，小写

            //获取主键ID
            string strPrimaryID = GetPrimaryKey(strTableName);
            //获取新增数据参数
            string strInsertPara = GetInsertParas(strTableName);

            //1--获取所有数据
            string strFunction1 = "\r\n";
            strFunction1 += "        /// <summary>\r\n";
            strFunction1 += "        /// 根据条件获取分页列表数据\r\n";
            strFunction1 += "        /// </summary>\r\n";
            strFunction1 += "        /// <param name=\"page\">分页</param>\r\n";
            strFunction1 += "        /// <returns>返回JSON数据</returns>\r\n";
            strFunction1 += "        [Route(\""+ strRouteDir + "/page/{page:int}\")]\r\n";
            strFunction1 += "        [HttpGet]\r\n";
            strFunction1 += "        public HttpResponseMessage GetAll(int page)\r\n";
            strFunction1 += "        {\r\n";
            strFunction1 += "            HttpResponseMessage message = null;\r\n";
            strFunction1 += "\r\n";
            strFunction1 += "            string strJSONDataList = \"\";\r\n";
            strFunction1 += "            string strFormatJSON = \"\";\r\n";
            strFunction1 += "\r\n";
            strFunction1 += "            try\r\n";
            strFunction1 += "            {\r\n";
            strFunction1 += "                int rowCount = 0;\r\n";
            strFunction1 += "                int pageSize = "+strProjectName+".API.Common.Config.nPageSize;\r\n";
            strFunction1 += "                int nLimitPre = (page - 1) * pageSize;\r\n";
            strFunction1 += "                string strWhere = \" ORDER BY created_at DESC LIMIT \" + nLimitPre + \",\" + pageSize;\r\n";
            strFunction1 += "                int countAll = " + strProjectName + ".BLL." + strUpperTableName + "BLL.CountNumByWhere(\"\");\r\n";
            strFunction1 += "                int pageCount = countAll / pageSize;\r\n";
            strFunction1 += "                if (countAll % pageSize > 0)\r\n";
            strFunction1 += "                {\r\n";
            strFunction1 += "                    pageCount = pageCount + 1;\r\n";
            strFunction1 += "                }\r\n";
            strFunction1 += "\r\n";
            strFunction1 += "                List<" + strProjectName + ".Model."+ strUpperTableName + "Model> list = " + strProjectName + ".BLL." + strUpperTableName + "BLL.SelectAllByWhere(strWhere);\r\n";
            strFunction1 += "                foreach (" + strProjectName + ".Model." + strUpperTableName + "Model model in list)\r\n";
            strFunction1 += "                {\r\n";
            strFunction1 += GetJsonDataList(strProjectName, strTableName, "all") + "\r\n";//参数
            strFunction1 += "\r\n";
            strFunction1 += "                   strJSONDataList += \"{\" + strColumn + \"},\";\r\n";
            strFunction1 += "                }\r\n";
            strFunction1 += "\r\n";
            strFunction1 += "                //去掉最后一个逗号\r\n";
            strFunction1 += "                if (strJSONDataList.Length > 0)\r\n";
            strFunction1 += "                {\r\n";
            strFunction1 += "                   strJSONDataList = strJSONDataList.Substring(0, strJSONDataList.Length - 1);\r\n";
            strFunction1 += "                }\r\n";
            strFunction1 += "                strFormatJSON = \"{\\\"code\\\":0,\\\"msg\\\":\\\"success\\\",\\\"row_count\\\":\" + rowCount + \",\\\"page_count\\\":\" + pageCount + \",\\\"data\\\":{\\\"list\\\":[\" + strJSONDataList + \"]}}\";\r\n";
            strFunction1 += "            }\r\n";
            strFunction1 += "            catch (Exception ex)\r\n";
            strFunction1 += "            {\r\n";
            strFunction1 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "Controller), ex, \"获取所有数据\", \"GetAll\", true);\r\n";
            strFunction1 += "                strFormatJSON = \"{\\\"code\\\":1,\\\"msg\\\":\\\"fail\\\",\\\"data\\\":{\\\"error\\\":\\\"\" + ex + \"\\\"}}\";\r\n";
            strFunction1 += "            }\r\n";
            strFunction1 += "\r\n";
            strFunction1 += "            message = " + strProjectName + ".Utility.JSONHelper.toJson(strFormatJSON);\r\n";
            strFunction1 += "            return message;\r\n";
            strFunction1 += "\r\n";
            strFunction1 += "        }\r\n";

            //2-根据ID获取表中单条数据
            string strFunction2 = "\r\n";
            strFunction2 += "        /// <summary>\r\n";
            strFunction2 += "        /// 根据ID获取表中单条数据\r\n";
            strFunction2 += "        /// </summary>\r\n";
            strFunction2 += "        /// <returns>返回JSON数据</returns>\r\n";
            strFunction2 += "        [Route(\"" + strRouteDir + "/{id:int}\")]\r\n";
            strFunction2 += "        [HttpGet]\r\n";
            strFunction2 += "        public HttpResponseMessage GetOne(int id)\r\n";
            strFunction2 += "        {\r\n";
            strFunction2 += "            HttpResponseMessage message = null;\r\n";
            strFunction2 += "\r\n";
            strFunction2 += "            string strJSONDataList = \"\";\r\n";
            strFunction2 += "            string strFormatJSON = \"\";\r\n";
            strFunction2 += "\r\n";
            strFunction2 += "            try\r\n";
            strFunction2 += "            {\r\n";
            strFunction2 += "                " + strProjectName + ".Model." + strUpperTableName + "Model model = " + strProjectName + ".BLL." + strUpperTableName + "BLL.SelectOneByWhere(\"WHERE "+ strPrimaryID + "=\" + id);\r\n";
            strFunction2 += GetJsonDataList(strProjectName,strTableName,"one") + "\r\n";//参数
            strFunction2 += "\r\n";
            strFunction2 += "                strJSONDataList = \"{\" + strColumn + \"}\";\r\n";
            strFunction2 += "\r\n";
            strFunction2 += "                strFormatJSON = \"{\\\"code\\\":0,\\\"msg\\\":\\\"success\\\",\\\"data\\\": \" + strJSONDataList + \"}\";\r\n";
            strFunction2 += "            }\r\n";
            strFunction2 += "            catch (Exception ex)\r\n";
            strFunction2 += "            {\r\n";
            strFunction2 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "Controller), ex, \"根据ID获取表中单条数据\", \"GetOne\", true);\r\n";
            strFunction2 += "                strFormatJSON = \"{\\\"code\\\":1,\\\"msg\\\":\\\"fail\\\",\\\"data\\\":{\\\"error\\\":\\\"\" + ex + \"\\\"}}\";\r\n";
            strFunction2 += "            }\r\n";
            strFunction2 += "\r\n";
            strFunction2 += "            message = " + strProjectName + ".Utility.JSONHelper.toJson(strFormatJSON);\r\n";
            strFunction2 += "            return message;\r\n";
            strFunction2 += "\r\n";
            strFunction2 += "        }\r\n";

            //3-新增数据
            string strFunction3 = "\r\n";
            strFunction3 += "        /// <summary>\r\n";
            strFunction3 += "        /// 新增数据\r\n";
            strFunction3 += "        /// </summary>\r\n";
            strFunction3 += "        /// <returns>返回JSON数据</returns>\r\n";
            strFunction3 += "        [Route(\"" + strRouteDir + "\")]\r\n";
            strFunction3 += "        [HttpPost]\r\n";
            strFunction3 += "        public HttpResponseMessage Post([FromBody]" + strProjectName + ".Model." + strUpperTableName + "Model para)\r\n";
            strFunction3 += "        {\r\n";
            strFunction3 += "            HttpResponseMessage message = null;\r\n";
            strFunction3 += "\r\n";
            strFunction3 += "            string strFormatJSON = \"\";\r\n";
            strFunction3 += "\r\n";
            strFunction3 += "            try\r\n";
            strFunction3 += "            {\r\n";
            strFunction3 += "                " + strProjectName + ".Model." + strUpperTableName + "Model model = new " + strProjectName + ".Model." + strUpperTableName + "Model();\r\n";
            strFunction3 += GetInsertParas(strTableName) + "\r\n";//参数
            strFunction3 += "                " + strProjectName + ".BLL." + strUpperTableName + "BLL.InsertData(model);\r\n";
            strFunction3 += "\r\n";
            strFunction3 += "                strFormatJSON = \"{\\\"code\\\":0,\\\"msg\\\":\\\"success\\\",\\\"data\\\":{\\\"content\\\":\\\"提交成功\\\"}}\";\r\n";
            strFunction3 += "            }\r\n";
            strFunction3 += "            catch (Exception ex)\r\n";
            strFunction3 += "            {\r\n";
            strFunction3 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "Controller), ex, \"新增数据\", \"Post\", true);\r\n";
            strFunction3 += "                strFormatJSON = \"{\\\"code\\\":1,\\\"msg\\\":\\\"fail\\\",\\\"data\\\":{\\\"error\\\":\\\"\" + ex + \"\\\"}}\";\r\n";
            strFunction3 += "            }\r\n";
            strFunction3 += "\r\n";
            strFunction3 += "            message = " + strProjectName + ".Utility.JSONHelper.toJson(strFormatJSON);\r\n";
            strFunction3 += "            return message;\r\n";
            strFunction3 += "\r\n";
            strFunction3 += "        }\r\n";


            //4-更新数据
            string strFunction4 = "\r\n";
            strFunction4 += "        /// <summary>\r\n";
            strFunction4 += "        /// 更新数据\r\n";
            strFunction4 += "        /// </summary>\r\n";
            strFunction4 += "        /// <returns>返回JSON数据</returns>\r\n";
            strFunction4 += "        [Route(\"" + strRouteDir + "\")]\r\n";
            strFunction4 += "        [HttpPut]\r\n";
            strFunction4 += "        public HttpResponseMessage Put(int id, [FromBody]" + strProjectName + ".Model." + strUpperTableName + "Model para)\r\n";
            strFunction4 += "        {\r\n";
            strFunction4 += "            HttpResponseMessage message = null;\r\n";
            strFunction4 += "\r\n";
            strFunction4 += "            string strFormatJSON = \"\";\r\n";
            strFunction4 += "\r\n";
            strFunction4 += "            try\r\n";
            strFunction4 += "            {\r\n";
            strFunction4 += "                " + strProjectName + ".Model." + strUpperTableName + "Model model = new " + strProjectName + ".Model." + strUpperTableName + "Model();\r\n";
            strFunction4 += "\r\n";
            strFunction4 += GetInsertParas(strTableName) + "\r\n";//参数
            strFunction4 += "                " + strProjectName + ".BLL." + strUpperTableName + "BLL.UpdateData(model, \"WHERE " + strPrimaryID + "=\" + id);\r\n";
            strFunction4 += "\r\n";
            strFunction4 += "                strFormatJSON = \"{\\\"code\\\":0,\\\"msg\\\":\\\"success\\\",\\\"data\\\":{\\\"content\\\":\\\"更新成功\\\"}}\";\r\n";
            strFunction4 += "            }\r\n";
            strFunction4 += "            catch (Exception ex)\r\n";
            strFunction4 += "            {\r\n";
            strFunction4 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "Controller), ex, \"更新数据\", \"Put\", true);\r\n";
            strFunction4 += "                strFormatJSON = \"{\\\"code\\\":1,\\\"msg\\\":\\\"fail\\\",\\\"data\\\":{\\\"error\\\":\\\"\" + ex + \"\\\"}}\";\r\n";
            strFunction4 += "            }\r\n";
            strFunction4 += "\r\n";
            strFunction4 += "            message = " + strProjectName + ".Utility.JSONHelper.toJson(strFormatJSON);\r\n";
            strFunction4 += "            return message;\r\n";
            strFunction4 += "\r\n";
            strFunction4 += "        }\r\n";

            //5-删除数据
            string strFunction5 = "\r\n";
            strFunction5 += "        /// <summary>\r\n";
            strFunction5 += "        /// 删除数据\r\n";
            strFunction5 += "        /// </summary>\r\n";
            strFunction5 += "        /// <returns>返回JSON数据</returns>\r\n";
            strFunction5 += "        [Route(\"" + strRouteDir + "/{id:int}\")]\r\n";
            strFunction5 += "        [HttpDelete]\r\n";
            strFunction5 += "        public HttpResponseMessage Delete(int id)\r\n";
            strFunction5 += "        {\r\n";
            strFunction5 += "            HttpResponseMessage message = null;\r\n";
            strFunction5 += "\r\n";
            strFunction5 += "            string strFormatJSON = \"\";\r\n";
            strFunction5 += "\r\n";
            strFunction5 += "            try\r\n";
            strFunction5 += "            {\r\n";
            strFunction5 += "                " + strProjectName + ".BLL." + strUpperTableName + "BLL.DeleteData(\"WHERE " + strPrimaryID + "=\" + id);\r\n";
            strFunction5 += "\r\n";
            strFunction5 += "                strFormatJSON = \"{\\\"code\\\":0,\\\"msg\\\":\\\"success\\\",\\\"data\\\":{\\\"content\\\":\\\"删除成功\\\"}}\";\r\n";
            strFunction5 += "            }\r\n";
            strFunction5 += "            catch (Exception ex)\r\n";
            strFunction5 += "            {\r\n";
            strFunction5 += "                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strUpperTableName + "Controller), ex, \"删除数据\", \"Put\", true);\r\n";
            strFunction5 += "                strFormatJSON = \"{\\\"code\\\":1,\\\"msg\\\":\\\"fail\\\",\\\"data\\\":{\\\"error\\\":\\\"\" + ex + \"\\\"}}\";\r\n";
            strFunction5 += "            }\r\n";
            strFunction5 += "\r\n";
            strFunction5 += "            message = " + strProjectName + ".Utility.JSONHelper.toJson(strFormatJSON);\r\n";
            strFunction5 += "            return message;\r\n";
            strFunction5 += "\r\n";
            strFunction5 += "        }\r\n";

            strDALFunctionList = strFunction1 + strFunction2 + strFunction3 + strFunction4 + strFunction5;

            return strDALFunctionList;
        }



        /// <summary>
        /// 获取表数据生成JSON格式输出
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetJsonDataList(string strProjectName,string strTableName,string strFlag)
        {
            string strReturnValue = "";

            try
            {
                string strParaList = "";
                string strJsonList = "";

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
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);

                    string strValue = "";
                    string strData = "";

                    //包含主键
                    string valuePara = "";
                    string jsonValue = "";
                    string strBackspace = "";
                    if (strFlag == "one")
                    {
                        strBackspace = "                ";
                    }
                    else
                    {
                        strBackspace = "                   ";
                    }

                    if (strDataType == "int")
                    {
                        valuePara = "n" + strName;
                        jsonValue = "\" + " + valuePara + " + \"";
                        strValue = strBackspace + "int " + valuePara + " = model." + strName + ";\r\n";
                        strData = "n" + strName;
                    }
                    else if (strDataType == "float")
                    {
                        valuePara = "d" + strName;
                        jsonValue = "\" + " + valuePara + " + \"";
                        strValue = strBackspace + "double " + valuePara + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "double")
                    {
                        valuePara = "d" + strName;
                        jsonValue = "\" + " + valuePara + " + \"";
                        strValue = strBackspace + "double " + valuePara + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "decimal")
                    {
                        valuePara = "d" + strName;
                        jsonValue = "\" + " + valuePara + " + \"";
                        strValue = strBackspace + "double " + valuePara + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "bool")
                    {
                        valuePara = "b" + strName;
                        jsonValue = "\" + "+ valuePara + ".ToString().ToLower() + \"";
                        strValue = strBackspace + "bool " + valuePara + " = model." + strName + ";\r\n";
                        strData = "b" + strName;
                    }
                    else if (strDataType == "DateTime")
                    {
                        valuePara = "long" + strName;
                        jsonValue = "\" + " + valuePara + " + \"";
                        //jsonValue = "\\\"\" + " + valuePara + " + \"\\\"";
                        //strValue = strBackspace + "string " + valuePara + " = model." + strName + ".ToString(\"yyyy-MM-dd HH:mm\");\r\n";
                        strValue = strBackspace + "long " + valuePara + " = "+ strProjectName + ".Utility.CommonHelper.ConvertDateTimeInt(model." + strName + ");\r\n";
                        strData = "str" + strName;
                    }
                    else if (strDataType == "string")
                    {
                        valuePara = "str" + strName;
                        jsonValue = "\\\"\" + " + valuePara + " + \"\\\"";
                        strValue = strBackspace + "string " + valuePara + " = model." + strName + ";\r\n";
                        strData = "str" + strName;
                    }
                    else
                    {
                        valuePara = "str" + strName;
                        jsonValue = "\\\"\" + "+ valuePara + " + \"\\\"";
                        strValue = strBackspace + "string " + valuePara + " = model." + strName + ";\r\n";
                        strData = "str" + strName;
                    }

                    strParaList += strValue;

                    //JSON Data
                    string strJsonValue = "";
                    
                    if (nNum == 0)
                    {
                        strJsonValue = strBackspace + "string strColumn = \"\\\"" + strColumnName + "\\\":"+ jsonValue + ",\";\r\n";
                    }
                    else
                    {
                        strJsonValue = strBackspace + "strColumn += \"\\\"" + strColumnName + "\\\":" + jsonValue + ",\";\r\n";
                    }

                    strJsonList += strJsonValue;

                    nNum++;
                }
                dr.Dispose();
                cn.Close();

                strReturnValue = strParaList + "\r\n" + strJsonList;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(RestAPIHelper), ex, "获取表数据生成JSON格式输出", "GetJsonDataList",false);
            }

            if (strReturnValue.Length > 5)
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 5);
            }

            strReturnValue = strReturnValue + "\";";

            return strReturnValue;
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
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);

                    if (strColumnKey != "PRI")
                    {
                        if (strName=="CreatedAt")
                        {
                            strReturnValue += "                model." + strName + " = DateTime.Now;\r\n";
                        }
                        else
                        {
                            strReturnValue += "                model." + strName + " = para." + strName + ";\r\n";
                        }
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

       
    }
}
