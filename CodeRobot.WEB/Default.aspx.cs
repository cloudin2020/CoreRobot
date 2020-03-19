using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            string strConn = "Server=101.201.45.142;Database=buddhadb;Uid=root;Pwd=53344521;CharSet=utf8";

            MySqlConnection cn = new MySqlConnection(strConn);
            cn.Open();
            string strSql = "SELECT id FROM bu_dynamic_t ORDER BY id DESC LIMIT 0,1";
            MySqlCommand cmd = new MySqlCommand(strSql, cn);
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                //
            }
            dr.Dispose();
            cn.Close();
        }
    }
}