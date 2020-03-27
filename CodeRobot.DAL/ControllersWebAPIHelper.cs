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
    /// .net core web api处理请求类
    /// </summary>
    public class ControllersWebAPIHelper
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
            strClassName = CommonHelper.GetTableNameUpper(strClassName);
            string strTableNameSpec = CommonHelper.GetTableNameFirstLowerSecondUpper(strClassName);//如：newsTypes
            string strTableNameLower = strTableNameSpec.ToLower();//如：newstypes

            Directory.CreateDirectory(strFilePath);
            StreamWriter sw = new StreamWriter(strFilePath + "\\" + strClassName + "Controller.cs", false, Encoding.GetEncoding("utf-8"));

            sw.WriteLine("using Microsoft.AspNetCore.Http;");
            sw.WriteLine("using Microsoft.AspNetCore.Mvc;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine("using System.Linq;");
            sw.WriteLine("using Microsoft.EntityFrameworkCore;");
            sw.WriteLine("using "+strProjectName+".API.Data;");
            sw.WriteLine("using " + strProjectName + ".Models;");
            sw.WriteLine("using System;");
            sw.WriteLine("using log4net;");
            sw.WriteLine("using System.Threading.Tasks;");
            sw.WriteLine("");
            sw.WriteLine("namespace " + strProjectName + ".API.Controllers");
            sw.WriteLine("{");
            sw.WriteLine("    /// <summary>");
            sw.WriteLine("    /// "+ strTableComment + "类，定义了CURD相关操作");
            sw.WriteLine("    /// </summary>");
            sw.WriteLine("    [Produces(\"application/json\")]");
            sw.WriteLine("    [Route(\"api/[controller]\")]");
            sw.WriteLine("    [ApiController]");
            sw.WriteLine("    public class " + strClassName + "Controller : Controller");
            sw.WriteLine("    {");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 初始化数据库上下文");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        private readonly " + strProjectName + "ApiContext _context;");
            sw.WriteLine("        private readonly ILog log;");
            sw.WriteLine("");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 构造及初始化类参数");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        public " + strClassName + "Controller(" + strProjectName + "ApiContext context)");
            sw.WriteLine("        {");
            sw.WriteLine("            _context = context;");
            sw.WriteLine("            this.log = LogManager.GetLogger(Startup.repository.Name, typeof(" + strClassName + "Controller));");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 分页显示" + strTableComment + "数据");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"page\">分页</param>");
            sw.WriteLine("        /// <param name=\"limit\">每页显示数量</param>");
            sw.WriteLine("        /// <returns></returns>");
            sw.WriteLine("        [HttpGet]");
            sw.WriteLine("        [Route(\"/api/" + strTableNameLower + "\")]");
            sw.WriteLine("        public async Task<IActionResult> GetByPage(int page, int limit)");
            sw.WriteLine("        {");
            sw.WriteLine("            long lCount = 0;");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            if (!string.IsNullOrEmpty(CommonHelper.GetDetailsSelectJoinList(strTableName)))
            {
                sw.WriteLine("                var result = from item in _context.Set<"+ strClassName + ">()");
                sw.WriteLine("                             "+ CommonHelper.GetDetailsSelectJoinList(strTableName));
                sw.WriteLine("                             select new");
                sw.WriteLine("                             {");
                sw.WriteLine(CommonHelper.GetLeftSelectColumnName2(strTableName, "item"));
                sw.WriteLine(CommonHelper.GetDetailsSelectJoinColumn(strTableName, strPrimaryKey, CommonHelper.GetLeftSelectColumnName2(strTableName, "item")));
                sw.WriteLine("");
                sw.WriteLine("                              };");
                sw.WriteLine("                //if (!string.IsNullOrEmpty(search_key))");
                sw.WriteLine("                    //result = result.Where(n => n.search_key.Contains(search_key));");
                sw.WriteLine("                var list = await result.OrderByDescending(n => n."+strPrimaryKey+").Skip((page - 1) * limit).Take(limit).ToListAsync();");
                sw.WriteLine("                lCount = result.LongCount();");
                sw.WriteLine("                if (lCount <= 0)");
                sw.WriteLine("                {");
                sw.WriteLine("                    return Json(new { code = 0, msg = \"暂无数据\" });");
                sw.WriteLine("                }");
                sw.WriteLine("                else");
                sw.WriteLine("                {");
                sw.WriteLine("                    return Json(new { code = 0, msg = \"success\", data = list, count = lCount });");
                sw.WriteLine("                }");
            }
            else
            {
                sw.WriteLine("                var result = _context." + strTableName + ".Where(n => n." + strPrimaryKey + " > 0);");
                sw.WriteLine("                lCount = result.LongCount();");
                sw.WriteLine("                var list =  await result.OrderByDescending(n => n." + strPrimaryKey + ").Skip((page - 1) * limit).Take(limit).ToListAsync();");
                sw.WriteLine("                return Json(new { code = 0, msg = \"success\", data = list, count = lCount });");
            }
            
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> GetDataByPage\", ex);");
            sw.WriteLine("                return Json(new { code = 1, msg = ex.Message });");
            sw.WriteLine("            }");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 获取" + strTableComment + "表中数据填充下拉表单");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <returns>返回数据列表</returns>");
            sw.WriteLine("        [HttpGet]");
            sw.WriteLine("        [Route(\"/api/" + strTableNameLower + "/select\")]");
            sw.WriteLine("        public async Task<IActionResult> Select()");
            sw.WriteLine("        {");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                var list = await _context." + strTableName + ".OrderByDescending(n => n." + strPrimaryKey + ").ToListAsync();");
            sw.WriteLine("                return Json(new { code = 0, msg = \"success\", data = list });");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> GetAll\", ex);");
            sw.WriteLine("                return Json(new { code = 1, msg = ex.Message });");
            sw.WriteLine("            }");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 根据ID获取"+ strTableComment + "详情");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"id\">"+ strTableComment + "ID</param>");
            sw.WriteLine("        /// <returns>返回"+ strTableComment + "详情</returns>");
            sw.WriteLine("        [HttpGet(\"{id}\")]");
            if (CommonHelper.ChecktKeyIsBigint(strTableName, strPrimaryKey))
            {
                sw.WriteLine("        public async Task<IActionResult> GetById(long id)");
            }
            else
            {
                sw.WriteLine("        public async Task<IActionResult> GetById(int id)");
            }
            sw.WriteLine("        {");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                //log.Debug(\"id=\"+id);");
            sw.WriteLine("                var item = await _context." + strTableName + ".FirstOrDefaultAsync(m => m." + strPrimaryKey + " == id);");
            sw.WriteLine("                if (item == null)");
            sw.WriteLine("                {");
            sw.WriteLine("                    return Json(new { code = 1, msg = \"数据为空\" });");
            sw.WriteLine("                }");
            sw.WriteLine("                else");
            sw.WriteLine("                {");
            if (!string.IsNullOrEmpty(CommonHelper.GetDetailsSelectJoin(strTableName)))
            {
                sw.WriteLine("                    var result = ("+ CommonHelper.GetDetailsSelectJoin(strTableName));
                sw.WriteLine("                                  select new");
                sw.WriteLine("                                  {");
                sw.WriteLine(CommonHelper.GetLeftSelectColumnName2(strTableName,"item"));
                sw.WriteLine(CommonHelper.GetDetailsSelectJoinColumn(strTableName, strPrimaryKey, CommonHelper.GetLeftSelectColumnName2(strTableName, "item")));
                sw.WriteLine("");
                sw.WriteLine("                                  }).FirstOrDefault();");
                sw.WriteLine("                    return Json(new { code = 0, msg = \"success\", data = result });");
                sw.WriteLine("");
            }
            else
            {
                sw.WriteLine("                    return Json(new { code = 0, msg = \"success\", data = item });");
            }
            sw.WriteLine("                }");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> GetById\", ex);");
            sw.WriteLine("                return Json(new { code = 1, msg = ex.Message });");
            sw.WriteLine("            }");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 新增"+ strTableComment + "");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"" + strTableNameSpec + "\">构造" + strTableComment + "对象数据</param>");
            sw.WriteLine("        /// <returns>返回新增的"+ strTableComment + "</returns>          ");
            sw.WriteLine("        [HttpPost]");
            sw.WriteLine("        public async Task<IActionResult> Create(" + strClassName + " " + strTableNameSpec + ")");
            sw.WriteLine("        {");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                //log.Debug(" + CommonHelper.GetSaveLogColumnName(strTableName, strTableNameSpec) + ");");
            if (CommonHelper.ChecktCreatedAtKey(strTableName, "created_at"))
            {
                sw.WriteLine("                " + strTableNameSpec + ".created_at = DateTime.Now;");
            }
            sw.WriteLine("                _context." + strTableName + ".Add(" + strTableNameSpec + ");");
            sw.WriteLine("                await _context.SaveChangesAsync();");
            sw.WriteLine("                return Json(new { code = 0, msg = \"success\", data = " + strTableNameSpec + " });");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> Create\", ex);");
            sw.WriteLine("                return Json(new { code = 1, msg = ex.Message });");
            sw.WriteLine("            }");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 根据ID更新"+ strTableComment + "");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"id\">ID</param>");
            sw.WriteLine("        /// <param name=\"" + strTableNameSpec + "\">构造"+ strTableComment + "对象数据</param>");
            sw.WriteLine("        /// <returns></returns>");
            sw.WriteLine("        [HttpPut(\"{id}\")]");
            if (CommonHelper.ChecktKeyIsBigint(strTableName, strPrimaryKey))
            {
                sw.WriteLine("        public async Task<IActionResult> Update(long id, " + strClassName + " " + strTableNameSpec + ")");
            }
            else
            {
                sw.WriteLine("        public async Task<IActionResult> Update(int id, " + strClassName + " " + strTableNameSpec + ")");
            }
            sw.WriteLine("        {");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                //log.Debug(" + CommonHelper.GetSaveLogColumnName(strTableName, strTableNameSpec) + ");");
            sw.WriteLine("                if (" + strTableNameSpec + " == null || " + strTableNameSpec + "." + strPrimaryKey + " != id)");
            sw.WriteLine("                {");
            sw.WriteLine("                    return Json(new { code = 1, msg = \"ID不存在\" });");
            sw.WriteLine("                }");
            sw.WriteLine("");
            sw.WriteLine("                var item = _context." + strTableName + ".Find(id);");
            sw.WriteLine("");
            sw.WriteLine("                if (item == null)");
            sw.WriteLine("                {");
            sw.WriteLine("                    return Json(new { code = 1, msg = \"ID不存在，更新失败！\" });");
            sw.WriteLine("                }");
            sw.WriteLine("");
            sw.WriteLine(GetUpdateItems(strTableNameSpec, strTableName));
            sw.WriteLine("");
            sw.WriteLine("                _context." + strTableName + ".Update(item);");
            sw.WriteLine("                await _context.SaveChangesAsync();");
            sw.WriteLine("");
            sw.WriteLine("                return Json(new { code = 0, msg = \"更新成功\"});");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> Update\", ex);");
            sw.WriteLine("                return Json(new { code = 1, msg = ex.Message });");
            sw.WriteLine("            }");
            sw.WriteLine("        }");
            sw.WriteLine("        ");
            sw.WriteLine("        /// <summary>");
            sw.WriteLine("        /// 删除"+ strTableComment + "");
            sw.WriteLine("        /// </summary>");
            sw.WriteLine("        /// <param name=\"id\">ID</param>");
            sw.WriteLine("        /// <returns>返回是否删除成功</returns>");
            sw.WriteLine("        [HttpDelete(\"{id}\")]");
            if (CommonHelper.ChecktKeyIsBigint(strTableName, strPrimaryKey))
            {
                sw.WriteLine("        public async Task<IActionResult> Delete(long id)");
            }
            else
            {
                sw.WriteLine("        public async Task<IActionResult> Delete(int id)");
            }
                
            sw.WriteLine("        {");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                var item = _context." + strTableName + ".Find(id);");
            sw.WriteLine("                if (item == null)");
            sw.WriteLine("                {");
            sw.WriteLine("                    return Json(new { code = 1, msg = \"ID为空，删除失败！\" });");
            sw.WriteLine("                }");
            sw.WriteLine("");
            sw.WriteLine("                _context." + strTableName + ".Remove(item);");
            sw.WriteLine("                await _context.SaveChangesAsync();");
            sw.WriteLine("");
            sw.WriteLine("                return Json(new { code = 0, msg = \"删除成功\" });");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> Create\", ex);");
            sw.WriteLine("                return Json(new { code = 1, msg = ex.Message });");
            sw.WriteLine("            }");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("    }");
            sw.WriteLine("}");
            sw.Close();

        }

        /// <summary>
        /// 获取更新字段
        /// </summary>
        /// <param name="strNewName"></param>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetUpdateItems(string strNewName, string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,DATA_TYPE,COLUMN_TYPE,COLUMN_KEY,COLUMN_COMMENT,EXTRA,COLUMN_DEFAULT,CHARACTER_SET_NAME FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();//注释

                    string strValue = "                item."+ strColumnName + " = "+ strNewName + "." + strColumnName + ";\r\n";
                    strReturnValue += strValue;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取搜索字段列表", "GetSearchItemList", false);
            }

            return strReturnValue;

        }

    }
}
