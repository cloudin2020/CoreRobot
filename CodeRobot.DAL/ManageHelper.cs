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
    public class ManageHelper
    {
        /// <summary>
        /// 根据表名创建ManageHelper类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateManageFile(string strFilePath, string strProjectName, string strTableName, string strTableComment)
        {
            string strClassName = CommonHelper.GetClassName(strTableName).ToLower();

            string strClassNameUpper = CommonHelper.GetClassName(strTableName);//类名
            strClassNameUpper = CommonHelper.GetTableNameUpper(strClassNameUpper);

            string strPath = strFilePath + "\\api\\" + strClassName;
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            //获取主键ID
            string strPrimaryID = GetPrimaryKey(strTableName);

            //API-Add新增-------------------------------------------1
            //Add-1:生成aspx.cs文件
            StreamWriter sw = new StreamWriter(strPath + "\\Add.aspx.cs", false, Encoding.GetEncoding("utf-8"));
            sw.WriteLine("using System;");
            sw.WriteLine("using System.Web.UI;");
            sw.WriteLine("");
            sw.WriteLine("/// <summary>");
            sw.WriteLine("/// " + strTableComment + "新增数据");
            sw.WriteLine("/// </summary>");
            sw.WriteLine("public partial class api_" + strClassName + "_Add : System.Web.UI.Page");
            sw.WriteLine("{");
            sw.WriteLine("");
            sw.WriteLine("    protected void Page_Load(object sender, EventArgs e)");
            sw.WriteLine("    {");
            sw.WriteLine("        if (!Page.IsPostBack)");
            sw.WriteLine("        {");
            sw.WriteLine("            string strReturnValue = \"\";");
            sw.WriteLine("");
            sw.WriteLine(SetRequestParas(strProjectName, strTableName));//参数
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                /*str" + strClassNameUpper + " = System.Web.HttpUtility.UrlDecode(str" + strClassNameUpper + ");*/");
            sw.WriteLine("                " + strProjectName + ".Model." + strClassNameUpper + "Model model = new " + strProjectName + ".Model." + strClassNameUpper + "Model();");
            sw.WriteLine(SetValuesPara(strTableName));//传入参数
            sw.WriteLine("                " + strProjectName + ".BLL." + strClassNameUpper + "BLL.InsertData(model);");
            sw.WriteLine("                strReturnValue = \"{\\\"code\\\":\\\"0\\\",\\\"msg\\\":\\\"保存成功\\\"}\"; ");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                " + strProjectName + ".Utility.LogHelper.Error(typeof(api_" + strClassName + "_Add), ex, \"\", \"Page_Load\", true);");
            sw.WriteLine("                strReturnValue = \"{\\\"code\\\":\\\"1\\\",\\\"msg\\\":\\\"保存失败\\\"}\"; ");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            Response.ContentType = \"application/json\";");
            sw.WriteLine("            Response.Write(strReturnValue);");
            sw.WriteLine("        }");
            sw.WriteLine("    }");
            sw.WriteLine("}");
            sw.Close();

            //API-Add-2:生成aspx文件
            StreamWriter sw2 = new StreamWriter(strPath + "\\Add.aspx", false, Encoding.GetEncoding("utf-8"));
            sw2.WriteLine("<%@ Page Language=\"C#\" AutoEventWireup=\"true\" CodeFile=\"Add.aspx.cs\" Inherits=\"api_" + strClassName + "_Add\" %>");
            sw2.Close();

            //API-Delete删除-------------------------------------------2
            //Delete-1：生成aspx.cs文件
            StreamWriter sw3 = new StreamWriter(strPath + "\\Delete.aspx.cs", false, Encoding.GetEncoding("utf-8"));
            sw3.WriteLine("using System;");
            sw3.WriteLine("using System.Web.UI;");
            sw3.WriteLine("");
            sw3.WriteLine("/// <summary>");
            sw3.WriteLine("/// " + strTableComment + "删除数据");
            sw3.WriteLine("/// </summary>");
            sw3.WriteLine("public partial class api_" + strClassName + "_Delete : System.Web.UI.Page");
            sw3.WriteLine("{");
            sw3.WriteLine("");
            sw3.WriteLine("    protected void Page_Load(object sender, EventArgs e)");
            sw3.WriteLine("    {");
            sw3.WriteLine("        if (!Page.IsPostBack)");
            sw3.WriteLine("        {");
            sw3.WriteLine("            string strReturnValue = \"\";");
            sw3.WriteLine("");
            sw3.WriteLine(SetRequestParas(strTableName, true));//参数
            sw3.WriteLine("            try");
            sw3.WriteLine("            {");
            sw3.WriteLine("                " + strProjectName + ".BLL." + strClassNameUpper + "BLL.DeleteData(\" WHERE " + strPrimaryID + " IN (\" + str" + GetPrimaryKey(strTableName, true) + " + \")\");");
            sw3.WriteLine("");
            sw3.WriteLine("                strReturnValue = \"{\\\"code\\\":\\\"0\\\",\\\"msg\\\":\\\"删除成功\\\"}\"; ");
            sw3.WriteLine("            }");
            sw3.WriteLine("            catch (Exception ex)");
            sw3.WriteLine("            {");
            sw3.WriteLine("                " + strProjectName + ".Utility.LogHelper.Error(typeof(api_" + strClassName + "_Delete), ex, \"\", \"Page_Load\", true);");
            sw3.WriteLine("                strReturnValue = \"{\\\"code\\\":\\\"1\\\",\\\"msg\\\":\\\"删除失败\\\"}\"; ");
            sw3.WriteLine("            }");
            sw3.WriteLine("");
            sw3.WriteLine("            Response.ContentType = \"application/json\";");
            sw3.WriteLine("            Response.Write(strReturnValue);");
            sw3.WriteLine("        }");
            sw3.WriteLine("    }");
            sw3.WriteLine("}");
            sw3.Close();

            //API-Delete-2:生成aspx文件
            StreamWriter sw4 = new StreamWriter(strPath + "\\Delete.aspx", false, Encoding.GetEncoding("utf-8"));
            sw4.WriteLine("<%@ Page Language=\"C#\" AutoEventWireup=\"true\" CodeFile=\"Delete.aspx.cs\" Inherits=\"api_" + strClassName + "_Delete\" %>");
            sw4.Close();

            //API-Edit修改-------------------------------------------3
            //Edit-1:生成aspx.cs文件
            StreamWriter sw5 = new StreamWriter(strPath + "\\Edit.aspx.cs", false, Encoding.GetEncoding("utf-8"));
            sw5.WriteLine("using System;");
            sw5.WriteLine("using System.Web.UI;");
            sw5.WriteLine("");
            sw5.WriteLine("/// <summary>");
            sw5.WriteLine("/// " + strTableComment + "编辑数据");
            sw5.WriteLine("/// </summary>");
            sw5.WriteLine("public partial class api_" + strClassName + "_Edit : System.Web.UI.Page");
            sw5.WriteLine("{");
            sw5.WriteLine("");
            sw5.WriteLine("    protected void Page_Load(object sender, EventArgs e)");
            sw5.WriteLine("    {");
            sw5.WriteLine("        if (!Page.IsPostBack)");
            sw5.WriteLine("        {");
            sw5.WriteLine("            string strReturnValue = \"\";");
            sw5.WriteLine("");
            sw5.WriteLine(SetRequestParas(strProjectName, strTableName, 2));//参数
            sw5.WriteLine("            try");
            sw5.WriteLine("            {");
            sw5.WriteLine("                /*str" + strClassNameUpper + " = System.Web.HttpUtility.UrlDecode(str" + strClassNameUpper + ");*/");
            sw5.WriteLine("                " + strProjectName + ".Model." + strClassNameUpper + "Model model = new " + strProjectName + ".Model." + strClassNameUpper + "Model();");
            sw5.WriteLine(SetValuesPara(strTableName, 2));//传入参数
            sw5.WriteLine("                " + strProjectName + ".BLL." + strClassNameUpper + "BLL.UpdateData(model,\" WHERE " + strPrimaryID + "=\" + str" + GetPrimaryKey(strTableName, true) + ");");
            sw5.WriteLine("                strReturnValue = \"{\\\"code\\\":\\\"0\\\",\\\"msg\\\":\\\"保存成功\\\"}\"; ");
            sw5.WriteLine("            }");
            sw5.WriteLine("            catch (Exception ex)");
            sw5.WriteLine("            {");
            sw5.WriteLine("                " + strProjectName + ".Utility.LogHelper.Error(typeof(api_" + strClassName + "_Edit), ex, \"\", \"Page_Load\", true);");
            sw5.WriteLine("                strReturnValue = \"{\\\"code\\\":\\\"1\\\",\\\"msg\\\":\\\"保存失败\\\"}\"; ");
            sw5.WriteLine("            }");
            sw5.WriteLine("");
            sw5.WriteLine("            Response.ContentType = \"application/json\";");
            sw5.WriteLine("            Response.Write(strReturnValue);");
            sw5.WriteLine("        }");
            sw5.WriteLine("    }");
            sw5.WriteLine("}");
            sw5.Close();

            //API-Edit-2:生成aspx文件
            StreamWriter sw6 = new StreamWriter(strPath + "\\Edit.aspx", false, Encoding.GetEncoding("utf-8"));
            sw6.WriteLine("<%@ Page Language=\"C#\" AutoEventWireup=\"true\" CodeFile=\"Edit.aspx.cs\" Inherits=\"api_" + strClassName + "_Edit\" %>");
            sw6.Close();



            //生成每个表的管理页面及添加页面
            //管理页面-------------------------------------------4
            //生成aspx.cs文件
            string strManageFilePath = strFilePath + "\\" + strClassName;
            if (!Directory.Exists(strManageFilePath))
            {
                Directory.CreateDirectory(strManageFilePath);
            }

            StreamWriter sw7 = new StreamWriter(strManageFilePath + "\\Default.aspx.cs", false, Encoding.GetEncoding("utf-8"));
            sw7.WriteLine("using System;");
            sw7.WriteLine("using System.Collections.Generic;");
            sw7.WriteLine("using System.Web.UI;");
            sw7.WriteLine("");
            sw7.WriteLine("/// <summary>");
            sw7.WriteLine("/// " + strTableComment + "管理");
            sw7.WriteLine("/// </summary>");
            sw7.WriteLine("public partial class " + strClassName + "_Default : System.Web.UI.Page");
            sw7.WriteLine("{");
            sw7.WriteLine("");
            sw7.WriteLine("    public string strShowDataList = \"\";");
            sw7.WriteLine("");
            sw7.WriteLine("    protected void Page_Load(object sender, EventArgs e)");
            sw7.WriteLine("    {");
            sw7.WriteLine("        if (!Page.IsPostBack)");
            sw7.WriteLine("        {");
            sw7.WriteLine("");
            sw7.WriteLine("            try");
            sw7.WriteLine("            {");
            sw7.WriteLine("                List<" + strProjectName + ".Model." + strClassNameUpper + "Model> list = " + strProjectName + ".BLL." + strClassNameUpper + "BLL.SelectAllByWhere(\"WHERE is_del=0 \");");
            sw7.WriteLine("                foreach(" + strProjectName + ".Model." + strClassNameUpper + "Model model in list)");
            sw7.WriteLine("                {");
            sw7.WriteLine("");
            sw7.WriteLine(GetDefaultDataList(strProjectName, strTableName));//传入参数
            sw7.WriteLine("");
            sw7.WriteLine("                }");
            sw7.WriteLine("            }");
            sw7.WriteLine("            catch (Exception ex)");
            sw7.WriteLine("            {");
            sw7.WriteLine("                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strClassName + "_Default), ex, \"\", \"Page_Load\", true);");
            sw7.WriteLine("            }");
            sw7.WriteLine("");
            sw7.WriteLine("        }");
            sw7.WriteLine("    }");
            sw7.WriteLine("}");
            sw7.Close();

            //生成aspx文件
            //读取模板内容
            FileStream fs1 = new FileStream(Application.StartupPath + "\\template\\html\\manage.html", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);
            string strGetPageHTMLContentForManagePage = sr1.ReadToEnd();
            sr1.Close();
            fs1.Close();

            strGetPageHTMLContentForManagePage = GetHTMLData(strGetPageHTMLContentForManagePage, strTableName);
            StreamWriter sw8 = new StreamWriter(strManageFilePath + "\\Default.aspx", false, Encoding.GetEncoding("utf-8"));
            sw8.WriteLine(strGetPageHTMLContentForManagePage);
            sw8.Close();

            //添加页面-------------------------------------------5
            //生成add.aspx文件
            //读取模板内容
            FileStream fs2 = new FileStream(Application.StartupPath + "\\template\\html\\add.html", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr2 = new StreamReader(fs2, Encoding.UTF8);
            string strGetPageHTMLContentForAddPage = sr2.ReadToEnd();
            sr2.Close();
            fs2.Close();

            strGetPageHTMLContentForAddPage = GetAddPageHTMLData(strGetPageHTMLContentForAddPage, strTableName, strManageFilePath, strProjectName);
            StreamWriter sw10 = new StreamWriter(strManageFilePath + "\\Add.aspx", false, Encoding.GetEncoding("utf-8"));
            sw10.WriteLine(strGetPageHTMLContentForAddPage);
            sw10.Close();


            //编辑页面-------------------------------------------6
            //生成edit.aspx文件
            //读取模板内容
            FileStream fs3 = new FileStream(Application.StartupPath + "\\template\\html\\edit.html", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr3 = new StreamReader(fs3, Encoding.UTF8);
            string strGetPageHTMLContentForEditPage = sr3.ReadToEnd();
            sr3.Close();
            fs3.Close();

            strGetPageHTMLContentForEditPage = GetEditPageHTMLData(strGetPageHTMLContentForEditPage, strTableName, strManageFilePath, strProjectName);
            StreamWriter sw11 = new StreamWriter(strManageFilePath + "\\Edit.aspx", false, Encoding.GetEncoding("utf-8"));
            sw11.WriteLine(strGetPageHTMLContentForEditPage);
            sw11.Close();


            //显示页面-------------------------------------------6
            //生成view.aspx文件
            //读取模板内容
            FileStream fs4 = new FileStream(Application.StartupPath + "\\template\\html\\view.html", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr4 = new StreamReader(fs4, Encoding.UTF8);
            string strGetPageHTMLContentForViewPage = sr4.ReadToEnd();
            sr4.Close();
            fs4.Close();

            strGetPageHTMLContentForViewPage = GetViewPageHTMLData(strGetPageHTMLContentForViewPage, strTableName, strManageFilePath, strProjectName);
            StreamWriter sw13 = new StreamWriter(strManageFilePath + "\\View.aspx", false, Encoding.GetEncoding("utf-8"));
            sw13.WriteLine(strGetPageHTMLContentForViewPage);
            sw13.Close();


            //生成每个表的搜索页面
            //搜索页面-------------------------------------------7
            //读取模板内容
            FileStream fs12 = new FileStream(Application.StartupPath + "\\template\\html\\search.html", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr12 = new StreamReader(fs12, Encoding.UTF8);
            string strGetPageHTMLContentForSearchPage = sr12.ReadToEnd();
            sr12.Close();
            fs12.Close();

            strGetPageHTMLContentForSearchPage = GetSearchPageHTMLData(strGetPageHTMLContentForSearchPage, strTableName, strManageFilePath, strProjectName);
            StreamWriter sw12 = new StreamWriter(strManageFilePath + "\\Search.aspx", false, Encoding.GetEncoding("utf-8"));
            sw12.WriteLine(strGetPageHTMLContentForSearchPage);
            sw12.Close();

        }

        /// <summary>
        /// 设置请求参数列表：新增
        /// </summary>
        /// <param name="strProjectName">项目名称</param>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string SetRequestParas(string strProjectName, string strTableName)
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

                    //不包含主键
                    if (strColumnKey != "PRI")
                    {
                        string strName = CommonHelper.GetTableNameUpper(strColumnName);
                        strReturnValue += "            string str" + strName + " = Request[\"" + strName + "\"];\r\n";
                        strReturnValue += "            str" + strName + " = " + strProjectName + ".Utility.StringHelper.ReplaceSQLChar(str" + strName + ");\r\n";
                        nNum++;
                    }
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "设置请求参数列表：新增、修改", "SetRequestParas", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 设置请求参数列表：修改
        /// </summary>
        /// <param name="strProjectName">项目名称</param>
        /// <param name="strTableName"></param>
        /// <param name="nFlag"></param>
        /// <returns></returns>
        public static string SetRequestParas(string strProjectName, string strTableName, int nFlag)
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

                    //包含主键
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);
                    strReturnValue += "            string str" + strName + " = Request[\"" + strName + "\"];\r\n";
                    strReturnValue += "            str" + strName + " = " + strProjectName + ".Utility.StringHelper.ReplaceSQLChar(str" + strName + ");\r\n";
                    nNum++;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "设置请求参数列表：新增、修改", "SetRequestParas", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 设置请求参数列表：删除
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string SetRequestParas(string strTableName, bool isDelete)
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

                    if (strColumnKey == "PRI" && isDelete == true)
                    {
                        string strName = CommonHelper.GetTableNameUpper(strColumnName);
                        strReturnValue += "            string str" + strName + " = Request[\"" + strName + "\"];\r\n";
                        strReturnValue += "            if (str" + strName + ".Contains(\",\"))\r\n";
                        strReturnValue += "            {\r\n";
                        strReturnValue += "                str" + strName + " = str" + strName + ".Substring(0, str" + strName + ".Length - 1);\r\n";
                        strReturnValue += "            }\r\n";
                        nNum++;
                    }
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "设置请求参数列表：删除", "SetRequestParas", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取表中主键
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        public static string GetPrimaryKey(string strTableName, bool isDelete)
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
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    strColumnComment = CommonHelper.GetColumnKeyComment(strColumnComment);

                    if (strColumnKey == "PRI" && isDelete == true)
                    {
                        string strName = CommonHelper.GetTableNameUpper(strColumnName);
                        strReturnValue = strName;
                    }
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "设置请求参数列表：删除", "SetRequestParas", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 设置传入参数值：新增，删除
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string SetValuesPara(string strTableName)
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
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);

                    string strValue = "";

                    //不包含主键
                    if (strColumnKey != "PRI")
                    {
                        if (strDataType == "int")
                        {
                            strValue = "                model." + strName + " = int.Parse(str" + strName + ");\r\n";
                        }
                        else if (strDataType == "float")
                        {
                            strValue = "                model." + strName + " = Convert.ToDouble(str" + strName + ");\r\n";
                        }
                        else if (strDataType == "double")
                        {
                            strValue = "                model." + strName + " = Convert.ToDouble(str" + strName + ");\r\n";
                        }
                        else if (strDataType == "decimal")
                        {
                            strValue = "                model." + strName + " = Convert.ToDouble(str" + strName + ");\r\n";
                        }
                        else if (strDataType == "bool")
                        {
                            strValue = "                model." + strName + " = str" + strName + " == \"1\" ? true : false;\r\n";
                        }
                        else if (strDataType == "DateTime")
                        {
                            strValue = "                model." + strName + " = DateTime.Now;\r\n";
                        }
                        else if (strDataType == "string")
                        {
                            strValue = "                model." + strName + " = str" + strName + ";\r\n";
                        }
                        else
                        {
                            strValue = "                model." + strName + " = str" + strName + ";\r\n";
                        }
                    }

                    strReturnValue += strValue;

                    nNum++;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "设置传入参数值", "SetValuesPara", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 设置传入参数值：修改
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <param name="nFlag"></param>
        /// <returns></returns>
        public static string SetValuesPara(string strTableName, int nFlag)
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
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);

                    string strValue = "";
                    //包含主键
                    if (strDataType == "int")
                    {
                        strValue = "                model." + strName + " = int.Parse(str" + strName + ");\r\n";
                    }
                    else if (strDataType == "float")
                    {
                        strValue = "                model." + strName + " = Convert.ToDouble(str" + strName + ");\r\n";
                    }
                    else if (strDataType == "double")
                    {
                        strValue = "                model." + strName + " = Convert.ToDouble(str" + strName + ");\r\n";
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = "                model." + strName + " = Convert.ToDouble(str" + strName + ");\r\n";
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = "                model." + strName + " = str" + strName + " == \"1\" ? true : false;\r\n";
                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = "                model." + strName + " = DateTime.Now;\r\n";
                    }
                    else if (strDataType == "string")
                    {
                        strValue = "                model." + strName + " = str" + strName + ";\r\n";
                    }
                    else
                    {
                        strValue = "                model." + strName + " = str" + strName + ";\r\n";
                    }

                    strReturnValue += strValue;

                    nNum++;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "设置传入参数值", "SetValuesPara", false);
            }

            return strReturnValue;
        }


        /// <summary>
        /// 获取管理页面列表数据
        /// </summary>
        /// <param name="strProjectName">项目名称</param>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetDefaultDataList(string strProjectName, string strTableName)
        {
            string strReturnValue = "";

            try
            {
                string strParaList = "";
                string strDataList = "";

                string strUpperTableName = CommonHelper.GetClassName(strTableName);
                strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);

                int nNum = 0;
                string strPrimaryKeyValue = "";

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
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//字段说明
                    strDataType = StringHelper.GetCSharpDBType(strDataType);
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);

                    if (strColumnKey == "PRI")
                    {
                        strPrimaryKeyValue = "n" + strName;
                    }

                    string strValue = "";
                    string strData = "";

                    //包含主键
                    if (strDataType == "int")
                    {
                        strValue = "                int n" + strName + " = model." + strName + ";\r\n";
                        strData = "n" + strName;
                        if (strColumnComment.Contains("types"))
                        {
                            //解析表名
                            string strGetTableName = "";
                            try
                            {
                                strColumnComment = strColumnComment.Replace("[", ",");
                                strColumnComment = strColumnComment.Replace("]", ",");
                                string[] splitComment = strColumnComment.Split(new char[] { ',' });
                                if (splitComment.Length > 0)
                                {
                                    strGetTableName = splitComment[1];
                                }
                            }
                            catch (Exception ex)
                            {
                                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "解析表名", "SetValuesPara", false);
                            }
                            strGetTableName = CommonHelper.GetClassName(strGetTableName);
                            strGetTableName = CommonHelper.GetTableNameUpper(strGetTableName);

                            strValue += "                string strTypeName = " + strProjectName + ".BLL." + strGetTableName + "BLL.GetColumnValueByWhere(\"type_name\", \"WHERE type_id=\" + n" + strName + ");\r\n";
                        }
                    }
                    else if (strDataType == "float")
                    {
                        strValue = "                double d" + strName + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "double")
                    {
                        strValue = "                double d" + strName + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = "                double d" + strName + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = "                bool b" + strName + " = model." + strName + ";\r\n";
                        strData = "b" + strName;
                        strValue += "                string str" + strName + " = \"<span class=\"badge badge-info\">否</span>\";\r\n";
                        strValue += "                if (b" + strName + ")\r\n";
                        strValue += "                {\r\n";
                        strValue += "                    str" + strName + " = \"<span class=\"badge badge-warning\">是</span>\";\r\n";
                        strValue += "                }\r\n";
                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = "                string str" + strName + " = model." + strName + ".ToString(\"yyyy-MM-dd HH:mm\");\r\n";
                        strData = "str" + strName;
                    }
                    else if (strDataType == "string")
                    {
                        strValue = "                string str" + strName + " = model." + strName + ";\r\n";
                        strData = "str" + strName;
                        string strNameLower = strName.ToLower();
                        if (strNameLower.Contains("img") || strNameLower.Contains("image") || strNameLower.Contains("avatar"))
                        {
                            strValue += "                if (string.IsNullOrEmpty(model." + strName + "))\r\n";
                            strValue += "                {\r\n";
                            strValue += "                    str" + strName + " = \"<img src=\\\"/images/noimg.jpg\\\" width=\\\"50\\\" height=\\\"50\\\">\";\r\n";
                            strValue += "                }\r\n";
                            strValue += "                else\r\n";
                            strValue += "                {\r\n";
                            strValue += "                    str" + strName + " = \"<img src=\\\"\" + " + strProjectName + ".Utility.ConfigHelper.GetImgURL() + model." + strName + " + \"\\\" width=\\\"50\\\" height=\\\"50\\\">\";\r\n";
                            strValue += "                }\r\n";
                        }
                    }
                    else
                    {
                        strValue = "                    string str" + strName + " = model." + strName + ";\r\n";
                        strData = "str" + strName;
                    }

                    strParaList += strValue;

                    strDataList += "                strValue += \"<td>\" + " + strData + " + \"</td>\";\r\n";

                    nNum++;
                }
                dr.Dispose();
                cn.Close();

                string strDataValue = "                string strValue = \"<tr class=\\\"\\\">\";\r\n";
                strDataValue += "                strValue += \"<td ><input name =\\\"groupCheckbox\\\"  style=\\\"width: 20px;height: 20px\\\" type=\\\"checkbox\\\" id=\\\"\" + " + strPrimaryKeyValue + " + \"\\\" value=\\\"\\\" /></td>\";\r\n";
                strDataValue += strDataList;
                strDataValue += "                strValue += \"<td><a class=\\\"edit\\\" href=\\\"javascript:;\\\"><i class=\\\"fa fa-pencil color-green\\\"> 编辑</a></td>\";\r\n";
                strDataValue += "                strValue += \"<td><a class=\\\"delete\\\" href=\\\"javascript:;\\\"><i class=\\\"fa fa-trash-o color-red\\\"> 删除</a></td>\";\r\n";
                strDataValue += "                strValue += \"</tr>\";\r\n\r\n";
                strDataValue += "                strShowDataList += strValue;\r\n";

                strReturnValue = strParaList + "\r\n" + strDataValue;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "设置传入参数值", "SetValuesPara", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取搜索页面数据
        /// </summary>
        /// <param name="strProjectName"></param>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetSearchDataList(string strProjectName, string strTableName)
        {
            string strReturnValue = "";

            try
            {
                string strParaList = "";
                string strDataList = "";

                string strUpperTableName = CommonHelper.GetClassName(strTableName);
                strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);

                int nNum = 0;
                string strPrimaryKeyValue = "";

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
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//字段说明
                    strDataType = StringHelper.GetCSharpDBType(strDataType);
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);

                    if (strColumnKey == "PRI")
                    {
                        strPrimaryKeyValue = "n" + strName;
                    }

                    string strValue = "";
                    string strData = "";

                    //包含主键
                    if (strDataType == "int")
                    {
                        strValue = "                int n" + strName + " = model." + strName + ";\r\n";
                        strData = "n" + strName;
                        if (strColumnComment.Contains("types"))
                        {
                            //解析表名
                            string strGetTableName = "";
                            try
                            {
                                strColumnComment = strColumnComment.Replace("[", ",");
                                strColumnComment = strColumnComment.Replace("]", ",");
                                string[] splitComment = strColumnComment.Split(new char[] { ',' });
                                if (splitComment.Length > 0)
                                {
                                    strGetTableName = splitComment[1];
                                }
                            }
                            catch (Exception ex)
                            {
                                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "解析表名", "SetValuesPara", false);
                            }
                            strGetTableName = CommonHelper.GetClassName(strGetTableName);
                            strGetTableName = CommonHelper.GetTableNameUpper(strGetTableName);

                            strValue += "                string strTypeName = " + strProjectName + ".BLL." + strGetTableName + "BLL.GetColumnValueByWhere(\"type_name\", \"WHERE type_id=\" + n" + strName + ");\r\n";
                        }
                    }
                    else if (strDataType == "float")
                    {
                        strValue = "                double d" + strName + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "double")
                    {
                        strValue = "                double d" + strName + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = "                double d" + strName + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = "                bool b" + strName + " = model." + strName + ";\r\n";
                        strData = "b" + strName;
                        strValue += "                string str" + strName + " = \"否\";\r\n";
                        strValue += "                if (b" + strName + ")\r\n";
                        strValue += "                {\r\n";
                        strValue += "                    str" + strName + " = \"是\";\r\n";
                        strValue += "                }\r\n";
                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = "                string str" + strName + " = model." + strName + ".ToString(\"yyyy-MM-dd HH:mm\");\r\n";
                        strData = "str" + strName;
                    }
                    else if (strDataType == "string")
                    {
                        strValue = "                string str" + strName + " = model." + strName + ";\r\n";
                        strData = "str" + strName;
                        string strNameLower = strName.ToLower();
                        if (strNameLower.Contains("img") || strNameLower.Contains("image") || strNameLower.Contains("avatar"))
                        {
                            strValue += "                if (string.IsNullOrEmpty(model." + strName + "))\r\n";
                            strValue += "                {\r\n";
                            strValue += "                    str" + strName + " = \"<img src=\\\"/images/noimg.jpg\\\" width=\\\"50\\\" height=\\\"50\\\">\";\r\n";
                            strValue += "                }\r\n";
                            strValue += "                else\r\n";
                            strValue += "                {\r\n";
                            strValue += "                    str" + strName + " = \"<img src=\\\"\" + " + strProjectName + ".Utility.ConfigHelper.GetImgURL() + model." + strName + " + \"\\\" width=\\\"50\\\" height=\\\"50\\\">\";\r\n";
                            strValue += "                }\r\n";
                        }
                    }
                    else
                    {
                        strValue = "                    string str" + strName + " = model." + strName + ";\r\n";
                        strData = "str" + strName;
                    }

                    strParaList += strValue;

                    strDataList += "                strValue += \"<td>\" + " + strData + " + \"</td>\";\r\n";

                    nNum++;
                }
                dr.Dispose();
                cn.Close();

                string strDataValue = "                string strValue = \"<tr class=\\\"\\\">\";\r\n";
                strDataValue += "                strValue += \"<td ><input name =\\\"groupCheckbox\\\"  style=\\\"width: 20px;height: 20px\\\"  type=\\\"checkbox\\\" id=\\\"\" + " + strPrimaryKeyValue + " + \"\\\" value=\\\"\\\" /></td>\";\r\n";
                strDataValue += strDataList;
                strDataValue += "                strValue += \"<td><a href=\\\"view.aspx?id=\" + " + strPrimaryKeyValue + " + \"&roleid=-1&rank=-1&code=-1\\\"><i class=\\\"fa fa-search color-green\\\"></i> 查看</a> | <a href=\\\"edit.aspx?id=\" + " + strPrimaryKeyValue + " + \"&roleid=-1&rank=-1&code=-1\\\"><i class=\\\"fa fa-pencil color-green\\\"></i> 编辑</a> | <a class=\\\"delete\\\" href=\\\"javascript:;\\\"><i class=\\\"fa fa-trash-o color-red\\\"></i> 删除</a></td>\";\r\n";
                strDataValue += "                strValue += \"</tr>\";\r\n\r\n";
                strDataValue += "                strShowDataList += strValue;\r\n";

                strReturnValue = strParaList + "\r\n" + strDataValue;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "设置传入参数值", "SetValuesPara", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 生成新增页面aspx.cs代码参数
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetDataListForAddPage(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                string strParaList = "";

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
                    if (strDataType == "int")
                    {
                        strValue = "                    int n" + strName + " = model." + strName + ";\r\n";
                        strData = "n" + strName;
                    }
                    else if (strDataType == "float")
                    {
                        strValue = "                    double d" + strName + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "double")
                    {
                        strValue = "                    double d" + strName + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = "                    double d" + strName + " = model." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = "                    bool b" + strName + " = model." + strName + ";\r\n";
                        strData = "b" + strName;
                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = "                    string str" + strName + " = model." + strName + ".ToString(\"yyyy-MM-dd HH:mm\");\r\n";
                        strData = "str" + strName;
                    }
                    else if (strDataType == "string")
                    {
                        strValue = "                    string str" + strName + " = model." + strName + ";\r\n";
                        strData = "str" + strName;
                    }
                    else
                    {
                        strValue = "                    string str" + strName + " = model." + strName + ";\r\n";
                        strData = "str" + strName;
                    }

                    string strColumnName2 = strColumnName.ToLower();
                    if (strColumnKey == "PRI")
                    {
                        //主键
                        strParaList += "                    int nId = model." + strName + ";\r\n";
                    }
                    if (strColumnName2.Contains("name"))
                    {
                        //下拉值
                        strParaList += "                    string strName = model." + strName + ";\r\n";
                    }

                    nNum++;
                }
                dr.Dispose();
                cn.Close();

                string strShowDataListPara = "strShow" + strUpperTableName + "DataList";

                string strDataValue = "";
                strDataValue += strParaList;
                strDataValue += "                    string strValue = \"<option value=\\\"\" + nId + \"\\\" >\" + strName + \"</option>\";\r\n";
                strDataValue += "                    " + strShowDataListPara + " += strValue;\r\n";


                strReturnValue = strDataValue;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "生成新增页面aspx.cs代码参数", "GetDataListForAddPage", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 生成修改页面aspx.cs代码参数
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetDataListForEditPage(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                string strParaList = "";

                string strUpperTableName = CommonHelper.GetClassName(strTableName);
                strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);

                int nNum = 0;
                string strPrimaryKeyUpper = "";

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
                    if (strDataType == "int")
                    {
                        strValue = "                    int n" + strName + " = data." + strName + ";\r\n";
                        strData = "n" + strName;
                    }
                    else if (strDataType == "float")
                    {
                        strValue = "                    double d" + strName + " = data." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "double")
                    {
                        strValue = "                    double d" + strName + " = data." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = "                    double d" + strName + " = data." + strName + ";\r\n";
                        strData = "d" + strName;
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = "                    bool b" + strName + " = data." + strName + ";\r\n";
                        strData = "b" + strName;
                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = "                    string str" + strName + " = data." + strName + ".ToString(\"yyyy-MM-dd HH:mm\");\r\n";
                        strData = "str" + strName;
                    }
                    else if (strDataType == "string")
                    {
                        strValue = "                    string str" + strName + " = data." + strName + ";\r\n";
                        strData = "str" + strName;
                    }
                    else
                    {
                        strValue = "                    string str" + strName + " = data." + strName + ";\r\n";
                        strData = "str" + strName;
                    }

                    string strColumnName2 = strColumnName.ToLower();
                    if (strColumnKey == "PRI")
                    {
                        //主键
                        strParaList += "                    int nId = data." + strName + ";\r\n";
                        strPrimaryKeyUpper = strName;
                    }
                    if (strColumnName2.Contains("name"))
                    {
                        //下拉值
                        strParaList += "                    string strName = data." + strName + ";\r\n";
                    }


                    nNum++;
                }
                dr.Dispose();
                cn.Close();

                string strShowDataListPara = "strShow" + strUpperTableName + "DataList";

                string strDataValue = "";
                strDataValue += strParaList;
                strDataValue += "                    string strValue = \"\";\r\n";
                strDataValue += "                    if (nId == n" + strPrimaryKeyUpper + ")\r\n";
                strDataValue += "                    {\r\n";
                strDataValue += "                        strValue = \"<option value=\\\"\" + nId + \"\\\"  selected=\\\"selected\\\">\" + strName + \"</option>\";\r\n";
                strDataValue += "                    }\r\n";
                strDataValue += "                    else\r\n";
                strDataValue += "                    {\r\n";
                strDataValue += "                        strValue = \"<option value=\\\"\" + nId + \"\\\" >\" + strName + \"</option>\";\r\n";
                strDataValue += "                    }\r\n";
                strDataValue += "                    " + strShowDataListPara + " += strValue;\r\n";

                strReturnValue = strDataValue;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "生成新增页面aspx.cs代码参数", "GetDataListForAddPage", false);
            }

            return strReturnValue;
        }


        /// <summary>
        /// 处理HTML代码
        /// </summary>
        /// <param name="strContent">HTML源码内容</param>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetHTMLData(string strContent, string strTableName)
        {
            string strReturnValue = "";

            try
            {
                string strClassName = CommonHelper.GetClassName(strTableName).ToLower();

                //替换文件名-1
                strContent = strContent.Replace("{classname}", strClassName);

                string strUpperTableName = CommonHelper.GetClassName(strTableName);
                strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);

                string strTableNameComment = "";//表注释
                string strTableTHList = "";//表头
                string strEditLineList = "";
                string strPostEditDataList = "";
                string strPostParasList = "";
                string strColumnParaList = "";
                string strAlertColumnParaList = "";
                string strPostParaList = "";
                string strAddLineList = "";
                string strAddDataLineList = "";
                string strAddParasList = "";
                string strColumnParaListAdd = "";
                string strAddParaList = "";
                string strCancelDataLineList = "";
                string strColumnItemList = "";
                string strDeletePara = "";
                string strDelPrimaryKey = "";

                int nNum = 0;
                int nNum2 = 0;
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT,DATA_TYPE FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    nNum2 = nNum + 1;
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strDataType = dr["DATA_TYPE"].ToString();//数据类型
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    strColumnComment = CommonHelper.GetColumnKeyComment(strColumnComment);

                    strDataType = StringHelper.GetCSharpDBType(strDataType);
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);

                    //通过表中主键获取表名注释
                    if (strColumnKey == "PRI")
                    {
                        strColumnComment = strColumnComment.Replace("表ID", "");
                        strTableNameComment = strColumnComment;
                    }

                    string strTableTH = "";//表头

                    //包含主键
                    if (strDataType == "int")
                    {
                        strTableTH = strColumnComment;
                    }
                    else if (strDataType == "float")
                    {
                        strTableTH = strColumnComment;
                    }
                    else if (strDataType == "double")
                    {
                        strTableTH = strColumnComment;
                    }
                    else if (strDataType == "decimal")
                    {
                        strTableTH = strColumnComment;
                    }
                    else if (strDataType == "bool")
                    {
                        strTableTH = strColumnComment;
                    }
                    else if (strDataType == "DateTime")
                    {
                        strTableTH = strColumnComment;
                    }
                    else if (strDataType == "string")
                    {
                        strTableTH = strColumnComment;
                    }
                    else
                    {
                        strTableTH = strColumnComment;
                    }

                    if (nNum == 0)
                    {
                        strTableTH = "\r\n                                                    <th>ID</th>\r\n";
                    }
                    else
                    {
                        strTableTH = "                                                    <th>" + strTableTH + "</th>\r\n";
                    }

                    //表头-3
                    strTableTHList += strTableTH;

                    //编辑状态下UI-4
                    string strEditLine = "";
                    if (nNum == 0)
                    {
                        strEditLine = "                                jqTds[" + nNum2 + "].innerHTML = '<input type=\"text\" disabled=\"disabled\" class=\"form-control small\" value=\"' + aData[" + nNum2 + "] + '\">';\r\n";
                    }
                    else
                    {
                        strEditLine = "                                jqTds[" + nNum2 + "].innerHTML = '<input type=\"text\" class=\"form-control small\" value=\"' + aData[" + nNum2 + "] + '\">';\r\n";
                    }

                    strEditLineList += strEditLine;

                    //编辑提交数据-5
                    string strPostEditData = "";
                    if (nNum == 0)
                    {
                        strPostEditData = "\r\n                                oTable.fnUpdate(jqInputs[" + nNum + "].value, nRow, " + nNum + ", false);\r\n";
                    }
                    else
                    {
                        strPostEditData = "                                oTable.fnUpdate(jqInputs[" + nNum + "].value, nRow, " + nNum + ", false);\r\n";
                    }

                    strPostEditDataList += strPostEditData;

                    //编辑提交参数拼合-6
                    string strPostParas = "jqInputs[" + nNum + "].value";
                    strPostParasList += strPostParas + ",";

                    //编辑提交函数参数-7
                    strColumnParaList += strColumnName + ",";
                    strAlertColumnParaList += strColumnName + " + \",\" +";

                    //编辑提交函数参数-8
                    string strPostPara = "'" + strName + "': " + strColumnName;
                    strPostParaList += strPostPara + ",";

                    //新增状态下UI-10
                    string strAddLine = "";
                    if (nNum == 0)
                    {
                        strAddLine = "                                jqTds[" + nNum2 + "].innerHTML = '';\r\n";
                    }
                    else
                    {
                        strAddLine = "                                jqTds[" + nNum2 + "].innerHTML = '<input type=\"text\" class=\"form-control small\" value=\"' + aData[" + nNum2 + "] + '\">';\r\n";
                    }

                    strAddLineList += strAddLine;

                    //新增状态下数据-11
                    string strAddDataLine = "";
                    if (nNum == 0)
                    {
                        strAddDataLine = "                                oTable.fnUpdate('', nRow, 1, false);\r\n";
                    }
                    else
                    {
                        int index = nNum - 1;
                        strAddDataLine = "                                oTable.fnUpdate(jqInputs[" + index + "].value, nRow, " + nNum2 + ", false);\r\n";
                    }

                    strAddDataLineList += strAddDataLine;

                    //新增状态下数据-12
                    //不包含主键
                    if (strColumnKey != "PRI")
                    {
                        int index = nNum - 1;
                        string strAddParas = "jqInputs[" + index + "].value";
                        strAddParasList += strAddParas + ",";

                        strColumnParaListAdd += strColumnName + ",";

                        string strAddPara = "'" + strName + "': " + strColumnName;
                        strAddParaList += strAddPara + ",";
                    }

                    //取消状态下数据-13
                    string strCancelDataLine = "";
                    if (nNum == 0)
                    {
                        strCancelDataLine = "                                oTable.fnUpdate('', nRow, 1, false);\r\n";
                    }
                    else
                    {
                        int index = nNum - 1;
                        strCancelDataLine = "                                oTable.fnUpdate(jqInputs[" + index + "].value, nRow, " + nNum2 + ", false);\r\n";
                    }

                    strCancelDataLineList += strCancelDataLine;

                    //新增数据UI列模型-14
                    string strColumnItem = "''";
                    strColumnItemList += strColumnItem + ",";

                    //删除参数-15
                    if (strColumnKey == "PRI")
                    {
                        strDeletePara = " '" + strName + "': id";
                        strDelPrimaryKey = strName;
                    }

                    nNum++;


                }
                dr.Dispose();
                cn.Close();

                int nextNum = nNum + 1;
                int nextNum2 = nNum + 2;

                //替换表名-2
                strContent = strContent.Replace("{title}", strTableNameComment + "管理");

                //替换表头-3
                strContent = strContent.Replace("{th}", strTableTHList);

                //替换编辑状态下UI-4
                strEditLineList = "\r\n                                jqTds[0].innerHTML = '';\r\n" + strEditLineList;
                strEditLineList += "                                jqTds[" + nextNum + "].innerHTML = '<a class=\"edit\" href=\"\">更新</a>';\r\n";
                strEditLineList += "                                jqTds[" + nextNum2 + "].innerHTML = '<a class=\"cancel\" href=\"\">取消</a>';\r\n";

                strContent = strContent.Replace("/*{edit-ui-list}*/", strEditLineList);

                //编辑提交数据-5
                strPostEditDataList += "                                oTable.fnUpdate('<a class=\"edit\" href=\"\">编辑</a>', nRow, " + nextNum + ", false);\r\n";
                strPostEditDataList += "                                oTable.fnUpdate('<a class=\"delete\" href=\"\">删除</a>', nRow, " + nextNum2 + ", false);\r\n";

                strContent = strContent.Replace("/*{edit-post-data-list}*/", strPostEditDataList);

                //编辑提交参数拼合-6
                if (strPostParasList.Length > 0)
                {
                    strPostParasList = strPostParasList.Substring(0, strPostParasList.Length - 1);
                }
                strPostParasList = "editData(" + strPostParasList + ")";

                strContent = strContent.Replace("/*{edit-post-data-para}*/", strPostParasList);

                //编辑提交函数参数-7
                if (strColumnParaList.Length > 0)
                {
                    strColumnParaList = strColumnParaList.Substring(0, strColumnParaList.Length - 1);
                }

                strContent = strContent.Replace("function_column_paras_edit", strColumnParaList);
                strContent = strContent.Replace("function_column_paras_alert", strAlertColumnParaList);


                //编辑提交函数参数-8
                if (strPostParaList.Length > 0)
                {
                    strPostParaList = strPostParaList.Substring(0, strPostParaList.Length - 1);
                }

                strContent = strContent.Replace("edit_column_paras", strPostParaList);

                //编辑提交URL-9
                strContent = strContent.Replace("{tablename}", strClassName);

                //新增状态下UI-10
                strAddLineList = "\r\n                                jqTds[0].innerHTML = '';\r\n" + strAddLineList;
                strAddLineList += "                                jqTds[" + nextNum + "].innerHTML = '<a class=\"edit\" href=\"\">保存</a>';\r\n";
                strAddLineList += "                                jqTds[" + nextNum2 + "].innerHTML = '<a class=\"cancel\" href=\"\">取消</a>';\r\n";

                strContent = strContent.Replace("/*{add-ui-list}*/", strAddLineList);

                //新增状态下数据-11
                strAddDataLineList = "\r\n                                oTable.fnUpdate('', nRow, 0, false);\r\n" + strAddDataLineList;
                strAddDataLineList += "                                oTable.fnUpdate('<a class=\"edit\" href=\"\">编辑</a>', nRow, " + nextNum + ", false);\r\n";
                strAddDataLineList += "                                oTable.fnUpdate('<a class=\"delete\" href=\"\">删除</a>', nRow, " + nextNum2 + ", false);\r\n";

                strContent = strContent.Replace("/*{add-data-list}*/", strAddDataLineList);

                //新增状态下数据-12
                if (strAddParasList.Length > 0)
                {
                    strAddParasList = strAddParasList.Substring(0, strAddParasList.Length - 1);
                }
                strAddParasList = "saveData(" + strAddParasList + ")";
                strContent = strContent.Replace("/*{add-post-data-para}*/", strAddParasList);

                if (strColumnParaListAdd.Length > 0)
                {
                    strColumnParaListAdd = strColumnParaListAdd.Substring(0, strColumnParaListAdd.Length - 1);
                }
                //替换新增数据函数中的参数
                strContent = strContent.Replace("function_column_paras_add", strColumnParaListAdd);

                if (strAddParaList.Length > 0)
                {
                    strAddParaList = strAddParaList.Substring(0, strAddParaList.Length - 1);
                }
                //替换新增数据函数中的提交参数
                strContent = strContent.Replace("add_column_paras", strAddParaList);


                //取消状态下数据-13
                strCancelDataLineList = "\r\n                                oTable.fnUpdate('', nRow, 0, false);\r\n" + strCancelDataLineList;
                strCancelDataLineList += "                                oTable.fnUpdate('<a class=\"edit\" href=\"\">编辑</a>', nRow, " + nextNum + ", false);\r\n";

                strContent = strContent.Replace("/*{cancel-data-list}*/", strCancelDataLineList);

                //新增数据UI列模型-14
                strColumnItemList = "''," + strColumnItemList;
                strColumnItemList = "[" + strColumnItemList + "'<a class=\"edit\" href=\"\">编辑</a>', '<a class=\"cancel\" data-mode=\"new\" href=\"\">取消</a>']";

                strContent = strContent.Replace("add_data_column_model", strColumnItemList);

                //替换删除参数-15
                strContent = strContent.Replace("delete_column_paras", strDeletePara);

                //替换批量删除-16
                strContent = strContent.Replace("{primarykey}", strDelPrimaryKey);

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "GetHTMLData", false);
            }

            return strReturnValue;
        }


        /// <summary>
        /// 获取新增页面HTML代码
        /// </summary>
        /// <param name="strContent">HTML源码内容</param>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetAddPageHTMLData(string strContent, string strTableName, string strManageFilePath, string strProjectName)
        {
            string strReturnValue = "";

            try
            {
                string strClassName = CommonHelper.GetClassName(strTableName).ToLower();

                //替换文件名-1
                strContent = strContent.Replace("{classname}", strClassName);

                string strUpperTableName = CommonHelper.GetClassName(strTableName);
                strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);

                string strTableNameComment = "";//表注释
                string strColumnItemList = "";
                string strParaList = "";//参数
                string strParaJsonList = "";//参数
                string strUnionTable = "";
                string strDefineValueList = "";
                string strParaSelectList = "";

                int nNum = 0;
                int nSelectNum = 0;
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
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);

                    strDataType = StringHelper.GetCSharpDBType(strDataType);
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);

                    //通过表中主键获取表名注释
                    if (strColumnKey == "PRI")
                    {
                        strColumnComment = strColumnComment.Replace("表ID", "");
                        strTableNameComment = strTableNameComment = strColumnComment;
                    }

                    //不包含主键
                    if (strColumnKey != "PRI")
                    {
                        //获取注释中的表名
                        string strGetUnionTableName = "";
                        string strGetSelectTableName = "";
                        if (strColumnComment.Contains("["))
                        {
                            string[] splitCommentsA = strColumnComment.Split(new char[] { '[' });
                            string strGetSplitStr = splitCommentsA[1];
                            string[] splitCommentsB = strGetSplitStr.Split(new char[] { ']' });
                            strGetUnionTableName = splitCommentsB[0];//获取关联表名
                            strGetSelectTableName = strGetUnionTableName;

                            strUnionTable += strGetUnionTableName + ",";//存储给asp.cs页面使用

                            strGetUnionTableName = CommonHelper.GetClassName(strGetUnionTableName);
                            strGetUnionTableName = CommonHelper.GetTableNameUpper(strGetUnionTableName);

                        }

                        string strShowSelectValue = "strShow" + strGetUnionTableName + "DataList";

                        //获取字段对应的编辑选项
                        string strColumn = "";
                        if (strColumnComment.Contains("{下拉}"))
                        {
                            string strDefineValue = "    public string " + strShowSelectValue + " = \"\";\r\n";
                            strDefineValueList += strDefineValue;

                            //UI页面
                            strColumn = "                                    <div class=\"form-group\">\r\n";
                            strColumn += "                                        <label for=\"cname\" class=\"control-label col-lg-2\">" + strColumnComment2 + " <!--<span class=\"color-red\">(*)</span>--></label>\r\n";
                            strColumn += "                                        <div class=\"col-lg-4\">\r\n";
                            strColumn += "                                            <select class=\"form-control  m-bot15\" id=\"" + strColumnName + "\">\r\n";
                            strColumn += "                                                <option value =\"-1\">请选择</option>\r\n";
                            strColumn += "                                                <%=" + strShowSelectValue + " %>\r\n";
                            strColumn += "                                            </select>\r\n";
                            strColumn += "                                        </div>\r\n";
                            strColumn += "                                    </div>\r\n";

                            //aspx.cs页面
                            if (!string.IsNullOrEmpty(strGetUnionTableName))
                            {
                                string strColumnSelect = "\r\n                //" + strGetUnionTableName + "\r\n";
                                strColumnSelect += "                List<" + strProjectName + ".Model." + strGetUnionTableName + "Model> list" + nSelectNum + " = " + strProjectName + ".BLL." + strGetUnionTableName + "BLL.SelectAllByWhere(\"\");\r\n";
                                strColumnSelect += "                foreach(" + strProjectName + ".Model." + strGetUnionTableName + "Model model in list" + nSelectNum + ")\r\n";
                                strColumnSelect += "                {\r\n";
                                strColumnSelect += GetDataListForAddPage(strGetSelectTableName);//传入参数
                                strColumnSelect += "                }\r\n";

                                strParaSelectList += strColumnSelect;
                                nSelectNum++;
                            }

                        }
                        else if (strColumnComment.Contains("{勾选}"))
                        {
                            strColumn = "\r\n                                    <div class=\"form-group\">\r\n";
                            strColumn += "                                        <label for=\"cname\" class=\"control-label col-lg-2 col-sm-3\">" + strColumnComment2 + " <!--<span class=\"color-red\">(*)</span>--></label>\r\n";
                            strColumn += "                                        <div class=\"col-lg-4  col-sm-9\">\r\n";
                            strColumn += "                                            <input type=\"checkbox\" id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" style=\"width: 20px\" class=\"checkbox form-control\" />\r\n";
                            strColumn += "                                        </div>\r\n";
                            strColumn += "                                    </div>\r\n";
                        }
                        else
                        {
                            strColumn = "\r\n                                    <div class=\"form-group\">\r\n";
                            strColumn += "                                        <label for=\"cname\" class=\"control-label col-lg-2\">" + strColumnComment2 + " <!--<span class=\"color-red\">(*)</span>--></label>\r\n";
                            strColumn += "                                        <div class=\"col-lg-4\">\r\n";
                            strColumn += "                                            <input class=\"form-control\" type=\"text\" id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" placeholder=\"请输入" + strColumnComment2 + "\" maxlength=\"20\"/>\r\n";
                            strColumn += "                                        </div>\r\n";
                            strColumn += "                                    </div>\r\n";
                        }

                        strColumnItemList += strColumn;
                    }

                    //获取参数
                    string strPara = "";
                    if (strColumnComment.Contains("{勾选}"))
                    {
                        strPara = "\r\n            var " + strColumnName + " = \"\";\r\n";
                        strPara += "            if ($(\"input[name='" + strColumnName + "']:checkbox:checked\").length > 0) {\r\n";
                        strPara += "                " + strColumnName + " = \"1\";\r\n";
                        strPara += "            }\r\n";
                        strPara += "            else\r\n";
                        strPara += "            {\r\n";
                        strPara += "                " + strColumnName + " = \"0\";\r\n";
                        strPara += "            }\r\n";
                    }
                    else
                    {
                        strPara = "\r\n            var " + strColumnName + " = $('#" + strColumnName + "').val();";
                    }

                    strParaList += strPara;

                    //提交参数
                    string strParaJson = "'" + strName + "': " + strColumnName + "";
                    strParaJsonList += strParaJson + ",";

                    nNum++;
                }
                dr.Dispose();
                cn.Close();

                int nextNum = nNum;
                int nextNum2 = nNum + 1;

                //替换表名-2
                strContent = strContent.Replace("{title}", "新增" + strTableNameComment);

                //替换字段对应的编辑选项-3
                strContent = strContent.Replace("{column_list}", strColumnItemList);

                //替换参数-4
                strContent = strContent.Replace("{setparas}", strParaList);

                //替换参数-5
                if (strParaJsonList.Length > 0)
                {
                    strParaJsonList = strParaJsonList.Substring(0, strParaJsonList.Length - 1);
                }
                strContent = strContent.Replace("json_data_paras", strParaJsonList);

                //存储下拉表
                //生成add.aspx.cs文件
                StreamWriter sw9 = new StreamWriter(strManageFilePath + "\\Add.aspx.cs", false, Encoding.GetEncoding("utf-8"));
                sw9.WriteLine("using System;");
                sw9.WriteLine("using System.Collections.Generic;");
                sw9.WriteLine("using System.Web.UI;");
                sw9.WriteLine("");
                sw9.WriteLine("/// <summary>");
                sw9.WriteLine("/// " + strTableNameComment + "新增数据");
                sw9.WriteLine("/// </summary>");
                sw9.WriteLine("public partial class " + strClassName + "_Add : System.Web.UI.Page");
                sw9.WriteLine("{");
                sw9.WriteLine("");
                sw9.WriteLine(strDefineValueList);
                sw9.WriteLine("");
                sw9.WriteLine("    protected void Page_Load(object sender, EventArgs e)");
                sw9.WriteLine("    {");
                sw9.WriteLine("        if (!Page.IsPostBack)");
                sw9.WriteLine("        {");
                sw9.WriteLine("");
                sw9.WriteLine("            try");
                sw9.WriteLine("            {");
                //sw9.WriteLine("                //清除缓存图片");
                //sw9.WriteLine("                "+strProjectName+ ".Utility.CommonHelper.SaveCookie(\"" + strProjectName + "_Admin_UploadPath\", \"\", 1);");
                //sw9.WriteLine("                " + strProjectName + ".Utility.CommonHelper.SaveCookie(\"" + strProjectName + "_Admin_UploadFileName\", \"\", 1);");
                sw9.WriteLine(strParaSelectList);
                sw9.WriteLine("            }");
                sw9.WriteLine("            catch (Exception ex)");
                sw9.WriteLine("            {");
                sw9.WriteLine("                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strClassName + "_Add), ex, \"\", \"Page_Load\", true);");
                sw9.WriteLine("            }");
                sw9.WriteLine("");
                sw9.WriteLine("        }");
                sw9.WriteLine("    }");
                sw9.WriteLine("}");
                sw9.Close();

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "GetAddPageHTMLData", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取修改页面HTML代码
        /// </summary>
        /// <param name="strContent">HTML源码内容</param>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetEditPageHTMLData(string strContent, string strTableName, string strManageFilePath, string strProjectName)
        {
            string strReturnValue = "";

            try
            {
                string strClassName = CommonHelper.GetClassName(strTableName).ToLower();

                //替换文件名-1
                strContent = strContent.Replace("{classname}", strClassName);

                string strUpperTableName = CommonHelper.GetClassName(strTableName);
                strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);

                string strTableNameComment = "";//表注释
                string strColumnItemList = "";
                string strParaList = "";//参数
                string strParaJsonList = "";//参数
                string strParaValueList = "";//参数
                string strUnionTable = "";
                string strDefineValueList = "";
                string strDefineValueList2 = "";
                string strParaSelectList = "";
                string strPrimaryKey = "";
                string strPrimaryKeyUpper = "";
                string strImgKey = "";
                string strHiddenKeyId = "";

                int nNum = 0;
                int nSelectNum = 0;
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
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);

                    strDataType = StringHelper.GetCSharpDBType(strDataType);
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);

                    //通过表中主键获取表名注释

                    if (strColumnKey == "PRI")
                    {
                        strColumnComment = strColumnComment.Replace("表ID", "");
                        strTableNameComment = strTableNameComment = strColumnComment;
                        strPrimaryKey = strColumnName;
                        strPrimaryKeyUpper = CommonHelper.GetTableNameUpper(strPrimaryKey);

                        strHiddenKeyId = "    <input type=\"hidden\" id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" value=\"<%=strShow" + strPrimaryKeyUpper + " %>\" />";
                    }

                    //不包含主键
                    if (strColumnKey != "PRI")
                    {
                        //获取注释中的表名
                        string strGetUnionTableName = "";
                        string strGetSelectTableName = "";
                        if (strColumnComment.Contains("["))
                        {
                            string[] splitCommentsA = strColumnComment.Split(new char[] { '[' });
                            string strGetSplitStr = splitCommentsA[1];
                            string[] splitCommentsB = strGetSplitStr.Split(new char[] { ']' });
                            strGetUnionTableName = splitCommentsB[0];//获取关联表名
                            strGetSelectTableName = strGetUnionTableName;

                            strUnionTable += strGetUnionTableName + ",";//存储给asp.cs页面使用

                            strGetUnionTableName = CommonHelper.GetClassName(strGetUnionTableName);
                            strGetUnionTableName = CommonHelper.GetTableNameUpper(strGetUnionTableName);

                        }

                        string strShowSelectValue = "strShow" + strGetUnionTableName + "DataList";
                        string strDefineValue2 = "    public string strShow" + strName + " = \"\";\r\n";
                        strDefineValueList2 += strDefineValue2;

                        //获取字段对应的编辑选项
                        string strColumn = "";
                        if (strColumnComment.Contains("{下拉}"))
                        {
                            string strDefineValue = "    public string " + strShowSelectValue + " = \"\";\r\n";
                            strDefineValueList += strDefineValue;

                            //UI页面
                            strColumn = "\r\n                                    <div class=\"form-group\">\r\n";
                            strColumn += "                                        <label for=\"cname\" class=\"control-label col-lg-2\">" + strColumnComment2 + " <!--<span class=\"color-red\">(*)</span>--></label>\r\n";
                            strColumn += "                                        <div class=\"col-lg-4\">\r\n";
                            strColumn += "                                            <select class=\"form-control m-bot15\" id=\"" + strColumnName + "\">\r\n";
                            strColumn += "                                                <option value =\"-1\">请选择</option>\r\n";
                            strColumn += "                                                <%=" + strShowSelectValue + " %>\r\n";
                            strColumn += "                                            </select>\r\n";
                            strColumn += "                                        </div>\r\n";
                            strColumn += "                                    </div>\r\n";

                            //aspx.cs页面
                            if (!string.IsNullOrEmpty(strGetUnionTableName))
                            {
                                string strColumnSelect = "\r\n                //" + strGetUnionTableName + "\r\n";
                                strColumnSelect += "                List<" + strProjectName + ".Model." + strGetUnionTableName + "Model> list" + nSelectNum + " = " + strProjectName + ".BLL." + strGetUnionTableName + "BLL.SelectAllByWhere(\"\");\r\n";
                                strColumnSelect += "                foreach(" + strProjectName + ".Model." + strGetUnionTableName + "Model data in list" + nSelectNum + ")\r\n";
                                strColumnSelect += "                {\r\n";
                                strColumnSelect += GetDataListForEditPage(strGetSelectTableName);//传入参数
                                strColumnSelect += "                }\r\n";

                                strParaSelectList += strColumnSelect;
                                nSelectNum++;
                            }

                        }
                        else if (strColumnComment.Contains("{勾选}"))
                        {
                            strColumn = "\r\n                                    <div class=\"form-group\">\r\n";
                            strColumn += "                                        <label for=\"cname\" class=\"control-label col-lg-2 col-sm-3\">" + strColumnComment2 + " <!--<span class=\"color-red\">(*)</span>--></label>\r\n";
                            strColumn += "                                        <div class=\"col-lg-4  col-sm-9\">\r\n";
                            strColumn += "                                            <input type=\"checkbox\" id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" style=\"width: 20px\" class=\"checkbox form-control\" <%=strShow" + strName + " %>  />\r\n";
                            strColumn += "                                        </div>\r\n";
                            strColumn += "                                    </div>\r\n";
                        }
                        else
                        {
                            strColumn = "\r\n                                    <div class=\"form-group\">\r\n";
                            strColumn += "                                        <label for=\"cname\" class=\"control-label col-lg-2\">" + strColumnComment2 + " <!--<span class=\"color-red\">(*)</span>--></label>\r\n";
                            strColumn += "                                        <div class=\"col-lg-4\">\r\n";
                            strColumn += "                                            <input class=\"form-control\" type=\"text\" id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" value=\"<%=strShow" + strName + " %>\" placeholder=\"请输入" + strColumnComment2 + "\" maxlength=\"20\"/>\r\n";
                            strColumn += "                                        </div>\r\n";
                            strColumn += "                                    </div>\r\n";
                        }

                        strColumnItemList += strColumn;
                    }

                    //获取参数
                    string strPara = "";
                    if (strColumnComment.Contains("{勾选}"))
                    {
                        strPara = "\r\n            var " + strColumnName + " = \"\";\r\n";
                        strPara += "            if ($(\"input[name='" + strColumnName + "']:checkbox:checked\").length > 0) {\r\n";
                        strPara += "                " + strColumnName + " = \"1\";\r\n";
                        strPara += "            }\r\n";
                        strPara += "            else\r\n";
                        strPara += "            {\r\n";
                        strPara += "                " + strColumnName + " = \"0\";\r\n";
                        strPara += "            }\r\n";
                    }
                    else
                    {
                        strPara = "\r\n            var " + strColumnName + " = $('#" + strColumnName + "').val();";
                    }

                    strParaList += strPara;

                    //提交参数
                    string strParaJson = "'" + strName + "': " + strColumnName + "";
                    strParaJsonList += strParaJson + ",";

                    //获取读取数据字段赋值到UI页面
                    string strValue = "";
                    //包含主键
                    if (strDataType == "int")
                    {
                        strValue = "                int n" + strName + " = model." + strName + ";\r\n";
                    }
                    else if (strDataType == "float")
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ".ToString();\r\n";
                    }
                    else if (strDataType == "double")
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ".ToString();\r\n";
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ".ToString();\r\n";
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = "                bool b" + strName + " = model." + strName + ";\r\n";
                        strValue += "                if (b" + strName + ")\r\n";
                        strValue += "                {\r\n";
                        strValue += "                    strShow" + strName + " = \"checked\";\r\n";
                        strValue += "                }\r\n";

                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ".ToString();\r\n";
                    }
                    else if (strDataType == "string")
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ";\r\n";
                        string strNameLower = strName.ToLower();
                        if (strNameLower.Contains("img") || strNameLower.Contains("image") || strNameLower.Contains("avatar"))
                        {
                            strValue += "                strShowImg = " + strProjectName + ".Utility.ConfigHelper.GetImgURL() + strShow" + strName + ";\r\n";
                            strValue += "                strShowImgUrl =strShow" + strName + ";\r\n";
                            strImgKey = CommonHelper.GetTableNameUpper(strName);
                        }

                    }
                    else
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ";\r\n";
                    }

                    strParaValueList += strValue;

                    nNum++;
                }
                dr.Dispose();
                cn.Close();

                int nextNum = nNum;
                int nextNum2 = nNum + 1;

                //替换表名-2
                strContent = strContent.Replace("{title}", "修改" + strTableNameComment);

                //替换字段对应的编辑选项-3
                strContent = strContent.Replace("{column_list}", strColumnItemList);

                //替换参数-4
                strContent = strContent.Replace("{setparas}", strParaList);

                //替换参数-5
                if (strParaJsonList.Length > 0)
                {
                    strParaJsonList = strParaJsonList.Substring(0, strParaJsonList.Length - 1);
                }
                strContent = strContent.Replace("json_data_paras", strParaJsonList);

                //替换隐藏参数
                strContent = strContent.Replace("{hiddenparas}", strHiddenKeyId);

                //存储下拉表
                //生成edit.aspx.cs文件
                StreamWriter sw = new StreamWriter(strManageFilePath + "\\Edit.aspx.cs", false, Encoding.GetEncoding("utf-8"));
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using System.Web.UI;");
                sw.WriteLine("");
                sw.WriteLine("/// <summary>");
                sw.WriteLine("/// " + strTableNameComment + "编辑数据");
                sw.WriteLine("/// </summary>");
                sw.WriteLine("public partial class " + strClassName + "_Edit : System.Web.UI.Page");
                sw.WriteLine("{");
                sw.WriteLine("");
                sw.WriteLine("    public string strShow" + strPrimaryKeyUpper + " = \"\";");
                sw.WriteLine("    public string strShowImg = \"\";");
                sw.WriteLine("    public string strShowImgUrl = \"\";");
                sw.WriteLine(strDefineValueList2 + strDefineValueList);
                sw.WriteLine("");
                sw.WriteLine("    protected void Page_Load(object sender, EventArgs e)");
                sw.WriteLine("    {");
                sw.WriteLine("        if (!Page.IsPostBack)");
                sw.WriteLine("        {");
                sw.WriteLine("");
                sw.WriteLine("            try");
                sw.WriteLine("            {");
                sw.WriteLine("                string str" + strPrimaryKeyUpper + " = Request.QueryString[\"id\"].ToString();");
                sw.WriteLine("                strShow" + strPrimaryKeyUpper + " = str" + strPrimaryKeyUpper + ";");
                sw.WriteLine("                " + strProjectName + ".Model." + strUpperTableName + "Model model = " + strProjectName + ".BLL." + strUpperTableName + "BLL.SelectOneByWhere(\"WHERE " + strPrimaryKey + "=\"+ str" + strPrimaryKeyUpper + ");");
                if (!string.IsNullOrEmpty(strImgKey))
                {
                    sw.WriteLine("                " + strProjectName + ".Utility.CommonHelper.SaveCookie(\"" + strProjectName + "_Admin_UploadFileName\", model." + strImgKey + ", 1);");
                }
                sw.WriteLine("");
                sw.WriteLine(strParaValueList);
                sw.WriteLine("");
                sw.WriteLine(strParaSelectList);
                sw.WriteLine("            }");
                sw.WriteLine("            catch (Exception ex)");
                sw.WriteLine("            {");
                sw.WriteLine("                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strClassName + "_Edit), ex, \"\", \"Page_Load\", true);");
                sw.WriteLine("            }");
                sw.WriteLine("");
                sw.WriteLine("        }");
                sw.WriteLine("    }");
                sw.WriteLine("}");
                sw.Close();

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "GetAddPageHTMLData", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取显示页面HTML代码
        /// </summary>
        /// <param name="strContent">HTML源码内容</param>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetViewPageHTMLData(string strContent, string strTableName, string strManageFilePath, string strProjectName)
        {
            string strReturnValue = "";

            try
            {
                string strClassName = CommonHelper.GetClassName(strTableName).ToLower();

                //替换文件名-1
                strContent = strContent.Replace("{classname}", strClassName);

                string strUpperTableName = CommonHelper.GetClassName(strTableName);
                strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);

                string strTableNameComment = "";//表注释
                string strColumnItemList = "";
                string strParaValueList = "";//参数
                string strUnionTable = "";
                string strDefineValueList = "";
                string strDefineValueList2 = "";
                string strParaSelectList = "";
                string strPrimaryKey = "";
                string strPrimaryKeyUpper = "";
                string strImgKey = "";
                string strHiddenKeyId = "";

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
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);

                    strDataType = StringHelper.GetCSharpDBType(strDataType);
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);

                    //通过表中主键获取表名注释

                    if (strColumnKey == "PRI")
                    {
                        strColumnComment = strColumnComment.Replace("表ID", "");
                        strTableNameComment = strTableNameComment = strColumnComment;
                        strPrimaryKey = strColumnName;
                        strPrimaryKeyUpper = CommonHelper.GetTableNameUpper(strPrimaryKey);

                        strHiddenKeyId = "    <input type=\"hidden\" id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" value=\"<%=strShow" + strPrimaryKeyUpper + " %>\" />";
                    }

                    //不包含主键
                    if (strColumnKey != "PRI")
                    {
                        //获取注释中的表名
                        string strGetUnionTableName = "";
                        string strGetSelectTableName = "";
                        if (strColumnComment.Contains("["))
                        {
                            string[] splitCommentsA = strColumnComment.Split(new char[] { '[' });
                            string strGetSplitStr = splitCommentsA[1];
                            string[] splitCommentsB = strGetSplitStr.Split(new char[] { ']' });
                            strGetUnionTableName = splitCommentsB[0];//获取关联表名
                            strGetSelectTableName = strGetUnionTableName;

                            strUnionTable += strGetUnionTableName + ",";//存储给aspx.cs页面使用

                            strGetUnionTableName = CommonHelper.GetClassName(strGetUnionTableName);
                            strGetUnionTableName = CommonHelper.GetTableNameUpper(strGetUnionTableName);

                        }

                        string strShowSelectValue = "strShow" + strGetUnionTableName + "DataList";
                        string strDefineValue2 = "    public string strShow" + strName + " = \"\";\r\n";
                        strDefineValueList2 += strDefineValue2;

                        //获取字段对应的编辑选项
                        string strColumn = "";
                        if (strColumnComment.Contains("头像") || strColumnComment.Contains("图片") || strColumnComment.Contains("封面"))
                        {
                            strColumn = "\r\n                                    <div class=\"form-group\">\r\n";
                            strColumn += "                                        <label for=\"cname\" class=\"control-label col-lg-2\">" + strColumnComment2 + "</label>\r\n";
                            strColumn += "                                        <div class=\"col-lg-4\">\r\n";
                            strColumn += "                                            <%=strShow" + strName + " %>\r\n";
                            strColumn += "                                        </div>\r\n";
                            strColumn += "                                    </div>\r\n";
                        }
                        else
                        {
                            strColumn = "\r\n                                    <div class=\"form-group\">\r\n";
                            strColumn += "                                        <label for=\"cname\" class=\"control-label col-lg-2\">" + strColumnComment2 + "</label>\r\n";
                            strColumn += "                                        <div class=\"col-lg-4\">\r\n";
                            strColumn += "                                            <span class=\"form-control\" style=\"border: 0; color: #999;\"><%=strShow" + strName + " %></span>\r\n";
                            strColumn += "                                        </div>\r\n";
                            strColumn += "                                    </div>\r\n";
                        }

                        strColumnItemList += strColumn;
                    }


                    //获取读取数据字段赋值到UI页面
                    string strValue = "";
                    //包含主键
                    if (strDataType == "int")
                    {
                        strValue = "                int n" + strName + " = model." + strName + ";\r\n";
                        strValue += "                strShow" + strName + " = n" + strName + ".ToString();\r\n";
                        if (strColumnComment.Contains("types"))
                        {
                            //解析表名
                            string strGetTableName = "";
                            try
                            {
                                strColumnComment = strColumnComment.Replace("[", ",");
                                strColumnComment = strColumnComment.Replace("]", ",");
                                string[] splitComment = strColumnComment.Split(new char[] { ',' });
                                if (splitComment.Length > 0)
                                {
                                    strGetTableName = splitComment[1];
                                }
                            }
                            catch (Exception ex)
                            {
                                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "解析表名", "SetValuesPara", false);
                            }
                            strGetTableName = CommonHelper.GetClassName(strGetTableName);
                            strGetTableName = CommonHelper.GetTableNameUpper(strGetTableName);

                            strValue += "                strShow" + strName + " = " + strProjectName + ".BLL." + strGetTableName + "BLL.GetColumnValueByWhere(\"type_name\", \"WHERE type_id=\" + n" + strName + ");\r\n";
                        }
                    }
                    else if (strDataType == "float")
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ".ToString();\r\n";
                    }
                    else if (strDataType == "double")
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ".ToString();\r\n";
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ".ToString();\r\n";
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = "                bool b" + strName + " = model." + strName + ";\r\n";
                        strValue += "                if (b" + strName + ")\r\n";
                        strValue += "                {\r\n";
                        strValue += "                    strShow" + strName + " = \"是\";\r\n";
                        strValue += "                }\r\n";
                        strValue += "                else\r\n";
                        strValue += "                {\r\n";
                        strValue += "                    strShow" + strName + " = \"否\";\r\n";
                        strValue += "                }\r\n";

                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ".ToString();\r\n";
                    }
                    else if (strDataType == "string")
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ";\r\n";
                        string strNameLower = strName.ToLower();
                        if (strNameLower.Contains("img") || strNameLower.Contains("image") || strNameLower.Contains("avatar"))
                        {
                            strValue += "                if (string.IsNullOrEmpty(model." + strName + "))\r\n";
                            strValue += "                {\r\n";
                            strValue += "                    strShow" + strName + " = \"<img src=\\\"/images/noimg.jpg\\\" width=\\\"100\\\" height=\\\"100\\\">\";\r\n";
                            strValue += "                }\r\n";
                            strValue += "                else\r\n";
                            strValue += "                {\r\n";
                            strValue += "                    strShow" + strName + " = \"<img src=\\\"\" + " + strProjectName + ".Utility.ConfigHelper.GetImgURL() + model." + strName + " + \"\\\" width=\\\"100\\\" height=\\\"100\\\">\";\r\n";
                            strValue += "                }\r\n";
                            strImgKey = CommonHelper.GetTableNameUpper(strName);
                        }

                    }
                    else
                    {
                        strValue = "                strShow" + strName + " = model." + strName + ";\r\n";
                    }

                    strParaValueList += strValue;

                    nNum++;
                }
                dr.Dispose();
                cn.Close();

                int nextNum = nNum;
                int nextNum2 = nNum + 1;

                //替换表名-2
                strContent = strContent.Replace("{title}", strTableNameComment);

                //替换字段对应的编辑选项-3
                strContent = strContent.Replace("{column_items}", strColumnItemList);

                //存储下拉表
                //生成view.aspx.cs文件
                StreamWriter sw = new StreamWriter(strManageFilePath + "\\View.aspx.cs", false, Encoding.GetEncoding("utf-8"));
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using System.Web.UI;");
                sw.WriteLine("");
                sw.WriteLine("/// <summary>");
                sw.WriteLine("/// " + strTableNameComment + "详情数据");
                sw.WriteLine("/// </summary>");
                sw.WriteLine("public partial class " + strClassName + "_View : System.Web.UI.Page");
                sw.WriteLine("{");
                sw.WriteLine("");
                sw.WriteLine("    public string strShow" + strPrimaryKeyUpper + " = \"\";");
                sw.WriteLine("    public string strShowImg = \"\";");
                sw.WriteLine(strDefineValueList2 + strDefineValueList);
                sw.WriteLine("");
                sw.WriteLine("    protected void Page_Load(object sender, EventArgs e)");
                sw.WriteLine("    {");
                sw.WriteLine("        if (!Page.IsPostBack)");
                sw.WriteLine("        {");
                sw.WriteLine("");
                sw.WriteLine("            try");
                sw.WriteLine("            {");
                sw.WriteLine("                string str" + strPrimaryKeyUpper + " = Request.QueryString[\"id\"].ToString();");
                sw.WriteLine("                strShow" + strPrimaryKeyUpper + " = str" + strPrimaryKeyUpper + ";");
                sw.WriteLine("                " + strProjectName + ".Model." + strUpperTableName + "Model model = " + strProjectName + ".BLL." + strUpperTableName + "BLL.SelectOneByWhere(\"WHERE " + strPrimaryKey + "=\"+ str" + strPrimaryKeyUpper + ");");
                sw.WriteLine("");
                sw.WriteLine(strParaValueList);
                sw.WriteLine("");
                sw.WriteLine(strParaSelectList);
                sw.WriteLine("            }");
                sw.WriteLine("            catch (Exception ex)");
                sw.WriteLine("            {");
                sw.WriteLine("                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strClassName + "_View), ex, \"\", \"Page_Load\", true);");
                sw.WriteLine("            }");
                sw.WriteLine("");
                sw.WriteLine("        }");
                sw.WriteLine("    }");
                sw.WriteLine("}");
                sw.Close();

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "获取显示页面HTML代码", "GetViewPageHTMLData", false);
            }

            return strReturnValue;
        }


        /// <summary>
        /// 生成搜搜页面代码
        /// </summary>
        /// <param name="strContent">HTML源码内容</param>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetSearchPageHTMLData(string strContent, string strTableName, string strManageFilePath, string strProjectName)
        {
            string strReturnValue = "";

            try
            {
                string strClassName = CommonHelper.GetClassName(strTableName).ToLower();

                //替换文件名-1
                strContent = strContent.Replace("{classname}", strClassName);

                string strUpperTableName = CommonHelper.GetClassName(strTableName);
                strUpperTableName = CommonHelper.GetTableNameUpper(strUpperTableName);

                string strTableNameComment = "";//表注释
                string strColumnUIParaList = "";
                string strColumnCodeParaList = "";
                string strWhereParas = "";//拼接搜索条件
                string strSearchColumnParas = "";//需要加入搜索条件的字段
                string strUIFunctionSearchParas = "";//UI搜索传入条件
                string strUIFunctionSearchParasUrl = "";//UI搜索传入条件

                string strTableTHList = "";//表头
                string strEditLineList = "";
                string strPostEditDataList = "";
                string strPostParasList = "";
                string strColumnParaList = "";
                string strAlertColumnParaList = "";
                string strPostParaList = "";
                string strAddLineList = "";
                string strAddDataLineList = "";
                string strAddParasList = "";
                string strColumnParaListAdd = "";
                string strAddParaList = "";
                string strCancelDataLineList = "";
                string strColumnItemList = "";
                string strDeletePara = "";
                string strDelPrimaryKey = "";

                string strUnionTable = "";
                string strDefineValueList = "";
                string strDefineValueList2 = "";
                string strPrimaryKey = "";
                string strPrimaryKeyUpper = "";
                string strHiddenKeyId = "";

                int nNum = 0;
                int nNum2 = 0;
                int nSelectNum = 0;
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT,DATA_TYPE FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    nNum2 = nNum + 1;

                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strDataType = dr["DATA_TYPE"].ToString();//数据类型
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);

                    strDataType = StringHelper.GetCSharpDBType(strDataType);
                    string strName = CommonHelper.GetTableNameUpper(strColumnName);

                    //通过表中主键获取表名注释

                    if (strColumnKey == "PRI")
                    {
                        strColumnComment = strColumnComment.Replace("表ID", "");
                        strTableNameComment = strTableNameComment = strColumnComment;
                        strPrimaryKey = strColumnName;
                        strPrimaryKeyUpper = CommonHelper.GetTableNameUpper(strPrimaryKey);

                        strHiddenKeyId = "    <input type=\"hidden\" id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" value=\"<%=strShow" + strPrimaryKeyUpper + " %>\"/>";

                    }

                    //不包含主键
                    if (strColumnKey != "PRI")
                    {
                        //获取注释中的表名
                        string strGetUnionTableName = "";
                        string strGetSelectTableName = "";
                        if (strColumnComment.Contains("["))
                        {
                            string[] splitCommentsA = strColumnComment.Split(new char[] { '[' });
                            string strGetSplitStr = splitCommentsA[1];
                            string[] splitCommentsB = strGetSplitStr.Split(new char[] { ']' });
                            strGetUnionTableName = splitCommentsB[0];//获取关联表名
                            strGetSelectTableName = strGetUnionTableName;

                            strUnionTable += strGetUnionTableName + ",";//存储给asp.cs页面使用

                            strGetUnionTableName = CommonHelper.GetClassName(strGetUnionTableName);
                            strGetUnionTableName = CommonHelper.GetTableNameUpper(strGetUnionTableName);

                        }

                        //获取字段对应的编辑选项
                        string strColumn = "";
                        string strColumnCode = "";
                        string strSearchParas = "";
                        string strSearchParaUrl = "";
                        if (strColumnComment.Contains("{搜索}"))
                        {
                            string strSearchName = "str" + strName;
                            strSearchColumnParas += strSearchName + " + ";

                            string strDefineValue = "    public string strShow" + strName + " = \"\";\r\n";
                            strDefineValueList += strDefineValue;

                            strSearchParaUrl = " + \"&" + strColumnName + "=\" + " + strColumnName + "";

                            //构造参数
                            if (strColumnComment.Contains("{勾选}"))
                            {
                                //UI
                                strColumn = "\r\n                                                <div class=\"col-lg-2\">\r\n";
                                strColumn += "                                                    &nbsp;\r\n";
                                strColumn += "                                                    <div class=\"checkbox\">\r\n";
                                strColumn += "                                                        <label><input type=\"checkbox\" <%=strShow" + strName + " %> id=\"" + strColumnName + "\" name=\"" + strColumnName + "\"  checked=\"checked\"></label>" + strColumnComment2 + "\r\n";
                                strColumn += "                                                    </div>\r\n";
                                strColumn += "                                                </div>\r\n";

                                //Search
                                strSearchParas = "\r\n                var " + strColumnName + " = \"\";\r\n";
                                strSearchParas += "                if ($(\"input[name='" + strColumnName + "']:checkbox:checked\").length > 0) {\r\n";
                                strSearchParas += "                    " + strColumnName + " = \"1\";\r\n";
                                strSearchParas += "                }\r\n";
                                strSearchParas += "                else {\r\n";
                                strSearchParas += "                    " + strColumnName + " = \"0\";\r\n";
                                strSearchParas += "                }\r\n";

                                //Code
                                strColumnCode = "\r\n                //" + strColumnComment2 + "\r\n";
                                strColumnCode += "                string str" + strName + " = \"\";\r\n";
                                strColumnCode += "                if (Request.QueryString[\"" + strColumnName + "\"] == null)\r\n";
                                strColumnCode += "                {\r\n";
                                strColumnCode += "                    str" + strName + " = \"\";\r\n";
                                strColumnCode += "                    strShow" + strName + " = \"checked=checked\";\r\n";
                                strColumnCode += "                }\r\n";
                                strColumnCode += "                else\r\n";
                                strColumnCode += "                {\r\n";
                                strColumnCode += "                    string strGet" + strName + " = Request.QueryString[\"" + strColumnName + "\"].ToString();\r\n";
                                strColumnCode += "                    if (!string.IsNullOrEmpty(strGet" + strName + "))\r\n";
                                strColumnCode += "                    {\r\n";
                                strColumnCode += "                        if (strGet" + strName + " == \"1\")\r\n";
                                strColumnCode += "                        {\r\n";
                                strColumnCode += "                            str" + strName + " = \" AND " + strColumnName + "=1\";\r\n";
                                strColumnCode += "                            strShow" + strName + " = \"checked= checked\";\r\n";
                                strColumnCode += "                        }\r\n";
                                strColumnCode += "                        else\r\n";
                                strColumnCode += "                        {\r\n";
                                strColumnCode += "                            str" + strName + " = \" AND " + strColumnName + "=0\";\r\n";
                                strColumnCode += "                            strShow" + strName + " = \"\";\r\n";
                                strColumnCode += "                        }\r\n";
                                strColumnCode += "                    }\r\n";
                                strColumnCode += "                }\r\n";


                            }
                            else if (strColumnComment.Contains("{下拉}"))
                            {
                                string strDataListPara = "strShow" + strGetUnionTableName + "Id";
                                string strDefineValueSpecial = "    public string strShow" + strGetUnionTableName + "Id = \"\";\r\n";
                                strDefineValueList += strDefineValueSpecial;

                                //UI
                                strColumn = "\r\n                                                <div class=\"col-lg-2\">\r\n";
                                //strColumn += "                                                    " + strColumnComment2 + "：\r\n";
                                strColumn += "                                                    <select class=\"form-control m-bot15\" id=\"" + strColumnName + "\">\r\n";
                                strColumn += "                                                        <option value=\"-1\">请选择</option>\r\n";
                                strColumn += "                                                        <%=" + strDataListPara + " %>\r\n";
                                strColumn += "                                                    </select>\r\n";
                                strColumn += "                                                </div>\r\n";

                                //Search
                                strSearchParas = "                var " + strColumnName + " = $('#" + strColumnName + "').val();\r\n";

                                //Code
                                //default
                                string strColumnSelectPara = "";
                                if (!string.IsNullOrEmpty(strGetUnionTableName))
                                {
                                    string strColumnSelect = "\r\n                //" + strGetUnionTableName + " default\r\n";
                                    strColumnSelect += "                List<" + strProjectName + ".Model." + strGetUnionTableName + "Model> list" + nSelectNum + " = " + strProjectName + ".BLL." + strGetUnionTableName + "BLL.SelectAllByWhere(\"\");\r\n";
                                    strColumnSelect += "                foreach(" + strProjectName + ".Model." + strGetUnionTableName + "Model model in list" + nSelectNum + ")\r\n";
                                    strColumnSelect += "                {\r\n";
                                    strColumnSelect += GetDataListForAddPage(strGetSelectTableName);//传入参数
                                    strColumnSelect += "                }\r\n";

                                    strColumnSelectPara = strColumnSelect;
                                }
                                strColumnSelectPara = strColumnSelectPara.Replace("DataList", "Id");
                                //edit
                                string strColumnSelectEditPara = "";
                                if (!string.IsNullOrEmpty(strGetUnionTableName))
                                {
                                    string strColumnSelect = "\r\n                //" + strGetUnionTableName + " default\r\n";
                                    strColumnSelect += "                List<" + strProjectName + ".Model." + strGetUnionTableName + "Model> list" + nSelectNum + " = " + strProjectName + ".BLL." + strGetUnionTableName + "BLL.SelectAllByWhere(\"\");//注意修改条件\r\n";
                                    strColumnSelect += "                foreach(" + strProjectName + ".Model." + strGetUnionTableName + "Model data in list" + nSelectNum + ")\r\n";
                                    strColumnSelect += "                {\r\n";
                                    strColumnSelect += GetDataListForEditPage(strGetSelectTableName);//传入参数
                                    strColumnSelect += "                }\r\n";

                                    //替换
                                    string strReplace1 = "n" + strName + ")";
                                    string strReplace2 = "int.Parse(strGet" + strName + "))";
                                    strColumnSelect = strColumnSelect.Replace(strReplace1, strReplace2);

                                    strColumnSelectEditPara = strColumnSelect;
                                }
                                strColumnSelectEditPara = strColumnSelectEditPara.Replace("DataList", "Id");

                                strColumnCode = "\r\n                //" + strColumnComment2 + "\r\n";
                                strColumnCode += "                string str" + strName + " = \"\";\r\n";
                                strColumnCode += "                if (Request.QueryString[\"" + strColumnName + "\"] == null)\r\n";
                                strColumnCode += "                {\r\n";
                                strColumnCode += "                    str" + strName + " = \"\";\r\n";
                                strColumnCode += "                    " + strColumnSelectPara + "\r\n";
                                strColumnCode += "                }\r\n";
                                strColumnCode += "                else\r\n";
                                strColumnCode += "                {\r\n";
                                strColumnCode += "                    string strGet" + strName + " = Request.QueryString[\"" + strColumnName + "\"].ToString();\r\n";
                                strColumnCode += "                    if(strGet" + strName + " != \"-1\")\r\n";
                                strColumnCode += "                    {\r\n";
                                strColumnCode += "                        str" + strName + " = \" AND " + strColumnName + "=\" + strGet" + strName + ";\r\n";
                                strColumnCode += "                        " + strColumnSelectEditPara + "\r\n";
                                strColumnCode += "                    }\r\n";
                                strColumnCode += "                    else\r\n";
                                strColumnCode += "                    {\r\n";
                                strColumnCode += "                        str" + strName + " = \"\";\r\n";
                                strColumnCode += "                        " + strColumnSelectPara + "\r\n";
                                strColumnCode += "                    }\r\n";
                                strColumnCode += "                }\r\n";
                            }
                            else
                            {
                                //UI
                                strColumn = "\r\n                                                <div class=\"col-lg-2\">\r\n";
                                strColumn += "                                                    <input type=\"text\" class=\"form-control\" id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" placeholder=\"输入" + strColumnComment2 + "\" value=\"<%=strShow" + strName + " %>\" maxlength=\"20\">\r\n";
                                strColumn += "                                                </div>\r\n";

                                //Search
                                strSearchParas = "                var " + strColumnName + " = $('#" + strColumnName + "').val();\r\n";

                                //Code
                                strColumnCode = "\r\n                //" + strColumnComment2 + "\r\n";
                                strColumnCode += "                string str" + strName + " = \"\";\r\n";
                                strColumnCode += "                if (Request.QueryString[\"" + strColumnName + "\"] == null)\r\n";
                                strColumnCode += "                {\r\n";
                                strColumnCode += "                    str" + strName + " = \"\";\r\n";
                                strColumnCode += "                }\r\n";
                                strColumnCode += "                else\r\n";
                                strColumnCode += "                {\r\n";
                                strColumnCode += "                    string strGet" + strName + " = Request.QueryString[\"" + strColumnName + "\"].ToString();\r\n";
                                strColumnCode += "                    if(!string.IsNullOrEmpty(strGet" + strName + "))\r\n";
                                strColumnCode += "                    {\r\n";
                                strColumnCode += "                        str" + strName + " = \" AND " + strColumnName + " like '%\" + strGet" + strName + " + \"%'\";\r\n";
                                strColumnCode += "                        strShow" + strName + " = strGet" + strName + ";\r\n";
                                strColumnCode += "                    }\r\n";
                                strColumnCode += "                }\r\n";


                            }
                        }

                        strColumnUIParaList += strColumn;
                        strColumnCodeParaList += strColumnCode;
                        strUIFunctionSearchParas += strSearchParas;
                        strUIFunctionSearchParasUrl += strSearchParaUrl;

                    }


                    //管理页面所用的功能
                    string strTableTH = "";//表头

                    //包含主键
                    if (strDataType == "int")
                    {
                        strTableTH = strColumnComment2;
                    }
                    else if (strDataType == "float")
                    {
                        strTableTH = strColumnComment2;
                    }
                    else if (strDataType == "double")
                    {
                        strTableTH = strColumnComment2;
                    }
                    else if (strDataType == "decimal")
                    {
                        strTableTH = strColumnComment2;
                    }
                    else if (strDataType == "bool")
                    {
                        strTableTH = strColumnComment2;
                    }
                    else if (strDataType == "DateTime")
                    {
                        strTableTH = strColumnComment2;
                    }
                    else if (strDataType == "string")
                    {
                        strTableTH = strColumnComment2;
                    }
                    else
                    {
                        strTableTH = strColumnComment2;
                    }

                    if (nNum == 0)
                    {
                        strTableTH = "\r\n                                                    <th>ID</th>\r\n";
                    }
                    else
                    {
                        strTableTH = "                                                    <th>" + strTableTH + "</th>\r\n";
                    }

                    //表头-3
                    strTableTHList += strTableTH;

                    //编辑状态下UI-4
                    string strEditLine = "";
                    if (nNum == 0)
                    {
                        strEditLine = "                                jqTds[" + nNum2 + "].innerHTML = '<input type=\"text\" disabled=\"disabled\" class=\"form-control small\" value=\"' + aData[" + nNum2 + "] + '\">';\r\n";
                    }
                    else
                    {
                        strEditLine = "                                jqTds[" + nNum2 + "].innerHTML = '<input type=\"text\" class=\"form-control small\" value=\"' + aData[" + nNum2 + "] + '\">';\r\n";
                    }

                    strEditLineList += strEditLine;

                    //编辑提交数据-5
                    string strPostEditData = "";
                    if (nNum == 0)
                    {
                        strPostEditData = "\r\n                                oTable.fnUpdate(jqInputs[" + nNum + "].value, nRow, " + nNum + ", false);\r\n";
                    }
                    else
                    {
                        strPostEditData = "                                oTable.fnUpdate(jqInputs[" + nNum + "].value, nRow, " + nNum + ", false);\r\n";
                    }

                    strPostEditDataList += strPostEditData;

                    //编辑提交参数拼合-6
                    string strPostParas = "jqInputs[" + nNum + "].value";
                    strPostParasList += strPostParas + ",";

                    //编辑提交函数参数-7
                    strColumnParaList += strColumnName + ",";
                    strAlertColumnParaList += strColumnName + " + \",\" +";

                    //编辑提交函数参数-8
                    string strPostPara = "'" + strName + "': " + strColumnName;
                    strPostParaList += strPostPara + ",";

                    //新增状态下UI-10
                    string strAddLine = "";
                    if (nNum == 0)
                    {
                        strAddLine = "                                jqTds[" + nNum2 + "].innerHTML = '';\r\n";
                    }
                    else
                    {
                        strAddLine = "                                jqTds[" + nNum2 + "].innerHTML = '<input type=\"text\" class=\"form-control small\" value=\"' + aData[" + nNum2 + "] + '\">';\r\n";
                    }

                    strAddLineList += strAddLine;

                    //新增状态下数据-11
                    string strAddDataLine = "";
                    if (nNum == 0)
                    {
                        strAddDataLine = "                                oTable.fnUpdate('', nRow, 1, false);\r\n";
                    }
                    else
                    {
                        int index = nNum - 1;
                        strAddDataLine = "                                oTable.fnUpdate(jqInputs[" + index + "].value, nRow, " + nNum2 + ", false);\r\n";
                    }

                    strAddDataLineList += strAddDataLine;

                    //新增状态下数据-12
                    //不包含主键
                    if (strColumnKey != "PRI")
                    {
                        int index = nNum - 1;
                        string strAddParas = "jqInputs[" + index + "].value";
                        strAddParasList += strAddParas + ",";

                        strColumnParaListAdd += strColumnName + ",";

                        string strAddPara = "'" + strName + "': " + strColumnName;
                        strAddParaList += strAddPara + ",";
                    }

                    //取消状态下数据-13
                    string strCancelDataLine = "";
                    if (nNum == 0)
                    {
                        strCancelDataLine = "                                oTable.fnUpdate('', nRow, 1, false);\r\n";
                    }
                    else
                    {
                        int index = nNum - 1;
                        strCancelDataLine = "                                oTable.fnUpdate(jqInputs[" + index + "].value, nRow, " + nNum2 + ", false);\r\n";
                    }

                    strCancelDataLineList += strCancelDataLine;

                    //新增数据UI列模型-14
                    string strColumnItem = "''";
                    strColumnItemList += strColumnItem + ",";

                    //删除参数-15
                    if (strColumnKey == "PRI")
                    {
                        strDeletePara = " '" + strName + "': id";
                        strDelPrimaryKey = strName;
                    }

                    nNum++;
                }
                dr.Dispose();
                cn.Close();

                //UI
                //搜索参数
                string strSearchUIParas = "";
                strSearchUIParas = "\r\n                                    <div class=\"form-group\">\r\n";
                strSearchUIParas += "                                        <div class=\"col-lg-12\">\r\n";
                strSearchUIParas += "                                            <div class=\"row\">\r\n";
                strSearchUIParas += strColumnUIParaList;
                strSearchUIParas += "                                            </div>\r\n";
                strSearchUIParas += "                                        </div>\r\n";
                strSearchUIParas += "                                    </div>\r\n";

                //Code
                //开始日期
                string strColumnCodeStartDate = "\r\n                //开始日期\r\n";
                strColumnCodeStartDate += "                string strStartDate = \"\";\r\n";
                strColumnCodeStartDate += "                if (Request.QueryString[\"start_date\"] == null)\r\n";
                strColumnCodeStartDate += "                {\r\n";
                strColumnCodeStartDate += "                   strStartDate = \"\";\r\n";
                strColumnCodeStartDate += "                   strShowStartDate = \"\";\r\n";
                strColumnCodeStartDate += "                }\r\n";
                strColumnCodeStartDate += "                else\r\n";
                strColumnCodeStartDate += "                {\r\n";
                strColumnCodeStartDate += "                    strStartDate = Request.QueryString[\"start_date\"].ToString();\r\n";
                strColumnCodeStartDate += "                    if (!string.IsNullOrEmpty(strStartDate))\r\n";
                strColumnCodeStartDate += "                    {\r\n";
                strColumnCodeStartDate += "                        strStartDate = Convert.ToDateTime(strStartDate).ToString(\"yyyy-MM-dd\");\r\n";
                strColumnCodeStartDate += "                        strShowStartDate = Convert.ToDateTime(strStartDate).ToString(\"yyyy/MM/dd\");\r\n";
                strColumnCodeStartDate += "                    }\r\n";
                strColumnCodeStartDate += "                }\r\n";
                //结束日期
                string strColumnCodeEndDate = "\r\n                //结束日期\r\n";
                strColumnCodeEndDate += "                string strEndDate = \"\";\r\n";
                strColumnCodeEndDate += "                if (Request.QueryString[\"end_date\"] == null)\r\n";
                strColumnCodeEndDate += "                {\r\n";
                strColumnCodeEndDate += "                   strEndDate = \"\";\r\n";
                strColumnCodeEndDate += "                   strShowEndDate = \"\";\r\n";
                strColumnCodeEndDate += "                   strShowSearchResult = \"style=\\\"display: none; \\\"\";\r\n";
                strColumnCodeEndDate += "                }\r\n";
                strColumnCodeEndDate += "                else\r\n";
                strColumnCodeEndDate += "                {\r\n";
                strColumnCodeEndDate += "                    strEndDate = Request.QueryString[\"end_date\"].ToString();\r\n";
                strColumnCodeEndDate += "                    if (!string.IsNullOrEmpty(strEndDate))\r\n";
                strColumnCodeEndDate += "                    {\r\n";
                strColumnCodeEndDate += "                        strEndDate = Convert.ToDateTime(strEndDate).ToString(\"yyyy-MM-dd\");\r\n";
                strColumnCodeEndDate += "                        strShowEndDate = Convert.ToDateTime(strEndDate).ToString(\"yyyy/MM/dd\");\r\n";
                strColumnCodeEndDate += "                        strShowSearchResult = \"style=\\\"display: block; \\\"\";\r\n";
                strColumnCodeEndDate += "                    }\r\n";
                strColumnCodeEndDate += "                }\r\n";

                strColumnCodeParaList = strColumnCodeParaList + strColumnCodeStartDate + strColumnCodeEndDate;

                //构造搜索条件
                //去掉最后一个+号，还有一个空格，2个字符
                if (strSearchColumnParas.Length > 0)
                {
                    strSearchColumnParas = strSearchColumnParas.Substring(0, strSearchColumnParas.Length - 2);
                }

                string strWhereForDate = "\r\n                    string strSearchDate = \"\";";
                strWhereForDate += "\r\n                    if (strStartDate == strEndDate && !string.IsNullOrEmpty(strStartDate))";
                strWhereForDate += "\r\n                    {";
                strWhereForDate += "\r\n                       strSearchDate = \" AND DATE(created_at)= '\" + strStartDate + \"'\";";
                strWhereForDate += "\r\n                    }";
                strWhereForDate += "\r\n                    else if (string.IsNullOrEmpty(strStartDate)  && string.IsNullOrEmpty(strEndDate))";
                strWhereForDate += "\r\n                    {";
                strWhereForDate += "\r\n                        strSearchDate = \"\";";
                strWhereForDate += "\r\n                    }";
                strWhereForDate += "\r\n                    else";
                strWhereForDate += "\r\n                    {";
                strWhereForDate += "\r\n                        strSearchDate = \" AND created_at >= '\" + strStartDate + \"'  AND created_at<= '\" + strEndDate + \"' \";";
                strWhereForDate += "\r\n                    }";

                if (!string.IsNullOrEmpty(strSearchColumnParas))
                {

                    strWhereParas = strWhereForDate + "\r\n                    strWhere = \"WHERE is_del=0 \" + strSearchDate + " + strSearchColumnParas + ";\r\n";
                }
                else
                {
                    strWhereParas = strWhereForDate + "\r\n                    strWhere = \"WHERE \" + strSearchDate;\r\n";
                }

                int nextNum = nNum;
                int nextNum2 = nNum + 1;

                //替换表名-0
                strContent = strContent.Replace("{title}", strTableNameComment + "查询");

                //替换搜索参数-1
                strContent = strContent.Replace("{search_paras}", strSearchUIParas);

                //替换搜索传入参数-2
                strContent = strContent.Replace("{function_search_paras}", "\r\n" + strUIFunctionSearchParas);

                //替换搜索URL传入参数-3
                strContent = strContent.Replace("{function_search_paras_url}", strUIFunctionSearchParasUrl);

                //替换表头-3
                strContent = strContent.Replace("{th}", strTableTHList);


                //替换编辑状态下UI-4
                strEditLineList = "\r\n                                jqTds[0].innerHTML = '';\r\n" + strEditLineList;
                strEditLineList += "                                jqTds[" + nextNum + "].innerHTML = '<a class=\"edit\" href=\"\">更新</a>';\r\n";
                strEditLineList += "                                jqTds[" + nextNum2 + "].innerHTML = '<a class=\"cancel\" href=\"\">取消</a>';\r\n";

                strContent = strContent.Replace("/*{edit-ui-list}*/", strEditLineList);

                //编辑提交数据-5
                strPostEditDataList += "                                oTable.fnUpdate('<a class=\"edit\" href=\"\">编辑</a>', nRow, " + nextNum + ", false);\r\n";
                strPostEditDataList += "                                oTable.fnUpdate('<a class=\"delete\" href=\"\">删除</a>', nRow, " + nextNum2 + ", false);\r\n";

                strContent = strContent.Replace("/*{edit-post-data-list}*/", strPostEditDataList);

                //编辑提交参数拼合-6
                if (strPostParasList.Length > 0)
                {
                    strPostParasList = strPostParasList.Substring(0, strPostParasList.Length - 1);
                }
                strPostParasList = "editData(" + strPostParasList + ")";

                strContent = strContent.Replace("/*{edit-post-data-para}*/", strPostParasList);

                //编辑提交函数参数-7
                if (strColumnParaList.Length > 0)
                {
                    strColumnParaList = strColumnParaList.Substring(0, strColumnParaList.Length - 1);
                }

                strContent = strContent.Replace("function_column_paras_edit", strColumnParaList);
                strContent = strContent.Replace("function_column_paras_alert", strAlertColumnParaList);


                //编辑提交函数参数-8
                if (strPostParaList.Length > 0)
                {
                    strPostParaList = strPostParaList.Substring(0, strPostParaList.Length - 1);
                }

                strContent = strContent.Replace("edit_column_paras", strPostParaList);

                //编辑提交URL-9
                strContent = strContent.Replace("{tablename}", strClassName);

                //新增状态下UI-10
                strAddLineList = "\r\n                                jqTds[0].innerHTML = '';\r\n" + strAddLineList;
                strAddLineList += "                                jqTds[" + nextNum + "].innerHTML = '<a class=\"edit\" href=\"\">保存</a>';\r\n";
                strAddLineList += "                                jqTds[" + nextNum2 + "].innerHTML = '<a class=\"cancel\" href=\"\">取消</a>';\r\n";

                strContent = strContent.Replace("/*{add-ui-list}*/", strAddLineList);

                //新增状态下数据-11
                strAddDataLineList = "\r\n                                oTable.fnUpdate('', nRow, 0, false);\r\n" + strAddDataLineList;
                strAddDataLineList += "                                oTable.fnUpdate('<a class=\"edit\" href=\"\">编辑</a>', nRow, " + nextNum + ", false);\r\n";
                strAddDataLineList += "                                oTable.fnUpdate('<a class=\"delete\" href=\"\">删除</a>', nRow, " + nextNum2 + ", false);\r\n";

                strContent = strContent.Replace("/*{add-data-list}*/", strAddDataLineList);

                //新增状态下数据-12
                if (strAddParasList.Length > 0)
                {
                    strAddParasList = strAddParasList.Substring(0, strAddParasList.Length - 1);
                }
                strAddParasList = "saveData(" + strAddParasList + ")";
                strContent = strContent.Replace("/*{add-post-data-para}*/", strAddParasList);

                if (strColumnParaListAdd.Length > 0)
                {
                    strColumnParaListAdd = strColumnParaListAdd.Substring(0, strColumnParaListAdd.Length - 1);
                }
                //替换新增数据函数中的参数
                strContent = strContent.Replace("function_column_paras_add", strColumnParaListAdd);

                if (strAddParaList.Length > 0)
                {
                    strAddParaList = strAddParaList.Substring(0, strAddParaList.Length - 1);
                }
                //替换新增数据函数中的提交参数
                strContent = strContent.Replace("add_column_paras", strAddParaList);


                //取消状态下数据-13
                strCancelDataLineList = "\r\n                                oTable.fnUpdate('', nRow, 0, false);\r\n" + strCancelDataLineList;
                strCancelDataLineList += "                                oTable.fnUpdate('<a class=\"edit\" href=\"\">编辑</a>', nRow, " + nextNum + ", false);\r\n";

                strContent = strContent.Replace("/*{cancel-data-list}*/", strCancelDataLineList);

                //新增数据UI列模型-14
                strColumnItemList = "''," + strColumnItemList;
                strColumnItemList = "[" + strColumnItemList + "'<a class=\"edit\" href=\"\">编辑</a>', '<a class=\"cancel\" data-mode=\"new\" href=\"\">取消</a>']";

                strContent = strContent.Replace("add_data_column_model", strColumnItemList);

                //替换删除参数-15
                strContent = strContent.Replace("delete_column_paras", strDeletePara);

                //替换批量删除-16
                strContent = strContent.Replace("{primarykey}", strDelPrimaryKey);

                //存储下拉表
                //生成edit.aspx.cs文件
                StreamWriter sw = new StreamWriter(strManageFilePath + "\\Search.aspx.cs", false, Encoding.GetEncoding("utf-8"));
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using System.Web.UI;");
                sw.WriteLine("");
                sw.WriteLine("/// <summary>");
                sw.WriteLine("/// " + strTableNameComment + "查询数据");
                sw.WriteLine("/// </summary>");
                sw.WriteLine("public partial class " + strClassName + "_Search : System.Web.UI.Page");
                sw.WriteLine("{");
                sw.WriteLine("");
                sw.WriteLine("    public string strShowRoleId = \"\";");
                sw.WriteLine("    public string strShowRank = \"\";");
                sw.WriteLine("    public string strShowCode = \"\";");
                sw.WriteLine("    public string strShowStartDate = \"\";");
                sw.WriteLine("    public string strShowEndDate = \"\";");
                sw.WriteLine("    public string strShowDataList = \"\";");
                sw.WriteLine("    public string strShowSearchResult = \"\";");
                sw.WriteLine(strDefineValueList2 + strDefineValueList);
                sw.WriteLine("");
                sw.WriteLine("    protected void Page_Load(object sender, EventArgs e)");
                sw.WriteLine("    {");
                sw.WriteLine("        if (!Page.IsPostBack)");
                sw.WriteLine("        {");
                sw.WriteLine("");
                sw.WriteLine("            try");
                sw.WriteLine("            {");
                sw.WriteLine("                if (Request.QueryString[\"roleid\"] != null)");
                sw.WriteLine("                {");
                sw.WriteLine("                    strShowRoleId = Request.QueryString[\"roleid\"].ToString();");
                sw.WriteLine("                }");
                sw.WriteLine("                if (Request.QueryString[\"rank\"] != null)");
                sw.WriteLine("                {");
                sw.WriteLine("                    strShowRank = Request.QueryString[\"rank\"].ToString();");
                sw.WriteLine("                }");
                sw.WriteLine("                if (Request.QueryString[\"code\"] != null)");
                sw.WriteLine("                {");
                sw.WriteLine("                    strShowCode = Request.QueryString[\"code\"].ToString();");
                sw.WriteLine("                }");
                sw.WriteLine("");
                sw.WriteLine("" + strColumnCodeParaList);
                sw.WriteLine("                //拼接搜索条件");
                sw.WriteLine("                string strWhere = \"\";");
                sw.WriteLine("                if (Request.QueryString[\"action\"]==null)");
                sw.WriteLine("                {");
                sw.WriteLine("                    //默认读取最新20条数据");
                sw.WriteLine("                    strShowSearchResult = \"style =\\\"display: block; \\\"\"; ");
                sw.WriteLine("                    strWhere = \" WHERE is_del=0 ORDER BY created_at DESC LIMIT 0, 20\";");
                sw.WriteLine("                }");
                sw.WriteLine("                else");
                sw.WriteLine("                {");
                sw.WriteLine("                    " + strWhereParas);
                sw.WriteLine("                }");
                sw.WriteLine("");
                sw.WriteLine("                SearchResults(strWhere);");
                sw.WriteLine("");
                sw.WriteLine("            }");
                sw.WriteLine("            catch (Exception ex)");
                sw.WriteLine("            {");
                sw.WriteLine("                " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strClassName + "_Search), ex, \"获取搜索结果\", \"Page_Load\", true);");
                sw.WriteLine("            }");
                sw.WriteLine("");
                sw.WriteLine("        }");
                sw.WriteLine("    }");
                sw.WriteLine("    protected void SearchResults(string strWhere)");
                sw.WriteLine("    {");
                sw.WriteLine("        //Response.Write(strWhere);");
                sw.WriteLine("        try");
                sw.WriteLine("        {");
                sw.WriteLine("            List<" + strProjectName + ".Model." + strUpperTableName + "Model> list = " + strProjectName + ".BLL." + strUpperTableName + "BLL.SelectAllByWhere(strWhere);");
                sw.WriteLine("            foreach(" + strProjectName + ".Model." + strUpperTableName + "Model model in list)");
                sw.WriteLine("            {");
                sw.WriteLine("");
                sw.WriteLine(GetSearchDataList(strProjectName, strTableName));//传入参数
                sw.WriteLine("");
                sw.WriteLine("             }");
                sw.WriteLine("         }");
                sw.WriteLine("        catch (Exception ex)");
                sw.WriteLine("        {");
                sw.WriteLine("            " + strProjectName + ".Utility.LogHelper.Error(typeof(" + strClassName + "_Search), ex, \"\", \"Page_Load\", true);");
                sw.WriteLine("        }");
                sw.WriteLine("");
                sw.WriteLine("    }");
                sw.WriteLine("}");
                sw.Close();

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "GetAddPageHTMLData", false);
            }

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
                    strColumnComment = CommonHelper.GetColumnKeyComment(strColumnComment);

                    if (strColumnKey == "PRI")
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
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中的主键ID", "GetPrimaryKey", false);
            }

            return strReturnValue;

        }
    }
}
