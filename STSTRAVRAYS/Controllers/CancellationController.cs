using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using STSTRAVRAYS.Rays_service;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
//using STSCBT.Models;

namespace STSTRAVRAYS.Controllers
{
    public class CancellationController : Controller
    {
        //  Rays_service.RaysService _RAYS_SERVICE = new Rays_service.RaysService();
        RaysService _RAYS_SERVICE = new RaysService();
        public static string strPagename = "CancellationController";
        public static Hashtable AirportNames = new Hashtable();
        public static string strProduct = ConfigurationManager.AppSettings["Producttype"] != null ? ConfigurationManager.AppSettings["Producttype"].ToString() : "";
        public static string strBranchCredit = ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"] != null ? ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"].ToString().ToUpper() : "";
        public static string strRaysURL = ConfigurationManager.AppSettings["ServiceURI"] != null ? ConfigurationManager.AppSettings["ServiceURI"].ToString() : "";
        public static string strTopupRaysURL = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"] != null ? ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString() : "";
        public static string strQTKTRaysURL = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"] != null ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : "";






        public ActionResult GetCancellationPNR(string strSPNR, string strArilinePNR, string strCRSPNR)
        {
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strResult = string.Empty;
            string strFunctionName = string.Empty;
            string strLoginAgentID = Session["AGENTID"] != null ? Session["AGENTID"].ToString() : "";
            string strLoginClientID = Session["CLIENTID"] != null ? Session["CLIENTID"].ToString() : "";
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
            string strTKTFLAG = Session["TKTFLAG"] != null ? Session["TKTFLAG"].ToString() : "DTKT";
            strFunctionName = "GetCancellationPNR";
            string getAgentType = string.Empty;
            string getprevilage = string.Empty;
            ArrayList _ResArr = new ArrayList();
            _ResArr.Add("");//0 PNR RESPONSE
            _ResArr.Add("");//1 PAX RESPONSE
            _ResArr.Add("");//2 TICKET RESPONSE
            _ResArr.Add("");//3 SHOW ONLINE BUTTON
            try
            {
                if (strLoginPosId == "" || strLoginPosTId == "" || strLoginIPAddress == "" || strLoginSeqId == "" || strLoginTerminalType == "")
                {
                    return Json(new { Status = "-1", Result = "", Message = "Session Expired" });
                }
                if (string.IsNullOrEmpty(strSPNR) && string.IsNullOrEmpty(strArilinePNR) && string.IsNullOrEmpty(strCRSPNR))
                {
                    return Json(new { Status = "0", Result = "", Message = "Please enter any PNR." });
                }

                #region
                string ConsoleAgent = ConfigurationManager.AppSettings["ConsoleAgent"] != null ? ConfigurationManager.AppSettings["ConsoleAgent"].ToString() : "";
                if (getprevilage.ToString() == "S")
                {
                    strLoginTerminalID = string.Empty;
                }
                else
                {
                    strLoginTerminalID = Session["POS_TID"].ToString();
                }
                if (getAgentType == ConsoleAgent) //Travrays Admin
                {
                    strLoginPosId = string.Empty;
                    strLoginTerminalID = string.Empty;
                }
                if (strLoginTerminalType == "T")
                {
                    strLoginPosId = string.Empty;
                }
                #endregion

                StringBuilder strpassdetails = new StringBuilder();
                DataSet dsViewPNR = new DataSet();
                DataSet dsFareDispDet = new DataSet();
                DataSet dsPassDetails = new DataSet();

                byte[] dsViewPNR_compress = new byte[] { };
                byte[] dsFareDispDet_compress = new byte[] { };


                string refError = "ERROR";
                string strErrorMsg = "ERROR";
                bool result = false;

                strSPNR = strSPNR.Trim();
                strArilinePNR = strArilinePNR.Trim();
                strCRSPNR = strCRSPNR.Trim();
                DataSet dsDisplayDet = new DataSet();

                _RAYS_SERVICE.Url = strTKTFLAG == "QTKT" ? strQTKTRaysURL : strRaysURL;

                result = _RAYS_SERVICE.Fetch_PNR_Verification_Details_NewByte(strLoginPosId, strSPNR, strArilinePNR, strCRSPNR, "", strLoginTerminalID, strLoginUserName, strLoginIPAddress, "W", Convert.ToDecimal(strLoginSeqId), ref dsViewPNR_compress, ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "FetchCancellationPNR", "FetchPNRdetailsforcancellation", getAgentType, strLoginTerminalID);

                if (strProduct == "RBOA" && result == false && strLoginTerminalType == "T")
                {
                    _RAYS_SERVICE.Url = strTopupRaysURL;
                    result = _RAYS_SERVICE.Fetch_PNR_Verification_Details_NewByte(strLoginPosId, strSPNR, strArilinePNR, strCRSPNR, "", strLoginTerminalID, strLoginUserName, strLoginIPAddress, "W", Convert.ToDecimal(strLoginSeqId), ref dsViewPNR_compress, ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "FetchCancellationPNR", "FetchPNRdetailsforcancellation", getAgentType, strLoginTerminalID);
                }
                dsViewPNR = Decompress(dsViewPNR_compress);
                dsDisplayDet = Decompress(dsFareDispDet_compress);

                if (result == true && dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                {                   
                    if (dsViewPNR.Tables[0].AsEnumerable().Any(v => v.Field<string>("TICKET_STATUS_FLAG") == "C"))
                    {
                        string spnr = dsViewPNR.Tables[0].Rows[0]["S_PNR"].ToString();
                        string airline_pnr = dsViewPNR.Tables[0].Rows[0]["AIRLINE_PNR"].ToString();
                        string crs_pnr = dsViewPNR.Tables[0].Rows[0]["CRS_PNR"].ToString();
                        Session.Add("VIEWPNR_" + spnr, dsViewPNR);
                        _ResArr[0] = spnr + "|" + airline_pnr + "|" + crs_pnr;
                        var PaxInfo = (from input in dsViewPNR.Tables[0].AsEnumerable()
                                       where input["SEGMENT_NO"].ToString() == "1"
                                       select new
                                       {
                                           SPNR = input["S_PNR"].ToString(),
                                           PASSENGER_TYPE = input["PASSENGER_TYPE"].ToString(),
                                           PASSENGER_NAME = input["PASSENGER_NAME"].ToString(),
                                           TICKET_NO = input["TICKET_NO"].ToString(),
                                           STATUS = input["TICKET_STATUS"].ToString(),
                                           GROSSFARE = input["GROSSFARE"].ToString(),
                                           PAX_REF_NO = input["PAX_REF_NO"].ToString()
                                       });
                        string paxdetails = JsonConvert.SerializeObject(PaxInfo);
                        _ResArr[1] = paxdetails;
                        var Ticketinfo = (from input in dsViewPNR.Tables[0].AsEnumerable()
                                          where input["PAX_REF_NO"].ToString() == "1"
                                          select new
                                          {
                                              FLIGHT_NO = input["FLIGHT_NO"].ToString(),
                                              ORIGIN = input["ORIGIN"].ToString(),
                                              DESTINATION = input["DESTINATION"].ToString(),
                                              DEPT_DATE = input["DEPTDT"].ToString(),
                                              ARR_DATE = input["ARRIVALDT"].ToString(),
                                              //DEPT_DATE = input["DEPT_DATE"].ToString(),
                                              //ARR_DATE = input["ARRIVAL_DATE"].ToString(),
                                              STATUS = input["TICKET_STATUS"].ToString(),
                                              SPNR = input["S_PNR"].ToString(),
                                              SEG_NO = input["SEGMENT_NO"].ToString(),
                                              DEPART_DATE = input["DEPT_DATE"].ToString()
                                          });
                        string Ticketdetails = JsonConvert.SerializeObject(Ticketinfo);
                        _ResArr[2] = Ticketdetails;

                        string StockType = Regex.Split(dsViewPNR.Tables[0].Rows[0]["StockType"].ToString(), "SPLIT")[0].ToString();
                        string Aircategory = dsViewPNR.Tables[0].Rows[0]["AIRLINE_CATEGORY"].ToString();
                        _ResArr[3] = ((Aircategory != "" && ConfigurationManager.AppSettings["Onlinecancellation"].ToString().Contains(Aircategory))
                            || (StockType != "" && ConfigurationManager.AppSettings["Onlinecancellation"].ToString().Trim().Contains(StockType))) ? "ONLINE" : "OFFLINE";

                        strStatus = "1";
                        strResult = JsonConvert.SerializeObject(_ResArr);
                        strMessage = "";
                    }
                    else if (dsViewPNR.Tables[0].AsEnumerable().All(v => v.Field<string>("TICKET_STATUS_FLAG") == "F"))
                    {
                        strStatus = "0";
                        strMessage = "Ticket already Cancelled. (#03)";
                    }
                    else if (dsViewPNR.Tables[0].AsEnumerable().All(v => v.Field<string>("TICKET_STATUS_FLAG") == "R"))
                    {
                        strStatus = "0";
                        strMessage = "Ticket already Rescheduled. (#03)";
                    }
                    else if (dsViewPNR.Tables[0].AsEnumerable().All(v => v.Field<string>("TICKET_STATUS_FLAG") == "D"))
                    {
                        strStatus = "0";
                        strMessage = "Ticket already requested for Cancellation. Please contact support team. (#03)";
                    }
                    else if (dsViewPNR.Tables[0].AsEnumerable().All(v => v.Field<string>("TICKET_STATUS_FLAG") == "S"))
                    {
                        strStatus = "0";
                        strMessage = "Ticket already requested for Reschedule. Please contact support team. (#03)";
                    }
                    else
                    {
                        strStatus = "0";
                        strMessage = "Kindly check the PNR status (#03)";
                    }                 

                    //if (strLoginTerminalType == "W")
                    //{

                    // }




                }
                else
                {
                    strStatus = "0";
                    strResult = "";
                    strMessage = "No Records found!.";
                }
            }
            catch (Exception ex)
            {
                strMessage = "Unable to get requested details. please contact support team(#05)";
                strStatus = "0";
                strResult = ex.ToString();

                DatabaseLog.LogData(strLoginUserName, "X", "CancellationController", strFunctionName + "-ERR", strResult.ToString(), strLoginPosId, strLoginPosTId, strLoginSeqId);
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage });
        }

