using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeRobot.DAL
{
    public class HTMLHelper
    {
        /// <summary>
        /// 创建HTML项目相关文件
        /// </summary>
        /// <param name="strFilePath">项目路径</param>
        /// <param name="strProjectName">项目名称</param>
        public static void CreateHTMLFile(string strFilePath, string strProjectName)
        {
            try
            {
                string strUIH5Path = strFilePath + "\\H5";
                if (!Directory.Exists(strUIH5Path))
                {
                    Directory.CreateDirectory(strUIH5Path);
                }
                CodeRobot.Utility.TxtFile.CreateReadMeFile("# " + strProjectName + ".HTML.H5\r\n### **H5客户端HTML文件**", strUIH5Path);


                string strUIWebPath = strFilePath + "\\Web";
                if (!Directory.Exists(strUIWebPath))
                {
                    Directory.CreateDirectory(strUIWebPath);
                }
                CodeRobot.Utility.TxtFile.CreateReadMeFile("# " + strProjectName + ".HTML.Web\r\n### **项目官网相关HTML文件**", strUIWebPath);


                string strUIMiniAppPath = strFilePath + "\\MiniApp";
                if (!Directory.Exists(strUIMiniAppPath))
                {
                    Directory.CreateDirectory(strUIMiniAppPath);
                }
                CodeRobot.Utility.TxtFile.CreateReadMeFile("# " + strProjectName + ".HTML.MiniApp\r\n### **微信小程序相关HTML文件**", strUIMiniAppPath);


            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建HTML项目相关文件", "CreateHTMLFile", false);
            }
        }
    }
}
