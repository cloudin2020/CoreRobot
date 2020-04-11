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
    /// Java 显示详情类
    /// </summary>
    public class JavaViewsDetailsHelper
    {

        /// <summary>
        /// 根据表名创建详情类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateViewDetailsClass(string strFilePath,string strProjectName,string strHtmlInputList, string strTableName,string strTableComment)
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
            
            StreamWriter sw = new StreamWriter(strNewPath + "\\Details.html", false, Encoding.GetEncoding("utf-8"));

            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<html>");
            sw.WriteLine("<head>");
            sw.WriteLine("<meta charset=\"utf-8\">");
            sw.WriteLine("<title>"+strTableComment+"详情</title>");
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
            sw.WriteLine("");
            sw.WriteLine("	<div class=\"layui-form\" lay-filter=\"layuiadmin-app-form-list\" id=\"layuiadmin-app-form-list\" style=\"padding: 20px 30px 0 0;\">");
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
            sw.WriteLine("   ");
            sw.WriteLine("})");
            sw.WriteLine(" </script>");
            sw.WriteLine("</body>");
            sw.WriteLine("</html>");

            sw.Close();

        }

        
    }
}
