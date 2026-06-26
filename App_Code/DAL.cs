using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Drawing;
using System.Globalization;
using System.Configuration;

public class DAL
{
    public SqlConnection objSQlConnection;
    public SqlConnection objSQlConnection1;
    public SqlDataAdapter sda = new SqlDataAdapter();
    public DataTable dt = new DataTable();
    public HttpRequest request;
    public string AppUrl = "";
    public SqlConnectionStringBuilder Connection = new SqlConnectionStringBuilder();
    public SqlConnectionStringBuilder Connection1 = new SqlConnectionStringBuilder();
    private SqlCommand sqlCmd = new SqlCommand();
    public string activeCondition = "RowStatus='Y'";
    public string tblUserGrpMaster = "M_UserGroupMaster";
    public string tblUserMaster = "M_UserMaster";
    public string tblStateMaster = "M_StateDivMaster";
    public string tblDistrictMaster = "M_DistrictMaster";
    public string tblCityStateMaster = "M_CityStateMaster";
    public string tblBankMaster = "M_BankMaster";
    public string tblNewsMaster = "M_NewsSeminarMaster";
    public string tblNewsTypeMaster = "M_NewsTypeMaster";
    public string tblCountryMaster = "M_CountryMaster";
    public string tblKitMaster = "M_KitMaster";
    public string tblAchieverMaster = "M_AchieverMaster";
    public string tblMeetingMaster = "M_MeetingMaster";
    public string tblUserPermision = "M_UserPermissionMaster";
    public string tblMenuMaster = "M_WebMenuMaster";
    public string tblSearchCriteria = "M_SearchCriteriaMaster";
    public string tblMemberMaster = "M_MemberMaster";
    public string tblKitProductMaster = "M_KitProductMaster";
    public string tblCTypeMaster = "M_ComplaintTypeMaster";
    public string CompID { get; private set; }
    public string dBName { get; private set; }
    public string dBSelectName { get; private set; }
    public string IsoStart { get; private set; }
    public string IsoEnd { get; private set; }
    private string _ConnectionString;
    public DAL()
    {
        //_ConnectionString = strConnectionString;
        //objSQlConnection = new SqlConnection(_ConnectionString);
        
        if (HttpContext.Current != null && HttpContext.Current.Session != null)
        {
            CompID = HttpContext.Current.Session["CompID"].ToString() ?? "";
            dBName = HttpContext.Current.Session["MlmDatabaseName" + CompID].ToString() ?? "";
            dBSelectName = HttpContext.Current.Session["MlmSelectDatabaseName" + CompID].ToString() ?? "";
        }
        Connection.ConnectionString = (string)HttpContext.Current.Session["MlmDatabase" + CompID];
        objSQlConnection = new SqlConnection(Connection.ConnectionString);

        Connection1.ConnectionString = (string)HttpContext.Current.Session["MlmSelectDatabase" + CompID];
        objSQlConnection1 = new SqlConnection(Connection1.ConnectionString);
        IsoStart = " Alter Database " + dBSelectName + " Set Allow_SnapShot_isolation on; Set Transaction Isolation level read uncommitted;Set nocount on; Begin Tran ";

        IsoEnd = " Commit Tran ";
    }
    public int SaveData(string qry)
    {
        int result = 0;
        try
        {
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();

            sqlCmd = new SqlCommand(qry, objSQlConnection);
            result = sqlCmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            // swallow as original did; could log here
            result = 0;
        }
        finally
        {
            if (objSQlConnection != null && objSQlConnection.State == ConnectionState.Open)
            {
                try { objSQlConnection.Close(); } catch { }
            }
        }

        return result;
    }

    public DataTable GetData(string qry)
    {
        DataTable tempDt = null;
        try
        {
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();

            sda = new SqlDataAdapter(qry, objSQlConnection);
            dt = new DataTable();
            sda.Fill(dt);
            tempDt = dt;
        }
        catch (Exception)
        {
            // swallow as original did; could log here
        }
        finally
        {
            if (objSQlConnection != null && objSQlConnection.State == ConnectionState.Open)
            {
                try { objSQlConnection.Close(); } catch { }
            }
        }

        return tempDt;
    }

    public int UpdateData(string qry, string ParaName = "", string ParaValue = "")
    {
        int j = 0;
        try
        {
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();

            if (string.IsNullOrEmpty(ParaName) && string.IsNullOrEmpty(ParaValue))
            {
                sqlCmd = new SqlCommand(qry, objSQlConnection);
            }
            else
            {
                var strParaName = ParaName.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var strParaValue = ParaValue.Split(new[] { ';' }, StringSplitOptions.None);
                sqlCmd = new SqlCommand(qry, objSQlConnection);
                for (int i = 0; i < strParaName.Length && i < strParaValue.Length; i++)
                {
                    sqlCmd.Parameters.AddWithValue(strParaName[i], strParaValue[i]);
                }
            }

            int a = sqlCmd.ExecuteNonQuery();
            j = a;
        }
        catch (Exception)
        {
            j = 0;
        }
        finally
        {
            if (objSQlConnection != null && objSQlConnection.State == ConnectionState.Open)
            {
                try { objSQlConnection.Close(); } catch { }
            }
        }

        return j;
    }

