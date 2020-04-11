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
    /// Java 编辑类
    /// </summary>
    public class JavaViewsEditHelper
    {

        /// <summary>
        /// 根据表名创建编辑类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateViewEditClass(string strFilePath,string strProjectName,string strHtmlInputList, string strTableName,string strTableComment)
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
            strClassName = CommonHelper.GetTableNameUpper(strClassName);//News
            string strTableNameSpec = CommonHelper.GetTableNameFirstLowerSecondUpper(strClassName);//如：newsTypes
            string strTableNameLower = strTableNameSpec.ToLower();//如：newstypes

            string strNewPath = strFilePath + "\\" + strTableNameLower + "\\views";
            if (!Directory.Exists(strNewPath))
            {
                Directory.CreateDirectory(strNewPath);
            }
            
            StreamWriter sw = new StreamWriter(strNewPath + "\\Edit.html", false, Encoding.GetEncoding("utf-8"));

            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<html>");
            sw.WriteLine("<head>");
            sw.WriteLine("<meta charset=\"utf-8\">");
            sw.WriteLine("<title>编辑" + strTableComment + "</title>");
            sw.WriteLine("<meta name=\"renderer\" content=\"webkit\">");
            sw.WriteLine("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">");
            sw.WriteLine("<meta name=\"viewport\"");
            sw.WriteLine("	content=\"width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=0\">");
            sw.WriteLine("<link rel=\"stylesheet\" th:href=\"@{/layuiadmin/layui/css/layui.css}\"");
            sw.WriteLine("	media=\"all\">");
            sw.WriteLine("<link rel=\"stylesheet\" th:href=\"@{/layuiadmin/style/admin.css}\"");
            sw.WriteLine("	media=\"all\">");
            sw.WriteLine("</head>");
            sw.WriteLine("<body>");
            sw.WriteLine("	<div class=\"layui-form\" lay-filter=\"layuiadmin-app-form-list\"");
            sw.WriteLine("		id=\"layuiadmin-app-form-list\" style=\"padding: 20px 30px 0 0;\">");
            sw.WriteLine("");
            sw.WriteLine(strHtmlInputList);
            sw.WriteLine("");
            sw.WriteLine("		<div class=\"layui-form-item layui-hide\">");
            sw.WriteLine("			<input type=\"button\" lay-submit");
            sw.WriteLine("				lay-filter=\"layuiadmin-app-form-submit\"");
            sw.WriteLine("				id=\"layuiadmin-app-form-submit\" value=\"确认添加\"> <input");
            sw.WriteLine("				type=\"button\" lay-submit lay-filter=\"layuiadmin-app-form-edit\"");
            sw.WriteLine("				id=\"layuiadmin-app-form-edit\" value=\"确认编辑\">");
            sw.WriteLine("		</div>");
            sw.WriteLine("	</div>");
            sw.WriteLine("	<script th:src=\"@{/layuiadmin/layui/layui.js}\"></script>");
            sw.WriteLine("	<script>");
            sw.WriteLine("layui.config({");
            sw.WriteLine("    base: '/layuiadmin/' //静态资源所在路径");
            sw.WriteLine("}).extend({");
            sw.WriteLine("    index: '/lib/index', //主入口模块");
            sw.WriteLine("}).use(['index', 'layedit', 'form'], function () {");
            sw.WriteLine("    var $ = layui.$");
            sw.WriteLine("        , form = layui.form");
            sw.WriteLine("        , admin = layui.admin");
            sw.WriteLine("        , layedit = layui.layedit;");
            sw.WriteLine("");
            sw.WriteLine("    //监听指定开关");
            sw.WriteLine("    form.on('switch(switch_status)', function (data) {");
            sw.WriteLine("        if (this.checked) {");
            sw.WriteLine("            $(\"#news_status\").val(true);");
            sw.WriteLine("        } else {");
            sw.WriteLine("            $(\"#news_status\").val(false);");
            sw.WriteLine("        }");
            sw.WriteLine("    });");
            sw.WriteLine("");
            sw.WriteLine("    //监听提交");
            sw.WriteLine("    form.on('submit(layuiadmin-app-form-submit)', function (data) {");
            sw.WriteLine("        var field = data.field; //获取提交的字段");
            sw.WriteLine("        var index = parent.layer.getFrameIndex(window.name); //先得到当前iframe层的索引");
            sw.WriteLine("");
            sw.WriteLine("        //提交 Ajax 成功后，关闭当前弹层并重载表格");
            sw.WriteLine("        loading = layer.load(2, {");
            sw.WriteLine("            shade: [0.2, '#000']");
            sw.WriteLine("        });");
            sw.WriteLine("");
            sw.WriteLine("        //提交数据");
            sw.WriteLine("        admin.req({");
            sw.WriteLine("            method: 'POST',");
            sw.WriteLine("            url: '/"+strTableNameLower+"/edit' ");
            sw.WriteLine("            , data: data.field");
            sw.WriteLine("            , done: function (res) {");
            sw.WriteLine("                layer.close(loading);");
            sw.WriteLine("                layer.msg(res.msg, {");
            sw.WriteLine("                    offset: '15px'");
            sw.WriteLine("                    , icon: 1");
            sw.WriteLine("                    , time: 1000");
            sw.WriteLine("                }, function () {");
            sw.WriteLine("                    window.location.reload();");
            sw.WriteLine("                });");
            sw.WriteLine("            }");
            sw.WriteLine("        });");
            sw.WriteLine("    });");
            sw.WriteLine("})");
            sw.WriteLine(" </script>");
            sw.WriteLine("</body>");
            sw.WriteLine("</html>");



            sw.Close();

        }

        
    }
}
