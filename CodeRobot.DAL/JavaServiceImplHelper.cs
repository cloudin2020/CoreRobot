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
    public class JavaServiceImplHelper
    {

        /// <summary>
        /// 根据表名创建Service类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateServiceImplClass(string strFilePath,string strProjectName, string strTableName,string strTableComment)
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

            string strNewPath = strFilePath + "\\" + strTableNameLower + "\\service\\impl";
            if (!Directory.Exists(strNewPath))
            {
                Directory.CreateDirectory(strNewPath);
            }
            
            StreamWriter sw = new StreamWriter(strNewPath + "\\" + strClassName + "Impl.java", false, Encoding.GetEncoding("utf-8"));
            
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
            sw.WriteLine("package cc.mrbird.febs.news.service.impl;");
            sw.WriteLine("");
            sw.WriteLine("import java.util.Arrays;");
            sw.WriteLine("import java.util.Date;");
            sw.WriteLine("import java.util.List;");
            sw.WriteLine("");
            sw.WriteLine("import org.springframework.stereotype.Service;");
            sw.WriteLine("import org.springframework.transaction.annotation.Propagation;");
            sw.WriteLine("import org.springframework.transaction.annotation.Transactional;");
            sw.WriteLine("");
            sw.WriteLine("import com.baomidou.mybatisplus.core.metadata.IPage;");
            sw.WriteLine("import com.baomidou.mybatisplus.extension.plugins.pagination.Page;");
            sw.WriteLine("import com.baomidou.mybatisplus.extension.service.impl.ServiceImpl;");
            sw.WriteLine("");
            sw.WriteLine("import cc.mrbird.febs.common.entity.FebsConstant;");
            sw.WriteLine("import cc.mrbird.febs.common.entity.QueryRequest;");
            sw.WriteLine("import cc.mrbird.febs.common.utils.SortUtil;");
            sw.WriteLine("import cc.mrbird.febs." + strTableNameLower + ".entity."+ strClassName + ";");
            sw.WriteLine("import cc.mrbird.febs." + strTableNameLower + ".mapper." + strClassName + "Mapper;");
            sw.WriteLine("import cc.mrbird.febs." + strTableNameLower + ".service.I" + strClassName + "Service;");
            sw.WriteLine("");
            sw.WriteLine("@Service");
            sw.WriteLine("@Transactional(propagation = Propagation.SUPPORTS, readOnly = true, rollbackFor = Exception.class)");
            sw.WriteLine("public class " + strClassName + "ServiceImpl extends ServiceImpl<" + strClassName + "Mapper, " + strClassName + "> implements I" + strClassName + "Service {");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 获取列表");
            sw.WriteLine("	 */");
            sw.WriteLine("	@Override");
            sw.WriteLine("	public IPage<" + strClassName + "> findList(" + strClassName + " " + strTableNameLower + ", QueryRequest request) {");
            sw.WriteLine("		Page<" + strClassName + "> page = new Page<>(request.getPageNum(), request.getPageSize());");
            sw.WriteLine("		SortUtil.handlePageSort(request, page, \"userId\", FebsConstant.ORDER_ASC, false);");
            sw.WriteLine("		return this.baseMapper.findDetailPage(page, " + strTableNameLower + ");");
            sw.WriteLine("	}");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 创建新闻");
            sw.WriteLine("	 */");
            sw.WriteLine("	@Override");
            sw.WriteLine("	public void create" + strClassName + "(" + strClassName + " " + strTableNameLower + ") {");
            sw.WriteLine("		news.setCreatedAt(new Date());");
            sw.WriteLine("		save(" + strTableNameLower + ");");
            sw.WriteLine("	}");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 删除新闻");
            sw.WriteLine("	 */");
            sw.WriteLine("	@Override");
            sw.WriteLine("	@Transactional");
            sw.WriteLine("	public void delete" + strClassName + "(String[] ids) {");
            sw.WriteLine("		List<String> list = Arrays.asList(ids);");
            sw.WriteLine("		this.removeByIds(list);");
            sw.WriteLine("	}");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 更新新闻");
            sw.WriteLine("	 */");
            sw.WriteLine("	@Override");
            sw.WriteLine("	@Transactional");
            sw.WriteLine("	public void update" + strClassName + "(" + strClassName + " " + strTableNameLower + ") {");
            sw.WriteLine("		updateById(" + strTableNameLower + ");");
            sw.WriteLine("	}");
            sw.WriteLine("");
            sw.WriteLine("	/**");
            sw.WriteLine("	 * 根据ID获取详情");
            sw.WriteLine("	 */");
            sw.WriteLine("	@Override");
            sw.WriteLine("	public " + strClassName + " findById(Long id) {");
            sw.WriteLine("		return getById(id);");
            sw.WriteLine("	}");
            sw.WriteLine("");
            sw.WriteLine("}");

            sw.Close();

        }

        
    }
}
