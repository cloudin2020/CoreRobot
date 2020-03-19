using System;
using System.Collections.Generic;
using System.Text;

namespace CodeRobot.Utility
{
    /// <summary>
    /// 通用字符存储类
    /// </summary>
    public class PublicValue
    {
        //全局消息
        public static string m_strMessage;

        /// <summary>
        /// 存储消息
        /// </summary>
        /// <param name="strMsg">消息值</param>
        public static void SaveMessage(string strMsg)
        {
            m_strMessage = strMsg + "\n" + m_strMessage;
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <returns>返回消息值</returns>
        public static string GetMessage()
        {
            return m_strMessage;
        }


        public static void SaveTable(string strValue)
        {
            CommonHelper.SaveCookie("CodeRobo_Table", strValue, 1);
        }

        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <returns>返回用户名</returns>
        public static string GetTable()
        {
            return CommonHelper.GetCookieString("CodeRobo_Table", true);
        }
    }
}