        public ActionResult RequestCancellation(string strSPNR, string PaxDetails, string Ticketdetails,
            string PaxcancelationStatus, string Ticketcancelstatus, string Remarks, string OfflineRequest)
        {
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strResult = string.Empty;
            string strFunctionName = string.Empty;
            string strLoginAgentID = Session["AGENTID"] != null ? Session["AGENTID"].ToString() : "";
            string strLoginClientID = Session["CLIENTID"] != null ? Session["CLIENTID"].ToString() : "";
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
            string strTKTFLAG = Session["TKTFLAG"] != null ? Session["TKTFLAG"].ToString() : "DTKT";
            strFunctionName = "RequestCancellation-" + strSPNR;
            string getAgentType = string.Empty;
            string getprevilage = string.Empty;
            string departureDate = string.Empty;
            string strErrorMsg = string.Empty;
            ArrayList _ResArr = new ArrayList();
            _ResArr.Add("");//0
            _ResArr.Add("");//1
            _ResArr.Add("");//2
            _ResArr.Add("");//3
            _ResArr.Add("");//4
            _ResArr.Add("");//5
            _ResArr.Add("");//6 Online Penalty Amount
            _ResArr.Add("");//7 Online Refund Amount
            _ResArr.Add("");//8 SSR Details
            string strPaxRef = string.Empty;
            string strSegRef = string.Empty;
            int totalpaxcount = 0;
            int totpaxcountwoinfant = 0;
            int maxfareid = 1;
            string AirlineCategory = "";
            try
            {
                
                if (strLoginPosId == "" || strLoginPosTId == "" || strLoginIPAddress == "" || strLoginSeqId == "" || strLoginTerminalType == "")
                {
                    return Json(new { Status = "-1", Result = "", Message = "Session Expired" });
                }

                #region
                string ConsoleAgent = ConfigurationManager.AppSettings["ConsoleAgent"] != null ? ConfigurationManager.AppSettings["ConsoleAgent"].ToString() : "";
                if (getprevilage.ToString() == "S")
                {
                    strLoginTerminalID = string.Empty;
                }
                else
                {
                    strLoginTerminalID = Session["POS_TID"].ToString();
                }
                if (getAgentType == ConsoleAgent) //Travrays Admin
                {
                    strLoginPosId = string.Empty;
                    strLoginTerminalID = string.Empty;
                }
                if (strLoginTerminalType == "T")
                {
                    strLoginPosId = string.Empty;
                }
                #endregion

                _RAYS_SERVICE.Url = strRaysURL;


                #region log

                string Logdetais = "<CANCELATION REQUEST><PAXDETAILS>" + PaxDetails + "</PAXDETAILS><TICKETDETAILS>" + Ticketdetails + "</TICKETDETAILS>";
                Logdetais += "<OFFLINEREQUEST>" + OfflineRequest + "</OFFLINEREQUEST>";
                Logdetais += "<PAXCANCELATIONSTATUS>" + PaxcancelationStatus + "</PAXCANCELATIONSTATUS>";
                Logdetais += "<TICKETCANCELSTATUS>" + Ticketcancelstatus + "</TICKETCANCELSTATUS>";
                Logdetais += "</CANCELATION REQUEST>";


                DatabaseLog.LogData(Session["username"].ToString(), "T", "CancellationController", strFunctionName + "-REQ-Input", Logdetais, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                #endregion


                byte[] dsViewPNR_compress = new byte[] { };
                byte[] dsFareDispDet_compress = new byte[] { };
                PaxDetails = PaxDetails.TrimEnd('|');
                Ticketdetails = Ticketdetails.TrimEnd('|');
                string Spnr = PaxDetails.Trim().Split('|')[0].Split(',')[1].ToString();
                string[] PaxRef = PaxDetails.Split('|');
                for (int _PaxRef = 0; _PaxRef < PaxRef.Length; _PaxRef++)
                {
                    strPaxRef += PaxRef[_PaxRef].Split(',')[2].ToString() + "|";
                }
                string[] SegRef = Ticketdetails.Split('|');
                for (int _SegRef = 0; _SegRef < SegRef.Length; _SegRef++)
                {
                    strSegRef += SegRef[_SegRef].Split(',')[0].ToString() + "|";
                }
                strPaxRef.TrimEnd('|');
                strSegRef.TrimEnd('|');
                string refError = string.Empty;

                bool result = _RAYS_SERVICE.Fetch_PNR_Verification_Details_NewByte(strLoginPosId, strSPNR, "", "", "",
                               strLoginPosTId, strLoginUserName, strLoginIPAddress, strLoginTerminalType, Convert.ToDecimal(strLoginSeqId),
                               ref dsViewPNR_compress, ref dsFareDispDet_compress, ref refError, ref strErrorMsg,
                               "Cancel PNR request", "PNR cancelrequest", getAgentType, strLoginPosTId);
                DataSet dsViewPNR = Decompress(dsViewPNR_compress);
                DataSet dsFareDispDet = Decompress(dsFareDispDet_compress);
                string checkflag = string.Empty;
                if (dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                {
                    string StockType = Regex.Split(dsViewPNR.Tables[0].Rows[0]["StockType"].ToString(), "SPLIT")[0].ToString();
                    string Aircategory = dsViewPNR.Tables[0].Rows[0]["AIRLINE_CATEGORY"].ToString();
                    checkflag = (ConfigurationManager.AppSettings["Onlinecancellation"].ToString().Contains(Aircategory) || ConfigurationManager.AppSettings["Onlinecancellation"].ToString().Contains(StockType)) ? "ONLINE" : "OFFLINE";
                    checkflag = OfflineRequest == "Y" || PaxcancelationStatus == "P" || Ticketcancelstatus == "P" ? "OFFLINE" : checkflag;
                    string spnr = dsViewPNR.Tables[0].Rows[0]["S_PNR"].ToString();
                    Session.Add("VIEWPNR_" + spnr, dsViewPNR);
                    totalpaxcount = Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["AdultCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["ChildCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["InfantCount"]);
                    totpaxcountwoinfant = Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["AdultCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["ChildCount"]);
                    AirlineCategory = dsViewPNR.Tables[0].Rows[0]["AIRCATEGORY"].ToString();

                    maxfareid = dsViewPNR.Tables[0].AsEnumerable().Select(v => (v.Field<Int16>("FARE_ID"))).Distinct().Max();
                }

              
                DataSet dsCancelTicketDetails = new DataSet();

                DataTable dtTcktDet = new DataTable("PNR_DETAILS");

                dtTcktDet.Columns.Add("NAME");
                dtTcktDet.Columns.Add("SPNR");
                dtTcktDet.Columns.Add("PAX_REF_NO");
                dtTcktDet.Columns.Add("SEGMENT_NO");
                dtTcktDet.Columns.Add("REMARKS");

                dtTcktDet.Columns.Add("PAXCOUNT");
                dtTcktDet.Columns.Add("DEPDATE");
                dtTcktDet.Columns.Add("ORIGIN");
                dtTcktDet.Columns.Add("DESTINATION");
                dtTcktDet.Columns.Add("AGENT_EMAILID");
                dtTcktDet.Columns.Add("AGENT_CONTACTNO");
                dtTcktDet.Columns.Add("AIRPNR");
                dtTcktDet.Columns.Add("PAXTYPE");
                dtTcktDet.Columns.Add("AIRPORT_ID");
                dtTcktDet.Columns.Add("TRIP_DESC");
                dtTcktDet.Columns.Add("S_PNR");
                dtTcktDet.Columns.Add("CANCELMODE");

                for (var i = 0; i < dsViewPNR.Tables[0].Rows.Count; i++)
                {
                    if (strPaxRef.Contains(dsViewPNR.Tables[0].Rows[i]["PAX_REF_NO"].ToString()) &&
                        strSegRef.Contains(dsViewPNR.Tables[0].Rows[i]["SEGMENT_NO"].ToString()))
                    {
                        dtTcktDet.Rows.Add(dsViewPNR.Tables[0].Rows[i]["PASSENGER_NAME"].ToString(), dsViewPNR.Tables[0].Rows[i]["S_PNR"].ToString(), dsViewPNR.Tables[0].Rows[i]["PAX_REF_NO"].ToString(), dsViewPNR.Tables[0].Rows[i]["SEGMENT_NO"].ToString(),
                            Remarks, totalpaxcount, dsViewPNR.Tables[0].Rows[0]["DEPTDT"].ToString(), dsViewPNR.Tables[0].Rows[0]["ORIGINCODE"].ToString(), //dsViewPNR.Tables[0].Rows[0]["DEPTDT"].ToString()
                            dsViewPNR.Tables[0].Rows[0]["DESTINATIONCODE"].ToString(), dsViewPNR.Tables[0].Rows[0]["AGENT_EMAIL_ID"].ToString(), (dsViewPNR.Tables[0].Rows[0]["AGENCY_NAMEADDRESS"] != null && dsViewPNR.Tables[0].Rows[0]["AGENCY_NAMEADDRESS"].ToString() != "" ? dsViewPNR.Tables[0].Rows[0]["AGENCY_NAMEADDRESS"].ToString().Split('~')[5] : ""),
                            dsViewPNR.Tables[0].Rows[0]["AIRLINE_PNR"].ToString(), dsViewPNR.Tables[0].Rows[0]["PASSENGER_TYPE"].ToString(), dsViewPNR.Tables[0].Rows[0]["AIRPORTID"].ToString(), dsViewPNR.Tables[0].Rows[0]["TRIP_DESC"].ToString(), dsViewPNR.Tables[0].Rows[i]["S_PNR"].ToString(), "O");
                    }
                }

                DataTable RQRStable = new DataTable("RQRS");
                RQRStable.Columns.Add("AgentID");
                RQRStable.Columns.Add("AgentType");
                RQRStable.Columns.Add("BOAID");
                RQRStable.Columns.Add("BOATreminalID");
                RQRStable.Columns.Add("CoOrdinatorID");
                RQRStable.Columns.Add("TerminalID");
                RQRStable.Columns.Add("UserName");
                RQRStable.Columns.Add("Version");
                RQRStable.Columns.Add("Environment");
                RQRStable.Columns.Add("AppType");
                RQRStable.Columns.Add("ProductID");
                RQRStable.Columns.Add("APPCurrency");
                RQRStable.Columns.Add("Platform");
                RQRStable.Columns.Add("CancelAdultCount");
                RQRStable.Columns.Add("CancelChildCount");
                RQRStable.Columns.Add("CancelInfantCount");
                //dsViewPNR.Tables[0].Rows[0]["agenttype"].ToString()
                RQRStable.Rows.Add(dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), "", dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), dsViewPNR.Tables[0].Rows[0]["TERMINAL_ID"].ToString(),
                    "", dsViewPNR.Tables[0].Rows[0]["TERMINAL_ID"].ToString(), dsViewPNR.Tables[0].Rows[0]["USER_NAME"].ToString(), "", "W", "B2B", "", dsViewPNR.Tables[0].Rows[0]["Currency"].ToString(), "B",
                    dsViewPNR.Tables[0].Rows[0]["AdultCount"].ToString(), dsViewPNR.Tables[0].Rows[0]["ChildCount"].ToString(), dsViewPNR.Tables[0].Rows[0]["InfantCount"].ToString());
                DataTable dtViewPNR = new DataTable();
                dtViewPNR = dsViewPNR.Tables[0].Copy();
                dtViewPNR.AcceptChanges();
                foreach (DataRow _Pax in dtViewPNR.Rows)
                {
                    if (!strPaxRef.Contains(_Pax["PAX_REF_NO"].ToString()) ||
                        (strPaxRef.Contains(_Pax["PAX_REF_NO"].ToString()) &&
                        !strSegRef.Contains(_Pax["SEGMENT_NO"].ToString())))
                        _Pax.Delete();
                }
                dtViewPNR.AcceptChanges();
                dsCancelTicketDetails.Tables.Add(dtTcktDet.Copy());
                dsCancelTicketDetails.Tables.Add(dtViewPNR.Copy());
                dsCancelTicketDetails.Tables.Add(RQRStable.Copy());


                string cursorSeqNo = string.Empty;
                DataSet dsPenalty = new DataSet();
                string strPopUp = string.Empty;
                string strpopupCurrency = string.Empty;
                bool onOFFflag = false;
                string request = string.Empty;
                strErrorMsg = string.Empty;
                onOFFflag = checkflag == "ONLINE" ? true : false;// for online request flag
                                                                 //  string Penaltyresult = _RAYS_SERVICE.Fetch_UpdateDetails(dsCancelTicketDetails, strLoginPosId, strLoginPosId, strLoginUserName, "", strLoginPosTId, Convert.ToDecimal(Session["sequenceid"].ToString()), ref cursorSeqNo, ref strErrorMsg, "Fetch_UpdateDetails", "OnlinecancellationFetchPenalty", "", strLoginPosId, strLoginPosTId, ref dsPenalty, ref request, ref onOFFflag);

                string Penaltyresult = _RAYS_SERVICE.Fetch_UpdateDetailsWithCancelPopup(dsCancelTicketDetails, strLoginPosId,
                    strLoginPosId, strLoginUserName, "", strLoginPosTId, Convert.ToDecimal(Session["sequenceid"].ToString()),
                    ref cursorSeqNo, ref strErrorMsg, "Fetch_UpdateDetails", "OnlinecancellationFetchPenalty", "", strLoginPosId,
                    strLoginPosTId, ref dsPenalty, ref request, ref onOFFflag, ref strPopUp, ref strpopupCurrency);

                string logdata = "<REQUEST><URL>" + _RAYS_SERVICE.Url.ToString() + "</URL><INPUT>" + dsCancelTicketDetails.GetXml() + "</INPUT><POSID>" + Session["POS_ID"].ToString()
                                + "</POSID><POSTID>" + strLoginPosId + "</POSTID><USERNAME>" + Session["username"].ToString() + "</USERNAME><CURSORSEQNO>" + cursorSeqNo + "</CURSORSEQNO><ERRORMSG>" + strErrorMsg
                                + "</ERRORMSG></REQUEST><RESPONSE><PENALTY>" + dsPenalty.GetXml() + "</PENALTY><REQ_REF>" + request + "</REQ_REF><FLAG>" + onOFFflag + " " + checkflag + "</FLAG> <STRPOPUP>" + strPopUp + "</STRPOPUP></RESPONSE>";


                DatabaseLog.LogData(Session["username"].ToString(), "E", "CancellationController", strFunctionName, logdata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                Session.Add("Cancelseqid", cursorSeqNo);
                Session.Add("Cancellationreq", string.IsNullOrEmpty(request) ? "" : request); // need to pass in confirm cancellation request
                if (strLoginTerminalType != "T")
                {
                    if ((AirlineCategory == "LCC" && !(dsPenalty != null && dsPenalty.Tables.Count > 0 && dsPenalty.Tables[0].Rows.Count > 0)) || !onOFFflag)
                    {
                        strStatus = "0";
                        strMessage = (strErrorMsg != null && strErrorMsg != "") ? strErrorMsg :
                            (cursorSeqNo != null && cursorSeqNo != "") ? "Your cancellation request has been successfully received with the reference ID :" + cursorSeqNo + " ."
                            : "Unable to get requested details. please contact support team (#03)";
                        strResult = "";

                        return Json(new { Status = "2", Message = "", Result = strMessage });
                    }
                }

                //if (onOFFflag == false && strLoginTerminalType == "W")
                //{

                //    strStatus = "0";
                //    strMessage = (strErrorMsg != null && strErrorMsg != "") ? strErrorMsg :
                //        (cursorSeqNo != null && cursorSeqNo != "") ? "Your cancellation request has been successfully received with the reference ID :" + cursorSeqNo + " ."
                //        : "Unable to get requested details. please contact support team (#03)";
                //    strResult = "";

                //    return Json(new { Status = "2", Message = "", Result = strMessage });
                //}
                DataSet dsResult = new DataSet();
                try
                {
                    byte[] outdata = Convert.FromBase64String(strPopUp);

                    if (outdata != null && outdata.Count() > 0)
                        dsResult = Decompress(outdata);
                }
                catch (Exception ex)
                {
                    return Json(new { Status = "0", Message = strErrorMsg, Result = "" });
                }

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                   
                    string strOnlinePenaltyAmt = dsPenalty != null && dsPenalty.Tables.Count > 0 && dsPenalty.Tables[0].Rows.Count > 0 && dsPenalty.Tables[0].Columns.Contains("PenaltyAmount") ? dsPenalty.Tables[0].Rows[0]["PenaltyAmount"].ToString() : "0";
                    string strOnlineRefundAmt = dsPenalty != null && dsPenalty.Tables.Count > 0 && dsPenalty.Tables[0].Rows.Count > 0 && dsPenalty.Tables[0].Columns.Contains("RefundAmount") ? dsPenalty.Tables[0].Rows[0]["RefundAmount"].ToString() : "0";

                    if (AirlineCategory == "LCC")
                    {
                        if (strOnlinePenaltyAmt != "0")
                        {
                            strOnlinePenaltyAmt = ((Convert.ToDouble(strOnlinePenaltyAmt) / maxfareid) / Convert.ToDouble(totpaxcountwoinfant)).ToString();
                        }
                        if (strOnlineRefundAmt != "0")
                        {
                            strOnlineRefundAmt = ((Convert.ToDouble(strOnlineRefundAmt) / maxfareid) / Convert.ToDouble(totpaxcountwoinfant)).ToString();
                        }
                    }

                    string strAirlineData = string.Empty;
                    string strTravelData = string.Empty;
                    string strPenalityData = string.Empty;
                    // byte[] outdata = Convert.FromBase64String(strErrorMsg);

                    string strresponse = JsonConvert.SerializeObject(dsResult);
                    strPenalityData = dsPenalty != null && dsPenalty.Tables.Count > 0 ? JsonConvert.SerializeObject(dsPenalty.Tables[0]) : "";
                    var qryAirline = (from _Air in dsResult.Tables[0].AsEnumerable()
                                      where _Air["PAX_REF_NO"].ToString() == strPaxRef.Split('|')[0].ToString().Trim()
                                      orderby Convert.ToInt32(_Air["TotalSegment"].ToString()) ascending
                                      select new
                                      {
                                          Flight_No = _Air["AIRLINECODE"].ToString() + " " + _Air["FLIGHTNO"].ToString(),
                                          Segment_No = _Air["TotalSegment"].ToString(),
                                          Origin = GetCityName(_Air["Origin"].ToString().Trim()),
                                          OriginCode = _Air["Origin"].ToString().Trim(),
                                          Destination = GetCityName(_Air["Destination"].ToString().Trim()),
                                          DestinationCode = _Air["Destination"].ToString().Trim(),
                                          Depature = _Air["DEPARTURETIME"].ToString(),
                                          Arrival = _Air["ARRIVALTIME"].ToString(),
                                          Class = _Air["CLASSCODE"].ToString(),
                                          VOID = _Air["VOID"].ToString(),
                                          AirlineCateogry = _Air["TCK_AIRLINE_CATEGORY"].ToString()
                                      }).ToList();
                    //  strAirlineData = JsonConvert.SerializeObject(qryAirline);
                    var qryTravel = (from _Air in dsResult.Tables[0].AsEnumerable()
                                     orderby Convert.ToInt32(_Air["PAX_REF_NO"].ToString()) ascending
                                     select new
                                     {
                                         Pax_Ref = _Air["PAX_REF_NO"].ToString(),
                                         Pax_Name = _Air["Name"].ToString(),
                                         Pax_Type = _Air["PAX_TYPE"].ToString(),
                                         Segment_No = _Air["TotalSegment"].ToString(),
                                         Ticket_No = _Air["TICKETNO"].ToString(),
                                         Origin = GetCityName(_Air["Origin"].ToString().Trim()),
                                         OriginCode = _Air["Origin"].ToString().Trim(),
                                         Destination = GetCityName(_Air["Destination"].ToString().Trim()),
                                         DestinationCode = _Air["Destination"].ToString().Trim(),
                                         Status = _Air["Status"].ToString(),
                                         BasicFare = _Air["BASICFARE"].ToString(),
                                         TaxFare = _Air["TAXAMOUNT"].ToString(),
                                         GrossFare = _Air["GROSS"].ToString(),
                                         FareID = _Air["PCI_FARE_ID"].ToString(),
                                         FareBreakup = _Air["PCI_TAX_BREAKUP"].ToString(),
                                         AgentID = _Air["AgentID"].ToString(),
                                         TerminalID = _Air["TerminalID"].ToString(),
                                         SPNR = _Air["SPNR"].ToString(),
                                         AirlinePNR = _Air["AirlinePNR"].ToString(),
                                         CRSPNR = _Air["CRSPNR"].ToString(),
                                         PaymentMode = _Air["FORMOFPAYMENT"].ToString(),
                                         PlatingCarrier = _Air["AIRLINECODE"].ToString().Trim(),
                                         Airlines = _Air["Airline"].ToString(),
                                         Trip_Desc = _Air["CLASSCODE"].ToString(),
                                         BranchID = _Air["BOABRANCHID"].ToString(),
                                         BookingType = _Air["BookingType"].ToString(),
                                         PCI_DISPLAY_CURRENCY = _Air["PCI_DISPLAY_CURRENCY"].ToString(),
                                         PCI_DISPLAY_ROE = _Air["PCI_DISPLAY_ROE"].ToString(),
                                         PARTIAL = _Air["PARTIAL"].ToString(),
                                         AirlineCateogry = _Air["TCK_AIRLINE_CATEGORY"].ToString(),
                                         CRS_ID = dsViewPNR.Tables[0].Rows[0]["CRS_ID"].ToString(),
                                         VOID = _Air["VOID"].ToString(),
                                         CANCELMODE = _Air["CANCELMODE"].ToString(),
                                         TripNo = _Air["TCK_TRIP_NO"].ToString(),
                                     });
                    var qrytravels = qryTravel.GroupBy(_grp => _grp.Pax_Ref).Select(_grp => _grp.GroupBy(_sgrp => _sgrp.FareID).Select(_sgrp => _sgrp.ToList()).ToList()).ToList();
                    strTravelData = JsonConvert.SerializeObject(qrytravels);
                    DataTable dtTable = new DataTable();
                    dtTable = ConvertToDataTable(qryTravel.ToList());
                    DataSet dsSet = new DataSet();
                    dsSet.Tables.Add(dtTable);
                    /*    Airline Details    */
                    _ResArr[0] = JsonConvert.SerializeObject(qryAirline);
                    /*    Ticket Details     */
                    _ResArr[1] = JsonConvert.SerializeObject(qrytravels);
                    /*    Penality Details   */
                    _ResArr[2] = JsonConvert.SerializeObject(dsResult.Tables[8]);
                    /*    Markup Details     */
                    _ResArr[3] = JsonConvert.SerializeObject(dsResult.Tables[9]);
                    /*    Agency Details     */
                    _ResArr[4] = "";
                    /*    Request Details    */
                    _ResArr[5] = JsonConvert.SerializeObject(dsSet);
                    /*  Online Penalty Amount */
                    _ResArr[6] = strOnlinePenaltyAmt;
                    /*  Online Refund Amount */
                    _ResArr[7] = strOnlineRefundAmt;
                    //SSR Details
                    _ResArr[8] = JsonConvert.SerializeObject(dsResult.Tables[5]);


                    strResult = JsonConvert.SerializeObject(_ResArr);
                    strMessage = "";
                    strStatus = "1";
                }
                else
                {
                    if (cursorSeqNo != "")
                    {
                        strStatus = "0";
                        strMessage = "Your cancellation request has been successfully received with the reference ID :" + cursorSeqNo + " .";
                        strResult = "";
                    }
                    else if (strErrorMsg != null && strErrorMsg != "")
                    {

                        strStatus = "0";
                        strMessage = strErrorMsg;
                        strResult = "";
                    }
                  
                    //else
                    //{
                    //    JsonResult JResult = (JsonResult)OfflineCancellation(strSPNR, PaxDetails, Ticketdetails, PaxcancelationStatus, Ticketcancelstatus, Remarks);
                    //    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    //    JObject JObject = JsonConvert.DeserializeObject<JObject>(serializer.Serialize(JResult.Data));
                    //    strStatus = JObject["Status"].ToString();
                    //    strMessage = JObject["Message"].ToString();
                    //    strResult = JObject["Result"].ToString();
                    //}
                }
                // }
            }
            catch (Exception ex)
            {
                strMessage = "Unable to get requested details. please contact support team (#05)";
                strStatus = "0";
                strResult = ex.ToString();
                DatabaseLog.LogData(strLoginUserName, "X", "CancellationController", strFunctionName + "-ERR", strResult.ToString(), strLoginPosId, strLoginPosTId, strLoginSeqId);
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage });
        }