    public int ExecuteProcedure(string procname, string ParaName = "", string ParaValue = "")
    {
        int j = 0;
        SqlCommand cmd = null;
        try
        {
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();

            cmd = objSQlConnection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if (!string.IsNullOrEmpty(ParaName) && !string.IsNullOrEmpty(ParaValue))
            {
                var strParaName = ParaName.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var strParaValue = ParaValue.Split(new[] { ';' }, StringSplitOptions.None);
                for (int i = 0; i < strParaName.Length && i < strParaValue.Length; i++)
                {
                    cmd.Parameters.AddWithValue(strParaName[i], strParaValue[i]);
                }
            }

            cmd.CommandText = procname;
            int a = cmd.ExecuteNonQuery();
            j = a;
        }
        catch (Exception)
        {
            j = 0;
        }
        finally
        {
            if (objSQlConnection != null && objSQlConnection.State == ConnectionState.Open)
            {
                try { objSQlConnection.Close(); } catch { }
            }
        }

        return j;
    }

    public DataTable GenerateTreeProc(string strqry)
    {
        DataTable tempDt = null;
        try
        {
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();

            using (var comm = new SqlCommand(strqry, objSQlConnection))
            {
                comm.CommandTimeout = 100000000;
                using (var ad = new SqlDataAdapter(comm))
                {
                    dt = new DataTable();
                    ad.Fill(dt);
                    tempDt = dt;
                }
            }
        }
        catch (Exception)
        {
            // swallow
        }
        finally
        {
            if (objSQlConnection != null && objSQlConnection.State == ConnectionState.Open)
            {
                try { objSQlConnection.Close(); } catch { }
            }
        }

        return tempDt;
    }

    public DataSet ExecProcDataSet(string strqry)
    {
        var dsGetData = new DataSet();
        try
        {
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();

            using (var comm = new SqlCommand(strqry, objSQlConnection))
            {
                comm.CommandTimeout = 100000000;
                using (var ad = new SqlDataAdapter(comm))
                {
                    ad.Fill(dsGetData);
                }
            }
        }
        catch (Exception)
        {
            // swallow
        }
        finally
        {
            if (objSQlConnection != null && objSQlConnection.State == ConnectionState.Open)
            {
                try { objSQlConnection.Close(); } catch { }
            }
        }

        return dsGetData;
    }

    public int UpdateMultipleFields(string tblName, string ParaName = "", string ParaValue = "", string whereCond = "")
    {
        int j = 0;
        try
        {
            if (objSQlConnection.State == ConnectionState.Closed)
                objSQlConnection.Open();

            string SubPart = string.Empty;
            if (!string.IsNullOrEmpty(ParaName) && !string.IsNullOrEmpty(ParaValue))
            {
                var strParaName = ParaName.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var strParaValue = ParaValue.Split(new[] { ';' }, StringSplitOptions.None);
                for (int i = 0; i < strParaName.Length && i < strParaValue.Length; i++)
                {
                    SubPart += strParaName[i] + "='" + strParaValue[i] + "',";
                }
            }

            if (!string.IsNullOrEmpty(SubPart))
            {
                SubPart = SubPart.Remove(SubPart.Length - 1, 1);
            }

            string qry = " update " + tblName + " set " + SubPart + whereCond;
            sqlCmd = new SqlCommand(qry, objSQlConnection);
            int a = sqlCmd.ExecuteNonQuery();
            j = a;
        }
        catch (Exception)
        {
            j = 0;
        }
        finally
        {
            if (objSQlConnection != null && objSQlConnection.State == ConnectionState.Open)
            {
                try { objSQlConnection.Close(); } catch { }
            }
        }

        return j;
    }

    public string ClearInject(string StrObj)
    {
        try
        {
            if (StrObj == null) return null;
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
        }
        catch (Exception)
        {
            // swallow
        }
        return StrObj;
    }

    public void FillCombo(string qry, DropDownList CmbNm, string DisFld, string ValFld)
    {
        try
        {
            dt = new DataTable();
            dt = GetData(qry);
            if (CmbNm != null && dt != null)
            {
                CmbNm.DataSource = dt;
                CmbNm.DataTextField = DisFld;
                CmbNm.DataValueField = ValFld;
                CmbNm.DataBind();
            }
        }
        catch (Exception)
        {
            // swallow
        }
    }

    public void WriteToFile(string text)
    {
        try
        {
            string path = HttpContext.Current.Server.MapPath("~/images/ErrorLog.txt");

            // original VB inserted into DB via SqlHelper; replicating that call
            string compSessionKey = "MlmDatabase" + HttpContext.Current.Session["CompID"];
            string dbConnString = HttpContext.Current.Session[compSessionKey] != null
                ? HttpContext.Current.Session[compSessionKey].ToString()
                : null;

            string str = " insert Into TrnErrorLog(Pth,txt) Values('" + path + "','" + text + "--CompID = " + HttpContext.Current.Session["CompID"] + "')";
            if (!string.IsNullOrEmpty(dbConnString))
            {
                // SqlHelper used in VB; ensure Microsoft.ApplicationBlocks.Data is referenced if you want to use this
                SqlHelper.ExecuteNonQuery(dbConnString, CommandType.Text, str);
            }
        }
        catch (Exception)
        {
            // swallow
        }
    }
}
