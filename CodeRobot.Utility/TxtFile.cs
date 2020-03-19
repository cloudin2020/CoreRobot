using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CodeRobot.Utility
{
    /// <summary>
    /// 生成Txt文件
    /// </summary>
    public class TxtFile
    {

        /// <summary>
        /// 生成文本文件
        /// </summary>
        /// <param name="strContent">写入文本文件内容</param>
        /// <param name="strFileName">文件名称</param>
        public static void CreateTxtFile(string strContent, string strFileName)
        {
            try
            {
                string strPath = Application.StartupPath;
                string strNewPath = strPath + "\\txt\\" + DateTime.Now.ToString("yyyyMMdd");
                if (!Directory.Exists(strNewPath))
                {
                    Directory.CreateDirectory(strNewPath);
                }
                string strFilePath = strNewPath + "\\" + strFileName + ".txt";

                FileInfo fi = new FileInfo(strFilePath);
                if (fi.Exists && fi.Length > 2048 * 1024)
                {
                    fi.Delete();
                }
                FileStream fs = fi.OpenWrite();

                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.Write(strContent + "\r\n");
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(TxtFile), ex, "生成文本文件", "CreateTxtFile", false);
            }
        }

        /// <summary>
        /// 读取文本文件里的内容
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <returns></returns>
        public static string ReadTxtFile(string strFileName)
        {
            string strReturnValue = "";

            try
            {
                StreamReader sr = new StreamReader(Application.StartupPath + "\\" + strFileName + ".txt", Encoding.Default);
                string strLine;
                while ((strLine = sr.ReadLine()) != null)
                {
                    string strValue = strLine.ToString();
                    strReturnValue += strValue;
                }
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(TxtFile), ex, "读取文本文件里的内容", "ReadTxtFile", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 创建README文件
        /// </summary>
        /// <param name="strContent">内容</param>
        /// <param name="strFilePath">路径</param>
        public static void CreateReadMeFile(string strContent, string strFilePath)
        {
            try
            {
                string strPath = strFilePath + "\\README.md";

                FileInfo fi = new FileInfo(strPath);
                if (fi.Exists && fi.Length > 2048 * 1024)
                {
                    fi.Delete();
                }
                FileStream fs = fi.OpenWrite();

                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.Write(strContent + "\r\n");
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(TxtFile), ex, "创建README文件", "CreateReadMeFile", false);
            }
        }

        /// <summary>
        /// 创建SQL文件
        /// </summary>
        /// <param name="strDBName"></param>
        /// <param name="strFilePath"></param>
        /// <param name="strContent"></param>
        public static void CreateSQLFile(string strDBName, string strFilePath, string strContent)
        {
            try
            {
                string strPath = strFilePath + "\\" + strDBName + ".txt";

                FileInfo fi = new FileInfo(strPath);
                if (fi.Exists && fi.Length > 2048 * 1024)
                {
                    fi.Delete();
                }
                FileStream fs = fi.OpenWrite();

                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.Write(strContent + "\r\n");
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(TxtFile), ex, "创建SQL文件", "CreateSQLFile", false);
            }
        }

        /// <summary>
        /// 生成数据库文档
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strFileName"></param>
        public static void CreateDoumentsFile(string strContent, string strFileName)
        {
            try
            {
                string strPath = Application.StartupPath;
                string strNewPath = strPath + "\\documents";
                if (!Directory.Exists(strNewPath))
                {
                    Directory.CreateDirectory(strNewPath);
                }
                string strFilePath = strNewPath + "\\" + strFileName + ".txt";

                FileInfo fi = new FileInfo(strFilePath);
                if (fi.Exists && fi.Length > 2048 * 1024)
                {
                    fi.Delete();
                }
                FileStream fs = fi.OpenWrite();

                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.Write(strContent + "\r\n");
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(TxtFile), ex, "生成文本文件", "CreateTxtFile", false);
            }
        }

    }
}
