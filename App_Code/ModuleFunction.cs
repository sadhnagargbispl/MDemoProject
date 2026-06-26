using System;
using System.Data;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;

public class ModuleFunction
{
    private DataTable dt;
    private DAL objDal;

    // Optional parameterless ctor (commented out in your VB). Uncomment if needed.
    // public ModuleFunction()
    // {
    //     objDal = new DAL(HttpContext.Current.Session["MlmDatabase" + HttpContext.Current.Session["CompID"]].ToString());
    // }

    public ModuleFunction(string strConnectionString)
    {
        objDal = new DAL();
    }

    /// <summary>
    /// Mimics the VB EncodeBase64 method: trims input, replaces spaces with '+',
    /// and pads to a multiple of 4 using '='. Note: this does not Base64-encode the input.
    /// </summary>
    public string EncodeBase64(string data)
    {
        if (data == null) return null;

        string s = data.Trim().Replace(" ", "+");
        int rem = s.Length % 4;
        if (rem > 0)
        {
            s = s.PadRight(s.Length + (4 - rem), (char)61); // 61 == '='
        }
        return s;
    }

    public void FillCombo(string qry, DropDownList ddlToBeFill, string dataText = "", string dataValue = "")
    {
        dt = new DataTable();
        dt = objDal.GetData(qry);
        ddlToBeFill.DataSource = dt;
        ddlToBeFill.DataTextField = dataText;
        ddlToBeFill.DataValueField = dataValue;
        ddlToBeFill.DataBind();
    }

    /// <summary>
    /// Method to get client IP address.
    /// set GetLan = true if you want to get local (LAN) connected IP address.
    /// </summary>
    public string GetVisitorIPAddress(bool GetLan = false)
    {
        string visitorIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (string.IsNullOrEmpty(visitorIPAddress))
        {
            visitorIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        if (string.IsNullOrEmpty(visitorIPAddress))
        {
            visitorIPAddress = HttpContext.Current.Request.UserHostAddress;
        }

        if (string.IsNullOrEmpty(visitorIPAddress) || visitorIPAddress.Trim() == "::1")
        {
            GetLan = true;
            visitorIPAddress = string.Empty;
        }

        if (GetLan)
        {
            if (string.IsNullOrEmpty(visitorIPAddress))
            {
                // This is for local (LAN) connected IP address
                string stringHostName = Dns.GetHostName();
                IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                try
                {
                    visitorIPAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
                }
                catch
                {
                    try
                    {
                        visitorIPAddress = arrIpAddress[0].ToString();
                    }
                    catch
                    {
                        try
                        {
                            arrIpAddress = Dns.GetHostAddresses(stringHostName);
                            visitorIPAddress = arrIpAddress[0].ToString();
                        }
                        catch
                        {
                            visitorIPAddress = "127.0.0.1";
                        }
                    }
                }
            }
        }

        return visitorIPAddress;
    }
}
