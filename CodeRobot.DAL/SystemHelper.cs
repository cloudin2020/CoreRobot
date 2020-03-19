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
    public class SystemHelper
    {
        /// <summary>
        /// 创建项目解决方案
        /// </summary>
        /// <param name="strFilePath">项目路径</param>
        /// <param name="strProjectName">项目名称</param>
        public static void CreateSolutionSystemFile(string strFilePath, string strProjectName)
        {
            try
            {
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(System.Windows.Forms.Application.StartupPath + "\\config.ini");
                string strVisualStudio = iniFile.GetString("BASE", "VS", "");
                string strVisualStudioVersion = iniFile.GetString("BASE", "VS_VERSION", "");
                string strMinimumVisualStudioVersion = iniFile.GetString("BASE", "MINI_VS_VERSION", "");
                string strNetFramework = iniFile.GetString("BASE", "FRAMEWORK", "");//.net framework版本号
                string strGUIDSolutionFramework = iniFile.GetString("GUID", "SOLUTION_FRAMEWORK", "").ToUpper();
                string strGUIDSolutionWeb = iniFile.GetString("GUID", "SOLUTION_WEB", "").ToUpper();
                string strGUIDDBSqlHelper = iniFile.GetString("GUID", "DBSQLHELPER", "").ToUpper();
                string strGUIDModel = iniFile.GetString("GUID", "MODEL", "").ToUpper();
                string strGUIDDAL = iniFile.GetString("GUID", "DAL", "").ToUpper();
                string strGUIDBLL = iniFile.GetString("GUID", "BLL", "").ToUpper();
                string strGUIDUtility = iniFile.GetString("GUID", "UTILITY", "").ToUpper();
                string strGUIDSMS = iniFile.GetString("GUID", "SMS", "").ToUpper();
                string strGUIDPush = iniFile.GetString("GUID", "PUSH", "").ToUpper();
                string strGUIDPay = iniFile.GetString("GUID", "PAY", "").ToUpper();
                string strGUIDUI = iniFile.GetString("GUID", "UI", "").ToUpper();
                string strGUIDHTML = iniFile.GetString("GUID", "HTML", "").ToUpper();
                string strGUIDPM = iniFile.GetString("GUID", "PM", "").ToUpper();
                string strGUIDTest = iniFile.GetString("GUID", "TEST", "").ToUpper();
                string strGUIDData = iniFile.GetString("GUID", "DATA", "").ToUpper();
                string strGUIDH5 = iniFile.GetString("GUID", "H5", "").ToUpper();
                string strGUIDManage = iniFile.GetString("GUID", "MANAGE", "").ToUpper();
                string strGUIDWeb = iniFile.GetString("GUID", "WEB", "").ToUpper();
                string strGUIDWeChatApp = iniFile.GetString("GUID", "WeChatApp", "").ToUpper();

                //创建WEB项目浏览虚拟端口
                int nUIPort = int.Parse(CodeRobot.Utility.CommonHelper.GenerateRandomNum(5));
                int nHTMLPort = nUIPort + 1;
                int nPMPort= nHTMLPort + 1;
                int nTestPort = nPMPort + 1;
                int nDataPort = nTestPort + 1;
                int nH5Port = nDataPort + 1;
                int nManagePort = nH5Port + 1;
                int nWebPort = nManagePort + 1;
                int nWeChatAppPort = nWebPort + 1;

                Directory.CreateDirectory(strFilePath);

                //创建项目文件-------------------------------------------1
                FileStream fs1 = new FileStream(Application.StartupPath + "\\template\\system\\Solution.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);
                string strGetPageHTMLContentForSolution = sr1.ReadToEnd();
                sr1.Close();
                fs1.Close();

                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_SOLUTION_FRAMEWORK", strGUIDSolutionFramework);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_SOLUTION_WEB", strGUIDSolutionWeb);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_DBSQLHELPER", strGUIDDBSqlHelper);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_MODEL", strGUIDModel);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_DAL", strGUIDDAL);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_BLL", strGUIDBLL);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_UTILITY", strGUIDUtility);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_SMS", strGUIDSMS);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_UI", strGUIDUI);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_HTML", strGUIDHTML);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_PM", strGUIDPM);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_TEST", strGUIDTest);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_H5", strGUIDH5);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_MANAGE", strGUIDManage);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_DATA", strGUIDData);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_WEB", strGUIDWeb);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("GUID_WeChatApp", strGUIDWeChatApp);

                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{UI_PORT}", nUIPort.ToString());
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{HTML_PORT}", nHTMLPort.ToString());
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{PM_PORT}", nPMPort.ToString());
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{TEST_PORT}", nTestPort.ToString());
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{DATA_PORT}", nDataPort.ToString());
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{H5_PORT}", nH5Port.ToString());
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{MANAGE_PORT}", nManagePort.ToString());
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{WEB_PORT}", nWebPort.ToString());
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{WeChatApp_PORT}", nWeChatAppPort.ToString());

                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{VS}", strVisualStudio);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{VS_VERSION}", strVisualStudioVersion);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{MINI_VS_VERSION}", strMinimumVisualStudioVersion);
                strGetPageHTMLContentForSolution = strGetPageHTMLContentForSolution.Replace("{NET_FRAMEWORK}", strNetFramework);

                StreamWriter sw1 = new StreamWriter(strFilePath + "\\" + strProjectName + "Solution.sln", false, Encoding.GetEncoding("utf-8"));
                sw1.WriteLine(strGetPageHTMLContentForSolution);
                sw1.Close();

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建项目解决方案", "CreateSolutionSystemFile",false);
            }
        }

        /// <summary>
        /// 创建DBSqlHelper系统文件及项目文件
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strProjectName"></param>
        public static void CreateDBSqlHelperSystemFile(string strFilePath, string strProjectName)
        {
            try
            {
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(System.Windows.Forms.Application.StartupPath + "\\config.ini");
                string strFramework = iniFile.GetString("BASE", "FRAMEWORK", "");//.net framework版本号
                string strGUID = iniFile.GetString("GUID", "DBSQLHELPER", "");//项目GUID

                Directory.CreateDirectory(strFilePath);

                //创建项目文件-------------------------------------------1
                FileStream fs1 = new FileStream(Application.StartupPath + "\\template\\system\\DBSqlHelper_csproj.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);
                string strGetPageHTMLContentForCSPROJ = sr1.ReadToEnd();
                sr1.Close();
                fs1.Close();

                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{framwork_version}", strFramework);

                StreamWriter sw1 = new StreamWriter(strFilePath + "\\" + strProjectName + ".DBSqlHelper.csproj", false, Encoding.GetEncoding("utf-8"));
                sw1.WriteLine(strGetPageHTMLContentForCSPROJ);
                sw1.Close();

                //创建AssemblyInfo文件-------------------------------------------2
                string strAssemblyPath = strFilePath + "\\Properties";
                if (!Directory.Exists(strAssemblyPath))
                {
                    Directory.CreateDirectory(strAssemblyPath);
                }

                FileStream fs2 = new FileStream(Application.StartupPath + "\\template\\system\\DBSqlHelper_AssemblyInfo.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr2 = new StreamReader(fs2, Encoding.UTF8);
                string strGetPageHTMLContentForAssembly = sr2.ReadToEnd();
                sr2.Close();
                fs2.Close();

                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_year}", DateTime.Now.Year.ToString());

                StreamWriter sw2 = new StreamWriter(strAssemblyPath + "\\AssemblyInfo.cs", false, Encoding.GetEncoding("utf-8"));
                sw2.WriteLine(strGetPageHTMLContentForAssembly);
                sw2.Close();

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建BLL README文件", "CreateBLLFile",false);
            }
        }

        /// <summary>
        /// 创建Model系统文件及项目文件
        /// </summary>
        /// <param name="strFilePath">文件路径</param>
        /// <param name="strProjectName">项目名称</param>
        /// <param name="strTableList">编译文件名称列表</param>
        public static void CreateModelSystemFile(string strFilePath, string strProjectName,string strTableList)
        {
            try
            {
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(System.Windows.Forms.Application.StartupPath + "\\config.ini");
                string strFramework = iniFile.GetString("BASE", "FRAMEWORK", "");//.net framework版本号
                string strGUID = iniFile.GetString("GUID", "MODEL", "");//项目GUID

                Directory.CreateDirectory(strFilePath);

                //创建项目文件-------------------------------------------1
                FileStream fs1 = new FileStream(Application.StartupPath + "\\template\\system\\Model_csproj.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);
                string strGetPageHTMLContentForCSPROJ = sr1.ReadToEnd();
                sr1.Close();
                fs1.Close();

                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{framwork_version}", strFramework);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{compile_table_list}", strTableList);

                StreamWriter sw1 = new StreamWriter(strFilePath + "\\" + strProjectName + ".Model.csproj", false, Encoding.GetEncoding("utf-8"));
                sw1.WriteLine(strGetPageHTMLContentForCSPROJ);
                sw1.Close();

                //创建AssemblyInfo文件-------------------------------------------2
                string strAssemblyPath = strFilePath + "\\Properties";
                if (!Directory.Exists(strAssemblyPath))
                {
                    Directory.CreateDirectory(strAssemblyPath);
                }

                FileStream fs2 = new FileStream(Application.StartupPath + "\\template\\system\\Model_AssemblyInfo.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr2 = new StreamReader(fs2, Encoding.UTF8);
                string strGetPageHTMLContentForAssembly = sr2.ReadToEnd();
                sr2.Close();
                fs2.Close();

                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_year}", DateTime.Now.Year.ToString());

                StreamWriter sw2 = new StreamWriter(strAssemblyPath + "\\AssemblyInfo.cs", false, Encoding.GetEncoding("utf-8"));
                sw2.WriteLine(strGetPageHTMLContentForAssembly);
                sw2.Close();

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建Model系统文件及项目文件", "CreateModelSystemFile",false);
            }
        }

        /// <summary>
        /// 创建DAL系统文件及项目文件
        /// </summary>
        /// <param name="strFilePath">文件路径</param>
        /// <param name="strProjectName">项目名称</param>
        /// <param name="strTableList">编译文件名称列表</param>
        public static void CreateDALSystemFile(string strFilePath, string strProjectName, string strTableList)
        {
            try
            {
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(System.Windows.Forms.Application.StartupPath + "\\config.ini");
                string strFramework = iniFile.GetString("BASE", "FRAMEWORK", "");//.net framework版本号
                string strGUID = iniFile.GetString("GUID", "DAL", "");//项目GUID
                string strDBSQLHELPERGUID = iniFile.GetString("GUID", "DBSQLHELPER", "");
                string strMODELGUID = iniFile.GetString("GUID", "MODEL", "");
                string strUTILITYGUID = iniFile.GetString("GUID", "UTILITY", "");

                Directory.CreateDirectory(strFilePath);

                //创建项目文件-------------------------------------------1
                FileStream fs1 = new FileStream(Application.StartupPath + "\\template\\system\\DAL_csproj.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);
                string strGetPageHTMLContentForCSPROJ = sr1.ReadToEnd();
                sr1.Close();
                fs1.Close();

                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{framwork_version}", strFramework);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{compile_table_list}", strTableList);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{DBSqlHelper_GUID}", strDBSQLHELPERGUID);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("Model_GUID", strMODELGUID);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("Utility_GUID", strUTILITYGUID);

                StreamWriter sw1 = new StreamWriter(strFilePath + "\\" + strProjectName + ".DAL.csproj", false, Encoding.GetEncoding("utf-8"));
                sw1.WriteLine(strGetPageHTMLContentForCSPROJ);
                sw1.Close();

                //创建AssemblyInfo文件-------------------------------------------2
                string strAssemblyPath = strFilePath + "\\Properties";
                if (!Directory.Exists(strAssemblyPath))
                {
                    Directory.CreateDirectory(strAssemblyPath);
                }

                FileStream fs2 = new FileStream(Application.StartupPath + "\\template\\system\\DAL_AssemblyInfo.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr2 = new StreamReader(fs2, Encoding.UTF8);
                string strGetPageHTMLContentForAssembly = sr2.ReadToEnd();
                sr2.Close();
                fs2.Close();

                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_year}", DateTime.Now.Year.ToString());

                StreamWriter sw2 = new StreamWriter(strAssemblyPath + "\\AssemblyInfo.cs", false, Encoding.GetEncoding("utf-8"));
                sw2.WriteLine(strGetPageHTMLContentForAssembly);
                sw2.Close();

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建DAL系统文件及项目文件", "CreateDALSystemFile",false);
            }
        }


        /// <summary>
        /// 创建BLL系统文件及项目文件
        /// </summary>
        /// <param name="strFilePath">文件路径</param>
        /// <param name="strProjectName">项目名称</param>
        /// <param name="strTableList">编译文件名称列表</param>
        public static void CreateBLLSystemFile(string strFilePath, string strProjectName, string strTableList)
        {
            try
            {
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(System.Windows.Forms.Application.StartupPath + "\\config.ini");
                string strFramework = iniFile.GetString("BASE", "FRAMEWORK", "");//.net framework版本号
                string strGUID = iniFile.GetString("GUID", "BLL", "");//项目GUID
                string strMODELGUID = iniFile.GetString("GUID", "MODEL", "");
                string strDALGUID = iniFile.GetString("GUID", "DAL", "");

                Directory.CreateDirectory(strFilePath);

                //创建项目文件-------------------------------------------1
                FileStream fs1 = new FileStream(Application.StartupPath + "\\template\\system\\BLL_csproj.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);
                string strGetPageHTMLContentForCSPROJ = sr1.ReadToEnd();
                sr1.Close();
                fs1.Close();

                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{framwork_version}", strFramework);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{compile_table_list}", strTableList);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("DAL_GUID", strDALGUID);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("Model_GUID", strMODELGUID);

                StreamWriter sw1 = new StreamWriter(strFilePath + "\\" + strProjectName + ".BLL.csproj", false, Encoding.GetEncoding("utf-8"));
                sw1.WriteLine(strGetPageHTMLContentForCSPROJ);
                sw1.Close();

                //创建AssemblyInfo文件-------------------------------------------2
                string strAssemblyPath = strFilePath + "\\Properties";
                if (!Directory.Exists(strAssemblyPath))
                {
                    Directory.CreateDirectory(strAssemblyPath);
                }

                FileStream fs2 = new FileStream(Application.StartupPath + "\\template\\system\\BLL_AssemblyInfo.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr2 = new StreamReader(fs2, Encoding.UTF8);
                string strGetPageHTMLContentForAssembly = sr2.ReadToEnd();
                sr2.Close();
                fs2.Close();

                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_year}", DateTime.Now.Year.ToString());

                StreamWriter sw2 = new StreamWriter(strAssemblyPath + "\\AssemblyInfo.cs", false, Encoding.GetEncoding("utf-8"));
                sw2.WriteLine(strGetPageHTMLContentForAssembly);
                sw2.Close();

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建BLL系统文件及项目文件", "CreateBLLSystemFile",false);
            }
        }

        /// <summary>
        /// 创建Utility系统文件及项目文件
        /// </summary>
        /// <param name="strFilePath">文件路径</param>
        /// <param name="strProjectName">项目名称</param>
        public static void CreateUtilitySystemFile(string strFilePath, string strProjectName)
        {
            try
            {
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(System.Windows.Forms.Application.StartupPath + "\\config.ini");
                string strFramework = iniFile.GetString("BASE", "FRAMEWORK", "");//.net framework版本号
                string strGUID = iniFile.GetString("GUID", "Utility", "");//项目GUID

                Directory.CreateDirectory(strFilePath);

                //创建项目文件-------------------------------------------1
                FileStream fs1 = new FileStream(Application.StartupPath + "\\template\\system\\Utility_csproj.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);
                string strGetPageHTMLContentForCSPROJ = sr1.ReadToEnd();
                sr1.Close();
                fs1.Close();

                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{framwork_version}", strFramework);
                //strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{compile_table_list}", strTableList);

                StreamWriter sw1 = new StreamWriter(strFilePath + "\\" + strProjectName + ".Utility.csproj", false, Encoding.GetEncoding("utf-8"));
                sw1.WriteLine(strGetPageHTMLContentForCSPROJ);
                sw1.Close();

                //创建AssemblyInfo文件-------------------------------------------2
                string strAssemblyPath = strFilePath + "\\Properties";
                if (!Directory.Exists(strAssemblyPath))
                {
                    Directory.CreateDirectory(strAssemblyPath);
                }

                FileStream fs2 = new FileStream(Application.StartupPath + "\\template\\system\\Utility_AssemblyInfo.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr2 = new StreamReader(fs2, Encoding.UTF8);
                string strGetPageHTMLContentForAssembly = sr2.ReadToEnd();
                sr2.Close();
                fs2.Close();

                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_year}", DateTime.Now.Year.ToString());

                StreamWriter sw2 = new StreamWriter(strAssemblyPath + "\\AssemblyInfo.cs", false, Encoding.GetEncoding("utf-8"));
                sw2.WriteLine(strGetPageHTMLContentForAssembly);
                sw2.Close();

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建SMS系统文件及项目文件", "CreateSMSSystemFile",false);
            }
        }

        /// <summary>
        /// 创建SMS系统文件及项目文件
        /// </summary>
        /// <param name="strFilePath">文件路径</param>
        /// <param name="strProjectName">项目名称</param>
        public static void CreateSMSSystemFile(string strFilePath, string strProjectName)
        {
            try
            {
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(System.Windows.Forms.Application.StartupPath + "\\config.ini");
                string strFramework = iniFile.GetString("BASE", "FRAMEWORK", "");//.net framework版本号
                string strGUID = iniFile.GetString("GUID", "SMS", "");//项目GUID
                string strUTILITYGUID = iniFile.GetString("GUID", "UTILITY", "");//项目GUID

                Directory.CreateDirectory(strFilePath);

                //创建项目文件-------------------------------------------1
                FileStream fs1 = new FileStream(Application.StartupPath + "\\template\\system\\SMS_csproj.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);
                string strGetPageHTMLContentForCSPROJ = sr1.ReadToEnd();
                sr1.Close();
                fs1.Close();

                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{framwork_version}", strFramework);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{Utility_GUID}", strUTILITYGUID);

                StreamWriter sw1 = new StreamWriter(strFilePath + "\\" + strProjectName + ".SMS.csproj", false, Encoding.GetEncoding("utf-8"));
                sw1.WriteLine(strGetPageHTMLContentForCSPROJ);
                sw1.Close();

                //创建AssemblyInfo文件-------------------------------------------2
                string strAssemblyPath = strFilePath + "\\Properties";
                if (!Directory.Exists(strAssemblyPath))
                {
                    Directory.CreateDirectory(strAssemblyPath);
                }

                FileStream fs2 = new FileStream(Application.StartupPath + "\\template\\system\\SMS_AssemblyInfo.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr2 = new StreamReader(fs2, Encoding.UTF8);
                string strGetPageHTMLContentForAssembly = sr2.ReadToEnd();
                sr2.Close();
                fs2.Close();

                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_year}", DateTime.Now.Year.ToString());

                StreamWriter sw2 = new StreamWriter(strAssemblyPath + "\\AssemblyInfo.cs", false, Encoding.GetEncoding("utf-8"));
                sw2.WriteLine(strGetPageHTMLContentForAssembly);
                sw2.Close();

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建SMS系统文件及项目文件", "CreateSMSSystemFile",false);
            }
        }

        /// <summary>
        /// 创建Push系统文件及项目文件
        /// </summary>
        /// <param name="strFilePath">文件路径</param>
        /// <param name="strProjectName">项目名称</param>
        public static void CreatePushSystemFile(string strFilePath, string strProjectName)
        {
            try
            {
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(System.Windows.Forms.Application.StartupPath + "\\config.ini");
                string strFramework = iniFile.GetString("BASE", "FRAMEWORK", "");//.net framework版本号
                string strGUID = iniFile.GetString("GUID", "Push", "");//项目GUID

                Directory.CreateDirectory(strFilePath);

                //创建项目文件-------------------------------------------1
                FileStream fs1 = new FileStream(Application.StartupPath + "\\template\\system\\Push_csproj.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr1 = new StreamReader(fs1, Encoding.UTF8);
                string strGetPageHTMLContentForCSPROJ = sr1.ReadToEnd();
                sr1.Close();
                fs1.Close();

                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{framwork_version}", strFramework);
                //strGetPageHTMLContentForCSPROJ = strGetPageHTMLContentForCSPROJ.Replace("{compile_table_list}", strTableList);

                StreamWriter sw1 = new StreamWriter(strFilePath + "\\" + strProjectName + ".Push.csproj", false, Encoding.GetEncoding("utf-8"));
                sw1.WriteLine(strGetPageHTMLContentForCSPROJ);
                sw1.Close();

                //创建AssemblyInfo文件-------------------------------------------2
                string strAssemblyPath = strFilePath + "\\Properties";
                if (!Directory.Exists(strAssemblyPath))
                {
                    Directory.CreateDirectory(strAssemblyPath);
                }

                FileStream fs2 = new FileStream(Application.StartupPath + "\\template\\system\\Push_AssemblyInfo.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr2 = new StreamReader(fs2, Encoding.UTF8);
                string strGetPageHTMLContentForAssembly = sr2.ReadToEnd();
                sr2.Close();
                fs2.Close();

                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_guid}", strGUID);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_name}", strProjectName);
                strGetPageHTMLContentForAssembly = strGetPageHTMLContentForAssembly.Replace("{project_year}", DateTime.Now.Year.ToString());

                StreamWriter sw2 = new StreamWriter(strAssemblyPath + "\\AssemblyInfo.cs", false, Encoding.GetEncoding("utf-8"));
                sw2.WriteLine(strGetPageHTMLContentForAssembly);
                sw2.Close();

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建Push系统文件及项目文件", "CreatePushSystemFile",false);
            }
        }
    }
}
