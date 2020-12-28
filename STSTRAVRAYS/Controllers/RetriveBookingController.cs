using Newtonsoft.Json;
using QUEUERQRS;
using STSTRAVRAYS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Globalization;
using STSTRAVRAYS.InplantService;
using System.IO.Compression;
using STSTRAVRAYS.Rays_service;
using System.Text.RegularExpressions;

namespace STSTRAVRAYS.Controllers
{
    public class RetriveBookingController : LoginController
    {
        // GET: /QueueTicket/
     

        Inplantservice _InplantService = new Inplantservice();
        RaysService _RaysService = new RaysService();
        Base.ServiceUtility _ServiceUtility = new Base.ServiceUtility();
        string strBranchCredit = ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"] != null ? ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"].ToString().ToUpper() : "";
        string strModule = (ConfigurationManager.AppSettings["PLATFORM"] != null ? ConfigurationManager.AppSettings["PLATFORM"].ToString().ToUpper().Trim() : "")
            + (Convert.ToString(System.Web.HttpContext.Current.Session["RetrieveFlag"]) == "A" ? "" : "QT");//For payment mode fetch function

        

        public ActionResult RetriveBooking()
        {

            if (Request.QueryString["SECKEY"] != null && Request.QueryString["SECKEY"] != "")
            {
                string Encquery = Request.QueryString["SECKEY"];
                string today = DateTime.Now.ToString("dd/MM/yyyy");
                string Querystring = Base.DecryptKEY(Encquery, "RIYA" + today);

                string[] keyval = new string[20];
                string[] Query = Querystring.Split('&');

                Session.Clear();
                for (int i = 0; i < Query.Length; i++)
                {
                    Session.Add(Query[i].Split('=')[0].Trim().ToUpper(), Query[i].Split('=')[1].Trim());
                }

                if (Session["SECKEY"] != null && Session["SECKEY"].ToString() != "")
                {
                    Session["SECKEY"] = Request.QueryString["SECKEY"].ToString();
                }
                else
                {
                    Session.Add("SECKEY", Request.QueryString["SECKEY"]);
                    Loginsubmit(Session["TERMINALID"].ToString(), Session["USERNAME"].ToString(), Session["PWD"].ToString(), Request.QueryString["TERMINALTYPE"].ToString());
                }

            }

            if (Session["agentid"] == null)
            {
                ViewBag.SessionExp = "Your session has expired!";
            }

            Session.Add("RetrieveFlag", "T"); //  Ticketing


            string stryears = Base.LoadServerdatetime();
            ViewBag.Servercurrentdate = stryears;

            string yearsvalue = stryears.Split('/')[2];
            ViewBag.ServerDateTime = yearsvalue;

            return View("~/Views/Flights/RetriveBooking.cshtml");
        }

        public ActionResult Fetch_QueuePNR_Details(string CrsPnr, string CRS, string QueueingNumber,
            string AirportTypes, string Corporatename, string Employeename, string EmpCostCenter, string EmpRefID,
            string BranchID, string officeid, string FareType)
        {
            ArrayList pnrResult = new ArrayList();
            int Error = 0;
            int Result = 1;
            int pnrdata = 2;
            int FareQualifier = 3;
            int tst_count = 4;
            int pax_count = 5;
            decimal totgrossamount = 0;
            int TicketStatus = 6;
            int TotCnt = 0;
            int PlatingCarrier = 7;
            int CurrencyFlag = 11;
            int Checkpcc = 12;
            int GSTDetails = 13;

            pnrResult.Add(""); // Error  - 0
            pnrResult.Add("");// ResultURL  - 1
            pnrResult.Add("");// pnrds - 2
            pnrResult.Add("");// pnrFare - 3    
            pnrResult.Add("");// tstcount - 4 
            pnrResult.Add("");// paxcount - 5 
            pnrResult.Add("");// TicketStatus - 6 
            pnrResult.Add("");// PlatingCarrier - 7
            pnrResult.Add("");// FareFLag - 8
            pnrResult.Add("");// passthrough card -9
            pnrResult.Add("");// ReferenceToken -10
            pnrResult.Add("");// currencyflag -11
            pnrResult.Add("");//checkpcc - 12
            pnrResult.Add("");//GST - 13

            string AdtChd = string.Empty;
            string AirlineCET = string.Empty;
            DataSet dsPnrDetails = new DataSet();
            DataSet dsFetchAgentGSTDetails = new DataSet();
            string strErrorMsg = string.Empty;
            string strLoginDetails = string.Empty;
            string pnrData = string.Empty;
            string RetrievePCC = string.Empty;
            //string RetrieveGalileo = string.Empty;
            //test
            //officeid = "BOMVS34OA";
            //test
            //*******************************************
            string strAppType = "B2B";
            string strGroupID = string.Empty;
            string strClientID = Corporatename;
            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;
            string strBranchId = string.Empty;
            string strTerminalType = string.Empty;
            //*******************************************
            StringWriter strwrt = new StringWriter();
            string xmlData = string.Empty;
            bool RetrivePnr = false;
            decimal dcTotalFare = 0;
            string strResult = string.Empty;
            bool RebookFlag = false;

            string strTicketStatus = string.Empty;
            string strCorporatecode = string.Empty;
            string strPLATINGCARRIER = string.Empty;
            DataTable dtTSTCount = new DataTable();
            string strPlatingCarrier = string.Empty;

            string Paymentdetails = string.Empty;
            string strfareFlag = string.Empty;
            string strReferenceToken = string.Empty;
            string PassthroughCard = string.Empty;
            string strPosID = string.Empty;
            string strPosTID = string.Empty;
            try
            {

                strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
                strUserName = Session["username"] != null ? Session["username"].ToString() : "";
                Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
                strBranchId = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";

                if (strAgentID == "" || strPosID == "" || strTerminalType == "" || strUserName == "")
                {
                    pnrResult[Error] = "Your session has expired!";
                    DatabaseLog.Retrievetcktlog("Session Timeout", "E", "RetrieveBookingController.cs", "Fetch_QueuePNR_Details", DateTime.Now.ToString("yyyyMMddHHmmssfff"), strClientID, strPosTID, DateTime.Now.ToString("yyyyMMdd"));
                    return Json(new { Status = "-1", Message = pnrResult[Error], Result = pnrResult });
                }
                #region UsageLog
                try
                {
                    string strUsgLogRes = Base.Commonlog("Retrieve PNR Details", "", "FETCH");
                }
                catch (Exception e) { }
                #endregion

                string AgentOfficeID = "";
                decimal dcTDSPercentage = 0;
                string strTopUpBranchID = string.Empty;
                string Domestic_Countrycode = ConfigurationManager.AppSettings["COUNTRY"] != null && ConfigurationManager.AppSettings["COUNTRY"] != "" ? ConfigurationManager.AppSettings["COUNTRY"].ToString() : "I";

                DataSet dsAgentDetails = new DataSet();
                DataTable dtAgent = new DataTable("AGENT_INFO");
                dtAgent.Columns.Add("AGENT_TYPE");
                dtAgent.Columns.Add("TDS_PERCENTAGE");
                dtAgent.Columns.Add("BRANCHID");
                dtAgent.Columns.Add("HEADER_BRANCHID");
                dtAgent.Columns.Add("PAYMENTMODE");
                dtAgent.Columns.Add("CRS_PNR");
                dtAgent.Columns.Add("CRS_ID");
                dtAgent.Columns.Add("SEGMENTTYPE");
                dtAgent.Columns.Add("OFFICE_ID");
                dtAgent.Columns.Add("QUEUENUMBER");
                dtAgent.Columns.Add("RETRIEVEPCC");
                dtAgent.Rows.Add("RI", dcTDSPercentage, BranchID, strTopUpBranchID, "C", CrsPnr, CRS, "", AgentOfficeID, QueueingNumber.Trim(), RetrievePCC.Trim());
                dsAgentDetails.Tables.Add(dtAgent.Copy());
                string strClientTerminalID = string.Empty;
                string ClientID = string.Empty;
                if (strTerminalType == "T" && strClientID != "")
                {
                    ClientID = strClientID;
                    strClientTerminalID = strClientID + "01";
                }
                else
                {
                    ClientID = strPosID;
                    strClientTerminalID = strPosTID;
                }

                QueueRetrieveRQ _QueueRetrieveRQ = new QueueRetrieveRQ();
                QueueRetrieveRS _QueueRetrieveRS = new QueueRetrieveRS();

                AgentDetails _agent = new AgentDetails();
                _agent.AgentID = strAgentID;
                _agent.TerminalID = strTerminalId;
                _agent.AppType = "B2B";
                _agent.UserName = strUserName;
                _agent.BranchID = BranchID;
                _agent.ProductType = "RC";
                _agent.PNROfficeId = officeid;
                _agent.TicketingOfficeId = "";
                _agent.BOATerminalID = "";
                _agent.Environment = (strTerminalType == "T" ? "I" : "W");
                _agent.ClientID = Corporatename;
                _agent.Version = "";
                _agent.BOAID = ClientID;
                _agent.BOATerminalID = strClientTerminalID;
                _agent.AgentType = "";
                _agent.CoOrdinatorID = "";
                _agent.IssuingBranchID = BranchID;
                _agent.EMP_ID = Employeename;
                _agent.COST_CENTER = EmpCostCenter;
                _agent.Ipaddress = Ipaddress;
                _agent.Platform = "B";
                _agent.ProjectID = ConfigurationManager.AppSettings["ProjectCode"].ToString();

                PNRDetail _pnrdetail = new PNRDetail();
                _pnrdetail.CRSPNR = CrsPnr;
                _pnrdetail.CRSID = CRS;
                _pnrdetail.QUEUENUMBER = QueueingNumber;
                _pnrdetail.AUTOQUEUEFLAG = "";
                _pnrdetail.QUEUEID = "";

                string request = "";
                string methodname = "InvokePNRRetrieve";
                int servicetimeout = Convert.ToInt32(ConfigurationManager.AppSettings["QTKT_servicetimeout"]);

                _QueueRetrieveRQ.AgentDetail = _agent;
                _QueueRetrieveRQ.PNR = _pnrdetail;

                request = JsonConvert.SerializeObject(_QueueRetrieveRQ);

                MyWebClient client = new MyWebClient();
                //client.LintTimeout = servicetimeout;
                string strURLpath = ConfigurationManager.AppSettings["QTKT_Service_URL"].ToString();
                string response = string.Empty;
                string errorMessage = string.Empty;
                string Request = request;
                string url = strURLpath + "/" + methodname;
                // client.LintTimeout = int.MaxValue;
                client.Headers["Content-type"] = "application/json";

                //****************REQUEST LOG****************************************

                string strTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                xmlData = "<REQUEST>FETCH_QUEUEPNR_DETAILS</REQUEST>" +
                           "<CRSPNR>" + CrsPnr + "</CRSPNR>" +
                           "<CRSID>" + CRS + "</CRSID>" +
                           "<BRANCHID>" + BranchID + "</BRANCHID>" +
                           "<QUEUENUMBER>" + QueueingNumber + "</QUEUENUMBER>" +
                           "<REQUEST>" + request + "</REQUEST>";

                DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "Fetch_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, sequnceID);
                //****************END REQUEST LOG****************************************

                byte[] data = client.UploadData(url, "POST", Encoding.ASCII.GetBytes(Request));
                string strresponse = Encoding.ASCII.GetString(data);

                // string strresponse = System.IO.File.ReadAllText(@"D:\RET.txt");//while putting live

                if (!string.IsNullOrEmpty(strresponse))
                {
                    dsPnrDetails = _ServiceUtility.convertJsonStringToDataSet(strresponse, "");
                    Session["RETRIVEPNR" + CrsPnr.ToUpper().Trim()] = strresponse;
                }

                //********************RESPONSE LOG*****************************************
                dsPnrDetails.WriteXml(strwrt);
                xmlData = string.Empty;
                xmlData = "<EVENT><RESPONSE>FETCH_QUEUEPNR_DETAILS</RESPONSE><RESPONSEJSONSTRING>" + strresponse + "</RESPONSEJSONSTRING></EVENT>";

                DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "Fetch_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, sequnceID);
                //********************END RESPONSE LOG*****************************************

                #region SERVICE URL BRANCH BASED -- STS115
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (BranchID != "" && strBranchCredit.Contains(BranchID)))
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                        _InplantService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_INPLANT_SERVICE"].ToString();
                    }
                    else
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                        _InplantService.Url = ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString();
                    }
                }
                else
                {
                    _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                    _InplantService.Url = ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString();
                }
                #endregion

                if (dsPnrDetails != null && dsPnrDetails.Tables.Count > 0 && dsPnrDetails.Tables[0].Rows.Count > 0)
                {
                    strfareFlag = dsPnrDetails.Tables[0].Rows[0]["IsFare"].ToString().ToUpper().Trim();
                    strReferenceToken = dsPnrDetails.Tables[0].Columns.Contains("ReferenceToken") ? dsPnrDetails.Tables[0].Rows[0]["ReferenceToken"].ToString().ToUpper().Trim() : "";
                    pnrResult[8] = strfareFlag;
                    pnrResult[10] = strReferenceToken;
                }

                DataTable dtGSTDetails = new DataTable();
                string strGSTDetails = string.Empty;
                if (dsPnrDetails != null && dsPnrDetails.Tables.Count > 0 && dsPnrDetails.Tables.Contains("GstDetails") && dsPnrDetails.Tables["GstDetails"].Rows.Count > 0)
                {
                    dtGSTDetails = dsPnrDetails.Tables["GstDetails"].Copy();
                    dtGSTDetails.Columns.Remove("rootNode_Id");
                    if (dtGSTDetails.Rows[0]["GSTNumber"].ToString() == "")
                    {
                        dtGSTDetails.Rows[0].Delete();
                    }
                    dtGSTDetails.AcceptChanges();
                }

                if (dsPnrDetails != null && dsPnrDetails.Tables.Count > 0 &&
                dsPnrDetails.Tables.Contains("PassengerPNRDetails") && dsPnrDetails.Tables["PassengerPNRDetails"].Rows.Count > 0 &&
                dsPnrDetails.Tables.Contains("Result") && dsPnrDetails.Tables["Result"].Rows.Count > 0 &&
                dsPnrDetails.Tables["Result"].Rows[0]["Code"].ToString().Trim() == "1")
                {
                    if (strfareFlag == "Y")
                    {
                        foreach (DataRow drTSTcheck in dsPnrDetails.Tables["PassengerPNRDetails"].Rows)
                        {
                            if (drTSTcheck["TSTCOUNT"].ToString() == "")
                            {
                                pnrResult[Error] = "Unable to retrieve the PNR, due to the given PNR priced partially. Please remove the existing fares or do pricing the respective Itinerary.";
                                return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                            }
                        }
                    }



                    if (dtGSTDetails.Rows.Count > 0)
                    {
                        strGSTDetails = "API~" + JsonConvert.SerializeObject(dsPnrDetails.Tables[1]).ToString();
                    }
                    else
                    {
                        try
                        {
                            dsFetchAgentGSTDetails = _InplantService.Fetch_Client_GST_Details(strClientID, strUserName, strBranchId, Ipaddress, sequnceID, strAppType, strTerminalType, strGroupID);
                        }
                        catch (Exception Ex)
                        {

                        }

                        if (dsFetchAgentGSTDetails != null && dsFetchAgentGSTDetails.Tables.Count > 0 && dsFetchAgentGSTDetails.Tables[0].Rows.Count > 0)
                        {
                            strGSTDetails = "APP~" + JsonConvert.SerializeObject(dsFetchAgentGSTDetails.Tables[0]).ToString();
                        }
                    }

                    pnrResult[GSTDetails] = strGSTDetails;

                    if (ConfigurationManager.AppSettings["ENVIRONMENT"].ToString() == "1" && Session["RetrieveFlag"] != null && Session["RetrieveFlag"].ToString().Trim() == "T")
                    {
                        string strCheck_RQT_AGN_PCC = string.Empty;
                        string strDisplayMsg = string.Empty;
                        string strBranch = strTerminalType.ToUpper().Trim() == "T" ? "ALL" : BranchID;
                        byte[] byteresponse = _InplantService.FECTH_OWN_SUPP_DETAILS_RQT_CLIENT_PCC(CRS, strBranch, Corporatename, strUserName, Ipaddress, sequnceID, strTerminalId, strTerminalType, strresponse, ref strDisplayMsg);
                        DataSet dsRQT_AGN_PCC = Base.Decompress(byteresponse);

                        if (strDisplayMsg != "" && strDisplayMsg != null)
                        {
                            pnrResult[Error] = strDisplayMsg;
                            return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                        }

                        if (dsRQT_AGN_PCC != null && dsRQT_AGN_PCC.Tables.Count > 0 && dsRQT_AGN_PCC.Tables[0].Rows.Count > 0)
                        {
                            strCheck_RQT_AGN_PCC = JsonConvert.SerializeObject(dsRQT_AGN_PCC.Tables[0]).ToString();
                            AgentOfficeID = dsRQT_AGN_PCC.Tables.Contains("P_FETCH_RQT_CLIENT_PCC1") ? dsRQT_AGN_PCC.Tables["P_FETCH_RQT_CLIENT_PCC1"].Rows[0]["AGENT OFFICEID"].ToString() : "";
                            Session["LOADPCC" + CrsPnr] = dsRQT_AGN_PCC;

                        }
                        else
                        {
                            pnrResult[Error] = "OfficeID is not assigned for this user. Please assign officeID.";
                            return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                        }

                        if (strTerminalType.ToUpper().Trim() == "W")
                        {
                            int TempOfficeID = 0;

                            string[] strOfficeID = AgentOfficeID.Split(',');
                            foreach (string strOffID in strOfficeID)
                            {
                                if (dsPnrDetails.Tables["PassengerPNRDetails"].Rows[0]["OFFICEID"].ToString().ToUpper().Trim() == strOffID.ToUpper().Trim())
                                {
                                    TempOfficeID = 1;
                                    break;
                                }
                            }
                            if (TempOfficeID == 0)
                            {
                                pnrResult[Error] = "OfficeID mismatched. Please check your officeID.";
                                return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                            }
                        }

                        pnrResult[Checkpcc] = strCheck_RQT_AGN_PCC;

                    }

                    if (ConfigurationManager.AppSettings["ENVIRONMENT"].ToString() == "0" || (Session["RetrieveFlag"] != null && Session["RetrieveFlag"].ToString().Trim() == "A"))
                    {
                        pnrResult[Checkpcc] = dsPnrDetails.Tables["PassengerPNRDetails"].Rows[0]["QUEUINGOFFICEID"].ToString() != "" ? dsPnrDetails.Tables["PassengerPNRDetails"].Rows[0]["QUEUINGOFFICEID"].ToString() : dsPnrDetails.Tables["PassengerPNRDetails"].Rows[0]["OFFICEID"].ToString();
                    }
                    pnrResult[FareQualifier] = dsPnrDetails.Tables["PassengerPNRDetails"].Rows[0]["FareQualifier"].ToString().ToUpper().Trim();

                    string Currency = dsPnrDetails.Tables["PassengerPNRDetails"].Rows[0]["Currency"].ToString();
                    pnrResult[CurrencyFlag] = Currency;
                    var TotalFare = (from _lstFre in dsPnrDetails.Tables["PassengerPNRDetails"].AsEnumerable()
                                     group _lstFre by new
                                     {
                                         PAYMENTINFO = _lstFre["PAYMENTINFO"].ToString().Trim(),

                                     } into _farelst
                                     select new
                                     {
                                         // GROSSAMT = _farelst.Key.GROSSAMT,
                                         FAREDET_NEW = _farelst,
                                     }).ToList();

                    //********************RESPONSE LOG*****************************************
                    dsPnrDetails.WriteXml(strwrt);
                    xmlData = string.Empty;

                    PassthroughCard = dsPnrDetails.Tables["PassengerPNRDetails"].Rows[0]["PAYMENTINFO"].ToString().Trim();
                    pnrResult[9] = PassthroughCard;
                    xmlData = "<EVENT>" + "<RESPONSE>FETCH_QUEUEPNR_DETAILS</RESPONSE>" +
                                        "<STATUS>SUCCESS</STATUS>" +
                                        "<PNRINFO>" + strwrt.ToString() + "</PNRINFO>" +
                                        "<ERRORMSG>" + strErrorMsg + "</ERRORMSG>" +
                                        "</EVENT>";
                    DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "Fetch_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, sequnceID);
                    //********************END RESPONSE LOG*****************************************
                    #region  TST count is Empty
                    DataTable PASSENGER_PNRINFO = new DataTable("PassengerPNRDetails");
                    if (dsPnrDetails.Tables["PassengerPNRDetails"].Rows.Count > 0)
                    {
                        PASSENGER_PNRINFO = dsPnrDetails.Tables["PassengerPNRDetails"].Clone();
                        foreach (DataRow drRowsNew in dsPnrDetails.Tables["PassengerPNRDetails"].Rows)
                        {
                            if (!string.IsNullOrEmpty(drRowsNew["TSTCOUNT"].ToString().ToUpper().Trim()))
                            {
                                PASSENGER_PNRINFO.Rows.Add(drRowsNew.ItemArray);
                            }
                            PASSENGER_PNRINFO.AcceptChanges();
                        }
                    }

                    //dsPnrDetails.Tables.Remove("PassengerPNRDetails"); by ragaveni 2805
                    //dsPnrDetails.Tables.Add(PASSENGER_PNRINFO.Copy());
                    #endregion

                    // Checking Airport Domestic or not
                    if (!dsAgentDetails.Tables["AGENT_INFO"].Columns.Contains("AIRPORT_TYPE"))
                        dsAgentDetails.Tables["AGENT_INFO"].Columns.Add("AIRPORT_TYPE");
                    dsPnrDetails.Tables.Add(dsAgentDetails.Tables["AGENT_INFO"].Copy());
                    string FromCity = string.Empty;
                    string FromCategory = string.Empty;
                    string ToCity = string.Empty;
                    string ToCategory = string.Empty;
                    var qryOrigin = (from p in dsPnrDetails.Tables["PassengerPNRDetails"].AsEnumerable()
                                     select new
                                     {
                                         ORIGIN = p.Field<string>("ORIGIN"),
                                         DESTINATION = p.Field<string>("DESTINATION")
                                     }).Distinct();

                    DataSet dsMasterData = new DataSet();

                    string AirportType = string.Empty;
                    string strPath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, @"XML\\City_Airport.xml");
                    dsMasterData.ReadXml(strPath);//                
                    if (qryOrigin.Count() > 0)
                    {
                        DataTable dtCategory = ConvertToDataTable(qryOrigin);
                        if (dsMasterData.Tables["CITYAIRPORTDET"].Rows.Count > 0)
                        {
                            foreach (DataRow drCity in dtCategory.Rows)
                            {
                                FromCity = drCity["ORIGIN"].ToString();
                                ToCity = drCity["DESTINATION"].ToString();

                                var qryFromCategory = from p in dsMasterData.Tables["CITYAIRPORTDET"].AsEnumerable()
                                                      where p.Field<string>("ID") == FromCity.ToUpper().Trim()
                                                      select p;
                                DataView dvFromCategory = qryFromCategory.AsDataView();
                                if (dvFromCategory.Count != 0)
                                {
                                    dtCategory = qryFromCategory.CopyToDataTable();
                                    FromCategory = dtCategory.Rows[0]["COUNTRY"].ToString();
                                }

                                var qryToCategory = from p in dsMasterData.Tables["CITYAIRPORTDET"].AsEnumerable()
                                                    where p.Field<string>("ID") == ToCity.ToUpper().Trim()
                                                    select p;
                                DataView dvToCategory = qryToCategory.AsDataView();
                                if (dvToCategory.Count != 0)
                                {
                                    dtCategory = qryToCategory.CopyToDataTable();
                                    ToCategory = dtCategory.Rows[0]["COUNTRY"].ToString();
                                }
                                if (FromCategory.ToUpper().Trim() == Domestic_Countrycode && ToCategory.ToUpper().Trim() == Domestic_Countrycode)
                                {
                                    AirportType = "D";
                                }
                                else
                                {
                                    AirportType = "I";
                                    break;
                                }
                            }

                            dsPnrDetails.Tables["AGENT_INFO"].Rows[0]["AIRPORT_TYPE"] = AirportType;
                            if (AirportType.ToUpper().Trim() == "I")
                            {
                                var qryTST = (from p in dsPnrDetails.Tables["PassengerPNRDetails"].AsEnumerable()
                                              select new
                                              {
                                                  TSTCOUNT = p.Field<string>("TSTCOUNT")
                                              }).Distinct();
                                if (qryTST.Count() > 1)
                                {
                                    strErrorMsg = "Multi TST PNR is not allowed for international ticketing";
                                }
                            }
                        }
                        else
                        {
                            strErrorMsg = "Problem occured while retrive the PNR details (#01).";
                            pnrResult[Error] = strErrorMsg.ToString();
                            //return pnrResult;
                            return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                        }
                    }
                    else
                    {
                        strErrorMsg = "Problem occured while retrive the PNR details (#02).";
                        pnrResult[Error] = strErrorMsg.ToString();
                        //return pnrResult;
                        return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                    }

                    //DataTable dtPNRDetails = new DataTable("PassengerPNRDetails");
                    //if (dsPnrDetails.Tables.Contains("PassengerPNRDetails"))
                    //{
                    //    DataTable dtTcktPNRDetails = new DataTable();
                    //    DataTable dtAccountPNRDetails = new DataTable();

                    //    var qryTcktPNRTemp = from p in dsPnrDetails.Tables["PassengerPNRDetails"].AsEnumerable()// ticketed PNR's
                    //                         where p.Field<string>("TICKETNO").ToString() != ""
                    //                         select p;
                    //    DataView dvTcktPNRTemp = qryTcktPNRTemp.AsDataView();
                    //    if (dvTcktPNRTemp.Count > 0)
                    //        dtTcktPNRDetails = qryTcktPNRTemp.CopyToDataTable();

                    //    // check pnr exist or not
                    //    bool PNRExist = Check_and_fetch_queuePNR(CrsPnr, strAgentID, strUserName, Ipaddress,
                    //           "W", Convert.ToDecimal(sequnceID), ref strErrorMsg, ref dtTcktPNRDetails, ref dtAccountPNRDetails);
                    //    if (PNRExist == true && dtTcktPNRDetails != null && dtTcktPNRDetails.Rows.Count > 0)
                    //    {
                    //        dtTcktPNRDetails.TableName = "TICKETED_DETAILS";
                    //        dsPnrDetails.Tables.Add(dtTcktPNRDetails.Copy());
                    //    }

                    //    var qryPNRTemp = from p in dsPnrDetails.Tables["PassengerPNRDetails"].AsEnumerable()// Non ticketed PNR's
                    //                     where p.Field<string>("TICKETNO").ToString() == ""
                    //                     select p;
                    //    DataView dvPNRTemp = qryPNRTemp.AsDataView();
                    //    if (dvPNRTemp.Count > 0)
                    //        dtPNRDetails = qryPNRTemp.CopyToDataTable();
                    //    else if (dtAccountPNRDetails == null || dtAccountPNRDetails.Rows.Count <= 0)
                    //    {
                    //        strErrorMsg = "This PNR is already ticketed. Please check in booked history";
                    //        pnrResult[Error] = strErrorMsg.ToString();
                    //        return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                    //    }

                    //    if (dtAccountPNRDetails != null && dtAccountPNRDetails.Rows.Count > 0)
                    //        dtPNRDetails.Merge(dtAccountPNRDetails.Copy());
                    //}

                    RetrivePnr = CalculatingAccountsForQueueTicketing("W", strAgentID, Ipaddress,
                        strUserName, Convert.ToDecimal(sequnceID), dsPnrDetails, ref strErrorMsg, ref dcTotalFare, ref strResult, RebookFlag, "PNR", strfareFlag);

                    dsPnrDetails = _ServiceUtility.convertJsonStringToDataSet(strResult, "");

                    pnrData = "PNR_INFO" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    Session[pnrData] = dsPnrDetails;
                    pnrResult[pnrdata] = pnrData.ToString();
                    DataTable PASSENGER_PNRINFO_TWO = new DataTable();
                    if (dsPnrDetails.Tables.Count > 0)
                    {
                        if (dsPnrDetails.Tables["PassengerPNRDetails"].Rows.Count > 0)
                            PASSENGER_PNRINFO_TWO = dsPnrDetails.Tables["PassengerPNRDetails"];
                    }
                    else
                    {
                        pnrResult[Error] = "Please enter valid crs pnr!";
                        //return pnrResult;
                        return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                    }
                    StringBuilder pnr_details = new StringBuilder();
                    pnr_details.Append("<div><h4 class='bStyle'><i class='fa fa-plane' style='padding-right:6px;'></i>Flight Details</h4></div>");
                    pnr_details.Append("<table id='table1' width='100%' cellpadding='3px' class='no-more-tables table-striped'>");//Origin</th><th>Destination//<th>Ticketing Carrier</th>
                    pnr_details.Append("<thead><tr class='dv_table_header' style='border: 1px solid #ddd;background:#b60725!important;color: #fff !important;text-align: center;'><th>Select</th><th>Ticketing status</th><th>Sector</th><th>Flight No</th><th>Departure Date</th><th>Arrival Date</th><th style='padding:5px;'>Class</th><th style='padding:5px;'>Baggage</th></tr></thead>");

                    //////////////////FLIGHT DETAILS   
                    DataTable tableTSTBased2 = new DataTable();
                    if (strfareFlag == "N")
                    {
                        var qryTST2NEW = (from p in dsPnrDetails.Tables["PassengerPNRDetails"].AsEnumerable()
                                          select new
                                          {
                                              TICKETINGCARRIER = p.Field<string>("TICKETINGCARRIER")
                                          }).Distinct();

                        tableTSTBased2 = ConvertToDataTable(qryTST2NEW);
                    }
                    else
                    {

                        var qryTST2 = (from p in dsPnrDetails.Tables["PassengerPNRDetails"].AsEnumerable()
                                       select new
                                       {
                                           TSTCOUNT = p.Field<string>("TSTCOUNT")
                                       }).Distinct();
                        tableTSTBased2 = ConvertToDataTable(qryTST2);
                    }

                    var QryTICKETCount = (from p in PASSENGER_PNRINFO_TWO.AsEnumerable()
                                          where p["TICKETNO"].ToString().Trim() == ""
                                          select new
                                          {
                                              TICKETNO = p.Field<string>("TICKETNO").Trim()
                                          }).Distinct();

                    if (QryTICKETCount.Count() < 1)
                    {
                        strTicketStatus = "1";
                    }
                    else
                    {
                        strTicketStatus = "0";
                    }

                    string str_paxcount = string.Empty;
                    DataTable dtPaxCount = new DataTable();
                    if (tableTSTBased2.Rows.Count > 0)
                    {
                        foreach (DataRow drTSTBased in tableTSTBased2.Rows)
                        {

                            if (strfareFlag == "N")
                            {
                                var paxcounts = (from p in dsPnrDetails.Tables["PAXDETAILS"].AsEnumerable()
                                                 where p["TICKETINGCARRIER"].ToString().Trim() == drTSTBased["TICKETINGCARRIER"].ToString().Trim()
                                                 select new
                                                 {
                                                     adultcount = p["ADULTCOUNT"].ToString().Trim(),
                                                     childcount = p["CHILDCOUNT"].ToString().Trim(),
                                                     infantcount = p["INFANTCOUNT"].ToString().Trim()
                                                 });

                                dtPaxCount = ConvertToDataTable(paxcounts);
                            }
                            else
                            {
                                var paxcounts = (from p in dsPnrDetails.Tables["PAXDETAILS"].AsEnumerable()
                                                 where p["TSTCOUNT"].ToString().Trim() == drTSTBased["TSTCOUNT"].ToString().Trim()
                                                 select new
                                                 {
                                                     adultcount = p["ADULTCOUNT"].ToString().Trim(),
                                                     childcount = p["CHILDCOUNT"].ToString().Trim(),
                                                     infantcount = p["INFANTCOUNT"].ToString().Trim()
                                                 });

                                dtPaxCount = ConvertToDataTable(paxcounts);
                            }
                            if (string.IsNullOrEmpty(str_paxcount))
                            {
                                str_paxcount = dtPaxCount.Rows[0]["adultcount"].ToString() + "|" + dtPaxCount.Rows[0]["childcount"].ToString() + "|" + dtPaxCount.Rows[0]["infantcount"].ToString();
                            }
                            else
                            {
                                str_paxcount = str_paxcount + "," + dtPaxCount.Rows[0]["adultcount"].ToString() + "|" + dtPaxCount.Rows[0]["childcount"].ToString() + "|" + dtPaxCount.Rows[0]["infantcount"].ToString();
                            }


                            DataTable dtSegment = new DataTable();
                            DataTable dtSegmentnew = new DataTable();
                            if (strfareFlag == "N")
                            {
                                var QrySegment = (from p in PASSENGER_PNRINFO_TWO.AsEnumerable()
                                                  where p["TICKETINGCARRIER"].ToString().Trim() == drTSTBased["TICKETINGCARRIER"].ToString().Trim()
                                                  select new
                                                  {
                                                      ORIGIN = p.Field<string>("ORIGIN").Trim(),
                                                      DESTINATION = p.Field<string>("DESTINATION").Trim(),
                                                      AIRLINECODE = p.Field<string>("AIRLINECODE").Trim(),
                                                      CLASS = p.Field<string>("CLASS").Trim(),
                                                      DEPARTUREDATE = p.Field<string>("DEPARTUREDATE").Replace(" ", "").Trim() + ' ' + p.Field<string>("DEPARTURETIME").Replace(" ", "").Trim(),
                                                      ARRIVALDATE = p.Field<string>("ARRIVALDATE").Replace(" ", "").Trim() + ' ' + p.Field<string>("ARRIVALTIME").Replace(" ", "").Trim(),
                                                      FLIGHTNO = p.Field<string>("FLIGHTNO").Trim(),
                                                      TICKETINGCARRIER = p.Field<string>("TICKETINGCARRIER").Trim(),
                                                      BAGGAGE = p.Field<string>("BAGGAGE").Trim(),
                                                      FareID = p.Field<string>("FareID").Trim(),
                                                      FareQualifier = p.Field<string>("FareQualifier").Trim(),
                                                      //PAXREFERENCE = p.Field<string>("PAXREFERENCE").Trim(),
                                                      TSTREFERENCE = p.Field<string>("TSTREFERENCE").Trim(),
                                                      TSTCOUNT = p.Field<string>("TSTCOUNT").Trim()
                                                  }).Distinct();

                                dtSegment = ConvertToDataTable(QrySegment);

                                var QrySegmentNew = (from p in PASSENGER_PNRINFO_TWO.AsEnumerable()
                                                     where p["TICKETINGCARRIER"].ToString().Trim() == drTSTBased["TICKETINGCARRIER"].ToString().Trim()
                                                     group p by p["PAXREFERENCE"] into g
                                                     select new
                                                     {
                                                         ORIGIN = g.FirstOrDefault().Field<string>("ORIGIN").Trim(),
                                                         DESTINATION = g.FirstOrDefault().Field<string>("DESTINATION").Trim(),
                                                         AIRLINECODE = g.FirstOrDefault().Field<string>("AIRLINECODE").Trim(),
                                                         CLASS = g.FirstOrDefault().Field<string>("CLASS").Trim(),
                                                         DEPARTUREDATE = g.FirstOrDefault().Field<string>("DEPARTUREDATE").Replace(" ", "").Trim() + ' ' + g.FirstOrDefault().Field<string>("DEPARTURETIME").Replace(" ", "").Trim(),
                                                         ARRIVALDATE = g.FirstOrDefault().Field<string>("ARRIVALDATE").Replace(" ", "").Trim() + ' ' + g.FirstOrDefault().Field<string>("ARRIVALTIME").Replace(" ", "").Trim(),
                                                         FLIGHTNO = g.FirstOrDefault().Field<string>("FLIGHTNO").Trim(),
                                                         TICKETINGCARRIER = g.FirstOrDefault().Field<string>("TICKETINGCARRIER").Trim(),
                                                         BAGGAGE = g.FirstOrDefault().Field<string>("BAGGAGE").Trim(),
                                                         FareID = g.FirstOrDefault().Field<string>("FareID").Trim(),
                                                         FareQualifier = g.FirstOrDefault().Field<string>("FareQualifier").Trim(),
                                                         PAXREFERENCE = g.FirstOrDefault().Field<string>("PAXREFERENCE").Trim(),
                                                         TSTCOUNT = g.FirstOrDefault().Field<string>("TSTCOUNT").Trim()
                                                         //TSTCOUNT = g.FirstOrDefault().Field<string>("TSTCOUNT").Trim() == "" ? "1" : g.FirstOrDefault().Field<string>("TSTCOUNT").Trim()
                                                     }).Distinct();
                                dtSegmentnew = ConvertToDataTable(QrySegmentNew);
                            }
                            else
                            {
                                var QrySegment = (from p in PASSENGER_PNRINFO_TWO.AsEnumerable()
                                                  where p["TSTCOUNT"].ToString().Trim() == drTSTBased["TSTCOUNT"].ToString().Trim()
                                                  select new
                                                  {
                                                      ORIGIN = p.Field<string>("ORIGIN").Trim(),
                                                      DESTINATION = p.Field<string>("DESTINATION").Trim(),
                                                      AIRLINECODE = p.Field<string>("AIRLINECODE").Trim(),
                                                      CLASS = p.Field<string>("CLASS").Trim(),
                                                      DEPARTUREDATE = p.Field<string>("DEPARTUREDATE").Replace(" ", "").Trim() + ' ' + p.Field<string>("DEPARTURETIME").Replace(" ", "").Trim(),
                                                      ARRIVALDATE = p.Field<string>("ARRIVALDATE").Replace(" ", "").Trim() + ' ' + p.Field<string>("ARRIVALTIME").Replace(" ", "").Trim(),
                                                      FLIGHTNO = p.Field<string>("FLIGHTNO").Trim(),
                                                      TICKETINGCARRIER = p.Field<string>("TICKETINGCARRIER").Trim(),
                                                      BAGGAGE = p.Field<string>("BAGGAGE").Trim(),
                                                      FareID = p.Field<string>("FareID").Trim(),
                                                      FareQualifier = p.Field<string>("FareQualifier").Trim(),
                                                      TSTREFERENCE = p.Field<string>("TSTREFERENCE").Trim(),
                                                      //PAXREFERENCE = p.Field<string>("PAXREFERENCE").Trim(),
                                                      TSTCOUNT = p.Field<string>("TSTCOUNT").Trim()
                                                  }).Distinct();

                                dtSegment = ConvertToDataTable(QrySegment);

                                var QrySegmentNew = (from p in PASSENGER_PNRINFO_TWO.AsEnumerable()
                                                     where p["TSTCOUNT"].ToString().Trim() == drTSTBased["TSTCOUNT"].ToString().Trim()
                                                     group p by p["PAXREFERENCE"] into g
                                                     select new
                                                     {
                                                         ORIGIN = g.FirstOrDefault().Field<string>("ORIGIN").Trim(),
                                                         DESTINATION = g.FirstOrDefault().Field<string>("DESTINATION").Trim(),
                                                         AIRLINECODE = g.FirstOrDefault().Field<string>("AIRLINECODE").Trim(),
                                                         CLASS = g.FirstOrDefault().Field<string>("CLASS").Trim(),
                                                         DEPARTUREDATE = g.FirstOrDefault().Field<string>("DEPARTUREDATE").Replace(" ", "").Trim() + ' ' + g.FirstOrDefault().Field<string>("DEPARTURETIME").Replace(" ", "").Trim(),
                                                         ARRIVALDATE = g.FirstOrDefault().Field<string>("ARRIVALDATE").Replace(" ", "").Trim() + ' ' + g.FirstOrDefault().Field<string>("ARRIVALTIME").Replace(" ", "").Trim(),
                                                         FLIGHTNO = g.FirstOrDefault().Field<string>("FLIGHTNO").Trim(),
                                                         TICKETINGCARRIER = g.FirstOrDefault().Field<string>("TICKETINGCARRIER").Trim(),
                                                         BAGGAGE = g.FirstOrDefault().Field<string>("BAGGAGE").Trim(),
                                                         FareID = g.FirstOrDefault().Field<string>("FareID").Trim(),
                                                         FareQualifier = g.FirstOrDefault().Field<string>("FareQualifier").Trim(),
                                                         PAXREFERENCE = g.FirstOrDefault().Field<string>("PAXREFERENCE").Trim(),
                                                         TSTCOUNT = g.FirstOrDefault().Field<string>("TSTCOUNT").Trim()
                                                         //TSTCOUNT = g.FirstOrDefault().Field<string>("TSTCOUNT").Trim() == "" ? "1" : g.FirstOrDefault().Field<string>("TSTCOUNT").Trim()
                                                     }).Distinct();
                                dtSegmentnew = ConvertToDataTable(QrySegmentNew);
                            }

                            string segment_car = string.Empty;
                            int i = 0;//for no of radio count
                            int y = 0;

                            if (strfareFlag == "N")
                            {
                                foreach (DataRow dr in dtSegmentnew.Rows)
                                {
                                    //if (segment_car != dsPnrDetails.Tables["PassengerPNRDetails"].Rows[i]["SEGMENTNO"].ToString())
                                    //{
                                    //if (!AirlineCET.Contains(dr["TICKETINGCARRIER"].ToString().Trim()))
                                    //{
                                    AirlineCET += dr["TICKETINGCARRIER"].ToString().Trim() + "~" + dr["ORIGIN"].ToString().Trim() + "-" + dr["DESTINATION"].ToString().Trim() + '~' + dr["PAXREFERENCE"].ToString().Trim() + "~" + dr["TICKETINGCARRIER"].ToString().Trim() + "*";
                                    //AirlineCET += dr["TICKETINGCARRIER"].ToString().Trim() + "~" + dr["ORIGIN"].ToString().Trim() + "-" + dr["DESTINATION"].ToString().Trim() + '~' + "" + "~" + dr["TSTCOUNT"].ToString().Trim() + "|";
                                    //AirlineCET += dr["TICKETINGCARRIER"].ToString().Trim() + "~";
                                    //}
                                    // }
                                    strPlatingCarrier += dr["TICKETINGCARRIER"].ToString().Trim() + "~" + dr["TICKETINGCARRIER"].ToString().Trim() + "|";
                                    segment_car = dsPnrDetails.Tables["PassengerPNRDetails"].Rows[i]["SEGMENTNO"].ToString();
                                }
                            }
                            else
                            {
                                foreach (DataRow dr in dtSegmentnew.Rows)
                                {
                                    //if (segment_car != dsPnrDetails.Tables["PassengerPNRDetails"].Rows[i]["SEGMENTNO"].ToString())
                                    //{
                                    //if (!AirlineCET.Contains(dr["TICKETINGCARRIER"].ToString().Trim()))
                                    //{
                                    AirlineCET += dr["TICKETINGCARRIER"].ToString().Trim() + "~" + dr["ORIGIN"].ToString().Trim() + "-" + dr["DESTINATION"].ToString().Trim() + '~' + dr["PAXREFERENCE"].ToString().Trim() + "~" + dr["TSTCOUNT"].ToString().Trim() + "*";
                                    //AirlineCET += dr["TICKETINGCARRIER"].ToString().Trim() + "~" + dr["ORIGIN"].ToString().Trim() + "-" + dr["DESTINATION"].ToString().Trim() + '~' + "" + "~" + dr["TSTCOUNT"].ToString().Trim() + "|";
                                    //AirlineCET += dr["TICKETINGCARRIER"].ToString().Trim() + "~";
                                    //}
                                    // }
                                    strPlatingCarrier += dr["TICKETINGCARRIER"].ToString().Trim() + "~" + dr["TSTCOUNT"].ToString().Trim() + "|";
                                    segment_car = dsPnrDetails.Tables["PassengerPNRDetails"].Rows[i]["SEGMENTNO"].ToString();
                                }
                            }
                            if (strfareFlag == "N")
                            {
                                var QryTSTCount = (from p in PASSENGER_PNRINFO_TWO.AsEnumerable()
                                                   where p["TICKETINGCARRIER"].ToString().Trim() == drTSTBased["TICKETINGCARRIER"].ToString().Trim()
                                                   select new
                                                   {
                                                       TICKETNO = p.Field<string>("TICKETNO").Trim()
                                                   }).Distinct();

                                dtTSTCount = ConvertToDataTable(QryTSTCount);

                            }
                            else
                            {

                                var QryTSTCount = (from p in PASSENGER_PNRINFO_TWO.AsEnumerable()
                                                   where p["TSTCOUNT"].ToString().Trim() == drTSTBased["TSTCOUNT"].ToString().Trim()
                                                   select new
                                                   {
                                                       TICKETNO = p.Field<string>("TICKETNO").Trim()
                                                   }).Distinct();

                                dtTSTCount = ConvertToDataTable(QryTSTCount);
                            }

                            foreach (DataRow dr in dtSegment.Rows)
                            {
                                //if (segment_car != dsPnrDetails.Tables["PassengerPNRDetails"].Rows[i]["SEGMENTNO"].ToString())
                                //{
                                //    //if (!AirlineCET.Contains(dr["TICKETINGCARRIER"].ToString().Trim()))
                                //    //{
                                //        //AirlineCET += dr["TICKETINGCARRIER"].ToString().Trim() + "~" + dr["ORIGIN"].ToString().Trim() + "-" + dr["DESTINATION"].ToString().Trim()  + '~' + dr["PAXREFERENCE"].ToString().Trim() + "~" + dr["TSTCOUNT"].ToString().Trim() + "|";
                                //    AirlineCET += dr["TICKETINGCARRIER"].ToString().Trim() + "~" + dr["ORIGIN"].ToString().Trim() + "-" + dr["DESTINATION"].ToString().Trim() + '~' + "" + "~" + dr["TSTCOUNT"].ToString().Trim() + "|";
                                //        //AirlineCET += dr["TICKETINGCARRIER"].ToString().Trim() + "~";
                                //    //}
                                //}
                                //segment_car = dsPnrDetails.Tables["PassengerPNRDetails"].Rows[i]["SEGMENTNO"].ToString();

                                if (strfareFlag == "N")
                                    pnr_details.Append("<tr id='" + y + "segrow" + drTSTBased["TICKETINGCARRIER"].ToString() + "' style='background:white;text-align: center;border-bottom: 1px dashed #fff;'>");
                                else
                                    pnr_details.Append("<tr id='" + y + "segrow" + drTSTBased["TSTCOUNT"].ToString() + "' style='background:white;text-align: center;border-bottom: 1px dashed #fff;'>");


                                if (strfareFlag == "N")
                                {
                                    if (dtTSTCount.Rows[0]["TICKETNO"].ToString().Trim() == "" && i == 0)
                                    {
                                        pnr_details.Append("<td class='clswhite' data-title='Select' rowspan=" + dtSegment.Rows.Count.ToString() + "><div><input type='checkbox' class='clscheck' onchange='ClearFareType(this)' data-tstref='" + dr["TSTREFERENCE"].ToString() + "'  valuex" + drTSTBased["TICKETINGCARRIER"].ToString() + " id='tst" + drTSTBased["TICKETINGCARRIER"].ToString() + "' style='visibility:hidden;'><label for='tst" + drTSTBased["TICKETINGCARRIER"].ToString() + "' style='cursor:pointer;display:inline-block;'><div class='RadioDeselected' id='" + drTSTBased["TICKETINGCARRIER"].ToString() + "'  data-FareQualifier='" + dr["FareQualifier"].ToString().Trim() + "'  onclick='checkClick(this)'></div></label></div></td>");
                                        pnr_details.Append("<td class='clswhite' data-title='Ticketing status' rowspan=" + dtSegment.Rows.Count.ToString() + "><div style='color:blue;'>Pending</div></td>");
                                        i++;
                                    }
                                    else if (dtTSTCount.Rows[0]["TICKETNO"].ToString().Trim() != "" && i == 0)
                                    {
                                        pnr_details.Append("<td class='clswhite' data-title='Select' rowspan=" + dtSegment.Rows.Count.ToString() + "><div><input type='checkbox' disabled='disabled' data-tstref='" + dr["TSTREFERENCE"].ToString() + "' valuex" + drTSTBased["TICKETINGCARRIER"].ToString() + " id='tst" + drTSTBased["TICKETINGCARRIER"].ToString() + "' style='visibility:hidden;cursor:not-allowed'><label for='tst" + drTSTBased["TICKETINGCARRIER"].ToString() + "' style='cursor:pointer;display:inline-block;'><div style='cursor: not-allowed;' class='RadioDeselected' id='" + drTSTBased["TICKETINGCARRIER"].ToString() + "'  data-FareQualifier='" + dr["FareQualifier"].ToString().Trim() + "' disabled='diasabled'></div></label></div></td>");
                                        pnr_details.Append("<td class='clswhite' data-title='Ticketing status' rowspan=" + dtSegment.Rows.Count.ToString() + "><div class='ticked-pnr'>Ticketed</div></td>");
                                        //pnr_details.Append("<td style='color:#da0000;'>Booked</td>");
                                        i++;
                                    }
                                }
                                else
                                {
                                    if (dtTSTCount.Rows[0]["TICKETNO"].ToString().Trim() == "" && i == 0)
                                    {
                                        pnr_details.Append("<td class='clswhite' data-title='Select' rowspan=" + dtSegment.Rows.Count.ToString() + "><div><input type='checkbox' class='clscheck' onchange='ClearFareType(this)' data-tstref='" + dr["TSTREFERENCE"].ToString() + "'  valuex" + drTSTBased["TSTCOUNT"].ToString() + " id='tst" + drTSTBased["TSTCOUNT"].ToString() + "' style='visibility:hidden;'><label for='tst" + drTSTBased["TSTCOUNT"].ToString() + "' style='cursor:pointer;display:inline-block;'><div class='RadioDeselected' id='" + drTSTBased["TSTCOUNT"].ToString() + "'  data-FareQualifier='" + dr["FareQualifier"].ToString().Trim() + "'  onclick='checkClick(this)'></div></label></div></td>");
                                        pnr_details.Append("<td class='clswhite' data-title='Ticketing status' rowspan=" + dtSegment.Rows.Count.ToString() + "><div style='color:blue;'>Pending</div></td>");
                                        i++;
                                    }
                                    else if (dtTSTCount.Rows[0]["TICKETNO"].ToString().Trim() != "" && i == 0)
                                    {
                                        pnr_details.Append("<td class='clswhite' data-title='Select' rowspan=" + dtSegment.Rows.Count.ToString() + "><div><input type='checkbox' disabled='disabled' valuex" + drTSTBased["TSTCOUNT"].ToString() + " data-tstref='" + dr["TSTREFERENCE"].ToString() + "' id='tst" + drTSTBased["TSTCOUNT"].ToString() + "' style='visibility:hidden;cursor:not-allowed'><label for='tst" + drTSTBased["TSTCOUNT"].ToString() + "' style='cursor:pointer;display:inline-block;'><div style='cursor: not-allowed;' class='RadioDeselected' id='" + drTSTBased["TSTCOUNT"].ToString() + "'  data-FareQualifier='" + dr["FareQualifier"].ToString().Trim() + "' disabled='diasabled'></div></label></div></td>");
                                        pnr_details.Append("<td class='clswhite' data-title='Ticketing status' rowspan=" + dtSegment.Rows.Count.ToString() + "><div class='ticked-pnr'>Ticketed</div></td>");
                                        //pnr_details.Append("<td style='color:#da0000;'>Booked</td>");
                                        i++;
                                    }
                                }

                                pnr_details.Append("<td class='clswhite' data-title='Sector'>" + dr["ORIGIN"].ToString().Trim() + " → " + dr["DESTINATION"].ToString().Trim() + "</td>");
                                //pnr_details.Append("<td class='clswhite' data-title='Origin'>" + dr["ORIGIN"].ToString().Trim() + "</td>");
                                //pnr_details.Append("<td class='clswhite' data-title='Destination'>" + dr["DESTINATION"].ToString().Trim() + "</td>");
                                pnr_details.Append("<td class='clswhite' data-title='Flight No'>" + dr["AIRLINECODE"].ToString().Trim() + " " + dr["FLIGHTNO"].ToString().Trim() + "</td>");
                                //pnr_details.Append("<td class='clswhite' data-title='Ticketing Carrier'>" + dr["TICKETINGCARRIER"].ToString().Trim() + "</td>");
                                //DateTime dtD = DateTime.Parse(dr["DEPARTUREDATE"].ToString());

                                DateTime dtD = DateTime.ParseExact(dr["DEPARTUREDATE"].ToString().Trim(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                DateTime dtA = DateTime.ParseExact(dr["ARRIVALDATE"].ToString().Trim(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                                string DepartureDate = dtD.ToString("dd MMM yyyy");
                                string DepartureTime = dtD.ToString("HH:mm");

                                string ArrivalDate = dtA.ToString("dd MMM yyyy");
                                string ArrivalTime = dtA.ToString("HH:mm");

                                pnr_details.Append("<td class='clswhite' data-title='Departure Date'>" + DepartureDate + " <b>" + DepartureTime + "</b></td>");
                                pnr_details.Append("<td class='clswhite' data-title='Arrival Date'>" + ArrivalDate + " <b>" + ArrivalTime + "</b></td>");
                                pnr_details.Append("<td class='clswhite' data-title='Class'>" + dr["CLASS"].ToString().Trim() + "</td>");
                                pnr_details.Append("<td class='clswhite clsbold' data-title='BAGGAGE'>" + dr["BAGGAGE"].ToString().Trim() + "</td>");
                                strPLATINGCARRIER = dr["AIRLINECODE"].ToString().Trim();

                                pnr_details.Append("</tr>");
                                y++;
                            }

                            pnrResult[TicketStatus] = strTicketStatus;

                            //DataSet PromocodeDet = GetpromocodeNEW(BranchID, Corporatename);
                            ////string strFre_Code = FareType == "C" ? "CC" : FareType == "R" ? "RC" : "PC";//_PCCode["TC_TYPE"].ToString().Trim() == strFre_Code && 
                            //if (PromocodeDet != null && PromocodeDet.Tables.Count > 0 && PromocodeDet.Tables[0].Rows.Count > 0)
                            //{
                            //    var PromocodeS = (from _PCCode in PromocodeDet.Tables[0].AsEnumerable()
                            //                      where _PCCode["TC_AIRLINE"].ToString().Trim() == strPLATINGCARRIER && _PCCode["ftc_crs_id"].ToString().ToUpper().Trim() == CRS.ToString().ToUpper().Trim() && (_PCCode["TC_TYPE"].ToString().Trim() == "PC" || _PCCode["TC_TYPE"].ToString().Trim() == "CC")
                            //                      select new
                            //                      {
                            //                          PCCOde = _PCCode["TC_CODE"].ToString().Trim(),
                            //                          Airline = _PCCode["TC_AIRLINE"].ToString().Trim(),
                            //                          AirportType = _PCCode["TC_AIRPORT_TYPE"].ToString().Trim(),
                            //                          TYPE = _PCCode["TC_TYPE"].ToString().Trim()
                            //                      }
                            //                         ).ToList();
                            //    if (PromocodeS.Count > 0)
                            //    {
                            //        strCorporatecode = PromocodeS.FirstOrDefault().PCCOde.ToString();
                            //    }
                            //    else
                            //    {
                            //        strCorporatecode = "";
                            //    }
                            //    Session["QueueCorporateCode" + CrsPnr.ToUpper().Trim()] = strCorporatecode;
                            //    strCorporatecode = strTerminalType.ToUpper().Trim() == "T" ? strCorporatecode : "";
                            //}

                        }
                    }
                    else
                    {
                        pnrResult[Error] = "Currently unable to process your request! L280";
                        //return pnrResult;
                        return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                    }

                    pnr_details.Append("</table>");


                    string farebreakup = string.Empty;

                    pnr_details.Append("<div><h4 class='bStyle'><i class='fa fa-fax' style='padding-right:6px;'></i>Pax Details</h4></div>");//style='margin-top: 2%;'
                    pnr_details.Append("<table id='table2' class='table no-more-tables table-striped' width='100%' style='border: 1px solid #ccc;' cellpadding='3px'>");

                    int adult_select = 0;
                    int child_select = 0;
                    int infant_select = 0;


                    string tstcount = string.Empty;
                    int tstcount_arrayform = 0;

                    if (strfareFlag == "N")
                    {
                        var qryTST_TWO = (from p in dsPnrDetails.Tables["PassengerPNRDetails"].AsEnumerable()
                                          select new
                                          {
                                              TICKETINGCARRIER = p.Field<string>("TICKETINGCARRIER")
                                          }).Distinct();

                        DataTable tableTSTBased = ConvertToDataTable(qryTST_TWO);
                        if (tableTSTBased.Rows.Count > 0)
                        {
                            //for each tstcount wise pax details
                            totgrossamount = 0;
                            int for_hr_line = 0;
                            int wit = 0, wot = 0;
                            foreach (DataRow drTSTBased in tableTSTBased.Rows)
                            {
                                int adult_selectNEW = 0;
                                int child_selectNEW = 0;
                                int infant_selectNEW = 0;

                                if (tstcount_arrayform == 0) { tstcount = drTSTBased["TICKETINGCARRIER"].ToString(); } else { tstcount = tstcount + "," + drTSTBased["TICKETINGCARRIER"].ToString(); }

                                var getFareBasedOnTSTCOUNT = from p in dsPnrDetails.Tables["PAXDETAILS"].AsEnumerable()
                                                             where p["TICKETINGCARRIER"].ToString().Trim() == drTSTBased["TICKETINGCARRIER"].ToString().Trim()
                                                             select p;

                                DataTable dt = getFareBasedOnTSTCOUNT.CopyToDataTable();

                                //getting updated basic fare total fare and tax amount for adultt, child and infant based on current loop tstcount
                                string[] UPDATED_BASIC_FARE_TAG = dt.Rows[0]["BASIC_FARE_TAG"].ToString().Split('|');

                                string[] UPDATED_TAXAMOUNT = dt.Rows[0]["TAXAMOUNT"].ToString().Split('|');

                                string[] UPDATED_TOTALFARETAG = dt.Rows[0]["TOTALFARETAG"].ToString().Split('|');

                                // for origin and destination of current loop tstcount
                                var orgDestTST = (from p in dsPnrDetails.Tables["PassengerPNRDetails"].AsEnumerable()
                                                  where p.Field<string>("TICKETINGCARRIER").ToString().ToUpper().Trim() == drTSTBased["TICKETINGCARRIER"].ToString().ToUpper().Trim()
                                                  select new
                                                  {
                                                      ORIGIN = p.Field<string>("ORIGIN").Trim(),
                                                      DESTINATION = p.Field<string>("DESTINATION").Trim()
                                                  }).Distinct();
                                DataTable OriginTable = ConvertToDataTable(orgDestTST);

                                //if (for_hr_line != 0)
                                //    pnr_details.Append("<tr><td colspan='8'><hr style='border:0;'></td><tr>");//STS-166

                                // pnr_details.Append("<tr><td colspan='8' style='color:rgb(116,40,148);font-weight:bold;text-align: left !important;'>");
                                string perTripTSTCOUNT = string.Empty;
                                perTripTSTCOUNT = OriginTable.Rows[0]["ORIGIN"].ToString() + " → " + OriginTable.Rows[OriginTable.Rows.Count - 1]["DESTINATION"].ToString();
                                //pnr_details.Append(perTripTSTCOUNT + "</td></tr>");

                                //
                                var qryPassenger = (from p in PASSENGER_PNRINFO_TWO.AsEnumerable()
                                                    where p["TICKETINGCARRIER"].ToString().Trim() == drTSTBased["TICKETINGCARRIER"].ToString().Trim()
                                                    group p by p["PAXNO"]
                                                        into g
                                                    // orderby p["PAXTYPE"] ascending
                                                    select new
                                                    {
                                                        //TITLE = ((g.Field<string>("TITLE") == null || string.IsNullOrEmpty(g.Field<string>("TITLE"))) ? "" : p.Field<string>("TITLE").Trim()),
                                                        //FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                                        //LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                                        //DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                                                        //PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                                                        //BASEAMT = p.Field<string>("BASEAMT").Trim(),
                                                        //TOTALTAXAMT = p.Field<string>("TOTALTAXAMT").Trim(),
                                                        //GROSSAMT = p.Field<string>("GROSSAMT").Trim(),
                                                        //BAGGAGE = p.Field<string>("BAGGAGE").Trim(),
                                                        ////SEQNO = p.Field<string>("SEQNO").Trim(),
                                                        //PAXNO = p.Field<string>("PAXNO").Trim(),
                                                        //TSTCOUNT = p.Field<string>("TSTCOUNT").Trim(),
                                                        //TICKETNO = p.Field<string>("TICKETNO").Trim(),
                                                        //PAXREFNO = p.Field<string>("PAXREFERENCE").Trim()

                                                        TITLE = g.FirstOrDefault().Field<string>("TITLE").Trim() == null || string.IsNullOrEmpty(g.FirstOrDefault().Field<string>("TITLE").Trim()) ? "" : g.FirstOrDefault().Field<string>("TITLE").Trim(),
                                                        FIRSTNAME = g.FirstOrDefault().Field<string>("FIRSTNAME").Trim(),
                                                        LASTNAME = g.FirstOrDefault().Field<string>("LASTNAME").Trim(),
                                                        DATEOFBIRTH = (g.FirstOrDefault().Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(g.FirstOrDefault().Field<string>("DATEOFBIRTH"))) ? "" : g.FirstOrDefault().Field<string>("DATEOFBIRTH").Trim(),
                                                        PAXTYPE = g.FirstOrDefault().Field<string>("PAXTYPE").Trim(),
                                                        BASEAMT = g.FirstOrDefault().Field<string>("BASEAMT").Trim(),
                                                        TOTALTAXAMT = g.FirstOrDefault().Field<string>("TOTALTAXAMT").Trim(),
                                                        GROSSAMT = g.FirstOrDefault().Field<string>("GROSSAMT").Trim(),
                                                        BAGGAGE = g.FirstOrDefault().Field<string>("BAGGAGE").Trim(),
                                                        //SEQNO = p.Field<string>("SEQNO").Trim(),
                                                        PAXNO = g.FirstOrDefault().Field<string>("PAXNO").Trim(),
                                                        TSTCOUNT = g.FirstOrDefault().Field<string>("TSTCOUNT").Trim(),
                                                        TICKETNO = g.FirstOrDefault().Field<string>("TICKETNO").Trim(),
                                                        PAXREFNO = g.FirstOrDefault().Field<string>("PAXREFERENCE").Trim(),
                                                        TICKETINGCARRIER = g.FirstOrDefault().Field<string>("TICKETINGCARRIER").Trim(),
                                                        DISCOUNT = g.FirstOrDefault().Field<string>("EARNINGS").Trim(),
                                                        MARKUP = g.FirstOrDefault().Field<string>("MARKUP").Trim(),
                                                        PLB = g.FirstOrDefault().Field<string>("PLB").Trim()
                                                    }).Distinct();

                                DataTable dtPassenger = ConvertToDataTable(qryPassenger);

                                if (dsPnrDetails.Tables["PAXDETAILS"].Rows.Count < 0)
                                {
                                    pnrResult[Error] = "Problem occured while fetching pnr details!";
                                    //return pnrResult;
                                    return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                                }

                                decimal total_fare_value_for_radio = 0;
                                int adt = 0;
                                int chd = 0;
                                int inf = 0;

                                string tstno = string.Empty;
                                foreach (DataRow dr in dtPassenger.Rows)
                                {
                                    if (dr["TICKETNO"].ToString().Trim() != "" && tstno != dr["TICKETINGCARRIER"].ToString().Trim())
                                    {
                                        //pnr_details.Append("<thead><tr class='dv_table_header' style='background: #59c45a;color: #fff;text-align: center;'>
                                        //<th class='td_center'>Title</th><th class='td_center'>Name</th><th class='td_center'>Date of Birth</th><th class='td_center'>Gender</th>
                                        //<th class='td_center'>Ticket No.</th><th class='td_center'>Type</th>");
                                        if (wit == 0)
                                        {
                                            pnr_details.Append("<thead><tr class='dv_table_header' style='background: #27a5f5;color: #fff;text-align: center;'><th class='td_center'>Title</th><th class='td_center'>Name</th><th class='td_center'>Ticket No.</th><th class='td_center'>Type</th><th class='td_center'>Sector</th>");
                                            pnr_details.Append("<th class='td_right'><nobr>Base Amount<nobr></th><th class='td_right'><nobr>Tax Amount<nobr></th><th class='td_right'><nobr>Gross Amount</nobr></th><th class='td_right clsearn'>Commission</th><th class='td_right clsearn'>PLB</th><th class='td_right clsearn'>Markup</th></tr></thead>");//<th class='td_center' style='padding:5px;'>Baggage</th>
                                            wit++;
                                        }
                                    }
                                    else if (dr["TICKETNO"].ToString().Trim() == "" && tstno != dr["TICKETINGCARRIER"].ToString().Trim())
                                    {
                                        if (wot == 0)
                                        {
                                            pnr_details.Append("<thead><tr class='dv_table_header' style='background: #27a5f5;color: #fff;text-align: center;'><th class='td_center'>Title</th><th class='td_center'>Name</th>");

                                            if (AirportTypes == "I")
                                            {
                                                pnr_details.Append("<th class='td_center'><nobr>Date of Birth</nobr></th><th class='td_center'>Gender</th>");
                                            }
                                            pnr_details.Append("<th class='td_center'>Ticket No.</th><th class='td_center'>Sector</th><th class='td_center'>Type</th>");
                                            if (AirportTypes == "I")
                                            {
                                                pnr_details.Append("<th class='td_center'><nobr>Passport No.<nobr></th>");
                                                pnr_details.Append("<th class='td_center'><nobr>Passport Iss. Country<nobr></th>");
                                                pnr_details.Append("<th class='td_center'><nobr>Passport Exp. Date<nobr></th>");
                                            }
                                            pnr_details.Append("<th class='td_right'><nobr>Base Amount<nobr></th><th class='td_right'><nobr>Tax Amount<nobr></th><th class='td_right'><nobr>Gross Amount</nobr></th><th class='td_right clsearn'>Commission</th><th class='td_right clsearn'>PLB</th><th class='td_right clsearn'>Markup</th></tr></thead>");//<th class='td_center' style='padding:5px;'>Baggage</th>
                                            wot++;
                                        }
                                    }

                                    string title = dr["TITLE"].ToString().ToUpper().Trim();
                                    if (dr["PAXTYPE"].ToString().Trim() == "ADT") ///FOR ADULT
                                    {
                                        adult_selectNEW++;
                                        TotCnt++;

                                        pnr_details.Append("<tr id='" + adt + "title_adult_tr" + dr["TICKETINGCARRIER"].ToString() + "' style='background:white'>");
                                        if (title != "DR" && title != "MR" && title != "MS" && title != "MRS")
                                        {
                                            adult_select++;
                                            pnr_details.Append("<td data-title='Title' class='td_center clswhite'><select style='width:80px;' id='" + dr["TICKETINGCARRIER"].ToString() + "title_adult" + adult_select + "' >");
                                            pnr_details.Append("<option value='Title' selected='selected'>Title</option>");
                                            pnr_details.Append("<option value='DR'>DR</option>");
                                            pnr_details.Append("<option value='MR'>MR</option>");
                                            pnr_details.Append("<option value='MS'>MS</option>");
                                            pnr_details.Append("<option value='MRS'>MRS</option>");
                                            pnr_details.Append("</select>" + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");
                                        }
                                        else
                                        {
                                            dr["FIRSTNAME"] = dr["FIRSTNAME"].ToString().Trim();
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Title'>" + title + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");
                                        }
                                        if (AirportTypes == "I")
                                        {

                                            if (string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                            {
                                                pnr_details.Append("<td class='clswhite' data-title='Date of birth.'> <input readonly='readonly' class='form-control ADTDatepickerint' maxlength='16' data-DOB=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "ADultDOB" + adult_selectNEW + "' placeholder='Date of Birth' type='text' value=''/>  </td>");
                                                pnr_details.Append("<td data-title='Gender' class='td_center clswhite'><select style='width:80px;' id='" + dr["TICKETINGCARRIER"].ToString() + "Gender_adult" + adult_selectNEW + "' >");
                                                pnr_details.Append("<option value='' selected='selected'>Gender</option>");
                                                pnr_details.Append("<option value='M'>Male</option>");
                                                pnr_details.Append("<option value='F'>Female</option>");
                                                pnr_details.Append("</select>" + "</td>");
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>" + dr["TICKETNO"].ToString().Trim() + "</td>");
                                        }
                                        else
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>-</td>");
                                        }
                                        pnr_details.Append("<td class='td_right clswhite' data-title='Sector'>" + perTripTSTCOUNT + "</td>");//STS-166
                                        pnr_details.Append("<td class='td_center clswhite' data-title='Type'>" + "ADULT" + "</td>");
                                        if (string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            if (AirportTypes == "I")
                                            {
                                                pnr_details.Append("<td class='clswhite' data-title='Passport No.'> <input class='passportno clspstAlphaWithOutSpace' maxlength='16' data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "Passwort_adult" + adult_selectNEW + "' placeholder='Passport No' onkeypress='javascript:return validateAlphandNumeric(event);' type='text' value=''/>  </td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Iss. Country'> <select data-IssuedCountry=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "IssuedCountry" + adult_selectNEW + "' class='form-control PassIssueCountry'></select></td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Exp. Date'> <input readonly='readonly' class='form-control PASSPORTDatepickerint' maxlength='16' data-ExpiryDate=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "ExpiryDate" + adult_selectNEW + "' placeholder='Passport Exp. Date' type='text' value=''/>  </td>");
                                            }
                                        }

                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Base Amount'><b>" + UPDATED_BASIC_FARE_TAG[0].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Tax Amount'><b>" + UPDATED_TAXAMOUNT[0].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Gross Amount'><b>" + UPDATED_TOTALFARETAG[0].ToString() + "</b></td>");

                                        if (strfareFlag == "Y")
                                            totgrossamount += Math.Round(Convert.ToDecimal(UPDATED_TOTALFARETAG[0].ToString()));

                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='disvalue'>" + dr["DISCOUNT"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='plbvalue'>" + dr["PLB"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='markvalue'>" + dr["MARKUP"].ToString().Trim() + "</label></td>");
                                        adt++;
                                    }
                                    if (dr["PAXTYPE"].ToString().Trim() == "CHD")   //FOR CHILD
                                    {
                                        child_selectNEW++;
                                        TotCnt++;

                                        pnr_details.Append("<tr id='" + chd + "title_child_tr" + dr["TICKETINGCARRIER"].ToString() + "' style='background:white'>");
                                        if (title != "MISS" && title != "MSTR")
                                        {
                                            child_select++;
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Title'><select style='width:80px;' id='" + dr["TICKETINGCARRIER"].ToString() + "title_child" + child_select + "'' onchange='chd_title_change(this.id)'>");
                                            pnr_details.Append("<option value='Title' selected='selected' >Title</option>");
                                            pnr_details.Append("<option value='MISS'>MISS</option>");
                                            pnr_details.Append("<option value='MSTR'>MSTR</option>");
                                            pnr_details.Append("</select>" + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");
                                        }
                                        else
                                        {
                                            dr["FIRSTNAME"] = dr["FIRSTNAME"].ToString().Trim();
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Title'>" + title + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");
                                        }

                                        if (AirportTypes == "I" && string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            pnr_details.Append("<td class='clswhite' data-title='Date of birth.'> <input readonly='readonly' class='form-control CHDDatepickerint' maxlength='16' data-DOB=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "ChildDOB" + child_selectNEW + "' placeholder='Date of Birth' type='text' value=''/>  </td>");
                                            pnr_details.Append("<td data-title='Gender' class='td_center clswhite'><select style='width:80px;' id='" + dr["TICKETINGCARRIER"].ToString() + "Gender_Child" + child_selectNEW + "'>");
                                            pnr_details.Append("<option value='' selected='selected'>Gender</option>");
                                            pnr_details.Append("<option value='M'>Male</option>");
                                            pnr_details.Append("<option value='F'>Female</option>");
                                            pnr_details.Append("</select>" + "</td>");
                                        }
                                        if (!string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>" + dr["TICKETNO"].ToString().Trim() + "</td>");
                                        }
                                        else
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>-</td>");
                                        }
                                        pnr_details.Append("<td class='td_right clswhite' data-title='Sector'>" + perTripTSTCOUNT + "</td>");//STS-166
                                        pnr_details.Append("<td class='td_center clswhite' data-title='Type'>" + "CHILD" + "</td>");

                                        if (string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            if (AirportTypes == "I")
                                            {
                                                pnr_details.Append("<td class='clswhite' data-title='Passport No.'> <input class='passportno clspstAlphaWithOutSpace' maxlength='16' onkeypress='javascript:return validateAlphandNumeric(event);' data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "Passwort_child" + child_selectNEW + "' placeholder='Passport No.' type='text' value=''/>  </td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Iss. Country'> <select data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "childIssuedCountry" + child_selectNEW + "' class='form-control PassIssueCountry'></select></td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Exp. Date'> <input readonly='readonly' class='form-control PASSPORTDatepickerint' maxlength='16' data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "childExpiryDate" + child_selectNEW + "' placeholder='Passport Exp. Date' type='text' value=''/>  </td>");
                                            }
                                        }

                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Base Amount'><b>" + UPDATED_BASIC_FARE_TAG[1].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Tax Amount'><b>" + UPDATED_TAXAMOUNT[1].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Gross Amount'><b>" + UPDATED_TOTALFARETAG[1].ToString() + "</b></td>");
                                        if (strfareFlag == "Y")
                                            totgrossamount += Math.Round(Convert.ToDecimal(UPDATED_TOTALFARETAG[1].ToString()));

                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='disvalue'>" + dr["DISCOUNT"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='plbvalue'>" + dr["PLB"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='markvalue'>" + dr["MARKUP"].ToString().Trim() + "</label></td>");
                                        chd++;

                                    }
                                    if (dr["PAXTYPE"].ToString().Trim() == "INF")   ///FOR INFANT
                                    {
                                        infant_selectNEW++;

                                        pnr_details.Append("<tr id='" + inf + "title_infant_tr" + dr["TICKETINGCARRIER"].ToString() + "' style='background:white'>");
                                        if (title != "MISS" && title != "MSTR")
                                        {
                                            infant_select++;
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Title'><select style='width:80px;' id='" + dr["TICKETINGCARRIER"].ToString() + "title_infant" + infant_select + "'' onchange='inf_title_change(this.id)'>");
                                            pnr_details.Append("<option value='Title' >Title</option>");
                                            pnr_details.Append("<option value='MISS'>MISS</option>");
                                            pnr_details.Append("<option value='MSTR' selected='selected'>MSTR</option>");
                                            pnr_details.Append("</select>" + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");

                                        }
                                        else
                                        {
                                            dr["FIRSTNAME"] = dr["FIRSTNAME"].ToString().Trim();
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Title'>" + title + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");
                                        }
                                        if (AirportTypes == "I" && string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            pnr_details.Append("<td class='clswhite' data-title='Date of birth.'> <input readonly='readonly' class='form-control INFDatepickerint' maxlength='16' data-DOB=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "InfantDOB" + infant_selectNEW + "' placeholder='Date of Birth' type='text' value=''/>  </td>");

                                            pnr_details.Append("<td data-title='Gender' class='td_center clswhite'><select style='width:80px;' id='" + dr["TICKETINGCARRIER"].ToString() + "Gender_Infant" + infant_selectNEW + "' onchange='adt_title_change(this.id)'>");
                                            pnr_details.Append("<option value='' selected='selected'>Gender</option>");
                                            pnr_details.Append("<option value='M'>Male</option>");
                                            pnr_details.Append("<option value='F'>Female</option>");
                                            pnr_details.Append("</select>" + "</td>");
                                        }
                                        if (!string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>" + dr["TICKETNO"].ToString().Trim() + "</td>");
                                        }
                                        else
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>-</td>");
                                        }
                                        pnr_details.Append("<td class='td_right clswhite' data-title='Sector'>" + perTripTSTCOUNT + "</td>");//STS-166
                                        pnr_details.Append("<td class='td_center clswhite'>" + "INFANT" + "</td>");
                                        if (string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            if (AirportTypes == "I")
                                            {
                                                pnr_details.Append("<td class='clswhite' data-title='Passport No.'> <input class='passportno clspstAlphaWithOutSpace' onkeypress='javascript:return validateAlphandNumeric(event);' data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "Passwort_infant" + infant_selectNEW + "' maxlength='16' placeholder='Passport No.' type='text' value=''/>  </td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Iss. Country'> <select data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "infantIssuedCountry" + infant_selectNEW + "' class='form-control PassIssueCountry'></select></td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Exp. Date'> <input  maxlength='16' data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TICKETINGCARRIER"].ToString() + "infantExpiryDate" + infant_selectNEW + "' placeholder='Passport Exp. Date' type='text' readonly='readonly' class='form-control PASSPORTDatepickerint' value=''/>  </td>");

                                            }
                                        }

                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Base Amount'><b>" + UPDATED_BASIC_FARE_TAG[2].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Tax Amount'><b>" + UPDATED_TAXAMOUNT[2].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Gross Amount'><b>" + UPDATED_TOTALFARETAG[2].ToString() + "</b></td>");
                                        if (strfareFlag == "Y")
                                            totgrossamount += Math.Round(Convert.ToDecimal(UPDATED_TOTALFARETAG[2].ToString()));

                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='disvalue'>" + dr["DISCOUNT"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='plbvalue'>" + dr["PLB"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='markvalue'>" + dr["MARKUP"].ToString().Trim() + "</label></td>");
                                        inf++;
                                    }

                                    //pnr_details.Append("<td class='td_center clswhite' data-title='Baggage'>" + dr["BAGGAGE"].ToString().Trim() + "</td>");
                                    pnr_details.Append("</tr>");
                                    tstno = dr["TSTCOUNT"].ToString().Trim();
                                }
                                int addnew = 0;
                                int chdnew = 0;
                                for (int l = 0; l < dsPnrDetails.Tables["PAX_INFO"].Rows.Count; l++)
                                {
                                    if (dsPnrDetails.Tables["PAX_INFO"].Rows[l]["PAXTYPE"].ToString() == "ADT")
                                    {
                                        addnew++;
                                    }
                                    else
                                    {
                                        chdnew++;
                                    }

                                }
                                AdtChd = addnew + "|" + chdnew;
                                adult_select = 0;
                                child_select = 0;
                                infant_select = 0;
                                var QryResult = (from p in dsPnrDetails.Tables["PAXDETAILS"].AsEnumerable()
                                                 where p.Field<string>("TICKETINGCARRIER").ToString().ToUpper().Trim() == drTSTBased["TICKETINGCARRIER"].ToString().ToUpper().Trim()
                                                 select p);

                                DataView dvResult = QryResult.AsDataView();
                                if (dvResult.Count > 0)//getting total fare each tstcount
                                {
                                    DataTable dtTicketFare = QryResult.CopyToDataTable();
                                    total_fare_value_for_radio = Convert.ToDecimal(dtTicketFare.Rows[0]["GROSSFARE"].ToString());
                                }
                                //collecting total fare for display in radio html and replacing it in a processing string builder                     
                                string oldvalue = "valuex" + drTSTBased["TICKETINGCARRIER"].ToString();
                                string newvalue = "value='" + total_fare_value_for_radio.ToString() + "'";
                                pnr_details.Replace(oldvalue, newvalue);
                                //for_hr_line++;
                                tstcount_arrayform++;
                            }
                            Session["TotalGrossAmount"] = totgrossamount;
                        }
                        pnr_details.Append("</table>");

                        pnrResult[tst_count] = tstcount.ToString();
                        pnrResult[pax_count] = str_paxcount.ToString();
                        pnrResult[PlatingCarrier] = strPlatingCarrier.TrimEnd('|');

                        ///IF A TICKETED TABLE IS EXIST THEN
                        # region
                        if (dsPnrDetails.Tables.Contains("TICKETED_DETAILS") && dsPnrDetails.Tables["TICKETED_DETAILS"].Rows.Count > 0)
                        {
                            pnr_details.Append("<table id='table3' width='100%' cellpadding='3px'>");
                            pnr_details.Append("<tr><th colspan='5'>Ticketed Segments</th></tr>");
                            pnr_details.Append("<tr class='dv_table_header' style='opacity:0.7'><th class='td_center'>Origin</th><th class='td_center'>Destination</th><th class='td_center'>Departure Date</th><th class='td_center'>Arrival Date</th><th class='td_center'>Class</th></tr>");

                            /// PAX DETAILS
                            var qryTST_booked = (from p in dsPnrDetails.Tables["TICKETED_DETAILS"].AsEnumerable()
                                                 select new
                                                 {
                                                     TICKETINGCARRIER = p.Field<string>("TICKETINGCARRIER")
                                                 }).Distinct();

                            DataTable tableTSTBased_booked = ConvertToDataTable(qryTST_booked);
                            if (tableTSTBased.Rows.Count > 0)
                                //for each tstcount wise pax details
                                foreach (DataRow drTSTBased in tableTSTBased_booked.Rows)
                                {

                                    var qryPassenger_booked = (from p in dsPnrDetails.Tables["TICKETED_DETAILS"].AsEnumerable()
                                                               where p["TICKETINGCARRIER"].ToString().Trim() == drTSTBased["TSTCOUNT"].ToString().Trim()
                                                               orderby p["PAXTYPE"] ascending
                                                               select new
                                                               {
                                                                   ORIGIN = p.Field<string>("ORIGIN").Trim(),
                                                                   DESTINATION = p.Field<string>("DESTINATION").Trim(),
                                                                   DEPARTUREDATE = p.Field<string>("DEPARTUREDATE").Trim(),
                                                                   ARRIVALDATE = p.Field<string>("ARRIVALDATE").Trim(),
                                                                   CLASS = p.Field<string>("CLASS").Trim()
                                                               }).Distinct();

                                    DataTable dtPassenger_booked = ConvertToDataTable(qryPassenger_booked);

                                    if (dsPnrDetails.Tables["PAXDETAILS"].Rows.Count < 0)
                                    {
                                        pnrResult[Error] = "Problem occured while fetching pnr details!";
                                        return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                                    }


                                    foreach (DataRow dr in dtPassenger_booked.Rows)
                                    {
                                        pnr_details.Append("<tr id='title_adult_tr'>");
                                        pnr_details.Append("<td class='td_center'>" + dr["ORIGIN"].ToString().Trim() + "</td>");
                                        pnr_details.Append("<td class='td_center'>" + dr["DESTINATION"].ToString().Trim() + "</td>");
                                        pnr_details.Append("<td class='td_center'><b>" + dr["DEPARTUREDATE"].ToString().Trim() + "</b></td>");
                                        pnr_details.Append("<td class='td_center'><b>" + dr["ARRIVALDATE"].ToString().Trim() + "</b></td>");
                                        pnr_details.Append("<td class='td_center'>" + dr["CLASS"].ToString().Trim() + "</td>");
                                        pnr_details.Append("</tr>");
                                    }
                                }
                            pnr_details.Append("</table>");
                        }
                        # endregion

                        pnrResult[Result] = pnr_details.ToString();
                    }
                    //////////////////PAX DETAILS
                    else
                    {
                        var qryTST_TWO = (from p in dsPnrDetails.Tables["PassengerPNRDetails"].AsEnumerable()
                                          select new
                                          {
                                              TSTCOUNT = p.Field<string>("TSTCOUNT")
                                          }).Distinct();

                        DataTable tableTSTBased = ConvertToDataTable(qryTST_TWO);
                        if (tableTSTBased.Rows.Count > 0)
                        {
                            //for each tstcount wise pax details
                            totgrossamount = 0;
                            int for_hr_line = 0;
                            int wit = 0, wot = 0;
                            foreach (DataRow drTSTBased in tableTSTBased.Rows)
                            {
                                int adult_selectNEW = 0;
                                int child_selectNEW = 0;
                                int infant_selectNEW = 0;

                                if (tstcount_arrayform == 0) { tstcount = drTSTBased["TSTCOUNT"].ToString(); } else { tstcount = tstcount + "," + drTSTBased["TSTCOUNT"].ToString(); }



                                var getFareBasedOnTSTCOUNT = from p in dsPnrDetails.Tables["PAXDETAILS"].AsEnumerable()
                                                             where p["TSTCOUNT"].ToString().Trim() == drTSTBased["TSTCOUNT"].ToString().Trim()
                                                             select p;



                                DataTable dt = getFareBasedOnTSTCOUNT.CopyToDataTable();

                                //getting updated basic fare total fare and tax amount for adultt, child and infant based on current loop tstcount
                                string[] UPDATED_BASIC_FARE_TAG = dt.Rows[0]["BASIC_FARE_TAG"].ToString().Split('|');

                                string[] UPDATED_TAXAMOUNT = dt.Rows[0]["TAXAMOUNT"].ToString().Split('|');

                                string[] UPDATED_TOTALFARETAG = dt.Rows[0]["TOTALFARETAG"].ToString().Split('|');

                                // for origin and destination of current loop tstcount
                                var orgDestTST = (from p in dsPnrDetails.Tables["PassengerPNRDetails"].AsEnumerable()
                                                  where p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == drTSTBased["TSTCOUNT"].ToString().ToUpper().Trim()
                                                  select new
                                                  {
                                                      ORIGIN = p.Field<string>("ORIGIN").Trim(),
                                                      DESTINATION = p.Field<string>("DESTINATION").Trim()
                                                  }).Distinct();
                                DataTable OriginTable = ConvertToDataTable(orgDestTST);

                                //if (for_hr_line != 0)
                                //    pnr_details.Append("<tr><td colspan='8'><hr style='border:0;'></td><tr>");//STS-166

                                //pnr_details.Append("<tr><td colspan='8' style='color:rgb(116,40,148);font-weight:bold;text-align: left !important;'>");
                                string perTripTSTCOUNT = string.Empty;
                                perTripTSTCOUNT = OriginTable.Rows[0]["ORIGIN"].ToString() + " → " + OriginTable.Rows[OriginTable.Rows.Count - 1]["DESTINATION"].ToString();
                                //pnr_details.Append(perTripTSTCOUNT + "</td></tr>");

                                //
                                var qryPassenger = (from p in PASSENGER_PNRINFO_TWO.AsEnumerable()
                                                    where p["TSTCOUNT"].ToString().Trim() == drTSTBased["TSTCOUNT"].ToString().Trim()
                                                    group p by p["PAXNO"]
                                                        into g
                                                    // orderby p["PAXTYPE"] ascending
                                                    select new
                                                    {
                                                        //TITLE = ((g.Field<string>("TITLE") == null || string.IsNullOrEmpty(g.Field<string>("TITLE"))) ? "" : p.Field<string>("TITLE").Trim()),
                                                        //FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                                        //LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                                        //DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                                                        //PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                                                        //BASEAMT = p.Field<string>("BASEAMT").Trim(),
                                                        //TOTALTAXAMT = p.Field<string>("TOTALTAXAMT").Trim(),
                                                        //GROSSAMT = p.Field<string>("GROSSAMT").Trim(),
                                                        //BAGGAGE = p.Field<string>("BAGGAGE").Trim(),
                                                        ////SEQNO = p.Field<string>("SEQNO").Trim(),
                                                        //PAXNO = p.Field<string>("PAXNO").Trim(),
                                                        //TSTCOUNT = p.Field<string>("TSTCOUNT").Trim(),
                                                        //TICKETNO = p.Field<string>("TICKETNO").Trim(),
                                                        //PAXREFNO = p.Field<string>("PAXREFERENCE").Trim()

                                                        TITLE = g.FirstOrDefault().Field<string>("TITLE").Trim() == null || string.IsNullOrEmpty(g.FirstOrDefault().Field<string>("TITLE").Trim()) ? "" : g.FirstOrDefault().Field<string>("TITLE").Trim(),
                                                        FIRSTNAME = g.FirstOrDefault().Field<string>("FIRSTNAME").Trim(),
                                                        LASTNAME = g.FirstOrDefault().Field<string>("LASTNAME").Trim(),
                                                        DATEOFBIRTH = (g.FirstOrDefault().Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(g.FirstOrDefault().Field<string>("DATEOFBIRTH"))) ? "" : g.FirstOrDefault().Field<string>("DATEOFBIRTH").Trim(),
                                                        PAXTYPE = g.FirstOrDefault().Field<string>("PAXTYPE").Trim(),
                                                        BASEAMT = g.FirstOrDefault().Field<string>("BASEAMT").Trim(),
                                                        TOTALTAXAMT = g.FirstOrDefault().Field<string>("TOTALTAXAMT").Trim(),
                                                        GROSSAMT = g.FirstOrDefault().Field<string>("GROSSAMT").Trim(),
                                                        BAGGAGE = g.FirstOrDefault().Field<string>("BAGGAGE").Trim(),
                                                        //SEQNO = p.Field<string>("SEQNO").Trim(),
                                                        PAXNO = g.FirstOrDefault().Field<string>("PAXNO").Trim(),
                                                        TSTCOUNT = g.FirstOrDefault().Field<string>("TSTCOUNT").Trim(),
                                                        TICKETNO = g.FirstOrDefault().Field<string>("TICKETNO").Trim(),
                                                        PAXREFNO = g.FirstOrDefault().Field<string>("PAXREFERENCE").Trim(),
                                                        DISCOUNT = g.FirstOrDefault().Field<string>("EARNINGS").Trim(),
                                                        MARKUP = g.FirstOrDefault().Field<string>("MARKUP").Trim(),
                                                        PLB = g.FirstOrDefault().Field<string>("PLB").Trim()
                                                    }).Distinct();

                                DataTable dtPassenger = ConvertToDataTable(qryPassenger);

                                if (dsPnrDetails.Tables["PAXDETAILS"].Rows.Count < 0)
                                {
                                    pnrResult[Error] = "Problem occured while fetching pnr details!";
                                    //return pnrResult;
                                    return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                                }

                                decimal total_fare_value_for_radio = 0;
                                int adt = 0;
                                int chd = 0;
                                int inf = 0;

                                string tstno = string.Empty;
                                foreach (DataRow dr in dtPassenger.Rows)
                                {
                                    if (dr["TICKETNO"].ToString().Trim() != "" && tstno != dr["TSTCOUNT"].ToString().Trim())
                                    {
                                        //pnr_details.Append("<thead><tr class='dv_table_header' style='background: #59c45a;color: #fff;text-align: center;'><th class='td_center'>Title</th><th class='td_center'>Name</th><th class='td_center'>Date of Birth</th><th class='td_center'>Gender</th><th class='td_center'>Ticket No.</th><th class='td_center'>Type</th>");
                                        if (wit == 0)
                                        {
                                            pnr_details.Append("<thead><tr class='dv_table_header' style='background: #27a5f5;color: #fff;text-align: center;'><th class='td_center'>Title</th><th class='td_center'>Name</th><th class='td_center'>Ticket No.</th><th class='td_center'>Sector</th><th class='td_center'>Type</th>");
                                            pnr_details.Append("<th class='td_right'><nobr>Base Amount<nobr></th><th class='td_right'><nobr>Tax Amount<nobr></th><th class='td_right'><nobr>Gross Amount</nobr></th><th class='td_right clsearn'>Commission</th><th class='td_right clsearn'>PLB</th><th class='td_right clsearn'>Markup</th></tr></thead>");//<th class='td_center' style='padding:5px;'>Baggage</th>
                                            wit++;
                                        }
                                    }
                                    else if (dr["TICKETNO"].ToString().Trim() == "" && tstno != dr["TSTCOUNT"].ToString().Trim())
                                    {
                                        if (wot == 0)
                                        {
                                            pnr_details.Append("<thead><tr class='dv_table_header' style='background: #27a5f5;color: #fff;text-align: center;'><th class='td_center'>Title</th><th class='td_center'>Name</th>");

                                            if (AirportTypes == "I")
                                            {
                                                pnr_details.Append("<th class='td_center'><nobr>Date of Birth</nobr></th><th class='td_center'>Gender</th>");
                                            }
                                            pnr_details.Append("<th class='td_center'>Ticket No.</th><th class='td_center'>Sector</th><th class='td_center'>Type</th>");
                                            if (AirportTypes == "I")
                                            {
                                                pnr_details.Append("<th class='td_center'><nobr>Passport No.<nobr></th>");
                                                pnr_details.Append("<th class='td_center'><nobr>Passport Iss. Country<nobr></th>");
                                                pnr_details.Append("<th class='td_center'><nobr>Passport Exp. Date<nobr></th>");
                                            }
                                            pnr_details.Append("<th class='td_right'><nobr>Base Amount<nobr></th><th class='td_right'><nobr>Tax Amount<nobr></th><th class='td_right'><nobr>Gross Amount</nobr></th><th class='td_right clsearn'>Commission</th><th class='td_right clsearn'>PLB</th><th class='td_right clsearn'>Markup</th></tr></thead>");//<th class='td_center' style='padding:5px;'>Baggage</th>
                                            wot++;
                                        }
                                    }

                                    string title = dr["TITLE"].ToString().ToUpper().Trim();
                                    if (dr["PAXTYPE"].ToString().Trim() == "ADT") ///FOR ADULT
                                    {
                                        adult_selectNEW++;
                                        TotCnt++;

                                        pnr_details.Append("<tr id='" + adt + "title_adult_tr" + dr["TSTCOUNT"].ToString() + "' style='background:white'>");
                                        if (title != "DR" && title != "MR" && title != "MS" && title != "MRS")
                                        {
                                            adult_select++;
                                            pnr_details.Append("<td data-title='Title' class='td_center clswhite'><select style='width:80px;' id='" + dr["TSTCOUNT"].ToString() + "title_adult" + adult_select + "' >");
                                            pnr_details.Append("<option value='Title' selected='selected'>Title</option>");
                                            pnr_details.Append("<option value='DR'>DR</option>");
                                            pnr_details.Append("<option value='MR'>MR</option>");
                                            pnr_details.Append("<option value='MS'>MS</option>");
                                            pnr_details.Append("<option value='MRS'>MRS</option>");
                                            pnr_details.Append("</select>" + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");
                                        }
                                        else
                                        {
                                            dr["FIRSTNAME"] = dr["FIRSTNAME"].ToString().Trim();
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Title'>" + title + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");
                                        }
                                        if (AirportTypes == "I")
                                        {
                                            if (string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                            {
                                                pnr_details.Append("<td class='clswhite' data-title='Date of birth.'> <input readonly='readonly' class='form-control ADTDatepickerint' maxlength='16' data-DOB=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "ADultDOB" + adult_selectNEW + "' placeholder='Date of Birth' type='text' value=''/>  </td>");
                                                pnr_details.Append("<td data-title='Gender' class='td_center clswhite'><select style='width:80px;' id='" + dr["TSTCOUNT"].ToString() + "Gender_adult" + adult_selectNEW + "' >");
                                                pnr_details.Append("<option value='' selected='selected'>Gender</option>");
                                                pnr_details.Append("<option value='M'>Male</option>");
                                                pnr_details.Append("<option value='F'>Female</option>");
                                                pnr_details.Append("</select>" + "</td>");
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>" + dr["TICKETNO"].ToString().Trim() + "</td>");
                                        }
                                        else
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>-</td>");
                                        }

                                        pnr_details.Append("<td class='td_right clswhite' data-title='Sector'>" + perTripTSTCOUNT + "</td>");//STS-166
                                        pnr_details.Append("<td class='td_center clswhite' data-title='Type'>" + "ADULT" + "</td>");
                                        if (string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            if (AirportTypes == "I")
                                            {
                                                pnr_details.Append("<td class='clswhite' data-title='Passport No.'> <input class='passportno clspstAlphaWithOutSpace' maxlength='16' data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "Passwort_adult" + adult_selectNEW + "' placeholder='Passport No' onkeypress='javascript:return validateAlphandNumeric(event);' type='text' value=''/>  </td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Iss. Country'> <select data-IssuedCountry=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "IssuedCountry" + adult_selectNEW + "' class='form-control PassIssueCountry'></select></td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Exp. Date'> <input readonly='readonly' class='form-control PASSPORTDatepickerint' maxlength='16' data-ExpiryDate=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "ExpiryDate" + adult_selectNEW + "' placeholder='Passport Exp. Date' type='text' value=''/>  </td>");
                                            }
                                        }

                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Base Amount'><b>" + UPDATED_BASIC_FARE_TAG[0].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Tax Amount'><b>" + UPDATED_TAXAMOUNT[0].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Gross Amount'><b>" + UPDATED_TOTALFARETAG[0].ToString() + "</b></td>");

                                        if (strfareFlag == "Y")
                                            totgrossamount += Math.Round(Convert.ToDecimal(UPDATED_TOTALFARETAG[0].ToString()));

                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='disvalue'>" + dr["DISCOUNT"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='plbvalue'>" + dr["PLB"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='markvalue'>" + dr["MARKUP"].ToString().Trim() + "</label></td>");
                                        adt++;

                                    }
                                    if (dr["PAXTYPE"].ToString().Trim() == "CHD")   //FOR CHILD
                                    {
                                        child_selectNEW++;
                                        TotCnt++;

                                        pnr_details.Append("<tr id='" + chd + "title_child_tr" + dr["TSTCOUNT"].ToString() + "' style='background:white'>");
                                        if (title != "MISS" && title != "MSTR")
                                        {
                                            child_select++;
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Title'><select style='width:80px;' id='" + dr["TSTCOUNT"].ToString() + "title_child" + child_select + "'' onchange='chd_title_change(this.id)'>");
                                            pnr_details.Append("<option value='Title' selected='selected' >Title</option>");
                                            pnr_details.Append("<option value='MISS'>MISS</option>");
                                            pnr_details.Append("<option value='MSTR'>MSTR</option>");
                                            pnr_details.Append("</select>" + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");
                                        }
                                        else
                                        {
                                            dr["FIRSTNAME"] = dr["FIRSTNAME"].ToString().Trim();
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Title'>" + title + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");
                                        }

                                        if (AirportTypes == "I" && string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            pnr_details.Append("<td class='clswhite' data-title='Date of birth.'> <input readonly='readonly' class='form-control CHDDatepickerint' maxlength='16' data-DOB=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "ChildDOB" + child_selectNEW + "' placeholder='Date of Birth' type='text' value=''/>  </td>");
                                            pnr_details.Append("<td data-title='Gender' class='td_center clswhite'><select style='width:80px;' id='" + dr["TSTCOUNT"].ToString() + "Gender_Child" + child_selectNEW + "'>");
                                            pnr_details.Append("<option value='' selected='selected'>Gender</option>");
                                            pnr_details.Append("<option value='M'>Male</option>");
                                            pnr_details.Append("<option value='F'>Female</option>");
                                            pnr_details.Append("</select>" + "</td>");
                                        }
                                        if (!string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>" + dr["TICKETNO"].ToString().Trim() + "</td>");
                                        }
                                        else
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>-</td>");
                                        }
                                        pnr_details.Append("<td class='td_right clswhite' data-title='Sector'>" + perTripTSTCOUNT + "</td>");//STS-166
                                        pnr_details.Append("<td class='td_center clswhite' data-title='Type'>" + "CHILD" + "</td>");
                                        if (string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            if (AirportTypes == "I")
                                            {
                                                pnr_details.Append("<td class='clswhite' data-title='Passport No.'> <input class='passportno clspstAlphaWithOutSpace' maxlength='16' onkeypress='javascript:return validateAlphandNumeric(event);' data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "Passwort_child" + child_selectNEW + "' placeholder='Passport No.' type='text' value=''/>  </td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Iss. Country'> <select data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "childIssuedCountry" + child_selectNEW + "' class='form-control PassIssueCountry'></select></td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Exp. Date'> <input readonly='readonly' class='form-control PASSPORTDatepickerint' maxlength='16' data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "childExpiryDate" + child_selectNEW + "' placeholder='Passport Exp. Date.' type='text' value=''/>  </td>");

                                            }
                                        }

                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Base Amount'><b>" + UPDATED_BASIC_FARE_TAG[1].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Tax Amount'><b>" + UPDATED_TAXAMOUNT[1].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite clsbold' data-title='Gross Amount'><b>" + UPDATED_TOTALFARETAG[1].ToString() + "</b></td>");
                                        if (strfareFlag == "Y")
                                            totgrossamount += Math.Round(Convert.ToDecimal(UPDATED_TOTALFARETAG[1].ToString()));

                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='disvalue'>" + dr["DISCOUNT"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='plbvalue'>" + dr["PLB"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='markvalue'>" + dr["MARKUP"].ToString().Trim() + "</label></td>");
                                        chd++;

                                    }
                                    if (dr["PAXTYPE"].ToString().Trim() == "INF")   ///FOR INFANT
                                    {
                                        infant_selectNEW++;

                                        pnr_details.Append("<tr id='" + inf + "title_infant_tr" + dr["TSTCOUNT"].ToString() + "' style='background:white'>");
                                        if (title != "MISS" && title != "MSTR")
                                        {
                                            infant_select++;
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Title'><select style='width:80px;' id='" + dr["TSTCOUNT"].ToString() + "title_infant" + infant_select + "'' onchange='inf_title_change(this.id)'>");
                                            pnr_details.Append("<option value='Title' >Title</option>");
                                            pnr_details.Append("<option value='MISS'>MISS</option>");
                                            pnr_details.Append("<option value='MSTR' selected='selected'>MSTR</option>");
                                            pnr_details.Append("</select>" + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");

                                        }
                                        else
                                        {
                                            dr["FIRSTNAME"] = dr["FIRSTNAME"].ToString().Trim();
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Title'>" + title + "</td><td class='td_center clswhite' data-title='Name'>" + dr["FIRSTNAME"].ToString().Trim() + " " + dr["LASTNAME"].ToString().Trim() + "</td>");
                                        }
                                        if (AirportTypes == "I" && string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            pnr_details.Append("<td class='clswhite' data-title='Date of birth.'> <input readonly='readonly' class='form-control INFDatepickerint' maxlength='16' data-DOB=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "InfantDOB" + infant_selectNEW + "' placeholder='Date of Birth' type='text' value=''/>  </td>");

                                            pnr_details.Append("<td data-title='Gender' class='td_center clswhite'><select style='width:80px;' id='" + dr["TSTCOUNT"].ToString() + "Gender_Infant" + infant_selectNEW + "' onchange='adt_title_change(this.id)'>");
                                            pnr_details.Append("<option value='' selected='selected'>Gender</option>");
                                            pnr_details.Append("<option value='M'>Male</option>");
                                            pnr_details.Append("<option value='F'>Female</option>");
                                            pnr_details.Append("</select>" + "</td>");
                                        }
                                        if (!string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>" + dr["TICKETNO"].ToString().Trim() + "</td>");
                                        }
                                        else
                                        {
                                            pnr_details.Append("<td class='td_center clswhite' data-title='Ticket No.'>-</td>");
                                        }
                                        pnr_details.Append("<td class='td_right clswhite' data-title='Sector'>" + perTripTSTCOUNT + "</td>");//STS-166
                                        pnr_details.Append("<td class='td_center clswhite'>" + "INFANT" + "</td>");
                                        if (string.IsNullOrEmpty(dr["TICKETNO"].ToString().Trim()))
                                        {
                                            if (AirportTypes == "I")
                                            {
                                                pnr_details.Append("<td class='clswhite' data-title='Passport No.'> <input class='passportno clspstAlphaWithOutSpace' onkeypress='javascript:return validateAlphandNumeric(event);' data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "Passwort_infant" + infant_selectNEW + "' maxlength='16' placeholder='Passport No.' type='text' value=''/>  </td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Iss. Country'> <select data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "infantIssuedCountry" + infant_selectNEW + "' class='form-control PassIssueCountry'></select></td>");
                                                pnr_details.Append("<td class='clswhite' data-title='Passport Exp. Date'> <input  maxlength='16' data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " id='" + dr["TSTCOUNT"].ToString() + "infantExpiryDate" + infant_selectNEW + "' placeholder='Passport Exp. Date' type='text' readonly='readonly' class='form-control PASSPORTDatepickerint' value=''/>  </td>");

                                            }
                                        }

                                        pnr_details.Append("<td class='td_right clswhite' data-title='Base Amount'><b>" + UPDATED_BASIC_FARE_TAG[2].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite' data-title='Tax Amount'><b>" + UPDATED_TAXAMOUNT[2].ToString() + "</b></td>");
                                        pnr_details.Append("<td class='td_right clswhite' data-title='Gross Amount'><b>" + UPDATED_TOTALFARETAG[2].ToString() + "</b></td>");
                                        if (strfareFlag == "Y")
                                            totgrossamount += Math.Round(Convert.ToDecimal(UPDATED_TOTALFARETAG[2].ToString()));

                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='disvalue'>" + dr["DISCOUNT"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='plbvalue'>" + dr["PLB"].ToString().Trim() + "</label></td>");
                                        pnr_details.Append("<td><label data-paxtype=" + dr["PAXTYPE"].ToString().Trim() + " class='markvalue'>" + dr["MARKUP"].ToString().Trim() + "</label></td>");
                                        inf++;
                                    }
                                    //pnr_details.Append("<td class='td_center clswhite' data-title='Baggage'>" + dr["BAGGAGE"].ToString().Trim() + "</td>");
                                    pnr_details.Append("</tr>");
                                    tstno = dr["TSTCOUNT"].ToString().Trim();
                                }
                                int addnew = 0;
                                int chdnew = 0;
                                for (int l = 0; l < dsPnrDetails.Tables["PAX_INFO"].Rows.Count; l++)
                                {
                                    if (dsPnrDetails.Tables["PAX_INFO"].Rows[l]["PAXTYPE"].ToString() == "ADT")
                                    {
                                        addnew++;
                                    }
                                    else
                                    {
                                        chdnew++;
                                    }
                                }
                                AdtChd = addnew + "|" + chdnew;
                                adult_select = 0;
                                child_select = 0;
                                infant_select = 0;
                                var QryResult = (from p in dsPnrDetails.Tables["PAXDETAILS"].AsEnumerable()
                                                 where p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == drTSTBased["TSTCOUNT"].ToString().ToUpper().Trim()
                                                 select p);

                                DataView dvResult = QryResult.AsDataView();
                                if (dvResult.Count > 0)//getting total fare each tstcount
                                {
                                    DataTable dtTicketFare = QryResult.CopyToDataTable();
                                    total_fare_value_for_radio = Convert.ToDecimal(dtTicketFare.Rows[0]["GROSSFARE"].ToString());
                                }
                                //collecting total fare for display in radio html and replacing it in a processing string builder                     
                                string oldvalue = "valuex" + drTSTBased["TSTCOUNT"].ToString();
                                string newvalue = "value='" + total_fare_value_for_radio.ToString() + "'";
                                pnr_details.Replace(oldvalue, newvalue);
                                //for_hr_line++;
                                tstcount_arrayform++;
                            }
                            Session["TotalGrossAmount"] = totgrossamount;
                        }
                        pnr_details.Append("</table>");

                        pnrResult[tst_count] = tstcount.ToString();
                        pnrResult[pax_count] = str_paxcount.ToString();
                        pnrResult[PlatingCarrier] = strPlatingCarrier.TrimEnd('|');

                        ///IF A TICKETED TABLE IS EXIST THEN
                        # region
                        if (dsPnrDetails.Tables.Contains("TICKETED_DETAILS") && dsPnrDetails.Tables["TICKETED_DETAILS"].Rows.Count > 0)
                        {
                            pnr_details.Append("<table id='table3' width='100%' cellpadding='3px'>");
                            pnr_details.Append("<tr><th colspan='5'>Ticketed Segments</th></tr>");
                            pnr_details.Append("<tr class='dv_table_header' style='opacity:0.7'><th class='td_center'>Origin</th><th class='td_center'>Destination</th><th class='td_center'>Departure Date</th><th class='td_center'>Arrival Date</th><th class='td_center'>Class</th></tr>");

                            /// PAX DETAILS
                            var qryTST_booked = (from p in dsPnrDetails.Tables["TICKETED_DETAILS"].AsEnumerable()
                                                 select new
                                                 {
                                                     TSTCOUNT = p.Field<string>("TSTCOUNT")
                                                 }).Distinct();

                            DataTable tableTSTBased_booked = ConvertToDataTable(qryTST_booked);
                            if (tableTSTBased.Rows.Count > 0)
                                //for each tstcount wise pax details
                                foreach (DataRow drTSTBased in tableTSTBased_booked.Rows)
                                {

                                    var qryPassenger_booked = (from p in dsPnrDetails.Tables["TICKETED_DETAILS"].AsEnumerable()
                                                               where p["TSTCOUNT"].ToString().Trim() == drTSTBased["TSTCOUNT"].ToString().Trim()
                                                               orderby p["PAXTYPE"] ascending
                                                               select new
                                                               {
                                                                   ORIGIN = p.Field<string>("ORIGIN").Trim(),
                                                                   DESTINATION = p.Field<string>("DESTINATION").Trim(),
                                                                   DEPARTUREDATE = p.Field<string>("DEPARTUREDATE").Trim(),
                                                                   ARRIVALDATE = p.Field<string>("ARRIVALDATE").Trim(),
                                                                   CLASS = p.Field<string>("CLASS").Trim()
                                                               }).Distinct();

                                    DataTable dtPassenger_booked = ConvertToDataTable(qryPassenger_booked);

                                    if (dsPnrDetails.Tables["PAXDETAILS"].Rows.Count < 0)
                                    {
                                        pnrResult[Error] = "Problem occured while fetching pnr details!";
                                        return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
                                    }

                                    foreach (DataRow dr in dtPassenger_booked.Rows)
                                    {
                                        pnr_details.Append("<tr id='title_adult_tr'>");
                                        pnr_details.Append("<td class='td_center'>" + dr["ORIGIN"].ToString().Trim() + "</td>");
                                        pnr_details.Append("<td class='td_center'>" + dr["DESTINATION"].ToString().Trim() + "</td>");
                                        pnr_details.Append("<td class='td_center'><b>" + dr["DEPARTUREDATE"].ToString().Trim() + "</b></td>");
                                        pnr_details.Append("<td class='td_center'><b>" + dr["ARRIVALDATE"].ToString().Trim() + "</b></td>");
                                        pnr_details.Append("<td class='td_center'>" + dr["CLASS"].ToString().Trim() + "</td>");
                                        pnr_details.Append("</tr>");
                                    }
                                }
                            pnr_details.Append("</table>");
                        }
                        # endregion

                        pnrResult[Result] = pnr_details.ToString();
                    }
                }
                else
                {
                    if (dsPnrDetails != null && dsPnrDetails.Tables.Count > 0)
                        dsPnrDetails.WriteXml(strwrt);
                    strErrorMsg = "Unable to retrive PNR details. Please check PNR and try later.";

                    if (dsPnrDetails != null && dsPnrDetails.Tables.Count > 0 &&
                     dsPnrDetails.Tables.Contains("Result") && dsPnrDetails.Tables["Result"].Rows.Count > 0 &&
                      !string.IsNullOrEmpty(dsPnrDetails.Tables["Result"].Rows[0]["ErrorDescription"].ToString()))
                    {
                        strErrorMsg = dsPnrDetails.Tables["Result"].Rows[0]["ErrorDescription"].ToString();
                    }
                    else if (dsPnrDetails != null && dsPnrDetails.Tables.Count > 0 &&
                     dsPnrDetails.Tables.Contains("Result") && dsPnrDetails.Tables["Result"].Rows.Count > 0 &&
                      !string.IsNullOrEmpty(dsPnrDetails.Tables["Result"].Rows[0]["ErrorDisplay"].ToString()))
                    {
                        strErrorMsg = dsPnrDetails.Tables["Result"].Rows[0]["ErrorDisplay"].ToString();
                    }
                    xmlData = string.Empty;
                    xmlData = "<EVENT>" + "<RESPONSE>FETCH_QUEUEPNR_DETAILS</RESPONSE>" +
                                        "<STATUS>FAILED</STATUS>" +
                                         "<PNRINFO>" + strwrt.ToString() + "</PNRINFO>" +
                                        "<ERRORMSG>" + strErrorMsg + "</ERRORMSG>" +
                                        "</EVENT>";

                    DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "Fetch_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, sequnceID);
                    pnrResult[Error] = strErrorMsg.ToString();
                    return Json(new { Status = "0", Message = "", Result = pnrResult });
                }

                string Ticketingcarrier = dsPnrDetails.Tables["PassengerPNRDetails"].Rows[0]["TICKETINGCARRIER"].ToString();
                DataSet dataSet = _RaysService.Fetch_Client_Credit_Balance_DetailsWEB(Corporatename, Session["username"].ToString(), Session["ipAddress"].ToString(), Session["sequenceid"].ToString(), strModule);

                if (dataSet != null && dataSet.Tables.Count != 0 && dataSet.Tables[0].Rows.Count != 0)
                {
                    string Allowbokingtype = dataSet.Tables[0].Rows[0]["CLT_ALLOW_ACC_TYPE"].ToString().Trim();

                    if (Allowbokingtype.Contains("C"))
                    {
                        Paymentdetails += "C";
                    }
                    if (Allowbokingtype.Contains("B"))
                    {
                        Paymentdetails += "~B";
                    }
                    if (Allowbokingtype.Contains("H") && strTerminalType == "T")
                    {
                        Paymentdetails += "~H";
                    }
                    if (Allowbokingtype.Contains("T") && strTerminalType == "T")
                    {
                        Paymentdetails += "~T";
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.ToString();

                DatabaseLog.Retrievetcktlog(strUserName, "X", "RetrieveBookingController.cs", "Fetch_QueuePNR_Details", ErrorMsg, strClientID, strPosTID, sequnceID);
                if (ex.Message.ToString().ToUpper().Trim().Contains("TIMED OUT") || ex.Message.ToString().ToUpper().Trim().Contains("SERVICE UNAVAILABLE.")
                     || ex.Message.ToString().ToUpper().Trim().Contains("SERVER"))
                    pnrResult[Error] = ex.Message.ToString();
                else
                    pnrResult[Error] = "Unable to retrieve pnr details!";
                return Json(new { Status = "0", Message = pnrResult[Error], Result = pnrResult });
            }
            return Json(new { Status = "1", Message = "", Result = pnrResult, Corporatecode = strCorporatecode, TOTALGrossFare = totgrossamount, AirlineCET = AirlineCET.TrimEnd('|'), TotalCount = TotCnt, adtchild = AdtChd, Paymentdetails = Paymentdetails });
        }

        public ActionResult RetrivePNRDetails(string CrsPnr, string CRS, string QueueingNumber,
            string AirportTypes, string Corporatename, string Employeename, string EmpCostCenter, string EmpRefID, string BranchID,
            string Faretype, string AirlineCategory, string AirlinePNR, string AirlineName, string OfficeID)
        {
            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;
            string strBranchId = string.Empty;
            string strPosID = string.Empty;
            string strPosTID = string.Empty;
            string strClientID = Corporatename;
            string strTerminalType = string.Empty;

            string lstrSupplierID = string.Empty;
            string lstrTCKPCC = string.Empty;
            string lstrBookinSupp = string.Empty;
            string lstrBookPCC = string.Empty;
            string strFareType = string.Empty;
            string cmbFareType = string.Empty;

            string Errormsg = string.Empty;
            DataSet pdsPnrResp = new DataSet();

            string stu = string.Empty;
            string msg = string.Empty;
            string result = string.Empty;

            string strJson = string.Empty;
            string strSessionKey = string.Empty;

            DataSet dsRetrivePNR = new DataSet();

            string Paymentdetails = string.Empty;

            try
            {
                #region UsageLog
                try
                {
                    string strUsgLogRes = Base.Commonlog("RetrivePNRDetails", "", "FETCH");
                    if (strBranchCredit != "")
                    {
                        if (strBranchCredit == "ALL" || (BranchID != "" && strBranchCredit.Contains(BranchID)))
                        {
                            _RaysService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                            _InplantService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_INPLANT_SERVICE"].ToString();
                        }
                        else
                        {
                            _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                            _InplantService.Url = ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString();
                        }
                    }
                    else
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                        _InplantService.Url = ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString();
                    }
                }
                catch (Exception e) { }
                #endregion

                strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
                strUserName = Session["username"] != null ? Session["username"].ToString() : "";
                Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
                strBranchId = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();

                AgentDetails _AgentDetail = new AgentDetails();

                _AgentDetail.AgentID = strAgentID;
                _AgentDetail.TerminalID = strTerminalId;
                _AgentDetail.ClientID = Corporatename;
                _AgentDetail.BOAID = strPosID;
                _AgentDetail.BOATreminalID = strPosTID;
                _AgentDetail.CoOrdinatorID = "";
                _AgentDetail.BranchID = BranchID;
                _AgentDetail.Airportid = (!string.IsNullOrEmpty(AirportTypes) ? AirportTypes : "");
                _AgentDetail.Platform = "B";// "B",
                _AgentDetail.ProjectId = ConfigurationManager.AppSettings["ProjectCode"].ToString();
                _AgentDetail.APPCurrency = "";
                _AgentDetail.UserName = strUserName;
                _AgentDetail.TicketingOfficeId = OfficeID;
                _AgentDetail.PNROfficeId = "";


                _AgentDetail.AppType = "";
                _AgentDetail.Version = "";
                _AgentDetail.AgentType = "";
                _AgentDetail.Environment = (strTerminalType == "T" ? "I" : "W");
                _AgentDetail.ProductID = "";
                _AgentDetail.APIUSE = new string[] { };

                RetrivePNRBOA_RQ Pnrreq = new RetrivePNRBOA_RQ()
                {
                    Platform = "B",//"B",
                    CRSID = CRS,
                    CRSPNR = CrsPnr,
                    Ticketing = false,
                    AirlineCategory = AirlineCategory,
                    AirlinePNR = AirlinePNR,
                    TrackID = "",
                    FareType = Faretype,
                    AgentDetail = _AgentDetail,
                    TicketPCC = OfficeID
                };

                MyWebClient client = new MyWebClient();
                string request = JsonConvert.SerializeObject(Pnrreq).ToString();

                string Query = "RetrivePNR";
                string strURLpath = ConfigurationManager.AppSettings["RetriveUrl"].ToString();//RetriveUrl
                string url = strURLpath + "/" + Query;
                client.Headers["Content-type"] = "application/json";

                string strTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string xmlData = "<REQUEST>FETCH_QUEUEPNR_DETAILS</REQUEST>" +
                               "<CRSPNR>" + CrsPnr + "</CRSPNR>" +
                                "<CRSID>" + CRS + "</CRSID>" +
                               "<BRANCHID>" + strBranchId + "</BRANCHID>" +
                               "<REQTIME>" + strTime + "</REQTIME>" +
                               "<QUEUENUMBER>" + QueueingNumber + "</QUEUENUMBER>" +
                               "<REQUEST_JSON>" + request + "</REQUEST_JSON>";

                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Fetch_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");


                byte[] data = client.UploadData(url, "POST", Encoding.ASCII.GetBytes(request));
                string strresponse = Encoding.ASCII.GetString(data);



                string strTimeREs = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                xmlData = "<EVENT>" + "<RESPONSE>FETCH_QUEUEPNR_DETAILS</RESPONSE>" +
                               "<CRSPNR>" + CrsPnr + "</CRSPNR>" +
                                "<CRSID>" + CRS + "</CRSID>" +
                               "<BRANCHID>" + strBranchId + "</BRANCHID>" +
                               "<RESTIME>" + strTimeREs + "</RESTIME>" +
                               "<QUEUENUMBER>" + QueueingNumber + "</QUEUENUMBER>" +
                               "<RESPONSEJSON>" + strresponse + "</RESPONSEJSON>" +
                               "</EVENT>";

                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Fetch_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");


                pdsPnrResp = _ServiceUtility.convertJsonStringToDataSet(strresponse, "");

                if (pdsPnrResp.Tables["STU"].Rows[0]["RSC"].ToString() == "1")
                {

                    dsRetrivePNR = new DataSet();
                    dsRetrivePNR.ReadXml(new System.IO.StringReader(pdsPnrResp.Tables[0].Rows[0]["PDT"].ToString()));
                    dsRetrivePNR.AcceptChanges();

                    strSessionKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    Session.Add("RetriveDet_" + strSessionKey, dsRetrivePNR);

                    string Sessionkey = "RetriveDet_" + strSessionKey;

                    Session.Add("Sessionkey", Sessionkey);
                    strJson = JsonConvert.SerializeObject(dsRetrivePNR);

                    stu = "1";


                    DataSet dataSet = _RaysService.Fetch_Client_Credit_Balance_DetailsWEB(Corporatename, Session["username"].ToString(), Session["ipAddress"].ToString(), Session["sequenceid"].ToString(), strModule);

                    if (dataSet != null && dataSet.Tables.Count != 0 && dataSet.Tables[0].Rows.Count != 0)
                    {
                        string Allowbokingtype = dataSet.Tables[0].Rows[0]["CLT_ALLOW_ACC_TYPE"].ToString().Trim();
                        //if (Allowbokingtype.Contains("C"))
                        //{
                        //    Paymentdetails += "C";
                        //}
                        if (Allowbokingtype.Contains("T") && strTerminalType == "T")
                        {
                            Paymentdetails += "T";
                        }
                        if (Allowbokingtype.Contains("H") && strTerminalType == "T")
                        {
                            Paymentdetails += "~H";
                        }
                    }

                }
                else
                {

                    strSessionKey = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    Session.Add("RetriveDet_" + strSessionKey, "0");

                    string Sessionkey = "RetriveDet_" + strSessionKey;

                    Session.Add("Sessionkey", Sessionkey);
                    Session.Add("ERRORMSG", pdsPnrResp.Tables["STU"].Rows[0]["ERR"].ToString());

                    stu = "0";
                    msg = "Unable to retrieve pnr details";
                }

            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString() + ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line")).ToString();
                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "X", "RetrieveBookingController.cs", "Fetch_QueuePNR_Details", ErrorMsg, strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                if (ex.Message.ToString().ToUpper().Trim().Contains("TIMED OUT") || ex.Message.ToString().ToUpper().Trim().Contains("SERVICE UNAVAILABLE.")
                     || ex.Message.ToString().ToUpper().Trim().Contains("SERVER"))
                    Errormsg = ex.Message.ToString();
                else
                    msg = "Unable to retrieve pnr details!";
                return Json(new { Status = "0", Message = msg, Result = "" });
            }

            return Json(new { Status = stu, Message = msg, Result = strJson, SessionKey = "RetriveDet_" + strSessionKey, Paymentdetails = Paymentdetails });
        }

        public ActionResult Fetch_QueueFARE_Details(string hdf_pnrinfo, string CorporateCode, string CrsPnr, string CRS, string QueueingNumber, string TSTCOUNT,
            string AirportTypes, string Corporatename, string Employeename, string Empmailname, string EmpCostCenter, string EmpRefID, string BranchID,
            string SessionKey, string GSTDetails, string Faretype, string DiscountVal, string Markupval, string TotalGrossamt, string Newempflag, string faretypereason,
            string PaymentMode, string CommonValue, string OtherTicketInfo, string StrFareFlag, string Ticketing_PCC, string strPlatingcarrier)
        {
            ArrayList result = new ArrayList();
            DataSet dsBooked = new DataSet();
            int error = 0;
            int res_sesid = 1;
            int res_normalFare = 2;
            int res_specialFare = 3;
            int res_FareDiff = 4;
            int res_json = 5;
            int res_adultcount = 6;
            int res_childcount = 7;
            int res_infantcount = 8;
            int res_token = 9;
            int res_PNRdetails = 10;
            int Flightdetails = 11;
            int res_officeid = 12;
            int Res_PNROWNOFFICEID = 13;
            string ApprovalDetails = string.Empty;
            string fareData = string.Empty;
            result.Add("");//0
            result.Add("");//1
            result.Add("");//2
            result.Add("");//3
            result.Add("");//4
            result.Add("");//5
            result.Add("");//6
            result.Add("");//7
            result.Add("");//8
            result.Add("");//9
            result.Add("");//10
            result.Add("");//11
            result.Add("");//for office id usage incentive disable 12
            result.Add("");// For PNR Own Office ID 13
            string strErrorMsg = string.Empty;
            string str_pnrinfo = string.Empty;
            string IPAddress = string.Empty;
            string FarePricingPCC = string.Empty;

            StringWriter strwrt = new StringWriter();

            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;
            string strBranchId = string.Empty;
            string strTopUpBranchID = string.Empty;
            string strOfficeId = string.Empty;
            string strCorporatecode = string.Empty;
            string TicketingOfficeId = string.Empty;
            string strAUTOQUEUEFLAG = "N";
            string strPlattingCarrier = string.Empty;
            string xmlData = string.Empty;
            string strClientID = Corporatename;
            string strPosID = string.Empty;
            string strPosTID = string.Empty;
            string Paymentdetails = string.Empty;

            DataSet dsRetrieveFARE = new DataSet();
            DataSet dsRQT_AGN_PCC = new DataSet();
            string strResult = string.Empty;
            string strPricingToken = string.Empty;

            GSTDETAILS _gstdetails = new GSTDETAILS();

            string PaxReferenceTST = string.Empty;
            string PaxReferenceTSTADULT = string.Empty;
            string PaxReferenceTSTCHILD = string.Empty;
            string PaxReferenceTSTINFANT = string.Empty;

            string strPNROWNOfficeID = string.Empty;
            string strAgnBranchpcc = string.Empty;

            try
            {
                strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
                strUserName = Session["username"] != null ? Session["username"].ToString() : "";
                Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0";
                strBranchId = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                strResult = (Session["RETRIVEPNR" + CrsPnr.ToUpper().Trim()] != null && Session["RETRIVEPNR" + CrsPnr.ToUpper().Trim()] != "") ? Session["RETRIVEPNR" + CrsPnr.ToUpper().Trim()].ToString() : "";

                string AgentType = string.Empty;

                if (strAgentID == "" || strPosID == "" || strTerminalType == "" || strUserName == "" || strResult == "")
                {
                    result[error] = "Your session has expired!";
                    DatabaseLog.Retrievetcktlog("Session Timeout", "E", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", DateTime.Now.ToString("yyyyMMddHHmmssfff"), strClientID, strPosTID, DateTime.Now.ToString("yyyyMMdd"));
                    return Json(new { Status = "-1", Message = result[error], Result = result });
                }

                object objPNR = Session[hdf_pnrinfo.TrimEnd('#')];
                if (objPNR == null)
                {
                    result[error] = "Your session has expired!";
                    return Json(new { Status = "-1", Message = result[error], Result = result });
                }

                #region UsageLog
                try
                {
                    string strUsgLogRes = Base.Commonlog("Check Fare", "", "FETCH");
                }
                catch (Exception e) { }
                #endregion

                #region SERVICE URL BRANCH BASED -- STS115
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (BranchID != "" && strBranchCredit.Contains(BranchID)))
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                        _InplantService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_INPLANT_SERVICE"].ToString();
                    }
                    else
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                        _InplantService.Url = ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString();
                    }
                }
                else
                {
                    _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                    _InplantService.Url = ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString();
                }
                #endregion

                DataSet dsPNRINFO = ((DataSet)Session[hdf_pnrinfo]).Copy();

                var orgDestTST = (from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                  where (StrFareFlag == "Y" ? p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == TSTCOUNT.Trim() : p["TICKETINGCARRIER"].ToString().Trim() == TSTCOUNT.Trim())
                                  select new
                                  {
                                      ORIGIN = p.Field<string>("ORIGIN"),
                                      DESTINATION = p.Field<string>("DESTINATION"),
                                      //
                                      AIRLINECODE = p.Field<string>("AIRLINECODE"),
                                      FLIGHTNO = p.Field<string>("FLIGHTNO"),
                                      DEPARTUREDATETIME = p.Field<string>("DEPARTUREDATE").Trim() + " " + p.Field<string>("DEPARTURETIME").Trim(),
                                      ARRIVALDATETIME = p.Field<string>("ARRIVALDATE").Trim() + " " + p.Field<string>("ARRIVALTIME").Trim(),
                                      CLASS = p.Field<string>("CLASS")
                                      //
                                  }).Distinct();

                DataTable OriginTable = ConvertToDataTable(orgDestTST);
                //
                result[Flightdetails] = JsonConvert.SerializeObject(OriginTable).ToString();
                //

                string perTripTSTCOUNT = OriginTable.Rows[0]["ORIGIN"].ToString() + " → " + OriginTable.Rows[OriginTable.Rows.Count - 1]["DESTINATION"].ToString();

                //Earntest
                var qryPassenger = (from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                    where (StrFareFlag == "Y" ? p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == TSTCOUNT.Trim() : p["TICKETINGCARRIER"].ToString().Trim() == TSTCOUNT.Trim())
                                    group p by p["PAXNO"]
                                        into g
                                    select new
                                    {
                                        TSTCOUNT = g.FirstOrDefault().Field<string>("TSTCOUNT").Trim(),
                                        TICKETINGCARRIER = g.FirstOrDefault().Field<string>("TICKETINGCARRIER").Trim(),
                                        PAXNO = g.FirstOrDefault().Field<string>("PAXNO").Trim(),
                                        TITLE = g.FirstOrDefault().Field<string>("TITLE").Trim() == null || string.IsNullOrEmpty(g.FirstOrDefault().Field<string>("TITLE").Trim()) ? "" : g.FirstOrDefault().Field<string>("TITLE").Trim(),
                                        FIRSTNAME = g.FirstOrDefault().Field<string>("FIRSTNAME").Trim(),
                                        LASTNAME = g.FirstOrDefault().Field<string>("LASTNAME").Trim(),
                                        PAXTYPE = g.FirstOrDefault().Field<string>("PAXTYPE").Trim(),
                                        SECTOR = perTripTSTCOUNT,
                                        BASEAMT = g.FirstOrDefault().Field<string>("BASEAMT").Trim(),
                                        TOTALTAXAMT = g.FirstOrDefault().Field<string>("TOTALTAXAMT").Trim(),
                                        GROSSAMT = g.FirstOrDefault().Field<string>("GROSSAMT").Trim(),
                                        EARNINGS_REF_ID = g.FirstOrDefault().Field<string>("EARNINGS_REF_ID").Trim(),
                                        COMMISSION = g.FirstOrDefault().Field<string>("EARNINGS").Trim(),
                                        PLB = g.FirstOrDefault().Field<string>("PLB").Trim(),
                                        MARKUP = g.FirstOrDefault().Field<string>("MARKUP").Trim(),
                                        SERVICE_FEE = g.FirstOrDefault().Field<string>("SERVICE_FEE").Trim(),
                                        INCENTIVES = g.FirstOrDefault().Field<string>("INCENTIVE").Trim(),
                                        TAXBREAKUP = g.FirstOrDefault().Field<string>("TAXBREAKUP").Trim(),
                                        PAYMENTINFO = g.FirstOrDefault().Field<string>("PAYMENTINFO").Trim()
                                    }).Distinct();

                DataTable dtSelectedTST = ConvertToDataTable(qryPassenger);
                result[res_PNRdetails] = JsonConvert.SerializeObject(dtSelectedTST).ToString();
                //

                if (strTerminalType.ToUpper().Trim() == "W" && ConfigurationManager.AppSettings["ENVIRONMENT"].ToString() == "1")
                {
                    int TempOfficeID = 0;
                    string strQueue_AGN_OfficeID = dsPNRINFO.Tables["PassengerPNRDetails"].Rows[0]["QUEUINGOFFICEID"].ToString() != "" ? dsPNRINFO.Tables["PassengerPNRDetails"].Rows[0]["QUEUINGOFFICEID"].ToString() : dsPNRINFO.Tables["PassengerPNRDetails"].Rows[0]["OFFICEID"].ToString();


                    xmlData = "<EVENT>" + "<REQUEST>Fetch_QueueFARE_Details-CHECK_AGENCYPCC_QTKT</REQUEST>" +
                                        "<QUEUEINGPCC>" + strQueue_AGN_OfficeID + "</QUEUEINGPCC>" +
                                      "<TICKETINGCC>" + Ticketing_PCC + "</TICKETINGCC>" +
                                        "<AGENTID>" + Corporatename + "</AGENTID>" +
                                        "<USERNAME>" + strUserName + "</USERNAME>" +
                                        "</EVENT>";
                    DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", xmlData.ToString(), strClientID, strPosTID, sequnceID);



                    string strBlockMsg = string.Empty;

                    byte[] byteresponse = _InplantService.FECTH_OWN_SUPP_DETAILS_RQT_CLIENT_PCC(CRS, BranchID, Corporatename, strUserName, Ipaddress, sequnceID, strTerminalId, strTerminalType, strResult, ref strBlockMsg);
                    dsRQT_AGN_PCC = Base.Decompress(byteresponse);

                    xmlData = "<EVENT><RESPONSE>Fetch_QueueFARE_Details-CHECK_AGENCYPCC_QTKT</RESPONSE><QUEUEINGPCC>" + strQueue_AGN_OfficeID + "</QUEUEINGPCC><TICKETINGCC>" + Ticketing_PCC + "</TICKETINGCC><DATA>" + dsRQT_AGN_PCC.GetXml() + "</DATA></EVENT>";
                    DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", xmlData.ToString(), strClientID, strPosTID, sequnceID);

                    if (dsRQT_AGN_PCC != null && dsRQT_AGN_PCC.Tables.Count > 0 && dsRQT_AGN_PCC.Tables[0].Rows.Count > 0)
                    {
                        if (!strQueue_AGN_OfficeID.Contains('*'))
                        {

                            foreach (DataRow dr in dsRQT_AGN_PCC.Tables[0].Rows)
                            {
                                if (strQueue_AGN_OfficeID.ToUpper().Trim().Split(',').ToArray().Contains(dr["BOOKING_PCC"].ToString().ToUpper().Trim()))
                                {
                                    TempOfficeID = 1;
                                    break;
                                }
                            }
                            //if (strQueue_AGN_OfficeID.ToUpper().Trim().Split(',').ToArray().Contains(Ticketing_PCC.ToUpper().Trim()) && TempOfficeID == 1)
                            //{
                            //    TempOfficeID = 2;
                            //}

                            if (TempOfficeID != 1)
                            {
                                result[error] = TempOfficeID == 0 ? "Kindly queue the pnr into branch PCC and proceed the ticketing." : "Please select your queuing branch PCC(OfficeID).";
                                return Json(new { Status = "0", Message = result[error], Result = result });
                            }
                        }
                    }
                    else
                    {
                        result[error] = "OfficeID is not assigned for this user. Please assign officeID.";
                        return Json(new { Status = "0", Message = result[error], Result = result });
                    }
                }
                //***************FOR TST COUNT*********************************************
                DataTable paxdetails_new = new DataTable();
                DataTable pax_info_new = new DataTable();
                DataTable passenger_pnrinfo_new = new DataTable();


                if (StrFareFlag == "N")
                {
                    var linq_paxdetails_new = from p in dsPNRINFO.Tables["PAXDETAILS"].AsEnumerable()
                                              where TSTCOUNT.Contains(p["TICKETINGCARRIER"].ToString())
                                              select p;

                    paxdetails_new = linq_paxdetails_new.CopyToDataTable<DataRow>();
                    paxdetails_new.TableName = "PAXDETAILS";

                    var linq_pax_info_new = from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                                            where TSTCOUNT.Contains(p["PLATINGCARRIER"].ToString())
                                            select p;

                    pax_info_new = linq_pax_info_new.CopyToDataTable();
                    pax_info_new.TableName = "PAX_INFO";
                    var linq_passenger_pnrinfo = from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                                 where TSTCOUNT.Contains(p["TICKETINGCARRIER"].ToString())
                                                 select p;

                    passenger_pnrinfo_new = linq_passenger_pnrinfo.CopyToDataTable();
                    passenger_pnrinfo_new.TableName = "PassengerPNRDetails";
                }
                else
                {
                    var linq_paxdetails_new = from p in dsPNRINFO.Tables["PAXDETAILS"].AsEnumerable()
                                              where TSTCOUNT.Contains(p["TSTCOUNT"].ToString())
                                              select p;

                    paxdetails_new = linq_paxdetails_new.CopyToDataTable<DataRow>();
                    paxdetails_new.TableName = "PAXDETAILS";

                    var linq_pax_info_new = from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                                            where TSTCOUNT.Contains(p["TSTCOUNT"].ToString())
                                            select p;

                    pax_info_new = linq_pax_info_new.CopyToDataTable();
                    pax_info_new.TableName = "PAX_INFO";
                    var linq_passenger_pnrinfo = from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                                 where TSTCOUNT.Contains(p["TSTCOUNT"].ToString())
                                                 select p;

                    passenger_pnrinfo_new = linq_passenger_pnrinfo.CopyToDataTable();
                    passenger_pnrinfo_new.TableName = "PassengerPNRDetails";
                }

                dsPNRINFO.Tables.Remove("PAXDETAILS");
                dsPNRINFO.Tables.Remove("PAX_INFO");
                dsPNRINFO.Tables.Remove("PassengerPNRDetails");

                dsPNRINFO.Tables.Add(paxdetails_new);
                dsPNRINFO.Tables.Add(pax_info_new);
                dsPNRINFO.Tables.Add(passenger_pnrinfo_new);
                //***************END FOR TST COUNT******************************************


                var qryAdtCount = (from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                                   where p.Field<string>("PAXTYPE") == "ADT"
                                   orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                                   select new
                                   {
                                       FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                       LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                       DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                                       PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                                       PAXNO = p.Field<string>("PAXNO").Trim(),

                                   }).Distinct();

                var qryChdCount = (from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                                   where p.Field<string>("PAXTYPE") == "CHD"
                                   orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                                   select new
                                   {
                                       FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                       LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                       DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                                       PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                                       PAXNO = p.Field<string>("PAXNO").Trim()
                                   }).Distinct();

                var qryInfCount = (from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                                   where p.Field<string>("PAXTYPE") == "INF"
                                   orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                                   select new
                                   {
                                       FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                       LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                       DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                                       PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                                       PAXNO = p.Field<string>("PAXNO").Trim()
                                   }).Distinct();

                int adultCount = int.Parse(qryAdtCount.Count().ToString());
                int childCount = int.Parse(qryChdCount.Count().ToString());
                int infantCount = int.Parse(qryInfCount.Count().ToString());

                result[res_adultcount] = adultCount.ToString();
                result[res_childcount] = childCount.ToString();
                result[res_infantcount] = infantCount.ToString();

                //DataTable dtPaxRefernce = new DataTable();
                //var qryAdtCountpX = (from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                //                   where p.Field<string>("TSTCOUNT") == TSTCOUNT
                //                   orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                //                   select new
                //                   {
                //                       FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                //                       LASTNAME = p.Field<string>("LASTNAME").Trim(),
                //                       DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                //                       PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                //                       PAXNO = p.Field<string>("PAXNO").Trim(),
                //                       PAXREFERENCE = p.Field<string>("PAXREFERENCE").Trim(),
                //                   }).Distinct();

                //if (qryAdtCountpX.Count() > 1)
                //{
                //    dtPaxRefernce = ConvertToDataTable(qryAdtCountpX);
                //}

                DataTable dtTSTSegment = new DataTable();
                if (StrFareFlag == "N")
                {
                    var QrySegment = (from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                      where p.Field<string>("TICKETINGCARRIER").Trim() == TSTCOUNT
                                      select new
                                      {
                                          ORIGIN = p.Field<string>("ORIGIN").Trim(),
                                          DESTINATION = p.Field<string>("DESTINATION").Trim(),
                                          TSTREFERENCE = p.Field<string>("TSTREFERENCE").Trim(),
                                          SEGMENTREFERENCE = p.Field<string>("SEGMENTREFERENCE").Trim(),
                                          PAXREFERENCE = p.Field<string>("PAXREFERENCE").Trim(),
                                          //PLATINGCARRIER = p.Field<string>("PLATINGCARRIER").Trim(),
                                          PLATINGCARRIER = p.Field<string>("TICKETINGCARRIER").Trim(),
                                          FAREBASISCODE = p.Field<string>("FAREBASISCODE").Trim(),
                                          TSTCOUNT = p.Field<string>("TSTCOUNT").Trim(),
                                          GROSSAMT = p.Field<string>("GROSSAMT").Trim(),
                                          SEGMENTNO = p.Field<string>("SEGMENTNO").Trim(),
                                          TITLE = p.Field<string>("TITLE").Trim(),
                                          FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                          LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                          DEPARTUREDATE = p.Field<string>("DEPARTUREDATE").Trim(),
                                          ARRIVALDATE = p.Field<string>("ARRIVALDATE").Trim(),
                                          DEPARTURETIME = p.Field<string>("DEPARTURETIME").Trim(),
                                          ARRIVALTIME = p.Field<string>("ARRIVALTIME").Trim(),
                                          CLASS = p.Field<string>("CLASS").Trim(),
                                      }).Distinct();
                    dtTSTSegment = ConvertToDataTable(QrySegment);
                }
                else
                {

                    var QrySegment = (from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                      select new
                                      {
                                          ORIGIN = p.Field<string>("ORIGIN").Trim(),
                                          DESTINATION = p.Field<string>("DESTINATION").Trim(),
                                          TSTREFERENCE = p.Field<string>("TSTREFERENCE").Trim(),
                                          SEGMENTREFERENCE = p.Field<string>("SEGMENTREFERENCE").Trim(),
                                          PAXREFERENCE = p.Field<string>("PAXREFERENCE").Trim(),
                                          //PLATINGCARRIER = p.Field<string>("PLATINGCARRIER").Trim(),
                                          PLATINGCARRIER = p.Field<string>("TICKETINGCARRIER").Trim(),
                                          FAREBASISCODE = p.Field<string>("FAREBASISCODE").Trim(),
                                          TSTCOUNT = p.Field<string>("TSTCOUNT").Trim(),
                                          GROSSAMT = p.Field<string>("GROSSAMT").Trim(),
                                          SEGMENTNO = p.Field<string>("SEGMENTNO").Trim(),
                                          TITLE = p.Field<string>("TITLE").Trim(),
                                          FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                          LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                          DEPARTUREDATE = p.Field<string>("DEPARTUREDATE").Trim(),
                                          ARRIVALDATE = p.Field<string>("ARRIVALDATE").Trim(),
                                          DEPARTURETIME = p.Field<string>("DEPARTURETIME").Trim(),
                                          ARRIVALTIME = p.Field<string>("ARRIVALTIME").Trim(),
                                          CLASS = p.Field<string>("CLASS").Trim(),
                                      }).Distinct();

                    dtTSTSegment = ConvertToDataTable(QrySegment);
                }

                string segmenType = string.Empty;
                segmenType = dsPNRINFO.Tables["AGENT_INFO"].Rows[0]["AIRPORT_TYPE"].ToString().ToUpper().Trim();

                //***********************calculating Booking Amount***************************
                decimal dcTotalFare = 0;

                foreach (DataRow drRows in dsPNRINFO.Tables["PAX_INFO"].Rows)
                {
                    if (drRows["PAXTYPE"].ToString().ToUpper().Trim() == "ADT")
                    {
                        PaxReferenceTSTADULT += drRows["PAXREFERENCE"].ToString() + ",";
                    }
                    if (drRows["PAXTYPE"].ToString().ToUpper().Trim() == "CHD")
                    {
                        PaxReferenceTSTCHILD += drRows["PAXREFERENCE"].ToString() + ",";
                    }
                    if (drRows["PAXTYPE"].ToString().ToUpper().Trim() == "INF")
                    {
                        PaxReferenceTSTINFANT += drRows["PAXREFERENCE"].ToString() + ",";
                    }
                }


                PaxReferenceTST = (PaxReferenceTSTADULT == "" ? "0" : PaxReferenceTSTADULT.Trim(',')) + "~" + (PaxReferenceTSTCHILD == "" ? "0" : PaxReferenceTSTCHILD.Trim(',')) + "~" + (PaxReferenceTSTINFANT == "" ? "0" : PaxReferenceTSTINFANT.Trim(','));


                foreach (DataRow drRows in dsPNRINFO.Tables["PAXDETAILS"].Rows)
                {
                    dcTotalFare += Convert.ToDecimal(drRows["GROSSFARE"].ToString());
                }

                //***********************End calculating Booking Amount***************************

                string tourCode = string.Empty;
                string bookingAmt = string.Empty;
                string segmentrefence = string.Empty;
                string paxrefence = string.Empty;

                bookingAmt = dtTSTSegment.Rows[0]["GROSSAMT"].ToString().ToUpper().Trim();
                segmentrefence = dtTSTSegment.Rows[0]["SEGMENTREFERENCE"].ToString().ToUpper().Trim();
                paxrefence = dtTSTSegment.Rows[0]["PAXREFERENCE"].ToString().Trim();

                var results = from myRow in dtTSTSegment.AsEnumerable()
                              where myRow["PAXREFERENCE"].ToString().Trim() == paxrefence.ToString().Trim()
                              select myRow;

                DataSet dsPriceSegment = new DataSet();
                DataTable dtPriceSegment = results.CopyToDataTable();
                dtPriceSegment.TableName = "PRICESEGMENT";
                dsPriceSegment.Tables.Add(dtPriceSegment.Copy());

                //strAUTOQUEUEFLAG = CRS.ToString().ToUpper().Trim() == "1G" ? "Y" : "N";
                //------------------------------------------------------------------------
                #region PRICING CODE
                if (strTerminalType.ToUpper().Trim() == "W")
                {
                    if (string.IsNullOrEmpty(CorporateCode))
                    {
                        try
                        {

                            string strBaseOrgin = string.Empty;
                            string strbaseDestination = string.Empty;
                            string strBaseDeptDate = string.Empty;

                            string travelOrigin = string.Empty;
                            string travelDestination = string.Empty;
                            string travelFromDate = string.Empty;
                            string travelToDate = string.Empty;
                            string strSector = string.Empty;
                            string RBDCLASS = string.Empty;

                            foreach (DataRow dsrows in dsPriceSegment.Tables["PRICESEGMENT"].Rows)
                            {
                                RBDCLASS += dsrows["CLASS"].ToString() + ",";
                                strSector += dsrows["ORIGIN"].ToString().ToUpper().TrimEnd() + "-" + dsrows["DESTINATION"].ToString().ToUpper().TrimEnd() + ",";
                            }

                            //****************************EXTRA PARAMETER**********************************

                            decimal minSegmentId = 0;
                            var qryMinQry = from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                            select Convert.ToDecimal(p.Field<string>("SEGMENTNO").ToString());
                            minSegmentId = qryMinQry.Min();

                            decimal maxSegmentId = 0;
                            var qryMaxQry = from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                            select Convert.ToDecimal(p.Field<string>("SEGMENTNO").ToString());
                            maxSegmentId = qryMaxQry.Max();

                            var qryMin = (from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                          where p.Field<string>("SEGMENTNO").ToString().Trim() == minSegmentId.ToString().Trim()
                                          select new
                                          {
                                              DEPARTUREDATE = p.Field<string>("DEPARTUREDATE"),
                                              ORIGIN = p.Field<string>("ORIGIN")
                                          }).Distinct();

                            if (qryMin != null && qryMin.Count() > 0)
                            {
                                DataTable dtMin = ConvertToDataTable(qryMin);
                                if (dtMin != null && dtMin.Rows.Count > 0 && dtMin.Columns.Contains("DEPARTUREDATE"))
                                    travelFromDate = dtMin.Rows[0]["DEPARTUREDATE"].ToString();
                                travelFromDate = DateTime.ParseExact(travelFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                if (dtMin != null && dtMin.Rows.Count > 0 && dtMin.Columns.Contains("ORIGIN"))
                                    travelOrigin = dtMin.Rows[0]["ORIGIN"].ToString();
                            }

                            var qryMax = (from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                          where p.Field<string>("SEGMENTNO").ToString().Trim() == maxSegmentId.ToString().Trim()
                                          select new
                                          {
                                              ARRIVALDATE = p.Field<string>("ARRIVALDATE"),
                                              DESTINATION = p.Field<string>("DESTINATION")

                                          }).Distinct();
                            if (qryMax != null && qryMax.Count() > 0)
                            {
                                DataTable dtMax = ConvertToDataTable(qryMax);
                                if (dtMax != null && dtMax.Rows.Count > 0 && dtMax.Columns.Contains("ARRIVALDATE"))
                                    travelToDate = dtMax.Rows[0]["ARRIVALDATE"].ToString();
                                travelToDate = DateTime.ParseExact(travelToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                if (dtMax != null && dtMax.Rows.Count > 0 && dtMax.Columns.Contains("DESTINATION"))
                                    travelDestination = dtMax.Rows[0]["DESTINATION"].ToString();

                            }
                            //****************************END EXTRA PARAMETER**********************************

                            try
                            {
                                string strGetBaseOrigin = string.Empty;
                                string strGetBaseDestination = string.Empty;
                                string strGetBaseTravelDeptDate = string.Empty;
                                string strGetTravelEndDate = string.Empty;
                                DataTable dtFetchBase = new DataTable();
                                dtFetchBase = dsPNRINFO.Tables["PassengerPNRDetails"].Copy();
                                FindBaseCities_NEW(dtFetchBase, strAgentID, strUserName, Ipaddress, strTerminalType, Convert.ToDecimal(sequnceID),
                                    ref strGetBaseOrigin, ref strGetBaseDestination, ref strGetBaseTravelDeptDate, ref strGetTravelEndDate, strTerminalId);

                                if (!string.IsNullOrEmpty(strGetBaseOrigin))
                                {
                                    travelOrigin = strGetBaseOrigin;
                                    strBaseOrgin = strGetBaseOrigin;
                                }
                                if (!string.IsNullOrEmpty(strGetBaseDestination))
                                {
                                    travelDestination = strGetBaseDestination;
                                    strbaseDestination = strGetBaseDestination;
                                }
                                if (!string.IsNullOrEmpty(strGetBaseTravelDeptDate))
                                    travelFromDate = strGetBaseTravelDeptDate;
                                if (!string.IsNullOrEmpty(strGetTravelEndDate))
                                    travelToDate = strGetTravelEndDate;

                            }
                            catch (Exception ex)
                            {
                                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "X", "RetrieveBookingController.cs", "FindBaseCities_NEW", ex.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                                strBaseOrgin = travelOrigin;
                                strbaseDestination = travelDestination;
                            }
                            strSector += strBaseOrgin.ToUpper().TrimEnd() + "-" + strbaseDestination.ToUpper().TrimEnd();
                            //strSector = strSector.TrimEnd(',');
                            string Eventdt = "<EVENT><REQUEST>FetchPricingcodeForQTKT CORPORATE CODE DETAILS</REQUEST>"
                                    + "<AGENT_ID>" + strClientID.ToString().Trim().ToUpper() + "</AGENT_ID>"
                                    + "<BRANCH_ID>" + BranchID.ToString().Trim() + "</BRANCH_ID>"
                                    + "<AIRLINE_CODE>" + dsPriceSegment.Tables["PRICESEGMENT"].Rows[0]["PLATINGCARRIER"].ToString().ToUpper().Trim() + "</AIRLINE_CODE>"
                                    + "<AIRPORT_TYPE>" + segmenType.ToString().Trim().ToUpper() + "</AIRPORT_TYPE>"
                                    + "<SUPPLIER_ID>" + CRS.ToString().ToUpper().Trim() + "</SUPPLIER_ID>"
                                    + "<RBD_CLASS>" + RBDCLASS.ToString().ToUpper().TrimEnd(',') + "</RBD_CLASS>"
                                    + "<TRAVEL_FROM>" + travelFromDate.ToString().Trim() + "</TRAVEL_FROM>"
                                    + "<TRAVEL_TO>" + travelToDate.ToString().Trim() + "</TRAVEL_TO>"
                                    + "<ORIGIN>" + travelOrigin.ToString().Trim() + "</ORIGIN>"
                                    + "</EVENT>";
                            DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "checkfarepricingcode~REQ", Eventdt.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");

                            DataSet dsPricingcodeForQTKT = _InplantService.FetchPricingcodeForQTKT("", strClientID, strClientID + "01", strUserName, strBranchId, CRS.ToString().ToUpper(),
                                Ipaddress, sequnceID, strTerminalType, strBaseOrgin, strbaseDestination, strSector, RBDCLASS, "", travelFromDate, travelToDate,
                                dsPriceSegment.Tables["PRICESEGMENT"].Rows[0]["PLATINGCARRIER"].ToString().ToUpper().Trim(), "", "Fetch_QueueFARE_Details", ref strErrorMsg);

                            #region Response Log
                            Eventdt = "<EVENT><RESPONSE>Fetch_QueueFARE_Details CORPORATE CODE DETAILS</RESPONSE>";
                            if (dsPricingcodeForQTKT != null && dsPricingcodeForQTKT.Tables.Count > 0)
                            {
                                if (dsPricingcodeForQTKT.Tables.Contains("FETCH_PRICINGCODE") && dsPricingcodeForQTKT.Tables["FETCH_PRICINGCODE"].Rows.Count > 0)
                                {
                                    strCorporatecode = dsPricingcodeForQTKT.Tables["FETCH_PRICINGCODE"].Columns.Contains("TC_CODE") ? dsPricingcodeForQTKT.Tables["FETCH_PRICINGCODE"].Rows[0]["TC_CODE"].ToString().Trim() : "";
                                    Eventdt += "<RESPONSE>SUCCESS</RESPONSE>";
                                }
                                else
                                {
                                    strCorporatecode = "";
                                    Eventdt += "<RESPONSE>FAILURE</RESPONSE>";
                                }
                                if (dsPricingcodeForQTKT.Tables.Contains("AGENT_BRANCH_PCC") && dsPricingcodeForQTKT.Tables["AGENT_BRANCH_PCC"].Rows.Count > 0)
                                {
                                    strAgnBranchpcc = dsPricingcodeForQTKT.Tables["AGENT_BRANCH_PCC"].Columns.Contains("BRANCH_PCC") ? dsPricingcodeForQTKT.Tables[1].Rows[0]["BRANCH_PCC"].ToString().TrimEnd(',') : "";
                                }                               
                            }
                            Eventdt += "<CORPORATE_CODE>" + (strCorporatecode) + "</CORPORATE_CODE>";
                            Eventdt += "<ERRORMSG>" + strErrorMsg + "</ERRORMSG>";
                            Eventdt += "</EVENT>";
                            DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "checkfarepricingcode~RES", Eventdt.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "X", "RetrieveBookingController.cs", "checkfarepricingcode", ex.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                            strCorporatecode = "";
                        }
                    }
                    else
                    {
                        strCorporatecode = CorporateCode;
                    }
                }
                else
                {
                    strCorporatecode = CorporateCode;
                }
                //Session["QueueCorporateCode" + CrsPnr.ToUpper().Trim()] = strCorporatecode;
                #endregion

                #region INDESK USER INCENTIVE EDIT OPTIONS FOR PNR OWN OFFICE ID AND BRANCE OFFICE ID SHOULD BE SAME ONLY

                //string strQueue_AGNENT_OfficeID = string.Empty;

                //if (dsPricingcodeForQTKT != null && dsPNRINFO.Tables.Count > 0)
                //{
                //    strQueue_AGNENT_OfficeID = dsPNRINFO.Tables.Contains("PassengerPNRDetails") ? dsPNRINFO.Tables["PassengerPNRDetails"].Rows[0]["QUEUINGOFFICEID"].ToString() : "";
                //}
                strPNROWNOfficeID = dsPNRINFO.Tables["PassengerPNRDetails"].Columns.Contains("OFFICEID") ? dsPNRINFO.Tables["PassengerPNRDetails"].Rows[0]["OFFICEID"].ToString() : "";
                //result[res_officeid] = strQueue_AGNENT_OfficeID.Contains(strPNROWNOfficeID) ? true : false;
                result[res_officeid] = strAgnBranchpcc.Contains(strPNROWNOfficeID) ? true : false; 
                result[Res_PNROWNOFFICEID] = strPNROWNOfficeID;

                #endregion

                //****************RQRS Request Formation By VIMAL****************************
                string strClientTerminalID = string.Empty;
                string ClientID = string.Empty;
                if (strTerminalType == "T" && strClientID != "")
                {
                    ClientID = strClientID;
                    strClientTerminalID = strClientID + "01";
                }
                else
                {
                    ClientID = strPosID;
                    strClientTerminalID = strPosTID;
                }

                QueueTicketingFarePricingRQ _QueueTicketingFarePricingRQ = new QueueTicketingFarePricingRQ();
                QueueTicketingFarePricingRS _QueueTicketingFarePricingRS = new QueueTicketingFarePricingRS();

                AgentDetails _Agentdetail = new AgentDetails();
                _Agentdetail.AgentID = strAgentID;
                _Agentdetail.TerminalID = strTerminalId;

                TicketingOfficeId = Ticketing_PCC;

                _Agentdetail.AppType = "B2B";
                _Agentdetail.UserName = strUserName;
                _Agentdetail.BranchID = BranchID;
                _Agentdetail.ProductType = "RC";
                _Agentdetail.PNROfficeId = FarePricingPCC;
                _Agentdetail.TicketingOfficeId = Ticketing_PCC;

                _Agentdetail.ClientID = Corporatename;
                _Agentdetail.Version = "";
                _Agentdetail.BOAID = ClientID;
                _Agentdetail.BOATerminalID = strClientTerminalID;
                _Agentdetail.AgentType = "";
                _Agentdetail.CoOrdinatorID = "";
                _Agentdetail.IssuingBranchID = BranchID;
                _Agentdetail.EMP_ID = Employeename;
                _Agentdetail.COST_CENTER = EmpCostCenter;
                _Agentdetail.Ipaddress = Ipaddress;
                _Agentdetail.Environment = (strTerminalType == "T" ? "I" : "W");
                _Agentdetail.Platform = "B";
                _Agentdetail.PNROfficeId = SessionKey;
                _Agentdetail.ProjectID = ConfigurationManager.AppSettings["ProjectCode"].ToString();

                FarePricePNRDetail _pnr = new FarePricePNRDetail();
                _pnr.CRSPNR = CrsPnr;
                _pnr.CRSID = CRS;
                _pnr.CORPORATECODE = strCorporatecode;
                _pnr.TOURCODE = tourCode;
                _pnr.QUEUENUMBER = dsPNRINFO.Tables["AGENT_INFO"].Rows[0]["QUEUENUMBER"].ToString().ToUpper().Trim();
                _pnr.BOOKINGAMOUNT = dcTotalFare.ToString();
                _pnr.ADULT = adultCount.ToString();
                _pnr.CHILD = childCount.ToString();
                _pnr.INFANT = infantCount.ToString();
                _pnr.SEGMENTTYPE = segmenType;
                _pnr.AUTOQUEUEFLAG = strAUTOQUEUEFLAG;
                _pnr.QUEUEID = BranchID;

                DataTable dtSegDET = dsPriceSegment.Tables["PRICESEGMENT"];
                PricingSegmentDetails _seg_details;
                List<PricingSegmentDetails> _lstsegdet = new List<PricingSegmentDetails>();

                string strPLATINGCARRIER = string.Empty;
                foreach (DataRow drSegRows in dtSegDET.Rows)
                {
                    strPlattingCarrier = drSegRows["PLATINGCARRIER"].ToString();
                    _seg_details = new PricingSegmentDetails();

                    _seg_details.CARRIERCODE = drSegRows["PLATINGCARRIER"].ToString();
                    _seg_details.ORIGIN = drSegRows["ORIGIN"].ToString();
                    _seg_details.DESTINATION = drSegRows["DESTINATION"].ToString();
                    _seg_details.DEPARTUREDATE = drSegRows["DEPARTUREDATE"].ToString();
                    _seg_details.ARRIVALDATE = drSegRows["ARRIVALDATE"].ToString();
                    _seg_details.DEPARTURETIME = drSegRows["DEPARTURETIME"].ToString();
                    _seg_details.ARRIVALTIME = drSegRows["ARRIVALTIME"].ToString();
                    _seg_details.CLASS = drSegRows["CLASS"].ToString();
                    _seg_details.FAREBASISCODE = drSegRows["FAREBASISCODE"].ToString();
                    _seg_details.PLATINGCARRIER = ((strPlatingcarrier != null && strPlatingcarrier != "") ? strPlatingcarrier : drSegRows["PLATINGCARRIER"].ToString());
                    _seg_details.TSTREFERENCE = drSegRows["TSTREFERENCE"].ToString();
                    _seg_details.SEGMENTREFERENCE = drSegRows["SEGMENTREFERENCE"].ToString();
                    _seg_details.PAXTSTREFERENCE = PaxReferenceTST.TrimEnd('~');
                    strPLATINGCARRIER = ((strPlatingcarrier != null && strPlatingcarrier != "") ? strPlatingcarrier : drSegRows["PLATINGCARRIER"].ToString());
                    _lstsegdet.Add(_seg_details);
                }

                if (GSTDetails != "")
                {
                    string[] _gstLst = GSTDetails.Split('|');
                    _gstdetails.GSTAddress = _gstLst[3].ToString();
                    _gstdetails.GSTCompanyName = _gstLst[2].ToString();
                    _gstdetails.GSTEmailID = _gstLst[4].ToString();
                    _gstdetails.GSTMobileNumber = _gstLst[5].ToString();
                    _gstdetails.GSTNumber = _gstLst[1].ToString();
                }

                Payment _payemnt = new Payment();

                #region Request&Log
                string xmldata = "<REQUEST>Fetch_PassThrough_Card_details" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</REQUEST>" +
                                  "<AIRLINECODE>" + strPLATINGCARRIER.TrimEnd('|').Split('|')[0] + "</AIRLINECODE>" +
                                  "<AIRPORTID>" + segmenType + "</AIRPORTID>" +
                                  "<EMPCODE>" + "" + "</EMPCODE>" +
                                  "<CORPORATE>" + Corporatename + "</CORPORATE>" +
                                   "<CRSTYPE>" + CRS + "</CRSTYPE>" +
                                    "<BOOKINGAMOUNT>" + dcTotalFare.ToString() + "</BOOKINGAMOUNT>";

                DataSet stscard = _RaysService.Fetch_PassThrough_Card_detailsRetrieve(Corporatename, strPLATINGCARRIER.TrimEnd('|').Split('|')[0], segmenType, "", CRS, dcTotalFare.ToString());

                StringWriter strWriter = new StringWriter();
                stscard.WriteXml(strWriter);
                xmldata += "<RESPONSE>Fetch_PassThrough_Card_details" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</RESPONSE>" +
                           "<DATA>" + strWriter + "</DATA>";
                string LstrDetails = "<Fetch_PassThrough_Card_details-RESPONSE><URL>[<![CDATA[" + ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() + "]]>]</URL><QUERY>" + xmldata + "</QUERY><RESTIME>" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</RESTIME></Fetch_PassThrough_Card_details-RESPONSE>";
                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "T", "RetrieveBookingController.cs", "Fetch_PassThrough_Card_details", LstrDetails, strClientID, strPosTID, Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : "0");
                #endregion


                string strCardDetails = string.Empty;

                if (stscard.Tables.Count > 0 && stscard.Tables[0].Rows.Count > 0)
                {
                    _payemnt.PAYMENTMODE = "B";
                    _payemnt.CARDTYPE = stscard.Tables[0].Rows[0]["FOP_CARD_TYPE"].ToString();
                    _payemnt.CARDNUMBER = stscard.Tables[0].Rows[0]["FOP_CARD_NUMBER"].ToString();
                    _payemnt.CVVNUMBER = stscard.Tables[0].Rows[0]["FOP_CVV"].ToString();
                    _payemnt.CARDNAME = stscard.Tables[0].Rows[0]["FOP_CARD_NAME"].ToString();
                    _payemnt.EXPIRYDATE = stscard.Tables[0].Rows[0]["PG_EXPIRY_DATE"].ToString();
                }
                else
                {
                    _payemnt.PAYMENTMODE = "";
                    _payemnt.CARDTYPE = "";
                    _payemnt.CARDNUMBER = "";
                    _payemnt.CVVNUMBER = "";
                    _payemnt.CARDNAME = "";
                    _payemnt.EXPIRYDATE = "";
                }
                _payemnt.PASSENGER_CONTACTNO = "";
                _payemnt.REFERANSEID = "";


                string request = "";// JsonConvert.SerializeObject(_request).ToString();
                string methodname = "InvokeFarePrice";
                int servicetimeout = Convert.ToInt32(ConfigurationManager.AppSettings["QTKT_servicetimeout"]);

                _QueueTicketingFarePricingRQ.AgentDetail = _Agentdetail;
                _QueueTicketingFarePricingRQ.PricePNRDetail = _pnr;
                _QueueTicketingFarePricingRQ.PriceSegment = _lstsegdet;
                _QueueTicketingFarePricingRQ.GstDetails = _gstdetails;
                _QueueTicketingFarePricingRQ.PaymentDetail = _payemnt;
                _QueueTicketingFarePricingRQ.ISAllFare = true;

                request = JsonConvert.SerializeObject(_QueueTicketingFarePricingRQ);

                MyWebClient client = new MyWebClient();
                //client.LintTimeout = servicetimeout;
                string strURLpath = ConfigurationManager.AppSettings["QTKT_Service_URL"].ToString();
                string response = string.Empty;
                string errorMessage = string.Empty;
                string Request = request;
                string url = strURLpath + "/" + methodname;

                //  client.LintTimeout = int.MaxValue;
                client.Headers["Content-type"] = "application/json";

                //****************REQUEST LOG****************************************

                string strTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                xmlData = "<Event><REQUEST><REQUESTTIME>" + strTime + "</REQUESTTIME><PAGENAME>QUEUETICKET_WCF</PAGENAME><METHODNAME>Fetch_QueueFARE_Details</METHODNAME>"
                    + "<REQUESTJSONSTRING>" + Request + "</REQUESTJSONSTRING>"
                 + "</REQUEST></Event>";

                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                //****************END REQUEST LOG****************************************

                byte[] data = client.UploadData(url, "POST", Encoding.ASCII.GetBytes(Request));
                string strresponse = Encoding.ASCII.GetString(data);

                // string strresponse = System.IO.File.ReadAllText(@"E:\QTKTCHECKFARERES.txt");//while putting live

                QueueTicketingFarePricingRS _FareRes = JsonConvert.DeserializeObject<QueueTicketingFarePricingRS>(strresponse);

                if (_FareRes.PricingToken != null && _FareRes.PricingToken.ToString() != string.Empty)
                {
                    strPricingToken = _FareRes.PricingToken.ToString();
                }
                result[res_token] = strPricingToken;

                //****************RESPONSE LOG****************************************

                strTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                xmlData = "<Event><RESPONSE><RESPONSETIME>" + strTime + "</RESPONSETIME><PAGENAME>QUEUETICKET_WCF</PAGENAME><METHODNAME>Fetch_QueueFARE_Details</METHODNAME>"
                + "<RESPONSEJSONSTRING>" + strresponse + "</RESPONSEJSONSTRING>"
                + "</RESPONSE></Event>";

                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                //****************END REQUEST LOG****************************************

                if (!string.IsNullOrEmpty(strresponse))
                {
                    dsRetrieveFARE = _ServiceUtility.convertJsonStringToDataSet(strresponse, "");
                }

                if (dsRetrieveFARE != null && dsRetrieveFARE.Tables.Count > 0 &&
                dsRetrieveFARE.Tables.Contains("FarePriceDetails") && dsRetrieveFARE.Tables["FarePriceDetails"].Rows.Count > 0 &&
                dsRetrieveFARE.Tables.Contains("Result") && dsRetrieveFARE.Tables["Result"].Rows.Count > 0 &&
                dsRetrieveFARE.Tables["Result"].Rows[0]["Code"].ToString().Trim() == "1")
                {
                    //********************RESPONSE LOG*****************************************
                    dsRetrieveFARE.WriteXml(strwrt);
                    xmlData = string.Empty;

                    xmlData = "<EVENT>" + "<RESPONSE>Fetch_QueueFARE_Details</RESPONSE>" +
                                        "<STATUS>SUCCESS</STATUS>" +
                                        "<FAREDETAILSINFO>" + strwrt.ToString() + "</FAREDETAILSINFO>" +
                                        "<ERRORMSG>" + strErrorMsg + "</ERRORMSG>" +
                                        "</EVENT>";
                    DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                    //********************END RESPONSE LOG***********************
                    //*****************WHEN NORMAL FARE AND SPECIAL FARE SAME*******************************
                    var _faresdetails = from _fares in dsRetrieveFARE.Tables["FarePriceDetails"].AsEnumerable()
                                        orderby _fares.Field<string>("GROSSFARE").ToString() descending
                                        select new
                                        {
                                            GROSSFARE = _fares.Field<string>("GROSSFARE").ToString(),
                                            FAREQUALIFIER = _fares.Field<string>("FAREQUALIFIER").ToString(),
                                        };

                    var groupedfaresList = _faresdetails.GroupBy(u => u.GROSSFARE).Select(grp => grp.ToList()).ToList();

                    DataTable dtfinalfare = new DataTable();
                    dtfinalfare.Columns.Add("GROSSFARE");
                    dtfinalfare.Columns.Add("FAREQUALIFIER");
                    for (int i = 0; i < groupedfaresList.Count; i++)
                    {
                        if (groupedfaresList[i].Count > 1)
                        {
                            for (int j = 0; j < groupedfaresList[i].Count; j++)
                            {
                                if (groupedfaresList[i][j].FAREQUALIFIER.ToString() != "N")
                                    dtfinalfare.Rows.Add(groupedfaresList[i][j].GROSSFARE, groupedfaresList[i][j].FAREQUALIFIER);
                            }
                        }
                        else
                        {
                            dtfinalfare.Rows.Add(groupedfaresList[i][0].GROSSFARE, groupedfaresList[i][0].FAREQUALIFIER);
                        }
                    }
                    if (dtfinalfare != null && dtfinalfare.Rows.Count > 0)
                    {
                        var qryPNRTemp = (from p in dtfinalfare.AsEnumerable()
                                          from q in dsRetrieveFARE.Tables["FarePriceDetails"].AsEnumerable()
                                          where p.Field<string>("FAREQUALIFIER").ToString().ToUpper().Trim() == q.Field<string>("FAREQUALIFIER").ToString().ToUpper().Trim()
                                          && p.Field<string>("GROSSFARE").ToString().ToUpper().Trim() == q.Field<string>("GROSSFARE").ToString().ToUpper().Trim()
                                          select q);
                        DataTable dtPNRTemp = new DataTable();
                        dtPNRTemp = qryPNRTemp.CopyToDataTable();
                        dtPNRTemp.TableName = "FarePriceDetails";
                        dsRetrieveFARE.Tables["FarePriceDetails"].Clear();
                        dsRetrieveFARE.Tables["FarePriceDetails"].Merge(dtPNRTemp);
                    }

                    strResult = JsonConvert.SerializeObject(dsRetrieveFARE);
                    result[res_json] = strResult;
                    fareData = "FARE_INFO" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    Session[fareData] = dsRetrieveFARE;
                    result[res_sesid] = fareData.ToString();
                    result[res_normalFare] = dcTotalFare.ToString();
                    string specialFare = string.Empty;
                    specialFare = dsRetrieveFARE.Tables["FarePriceDetails"].Rows[0]["GROSSFARE"].ToString();
                    string diffFare = string.Empty;
                    diffFare = dsRetrieveFARE.Tables["FarePriceDetails"].Rows[0]["DIFFERENCEAMOUNT"].ToString();
                    result[res_specialFare] = specialFare.ToString();
                    result[res_FareDiff] = diffFare.ToString();

                }
                else if (dsRetrieveFARE != null && dsRetrieveFARE.Tables.Count > 0 &&
          dsRetrieveFARE.Tables.Contains("Result") && dsRetrieveFARE.Tables["Result"].Rows.Count > 0 &&
          dsRetrieveFARE.Tables["Result"].Rows[0]["Code"].ToString().Trim() == "2")
                {

                    if (TicketingOfficeId != "")
                        strErrorMsg = "Kindly queue your pnr (" + CrsPnr + ") into (" + TicketingOfficeId + ") PCC";
                    else
                        strErrorMsg = "Unable to retrive Fare details. Please try later.";


                    xmlData = string.Empty;
                    xmlData = "<EVENT>" + "<RESPONSE>Fetch_QueueFARE_Details</RESPONSE>" +
                                        "<STATUS>FAILED</STATUS>" +
                                         "<PNRINFO>" + strwrt.ToString() + "</PNRINFO>" +
                                        "<ERRORMSG>" + strErrorMsg + "</ERRORMSG>" +
                                        "</EVENT>";
                    DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                    result[error] = strErrorMsg;
                    //   return result;
                    return Json(new { Status = "0", Message = result[error], Result = result });
                }
                else
                {
                    if (dsRetrieveFARE != null && dsRetrieveFARE.Tables.Count > 0 &&
                        dsRetrieveFARE.Tables.Contains("Result") && dsRetrieveFARE.Tables["Result"].Rows.Count > 0)
                    {
                        dsRetrieveFARE.WriteXml(strwrt);
                        //strErrorMsg = dsRetrieveFARE.Tables["Result"].Rows[0]["ErrorDisplay"].ToString();
                        //if (string.IsNullOrEmpty(strErrorMsg))
                        //{
                        //    strErrorMsg = dsRetrieveFARE.Tables["Result"].Rows[0]["ErrorDescription"].ToString();
                        //}
                        strErrorMsg = "Unable to retrive Fare details. Please try later.";
                        if (!string.IsNullOrEmpty(dsRetrieveFARE.Tables["Result"].Rows[0]["ErrorDisplay"].ToString()))
                        {
                            strErrorMsg = dsRetrieveFARE.Tables["Result"].Rows[0]["ErrorDisplay"].ToString();
                        }
                        else if (!string.IsNullOrEmpty(dsRetrieveFARE.Tables["Result"].Rows[0]["ErrorDescription"].ToString()))
                        {
                            strErrorMsg = dsRetrieveFARE.Tables["Result"].Rows[0]["ErrorDescription"].ToString();
                        }
                    }
                    else
                    {
                        strErrorMsg = "Unable to retrive Fare details. Please try later.";
                    }

                    //****************INSERT TRACK FOR FAILED FARE PRICING***************************
                    //DateTime dtToday = DateTime.Now;
                    //string prefix = agentID.ToString().Trim().ToUpper() + dtToday.Day.ToString() + dtToday.Month.ToString() + dtToday.Year.ToString() + "T";

                    //if (dsPNRReq.Tables["AGENT_INFO"].Columns.Contains("PARENT_TRACKID") && !string.IsNullOrEmpty(dsPNRReq.Tables["AGENT_INFO"].Rows[0]["PARENT_TRACKID"].ToString().Trim()))
                    //    prefix += "|" + dsPNRReq.Tables["AGENT_INFO"].Rows[0]["PARENT_TRACKID"].ToString().Trim();
                    //string PaymentMode = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["PAYMENTMODE"].ToString().ToUpper().Trim();
                    //string strErrorMsg1 = string.Empty;
                    //bool returnStatus = Insert_UserTrackID(agentID, userName, "PRICING-FAILED", strErrorMsg,
                    //     prefix.ToString(), "INSERT", terminalType, ipAddress, sequenceID, PaymentMode, ref strErrorMsg1,
                    //     ref trackIDStatus, dsPNRReq, dcTotalFare);

                    //if (string.IsNullOrEmpty(trackIDStatus))
                    //{
                    //    strErrorMsg = "Problem occurred while generate the track ID";
                    //    return false;
                    //}
                    //****************END INSERT TRACK FOR FAILED FARE PRICING***************************
                    xmlData = string.Empty;
                    xmlData = "<EVENT>" + "<RESPONSE>Fetch_QueueFARE_Details</RESPONSE>" +
                                        "<STATUS>FAILED</STATUS>" +
                                         "<PNRINFO>" + strwrt.ToString() + "</PNRINFO>" +
                                        "<ERRORMSG>" + strErrorMsg + "</ERRORMSG>" +
                                        "</EVENT>";
                    DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                    result[error] = strErrorMsg;
                    // return result;
                    return Json(new { Status = "0", Message = result[error], Result = result });
                }
                //-------------------------------------------------------------------------

                DataSet dataSet = _RaysService.Fetch_Client_Credit_Balance_DetailsWEB(Corporatename, Session["username"].ToString(), Session["ipAddress"].ToString(), Session["sequenceid"].ToString(), strModule);

                if (dataSet != null && dataSet.Tables.Count != 0 && dataSet.Tables[0].Rows.Count != 0)
                {
                    string Allowbokingtype = dataSet.Tables[0].Rows[0]["CLT_ALLOW_ACC_TYPE"].ToString().Trim();
                    if (Allowbokingtype.Contains("C"))
                    {
                        Paymentdetails += "C";
                    }
                    if (Allowbokingtype.Contains("B") && ConfigurationManager.AppSettings["AllowPassThroughAirline"].ToString().Contains(strPlattingCarrier))
                    {
                        Paymentdetails += "~B";
                    }
                    if (Allowbokingtype.Contains("T") && strTerminalType == "T")
                    {
                        Paymentdetails += "~T";
                    }
                    if (Allowbokingtype.Contains("H") && strTerminalType == "T")
                    {
                        Paymentdetails += "~H";
                    }
                }
                if (dataSet != null && dataSet.Tables.Count > 10 && dataSet.Tables[10].Rows.Count > 0)
                    ApprovalDetails = JsonConvert.SerializeObject(dataSet.Tables[10]);

            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString() + ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line")).ToString();
                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "X", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", ErrorMsg, strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                result[error] = "Problem occured while getting Fare details!";
                return Json(new { Status = "0", Message = result[error], Result = result });
            }
            return Json(new { Status = "1", Message = "", Result = result, Paymentdetails = Paymentdetails, ApprovalDetails = ApprovalDetails });
        }

        private void FindBaseCities_NEW(DataTable dtPNRReq, string agentID, string userName, string ipAddress, string terminalType, decimal sequenceID, ref string strBaseOrgin, ref string strbaseDestination, ref string strBaseDeptDate, ref string strFinalArrivalDate, string strTerminalID)
        {
            try
            {
                strBaseOrgin = string.Empty;
                strbaseDestination = string.Empty;
                strBaseDeptDate = string.Empty;
                strFinalArrivalDate = string.Empty;

                DataTable dtActualData = new DataTable();
                //**********************vasan****************************

                var qryMinQryDeparture = (from p in dtPNRReq.AsEnumerable()
                                          orderby Convert.ToDecimal(DateTime.ParseExact(p["DEPARTUREDATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd") + p["DEPARTURETIME"].ToString().Replace(":", ""))
                                          select new
                                          {
                                              SEG_NO = p["SEGMENTNO"].ToString(),
                                              DEPARTUREDATETIME = p["DEPARTURETIME"].ToString(),//p["DEPARTUREDATE"].ToString() + " " + p["DEPARTURETIME"].ToString(),
                                              DEPARTUREDATE = DateTime.ParseExact(p["DEPARTUREDATE"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"),//p["DEPARTUREDATE"].ToString(),//
                                              ORIGIN = p["ORIGIN"].ToString(),
                                              ARRIVALDATETIME = p["ARRIVALDATE"].ToString() + " " + p["ARRIVALTIME"].ToString(),
                                              ARRIVALDATE = DateTime.ParseExact(p["ARRIVALDATE"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd"),//p["ARRIVALDATE"].ToString(),//
                                              DESTINATION = p["DESTINATION"].ToString(),
                                              LOGIC_DATE_RANGE = "",
                                              LOGIC_TIME_RANGE = ""
                                          }).Distinct();

                dtActualData = ConvertToDataTable(qryMinQryDeparture);

                if (dtActualData.Rows.Count > 0)
                {
                    strBaseOrgin = dtActualData.Rows[0]["ORIGIN"].ToString();
                    int TotSegCount = 0;
                    TotSegCount = dtActualData.Rows.Count;
                    strbaseDestination = dtActualData.Rows[TotSegCount - 1]["DESTINATION"].ToString();
                    strBaseDeptDate = dtActualData.Rows[0]["DEPARTUREDATE"].ToString();
                    strFinalArrivalDate = dtActualData.Rows[TotSegCount - 1]["ARRIVALDATE"].ToString();


                    if (strBaseOrgin == strbaseDestination)
                    {
                        if (dtActualData.Rows.Count == 2)
                        {
                            if (strBaseOrgin != dtActualData.Rows[1]["DESTINATION"].ToString())
                            {
                                strbaseDestination = dtActualData.Rows[1]["DESTINATION"].ToString();
                            }
                        }
                        else if (dtActualData.Rows.Count > 2)
                        {
                            for (int Rowcnt = 0; Rowcnt < dtActualData.Rows.Count; Rowcnt++)
                            {
                                string SecondDeptDate = string.Empty;
                                if (Rowcnt != dtActualData.Rows.Count - 1)
                                {
                                    string FirstArrDate = dtActualData.Rows[Rowcnt]["DEPARTUREDATE"].ToString();// DateTime.ParseExact(dtActualData.Rows[Rowcnt]["DEPARTUREDATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

                                    SecondDeptDate = dtActualData.Rows[Rowcnt + 1]["ARRIVALDATE"].ToString();// DateTime.ParseExact(dtActualData.Rows[Rowcnt + 1]["ARRIVALDATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

                                    dtActualData.Rows[Rowcnt]["LOGIC_DATE_RANGE"] = Convert.ToDecimal(SecondDeptDate) - Convert.ToDecimal(FirstArrDate);
                                }
                                else
                                    dtActualData.Rows[Rowcnt]["LOGIC_DATE_RANGE"] = "";
                            }

                            if (dtActualData.Rows.Count > 1)
                            {
                                var qryMaxQryThree = from p in dtActualData.AsEnumerable()
                                                     where p["LOGIC_DATE_RANGE"].ToString() != string.Empty
                                                     select Convert.ToDecimal(p.Field<string>("LOGIC_DATE_RANGE").ToString());


                                decimal MaxSegmentId = qryMaxQryThree.Max();

                                var qryDateFinal = (from p in dtActualData.AsEnumerable()
                                                    where p["LOGIC_DATE_RANGE"].ToString() == MaxSegmentId.ToString()
                                                    //orderby Convert.ToDecimal(p["SEG_NO"].ToString()) ascending//neeed to change datetime 
                                                    orderby Convert.ToDecimal(p["DEPARTUREDATE"].ToString() + p["DEPARTUREDATETIME"].ToString().Replace(":", ""))//(DateTime.ParseExact(p["DEPARTUREDATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd") + p["DEPARTURETIME"].ToString().Replace(":", ""))
                                                    select new
                                                    {
                                                        SEG_NO = p["SEG_NO"],
                                                        DES_SEG = p["DESTINATION"],
                                                        LOGIC_DATE_RANGE = p["LOGIC_DATE_RANGE"]
                                                    }).Distinct();
                                if (qryDateFinal != null)
                                {
                                    DataTable dtFinal = ConvertToDataTable(qryDateFinal);
                                    strbaseDestination = dtFinal.Rows[0]["DES_SEG"].ToString();
                                }
                            }
                        }
                    }
                }
                else
                {
                    strBaseOrgin = string.Empty;
                    strbaseDestination = string.Empty;
                    strBaseDeptDate = string.Empty;
                    strFinalArrivalDate = string.Empty;
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.Retrievetcktlog(userName, "X", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", ex.Message.ToString(), agentID, strTerminalID, sequenceID.ToString());
                strBaseOrgin = string.Empty;
                strbaseDestination = string.Empty;
                strBaseDeptDate = string.Empty;
                strFinalArrivalDate = string.Empty;
            }
        }

        public ActionResult Fetch_Queue_Accounting(string hdf_pnrinfo, string CorporateCode, string CrsPnr, string CRS, string QueueingNumber, string TSTCOUNT,
            string AirportTypes, string Corporatename, string Employeename, string Empmailname, string EmpCostCenter, string EmpRefID, string BranchID, string SessionKey,
            string GSTDetails, string Faretype, string DiscountVal, string Markupval, string TotalGrossamt, string Newempflag, string faretypereason,
            string PaymentMode, string CommonValue, string OtherTicketInfo, string PaymentDetails, string strCashPaymentDet, string cardGetSet, string AirlineCategory,
            string Tourcode)
        {
            ArrayList result = new ArrayList();
            DataSet dsBooked = new DataSet();
            int error = 0;
            int res_sesid = 1;
            int res_normalFare = 2;
            int res_specialFare = 3;
            int res_FareDiff = 4;
            int res_json = 5;
            int res_adultcount = 6;
            int res_childcount = 7;
            int res_infantcount = 8;
            int res_token = 9;

            string fareData = string.Empty;
            result.Add("");
            result.Add("");
            result.Add("");
            result.Add("");
            result.Add("");
            result.Add("");//5
            result.Add("");
            result.Add("");
            result.Add("");
            result.Add("");

            string strErrorMsg = string.Empty;
            string str_pnrinfo = string.Empty;
            string IPAddress = string.Empty;
            string FarePricingPCC = string.Empty;

            StringWriter strwrt = new StringWriter();

            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;
            string strBranchId = string.Empty;
            string strTopUpBranchID = string.Empty;
            string strOfficeId = string.Empty;
            string strCorporatecode = string.Empty;
            string TicketingOfficeId = string.Empty;
            string strAUTOQUEUEFLAG = "N";
            string strPlattingCarrier = string.Empty;
            string xmlData = string.Empty;
            string strClientID = Corporatename;

            DataSet dsRetrieveFARE = new DataSet();
            string strResult = string.Empty;
            string strPricingToken = string.Empty;

            string strPNR = string.Empty;

            string TravPNR = string.Empty;
            string strErrormsg = string.Empty;
            string lstrSupplierID = string.Empty;
            string lstrTCKPCC = string.Empty;
            string lstrBookinSupp = string.Empty;
            string lstrBookPCC = string.Empty;
            string lstrFareType = string.Empty;
            string lstrCabin = string.Empty;

            string strposid = string.Empty;
            string strpostid = string.Empty;
            byte[] picbyte = null;

            string CheckpnrStu = string.Empty;
            string TerminalType = string.Empty;
            string sessionKey = string.Empty;
            string _TravPNR = string.Empty;
            string _errorMSG = string.Empty;
            string OfficeID = SessionKey;

            try
            {

                strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
                strposid = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strpostid = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                strUserName = Session["username"] != null ? Session["username"].ToString() : "";
                SessionKey = Session["Sessionkey"] != null ? Session["Sessionkey"].ToString() : "";
                TerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();

                if (strAgentID == "" || strposid == "" || strTerminalId == "" || strUserName == "")
                {
                    result[error] = "Your session has expired!";
                    return Json(new { Status = "-1", Message = result[error], Result = result });
                }

                #region FOR CHECKING CLIENT BALANCE
                try
                {

                    decimal currentBanlance = 0;
                    string strErrorMsgs = string.Empty;
                    _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();

                    if (PaymentMode == "T")
                    {
                        DataSet dsCurrentBalance = _RaysService.Fetch_Client_Credit_Balance_DetailsWEB(Corporatename, strUserName, Session["IPAddress"] != null ? Session["IPAddress"].ToString() : "",
                            Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "", strModule);

                        if (dsCurrentBalance != null && dsCurrentBalance.Tables.Count > 0) //&& dsCurrentBalance.Tables[3].Rows.Count > 0
                        {
                            for (int i = 0; i < dsCurrentBalance.Tables[0].Rows.Count; i++)
                            {
                                string ccur = dsCurrentBalance.Tables[0].Rows[i]["CLT_ALLOW_ACC_TYPE"].ToString().Trim();
                                string appcurrency = dsCurrentBalance.Tables[0].Rows[i]["CCD_CURRENCY_CODE"].ToString().Trim();
                                string[] ss = ccur.Split('|');
                                if (ss.Length > 1)
                                {
                                    for (int j = 0; j < ss.Length; j++)
                                    {
                                        if (ConfigurationManager.AppSettings["currency"].ToString() == appcurrency)
                                        {
                                            if (ss[j] == "C" && (PaymentMode == "C" || PaymentMode == "H"))
                                            {
                                                currentBanlance = Convert.ToDecimal(dsCurrentBalance.Tables[0].Rows[i]["CREDITBALANCE"].ToString());
                                            }
                                            if (ss[j] == "T" && PaymentMode == "T")
                                            {
                                                currentBanlance = Convert.ToDecimal(dsCurrentBalance.Tables[0].Rows[i]["CLT_OPENING_BALANCE"].ToString());
                                            }
                                        }
                                    }
                                }
                            }


                            string togrossamt = Session["TotalGrossAmount"] != null ? Session["TotalGrossAmount"].ToString() : TotalGrossamt != null ? TotalGrossamt : "";
                            //currentBanlance = Convert.ToDecimal(dsCurrentBalance.Tables[3].Rows[0]["CLIENT_BALANCE"].ToString());
                            if (currentBanlance < Convert.ToDecimal(togrossamt))
                            {
                                result[error] = "Sorry for the inconvenience balance is too low. Please update the balance !.";
                                return Json(new { Status = "0", Message = result[error], Result = result });
                            }
                        }
                        else
                        {
                            // DatabaseLog.LogData(_CREDENTIALS.CLIENT_ID, "E", "BOOKINGPAGE", "BRS~" + _CREDENTIALS.TRIPTYPE + "~" + _ItineraryFlightsList[0].Stock + "~" + "Success", xmldata, Session["CompanyID"] != null ? Session["CompanyID"].ToString() : "", Session["UserID"] != null ? Session["UserID"].ToString() : "", Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : "", strIPAddress.ToString());
                            DatabaseLog.Retrievetcktlog(Corporatename, "E", "RetrieveBookingController.cs", "Fetch_Queue_Accounting", currentBanlance.ToString(), strClientID, strpostid, Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : "0");
                            result[error] = "Unable to get Balance Amount.Please contact customer care.";
                            return Json(new { Status = "0", Message = result[error], Result = result });

                        }
                    }
                }
                catch (Exception ex)
                {
                    result[error] = "Problem occured while getting client balance - (#05)";
                    return Json(new { Status = "0", Message = result[error], Result = result });
                }

                #endregion

                //if (SessionKey == "")
                //{
                Object _retriveDetails = RetrivePNRDetails(CrsPnr, CRS, QueueingNumber, AirportTypes,
                                        Corporatename, Employeename, EmpCostCenter, EmpRefID, BranchID, Faretype, "", "", "", OfficeID);
                //}

                SessionKey = Session["Sessionkey"] != null ? Session["Sessionkey"].ToString() : "";
                //RetriveResponse L = (RetriveResponse)_retriveDetails.Data;
                DataSet dsPNRINFO = new DataSet();

                if (SessionKey != "")
                {
                    dsPNRINFO = ((DataSet)Session[SessionKey]).Copy();
                }
                else
                {
                    return Json(new { Status = "0", Message = "Unable to retrieve pnr details", Result = "" });
                }

                #region insert new employee details
                if (hdf_pnrinfo != "1")
                {
                    if (Newempflag == "Y")
                    {
                        try
                        {
                            _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                            string strbDOB = "";
                            string strUserID = "";
                            string xmldata = "";
                            string strbExistingEmp = "";
                            string returnStr = "";
                            string returnEmp = "";
                            string strbTitle = "";
                            string strbFName = "";
                            string strbLName = "";
                            string strbbEmailid = "";
                            string strbGender = "";
                            string strbEmp = "";
                            string strSequenceid = "0";
                            string strIpaddress = string.Empty;
                            if (dsPNRINFO != null && dsPNRINFO.Tables.Count > 0)
                            {
                                if (dsPNRINFO.Tables["T_T_PASSENGER_INFO"] != null && dsPNRINFO.Tables["T_T_PASSENGER_INFO"].Rows.Count > 0)
                                {
                                    strbTitle = dsPNRINFO.Tables["T_T_PASSENGER_INFO"].Rows[0]["PSG_PASSENGER_TITLE"].ToString();
                                    strbFName = dsPNRINFO.Tables["T_T_PASSENGER_INFO"].Rows[0]["PSG_FIRST_NAME"].ToString();
                                    strbLName = dsPNRINFO.Tables["T_T_PASSENGER_INFO"].Rows[0]["PSG_LAST_NAME"].ToString();
                                    strbDOB = dsPNRINFO.Tables["T_T_PASSENGER_INFO"].Rows[0]["PSG_DOB"].ToString();
                                }
                            }
                            //if (streditoptions != "Y")
                            //{
                            //    strCheckEmail = _RaysService.CheckEmailAvailability(strbEmailid, strbEmp, strbCompCode, _insertMeetingDet.USERID, Session["UserName"] != null ? Session["UserName"].ToString() : "", strSequenceid, strIpaddress);
                            //}
                            //if (streditoptions == "Y")
                            //{
                            //    strbExistingEmp = streditusrcode;
                            //    strCheckEmail = "VALID";
                            //}
                            string strbContact = EmpCostCenter;
                            int strActive = 1;
                            strbExistingEmp = EmpRefID;
                            strbEmp = EmpRefID;
                            strUserID = EmpRefID;
                            strbbEmailid = Empmailname;
                            _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                            strbDOB = strbDOB != null && strbDOB != "" ? DateTime.ParseExact(strbDOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy") : "";

                            #region Request
                            xmldata = "<EVENT><REQUEST>INSERTMEETINGDETAILSTDK_CREATEEMPLOYEE</REQUEST>"
                                + "<strCompanyCode>" + Corporatename + "</strCompanyCode>"
                                + "<strbbEmailid>" + Employeename + "</strbbEmailid>"
                                 //  + "<strbUSDCode>" + strbUSDCode + "</strbUSDCode>"
                                 + "<strbExistingEmp>" + strbExistingEmp + "</strbExistingEmp>"
                                 + "<strbTitle>" + strbTitle + "</strbTitle>"
                                 + "<strbFName>" + strbFName + "</strbFName>"
                                  + "<strbLName>" + strbLName + "</strbLName>"
                                  + "<strbGender>" + strbGender + "</strbGender>"
                                  + "<strbDOB>" + strbDOB + "</strbDOB>"
                                  + "<strbContact>" + strbContact + "</strbContact>"
                                  + "<strActive>" + strActive + "</strActive>"
                                  + "<strUserID>" + strUserID + "</strUserID>"
                                  + "<strbEmp>" + strbEmp + "</strbEmp></EVENT>";
                            DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "T", "RetrieveBookingController.cs", "InsertMeetingDetailsTDK", xmldata, strClientID, strpostid, "");
                            #endregion

                            string blresul = _RaysService.InsertWEBEmployeeDetails(Corporatename, "", "", "", "", Empmailname,
                              Employeename, strbExistingEmp, strbTitle, strbFName, strbLName, strbGender, strbDOB, "", strbContact, "",
                               "", "", "", "", "", "", "", "", "",
                               "", strActive, "", "", "", "", "",
                               "", "", "", "", "", "", "",
                               "", "", "", "", "", "", "",
                               "", "", "", "", ref returnStr, ref returnEmp, "",
                               "", "", "", Corporatename, strUserID, Session["UserName"] != null ? Session["UserName"].ToString() : "", strSequenceid, strIpaddress, strbEmp, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");


                            if (returnEmp != null && returnEmp != "" && returnEmp != "EmailExist" && returnEmp != "IdExist")
                            {
                                Empmailname = Employeename;
                                Employeename = returnEmp;
                                Session["Employeename"] = returnEmp;
                                Session["Empmailname"] = Empmailname;
                                EmpCostCenter = "";
                                EmpRefID = "NEW";
                            }
                            else if ((returnEmp == null || returnEmp == "" || returnEmp == "EmailExist" || returnEmp == "IdExist"))
                            {
                                result[error] = returnEmp == "" ? "Problem occured while Create Employee Details .Please contact customer care(#05)" : "Mailid is already exist.Please contact customer care(#05)";
                                return Json(new { Status = "0", Message = result[error], Result = result });

                            }

                        }
                        catch (Exception ex)
                        {
                            result[error] = "Problem occured while creating employee details! (#05)";
                            return Json(new { Status = "0", Message = result[error], Result = result });
                        }
                    }
                }
                else
                {
                    if (Session["Empmailname"] != null && Session["Empmailname"].ToString() == Employeename)
                    {
                        Empmailname = Session["Empmailname"].ToString();
                        Employeename = Session["Employeename"] != null ? Session["Employeename"].ToString() : "";
                        EmpCostCenter = "";
                        EmpRefID = "NEW";
                    }
                }
                #endregion


                if (dsPNRINFO.Tables["T_T_TICKET_INFO"].Columns.Contains("TCK_TOUR_CODE") && !(dsPNRINFO.Tables["T_T_TICKET_INFO"].Rows[0]["TCK_TOUR_CODE"] != null && dsPNRINFO.Tables["T_T_TICKET_INFO"].Rows[0]["TCK_TOUR_CODE"].ToString() != ""))
                {
                    dsPNRINFO.Tables["T_T_TICKET_INFO"].Rows[0].SetField("TCK_TOUR_CODE", Tourcode);
                }

                dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Columns.Add("PCI_CLIENT_DISCOUNT");
                dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Columns.Add("PCI_CLIENT_MARKUP");
                dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Columns.Add("POSID");
                dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Columns.Add("POSTERMINALID");
                dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Columns.Add("BOABRANCHID");
                dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Columns.Add("CLIENTID");
                dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Columns.Add("FORMOFPAYMENT");
                dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Columns.Add("AIRLINE_CATEGORY");


                for (int i = 0; i < dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Rows.Count; i++)
                {
                    dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Rows[i]["POSID"] = strposid;
                    dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Rows[i]["POSTERMINALID"] = strpostid;
                    dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Rows[i]["BOABRANCHID"] = BranchID;
                    dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Rows[i]["CLIENTID"] = Corporatename;
                    dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Rows[i]["FORMOFPAYMENT"] = PaymentMode;
                    dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Rows[i]["AIRLINE_CATEGORY"] = AirlineCategory;
                }


                string[] Discount_Val = null;
                string[] Markup_val = null;
                Discount_Val = DiscountVal.Split('~');
                Markup_val = Markupval.Split('~');
                for (int z = 0; z < Discount_Val.Length - 1; z++)
                {
                    TotalGrossamt += Discount_Val[z] + Markup_val[z];
                    dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Rows[z]["PCI_CLIENT_DISCOUNT"] = Discount_Val[z];
                    dsPNRINFO.Tables["T_T_PAX_CLASS_INFO"].Rows[z]["PCI_CLIENT_MARKUP"] = Markup_val[z];
                }


                string[] strcomval = CommonValue.Split('|');

                DataTable dtTicket = new DataTable();
                DataRow drticket = dtTicket.NewRow();

                dtTicket.TableName = "DT_TICKET";
                dtTicket.Columns.Add("PNR");
                dtTicket.Columns.Add("CORPORATEID");
                dtTicket.Columns.Add("EMPLOYEEID");
                dtTicket.Columns.Add("EMAILID");
                dtTicket.Columns.Add("REMARK");
                dtTicket.Columns.Add("CHARGEBILITY");
                dtTicket.Columns.Add("COSTCENTER");
                dtTicket.Columns.Add("GSTSTATE");
                dtTicket.Columns.Add("GSTNO");
                dtTicket.Columns.Add("GSTCONTACTNO");
                dtTicket.Columns.Add("GSTMAIL");
                dtTicket.Columns.Add("GSTCONTACT");
                dtTicket.Columns.Add("GSTADDRESS");
                dtTicket.Columns.Add("REFERENCEID");
                dtTicket.Columns.Add("FORMOFPAYMENT");
                dtTicket.Columns.Add("PAYMENT_REF_NO");
                dtTicket.Columns.Add("PASSTHROUGH_TYPE");
                dtTicket.Columns.Add("PASSTHROUGH_UATP_NO");
                dtTicket.Columns.Add("GUEST_MAILID");
                dtTicket.Columns.Add("GUEST_PHONENO");

                dtTicket.Columns.Add("COST_CENTER_ID");
                dtTicket.Columns.Add("TD_REASON");
                dtTicket.Columns.Add("BOOKING_TYPE");
                dtTicket.Columns.Add("TO_COSTCENTER_I");
                dtTicket.Columns.Add("RECHARGE");
                dtTicket.Columns.Add("TRAVEL_REQNO");
                dtTicket.Columns.Add("BUDGET_CODE");
                dtTicket.Columns.Add("JOB_NUMBER");
                dtTicket.Columns.Add("PACKAGE_ID");
                dtTicket.Columns.Add("SUB_REASON");

                drticket["PNR"] = "";
                drticket["CORPORATEID"] = Corporatename;
                drticket["EMPLOYEEID"] = Employeename;
                drticket["EMAILID"] = Empmailname;
                drticket["REMARK"] = strcomval[1];
                drticket["CHARGEBILITY"] = "";
                drticket["COSTCENTER"] = EmpCostCenter;
                drticket["REFERENCEID"] = EmpRefID;
                drticket["FORMOFPAYMENT"] = PaymentMode;
                drticket["PAYMENT_REF_NO"] = strcomval[0];
                drticket["PASSTHROUGH_TYPE"] = strcomval[2];
                drticket["PASSTHROUGH_UATP_NO"] = strcomval[3];
                // drticket["GUEST_MAILID"] = strcomval[4];
                //  drticket["GUEST_PHONENO"] = strcomval[5];


                if (GSTDetails != "")
                {
                    string[] _gstLst = GSTDetails.Split('|');
                    drticket["GSTSTATE"] = _gstLst[0].ToString();
                    drticket["GSTNO"] = _gstLst[1].ToString();
                    drticket["GSTCONTACTNO"] = _gstLst[5].ToString();
                    drticket["GSTMAIL"] = _gstLst[4].ToString();
                    // drticket["GSTCONTACT"] = _gstLst[5].ToString();
                    // drticket["GSTADDRESS"] = _gstLst[3].ToString();

                }

                string[] _strERPDeatils_TICK = OtherTicketInfo.Split('|');

                drticket["COST_CENTER_ID"] = EmpCostCenter;
                drticket["TD_REASON"] = _strERPDeatils_TICK[4];
                drticket["BOOKING_TYPE"] = Session["Booking_Type"] != null && Session["Booking_Type"].ToString() == "E" ? "E" : "R";
                drticket["TO_COSTCENTER_I"] = EmpCostCenter;
                drticket["RECHARGE"] = _strERPDeatils_TICK[3];
                drticket["TRAVEL_REQNO"] = _strERPDeatils_TICK[1];
                drticket["BUDGET_CODE"] = _strERPDeatils_TICK[0];
                drticket["JOB_NUMBER"] = _strERPDeatils_TICK[6];
                drticket["PACKAGE_ID"] = _strERPDeatils_TICK[7];
                drticket["SUB_REASON"] = _strERPDeatils_TICK[2];

                dtTicket.Rows.InsertAt(drticket, 0);

                if (!dsPNRINFO.Tables.Contains("DT_TICKET"))
                    dsPNRINFO.Tables.Add(dtTicket);

                DataTable dtERP_ATTRIBUTES = new DataTable();
                dtERP_ATTRIBUTES.TableName = "ERP_ATTRIBUTES";
                dtERP_ATTRIBUTES.Columns.Add("AttributesName");
                dtERP_ATTRIBUTES.Columns.Add("AttributesValue");


                DataRow drERPS_Attributes = dtERP_ATTRIBUTES.NewRow();
                if (faretypereason != null && faretypereason != "")
                {
                    string[] _strERPDeatils = faretypereason.TrimEnd('~').Split('~');

                    for (int i = 0; i < _strERPDeatils.Count(); i++)
                    {
                        drERPS_Attributes = dtERP_ATTRIBUTES.NewRow();
                        string[] lstarray = _strERPDeatils[i].Split('|');
                        drERPS_Attributes["AttributesName"] = "ERP_ATR_COL" + lstarray[0];
                        drERPS_Attributes["AttributesValue"] = lstarray[2];
                        dtERP_ATTRIBUTES.Rows.InsertAt(drERPS_Attributes, 0);
                    }
                }


                DataTable dtReference = new DataTable();
                dtReference = new DataTable();
                dtReference.TableName = "Reference";
                dtReference.Columns.Add("TYPE");
                dtReference.Columns.Add("PAXREF");
                dtReference.Columns.Add("EMAILID");
                dtReference.Columns.Add("CONTACTNO");
                dtReference.Columns.Add("EMPLOYEEID");
                dtReference.Columns.Add("EMPLOYEEEMAILID");
                dtReference.Columns.Add("COSTCENTRE");

                DataRow drGuest = dtReference.NewRow();
                if (strcomval[4] != null && strcomval[4] != "")
                {
                    string[] strGuestdet = strcomval[4].Split('~');
                    drGuest["TYPE"] = Newempflag;
                    drGuest["EMAILID"] = strGuestdet[0];
                    drGuest["CONTACTNO"] = strGuestdet[1];
                    drGuest["EMPLOYEEID"] = Employeename;
                    drGuest["EMPLOYEEEMAILID"] = Empmailname;
                    drGuest["COSTCENTRE"] = EmpCostCenter;
                    drGuest["PAXREF"] = "1";
                }
                dtReference.Rows.InsertAt(drGuest, 0);


                DataTable dtpaymentMode = new DataTable();
                dtpaymentMode.TableName = "PaymentMode";

                dtpaymentMode.Columns.Add("PAYMENTMODE");
                dtpaymentMode.Columns.Add("CRADNO");
                dtpaymentMode.Columns.Add("CRADTYPE");
                dtpaymentMode.Columns.Add("TRACKID");
                dtpaymentMode.Columns.Add("PRICINGCODE");

                string[] paymentDet = CommonValue.Split('|');
                dtpaymentMode.Rows.Add(paymentDet[5]
                    , (paymentDet[5].ToUpper() == "B") ? paymentDet[5].Trim() : ""
                    , (paymentDet[5].ToUpper() == "B") ? paymentDet[2].ToUpper() : "",
                    ((paymentDet[5].ToUpper() == "P") ? paymentDet[5].Trim() : ""), CorporateCode);

                #region cash payment info details
                DataTable dtCashPayment = new DataTable();
                if (cardGetSet == "H" && !string.IsNullOrEmpty(strCashPaymentDet))
                {
                    dtCashPayment.TableName = "PAYMENTINFO";
                    dtCashPayment.Columns.Add("TPI_PRODUCT");
                    dtCashPayment.Columns.Add("TPI_EMPLOYEE_CODE");
                    dtCashPayment.Columns.Add("TPI_EMPLOYEE_NAME");
                    dtCashPayment.Columns.Add("TPI_EMPLOYEE_EMAILID");
                    dtCashPayment.Columns.Add("TPI_EMPLOYEE_BRANCH");
                    dtCashPayment.Columns.Add("TPI_CUSTOMER_MOBILENO");
                    dtCashPayment.Columns.Add("TPI_CUSTOMER_EMAILID");
                    dtCashPayment.Columns.Add("TPI_BOOKING_REFERENCE");
                    dtCashPayment.Columns.Add("TPI_EXPECT_PAYMENT");
                    dtCashPayment.Columns.Add("TPI_PAYMENT_MODE");
                    dtCashPayment.Columns.Add("TPI_PAN_CARD");
                    dtCashPayment.Columns.Add("TPI_REMARKS");
                    dtCashPayment.Columns.Add("USERNAME");
                    dtCashPayment.Columns.Add("APPNAME");
                    dtCashPayment.Columns.Add("BOOKING_TYPE");
                    dtCashPayment.Columns.Add("AIRLINE_CATEGORY");
                   
                    

                    string[] strCashPaymentDetails = Regex.Split(strCashPaymentDet, "SPLITCASH");

                    dtCashPayment.Rows.Add(
                                                         "AIR",// TPI_PRODUCT");
                                                         strCashPaymentDetails[3],
                                                          strCashPaymentDetails[4],
                                                         strCashPaymentDetails[5],
                                                          strCashPaymentDetails[7],
                                                         strCashPaymentDetails[0],// TPI_CUSTOMER_MOBILENO");
                                                         strCashPaymentDetails[1],// TPI_CUSTOMER_EMAILID");
                                                         strCashPaymentDetails[2],// TPI_BOOKING_REFERENCE");
                                                         DateTime.ParseExact(strCashPaymentDetails[8], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"),// TPI_EXPECT_PAYMENT");
                                                         strCashPaymentDetails[9],// TPI_PAYMENT_MODE");
                                                                strCashPaymentDetails[10],// TPI_PAN_CARD");
                                                                strCashPaymentDetails[11],// TPI_REMARKS");
                                                               strUserName, // USERNAME");
                                                               ConfigurationManager.AppSettings["Producttype"].ToString(),
                                                               "ACCOUNTING",
                                                               AirlineCategory);


                }
                #endregion


                dsPNRINFO.Tables.Add(dtReference);
                dsPNRINFO.Tables.Add(dtERP_ATTRIBUTES);
                dsPNRINFO.Tables.Add(dtpaymentMode);
                dsPNRINFO.Tables.Add(dtCashPayment);

                _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();

                if (hdf_pnrinfo != "1")
                {
                    CheckpnrStu = _RaysService.RetrivePNRccounting_DUP_CHK_BOA_NEW(
                        CrsPnr, strAgentID, strTerminalId, strUserName, "", "W", "0", ref _TravPNR, ref _errorMSG, "RC", strposid, strpostid);
                    if (_errorMSG != "")
                    {

                        return Json(new { Status = "2", Message = "", Result = _errorMSG });
                    }
                }


                picbyte = Compress(dsPNRINFO);
                string strRequest = JsonConvert.SerializeObject(dsPNRINFO).ToString();

                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", strRequest, strClientID, strpostid, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");

                strPNR = _RaysService.RetrivePNRccounting_BOA_NEW(
                    Convert.ToBase64String(picbyte),
                    strAgentID,
                    strTerminalId,
                    strUserName, "1", TerminalType, "1",//  "W"
                    ref TravPNR,
                    ref strErrormsg,
                   lstrSupplierID,
                   OfficeID,//lstrTCKPCC
                   lstrBookinSupp,
                   lstrBookPCC,
                   "", "", "RT", "", "",
                   lstrFareType,
                   lstrCabin,
                   "RC",
                   strposid,
                   strpostid);

                if (strErrormsg != null)
                {

                }


            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString() + ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line")).ToString();
                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "X", "RetrieveBookingController.cs", "Fetch_QueueFARE_Details", ErrorMsg, strClientID, strpostid, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                result[error] = "Problem occured while getting Fare details!";
                return Json(new { Status = "0", Message = result[error], Result = result });
            }

            return Json(new { Status = "1", Message = "", Result = strErrormsg });
        }

        public ActionResult ConfirmFare(string hdf_pnrinfo, string BookingAmt, string hdffareinfo, string farequalifier, string strfareFlag)
        {
            ArrayList result = new ArrayList();
            int error = 0;
            int result1 = 1;
            result.Add("");
            result.Add("");

            string strErrorMsg = string.Empty;
            string str_pnrinfo = string.Empty;
            bool RebookFlag = false;
            bool ClearFlag = false;
            DataSet dsBooked = new DataSet();
            bool TicketingPnr = false;
            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;
            string strBranchId = string.Empty;
            string strResult = string.Empty;
            string strTime = string.Empty;
            string stxml = string.Empty;
            string strPosID = string.Empty;
            string strPosTID = string.Empty;
            try
            {
                strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
                strUserName = Session["username"] != null ? Session["username"].ToString() : "";
                Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
                strBranchId = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";

                if (strAgentID == "" || strPosID == "" || strTerminalId == "" || strUserName == "")
                {
                    result[error] = "Your session has expired!";
                    DatabaseLog.Retrievetcktlog("Session Timeout", "E", "RetrieveBookingController.cs", "ConfirmFare", DateTime.Now.ToString("yyyyMMddHHmmssfff"), strPosID, strPosTID, DateTime.Now.ToString("yyyyMMdd"));
                    return Json(new { Status = "-1", Message = result[error], Result = result });
                }

                object objPNR = Session[hdf_pnrinfo.TrimEnd('#')];
                if (objPNR == null)
                {
                    result[error] = "Your session has expired!";
                    return Json(new { Status = "0", Message = result[error], Result = result });
                }

                DataSet dsPNRINFO = ((DataSet)Session[hdf_pnrinfo]).Copy();

                //*********************NEW FARE********************
                DataSet dsFAREINFO = ((DataSet)Session[hdffareinfo]).Copy();
                var QryTST = (from p in dsFAREINFO.Tables["FarePriceDetails"].AsEnumerable()
                              where p["GROSSFARE"].ToString().Trim() == BookingAmt.ToString().Trim() && p["FAREQUALIFIER"].ToString().Trim() == farequalifier.ToString().Trim()
                              select new
                              {
                                  ADTGROSSFARE = p["ADTGROSSFARE"].ToString().Trim(),
                                  ADTTAXBREAKUP = p["ADTTAXBREAKUP"].ToString().Trim(),
                                  CHDGROSSFARE = p["CHDGROSSFARE"].ToString().Trim(),
                                  CHDTAXBREAKUP = p["CHDTAXBREAKUP"].ToString().Trim(),
                                  INFGROSSFARE = p["INFGROSSFARE"].ToString().Trim(),
                                  INFTAXBREAKUP = p["INFTAXBREAKUP"].ToString().Trim(),
                                  CORPORATECODE = p["CORPORATECODE"].ToString().Trim(),
                                  FAREBASISCODE = p["FAREBASISCODE"].ToString().Trim(),
                                  FAREQUALIFIER = p["FAREQUALIFIER"].ToString().Trim(),
                                  GROSSFARE = p["GROSSFARE"].ToString().Trim(),
                                  ADTBASEFARE = p["ADTBASEFARE"].ToString().Trim(),
                                  CHDBASEFARE = p["CHDBASEFARE"].ToString().Trim(),
                                  INFBASEFARE = p["INFBASEFARE"].ToString().Trim()
                              }).Distinct();

                DataTable dtTST = ConvertToDataTable(QryTST);

                string adultfare = dtTST.Rows[0]["ADTGROSSFARE"].ToString().Trim();
                string adulttax = dtTST.Rows[0]["ADTTAXBREAKUP"].ToString().Trim();
                string childfare = dtTST.Rows[0]["CHDGROSSFARE"].ToString().Trim();
                string childtax = dtTST.Rows[0]["CHDTAXBREAKUP"].ToString().Trim();
                string infantfare = dtTST.Rows[0]["INFGROSSFARE"].ToString().Trim();
                string infanttax = dtTST.Rows[0]["INFTAXBREAKUP"].ToString().Trim();
                string ADTBASEFARE = dtTST.Rows[0]["ADTBASEFARE"].ToString().Trim();
                string CHDBASEFARE = dtTST.Rows[0]["CHDBASEFARE"].ToString().Trim();
                string INFBASEFARE = dtTST.Rows[0]["INFBASEFARE"].ToString().Trim();

                string str_res = Newtonsoft.Json.JsonConvert.SerializeObject(QryTST);

                // DataTable dtTEST = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(str_res);//Utilities.ConvertToDataTable(qryTest);



                foreach (DataRow dr in dsPNRINFO.Tables["PassengerPNRDetails"].Rows)
                {
                    if (dr["PAXTYPE"].ToString().Trim() == "ADT")
                    {
                        dr["GROSSAMT"] = adultfare;
                        dr["TAXBREAKUP"] = adulttax;
                        dr["BASEAMT"] = ADTBASEFARE;
                    }
                    else if (dr["PAXTYPE"].ToString().Trim() == "CHD")
                    {
                        dr["GROSSAMT"] = childfare;
                        dr["TAXBREAKUP"] = childtax;
                        dr["BASEAMT"] = CHDBASEFARE;

                    }
                    else if (dr["PAXTYPE"].ToString().Trim() == "INF")
                    {
                        dr["GROSSAMT"] = infantfare;
                        dr["TAXBREAKUP"] = infanttax;
                        dr["BASEAMT"] = INFBASEFARE;

                    }
                }
                Session[hdf_pnrinfo] = dsPNRINFO;
                decimal dctTotalFare = 0;

                TicketingPnr = CalculatingAccountsForQueueTicketing("W", strAgentID, Ipaddress,
                            strUserName, Convert.ToDecimal(sequnceID), dsPNRINFO, ref strErrorMsg, ref dctTotalFare, ref strResult, RebookFlag, "PNR", strfareFlag);

                if (strResult != string.Empty)
                {
                    dsBooked = _ServiceUtility.convertJsonStringToDataSet(strResult, "");
                    //********************RESPONSE LOG*****************************************
                    strTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    stxml = "<Event><RESPONSE><RESPONSETIME>" + strTime + "</RESPONSETIME><ConfirmfareResult>SUCCESS</ConfirmfareResult>" + "<strErrorMsg>" + strErrorMsg + "</strErrorMsg>" + "<strResult>" + strResult + "</strResult>" +
              "</RESPONSE></Event>";
                    DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "ConfirmFare", stxml.ToString(), strPosID, strPosTID, sequnceID);
                    //********************END RESPONSE LOG*****************************************
                    if (dsBooked != null && dsBooked.Tables.Count > 0 && dsBooked.Tables[0].Rows.Count > 0)
                    {
                        Session[hdf_pnrinfo] = dsBooked;
                    }
                    result[result1] = "SUCCCESS";
                }
                //if fare changed[fare_change]
                else
                {
                    result[result1] = "FAILED";
                    //********************RESPONSE LOG*****************************************
                    strTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    stxml = "<Event><RESPONSE><RESPONSETIME>" + strTime + "</RESPONSETIME><ConfirmfareResult>FAILED</ConfirmfareResult>" + "<strErrorMsg>" + strErrorMsg + "</strErrorMsg>" +
              "</RESPONSE></Event>";
                    DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "ConfirmFare", stxml.ToString(), strPosID, strPosTID, sequnceID);
                    //********************END RESPONSE LOG*****************************************
                }
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString() + ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line")).ToString();
                DatabaseLog.Retrievetcktlog(strUserName, "X", "RetrieveBookingController.cs", "ConfirmFare", ErrorMsg, strPosID, strPosTID, sequnceID);
                result[error] = "Problem occured while Confirm Fare!";
                return Json(new { Status = "0", Message = result[error], Result = result });
            }
            return Json(new { Status = "1", Message = "", Result = result });
        }

        public ActionResult Ticketing_QueuePNR_Details(string hdf_pnrinfo, string missing_title_select, string TSTCOUNT, string cardGetSet, string BookingAmt,
            string hdffareinfo, string farequalifier, string faretoken, string AirportTypes, string Corporatename, string Employeename, string EmpCostCenter,
            string EmpRefID, string BranchID, string GSTDetails, string SessionKey, string Employee_MobileNo, string PassportDetails, string tstReference,
            string FFNDetails, string Passthrough, string OBCTAX, string WithoutTaxBookingAMount, string StrFareFlag, string CRSTYPE, string ERPDetails,
            string OtherTicketInfo, string Markup, string Commission, string PLB, string TicketingMode, string CorporteCode, bool RetrieveIsGST, string Earnings,
            string CRSPNR, string strEndorsement, bool strRebookFlag, bool strFOPFlag, string strAsPerCRS, string strPlatingcarrier, string strPricingcode,
            string strCashPaymentDet)
        {
            ArrayList result = new ArrayList();
            DataSet dsBooked = new DataSet();
            int error = 0;
            int result1 = 1;
            int ds_booked = 2;
            int clear_flag = 3;
            int fare_change = 4;
            //int ServiceFee = 5;
            //int MarkUp = 6;

            result.Add("");
            result.Add("");
            result.Add("");
            result.Add("");
            result.Add("");
            //result.Add("");
            //result.Add("");

            string strErrorMsg = string.Empty;
            string str_pnrinfo = string.Empty;
            bool RebookFlag = false;
            bool ClearFlag = false;
            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;
            string strBranchId = string.Empty;
            string xmlData = string.Empty;
            string strClientID = Corporatename;
            string strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();

            bool TicketingPnr = false;
            string strErrorMessage = string.Empty;
            string trackIDStatus = string.Empty;
            DataTable dtTicketReq = new DataTable();
            StringWriter strwrt = new StringWriter();
            DataSet dsTicketingPNRResponse = new DataSet();
            DataSet dsTicketing = new DataSet();
            DataSet dsTicketingPNRReq = new DataSet("QUEUETICKETING");
            DataSet dsPNRReq = new DataSet("QUEUE_PNR");
            DataTable dtTicketedTST = new DataTable();
            DataTable ResultCode = new DataTable();
            DataSet ds = new DataSet();
            string strTrackIds = string.Empty;
            ClearFlag = true;
            string strSupplier = string.Empty;
            bool trackStatus = false;
            string strSegmentType = string.Empty;
            string BookingBranchID = string.Empty;
            string TicketingOfficeId = string.Empty;
            string RetrivePCC = string.Empty;

            string travelToDate = string.Empty;
            string travelOrigin = string.Empty;
            string travelFromDate = string.Empty;
            string travelDestination = string.Empty;
            string RBDCLASS = string.Empty;

            string strBaseOrgin = string.Empty;
            string strBaseDeptDate = string.Empty;
            string strbaseDestination = string.Empty;
            string strLastArrivalDate = string.Empty;
            DataTable dtActualData = new DataTable();
            DataTable dtFinal = new DataTable();
            string strResult = string.Empty;
            string strTime = string.Empty;
            string Corporatecode = string.Empty;

            string strresponse = string.Empty;

            string[] strpaxlist = null;
            string[] stradtlist = null;
            string[] AdtPaxlist = null;
            string[] strchdtlist = null;
            string[] CHDPaxlist = null;
            string[] strinftlist = null;
            string[] INFPaxlist = null;

            string[] FFNNumberlst = null;
            string[] FFNNumberDetlst = null;

            string[] strTotalEarning = null;
            string[] strPaxEarning = null;

            try
            {
                PNRDetails _pnrdetail = new PNRDetails();
                TicketPnrDetail _tkt_pnrdetails = new TicketPnrDetail();
                List<TicketPnrDetail> _lstpnrdet = new List<TicketPnrDetail>();
                PaxDetails Paxdetailsnew = new PaxDetails();
                List<PaxDetails> _lstpaxdet = new List<PaxDetails>();
                FFNumber FFNDetailsN = new FFNumber();
                List<FFNumber> _lstFFNdet = new List<FFNumber>();
                AgentDetails _Agentdetail = new AgentDetails();
                CBT_Credentials _CBT_Credentials = new CBT_Credentials();
                ERP_Attribute _ERP_Attribute = new ERP_Attribute();
                List<ERP_Attribute> _LST_ERP_Attribute = new List<ERP_Attribute>();
                Earning _Earning = new Earning();
                List<Earning> _lstEarning = new List<Earning>();
                QueueTicketingRQ _QueueTicketingRQ = new QueueTicketingRQ();
                QueueTicketingRS _QueueTicketingRS = new QueueTicketingRS();
                PAYMENT_INFO _CashPaymentInfo = new PAYMENT_INFO();

                string strPLATINGCARRIER = string.Empty;

                strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
                strUserName = Session["username"] != null ? Session["username"].ToString() : "";
                Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
                strBranchId = Session["branchid"] != null ? Session["branchid"].ToString() : "";

                if (strAgentID == "" || strPosID == "" || strTerminalType == "" || strUserName == "")
                {
                    result[error] = "Your session has expired!";
                    DatabaseLog.Retrievetcktlog("Session Timeout", "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", DateTime.Now.ToString("yyyyMMddHHmmssfff"), strClientID, strPosTID, DateTime.Now.ToString("yyyyMMdd"));
                    return Json(new { Status = "-1", Message = result[error], Result = result });
                }

                object objPNR = Session[hdf_pnrinfo.TrimEnd('#')];
                if (objPNR == null)
                {
                    result[error] = "Your session has expired!";
                    return Json(new { Status = "-1", Message = result[error], Result = result });
                }

                #region UsageLog
                try
                {
                    string strUsgLogRes = Base.Commonlog("Retrieve Ticketing", "", "BOOK");
                }
                catch (Exception e) { }
                #endregion

                #region SERVICE URL BRANCH BASED -- STS115
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (BranchID != "" && strBranchCredit.Contains(BranchID)))
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                        _InplantService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_INPLANT_SERVICE"].ToString();
                    }
                    else
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                        _InplantService.Url = ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString();
                    }
                }
                else
                {
                    _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                    _InplantService.Url = ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString();
                }
                #endregion

                #region cash payment info details
                if (cardGetSet == "H" && !string.IsNullOrEmpty(strCashPaymentDet))
                {
                    string[] strCashPaymentDetails = Regex.Split(strCashPaymentDet, "SPLITCASH");
                    _CashPaymentInfo.CUSTOMER_MOBILENO = strCashPaymentDetails[0];
                    _CashPaymentInfo.CUSTOMER_EMAILID = strCashPaymentDetails[1];
                    _CashPaymentInfo.BOOKING_REFERENCE = strCashPaymentDetails[2];
                    _CashPaymentInfo.EMPLOYEE_CODE = strCashPaymentDetails[3];
                    _CashPaymentInfo.EMPLOYEE_NAME = strCashPaymentDetails[4];
                    _CashPaymentInfo.EMPLOYEE_EMAILID = strCashPaymentDetails[5];
                    _CashPaymentInfo.EMPLOYEE_BRANCH = strCashPaymentDetails[7];
                    _CashPaymentInfo.EXPECT_PAYMENT = strCashPaymentDetails[8];
                    _CashPaymentInfo.PAYMENT_MODE = strCashPaymentDetails[9];
                    _CashPaymentInfo.PAN_CARD = strCashPaymentDetails[10];
                    _CashPaymentInfo.REMARKS = strCashPaymentDetails[11];
                    _CashPaymentInfo.BOOKED_BY = strUserName;
                }
                #endregion

                xmlData = "<EVENT><REQUEST>TICKETING_QUEUEPNR_DETAILS-SUBMIT</REQUEST>";
                xmlData += "<PNRSESSION>" + hdf_pnrinfo + "</PNRSESSION>";
                xmlData += "<TITLE>" + missing_title_select + "</TITLE>";
                xmlData += "<TSTCOUNT>" + TSTCOUNT + "</TSTCOUNT>";
                xmlData += "<CARD>" + cardGetSet + "</CARD>";
                xmlData += "<BOOKAMT>" + BookingAmt + "</BOOKAMT>";
                xmlData += "<FARESESSION>" + hdffareinfo + "</FARESESSION>";
                xmlData += "<FAREQUALIFIER>" + farequalifier + "</FAREQUALIFIER>";
                xmlData += "<FARETOKEN>" + faretoken + "</FARETOKEN>";
                xmlData += "<AIRPORTTYPE>" + AirportTypes + "</AIRPORTTYPE>";
                xmlData += "<CLIENTID>" + Corporatename + "</CLIENTID>";
                xmlData += "<EMPNAME>" + Employeename + "</EMPNAME>";
                xmlData += "<EMPCC>" + EmpCostCenter + "</EMPCC>";
                xmlData += "<EMPREFID>" + EmpRefID + "</EMPREFID>";
                xmlData += "<BRANCHID>" + BranchID + "</BRANCHID>";
                xmlData += "<GST>" + GSTDetails + "</GST>";
                xmlData += "<OFFICEID>" + SessionKey + "</OFFICEID>";
                xmlData += "<EMPMOB>" + Employee_MobileNo + "</EMPMOB>";
                xmlData += "<PASSPORT>" + PassportDetails + "</PASSPORT>";
                xmlData += "<TSTREF>" + tstReference + "</TSTREF>";
                xmlData += "<FFN>" + FFNDetails + "</FFN>";
                xmlData += "<PASSTHROUGH>" + Passthrough + "</PASSTHROUGH>";
                xmlData += "<OBCTAX>" + OBCTAX + "</OBCTAX>";
                xmlData += "<WOBOOKAMT>" + WithoutTaxBookingAMount + "</WOBOOKAMT>";
                xmlData += "<FAREFLAG>" + StrFareFlag + "</FAREFLAG>";
                xmlData += "<CRSTYPE>" + CRSTYPE + "</CRSTYPE>";
                xmlData += "<ERP>" + ERPDetails + "</ERP>";
                xmlData += "<OTHER>" + OtherTicketInfo + "</OTHER>";
                xmlData += "<MARKUP>" + Markup + "</MARKUP>";
                xmlData += "<COMM>" + Commission + "</COMM>";
                xmlData += "<PLB>" + PLB + "</PLB>";
                xmlData += "<TICKETINGMODE>" + TicketingMode + "</TICKETINGMODE>";
                xmlData += "<CORPORATECODE>" + CorporteCode + "</CORPORATECODE>";
                xmlData += "<EARNINGS>" + Earnings + "</EARNINGS>";
                xmlData += "<CRSPNR>" + CRSPNR + "</CRSPNR>";
                xmlData += "<ENDORSEMENT>" + strEndorsement + "</ENDORSEMENT>";
                xmlData += "<REBOOK>" + strRebookFlag.ToString() + "</REBOOK>";
                xmlData += "<FOPFLAG>" + strFOPFlag.ToString() + "</FOPFLAG>";
                xmlData += "<GSTFLAG>" + RetrieveIsGST.ToString() + "</GSTFLAG>";
                xmlData += "</EVENT>";

                DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details~Submit", xmlData, strClientID, strPosTID, sequnceID);

                DataSet dsPNRINFO = ((DataSet)Session[hdf_pnrinfo]).Copy();
                DataTable dtTST = new DataTable();
                string strMarkUp = string.Empty;
                string strCommission = string.Empty;
                string strPLB = string.Empty;
                string strIncentive = string.Empty;
                string strServiceFee = string.Empty;

                if (TicketingMode == "CT")
                {
                    DataSet dsFAREINFO = ((DataSet)Session[hdffareinfo]).Copy();
                    var QryTST = (from p in dsFAREINFO.Tables["FarePriceDetails"].AsEnumerable()
                                  where p["GROSSFARE"].ToString().Trim() == BookingAmt.ToString().Trim() && p["FAREQUALIFIER"].ToString().Trim() == farequalifier.ToString().Trim()
                                  select new
                                  {
                                      ADTGROSSFARE = p["ADTGROSSFARE"].ToString().Trim(),
                                      ADTTAXBREAKUP = p["ADTTAXBREAKUP"].ToString().Trim(),
                                      CHDGROSSFARE = p["CHDGROSSFARE"].ToString().Trim(),
                                      CHDTAXBREAKUP = p["CHDTAXBREAKUP"].ToString().Trim(),
                                      INFGROSSFARE = p["INFGROSSFARE"].ToString().Trim(),
                                      INFTAXBREAKUP = p["INFTAXBREAKUP"].ToString().Trim(),
                                      CORPORATECODE = p["CORPORATECODE"].ToString().Trim(),
                                      MARKUP = p["MARKUP"].ToString().Trim(),
                                      COMMISSION = p["EARNINGS"].ToString().Trim(),
                                      PLB = p["PLB_AMT"].ToString().Trim(),
                                      INCENTIVE = p["INCENTIVE"].ToString().Trim(),
                                  }).Distinct();

                    dtTST = ConvertToDataTable(QryTST);

                    strMarkUp = dtTST.Rows[0]["MARKUP"].ToString().Trim();
                    strCommission = dtTST.Rows[0]["COMMISSION"].ToString().Trim();
                    strPLB = dtTST.Rows[0]["PLB"].ToString().Trim();
                    strIncentive = dtTST.Rows[0]["INCENTIVE"].ToString().Trim();
                    Corporatecode = dtTST.Rows[0]["CORPORATECODE"].ToString().Trim();
                }

                //Earnings Request Formation
                strTotalEarning = Earnings.TrimEnd('~').Split('~');

                xmlData = string.Empty;
                xmlData = "<REQUEST><ACTION>EARNINGS</ACTION>";
                for (int earncount = 0; earncount < strTotalEarning.Length; earncount++)
                {
                    strPaxEarning = strTotalEarning[earncount].Split('|');

                    _Earning = new Earning();
                    _Earning.SEGMENTNO = "";
                    _Earning.OBCTax = "";
                    _Earning.PAXNO = strPaxEarning[0];
                    _Earning.SERVICE_FEE = strPaxEarning[14];//strPaxEarning[9];

                    _Earning.COMM = strPaxEarning[3];
                    _Earning.PLBAmount = strPaxEarning[6];
                    _Earning.INCENTIVE = strPaxEarning[10];
                    _Earning.MARKUP = "";


                    if (strPaxEarning[7] == "BF")
                    {
                        _Earning.MARKUP_ON_FARE = strPaxEarning[13];//strPaxEarning[8];
                        _Earning.MARKUP_ON_TAX = "";
                        _Earning.NEW_MARKUP_ON_FARE = strPaxEarning[13];
                        _Earning.NEW_MARKUP_ON_TAX = "";
                    }
                    else
                    {
                        _Earning.MARKUP_ON_TAX = strPaxEarning[13];//strPaxEarning[8];
                        _Earning.MARKUP_ON_FARE = "";
                        _Earning.NEW_MARKUP_ON_TAX = strPaxEarning[13];
                        _Earning.NEW_MARKUP_ON_FARE = "";
                    }
                    _Earning.NEW_COMM = strPaxEarning[11];
                    _Earning.NEW_PLBAmount = strPaxEarning[12];
                    _Earning.NEW_SERVICE_FEE = strPaxEarning[14];
                    _Earning.NEW_INCENTIVE = strPaxEarning[15];
                    if ((Convert.ToDouble(strPaxEarning[3]) < Convert.ToDouble(strPaxEarning[11])) ||
                        (Convert.ToDouble(strPaxEarning[6]) < Convert.ToDouble(strPaxEarning[12])) ||
                        //(Convert.ToDouble(strPaxEarning[8]) != Convert.ToDouble(strPaxEarning[13])) ||
                        //(Convert.ToDouble(strPaxEarning[9]) != Convert.ToDouble(strPaxEarning[14])) ||
                        (Convert.ToDouble(strPaxEarning[10]) < Convert.ToDouble(strPaxEarning[15])))
                    {
                        _Earning.DIFF_FLAG = "G";
                        _Earning.APPROVED_BY = strPaxEarning[16];
                    }
                    else if ((Convert.ToDouble(strPaxEarning[3]) > Convert.ToDouble(strPaxEarning[11])) ||
                        (Convert.ToDouble(strPaxEarning[6]) > Convert.ToDouble(strPaxEarning[12])) ||
                        //(Convert.ToDouble(strPaxEarning[8]) != Convert.ToDouble(strPaxEarning[13])) ||
                        //(Convert.ToDouble(strPaxEarning[9]) != Convert.ToDouble(strPaxEarning[14])) ||
                        (Convert.ToDouble(strPaxEarning[10]) > Convert.ToDouble(strPaxEarning[15])))
                    {
                        _Earning.DIFF_FLAG = "L";
                        _Earning.APPROVED_BY = strPaxEarning[16];
                    }
                    else
                    {
                        _Earning.DIFF_FLAG = "";
                        _Earning.APPROVED_BY = "";
                    }
                    _lstEarning.Add(_Earning);

                    xmlData += "<EARNINGS>";
                    xmlData += "<PAXNO>" + strPaxEarning[0] + "</PAXNO>";
                    xmlData += "<COMM_TYPE>" + strPaxEarning[1] + "</COMM_TYPE>";
                    xmlData += "<COMM_PERCENT>" + strPaxEarning[2] + "</COMM_PERCENT>";
                    xmlData += "<COMM_AMOUNT>" + strPaxEarning[3] + "</COMM_AMOUNT>";
                    xmlData += "<PLB_TYPE>" + strPaxEarning[4] + "</PLB_TYPE>";
                    xmlData += "<PLB_PERCENT>" + strPaxEarning[5] + "</PLB_PERCENT>";
                    xmlData += "<PLB_AMOUNT>" + strPaxEarning[6] + "</PLB_AMOUNT>";
                    xmlData += "<MARKUP_TYPE>" + strPaxEarning[7] + "</MARKUP_TYPE>";
                    xmlData += "<MARKUP_AMOUNT>" + strPaxEarning[8] + "</MARKUP_AMOUNT>";
                    xmlData += "<SERVICE_FEE>" + strPaxEarning[9] + "</SERVICE_FEE>";
                    xmlData += "<INCENTIVE>" + strPaxEarning[10] + "</INCENTIVE>";
                    xmlData += "<NEW_COMM>" + strPaxEarning[11] + "</NEW_COMM>";
                    xmlData += "<NEW_PLBAmount>" + strPaxEarning[12] + "</NEW_PLBAmount>";
                    xmlData += "<NEW_MARKUP>" + strPaxEarning[13] + "</NEW_MARKUP>";
                    xmlData += "<NEW_SERVICE_FEE>" + strPaxEarning[14] + "</NEW_SERVICE_FEE>";
                    xmlData += "<NEW_INCENTIVE>" + strPaxEarning[15] + "</NEW_INCENTIVE>";
                    xmlData += "</EARNINGS>";
                }
                xmlData += "</REQUEST>";

                DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details~Earnings", xmlData, strClientID, strPosTID, sequnceID);
                //
                //adding card details for Pass Through Start
                DataTable tbPassThrough = new DataTable("PAYMENT");
                tbPassThrough.Columns.Add("MODE");
                tbPassThrough.Columns.Add("CARDTYPE");
                tbPassThrough.Columns.Add("CARDNUMBER");
                tbPassThrough.Columns.Add("EXPIRYDATE");
                tbPassThrough.Columns.Add("CVVNUMBER");
                tbPassThrough.Columns.Add("CARDNAME");
                tbPassThrough.Columns.Add("CORPORATECODE");
                string cardType = string.Empty;
                string cardName = string.Empty;
                string cardNumber = string.Empty;
                string cardExpirymonth = string.Empty;
                string cardExpiryyear = string.Empty;
                string cardCVV = string.Empty;

                string[] cardvalues = cardGetSet.Split('*');

                #region  Airline  based payment options block

                //if (ConfigurationManager.AppSettings["Air_BasePaymtOptBlck"].ToString().ToUpper().Contains(dsPNRINFO.Tables["PassengerPNRDetails"].Rows[0]["PLATINGCARRIER"].ToString().ToUpper().Trim()) && cardvalues[0].ToUpper().Trim() == "B" && strTerminalType == "W")
                //{
                //    result[error] = "Credit Card issuance not permitted for this Airline.";
                //    return Json(new { Status = "0", Message = "", Result = result });
                //}

                #endregion

                if (cardGetSet.Trim() != "")
                {
                    bool BTAFlag = true;
                    //string[] cardvalues = cardGetSet.Split('*');
                    if (dsPNRINFO.Tables.Contains("AGENT_INFO"))
                    {
                        if (cardvalues[0] == "CA")
                        {
                            dsPNRINFO.Tables["AGENT_INFO"].Rows[0]["PAYMENTMODE"] = "C";
                        }
                        else if (cardvalues[0] == "T")
                        {
                            dsPNRINFO.Tables["AGENT_INFO"].Rows[0]["PAYMENTMODE"] = "T";
                        }
                    }
                    tbPassThrough.Rows.Add(cardvalues[0], cardType, cardNumber, cardExpirymonth + cardExpiryyear, cardCVV, cardName);
                }

                tbPassThrough.Rows[0]["CORPORATECODE"] = Corporatecode;
                if (!dsPNRINFO.Tables.Contains("PAYMENT"))
                {
                    dsPNRINFO.Tables.Add(tbPassThrough.Copy());
                }
                if (!dsPNRINFO.Tables["AGENT_INFO"].Columns.Contains("RePricingFlag"))
                {
                    dsPNRINFO.Tables["AGENT_INFO"].Columns.Add("RePricingFlag");
                    dsPNRINFO.Tables["AGENT_INFO"].Rows[0]["RePricingFlag"] = "N";
                }


                if (objPNR == null)
                {
                    result[error] = "Your session has expired!";
                    return Json(new { Status = "-1", Message = result[error], Result = result });
                }

                if (Session["agentid"] == null && Session["agentid"].ToString() == string.Empty)
                {
                    result[error] = "Your session has expired!";
                    return Json(new { Status = "-1", Message = result[error], Result = result });
                }
                //////start//////Ticket Request only selected TSTCOUNT
                DataTable paxdetails_new = new DataTable();
                DataTable pax_info_new = new DataTable();
                DataTable passenger_pnrinfo_new = new DataTable();


                if (StrFareFlag == "N")
                {
                    var linq_paxdetails_new = from p in dsPNRINFO.Tables["PAXDETAILS"].AsEnumerable()
                                              where TSTCOUNT.Contains(p["PLATINGCARRIER"].ToString())
                                              select p;

                    paxdetails_new = linq_paxdetails_new.CopyToDataTable<DataRow>();
                    paxdetails_new.TableName = "PAXDETAILS";


                    var linq_pax_info_new = from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                                            where TSTCOUNT.Contains(p["PLATINGCARRIER"].ToString())
                                            select p;

                    pax_info_new = linq_pax_info_new.CopyToDataTable();
                    pax_info_new.TableName = "PAX_INFO";
                    var linq_passenger_pnrinfo = from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                                 where TSTCOUNT.Contains(p["PLATINGCARRIER"].ToString())
                                                 select p;

                    passenger_pnrinfo_new = linq_passenger_pnrinfo.CopyToDataTable();
                    passenger_pnrinfo_new.TableName = "PassengerPNRDetails";
                }
                else
                {
                    var linq_paxdetails_new = from p in dsPNRINFO.Tables["PAXDETAILS"].AsEnumerable()
                                              where TSTCOUNT.Contains(p["TSTCOUNT"].ToString())
                                              select p;

                    paxdetails_new = linq_paxdetails_new.CopyToDataTable<DataRow>();
                    paxdetails_new.TableName = "PAXDETAILS";

                    var linq_pax_info_new = from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                                            where TSTCOUNT.Contains(p["TSTCOUNT"].ToString())
                                            select p;

                    pax_info_new = linq_pax_info_new.CopyToDataTable();
                    pax_info_new.TableName = "PAX_INFO";
                    var linq_passenger_pnrinfo = from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                                 where TSTCOUNT.Contains(p["TSTCOUNT"].ToString())
                                                 select p;

                    passenger_pnrinfo_new = linq_passenger_pnrinfo.CopyToDataTable();
                    passenger_pnrinfo_new.TableName = "PassengerPNRDetails";
                }

                dsPNRINFO.Tables.Remove("PAXDETAILS");
                dsPNRINFO.Tables.Remove("PAX_INFO");
                dsPNRINFO.Tables.Remove("PassengerPNRDetails");

                dsPNRINFO.Tables.Add(paxdetails_new);
                dsPNRINFO.Tables.Add(pax_info_new);
                dsPNRINFO.Tables.Add(passenger_pnrinfo_new);

                ////end/////////Ticket Request only selected TSTCOUNT 
                /////start////Changing the "title empty" table with given title

                if (missing_title_select.Trim() != "")
                {
                    string[] missing_title = missing_title_select.Split(',');

                    DataTable pax_info = new DataTable("PAX_INFO");
                    var pax_info_var = from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                                           //orderby p["PAXTYPE"] ascending
                                       select p;

                    pax_info = pax_info_var.CopyToDataTable();
                    int i = 0;

                    foreach (DataRow dr in pax_info.Rows)
                    {
                        if (missing_title[i] == "") i = 0;
                        string title = dr["TITLE"].ToString().ToUpper().Trim();
                        if (title == "" && title != "DR" && title != "MR" && title != "MS" && title != "MRS" && title != "MISS" && title != "MSTR")
                        {
                            dr["TITLE"] = missing_title[i];
                            i++;
                        }
                    }
                    dsPNRINFO.Tables.Remove("PAX_INFO");
                    pax_info.TableName = "PAX_INFO";
                    dsPNRINFO.Tables.Add(pax_info.Copy());
                }

                /////end////Changing the "title empty" table with given title
                str_pnrinfo = JsonConvert.SerializeObject(dsPNRINFO);
                dsPNRReq = _ServiceUtility.convertJsonStringToDataSet(str_pnrinfo, "");
                strSegmentType = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["AIRPORT_TYPE"].ToString().ToUpper().Trim();
                strSupplier = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["CRS_ID"].ToString().ToUpper().Trim();
                string PaymentMode = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["PAYMENTMODE"].ToString().ToUpper().Trim();
                TicketingOfficeId = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["RETRIEVEPCC"].ToString().ToUpper().Trim();
                RetrivePCC = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["RETRIEVEPCC"].ToString().ToUpper().Trim();
                //decimal dcTotalFare = Convert.ToDecimal(dsPNRReq.Tables["AGENT_INFO"].Rows[0]["TOTALFARE"].ToString().ToUpper().Trim());            
                decimal dcTotalFare = 0;

                if (dsPNRReq.Tables.Contains("PAXDETAILS_OLD_INFO") && RebookFlag == true)
                {
                    foreach (DataRow drRows in dsPNRReq.Tables["PAXDETAILS_OLD_INFO"].Rows)
                    {
                        dcTotalFare += Convert.ToDecimal(drRows["GROSSFARE"].ToString());

                    }
                }
                else
                {
                    foreach (DataRow drRows in dsPNRReq.Tables["PAXDETAILS"].Rows)
                    {
                        dcTotalFare += Convert.ToDecimal(drRows["GROSSFARE"].ToString());
                    }
                }
                RBDCLASS = string.Empty;

                string paxrefence = dsPNRReq.Tables["PassengerPNRDetails"].Rows[0]["PAXREFERENCE"].ToString();
                var results = from myRow in dsPNRReq.Tables["PassengerPNRDetails"].AsEnumerable()
                              where myRow["PAXREFERENCE"].ToString().Trim() == paxrefence.ToString().Trim()
                              select myRow;
                DataTable dtPriceSegment = results.CopyToDataTable();
                foreach (DataRow dsrowsPAX in dtPriceSegment.Rows)
                {
                    RBDCLASS += dsrowsPAX["CLASS"].ToString() + ",";
                }

                //****************************EXTRA PARAMETER**********************************
                string minSegmentId = dsPNRReq.Tables["PassengerPNRDetails"].AsEnumerable()
              .Min(r => r.Field<string>("SEGMENTNO"));

                string maxSegmentId = dsPNRReq.Tables["PassengerPNRDetails"].AsEnumerable()
                                  .Max(r => r.Field<string>("SEGMENTNO"));



                var qryMin = (from p in dsPNRReq.Tables["PassengerPNRDetails"].AsEnumerable()
                              where p.Field<string>("SEGMENTNO").ToString().Trim() == minSegmentId.ToString().Trim()
                              select new
                              {
                                  DEPARTUREDATE = p.Field<string>("DEPARTUREDATE"),
                                  ORIGIN = p.Field<string>("ORIGIN")
                              }).Distinct();
                if (qryMin != null && qryMin.Count() > 0)
                {
                    DataTable dtMin = ConvertToDataTable(qryMin);
                    if (dtMin != null && dtMin.Rows.Count > 0 && dtMin.Columns.Contains("DEPARTUREDATE"))
                        travelFromDate = dtMin.Rows[0]["DEPARTUREDATE"].ToString();
                    travelFromDate = DateTime.ParseExact(travelFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    if (dtMin != null && dtMin.Rows.Count > 0 && dtMin.Columns.Contains("ORIGIN"))
                        travelOrigin = dtMin.Rows[0]["ORIGIN"].ToString();
                }

                var qryMax = (from p in dsPNRReq.Tables["PassengerPNRDetails"].AsEnumerable()
                              where p.Field<string>("SEGMENTNO").ToString().Trim() == maxSegmentId.ToString().Trim()
                              select new
                              {
                                  ARRIVALDATE = p.Field<string>("ARRIVALDATE"),
                                  DESTINATION = p.Field<string>("DESTINATION")

                              }).Distinct();
                if (qryMax != null && qryMax.Count() > 0)
                {
                    DataTable dtMax = ConvertToDataTable(qryMax);
                    if (dtMax != null && dtMax.Rows.Count > 0 && dtMax.Columns.Contains("ARRIVALDATE"))
                        travelToDate = dtMax.Rows[0]["ARRIVALDATE"].ToString();
                    travelToDate = DateTime.ParseExact(travelToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    if (dtMax != null && dtMax.Rows.Count > 0 && dtMax.Columns.Contains("DESTINATION"))
                        travelDestination = dtMax.Rows[0]["DESTINATION"].ToString();

                }

                try
                {

                    if (travelOrigin.ToUpper().Trim() == travelDestination.ToUpper().Trim())
                    {

                        string MaxSegmentId = dsPNRReq.Tables["PassengerPNRDetails"].AsEnumerable()
                                                            .Max(r => r["SEGMENTNO"].ToString());

                        var qryActualData = (from p in dsPNRReq.Tables["PassengerPNRDetails"].AsEnumerable()
                                             orderby p["SEGMENTNO"] ascending
                                             select new
                                             {
                                                 SEG_NO = p["SEGMENTNO"],
                                                 ORG_DEPT_DATE = p["DEPARTUREDATE"],
                                                 DES_ARRIVAL_DATE = p["ARRIVALDATE"],
                                                 ORG_DEPT_TIME = p["DEPARTURETIME"],
                                                 DES_ARRIVAL_TIME = p["ARRIVALTIME"],
                                                 ORG_SEG = p["ORIGIN"],
                                                 DES_SEG = p["DESTINATION"],
                                                 LOGIC_DATE_RANGE = "",
                                                 LOGIC_TIME_RANGE = ""
                                             }).Distinct();

                        if (qryActualData != null)
                            dtActualData = ConvertToDataTable(qryActualData);


                        if (dtActualData.Rows.Count > 0)
                        {

                            strBaseOrgin = dtActualData.Rows[0]["ORG_SEG"].ToString();
                            strBaseDeptDate = dtActualData.Rows[0]["ORG_DEPT_TIME"].ToString();
                            if (MaxSegmentId == "1")
                            {
                                strbaseDestination = dtActualData.Rows[0]["DES_SEG"].ToString();
                                strLastArrivalDate = dtActualData.Rows[0]["DES_ARRIVAL_TIME"].ToString();
                            }
                            if (MaxSegmentId == "2")
                            {
                                strbaseDestination = dtActualData.Rows[1]["DES_SEG"].ToString();
                                strLastArrivalDate = dtActualData.Rows[1]["DES_ARRIVAL_TIME"].ToString();
                            }
                            else
                            {
                                if (dtActualData.Rows.Count > 0)
                                {
                                    for (int Rowcnt = 0; Rowcnt < dtActualData.Rows.Count; Rowcnt++)
                                    {
                                        string SecondDeptDate = string.Empty;
                                        if (Rowcnt != dtActualData.Rows.Count - 1)
                                        {
                                            string FirstArrDate = DateTime.ParseExact(dtActualData.Rows[Rowcnt]["ORG_DEPT_DATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

                                            SecondDeptDate = DateTime.ParseExact(dtActualData.Rows[Rowcnt + 1]["DES_ARRIVAL_DATE"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

                                            dtActualData.Rows[Rowcnt]["LOGIC_DATE_RANGE"] = Convert.ToDecimal(SecondDeptDate) - Convert.ToDecimal(FirstArrDate);
                                        }
                                        else
                                            dtActualData.Rows[Rowcnt]["LOGIC_DATE_RANGE"] = "";
                                    }

                                    MaxSegmentId = dtActualData.AsEnumerable()
                                                        .Max(r => r["LOGIC_DATE_RANGE"].ToString());

                                    var qryDateFinal = (from p in dtActualData.AsEnumerable()
                                                        where p["LOGIC_DATE_RANGE"].ToString() == MaxSegmentId
                                                        orderby p["SEG_NO"] ascending
                                                        select new
                                                        {
                                                            SEG_NO = p["SEG_NO"],
                                                            DES_SEG = p["DES_SEG"],
                                                            LOGIC_DATE_RANGE = p["LOGIC_DATE_RANGE"]
                                                        }).Distinct();
                                    if (qryDateFinal != null)
                                    {
                                        dtFinal = ConvertToDataTable(qryDateFinal);
                                        strbaseDestination = dtFinal.Rows[0]["DES_SEG"].ToString();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        strBaseOrgin = travelOrigin;
                        strbaseDestination = travelDestination;
                    }
                }
                catch (Exception ex)
                {
                    string ErrorMsg = ex.ToString();
                    DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "X", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", ErrorMsg, strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                    strBaseOrgin = travelOrigin;
                    strbaseDestination = travelDestination;
                }

                //if (PaymentMode == "C")
                //{
                //    string strBalance = string.Empty;
                //    string Balance = string.Empty;
                //    // Fetch_CurrentBalance(agentID, userName, ipAddress, terminalType, sequenceID, PaymentMode, "AIR", ref strErrorMsg, ref strBalance, ref Balance);

                //    if (string.IsNullOrEmpty(Balance.ToString().Trim())) Balance = "0";
                //    if (Convert.ToDecimal(Balance) < dcTotalFare)
                //    {
                //        //ClearFlag = false;
                //        //strErrorMsg = "Your current balance is not sufficient to book the ticket.Please increase your balance and continue";
                //        //result[error] = strErrorMsg;
                //        //return result;
                //    }
                //}
                //****************************END EXTRA PARAMETER**********************************
                // TrackID generation
                string terminalType = "W";
                if (terminalType != "T")
                {
                    //if (RebookFlag == true)
                    //{
                    //    //trackStatus = Insert_UserTrackID(agentID, userName,
                    //    //              "RE-TICKET", strErrorMsg, dsPNRReq.Tables["AGENT_INFO"].Rows[0]["PARENT_TRACKID"].ToString().Trim(), "UPDATE", terminalType.ToString(), ipAddress.ToString(),
                    //    //              Convert.ToDecimal(sequenceID), "", ref strErrorMsg, ref strTrackIds, ds, 0);
                    //}

                    //DateTime dtToday = DateTime.Now;
                    ////string prefix = agentID.ToString().Trim().ToUpper() + dtToday.Day.ToString() + dtToday.Month.ToString() + dtToday.Year.ToString() + "T";
                    //string prefix = "";

                    //if (dsPNRReq.Tables["AGENT_INFO"].Columns.Contains("PARENT_TRACKID") && !string.IsNullOrEmpty(dsPNRReq.Tables["AGENT_INFO"].Rows[0]["PARENT_TRACKID"].ToString().Trim()))
                    //    prefix += "|" + dsPNRReq.Tables["AGENT_INFO"].Rows[0]["PARENT_TRACKID"].ToString().Trim();

                    //bool returnStatus = Insert_UserTrackID(agentID, userName, "IN PROCESS", "",
                    //     prefix.ToString(), "INSERT", terminalType, ipAddress, sequenceID, PaymentMode, ref strErrorMsg,
                    //     ref trackIDStatus, dsPNRReq, dcTotalFare);

                    //if (string.IsNullOrEmpty(trackIDStatus))
                    //{
                    //    //strErrorMsg = "Problem occurred while generate the track ID";
                    //    //result[error] = strErrorMsg;
                    //    //return result;
                    //}

                    if (!dsPNRReq.Tables["PAXDETAILS"].Columns.Contains("TRACKID"))
                        dsPNRReq.Tables["PAXDETAILS"].Columns.Add("TRACKID");
                    dsPNRReq.Tables["PAXDETAILS"].Rows[0]["TRACKID"] = trackIDStatus;

                    var QryAll = (from p in dsPNRReq.Tables["PassengerPNRDetails"].AsEnumerable()// For already Ticketed TST accounting purpose
                                  where p.Field<string>("TICKETNO").ToString() != ""
                                  select p);

                    DataView dvAllFare = QryAll.AsDataView();
                    if (dvAllFare.Count > 0)
                    {
                        string crsPNR = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["CRS_PNR"].ToString().ToUpper().Trim();

                        DataTable dtTcktPNRDetails = QryAll.CopyToDataTable();
                        //bool PNRExist = Check_and_fetch_queuePNR(crsPNR, agentID, userName, ipAddress,
                        //       terminalType, sequenceID, ref strErrorMsg, ref dtTcktPNRDetails, ref dtTicketedTST);

                        if (dtTicketedTST != null && dtTicketedTST.Rows.Count > 0)
                        {
                            dtTicketedTST.TableName = "PASSENGER_TICKETINFO";
                            if (!dtTicketedTST.Columns.Contains("USERTRACKID"))
                                dtTicketedTST.Columns.Add("USERTRACKID");
                            foreach (DataRow dr in dtTicketedTST.Rows)
                            {
                                dr["USERTRACKID"] = trackIDStatus;
                            }

                            if (StrFareFlag == "N")
                            {
                                var qryTST = (from p in dtTicketedTST.AsEnumerable()
                                              select new
                                              {
                                                  TSTCOUNT = p.Field<string>("PLATINGCARRIER")
                                              }).Distinct();
                                if (qryTST.Count() > 0)
                                {
                                    ResultCode.TableName = "Result";
                                    ResultCode.Columns.Add("CODE");
                                    ResultCode.Columns.Add("TSTCOUNT");
                                    DataTable dtTST3 = ConvertToDataTable(qryTST);
                                    foreach (DataRow drRows in dtTST3.Rows)
                                    {
                                        ResultCode.Rows.Add("1", drRows["TSTCOUNT"]);
                                    }
                                }
                            }
                            else
                            {
                                var qryTST = (from p in dtTicketedTST.AsEnumerable()
                                              select new
                                              {
                                                  TSTCOUNT = p.Field<string>("TSTCOUNT")
                                              }).Distinct();
                                if (qryTST.Count() > 0)
                                {
                                    ResultCode.TableName = "Result";
                                    ResultCode.Columns.Add("CODE");
                                    ResultCode.Columns.Add("TSTCOUNT");
                                    DataTable dtTST3 = ConvertToDataTable(qryTST);
                                    foreach (DataRow drRows in dtTST3.Rows)
                                    {
                                        ResultCode.Rows.Add("1", drRows["TSTCOUNT"]);
                                    }
                                }
                            }
                        }
                    }

                    QryAll = (from p in dsPNRReq.Tables["PassengerPNRDetails"].AsEnumerable()
                              where p.Field<string>("TICKETNO").ToString() == ""
                              select p);
                    dvAllFare = QryAll.AsDataView();
                    if (dvAllFare.Count > 0)
                    {
                        dtTicketReq = QryAll.CopyToDataTable();
                    }
                }
                else
                {
                    trackIDStatus = dsPNRReq.Tables["PAXDETAILS"].Rows[0]["TRACKID"].ToString();
                    dtTicketReq = dsPNRReq.Tables["PassengerPNRDetails"].Copy();
                }

                #region //LIVE
                //Generate REQ for SWS for ticketing
                if (dtTicketReq != null && dtTicketReq.Rows.Count > 0)
                {
                    DataTable dtAgentDetails = new DataTable("AGENTDETAILS");
                    DataTable dtPNR = new DataTable("PNR");

                    dtAgentDetails.Columns.Add("AGENTID");
                    dtAgentDetails.Columns.Add("TERMINALID");
                    dtAgentDetails.Columns.Add("BRANCHID");
                    dtAgentDetails.Columns.Add("HEADERBRANCHID");
                    dtAgentDetails.Columns.Add("AIRPORT");
                    dtAgentDetails.Columns.Add("RePricingFlag");  // Changes by vimal 24102016  - ticketing Add Columns RePricingFlag

                    dtPNR.Columns.Add("CRSPNR");
                    dtPNR.Columns.Add("TRACKID");
                    dtPNR.Columns.Add("CRSID");
                    dtPNR.Columns.Add("ORIGIN");
                    dtPNR.Columns.Add("DESTINATION");
                    dtPNR.Columns.Add("TSTREFERENCE");
                    dtPNR.Columns.Add("SEGMENTREFERENCE");
                    dtPNR.Columns.Add("PAXREFERENCE");
                    dtPNR.Columns.Add("PLATINGCARRIER");
                    dtPNR.Columns.Add("FAREBASISCODE");
                    dtPNR.Columns.Add("TSTCOUNT");
                    dtPNR.Columns.Add("BOOKINGAMOUNT");
                    dtPNR.Columns.Add("CORPORATECODE");
                    dtPNR.Columns.Add("QUEUENUMBER");

                    string StrRePricingFlag = string.Empty;

                    if (dsPNRReq.Tables["AGENT_INFO"].Columns.Contains("RePricingFlag"))
                    {
                        StrRePricingFlag = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["RePricingFlag"].ToString();
                    }
                    else
                    {
                        StrRePricingFlag = "N";
                    }

                    dtAgentDetails.Rows.Add(strAgentID, strUserName,
                        dsPNRReq.Tables["AGENT_INFO"].Rows[0]["BRANCHID"].ToString(),
                        dsPNRReq.Tables["AGENT_INFO"].Rows[0]["HEADER_BRANCHID"].ToString(),
                        dsPNRReq.Tables["AGENT_INFO"].Rows[0]["AIRPORT_TYPE"].ToString(),
                        StrRePricingFlag);

                    BookingBranchID = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["BRANCHID"].ToString();

                    #region Booking Branch ID Changes

                    //DataSet dsBranchID = new DataSet();
                    //Hashtable my_param = new Hashtable();
                    //my_param.Add("AIRLINECODE", dsPNRReq.Tables["PassengerPNRDetails"].Rows[0]["TICKETINGCARRIER"].ToString().Trim());
                    //my_param.Add("ORIGIN", strBaseOrgin.ToString().Trim().ToUpper());
                    //my_param.Add("DESTINATION", strbaseDestination.ToString().Trim().ToUpper());
                    //my_param.Add("CRSCODE", dsPNRReq.Tables["AGENT_INFO"].Rows[0]["CRS_ID"].ToString().Trim().ToUpper());
                    //my_param.Add("AGENTTYPE", dsPNRReq.Tables["AGENT_INFO"].Rows[0]["AGENT_TYPE"].ToString().Trim().ToUpper());
                    //my_param.Add("RBD", RBDCLASS.ToString().ToUpper().TrimEnd(','));   // #STS105 ON 20170812
                    //my_param.Add("BASEBRANCH", dsPNRReq.Tables["AGENT_INFO"].Rows[0]["BRANCHID"].ToString().Trim().ToUpper());
                    //my_param.Add("AGENTID", dsPNRReq.Tables["PAXDETAILS"].Rows[0]["AGENTID"].ToString().Trim().ToUpper());
                    //my_param.Add("DEPARTURE", travelFromDate.ToString().Trim().ToUpper());
                    //my_param.Add("RETURNDATE", travelToDate.ToString().Trim().ToUpper());
                    //my_param.Add("AIRPORTTYPE", dsPNRReq.Tables["AGENT_INFO"].Rows[0]["SEGMENTTYPE"].ToString().Trim().ToUpper());
                    //my_param.Add("PAYMENTTYPE", PaymentMode);
                    //my_param.Add("FARETYPE", farequalifier);

                    //#region Request Log
                    //string XMLDATA = "";
                    //XMLDATA = "<EVENT><REQUEST>FETCHTICKETINGBRANCH</REQUEST>"
                    //           + "<AGENTID>" + strAgentID.ToString().Trim().ToUpper() + "</AGENTID>"
                    //            + "<AIRLINECODE>" + dsPNRReq.Tables["PassengerPNRDetails"].Rows[0]["TICKETINGCARRIER"].ToString().Trim() + "</AIRLINECODE>"
                    //            + "<CRSCODE>" + dsPNRReq.Tables["AGENT_INFO"].Rows[0]["CRS_ID"].ToString().Trim().ToUpper() + "</CRSCODE>"
                    //            + "<AGENTTYPE>" + dsPNRReq.Tables["AGENT_INFO"].Rows[0]["AGENT_TYPE"].ToString().Trim().ToUpper() + "</AGENTTYPE>"
                    //            + "<RBD>" + RBDCLASS.ToString().ToUpper().TrimEnd(',') + "</RBD>"
                    //            + "<BASEBRANCH>" + dsPNRReq.Tables["AGENT_INFO"].Rows[0]["BRANCHID"].ToString().Trim().ToUpper() + "</BASEBRANCH>"
                    //            + "<DEPARTURE>" + travelFromDate.ToString().Trim() + "</DEPARTURE>"
                    //            + "<RETURNDATE>" + travelToDate.ToString().Trim() + "</RETURNDATE>"
                    //            + "<ORIGIN>" + strBaseOrgin.ToString().Trim() + "</ORIGIN>"
                    //            + "<DESTINATION>" + strbaseDestination.ToString().Trim() + "</DESTINATION>"
                    //            + "<AIRPORTTYPE>" + dsPNRReq.Tables["AGENT_INFO"].Rows[0]["SEGMENTTYPE"].ToString().Trim().ToUpper() + "</AIRPORTTYPE>"
                    //            + "<PAYMENTTYPE></PAYMENTTYPE>"
                    //            + "<FARETYPE></FARETYPE>"
                    //            + "</EVENT>";

                    //DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", XMLDATA.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");

                    //#endregion

                    //bool BranchStatus = false;
                    //// BranchStatus = DBHandler.dsResultProcedure(SelectProcedure.NEW_BOOKING_BRANCH, "P", my_param, ref dsBranchID, ref strErrorMsg);
                    ////*************END NEW PARAMETERS**************************************

                    //#region RES LOG
                    //xmlData = string.Empty;
                    //xmlData = "<EVENT>" + "<REQUEST>BRANCHID_CHANGES</REQUEST>" +
                    //       "<TKTBRANCHID>" + dsBranchID.GetXml() + "</TKTBRANCHID>" +
                    //       "</EVENT>";

                    //DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");

                    //#endregion

                    //DataTable dtBranch = new DataTable();
                    //if (BranchStatus == true && dsBranchID != null && dsBranchID.Tables.Count > 0 && dsBranchID.Tables[0].Rows.Count > 0)
                    //{
                    //    dtBranch = dsBranchID.Tables[0].Copy();
                    //    if (dtBranch != null && dtBranch.Rows.Count > 0)
                    //    {
                    //        #region New methode   #STS105 COM ON 20170729

                    //        xmlData = string.Empty;
                    //        xmlData = "<EVENT>" + "<REQUEST>BRANCHID_CHANGES</REQUEST>" +
                    //               "<TKTBRANCHID>" + dtBranch.Rows[0]["TKB_TICKETING_BRANCH_ID"].ToString().Trim() + "</TKTBRANCHID>" +
                    //               "<OFFICEID>" + dtBranch.Rows[0]["TKB_TICKETING_OFFICE_ID"].ToString().Trim() + "</OFFICEID>" +
                    //               "</EVENT>";
                    //        DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");

                    //        BookingBranchID = dtBranch.Rows[0]["TKB_TICKETING_BRANCH_ID"].ToString().Trim();
                    //        TicketingOfficeId = dtBranch.Rows[0]["TKB_TICKETING_OFFICE_ID"].ToString().Trim();

                    //        #endregion
                    //    }
                    //}

                    #endregion

                    Session["TKTFLAG"] = "QTKT";//DTKT//QTKT

                    if (StrFareFlag == "N")
                    {
                        var qryTST = (from p in dtTicketReq.AsEnumerable()
                                      select new
                                      {
                                          TSTCOUNT = p.Field<string>("PLATINGCARRIER")
                                      }).Distinct();
                        if (qryTST.Count() > 0)
                        {
                            DataTable dtTST2 = ConvertToDataTable(qryTST);
                            foreach (DataRow drRows in dtTST2.Rows)
                            {
                                dsTicketingPNRReq = new DataSet();
                                dtPNR = dtPNR.Clone();
                                var QrySegment = (from p in dtTicketReq.AsEnumerable()
                                                  where p.Field<string>("PLATINGCARRIER").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim()
                                                  select new
                                                  {
                                                      ORIGIN = p.Field<string>("ORIGIN").Trim(),
                                                      DESTINATION = p.Field<string>("DESTINATION").Trim(),
                                                      TSTREFERENCE = p.Field<string>("TSTREFERENCE").Trim(),
                                                      SEGMENTREFERENCE = p.Field<string>("SEGMENTREFERENCE").Trim(),
                                                      PAXREFERENCE = p.Field<string>("PAXREFERENCE").Trim(),
                                                      PLATINGCARRIER = p.Field<string>("PLATINGCARRIER").Trim(),
                                                      FAREBASISCODE = p.Field<string>("FAREBASISCODE").Trim(),
                                                      TSTCOUNT = p.Field<string>("TSTCOUNT").Trim(),
                                                      TOURCODE = p.Field<string>("TOURCODE").Trim(),

                                                  }).Distinct();
                                DataTable dtTSTSegment = ConvertToDataTable(QrySegment);

                                var QryAPIFare = (from p in dsPNRReq.Tables["PAXDETAILS"].AsEnumerable()
                                                  where p.Field<string>("PLATINGCARRIER").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim()
                                                  select new
                                                  {
                                                      API_GROSSFARE = p.Field<string>("API_GROSSFARE")
                                                  }).Distinct();
                                DataTable dtAPITSTFare = ConvertToDataTable(QryAPIFare);

                                string CorporateCode = string.Empty;
                                CorporateCode = dsPNRReq.Tables["PAYMENT"].Rows[0]["CORPORATECODE"].ToString();

                                foreach (DataRow drTSTSegmentRows in dtTSTSegment.Rows)
                                {
                                    dtPNR.Rows.Add(dsPNRReq.Tables["PassengerPNRDetails"].Rows[0]["CRSPNR"].ToString(),
                                    dsPNRReq.Tables["PAXDETAILS"].Rows[0]["TRACKID"].ToString(),
                                    dsPNRReq.Tables["AGENT_INFO"].Rows[0]["CRS_ID"].ToString(), drTSTSegmentRows["ORIGIN"],
                                    drTSTSegmentRows["DESTINATION"], drTSTSegmentRows["TSTREFERENCE"], drTSTSegmentRows["SEGMENTREFERENCE"],
                                    drTSTSegmentRows["PAXREFERENCE"], drTSTSegmentRows["PLATINGCARRIER"], drTSTSegmentRows["FAREBASISCODE"],
                                    drTSTSegmentRows["TSTCOUNT"], dtAPITSTFare.Rows[0]["API_GROSSFARE"], CorporateCode, dsPNRReq.Tables["AGENT_INFO"].Rows[0]["QUEUENUMBER"].ToString());
                                }

                                dsTicketingPNRReq.Tables.Add(dtAgentDetails.Copy());
                                dsTicketingPNRReq.Tables.Add(dtPNR.Copy());
                                if (dsPNRReq.Tables.Contains("PAYMENT"))
                                    dsTicketingPNRReq.Tables.Add(dsPNRReq.Tables["PAYMENT"].Copy());


                                strwrt = new StringWriter();
                                dsTicketingPNRReq.WriteXml(strwrt);

                                var Qrypnrdetails = (from p in dsPNRReq.Tables["PassengerPNRDetails"].AsEnumerable()
                                                     where p.Field<string>("PLATINGCARRIER").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim()
                                                     select p).Distinct();
                                DataTable dtTSTPnrdetails = Qrypnrdetails.CopyToDataTable();

                                foreach (DataRow drTSTSegmentRows in dtTSTSegment.Rows)
                                {
                                    _pnrdetail = new PNRDetails();
                                    _pnrdetail.CRSPNR = dsPNRReq.Tables["PassengerPNRDetails"].Rows[0]["CRSPNR"].ToString();
                                    _pnrdetail.CRSID = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["CRS_ID"].ToString();
                                    _pnrdetail.CORPORATECODE = dsPNRReq.Tables["PAYMENT"].Rows[0]["CORPORATECODE"].ToString();
                                    _pnrdetail.TOURCODE = (strPricingcode != null && strPricingcode != "") ? strPricingcode : drTSTSegmentRows["TOURCODE"].ToString();
                                    //_pnrdetail.BOOKINGAMOUNT = dtAPITSTFare.Rows[0]["API_GROSSFARE"].ToString();fareQualifier
                                    _pnrdetail.BOOKINGAMOUNT = WithoutTaxBookingAMount.ToString();
                                    _pnrdetail.QUEUENUMBER = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["QUEUENUMBER"].ToString();
                                    _pnrdetail.ADULT = dsPNRReq.Tables["PAXDETAILS"].Rows[0]["ADULTCOUNT"].ToString();
                                    _pnrdetail.CHILD = dsPNRReq.Tables["PAXDETAILS"].Rows[0]["CHILDCOUNT"].ToString();
                                    _pnrdetail.INFANT = dsPNRReq.Tables["PAXDETAILS"].Rows[0]["INFANTCOUNT"].ToString();
                                    _pnrdetail.SEGMENTTYPE = strSegmentType;
                                    _pnrdetail.FAREQUALIFIER = farequalifier;
                                    _pnrdetail.OBCTax = OBCTAX;
                                    _pnrdetail.MARKUP = Markup;
                                    _pnrdetail.EARNINGS = Commission;
                                    _pnrdetail.PLBAmount = PLB;
                                    _pnrdetail.Endorsement = strEndorsement;
                                }

                                foreach (DataRow drTSTpnrRows in dtTSTPnrdetails.Rows)
                                {
                                    _tkt_pnrdetails = new TicketPnrDetail();
                                    _tkt_pnrdetails.AIRLINEPNR = drTSTpnrRows["AIRLINEPNR"].ToString();
                                    _tkt_pnrdetails.CARRIERCODE = drTSTpnrRows["AIRLINECODE"].ToString();
                                    _tkt_pnrdetails.FLIGHTNO = drTSTpnrRows["FLIGHTNO"].ToString();
                                    _tkt_pnrdetails.ORIGIN = drTSTpnrRows["ORIGIN"].ToString();
                                    _tkt_pnrdetails.DESTINATION = drTSTpnrRows["DESTINATION"].ToString();
                                    _tkt_pnrdetails.DEPARTUREDATE = drTSTpnrRows["DEPARTUREDATE"].ToString();
                                    _tkt_pnrdetails.ARRIVALDATE = drTSTpnrRows["ARRIVALDATE"].ToString();
                                    _tkt_pnrdetails.DEPARTURETIME = drTSTpnrRows["DEPARTURETIME"].ToString();
                                    _tkt_pnrdetails.ARRIVALTIME = drTSTpnrRows["ARRIVALTIME"].ToString();
                                    _tkt_pnrdetails.CLASS = drTSTpnrRows["CLASS"].ToString();
                                    _tkt_pnrdetails.FAREBASISCODE = drTSTpnrRows["FAREBASISCODE"].ToString();
                                    //_tkt_pnrdetails.TSTREFERENCE = drTSTpnrRows["TSTREFERENCE"].ToString();tstReference
                                    _tkt_pnrdetails.TSTREFERENCE = tstReference;
                                    _tkt_pnrdetails.SEGMENTREFERENCE = drTSTpnrRows["SEGMENTREFERENCE"].ToString();
                                    _tkt_pnrdetails.PAXREFERENCE = drTSTpnrRows["PAXREFERENCE"].ToString();
                                    _tkt_pnrdetails.PLATINGCARRIER = ((strPlatingcarrier != null && strPlatingcarrier != "") ? strPlatingcarrier : drTSTpnrRows["TICKETINGCARRIER"].ToString());
                                    _tkt_pnrdetails.TSTCOUNT = drTSTpnrRows["TSTCOUNT"].ToString();
                                    _tkt_pnrdetails.PAXNO = drTSTpnrRows["PAXNO"].ToString();
                                    _tkt_pnrdetails.SEGMENTNO = drTSTpnrRows["SEGMENTNO"].ToString();

                                    //_tkt_pnrdetails.PLATINGCARRIER
                                    strPLATINGCARRIER = drTSTpnrRows["PLATINGCARRIER"].ToString();
                                    _lstpnrdet.Add(_tkt_pnrdetails);

                                }
                            }

                            if (ERPDetails != null && ERPDetails != "")
                            {
                                string[] _strERPDeatils = ERPDetails.TrimEnd('~').Split('~');

                                for (int i = 0; i < _strERPDeatils.Count(); i++)
                                {
                                    _ERP_Attribute = new ERP_Attribute();
                                    string[] lstarray = _strERPDeatils[i].Split('|');
                                    _ERP_Attribute.AttributesName = "ERP_ATR_COL" + lstarray[0];
                                    _ERP_Attribute.AttributesValue = lstarray[2];
                                    _LST_ERP_Attribute.Add(_ERP_Attribute);
                                }
                            }

                            string[] _strERPDeatils_TICK = OtherTicketInfo.Split('|');

                            _CBT_Credentials.ApprovedBy = "";
                            _CBT_Credentials.Chargeabilty = "";
                            _CBT_Credentials.Department = "";
                            _CBT_Credentials.CustLocation = "";
                            _CBT_Credentials.EmpCode = "";
                            _CBT_Credentials.PersonalBooking = "";
                            _CBT_Credentials.ProjectCode = "";
                            _CBT_Credentials.TRNumber = "";
                            _CBT_Credentials.Budgetcode = "";
                            _CBT_Credentials.VesselName = "";
                            _CBT_Credentials.CostCenter = EmpCostCenter;
                            _CBT_Credentials.CorpEmpCode = "";
                            _CBT_Credentials.ERP_Attributes = _LST_ERP_Attribute;

                            TicketingOfficeId = SessionKey;
                            //***************************Request in RQRS formatt**********************************
                            //
                            string strClientTerminalID = string.Empty;
                            string ClientID = string.Empty;
                            if (strTerminalType == "T" && strClientID != "")
                            {
                                ClientID = strClientID;
                                strClientTerminalID = strClientID + "01";
                            }
                            else
                            {
                                ClientID = strPosID;
                                strClientTerminalID = strPosTID;
                            }
                            //
                            _Agentdetail.AgentID = strAgentID;
                            _Agentdetail.TerminalID = strTerminalId;
                            _Agentdetail.AppType = "B2B";
                            _Agentdetail.UserName = strUserName;
                            _Agentdetail.BranchID = BranchID;// strBranchId;
                            ////_Agentdetail.BranchID = "38";
                            //_Agentdetail.BranchID = BookingBranchID;//change while putting live 
                            _Agentdetail.ProductType = "RC";
                            _Agentdetail.PNROfficeId = RetrivePCC;
                            _Agentdetail.TicketingOfficeId = TicketingOfficeId;
                            _Agentdetail.ClientID = Corporatename;//
                            _Agentdetail.Version = "";
                            //_Agentdetail.BOAID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                            //_Agentdetail.BOATerminalID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                            _Agentdetail.BOAID = ClientID;
                            _Agentdetail.BOATerminalID = strClientTerminalID;
                            _Agentdetail.AgentType = "";
                            _Agentdetail.CoOrdinatorID = "";
                            _Agentdetail.IssuingBranchID = BranchID;
                            _Agentdetail.EMP_ID = Employeename;
                            _Agentdetail.COST_CENTER = EmpCostCenter;
                            _Agentdetail.Ipaddress = Ipaddress;
                            _Agentdetail.Environment = (strTerminalType == "T" ? "I" : "W");
                            _Agentdetail.Platform = "B";
                            _Agentdetail.PNROfficeId = SessionKey;

                            GSTDETAILS _gstdetails = new GSTDETAILS();

                            if (GSTDetails != "")
                            {
                                string[] _gstLst = GSTDetails.Split('|');
                                _gstdetails.GSTAddress = _gstLst[3].ToString();
                                _gstdetails.GSTCompanyName = _gstLst[2].ToString();
                                _gstdetails.GSTEmailID = _gstLst[4].ToString();
                                _gstdetails.GSTMobileNumber = _gstLst[5].ToString();
                                _gstdetails.GSTNumber = _gstLst[1].ToString();

                                if (CRSTYPE.ToUpper().Trim() == "1G")
                                {
                                    _gstdetails.GSTStateCode = _gstLst[6].ToString();
                                    _gstdetails.GSTCityCode = _gstLst[7].ToString();
                                    _gstdetails.GSTPincode = _gstLst[8].ToString();
                                }
                                else
                                {
                                    _gstdetails.GSTStateCode = "";
                                    _gstdetails.GSTCityCode = "";
                                    _gstdetails.GSTPincode = "";
                                }

                            }

                            int paxref_no = 0;
                            for (int i = 0; i < dsPNRReq.Tables["PAX_INFO"].Rows.Count; i++)
                            {
                                paxref_no++;
                                Paxdetailsnew = new PaxDetails();

                                if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "ADT")
                                {
                                    Paxdetailsnew.PaxType = "ADULT";
                                }
                                else if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "CHD")
                                {
                                    Paxdetailsnew.PaxType = "CHILD";
                                }
                                else if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "INF")
                                {
                                    Paxdetailsnew.PaxType = "INFANT";
                                }

                                Paxdetailsnew.Title = dsPNRReq.Tables["PAX_INFO"].Rows[i]["TITLE"].ToString();
                                Paxdetailsnew.FirstName = dsPNRReq.Tables["PAX_INFO"].Rows[i]["FIRSTNAME"].ToString();
                                Paxdetailsnew.LastName = dsPNRReq.Tables["PAX_INFO"].Rows[i]["LASTNAME"].ToString();
                                //Paxdetailsnew.Gender = drRows["FIRSTNAME"].ToString();//AdtPaxlist[4] != null ? AdtPaxlist[4].ToString().ToUpper() == "M" ? "Male" : "Female" : "";
                                // Paxdetailsnew.DOB = dsPNRReq.Tables["PassengerPNRDetails"].Rows[i]["DATEOFBIRTH"].ToString();// (AdtPaxlist[5].ToString() == "undefined" || AdtPaxlist[5] == null) ? "" : AdtPaxlist[5];
                                Paxdetailsnew.PaxRefNumber = dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXREFERENCE"].ToString();
                                if (PassportDetails != null && PassportDetails != "")
                                {
                                    strpaxlist = PassportDetails.TrimEnd('~').Split('~');
                                    if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "ADT")
                                    {
                                        stradtlist = strpaxlist[0].TrimEnd('_').Split('_');

                                        AdtPaxlist = stradtlist[i].Split('|');

                                        Paxdetailsnew.Gender = (AdtPaxlist.Length > 4 && AdtPaxlist[4] != null && AdtPaxlist[4] != "") ? (AdtPaxlist[4].ToString().ToUpper() == "M" ? "Male" : "Female") : "";
                                        Paxdetailsnew.DOB = (AdtPaxlist.Length > 3 && AdtPaxlist[3] != null) ? AdtPaxlist[3] : "";
                                        Paxdetailsnew.PassportNo = ((AdtPaxlist[0] != null) ? AdtPaxlist[0] : "") + '|' + ((AdtPaxlist.Length > 1 && AdtPaxlist[1] != null) ? AdtPaxlist[1] : "");
                                        Paxdetailsnew.PassportExpiry = (AdtPaxlist.Length > 2 && AdtPaxlist[2] != null) ? AdtPaxlist[2] : "";
                                    }
                                }

                                if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "CHD")
                                {
                                    if (strpaxlist.Length > 1)
                                    {
                                        if (strpaxlist[1] != null && strpaxlist[1] != "")
                                        {
                                            strchdtlist = strpaxlist[1].TrimEnd('_').Split('_');

                                            CHDPaxlist = strchdtlist[i].Split('|');

                                            Paxdetailsnew.Gender = (CHDPaxlist.Length > 4 && CHDPaxlist[4] != null && CHDPaxlist[4] != "") ? (CHDPaxlist[4].ToString().ToUpper() == "M" ? "Male" : "Female") : "";
                                            Paxdetailsnew.DOB = (CHDPaxlist.Length > 3 && CHDPaxlist[3] != null) ? CHDPaxlist[3] : "";
                                            Paxdetailsnew.PassportNo = ((CHDPaxlist[0] != null) ? CHDPaxlist[0] : "") + '|' + ((CHDPaxlist.Length > 1 && CHDPaxlist[1] != null) ? CHDPaxlist[1] : "");
                                            Paxdetailsnew.PassportExpiry = (CHDPaxlist.Length > 2 && CHDPaxlist[2] != null) ? CHDPaxlist[2] : "";
                                        }
                                    }
                                }

                                if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "INF")
                                {
                                    if (strpaxlist.Length > 2 && strpaxlist[2] != null && strpaxlist[2] != "")
                                    {
                                        strinftlist = strpaxlist[2].TrimEnd('_').Split('_');
                                        INFPaxlist = strinftlist[i].Split('|');

                                        Paxdetailsnew.Gender = (INFPaxlist.Length > 4 && INFPaxlist[4] != null && INFPaxlist[4] != "") ? (INFPaxlist[4].ToString().ToUpper() == "M" ? "Male" : "Female") : "";
                                        Paxdetailsnew.DOB = (INFPaxlist.Length > 3 && INFPaxlist[3] != null) ? INFPaxlist[3] : "";
                                        Paxdetailsnew.PassportNo = ((INFPaxlist[0] != null) ? INFPaxlist[0] : "") + '|' + ((INFPaxlist.Length > 1 && INFPaxlist[1] != null) ? INFPaxlist[1] : "");
                                        Paxdetailsnew.PassportExpiry = (INFPaxlist.Length > 2 && INFPaxlist[2] != null) ? INFPaxlist[2] : "";
                                    }
                                }

                                _lstpaxdet.Add(Paxdetailsnew);
                            }

                            #region FFNumber
                            FFNNumberlst = FFNDetails.TrimEnd('~').Split('~');
                            FFNDetailsN = new FFNumber();
                            //if (strairlinePLTC != null && strairlinePLTC != "" && FFNNumberlst != null && FFNNumberlst[0] != "" && FFNNumberlst.Length > 0 && checklcc.Contains("FSC"))
                            //{


                            for (int j = 0; j < FFNNumberlst.Length; j++)
                            {

                                FFNNumberDetlst = FFNNumberlst[j].TrimEnd('*').Split('*');

                                if (FFNNumberDetlst[0] != null && FFNNumberDetlst[0].ToString() != "" && FFNNumberDetlst[1] != null && FFNNumberDetlst[1].ToString() != "")
                                {
                                    //FFNDetailsN.PaxRefNumber = dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXREFERENCE"].ToString();
                                    FFNDetailsN = new FFNumber();
                                    FFNDetailsN.AirlineCode = FFNNumberDetlst[0].ToString();
                                    FFNDetailsN.FlyerNumber = FFNNumberDetlst[1].ToString();
                                    FFNDetailsN.PaxRefNumber = FFNNumberDetlst[2];

                                    _lstFFNdet.Add(FFNDetailsN);
                                }
                            }
                            // }
                            // }
                            #endregion

                            Payment _payemnt = new Payment();

                            if (!string.IsNullOrEmpty(Passthrough))
                            {
                                string[] strpassthroughval = Passthrough.Trim().Split('|');


                                if (Passthrough != null && Passthrough != "")
                                {
                                    _payemnt.PAYMENTMODE = "B";
                                    _payemnt.CARDTYPE = strpassthroughval[0].ToString();
                                    _payemnt.CARDNUMBER = strpassthroughval[2].ToString();
                                    _payemnt.CVVNUMBER = strpassthroughval[3].ToString();
                                    _payemnt.CARDNAME = strpassthroughval[1].ToString();
                                    _payemnt.EXPIRYDATE = strpassthroughval[4].ToString();
                                }
                                else
                                {
                                    _payemnt.PAYMENTMODE = "";
                                    _payemnt.CARDTYPE = "";
                                    _payemnt.CARDNUMBER = "";
                                    _payemnt.CVVNUMBER = "";
                                    _payemnt.CARDNAME = "";
                                    _payemnt.EXPIRYDATE = "";
                                }
                            }
                            else
                            {
                                _payemnt.PAYMENTMODE = dsPNRReq.Tables["PAYMENT"].Rows[0]["MODE"].ToString();
                                _payemnt.CARDTYPE = dsPNRReq.Tables["PAYMENT"].Rows[0]["CARDTYPE"].ToString();
                                _payemnt.CARDNUMBER = dsPNRReq.Tables["PAYMENT"].Rows[0]["CARDNUMBER"].ToString();
                                _payemnt.EXPIRYDATE = dsPNRReq.Tables["PAYMENT"].Rows[0]["EXPIRYDATE"].ToString();
                                _payemnt.CVVNUMBER = dsPNRReq.Tables["PAYMENT"].Rows[0]["CVVNUMBER"].ToString();
                                _payemnt.CARDNAME = dsPNRReq.Tables["PAYMENT"].Rows[0]["CARDNAME"].ToString();
                            }

                            _payemnt.REFERANSEID = "";
                            _payemnt.PASSENGER_CONTACTNO = Employee_MobileNo != null ? Employee_MobileNo : "";
                            _payemnt.PaymentInfo = strAsPerCRS;
                            //******************************

                            DataTable dtPNRDET = dsPNRReq.Tables["PassengerPNRDetails"];

                            string strPCdetails = string.Empty;

                            //if (Session["PROMOCODERESPONSE_Queue"] != null)
                            //{
                            //    strPCdetails = Session["PROMOCODERESPONSE_Queue"].ToString();
                            //    DataSet PromocodeDet = (DataSet)JsonConvert.DeserializeObject(strPCdetails, typeof(DataSet));
                            //    if (PromocodeDet != null && PromocodeDet.Tables[0].Rows.Count > 0)
                            //    {
                            //        var PromocodeS = (from _PCCode in PromocodeDet.Tables[0].AsEnumerable()
                            //                          where _PCCode["TC_TYPE"].ToString().Trim() == "PC" && _PCCode["TC_AIRLINE"].ToString().Trim() == strPLATINGCARRIER
                            //                          select new
                            //                          {
                            //                              PCCOde = _PCCode["TC_CODE"].ToString().Trim(),
                            //                              Airline = _PCCode["TC_AIRLINE"].ToString().Trim(),
                            //                              AirportType = _PCCode["TC_AIRPORT_TYPE"].ToString().Trim(),
                            //                              TYPE = _PCCode["TC_TYPE"].ToString().Trim()
                            //                          }
                            //                             ).ToList();
                            //        var Tourcode = (from _PCCode in PromocodeDet.Tables[0].AsEnumerable()
                            //                        where _PCCode["TC_TYPE"].ToString().Trim() == "TC" && _PCCode["TC_AIRLINE"].ToString().Trim() == strPLATINGCARRIER
                            //                        select new
                            //                        {
                            //                            TCOde = _PCCode["TC_CODE"].ToString().Trim(),
                            //                            Airline = _PCCode["TC_AIRLINE"].ToString().Trim(),
                            //                            AirportType = _PCCode["TC_AIRPORT_TYPE"].ToString().Trim(),
                            //                            TYPE = _PCCode["TC_TYPE"].ToString().Trim()
                            //                        }
                            //                             ).ToList();
                            //        _pnrdetail.TOURCODE = Tourcode.FirstOrDefault().TCOde.ToString();
                            //        _pnrdetail.CORPORATECODE = PromocodeS.FirstOrDefault().PCCOde.ToString();
                            //    }
                            //}

                            string request = "";
                            string methodname = "InvokePNRTicketing";
                            int servicetimeout = Convert.ToInt32(ConfigurationManager.AppSettings["QTKT_servicetimeout"]);

                            _QueueTicketingRQ.AgentDetail = _Agentdetail;
                            _QueueTicketingRQ.TicketPnrDetails = _lstpnrdet;
                            _QueueTicketingRQ.PNRDetail = _pnrdetail;
                            _QueueTicketingRQ.PaymentDetail = _payemnt;
                            _QueueTicketingRQ.Token = faretoken;
                            _QueueTicketingRQ.GstDetails = _gstdetails;
                            _QueueTicketingRQ.PaxDetails = _lstpaxdet;
                            _QueueTicketingRQ.FFNumber = _lstFFNdet;
                            _QueueTicketingRQ.CBT_Input = _CBT_Credentials;
                            _QueueTicketingRQ.TicketingMode = TicketingMode;
                            _QueueTicketingRQ.IsGST = RetrieveIsGST;
                            _QueueTicketingRQ.ISAllFare = true;
                            _QueueTicketingRQ.Earnings = _lstEarning;
                            _QueueTicketingRQ.ISReeBook = strRebookFlag;
                            _QueueTicketingRQ.ISPNRFOP = strFOPFlag;
                            _QueueTicketingRQ.PAYMENT_INFO = _CashPaymentInfo;

                            request = JsonConvert.SerializeObject(_QueueTicketingRQ);
                            MyWebClient client = new MyWebClient();

                            string strURLpath = ConfigurationManager.AppSettings["QTKT_Service_URL"].ToString();
                            string response = string.Empty;
                            string errorMessage = string.Empty;

                            string Request = request;
                            string url = strURLpath + "/" + methodname;

                            // client.LintTimeout = int.MaxValue;
                            client.Headers["Content-type"] = "application/json";

                            //****************REQUEST LOG****************************************

                            strTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            xmlData = "<Event><REQUEST><REQUESTTIME>" + strTime + "</REQUESTTIME><PAGENAME>RetrieveBookingController.cs</PAGENAME><METHODNAME>Ticketing_QueuePNR_Details</METHODNAME>"
                                + "<REQUESTJSONSTRING>" + Request + "</REQUESTJSONSTRING>"
                             + "</REQUEST></Event>";

                            DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                            //****************END REQUEST LOG****************************************

                            byte[] data = client.UploadData(url, "POST", Encoding.ASCII.GetBytes(Request));
                            strresponse = Encoding.ASCII.GetString(data);

                            //string strresponse = System.File.ReadAllText(@"D:\BookingResponse.txt");//while putting live

                            //****************RESPONSE LOG****************************************

                            strTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            xmlData = "<Event><RESPONSE><RESPONSETIME>" + strTime + "</RESPONSETIME><PAGENAME>QUEUETICKET_WCF</PAGENAME><METHODNAME>Ticketing_QueuePNR_Details</METHODNAME>"
                               + "<RESPONSEJSONSTRING>" + strresponse + "</RESPONSEJSONSTRING>"
                            + "</RESPONSE></Event>";

                            DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                            //****************END REQUEST LOG****************************************

                            if (!string.IsNullOrEmpty(strresponse))
                            {
                                dsTicketing = _ServiceUtility.convertJsonStringToDataSet(strresponse, "");
                            }

                            //***************************end Request in RQRS formatt**********************************
                            xmlData = string.Empty;

                            xmlData = "<EVENT>" + "<REQUEST>TICKETING_QUEUEPNR_DETAILS</REQUEST>" +
                                                "<STATUS>SUCCESS</STATUS>" +
                                                "<PNRINFO>" + strwrt.ToString() + "</PNRINFO>" +
                                                "</EVENT>";
                            DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");

                            strwrt = new StringWriter();
                            dsTicketing.WriteXml(strwrt);
                            xmlData = string.Empty;

                            xmlData = "<EVENT>" + "<RESPONSE>TICKETING_QUEUEPNR_DETAILS</RESPONSE>" +
                                                "<STATUS>SUCCESS</STATUS>" +
                                                "<PNRINFO>" + strwrt.ToString() + "</PNRINFO>" +
                                                "<ERRORMSG>" + strErrorMsg + "</ERRORMSG>" +
                                                "</EVENT>";

                            dsTicketingPNRResponse.Merge(dsTicketing.Copy());

                            #region Inser Ticket Details
                            if (dsTicketingPNRResponse != null && dsTicketingPNRResponse.Tables[1].Rows.Count > 0 && dsTicketingPNRResponse.Tables[1].Rows[0]["Code"].ToString() == "1")
                            {

                                string strErrorMsgs = string.Empty;
                                string str_UserName = Session["username"] != null ? Session["username"].ToString() : "";
                                string str_agentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                                string str_TerminalID = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
                                string str_PNR = dsTicketingPNRResponse.Tables[0].Rows[0]["SPNR"].ToString();
                                string str_EmployeeID = Employeename;
                                string str_CorporateID = Corporatename;
                                string str_REFID = EmpCostCenter;
                                string str_Remarks = string.Empty;
                                string str_ReferanseID = EmpRefID;
                                string str_Chargeability = string.Empty;
                                string str_Cost_Center = EmpCostCenter;
                                string str_TR_Reason = string.Empty;
                                string str_BookingType = "R";

                                string str_To_Cost_Center = EmpCostCenter;
                                string str_Recharge = string.Empty;
                                string str_TravelReq_No = string.Empty;
                                string str_Budget_Code = string.Empty;
                                string str_Job_Number = string.Empty;
                                string str_Package_ID = string.Empty;
                                string str_subreason = string.Empty;

                                string Bool_Ticket = _InplantService.Update_Ticket_Details_Table(str_agentID, str_TerminalID, str_UserName, str_PNR,
                                    str_EmployeeID, str_CorporateID, str_REFID, str_Remarks, str_ReferanseID, str_Chargeability, str_Cost_Center, str_TR_Reason, str_BookingType,
                                    str_To_Cost_Center, str_Recharge, str_TravelReq_No, str_Budget_Code, str_Job_Number, str_Package_ID, str_subreason, ref strErrorMsgs);

                                #region ERPPUSH
                                string strUserName_ERP = dsTicketingPNRResponse.Tables[0].Rows.Count > 0 ? dsTicketingPNRResponse.Tables[0].Rows[0]["FIRSTNAME"].ToString() + " " + dsTicketingPNRResponse.Tables[0].Rows[0]["LASTNAME"].ToString() : "";
                             
                                _RaysService.ERPPUSHFORBOOKING(str_CorporateID, str_PNR, "", "AIR", "S", "", strUserName_ERP, ControllerContext.HttpContext.Request.UserHostName.ToString(), "");
                                string xmldata = "<ERPPUSHREQ><ERPSPNR>" + "ERPPUSHDETAILS" + "</ERPSPNR><ERPUSERNAM>" + strUserName_ERP + "</ERPUSERNAM></ERPPUSHREQ>";
                                DatabaseLog.Retrievetcktlog(System.Web.HttpContext.Current.Session["UserMailID"] != null ? System.Web.HttpContext.Current.Session["UserMailID"].ToString() : "", "T", "RetrieveBookingController.cs", "ERPPUSH", xmldata, strClientID, strPosTID, System.Web.HttpContext.Current.Session["SEQUENCEID"] != null && System.Web.HttpContext.Current.Session["SEQUENCEID"].ToString() != "" ? System.Web.HttpContext.Current.Session["SEQUENCEID"].ToString() : "");
                                #endregion
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        var qryTST = (from p in dtTicketReq.AsEnumerable()
                                      select new
                                      {
                                          TSTCOUNT = p.Field<string>("TSTCOUNT")
                                      }).Distinct();
                        if (qryTST.Count() > 0)
                        {
                            DataTable dtTST2 = ConvertToDataTable(qryTST);
                            foreach (DataRow drRows in dtTST2.Rows)
                            {
                                dsTicketingPNRReq = new DataSet();
                                dtPNR = dtPNR.Clone();
                                var QrySegment = (from p in dtTicketReq.AsEnumerable()
                                                  where p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim()
                                                  select new
                                                  {
                                                      ORIGIN = p.Field<string>("ORIGIN").Trim(),
                                                      DESTINATION = p.Field<string>("DESTINATION").Trim(),
                                                      TSTREFERENCE = p.Field<string>("TSTREFERENCE").Trim(),
                                                      SEGMENTREFERENCE = p.Field<string>("SEGMENTREFERENCE").Trim(),
                                                      PAXREFERENCE = p.Field<string>("PAXREFERENCE").Trim(),
                                                      PLATINGCARRIER = p.Field<string>("PLATINGCARRIER").Trim(),
                                                      FAREBASISCODE = p.Field<string>("FAREBASISCODE").Trim(),
                                                      TSTCOUNT = p.Field<string>("TSTCOUNT").Trim(),
                                                      TOURCODE = p.Field<string>("TOURCODE").Trim(),
                                                  }).Distinct();

                                DataTable dtTSTSegment = ConvertToDataTable(QrySegment);

                                var QryAPIFare = (from p in dsPNRReq.Tables["PAXDETAILS"].AsEnumerable()
                                                  where p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim()
                                                  select new
                                                  {
                                                      API_GROSSFARE = p.Field<string>("API_GROSSFARE")
                                                  }).Distinct();

                                DataTable dtAPITSTFare = ConvertToDataTable(QryAPIFare);

                                string CorporateCode = string.Empty;
                                CorporateCode = dsPNRReq.Tables["PAYMENT"].Rows[0]["CORPORATECODE"].ToString();

                                foreach (DataRow drTSTSegmentRows in dtTSTSegment.Rows)
                                {
                                    dtPNR.Rows.Add(dsPNRReq.Tables["PassengerPNRDetails"].Rows[0]["CRSPNR"].ToString(),
                                    dsPNRReq.Tables["PAXDETAILS"].Rows[0]["TRACKID"].ToString(),
                                    dsPNRReq.Tables["AGENT_INFO"].Rows[0]["CRS_ID"].ToString(), drTSTSegmentRows["ORIGIN"],
                                    drTSTSegmentRows["DESTINATION"], drTSTSegmentRows["TSTREFERENCE"], drTSTSegmentRows["SEGMENTREFERENCE"],
                                    drTSTSegmentRows["PAXREFERENCE"], drTSTSegmentRows["PLATINGCARRIER"], drTSTSegmentRows["FAREBASISCODE"],
                                    drTSTSegmentRows["TSTCOUNT"], dtAPITSTFare.Rows[0]["API_GROSSFARE"], CorporateCode, dsPNRReq.Tables["AGENT_INFO"].Rows[0]["QUEUENUMBER"].ToString());
                                }

                                dsTicketingPNRReq.Tables.Add(dtAgentDetails.Copy());
                                dsTicketingPNRReq.Tables.Add(dtPNR.Copy());
                                if (dsPNRReq.Tables.Contains("PAYMENT"))
                                    dsTicketingPNRReq.Tables.Add(dsPNRReq.Tables["PAYMENT"].Copy());


                                strwrt = new StringWriter();
                                dsTicketingPNRReq.WriteXml(strwrt);

                                var Qrypnrdetails = (from p in dsPNRReq.Tables["PassengerPNRDetails"].AsEnumerable()
                                                     where p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim()
                                                     select p).Distinct();
                                DataTable dtTSTPnrdetails = Qrypnrdetails.CopyToDataTable();

                                foreach (DataRow drTSTSegmentRows in dtTSTSegment.Rows)
                                {
                                    _pnrdetail = new PNRDetails();
                                    _pnrdetail.CRSPNR = dsPNRReq.Tables["PassengerPNRDetails"].Rows[0]["CRSPNR"].ToString();
                                    _pnrdetail.CRSID = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["CRS_ID"].ToString();
                                    _pnrdetail.CORPORATECODE = dsPNRReq.Tables["PAYMENT"].Rows[0]["CORPORATECODE"].ToString();
                                    _pnrdetail.TOURCODE = (strPricingcode != null && strPricingcode != "") ? strPricingcode : drTSTSegmentRows["TOURCODE"].ToString();
                                    //_pnrdetail.BOOKINGAMOUNT = dtAPITSTFare.Rows[0]["API_GROSSFARE"].ToString();fareQualifier
                                    _pnrdetail.BOOKINGAMOUNT = WithoutTaxBookingAMount.ToString();
                                    _pnrdetail.QUEUENUMBER = dsPNRReq.Tables["AGENT_INFO"].Rows[0]["QUEUENUMBER"].ToString();
                                    _pnrdetail.ADULT = dsPNRReq.Tables["PAXDETAILS"].Rows[0]["ADULTCOUNT"].ToString();
                                    _pnrdetail.CHILD = dsPNRReq.Tables["PAXDETAILS"].Rows[0]["CHILDCOUNT"].ToString();
                                    _pnrdetail.INFANT = dsPNRReq.Tables["PAXDETAILS"].Rows[0]["INFANTCOUNT"].ToString();
                                    _pnrdetail.SEGMENTTYPE = strSegmentType;
                                    _pnrdetail.FAREQUALIFIER = farequalifier;
                                    _pnrdetail.OBCTax = OBCTAX;
                                    _pnrdetail.MARKUP = Markup;
                                    _pnrdetail.EARNINGS = Commission;
                                    _pnrdetail.PLBAmount = PLB;
                                    _pnrdetail.Endorsement = strEndorsement;
                                }

                                foreach (DataRow drTSTpnrRows in dtTSTPnrdetails.Rows)
                                {
                                    _tkt_pnrdetails = new TicketPnrDetail();

                                    _tkt_pnrdetails.AIRLINEPNR = drTSTpnrRows["AIRLINEPNR"].ToString();
                                    _tkt_pnrdetails.CARRIERCODE = drTSTpnrRows["AIRLINECODE"].ToString();
                                    _tkt_pnrdetails.FLIGHTNO = drTSTpnrRows["FLIGHTNO"].ToString();
                                    _tkt_pnrdetails.ORIGIN = drTSTpnrRows["ORIGIN"].ToString();
                                    _tkt_pnrdetails.DESTINATION = drTSTpnrRows["DESTINATION"].ToString();
                                    _tkt_pnrdetails.DEPARTUREDATE = drTSTpnrRows["DEPARTUREDATE"].ToString();
                                    _tkt_pnrdetails.ARRIVALDATE = drTSTpnrRows["ARRIVALDATE"].ToString();
                                    _tkt_pnrdetails.DEPARTURETIME = drTSTpnrRows["DEPARTURETIME"].ToString();
                                    _tkt_pnrdetails.ARRIVALTIME = drTSTpnrRows["ARRIVALTIME"].ToString();
                                    _tkt_pnrdetails.CLASS = drTSTpnrRows["CLASS"].ToString();
                                    _tkt_pnrdetails.FAREBASISCODE = drTSTpnrRows["FAREBASISCODE"].ToString();
                                    //_tkt_pnrdetails.TSTREFERENCE = drTSTpnrRows["TSTREFERENCE"].ToString();tstReference
                                    _tkt_pnrdetails.TSTREFERENCE = tstReference;
                                    _tkt_pnrdetails.SEGMENTREFERENCE = drTSTpnrRows["SEGMENTREFERENCE"].ToString();
                                    _tkt_pnrdetails.PAXREFERENCE = drTSTpnrRows["PAXREFERENCE"].ToString();
                                    _tkt_pnrdetails.PLATINGCARRIER = ((strPlatingcarrier != null && strPlatingcarrier != "") ? strPlatingcarrier : drTSTpnrRows["TICKETINGCARRIER"].ToString());
                                    _tkt_pnrdetails.TSTCOUNT = drTSTpnrRows["TSTCOUNT"].ToString();
                                    _tkt_pnrdetails.PAXNO = drTSTpnrRows["PAXNO"].ToString();
                                    _tkt_pnrdetails.SEGMENTNO = drTSTpnrRows["SEGMENTNO"].ToString();

                                    //_tkt_pnrdetails.PLATINGCARRIER
                                    strPLATINGCARRIER = drTSTpnrRows["PLATINGCARRIER"].ToString();
                                    _lstpnrdet.Add(_tkt_pnrdetails);

                                }
                            }
                            if (ERPDetails != null && ERPDetails != "")
                            {
                                string[] _strERPDeatils = ERPDetails.TrimEnd('~').Split('~');

                                for (int i = 0; i < _strERPDeatils.Count(); i++)
                                {
                                    _ERP_Attribute = new ERP_Attribute();
                                    string[] lstarray = _strERPDeatils[i].Split('|');
                                    _ERP_Attribute.AttributesName = "ERP_ATR_COL" + lstarray[0];
                                    _ERP_Attribute.AttributesValue = lstarray[2];
                                    _LST_ERP_Attribute.Add(_ERP_Attribute);
                                }
                            }

                            string[] _strERPDeatils_TICK = OtherTicketInfo.Split('|');

                            _CBT_Credentials.ApprovedBy = "";
                            _CBT_Credentials.Chargeabilty = "";
                            _CBT_Credentials.Department = "";
                            _CBT_Credentials.CustLocation = "";
                            _CBT_Credentials.EmpCode = "";
                            _CBT_Credentials.PersonalBooking = "";
                            _CBT_Credentials.ProjectCode = "";

                            _CBT_Credentials.TRNumber = "";
                            _CBT_Credentials.Budgetcode = "";
                            _CBT_Credentials.VesselName = "";
                            _CBT_Credentials.CostCenter = EmpCostCenter;
                            _CBT_Credentials.CorpEmpCode = "";
                            _CBT_Credentials.ERP_Attributes = _LST_ERP_Attribute;

                            TicketingOfficeId = SessionKey;
                            //***************************Request in RQRS formatt**********************************
                            string strClientTerminalID = string.Empty;
                            string ClientID = string.Empty;
                            if (strTerminalType == "T" && strClientID != "")
                            {
                                ClientID = strClientID;
                                strClientTerminalID = strClientID;
                            }
                            else
                            {
                                ClientID = strPosID;
                                strClientTerminalID = strPosTID;
                            }
                            //
                            _Agentdetail.AgentID = strAgentID;
                            _Agentdetail.TerminalID = strTerminalId;
                            _Agentdetail.AppType = "B2B";
                            _Agentdetail.UserName = strUserName;
                            _Agentdetail.BranchID = BranchID;
                            _Agentdetail.ProductType = "RC";
                            _Agentdetail.PNROfficeId = RetrivePCC;
                            _Agentdetail.TicketingOfficeId = TicketingOfficeId;
                            _Agentdetail.ClientID = Corporatename;//
                            _Agentdetail.Version = "";
                            _Agentdetail.BOAID = ClientID;
                            _Agentdetail.BOATerminalID = strClientTerminalID;
                            _Agentdetail.AgentType = "";
                            _Agentdetail.CoOrdinatorID = "";
                            _Agentdetail.IssuingBranchID = BranchID;
                            _Agentdetail.EMP_ID = Employeename;
                            _Agentdetail.COST_CENTER = EmpCostCenter;
                            _Agentdetail.Ipaddress = Ipaddress;
                            _Agentdetail.Environment = (strTerminalType == "T" ? "I" : "W");
                            _Agentdetail.Platform = "B";
                            _Agentdetail.PNROfficeId = SessionKey;

                            GSTDETAILS _gstdetails = new GSTDETAILS();

                            if (GSTDetails != "")
                            {
                                string[] _gstLst = GSTDetails.Split('|');
                                _gstdetails.GSTAddress = _gstLst[3].ToString();
                                _gstdetails.GSTCompanyName = _gstLst[2].ToString();
                                _gstdetails.GSTEmailID = _gstLst[4].ToString();
                                _gstdetails.GSTMobileNumber = _gstLst[5].ToString();
                                _gstdetails.GSTNumber = _gstLst[1].ToString();

                                if (CRSTYPE.ToUpper().Trim() == "1G")
                                {
                                    _gstdetails.GSTStateCode = _gstLst[6].ToString();
                                    _gstdetails.GSTCityCode = _gstLst[7].ToString();
                                    _gstdetails.GSTPincode = _gstLst[8].ToString();
                                }
                                else
                                {
                                    _gstdetails.GSTStateCode = "";
                                    _gstdetails.GSTCityCode = "";
                                    _gstdetails.GSTPincode = "";
                                }
                            }

                            int paxref_no = 0;
                            for (int i = 0; i < dsPNRReq.Tables["PAX_INFO"].Rows.Count; i++)
                            {
                                paxref_no++;

                                Paxdetailsnew = new PaxDetails();

                                if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "ADT")
                                    Paxdetailsnew.PaxType = "ADULT";
                                else if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "CHD")
                                    Paxdetailsnew.PaxType = "CHILD";
                                else if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "INF")
                                    Paxdetailsnew.PaxType = "INFANT";

                                Paxdetailsnew.Title = dsPNRReq.Tables["PAX_INFO"].Rows[i]["TITLE"].ToString();
                                Paxdetailsnew.FirstName = dsPNRReq.Tables["PAX_INFO"].Rows[i]["FIRSTNAME"].ToString();
                                Paxdetailsnew.LastName = dsPNRReq.Tables["PAX_INFO"].Rows[i]["LASTNAME"].ToString();

                                //Paxdetailsnew.Gender = drRows["FIRSTNAME"].ToString();//AdtPaxlist[4] != null ? AdtPaxlist[4].ToString().ToUpper() == "M" ? "Male" : "Female" : "";
                                // Paxdetailsnew.DOB = dsPNRReq.Tables["PassengerPNRDetails"].Rows[i]["DATEOFBIRTH"].ToString();// (AdtPaxlist[5].ToString() == "undefined" || AdtPaxlist[5] == null) ? "" : AdtPaxlist[5];
                                Paxdetailsnew.PaxRefNumber = dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXREFERENCE"].ToString();
                                if (PassportDetails != null && PassportDetails != "")
                                {
                                    strpaxlist = PassportDetails.TrimEnd('~').Split('~');
                                }

                                if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "ADT")
                                {
                                    stradtlist = strpaxlist[0].TrimEnd('_').Split('_');
                                    AdtPaxlist = stradtlist[i].Split('|');

                                    Paxdetailsnew.Gender = (AdtPaxlist.Length > 4 && AdtPaxlist[4] != null && AdtPaxlist[4] != "") ? (AdtPaxlist[4].ToString().ToUpper() == "M" ? "Male" : "Female") : "";
                                    Paxdetailsnew.DOB = (AdtPaxlist.Length > 3 && AdtPaxlist[3] != null) ? AdtPaxlist[3] : "";
                                    Paxdetailsnew.PassportNo = ((AdtPaxlist[0] != null) ? AdtPaxlist[0] : "") + '|' + ((AdtPaxlist.Length > 1 && AdtPaxlist[1] != null) ? AdtPaxlist[1] : "");
                                    Paxdetailsnew.PassportExpiry = (AdtPaxlist.Length > 2 && AdtPaxlist[2] != null) ? AdtPaxlist[2] : "";
                                }

                                if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "CHD")
                                {
                                    if (strpaxlist.Length > 1)
                                    {
                                        if (strpaxlist[1] != null && strpaxlist[1] != "")
                                        {
                                            strchdtlist = strpaxlist[1].TrimEnd('_').Split('_');
                                            CHDPaxlist = strchdtlist[i].Split('|');

                                            Paxdetailsnew.Gender = (CHDPaxlist.Length > 4 && CHDPaxlist[4] != null && CHDPaxlist[4] != "") ? (CHDPaxlist[4].ToString().ToUpper() == "M" ? "Male" : "Female") : "";
                                            Paxdetailsnew.DOB = (CHDPaxlist.Length > 3 && CHDPaxlist[3] != null) ? CHDPaxlist[3] : "";
                                            Paxdetailsnew.PassportNo = ((CHDPaxlist[0] != null) ? CHDPaxlist[0] : "") + '|' + ((CHDPaxlist.Length > 1 && CHDPaxlist[1] != null) ? CHDPaxlist[1] : "");
                                            Paxdetailsnew.PassportExpiry = (CHDPaxlist.Length > 2 && CHDPaxlist[2] != null) ? CHDPaxlist[2] : "";
                                        }
                                    }
                                }

                                if (dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXTYPE"].ToString().ToUpper() == "INF")
                                {
                                    if (strpaxlist.Length > 2 && strpaxlist[2] != null && strpaxlist[2] != "")
                                    {
                                        strinftlist = strpaxlist[2].TrimEnd('_').Split('_');
                                        INFPaxlist = strinftlist[i].Split('|');

                                        Paxdetailsnew.Gender = (INFPaxlist.Length > 4 && INFPaxlist[4] != null && INFPaxlist[4] != "") ? (INFPaxlist[4].ToString().ToUpper() == "M" ? "Male" : "Female") : "";
                                        Paxdetailsnew.DOB = (INFPaxlist.Length > 3 && INFPaxlist[3] != null) ? INFPaxlist[3] : "";
                                        Paxdetailsnew.PassportNo = ((INFPaxlist[0] != null) ? INFPaxlist[0] : "") + '|' + ((INFPaxlist.Length > 1 && INFPaxlist[1] != null) ? INFPaxlist[1] : "");
                                        Paxdetailsnew.PassportExpiry = (INFPaxlist.Length > 2 && INFPaxlist[2] != null) ? INFPaxlist[2] : "";
                                    }
                                }

                                _lstpaxdet.Add(Paxdetailsnew);
                            }

                            #region FFNumber
                            FFNNumberlst = FFNDetails.TrimEnd('~').Split('~');
                            FFNDetailsN = new FFNumber();
                            //if (strairlinePLTC != null && strairlinePLTC != "" && FFNNumberlst != null && FFNNumberlst[0] != "" && FFNNumberlst.Length > 0 && checklcc.Contains("FSC"))
                            //{


                            for (int j = 0; j < FFNNumberlst.Length; j++)
                            {

                                FFNNumberDetlst = FFNNumberlst[j].TrimEnd('*').Split('*');

                                if (FFNNumberDetlst[0] != null && FFNNumberDetlst[0].ToString() != "" && FFNNumberDetlst[1] != null && FFNNumberDetlst[1].ToString() != "")
                                {
                                    //FFNDetailsN.PaxRefNumber = dsPNRReq.Tables["PAX_INFO"].Rows[i]["PAXREFERENCE"].ToString();
                                    FFNDetailsN = new FFNumber();
                                    FFNDetailsN.AirlineCode = FFNNumberDetlst[0].ToString();
                                    FFNDetailsN.FlyerNumber = FFNNumberDetlst[1].ToString();
                                    FFNDetailsN.PaxRefNumber = FFNNumberDetlst[2];

                                    _lstFFNdet.Add(FFNDetailsN);
                                }
                            }
                            // }
                            // }
                            #endregion

                            Payment _payemnt = new Payment();

                            if (!string.IsNullOrEmpty(Passthrough))
                            {
                                string[] strpassthroughval = Passthrough.Trim().Split('|');


                                if (Passthrough != null && Passthrough != "")
                                {
                                    _payemnt.PAYMENTMODE = "B";
                                    _payemnt.CARDTYPE = strpassthroughval[0].ToString();
                                    _payemnt.CARDNUMBER = strpassthroughval[2].ToString();
                                    _payemnt.CVVNUMBER = strpassthroughval[3].ToString();
                                    _payemnt.CARDNAME = strpassthroughval[1].ToString();
                                    _payemnt.EXPIRYDATE = strpassthroughval[4].ToString();
                                }
                                else
                                {
                                    _payemnt.PAYMENTMODE = "";
                                    _payemnt.CARDTYPE = "";
                                    _payemnt.CARDNUMBER = "";
                                    _payemnt.CVVNUMBER = "";
                                    _payemnt.CARDNAME = "";
                                    _payemnt.EXPIRYDATE = "";
                                }
                            }
                            else
                            {
                                _payemnt.PAYMENTMODE = dsPNRReq.Tables["PAYMENT"].Rows[0]["MODE"].ToString();
                                _payemnt.CARDTYPE = dsPNRReq.Tables["PAYMENT"].Rows[0]["CARDTYPE"].ToString();
                                _payemnt.CARDNUMBER = dsPNRReq.Tables["PAYMENT"].Rows[0]["CARDNUMBER"].ToString();
                                _payemnt.EXPIRYDATE = dsPNRReq.Tables["PAYMENT"].Rows[0]["EXPIRYDATE"].ToString();
                                _payemnt.CVVNUMBER = dsPNRReq.Tables["PAYMENT"].Rows[0]["CVVNUMBER"].ToString();
                                _payemnt.CARDNAME = dsPNRReq.Tables["PAYMENT"].Rows[0]["CARDNAME"].ToString();
                            }

                            _payemnt.REFERANSEID = "";
                            _payemnt.PASSENGER_CONTACTNO = Employee_MobileNo != null ? Employee_MobileNo : "";
                            _payemnt.PaymentInfo = strAsPerCRS;

                            DataTable dtPNRDET = dsPNRReq.Tables["PassengerPNRDetails"];

                            string strPCdetails = string.Empty;
                            string request = "";// JsonConvert.SerializeObject(_request).ToString();
                            string methodname = "InvokePNRTicketing";
                            int servicetimeout = Convert.ToInt32(ConfigurationManager.AppSettings["QTKT_servicetimeout"]);

                            _QueueTicketingRQ.AgentDetail = _Agentdetail;
                            _QueueTicketingRQ.TicketPnrDetails = _lstpnrdet;
                            _QueueTicketingRQ.PNRDetail = _pnrdetail;
                            _QueueTicketingRQ.PaymentDetail = _payemnt;
                            _QueueTicketingRQ.Token = faretoken;
                            _QueueTicketingRQ.GstDetails = _gstdetails;
                            _QueueTicketingRQ.PaxDetails = _lstpaxdet;
                            _QueueTicketingRQ.FFNumber = _lstFFNdet;
                            _QueueTicketingRQ.CBT_Input = _CBT_Credentials;
                            _QueueTicketingRQ.TicketingMode = TicketingMode;
                            _QueueTicketingRQ.IsGST = RetrieveIsGST;
                            _QueueTicketingRQ.ISAllFare = true;
                            _QueueTicketingRQ.Earnings = _lstEarning;
                            _QueueTicketingRQ.ISReeBook = strRebookFlag;
                            _QueueTicketingRQ.ISPNRFOP = strFOPFlag;
                            _QueueTicketingRQ.PAYMENT_INFO = _CashPaymentInfo;

                            request = JsonConvert.SerializeObject(_QueueTicketingRQ);
                            MyWebClient client = new MyWebClient();

                            string strURLpath = ConfigurationManager.AppSettings["QTKT_Service_URL"].ToString();
                            string response = string.Empty;
                            string errorMessage = string.Empty;

                            string Request = request;
                            string url = strURLpath + "/" + methodname;

                            // client.LintTimeout = int.MaxValue;
                            client.Headers["Content-type"] = "application/json";

                            //****************REQUEST LOG****************************************

                            strTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            xmlData = "<Event><REQUEST><REQUESTTIME>" + strTime + "</REQUESTTIME><PAGENAME>QUEUETICKET_WCF</PAGENAME><METHODNAME>Ticketing_QueuePNR_Details</METHODNAME>"
                                + "<REQUESTJSONSTRING>" + Request + "</REQUESTJSONSTRING>"
                             + "</REQUEST></Event>";

                            DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                            //****************END REQUEST LOG****************************************

                            byte[] data = client.UploadData(url, "POST", Encoding.ASCII.GetBytes(Request));
                            strresponse = Encoding.ASCII.GetString(data);//while putting live

                            //strresponse = System.IO.File.ReadAllText(@"C:\Users\STS\Desktop\BookingResponse.txt");//while putting live

                            //****************RESPONSE LOG****************************************

                            strTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            xmlData = "<Event><RESPONSE><RESPONSETIME>" + strTime + "</RESPONSETIME><PAGENAME>RetrieveBookingController.cs</PAGENAME><METHODNAME>Ticketing_QueuePNR_Details</METHODNAME>"
                               + "<RESPONSEJSONSTRING>" + strresponse + "</RESPONSEJSONSTRING>"
                            + "</RESPONSE></Event>";

                            DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                            //****************END REQUEST LOG****************************************

                            if (!string.IsNullOrEmpty(strresponse))
                            {
                                dsTicketing = _ServiceUtility.convertJsonStringToDataSet(strresponse, "");
                            }
                            //***************************end Request in RQRS formatt**********************************
                            xmlData = string.Empty;

                            xmlData = "<EVENT>" + "<REQUEST>TICKETING_QUEUEPNR_DETAILS</REQUEST>" +
                                                "<STATUS>SUCCESS</STATUS>" +
                                                "<PNRINFO>" + strwrt.ToString() + "</PNRINFO>" +
                                                "</EVENT>";
                            DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", xmlData.ToString(), strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");

                            strwrt = new StringWriter();
                            dsTicketing.WriteXml(strwrt);
                            xmlData = string.Empty;

                            xmlData = "<EVENT>" + "<RESPONSE>TICKETING_QUEUEPNR_DETAILS</RESPONSE>" +
                                                "<STATUS>SUCCESS</STATUS>" +
                                                "<PNRINFO>" + strwrt.ToString() + "</PNRINFO>" +
                                                "<ERRORMSG>" + strErrorMsg + "</ERRORMSG>" +
                                                "</EVENT>";

                            dsTicketingPNRResponse.Merge(dsTicketing.Copy());

                            #region Inser Ticket Details
                            if (dsTicketingPNRResponse != null && dsTicketingPNRResponse.Tables[1].Rows.Count > 0 && dsTicketingPNRResponse.Tables[1].Rows[0]["Code"].ToString() == "1")
                            {

                                
                                string strErrorMsgs = string.Empty;
                                string str_UserName = Session["username"] != null ? Session["username"].ToString() : "";
                                string str_agentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                                string str_TerminalID = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
                                string str_PNR = dsTicketingPNRResponse.Tables[0].Rows[0]["SPNR"].ToString();
                                string str_EmployeeID = Employeename;
                                string str_CorporateID = Corporatename;
                                string str_REFID = EmpCostCenter;
                                string str_Remarks = string.Empty;
                                string str_ReferanseID = EmpRefID;
                                string str_Chargeability = string.Empty;
                                string str_Cost_Center = EmpCostCenter;
                                string str_TR_Reason = string.Empty;
                                string str_BookingType = "R";

                                string str_To_Cost_Center = EmpCostCenter;
                                string str_Recharge = string.Empty;
                                string str_TravelReq_No = string.Empty;
                                string str_Budget_Code = string.Empty;
                                string str_Job_Number = string.Empty;
                                string str_Package_ID = string.Empty;
                                string str_subreason = string.Empty;

                                string Bool_Ticket = _InplantService.Update_Ticket_Details_Table(str_agentID, str_TerminalID, str_UserName, str_PNR,
                                    str_EmployeeID, str_CorporateID, str_REFID, str_Remarks, str_ReferanseID, str_Chargeability, str_Cost_Center, str_TR_Reason, str_BookingType,
                                    str_To_Cost_Center, str_Recharge, str_TravelReq_No, str_Budget_Code, str_Job_Number, str_Package_ID, str_subreason, ref strErrorMsgs);

                                #region ERPPUSH
                                string strUserName_ERP = dsTicketingPNRResponse.Tables[0].Rows.Count > 0 ? dsTicketingPNRResponse.Tables[0].Rows[0]["FIRSTNAME"].ToString() + " " + dsTicketingPNRResponse.Tables[0].Rows[0]["LASTNAME"].ToString() : "";
                                
                                _RaysService.ERPPUSHFORBOOKING(str_CorporateID, str_PNR, "", "AIR", "S", "", strUserName_ERP, ControllerContext.HttpContext.Request.UserHostName.ToString(), "");
                                string xmldata = "<ERPPUSHREQ><ERPSPNR>" + "ERPPUSHDETAILS" + "</ERPSPNR><ERPUSERNAM>" + strUserName_ERP + "</ERPUSERNAM></ERPPUSHREQ>";
                                DatabaseLog.Retrievetcktlog(System.Web.HttpContext.Current.Session["UserMailID"] != null ? System.Web.HttpContext.Current.Session["UserMailID"].ToString() : "", "T", "TRIPBOOKINGPAGE", "ERPPUSH", xmldata, str_CorporateID, str_EmployeeID, System.Web.HttpContext.Current.Session["SEQUENCEID"] != null && System.Web.HttpContext.Current.Session["SEQUENCEID"].ToString() != "" ? System.Web.HttpContext.Current.Session["SEQUENCEID"].ToString() : "");

                                #endregion


                            }
                            #endregion
                        }
                    }
                }
                //}
                dsTicketingPNRResponse.Merge(dtTicketedTST.Copy()); // Already ticketd TST accounting purpose
                dsTicketingPNRResponse.Merge(ResultCode.Copy()); // Already ticketd TST accounting purpose

                #endregion//LIVE

                if (dsTicketingPNRResponse != null && dsTicketingPNRResponse.Tables.Count > 0 && dsTicketingPNRResponse.Tables.Contains("Result")
                    && dsTicketingPNRResponse.Tables["Result"].Rows.Count > 0)
                {
                    result[ds_booked] = strresponse;
                    if (dsTicketingPNRResponse.Tables.Contains("PassengerPNRDetails") && dsTicketingPNRResponse.Tables["PassengerPNRDetails"].Rows.Count > 0)
                    {
                        result[error] = "";
                        DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", xmlData, strClientID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
                    }
                    else
                    {
                        strErrorMsg = "Unable to process ticketing. Please contact customer care (#03).";

                        if (dsTicketingPNRResponse.Tables.Contains("ERROR") && dsTicketingPNRResponse.Tables["ERROR"].Rows.Count > 0 &&
                        !string.IsNullOrEmpty(dsTicketingPNRResponse.Tables["ERROR"].Rows[0]["ERRORTEXT"].ToString()))
                        {
                            strErrorMsg = dsTicketingPNRResponse.Tables["ERROR"].Rows[0]["ERRORTEXT"].ToString();
                        }
                        else if (!string.IsNullOrEmpty(dsTicketingPNRResponse.Tables["Result"].Rows[0]["Code"].ToString()) && dsTicketingPNRResponse.Tables["Result"].Rows[0]["Code"].ToString() == "3")
                        {
                            strErrorMsg = dsTicketingPNRResponse.Tables["Result"].Rows[0]["ErrorDescription"].ToString();
                            result[fare_change] = "TRUE";
                        }
                        else if (!string.IsNullOrEmpty(dsTicketingPNRResponse.Tables["Result"].Rows[0]["ErrorDescription"].ToString()))
                        {
                            strErrorMsg = dsTicketingPNRResponse.Tables["Result"].Rows[0]["ErrorDescription"].ToString();
                        }
                        else if (!string.IsNullOrEmpty(dsTicketingPNRResponse.Tables["Result"].Rows[0]["ErrorDisplay"].ToString()))
                        {
                            strErrorMsg = dsTicketingPNRResponse.Tables["Result"].Rows[0]["ErrorDisplay"].ToString();
                        }

                        result[error] = strErrorMsg;
                        xmlData = string.Empty;
                        xmlData = "<EVENT>" + "<RESPONSE>Ticketing_QueuePNR_Details</RESPONSE>" +
                                            "<STATUS>FAILED</STATUS>" +
                                            "<ERRORMSG>" + strErrorMsg + "</ERRORMSG>" +
                                            "</EVENT>";
                        DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", xmlData, strClientID, strPosTID, sequnceID);
                    }
                }
                else
                {
                    strErrorMsg = "Unable to process ticketing. Please contact customer care (#03).";
                    result[error] = strErrorMsg;
                    xmlData = string.Empty;
                    xmlData = "<EVENT>" + "<RESPONSE>Ticketing_QueuePNR_Details</RESPONSE>" +
                                        "<STATUS>FAILED</STATUS>" +
                                        "<ERRORMSG>" + strErrorMsg + "</ERRORMSG>" +
                                        "</EVENT>";
                    DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", xmlData, strClientID, strPosTID, sequnceID);
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.Retrievetcktlog(strUserName, "X", "RetrieveBookingController.cs", "Ticketing_QueuePNR_Details", ex.ToString(), strClientID, strPosTID, sequnceID);
                result[error] = "Problem occured while ticketing!";
                return Json(new { Status = "0", Message = result[error], Result = result });
            }
            result[clear_flag] = ClearFlag;
            // return result;

            return Json(new { Status = "1", Message = "", Result = result });
        }

        public bool Check_and_fetch_queuePNR(string CRSPNR, string agentID, string userName, string ipAddress,
            string terminalType, decimal sequenceID, ref string strErrorMsg, ref DataTable dtTcktPNRDetails, ref DataTable dtAccountPNRDetails)
        {
            DataSet dsPNRDetails = new DataSet();
            Hashtable my_param = new Hashtable();
            my_param.Add("CRS_PNR", CRSPNR);
            bool Result = false;
            string strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            try
            {
                DataTable dtResult = new DataTable();
                DataTable dtTPNRDetails = new DataTable();

                dtAccountPNRDetails = new DataTable();
                // Result = DBHandler.dsResultProcedure(SelectProcedure.Fetch_CRSPNR_INFO, "P", my_param, ref dsPNRDetails, ref strErrorMsg);
                Result = true;
                if (Result == true && dsPNRDetails != null && dsPNRDetails.Tables.Count > 0 && dsPNRDetails.Tables[0].Rows.Count > 0)
                {
                    Result = true;
                    int RowNos = 0;
                    var QryTSTCount = (from p in dtTcktPNRDetails.AsEnumerable()
                                       select new
                                       {
                                           TSTCOUNT = p.Field<string>("TSTCOUNT").Trim()
                                       }).Distinct();

                    DataTable dtTSTCount = ConvertToDataTable(QryTSTCount);
                    foreach (DataRow drRows in dtTSTCount.Rows)
                    {
                        var QryTST = (from p in dtTcktPNRDetails.AsEnumerable()
                                      where p["TSTCOUNT"].ToString().Trim() == drRows["TSTCOUNT"].ToString().Trim()
                                      select new
                                      {
                                          TICKETNO = p.Field<string>("TICKETNO").Trim()
                                      }).Distinct();
                        DataTable dtTST = ConvertToDataTable(QryTST);
                        bool PNRExist = false;
                        string TSTCOUNT = string.Empty;
                        foreach (DataRow drRowsTemp in dsPNRDetails.Tables[0].Rows)
                        {
                            if (dtTST.Rows[0]["TICKETNO"].ToString().ToUpper().Trim() == drRowsTemp["TICKETNO"].ToString().ToUpper().Trim())
                            {
                                TSTCOUNT = drRows["TSTCOUNT"].ToString().ToUpper().Trim();
                                PNRExist = true;
                                break;
                            }
                        }
                        if (PNRExist == true)
                        {
                            var qryPNRTemp = (from p in dtTcktPNRDetails.AsEnumerable()
                                              where p["TSTCOUNT"].ToString().ToUpper().Trim() == TSTCOUNT
                                              select p);
                            DataView dvPNRTemp = qryPNRTemp.AsDataView();
                            DataTable dtDetails = new DataTable();
                            if (dvPNRTemp.Count > 0)
                            {
                                dtDetails = qryPNRTemp.CopyToDataTable();

                                dtResult.Merge(dtDetails.Copy());
                            }
                        }

                        RowNos++;
                    }
                    var qryPaxOld = dtTcktPNRDetails.AsEnumerable().Select(OldPax => new { TSTCOUNT = OldPax["TSTCOUNT"].ToString() });
                    var qryPaxNew = dtResult.AsEnumerable().Select(NewPax => new { TSTCOUNT = NewPax["TSTCOUNT"].ToString() });
                    var exceptOldNew = qryPaxOld.Except(qryPaxNew);

                    if (exceptOldNew.Count() > 0)
                    {
                        dtAccountPNRDetails = (from p in dtTcktPNRDetails.AsEnumerable()
                                               join OldPaxNewPax in exceptOldNew on p["TSTCOUNT"].ToString() equals OldPaxNewPax.TSTCOUNT
                                               select p).CopyToDataTable();
                    }
                    dtTcktPNRDetails = dtResult;
                }
                else
                {
                    dtAccountPNRDetails = dtTcktPNRDetails;
                    dtTcktPNRDetails = new DataTable();
                    Result = true;
                }
            }
            catch (Exception ex)
            {
                Result = false;
                strErrorMsg = "Problem occured while check the PNR details";
                string ErrorMsg = ex.Message.ToString() + ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line")).ToString();
                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "X", "RetrieveBookingController.cs", "CHECK_AND_FETCH_QUEUEPNR", ErrorMsg, strPosID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
            }
            return Result;
        }

        public bool CalculatingAccountsForQueueTicketing(string terminalType, string agentID, string ipAddress,
            string userName, decimal sequenceID, DataSet dsAPIPNRDetails, ref string strErrorMsg, ref decimal dcTotalFare,
            ref string strAccountInfo, bool RebookFlag, string Flag, string strfareFlag)
        {
            strErrorMsg = string.Empty;
            string strOutputMsg = string.Empty;
            bool Result = false;
            DataSet dsAirlineMarkup = new DataSet();
            DataSet dsAgentDetails = new DataSet();
            DataTable dtPNRDetails = new DataTable("PassengerPNRDetails");
            DataTable dtTicketedPNRs = new DataTable();
            //string strfareFlag = string.Empty;
            string strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            try
            {
                string IPaddress = ControllerContext.HttpContext.Request.UserHostAddress.ToString();

                StringWriter strWrite = new StringWriter();
                dsAPIPNRDetails.WriteXml(strWrite);

                string logxmlData = "<EVENT><REQUEST>QUEUE_ACOUNT_REQUEST</REQUEST><TICKETRESERVATION>" + strWrite + "</TICKETRESERVATION></EVENT>";
                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "E", "RetrieveBookingController.cs", "Fetch_QueuePNR_Details", logxmlData.ToString(), strPosID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");

                string AgentType = dsAPIPNRDetails.Tables["AGENT_INFO"].Rows[0]["AGENT_TYPE"].ToString().ToUpper().Trim();
                string TDSPercentage = dsAPIPNRDetails.Tables["AGENT_INFO"].Rows[0]["TDS_PERCENTAGE"].ToString().ToUpper().Trim();
                string BranchID = dsAPIPNRDetails.Tables["AGENT_INFO"].Rows[0]["BRANCHID"].ToString().ToUpper().Trim();
                string HeaderBranchID = dsAPIPNRDetails.Tables["AGENT_INFO"].Rows[0]["HEADER_BRANCHID"].ToString().ToUpper().Trim();
                string PaymentMode = dsAPIPNRDetails.Tables["AGENT_INFO"].Rows[0]["PAYMENTMODE"].ToString().ToUpper().Trim();
                string CRSID = dsAPIPNRDetails.Tables["AGENT_INFO"].Rows[0]["CRS_ID"].ToString().ToUpper().Trim();
                string AirportType = dsAPIPNRDetails.Tables["AGENT_INFO"].Rows[0]["AIRPORT_TYPE"].ToString().ToUpper().Trim();


                //if (dsAPIPNRDetails != null && dsAPIPNRDetails.Tables.Count > 0 && dsAPIPNRDetails.Tables[0].Rows.Count > 0)
                //{
                //    strfareFlag = dsAPIPNRDetails.Tables[0].Rows[0]["IsFare"].ToString().ToUpper().Trim();
                //}


                if (Flag == null || Flag != "PNR")
                {
                    var qryTicketTemp = from p in dsAPIPNRDetails.Tables["PassengerPNRDetails"].AsEnumerable()// Non ticketed PNR's
                                        where p.Field<string>("TICKETNO").ToString() == ""
                                        select p;
                    DataView dvTemp = qryTicketTemp.AsDataView();
                    if (dvTemp.Count > 0)
                        dtPNRDetails = qryTicketTemp.CopyToDataTable();

                    qryTicketTemp = from p in dsAPIPNRDetails.Tables["PassengerPNRDetails"].AsEnumerable()// Ticketed PNR's
                                    where p.Field<string>("TICKETNO").ToString() != ""
                                    select p;
                    dvTemp = qryTicketTemp.AsDataView();
                    if (dvTemp.Count > 0)
                        dtTicketedPNRs = qryTicketTemp.CopyToDataTable();
                    dtTicketedPNRs.TableName = "PASSENGER_TICKETINFO";
                    if (dsAPIPNRDetails.Tables.Contains("PASSENGER_TICKETINFO")) // already ticketed pnr appended
                        dtTicketedPNRs.Merge(dsAPIPNRDetails.Tables["PASSENGER_TICKETINFO"].Copy());
                }
                else
                {
                    dtPNRDetails = dsAPIPNRDetails.Tables["PassengerPNRDetails"];
                }

                if (dtPNRDetails != null && dtPNRDetails.Rows.Count > 0)
                {
                    if (!dtPNRDetails.Columns.Contains("PLATINGCARRIER"))
                    {
                        dtPNRDetails.Columns.Add("PLATINGCARRIER");

                        foreach (DataRow dr in dtPNRDetails.Rows)
                        {
                            dr["PLATINGCARRIER"] = dr["TICKETINGCARRIER"].ToString().Trim();
                        }
                    }
                    //
                    DataTable dtTST = new DataTable();
                    if (strfareFlag.ToUpper().Trim() == "N")
                    {
                        var qryTST = (from p in dtPNRDetails.AsEnumerable()
                                      select new
                                      {
                                          PLATINGCARRIER = p.Field<string>("PLATINGCARRIER")
                                      }).Distinct();
                        if (qryTST.Count() > 0)
                        {
                            dtTST = ConvertToDataTable(qryTST);
                        }
                    }
                    else
                    {
                        var qryTST = (from p in dtPNRDetails.AsEnumerable()
                                      select new
                                      {
                                          TSTCOUNT = p.Field<string>("TSTCOUNT")
                                      }).Distinct();
                        if (qryTST.Count() > 0)
                        {
                            dtTST = ConvertToDataTable(qryTST);
                        }
                    }

                    if (dtTST.Rows.Count > 0)
                    {
                        // DataTable dtTST = ConvertToDataTable(qryTST);
                        string xmlData = "<PAXDETAILS_FARE>";
                        string paxXmlData = string.Empty;
                        DataTable dtPassenger = new DataTable("PAX_INFO");
                        dtPassenger.TableName = "PAX_INFO";
                        foreach (DataRow drRows in dtTST.Rows)
                        {
                            decimal dcGrossFare = 0;
                            decimal dcAPIGrossFare = 0;
                            string APITotalFareTag = string.Empty;
                            DataTable dtAdult = new DataTable();
                            DataTable dtChild = new DataTable();
                            DataTable dtInfant = new DataTable();

                            var QrySegment = (from p in dtPNRDetails.AsEnumerable()
                                              where (strfareFlag.ToUpper().Trim() == "Y" ? p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim() : p.Field<string>("PLATINGCARRIER").ToString().ToUpper().Trim() == drRows["PLATINGCARRIER"].ToString().ToUpper().Trim())
                                              select new
                                              {
                                                  ORIGIN = p.Field<string>("ORIGIN").Trim(),
                                                  DESTINATION = p.Field<string>("DESTINATION").Trim(),
                                                  AIRLINECODE = p.Field<string>("AIRLINECODE").Trim(),
                                                  //FAREBASISCODE = p.Field<string>("FAREBASIS").Trim(),
                                                  //CLASS = p.Field<string>("CLASS").Trim(),
                                                  DEPARTUREDATE = p.Field<string>("DEPARTUREDATE").Trim(),
                                                  ARRIVALDATE = p.Field<string>("ARRIVALDATE").Trim(),
                                                  DEPARTURETIME = p.Field<string>("DEPARTURETIME").Trim(),
                                                  ARRIVALTIME = p.Field<string>("ARRIVALTIME").Trim(),
                                                  FLIGHTNO = p.Field<string>("FLIGHTNO").Trim(),
                                                  AIRLINECATEGORY = p.Field<string>("AIRLINECATEGORY").Trim(),
                                                  PLATINGCARRIER = p.Field<string>("PLATINGCARRIER").Trim()
                                              }).Distinct();

                            //dtSegment = Utilities.ConvertToDataTable(QrySegment);
                            DataTable dtTSTSegment = ConvertToDataTable(QrySegment);

                            string PalttingCarrier = dtTSTSegment.Rows[0]["PLATINGCARRIER"].ToString().ToUpper().Trim();
                            string AirlineCategory = dtTSTSegment.Rows[0]["AIRLINECATEGORY"].ToString().ToUpper().Trim();
                            string strDepDate = dtTSTSegment.Rows[0]["DEPARTUREDATE"].ToString().ToUpper().Trim();

                            var qryAdtCount = (from p in dtPNRDetails.AsEnumerable()
                                               where p.Field<string>("PAXTYPE") == "ADT" &&
                                               (strfareFlag.ToUpper().Trim() == "Y" ? p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim() : p.Field<string>("PLATINGCARRIER").ToString().ToUpper().Trim() == drRows["PLATINGCARRIER"].ToString().ToUpper().Trim())
                                               //orderby Convert.ToInt32(p.Field<string>("SEQNO")) ascending
                                               orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                                               select new
                                               {
                                                   // SEQNO = p.Field<string>("SEQNO"),
                                                   PAXNO = p.Field<string>("PAXNO"),
                                                   BASEAMT = p.Field<string>("BASEAMT"),
                                                   GROSSAMT = p.Field<string>("GROSSAMT"),
                                                   TOTALTAXAMT = p.Field<string>("TOTALTAXAMT"),
                                                   TAXBREAKUP = p.Field<string>("TAXBREAKUP"),
                                                   PAXTYPE = p.Field<string>("PAXTYPE"),
                                                   PAXREFERENCE = p.Field<string>("PAXREFERENCE"),
                                                   //Platingcarrier = p.Field<string>("PLATINGCARRIER").Trim()
                                               }).Distinct();

                            dtAdult = ConvertToDataTable(qryAdtCount);

                            var qryChdCount = (from p in dtPNRDetails.AsEnumerable()
                                               where p.Field<string>("PAXTYPE") == "CHD" &&
                                               (strfareFlag.ToUpper().Trim() == "Y" ? p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim() : p.Field<string>("PLATINGCARRIER").ToString().ToUpper().Trim() == drRows["PLATINGCARRIER"].ToString().ToUpper().Trim())
                                               //orderby Convert.ToInt32(p.Field<string>("SEQNO")) ascending
                                               orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                                               select new
                                               {
                                                   //SEQNO = p.Field<string>("SEQNO"),
                                                   PAXNO = p.Field<string>("PAXNO"),
                                                   BASEAMT = p.Field<string>("BASEAMT"),
                                                   GROSSAMT = p.Field<string>("GROSSAMT"),
                                                   TOTALTAXAMT = p.Field<string>("TOTALTAXAMT"),
                                                   TAXBREAKUP = p.Field<string>("TAXBREAKUP"),
                                                   PAXTYPE = p.Field<string>("PAXTYPE"),
                                                   //PLATINGCARRIER = p.Field<string>("PLATINGCARRIER").Trim(),
                                                   PAXREFERENCE = p.Field<string>("PAXREFERENCE"),
                                               }).Distinct();

                            dtChild = ConvertToDataTable(qryChdCount);

                            var qryInfCount = (from p in dtPNRDetails.AsEnumerable()
                                               where p.Field<string>("PAXTYPE") == "INF" &&
                                               (strfareFlag.ToUpper().Trim() == "Y" ? p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim() : p.Field<string>("PLATINGCARRIER").ToString().ToUpper().Trim() == drRows["PLATINGCARRIER"].ToString().ToUpper().Trim())
                                               //                                           orderby Convert.ToInt32(p.Field<string>("SEQNO")) ascending
                                               orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                                               select new
                                               {
                                                   //SEQNO = p.Field<string>("SEQNO"),
                                                   PAXNO = p.Field<string>("PAXNO"),
                                                   BASEAMT = p.Field<string>("BASEAMT"),
                                                   GROSSAMT = p.Field<string>("GROSSAMT"),
                                                   TOTALTAXAMT = p.Field<string>("TOTALTAXAMT"),
                                                   TAXBREAKUP = p.Field<string>("TAXBREAKUP"),
                                                   PAXTYPE = p.Field<string>("PAXTYPE"),
                                                   PAXREFERENCE = p.Field<string>("PAXREFERENCE"),
                                                   //PLATINGCARRIER = p.Field<string>("PLATINGCARRIER").Trim()
                                               }).Distinct();

                            dtInfant = ConvertToDataTable(qryInfCount);

                            int adultCount = int.Parse(qryAdtCount.Count().ToString());
                            int childCount = int.Parse(qryChdCount.Count().ToString());
                            int infantCount = int.Parse(qryInfCount.Count().ToString());

                            string BasicFare = string.Empty; string TotalFareTag = string.Empty; string taxBreakUpTag = string.Empty;
                            decimal SCTaxCurrent = 0; string strTaxAmount = string.Empty; string taxBreakUp = string.Empty;

                            //if (strfareFlag == "Y")
                            //{
                            if (adultCount > 0)
                                BasicFare += dtAdult.Rows[0]["BASEAMT"].ToString() + "|";
                            else
                                BasicFare += "0|";

                            if (childCount > 0)
                                BasicFare += dtChild.Rows[0]["BASEAMT"].ToString() + "|";
                            else
                                BasicFare += "0|";
                            if (infantCount > 0)
                                BasicFare += dtInfant.Rows[0]["BASEAMT"].ToString() + "|";
                            else
                                BasicFare += "0";


                            if (adultCount > 0)
                                taxBreakUp += "BF:" + dtAdult.Rows[0]["BASEAMT"].ToString() + "/" + dtAdult.Rows[0]["TAXBREAKUP"].ToString() + "|";
                            else
                                taxBreakUp += "BF:0/|";
                            if (childCount > 0)
                                taxBreakUp += "BF:" + dtChild.Rows[0]["BASEAMT"].ToString() + "/" + dtChild.Rows[0]["TAXBREAKUP"].ToString() + "|";
                            else
                                taxBreakUp += "BF:0/|";
                            if (infantCount > 0)
                                taxBreakUp += "BF:" + dtInfant.Rows[0]["BASEAMT"].ToString() + "/" + dtInfant.Rows[0]["TAXBREAKUP"].ToString() + "|";
                            else
                                taxBreakUp += "BF:0/";


                            if (adultCount > 0)
                                TotalFareTag += dtAdult.Rows[0]["GROSSAMT"].ToString() + "|";
                            else
                                TotalFareTag += "0|";
                            if (childCount > 0)
                                TotalFareTag += dtChild.Rows[0]["GROSSAMT"].ToString() + "|";
                            else
                                TotalFareTag += "0|";
                            if (infantCount > 0)
                                TotalFareTag += dtInfant.Rows[0]["GROSSAMT"].ToString() + "|";
                            else
                                TotalFareTag += "0";


                            if (adultCount > 0)
                                strTaxAmount += dtAdult.Rows[0]["TOTALTAXAMT"].ToString() + "|";
                            else
                                strTaxAmount += "0|";
                            if (childCount > 0)
                                strTaxAmount += dtChild.Rows[0]["TOTALTAXAMT"].ToString() + "|";
                            else
                                strTaxAmount += "0|";
                            if (infantCount > 0)
                                strTaxAmount += dtInfant.Rows[0]["TOTALTAXAMT"].ToString() + "|";
                            else
                                strTaxAmount += "0";

                            APITotalFareTag = TotalFareTag;

                            //bool Markup = Fetch_Airline_Markup_Details(agentID, userName, ipAddress, terminalType, sequenceID,
                            //                                     ref dsAirlineMarkup, ref strErrorMsg);
                            //if (Markup == true)
                            //{
                            //    if (dsAirlineMarkup != null && dsAirlineMarkup.Tables.Count > 0)
                            //    {
                            //        bool boolHavSC = false;
                            //        decimal SCTaxExist = 0;
                            //        int connectingCount = QrySegment.Count();

                            //        string strtaxBreakUp = CheckSCTax(taxBreakUp, PalttingCarrier,
                            //             connectingCount, ref boolHavSC, ref SCTaxExist, ref SCTaxCurrent, "O", AirportType, adultCount,
                            //             childCount, infantCount, "N", dsAirlineMarkup.Tables[0]);

                            //        TotalFareTag = EditAmount(TotalFareTag, SCTaxExist, SCTaxCurrent);
                            //        strTaxAmount = EditAmount(strTaxAmount, SCTaxExist, SCTaxCurrent);
                            //    }
                            //}
                            //else
                            //{
                            //    return false;
                            //}
                            //}
                            DataSet dsServiceCharge = new DataSet();
                            string MgmtTag = "0|0|0";
                            decimal MgmtAmount = 0;

                            //bool MgmtFees = Fetch_ServiceCharge(AirportType, agentID, userName, ipAddress,
                            //    terminalType, sequenceID, ref strErrorMsg, ref dsServiceCharge);
                            //if (MgmtFees == true)
                            //{
                            //    if (dsServiceCharge != null && dsServiceCharge.Tables.Count > 0)
                            //    {
                            //        MgmtAmount = ServiceChargeForAirlines(PalttingCarrier, "ADT", adultCount,
                            //                     childCount, infantCount, ref MgmtTag, dsServiceCharge.Tables[0]);
                            //    }
                            //}
                            //else
                            //{
                            //    return false;
                            //}

                            decimal totalAmountIncentive = 0;
                            decimal totalAmountCommision = 0;
                            decimal TotalPLBAmount = 0;
                            string CompCommisionAmt = string.Empty;
                            string incentiveCommision = string.Empty;
                            string PLB_PaxAmount = string.Empty;
                            string incentiveReferenceID = string.Empty;
                            string commisionReferenceID = string.Empty;
                            string compCommissionReferenceID = string.Empty;
                            string PLBReferenceID = string.Empty;
                            //GetCommissionDetails(PalttingCarrier, AirportType, AirlineCategory,
                            //agentID, userName, ipAddress, terminalType,
                            //   sequenceID, "N", ref totalAmountIncentive, ref totalAmountCommision,
                            //   ref incentiveCommision, ref CompCommisionAmt, ref incentiveReferenceID, ref commisionReferenceID,
                            //   ref compCommissionReferenceID, ref strOutputMsg, ref strErrorMsg, ref PLBReferenceID, ref PLB_PaxAmount, ref TotalPLBAmount,
                            //   adultCount, childCount, infantCount, taxBreakUp, TotalFareTag, strTaxAmount, AgentType, sequenceID, strDepDate, dtPNRDetails);
                            // Create Pax Track Data...

                            if (strfareFlag == "Y")
                            {
                                dcGrossFare = ((SplitFareByAdultPax(TotalFareTag) * adultCount) +
                                                (SplitFareByChildPax(TotalFareTag) * childCount) +
                                                            (SplitFareByInfantPax(TotalFareTag) * infantCount));

                                dcAPIGrossFare = ((SplitFareByAdultPax(APITotalFareTag) * adultCount) +
                                                (SplitFareByChildPax(APITotalFareTag) * childCount) +
                                                            (SplitFareByInfantPax(APITotalFareTag) * infantCount));
                            }

                            xmlData += "<PAXDETAILS>";
                            xmlData += "<STATUS>BOOKING</STATUS>";
                            xmlData += "<ADULTCOUNT>" + adultCount + "</ADULTCOUNT>";
                            xmlData += "<CHILDCOUNT>" + childCount + "</CHILDCOUNT>";
                            xmlData += "<INFANTCOUNT>" + infantCount + "</INFANTCOUNT>";
                            xmlData += "<CONTACTNO>" + "" + "</CONTACTNO>";
                            xmlData += "<EMAILID>" + "" + "</EMAILID>";
                            xmlData += "<AGENTID>" + agentID + "</AGENTID>";
                            xmlData += "<USERNAME>" + userName + "</USERNAME>";
                            xmlData += "<SEQUENCEID>" + sequenceID + "</SEQUENCEID>";
                            xmlData += "<TOTALFARETAG>" + TotalFareTag + "</TOTALFARETAG>";
                            xmlData += "<GROSSFARE>" + dcGrossFare + "</GROSSFARE>";
                            xmlData += "<API_GROSSFARE>" + dcAPIGrossFare + "</API_GROSSFARE>";
                            xmlData += "<TAXAMOUNT>" + strTaxAmount + "</TAXAMOUNT>";
                            xmlData += "<TAXBREAKUP>" + taxBreakUp + "</TAXBREAKUP>";
                            xmlData += "<BASIC_FARE_TAG>" + BasicFare + "</BASIC_FARE_TAG>";
                            xmlData += "<COMMISIONREFERENCEID>" + commisionReferenceID + "</COMMISIONREFERENCEID>";
                            xmlData += "<INCENTIVEREFERENCEID>" + incentiveReferenceID + "</INCENTIVEREFERENCEID>";
                            xmlData += "<COMPCOMMISSIONREFERENCEID>" + compCommissionReferenceID + "</COMPCOMMISSIONREFERENCEID>";
                            xmlData += "<PLBREFERENCEID>" + PLBReferenceID + "</PLBREFERENCEID>";
                            xmlData += "<TOTALAMOUNTCOMMISION>" + totalAmountCommision + "</TOTALAMOUNTCOMMISION>";
                            xmlData += "<TOTALAMOUNTINCENTIVE>" + totalAmountIncentive + "</TOTALAMOUNTINCENTIVE>";
                            xmlData += "<TOTALPLBAMOUNT>" + TotalPLBAmount + "</TOTALPLBAMOUNT>";
                            xmlData += "<INCENTIVECOMMISION>" + incentiveCommision + "</INCENTIVECOMMISION>";
                            xmlData += "<COMPCOMMISIONAMT>" + CompCommisionAmt + "</COMPCOMMISIONAMT>";
                            xmlData += "<PLB_PAXAMOUNT>" + PLB_PaxAmount + "</PLB_PAXAMOUNT>";
                            xmlData += "<SCTAX>" + SCTaxCurrent + "</SCTAX>";
                            xmlData += "<MANAGEMENT_FEE_TAG>" + MgmtTag + "</MANAGEMENT_FEE_TAG>";
                            xmlData += "<TOTAL_MANAGEMENT_FEE>" + MgmtAmount + "</TOTAL_MANAGEMENT_FEE>";
                            xmlData += "<TDS_PERCENTAGE>" + TDSPercentage + "</TDS_PERCENTAGE>";
                            xmlData += "<PAYMENTMODE>" + PaymentMode + "</PAYMENTMODE>";
                            xmlData += "<BRANCHID>" + BranchID + "</BRANCHID>";
                            xmlData += "<MASTER_BRANCHID>" + HeaderBranchID + "</MASTER_BRANCHID>";
                            xmlData += "<CRSID>" + CRSID + "</CRSID>";
                            if (strfareFlag == "Y")
                            {
                                xmlData += "<TSTCOUNT>" + drRows["TSTCOUNT"].ToString().ToUpper().Trim() + "</TSTCOUNT>";
                            }
                            else
                            {
                                xmlData += "<TICKETINGCARRIER>" + drRows["PLATINGCARRIER"].ToString().ToUpper().Trim() + "</TICKETINGCARRIER>";
                                xmlData += "<PLATINGCARRIER>" + drRows["PLATINGCARRIER"].ToString().ToUpper().Trim() + "</PLATINGCARRIER>";
                            }
                            xmlData += "</PAXDETAILS>";

                            DataTable dtTempPassenger = new DataTable();
                            if (strfareFlag.ToUpper().Trim() == "N")
                            {
                                var qryPassenger = (from p in dtPNRDetails.AsEnumerable()
                                                    where p.Field<string>("PLATINGCARRIER").ToString().ToUpper().Trim() == drRows["PLATINGCARRIER"].ToString().ToUpper().Trim()
                                                    orderby p["PAXTYPE"] ascending// orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                                                    select new
                                                    {
                                                        TITLE = ((p.Field<string>("TITLE") == null || string.IsNullOrEmpty(p.Field<string>("TITLE"))) ? "" : p.Field<string>("TITLE").Trim()),
                                                        FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                                        LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                                        DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                                                        PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                                                        PAXNO = p.Field<string>("PAXNO").Trim(),
                                                        TICKETNO = p.Field<string>("TICKETNO").Trim(),
                                                        PAXREFERENCE = p.Field<string>("PAXREFERENCE"),
                                                        PLATINGCARRIER = p.Field<string>("PLATINGCARRIER").Trim()
                                                    }).Distinct();
                                dtTempPassenger = ConvertToDataTable(qryPassenger);
                            }
                            else
                            {
                                var qryPassenger = (from p in dtPNRDetails.AsEnumerable()
                                                    where p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim()
                                                    orderby p["PAXTYPE"] ascending//orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                                                    select new
                                                    {
                                                        TITLE = ((p.Field<string>("TITLE") == null || string.IsNullOrEmpty(p.Field<string>("TITLE"))) ? "" : p.Field<string>("TITLE").Trim()),
                                                        FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                                        LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                                        DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                                                        PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                                                        PAXNO = p.Field<string>("PAXNO").Trim(),
                                                        TICKETNO = p.Field<string>("TICKETNO").Trim(),
                                                        PAXREFERENCE = p.Field<string>("PAXREFERENCE"),
                                                        TSTCOUNT = p.Field<string>("TSTCOUNT").Trim()
                                                    }).Distinct();
                                dtTempPassenger = ConvertToDataTable(qryPassenger);
                            }
                            //var qryPassenger = (from p in dtPNRDetails.AsEnumerable()
                            //                    where (strfareFlag.ToUpper().Trim() == "Y" ? p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == drRows["TSTCOUNT"].ToString().ToUpper().Trim() : p.Field<string>("PLATINGCARRIER").ToString().ToUpper().Trim() == drRows["PLATINGCARRIER"].ToString().ToUpper().Trim())
                            //                    //orderby Convert.ToInt32(p.Field<string>("SEQNO")) ascending
                            //                    orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                            //                    select new
                            //                    {
                            //                        TITLE = ((p.Field<string>("TITLE") == null || string.IsNullOrEmpty(p.Field<string>("TITLE"))) ? "" : p.Field<string>("TITLE").Trim()),
                            //                        FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                            //                        LASTNAME = p.Field<string>("LASTNAME").Trim(),
                            //                        DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                            //                        PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                            //                        //SEQNO = p.Field<string>("SEQNO").Trim(),
                            //                        PAXNO = p.Field<string>("PAXNO").Trim(),
                            //                        TSTCOUNT = p.Field<string>("TSTCOUNT").Trim(),
                            //                        TICKETNO = p.Field<string>("TICKETNO").Trim(),
                            //                        PAXREFERENCE = p.Field<string>("PAXREFERENCE"),
                            //                        PLATINGCARRIER = p.Field<string>("PLATINGCARRIER").Trim()
                            //                    }).Distinct();
                            //DataTable dtTempPassenger = ConvertToDataTable(qryPassenger);
                            if (string.IsNullOrEmpty(dtTempPassenger.Rows[0]["TICKETNO"].ToString().Trim()))
                                dcTotalFare += dcGrossFare;

                            dtPassenger.Merge(dtTempPassenger.Copy());
                        }
                        xmlData += "</PAXDETAILS_FARE>";

                        // Insert the UserTrackID
                        string strErrorMessage = string.Empty;
                        string trackIDStatus = string.Empty;
                        StringReader XmlInsenstrUpdate = new StringReader(xmlData);
                        XmlTextReader xmlInsentxtUpdate = new XmlTextReader(XmlInsenstrUpdate);
                        DataSet dsXmlData = new DataSet(); dsXmlData.ReadXml(xmlInsentxtUpdate);

                        if (dsXmlData != null && dsXmlData.Tables.Count > 0)
                        {
                            if (!dsAPIPNRDetails.Tables["AGENT_INFO"].Columns.Contains("TOTALFARE"))
                                dsAPIPNRDetails.Tables["AGENT_INFO"].Columns.Add("TOTALFARE");

                            dsAPIPNRDetails.Tables["AGENT_INFO"].Rows[0]["TOTALFARE"] = dcTotalFare;

                            dtPNRDetails.TableName = "PassengerPNRDetails";
                            dsXmlData.Tables.Add(dtPassenger.Copy());
                            dsXmlData.Tables.Add(dtPNRDetails.Copy());
                            dsXmlData.Tables.Add(dtTicketedPNRs.Copy());
                            dsXmlData.Tables.Add(dsAPIPNRDetails.Tables["AGENT_INFO"].Copy());
                            if (dsAPIPNRDetails.Tables.Contains("PAX_OLD_INFO"))
                                dsXmlData.Tables.Add(dsAPIPNRDetails.Tables["PAX_OLD_INFO"].Copy());
                            if (dsAPIPNRDetails.Tables.Contains("PAXDETAILS_OLD_INFO"))
                                dsXmlData.Tables.Add(dsAPIPNRDetails.Tables["PAXDETAILS_OLD_INFO"].Copy());
                            if (dsAPIPNRDetails.Tables.Contains("PASSENGER_OLD_PNRINFO"))
                                dsXmlData.Tables.Add(dsAPIPNRDetails.Tables["PASSENGER_OLD_PNRINFO"].Copy());
                            if (dsAPIPNRDetails.Tables.Contains("TICKETED_DETAILS"))
                                dsXmlData.Tables.Add(dsAPIPNRDetails.Tables["TICKETED_DETAILS"].Copy());

                            strAccountInfo = JsonConvert.SerializeObject(dsXmlData);
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                        }
                    }
                    else
                    {
                        Result = false;
                    }
                    //
                }
                else
                {
                    strErrorMsg = "This PNR is already ticketed. please check and try later.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Result = false;
                strErrorMsg = "Problem occured while process the PNR details";
                string ErrorMsg = ex.Message.ToString() + ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line")).ToString();
                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "X", "RetrieveBookingController.cs", "CalculatingAccountsForQueueTicketing", ErrorMsg, strPosID, strPosTID, Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "");
            }
            return Result;
        }

        public ActionResult RetrieveFareRules(string hdf_pnrinfo, string TSTCOUNT,
            string hdffareinfo, string farequalifier, string faretoken, string FareType, string AirportTypes, string Corporatename, string Employeename, string EmpCostCenter,
            string EmpRefID, string BranchID, string SessionKey, string CRSPNR, string AirlinePNR, string CRSTYPE, string AirlineCategory, string Queuenumber, string strCorporateCode, string StrFareFlag)
        {
            string stu = string.Empty;
            string msg = string.Empty;
            string err = string.Empty;
            string UserName = string.Empty;
            string UserID = string.Empty;
            string CompanyID = string.Empty;
            string Ipaddress = string.Empty;
            string sequenceno = "0";
            string strClientID = Corporatename;
            string strPosID = string.Empty;
            string strPosTID = string.Empty;
            string strTerminalType = string.Empty;
            Queuenumber = SessionKey;
            ArrayList result = new ArrayList();
            result.Add("");
            result.Add("");
            result.Add("");
            result.Add("");
            result.Add("");
            result.Add("");//5
            result.Add("");
            result.Add("");
            result.Add("");
            result.Add("");

            int error = 0;

            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;

            string sequnceID = string.Empty;
            string strBranchId = string.Empty;
            string strTopUpBranchID = string.Empty;
            string strOfficeId = string.Empty;
            try
            {

                strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
                strUserName = Session["username"] != null ? Session["username"].ToString() : "";
                Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0";
                strBranchId = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                decimal dcTDSPercentage = 0;
                string AgentType = string.Empty;

                if (strAgentID == "" || strPosID == "" || strUserName == "")
                {
                    result[error] = "Your session has expired!";
                    DatabaseLog.Retrievetcktlog("Session Timeout", "E", "RetrieveBookingController.cs", "RetrieveFareRules", DateTime.Now.ToString("yyyyMMddHHmmssfff"), strClientID, strPosTID, DateTime.Now.ToString("yyyyMMdd"));
                    return Json(new { Status = "-1", Message = result[error], Result = result });
                }


                object objPNR = Session[hdf_pnrinfo.TrimEnd('#')];
                if (objPNR == null)
                {
                    result[error] = "Your session has expired!";
                    return Json(new { Status = "-1", Message = result[error], Result = result });
                }

                #region UsageLog
                try
                {
                    string strUsgLogRes = Base.Commonlog("RetrivePNRFareRule", "", "FETCH");
                }
                catch (Exception e) { }
                #endregion

               
                DataSet dsPNRINFO = ((DataSet)Session[hdf_pnrinfo]).Copy();

                string segmenType = string.Empty;
                segmenType = dsPNRINFO.Tables["AGENT_INFO"].Rows[0]["AIRPORT_TYPE"].ToString().ToUpper().Trim();


                var qryAdtCount = (from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                                   where p.Field<string>("PAXTYPE") == "ADT"
                                   orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                                   select new
                                   {
                                       FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                       LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                       DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                                       PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                                       PAXNO = p.Field<string>("PAXNO").Trim()
                                   }).Distinct();

                var qryChdCount = (from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                                   where p.Field<string>("PAXTYPE") == "CHD"
                                   orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                                   select new
                                   {
                                       FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                       LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                       DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                                       PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                                       PAXNO = p.Field<string>("PAXNO").Trim()
                                   }).Distinct();

                var qryInfCount = (from p in dsPNRINFO.Tables["PAX_INFO"].AsEnumerable()
                                   where p.Field<string>("PAXTYPE") == "INF"
                                   orderby Convert.ToInt32(p.Field<string>("PAXNO")) ascending
                                   select new
                                   {
                                       FIRSTNAME = p.Field<string>("FIRSTNAME").Trim(),
                                       LASTNAME = p.Field<string>("LASTNAME").Trim(),
                                       DATEOFBIRTH = (p.Field<string>("DATEOFBIRTH") == null || string.IsNullOrEmpty(p.Field<string>("DATEOFBIRTH"))) ? "" : p.Field<string>("DATEOFBIRTH").Trim(),
                                       PAXTYPE = p.Field<string>("PAXTYPE").Trim(),
                                       PAXNO = p.Field<string>("PAXNO").Trim()
                                   }).Distinct();

                int adultCount = int.Parse(qryAdtCount.Count().ToString());
                int childCount = int.Parse(qryChdCount.Count().ToString());
                int infantCount = int.Parse(qryInfCount.Count().ToString());

                DataTable dtTSTSegment = new DataTable();

                if (StrFareFlag == "N")
                {
                    var QrySegment = (from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                      where p.Field<string>("TICKETINGCARRIER").ToString().ToUpper().Trim() == TSTCOUNT
                                      group p by p.Field<string>("SEGMENTNO").ToString() into g
                                      select new
                                      {
                                          ORIGIN = g.FirstOrDefault().Field<string>("ORIGIN").Trim(),
                                          DESTINATION = g.FirstOrDefault().Field<string>("DESTINATION").Trim(),
                                          TSTREFERENCE = g.FirstOrDefault().Field<string>("TSTREFERENCE").Trim(),
                                          SEGMENTREFERENCE = g.FirstOrDefault().Field<string>("SEGMENTREFERENCE").Trim(),
                                          PAXREFERENCE = g.FirstOrDefault().Field<string>("PAXREFERENCE").Trim(),
                                          //PLATINGCARRIER = p.Field<string>("PLATINGCARRIER").Trim(),
                                          PLATINGCARRIER = g.FirstOrDefault().Field<string>("TICKETINGCARRIER").Trim(),
                                          FAREBASISCODE = g.FirstOrDefault().Field<string>("FAREBASISCODE").Trim(),
                                          TSTCOUNT = g.FirstOrDefault().Field<string>("TSTCOUNT").Trim(),
                                          GROSSAMT = g.FirstOrDefault().Field<string>("GROSSAMT").Trim(),
                                          SEGMENTNO = g.FirstOrDefault().Field<string>("SEGMENTNO").Trim(),
                                          TITLE = g.FirstOrDefault().Field<string>("TITLE").Trim(),
                                          FIRSTNAME = g.FirstOrDefault().Field<string>("FIRSTNAME").Trim(),
                                          LASTNAME = g.FirstOrDefault().Field<string>("LASTNAME").Trim(),
                                          DEPARTUREDATE = g.FirstOrDefault().Field<string>("DEPARTUREDATE").Trim(),
                                          ARRIVALDATE = g.FirstOrDefault().Field<string>("ARRIVALDATE").Trim(),
                                          DEPARTURETIME = g.FirstOrDefault().Field<string>("DEPARTURETIME").Trim(),
                                          ARRIVALTIME = g.FirstOrDefault().Field<string>("ARRIVALTIME").Trim(),
                                          CLASS = g.FirstOrDefault().Field<string>("CLASS").Trim(),
                                          FareID = g.FirstOrDefault().Field<string>("FareID").Trim(),
                                          FareQualifier = g.FirstOrDefault().Field<string>("FareQualifier").Trim(),
                                          FLIGHTNO = g.FirstOrDefault().Field<string>("FLIGHTNO").Trim(),
                                          AIRLINECATEGORY = g.FirstOrDefault().Field<string>("AIRLINECATEGORY").Trim()
                                      }).Distinct();
                    dtTSTSegment = ConvertToDataTable(QrySegment);
                }
                else
                {
                    var QrySegment = (from p in dsPNRINFO.Tables["PassengerPNRDetails"].AsEnumerable()
                                      where p.Field<string>("TSTCOUNT").ToString().ToUpper().Trim() == TSTCOUNT
                                      group p by p.Field<string>("SEGMENTNO").ToString() into g
                                      select new
                                      {
                                          ORIGIN = g.FirstOrDefault().Field<string>("ORIGIN").Trim(),
                                          DESTINATION = g.FirstOrDefault().Field<string>("DESTINATION").Trim(),
                                          TSTREFERENCE = g.FirstOrDefault().Field<string>("TSTREFERENCE").Trim(),
                                          SEGMENTREFERENCE = g.FirstOrDefault().Field<string>("SEGMENTREFERENCE").Trim(),
                                          PAXREFERENCE = g.FirstOrDefault().Field<string>("PAXREFERENCE").Trim(),
                                          //PLATINGCARRIER = p.Field<string>("PLATINGCARRIER").Trim(),
                                          PLATINGCARRIER = g.FirstOrDefault().Field<string>("TICKETINGCARRIER").Trim(),
                                          FAREBASISCODE = g.FirstOrDefault().Field<string>("FAREBASISCODE").Trim(),
                                          TSTCOUNT = g.FirstOrDefault().Field<string>("TSTCOUNT").Trim(),
                                          GROSSAMT = g.FirstOrDefault().Field<string>("GROSSAMT").Trim(),
                                          SEGMENTNO = g.FirstOrDefault().Field<string>("SEGMENTNO").Trim(),
                                          TITLE = g.FirstOrDefault().Field<string>("TITLE").Trim(),
                                          FIRSTNAME = g.FirstOrDefault().Field<string>("FIRSTNAME").Trim(),
                                          LASTNAME = g.FirstOrDefault().Field<string>("LASTNAME").Trim(),
                                          DEPARTUREDATE = g.FirstOrDefault().Field<string>("DEPARTUREDATE").Trim(),
                                          ARRIVALDATE = g.FirstOrDefault().Field<string>("ARRIVALDATE").Trim(),
                                          DEPARTURETIME = g.FirstOrDefault().Field<string>("DEPARTURETIME").Trim(),
                                          ARRIVALTIME = g.FirstOrDefault().Field<string>("ARRIVALTIME").Trim(),
                                          CLASS = g.FirstOrDefault().Field<string>("CLASS").Trim(),
                                          FareID = g.FirstOrDefault().Field<string>("FareID").Trim(),
                                          FareQualifier = g.FirstOrDefault().Field<string>("FareQualifier").Trim(),
                                          FLIGHTNO = g.FirstOrDefault().Field<string>("FLIGHTNO").Trim(),
                                          AIRLINECATEGORY = g.FirstOrDefault().Field<string>("AIRLINECATEGORY").Trim()
                                      }).Distinct();
                    dtTSTSegment = ConvertToDataTable(QrySegment);
                }


                #region Declare all Models
                FareRuleRQ _FareRuleRQ = new FareRuleRQ();
                AgentDetails _Agentdetail = new AgentDetails();
                RQFlights _Flts = new RQFlights();
                List<RQFlights> _lstFlts = new List<RQFlights>();
                FareRuleRS _FareRuleRS = new FareRuleRS();
                Segment Segment = new Segment();

                _Agentdetail.AgentID = strAgentID;
                _Agentdetail.TerminalID = strTerminalId;

                _Agentdetail.AppType = "B2B";
                _Agentdetail.UserName = strUserName;
                //_Agentdetail.BranchID = "39";
                _Agentdetail.BranchID = BranchID;
                _Agentdetail.ProductType = "RC";
                _Agentdetail.TicketingOfficeId = Queuenumber;
                _Agentdetail.ClientID = Corporatename;
                _Agentdetail.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                _Agentdetail.Environment = (strTerminalType == "T" ? "I" : "W");
                _Agentdetail.BOAID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                _Agentdetail.BOATerminalID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                _Agentdetail.AgentType = dsPNRINFO.Tables["AGENT_INFO"].Rows[0]["Agent_type"].ToString();
                _Agentdetail.CoOrdinatorID = "";
                _Agentdetail.IssuingBranchID = BranchID;
                _Agentdetail.EMP_ID = Employeename;
                _Agentdetail.COST_CENTER = EmpCostCenter;
                _Agentdetail.Ipaddress = Ipaddress;
                _Agentdetail.Airportid = AirportTypes;
                _Agentdetail.ProjectId = ConfigurationManager.AppSettings["ProjectCode"].ToString();
                _Agentdetail.Platform = "B";
                _Agentdetail.ProductID = "INRC02";
                _Agentdetail.APPCurrency = Session["Sesscurtype"] != null && Session["Sesscurtype"] != "" ? Session["Sesscurtype"].ToString() : "";
                _Agentdetail.PNROfficeId = SessionKey;

                //_AgnDet.IssuingBranchID = Session["BRANCH_ID"] != null ? Session["BRANCH_ID"].ToString() : "";
                //_AgnDet.EMP_ID = UserID.ToString();
                #endregion

                #region Flights
                foreach (DataRow drSegRows in dtTSTSegment.Rows)
                {
                    _Flts = new RQFlights();
                    _Flts.CarrierCode = drSegRows["PLATINGCARRIER"].ToString();
                    _Flts.FlightNumber = drSegRows["FLIGHTNO"].ToString();
                    _Flts.Origin = drSegRows["ORIGIN"].ToString();
                    _Flts.Destination = drSegRows["DESTINATION"].ToString();
                    _Flts.StartTerminal = "";
                    _Flts.EndTerminal = "";
                    _Flts.DepartureDateTime = drSegRows["DEPARTUREDATE"].ToString() + " " + drSegRows["DEPARTURETIME"].ToString();
                    _Flts.ArrivalDateTime = drSegRows["ARRIVALDATE"].ToString() + " " + drSegRows["ARRIVALTIME"].ToString();
                    _Flts.Class = drSegRows["CLASS"].ToString();
                    _Flts.Cabin = "";
                    _Flts.FareBasisCode = drSegRows["FAREBASISCODE"].ToString();
                    _Flts.AirlineCategory = AirlineCategory;
                    _Flts.PlatingCarrier = drSegRows["PLATINGCARRIER"].ToString();
                    _Flts.ReferenceToken = "";
                    _Flts.SegRef = drSegRows["SEGMENTREFERENCE"].ToString();
                    _Flts.ItinRef = drSegRows["TSTCOUNT"].ToString(); ;
                    _Flts.FareID = drSegRows["FareID"].ToString();
                    _Flts.CorporateCode = strCorporateCode;
                    // _Flts.SeatAvailFlag = lst[i].AvailSeat;
                    // _Flts.FareType = FareType;
                    //_Flts.OfflineFlag = 0;

                    _lstFlts.Add(_Flts);
                }
                #endregion

                #region Segment

                string strPLATINGCARRIER = string.Empty;
                foreach (DataRow drSegRows in dtTSTSegment.Rows)
                {

                    // Segment = new PricingSegmentDetails();

                    Segment.BaseOrigin = drSegRows["ORIGIN"].ToString();
                    Segment.BaseDestination = drSegRows["DESTINATION"].ToString(); ;
                    Segment.SegmentType = segmenType;
                    Segment.Adult = adultCount;
                    Segment.Child = childCount;
                    Segment.Infant = infantCount;
                    Segment.TripType = "";// lst[0].SegRef == "1" ? "R" : "O"; 

                }
                #endregion

                _FareRuleRQ.AgentDetail = _Agentdetail;
                _FareRuleRQ.FlightsDetails = _lstFlts;
                _FareRuleRQ.SegmentDetails = Segment;
                _FareRuleRQ.Stock = CRSTYPE;
                _FareRuleRQ.AirlinePNR = AirlinePNR;
                _FareRuleRQ.CRSPNR = CRSPNR;
                _FareRuleRQ.OflineFlag = true;
                _FareRuleRQ.FareType = FareType;
                _FareRuleRQ.CRSID = CRSTYPE;

                string StrInput = JsonConvert.SerializeObject(_FareRuleRQ);

                string useridunique = Session["UniqueIduser"] != null ? Session["UniqueIduser"].ToString() : ""; // by vijai for log purpose 16042018
                #region Request Log
                string ReqTime = "FareruleRequest_" + DateTime.Now;
                string LstrDetails = "<THREAD_REQUEST><FUNCTION>FARE RULE REQUEST</FUNCTION><QUERY>" + StrInput + "</QUERY><TIME>" + ReqTime + "</TIME></THREAD_REQUEST>";
                DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", useridunique + " ~ FARE RULES", LstrDetails, strClientID, strPosTID, sequenceno);
                #endregion
                string strURLpath = ConfigurationManager.AppSettings["QTKT_Service_URL"].ToString();
                MyWebClient Client = new MyWebClient();
                Client.Headers["Content-Type"] = "application/json";
                byte[] byteGetLogin = Client.UploadData(strURLpath + "/" + "GetFareRuleInfo", "POST", Encoding.ASCII.GetBytes(StrInput));
                string strResponse = Encoding.ASCII.GetString(byteGetLogin);

                _FareRuleRS = JsonConvert.DeserializeObject<FareRuleRS>(strResponse);

                if (_FareRuleRS.Status.ResultCode == "1")
                {
                    stu = "1";
                    msg = _FareRuleRS.FareRule.FareRuleText;
                }
                else
                {
                    stu = "0";
                    err = _FareRuleRS.Status.Error;
                }

                #region Response Log
                ReqTime = "FareruleResponse_" + DateTime.Now;
                LstrDetails = "<THREAD_RESPONSE><FUNCTION>FARE RULE RESPONSE</FUNCTION><QUERY>" + strResponse + "</QUERY><TIME>" + ReqTime + "</TIME></THREAD_RESPONSE>";
                DatabaseLog.Retrievetcktlog(strUserName, "E", "RetrieveBookingController.cs", "FARE RULES", LstrDetails, strAgentID, strTerminalId, sequenceno);
                #endregion
            }
            catch (Exception ex)
            {
                stu = "0";

                if (ex.Message.ToString().ToUpper().Trim().Contains("THE OPERATION HAS TIMED OUT"))
                {
                    err = "THE OPERATION HAS TIMED OUT.";
                }
                else if (ex.Message.ToString() == "Unable to connect the remote server")
                {
                    err = "Unable To Connect the Remote Server.";
                }
                else
                {
                    err = "Problem occured While  get fare rules.Please contact support team (#05).";
                }
                string ErrorMsg = ex.Message.ToString() + ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line")).ToString();
                DatabaseLog.Retrievetcktlog(strUserName, "X", "RetrieveBookingController.cs", "FARE RULES", ErrorMsg, strClientID, strPosTID, sequenceno);
            }
            return Json(new { status = stu, message = msg, error = err });
        }

        public ActionResult Fetch_PassThrough_Card_details(string Corporate, string AirlineCode, string AIRPORT_ID, string EMPCODE, string CRSTYPE, string BOOKINGAMOUNT)
        {
            string strCardDetails = string.Empty;
            string LstrDetails = string.Empty;
            string xmldata = string.Empty;

            //DataTable _dtMeetingDetails = (DataTable)Session["SLoginDetails"];
            //string CompanyID = _dtMeetingDetails != null && _dtMeetingDetails.Rows[0]["COMPANYID"].ToString() != "" ? _dtMeetingDetails.Rows[0]["COMPANYID"].ToString() : "";
            //string UserID = _dtMeetingDetails != null && _dtMeetingDetails.Rows[0]["USERID"] != null && _dtMeetingDetails.Rows[0]["USERID"].ToString() != "" ? _dtMeetingDetails.Rows[0]["USERID"].ToString() : "";
            //string UserName = _dtMeetingDetails != null && _dtMeetingDetails.Rows[0]["USERNAME"] != null && _dtMeetingDetails.Rows[0]["USERNAME"].ToString() != "" ? _dtMeetingDetails.Rows[0]["USERNAME"].ToString() : "";
            //string SequenceID = _dtMeetingDetails != null ? _dtMeetingDetails.Rows[0]["SESSIONID"].ToString() : "";
            string strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
            string strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
            string SequenceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            string strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string IPaddress = ControllerContext.HttpContext.Request.UserHostAddress.ToString();
            try
            {
                _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                //EMPCODE = UserID;
                #region Request&Log
                xmldata = "<REQUEST>Fetch_PassThrough_Card_details" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</REQUEST>" +
                                "<AIRLINECODE>" + AirlineCode.TrimEnd('|').Split('|')[0] + "</AIRLINECODE>" +
                                "<AIRPORTID>" + AIRPORT_ID + "</AIRPORTID>" +
                                "<EMPCODE>" + EMPCODE + "</EMPCODE>" +
                                "<CORPORATE>" + Corporate + "</CORPORATE>" +
                                 "<CRSTYPE>" + CRSTYPE + "</CRSTYPE>" +
                                  "<BOOKINGAMOUNT>" + BOOKINGAMOUNT + "</BOOKINGAMOUNT>";
                DataSet stscard = _RaysService.Fetch_PassThrough_Card_detailsRetrieve(Corporate, AirlineCode.TrimEnd('|').Split('|')[0], AIRPORT_ID, EMPCODE, CRSTYPE, BOOKINGAMOUNT);
                StringWriter strWriter = new StringWriter();
                stscard.WriteXml(strWriter);
                xmldata += "<RESPONSE>Fetch_PassThrough_Card_details" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</RESPONSE>" +
                           "<DATA>" + strWriter + "</DATA>";
                LstrDetails = "<Fetch_PassThrough_Card_details-RESPONSE><URL>[<![CDATA[" + ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() + "]]>]</URL><QUERY>" + xmldata + "</QUERY><RESTIME>" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</RESTIME></Fetch_PassThrough_Card_details-RESPONSE>";
                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "T", "RetrieveBookingController.cs", "Fetch_PassThrough_Card_details", LstrDetails, strPosID, strPosTID, Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : "0");
                #endregion

                if (stscard.Tables.Count > 0 && stscard.Tables[0].Rows.Count > 0)
                {
                    var CardDetails = (from cdt in stscard.Tables[0].AsEnumerable()
                                       select new
                                       {
                                           PG_BANK_NAME = cdt["FOP_BANK_NAME"].ToString(),
                                           PG_CARD_TYPE = cdt["FOP_CARD_TYPE"].ToString(),
                                           PG_CARD_NUMBER = cdt["FOP_CARD_NUMBER"].ToString(),
                                           PG_CVV = cdt["FOP_CVV"].ToString(),
                                           PG_EXPIRY_DATE = cdt["PG_EXPIRY_DATE"].ToString(),
                                           PG_HOLDER_NAME = cdt["FOP_HOLDER_NAME"].ToString(),
                                           PG_CARD_NAME = cdt["FOP_CARD_NAME"].ToString(),
                                           PG_CARD_OBC_AMOUNT = cdt["FOP_CARD_OBC_AMOUNT"].ToString()
                                       }).ToList();
                    strCardDetails = JsonConvert.SerializeObject(CardDetails);
                }
                else
                {
                    return Json(new { status = "00", Errormsg = "No Card Details Found,Please choose any other payment modes. ", arrfetchdata = "" });
                }
            }
            catch (Exception ex)
            {
                string errormgs = string.Empty;
                if (ex.Message.ToString() == "The operation has timed out.")
                    errormgs = ex.Message.ToString();
                else if (ex.Message.ToString() == "Unable to connect the remote server.")
                    errormgs = "unable to connect the remote server.";
                else
                    errormgs = "Problem occured While Fetch Card Details.Please contact support team (#05).";
                xmldata += "<Exception>" + ex.Message.ToString() + ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line")).ToString() + "</Exception>";
                LstrDetails = "<FETCH PASS THROUGH CARD DETAILS-ERROR><URL>[<![CDATA[" + ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() + "]]>]</URL><QUERY>" + xmldata + "</QUERY><RESTIME>" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</RESTIME></FETCH PASS THROUGH CARD DETAILS-ERROR>";
                DatabaseLog.Retrievetcktlog(Session["username"] != null ? Session["username"].ToString() : "", "X", "RetrieveBookingController.cs", "FETCH PASS THROUGH CARD DETAILS", LstrDetails, strPosID, strPosTID, SequenceID);
                return Json(new { status = "00", Errormsg = errormgs, arrfetchdata = "" });
            }
            return Json(new { status = "01", Errormsg = "", arrfetchdata = strCardDetails });
        }

        public ActionResult LoadPCCDetails(string strOfficeID, string strCRSPNR)
        {
            DataSet dsRQT_AGN_PCC = (DataSet)Session["LOADPCC" + strCRSPNR];
            string strPCCDetails = string.Empty;
            try
            {
                if (dsRQT_AGN_PCC != null && dsRQT_AGN_PCC.Tables.Count > 0)
                {
                    var qry = (from PCC in dsRQT_AGN_PCC.Tables[0].AsEnumerable()
                               where PCC.Field<string>("CCR_PCC") == strOfficeID
                               select new
                               {
                                   PCCDetails = PCC["CCR_CREDENTIALS"],
                               }).Distinct();

                    DataTable dtPCCDetails = ConvertToDataTable(qry);
                    DataSet dsPCCDetails = new DataSet();
                    StringReader theReader = new StringReader(dtPCCDetails.Rows[0]["PCCDetails"].ToString());
                    dsPCCDetails.ReadXml(theReader);
                    dsPCCDetails.Tables[0].TableName = "CREDENTIALS";
                    strPCCDetails = JsonConvert.SerializeObject(dsPCCDetails).ToString();
                }
                else
                {
                    strPCCDetails = "-1";
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { Result = strPCCDetails });
        }

        public static decimal SplitFareByAdultPax(string amount)
        {
            decimal adultFare = 0;
            string[] fareSplitUp = amount.Split('|');
            adultFare = Convert.ToDecimal(fareSplitUp[0].ToString());
            return adultFare;
        }

        public static decimal SplitFareByChildPax(string tagAmount)
        {
            decimal childFare = 0;
            string[] fareSplitUp = tagAmount.Split('|');
            if (fareSplitUp.Length >= 2)
                childFare = Convert.ToDecimal(fareSplitUp[1].ToString());
            else
                childFare = Convert.ToDecimal(fareSplitUp[0].ToString());
            return childFare;
        }

        public static decimal SplitFareByInfantPax(string tagAmount)
        {
            decimal infantFare = 0;
            string[] fareSplitUp = tagAmount.Split('|');
            if (fareSplitUp.Length >= 3)
                infantFare = Convert.ToDecimal(fareSplitUp[2].ToString());
            else
                infantFare = Convert.ToDecimal(fareSplitUp[0].ToString());
            return infantFare;
        }

        public static string getAirlineName(string airlineCode)
        {
            string strpath = Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, @"XML\\AirlineNames.xml");
            DataSet dsAirways = new DataSet();
            dsAirways.ReadXml(strpath);
            string airwaysName = "";
            var qryAirlineName = from p in dsAirways.Tables["AIRLINEDET"].AsEnumerable()
                                 where p.Field<string>("_CODE") == airlineCode
                                 select p;
            DataView dvAirlineCode = qryAirlineName.AsDataView();
            if (dvAirlineCode.Count == 0)
                airwaysName = airlineCode;
            else
            {
                DataTable dtAilineCode = new DataTable();
                dtAilineCode = qryAirlineName.CopyToDataTable();
                airwaysName = dtAilineCode.Rows[0]["_NAME"].ToString();
            }
            return airwaysName;
        }

        public static DataTable ConvertToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names   
            System.Reflection.PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow   
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;

        }

        public DataSet GetpromocodeNEW(string IssuingBranchID, string ClientID)
        {
            string _rError = string.Empty;
            string CODE_FLAG = string.Empty;
            string GST_FLAG = string.Empty;
            string ReqTime = string.Empty;
            string xmldata = string.Empty;
            string LstrDetails = string.Empty;

            DataSet ResultJson = new DataSet();
            string strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            try
            {

                _InplantService.Url = ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString();
                RQRS.PromoCodeRS PromoCodeRSnew = new RQRS.PromoCodeRS();

                #region Request Log

                ReqTime = "FetchPromocodenewReq:" + DateTime.Now;

                ReqTime = "PROMOCODEREQUEST" + DateTime.Now;
                xmldata = "<EVENT><REQUEST>FETCHPROMOCODESNEW</REQUEST>" +
                           "<IssuingBranchID>" + IssuingBranchID + "</IssuingBranchID>" +
                             "<ClientID>" + ClientID + "</ClientID>" +
                              "</EVENT>";
                LstrDetails = "<PROMOCODE_REQUEST><URL>[<![CDATA[" + ConfigurationManager.AppSettings["APPS_URL"].ToString() + "]]>]</URL><QUERY>" + xmldata + "</QUERY><REQTIME>" + ReqTime + "</REQTIME></PROMOCODE_REQUEST>";

                DatabaseLog.Retrievetcktlog(System.Web.HttpContext.Current.Session["username"] != null ? System.Web.HttpContext.Current.Session["username"].ToString() : "", "T", "RetrieveBookingController.cs", "PRQ~", LstrDetails, strPosID, strPosTID, "0");

                #endregion
                System.Web.HttpContext.Current.Session["branchid"] = IssuingBranchID;
                System.Web.HttpContext.Current.Session.Add("BRANCH_ID", IssuingBranchID);
                System.Web.HttpContext.Current.Session["CompanyID"] = ClientID;
                DataSet Dspromocode = _InplantService.FetchCodeDetails(IssuingBranchID, ClientID, ref _rError, CODE_FLAG, GST_FLAG);

                #region Respose Log

                ReqTime = "FetchPromocodenewRes:" + DateTime.Now;
                ReqTime = "PROMOCODERESPONSE" + DateTime.Now;
                xmldata = "<EVENT><REQUEST>FETCHPROMOCODESNEW</REQUEST>" +
                           "<IssuingBranchID>" + IssuingBranchID + "</IssuingBranchID>" +
                             "<ClientID>" + ClientID + "</ClientID>" +
                             "<Result>" + JsonConvert.SerializeObject(Dspromocode) + "</Result>" +
                              "</EVENT>";
                LstrDetails = "<PROMOCODE_RESPONSE><URL>[<![CDATA[" + ConfigurationManager.AppSettings["APPS_URL"].ToString() + "]]>]</URL><QUERY>" + xmldata + "</QUERY><REQTIME>" + ReqTime + "</REQTIME></PROMOCODE_RESPONSE>";

                DatabaseLog.Retrievetcktlog(System.Web.HttpContext.Current.Session["username"] != null ? System.Web.HttpContext.Current.Session["username"].ToString() : "", "T", "RetrieveBookingController.cs", "PRS~", LstrDetails, strPosID, strPosTID, "0");

                #endregion

                if (Dspromocode != null && Dspromocode.Tables != null && Dspromocode.Tables.Count > 0 && Dspromocode.Tables[0].Rows != null && Dspromocode.Tables[0].Rows.Count > 0)
                {
                    string strResponse = JsonConvert.SerializeObject(Dspromocode);
                    ResultJson = Dspromocode;
                    System.Web.HttpContext.Current.Session.Add("PROMOCODERESPONSE_Queue", strResponse);
                    System.Web.HttpContext.Current.Session.Add("PROMOCODERESPONSESTATUS_Queue", "1");
                }
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString() + ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line")).ToString();
                DatabaseLog.Retrievetcktlog(Session["UserName"] != null ? Session["UserName"].ToString() : "", "X", "RetrieveBookingController.cs", "GetpromocodeNEW", ErrorMsg, strPosID, strPosTID, "0");
            }
            return ResultJson;
        }

        public Byte[] Compress(DataSet dataset)
        {
            Byte[] data;
            MemoryStream mem = new MemoryStream();
            GZipStream zip = new GZipStream(mem, CompressionMode.Compress);
            dataset.WriteXml(zip, XmlWriteMode.WriteSchema);
            zip.Close();
            data = mem.ToArray();
            mem.Close();
            return data;
        }

        public ActionResult GetClientBalance(string strAgentId, string strBranchID)
        {
            string strAgentid = string.Empty;
            string strUserName = string.Empty;
            string strTerminalID = string.Empty;
            string strIPaddress = string.Empty;
            string strSequenceId = DateTime.Now.ToString("yyyyMMddhhmmss");
            byte[] dsclien_compress = new byte[] { };
            DataSet dsData = new DataSet();
            string strData = string.Empty;
            string strStatus = string.Empty;
            string strMsg = string.Empty;
            string strPosID = string.Empty;
            string strPosTID = string.Empty;
            try
            {
                if (Session["agentid"] == null || Session["agentid"].ToString() == "")
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }

                strAgentid = (strAgentId != null && strAgentId != "") ? strAgentId : "";
                strUserName = Session["UserName"] != null ? Session["UserName"].ToString() : "";
                strIPaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                strSequenceId = Session["SequenceID"] != null ? Session["SequenceID"].ToString() : strSequenceId;
                strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";

                #region SERVICE URL BRANCH BASED -- STS115
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (strBranchID != "" && strBranchCredit.Contains(strBranchID)))
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                        _InplantService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_INPLANT_SERVICE"].ToString();
                    }
                    else
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                        _InplantService.Url = ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString();
                    }
                }
                else
                {
                    _RaysService.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                    _InplantService.Url = ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString();
                }
                #endregion

                string ReqTime = DateTime.Now.ToString();
                string REQXML = "<EVENT><REQUEST>GETCLIENTBALANCE</REQUEST><REQTIME>" + ReqTime + "</REQTIME><AGENTID>" + strAgentid + "</AGENTID></EVENT>";
                DatabaseLog.Retrievetcktlog(strUserName, "E", "CntrlPanelController", "GetClientBalance", REQXML.ToString(), strPosID, strPosTID, strSequenceId);

                string strCurrencyCode = ConfigurationManager.AppSettings["currency"].ToString();
                dsclien_compress = _RaysService.Fetch_Client_Credit_Balance_New(strAgentid, "", strTerminalID, strUserName, strIPaddress, strSequenceId, "", strCurrencyCode);
                if (dsclien_compress != null)
                {
                    dsData = Decompress(dsclien_compress);
                }
                string ResTime = DateTime.Now.ToString();
                string RESXML = "<EVENT><RESPONSE>GETCLIENTBALANCE</RESPONSE><RESTIME>" + ResTime + "</RESTIME><RES>" + dsData.GetXml() + "</RES></EVENT>";
                DatabaseLog.Retrievetcktlog(strUserName, "E", "CntrlPanelController", "GetClientBalance", RESXML.ToString(), strPosID, strPosTID, strSequenceId);

                if (dsData != null && dsData.Tables.Count != 0 && dsData.Tables[0].Rows.Count != 0)
                {
                    strData = JsonConvert.SerializeObject(dsData).ToString();
                    if (dsData.Tables[0].Columns.Contains("ErrorMsg"))
                    {
                        strStatus = "02";
                        strMsg = dsData.Tables[0].Rows[0]["ErrorMsg"].ToString().Trim();
                    }
                    else
                    {
                        Session["CltBranchID"] = dsData.Tables[0].Rows[0]["CLT_BRANCH_ID"].ToString().Trim();
                        Session["CltBalFlag"] = dsData.Tables[0].Rows[0]["CLT_BALANCE_CHECK"].ToString().Trim();
                        Session["CltClientID"] = dsData.Tables[0].Rows[0]["CLT_CLIENT_ID"].ToString().Trim();
                        strStatus = "01";
                    }
                }
                else
                {
                    strStatus = "00";
                    strMsg = "Please Check Agent ID";
                }
            }
            catch (Exception ex)
            {
                strStatus = "00";
                strMsg = "Unable to fetch Agent balance. Please contact support team.";
                DatabaseLog.Retrievetcktlog(strUserName, "X", "CntrlPanelController", "GetClientBalance", ex.ToString(), strPosID, strPosTID, strSequenceId);
            }
            return Json(new { Status = strStatus, Message = strMsg, Result = strData });
        }

        private DataSet Decompress(Byte[] data)
        {
            try
            {
                MemoryStream mem = new MemoryStream(data);
                GZipStream zip = new GZipStream(mem, CompressionMode.Decompress);
                DataSet dataset = new DataSet();
                dataset.ReadXml(zip, XmlReadMode.ReadSchema);
                zip.Close();
                mem.Close();
                return dataset;
            }
            catch (Exception ex)
            {
                //string count = "PNR022";
                //string Function = "Decompress";
                string Message = ex.ToString();
                string Description = "<ERROR><DESC>" + Message + "</DESC></ERROR>";
                // ErrorLog(userID, IP, "A", "X", Pagename, count + "-" + Function, Description, sequentialID);
                return null;
            }
        }

        #region agent transation report
        public ActionResult RequestTopupFunction(string fromdate, string todate, string sPNR, string option, string product, string AccountType, string AgentId, string Currencycode, string Terminal_ID, string strMigratedAgent)
        {
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string strAgentID = string.Empty;
            string terminalType = string.Empty;
            string strResponse = string.Empty;
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;
            byte[] decompress = new byte[] { };
            ArrayList array_topup = new ArrayList();
            StringBuilder AgentTopup = new StringBuilder();
            StringBuilder stroptions = new StringBuilder();
            array_topup.Add("");
            array_topup.Add("");
            int Error = 0;
            int responseBookedHistory = 1;
            DataSet dsAgentDetails = new DataSet();
            try
            {
                DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "E", "AgentTopUp", "RequestTopupFunction", "<EVENT><FROMDATE>" + fromdate + "</FROMDATE><TODATE>" + todate + "</TODATE><SPNR>" + sPNR + "</SPNR><OPTION>" + option + "</OPTION><PRODUCT>" + product + "</PRODUCT><ACCOUNTTYPE>" + AccountType + "</ACCOUNTTYPE></EVENT>", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                strTerminalId = Session["POS_TID"].ToString();
                strUserName = Session["username"].ToString();
                strAgentID = (AgentId != null && AgentId != "") ? (AgentId != "ALL" ? AgentId : "") : Session["POS_ID"].ToString();
                terminalType = (Session["TerminalType"] != null && Session["TerminalType"].ToString() != "" ? Session["TerminalType"].ToString() : "");
                strResponse = "";
                Ipaddress = Session["ipAddress"].ToString();
                sequnceID = Session["sequenceid"].ToString();
                string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");
                string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"].ToString() != "") ? Session["TKTFLAG"].ToString() : "DTKT";
                string strErrorMsg = "";
                string fromDate = fromdate;
                string toDate = todate;
                string terminalUser = (Terminal_ID != null && Terminal_ID != "") ? Terminal_ID : Session["POS_TID"].ToString();
                string lstrref = "";

                byte[] dsViewPNR_compress = new byte[] { };
                byte[] Compressed = new byte[] { };

                if (fromDate != null && fromDate != "")
                {
                    fromDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                }
                if (toDate != null && toDate != "")
                {
                    toDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                }

                    _RaysService.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();



                dsViewPNR_compress = _RaysService.Fetch_Transsactionbyte(sPNR.ToString(), strAgentID.ToString(), fromDate.ToString(), toDate.ToString(),
                terminalUser, "", AccountType.ToString(), strAgentID.ToString(), strTerminalId.ToString(), strUserName.ToString(),
                Ipaddress.ToString(), terminalType.ToString(), Convert.ToDecimal(sequnceID.ToString()), product.ToString(), option.ToString(),
                ref strErrorMsg, "Retrive-Topup or Credit Account", "RequestTopupFunction", lstrref, Compressed, Currencycode, "");

                if (dsViewPNR_compress != null)
                {
                    dsAgentDetails = Decompress(dsViewPNR_compress);
                }

                if (dsAgentDetails != null && dsAgentDetails.Tables[0] != null && dsAgentDetails.Tables[0].Rows.Count > 0)
                {
                    Session.Add("TOPUPREPORT", dsAgentDetails);
                    array_topup[responseBookedHistory] = JsonConvert.SerializeObject(dsAgentDetails.Tables[0]);
                }
                else
                {
                    Session.Add("TOPUPREPORT", dsAgentDetails);
                    array_topup[Error] = "No Record(s) Found";
                }
            }

            catch (Exception ex)
            {
                DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "X", "AgentTopUp", "RequestTopupFunction", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                array_topup[Error] = "Problem Occured.Please contact customercare";
            }
            return Json(new { Status = "", Message = "", Result = array_topup });
        }

        #endregion

        public ActionResult RetrieveAccounting()
        {
            strModule = (ConfigurationManager.AppSettings["PLATFORM"] != null ? ConfigurationManager.AppSettings["PLATFORM"].ToString().ToUpper().Trim() : "");
            Session.Add("RetrieveFlag", "A"); //Accounting
            return View();
        }

        public ActionResult LoadOfficeId()
        {
            string stu = string.Empty;
            string msg = string.Empty;
            string result = string.Empty;
            //string strAstAgentId = Convert.ToString(Session["SESSASTAGENTID"]);
            //string strAstTerminalId = Convert.ToString(Session["SESSASTTERMINALID"]);
            string strLoginAgentID = Session["AGENTID"] != null ? Session["AGENTID"].ToString() : "";
            string strLoginTerminalID = Session["TERMINALID"] != null ? Session["TERMINALID"].ToString() : "";
            string strLoginUserName = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "";
            string strLoginIPAddress = Session["IPADDRESS"] != null ? Session["IPADDRESS"].ToString() : "";
            string strLoginTerminalType = Session["TERMINALTYPE"] != null ? Session["TERMINALTYPE"].ToString() : "";
            string strLoginAgentType = Session["CLIENTTYPE"] != null ? Session["CLIENTTYPE"].ToString() : "";
            string strLoginBranchId = Session["BRANCHID"] != null ? Session["BRANCHID"].ToString() : "";
            string strLoginSeqId = Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : DateTime.Now.ToString("ddMMyyyy");
            string strLoginPosId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strLoginPosTId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strLoginPlatform = Session["PLATFORM"] != null ? Session["PLATFORM"].ToString() : "";
            byte[] _resultbyt = null;
            try
            {
                string Referrormsg = string.Empty;
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                _resultbyt = _RaysService.FetchCradentialsdetailsPCC(strLoginUserName, "", strLoginSeqId, ref Referrormsg, "RETRIEVEBOOKING", "", "");
               // _resultbyt = _RaysService.FETCH_BOA_OWN_SUPP_DETAILS(strAstAgentId, strLoginUserName, strLoginIPAddress, strLoginSeqId, strAstTerminalId);
                DataSet strResponse = new DataSet();
                try
                {
                    strResponse = Base.Decompress(_resultbyt);
                }
                catch (Exception ex)
                {
                    strResponse = null;
                }

                string str_Log_Param = "<EVENT><AGENTID>" + strLoginAgentID + "</AGENTID><USERNAME>" + strLoginUserName + "</USERNAME><RESPONSE_JSON>" + JsonConvert.SerializeObject(strResponse) + "</RESPONSE_JSON></EVENT>";
                DatabaseLog.Retrievetcktlog(strLoginUserName, "E", "QueueTicket", "Load Office Id", str_Log_Param, strLoginAgentID, strLoginTerminalID, strLoginSeqId);

               
                if (strResponse != null && strResponse.Tables.Count > 0 && strResponse.Tables.Count > 3 && strResponse.Tables[2].Rows.Count > 0)
                {
                    stu = "1";
                }
                else
                {
                    stu = "0";
                    msg = "No records found.";
                }
                result = JsonConvert.SerializeObject(strResponse);
            }
            catch (Exception ex)
            {
                DatabaseLog.Retrievetcktlog(strLoginUserName, "X", "QueueTicket", "Load Office Id", ex.ToString(), strLoginAgentID, strLoginTerminalID, strLoginSeqId);
                if (ex.Message.ToString().ToUpper().Trim().Contains("TIMED OUT") || ex.Message.ToString().ToUpper().Trim().Contains("SERVICE UNAVAILABLE.")
                     || ex.Message.ToString().ToUpper().Trim().Contains("SERVER"))

                    msg = "Unable to retrieve office id details!";
                return Json(new { Status = "0", Message = msg, Result = "" });
            }
            var temp = Json(new { Status = stu, Message = msg, Result = result }, JsonRequestBehavior.AllowGet);
            temp.MaxJsonLength = int.MaxValue;
            return temp;
        }

        public ActionResult FetchPassthroughCardDetails()
        {
            string stu = string.Empty;
            string msg = string.Empty;
            string result = string.Empty;  
            string strLoginAgentID = Session["AGENTID"] != null ? Session["AGENTID"].ToString() : "";
            string strLoginTerminalID = Session["TERMINALID"] != null ? Session["TERMINALID"].ToString() : "";
            string strLoginUserName = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "";
            string strLoginIPAddress = Session["IPADDRESS"] != null ? Session["IPADDRESS"].ToString() : "";
            string strLoginTerminalType = Session["TERMINALTYPE"] != null ? Session["TERMINALTYPE"].ToString() : "";
            string strLoginAgentType = Session["CLIENTTYPE"] != null ? Session["CLIENTTYPE"].ToString() : "";
            string strLoginBranchId = Session["BRANCHID"] != null ? Session["BRANCHID"].ToString() : "";
            string strLoginSeqId = Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : DateTime.Now.ToString("ddMMyyyy");
            string strLoginPosId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strLoginPosTId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strLoginPlatform = Session["PLATFORM"] != null ? Session["PLATFORM"].ToString() : "";
           
            try
            {
                string Referrormsg = string.Empty;
                string strResponse = string.Empty;
                DataSet dsCardDetails = new DataSet();
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                bool returnres = _RaysService.FetchFOPCardDetailsRawContent(strLoginAgentID, strLoginTerminalID, strLoginUserName, strLoginIPAddress, "RETRIEVEBOOKING", Convert.ToDecimal(strLoginSeqId), ref strResponse);

               
                try
                {
                    dsCardDetails = _ServiceUtility.convertJsonStringToDataSet(strResponse, "");
                  
                }
                catch (Exception ex)
                {
                   
                }

                string str_Log_Param = "<EVENT><AGENTID>" + strLoginAgentID + "</AGENTID><USERNAME>" + strLoginUserName + "</USERNAME><RESPONSE_JSON>" + JsonConvert.SerializeObject(strResponse) + "</RESPONSE_JSON></EVENT>";
                DatabaseLog.Retrievetcktlog(strLoginUserName, "E", "QueueTicket", "Load Office Id", str_Log_Param, strLoginAgentID, strLoginTerminalID, strLoginSeqId);


                if (dsCardDetails != null && dsCardDetails.Tables.Count > 0)
                {
                    stu = "1";
                    result = strResponse;
                }
                else
                {
                    stu = "0"; 
                }
                
            }
            catch (Exception ex)
            {
                DatabaseLog.Retrievetcktlog(strLoginUserName, "X", "QueueTicket", "Load Office Id", ex.ToString(), strLoginAgentID, strLoginTerminalID, strLoginSeqId);
                if (ex.Message.ToString().ToUpper().Trim().Contains("TIMED OUT") || ex.Message.ToString().ToUpper().Trim().Contains("SERVICE UNAVAILABLE.")
                     || ex.Message.ToString().ToUpper().Trim().Contains("SERVER"))

                    msg = "Unable to get card details!";
                return Json(new { Status = "0", Message = msg, Result = "" });
            }
            var temp = Json(new { Status = stu, Message = msg, Result = result }, JsonRequestBehavior.AllowGet);
            temp.MaxJsonLength = int.MaxValue;
            return temp;
        }
    }
}

