using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace CodeRobot.DAL
{
    public class AndroidHelper
    {
        /// <summary>
        /// 生成安卓客户端 Model实体类
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strProjectName"></param>
        /// <param name="strTableName"></param>
        /// <param name="strPackage"></param>
        public static void CreateAndroidModelClass(string strFilePath, string strProjectName, string strTableName, string strPackage)
        {
            //读取版权信息
            CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
            string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
            string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
            string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
            string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
            string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");


            string strClassName = CommonHelper.GetClassName(strTableName);//类名
            strClassName = CommonHelper.GetTableNameUpper(strClassName);

            var utf8WithoutBom = new System.Text.UTF8Encoding(false);
            Directory.CreateDirectory(strFilePath);
            StreamWriter sw = new StreamWriter(strFilePath + "\\" + strClassName + "Object.java", false, utf8WithoutBom);
            sw.WriteLine("package " + strPackage + ".model;");
            sw.WriteLine("\r\n");
            sw.WriteLine("import org.json.JSONObject;");
            sw.WriteLine("import java.io.Serializable;");
            sw.WriteLine("\r\n");
            sw.WriteLine("/**");
            sw.WriteLine(" * " + CommonHelper.GetTableComment(strTableName));
            sw.WriteLine(" * Author: " + strAuthor + ".");
            sw.WriteLine(" * CreateDate: " + strCreateDate + ".");
            sw.WriteLine(" * UpdateDate: " + DateTime.Now.ToString("yyyy年M月d日") + ".");
            sw.WriteLine(" * Copyright (c) " + DateTime.Now.Year + " "+ strCompany + ". All rights reserved.");
            sw.WriteLine(" */");
            sw.WriteLine("public class " + strClassName + "Object implements Serializable{");
            sw.WriteLine("\r\n");
            sw.WriteLine(GetDefineParaList(strTableName) + "\r\n");
            sw.WriteLine("    public " + strClassName + "Object(JSONObject json) {");
            sw.WriteLine("\r\n");
            sw.WriteLine(GetJSONParaList(strTableName) + "\r\n");
            sw.WriteLine("    }");
            sw.WriteLine("    ");
            sw.WriteLine("    public " + strClassName + "Object() {");
            sw.WriteLine("\r\n");
            sw.WriteLine("    }");
            sw.WriteLine("}");
            sw.Close();

        }


        /// <summary>
        /// 生成Android客户端 Java文件
        /// </summary>
        /// <param name="strFilePath">保存文件路径</param>
        /// <param name="strProjectName">项目名称</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strPackage">包名</param>
        public static void CreateJavaClass(string strFilePath, string strProjectName, string strTableName, string strPackage)
        {
            try
            {
                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetTableNameFirtWordLower(strTableName);//如：news,newstype

                Directory.CreateDirectory(strFilePath);
                var utf8WithoutBom = new System.Text.UTF8Encoding(false);//Android Studio必须是UTF8无Bom编码格式

                //***ListActivity.java
                //读取模板内容
                FileStream fsListActivity = new FileStream(Application.StartupPath + "\\template\\android\\ListActivity.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader srListActivity = new StreamReader(fsListActivity, Encoding.UTF8);
                string strGetPageHTMLContentForListActivity = srListActivity.ReadToEnd();
                srListActivity.Close();
                fsListActivity.Close();

                strGetPageHTMLContentForListActivity = ReplaceHTMLDataForListActivity(strGetPageHTMLContentForListActivity, strTableName, strProjectName,strPackage);
                StreamWriter swListActivity = new StreamWriter(strFilePath + "\\" + strClassName + "ListActivity.java", false, utf8WithoutBom);
                swListActivity.WriteLine(strGetPageHTMLContentForListActivity);
                swListActivity.Close();

                //***ListFragment.java
                //读取模板内容
                FileStream fsListFragment = new FileStream(Application.StartupPath + "\\template\\android\\ListFragment.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader srListFragment = new StreamReader(fsListFragment, Encoding.UTF8);
                string strGetPageHTMLContentForListFragment = srListFragment.ReadToEnd();
                srListFragment.Close();
                fsListFragment.Close();

                strGetPageHTMLContentForListFragment = ReplaceHTMLDataForListFragment(strGetPageHTMLContentForListFragment, strTableName, strProjectName, strPackage);
                StreamWriter swListFragment = new StreamWriter(strFilePath + "\\" + strClassName + "ListFragment.java", false, utf8WithoutBom);
                swListFragment.WriteLine(strGetPageHTMLContentForListFragment);
                swListFragment.Close();

                //***ListBaseFragment.java
                //读取模板内容
                FileStream fsListBaseFragment = new FileStream(Application.StartupPath + "\\template\\android\\ListBaseFragment.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader srListBaseFragment = new StreamReader(fsListBaseFragment, Encoding.UTF8);
                string strGetPageHTMLContentForListBaseFragment = srListBaseFragment.ReadToEnd();
                srListBaseFragment.Close();
                fsListBaseFragment.Close();

                strGetPageHTMLContentForListBaseFragment = ReplaceHTMLDataForListBaseFragment(strGetPageHTMLContentForListBaseFragment, strTableName, strProjectName, strPackage);
                StreamWriter swListBaseFragment = new StreamWriter(strFilePath + "\\" + strClassName + "ListBaseFragment.java", false, utf8WithoutBom);
                swListBaseFragment.WriteLine(strGetPageHTMLContentForListBaseFragment);
                swListBaseFragment.Close();

                //***DetailActivity.java
                //读取模板内容
                FileStream fsDetailActivity = new FileStream(Application.StartupPath + "\\template\\android\\DetailActivity.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader srDetailActivity = new StreamReader(fsDetailActivity, Encoding.UTF8);
                string strGetPageHTMLContentForDetailActivity = srDetailActivity.ReadToEnd();
                srDetailActivity.Close();
                fsDetailActivity.Close();

                strGetPageHTMLContentForDetailActivity = ReplaceHTMLDataForDetailActivity(strGetPageHTMLContentForDetailActivity, strTableName, strProjectName, strPackage);
                StreamWriter swDetailActivity = new StreamWriter(strFilePath + "\\" + strClassName + "DetailActivity.java", false, utf8WithoutBom);
                swDetailActivity.WriteLine(strGetPageHTMLContentForDetailActivity);
                swDetailActivity.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ModelHelper), ex, "生成iOS客户端 Controllers", "CreateJavaClass",false);
            }
        }

        /// <summary>
        /// 生成Android客户端 XML文件
        /// </summary>
        /// <param name="strFilePath">保存文件路径</param>
        /// <param name="strProjectName">项目名称</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strPackage">包名</param>
        public static void CreateXMLFile(string strFilePath, string strProjectName, string strTableName, string strPackage)
        {
            try
            {
                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetClassName(strTableName).ToLower();//如：news,newstype

                Directory.CreateDirectory(strFilePath);
                var utf8WithoutBom = new System.Text.UTF8Encoding(false);//Android Studio必须是UTF8无Bom编码格式

                //***ActivityList.java
                //读取模板内容
                FileStream fsActivityList = new FileStream(Application.StartupPath + "\\template\\android\\activity_list.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader srActivityList = new StreamReader(fsActivityList, Encoding.UTF8);
                string strGetPageHTMLContentForActivityList = srActivityList.ReadToEnd();
                srActivityList.Close();
                fsActivityList.Close();

                strGetPageHTMLContentForActivityList = ReplaceHTMLDataForActivityListXML(strGetPageHTMLContentForActivityList, strTableName, strProjectName, strPackage);
                StreamWriter swActivityList = new StreamWriter(strFilePath + "\\activity_" + strTableNameLower + "_list.xml", false, utf8WithoutBom);
                swActivityList.WriteLine(strGetPageHTMLContentForActivityList);
                swActivityList.Close();

                //***FragmentList.java
                //读取模板内容
                FileStream fsFragmentList = new FileStream(Application.StartupPath + "\\template\\android\\fragment_list.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader srFragmentList = new StreamReader(fsFragmentList, Encoding.UTF8);
                string strGetPageHTMLContentForFragmentList = srFragmentList.ReadToEnd();
                srFragmentList.Close();
                fsFragmentList.Close();

                strGetPageHTMLContentForFragmentList = ReplaceHTMLDataForFragmentListXML(strGetPageHTMLContentForFragmentList, strTableName, strProjectName, strPackage);
                StreamWriter swFragmentList = new StreamWriter(strFilePath + "\\fragment_" + strTableNameLower + "_list.xml", false, utf8WithoutBom);
                swFragmentList.WriteLine(strGetPageHTMLContentForFragmentList);
                swFragmentList.Close();

                //***FragmentListItem.java
                //读取模板内容
                FileStream fsFragmentListItem = new FileStream(Application.StartupPath + "\\template\\android\\fragment_list_item.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader srFragmentListItem = new StreamReader(fsFragmentListItem, Encoding.UTF8);
                string strGetPageHTMLContentForFragmentListItem = srFragmentListItem.ReadToEnd();
                srFragmentListItem.Close();
                fsFragmentListItem.Close();

                strGetPageHTMLContentForFragmentListItem = ReplaceHTMLDataForFragmentListItemXML(strGetPageHTMLContentForFragmentListItem, strTableName, strProjectName, strPackage);
                StreamWriter swFragmentListItem = new StreamWriter(strFilePath + "\\fragment_" + strTableNameLower + "_list_item.xml", false, utf8WithoutBom);
                swFragmentListItem.WriteLine(strGetPageHTMLContentForFragmentListItem);
                swFragmentListItem.Close();

                //***ActivityDetail.java
                //读取模板内容
                FileStream fsActivityDetail = new FileStream(Application.StartupPath + "\\template\\android\\activity_detail.html", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader srActivityDetail = new StreamReader(fsActivityDetail, Encoding.UTF8);
                string strGetPageHTMLContentForActivityDetail = srActivityDetail.ReadToEnd();
                srActivityDetail.Close();
                fsActivityDetail.Close();

                strGetPageHTMLContentForActivityDetail = ReplaceHTMLDataForActivityDetailXML(strGetPageHTMLContentForActivityDetail, strTableName, strProjectName, strPackage);
                StreamWriter swActivityDetail = new StreamWriter(strFilePath + "\\activity_" + strTableNameLower + "_detail.xml", false, utf8WithoutBom);
                swActivityDetail.WriteLine(strGetPageHTMLContentForActivityDetail);
                swActivityDetail.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ModelHelper), ex, "生成Android客户端 XML文件", "CreateXMLFile",false);
            }
        }


        /// <summary>
        /// 读取DetailViewController模板内容，并替换成对应的表数据
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strTableName"></param>
        /// <param name="strProjectName"></param>
        /// <returns></returns>
        public static string ReplaceHTMLDataForListActivity(string strContent, string strTableName, string strProjectName,string strPackage)
        {
            string strReturnValue = "";

            try
            {
                //读取版权信息
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
                string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
                string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
                string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
                string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
                string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

                //获取主键ID
                string strPrimaryID = CommonHelper.GetPrimaryKey(strTableName);
                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetTableNameFirtWordLower(strTableName);//如：news,newstype

                //替换文件名-1
                strContent = strContent.Replace("{packagename}", strPackage);
                strContent = strContent.Replace("{classname_uper}", strClassName);
                strContent = strContent.Replace("{classname_lower}", strTableNameLower);
                strContent = strContent.Replace("{projectname}", strProjectName);
                strContent = strContent.Replace("{tablecomment}", CommonHelper.GetTableComment(strTableName));
                strContent = strContent.Replace("{author}", strAuthor);
                strContent = strContent.Replace("{date}", DateTime.Now.ToString("yyyy/M/d"));
                strContent = strContent.Replace("{year}", DateTime.Now.Year.ToString());
                strContent = strContent.Replace("{company}", strCompany);
                strContent = strContent.Replace("{primaryid}", strPrimaryID);

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "ReplaceHTMLDataForListActivity",false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 设置ListFragment页面模板内容
        /// </summary>
        /// <param name="strContent">模板内容</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strPackage">包名</param>
        /// <returns></returns>
        public static string ReplaceHTMLDataForListFragment(string strContent, string strTableName, string strProjectName, string strPackage)
        {
            string strReturnValue = "";

            try
            {
                //读取版权信息
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
                string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
                string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
                string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
                string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
                string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

                //获取主键ID
                string strPrimaryID = CommonHelper.GetPrimaryKey(strTableName);
                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetClassName(strTableName).ToLower();//如：news,newstype

                //替换文件名-1
                strContent = strContent.Replace("{packagename}", strPackage);
                strContent = strContent.Replace("{classname_uper}", strClassName);
                strContent = strContent.Replace("{classname_uper_all}", strClassName.ToUpper());
                strContent = strContent.Replace("{classname_lower}", strTableNameLower);
                strContent = strContent.Replace("{projectname}", strProjectName);
                strContent = strContent.Replace("{tablecomment}", CommonHelper.GetTableComment(strTableName));
                strContent = strContent.Replace("{author}", strAuthor);
                strContent = strContent.Replace("{date}", DateTime.Now.ToString("yyyy/M/d"));
                strContent = strContent.Replace("{year}", DateTime.Now.Year.ToString());
                strContent = strContent.Replace("{company}", strCompany);
                strContent = strContent.Replace("{primaryid}", strPrimaryID);

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "ReplaceHTMLDataForListActivity",false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 设置ListBaseFragment页面模板内容
        /// </summary>
        /// <param name="strContent">模板内容</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strPackage">包名</param>
        /// <returns></returns>
        public static string ReplaceHTMLDataForListBaseFragment(string strContent, string strTableName, string strProjectName, string strPackage)
        {
            string strReturnValue = "";

            try
            {
                //读取版权信息
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
                string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
                string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
                string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
                string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
                string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

                //获取主键ID
                string strPrimaryID = CommonHelper.GetPrimaryKey(strTableName);
                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetClassName(strTableName).ToLower();//如：news,newstype

                //替换文件名-1
                strContent = strContent.Replace("{packagename}", strPackage);
                strContent = strContent.Replace("{classname_uper}", strClassName);
                strContent = strContent.Replace("{classname_lower}", strTableNameLower);
                strContent = strContent.Replace("{projectname}", strProjectName);
                strContent = strContent.Replace("{tablecomment}", CommonHelper.GetTableComment(strTableName));
                strContent = strContent.Replace("{author}", strAuthor);
                strContent = strContent.Replace("{date}", DateTime.Now.ToString("yyyy/M/d"));
                strContent = strContent.Replace("{year}", DateTime.Now.Year.ToString());
                strContent = strContent.Replace("{company}", strCompany);
                strContent = strContent.Replace("{primaryid}", strPrimaryID);

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "ReplaceHTMLDataForListActivity",false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 设置DetailActivity页面模板内容
        /// </summary>
        /// <param name="strContent">模板内容</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strPackage">包名</param>
        /// <returns></returns>
        public static string ReplaceHTMLDataForDetailActivity(string strContent, string strTableName, string strProjectName, string strPackage)
        {
            string strReturnValue = "";

            try
            {
                //读取版权信息
                CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");
                string strCompany = iniFile.GetString("COPYRIGHT", "COMPANY", "");
                string strAuthor = iniFile.GetString("COPYRIGHT", "AUTHOR", "");
                string strVersion = iniFile.GetString("COPYRIGHT", "VERSION", "");
                string strCode = iniFile.GetString("COPYRIGHT", "CODE", "");
                string strCreateDate = iniFile.GetString("BASE", "CREATE_DATE", "");

                //获取主键ID
                string strPrimaryID = CommonHelper.GetPrimaryKey(strTableName);
                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetClassName(strTableName).ToLower();//如：news,newstype

                //替换文件名-1
                strContent = strContent.Replace("{packagename}", strPackage);
                strContent = strContent.Replace("{classname_uper}", strClassName);
                strContent = strContent.Replace("{classname_uper_all}", strClassName.ToUpper());
                strContent = strContent.Replace("{classname_lower}", strTableNameLower);
                strContent = strContent.Replace("{projectname}", strProjectName);
                strContent = strContent.Replace("{tablecomment}", CommonHelper.GetTableComment(strTableName));
                strContent = strContent.Replace("{author}", strAuthor);
                strContent = strContent.Replace("{date}", DateTime.Now.ToString("yyyy/M/d"));
                strContent = strContent.Replace("{year}", DateTime.Now.Year.ToString());
                strContent = strContent.Replace("{company}", strCompany);
                strContent = strContent.Replace("{primaryid}", strPrimaryID);

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "ReplaceHTMLDataForListActivity",false);
            }

            return strReturnValue;
        }


        /// <summary>
        /// 设置activity_list.xml页面模板内容
        /// </summary>
        /// <param name="strContent">模板内容</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strPackage">包名</param>
        /// <returns></returns>
        public static string ReplaceHTMLDataForActivityListXML(string strContent, string strTableName, string strProjectName, string strPackage)
        {
            string strReturnValue = "";

            try
            {
                //获取主键ID
                string strPrimaryID = CommonHelper.GetPrimaryKey(strTableName);
                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetClassName(strTableName).ToLower();//如：news,newstype

                //替换文件名-1
                //strContent = strContent.Replace("{packagename}", strPackage);
                //strContent = strContent.Replace("{classname_uper}", strClassName);
                //strContent = strContent.Replace("{classname_lower}", strTableNameLower);
                //strContent = strContent.Replace("{projectname}", strProjectName);
                strContent = strContent.Replace("{tablecomment}", CommonHelper.GetTableComment(strTableName));
                //strContent = strContent.Replace("{author}", strAuthor);
                //strContent = strContent.Replace("{date}", DateTime.Now.ToString("yyyy/M/d"));
                //strContent = strContent.Replace("{year}", DateTime.Now.Year.ToString());
                //strContent = strContent.Replace("{company}", strCompany);
                //strContent = strContent.Replace("{primaryid}", strPrimaryID);

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "ReplaceHTMLDataForListActivity",false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 设置activity_list.xml页面模板内容
        /// </summary>
        /// <param name="strContent">模板内容</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strPackage">包名</param>
        /// <returns></returns>
        public static string ReplaceHTMLDataForFragmentListXML(string strContent, string strTableName, string strProjectName, string strPackage)
        {
            string strReturnValue = "";

            try
            {
                //获取主键ID
                string strPrimaryID = CommonHelper.GetPrimaryKey(strTableName);
                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetClassName(strTableName).ToLower();//如：news,newstype

                //替换文件名-1
                //strContent = strContent.Replace("{packagename}", strPackage);
                //strContent = strContent.Replace("{classname_uper}", strClassName);
                //strContent = strContent.Replace("{classname_lower}", strTableNameLower);
                //strContent = strContent.Replace("{projectname}", strProjectName);
                strContent = strContent.Replace("{tablecomment}", CommonHelper.GetTableComment(strTableName));
                //strContent = strContent.Replace("{author}", strAuthor);
                //strContent = strContent.Replace("{date}", DateTime.Now.ToString("yyyy/M/d"));
                //strContent = strContent.Replace("{year}", DateTime.Now.Year.ToString());
                //strContent = strContent.Replace("{company}", strCompany);
                //strContent = strContent.Replace("{primaryid}", strPrimaryID);

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "ReplaceHTMLDataForListActivity",false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 设置activity_list.xml页面模板内容
        /// </summary>
        /// <param name="strContent">模板内容</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strPackage">包名</param>
        /// <returns></returns>
        public static string ReplaceHTMLDataForFragmentListItemXML(string strContent, string strTableName, string strProjectName, string strPackage)
        {
            string strReturnValue = "";

            try
            {
                //获取主键ID
                string strPrimaryID = CommonHelper.GetPrimaryKey(strTableName);
                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetClassName(strTableName).ToLower();//如：news,newstype

                //替换文件名-1
                //strContent = strContent.Replace("{packagename}", strPackage);
                //strContent = strContent.Replace("{classname_uper}", strClassName);
                //strContent = strContent.Replace("{classname_lower}", strTableNameLower);
                //strContent = strContent.Replace("{projectname}", strProjectName);
                strContent = strContent.Replace("{tablecomment}", CommonHelper.GetTableComment(strTableName));
                //strContent = strContent.Replace("{author}", strAuthor);
                //strContent = strContent.Replace("{date}", DateTime.Now.ToString("yyyy/M/d"));
                //strContent = strContent.Replace("{year}", DateTime.Now.Year.ToString());
                //strContent = strContent.Replace("{company}", strCompany);
                //strContent = strContent.Replace("{primaryid}", strPrimaryID);

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "ReplaceHTMLDataForListActivity",false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 设置activity_detail.xml页面模板内容
        /// </summary>
        /// <param name="strContent">模板内容</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strPackage">包名</param>
        /// <returns></returns>
        public static string ReplaceHTMLDataForActivityDetailXML(string strContent, string strTableName, string strProjectName, string strPackage)
        {
            string strReturnValue = "";

            try
            {
                //获取主键ID
                string strPrimaryID = CommonHelper.GetPrimaryKey(strTableName);
                string strClassName = CommonHelper.GetClassName(strTableName);//类名
                strClassName = CommonHelper.GetTableNameUpper(strClassName);//如：News,NewsType
                string strTableNameLower = CommonHelper.GetClassName(strTableName).ToLower();//如：news,newstype

                //替换文件名-1
                //strContent = strContent.Replace("{packagename}", strPackage);
                //strContent = strContent.Replace("{classname_uper}", strClassName);
                //strContent = strContent.Replace("{classname_lower}", strTableNameLower);
                //strContent = strContent.Replace("{projectname}", strProjectName);
                strContent = strContent.Replace("{tablecomment}", CommonHelper.GetTableComment(strTableName));
                //strContent = strContent.Replace("{author}", strAuthor);
                //strContent = strContent.Replace("{date}", DateTime.Now.ToString("yyyy/M/d"));
                //strContent = strContent.Replace("{year}", DateTime.Now.Year.ToString());
                //strContent = strContent.Replace("{company}", strCompany);
                //strContent = strContent.Replace("{primaryid}", strPrimaryID);

                strReturnValue = strContent;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ManageHelper), ex, "处理HTML代码", "ReplaceHTMLDataForListActivity",false);
            }

            return strReturnValue;
        }


        /// <summary>
        /// 获取定义参数列表
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetDefineParaList(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                int nNum = 0;
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT,DATA_TYPE FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnNameUpper = CommonHelper.GetTableNameUpper(strColumnName);//如：UserName
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    strColumnComment = CommonHelper.GetColumnKeyComment(strColumnComment);

                    string strDataType = dr["DATA_TYPE"].ToString();//数据类型
                    strDataType = CodeRobot.Utility.StringHelper.GetCSharpDBType(strDataType);

                    //获取数据类型
                    string strValue = "";
                    if (strDataType == "int")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "float")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "double")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = "long";
                    }
                    else if (strDataType == "string")
                    {
                        strValue = "String";
                    }
                    else
                    {
                        strValue = "String";
                    }

                    string strColumn = "";
                    if (strValue == "int")
                    {
                        strColumn = "    /**\r\n";
                        strColumn += "    "+ strColumnComment+ "\r\n";
                        strColumn += "    */\r\n";
                        strColumn += "    public int " + strColumnName + ";";
                    }
                    else if (strValue == "long")
                    {
                        strColumn = "    /**\r\n";
                        strColumn += "    " + strColumnComment + "\r\n";
                        strColumn += "    */\r\n";
                        strColumn += "    public long " + strColumnName + " = 0;";
                    }
                    else
                    {
                        strColumn = "    /**\r\n";
                        strColumn += "    " + strColumnComment + "\r\n";
                        strColumn += "    */\r\n";
                        strColumn += "    public String " + strColumnName + " = \"\";";
                    }

                    strReturnValue += strColumn + "\r\n";
                    nNum++;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ModelHelper), ex, "获取定义参数列表", "GetDefineParaList",false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取读取服务端JSON数据代码列表
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetJSONParaList(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                int nNum = 0;
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT,DATA_TYPE FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnNameUpper = CommonHelper.GetTableNameUpper(strColumnName);//如：UserName
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释
                    strColumnComment = CommonHelper.GetColumnKeyComment(strColumnComment);

                    string strDataType = dr["DATA_TYPE"].ToString();//数据类型
                    strDataType = CodeRobot.Utility.StringHelper.GetCSharpDBType(strDataType);

                    //获取数据类型
                    string strValue = "";
                    if (strDataType == "int")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "float")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "double")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "decimal")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "bool")
                    {
                        strValue = "int";
                    }
                    else if (strDataType == "DateTime")
                    {
                        strValue = "long";
                    }
                    else if (strDataType == "string")
                    {
                        strValue = "String";
                    }
                    else
                    {
                        strValue = "String";
                    }

                    string strColumn = "";
                    if (strValue == "int")
                    {
                        strColumn = "        " + strColumnName + " = json.optInt(\"" + strColumnName + "\");";
                    }
                    else if (strValue == "long")
                    {
                        strColumn = "        " + strColumnName + " = json.optLong(\"" + strColumnName + "\");";
                    }
                    else
                    {
                        strColumn = "        " + strColumnName + " = json.optString(\"" + strColumnName + "\");";
                    }

                    strReturnValue += strColumn + "\r\n";
                    nNum++;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ModelHelper), ex, "获取读取服务端JSON数据代码列表", "GetJSONParaList",false);
            }

            return strReturnValue;
        }
    }
}
