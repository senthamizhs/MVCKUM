using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Collections;
using System.Globalization;

public class DBHandler
{
    public string provider;
    public static string Flag;

    public static string Triptype;

    public static string SegmentType;

    public static string AgentId;

    public static string TerminalId;


    private string _constr = "TravelDB";
    public string ConStr
    {
        get
        {
            return (_constr);
        }
        set
        {
            _constr = value;
        }
    }
    private string _usrname;
    public string UsrName
    {
        get
        {
            return (_usrname);
        }
        set
        {
            _usrname = value;
        }
    }
    public static DateTime SQLMinDate = new DateTime(1900, 1, 1, 0, 0, 0);

    //================================================================================	

    //	Excecutes the Select Procedure with the parameters given by users and 
    //	it is excecuting through the dataadapter and returns the value in dataset.

    //=================================================================================	

    public static bool ExecuteProcedure(string ProcedureName, DataTable InsertPublishedFare, ref string errormsg)
    {
        SqlConnection my_connection = null;
        DataSet my_dataset = new DataSet();
        string my_procedure = ProcedureName;
        SqlTransaction my_tran = null;
        try
        {

            my_connection = OpenApiConnection();
            my_tran = my_connection.BeginTransaction();
            //bool booCheck = false;
            for (int lindex = 0; lindex < InsertPublishedFare.Rows.Count; lindex++)
            {

                Hashtable Parameters = new Hashtable();
                Parameters.Add("PF_BOOKING_TYPE", InsertPublishedFare.Rows[lindex]["BOOKING_TYPE"].ToString());// "W9 505");
                Parameters.Add("PF_FLIGHT_NUM", InsertPublishedFare.Rows[lindex]["FLIGHT_NO"].ToString());// "W9 505");
                Parameters.Add("PF_ORIGIN_CODE", InsertPublishedFare.Rows[lindex]["ORIGIN"].ToString());//"RGN");
                Parameters.Add("PF_DESTINATION_CODE", InsertPublishedFare.Rows[lindex]["DESTINATION"].ToString());//"BKK");
                if (InsertPublishedFare.Rows[lindex]["TRIP_TYPE"].ToString().Contains("R"))
                    Parameters.Add("PF_SECTOR", InsertPublishedFare.Rows[lindex]["ORIGIN"].ToString() +
                        " -> " + InsertPublishedFare.Rows[lindex]["DESTINATION"].ToString() +
                        " -> " + InsertPublishedFare.Rows[lindex]["ORIGIN"].ToString());//"<Fare></Fare>");
                else
                    Parameters.Add("PF_SECTOR", InsertPublishedFare.Rows[lindex]["SECTOR"].ToString());//"<Fare></Fare>");


                Parameters.Add("PF_FARE_DETAILS", InsertPublishedFare.Rows[lindex]["FARE"].ToString());//"<Fare></Fare>");
                Parameters.Add("PF_CLASS_CODE", InsertPublishedFare.Rows[lindex]["CLASS"].ToString());//"W");
                Parameters.Add("PF_ADULT_BASEFAE", InsertPublishedFare.Rows[lindex]["ADTBASEFARE"].ToString());//"W");

                Parameters.Add("PF_FARE_BASIS", InsertPublishedFare.Rows[lindex]["FARE_BASIS"].ToString());//"W2IP");
                Parameters.Add("PF_TRIP_TYPE", InsertPublishedFare.Rows[lindex]["TRIP_TYPE"].ToString());// "O");
                Parameters.Add("PF_CURRENCY_CODE", InsertPublishedFare.Rows[lindex]["CURRENCY_CODE"].ToString());// "N");
                Parameters.Add("PF_NEGO_FARE", InsertPublishedFare.Rows[lindex]["NEGO_FARE"].ToString());//"Y");

                DateTime ldtEffFrmDate = DateTime.ParseExact(InsertPublishedFare.Rows[lindex]["EFF_FROM_DATE"].ToString(),
                    "dd MMM yyyy", CultureInfo.InvariantCulture);
                DateTime ldtEffToDate = DateTime.ParseExact(InsertPublishedFare.Rows[lindex]["EFF_TO_DATE"].ToString(),
                   "dd MMM yyyy", CultureInfo.InvariantCulture);

                Parameters.Add("PF_EFF_FROM_DATE", ldtEffFrmDate.ToString("MM/dd/yyyy"));// "N");
                Parameters.Add("PF_EFF_TO_DATE", ldtEffToDate.ToString("MM/dd/yyyy"));//"Y");

                Parameters.Add("PF_FARE_REFUNDABLE", InsertPublishedFare.Rows[lindex]["fareRefundable"].ToString());// "N");
                Parameters.Add("PF_FARE_RULETEXT", InsertPublishedFare.Rows[lindex]["FareRuleText"].ToString());//"Y");

                SqlCommand my_command = my_connection.CreateCommand();
                my_command.CommandText = ProcedureName;
                my_command.CommandType = CommandType.StoredProcedure;
                my_command.Transaction = my_tran;
                AssignParameters(my_command, Parameters);
                my_command.ExecuteNonQuery();

            }
            my_tran.Commit();
        }
        catch (Exception ex)
        {
            DatabaseLog.foldererrorlog(ex.ToString(), "ExecuteProcedure", "Dbhandler");
            errormsg = ex.ToString();
            //StoreLocalFolder(errormsg);
            my_tran.Rollback();
            return false;
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return true;
    }



    public static string ExecProcedureReturnsSingleValue(string ProcedureName, Hashtable Parameters, ref string ErrorMsg)
    {
        SqlConnection my_connection_s = null;
        string retValue = string.Empty;
        ErrorMsg = string.Empty;
        try
        {

            my_connection_s = OpenSqlConnectionCS("APIDB");
            SqlCommand my_command = my_connection_s.CreateCommand();
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            AssignParameters(my_command, Parameters);
            my_command.CommandTimeout = 0;
            retValue = my_command.ExecuteScalar().ToString();

        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecProcedureReturnsSingleValue", "Dbhandler");
            ErrorMsg = my_exception.Message.ToString();
        }
        finally
        {
            if (my_connection_s != null)
            {
                my_connection_s.Close();
            }
        }
        return (retValue);
    }

        
    public static bool ExecuteProcedureCheck(string ProcedureName, ref DataTable InsertPublishedFare, ref string errormsg)
    {

        try
        {
            string agentidflag = "0";
            DataTable dtTemp = new DataTable();
            if (InsertPublishedFare.Columns.Contains("AgentId") && InsertPublishedFare.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(InsertPublishedFare.Rows[0]["AgentId"].ToString().Trim()))
                {
                    dtTemp = InsertPublishedFare.Copy();
                    agentidflag = InsertPublishedFare.Rows[0]["AgentId"].ToString().Trim();
                    InsertPublishedFare.Rows.Clear();
                    for (int iindex = 0; iindex < agentidflag.Split('|').Length; iindex++)
                    {
                        for (int incr = 0; incr < dtTemp.Rows.Count; incr++)
                        {
                            DataRow dr = InsertPublishedFare.NewRow();
                            dr["BOOKING_TYPE"] = agentidflag.Split('|')[iindex].ToString();
                            dr["FLIGHT_NO"] = dtTemp.Rows[incr]["FLIGHT_NO"].ToString();
                            dr["ORIGIN"] = dtTemp.Rows[incr]["ORIGIN"].ToString();
                            dr["DESTINATION"] = dtTemp.Rows[incr]["DESTINATION"].ToString();
                            dr["SECTOR"] = dtTemp.Rows[incr]["SECTOR"].ToString();
                            dr["FARE"] = dtTemp.Rows[incr]["FARE"].ToString();
                            dr["CLASS"] = dtTemp.Rows[incr]["CLASS"].ToString();
                            dr["FARE_BASIS"] = dtTemp.Rows[incr]["FARE_BASIS"].ToString();
                            dr["ADTBASEFARE"] = dtTemp.Rows[incr]["ADTBASEFARE"].ToString();
                            dr["TRIP_TYPE"] = dtTemp.Rows[incr]["TRIP_TYPE"].ToString();
                            dr["CURRENCY_CODE"] = dtTemp.Rows[incr]["CURRENCY_CODE"].ToString();
                            dr["NEGO_FARE"] = dtTemp.Rows[incr]["NEGO_FARE"].ToString();
                            dr["EFF_FROM_DATE"] = dtTemp.Rows[incr]["EFF_FROM_DATE"].ToString();
                            dr["EFF_TO_DATE"] = dtTemp.Rows[incr]["EFF_TO_DATE"].ToString();
                            dr["AgentId"] = dtTemp.Rows[incr]["AgentId"].ToString();
                            dr["fareRefundable"] = dtTemp.Rows[incr]["fareRefundable"].ToString();
                            dr["FareRuleText"] = dtTemp.Rows[incr]["FareRuleText"].ToString();
                            InsertPublishedFare.Rows.Add(dr);
                        }
                    }
                    agentidflag = "1";
                }
            }
            //bool booCheck = false;
            string Agentid = string.Empty;
            string ExistAgentid = string.Empty;
            for (int lindex = 0; lindex < InsertPublishedFare.Rows.Count; lindex++)
            {
                if (Agentid != string.Empty && Agentid == InsertPublishedFare.Rows[lindex]["BOOKING_TYPE"].ToString())
                    continue;
                Hashtable Parameters = new Hashtable();
                //
                Parameters.Add("PF_BOOKING_TYPE", InsertPublishedFare.Rows[lindex]["BOOKING_TYPE"].ToString());// "W9 505");
                //   Parameters.Add("PF_FLIGHT_NUM", InsertPublishedFare.Rows[lindex]["FLIGHT_NO"].ToString());// "W9 505");
                Parameters.Add("PF_FLIGHT_NUM", InsertPublishedFare.Rows[lindex]["FLIGHT_NO"].ToString());// "W9 505");
                Parameters.Add("PF_ORIGIN_CODE", InsertPublishedFare.Rows[lindex]["ORIGIN"].ToString());//"RGN");
                Parameters.Add("PF_DESTINATION_CODE", InsertPublishedFare.Rows[lindex]["DESTINATION"].ToString());//"BKK");
                if (InsertPublishedFare.Rows[lindex]["TRIP_TYPE"].ToString().Contains("R"))
                {
                    Parameters.Add("PF_SECTOR", InsertPublishedFare.Rows[lindex]["ORIGIN"].ToString() +
                        " -> " + InsertPublishedFare.Rows[lindex]["DESTINATION"].ToString() +
                        " -> " + InsertPublishedFare.Rows[lindex]["ORIGIN"].ToString());//"<Fare></Fare>");
                }
                else
                {
                    Parameters.Add("PF_SECTOR", InsertPublishedFare.Rows[lindex]["SECTOR"].ToString());//"<Fare></Fare>");
                }

                Parameters.Add("PF_FARE_DETAILS", InsertPublishedFare.Rows[lindex]["FARE"].ToString());//"<Fare></Fare>");
                Parameters.Add("PF_CLASS_CODE", InsertPublishedFare.Rows[lindex]["CLASS"].ToString());//"W");
                Parameters.Add("PF_ADULT_BASEFAE", InsertPublishedFare.Rows[lindex]["ADTBASEFARE"].ToString());//"W");

                Parameters.Add("PF_FARE_BASIS", InsertPublishedFare.Rows[lindex]["FARE_BASIS"].ToString());//"W2IP");
                Parameters.Add("PF_TRIP_TYPE", InsertPublishedFare.Rows[lindex]["TRIP_TYPE"].ToString());// "O");
                Parameters.Add("PF_CURRENCY_CODE", InsertPublishedFare.Rows[lindex]["CURRENCY_CODE"].ToString());// "N");
                Parameters.Add("PF_NEGO_FARE", InsertPublishedFare.Rows[lindex]["NEGO_FARE"].ToString());//"Y");

                DateTime ldtEffFrmDate = DateTime.ParseExact(InsertPublishedFare.Rows[lindex]["EFF_FROM_DATE"].ToString(),
                    "dd MMM yyyy", CultureInfo.InvariantCulture);
                DateTime ldtEffToDate = DateTime.ParseExact(InsertPublishedFare.Rows[lindex]["EFF_TO_DATE"].ToString(),
                   "dd MMM yyyy", CultureInfo.InvariantCulture);

                Parameters.Add("PF_EFF_FROM_DATE", ldtEffFrmDate.ToString("MM/dd/yyyy"));// "N");
                Parameters.Add("PF_EFF_TO_DATE", ldtEffToDate.ToString("MM/dd/yyyy"));//"Y");
                Parameters.Add("PF_FARE_REFUNDABLE", InsertPublishedFare.Rows[lindex]["fareRefundable"].ToString());//"Y");

                Agentid = InsertPublishedFare.Rows[lindex]["BOOKING_TYPE"].ToString();

                string lstrErrorMessage = "";
                string lstrData = ExecProcedureReturnsSingleValue("", Parameters, ref lstrErrorMessage);
                if (!(lstrData.Replace(" ", "")).Contains("NOTEXISTS"))
                {
                    // StoreLocalFolder(lstrData + lstrErrorMessage);
                    errormsg = "EXIST";
                    if (agentidflag == "0")
                        return false;
                    else
                        ExistAgentid += Agentid + " , ";
                }
                else if (agentidflag == "0")
                    return true;
            }

            if (agentidflag == "1")
            {
                var qry = from p in InsertPublishedFare.AsEnumerable()
                          where (!ExistAgentid.Contains(p["BOOKING_TYPE"].ToString().Trim()))
                          select p;
                if (qry.Count() > 0)
                {
                    InsertPublishedFare = qry.CopyToDataTable();
                    errormsg = "T~" + ExistAgentid.Trim().TrimEnd(',');
                    return true;
                }
                else
                {
                    errormsg = "EXIST";
                    return false;
                }
            }

        }
        catch (Exception ex)
        {
            DatabaseLog.foldererrorlog(ex.ToString(), "ExecuteProcedureCheck", "Dbhandler");
            errormsg = ex.ToString();
            //StoreLocalFolder(errormsg);

            return false;
        }
        finally
        {

        }
        return true;
    }
    

