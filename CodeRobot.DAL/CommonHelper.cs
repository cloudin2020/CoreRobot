using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace CodeRobot.DAL
{
    /// <summary>
    /// 通用模块
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// 获取表中的主键ID
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetPrimaryKey(string strTableName)
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
                    strColumnComment = GetColumnKeyComment(strColumnComment);

                    if (strColumnKey == "PRI")
                    {
                        strReturnValue = strColumnName;
                        break;
                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中的主键ID", "GetPrimaryKey",false);
            }

            return strReturnValue;

        }


        /// <summary>
        /// 获取表注释，如：新闻表
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns></returns>
        public static string GetTableComment(string strTableName)
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
                    strColumnComment = GetColumnKeyComment(strColumnComment);

                    if (!string.IsNullOrEmpty(strColumnComment))
                    {
                        strColumnComment = strColumnComment.Replace("表ID","");
                    }

                    if (strColumnKey == "PRI")
                    {
                        strReturnValue = strColumnComment;
                        break;
                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表注释", "GetTableComment",false);
            }

            return strReturnValue;

        }

        /// <summary>
        /// 处理表中的列名注释
        /// </summary>
        /// <param name="strColumnComment">原列名注释</param>
        /// <returns></returns>
        public static string GetColumnKeyComment(string strColumnComment)
        {
            string strReturnString = "";

            try
            {
                strColumnComment = strColumnComment.Replace("{下拉}", "");
                strColumnComment = strColumnComment.Replace("{勾选}", "");
                strColumnComment = strColumnComment.Replace("{单选}", "");
                strColumnComment = strColumnComment.Replace("{复选}", "");
                strColumnComment = strColumnComment.Replace("{图片}", "");
                strColumnComment = strColumnComment.Replace("{搜索}", "");
                strColumnComment = strColumnComment.Replace("{搜索}", "");
                strColumnComment = strColumnComment.Replace("{TEXTAREA}", "");
                strColumnComment = strColumnComment.Replace("{MutiPages}", "");
                strColumnComment = strColumnComment.Replace("{ParentPages}", "");

                if (strColumnComment.Contains("]"))
                {
                    string[] splitStr = strColumnComment.Split(new char[] { ']' });
                    strReturnString = splitStr[1];
                }
                else if (strColumnComment.Contains(":"))
                {
                    string[] splitStr = strColumnComment.Split(new char[] { ':' });
                    strReturnString = splitStr[0];
                }
                else
                {
                    strReturnString = strColumnComment;
                }

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CommonHelper), ex, "处理表中的列名注释", "GetColumnKeyComment",false);
            }

            return strReturnString;
        }

        public static string GetColumnKeyCommentRight(string strColumnComment)
        {
            string strReturnString = "";

            try
            {
                strColumnComment = strColumnComment.Replace("{下拉}", "");
                strColumnComment = strColumnComment.Replace("{勾选}", "");
                strColumnComment = strColumnComment.Replace("{单选}", "");
                strColumnComment = strColumnComment.Replace("{复选}", "");
                strColumnComment = strColumnComment.Replace("{图片}", "");
                strColumnComment = strColumnComment.Replace("{搜索}", "");
                strColumnComment = strColumnComment.Replace("{搜索}", "");
                strColumnComment = strColumnComment.Replace("{TEXTAREA}", "");
                strColumnComment = strColumnComment.Replace("{MutiPages}", "");
                strColumnComment = strColumnComment.Replace("{ParentPages}", "");

                if (strColumnComment.Contains("]"))
                {
                    string[] splitStr = strColumnComment.Split(new char[] { ']' });
                    strReturnString = splitStr[1];
                }
                else if (strColumnComment.Contains(":"))
                {
                    string[] splitStr = strColumnComment.Split(new char[] { ':' });
                    strReturnString = splitStr[1];
                }
                else
                {
                    strReturnString = strColumnComment;
                }

            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CommonHelper), ex, "处理表中的列名注释", "GetColumnKeyComment", false);
            }

            return strReturnString;
        }


        /// <summary>
        /// 生成首字母大写表名
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetTableNameUpper(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                if (strTableName.Contains("_"))
                {
                    string[] splitTableName = strTableName.Split(new char[] { '_' });
                    string strWordOne = splitTableName[0].ToString();
                    string strWordTwo = splitTableName[1].ToString();

                    string strNewWordOne = strWordOne.Substring(0, 1);
                    strWordOne = strNewWordOne.ToUpper() + strWordOne.Substring(1);

                    string strNewstrWordTwo = strWordTwo.Substring(0, 1);
                    strWordTwo = strNewstrWordTwo.ToUpper() + strWordTwo.Substring(1);

                    strReturnValue = strWordOne + strWordTwo;

                }
                else
                {
                    string strWords = strTableName.Substring(0, 1);
                    strWords = strWords.ToUpper() + strTableName.Substring(1);
                    strReturnValue = strWords;
                }
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CommonHelper), ex, "生成首字母大写表名", "GetTableNameUpper",false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 根据生成首字母大写的表名，再次把首字母强制为小写,-后为大写
        /// </summary>
        /// <param name="strTableNameUpper"></param>
        /// <returns></returns>
        public static string GetTableNameFirstLowerSecondUpper(string strTableNameUpper)
        {
            string strReturnValue = "";

            try
            {
                string strNewWordOne = strTableNameUpper.Substring(0, 1);
                strTableNameUpper = strNewWordOne.ToLower() + strTableNameUpper.Substring(1, strTableNameUpper.Length - 1);

                strReturnValue = strTableNameUpper;
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CommonHelper), ex, "根据生成首字母大写的表名，再次把首字母强制为小写", "GetTableNameFirstLower", false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取类名，过滤“_”前面的字符
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetClassName(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                if (strTableName.Contains("_"))
                {
                    string[] splitNames = strTableName.Split(new char[] { '_' });
                    if (splitNames.Length == 2)
                    {
                        strReturnValue = splitNames[1];
                    }
                    if (splitNames.Length == 3)
                    {
                        string strBehindStr = splitNames[2];
                        strBehindStr = GetTableNameUpper(strBehindStr);
                        strReturnValue = splitNames[1] + strBehindStr;
                    }

                }
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CommonHelper), ex, "获取类名", "GetClassName",false);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取首字母小写表名
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetTableNameFirtWordLower(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                if (strTableName.Contains("_"))
                {
                    string[] splitNames = strTableName.Split(new char[] { '_' });
                    string strBehind = splitNames[1].ToString();

                    string strNewWordOne = strBehind.Substring(0, 1);
                    strNewWordOne = strNewWordOne.ToLower();
                    strReturnValue = strNewWordOne + strBehind.Substring(1, strBehind.Length - 1);
                }
                else
                {
                    string strNewWordOne = strTableName.Substring(0, 1);
                    strNewWordOne = strNewWordOne.ToLower();
                    strReturnValue = strNewWordOne + strTableName.Substring(1, strTableName.Length - 1);
                }
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CommonHelper), ex, "获取首字母小写表名", "GetTableNameFirtWordLower",false);
            }

            return strReturnValue;
        }


        /// <summary>
        /// 获取表中所有字段，不包含主键
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetAllColumnNameNotKey(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    if (strColumnKey != "PRI")
                    {
                        strReturnValue += strColumnName + ",";

                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段", "GetAllColumnName", false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        public static string GetSpecialColumnNameNotKey(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    if (strColumnKey != "PRI")
                    {
                        if (!strColumnComment.Contains("{添加排除}"))
                        {
                            strReturnValue += strColumnName + ",";
                        }

                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段", "GetAllColumnName", false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取表中所有字段
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetAllColumnName(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    strReturnValue += strColumnName + ",";
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段", "GetAllColumnName", false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取表中部分字段，过滤掉排除的
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetSpecialColumnName(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    if (!strColumnComment.Contains("{添加排除}"))
                    {
                        strReturnValue += strColumnName + ",";
                    }
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段", "GetAllColumnName", false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取表中所有字段@
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetAllColumnNameForAt(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    if (strColumnKey != "PRI")
                    {
                        strReturnValue += "@" + strColumnName + ",";
                    }
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段@", "GetAllColumnNameForAt", false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        public static string GetSpecialColumnNameForAt(string strTableName)
        {
            string strReturnValue = "";

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME,COLUMN_KEY,COLUMN_COMMENT FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string strColumnName = dr["COLUMN_NAME"].ToString();//字段列名
                    string strColumnKey = dr["COLUMN_KEY"].ToString().Trim();//是否是主键
                    string strColumnComment = dr["COLUMN_COMMENT"].ToString();

                    if (strColumnKey != "PRI")
                    {
                        if (!strColumnComment.Contains("{添加排除}"))
                        {
                            strReturnValue += "@" + strColumnName + ",";
                        }

                    }
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中所有字段@", "GetAllColumnNameForAt", false);
            }

            if (!string.IsNullOrEmpty(strReturnValue))
            {
                strReturnValue = strReturnValue.Substring(0, strReturnValue.Length - 1);
            }

            return strReturnValue;
        }

        /// <summary>
        /// 获取表中字段数量
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static int GetColumnNum(string strTableName)
        {
            int nNum = 0;

            try
            {
                MySqlConnection cn = new MySqlConnection(CodeRobot.DBSqlHelper.DBMySQLHelper.ConnectionMySQL());
                cn.Open();
                string strSql = "SELECT COLUMN_NAME FROM `information_schema`.`COLUMNS` WHERE TABLE_NAME='" + strTableName + "'";
                MySqlCommand cmd = new MySqlCommand(strSql, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    nNum++;
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中字段数量", "GetColumnNum", false);
            }

            //减去自增主键
            nNum = nNum - 1;

            return nNum;
        }


        /// <summary>
        /// 获取表中的Name字段
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <returns></returns>
        public static string GetKeyNameForSelect(string strTableName)
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
                    strColumnComment = GetColumnKeyComment(strColumnComment);

                    if (strColumnName.Contains("_name"))
                    {
                        strReturnValue = strColumnName;
                        break;
                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "获取表中的Name字段", "GetKeyNameForSelect", false);
            }

            return strReturnValue;

        }


        /// <summary>
        /// 检测字段是否存在
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strCheckColumnName"></param>
        /// <returns></returns>
        public static bool ChecktCreatedAtKey(string strTableName, string strCheckColumnName)
        {
            bool bValue = false;

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
                    string strColumnType = dr["COLUMN_TYPE"].ToString();//注释

                    if (strColumnName == strCheckColumnName)
                    {
                        bValue = true;
                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "检测字段是否存在", "ChecktCreatedAtKey", false);
            }

            return bValue;

        }

        /// <summary>
        /// 检测是否bigint
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strPrimaryKey"></param>
        /// <returns></returns>
        public static bool ChecktKeyIsBigint(string strTableName, string strPrimaryKey)
        {
            bool bValue = false;

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
                    string strColumnType = dr["COLUMN_TYPE"].ToString();//注释

                    if (strColumnName == strPrimaryKey)
                    {
                        if (strColumnKey == "bigint")
                        {
                            bValue = true;
                        }

                    }

                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "检测字段是否存在", "ChecktCreatedAtKey", false);
            }

            return bValue;

        }

        /// <summary>
        /// 获取生成日志字段
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="newTable"></param>
        /// <returns></returns>
        public static string GetSaveLogColumnName(string strTableName,string newTable)
        {
            string returnValue = "";

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
                    string strColumnType = dr["COLUMN_TYPE"].ToString();//注释

                    returnValue += "\","+ strColumnName + "=\" +"+ newTable + "." + strColumnName + "+";
                }
                dr.Dispose();
                cn.Close();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(CodeHelper), ex, "检测字段是否存在", "GetSaveLogColumnName", false);
            }

            if (returnValue.Length>0)
            {
                returnValue = returnValue.Substring(0, returnValue.Length - 1);
            }

            return returnValue;

        }
    }
}
