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
    public class ControllersHelper
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
            sw.WriteLine("using System;");
            sw.WriteLine("using System.Linq;");
            sw.WriteLine("using System.Threading.Tasks;");
            sw.WriteLine("using Microsoft.AspNetCore.Mvc;");
            sw.WriteLine("using Microsoft.EntityFrameworkCore;");
            sw.WriteLine("using log4net;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine("using "+ strProjectName + ".Manage.Data;");
            sw.WriteLine("using " + strProjectName + ".Models;");
            sw.WriteLine("");
            sw.WriteLine("namespace " + strProjectName + ".Manage.Controllers");
            sw.WriteLine("{");
            sw.WriteLine("");
            sw.WriteLine("    /// <summary>");
            sw.WriteLine("    /// 版权所有: Copyright © " + DateTime.Now.Year.ToString() + " "+strCompany+". 保留所有权利。");
            sw.WriteLine("    /// 内容摘要: " + strClassName + " 处理请求的类");
            sw.WriteLine("    /// 创建日期：" + Convert.ToDateTime(strCreateDate).ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 更新日期：" + DateTime.Now.ToString("yyyy年M月d日"));
            sw.WriteLine("    /// 版    本：V" + strVersion + "." + strCode + " ");
            sw.WriteLine("    /// 作    者：" + strAuthor);
            sw.WriteLine("    /// </summary>");
            sw.WriteLine("    public class " + strClassName + "Controller : Controller");
            sw.WriteLine("    {");
            sw.WriteLine("        private readonly " + strProjectName + "ManageContext _context;");
            sw.WriteLine("        private readonly ILog log;");
            sw.WriteLine("");
            sw.WriteLine("        // "+ strTableComment + " 实例化数据上下文");
            sw.WriteLine("        public "+ strClassName + "Controller(" + strProjectName + "ManageContext context)");
            sw.WriteLine("        {");
            sw.WriteLine("            _context = context;");
            sw.WriteLine("            this.log = LogManager.GetLogger(Startup.repository.Name, typeof(" + strClassName + "Controller));");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        // GET: /<controller>/ Index.cshtml");
            sw.WriteLine("        public IActionResult Index()");
            sw.WriteLine("        {");
            sw.WriteLine("            return View();");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        // GET: /<controller>/ Create.cshtml");
            sw.WriteLine("        public IActionResult Create()");
            sw.WriteLine("        {");
            sw.WriteLine("            return View();");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        // GET: /<controller>/ Edit.cshtml");
            sw.WriteLine("        public IActionResult Edit()");
            sw.WriteLine("        {");
            sw.WriteLine("            return View();");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        // GET: /<controller>/ Views.cshtml");
            sw.WriteLine("        public IActionResult Views()");
            sw.WriteLine("        {");
            sw.WriteLine("            return View();");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        // GET: "+ strTableNameLower +"/lists/?page=1&limit=10");
            sw.WriteLine("        public async Task<IActionResult> List(int page, int limit)");
            sw.WriteLine("        {");
            sw.WriteLine("            var list = new List<"+strClassName+">();");
            sw.WriteLine("            long lCount = 0;");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("               var result = _context." + strTableName+".Where(n => n."+ strPrimaryKey + " > 0);");
            sw.WriteLine("               //if (!string.IsNullOrEmpty(news_title))");
            sw.WriteLine("                   //result = result.Where(n => n.news_title.Contains(news_title));");
            sw.WriteLine("");
            sw.WriteLine("               list = await result.OrderByDescending(n => n." + strPrimaryKey + ").Skip((page - 1) * limit).Take(limit).ToListAsync();");
            sw.WriteLine("               lCount = result.LongCount();");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> List\", ex);");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            return Json(new { code = 0, msg = \"success\", data = list, count = lCount });");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("        // GET: " + strTableNameLower + "/select");
            sw.WriteLine("        public async Task<IActionResult> Select()");
            sw.WriteLine("        {");
            sw.WriteLine("            var list = new List<" + strClassName + ">();");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("               var result = _context." + strTableName + ".Where(n => n." + strPrimaryKey + " > 0);");
            sw.WriteLine("");
            sw.WriteLine("               list = await result.OrderByDescending(n => n." + strPrimaryKey + ").ToListAsync();");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> Select\", ex);");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            return Json(new { code = 0, msg = \"success\", data = list });");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        // GET: "+ strTableNameLower +"/details/1");
            sw.WriteLine("        public async Task<IActionResult> Details(int? id)");
            sw.WriteLine("        {");
            sw.WriteLine("            var " + strTableNameSpec + " = new "+strClassName+"();");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("               if (id == null)");
            sw.WriteLine("               {");
            sw.WriteLine("                   return Json(new { code = 1, msg = \"获取数据失败\" });");
            sw.WriteLine("               }");
            sw.WriteLine("");
            sw.WriteLine("               " + strTableNameSpec+" = await _context."+strTableName+"");
            sw.WriteLine("                   .FirstOrDefaultAsync(m => m." + strPrimaryKey + " == id);");
            sw.WriteLine("               if (" + strTableNameSpec+" == null)");
            sw.WriteLine("               {");
            sw.WriteLine("                   return Json(new { code = 1, msg = \"获取数据失败\" });");
            sw.WriteLine("               }");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> Details\", ex);");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            return Json(new { code = 0, msg = \"获取数据成功\", data = "+strTableNameSpec+"});");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        // POST: "+ strTableNameLower +"/create");
            sw.WriteLine("        [HttpPost]");
            sw.WriteLine("        [ValidateAntiForgeryToken]");
            sw.WriteLine("        public async Task<IActionResult> Create([Bind(\""+ strAllColumnNameNotKey + "\")] " + strClassName + " "+strTableNameSpec+")");
            sw.WriteLine("        {");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                _context.Add(" + strTableNameSpec+");");
            sw.WriteLine("                await _context.SaveChangesAsync();");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> Create\", ex);");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            return Json(new { code = 0, msg = \"添加成功\" });");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        // POST: "+ strTableNameLower +"/edit/1");
            sw.WriteLine("        [HttpPost]");
            sw.WriteLine("        [ValidateAntiForgeryToken]");
            sw.WriteLine("        public async Task<IActionResult> Edit(int id, [Bind(\""+ strAllColumnName+ "\")] " + strClassName + " "+strTableNameSpec+")");
            sw.WriteLine("        {");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                if (id != " + strTableNameSpec+ "." + strPrimaryKey + ")");
            sw.WriteLine("                {");
            sw.WriteLine("                    return Json(new { code = 0, msg = \"ID不存在\" });");
            sw.WriteLine("                }");
            sw.WriteLine("");
            sw.WriteLine("                try");
            sw.WriteLine("                {");
            sw.WriteLine("                    _context.Update(" + strTableNameSpec+");");
            sw.WriteLine("                    await _context.SaveChangesAsync();");
            sw.WriteLine("                }");
            sw.WriteLine("                catch (DbUpdateConcurrencyException)");
            sw.WriteLine("                {");
            sw.WriteLine("                    if (!" + strClassName + "Exists(" + strTableNameSpec+ "." + strPrimaryKey + "))");
            sw.WriteLine("                    {");
            sw.WriteLine("                        return Json(new { code = 0, msg = \"更新失败\" });");
            sw.WriteLine("                    }");
            sw.WriteLine("                    else");
            sw.WriteLine("                    {");
            sw.WriteLine("                        throw;");
            sw.WriteLine("                    }");
            sw.WriteLine("                }");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> Edit\", ex);");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            return Json(new { code = 0, msg = \"更新成功\" });");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        // POST: "+ strTableNameLower +"/delete/1");
            sw.WriteLine("        [HttpPost]");
            sw.WriteLine("        [ValidateAntiForgeryToken]");
            sw.WriteLine("        public async Task<IActionResult> Delete(int id)");
            sw.WriteLine("        {");
            sw.WriteLine("            try");
            sw.WriteLine("            {");
            sw.WriteLine("                var " + strTableNameSpec+" = await _context."+strTableName+".FindAsync(id);");
            sw.WriteLine("                _context." + strTableName+".Remove("+strTableNameSpec+");");
            sw.WriteLine("                await _context.SaveChangesAsync();");
            sw.WriteLine("            }");
            sw.WriteLine("            catch (Exception ex)");
            sw.WriteLine("            {");
            sw.WriteLine("                log.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + \" -> Delete\", ex);");
            sw.WriteLine("            }");
            sw.WriteLine("");
            sw.WriteLine("            return Json(new { code = 0, msg = \"删除成功\" });");
            sw.WriteLine("        }");
            sw.WriteLine("");
            sw.WriteLine("        // 检测id是否存在");
            sw.WriteLine("        private bool "+ strClassName + "Exists(int id)");
            sw.WriteLine("        {");
            sw.WriteLine("            return _context."+strTableName+ ".Any(e => e." + strPrimaryKey + " == id);");
            sw.WriteLine("        }");
            sw.WriteLine("    }");
            sw.WriteLine("}");
            sw.Close();

        }

    }
}