        public ActionResult OfflineCancellation(string strSPNR, string PaxDetails, string Ticketdetails,
            string PaxcancelationStatus, string Ticketcancelstatus, string Remarks)
        {
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strResult = string.Empty;
            string strFunctionName = string.Empty;
            string strLoginAgentID = Session["AGENTID"] != null ? Session["AGENTID"].ToString() : "";
            string strLoginClientID = Session["CLIENTID"] != null ? Session["CLIENTID"].ToString() : "";
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
            string strTKTFLAG = Session["TKTFLAG"] != null ? Session["TKTFLAG"].ToString() : "DTKT";
            strFunctionName = "OfflineCancellation";
            string getAgentType = string.Empty;
            string getprevilage = string.Empty;
            string departureDate = string.Empty;
            string strErrorMsg = string.Empty;
            ArrayList _ResArr = new ArrayList();
            _ResArr.Add("");//0 PNR RESPONSE
            _ResArr.Add("");//1 PAX RESPONSE
            _ResArr.Add("");//2 TICKET RESPONSE
            try
            {
                if (strLoginPosId == "" || strLoginPosTId == "" || strLoginIPAddress == "" || strLoginSeqId == "" || strLoginTerminalType == "")
                {
                    return Json(new { Status = "-1", Result = "", Message = "Session Expired" });
                }

                #region
                string ConsoleAgent = ConfigurationManager.AppSettings["ConsoleAgent"] != null ? ConfigurationManager.AppSettings["ConsoleAgent"].ToString() : "";
                if (getprevilage.ToString() == "S")
                {
                    strLoginTerminalID = string.Empty;
                }
                else
                {
                    strLoginTerminalID = Session["POS_TID"].ToString();
                }
                if (getAgentType == ConsoleAgent) //Travrays Admin
                {
                    strLoginPosId = string.Empty;
                    strLoginTerminalID = string.Empty;
                }
                if (strLoginTerminalType == "T")
                {
                    strLoginPosId = string.Empty;
                }
                #endregion

                _RAYS_SERVICE.Url = strRaysURL;


                DataSet dsSequenceNo = new DataSet();
                DataTable dataTable = new DataTable();
                DataSet dsPanalty = new DataSet();
                dataTable.Columns.Add("NAME");
                dataTable.Columns.Add("S_PNR");
                dataTable.Columns.Add("SPNR");
                dataTable.Columns.Add("PAX_REF_NO");
                dataTable.Columns.Add("SEGMENT_NO");
                dataTable.Columns.Add("REMARKS");
                dataTable.Columns.Add("DEPDATE");
                dataTable.Columns.Add("ORIGIN");
                dataTable.Columns.Add("DESTINATION");
                dataTable.Columns.Add("AGENT_EMAILID");
                dataTable.Columns.Add("AGENT_CONTACTNO");
                dataTable.Columns.Add("AIRPNR");
                dataTable.Columns.Add("PAXTYPE");
                dataTable.Columns.Add("AIRPORT_ID");
                dataTable.Columns.Add("TRIP_DESC");

                #region log

                string Logdetais = "<CANCELATION REQUEST><PAXDETAILS>" + PaxDetails + "</PAXDETAILS><TICKETDETAILS>" + Ticketdetails + "</TICKETDETAILS></CANCELATION REQUEST>";


                DatabaseLog.LogData(Session["username"].ToString(), "T", "CancellationController", strFunctionName + "-REQ-Input", Logdetais, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                #endregion

                DataSet dsViewPNR = new DataSet();
                dsViewPNR = (DataSet)Session["VIEWPNR_" + strSPNR];

                Ticketdetails = Ticketdetails.TrimEnd('|');
                string[] arrval = Ticketdetails.Split('|');

                PaxDetails = PaxDetails.TrimEnd('|');
                string[] arrval_pax = PaxDetails.Split('|');

                for (int i = 0; i < arrval_pax.Length; i++)
                {
                    string[] paxdetsplit = arrval_pax[i].Split(',');
                    for (int j = 0; j < arrval.Length; j++)
                    {
                        string[] ticketdetsplit = arrval[j].Split(',');

                        var qry = from p in dsViewPNR.Tables[0].AsEnumerable()
                                  where p["PAX_REF_NO"].ToString() == paxdetsplit[2].Trim() && p["SEGMENT_NO"].ToString() == ticketdetsplit[0].Trim()
                                  select p;
                        DataView dv = qry.AsDataView();
                        DataTable dtData = new DataTable();
                        if (dv.Count > 0)
                        {
                            dtData = qry.CopyToDataTable();
                        }
                        DataRow dataRow = dataTable.NewRow();

                        dataRow["NAME"] = dtData.Rows[0]["PASSENGER_NAME"].ToString();
                        dataRow["S_PNR"] = dtData.Rows[0]["S_PNR"].ToString();
                        dataRow["SPNR"] = dtData.Rows[0]["S_PNR"].ToString();
                        dataRow["PAX_REF_NO"] = dtData.Rows[0]["PAX_REF_NO"].ToString();
                        dataRow["SEGMENT_NO"] = dtData.Rows[0]["SEGMENT_NO"].ToString();
                        dataRow["REMARKS"] = Remarks;
                        dataRow["DEPDATE"] = dtData.Rows[0]["DEPTDT"].ToString();
                        dataRow["ORIGIN"] = dtData.Rows[0]["ORIGINCODE"].ToString();
                        dataRow["DESTINATION"] = dtData.Rows[0]["DESTINATIONCODE"].ToString();
                        dataRow["AGENT_EMAILID"] = dtData.Rows[0]["AGENT_EMAIL_ID"].ToString();
                        dataRow["AGENT_CONTACTNO"] = dtData.Rows[0]["CONTACT_NO"].ToString();
                        dataRow["AIRPNR"] = dtData.Rows[0]["AIRLINE_PNR"].ToString();
                        dataRow["PAXTYPE"] = dtData.Rows[0]["PASSENGER_TYPE"].ToString();
                        dataRow["AIRPORT_ID"] = dtData.Rows[0]["AIRPORTID"].ToString();
                        dataRow["TRIP_DESC"] = dtData.Rows[0]["TRIP_DESC"].ToString();
                        dataTable.Rows.Add(dataRow);
                        dataTable.AcceptChanges();
                        departureDate = dtData.Rows[0]["DEPTDT"].ToString();
                    }
                }

                dsSequenceNo.Tables.Add(dataTable.Copy());
                dsSequenceNo.Tables.Add(dsViewPNR.Tables[0].Copy());

                dsSequenceNo.Tables[1].TableName = "Details";


                CultureInfo cii = new CultureInfo("en-GB", true);
                cii.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy HH:mm:ss";
                DateTime dtDepart = Convert.ToDateTime(departureDate, cii);

                ////Get date and time in US Mountain Standard Time 
                DateTime dtDepartureDate = dtDepart;// TimeZoneInfo.ConvertTime(dtDepart, timeZoneInfo);

                DateTime dtToday = DateTime.Now;
                TimeSpan span = dtDepartureDate.Subtract(dtToday);
                double totHours = span.TotalHours;
                if (totHours <= 0)
                {
                    return Json(new { Status = "0", Result = "", Message = "PNR already departured cancellation not allowed" });
                }

                string strSeqNo = string.Empty;
                string output = string.Empty;
                string request = string.Empty;

                strErrorMsg = string.Empty;

                bool cancellationflag = false;

                output = _RAYS_SERVICE.Fetch_UpdateDetails(dsSequenceNo, strLoginPosId, strLoginPosTId, strLoginUserName,
                                strLoginIPAddress, "W", Convert.ToDecimal(strLoginSeqId), ref strSeqNo, ref strErrorMsg,
                                "CancellationController", "Cancelation Request", "",
                                strLoginPosId, strLoginPosTId, ref dsPanalty, ref request, ref cancellationflag);

                #region Log
                StringWriter strWriter = new StringWriter();
                dsSequenceNo.WriteXml(strWriter);
                string LstrDetails = "<CANCELATION><DETAILS>" + strWriter.ToString() + "</DETAILS><OUTPUT>" + output.ToString() +
                   "</OUTPUT></CANCELATION>";

                DatabaseLog.LogData(Session["username"].ToString(), "T", "CancellationController", strFunctionName + "-RES-Input", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                #endregion

                if (output == "1")
                {
                    strStatus = "2";
                    strResult = "Your cancellation request has been successfully received with the reference ID : " + strSeqNo + " .";
                }
                else
                {
                    strStatus = "0";
                    strMessage = (string.IsNullOrEmpty(strErrorMsg) ? "Unable to Process your request" : strErrorMsg);
                }
            }
            catch (Exception ex)
            {
                strMessage = "Unable to get requested details. please contact support team(#05)";
                strStatus = "0";
                strResult = ex.ToString();
                DatabaseLog.LogData(strLoginUserName, "X", "CancellationController", strFunctionName + "-ERR", strResult.ToString(), strLoginPosId, strLoginPosTId, strLoginSeqId);
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage });
        }

        public ActionResult ConfirmCancellation(string strSPNR, string strAirPNR, string strCRSPNR, string strRemarks,
                string strCancellationdata, string strCancelflag, string strRequestData, string strCancelSeq, string strWaivertext, string strRefundFlag)
        {
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strResult = string.Empty;
            string strFunctionName = string.Empty;
            DataSet dsViewPNR = new DataSet();
            string strLoginAgentID = Session["AGENTID"] != null ? Session["AGENTID"].ToString() : "";
            string strLoginClientID = Session["CLIENTID"] != null ? Session["CLIENTID"].ToString() : "";
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
            string strTKTFLAG = Session["TKTFLAG"] != null ? Session["TKTFLAG"].ToString() : "DTKT";
            strFunctionName = "ConfirmCancellation-" + strSPNR;
            try
            {
                _RAYS_SERVICE.Url = strRaysURL;
                strRefundFlag = !string.IsNullOrEmpty(strRefundFlag) ? strRefundFlag : "false";
                if (strCancelflag != "N")
                {
                    dsViewPNR = convertJsonStringToDataSet(strRequestData, "");

                    if (dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                    {

                        DataSet dsFscInput = new DataSet();
                        DataTable dtTcktDetcancel = new DataTable("FAREDETAILS");
                        dtTcktDetcancel.Columns.Add("REFUNDAMOUNT");
                        dtTcktDetcancel.Columns.Add("CANCELCHARGEAMOUNT");
                        dtTcktDetcancel.Columns.Add("COMMISSIONPLBAMOUNT");
                        dtTcktDetcancel.Columns.Add("PENALITYAMOUNT");
                        dtTcktDetcancel.Columns.Add("CREDITAMOUNT");
                        dtTcktDetcancel.Columns.Add("SEGNO");
                        dtTcktDetcancel.Columns.Add("PAXNO");
                        dtTcktDetcancel.Columns.Add("PAXNAME");
                        dtTcktDetcancel.Columns.Add("AGENTID");
                        dtTcktDetcancel.Columns.Add("TERMINALID");
                        dtTcktDetcancel.Columns.Add("CRSPNR");
                        dtTcktDetcancel.Columns.Add("AIRPNR");
                        dtTcktDetcancel.Columns.Add("PAYMENT_MODE");
                        dtTcktDetcancel.Columns.Add("AIRLINE_CODE");
                        dtTcktDetcancel.Columns.Add("AIRLINE_NAME");
                        dtTcktDetcancel.Columns.Add("ORIGIN");
                        dtTcktDetcancel.Columns.Add("DESTINATION");
                        dtTcktDetcancel.Columns.Add("TRIP_TYPE");
                        dtTcktDetcancel.Columns.Add("NEWCRSPNR");
                        dtTcktDetcancel.Columns.Add("NEWAIRPNR");
                        dtTcktDetcancel.Columns.Add("NEWTICKETNO");
                        dtTcktDetcancel.Columns.Add("MARKUP");
                        dtTcktDetcancel.Columns.Add("CLIENTPENALITY");
                        dtTcktDetcancel.Columns.Add("CLIENTID");
                        dtTcktDetcancel.Columns.Add("BOABRANCHID");
                        dtTcktDetcancel.Columns.Add("FORMOFPAYMENT");
                        dtTcktDetcancel.Columns.Add("COORDINATORCODE");
                        dtTcktDetcancel.Columns.Add("POSTERMINALID");
                        dtTcktDetcancel.Columns.Add("STOCKTYPE");
                        dtTcktDetcancel.Columns.Add("POSID");
                        dtTcktDetcancel.Columns.Add("CANCELMODE");
                        dtTcktDetcancel.Columns.Add("REFUND_BREAKUP");

                        string[] strPenalityDet = Regex.Split(strCancellationdata, "SPLITPAX");
                        double totcancelpenalty = 0;
                        string paxpenalty = string.Empty;
                        string farebreakup = string.Empty;
                        for (int _seg = 0; _seg < strPenalityDet.Length - 1; _seg++)
                        {
                            string[] strPaxPenality = Regex.Split(strPenalityDet[_seg], "SPLITPENALITY");
                            for (int i = 0; i < dsViewPNR.Tables[0].Rows.Count; i++)
                            {
                                if (dsViewPNR.Tables[0].Rows[i]["Pax_Ref"].ToString() == strPaxPenality[0]
                                    && strPaxPenality[3].Contains(dsViewPNR.Tables[0].Rows[i]["Segment_No"].ToString()))
                                {
                                    string strMarkup = "0";
                                    if (strPaxPenality[8] == "PPS")
                                        strMarkup = strPaxPenality[5].ToString();
                                    else if (strPaxPenality[8] != "PPS")
                                        strMarkup = strPaxPenality[3].Split(',')[0] == dsViewPNR.Tables[0].Rows[i]["Segment_No"].ToString() ? strPaxPenality[5].ToString() : "0";

                                    totcancelpenalty += Convert.ToDouble(strPaxPenality[3].Split(',')[0] == dsViewPNR.Tables[0].Rows[i]["Segment_No"].ToString() ? strPaxPenality[6].ToString() : "0");

                                    if (strPaxPenality[3].Split(',')[0] == dsViewPNR.Tables[0].Rows[i]["Segment_No"].ToString())
                                        paxpenalty += strPaxPenality[6].ToString() + "|";

                                    farebreakup = strPaxPenality[1].ToString();

                                    double totrefund = strPaxPenality[3].Split(',')[0] == dsViewPNR.Tables[0].Rows[i]["Segment_No"].ToString() ? Convert.ToDouble(strPaxPenality[4].ToString()) : 0;
                                    double totpenalty = Convert.ToDouble(strPaxPenality[3].Split(',')[0] == dsViewPNR.Tables[0].Rows[i]["Segment_No"].ToString() ? strPaxPenality[6].ToString() : "0");

                                    dtTcktDetcancel.Rows.Add(totrefund,
                                                            strPaxPenality[3].Split(',')[0] == dsViewPNR.Tables[0].Rows[i]["Segment_No"].ToString() ? strPaxPenality[7].ToString() : "0", "0",
                                                            strPaxPenality[3].Split(',')[0] == dsViewPNR.Tables[0].Rows[i]["Segment_No"].ToString() ? strPaxPenality[6].ToString() : "0",
                                                            (totrefund - totpenalty).ToString(),
                                                            dsViewPNR.Tables[0].Rows[i]["Segment_No"].ToString(), dsViewPNR.Tables[0].Rows[i]["Pax_Ref"].ToString(),
                                                            dsViewPNR.Tables[0].Rows[i]["Pax_Name"].ToString(), dsViewPNR.Tables[0].Rows[i]["AgentID"].ToString(),
                                                            dsViewPNR.Tables[0].Rows[i]["TerminalID"].ToString(), dsViewPNR.Tables[0].Rows[i]["CRSPNR"].ToString(),
                                                            dsViewPNR.Tables[0].Rows[i]["AirlinePNR"].ToString(), dsViewPNR.Tables[0].Rows[i]["PaymentMode"].ToString(),
                                                            dsViewPNR.Tables[0].Rows[i]["PlatingCarrier"].ToString(), dsViewPNR.Tables[0].Rows[i]["Airlines"].ToString(),
                                                            dsViewPNR.Tables[0].Rows[i]["OriginCode"].ToString(), dsViewPNR.Tables[0].Rows[i]["DestinationCode"].ToString(),
                                                            dsViewPNR.Tables[0].Rows[i]["Trip_Desc"].ToString(), dsViewPNR.Tables[0].Rows[i]["CRSPNR"].ToString(),
                                                            dsViewPNR.Tables[0].Rows[i]["AirlinePNR"].ToString(), dsViewPNR.Tables[0].Rows[i]["Ticket_No"].ToString(),
                                                            strMarkup, "0", dsViewPNR.Tables[0].Rows[i]["AgentID"].ToString(),
                                                            dsViewPNR.Tables[0].Rows[i]["BranchID"].ToString(), dsViewPNR.Tables[0].Rows[i]["PaymentMode"].ToString(), "",
                                                            dsViewPNR.Tables[0].Rows[i]["TerminalID"].ToString(), "OWNSTOCK", dsViewPNR.Tables[0].Rows[i]["AgentID"].ToString(),
                                                            dsViewPNR.Tables[0].Rows[0]["CANCELMODE"].ToString(),
                                                            (strPaxPenality[3].Split(',')[0] == dsViewPNR.Tables[0].Rows[i]["Segment_No"].ToString() ? strPaxPenality[1].ToString() : ""));
                                }
                            }
                        }

                        DataSet dsfare = new DataSet();
                        dsfare.Tables.Add(dtTcktDetcancel.Copy());

                        string[] PAX = (from pax in dsViewPNR.Tables[0].AsEnumerable()
                                        select pax["Pax_Ref"].ToString()).Distinct().ToArray();
                        DataTable dtPNR = new DataTable("NewPNRS");
                        dtPNR.Columns.Add("PNR");
                        dtPNR.Columns.Add("PAXNO");

                        var ppvarPaxRef = from q in dsViewPNR.Tables[0].AsEnumerable()
                                          where q["Pax_Ref"].ToString() == PAX[0].ToString()
                                          select q;
                        DataView dv = new DataView();
                        DataTable dtTriptype = new DataTable();
                        dv = ppvarPaxRef.AsDataView();
                        if (dv != null && dv.Count > 0)
                        {
                            dtTriptype = ppvarPaxRef.CopyToDataTable();
                            dtPNR.Rows.Add(dtTriptype.Rows[0]["SPNR"].ToString(), string.Join(",", PAX));
                        }
                        DataSet dspnr = new DataSet();
                        DataSet dsssrpnr = new DataSet();
                        DataTable dtssrpnr = new DataTable();
                        dspnr.Tables.Add(dtPNR.Copy());
                        dsssrpnr.Tables.Add(dtssrpnr.Copy());

                        string Aircategory = dsViewPNR.Tables[0].Rows[0]["AirlineCateogry"].ToString();
                        bool fsconlineflag = strCancelflag == "O" ? false : dsViewPNR.Tables[0].Rows[0]["CANCELMODE"].ToString() == "O" ? true : false;//(ConfigurationManager.AppSettings["OnlineCancelAirline"].ToString().Contains(Aircategory)) ? true : false;


                        if (Aircategory == "FSC")
                        {
                            //DataSet dsFscRequest = FscCancelRequestFormation(dsViewPNR, strSPNR, strAirPNR, strCRSPNR, strRemarks);
                            //byte[] finaldata = Compress(dsFscRequest);
                            //string strinput = string.Empty;
                            //if (finaldata != null && finaldata.Count() > 0)
                            //    strinput = Convert.ToBase64String(finaldata);

                            //byte[] outdata = _RAYS_SERVICE.OfflineAmadeusCancellation(strinput);

                            DataTable dtgetdetails = new DataTable();
                            dtgetdetails.Columns.Add("USERNAME");
                            dtgetdetails.Columns.Add("IPADDRESS");
                            dtgetdetails.Columns.Add("SEQUENCEID");
                            dtgetdetails.Columns.Add("TERMINALID");
                            dtgetdetails.Columns.Add("PNR");
                            dtgetdetails.Columns.Add("CRS_ID");
                            dtgetdetails.Columns.Add("Stock");
                            dtgetdetails.Columns.Add("GROSSFARE");
                            dtgetdetails.Columns.Add("WaiverCode");
                            dtgetdetails.Columns.Add("WaiverRemarks");
                            dtgetdetails.Columns.Add("CancelPenalty");

                            dtgetdetails.Columns.Add("AgentID");
                            dtgetdetails.Columns.Add("AgentType");
                            dtgetdetails.Columns.Add("APIUSE");
                            dtgetdetails.Columns.Add("APPCurrency");
                            dtgetdetails.Columns.Add("AppType");
                            dtgetdetails.Columns.Add("BOAID");
                            dtgetdetails.Columns.Add("BOATreminalID");
                            dtgetdetails.Columns.Add("CoOrdinatorID");
                            dtgetdetails.Columns.Add("Environment");
                            dtgetdetails.Columns.Add("Platform");
                            dtgetdetails.Columns.Add("ProductID");
                            dtgetdetails.Columns.Add("Version");
                            dtgetdetails.Columns.Add("IsVoid");
                            dtgetdetails.Columns.Add("IsRefundOnly");
                            dtgetdetails.Columns.Add("PAXPENALITY");
                            dtgetdetails.Columns.Add("FAREBREAKUP");

                            DataRow dr = dtgetdetails.NewRow();
                            dr["USERNAME"] = strLoginUserName;
                            dr["IPADDRESS"] = strLoginIPAddress;
                            dr["SEQUENCEID"] = strLoginSeqId;
                            dr["TERMINALID"] = strLoginTerminalID;
                            dr["PNR"] = strSPNR;
                            dr["CRS_ID"] = dsViewPNR.Tables[0].Rows[0]["CRS_ID"].ToString();//lnkcancelreq.Tag != null ? lnkcancelreq.Tag : "";
                            dr["Stock"] = dsViewPNR.Tables[0].Rows[0]["CRS_ID"].ToString();//lnkcancelreq.Tag != null ? lnkcancelreq.Tag : "";
                            dr["GROSSFARE"] = "";
                            dr["CancelPenalty"] = strLoginTerminalType != "T" ? "" : totcancelpenalty.ToString();//AMspenalityAmount.ToString();
                            dr["WaiverCode"] = strWaivertext;// txtwavecode.Text;
                            dr["WaiverRemarks"] = strRemarks;

                            dr["AgentID"] = strLoginAgentID;
                            dr["AgentType"] = strLoginAgentType;
                            dr["APIUSE"] = null;
                            dr["APPCurrency"] = "";
                            dr["AppType"] = "B2B";
                            dr["BOAID"] = strLoginPosId;
                            dr["BOATreminalID"] = strLoginPosTId;
                            dr["CoOrdinatorID"] = "";
                            dr["Environment"] = "";
                            dr["Platform"] = "B";
                            dr["ProductID"] = "INRC02";
                            dr["Version"] = "";
                            dr["IsVoid"] = dsViewPNR.Tables[0].Rows[0]["VOID"].ToString();//lnkcancelreq.Text.Contains("Void") ? "Y" : "N";
                            dr["IsRefundOnly"] = strRefundFlag;
                            dr["PAXPENALITY"] = strLoginTerminalType != "T" ? "" : paxpenalty.TrimEnd('|');// AMSPENALITY;
                            dr["FAREBREAKUP"] = strLoginTerminalType != "T" ? "" : farebreakup;// AMSPENALITY;

                            dtgetdetails.Rows.Add(dr);

                            string strinput = string.Empty;
                            dsFscInput = new DataSet();
                            dsFscInput.Tables.Add(dtgetdetails.Copy());
                        }

                        Byte[] picbyte = Compress(dsfare);
                        Byte[] picbyte1 = Compress(dspnr);
                        Byte[] picbyte2 = Compress(dsssrpnr);
                        Byte[] bytfscinput = Compress(dsFscInput);


                        StringWriter strWriter1 = new StringWriter();
                        dsfare.WriteXml(strWriter1);
                        StringWriter strWriter2 = new StringWriter();
                        dspnr.WriteXml(strWriter2);
                        StringWriter strWriter3 = new StringWriter();
                        dsssrpnr.WriteXml(strWriter3);
                        StringWriter strWriter4 = new StringWriter();
                        dsFscInput.WriteXml(strWriter4);

                        bool resultflag = strCancelflag == "O" ? false : true;
                        #region RequestLog
                        string ReqTime = "ONLINECANCELLATIONREQTIME:" + DateTime.Now;
                        string xmldata = "<EVENT><REQUEST>CANCELLATIONDETAILS_Update_Cancellation_Status_new_BOA</REQUEST>" +
                                         "<CANCELLATIONDATA1>" + strWriter1 + "</CANCELLATIONDATA1>" +
                                         "<CANCELLATIONDATA2>" + strWriter2 + "</CANCELLATIONDATA2>" +
                                         "<CANCELLATIONDATA3>" + strWriter3 + "</CANCELLATIONDATA3>" +
                                         "<FSCINPUT>" + strWriter4 + "</FSCINPUT>" +
                                         "<strtermonalid>" + Session["POS_TID"].ToString() + "</strtermonalid>" +
                                         "<REMARKS>" + strRemarks + "</REMARKS>" +
                                         "<CANCELLAIONREQ>" + (Session["Cancellationreq"] != null ? Session["Cancellationreq"].ToString() : "") + "</CANCELLAIONREQ>" +
                                         "<RESULT>" + resultflag + "</RESULT>" +
                                         "<spnr>" + strSPNR + "</spnr>" +
                                         "<FSCONLINEFLAG>" + fsconlineflag + "</FSCONLINEFLAG>" +
                                          "<AIRCATEGORY>" + Aircategory + "</AIRCATEGORY>" +
                                        "</EVENT>";
                        string LstrDetails = "<CANCELLATION_REQUEST><URL>[<![CDATA[" + _RAYS_SERVICE.Url + "]]>]</URL><QUERY>" + xmldata + "</QUERY><REQTIME>" + ReqTime + "</REQTIME></CANCELLATION_REQUEST>";
                        #endregion
                        DatabaseLog.LogData(strLoginUserName, "E", "CancellationController", strFunctionName + "-REQUEST", LstrDetails.ToString(), strLoginPosId, strLoginPosTId, strLoginSeqId);

                        //   string strresult = _RAYS_SERVICE.Update_Cancellation_Status_new_BOA(Convert.ToBase64String(picbyte), Convert.ToBase64String(picbyte1), Session["POS_TID"].ToString(),
                        //    strSPNR.ToString(), "F", "", "", "", "", "0", strRemarks, strPagename, strFunctionName + "-Confirm", 0, 0, Convert.ToBase64String(picbyte2), strLoginAgentType, Session["Cancellationreq"] != null ? Session["Cancellationreq"].ToString() : "", resultflag);

                        string strresult = _RAYS_SERVICE.Update_Cancellation_Status_new_BOA_New(Convert.ToBase64String(picbyte),
                            Convert.ToBase64String(picbyte1), Session["POS_TID"].ToString(), strSPNR.ToString(), "F", "", "", "", "",
                            "0", strRemarks, strPagename, strFunctionName + "-Confirm", 0, 0, Convert.ToBase64String(picbyte2),
                            strLoginAgentType, Session["Cancellationreq"] != null ? Session["Cancellationreq"].ToString() : "",
                            resultflag, strLoginTerminalType, strLoginTerminalID, Aircategory, fsconlineflag, Convert.ToBase64String(bytfscinput));

                        #region RequestLog
                        ReqTime = "ONLINECANCELLATIONRESTIME:" + DateTime.Now;
                        xmldata = "<EVENT><RESPONSE>CANCELLATIONDETAILS_Update_Cancellation_Status_new_BOA</RESPONSE>" +
                                        "<CANCELLATIONDATA1>" + strWriter1 + "</CANCELLATIONDATA1>" +
                                        "<CANCELLATIONDATA2>" + strWriter2 + "</CANCELLATIONDATA2>" +
                                        "<strtermonalid>" + Session["POS_TID"].ToString() + "</strtermonalid>" +
                                        "<spnr>" + strSPNR + "</spnr>" +
                                        "<strresult>" + strresult + "</strresult>" +
                                        "</EVENT>";
                        LstrDetails = "<CANCELLATION_RESPONSE><URL>[<![CDATA[" + _RAYS_SERVICE.Url + "]]>]</URL><QUERY>" + xmldata + "</QUERY><REQTIME>" + ReqTime + "</REQTIME></CANCELLATION_RESPONSE>";
                        #endregion
                        DatabaseLog.LogData(strLoginUserName, "E", "CancellationController", strFunctionName + "-RESPONSE", LstrDetails.ToString(), strLoginPosId, strLoginPosTId, strLoginSeqId);
                        if (strresult == "1") //&& resultflag == true
                        {
                            strResult = "PNR cancelled successfully";
                            strMessage = strResult;
                            strStatus = strresult;// strresult.Contains("Ticket Cancelled Successfully") ? "1" : "0";
                        }
                        else if (!string.IsNullOrEmpty(strresult) && strresult.ToUpper().Contains("SUCCESSFULLY"))
                        {
                            strResult = strresult;
                            strMessage = strresult;
                            strStatus = "1";
                        }
                        else if (strresult != null && strresult != "" && strresult.StartsWith("OUT"))
                        {
                            strStatus = strLoginTerminalType != "T" ? "1" : "3";
                            strMessage = strLoginTerminalType != "T" ? "Your cancellation request has been successfully received" : strresult.Split('-').Length > 1 ? strresult.Split('-')[1] : strresult;
                            strResult = "";
                        }
                        else if (!string.IsNullOrEmpty(strresult) && strresult.ToUpper().Contains("DO YOU WISH TO PROCEED REFUND"))
                        {
                            strStatus = strLoginTerminalType != "T" ? "1" : "4";
                            strMessage = strLoginTerminalType != "T" ? "Your cancellation request has been successfully received" : strresult;
                            strresult = "";
                        }
                        else
                        {
                            strResult = "";
                            strMessage = string.IsNullOrEmpty(strresult) ? "Unable to cancel PNR please contact support team. (#03)" : strresult;
                            strStatus = "0";
                        }
                    }
                }
                else  // Cancel cancellation request
                {
                    string cancelseq = Session["Cancelseqid"] != null && Session["Cancelseqid"].ToString() != "" ? Session["Cancelseqid"].ToString() : "";
                    strCancelSeq = cancelseq;
                    string cancelresponse = _RAYS_SERVICE.UpdateCancelTicketStatus_BOA(strSPNR, strCancelSeq, strRemarks, strLoginUserName, strLoginIPAddress, strLoginSeqId, strPagename, strFunctionName + "-Cancel", strLoginTerminalID, "", "CANCEL");
                    string XML = "<CANCELREQUEST><SPNR>" + strSPNR + "</SPNR><CANCELSEQ>" + strCancelSeq + "</CANCELSEQ><REMARKS>" + strRemarks + "</REMARKS><USERNAME>" + strLoginUserName + "</USERNAME><FLAG>CANCEL</FLAG></CANCELREQUEST>";
                    DatabaseLog.LogData(strLoginUserName, "E", "CancellationController", strFunctionName + "-CANCEL", XML.ToString(), strLoginPosId, strLoginPosTId, strLoginSeqId);
                    strMessage = "PNR request cancelled successfully";
                    strStatus = "2";
                }
            }
            catch (Exception ex)
            {
                strMessage = "Unable to cancel the pnr please contact support team(#05)";
                strStatus = "0";
                strResult = ex.ToString();
                DatabaseLog.LogData(strLoginUserName, "X", "CancellationController", strFunctionName + "-ERR", strResult.ToString(), strLoginPosId, strLoginPosTId, strLoginSeqId);
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage });
        }

        public DataSet convertJsonStringToDataSet(string jsonString, string strFunction)
        {
            DataSet dsResultset = new DataSet();
            try
            {
                XmlDocument _xmlDoc = new XmlDocument();
                jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
                _xmlDoc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
                dsResultset.ReadXml(new XmlNodeReader(_xmlDoc));
                return dsResultset;
            }
            catch (Exception)
            {
                return dsResultset;
            }
        }

        public static byte[] Compress(DataSet dataSet)
        {
            Byte[] data;
            MemoryStream mem = new MemoryStream();
            GZipStream zip = new GZipStream(mem, CompressionMode.Compress);
            dataSet.WriteXml(zip, XmlWriteMode.WriteSchema);
            zip.Close();
            data = mem.ToArray();
            mem.Close();
            return data;
        }

        public static DataSet Decompress(Byte[] data)
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
                string Message = ex.ToString();
                string Description = "<ERROR><DESC>" + Message + "</DESC></ERROR>";
                return null;
            }
        }

        public static string GetCityName(string strCityCode)
        {
            string strCityName = string.Empty;
            try
            {
                strCityCode = strCityCode.Trim();
                if (AirportNames.ContainsKey(strCityCode))
                {
                    var foo = AirportNames[strCityCode];
                    return foo.ToString();
                }
                else
                {
                    DataSet dsAirways = new DataSet();
                    dsAirways.ReadXml(System.Web.Hosting.HostingEnvironment.MapPath("~/XML/CityAirport_Lst.xml").ToString());
                    var qryAirlineName = from p in dsAirways.Tables
                                       ["AIR"].AsEnumerable()
                                         where p.Field<string>
                                       ("ID") == strCityCode
                                         select p;
                    DataView dvAirlineCode = qryAirlineName.AsDataView();
                    if (dvAirlineCode.Count == 0)
                        strCityName = strCityCode;
                    else
                    {
                        DataTable dtAilineCode = new DataTable();
                        dtAilineCode = qryAirlineName.CopyToDataTable();
                        strCityName = dtAilineCode.Rows[0]["AN"].ToString().Split('-')[0];
                        AirportNames.Add(strCityCode, strCityName);
                    }
                }
            }
            catch (Exception ex)
            {
                strCityName = strCityCode;
            }
            return strCityName;
        }

        public DataTable ConvertToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names   
            PropertyInfo[] oProps = null;

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

        public DateTime TodayDateinTimeZone()
        {
            //Convert India Standard Time Zone
            string dt = DateTime.Now.ToString();
            DateTime localDateTime = DateTime.Parse(dt); // Local .NET timeZone.
            DateTime utcDateTime = localDateTime.ToUniversalTime();

            // string nzTimeZoneKey = "India Standard Time";
            string nzTimeZoneKey = ConfigurationManager.AppSettings["Servertimezone"].ToString();
            TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(nzTimeZoneKey);
            DateTime nzDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, nzTimeZone);
            return nzDateTime;

        }

        //public DataSet FscCancelRequestFormation(DataSet dsViewPNR, string strSPNR, string strAirPNR, string strCRSPNR, string strRemarks)
        //{
        //    string strLoginAgentID = Session["AGENTID"] != null ? Session["AGENTID"].ToString() : "";
        //    string strLoginClientID = Session["CLIENTID"] != null ? Session["CLIENTID"].ToString() : "";
        //    string strLoginTerminalID = Session["TERMINALID"] != null ? Session["TERMINALID"].ToString() : "";
        //    string strLoginUserName = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "";
        //    string strLoginIPAddress = Session["IPADDRESS"] != null ? Session["IPADDRESS"].ToString() : "";
        //    string strLoginTerminalType = Session["TERMINALTYPE"] != null ? Session["TERMINALTYPE"].ToString() : "";
        //    string strLoginAgentType = Session["CLIENTTYPE"] != null ? Session["CLIENTTYPE"].ToString() : "";
        //    string strLoginBranchId = Session["BRANCHID"] != null ? Session["BRANCHID"].ToString() : "";
        //    string strLoginSeqId = Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : DateTime.Now.ToString("ddMMyyyy");
        //    string strLoginPosId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
        //    string strLoginPosTId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
        //    string strLoginPlatform = Session["PLATFORM"] != null ? Session["PLATFORM"].ToString() : "";
        //    string strTKTFLAG = Session["TKTFLAG"] != null ? Session["TKTFLAG"].ToString() : "DTKT";

        //    DataTable dtgetdetails = new DataTable();
        //    dtgetdetails.Columns.Add("USERNAME");
        //    dtgetdetails.Columns.Add("IPADDRESS");
        //    dtgetdetails.Columns.Add("SEQUENCEID");
        //    dtgetdetails.Columns.Add("TERMINALID");
        //    dtgetdetails.Columns.Add("PNR");
        //    dtgetdetails.Columns.Add("CRS_ID");
        //    dtgetdetails.Columns.Add("GROSSFARE");
        //    dtgetdetails.Columns.Add("WaiverCode");
        //    dtgetdetails.Columns.Add("WaiverRemarks");
        //    dtgetdetails.Columns.Add("CancelPenalty");

        //    dtgetdetails.Columns.Add("AgentID");
        //    dtgetdetails.Columns.Add("AgentType");
        //    dtgetdetails.Columns.Add("APIUSE");
        //    dtgetdetails.Columns.Add("APPCurrency");
        //    dtgetdetails.Columns.Add("AppType");
        //    dtgetdetails.Columns.Add("BOAID");
        //    dtgetdetails.Columns.Add("BOATreminalID");
        //    dtgetdetails.Columns.Add("CoOrdinatorID");
        //    dtgetdetails.Columns.Add("Environment");
        //    dtgetdetails.Columns.Add("Platform");
        //    dtgetdetails.Columns.Add("ProductID");
        //    dtgetdetails.Columns.Add("Version");
        //    dtgetdetails.Columns.Add("IsVoid");
        //    dtgetdetails.Columns.Add("IsRefundOnly");
        //    dtgetdetails.Columns.Add("PAXPENALITY");

        //    DataRow dr = dtgetdetails.NewRow();
        //    dr["USERNAME"] = strLoginUserName;
        //    dr["IPADDRESS"] = strLoginIPAddress;
        //    dr["SEQUENCEID"] = strLoginSeqId;
        //    dr["TERMINALID"] = strLoginTerminalID;
        //    dr["PNR"] = strSPNR;
        //    dr["CRS_ID"] = "";//lnkcancelreq.Tag != null ? lnkcancelreq.Tag : "";
        //    dr["GROSSFARE"] = "";
        //    dr["CancelPenalty"] = "";//AMspenalityAmount.ToString();
        //    dr["WaiverCode"] = "";// txtwavecode.Text;
        //    dr["WaiverRemarks"] = strRemarks;

        //    dr["AgentID"] = strLoginAgentID;
        //    dr["AgentType"] = strLoginAgentType;
        //    dr["APIUSE"] = null;
        //    dr["APPCurrency"] = "";
        //    dr["AppType"] = "B2B";
        //    dr["BOAID"] = strLoginPosId;
        //    dr["BOATreminalID"] = strLoginPosTId;
        //    dr["CoOrdinatorID"] = "";
        //    dr["Environment"] = "";
        //    dr["Platform"] = "B";
        //    dr["ProductID"] = "INRC02";
        //    dr["Version"] = "";
        //    dr["IsVoid"] = "N";//lnkcancelreq.Text.Contains("Void") ? "Y" : "N";
        //    dr["IsRefundOnly"] = false;
        //    dr["PAXPENALITY"] = "";// AMSPENALITY;

        //    dtgetdetails.Rows.Add(dr);

        //    string strinput = string.Empty;
        //    DataSet dsdetails = new DataSet();
        //    dsdetails.Tables.Add(dtgetdetails.Copy());

        //    return dsdetails;


        //}
    }
}
