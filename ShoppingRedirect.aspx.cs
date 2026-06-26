using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Web.Configuration;

public partial class ShoppingRedirect : System.Web.UI.Page
{
    DataTable dt;
    DAL Obj;
    clsGeneral objGen = new clsGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        string url = "";
        string ref11 = "Login";
        string info1 = Session["IDNo"].ToString() + ";" + Session["MemPassw"].ToString();
        string red = Base64Encode(ref11);
        string ww = Base64Encode(info1);
        if (Session["compid"].ToString() == "1106")
        {
            url = "https://myhemalika.com/Account/Directlogin?refs=" + red + "&info=" + ww;
        }
        else
        {
            url = "https://makeandgrowth.com/Account/Directlogin?refs=" + red + "&info=" + ww;
        }

        Response.Redirect(url);
    }

    private string Base64Encode(string plainText)
    {
        byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    private string Base64Decode(string base64EncodedData)
    {
        byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
