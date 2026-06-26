using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
public partial class WUCMenu : System.Web.UI.UserControl
{
    DataTable dtMenu = new DataTable();
    DataTable dtMenuMain = new DataTable();
    // DAL objDAL = new DAL(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]);
    DAL objDAL;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // Initialize DAL using session key "MlmDatabase" + CompID
            string compId = Session["CompID"]?.ToString();
            string dbSessionKey = "MlmDatabase" + (compId ?? string.Empty);
            objDAL = new DAL();

            // If CompID is not set, redirect to welcome.aspx
            if (compId == null)
            {
                Response.Redirect("welcome.aspx", false);
                return;
            }

            // Load menu
            Load_Menu();

        }
    }
    private void FillDetail()
    {
        try
        {
            string kitid = string.Empty;
            string strquery = objDAL.IsoStart + " select a.KitId,c.kitid as k,formno from " + objDAL.dBName + "..InvoiceZara as a," + objDAL.dBName + "..m_membermaster as c" +
                " where   a.kitid=c.kitid   AND FormNo='" + Session["Formno"] + "' AND c.kitid='" + Session["MemKit"] + "' " + objDAL.IsoEnd;
            DataSet ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]]?.ToString(), CommandType.Text, strquery);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                kit.Text = ds.Tables[0].Rows[0]["KitId"].ToString();
                // kitid = ds.Tables[0].Rows[0]["k"].ToString();
                // kitid = "OK";
                Session["kit"] = "OK";
            }
            else
            {
                Session["MemKit"] = "0";
            }

            // Exec stored proc to determine Session("K")
            string str = " Exec Sp_ShowRedeemMEnu '" + Session["formno"] + "'";
            DataSet ds1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]?.ToString(), CommandType.Text, str);

            // default to "Y" unless the proc returns a numeric K != 0
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                object kObj = ds1.Tables[0].Rows[0]["K"];
                int kVal = 0;
                int.TryParse(kObj?.ToString(), out kVal);

                if (kVal == 0)
                    Session["K"] = "Y";
                else
                    Session["K"] = "N";
            }
            else
            {
                // matching original code's assumption: if nothing returned, consider "Y"
                Session["K"] = "Y";
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            // objDAL must be available as a class-level variable (as in your VB code)
            objDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void Load_Menu()
    {
        string str;
        try
        {
            StringBuilder html = new StringBuilder();

            string Key = "6b04d38748f94490a636cf1be3d82841";
            string IV = "f8adbf3c94b7463d";

            byte[] KeyB = null;
            byte[] IVB = null;
            string LogKey = string.Empty;

            try
            {
                string compId = HttpContext.Current.Session["CompId"]?.ToString();
                if (compId == "1077")
                {
                    // do nothing for this comp id as in original VB
                }
                else
                {
                    KeyB = Encoding.ASCII.GetBytes(Key);
                    IVB = Encoding.ASCII.GetBytes(IV);

                    // Encrypt should be a method available in this class (converted to C#)
                    string raw = HttpContext.Current.Session["LoginKey"]?.ToString() + "|" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    LogKey = CryptoHelper.Encrypt(raw, KeyB, IVB);
                    LogKey = "data=" + HttpContext.Current.Server.UrlEncode(LogKey);
                }
            }
            catch (Exception)
            {
                // swallow to match original behavior
            }

            string CashBack = string.Empty;
            string LoginUtility = string.Empty;
            string ShoppingPortal = string.Empty;
            string ShoppingPortal1 = string.Empty;
            string MoviePortal = string.Empty;
            string Mart = string.Empty;
            string MartLogin = string.Empty;

            string comp = HttpContext.Current.Session["compid"]?.ToString();
            if (comp == "1077")
            {
                // nothing
            }
            else
            {
                MoviePortal = Base64Helpers.Base64Encode(HttpContext.Current.Session["MoviePortal"]?.ToString() ?? string.Empty);
                ShoppingPortal1 = "ShoppingRedirect.aspx";

                CashBack = "user_info=" + Base64Helpers.Base64Encode(HttpContext.Current.Session["CashBack"]?.ToString() ?? string.Empty)
                           + "&log_key=" + HttpContext.Current.Session["RLoginCashBack"]?.ToString();

                Mart = "mod=" + HttpContext.Current.Session["RMartMod"]?.ToString()
                       + "&" + HttpContext.Current.Session["Mart"]?.ToString()
                       + "&token=" + HttpContext.Current.Session["RLoginMart"]?.ToString();

                LoginUtility = "url=" + Base64Helpers.Base64Encode((HttpContext.Current.Session["LoginUtility"]?.ToString() ?? string.Empty)
                               + "&Token=" + HttpContext.Current.Session["RLoginUtility"]?.ToString());

                ShoppingPortal = "mod=" + HttpContext.Current.Session["RShopMod"]?.ToString()
                                 + "&" + HttpContext.Current.Session["Shopping"]?.ToString()
                                 + "&token=" + HttpContext.Current.Session["RLoginShopping"]?.ToString();

                MartLogin = "MartLogin.aspx";
            }

            DataSet ds = new DataSet();
            str = " Select a.MenuId as MenuId, a.MenuName as MenuName,a.ParentId as ParentId, " +
                  "  Replace(Replace(Replace(Replace(Replace(Replace(Replace(a.OnSelect,'prams','" + LogKey +
                  "'),'CashBack','" + CashBack + "'),'LogUti','" + LoginUtility + "'),'ShopUti','" + ShoppingPortal +
                  "'),'ShopPorNew','" + ShoppingPortal1 + "'),'UserIDMovie','" + MoviePortal + "'),'MartLogin','" + MartLogin + "') as OnSelect" +
                  " from M_CompWiseWebMenuMasterDis a Where a.ActiveStatus = 'Y' And a.RowStatus ='Y' And a.CompanyID = '" +
                  HttpContext.Current.Session["CompID"]?.ToString() + "' order by Convert(decimal,RTRIM(LTRIM(a.Hierar))),a.MenuId";

            // Note: In VB you used Application("sConnect") - keep same here (ensure Application["sConnect"] is a valid connection string)
            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Application["sConnect"]?.ToString(), CommandType.Text, str);

            dtMenu = (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : new DataTable();
            HttpContext.Current.Session["Menu"] = dtMenu;

            if (dtMenu.Rows.Count > 0)
            {
                foreach (DataRow dr in dtMenu.Rows)
                {
                    string parentId = dr["ParentId"]?.ToString();
                    if (!String.Equals(dr["MenuName"]?.ToString(), "-"))
                    {
                        if (int.TryParse(parentId, out int pid) && pid == 0)
                        {
                            string mainMenu;
                            string subMenu = Load_SubMenu(dr["MenuId"], dtMenu); // Ensure this method exists in C# and returns string

                            string onSelect = dr["OnSelect"]?.ToString() ?? string.Empty;
                            if (string.IsNullOrEmpty(onSelect) || onSelect == "#")
                            {
                                mainMenu = "<li class=\"dropdown\">" +
            "<a class=\"menu-toggle nav-link has-dropdown\">" +
            "<i data-feather=\"monitor\"></i>" +
            "<span>" + dr["MenuName"] + "</span></a>" +
            "<ul class=\"dropdown-menu\">" + subMenu + "</ul>" +
            "</li>";
                            }
                            else
                            {
                                mainMenu = "<li class=\"dropdown\"><a href=\"" + onSelect + "\" class=\"nav-link\"><i data-feather=\"calendar\"></i><span>" + dr["MenuName"] + "</span></a></li>";
                            }

                            html.Append(mainMenu);
                        }
                    }
                }
            }

            // finally set the inner html of the menu control (assumes menu is an HtmlGenericControl with runat=server)
            menu.InnerHtml = html.ToString();
        }
        catch (Exception ex)
        {
            // swallow as original did; optionally log:
            // objDAL?.WriteToFile(ex.Message);
        }
    }
    private string Load_SubMenu(object MenuIdObj, DataTable dt)
    {
        var html = new StringBuilder();

        try
        {
            if (dt == null || dt.Rows.Count == 0)
                return html.ToString();

            // Convert MenuId to integer if possible
            int menuId = 0;
            if (MenuIdObj != null)
                int.TryParse(MenuIdObj.ToString(), out menuId);

            foreach (DataRow dr in dt.Rows)
            {
                string parentIdStr = dr["ParentId"]?.ToString() ?? string.Empty;
                if (!int.TryParse(parentIdStr, out int parentId))
                    parentId = 0;

                // Only consider rows that are children of supplied MenuId and have a visible name
                if (parentId != 0 && parentId == menuId && !string.Equals(dr["MenuName"]?.ToString(), "-"))
                {
                    string onSelect = dr["OnSelect"]?.ToString() ?? string.Empty;
                    string menuName = dr["MenuName"]?.ToString() ?? string.Empty;

                    string compId = HttpContext.Current.Session["CompId"]?.ToString();
                    string customerType = HttpContext.Current.Session["customertype"]?.ToString();
                    string memKit = HttpContext.Current.Session["MemKit"]?.ToString();
                    string sessionK = HttpContext.Current.Session["K"]?.ToString();
                    string registrationType = HttpContext.Current.Session["registrationtype"]?.ToString();

                    // Helper to append a hidden LI
                    void AppendHidden() => html.AppendFormat("<li style='display:none;'><a href=\"{0}\" >{1}</a></li>    ", onSelect, menuName);
                    void AppendNormal() => html.AppendFormat("<li><a href=\"{0}\" >{1}</a></li>   ", onSelect, menuName);

                    if (compId == "1057")
                    {
                        if (customerType == "C" && menuName == "My Agent")
                        {
                            AppendHidden();
                        }
                        else if (customerType == "A" && (menuName == "ID Activation" || menuName == "Activation Detail"))
                        {
                            AppendHidden();
                        }
                        else if (memKit == "0" && menuName == "Invoice")
                        {
                            AppendHidden();
                        }
                        else if (sessionK == "Y" && menuName == "Redemption Voucher")
                        {
                            AppendHidden();
                        }
                        else
                        {
                            AppendNormal();
                        }
                    }
                    else if (compId == "1093")
                    {
                        // Map registrationtype to menu text variants
                        if (registrationType == "1" && menuName == "Update Rank")
                        {
                            html.AppendFormat("<li><a href=\"{0}\" >Master Registration</a></li>    ", onSelect);
                        }
                        else if (registrationType == "2" && menuName == "Update Rank")
                        {
                            html.AppendFormat("<li><a href=\"{0}\" >Agency Registration</a></li>    ", onSelect);
                        }
                        else if (registrationType == "3" && menuName == "Update Rank")
                        {
                            html.AppendFormat("<li><a href=\"{0}\" >Agent Registration</a></li>    ", onSelect);
                        }
                        else if (registrationType == "4" && menuName == "Update Rank")
                        {
                            html.AppendFormat("<li><a href=\"{0}\" >Emall Registration</a></li>    ", onSelect);
                        }
                        else if ((registrationType == "5" || registrationType == "6") && menuName == "Update Rank")
                        {
                            // hide for types 5 & 6
                            AppendHidden();
                        }
                        else
                        {
                            html.AppendFormat("<li ><a href=\"{0}\" >{1}</a></li>   ", onSelect, menuName);
                        }
                    }
                    else
                    {
                        if (customerType == "C" && menuName == "My Agent")
                        {
                            AppendHidden();
                        }
                        else if (customerType == "A" && (menuName == "ID Activation" || menuName == "Activation Detail"))
                        {
                            AppendHidden();
                        }
                        else
                        {
                            AppendNormal();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Keep behavior similar to original (writes to response). Consider logging instead.
            HttpContext.Current.Response.Write(ex.Message);
        }

        return html.ToString();
    }
}