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
    public class JavaControllersHelper
    {

        /// <summary>
        /// 根据表名创建Controllers类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateControllersClass(string strFilePath,string strProjectName, string strTableName,string strTableComment)
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
            
            StreamWriter sw = new StreamWriter(strNewPath + "\\" + strClassName + "Controller.java", false, Encoding.GetEncoding("utf-8"));
            
            sw.WriteLine("/*");
            sw.WriteLine(" * Copyright "+ DateTime.Now.Year +" Cloudin.");
            sw.WriteLine(" *");
            sw.WriteLine(" * Licensed under the Apache License, Version 2.0 (the \"License\");");
            sw.WriteLine(" * you may not use this file except in compliance with the License.");
            sw.WriteLine(" * You may obtain a copy of the License at");
            sw.WriteLine(" *");
            sw.WriteLine(" *      http://www.apache.org/licenses/LICENSE-2.0");
            sw.WriteLine(" *");
            sw.WriteLine(" * Unless required by applicable law or agreed to in writing, software");
            sw.WriteLine(" * distributed under the License is distributed on an \"AS IS\" BASIS,");
            sw.WriteLine(" * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.");
            sw.WriteLine(" * See the License for the specific language governing permissions and");
            sw.WriteLine(" * limitations under the License.");
            sw.WriteLine(" */");
            sw.WriteLine("package cc.mrbird.febs." + strTableNameLower + ".controller;");
            sw.WriteLine("");
            sw.WriteLine("import java.util.Map;");
            sw.WriteLine("");
            sw.WriteLine("import javax.validation.Valid;");
            sw.WriteLine("import javax.validation.constraints.NotBlank;");
            sw.WriteLine("");
            sw.WriteLine("import org.apache.shiro.authz.annotation.RequiresPermissions;");
            sw.WriteLine("import org.springframework.beans.factory.annotation.Autowired;");
            sw.WriteLine("import org.springframework.validation.annotation.Validated;");
            sw.WriteLine("import org.springframework.web.bind.annotation.GetMapping;");
            sw.WriteLine("import org.springframework.web.bind.annotation.PathVariable;");
            sw.WriteLine("import org.springframework.web.bind.annotation.PostMapping;");
            sw.WriteLine("import org.springframework.web.bind.annotation.RequestMapping;");
            sw.WriteLine("import org.springframework.web.bind.annotation.RestController;");
            sw.WriteLine("");
            sw.WriteLine("import com.baomidou.mybatisplus.core.toolkit.StringPool;");
            sw.WriteLine("");
            sw.WriteLine("import cc.mrbird.febs.common.annotation.ControllerEndpoint;");
            sw.WriteLine("import cc.mrbird.febs.common.controller.BaseController;");
            sw.WriteLine("import cc.mrbird.febs.common.entity.QueryRequest;");
            sw.WriteLine("import cc.mrbird.febs.common.entity.ResultResponse;");
            sw.WriteLine("import cc.mrbird.febs.common.exception.FebsException;");
            sw.WriteLine("import cc.mrbird.febs." + strTableNameLower + ".entity." + strClassName + ";");
            sw.WriteLine("import cc.mrbird.febs." + strTableNameLower + ".service.I" + strClassName + "Service;");
            sw.WriteLine("");
            sw.WriteLine("/**");
            sw.WriteLine(" * "+ strTableComment + "类");
            sw.WriteLine(" * @Author Cloudin");
            sw.WriteLine(" * @CreateDate "+ Convert.ToDateTime(strCreateDate).ToString("yyyy年M月d日"));
            sw.WriteLine(" * @UpdateDate "+ DateTime.Now.ToString("yyyy年M月d日"));
            sw.WriteLine(" * @Version V" + strVersion + "." + strCode);
            sw.WriteLine(" */");
            sw.WriteLine("@Validated");
            sw.WriteLine("@RestController");
            sw.WriteLine("@RequestMapping(\"" + strTableNameLower + "\")");
            sw.WriteLine("public class "+ strClassName + "Controller extends BaseController {");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 声明调用" + strTableComment + "接口");
            sw.WriteLine("	 */");
            sw.WriteLine("	@Autowired");
            sw.WriteLine("	private I" + strClassName + "Service " + strTableNameLower + "Service;");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 获取" + strTableComment + "列表");
            sw.WriteLine("	 * @param " + strTableNameLower + "");
            sw.WriteLine("	 * @param request");
            sw.WriteLine("	 * @return");
            sw.WriteLine("	 */");
            sw.WriteLine("	@GetMapping(\"list\")");
            sw.WriteLine("	//@RequiresPermissions(\""+ strTableNameLower + ":list\")");
            sw.WriteLine("	public ResultResponse " + strClassName + "List(" + strClassName + " " + strTableNameLower + ", QueryRequest request) {");
            sw.WriteLine("		Map<String, Object>");
            sw.WriteLine("    dataTable = getDataTable(this." + strTableNameLower + ".findDetailList(" + strTableNameLower + ", request));");
            sw.WriteLine("    return new ResultResponse().success().tableListData(dataTable);");
            sw.WriteLine("    }");
            sw.WriteLine("");
            sw.WriteLine("    /**");
            sw.WriteLine("    * 新增"+ strTableComment + "");
            sw.WriteLine("    * @param " + strTableNameLower + "");
            sw.WriteLine("    * @return");
            sw.WriteLine("    */");
            sw.WriteLine("    @PostMapping(\"create\")");
            sw.WriteLine("    //@RequiresPermissions(\"" + strTableNameLower + ":create\")");
            sw.WriteLine("    @ControllerEndpoint(operation = \"新增"+ strTableComment + "\", exceptionMessage = \"新增"+ strTableComment + "失败\")");
            sw.WriteLine("    public ResultResponse addUser(@Valid " + strClassName + " " + strTableNameLower + ") {");
            sw.WriteLine("    this." + strTableNameLower + ".create" + strClassName + "(" + strTableNameLower + ");");
            sw.WriteLine("    return new ResultResponse().success();");
            sw.WriteLine("    }");
            sw.WriteLine("");
            sw.WriteLine("    /**");
            sw.WriteLine("    * 删除"+ strTableComment + "，支持批量删除，逗号分隔");
            sw.WriteLine("    * @param Ids");
            sw.WriteLine("    * @return");
            sw.WriteLine("    */");
            sw.WriteLine("    @PostMapping(\"delete/{Ids}\")");
            sw.WriteLine("    //@RequiresPermissions(\"" + strTableNameLower + ":delete\")");
            sw.WriteLine("    @ControllerEndpoint(operation = \"删除"+ strTableComment + "\", exceptionMessage = \"删除"+ strTableComment + "失败\")");
            sw.WriteLine("    public ResultResponse deleteUsers(@NotBlank(message = \"{required}\") @PathVariable String Ids) {");
            sw.WriteLine("    String[] ids = Ids.split(StringPool.COMMA);");
            sw.WriteLine("    this." + strTableNameLower + ".delete" + strClassName + "(ids);");
            sw.WriteLine("    return new ResultResponse().success();");
            sw.WriteLine("    }");
            sw.WriteLine("");
            sw.WriteLine("    /**");
            sw.WriteLine("    * 查看"+ strTableComment + "");
            sw.WriteLine("    * @param newId");
            sw.WriteLine("    * @return");
            sw.WriteLine("    */");
            sw.WriteLine("    @PostMapping(\"details/{newId}\")");
            sw.WriteLine("    //@RequiresPermissions(\"" + strTableNameLower + ":details\")");
            sw.WriteLine("    @ControllerEndpoint(operation = \"查看"+ strTableComment + "\", exceptionMessage = \"查看"+ strTableComment + "失败\")");
            sw.WriteLine("    public ResultResponse edit" + strClassName + "(@NotBlank(message = \"{required}\") @PathVariable Long newId) {");
            sw.WriteLine("    " + strClassName + " " + strTableNameLower + " = this." + strTableNameLower + "Service.findById(newId);");
            sw.WriteLine("    return new ResultResponse().success().data(" + strTableNameLower + ");");
            sw.WriteLine("    }");
            sw.WriteLine("");
            sw.WriteLine("    /**");
            sw.WriteLine("    * 编辑"+ strTableComment + "");
            sw.WriteLine("    * @param " + strTableNameLower + "");
            sw.WriteLine("    * @return");
            sw.WriteLine("    */");
            sw.WriteLine("    @PostMapping(\"edit\")");
            sw.WriteLine("    //@RequiresPermissions(\"" + strTableNameLower + ":edit\")");
            sw.WriteLine("    @ControllerEndpoint(operation = \"编辑"+ strTableComment + "\", exceptionMessage = \"编辑"+ strTableComment + "失败\")");
            sw.WriteLine("    public ResultResponse updateNew(@Valid " + strClassName + " " + strTableNameLower + ") {");
            sw.WriteLine("    if (" + strTableNameLower + ".get" + strClassName + "Id() == null)");
            sw.WriteLine("    throw new FebsException(\"ID为空\");");
            sw.WriteLine("    this." + strTableNameLower + "Service.update" + strClassName + "(" + strTableNameLower + ");");
            sw.WriteLine("    return new ResultResponse().success();");
            sw.WriteLine("    }");
            sw.WriteLine("    }");

            sw.Close();

        }

        
    }
}
