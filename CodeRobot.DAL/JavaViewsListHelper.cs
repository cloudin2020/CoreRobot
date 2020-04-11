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
    public class JavaViewsListHelper
    {

        /// <summary>
        /// 根据表名创建列表显示类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateViewListClass(string strFilePath,string strProjectName,string strHtmlInputList, string strTableName,string strTableComment)
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

            string strNewPath = strFilePath + "\\" + strTableNameLower + "\\views";
            if (!Directory.Exists(strNewPath))
            {
                Directory.CreateDirectory(strNewPath);
            }
            
            StreamWriter sw = new StreamWriter(strNewPath + "\\List.html", false, Encoding.GetEncoding("utf-8"));

            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<html xmlns:th=\"http://www.thymeleaf.org\" xmlns:shiro=\"http://www.pollix.at/thymeleaf/shiro\">");
            sw.WriteLine("<head>");
            sw.WriteLine("<meta charset=\"utf-8\">");
            sw.WriteLine("<title>" + strTableComment2 + "列表</title>");
            sw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            sw.WriteLine("<meta name=\"renderer\" content=\"webkit\">");
            sw.WriteLine("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">");
            sw.WriteLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=0\">");
            sw.WriteLine("<link rel=\"stylesheet\" th:href=\"@{/layuiadmin/layui/css/layui.css}\" media=\"all\">");
            sw.WriteLine("<link rel=\"stylesheet\" th:href=\"@{/layuiadmin/style/admin.css}\" media=\"all\">");
            sw.WriteLine("</head>");
            sw.WriteLine("<body>");
            sw.WriteLine("	<div class=\"layui-fluid\">");
            sw.WriteLine("		<div class=\"layui-card\">");
            sw.WriteLine("			<div class=\"layui-form layui-card-header layuiadmin-card-header-auto\">");
            sw.WriteLine("				<div class=\"layui-form-item\">");
            sw.WriteLine(GetSearchItemList(strTableName));
            sw.WriteLine("					<div class=\"layui-inline\">");
            sw.WriteLine("						<button class=\"layui-btn layuiadmin-btn-list\" lay-submit");
            sw.WriteLine("							lay-filter=\"LAY-app-contlist-search\">");
            sw.WriteLine("							<i class=\"layui-icon layui-icon-search layuiadmin-button-btn\"></i>");
            sw.WriteLine("						</button>");
            sw.WriteLine("					</div>");
            sw.WriteLine("				</div>");
            sw.WriteLine("			</div>");
            sw.WriteLine("");
            sw.WriteLine("			<div class=\"layui-card-body\">");
            sw.WriteLine("				<div style=\"padding-bottom: 10px;\">");
            sw.WriteLine("					<button class=\"layui-btn layuiadmin-btn-list\" data-type=\"batchdel\">删除</button>");
            sw.WriteLine("					<button class=\"layui-btn layuiadmin-btn-list\" data-type=\"add\">添加</button>");
            sw.WriteLine("				</div>");
            sw.WriteLine("				<table id=\"LAY-app-content-list\" lay-filter=\"LAY-app-content-list\"></table>");
            sw.WriteLine("				<script type=\"text/html\" id=\"statusTpl\">");
            sw.WriteLine("          {{#  if(d.status){ }}");
            sw.WriteLine("            <button class=\"layui-btn layui-btn-xs\">已发布</button>");
            sw.WriteLine("          {{#  } else { }}");
            sw.WriteLine("            <button class=\"layui-btn layui-btn-primary layui-btn-xs\">待修改</button>");
            sw.WriteLine("          {{#  } }}");
            sw.WriteLine("        </script>");
            sw.WriteLine("				<script type=\"text/html\" id=\"table-content-list\">");
            sw.WriteLine("          <a class=\"layui-btn layui-btn-primary layui-btn-xs\" lay-event=\"views\">查看</a>");
            sw.WriteLine("          <a class=\"layui-btn layui-btn-normal layui-btn-xs\" lay-event=\"edit\"><i class=\"layui-icon layui-icon-edit\"></i>编辑</a>");
            sw.WriteLine("          <a class=\"layui-btn layui-btn-danger layui-btn-xs\" lay-event=\"del\"><i class=\"layui-icon layui-icon-delete\"></i>删除</a>");
            sw.WriteLine("        </script>");
            sw.WriteLine("			</div>");
            sw.WriteLine("		</div>");
            sw.WriteLine("	</div>");
            sw.WriteLine("");
            sw.WriteLine("	<script th:src=\"@{/layuiadmin/layui/layui.js}\"></script>");
            sw.WriteLine("	<script>");
            sw.WriteLine("  layui.config({");
            sw.WriteLine("      base: '/layuiadmin/' //静态资源所在路径");
            sw.WriteLine("  }).extend({");
            sw.WriteLine("      index: '/lib/index' //主入口模块");
            sw.WriteLine("  }).use(['index', 'table'], function () {");
            sw.WriteLine("      var table = layui.table");
            sw.WriteLine("          , admin = layui.admin");
            sw.WriteLine("          , form = layui.form;");
            sw.WriteLine("");
            sw.WriteLine("      //Ajax加载表格数据");
            sw.WriteLine("      table.render({");
            sw.WriteLine("          elem: \"#LAY-app-content-list\",");
            sw.WriteLine("          url: \"/"+ strTableNameLower + "/list\",");
            sw.WriteLine("          toolbar: true,");
            sw.WriteLine("          cols: [");
            sw.WriteLine("              [{");
            sw.WriteLine("                  type: \"checkbox\",");
            sw.WriteLine("                  fixed: \"left\"");
            sw.WriteLine("              },");
            sw.WriteLine(GetTableColsList(strTableName));
            sw.WriteLine("              {");
            sw.WriteLine("                  title: \"操作\",");
            sw.WriteLine("                  minWidth: 200,");
            sw.WriteLine("                  align: \"center\",");
            sw.WriteLine("                  fixed: \"right\",");
            sw.WriteLine("                  toolbar: \"#table-content-list\"");
            sw.WriteLine("              }]");
            sw.WriteLine("          ],");
            sw.WriteLine("          page: !0,");
            sw.WriteLine("          limit: 10,");
            sw.WriteLine("          limits: [10, 15, 20, 25, 30],");
            sw.WriteLine("          text: \"对不起，加载出现异常！\"");
            sw.WriteLine("      }),");
            sw.WriteLine("");
            sw.WriteLine("          //右侧操作事件");
            sw.WriteLine("          table.on(\"tool(LAY-app-content-list)\", function (t) {");
            sw.WriteLine("              var " + strPrimaryKey + " = t.data." + strPrimaryKey + ";");
            sw.WriteLine("              console.log('id=' + " + strPrimaryKey + ");");
            sw.WriteLine("              if (\"del\" === t.event) {");
            sw.WriteLine("                  layer.confirm(\"确定删除此" + strTableComment2 + "？\", function (e) {");
            sw.WriteLine("                      admin.req({              ");
            sw.WriteLine("                          method: 'POST',");
            sw.WriteLine("                          url: '/" + strTableNameLower + "/delete/' + " + strPrimaryKey + "");
            sw.WriteLine("                          , done: function (res) {");
            sw.WriteLine("                              //console.log(res.msg);");
            sw.WriteLine("                              t.del();");
            sw.WriteLine("                              layer.close(e);");
            sw.WriteLine("                          }");
            sw.WriteLine("                      });");
            sw.WriteLine("");
            sw.WriteLine("                  });");
            sw.WriteLine("              }");
            sw.WriteLine("              else if (\"edit\" === t.event) {");
            sw.WriteLine("            	  layer.open({");
            sw.WriteLine("                      type: 2,");
            sw.WriteLine("                      title: '编辑" + strTableComment2 + "',");
            sw.WriteLine("                      content: '/febs/views/" + strTableNameLower + "/edit/'+ " + strPrimaryKey + ",");
            sw.WriteLine("                      maxmin: true,");
            sw.WriteLine("                      area: ['550px', '500px'],");
            sw.WriteLine("                      btn: ['确定', '取消'],");
            sw.WriteLine("                      yes: function(index, layero){");
            sw.WriteLine("                        //点击确认触发 iframe 内容中的按钮提交");
            sw.WriteLine("                        var submit = layero.find('iframe').contents().find(\"#layuiadmin-app-form-submit\");");
            sw.WriteLine("                        submit.click();");
            sw.WriteLine("                      }");
            sw.WriteLine("                    }); ");
            sw.WriteLine("              }");
            sw.WriteLine("              else if (\"views\" === t.event) {");
            sw.WriteLine("            	  layer.open({");
            sw.WriteLine("                      type: 2,");
            sw.WriteLine("                      title: '查看" + strTableComment2 + "',");
            sw.WriteLine("                      content: '/febs/views/" + strTableNameLower + "/details/'+ " + strPrimaryKey + ",");
            sw.WriteLine("                      maxmin: true,");
            sw.WriteLine("                      area: ['550px', '500px'],");
            sw.WriteLine("                      btn: ['确定', '取消'],");
            sw.WriteLine("                      yes: function(index, layero){");
            sw.WriteLine("                        //点击确认触发 iframe 内容中的按钮提交");
            sw.WriteLine("                        var submit = layero.find('iframe').contents().find(\"#layuiadmin-app-form-submit\");");
            sw.WriteLine("                        submit.click();");
            sw.WriteLine("                      }");
            sw.WriteLine("                    }); ");
            sw.WriteLine("              }");
            sw.WriteLine("          }),");
            sw.WriteLine("");
            sw.WriteLine("          //监听搜索");
            sw.WriteLine("          form.on('submit(LAY-app-contlist-search)', function (data) {");
            sw.WriteLine("              var field = data.field;");
            sw.WriteLine("");
            sw.WriteLine("              //执行重载");
            sw.WriteLine("              table.reload('LAY-app-content-list', {");
            sw.WriteLine("                  where: field");
            sw.WriteLine("              });");
            sw.WriteLine("          });");
            sw.WriteLine("");
            sw.WriteLine("      var $ = layui.$, active = {");
            sw.WriteLine("          //批量删除");
            sw.WriteLine("          batchdel: function () {");
            sw.WriteLine("              var checkStatus = table.checkStatus('LAY-app-content-list')");
            sw.WriteLine("                  , checkData = checkStatus.data; //得到选中的数据");
            sw.WriteLine("");
            sw.WriteLine("              if (checkData.length === 0) {");
            sw.WriteLine("                  return layer.msg('请选择数据');");
            sw.WriteLine("              }");
            sw.WriteLine("");
            sw.WriteLine("              //批量删除");
            sw.WriteLine("              layer.confirm('确定删除吗？', function (index) {");
            sw.WriteLine("");
            sw.WriteLine("                  checkData.forEach(function (n, i) {");
            sw.WriteLine("                      console.log(\"" + strPrimaryKey + "=\" + n." + strPrimaryKey+");");
            sw.WriteLine("                      admin.req({");
            sw.WriteLine("                          method: 'POST',");
            sw.WriteLine("                          url: '/" + strTableNameLower + "/delete/' + n." + strPrimaryKey + "");
            sw.WriteLine("                          , done: function (res) {");
            sw.WriteLine("                              console.log(res.msg);");
            sw.WriteLine("                          }");
            sw.WriteLine("                      });");
            sw.WriteLine("                  });");
            sw.WriteLine("");
            sw.WriteLine("                  table.reload('LAY-app-content-list');");
            sw.WriteLine("                  layer.msg('已删除');");
            sw.WriteLine("              });");
            sw.WriteLine("");
            sw.WriteLine("          },");
            sw.WriteLine("          //新增");
            sw.WriteLine("          add: function(){");
            sw.WriteLine("              layer.open({");
            sw.WriteLine("                type: 2,");
            sw.WriteLine("                title: '新增" + strTableComment2 + "',");
            sw.WriteLine("                content: '/febs/views/" + strTableNameLower + "/create',");
            sw.WriteLine("                maxmin: true,");
            sw.WriteLine("                area: ['550px', '500px'],");
            sw.WriteLine("                btn: ['确定', '取消'],");
            sw.WriteLine("                yes: function(index, layero){");
            sw.WriteLine("                  //点击确认触发 iframe 内容中的按钮提交");
            sw.WriteLine("                  var submit = layero.find('iframe').contents().find(\"#layuiadmin-app-form-submit\");");
            sw.WriteLine("                  submit.click();");
            sw.WriteLine("                }");
            sw.WriteLine("              }); ");
            sw.WriteLine("           }, ");
            sw.WriteLine("      };");
            sw.WriteLine("");
            sw.WriteLine("      $('.layui-btn.layuiadmin-btn-list').on('click', function () {");
            sw.WriteLine("          var type = $(this).data('type');");
            sw.WriteLine("          active[type] ? active[type].call(this) : '';");
            sw.WriteLine("      });");
            sw.WriteLine("");
            sw.WriteLine("  });");
            sw.WriteLine("  </script>");
            sw.WriteLine("");
            sw.WriteLine("</body>");
            sw.WriteLine("</html>");


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
