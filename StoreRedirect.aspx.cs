using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StoreRedirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string formPostText = "";
            string compId = Convert.ToString(Session["CompID"]);
            formPostText =
                "<form method=\"POST\" action=\"https://store.makeandgrowth.com/members/index.php\" name=\"frm2Post\">" +
                "<input type=\"hidden\" name=\"token\" value=\"453ecd0dca082bc94cac8d06406305f1\" />" +
                "<input type=\"hidden\" name=\"mod\" value=\"interLogin\" />" +
                "<input type=\"hidden\" name=\"userid\" value=\"" + Session["IDNo"] + "\" />" +
                "<input type=\"hidden\" name=\"password\" value=\"" + Session["MemPassw"] + "\" />" +
                "<script type=\"text/javascript\">document.frm2Post.submit();</script>" +
                "</form>";
            Response.Write(formPostText);
        }
        catch (Exception)
        {
            // silently handled as in VB code
        }
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