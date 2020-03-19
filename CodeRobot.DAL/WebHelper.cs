using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace CodeRobot.DAL
{
    public class WebHelper
    {
        /// <summary>
        /// 创建Web项目相关文件，如：Manage,Web,H5等Web项目
        /// </summary>
        /// <param name="strFilePath">项目路径</param>
        /// <param name="strProjectName">项目名称</param>
        public static void CreateWebFile(string strFilePath, string strProjectName)
        {
            try
            {
                //拷贝第三方库文件及配置文件
                //后台项目
                string strManagePath = strFilePath + "\\"+ strProjectName + ".Manage";
                if (!Directory.Exists(strManagePath))
                {
                    Directory.CreateDirectory(strManagePath);
                }

                File.Copy(Application.StartupPath + "\\copy\\web\\Web.config", strManagePath + "\\Web.config", true);
                File.Copy(Application.StartupPath + "\\copy\\web\\Web.Debug.config", strManagePath + "\\Web.Debug.config", true);
                File.Copy(Application.StartupPath + "\\copy\\log4net.config", strManagePath + "\\log4net.config", true);

                //Web项目
                string strWebPath = strFilePath + "\\" + strProjectName + ".Web";
                if (!Directory.Exists(strWebPath))
                {
                    Directory.CreateDirectory(strWebPath);
                }

                File.Copy(Application.StartupPath + "\\copy\\web\\Web.config", strWebPath + "\\Web.config", true);
                File.Copy(Application.StartupPath + "\\copy\\web\\Web.Debug.config", strWebPath + "\\Web.Debug.config", true);
                File.Copy(Application.StartupPath + "\\copy\\log4net.config", strWebPath + "\\log4net.config", true);

                //H5项目
                string strH5Path = strFilePath + "\\" + strProjectName + ".H5";
                if (!Directory.Exists(strH5Path))
                {
                    Directory.CreateDirectory(strH5Path);
                }

                File.Copy(Application.StartupPath + "\\copy\\web\\Web.config", strH5Path + "\\Web.config", true);
                File.Copy(Application.StartupPath + "\\copy\\web\\Web.Debug.config", strH5Path + "\\Web.Debug.config", true);
                File.Copy(Application.StartupPath + "\\copy\\log4net.config", strH5Path + "\\log4net.config", true);


            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(ReadmeHelper), ex, "创建UI项目相关文件", "CreateUIFile",false);
            }
        }
    }
}
