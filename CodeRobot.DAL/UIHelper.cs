using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeRobot.DAL
{
    public class UIHelper
    {
        /// <summary>
        /// 创建UI项目相关文件
        /// </summary>
        /// <param name="strFilePath">项目路径</param>
        /// <param name="strProjectName">项目名称</param>
        public static void CreateUIFile(string strFilePath, string strProjectName)
        {
            try
            {
                string strUIAndroidPath = strFilePath+ "\\Android";
                if (!Directory.Exists(strUIAndroidPath))
                {
                    Directory.CreateDirectory(strUIAndroidPath);
                }
                CodeRobot.Utility.TxtFile.CreateReadMeFile("# " + strProjectName + ".UI.Android\r\n### **Android客户端UI文件**", strUIAndroidPath);

                string strUIH5Path = strFilePath + "\\H5";
                if (!Directory.Exists(strUIH5Path))
                {
                    Directory.CreateDirectory(strUIH5Path);
                }
                CodeRobot.Utility.TxtFile.CreateReadMeFile("# " + strProjectName + ".UI.H5\r\n### **H5客户端UI文件**", strUIH5Path);

                string strUIiOSPath = strFilePath + "\\iOS";
                if (!Directory.Exists(strUIiOSPath))
                {
                    Directory.CreateDirectory(strUIiOSPath);
                }
                CodeRobot.Utility.TxtFile.CreateReadMeFile("# " + strProjectName + ".UI.iOS\r\n### **iOS客户端UI文件**", strUIiOSPath);

                string strUILogoPath = strFilePath + "\\VI";
                if (!Directory.Exists(strUILogoPath))
                {
                    Directory.CreateDirectory(strUILogoPath);
                }
                CodeRobot.Utility.TxtFile.CreateReadMeFile("# " + strProjectName + ".UI.VI\r\n### **VI相关UI文件**", strUILogoPath);

                string strUIManagePath = strFilePath + "\\Manage";
                if (!Directory.Exists(strUIManagePath))
                {
                    Directory.CreateDirectory(strUIManagePath);
                }
                CodeRobot.Utility.TxtFile.CreateReadMeFile("# " + strProjectName + ".UI.Manage\r\n### **Manage后台系统UI文件**", strUIManagePath);

                string strUIOthersPath = strFilePath + "\\Others";
                if (!Directory.Exists(strUIOthersPath))
                {
                    Directory.CreateDirectory(strUIOthersPath);
                }
                CodeRobot.Utility.TxtFile.CreateReadMeFile("# " + strProjectName + ".UI.Others\r\n### **Others相关UI文件**", strUIOthersPath);

                string strUIWebPath = strFilePath + "\\Web";
                if (!Directory.Exists(strUIWebPath))
                {
                    Directory.CreateDirectory(strUIWebPath);
                }
                CodeRobot.Utility.TxtFile.CreateReadMeFile("# " + strProjectName + ".UI.Web\r\n### **Web相关UI文件**", strUIWebPath);


                string strUIMiniAppPath = strFilePath + "\\MiniApp";
                if (!Directory.Exists(strUIMiniAppPath))
                {
                    Directory.CreateDirectory(strUIMiniAppPath);
                }
                CodeRobot.Utility.TxtFile.CreateReadMeFile("# " + strProjectName + ".UI.MiniApp\r\n### **MiniApp相关UI文件**", strUIMiniAppPath);


            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建UI项目相关文件", "CreateUIFile",false);
            }
        }
    }
}
