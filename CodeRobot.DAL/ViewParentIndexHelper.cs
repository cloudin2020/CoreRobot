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
    public class ViewParentIndexHelper
    {

        /// <summary>
        /// 根据表名创建Index管理页面
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateViewIndexClass(string strFilePath,string strProjectName, string strTableName,string strTableComment)
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
            string strNameValue = strPrimaryKey.Replace("_id", "");

            Directory.CreateDirectory(strFilePath);
            //为每个表创建一个独立目录存放
            string strTableNamePath = strFilePath + "\\"+ strClassName;
            if (!Directory.Exists(strTableNamePath))
            {
                Directory.CreateDirectory(strTableNamePath);
            }
            StreamWriter sw = new StreamWriter(strTableNamePath + "\\Index.cshtml", false, Encoding.GetEncoding("utf-8"));//生成物理文件
            string strTableComment2 = CommonHelper.GetColumnKeyComment(strTableComment);

            sw.WriteLine("@{");
            sw.WriteLine("    ViewData[\"Title\"] = \"" + strTableComment2 + "列表\";");
            sw.WriteLine("    Layout = \"~/Views/Shared/_LayoutNone.cshtml\";");
            sw.WriteLine("}");
            sw.WriteLine("<div class=\"layui-fluid\">");
            sw.WriteLine("    <div class=\"layui-card\">");
            sw.WriteLine("");
            sw.WriteLine("        <div class=\"layui-card-body\">");
            sw.WriteLine("            <div style=\"padding-bottom: 10px;\">");
            sw.WriteLine("                <button class=\"layui-btn layuiadmin-btn-list\" onclick=\"add();\">新增</button>");
            sw.WriteLine("                <button class=\"layui-btn layuiadmin-btn-list\" onclick=\"refresh();\">刷新</button>");
            sw.WriteLine("            </div>");
            sw.WriteLine("            <div id=\"LAY-app-content-list\"></div>");
            sw.WriteLine("        </div>");
            sw.WriteLine("    </div>");
            sw.WriteLine("</div>");
            sw.WriteLine("");
            sw.WriteLine("<script src=\"~/layuiadmin/layui/layui.js\"></script>");
            sw.WriteLine("<script src=\"~/lib/jquery/dist/jquery.min.js\"></script>");
            sw.WriteLine("<script type=\"text/javascript\">");
            sw.WriteLine("    //删除");
            sw.WriteLine("    function del(row) {");
            sw.WriteLine("        layer.confirm(\"确定删除此" + strTableComment2 + "？\", function (e) {");
            sw.WriteLine("");
            sw.WriteLine("            $.ajax({");
            sw.WriteLine("                headers: {");
            sw.WriteLine("                    'RequestVerificationToken': csrfToken");
            sw.WriteLine("                },");
            sw.WriteLine("                url: '/" + strTableNameLower + "/delete/' + row.id,");
            sw.WriteLine("                type: \"POST\",");
            sw.WriteLine("                cache: false,");
            sw.WriteLine("                dataType: \"json\",");
            sw.WriteLine("                success: function (res) {");
            sw.WriteLine("                    console.log(res.msg);");
            sw.WriteLine("                    layer.close(e);");
            sw.WriteLine("                    window.location.reload();");
            sw.WriteLine("                }");
            sw.WriteLine("            });");
            sw.WriteLine("        });");
            sw.WriteLine("    }");
            sw.WriteLine("");
            sw.WriteLine("    //刷新");
            sw.WriteLine("    function refresh() {");
            sw.WriteLine("        window.location.reload();");
            sw.WriteLine("    }");
            sw.WriteLine("");
            sw.WriteLine("    //新增");
            sw.WriteLine("    function add() {");
            sw.WriteLine("");
            sw.WriteLine("        layer.open({");
            sw.WriteLine("            type: 2");
            sw.WriteLine("            , title: '新增" + strTableComment2 + "'");
            sw.WriteLine("            , content: '/" + strTableNameLower + "/create'");
            sw.WriteLine("            , maxmin: true");
            sw.WriteLine("            , area: ['550px', '550px']");
            sw.WriteLine("            , btn: ['确定', '取消']");
            sw.WriteLine("            , yes: function (index, layero) {");
            sw.WriteLine("                //点击确认触发 iframe 内容中的按钮提交");
            sw.WriteLine("                var submit = layero.find('iframe').contents().find(\"#layuiadmin-app-form-submit\");");
            sw.WriteLine("                submit.click();");
            sw.WriteLine("            }");
            sw.WriteLine("        });");
            sw.WriteLine("    }");
            sw.WriteLine("");
            sw.WriteLine("    //编辑");
            sw.WriteLine("    function edit(row) {");
            sw.WriteLine("");
            sw.WriteLine("        layer.open({");
            sw.WriteLine("            type: 2");
            sw.WriteLine("            , title: '编辑" + strTableComment2 + "'");
            sw.WriteLine("            , content: '/" + strTableNameLower + "/edit?id=' + row.id");
            sw.WriteLine("            , maxmin: true");
            sw.WriteLine("            , area: ['550px', '550px']");
            sw.WriteLine("            , btn: ['确定', '取消']");
            sw.WriteLine("            , yes: function (index, layero) {");
            sw.WriteLine("                //点击确认触发 iframe 内容中的按钮提交");
            sw.WriteLine("                var submit = layero.find('iframe').contents().find(\"#layuiadmin-app-form-edit\");");
            sw.WriteLine("                submit.click();");
            sw.WriteLine("            }");
            sw.WriteLine("        });");
            sw.WriteLine("    }");
            sw.WriteLine("");
            sw.WriteLine("    var layout = [");
            sw.WriteLine("        { name: '名称', treeNodes: true, headerClass: 'value_col', colClass: 'value_col', style: '' },");
            sw.WriteLine("        { name: 'ID', field: 'id', headerClass: 'value_col', colClass: 'value_col', style: '' },");
            sw.WriteLine("        { name: '备注', field: 'remark', headerClass: 'value_col', colClass: 'value_col', style: '' },");
            sw.WriteLine("        {");
            sw.WriteLine("            name: '操作',");
            sw.WriteLine("            headerClass: 'value_col',");
            sw.WriteLine("            colClass: 'value_col',");
            sw.WriteLine("            style: 'width: 20%',");
            sw.WriteLine("            render: function (row) {");
            sw.WriteLine("                return \"<a class=\\\"layui-btn layui-btn-xs\\\" onclick='edit(\" + row + \")'><i class=\\\"layui-icon layui-icon-edit\\\"></i>编辑</a> <a class='layui-btn layui-btn-danger layui-btn-sm' onclick='del(\" + row + \")'><i class='layui-icon'>&#xe640;</i> 删除</a>\"; //列渲染");
            sw.WriteLine("            }");
            sw.WriteLine("        },");
            sw.WriteLine("    ];");
            sw.WriteLine("");
            sw.WriteLine("    layui.config({");
            sw.WriteLine("        base: '/layuiadmin/' //静态资源所在路径");
            sw.WriteLine("    }).extend({");
            sw.WriteLine("        treetable: 'treetable-lay/treetable',");
            sw.WriteLine("        index: '/lib/index' //主入口模块");
            sw.WriteLine("    }).use(['index', 'table', 'laydate', 'util', 'treetable', 'layer'], function () {");
            sw.WriteLine("        var layer = layui.layer, form = layui.form, admin = layui.admin, $ = layui.jquery;");
            sw.WriteLine("");
            sw.WriteLine("        $(function () {");
            sw.WriteLine("");
            sw.WriteLine("            //读取数据加载loading..");
            sw.WriteLine("            loading = layer.load(2, {");
            sw.WriteLine("                shade: [0.2, '#000']");
            sw.WriteLine("            });");
            sw.WriteLine("");
            sw.WriteLine("            //列表");
            sw.WriteLine("            admin.req({");
            sw.WriteLine("                url: '/" + strTableNameLower + "/list?id=-1'");
            sw.WriteLine("                , done: function (res) {");
            sw.WriteLine("                    layer.close(loading);");
            sw.WriteLine("                    console.log(res.data);");
            sw.WriteLine("                    var count = res.count;");
            sw.WriteLine("                    var arrayList = [];");
            sw.WriteLine("                    var num = 1;");
            sw.WriteLine("                    $.each(res.data, function (i, item) {");
            sw.WriteLine("                        var " + strPrimaryKey + " = res.data[i]." + strPrimaryKey+";");
            sw.WriteLine("                        //console.log('" + strPrimaryKey + "=' + " + strPrimaryKey + " + ',count=' + count);");
            sw.WriteLine("");
            sw.WriteLine("                        //获取子节点");
            sw.WriteLine("                        admin.req({");
            sw.WriteLine("                            url: '/" + strTableNameLower + "/list?id=' + " + strPrimaryKey + "");
            sw.WriteLine("                            , done: function (res2) {");
            sw.WriteLine("                                //console.log(res2.data);");
            sw.WriteLine("                                var count2 = res2.count;");
            sw.WriteLine("                                var arrarSubList = [];");
            sw.WriteLine("                                $.each(res2.data, function (a, item) {");
            sw.WriteLine("                                    arrarSubList.push({ \"id\": res2.data[a]." + strPrimaryKey + ", \"name\": res2.data[a]."+ strNameValue + "_name, \"remark\": res2.data[a]." + strNameValue + "_remark });");
            sw.WriteLine("                                });");
            sw.WriteLine("");
            sw.WriteLine("                                arrayList.push({ \"id\": res.data[i]." + strPrimaryKey + ", \"name\": res.data[i]." + strNameValue + "_name, \"remark\": res.data[i]." + strNameValue + "_remark, \"children\": arrarSubList });");
            sw.WriteLine("");
            sw.WriteLine("                                if (num == count) {");
            sw.WriteLine("                                    var tree1 = layui.treetable({");
            sw.WriteLine("                                        elem: '#LAY-app-content-list', //传入元素选择器");
            sw.WriteLine("                                        checkbox: false,");
            sw.WriteLine("                                        nodes: arrayList,");
            sw.WriteLine("                                        layout: layout");
            sw.WriteLine("                                    });");
            sw.WriteLine("");
            sw.WriteLine("                                    form.render();");
            sw.WriteLine("                                }");
            sw.WriteLine("                                num++;");
            sw.WriteLine("                            }");
            sw.WriteLine("                        });");
            sw.WriteLine("                    });");
            sw.WriteLine("                }");
            sw.WriteLine("            });");
            sw.WriteLine("");
            sw.WriteLine("        });");
            sw.WriteLine("");
            sw.WriteLine("    });");
            sw.WriteLine("");
            sw.WriteLine("</script>");

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
                        strValue += "                    <label class=\"layui-form-label\">"+ strColumnComment2 + "</label>\r\n";
                        strValue += "                    <div class=\"layui-input-inline\">\r\n";
                        strValue += "                        <input type=\"text\" id=\""+ strColumnName + "\" name=\"" + strColumnName + "\" placeholder=\"请输入" + strColumnComment2 + "\" autocomplete=\"off\" class=\"layui-input\">\r\n";
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
