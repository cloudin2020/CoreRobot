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
    /// Java Controller View类
    /// </summary>
    public class JavaControllersViewHelper
    {

        /// <summary>
        /// 根据表名创建Controllers类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateControllersViewClass(string strFilePath,string strProjectName, string strTableName,string strTableComment)
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

            string strNewPath = strFilePath + "\\" + strTableNameLower + "\\controller";
            if (!Directory.Exists(strNewPath))
            {
                Directory.CreateDirectory(strNewPath);
            }
            
            StreamWriter sw = new StreamWriter(strNewPath + "\\ViewController.java", false, Encoding.GetEncoding("utf-8"));

            sw.WriteLine("package cc.mrbird.febs."+ strTableNameLower + ".controller;");
            sw.WriteLine("");
            sw.WriteLine("import org.apache.shiro.authz.annotation.RequiresPermissions;");
            sw.WriteLine("import org.springframework.beans.factory.annotation.Autowired;");
            sw.WriteLine("import org.springframework.stereotype.Controller;");
            sw.WriteLine("import org.springframework.ui.Model;");
            sw.WriteLine("import org.springframework.web.bind.annotation.GetMapping;");
            sw.WriteLine("import org.springframework.web.bind.annotation.PathVariable;");
            sw.WriteLine("");
            sw.WriteLine("import cc.mrbird.febs.common.controller.BaseController;");
            sw.WriteLine("import cc.mrbird.febs.common.entity.FebsConstant;");
            sw.WriteLine("import cc.mrbird.febs.common.utils.FebsUtil;");
            sw.WriteLine("import cc.mrbird.febs." + strTableNameLower + ".entity."+ strClassName + ";");
            sw.WriteLine("import cc.mrbird.febs." + strTableNameLower + ".service.I" + strClassName + "Service;");
            sw.WriteLine("");
            sw.WriteLine("@Controller(\"" + strTableNameLower + "View\")");
            sw.WriteLine("public class ViewController extends BaseController {");
            sw.WriteLine("");
            sw.WriteLine("	@Autowired");
            sw.WriteLine("	private I" + strClassName + "Service" + strTableNameLower + "Service;");
            sw.WriteLine("");
            sw.WriteLine("	@GetMapping(FebsConstant.VIEW_PREFIX + \"" + strTableNameLower + "/list\")");
            sw.WriteLine("	//@RequiresPermissions(\"" + strTableNameLower + ":list\")");
            sw.WriteLine("	public String List() {");
            sw.WriteLine("		return FebsUtil.view(\"" + strTableNameLower + "/list\");");
            sw.WriteLine("	}");
            sw.WriteLine("");
            sw.WriteLine("	@GetMapping(FebsConstant.VIEW_PREFIX + \"" + strTableNameLower + "/create\")");
            sw.WriteLine("	//@RequiresPermissions(\"" + strTableNameLower + ":create\")");
            sw.WriteLine("	public String Create() {");
            sw.WriteLine("		return FebsUtil.view(\"" + strTableNameLower + "/create\");");
            sw.WriteLine("	}");
            sw.WriteLine("");
            sw.WriteLine("	@GetMapping(FebsConstant.VIEW_PREFIX + \"" + strTableNameLower + "/view/{id}\")");
            sw.WriteLine("	//@RequiresPermissions(\"" + strTableNameLower + ":view\")");
            sw.WriteLine("	public String View(@PathVariable Long id, Model model) {");
            sw.WriteLine("		" + strClassName + " " + strTableNameLower + " = " + strTableNameLower + "Service.findById(id);");
            sw.WriteLine("		model.addAttribute(\"" + strTableNameLower + "\", " + strTableNameLower + ");");
            sw.WriteLine("		return FebsUtil.view(\"" + strTableNameLower + "/view\");");
            sw.WriteLine("	}");
            sw.WriteLine("");
            sw.WriteLine("	@GetMapping(FebsConstant.VIEW_PREFIX + \"" + strTableNameLower + "/edit/{id}\")");
            sw.WriteLine("	//@RequiresPermissions(\"" + strTableNameLower + ":edit\")");
            sw.WriteLine("	public String Edit(@PathVariable Long id, Model model) {");
            sw.WriteLine("		" + strClassName + " " + strTableNameLower + " = " + strTableNameLower + "Service.findById(id);");
            sw.WriteLine("		model.addAttribute(\"" + strTableNameLower + "\", " + strTableNameLower + ");");
            sw.WriteLine("		return FebsUtil.view(\"" + strTableNameLower + "/edit\");");
            sw.WriteLine("	}");
            sw.WriteLine("");
            sw.WriteLine("}");


            sw.Close();

        }

        
    }
}
