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
    /// Java Service类
    /// </summary>
    public class JavaServiceHelper
    {

        /// <summary>
        /// 根据表名创建Service类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateServiceClass(string strFilePath,string strProjectName, string strTableName,string strTableComment)
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

            string strNewPath = strFilePath + "\\" + strTableNameLower + "\\service";
            if (!Directory.Exists(strNewPath))
            {
                Directory.CreateDirectory(strNewPath);
            }
            
            StreamWriter sw = new StreamWriter(strNewPath + "\\I" + strClassName + ".java", false, Encoding.GetEncoding("utf-8"));
            
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
            sw.WriteLine("package cc.mrbird.febs.news.service;");
            sw.WriteLine("");
            sw.WriteLine("import com.baomidou.mybatisplus.core.metadata.IPage;");
            sw.WriteLine("import com.baomidou.mybatisplus.extension.service.IService;");
            sw.WriteLine("");
            sw.WriteLine("import cc.mrbird.febs.common.entity.QueryRequest;");
            sw.WriteLine("import cc.mrbird.febs." + strTableNameLower + ".entity." + strClassName + ";");
            sw.WriteLine("");
            sw.WriteLine("public interface INewsService extends IService<" + strClassName + "> {");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 获取列表");
            sw.WriteLine("	 * @param " + strTableNameLower + "");
            sw.WriteLine("	 * @param request");
            sw.WriteLine("	 * @return");
            sw.WriteLine("	 */");
            sw.WriteLine("	IPage<" + strClassName + "> findList(News " + strTableNameLower + ", QueryRequest request);");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 新增");
            sw.WriteLine("	 *");
            sw.WriteLine("	 * @param user user");
            sw.WriteLine("	 */");
            sw.WriteLine("	void create" + strClassName + "(" + strClassName + " " + strTableNameLower + ");");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 删除");
            sw.WriteLine("	 */");
            sw.WriteLine("	void delete" + strClassName + "(String[] Ids);");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 修改");
            sw.WriteLine("	 */");
            sw.WriteLine("	void update" + strClassName + "(" + strClassName + " " + strTableNameLower + ");");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 通过ID获取详情");
            sw.WriteLine("	 */");
            sw.WriteLine("	" + strClassName + " findById(Long id);");
            sw.WriteLine("}");


            sw.Close();

        }

        
    }
}
