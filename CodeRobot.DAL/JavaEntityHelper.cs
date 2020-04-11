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
    public class JavaEntityHelper
    {

        /// <summary>
        /// 根据表名创建Controllers类
        /// </summary>
        /// <param name="strFilePath">保存路径</param>
        /// <param name="strProjectName">项目名</param>
        /// <param name="strTableName">表名</param>
        public static void CreateEntityClass(string strFilePath,string strProjectName,string strDefineList,string strValueList, string strTableName,string strTableComment)
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

            string strNewPath = strFilePath + "\\" + strTableNameLower + "\\entity";
            if (!Directory.Exists(strNewPath))
            {
                Directory.CreateDirectory(strNewPath);
            }
            
            StreamWriter sw = new StreamWriter(strNewPath + "\\" + strClassName + ".java", false, Encoding.GetEncoding("utf-8"));

            sw.WriteLine("/*");
            sw.WriteLine(" * Copyright 2020 Cloudin.");
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
            sw.WriteLine("package cc.mrbird.febs." + strTableNameLower + ".entity;");
            sw.WriteLine("");
            sw.WriteLine("import java.io.Serializable;");
            sw.WriteLine("import java.util.Date;");
            sw.WriteLine("");
            sw.WriteLine("import com.baomidou.mybatisplus.annotation.IdType;");
            sw.WriteLine("import com.baomidou.mybatisplus.annotation.TableField;");
            sw.WriteLine("import com.baomidou.mybatisplus.annotation.TableId;");
            sw.WriteLine("import com.baomidou.mybatisplus.annotation.TableName;");
            sw.WriteLine("import com.wuwenze.poi.annotation.Excel;");
            sw.WriteLine("");
            sw.WriteLine("import lombok.Data;");
            sw.WriteLine("");
            sw.WriteLine("/**");
            sw.WriteLine(" * "+strTableComment+"类");
            sw.WriteLine(" * @Author Cloudin");
            sw.WriteLine(" * @CreateDate " + Convert.ToDateTime(strCreateDate).ToString("yyyy年M月d日"));
            sw.WriteLine(" * @UpdateDate " + DateTime.Now.ToString("yyyy年M月d日"));
            sw.WriteLine(" * @Version V" + strVersion + "." + strCode);
            sw.WriteLine(" */");
            sw.WriteLine("@Data");
            sw.WriteLine("@TableName(\""+ strTableName + "\")");
            sw.WriteLine("@Excel(\"" + strTableComment + "\")");
            sw.WriteLine("public class " + strClassName + " implements Serializable {");
            sw.WriteLine("");
            sw.WriteLine(strDefineList);
            sw.WriteLine("");
            sw.WriteLine(strValueList);
            sw.WriteLine("}");


            sw.Close();

        }

        
    }
}