    public static DataSet ExecProcedureReturnsDataset(string ProcedureName, Hashtable Parameters)
    {
        SqlConnection my_connection = null;
        DataSet my_dataset = new DataSet();
        string my_procedure = ProcedureName;
        try
        {

            my_connection = OpenSqlConnection();
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            my_command.CommandTimeout = 0;
            AssignParameters(my_command, Parameters);
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataset, "Temp");


        }
        catch (Exception ex)
        {
            DatabaseLog.foldererrorlog(ex.ToString(), "ExecProcedureReturnsDataset", "Dbhandler");
            //StoreLocalFolder(ex.Message + "\r\n" + ex.StackTrace);
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_dataset);
    }



    public static DataSet ExecDedicateSelectProcedure(string ProcedureName, Hashtable Parameters)
    {
        SqlConnection my_connection = null;
        DataSet my_dataset = new DataSet();
        string my_procedure = ProcedureName;
        try
        {
            my_connection = OpenDedicateSqlConnection();
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            assignParameters(my_command, Parameters);
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataset, ProcedureName);
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecDedicateSelectProcedure", "Dbhandler");
            //  WriteLog(my_exception.Message + "\n" + my_exception.StackTrace);
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        //  if (checkProcedureExists(my_procedure))
        if (my_dataset != null && my_dataset.Tables.Count > 0 && my_dataset.Tables[0].Rows.Count > 0)
        {
            return (my_dataset);
        }
        else
        {
            if (my_dataset.Tables.Count > 1 && my_dataset.Tables[1].Rows.Count > 0)
            {
                return (my_dataset);
            }
            else
            {
                return (null);
            }
        }
    }
    public static bool ExecNonProcedure(string ProcedureName, Hashtable Parameters, ref string strErrorMsg)
    {
        SqlConnection my_connection = null;
        SqlCommand my_command = null;
        SqlTransaction my_tran = null;
        string my_result = string.Empty;
        try
        {
            my_connection = OpenSqlConnection();
            my_tran = my_connection.BeginTransaction();
            my_command = my_connection.CreateCommand();
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            my_command.CommandTimeout = 0;
            my_command.Transaction = my_tran;
            assignParameters(my_command, Parameters);
            my_result = my_command.ExecuteNonQuery().ToString();
            my_tran.Commit();
            return true;
        }
        catch (System.Data.SqlClient.SqlException my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonProcedure", "Dbhandler");
            if (my_exception.Number == 2627)
            {
                strErrorMsg = my_exception.Message; // +"\n" + my_exception.StackTrace;
                my_tran.Rollback();
            }
            else
            {
                strErrorMsg = my_exception.Message; // +"\n" + my_exception.StackTrace;
                my_tran.Rollback();
            }
            return false;
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonProcedure", "Dbhandler");
            strErrorMsg = my_exception.Message; // +"\n" + my_exception.StackTrace;
            my_tran.Rollback();
            return false;
        }
        finally
        {
            //strErrorMsg = my_result;
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        //return (my_result);
    }


    public static DataSet ExecBServSelectProcedure(string ProcedureName, Hashtable Parameters)
    {
        SqlConnection my_connection = null;
        DataSet my_dataset = new DataSet();
        string my_procedure = ProcedureName;
        try
        {
            my_connection = OpenBservSqlConnection();
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            assignParameters(my_command, Parameters);
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataset, ProcedureName);
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecBServSelectProcedure", "Dbhandler");
            //  WriteLog(my_exception.Message + "\n" + my_exception.StackTrace);
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        //  if (checkProcedureExists(my_procedure))
        if (my_dataset != null && my_dataset.Tables.Count > 0 && my_dataset.Tables[0].Rows.Count > 0)
        {
            return (my_dataset);
        }
        else
        {
            if (my_dataset.Tables.Count > 1 && my_dataset.Tables[1].Rows.Count > 0)
            {
                return (my_dataset);
            }
            else
            {
                return (null);
            }
        }
    }
    public static string ExecLog(string ProcedureName, Hashtable Parameters, ref Hashtable Outputparams)
    {

        SqlConnection my_connection = null;
        SqlCommand my_command = null;
        SqlTransaction my_tran = null;
        string my_result;
        try
        {
            my_connection = OpenSqlConnectionCS("LOG");
            my_tran = my_connection.BeginTransaction();
            my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            my_command.Transaction = my_tran;
            assignParameters(my_command, Parameters);
            assignOutputParameters(my_command, Outputparams);
            my_result = my_command.ExecuteNonQuery().ToString();
            Outputparams = DBHandler.GetOutputParameters(my_command, Outputparams);
            my_tran.Commit();
        }
        catch (System.Data.SqlClient.SqlException my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecLog", "Dbhandler");
            if (my_exception.Number == 2627)
            {
                my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
            else
            {
                my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecLog", "Dbhandler");
            my_tran.Rollback();
            my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_result);
    }
    public static DataSet ExecSelectProcedure(string ProcedureName, Hashtable Parameters)
    {
        SqlConnection my_connection = null;
        DataSet my_dataset = new DataSet();
        string my_procedure = ProcedureName;
        try
        {
            my_connection = OpenSqlConnection();
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            assignParameters(my_command, Parameters);
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataset, ProcedureName);
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecSelectProcedure", "Dbhandler");
            //  WriteLog(my_exception.Message + "\n" + my_exception.StackTrace);
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        //  if (checkProcedureExists(my_procedure))
        if (my_dataset != null && my_dataset.Tables.Count > 0 && my_dataset.Tables[0].Rows.Count > 0)
        {
            return (my_dataset);
        }
        else
        {
            if (my_dataset.Tables.Count > 1 && my_dataset.Tables[1].Rows.Count > 0)
            {
                return (my_dataset);
            }
            else
            {
                return (null);
            }
        }
    }
    public static DataSet ExecSelectProcedureAPI(string ProcedureName, Hashtable Parameters, ref string strError,string APPType)
    {
        // strError = "NoError";
        SqlConnection my_connection = null;
        DataSet my_dataset = new DataSet();
        string my_procedure = ProcedureName;
        try
        {
            my_connection = OpenSqlConnectionCS(APPType);
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            assignParameters(my_command, Parameters);
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataset, ProcedureName);
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecSelectProcedureAPI", "Dbhandler");
            strError = my_exception.Message.ToString();
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return my_dataset;
        //  if (checkProcedureExists(my_procedure))
    }

    public static DataSet ExecSelectProcedure(string ProcedureName, Hashtable Parameters, ref string strError)
    {
       // strError = "NoError";
        SqlConnection my_connection = null;
        DataSet my_dataset = new DataSet();
        string my_procedure = ProcedureName;
        try
        {
            my_connection = OpenSqlConnection();
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            assignParameters(my_command, Parameters);
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataset, ProcedureName);
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecSelectProcedure", "Dbhandler");
            strError = my_exception.Message.ToString();
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return my_dataset;
        //  if (checkProcedureExists(my_procedure))
    }

    //================================================================================	

    //	Excecutes the Select Procedure with the parameters given by users and 
    //	it is excecuting through the dataadapter and returns the value in dataset.

    //=================================================================================		
    public static DataSet ExecSelectProcedure(string ProcedureName)
    {
        SqlConnection my_connection = null;
        DataSet my_dataset = new DataSet();
        string my_procedure = ProcedureName;
        try
        {
            //  if (checkProcedureExists(my_procedure))
            //  {
            my_connection = OpenSqlConnection();
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataset, ProcedureName);
            //  }

        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecSelectProcedure", "Dbhandler");
            //  WriteLog(my_exception.Message + "\n" + my_exception.StackTrace);
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }

        return (my_dataset);

    }
    public static DataTable ExecProcedureWithParamAPI(string ProcedureName, Hashtable Parameters, string tableName,string appType)
    {
        SqlConnection my_connection = null;
        DataTable my_dataTable = new DataTable(tableName);
        try
        {
            my_connection = OpenSqlConnectionCS(appType);
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            assignParameters(my_command, Parameters);
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataTable);
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecProcedureWithParamAPI", "Dbhandler");
            //  WriteLog(my_exception.Message + "\n" + my_exception.StackTrace);
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_dataTable);
    }

    public static DataTable ExecProcedureWithParam(string ProcedureName, Hashtable Parameters, string tableName)
    {
        SqlConnection my_connection = null;
        DataTable my_dataTable = new DataTable(tableName);
        try
        {
            my_connection = OpenSqlConnection();
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            assignParameters(my_command, Parameters);
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataTable);
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecProcedureWithParam", "Dbhandler");
            //  WriteLog(my_exception.Message + "\n" + my_exception.StackTrace);
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_dataTable);
    }

    //=========================================================================================
    //Modified By senthil on 04/01/2007 Execute Non Select Queries with Output Parameter
    //=========================================================================================
    public static string ExecDedicateNonSelectProcedure(string ProcedureName, Hashtable Parameters, ref Hashtable Outputparams)
    {
        SqlConnection my_connection = null;
        SqlCommand my_command = null;
        SqlTransaction my_tran = null;
        string my_result;
        try
        {
            my_connection = OpenDedicateSqlConnection();
            my_tran = my_connection.BeginTransaction();
            my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            my_command.Transaction = my_tran;
            assignParameters(my_command, Parameters);
            assignOutputParameters(my_command, Outputparams);
            my_result = my_command.ExecuteNonQuery().ToString();
            Outputparams = GetOutputParameters(my_command, Outputparams);
            my_tran.Commit();
        }
        catch (System.Data.SqlClient.SqlException my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecDedicateNonSelectProcedure", "Dbhandler");
            if (my_exception.Number == 2627)
            {
                my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
            else
            {
                my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecDedicateNonSelectProcedure", "Dbhandler");
            my_tran.Rollback();
            my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_result);
    }

    public static string ExecNonSelectProcedureAPI(string ProcedureName, Hashtable Parameters,
                    ref Hashtable Outputparams, ref string strErrorMsg,string APPType)
    {
        SqlConnection my_connection = null;
        SqlCommand my_command = null;
        SqlTransaction my_tran = null;
        string my_result = string.Empty;
        try
        {
            my_connection = OpenSqlConnectionCS(APPType);
            my_tran = my_connection.BeginTransaction();
            my_command = my_connection.CreateCommand();
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            my_command.Transaction = my_tran;
            assignParameters(my_command, Parameters);
            assignOutputParameters(my_command, Outputparams);
            my_result = my_command.ExecuteNonQuery().ToString();
            Outputparams = GetOutputParameters(my_command, Outputparams);
            my_tran.Commit();
        }
        catch (System.Data.SqlClient.SqlException my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonSelectProcedureAPI", "Dbhandler");
            strErrorMsg = my_exception.Message;
            if (my_exception.Number == 2627)
            {
                //my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
                my_tran.Rollback();
            }
            else
            {
                //my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
                my_tran.Rollback();
            }
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonSelectProcedureAPI", "Dbhandler");
            strErrorMsg = my_exception.Message;
            //my_result = my_exception.Message; 
            my_result = string.Empty;
            my_tran.Rollback();
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_result);
    }

    public static string ExecNonSelectProcedure(string ProcedureName, Hashtable Parameters,
                        ref Hashtable Outputparams, ref string strErrorMsg)
    {
        SqlConnection my_connection = null;
        SqlCommand my_command = null;
        SqlTransaction my_tran = null;
        string my_result = string.Empty;
        try
        {
            my_connection = OpenSqlConnection();
            my_tran = my_connection.BeginTransaction();
            my_command = my_connection.CreateCommand();
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            my_command.Transaction = my_tran;
            assignParameters(my_command, Parameters);
            assignOutputParameters(my_command, Outputparams);
            my_result = my_command.ExecuteNonQuery().ToString();
            Outputparams = GetOutputParameters(my_command, Outputparams);
            my_tran.Commit();
        }
        catch (System.Data.SqlClient.SqlException my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonSelectProcedure", "Dbhandler");
            strErrorMsg = my_exception.Message;
            if (my_exception.Number == 2627)
            {
                //my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
                my_tran.Rollback();
            }
            else
            {
                //my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
                my_tran.Rollback();
            }
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonSelectProcedure", "Dbhandler");
            strErrorMsg = my_exception.Message;
            //my_result = my_exception.Message; 
            my_result = string.Empty;
            my_tran.Rollback();
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_result);
    }
    

    public static string ExecNonSelectProcedure(string ProcedureName, Hashtable Parameters, ref Hashtable Outputparams)
    {
        SqlConnection my_connection = null;
        SqlCommand my_command = null;
        SqlTransaction my_tran = null;
        string my_result;
        try
        {
            my_connection = OpenSqlConnection();
            my_tran = my_connection.BeginTransaction();
            my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            my_command.Transaction = my_tran;
            assignParameters(my_command, Parameters);
            assignOutputParameters(my_command, Outputparams);
            my_result = my_command.ExecuteNonQuery().ToString();
            Outputparams = DBHandler.GetOutputParameters(my_command, Outputparams);
            my_tran.Commit();
        }
        catch (System.Data.SqlClient.SqlException my_exception)
        {
            if (my_exception.Number == 2627)
            {
                my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
            else
            {
                my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonSelectProcedure", "Dbhandler");
        }
        catch (Exception my_exception)
        {
            my_tran.Rollback();
            my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonSelectProcedure", "Dbhandler");
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_result);
    }

    //================================================================================
    //	Checks whether the storedProcedure is existing in the database or not.		 
    //=================================================================================

    private static bool checkProcedureExists(string ProcedureName)
    {
        SqlConnection my_connection = null;
        SqlDataReader my_reader = null;
        bool exists = false;
        try
        {
            my_connection = OpenSqlConnection();
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = "_findprocedure";
            my_command.CommandType = CommandType.StoredProcedure;
            my_command.Parameters.Add("@PROCEDURENAME", SqlDbType.NVarChar, 50);
            my_command.Parameters["@PROCEDURENAME"].Value = ProcedureName;
            my_reader = my_command.ExecuteReader();
            if (my_reader.Read())
            {
                exists = true;
            }
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "checkProcedureExists", "Dbhandler");
            //   WriteLog(my_exception.Message + "\n" + my_exception.StackTrace);
        }
        finally
        {
            if (my_reader != null)
            {
                my_reader.Close();
            }
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (exists);
    }


    //================================================================================
    //	Assigning the parameter and values for the sqlcommand that carries the stored
    // procedure or SQL Query.
    //=================================================================================
    public static void assignParameters(SqlCommand Command, Hashtable Parameters)
    {
        if (Parameters == null) return;
        foreach (object key in Parameters.Keys)
        {
            //if (Parameters[key] == null || Parameters[key].ToString() == string.Empty)
            //{
            //    Command.Parameters.Add(("@" + key.ToString().ToUpper()), DBNull.Value);
            //}
            if (Parameters[key].Equals(DateTime.MinValue))
            {
                Command.Parameters.Add(("@" + key.ToString().ToUpper()), SQLMinDate);
            }
            else
            {
                if (key.ToString() != "AG_LOGO")
                    Command.Parameters.Add(("@" + key.ToString().ToUpper()), Parameters[key]);
                else
                {
                    var contentsParam = new SqlParameter("@" + key.ToString().ToUpper(), SqlDbType.Image);
                    contentsParam.Value = Parameters[key] ?? (object)DBNull.Value;
                    Command.Parameters.Add(contentsParam);

                }

            }
        }
    }

    public static string ExecuteReader(string ProcedureName, Hashtable Parameters)
    {
        SqlConnection my_connection = null;
        SqlDataReader sqlDatareader = null;
        string my_result = string.Empty;
        try
        {
            my_connection = OpenSqlConnection();
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            assignParameters(my_command, Parameters);
            sqlDatareader = my_command.ExecuteReader();
            if (sqlDatareader.Read())
            {
                my_result = sqlDatareader[0].ToString();
            }
            sqlDatareader.Close();
        }
        catch (Exception my_exception)
        {
            //  WriteLog(my_exception.Message + "\n" + my_exception.StackTrace);
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecuteReader", "Dbhandler");
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_result);
    }

    //================================================================================
    //	Assigning output parameters for the sqlcommand that carries the stored
    // procedure or SQL Query.
    //=================================================================================
    private static void assignOutputParameters(SqlCommand Command, Hashtable Parameters)
    {
        if (Parameters == null) return;
        foreach (object key in Parameters.Keys)
        {


            SqlParameter param = new SqlParameter(("@" + key.ToString().ToUpper()), SqlDbType.VarChar, 50);
            param.Direction = ParameterDirection.Output;
            Command.Parameters.Add(param);

        }
    }

    //================================================================================
    //	Taking output parameters from the sqlcommand that carries the stored
    // procedure.
    //=================================================================================
    public static Hashtable GetOutputParameters(SqlCommand Command, Hashtable Parameters)
    {
        Hashtable my_row = new Hashtable();
        if (Parameters == null) return my_row;
        foreach (object key in Parameters.Keys)
        {
            my_row.Add((key.ToString().ToUpper()), Command.Parameters["@" + key.ToString().ToUpper()].Value.ToString());
        }
        return my_row;
    }

  

    #region DB CONNECTION NOT USING

    public static SqlConnection OpenSqlConnectionCS(string APPtype)
    {
        SqlConnection my_connection = new SqlConnection();
        string connectionString = CreateConnStrCS(APPtype);
        my_connection.ConnectionString = connectionString;
        my_connection.Open();
        return (my_connection);
    }
    //=================================================================================
    //	Establishing the SQL Connection string 
    //=================================================================================		
    public static SqlConnection OpenSqlConnection()
    {
        SqlConnection my_connection = new SqlConnection();
        string connectionString = CreateConnStr();
        my_connection.ConnectionString = connectionString;
        my_connection.Open();
        return (my_connection);
    }
    public static SqlConnection OpenDedicateSqlConnection()
    {
        SqlConnection my_connection = new SqlConnection();
        string connectionString = CreateDedicateConnStr();
        my_connection.ConnectionString = connectionString;
        my_connection.Open();
        return (my_connection);
    }
    public static SqlConnection OpenBservSqlConnection()
    {
        SqlConnection my_connection = new SqlConnection();
        string connectionString = CreateBServConnStr();
        my_connection.ConnectionString = connectionString;
        my_connection.Open();
        return (my_connection);
    }
    public static SqlConnection OpenApiConnection()
    {
        SqlConnection my_connection = new SqlConnection();
        string connectionString = CreateAPIConnStr();
        my_connection.ConnectionString = connectionString;
        my_connection.Open();
        return (my_connection);
    }


    //================================================================================	
    //	Creating connection from XML file located in the system Directory, If it is 
    //  not existing in the System Directory it will create a new XML file to mention 
    //  the connection details.
    //=================================================================================


    private static String CreateConnStrCS(string APPType)
    {
        string connectionString = null;
        if (APPType == "APPDB")
        {
            connectionString = "";// ConfigurationManager.AppSettings["TravelDB"].ToString();
            return (connectionString);
        }
        else if (APPType == "APIDB")
        {
            connectionString = "";// ConfigurationManager.AppSettings["API_SERVER"].ToString();
            return (connectionString);
        }
        else if (APPType == "LOG")
        {
            connectionString = "";//ConfigurationManager.AppSettings["LOGDB"].ToString();
            return (connectionString);
        }
        return (connectionString);
    }
    private static String CreateConnStr()
    {
        string connectionString = null;
        connectionString = "";// ConfigurationManager.AppSettings["TravelDB"].ToString();
        return (connectionString);

    }
    private static String CreateBServConnStr()
    {
        string connectionString = null;
        connectionString = "";// ConfigurationManager.AppSettings["TravelDB"].ToString();
        return (connectionString);

    }
    private static String CreateDedicateConnStr()
    {
        string connectionString = null;
        connectionString = "";// ConfigurationManager.AppSettings["TravelDB"].ToString();
        return (connectionString);

    }
    private static String CreateAPIConnStr()
    {
        string connectionString = null;
        connectionString = "";// ConfigurationManager.AppSettings["API_SERVER"].ToString();
        return (connectionString);

    }

    #endregion

    public DBHandler()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public static string ExecNonUpdateCS(string ProcedureName, Hashtable Parameters,string APPtype)
    {
        SqlConnection my_connection = null;
        SqlCommand my_command = null;
        SqlTransaction my_tran = null;
        string my_result;
        try
        {
            my_connection = OpenSqlConnectionCS(APPtype);
            my_tran = my_connection.BeginTransaction();
            my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            my_command.Transaction = my_tran;
            assignParameters(my_command, Parameters);
            my_result = my_command.ExecuteNonQuery().ToString();
            my_tran.Commit();
        }
        catch (System.Data.SqlClient.SqlException my_exception)
        {
            if (my_exception.Number == 2627)
            {
                my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
            else
            {
                my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonUpdateCS", "Dbhandler");
        }
        catch (Exception my_exception)
        {
            my_tran.Rollback();
            my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonUpdateCS", "Dbhandler");
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_result);
    }

    public static string ExecNonUpdate(string ProcedureName, Hashtable Parameters)
    {
        SqlConnection my_connection = null;
        SqlCommand my_command = null;
        SqlTransaction my_tran = null;
        string my_result;
        try
        {
            my_connection = OpenSqlConnection();
            my_tran = my_connection.BeginTransaction();
            my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            my_command.Transaction = my_tran;
            assignParameters(my_command, Parameters);
            my_result = my_command.ExecuteNonQuery().ToString();
            my_tran.Commit();
        }
        catch (System.Data.SqlClient.SqlException my_exception)
        {
            if (my_exception.Number == 2627)
            {
                my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
            else
            {
                my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonUpdate", "Dbhandler");
        }
        catch (Exception my_exception)
        {
            my_tran.Rollback();
            my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecNonUpdate", "Dbhandler");
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_result);
    }


    public static DataTable ExecProcedure(string ProcedureName, string tableName)
    {
        SqlConnection my_connection = null;
        DataTable my_dataTable = new DataTable(tableName);
        try
        {
            my_connection = OpenSqlConnection();
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataTable);
        }
        catch (Exception my_exception)
        {
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecProcedure", "Dbhandler");
            //  WriteLog(my_exception.Message + "\n" + my_exception.StackTrace);
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return (my_dataTable);
    }




    public static DataSet ExecSelectcomm(string ProcedureName, Hashtable Parameters)
    {
        SqlConnection my_connection = null;
        DataSet my_dataset = new DataSet();
        string my_procedure = ProcedureName;
        try
        {
            my_connection = OpenSqlConnection();
            SqlCommand my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            assignParameters(my_command, Parameters);
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataset, ProcedureName);
        }
        catch (Exception my_exception)
        {
            my_exception.Message.ToString();
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecSelectcomm", "Dbhandler");
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        //  if (checkProcedureExists(my_procedure))
        if (my_dataset.Tables[0].Rows.Count > 0)
        {
            return (my_dataset);
        }
        else
        {
            return (null);
        }
    }

    public static DataSet ExecSelectProc(string ProcedureName, Hashtable Parameters)
    {
        SqlConnection my_connection = null;
        SqlCommand my_command = null;
        SqlTransaction my_tran = null;
        DataSet my_dataset = new DataSet();
        string my_result;
        try
        {
            my_connection = OpenSqlConnection();
            my_tran = my_connection.BeginTransaction();
            my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            my_command.Transaction = my_tran;
            assignParameters(my_command, Parameters);
            //assignOutputParameters(my_command, Outputparams);
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataset, ProcedureName);
            my_tran.Commit();
            my_result = string.Empty;
        }
        catch (System.Data.SqlClient.SqlException my_exception)
        {
            if (my_exception.Number == 2627)
            {
                if (my_tran != null)
                    my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
            else
            {
                if (my_tran != null)
                    my_tran.Rollback();
                my_result = my_exception.Message; // +"\n" + my_exception.StackTrace;
            }
            DatabaseLog.foldererrorlog(my_exception.ToString(), "ExecSelectProc", "Dbhandler");
            throw my_exception;
        }
        catch (Exception my_exceptionEx)
        {
            if (my_tran != null)
                my_tran.Rollback();
            my_result = my_exceptionEx.Message; // +"\n" + my_exception.StackTrace;
            DatabaseLog.foldererrorlog(my_exceptionEx.ToString(), "ExecSelectProc", "Dbhandler");
            throw my_exceptionEx;
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return my_dataset;
    }

    public static void AssignParameters(SqlCommand Command, Hashtable Parameters)
    {
        foreach (object key in Parameters.Keys)
        {
            if (Parameters[key].Equals(DateTime.MinValue))
            {
                Command.Parameters.Add(("@" + key.ToString().ToUpper()), SQLMinDate);
            }
            else
            {
                if (key != "AG_LOGO")
                    Command.Parameters.Add(("@" + key.ToString().ToUpper()), Parameters[key]);
                else
                {
                    var contentsParam = new SqlParameter("@" + key.ToString().ToUpper(), SqlDbType.Image);
                    contentsParam.Value = Parameters[key] ?? (object)DBNull.Value;
                    Command.Parameters.Add(contentsParam);

                }
            }
        }

    }



    public static DataSet ExecSelectProc(string ProcedureName, Hashtable Parameters, ref string errorMsg)
    {
        SqlConnection my_connection = null;
        SqlCommand my_command = null;
        //SqlTransaction my_tran = null;
        DataSet my_dataset = new DataSet();
        string my_result;
        try
        {
            errorMsg = string.Empty;

            my_connection = OpenSqlConnection();
            
            my_command = my_connection.CreateCommand();
            my_command.CommandTimeout = 0;
            my_command.CommandText = ProcedureName;
            my_command.CommandType = CommandType.StoredProcedure;
            assignParameters(my_command, Parameters);
            //assignOutputParameters(my_command, Outputparams);
            SqlDataAdapter my_adapter = new SqlDataAdapter();
            my_adapter.SelectCommand = my_command;
            my_adapter.Fill(my_dataset, ProcedureName);
            my_result = string.Empty;
        }
        catch (System.Data.SqlClient.SqlException my_exception)
        {
            if (my_exception.Number == 2627)
            {
                errorMsg = my_exception.Message.ToString(); // +"\n" + my_exception.StackTrace;
            }
            else
            {
                errorMsg = my_exception.Message.ToString(); // +"\n" + my_exception.StackTrace;
            }
        }
        catch (Exception my_exceptionEx)
        {
            errorMsg = my_exceptionEx.Message.ToString(); // +"\n" + my_exception.StackTrace;
            DatabaseLog.foldererrorlog(my_exceptionEx.ToString(), "ExecSelectProc", "Dbhandler");
        }
        finally
        {
            if (my_connection != null)
            {
                my_connection.Close();
            }
        }
        return my_dataset;
    }
}

public enum Stock
{
    Check = 0,
    Block = 1,
    Release = 2

}
