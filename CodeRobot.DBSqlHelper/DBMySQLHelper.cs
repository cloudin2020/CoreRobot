using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace CodeRobot.DBSqlHelper
{
    /// <summary>
    /// 数据连接类
    /// </summary>
    public class DBMySQLHelper
    {
        private static MySqlConnection cn;

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        public static string ConnectionMySQL()
        {
            CodeRobot.Utility.IniFile iniFile = new Utility.IniFile(Application.StartupPath + "\\config.ini");

            string strServer = iniFile.GetString("BASE", "SERVER", "");//数据库地址
            string strDBName = iniFile.GetString("BASE", "DBNAME", "");//数据库名
            string strDBUser = iniFile.GetString("BASE", "USERID", "");//登录数据库用户名
            string strDBPassword = iniFile.GetString("BASE", "PASSWORD", "");//登录数据库密码
            string strCharset = iniFile.GetString("BASE", "CHARSET", "");//数据库字符集

            //解密
            //strServer = CodeRobot.Utility.CryptographyHelper.Decrypt(strServer, CodeRobot.Utility.CommonHelper.cryptographyKey);
            //strDBName = CodeRobot.Utility.CryptographyHelper.Decrypt(strDBName, CodeRobot.Utility.CommonHelper.cryptographyKey);
            //strDBUser = CodeRobot.Utility.CryptographyHelper.Decrypt(strDBUser, CodeRobot.Utility.CommonHelper.cryptographyKey);
            //strDBPassword = CodeRobot.Utility.CryptographyHelper.Decrypt(strDBPassword, CodeRobot.Utility.CommonHelper.cryptographyKey);

            //拼接连接字符串
            string strConnectionStr = "Server=" + strServer + ";Uid=" + strDBUser + ";Pwd=" + strDBPassword + ";CharSet=" + strCharset;

            return strConnectionStr;
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns>返回MySqlConnection实例</returns>
        public static MySqlConnection GetCon()
        {
            if (cn == null)
            {
                cn = new MySqlConnection(ConnectionMySQL());
            }
            return cn;
        }

        /// <summary>
        /// 执行数据库操作语句
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>返回影响行数</returns>
        public static int ExecuteSql(string strSql)
        {
            int iNum = -1;
            MySqlConnection cn = DBMySQLHelper.GetCon();
            MySqlCommand cm = new MySqlCommand(strSql, cn);
            try
            {
                cn.Open();
                iNum = cm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(DBMySQLHelper), ex, "执行数据库操作语句", "ExecuteSql", false);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
            return iNum;
        }

        /// <summary>
        /// 函数输入SQL语句，输出一个结果集（DATATABLE）
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>返回 DataTable</returns>
        public static DataTable GetTable(string strSql)
        {
            DataTable dt = new DataTable();
            MySqlConnection cn = DBMySQLHelper.GetCon();
            try
            {
                cn.Open();
                MySqlCommand cm = new MySqlCommand(strSql, cn);
                MySqlDataAdapter da = new MySqlDataAdapter(cm);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(DBMySQLHelper), ex, "函数输入SQL语句，输出一个结果集（DATATABLE）", "GetTable", false);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
            return dt;
        }

        /// <summary>
        /// 函数输入SQL语句，返回一个SQLiteDataReader结果集
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>返回 SQLiteDataReader 实例</returns>
        public static MySqlDataReader ExReader(string strSql)
        {
            MySqlConnection cn = DBMySQLHelper.GetCon();
            MySqlDataReader dr = null;
            try
            {
                cn.Open();
                MySqlCommand cm = new MySqlCommand(strSql, cn);
                dr = cm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                CodeRobot.Utility.LogHelper.Error(typeof(DBMySQLHelper), ex, "函数输入SQL语句，返回一个OleDbDataReader结果集", "MySqlDataReader", false);
            }

            return dr;
        }
    }
}
