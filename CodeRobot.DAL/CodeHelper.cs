using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace CodeRobot.DAL
{
    public class CodeHelper
    {
        CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");

        /// <summary>
        /// 获取数据库中的表
        /// </summary>
        /// <param name="strDBName">数据库名</param>
        public void GetTables(string strDBName, System.Windows.Forms.RichTextBox txtMessage)
        {
            try
            {
                //项目名
                string strProjectName = iniFile.GetString("BASE", "PROJECT", "");
                string strSpecialTableName = iniFile.GetString("BASE", "TABLENAME", "");//指定表名生成

                //获取路径
                string strProjectPath = iniFile.GetString("DIR", "PROJECT", "");
                string strBLLPath = iniFile.GetString("DIR", "BLL", "");
                string strDALPath = iniFile.GetString("DIR", "DAL", "");
                string strDBSqlHelperPath = iniFile.GetString("DIR", "DBSQLHELPER", "");
                string strModelPath = iniFile.GetString("DIR", "MODEL", "");
                string strUtilityPath = iniFile.GetString("DIR", "UTILITY", "");
                string strManagePath = iniFile.GetString("DIR", "MANAGE", "");
                string strAPIPath = iniFile.GetString("DIR", "API", "");
                string strHTML5Path = iniFile.GetString("DIR", "HTML5", "");
                string strWebPath = iniFile.GetString("DIR", "WEB", "");
                string strWeChatAppPath = iniFile.GetString("DIR", "WeChatApp", "");
                string strUIPath = iniFile.GetString("DIR", "UI", "");
                string strPMPath = iniFile.GetString("DIR", "PM", "");
                string strHTMLPath = iniFile.GetString("DIR", "HTML", "");
                string strDataPath = iniFile.GetString("DIR", "DATA", "");
                string strTestPath = iniFile.GetString("DIR", "TEST", "");
                string strSMSPath = iniFile.GetString("DIR", "SMS", "");
                string strPushPath = iniFile.GetString("DIR", "PUSH", "");

                string strAndroidPackage = iniFile.GetString("ANDROID", "PACKAGE", "");//获取Android包名
                string strAndroidModelPath = iniFile.GetString("ANDROID", "MODEL", "");//获取Android生成目录
                string strAndroidJavaPath = iniFile.GetString("ANDROID", "JAVA", "");//获取Android生成目录
                string strAndroidLayoutPath = iniFile.GetString("ANDROID", "LAYOUT", "");//获取Android生成目录

                string striOSPackage = iniFile.GetString("IOS", "PACKAGE", "");//获取iOS包名
                string striOSControllersPath = iniFile.GetString("IOS", "CONTROLLERS", "");//获取iOS生成目录
                string striOSViewsPath = iniFile.GetString("IOS", "VIEWS", "");//获取iOS生成目录
                string striOSModelsPath = iniFile.GetString("IOS", "MODELS", "");//获取iOS生成目录

                string strCoreMainPath = iniFile.GetString("CORE", "MAIN", "");//获取Core生成目录
                string strCoreDataPath = iniFile.GetString("CORE", "DATA", "");
                string strCoreViewsPath = iniFile.GetString("CORE", "VIEWS", "");
                string strControllersPath = iniFile.GetString("CORE", "CONTROLLERS", "");
                string strWebApiPath = iniFile.GetString("CORE", "WEBAPI", "");
                string strApiModelsPath = iniFile.GetString("CORE", "APIMODELS", "");
                string strModelsPath = iniFile.GetString("CORE", "MODELS", "");

                string strUniAppPath = iniFile.GetString("APP", "UNIAPP", "");

                string strJavaPath = iniFile.GetString("JAVA", "JAVA", "");
                string strJavaControllerPath = iniFile.GetString("JAVA", "CONTROLLER", "");
                string strJavaEntityPath = iniFile.GetString("JAVA", "ENTITY", "");
                string strJavaMapperPath = iniFile.GetString("JAVA", "MAPPER", "");
                string strJavaServicePath = iniFile.GetString("JAVA", "SERVICE", "");
                string strJavaSrcMapperPath = iniFile.GetString("JAVA", "SRCMAPPER", "");
                string strJavaSrcViewsPath = iniFile.GetString("JAVA", "SRCVIEWS", "");

                //生成数据库连接文件
                DBSqlHelper.CreateDBSqlHelperFile(strDBSqlHelperPath, strProjectName);

                //创建README文件
                ReadmeHelper.CreateProjectFile(strProjectPath, strProjectName);
                ReadmeHelper.CreateBLLFile(strBLLPath, strProjectName);
                ReadmeHelper.CreateDALFile(strDALPath, strProjectName);
                ReadmeHelper.CreateDBSqlHelperFile(strDBSqlHelperPath, strProjectName);
                ReadmeHelper.CreateModelFile(strModelPath, strProjectName);
                ReadmeHelper.CreateUtilityFile(strUtilityPath, strProjectName);
                ReadmeHelper.CreateManageFile(strManagePath, strProjectName);
                ReadmeHelper.CreateWebFile(strWebPath, strProjectName);
                ReadmeHelper.CreateWeChatFile(strHTML5Path, strProjectName);
                ReadmeHelper.CreateWeChatAppFile(strWeChatAppPath, strProjectName);
                ReadmeHelper.CreateRESTAPIFile(strAPIPath, strProjectName);
                ReadmeHelper.CreateUIFile(strUIPath, strProjectName);
                ReadmeHelper.CreatePMFile(strPMPath, strProjectName);
                ReadmeHelper.CreateHTMLFile(strHTMLPath, strProjectName);
                ReadmeHelper.CreateTESTFile(strTestPath, strProjectName);
                ReadmeHelper.CreateDATAFile(strDataPath, strProjectName);
                ReadmeHelper.CreateSMSFile(strSMSPath, strProjectName);
                ReadmeHelper.CreatePUSHFile(strPushPath, strProjectName);
                ReadmeHelper.CreateCoreFile(strCoreMainPath, strProjectName);
                ReadmeHelper.CreateJavaFile(strJavaControllerPath, strProjectName);

                int num = 1;
                string strCompileTableModel = "";
                string strCompileTableDAL = "";
                string strCompileTableBLL = "";
                string strCoreContextList = "";

                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "";
                if (string.IsNullOrEmpty(strSpecialTableName) || strSpecialTableName == "指定表名")
                {
                    strSql = "SELECT TABLE_NAME,COLUMN_NAME,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_SCHEMA='" + strDBName + "' GROUP BY TABLE_NAME";
                }
                else
                {
                    strSql = "SELECT TABLE_NAME,COLUMN_NAME,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_SCHEMA='" + strDBName + "' AND TABLE_NAME='" + strSpecialTableName + "' GROUP BY TABLE_NAME";
                }
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strTableName = dr["TABLE_NAME"].ToString();
                    string strColumnName = dr["COLUMN_NAME"].ToString();//主关键字
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//主关键字说明
                    //strColumnComment = strColumnComment.Replace("ID", "");
                    strColumnComment = CommonHelper.GetColumnKeyComment(strColumnComment);


                    string strClassName = CommonHelper.GetClassName(strTableName);

                    CodeRobot.Utility.PublicValue.SaveMessage(Convert.ToDateTime(DateTime.Now).ToString("MM-dd HH:mm:ss") + " " + strTableName + " -> 已生成");
                    txtMessage.Text = CodeRobot.Utility.PublicValue.GetMessage();

                    //读取表中的字段
                    GetColumnList(strTableName, strColumnComment);

                    //生成数据处理类
                    //DALHelper.CreateDALClass(strDALPath, strProjectName, strTableName, strColumnComment);

                    //生成业务逻辑类
                    //BLLHelper.CreateBLLClass(strBLLPath, strProjectName, strTableName, strColumnComment);

                    //生成后台代码文件
                    //ManageHelper.CreateManageFile(strManagePath, strProjectName, strTableName, strColumnComment);

                    //生成RESTAPI
                    //RestAPIHelper.CreateRestAPIClass(strAPIPath, strProjectName, strTableName, strColumnComment);

                    //生成HTML5
                    //HTML5Helper.CreateHTML5File(strHTML5Path, strProjectName, strTableName, strColumnComment);

                    //生成Android Object
                    //AndroidHelper.CreateAndroidModelClass(strAndroidModelPath, strProjectName, strTableName, strAndroidPackage);
                    //AndroidHelper.CreateJavaClass(strAndroidJavaPath, strProjectName, strTableName, strAndroidPackage);
                    //AndroidHelper.CreateXMLFile(strAndroidLayoutPath, strProjectName, strTableName, strAndroidPackage);

                    //生成iOS Object
                    //iOSHelper.CreateModelsClass(striOSModelsPath, strProjectName, strTableName, striOSPackage);
                    //iOSHelper.CreateViewsClass(striOSViewsPath, strProjectName, strTableName, striOSPackage);
                    //iOSHelper.CreateControllersClass(striOSControllersPath, strProjectName, strTableName, striOSPackage);

                    //生成.net core
                    string strCoreValue = "        \r\n";
                    strCoreValue += "        /// <summary>\r\n";
                    strCoreValue += "        /// "+ strColumnComment+ "\r\n";
                    strCoreValue += "        /// </summary>\r\n";
                    strCoreValue += "        public DbSet<"+strProjectName+".Models."+ CommonHelper.GetTableNameUpper(strClassName) + "> "+ strTableName + " { get; set; }\r\n";
                    strCoreContextList += strCoreValue;

                    ControllersHelper.CreateControllersClass(strControllersPath, strProjectName, strTableName, strColumnComment);
                    if (strColumnComment.Contains("{MutiPages}"))
                    {
                        //生成新增或编辑独立页面模式
                        ViewMutiIndexHelper.CreateViewMutiIndexClass(strCoreViewsPath, strProjectName, strTableName, strColumnComment);
                        ViewMutiCreateHelper.CreateViewMutiCreateClass(strCoreViewsPath, strProjectName, strTableName, strColumnComment);
                        ViewMutiEditHelper.CreateViewMutiEditClass(strCoreViewsPath, strProjectName, strTableName, strColumnComment);
                        ViewMutiViewsHelper.CreateViewMutiViewsClass(strCoreViewsPath, strProjectName, strTableName, strColumnComment);
                    }
                    else if (strColumnComment.Contains("{ParentPages}"))
                    {
                        //生成树形管理页面
                        ViewParentIndexHelper.CreateViewIndexClass(strCoreViewsPath, strProjectName, strTableName, strColumnComment);
                        ViewCreateHelper.CreateViewCreateClass(strCoreViewsPath, strProjectName, strTableName, strColumnComment);
                        ViewEditHelper.CreateViewEditClass(strCoreViewsPath, strProjectName, strTableName, strColumnComment);
                    }
                    else
                    {
                        //生成新增或编辑弹窗模式
                        ViewIndexHelper.CreateViewIndexClass(strCoreViewsPath, strProjectName, strTableName, strColumnComment);
                        ViewCreateHelper.CreateViewCreateClass(strCoreViewsPath, strProjectName, strTableName, strColumnComment);
                        ViewEditHelper.CreateViewEditClass(strCoreViewsPath, strProjectName, strTableName, strColumnComment);
                    }

                    //生成WEB API Controller
                    ControllersWebAPIHelper.CreateControllersClass(strWebApiPath, strProjectName, strTableName, strColumnComment);

                    //每个项目的系统文件

                    strClassName = CommonHelper.GetTableNameUpper(strClassName);
                    strCompileTableModel += "    <Compile Include=\"" + strClassName + "Model.cs\" />\r\n";
                    strCompileTableDAL += "    <Compile Include=\"" + strClassName + "DAL.cs\" />\r\n";
                    strCompileTableBLL += "    <Compile Include=\"" + strClassName + "BLL.cs\" />\r\n";

                    //生成表SQL
                    try
                    {
                        string strSQLContent = "-- ----------------------------\r\n";
                        strSQLContent += "-- Table structure for `" + strTableName + "`\r\n";
                        strSQLContent += "-- ----------------------------\r\n";
                        strSQLContent += "DROP TABLE IF EXISTS `" + strTableName + "`;\r\n";
                        strSQLContent += "CREATE TABLE `" + strTableName + "` (\r\n";
                        strSQLContent += GetSQL(strTableName);
                        strSQLContent += "\r\n) ENGINE=InnoDB DEFAULT CHARSET=utf8;\r\n";

                        CodeRobot.Utility.TxtFile.CreateSQLFile(strDBName, strDataPath, strSQLContent);

                    }
                    catch (Exception ex)
                    {
                        CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建SQL相关文件", "GetTables", false);
                    }


                    //生成JAVA代码
                    JavaControllersHelper.CreateControllersClass(strJavaPath, strProjectName, strTableName, strColumnComment);
                    JavaControllersViewHelper.CreateControllersViewClass(strJavaPath, strProjectName, strTableName, strColumnComment);
                    JavaMapperHelper.CreateMapperClass(strJavaPath, strProjectName, strTableName, strColumnComment);
                    JavaServiceHelper.CreateServiceClass(strJavaPath, strProjectName, strTableName, strColumnComment);
                    JavaServiceImplHelper.CreateServiceImplClass(strJavaPath, strProjectName, strTableName, strColumnComment);
                    JavaViewsMapperHelper.CreateViewMapperClass(strJavaPath, strProjectName, strTableName, strColumnComment);

                    num++;
                }
                dr.Dispose();
                cn.Close();

                CodeRobot.Utility.PublicValue.SaveMessage(Convert.ToDateTime(DateTime.Now).ToString("MM-dd HH:mm:ss") + " 共处理 " + num + " 个数据表");
                txtMessage.Text = CodeRobot.Utility.PublicValue.GetMessage();

                //生成HTML项目下文件
                //HTMLHelper.CreateHTMLFile(strHTMLPath, strProjectName);

                //生成UI项目下文件
                UIHelper.CreateUIFile(strUIPath, strProjectName);

                //创建Utility类库下文件
                //UtilityHelper.CreateUtilityFile(strUtilityPath, strProjectName);

                //创建SMS类库下文件
                //SMSHelper.CreateSMSClass(strSMSPath, strProjectName);

                //创建相关项目的系统文件及项目文件
                SystemHelper.CreateSolutionSystemFile(strProjectPath, strProjectName);
                //SystemHelper.CreateDBSqlHelperSystemFile(strDBSqlHelperPath, strProjectName);
                //SystemHelper.CreateModelSystemFile(strModelPath, strProjectName, strCompileTableModel);
                //SystemHelper.CreateDALSystemFile(strDALPath, strProjectName, strCompileTableDAL);
                //SystemHelper.CreateBLLSystemFile(strBLLPath, strProjectName, strCompileTableBLL);
                //SystemHelper.CreateUtilitySystemFile(strUtilityPath, strProjectName);
                //SystemHelper.CreateSMSSystemFile(strSMSPath, strProjectName);
                //SystemHelper.CreatePushSystemFile(strPushPath, strProjectName);

                //创建Web项目的相关配置文件
                WebHelper.CreateWebFile(strProjectPath, strProjectName);

                ContextHelper.CreateContextClass(strCoreDataPath, strProjectName, strCoreContextList);
                ContextAPIHelper.CreateContextClass(strCoreDataPath, strProjectName, strCoreContextList);

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取数据库中的表", "GetTables", false);
            }

        }

        /// <summary>
        /// 获取表中的字段
        /// </summary>
        /// <param name="strTableName">表名</param>
        public void GetColumnList(string strTableName, string strTableColumnComment)
        {
            try
            {
                string strModelPath = iniFile.GetString("DIR", "MODEL", "");//实体类路径
                string strModelsPath = iniFile.GetString("CORE", "MODELS", "");//实体类路径
                string strProjectName = iniFile.GetString("BASE", "PROJECT", "");//项目名

                string strJavaModelPath = iniFile.GetString("JAVA", "JAVA", ""); ;//实体类路径

                string strEntityVariableList = "";//实体变量
                string strEntityFunctionList = "";//实体方法

                string strJavaEntityVariableList = "";//Java实体变量
                string strJavaEntityFunctionList = "";//Java实体方法
                string strJavaHtmlCreateList = "";
                string strJavaHtmlEditList = "";
                string strJavaHtmlDetailsList = "";
                string strJavaHtmlList = "";

                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,DATA_TYPE,COLUMN_TYPE,COLUMN_KEY,COLUMN_COMMENT,EXTRA,COLUMN_DEFAULT,CHARACTER_SET_NAME FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strDataType = dr["DATA_TYPE"].ToString();//数据类型
                    strDataType = CodeRobot.Utility.StringHelper.GetCSharpDBType(strDataType);
                    string strColumnType = dr["COLUMN_TYPE"].ToString();//列类型长度
                    string strColumnKey = dr["COLUMN_KEY"].ToString();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    string strExtra = dr["EXTRA"].ToString();//是否自增
                    string strColumnDefault = dr["COLUMN_DEFAULT"].ToString();//默认值
                    string strCharacterSetName = dr["CHARACTER_SET_NAME"].ToString();//字符集

                    //不需要过滤
                    //strColumnComment = CommonHelper.GetColumnKeyComment(strColumnComment);

                    //构造实体类变量
                    #region
                    string strDefineValue = "m" + CommonHelper.GetTableNameUpper(strColumnName);
                    string strDefineCoreValue = strColumnName;


                    string strClassName = CommonHelper.GetTableNameUpper(strColumnName);
                    strClassName = CommonHelper.GetTableNameUpper(strClassName);//News
                    string strTableNameSpec = CommonHelper.GetTableNameFirstLowerSecondUpper(strClassName);//如：newsTypes
                    string strTableNameLower = strTableNameSpec.ToLower();//如：newstypes

                    //定义变量
                    string strValue = "";
                    string strJavaValue = "";
                    if (strColumnKey== "PRI")
                    {
                        strValue = "        /// <summary>\r\n";
                        strValue += "        /// " + strColumnComment + "\r\n";
                        strValue += "        /// </summary>\r\n";
                        strValue += "        [Key]\r\n";
                        strValue += "        public " + strDataType + "  " + strDefineCoreValue + " { get; set; }\r\n";

                        strJavaValue = "    \r\n";
                        strJavaValue = "    /**\r\n";
                        strJavaValue += "     * " + strColumnComment+ "\r\n";
                        strJavaValue += "     */\r\n";
                        strJavaValue += "    @TableId(value = \"" + strColumnName + "\", type = IdType.AUTO)\r\n";
                        strJavaValue += "    private " + strDataType + " "+ strClassName + ";\r\n";

                    }
                    else if (strColumnType== "datetime")
                    {
                        strValue = "        /// <summary>\r\n";
                        strValue += "        /// " + strColumnComment + "\r\n";
                        strValue += "        /// </summary>\r\n";
                        strValue += "        [DataType(DataType.DateTime)]\r\n";
                        strValue += "        public " + strDataType + "  " + strDefineCoreValue + " { get; set; }\r\n";
                    }
                    else
                    {
                        strValue = "        /// <summary>\r\n";
                        strValue += "        /// " + strColumnComment + "\r\n";
                        strValue += "        /// </summary>\r\n";
                        strValue += "        public " + strDataType + "  " + strDefineCoreValue + " { get; set; }\r\n";

                        strJavaValue = "    /**\r\n";
                        strJavaValue += "     * " + strColumnComment+ "\r\n";
                        strJavaValue += "     */\r\n";
                        strJavaValue += "    @TableId(value = \"" + strColumnName + "\")\r\n";
                        strJavaValue += "    private " + strDataType + " " + strClassName + ";\r\n";
                    }
                    strEntityVariableList += strValue;
                    strJavaEntityVariableList += strJavaValue;

                    //定义方法
                    string strFunction = "\r\n";
                    strFunction += "        /// <summary>\r\n";
                    strFunction += "        /// " + strColumnComment + "\r\n";
                    strFunction += "        /// </summary>\r\n";
                    strFunction += "        public " + strDataType + " " + CommonHelper.GetTableNameUpper(strColumnName) + "\r\n";
                    strFunction += "        {\r\n";
                    strFunction += "            get\r\n";
                    strFunction += "            {\r\n";
                    strFunction += "                return " + strDefineValue + ";\r\n";
                    strFunction += "            }\r\n";
                    strFunction += "            set\r\n";
                    strFunction += "            {\r\n";
                    strFunction += "                " + strDefineValue + " = value;\r\n";
                    strFunction += "            }\r\n";
                    strFunction += "        }\r\n";

                    strEntityFunctionList += strFunction;


                    //定义Java方法
                    string strJavaFunction = "	public " + strDataType + " get" + strClassName + "() {\r\n";
                    strJavaFunction += "		return " + strTableNameSpec + ";\r\n";
                    strJavaFunction += "	}\r\n";
                    strJavaFunction += "	public void set"+ strClassName + "(" + strDataType + " " + strTableNameSpec + ") {\r\n";
                    strJavaFunction += "		this." + strTableNameSpec + " = " + strTableNameSpec + ";\r\n";
                    strJavaFunction += "	}\r\n";

                    strJavaEntityFunctionList += strJavaFunction;
                    //定义Java 新增字段
                    string strJavaHtml = "		<div class=\"layui-form-item\">\r\n";
                    strJavaHtml += "			<label class=\"layui-form-label\">"+strColumnComment+ "</label>\r\n";
                    strJavaHtml += "			<div class=\"layui-input-inline\">\r\n";
                    strJavaHtml += "				<input type=\"text\" name=\"" + strColumnName + "\" lay-verify=\"required\" placeholder=\"请输入" + strColumnComment + "\" autocomplete=\"off\" class=\"layui-input\">\r\n";
                    strJavaHtml += "			</div>\r\n";
                    strJavaHtmlCreateList += strJavaHtml;
                    //定义Java 编辑字段
                    string strJavaEditHtml = "		<div class=\"layui-form-item\">\r\n";
                    strJavaEditHtml += "			<label class=\"layui-form-label\">" + strColumnComment + "</label>\r\n";
                    strJavaEditHtml += "			<div class=\"layui-input-inline\">\r\n";
                    strJavaEditHtml += "				<input type=\"text\"  th:value=\"${"+ strTableNameLower + "."+strColumnName+"}\" name=\"" + strColumnName + "\" lay-verify=\"required\" placeholder=\"请输入" + strColumnComment + "\" autocomplete=\"off\" class=\"layui-input\">\r\n";
                    strJavaEditHtml += "			</div>\r\n";
                    strJavaHtmlEditList += strJavaEditHtml;
                    //定义Java 详情字段
                    string strJavaDetailsHtml = "		<div class=\"layui-form-item\">\r\n";
                    strJavaDetailsHtml += "			<label class=\"layui-form-label\">" + strColumnComment + "</label>\r\n";
                    strJavaDetailsHtml += "			<div class=\"layui-input-inline\">\r\n";
                    strJavaDetailsHtml += "				<span th:text=\"${" + strTableNameLower + "." + strColumnName + "}\"></span>\r\n";
                    strJavaDetailsHtml += "			</div>\r\n";
                    strJavaHtmlDetailsList += strJavaDetailsHtml;

                    #endregion

                }
                dr.Dispose();
                cn.Close();

                //生成.net core实体类
                //ModelHelper.CreateModelClass(strModelPath, strProjectName, strTableName, strEntityVariableList, strTableColumnComment);
                ModelHelper.CreateModelClass(strModelsPath, strProjectName, strTableName, strEntityVariableList, strTableColumnComment);

                //生成Java实体类
                JavaEntityHelper.CreateEntityClass(strJavaModelPath, strProjectName, strJavaEntityVariableList,strJavaEntityFunctionList, strTableName, strTableColumnComment);
                //生成新增页面
                JavaViewsCreateHelper.CreateViewCreateClass(strJavaModelPath, strProjectName, strJavaHtmlCreateList, strTableName, strTableColumnComment);
                JavaViewsEditHelper.CreateViewEditClass(strJavaModelPath, strProjectName, strJavaHtmlEditList, strTableName, strTableColumnComment);
                JavaViewsListHelper.CreateViewListClass(strJavaModelPath, strProjectName, strJavaHtmlList, strTableName, strTableColumnComment);
                JavaViewsDetailsHelper.CreateViewDetailsClass(strJavaModelPath, strProjectName, strJavaHtmlDetailsList, strTableName, strTableColumnComment);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中的字段", "GetColumnList", false);
            }

        }

        /// <summary>
        /// 获取数据库表中所有表
        /// </summary>
        /// <param name="strDBName">数据库名</param>
        public static void GetDataDesignForTableList(string strDBName)
        {
            MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
            cn.Open();
            string strSql = "SELECT TABLE_NAME FROM `information_schema`.`COLUMNS` WHERE TABLE_SCHEMA='" + strDBName + "' GROUP BY TABLE_NAME";
            MySqlCommand cmd = new MySqlCommand(strSql, cn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string strTableName = dr["TABLE_NAME"].ToString();
                GetDataDesignForDocutment(strTableName);
            }
            dr.Dispose();
            cn.Close();
        }

        /// <summary>
        /// 生成表对应的文档
        /// </summary>
        /// <param name="strTableName">表名</param>
        public static void GetDataDesignForDocutment(string strTableName)
        {
            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,DATA_TYPE,COLUMN_TYPE,COLUMN_KEY,COLUMN_COMMENT,EXTRA,COLUMN_DEFAULT,CHARACTER_SET_NAME,IS_NULLABLE FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strDataType = dr["DATA_TYPE"].ToString();//字段类型
                    string strColumnType = dr["COLUMN_TYPE"].ToString();//字段长度
                    string strColumnKey = dr["COLUMN_KEY"].ToString();//是否是主键
                    string strIsNullable = dr["IS_NULLABLE"].ToString();//是否为空
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    strColumnComment = strColumnComment.Replace("{搜索}", "");
                    strColumnComment = strColumnComment.Replace("{下拉}", "");
                    strColumnComment = strColumnComment.Replace("{勾选}", "");

                    if (strIsNullable == "NO")
                    {
                        strIsNullable = "否";
                    }
                    else
                    {
                        strIsNullable = "是";
                    }

                    if (strColumnKey == "PRI")
                    {
                        strColumnKey = "是";
                    }
                    else
                    {
                        strColumnKey = "否";
                    }
                    string strColumn = strColumnName + "	" + strDataType + "	" + strColumnType + "	" + strIsNullable + "	" + strColumnKey + "	" + strColumnComment;
                    CodeRobot.Utility.TxtFile.CreateDoumentsFile(strColumn, strTableName);
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "生成表对应的文档", "GetDataDesignForDocutment", false);
            }
        }

        /// <summary>
        /// 生成表对应的SQL
        /// </summary>
        /// <param name="strTableName">表名</param>
        public static string GetSQL(string strTableName)
        {
            string strReturnString = "";

            try
            {
                string strPrimaryKey = "";
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,DATA_TYPE,COLUMN_TYPE,COLUMN_KEY,COLUMN_COMMENT,EXTRA,COLUMN_DEFAULT,CHARACTER_SET_NAME,IS_NULLABLE FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strDataType = dr["DATA_TYPE"].ToString();//字段类型
                    string strColumnType = dr["COLUMN_TYPE"].ToString();//字段长度
                    string strColumnKey = dr["COLUMN_KEY"].ToString();//是否是主键
                    string strIsNullable = dr["IS_NULLABLE"].ToString();//是否为空
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释

                    if (strColumnKey == "PRI")
                    {
                        strPrimaryKey = strColumnName;
                    }

                    string strColumn = "`" + strColumnName + "` " + strColumnType + " NOT NULL COMMENT '" + strColumnComment + "',\r\n";
                    strReturnString += strColumn;
                }
                dr.Dispose();
                cn.Close();

                strReturnString = strReturnString + "PRIMARY KEY (`" + strPrimaryKey + "`)";
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "生成表对应的SQL", "GetSQL", false);
            }

            return strReturnString;
        }

    }
}
