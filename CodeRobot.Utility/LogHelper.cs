using System;
using log4net;

namespace CodeRobot.Utility
{

    /// <summary>
    /// 版权所有: Copyright © 2017 Cloudin. 保留所有权利。
    /// 内容摘要: 日志记录类(注意：如果是Winform程序，必须把log4net.config拷贝到Debug目录下)
    /// 完成日期：2017年05月15日
    /// 版    本：V1.0 
    /// 作    者：Knight
    /// </summary>
    public class LogHelper
    {

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="t">通常是类名</param>
        /// <param name="ex">异常对象</param>
        /// <param name="strMessageObject">通常是方法名或指定记录消息类型</param>
        /// <param name="strFunctionName">通常是函数名</param>
        /// <param name="hasWeb">是否记录网站日志：true=是，false=否（记录记录应用程序日志）</param>
        public static void Error(Type t, Exception ex,string strMessageObject,string strFunctionName,bool hasWeb)
        {
            string strPath = "";
            if(hasWeb)
            {
                strPath = System.Web.HttpContext.Current.Server.MapPath("//") + "log4net.config";//WEB
            }
            else
            {
                strPath = System.Windows.Forms.Application.StartupPath + "\\log4net.config";//WinForm
            }

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo((strPath)));

            ILog ilog = LogManager.GetLogger(t);
            if(ilog.IsErrorEnabled)
            {
                ilog.Error(strMessageObject, ex);
            }
        }


        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="t">通常是类名</param>
        /// <param name="strMessageObject">通常是方法名或指定记录消息类型</param>
        /// <param name="strFunctionName">通常是函数名</param>
        /// <param name="hasWeb">是否记录网站日志：true=是，false=否（记录记录应用程序日志）</param>
        public static void Debug(Type t, string strMessageObject, string strFunctionName,bool hasWeb)
        {
            string strPath = "";
            if(hasWeb)
            {
                strPath = System.Web.HttpContext.Current.Server.MapPath("//") + "log4net.config";//WEB
            }
            else
            {
                strPath = System.Windows.Forms.Application.StartupPath + "\\log4net.config";//WinForm
            }

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo((strPath)));

            ILog ilog = LogManager.GetLogger(t);
            if(ilog.IsDebugEnabled)
            {
                ilog.Debug(strMessageObject);
            }
        }


        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="t">通常是类名</param>
        /// <param name="strMessageObject">通常是方法名或指定记录消息类型</param>
        /// <param name="strFunctionName">通常是函数名</param>
        /// <param name="hasWeb">是否记录网站日志：true=是，false=否（记录记录应用程序日志）</param>
        public static void Info(Type t, string strMessageObject, string strFunctionName,bool hasWeb)
        {
            string strPath = "";
            if(hasWeb)
            {
                strPath = System.Web.HttpContext.Current.Server.MapPath("//") + "log4net.config";//WEB
            }
            else
            {
                strPath = System.Windows.Forms.Application.StartupPath + "\\log4net.config";//WinForm
            }

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo((strPath)));

            ILog ilog = LogManager.GetLogger(t);
            if(ilog.IsInfoEnabled)
            {
                ilog.Info(strMessageObject);
            }

        }


        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="t">通常是类名</param>
        /// <param name="strMessageObject">通常是方法名或指定记录消息类型</param>
        /// <param name="strFunctionName">通常是函数名</param>
        /// <param name="hasWeb">是否记录网站日志：true=是，false=否（记录记录应用程序日志）</param>
        public static void Warn(Type t, string strMessageObject, string strFunctionName,bool hasWeb)
        {
            string strPath = "";
            if(hasWeb)
            {
                strPath = System.Web.HttpContext.Current.Server.MapPath("//") + "log4net.config";//WEB
            }
            else
            {
                strPath = System.Windows.Forms.Application.StartupPath + "\\log4net.config";//WinForm
            }

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo((strPath)));

            ILog ilog = LogManager.GetLogger(t);
            if(ilog.IsWarnEnabled)
            {
                ilog.Warn(strMessageObject);
            }
        }


        /// <summary>
        /// 记录严重错误日志
        /// </summary>
        /// <param name="t">通常是类名</param>
        /// <param name="strMessageObject">通常是方法名或指定记录消息类型</param>
        /// <param name="strFunctionName">通常是函数名</param>
        /// <param name="hasWeb">是否记录网站日志：true=是，false=否（记录记录应用程序日志）</param>
        public static void Fatal(Type t, string strMessageObject, string strFunctionName,bool hasWeb)
        {
            string strPath = "";
            if(hasWeb)
            {
                strPath = System.Web.HttpContext.Current.Server.MapPath("//") + "log4net.config";//WEB
            }
            else
            {
                strPath = System.Windows.Forms.Application.StartupPath + "\\log4net.config";//WinForm
            }

            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo((strPath)));

            ILog ilog = LogManager.GetLogger(t);
            if(ilog.IsFatalEnabled)
            {
                ilog.Fatal(strMessageObject);
            }
        }


    }
}

