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
    /// Java 列表显示类
    /// </summary>
    public class JavaViewsMapperHelper
    {

        /// <summary>
        /// 根据表名创建列表显示类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateViewMapperClass(string strFilePath,string strProjectName, string strTableName,string strTableComment)
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

            string strTableComment2 = CommonHelper.GetColumnKeyComment(strTableComment);

            string strNewPath = strFilePath + "\\" + strTableNameLower + "\\views-mapper";
            if (!Directory.Exists(strNewPath))
            {
                Directory.CreateDirectory(strNewPath);
            }

            StreamWriter sw = new StreamWriter(strNewPath + "\\" + strClassName + "Mapper.xml", false, Encoding.GetEncoding("utf-8"));

            sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sw.WriteLine("<!DOCTYPE mapper PUBLIC \"-//mybatis.org//DTD Mapper 3.0//EN\" \"http://mybatis.org/dtd/mybatis-3-mapper.dtd\">");
            sw.WriteLine("<mapper namespace=\"cc.mrbird.febs."+ strTableNameLower + ".mapper." + strClassName + "Mapper\">");
            sw.WriteLine("	<select id=\"findDetailPage\" parameterType=\"" + strTableNameLower + "\"");
            sw.WriteLine("		resultType=\"" + strTableNameLower + "\">");
            sw.WriteLine("		SELECT");
            string result = CommonHelper.GetLeftSelectColumnName2(strTableName, "item");
            result = result.Replace("                                      ", "		");
            sw.WriteLine(result);
            string result2 = CommonHelper.GetDetailsSelectJoinColumn(strTableName, strPrimaryKey, CommonHelper.GetLeftSelectColumnName2(strTableName, "a"));
            result2 = result2.Replace("                                      ", "		");
            sw.WriteLine(result2);
            sw.WriteLine("		FROM "+ strTableName + " item");
            sw.WriteLine(CommonHelper.GetDetailsSelectJoinListForJava(strTableName));
            sw.WriteLine("		where 1=1 	 ");
            sw.WriteLine("");
            sw.WriteLine(CommonHelper.GetSearchListForJava(strTableName));
            sw.WriteLine("");
            sw.WriteLine("	</select>");
            sw.WriteLine("</mapper>");


            sw.Close();

        }



        /// <summary>
        /// 获取搜索字段列表
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetSearchItemList(string strTableName)
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


                    if (strColumnComment.Contains("{搜索}"))
                    {
                        string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);

                        string strValue = "                <div class=\"layui-inline\">\r\n";
                        strValue += "                    <label class=\"layui-form-label\">" + strColumnComment2 + "</label>\r\n";
                        strValue += "                    <div class=\"layui-input-inline\">\r\n";
                        strValue += "                        <input type=\"text\" id=\"" + strColumnName + "\" name=\"" + strColumnName + "\" placeholder=\"请输入" + strColumnComment2 + "\" autocomplete=\"off\" class=\"layui-input\">\r\n";
                        strValue += "                    </div>\r\n";
                        strValue += "                </div>\r\n";

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
        /// 获取表格显示字段列表
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetTableColsList(string strTableName)
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

                    string strColumnComment2 = CommonHelper.GetColumnKeyComment(strColumnComment);

                    string strValue = "                {\r\n";
                    strValue += "                    field: \"" + strColumnName + "\",\r\n";
                    strValue += "                    title: \"" + strColumnComment2 + "\",\r\n";
                    if (strColumnName == "created_at" || strColumnKey == "PRI")
                    {
                        //主键和日期需要排序
                        strValue += "                    sort: !0,\r\n";
                    }
                    if (strColumnKey == "PRI")
                    {
                        //主键设置宽度
                        strValue += "                    width: 80,\r\n";
                    }
                    if (strColumnName == "created_at")
                    {
                        //日期设置宽度及格式
                        strValue += "                    width: 180,\r\n";
                        strValue += "                    templet: \"<div>{{layui.util.toDateString(d.created_at, 'yyyy-MM-dd HH:mm')}}</div>\",\r\n";
                    }
                    strValue += "                },\r\n";

                    strReturnValue += strValue;

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


    }
}
