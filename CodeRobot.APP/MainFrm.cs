using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace CodeRobot.APP
{
    public partial class MainFrm : Form
    {
        delegate void SetTextCallback(string text);
        private Thread catchThread = null;

        CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");

        /// <summary>
        /// 显示实时消息到showMsg
        /// </summary>
        /// <param name="strMessage">实时消息</param>
        private void SetText(string strMessage)
        {
            if (this.showMsg.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { strMessage });
            }
            else
            {
                this.showMsg.Text = strMessage;
            }
        }

        /// <summary>
        /// 关闭整个应用程序进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KillProcess(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public MainFrm()
        {
            InitializeComponent();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            this.Closing += new CancelEventHandler(KillProcess);
            skinEngine1.SkinFile = "Calmness.ssk";

            //读取历史设置信息
            string strGetServer = iniFile.GetString("BASE", "SERVER", "");//数据库地址
            string strGetDBName = iniFile.GetString("BASE", "DBNAME", "");//数据库名
            string strGetDBUser = iniFile.GetString("BASE", "USERID", "");//登录数据库用户名
            string strGetDBPassword = iniFile.GetString("BASE", "PASSWORD", "");//登录数据库密码

            txtCharset.Text = iniFile.GetString("BASE", "CHARSET", "");//数据库字符集
            txtProjectName.Text = iniFile.GetString("BASE", "PROJECT", "");//项目命名

            //解密
            //if (!string.IsNullOrEmpty(strGetServer))
            //{
            //    try
            //    {
            //        strGetServer = CodeRobot.Utility.CryptographyHelper.Decrypt(strGetServer, CodeRobot.Utility.CommonHelper.cryptographyKey);
            //    }
            //    catch (Exception ex)
            //    {
            //        CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "MainFrm_Load", "解密", false);
            //    }

            //    try
            //    {
            //        strGetDBName = CodeRobot.Utility.CryptographyHelper.Decrypt(strGetDBName, CodeRobot.Utility.CommonHelper.cryptographyKey);
            //    }
            //    catch (Exception ex)
            //    {
            //        CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "MainFrm_Load", "解密", false);
            //    }

            //    try
            //    {
            //        strGetDBUser = CodeRobot.Utility.CryptographyHelper.Decrypt(strGetDBUser, CodeRobot.Utility.CommonHelper.cryptographyKey);
            //    }
            //    catch (Exception ex)
            //    {
            //        CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "MainFrm_Load", "解密", false);
            //    }

            //    try
            //    {
            //        strGetDBPassword = CodeRobot.Utility.CryptographyHelper.Decrypt(strGetDBPassword, CodeRobot.Utility.CommonHelper.cryptographyKey);
            //    }
            //    catch (Exception ex)
            //    {
            //        CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "MainFrm_Load", "解密", false);
            //    }
            //}
            
            txtServer.Text = strGetServer;
            txtDBName.Text = strGetDBName;
            txtUserID.Text = strGetDBUser;
            txtPassword.Text = strGetDBPassword;

            //各项目GUID编码
            string strDBSqlHelperGUID = iniFile.GetString("GUID", "DBSQLHELPER", "");
            if (string.IsNullOrEmpty(strDBSqlHelperGUID))
            {
                strDBSqlHelperGUID = System.Guid.NewGuid().ToString("D");
                iniFile.WriteValue("GUID", "DBSqlHelper", strDBSqlHelperGUID);
            }

            string strMODELGUID = iniFile.GetString("GUID", "MODEL", "");
            if (string.IsNullOrEmpty(strMODELGUID))
            {
                strMODELGUID = System.Guid.NewGuid().ToString("D");
                iniFile.WriteValue("GUID", "MODEL", strMODELGUID);
            }

            string strDALGUID = iniFile.GetString("GUID", "DAL", "");
            if (string.IsNullOrEmpty(strDALGUID))
            {
                strDALGUID = System.Guid.NewGuid().ToString("D");
                iniFile.WriteValue("GUID", "DAL", strDALGUID);
            }

            string strBLLGUID = iniFile.GetString("GUID", "BLL", "");
            if (string.IsNullOrEmpty(strBLLGUID))
            {
                strBLLGUID = System.Guid.NewGuid().ToString("D");
                iniFile.WriteValue("GUID", "BLL", strBLLGUID);
            }

            string strUtilityGUID = iniFile.GetString("GUID", "UTILITY", "");
            if (string.IsNullOrEmpty(strUtilityGUID))
            {
                strUtilityGUID = System.Guid.NewGuid().ToString("D");
                iniFile.WriteValue("GUID", "Utility", strUtilityGUID);
            }

            string strSMSGUID = iniFile.GetString("GUID", "SMS", "");
            if (string.IsNullOrEmpty(strSMSGUID))
            {
                strSMSGUID = System.Guid.NewGuid().ToString("D");
                iniFile.WriteValue("GUID", "SMS", strSMSGUID);
            }

            string strPUSHGUID = iniFile.GetString("GUID", "PUSH", "");
            if (string.IsNullOrEmpty(strPUSHGUID))
            {
                strPUSHGUID = System.Guid.NewGuid().ToString("D");
                iniFile.WriteValue("GUID", "PUSH", strPUSHGUID);
            }


            //Android
            txtAndroidPackage.Text = iniFile.GetString("ANDROID", "PACKAGE", "");//包名
            //iOS
            txtiOSPackage.Text = iniFile.GetString("IOS", "PACKAGE", "");//包名

            //遍历历史记录，填充
            string strListData = iniFile.GetString("BASE", "LIST", "");
            if (!string.IsNullOrEmpty(strListData))
            {
                string[] splitData = strListData.Split(new char[] { ';' });
                for (int i = 0; i < splitData.Length; i++)
                {
                    string strProjectName = splitData[i];

                    cbList.Text = "请选择项目";
                    cbList.Items.Add(strProjectName);
                }
            }
            

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;

            catchThread = new Thread(new ThreadStart(DoTask));
            catchThread.Start();
        }

        private void DoTask()
        {
            try
            {
                CodeRobot.Utility.PublicValue.SaveMessage(Convert.ToDateTime(DateTime.Now).ToString("MM-dd HH:mm:ss") + " 任务开始...");
                this.SetText(CodeRobot.Utility.PublicValue.GetMessage());

                //数据库地址
                string strServer = txtServer.Text.Trim();
                //数据库名
                string strDBName = txtDBName.Text.Trim();
                //登录数据库用户名
                string strDBUser = txtUserID.Text.Trim();
                //登录数据库密码
                string strDBPassword = txtPassword.Text.Trim();
                //数据库字符集
                string strCharset = txtCharset.Text.Trim();
                //项目名称
                string strProjectName = txtProjectName.Text.Trim();
                //Android包名
                string strAndroidPackage = txtAndroidPackage.Text.Trim();
                //iOS包名
                string striOSPackage = txtiOSPackage.Text.Trim();

                //特殊字段需要加密存储
                //try
                //{
                //    strServer = CodeRobot.Utility.CryptographyHelper.Encryption(strServer, CodeRobot.Utility.CommonHelper.cryptographyKey);
                //    strDBName = CodeRobot.Utility.CryptographyHelper.Encryption(strDBName, CodeRobot.Utility.CommonHelper.cryptographyKey);
                //    strDBUser = CodeRobot.Utility.CryptographyHelper.Encryption(strDBUser, CodeRobot.Utility.CommonHelper.cryptographyKey);
                //    strDBPassword = CodeRobot.Utility.CryptographyHelper.Encryption(strDBPassword, CodeRobot.Utility.CommonHelper.cryptographyKey);
                //}
                //catch (Exception ex)
                //{
                //    CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "特殊字段需要加密存储", "DoTask", false);
                //}

                //存储设置
                iniFile.WriteValue("BASE", "SERVER", strServer);
                iniFile.WriteValue("BASE", "DBNAME", strDBName);
                iniFile.WriteValue("BASE", "USERID", strDBUser);
                iniFile.WriteValue("BASE", "PASSWORD", strDBPassword);
                iniFile.WriteValue("BASE", "CHARSET", strCharset);
                iniFile.WriteValue("BASE", "PROJECT", strProjectName);
                iniFile.WriteValue("ANDROID", "PACKAGE", strAndroidPackage);
                iniFile.WriteValue("IOS", "PACKAGE", striOSPackage);

                //拼接字符串
                string strProjectInfo = strProjectName;
                //存储到INI文件
                string strListData = iniFile.GetString("BASE", "LIST", "");//读取历史记录
                                                                           //更新版本号
                string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");//读取历史记录
                if (string.IsNullOrEmpty(strCode))
                {
                    strCode = "1";
                }
                int nCode = int.Parse(strCode) + 1;
                if (strListData.Contains(strProjectName))
                {
                    iniFile.WriteValue("COPYRIGHT", "CODE", nCode.ToString());
                }
                else
                {
                    //新项目
                    iniFile.WriteValue("COPYRIGHT", "VERSION", "1.0");
                    iniFile.WriteValue("COPYRIGHT", "CODE", "1");
                }

                if (!strListData.Contains(strProjectName))
                {
                    //如果已经存储，则不需要再存储此项目
                    if (string.IsNullOrEmpty(strListData))
                    {
                        iniFile.WriteValue("BASE", "LIST", strProjectInfo);
                    }
                    else
                    {
                        iniFile.WriteValue("BASE", "LIST", strListData + ";" + strProjectInfo);
                    }
                }

                //为每个项目创建一个XML本地文件存储信息
                string strXMLPath = Application.StartupPath + "\\xml";//为每个项目存储一个JSON文件
                if (!Directory.Exists(strXMLPath))
                {
                    Directory.CreateDirectory(strXMLPath);
                }
                string strXMLFile = strXMLPath + "\\" + strProjectName + ".xml";
                if (!File.Exists(strXMLFile))
                {
                    //创建
                    CreateXmlFile(strProjectName, strXMLFile, strServer, strDBName, strDBUser, strDBPassword, strCharset, striOSPackage, strAndroidPackage);
                }
                else
                {
                    //更新
                    //读取版本号
                    string strGetCode = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/Code", "");
                    int nGetCode = int.Parse(strGetCode);
                    nGetCode = nGetCode + 1;
                    //累加1，更新
                    CodeRobot.Utility.XMLHelper.Update(strXMLFile, "Project/Code", "", nGetCode.ToString());
                }


                //生成代码路径设置
                string strPath = Application.StartupPath;
                string strNewPath = strPath + "\\" + strProjectName;
                string strUtilityPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Utility";
                string strModelPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Model";
                string strDBSqlHelperPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".DBSqlHelper";
                string strDALPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".DAL";
                string strBLLPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".BLL";
                string strManagePath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Manage";//后台
                string strAPIPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".API";//RESTFUL API-APP接口文件
                string strAndroidModelsPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Android\\models";//Android实体类
                string strAndroidJavaPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Android\\java";//Android实体类
                string strAndroidLayoutPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Android\\layout";//Android实体类

                string striOSControllersPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".iOS\\Controllers";//iOS实体类
                string striOSViewsPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".iOS\\Views";//iOS View类
                string striOSModelsPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".iOS\\Models";//iOS Controller类
                string strWebPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Web";//官网
                string strWeChatPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".H5";//HTML5
                string strWeChatAppPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".WeChatApp";//小程序
                string strHTMLPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".HTML";//前端部门存放材料项目
                string strUIPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".UI";//设计部门存放材料项目
                string strPMPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".PM";//产品部门存放材料项目
                string strTestPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Test";//测试部门存放材料项目
                string strDataPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Data";//数据库备份项目
                string strSMSPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".SMS";//短信封装类
                string strPushPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Push";//消息推送封装类

                string strCorePath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Core";//.net core项目
                string strCoreDataPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Core\\Data";
                string strCoreViewsPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Core\\Views";
                string strCoreControllersPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Core\\Controllers";
                string strCoreWebApiPath = strPath + "\\" + strProjectName + "\\" + strProjectName + ".Core\\WebApi";

                string strTableName = tbTableName.Text.Trim();

                if (!Directory.Exists(strNewPath))
                {
                    Directory.CreateDirectory(strNewPath);
                    Directory.CreateDirectory(strUtilityPath);
                    Directory.CreateDirectory(strModelPath);
                    Directory.CreateDirectory(strDBSqlHelperPath);
                    Directory.CreateDirectory(strDALPath);
                    Directory.CreateDirectory(strBLLPath);
                    Directory.CreateDirectory(strManagePath);
                    Directory.CreateDirectory(strAPIPath);
                    Directory.CreateDirectory(strAndroidModelsPath);
                    Directory.CreateDirectory(strAndroidJavaPath);
                    Directory.CreateDirectory(strAndroidLayoutPath);
                    Directory.CreateDirectory(striOSControllersPath);
                    Directory.CreateDirectory(striOSViewsPath);
                    Directory.CreateDirectory(striOSModelsPath);
                    Directory.CreateDirectory(strWebPath);
                    Directory.CreateDirectory(strWeChatPath);
                    Directory.CreateDirectory(strWeChatAppPath);
                    Directory.CreateDirectory(strHTMLPath);
                    Directory.CreateDirectory(strUIPath);
                    Directory.CreateDirectory(strPMPath);
                    Directory.CreateDirectory(strTestPath);
                    Directory.CreateDirectory(strDataPath);
                    Directory.CreateDirectory(strSMSPath);
                    Directory.CreateDirectory(strPushPath);

                    Directory.CreateDirectory(strCorePath);
                    Directory.CreateDirectory(strCoreDataPath);
                    Directory.CreateDirectory(strCoreViewsPath);
                    Directory.CreateDirectory(strCoreControllersPath);
                    Directory.CreateDirectory(strCoreWebApiPath);

                    //存储路径
                    iniFile.WriteValue("DIR", "PROJECT", strNewPath);
                    iniFile.WriteValue("DIR", "UTILITY", strUtilityPath);
                    iniFile.WriteValue("DIR", "MODEL", strModelPath);
                    iniFile.WriteValue("DIR", "DBSQLHELPER", strDBSqlHelperPath);
                    iniFile.WriteValue("DIR", "DAL", strDALPath);
                    iniFile.WriteValue("DIR", "BLL", strBLLPath);
                    iniFile.WriteValue("BASE", "PROJECT", strProjectName);
                    iniFile.WriteValue("DIR", "MANAGE", strManagePath);
                    iniFile.WriteValue("DIR", "API", strAPIPath);
                    iniFile.WriteValue("DIR", "WEB", strWebPath);
                    iniFile.WriteValue("DIR", "HTML5", strWeChatPath);
                    iniFile.WriteValue("DIR", "WeChatApp", strWeChatAppPath);
                    iniFile.WriteValue("DIR", "HTML", strHTMLPath);
                    iniFile.WriteValue("DIR", "UI", strUIPath);
                    iniFile.WriteValue("DIR", "PM", strPMPath);
                    iniFile.WriteValue("DIR", "TEST", strTestPath);
                    iniFile.WriteValue("DIR", "DATA", strDataPath);
                    iniFile.WriteValue("DIR", "SMS", strSMSPath);
                    iniFile.WriteValue("DIR", "PUSH", strPushPath);

                    iniFile.WriteValue("ANDROID", "MODEL", strAndroidModelsPath);
                    iniFile.WriteValue("ANDROID", "JAVA", strAndroidJavaPath);
                    iniFile.WriteValue("ANDROID", "LAYOUT", strAndroidLayoutPath);
                    iniFile.WriteValue("IOS", "MODELS", striOSModelsPath);
                    iniFile.WriteValue("IOS", "VIEWS", striOSViewsPath);
                    iniFile.WriteValue("IOS", "CONTROLLERS", striOSControllersPath);

                    iniFile.WriteValue("CORE", "MAIN", strCorePath);
                    iniFile.WriteValue("CORE", "DATA", strCoreDataPath);
                    iniFile.WriteValue("CORE", "VIEWS", strCoreViewsPath);
                    iniFile.WriteValue("CORE", "CONTROLLERS", strCoreControllersPath);
                    iniFile.WriteValue("CORE", "WEBAPI", strCoreWebApiPath);

                    iniFile.WriteValue("BASE", "TABLENAME", strTableName);
                }

                //生成代码
                string strDecryptDBName = strDBName;// CodeRobot.Utility.CryptographyHelper.Decrypt(strDBName, CodeRobot.Utility.CommonHelper.cryptographyKey);
                CodeRobot.DAL.CodeHelper table = new DAL.CodeHelper();
                table.GetTables(strDecryptDBName, showMsg);

                CodeRobot.Utility.PublicValue.SaveMessage("\r\n" + Convert.ToDateTime(DateTime.Now).ToString("MM-dd HH:mm:ss") + " 任务结束!");
                this.SetText(CodeRobot.Utility.PublicValue.GetMessage());

                StopTask();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "执行任务", "DoTask", false);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopTask();
        }

        private void StopTask()
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            catchThread.Abort();
        }

        private void btnDocument_Click(object sender, EventArgs e)
        {
            btnDocument.Enabled = false;
            CodeRobot.Utility.PublicValue.SaveMessage(Convert.ToDateTime(DateTime.Now).ToString("MM-dd HH:mm:ss") + " 开始生成文档...");
            this.SetText(CodeRobot.Utility.PublicValue.GetMessage());

            //数据库名
            string strDBName = txtDBName.Text.Trim();
            CodeRobot.DAL.CodeHelper.GetDataDesignForTableList(strDBName);

            CodeRobot.Utility.PublicValue.SaveMessage("\r\n" + Convert.ToDateTime(DateTime.Now).ToString("MM-dd HH:mm:ss") + " 生成文档结束!");
            this.SetText(CodeRobot.Utility.PublicValue.GetMessage());
            btnDocument.Enabled = true;
        }

        private void cbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //选择的项目
            string strSelectProjectName = cbList.SelectedItem.ToString();
            //根据项目名称读取对应的XML存储文件数据
            string strXMLFile = Application.StartupPath + "\\xml\\" + strSelectProjectName + ".xml";
            string strProjectName = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/ProjectName", "");
            string strCreateDate = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/CreateDate", "");
            string strFrameworkVersion = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/FrameworkVersion", "");
            string strVersion = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/Version", "");
            string strCode = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/Code", "");
            string strServer = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/Server", "");
            string strDBName = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/DBName", "");
            string strDBUser = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/DBUser", "");
            string strDBPassword = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/DBPassword", "");
            string strCharset = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/Charset", "");
            string strCompany = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/Company", "");
            string strAuthor = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/Author", "");
            string strGUIDSolutionFramework = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDSolutionFramework", "");
            string strGUIDSolutionWeb = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDSolutionWeb", "");
            string strGUIDDBSQLHelper = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDDBSQLHelper", "");
            string strGUIDModel = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDModel", "");
            string strGUIDDAL = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDDAL", "");
            string strGUIDBLL = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDBLL", "");
            string strGUIDUtility = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDUtility", "");
            string strGUIDSMS = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDSMS", "");
            string strGUIDPush = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDPush", "");
            string strGUIDPay = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDPay", "");
            string strGUIDUI = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDUI", "");
            string strGUIDHTML = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDHTML", "");
            string strGUIDPM = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDPM", "");
            string strGUIDTest = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDTest", "");
            string strGUIDH5 = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDH5", "");
            string strGUIDData = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDData", "");
            string strGUIDManage = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDManage", "");
            string strGUIDWeb = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDWeb", "");
            string strGUIDWeChatApp = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/GUIDWeChatApp", "");
            string strAndroidPackage = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/iOSPackage", "");
            string striOSPackage = CodeRobot.Utility.XMLHelper.Read(strXMLFile, "Project/AndroidPackage", "");

            //解密敏感数据
            string strGetServer = strServer;
            string strGetDBName = strDBName;
            string strGetDBUser = strDBUser;
            string strGetDBPassword = strDBPassword;
            //try
            //{
            //    strGetServer = CodeRobot.Utility.CryptographyHelper.Decrypt(strServer, CodeRobot.Utility.CommonHelper.cryptographyKey);
            //}
            //catch (Exception ex)
            //{
            //    CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "MainFrm_Load", "解密", false);
            //}

            //try
            //{
            //    strGetDBName = CodeRobot.Utility.CryptographyHelper.Decrypt(strDBName, CodeRobot.Utility.CommonHelper.cryptographyKey);
            //}
            //catch (Exception ex)
            //{
            //    CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "MainFrm_Load", "解密", false);
            //}

            //try
            //{
            //    strGetDBUser = CodeRobot.Utility.CryptographyHelper.Decrypt(strDBUser, CodeRobot.Utility.CommonHelper.cryptographyKey);
            //}
            //catch (Exception ex)
            //{
            //    CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "MainFrm_Load", "解密", false);
            //}

            //try
            //{
            //    strGetDBPassword = CodeRobot.Utility.CryptographyHelper.Decrypt(strDBPassword, CodeRobot.Utility.CommonHelper.cryptographyKey);
            //}
            //catch (Exception ex)
            //{
            //    CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "MainFrm_Load", "解密", false);
            //}

            txtServer.Text = strGetServer;//数据库地址
            txtDBName.Text = strGetDBName;//数据库名
            txtUserID.Text = strGetDBUser;//登录数据库用户名
            txtPassword.Text = strGetDBPassword;//登录数据库密码
            txtCharset.Text = strCharset;//数据库字符集
            txtProjectName.Text = strProjectName;//项目命名
            txtAndroidPackage.Text = strAndroidPackage;//安卓包名
            txtiOSPackage.Text = striOSPackage;//iOS包名

            //根据每个项目的值写入Config.ini
            iniFile.WriteValue("BASE", "SERVER", strServer);
            iniFile.WriteValue("BASE", "DBNAME", strDBName);
            iniFile.WriteValue("BASE", "USERID", strDBUser);
            iniFile.WriteValue("BASE", "PASSWORD", strDBPassword);
            iniFile.WriteValue("BASE", "CHARSET", strCharset);
            iniFile.WriteValue("BASE", "PROJECT", strProjectName);
            iniFile.WriteValue("BASE", "CREATE_DATE", strCreateDate);
            iniFile.WriteValue("BASE", "FRAMEWORK", strFrameworkVersion);

            iniFile.WriteValue("ANDROID", "PACKAGE", strAndroidPackage);
            iniFile.WriteValue("GUID", "SOLUTION_FRAMEWORK", strGUIDSolutionFramework);
            iniFile.WriteValue("GUID", "SOLUTION_WEB", strGUIDSolutionWeb);
            iniFile.WriteValue("GUID", "DBSQLHELPER", strGUIDDBSQLHelper);
            iniFile.WriteValue("GUID", "MODEL", strGUIDModel);
            iniFile.WriteValue("GUID", "DAL", strGUIDDAL);
            iniFile.WriteValue("GUID", "BLL", strGUIDBLL);
            iniFile.WriteValue("GUID", "UTILITY", strGUIDUtility);
            iniFile.WriteValue("GUID", "SMS", strGUIDSMS);
            iniFile.WriteValue("GUID", "PUSH", strGUIDPush);
            iniFile.WriteValue("GUID", "PAY", strGUIDPay);
            iniFile.WriteValue("GUID", "UI", strGUIDUI);
            iniFile.WriteValue("GUID", "HTML", strGUIDHTML);
            iniFile.WriteValue("GUID", "PM", strGUIDPM);
            iniFile.WriteValue("GUID", "TEST", strGUIDTest);
            iniFile.WriteValue("GUID", "DATA", strGUIDData);
            iniFile.WriteValue("GUID", "H5", strGUIDH5);
            iniFile.WriteValue("GUID", "MANAGE", strGUIDManage);
            iniFile.WriteValue("GUID", "WEB", strGUIDWeb);
            iniFile.WriteValue("GUID", "WeChatApp", strGUIDWeChatApp);

            iniFile.WriteValue("COPYRIGHT", "COMPANY", strCompany);
            iniFile.WriteValue("COPYRIGHT", "AUTHOR", strAuthor);
            iniFile.WriteValue("COPYRIGHT", "VERSION", strVersion);
            iniFile.WriteValue("COPYRIGHT", "CODE", strCode);
            
        }


        /// <summary>
        /// 为每个项目创建一个XML本地文件存储相关信息
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strServer"></param>
        /// <param name="strDBName"></param>
        /// <param name="strDBUser"></param>
        /// <param name="strDBPassword"></param>
        /// <param name="strCharset"></param>
        public void CreateXmlFile(string strProjectName,string strFilePath,string strServer,string strDBName,string strDBUser,string strDBPassword,string strCharset,string striOSPackage,string strAndroidPackage)
        {
            try
            {
                string strGUIDSolutionFramework = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";//每台电脑生成的不一样，固定值
                string strGUIDSolutionWeb = "E24C65DC-7377-472B-9ABA-BC803B73C61A";//每台电脑生成的不一样，固定值
                string strGUIDDBSqlHelper = System.Guid.NewGuid().ToString("D");
                string strGUIDModel = System.Guid.NewGuid().ToString("D");
                string strGUIDDAL = System.Guid.NewGuid().ToString("D");
                string strGUIDBLL = System.Guid.NewGuid().ToString("D");
                string strGUIDUtility = System.Guid.NewGuid().ToString("D");
                string strGUIDSMS = System.Guid.NewGuid().ToString("D");
                string strGUIDPush = System.Guid.NewGuid().ToString("D");
                string strGUIDPay = System.Guid.NewGuid().ToString("D");

                string strGUIDUI = System.Guid.NewGuid().ToString("D");
                string strGUIDHTML = System.Guid.NewGuid().ToString("D");
                string strGUIDPM = System.Guid.NewGuid().ToString("D");
                string strGUIDTest = System.Guid.NewGuid().ToString("D");
                string strGUIDData = System.Guid.NewGuid().ToString("D");
                string strGUIDH5 = System.Guid.NewGuid().ToString("D");
                string strGUIDManage = System.Guid.NewGuid().ToString("D");
                string strGUIDWeb = System.Guid.NewGuid().ToString("D");
                string strGUIDWeChatApp = System.Guid.NewGuid().ToString("D");

                XmlDocument xmlDoc = new XmlDocument();
                //创建类型声明节点  
                XmlNode node = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
                xmlDoc.AppendChild(node);
                //创建根节点  
                XmlNode root = xmlDoc.CreateElement("Project");
                xmlDoc.AppendChild(root);
                //创建子节点
                CreateNode(xmlDoc, root, "ProjectName", strProjectName);
                CreateNode(xmlDoc, root, "CreateDate", DateTime.Now.ToString("yyyy-MM-dd"));
                CreateNode(xmlDoc, root, "FrameworkVersion", "4.5.2");//根据项目自行修改
                CreateNode(xmlDoc, root, "Version", "1.0");//根据项目自行修改
                CreateNode(xmlDoc, root, "Code", "1");//根据项目自行修改
                CreateNode(xmlDoc, root, "Server", strServer);
                CreateNode(xmlDoc, root, "DBName", strDBName);
                CreateNode(xmlDoc, root, "DBUser", strDBUser);
                CreateNode(xmlDoc, root, "DBPassword", strDBPassword);
                CreateNode(xmlDoc, root, "Charset", strCharset);
                CreateNode(xmlDoc, root, "Company", "Cloudin");//根据项目自行修改
                CreateNode(xmlDoc, root, "Author", "Cloudin");//根据项目自行修改
                CreateNode(xmlDoc, root, "GUIDSolutionFramework", strGUIDSolutionFramework);
                CreateNode(xmlDoc, root, "GUIDSolutionWeb", strGUIDSolutionWeb);
                CreateNode(xmlDoc, root, "GUIDDBSQLHelper", strGUIDDBSqlHelper);
                CreateNode(xmlDoc, root, "GUIDModel", strGUIDModel);
                CreateNode(xmlDoc, root, "GUIDDAL", strGUIDDAL);
                CreateNode(xmlDoc, root, "GUIDBLL", strGUIDBLL);
                CreateNode(xmlDoc, root, "GUIDUtility", strGUIDUtility);
                CreateNode(xmlDoc, root, "GUIDSMS", strGUIDSMS);
                CreateNode(xmlDoc, root, "GUIDPush", strGUIDPush);
                CreateNode(xmlDoc, root, "GUIDPay", strGUIDPay);
                CreateNode(xmlDoc, root, "GUIDUI", strGUIDUI);
                CreateNode(xmlDoc, root, "GUIDHTML", strGUIDHTML);
                CreateNode(xmlDoc, root, "GUIDPM", strGUIDPM);
                CreateNode(xmlDoc, root, "GUIDTest", strGUIDTest);
                CreateNode(xmlDoc, root, "GUIDData", strGUIDData);
                CreateNode(xmlDoc, root, "GUIDH5", strGUIDH5);
                CreateNode(xmlDoc, root, "GUIDManage", strGUIDManage);
                CreateNode(xmlDoc, root, "GUIDWeb", strGUIDWeb);
                CreateNode(xmlDoc, root, "GUIDWeChatApp", strGUIDWeChatApp);
                CreateNode(xmlDoc, root, "iOSPackage", striOSPackage);
                CreateNode(xmlDoc, root, "AndroidPackage", strAndroidPackage);

                try
                {
                    xmlDoc.Save(strFilePath);
                }
                catch (Exception ex)
                {
                    CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "创建XML", "CreateXmlFile", false);
                }

                //存储到Config.ini文件
                iniFile.WriteValue("GUID", "SOLUTION_FRAMEWORK", strGUIDSolutionFramework);
                iniFile.WriteValue("GUID", "SOLUTION_WEB", strGUIDSolutionWeb);
                iniFile.WriteValue("GUID", "DBSQLHELPER", strGUIDDBSqlHelper);
                iniFile.WriteValue("GUID", "MODEL", strGUIDModel);
                iniFile.WriteValue("GUID", "DAL", strGUIDDAL);
                iniFile.WriteValue("GUID", "BLL", strGUIDBLL);
                iniFile.WriteValue("GUID", "UTILITY", strGUIDUtility);
                iniFile.WriteValue("GUID", "SMS", strGUIDSMS);
                iniFile.WriteValue("GUID", "PUSH", strGUIDPush);
                iniFile.WriteValue("GUID", "PAY", strGUIDPay);
                iniFile.WriteValue("GUID", "UI", strGUIDUI);
                iniFile.WriteValue("GUID", "HTML", strGUIDHTML);
                iniFile.WriteValue("GUID", "PM", strGUIDPM);
                iniFile.WriteValue("GUID", "TEST", strGUIDTest);
                iniFile.WriteValue("GUID", "DATA", strGUIDData);
                iniFile.WriteValue("GUID", "H5", strGUIDH5);
                iniFile.WriteValue("GUID", "MANAGE", strGUIDManage);
                iniFile.WriteValue("GUID", "WEB", strGUIDWeb);
                iniFile.WriteValue("GUID", "WECHATAPP", strGUIDWeChatApp);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(MainFrm), ex, "为每个项目创建一个XML本地文件存储相关信息", "CreateXmlFile", false);
            }

        }

        /// <summary>    
        /// 创建节点    
        /// </summary>    
        /// <param name="xmldoc"></param>  xml文档  
        /// <param name="parentnode"></param>父节点    
        /// <param name="name"></param>  节点名  
        /// <param name="value"></param>  节点值  
        ///   
        public void CreateNode(XmlDocument xmlDoc, XmlNode parentNode, string name, string value)
        {
            XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, name, null);
            node.InnerText = value;
            parentNode.AppendChild(node);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            TestFrm newFrm = new TestFrm();
            newFrm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strNewLine = "";
            string strPath = Application.StartupPath + "\\template\\core\\Create.html";
            using (StreamReader sReader = new StreamReader(strPath, Encoding.UTF8))
            {
                string aLine;
                bool condition = true;

                while (true)
                {
                    aLine = sReader.ReadLine();
                    if (aLine == null)
                    {
                        condition = false;
                    }

                    if (condition)
                    {
                        aLine = aLine.Replace("\"","\\\"");
                        strNewLine += "sw.WriteLine(\""+ aLine + "\");\r\n";
                    }
                    else
                    {
                        break;
                    }
                }

            }


            showMsg.Text = strNewLine;
        }
    }
}
