﻿using System;
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
    public class ViewEditHelper
    {

        /// <summary>
        /// 根据表名创建Edit编辑页面
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateViewEditClass(string strFilePath,string strProjectName, string strTableName,string strTableComment)
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
            StreamWriter sw = new StreamWriter(strTableNamePath + "\\Edit.cshtml", false, Encoding.GetEncoding("utf-8"));//生成物理文件
            string strTableComment2 = CommonHelper.GetColumnKeyComment(strTableComment);

            sw.WriteLine("@{");
            sw.WriteLine("    ViewData[\"Title\"] = \"编辑" + strTableComment2 + "\";");
            sw.WriteLine("    Layout = \"~/Views/Shared/_LayoutNone.cshtml\";");
            sw.WriteLine("}");
            sw.WriteLine("<div class=\"layui-form\" lay-filter=\"layuiadmin-app-form-list\" id=\"layuiadmin-app-form-list\" style=\"padding: 20px 30px 0 0;\">");
            sw.WriteLine(GetEditItemList(strTableName));
            sw.WriteLine("    <div class=\"layui-form-item layui-hide\">");
            sw.WriteLine("        <input type=\"hidden\" id=\"created_at\" name=\"created_at\" value=\"@DateTime.Now.ToString()\" />");
            sw.WriteLine("        <input type=\"hidden\" id=\""+strPrimaryKey+ "\" name=\"" + strPrimaryKey + "\" value=\"@ViewContext.HttpContext.Request.Query[\"id\"]\" />");
            sw.WriteLine("        <input type=\"button\" lay-submit lay-filter=\"layuiadmin-app-form-submit\" id=\"layuiadmin-app-form-submit\" value=\"确认添加\">");
            sw.WriteLine("        <input type=\"button\" lay-submit lay-filter=\"layuiadmin-app-form-edit\" id=\"layuiadmin-app-form-edit\" value=\"确认编辑\">");
            sw.WriteLine("    </div>");
            sw.WriteLine("");
            sw.WriteLine("</div>");
            sw.WriteLine("");
            sw.WriteLine("<script src=\"~/layuiadmin/layui/layui.js\"></script>");
            sw.WriteLine("<script>");
            sw.WriteLine("    layui.config({");
            sw.WriteLine("        base: '/layuiadmin/' //静态资源所在路径");
            sw.WriteLine("    }).extend({");
            sw.WriteLine("        index: '/lib/index' //主入口模块");
            sw.WriteLine("    }).use(['index', 'laydate', 'form'], function () {");
            sw.WriteLine("        var $ = layui.$");
            sw.WriteLine("            , form = layui.form");
            sw.WriteLine("            , admin = layui.admin");
            sw.WriteLine("            , laydate = layui.laydate;");
            sw.WriteLine("");
            sw.WriteLine(GetSwitchScript(strTableName));
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
            sw.WriteLine(GetEditItemValueList(strTableName));
            sw.WriteLine("                    //if (!res.data." + GetSwitchStatusKey(strTableName) + ") {");
            sw.WriteLine("                        //$(\".layui-unselect\").removeClass(\"layui-form-onswitch\");");
            sw.WriteLine("                        //$(\"#switch_" + GetSwitchStatusKey(strTableName) + "\").removeAttr(\"checked\");");
            sw.WriteLine("                        //$(\".layui-unselect>em\").html(\"禁用\");");
            sw.WriteLine("                    //}");
            sw.WriteLine("");
            sw.WriteLine(GetSelectScript(strProjectName, strTableName));
            sw.WriteLine("");
            sw.WriteLine("                }");
            sw.WriteLine("            });");
            sw.WriteLine("");
            sw.WriteLine("        });");
            sw.WriteLine("");
            sw.WriteLine("        //监听提交");
            sw.WriteLine("        form.on('submit(layuiadmin-app-form-edit)', function (data) {");
            sw.WriteLine("            var field = data.field; //获取提交的字段");
            sw.WriteLine("            var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引");
            sw.WriteLine("");
            sw.WriteLine("            //提交 Ajax 成功后，关闭当前弹层并重载表格");
            sw.WriteLine("            loading = layer.load(2, {");
            sw.WriteLine("                shade: [0.2, '#000']");
            sw.WriteLine("            });");
            sw.WriteLine("            console.log('data=' + data.field." + strPrimaryKey + ");");
            sw.WriteLine("            //提交数据");
            sw.WriteLine("            admin.req({");
            sw.WriteLine("                headers: {");
            sw.WriteLine("                    'RequestVerificationToken': csrfToken");
            sw.WriteLine("                },");
            sw.WriteLine("                method: 'POST',");
            sw.WriteLine("                url: '/" + strTableNameLower + "/edit?id=' + data.field." + strPrimaryKey + "");
            sw.WriteLine("                , data: data.field");
            sw.WriteLine("                , done: function (res) {");
            sw.WriteLine("                    layer.close(loading);");
            sw.WriteLine("                    parent.layui.table.reload('LAY-app-content-list'); //重载表格");
            sw.WriteLine("                    layer.msg(res.msg, {");
            sw.WriteLine("                        offset: '15px'");
            sw.WriteLine("                        , icon: 1");
            sw.WriteLine("                        , time: 1000");
            sw.WriteLine("                    }, function () {");
            sw.WriteLine("                        parent.layer.close(index); //再执行关闭");
            sw.WriteLine("                    });");
            sw.WriteLine("                }");
            sw.WriteLine("            });");
            sw.WriteLine("");
            sw.WriteLine("        });");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("    })");
            sw.WriteLine("</script>");

            sw.Close();

        }

        /// <summary>
        /// 获取编辑字段列表
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetEditItemList(string strTableName)
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

                    if (strColumnKey== "PRI" || strColumnName == "created_at")
                    {
                        //
                    }
                    else
                    {

                        if (strColumnComment.Contains("{勾选}"))
                        {
                            string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);
                            string strColumnComment3 = CommonHelper.GetColumnKeyCommentRight(strColumnComment);

                            string strValue = "    <div class=\"layui-form-item\">\r\n";
                            strValue += "        <label class=\"layui-form-label\">" + strColumnComment2 + "</label>\r\n";
                            strValue += "        <div class=\"layui-input-inline\">\r\n";
                            strValue += "            <input type=\"checkbox\" lay-verify=\"required\" lay-filter=\"switch_" + strColumnName + "\" name=\"switch_" + strColumnName + "\" lay-skin=\"switch\" lay-text=\"" + strColumnComment3 + "\" checked=\"checked\">\r\n";
                            strValue += "            <input type=\"hidden\" name=\"" + strColumnName + "\" id=\"" + strColumnName + "\" value=\"true\" />\r\n";
                            strValue += "        </div>\r\n";
                            strValue += "    </div>\r\n";
                            strValue += "    \r\n";

                            strReturnValue += strValue;
                        }
                        else if (strColumnComment.Contains("{下拉}"))
                        {
                            string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);

                            string strValue = "    <div class=\"layui-form-item\">\r\n";
                            strValue += "        <label class=\"layui-form-label\">" + strColumnComment2 + "</label>\r\n";
                            strValue += "        <div class=\"layui-input-block\">\r\n";
                            strValue += "                <select id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" lay-verify=\"required\">\r\n";
                            strValue += "                    <option value=\"\">选择" + strColumnComment2 + "</option>\r\n";
                            strValue += "                </select>\r\n";
                            strValue += "        </div>\r\n";
                            strValue += "    </div>\r\n";
                            strValue += "    \r\n";

                            strReturnValue += strValue;
                        }
                        else
                        {
                            string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);

                            string strValue = "    <div class=\"layui-form-item\">\r\n";
                            strValue += "        <label class=\"layui-form-label\">" + strColumnComment2 + "</label>\r\n";
                            strValue += "        <div class=\"layui-input-block\">\r\n";
                            if (strColumnName.Contains("remark") || strColumnName.Contains("summary"))
                            {
                                strValue += "            <textarea id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" lay-verify=\"required\" placeholder=\"请输入" + strColumnComment2 + "\" autocomplete=\"off\" class=\"layui-textarea\"></textarea>\r\n";
                            }
                            else
                            {
                                strValue += "            <input type=\"text\" id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" lay-verify=\"required\" placeholder=\"请输入" + strColumnComment2 + "\" autocomplete=\"off\" class=\"layui-input\">\r\n";
                            }
                            strValue += "        </div>\r\n";
                            strValue += "    </div>\r\n";
                            strValue += "    \r\n";

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
        public static string GetEditItemValueList(string strTableName)
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
                        string strValue = "                    $(\"#"+ strColumnName + "\").val(res.data." + strColumnName + ");\r\n";

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
        /// 获取默认加载下拉框填充组件
        /// </summary>
        /// <param name="strProjectName"></param>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetSelectScript(string strProjectName, string strTableName)
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

                    //{下拉}[ypk_exhibition_types]会展类型
                    if (strColumnComment.Contains("{下拉}"))
                    {
                        string strComment = "";
                        if (strColumnComment.Contains("["))
                        {
                            string[] splitComment = strColumnComment.Split(new char[] { '[' });
                            strComment = splitComment[1];
                        }
                        if (strComment.Contains("]"))
                        {
                            string[] splitComment = strComment.Split(new char[] { ']' });
                            strComment = splitComment[0];
                        }

                        //表名
                        string strSelectTableName = strComment;
                        string strSelectId = CommonHelper.GetPrimaryKey(strSelectTableName);
                        string strSelectName = CommonHelper.GetKeyNameForSelect(strSelectTableName);

                        strComment = strComment.Replace(strProjectName.ToLower() + "_", "");
                        string strRouterName = strComment.ToLower();//如：newstypes
                        strRouterName = strRouterName.Replace("_", "");

                        string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);
                        string strResult = strColumnName.Replace("_", "");

                        string strValue = "                    \r\n";
                        strValue += "                    //" + strColumnComment2 + "\r\n";
                        strValue += "                    admin.req({\r\n";
                        strValue += "                        url: '/" + strRouterName + "/select'\r\n";
                        strValue += "                        , done: function (res_" + strResult + ") {\r\n";
                        strValue += "                            console.log(res_" + strResult + ".data);\r\n";
                        strValue += "                            $.each(res_" + strResult + ".data, function (i, item) {\r\n";
                        strValue += "                                if (res_" + strResult + ".data[i]." + strSelectId + " == res.data." + strSelectId + ") {\r\n";
                        strValue += "                                    $(\"#" + strSelectId + "\").append(\"<option value=\\\"\" + res_" + strResult + ".data[i]." + strSelectId + " + \"\\\" selected>\" + res_" + strResult + ".data[i]." + strSelectName + " + \"</option>\");\r\n";
                        strValue += "                                }\r\n";
                        strValue += "                                else {\r\n";
                        strValue += "                                    $(\"#" + strSelectId + "\").append(\"<option value=\\\"\" + res_" + strResult + ".data[i]." + strSelectId + " + \"\\\">\" + res_" + strResult + ".data[i]." + strSelectName + " + \"</option>\");\r\n";
                        strValue += "                                }\r\n";
                        strValue += "                            });\r\n";
                        strValue += "                            layui.form.render('select');\r\n";
                        strValue += "                        }\r\n";
                        strValue += "                    });\r\n";
                        strValue += "                    \r\n";

                        strReturnValue += strValue;
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
