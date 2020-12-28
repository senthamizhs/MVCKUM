using Newtonsoft.Json;
using STSTRAVRAYS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using STSTRAVRAYS.Rays_service;
using System.Globalization;
using System.Reflection;
using System.Net;
using System.Text.RegularExpressions;
using STSTRAVRAYS.InplantService;
using System.Runtime.Serialization;

namespace STSTRAVRAYS.Controllers
{
    public class PnrController : LoginController
    {
        Base.ServiceUtility Serv = new Base.ServiceUtility();
        string strProduct = ConfigurationManager.AppSettings["Producttype"] != null ? ConfigurationManager.AppSettings["Producttype"].ToString() : "";
        string terminalType = ConfigurationManager.AppSettings["TerminalType"].ToString();
        string strBranchCredit = ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"] != null ? ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"].ToString().ToUpper() : "";

        public ActionResult PnrVerification()
        {
            #region UsageLog
            string PageName = "Manage Booking";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e)
            {
            }
            #endregion

            if (Request.QueryString["SECKEY"] != null && Request.QueryString["SECKEY"] != "")
            {
                string sesskey = "";

                Session.Add("privilage", "S");
                Session.Add("ticketing", "Y");
                string Encquery = Request.QueryString["SECKEY"];
                string today = DateTime.Now.ToString("dd/MM/yyyy");
                string Querystring = Base.DecryptKEY(Encquery, "RIYA" + today);


                string[] keyval = new string[20];
                string[] Query = Querystring.Split('&');
                sesskey = "";
                string FLAG = string.Empty;
                if (Request.QueryString["MOBJOD_FLG"] != null && Request.QueryString["MOBJOD_FLG"] != "")
                {
                    FLAG = Request.QueryString["MOBJOD_FLG"].ToString();
                    Session["FLAG"] = FLAG;
                }

                Session["FLAG"] = FLAG;
                if (Session["agentid"] == null || Session["agentid"].ToString() == "")
                {
                    for (int i = 0; i < Query.Length; i++)
                    {
                        if (Query[i].Split('=')[0].Trim().ToUpper() == "AGENTID")
                        {
                            Session.Add("POS_ID", Query[i].Split('=')[1].Trim());
                        }
                        if (Query[i].Split('=')[0].Trim().ToUpper() == "TERMINALID")
                        {
                            Session.Add("POS_TID", Query[i].Split('=')[1].Trim());
                        }
                        if (Query[i].Split('=')[0].Trim().ToUpper() == "agenttype")
                        {
                            Session.Add("AGENTTYPE", Query[i].Split('=')[1].Trim());
                        }
                        else
                        {
                            Session.Add(Query[i].Split('=')[0].Trim().ToUpper() + sesskey, Query[i].Split('=')[1].Trim());
                        }
                    }
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

            if (Session["agentid"] == null || Session["agentid"].ToString() == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            if (Request.QueryString["strSPNRNO"] != null && Request.QueryString["strSPNRNO"] != "")
            {
                RequestViewPNRFunction(Request.QueryString["strSPNRNO"].ToString(), "", "", "");
            }

            ViewBag.tabflag = Request.QueryString["Tab"] != null && Request.QueryString["Tab"].Trim() != "" ? Request.QueryString["Tab"].Trim() : "0";
            ViewBag.ServerDateTime = Base.LoadServerdatetime();

            Session["TKTFLAG"] = Request.QueryString["Flag"] != null && Request.QueryString["Flag"].Trim() != "" ? "QTKT" : "DTKT";

            return View("~/Views/AfterBooking/PnrVerification.cshtml");
        }

        #region BBRules
        public ActionResult Bestbuyfarerule()
        {
            string Agentid = string.Empty;
            string TerminalId = string.Empty;
            string Username = string.Empty;
            string Faretype = string.Empty;
            string Errormgs = string.Empty;
            string ipaddress = string.Empty;
            string sequenceid = string.Empty;
            //RaysService _RaysService = new RaysService();
            InplantService.Inplantservice _inplantservice = new InplantService.Inplantservice();

            if (Request.QueryString["agentid"] != "" && Request.QueryString["agentid"] != null)
            {
                Agentid = Request.QueryString["agentid"].ToString();
            }
            if (Request.QueryString["tid"] != "" && Request.QueryString["tid"] != null)
            {
                TerminalId = Request.QueryString["tid"].ToString();
            }
            if (Request.QueryString["username"] != "" && Request.QueryString["username"] != null)
            {
                Username = Request.QueryString["username"].ToString();
            }
            if (Request.QueryString["IPA"] != "" && Request.QueryString["IPA"] != null)
            {
                Session.Add("ipAddress", Request.QueryString["IPA"].ToString());
            }
            if (Request.QueryString["BBtype"] != "" && Request.QueryString["BBtype"] != null)
            {
                Faretype = Request.QueryString["BBtype"].ToString();
            }
            if (Agentid != null && TerminalId != null && Username != null && Agentid != "" && TerminalId != "" && Username != "")
            {
                DataSet dst = new DataSet();
                Hashtable hst = new Hashtable();
                string formrule = string.Empty;
                StringBuilder sbd = new StringBuilder();
                hst.Add("STATUS", "1");
                hst.Add("BBTYPE", Faretype);
                ipaddress = Session["ipAddress"] != null && Session["ipAddress"] != "" ? Session["ipAddress"].ToString() : "";
                sequenceid = Session["sequenceid"] != null && Session["sequenceid"] != "" ? Session["sequenceid"].ToString() : "";
                _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                dst = _inplantservice.Fetch_Best_Buy_Content(Username, ipaddress, sequenceid, "Flight_view", "Bestbuyfarerule");
                if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Columns.Contains("BBR_CONTENT") && dst.Tables[0].Rows.Count > 0)
                {
                    string formrequest = dst.Tables[0].Rows[0]["BBR_CONTENT"].ToString();
                    string[] rulearr = Regex.Split(formrequest, "TravRaJeSh");
                    if (Faretype == "BB")
                    {
                        sbd.Append("<div><div style='float: left;'><img src='/Images/TR_logo.png'/></div><div style='height: 100px; text-align: center;'><br/><br/><label style='font-size: 26px; font-weight: bold; color: #27a5f5;font-family: Open Sans;'>TravRays&nbsp;BestBuy&nbsp;Rules</label></div>");
                    }
                    else if (Faretype == "BF")
                    {
                        sbd.Append("<div><div style='float: left;'><img src='/Images/TR_logo.png'/></div><div style='height: 100px; text-align: center;'><br/><br/><label style='font-size: 26px; font-weight: bold; color: #27a5f5;font-family: Open Sans;'>TravRays&nbsp;Bargain&nbsp;Fare&nbsp;Rules</label></div>");
                    }
                    else if (Faretype == "SM")
                    {
                        sbd.Append("<div><div style='float: left;'><img src='/Images/TR_logo.png'/></div><div style='height: 100px; text-align: center;'><br/><br/><label style='font-size: 26px; font-weight: bold; color: #27a5f5;font-family: Open Sans;'>TravRays&nbsp;Seamless&nbsp;Booking&nbsp;Rules</label></div>");
                    }
                    else if (Faretype == "HB")
                    {
                        sbd.Append("<div><div style='float: left;'><img src='/Images/TR_logo.png'/></div><div style='height: 100px; text-align: center;'><br/><br/><label style='font-size: 26px; font-weight: bold; color: #27a5f5;font-family: Open Sans;'>TravRays&nbsp;Hold&nbsp;Booking&nbsp;Rules</label></div>");
                    }
                    sbd.Append("<div><table style='padding: 2%;'>");
                    for (var i = 0; i < rulearr.Length; i++)
                    {
                        if (rulearr[i].ToString() != "")
                        {
                            int j = i + 1;
                            sbd.Append("<tr style='line-height: 30px;'><td><span style='font-weight: bold;'>Rule " + j + ":</span><span style='margin-left: 2%;'>" + rulearr[i].ToString() + "</span></td></tr>");
                        }
                    }
                    sbd.Append("</table></div>");
                    sbd.Append("</div>");

                }
                if (sbd.ToString() != "")
                {
                    ViewBag.Bestbuyrule = sbd.ToString();
                }
                string XMl = "<EVENT><REQUEST><PROCEDURE>P_FETCH_BESTBUY_RULES</PROCEDURE><STATUS>1</STATUS></REQUEST><RESPONSE>" + dst.GetXml() + "</RESPONSE><STRINGBUILDER>[<![CDATA[" + formrule.ToString() + "]]>]</STRINGBUILDER><Error>" + Errormgs + "</Error></EVENT>";
                DatabaseLog.LogData(Username, "E", "Bestfarerule", "Bestfarerule", XMl.ToString(), Agentid, TerminalId, "");
            }

            return View();
        }

        public ActionResult Bestbuyfareruleweb(string Booktype)
        {

            ArrayList BBRules = new ArrayList();
            InplantService.Inplantservice _inplantservice = new InplantService.Inplantservice();
            BBRules.Add("");
            BBRules.Add("");
            int error = 0;
            int response = 1;
            string Faretype = Booktype;
            string Errormgs = string.Empty;
            try
            {
                string sequnceID = Session["sequenceid"].ToString();
                string Agentid = Session["POS_ID"].ToString();
                string TerminalId = Session["POS_TID"].ToString();//"TASHA040000102";
                string Username = Session["username"].ToString();
                string strBranchId = Session["branchid"].ToString();
                string Ipaddress = Session["ipAddress"].ToString();
                string getprevilage = Session["privilage"].ToString();
                string getAgentType = Session["agenttype"].ToString();

                if (Agentid != null && TerminalId != null && Username != null && Agentid != "" && TerminalId != "" && Username != "")
                {
                    DataSet dst = new DataSet();
                    string formrule = string.Empty;
                    _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();

                    dst = _inplantservice.Fetch_Best_Buy_Content(Username, Ipaddress, sequnceID, "Flight_view", "Bestbuyfareruleweb");
                    if (dst != null && dst.Tables.Count > 0 && dst.Tables[0].Columns.Contains("BBR_CONTENT") && dst.Tables[0].Rows.Count > 0)
                    {
                        string formrequest = dst.Tables[0].Rows[0]["BBR_CONTENT"].ToString();
                        string[] rulearr = Regex.Split(formrequest, "TravRaJeSh");
                        if (Faretype == "BB")
                        {
                            formrule += "<div><div style='float: left;'><img src='/Images/TR_logo.png'/></div><div style='height: 100px; text-align: center;'><br/><br/><label style='font-size: 26px; font-weight: bold; color: #27a5f5;font-family: Open Sans;'>TravRays&nbsp;BestBuy&nbsp;Rules</label></div>";
                        }
                        else if (Faretype == "BF")
                        {
                            formrule += "<div><div style='float: left;'><img src='/Images/TR_logo.png'/></div><div style='height: 100px; text-align: center;'><br/><br/><label style='font-size: 26px; font-weight: bold; color: #27a5f5;font-family: Open Sans;'>TravRays&nbsp;Bargain&nbsp;Fare&nbsp;Rules</label></div>";
                        }
                        else if (Faretype == "SM")
                        {
                            formrule += "<div><div style='float: left;'><img src='/Images/TR_logo.png'/></div><div style='height: 100px; text-align: center;'><br/><br/><label style='font-size: 26px; font-weight: bold; color: #27a5f5;font-family: Open Sans;'>TravRays&nbsp;Seamless&nbsp;Booking&nbsp;Rules</label></div>";
                        }
                        else if (Faretype == "HB")
                        {
                            formrule += "<div><div style='float: left;'><img src='/Images/TR_logo.png'/></div><div style='height: 100px; text-align: center;'><br/><br/><label style='font-size: 26px; font-weight: bold; color: #27a5f5;font-family: Open Sans;'>TravRays&nbsp;Hold&nbsp;Booking&nbsp;Rules</label></div>";
                        }
                        formrule += "<div><table style='padding: 2%;'>";
                        for (var i = 0; i < rulearr.Length; i++)
                        {
                            if (rulearr[i].ToString() != "")
                            {
                                int j = i + 1;
                                formrule += "<tr style='line-height: 30px;'><td><span style='font-weight: bold;'>Rule " + j + ":</span><span style='margin-left: 2%;'>" + rulearr[i].ToString() + "</span></td></tr>";
                            }
                        }
                        formrule += "</table></div>";
                        formrule += "</div>";

                    }
                    if (formrule != "")
                    {
                        BBRules[response] = formrule.ToString();
                    }
                    else
                    {
                        BBRules[error] = "No Records Found";
                    }
                    string XMl = "<EVENT><REQUEST><PROCEDURE>P_FETCH_BESTBUY_RULES</PROCEDURE><STATUS>1</STATUS></REQUEST><RESPONSE>" + dst.GetXml() + "</RESPONSE><STRINGBUILDER>[<![CDATA[" + formrule.ToString() + "]]>]</STRINGBUILDER><Error>" + Errormgs + "</Error></EVENT>";
                    DatabaseLog.LogData(Username, "E", "Bestfarerule", "Bestfarerule", XMl.ToString(), Agentid, TerminalId, "");
                }
            }
            catch (Exception _ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Fetchfarerule", "Fetchfarerule-Error", _ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            return Json(new { Status = "", Message = "", Result = BBRules });
        }

        #endregion

        
        public ActionResult BookedHistory()
        {

            if (Session["agentid"] == null || Session["agentid"].ToString() == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }

            return View("~/Views/Flights/BookedHistory.cshtml");


        }

        #region booked history airline

        public ActionResult RequestViewBookedFunction(string fromdate, string todate, string traveldate, string sPNR, string csrPNR, string airPNR, string firstname,
            string lastname, string status, string payment, string rTraveldate, string rBlockPnr, string airlinecode, string Flightno, string AgentId, bool strMigratedAgent)
        {
            #region UsageLog
            string PageName = "Booked History";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
            }
            catch (Exception e)
            {

            }
            #endregion
            ArrayList result = new ArrayList();
            RaysService _RaysService = new RaysService();
            Inplantservice Inplntsvc = new Inplantservice();
            DataSet ds_res = new DataSet();
            DataSet d2 = new DataSet();
            result.Add("");
            result.Add("");
            result.Add("");

            int error = 0;
            int responce = 1;

            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string strAgentID = string.Empty;
            string terminalType = string.Empty;
            string strResponse = string.Empty;
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;
            string getprevilage = string.Empty;
            string getAgentType = string.Empty;

            byte[] decompress = new byte[] { };
            ArrayList array_bookedHistory = new ArrayList();
            StringBuilder strBookedHistoryBuilder = new StringBuilder();
            array_bookedHistory.Add("");
            array_bookedHistory.Add("");

            strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
            strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
            Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            terminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();// "W";
            string POS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string POS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"].ToString() != "") ? Session["TKTFLAG"].ToString() : "";
            string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");

            try
            {
                if (strTerminalId == "" || strUserName == "" || strAgentID == "" || Ipaddress == "" || sequnceID == "" || terminalType == "" || POS_ID == "" || POS_TID == "" || strTKTFLAG == "")
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }
                strResponse = "";
                getprevilage = Session["privilage"] != null && Session["privilage"].ToString() != "" ? Session["privilage"].ToString() : "";
                getAgentType = Session["agenttype"] != null && Session["agenttype"].ToString() != "" ? Session["agenttype"].ToString() : "";
                string ConsoleAgent = ConfigurationManager.AppSettings["ConsoleAgent"].ToString();// "";
                string fromDate = fromdate;
                string toDate = todate;
                string travelDate = string.Empty;
                string traveltoDate = string.Empty;
                string payment_type = payment;
                string blockdate = "";
                DataSet dsBookedHistory = new DataSet();

                string strErrot = Flightno;
                if ((string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate)) && rBlockPnr.ToUpper() != "YES")
                {
                    result[error] = "From date and to date are mandatory";
                    return Json(new { Status = "00", Message = "", Result = result });
                }

                if (fromDate != null && fromDate != "")
                {
                    fromDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    travelDate = "";
                    traveltoDate = "";
                }

                if (toDate != null && toDate != "")
                {
                    toDate = DateTime.ParseExact(todate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    travelDate = "";
                    traveltoDate = "";
                }

                if (rTraveldate.ToUpper() == "YES")
                {
                    fromDate = "";
                    toDate = "";
                    travelDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    traveltoDate = DateTime.ParseExact(todate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                }

                if (rBlockPnr == "yes")
                {
                    blockdate = Base.LoadServerdatetime();
                    blockdate = DateTime.ParseExact(blockdate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    if (string.IsNullOrEmpty(blockdate))
                    {
                        result[error] = "From date and to date are mandatory";
                        return Json(new { Status = "00", Message = "", Result = result });
                    }
                    travelDate = "";
                    traveltoDate = "";
                    fromDate = "";
                    toDate = "";
                }

                string flag = "";
                string logresponse = string.Empty;

                #region

                if (getprevilage.ToString() == "S")
                    strTerminalId = string.Empty;
                else
                    strTerminalId = Session["terminalid"].ToString();

                if (getAgentType == ConsoleAgent)
                {
                    strAgentID = string.Empty;
                    strTerminalId = string.Empty;
                }

                if (terminalType == "T")
                {
                    strAgentID = string.Empty;
                    strTerminalId = string.Empty;
                    POS_ID = AgentId != "" ? AgentId : "ALL";
                }

                #endregion

                Inplntsvc.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString() : ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();

                if (Convert.ToBoolean(strMigratedAgent))
                {
                    Inplntsvc.Url = ConfigurationManager.AppSettings["TOPUP_APPS_INPLANT_SERVICE"].ToString();
                }

                string strMasterData = string.Empty;
                if (Convert.ToBoolean(strMigratedAgent))
                {
                    string strBranchAccess = Session["BRANCH_ACCESS"] != null && Session["BRANCH_ACCESS"].ToString() != "" ? Session["BRANCH_ACCESS"].ToString() : "ALL";
                    if (strBranchCredit != "" && strBranchAccess != "")
                    {
                        if (strBranchAccess == "ALL")
                        {
                            strMasterData = strBranchCredit;
                        }
                        else
                        {
                            string[] tempcredit = strBranchCredit.TrimEnd(',').Split(',');
                            string[] tempbranch = strBranchAccess.TrimEnd(',').Split(',');
                            var temparr = tempcredit.Intersect(tempbranch).ToList<string>();
                            if (temparr.Count > 0)
                            {
                                strMasterData = string.Join(",", temparr.ToArray());
                            }
                        }
                    }
                }

                string XML = "<REQUEST><TIME>" + DateTime.Now + "</TIME><URL>" + Inplntsvc.Url + "</URL><SPNR>" + sPNR + "</SPNR><AIRLINEPNR>" + airPNR + "</AIRLINEPNR><CRSPNR>" + csrPNR + "</CRSPNR><POSID>" + POS_ID + "</POSID><STATUS>" + status + "</STATUS><FROMDATE>" + fromDate
                            + "</FROMDATE><TODATE>" + toDate + "</TODATE><POSTERMINALID>" + POS_TID + "</POSTERMINALID><USERNAME>" + strUserName + "</USERNAME><PAYMENT_TYPE>" + payment_type + "</PAYMENT_TYPE><AIRLINECODE>" + airlinecode.ToUpper()
                            + "</AIRLINECODE><FIRSTNAME>" + firstname + "</FIRSTNAME><LASTNAME>" + lastname + "</LASTNAME><TRAVELDATE>" + travelDate + "</TRAVELDATE><BLOCKDATE>" + blockdate + "</BLOCKDATE><FLIGHTNO>" + Flightno + "</FLIGHTNO><TRAVELTODATE>" + traveltoDate
                            + "</TRAVELTODATE></REQUEST>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "BookedHistory", "BookedHistory~REQ", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                dsBookedHistory = Inplntsvc.FetchBrBookedHistoryB2B(sPNR, csrPNR, airPNR, POS_ID, status, fromDate, toDate, POS_TID, strUserName, payment_type, airlinecode.ToUpper(), strMasterData, firstname, lastname, travelDate, blockdate, "", "", "", "", Flightno, traveltoDate);

                string RESXML = "<RESPONSE><TIME>" + DateTime.Now + "</TIME><DATA>" + (dsBookedHistory != null && dsBookedHistory.Tables.Count > 0 ? dsBookedHistory.Tables[0].Rows.Count.ToString() : "EMPTY") + "</DATA></RESPONSE>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "BookedHistory", "BookedHistory~RES", RESXML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                if (dsBookedHistory != null && dsBookedHistory.Tables.Count > 0 && dsBookedHistory.Tables[0].Rows.Count != 0)
                {
                    Session["Airline_bookedhistorydsBookedHistory"] = dsBookedHistory;
                    logresponse = "Success";
                    var details = (from input in dsBookedHistory.Tables[0].AsEnumerable()
                                   select new
                                   {
                                       Username = input["USER_NAME"],
                                       AIRLINE_NAME = input["AIRLINE_NAME"],
                                       SPNR = input["S_PNR"].ToString() + "|" + input["TICKET_STATUS_FLAG"].ToString(),
                                       TICKET_STATUS = input["TICKET_STATUS"],
                                       CRSPNR = input["CRSPNR"],
                                       AIRPNR = input["AIRPNR"],
                                       FLIGHT_NO = input["FLIGHT_NO"],
                                       SECTOR = input["SECTOR"],
                                       DEPT_DATE = input["DEPT_DATE"],
                                       ARRIVAL_DATE = input["ARRIVAL_DATE"],
                                       PASSENGER_NAME = input["PASSENGER_NAME"],
                                       TOTAL_PRICE = input["TOTAL_PRICE"],
                                       TRIP_TYPE = input["TRIP_TYPE"],
                                       BOOKED_DATE = input["BOOKED_DATE"],
                                       PAYMENTMODE = input["PAYMENTMODE"],
                                   });
                    DataTable datatable = Serv.ConvertToDataTable(details);

                    result[responce] = JsonConvert.SerializeObject(datatable);

                    #region Log
                    StringWriter strres = new StringWriter();
                    if (Base.ReqLog)
                    {
                        if (dsBookedHistory != null)
                            dsBookedHistory.WriteXml(strres);
                    }
                    #endregion
                }
                else
                {
                    logresponse = "Failure";
                    result[error] = "No Records(s) Found.";

                    string LstrDetails = "<REQ><FROMDATE>" + fromdate + "</FROMDATE>" +
                  "<TODATE>" + todate + "</TODATE><TODATE>" + todate + "</TODATE><REQ>" +
                 "<RESPONSE>" + "No Records(s) Found." + "</RESPONSE>";
                    DatabaseLog.LogData(Session["username"].ToString(), "T", "AIR Booked History", "AIR Booked History", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
            }
            catch (Exception ex)
            {
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "AIR Booked History", "AIR Booked History", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            var temp = Json(new
            {
                Status = "",
                Message = "",
                Result = result
            });
            temp.MaxJsonLength = int.MaxValue;
            return temp;
        }


        #endregion

        #region viewpnr
        public ActionResult RequestViewPNRFunction(string strSPNRNO, string strAirPNRNO, string strCRSPNRNO, string strPaymentmode)
        {
            #region UsageLog
            string PageName = "PNR Verification";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
            }
            catch (Exception e)
            {

            }
            #endregion
            
            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string strBranchId = string.Empty;
            string Ipaddress = string.Empty;
            string getprevilage = string.Empty;
            string getAgentType = string.Empty;
            string sequnceID = string.Empty;

            RaysService _RaysService = new RaysService();
            ArrayList arrayViewPNR = new ArrayList();
            ArrayList arrayaddPNR = new ArrayList();
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            int Error = 0;
            int Response = 1;
            int Pnr_details = 3;
            int popup_pnedetails = 4;
            string PayMode = string.Empty;
            StringBuilder strBuilderPNR = new StringBuilder();
            DataSet dsDisplayDet = new DataSet();
            //int Result = 2;
            int adultcount = 0;
            int childcount = 0;
            int infantcount = 0;
            string triptype = string.Empty;
            string cabinval = string.Empty;
            string Pnrdetails = string.Empty;

            strTerminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString().ToUpper().Trim() : "";
            strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            terminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();// "W";
            string POS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string POS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"] != "") ? Session["TKTFLAG"].ToString() : "";
            string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");

            try
            {
                if (strTerminalId == "" || strUserName == "" || strAgentID == "" || Ipaddress == "" || sequnceID == "" || terminalType == "" || POS_ID == "" || POS_TID == "" || strTKTFLAG == "")
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }

                strBranchId = Session["branchid"].ToString();
                getprevilage = Session["privilage"].ToString();
                getAgentType = Session["agenttype"].ToString();



                StringBuilder strpassdetails = new StringBuilder();
                DataSet dsViewPNR = new DataSet();
                DataSet dsFareDispDet = new DataSet();
                DataSet dsPassDetails = new DataSet();

                byte[] dsViewPNR_compress = new byte[] { };
                byte[] dsFareDispDet_compress = new byte[] { };

                string refError = "ERROR";
                string strErrorMsg = "";
                bool result = false;
                DataSet dsContactDetails = new DataSet();

                #region
                string TerminalApp = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();

                if (getprevilage.ToString() == "S")
                    strTerminalId = string.Empty;
                else
                    strTerminalId = Session["POS_TID"].ToString();

                if ("T" == TerminalApp)
                {
                    strAgentID = string.Empty;
                }

                strSPNRNO = strSPNRNO.Trim();
                strAirPNRNO = strAirPNRNO.Trim();
                strCRSPNRNO = strCRSPNRNO.Trim();



                #endregion

                _RaysService.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                result = _RaysService.Fetch_PNR_Verification_Details_NewByte(strAgentID, strSPNRNO, strAirPNRNO, strCRSPNRNO, "", strTerminalId, strUserName, Ipaddress, "W", Convert.ToDecimal(sequnceID), ref dsViewPNR_compress, ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "Fetchviewpnr", "Fetchviewpnrdetails", getAgentType, strTerminalId);

                if (strProduct == "RBOA" && result == false && TerminalApp == "T")
                {
                    _RaysService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                    result = _RaysService.Fetch_PNR_Verification_Details_NewByte(strAgentID, strSPNRNO, strAirPNRNO, strCRSPNRNO, "", strTerminalId, strUserName, Ipaddress, "W", Convert.ToDecimal(sequnceID), ref dsViewPNR_compress, ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "Fetchviewpnr", "Fetchviewpnrdetails", getAgentType, strTerminalId);
                }


                dsViewPNR = Base.Decompress(dsViewPNR_compress);
                dsFareDispDet = Base.Decompress(dsFareDispDet_compress);

                if (result == false)
                {
                    if (dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                    {
                        string record_view = JsonConvert.SerializeObject(dsViewPNR);
                        arrayViewPNR[popup_pnedetails] = record_view;
                    }
                    else
                    {
                        string display = "Please enter valid PNR No.";
                        arrayViewPNR[Error] = display.ToString();
                    }
                }
                else
                {
                    if (dsViewPNR != null && dsViewPNR.Tables.Count > 0)
                    {
                        adultcount = Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["AdultCount"].ToString());
                        childcount = Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["ChildCount"].ToString());
                        infantcount = Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["InfantCount"].ToString());
                        triptype = dsViewPNR.Tables[0].Rows[0]["TRIP_DESC"].ToString();
                        cabinval = (dsViewPNR.Tables[0].Rows[0]["CLASS_NAME"].ToString().ToUpper() == "ECONOMY" ? "E" : (dsViewPNR.Tables[0].Rows[0]["CLASS_NAME"].ToString().ToUpper() == "BUSINESS" ? "B" : (dsViewPNR.Tables[0].Rows[0]["CLASS_NAME"].ToString().ToUpper() == "PREMIUM ECONOMY" ? "P" : "F")));

                        string sprr_s = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["S_PNR"].ToString();
                        string crs_s = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["CRS_PNR"].ToString();
                        string air_s = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["AIRLINE_PNR"].ToString();
                        string tripr_s = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TRIP_DESC"].ToString();
                        string strClientBranchID = string.Empty;
                        strClientBranchID = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("TAI_ISSUING_BRANCH_ID") ? dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TAI_ISSUING_BRANCH_ID"].ToString() : "";
                        Session.Add("ClientBranchID", strClientBranchID);
                        Pnrdetails = sprr_s + "~" + air_s + "~" + crs_s + "~" + triptype + "~" + cabinval;

                        arrayViewPNR[Pnr_details] = sprr_s + "|" + crs_s + "|" + air_s + "|" + tripr_s;

                    }

                    if (dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0 && dsFareDispDet != null && dsFareDispDet.Tables.Count > 0 && dsFareDispDet.Tables[0].Rows.Count > 0)
                    {
                        if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("PAYMENT_MODE_DESCRIPTION") && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PAYMENT_MODE_DESCRIPTION"] != null
                                                    && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PAYMENT_MODE_DESCRIPTION"].ToString() != "")
                        {
                            PayMode = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PAYMENT_MODE_DESCRIPTION"].ToString();
                        }
                        else if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PAYMENT_MODE"].ToString() == "T" || dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PAYMENT_MODE"].ToString() == "Topup")
                        {
                            PayMode = "Agent Topup Account";
                        }
                        else if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PAYMENT_MODE"].ToString() == "C" || dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PAYMENT_MODE"].ToString().ToUpper() == "CREDIT")
                        {
                            PayMode = "Agent Credit Account";
                        }
                        else if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PAYMENT_MODE"].ToString() == "P")
                        {
                            PayMode = "Payment Gateway";
                        }
                        else
                        {
                            PayMode = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PAYMENT_MODE"].ToString();
                        }

                        ViewBag.Emailid = ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("EMAIL_ID") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["EMAIL_ID"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["EMAIL_ID"].ToString() : "");



                        #region for theme3
                        if (ConfigurationManager.AppSettings["apptheme"].ToString().ToUpper().Trim() == "THEME3")
                        {
                            strBuilderPNR.Append("<div class='Viewpnr col-xs-12 col-md-12 col-sm-12 col-lg-12 form-group col0' ><label style='margin-bottom:10px;margin-top:10px;width: 100%;text-decoration:underline;text-align:left;cursor:pointer;margin-top:0px !important;font-weight: 500;' class='cls-header' onclick='morepnrdetail()'><i class='fa fa-file-text' style='display:none;'></i>More Details<label class='fa fa-angle-double-down faa-bounce animated' style='margin-top:0px !important;margin-bottom:0px !important;'></label><span style='text-align: right;float: right;color: red;'>All Fares in " + (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("Currency") ? (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["Currency"].ToString() != "" ? dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["Currency"].ToString() : ConfigurationManager.AppSettings["Currency"].ToString()) : ConfigurationManager.AppSettings["Currency"].ToString()) + "</span></label><div class='col-xs-12 col-md-12 col-sm-12 col-lg-12 Viewaddress mrepnrdetail' style='display:none'>");
                            strBuilderPNR.Append("<div class='row'>");
                            strBuilderPNR.Append("<div class='col-md-4 o-hidden'>");
                            strBuilderPNR.Append("<table style='text-align: left;' border='0' class='Viewleft table table-responsive vpnrtbl'>");
                            strBuilderPNR.Append("<tr><th class='subbtitle' colspan='3' style='border-top-style: hidden;color:black;background-color: #ccc;'> Contact Details </th></tr>");
                            strBuilderPNR.Append("<tr><td>Contact Name</td><td>:</td><td>" + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("NAME") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["NAME"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["NAME"].ToString().TrimStart('.') : "N/A") + "</td></tr>");
                            strBuilderPNR.Append("<tr><td>Contact No.</td><td>:</td><td>" + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("MOBILE_NO") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["MOBILE_NO"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["MOBILE_NO"].ToString() : "N/A") + "</td></tr>");//((dsContactDetails.Tables[0].Rows[0]["MOBILE_NO"].ToString().Trim() != "") ? dsContactDetails.Tables[0].Rows[0]["MOBILE_NO"].ToString() : "N/A") + "</td></tr>");

                            strBuilderPNR.Append("<tr><td>Agency Name</td><td>:</td><td>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["AGENCY_NAME"] + "</td></tr>");
                            strBuilderPNR.Append("<tr><td>Email ID</td><td>:</td><td>" + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("EMAIL_ID") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["EMAIL_ID"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["EMAIL_ID"].ToString() : "N/A") + "</td></tr>");


                            strBuilderPNR.Append("<tr><td>Agency No.</td><td>:</td><td>" + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("AGENT NO") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["AGENT NO"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["AGENT NO"].ToString() : "N/A") + "</td></tr>");
                            strBuilderPNR.Append("<tr><td>Business No.</td><td>:</td><td> " + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("BUSINESS NO") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["BUSINESS NO"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["BUSINESS NO"].ToString() : "N/A") + "</td></tr>");
                            strBuilderPNR.Append("<tr><td>Home No.</td><td>:</td><td>" + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("HOME NO") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["HOME NO"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["HOME NO"].ToString() : "N/A") + "</td></tr>");
                            strBuilderPNR.Append("</table>");
                            strBuilderPNR.Append("</div>");


                            strBuilderPNR.Append("<div class='col-md-4 o-hidden'>");
                            strBuilderPNR.Append("<table class='Viewleft table table-responsive' id='viewtable1'>");
                            strBuilderPNR.Append("<tr><th class='subbtitle' colspan='3' style='border-top-style: hidden;color:black;background-color: #ccc;'> Booked Details </th></tr>");
                            strBuilderPNR.Append("<tr><td>Booked&nbsp;Agent</td><td>:</td><td>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["AGENT_ID"] + "</td></tr>");
                            strBuilderPNR.Append("<tr><td>Operator&nbsp;Name</td><td>:</td><td>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PLATING_CARRIER"] + "</td></tr>");
                            strBuilderPNR.Append("<tr><td>Payment Mode</td><td>:</td><td>" + PayMode + "</td></tr>");
                            strBuilderPNR.Append("<tr><td>Booked&nbsp;Date</td><td>:</td><td>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["BOOKED_DATE"] + "</td></tr>");
                            strBuilderPNR.Append("</table>");

                            strBuilderPNR.Append("<table class='Viewleft table table-responsive' id='viewtable2' style='display:none'>");
                            strBuilderPNR.Append("<tr><th class='subbtitle' colspan='3' style='border-top-style: hidden;color:black;background-color: #ccc;'> Booked Details </th></tr>");
                            strBuilderPNR.Append("<tr style='background-color: #2cc185; color: white;'><td>Booked&nbsp;Agent</td><td>:</td><td>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["AGENT_ID"] + "</td></tr>");
                            strBuilderPNR.Append("<tr style='background-color: #2cc185; color: white;'><td>Operator&nbsp;Name</td><td>:</td><td>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PLATING_CARRIER"] + "</td></tr>");
                            strBuilderPNR.Append("<tr style='background-color: #2cc185; color: white;'><td>Payment Mode</td><td>:</td><td>" + PayMode + "</td></tr>");
                            strBuilderPNR.Append("<tr style='background-color: #2cc185; color: white;'><td>Booked&nbsp;Date</td><td>:</td><td>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["BOOKED_DATE"] + "</td></tr>");
                            strBuilderPNR.Append("</table>");
                            strBuilderPNR.Append("</div>");


                            strBuilderPNR.Append("<div class='col-xs-12 col-md-4 col-sm-12 col-lg-4  o-hidden'>");
                            strBuilderPNR.Append("<table style='text-align: left;' border='0' class='Viewright table table-responsive vpnrtb2'><tr>");

                            strBuilderPNR.Append("<tr><th class='subbtitle' colspan='3' style='border-top-style: hidden;color:black;background-color: #ccc;' > Fare Details </th></tr>");
                            strBuilderPNR.Append("<tr><td class='leftaln'>Total&nbsp;Basic</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td><td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["BASICFAREAMOUNT"] + "</td></tr>");

                            strBuilderPNR.Append("<tr><td class='leftaln'>Total&nbsp;Tax</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td><td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["TAXAMOUNTCLT"] + "</td></tr>");

                            strBuilderPNR.Append("<tr><td class='leftaln'>Service&nbsp;charge(MF)</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                            strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["SERVICECHARGE"] + "</td></tr>");

                            if (dsFareDispDet.Tables[0].Columns.Contains("PCI_SF") && Convert.ToDouble(dsFareDispDet.Tables[0].Rows[0]["PCI_SF"]) != 0)
                            {
                                strBuilderPNR.Append("<tr><td class='leftaln'>Service&nbsp;charge(SF)</td>");
                                strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                                strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["PCI_SF"] + "</td></tr>");
                            }
                            if (dsFareDispDet.Tables[0].Columns.Contains("PCI_SF_GST") && Convert.ToDouble(dsFareDispDet.Tables[0].Rows[0]["PCI_SF_GST"]) != 0)
                            {
                                strBuilderPNR.Append("<tr><td class='leftaln'>GST&nbsp;on&nbsp;SF</td>");
                                strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                                strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["PCI_SF_GST"] + "</td></tr>");
                            }
                            if (dsFareDispDet.Tables[0].Columns.Contains("Meals_Amt") && Convert.ToDouble(dsFareDispDet.Tables[0].Rows[0]["Meals_Amt"]) != 0)
                            {
                                strBuilderPNR.Append("<tr><td class='leftaln'>Meals&nbsp;Amount</td>");
                                strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                                strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["Meals_Amt"] + "</td></tr>");
                            }
                            if (dsFareDispDet.Tables[0].Columns.Contains("Baggage_Amt") && Convert.ToDouble(dsFareDispDet.Tables[0].Rows[0]["Baggage_Amt"]) != 0)
                            {
                                strBuilderPNR.Append("<tr><td class='leftaln'>Baggage&nbsp;Amount</td>");
                                strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                                strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["Baggage_Amt"] + "</td></tr>");
                            }
                            if (dsFareDispDet.Tables[0].Columns.Contains("Seat_Amt") && Convert.ToDouble(dsFareDispDet.Tables[0].Rows[0]["Seat_Amt"]) != 0)
                            {
                                strBuilderPNR.Append("<tr><td class='leftaln'>Seat&nbsp;Amount</td>");
                                strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                                strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["Seat_Amt"] + "</td></tr>");
                            }
                            strBuilderPNR.Append("<tr><td class='leftaln'>Commission</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td><td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["Discount"] + "</td></tr>");

                            strBuilderPNR.Append("<tr><td class='leftaln'>Total&nbsp;TDS</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td><td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["TDS_AMOUNT"] + "</td></tr>");

                            strBuilderPNR.Append("<tr><td class='leftaln'>Net&nbsp;Amount</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td><td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["NETAMOUNTCLT"] + "</td></tr>");

                            strBuilderPNR.Append("<tr class='bgcolor_red'><td class='leftaln'><b style='font-weight: 900;color: #eeeeee;'>Total&nbsp;Gross </b></td><td style='text-align:center;font-weight: 900;color: #eeeeee;'>:</td><td style='text-align:right'><b style='font-weight: 900;color: #eeeeee;'>" + dsFareDispDet.Tables[0].Rows[0]["GROSSAMOUNTCLT"] + "</b></td></tr>");
                            strBuilderPNR.Append("</table></div></div>");

                            strBuilderPNR.Append("</div></div>");

                            strBuilderPNR.Append("<div border='0'  class='form-group Viewtotal col-xs-12 col-md-12 col-sm-12 col-lg-12 ' style='padding-top: 6px;padding-bottom: 6px;display:none'>");

                            strBuilderPNR.Append("<div class='Viewtotal col-xs-12 col-sm-6 col-md-3'>");
                            strBuilderPNR.Append("<div class='row'>");
                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc1'>");
                            strBuilderPNR.Append("<span>Booked&nbsp;Agent</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc2'>");
                            strBuilderPNR.Append("<span>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["AGENT_ID"] + "</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("</div>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='Viewtotal col-xs-12 col-sm-6 col-md-3'>");
                            strBuilderPNR.Append("<div class='row'>");
                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc1'>");
                            strBuilderPNR.Append("<span>Operator&nbsp;Name</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc2'>");
                            strBuilderPNR.Append("<span>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PLATING_CARRIER"] + "</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("</div>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='Viewtotal col-xs-12 col-sm-6 col-md-3'>");
                            strBuilderPNR.Append("<div class='row'>");
                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc1'>");
                            strBuilderPNR.Append("<span>Payment Mode</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc2'>");
                            strBuilderPNR.Append("<span>" + PayMode + "</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("</div>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='Viewtotal col-xs-12 col-sm-6 col-md-3'>");
                            strBuilderPNR.Append("<div class='row'>");
                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc1'>");
                            strBuilderPNR.Append("<span>Booked&nbsp;Date</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc2'>");
                            strBuilderPNR.Append("<span>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["BOOKED_DATE"] + "</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("</div>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("</div><br />");
                            strBuilderPNR.Append("</tr></div><br />");

                            strBuilderPNR.Append("</div>");
                            if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows.Count > 0)
                            {
                                strBuilderPNR.Append("<div class='TxBandNew' style='text-align: left;'> <label style='margin-bottom:10px;margin-top:10px;width: 100%;font-weight:500 ' class='cls-header '><i class='fa fa-users' style='display:none;'></i>Passenger Details<span style='text-align: right;float: right;color: #3cab37;font-weight: bold;'> " + (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TCK_SPECIAL_TRIP"]) + "</span></label></div> ");
                                strBuilderPNR.Append("<div class='_Muaepass' style='overflow:auto;'> ");
                                var PNRREFNO = (from input in dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].AsEnumerable()
                                                select new
                                                {
                                                    PAX_REF_NO = input["PAX_REF_NO"].ToString(),
                                                    name = input["PASSENGER_NAME"].ToString(),
                                                    Type = input["PASSENGER_TYPE"].ToString()
                                                }).Distinct();
                                DataTable dtpaxcount = Serv.ConvertToDataTable(PNRREFNO);
                                #region

                                strBuilderPNR.Append("<table  class='pnrviewtab table table-hover table-striped no-more-tables' data-click-to-select='true' id='viewpnrdetailstable' >");

                                strBuilderPNR.Append("<thead><tr>");
                                strBuilderPNR.Append("<th data-field='direct1' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Passenger Name</label></th>");
                                strBuilderPNR.Append("<th data-field='direct2' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Ticket&nbsp;No.</label></th>");
                                strBuilderPNR.Append("<th data-field='direct3' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Flight&nbsp;No.</label></th>");
                                strBuilderPNR.Append("<th data-field='direct4' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Origin</label></th>");
                                strBuilderPNR.Append("<th data-field='direct5' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Destination</label></th>");

                                strBuilderPNR.Append("<th data-field='direct6' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Departure</label></th>");
                                strBuilderPNR.Append("<th data-field='direct7' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Arrival</label></th>");
                                strBuilderPNR.Append("<th data-field='direct8' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Class</label></th>");
                                strBuilderPNR.Append("<th data-field='direct9' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>FareRule</label></th>");
                                strBuilderPNR.Append("<th data-field='direct10' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Status</label></th>");

                                strBuilderPNR.Append("<th data-field='direct11' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Basic&nbsp;Fare</label></th>");
                                strBuilderPNR.Append("<th data-field='direct12' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>YQ&nbsp;Tax</label></th>");
                                strBuilderPNR.Append("<th data-field='direct13' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Tax&nbsp;&&nbsp;Others</label></th>");

                                strBuilderPNR.Append("<th data-field='direct14' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>GrossFare</label></th>");
                                if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("TUNEURL") && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TUNEURL"] != null && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TUNEURL"].ToString() != "")
                                {
                                    strBuilderPNR.Append("<th data-field='direct15' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>View&nbsp;Policy</label></th>");
                                }
                                strBuilderPNR.Append("</tr></thead><tbody>");

                                for (int i = 1; i <= dtpaxcount.Rows.Count; i++)
                                {

                                    var qryAirlineMarge = (from input in dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].AsEnumerable()
                                                           where (input["PAX_REF_NO"].ToString()).Trim() ==
                                                           dtpaxcount.Rows[i - 1]["PAX_REF_NO"].ToString()
                                                           select new
                                                           {
                                                               PASSENGER_NAME = input["PASSENGER_NAME"].ToString(),
                                                               Ticket_No = input["TICKET_NO"].ToString(),
                                                               Flight_No = input["FLIGHT_NO"].ToString(),
                                                               ORIGIN = input["ORIGIN"].ToString(),
                                                               DESTINATION = input["DESTINATION"].ToString(),
                                                               DEPT_DATE = Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["DEPT_DATE"].ToString()), "dd/MM/yyyy HH:mm"),
                                                               CLASS_ID = input["CLASS_ID"].ToString(),
                                                               BASICFAREAMOUNT = input["BASICFARE"].ToString(),
                                                               GROSSFARE = input["GROSSFARE"].ToString(),
                                                               Status = input["TICKET_STATUS"].ToString(),
                                                               Arrival = Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["ARRIVAL_DATE"].ToString()), "dd/MM/yyyy HH:mm"),
                                                               Yq_tax = input["PCI_YQ_FARE"].ToString(),
                                                               Tax_AMT = input["TAXAMTP"].ToString(),
                                                               FARE_ID = input["FARE_ID"].ToString(),
                                                               transfee = (Base.ServiceUtility.ConvertToDecimal(input["TRANSACTION_FEE"].ToString()) + Base.ServiceUtility.ConvertToDecimal(input["SERVICECHARGE"].ToString())).ToString(),
                                                               AirlineCategory = input["AIRCATEGORY"].ToString() + "SpLitPResna" + input["ORIGINCODE"].ToString() + "SpLitPResna" + input["DESTINATIONCODE"].ToString() + "SpLitPResna" + Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["DEPT_DATE"].ToString()), "dd MMM yyyy HH:mm") + "SpLitPResna" + input["Token"].ToString() + "SpLitPResna" + input["PLATING_CARRIER"].ToString() + "SpLitPResna" + Regex.Split(input["StockType"].ToString(), "SPLIT")[0],
                                                               inva = input["ORIGINCODE"].ToString() + "SpLitPResna" + input["DESTINATIONCODE"].ToString() + "SpLitPResna" + Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["DEPT_DATE"].ToString()), "dd MMM yyyy HH:mm") + "SpLitPResna" + Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["ARRIVAL_DATE"].ToString()), "dd MMM yyyy HH:mm") + "SpLitPResna" + "fourempty" + "SpLitPResna" + input["CLASS_ID"].ToString() + "SpLitPResna" + "sixempty" + "SpLitPResna" + "sevenempty" + "SpLitPResna" + "eightempty" + "SpLitPResna" + input["PLATING_CARRIER"].ToString() + "SpLitPResna" + "tenempty" + "SpLitPResna" + "elevempty"
                                                               + "SpLitPResna" + input["Token"].ToString() + "SpLitPResna" + "thirtempty" + "SpLitPResna" + "fourtenempty" + "SpLitPResna" + "fivtenempty" + "SpLitPResna" + input["AIRCATEGORY"].ToString() + "SpLitPResna" + input["FARE_BASIS"].ToString() + "SpLitPResna" + input["FLIGHT_NO"].ToString() + "SpLitPResna" + "0" + "SpLitPResna" + "twentyempty" + "SpLitPResna" + "twntyonenempty" + "SpLitPResna" + "22nempty" + "SpLitPResna" + Regex.Split(input["StockType"].ToString(), "SPLIT")[0]
                                                               + "SpLitPResna" + "twentfourempty" + "SpLitPResna" + input["FARE_ID"].ToString() + "SpLitPResna" + "twentsixempty" + "SpLitPResna" + "twentyseven" + "SpLitPResna" + "twentyeight" + "SpLitPResna" + input["SEGMENT_NO"].ToString() + "SpLitPResna" + "thirty" + "SpLitPResna" + "thirtyone" + "SpLitPResna" + "thirtytwo" + "SpLitPResna" + "thirtythree" + "SpLitPResna" + input["SPECIAL_TRIP"].ToString() + "SpLitPResna" + input["SPECIAL_TRIP"].ToString() + "SpLitPResna" + "thirtysix",
                                                               AIRPORT_ID = input["AIRPORT_ID"].ToString()
                                                           });

                                    if (qryAirlineMarge.Count() > 0)
                                    {

                                        DataTable dtsegcount = Serv.ConvertToDataTable(qryAirlineMarge);
                                        int rows = dtsegcount.Rows.Count;

                                        var Fare = (from fare in dtsegcount.AsEnumerable()
                                                    select fare["FARE_ID"].ToString()).Distinct();

                                        if (Fare.Count() == 1)
                                        {
                                            decimal Tax_fare = 0;
                                            decimal Grossamount = 0;
                                            decimal Tax_consolidate = 0;

                                            for (int tax = 0; tax < dtsegcount.Rows.Count; tax++)
                                            {
                                                if (tax > 0)
                                                {
                                                    Tax_consolidate = Tax_consolidate + Base.ServiceUtility.ConvertToDecimal(dtsegcount.Rows[tax]["Tax_AMT"].ToString());
                                                }
                                                Tax_fare = Tax_fare + Base.ServiceUtility.ConvertToDecimal(dtsegcount.Rows[tax]["Tax_AMT"].ToString());
                                            }

                                            for (int j = 0; j < dtsegcount.Rows.Count; j++)
                                            {

                                                strBuilderPNR.Append("<tr>");
                                                strBuilderPNR.Append("<td data-title='Passenger Name'><B>" + dtsegcount.Rows[j]["PASSENGER_NAME"] + "</B></td>");
                                                strBuilderPNR.Append("<td data-title='Ticket No.'><B>" + dtsegcount.Rows[j]["Ticket_No"] + "</B></td>");
                                                strBuilderPNR.Append("<td data-title='Flight No.'>" + dtsegcount.Rows[j]["Flight_No"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Origin'>" + dtsegcount.Rows[j]["ORIGIN"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Destination'>" + dtsegcount.Rows[j]["DESTINATION"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Departure'>" + dtsegcount.Rows[j]["DEPT_DATE"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Arrival'>" + dtsegcount.Rows[j]["Arrival"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Class'>" + dtsegcount.Rows[j]["CLASS_ID"] + "</td>");
                                                strBuilderPNR.Append("<td class='' data-title='FareRule'><a href='#'><label style='cursor: pointer; color: blue; border-bottom: 1px solid;' id='Fare_rule" + j + "' data-airlinecatagory=" + JsonConvert.SerializeObject(dtsegcount.Rows[j]["AirlineCategory"]) + " data-flightid=" + dtsegcount.Rows[j]["Flight_No"] + "  data-inva=" + JsonConvert.SerializeObject(dtsegcount.Rows[j]["inva"]) + " data-paxcount=" + adultcount + '|' + childcount + '|' + infantcount + " data-pnrdetails=" + Pnrdetails + " data-AirportId=" + dtsegcount.Rows[j]["AIRPORT_ID"] + " onclick='javascript:GetFareRule(this," + j + ");'>Farerule</label></a></td>");
                                                strBuilderPNR.Append("<td class='' data-title='Status'>" + dtsegcount.Rows[j]["Status"] + "</td>");

                                                if (j == 0)
                                                {
                                                    string str_rcont = (dtsegcount.Rows.Count > 1 && ConfigurationManager.AppSettings["apptheme"].ToString() == "THEME1") ? "farecls" : "";
                                                    strBuilderPNR.Append("<td data-title='Basic&nbsp;Fare' rowspan='" + dtsegcount.Rows.Count + "' style='text-align: right;padding-right: 5px !important;' class='" + str_rcont + "'>" + dtsegcount.Rows[j]["BASICFAREAMOUNT"] + "</td>");
                                                    strBuilderPNR.Append("<td data-title='YQ&nbsp;Tax' rowspan='" + dtsegcount.Rows.Count + "' style='text-align: right;padding-right: 5px !important;' class='" + str_rcont + "'>" + dtsegcount.Rows[j]["Yq_tax"] + "</td>");
                                                    strBuilderPNR.Append("<td data-title='Tax&nbsp;&&nbsp;Amount+Others' rowspan='" + dtsegcount.Rows.Count + "'  style='text-align: right;padding-right: 5px !important;' class='" + str_rcont + "' >" + Tax_fare + "</td>");
                                                    decimal valgross = Base.ServiceUtility.ConvertToDecimal(dtsegcount.Rows[j]["GROSSFARE"].ToString());
                                                    Grossamount = Base.ServiceUtility.ConvertToDecimal(Tax_consolidate.ToString()) + Base.ServiceUtility.ConvertToDecimal(valgross.ToString());
                                                    //Grossamount = Convert.ToInt32(dtsegcount.Rows[j]["GROSSFARE"]);
                                                    strBuilderPNR.Append("<td data-title='GrossFare' style='text-align: right;padding-right: 5px !important;' rowspan='" + dtsegcount.Rows.Count + "' class='" + str_rcont + "'><B>" + valgross + "</B></td>");
                                                }
                                                if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("TUNEURL") && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TUNEURL"] != null && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TUNEURL"].ToString() != "")
                                                {
                                                    //strBuilderPNR.Append("<th data-field='direct15" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Policy&nbsp;Link</label></th>");
                                                    strBuilderPNR.Append("<td data-title='Class'><a style='color:#27a5f5' href=" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TUNEURL"] + ">View&nbsp;Policy</a></td>");
                                                    // strBuilderPNR.Append("</div>");
                                                }
                                                strBuilderPNR.Append("<input type='hidden' id='AirlineCategory" + j + "' value='" + dtsegcount.Rows[j]["AirlineCategory"] + "' />");
                                                strBuilderPNR.Append("<input type='hidden' id='FlightID" + j + "'  value='" + dtsegcount.Rows[j]["Flight_No"] + "' /> ");
                                                strBuilderPNR.Append("<input type='hidden' id='inva" + j + "' value='" + dtsegcount.Rows[j]["inva"] + "' />  </td>");

                                                strBuilderPNR.Append("</tr>");
                                            }
                                        }
                                        else
                                        {
                                            string lstrtempfareid = "";
                                            for (int j = 0; j < dtsegcount.Rows.Count; j++)
                                            {

                                                strBuilderPNR.Append("<tr>");
                                                strBuilderPNR.Append("<td data-title='Passenger Name'><B>" + dtsegcount.Rows[j]["PASSENGER_NAME"] + "</B></td>");
                                                strBuilderPNR.Append("<td data-title='Ticket No.'><B>" + dtsegcount.Rows[j]["Ticket_No"] + "</B></td>");
                                                strBuilderPNR.Append("<td data-title='Flight No.'>" + dtsegcount.Rows[j]["Flight_No"] + "</td>");
                                                strBuilderPNR.Append("<td  data-title='Origin'>" + dtsegcount.Rows[j]["ORIGIN"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Destination'>" + dtsegcount.Rows[j]["DESTINATION"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Departure'>" + dtsegcount.Rows[j]["DEPT_DATE"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Arrival'>" + dtsegcount.Rows[j]["Arrival"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Class'>" + dtsegcount.Rows[j]["CLASS_ID"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='FareRule' class='label label-success spnEarn'><a href='#'><label style='cursor: pointer; color: blue; border-bottom: 1px solid;' id='Fare_rule" + j + "' data-airlinecatagory=" + JsonConvert.SerializeObject(dtsegcount.Rows[j]["AirlineCategory"]) + " data-flightid=" + dtsegcount.Rows[j]["Flight_No"] + "  data-inva=" + JsonConvert.SerializeObject(dtsegcount.Rows[j]["inva"]) + "  data-paxcount=" + adultcount + '|' + childcount + '|' + infantcount + " data-pnrdetails=" + Pnrdetails + " data-AirportId=" + dtsegcount.Rows[j]["AIRPORT_ID"] + " onclick='javascript:GetFareRule(this," + j + ");'>Farerule</label></a></td>");

                                                //strBuilderPNR.Append("<td ><a href='#'><label id='Fare_rule" + j + "'  onclick='javascript:GetFareRule(" + j + ");'>Farerule</label></a>   </td>");
                                                strBuilderPNR.Append("<td data-title='Status'><B>" + dtsegcount.Rows[j]["Status"] + "</B></td>");

                                                if (string.IsNullOrEmpty(lstrtempfareid) || lstrtempfareid != dtsegcount.Rows[j]["FARE_ID"].ToString().Trim())
                                                {
                                                    strBuilderPNR.Append("<td data-title='Basic&nbsp;Fare' rowspan='" + (dtsegcount.Rows.Count / 2) + "'>" +
                                                        dtsegcount.Rows[j]["BASICFAREAMOUNT"] + "</td>");
                                                    strBuilderPNR.Append("<td data-title='YQ&nbsp;Tax' rowspan='" + (dtsegcount.Rows.Count / 2) + "'>" +
                                                        dtsegcount.Rows[j]["Yq_tax"] + "</td>");
                                                    strBuilderPNR.Append("<td data-title='Tax&Others' rowspan='" + (dtsegcount.Rows.Count / 2) + "'>" +
                                                        dtsegcount.Rows[j]["Tax_AMT"] + "</td>");
                                                    strBuilderPNR.Append("<td data-title='GrossFare' rowspan='" + (dtsegcount.Rows.Count / 2) + "'><B>" +
                                                        dtsegcount.Rows[j]["GROSSFARE"] + "</B>");
                                                    lstrtempfareid = dtsegcount.Rows[j]["FARE_ID"].ToString().Trim();
                                                }
                                                strBuilderPNR.Append("<input type='hidden'  id='AirlineCategory" + j + "'   value='" + dtsegcount.Rows[j]["AirlineCategory"] + "' />");
                                                strBuilderPNR.Append("<input type='hidden' id='FlightID" + j + "' value='" + dtsegcount.Rows[j]["Flight_No"] + "' /> ");
                                                strBuilderPNR.Append("<input type='hidden' id='inva" + j + "'  value='" + dtsegcount.Rows[j]["inva"] + "' /></td>");
                                                strBuilderPNR.Append("</tr>");
                                            }
                                        }
                                    }


                                }
                                strBuilderPNR.Append("</tbody></table><br />");
                                #endregion
                                #region New Additional Services Details
                                if (ConfigurationManager.AppSettings["ADDONS_VIEWPNR"] != null && ConfigurationManager.AppSettings["ADDONS_VIEWPNR"].ToString() == "Y" && Convert.ToDateTime(dsViewPNR.Tables[0].Rows[0]["DEPT_DATE"].ToString()) > DateTime.Now && dsViewPNR.Tables[0].Rows[0]["TICKET_STATUS"].ToString().ToUpper() == "CONFIRMED")
                                {
                                    strBuilderPNR.Append("<div class=\"TxBandNew\" style=\"text-align: left;\"><label style=\"margin-bottom:10px;margin-top:10px;width: 100%;font-weight:500\" class=\"cls-header\">Additional Service</label></div>");
                                    strBuilderPNR.Append("<div class='col-xs-12' id='dvAdditionalServ'>");
                                    strBuilderPNR.Append("<a href='" + Url.Action("OtherProduct", "OtherProduct") + "?Flag=ANCV'>");
                                    strBuilderPNR.Append("<div class=\"clsSSRAddons ssr-sec\" style=\"margin: 10px !important;padding: 10px;\">");
                                    strBuilderPNR.Append("<div class=\"service_icon_o\"><img src=\"/Images/"+ strProduct + "/Other.png\"></div>");
                                    strBuilderPNR.Append("<br /><span>Ancillary Services</span>");
                                    strBuilderPNR.Append("</div>");
                                    strBuilderPNR.Append("</a>");
                                    strBuilderPNR.Append("</div>");
                                    Session.Add("PNRFLAG_ANC", strSPNRNO);
                                }
                                #endregion

                                strBuilderPNR.Append("<div class='col-xs-12 col-md-6 col-sm-8 col-lg-6 col-lg-offset-3 col-md-offset-3 col-sm-offset-2' style='text-align:center;margin-top: 2%;' >");
                                if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() == "H" && Session["ticketing"].ToString().Trim() == "Y")
                                {
                                    strBuilderPNR.Append("<div class=' col-xs-6 col-md-6 col-sm-4 col-lg-6'><input type='button' class='action-button shadow animate color' value='Issue Ticket' style='width:100%'  onclick='return toticket();'/></div>");
                                    strBuilderPNR.Append("<div class=' col-xs-6 col-md-6 col-sm-4 col-lg-6'><input type='button' class='action-button action-buttonB shadow animate  colorB1' value='Cancel Ticket' style='width:100%'  onclick='return cancelticket();'/></div>");
                                }
                                if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() != "H" && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() != "B" && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() != "X" && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() != "I" && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() != "W")
                                    if (dtpaxcount.Rows.Count > 1)
                                    {
                                        strBuilderPNR.Append("<div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_fare'  class='cb' name='Chk_fare' /><label class='cbx' for='Chk_fare'></label><label class='lbl' for='Chk_fare'>With Fare</label></div>"
                                                            + "<div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_logo' checked=''  class='cb' name='Chk_logo' /><label class='cbx' for='Chk_logo'></label><label class='lbl' for='Chk_logo'>With logo</label></div>"
                                                            + "<div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' id='Chk_fare2'  class='cb' type='checkbox' name='Chk_fare2' /><label class='cbx' for='Chk_fare2'></label><label class='lbl' for='Chk_fare2'>Single Ticket</label></div><div class='col-xs-12 col-md-4 col-sm-4 col-lg-4'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='Print' onclick='Print_return();' /></div>");
                                    }
                                    else
                                    {
                                        strBuilderPNR.Append("<div class='col-xs-12 col-sm-12 col-md-8 col-md-offset-2 col-lg-8 col-lg-offset-2'><div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_fare'  class='cb' name='Chk_fare' /><label class='cbx' for='Chk_fare'></label><label class='lbl' for='Chk_fare'>With Fare</label></div><div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_logo' checked=''  class='cb' name='Chk_logo' /><label class='cbx' for='Chk_logo'></label><label class='lbl' for='Chk_logo'>With logo</label></div><div class='col-xs-12 col-md-6 col-sm-6 col-lg-6'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='Print' onclick='Print_return();' /></div><div class='cntr cntr col-xs-12 col-md-4 col-sm-4 col-lg-4' style='display: none;'><input style='display: none;' id='Chk_fare2'  class='cb' type='checkbox' name='Chk_fare2' /><label style='display: none;' class='cbx' for='Chk_fare2'></label><label style='display: none;' class='lbl' for='Chk_fare2'>Single Ticket</label></div></div>");//
                                    }
                                strBuilderPNR.Append("</div></div></br>");
                                arrayViewPNR[Response] = strBuilderPNR.ToString();
                            }
                        }
                        #endregion
                        else
                        {
                            strBuilderPNR.Append("<div class='Viewpnr col-xs-12 col-md-12 col-sm-12 col-lg-12 form-group' ><label style='margin-bottom:10px;margin-top:10px;width: 100%;text-decoration:underline;text-align:left;cursor:pointer;margin-top:0px !important;font-weight: 500;' class='cls-header' onclick='morepnrdetail()'>More Details<label class='fa fa-angle-double-down faa-bounce animated' style='margin-top:0px !important;margin-bottom:0px !important;'></label><span style='text-align: right;float: right;color: red;'>All Fares in " + (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("Currency") ? (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["Currency"].ToString() != "" ? dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["Currency"].ToString() : ConfigurationManager.AppSettings["Currency"].ToString()) : ConfigurationManager.AppSettings["Currency"].ToString()) + "</span></label><div class='col-xs-12 col-md-6 col-sm-12 col-lg-7 Viewaddress mrepnrdetail pad-0-res mg-btm-15-res' style='display:none'>");
                            strBuilderPNR.Append("<table style='text-align: left;' border='0' class='Viewleft table table-responsive vpnrtbl'>");
                            strBuilderPNR.Append("<tr><th class='subbtitle' colspan='3' style='border-top-style: hidden;color:black;background-color: #ccc;'> Contact Details </th></tr>");
                            strBuilderPNR.Append("<tr><td>Contact Name</td><td>:</td><td>" + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("NAME") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["NAME"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["NAME"].ToString().TrimStart('.') : "N/A") + "</td></tr>");
                            strBuilderPNR.Append("<tr><td>Contact No.</td><td>:</td><td>" + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("MOBILE_NO") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["MOBILE_NO"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["MOBILE_NO"].ToString() : "N/A") + "</td></tr>");//((dsContactDetails.Tables[0].Rows[0]["MOBILE_NO"].ToString().Trim() != "") ? dsContactDetails.Tables[0].Rows[0]["MOBILE_NO"].ToString() : "N/A") + "</td></tr>");

                            strBuilderPNR.Append("<tr><td>Agency Name</td><td>:</td><td>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["AGENCY_NAME"] + "</td></tr>");
                            strBuilderPNR.Append("<tr><td>Email ID</td><td>:</td><td>" + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("EMAIL_ID") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["EMAIL_ID"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["EMAIL_ID"].ToString() : "N/A") + "</td></tr>");


                            strBuilderPNR.Append("<tr><td>Agency No.</td><td>:</td><td>" + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("AGENT NO") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["AGENT NO"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["AGENT NO"].ToString() : "N/A") + "</td></tr>");
                            strBuilderPNR.Append("<tr><td>Business No.</td><td>:</td><td> " + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("BUSINESS NO") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["BUSINESS NO"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["BUSINESS NO"].ToString() : "N/A") + "</td></tr>");
                            strBuilderPNR.Append("<tr><td>Home No.</td><td>:</td><td>" + ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("HOME NO") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["HOME NO"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["HOME NO"].ToString() : "N/A") + "</td></tr>");
                            strBuilderPNR.Append("</table>");

                            strBuilderPNR.Append("<table class='Viewleft table table-responsive' id='viewtable1'>");
                            strBuilderPNR.Append("<tr style='background-color: #2cc185;'><td class='whiter'>Booked&nbsp;Agent</td><td>:</td><td class='whiter'>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["AGENT_ID"] + "</td>");
                            strBuilderPNR.Append("<td class='whiter'>Operator&nbsp;Name</td><td>:</td><td class='whiter'>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PLATING_CARRIER"] + "</td></tr>");
                            strBuilderPNR.Append("<tr style='background-color: #2cc185;'><td class='whiter'>Payment Mode</td><td>:</td><td class='whiter'>" + PayMode + "</td>");
                            strBuilderPNR.Append("<td class='whiter'>Booked&nbsp;Date</td><td>:</td><td class='whiter'>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["BOOKED_DATE"] + "</td></tr>");
                            strBuilderPNR.Append("</table>");

                            strBuilderPNR.Append("<table class='Viewleft table table-responsive' id='viewtable2' style='display:none'>");
                            strBuilderPNR.Append("<tr style='background-color: #2cc185; color: white;'><td>Booked&nbsp;Agent</td><td>:</td><td>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["AGENT_ID"] + "</td></tr>");
                            strBuilderPNR.Append("<tr style='background-color: #2cc185; color: white;'><td>Operator&nbsp;Name</td><td>:</td><td>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PLATING_CARRIER"] + "</td></tr>");
                            strBuilderPNR.Append("<tr style='background-color: #2cc185; color: white;'><td>Payment Mode</td><td>:</td><td>" + PayMode + "</td></tr>");
                            strBuilderPNR.Append("<tr style='background-color: #2cc185; color: white;'><td>Booked&nbsp;Date</td><td>:</td><td>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["BOOKED_DATE"] + "</td></tr>");
                            strBuilderPNR.Append("</table>");


                            strBuilderPNR.Append("</div>");
                            strBuilderPNR.Append("<div class='col-xs-12 col-md-6 col-sm-12 col-lg-5  mrepnrdetail pad-0-res' style='display:none'>");
                            strBuilderPNR.Append("<table style='text-align: left;' border='0' class='Viewright table table-responsive vpnrtb2'><tr>");

                            strBuilderPNR.Append("<tr><th class='subbtitle' colspan='3' style='border-top-style: hidden;color:black;background-color: #ccc;' > Fare Details </th></tr>");
                            strBuilderPNR.Append("<tr><td class='leftaln'>Total&nbsp;Basic</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td><td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["BASICFAREAMOUNT"] + "</td></tr>");

                            strBuilderPNR.Append("<tr><td class='leftaln'>Total&nbsp;Tax</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td><td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["TAXAMOUNTCLT"] + "</td></tr>");

                            strBuilderPNR.Append("<tr><td class='leftaln'>Service&nbsp;charge(MF)</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                            strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["SERVICECHARGE"] + "</td></tr>");

                            if (dsFareDispDet.Tables[0].Columns.Contains("PCI_SF") && Convert.ToDouble(dsFareDispDet.Tables[0].Rows[0]["PCI_SF"]) != 0)
                            {
                                strBuilderPNR.Append("<tr><td class='leftaln'>Service&nbsp;charge(SF)</td>");
                                strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                                strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["PCI_SF"] + "</td></tr>");
                            }
                            if (dsFareDispDet.Tables[0].Columns.Contains("PCI_SF_GST") && Convert.ToDouble(dsFareDispDet.Tables[0].Rows[0]["PCI_SF_GST"]) != 0)
                            {
                                strBuilderPNR.Append("<tr><td class='leftaln'>GST&nbsp;on&nbsp;SF</td>");
                                strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                                strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["PCI_SF_GST"] + "</td></tr>");
                            }
                            if (dsFareDispDet.Tables[0].Columns.Contains("Meals_Amt") && Convert.ToDouble(dsFareDispDet.Tables[0].Rows[0]["Meals_Amt"]) != 0)
                            {
                                strBuilderPNR.Append("<tr><td class='leftaln'>Meals&nbsp;Amount</td>");
                                strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                                strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["Meals_Amt"] + "</td></tr>");
                            }
                            if (dsFareDispDet.Tables[0].Columns.Contains("Baggage_Amt") && Convert.ToDouble(dsFareDispDet.Tables[0].Rows[0]["Baggage_Amt"]) != 0)
                            {
                                strBuilderPNR.Append("<tr><td class='leftaln'>Baggage&nbsp;Amount</td>");
                                strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                                strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["Baggage_Amt"] + "</td></tr>");
                            }
                            if (dsFareDispDet.Tables[0].Columns.Contains("Seat_Amt") && Convert.ToDouble(dsFareDispDet.Tables[0].Rows[0]["Seat_Amt"]) != 0)
                            {
                                strBuilderPNR.Append("<tr><td class='leftaln'>Seat&nbsp;Amount</td>");
                                strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                                strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["Seat_Amt"] + "</td></tr>");
                            }
                            if (dsFareDispDet.Tables[0].Columns.Contains("Other_SSR") && Convert.ToDouble(dsFareDispDet.Tables[0].Rows[0]["Other_SSR"]) != 0)
                            {
                                strBuilderPNR.Append("<tr><td class='leftaln'>Other&nbsp;SSR&nbsp;Amount</td>");
                                strBuilderPNR.Append("<td style='text-align:center'>:</td>");
                                strBuilderPNR.Append("<td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["Other_SSR"] + "</td></tr>");
                            }
                            
                            strBuilderPNR.Append("<tr><td class='leftaln'>Commission</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td><td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["Discount"] + "</td></tr>");

                            strBuilderPNR.Append("<tr><td class='leftaln'>Total&nbsp;TDS</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td><td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["TDS_AMOUNT"] + "</td></tr>");

                            strBuilderPNR.Append("<tr><td class='leftaln'>Net&nbsp;Amount</td>");
                            strBuilderPNR.Append("<td style='text-align:center'>:</td><td style='text-align:right'>" + dsFareDispDet.Tables[0].Rows[0]["NETAMOUNTCLT"] + "</td></tr>");

                            strBuilderPNR.Append("<tr class='bgcolor_red'><td class='leftaln whiter'><b style='font-weight: 900;'>Total&nbsp;Gross </b></td><td style='text-align:center;font-weight: 900;'>:</td><td clas='whiter' style='text-align:right'><b class='whiter' style='font-weight: 900;'>" + dsFareDispDet.Tables[0].Rows[0]["GROSSAMOUNTCLT"] + "</b></td></tr>");
                            strBuilderPNR.Append("</table></div></div>");


                            strBuilderPNR.Append("<div border='0'  class='form-group Viewtotal col-xs-12 col-md-12 col-sm-12 col-lg-12 ' style='padding-top: 6px;padding-bottom: 6px;display:none'>");

                            strBuilderPNR.Append("<div class='Viewtotal col-xs-12 col-sm-6 col-md-3'>");
                            strBuilderPNR.Append("<div class='row'>");
                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc1'>");
                            strBuilderPNR.Append("<span>Booked&nbsp;Agent</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc2'>");
                            strBuilderPNR.Append("<span>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["AGENT_ID"] + "</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("</div>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='Viewtotal col-xs-12 col-sm-6 col-md-3'>");
                            strBuilderPNR.Append("<div class='row'>");
                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc1'>");
                            strBuilderPNR.Append("<span>Operator&nbsp;Name</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc2'>");
                            strBuilderPNR.Append("<span>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PLATING_CARRIER"] + "</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("</div>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='Viewtotal col-xs-12 col-sm-6 col-md-3'>");
                            strBuilderPNR.Append("<div class='row'>");
                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc1'>");
                            strBuilderPNR.Append("<span>Payment Mode</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc2'>");
                            strBuilderPNR.Append("<span>" + PayMode + "</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("</div>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='Viewtotal col-xs-12 col-sm-6 col-md-3'>");
                            strBuilderPNR.Append("<div class='row'>");
                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc1'>");
                            strBuilderPNR.Append("<span>Booked&nbsp;Date</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("<div class='col-xs-6 vpnrc2'>");
                            strBuilderPNR.Append("<span>" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["BOOKED_DATE"] + "</span>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("</div>");
                            strBuilderPNR.Append("</div>");

                            strBuilderPNR.Append("</div><br />");
                            strBuilderPNR.Append("</tr></div><br />");

                            strBuilderPNR.Append("</div>");

                            if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows.Count > 0)
                            {
                                strBuilderPNR.Append("<div class='TxBandNew col-xs-12 col-sm-12 pad-0ri' style='text-align: left;'> <label style='margin-bottom:10px;margin-top:10px;width: 100%;font-weight:500 ' class='cls-header '>Passenger Details<span style='text-align: right;float: right;color: #3cab37;font-weight: bold;'> " + (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TCK_SPECIAL_TRIP"]) + "</span></label></div> ");

                                strBuilderPNR.Append("<div class='col-sm-12 col-xs-12 pad-0ri'> ");
                                var PNRREFNO = (from input in dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].AsEnumerable()
                                                select new
                                                {
                                                    PAX_REF_NO = input["PAX_REF_NO"].ToString(),
                                                    name = input["PASSENGER_NAME"].ToString(),
                                                    Type = input["PASSENGER_TYPE"].ToString()
                                                }).Distinct();
                                DataTable dtpaxcount = Serv.ConvertToDataTable(PNRREFNO);
                                #region

                                for (int i = 1; i <= dtpaxcount.Rows.Count; i++)
                                {
                                    strBuilderPNR.Append("<h5 class='subbtitle' style='text-align: left;margin-bottom: 0px;'><label style='margin-top: 20px;margin-bottom: 10px;'><b>" +
                                    dtpaxcount.Rows[i - 1]["name"] + "(" + dtpaxcount.Rows[i - 1]["Type"] + ")" + "</b></label></h5>");
                                    // strBuilderPNR.Append("<table  class='pnrviewtab table table-hover table-striped' data-click-to-select='true' >");
                                    strBuilderPNR.Append("<table  class='pnrviewtab table table-hover table-striped no-more-tables' data-click-to-select='true' id='viewpnrdetailstable' >");
                                    //strBuilderPNR.Append("<thead><tr  width='100%' style='background-color:#d3e9ff;'><td colspan=\"14\" valign='middle' align='left' style='color:black' class='TXt_style'></td></tr>");

                                    strBuilderPNR.Append("<thead><tr>");

                                    strBuilderPNR.Append("<th data-field='direct1" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>Ticket&nbsp;No.</label></th>");
                                    strBuilderPNR.Append("<th data-field='direct2" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>Flight&nbsp;No.</label></th>");
                                    strBuilderPNR.Append("<th data-field='direct3" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>Origin</label></th>");
                                    strBuilderPNR.Append("<th data-field='direct4" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>Destination</label></th>");

                                    strBuilderPNR.Append("<th data-field='direct5" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>Departure</label></th>");
                                    strBuilderPNR.Append("<th data-field='direct6" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>Arrival</label></th>");
                                    strBuilderPNR.Append("<th data-field='direct7" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>Class</label></th>");
                                    strBuilderPNR.Append("<th data-field='direct8" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>FareRule</label></th>");
                                    strBuilderPNR.Append("<th data-field='direct9" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>Status</label></th>");

                                    strBuilderPNR.Append("<th data-field='direct11" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>Basic&nbsp;Fare</label></th>");
                                    strBuilderPNR.Append("<th data-field='direct12" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>YQ&nbsp;Tax</label></th>");
                                    strBuilderPNR.Append("<th data-field='direct13" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>Tax&nbsp;&&nbsp;Others</label></th>");

                                    strBuilderPNR.Append("<th data-field='direct14" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff;'>GrossFare</label></th>");
                                    if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("TUNEURL") && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TUNEURL"] != null && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TUNEURL"].ToString() != "")
                                    {
                                        strBuilderPNR.Append("<th data-field='direct15" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>View&nbsp;Policy</label></th>");
                                        // strBuilderPNR.Append("<span>Insurance Booking URL</span><a color='#27a5f5' href=" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TUNEURL"] + ">Policy Link</a>");
                                        // strBuilderPNR.Append("</div>");
                                    }
                                    strBuilderPNR.Append("</tr></thead><tbody>");

                                    var qryAirlineMarge = (from input in dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].AsEnumerable()
                                                           where (input["PAX_REF_NO"].ToString()).Trim() ==
                                                           dtpaxcount.Rows[i - 1]["PAX_REF_NO"].ToString()
                                                           select new
                                                           {
                                                               PASSENGER_NAME = input["PASSENGER_NAME"].ToString(),
                                                               Ticket_No = input["TICKET_NO"].ToString(),
                                                               Flight_No = input["FLIGHT_NO"].ToString(),
                                                               ORIGIN = input["ORIGIN"].ToString(),
                                                               DESTINATION = input["DESTINATION"].ToString(),
                                                               DEPT_DATE = Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["DEPT_DATE"].ToString()), "dd/MM/yyyy HH:mm"),
                                                               CLASS_ID = input["CLASS_ID"].ToString(),
                                                               BASICFAREAMOUNT = input["BASICFARE"].ToString(),
                                                               GROSSFARE = input["GROSSFARE"].ToString(),
                                                               Status = input["TICKET_STATUS"].ToString(),
                                                               Arrival = Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["ARRIVAL_DATE"].ToString()), "dd/MM/yyyy HH:mm"),
                                                               Yq_tax = input["PCI_YQ_FARE"].ToString(),
                                                               Tax_AMT = input["TAXAMTP"].ToString(),
                                                               FARE_ID = input["FARE_ID"].ToString(),
                                                               transfee = (Base.ServiceUtility.ConvertToDecimal(input["TRANSACTION_FEE"].ToString()) + Base.ServiceUtility.ConvertToDecimal(input["SERVICECHARGE"].ToString())).ToString(),
                                                               AirlineCategory = input["AIRCATEGORY"].ToString() + "SpLitPResna" + input["ORIGINCODE"].ToString() + "SpLitPResna" + input["DESTINATIONCODE"].ToString() + "SpLitPResna" + Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["DEPT_DATE"].ToString()), "dd MMM yyyy HH:mm") + "SpLitPResna" + input["Token"].ToString() + "SpLitPResna" + input["PLATING_CARRIER"].ToString() + "SpLitPResna" + Regex.Split(input["StockType"].ToString(), "SPLIT")[0],
                                                               inva = input["ORIGINCODE"].ToString() + "SpLitPResna" + input["DESTINATIONCODE"].ToString() + "SpLitPResna" + Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["DEPT_DATE"].ToString()), "dd MMM yyyy HH:mm") + "SpLitPResna" + Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["ARRIVAL_DATE"].ToString()), "dd MMM yyyy HH:mm") + "SpLitPResna" + "fourempty" + "SpLitPResna" + input["CLASS_ID"].ToString() + "SpLitPResna" + "sixempty" + "SpLitPResna" + "sevenempty" + "SpLitPResna" + "eightempty" + "SpLitPResna" + input["PLATING_CARRIER"].ToString() + "SpLitPResna" + "tenempty" + "SpLitPResna" + "elevempty"
                                                               + "SpLitPResna" + input["Token"].ToString() + "SpLitPResna" + "thirtempty" + "SpLitPResna" + "fourtenempty" + "SpLitPResna" + "fivtenempty" + "SpLitPResna" + input["AIRCATEGORY"].ToString() + "SpLitPResna" + input["FARE_BASIS"].ToString() + "SpLitPResna" + input["FLIGHT_NO"].ToString() + "SpLitPResna" + "0" + "SpLitPResna" + "twentyempty" + "SpLitPResna" + "twntyonenempty" + "SpLitPResna" + "22nempty" + "SpLitPResna" + Regex.Split(input["StockType"].ToString(), "SPLIT")[0]
                                                               + "SpLitPResna" + "twentfourempty" + "SpLitPResna" + input["FARE_ID"].ToString() + "SpLitPResna" + "twentsixempty" + "SpLitPResna" + "twentyseven" + "SpLitPResna" + "twentyeight" + "SpLitPResna" + input["SEGMENT_NO"].ToString() + "SpLitPResna" + "thirty" + "SpLitPResna" + "thirtyone" + "SpLitPResna" + "thirtytwo" + "SpLitPResna" + "thirtythree" + "SpLitPResna" + input["SPECIAL_TRIP"].ToString() + "SpLitPResna" + input["SPECIAL_TRIP"].ToString() + "SpLitPResna" + "thirtysix",
                                                               AIRPORT_ID = input["AIRPORT_ID"].ToString()
                                                           });

                                    if (qryAirlineMarge.Count() > 0)
                                    {

                                        DataTable dtsegcount = Serv.ConvertToDataTable(qryAirlineMarge);
                                        int rows = dtsegcount.Rows.Count;

                                        var Fare = (from fare in dtsegcount.AsEnumerable()
                                                    select fare["FARE_ID"].ToString()).Distinct();

                                        if (Fare.Count() == 1)
                                        {
                                            decimal Tax_fare = 0;
                                            decimal Grossamount = 0;
                                            decimal Tax_consolidate = 0;

                                            for (int tax = 0; tax < dtsegcount.Rows.Count; tax++)
                                            {
                                                if (tax > 0)
                                                {
                                                    Tax_consolidate = Tax_consolidate + Base.ServiceUtility.ConvertToDecimal(dtsegcount.Rows[tax]["Tax_AMT"].ToString());
                                                }
                                                Tax_fare = Tax_fare + Base.ServiceUtility.ConvertToDecimal(dtsegcount.Rows[tax]["Tax_AMT"].ToString());
                                            }

                                            for (int j = 0; j < dtsegcount.Rows.Count; j++)
                                            {

                                                strBuilderPNR.Append("<tr>");

                                                strBuilderPNR.Append("<td data-title='Ticket No.'><B>" + dtsegcount.Rows[j]["Ticket_No"] + "</B></td>");
                                                strBuilderPNR.Append("<td data-title='Flight No.'>" + dtsegcount.Rows[j]["Flight_No"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Origin'>" + dtsegcount.Rows[j]["ORIGIN"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Destination'>" + dtsegcount.Rows[j]["DESTINATION"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Departure'>" + dtsegcount.Rows[j]["DEPT_DATE"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Arrival'>" + dtsegcount.Rows[j]["Arrival"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Class'>" + dtsegcount.Rows[j]["CLASS_ID"] + "</td>");
                                                strBuilderPNR.Append("<td class='' data-title='FareRule'><a href='#'><label style='cursor: pointer; color: blue; border-bottom: 1px solid;' id='Fare_rule" + j + "' data-airlinecatagory=" + JsonConvert.SerializeObject(dtsegcount.Rows[j]["AirlineCategory"]) + " data-flightid=" + dtsegcount.Rows[j]["Flight_No"] + "  data-inva=" + JsonConvert.SerializeObject(dtsegcount.Rows[j]["inva"]) + " data-paxcount=" + adultcount + '|' + childcount + '|' + infantcount + " data-pnrdetails=" + Pnrdetails + " data-AirportId=" + dtsegcount.Rows[j]["AIRPORT_ID"] + " onclick='javascript:GetFareRule(this," + j + ");'>Farerule</label></a></td>");
                                                strBuilderPNR.Append("<td class='' data-title='Status'>" + dtsegcount.Rows[j]["Status"] + "</td>");

                                                if (j == 0)
                                                {
                                                    string str_rcont = (dtsegcount.Rows.Count > 1 && ConfigurationManager.AppSettings["apptheme"].ToString() == "THEME1") ? "farecls" : "";
                                                    strBuilderPNR.Append("<td data-title='Basic&nbsp;Fare' rowspan='" + dtsegcount.Rows.Count + "' style='text-align: right;padding-right: 5px !important;' class='" + str_rcont + "'>" + dtsegcount.Rows[j]["BASICFAREAMOUNT"] + "</td>");
                                                    strBuilderPNR.Append("<td data-title='YQ&nbsp;Tax' rowspan='" + dtsegcount.Rows.Count + "' style='text-align: right;padding-right: 5px !important;' class='" + str_rcont + "'>" + dtsegcount.Rows[j]["Yq_tax"] + "</td>");
                                                    strBuilderPNR.Append("<td data-title='Tax&nbsp;&&nbsp;Amount+Others' rowspan='" + dtsegcount.Rows.Count + "'  style='text-align: right;padding-right: 5px !important;' class='" + str_rcont + "' >" + Tax_fare + "</td>");
                                                    //Grossamount = Base.ServiceUtility.ConvertToDecimal(Tax_consolidate.ToString()) + Base.ServiceUtility.ConvertToDecimal(dtsegcount.Rows[j]["GROSSFARE"].ToString());
                                                    Grossamount = Base.ServiceUtility.ConvertToDecimal(dtsegcount.Rows[j]["GROSSFARE"].ToString());
                                                    strBuilderPNR.Append("<td data-title='GrossFare' style='text-align: right;padding-right: 5px !important;' rowspan='" + dtsegcount.Rows.Count + "' class='" + str_rcont + "'><B>" + Grossamount + "</B></td>");
                                                }
                                                if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("TUNEURL") && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TUNEURL"] != null && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TUNEURL"].ToString() != "")
                                                {
                                                    //strBuilderPNR.Append("<th data-field='direct15" + i + "' data-align='left' data-sortable='false'> <label style='color: #fff !important;'>Policy&nbsp;Link</label></th>");
                                                    strBuilderPNR.Append("<td data-title='Class'><a style='color:#27a5f5' href=" + dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TUNEURL"] + ">View&nbsp;Policy</a></td>");
                                                    // strBuilderPNR.Append("</div>");
                                                }
                                                strBuilderPNR.Append("<input type='hidden' id='AirlineCategory" + j + "' value='" + dtsegcount.Rows[j]["AirlineCategory"] + "' />");
                                                strBuilderPNR.Append("<input type='hidden' id='FlightID" + j + "'  value='" + dtsegcount.Rows[j]["Flight_No"] + "' /> ");
                                                strBuilderPNR.Append("<input type='hidden' id='inva" + j + "' value='" + dtsegcount.Rows[j]["inva"] + "' />  </td>");

                                                strBuilderPNR.Append("</tr>");
                                            }
                                        }
                                        else
                                        {
                                            string lstrtempfareid = "";
                                            for (int j = 0; j < dtsegcount.Rows.Count; j++)
                                            {

                                                strBuilderPNR.Append("<tr>");

                                                strBuilderPNR.Append("<td data-title='Ticket No.'><B>" + dtsegcount.Rows[j]["Ticket_No"] + "</B></td>");
                                                strBuilderPNR.Append("<td data-title='Flight No.'>" + dtsegcount.Rows[j]["Flight_No"] + "</td>");
                                                strBuilderPNR.Append("<td  data-title='Origin'>" + dtsegcount.Rows[j]["ORIGIN"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Destination'>" + dtsegcount.Rows[j]["DESTINATION"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Departure'>" + dtsegcount.Rows[j]["DEPT_DATE"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Arrival'>" + dtsegcount.Rows[j]["Arrival"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='Class'>" + dtsegcount.Rows[j]["CLASS_ID"] + "</td>");
                                                strBuilderPNR.Append("<td data-title='FareRule' class='label label-success spnEarn'><a href='#'><label style='cursor: pointer; color: blue; border-bottom: 1px solid;' id='Fare_rule" + j + "' data-airlinecatagory=" + JsonConvert.SerializeObject(dtsegcount.Rows[j]["AirlineCategory"]) + " data-flightid=" + dtsegcount.Rows[j]["Flight_No"] + "  data-inva=" + JsonConvert.SerializeObject(dtsegcount.Rows[j]["inva"]) + "  data-paxcount=" + adultcount + '|' + childcount + '|' + infantcount + " data-pnrdetails=" + Pnrdetails + " data-AirportId=" + dtsegcount.Rows[j]["AIRPORT_ID"] + " onclick='javascript:GetFareRule(this," + j + ");'>Farerule</label></a></td>");

                                                strBuilderPNR.Append("<td data-title='Status'><B>" + dtsegcount.Rows[j]["Status"] + "</B></td>");

                                                if (string.IsNullOrEmpty(lstrtempfareid) || lstrtempfareid != dtsegcount.Rows[j]["FARE_ID"].ToString().Trim())
                                                {
                                                    strBuilderPNR.Append("<td data-title='Basic&nbsp;Fare' rowspan='" + (dtsegcount.Rows.Count / 2) + "'>" +
                                                        dtsegcount.Rows[j]["BASICFAREAMOUNT"] + "</td>");
                                                    strBuilderPNR.Append("<td data-title='YQ&nbsp;Tax' rowspan='" + (dtsegcount.Rows.Count / 2) + "'>" +
                                                        dtsegcount.Rows[j]["Yq_tax"] + "</td>");
                                                    strBuilderPNR.Append("<td data-title='Tax&Others' rowspan='" + (dtsegcount.Rows.Count / 2) + "'>" +
                                                        dtsegcount.Rows[j]["Tax_AMT"] + "</td>");
                                                    strBuilderPNR.Append("<td data-title='GrossFare' rowspan='" + (dtsegcount.Rows.Count / 2) + "'><B>" +
                                                        dtsegcount.Rows[j]["GROSSFARE"] + "</B>");
                                                    lstrtempfareid = dtsegcount.Rows[j]["FARE_ID"].ToString().Trim();
                                                }
                                                strBuilderPNR.Append("<input type='hidden'  id='AirlineCategory" + j + "'   value='" + dtsegcount.Rows[j]["AirlineCategory"] + "' />");
                                                strBuilderPNR.Append("<input type='hidden' id='FlightID" + j + "' value='" + dtsegcount.Rows[j]["Flight_No"] + "' /> ");
                                                strBuilderPNR.Append("<input type='hidden' id='inva" + j + "'  value='" + dtsegcount.Rows[j]["inva"] + "' /></td>");
                                                strBuilderPNR.Append("</tr>");
                                            }
                                        }
                                    }

                                    strBuilderPNR.Append("</tbody></table><br />");

                                }
                                #endregion
                                #region New Additional Services Details
                                if (ConfigurationManager.AppSettings["ADDONS_VIEWPNR"] != null && ConfigurationManager.AppSettings["ADDONS_VIEWPNR"].ToString() == "Y" && Convert.ToDateTime(dsViewPNR.Tables[0].Rows[0]["DEPT_DATE"].ToString()) > DateTime.Now && dsViewPNR.Tables[0].Rows[0]["TICKET_STATUS"].ToString().ToUpper() == "CONFIRMED")
                                {
                                    strBuilderPNR.Append("<div class=\"TxBandNew\" style=\"text-align: left;\"><label style=\"margin-bottom:10px;margin-top:10px;width: 100%;font-weight:500\" class=\"cls-header\">Additional Service</label></div>");
                                    strBuilderPNR.Append("<div class='col-xs-12' id='dvAdditionalServ'>");
                                    strBuilderPNR.Append("<a href='" + Url.Action("OtherProduct", "OtherProduct") + "?Flag=ANCV'>");
                                    strBuilderPNR.Append("<div class=\"clsSSRAddons ssr-sec\" style=\"margin: 10px !important;padding: 10px;\">");
                                    strBuilderPNR.Append("<div class=\"service_icon_o\"><img src=\"/Images/" + strProduct + "/Other.png\"></div>");
                                    strBuilderPNR.Append("<br /><span>Ancillary Services</span>");
                                    strBuilderPNR.Append("</div>");
                                    strBuilderPNR.Append("</a>");
                                    strBuilderPNR.Append("</div>");
                                    Session.Add("PNRFLAG_ANC", strSPNRNO);
                                }
                                #endregion
                                strBuilderPNR.Append("<div class='col-xs-10 col-md-6 col-sm-8 col-lg-11 col-lg-offset-1' style='text-align:center;margin-top: 2%;' >");
                                if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() == "H" && Session["ticketing"].ToString().Trim() == "Y")
                                {
                                    strBuilderPNR.Append("<div class=' col-xs-6 col-md-6 col-sm-4 col-lg-6'><input type='button' class='action-button shadow animate color' value='Issue Ticket' style='width:100%'  onclick='return toticket();'/></div>");
                                    strBuilderPNR.Append("<div class=' col-xs-6 col-md-6 col-sm-4 col-lg-6'><input type='button' class='action-button action-buttonB shadow animate  colorB1' value='Cancel Ticket' style='width:100%'  onclick='return cancelticket();'/></div>");
                                }
                                if (dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() != "H" && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() != "B" && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() != "X" && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() != "I" && dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TICKET_STATUS_FLAG"].ToString() != "W")

                                    if (ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA")
                                    {
                                        if (dtpaxcount.Rows.Count > 1)
                                        {
                                            if (dsViewPNR.Tables[0].Columns.Contains("Reprint_Policy") == true && dsViewPNR.Tables[0].Rows[0]["Reprint_Policy"].ToString() != "")
                                            {
                                                string Policyno = dsViewPNR.Tables[0].Rows[0]["Reprint_Policy"].ToString();
                                                strBuilderPNR.Append("<div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_fare'  class='cb' name='Chk_fare' /><label class='cbx' for='Chk_fare'></label><label class='lbl' for='Chk_fare'>With Fare</label></div><div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_logo' checked=''  class='cb' name='Chk_logo' /><label class='cbx' for='Chk_logo'></label><label class='lbl' for='Chk_logo'>With logo</label></div><div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' id='Chk_fare2'  class='cb' type='checkbox' name='Chk_fare2' /><label class='cbx' for='Chk_fare2'></label><label class='lbl' for='Chk_fare2'>Single Ticket</label></div><div class='col-xs-12 col-md-3 col-sm-3 col-lg-2'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='Print' onclick='Print_return();' /></div><div class='col-xs-12 col-md-2 col-lg-2'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='SMS' onclick='ViewSMS()' /></div><div class='col-xs-12 col-md-3 col-sm-3 col-lg-2 col0'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='Print Policy' onclick='PrintInspolicy(" + Policyno + ")' /></div></div>");
                                            }
                                            else
                                            {
                                                strBuilderPNR.Append("<div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_fare'  class='cb' name='Chk_fare' /><label class='cbx' for='Chk_fare'></label><label class='lbl' for='Chk_fare'>With Fare</label></div><div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_logo' checked=''  class='cb' name='Chk_logo' /><label class='cbx' for='Chk_logo'></label><label class='lbl' for='Chk_logo'>With logo</label></div><div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' id='Chk_fare2'  class='cb' type='checkbox' name='Chk_fare2' /><label class='cbx' for='Chk_fare2'></label><label class='lbl' for='Chk_fare2'>Single Ticket</label></div><div class='col-xs-12 col-md-3 col-sm-3 col-lg-2'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='Print' onclick='Print_return();' /></div><div class='col-xs-12 col-md-2 col-lg-2'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='SMS' onclick='ViewSMS()' /></div><div class='col-xs-12 col-md-3 col-sm-3 col-lg-2'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='Email' onclick='SendtoMail()'></div></div>");
                                            }
                                        }
                                        else
                                        {
                                            strBuilderPNR.Append("<div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_fare'  class='cb' name='Chk_fare' /><label class='cbx' for='Chk_fare'></label><label class='lbl' for='Chk_fare'>With Fare</label></div><div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_logo' checked=''  class='cb' name='Chk_logo' /><label class='cbx' for='Chk_logo'></label><label class='lbl' for='Chk_logo'>With logo</label></div><div class='col-xs-12 col-md-4 col-sm-4 col-lg-2'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='Print' onclick='Print_return();' /></div><div class='col-xs-12 col-md-3 col-sm-3 col-lg-2'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='SMS' onclick='ViewSMS()' /></div><div class='col-xs-12 col-md-2 col-lg-2'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='Email' onclick='SendtoMail()'></div><div class='cntr col-xs-12 col-md-2 col-lg-2' style='display: none;'><input style='display: none;' id='Chk_fare2'  class='cb' type='checkbox' name='Chk_fare2' /><label style='display: none;' class='cbx' for='Chk_fare2'></label><label style='display: none;' class='lbl' for='Chk_fare2'>Single Ticket</label></div>");//<div class='col-xs-12  col-sm-12 col-md-8 col-md-offset-2 col-lg-8 col-lg-offset-2'></div>
                                        }
                                    }
                                    else
                                    {
                                        if (dtpaxcount.Rows.Count > 1)
                                        {
                                            strBuilderPNR.Append("<div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_fare'  class='cb' name='Chk_fare' /><label class='cbx' for='Chk_fare'></label><label class='lbl' for='Chk_fare'>With Fare</label></div><div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_logo' checked=''  class='cb' name='Chk_logo' /><label class='cbx' for='Chk_logo'></label><label class='lbl' for='Chk_logo'>With logo</label></div><div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' id='Chk_fare2'  class='cb' type='checkbox' name='Chk_fare2' /><label class='cbx' for='Chk_fare2'></label><label class='lbl' for='Chk_fare2'>Single Ticket</label></div><div class='col-xs-12 col-md-3 col-sm-3 col-lg-2'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='Print' onclick='Print_return();' /></div></div>");
                                        }
                                        else
                                        {
                                            strBuilderPNR.Append("<div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_fare'  class='cb' name='Chk_fare' /><label class='cbx' for='Chk_fare'></label><label class='lbl' for='Chk_fare'>With Fare</label></div><div class='cntr col-xs-12 col-md-2 col-sm-2 col-lg-2'><input style='display: none;' type='checkbox' id='Chk_logo' checked=''  class='cb' name='Chk_logo' /><label class='cbx' for='Chk_logo'></label><label class='lbl' for='Chk_logo'>With logo</label></div><div class='col-xs-12 col-md-4 col-sm-4 col-lg-2'><input style='width: 100%;' type='button' class='action-button shadow animate color' value='Print' onclick='Print_return();' /></div><div class='cntr col-xs-12 col-md-2 col-lg-2' style='display: none;'><input style='display: none;' id='Chk_fare2'  class='cb' type='checkbox' name='Chk_fare2' /><label style='display: none;' class='cbx' for='Chk_fare2'></label><label style='display: none;' class='lbl' for='Chk_fare2'>Single Ticket</label></div>");//<div class='col-xs-12  col-sm-12 col-md-8 col-md-offset-2 col-lg-8 col-lg-offset-2'></div>
                                        }
                                    }
                                strBuilderPNR.Append("</div></div></br>");
                                arrayViewPNR[Response] = strBuilderPNR.ToString();
                            }
                        }

                    }
                    else
                    {
                        string display = "Please enter valid PNR No.";
                        arrayViewPNR[Error] = display.ToString();
                        //   ClientScript.RegisterStartupScript(this.GetType(), "Invalid input", "alert('" + display + "');", true);
                    }

                }

            }
            catch (Exception _ex)
            {
                if (strTKTFLAG == "QTKT")
                {
                    DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "X", "ViewPnr", "RequestViewPNRFunction", _ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                else
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "ViewPnr", "RequestViewPNRFunction", _ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                arrayViewPNR[Error] = "Please enter valid PNR No.";
                //    ClientScript.RegisterStartupScript(this.GetType(), "Invalid input", "alert('" + display + "');", true);
            }
            return Json(new { Status = "", Message = "", Result = arrayViewPNR });
            //  return ;
        }
        #endregion

        #region cancellation function

        public ActionResult CancellationRequest(string strSPNRNO, string strAirPNRNO, string strCRSPNRNO, string strpaymentmode)
        {
            #region UsageLog
            string PageName = "Cancellation";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
            }
            catch (Exception e) { }
            #endregion

            RaysService _RaysService = new RaysService();
            string strAgentID = string.Empty;// Session["agentid"].ToString();//"TASHA0400001";
            string strTerminalId = string.Empty;//  Session["terminalid"].ToString();//"TASHA040000102";
            string strUserName = string.Empty;//  Session["username"].ToString();//"mohammed";
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;
            ArrayList arrayViewPNR = new ArrayList();
            ArrayList arrayaddPNR = new ArrayList();
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");

            int pax_response = 1;
            int pnr_response = 2;
            int modify_response = 3;
            int tckt_response = 4;
            int error = 0;

            string getAgentType = string.Empty;
            string getprevilage = string.Empty;
            StringBuilder strBuilderPNR = new StringBuilder();

            strTerminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString().ToUpper().Trim() : "";
            strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            terminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();// "W";
            string POS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string POS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"] != "") ? Session["TKTFLAG"].ToString() : "";
            string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");

            try
            {
                if (strTerminalId == "" || strUserName == "" || strAgentID == "" || Ipaddress == "" || sequnceID == "" || terminalType == "" || POS_ID == "" || POS_TID == "" || strTKTFLAG == "")
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }
                getprevilage = Session["privilage"].ToString();
                getAgentType = Session["agenttype"].ToString();

                #region
                string ConsoleAgent = ConfigurationManager.AppSettings["ConsoleAgent"].ToString();// "";
                if (getprevilage.ToString() == "S")
                {
                    strTerminalId = string.Empty;
                }
                else
                {
                    strTerminalId = Session["POS_TID"].ToString();
                }
                if (getAgentType == ConsoleAgent)
                {
                    strAgentID = string.Empty;
                    strTerminalId = string.Empty;
                }
                if (terminalType == "T")
                {
                    strAgentID = string.Empty;
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

                strSPNRNO = strSPNRNO.Trim();
                strAirPNRNO = strAirPNRNO.Trim();
                strCRSPNRNO = strCRSPNRNO.Trim();
                DataSet dsDisplayDet = new DataSet();
                _RaysService.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                result = _RaysService.Fetch_PNR_Verification_Details_NewByte(strAgentID, strSPNRNO, strAirPNRNO, strCRSPNRNO, "", strTerminalId, strUserName, Ipaddress, "W", Convert.ToDecimal(sequnceID), ref dsViewPNR_compress, ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "FetchCancellationPNR", "FetchPNRdetailsforcancellation", getAgentType, strTerminalId);

                if (strProduct == "RBOA" && result == false && terminalType == "T")
                {
                    _RaysService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                    result = _RaysService.Fetch_PNR_Verification_Details_NewByte(strAgentID, strSPNRNO, strAirPNRNO, strCRSPNRNO, "", strTerminalId, strUserName, Ipaddress, "W", Convert.ToDecimal(sequnceID), ref dsViewPNR_compress, ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "FetchCancellationPNR", "FetchPNRdetailsforcancellation", getAgentType, strTerminalId);
                }
                dsViewPNR = Base.Decompress(dsViewPNR_compress);
                dsDisplayDet = Base.Decompress(dsFareDispDet_compress);


                if (result == false)
                {
                    if (dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                    {
                        string record = JsonConvert.SerializeObject(dsViewPNR);
                        arrayViewPNR[modify_response] = record;
                    }
                    else
                    {
                        arrayViewPNR[error] = "Please enter valid PNR No.";
                    }
                }
                else
                {
                    if (dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[1].Rows.Count == 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                    {
                        string strClientBranchID = string.Empty;
                        strClientBranchID = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("TAI_ISSUING_BRANCH_ID") ? dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TAI_ISSUING_BRANCH_ID"].ToString() : "";
                        Session.Add("ClientBranchID", strClientBranchID);
                        if (dsViewPNR != null)
                        {
                            string spnr = dsViewPNR.Tables[0].Rows[0]["S_PNR"].ToString();
                            string airline_pnr = dsViewPNR.Tables[0].Rows[0]["AIRLINE_PNR"].ToString();
                            string crs_pnr = dsViewPNR.Tables[0].Rows[0]["CRS_PNR"].ToString();
                            arrayViewPNR[pnr_response] = spnr + "|" + airline_pnr + "|" + crs_pnr;
                        }
                        if (dsViewPNR.Tables[0].Rows.Count > 0)
                        {

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
                            string paxdetails = JsonConvert.SerializeObject(PaxInfo);
                            string Ticketdetails = JsonConvert.SerializeObject(Ticketinfo);
                            arrayViewPNR[pax_response] = paxdetails;
                            arrayViewPNR[tckt_response] = Ticketdetails;
                        }
                    }
                    else if (dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[1].Rows.Count > 0)
                    {
                        arrayViewPNR[error] = "Already reschedule request in-processing.  Reschedule id:" + (dsViewPNR.Tables[1].Rows.Count > 0 ? dsViewPNR.Tables[1].Rows[0]["RDL_SEQ_NO"].ToString() : "");
                    }
                    else
                    {
                        arrayViewPNR[error] = "Please enter valid PNR No.";
                    }
                }
            }
            catch (Exception ex)
            {
                if (strTKTFLAG == "QTKT")
                {
                    DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "X", "Cancellation", "Requestcancellation", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                else
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "Cancellation", "Requestcancellation", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                arrayViewPNR[error] = "Please enter valid PNR No.";
            }
            return Json(new { Status = "", Message = "", Result = arrayViewPNR });
        }

        public ActionResult cancelrequest(string PaxDetails, string Ticketdetails, string PaxcancelationStatus, string Ticketcancelstatus, string Remarks, string strPaymentmode)
        {
            RaysService _RaysService = new RaysService();
            DataSet dtRaysResult = new DataSet();
            string strResult = string.Empty;
            string departureDate = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            int penaltyamt = 1;
            int refundamount = 2;
            int Cancelationtype = 3;
            DataSet dsViewPNR = new DataSet();
            DataSet dsFareDispDet = new DataSet();

            string refError = "ERROR";
            string strErrorMsg = "ERROR";
            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string sequnceID = string.Empty;
            string Ipaddress = string.Empty;
            string getprevilage = string.Empty;
            string getAgentType = string.Empty;
            bool result = false;
            bool response = false;
            string checkflag = string.Empty;


            strTerminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString().ToUpper().Trim() : "";
            strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            terminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();// "W";
            string POS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string POS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"] != "") ? Session["TKTFLAG"].ToString() : "";
            string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");

            try
            {
                if (strTerminalId == "" || strUserName == "" || strAgentID == "" || Ipaddress == "" || sequnceID == "" || terminalType == "" || POS_ID == "" || POS_TID == "" || strTKTFLAG == "")
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }
                #region UsageLog
                string PageName = "Cancellation";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "UPDATE");
                }
                catch (Exception e) { }
                #endregion
                getprevilage = Session["privilage"].ToString();
                getAgentType = Session["agenttype"].ToString();

                #region
                string ConsoleAgent = ConfigurationManager.AppSettings["ConsoleAgent"].ToString();// "";
                if (getprevilage.ToString() == "S")
                {
                    strTerminalId = string.Empty;
                }
                else
                {
                    strTerminalId = Session["terminalid"].ToString();
                }
                if (getAgentType == ConsoleAgent)
                {
                    strAgentID = string.Empty;
                    strTerminalId = string.Empty;
                }
                if (terminalType == "T")
                {
                    strAgentID = string.Empty;
                }
                #endregion

                byte[] dsViewPNR_compress = new byte[] { };
                byte[] dsFareDispDet_compress = new byte[] { };

                string Spnr = PaxDetails.TrimEnd('|').Trim().Split('|')[0].Split(',')[1].ToString();

                #region SERVICE URL BRANCH BASED -- STS195
                string strClientBranchID = Session["ClientBranchID"] != null && Session["ClientBranchID"].ToString() != "" ? Session["ClientBranchID"].ToString() : "";
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (strClientBranchID != "" && strBranchCredit.Contains(strClientBranchID)))
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                    }
                    else
                    {
                        _RaysService.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    }
                }
                else
                {
                    _RaysService.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                }
                #endregion
                result = _RaysService.Fetch_PNR_Verification_Details_NewByte(strAgentID, Spnr, "", "", "",
                               strTerminalId, strUserName, Ipaddress, "W", Convert.ToDecimal(sequnceID), ref dsViewPNR_compress,
                           ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "Cancel PNR request", "PNR cancelrequest", getAgentType, strTerminalId);


                dsViewPNR = Base.Decompress(dsViewPNR_compress);
                dsFareDispDet = Base.Decompress(dsFareDispDet_compress);

                if (dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                {
                    string StockType = Regex.Split(dsViewPNR.Tables[0].Rows[0]["StockType"].ToString(), "SPLIT")[0].ToString();
                    string Aircategory = dsViewPNR.Tables[0].Rows[0]["AIRLINE_CATEGORY"].ToString();
                    checkflag = (ConfigurationManager.AppSettings["Onlinecancellation"].ToString().Contains(Aircategory) || ConfigurationManager.AppSettings["Onlinecancellation"].ToString().Contains(StockType)) ? "ONLINE" : "OFFLINE";
                }

                if (PaxcancelationStatus == "P" || Ticketcancelstatus == "P" || checkflag == "" || checkflag == "OFFLINE") // Direct send offline request if partial cancellation
                {
                    Array_Book[Cancelationtype] = "0";
                    Array_Book[error] = Offlinecancellationrequest(PaxDetails, Ticketdetails, PaxcancelationStatus, Ticketcancelstatus, Remarks);
                    return Json(new { Status = "", Message = "", Result = Array_Book });
                }
                else // online cancellation request send
                {
                    if (dsViewPNR.Tables[0].Rows[0]["AIRLINE_CATEGORY"].ToString() == "LCC")
                    {

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
                            dtTcktDet.Rows.Add(dsViewPNR.Tables[0].Rows[i]["PASSENGER_NAME"].ToString(), dsViewPNR.Tables[0].Rows[i]["S_PNR"].ToString(), dsViewPNR.Tables[0].Rows[i]["PAX_REF_NO"].ToString(), dsViewPNR.Tables[0].Rows[i]["SEGMENT_NO"].ToString(),
                                Remarks, Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["AdultCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["ChildCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["InfantCount"]), dsViewPNR.Tables[0].Rows[0]["DEPTDT"].ToString(), dsViewPNR.Tables[0].Rows[0]["ORIGINCODE"].ToString(), //dsViewPNR.Tables[0].Rows[0]["DEPTDT"].ToString()
                                dsViewPNR.Tables[0].Rows[0]["DESTINATIONCODE"].ToString(), dsViewPNR.Tables[0].Rows[0]["AGENT_EMAIL_ID"].ToString(), (dsViewPNR.Tables[0].Rows[0]["AGENCY_NAMEADDRESS"] != null && dsViewPNR.Tables[0].Rows[0]["AGENCY_NAMEADDRESS"].ToString() != "" ? dsViewPNR.Tables[0].Rows[0]["AGENCY_NAMEADDRESS"].ToString().Split('~')[5] : ""),
                                dsViewPNR.Tables[0].Rows[0]["AIRLINE_PNR"].ToString(), dsViewPNR.Tables[0].Rows[0]["PASSENGER_TYPE"].ToString(), dsViewPNR.Tables[0].Rows[0]["AIRPORTID"].ToString(), dsViewPNR.Tables[0].Rows[0]["TRIP_DESC"].ToString(), dsViewPNR.Tables[0].Rows[i]["S_PNR"].ToString(), "O");
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
                        RQRStable.Rows.Add(dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), Session["agenttype"].ToString(), dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), dsViewPNR.Tables[0].Rows[0]["TERMINAL_ID"].ToString(),
                            "", dsViewPNR.Tables[0].Rows[0]["TERMINAL_ID"].ToString(), dsViewPNR.Tables[0].Rows[0]["USER_NAME"].ToString(), "", "W", "B2B", Session["PRODUCT_CODE"].ToString(), dsViewPNR.Tables[0].Rows[0]["Currency"].ToString(), "B",
                            dsViewPNR.Tables[0].Rows[0]["AdultCount"].ToString(), dsViewPNR.Tables[0].Rows[0]["ChildCount"].ToString(), dsViewPNR.Tables[0].Rows[0]["InfantCount"].ToString());

                        dsCancelTicketDetails.Tables.Add(dtTcktDet.Copy());
                        dsCancelTicketDetails.Tables.Add(dsViewPNR.Tables[0].Copy());
                        dsCancelTicketDetails.Tables.Add(RQRStable.Copy());


                        string cursorSeqNo = string.Empty;
                        DataSet dsPenalty = new DataSet();
                        bool onOFFflag = false;
                        string request = string.Empty;
                        strErrorMsg = string.Empty;
                        onOFFflag = true;// for online request flag
                        string Penaltyresult = _RaysService.Fetch_UpdateDetails(dsCancelTicketDetails, Session["POS_ID"].ToString(), Session["POS_ID"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(), Session["POS_TID"].ToString(), Convert.ToDecimal(Session["sequenceid"].ToString()), ref cursorSeqNo, ref strErrorMsg, "Fetch_UpdateDetails", "OnlinecancellationFetchPenalty", Session["agenttype"].ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), ref dsPenalty, ref request, ref onOFFflag);

                        string logdata = "<REQUEST><URL>" + ConfigurationManager.AppSettings["ServiceURI"].ToString() + "</URL><INPUT>" + dsCancelTicketDetails.GetXml() + "</INPUT><POSID>" + Session["POS_ID"].ToString()
                                        + "</POSID><POSTID>" + Session["POS_ID"].ToString() + "</POSTID><USERNAME>" + Session["username"].ToString() + "</USERNAME><CURSORSEQNO>" + cursorSeqNo + "</CURSORSEQNO><ERRORMSG>" + strErrorMsg
                                        + "</ERRORMSG></REQUEST><RESPONSE><PENALTY>" + dsPenalty.GetXml() + "</PENALTY><REQ_REF>" + request + "</REQ_REF><FLAG>" + onOFFflag + " "+checkflag +"</FLAG></RESPONSE>";

                        if (strTKTFLAG == "QTKT")
                        {
                            DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "E", "CancellationREQ", "OnlineCancellation-FetchPenalty", logdata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        }
                        else
                        {
                            DatabaseLog.LogData(Session["username"].ToString(), "E", "CancellationREQ", "OnlineCancellation-FetchPenalty", logdata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        }
                        Session.Add("Cancelseqid", cursorSeqNo);
                        Session.Add("Cancellationreq", string.IsNullOrEmpty(request) ? "" : request); // need to pass in confirm cancellation request
                        Array_Book[Cancelationtype] = "1";//0- for offline ,1 for online
                        if (onOFFflag == false)
                        {
                            if (strErrorMsg != null && strErrorMsg != "")
                            {
                                Array_Book[error] = strErrorMsg;
                            }
                            else
                            {
                                Array_Book[error] = Offlinecancellationrequest(PaxDetails, Ticketdetails, PaxcancelationStatus, Ticketcancelstatus, Remarks);
                            }
                            return Json(new { Status = "", Message = "", Result = Array_Book });
                        }
                        if (dsPenalty != null && dsPenalty.Tables.Count > 0)
                        {
                            Session.Add("PENALITYDATA", dsPenalty);
                            string strOnlinePenaltyAmt = ((dsPenalty.Tables.Count > 1 && dsPenalty.Tables[1].Columns.Contains("PenaltyAmount")) ? dsPenalty.Tables[1].Rows[0]["PenaltyAmount"].ToString() : (dsPenalty.Tables[0].Columns.Contains("PenaltyAmount") && dsPenalty.Tables[0].Rows.Count > 0) ? dsPenalty.Tables[0].Rows[0]["PenaltyAmount"].ToString() : "0");
                            string strOnlineRefundAmt = ((dsPenalty.Tables.Count > 1 && dsPenalty.Tables[1].Columns.Contains("RefundAmount")) ? dsPenalty.Tables[1].Rows[0]["RefundAmount"].ToString() : (dsPenalty.Tables[0].Columns.Contains("RefundAmount") && dsPenalty.Tables[0].Rows.Count > 0) ? dsPenalty.Tables[0].Rows[0]["RefundAmount"].ToString() : "0");
                            string markupamnt = string.Empty;

                            try
                            {
                                byte[] outdata = Convert.FromBase64String(strErrorMsg);
                                DataSet dsResult = new DataSet();
                                if (outdata != null && outdata.Count() > 0)
                                    dsResult = Base.Decompress(outdata);
                                string strresponse = JsonConvert.SerializeObject(dsResult);

                                var qryTravel = (from _Air in dsResult.Tables[0].AsEnumerable()
                                                 where _Air["PAX_REF_NO"].ToString() == "1"
                                                 select new
                                                 {
                                                     Trip_No = _Air["TCK_TRIP_NO"].ToString(),
                                                     Segment_No = _Air["TotalSegment"].ToString(),
                                                 }).ToList();
                                var qryTravels = qryTravel.GroupBy(grp => grp.Trip_No).Select(grp => grp.ToList()).ToList();
                                string Tripdata = string.Empty;
                                for (int mkp = 0; mkp < qryTravels.Count; mkp++)
                                {
                                    Tripdata += qryTravels[mkp][0].Segment_No.ToString() + ",";
                                }
                                Tripdata = Tripdata.TrimEnd(',');
                                Session.Add("TRIPDATA", Tripdata);

                                if (dsPenalty.Tables.Count > 2 && dsPenalty.Tables[2].Rows.Count > 0 && dsPenalty.Tables[2].Columns.Contains("CPR_MARKUP"))
                                {
                                    if (dsPenalty.Tables[2].Rows[0]["CPR_MARKUP_BASED"].ToString() == "PPT")
                                        markupamnt = (Convert.ToDecimal(dsPenalty.Tables[2].Rows[0]["CPR_MARKUP"].ToString()) * Convert.ToDecimal((Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["AdultCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["ChildCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["InfantCount"])).ToString())).ToString();
                                    else if (dsPenalty.Tables[2].Rows[0]["CPR_MARKUP_BASED"].ToString() == "PPS")
                                        markupamnt = (Convert.ToDecimal(dsPenalty.Tables[2].Rows[0]["CPR_MARKUP"].ToString()) * Convert.ToDecimal((Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["AdultCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["ChildCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["InfantCount"])).ToString()) * Convert.ToDecimal(dsViewPNR.Tables[0].Rows.Count)).ToString();
                                    else if (dsPenalty.Tables[2].Rows[0]["CPR_MARKUP_BASED"].ToString() == "PPI")
                                        markupamnt = (Convert.ToDecimal(dsPenalty.Tables[2].Rows[0]["CPR_MARKUP"].ToString()) * Convert.ToDecimal((Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["AdultCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["ChildCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["InfantCount"])).ToString()) * Convert.ToDecimal(qryTravels.Count)).ToString();

                                }
                            }
                            catch (Exception ex)
                            {
                                if (dsPenalty.Tables.Count > 2 && dsPenalty.Tables[2].Rows.Count > 0 && dsPenalty.Tables[2].Columns.Contains("CPR_MARKUP"))
                                {
                                    markupamnt = (Convert.ToDecimal(dsPenalty.Tables[2].Rows[0]["CPR_MARKUP"].ToString()) * Convert.ToDecimal((Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["AdultCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["ChildCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["InfantCount"])).ToString())).ToString();
                                }
                            }
                            markupamnt = markupamnt == "" ? "0" : markupamnt;
                            strOnlinePenaltyAmt = (Convert.ToDouble(strOnlinePenaltyAmt) + Convert.ToDouble(markupamnt)).ToString();
                            Array_Book[penaltyamt] = strOnlinePenaltyAmt;// (dsPenalty.Tables[1].Columns.Contains("PenaltyAmount") ? dsPenalty.Tables[1].Rows[0]["PenaltyAmount"].ToString() : dsPenalty.Tables[0].Columns.Contains("PenaltyAmount") ? dsPenalty.Tables[0].Rows[0]["PenaltyAmount"].ToString() : "0"); 
                            Array_Book[refundamount] = strOnlineRefundAmt; // (dsPenalty.Tables[1].Columns.Contains("RefundAmount") ? dsPenalty.Tables[1].Rows[0]["RefundAmount"].ToString() : dsPenalty.Tables[0].Columns.Contains("RefundAmount") ? dsPenalty.Tables[0].Rows[0]["RefundAmount"].ToString() : "0"); 

                            return Json(new { Status = "", Message = "", Result = Array_Book });
                        }
                        else
                        {
                            if (strErrorMsg != null && strErrorMsg != "")
                            {
                                Array_Book[error] = strErrorMsg;
                            }
                            else
                            {
                                Array_Book[error] = Offlinecancellationrequest(PaxDetails, Ticketdetails, PaxcancelationStatus, Ticketcancelstatus, Remarks);
                            }
                            return Json(new { Status = "", Message = "", Result = Array_Book });
                        }

                    }
                    else
                    {
                        byte[] byteRaysResults = _RaysService.GetTicketcancelpopup_BOA(dsViewPNR.Tables[0].Rows[0]["S_PNR"].ToString(), "", dsViewPNR.Tables[0].Rows[0]["USER_NAME"].ToString(), Session["ipAddress"].ToString(), "", "GetTicketcancelpopup_BOA", "FetchFscCancellationPenaltyAmount");
                        dtRaysResult = Base.Decompress(byteRaysResults);
                        Session.Add("Cancellationreq", ""); //** need to pass in confirm cancellation request LCC airline available so here pass as empty **//
                        Session.Add("Cancelseqid", ""); //** need to pass in cancel cancellation request LCC airline available so here pass as empty **//
                        string logdata = "<REQUEST><URL>" + ConfigurationManager.AppSettings["ServiceURI"].ToString() + "</URL><SPNR>" + dsViewPNR.Tables[0].Rows[0]["S_PNR"].ToString() + "</SPNR><POSID>" + Session["POS_ID"].ToString()
                                        + "</POSID><POSTID>" + Session["POS_ID"].ToString() + "</POSTID><USERNAME>" + dsViewPNR.Tables[0].Rows[0]["USER_NAME"].ToString() + "</USERNAME></REQUEST><RESPONSE><PENALTY>" + dtRaysResult.GetXml()
                                        + "</PENALTY></RESPONSE>";
                        if (strTKTFLAG == "QTKT")
                        {
                            DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "E", "FSC-Cancellation", "OnlineCancellation-FetchPenalty", logdata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        }
                        else
                        {
                            DatabaseLog.LogData(Session["username"].ToString(), "E", "FSC-Cancellation", "OnlineCancellation-FetchPenalty", logdata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        }
                        if (dtRaysResult != null && dtRaysResult.Tables != null && dtRaysResult.Tables.Count > 8 && dtRaysResult.Tables[8].Rows.Count > 0)
                        {
                            string penaltyamount = "";
                            string markupamnt = "";
                            string TotalPenaltyamont = "";
                            if (dtRaysResult.Tables[8].Columns.Contains("CPR_PENALTY") && dtRaysResult.Tables[8].Rows[0]["CPR_PENALTY"].ToString() != "")
                            {
                                penaltyamount = dtRaysResult.Tables[8].Rows[0]["CPR_PENALTY"].ToString();
                                if (dtRaysResult.Tables.Count > 9 && dtRaysResult.Tables[9].Rows.Count > 0 && dtRaysResult.Tables[9].Columns.Contains("CPR_MARKUP"))
                                {
                                    markupamnt = dtRaysResult.Tables[9].Rows[0]["CPR_MARKUP"].ToString();
                                }
                                markupamnt = markupamnt == "" ? "0" : markupamnt;
                                TotalPenaltyamont = (Convert.ToDouble(penaltyamount) + Convert.ToDouble(markupamnt)).ToString();

                                Array_Book[penaltyamt] = TotalPenaltyamont;
                                Array_Book[refundamount] = "0";
                                Array_Book[Cancelationtype] = "1";
                                return Json(new { Status = "", Message = "", Result = Array_Book });
                            }
                            else
                            {
                                Array_Book[Cancelationtype] = "0";
                                Array_Book[error] = Offlinecancellationrequest(PaxDetails, Ticketdetails, PaxcancelationStatus, Ticketcancelstatus, Remarks);
                                return Json(new { Status = "", Message = "", Result = Array_Book });
                            }
                        }
                        else
                        {
                            Array_Book[Cancelationtype] = "0";
                            Array_Book[error] = Offlinecancellationrequest(PaxDetails, Ticketdetails, PaxcancelationStatus, Ticketcancelstatus, Remarks);
                            return Json(new { Status = "", Message = "", Result = Array_Book });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Array_Book[error] = "Problem occured in cancellation request ";
                if (strTKTFLAG == "QTKT")
                {
                    DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "X", "Availpage", "Cancelrequest", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                else
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "Availpage", "Cancelrequest", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
            }

            return Json(new { Status = "", Message = "", Result = Array_Book });
        }


        public string Offlinecancellationrequest(string PaxDetails, string Ticketdetails, string PaxcancelationStatus, string Ticketcancelstatus, string Remarks)
        {
            string res_string = string.Empty;
            DataSet dsViewPNR = new DataSet();
            Rays_service.RaysService _raysservice = new Rays_service.RaysService();
            string departureDate = string.Empty;
            string strErrorMsg = string.Empty;
            bool result = false;
            string refError = string.Empty;
            DataSet dsFareDispDet = new DataSet();
            string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"].ToString() != "") ? Session["TKTFLAG"].ToString() : "DTKT";
            try
            {
                string strAgentID = Session["POS_ID"].ToString();//"TASHA0400001";
                string strTerminalId = Session["POS_TID"].ToString();//"TASHA040000102";
                string strUserName = Session["username"].ToString();//"mohammed";
                string sequnceID = Session["sequenceid"].ToString();
                string Ipaddress = Session["ipAddress"].ToString();
                string getprevilage = Session["privilage"].ToString();
                string getAgentType = Session["agenttype"].ToString();

                string Spnr = PaxDetails.TrimEnd('|').Trim().Split('|')[0].Split(',')[1].ToString();
                #region SERVICE URL BRANCH BASED -- STS195
                string strClientBranchID = Session["ClientBranchID"] != null && Session["ClientBranchID"].ToString() != "" ? Session["ClientBranchID"].ToString() : "";
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (strClientBranchID != "" && strBranchCredit.Contains(strClientBranchID)))
                    {
                        _raysservice.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                    }
                    else
                    {
                        _raysservice.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    }
                }
                else
                {
                    _raysservice.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                }
                #endregion
                byte[] dsViewPNR_compress = new byte[] { };
                byte[] dsFareDispDet_compress = new byte[] { };
                if (terminalType == "T")
                {
                    strAgentID = string.Empty;
                }

                result = _raysservice.Fetch_PNR_Verification_Details_NewByte(strAgentID, Spnr, "", "", "",
                          strTerminalId, strUserName, Ipaddress, "W", Convert.ToDecimal(sequnceID), ref dsViewPNR_compress,
                      ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "Offlinecancellation", "OfflinecancellationRequest", getAgentType, strTerminalId);


                dsViewPNR = Base.Decompress(dsViewPNR_compress);
                dsFareDispDet = Base.Decompress(dsFareDispDet_compress);

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

                if (strTKTFLAG == "QTKT")
                {
                    DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "T", "Offlinecancellation", "Offlinecancellation-Input", Logdetais, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                else
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "T", "Offlinecancellation", "Offlinecancellation-Input", Logdetais, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                #endregion

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

                DateTime dtToday = TodayDateinTimeZone();
                TimeSpan span = dtDepartureDate.Subtract(dtToday);
                double totHours = span.TotalHours;
                if (totHours <= 0)
                {
                    res_string = "PNR already departured cancellation not allowed";
                }

                string strSeqNo = string.Empty;
                string output = string.Empty;
                string request = string.Empty;

                strErrorMsg = string.Empty;

                bool cancellationflag = false;

                output = _raysservice.Fetch_UpdateDetails(dsSequenceNo, strAgentID, strTerminalId, strUserName,
                                Ipaddress, "W", Convert.ToDecimal(sequnceID), ref strSeqNo, ref strErrorMsg, "OfflineCancellation", "Cancelation Request", Session["agenttype"].ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), ref dsPanalty, ref request, ref cancellationflag);

                #region Log
                StringWriter strWriter = new StringWriter();
                dsSequenceNo.WriteXml(strWriter);
                string LstrDetails = "<CANCELATION><DETAILS>" + strWriter.ToString() + "</DETAILS><OUTPUT>" + output.ToString() +
                   "</OUTPUT></CANCELATION>";
                if (strTKTFLAG == "QTKT")
                {
                    DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "T", "Offlinecancellation", "Offlinecancellation-Input", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                else
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "T", "Offlinecancellation", "Offlinecancellation-Input", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                #endregion

                if (output == "1")
                {
                    res_string = strErrorMsg;
                }
                else if (output == "0")
                {
                    res_string = (string.IsNullOrEmpty(strErrorMsg) ? "Unable to Process your request" : strErrorMsg);
                }
            }
            catch (Exception ex)
            {
                res_string = "Problem occured in cancellation request ";
                if (strTKTFLAG == "QTKT")
                {
                    DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "X", "Offlinecancellation", "Offlinecancellationrequest", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                else
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "Offlinecancellation", "Offlinecancellationrequest", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
            }
            return res_string;
        }

        public ActionResult Onlinecancellationconfirm(string SPNR, string Airlinepenalty, string Refundamt, string Cancelflag, string Remarks, string Airpnr, string CRSPNR)
        {
            ArrayList arrlst = new ArrayList();
            RaysService _RaysService = new RaysService();
            

            string strResult = string.Empty;
            string departureDate = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            arrlst.Add("");
            arrlst.Add("");
            arrlst.Add("");
            int error = 0;
            int cancelres = 1;
            int cancelreq = 2;
            DataSet dsViewPNR = new DataSet();
            DataSet dsFareDispDet = new DataSet();

            string refError = "ERROR";
            string strErrorMsg = "ERROR";
            string strAgentID = Session["POS_ID"].ToString();//"TASHA0400001";
            string strTerminalId = Session["POS_TID"].ToString();//"TASHA040000102";
            string strUserName = Session["username"].ToString();//"mohammed";
            string sequnceID = Session["sequenceid"].ToString();
            string Ipaddress = Session["ipAddress"].ToString();
            string getprevilage = Session["privilage"].ToString();
            string getAgentType = Session["agenttype"].ToString();
            string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"] != "") ? Session["TKTFLAG"].ToString() : "";
            bool result = false;
            bool response = false;

            string cancelresponse = string.Empty;

            try
            {
                #region
                string ConsoleAgent = ConfigurationManager.AppSettings["ConsoleAgent"].ToString();// "";
                if (getprevilage.ToString() == "S")
                {
                    strTerminalId = string.Empty;
                }
                else
                {
                    strTerminalId = Session["terminalid"].ToString();
                }
                if (getAgentType == ConsoleAgent)
                {
                    strAgentID = string.Empty;
                    strTerminalId = string.Empty;
                }

                #endregion

                #region SERVICE URL BRANCH BASED -- STS195
                string strClientBranchID = Session["ClientBranchID"] != null && Session["ClientBranchID"].ToString() != "" ? Session["ClientBranchID"].ToString() : "";
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (strClientBranchID != "" && strBranchCredit.Contains(strClientBranchID)))
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                    }
                    else
                    {
                        _RaysService.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    }
                }
                else
                {
                    _RaysService.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                }
                #endregion
                if (Cancelflag == "YES") // Confirm cancellation
                {
                    byte[] dsViewPNR_compress = new byte[] { };
                    byte[] dsFareDispDet_compress = new byte[] { };

                    if (terminalType == "T")
                    {
                        strAgentID = string.Empty;
                    }
                    result = _RaysService.Fetch_PNR_Verification_Details_NewByte(strAgentID, SPNR, Airpnr, CRSPNR, "",
                                   strTerminalId, strUserName, Ipaddress, "W", Convert.ToDecimal(sequnceID), ref dsViewPNR_compress,
                               ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "OnlineCancellation", "OnlineCancellationPNR", getAgentType, strTerminalId);

                    dsViewPNR = Base.Decompress(dsViewPNR_compress);
                    dsFareDispDet = Base.Decompress(dsFareDispDet_compress);

                    decimal Splitrefund = 0;
                    decimal Splitpenalty = 0;

                    if (dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                    {
                        double Adt_count = Convert.ToDouble(string.IsNullOrEmpty(dsViewPNR.Tables[0].Rows[0]["ADULTCOUNT"].ToString()) ? "1" : dsViewPNR.Tables[0].Rows[0]["ADULTCOUNT"].ToString());
                        double Chd_count = Convert.ToDouble(string.IsNullOrEmpty(dsViewPNR.Tables[0].Rows[0]["CHILDCOUNT"].ToString()) ? "0" : dsViewPNR.Tables[0].Rows[0]["CHILDCOUNT"].ToString());
                        double Inf_count = Convert.ToDouble(string.IsNullOrEmpty(dsViewPNR.Tables[0].Rows[0]["INFANTCOUNT"].ToString()) ? "0" : dsViewPNR.Tables[0].Rows[0]["INFANTCOUNT"].ToString());

                        double tot_pax = Adt_count + Chd_count + Inf_count;

                        DataTable dtTcktDetcancel = new DataTable("FAREDETAILS");
                        dtTcktDetcancel.Columns.Add("REFUNDAMOUNT");
                        dtTcktDetcancel.Columns.Add("CANCELCHARGEAMOUNT");
                        dtTcktDetcancel.Columns.Add("COMMISSIONPLBAMOUNT");
                        dtTcktDetcancel.Columns.Add("PENALITYAMOUNT");
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


                        for (int i = 0; i < dsViewPNR.Tables[0].Rows.Count; i++)
                        {
                            DataSet DsPenality = new DataSet();
                            DsPenality = (DataSet)(Session["PENALITYDATA"]);

                            string strMarkup = "0";
                            string strTrip = Session["TRIPDATA"].ToString();
                            if (DsPenality != null && DsPenality.Tables.Count > 2 && DsPenality.Tables[2].Rows.Count > 0 && DsPenality.Tables[2].Columns.Contains("CPR_MARKUP"))
                            {
                                if (DsPenality.Tables[2].Rows[0]["CPR_MARKUP_BASED"].ToString() == "PPS")
                                    strMarkup = DsPenality.Tables[2].Rows[0]["CPR_MARKUP"].ToString();
                                else if (DsPenality.Tables[2].Rows[0]["CPR_MARKUP_BASED"].ToString() == "PPT")
                                    strMarkup = dsViewPNR.Tables[0].Rows[i]["SEGMENT_NO"].ToString() == "1" ? DsPenality.Tables[2].Rows[0]["CPR_MARKUP"].ToString() : "0";
                                else if (DsPenality.Tables[2].Rows[0]["CPR_MARKUP_BASED"].ToString() == "PPI")
                                    strMarkup = strTrip.Contains(dsViewPNR.Tables[0].Rows[i]["SEGMENT_NO"].ToString()) ? DsPenality.Tables[2].Rows[0]["CPR_MARKUP"].ToString() : "0";
                            }
                            if (DsPenality != null && DsPenality.Tables.Count > 0 && DsPenality.Tables[0].Rows.Count > 0)
                            {
                                Splitpenalty = Convert.ToDecimal(DsPenality.Tables[0].Rows[0]["PenaltyAmount"].ToString()) / Convert.ToDecimal(tot_pax);
                                Splitrefund = Convert.ToDecimal(DsPenality.Tables[0].Rows[0]["RefundAmount"].ToString()) / Convert.ToDecimal(tot_pax);
                            }

                            dtTcktDetcancel.Rows.Add((dsViewPNR.Tables[0].Rows[i]["SEGMENT_NO"].ToString() == "1" ? Splitrefund.ToString() : "0"), "0", "0", (dsViewPNR.Tables[0].Rows[i]["SEGMENT_NO"].ToString() == "1" ? Splitpenalty.ToString() : "0"), dsViewPNR.Tables[0].Rows[i]["SEGMENT_NO"].ToString(), dsViewPNR.Tables[0].Rows[i]["PAX_REF_NO"].ToString(),
                                      dsViewPNR.Tables[0].Rows[i]["PASSENGER_NAME"].ToString(), dsViewPNR.Tables[0].Rows[i]["AGENT_ID"].ToString(), dsViewPNR.Tables[0].Rows[i]["TERMINAL_ID"].ToString(), dsViewPNR.Tables[0].Rows[i]["CRS_PNR"].ToString(), dsViewPNR.Tables[0].Rows[i]["AIRLINE_PNR"].ToString(), dsViewPNR.Tables[0].Rows[i]["PAYMENT_MODE"].ToString(),
                                      dsViewPNR.Tables[0].Rows[i]["PLATING_CARRIER"].ToString(), dsViewPNR.Tables[0].Rows[i]["AIRLINES"].ToString(), dsViewPNR.Tables[0].Rows[i]["ORIGINCODE"].ToString(), dsViewPNR.Tables[0].Rows[i]["DESTINATIONCODE"].ToString(), dsViewPNR.Tables[0].Rows[i]["TRIP_DESC"].ToString(),
                                      dsViewPNR.Tables[0].Rows[i]["CRS_PNR"].ToString(), dsViewPNR.Tables[0].Rows[i]["AIRLINE_PNR"].ToString(), "", strMarkup, "0", dsViewPNR.Tables[0].Rows[i]["AGENT_ID"].ToString(), dsViewPNR.Tables[0].Rows[i]["TAI_ISSUING_BRANCH_ID"].ToString(),
                                      dsViewPNR.Tables[0].Rows[i]["PAYMENT_MODE"].ToString(), "", dsViewPNR.Tables[0].Rows[i]["TERMINAL_ID"].ToString(), "OWNSTOCK", dsViewPNR.Tables[0].Rows[i]["AGENT_ID"].ToString(), "O");
                        }
                        DataSet dsfare = new DataSet();
                        dsfare.Tables.Add(dtTcktDetcancel.Copy());

                        string[] PAX = (from pax in dsViewPNR.Tables[0].AsEnumerable()
                                        select pax["PAX_REF_NO"].ToString()).Distinct().ToArray();
                        DataTable dtPNR = new DataTable("NewPNRS");
                        dtPNR.Columns.Add("PNR");
                        dtPNR.Columns.Add("PAXNO");

                        var ppvarPaxRef = from q in dsViewPNR.Tables[0].AsEnumerable()
                                          where q["PAX_REF_NO"].ToString() == PAX[0].ToString()
                                          select q;
                        DataView dv = new DataView();
                        DataTable dtTriptype = new DataTable();
                        dv = ppvarPaxRef.AsDataView();
                        if (dv != null && dv.Count > 0)
                        {
                            dtTriptype = ppvarPaxRef.CopyToDataTable();
                            dtPNR.Rows.Add(dtTriptype.Rows[0]["S_PNR"].ToString(), string.Join(",", PAX));
                        }
                        DataSet dspnr = new DataSet();
                        DataSet dsssrpnr = new DataSet();
                        DataTable dtssrpnr = new DataTable();
                        dspnr.Tables.Add(dtPNR.Copy());
                        dsssrpnr.Tables.Add(dtssrpnr.Copy());

                        Byte[] picbyte = Base.Utilities.ConvertDataSetToByteArray(dsfare);
                        Byte[] picbyte1 = Base.Utilities.ConvertDataSetToByteArray(dspnr);
                        Byte[] picbyte2 = Base.Utilities.ConvertDataSetToByteArray(dsssrpnr);

                        StringWriter strWriter1 = new StringWriter();
                        dsfare.WriteXml(strWriter1);
                        StringWriter strWriter2 = new StringWriter();
                        dspnr.WriteXml(strWriter2);
                        StringWriter strWriter3 = new StringWriter();
                        dsssrpnr.WriteXml(strWriter3);

                        bool resultflag = true;
                        #region RequestLog
                        string ReqTime = "ONLINECANCELLATIONREQTIME:" + DateTime.Now;
                        string xmldata = "<EVENT><REQUEST>CANCELLATIONDETAILS_Update_Cancellation_Status_new_BOA</REQUEST>" +
                                         "<CANCELLATIONDATA1>" + strWriter1 + "</CANCELLATIONDATA1>" +
                                         "<CANCELLATIONDATA2>" + strWriter2 + "</CANCELLATIONDATA2>" +
                                         "<CANCELLATIONDATA3>" + strWriter3 + "</CANCELLATIONDATA3>" +
                                         "<strtermonalid>" + Session["POS_TID"].ToString() + "</strtermonalid>" +
                                         "<REMARKS>" + Remarks + "</REMARKS>" +
                                         "<CANCELLAIONREQ>" + Session["Cancellationreq"].ToString() + "</CANCELLAIONREQ>" +
                                         "<RESULT>" + resultflag + "</RESULT>" +
                                         "<spnr>" + SPNR + "</spnr>" +
                                        "</EVENT>";
                        string LstrDetails = "<CANCELLATION_REQUEST><URL>[<![CDATA[" + ConfigurationManager.AppSettings["serviceuri"].ToString() + "]]>]</URL><QUERY>" + xmldata + "</QUERY><REQTIME>" + ReqTime + "</REQTIME></CANCELLATION_REQUEST>";
                        #endregion
                        if (strTKTFLAG == "QTKT")
                        {
                            DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "E", "OnlineCancellation", "Cancelrequest-Request", LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        }
                        else
                        {
                            DatabaseLog.LogData(Session["username"].ToString(), "E", "OnlineCancellation", "Cancelrequest-Request", LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        }
                        string strresult = _RaysService.Update_Cancellation_Status_new_BOA(Convert.ToBase64String(picbyte), Convert.ToBase64String(picbyte1), Session["POS_TID"].ToString(),
                            SPNR.ToString(), "F", "", "", "", "", "0", Remarks, "Update_Cancellation_Status_new_BOA", "OnlineCancellation-Confirm", 0, 0, Convert.ToBase64String(picbyte2), Session["Agenttype"].ToString(), string.IsNullOrEmpty(Session["Cancellationreq"].ToString()) ? "" : Session["Cancellationreq"].ToString(), resultflag);

                        #region RequestLog
                        ReqTime = "ONLINECANCELLATIONRESTIME:" + DateTime.Now;
                        xmldata = "<EVENT><RESPONSE>CANCELLATIONDETAILS_Update_Cancellation_Status_new_BOA</RESPONSE>" +
                                        "<CANCELLATIONDATA1>" + strWriter1 + "</CANCELLATIONDATA1>" +
                                        "<CANCELLATIONDATA2>" + strWriter2 + "</CANCELLATIONDATA2>" +
                                        "<strtermonalid>" + Session["POS_TID"].ToString() + "</strtermonalid>" +
                                        "<spnr>" + SPNR + "</spnr>" +
                                        "<strresult>" + strresult + "</strresult>" +
                                        "</EVENT>";
                        LstrDetails = "<CANCELLATION_RESPONSE><URL>[<![CDATA[" + ConfigurationManager.AppSettings["serviceuri"].ToString() + "]]>]</URL><QUERY>" + xmldata + "</QUERY><REQTIME>" + ReqTime + "</REQTIME></CANCELLATION_RESPONSE>";
                        #endregion
                        if (strTKTFLAG == "QTKT")
                        {
                            DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "E", "OnlineCancellation", "Cancelrequest-Response", LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        }
                        else
                        {
                            DatabaseLog.LogData(Session["username"].ToString(), "E", "OnlineCancellation", "Cancelrequest-Response", LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        }
                        if (strresult != "0" && resultflag == true)
                        {
                            arrlst[cancelres] = strresult.Equals("1") ? "PNR cancelled successfully" : strresult; //strresult.Contains("Ticket Cancelled Successfully") ? "PNR cancelled successfully" : strresult;
                            return Json(new { Status = "", Message = "", Result = arrlst });
                        }
                        else
                        {
                            arrlst[cancelres] = "PNR cancelled successfully, unable to process transaction please contact support team(#03)";
                            return Json(new { Status = "", Message = "", Result = arrlst });
                        }
                    }
                }
                else  // Cancel cancellation request
                {
                    string cancelseq = string.IsNullOrEmpty(Session["Cancelseqid"].ToString()) ? "" : Session["Cancelseqid"].ToString();
                    cancelresponse = _RaysService.UpdateCancelTicketStatus_BOA(SPNR, cancelseq, Remarks, Session["username"].ToString(), Session["ipAddress"].ToString(), Session["sequenceid"].ToString(), "AfterBookingController", "OnlineCancellationDet", Session["terminalid"].ToString(), "", "CANCEL");
                    string XML = "<CANCELREQUEST><SPNR>" + SPNR + "</SPNR><CANCELSEQ>" + cancelseq + "</CANCELSEQ><REMARKS>" + Remarks + "</REMARKS><USERNAME>" + Session["username"].ToString() + "</USERNAME><FLAG>CANCEL</FLAG></CANCELREQUEST>";
                    if (strTKTFLAG == "QTKT")
                    {
                        DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "E", "OnlineCancellation", "Requestcancellation", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    }
                    else
                    {
                        DatabaseLog.LogData(Session["username"].ToString(), "E", "OnlineCancellation", "Requestcancellation", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    }
                    arrlst[cancelres] = "PNR request cancelled successfully";
                    return Json(new { Status = "", Message = "", Result = arrlst });
                }
            }
            catch (Exception ex)
            {
                arrlst[error] = "Unable to cancel the pnr please contact support team";
                if (strTKTFLAG == "QTKT")
                {
                    DatabaseLog.Retrievetcktlog(Session["username"].ToString(), "X", "OnlineCancellation", "Cancelrequest-Error", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                else
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "OnlineCancellation", "Cancelrequest-Error", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
            }
            return Json(new { Status = "", Message = "", Result = arrlst });
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


        #endregion

    
        #region to ticket

        public ActionResult Toticket(string SPNR, string Page)
        {

            string strResult = string.Empty;
            string departureDate = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            // Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            // int response = 1;
            int FlightResponse = 1;
            int PaxResponse = 2;
            int GrossFareRespose = 3;
            // int FareRespose = 4;
            string Output = string.Empty;// "";
            string AgentID = string.Empty;// "TIQAJ0100001";
            string TerminalID = string.Empty;// "TIQAJ010000103";
            string UserName = string.Empty;// "sathees";
            string strErrorMsg = string.Empty;// "";
            string ipAddress = string.Empty;
            // string phonrno = string.Empty;// "9494387811";
            string sequenceId = string.Empty;

            string strError = string.Empty;
            string airlineCode = string.Empty;
            string airportID = string.Empty;
            string airCategory = string.Empty;
            string airName = string.Empty;
            DataSet dsBookFlight = new DataSet();
            DataSet dsViewPNR = new DataSet();
            DataSet dsFareDispDet = new DataSet();

            byte[] dsViewPNR_compress = new byte[] { };
            byte[] dsFareDispDet_compress = new byte[] { };

            if (Session["Availterminal"] == null)
            {
                Session.Add("Availterminal", Session["terminalid"].ToString());
            }

            string refError = string.Empty;
            try
            {

                object _obj = new object();
                _obj = Session["agentid"];
                if (_obj == null && Page == "FBP")
                {
                    return Json(new { Status = "-1", Message = Array_Book[error], Result = "", TicketJson = "" });
                }
                #region UsageLog
                string PageName = "To Ticket";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "UPDATE");
                }
                catch (Exception e) { }
                #endregion

                AgentID = (Session["Allowpoweruser"] != null && Session["Allowpoweruser"].ToString() != "") ? Session["POS_ID"].ToString() : Session["POS_ID"].ToString();
                TerminalID = (Session["Allowpoweruser"] != null && Session["Allowpoweruser"].ToString() != "") ? Session["POS_TID"].ToString() : Session["POS_TID"].ToString();
                UserName = Session["username"].ToString();
                ipAddress = Session["ipAddress"].ToString();
                sequenceId = Session["sequenceid"].ToString();

                string resultCode = string.Empty;
                string returnMsg = string.Empty;
                DataSet dsTicketPassengerDetails = new DataSet();

                bool result = false;

                #region Log Request

                StringWriter strREQUEST = new System.IO.StringWriter();

                string LstrDetails = "<TOTICKET_REQUEST>" + SPNR + " <URL>[<![CDATA[" + Session["travraysws_url"] + "/" + Session["travraysws_vdir"] + "/Rays.svc/"
                + "]]>]</URL><REQUEST_TIME>" + DateTime.Now.TimeOfDay.ToString().Replace(':', '_') + "</REQUEST_TIME></TOTICKET_REQUEST>";


                DatabaseLog.LogData(Session["username"].ToString(), "B", "Booked History", "Booked History -> TO TICKET -> Request", LstrDetails + strREQUEST.ToString() + "~" + Page, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["terminalid"].ToString() + ")", Session["sequenceid"].ToString());

                #endregion
                // BlockPNRTicketing = false;
                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                string ReqTime = DateTime.Now.ToString("yyyyMMdd");

                result = _rays_servers.Fetch_PNR_Verification_Details_NewByte("", SPNR, "", "", "",
                            TerminalID, UserName, ipAddress, "W", Convert.ToDecimal(sequenceId), ref dsViewPNR_compress,
                        ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "Report", "RequestViewPNRFunction", Session["agenttype"].ToString(), Session["POS_TID"].ToString());

                if (dsViewPNR_compress != null)
                {
                    dsViewPNR = Base.Decompress(dsViewPNR_compress);
                }
                if (dsFareDispDet_compress != null)
                {
                    dsFareDispDet = Base.Decompress(dsFareDispDet_compress);
                }


                dsTicketPassengerDetails = _rays_servers.Fetch_and_Ticketing_BlockPnr_Details(SPNR.ToString(), AgentID, TerminalID,
              UserName, ipAddress, "W", Convert.ToDecimal(sequenceId), ref returnMsg,
              "BookedHistory", "BookedHistory -> TO TICKET", "B", ConfigurationManager.AppSettings["ProjectCode"].ToString());

                #region Log Response

                strREQUEST = new System.IO.StringWriter();



                DateTime.Now.TimeOfDay.ToString().Replace(':', '_');

                LstrDetails = "<TOTICKET>" + returnMsg + " <URL>[<![CDATA[" + Session["travraysws_url"] + "/" + Session["travraysws_vdir"] + "/Rays.svc/"
                + "]]>]</URL><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME></TOTICKET>";


                DatabaseLog.LogData(Session["username"].ToString(), "B", "Booked History", "Booked History -> TO TICKET -> Response", LstrDetails + strREQUEST.ToString() + "~" + Page, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                #endregion

                if (string.IsNullOrEmpty(returnMsg))
                {

                    Array_Book[error] = "Unable to Book the ticket.please contact customer care";
                    ViewBag.toticket_response = "Unable to Book the ticket.please contact customer care";
                    if (Page == "FBP")
                    {
                        return Json(new { Status = "0", Message = Array_Book[error], Result = "", TicketJson = "" });
                    }
                    return PartialView("_Ticketingsuccess", "");
                }
                else
                {
                    //TravRaysWebApp.Rays_service.RaysService _rays_servers = new TravRaysWebApp.Rays_service.RaysService();
                    ViewBag.bookingresponse = returnMsg;
                    var RetVal = JsonConvert.DeserializeObject<RQRS.BookingRS>(returnMsg);
                    string RetVale = JsonConvert.SerializeObject(RetVal);
                    dsBookFlight = Serv.convertJsonStringToDataSet(RetVale, "");

                    if (RetVale != null)
                    {
                        #region Log

                        DateTime.Now.TimeOfDay.ToString().Replace(':', '_');

                        string LogrDetail = "<TOTICKET><RESPONSE DETAILS>" + dsBookFlight.GetXml() + "</RESPONSE DETAILS> <URL>[<![CDATA[" + Session["travraysws_url"] + "/" + Session["travraysws_vdir"] + "/Rays.svc/"
                        + "]]>]</URL><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME></TOTICKET>";


                        DatabaseLog.LogData(Session["username"].ToString(), "B", "Booked History", "Booked History -> TO TICKET -> Response ->After DeserializeObject ", LogrDetail, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                        #endregion

                        string result1 = RetVal.ResultCode; //dsBookFlight.Tables["rootNode"].Rows[0]["BRC"].ToString();

                        ViewBag.toticket_resultcode = result1;
                        Array_Book[error] = result1;



                        string triptype = (dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0 && dsViewPNR.Tables[0].Rows[0]["TRIP_DESC"].ToString() != "" ? dsViewPNR.Tables[0].Rows[0]["TRIP_DESC"].ToString() : "O");

                        if (result1 == "1")
                        {



                            var Tkt = from Ticket in RetVal.PnrDetails.AsEnumerable()
                                      group Ticket by Ticket.ITINREF
                                          into Ftkt
                                      select new
                                      {
                                          //     TITLE = Ftkt.First().TITLE,
                                          SPNR = Ftkt.First().SPNR,
                                          CRSPNR = Ftkt.First().CRSPNR,
                                          ARILINEPNR = Ftkt.First().AIRLINEPNR,
                                          FLIGHTNO = Ftkt.First().FLIGHTNO,
                                          AIRLINECODE = Ftkt.First().AIRLINECODE,
                                          CLASS = Ftkt.First().CLASS,
                                          ORIGIN = Ftkt.First().ORIGIN,
                                          DESTINATION = Ftkt.First().DESTINATION,
                                          DEPARTUREDATE = Ftkt.First().DEPARTUREDATE,
                                          ARRIVALDATE = Ftkt.First().ARRIVALDATE


                                      };

                            ViewBag.toticketpnr_details_1 = JsonConvert.SerializeObject(Tkt);
                            Array_Book[FlightResponse] = JsonConvert.SerializeObject(Tkt);



                            string TotalFare = "";

                            if (triptype.ToUpper() == "R")
                            {

                                int index = 0;
                                string lstrpax = "";
                                var Paxdet = from FPass in RetVal.PnrDetails.AsEnumerable()
                                             where ((index >= (RetVal.PnrDetails.Count / 2) ? false : true) &&
                                             (lstrpax.Contains(FPass.SEQNO) ? false : true))
                                             select new
                                             {
                                                 TITLE = FPass.TITLE,
                                                 First = FPass.FIRSTNAME,
                                                 LASTNAME = FPass.LASTNAME,
                                                 PAXTYPE = FPass.PAXTYPE,
                                                 DATEOFBIRTH = FPass.DATEOFBIRTH,
                                                 TICKETNO = FPass.TICKETNO,
                                                 USERTRACKID = FPass.USERTRACKID,
                                                 //GROSSFARE = FPass.Select(Grss => Convert.ToDouble(Grss.GROSSFARE)).ToArray().Sum().ToString("0.00"),
                                                 GROSSFARE = (Base.ServiceUtility.CovertToDouble((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")),
                                                 ServiceCharge = ((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"),
                                                 markup = FPass.MARKUP,
                                                 test = index++,
                                                 pax = faresplit(FPass.SEQNO, ref lstrpax)
                                             };
                                ViewBag.toticketpnr_details_2 = JsonConvert.SerializeObject(Paxdet);
                                Array_Book[PaxResponse] = JsonConvert.SerializeObject(Paxdet);

                                lstrpax = "";
                                index = 0;
                                var PaxFare = from FPass in RetVal.PnrDetails.AsEnumerable()
                                              where ((index >= (RetVal.PnrDetails.Count / 2) ? false : true) &&
                                          (lstrpax.Contains(FPass.SEQNO) ? false : true))
                                              select new
                                              {
                                                  GROSSFARE = ((Base.ServiceUtility.CovertToDouble((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                       + Base.ServiceUtility.CovertToDouble((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0"))),
                                                  pax = faresplit(FPass.SEQNO, ref lstrpax),
                                                  index = index++
                                              };

                                TotalFare = PaxFare.Select(Grss => Convert.ToDouble(Grss.GROSSFARE)).ToArray().Sum().ToString();

                                //Array_Book[FlightResponse] = JsonConvert.SerializeObject(Tkt);
                                Array_Book[GrossFareRespose] = JsonConvert.SerializeObject(TotalFare);
                                ViewBag.totalfare = JsonConvert.SerializeObject(TotalFare);



                            }
                            else
                            {
                                string lstrpax = "";


                                var Paxdet = from FPass in RetVal.PnrDetails.AsEnumerable()
                                             where ((lstrpax.Contains(FPass.SEQNO) ? false : true))
                                             select new
                                             {
                                                 TITLE = FPass.TITLE,
                                                 First = FPass.FIRSTNAME,
                                                 LASTNAME = FPass.LASTNAME,
                                                 PAXTYPE = FPass.PAXTYPE,
                                                 DATEOFBIRTH = FPass.DATEOFBIRTH,
                                                 TICKETNO = FPass.TICKETNO,
                                                 USERTRACKID = FPass.USERTRACKID,
                                                 //GROSSFARE = FPass.Select(Grss => Convert.ToDouble(Grss.GROSSFARE)).ToArray().Sum().ToString("0.00"),
                                                 GROSSFARE = ((Base.ServiceUtility.CovertToDouble((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0"))),
                                                 ServiceCharge = ((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"),
                                                 markup = FPass.MARKUP,
                                                 pax = faresplit(FPass.SEQNO, ref lstrpax)

                                             };
                                ViewBag.toticketpnr_details_2 = JsonConvert.SerializeObject(Paxdet);
                                Array_Book[PaxResponse] = JsonConvert.SerializeObject(Paxdet);

                                lstrpax = "";
                                var PaxFare = from FPass in RetVal.PnrDetails.AsEnumerable()
                                              where ((lstrpax.Contains(FPass.SEQNO) ? false : true))
                                              select new
                                              {
                                                  GROSSFARE = ((Base.ServiceUtility.CovertToDouble((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                       + Base.ServiceUtility.CovertToDouble((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0"))),
                                                  pax = faresplit(FPass.SEQNO, ref lstrpax)
                                              };

                                TotalFare = PaxFare.Select(Grss => Convert.ToDouble(Grss.GROSSFARE)).ToArray().Sum().ToString();

                                ViewBag.totalfare = JsonConvert.SerializeObject(TotalFare);
                                Array_Book[GrossFareRespose] = JsonConvert.SerializeObject(TotalFare);
                            }
                            if (Page == "FBP")
                            {
                                return Json(new { Status = "1", Message = "", Result = "", TicketJson = ViewBag.toticketpnr_details_2 });
                            }


                        }
                        else
                        {
                            Array_Book[error] = string.IsNullOrEmpty(dsBookFlight.Tables["rootNode"].Rows[0]["BEr"].ToString()) ? "Unable to process your request. please contact customer care" : dsBookFlight.Tables["rootNode"].Rows[0]["BEr"].ToString();
                            if (Page == "FBP")
                            {
                                return Json(new { Status = "0", Message = Array_Book[error], Result = "", TicketJson = "" });

                            }

                            DatabaseLog.LogData(Session["username"].ToString(), "ER", "Booked History", "Booked History -> TO TICKET -> Response -> Result 0", dsBookFlight.Tables["rootNode"].Rows[0]["BEr"].ToString() + "~" + Page + "</BOOKING_RESPONSE>", Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                if (Page == "FBP")
                {
                    return Json(new { Status = "0", Message = "", Result = "", TicketJson = "" });
                }
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Booked History", "Booked History -> TO TICKET -> Error", ex.ToString() + "~" + Page, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

            }

            return PartialView("_Ticketingsuccess", "");
        }


        public ActionResult bookedhistoryToticket(string SPNR, string Page)
        {

            string strResult = string.Empty;
            string departureDate = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            // Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            // int response = 1;
            int FlightResponse = 1;
            int PaxResponse = 2;
            int GrossFareRespose = 3;
            // int FareRespose = 4;
            string Output = string.Empty;// "";
            string AgentID = string.Empty;// "TIQAJ0100001";
            string TerminalID = string.Empty;// "TIQAJ010000103";
            string UserName = string.Empty;// "sathees";
            string strErrorMsg = string.Empty;// "";
            string ipAddress = string.Empty;
            // string phonrno = string.Empty;// "9494387811";
            string sequenceId = string.Empty;

            string strError = string.Empty;
            string airlineCode = string.Empty;
            string airportID = string.Empty;
            string airCategory = string.Empty;
            string airName = string.Empty;
            DataSet dsBookFlight = new DataSet();
            DataSet dsViewPNR = new DataSet();
            DataSet dsFareDispDet = new DataSet();

            byte[] dsViewPNR_compress = new byte[] { };
            byte[] dsFareDispDet_compress = new byte[] { };

            if (Session["Availterminal"] == null)
            {
                Session.Add("Availterminal", Session["terminalid"].ToString());
            }

            string refError = string.Empty;
            try
            {

                object _obj = new object();
                _obj = Session["agentid"];
                if (_obj == null && Page == "FBP")
                {
                    return Json(new { Status = "-1", Message = Array_Book[error], Result = "", TicketJson = "" });
                }
                #region UsageLog
                string PageName = "To Ticket";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "UPDATE");
                }
                catch (Exception e) { }
                #endregion

                AgentID = (Session["Allowpoweruser"] != null && Session["Allowpoweruser"].ToString() != "") ? Session["POS_ID"].ToString() : Session["POS_ID"].ToString();
                TerminalID = (Session["Allowpoweruser"] != null && Session["Allowpoweruser"].ToString() != "") ? Session["POS_TID"].ToString() : Session["POS_TID"].ToString();
                UserName = Session["username"].ToString();
                ipAddress = Session["ipAddress"].ToString();
                sequenceId = Session["sequenceid"].ToString();

                string resultCode = string.Empty;
                string returnMsg = string.Empty;
                DataSet dsTicketPassengerDetails = new DataSet();

                bool result = false;

                #region Log Request

                StringWriter strREQUEST = new System.IO.StringWriter();

                string LstrDetails = "<TOTICKET_REQUEST>" + SPNR + " <URL>[<![CDATA[" + Session["travraysws_url"] + "/" + Session["travraysws_vdir"] + "/Rays.svc/"
                + "]]>]</URL><REQUEST_TIME>" + DateTime.Now.TimeOfDay.ToString().Replace(':', '_') + "</REQUEST_TIME></TOTICKET_REQUEST>";


                DatabaseLog.LogData(Session["username"].ToString(), "B", "Booked History", "Booked History -> TO TICKET -> Request", LstrDetails + strREQUEST.ToString() + "~" + Page, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["terminalid"].ToString() + ")", Session["sequenceid"].ToString());

                #endregion
                // BlockPNRTicketing = false;
                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                string ReqTime = DateTime.Now.ToString("yyyyMMdd");

                result = _rays_servers.Fetch_PNR_Verification_Details_NewByte("", SPNR, "", "", "",
                            TerminalID, UserName, ipAddress, "W", Convert.ToDecimal(sequenceId), ref dsViewPNR_compress,
                        ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "Report", "RequestViewPNRFunction", Session["agenttype"].ToString(), Session["POS_TID"].ToString());

                if (dsViewPNR_compress != null)
                {
                    dsViewPNR = Base.Decompress(dsViewPNR_compress);
                }
                if (dsFareDispDet_compress != null)
                {
                    dsFareDispDet = Base.Decompress(dsFareDispDet_compress);
                }


                dsTicketPassengerDetails = _rays_servers.Fetch_and_Ticketing_BlockPnr_Details(SPNR.ToString(), AgentID, TerminalID,
              UserName, ipAddress, "W", Convert.ToDecimal(sequenceId), ref returnMsg,
              "BookedHistory", "BookedHistory -> TO TICKET", "B", ConfigurationManager.AppSettings["ProjectCode"].ToString());

                #region Log Response

                strREQUEST = new System.IO.StringWriter();

                DateTime.Now.TimeOfDay.ToString().Replace(':', '_');

                LstrDetails = "<TOTICKET>" + returnMsg + " <URL>[<![CDATA[" + Session["travraysws_url"] + "/" + Session["travraysws_vdir"] + "/Rays.svc/"
                + "]]>]</URL><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME></TOTICKET>";


                DatabaseLog.LogData(Session["username"].ToString(), "B", "Booked History", "Booked History -> TO TICKET -> Response", LstrDetails + strREQUEST.ToString() + "~" + Page, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                #endregion

                if (string.IsNullOrEmpty(returnMsg))
                {
                    Array_Book[error] = "Unable to Book the ticket.please contact customer care";
                    ViewBag.toticket_response = "Unable to Book the ticket.please contact customer care";
                    return Json(new { Status = "0", Message = Array_Book[error], Result = "", TicketJson = "" });
                }
                else
                {
                    //TravRaysWebApp.Rays_service.RaysService _rays_servers = new TravRaysWebApp.Rays_service.RaysService();
                    ViewBag.bookingresponse = returnMsg;
                    var RetVal = JsonConvert.DeserializeObject<RQRS.BookingRS>(returnMsg);
                    string RetVale = JsonConvert.SerializeObject(RetVal);
                    dsBookFlight = Serv.convertJsonStringToDataSet(RetVale, "");

                    if (RetVale != null)
                    {
                        #region Log

                        DateTime.Now.TimeOfDay.ToString().Replace(':', '_');

                        string LogrDetail = "<TOTICKET><RESPONSE DETAILS>" + dsBookFlight.GetXml() + "</RESPONSE DETAILS> <URL>[<![CDATA[" + Session["travraysws_url"] + "/" + Session["travraysws_vdir"] + "/Rays.svc/"
                        + "]]>]</URL><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME></TOTICKET>";


                        DatabaseLog.LogData(Session["username"].ToString(), "B", "Booked History", "Booked History -> TO TICKET -> Response ->After DeserializeObject ", LogrDetail, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                        #endregion

                        string result1 = RetVal.ResultCode; //dsBookFlight.Tables["rootNode"].Rows[0]["BRC"].ToString();

                        ViewBag.toticket_resultcode = result1;
                        Array_Book[error] = result1;



                        string triptype = (dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0 && dsViewPNR.Tables[0].Rows[0]["TRIP_DESC"].ToString() != "" ? dsViewPNR.Tables[0].Rows[0]["TRIP_DESC"].ToString() : "O");

                        if (result1 == "1")
                        {
                            var Tkt = from Ticket in RetVal.PnrDetails.AsEnumerable()
                                      group Ticket by Ticket.ITINREF
                                          into Ftkt
                                      select new
                                      {
                                          //     TITLE = Ftkt.First().TITLE,
                                          SPNR = Ftkt.First().SPNR,
                                          CRSPNR = Ftkt.First().CRSPNR,
                                          ARILINEPNR = Ftkt.First().AIRLINEPNR,
                                          FLIGHTNO = Ftkt.First().FLIGHTNO,
                                          AIRLINECODE = Ftkt.First().AIRLINECODE,
                                          CLASS = Ftkt.First().CLASS,
                                          ORIGIN = Ftkt.First().ORIGIN,
                                          DESTINATION = Ftkt.First().DESTINATION,
                                          DEPARTUREDATE = Ftkt.First().DEPARTUREDATE,
                                          ARRIVALDATE = Ftkt.First().ARRIVALDATE


                                      };

                            ViewBag.toticketpnr_details_1 = JsonConvert.SerializeObject(Tkt);
                            Array_Book[FlightResponse] = JsonConvert.SerializeObject(Tkt);



                            string TotalFare = "";

                            if (triptype.ToUpper() == "R")
                            {

                                int index = 0;
                                string lstrpax = "";
                                var Paxdet = from FPass in RetVal.PnrDetails.AsEnumerable()
                                             where ((index >= (RetVal.PnrDetails.Count / 2) ? false : true) &&
                                             (lstrpax.Contains(FPass.SEQNO) ? false : true))
                                             select new
                                             {
                                                 TITLE = FPass.TITLE,
                                                 First = FPass.FIRSTNAME,
                                                 LASTNAME = FPass.LASTNAME,
                                                 PAXTYPE = FPass.PAXTYPE,
                                                 DATEOFBIRTH = FPass.DATEOFBIRTH,
                                                 TICKETNO = FPass.TICKETNO,
                                                 USERTRACKID = FPass.USERTRACKID,
                                                 //GROSSFARE = FPass.Select(Grss => Convert.ToDouble(Grss.GROSSFARE)).ToArray().Sum().ToString("0.00"),
                                                 GROSSFARE = (Base.ServiceUtility.CovertToDouble((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")),
                                                 ServiceCharge = ((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"),
                                                 markup = FPass.MARKUP,
                                                 test = index++,
                                                 pax = faresplit(FPass.SEQNO, ref lstrpax)
                                             };
                                ViewBag.toticketpnr_details_2 = JsonConvert.SerializeObject(Paxdet);
                                Array_Book[PaxResponse] = JsonConvert.SerializeObject(Paxdet);

                                lstrpax = "";
                                index = 0;
                                var PaxFare = from FPass in RetVal.PnrDetails.AsEnumerable()
                                              where ((index >= (RetVal.PnrDetails.Count / 2) ? false : true) &&
                                          (lstrpax.Contains(FPass.SEQNO) ? false : true))
                                              select new
                                              {
                                                  GROSSFARE = ((Base.ServiceUtility.CovertToDouble((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                       + Base.ServiceUtility.CovertToDouble((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0"))),
                                                  pax = faresplit(FPass.SEQNO, ref lstrpax),
                                                  index = index++
                                              };

                                TotalFare = PaxFare.Select(Grss => Convert.ToDouble(Grss.GROSSFARE)).ToArray().Sum().ToString();

                                //Array_Book[FlightResponse] = JsonConvert.SerializeObject(Tkt);
                                Array_Book[GrossFareRespose] = JsonConvert.SerializeObject(TotalFare);
                                ViewBag.totalfare = JsonConvert.SerializeObject(TotalFare);



                            }
                            else
                            {
                                string lstrpax = "";


                                var Paxdet = from FPass in RetVal.PnrDetails.AsEnumerable()
                                             where ((lstrpax.Contains(FPass.SEQNO) ? false : true))
                                             select new
                                             {
                                                 TITLE = FPass.TITLE,
                                                 First = FPass.FIRSTNAME,
                                                 LASTNAME = FPass.LASTNAME,
                                                 PAXTYPE = FPass.PAXTYPE,
                                                 DATEOFBIRTH = FPass.DATEOFBIRTH,
                                                 TICKETNO = FPass.TICKETNO,
                                                 USERTRACKID = FPass.USERTRACKID,
                                                 //GROSSFARE = FPass.Select(Grss => Convert.ToDouble(Grss.GROSSFARE)).ToArray().Sum().ToString("0.00"),
                                                 GROSSFARE = ((Base.ServiceUtility.CovertToDouble((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                     + Base.ServiceUtility.CovertToDouble((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0"))),
                                                 ServiceCharge = ((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"),
                                                 markup = FPass.MARKUP,
                                                 pax = faresplit(FPass.SEQNO, ref lstrpax)

                                             };
                                ViewBag.toticketpnr_details_2 = JsonConvert.SerializeObject(Paxdet);
                                Array_Book[PaxResponse] = JsonConvert.SerializeObject(Paxdet);

                                lstrpax = "";
                                var PaxFare = from FPass in RetVal.PnrDetails.AsEnumerable()
                                              where ((lstrpax.Contains(FPass.SEQNO) ? false : true))
                                              select new
                                              {
                                                  GROSSFARE = ((Base.ServiceUtility.CovertToDouble((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                      + Base.ServiceUtility.CovertToDouble((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                       + Base.ServiceUtility.CovertToDouble((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0"))),
                                                  pax = faresplit(FPass.SEQNO, ref lstrpax)
                                              };

                                TotalFare = PaxFare.Select(Grss => Convert.ToDouble(Grss.GROSSFARE)).ToArray().Sum().ToString();

                                ViewBag.totalfare = JsonConvert.SerializeObject(TotalFare);
                                Array_Book[GrossFareRespose] = JsonConvert.SerializeObject(TotalFare);
                            }
                            return Json(new { Status = "1", Message = "", Result = "", TicketJson = ViewBag.toticketpnr_details_2 });
                        }
                        else
                        {
                            Array_Book[error] = string.IsNullOrEmpty(dsBookFlight.Tables["rootNode"].Rows[0]["BEr"].ToString()) ? "Unable to process your request. please contact customer care" : dsBookFlight.Tables["rootNode"].Rows[0]["BEr"].ToString();
                            return Json(new { Status = "0", Message = Array_Book[error], Result = "", TicketJson = "" });
                            DatabaseLog.LogData(Session["username"].ToString(), "ER", "Booked History", "Booked History -> TO TICKET -> Response -> Result 0", dsBookFlight.Tables["rootNode"].Rows[0]["BEr"].ToString() + "~" + Page + "</BOOKING_RESPONSE>", Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Booked History", "Booked History -> TO TICKET -> Error", ex.ToString() + "~" + Page, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
            }

            return Json(new { Status = "0", Message = Array_Book[error], Result = "", TicketJson = "" });
        }


        private string faresplit(string lstrSeq, ref string lstrpax)
        {
            lstrpax = lstrpax + "|" + lstrSeq;
            return lstrSeq;
        }
        #endregion

        #region to cancel

        public ActionResult cancelticket(string SPNR)
        {

            string strResult = string.Empty;
            string departureDate = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            int response = 1;
            string Output = string.Empty;// "";
            string AgentID = string.Empty;// "TIQAJ0100001";
            string TerminalID = string.Empty;// "TIQAJ010000103";
            string UserName = string.Empty;// "sathees";
            string strErrorMsg = string.Empty;// "";
            string ipAddress = string.Empty;
            // string phonrno = string.Empty;// "9494387811";
            string sequenceId = string.Empty;

            string strError = string.Empty;
            string airlineCode = string.Empty;
            string airportID = string.Empty;
            string airCategory = string.Empty;
            string airName = string.Empty;
            string branch = string.Empty;


            try
            {
                AgentID = Session["POS_ID"].ToString();
                TerminalID = Session["POS_TID"].ToString();
                UserName = Session["username"].ToString();
                ipAddress = Session["ipAddress"].ToString();
                sequenceId = Session["sequenceid"].ToString();
                branch = Session["branchid"].ToString();

                string resultCode = string.Empty;
                string returnMsg = string.Empty;
                DataSet dsTicketPassengerDetails = new DataSet();

                // bool result = false;
                #region UsageLog
                string PageName = "To Cancel";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "UPDATE");
                }
                catch (Exception e) { }
                #endregion
                // BlockPNRTicketing = false;
                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                string ReqTime = DateTime.Now.ToString("yyyyMMddHHmm");

                //string returnMsg = string.Empty;
                bool result = _rays_servers.BlockedPNRToCancel(SPNR.ToString(), branch, branch, AgentID, TerminalID,
                     UserName, ipAddress, "W", Convert.ToDecimal(sequenceId), ref returnMsg, "BookedHistory", "bgwCancelPNR_DoWork");

                #region Log




                ReqTime = DateTime.Now.TimeOfDay.ToString().Replace(':', '_');

                string LstrDetails = "<CANCELTICKET>" + result + " <URL>[<![CDATA[" + Session["travraysws_url"] + "/" + Session["travraysws_vdir"] + "/Rays.svc/"
                + "]]>]</URL><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME></CANCELTICKET>";


                DatabaseLog.LogData(Session["username"].ToString(), "B", "Booked History", "CANCELTICKET", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                #endregion

                if (result == true)
                {
                    if (!string.IsNullOrEmpty(returnMsg.Trim()))
                    {
                        Array_Book[response] = returnMsg;
                    }
                    else
                    {
                        Array_Book[response] = "Ticket Cancelled Sucessfully";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(returnMsg.Trim()))
                    {
                        Array_Book[error] = returnMsg;
                    }
                    else
                    {
                        Array_Book[error] = "Unable process cancel request. Please contact cusomer care "; //returnMsg.ToString().Trim();
                    }
                }

            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Booked History", "CANCELTICKET_Request", ex.Message.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

            }
            return Json(new { Status = "", Message = "", Result = Array_Book });
        }
        #endregion

        #region NOSHOW

        public ActionResult FetchNoShowRefundDetails(string strFromDate, string strToDate, string strRiyaPNR, string strStatus, string strAirportID)
        {
          
            RaysService _RaysService = new RaysService();
            string Status = string.Empty;
            string ErrorMessage = string.Empty;
            string strResult = string.Empty;
            string strRefundDetails = string.Empty;
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_ID"].ToString() : "";
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strsequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0";
            string strIpaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            bool Output = false;
            DataSet dsNoshow = new DataSet();
            try
            {

                if (strAgentID == "")
                {
                    Status = "0";
                    return Json(new { Status = Status, Message = ErrorMessage, Result = strResult });
                }

                strFromDate = DateTime.ParseExact(strFromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                strToDate = DateTime.ParseExact(strToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");

                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                Output = _RaysService.FetchNoShowRefundDetails(strAgentID, strTerminalId, strUserName, strTerminalType, strIpaddress,
                               Convert.ToDecimal(strsequnceID), strRiyaPNR, strFromDate, strToDate, strStatus, strAirportID, ref ErrorMessage, ref strRefundDetails);

                if (Output == true && strRefundDetails != "")
                {
                    Status = "01";
                    ErrorMessage = "";
                    strResult = strRefundDetails;

                    string LstrDetails = "<REQ><FROMDATE>" + strFromDate + "</FROMDATE><TODATE>" + strToDate + "</TODATE>" +
                "<SPNR>" + strRiyaPNR + "</SPNR><STATUS>" + strStatus + "</STATUS><AIRPORTID>" + strAirportID + "</AIRPORTID><REQ>" +
                "<RESPONSE>" + strRefundDetails + "</RESPONSE>";

                    DatabaseLog.LogData(strUserName, "T", "PNRController.cs", "FetchNoShowRefundDetails", LstrDetails, strAgentID, strTerminalId, strsequnceID);
                }
                else
                {
                    Status = "00";
                    ErrorMessage = ErrorMessage != "" ? ErrorMessage : "No Records Found.";
                    strResult = "";

                    string LstrDetails = "<REQ><FROMDATE>" + strFromDate + "</FROMDATE><TODATE>" + strToDate + "</TODATE>" +
               "<SPNR>" + strRiyaPNR + "</SPNR><STATUS>" + strStatus + "</STATUS><AIRPORTID>" + strAirportID + "</AIRPORTID><REQ>" +
               "<RESPONSE>" + ErrorMessage + "</RESPONSE>";

                    DatabaseLog.LogData(strUserName, "T", "PNRController.cs", "FetchNoShowRefundDetails", LstrDetails, strAgentID, strTerminalId, strsequnceID);
                }

            }
            catch (Exception ex)
            {
                Status = "00";
                strResult = "";
                ErrorMessage = "Problem Occured, Please contact support team (#05).";
                DatabaseLog.LogData(strUserName, "X", "PNRController.cs", "FetchNoShowRefundDetails", ex.ToString(), strAgentID, strTerminalId, strsequnceID);
            }
            return Json(new { Status = Status, Message = ErrorMessage, Result = strResult });
        }

        public ActionResult FetchNoShowDashPNRDetails(string strSequenceNo)
        {
            RaysService _RaysService = new RaysService();
            string Status = string.Empty;
            string ErrorMessage = string.Empty;
            string strResult = string.Empty;
            string strPNRDetails = string.Empty;
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_ID"].ToString() : "";
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strsequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0";
            string strIpaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            bool Output = false;
            DataSet dsNoshow = new DataSet();
            try
            {

                if (strAgentID == "")
                {
                    Status = "0";
                    return Json(new { Status = Status, Message = ErrorMessage, Result = strResult });
                }
                #region UsageLog
                string PageName = "NoShow";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                Output = _RaysService.FetchNoShowDashPNRDetails(strAgentID, strSequenceNo, strTerminalId, strUserName,
                   strTerminalType, strIpaddress, Convert.ToDecimal(strsequnceID), ref ErrorMessage, ref strPNRDetails);

                if (Output == true && strPNRDetails != "")
                {
                    Status = "01";
                    ErrorMessage = "";
                    strResult = strPNRDetails;

                    string LstrDetails = "<REQ><SEQUENCENO>" + strSequenceNo + "</SEQUENCENO></REQ><RESPONSE>" + strPNRDetails + "</RESPONSE>";

                    DatabaseLog.LogData(strUserName, "T", "PNRController.cs", "FetchNoShowDashPNRDetails", LstrDetails, strAgentID, strTerminalId, strsequnceID);
                }
                else
                {
                    Status = "00";
                    ErrorMessage = ErrorMessage != "" ? ErrorMessage : "No Records Found.";
                    strResult = "";

                    string LstrDetails = "<REQ><SEQUENCENO>" + strSequenceNo + "</SEQUENCENO></REQ><RESPONSE>" + ErrorMessage + "</RESPONSE>";

                    DatabaseLog.LogData(strUserName, "T", "PNRController.cs", "FetchNoShowDashPNRDetails", LstrDetails, strAgentID, strTerminalId, strsequnceID);
                }

            }
            catch (Exception ex)
            {
                Status = "00";
                strResult = "";
                ErrorMessage = "Problem Occured, Please contact support team (#05).";
                DatabaseLog.LogData(strUserName, "X", "PNRController.cs", "FetchNoShowDashPNRDetails", ex.ToString(), strAgentID, strTerminalId, strsequnceID);
            }
            return Json(new { Status = Status, Message = ErrorMessage, Result = strResult });
        }

        public ActionResult FetchNewRequest(string strRiyaPNR, string strAirlinePNR, string strCRSPNR)
        {
            RaysService _RaysService = new RaysService();
            string Status = string.Empty;
            string ErrorMessage = string.Empty;
            string Error = string.Empty;
            string strResult = string.Empty;
            string strRefundDetails = string.Empty;
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_ID"].ToString() : "";
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strsequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0";
            string strIpaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            DataSet dsNewRequest = new DataSet();
            try
            {

                if (strAgentID == "")
                {
                    Status = "0";
                    return Json(new { Status = Status, Message = ErrorMessage, Result = strResult });
                }

                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                dsNewRequest = _RaysService.Fetch_Pnr_Details_New(strAgentID, strRiyaPNR, strAirlinePNR, strCRSPNR, "", strTerminalId, strUserName, strIpaddress,
                    strTerminalType, Convert.ToDecimal(strsequnceID), ref Error, ref ErrorMessage, "PNR Verification", "bgwFetchPNR_DoWork");

                if (dsNewRequest != null && dsNewRequest.Tables.Count > 0)
                {
                    Status = "01";
                    ErrorMessage = "";
                    strResult = JsonConvert.SerializeObject(dsNewRequest.Tables[0]).ToString();

                    string LstrDetails = "<REQ><SPNR>" + strRiyaPNR + "</SPNR><AIRPNR>" + strAirlinePNR + "</STATUS><CRSPNR>" + strCRSPNR + "</AIRPORTID><REQ>" +
                "<RESPONSE>" + strResult + "</RESPONSE>";

                    DatabaseLog.LogData(strUserName, "T", "PNRController.cs", "FetchNewRequest", LstrDetails, strAgentID, strTerminalId, strsequnceID);
                }
                else
                {
                    Status = "00";
                    ErrorMessage = "No Records Found.";
                    strResult = "";

                    string LstrDetails = "<REQ><SPNR>" + strRiyaPNR + "</SPNR><AIRPNR>" + strAirlinePNR + "</STATUS><CRSPNR>" + strCRSPNR + "</AIRPORTID><REQ>" +
               "<RESPONSE>" + ErrorMessage + "</RESPONSE>";

                    DatabaseLog.LogData(strUserName, "T", "PNRController.cs", "FetchNewRequest", LstrDetails, strAgentID, strTerminalId, strsequnceID);
                }

            }
            catch (Exception ex)
            {
                Status = "00";
                ErrorMessage = "Problem Occured, Please contact support team (#05).";
                strResult = "";

                DatabaseLog.LogData(strUserName, "x", "PNRController.cs", "FetchNewRequest", ex.ToString(), strAgentID, strTerminalId, strsequnceID);
            }
            return Json(new { Status = Status, Message = ErrorMessage, Result = strResult });
        }

        public ActionResult NoShowsubmit_Click(string strNoshowpnrdetails, string strRemarks)
        {
            RaysService _RaysService = new RaysService();
            string Status = string.Empty;
            string ErrorMessage = string.Empty;
            string strRef = string.Empty;
            string strResult = string.Empty;
            string strRefundDetails = string.Empty;
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_ID"].ToString() : "";
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strsequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0";
            string strIpaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            DataSet dsNewRequest = new DataSet();
            DataSet dsnoshowcancel = new DataSet();
            DataSet dsPenalty = new DataSet();
            string Output = string.Empty;
            string request = string.Empty;
            bool OnlineCancellation = false;

            try
            {
                if (strAgentID == "")
                {
                    Status = "0";
                    return Json(new { Status = Status, Message = ErrorMessage, Result = strResult });
                }

                dsnoshowcancel = JsonConvert.DeserializeObject<DataSet>(strNoshowpnrdetails);

                DataTable dtnoshowcancel = new DataTable();
                dtnoshowcancel.Columns.Add("SPNR");
                dtnoshowcancel.Columns.Add("PAX_REF_NO");
                dtnoshowcancel.Columns.Add("SEGMENT_NO");
                dtnoshowcancel.Columns.Add("REMARKS");
                dtnoshowcancel.Columns.Add("DEPARTURE_DATE");
                dtnoshowcancel.Columns.Add("ARRIVAL_DATE");
                dtnoshowcancel.Columns.Add("ORIGIN");
                dtnoshowcancel.Columns.Add("DESTINATION");
                dtnoshowcancel.Columns.Add("AIRPNR");
                dtnoshowcancel.Columns.Add("TRIP_DESC");
                dtnoshowcancel.Columns.Add("CANCELMODE");
                dtnoshowcancel.Columns.Add("NOSHOW");
                dtnoshowcancel.Columns.Add("S_PNR");
                foreach (DataRow drrow in dsnoshowcancel.Tables[0].Rows)
                {
                    dtnoshowcancel.Rows.Add(drrow["S_PNR"].ToString(), drrow["PAX_REF_NO"].ToString(), drrow["SEGMENT_NO"].ToString(),
                        strRemarks, drrow["DEPT_DATE"].ToString(), drrow["ARRIVAL_DATE"].ToString(),
                        drrow["ORIGIN"].ToString(), drrow["DESTINATION"].ToString(), drrow["AIRLINE_PNR"].ToString(),
                        drrow["TRIP_DESC"].ToString(), "F", "Y", drrow["S_PNR"].ToString());
                }
                dsnoshowcancel.Tables.Remove("NOSHOW");
                dsnoshowcancel.Tables.Add(dtnoshowcancel);

                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                Output = _RaysService.Fetch_UpdateDetails(dsnoshowcancel, strAgentID, strTerminalId, strUserName, strIpaddress, strTerminalId, Convert.ToDecimal(strsequnceID),
     ref strRef, ref ErrorMessage, "PNRVerification", "NoShowsubmit_Click", strAgentType.ToUpper().Trim(), strAgentID, strTerminalId, ref dsPenalty, ref request, ref OnlineCancellation);

                if (Output.Trim() == "1")
                {
                    Status = "01";
                    ErrorMessage = "";
                    strResult = "Your cancellation request has been completed" + "\n" + "Your Sequence ID : " + strRef.ToString();
                }
                else if (Output.Trim() == "0")
                {
                    if (!string.IsNullOrEmpty(strRef.ToString().Trim()))
                    {
                        ErrorMessage = strRef.ToString();
                    }
                    else
                    {
                        ErrorMessage = "Problem occured in cancellation request.";
                    }
                    strResult = "";
                    Status = "00";
                }
                else
                {
                    Status = "00";
                    ErrorMessage = "Unable To Cancel the PNR.";
                    strResult = "";
                }

                string LstrDetails = "<REQ><PNRDETAILS>" + dsnoshowcancel.GetXml().ToString() + "</PNRDETAILS><REMARKS>" + strRemarks + "</REMARKS><REQ>" +
                    "<RESPONSE>" + (Output.Trim() == "1" ? strResult : ErrorMessage) + "</RESPONSE>";

                DatabaseLog.LogData(strUserName, "T", "PNRController.cs", "FetchNoShowRefundDetails", LstrDetails, strAgentID, strTerminalId, strsequnceID);
            }
            catch (Exception ex)
            {
                Status = "00";
                ErrorMessage = "Problem Occured, Please contact support team (#05).";
                strResult = "";
                DatabaseLog.LogData(strUserName, "X", "PNRController.cs", "NoShowsubmit_Click", ex.ToString(), strAgentID, strTerminalId, strsequnceID);
            }
            return Json(new { Status = Status, Message = ErrorMessage, Result = strResult });
        }

        #endregion

        #region GROUPSTATUS

        public ActionResult GroupStatusHistory(string strGroupDate, string strGroupTo, string strStatus, string strAirportId, string strReference, string strTravel)
        {
            #region UsageLog
            string PageName = "Group Status";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH"); 
            }
            catch (Exception e) { }
            #endregion
            ArrayList group = new ArrayList();
            group.Add("");
            group.Add("");
            int error = 0;
            int response = 1;
            string strGroupDetails = string.Empty;
            string strErrorMsg = string.Empty;
            RaysService _Rays_service = new RaysService();
            string POS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string POS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string username = Session["username"] != null ? Session["username"].ToString() : "";
            string TerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string ipAddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string sequenceid = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            strGroupDate = DateTime.ParseExact(strGroupDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            strGroupTo = DateTime.ParseExact(strGroupTo, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            strTravel = DateTime.ParseExact(strTravel, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            try
            {
                _Rays_service.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                bool Output = _Rays_service.FetchGroupRequestDetails(POS_ID, POS_TID, username, TerminalType, ipAddress, Convert.ToDecimal(sequenceid),
                    strReference, strGroupDate, strGroupTo, strTravel, ref strGroupDetails, ref strErrorMsg, strStatus, strAirportId);
                if (Output == true)
                {
                    group[response] = strGroupDetails;
                    string LstrDetails = "<REQ><FROMDATE>" + strGroupDate + "</FROMDATE><TODATE>" + strGroupTo + "</TODATE><STATUS>" + strStatus + "</STATUS><AIRPORTID>" + strAirportId + "</AIRPORTID><REFERENCE>" + strReference + "</REFERENCE><TRAVELDATE>" + strTravel + "</TRAVELDATE><REQ>" +
                "<RESPONSE>" + strGroupDetails + "</RESPONSE>";
                    DatabaseLog.LogData(username, "E", "PnrController.cs", "GroupStatusHistory", LstrDetails, POS_ID, POS_TID, sequenceid);

                }
                else
                {
                    group[error] = "problem occured while getting details";
                    string LstrDetails = "<REQ><FROMDATE>" + strGroupDate + "</FROMDATE><TODATE>" + strGroupTo + "</TODATE><STATUS>" + strStatus + "</STATUS><AIRPORTID>" + strAirportId + "</AIRPORTID><REFERENCE>" + strReference + "</REFERENCE><TRAVELDATE>" + strTravel + "</TRAVELDATE><REQ>" +
             "<RESPONSE>" + group[error] + "</RESPONSE>";
                    DatabaseLog.LogData(username, "E", "PnrController.cs", "GroupStatusHistory", LstrDetails, POS_ID, POS_TID, sequenceid);

                }

            }
            catch (Exception e)
            {
                return Json(new { Status = "", Message = "", Result = "problem occured while getting details-#05" });
                DatabaseLog.LogData(username, "X", "PnrController.cs", "GroupStatusHistory", e.ToString(), POS_ID, POS_TID, sequenceid);
            }
            return Json(new { Status = "", Message = "", Result = group });
        }

        #endregion

        public ActionResult Invoice(string SPNR, string AirPNR, string CRSPNR)
        {
            ArrayList arrInvoice = new ArrayList();
            string agentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
            string strterminalid = Session["terminalid"] != null ? Session["terminalid"].ToString() : "";
            string cancellationid = string.Empty;
            string rescheduleid = string.Empty;
            int invoiceFlag = 0;
            arrInvoice.Add("");
            arrInvoice.Add("");
            arrInvoice.Add("");
            arrInvoice.Add("");
            arrInvoice.Add("");
            arrInvoice.Add("");
            string lblOnwardDiscount = string.Empty;
            string lblReturnDiscount = string.Empty;
            string txtAdultDiscount = "0";
            string txtChildDiscount = "0";
            string txtInfantDiscount = "0";
            string txtAdultDiscountRound = "0";
            string txtChildDiscountRound = "0";
            string txtInfantDiscountRound = "0";


            string strErrorMsg = string.Empty;
            bool adultflag = false;
            bool childflag = false;
            bool adultinfantflag = false;
            int error = 0;
            int result = 1;
            int msg = 2;
            int Details = 3;
            int Count = 4;
            int Flag = 5;
            string Error = "";
            StringBuilder str_Builder = new StringBuilder();
            //string BUSPNR = "";
            string lstrPagename = "";
            string lstrFunctionname = "";
            string CancellationValues = string.Empty;
            string strResult = string.Empty;
            string strCompanyID = string.Empty;
            string strUserID = string.Empty;
            string strUserName = string.Empty;
            string Sequenceid = string.Empty;
            string ErrMsg = string.Empty;
            string POS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string POS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string UserName = Session["UserName"] != null ? Session["UserName"].ToString() : "";
            bool Ds_Res = new bool();
            DataSet InvoiceDetails = new DataSet();
            DataSet dsDisplayDet = new DataSet();
            string strIPAddress = "";
            string xmldata = string.Empty;
            string bSpnr = string.Empty;
            string bRiyaPnr = string.Empty;
            string strstatus = string.Empty;
            string strerrormsg = string.Empty;
            string SequenceID = string.Empty;
            string Ipaddress = string.Empty;
            decimal seqno = 0;
            DataTable dtCredentials = new DataTable();

            try
            {
                #region UsageLog
                string PageName = "INVOICE";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                #region RequestLog
                string ReqTime = "BviewpnrdetReqTime:" + DateTime.Now;
                xmldata = "<EVENT><REQUEST>Fetch_PNR_Verification_Details_NewByte</REQUEST>" +
                               "<COMPANYID>" + strCompanyID + "</COMPANYID>" +
                               "<USERID>" + strUserID + "</USERID>" +
                               "</EVENT>";
                string LstrDetails = "<BviewpnrdetReq><URL>[<![CDATA[" + ConfigurationManager.AppSettings["ServiceURI"].ToString() + "]]>]</URL><QUERY>" + xmldata + "</QUERY><REQTIME>" + ReqTime + "</REQTIME></BviewpnrdetReq>";
                // DatabaseLog.LogData(Session["Loginusermailid"] != null ? Session["Loginusermailid"].ToString() : "", "T", "AfterBookingController", "BviewpnrdetReq~" + ConfigurationManager.AppSettings["AppType"].ToString(), LstrDetails, strUserID, strCompanyID, Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : "0", Ipaddress);
                #endregion

                byte[] dsViewPNR_compress = new byte[] { };
                byte[] dsFareDispDet_compress = new byte[] { };

                RaysService _rays = new RaysService();
                _rays.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                // Ds_Res = _rays.Fetch_PNR_Verification_Details_New(Session["POS_ID"].ToString(), SPNR, AirPNR, CRSPNR, "", Session["POS_TID"].ToString(), Session["username"].ToString(), "", "", seqno, ref InvoiceDetails, ref dsDisplayDet, ref Error, ref ErrMsg, lstrPagename, "PrintTicket");
                Ds_Res = _rays.Fetch_PNR_Verification_Details_NewByte(Session["POS_ID"].ToString(), SPNR, AirPNR, CRSPNR, "", Session["POS_TID"].ToString(), Session["username"].ToString(), "", "", seqno, ref dsViewPNR_compress, ref dsFareDispDet_compress, ref Error, ref ErrMsg, lstrPagename, "Invoice", Session["agenttype"].ToString(), POS_TID);

                InvoiceDetails = Base.Decompress(dsViewPNR_compress);
                dsDisplayDet = Base.Decompress(dsFareDispDet_compress);
                string testjson = JsonConvert.SerializeObject(InvoiceDetails);

                #region ResponseLog
                string ResTime = "Fetch_PNR_Verification_Details_New1ResponseTime:" + DateTime.Now;
                StringWriter strWriter = new StringWriter();
                xmldata = "<EVENT><RESPONSE>Fetch_PNR_Verification_Details_NewByte</RESPONSE>" +
                           "<RESULT>" + (JsonConvert.SerializeObject(Ds_Res)) + "</RESULT>" +
                               "<RESPONSEDATA>" + strWriter + "</RESPONSEDATA>" +
                              "</EVENT>";
                LstrDetails = "<Fetch_PNR_Verification_Details_NewByte><URL>[<![CDATA[" + ConfigurationManager.AppSettings["ServiceURI"].ToString() + "]]>]</URL><QUERY>" + xmldata + "</QUERY><RESTIME>" + ResTime + "</RESTIME></Fetch_PNR_Verification_Details_NewByte>";

                // DatabaseLog.LogData(Session["Loginusermailid"] != null ? Session["Loginusermailid"].ToString() : "", "T", "AfterBookingController", "Fetch_PNR_Verification_Details_New1~" + "~" + ConfigurationManager.AppSettings["AppType"].ToString(), LstrDetails, strUserID, strCompanyID, Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : "0", Ipaddress);
                #endregion

                Session["InvoiceDetails"] = InvoiceDetails;
                Session["lblSelectedSPNR"] = SPNR.ToString().Trim();

                if (InvoiceDetails != null && InvoiceDetails.Tables.Count > 0 && InvoiceDetails.Tables[0].Rows.Count > 0)
                {
                    if (InvoiceDetails.Tables[0].Rows[0]["DISCOUNT_CODE"].ToString().ToUpper().Trim() != "0.1") //strAgentType.ToString().Trim().ToUpper() == "RI" &&
                    {
                        var qryCancel = from p in InvoiceDetails.Tables[0].AsEnumerable()
                                        where (p.Field<string>("TICKET_STATUS").ToString().ToUpper().Trim() == "CONFIRMED" ||
                                               p.Field<string>("TICKET_STATUS").ToString().ToUpper().Trim() == "LIVE" ||
                                               p.Field<string>("TICKET_STATUS").ToString().ToUpper().Trim() == "TO BE CANCELLED" ||
                                               p.Field<string>("TICKET_STATUS").ToString().ToUpper().Trim() == "TO BE RESCHEDULE")
                                        select p;

                        DataView dvCancel;
                        DataTable dtCancel = new DataTable();
                        dvCancel = qryCancel.AsDataView();
                        if (dvCancel.Count > 0)
                        {
                        }
                        else
                        {
                            string sScript;
                            invoiceFlag = 1;
                            arrInvoice[Flag] = "1";
                            sScript = "window.open('ShowDetails.aspx', 'Graph', 'scrollbars=yes,toolbar=no,location=0,menubar=no,resizable=no,width=980,height=600,top=50,left=10;');";
                            // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
                            // return;
                        }


                        string strTrip = string.Empty;
                        var qryCheckRoundTrip = (from p in InvoiceDetails.Tables[0].AsEnumerable()
                                                 group p by p.Field<string>("TRIP_DESC") into n
                                                 select new { test = n.Key }).Distinct();
                        int count = qryCheckRoundTrip.Count();
                        if (count == 2)
                        {
                            Session["DiscountTrip"] = "R";

                        }
                        else
                        {
                            Session["DiscountTrip"] = "O";
                            foreach (DataRow drRows in InvoiceDetails.Tables[0].Rows)
                                drRows["TRIP_DESC"] = "O";
                        }
                        DataTable dtAirDetail = new DataTable();
                        var qryPass = (from p in InvoiceDetails.Tables[0].AsEnumerable()
                                       select new
                                       {
                                           AIRPORTID = p.Field<string>("AIRPORTID"),
                                           FLIGHTNO = p.Field<string>("FLIGHT_NO"),
                                           ORIGIN = p.Field<string>("ORIGIN"),
                                           DESTINATION = p.Field<string>("DESTINATION"),
                                           ORIGINCODE = p.Field<string>("ORIGINCODE"),
                                           DESTINATIONCODE = p.Field<string>("DESTINATIONCODE"),
                                           S_PNR = p.Field<string>("S_PNR"),
                                           TRIP_DESC = p.Field<string>("TRIP_DESC")
                                       }).Distinct();

                        dtAirDetail = Serv.ConvertToDataTable(qryPass);
                        if (Session["DiscountTrip"].ToString() == "R")
                        {

                            var temp = from p in dtAirDetail.AsEnumerable()
                                       where p.Field<string>("TRIP_DESC").ToString() == "O"
                                       select p;
                            DataTable dtOrigin = temp.CopyToDataTable();

                            var temp1 = from p in dtAirDetail.AsEnumerable()
                                        where p.Field<string>("TRIP_DESC").ToString() == "R"
                                        select p;
                            DataTable dtOrigin1 = temp1.CopyToDataTable();

                            lblOnwardDiscount = dtOrigin.Rows[0]["ORIGIN"].ToString() + " - " + dtOrigin.Rows[dtOrigin.Rows.Count - 1]["DESTINATION"].ToString();
                            lblReturnDiscount = dtOrigin1.Rows[0]["ORIGIN"].ToString() + " - " + dtOrigin1.Rows[dtOrigin1.Rows.Count - 1]["DESTINATION"].ToString();
                        }
                        else
                        {
                            lblOnwardDiscount = dtAirDetail.Rows[0]["ORIGIN"].ToString() + " - " + dtAirDetail.Rows[dtAirDetail.Rows.Count - 1]["DESTINATION"].ToString();
                            lblReturnDiscount = dtAirDetail.Rows[0]["DESTINATION"].ToString() + " - " + dtAirDetail.Rows[dtAirDetail.Rows.Count - 1]["ORIGIN"].ToString();
                        }

                        DataView tempDataView;
                        DataTable dtoneWay = new DataTable();
                        var qryOneWayAvailFlights = from p in InvoiceDetails.Tables[0].AsEnumerable()
                                                    where p.Field<string>("TRIP_DESC").ToUpper().Trim() == "O"
                                                    select p;
                        tempDataView = qryOneWayAvailFlights.AsDataView();
                        if (tempDataView.Count != 0)
                        {
                            dtoneWay = qryOneWayAvailFlights.CopyToDataTable();
                        }
                        string OnwFareId = string.Empty;
                        string OnwPaxRefNo = string.Empty;
                        string ReturnFareId = string.Empty;
                        string ReturnRefNo = string.Empty;
                        foreach (DataRow dr in dtoneWay.Rows)
                        {
                            if ((dr["PASSENGER_TYPE"].ToString().ToUpper().Trim() == "ADULT" || dr["PASSENGER_TYPE"].ToString().ToUpper() == "ADT") &&
                                dr["TICKET_STATUS"].ToString().ToUpper().Trim() != "CANCELLED" && (OnwFareId.ToUpper().Trim() != dr["FARE_ID"].ToString().ToUpper().Trim()
                                    || OnwPaxRefNo != dr["PAX_REF_NO"].ToString()))
                            {
                                adultflag = true;
                                if (!string.IsNullOrEmpty(dr["PCI_COMMISION_AMT"].ToString()))
                                {
                                    if (Convert.ToDecimal(dr["PCI_COMMISION_AMT"].ToString()) > 0)
                                    {
                                        string[] strAmount = dr["PCI_COMMISION_AMT"].ToString().Split('.');
                                        txtAdultDiscount = strAmount[0].ToString();
                                    }
                                    else
                                        txtAdultDiscount = "0";

                                }
                                else
                                {
                                    adultflag = true;
                                    txtAdultDiscount = "0";
                                }
                            }
                            if ((dr["PASSENGER_TYPE"].ToString().ToUpper().Trim() == "CHILD" || dr["PASSENGER_TYPE"].ToString().ToUpper() == "CHD") &&
                                dr["TICKET_STATUS"].ToString().ToUpper().Trim() != "CANCELLED" && (OnwFareId.ToUpper().Trim() != dr["FARE_ID"].ToString().ToUpper().Trim()
                                    || OnwPaxRefNo != dr["PAX_REF_NO"].ToString()))
                            {
                                childflag = true;
                                if (!string.IsNullOrEmpty(dr["PCI_COMMISION_AMT"].ToString()))
                                {
                                    if (Convert.ToDecimal(dr["PCI_COMMISION_AMT"].ToString()) > 0)
                                    {
                                        string[] strAmount = dr["PCI_COMMISION_AMT"].ToString().Split('.');
                                        txtChildDiscount = strAmount[0].ToString();
                                    }
                                    else
                                        txtChildDiscount = "0";
                                }
                                else
                                {
                                    childflag = true;
                                    txtChildDiscount = "0";
                                }
                            }
                            if ((dr["PASSENGER_TYPE"].ToString().ToUpper().Trim() == "INFANT" || dr["PASSENGER_TYPE"].ToString().ToUpper() == "INF") &&
                                dr["TICKET_STATUS"].ToString().ToUpper().Trim() != "CANCELLED" && (OnwFareId.ToUpper().Trim() != dr["FARE_ID"].ToString().ToUpper().Trim()
                                    || OnwPaxRefNo != dr["PAX_REF_NO"].ToString()))
                            {
                                adultinfantflag = true;
                                if (!string.IsNullOrEmpty(dr["PCI_COMMISION_AMT"].ToString()))
                                {
                                    if (Convert.ToDecimal(dr["PCI_COMMISION_AMT"].ToString()) > 0)
                                    {
                                        string[] strAmount = dr["PCI_COMMISION_AMT"].ToString().Split('.');
                                        txtInfantDiscount = strAmount[0].ToString();
                                    }
                                    else
                                        txtInfantDiscount = "0";
                                }
                                else
                                {
                                    adultinfantflag = true;
                                    txtInfantDiscount = "0";
                                }
                            }
                            OnwFareId = dr["FARE_ID"].ToString();
                            OnwPaxRefNo = dr["PAX_REF_NO"].ToString();
                        }
                        if (Session["DiscountTrip"].ToString() == "R")
                        {
                            DataTable dtRoundTrip = new DataTable();
                            //btnOkDiscount.Text = "Next";
                            DataView tempDataView1;
                            var qryRndWayAvailFlights = from p in InvoiceDetails.Tables[0].AsEnumerable()
                                                        where p.Field<string>("TRIP_DESC").ToUpper().Trim() == "R"
                                                        select p;
                            tempDataView1 = qryRndWayAvailFlights.AsDataView();
                            if (tempDataView1.Count != 0)
                            {
                                dtRoundTrip = qryRndWayAvailFlights.CopyToDataTable();
                            }
                            foreach (DataRow dr in dtRoundTrip.Rows)
                            {
                                if ((dr["PASSENGER_TYPE"].ToString().ToUpper().Trim() == "ADULT" || dr["PASSENGER_TYPE"].ToString().ToUpper() == "ADT") &&
                                     dr["TICKET_STATUS"].ToString().ToUpper().Trim() != "CANCELLED" && (ReturnFareId.ToUpper().Trim() != dr["FARE_ID"].ToString().ToUpper().Trim()
                                    || ReturnRefNo != dr["PAX_REF_NO"].ToString()))
                                {
                                    adultflag = true;
                                    if (!string.IsNullOrEmpty(dr["PCI_COMMISION_AMT"].ToString()))
                                    {
                                        if (Convert.ToDecimal(dr["PCI_COMMISION_AMT"].ToString()) > 0)
                                        {
                                            string[] strAmount = dr["PCI_COMMISION_AMT"].ToString().Split('.');
                                            txtAdultDiscountRound = strAmount[0].ToString();
                                        }
                                        else
                                            txtAdultDiscountRound = "0";
                                    }
                                    else
                                    {
                                        adultflag = true;
                                        txtAdultDiscountRound = "0";
                                    }
                                }
                                if ((dr["PASSENGER_TYPE"].ToString().ToUpper().Trim() == "CHILD" || dr["PASSENGER_TYPE"].ToString().ToUpper() == "CHD") &&
                                     dr["TICKET_STATUS"].ToString().ToUpper().Trim() != "CANCELLED" && (ReturnFareId.ToUpper().Trim() != dr["FARE_ID"].ToString().ToUpper().Trim()
                                    || ReturnRefNo != dr["PAX_REF_NO"].ToString()))
                                {
                                    childflag = true;
                                    if (!string.IsNullOrEmpty(dr["PCI_COMMISION_AMT"].ToString()))
                                    {
                                        if (Convert.ToDecimal(dr["PCI_COMMISION_AMT"].ToString()) > 0)
                                        {
                                            string[] strAmount = dr["PCI_COMMISION_AMT"].ToString().Split('.');
                                            txtChildDiscountRound = strAmount[0].ToString();
                                        }
                                        else
                                            txtChildDiscountRound = "0";
                                    }
                                    else
                                    {
                                        childflag = true;
                                        txtChildDiscountRound = "0";
                                    }
                                }
                                if ((dr["PASSENGER_TYPE"].ToString().ToUpper().Trim() == "INFANT" || dr["PASSENGER_TYPE"].ToString().ToUpper() == "INF") &&
                                     dr["TICKET_STATUS"].ToString().ToUpper().Trim() != "CANCELLED" && (ReturnFareId.ToUpper().Trim() != dr["FARE_ID"].ToString().ToUpper().Trim()
                                    || ReturnRefNo != dr["PAX_REF_NO"].ToString()))
                                {
                                    adultinfantflag = true;
                                    if (!string.IsNullOrEmpty(dr["PCI_COMMISION_AMT"].ToString()))
                                    {
                                        if (Convert.ToDecimal(dr["PCI_COMMISION_AMT"].ToString()) > 0)
                                        {
                                            string[] strAmount = dr["PCI_COMMISION_AMT"].ToString().Split('.');
                                            txtInfantDiscountRound = strAmount[0].ToString();
                                        }
                                        else
                                            txtInfantDiscountRound = "0";
                                    }
                                    else
                                    {
                                        adultinfantflag = true;
                                        txtInfantDiscountRound = "0";
                                    }
                                }
                                ReturnFareId = dr["FARE_ID"].ToString();
                                ReturnRefNo = dr["PAX_REF_NO"].ToString();
                            }
                        }

                    }
                    else
                    {
                        string sScript;
                        arrInvoice[Flag] = "1";
                        //return arrInvoice;


                    }
                    arrInvoice[Details] = txtAdultDiscount + "|" + txtChildDiscount + "|" + txtInfantDiscount + "|" + txtAdultDiscountRound + "|" + txtChildDiscountRound + "|" + txtInfantDiscountRound;
                    arrInvoice[Count] = Session["DiscountTrip"] + "|" + lblOnwardDiscount + "|" + lblReturnDiscount + "|" + adultflag + "|" + childflag + "|" + adultinfantflag;
                }


            }
            catch (Exception ex)
            {
                strstatus = "0";
                string errormgs = string.Empty;
                if (ex.Message.ToString() == "The operation has timed out")
                    errormgs = ex.Message.ToString();
                else if (ex.Message.ToString() == "Unable to connect the remote server")
                    errormgs = "unable to connect the remote server.";
                else
                    errormgs = "Problem occured While BviewPNR details.Please contact support team (#05).";

                strerrormsg = errormgs;
                DatabaseLog.LogData(Session["username"].ToString(), "X", "PerformaInvoice", "PerformaInvoice", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            return Json(new { status = "01", Error = "", Result = "" });
        }

        public ActionResult FormingInvoiceDetails()
        {
            try
            {
                Session["strBuild"] = "";
                string strErrorMsg = string.Empty;
                DataSet InvoiceDetails = (DataSet)Session["InvoiceDetails"];

                if (InvoiceDetails != null && InvoiceDetails.Tables.Count > 0 && InvoiceDetails.Tables[0].Rows.Count > 0)
                {

                    var qryAmountDetails = (from p in InvoiceDetails.Tables[0].AsEnumerable()
                                            select new
                                            {
                                                PaxType = p.Field<string>("PASSENGER_TYPE"),
                                                Title = p.Field<string>("PASSENGER_TITLE"),
                                                FirstName = p.Field<string>("FIRST_NAME"),
                                                LastName = p.Field<string>("LAST_NAME"),
                                                TaxAmount = p.Field<decimal?>("TAXAMOUNT"),
                                                MEALSAMOUNT = p.Field<decimal?>("MEALS_AMOUNT"),
                                                SERVICECHARGE = p.Field<decimal?>("SERVICECHARGEORG"), // p.Field<decimal>("SERVICECHARGE"),
                                                TRANSACTION_FEE = p.Field<decimal?>("TRANS_FEE"),
                                                SERVICE_TAX = p.Field<decimal?>("SERVICE_TAX"),
                                                TAXCODE = p.Field<string>("TAXCODE"),
                                                BasicFare = p.Field<decimal?>("BASICFAREAMOUNT"),
                                                PAX_REF_NO = p.Field<Int16>("PAX_REF_NO"),
                                                PREVIOUS_TICKET = p.Field<string>("PREVIOUS_TICKET_NO"),
                                                SUPPILER_PENALITY = p.Field<decimal?>("SUPPILER_PENALITY"),
                                                AGENT_PENALITY = p.Field<decimal?>("AGENT_PENALITY"),
                                                FARE_DIFFERENCE = p.Field<decimal?>("FARE_DIFFERENCE"),
                                                TRIP_DESC = p.Field<string>("TRIP_DESC"),
                                                AIRLINE_PNR = p.Field<string>("AIRLINE_PNR"),
                                                ORIGINCODE = p.Field<string>("ORIGINCODE"),
                                                DESTINATIONCODE = p.Field<string>("DESTINATIONCODE"),
                                                PLATING_CARRIER = p.Field<string>("PLATING_CARRIER"),
                                                DEPT_DATE = p.Field<DateTime>("DEPT_DATE"),
                                                ARRIVAL_DATE = p.Field<DateTime>("ARRIVAL_DATE"),
                                                CLASS_TYPE = p.Field<string>("CLASS_TYPE"),
                                                PCI_TDS_AMOUNT = p.Field<decimal?>("PCI_TDS_AMOUNT"),
                                                PLB_TDS_AMOUNT = p.Field<decimal?>("PLB_TDS_AMOUNT"),
                                                INCENTIVE_AMT = p.Field<decimal?>("PCI_COMMISION_AMT"),
                                                BAGGAGE_AMOUNT = p.Field<decimal?>("BAGGAGE_AMOUNT"),
                                                SSR_AMOUNT = p.Field<decimal?>("SSR_AMOUNT"),
                                                ISSUED_DATE = p.Field<string>("BOOKED_DATE"),
                                                SEAT_AMOUNT = p.Field<decimal?>("SEAT_AMOUNT"),
                                                // COMMISSION_AMT = "",//p.Field<string>("COMMISSION_AMT")
                                                PLB_AMOUNT = p.Field<decimal?>("PLB_AMOUNT"),
                                                SF_GST = p.Field<decimal?>("PCI_SF_GST"),
                                                SF_TAX = p.Field<decimal?>("PCI_SF"),
                                                MARKUP = p.Field<decimal?>("PAI_TKT_MANAGEMENT_FEE"),
                                                SERVICE_FEE = p.Field<decimal>("SERVICE_FEE"),
                                                //SSR_AMOUNT = "" //p.Field<decimal>("SSR_AMOUNT")
                                            }).Distinct();
                    DataTable dtTempInvoice = Serv.ConvertToDataTable(qryAmountDetails);

                    string strOrgDest = string.Empty;
                    string Origin_code = string.Empty;
                    string Destination_code = string.Empty;

                    string Origin_codeRnd = string.Empty;
                    string Destination_codeRnd = string.Empty;

                    if (InvoiceDetails.Tables[0].Rows[0]["SPECIAL_TRIP"].ToString() == "S")
                    {
                        Origin_code = InvoiceDetails.Tables[0].Rows[0]["ORIGINCODE"].ToString();
                        Destination_code = InvoiceDetails.Tables[0].Rows[0]["ORIGINCODE"].ToString();

                        strOrgDest = InvoiceDetails.Tables[0].Rows[0]["ORIGIN"].ToString() + " --> " + InvoiceDetails.Tables[0].Rows[0]["ORIGIN"].ToString();
                    }
                    else
                    {
                        var qryPass = (from p in InvoiceDetails.Tables[0].AsEnumerable()
                                       select new
                                       {
                                           TRIP_DESC = p.Field<string>("TRIP_DESC")
                                       }).Distinct();
                        DataTable dtTrip = Serv.ConvertToDataTable(qryPass);
                        if (dtTrip.Rows.Count == 1)
                        {
                            Origin_code = InvoiceDetails.Tables[0].Rows[0]["ORIGINCODE"].ToString();
                            Destination_code = InvoiceDetails.Tables[0].Rows[InvoiceDetails.Tables[0].Rows.Count - 1]["DESTINATIONCODE"].ToString();

                            foreach (DataRow drRows in dtTempInvoice.Rows)
                                drRows["TRIP_DESC"] = "O";
                        }
                        else if (dtTrip.Rows.Count == 2)
                        {
                            DataTable dtOneWay = new DataTable();
                            var qrytrip = from p in InvoiceDetails.Tables[0].AsEnumerable()
                                          where p.Field<string>("TRIP_DESC") == "O"
                                          select p;
                            DataView dview = qrytrip.AsDataView();
                            if (dview.Count > 0)
                            {
                                dtOneWay = qrytrip.CopyToDataTable();
                                Origin_code = dtOneWay.Rows[0]["ORIGINCODE"].ToString();
                                Destination_code = dtOneWay.Rows[dtOneWay.Rows.Count - 1]["DESTINATIONCODE"].ToString();
                            }

                            DataTable dtRndWay = new DataTable();
                            var qryRndtrip = from p in InvoiceDetails.Tables[0].AsEnumerable()
                                             where p.Field<string>("TRIP_DESC") == "R"
                                             select p;
                            DataView dRview = qryRndtrip.AsDataView();
                            if (dRview.Count > 0)
                            {
                                dtRndWay = qryRndtrip.CopyToDataTable();
                                Origin_codeRnd = dtRndWay.Rows[0]["ORIGINCODE"].ToString();
                                Destination_codeRnd = dtRndWay.Rows[dtRndWay.Rows.Count - 1]["DESTINATIONCODE"].ToString();
                            }
                        }
                        strOrgDest = InvoiceDetails.Tables[0].Rows[0]["ORIGIN"].ToString() + " --> " + InvoiceDetails.Tables[0].Rows[InvoiceDetails.Tables[0].Rows.Count - 1]["DESTINATION"].ToString();
                    }

                    decimal mealsAmount = 0;
                    decimal SeatAmount = 0; decimal TotalSSRAmount = 0;
                    decimal BaggageAmount = 0;
                    decimal totalpassAdult = 0, totalpassChild = 0, totalpassAdultInfant = 0;
                    decimal Adulttotaltax = 0, Childtotaltax = 0, AdultInfanttotaltax = 0;
                    decimal adultServiceCharge = 0, childServiceCharge = 0, infantServiceCharge = 0;
                    decimal ServiceCharge = 0, totalAdultServiceTax = 0, totalChildServiceTax = 0, totalInfantServiceTax = 0;
                    decimal AdultCommision = 0, childCommision = 0, infantCommision = 0;
                    decimal AdultTDS = 0, childTDS = 0, InfantTDS = 0;
                    decimal AdultDiscount = 0, ChildDiscout = 0, infantDiscount = 0;
                    decimal totalAdultTransactionFee = 0, totalChildTransactionFee = 0, totalInfantTransactionFee = 0;
                    decimal TotalEarning = 0;
                    decimal OthersAmount = 0;
                    decimal ServiceFee = 0;
                    decimal sf_tax = 0, sf_gst = 0, markup = 0;



                    var tempPaxDetails = (from p in InvoiceDetails.Tables[0].AsEnumerable()
                                          select new
                                          {
                                              PaxType = p.Field<string>("PASSENGER_TYPE"),
                                              TaxAmount = p.Field<decimal?>("TAXAMOUNT"),
                                              MEALSAMOUNT = p.Field<decimal?>("MEALS_AMOUNT"),
                                              SERVICECHARGE = p.Field<decimal?>("SERVICECHARGEORG"),
                                              TRANSACTION_FEE = p.Field<decimal?>("TRANS_FEE"),
                                              SERVICE_TAX = p.Field<decimal?>("SERVICE_TAX"),
                                              TAXCODE = p.Field<string>("TAXCODE"),
                                              BasicFare = p.Field<decimal?>("BASICFAREAMOUNT"),
                                              SUPPILER_PENALITY = p.Field<decimal?>("SUPPILER_PENALITY"),
                                              AGENT_PENALITY = p.Field<decimal?>("AGENT_PENALITY"),
                                              FARE_DIFFERENCE = p.Field<decimal?>("FARE_DIFFERENCE"),
                                              PCI_TDS_AMOUNT = p.Field<decimal?>("PCI_TDS_AMOUNT"),
                                              PLB_TDS_AMOUNT = p.Field<decimal?>("PLB_TDS_AMOUNT"),
                                              INCENTIVE_AMT = p.Field<decimal?>("PCI_COMMISION_AMT"),
                                              BAGGAGE_AMOUNT = p.Field<decimal?>("BAGGAGE_AMOUNT"),
                                              PAX_REF_NO = p.Field<Int16>("PAX_REF_NO"),
                                              // COMMISSION_AMT = p.Field<string>("COMMISSION_AMT"),
                                              SSR_AMOUNT = p.Field<decimal?>("SSR_AMOUNT"),
                                              ISSUED_DATE = p.Field<string>("BOOKED_DATE"),
                                              SEAT_AMOUNT = p.Field<decimal?>("SEAT_AMOUNT"),
                                              PLB_AMOUNT = p.Field<decimal?>("PLB_AMOUNT"),
                                              SF_AMT = p.Field<decimal?>("PCI_SF"),
                                              SF_GST = p.Field<decimal?>("PCI_SF_GST"),
                                              MARKUP = p.Field<decimal?>("PAI_TKT_MANAGEMENT_FEE"),
                                              // SERVICE_FEE = p.Field<decimal>("SERVICE_FEE"),
                                              // SSR_AMOUNT = p.Field<decimal>("SSR_AMOUNT"),
                                              // SEAT_AMOUNT = p.Field<decimal>("SEAT_AMOUNT")
                                          }).Distinct();

                    DataTable dtTempFareDetails = Serv.ConvertToDataTable(tempPaxDetails);

                    for (int Passengercount = 0; Passengercount <= dtTempFareDetails.Rows.Count - 1; Passengercount++)
                    {

                        if (dtTempFareDetails.Rows[Passengercount]["PaxType"].ToString().ToUpper().Trim() == "ADULT" ||
                            dtTempFareDetails.Rows[Passengercount]["PaxType"].ToString().ToUpper().Trim() == "ADT")
                        {
                            totalpassAdult += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["BasicFare"].ToString());
                            Adulttotaltax += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["TaxAmount"].ToString());
                            totalAdultServiceTax += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SERVICE_TAX"].ToString());
                            //totalAdultTransactionFee += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["TRANSACTION_FEE"].ToString());
                            //AdultCommision += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["PCI_COMMISION_AMT"].ToString());
                            AdultDiscount += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["INCENTIVE_AMT"].ToString());
                            AdultTDS += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["PCI_TDS_AMOUNT"].ToString());
                            sf_tax += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SF_AMT"].ToString());
                            markup += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["MARKUP"].ToString());
                            sf_gst += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SF_GST"].ToString());
                            if (!(dtTempFareDetails.Rows[Passengercount]["SERVICECHARGE"].ToString() == "-1"))
                                adultServiceCharge += dtTempFareDetails.Rows[Passengercount]["SERVICECHARGE"].ToString() != "" ? Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SERVICECHARGE"].ToString()) : 0;
                        }
                        if (dtTempFareDetails.Rows[Passengercount]["PaxType"].ToString().ToUpper().Trim() == "CHILD" ||
                            dtTempFareDetails.Rows[Passengercount]["PaxType"].ToString().ToUpper().Trim() == "CHD")
                        {
                            totalpassChild += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["BasicFare"].ToString());
                            Childtotaltax += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["TaxAmount"].ToString());
                            totalChildServiceTax += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SERVICE_TAX"].ToString());
                            //totalChildTransactionFee += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["TRANSACTION_FEE"].ToString());
                            //childCommision += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["PCI_COMMISION_AMT"].ToString());
                            childTDS += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["PCI_TDS_AMOUNT"].ToString());
                            ChildDiscout += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["INCENTIVE_AMT"].ToString());
                            sf_tax += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SF_AMT"].ToString());
                            markup += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["MARKUP"].ToString());
                            sf_gst += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SF_GST"].ToString());
                            if (!(dtTempFareDetails.Rows[Passengercount]["SERVICECHARGE"].ToString() == "-1"))
                                childServiceCharge += dtTempFareDetails.Rows[Passengercount]["SERVICECHARGE"].ToString() != "" ? Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SERVICECHARGE"].ToString()) : 0;
                        }
                        if (dtTempFareDetails.Rows[Passengercount]["PaxType"].ToString().ToUpper().Trim() == "INFANT" ||
                            dtTempFareDetails.Rows[Passengercount]["PaxType"].ToString().ToUpper().Trim() == "INF")
                        {
                            totalpassAdultInfant += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["BasicFare"].ToString());
                            AdultInfanttotaltax += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["TaxAmount"].ToString());
                            totalInfantServiceTax += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SERVICE_TAX"].ToString());
                            //totalInfantTransactionFee += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["TRANSACTION_FEE"].ToString());
                            //infantCommision += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["PCI_COMMISION_AMT"].ToString());
                            InfantTDS += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["PCI_TDS_AMOUNT"].ToString());
                            infantDiscount += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["INCENTIVE_AMT"].ToString());
                            sf_tax += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SF_AMT"].ToString());
                            markup += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["MARKUP"].ToString());
                            sf_gst += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SF_GST"].ToString());
                            if (!(dtTempFareDetails.Rows[Passengercount]["SERVICECHARGE"].ToString() == "-1"))
                                infantServiceCharge += dtTempFareDetails.Rows[Passengercount]["SERVICECHARGE"].ToString() != "" ? Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SERVICECHARGE"].ToString()) : 0;
                        }
                        mealsAmount += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["MealsAmount"].ToString());
                        BaggageAmount += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["BAGGAGE_AMOUNT"].ToString());
                        OthersAmount += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SSR_AMOUNT"].ToString());
                        // ServiceFee += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SERVICE_FEE"].ToString());
                        SeatAmount += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["SEAT_AMOUNT"].ToString());
                        // TotalEarning += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["INCENTIVE_AMT"].ToString());
                        TotalEarning += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["PLB_AMOUNT"].ToString());
                        //  TotalEarning += Convert.ToDecimal(dtTempFareDetails.Rows[Passengercount]["COMMISSION_AMT"].ToString());

                    }

                    ServiceCharge = adultServiceCharge + childServiceCharge + infantServiceCharge;
                    decimal totalServiceTax = totalAdultServiceTax + totalChildServiceTax + totalInfantServiceTax;
                    decimal totalTransactionFee = totalAdultTransactionFee + totalChildTransactionFee + totalInfantTransactionFee;
                    decimal totalCommision = AdultCommision + childCommision + infantCommision;
                    decimal totalTDS = AdultTDS + childTDS + InfantTDS;
                    decimal totalDiscount = AdultDiscount + ChildDiscout + infantDiscount;
                    decimal totalBasicAmt = totalpassAdult + totalpassChild + totalpassAdultInfant;

                    //decimal totaltaxamt = Adulttotaltax + Childtotaltax + AdultInfanttotaltax + mealsAmount + BaggageAmount + SeatAmount + OthersAmount + ServiceFee + totalTDS + sf_tax + sf_gst + markup;
                    decimal totaltaxamt = Adulttotaltax + Childtotaltax + AdultInfanttotaltax + mealsAmount + BaggageAmount + SeatAmount + OthersAmount + sf_tax + sf_gst + markup;

                    decimal grosstotal = 0;
                    if (Session["agenttype"].ToString().Trim().ToUpper() == "RI")
                    {
                        //grosstotal = (ServiceCharge + totalpassAdult + totalpassChild + totalpassAdultInfant
                        //                      + Adulttotaltax + Childtotaltax + AdultInfanttotaltax + totalServiceTax
                        //                      + mealsAmount + BaggageAmount + SeatAmount + OthersAmount + ServiceFee + totalTDS + sf_tax + sf_gst + markup) - (totalDiscount);
                        grosstotal = (ServiceCharge + totalpassAdult + totalpassChild + totalpassAdultInfant
                                            + Adulttotaltax + Childtotaltax + AdultInfanttotaltax
                                            + mealsAmount + BaggageAmount + SeatAmount + OthersAmount + sf_tax + sf_gst + markup) - (totalDiscount);
                    }
                    else if (Session["agenttype"].ToString().Trim().ToUpper() == "RE" && InvoiceDetails.Tables[0].Rows[0]["PAYMENT_MODE"].ToString().ToUpper().Trim() == "P")
                    {
                        //grosstotal = (ServiceCharge + totalpassAdult + totalpassChild + totalpassAdultInfant
                        //                      + Adulttotaltax + Childtotaltax + AdultInfanttotaltax + totalServiceTax
                        //                      + mealsAmount + BaggageAmount + SeatAmount + OthersAmount + ServiceFee + totalTDS + sf_tax + sf_gst + markup) - (TotalEarning);

                        grosstotal = (ServiceCharge + totalpassAdult + totalpassChild + totalpassAdultInfant
                                             + Adulttotaltax + Childtotaltax + AdultInfanttotaltax
                                             + mealsAmount + BaggageAmount + SeatAmount + OthersAmount + sf_tax + sf_gst + markup) - (TotalEarning);
                        totaltaxamt = totaltaxamt - TotalEarning;
                    }
                    else
                    {
                        //grosstotal = (ServiceCharge + totalpassAdult + totalpassChild + totalpassAdultInfant
                        //                     + Adulttotaltax + Childtotaltax + AdultInfanttotaltax
                        //                     + mealsAmount + BaggageAmount + SeatAmount + totalServiceTax + OthersAmount + ServiceFee + totalTDS + sf_tax + sf_gst + markup);

                        grosstotal = (ServiceCharge + totalpassAdult + totalpassChild + totalpassAdultInfant
                                            + Adulttotaltax + Childtotaltax + AdultInfanttotaltax
                                            + mealsAmount + BaggageAmount + SeatAmount + OthersAmount + sf_tax + sf_gst + markup);
                    }
                    var address = InvoiceDetails.Tables[0].Rows[0]["AGENCY_NAMEADDRESS"].ToString().Split('~');
                    try
                    {
                        // string[] helptext = InvoiceDetails.Tables["P_FETCH_PNR_DETAILS_CBT6"].Rows[0]["COM_HELP_TEXT"].ToString().Split('|');
                        ViewBag.EMailid = "";// helptext[0];
                        ViewBag.Contacno = "";// helptext[1];
                    }
                    catch (Exception ex)
                    {


                    }

                    StringBuilder strBuildHeader = new StringBuilder();

                    strBuildHeader.Append("<html><head><style>@media print{table {page-break-after:auto}}</style>");
                    strBuildHeader.Append("<style type='text/css'>");
                    strBuildHeader.Append("table.sample {");
                    strBuildHeader.Append("border-width: 1px;border-spacing: 2px;border-style: 	 outset;border-color:black;border-collapse: collapse;");
                    strBuildHeader.Append("}");
                    strBuildHeader.Append("table.TicketStatus {");
                    strBuildHeader.Append("width:30%;	border-width: 1px;	border-spacing: 2px;border-style: outset;	border-color: #E7E7E7;	border-collapse: collapse;	background-color: white;");
                    strBuildHeader.Append("}");
                    strBuildHeader.Append(".th{");
                    strBuildHeader.Append(" background-color:black; ");
                    strBuildHeader.Append("}");
                    strBuildHeader.Append("table.sample th {");
                    strBuildHeader.Append("}");
                    strBuildHeader.Append("table.sample td { ");
                    strBuildHeader.Append("border-width:  1px;padding: 1px;	border-style: inset;border-collapse: collapse	border-color: #E7E7E7;	background-color: Transparent;	-moz-border-radius: ;filter:alpha(opacity=80);opacity:0.8;");
                    strBuildHeader.Append("}");
                    strBuildHeader.Append("</style>");
                    strBuildHeader.Append("</head>");
                    strBuildHeader.Append("<body>");
                    if (dtTempInvoice != null && dtTempInvoice.Rows.Count > 0)
                    {
                        StringBuilder strBuild = new StringBuilder();

                        strBuild.Append("<table width='100%' style='font-family:Verdana;font-size:12px;'>");
                        strBuild.Append("<tr>");
                        strBuild.Append("<td align='right'>");
                        strBuild.Append("<input type='Button' id='btnPrint' style=\"font-weight:bold; background-repeat: no-repeat; width: 55px;'\" value='Print' onclick='Print();'/>");
                        strBuild.Append("</td>");
                        strBuild.Append("</tr>");
                        strBuild.Append("</table>");

                        strBuild.Append("<table id='tblPrint' width='100%' border='0' style='font-family: Verdana; font-size: 13px; font-weight: bold'><tr>");
                        if (Session["agenttype"].ToString().Trim().ToUpper() == "RE")
                            strBuild.Append("<td align='left' style='color: Red; font-family: Verdana; font-size: 20px; font-weight: bold;'>Invoice <span style='color:Black; font-family: Verdana; font-size: 13px; font-weight: bold'>- " + InvoiceDetails.Tables[0].Rows[0]["BOOKED_DATE"] + "</span></td>");
                        else
                            strBuild.Append("<td align='left' style='color: Red; font-family: Verdana; font-size: 20px; font-weight: bold;'>Performa Invoice <span style='color:Black; font-family: Verdana; font-size: 13px; font-weight: bold'>- " + InvoiceDetails.Tables[0].Rows[0]["BOOKED_DATE"] + "</span></td>");

                        if (Session["agenttype"].ToString().Trim().ToUpper() == "RI" || Session["agenttype"].ToString().Trim().ToUpper() == "RE")
                        {
                            // strBuild.Append("<td align='right'><img alt='riyalogo' src='Images/Menu/Riyalatest.png' /></td>");
                        }
                        else
                        {
                            strBuild.Append("<td align='right'>&nbsp;</td>");
                        }
                        strBuild.Append("</tr></table>");
                        strBuild.Append("<table width='100%'><tr><td><table width='100%' class='sample'><tr bgcolor='#555555'><td colspan='9' style='background-color: Black; font-size: 1px' bgcolor='#555555'></td></tr></table></td></tr></table>");
                        strBuild.Append("<table width='100%' border='0' style='font-family: Verdana; font-size: 13px; font-weight: bold'><tr align='left' style='border-top:2px solid;border-bottom:2px solid '><td style='padding:5px;'>Inquiry No. :&nbsp;&nbsp;&nbsp;&nbsp;" + Session["lblSelectedSPNR"].ToString().Trim() + "</td></tr></table>");
                        strBuild.Append("<table width='100%'><tr><td><table width='100%'  class='sample'><tr bgcolor='#555555'><td colspan='9' style='background-color: Black; font-size: 1px' bgcolor='#555555'></td></tr></table></td></tr></table>");
                        strBuild.Append("<table width='100%' border='0' style='font-family: Verdana; font-size: 13px; font-weight: bold'><tr><td width='50%'>");

                        strBuild.Append("<table width='100%' style='font-family: Verdana; font-size: 11px; font-weight: bold'>");

                        //string[] Address = GlobalVar.getAgencyAddress.Split('~');
                        strBuild.Append("<tr><td align='left' style='height: auto; font-family: Verdana; font-size: 11px; font-weight: bold'>" + address[0] + "</td></tr>");
                        strBuild.Append("<tr><td align='left' style='height: auto; font-family: Verdana; font-size: 10px; font-weight: bold'>" + address[1] + "</td></tr>");
                        strBuild.Append("<tr><td align='left' style='height: auto; font-family: Verdana; font-size: 10px; font-weight: bold'>" + address[2] + "</td></tr>");
                        strBuild.Append("<tr><td align='left' style='height: auto; font-family: Verdana; font-size: 10px; font-weight: bold'>" + address[3] + "</td></tr>");
                        strBuild.Append("<tr>");
                        strBuild.Append("<td align='left' style='height: auto; font-family: Verdana; font-size: 10px; font-weight: bold'>");
                        strBuild.Append("Contact No : " + address[4].ToString() + "/" + address[5].ToString());
                        strBuild.Append("</td></tr>");
                        //strBuild.Append("<tr><td align='left'>Riya Travel & Tours (I) Pvt Ltd</td></tr><tr><td align='left'>7TH FLR, SAI ENCLAVE</td></tr><tr><td align='left'>VIKHROLI,MUMBAI</td></tr>");
                        //<tr><td align='left'>Service Tax Reg. No. AAACR3178BSD028</td></tr>
                        strBuild.Append(" <tr><td align='left'>FAX No. :</td></tr>");

                        if (InvoiceDetails.Tables[0].Columns.Contains("AGENT_GSTNO") && !string.IsNullOrEmpty(InvoiceDetails.Tables[0].Rows[0]["AGENT_GSTNO"].ToString()))
                            strBuild.Append("<tr><td align='left' style='height: auto; font-family: Verdana; font-size: 11px; '>GSTN : " + InvoiceDetails.Tables[0].Rows[0]["AGENT_GSTNO"].ToString() + "</td></tr>");

                        strBuild.Append("</table></td>");
                        strBuild.Append("<td width='50%'><table width='100%' style='font-family: Verdana; font-size: 11px; font-weight: bold'>");

                        var qryPaxIn = (from p in dtTempInvoice.AsEnumerable()
                                        where (p.Field<string>("PaxType").ToString().ToUpper().Trim() == "ADT" ||
                                               p.Field<string>("PaxType").ToString().ToUpper().Trim() == "ADULT")
                                        select new
                                        {
                                            Title = p.Field<string>("Title"),
                                            PaxType = p.Field<string>("PaxType"),
                                            FirstName = p.Field<string>("FirstName"),
                                            LastName = p.Field<string>("LastName"),
                                        }).Distinct();
                        DataTable dttravellers = Serv.ConvertToDataTable(qryPaxIn);
                        strBuild.Append("<tr align='left'><td>Bill&nbsp;To&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + dttravellers.Rows[0]["Title"].ToString() + " . " + dttravellers.Rows[0]["FirstName"].ToString() + " " + dttravellers.Rows[0]["LastName"].ToString() + "</td></tr><tr><td style='font-family: Verdana; font-size: 3px; font-weight: bold'>&nbsp; &nbsp;</td></tr>");
                        strBuild.Append("<tr align='left'><td>E-Mail&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + InvoiceDetails.Tables[0].Rows[0]["EMAIL_ID"].ToString() + "</td></tr><tr><td style='font-family: Verdana; font-size: 3px; font-weight: bold'>&nbsp; &nbsp;</td></tr>");
                        strBuild.Append("<tr align='left'><td>Phone&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp; " + InvoiceDetails.Tables[0].Rows[0]["CONTACT_NO"].ToString() + "</td></tr><tr><td style='font-family: Verdana; font-size: 3px; font-weight: bold'>&nbsp; &nbsp;</td></tr>");
                        //if ((InvoiceDetails.Tables[0].Rows[0]["MO_NUMBER"] != null && InvoiceDetails.Tables[0].Rows[0]["MO_NUMBER"].ToString() != ""))
                        //{
                        //    strBuild.Append("<tr align='left'><td>Address&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + InvoiceDetails.Tables[0].Rows[0]["ADDRESS"].ToString() + "</td></tr><tr><td style='font-family: Verdana; font-size: 3px; font-weight: bold'>&nbsp; &nbsp;</td></tr>");
                        //    strBuild.Append("<tr align='left'><td>MO&nbsp;Number&nbsp;&nbsp;:&nbsp;" + InvoiceDetails.Tables[0].Rows[0]["MO_NUMBER"].ToString() + "</td></tr><tr><td style='font-family: Verdana; font-size: 3px; font-weight: bold'>&nbsp; &nbsp;</td></tr></table></td></tr></table><br />");
                        //}
                        //else
                        strBuild.Append("<tr align='left'><td>Address&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;" + InvoiceDetails.Tables[0].Rows[0]["ADDRESS"].ToString() + "</td></tr><tr><td style='font-family: Verdana; font-size: 3px; font-weight: bold'>&nbsp; &nbsp;</td></tr></table></td></tr></table><br />");

                        strBuild.Append("<table width='100%' class='sample' border='1' style='font-family: Verdana; font-size: 11px;border-collapse: collapse;font-weight: bold; border-style:ridge; border-color:black;'>");
                        strBuild.Append("<tr style='background-color:#8C8C87;'><td>Air Details</td></tr><tr style='background-color:#8C8C87;'><td>PNR NO</td><td>Airline Code</td><td>From City</td><td>To City</td><td>Class</td><td>Departure Date</td><td>Dep.Time</td><td>Arrival Date</td><td>Arr. Time</td></tr>");

                        DataTable dttravelingDetails = new DataTable();
                        var qrySeg = (from p in InvoiceDetails.Tables[0].AsEnumerable()
                                      select new
                                      {
                                          S_PNR = p.Field<string>("S_PNR"),
                                          FLIGHT_NO = p.Field<string>("FLIGHT_NO"),
                                          SEGMENT_NO = p.Field<Int16>("SEGMENT_NO"),
                                          //PAX_REF_NO = p.Field<string>("PAX_REF_NO"),
                                          ORIGINCODE = p.Field<string>("ORIGIN"),
                                          DESTINATIONCODE = p.Field<string>("DESTINATION"),
                                          AIRLINE_PNR = p.Field<string>("AIRLINE_PNR"),
                                          CRS_PNR = p.Field<string>("CRS_PNR"),
                                          DEPT_DATE = p.IsNull("DEPT_DATE") ? (Nullable<DateTime>)null : p.Field<DateTime>("DEPT_DATE"),
                                          ARRIVAL_DATE = p.IsNull("ARRIVAL_DATE") ? (Nullable<DateTime>)null : p.Field<DateTime>("ARRIVAL_DATE"),
                                          CLASS_ID = p.Field<string>("CLASS_ID"),
                                          CLASS_TYPE = p.Field<string>("CLASS_TYPE"),
                                          AIRLINE_CATEGORY = p.Field<string>("AIRLINE_CATEGORY"),
                                          ORIGIN_CODE = p.Field<string>("ORIGINCODE"),
                                          DESTINATION_CODE = p.Field<string>("DESTINATIONCODE"),
                                          FARE_BASIS = p.Field<string>("FARE_BASIS"),
                                          PLATING_CARRIER = p.Field<string>("PLATING_CARRIER")
                                      }).Distinct();
                        dttravelingDetails = Serv.ConvertToDataTable(qrySeg);


                        string classtype = string.Empty;
                        for (int i = 0; i < dttravelingDetails.Rows.Count; i++)
                        {
                            strBuild.Append("<tr><td>" + dttravelingDetails.Rows[i]["AIRLINE_PNR"].ToString() + "</td>");
                            strBuild.Append("<td>" + dttravelingDetails.Rows[i]["PLATING_CARRIER"].ToString() + "</td>");
                            strBuild.Append("<td>" + dttravelingDetails.Rows[i]["ORIGIN_CODE"].ToString() + "</td>");
                            strBuild.Append("<td>" + dttravelingDetails.Rows[i]["DESTINATION_CODE"].ToString() + "</td>");

                            if (dttravelingDetails.Rows[i]["CLASS_TYPE"].ToString() == "B")
                            {
                                classtype = "Business";
                            }
                            else if (dttravelingDetails.Rows[i]["CLASS_TYPE"].ToString() == "F")
                            {
                                classtype = "First Class";
                            }
                            else
                            {
                                classtype = "Economy";
                            }

                            strBuild.Append("<td>" + classtype.ToString() + "</td>");
                            var date = CultureInfoToConvertDatetimeFormat(Convert.ToDateTime(dttravelingDetails.Rows[i]["DEPT_DATE"].ToString()), "dd/MM/yyyy HH:mm");
                            var datechange = date.Split(' ');
                            strBuild.Append("<td>" + datechange[0] + "</td>");
                            strBuild.Append("<td>" + datechange[1] + "</td>");
                            var Arrdate = CultureInfoToConvertDatetimeFormat(Convert.ToDateTime(dttravelingDetails.Rows[i]["ARRIVAL_DATE"].ToString()), "dd/MM/yyyy HH:mm");
                            var Arrdatechange = Arrdate.Split(' ');
                            strBuild.Append("<td>" + Arrdatechange[0] + "</td>");
                            strBuild.Append("<td>" + Arrdatechange[1] + "</td></tr>");
                        }

                        strBuild.Append("</table><br />");

                        strBuild.Append("<table border='1' width='100%' class='sample' style='font-family: Verdana; border-style:ridge; border-color:black;border-collapse: collapse; font-size: 11px;font-weight: bold'>"); //<td>Commission</td><td>Surcharge Amt. </td>
                        strBuild.Append("<tr style='background-color:#8C8C87;'><td>Ticket Details</td></tr>");
                        //if (Session["agenttype"].ToString().Trim().ToUpper() == "RI")
                        //    strBuild.Append("<tr style='background-color:#8C8C87;'><td align='center'>Passenger Name</td><td align='center'>Sector Name</td><td align='center'> Base Amt.</td><td align='center'>Tax Amt.</td><td align='center'>Mgmt. Fee</td><td align='center'>TDS Amt</td><td align='center'>Service Charges</td>");
                        //else
                        strBuild.Append("<tr style='background-color:#8C8C87;'><td align='center'>Passenger Name</td><td align='center'>Sector Name</td><td align='center'> Base Amt.</td><td align='center'>Tax Amt.</td><td align='center'>Mgmt. Fee</td>");


                        int flag = 0;
                        if (!string.IsNullOrEmpty(dtTempInvoice.Rows[0]["INCENTIVE_AMT"].ToString()) && Session["agenttype"].ToString().Trim().ToUpper() == "RI")
                        {
                            strBuild.Append("<td align='center'>Commission</td>");
                            flag = 1;
                        }
                        strBuild.Append("<td align='center'>Total</td></tr>");

                        DataTable dtTicket = new DataTable();
                        dtTicket = InvoiceDetails.Tables[0].Copy();
                        string tripType = "0";
                        var qryTripType = (from p in dtTicket.AsEnumerable()
                                           select new
                                           {
                                               SeqNo = p.Field<string>("TRIP_DESC")
                                           }).Distinct();
                        if (qryTripType.Count() > 1)
                            tripType = "R";
                        if (qryTripType.Count() == 1)
                        {
                            foreach (DataRow drRows in InvoiceDetails.Tables[0].Rows)
                            {
                                drRows["TRIP_DESC"] = "O";
                            }
                        }
                        string taxCodeAdult = string.Empty;
                        string taxCodeChild = string.Empty;
                        string taxCodeInfant = string.Empty;
                        string taxCode = string.Empty;
                        DataTable dtPassengerDetails = InvoiceDetails.Tables[0].Copy();
                        DataTable dtTempRway = new DataTable();
                        DataTable dtTempOnway = new DataTable();
                        DataTable dtTempPaxDet = new DataTable();
                        int countOneway = 0;
                        int countRoundway = 0;
                        var qryPaxInfoDetails = (from p in dtPassengerDetails.AsEnumerable()
                                                 where p.Field<string>("TRIP_DESC") == "O"
                                                 select new
                                                 {
                                                     Title = p.Field<string>("PASSENGER_TITLE"),
                                                     PaxType = p.Field<string>("PASSENGER_TYPE"),
                                                     ContactNo = p.Field<string>("CONTACT_NO"),
                                                     FirstName = p.Field<string>("FIRST_NAME"),
                                                     LastName = p.Field<string>("LAST_NAME"),
                                                     MEALSAMOUNT = p.Field<decimal?>("MEALS_AMOUNT"),
                                                     TAXCODE = p.Field<string>("TAXCODE"),
                                                     PAX_REF_NO = p.Field<Int16>("PAX_REF_NO"),
                                                     PLATING_CARRIER = p.Field<string>("PLATING_CARRIER"),
                                                     TRIP_DESC = p.Field<string>("TRIP_DESC")
                                                 }).Distinct();
                        dtTempPaxDet = Serv.ConvertToDataTable(qryPaxInfoDetails);
                        dtTempOnway = dtTempPaxDet.Copy();
                        if (dtTempPaxDet != null)
                            countOneway = dtTempOnway.Rows.Count;
                        if (tripType == "R")
                        {
                            var qryPaxInfosRway = (from p in dtPassengerDetails.AsEnumerable()
                                                   where p.Field<string>("TRIP_DESC").ToString().ToUpper().Trim() == "R"
                                                   select new
                                                   {
                                                       Title = p.Field<string>("PASSENGER_TITLE"),
                                                       PaxType = p.Field<string>("PASSENGER_TYPE"),
                                                       ContactNo = p.Field<string>("CONTACT_NO"),
                                                       FirstName = p.Field<string>("FIRST_NAME"),
                                                       LastName = p.Field<string>("LAST_NAME"),
                                                       MEALSAMOUNT = p.Field<decimal?>("MEALS_AMOUNT"),
                                                       TAXCODE = p.Field<string>("TAXCODE"),
                                                       PAX_REF_NO = p.Field<Int16>("PAX_REF_NO"),
                                                       PLATING_CARRIER = p.Field<string>("PLATING_CARRIER"),
                                                       TRIP_DESC = p.Field<string>("TRIP_DESC")

                                                   }).Distinct();
                            dtTempRway = Serv.ConvertToDataTable(qryPaxInfosRway);
                            countRoundway = dtTempRway.Rows.Count;
                        }
                        if (countOneway <= countRoundway)
                        {
                            foreach (DataRow drRway in dtTempRway.Rows)
                            {
                                int count2 = 0;
                                int countOne = Convert.ToInt32(drRway["PAX_REF_NO"].ToString()) - dtTempRway.Rows.Count;
                                foreach (DataRow drOwayRows in dtTempOnway.Rows)
                                {
                                    if (countOne.ToString().Contains(drOwayRows["PAX_REF_NO"].ToString()))
                                    {
                                        count2 = 1;
                                        break;
                                    }
                                }
                                if (count2 != 1)
                                {
                                    foreach (DataRow drRwayRows in dtTempRway.Rows)
                                    {
                                        if (drRwayRows["PAX_REF_NO"].ToString() == (countOne + dtTempRway.Rows.Count).ToString())
                                            drRwayRows["TRIP_DESC"] = "O";
                                    }
                                }
                            }
                            dtTempOnway.Merge(dtTempRway.Copy());
                            dtTempPaxDet = new DataTable();
                            dtTempPaxDet = dtTempOnway.Copy();
                        }


                        var qryTicketDetails = (from p in dtTicket.AsEnumerable()
                                                select new
                                                {
                                                    Title = p.Field<string>("PASSENGER_TITLE"),
                                                    PaxType = p.Field<string>("PASSENGER_TYPE"),
                                                    // ContactNo = p.Field<string>("CONTACT_NO"),
                                                    FirstName = p.Field<string>("FIRST_NAME"),
                                                    LastName = p.Field<string>("LAST_NAME"),
                                                    TaxAmount = p.Field<decimal?>("TAXAMOUNT"),
                                                    //  MEALS = p.Field<string>("MEALS"),                                           
                                                    MEALSAMOUNT = p.Field<decimal?>("MEALS_AMOUNT"),
                                                    SERVICECHARGE = p.Field<decimal?>("SERVICECHARGEORG"),
                                                    TRANSACTION_FEE = p.Field<decimal?>("TRANS_FEE"),
                                                    SERVICE_TAX = p.Field<decimal>("SERVICE_TAX"),
                                                    TAXCODE = p.Field<string>("TAXCODE"),
                                                    BasicFare = p.Field<decimal?>("BASICFAREAMOUNT"),
                                                    PAX_REF_NO = p.Field<Int16>("PAX_REF_NO"),
                                                    PREVIOUS_TICKET = p.Field<string>("PREVIOUS_TICKET_NO"),
                                                    SUPPILER_PENALITY = p.Field<decimal?>("SUPPILER_PENALITY"),
                                                    AGENT_PENALITY = p.Field<decimal?>("AGENT_PENALITY"),
                                                    FARE_DIFFERENCE = p.Field<decimal?>("FARE_DIFFERENCE"),
                                                    TRIP_DESC = p.Field<string>("TRIP_DESC"),
                                                    AIRLINE_PNR = p.Field<string>("AIRLINE_PNR"),
                                                    ORIGINCODE = p.Field<string>("ORIGINCODE"),
                                                    DESTINATIONCODE = p.Field<string>("DESTINATIONCODE"),
                                                    PLATING_CARRIER = p.Field<string>("PLATING_CARRIER"),
                                                    DEPT_DATE = p.Field<DateTime>("DEPT_DATE"),
                                                    ARRIVAL_DATE = p.Field<DateTime>("ARRIVAL_DATE"),
                                                    CLASS_TYPE = p.Field<string>("CLASS_TYPE"),
                                                    PCI_TDS_AMOUNT = p.Field<decimal?>("PCI_TDS_AMOUNT"),
                                                    PLB_TDS_AMOUNT = p.Field<decimal?>("PLB_TDS_AMOUNT"),
                                                    INCENTIVE_AMT = p.Field<decimal?>("PCI_COMMISION_AMT"),
                                                    BAGGAGE_AMOUNT = p.Field<decimal?>("BAGGAGE_AMOUNT"),
                                                    //   COMMISSION_AMT = p.Field<string>("COMMISSION_AMT"),
                                                    SSR_AMOUNT = p.Field<decimal?>("SSR_AMOUNT"),
                                                    ISSUED_DATE = p.Field<string>("BOOKED_DATE"),
                                                    SEAT_AMOUNT = p.Field<decimal?>("SEAT_AMOUNT"),
                                                    PLB_AMOUNT = p.Field<decimal?>("PLB_AMOUNT"),
                                                    SF_AMT = p.Field<decimal?>("PCI_SF"),
                                                    SF_GST = p.Field<decimal?>("PCI_SF_GST"),
                                                    MARKUP = p.Field<decimal?>("PAI_TKT_MANAGEMENT_FEE"),
                                                    SERVICE_FEE = p.Field<decimal>("SERVICE_FEE"),
                                                    //SSR_AMOUNT = p.Field<decimal>("SSR_AMOUNT"),
                                                    //   SEAT_AMOUNT = p.Field<decimal>("SEAT_AMOUNT")
                                                }).Distinct();
                        DataTable dtFareDetails = Serv.ConvertToDataTable(qryTicketDetails);


                        int totalPaxCount = 0;
                        var qryPaxCount = (from p in dtTicket.AsEnumerable()
                                           select new
                                           {
                                               PAX_REF_NO = p.Field<Int16>("PAX_REF_NO")
                                           }).Distinct();
                        totalPaxCount = qryPaxCount.Count();

                        int paxCount1 = 0;
                        if (tripType.ToString() == "R")
                        {
                            paxCount1 = totalPaxCount / 2;
                        }
                        int currentPaxNo1 = 0;
                        int NextPaxNo1 = 0;
                        DataTable dtPaxnames1 = new DataTable();
                        string addedTaxBreakup = string.Empty;
                        for (int i = 0; i < dtTempPaxDet.Rows.Count; i++)
                        {

                            decimal basicFare = 0, taxamt = 0, serviestax = 0, serviechargeAmt = 0, TransactionFee = 0, grossFare = 0, Commision = 0, TDS_Amount = 0, Discount = 0, sfgst = 0, sftax = 0, mark_up = 0;
                            decimal decMealsAmount = 0, sumMealAmt = 0, decSeatAmt = 0;
                            decimal decBaggageAmount = 0, PaxEarning = 0, PaxOthersAmount = 0, PaxServiceFee = 0;
                            currentPaxNo1 = Convert.ToInt32(dtTempPaxDet.Rows[i]["PAX_REF_NO"].ToString());
                            if (tripType.ToString() == "R")
                            {
                                NextPaxNo1 = (paxCount1) + currentPaxNo1;
                            }
                            int[] paxRefNo = { currentPaxNo1, NextPaxNo1 };

                            var Temp = from p in dtFareDetails.AsEnumerable()
                                       where paxRefNo.Contains(p.Field<Int16>("PAX_REF_NO"))
                                       select p;
                            DataView dvPaxFareDetails = Temp.AsDataView();
                            if (dvPaxFareDetails.Count <= 0)
                                continue;
                            DataTable dtPaxFareDetails = Temp.CopyToDataTable();


                            bool setOnward = false;
                            bool setReturn = false;
                            decimal TotalTaxAmount = 0;
                            for (int j = 0; j < dtPaxFareDetails.Rows.Count; j++)
                            {
                                if ((paxRefNo[0].ToString() == "0" || string.IsNullOrEmpty(paxRefNo[0].ToString()))
                                        && (paxRefNo[1].ToString() == "0" || string.IsNullOrEmpty(paxRefNo[1].ToString())))
                                    continue;

                                if ((setOnward == true && dtPaxFareDetails.Rows[j]["PAX_REF_NO"].ToString().Trim().ToUpper() == currentPaxNo1.ToString()) ||
                                    (setReturn == true && dtPaxFareDetails.Rows[j]["PAX_REF_NO"].ToString().Trim().ToUpper() == NextPaxNo1.ToString()))
                                    continue;

                                if (dtPaxFareDetails.Rows[j]["PAX_REF_NO"].ToString().Trim().ToUpper() == NextPaxNo1.ToString())
                                {
                                    setReturn = true;
                                    paxRefNo[1] = 0;
                                }
                                else if (dtPaxFareDetails.Rows[j]["PAX_REF_NO"].ToString().Trim().ToUpper() == currentPaxNo1.ToString())
                                {
                                    setOnward = true;
                                    paxRefNo[0] = 0;
                                }

                                string[] TotalOnwardReturnsMealsAmt = dtPaxFareDetails.Rows[j]["MealsAmount"].ToString().Split('|');
                                for (int index = 0; index < TotalOnwardReturnsMealsAmt.Length; index++)
                                {
                                    decMealsAmount += Convert.ToDecimal(TotalOnwardReturnsMealsAmt[index].ToString());
                                    sumMealAmt += Convert.ToDecimal(TotalOnwardReturnsMealsAmt[index].ToString());
                                }
                                string[] TotalOnwardReturnsBaggageAmt = dtPaxFareDetails.Rows[j]["BAGGAGE_AMOUNT"].ToString().Split('|');
                                for (int index = 0; index < TotalOnwardReturnsBaggageAmt.Length; index++)
                                {
                                    decBaggageAmount += Convert.ToDecimal(TotalOnwardReturnsBaggageAmt[index].ToString());
                                }

                                if (dtPaxFareDetails.Rows[j]["SERVICECHARGE"].ToString() == "-1")
                                {
                                    dtPaxFareDetails.Rows[j]["SERVICECHARGE"] = 0;
                                }

                                PaxOthersAmount += Convert.ToDecimal(dtPaxFareDetails.Rows[j]["SSR_AMOUNT"].ToString());
                                PaxServiceFee += Convert.ToDecimal(dtPaxFareDetails.Rows[j]["SERVICE_FEE"].ToString());
                                decSeatAmt += Convert.ToDecimal(dtPaxFareDetails.Rows[j]["SEAT_AMOUNT"].ToString());

                                basicFare += Convert.ToDecimal(dtPaxFareDetails.Rows[j]["BASICFARE"].ToString());
                                taxamt += Convert.ToDecimal(dtPaxFareDetails.Rows[j]["TaxAmount"].ToString());
                                serviestax += Convert.ToDecimal(dtPaxFareDetails.Rows[j]["SERVICE_TAX"].ToString());
                                serviechargeAmt += dtPaxFareDetails.Rows[j]["SERVICECHARGE"].ToString() != "" ? Convert.ToDecimal(dtPaxFareDetails.Rows[j]["SERVICECHARGE"].ToString()) : 0;
                                PaxEarning += Convert.ToDecimal(dtPaxFareDetails.Rows[j]["PLB_AMOUNT"].ToString());
                                addedTaxBreakup = TaxBreakUpOTChargesIntegration(dtPaxFareDetails.Rows[j]["TAXCODE"].ToString(), addedTaxBreakup);
                                sfgst += Convert.ToDecimal(dtPaxFareDetails.Rows[j]["SF_GST"].ToString());
                                mark_up += Convert.ToDecimal(dtPaxFareDetails.Rows[j]["MARKUP"].ToString());
                                sftax += Convert.ToDecimal(dtFareDetails.Rows[j]["SF_AMT"].ToString());
                                TDS_Amount += Convert.ToDecimal(dtFareDetails.Rows[j]["PCI_TDS_AMOUNT"].ToString());
                            }

                            //TotalTaxAmount = taxamt + sumMealAmt + decBaggageAmount + SeatAmount + PaxOthersAmount + PaxServiceFee + sfgst + sftax + TDS_Amount + markup;
                            TotalTaxAmount = taxamt + sumMealAmt + decBaggageAmount + decSeatAmt + PaxOthersAmount + PaxServiceFee + sfgst + sftax + markup;
                            TotalSSRAmount += sumMealAmt + decBaggageAmount + decSeatAmt + PaxOthersAmount;

                            if (Session["agenttype"].ToString().Trim().ToUpper() == "RI")
                            {
                                //grossFare = (basicFare + taxamt + serviechargeAmt + sumMealAmt + decBaggageAmount + SeatAmount + serviestax + PaxOthersAmount + PaxServiceFee + sfgst + sftax + TDS_Amount + mark_up) - (Discount);
                                grossFare = (basicFare + taxamt + serviechargeAmt + sumMealAmt + decBaggageAmount + decSeatAmt + PaxOthersAmount + PaxServiceFee + sfgst + sftax + mark_up) - (Discount);
                            }
                            else if (Session["agenttype"].ToString().Trim().ToUpper() == "RE" && InvoiceDetails.Tables[0].Rows[0]["PAYMENT_MODE"].ToString().ToUpper().Trim() == "P")
                            {
                                //grossFare = (basicFare + taxamt + serviechargeAmt + sumMealAmt + decBaggageAmount + SeatAmount + serviestax + PaxOthersAmount + PaxServiceFee + sfgst + sftax + TDS_Amount + mark_up) - (PaxEarning);
                                grossFare = (basicFare + taxamt + serviechargeAmt + sumMealAmt + decBaggageAmount + decSeatAmt + PaxOthersAmount + PaxServiceFee + sfgst + sftax + mark_up) - (PaxEarning);
                                TotalTaxAmount = TotalTaxAmount - PaxEarning;
                            }
                            else
                            {
                                //grossFare = (basicFare + taxamt + serviechargeAmt + sumMealAmt + decBaggageAmount + SeatAmount + serviestax + PaxOthersAmount + PaxServiceFee + sfgst + sftax + TDS_Amount + mark_up);

                                grossFare = (basicFare + taxamt + serviechargeAmt + sumMealAmt + decBaggageAmount + decSeatAmt + PaxOthersAmount + PaxServiceFee + sfgst + sftax + mark_up);
                            }

                            DataRow[] dataRow;
                            dataRow = dtFareDetails.Select("PAX_REF_NO in ('" + currentPaxNo1 + "','" + NextPaxNo1 + "')");
                            foreach (DataRow drRemove in dataRow)
                                dtFareDetails.Rows.Remove(drRemove);

                            // for sector wise
                            var qryCheckRoundTrip = (from p in InvoiceDetails.Tables[0].AsEnumerable()
                                                     group p by p.Field<string>("TRIP_DESC") into n
                                                     select new { test = n.Key }).Distinct();
                            int count = qryCheckRoundTrip.Count();
                            if (count == 2)
                            {
                                Session["DiscountTrip"] = "R";
                            }
                            else
                            {
                                Session["DiscountTrip"] = "O";
                            }
                            DataTable dtAirDetail = new DataTable();
                            var qryPass = (from p in InvoiceDetails.Tables[0].AsEnumerable()
                                           select new
                                           {
                                               AIRPORTID = p.Field<string>("AIRPORTID"),
                                               FLIGHTNO = p.Field<string>("FLIGHT_NO"),
                                               ORIGIN = p.Field<string>("ORIGIN"),
                                               DESTINATION = p.Field<string>("DESTINATION"),
                                               ORIGINCODE = p.Field<string>("ORIGINCODE"),
                                               DESTINATIONCODE = p.Field<string>("DESTINATIONCODE"),
                                               S_PNR = p.Field<string>("S_PNR"),
                                               TRIP_DESC = p.Field<string>("TRIP_DESC")
                                           }).Distinct();

                            dtAirDetail = Serv.ConvertToDataTable(qryPass);
                            string strSector = string.Empty;
                            if (Session["DiscountTrip"].ToString() == "R")
                            {

                                var temp = from p in dtAirDetail.AsEnumerable()
                                           where p.Field<string>("TRIP_DESC").ToString() == "O"
                                           select p;
                                DataTable dtOrigin = temp.CopyToDataTable();

                                var temp1 = from p in dtAirDetail.AsEnumerable()
                                            where p.Field<string>("TRIP_DESC").ToString() == "R"
                                            select p;
                                DataTable dtOrigin1 = temp1.CopyToDataTable();

                                strSector = dtOrigin.Rows[0]["ORIGINCODE"].ToString() + " --> " + dtOrigin.Rows[dtOrigin.Rows.Count - 1]["DESTINATIONCODE"].ToString()
                                                + " --> " + dtOrigin1.Rows[dtOrigin1.Rows.Count - 1]["DESTINATIONCODE"].ToString();
                            }
                            else
                            {
                                if (InvoiceDetails.Tables[0].Rows[0]["SPECIAL_TRIP"].ToString().ToUpper().Trim() == "S" ||
                                    (InvoiceDetails.Tables[0].Rows[0]["SPECIAL_TRIP"].ToString().ToUpper().Trim() == "R" &&
                                     InvoiceDetails.Tables[0].Rows[0]["AIRPORTID"].ToString().ToUpper().Trim() == "I"))
                                {
                                    strSector = dtAirDetail.Rows[0]["ORIGINCODE"].ToString() + " --> " + dtAirDetail.Rows[0]["DESTINATIONCODE"].ToString()
                                        + " --> " + dtAirDetail.Rows[0]["ORIGINCODE"].ToString();
                                }
                                else
                                    strSector = dtAirDetail.Rows[0]["ORIGINCODE"].ToString() + " --> " + dtAirDetail.Rows[dtAirDetail.Rows.Count - 1]["DESTINATIONCODE"].ToString();
                            }
                            strBuild.Append("<tr ><td align='left'>" + dtTempPaxDet.Rows[i]["Title"].ToString() + " . " + dtTempPaxDet.Rows[i]["FirstName"].ToString() +
                                        " " + dtTempPaxDet.Rows[i]["LastName"].ToString() + "(" + dtTempPaxDet.Rows[i]["PaxType"].ToString() + ")" + "</td>");

                            strBuild.Append("<td>" + strSector + "</td>");
                            strBuild.Append("<td align='right'>" + basicFare.ToString("0.00", CultureInfo.InvariantCulture) + "</td>");

                            if (Session["agenttype"].ToString().Trim().ToUpper() == "RI")
                                strBuild.Append("<td align='right'>" + TotalTaxAmount.ToString("0.00", CultureInfo.InvariantCulture) + "</td>");
                            else
                                //strBuild.Append("<td align='right'>" + (TotalTaxAmount + serviestax).ToString("0.00", CultureInfo.InvariantCulture) + "</td>");
                                strBuild.Append("<td align='right'>" + (TotalTaxAmount).ToString("0.00", CultureInfo.InvariantCulture) + "</td>");

                            strBuild.Append("<td align='right'>" + serviechargeAmt.ToString("0.00", CultureInfo.InvariantCulture) + "</td>");
                            if (Session["agenttype"].ToString().Trim().ToUpper() == "RI")
                            {
                                strBuild.Append("<td align='right'>" + Discount.ToString("0.00", CultureInfo.InvariantCulture) + "</td>");
                            }
                            strBuild.Append("<td align='right'>" + grossFare.ToString("0.00", CultureInfo.InvariantCulture) + "</td></tr>");

                        }

                        strBuild.Append("<tr><td>&nbsp; &nbsp;</td>");
                        strBuild.Append("<td>&nbsp; &nbsp;</td>");
                        strBuild.Append("<td align='right'>" + totalBasicAmt.ToString("0.00", CultureInfo.InvariantCulture) + "</td>");

                        strBuild.Append("<td align='right'>" + (totalServiceTax + totaltaxamt).ToString("0.00", CultureInfo.InvariantCulture) + "</td>");

                        strBuild.Append("<td align='right'> " + ServiceCharge.ToString("0.00", CultureInfo.InvariantCulture) + "</td>");
                        if (Session["agenttype"].ToString().Trim().ToUpper() == "RI")
                        {
                            strBuild.Append("<td align='right'>" + totalDiscount.ToString("0.00", CultureInfo.InvariantCulture) + "</td>");

                        }
                        strBuild.Append("<td align='right'>" + grosstotal.ToString("0.00", CultureInfo.InvariantCulture) + "</td></tr>");

                        strBuild.Append("</table><br />");

                        strBuild.Append("<table width='100%'  style='font-family: Verdana; font-size: 10px; font-weight: bold'>");
                        strBuild.Append("<tr>");
                        strBuild.Append("<td style='width:200px'><span style='white-space:nowrap'>Taxes & Other Charges: " + formBreakup(addedTaxBreakup, "INVOICE") + (TotalSSRAmount > 0 ? "SSR:" + TotalSSRAmount : "") + (ServiceFee > 0 ? "; SFEE:" + ServiceFee : "") + "</span></td>");
                        strBuild.Append("</tr>");
                        strBuild.Append("</table>");

                        strBuild.Append("<table  width='100%' style='font-family: Verdana; font-size: 11px;'><tr align='left'><td>1) Account Payee cheque should be drawn in favour of 'RIYA TRAVEL & TOURS (INDIA) PVT. LTD.'</td></tr>");

                        strBuild.Append("<tr align='left'><td>2) Interest @ 18% p.a will be charged on all overdue payments.</td></tr>");
                        strBuild.Append("<tr align='left'><td>3) Should you have any queries or dispute on the invoice, Please send the details within 7 days of receipt of the invoice otherwise we consider it as accepted.</td></tr>");
                        strBuild.Append("<tr align='left'><td>4) This is a computer generated invoice does not require signature/stamp. </td></tr> </table>");
                        strBuild.Append("</body></html>");
                        Session["strBuild"] = strBuild.ToString();
                        ViewBag.Errormsg = "";
                    }
                    else
                    {
                        // lblError.Text = "Invoice History is not available for your filteration";
                        ViewBag.Errormsg = "Unable to get invoice details. Please contact support team (#03)";
                        Session["strBuild"] = "";
                    }
                }
                else
                {
                    ViewBag.Errormsg = "Unable to get invoice details. Please contact support team (#03)";
                    Session["strBuild"] = "";
                }
            }
            catch (Exception Ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "FormingInvoiceDetails", "PerformaInvoice", Ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                ViewBag.Errormsg = "Unable to get invoice details. Please contact support team (#05)";
                Session["strBuild"] = "";
            }
            return View("~/Views/AfterBooking/FormingInvoiceDetails.cshtml");

        }

        public static string formBreakup(string addedTaxBreakup, string strPrint)
        {

            string strBreakup = string.Empty;
            try
            {
                string[] splitAddedTax = addedTaxBreakup.Split('/');
                foreach (string addedTax in splitAddedTax)
                {
                    if (!string.IsNullOrEmpty(addedTax))
                    {
                        string taxCodeAdded = addedTax.ToString().Substring(0, addedTax.ToString().LastIndexOf(':')).Trim().ToUpper();

                        if (strPrint.ToString().ToUpper().Trim() == "INVOICE")
                        {
                            if (!(taxCodeAdded == "BF"))
                            {
                                strBreakup += addedTax + "; ";
                            }
                        }
                        else if (!(taxCodeAdded == "BF" || taxCodeAdded == "YQ" || taxCodeAdded == "K3" || taxCodeAdded == "JN"))
                        {
                            strBreakup += addedTax + "; ";
                        }

                    }
                }

            }
            catch (Exception ex)
            {
            }
            return strBreakup;

        }

        private string CultureInfoToConvertDatetimeFormat(DateTime dateTimeTemp, string format)
        {
            CultureInfo cii = new CultureInfo("en-GB", true);
            string requiredDate = dateTimeTemp.ToString(format, cii);
            return requiredDate;
        }

        public static string TaxBreakUpOTChargesIntegration(string currenttaxAmount, string addedTaxBreakup)
        {
            // Adult BreakUp


            string[] splitCurrentTaxs = currenttaxAmount.Split('|');
            string[] splitAddedTaxs = addedTaxBreakup.Split('|');
            string tax = string.Empty;

            if (string.IsNullOrEmpty(currenttaxAmount))
            {
                tax = addedTaxBreakup;
                return tax;
            }
            if (string.IsNullOrEmpty(addedTaxBreakup))
            {
                tax = currenttaxAmount.ToString() + "/";
                return tax;
            }
            for (int i = 0; i < splitCurrentTaxs.Length; i++)
            {
                string[] splitCurrentTax = splitCurrentTaxs[i].Split('/');
                string[] splitAddedTax = splitAddedTaxs[i].Split('/');

                foreach (string currentTax in splitCurrentTax)
                {
                    if (!string.IsNullOrEmpty(currentTax))
                    {
                        string deleteTax = string.Empty;
                        decimal fareCuurent = 0;
                        decimal fareAdded = 0;
                        string taxCodeCurrent = currentTax.ToString().Substring(0, currentTax.ToString().LastIndexOf(':'));
                        fareCuurent = Convert.ToDecimal(currentTax.ToString().Substring(currentTax.ToString().LastIndexOf(':') + 1));
                        if (splitAddedTax.Length != 0 && !string.IsNullOrEmpty(splitAddedTax[0].ToString()))
                        {
                            foreach (string addedTax in splitAddedTax)
                            {
                                if (!string.IsNullOrEmpty(addedTax))
                                {
                                    string taxCodeAdded = addedTax.ToString().Substring(0, addedTax.ToString().LastIndexOf(':'));
                                    if (taxCodeCurrent.ToString().ToUpper().Trim() == taxCodeAdded.ToString().ToUpper().Trim())
                                    {
                                        deleteTax = addedTax.ToString();
                                        fareAdded = Convert.ToDecimal(addedTax.ToString().Substring(addedTax.ToString().LastIndexOf(':') + 1));
                                    }
                                }
                            }
                        }
                        int numIndex = Array.IndexOf(splitAddedTax, deleteTax);
                        splitAddedTax = splitAddedTax.Where((val, idx) => idx != numIndex).ToArray();

                        // splitAddedTax = splitAddedTax.Where(x => !deleteTax.Contains(x)).ToArray();
                        tax += taxCodeCurrent + ":" + (fareCuurent + fareAdded).ToString() + "/";
                    }
                }

                if (splitAddedTax.Length != 0)
                {
                    if (!string.IsNullOrEmpty(splitAddedTax[0].ToString()))
                    {
                        foreach (string addedTax in splitAddedTax)
                        {
                            if (!string.IsNullOrEmpty(addedTax))
                            {
                                string taxCodeAdded = addedTax.ToString().Substring(0, addedTax.ToString().LastIndexOf(':'));
                                decimal fareExtra = Convert.ToDecimal(addedTax.ToString().Substring(addedTax.ToString().LastIndexOf(':') + 1));
                                tax += taxCodeAdded + ":" + (fareExtra) + "/";
                            }
                        }
                    }
                }

                tax = tax + "|";
            }
            tax = tax.TrimEnd('|');
            return tax;
        }
    }
}