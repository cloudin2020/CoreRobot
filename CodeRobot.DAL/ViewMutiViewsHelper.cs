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
    /// .net core处理请求类
    /// </summary>
    public class ViewMutiViewsHelper
    {

        /// <summary>
        /// 根据表名创建Views详情页面
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateViewMutiViewsClass(string strFilePath,string strProjectName, string strTableName,string strTableComment)
        {
            //读取版权信息
            CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
            string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
            string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
            string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
            string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
            string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

            string strTableNameUpper = CommonHelper.GetTableNameUpper(strTableName);
            
            string strPrimaryKey = CommonHelper.GetPrimaryKey(strTableName);//如：news_id

            string strAllColumnName = CommonHelper.GetAllColumnName(strTableName);
            string strAllColumnNameNotKey = CommonHelper.GetAllColumnNameNotKey(strTableName);

            string strClassName = CommonHelper.GetClassName(strTableName);//类名
            strClassName = CommonHelper.GetTableNameUpper(strClassName);
            string strTableNameSpec = CommonHelper.GetTableNameFirstLowerSecondUpper(strClassName);//如：newsTypes
            string strTableNameLower = strTableNameSpec.ToLower();//如：newstypes

            Directory.CreateDirectory(strFilePath);
            //为每个表创建一个独立目录存放
            string strTableNamePath = strFilePath + "\\"+ strClassName;
            if (!Directory.Exists(strTableNamePath))
            {
                Directory.CreateDirectory(strTableNamePath);
            }
            StreamWriter sw = new StreamWriter(strTableNamePath + "\\Views.cshtml", false, Encoding.GetEncoding("utf-8"));//生成物理文件

            string strTableComment2 = CommonHelper.GetColumnKeyComment(strTableComment);

            sw.WriteLine("@{");
            sw.WriteLine("    ViewData[\"Title\"] = \""+ strTableComment2 + "详情\";");
            sw.WriteLine("    Layout = \"~/Views/Shared/_LayoutNone.cshtml\";");
            sw.WriteLine("}");
            sw.WriteLine("");
            sw.WriteLine("<div class=\"layui-fluid\">");
            sw.WriteLine("    <div class=\"layui-row layui-col-space15\">");
            sw.WriteLine("        <div class=\"layui-col-md12\">");
            sw.WriteLine("            <div class=\"layui-card\">");
            sw.WriteLine("                <div class=\"layui-card-body\" pad15>");
            sw.WriteLine("");
            sw.WriteLine("                    <div class=\"layui-form\" wid100 lay-filter=\"\">");
            sw.WriteLine(GetViewsItemList(strTableName));
            sw.WriteLine("                    </div>");
            sw.WriteLine("");
            sw.WriteLine("                </div>");
            sw.WriteLine("            </div>");
            sw.WriteLine("        </div>");
            sw.WriteLine("    </div>");
            sw.WriteLine("");
            sw.WriteLine("    <input type=\"hidden\" name=\""+strPrimaryKey+ "\" id=\"" + strPrimaryKey + "\" value=\"@ViewContext.HttpContext.Request.Query[\"id\"]\" />");
            sw.WriteLine("</div>");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("<script src=\"~/layuiadmin/layui/layui.js\"></script>");
            sw.WriteLine("<script>");
            sw.WriteLine("    layui.config({");
            sw.WriteLine("        base: '/layuiadmin/' //静态资源所在路径");
            sw.WriteLine("    }).extend({");
            sw.WriteLine("        index: '/lib/index' //主入口模块");
            sw.WriteLine("    }).use(['index', 'form'], function () {");
            sw.WriteLine("        var $ = layui.$");
            sw.WriteLine("            , admin = layui.admin");
            sw.WriteLine("");
            sw.WriteLine("        //默认加载");
            sw.WriteLine("        $(function () {");
            sw.WriteLine("            var " + strPrimaryKey + " = $(\"#" + strPrimaryKey + "\").val();");
            sw.WriteLine("");
            sw.WriteLine("            //读取数据加载loading..");
            sw.WriteLine("            loading = layer.load(2, {");
            sw.WriteLine("                shade: [0.2, '#000']");
            sw.WriteLine("            });");
            sw.WriteLine("");
            sw.WriteLine("            admin.req({");
            sw.WriteLine("                url: '/"+ strTableNameLower + "/details'");
            sw.WriteLine("                , data: { id: " + strPrimaryKey + " }");
            sw.WriteLine("                , done: function (res) {");
            sw.WriteLine("                    layer.close(loading);");
            sw.WriteLine("                    console.log(res.data);");
            sw.WriteLine(GetViewsItemValueList(strTableName));
            sw.WriteLine("                }");
            sw.WriteLine("            });");
            sw.WriteLine("");
            sw.WriteLine("        });");
            sw.WriteLine("    })");
            sw.WriteLine("</script>");
            sw.Close();

        }

        /// <summary>
        /// 获取详情字段列表
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetViewsItemList(string strTableName)
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
                    string strColumnType = dr["COLUMN_TYPE"].ToString();//注释

                    if (strColumnKey== "PRI" || strColumnName == "created_at")
                    {
                        //
                    }
                    else
                    {
                        if (strColumnType == "longtext")
                        {
                            string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);
  
                            string strValue = "                        <div class=\"layui-form-item\">\r\n";
                            strValue += "                            <label class=\"layui-form-label\">"+ strColumnComment2 + "</label>\r\n";
                            strValue += "                            <div class=\"layui-word-aux layui-input-block\" style=\"line-height: 25px; margin-top: 10px;\" id=\""+ strColumnName + "\"></div>\r\n";
                            strValue += "                        </div>\r\n";
                            strValue += "                        \r\n";

                            strReturnValue += strValue;
                        }
                        else
                        {
                            string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);

                            string strValue = "                        <div class=\"layui-form-item\">\r\n";
                            strValue += "                            <label class=\"layui-form-label\">"+ strColumnComment2 + "</label>\r\n";
                            strValue += "                            <div class=\"layui-form-mid layui-word-aux\" id=\"" + strColumnName + "\"></div>\r\n";
                            strValue += "                        </div>\r\n";
                            strValue += "                        \r\n";

                            strReturnValue += strValue;
                        }
                    }
                    

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取新增字段列表", "GetCreateItemList", false);
            }

            return strReturnValue;

        }


        /// <summary>
        /// 获取编辑字段充填值列表
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetViewsItemValueList(string strTableName)
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

                    if (strColumnKey == "PRI" || strColumnName == "created_at")
                    {
                        //
                    }
                    else
                    {
                        string strValue = "                    $(\"#"+ strColumnName + "\").html(res.data." + strColumnName + ");\r\n";

                        strReturnValue += strValue;
                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取新增字段列表", "GetCreateItemList", false);
            }

            return strReturnValue;

        }


        /// <summary>
        /// 获取监听开关脚本
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetSwitchScript(string strTableName)
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


                    if (strColumnComment.Contains("{勾选}"))
                    {
                        string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);

                        string strValue = "        //监听指定开关\r\n";
                        strValue += "        form.on('switch(switch_"+ strColumnName + ")', function (data) {\r\n";
                        strValue += "            if (this.checked) {\r\n";
                        strValue += "                $(\"#" + strColumnName + "\").val(true);\r\n";
                        strValue += "            } else {\r\n";
                        strValue += "                $(\"#" + strColumnName + "\").val(false);\r\n";
                        strValue += "            }\r\n";
                        strValue += "        });\r\n";

                        strReturnValue += strValue;
                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取监听开关脚本", "GetSwitchScript", false);
            }

            return strReturnValue;

        }

        /// <summary>
        /// 获取富文本编辑器字段
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetEditorKey(string strTableName)
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
                    string strColumnType = dr["COLUMN_TYPE"].ToString();//注释

                    if (strColumnType== "longtext")
                    {
                        strReturnValue = strColumnName;
                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取搜索字段列表", "GetSearchItemList", false);
            }

            return strReturnValue;

        }

        /// <summary>
        /// 获取状态开关字段
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetSwitchStatusKey(string strTableName)
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
                    string strColumnType = dr["COLUMN_TYPE"].ToString();//注释

                    if (strColumnComment.Contains("{勾选}状态"))
                    {
                        strReturnValue = strColumnName;
                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取状态开关字段", "GetSwitchStatusKey", false);
            }

            return strReturnValue;

        }
    }
}
