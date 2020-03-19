using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRobot.DAL
{
    /// <summary>
    /// 创建README文件
    /// </summary>
    public class ReadmeHelper
    {
        /// <summary>
        /// 创建项目README文件
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strProjectName"></param>
        public static void CreateProjectFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + "\r\n";
                strLine += "\r\n";
                strLine += "### **底层项目**\r\n";
                strLine += "1. "+ strProjectName + ".DBSqlHelper\r\n";
                strLine += "2. " + strProjectName + ".Model\r\n";
                strLine += "3. " + strProjectName + ".DAL\r\n";
                strLine += "4. " + strProjectName + ".BLL\r\n";
                strLine += "5. " + strProjectName + ".Utility\r\n";
                strLine += "\r\n";
                strLine += "### **静态页面项目**\r\n";
                strLine += "" + strProjectName + ".HTML\r\n";
                strLine += "\r\n";
                strLine += "### **后台项目**\r\n";
                strLine += "" + strProjectName + ".Manage\r\n";
                strLine += "\r\n";
                strLine += "### **官网项目**\r\n";
                strLine += "" + strProjectName + ".Web\r\n";
                strLine += "\r\n";
                strLine += "### **H5项目**\r\n";
                strLine += "" + strProjectName + ".H5\r\n";
                strLine += "\r\n";
                strLine += "### **小程序项目**\r\n";
                strLine += "" + strProjectName + ".WeChatApp\r\n";
                strLine += "\r\n";
                strLine += "### **iOS项目**\r\n";
                strLine += "" + strProjectName + ".iOS\r\n";
                strLine += "\r\n";
                strLine += "### **Android项目**\r\n";
                strLine += "" + strProjectName + ".Android\r\n";
                strLine += "\r\n";
                strLine += "###** API接口项目**\r\n";
                strLine += "" + strProjectName + ".API\r\n";
                strLine += "\r\n";
                strLine += "### **短信接口项目**\r\n";
                strLine += "" + strProjectName + ".SMS\r\n";
                strLine += "\r\n";
                strLine += "### **推送接口项目**\r\n";
                strLine += "" + strProjectName + ".Push\r\n";
                strLine += "\r\n";
                strLine += "### **支付接口项目**\r\n";
                strLine += "" + strProjectName + ".Pay\r\n";
                strLine += "\r\n";
                strLine += "### **项目管理相关**\r\n";
                strLine += "1. " + strProjectName + ".PM 产品经理相关材料项目\r\n";
                strLine += "2. " + strProjectName + ".UI 产品设计相关材料项目\r\n";
                strLine += "3. " + strProjectName + ".Test 测试相关材料项目\r\n";
                strLine += "4. " + strProjectName + ".Data 数据库相关材料项目\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建项目README文件", "CreateProjectFile",false);
            }
        }

        /// <summary>
        /// 创建BLL README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateBLLFile(string strPath,string strProjectName)
        {
            try
            {
                string strLine = "# "+ strProjectName + ".BLL\r\n";
                strLine += "\r\n";
                strLine += "### **业务处理层**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "UsersModel user = new UsersModel();\r\n";
                strLine += "user.UserName = \"cloudin\";\r\n";
                strLine += "user.UserPassword = \"53344521\";\r\n";
                strLine += "user.NickName = \"云里科技\";\r\n";
                strLine += "user.UserType = 1;\r\n";
                strLine += "\r\n";
                strLine += "UsersBLL.InsertUsers(user);\r\n";
                strLine += "\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目说明**\r\n";
                strLine += "\r\n";
                strLine += "1. " + strProjectName + ".DAL //数据处理层\r\n";
                strLine += "2. " + strProjectName + ".Model //数据实体类\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建BLL README文件", "CreateBLLFile",false);
            }
        }

        /// <summary>
        /// 创建DAL README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateDALFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".DAL\r\n";
                strLine += "\r\n";
                strLine += "### **数据处理层**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "/// <summary>\r\n";
                strLine += "/// 新增一条数据\r\n";
                strLine += "/// </summary>\r\n";
                strLine += "/// <param name=\"usersModel\">实体类</param>\r\n";
                strLine += "/// <returns></returns>\r\n";
                strLine += "public static bool InsertUsers(UsersModel usersModel)\r\n";
                strLine += "{\r\n";
                strLine += "	UsersDAL usersDAL = new UsersDAL();\r\n";
                strLine += "\r\n";
                strLine += "	return usersDAL.InsertUsers(usersModel);\r\n";
                strLine += "}\r\n";
                strLine += "\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "1. install-package MySql.Data -pre\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目说明**\r\n";
                strLine += "\r\n";
                strLine += "1. " + strProjectName + ".DBSqlHelper //数据连接\r\n";
                strLine += "2. " + strProjectName + ".Model //数据实体类\r\n";
                strLine += "3. " + strProjectName + ".Utility //常用工具封装类\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建DAL README文件", "CreateDALFile",false);
            }
        }

        /// <summary>
        /// 创建DBSqlHelper README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateDBSqlHelperFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".DBSqlHelper\r\n";
                strLine += "\r\n";
                strLine += "### **数据连接封装类**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "private DBMySQLHelper mysqlHelper;\r\n";
                strLine += "\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建DBSqlHelper README文件", "CreateDBSqlHelperFile",false);
            }
        }

        /// <summary>
        /// 创建Model README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateModelFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".Model\r\n";
                strLine += "\r\n";
                strLine += "### **实体数据类**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "UsersModel user = new UsersModel();\r\n";
                strLine += "\r\n";
                strLine += "user.UserName = \"cloudin\";\r\n";
                strLine += "user.UserPassword = \"53344521\";\r\n";
                strLine += "user.UserType = 1;\r\n";
                strLine += "user.NickName = \"云里科技\";\r\n";
                strLine += "\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建Model README文件", "CreateModelFile",false);
            }
        }

        /// <summary>
        /// 创建Utility README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateUtilityFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".Utility\r\n";
                strLine += "\r\n";
                strLine += "### **常用工具封装**\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "" + strProjectName + ".Utility.LogHelper.Error(typeof(UsersDAL), ex, \"新增一条数据，SQL语句：\" + strSql, \"InsertUsers\",false);\r\n";
                strLine += "特别注意：如果是WinForm程序，必须把log4net.config配置文件拷贝到Debug目录下，否则日志记录不生效\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "1. Newtonsoft.Json\r\n";
                strLine += "2. System.Drawing.Common\r\n";
                strLine += "3. System.Configuration;\r\n";
                strLine += "4. System.Web.Extensions;\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "1. install-package log4net //日志管理\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建Utility README文件", "CreateUtilityFile",false);
            }
        }


        /// <summary>
        /// 创建MANAGE README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateManageFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".Manage\r\n";
                strLine += "\r\n";
                strLine += "### **后台管理系统**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "<span class=\"badge\">5</span>\r\n";
                strLine += "<span class=\"badge badge-primary\">10</span>\r\n";
                strLine += "<span class=\"badge badge-success\">15</span>\r\n";
                strLine += "<span class=\"badge badge-info\">20</span>\r\n";
                strLine += "<span class=\"badge badge-inverse\">25</span>\r\n";
                strLine += "<span class=\"badge badge-warning\">30</span>\r\n";
                strLine += "<span class=\"badge badge-important\">35</span>\r\n";
                strLine += "\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "install-package log4net //日志管理\r\n";
                strLine += "install-package Newtonsoft.Json //JSON\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目说明**\r\n";
                strLine += "\r\n";
                strLine += "1. " + strProjectName + ".DBSqlHelper //数据连接\r\n";
                strLine += "2. " + strProjectName + ".Model //数据实体类\r\n";
                strLine += "3. " + strProjectName + ".Utility //常用工具封装类\r\n";
                strLine += "4. " + strProjectName + ".BLL //业务逻辑处理类\r\n";
                strLine += "\r\n";
                strLine += "### **注意事项**\r\n";
                strLine += "\r\n";
                strLine += "1. 新建数据库时注意修改logs表中的type_id注释\r\n";
                strLine += "2. 修改users表中的user_type注释\r\n";
                strLine += "3. 修改news表中的category_id注释\r\n";
                strLine += "4. 修改videos表中的type_id注释\r\n";
                strLine += "5. 代码中有页面SQL语句带有表名bl_users,bl_new,bl_videos 要批量替换\r\n";
                strLine += "6. 生成图片命名规则cloudin_cn_\r\n";
                strLine += "7. Web.config配置文件修改配置路径\r\n";
                strLine += "<connectionStrings>\r\n";
                strLine += "    <add name=\"WesiteURL\" connectionString=\"http://www.xxx.com\" />\r\n";
                strLine += "    <add name=\"WeChatURL\" connectionString=\"http://wechat.xxx.com\" />\r\n";
                strLine += "    <add name=\"ImgURL\" connectionString=\"http://img.xxx.com\" />\r\n";
                strLine += "    <add name=\"SaveImgPath\" connectionString=\"c:\\web\\customers\\img.xxx.com\\\" />\r\n";
                strLine += "</connectionStrings>\r\n";
                strLine += "\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建MANAGE README文件", "CreateManageFile",false);
            }
        }


        /// <summary>
        /// 创建WEB README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateWebFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".Web\r\n";
                strLine += "\r\n";
                strLine += "### **官方网站**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "MySql.Data.dll //调用MySQL数据库\r\n";
                strLine += "1. install-package log4net //日志管理\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目说明**\r\n";
                strLine += "\r\n";
                strLine += "1. " + strProjectName + ".DBSqlHelper //数据连接\r\n";
                strLine += "2. " + strProjectName + ".Model //数据实体类\r\n";
                strLine += "3. " + strProjectName + ".Utility //常用工具封装类\r\n";
                strLine += "4. " + strProjectName + ".BLL //业务逻辑处理类\r\n";
                strLine += "5. " + strProjectName + ".DAL //数据处理类\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建WEB README文件", "CreateWebFile",false);
            }
        }


        /// <summary>
        /// 创建WeChatApp README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateWeChatAppFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".WeChatApp\r\n";
                strLine += "\r\n";
                strLine += "### **微信小程序**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建WeChatApp README文件", "CreateWeChatAppFile",false);
            }
        }


        /// <summary>
        /// 创建WeChat README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateWeChatFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".H5\r\n";
                strLine += "\r\n";
                strLine += "### **H5官网，微信版**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "```\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "1. MySql.Data.dll //调用MySQL数据库\r\n";
                strLine += "2. install-package log4net //日志管理\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目说明**\r\n";
                strLine += "\r\n";
                strLine += "1. " + strProjectName + ".DBSqlHelper //数据连接\r\n";
                strLine += "2. " + strProjectName + ".Model //数据实体类\r\n";
                strLine += "3. " + strProjectName + ".Utility //常用工具封装类\r\n";
                strLine += "4. " + strProjectName + ".BLL //业务逻辑处理类\r\n";
                strLine += "5. " + strProjectName + ".DAL //数据处理类\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建WeChat README文件", "CreateWeChatFile",false);
            }
        }

        /// <summary>
        /// 创建RESTAPI README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateRESTAPIFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".API\r\n";
                strLine += "\r\n";
                strLine += "### **Rest API**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "1. MySql.Data.dll //调用MySQL数据库\r\n";
                strLine += "2. install-package log4net //日志管理\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目说明**\r\n";
                strLine += "\r\n";
                strLine += "1. " + strProjectName + ".DBSqlHelper //数据连接\r\n";
                strLine += "2. " + strProjectName + ".Model //数据实体类\r\n";
                strLine += "3. " + strProjectName + ".Utility //常用工具封装类\r\n";
                strLine += "4. " + strProjectName + ".BLL //业务逻辑处理类\r\n";
                strLine += "5. " + strProjectName + ".DAL //数据处理类\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建RESTAPI README文件", "CreateRESTAPIFile",false);
            }
        }

        /// <summary>
        /// 创建PM README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreatePMFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".PM\r\n";
                strLine += "\r\n";
                strLine += "### **项目需求及产品原型相关文档**\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建PM README文件", "CreatePMFile",false);
            }
        }

        /// <summary>
        /// 创建UI README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateUIFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".UI\r\n";
                strLine += "\r\n";
                strLine += "### **UI源文件及切图资源文件**\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "UI设计把此项目对应的设计源文件及切图文件存储对应的文件夹\r\n";
                strLine += "每个项目按照各版本存储，如：\r\n";
                strLine += "1. web - 对应项目PC版UI文件\r\n";
                strLine += "2. android - 对应项目安卓版UI文件\r\n";
                strLine += "3. iOS - 对应项目iOS版UI文件\r\n";
                strLine += "4. manage - 对应项目后台管理系统UI文件\r\n";
                strLine += "5. h5 - 对应项目HTML5版UI文件\r\n";
                strLine += "6. vi - 对应项目VI文件\r\n";
                strLine += "7. others - 对应项目其他UI文件暂无\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建UI README文件", "CreateUIFile",false);
            }
        }

        /// <summary>
        /// 创建TEST README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateTESTFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".Test\r\n";
                strLine += "\r\n";
                strLine += "### **测试相关文档**\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建TEST README文件", "CreateTESTFile",false);
            }
        }

        /// <summary>
        /// 创建DATA README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateDATAFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".Data\r\n";
                strLine += "\r\n";
                strLine += "### **数据库日常备份**\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "存在以下情况需要备份数据库\r\n";
                strLine += "1. 数据结构存在新增表或删除表情况\r\n";
                strLine += "2. 后台数据有重大新增情况\r\n";
                strLine += "3. 发生重大版本更新情况下\r\n";
                strLine += "4. 正式上线版本时需要备份\r\n";
                strLine += "5. 数据库迁移时需要备份\r\n";
                strLine += "6. 日常最好每隔一周备份一次\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建PM README文件", "CreatePMFile",false);
            }
        }

        /// <summary>
        /// 创建HTML README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateHTMLFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".HTML\r\n";
                strLine += "\r\n";
                strLine += "### **前端静态页面资源存放处**\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "如果项目存在多个版本，请按以下规则存放\r\n";
                strLine += "1. web - 对应项目PC版前端静态文件\r\n";
                strLine += "2. h5 - 对应项目HTML5版前端静态文件\r\n";
                strLine += "3. miniapp - 对应项目微信小程序前端静态文件\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建UI README文件", "CreateUIFile",false);
            }
        }

        /// <summary>
        /// 创建PUSH README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreatePUSHFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".Push\r\n";
                strLine += "\r\n";
                strLine += "### **消息推送封装类**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "1. System.Web\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "1. install-package Newtonsoft.Json //JSON\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建PUSH README文件", "CreatePUSHFile",false);
            }
        }

        /// <summary>
        /// 创建SMS README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateSMSFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".SMS\r\n";
                strLine += "\r\n";
                strLine += "### **短信接口封装类**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "### **引用程序集说明**\r\n";
                strLine += "\r\n";
                strLine += "1. System.Web//web项目\r\n";
                strLine += "\r\n";
                strLine += "### **引用第三方组件**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目说明**\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建SMS README文件", "CreateSMSFile",false);
            }
        }

        /// <summary>
        /// 创建Manage(.net core) README文件
        /// </summary>
        /// <param name="strPath">路径</param>
        /// <param name="strProjectName">项目命名</param>
        public static void CreateCoreFile(string strPath, string strProjectName)
        {
            try
            {
                string strLine = "# " + strProjectName + ".Core\r\n";
                strLine += "\r\n";
                strLine += "### **后台管理系统.net core**\r\n";
                strLine += "\r\n";
                strLine += "demo:\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "暂无\r\n";
                strLine += "\r\n";
                strLine += "***\r\n";
                strLine += "\r\n";
                strLine += "### **引用框架**\r\n";
                strLine += "\r\n";
                strLine += "Microsoft.AspNetCore.App\r\n";
                strLine += "Microsoft.NETCore.App\r\n";
                strLine += "log4net\r\n";
                strLine += "System.Drawing.Common\r\n";
                strLine += "\r\n";
                strLine += "### **引用包**\r\n";
                strLine += "工具 → NuGet包管理 → 程序包管理控制台\r\n";
                strLine += "\r\n";
                strLine += "Microsoft.EntityFrameworkCore.Tools\r\n";
                strLine += "Pomelo.EntityFrameworkCore.MySql\r\n";
                strLine += "\r\n";
                strLine += "### **引用项目**\r\n";
                strLine += "\r\n";
                strLine += strProjectName + ".Models\r\n";
                strLine += strProjectName + ".Utility\r\n";
                strLine += "\r\n";

                CodeRobot.Utility.TxtFile.CreateReadMeFile(strLine, strPath);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建Manage(.net core) README文件", "CreateCoreFile", false);
            }
        }
    }
}
