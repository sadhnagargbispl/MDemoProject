using System;
using System.Data;
using System.Data.SqlClient;

public class cls_DataAccess
{
    public SqlConnection cnnObject;
    public string _SerucityCode = "";
    private SqlCommand cmd;
    private SqlTransaction tran;
    private string _ConnectionString;

    // Event to mimic VB's Event ConnectionOpen()
    public event EventHandler ConnectionOpen;

    public cls_DataAccess(string strConnectionString)
    {
        // _SerucityCode = strSecurityCode;
        _ConnectionString = strConnectionString;
    }

    public string ClearInject(string StrObj)
    {
        if (StrObj == null) return null;
        StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
        return StrObj;
    }

    public SqlConnection OpenConnection()
    {
        try
        {
            if (cnnObject == null)
            {
                cnnObject = new SqlConnection(_ConnectionString);
            }

            if (cnnObject.State == ConnectionState.Closed || cnnObject.State == ConnectionState.Broken)
            {
                cnnObject.Open();
                ConnectionOpen?.Invoke(this, EventArgs.Empty);
            }

            return cnnObject;
        }
        catch (Exception)
        {
            // Still raise the event like VB did
            ConnectionOpen?.Invoke(this, EventArgs.Empty);
            return null;
        }
    }

    public void CloseConnection()
    {
        try
        {
            if (cnnObject != null && cnnObject.State == ConnectionState.Open)
            {
                cnnObject.Close();
            }
        }
        catch (Exception)
        {
            // swallow as original
        }
    }

    public string ExecuteScaller_old(string strQuery)
    {
        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        using (var cmdLocal = new SqlCommand())
        {
            try
            {
                cmdLocal.Connection = cnnObject;
                cmdLocal.CommandType = CommandType.Text;
                cmdLocal.CommandText = strQuery;

                object result = cmdLocal.ExecuteScalar();
                return result != null ? result.ToString() : "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

    public string ExistOrNot(string strQuery)
    {
        string _returnValue = "";

        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        var da = new SqlDataAdapter(strQuery, cnnObject);
        var DTable = new DataTable();

        try
        {
            da.Fill(DTable);

            if (DTable.Rows.Count > 0)
            {
                _returnValue = DTable.Rows[0][0].ToString();
            }
        }
        catch (Exception)
        {
            // swallow like original
        }
        finally
        {
            da.Dispose();
        }

        return _returnValue;
    }

    // Note: keeps signature close to VB: returns DataTable and uses ref param for DTable
    public DataTable Fill_Data_Tables(string strQuery,  DataTable DTable)
    {
        if (cnnObject == null)
        {
            OpenConnection();
        }

        var da = new SqlDataAdapter(strQuery, cnnObject);
        DTable = new DataTable();

        try
        {
            da.Fill(DTable);
        }
        catch (Exception)
        {
            DTable = null;
            return null;
        }
        finally
        {
            da.Dispose();
        }

        return DTable;
    }

    // VB: Fill_Data_Tables_new(ByRef tran As SqlTransaction, ByVal strQuery As String, ByRef DTable As DataTable)
    public DataTable Fill_Data_Tables_new(SqlTransaction tranRef, string strQuery, ref DataTable DTable)
    {
        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        var da = new SqlDataAdapter(strQuery, cnnObject);
        DTable = new DataTable();

        try
        {
            da.Fill(DTable);
        }
        catch (Exception)
        {
            DTable = null;
            return null;
        }
        finally
        {
            da.Dispose();
        }

        return DTable;
    }

    public void Fill_DataSET_Tables(string strQuery, ref DataSet DS, string strTableName)
    {
        if (cnnObject == null)
        {
            OpenConnection();
        }

        var da = new SqlDataAdapter(strQuery, cnnObject);

        try
        {
            da.Fill(DS, strTableName);
        }
        catch (Exception)
        {
            // swallow as original
        }
        finally
        {
            da.Dispose();
        }
    }

    public string returnRandom(int iLen)
    {
        string returnRandom = "";
        if (cnnObject == null)
        {
            OpenConnection();
        }

        SqlDataReader dRead = null;
        try
        {
            using (var cmm = new SqlCommand($"select Left(NewId(),{iLen}) as RandomNum", cnnObject))
            {
                dRead = cmm.ExecuteReader();
                if (dRead.Read() == false)
                {
                    dRead.Close();
                }
                else
                {
                    returnRandom = dRead["RandomNum"].ToString();
                    dRead.Close();
                    return returnRandom;
                }
            }
        }
        catch (Exception)
        {
            // swallow as original
        }
        finally
        {
            dRead?.Dispose();
        }

        return returnRandom;
    }

    public int Fire_Query(string Query)
    {
        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        int affectedRows = 0;

        try
        {
            tran = cnnObject.BeginTransaction();
            cmd = new SqlCommand(Query, cnnObject)
            {
                CommandTimeout = 0,
                Transaction = tran
            };

            affectedRows += cmd.ExecuteNonQuery();
            tran.Commit();
            return affectedRows;
        }
        catch (Exception e)
        {
            try
            {
                tran?.Rollback();
            }
            catch { /* swallow */ }

            throw e;
        }
        finally
        {
            cmd = null;
        }
    }

    public int Fire_Query_For_Procedure(string Query)
    {
        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        int affectedRows = 0;

        try
        {
            tran = cnnObject.BeginTransaction();
            cmd = new SqlCommand(Query, cnnObject)
            {
                CommandTimeout = 0,
                Transaction = tran
            };
            cmd.ExecuteNonQuery();
            tran.Commit();
            affectedRows = 1;
            return affectedRows;
        }
        catch (Exception e)
        {
            try
            {
                tran?.Rollback();
            }
            catch { /* swallow */ }

            throw e;
        }
        finally
        {
            cmd = null;
        }
    }

    public DateTime Get_ServerDate()
    {
        string SqlD = "Select Cast(Convert(Varchar,Getdate(),106) as DateTime) as Dts";
        var SqlDs = new DataSet();

        Fill_DataSET_Tables(SqlD, ref SqlDs, "MyDate");

        if (cnnObject == null || cnnObject.State == ConnectionState.Closed)
        {
            OpenConnection();
        }

        if (SqlDs.Tables.Contains("MyDate") && SqlDs.Tables["MyDate"].Rows.Count > 0)
        {
            try
            {
                return Convert.ToDateTime(SqlDs.Tables["MyDate"].Rows[0]["Dts"]);
            }
            catch
            {
                return DateTime.Now.Date;
            }
        }
        else
        {
            return DateTime.Now.Date;
        }
    }
}
