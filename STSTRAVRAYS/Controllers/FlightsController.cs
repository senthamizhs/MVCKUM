using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using STSTRAVRAYS.Models;
using STSTRAVRAYS.Rays_service;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Globalization;
using System.Text;
using Seat_rays;
using System.Xml;
using STSTRAVRAYS.InplantService;

namespace STSTRAVRAYS.Controllers
{
    public class FlightsController : LoginController
    {
        BLL Bll = new BLL();
        RaysService _RayService = new RaysService();
        string strBranchCredit = ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"] != null ? ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"].ToString().ToUpper() : "";
        string strPlatform = ConfigurationManager.AppSettings["PLATFORM"] != null ? ConfigurationManager.AppSettings["PLATFORM"].ToString() : "";
        
        public ActionResult Flights()
        {
            #region UsageLOg
            string PageName = "Search Page";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e) { }
            #endregion

            if (ConfigurationManager.AppSettings["APP_HOSTING"].ToString() == "BSA")
            {
                if (Request["SECKEY"] != null && Request["SECKEY"].ToString() != "")
                {
                    string strB2CUserName = string.Empty, strB2CPassword = string.Empty, strB2CTerminalType = string.Empty;
                    string Encquery = Request.QueryString["SECKEY"];
                    string strKey = ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString();
                    string strQueryString = Base.DecryptKEY(Encquery, strKey);
                    string[] QueryString = strQueryString.ToString().Split('&');
                    for (int i = 0; i < QueryString.Length; i++)
                    {
                        string[] strData = QueryString[i].Split('=');
                        if (strData[0]== "USERNAME")
                        {
                            strB2CUserName = strData[1];
                        }
                        else if (strData[0] == "PWD")
                        {
                            strB2CPassword = strData[1];
                        }
                        else if (strData[0] == "TERMINALTYPE")
                        {
                            strB2CTerminalType = strData[1];
                        }
                        Session.Add(strData[0], strData[1]);
                    }
                    DataSet dsLogin = new DataSet();
                    _RayService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    dsLogin = _RayService.GET_LOGIN_DETAILS("", strB2CUserName, strB2CPassword, "W", "", "", "");
                    string strB2CTerminalID = string.Empty, strB2CAgentID = string.Empty;
                    if (dsLogin != null && dsLogin.Tables.Count > 0 && dsLogin.Tables[0].Rows.Count > 0)
                    {
                        if (dsLogin.Tables[0].Columns.Contains("ERROR"))
                        {
                            Session.Add("TERMINALTYPE", "M");
                            return RedirectToAction("SessionExp", "Redirect");
                        }
                        else
                        {
                            strB2CTerminalID = dsLogin.Tables[0].Rows[0]["LGN_TERMINAL_ID"].ToString().Trim().ToUpper();
                            strAgentID = dsLogin.Tables[0].Rows[0]["LGN_AGENT_ID"].ToString().Trim().ToUpper();
                            System.Web.HttpContext.Current.Session.Add("CustomerLogin", "Y");
                            bool msg = BSA_AssignSession(strB2CTerminalID, strB2CUserName, strB2CPassword, dsLogin, strB2CTerminalType);
                            string strProfilePIC = dsLogin.Tables[0].Rows[0]["LGN_PROFILE_PIC"].ToString().Trim();
                            string strUserFName = dsLogin.Tables[0].Rows[0]["LGN_AGENT_FIRSTNAME"].ToString().Trim();
                            string strUserLName = dsLogin.Tables[0].Rows[0]["LGN_AGENT_LASTNAME"].ToString().Trim();
                            string strMobileNO = dsLogin.Tables[0].Rows[0]["LGN_MOBILE_NO"].ToString().Trim().ToUpper();
                            System.Web.HttpContext.Current.Session.Add("TERMINALTYPE", strB2CTerminalType);
                            System.Web.HttpContext.Current.Session.Add("USERPROFILEPIC", strProfilePIC);
                            System.Web.HttpContext.Current.Session.Add("USERFNAME", strUserFName);
                            System.Web.HttpContext.Current.Session.Add("USERLNAME", strUserLName);
                            System.Web.HttpContext.Current.Session.Add("USERMOBILENO", strMobileNO);
                            System.Web.HttpContext.Current.Session.Add("USERNAME", strB2CUserName);
                            System.Web.HttpContext.Current.Session.Add("USERTITLE", dsLogin.Tables[0].Rows[0]["LGN_AGENT_TITLE"].ToString().Trim());
                            System.Web.HttpContext.Current.Session.Add("USERADDRESS", dsLogin.Tables[0].Rows[0]["LGN_ADDRESS_1"].ToString().Trim());
                            System.Web.HttpContext.Current.Session.Add("USERCITY", dsLogin.Tables[0].Rows[0]["LGN_CITY_ID"].ToString().Trim());
                            System.Web.HttpContext.Current.Session.Add("USERCOUNTRY", dsLogin.Tables[0].Rows[0]["LGN_COUNTRY_ID"].ToString().Trim());
                            System.Web.HttpContext.Current.Session.Add("USERPASSPORTNO", dsLogin.Tables[0].Rows[0]["PASSPORT_NO"].ToString().Trim());
                            System.Web.HttpContext.Current.Session.Add("USEROB", dsLogin.Tables[0].Rows[0]["DOB"].ToString().Trim());
                            System.Web.HttpContext.Current.Session.Add("USERPASSPORT_EXP", dsLogin.Tables[0].Rows[0]["LGN_PASSPORT_EXPIRY_DATE"].ToString().Trim());
                            System.Web.HttpContext.Current.Session.Add("USERPASS_ISSU_COUNTRY", dsLogin.Tables[0].Rows[0]["LGN_ISSUED_COUNTRY"].ToString().Trim());
                            System.Web.HttpContext.Current.Session.Add("USERPINCODE", dsLogin.Tables[0].Rows[0]["LGN_PINCODE"].ToString().Trim());
                        }
                    }
                    else
                    {
                        Session.Add("TERMINALTYPE", "M");
                        return RedirectToAction("SessionExp", "Redirect");
                    }
                }
                string strAgentId = Session["agentid"] != null && Session["agentid"].ToString() != "" ? Session["agentid"].ToString() : "";
                string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
                string strIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
                string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "";
                string strPOS_ID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
                string strPOS_TID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
                string strTerminalID = Session["terminalid"] != null && Session["terminalid"].ToString() != "" ? Session["terminalid"].ToString() : "";
                string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : "";
                if (strAgentId == "" || strUserName == "" || strIPAddress == "" || strTerminalID == "" || strTerminalType == "")
                {
                    strTerminalID = ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString();
                    strUserName = ConfigurationManager.AppSettings["BSA_USERNAME"].ToString();
                    string strPassword = ConfigurationManager.AppSettings["BSA_PASSWORD"].ToString();
                    strTerminalType = ConfigurationManager.AppSettings["TerminalType"].ToString();
                    string path = Server.MapPath("~/XML/LOGIN.xml");
                    string LoginDetXml = System.IO.File.ReadAllText(path);
                    DataSet dsBCI_LOGIN = new DataSet();
                    dsBCI_LOGIN.ReadXml(new XmlTextReader(new StringReader(LoginDetXml)));
                    bool msg = BSA_AssignSession(strTerminalID, strUserName, strPassword, dsBCI_LOGIN, strTerminalType);
                }
            }
            else if (Request.QueryString["SECKEY"] != null && Request.QueryString["SECKEY"] != "")
            {
                string sesskey = "";
                string Encquery = Request.QueryString["SECKEY"];
                string today = DateTime.Now.ToString("dd/MM/yyyy");
                string Querystring = Base.DecryptKEY(Encquery, "RIYA" + today);

                string[] keyval = new string[20];
                string[] Query = Querystring.Split('&');
                sesskey = "";

                if (Querystring.ToString() == "0")
                {
                    if (ConfigurationManager.AppSettings["APP_HOSTING"].ToString().ToUpper() == "BSA")
                        return RedirectToAction("Flights", "Flights");
                    else
                        return RedirectToAction("SessionExp", "Redirect");
                }

                Session.Add("JODIRLINEURL", Request.Url.OriginalString);
                if (Session["EnvironmentType"] == null || Session["EnvironmentType"] == "" || Session["EnvironmentType"] != "U")
                {
                    Session.Clear();
                    for (int i = 0; i < Query.Length; i++)
                    {
                        Session.Add(Query[i].Split('=')[0].Trim().ToUpper() + sesskey, Query[i].Split('=')[1].Trim());
                    }
                    if (Session["SECKEY"] != null && Session["SECKEY"].ToString() != "")
                    {
                        Session["SECKEY"] = Request.QueryString["SECKEY"].ToString();
                    }
                    else
                    {
                        Session.Add("SECKEY", Request.QueryString["SECKEY"]);
                    }
                    object ss = Loginsubmit(Session["TERMINALID" + sesskey].ToString(), Session["USERNAME" + sesskey].ToString(), Session["PASSWORD" + sesskey].ToString(), Session["TERMINALTYPE" + sesskey].ToString());
                }
            }
            if (Request.QueryString["CRMFLAG"] != null && Request.QueryString["CRMFLAG"] != "")
            {
                string enqid = Request.QueryString["Enquiry"] != null ? Request.QueryString["Enquiry"].ToString() : "";
                string SegmentCRM = Request.QueryString["Segment"] != null ? Request.QueryString["Segment"].ToString() : "";
                string callid = Request.QueryString["Callid"] != null ? Request.QueryString["Callid"].ToString() : "";
                string CRMFLag = Request.QueryString["CRMFLAG"] != null ? Request.QueryString["CRMFLAG"].ToString() : "";
                string chkselectcallid = Request.QueryString["CHk_selectCallid"] != null ? Request.QueryString["CHk_selectCallid"].ToString() : "";
                Session.Add("Enqidsameera", enqid);
                Session.Add("callidsameera", callid);
                Session.Add("ENQCALLID", enqid + "-" + callid);
                Session.Add("CRMFLag", CRMFLag);
                Session.Add("chkselectcallid", chkselectcallid);
                Session.Add("SegmentCRM", SegmentCRM);
                string qrystring = Request.QueryString["SelectClient"] != null ? Request.QueryString["SelectClient"].ToString() : "";
                Session.Add("SelectClient", qrystring);
                ViewBag.EnqueryClientid = qrystring;
            }
            if (Session["agentid"] == null || Session["agentid"].ToString() == "")
            {
                string strPGN = Session["PGN"] != null ? Session["PGN"].ToString() : "";
                string strTERMINALTYPE = Session["TERMINALTYPE"] != null ? Session["TERMINALTYPE"].ToString() : "";
                if ((strPGN == "M" || strPGN == "G") && ConfigurationManager.AppSettings["apptheme"].ToString().ToUpper() == "THEME4")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                }
                if (ConfigurationManager.AppSettings["APP_HOSTING"].ToString().ToUpper() == "BSA")
                    return RedirectToAction("Flights", "Flights");
                else
                    return RedirectToAction("SessionExp", "Redirect");
            }

            STSTRAVRAYS.Rays_service.RaysService _rays = new STSTRAVRAYS.Rays_service.RaysService();
            DataSet dst = new DataSet();

            ViewBag.maxdate = DateTime.Today.AddMonths(13).ToString("dd/MM/yyyy");

            string Air_Thread = ConfigurationManager.AppSettings["Air_Thread"].ToString();

            ViewBag.threadkey = Session["threadkey"] != null ? Session["threadkey"].ToString() + ((Air_Thread != "") ? "|" + Air_Thread + "" : "") : "";
            ViewBag.newthreadkey = Session["multithread"] != null ? Session["multithread"].ToString() : "";
            ViewBag.lccthreadkey = Session["lccthreadkey"] != null ? Session["lccthreadkey"].ToString() : "";
            ViewBag.fscthreadkey = Session["fscthreadkey"] != null ? Session["fscthreadkey"].ToString() : "";
            ViewBag.doublethread = Session["doublethread"] != null ? Session["doublethread"].ToString() : "";
            ViewBag.hdn_allowtrip = Session["tripConfig"] != null ? Session["tripConfig"].ToString() : "";
            ViewBag.hdnMultiThread = Session["multithreadkey"] != null ? Session["multithreadkey"].ToString() : "";

            ViewBag.threadkey_dom = Session["threadkey_dom"] != null ? Session["threadkey_dom"].ToString() : "";
            ViewBag.newthreadkey_dom = Session["newthread_dom"] != null ? Session["newthread_dom"].ToString() : "";
            ViewBag.lccthreadkey_dom = Session["lccthreadkey_dom"] != null ? Session["lccthreadkey_dom"].ToString() : "";
            ViewBag.fscthreadkey_dom = Session["fscthreadkey_dom"] != null ? Session["fscthreadkey_dom"].ToString() : "";
            ViewBag.hdnMultiThread_dom = Session["multithreadkey_dom"] != null ? Session["multithreadkey_dom"].ToString() : "";
            ViewBag.viathread = Session["VIATHREAD"] != null ? Session["VIATHREAD"].ToString() : "";

            ViewBag.hdn_checkflag = ConfigurationManager.AppSettings["Checktripflag"].ToString();
            ViewBag.hdn_country = ConfigurationManager.AppSettings["COUNTRY"].ToString();
            ViewBag.hdntrip_country = ConfigurationManager.AppSettings["trip_country"].ToString();
            ViewBag.hdnproductname = ConfigurationManager.AppSettings["Appname"].ToString();
            //ViewBag.hdnproductname = ConfigurationManager.AppSettings["Appname"].ToString();
            ViewBag.hdnsAllowmulticlass = Session["multiclass"] != null && Session["multiclass"].ToString() != "" ? Session["multiclass"].ToString() : "";

            /*New for student,army,sr.citizen fare*/
            ViewBag.studentthreadkey = Session["StudentThreadkey"] != null ? Session["StudentThreadkey"].ToString() : "";
            ViewBag.armythreadkey = Session["ArmyThreadkey"] != null ? Session["ArmyThreadkey"].ToString() : "";
            ViewBag.srcitizenthreadkey = Session["SRCitizenThreadkey"] != null ? Session["SRCitizenThreadkey"].ToString() : "";

            ViewBag.studentfareMessage = Session["StudentfareMsg"] != null ? Session["StudentfareMsg"].ToString() : "";
            ViewBag.armyfareMessage = Session["ArmyfareMsg"] != null ? Session["ArmyfareMsg"].ToString() : "";
            ViewBag.srcitizenfareMessage = Session["SRCitizenfareMsg"] != null ? Session["SRCitizenfareMsg"].ToString() : "";

            string sub = Session["POS_ID"].ToString().Substring(0, 5);
            ViewBag.hdnBaseorgin = sub.ToString().Substring(2, 3);
            Session.Add("Serverdatetime", LoadServerdatetime());

            ViewBag.currentdate = LoadServerdatetime();
            string Errormsg = string.Empty;

            try
            {
                string _Version = System.IO.File.ReadAllText(Server.MapPath("~/XML/VERSION/Scriptversion.txt"));
                ViewBag.fileversion = string.IsNullOrEmpty(_Version) ? DateTime.Now.ToString("yyMMdd") : _Version;
            }
            catch (Exception ex)
            {
                ViewBag.fileversion = DateTime.Now.ToString("yyMMdd");
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Fetchscriptversion", "Fetchscriptversion-Request filter", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }

            ViewBag.maxdate = DateTime.Today.AddMonths(13).ToString("dd/MM/yyyy");
            if (Session["SECKEY"] != null && Session["SECKEY"] != "")
            {

                if (Request.QueryString["MOBJOD_FLG"] != null && Request.QueryString["MOBJOD_FLG"] != "")
                {
                    Session["FLAG"] = "M";
                    return View("~/Views/Flights/MobileFlights.cshtml");
                }
                else
                {
                    string strPGN = Session["PGN"] != null ? Session["PGN"].ToString() : "";
                    string strTERMINALTYPE = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                    if (strPGN == "M" && strTERMINALTYPE == "L" && ConfigurationManager.AppSettings["apptheme"].ToString().ToUpper() == "THEME4")
                    {
                        return View("~/Views/Flights/Flights-Theme4.cshtml");
                    }
                    else if (strPGN == "G" && strTERMINALTYPE == "L" && ConfigurationManager.AppSettings["apptheme"].ToString().ToUpper() == "THEME4")
                    {
                        return View("~/Views/Flights/GDSFlights.cshtml");
                    }
                    else if (ConfigurationManager.AppSettings["apptheme"].ToString().ToUpper() == "THEME3")
                    {
                        return View("~/Views/Flights/Flights.cshtml");
                    }
                    else if (ConfigurationManager.AppSettings["APP_HOSTING"].ToString().ToUpper() == "BSA")
                    {
                        return View("~/Views/Search/Availability.cshtml");
                    }
                    else
                    {
                        return View("~/Views/Flights/Flights_Jod.cshtml");
                    }
                }
            }

            else if ((ConfigurationManager.AppSettings["apptheme"].ToString().ToUpper() == "THEME1")
                 || (ConfigurationManager.AppSettings["apptheme"].ToString().ToUpper() == "THEME3"))
            {
                return View("~/Views/Flights/Flights.cshtml");
            }
            else if (ConfigurationManager.AppSettings["apptheme"].ToString().ToUpper() == "THEME4")
            {
                return View("~/Views/Flights/Flights-Theme4.cshtml");
            }
            else if (ConfigurationManager.AppSettings["APP_HOSTING"].ToString().ToUpper() == "BSA")
            {
                return View("~/Views/Search/Availability.cshtml");
            }
            else
            {
                return View("~/Views/Flights/Flights-Theme2.cshtml");
            }

        }

        public ActionResult GDSFlights()
        {
            #region UsageLog
            string PageName = "GDS Search Page";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e) { }
            #endregion

            if (Session["agentid"] == null || Session["agentid"].ToString() == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }

            ViewBag.currentdate = LoadServerdatetime();
            ViewBag.maxdate = DateTime.Today.AddMonths(13).ToString("dd/MM/yyyy");
            return View("~/Views/Flights/GDSFlights.cshtml");
        }

        string strAgentID = string.Empty;
        string strTerminalId = string.Empty;
        string strUserName = string.Empty;
        string strBranchId = string.Empty;
        string sequnceID = string.Empty;
        string Ipaddress = string.Empty;
        string TerminalType = string.Empty;
        string strURLpath = ConfigurationManager.AppSettings["TRAVRAYSTWSA_HOST"];
        string strURLpathA = ConfigurationManager.AppSettings["TRAVRAYSTWSA_HOST"];
        string imgurl = ConfigurationManager.AppSettings["FlightUrl"];

        string strAppType = "B2B";

        Base.ServiceUtility Serv = new Base.ServiceUtility();
        BLL BAL = new BLL();

        #region Select Flight's

        public string LoadServerdatetime()
        {
            //double GMT = ConfigurationManager.AppSettings["GMTTIME"].ToString() == "" ? 0 : Convert.ToDouble(ConfigurationManager.AppSettings["GMTTIME"].ToString());
            double GMT = Convert.ToDouble(Session["GMTTIME"].ToString());
            DateTime dateTime = DateTime.Now.AddMinutes(GMT);//.Now.Date;
            return dateTime.ToString("dd/MM/yyyy");
        }

        public ActionResult SplFlt_fare(string FliNum, string Deaprt, string Arrive, string FullFlag, string TokenKey, string Trip, string BaseOrgin,
            string BaseDestination, string offflg, string Class, string TKey, string DEPTDATE, string ARRDATE, string Nfarecheck, string BestBuy, string oldfaremarkup,
            string AlterQueue)
        {
            string stu = string.Empty;
            string msg = string.Empty;

            DataTable dtSelectResponse = new DataTable();
            DataTable dtBookreq = new DataTable();
            DataTable dtBaggageSelect = new DataTable();
            DataTable dtmealseSelect = new DataTable();
            DataTable dtOtherSsrsel = new DataTable();
            DataTable dtBagout = new DataTable();
            ArrayList array_Response = new ArrayList();

            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");

            Base.ServiceUtility Serv = new Base.ServiceUtility();
            int error = 0;
            int grossfarewithoutmarkup = 1;
            int grossfarewithmarkup = 2;
            int farebreakup = 3;
            int oldgrosswithmarkup = 4;
            int markup = 5;
            int oldgrosswithoutmarkup = 6;
            int basicfare = 7;
            int oldandnewfare = 8;

            int ResultCode = 9;
            int ErrorMsg = 10;


            string Returnval = "";
            try
            {
                if (Session["agentid"] == null)
                {
                    array_Response[ResultCode] = "-1";
                    return Json(new { Status = "-1", Message = "", Result = array_Response });
                }

                string ValKey = "";

                if (TKey != null && TKey != "")
                {
                    if (TKey.Contains('~'))
                    {
                        string[] Availed = TKey.Split('~');
                        ValKey = Availed[Availed.Count() - 2].ToString().Split('_')[1].ToString().Trim();
                    }
                    else
                    {
                        string[] Availed = TKey.Split('_');
                        ValKey = Availed[1].ToString().Trim();
                    }
                }


                #region Form Booking Request .......
                string Origin = "";
                string Destination = "";
                string depttime = string.Empty;
                string ArrTime = string.Empty;
                string CNX = string.Empty;

                string FareCode = string.Empty;
                string SClass = string.Empty;
                string AirlineCategory = string.Empty;
                string OFFLINEFLAG = offflg;
                string strAdults = "";
                string strChildrens = "";
                string strInfants = "";
                string PlatingCarrier = "";
                string RBDCode = "";
                string ConnectionFlag = "";
                string BaseAmount = "";
                string GrossAmount = "";
                string StkType = "";

                RQRS.PriceItineary PriceIti = new RQRS.PriceItineary();
                List<RQRS.Itineraries> ListItenar = new List<RQRS.Itineraries>();
                List<RQRS.Flights> FlightsDet = new List<RQRS.Flights>();
                RQRS.Itineraries itinflights = new RQRS.Itineraries();
                RQRS.SegmentDetails SegDet = new RQRS.SegmentDetails();

                string[] ArrFliDet, FlightCount;
                if (FullFlag.Contains("SpLITSaTIS"))
                {
                    FlightCount = Regex.Split(FullFlag, "SpLITSaTIS");

                    for (int iF = 0; iF < FlightCount.Length; iF++)
                    {

                        if (FlightCount[iF].Contains("SpLitPResna"))
                        {
                            ArrFliDet = Regex.Split(FlightCount[iF], "SpLitPResna");

                            Origin = ArrFliDet[0];
                            Destination = ArrFliDet[1];
                            depttime = ArrFliDet[2];
                            ArrTime = ArrFliDet[3];
                            SClass = Class;
                            strAdults = ArrFliDet[6];
                            strChildrens = ArrFliDet[7];
                            strInfants = ArrFliDet[8];
                            PlatingCarrier = ArrFliDet[9];
                            RBDCode = ArrFliDet[5];
                            BaseAmount = ArrFliDet[13];
                            GrossAmount = ArrFliDet[14];
                            ConnectionFlag = ArrFliDet[15];
                            AirlineCategory = ArrFliDet[16];
                            FareCode = ArrFliDet[17];

                            RQRS.Flights Itindet = new RQRS.Flights();
                            Itindet.AirlineCategory = AirlineCategory;
                            Itindet.ArrivalDateTime = ArrTime;
                            Itindet.CNX = CNX;
                            Itindet.DepartureDateTime = depttime.Trim();
                            Itindet.Destination = Destination;
                            Itindet.FareBasisCode = FareCode.Trim();
                            Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                            Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                            Itindet.PlatingCarrier = PlatingCarrier;
                            Itindet.Class = SClass;//strFareclassOption.ToString().Trim();
                            Itindet.RBDCode = RBDCode;
                            Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                            Itindet.OfflineFlag = Convert.ToInt16(OFFLINEFLAG.Trim());
                            Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                            Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                            Itindet.ItinRef = ArrFliDet[19];
                            Itindet.CNX = ArrFliDet[20];
                            StkType = ArrFliDet[23];
                            FlightsDet.Add(Itindet);

                        }
                    }
                }
                else
                {
                    if (FullFlag.Contains("SpLitPResna"))
                    {
                        // ArrFliDet = FullFlag.Split('~');
                        ArrFliDet = Regex.Split(FullFlag, "SpLitPResna");

                        Origin = ArrFliDet[0];
                        Destination = ArrFliDet[1];
                        depttime = ArrFliDet[2];
                        ArrTime = ArrFliDet[3];
                        SClass = Class;
                        strAdults = ArrFliDet[6];
                        strChildrens = ArrFliDet[7];
                        strInfants = ArrFliDet[8];
                        PlatingCarrier = ArrFliDet[9];
                        RBDCode = ArrFliDet[5];
                        BaseAmount = ArrFliDet[13];
                        GrossAmount = ArrFliDet[14];
                        ConnectionFlag = ArrFliDet[15];
                        AirlineCategory = ArrFliDet[16];
                        FareCode = ArrFliDet[17];



                        RQRS.Flights Itindet = new RQRS.Flights();
                        Itindet.AirlineCategory = AirlineCategory;
                        Itindet.ArrivalDateTime = ArrTime;
                        Itindet.DepartureDateTime = depttime.Trim();
                        Itindet.Destination = Destination;
                        Itindet.FareBasisCode = FareCode.Trim();
                        Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                        Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                        Itindet.PlatingCarrier = PlatingCarrier;
                        Itindet.Class = SClass;//strFareclassOption.ToString().Trim();
                        Itindet.RBDCode = RBDCode;
                        Itindet.ReferenceToken = TokenKey; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                        Itindet.OfflineFlag = Convert.ToInt16(OFFLINEFLAG.Trim());
                        Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                        Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                        Itindet.ItinRef = ArrFliDet[19];
                        Itindet.CNX = ArrFliDet[20];
                        StkType = ArrFliDet[23];
                        FlightsDet.Add(Itindet);
                    }
                }



                SegDet.BaseOrigin = BaseOrgin;
                SegDet.BaseDestination = BaseDestination;



                SegDet.Adult = strAdults;
                SegDet.Child = strChildrens;
                SegDet.Infant = strInfants;

                SegDet.SegmentType = "I";
                SegDet.ClassType = SClass;
                SegDet.BookingType = "B2B";
                SegDet.TripType = Trip;
                SegDet.AppType = "W";



                RQRS.AgentDetails agent = new RQRS.AgentDetails();
                //agent.AgentId = Session["agentid"].ToString();
                //agent.TerminalId = Session["terminalid"].ToString();
                //agent.UserName = Session["username"].ToString();
                //agent.AppType = "B2B";
                //agent.Environment = "W";


                agent.AgentId = Session["agentid"].ToString();
                agent.Agenttype = "";
                agent.AirportID = "I";
                agent.AppType = "B2B";
                agent.BOAID = Session["POS_ID"].ToString();
                agent.BOAterminalID = Session["POS_TID"].ToString();
                agent.BranchID = "";
                agent.ClientID = "";
                agent.CoOrdinatorID = "";
                agent.Environment = "W";
                agent.TerminalId = Session["terminal"].ToString();
                agent.UserName = Session["username"].ToString();
                agent.Version = "";

                itinflights.Stock = StkType;
                itinflights.BaseAmount = BaseAmount;// dtBookFlight.Rows[0]["BasicFare"].ToString();
                itinflights.GrossAmount = GrossAmount;// dtBookFlight.Rows[0]["GrossFare"].ToString();
                itinflights.FlightDetails = FlightsDet;



                ListItenar.Add(itinflights);


                PriceIti.ItinearyDetails = ListItenar;
                PriceIti.BestBuyOption = BestBuy == "TRUE" ? true : false;
                PriceIti.SegmnetDetails = SegDet;
                PriceIti.AgentDetails = agent;
                PriceIti.Stock = StkType;
                PriceIti.AlterQueue = AlterQueue;

                #endregion

                #region Send Request to RemoteServer......

                try
                {
                    string request = JsonConvert.SerializeObject(PriceIti).ToString();
                    string Query = "InvokeHostCheck";
                    int hostchecktimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);

                    MyWebClient client = new MyWebClient();
                    client.LintTimeout = hostchecktimeout;
                    client.Headers["Content-type"] = "application/json";

                    #region Log


                    StringWriter strWriter = new StringWriter();
                    DataSet dsrequest = new DataSet();
                    dsrequest = Serv.convertJsonStringToDataSet(request, "");
                    dsrequest.WriteXml(strWriter);

                    string LstrDetails = "<BESTBUY_REQUEST><URL>[<![CDATA[" + Session["travraysws_url"] + "/" + Session["travraysws_vdir"] + "/Rays.svc/"
                        + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (hostchecktimeout).ToString() + "</TIMEOUT><TIME>"
                        + "</TIME>" + strWriter.ToString() + "</BESTBUY_REQUEST>";


                    DatabaseLog.LogData(Session["username"].ToString(), "S", "Best Buy Request", "Best Buy Request", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                    #endregion

                    if (ConfigurationManager.AppSettings["Splavailagent"].ToString() != "" && ConfigurationManager.AppSettings["Splavailagent"].ToString().Contains(Session["POS_TID"].ToString()))
                    {
                        strURLpath = ConfigurationManager.AppSettings["Spl_APPS_SELECT_URL"].ToString();
                    }
                    else
                    {
                        strURLpath = ConfigurationManager.AppSettings["APPS_SELECT_URL"].ToString();
                    }
                    byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));

                    string strResponse = System.Text.Encoding.ASCII.GetString(data);
                    #region Log


                    DataSet dsresponse = new DataSet();
                    dsresponse = Serv.convertJsonStringToDataSet(strResponse, "");
                    dsresponse.WriteXml(strWriter);

                    LstrDetails = "<BESTBUY_RESPONSE><URL>[<![CDATA[" + Session["travraysws_url"] + "/" + Session["travraysws_vdir"] + "/Rays.svc/"
                       + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (hostchecktimeout).ToString() + "</TIMEOUT><TIME>"
                       + "</TIME>" + strWriter.ToString() + "</BESTBUY_RESPONSE>";


                    DatabaseLog.LogData(Session["username"].ToString(), "S", "Best buy fare", "Get Best Buy Fare", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                    #endregion
                    if (string.IsNullOrEmpty(strResponse))
                    {
                        array_Response[ResultCode] = 0;
                        array_Response[error] = "Currently Special fare is not Available for this flight.";
                        DatabaseLog.LogData(Session["username"].ToString(), "S", "Best buy fare", "Get Best Buy Fare", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                    }
                    else
                    {
                        string GrossAmnt = string.Empty;
                        string servicecharge = string.Empty;
                        string Markup = string.Empty;
                        string Farebreakup = string.Empty;
                        string Basicfare = string.Empty;
                        string newclass = string.Empty;
                        string refundable = string.Empty;
                        string commission = string.Empty;
                        string FAREBASISCODE = string.Empty;

                        DataSet dsselect = new DataSet();
                        dsselect = Serv.convertJsonStringToDataSet(strResponse, "");
                        DataTable dsselect1 = new DataTable();
                        dsselect1 = dsselect.Tables[0];
                        Session.Add("Dobmand" + ValKey, dsselect1);
                        RQRS.PriceItenaryRS _availRes = JsonConvert.DeserializeObject<RQRS.PriceItenaryRS>(strResponse);
                        string strErrtemp = "";


                        if (_availRes.ResultCode == "1")
                        {

                            List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;



                            List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;
                            RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                            bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                            string TokenBooking = lstrPriceItenary[0].Token;
                            _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                            _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                            _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                            _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                            _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;

                            if (bestbuy == true)
                            {

                                DataTable dtmealseSele = new DataTable();
                                DataTable dtBaggageSele = new DataTable();
                                DataTable dtOtherSSR = new DataTable();
                                DataTable dtOtherbaggout = new DataTable();
                                Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse, ref strErrtemp,
                                    ref dtBaggageSele, ref dtmealseSele, ref dtOtherSSR, ref dtBookreq, ref dtOtherbaggout);
                                if (!string.IsNullOrEmpty(strErrtemp))
                                {

                                    DatabaseLog.LogData(Session["username"].ToString(), "S", "Best buy fare", "Best buy fare", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                    //Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                                    array_Response[error] = "Currently Best buy fare is not Available for this flight.";
                                }
                                if (dtSelectResponse != null
                                           && dtSelectResponse.Columns.Count > 1
                                           && dtSelectResponse.Rows.Count > 0)
                                {
                                    GrossAmnt = dtSelectResponse.Rows[0]["GrossAmount"].ToString();
                                    servicecharge = dtSelectResponse.Rows[0]["Servicecharge"].ToString();
                                    Markup = dtSelectResponse.Rows[0]["markup"].ToString();
                                    Farebreakup = dtSelectResponse.Rows[0]["TAXBREAKUP"].ToString();
                                    Basicfare = dtSelectResponse.Rows[0]["BaseAmount"].ToString();
                                    newclass = dtSelectResponse.Rows[0]["Class"].ToString();
                                    refundable = (string.IsNullOrEmpty(dtSelectResponse.Rows[0]["refund"].ToString()) ? "" : dtSelectResponse.Rows[0]["refund"].ToString().ToUpper());
                                    commission = dtSelectResponse.Rows[0]["Commission"].ToString().ToUpper();
                                    FAREBASISCODE = dtSelectResponse.Rows[0]["FAREBASISCODE"].ToString().ToUpper();

                                    if (dtSelectResponse.Rows[0]["GrossAmount"].ToString().Contains("|"))
                                    {
                                        string[] GAmnt = GrossAmnt.Split('|');
                                        GrossAmnt = GAmnt[0];
                                    }

                                    if (servicecharge.Contains("|"))
                                    {
                                        string[] serchrg = servicecharge.Split('|');
                                        servicecharge = serchrg[0];
                                    }
                                    if (Markup.Contains("|"))
                                    {
                                        string[] mrkup = Markup.Split('|');
                                        Markup = mrkup[0];
                                    }
                                    if (Farebreakup.Contains("|"))
                                    {
                                        string[] frebrkup = Farebreakup.Split('|');
                                        Farebreakup = frebrkup[0];
                                    }
                                    if (Basicfare.Contains("|"))
                                    {
                                        string[] basefare = Basicfare.Split('|');
                                        Basicfare = basefare[0];
                                    }

                                    if (newclass.Contains("|"))
                                    {
                                        string[] classss = newclass.Split('|');
                                        newclass = classss[0];
                                    }

                                    if (FAREBASISCODE.Contains("|"))
                                    {
                                        string[] farebasic = FAREBASISCODE.Split('|');
                                        FAREBASISCODE = farebasic[0];
                                    }

                                    if (refundable.Contains("|"))
                                    {
                                        string[] refund = refundable.Split('|');
                                        refundable = refund[0];
                                    }
                                    if (commission.Contains("|"))
                                    {
                                        string[] commiss = commission.Split('|');
                                        commission = commiss[0];
                                    }

                                    string oldgrossfeewithmarkup = (Base.ServiceUtility.ConvertToDecimal(PriceIti.ItinearyDetails[0].GrossAmount.ToString()) + Base.ServiceUtility.ConvertToDecimal(oldfaremarkup) + Base.ServiceUtility.ConvertToDecimal(servicecharge)).ToString();
                                    string oldgrossfeewithoutmarkup = (Base.ServiceUtility.ConvertToDecimal(PriceIti.ItinearyDetails[0].GrossAmount.ToString()) + Base.ServiceUtility.ConvertToDecimal(servicecharge)).ToString();
                                    string Newgrossfeewithmarkup = (Base.ServiceUtility.ConvertToDecimal(GrossAmnt) + Base.ServiceUtility.ConvertToDecimal(Markup) + Base.ServiceUtility.ConvertToDecimal(servicecharge)).ToString();
                                    string Newgrossfeewithoutmarkup = (Base.ServiceUtility.ConvertToDecimal(GrossAmnt) + Base.ServiceUtility.ConvertToDecimal(servicecharge)).ToString();
                                    string breakupfare = Farebreakup + (servicecharge != null && servicecharge != "0" && servicecharge != "" ? "/Serv.Chrg:" + servicecharge : "");

                                    array_Response[ResultCode] = 1;
                                    array_Response[grossfarewithoutmarkup] = Newgrossfeewithoutmarkup;
                                    array_Response[grossfarewithmarkup] = Newgrossfeewithmarkup;
                                    array_Response[markup] = Markup;
                                    array_Response[farebreakup] = breakupfare;

                                    array_Response[oldgrosswithmarkup] = oldgrossfeewithmarkup;
                                    array_Response[oldgrosswithoutmarkup] = oldgrossfeewithoutmarkup;
                                    array_Response[basicfare] = Basicfare;

                                    array_Response[oldandnewfare] = PriceIti.ItinearyDetails[0].FlightDetails[0].RBDCode + "-" + PriceIti.ItinearyDetails[0].FlightDetails[0].FareBasisCode + "Splitold" + PriceIti.ItinearyDetails[0].BaseAmount.ToString() + "Splitold" + oldgrossfeewithmarkup + "Splitold" + oldgrossfeewithoutmarkup + "Splitold" + oldfaremarkup + "Splitoldnew"
                                        + newclass + "-" + FAREBASISCODE + "Splitold" + Basicfare + "Splitold" + Newgrossfeewithmarkup + "Splitold" + Newgrossfeewithoutmarkup + "Splitold" + refundable + "Splitold" + breakupfare + "Splitold" + Markup + "Splitold" + commission + "Splitold" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();


                                    LstrDetails = "<BESTBUY><OLDFARE>" + PriceIti.ItinearyDetails[0].GrossAmount.ToString() + "</OLDFARE><NEWFARE>" + _availResponse.Fares[0].Faredescription[0].GrossAmount + "</NEWFARE></BESTBUY>";

                                    DatabaseLog.LogData(Session["username"].ToString(), "S", "Best buy fare", "Get Best Buy Fare", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                                }
                            }
                            else
                            {
                                array_Response[ResultCode] = 0;
                                array_Response[error] = "Currently Special fare is not Available for this flight.";
                            }
                        }
                        else if (_availRes.ResultCode == "2")
                        {
                            string[] GAmnt;

                            List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;



                            List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;
                            RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                            bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                            string TokenBooking = lstrPriceItenary[0].Token;
                            _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                            _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                            _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                            _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                            _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;




                            Session.Add("BestBuy" + ValKey, bestbuy.ToString());
                            Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                            Session.Add("Requestmarkup" + ValKey, (oldfaremarkup.ToString() != "" && oldfaremarkup.ToString() != null ? oldfaremarkup : "0"));

                            dtBaggageSelect = new DataTable();
                            dtmealseSelect = new DataTable();
                            dtOtherSsrsel = new DataTable();
                            dtBagout = new DataTable();
                            Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse,
                                ref strErrtemp, ref dtBaggageSelect, ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtBagout);
                            if (bestbuy == true)
                            {

                                GrossAmnt = dtSelectResponse.Rows[0]["GrossAmount"].ToString();
                                servicecharge = dtSelectResponse.Rows[0]["Servicecharge"].ToString();
                                Markup = dtSelectResponse.Rows[0]["markup"].ToString();
                                Farebreakup = dtSelectResponse.Rows[0]["TAXBREAKUP"].ToString();
                                Basicfare = dtSelectResponse.Rows[0]["BaseAmount"].ToString();
                                newclass = dtSelectResponse.Rows[0]["Class"].ToString();
                                refundable = (string.IsNullOrEmpty(dtSelectResponse.Rows[0]["refund"].ToString()) ? "" : dtSelectResponse.Rows[0]["refund"].ToString().ToUpper());
                                commission = dtSelectResponse.Rows[0]["Commission"].ToString().ToUpper();
                                FAREBASISCODE = dtSelectResponse.Rows[0]["FAREBASISCODE"].ToString().ToUpper();

                                if (dtSelectResponse.Rows[0]["GrossAmount"].ToString().Contains("|"))
                                {
                                    string[] GAmntt = GrossAmnt.Split('|');
                                    GrossAmnt = GAmntt[0];
                                }

                                if (servicecharge.Contains("|"))
                                {
                                    string[] serchrg = servicecharge.Split('|');
                                    servicecharge = serchrg[0];
                                }
                                if (Markup.Contains("|"))
                                {
                                    string[] mrkup = Markup.Split('|');
                                    Markup = mrkup[0];
                                }
                                if (Farebreakup.Contains("|"))
                                {
                                    string[] frebrkup = Farebreakup.Split('|');
                                    Farebreakup = frebrkup[0];
                                }
                                if (Basicfare.Contains("|"))
                                {
                                    string[] basefare = Basicfare.Split('|');
                                    Basicfare = basefare[0];
                                }

                                if (newclass.Contains("|"))
                                {
                                    string[] classss = newclass.Split('|');
                                    newclass = classss[0];
                                }

                                if (FAREBASISCODE.Contains("|"))
                                {
                                    string[] farebasic = FAREBASISCODE.Split('|');
                                    FAREBASISCODE = farebasic[0];
                                }

                                if (refundable.Contains("|"))
                                {
                                    string[] refund = refundable.Split('|');
                                    refundable = refund[0];
                                }
                                if (commission.Contains("|"))
                                {
                                    string[] commiss = commission.Split('|');
                                    commission = commiss[0];
                                }

                                string oldgrossfeewithmarkup = (Base.ServiceUtility.ConvertToDecimal(PriceIti.ItinearyDetails[0].GrossAmount.ToString()) + Base.ServiceUtility.ConvertToDecimal(oldfaremarkup) + Base.ServiceUtility.ConvertToDecimal(servicecharge)).ToString();
                                string oldgrossfeewithoutmarkup = (Base.ServiceUtility.ConvertToDecimal(PriceIti.ItinearyDetails[0].GrossAmount.ToString()) + Base.ServiceUtility.ConvertToDecimal(servicecharge)).ToString();
                                string Newgrossfeewithmarkup = (Base.ServiceUtility.ConvertToDecimal(GrossAmnt) + Base.ServiceUtility.ConvertToDecimal(Markup) + Base.ServiceUtility.ConvertToDecimal(servicecharge)).ToString();
                                string Newgrossfeewithoutmarkup = (Base.ServiceUtility.ConvertToDecimal(GrossAmnt) + Base.ServiceUtility.ConvertToDecimal(servicecharge)).ToString();
                                string breakupfare = Farebreakup + (servicecharge != null && servicecharge != "0" && servicecharge != "" ? "/Serv.Chrg:" + servicecharge : "");

                                array_Response[ResultCode] = 1;
                                array_Response[grossfarewithoutmarkup] = Newgrossfeewithoutmarkup;
                                array_Response[grossfarewithmarkup] = Newgrossfeewithmarkup;
                                array_Response[markup] = Markup;
                                array_Response[farebreakup] = breakupfare;

                                array_Response[oldgrosswithmarkup] = oldgrossfeewithmarkup;
                                array_Response[oldgrosswithoutmarkup] = oldgrossfeewithoutmarkup;
                                array_Response[basicfare] = Basicfare;

                                array_Response[oldandnewfare] = PriceIti.ItinearyDetails[0].FlightDetails[0].RBDCode + "-" + PriceIti.ItinearyDetails[0].FlightDetails[0].FareBasisCode + "Splitold" + PriceIti.ItinearyDetails[0].BaseAmount.ToString() + "Splitold" + oldgrossfeewithmarkup + "Splitold" + oldgrossfeewithoutmarkup + "Splitold" + oldfaremarkup + "Splitoldnew"
                                    + newclass + "-" + FAREBASISCODE + "Splitold" + Basicfare + "Splitold" + Newgrossfeewithmarkup + "Splitold" + Newgrossfeewithoutmarkup + "Splitold" + refundable + "Splitold" + breakupfare + "Splitold" + Markup + "Splitold" + commission + "Splitold" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();


                                LstrDetails = "<BESTBUY><OLDFARE>" + PriceIti.ItinearyDetails[0].GrossAmount.ToString() + "</OLDFARE><NEWFARE>" + _availResponse.Fares[0].Faredescription[0].GrossAmount + "</NEWFARE></BESTBUY>";

                                DatabaseLog.LogData(Session["username"].ToString(), "S", "Best buy fare", "Get Best Buy Fare", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                            }
                            else
                            {
                                GrossAmnt = dtSelectResponse.Rows[0]["GrossAmount"].ToString();
                                if (dtSelectResponse.Rows[0]["GrossAmount"].ToString().Contains("|"))
                                {
                                    GAmnt = GrossAmnt.Split('|');
                                    GrossAmnt = GAmnt[0];
                                }

                                if (GrossAmnt == PriceIti.ItinearyDetails[0].GrossAmount.ToString())// && _availRes.ResultCode == "1")
                                {

                                    dtSelectResponse.TableName = "TrackFareDetails";
                                    Session.Add("Response" + ValKey, dtSelectResponse);

                                    Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                    Session.Add("BaseDest" + ValKey, BaseDestination);
                                    Session.Add("TripType" + ValKey, Trip);
                                    Session.Add("Specialflagfare" + ValKey, "");
                                    Session.Add("TokenBooking" + ValKey, TokenBooking);
                                    Session.Add("Deaprt" + ValKey, Deaprt);
                                    Session.Add("Arrive" + ValKey, Arrive);
                                    Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                    Session.Add("RetDate" + ValKey, ARRDATE);
                                    Session.Add("Stock" + ValKey, StkType);
                                    Session.Add("segmclass" + ValKey, Class);
                                    Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                    Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                    if (Nfarecheck == "FALSE" && _availResponse.FareCheck.CheckFlag == "Y")
                                    {
                                        Returnval = "Fare has been revised from :" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " " + (Base.ServiceUtility.ConvertToDecimal(PriceIti.ItinearyDetails[0].GrossAmount.ToString())
                                       + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                        + " to :" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " "
                                       + (Base.ServiceUtility.ConvertToDecimal(SplitFareByAdultPax(dtSelectResponse.Rows[0]["GROSSAMOUNT"].ToString()).ToString())
                                       + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                       + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString()
                                       + Environment.NewLine + " Do you want to continue the flight selection");

                                        array_Response[ResultCode] = 2;
                                        array_Response[ErrorMsg] = Returnval;
                                    }
                                    else
                                    {
                                        array_Response[ResultCode] = 2;
                                        array_Response[ErrorMsg] = _availRes.Error;
                                    }

                                }

                                else
                                {

                                    dtSelectResponse.TableName = "TrackFareDetails";
                                    Session.Add("Response" + ValKey, dtSelectResponse);
                                    Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                    Session.Add("BaseDest" + ValKey, BaseDestination);
                                    Session.Add("TripType" + ValKey, Trip);
                                    Session.Add("Specialflagfare" + ValKey, "");
                                    Session.Add("TokenBooking" + ValKey, TokenBooking);
                                    Session.Add("Deaprt" + ValKey, Deaprt);
                                    Session.Add("Arrive" + ValKey, Arrive);
                                    Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                    Session.Add("RetDate" + ValKey, ARRDATE);
                                    Session.Add("segmclass" + ValKey, Class);
                                    Session.Add("Stock" + ValKey, StkType);
                                    Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                    Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                    Returnval = "Fare has been revised from :" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " "
                                        + (Base.ServiceUtility.ConvertToDecimal(PriceIti.ItinearyDetails[0].GrossAmount.ToString())
                                        + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                        + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString() + " to :"
                                        + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " "
                                        + (Base.ServiceUtility.ConvertToDecimal(SplitFareByAdultPax(dtSelectResponse.Rows[0]["GROSSAMOUNT"].ToString()).ToString())
                                        + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                        + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString()
                                        + Environment.NewLine + " Do you want to continue the flight selection";

                                    array_Response[ResultCode] = 2;
                                    array_Response[ErrorMsg] = Returnval;

                                }
                            }
                        }
                        else if (_availRes.ResultCode == "3")
                        {

                            strErrtemp = "";
                            GrossAmnt = "";

                            #region log


                            List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;



                            List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;
                            RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                            bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                            string TokenBooking = lstrPriceItenary[0].Token;
                            _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                            _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                            _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                            _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                            _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;


                            StringWriter strresWriter = new StringWriter();
                            DataSet dsresponse2 = new DataSet();
                            dsresponse2 = Serv.convertJsonStringToDataSet(strResponse, "");
                            dsresponse2.WriteXml(strresWriter);

                            string lstrtime = "SelectResponse" + DateTime.Now;

                            string lstrCata = Trip + " ~SRS~" +
                           PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;

                            string LstrRespDetails = "<SELECT_RESPONSE><RESPONSETIME>" + lstrtime + "</RESPONSETIME>" +
                           //+ "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                           "<Flights>" + (_availResponse == null || _availResponse.Flights == null ? "NULL" : _availResponse.Flights.Count.ToString()) + "</Flights>" +
                           "<Fares>" + (_availResponse == null || _availResponse.Fares == null ? "NULL" : _availResponse.Fares.Count.ToString()) + "</Fares>" +
                           "<Bagg>" + (_availResponse == null || _availResponse.Bagg == null ? "NULL" : _availResponse.Bagg.Count.ToString()) + "</Bagg>" +
                           "<Meal>" + (_availResponse == null || _availResponse.Meal == null ? "NULL" : _availResponse.Meal.Count.ToString()) + "</Meal>" +
                           "<DOBMandatory>" + (_availRes == null || _availRes.DOBMandatory == null ? "NULL" : _availRes.DOBMandatory.ToString()) + "</DOBMandatory>" +
                           "<PassportMandatory>" + (_availRes == null || _availRes.PassportMandatory == null ? "NULL" : _availRes.PassportMandatory.ToString()) + "</PassportMandatory>"
                                + strresWriter.ToString() + "</SELECT_RESPONSE>";

                            DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE SUCCESS " + TokenBooking, LstrRespDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                            # endregion

                            Session.Add("BestBuy" + ValKey, false);
                            Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                            Session.Add("Requestmarkup" + ValKey, (oldfaremarkup.ToString() != "" && oldfaremarkup.ToString() != null ? oldfaremarkup : "0"));
                            dtBaggageSelect = new DataTable();
                            dtmealseSelect = new DataTable();
                            dtOtherSsrsel = new DataTable();
                            dtBagout = new DataTable();
                            Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse, ref strErrtemp,
                                ref dtBaggageSelect, ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtBagout);

                            if (!string.IsNullOrEmpty(strErrtemp))
                            {

                                DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", "GRID SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                                array_Response[ResultCode] = 0;
                                array_Response[error] = strErrtemp;

                            }
                            if (dtSelectResponse != null
                                       && dtSelectResponse.Columns.Count > 1
                                       && dtSelectResponse.Rows.Count > 0)
                            {

                                dtSelectResponse.TableName = "TrackFareDetails";
                                Session.Add("Response" + ValKey, dtSelectResponse);

                                Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                Session.Add("BaseDest" + ValKey, BaseDestination);
                                Session.Add("TripType" + ValKey, Trip);
                                Session.Add("Specialflagfare" + ValKey, "");
                                Session.Add("TokenBooking" + ValKey, TokenBooking);
                                Session.Add("Deaprt" + ValKey, Deaprt);
                                Session.Add("Arrive" + ValKey, Arrive);
                                Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                Session.Add("RetDate" + ValKey, ARRDATE);
                                Session.Add("Stock" + ValKey, StkType);
                                Session.Add("segmclass" + ValKey, Class);
                                Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);



                                array_Response[ResultCode] = _availRes.ResultCode;
                                array_Response[error] = _availRes.Error;
                            }
                            else
                            {
                                DatabaseLog.LogData(Session["username"].ToString(), "ER", "InternationalFlights", "SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                Returnval = "Problem occured while select a flight";
                                array_Response[ResultCode] = 0;
                                array_Response[error] = Returnval;
                            }
                        }

                        else if (_availRes.ResultCode == "4")
                        {
                            array_Response[ResultCode] = _availRes.ResultCode;
                            array_Response[error] = _availRes.Error;
                        }
                        else
                        {
                            array_Response[ResultCode] = 0;
                            array_Response[error] = "Currently Special fare is not Available for this flight.";

                        }
                    }
                }
                catch (Exception ex)
                {
                    array_Response[ResultCode] = 0;
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "InternationalFlights", "SELECT REQUEST", ex.StackTrace.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                    Returnval = "Problem Occured. Please contact customercare";
                    array_Response[error] = Returnval;
                }
                #endregion
            }
            catch (Exception ex)
            {
                array_Response[ResultCode] = 0;
                Returnval = ex.Message;
                DatabaseLog.LogData(Session["username"].ToString(), "X", "InternationalFlights", "SELECT REQUEST", ex.ToString() + ex.StackTrace.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                array_Response[error] = Returnval;
            }
            //return array_Response;
            return Json(new { Status = "", Message = "", Result = array_Response });
        }

        public ActionResult Flights_DoWork(string FliNum, string Deaprt, string Arrive, string FullFlag, string TokenKey, string Trip, string BaseOrgin,
            string BaseDestination, string offflg, string Class, string TKey, string DEPTDATE, string ARRDATE, string Nfarecheck, string oldfaremarkup, string mobile,
            string bestbuyrequired, string AlterQueue, string SegmentType, string Multireqst,
            string reqcnt, string RtripComparFlg, string Specialflagfare, string ClientID, string BranchID, string GroupID, bool StudentFare, bool ArmyFare, bool SnrcitizenFare)

        {
            #region UsageLog
            string PageName = "SelectFlight";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, Session["UniqueIduser"] == null ? "" : Session["UniqueIduser"].ToString(), "SELECT");
            }
            catch (Exception e) { }
            #endregion
            DataTable dtSelectResponse = new DataTable();
            DataTable dtBookreq = new DataTable();
            DataTable dtSelectResponsetmp = new DataTable();


            DataTable dtBaggageSelect = new DataTable();
            DataTable dtmealseSelect = new DataTable();
            DataTable dtOtherSsrsel = new DataTable();
            DataTable dtBagout = new DataTable();
            ArrayList array_Response = new ArrayList();
            DataSet dataSet = new DataSet();
            ViewBag.mulcity_block = Trip.Trim();
            string basstrp = string.Empty;
            string TokenBooking = "";
            string strAirlinsecatagory = string.Empty;
            string refuncs = string.Empty;
            string chkMsg = string.Empty;

            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            //array_Response.Add("");
            //array_Response.Add("");
            //array_Response.Add("");
            //array_Response.Add("");
            Base.ServiceUtility Serv = new Base.ServiceUtility();
            RQRS.PriceItenaryRS _availRes = new RQRS.PriceItenaryRS();

            RaysService _rays_servers = new RaysService();
            _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
            InplantService.Inplantservice _inplantservice = new InplantService.Inplantservice();
            _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();

            int ResultCode = 0;
            int ErrorMsg = 1;
            //int JsonResponse = 2;
            //int Mob_Travelbuild_Response = 3;
            //int Mob_Amount_Response = 4;
            //int Mob_hidden_values = 5;

            Session["domseg"] = null;
            Session.Add("domseg", SegmentType);
            string Returnval = "";
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strClientType = Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : "";
            decimal Checkgrossamt = 0;
            ViewBag.ChkMsg = string.Empty;
            string Allowbokingtype = string.Empty;
            try
            {
                if (Session["agentid"] == null)
                {
                    ViewBag.status = "-1";
                    if (Multireqst == "Y")
                    {
                        return Json(new { Status = ViewBag.status, Message = "", Result = "" });
                    }
                    else
                    {
                        if (ConfigurationManager.AppSettings["APP_HOSTING"] == "BSA" || ConfigurationManager.AppSettings["APP_HOSTING"] == "B2B"
                            || ConfigurationManager.AppSettings["APP_HOSTING"] == "BOA")
                        {
                            return PartialView("_AvailSelect_BSA", "");
                        }
                        else
                        {
                            return PartialView("_AvailSelect", "");
                        }
                    }
                }

                strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
                strUserName = Session["username"] != null ? Session["username"].ToString() : "";
                Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
                strBranchId = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                BranchID = (string.IsNullOrEmpty(BranchID) && TerminalType.ToUpper() != "T") ? strBranchId : BranchID;
                TerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : "";
                string strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                string strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                ViewBag.AgentType = strTerminalType == "T" ? strClientType.ToUpper().Trim() : strAgentType.ToUpper().Trim();
                string StrDivideFare = StudentFare + "SPLITSRCAS" + ArmyFare + "SPLITSRCAS" + SnrcitizenFare;
                Session.Add("StudentArmyFareFlag", StrDivideFare);
                string ValKey = "";
                string ClientID_x = ClientID != null && ClientID != "" ? ClientID : Session["POS_ID"].ToString();
                Specialflagfare = string.IsNullOrEmpty(Specialflagfare) ? "" : Specialflagfare;
                if (Trip == "M" && SegmentType == "D")
                {
                    if (TKey != null && TKey != "")
                    {
                        if (TKey.Contains('~'))
                        {
                            string[] Availed = TKey.Split('~');

                            var distinctWords = (from w in Availed
                                                 select w).Distinct().ToList();

                            if (reqcnt == "0")
                            {

                                ValKey = distinctWords[Convert.ToInt32(reqcnt)].ToString().Split('_')[1].ToString().Trim();
                            }
                            else
                            {
                                ValKey = distinctWords[Convert.ToInt32(reqcnt)].ToString().Split('_')[1].ToString().Trim();
                            }


                        }
                        else
                        {
                            string[] Availed = TKey.Split('_');
                            ValKey = Availed[1].ToString().Trim();
                        }


                    }
                }
                else
                {
                    if (TKey != null && TKey != "")
                    {
                        if (TKey.Contains('~'))
                        {
                            string[] Availed = TKey.Split('~');
                            ValKey = Availed[Availed.Count() - 2].ToString().Split('_')[1].ToString().Trim();
                        }
                        else
                        {
                            string[] Availed = TKey.Split('_');
                            ValKey = Availed[1].ToString().Trim();
                        }


                    }
                }

                string RtripComparFlg_S = RtripComparFlg;
                Session["roundtripflg_SESS" + ValKey] = null;
                Session.Add("roundtripflg_SESS" + ValKey, RtripComparFlg_S);

                if (Session["sel_Request"] != null && Session["sel_Request"].ToString() != "")
                {
                    Session["sel_Request"] = FliNum + "SplItRaJeshTrav" + Deaprt + "SplItRaJeshTrav" + Arrive + "SplItRaJeshTrav" + FullFlag + "SplItRaJeshTrav" + TokenKey + "SplItRaJeshTrav" + Trip + "SplItRaJeshTrav" + BaseOrgin
                   + "SplItRaJeshTrav" + BaseDestination + "SplItRaJeshTrav" + offflg + "SplItRaJeshTrav" + Class + "SplItRaJeshTrav" + TKey + "SplItRaJeshTrav" + DEPTDATE + "SplItRaJeshTrav" + ARRDATE + "SplItRaJeshTrav" + Nfarecheck
                   + "SplItRaJeshTrav" + oldfaremarkup + "SplItRaJeshTrav" + mobile + "SplItRaJeshTrav" + bestbuyrequired + "SplItRaJeshTrav" + AlterQueue + "SplItRaJeshTrav" + SegmentType + "SplItRaJeshTrav" + Multireqst
                   + "SplItRaJeshTrav" + reqcnt + "SplItRaJeshTrav" + RtripComparFlg + "SplItRaJeshTrav" + ValKey;
                }
                else
                {
                    Session.Add("sel_Request", FliNum + "SplItRaJeshTrav" + Deaprt + "SplItRaJeshTrav" + Arrive + "SplItRaJeshTrav" + FullFlag + "SplItRaJeshTrav" + TokenKey + "SplItRaJeshTrav" + Trip + "SplItRaJeshTrav" + BaseOrgin
                    + "SplItRaJeshTrav" + BaseDestination + "SplItRaJeshTrav" + offflg + "SplItRaJeshTrav" + Class + "SplItRaJeshTrav" + TKey + "SplItRaJeshTrav" + DEPTDATE + "SplItRaJeshTrav" + ARRDATE + "SplItRaJeshTrav" + Nfarecheck
                    + "SplItRaJeshTrav" + oldfaremarkup + "SplItRaJeshTrav" + mobile + "SplItRaJeshTrav" + bestbuyrequired + "SplItRaJeshTrav" + AlterQueue + "SplItRaJeshTrav" + SegmentType + "SplItRaJeshTrav" + Multireqst
                    + "SplItRaJeshTrav" + reqcnt + "SplItRaJeshTrav" + RtripComparFlg + "SplItRaJeshTrav" + ValKey);
                }

                string balfetch = string.Empty;
                string agninfo = string.Empty;

                #region SERVICE URL BRANCH BASED -- STS115
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (BranchID != "" && strBranchCredit.Contains(BranchID)))
                    {
                        _rays_servers.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                        _inplantservice.Url = ConfigurationManager.AppSettings["TOPUP_APPS_INPLANT_SERVICE"].ToString();
                    }
                    else
                    {
                        _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                        _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                    }
                }
                else
                {
                    _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                }
                #endregion

                string strErrorMsg = string.Empty;

                #region Form Booking Request .......
                string Origin = "";
                string Destination = "";
                string depttime = string.Empty;
                string ArrTime = string.Empty;
                string CNX = string.Empty;
                string cabin, FareID = string.Empty;
                string FareCode = string.Empty;
                string SClass = string.Empty;
                string AirlineCategory = string.Empty;
                string OFFLINEFLAG = offflg;
                string strAdults = "";
                string strChildrens = "";
                string strInfants = "";
                string PlatingCarrier = "";
                string RBDCode = "";
                //string TransactionFlag = "";
                string ConnectionFlag = "";
                string BaseAmount = "";
                string GrossAmount = "";
                string StkType = "";
                string Start_terminal = "";
                string End_terminal = "";


                RQRS.PriceItineary PriceIti = new RQRS.PriceItineary();
                List<RQRS.Itineraries> ListItenar = new List<RQRS.Itineraries>();
                List<RQRS.Flights> FlightsDet = new List<RQRS.Flights>();
                RQRS.Itineraries itinflights = new RQRS.Itineraries();
                RQRS.SegmentDetails SegDet = new RQRS.SegmentDetails();

                RQRS.Flights Itindet = new RQRS.Flights();

                int flightcounts = 0;

                string[] TerminalDetails = new string[] { };
                string[] ArrFliDet, FlightCount, IFCount;

                IFCount = Regex.Split(FullFlag, "SpLiTWeB");

                ListItenar = new List<RQRS.Itineraries>();

                string rtsItinref = "0";


                if (Specialflagfare == "Y" || Specialflagfare == "S" || Specialflagfare == "P")
                {
                    #region Specialflagfare request formation

                    decimal basefare_spl = 0;
                    decimal grosss_spl = 0;
                    string faretypedes = "";
                    string Faretype = "";
                    FlightsDet = new List<RQRS.Flights>();
                    Decimal itenmanul = 0;
                    for (int iF = 0; iF < IFCount.Length; iF++)
                    {
                        if (IFCount[iF].Contains("SpLITSaTIS"))
                        {
                            // FlightCount = FullFlag.Split('>');
                            FlightCount = Regex.Split(IFCount[iF], "SpLITSaTIS");
                            TerminalDetails = new string[FlightCount.Length];



                            for (int fl = 0; fl < FlightCount.Length; fl++)
                            {
                                if (FlightCount[fl] != "")
                                {
                                    itinflights = new RQRS.Itineraries();

                                    flightcounts++;
                                    if (FlightCount[fl].Contains("SpLitPResna"))
                                    {
                                        ArrFliDet = Regex.Split(FlightCount[fl], "SpLitPResna");
                                        if (ArrFliDet[24] != null && ArrFliDet[24] != "")
                                        {
                                            Session.Add("terminaldetails", ArrFliDet[24]);
                                        }
                                        TerminalDetails[fl] = ArrFliDet[24];
                                        if (ArrFliDet[0].Split('~').Length > 1)
                                        {
                                            Origin = ArrFliDet[0].Split('~')[1]; //For Roundtrip Spl RQ from Roundtrip avail for same flights comes with ~ to identify return flight...
                                            rtsItinref = "1";
                                        }
                                        else
                                        {
                                            Origin = ArrFliDet[0];
                                        }
                                        Destination = ArrFliDet[1];
                                        depttime = ArrFliDet[2];
                                        ArrTime = ArrFliDet[3];
                                        SClass = Class;
                                        strAdults = ArrFliDet[6];
                                        strChildrens = ArrFliDet[7];
                                        strInfants = ArrFliDet[8];
                                        PlatingCarrier = ArrFliDet[9];
                                        RBDCode = ArrFliDet[5];

                                        Start_terminal = ArrFliDet[24].Split('\n').Length > 2 ? (ArrFliDet[24].Split('\n')[2].Split(':').Length > 1 ? ArrFliDet[24].Split('\n')[2].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";
                                        End_terminal = ArrFliDet[24].Split('\n').Length > 3 ? (ArrFliDet[24].Split('\n')[3].Split(':').Length > 1 ? ArrFliDet[24].Split('\n')[3].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";

                                        if (fl == 0)
                                        {
                                            basefare_spl = Base.ServiceUtility.ConvertToDecimal(ArrFliDet[13] != null && ArrFliDet[13] != "" ? ArrFliDet[13].ToString() : "0") + Base.ServiceUtility.ConvertToDecimal(basefare_spl.ToString());
                                            grosss_spl = Base.ServiceUtility.ConvertToDecimal(ArrFliDet[14] != null && ArrFliDet[14] != "" ? ArrFliDet[14].ToString() : "0") + Base.ServiceUtility.ConvertToDecimal(grosss_spl.ToString());
                                        }


                                        BaseAmount = ArrFliDet[13];
                                        GrossAmount = ArrFliDet[14];
                                        if (ArrFliDet[20] == "N")
                                        {
                                            Checkgrossamt += Base.ServiceUtility.ConvertToDecimal(ArrFliDet[31]);
                                        }
                                        ConnectionFlag = ArrFliDet[15];
                                        AirlineCategory = ArrFliDet[16];
                                        FareCode = ArrFliDet[17];
                                        cabin = ArrFliDet[21];
                                        FareID = ArrFliDet[25];


                                        faretypedes = ArrFliDet[34];
                                        refuncs = ArrFliDet[32] != null && ArrFliDet[32] != "" ? ArrFliDet[32].ToString() : "";
                                        if (refuncs != null && refuncs != "" && refuncs != "N/A")
                                        {
                                            ViewBag.refuncs = refuncs == "TRUE" ? "Refundable" : "Non-Refundable";
                                        }
                                        else { ViewBag.refuncs = ""; }
                                        if (AirlineCategory != null && AirlineCategory != "" && AirlineCategory.ToString().ToUpper() == "FSC")
                                        {
                                            Session["ALLOW_PAS"] = "Y";
                                        }
                                        else
                                        {
                                            Session["ALLOW_PAS"] = "N";
                                        }

                                        Itindet = new RQRS.Flights();
                                        Itindet.AirlineCategory = AirlineCategory;
                                        Itindet.ArrivalDateTime = ArrTime;
                                        if ((Specialflagfare == "P" || Specialflagfare == "Y") && ArrFliDet[16] == "FSC") //Specialflagfare  P-> Single PNR in Roundtrip , S-> Roundtrip Special PNR , Y-> Single PNR RT to RTspl
                                        {
                                            if (IFCount.Count() - 1 == iF && FlightCount.Length - 1 == fl)
                                            {
                                                Itindet.CNX = "N";
                                            }
                                            else
                                            {
                                                Itindet.CNX = "Y";
                                            }
                                        }
                                        else
                                        {
                                            Itindet.CNX = CNX;
                                        }


                                        Itindet.DepartureDateTime = depttime.Trim();
                                        Itindet.Destination = Destination;
                                        Itindet.FareBasisCode = FareCode.Trim();
                                        Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                                        Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                                        Itindet.PlatingCarrier = PlatingCarrier;
                                        Itindet.Class = RBDCode;//strFareclassOption.ToString().Trim();
                                        Itindet.RBDCode = RBDCode;
                                        Itindet.Cabin = SClass;
                                        Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                                        Itindet.OfflineFlag = OFFLINEFLAG.Trim().Split('~').Length > 1 ? Convert.ToInt16(OFFLINEFLAG.Trim().Split('~')[iF]) : 0; //Convert.ToInt16(OFFLINEFLAG.Trim().Split('~')[iF]);
                                        Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                                        Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                                        Itindet.SegRef = ArrFliDet.Length > 29 ? ArrFliDet[29] : "";
                                        Itindet.FareId = FareID;
                                        Itindet.FareDescription = "";
                                        Itindet.FareTypeDescription = faretypedes;
                                        Itindet.Via = ArrFliDet[33];
                                        Itindet.StartTerminal = Start_terminal;
                                        Itindet.EndTerminal = End_terminal;
                                        Itindet.OperatingCarrier = ArrFliDet[36];
                                        Itindet.FlyingTime = ArrFliDet[22];
                                        Itindet.JourneyTime = ArrFliDet[4];
                                        Itindet.PromoCodeDesc = ArrFliDet[37];
                                        Itindet.PromoCode = ArrFliDet[38];
                                        Itindet.SegmentDetails = ArrFliDet[24];
                                        Itindet.Refundable = ArrFliDet[32];
                                        Faretype = ArrFliDet[35];
                                        if (Multireqst == "Y" || RtripComparFlg == "1")
                                        {
                                            Itindet.ItinRef = (Multireqst == "Y" || RtripComparFlg == "1") ? iF.ToString() : RtripComparFlg == "0" ? rtsItinref : ArrFliDet[19]; //ArrFliDet[19]; 

                                        }
                                        else
                                        {
                                            if (ArrFliDet[20].ToString() == "N")
                                            {
                                                Itindet.ItinRef = itenmanul.ToString(); // drrFlights["itinRef"].ToString();
                                                itenmanul++;
                                            }
                                            else
                                            {
                                                Itindet.ItinRef = itenmanul.ToString(); // drrFlights["itinRef"].ToString();
                                            }
                                        }
                                        StkType = ArrFliDet[23];
                                        strAirlinsecatagory += PlatingCarrier + "|";
                                        FlightsDet.Add(Itindet);
                                    }
                                }
                            }
                        }
                        else
                        {
                            flightcounts++;
                            itinflights = new RQRS.Itineraries();
                            TerminalDetails = new string[1];


                            if (IFCount[iF].Contains("SpLitPResna"))
                            {
                                ArrFliDet = Regex.Split(IFCount[iF], "SpLitPResna");
                                string[] segmentarr = null;
                                if (ArrFliDet[24] != null && ArrFliDet[24] != "")
                                {
                                    string terminaldetail = ArrFliDet[24];
                                    segmentarr = terminaldetail.Split('\n');
                                }
                                TerminalDetails[0] = ArrFliDet[24];

                                Start_terminal = ArrFliDet[24].Split('\n').Length > 2 ? (ArrFliDet[24].Split('\n')[2].Split(':').Length > 1 ? ArrFliDet[24].Split('\n')[2].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";
                                End_terminal = ArrFliDet[24].Split('\n').Length > 3 ? (ArrFliDet[24].Split('\n')[3].Split(':').Length > 1 ? ArrFliDet[24].Split('\n')[3].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";

                                Origin = ArrFliDet[0];
                                Destination = ArrFliDet[1];
                                depttime = ArrFliDet[2];
                                ArrTime = ArrFliDet[3];
                                SClass = Class;
                                strAdults = ArrFliDet[6];
                                strChildrens = ArrFliDet[7];
                                strInfants = ArrFliDet[8];
                                PlatingCarrier = ArrFliDet[9];
                                RBDCode = ArrFliDet[5];
                                BaseAmount = ArrFliDet[13];
                                basefare_spl = Base.ServiceUtility.ConvertToDecimal(ArrFliDet[13] != null && ArrFliDet[13] != "" ? ArrFliDet[13].ToString() : "0") + Base.ServiceUtility.ConvertToDecimal(basefare_spl.ToString());
                                grosss_spl = Base.ServiceUtility.ConvertToDecimal(ArrFliDet[14] != null && ArrFliDet[14] != "" ? ArrFliDet[14].ToString() : "0") + Base.ServiceUtility.ConvertToDecimal(grosss_spl.ToString());

                                GrossAmount = ArrFliDet[14];
                                if (ArrFliDet[20] == "N")
                                {
                                    Checkgrossamt += Base.ServiceUtility.ConvertToDecimal(ArrFliDet[31]);
                                }
                                ConnectionFlag = ArrFliDet[15];
                                AirlineCategory = ArrFliDet[16];
                                FareCode = ArrFliDet[17];
                                FareID = ArrFliDet[25];
                                faretypedes = ArrFliDet[34];
                                refuncs = ArrFliDet[32] != null && ArrFliDet[32] != "" ? ArrFliDet[32].ToString() : "";
                                if (refuncs != null && refuncs != "" && refuncs != "N/A")
                                {
                                    ViewBag.refuncs = refuncs == "TRUE" ? "Refundabe" : "Non-Refundable";
                                }
                                else { ViewBag.refuncs = ""; }
                                if (AirlineCategory != null && AirlineCategory != "" && AirlineCategory.ToString().ToUpper() == "FSC")
                                {
                                    Session["ALLOW_PAS"] = "Y";
                                }
                                else
                                {
                                    Session["ALLOW_PAS"] = "N";
                                }

                                Itindet = new RQRS.Flights();

                                Itindet.AirlineCategory = AirlineCategory;
                                Itindet.ArrivalDateTime = ArrTime;
                                Itindet.DepartureDateTime = depttime.Trim();
                                Itindet.Destination = Destination;
                                Itindet.FareBasisCode = FareCode.Trim();
                                Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                                Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                                Itindet.PlatingCarrier = PlatingCarrier;
                                Itindet.Class = RBDCode;//strFareclassOption.ToString().Trim();
                                Itindet.Cabin = SClass;
                                Itindet.RBDCode = RBDCode;
                                Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                                Itindet.OfflineFlag = OFFLINEFLAG.Trim().Split('~').Length > 1 ? Convert.ToInt16(OFFLINEFLAG.Trim().Split('~')[iF]) : 0;
                                Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                                Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                                Itindet.SegRef = ArrFliDet.Length > 29 ? ArrFliDet[29] : "";
                                Itindet.Via = ArrFliDet[33];
                                Itindet.StartTerminal = Start_terminal;
                                Itindet.EndTerminal = End_terminal;
                                //Itindet.CNX = ArrFliDet[20];
                                Itindet.OperatingCarrier = ArrFliDet[36];
                                Itindet.FlyingTime = ArrFliDet[22];
                                Itindet.JourneyTime = ArrFliDet[4];
                                Itindet.PromoCodeDesc = ArrFliDet[37];
                                Itindet.PromoCode = ArrFliDet[38];
                                Itindet.SegmentDetails = ArrFliDet[24];
                                Itindet.Refundable = ArrFliDet[32];
                                Itindet.FareTypeDescription = faretypedes;
                                if ((Specialflagfare == "Y" || Specialflagfare == "P") && ArrFliDet[16] == "FSC")
                                {
                                    if (IFCount.Count() - 1 == iF)
                                    {
                                        Itindet.CNX = "N";
                                    }
                                    else
                                    {
                                        Itindet.CNX = "Y";
                                    }
                                }
                                else
                                {
                                    Itindet.CNX = CNX;
                                }

                                StkType = ArrFliDet[23];
                                Itindet.FareId = FareID;
                                Itindet.FareDescription = "";

                                Faretype = ArrFliDet[35];
                                if (Multireqst == "Y" || RtripComparFlg == "1")
                                {
                                    Itindet.ItinRef = (Multireqst == "Y" || RtripComparFlg == "1") ? iF.ToString() : ArrFliDet[19]; //ArrFliDet[19];//

                                }
                                else
                                {
                                    if (ArrFliDet[20].ToString() == "N")
                                    {
                                        Itindet.ItinRef = itenmanul.ToString(); // drrFlights["itinRef"].ToString();
                                        itenmanul++;
                                    }
                                    else
                                    {
                                        Itindet.ItinRef = itenmanul.ToString(); // drrFlights["itinRef"].ToString();
                                    }
                                }
                                FlightsDet.Add(Itindet);
                            }
                        }
                    }
                    itinflights.BaseAmount = basefare_spl.ToString();// dtBookFlight.Rows[0]["BasicFare"].ToString();
                    itinflights.GrossAmount = grosss_spl.ToString();// dtBookFlight.Rows[0]["GrossFare"].ToString();
                    itinflights.FlightDetails = FlightsDet;
                    itinflights.Stock = StkType;
                    itinflights.FareType = Faretype;
                    itinflights.FareTypeDescription = faretypedes != null && faretypedes != "" ? faretypedes : "";
                    itinflights.isStudentFare = StudentFare;
                    itinflights.isArmyFare = ArmyFare;
                    itinflights.isSnrCitizenFare = SnrcitizenFare;
                    itinflights.isLabourFare = faretypedes.ToUpper().ToString() == "LABOUR FARE" ? true : false;
                    ListItenar.Add(itinflights);

                    #endregion normal condition

                }
                else
                {
                    #region normal request formation

                    for (int iF = 0; iF < IFCount.Length; iF++)
                    {
                        string Faretype = string.Empty;

                        if (IFCount[iF].Contains("SpLITSaTIS"))
                        {
                            FlightCount = Regex.Split(IFCount[iF], "SpLITSaTIS");
                            TerminalDetails = new string[FlightCount.Length];
                            string faretypedes = "";

                            FlightsDet = new List<RQRS.Flights>();

                            for (int fl = 0; fl < FlightCount.Length; fl++)
                            {
                                if (FlightCount[fl] != "")
                                {
                                    itinflights = new RQRS.Itineraries();

                                    flightcounts++;
                                    if (FlightCount[fl].Contains("SpLitPResna"))
                                    {
                                        ArrFliDet = Regex.Split(FlightCount[fl], "SpLitPResna");
                                        if (ArrFliDet[24] != null && ArrFliDet[24] != "")
                                        {
                                            Session.Add("terminaldetails", ArrFliDet[24]);
                                        }
                                        TerminalDetails[fl] = ArrFliDet[24];
                                        if (ArrFliDet[0].Split('~').Length > 1)
                                        {
                                            Origin = ArrFliDet[0].Split('~')[1]; //For Roundtrip Spl RQ from Roundtrip avail for same flights comes with ~ to identify return flight...
                                            rtsItinref = "1";
                                        }
                                        else
                                        {
                                            Origin = ArrFliDet[0];
                                        }
                                        Destination = ArrFliDet[1];
                                        depttime = ArrFliDet[2];
                                        ArrTime = ArrFliDet[3];
                                        SClass = Class;
                                        strAdults = ArrFliDet[6];
                                        strChildrens = ArrFliDet[7];
                                        strInfants = ArrFliDet[8];
                                        PlatingCarrier = ArrFliDet[9];
                                        RBDCode = ArrFliDet[5];
                                        BaseAmount = ArrFliDet[13];
                                        GrossAmount = ArrFliDet[14];
                                        if (ArrFliDet[20] == "N")
                                        {
                                            Checkgrossamt += Base.ServiceUtility.ConvertToDecimal(ArrFliDet[31]);
                                        }
                                        ConnectionFlag = ArrFliDet[15];
                                        AirlineCategory = ArrFliDet[16];
                                        FareCode = ArrFliDet[17];
                                        cabin = ArrFliDet[21];
                                        FareID = ArrFliDet[25];
                                        faretypedes = ArrFliDet[34];
                                        Faretype = ArrFliDet[35];
                                        refuncs = ArrFliDet[32] != null && ArrFliDet[32] != "" ? ArrFliDet[32].ToString() : "";


                                        Start_terminal = ArrFliDet[24].Split('\n').Length > 2 ? (ArrFliDet[24].Split('\n')[2].Split(':').Length > 1 ? ArrFliDet[24].Split('\n')[2].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";
                                        End_terminal = ArrFliDet[24].Split('\n').Length > 3 ? (ArrFliDet[24].Split('\n')[3].Split(':').Length > 1 ? ArrFliDet[24].Split('\n')[3].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";


                                        if (refuncs != null && refuncs != "" && refuncs != "N/A")
                                        {
                                            ViewBag.refuncs = refuncs == "TRUE" ? "Refundable" : "Non-Refundable";
                                        }
                                        else { ViewBag.refuncs = ""; }

                                        if (AirlineCategory != null && AirlineCategory != "" && AirlineCategory.ToString().ToUpper() == "FSC")
                                        {
                                            Session["ALLOW_PAS"] = "Y";
                                        }
                                        else
                                        {
                                            Session["ALLOW_PAS"] = "N";
                                        }

                                        Itindet = new RQRS.Flights();
                                        Itindet.AirlineCategory = AirlineCategory;
                                        Itindet.ArrivalDateTime = ArrTime;
                                        Itindet.CNX = CNX;
                                        Itindet.DepartureDateTime = depttime.Trim();
                                        Itindet.Destination = Destination;
                                        Itindet.FareBasisCode = FareCode.Trim();
                                        Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                                        Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                                        Itindet.PlatingCarrier = PlatingCarrier;
                                        Itindet.Class = RBDCode;//strFareclassOption.ToString().Trim();
                                        Itindet.RBDCode = RBDCode;
                                        Itindet.Cabin = SClass;
                                        Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                                        Itindet.OfflineFlag = OFFLINEFLAG.Trim().Split('~').Length > 1 ? Convert.ToInt16(OFFLINEFLAG.Trim().Split('~')[iF]) : 0; //Convert.ToInt16(OFFLINEFLAG.Trim().Split('~')[iF]);
                                        Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                                        Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                                        Itindet.ItinRef = (Multireqst == "Y" || RtripComparFlg == "1") ? iF.ToString() : RtripComparFlg == "0" ? rtsItinref : ArrFliDet[19];// ArrFliDet[19]; // 
                                        Itindet.SegRef = ArrFliDet.Length > 29 ? ArrFliDet[29] : "";
                                        Itindet.CNX = ArrFliDet[20];//  RtripComparFlg != "0" ? ArrFliDet[20] : (fl != FlightCount.Length - 1 ? "Y" : "N"); //Roundtrip Spl RQ from Roundtrip Avail... tat's y CNX hardcoded...
                                        Itindet.FareId = FareID;
                                        StkType = ArrFliDet[23];
                                        Itindet.FareDescription = "";
                                        Itindet.FareTypeDescription = faretypedes;
                                        Itindet.OperatingCarrier = ArrFliDet[36];
                                        strAirlinsecatagory += PlatingCarrier + "|";
                                        Itindet.Via = ArrFliDet[33];
                                        Itindet.StartTerminal = Start_terminal;
                                        Itindet.EndTerminal = End_terminal;
                                        Itindet.FlyingTime = ArrFliDet[22];
                                        Itindet.JourneyTime = ArrFliDet[4];
                                        Itindet.PromoCodeDesc = ArrFliDet[37];
                                        Itindet.PromoCode = ArrFliDet[38];
                                        Itindet.SegmentDetails = ArrFliDet[24];
                                        Itindet.Refundable = ArrFliDet[32];
                                        FlightsDet.Add(Itindet);

                                    }
                                    itinflights.BaseAmount = BaseAmount;// dtBookFlight.Rows[0]["BasicFare"].ToString();
                                    itinflights.GrossAmount = GrossAmount;// dtBookFlight.Rows[0]["GrossFare"].ToString();
                                    itinflights.FlightDetails = FlightsDet;
                                    itinflights.Stock = StkType;
                                    itinflights.FareType = Faretype;
                                    itinflights.FareTypeDescription = faretypedes != null && faretypedes != "" ? faretypedes : "";
                                    itinflights.isStudentFare = StudentFare;
                                    itinflights.isArmyFare = ArmyFare;
                                    itinflights.isSnrCitizenFare = SnrcitizenFare;
                                    itinflights.isLabourFare = faretypedes.ToUpper().ToString() == "LABOUR FARE" ? true : false;
                                }
                            }
                            ListItenar.Add(itinflights);

                        }
                        else
                        {
                            flightcounts++;
                            itinflights = new RQRS.Itineraries();
                            FlightsDet = new List<RQRS.Flights>();
                            TerminalDetails = new string[1];
                            string faretypedes = "";
                            if (IFCount[iF].Contains("SpLitPResna"))
                            {
                                // ArrFliDet = FullFlag.Split('~');
                                ArrFliDet = Regex.Split(IFCount[iF], "SpLitPResna");


                                string[] segmentarr = null;
                                if (ArrFliDet[24] != null && ArrFliDet[24] != "")
                                {
                                    string terminaldetail = ArrFliDet[24];
                                    segmentarr = terminaldetail.Split('\n');
                                }
                                TerminalDetails[0] = ArrFliDet[24];
                                Origin = ArrFliDet[0];
                                Destination = ArrFliDet[1];
                                depttime = ArrFliDet[2];
                                ArrTime = ArrFliDet[3];
                                SClass = Class;
                                strAdults = ArrFliDet[6];
                                strChildrens = ArrFliDet[7];
                                strInfants = ArrFliDet[8];
                                PlatingCarrier = ArrFliDet[9];
                                RBDCode = ArrFliDet[5];
                                BaseAmount = ArrFliDet[13];
                                GrossAmount = ArrFliDet[14];
                                if (ArrFliDet[20] == "N")
                                {
                                    Checkgrossamt += Base.ServiceUtility.ConvertToDecimal(ArrFliDet[31]);
                                }
                                ConnectionFlag = ArrFliDet[15];
                                AirlineCategory = ArrFliDet[16];
                                FareCode = ArrFliDet[17];
                                FareID = ArrFliDet[25];


                                Start_terminal = ArrFliDet[24].Split('\n').Length > 2 ? (ArrFliDet[24].Split('\n')[2].Split(':').Length > 1 ? ArrFliDet[24].Split('\n')[2].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";
                                End_terminal = ArrFliDet[24].Split('\n').Length > 3 ? (ArrFliDet[24].Split('\n')[3].Split(':').Length > 1 ? ArrFliDet[24].Split('\n')[3].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";


                                refuncs = ArrFliDet[32] != null && ArrFliDet[32] != "" ? ArrFliDet[32].ToString() : "";
                                if (refuncs != null && refuncs != "" && refuncs != "N/A")
                                {
                                    ViewBag.refuncs = refuncs == "TRUE" ? "Refundable" : "Non-Refundable";
                                }
                                else { ViewBag.refuncs = ""; }

                                if (AirlineCategory != null && AirlineCategory != "" && AirlineCategory.ToString().ToUpper() == "FSC")
                                {
                                    Session["ALLOW_PAS"] = "Y";
                                }
                                else
                                {
                                    Session["ALLOW_PAS"] = "N";
                                }

                                faretypedes = ArrFliDet[34];

                                Faretype = ArrFliDet[35];
                                Itindet = new RQRS.Flights();

                                Itindet.AirlineCategory = AirlineCategory;
                                Itindet.ArrivalDateTime = ArrTime;
                                Itindet.DepartureDateTime = depttime.Trim();
                                Itindet.Destination = Destination;
                                Itindet.FareBasisCode = FareCode.Trim();
                                Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                                Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                                Itindet.PlatingCarrier = PlatingCarrier;
                                Itindet.Class = RBDCode;//strFareclassOption.ToString().Trim();
                                Itindet.Cabin = SClass;
                                Itindet.RBDCode = RBDCode;
                                Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                                Itindet.OfflineFlag = OFFLINEFLAG.Trim().Split('~').Length > 1 ? Convert.ToInt16(OFFLINEFLAG.Trim().Split('~')[iF]) : 0;
                                Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                                Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                                Itindet.ItinRef = (Multireqst == "Y" || RtripComparFlg == "1") ? iF.ToString() : ArrFliDet[19]; //ArrFliDet[19];// 
                                Itindet.SegRef = ArrFliDet.Length > 29 ? ArrFliDet[29] : "";
                                Itindet.CNX = ArrFliDet[20];
                                StkType = ArrFliDet[23];
                                Itindet.FareDescription = "";
                                Itindet.FareTypeDescription = faretypedes;
                                Itindet.FareId = FareID;
                                Itindet.Via = ArrFliDet[33];
                                Itindet.StartTerminal = Start_terminal;
                                Itindet.EndTerminal = End_terminal;
                                Itindet.OperatingCarrier = ArrFliDet[36];
                                Itindet.FlyingTime = ArrFliDet[22];
                                Itindet.JourneyTime = ArrFliDet[4];
                                Itindet.PromoCodeDesc = ArrFliDet[37];
                                Itindet.PromoCode = ArrFliDet[38];
                                Itindet.SegmentDetails = ArrFliDet[24];
                                Itindet.Refundable = ArrFliDet[32];
                                FlightsDet.Add(Itindet);
                            }

                            itinflights.BaseAmount = BaseAmount;// dtBookFlight.Rows[0]["BasicFare"].ToString();
                            itinflights.GrossAmount = GrossAmount;// dtBookFlight.Rows[0]["GrossFare"].ToString();
                            itinflights.FlightDetails = FlightsDet;
                            itinflights.Stock = StkType;
                            itinflights.FareType = Faretype;
                            itinflights.FareTypeDescription = faretypedes != null && faretypedes != "" ? faretypedes : "";
                            itinflights.isStudentFare = StudentFare;
                            itinflights.isArmyFare = ArmyFare;
                            itinflights.isSnrCitizenFare = SnrcitizenFare;
                            itinflights.isLabourFare = faretypedes.ToUpper().ToString() == "LABOUR FARE" ? true : false;
                            ListItenar.Add(itinflights);
                        }

                    }

                    #endregion normal condition
                }

                SegDet.BaseOrigin = BaseOrgin;
                SegDet.BaseDestination = BaseDestination;
                SegDet.Adult = strAdults;
                SegDet.Child = strChildrens != null && strChildrens != "" ? strChildrens.Trim() : "0";
                SegDet.Infant = strInfants != null && strInfants != "" ? strInfants.Trim() : "0";
                SegDet.SegmentType = SegmentType; //Trip=="R"?"D":"I";
                //   SegDet.RTSpecial = "";

                SegDet.ClassType = SClass;
                SegDet.BookingType = "B2B";
                //SegDet.TripType = (Trip == "Y" && Session["tripConfig"] != null && !Session["tripConfig"].ToString().Contains('R')) ? "R" : Trip;
                // SegDet.TripType = (Trip == "Y" && Specialflagfare == "Y") ? "Y" : Trip == "Y" ? "R" : Trip;
                //SegDet.TripType = Trip;
                SegDet.TripType = (ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA" && SegmentType == "I" && Trip == "Y") ? "R" : (ConfigurationManager.AppSettings["Producttype"].ToString() != "RIYA" && Trip == "Y" ? "R" : Trip);

                SegDet.AppType = strTerminalType;
                SegDet.SinglePNR = RtripComparFlg;// RtripComparFlg; //1-SinglePNR for Roundtrip Avail, 0-Roundtrip spl rq for Roundtrip avail (compare popup), "" means else part... by saranraj...
                SegDet.RTSpecial = Specialflagfare == "Y" ? "Y" : "";   //added by srinath--round trip sple fare hit(second hit)

                string strClientID = string.Empty; string strClientTerminalID = string.Empty;
                if (strTerminalType == "T" && ClientID != "")
                {
                    strClientID = ClientID.Substring(0, (ClientID.Length - 2));
                    strClientTerminalID = ClientID;
                }
                else
                {
                    strClientID = Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                    strClientTerminalID = Session["POS_TID"] != null && Session["POS_TID"] != "" ? Session["POS_TID"].ToString() : "";
                }


                RQRS.AgentDetails agent = new RQRS.AgentDetails();
                agent.AgentId = Session["agentid"] != null && Session["agentid"] != "" ? Session["agentid"].ToString() : "";
                agent.Agenttype = strTerminalType == "T" ? strClientType : strAgentType;
                agent.AirportID = SegmentType;//"D";

                agent.AppType = "B2B";
                agent.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                agent.BOAID = strClientID;
                agent.BOAterminalID = strClientTerminalID;
                agent.BranchID = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                agent.IssuingBranchID = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                //agent.IssuingBranchID = Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                agent.ClientID = strClientID;
                agent.CoOrdinatorID = "";
                agent.Environment = strTerminalType;
                agent.Platform = "B"; //ABCD
                agent.ProjectID = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"] != "" ? Session["PRODUCT_CODE"].ToString() : "";
                agent.TerminalId = Session["terminalid"] != null && Session["terminalid"] != "" ? Session["terminalid"].ToString() : "";
                agent.UserName = Session["username"].ToString();
                agent.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                agent.COST_CENTER = "";
                agent.EMP_ID = "";
                agent.UID = Session["UniqueIduser"] != null ? Session["UniqueIduser"].ToString() : "";
                agent.TID = "";
                agent.Group_ID = GroupID != null && GroupID != "" ? GroupID : "";

                PriceIti.ItinearyDetails = ListItenar;
                PriceIti.BestBuyOption = bestbuyrequired == "TRUE" ? true : false;
                PriceIti.SegmnetDetails = SegDet;
                PriceIti.AgentDetails = agent;
                PriceIti.Stock = StkType;
                PriceIti.AlterQueue = AlterQueue;

                //ViewBag.FltDets = JsonConvert.SerializeObject(ListItenar);//For showing Flight details in Razor view...
                #endregion

                #region Send Request to RemoteServer......

                try
                {
                    ViewBag.txt_UserName = Session["username"].ToString();
                    string[] PP = Session["agencyaddress"].ToString().Split('~');
                    if (PP.Count() >= 4)
                    {
                        ViewBag.txt_AgnNo = PP[4];
                    }
                    string request = JsonConvert.SerializeObject(PriceIti).ToString();
                    if (Session["Pricerequest"] != null && Session["Pricerequest"] != "")
                    {
                        Session["Pricerequest"] = request.ToString();
                    }
                    else
                    {
                        Session.Add("Pricerequest", request);
                    }
                    string Query = "InvokeHostCheck";
                    int hostchecktimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);

                    MyWebClient client = new MyWebClient();
                    client.LintTimeout = hostchecktimeout;
                    client.Headers["Content-type"] = "application/json";

                    #region Log

                    string ReqTime = "SelectReqest" + DateTime.Now;

                    StringWriter strWriter = new StringWriter();
                    DataSet dsrequest = new DataSet();
                    dsrequest = Serv.convertJsonStringToDataSet(request, "");
                    dsrequest.WriteXml(strWriter);

                    if (ConfigurationManager.AppSettings["Splavailagent"].ToString() != "" && ConfigurationManager.AppSettings["Splavailagent"].ToString().Contains(Session["POS_TID"].ToString()))
                    {
                        strURLpath = ConfigurationManager.AppSettings["Spl_APPS_SELECT_URL"].ToString();
                    }
                    else
                    {
                        strURLpath = ConfigurationManager.AppSettings["APPS_SELECT_URL"].ToString();
                    }


                    string lstrCat = Trip + " ~SRQ~" + PriceIti.Stock.ToString()
                        + "~" + BaseOrgin + "~" + BaseDestination;


                    string LstrDetails = "<SELECT_REQUEST><URL>[<![CDATA[" + strURLpath
                        + "]]>]</URL><QUERY>" + Query + "</QUERY><REQTIME>" + ReqTime + "</REQTIME><TIMEOUT>" + (hostchecktimeout).ToString() + "</TIMEOUT>" + ((Base.ReqLog) ?
                        strWriter.ToString() : request) + "<JSON>" + request + "</JSON></SELECT_REQUEST>";


                    DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCat + " :SELECT REQUEST", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                    #endregion

                    byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                    string strResponse = System.Text.Encoding.ASCII.GetString(data);
                    /****/


                    if (Trip == "M" && SegmentType == "D")
                    {
                        if (Session["Priceresponse" + ValKey] != null && Session["Priceresponse"] != "")
                        {
                            Session["Priceresponse" + ValKey] = strResponse.ToString();
                        }
                        else
                        {
                            Session.Add("Priceresponse" + ValKey, strResponse);
                        }
                    }
                    else
                    {
                        if (Session["Priceresponse"] != null && Session["Priceresponse"] != "")
                        {
                            Session["Priceresponse"] = strResponse.ToString();
                        }
                        else
                        {
                            Session.Add("Priceresponse", strResponse);
                        }
                    }

                    ViewBag.triptype = Trip;

                    System.Web.HttpContext.Current.Session["roundtripflg_SESS" + ValKey] = null;
                    System.Web.HttpContext.Current.Session["roundtripflg_SESS" + ValKey] = Trip == "R" && Specialflagfare != "Y" && Specialflagfare != "P" ? "1" : "";

                    if (string.IsNullOrEmpty(strResponse))
                    {

                        #region log
                        string lstrCata = Trip + " ~SRN~" +
                           PriceIti.Stock.ToString()
                       + "~" + BaseOrgin + "~" + BaseDestination;

                        DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE NULL", "Null Or Empty", Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                        #endregion

                    }
                    else
                    {
                        DataSet dsselect = new DataSet();
                        dsselect = Serv.convertJsonStringToDataSet(strResponse, "");
                        DataTable dsselect1 = new DataTable();
                        dsselect1 = dsselect.Tables[0];
                        Session.Add("Dobmand" + ValKey, dsselect1);
                        Session.Add("valkey", ValKey);
                        string Erroralert = string.Empty;
                        _availRes = JsonConvert.DeserializeObject<RQRS.PriceItenaryRS>(strResponse);
                        ViewBag.GSTresultcode = "0";
                        if (_availRes.ResultCode == "1" || _availRes.ResultCode == "2")
                        {
                            #region Get Existing GST Details (Added on 20190208 by STS185)
                            DataSet dsRequest = new DataSet();
                            dsselect = Serv.convertJsonStringToDataSet(request, "");

                            string strRequestDetails = JsonConvert.SerializeObject(dsselect);
                            DataTable dtGSTExistingDetails = new DataTable();
                            DataSet dsGSTExistingDetails = new DataSet();
                            string gstMandatory = string.Empty;
                            string strDispInfoMessage = string.Empty;
                            DataTable getdtGSTRegisterDetails = new DataTable();
                            DataTable getBTACardtype = new DataTable();
                            DataTable getPaxtitle = new DataTable();

                            string getGSTMandatory = string.Empty;
                            bool getLastNameMandatory = false;
                            bool getAllowLCCBlockPNR = false;

                            try
                            {
                                string errormsg = string.Empty;
                                ViewBag.GSTpopupmsg = "";

                                _rays_servers.FecthGSTandPopupMsgForClient(strClientID, strClientTerminalID, strRequestDetails,
                                    strUserName, "", Ipaddress, sequnceID.ToString(), ref gstMandatory, ref getLastNameMandatory,
                                    ref dsGSTExistingDetails, ref strDispInfoMessage, TerminalType, ref errormsg, strPlatform);

                                string LstrDetails1 = "<FETCH_GST_DETAILS><URL>[<![CDATA[" + ConfigurationManager.AppSettings["ServiceURI"].ToString()
                                    + "]]>]</URL><QUERY>" + Query + "</QUERY><REQTIME>" + ReqTime + "</REQTIME><TIMEOUT>" + (hostchecktimeout).ToString() + "</TIMEOUT>" + (strWriter.ToString()) + "</FETCH_GST_DETAILS><RESPONSE><GST_MANDATORY>" + gstMandatory
                                    + "</GST_MANDATORY><LASTNAMEMAND>" + getLastNameMandatory + "</LASTNAMEMAND><GSTDETAILS>" + dsGSTExistingDetails.GetXml() + "</GSTDETAILS><ALERTMSG>" + strDispInfoMessage + "</ALERTMSG></RESPONSE>";

                                DatabaseLog.LogData(Session["username"].ToString(), "S", "FetchGSTDetails", "FetchGSTDetails", LstrDetails1, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());


                                //Session.Add("PaxLastnameMand", getLastNameMandatory);
                                string Lastnamefield = getLastNameMandatory == true ? "1" : "0"; /* True for last name mandatory False for Last name not mandatory*/
                                ViewBag.PaxLastnameMand = Lastnamefield;

                                if ((PriceIti.ItinearyDetails[0].FlightDetails[0].AirlineCategory == "LCC"))
                                {
                                    Session["ALLOW_CREDIT"] = "N";
                                }

                                if (dsGSTExistingDetails != null && dsGSTExistingDetails.Tables.Count > 0)
                                {
                                    DataTable dtInsuranceDetails = new DataTable();
                                    dtInsuranceDetails = dsGSTExistingDetails.Tables.Contains("INSURANCE_FARE") && dsGSTExistingDetails.Tables["INSURANCE_FARE"].Rows.Count > 0 ? dsGSTExistingDetails.Tables["INSURANCE_FARE"].Copy() : null;
                                    Session["INSURANCEDETAILS"] = dtInsuranceDetails;

                                    getdtGSTRegisterDetails = dsGSTExistingDetails.Tables.Contains("GSTDETAILS") && dsGSTExistingDetails.Tables["GSTDETAILS"].Rows.Count > 0 ? dsGSTExistingDetails.Tables["GSTDETAILS"].Copy() : null;
                                    getBTACardtype = dsGSTExistingDetails.Tables.Contains("P_LOAD_MANUAL_GST2") && dsGSTExistingDetails.Tables["P_LOAD_MANUAL_GST2"].Rows.Count > 0 ? dsGSTExistingDetails.Tables["P_LOAD_MANUAL_GST2"].Copy() : null;
                                    gstMandatory = (gstMandatory != null) ? gstMandatory : "";// "1~1|1|1|1";
                                    string checkmandatory = (gstMandatory.Length > 2) ? gstMandatory.Split('~')[0] : "";
                                    if (checkmandatory != "" && checkmandatory == "1")
                                    {
                                        array_Response[ResultCode] = 1;
                                        ViewBag.GSTresultcode = "4";
                                        ViewBag.status = "04";
                                        ViewBag.GSTpopupmsg = (gstMandatory != "") ? strDispInfoMessage : "";// "GST is mandatory";
                                    }

                                    if (getdtGSTRegisterDetails != null && dsGSTExistingDetails.Tables["GSTDETAILS"].Rows.Count > 0)
                                    {
                                        ViewData["GSTDETAILS"] = JsonConvert.SerializeObject(getdtGSTRegisterDetails);
                                    }
                                    else
                                    {
                                        ViewData["GSTDETAILS"] = "";
                                    }

                                    if (dsGSTExistingDetails.Tables.Contains("PAX_TITLE") == true && dsGSTExistingDetails.Tables["PAX_TITLE"].Rows.Count != 0)
                                    {
                                        DataTable Paxdetails = dsGSTExistingDetails.Tables["PAX_TITLE"].Copy();
                                        ViewData["PAXTITLE"] = JsonConvert.SerializeObject(Paxdetails);
                                    }
                                    else
                                    {
                                        ViewData["PAXTITLE"] = "";
                                    }

                                    if (dsGSTExistingDetails.Tables.Contains("P_FETCH_POS_CREDIT_BALANCE") == true && dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows.Count != 0)
                                    {
                                        Allowbokingtype = dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CLT_ALLOW_ACC_TYPE"].ToString().Trim();

                                        if (Allowbokingtype.Contains("C") && (PriceIti.ItinearyDetails[0].FlightDetails[0].AirlineCategory != "LCC"))
                                        {
                                            Session["ALLOW_CREDIT"] = "Y";
                                        }
                                        else
                                        {
                                            Session["ALLOW_CREDIT"] = "N";
                                        }

                                        string strCustomerLogin = ((Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() != "") ? Session["CustomerLogin"].ToString() : "N");

                                        if (strCustomerLogin != "Y")
                                        {
                                            if (Allowbokingtype.Contains("T"))
                                            {
                                                Session["ALLOW_TOPUP"] = "Y";
                                            }
                                            else
                                            {
                                                Session["ALLOW_TOPUP"] = "N";
                                            }
                                        }
                                        if (Allowbokingtype.Contains("P")) // Domestic MultiCity PG Options Block
                                        {
                                            if (Trip == "M" && SegmentType == "D")
                                                Session["ALLOW_PG"] = "N";
                                            else
                                                Session["ALLOW_PG"] = "Y";
                                        }
                                        else
                                        {
                                            Session["ALLOW_PG"] = "N";
                                        }
                                        if (Allowbokingtype.Contains("B"))
                                        {
                                            Session["ALLOW_PAS"] = "Y";
                                        }
                                        else
                                        {
                                            Session["ALLOW_PAS"] = "N";
                                        }
                                        if (TerminalType.ToUpper().ToString() == "T")
                                        {
                                            string ccur = dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CLT_ALLOW_ACC_TYPE"].ToString().Trim();
                                            string appcurrency = dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CCD_CURRENCY_CODE"].ToString().Trim();
                                            string[] ss = ccur.Split('|');
                                            if (ss.Length > 0)
                                            {
                                                ViewBag.CltTopBalance= (dsGSTExistingDetails.Tables.Contains("P_FETCH_POS_CREDIT_BALANCE") && dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows.Count > 0) ? dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CLT_OPENING_BALANCE"].ToString() : "";
                                                ViewBag.CltCreditBalance = (dsGSTExistingDetails.Tables.Contains("P_FETCH_POS_CREDIT_BALANCE") && dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows.Count > 0) ? dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CREDITBALANCE"].ToString() : "";

                                                for (int j = 0; j < ss.Length; j++)
                                                {
                                                    if (ss[j] == "C")
                                                    {
                                                        balfetch += "Credit : " + appcurrency + " " + dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CREDITBALANCE"].ToString() + "|";
                                                    }
                                                    if (ss[j] == "T")
                                                    {
                                                        balfetch += "Topup : " + appcurrency + " " + dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CLT_OPENING_BALANCE"].ToString() + "|";
                                                    }
                                                }
                                            }
                                            if (dsGSTExistingDetails.Tables.Count > 8 && dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE6"].Rows.Count > 0)
                                            {
                                                agninfo = dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE6"].Rows[0]["CLT_CLIENT_ID"].ToString() + "~" + dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE6"].Rows[0]["CLT_CLIENT_NAME"].ToString() + "~" + dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE6"].Rows[0]["CLT_WINYATRA_ID"].ToString();
                                            }
                                        }
                                        else {
                                            ViewBag.CltTopBalance = (dsGSTExistingDetails.Tables.Contains("P_FETCH_POS_CREDIT_BALANCE") && dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows.Count > 0) ? dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CLT_OPENING_BALANCE"].ToString() : "";
                                            ViewBag.CltCreditBalance = (dsGSTExistingDetails.Tables.Contains("P_FETCH_POS_CREDIT_BALANCE") && dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows.Count > 0) ? dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CREDITBALANCE"].ToString() : "";
                                        }
                                        ViewBag.balsfet = balfetch;
                                    }
                                    else
                                    {
                                        if ((PriceIti.ItinearyDetails[0].FlightDetails[0].AirlineCategory == "LCC"))
                                        {
                                            Session["ALLOW_CREDIT"] = "N";
                                        }
                                    }

                                    #region // Fund control RBOA jum Booking balance check

                                    if (ConfigurationManager.AppSettings["ProductType"] == "RBOA")
                                    {
                                        DataSet dsTopupbalance = new DataSet();

                                        dsTopupbalance = _rays_servers.Fetch_Client_Credit_Balance_DetailsWEB(strClientID, strUserName, Ipaddress, sequnceID, strPlatform);

                                        string strAllowBookingType = (dsTopupbalance.Tables[0].Columns.Contains("CLT_ALLOW_ACC_TYPE")) ? dsTopupbalance.Tables[0].Rows[0]["CLT_ALLOW_ACC_TYPE"].ToString() : "";
                                        if (strAllowBookingType.Contains("C") && (PriceIti.ItinearyDetails[0].FlightDetails[0].AirlineCategory != "LCC"))
                                        {
                                            Session["ALLOW_CREDIT"] = "Y";
                                        }
                                        else
                                        {
                                            Session["ALLOW_CREDIT"] = "N";
                                        }
                                        if (strAllowBookingType.Contains("T"))
                                        {
                                            Session["ALLOW_TOPUP"] = "Y";
                                        }
                                        else
                                        {
                                            Session["ALLOW_TOPUP"] = "N";
                                        }
                                        if (strAllowBookingType.Contains("B"))
                                        {
                                            Session["ALLOW_PAS"] = "Y";
                                        }
                                        else
                                        {
                                            Session["ALLOW_PAS"] = "N";
                                        }

                                        ViewBag.AgentTopupbal = dsTopupbalance.Tables[0].Rows[0]["CLT_OPENING_BALANCE"].ToString();
                                        if (dsTopupbalance.Tables[0].Columns.Contains("CLT_ERP_CUST_TYPE") && dsTopupbalance.Tables[0].Rows[0]["CLT_ERP_CUST_TYPE"].ToString() == "B2C" &&
                                            dsTopupbalance.Tables[0].Columns.Contains("CLT_CREDIT_ERP_ID"))
                                        {
                                            if (dsTopupbalance.Tables[0].Rows[0]["CLT_CREDIT_ERP_ID"].ToString() == "")
                                            {
                                                Session["ALLOW_CREDIT"] = "N";
                                            }
                                            else
                                            {
                                                Session["ALLOW_TOPUP"] = "N";
                                            }
                                        }

                                        if (dsTopupbalance.Tables[0].Columns.Contains("CLT_CREDIT_ERP_ID") && dsTopupbalance.Tables[0].Rows[0]["CLT_CREDIT_ERP_ID"].ToString() != "")
                                        {
                                            Session["ALLOW_CREDIT"] = "Y";
                                        }

                                        bool blnAllowBranchTopup = false;
                                        if (dsTopupbalance.Tables[0].Columns.Contains("ALLOW_BRANCH_TOPUP") && dsTopupbalance.Tables[0].Rows[0]["ALLOW_BRANCH_TOPUP"].ToString() != "")
                                        {
                                            blnAllowBranchTopup = (dsTopupbalance.Tables[0].Rows[0]["ALLOW_BRANCH_TOPUP"].ToString() == "Y" ? true : false);
                                            Session["ALLOW_BRANCH_TOPUP"] = dsTopupbalance.Tables[0].Rows[0]["ALLOW_BRANCH_TOPUP"].ToString();
                                        }

                                        if (dsTopupbalance.Tables[0].Columns.Contains("CLT_ALLOW_ACC_TYPE") && dsTopupbalance.Tables[0].Rows[0]["CLT_ALLOW_ACC_TYPE"].ToString() != ""
                                            && dsTopupbalance.Tables[0].Rows[0]["CLT_ALLOW_ACC_TYPE"].ToString().Contains("H"))
                                        {
                                            string strAllowCashOption = string.Empty;
                                            bool blnAllowCashOption = false;
                                            foreach (var _priRes in PriceIti.ItinearyDetails)
                                            {
                                                strAllowCashOption = _priRes.FlightDetails[0].AirlineCategory.ToString();
                                            }
                                            if (strAllowCashOption.Contains("FSC") && !strAllowCashOption.Contains("LCC"))
                                            {
                                                blnAllowCashOption = true;
                                            }
                                            else if (strAllowCashOption.Contains("LCC") && blnAllowBranchTopup == true)
                                            {
                                                blnAllowCashOption = true;
                                            }
                                            Session.Add("ALLOW_CASH", blnAllowCashOption == true ? "Y" : "N");
                                        }
                                        else
                                        {
                                            Session.Add("ALLOW_CASH", "N");
                                        }

                                        ViewBag.BOAAgentType = dsTopupbalance.Tables[0].Columns.Contains("CLT_ERP_CUST_TYPE") ? dsTopupbalance.Tables[0].Rows[0]["CLT_ERP_CUST_TYPE"].ToString().ToUpper().Trim() : "B2B";

                                        ViewBag.AgentCreditbal = dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CREDITBALANCE"].ToString();
                                    }
                                    else { ViewBag.AgentTopupbal = "0"; ViewBag.AgentCreditbal = "0"; }

                                    if (strBranchCredit != "")
                                    {
                                        if (strBranchCredit == "ALL" || (BranchID != "" && strBranchCredit.Contains(BranchID)))
                                        {
                                            if (dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CLT_CREDIT_ERP_ID"].ToString() == "")
                                            {
                                                Session["ALLOW_CREDIT"] = "N";
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region log
                                    string lstrCata = Trip + " ~SRN~" +
                                       PriceIti.Stock.ToString()
                                   + "~" + BaseOrgin + "~" + BaseDestination;

                                    DatabaseLog.LogData(Session["username"].ToString(), "S", "FlightController.cs", lstrCata + " :GST RESPONSE NULL", "Null Or Empty", Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                    #endregion
                                }
                            }
                            catch (Exception ex)
                            {
                                getdtGSTRegisterDetails = null;
                                getLastNameMandatory = true;
                            }

                            List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;
                            List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;

                            bool hdn_allowblockpnr = true;
                            for (int bl = 0; bl < lstrPriceItenary.Count; bl++)
                            {
                                if (lstrPriceItenary[bl].AllowBlockPNR.ToString().Trim().ToUpper() != "TRUE")
                                {
                                    hdn_allowblockpnr = false;
                                }
                                if (lstrPriceItenary[bl].ALLOW_RQ != null && lstrPriceItenary[bl].ALLOW_RQ.ToString().ToUpper().Trim() == "Q" && PriceIti.ItinearyDetails[bl].Stock == "OSC")
                                {
                                    Session.Add("BOOKREQUESTONLY", "Y");
                                }
                            }

                            if (!string.IsNullOrEmpty(strDispInfoMessage.ToString().Trim()))
                                ViewBag.GSTpopupmsg = strDispInfoMessage;

                            if (dtSelectResponse != null && dtSelectResponse.Rows.Count != 0)
                                getAllowLCCBlockPNR = dtSelectResponse.Columns.Contains("AllowBlockPNR") ? (hdn_allowblockpnr.ToString().Trim().ToUpper() == "TRUE" ? true : false) : false;
                            #endregion

                            Session.Add("sSelectDataforBooking" + ValKey, _availRes);
                            Session.Add("sStrAirlinsecatagory" + ValKey, strAirlinsecatagory);

                            RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                            bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                            for (var itin = 0; itin < ListItenar.Count; itin++)
                            {
                                for (var N = 0; ListItenar[itin].FlightDetails.Count > N; N++)
                                {
                                    if ((ListItenar[itin].FlightDetails[N].FlightNumber.ToString() != lstrPriceItenary[itin].PriceRS[0].Flights[N].FlightNumber.ToString()))
                                    {
                                        chkMsg += "Your Flight has been changed from : " + ListItenar[itin].FlightDetails[N].FlightNumber.ToString() + " to : " + lstrPriceItenary[itin].PriceRS[0].Flights[N].FlightNumber.ToString() + " for " + ListItenar[itin].FlightDetails[N].Origin.ToString() + "-" + ListItenar[itin].FlightDetails[N].Destination.ToString() + "<br/>";
                                    }
                                    if ((ListItenar[itin].FlightDetails[N].DepartureDateTime.ToString() != lstrPriceItenary[itin].PriceRS[0].Flights[N].DepartureDateTime.ToString()))
                                    {
                                        chkMsg += "Departure date and time has been revised from : " + ListItenar[itin].FlightDetails[N].DepartureDateTime.ToString() + " to : " + lstrPriceItenary[itin].PriceRS[0].Flights[N].DepartureDateTime.ToString() + " for " + ListItenar[itin].FlightDetails[N].Origin.ToString() + "-" + ListItenar[itin].FlightDetails[N].Destination.ToString() + "<br/>";
                                    }
                                    if ((ListItenar[itin].FlightDetails[N].ArrivalDateTime.ToString() != lstrPriceItenary[itin].PriceRS[0].Flights[N].ArrivalDateTime.ToString()))
                                    {
                                        chkMsg += "Arrival date and time has been revised from : " + ListItenar[itin].FlightDetails[N].ArrivalDateTime.ToString() + " to : " + lstrPriceItenary[itin].PriceRS[0].Flights[N].ArrivalDateTime.ToString() + " for " + ListItenar[itin].FlightDetails[N].Origin.ToString() + "-" + ListItenar[itin].FlightDetails[N].Destination.ToString() + "<br/>";
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(chkMsg))
                            {
                                ViewBag.ChkMsg = chkMsg;
                                DatabaseLog.LogData(strUserName, "E", "FlightsController.cs", "Flights_Do_Work", chkMsg, strPosID, strPosTID, sequnceID);
                            }

                            TokenBooking = lstrPriceItenary[0].Token;
                            _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                            _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                            _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                            _availRes.AllowBlockPNR = hdn_allowblockpnr.ToString();// lstrPriceItenary[0].AllowBlockPNR;
                            _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;

                            if (Multireqst == "Y")
                            {
                                Session.Add("token" + reqcnt + ValKey, TokenBooking); //Added by saranraj for Domestic Multicity...
                            }

                            if ((PriceIti.ItinearyDetails != null) && (PriceIti.ItinearyDetails.Count > 0) && lstrPriceItenary[0].PriceRS != null &&
                                                                (lstrPriceItenary[0].PriceRS.Count > 0) &&
                                (flightcounts == lstrPriceItenary[0].PriceRS[0].Flights.Count() ||
                               lstrPriceItenary.Count > 1))//PriceIti.ItinearyDetails[0].FlightDetails.Count()
                            {

                                ViewBag.Bookticketoption = Session["ticketing"] != null ? Session["ticketing"].ToString().Trim() : "N";

                                if (lstrPriceItenary.Count > 1)
                                {
                                    List<RQRS.Flights> FlightDetail = new List<RQRS.Flights>();
                                    List<RQRS.Flights> FlightDetailfin = new List<RQRS.Flights>();
                                    List<List<RQRS.Flights>> Lstlstflgtdetails = new List<List<RQRS.Flights>>();

                                    for (int indx = 0; indx < lstrPriceItenary.Count; indx++)
                                    {
                                        FlightDetail = new List<RQRS.Flights>();
                                        FlightDetail = lstrPriceItenary[indx].PriceRS[0].Flights;

                                        for (int myindx = 0; myindx < FlightDetail.Count(); myindx++)
                                        {
                                            RQRS.Flights flightdet = new RQRS.Flights();
                                            flightdet = FlightDetail[myindx];
                                            FlightDetailfin.Add(flightdet);
                                        }
                                        if (indx == 0)
                                        {
                                            Lstlstflgtdetails = FlightDetail.GroupBy(v => v.ItinRef).Select(j => j.ToList()).ToList();
                                        }
                                        else
                                        {
                                            Lstlstflgtdetails.AddRange(FlightDetail.GroupBy(v => v.ItinRef).Select(j => j.ToList()).ToList());
                                        }
                                    }
                                    ViewBag.FltDets = JsonConvert.SerializeObject(FlightDetailfin);
                                    //ViewBag.LstofLstFltDetails = JsonConvert.SerializeObject(FlightDetailfin.GroupBy(v => v.ItinRef).Select(j => j.ToList()).ToList());
                                    ViewBag.LstofLstFltDetails = JsonConvert.SerializeObject(Lstlstflgtdetails);
                                }
                                else
                                {
                                    ViewBag.FltDets = JsonConvert.SerializeObject(lstrPriceItenary[0].PriceRS[0].Flights);
                                    ViewBag.LstofLstFltDetails = JsonConvert.SerializeObject(lstrPriceItenary[0].PriceRS[0].Flights.GroupBy(v => v.ItinRef).Select(j => j.ToList()).ToList());
                                }
                                string str_Allow_Block_Pnr = (_availRes.AllowBlockPNR != null && _availRes.AllowBlockPNR != "" && _availRes.AllowBlockPNR.ToString().ToUpper() == "TRUE" ? "Y" : "N");

                                ViewBag.Blockticketoption = Session["block"] != null && Session["block"].ToString().Trim() == "Y" && str_Allow_Block_Pnr == "Y" ? "Y" : "N";
                                ViewBag.hdnbestbuyallow = _availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[3] : "N";  //added by Rajesh
                                ViewBag.hdnbargainflag = _availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[2] : "N";  //added by Rajesh
                                ViewBag.hdnallowtune = _availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[4] : "N";  //added by Rajesh
                                ViewBag.hdnDObMand = _availRes.DOBMandatory != null && _availRes.DOBMandatory != "" ? _availRes.DOBMandatory.ToString().ToUpper() : "TRUE";
                                ViewBag.hdnPassmand = _availRes.PassportMandatory != null && _availRes.PassportMandatory != "" ? _availRes.PassportMandatory.ToString().ToUpper() : "TRUE";

                                ViewBag.shoqcommitons = Session["commission"] != null ? Session["commission"].ToString().Trim() : "N";

                                string strErrtemp = "";
                                string GrossAmnt = "";
                                string Sf_tax = "";
                                string Sf_GST = "";
                                string Service_fee = "";
                                string MarkUp = "";
                                string tot_sercharg = "";

                                string[] GAmnt;

                                #region log

                                StringWriter strresWriter = new StringWriter();
                                DataSet dsresponse1 = new DataSet();
                                dsresponse1 = Serv.convertJsonStringToDataSet(strResponse, "");
                                dsresponse1.WriteXml(strresWriter);

                                string lstrtime = "SelectResponse" + DateTime.Now;

                                string lstrCata = Trip + " ~SRS~" +
                               PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;

                                string LstrRespDetails = "<SELECT_RESPONSE><REQTIME>" + ReqTime + "</REQTIME><RESPONSETIME>" + lstrtime + "</RESPONSETIME>"
                                     + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                               "<Flights>" + (_availResponse == null || _availResponse.Flights == null ? "NULL" : _availResponse.Flights.Count.ToString()) + "</Flights>" +
                               "<Fares>" + (_availResponse == null || _availResponse.Fares == null ? "NULL" : _availResponse.Fares.Count.ToString()) + "</Fares>" +
                               "<Bagg>" + (_availResponse == null || _availResponse.Bagg == null ? "NULL" : _availResponse.Bagg.Count.ToString()) + "</Bagg>" +
                               "<Meal>" + (_availResponse == null || _availResponse.Meal == null ? "NULL" : _availResponse.Meal.Count.ToString()) + "</Meal>" +
                               "<DOBMandatory>" + (_availRes == null || _availRes.DOBMandatory == null ? "NULL" : _availRes.DOBMandatory.ToString()) + "</DOBMandatory>" +
                               "<PassportMandatory>" + (_availRes == null || _availRes.PassportMandatory == null ? "NULL" : _availRes.PassportMandatory.ToString()) + "</PassportMandatory>"
                                    + strresWriter.ToString() + "</SELECT_RESPONSE>";

                                DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE SUCCESS " + TokenBooking, LstrRespDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                # endregion


                                Session.Add("BestBuy" + ValKey, bestbuy.ToString());
                                Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                                Session.Add("Requestmarkup" + ValKey, (oldfaremarkup.ToString() != "" && oldfaremarkup.ToString() != null ? oldfaremarkup : "0"));

                                if (bestbuy == true)
                                {
                                    ViewBag.privtfareoldgros = (string.IsNullOrEmpty((_availResponse.Fares[0].Faredescription[0].OldFare).ToString()) ? "0" : _availResponse.Fares[0].Faredescription[0].OldFare);
                                }

                                dtBaggageSelect = new DataTable();
                                dtmealseSelect = new DataTable();
                                dtOtherSsrsel = new DataTable();
                                dtBagout = new DataTable();
                                Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse, ref strErrtemp,
                                  ref dtBaggageSelect, ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtBagout);

                                #region allow GST
                                string strAllowGST = string.Empty;
                                string[] strarrAllowGST = (from _PaxAddl in _availRes.PriceItenarys.AsEnumerable()
                                               select (string.IsNullOrEmpty(_PaxAddl.Allowgst) ? "FALSE" : _PaxAddl.Allowgst.ToString().ToUpper())).Distinct().ToArray();
                                strAllowGST = strarrAllowGST.Contains("TRUE") ? "TRUE" : "FALSE";
                                ViewBag.AllowGST = ViewBag.status == "04" ? "TRUE" : strAllowGST;
                                #endregion
                                #region Passenger Additional info Mandatory
                                string[] strPaxAddlMand = new string[] { };
                                strPaxAddlMand = (from _PaxAddl in _availRes.PriceItenarys.AsEnumerable()
                                                  select (_PaxAddl.UAE_SEGMENT != null ? _PaxAddl.UAE_SEGMENT.ToString().ToUpper() : "FALSE")).Distinct().ToArray();
                                ViewBag.NeedPaxAddlInfo = strPaxAddlMand.Contains("TRUE") ? "Y" : "N";
                                #endregion
                                // ViewBag.BOOKING_rEQUEST = dtBookreq;

                                if (Trip == "M" && SegmentType == "D")
                                {
                                    Session.Add("dtBookreq_table" + ValKey, dtBookreq);
                                }
                                else
                                {
                                    Session.Add("dtBookreq_table" + ValKey, dtBookreq);
                                }

                                if (dtSelectResponse.Rows.Count > 0)
                                {
                                    if (lstrPriceItenary.Count() == 1)
                                    {
                                        var temp = (from p in dtSelectResponse.AsEnumerable()
                                                    group p by p["SegRef"].ToString() into g
                                                    select g.FirstOrDefault()).Distinct();

                                        if (temp.Count() > 0)
                                        {
                                            dtSelectResponse = new DataTable();
                                            dtSelectResponse = temp.CopyToDataTable();
                                        }
                                    }
                                }
                                //End...
                                string Fareandclasschange = string.Empty;
                                if (!string.IsNullOrEmpty(strErrtemp))
                                {
                                    DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", "GRID SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                    Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                                    array_Response[ResultCode] = 0;
                                    ViewBag.resultcode = "0";
                                    array_Response[ErrorMsg] = "Unable to select flight";
                                    ViewBag.ErrorMsg = "Unable to select flight-(#03)";
                                }

                                if (dtSelectResponse != null
                                              && dtSelectResponse.Columns.Count > 1
                                              && dtSelectResponse.Rows.Count > 0)
                                {

                                    GrossAmnt = dtSelectResponse.Rows[0]["GrossAmount"].ToString();
                                    Sf_tax = dtSelectResponse.Rows[0]["Sftax"].ToString();
                                    Sf_GST = dtSelectResponse.Rows[0]["SFGST"].ToString();
                                    Service_fee = dtSelectResponse.Rows[0]["Servicefee"].ToString();
                                    MarkUp = dtSelectResponse.Rows[0]["MarkUp"].ToString();
                                    tot_sercharg = dtSelectResponse.Rows[0]["Servicecharge"].ToString();
                                    if (dtSelectResponse.Rows[0]["GrossAmount"].ToString().Contains("|"))
                                    {
                                        GAmnt = GrossAmnt.Split('|');
                                        GrossAmnt = GAmnt[0];
                                    }

                                    Sf_tax = Sf_tax.Contains("|") ? Sf_tax.Split('|')[0] : Sf_tax;
                                    Sf_GST = Sf_GST.Contains("|") ? Sf_GST.Split('|')[0] : Sf_GST;
                                    Service_fee = Service_fee.Contains("|") ? Service_fee.Split('|')[0] : Service_fee;
                                    MarkUp = MarkUp.Contains("|") ? MarkUp.Split('|')[0] : MarkUp;
                                    tot_sercharg = tot_sercharg.Contains("|") ? tot_sercharg.Split('|')[0] : tot_sercharg;
                                    decimal Tot_grs = Base.ServiceUtility.ConvertToDecimal(GrossAmnt) + Base.ServiceUtility.ConvertToDecimal(Sf_tax) + Base.ServiceUtility.ConvertToDecimal(Sf_GST) + Base.ServiceUtility.ConvertToDecimal(Service_fee) + Base.ServiceUtility.ConvertToDecimal(MarkUp) + Base.ServiceUtility.ConvertToDecimal(tot_sercharg);


                                    if (bestbuy == false)
                                    {

                                        decimal checkdiff = (Base.ServiceUtility.ConvertToDecimal(Tot_grs.ToString()) - Base.ServiceUtility.ConvertToDecimal(Checkgrossamt.ToString()));
                                        decimal limitroundvalue = (checkdiff < 0 ? (-1 * checkdiff) : checkdiff);
                                        decimal checklimit = Base.ServiceUtility.ConvertToDecimal(ConfigurationManager.AppSettings["Farechecklimit"].ToString());

                                        if ((Convert.ToDouble(Tot_grs) == Convert.ToDouble(Checkgrossamt.ToString()) || (checklimit >= limitroundvalue)) && _availRes.ResultCode == "1")//changed by udhaya for the process of round trip selecting fare PriceIti.ItinearyDetails[0].GrossAmount.ToString()
                                        {

                                            dtSelectResponse.TableName = "TrackFareDetails";
                                            if (Multireqst == "Y")
                                            {
                                                Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                            }
                                            else
                                            {
                                                Session.Add("Response" + ValKey, dtSelectResponse);
                                            }

                                            Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                            Session.Add("BaseDest" + ValKey, BaseDestination);
                                            Session.Add("TripType" + ValKey, Trip);
                                            Session.Add("Specialflagfare" + ValKey, Specialflagfare);
                                            Session.Add("TokenBooking" + ValKey, TokenBooking);
                                            Session.Add("Deaprt" + ValKey, Deaprt);
                                            Session.Add("Arrive" + ValKey, Arrive);
                                            Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                            Session.Add("RetDate" + ValKey, ARRDATE);
                                            Session.Add("Stock" + ValKey, StkType);
                                            Session.Add("segmclass" + ValKey, Class);
                                            Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                            Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                            Session.Add("dtservicemeal", dtmealseSelect);
                                            ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);

                                            if (Nfarecheck == "FALSE" && _availResponse.FareCheck.CheckFlag == "0" && _availRes.PriceItenarys[0].Allow_Fare_Change_Popup == "Y")
                                            {
                                                string Currency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                                                Returnval = "Fare has been revised from : " + Currency + (Base.ServiceUtility.ConvertToDecimal(PriceIti.ItinearyDetails[0].GrossAmount.ToString())
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                                + " to : " + Currency +
                                                (Base.ServiceUtility.ConvertToDecimal(SplitFareByAdultPax(dtSelectResponse.Rows[0]["GROSSAMOUNT"].ToString()).ToString())
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString()
                                               + Environment.NewLine + " Do you want to continue the flight selection");

                                                Fareandclasschange = Returnval;
                                                array_Response[ResultCode] = 2;
                                                ViewBag.resultcode = "2";
                                                ViewBag.status = "02";
                                                array_Response[ErrorMsg] = Returnval;
                                                //array_Response[ErrorMsg1] = Returnval;
                                                ViewBag.ErrorMsg = Returnval;
                                                DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", "SELECT RESPONSE-Fare Change", Returnval.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                            }
                                            else if (_availRes.PriceItenarys[0].Error != null && _availRes.PriceItenarys[0].Error != "")
                                            {
                                                array_Response[ResultCode] = 2;
                                                ViewBag.resultcode = "2";
                                                ViewBag.status = "02";
                                                array_Response[ErrorMsg] = _availRes.PriceItenarys[0].Error;
                                                //array_Response[ErrorMsg1] = Returnval;
                                                ViewBag.ErrorMsg = _availRes.PriceItenarys[0].Error;
                                            }
                                            else
                                            {
                                                array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                                ViewBag.resultcode = _availRes.ResultCode.ToString();
                                                ViewBag.status = _availRes.ResultCode.ToString();
                                                array_Response[ErrorMsg] = _availRes.Error;
                                                ViewBag.ErrorMsg = _availRes.Error + "-(#01)";
                                            }
                                        }
                                        else
                                        {
                                            dtSelectResponse.TableName = "TrackFareDetails";
                                            if (Multireqst == "Y")
                                            {
                                                Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                            }
                                            else
                                            {
                                                Session.Add("Response" + ValKey, dtSelectResponse);
                                            }
                                            //string TCnt = strAdults + "," + strChildrens + "," + strInfants;
                                            //Session.Add("TravellerCount" + ValKey, TCnt);
                                            Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                            Session.Add("BaseDest" + ValKey, BaseDestination);
                                            Session.Add("TripType" + ValKey, Trip);
                                            Session.Add("Specialflagfare" + ValKey, Specialflagfare);
                                            Session.Add("TokenBooking" + ValKey, TokenBooking);
                                            Session.Add("Deaprt" + ValKey, Deaprt);
                                            Session.Add("Arrive" + ValKey, Arrive);
                                            Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                            Session.Add("RetDate" + ValKey, ARRDATE);
                                            Session.Add("segmclass" + ValKey, Class);
                                            Session.Add("Stock" + ValKey, StkType);
                                            Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                            Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                            Session.Add("dtservicemeal", dtmealseSelect);

                                            ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);

                                            var linq_Grss = (from _lsiti in PriceIti.ItinearyDetails.AsEnumerable()
                                                             group _lsiti by _lsiti.GrossAmount
                                                                 into Grsfare
                                                             select new
                                                             {
                                                                 GrsAmount = Base.ServiceUtility.ConvertToDecimal(Grsfare.Sum(A => Convert.ToDecimal(A.GrossAmount)).ToString())
                                                             }).ToList().Sum(A => A.GrsAmount);

                                            if (Convert.ToDouble(Tot_grs) != Convert.ToDouble(Checkgrossamt.ToString()) && _availRes.PriceItenarys[0].Allow_Fare_Change_Popup == "Y")
                                            {
                                                string Currency = Session["App_currency"] != null && Session["App_currency"].ToString() != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                                                Returnval = "Fare has been revised from :" + Currency + " " + Checkgrossamt
                                                    + " to :"
                                                    + Currency + " "
                                                    + (Base.ServiceUtility.ConvertToDecimal(SplitFareByAdultPax(dtSelectResponse.Rows[0]["GROSSAMOUNT"].ToString()).ToString())
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString()
                                                    + Environment.NewLine + " Do you want to continue the flight selection";
                                                ViewBag.ErrorMsg = Returnval;
                                                Fareandclasschange = Returnval;
                                                Erroralert += (Erroralert != "" ? Environment.NewLine + Returnval : Returnval);
                                                DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", "SELECT RESPONSE-Fare Change", Erroralert.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                            }

                                            if (_availRes.Error != null && _availRes.Error != "")
                                            {
                                                Returnval = _availRes.Error;
                                                ViewBag.ErrorMsg = Returnval;
                                            }

                                            if (_availRes.PriceItenarys[0].Allow_Fare_Change_Popup == "N" && string.IsNullOrEmpty(Returnval))
                                            {

                                                array_Response[ResultCode] = "1";
                                                ViewBag.resultcode = "1";
                                                ViewBag.status = "01";
                                                array_Response[ErrorMsg] = Returnval;
                                            }
                                            else
                                            {
                                                array_Response[ResultCode] = 2;
                                                ViewBag.resultcode = "2";
                                                ViewBag.status = "02";
                                                array_Response[ErrorMsg] = Returnval;
                                            }

                                            //array_Response[ErrorMsg1] = Returnval;
                                        }
                                    }
                                    else
                                    {

                                        dtSelectResponse.TableName = "TrackFareDetails";
                                        if (Multireqst == "Y")
                                        {
                                            Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                        }
                                        else
                                        {
                                            Session.Add("Response" + ValKey, dtSelectResponse);
                                        }

                                        Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                        Session.Add("BaseDest" + ValKey, BaseDestination);
                                        Session.Add("TripType" + ValKey, Trip);
                                        Session.Add("Specialflagfare" + ValKey, Specialflagfare);
                                        Session.Add("TokenBooking" + ValKey, TokenBooking);
                                        Session.Add("Deaprt" + ValKey, Deaprt);
                                        Session.Add("Arrive" + ValKey, Arrive);
                                        Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                        Session.Add("RetDate" + ValKey, ARRDATE);
                                        Session.Add("Stock" + ValKey, StkType);
                                        Session.Add("segmclass" + ValKey, Class);
                                        Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                        Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                        Session.Add("dtservicemeal", dtmealseSelect);

                                        ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);
                                        array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                        ViewBag.resultcode = _availRes.ResultCode.ToString();
                                        ViewBag.status = _availRes.ResultCode.ToString();
                                        array_Response[ErrorMsg] = _availRes.Error;
                                        ViewBag.ErrorMsg = _availRes.Error + "-(#01)";
                                    }

                                    int IfCount = dtSelectResponse.Rows.Count;
                                    int fCount = 0;

                                    foreach (var Li in PriceIti.ItinearyDetails[0].FlightDetails)
                                    {
                                        if (fCount <= IfCount)
                                        {
                                            if (dtSelectResponse.Rows[fCount]["FareBasisCode"].ToString() != Li.FareBasisCode && ConfigurationManager.AppSettings["Revisedclassalert"].ToString().Contains(StkType) == false && _availRes.PriceItenarys[0].Allow_Fare_Change_Popup == "Y")
                                            {
                                                if (Fareandclasschange != "")
                                                {
                                                    Returnval = "Class has been revised from : " + Li.FareBasisCode + " to : " + dtSelectResponse.Rows[fCount]["FareBasisCode"].ToString() + Environment.NewLine + Fareandclasschange;
                                                }
                                                else
                                                {
                                                    Returnval = "Class has been revised from : " + Li.FareBasisCode + " to : " + dtSelectResponse.Rows[fCount]["FareBasisCode"].ToString() + Environment.NewLine + " Do you want to continue the flight selection";
                                                }
                                                array_Response[ResultCode] = 2;
                                                ViewBag.resultcode = "2";
                                                ViewBag.status = "02";
                                                ViewBag.ErrorMsg = Returnval;
                                                array_Response[ErrorMsg] = Returnval;
                                                DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", "SELECT RESPONSE-Fare Change", Returnval.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                            }
                                            fCount++;
                                        }
                                    }
                                }
                                else
                                {
                                    DatabaseLog.LogData(Session["username"].ToString(), "ER", "InternationalFlights", "SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                    Returnval = "Problem occured while select a flight";
                                    ViewBag.ErrorMsg = "Problem occured while select a flight";
                                    array_Response[ResultCode] = 0;
                                    ViewBag.resultcode = "0";
                                    ViewBag.status = "00";
                                    ViewBag.ErrorMsg = Returnval;
                                    array_Response[ErrorMsg] = Returnval;
                                }

                            }
                            else
                            {
                                string XML = "<EVENT><URL>[<![CDATA[" + strResponse + "]]>]</URL></EVENT>";
                                DatabaseLog.LogData(Session["username"].ToString(), "E", "InternationalFlights", "SELECT RESPONSE Invalid", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                array_Response[ResultCode] = 0;
                                ViewBag.resultcode = "0";
                                ViewBag.status = "00";
                                array_Response[ErrorMsg] = "Selected flight not available";
                                ViewBag.ErrorMsg = "Selected flight not available" + "-(#03)";
                            }
                        }
                        else if (_availRes.ResultCode == "3")
                        {
                            string strErrtemp = "";
                            List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;
                            List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;
                            RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                            bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                            TokenBooking = lstrPriceItenary[0].Token;
                            _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                            _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                            _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                            _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                            _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;

                            #region log


                            StringWriter strresWriter = new StringWriter();
                            DataSet dsresponse = new DataSet();
                            dsresponse = Serv.convertJsonStringToDataSet(strResponse, "");
                            dsresponse.WriteXml(strresWriter);

                            string lstrtime = "SelectResponse" + DateTime.Now;

                            string lstrCata = Trip + " ~SRS~" +
                           PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;

                            string LstrRespDetails = "<SELECT_RESPONSE><REQTIME>" + ReqTime + "</REQTIME><RESPONSETIME>" + lstrtime + "</RESPONSETIME>"
                                 + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                           "<Flights>" + (_availResponse == null || _availResponse.Flights == null ? "NULL" : _availResponse.Flights.Count.ToString()) + "</Flights>" +
                           "<Fares>" + (_availResponse == null || _availResponse.Fares == null ? "NULL" : _availResponse.Fares.Count.ToString()) + "</Fares>" +
                           "<Bagg>" + (_availResponse == null || _availResponse.Bagg == null ? "NULL" : _availResponse.Bagg.Count.ToString()) + "</Bagg>" +
                           "<Meal>" + (_availResponse == null || _availResponse.Meal == null ? "NULL" : _availResponse.Meal.Count.ToString()) + "</Meal>" +
                           "<DOBMandatory>" + (_availRes == null || _availRes.DOBMandatory == null ? "NULL" : _availRes.DOBMandatory.ToString()) + "</DOBMandatory>" +
                           "<PassportMandatory>" + (_availRes == null || _availRes.PassportMandatory == null ? "NULL" : _availRes.PassportMandatory.ToString()) + "</PassportMandatory>"
                                + strresWriter.ToString() + "</SELECT_RESPONSE>";

                            DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE SUCCESS " + TokenBooking, LstrRespDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                            #endregion

                            Session.Add("BestBuy" + ValKey, bestbuy.ToString());
                            Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                            Session.Add("Requestmarkup" + ValKey, (oldfaremarkup.ToString() != "" && oldfaremarkup.ToString() != null ? oldfaremarkup : "0"));

                            dtBaggageSelect = new DataTable();
                            dtmealseSelect = new DataTable();
                            dtOtherSsrsel = new DataTable();
                            dtBagout = new DataTable();
                            Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse, ref strErrtemp,
                                ref dtBaggageSelect, ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtBagout);

                            if (!string.IsNullOrEmpty(strErrtemp))
                            {

                                DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", "GRID SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                                array_Response[ResultCode] = 0;
                                ViewBag.resultcode = "0";
                                array_Response[ErrorMsg] = "Unable to select flight-(#03).";
                                ViewBag.ErrorMsg = "Unable to select flight" + "-(#03).";
                                ViewBag.status = "00";
                            }
                            if (dtSelectResponse != null
                                       && dtSelectResponse.Columns.Count > 1
                                       && dtSelectResponse.Rows.Count > 0)
                            {

                                dtSelectResponse.TableName = "TrackFareDetails";
                                if (Multireqst == "Y")
                                {
                                    Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                }
                                else
                                {
                                    Session.Add("Response" + ValKey, dtSelectResponse);
                                }

                                ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);

                                Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                Session.Add("BaseDest" + ValKey, BaseDestination);
                                Session.Add("TripType" + ValKey, Trip);
                                Session.Add("Specialflagfare" + ValKey, Specialflagfare);
                                Session.Add("TokenBooking" + ValKey, TokenBooking);
                                Session.Add("Deaprt" + ValKey, Deaprt);
                                Session.Add("Arrive" + ValKey, Arrive);
                                Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                Session.Add("RetDate" + ValKey, ARRDATE);
                                Session.Add("Stock" + ValKey, StkType);
                                Session.Add("segmclass" + ValKey, Class);
                                Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                Session.Add("dtservicemeal", dtmealseSelect);

                                array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                ViewBag.resultcode = _availRes.ResultCode.ToString();
                                ViewBag.status = _availRes.ResultCode.ToString();
                                array_Response[ErrorMsg] = _availRes.Error;
                                ViewBag.ErrorMsg = _availRes.Error;
                            }
                            else
                            {
                                DatabaseLog.LogData(Session["username"].ToString(), "ER", "InternationalFlights", "SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                Returnval = "Problem occured while select a flight";
                                array_Response[ResultCode] = 0;
                                ViewBag.resultcode = "0";
                                ViewBag.status = "00";
                                ViewBag.ErrorMsg = Returnval;
                                array_Response[ErrorMsg] = Returnval;
                            }
                        }
                        else
                        {
                            Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                            array_Response[ResultCode] = 0;
                            ViewBag.resultcode = "0";
                            array_Response[ErrorMsg] = Returnval;
                            ViewBag.ErrorMsg = Returnval + "-(#01)";
                            string lstrCata = Trip + " ~SRN~" +
                            PriceIti.Stock.ToString()
                               + "~" + BaseOrgin + "~" + BaseDestination;
                            DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE NULL", Returnval, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                            ViewBag.status = "00";
                        }

                        ViewBag.Totslpaxcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString() : "";
                        ViewBag.Adultcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString().Split(',')[0] : "";
                        ViewBag.Childcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString().Split(',')[1] : "0";
                        ViewBag.Infantcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString().Split(',')[2] : "0";

                        List<DataRow> _listonewaydata = new List<DataRow>();
                        List<DataRow> _listroundtripdata = new List<DataRow>();
                        List<int> _listTotaldata = new List<int>();

                        //_listTotaldata.Add(_listonewaydata);

                        ViewBag.Trips = Trip;

                        ViewBag.Sector = "";
                        ViewBag.baggage = "";
                        ViewBag.Fre_flyr_no = "";
                        ViewBag.seatmappopup = "";
                        ViewBag.Insurancedata = "";

                        ViewBag.Meals = "";
                        ViewBag.BaggageSSR = "";
                        ViewBag.FFN = "";
                        ViewBag.OtherSSR = "";

                        if (dtSelectResponse != null && dtSelectResponse.Rows.Count > 0)
                        {
                            loadsessionvalues(dtSelectResponse, Convert.ToInt32(strAdults),
                                Convert.ToInt32((strChildrens != null && strChildrens != "" ? strChildrens.Trim() : "0")),
                                Convert.ToInt32((strInfants != null && strInfants != "" ? strInfants.Trim() : "0")), ValKey);
                            ArrayList SSRRetArray = Bll.Create_Meals_Bagg(dtSelectResponse, ValKey, dtOtherSsrsel, dtBagout, strAdults, strChildrens);

                            ViewBag.Sector = SSRRetArray[0].ToString();
                            ViewBag.baggage = SSRRetArray[2].ToString();
                            ViewBag.Fre_flyr_no = SSRRetArray[6].ToString();
                            ViewBag.frqno = SSRRetArray[5].ToString();
                            ViewBag.seatmappopup = SSRRetArray[8].ToString();
                            ViewBag.Insurancedata = SSRRetArray[11].ToString();
                            ViewBag.Otherssrdata = SSRRetArray[15].ToString();
                            ViewBag.Otherssrspicemax = SSRRetArray[17].ToString();
                            ViewBag.Otherssrprcheckinselect = SSRRetArray[19].ToString();
                            ViewBag.Otherssrbagoutselect = SSRRetArray[21].ToString();
                            ViewBag.Meals = SSRRetArray[22].ToString();
                            ViewBag.BaggageSSR = SSRRetArray[23].ToString();
                            ViewBag.FFN = SSRRetArray[24].ToString();
                            ViewBag.OtherSSR = SSRRetArray[25].ToString();

                            Session.Add("SSR", SSRRetArray[4].ToString());
                            Session.Add("FRQ", SSRRetArray[7].ToString());

                        }

                        string td_Adult, td_child, td_infant = string.Empty;

                        td_Adult = ViewBag.Adultcount;
                        td_child = ViewBag.Childcount;
                        td_infant = ViewBag.Infantcount;

                        Session.Add("PaxCount" + ValKey, td_Adult + "|" + td_child + "|" + td_infant);

                        ViewBag.ValKey = ValKey;
                        ViewBag.ServerDateTime = Base.LoadServerdatetime();
                        string isbookletfare = Session["ISBOOKLETFARE"] != null && Session["ISBOOKLETFARE"].ToString() != "" ? Session["ISBOOKLETFARE"].ToString().ToUpper() : "";
                        if (isbookletfare == "TRUE")
                        {
                            Session.Add("domseg", "D");
                        }
                    }
                }
                catch (Exception ex)
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "InternationalFlights", "SELECT REQUEST", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    Returnval = "Problem Occured. Please contact customercare";
                    array_Response[ResultCode] = 0;
                    ViewBag.resultcode = "0";
                    array_Response[ErrorMsg] = Returnval;
                    ViewBag.ErrorMsg = Returnval + "-(#05)";

                    ViewBag.status = "00";
                }
                #endregion
            }
            catch (Exception ex)
            {
                ViewBag.status = "00";
                Returnval = ex.Message;
                DatabaseLog.LogData(Session["username"].ToString(), "X", "InternationalFlights", "SELECT REQUEST", ex.ToString() + ex.StackTrace.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                array_Response[ResultCode] = 0;
                ViewBag.resultcode = "0";
                array_Response[ErrorMsg] = Returnval;
                ViewBag.ErrorMsg = Returnval + "-(#05)";
            }

            ViewBag.Multireqst = "N";

            if (Multireqst == "Y")
            {

                return Json(new
                {
                    status = ViewBag.status,
                    Message = ViewBag.ErrorMsg,
                    Result = ViewBag.Response,
                    adtcount = ViewBag.Adultcount,
                    chdcount = ViewBag.Childcount,
                    infcount = ViewBag.Infantcount,
                    shoqcommitons = ViewBag.shoqcommitons,
                    hdnDObMand = ViewBag.hdnDObMand,
                    hdnPassmand = ViewBag.hdnPassmand,
                    Totslpaxcount = ViewBag.Totslpaxcount,
                    ValKey = ViewBag.ValKey,
                    resultcode = ViewBag.resultcode,
                    token = TokenBooking
                });
            }
            else
            {
                if (ConfigurationManager.AppSettings["APP_HOSTING"] == "BSA" || ConfigurationManager.AppSettings["APP_HOSTING"] == "B2B"
                            || ConfigurationManager.AppSettings["APP_HOSTING"] == "BOA")
                {
                    return PartialView("_AvailSelect_BSA", "");
                }
                else
                {
                    return PartialView("_AvailSelect", "");
                }
            }
        }


        public ActionResult Bestbuyfetch(string oldgrossamount, string mealamount, string BaggageAmnt, string seatamount, string Li_Totalfare)
        {
            DataTable dtSelectResponse = new DataTable();
            DataTable dtSelectResponsetmp = new DataTable();
            DataTable dtBaggageSelect = new DataTable();
            DataTable dtmealseSelect = new DataTable();
            DataTable dtOtherSsrsel = new DataTable();
            DataTable dtBookreq = new DataTable();
            DataTable dtbuggout = new DataTable();
            ArrayList array_Response = new ArrayList();
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            Base.ServiceUtility Serv = new Base.ServiceUtility();
            RQRS.PriceItenaryRS _availRes = new RQRS.PriceItenaryRS();


            int ResultCode = 0;
            int ErrorMsg = 1;
            int grossfare = 2;
            //int TokenBB = 3;
            string reqeust = string.Empty;
            string Returnval = "";
            try
            {
                ViewBag.bboldgross = oldgrossamount;
                ViewBag.bboldmeal = mealamount;

                ViewBag.bboldbagg = BaggageAmnt;
                ViewBag.bboldseat = seatamount;
                ViewBag.bboldtotal = Li_Totalfare;

                reqeust = Session["sel_Request"].ToString();
                string[] FlightRequest = new string[] { };
                if (reqeust.Contains("SplItRaJeshTrav"))
                {
                    FlightRequest = Regex.Split(reqeust, "SplItRaJeshTrav");

                }

                string FliNum = FlightRequest[0].ToString();
                string Deaprt = FlightRequest[1].ToString();
                string Arrive = FlightRequest[2].ToString();
                string FullFlag = FlightRequest[3].ToString();
                string TokenKey = FlightRequest[4].ToString();
                string Trip = FlightRequest[5].ToString();
                string BaseOrgin = FlightRequest[6].ToString();
                string BaseDestination = FlightRequest[7].ToString();
                string offflg = FlightRequest[8].ToString();
                string Class = FlightRequest[9].ToString();
                string TKey = FlightRequest[10].ToString();
                string DEPTDATE = FlightRequest[11].ToString();
                string ARRDATE = FlightRequest[12].ToString();
                string Nfarecheck = FlightRequest[13].ToString();
                string oldfaremarkup = FlightRequest[14].ToString();
                string mobile = FlightRequest[15].ToString();
                string bestbuyrequired = FlightRequest[16].ToString();
                string AlterQueue = FlightRequest[17].ToString();
                string SegmentType = FlightRequest[18].ToString();
                string Multireqst = FlightRequest[19].ToString();
                string reqcnt = FlightRequest[20].ToString();
                string RtripComparFlg = FlightRequest[21].ToString();
                string ValKey = FlightRequest[22].ToString();

                #region Form Booking Request .......
                string Origin = "";
                string Destination = "";
                string depttime = string.Empty;
                string ArrTime = string.Empty;
                string CNX = string.Empty;
                string cabin, FareID = string.Empty;
                string FareCode = string.Empty;
                string SClass = string.Empty;
                string AirlineCategory = string.Empty;
                string OFFLINEFLAG = offflg;
                string strAdults = "";
                string strChildrens = "";
                string strInfants = "";
                string PlatingCarrier = "";
                string RBDCode = "";
                //string TransactionFlag = "";
                string ConnectionFlag = "";
                string BaseAmount = "";
                string GrossAmount = "";
                string StkType = "";

                RQRS.PriceItineary PriceIti = new RQRS.PriceItineary();
                List<RQRS.Itineraries> ListItenar = new List<RQRS.Itineraries>();
                List<RQRS.Flights> FlightsDet = new List<RQRS.Flights>();
                RQRS.Itineraries itinflights = new RQRS.Itineraries();
                RQRS.SegmentDetails SegDet = new RQRS.SegmentDetails();

                RQRS.Flights Itindet = new RQRS.Flights();

                int flightcounts = 0;

                string[] TerminalDetails = new string[] { };
                string[] ArrFliDet, FlightCount, IFCount;

                IFCount = Regex.Split(FullFlag, "SpLiTWeB");

                ListItenar = new List<RQRS.Itineraries>();

                //string ValKey = "";

                //if (TKey != null && TKey != "")
                //{
                //    if (TKey.Contains('~'))
                //    {
                //        string[] Availed = TKey.Split('~');
                //        ValKey = Availed[Availed.Count() - 2].ToString().Split('_')[1].ToString().Trim();
                //    }
                //    else
                //    {
                //        string[] Availed = TKey.Split('_');
                //        ValKey = Availed[1].ToString().Trim();
                //    }
                //}

                for (int iF = 0; iF < IFCount.Length; iF++)
                {


                    if (IFCount[iF].Contains("SpLITSaTIS"))
                    {
                        FlightCount = Regex.Split(IFCount[iF], "SpLITSaTIS");
                        TerminalDetails = new string[FlightCount.Length];

                        FlightsDet = new List<RQRS.Flights>();

                        for (int fl = 0; fl < FlightCount.Length; fl++)
                        {
                            itinflights = new RQRS.Itineraries();

                            flightcounts++;
                            if (FlightCount[fl].Contains("SpLitPResna"))
                            {
                                ArrFliDet = Regex.Split(FlightCount[fl], "SpLitPResna");
                                if (ArrFliDet[24] != null && ArrFliDet[24] != "")
                                {
                                    Session.Add("terminaldetails", ArrFliDet[24]);
                                }
                                TerminalDetails[fl] = ArrFliDet[24];
                                Origin = ArrFliDet[0];
                                Destination = ArrFliDet[1];
                                depttime = ArrFliDet[2];
                                ArrTime = ArrFliDet[3];
                                SClass = Class;
                                strAdults = ArrFliDet[6];
                                strChildrens = ArrFliDet[7];
                                strInfants = ArrFliDet[8];
                                PlatingCarrier = ArrFliDet[9];
                                RBDCode = ArrFliDet[5];
                                BaseAmount = ArrFliDet[13];
                                GrossAmount = ArrFliDet[14];
                                ConnectionFlag = ArrFliDet[15];
                                AirlineCategory = ArrFliDet[16];
                                FareCode = ArrFliDet[17];
                                cabin = ArrFliDet[21];
                                FareID = ArrFliDet[25];

                                Itindet = new RQRS.Flights();
                                Itindet.AirlineCategory = AirlineCategory;
                                Itindet.ArrivalDateTime = ArrTime;
                                Itindet.CNX = CNX;

                                Itindet.DepartureDateTime = depttime.Trim();
                                Itindet.Destination = Destination;
                                Itindet.FareBasisCode = FareCode.Trim();
                                Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                                Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                                Itindet.PlatingCarrier = PlatingCarrier;
                                Itindet.Class = SClass;//strFareclassOption.ToString().Trim();
                                Itindet.RBDCode = RBDCode;
                                //Itindet.Cabin = cabin;
                                Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                                Itindet.OfflineFlag = Convert.ToInt16(OFFLINEFLAG.Trim().Split('~')[iF]);
                                Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                                Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                                Itindet.ItinRef = Multireqst == "Y" ? iF.ToString() : ArrFliDet[19];
                                Itindet.SegRef = ArrFliDet.Length > 29 ? ArrFliDet[29] : "";
                                Itindet.CNX = RtripComparFlg != "0" ? ArrFliDet[20] : (fl != FlightCount.Length - 1 ? "Y" : "N"); //Roundtrip Spl RQ from Roundtrip Avail... tat's y CNX hardcoded...
                                Itindet.FareId = FareID;
                                StkType = ArrFliDet[23];

                                FlightsDet.Add(Itindet);

                            }
                            itinflights.BaseAmount = BaseAmount;// dtBookFlight.Rows[0]["BasicFare"].ToString();
                            itinflights.GrossAmount = GrossAmount;// dtBookFlight.Rows[0]["GrossFare"].ToString();
                            itinflights.FlightDetails = FlightsDet;
                            itinflights.Stock = StkType;


                        }
                        ListItenar.Add(itinflights);

                    }
                    else
                    {
                        flightcounts++;
                        itinflights = new RQRS.Itineraries();
                        FlightsDet = new List<RQRS.Flights>();
                        TerminalDetails = new string[1];
                        if (IFCount[iF].Contains("SpLitPResna"))
                        {
                            // ArrFliDet = FullFlag.Split('~');
                            ArrFliDet = Regex.Split(IFCount[iF], "SpLitPResna");


                            string[] segmentarr = null;
                            if (ArrFliDet[24] != null && ArrFliDet[24] != "")
                            {
                                string terminaldetail = ArrFliDet[24];
                                segmentarr = terminaldetail.Split('\n');
                            }
                            TerminalDetails[0] = ArrFliDet[24];
                            Origin = ArrFliDet[0];
                            Destination = ArrFliDet[1];
                            depttime = ArrFliDet[2];
                            ArrTime = ArrFliDet[3];
                            SClass = Class;
                            strAdults = ArrFliDet[6];
                            strChildrens = ArrFliDet[7];
                            strInfants = ArrFliDet[8];
                            PlatingCarrier = ArrFliDet[9];
                            RBDCode = ArrFliDet[5];
                            BaseAmount = ArrFliDet[13];
                            GrossAmount = ArrFliDet[14];
                            ConnectionFlag = ArrFliDet[15];
                            AirlineCategory = ArrFliDet[16];
                            FareCode = ArrFliDet[17];
                            FareID = ArrFliDet[25];

                            Itindet = new RQRS.Flights();

                            Itindet.AirlineCategory = AirlineCategory;
                            Itindet.ArrivalDateTime = ArrTime;
                            Itindet.DepartureDateTime = depttime.Trim();
                            Itindet.Destination = Destination;
                            Itindet.FareBasisCode = FareCode.Trim();
                            Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                            Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                            Itindet.PlatingCarrier = PlatingCarrier;
                            Itindet.Class = SClass;//strFareclassOption.ToString().Trim();
                            Itindet.RBDCode = RBDCode;
                            Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                            Itindet.OfflineFlag = Convert.ToInt16(OFFLINEFLAG.Trim().Split('~')[iF]);
                            Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                            Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                            Itindet.ItinRef = Multireqst == "Y" ? iF.ToString() : ArrFliDet[19];
                            Itindet.SegRef = ArrFliDet.Length > 29 ? ArrFliDet[29] : "";
                            Itindet.CNX = ArrFliDet[20];
                            StkType = ArrFliDet[23];
                            Itindet.FareId = FareID;
                            FlightsDet.Add(Itindet);
                        }

                        itinflights.BaseAmount = BaseAmount;// dtBookFlight.Rows[0]["BasicFare"].ToString();
                        itinflights.GrossAmount = GrossAmount;// dtBookFlight.Rows[0]["GrossFare"].ToString();
                        itinflights.FlightDetails = FlightsDet;
                        itinflights.Stock = StkType;

                        // ListItenar.Add(itinflights);
                        ListItenar.Add(itinflights);
                    }

                }

                SegDet.BaseOrigin = BaseOrgin;
                SegDet.BaseDestination = BaseDestination;
                SegDet.Adult = strAdults;
                SegDet.Child = strChildrens != null && strChildrens != "" ? strChildrens.Trim() : "0";
                SegDet.Infant = strInfants != null && strInfants != "" ? strInfants.Trim() : "0";
                SegDet.SegmentType = SegmentType; //Trip=="R"?"D":"I";
                SegDet.ClassType = SClass;
                SegDet.BookingType = "B2B";
                SegDet.TripType = (Trip == "Y" && ConfigurationManager.AppSettings["COUNTRY"] != null && ConfigurationManager.AppSettings["COUNTRY"].ToString() == "AE") ? "R" : Trip;
                SegDet.AppType = "W";
                SegDet.SinglePNR = RtripComparFlg;// RtripComparFlg; //1-SinglePNR for Roundtrip Avail, 0-Roundtrip spl rq for Roundtrip avail (compare popup), "" means else part... by saranraj...


                RQRS.AgentDetails agent = new RQRS.AgentDetails();
                agent.AgentId = Session["Availagentid"].ToString();
                agent.Agenttype = "";
                agent.AirportID = SegmentType;//"D";
                agent.AppType = "B2B";
                agent.BOAID = Session["POS_ID"].ToString();
                agent.BOAterminalID = Session["POS_TID"].ToString();
                agent.BranchID = "";
                agent.ClientID = "";
                agent.CoOrdinatorID = "";
                agent.Environment = "W";
                agent.TerminalId = Session["Availterminal"].ToString();
                agent.UserName = Session["username"].ToString();
                agent.Version = "";




                PriceIti.ItinearyDetails = ListItenar;
                PriceIti.BestBuyOption = bestbuyrequired == "TRUE" ? true : false;
                PriceIti.SegmnetDetails = SegDet;
                PriceIti.AgentDetails = agent;
                PriceIti.Stock = StkType;
                PriceIti.AlterQueue = AlterQueue;

                // ViewBag.FltDets = JsonConvert.SerializeObject(ListItenar);//For showing Flight details in Razor view...
                #endregion

                string Query = "InvokeBestbuy";
                int hostchecktimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);

                string request = JsonConvert.SerializeObject(PriceIti).ToString();

                MyWebClient client = new MyWebClient();
                client.LintTimeout = hostchecktimeout;
                client.Headers["Content-type"] = "application/json";

                #region Log

                string ReqTime = "SelectReqest" + DateTime.Now;

                StringWriter strWriter = new StringWriter();
                DataSet dsrequest = new DataSet();
                dsrequest = Serv.convertJsonStringToDataSet(request, "");
                dsrequest.WriteXml(strWriter);

                //strURLpath = strURLpath.Replace("host", ConfigurationManager.AppSettings["TRAVRAYSTWS_URL"].ToString());
                //strURLpath = strURLpath.Replace("Service", ConfigurationManager.AppSettings["TRAVRAYSTWS_VDIR"].ToString());
                if (ConfigurationManager.AppSettings["Splavailagent"].ToString() != "" && ConfigurationManager.AppSettings["Splavailagent"].ToString().Contains(Session["POS_TID"].ToString()))
                {
                    strURLpath = ConfigurationManager.AppSettings["Spl_APPS_SELECT_URL"].ToString();
                }
                else
                {
                    strURLpath = ConfigurationManager.AppSettings["APPS_SELECT_URL"].ToString();
                }

                string LstrDetails = "<SELECT_REQUEST><URL>[<![CDATA[" + strURLpath
                    + "]]>]</URL><QUERY>" + Query + "</QUERY><REQTIME>" + ReqTime + "</REQTIME><TIMEOUT>" + (hostchecktimeout).ToString() + "</TIMEOUT>" + ((Base.ReqLog) ?
                    strWriter.ToString() : request) + "<JSON>" + request + "</JSON></SELECT_REQUEST>";

                DatabaseLog.LogData(Session["username"].ToString(), "S", "Bestbuy", "Bestbuy Request", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                #endregion

                /****/
                //byte[] data = { };

                byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                string strResponse = System.Text.Encoding.ASCII.GetString(data);


                /****/

                ViewBag.triptype = Trip;
                if (string.IsNullOrEmpty(strResponse))
                {
                    #region log
                    string lstrCata = Trip + " ~SRN~" +
                       PriceIti.Stock.ToString()
                   + "~" + BaseOrgin + "~" + BaseDestination;

                    DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE NULL", "Null Or Empty", Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                    #endregion
                }
                else
                {
                    DataSet dsselect = new DataSet();
                    dsselect = Serv.convertJsonStringToDataSet(strResponse, "");
                    DataTable dsselect1 = new DataTable();
                    dsselect1 = dsselect.Tables[0];
                    Session.Add("Dobmand" + ValKey, dsselect1);
                    Session.Add("valkey", ValKey);
                    _availRes = JsonConvert.DeserializeObject<RQRS.PriceItenaryRS>(strResponse);
                    if (_availRes.ResultCode == "1" || _availRes.ResultCode == "2")
                    {

                        List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;
                        List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;
                        RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                        bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                        string TokenBooking = lstrPriceItenary[0].Token;
                        _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                        _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                        _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                        _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                        _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;

                        if (Multireqst == "Y")
                        {
                            Session.Add("token" + reqcnt + ValKey, TokenBooking); //Added by saranraj for Domestic Multicity...
                        }

                        if ((PriceIti.ItinearyDetails != null) && (PriceIti.ItinearyDetails.Count > 0) && lstrPriceItenary[0].PriceRS != null &&
                                                            (lstrPriceItenary[0].PriceRS.Count > 0) &&
                            (flightcounts == lstrPriceItenary[0].PriceRS[0].Flights.Count()))//PriceIti.ItinearyDetails[0].FlightDetails.Count()
                        {

                            ViewBag.Bookticketoption = Session["ticketing"] != null ? Session["ticketing"].ToString().Trim() : "N";
                            ViewBag.FltDets = JsonConvert.SerializeObject(lstrPriceItenary[0].PriceRS[0].Flights);
                            string str_Allow_Block_Pnr = (_availRes.AllowBlockPNR != null && _availRes.AllowBlockPNR != "" && _availRes.AllowBlockPNR.ToString().ToUpper() == "TRUE" ? "Y" : "N");

                            ViewBag.Blockticketoption = Session["block"] != null && Session["block"].ToString().Trim() == "Y" && str_Allow_Block_Pnr == "Y" ? "Y" : "N";

                            ViewBag.hdnbestbuyallow = "1";//_availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[2] : "N";  //added by Rajesh
                            ViewBag.hdnbargainflag = _availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[2] : "N";   //added by Rajesh
                            ViewBag.hdnallowtune = _availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[4] : "N";  //added by Rajesh

                            ViewBag.hdnDObMand = _availRes.DOBMandatory != null && _availRes.DOBMandatory != "" ? _availRes.DOBMandatory.ToString().ToUpper() : "TRUE";
                            ViewBag.hdnPassmand = _availRes.PassportMandatory != null && _availRes.PassportMandatory != "" ? _availRes.PassportMandatory.ToString().ToUpper() : "TRUE";

                            ViewBag.shoqcommitons = Session["commission"] != null ? Session["commission"].ToString().Trim() : "N";

                            string strErrtemp = "";
                            string GrossAmnt = "";
                            string[] GAmnt;

                            #region log


                            StringWriter strresWriter = new StringWriter();
                            DataSet dsresponse1 = new DataSet();
                            dsresponse1 = Serv.convertJsonStringToDataSet(strResponse, "");
                            dsresponse1.WriteXml(strresWriter);

                            string lstrtime = "SelectResponse" + DateTime.Now;

                            string lstrCata = Trip + " ~SRS~" +
                           PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;

                            string LstrRespDetails = "<SELECT_RESPONSE><REQTIME>" + ReqTime + "</REQTIME><RESPONSETIME>" + lstrtime + "</RESPONSETIME>"
                                 + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                           "<Flights>" + (_availResponse == null || _availResponse.Flights == null ? "NULL" : _availResponse.Flights.Count.ToString()) + "</Flights>" +
                           "<Fares>" + (_availResponse == null || _availResponse.Fares == null ? "NULL" : _availResponse.Fares.Count.ToString()) + "</Fares>" +
                           "<Bagg>" + (_availResponse == null || _availResponse.Bagg == null ? "NULL" : _availResponse.Bagg.Count.ToString()) + "</Bagg>" +
                           "<Meal>" + (_availResponse == null || _availResponse.Meal == null ? "NULL" : _availResponse.Meal.Count.ToString()) + "</Meal>" +
                           "<DOBMandatory>" + (_availRes == null || _availRes.DOBMandatory == null ? "NULL" : _availRes.DOBMandatory.ToString()) + "</DOBMandatory>" +
                           "<PassportMandatory>" + (_availRes == null || _availRes.PassportMandatory == null ? "NULL" : _availRes.PassportMandatory.ToString()) + "</PassportMandatory>"
                                + strresWriter.ToString() + "</SELECT_RESPONSE>";

                            DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE SUCCESS " + TokenBooking, LstrRespDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                            # endregion

                            //   bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;
                            Session.Add("BestBuy" + ValKey, bestbuy.ToString());
                            Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                            Session.Add("Requestmarkup" + ValKey, (oldfaremarkup.ToString() != "" && oldfaremarkup.ToString() != null ? oldfaremarkup : "0"));

                            if (bestbuy == true)
                            {
                                ViewBag.privtfareoldgros = (string.IsNullOrEmpty((_availResponse.Fares[0].Faredescription[0].OldFare).ToString()) ? "0" : _availResponse.Fares[0].Faredescription[0].OldFare);
                            }

                            dtBaggageSelect = new DataTable();
                            dtmealseSelect = new DataTable();
                            dtOtherSsrsel = new DataTable();
                            dtbuggout = new DataTable();
                            Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse, ref strErrtemp,
                                ref dtBaggageSelect, ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtbuggout);

                            //Added by saranraj to avoid duplicate rows generated in LINQ... on 20170715...
                            if (dtSelectResponse.Rows.Count > 0)
                            {
                                var temp = (from p in dtSelectResponse.AsEnumerable()
                                            group p by p["SegRef"].ToString() into g
                                            select g.FirstOrDefault()).Distinct();

                                if (temp.Count() > 0)
                                {
                                    dtSelectResponse = new DataTable();
                                    dtSelectResponse = temp.CopyToDataTable();
                                }
                            }
                            //End...

                            if (!string.IsNullOrEmpty(strErrtemp))
                            {
                                DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", "GRID SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                                array_Response[ResultCode] = 0;
                                ViewBag.resultcode = "0";
                                array_Response[ErrorMsg] = "Unable to select flight";
                                ViewBag.ErrorMsg = "Unable to select flight-(#03)";
                            }

                            if (dtSelectResponse != null && dtSelectResponse.Columns.Count > 1 && dtSelectResponse.Rows.Count > 0)
                            {
                                if (TerminalDetails.Count() == dtSelectResponse.Rows.Count)
                                {
                                    for (int index = 0; index < dtSelectResponse.Rows.Count; index++)
                                    {
                                        dtSelectResponse.Rows[index]["DESTERMINAL"] = TerminalDetails[index].Split('\n').Length > 3 ?
                        (TerminalDetails[index].Split('\n')[3].Split(':').Length > 1 ?
                        TerminalDetails[index].Split('\n')[3].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";

                                        dtSelectResponse.Rows[index]["ORGTERMINAL"] = TerminalDetails[index].Split('\n').Length > 2 ?
                       (TerminalDetails[index].Split('\n')[2].Split(':').Length > 1 ?
                       TerminalDetails[index].Split('\n')[2].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";
                                        dtSelectResponse.AcceptChanges();
                                    }
                                }
                                GrossAmnt = dtSelectResponse.Rows[0]["GrossAmount"].ToString();
                                if (dtSelectResponse.Rows[0]["GrossAmount"].ToString().Contains("|"))
                                {
                                    GAmnt = GrossAmnt.Split('|');
                                    GrossAmnt = GAmnt[0];
                                }

                                if (bestbuy == false)
                                {
                                    var linq_Grs = (from _lsiti in PriceIti.ItinearyDetails.AsEnumerable()
                                                    group _lsiti by _lsiti.GrossAmount
                                                        into Grsfare
                                                    select new
                                                    {
                                                        GrsAmount = (Grsfare.Sum(A => Convert.ToDecimal(A.GrossAmount)))
                                                    }).ToList().Sum(A => A.GrsAmount);

                                    if (_availRes.ResultCode == "1" || _availRes.ResultCode == "2")//changed by udhaya for the process of round trip selecting fare PriceIti.ItinearyDetails[0].GrossAmount.ToString()
                                    {

                                        dtSelectResponse.TableName = "TrackFareDetails";
                                        if (Multireqst == "Y")
                                        {
                                            Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                        }
                                        else
                                        {
                                            Session.Add("Response" + ValKey, dtSelectResponse);
                                        }

                                        Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                        Session.Add("BaseDest" + ValKey, BaseDestination);
                                        Session.Add("TripType" + ValKey, Trip);
                                        Session.Add("Specialflagfare" + ValKey, "");
                                        Session.Add("TokenBooking" + ValKey, TokenBooking);
                                        Session.Add("Deaprt" + ValKey, Deaprt);
                                        Session.Add("Arrive" + ValKey, Arrive);
                                        Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                        Session.Add("RetDate" + ValKey, ARRDATE);
                                        Session.Add("Stock" + ValKey, StkType);
                                        Session.Add("segmclass" + ValKey, Class);
                                        Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                        Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                        ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);
                                        array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                        ViewBag.resultcode = _availRes.ResultCode.ToString();
                                        ViewBag.status = _availRes.ResultCode.ToString();
                                        array_Response[ErrorMsg] = _availRes.Error;
                                        ViewBag.ErrorMsg = _availRes.Error + "-(#01)";
                                        //}
                                    }
                                    else
                                    {
                                        dtSelectResponse.TableName = "TrackFareDetails";
                                        if (Multireqst == "Y")
                                        {
                                            Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                        }
                                        else
                                        {
                                            Session.Add("Response" + ValKey, dtSelectResponse);
                                        }
                                        //string TCnt = strAdults + "," + strChildrens + "," + strInfants;
                                        //Session.Add("TravellerCount" + ValKey, TCnt);
                                        Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                        Session.Add("BaseDest" + ValKey, BaseDestination);
                                        Session.Add("TripType" + ValKey, Trip);
                                        Session.Add("Specialflagfare" + ValKey, "");
                                        Session.Add("TokenBooking" + ValKey, TokenBooking);
                                        Session.Add("Deaprt" + ValKey, Deaprt);
                                        Session.Add("Arrive" + ValKey, Arrive);
                                        Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                        Session.Add("RetDate" + ValKey, ARRDATE);
                                        Session.Add("segmclass" + ValKey, Class);
                                        Session.Add("Stock" + ValKey, StkType);
                                        Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                        Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                        ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);

                                        var linq_Grss = (from _lsiti in PriceIti.ItinearyDetails.AsEnumerable()
                                                         group _lsiti by _lsiti.GrossAmount
                                                             into Grsfare
                                                         select new
                                                         {
                                                             GrsAmount = (Grsfare.Sum(A => Convert.ToDecimal(A.GrossAmount)))
                                                         }).ToList().Sum(A => A.GrsAmount);

                                        //  var linqconcatinate = (linq_Grs.Select(a=>a.GrsAmount ).Sum(b => b.GrossAmount)).ToString();
                                        if (_availRes.Error == null || _availRes.Error == "")
                                        {
                                            string Currency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                                            Returnval = "Fare has been revised from :" + Currency + " "
                                                + (Base.ServiceUtility.ConvertToDecimal(linq_Grss.ToString())
                                                + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                                + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))
                                                + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                                + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                                + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString() + " to :" +
                                               Currency + " "
                                                + (Base.ServiceUtility.ConvertToDecimal(SplitFareByAdultPax(dtSelectResponse.Rows[0]["GROSSAMOUNT"].ToString()).ToString())
                                                + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                                + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))
                                                + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                                + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                                + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString()
                                                + Environment.NewLine + " Do you want to continue the flight selection";
                                        }
                                        else
                                        {
                                            Returnval = _availRes.Error;
                                        }
                                        array_Response[ResultCode] = 2;
                                        ViewBag.resultcode = "2";
                                        ViewBag.status = "02";
                                        array_Response[ErrorMsg] = Returnval;
                                        ViewBag.ErrorMsg = Returnval;
                                    }
                                }
                                else
                                {
                                    dtSelectResponse.TableName = "TrackFareDetails";
                                    if (Multireqst == "Y")
                                    {
                                        Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                    }
                                    else
                                    {
                                        Session.Add("Response" + ValKey, dtSelectResponse);
                                    }

                                    Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                    Session.Add("BaseDest" + ValKey, BaseDestination);
                                    Session.Add("TripType" + ValKey, Trip);
                                    Session.Add("Specialflagfare" + ValKey, "");
                                    Session.Add("TokenBooking" + ValKey, TokenBooking);
                                    Session.Add("Deaprt" + ValKey, Deaprt);
                                    Session.Add("Arrive" + ValKey, Arrive);
                                    Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                    Session.Add("RetDate" + ValKey, ARRDATE);
                                    Session.Add("Stock" + ValKey, StkType);
                                    Session.Add("segmclass" + ValKey, Class);
                                    Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                    Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                    ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);
                                    array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                    ViewBag.resultcode = _availRes.ResultCode.ToString();
                                    ViewBag.status = _availRes.ResultCode.ToString();
                                    array_Response[ErrorMsg] = _availRes.Error;
                                    ViewBag.ErrorMsg = _availRes.Error + "-(#01)";
                                }

                                int IfCount = dtSelectResponse.Rows.Count;
                                int fCount = 0;

                                foreach (var Li in PriceIti.ItinearyDetails[0].FlightDetails)
                                {
                                    if (fCount <= IfCount)
                                    {
                                        if (dtSelectResponse.Rows[fCount]["FareBasisCode"].ToString() != Li.FareBasisCode && ConfigurationManager.AppSettings["Revisedclassalert"].ToString().Contains(StkType) == false)
                                        {
                                            Returnval = "Class has been revised from :" + Li.FareBasisCode + " to :" + dtSelectResponse.Rows[0]["FareBasisCode"].ToString() + Environment.NewLine + " Do you want to continue the flight selection";
                                            array_Response[ResultCode] = 2;
                                            ViewBag.resultcode = "2";
                                            ViewBag.status = "02";
                                            ViewBag.ErrorMsg = Returnval;
                                            array_Response[ErrorMsg] = Returnval;

                                        }
                                        fCount++;
                                    }
                                }
                            }
                            else
                            {
                                DatabaseLog.LogData(Session["username"].ToString(), "ER", "InternationalFlights", "SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                Returnval = "Problem occured while select a flight";
                                ViewBag.ErrorMsg = "Problem occured while select a flight";
                                array_Response[ResultCode] = 0;
                                ViewBag.resultcode = "0";
                                ViewBag.status = "00";
                                ViewBag.ErrorMsg = Returnval;
                                array_Response[ErrorMsg] = Returnval;
                            }
                        }
                        else
                        {
                            string XML = "<EVENT><URL>[<![CDATA[" + strResponse + "]]>]</URL></EVENT>";
                            DatabaseLog.LogData(Session["username"].ToString(), "E", "InternationalFlights", "SELECT RESPONSE Invalid", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                            array_Response[ResultCode] = 0;
                            ViewBag.resultcode = "0";
                            ViewBag.status = "00";
                            array_Response[ErrorMsg] = "Selected flight not available";
                            ViewBag.ErrorMsg = "Selected flight not available" + "-(#03)";
                        }
                    }
                    else if (_availRes.ResultCode == "3")
                    {
                        List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;
                        List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;
                        RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                        bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                        string TokenBooking = lstrPriceItenary[0].Token;
                        _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                        _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                        _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                        _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                        _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;

                        string strErrtemp = "";
                        #region log


                        StringWriter strresWriter = new StringWriter();
                        DataSet dsresponse = new DataSet();
                        dsresponse = Serv.convertJsonStringToDataSet(strResponse, "");
                        dsresponse.WriteXml(strresWriter);

                        string lstrtime = "SelectResponse" + DateTime.Now;

                        string lstrCata = Trip + " ~SRS~" +
                       PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;

                        string LstrRespDetails = "<SELECT_RESPONSE><REQTIME>" + ReqTime + "</REQTIME><RESPONSETIME>" + lstrtime + "</RESPONSETIME>"
                             + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                       "<Flights>" + (_availResponse == null || _availResponse.Flights == null ? "NULL" : _availResponse.Flights.Count.ToString()) + "</Flights>" +
                       "<Fares>" + (_availResponse == null || _availResponse.Fares == null ? "NULL" : _availResponse.Fares.Count.ToString()) + "</Fares>" +
                       "<Bagg>" + (_availResponse == null || _availResponse.Bagg == null ? "NULL" : _availResponse.Bagg.Count.ToString()) + "</Bagg>" +
                       "<Meal>" + (_availResponse == null || _availResponse.Meal == null ? "NULL" : _availResponse.Meal.Count.ToString()) + "</Meal>" +
                       "<DOBMandatory>" + (_availRes == null || _availRes.DOBMandatory == null ? "NULL" : _availRes.DOBMandatory.ToString()) + "</DOBMandatory>" +
                       "<PassportMandatory>" + (_availRes == null || _availRes.PassportMandatory == null ? "NULL" : _availRes.PassportMandatory.ToString()) + "</PassportMandatory>"
                            + strresWriter.ToString() + "</SELECT_RESPONSE>";

                        DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE SUCCESS " + TokenBooking, LstrRespDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());





                        # endregion

                        //   bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;
                        Session.Add("BestBuy" + ValKey, bestbuy.ToString());
                        Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                        Session.Add("Requestmarkup" + ValKey, (oldfaremarkup.ToString() != "" && oldfaremarkup.ToString() != null ? oldfaremarkup : "0"));

                        dtBaggageSelect = new DataTable();
                        dtmealseSelect = new DataTable();
                        dtOtherSsrsel = new DataTable();
                        dtbuggout = new DataTable();
                        Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse, ref strErrtemp,
                            ref dtBaggageSelect, ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtbuggout);

                        if (!string.IsNullOrEmpty(strErrtemp))
                        {

                            DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", "GRID SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                            Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                            array_Response[ResultCode] = 0;
                            ViewBag.resultcode = "0";
                            array_Response[ErrorMsg] = "Unable to select flight-(#03).";
                            ViewBag.ErrorMsg = "Unable to select flight" + "-(#03).";
                            ViewBag.status = "00";
                        }
                        if (dtSelectResponse != null
                                   && dtSelectResponse.Columns.Count > 1
                                   && dtSelectResponse.Rows.Count > 0)
                        {

                            dtSelectResponse.TableName = "TrackFareDetails";
                            if (Multireqst == "Y")
                            {
                                Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                            }
                            else
                            {
                                Session.Add("Response" + ValKey, dtSelectResponse);
                            }

                            ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);

                            Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                            Session.Add("BaseDest" + ValKey, BaseDestination);
                            Session.Add("TripType" + ValKey, Trip);
                            Session.Add("Specialflagfare" + ValKey, "");
                            Session.Add("TokenBooking" + ValKey, TokenBooking);
                            Session.Add("Deaprt" + ValKey, Deaprt);
                            Session.Add("Arrive" + ValKey, Arrive);
                            Session.Add("DepartureDate" + ValKey, DEPTDATE);
                            Session.Add("RetDate" + ValKey, ARRDATE);
                            Session.Add("Stock" + ValKey, StkType);
                            Session.Add("segmclass" + ValKey, Class);
                            Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                            Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                            array_Response[ResultCode] = _availRes.ResultCode.ToString();
                            ViewBag.resultcode = _availRes.ResultCode.ToString();
                            ViewBag.status = _availRes.ResultCode.ToString();
                            array_Response[ErrorMsg] = _availRes.Error;
                            ViewBag.ErrorMsg = _availRes.Error;
                        }
                        else
                        {
                            DatabaseLog.LogData(Session["username"].ToString(), "ER", "InternationalFlights", "SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                            Returnval = "Problem occured while select a flight";
                            array_Response[ResultCode] = 0;
                            ViewBag.resultcode = "0";
                            ViewBag.status = "00";
                            ViewBag.ErrorMsg = Returnval;
                            array_Response[ErrorMsg] = Returnval;
                        }
                    }
                    else
                    {
                        Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                        array_Response[ResultCode] = 0;
                        ViewBag.resultcode = "0";
                        array_Response[ErrorMsg] = Returnval;
                        ViewBag.ErrorMsg = Returnval + "-(#01)";
                        string lstrCata = Trip + " ~SRN~" +
                        PriceIti.Stock.ToString()
                           + "~" + BaseOrgin + "~" + BaseDestination;
                        DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE NULL", Returnval, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                        ViewBag.status = "00";
                    }
                    ViewBag.Totslpaxcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString() : "";
                    ViewBag.Adultcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString().Split(',')[0] : "";
                    ViewBag.Childcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString().Split(',')[1] : "0";
                    ViewBag.Infantcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString().Split(',')[2] : "0";

                    List<DataRow> _listonewaydata = new List<DataRow>();
                    List<DataRow> _listroundtripdata = new List<DataRow>();
                    List<int> _listTotaldata = new List<int>();

                    //_listTotaldata.Add(_listonewaydata);

                    ViewBag.Trips = Trip;
                    ViewBag.Sector = "";
                    ViewBag.baggage = "";
                    ViewBag.Fre_flyr_no = "";
                    ViewBag.seatmappopup = "";
                    ViewBag.Meals = "";
                    ViewBag.BaggageSSR = "";
                    ViewBag.FFN = "";
                    ViewBag.OtherSSR = "";

                    if (dtSelectResponse != null && dtSelectResponse.Rows.Count > 0)
                    {
                        double Grossamount = loadsessionvaluesreturn(dtSelectResponse, Convert.ToInt32(ViewBag.Adultcount), Convert.ToInt32(ViewBag.Childcount), Convert.ToInt32(ViewBag.Infantcount), ValKey);
                        ArrayList SSRRetArray = Bll.Create_Meals_Bagg(dtSelectResponse, ValKey, dtOtherSsrsel, dtbuggout, strAdults, strChildrens);

                        array_Response[grossfare] = Grossamount;
                        //array_Response[TokenBooking] = TokenBooking;

                        ViewBag.Sector = SSRRetArray[0].ToString();
                        ViewBag.baggage = SSRRetArray[2].ToString();
                        ViewBag.Fre_flyr_no = SSRRetArray[6].ToString();
                        ViewBag.frqno = SSRRetArray[5].ToString();
                        ViewBag.seatmappopup = SSRRetArray[8].ToString();
                        ViewBag.Insurancedata = SSRRetArray[11].ToString();
                        ViewBag.Otherssrdata = SSRRetArray[15].ToString();
                        ViewBag.Meals = SSRRetArray[22].ToString();

                        ViewBag.BaggageSSR = SSRRetArray[23].ToString();
                        ViewBag.FFN = SSRRetArray[24].ToString();
                        ViewBag.OtherSSR = SSRRetArray[25].ToString();

                        Session.Add("SSR", SSRRetArray[4].ToString());
                        Session.Add("FRQ", SSRRetArray[7].ToString());
                    }

                    string td_Adult, td_child, td_infant = string.Empty;

                    td_Adult = ViewBag.Adultcount;
                    td_child = ViewBag.Childcount;
                    td_infant = ViewBag.Infantcount;
                    Session.Add("PaxCount" + ValKey, td_Adult + "|" + td_child + "|" + td_infant);

                    ViewBag.ValKey = ValKey;
                    ViewBag.ServerDateTime = Base.LoadServerdatetime();
                }
            }
            catch (Exception ex)
            {
                ViewBag.status = "00";
                Returnval = ex.Message;
                DatabaseLog.LogData(Session["username"].ToString(), "X", "InternationalFlights", "SELECT REQUEST", ex.ToString() + ex.StackTrace.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                array_Response[ResultCode] = 0;
                ViewBag.resultcode = "0";
                array_Response[ErrorMsg] = Returnval;
                ViewBag.ErrorMsg = Returnval + "-(#05)";
            }
            if (ConfigurationManager.AppSettings["APP_HOSTING"] == "BSA" || ConfigurationManager.AppSettings["APP_HOSTING"] == "B2B"
                            || ConfigurationManager.AppSettings["APP_HOSTING"] == "BOA")
            {
                return PartialView("_AvailSelect_BSA", "");
            }
            else
            {
                return PartialView("_AvailSelect", "");
            }
        }

        public ActionResult Fetchlocaldata()
        {
            DataTable dtSelectResponse = new DataTable();
            DataTable dtSelectResponsetmp = new DataTable();
            DataTable dtBookreq = new DataTable();
            DataTable dtBaggageSelect = new DataTable();
            DataTable dtmealseSelect = new DataTable();
            DataTable dtOtherSsrsel = new DataTable();
            DataTable dtOtheroutbugg = new DataTable();
            ArrayList array_Response = new ArrayList();

            array_Response.Add("");
            array_Response.Add("");
            //array_Response.Add("");
            Base.ServiceUtility Serv = new Base.ServiceUtility();
            RQRS.PriceItenaryRS _availRes = new RQRS.PriceItenaryRS();


            int ResultCode = 0;
            int ErrorMsg = 1;
            // int selectrequest = 2;
            string Returnval = "";
            string strResponse = string.Empty;
            try
            {
                ViewBag.bboldgross = "";
                string requestinfo = Session["sel_Request"].ToString();
                string[] FlightRequest = new string[] { };

                if (requestinfo.Contains("SplItRaJeshTrav"))
                {
                    FlightRequest = Regex.Split(requestinfo, "SplItRaJeshTrav");

                }

                string FliNum = FlightRequest[0].ToString();
                string Deaprt = FlightRequest[1].ToString();
                string Arrive = FlightRequest[2].ToString();
                string FullFlag = FlightRequest[3].ToString();
                string TokenKey = FlightRequest[4].ToString();
                string Trip = FlightRequest[5].ToString();
                string BaseOrgin = FlightRequest[6].ToString();
                string BaseDestination = FlightRequest[7].ToString();
                string offflg = FlightRequest[8].ToString();
                string Class = FlightRequest[9].ToString();
                string TKey = FlightRequest[10].ToString();
                string DEPTDATE = FlightRequest[11].ToString();
                string ARRDATE = FlightRequest[12].ToString();
                string Nfarecheck = FlightRequest[13].ToString();
                string oldfaremarkup = FlightRequest[14].ToString();
                string mobile = FlightRequest[15].ToString();
                string bestbuyrequired = FlightRequest[16].ToString();
                string AlterQueue = FlightRequest[17].ToString();
                string SegmentType = FlightRequest[18].ToString();
                string Multireqst = FlightRequest[19].ToString();
                string reqcnt = FlightRequest[20].ToString();
                string RtripComparFlg = FlightRequest[21].ToString();
                string ValKey = FlightRequest[22].ToString();


                #region Form Booking Request .......
                string Origin = "";
                string Destination = "";
                string depttime = string.Empty;
                string ArrTime = string.Empty;
                string CNX = string.Empty;
                string cabin, FareID = string.Empty;
                string FareCode = string.Empty;
                string SClass = string.Empty;
                string AirlineCategory = string.Empty;
                string OFFLINEFLAG = offflg;
                string strAdults = "";
                string strChildrens = "";
                string strInfants = "";
                string PlatingCarrier = "";
                string RBDCode = "";
                //string TransactionFlag = "";
                string ConnectionFlag = "";
                string BaseAmount = "";
                string GrossAmount = "";
                string StkType = "";

                RQRS.PriceItineary PriceIti = new RQRS.PriceItineary();
                List<RQRS.Itineraries> ListItenar = new List<RQRS.Itineraries>();
                List<RQRS.Flights> FlightsDet = new List<RQRS.Flights>();
                RQRS.Itineraries itinflights = new RQRS.Itineraries();
                RQRS.SegmentDetails SegDet = new RQRS.SegmentDetails();

                RQRS.Flights Itindet = new RQRS.Flights();

                int flightcounts = 0;

                string[] TerminalDetails = new string[] { };
                string[] ArrFliDet, FlightCount, IFCount;

                IFCount = Regex.Split(FullFlag, "SpLiTWeB");

                ListItenar = new List<RQRS.Itineraries>();

                for (int iF = 0; iF < IFCount.Length; iF++)
                {


                    if (IFCount[iF].Contains("SpLITSaTIS"))
                    {
                        // FlightCount = FullFlag.Split('>');


                        FlightCount = Regex.Split(IFCount[iF], "SpLITSaTIS");
                        TerminalDetails = new string[FlightCount.Length];

                        FlightsDet = new List<RQRS.Flights>();

                        for (int fl = 0; fl < FlightCount.Length; fl++)
                        {
                            itinflights = new RQRS.Itineraries();

                            flightcounts++;
                            if (FlightCount[fl].Contains("SpLitPResna"))
                            {
                                ArrFliDet = Regex.Split(FlightCount[fl], "SpLitPResna");
                                if (ArrFliDet[24] != null && ArrFliDet[24] != "")
                                {
                                    Session.Add("terminaldetails", ArrFliDet[24]);
                                }
                                TerminalDetails[fl] = ArrFliDet[24];
                                Origin = ArrFliDet[0];
                                Destination = ArrFliDet[1];
                                depttime = ArrFliDet[2];
                                ArrTime = ArrFliDet[3];
                                SClass = Class;
                                strAdults = ArrFliDet[6];
                                strChildrens = ArrFliDet[7];
                                strInfants = ArrFliDet[8];
                                PlatingCarrier = ArrFliDet[9];
                                RBDCode = ArrFliDet[5];
                                BaseAmount = ArrFliDet[13];
                                GrossAmount = ArrFliDet[14];
                                ConnectionFlag = ArrFliDet[15];
                                AirlineCategory = ArrFliDet[16];
                                FareCode = ArrFliDet[17];
                                cabin = ArrFliDet[21];
                                FareID = ArrFliDet[25];

                                Itindet = new RQRS.Flights();
                                Itindet.AirlineCategory = AirlineCategory;
                                Itindet.ArrivalDateTime = ArrTime;
                                Itindet.CNX = CNX;

                                Itindet.DepartureDateTime = depttime.Trim();
                                Itindet.Destination = Destination;
                                Itindet.FareBasisCode = FareCode.Trim();
                                Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                                Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                                Itindet.PlatingCarrier = PlatingCarrier;
                                Itindet.Class = SClass;//strFareclassOption.ToString().Trim();
                                Itindet.RBDCode = RBDCode;
                                //Itindet.Cabin = cabin;
                                Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                                Itindet.OfflineFlag = Convert.ToInt16(OFFLINEFLAG.Trim().Split('~')[iF]);
                                Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                                Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                                Itindet.ItinRef = Multireqst == "Y" ? iF.ToString() : ArrFliDet[19];
                                Itindet.SegRef = ArrFliDet.Length > 29 ? ArrFliDet[29] : "";
                                Itindet.CNX = RtripComparFlg != "0" ? ArrFliDet[20] : (fl != FlightCount.Length - 1 ? "Y" : "N"); //Roundtrip Spl RQ from Roundtrip Avail... tat's y CNX hardcoded...
                                Itindet.FareId = FareID;
                                StkType = ArrFliDet[23];

                                FlightsDet.Add(Itindet);

                            }
                            itinflights.BaseAmount = BaseAmount;// dtBookFlight.Rows[0]["BasicFare"].ToString();
                            itinflights.GrossAmount = GrossAmount;// dtBookFlight.Rows[0]["GrossFare"].ToString();
                            itinflights.FlightDetails = FlightsDet;
                            itinflights.Stock = StkType;


                        }
                        ListItenar.Add(itinflights);

                    }
                    else
                    {
                        flightcounts++;
                        itinflights = new RQRS.Itineraries();
                        FlightsDet = new List<RQRS.Flights>();
                        TerminalDetails = new string[1];
                        if (IFCount[iF].Contains("SpLitPResna"))
                        {
                            // ArrFliDet = FullFlag.Split('~');
                            ArrFliDet = Regex.Split(IFCount[iF], "SpLitPResna");


                            string[] segmentarr = null;
                            if (ArrFliDet[24] != null && ArrFliDet[24] != "")
                            {
                                string terminaldetail = ArrFliDet[24];
                                segmentarr = terminaldetail.Split('\n');
                            }
                            TerminalDetails[0] = ArrFliDet[24];
                            Origin = ArrFliDet[0];
                            Destination = ArrFliDet[1];
                            depttime = ArrFliDet[2];
                            ArrTime = ArrFliDet[3];
                            SClass = Class;
                            strAdults = ArrFliDet[6];
                            strChildrens = ArrFliDet[7];
                            strInfants = ArrFliDet[8];
                            PlatingCarrier = ArrFliDet[9];
                            RBDCode = ArrFliDet[5];
                            BaseAmount = ArrFliDet[13];
                            GrossAmount = ArrFliDet[14];
                            ConnectionFlag = ArrFliDet[15];
                            AirlineCategory = ArrFliDet[16];
                            FareCode = ArrFliDet[17];
                            FareID = ArrFliDet[25];

                            Itindet = new RQRS.Flights();

                            Itindet.AirlineCategory = AirlineCategory;
                            Itindet.ArrivalDateTime = ArrTime;
                            Itindet.DepartureDateTime = depttime.Trim();
                            Itindet.Destination = Destination;
                            Itindet.FareBasisCode = FareCode.Trim();
                            Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                            Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                            Itindet.PlatingCarrier = PlatingCarrier;
                            Itindet.Class = SClass;//strFareclassOption.ToString().Trim();
                            Itindet.RBDCode = RBDCode;
                            Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                            Itindet.OfflineFlag = Convert.ToInt16(OFFLINEFLAG.Trim().Split('~')[iF]);
                            Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                            Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                            Itindet.ItinRef = Multireqst == "Y" ? iF.ToString() : ArrFliDet[19];
                            Itindet.SegRef = ArrFliDet.Length > 29 ? ArrFliDet[29] : "";
                            Itindet.CNX = ArrFliDet[20];
                            StkType = ArrFliDet[23];
                            Itindet.FareId = FareID;
                            FlightsDet.Add(Itindet);
                        }

                        itinflights.BaseAmount = BaseAmount;// dtBookFlight.Rows[0]["BasicFare"].ToString();
                        itinflights.GrossAmount = GrossAmount;// dtBookFlight.Rows[0]["GrossFare"].ToString();
                        itinflights.FlightDetails = FlightsDet;
                        itinflights.Stock = StkType;

                        // ListItenar.Add(itinflights);
                        ListItenar.Add(itinflights);
                    }

                }

                SegDet.BaseOrigin = BaseOrgin;
                SegDet.BaseDestination = BaseDestination;
                SegDet.Adult = strAdults;
                SegDet.Child = strChildrens != null && strChildrens != "" ? strChildrens.Trim() : "0";
                SegDet.Infant = strInfants != null && strInfants != "" ? strInfants.Trim() : "0";
                SegDet.SegmentType = SegmentType; //Trip=="R"?"D":"I";
                SegDet.ClassType = SClass;
                SegDet.BookingType = "B2B";
                SegDet.TripType = (Trip == "Y" && ConfigurationManager.AppSettings["COUNTRY"] != null && ConfigurationManager.AppSettings["COUNTRY"].ToString() == "AE") ? "R" : Trip;
                SegDet.AppType = "W";
                SegDet.SinglePNR = RtripComparFlg; //1-SinglePNR for Roundtrip Avail, 0-Roundtrip spl rq for Roundtrip avail (compare popup), "" means else part... by saranraj...


                RQRS.AgentDetails agent = new RQRS.AgentDetails();
                agent.AgentId = Session["Availagentid"].ToString();
                agent.Agenttype = "";
                agent.AirportID = SegmentType;//"D";
                agent.AppType = "B2B";
                agent.BOAID = Session["POS_ID"].ToString();
                agent.BOAterminalID = Session["POS_TID"].ToString();
                agent.BranchID = "";
                agent.ClientID = "";
                agent.CoOrdinatorID = "";
                agent.Environment = "W";
                agent.TerminalId = Session["Availterminal"].ToString();
                agent.UserName = Session["username"].ToString();
                agent.Version = "";




                PriceIti.ItinearyDetails = ListItenar;
                PriceIti.BestBuyOption = bestbuyrequired == "TRUE" ? true : false;
                PriceIti.SegmnetDetails = SegDet;
                PriceIti.AgentDetails = agent;
                PriceIti.Stock = StkType;
                PriceIti.AlterQueue = AlterQueue;

                // ViewBag.FltDets = JsonConvert.SerializeObject(ListItenar);//For showing Flight details in Razor view...
                #endregion

                if (Session["Priceresponse"] != null && Session["Priceresponse"] != "")
                {
                    strResponse = Session["Priceresponse"].ToString();



                    ViewBag.triptype = Trip;
                    if (string.IsNullOrEmpty(strResponse))
                    {

                        #region log
                        string lstrCata = Trip + " ~SRN~" +
                           PriceIti.Stock.ToString()
                       + "~" + BaseOrgin + "~" + BaseDestination;

                        //DatabaseLog.LogData("S", "InternationalFlights", lstrCata + " :SELECT RESPONSE NULL",
                        // "Null Or Empty");

                        DatabaseLog.LogData(Session["username"].ToString(), "S", "Fetchlocaldata", lstrCata + " :SELECT RESPONSE NULL", "Null Or Empty", Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                        #endregion

                    }
                    else
                    {


                        DataSet dsselect = new DataSet();
                        dsselect = Serv.convertJsonStringToDataSet(strResponse, "");
                        DataTable dsselect1 = new DataTable();
                        dsselect1 = dsselect.Tables[0];
                        Session.Add("Dobmand" + ValKey, dsselect1);

                        Session.Add("valkey", ValKey);
                        _availRes = JsonConvert.DeserializeObject<RQRS.PriceItenaryRS>(strResponse);
                        if (_availRes.ResultCode == "1" || _availRes.ResultCode == "2")
                        {

                            List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;
                            List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;
                            RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                            bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                            string TokenBooking = lstrPriceItenary[0].Token;
                            _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                            _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                            _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                            _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                            _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;

                            if (Multireqst == "Y")
                            {
                                Session.Add("token" + reqcnt + ValKey, TokenBooking); //Added by saranraj for Domestic Multicity...
                            }

                            if ((PriceIti.ItinearyDetails != null) && (PriceIti.ItinearyDetails.Count > 0) && lstrPriceItenary[0].PriceRS != null &&
                                                                (lstrPriceItenary[0].PriceRS.Count > 0) &&
                                (flightcounts == lstrPriceItenary[0].PriceRS[0].Flights.Count()))//PriceIti.ItinearyDetails[0].FlightDetails.Count()
                            {

                                ViewBag.Bookticketoption = Session["ticketing"] != null ? Session["ticketing"].ToString().Trim() : "N";
                                ViewBag.FltDets = JsonConvert.SerializeObject(lstrPriceItenary[0].PriceRS[0].Flights);
                                string str_Allow_Block_Pnr = (_availRes.AllowBlockPNR != null && _availRes.AllowBlockPNR != "" && _availRes.AllowBlockPNR.ToString().ToUpper() == "TRUE" ? "Y" : "N");

                                ViewBag.Blockticketoption = Session["block"] != null && Session["block"].ToString().Trim() == "Y" && str_Allow_Block_Pnr == "Y" ? "Y" : "N";

                                ViewBag.hdnbestbuyallow = _availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[3] : "N";  //added by Rajesh
                                ViewBag.hdnbargainflag = _availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[2] : "N";   //added by Rajesh
                                ViewBag.hdnallowtune = _availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[4] : "N";  //added by Rajesh

                                ViewBag.hdnDObMand = _availRes.DOBMandatory != null && _availRes.DOBMandatory != "" ? _availRes.DOBMandatory.ToString().ToUpper() : "TRUE";
                                ViewBag.hdnPassmand = _availRes.PassportMandatory != null && _availRes.PassportMandatory != "" ? _availRes.PassportMandatory.ToString().ToUpper() : "TRUE";

                                ViewBag.shoqcommitons = Session["commission"] != null ? Session["commission"].ToString().Trim() : "N";

                                string strErrtemp = "";
                                string GrossAmnt = "";

                                string[] GAmnt;

                                #region log


                                StringWriter strresWriter = new StringWriter();
                                DataSet dsresponse1 = new DataSet();
                                dsresponse1 = Serv.convertJsonStringToDataSet(strResponse, "");
                                dsresponse1.WriteXml(strresWriter);

                                string lstrtime = "SelectResponse" + DateTime.Now;

                                string lstrCata = Trip + " ~SRS~" +
                               PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;

                                string LstrRespDetails = "<SELECT_RESPONSE><RESPONSETIME>" + lstrtime + "</RESPONSETIME>" +
                               "<Flights>" + (_availResponse == null || _availResponse.Flights == null ? "NULL" : _availResponse.Flights.Count.ToString()) + "</Flights>" +
                               "<Fares>" + (_availResponse == null || _availResponse.Fares == null ? "NULL" : _availResponse.Fares.Count.ToString()) + "</Fares>" +
                               "<Bagg>" + (_availResponse == null || _availResponse.Bagg == null ? "NULL" : _availResponse.Bagg.Count.ToString()) + "</Bagg>" +
                               "<Meal>" + (_availResponse == null || _availResponse.Meal == null ? "NULL" : _availResponse.Meal.Count.ToString()) + "</Meal>" +
                               "<DOBMandatory>" + (_availRes == null || _availRes.DOBMandatory == null ? "NULL" : _availRes.DOBMandatory.ToString()) + "</DOBMandatory>" +
                               "<PassportMandatory>" + (_availRes == null || _availRes.PassportMandatory == null ? "NULL" : _availRes.PassportMandatory.ToString()) + "</PassportMandatory>"
                                    + strresWriter.ToString() + "</SELECT_RESPONSE>";

                                DatabaseLog.LogData(Session["username"].ToString(), "S", "Fetchlocaldata", lstrCata + " :SELECT RESPONSE SUCCESS " + TokenBooking, LstrRespDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                # endregion

                                Session.Add("BestBuy" + ValKey, bestbuy.ToString());
                                Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                                Session.Add("Requestmarkup" + ValKey, (oldfaremarkup.ToString() != "" && oldfaremarkup.ToString() != null ? oldfaremarkup : "0"));
                                if (bestbuy == true)
                                {
                                    ViewBag.privtfareoldgros = (string.IsNullOrEmpty((_availResponse.Fares[0].Faredescription[0].OldFare).ToString()) ? "0" : _availResponse.Fares[0].Faredescription[0].OldFare);
                                }

                                dtBaggageSelect = new DataTable();
                                dtmealseSelect = new DataTable();
                                dtOtherSsrsel = new DataTable();
                                dtOtheroutbugg = new DataTable();
                                Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse, ref strErrtemp,
                                     ref dtBaggageSelect, ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtOtheroutbugg);


                                //Added by saranraj to avoid duplicate rows generated in LINQ... on 20170715...
                                if (dtSelectResponse.Rows.Count > 0)
                                {
                                    var temp = (from p in dtSelectResponse.AsEnumerable()
                                                group p by p["SegRef"].ToString() into g
                                                select g.FirstOrDefault()).Distinct();

                                    if (temp.Count() > 0)
                                    {
                                        dtSelectResponse = new DataTable();
                                        dtSelectResponse = temp.CopyToDataTable();
                                    }
                                }
                                //End...

                                if (!string.IsNullOrEmpty(strErrtemp))
                                {

                                    DatabaseLog.LogData(Session["username"].ToString(), "S", "Fetchlocaldata", "GRID SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                    Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                                    array_Response[ResultCode] = 0;
                                    ViewBag.resultcode = "0";
                                    array_Response[ErrorMsg] = "Unable to select flight";
                                    ViewBag.ErrorMsg = "Unable to select flight-(#03)";

                                }
                                if (dtSelectResponse != null
                                              && dtSelectResponse.Columns.Count > 1
                                              && dtSelectResponse.Rows.Count > 0)
                                {

                                    if (TerminalDetails.Count() == dtSelectResponse.Rows.Count)
                                    {
                                        for (int index = 0; index < dtSelectResponse.Rows.Count; index++)
                                        {
                                            dtSelectResponse.Rows[index]["DESTERMINAL"] = TerminalDetails[index].Split('\n').Length > 3 ?
                            (TerminalDetails[index].Split('\n')[3].Split(':').Length > 1 ?
                            TerminalDetails[index].Split('\n')[3].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";

                                            dtSelectResponse.Rows[index]["ORGTERMINAL"] = TerminalDetails[index].Split('\n').Length > 2 ?
                           (TerminalDetails[index].Split('\n')[2].Split(':').Length > 1 ?
                           TerminalDetails[index].Split('\n')[2].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";
                                            dtSelectResponse.AcceptChanges();
                                        }

                                    }
                                    GrossAmnt = dtSelectResponse.Rows[0]["GrossAmount"].ToString();
                                    if (dtSelectResponse.Rows[0]["GrossAmount"].ToString().Contains("|"))
                                    {
                                        GAmnt = GrossAmnt.Split('|');
                                        GrossAmnt = GAmnt[0];
                                    }

                                    if (bestbuy == false)
                                    {
                                        var linq_Grs = (from _lsiti in PriceIti.ItinearyDetails.AsEnumerable()
                                                        group _lsiti by _lsiti.GrossAmount
                                                            into Grsfare
                                                        select new
                                                        {
                                                            GrsAmount = (Grsfare.Sum(A => Convert.ToDecimal(A.GrossAmount)))
                                                        }).ToList().Sum(A => A.GrsAmount);

                                        if (GrossAmnt == linq_Grs.ToString() && _availRes.ResultCode == "1")//changed by udhaya for the process of round trip selecting fare PriceIti.ItinearyDetails[0].GrossAmount.ToString()
                                        {

                                            dtSelectResponse.TableName = "TrackFareDetails";
                                            if (Multireqst == "Y")
                                            {
                                                Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                            }
                                            else
                                            {
                                                Session.Add("Response" + ValKey, dtSelectResponse);
                                            }

                                            Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                            Session.Add("BaseDest" + ValKey, BaseDestination);
                                            Session.Add("TripType" + ValKey, Trip);
                                            Session.Add("Specialflagfare" + ValKey, "");
                                            Session.Add("TokenBooking" + ValKey, TokenBooking);
                                            Session.Add("Deaprt" + ValKey, Deaprt);
                                            Session.Add("Arrive" + ValKey, Arrive);
                                            Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                            Session.Add("RetDate" + ValKey, ARRDATE);
                                            Session.Add("Stock" + ValKey, StkType);
                                            Session.Add("segmclass" + ValKey, Class);
                                            Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                            Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                            ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);

                                            if (Nfarecheck == "FALSE" && _availResponse.FareCheck.CheckFlag == "Y")
                                            {
                                                Returnval = "Fare has been revised from :" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " "
                                               + (Base.ServiceUtility.ConvertToDecimal(PriceIti.ItinearyDetails[0].GrossAmount.ToString())
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))
                                               + " to :" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " "
                                               + (Base.ServiceUtility.ConvertToDecimal(SplitFareByAdultPax(dtSelectResponse.Rows[0]["GROSSAMOUNT"].ToString()).ToString())
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))
                                               + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString()
                                               + Environment.NewLine + " Do you want to continue the flight selection");

                                                array_Response[ResultCode] = 2;
                                                ViewBag.resultcode = "2";
                                                ViewBag.status = "02";
                                                array_Response[ErrorMsg] = Returnval;
                                                ViewBag.ErrorMsg = Returnval;
                                            }
                                            else
                                            {
                                                array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                                ViewBag.resultcode = _availRes.ResultCode.ToString();
                                                ViewBag.status = _availRes.ResultCode.ToString();
                                                array_Response[ErrorMsg] = _availRes.Error;
                                                ViewBag.ErrorMsg = _availRes.Error + "-(#01)";
                                            }
                                        }
                                        else
                                        {

                                            dtSelectResponse.TableName = "TrackFareDetails";
                                            if (Multireqst == "Y")
                                            {
                                                Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                            }
                                            else
                                            {
                                                Session.Add("Response" + ValKey, dtSelectResponse);
                                            }
                                            //string TCnt = strAdults + "," + strChildrens + "," + strInfants;
                                            //Session.Add("TravellerCount" + ValKey, TCnt);
                                            Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                            Session.Add("BaseDest" + ValKey, BaseDestination);
                                            Session.Add("TripType" + ValKey, Trip);
                                            Session.Add("Specialflagfare" + ValKey, "");
                                            Session.Add("TokenBooking" + ValKey, TokenBooking);
                                            Session.Add("Deaprt" + ValKey, Deaprt);
                                            Session.Add("Arrive" + ValKey, Arrive);
                                            Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                            Session.Add("RetDate" + ValKey, ARRDATE);
                                            Session.Add("segmclass" + ValKey, Class);
                                            Session.Add("Stock" + ValKey, StkType);
                                            Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                            Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                            ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);


                                            var linq_Grss = (from _lsiti in PriceIti.ItinearyDetails.AsEnumerable()
                                                             group _lsiti by _lsiti.GrossAmount
                                                                 into Grsfare
                                                             select new
                                                             {
                                                                 GrsAmount = (Grsfare.Sum(A => Convert.ToDecimal(A.GrossAmount)))
                                                             }).ToList().Sum(A => A.GrsAmount);

                                            //  var linqconcatinate = (linq_Grs.Select(a=>a.GrsAmount ).Sum(b => b.GrossAmount)).ToString();
                                            if (_availRes.Error == null || _availRes.Error == "")
                                            {


                                                Returnval = "Fare has been revised from :" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " "
                                                    + (Base.ServiceUtility.ConvertToDecimal(linq_Grss.ToString())
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString() + " to :"
                                                    + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " "
                                                    + (Base.ServiceUtility.ConvertToDecimal(SplitFareByAdultPax(dtSelectResponse.Rows[0]["GROSSAMOUNT"].ToString()).ToString())
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))
                                                    + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString()
                                                    + Environment.NewLine + " Do you want to continue the flight selection";
                                            }
                                            else
                                            {
                                                Returnval = _availRes.Error;
                                            }
                                            array_Response[ResultCode] = 2;
                                            ViewBag.resultcode = "2";
                                            ViewBag.status = "02";
                                            array_Response[ErrorMsg] = Returnval;
                                            ViewBag.ErrorMsg = Returnval;

                                        }
                                    }
                                    else
                                    {

                                        dtSelectResponse.TableName = "TrackFareDetails";
                                        if (Multireqst == "Y")
                                        {
                                            Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                        }
                                        else
                                        {
                                            Session.Add("Response" + ValKey, dtSelectResponse);
                                        }

                                        Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                        Session.Add("BaseDest" + ValKey, BaseDestination);
                                        Session.Add("TripType" + ValKey, Trip);
                                        Session.Add("Specialflagfare" + ValKey, "");
                                        Session.Add("TokenBooking" + ValKey, TokenBooking);
                                        Session.Add("Deaprt" + ValKey, Deaprt);
                                        Session.Add("Arrive" + ValKey, Arrive);
                                        Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                        Session.Add("RetDate" + ValKey, ARRDATE);
                                        Session.Add("Stock" + ValKey, StkType);
                                        Session.Add("segmclass" + ValKey, Class);
                                        Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                        Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);

                                        ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);



                                        array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                        ViewBag.resultcode = _availRes.ResultCode.ToString();
                                        ViewBag.status = _availRes.ResultCode.ToString();
                                        array_Response[ErrorMsg] = _availRes.Error;
                                        ViewBag.ErrorMsg = _availRes.Error + "-(#01)";
                                    }

                                    int IfCount = dtSelectResponse.Rows.Count;
                                    int fCount = 0;

                                    foreach (var Li in PriceIti.ItinearyDetails[0].FlightDetails)
                                    {
                                        if (fCount <= IfCount)
                                        {
                                            if (dtSelectResponse.Rows[fCount]["FareBasisCode"].ToString() != Li.FareBasisCode && ConfigurationManager.AppSettings["Revisedclassalert"].ToString().Contains(StkType) == false)
                                            {
                                                Returnval = "Class has been revised from :" + Li.FareBasisCode + " to :" + dtSelectResponse.Rows[0]["FareBasisCode"].ToString() + Environment.NewLine + " Do you want to continue the flight selection";
                                                array_Response[ResultCode] = 2;
                                                ViewBag.resultcode = "2";
                                                ViewBag.status = "02";
                                                ViewBag.ErrorMsg = Returnval;
                                                array_Response[ErrorMsg] = Returnval;

                                            }
                                            fCount++;
                                        }
                                    }
                                }
                                else
                                {
                                    DatabaseLog.LogData(Session["username"].ToString(), "ER", "Fetchlocaldata", "SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                    Returnval = "Problem occured while select a flight";
                                    ViewBag.ErrorMsg = "Problem occured while select a flight";
                                    array_Response[ResultCode] = 0;
                                    ViewBag.resultcode = "0";
                                    ViewBag.status = "00";
                                    ViewBag.ErrorMsg = Returnval;
                                    array_Response[ErrorMsg] = Returnval;
                                }

                            }
                            else
                            {
                                string XML = "<EVENT><URL>[<![CDATA[" + strResponse + "]]>]</URL></EVENT>";
                                DatabaseLog.LogData(Session["username"].ToString(), "E", "Fetchlocaldata", "SELECT RESPONSE Invalid", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                array_Response[ResultCode] = 0;
                                ViewBag.resultcode = "0";
                                ViewBag.status = "00";
                                array_Response[ErrorMsg] = "Selected flight not available";
                                ViewBag.ErrorMsg = "Selected flight not available" + "-(#03)";
                            }
                        }
                        else if (_availRes.ResultCode == "3")
                        {

                            string strErrtemp = "";

                            List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;
                            List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;
                            RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                            bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                            string TokenBooking = lstrPriceItenary[0].Token;
                            _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                            _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                            _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                            _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                            _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;

                            // string[] GAmnt;



                            #region log


                            StringWriter strresWriter = new StringWriter();
                            DataSet dsresponse = new DataSet();
                            dsresponse = Serv.convertJsonStringToDataSet(strResponse, "");
                            dsresponse.WriteXml(strresWriter);

                            string lstrtime = "SelectResponse" + DateTime.Now;

                            string lstrCata = Trip + " ~SRS~" +
                           PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;

                            string LstrRespDetails = "<SELECT_RESPONSE><RESPONSETIME>" + lstrtime + "</RESPONSETIME>" +
                           "<Flights>" + (_availResponse == null || _availResponse.Flights == null ? "NULL" : _availResponse.Flights.Count.ToString()) + "</Flights>" +
                           "<Fares>" + (_availResponse == null || _availResponse.Fares == null ? "NULL" : _availResponse.Fares.Count.ToString()) + "</Fares>" +
                           "<Bagg>" + (_availResponse == null || _availResponse.Bagg == null ? "NULL" : _availResponse.Bagg.Count.ToString()) + "</Bagg>" +
                           "<Meal>" + (_availResponse == null || _availResponse.Meal == null ? "NULL" : _availResponse.Meal.Count.ToString()) + "</Meal>" +
                           "<DOBMandatory>" + (_availRes == null || _availRes.DOBMandatory == null ? "NULL" : _availRes.DOBMandatory.ToString()) + "</DOBMandatory>" +
                           "<PassportMandatory>" + (_availRes == null || _availRes.PassportMandatory == null ? "NULL" : _availRes.PassportMandatory.ToString()) + "</PassportMandatory>"
                                + strresWriter.ToString() + "</SELECT_RESPONSE>";

                            DatabaseLog.LogData(Session["username"].ToString(), "S", "Fetchlocaldata", lstrCata + " :SELECT RESPONSE SUCCESS " + TokenBooking, LstrRespDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                            # endregion



                            Session.Add("BestBuy" + ValKey, bestbuy.ToString());
                            Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                            Session.Add("Requestmarkup" + ValKey, (oldfaremarkup.ToString() != "" && oldfaremarkup.ToString() != null ? oldfaremarkup : "0"));


                            dtBaggageSelect = new DataTable();
                            dtmealseSelect = new DataTable();
                            dtOtherSsrsel = new DataTable();
                            dtOtheroutbugg = new DataTable();
                            Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse,
                                ref strErrtemp, ref dtBaggageSelect, ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtOtheroutbugg);

                            if (!string.IsNullOrEmpty(strErrtemp))
                            {

                                DatabaseLog.LogData(Session["username"].ToString(), "S", "Fetchlocaldata", "GRID SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                                array_Response[ResultCode] = 0;
                                ViewBag.resultcode = "0";
                                array_Response[ErrorMsg] = "Unable to select flight-(#03).";
                                ViewBag.ErrorMsg = "Unable to select flight" + "-(#03).";
                                ViewBag.status = "00";
                            }
                            if (dtSelectResponse != null
                                       && dtSelectResponse.Columns.Count > 1
                                       && dtSelectResponse.Rows.Count > 0)
                            {

                                dtSelectResponse.TableName = "TrackFareDetails";
                                if (Multireqst == "Y")
                                {
                                    Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                }
                                else
                                {
                                    Session.Add("Response" + ValKey, dtSelectResponse);
                                }

                                ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);

                                Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                Session.Add("BaseDest" + ValKey, BaseDestination);
                                Session.Add("TripType" + ValKey, Trip);
                                Session.Add("Specialflagfare" + ValKey, "");
                                Session.Add("TokenBooking" + ValKey, TokenBooking);
                                Session.Add("Deaprt" + ValKey, Deaprt);
                                Session.Add("Arrive" + ValKey, Arrive);
                                Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                Session.Add("RetDate" + ValKey, ARRDATE);
                                Session.Add("Stock" + ValKey, StkType);
                                Session.Add("segmclass" + ValKey, Class);
                                Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);



                                array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                ViewBag.resultcode = _availRes.ResultCode.ToString();
                                ViewBag.status = _availRes.ResultCode.ToString();
                                array_Response[ErrorMsg] = _availRes.Error;
                                ViewBag.ErrorMsg = _availRes.Error;
                            }
                            else
                            {
                                DatabaseLog.LogData(Session["username"].ToString(), "ER", "Fetchlocaldata", "SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                Returnval = "Problem occured while select a flight";
                                array_Response[ResultCode] = 0;
                                ViewBag.resultcode = "0";
                                ViewBag.status = "00";
                                ViewBag.ErrorMsg = Returnval;
                                array_Response[ErrorMsg] = Returnval;
                            }

                        }

                        else
                        {
                            Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                            array_Response[ResultCode] = 0;
                            ViewBag.resultcode = "0";
                            array_Response[ErrorMsg] = Returnval;
                            ViewBag.ErrorMsg = Returnval + "-(#01)";
                            string lstrCata = Trip + " ~SRN~" +
                            PriceIti.Stock.ToString()
                               + "~" + BaseOrgin + "~" + BaseDestination;
                            DatabaseLog.LogData(Session["username"].ToString(), "S", "Fetchlocaldata", lstrCata + " :SELECT RESPONSE NULL", Returnval, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                            ViewBag.status = "00";
                        }




                        ViewBag.Totslpaxcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString() : "";
                        ViewBag.Adultcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString().Split(',')[0] : "";
                        ViewBag.Childcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString().Split(',')[1] : "0";
                        ViewBag.Infantcount = Session["TravellerCount" + ValKey] != null ? Session["TravellerCount" + ValKey].ToString().Split(',')[2] : "0";


                        List<DataRow> _listonewaydata = new List<DataRow>();
                        List<DataRow> _listroundtripdata = new List<DataRow>();
                        List<int> _listTotaldata = new List<int>();


                        //_listTotaldata.Add(_listonewaydata);

                        ViewBag.Trips = Trip;

                        ViewBag.Sector = "";
                        ViewBag.Meals = "";
                        ViewBag.baggage = "";
                        ViewBag.Fre_flyr_no = "";
                        ViewBag.seatmappopup = "";
                        ViewBag.Meals = "";
                        ViewBag.BaggageSSR = "";
                        ViewBag.FFN = "";
                        ViewBag.OtherSSR = "";
                        if (dtSelectResponse != null && dtSelectResponse.Rows.Count > 0)
                        {
                            loadsessionvalues(dtSelectResponse, Convert.ToInt32(ViewBag.Adultcount), Convert.ToInt32(ViewBag.Childcount), Convert.ToInt32(ViewBag.Infantcount), ValKey);
                            ArrayList SSRRetArray = Bll.Create_Meals_Bagg(dtSelectResponse, ValKey, dtOtherSsrsel, dtOtheroutbugg, strAdults, strChildrens);

                            ViewBag.Sector = SSRRetArray[0].ToString();
                            ViewBag.baggage = SSRRetArray[2].ToString();
                            ViewBag.Fre_flyr_no = SSRRetArray[6].ToString();
                            ViewBag.frqno = SSRRetArray[5].ToString();
                            ViewBag.seatmappopup = SSRRetArray[8].ToString();
                            ViewBag.Insurancedata = SSRRetArray[11].ToString();
                            ViewBag.Otherssrdata = SSRRetArray[15].ToString();
                            ViewBag.Meals = SSRRetArray[22].ToString();
                            ViewBag.BaggageSSR = SSRRetArray[23].ToString();
                            ViewBag.FFN = SSRRetArray[24].ToString();
                            ViewBag.OtherSSR = SSRRetArray[25].ToString();

                            Session.Add("SSR", SSRRetArray[4].ToString());
                            Session.Add("FRQ", SSRRetArray[7].ToString());

                        }

                        string td_Adult, td_child, td_infant = string.Empty;

                        td_Adult = ViewBag.Adultcount;
                        td_child = ViewBag.Childcount;
                        td_infant = ViewBag.Infantcount;

                        //  ViewBag.datalist

                        Session.Add("PaxCount" + ValKey, td_Adult + "|" + td_child + "|" + td_infant);

                        ViewBag.ValKey = ValKey;
                        ViewBag.ServerDateTime = Base.LoadServerdatetime();
                    }
                }
                else
                {
                    string Specialflagfare = "";
                    string ClientID = "";
                    string BranchID = "";
                    string GroupID = "";

                    Flights_DoWork(FliNum, Deaprt, Arrive, FullFlag, TokenKey, Trip, BaseOrgin, BaseDestination
                        , offflg, Class, TKey, DEPTDATE, ARRDATE, Nfarecheck, oldfaremarkup, mobile, bestbuyrequired
                        , AlterQueue, SegmentType, Multireqst, reqcnt, RtripComparFlg, Specialflagfare, ClientID, BranchID, GroupID, false, false, false); ;
                }
            }
            catch (Exception ex)
            {
                ViewBag.status = "00";
                Returnval = ex.Message;
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Fetchlocaldata", "Fetchlocaldata-Select", ex.ToString() + ex.StackTrace.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                array_Response[ResultCode] = 0;
                ViewBag.resultcode = "0";
                array_Response[ErrorMsg] = Returnval;
                ViewBag.ErrorMsg = Returnval + "-(#05)";
            }
            if (ConfigurationManager.AppSettings["APP_HOSTING"] == "BSA" || ConfigurationManager.AppSettings["APP_HOSTING"] == "B2B"
                            || ConfigurationManager.AppSettings["APP_HOSTING"] == "BOA")
            {
                return PartialView("_AvailSelect_BSA", "");
            }
            else
            {
                return PartialView("_AvailSelect", "");
            }
        }


        public static double loadsessionvaluesreturn(DataTable getflightdetails, int td_Adult, int td_child, int td_infant, string ValKey)
        {
            //var totcom = "";
            //var totinst = "";
            double tSCh = 0;
            //double tmarkup = 0;
            string bustbuy = string.Empty;
            string bustbuyfareoldfare = string.Empty;
            string bestbuyoldmarkup = string.Empty;

            if (getflightdetails.Rows[0]["GrossAmount"].ToString().Contains('|'))
            {
                string[] paxtype = getflightdetails.Rows[0]["PaxType"].ToString().Split('|');
                string[] ServSplit = getflightdetails.Rows[0]["ServiceCharge"].ToString().Split('|');

                tSCh = Base.ServiceUtility.CovertToDouble(ServSplit[0]) * td_Adult;
                if (paxtype.Length == 2)
                {
                    var countOfpax = paxtype[1] == "INF" ? td_infant : td_child;
                    var TSCh = Base.ServiceUtility.CovertToDouble(ServSplit[1]) * countOfpax;
                    tSCh += TSCh;
                }
                else if (paxtype.Length == 3)
                {
                    if (paxtype[1] == "CHD" || paxtype[1] != "")
                    {
                        var CSCh = Base.ServiceUtility.CovertToDouble(ServSplit[1]) * td_child;
                        tSCh += CSCh;
                    }
                    if (paxtype[2] == "INF" || paxtype[2] != "")
                    {
                        var ISCh = Base.ServiceUtility.CovertToDouble(ServSplit[2]) * td_infant;
                        tSCh += ISCh;
                    }
                }

            }
            else
            {
                tSCh = Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["ServiceCharge"].ToString()) * td_Adult;
            }
            double GR = 0, Cham = 0, inft = 0, tot = 0, oldgr = 0, oldtot = 0, oldCham = 0, oldinft = 0;



            for (int i = 0; i < getflightdetails.Rows.Count; i++)
            {
                if (getflightdetails.Rows[i]["GrossAmount"].ToString().Contains('|'))
                {

                    string[] Amount = getflightdetails.Rows[i]["GrossAmount"].ToString().Split('|');
                    string[] Paxtype = getflightdetails.Rows[0]["PaxType"].ToString().Split('|');
                    string[] markup = getflightdetails.Rows[0]["markUp"].ToString().Split('|');
                    string[] oldorgamount = getflightdetails.Rows[i]["originaloldgross"].ToString().Split('|');
                    string[] oldorgmarkup = getflightdetails.Rows[0]["originaloldmarkup"].ToString().Split('|');


                    if (Amount.Length == 1)
                    {
                        GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                        oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                        tot = GR;
                        oldtot = oldgr;
                    }

                    if (Amount.Length == 2)
                    {
                        if (Paxtype[1] == "ADT")
                        {
                            if (td_infant > 0)
                                td_child = td_infant;

                            GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                            oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                            Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                            oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                            tot = GR + Cham;

                            oldtot = oldgr + oldCham;
                        }
                        if (Paxtype[1] == "CHD")
                        {
                            GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                            oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                            Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                            oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                            tot = GR + Cham;
                            oldtot = oldgr + oldCham;
                        }
                        if (Paxtype[1] == "INF")
                        {
                            GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                            oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                            inft = (td_infant * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_infant * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                            oldinft = (td_infant * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_infant * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                            tot = GR + inft;
                            oldtot = oldgr + oldinft;
                        }
                    }
                    if (Amount.Length == 3)
                    {
                        GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                        oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                        Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                        oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                        inft = (td_infant * Base.ServiceUtility.CovertToDouble(Amount[2])) + (td_infant * Base.ServiceUtility.CovertToDouble(markup[2].ToString()));
                        oldinft = (td_infant * Base.ServiceUtility.CovertToDouble(oldorgamount[2])) + (td_infant * Base.ServiceUtility.CovertToDouble(oldorgmarkup[2].ToString()));
                        tot = GR + Cham + inft;
                        oldtot = oldgr + oldCham + oldinft;
                    }
                }
                else
                {
                    GR = (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[i]["GrossAmount"].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["Markup"].ToString()));
                    oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[i]["originaloldgross"].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["originaloldmarkup"].ToString()));
                    tot = GR;
                    oldtot = oldgr;
                }
            }
            var OldT = tot;
            tot = tot + tSCh;
            if (bustbuy.ToUpper() == "TRUE")
            {
                var newOldT = oldtot;
                oldtot = oldtot + tSCh;

            }
            return tot;
        }


        public void loadsessionvalues(DataTable getflightdetails, int td_Adult, int td_child, int td_infant, string ValKey)
        {




            //var totcom = "";
            //var totinst = "";
            double tSCh = 0;
            //double tmarkup = 0;
            string bustbuy = string.Empty;
            string bustbuyfareoldfare = string.Empty;
            string bestbuyoldmarkup = string.Empty;
            try
            {
                if (getflightdetails.Rows[0]["GrossAmount"].ToString().Contains('|'))
                {
                    string[] paxtype = getflightdetails.Rows[0]["PaxType"].ToString().Split('|');
                    string[] ServSplit = getflightdetails.Rows[0]["ServiceCharge"].ToString().Split('|');

                    tSCh = Base.ServiceUtility.CovertToDouble(ServSplit[0]) * td_Adult;
                    if (paxtype.Length == 2)
                    {
                        var countOfpax = paxtype[1] == "INF" ? td_infant : td_child;
                        var TSCh = Base.ServiceUtility.CovertToDouble(ServSplit[1]) * countOfpax;
                        tSCh += TSCh;
                    }
                    else if (paxtype.Length == 3)
                    {
                        if (paxtype[1] == "CHD" || paxtype[1] != "")
                        {
                            var CSCh = Base.ServiceUtility.CovertToDouble(ServSplit[1]) * td_child;
                            tSCh += CSCh;
                        }
                        if (paxtype[2] == "INF" || paxtype[2] != "")
                        {
                            var ISCh = Base.ServiceUtility.CovertToDouble(ServSplit[2]) * td_infant;
                            tSCh += ISCh;
                        }
                    }

                }
                else
                {
                    tSCh = Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["ServiceCharge"].ToString()) * td_Adult;
                }
                double GR = 0, Cham = 0, inft = 0, tot = 0, oldgr = 0, oldtot = 0, oldCham = 0, oldinft = 0;



                for (int i = 0; i < getflightdetails.Rows.Count; i++)
                {
                    if (getflightdetails.Rows[i]["GrossAmount"].ToString().Contains('|'))
                    {

                        string[] Amount = getflightdetails.Rows[i]["GrossAmount"].ToString().Split('|');
                        string[] Paxtype = getflightdetails.Rows[0]["PaxType"].ToString().Split('|');
                        string[] markup = getflightdetails.Rows[0]["markUp"].ToString().Split('|');
                        string[] oldorgamount = getflightdetails.Rows[i]["originaloldgross"].ToString().Split('|');
                        string[] oldorgmarkup = getflightdetails.Rows[0]["originaloldmarkup"].ToString().Split('|');


                        if (Amount.Length == 1)
                        {
                            GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                            oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                            tot = GR;
                            oldtot = oldgr;
                        }

                        if (Amount.Length == 2)
                        {
                            if (Paxtype[1] == "ADT")
                            {
                                if (td_infant > 0)
                                    td_child = td_infant;

                                GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                                oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                                Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                                oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                                tot = GR + Cham;

                                oldtot = oldgr + oldCham;
                            }
                            if (Paxtype[1] == "CHD")
                            {
                                GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                                oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                                Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                                oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                                tot = GR + Cham;
                                oldtot = oldgr + oldCham;
                            }
                            if (Paxtype[1] == "INF")
                            {
                                GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                                oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                                inft = (td_infant * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_infant * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                                oldinft = (td_infant * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_infant * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                                tot = GR + inft;
                                oldtot = oldgr + oldinft;
                            }
                        }
                        if (Amount.Length == 3)
                        {

                            GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                            oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                            Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                            oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                            inft = (td_infant * Base.ServiceUtility.CovertToDouble(Amount[2])) + (td_infant * Base.ServiceUtility.CovertToDouble(markup[2].ToString()));
                            oldinft = (td_infant * Base.ServiceUtility.CovertToDouble(oldorgamount[2])) + (td_infant * Base.ServiceUtility.CovertToDouble(oldorgmarkup[2].ToString()));

                            tot = GR + Cham + inft;
                            oldtot = oldgr + oldCham + oldinft;


                        }

                    }
                    else
                    {
                        GR = (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[i]["GrossAmount"].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["Markup"].ToString()));
                        oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[i]["originaloldgross"].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["originaloldmarkup"].ToString()));
                        tot = GR;
                        oldtot = oldgr;


                    }
                }

                var OldT = tot;
                tot = tot + tSCh;
                if (bustbuy.ToUpper() == "TRUE")
                {
                    var newOldT = oldtot;
                    oldtot = oldtot + tSCh;

                }
                Session.Add("totamt" + ValKey, tot);
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "InternationalFlights", "SELECT REQUEST", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }

        }

        public ActionResult Domestic_Flights_DoWork(string Deaprt_on, string Arrive_on, string Deaprt_ret, string Arrive_ret, string FullFlag_on,
           string FullFlag_ret, string Trip, string BaseOrgin, string BaseDestination, string offflg_onn, string offflg_ret, string Class, string TKey,
           string DEPTDATE, string ARRDATE, string Nfarecheck, string mobile, string bestbuyrequired, string AlterQueue)//--?Not used anywhere
        {
            DataTable dtSelectResponse = new DataTable();
            DataTable dtBaggageSelect = new DataTable();
            DataTable dtmealseSelect = new DataTable();
            DataTable dtOtherSsrsel = new DataTable();
            ArrayList array_Response = new ArrayList();
            DataTable dtBookreq = new DataTable();
            DataTable dtBuggout = new DataTable();
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            Base.ServiceUtility Serv = new Base.ServiceUtility();
            int ResultCode = 0;
            int ErrorMsg = 1;
            string Returnval = "";
            try
            {

                string ValKey = "";

                if (TKey != null && TKey != "")
                {
                    if (TKey.Contains('~'))
                    {
                        string[] Availed = TKey.Split('~');
                        ValKey = Availed[Availed.Count() - 2].ToString().Split('_')[1].ToString().Trim();
                    }
                    else
                    {
                        string[] Availed = TKey.Split('_');
                        ValKey = Availed[1].ToString().Trim();
                    }
                }

                #region Form Booking Request .......
                string Origin = "";
                string Destination = "";
                string depttime = string.Empty;
                string ArrTime = string.Empty;
                string CNX = string.Empty;

                string FareCode = string.Empty;
                string SClass = string.Empty;
                string AirlineCategory = string.Empty;
                string OFFLINEFLAG = offflg_onn;
                string strAdults = "";
                string strChildrens = "";
                string strInfants = "";
                string PlatingCarrier = "";
                string RBDCode = "";
                // string TransactionFlag = "";
                string ConnectionFlag = "";
                string BaseAmount = "";
                string GrossAmount = "";
                string StkType = "";
                string Fullflag = FullFlag_on;


                RQRS.PriceItineary PriceIti = new RQRS.PriceItineary();
                List<RQRS.Itineraries> ListItenar = new List<RQRS.Itineraries>();
                List<RQRS.Flights> FlightsDet = new List<RQRS.Flights>();
                RQRS.Itineraries itinflights = new RQRS.Itineraries();
                RQRS.SegmentDetails SegDet = new RQRS.SegmentDetails();
                string[] ArrFliDet, FlightCount;
                if (Fullflag.Contains("SpLITSaTIS"))
                {
                    FlightCount = Regex.Split(Fullflag, "SpLITSaTIS");

                    for (int iF = 0; iF < FlightCount.Length; iF++)
                    {

                        if (FlightCount[iF].Contains("SpLitPResna"))
                        {

                            //ArrFliDet = FlightCount[iF].Split('~');
                            ArrFliDet = Regex.Split(FlightCount[iF], "SpLitPResna");
                            Origin = ArrFliDet[0];
                            Destination = ArrFliDet[1];
                            depttime = ArrFliDet[2];
                            ArrTime = ArrFliDet[3];
                            SClass = Class;
                            strAdults = ArrFliDet[6];
                            strChildrens = ArrFliDet[7];
                            strInfants = ArrFliDet[8];
                            PlatingCarrier = ArrFliDet[9];
                            RBDCode = ArrFliDet[5];
                            BaseAmount = ArrFliDet[13];
                            GrossAmount = ArrFliDet[14];
                            ConnectionFlag = ArrFliDet[15];
                            AirlineCategory = ArrFliDet[16];
                            FareCode = ArrFliDet[17];

                            RQRS.Flights Itindet = new RQRS.Flights();
                            Itindet.AirlineCategory = AirlineCategory;
                            Itindet.ArrivalDateTime = ArrTime;
                            Itindet.CNX = CNX;
                            Itindet.DepartureDateTime = depttime.Trim();
                            Itindet.Destination = Destination;
                            Itindet.FareBasisCode = FareCode.Trim();
                            Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                            Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                            Itindet.PlatingCarrier = PlatingCarrier;
                            Itindet.Class = SClass;//strFareclassOption.ToString().Trim();
                            Itindet.RBDCode = RBDCode;
                            Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                            Itindet.OfflineFlag = Convert.ToInt16(OFFLINEFLAG.Trim());
                            Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                            Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                            Itindet.ItinRef = ArrFliDet[19];
                            Itindet.CNX = ArrFliDet[20];
                            StkType = ArrFliDet[23];
                            FlightsDet.Add(Itindet);

                        }
                    }
                }
                else
                {
                    if (Fullflag.Contains("SpLitPResna"))
                    {
                        // ArrFliDet = FullFlag.Split('~');
                        ArrFliDet = Regex.Split(Fullflag, "SpLitPResna");
                        Origin = ArrFliDet[0];
                        Destination = ArrFliDet[1];
                        depttime = ArrFliDet[2];
                        ArrTime = ArrFliDet[3];
                        SClass = Class;
                        strAdults = ArrFliDet[6];
                        strChildrens = ArrFliDet[7];
                        strInfants = ArrFliDet[8];
                        PlatingCarrier = ArrFliDet[9];
                        RBDCode = ArrFliDet[5];
                        BaseAmount = ArrFliDet[13];
                        GrossAmount = ArrFliDet[14];
                        ConnectionFlag = ArrFliDet[15];
                        AirlineCategory = ArrFliDet[16];
                        FareCode = ArrFliDet[17];

                        RQRS.Flights Itindet = new RQRS.Flights();
                        Itindet.AirlineCategory = AirlineCategory;
                        Itindet.ArrivalDateTime = ArrTime;
                        Itindet.DepartureDateTime = depttime.Trim();
                        Itindet.Destination = Destination;
                        Itindet.FareBasisCode = FareCode.Trim();
                        Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                        Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                        Itindet.PlatingCarrier = PlatingCarrier;
                        Itindet.Class = SClass;//strFareclassOption.ToString().Trim();
                        Itindet.RBDCode = RBDCode;
                        Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                        Itindet.OfflineFlag = Convert.ToInt16(OFFLINEFLAG.Trim());
                        Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                        Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                        Itindet.ItinRef = ArrFliDet[19];
                        Itindet.CNX = ArrFliDet[20];
                        StkType = ArrFliDet[23];
                        FlightsDet.Add(Itindet);
                    }
                }

                itinflights.BaseAmount = BaseAmount;// dtBookFlight.Rows[0]["BasicFare"].ToString();
                itinflights.GrossAmount = GrossAmount;// dtBookFlight.Rows[0]["GrossFare"].ToString();
                itinflights.FlightDetails = FlightsDet;

                ListItenar.Add(itinflights);


                if (FullFlag_ret != "")
                {
                    FlightsDet = new List<RQRS.Flights>();
                    itinflights = new RQRS.Itineraries();
                    Fullflag = FullFlag_ret;
                    if (Fullflag.Contains("SpLITSaTIS"))
                    {
                        FlightCount = Regex.Split(Fullflag, "SpLITSaTIS");

                        for (int iF = 0; iF < FlightCount.Length; iF++)
                        {

                            if (FlightCount[iF].Contains("SpLitPResna"))
                            {

                                //ArrFliDet = FlightCount[iF].Split('~');
                                ArrFliDet = Regex.Split(FlightCount[iF], "SpLitPResna");
                                Origin = ArrFliDet[0];
                                Destination = ArrFliDet[1];
                                depttime = ArrFliDet[2];
                                ArrTime = ArrFliDet[3];
                                SClass = Class;
                                strAdults = ArrFliDet[6];
                                strChildrens = ArrFliDet[7];
                                strInfants = ArrFliDet[8];
                                PlatingCarrier = ArrFliDet[9];
                                RBDCode = ArrFliDet[5];
                                BaseAmount = ArrFliDet[13];
                                GrossAmount = ArrFliDet[14];
                                ConnectionFlag = ArrFliDet[15];
                                AirlineCategory = ArrFliDet[16];
                                FareCode = ArrFliDet[17];

                                RQRS.Flights Itindet = new RQRS.Flights();
                                Itindet.AirlineCategory = AirlineCategory;
                                Itindet.ArrivalDateTime = ArrTime;
                                Itindet.CNX = CNX;
                                Itindet.DepartureDateTime = depttime.Trim();
                                Itindet.Destination = Destination;
                                Itindet.FareBasisCode = FareCode.Trim();
                                Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                                Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                                Itindet.PlatingCarrier = PlatingCarrier;
                                Itindet.Class = SClass;//strFareclassOption.ToString().Trim();
                                Itindet.RBDCode = RBDCode;
                                Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                                Itindet.OfflineFlag = Convert.ToInt16(OFFLINEFLAG.Trim());
                                Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                                Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                                Itindet.ItinRef = ArrFliDet[19];
                                Itindet.CNX = ArrFliDet[20];
                                StkType = ArrFliDet[23];
                                FlightsDet.Add(Itindet);

                            }
                        }
                    }
                    else
                    {

                        if (Fullflag.Contains("SpLitPResna"))
                        {
                            // ArrFliDet = FullFlag.Split('~');
                            ArrFliDet = Regex.Split(Fullflag, "SpLitPResna");
                            Origin = ArrFliDet[0];
                            Destination = ArrFliDet[1];
                            depttime = ArrFliDet[2];
                            ArrTime = ArrFliDet[3];
                            SClass = Class;
                            strAdults = ArrFliDet[6];
                            strChildrens = ArrFliDet[7];
                            strInfants = ArrFliDet[8];
                            PlatingCarrier = ArrFliDet[9];
                            RBDCode = ArrFliDet[5];
                            BaseAmount = ArrFliDet[13];
                            GrossAmount = ArrFliDet[14];
                            ConnectionFlag = ArrFliDet[15];
                            AirlineCategory = ArrFliDet[16];
                            FareCode = ArrFliDet[17];
                            RQRS.Flights Itindet = new RQRS.Flights();
                            Itindet.AirlineCategory = AirlineCategory;
                            Itindet.ArrivalDateTime = ArrTime;
                            Itindet.DepartureDateTime = depttime.Trim();
                            Itindet.Destination = Destination;
                            Itindet.FareBasisCode = FareCode.Trim();
                            Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                            Itindet.Origin = Origin; //ArrOrigin[length].ToString().Trim();
                            Itindet.PlatingCarrier = PlatingCarrier;
                            Itindet.Class = SClass;//strFareclassOption.ToString().Trim();
                            Itindet.RBDCode = RBDCode;
                            Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                            Itindet.OfflineFlag = Convert.ToInt16(OFFLINEFLAG.Trim());
                            Itindet.ConnectionFlag = ConnectionFlag;//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                            Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                            Itindet.ItinRef = ArrFliDet[19];
                            Itindet.CNX = ArrFliDet[20];
                            StkType = ArrFliDet[23];
                            FlightsDet.Add(Itindet);
                        }
                    }
                    itinflights.BaseAmount = BaseAmount;// dtBookFlight.Rows[0]["BasicFare"].ToString();
                    itinflights.GrossAmount = GrossAmount;// dtBookFlight.Rows[0]["GrossFare"].ToString();
                    itinflights.FlightDetails = FlightsDet;


                    ListItenar.Add(itinflights);


                }

                SegDet.BaseOrigin = BaseOrgin;
                SegDet.BaseDestination = BaseDestination;



                SegDet.Adult = strAdults;
                SegDet.Child = strChildrens;
                SegDet.Infant = strInfants;
                SegDet.SegmentType = "I";
                SegDet.ClassType = SClass;
                SegDet.BookingType = "B2B";
                SegDet.TripType = Trip;
                SegDet.AppType = "W";



                RQRS.AgentDetails agent = new RQRS.AgentDetails();
                agent.AgentId = Session["Availagentid"].ToString();
                agent.Agenttype = "";
                agent.AirportID = "I";
                agent.AppType = "B2B";
                agent.BOAID = "";
                agent.BOAterminalID = "";
                agent.BranchID = "";
                agent.ClientID = "";
                agent.CoOrdinatorID = "";
                agent.Environment = "W";
                agent.TerminalId = Session["Availterminal"].ToString();
                agent.UserName = Session["username"].ToString();
                agent.Version = "";


                PriceIti.ItinearyDetails = ListItenar;
                PriceIti.BestBuyOption = bestbuyrequired == "TRUE" ? true : false;
                PriceIti.SegmnetDetails = SegDet;
                PriceIti.AgentDetails = agent;
                PriceIti.Stock = StkType;
                PriceIti.AlterQueue = AlterQueue;


                #endregion

                #region Send Request to RemoteServer......

                try
                {
                    string request = JsonConvert.SerializeObject(PriceIti).ToString();
                    string Query = "InvokeHostCheck";
                    int hostchecktimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);

                    MyWebClient client = new MyWebClient();
                    client.LintTimeout = hostchecktimeout;
                    client.Headers["Content-type"] = "application/json";

                    #region Log

                    string ReqTime = "SelectReqest" + DateTime.Now;

                    StringWriter strWriter = new StringWriter();
                    DataSet dsrequest = new DataSet();
                    dsrequest = Serv.convertJsonStringToDataSet(request, "");
                    dsrequest.WriteXml(strWriter);


                    string lstrCat = Trip + " ~SRQ~" + PriceIti.Stock.ToString()
                        + "~" + BaseOrgin + "~" + BaseDestination;

                    if (ConfigurationManager.AppSettings["Splavailagent"].ToString() != "" && ConfigurationManager.AppSettings["Splavailagent"].ToString().Contains(Session["POS_TID"].ToString()))
                    {
                        strURLpath = ConfigurationManager.AppSettings["Spl_APPS_SELECT_URL"].ToString();
                    }
                    else
                    {
                        strURLpath = ConfigurationManager.AppSettings["APPS_SELECT_URL"].ToString();
                    }

                    string LstrDetails = "<SELECT_REQUEST><URL>[<![CDATA[" + strURLpath
                        + "]]>]</URL><QUERY>" + Query + "</QUERY><REQTIME>" + ReqTime + "</REQTIME><TIMEOUT>" + (hostchecktimeout).ToString() + "</TIMEOUT>" + ((Base.ReqLog) ?
                        strWriter.ToString() : request) + "<JSON>" + request + "</JSON></SELECT_REQUEST>";




                    DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCat + " :SELECT REQUEST", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                    #endregion

                    /****/
                    byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                    string strResponse = System.Text.Encoding.ASCII.GetString(data);
                    /****/

                    if (string.IsNullOrEmpty(strResponse))
                    {

                        #region log
                        string lstrCata = Trip + " ~SRN~" +
                           PriceIti.Stock.ToString()
                       + "~" + BaseOrgin + "~" + BaseDestination;

                        //DatabaseLog.LogData("S", "InternationalFlights", lstrCata + " :SELECT RESPONSE NULL",
                        // "Null Or Empty");

                        DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE NULL", "Null Or Empty", Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                        #endregion

                    }
                    else
                    {
                        DataSet dsselect = new DataSet();
                        dsselect = Serv.convertJsonStringToDataSet(strResponse, "");
                        DataTable dsselect1 = new DataTable();
                        dsselect1 = dsselect.Tables[0];
                        Session.Add("Dobmand" + ValKey, dsselect1);

                        RQRS.PriceItenaryRS _availRes = JsonConvert.DeserializeObject<RQRS.PriceItenaryRS>(strResponse);
                        if (_availRes.ResultCode == "1" || _availRes.ResultCode == "2")
                        {
                            string strErrtemp = "";
                            string GrossAmnt = "";
                            string[] GAmnt;

                            List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;
                            List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;
                            RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                            bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                            string TokenBooking = lstrPriceItenary[0].Token;
                            _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                            _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                            _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                            _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                            _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;


                            #region log


                            StringWriter strresWriter = new StringWriter();
                            DataSet dsresponse1 = new DataSet();
                            dsresponse1 = Serv.convertJsonStringToDataSet(strResponse, "");
                            dsresponse1.WriteXml(strresWriter);

                            string lstrtime = "SelectResponse" + DateTime.Now;

                            string lstrCata = Trip + " ~SRS~" +
                           PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;

                            string LstrRespDetails = "<SELECT_RESPONSE><REQTIME>" + ReqTime + "</REQTIME><RESPONSETIME>" + lstrtime + "</RESPONSETIME>"
                                 + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                           "<Flights>" + (_availResponse == null || _availResponse.Flights == null ? "NULL" : _availResponse.Flights.Count.ToString()) + "</Flights>" +
                           "<Fares>" + (_availResponse == null || _availResponse.Fares == null ? "NULL" : _availResponse.Fares.Count.ToString()) + "</Fares>" +
                           "<Bagg>" + (_availResponse == null || _availResponse.Bagg == null ? "NULL" : _availResponse.Bagg.Count.ToString()) + "</Bagg>" +
                           "<Meal>" + (_availResponse == null || _availResponse.Meal == null ? "NULL" : _availResponse.Meal.Count.ToString()) + "</Meal>" +
                           "<DOBMandatory>" + (_availRes == null || _availRes.DOBMandatory == null ? "NULL" : _availRes.DOBMandatory.ToString()) + "</DOBMandatory>" +
                           "<PassportMandatory>" + (_availRes == null || _availRes.PassportMandatory == null ? "NULL" : _availRes.PassportMandatory.ToString()) + "</PassportMandatory>"
                                + strresWriter.ToString() + "</SELECT_RESPONSE>";

                            DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE SUCCESS " + TokenBooking, LstrRespDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());





                            # endregion
                            Session.Add("BestBuy" + ValKey, bestbuy.ToString());
                            Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                            Session.Add("Requestmarkup" + ValKey, (_availResponse.Fares[0].Faredescription[0].Markup.ToString() != "" && _availResponse.Fares[0].Faredescription[0].Markup.ToString() != null ? _availResponse.Fares[0].Faredescription[0].Markup : "0"));


                            dtBaggageSelect = new DataTable();
                            dtmealseSelect = new DataTable();
                            dtOtherSsrsel = new DataTable();
                            dtBuggout = new DataTable();
                            Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse,
                                ref strErrtemp, ref dtBaggageSelect, ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtBuggout);
                            if (!string.IsNullOrEmpty(strErrtemp))
                            {

                                DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", "GRID SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                                array_Response[ResultCode] = 0;
                                array_Response[ErrorMsg] = "Unable to select flight";

                            }
                            if (dtSelectResponse != null
                                       && dtSelectResponse.Columns.Count > 1
                                       && dtSelectResponse.Rows.Count > 0)
                            {
                                GrossAmnt = dtSelectResponse.Rows[0]["GrossAmount"].ToString();
                                if (dtSelectResponse.Rows[0]["GrossAmount"].ToString().Contains("|"))
                                {
                                    GAmnt = GrossAmnt.Split('|');
                                    GrossAmnt = GAmnt[0];
                                }

                                if (bestbuy == false)
                                {

                                    if (GrossAmnt == PriceIti.ItinearyDetails[0].GrossAmount.ToString())// && _availRes.ResultCode == "1")
                                    {

                                        dtSelectResponse.TableName = "TrackFareDetails";
                                        Session.Add("Response" + ValKey, dtSelectResponse);

                                        Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                        Session.Add("BaseDest" + ValKey, BaseDestination);
                                        Session.Add("TripType" + ValKey, Trip);
                                        Session.Add("Specialflagfare" + ValKey, "");
                                        Session.Add("TokenBooking" + ValKey, TokenBooking);
                                        Session.Add("Deaprt" + ValKey, "");
                                        Session.Add("Arrive" + ValKey, "");
                                        Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                        Session.Add("RetDate" + ValKey, ARRDATE);
                                        Session.Add("Stock" + ValKey, StkType);
                                        Session.Add("segmclass" + ValKey, Class);
                                        Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                        Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);



                                        if (Nfarecheck == "FALSE" && _availResponse.FareCheck.CheckFlag == "Y")
                                        {
                                            Returnval = "Fare has been revised from :" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " " + (Base.ServiceUtility.CovertToDouble(PriceIti.ItinearyDetails[0].GrossAmount.ToString())
                                           + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                            + " to :" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " "
                                           + (Base.ServiceUtility.CovertToDouble(SplitFareByAdultPax(dtSelectResponse.Rows[0]["GROSSAMOUNT"].ToString()).ToString())
                                           + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                           + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString()
                                           + Environment.NewLine + " Do you want to continue the flight selection");

                                            array_Response[ResultCode] = 2;
                                            array_Response[ErrorMsg] = Returnval;
                                        }
                                        else
                                        {
                                            array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                            array_Response[ErrorMsg] = _availRes.Error;
                                        }
                                    }

                                    else
                                    {

                                        dtSelectResponse.TableName = "TrackFareDetails";
                                        Session.Add("Response" + ValKey, dtSelectResponse);
                                        Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                        Session.Add("BaseDest" + ValKey, BaseDestination);
                                        Session.Add("TripType" + ValKey, Trip);
                                        Session.Add("Specialflagfare" + ValKey, "");
                                        Session.Add("TokenBooking" + ValKey, TokenBooking);
                                        Session.Add("Deaprt" + ValKey, "");
                                        Session.Add("Arrive" + ValKey, "");
                                        Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                        Session.Add("RetDate" + ValKey, ARRDATE);
                                        Session.Add("segmclass" + ValKey, Class);
                                        Session.Add("Stock" + ValKey, StkType);
                                        Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                        Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);


                                        Returnval = "Fare has been revised from :" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " "
                                            + (Base.ServiceUtility.CovertToDouble(PriceIti.ItinearyDetails[0].GrossAmount.ToString())
                                            + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                            + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString() + " to :"
                                            + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + " "
                                            + (Base.ServiceUtility.CovertToDouble(SplitFareByAdultPax(dtSelectResponse.Rows[0]["GROSSAMOUNT"].ToString()).ToString())
                                            + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                            + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString()
                                            + Environment.NewLine + " Do you want to continue the flight selection";

                                        array_Response[ResultCode] = 2;
                                        array_Response[ErrorMsg] = Returnval;

                                    }
                                }
                                else
                                {

                                    dtSelectResponse.TableName = "TrackFareDetails";
                                    Session.Add("Response" + ValKey, dtSelectResponse);

                                    Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                    Session.Add("BaseDest" + ValKey, BaseDestination);
                                    Session.Add("TripType" + ValKey, Trip);
                                    Session.Add("Specialflagfare" + ValKey, "");
                                    Session.Add("TokenBooking" + ValKey, TokenBooking);
                                    Session.Add("Deaprt" + ValKey, "");
                                    Session.Add("Arrive" + ValKey, "");
                                    Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                    Session.Add("RetDate" + ValKey, ARRDATE);
                                    Session.Add("Stock" + ValKey, StkType);
                                    Session.Add("segmclass" + ValKey, Class);
                                    Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                    Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);



                                    array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                    array_Response[ErrorMsg] = _availRes.Error;
                                }

                                int IfCount = dtSelectResponse.Rows.Count;
                                int fCount = 0;

                                foreach (var Li in PriceIti.ItinearyDetails[0].FlightDetails)
                                {
                                    if (fCount <= IfCount)
                                    {
                                        if (dtSelectResponse.Rows[fCount]["FareBasisCode"].ToString() != Li.FareBasisCode && ConfigurationManager.AppSettings["Revisedclassalert"].ToString().Contains(StkType) == false)
                                        {
                                            Returnval = "Class has been revised from :" + Li.FareBasisCode + " to :" + dtSelectResponse.Rows[0]["FareBasisCode"].ToString() + Environment.NewLine + " Do you want to continue the flight selection";
                                            array_Response[ResultCode] = 2;
                                            array_Response[ErrorMsg] = Returnval;
                                        }

                                        fCount++;
                                    }
                                }
                            }
                            else
                            {
                                DatabaseLog.LogData(Session["username"].ToString(), "ER", "InternationalFlights", "SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                Returnval = "Problem occured while select a flight";

                            }
                        }
                        else if (_availRes.ResultCode == "3")
                        {

                            string strErrtemp = "";


                            List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;
                            List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;
                            RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                            bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                            string TokenBooking = lstrPriceItenary[0].Token;
                            _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                            _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                            _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                            _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                            _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;

                            #region log


                            StringWriter strresWriter = new StringWriter();
                            DataSet dsresponse = new DataSet();
                            dsresponse = Serv.convertJsonStringToDataSet(strResponse, "");
                            dsresponse.WriteXml(strresWriter);

                            string lstrtime = "SelectResponse" + DateTime.Now;

                            string lstrCata = Trip + " ~SRS~" +
                           PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;

                            string LstrRespDetails = "<SELECT_RESPONSE><REQTIME>" + ReqTime + "</REQTIME><RESPONSETIME>" + lstrtime + "</RESPONSETIME>"
                                 + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                           "<Flights>" + (_availResponse == null || _availResponse.Flights == null ? "NULL" : _availResponse.Flights.Count.ToString()) + "</Flights>" +
                           "<Fares>" + (_availResponse == null || _availResponse.Fares == null ? "NULL" : _availResponse.Fares.Count.ToString()) + "</Fares>" +
                           "<Bagg>" + (_availResponse == null || _availResponse.Bagg == null ? "NULL" : _availResponse.Bagg.Count.ToString()) + "</Bagg>" +
                           "<Meal>" + (_availResponse == null || _availResponse.Meal == null ? "NULL" : _availResponse.Meal.Count.ToString()) + "</Meal>" +
                           "<DOBMandatory>" + (_availRes == null || _availRes.DOBMandatory == null ? "NULL" : _availRes.DOBMandatory.ToString()) + "</DOBMandatory>" +
                           "<PassportMandatory>" + (_availRes == null || _availRes.PassportMandatory == null ? "NULL" : _availRes.PassportMandatory.ToString()) + "</PassportMandatory>"
                                + strresWriter.ToString() + "</SELECT_RESPONSE>";

                            DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE SUCCESS " + TokenBooking, LstrRespDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());





                            # endregion
                            Session.Add("BestBuy" + ValKey, bestbuy.ToString());
                            Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                            Session.Add("Requestmarkup" + ValKey, (_availResponse.Fares[0].Faredescription[0].Markup.ToString() != "" && _availResponse.Fares[0].Faredescription[0].Markup.ToString() != null ? _availResponse.Fares[0].Faredescription[0].Markup : "0"));


                            dtBaggageSelect = new DataTable();
                            dtmealseSelect = new DataTable();
                            dtOtherSsrsel = new DataTable();
                            dtBuggout = new DataTable();
                            Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse, ref strErrtemp, ref dtBaggageSelect,
                                ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtBuggout);

                            if (!string.IsNullOrEmpty(strErrtemp))
                            {

                                DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", "GRID SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                                array_Response[ResultCode] = 0;
                                array_Response[ErrorMsg] = "Unable to select flight";

                            }
                            if (dtSelectResponse != null
                                       && dtSelectResponse.Columns.Count > 1
                                       && dtSelectResponse.Rows.Count > 0)
                            {

                                dtSelectResponse.TableName = "TrackFareDetails";
                                Session.Add("Response" + ValKey, dtSelectResponse);

                                Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                Session.Add("BaseDest" + ValKey, BaseDestination);
                                Session.Add("TripType" + ValKey, Trip);
                                Session.Add("Specialflagfare" + ValKey, "");
                                Session.Add("TokenBooking" + ValKey, TokenBooking);
                                Session.Add("Deaprt" + ValKey, "");
                                Session.Add("Arrive" + ValKey, "");
                                Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                Session.Add("RetDate" + ValKey, ARRDATE);
                                Session.Add("Stock" + ValKey, StkType);
                                Session.Add("segmclass" + ValKey, Class);
                                Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);


                                array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                array_Response[ErrorMsg] = _availRes.Error;
                            }
                            else
                            {
                                DatabaseLog.LogData(Session["username"].ToString(), "ER", "InternationalFlights", "SELECT RESPONSE", strErrtemp, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                                Returnval = "Problem occured while select a flight";

                            }

                        }

                        else
                        {
                            Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                            array_Response[ResultCode] = 0;
                            array_Response[ErrorMsg] = Returnval;
                            string lstrCata = Trip + " ~SRN~" +
                            PriceIti.Stock.ToString()
                               + "~" + BaseOrgin + "~" + BaseDestination;
                            DatabaseLog.LogData(Session["username"].ToString(), "S", "InternationalFlights", lstrCata + " :SELECT RESPONSE NULL", Returnval, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                        }
                    }
                }
                catch (Exception ex)
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "InternationalFlights", "SELECT REQUEST", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                    Returnval = "Problem Occured. Please contact customercare";
                    array_Response[ResultCode] = 0;
                    array_Response[ErrorMsg] = Returnval;


                }
                #endregion
            }
            catch (Exception ex)
            {
                Returnval = ex.Message;
                DatabaseLog.LogData(Session["username"].ToString(), "X", "InternationalFlights", "SELECT REQUEST", ex.ToString() + ex.StackTrace.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                array_Response[ResultCode] = 0;
                array_Response[ErrorMsg] = Returnval;

            }
            return Json(new { Status = "", Message = "", Result = array_Response });
            //return array_Response;

        }

        #endregion

        #region MultiCity b4 Select process starts
        public ActionResult MulFltb4Select(string[] aryInva, string[] arytoken, string[] aryffflg, string Class)
        {
            string[] ArrFliDet, FlightCount;
            try
            {

                Session.Add("domseg", "D");
                ViewBag.triptype = "M";
                if (Session["agentid"] == null)
                {
                    ViewBag.status = "-1";
                    if (ConfigurationManager.AppSettings["APP_HOSTING"] == "BSA" || ConfigurationManager.AppSettings["APP_HOSTING"] == "B2B"
                            || ConfigurationManager.AppSettings["APP_HOSTING"] == "BOA")
                    {
                        return PartialView("_AvailSelect_BSA", "");
                    }
                    else
                    {
                        return PartialView("_AvailSelect", "");
                    }
                }

                List<RQRS.Flights> FlightsDet = new List<RQRS.Flights>();
                RQRS.Itineraries itinflights = new RQRS.Itineraries();
                RQRS.Flights Itindet = new RQRS.Flights();
                List<RQRS.Itineraries> ListItenar = new List<RQRS.Itineraries>();

                FlightsDet = new List<RQRS.Flights>();
                for (int iF = 0; iF < aryInva.Length; iF++) //Segment count loop...
                {
                    FlightCount = Regex.Split(aryInva[iF], "SpLITSaTIS");
                    bool loopcon = false;
                    if (FlightCount.Length > 0)
                    {
                        loopcon = FlightCount[0].Contains("SpLitPResna");
                    }
                    if (loopcon == true)
                    {

                        for (int fl = 0; fl < FlightCount.Length; fl++) // Flight cound loop (for Connecting flight...)
                        {
                            if (FlightCount[fl] != "")
                            {
                                itinflights = new RQRS.Itineraries();
                                ArrFliDet = Regex.Split(FlightCount[fl], "SpLitPResna");

                                Itindet = new RQRS.Flights();
                                Itindet.AirlineCategory = ArrFliDet[16];
                                Itindet.ArrivalDateTime = ArrFliDet[3];
                                Itindet.CNX = "";

                                Itindet.DepartureDateTime = ArrFliDet[2].Trim();
                                Itindet.Destination = ArrFliDet[1];
                                Itindet.FareBasisCode = ArrFliDet[17].Trim();
                                Itindet.FlightNumber = ArrFliDet[18]; //ArrFliNum[length].ToString().Trim();
                                Itindet.Origin = ArrFliDet[0]; //ArrOrigin[length].ToString().Trim();
                                Itindet.PlatingCarrier = ArrFliDet[9];
                                Itindet.Class = Class;//strFareclassOption.ToString().Trim();
                                Itindet.RBDCode = ArrFliDet[5];
                                Itindet.ReferenceToken = ArrFliDet[12]; //(dtBookFlight.Rows.Count == ArrOrigin.Length) ? dtBookFlight.Rows[length]["ReferenceToken"].ToString().Trim() : dtBookFlight.Rows[0]["ReferenceToken"].ToString().Trim();
                                Itindet.OfflineFlag = aryffflg != null && aryffflg.Length > 0 ? Convert.ToInt16(aryffflg[0].Split(',')[0]) : 0;// Convert.ToInt16(aryffflg[0].Split(',')[0]); hide by srinath
                                Itindet.ConnectionFlag = ArrFliDet[15];//(dtBookFlight.Rows.Count > 1) ? "S" : "N";
                                Itindet.TransactionFlag = false;// (chkTransactionFee.Visible == true && chkTransactionFee.Checked == true) ? true : false;
                                Itindet.ItinRef = iF.ToString();// ArrFliDet[19];
                                Itindet.CNX = ArrFliDet[20];
                                Itindet.FareId = ArrFliDet[25];

                                FlightsDet.Add(Itindet);

                                if (fl == 0)
                                {
                                    ViewBag.Adultcount = ArrFliDet[6];
                                    ViewBag.Childcount = ArrFliDet[7];
                                    ViewBag.Infantcount = ArrFliDet[8];
                                }
                            }
                        }
                        // ListItenar.Add(itinflights);
                    }
                    else
                    {

                    }
                }

                ViewBag.txt_UserName = Session["username"].ToString();
                string[] PP = Session["agencyaddress"].ToString().Split('~');
                if (PP.Count() >= 4)
                {
                    ViewBag.txt_AgnNo = PP[4];

                }

                ViewBag.Bookticketoption = Session["ticketing"] != null ? Session["ticketing"].ToString().Trim() : "N";
                ViewBag.ServerDateTime = Base.LoadServerdatetime();
                ViewBag.status = "01";
                ViewBag.Multireqst = "Y";
                //ViewBag.FltDets = JsonConvert.SerializeObject(ListItenar);
                ViewBag.FltDets = JsonConvert.SerializeObject(FlightsDet);
                ViewBag.LstofLstFltDetails = JsonConvert.SerializeObject(FlightsDet.GroupBy(v => v.ItinRef).Select(j => j.ToList()).ToList());
            }
            catch (Exception ex)
            {
                ViewBag.status = "00";
                ViewBag.errorMsg = "Internal Problem occured while select (#05).";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "FlightsController", "MulFltb4Select", ex.ToString() + ex.StackTrace.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
            }
            if (ConfigurationManager.AppSettings["APP_HOSTING"] == "BSA" || ConfigurationManager.AppSettings["APP_HOSTING"] == "B2B"
                            || ConfigurationManager.AppSettings["APP_HOSTING"] == "BOA")
            {
                return PartialView("_AvailSelect_BSA", "");
            }
            else
            {
                return PartialView("_AvailSelect", "");
            }

        }
        #endregion

        private decimal SplitFareByAdultPax(string amount)
        {
            decimal adultFare = 0;
            try
            {
                string[] fareSplitUp = amount.Split('|');
                adultFare = Convert.ToDecimal(fareSplitUp[0].ToString(), CultureInfo.InvariantCulture);

                return adultFare;
            }
            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "SplitFareByAdultPax", "Newavailservice");
                return adultFare;
            }
        }

        #region Get SeatMap's

        public ActionResult getseatmap(string valkey, string Paxcount, string Paxname, string Totalpaxcount, string Adultnamedetails, string Childnamedetails, string ContactNumber, string MailID, string PrimeSeatFlag)//, string Index, string segmentindex,
        {
            ArrayList array_Response = new ArrayList();
            RaysService _rays_servers = new RaysService();
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");

            int Adult_count = Convert.ToInt16(Totalpaxcount.Split('@')[0]); // 1;
            int Child_count = Convert.ToInt16(Totalpaxcount.Split('@')[1]);// 0;
            int Infant_count = Convert.ToInt16(Totalpaxcount.Split('@')[2]);// 0;
            //int rom_index = 0;
            int response = 1;
            StringBuilder strBuliderRes = new StringBuilder();
            StringBuilder strTaxBuild = new StringBuilder();
            DataTable flitab = new DataTable();

            string stu = string.Empty;
            string msg = string.Empty;
            string error_ref = string.Empty;
            try
            {
                string strOrigin = string.Empty;
                string strDestination = string.Empty;
                string strDepdate = string.Empty;
                string strArrdate = string.Empty;
                string strTripType = string.Empty;
                flitab = (DataTable)Session["Response" + valkey];
                PrimeSeatFlag = PrimeSeatFlag.TrimEnd('~');
                string[] strPrime = PrimeSeatFlag.Split('~');

                RQRS.GetSeatMap_RQ _seatreq = new RQRS.GetSeatMap_RQ();
                RQRS.AgentDetailsSeat _agentdet = new RQRS.AgentDetailsSeat();

                //Seat_rays.AgentDetails _agentdet = new Seat_rays.AgentDetails();
                List<RQRS.RQFlights> _flightdet = new List<RQRS.RQFlights>();
                //Segment _segment = new Segment();
                Segment _flightsegment = new Segment();
                List<RQRS.ReqPassDetail> _passdet = new List<RQRS.ReqPassDetail>();
                RQRS.TripDetails newtrip = new RQRS.TripDetails();

                strTerminalId = Session["terminalid"].ToString();
                strUserName = Session["username"].ToString();
                strAgentID = Session["agentid"].ToString();
                Ipaddress = Session["ipAddress"].ToString();
                sequnceID = Session["sequenceid"].ToString();
                string posid = Session["POS_ID"].ToString();
                string postid = Session["POS_TID"].ToString();

                _agentdet.AgentId = strAgentID;
                _agentdet.TerminalId = strTerminalId;
                _agentdet.UserName = strUserName;
                _agentdet.Version = "";
                _agentdet.Environment = Session["TERMINALTYPE"].ToString();
                _agentdet.AppType = "B2B";
                _agentdet.Agenttype = "";
                _agentdet.AirportID = "";
                _agentdet.APPCurrency = Session["App_currency"].ToString();
                _agentdet.BOAID = posid;
                _agentdet.BOAterminalID = postid;
                _agentdet.ClientID = posid;
                _agentdet.CoOrdinatorID = "";
                _agentdet.EMP_ID = "";
                _agentdet.FareType = "";
                _agentdet.IssuingBranchID = "";
                _agentdet.Platform = "";
                _agentdet.ProjectId = Session["PRODUCT_CODE"].ToString();
                _agentdet.BranchID = Session["branchid"].ToString();
                _agentdet.SequenceID = sequnceID;
                _agentdet.TerminalType = Session["TERMINALTYPE"].ToString();

                StringWriter strWriter = new StringWriter();
                flitab.WriteXml(strWriter);

                DatabaseLog.LogData(Session["username"].ToString(), "E", "Getseatmap", "Seat map Table", strWriter.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                string fromdate = string.Empty;
                string todate = string.Empty;
                //int count = 1;

                if (flitab != null && flitab.Rows.Count > 0)//&& flitab.Rows[rom_index].ToString() != null
                {
                    for (int index = 0; index < flitab.Rows.Count; index++)
                    {
                        bool primeflag = false;
                        //if (segmentindex == flitab.Rows[index]["itinRef"].ToString())
                        //{
                        RQRS.RQFlights _flightreq = new RQRS.RQFlights();
                        _flightreq.AirlineCategory = flitab.Rows[index]["AIRLINECATEGORY"].ToString();
                        _flightreq.ArrivalDateTime = flitab.Rows[index]["ArrivalDate"].ToString();
                        _flightreq.Cabin = flitab.Rows[index]["Cabin"].ToString();
                        _flightreq.CarrierCode = flitab.Rows[index]["CarrierCode"].ToString();
                        _flightreq.Class = flitab.Rows[index]["Class"].ToString();
                        _flightreq.DepartureDateTime = flitab.Rows[index]["DepartureDate"].ToString();
                        _flightreq.Destination = flitab.Rows[index]["Destination"].ToString();
                        _flightreq.EndTerminal = flitab.Rows[index]["DESTERMINAL"].ToString();
                        _flightreq.FareBasisCode = flitab.Rows[index]["FAREBASISCODE"].ToString();
                        _flightreq.FareID = flitab.Rows[index]["Faresid"].ToString();
                        _flightreq.FlightNumber = flitab.Rows[index]["FlightNumber"].ToString();
                        _flightreq.ItinRef = flitab.Rows[index]["itinRef"].ToString();
                        _flightreq.Origin = flitab.Rows[index]["Origin"].ToString();
                        _flightreq.PlatingCarrier = flitab.Rows[index]["PlatingCarrier"].ToString();
                        _flightreq.ReferenceToken = flitab.Rows[index]["Token"].ToString();
                        _flightreq.SegRef = flitab.Rows[index]["SegRef"].ToString();// (index + 1).ToString();//
                        _flightreq.StartTerminal = flitab.Rows[index]["ORGTERMINAL"].ToString();
                        _flightreq.FareType = flitab.Rows[index]["FareType"].ToString();
                        _flightreq.SeatAvailFlag = "T";
                        _flightreq.Supplier = flitab.Rows[index]["stocktype"].ToString();
                        for (int s = 0; s < strPrime.Length; s++)
                        {
                            if ((flitab.Rows[index]["Origin"].ToString() + "-" + flitab.Rows[index]["Destination"].ToString()) == strPrime[s])
                            {
                                primeflag = true;
                            }
                        }
                        _flightreq.IsBundleSeatFare = primeflag;
                        //_flightreq.IsBundleSeatFare = PrimeSeatFlag == "" && PrimeSeatFlag != null ? false : (PrimeSeatFlag.Split('-').Length > index ? Convert.ToBoolean(PrimeSeatFlag.Split('-')[index]) : false);
                        _flightdet.Add(_flightreq);
                        //}
                    }
                }

                newtrip.Adultcount = Adult_count.ToString();
                newtrip.Childcount = Child_count.ToString();
                newtrip.Infantcount = Infant_count.ToString();
                newtrip.Destination = Session["BaseDest" + valkey].ToString();
                newtrip.Orgin = Session["BaseOrgin" + valkey].ToString();
                newtrip.Segmenttype = Session["domseg"] != null && Session["domseg"].ToString() != "" ? Session["domseg"].ToString() : "I";
                newtrip.Triptype = (Session["TripType" + valkey].ToString() == "Y") ? "R" : Session["TripType" + valkey].ToString();
                for (var i = 0; i < Adult_count; i++)
                {
                    string[] Passengerdetails = Adultnamedetails.Split('#');
                    string[] currentpaxdetails = Passengerdetails[i].Split('@');
                    RQRS.ReqPassDetail _passdetails = new RQRS.ReqPassDetail();
                    _passdetails.Firstname = currentpaxdetails[1].Split('~')[0];
                    _passdetails.Lastname = currentpaxdetails[1].Split('~')[1];
                    _passdetails.PaxType = "ADT";
                    _passdetails.Title = currentpaxdetails[0];
                    _passdetails.PAXTYPENO = "Adult" + (i + 1);

                    _passdetails.Gender = currentpaxdetails[1].Split('~')[2];
                    _passdetails.DOB = currentpaxdetails[1].Split('~')[3];
                    _passdetails.MailID = MailID;
                    _passdetails.Mobnumber = ContactNumber;
                    _passdet.Add(_passdetails);
                }

                if (Child_count > 0)
                {
                    for (var ch = 0; ch < Child_count; ch++)
                    {
                        string[] Passengerdetails = Childnamedetails.Split('#');
                        string[] currentpaxdetails = Passengerdetails[ch].Split('@');
                        RQRS.ReqPassDetail _passdetails = new RQRS.ReqPassDetail();
                        _passdetails.Firstname = currentpaxdetails[1].Split('~')[0];
                        _passdetails.Lastname = currentpaxdetails[1].Split('~')[1];
                        _passdetails.PaxType = "CHD";
                        _passdetails.Title = currentpaxdetails[0];
                        _passdetails.PAXTYPENO = "Child" + (ch + 1);

                        _passdetails.Gender = currentpaxdetails[1].Split('~')[2];
                        _passdetails.DOB = currentpaxdetails[1].Split('~')[3];
                        _passdetails.MailID = MailID;
                        _passdetails.Mobnumber = ContactNumber;
                        _passdet.Add(_passdetails);
                    }
                }

                _seatreq.AgentDetail = _agentdet;
                _seatreq.FlightsDetails = _flightdet;
                // _seatreq.SegmentDetails = _flightsegment;
                _seatreq.Platform = "B";
                _seatreq.TripDetails = newtrip;
                _seatreq.PassengerDetails = _passdet;
                _seatreq.Stock = flitab.Rows[0]["stocktype"].ToString();

                string request = JsonConvert.SerializeObject(_seatreq).ToString();

                string StrSeatTrackID = "SEATMAPRESPONSE" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                DataSet str_TrackID = new DataSet();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                str_TrackID = _rays_servers.FetchSeatTrack(strAgentID, strTerminalId, strAppType, StrSeatTrackID, request, "I");

                string request1 = "Key=" + UrlEncode(StrSeatTrackID);

                if (request1 != null && request1 != "")
                {
                    array_Response[response] = request1;
                }
                string xml = "<EVENT><KEY>[<![CDATA[" + request1 + "]]>]</KEY></EVENT>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "Getseatmap", "Keyvalue", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
            }
            catch (Exception First_Ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "getseatmap", "Search Request", First_Ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                //return array_Response;
                return Json(new { Status = "", Message = "", Result = array_Response });
            }

            // return array_Response;
            return Json(new { Status = "", Message = "", Result = array_Response });

        }

        public ActionResult getbookinsurnace(string valkey, string Paxcount, string Paxname, string Totalpaxcount, string Adultnamedetails, string Childnamedetails, string Infantnamedetails)//, string Index, string segmentindex,
        {
            ArrayList array_Response = new ArrayList();
            RaysService _rays_servers = new RaysService();
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");

            int Adult_count = Convert.ToInt16(Totalpaxcount.Split('@')[0]); // 1;
            int Child_count = Convert.ToInt16(Totalpaxcount.Split('@')[1]);// 0;
            int Infant_count = Convert.ToInt16(Totalpaxcount.Split('@')[2]);// 0;
            //int rom_index = 0;
            int response = 1;
            StringBuilder strBuliderRes = new StringBuilder();
            StringBuilder strTaxBuild = new StringBuilder();
            DataTable flitab = new DataTable();

            string stu = string.Empty;
            string msg = string.Empty;

            string error_ref = string.Empty;
            try
            {


                string strOrigin = string.Empty;
                string strDestination = string.Empty;
                string strDepdate = string.Empty;
                string strArrdate = string.Empty;
                string strTripType = string.Empty;
                string BOAID = string.Empty;
                string BOAterminalID = string.Empty;
                string BranchID = string.Empty;
                flitab = (DataTable)Session["Response" + valkey];

                Gettuneins_RQ _seatreq = new Gettuneins_RQ();
                // RQRS.AgentDetails _agentdet = new RQRS.AgentDetails();
                Seat_rays.AgentDetails _agentdet = new Seat_rays.AgentDetails();
                List<RQFlights> _flightdet = new List<RQFlights>();
                //Segment _segment = new Segment();
                Segment _flightsegment = new Segment();
                List<TuneReqPassDetail> _passdet = new List<TuneReqPassDetail>();
                TripDetails newtrip = new TripDetails();

                strTerminalId = Session["Availterminal"].ToString();
                strUserName = Session["username"].ToString();
                strAgentID = Session["agentid"] != null && Session["agentid"] != "" ? Session["agentid"].ToString() : "";
                Ipaddress = Session["ipAddress"].ToString();
                sequnceID = Session["sequenceid"].ToString();
                BOAID = Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                BOAterminalID = Session["POS_TID"] != null && Session["POS_TID"] != "" ? Session["POS_TID"].ToString() : "";
                BranchID = Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";


                _agentdet.AgentId = strAgentID;
                _agentdet.TerminalId = strTerminalId;
                _agentdet.UserName = strUserName;
                _agentdet.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                _agentdet.Environment = "W";
                _agentdet.AppType = "B2B";
                _agentdet.BOAID = BOAID;
                _agentdet.BOAterminalID = BOAterminalID;
                _agentdet.ClientID = BOAID;
                _agentdet.BranchId = BranchID;
                _agentdet.AgentType = Session["agenttype"] != null && Session["agenttype"] != "" ? Session["agenttype"].ToString() : "";
                _agentdet.Environment = "B";

                StringWriter strWriter = new StringWriter();
                flitab.WriteXml(strWriter);

                DatabaseLog.LogData(Session["username"].ToString(), "E", "Getseatmap", "Seat map Table", strWriter.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                string fromdate = string.Empty;
                string todate = string.Empty;
                //int count = 1;

                if (flitab != null && flitab.Rows.Count > 0)//&& flitab.Rows[rom_index].ToString() != null
                {
                    for (int index = 0; index < flitab.Rows.Count; index++)
                    {
                        //if (segmentindex == flitab.Rows[index]["itinRef"].ToString())
                        //{
                        RQFlights _flightreq = new RQFlights();
                        _flightreq.AirlineCategory = flitab.Rows[index]["AIRLINECATEGORY"].ToString();
                        _flightreq.ArrivalDateTime = flitab.Rows[index]["ArrivalDate"].ToString();
                        _flightreq.Cabin = flitab.Rows[index]["Cabin"].ToString();
                        _flightreq.CarrierCode = flitab.Rows[index]["CarrierCode"].ToString();
                        _flightreq.Class = flitab.Rows[index]["Class"].ToString();
                        _flightreq.DepartureDateTime = flitab.Rows[index]["DepartureDate"].ToString();
                        _flightreq.Destination = flitab.Rows[index]["Destination"].ToString();
                        _flightreq.EndTerminal = flitab.Rows[index]["DESTERMINAL"].ToString();
                        _flightreq.FareBasisCode = flitab.Rows[index]["FAREBASISCODE"].ToString();
                        _flightreq.FareID = flitab.Rows[index]["Faresid"].ToString();
                        _flightreq.FlightNumber = flitab.Rows[index]["FlightNumber"].ToString();
                        _flightreq.ItinRef = flitab.Rows[index]["itinRef"].ToString();
                        _flightreq.Origin = flitab.Rows[index]["Origin"].ToString();
                        _flightreq.PlatingCarrier = flitab.Rows[index]["PlatingCarrier"].ToString();
                        _flightreq.ReferenceToken = flitab.Rows[index]["Token"].ToString();
                        _flightreq.SegRef = (index + 1).ToString();// flitab.Rows[rom_index]["SegRef"].ToString();// (index + 1).ToString();//
                        _flightreq.StartTerminal = flitab.Rows[index]["ORGTERMINAL"].ToString();
                        _flightreq.FareType = flitab.Rows[index]["FareType"].ToString();
                        _flightreq.SeatAvailFlag = "T";
                        _flightdet.Add(_flightreq);
                        //}
                    }
                }

                newtrip.Adultcount = Adult_count.ToString();
                newtrip.Childcount = Child_count.ToString();
                newtrip.Infantcount = Infant_count.ToString();
                newtrip.Destination = Session["BaseDest" + valkey].ToString();
                newtrip.Orgin = Session["BaseOrgin" + valkey].ToString();
                newtrip.Segmenttype = "I";
                newtrip.Triptype = (Session["TripType" + valkey].ToString() == "Y" && ConfigurationManager.AppSettings["COUNTRY"] != null && ConfigurationManager.AppSettings["COUNTRY"].ToString() == "AE") ? "R" : Session["TripType" + valkey].ToString();
                for (var i = 0; i < Adult_count; i++)
                {
                    string[] Passengerdetails = Adultnamedetails.Split('#');
                    string[] currentpaxdetails = Passengerdetails[i].Split('@');
                    TuneReqPassDetail _passdetails = new TuneReqPassDetail();
                    _passdetails.Firstname = currentpaxdetails[1].Split('~')[0];
                    _passdetails.Lastname = currentpaxdetails[1].Split('~')[1];
                    _passdetails.PaxType = "ADT";
                    _passdetails.Title = currentpaxdetails[0];
                    _passdetails.PAXTYPENO = "Adult" + (i + 1);
                    _passdetails.Gender = currentpaxdetails[1].Split('~')[2];
                    _passdetails.DOB = currentpaxdetails[1].Split('~')[3];
                    _passdetails.PassportNo = currentpaxdetails[1].Split('~')[4];
                    _passdetails.ExpiryDate = currentpaxdetails[1].Split('~')[5];
                    _passdetails.IssCountry = currentpaxdetails[1].Split('~')[6];
                    _passdet.Add(_passdetails);

                }
                if (Child_count > 0)
                {
                    for (var ch = 0; ch < Child_count; ch++)
                    {
                        string[] Passengerdetails = Childnamedetails.Split('#');
                        string[] currentpaxdetails = Passengerdetails[ch].Split('@');
                        TuneReqPassDetail _passdetails = new TuneReqPassDetail();
                        _passdetails.Firstname = currentpaxdetails[1].Split('~')[0];
                        _passdetails.Lastname = currentpaxdetails[1].Split('~')[1];
                        _passdetails.PaxType = "CHD";
                        _passdetails.Title = currentpaxdetails[0];
                        _passdetails.PAXTYPENO = "Child" + (ch + 1);
                        _passdetails.Gender = currentpaxdetails[1].Split('~')[2];
                        _passdetails.DOB = currentpaxdetails[1].Split('~')[3];
                        _passdetails.PassportNo = currentpaxdetails[1].Split('~')[4];
                        _passdetails.ExpiryDate = currentpaxdetails[1].Split('~')[5];
                        _passdetails.IssCountry = currentpaxdetails[1].Split('~')[6];


                        _passdet.Add(_passdetails);
                    }
                }
                if (Infant_count > 0)
                {
                    for (var inf = 0; inf < Infant_count; inf++)
                    {
                        string[] Passengerdetails = Infantnamedetails.Split('#');
                        string[] currentpaxdetails = Passengerdetails[inf].Split('@');
                        TuneReqPassDetail _passdetails = new TuneReqPassDetail();
                        _passdetails.Firstname = currentpaxdetails[1].Split('~')[0];
                        _passdetails.Lastname = currentpaxdetails[1].Split('~')[1];
                        _passdetails.PaxType = "INF";
                        _passdetails.Title = currentpaxdetails[0];
                        _passdetails.PAXTYPENO = "Infant" + (inf + 1);
                        _passdetails.Gender = currentpaxdetails[1].Split('~')[2];
                        _passdetails.DOB = currentpaxdetails[1].Split('~')[3];
                        _passdetails.PassportNo = currentpaxdetails[1].Split('~')[4];
                        _passdetails.ExpiryDate = currentpaxdetails[1].Split('~')[5];
                        _passdetails.IssCountry = currentpaxdetails[1].Split('~')[6];


                        _passdet.Add(_passdetails);
                    }
                }

                _seatreq.AgentDetail = _agentdet;
                _seatreq.FlightsDetails = _flightdet;
                // _seatreq.SegmentDetails = _flightsegment;
                _seatreq.Platform = "T";
                _seatreq.TripDetails = newtrip;
                _seatreq.PassengerDetails = _passdet;
                _seatreq.Stock = flitab.Rows[0]["stocktype"].ToString();

                string request = JsonConvert.SerializeObject(_seatreq).ToString();

                string StrSeatTrackID = "TUNEINSRESPONSE" + DateTime.Now.ToString("yyyyMMddHHmmssfff");

                DataSet str_TrackID = new DataSet();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                str_TrackID = _rays_servers.FetchSeatTrack(strAgentID, strTerminalId, strAppType, StrSeatTrackID, request, "I");

                string request1 = "Key=" + UrlEncode(StrSeatTrackID);

                if (request1 != null && request1 != "")
                {
                    array_Response[response] = request1;
                }
                string xml = "<EVENT><KEY>[<![CDATA[" + request1 + "]]>]</KEY></EVENT>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "Gettunemap", "Keyvalue", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            catch (Exception First_Ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Gettunemap", "Search Request", First_Ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                //return array_Response;
                return Json(new { Status = "", Message = "", Result = array_Response });
            }

            // return array_Response;
            return Json(new { Status = "", Message = "", Result = array_Response });

        }

        public string UrlEncode(string strUrlValue)
        {
            try
            {
                strUrlValue = strUrlValue.Replace("&quot;", "\"");
                strUrlValue = strUrlValue.Replace("%", "%25");
                strUrlValue = strUrlValue.Replace("=", "%3D");
                strUrlValue = strUrlValue.Replace("+", "%2B");
                strUrlValue = strUrlValue.Replace(" ", "+");
                strUrlValue = strUrlValue.Replace("?", "%3F");
                strUrlValue = strUrlValue.Replace("/", "%2F");
                strUrlValue = strUrlValue.Replace("|", "%7C");
                strUrlValue = strUrlValue.Replace(";", "%3B");
                strUrlValue = strUrlValue.Replace("!", "%21");
                strUrlValue = strUrlValue.Replace("\"", "%22");
                strUrlValue = strUrlValue.Replace("#", "%23");
                strUrlValue = strUrlValue.Replace("$", "%24");
                strUrlValue = strUrlValue.Replace("&", "%26");
                strUrlValue = strUrlValue.Replace("'", "%27");
                strUrlValue = strUrlValue.Replace("(", "%28");
                strUrlValue = strUrlValue.Replace(")", "%29");
                strUrlValue = strUrlValue.Replace("*", "%2A");
                strUrlValue = strUrlValue.Replace(",", "%2C");
                //strUrlValue = strUrlValue.Replace("-", "%2D");
                strUrlValue = strUrlValue.Replace(":", "%3A");
                strUrlValue = strUrlValue.Replace("<", "%3C");
                strUrlValue = strUrlValue.Replace("=", "%3D");
                strUrlValue = strUrlValue.Replace(">", "%3E");
                strUrlValue = strUrlValue.Replace("@", "%40");
                strUrlValue = strUrlValue.Replace("[", "%5B");
                strUrlValue = strUrlValue.Replace("\\", "%5C");
                strUrlValue = strUrlValue.Replace("]", "%5D");
                strUrlValue = strUrlValue.Replace("^", "%5E");
                strUrlValue = strUrlValue.Replace("{", "%7B");
                strUrlValue = strUrlValue.Replace("}", "%7D");
                strUrlValue = strUrlValue.Replace("~", "%7E");
                return strUrlValue;
            }
            catch
            {
                return strUrlValue;
            }
        }

        public ActionResult SeatMpas()
        {
            ViewBag.hdnurl = ConfigurationManager.AppSettings["SEATMAPURL"].ToString();
            try
            {
                string xml = "<EVENT><KEY>[<![CDATA[" + ConfigurationManager.AppSettings["SEATMAPURL"].ToString() + "]]>]</KEY><MAPURL>[<![CDATA[" + Request.QueryString["Key"] + "]]>]</MAPURL></EVENT>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "getseatmapPageLoad", "Seats", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                if (Request.QueryString["Key"] != null && Request.QueryString["Key"] != "")
                {
                    ViewBag.Key = Request.QueryString["Key"];
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "getseatmapPageLoad", "Seats", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                RedirectToAction("SessionExp", "Redirect");
            }
            return View();
        }

        private DataSet FetchSeatTrack(string ASM_AGENT_ID, string ASM_TERMINAL_ID, string ASM_TERMINAL_TYPE, string ASM_TRACKID, string ASM_FLIGHTDETAILS, string ASM_FLAG)//--? Not used any where
        {
            DataSet result = new DataSet();
            string Errormsg = string.Empty;
            try
            {
                STSTRAVRAYS.Rays_service.RaysService _rayservice = new STSTRAVRAYS.Rays_service.RaysService();
                //Hashtable hstParam = new Hashtable();
                ////  hstParam.Add("ASM_AGENT_ID", ASM_AGENT_ID);
                //hstParam.Add("ASM_TERMINAL_ID", ASM_TERMINAL_ID);
                //hstParam.Add("ASM_TERMINAL_TYPE", ASM_TERMINAL_TYPE);
                //hstParam.Add("ASM_TRACKID", ASM_TRACKID);
                //hstParam.Add("ASM_FLIGHTDETAILS ", ASM_FLIGHTDETAILS);
                //hstParam.Add("ASM_FLAG ", ASM_FLAG);
                //string strErrorMsg = string.Empty;
                //DataTable dsoutput = new DataTable();
                //string Result = string.Empty;
                //result = DBHandler.ExecSelectProcedure("P_INSERT_AIRLINE_SEAT_DETAILS_WEB", hstParam);

                result = _rayservice.FetchSeatTrack(ASM_AGENT_ID, ASM_TERMINAL_ID, ASM_TERMINAL_TYPE, ASM_TRACKID, ASM_FLIGHTDETAILS, ASM_FLAG);

                string xml = "<EVENT><ASM_AGENT_ID>" + ASM_AGENT_ID + "</ASM_AGENT_ID><ASM_TERMINAL_ID>" + ASM_TERMINAL_ID + "</ASM_TERMINAL_ID><ASM_TERMINAL_TYPE>" + ASM_TERMINAL_TYPE + "</ASM_TERMINAL_TYPE><ASM_TRACKID>" + ASM_TRACKID + "</ASM_TRACKID><ASM_FLIGHTDETAILS>" + ASM_FLIGHTDETAILS + "</ASM_FLIGHTDETAILS><ASM_FLAG>" + ASM_FLAG + "</ASM_FLAG></EVENT>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "SeatmapRequest", "InsertSeatmapRequst", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                return result;
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "SeatmapRequest", "InsertSeatmapRequst", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                return result;
            }
        }

        public ActionResult ResultSeatmap(string retval)//, string Valkey
        {
            ArrayList resultarr = new ArrayList();
            resultarr.Add("");
            resultarr.Add("");
            resultarr.Add("");
            resultarr.Add("");
            resultarr.Add("");

            int response = 1;
            int error = 0;
            int responseseatamount = 2;
            int segmentdetails = 3;
            int ignore = 4;
            double seatamount = 0;
            string resultdata = string.Empty;
            string segmentdetailsforseat = string.Empty;
            DataTable flighttab = new DataTable();
            string Valkey = Session["valkey"].ToString();
            try
            {
                string xml = "<EVENT><RESULT>[<![CDATA[" + retval + "]]>]</RESULT></EVENT>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "SeatmapResponse", " Result SeatmapResponse", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                // GetSeat_RS _seatres = JsonConvert.DeserializeObject<GetSeat_RS>(retval);
                RQRS.GetSeat_RS _seatres = JsonConvert.DeserializeObject<RQRS.GetSeat_RS>(retval);

                if (_seatres != null && _seatres.Resultcode == "1")
                {
                    flighttab = (DataTable)Session["Response" + Valkey];

                    string Sectordetails = string.Empty;
                    if (_seatres.SeatmapsDetails != null)
                    {
                        for (var i = 0; i < _seatres.SeatmapsDetails.Count; i++)
                        {
                            if (_seatres.SeatmapsDetails[i].SEATNAME != "" && _seatres.SeatmapsDetails[i].SEATAMOUNT != "")
                            {
                                Sectordetails = _seatres.SeatmapsDetails[i].SectorDet.Replace("$", "~");
                                resultdata += _seatres.SeatmapsDetails[i].PaxRefNo + "~" + _seatres.SeatmapsDetails[i].SegRefNo + "~" + _seatres.SeatmapsDetails[i].SEATNAME + "~" + _seatres.SeatmapsDetails[i].SEATAMOUNT + "~" + _seatres.SeatmapsDetails[i].PaxType + "~" + _seatres.SeatmapsDetails[i].SeatRef + "~" + Sectordetails + "~" + (_seatres.SeatmapsDetails[i].SeatrefAPI ?? "") + "$";
                                seatamount = Convert.ToDouble(seatamount) + Convert.ToDouble(_seatres.SeatmapsDetails[i].SEATAMOUNT);
                            }
                            else
                            {
                                resultdata += _seatres.SeatmapsDetails[i].PaxRefNo + "~" + _seatres.SeatmapsDetails[i].SegRefNo + "~" + "Not Selected" + "~" + "Not Selected" + "~" + _seatres.SeatmapsDetails[i].PaxType + "~" + _seatres.SeatmapsDetails[i].SeatRef + "$";
                            }
                        }
                    }
                    if (flighttab != null)
                    {
                        for (var row = 0; row < flighttab.Rows.Count; row++)
                        {
                            segmentdetailsforseat += flighttab.Rows[row]["Origin"] + "->" + flighttab.Rows[row]["Destination"] + "@";
                        }
                    }
                    resultarr[segmentdetails] = segmentdetailsforseat.TrimEnd('@');

                    string xmldata = "<EVENT><RESULT>[<![CDATA[" + resultdata + "]]>]</RESULT><SEATAMOUNT>" + seatamount + "</SEATAMOUNT></EVENT>";

                    DatabaseLog.LogData(Session["username"].ToString(), "E", "SeatmapResultData", " Result SeatmapData", xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());


                    if (resultdata != null && resultdata != "")
                    {
                        resultarr[response] = resultdata;
                        resultarr[responseseatamount] = seatamount.ToString();
                    }
                    else
                    {
                        resultarr[error] = "Unable to select seat";
                    }
                }
                else if (_seatres.Resultcode == "2")
                {
                    resultarr[ignore] = "IGNORE";
                }
                else if (_seatres.Resultcode == "0")
                {
                    if (_seatres.Error != null && _seatres.Error != "")
                    {
                        resultarr[error] = _seatres.Error;
                    }
                    else
                    {
                        resultarr[error] = "Unable to select seat";
                    }
                }
                else
                {
                    resultarr[error] = "Unable to select seat";
                }
            }
            catch (Exception ex)
            {

                if (ex.Message.ToString().ToUpper().Trim().Contains("THE OPERATION HAS TIMED OUT"))
                {
                    resultarr[error] = "THE OPERATION HAS TIMED OUT";
                }
                else if (ex.Message.ToString() == "Unable to connect the remote server")
                {
                    resultarr[error] = "Unable To Connect the Remote Server";
                }
                else
                {
                    resultarr[error] = "Problem occured While ResultSeatmap.Please contact support team (#05)";
                }

                DatabaseLog.LogData(Session["username"].ToString(), "X", "SeatResponse", "Search Seat", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                return Json(new { Status = "", Message = "", Result = resultarr });
            }
            return Json(new { Status = "01", Message = "", Result = resultarr });
            //return resultarr;
        }

        #endregion

        #region Get  FareRule
        public ActionResult GetFlightFareRule(string FlightFareRuleid, string FareRuleAvailstring, string Triptype, string Paxcount, string Pnrdetails, string Cabin, string Segmenttype)
        {
            ArrayList array_FlightRule = new ArrayList(2);
            array_FlightRule.Add("");
            array_FlightRule.Add("");

            int error = 0;
            int response = 1;
            string AirlineCategory = string.Empty;
            string strResponse = string.Empty;
            string Stock = string.Empty;
            string strTstFlg = string.Empty;
            string Origin = string.Empty;
            string Destination = string.Empty;
            string DepartureDateTime = string.Empty;
            string ReferenceToken = string.Empty;
            string FlightNumber = string.Empty;
            string PlatingCarrier = string.Empty;
            string ArrivalDateTime = string.Empty;
            string Class = string.Empty;
            string BaseOrg = string.Empty;
            string Basedes = string.Empty;
            string Faretype = string.Empty;
            string rulesPath = string.Empty;
            string rulesPathDefault = string.Empty;
            string strErrorMsg = string.Empty;
            string strResult = string.Empty;
            string strTerminalType = string.Empty;
            string strAgentType = string.Empty;
            string strClientType = string.Empty;
            DataSet dsFareRule = new DataSet();
            DataTable dtFlightList = new DataTable();
            StringWriter strWriter = new StringWriter();
            DataSet dsrequest = new DataSet();
            DataSet dsFlightDetails = new DataSet();

            RQRS_ancillary.FareRuleRQ _FareRuleRQ = new RQRS_ancillary.FareRuleRQ();
            RQRS_ancillary.AgentDetails Agent = new RQRS_ancillary.AgentDetails();
            RQRS_ancillary.RQFlights _Flts = new RQRS_ancillary.RQFlights();
            List<RQRS_ancillary.RQFlights> _lstFlts = new List<RQRS_ancillary.RQFlights>();
            RQRS_ancillary.FareRuleRS _FareRuleRS = new RQRS_ancillary.FareRuleRS();

            try
            {
                #region UsageLog
                string PageName = "Fare Rule";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion

                strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
                strClientType = Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : "";

                Agent.AgentID = Session["POS_ID"].ToString();
                Agent.AgentType = strTerminalType == "T" ? strClientType : strAgentType;
                Agent.Airportid = (Segmenttype != null && Segmenttype.ToString() != "" ? Segmenttype : "I");
                Agent.AppType = "B2B";
                Agent.BOAID = Session["POS_ID"].ToString();
                Agent.BOATreminalID = Session["POS_TID"].ToString();
                Agent.BranchID = Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "";
                Agent.ClientID = Session["POS_ID"].ToString();
                Agent.CoOrdinatorID = "";
                Agent.Environment = "W";
                Agent.TerminalID = Session["POS_TID"].ToString();
                Agent.UserName = Session["username"].ToString();
                Agent.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                Agent.ProjectId = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"].ToString() != "" ? Session["PRODUCT_CODE"].ToString() : "";
                Agent.APPCurrency = Session["App_currency"] != null && Session["App_currency"].ToString() != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                Agent.Platform = "B";
                Agent.ProductID = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"].ToString() != "" ? Session["PRODUCT_CODE"].ToString() : "";

                if (FareRuleAvailstring.Contains("SpLITSaTIS"))
                {
                    var dddd = FareRuleAvailstring.Substring(0, 10);
                    string[] FlightCount = Regex.Split(FareRuleAvailstring, "SpLITSaTIS");

                    for (var i = 0; i < FlightCount.Count(); i++)
                    {
                        if (FlightCount[i] != "" && FlightCount[i].Contains("SpLitPResna"))
                        {

                            _Flts = new RQRS_ancillary.RQFlights();
                            _Flts.AirlineCategory = Regex.Split(FlightCount[i], "SpLitPResna")[16];
                            _Flts.Origin = Regex.Split(FlightCount[i], "SpLitPResna")[0];
                            _Flts.Destination = Regex.Split(FlightCount[i], "SpLitPResna")[1]; 
                            _Flts.DepartureDateTime = Regex.Split(FlightCount[i], "SpLitPResna")[2];
                            _Flts.ReferenceToken = Regex.Split(FlightCount[i], "SpLitPResna")[12]; 
                            _Flts.FlightNumber = Regex.Split(FlightCount[i], "SpLitPResna")[18];
                            _Flts.PlatingCarrier = Regex.Split(FlightCount[i], "SpLitPResna")[9];
                            _Flts.ArrivalDateTime = Regex.Split(FlightCount[i], "SpLitPResna")[3];
                            _Flts.Class = Regex.Split(FlightCount[i], "SpLitPResna")[5];
                            _Flts.FareBasisCode = Regex.Split(FlightCount[i], "SpLitPResna")[17];
                            _Flts.FareID = Regex.Split(FlightCount[i], "SpLitPResna")[25];
                            _Flts.ItinRef = Regex.Split(FlightCount[i], "SpLitPResna")[19];
                            _Flts.ReferenceToken = Regex.Split(FlightCount[i], "SpLitPResna")[12];
                            _Flts.SegRef = Regex.Split(FlightCount[i], "SpLitPResna")[29];
                            _Flts.CarrierCode = Regex.Split(FlightCount[i], "SpLitPResna")[9];
                            _Flts.Cabin = Cabin;
                            _Flts.SeatAvailFlag = "";
                            _Flts.PromoCode = "";
                            _Flts.StartTerminal = "";
                            _Flts.EndTerminal = "";
                            _Flts.FareType = "";

                            Faretype = Regex.Split(FlightCount[i], "SpLitPResna")[34];
                            Stock = Regex.Split(FlightCount[i], "SpLitPResna")[23];

                            _lstFlts.Add(_Flts);
                            if (BaseOrg == "")
                            {
                                BaseOrg = Regex.Split(FlightCount[i], "SpLitPResna")[0];
                            }
                            if (Regex.Split(FlightCount[i], "SpLitPResna")[19] == "0")
                            {
                                Basedes = Regex.Split(FlightCount[i], "SpLitPResna")[1];
                            }

                        }
                    }
                }
                else
                {
                    string[] SplitAirCategory = Regex.Split(FareRuleAvailstring, "SpLitPResna");

                    _Flts = new RQRS_ancillary.RQFlights();
                    _Flts.AirlineCategory = SplitAirCategory[16];
                    _Flts.Origin = SplitAirCategory[0].Trim();
                    _Flts.Destination = SplitAirCategory[1].Trim();
                    _Flts.DepartureDateTime = SplitAirCategory[2];
                    _Flts.ReferenceToken = SplitAirCategory[12];
                    _Flts.FlightNumber = SplitAirCategory[18];
                    _Flts.PlatingCarrier = SplitAirCategory[9];
                    _Flts.ArrivalDateTime = SplitAirCategory[3];
                    _Flts.Class = SplitAirCategory[5];
                    _Flts.CarrierCode = SplitAirCategory[9];
                    _Flts.Cabin = Cabin;
                    _Flts.FareType = "";
                    _Flts.SeatAvailFlag = "";
                    _Flts.PromoCode = "";
                    _Flts.StartTerminal = "";
                    _Flts.EndTerminal = "";
                    _Flts.FareBasisCode = SplitAirCategory[17];
                    _Flts.FareID = SplitAirCategory[25];
                    _Flts.ItinRef = SplitAirCategory[19];
                    _Flts.ReferenceToken = SplitAirCategory[12];
                    _Flts.SegRef = SplitAirCategory[29];

                    Stock = SplitAirCategory[23];
                    BaseOrg = SplitAirCategory[0];
                    Basedes = SplitAirCategory[1];
                    Faretype = SplitAirCategory[34];

                    _lstFlts.Add(_Flts);
                }

                string Triptypeflag = (Triptype == "Y" ? "R" : (Triptype == "R" ? "O" : Triptype == null ? "O" : Triptype));
                RQRS_ancillary.Segment Segment = new RQRS_ancillary.Segment();
                Segment.BaseOrigin = BaseOrg.Trim();
                Segment.BaseDestination = Basedes.Trim();
                if (Paxcount != null && Paxcount != "")
                {
                    Segment.Adult = (Paxcount.Contains('|') ? (Paxcount.Split('|').Length > 1 ? Convert.ToInt32(Paxcount.Split('|')[0]) : 1) : 1);
                    Segment.Child = (Paxcount.Contains('|') ? (Paxcount.Split('|').Length > 1 ? Convert.ToInt32(Paxcount.Split('|')[1]) : 0) : 0);
                    Segment.Infant = (Paxcount.Contains('|') ? (Paxcount.Split('|').Length > 1 ? Convert.ToInt32(Paxcount.Split('|')[2]) : 0) : 0);
                }
                else
                {
                    Segment.Adult = 1;
                    Segment.Child = 0;
                    Segment.Infant = 0;
                }
                Segment.SegmentType = (Segmenttype != null && Segmenttype.ToString() != "" ? Segmenttype : "I");
                Segment.TripType = Triptypeflag;


                string[] SplitFlightNumber = FlightFareRuleid.Split(' ');
                string SFCthread = Session["SFCthread"].ToString();
                
                #region For FareRule............
                _FareRuleRQ.FlightsDetails = _lstFlts;
                _FareRuleRQ.SegmentDetails = Segment;
                _FareRuleRQ.Stock = Stock;
                if (Pnrdetails != null && Pnrdetails != "")
                {
                    _FareRuleRQ.AirlinePNR = (Pnrdetails.Contains('~') ? (Pnrdetails.Split('~').Length > 1 ? Pnrdetails.Split('~')[1] : "") : "");
                    _FareRuleRQ.CRSPNR = (Pnrdetails.Contains('~') ? (Pnrdetails.Split('~').Length > 1 ? Pnrdetails.Split('~')[2] : "") : "");
                }
                else
                {
                    _FareRuleRQ.AirlinePNR = "";
                    _FareRuleRQ.CRSPNR = "";
                }
                _FareRuleRQ.FareType = Faretype;
                _FareRuleRQ.FetchType = "";
                _FareRuleRQ.TicketNo = "";
                _FareRuleRQ.CRSID = "";
                _FareRuleRQ.AgentDetail = Agent;
                #endregion

                #region Request.......
                try
                {
                    string request = JsonConvert.SerializeObject(_FareRuleRQ).ToString();
                    string Query = "GetFareRule";
                    MyWebClient client = new MyWebClient();
                    client.LintTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                    client.Headers["Content-type"] = "application/json";
                    string FarerulPath = (ConfigurationManager.AppSettings["FareRuleAncillaryURL"].ToString() == "") ? strURLpath : ConfigurationManager.AppSettings["FareRuleAncillaryURL"].ToString();

                    #region Log
                    dsrequest = Serv.convertJsonStringToDataSet(request, "");
                    dsrequest.WriteXml(strWriter);

                    string LstrDetails = "<GET_FARE_RULE_REQUEST><URL>[<![CDATA[" + FarerulPath + "]]>]</URL><QUERY>" + Query + "</QUERY><TIME>"
                        + "</TIME>" + strWriter.ToString() + "</GET_FARE_RULE_REQUEST>";


                    DatabaseLog.LogData(Session["username"].ToString(), "F", "flightrule-requst", "GetFlighFareRule", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    #endregion

                    byte[] data = client.UploadData(FarerulPath + Query, "POST", Encoding.ASCII.GetBytes(request));

                    strResponse = Encoding.ASCII.GetString(data);

                    #region Log
                    string ResTime = "AvailabityReqest" + DateTime.Now;
                    LstrDetails = "<GET_FARE_RULE_RESPONSE><AIRPORTCATEGORY>" + strResponse + "</AIRPORTCATEGORY><URL>[<![CDATA[" + strURLpath + "/Rays.svc/"
                       + "]]>]</URL><QUERY>" + Query + "</QUERY><RESPONSE_TIME>" + ResTime + "</RESPONSE_TIME></GET_FARE_RULE_RESPONSE>";

                    DatabaseLog.LogData(Session["username"].ToString(), "F", "Flightrule-response", "GetFlighFareRule ", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    #endregion

                    if (string.IsNullOrEmpty(strResponse))
                    {
                        array_FlightRule[error] = "Unable to get fare rule details";
                    }
                    else
                    {
                        _FareRuleRS = JsonConvert.DeserializeObject<RQRS_ancillary.FareRuleRS>(strResponse);
                        if (_FareRuleRS.Status.ResultCode == "1")
                        {
                            array_FlightRule[response] = _FareRuleRS.FareRule.FareRuleText != null && _FareRuleRS.FareRule.FareRuleText != "" ? _FareRuleRS.FareRule.FareRuleText : "Unable to get fare rule details";
                        }
                        else
                        {
                            array_FlightRule[error] = _FareRuleRS.Status.Error != null && _FareRuleRS.Status.Error != "" ? _FareRuleRS.Status.Error : "Unable to get fare rule details";
                        }
                    }

                }
                catch (Exception ex)
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "FareRuleViewer", "GetFareRule", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    array_FlightRule[error] = "Problem Occured. Please contact customercare";
                }
                #endregion
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "FareRuleViewer", "GetFareRule", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                if (ex.Message.ToString().ToUpper().Contains("TIMED OUT"))
                    array_FlightRule[error] = ex.Message.ToString();
                else
                    array_FlightRule[error] = "Problem Occured while fetching fare rule";
            }
            return Json(new { Status = "", Message = "", Result = array_FlightRule });
        }
        #endregion

        #region flight mail
        public ActionResult Mail_flights(string Toadd, string Subject, string Flights, string Origin, string Destination, string Frommail, string segloopcnt, string strhdr, string Changedfare)
        {
            string stu = string.Empty;
            string msg = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            int response = 0;
            int error = 1;
            string xml = string.Empty;
            string strPOS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPOS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strUsername = Session["username"] != null ? Session["username"].ToString() : "";
            string strIPAddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string strSequenceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string strTerminalType = Session["TerminalType"] != null ? Session["TerminalType"].ToString() : "";
            string port = ConfigurationManager.AppSettings["PortNo"].ToString();
            string Hostadd = ConfigurationManager.AppSettings["HostAddress"].ToString();
            string mailusername = ConfigurationManager.AppSettings["MailUsername"].ToString();
            string mailpassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
            try
            {
                if (Session["agentid"] == null)
                {
                    stu = "-1";
                    msg = "Session Expired.";
                    return Json(new { Status = stu, Message = msg, Result = "" });
                }
                DatabaseLog.LogData(Session["username"].ToString(), "E", "Email Avail", "Email Avail Send Request", Origin.ToString() + "|" + Destination, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                string output = string.Empty;
                //string MailFrom = Session["mailfromadd"].ToString();// ConfigurationManager.AppSettings["EnableSsl"].ToString();
                string MailFrom = Frommail; //Flights

                #region CmdbyRajesh


                StringBuilder Strflights = new StringBuilder();

                string[] FlightsDetails = Regex.Split(Flights, "SpRaj");
                string[] AgencyName = Session["AgencyDetailsformail"].ToString().Split('~');
                string[] Gross = Regex.Split(Changedfare, "SplitFare");
                string strxmlReq = "<REQUEST>" + Origin.ToString() + "|" + Destination + "</REQUEST><AGENCY>" + JsonConvert.SerializeObject(AgencyName) + "</AGENCY><Gross>" + JsonConvert.SerializeObject(Gross) + "</Gross><FlightsDetails>" + JsonConvert.SerializeObject(FlightsDetails) + "</FlightsDetails>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "Email sundar", "Email sundar req", strxmlReq, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                int GFare = 0;
                Strflights.Append("<div>");
                if (AgencyName.Length > 3)
                {
                    Strflights.Append("<table style='float:left'>");
                    Strflights.Append("<tr><td style='text-align:left;font-weight: bold;'>" + (!string.IsNullOrEmpty(AgencyName[0]) ? AgencyName[0] : "") + "</td></tr>");
                    Strflights.Append("<tr><td style='text-align:left;'>" + (!string.IsNullOrEmpty(AgencyName[2]) ? AgencyName[2] : "") + "</td></tr>");
                    Strflights.Append("<tr><td style='text-align:left;'>Phone No. : " + (!string.IsNullOrEmpty(AgencyName[3]) ? AgencyName[3] : "") + "</td></tr>");
                    Strflights.Append("<tr><td style='text-align:left;'>Email : " + (!string.IsNullOrEmpty(AgencyName[1]) ? AgencyName[1] : "") + "</td></tr>");
                    Strflights.Append("</table><br />");
                }
                Strflights.Append("<br /><table border='1' style='width:100%;font-family:Verdana;font-size:12px;border-collapse:collapse'>");
                Strflights.Append("<tr style='background-color:#fc8727;color:#fff;'><td colspan='8' style='text-align: center'><lable style='font-size:16px;'><b>" + Origin + "</b>&nbsp;To&nbsp;<b>" + Destination + "</b></lable></td></tr>");
                Strflights.Append("<tr style='background-color:#c6e4ff'><th style='text-align:center'>Airline</th><th style='text-align:center'>Departure</th><th style='text-align:center'>Arrival</th><th style='text-align:center'>Duration</th><th style='text-align:center'>Class</th><th style='text-align:center'>Basic&nbsp;Fare</th><th style='text-align:center'>Gross</th><th  style='text-align:center'>Baggage</th></tr>");
                for (int i = 0; i < FlightsDetails.Length - 1; i++)
                {
                    if (FlightsDetails[i] != "")
                    {
                        if (FlightsDetails[i].Contains("SpLITSaTIS"))
                        {
                            string[] Connflight = Regex.Split(FlightsDetails[i], "SpLITSaTIS");// FlightsDetails[i].Split('>');
                            int totalduration = 0;
                            for (int _dur = 0; _dur < Connflight.Length; _dur++)
                            {
                                string[] flidetails = Regex.Split(Connflight[_dur], "SpLitPResna");
                                if (flidetails[22] != null && flidetails[22] != "")
                                {
                                    totalduration += Convert.ToInt32(flidetails[22]);
                                }
                            }
                            for (int j = 0; j < Connflight.Length; j++)
                            {
                                string[] flidetails = Regex.Split(Connflight[j], "SpLitPResna");

                                Strflights.Append("<tr>");
                                Strflights.Append("<td style='text-align:center'>" + flidetails[18] + "<br /><img src='" + ConfigurationManager.AppSettings["FlightUrl"] + Regex.Split(FlightsDetails[i], "SpLitPResna")[9].ToString() + ".png" + "'><br />" + Base.Utilities.AirlineName(Regex.Split(FlightsDetails[i], "SpLitPResna")[9].ToString()) + " </td>");
                                Strflights.Append("<td style='text-align:center'>" + Base.Utilities.AirportcityName(flidetails[0].ToString()) + "<br>" + flidetails[2] + "</td>");
                                Strflights.Append("<td style='text-align:center'>" + Base.Utilities.AirportcityName(flidetails[1].ToString()) + "<br>" + flidetails[3] + "</td>");
                                if (j == 0)
                                {
                                    Strflights.Append("<td style='text-align:center;vertical-align:central' rowspan='" + Connflight.Length + "'>" + (totalduration / 60).ToString("00") + "h:" + (totalduration % 60).ToString("00") + "m" + "</td>");
                                }
                                Strflights.Append("<td style='text-align:center'>" + flidetails[5] + "-" + flidetails[17] + "</td>");
                                if (j == 0)
                                {
                                    Strflights.Append("<td style='text-align:center;vertical-align:central' rowspan='" + Connflight.Length + "'>" + Regex.Split(FlightsDetails[i], "SpLitPResna")[13] + "</td>");
                                    Strflights.Append("<td style='text-align:center;vertical-align:central' rowspan='" + Connflight.Length + "'>" + Gross[i] + "</td>");
                                }
                                Strflights.Append("<td style='text-align:center;vertical-align:central'>" + (string.IsNullOrEmpty((flidetails[24].Split('\n')[4]).Split(':')[1]) ? "N?A" : (flidetails[24].Split('\n')[4]).Split(':')[1]) + "</td>");
                                Strflights.Append("</tr>");
                            }

                        }
                        else
                        {
                            string[] flightdetais = Regex.Split(FlightsDetails[i], "SpLitPResna");

                            Strflights.Append("<tr>");
                            Strflights.Append("<td style='text-align:center'>" + flightdetais[18] + "<br /><img src='" + ConfigurationManager.AppSettings["FlightUrl"] + flightdetais[9].ToString() + ".png" + "'><br />" + Base.Utilities.AirlineName(flightdetais[9].ToString()) + "</td>");
                            Strflights.Append("<td style='text-align:center'>" + Base.Utilities.AirportcityName(flightdetais[0].ToString()) + "<br />" + flightdetais[2] + "</td>");
                            Strflights.Append("<td style='text-align:center'>" + Base.Utilities.AirportcityName(flightdetais[1].ToString()) + "<br />" + flightdetais[3] + "</td>");
                            if (flightdetais[4].ToString() != null || flightdetais[4] != "")
                            {
                                Strflights.Append("<td style='text-align:center'>" + (Convert.ToInt32(flightdetais[4]) / 60).ToString("00") + "h:" + (Convert.ToInt32(flightdetais[4]) % 60).ToString("00") + "m" + "</td>");
                            }
                            else
                            {
                                Strflights.Append("<td style='text-align:center'>" + flightdetais[4] + "</td>");
                            }
                            Strflights.Append("<td style='text-align:center'>" + flightdetais[5] + "-" + flightdetais[17] + "</td>");
                            Strflights.Append("<td style='text-align:center'>" + flightdetais[13] + "</td>");
                            Strflights.Append("<td style='text-align:center'>" + Gross[GFare] + "</td>");
                            Strflights.Append("<td style='text-align:center;vertical-align:central'>" + (string.IsNullOrEmpty((flightdetais[24].Split('\n')[4]).Split(':')[1]) ? "N/A" : (flightdetais[24].Split('\n')[4]).Split(':')[1]) + "</td>");
                            Strflights.Append("</tr>");
                            GFare++;
                        }
                    }
                }

                Strflights.Append("</table></div>");
                Strflights.Append("<div style='float:right'><label style='color:red;font-size:13px'>*All Fares in:" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + "</label></div>");
                string Flights_table = Strflights.ToString();
                DatabaseLog.LogData(Session["username"].ToString(), "E", "Email sundar", "Email sundar Res", Flights_table, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                #endregion

                #region UsageLog
                string PageName = "Availability Mail Send";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "SEND");
                }
                catch (Exception e) { }
                #endregion
                if (Flights.ToString() != "" && Toadd.ToString() != "")
                {
                    bool ssl = ConfigurationManager.AppSettings["EnableSsl"].ToString() == "false" ? false : true;
                    Mailref.MyService pp = new Mailref.MyService();
                    pp.Url = ConfigurationManager.AppSettings["mailurl"].ToString();

                    output = pp.SendAvailabilityMail(strPOS_ID, strPOS_TID, strUsername, strIPAddress, strTerminalType, strSequenceID,
                        MailFrom, "", Toadd, Subject, "", "", Flights_table, mailusername, mailpassword, Hostadd, port, ssl);
                    xml = "<EVENT><FROMMAIL>" + Toadd + "</FROMMAIL>" + "<agentid>" + strPOS_ID + "</agentid>" + "<terminalid>" + strPOS_TID + "</terminalid>"
                                + "<username>" + strUsername + "</username>" + "<ipAddress>" + strIPAddress + "</ipAddress>"
                                + "<sequenceid>" + strSequenceID + "</sequenceid>" + "<MailUsername>" + mailusername + "</MailUsername>"
                                + "<MailPassword>" + mailpassword + "</MailPassword>" + "<HostAddress>" + Hostadd + "</HostAddress>"
                                + "<MailPort>" + port + "</MailPort>" + "<ssl>" + ssl + "</ssl>" + "<MailFrom>" + MailFrom + "</MailFrom>"
                                + "<OUTPUT>" + output + "</OUTPUT></RESPONSE></EVENT>";

                    if (output.ToUpper() == "MAIL SENT" || output.ToUpper() == "SUCCESS")
                    {
                        stu = "01";
                        msg = "Your mail has been sent successfully.";
                        Array_Book[response] = msg;
                        DatabaseLog.LogData(Session["username"].ToString(), "S", "Email Avail", "Email Avail Send event", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                    }
                    else
                    {
                        stu = "00";
                        msg = "Mail sending failure. Please contact customer care (#03).";
                        DatabaseLog.LogData(Session["username"].ToString(), "S", "Email Avail", "Email Avail Send event", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                        Array_Book[error] = msg;
                    }
                }
                else
                {
                    xml = "<FLIGHTS>" + Flights + "</FLIGHTS><MAILID>" + Toadd + "</MAILID>";
                    stu = "00";
                    msg = "Mail sending failure. Please contact customer care (#03).";
                    DatabaseLog.LogData(Session["username"].ToString(), "S", "Email Avail", "Email Avail Send event", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                    Array_Book[error] = msg;
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Mail sending failure. Please contact customer care (#05).";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Email Avail", "Email Avail Send event", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                //DatabaseLog.LogData(Session["username"].ToString(), "X", "Email Avail", "Email Avail Send event", ex.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
                Array_Book[error] = msg;
            }
            //return Array_Book;
            return Json(new { Status = stu, Message = msg, Result = Array_Book });
        }

        #endregion

        #region Whats app message
        public ActionResult Whatsappmessage(string Whatsappno, string Flightdet)
        {
            string stu = string.Empty;
            string msg = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            int response = 0;
            int error = 1;
            int mobno = 2;
            string xml = string.Empty;
            string bindvalues = string.Empty;
            try
            {
                if (Session["agentid"] == null)
                {
                    stu = "-1";
                    msg = "Session Expired.";
                    return Json(new { Status = stu, Message = msg, Result = "" });
                }

                string XML = "<EVENT><WHATSAPPNO>" + Whatsappno + "</WHATSAPPNO><FLIGHTDET>" + Flightdet + "</FLIGHTDET></EVENT>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "whats app message", "whats app message avail send request", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                string output = string.Empty;

                StringBuilder stb = new StringBuilder();
                stb.Append(Flightdet);

                string[] Splitmultiple = Regex.Split(Flightdet, "SpliTwHatsaPP");

                for (int i = 0; i < Splitmultiple.Count(); i++)
                {
                    if (Splitmultiple[i] != "")
                    {
                        string[] splitvalues = Regex.Split(Splitmultiple[i], "SpLiTRaJ");
                        string Agentmob = Session["agencyaddress"].ToString().Split('~')[4];
                        string agentemail = Session["agencyaddress"].ToString().Split('~')[2];
                        string AgentContact = Session["agencyaddress"].ToString().Split('~')[3];

                        bindvalues += Uri.EscapeDataString("*_Your Flight Information_* ?? \r\n✈ Flight No : *" + splitvalues[0] + "* \r\nSector : *" + splitvalues[1] +
                                            (splitvalues[2] != "" ? splitvalues[2] : "") + splitvalues[3] + "* \r\n Dept ⏰: *" + splitvalues[4] + "* \r\n Arr ⏰: *" + splitvalues[5] +
                                            "* \r\n Class: *" + splitvalues[6] + "* \r\n Baggage: *" + splitvalues[7].TrimStart() + "* \r\n Gross Fare: *" + splitvalues[8] + "* \r\n Fare Type: *"
                                            + splitvalues[9] + "* \r\n Trip Type: *" + (splitvalues[10] == "O" ? "Oneway" : "Roundtrip") + "*\r\n--\r\n" +
                                            "Thank you for choosing *" + Session["agencyname"].ToString() + ".* \r\n" + "In case of any urgent support ? \r\n *Contact " + "☎ : "
                                            + AgentContact + "*\r\n" + "*Mobile " + "? : " + Agentmob + "*\r\n" + "*Email " + "? : " + agentemail + "* \r\n---------------------------------------\r\n");
                    }
                }
                Array_Book[response] = bindvalues;
                Array_Book[mobno] = Whatsappno;

            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Mail sending failure. Please contact customer care (#05).";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Email Avail", "Email Avail Send event", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                //DatabaseLog.LogData(Session["username"].ToString(), "X", "Email Avail", "Email Avail Send event", ex.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
                Array_Book[error] = msg;
            }
            return Json(new { Status = stu, Message = msg, Result = Array_Book });
        }

        #endregion

        public ActionResult Whatsappmessagenew(string Whatsappno, string Flightdet, string Allowfare, string Origin, string Destination, string Triptype, string Foundfaretype)
        {
            string stu = string.Empty;
            string msg = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            int response = 0;
            int error = 1;
            int mobno = 2;
            string xml = string.Empty;
            string bindvalues = string.Empty;
            try
            {
                if (Session["agentid"] == null)
                {
                    stu = "-1";
                    msg = "Session Expired.";
                    return Json(new { Status = stu, Message = msg, Result = "" });
                }
                #region UsageLog
                string PageName = "Whatsapp FlightShare";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "SEND");
                }
                catch (Exception e) { }
                #endregion

                string XML = "<EVENT><WHATSAPPNO>" + Whatsappno + "</WHATSAPPNO><FLIGHTDET>" + Flightdet + "</FLIGHTDET></EVENT>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "whats app message", "whats app message avail send request", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                string output = string.Empty;

                StringBuilder stb = new StringBuilder();
                stb.Append(Flightdet);
                string Agentmob = Session["agencyaddress"].ToString().Split('~')[4];
                string agentemail = Session["agencyaddress"].ToString().Split('~')[2];
                string AgentContact = Session["agencyaddress"].ToString().Split('~')[3];
                string Sectordetails = (Triptype == "Y" ? Origin + " - " + Destination + " - " + Origin : Origin + " - " + Destination);
                string[] Splitmultiple = Regex.Split(Flightdet, "SpliTwHatsaPP");
                bindvalues += "*_Your Flight Information_ for* *" + Sectordetails + "* ??";
                for (int i = 0; i < Splitmultiple.Count(); i++)
                {
                    if (Splitmultiple[i] != "")
                    {
                        if (Splitmultiple[i].Contains("SplItFliGth") == true)
                        {

                            string[] Splitdetails = Regex.Split(Splitmultiple[i], "SplItFliGth");
                            for (int j = 0; j < Splitdetails.Count(); j++)
                            {
                                if (Splitdetails[j] != "")
                                {
                                    string[] splitvalues = Regex.Split(Splitdetails[j], "SpLiTRaJ");
                                    if (j == 0)
                                    {
                                        //bindvalues += (i + 1) + " ✈ *" + splitvalues[0] + "*\t*" + splitvalues[1] + "*\t*" + splitvalues[2] + "*\t*" + splitvalues[3].TrimStart() + (Allowfare == "YES" ? "*\t*" + splitvalues[4] + "*\t*" + splitvalues[5] + "*\r\n\r\n" : "*\r\n\r\n");
                                        bindvalues += "\r\n\r\n" + (i + 1) + " ✈ *" + splitvalues[0] + "*" + // Flight No
                                                        ((splitvalues[0].Split(' ')[1]).ToString().Length == 2 ? "      " : ((splitvalues[0].Split(' ')[1]).ToString().Length == 3 ? "    " : "   "))
                                                         + splitvalues[6].TrimEnd() + " - " + splitvalues[7].TrimEnd() + "  "
                                                        + (splitvalues[1].Substring(0, 7) + //Dept Date
                                                        " *" + splitvalues[1].Split(' ')[3]) + "*  " + //Dept Time
                                                          (splitvalues[2].Substring(0, 7) + //Arr Date
                                                        " *" + splitvalues[2].Split(' ')[3]) + "*  *" + //Arr Time
                                                        splitvalues[3].TrimStart().Replace("HB", "").Trim().Replace(" ", "") + "*" +//Baggage
                                                       (splitvalues[3].TrimStart().Replace("HB", "").Trim().Replace(" ", "").ToString().Length == 4 ? "   " : "    ") +
                                                        (Allowfare == "YES" ? "*" + splitvalues[4] + //Refundable
                                                        "*  *" + Session["App_currency"].ToString() + "*  " +
                                                        (splitvalues[5].Replace("N/A", "").Trim() == "" ? "" :
                                                        ("*" + splitvalues[5].Replace("N/A", "").Trim() + "*")) : "");
                                    }
                                    else
                                    {
                                        //bindvalues += " ✈ *" + splitvalues[0] + "*\t*" + splitvalues[1] + "*\t*" + splitvalues[2] + "*\t*" + splitvalues[3].TrimStart() + "*\r\n\r\n";

                                        bindvalues += "\r\n   ✈ *" + splitvalues[0] + "*" + // Flight No
                                                        ((splitvalues[0].Split(' ')[1]).ToString().Length == 2 ? "     " : ((splitvalues[0].Split(' ')[1]).ToString().Length == 3 ? "   " : "  "))
                                                       + splitvalues[6].TrimEnd() + " - " + splitvalues[7].TrimEnd() + "  "
                                                        + (splitvalues[1].Substring(0, 7) + //Dept Date
                                                       " *" + splitvalues[1].Split(' ')[3]) + "*  " + //Dept Time
                                                        (splitvalues[2].Substring(0, 7) + //Arr Date
                                                       " *" + splitvalues[2].Split(' ')[3]) + "*  *" + //Arr Time
                                                       splitvalues[3].TrimStart().Replace("HB", "").Trim().Replace(" ", "") + "*\r\n\r\n";//Baggage
                                        //" \t" + splitvalues[4].Replace("N/A", "").Trim() + //Refundable
                                        //(Allowfare == "YES" ? "*\t*" + splitvalues[5] + "*\r\n\r\n" : "*\r\n\r\n");
                                    }
                                }
                            }

                        }
                        else
                        {
                            string[] splitvalues = Regex.Split(Splitmultiple[i], "SpLiTRaJ");
                            bindvalues += "\r\n\r\n" + (i + 1) + " ✈ *" + splitvalues[0] + "*" +// Flight No
                                ((splitvalues[0].Split(' ')[1]).ToString().Length == 2 ? "      " : ((splitvalues[0].Split(' ')[1]).ToString().Length == 3 ? "    " : "   "))
                                + splitvalues[6].TrimEnd() + " - " + splitvalues[7].TrimEnd() + "  "
                                + (splitvalues[1].Substring(0, 7) + //Dept Date
                                " *" + splitvalues[1].Split(' ')[3]) + "*  " + //Dept Time
                                  (splitvalues[2].Substring(0, 7) + //Arr Date
                                " *" + splitvalues[2].Split(' ')[3]) + "*  *" + //Arr Time
                                splitvalues[3].TrimStart().Replace("HB", "").Trim().Replace(" ", "") + "*" +//Baggage
                                (splitvalues[3].TrimStart().Replace("HB", "").Trim().Replace(" ", "").ToString().Length == 4 ? "   " : "    ") +
                                 (Allowfare == "YES" ? "*" + splitvalues[4] + "* *" + Session["App_currency"].ToString() + "*  " +
                                 (splitvalues[5].Replace("N/A", "").Trim() == "" ? "" : ("*" + splitvalues[5].Replace("N/A", "").Trim() + "*")) : "");
                        }
                    }
                }

                if (Foundfaretype == "1" && Allowfare == "YES")
                {
                    //bindvalues += "\r\n---\r\n_Note:_\r\nAll Fares In *(" + Session["App_currency"].ToString() + ")*\r\n\r\n";
                    bindvalues += "*R* --> Refundable\r\n*NR* --> Non Refundable";
                }
                bindvalues += "\r\n\r\nThank you for choosing *" + Session["agencyname"].ToString() + ".* \r\n" + "In case of any urgent support ? \r\n *Contact " + "☎ : " + AgentContact + "*\r\n" + "*Mobile " + "? : " + Agentmob + "*\r\n" + "*Email " + "? : " + agentemail + "*";
                string Senddetails = Uri.EscapeDataString(bindvalues);

                Array_Book[response] = Senddetails;
                Array_Book[mobno] = Whatsappno;

            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Mail sending failure. Please contact customer care (#05).";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Email Avail", "Email Avail Send event", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                Array_Book[error] = msg;
            }
            return Json(new { Status = stu, Message = msg, Result = Array_Book });
        }

        #region insurance tune mail
        public ActionResult Mail_insurance(string Toadd, string Subject, string Frommail, string Insurancedet, string insurancemail_bookingdata)
        {
            string stu = string.Empty;
            string msg = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            int response = 0;
            int error = 1;
            string xml = string.Empty;
            string strPOS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPOS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strUsername = Session["username"] != null ? Session["username"].ToString() : "";
            string strIPAddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string strSequenceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string strTerminalType = Session["TerminalType"] != null ? Session["TerminalType"].ToString() : "";
            string strPort = ConfigurationManager.AppSettings["PortNo"].ToString();
            string strHost = ConfigurationManager.AppSettings["HostAddress"].ToString();
            string strMailusername = ConfigurationManager.AppSettings["MailUsername"].ToString();
            string strMailpassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
            try
            {
                if (Session["agentid"] == null)
                {
                    stu = "-1";
                    msg = "Session Expired.";
                    return Json(new { Status = stu, Message = msg, Result = "" });
                }
                DatabaseLog.LogData(Session["username"].ToString(), "E", "Email Insurance", "Email Insurance Send Request", Insurancedet, Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
                string output = string.Empty;
                string MailFrom = Frommail;
                StringBuilder Strflights = new StringBuilder();
                if (Insurancedet != null && Insurancedet != "")
                {
                    string[] insurancedetai = Insurancedet.ToString().Split('~');
                    Strflights.Append("<div><table style='float:left'>");
                    Strflights.Append("<tr><td>SPNR</td><td style='text-align:left;font-weight: bold;'>" + insurancedetai[0] + "</td></tr>");
                    Strflights.Append("<tr><td>TrackId</td><td style='text-align:left;'>" + insurancedetai[1] + "</td></tr>");
                    Strflights.Append("</table></div><br />");
                    string[] dat1 = JsonConvert.DeserializeObject<string[]>(insurancemail_bookingdata);
                }

                //   Strflights.Append("");
                Strflights.Append("<div style='float:right'><label style='color:red;font-size:13px'>*All Fares in:" + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + "</label></div>");
                string Flights_table = Strflights.ToString();
                if (Insurancedet.ToString() != "" && Toadd.ToString() != "")
                {
                    bool ssl = ConfigurationManager.AppSettings["EnableSsl"].ToString() == "false" ? false : true;
                    Mailref.MyService pp = new Mailref.MyService();
                    pp.Url = ConfigurationManager.AppSettings["mailurl"].ToString();

                    output = pp.SendMailSingleTicket(strPOS_ID, strPOS_TID, strUsername, strIPAddress, strTerminalType, strSequenceID, Toadd, "",
                                    Subject, Flights_table, "Flight", Flights_table, strMailusername, strMailpassword, strHost,
                                    strPort, ssl, MailFrom, ".html");
                    xml = "<EVENT><FROMMAIL>" + Toadd + "</FROMMAIL>" + "<agentid>" + strPOS_ID + "</agentid>"
                                + "<terminalid>" + strPOS_TID + "</terminalid>" + "<username>" + strUsername + "</username>"
                                + "<ipAddress>" + strIPAddress + "</ipAddress>" + "<sequenceid>" + strSequenceID + "</sequenceid>"
                                + "<MailUsername>" + strMailusername + "</MailUsername>" + "<MailPassword>" + strMailpassword + "</MailPassword>"
                                + "<HostAddress>" + strHost + "</HostAddress>" + "<MailPort>" + strPort + "</MailPort>"
                                + "<ssl>" + ssl + "</ssl>" + "<MailFrom>" + MailFrom + "</MailFrom>"
                                + "<EXTension>" + ".html" + "</EXTension><RESPONSE>" + output + "</RESPONSE></EVENT>";

                    if (output == "Mail Sent")
                    {
                        stu = "01";
                        msg = "Your mail has been sent successfully.";
                        Array_Book[response] = msg;
                        DatabaseLog.LogData(Session["username"].ToString(), "S", "Email Avail", "Email Avail Send event", xml.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
                    }
                    else
                    {
                        stu = "00";
                        msg = "Mail sending failure. Please contact customer care (#03).";
                        DatabaseLog.LogData(Session["username"].ToString(), "S", "Email Avail", "Email Avail Send event", xml.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
                        Array_Book[error] = msg;
                    }
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Mail sending failure. Please contact customer care (#05).";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Email Avail", "Email Avail Send event", ex.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
                //DatabaseLog.LogData(Session["username"].ToString(), "X", "Email Avail", "Email Avail Send event", ex.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
                Array_Book[error] = msg;
            }
            //return Array_Book;
            return Json(new { Status = stu, Message = msg, Result = Array_Book });
        }

        #endregion

        #region bookinsurnace

        public ActionResult BookInsurance()
        {
            ViewBag.hdnurl = ConfigurationManager.AppSettings["Book_insurl"].ToString();

            try
            {
                string xml = "<EVENT><KEY>[<![CDATA[" + ConfigurationManager.AppSettings["Book_insurl"].ToString() + "]]>]</KEY><MAPURL>[<![CDATA[" + Request.QueryString["Key"] + "]]>]</MAPURL></EVENT>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "gettuneinsurancePageLoad", "Book_insurl", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                if (Request.QueryString["Key"] != null && Request.QueryString["Key"] != "")
                {
                    ViewBag.Key = Request.QueryString["Key"];

                }
            }

            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "gettuneinsurancePageLoad", "Book_insurl", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                RedirectToAction("SessionExp", "Redirect");
            }

            return View();
        }
        #endregion

        public ActionResult Fetchfarecalendar(string origin, string destination, string triptype, string Datevalue, string Flag, string Returndate)
        {
            string getTerminalType = string.Empty;// "W";
            string getIpAddress = string.Empty;// "1";
            string getSequenceId = string.Empty;// "1";
            string getTerminalID = string.Empty;// "TIQAJ010000103";
            string getUserName = string.Empty;// "sathees";

            string stu = string.Empty;
            string msg = string.Empty;
            string res = string.Empty;
            string lowfare = string.Empty;
            string lstrlowfare = string.Empty;
            DataSet my_ds = new DataSet();
            Hashtable my_param = new Hashtable();
            string strErrorMsg = string.Empty;
            try
            {
                if (Session["agentid"] == null)
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }

                //Rays_service.RaysService _rays_servers = new Rays_service.RaysService();
                InplantService.Inplantservice _inplantservice = new InplantService.Inplantservice();

                if (origin.Contains('(') == true && origin.Contains(')') == true && destination.Contains('(') == true && destination.Contains(')') == true)
                {
                    string org = origin.Split('(')[1].Split(')')[0];// txtStartingCityCode.Text.Trim();
                    string dest = destination.Split('(')[1].Split(')')[0];// txtReachingCityCode.Text.Trim();
                    string getTriptType = triptype;
                    getTerminalID = Session["terminalid"].ToString();
                    getTerminalType = "W";
                    getIpAddress = Session["ipAddress"].ToString();
                    getSequenceId = Session["sequenceid"].ToString();
                    getUserName = Session["username"].ToString();

                    string Month = string.Empty;
                    string Year = string.Empty;
                    string Curentday = string.Empty;
                    if (Flag != "1")
                    {
                        if (triptype == "O")
                        {
                            string Datemonthyear = Base.LoadServerdatetime();
                            string monthval = Datemonthyear.Split('/')[1];
                            string day = Datemonthyear.Split('/')[0];
                            string Crntyear = Datemonthyear.Split('/')[2];
                            if (Datevalue != null)
                            {
                                Month = (Convert.ToInt16(Datevalue.Split('_')[0].ToString()) > 9 ? Datevalue.Split('_')[0].ToString() : "0" + Datevalue.Split('_')[0].ToString());
                                Year = Datevalue.Split('_')[1].ToString();
                                if ((Convert.ToInt16(Month) > Convert.ToInt16(monthval) && Convert.ToInt16(Year) == Convert.ToInt16(Crntyear)) || Convert.ToInt16(Year) > Convert.ToInt16(Crntyear))
                                {
                                    Curentday = Year + Month + "01";
                                }
                                else
                                {
                                    Curentday = Crntyear + monthval + day;
                                }
                            }
                            else
                            {
                                Curentday = Crntyear + monthval + day;
                            }
                        }
                        else
                        {
                            string Datemonthyear = Base.LoadServerdatetime();
                            string monthval = Datemonthyear.Split('/')[1];
                            string day = Datemonthyear.Split('/')[0];
                            string Crntyear = Datemonthyear.Split('/')[2];
                            if (Datevalue != null)
                            {
                                Month = (Convert.ToInt16(Datevalue.Split('_')[0].ToString()) > 9 ? Datevalue.Split('_')[0].ToString() : "0" + Datevalue.Split('_')[0].ToString());
                                Year = Datevalue.Split('_')[1].ToString();
                                if (Datevalue != null && (Convert.ToInt16(Month) > Convert.ToInt16(monthval) && Convert.ToInt16(Year) == Convert.ToInt16(Crntyear)) || Convert.ToInt16(Year) > Convert.ToInt16(Crntyear))
                                {
                                    Curentday = Year + Month + "01";
                                }
                                else
                                {
                                    Curentday = DateTime.ParseExact(Datevalue, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                                }
                            }
                            else
                            {
                                Curentday = Crntyear + monthval + day;
                            }
                        }
                    }
                    else
                    {
                        if (triptype == "O")
                        {
                            string Datemonthyear = Base.LoadServerdatetime();
                            string monthval = Datevalue.Split('/')[1];
                            string Yearval = Datevalue.Split('/')[2];
                            string Curmonth = Datemonthyear.Split('/')[1];
                            string Crntyear = Datevalue.Split('/')[2];

                            if ((Convert.ToInt16(monthval) > Convert.ToInt16(Curmonth) && Convert.ToInt16(Crntyear) == Convert.ToInt16(Yearval)) || Convert.ToInt16(Crntyear) > Convert.ToInt16(Yearval))
                            {
                                Curentday = Yearval + monthval + "01";
                            }
                            else
                            {
                                Curentday = DateTime.ParseExact(Datemonthyear, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                            }
                        }
                        else
                        {
                            //Returndate
                            string monthval = Returndate.Split('/')[1];
                            string Yearval = Returndate.Split('/')[2];
                            string Curmonth = Datevalue.Split('/')[1];
                            string Crntyear = Datevalue.Split('/')[2];

                            if ((Convert.ToInt16(monthval) > Convert.ToInt16(Curmonth) && Convert.ToInt16(Crntyear) == Convert.ToInt16(Yearval)) || Convert.ToInt16(Crntyear) > Convert.ToInt16(Yearval))
                            {
                                Curentday = Yearval + monthval + "01";
                            }
                            else
                            {
                                Curentday = DateTime.ParseExact(Datevalue, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                            }
                        }
                    }

                    string xml = "<EVENT><ORG>" + org + "</ORG><DEST>" + dest + "</DEST><GETTRIPTTYPE>" + getTriptType + "</GETTRIPTTYPE><GETTERMINALID>" + getTerminalID + "</GETTERMINALID><MONTH>" + Month + "</MONTH><YEAR>" + Year + "</YEAR><CURRENTDAY>" + Curentday + "</CURRENTDAY></EVENT>";

                    DatabaseLog.LogData(Session["username"].ToString(), "E", "Fetchfarecalendar", "CalendarFare", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                    _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                    my_ds = _inplantservice.Fetchfarecalendar(org, dest, getTriptType, Curentday, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["username"].ToString(), Session["ipaddress"].ToString());


                    DatabaseLog.LogData(Session["username"].ToString(), "E", "Fetchfarecalendar", "CalendarFare", my_ds.GetXml(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                    if (my_ds != null && my_ds.Tables.Count > 0 && my_ds.Tables[0].Rows.Count > 0)
                    {
                        var Result = from p in my_ds.Tables[0].AsEnumerable()
                                     select new
                                     {
                                         Availfare = p["AVAILFARE"].ToString(),
                                         AvailDate = p["DATEVALUE"].ToString(),
                                         AvailDateref = p["DATEREF"].ToString()
                                     };
                        res = JsonConvert.SerializeObject(Result);

                        var Lowfare = (from r in my_ds.Tables[0].AsEnumerable()
                                       orderby Convert.ToDouble(r["AVAILFARE"]) ascending
                                       select new
                                       {
                                           Availlowfare = r["AVAILFARE"].ToString(),
                                       }).Distinct();

                        lowfare = JsonConvert.SerializeObject(Lowfare);
                    }
                }
            }
            catch (Exception ex)
            {
                string Requestparam = "<ERROR><REQUEST><ORIGIN>" + origin + "</ORIGIN><DESTINATION>" + destination + "</DESTINATION><TRIPTYPE>" + triptype + "</TRIPTYPE><DATEVALUE>" + Datevalue + "</DATEVALUE><RETURNDATE>" + Returndate + "</RETURNDATE></REQUEST><EXCEPTION>" + ex.ToString() + "</EXCEPTION></ERROR>";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "SearchController", "CalendarFare", Requestparam.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            return Json(new { Status = stu, Message = msg, Result = res, Response = lowfare });
        }

        public ActionResult SendAvailabilitySms(string Origin, string Destination, string DepartureDate, string FlightNo, string GrossAmount, string MobileNumber)
        {

            string xml = string.Empty;
            string refstring = string.Empty;
            string Status = string.Empty;
            string Error = string.Empty;
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strSequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : DateTime.Now.ToString("ddMMyyyy");
            string AgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strAgentLogin = Session["IsAgentLogin"] != null && Session["IsAgentLogin"].ToString() != "" ? Session["IsAgentLogin"].ToString() : "N";
            string strCustomerLogin = Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() != "" ? Session["CustomerLogin"].ToString() : "N";
            string strPlatform = string.Empty;
            try
            {
                if (strUserName == "" || strTerminalId == "" || strAgentID == "" || strSequnceID == "" || AgentType == "" || Ipaddress == "" || strTerminalType == "")
                {
                    return Json(new { Error = "Session Expires..", Status = "-1" });
                }
                InplantService.Inplantservice Inplant_wsdl = new InplantService.Inplantservice();
                Inplant_wsdl.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                Rays_service.RaysService _RaysService = new Rays_service.RaysService();
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                DataSet Ds_Tab = new DataSet();
                DataSet dsFareDispDet = new DataSet();
                StringBuilder strBuildSMS = new StringBuilder();
                string refError = "ERROR";

                string[] departureDate = DepartureDate.Split('\n');
                DateTime Depature = DateTime.ParseExact(departureDate[0].ToString(), "dd MMM yyyy HH:mm", CultureInfo.InvariantCulture);
                string departureTime = Depature.ToString("HH:mm");
                string[] flightnumber = FlightNo.Split('\n');
                /*SMS FORMATE*/
                strBuildSMS.Append(Origin.ToString() + " - " + Destination.ToString() + " ");
                strBuildSMS.Append("Flight:" + flightnumber[0] + " ");
                strBuildSMS.Append("Dt:" + Depature.ToString("dd MMM yyyy") + " ");
                strBuildSMS.Append("Time:" + departureTime + " ");
                strBuildSMS.Append("for Rs. ");
                string Basicfare = GrossAmount.ToString();
                strBuildSMS.Append(Basicfare);
                /*SMS FORMATE*/
                string XML = "<EVENT><MOBILENO>" + strBuildSMS.ToString() + "</MOBILENO><MESSAGE>" + strBuildSMS.ToString() + "</MESSAGE></EVENT>";
                DatabaseLog.LogData(strUserName, "E", "FlightController.cs", "SendAvailabilitySms", XML.ToString(), strAgentID, strTerminalId, strSequnceID);

                string ipaddress = Session["IPAddress"] != null ? Session["IPAddress"].ToString() : "";
                strPlatform = strAgentLogin != "Y" ? "B2C" : "B2B";
                bool smsRes = _RaysService.Send_SMS_Riya(strAgentID, strTerminalId, MobileNumber, strBuildSMS.ToString(), ref refstring, strUserName, "AIR", ipaddress, strTerminalType, Convert.ToDecimal(strSequnceID == "" ? "0" : strSequnceID), ref refError, strPlatform);

                xml = "<EVENT><SMSRESPONSE>" + smsRes + "</SMSRESPONSE></EVENT>";
                DatabaseLog.LogData(strUserName, "E", "FlightController.cs", "SendAvailabilitySms", XML.ToString(), strAgentID, strTerminalId, strSequnceID);
                if (smsRes == true)
                {
                    Status = "01";
                    Error = "SMS send successfully.";
                    XML = "<EVENT><MOBILENO>" + strBuildSMS.ToString() + "</MOBILENO><RESULT>" + strBuildSMS.ToString() + "</RESULT></EVENT>";
                    DatabaseLog.LogData(strUserName, "E", "FlightController.cs", "SendAvailabilitySms", XML.ToString(), strAgentID, strTerminalId, strSequnceID);
                }
                else
                {
                    Status = "00";
                    Error = (refError != "") ? refError : "Unable to send SMS.";
                    XML = "<EVENT><MOBILENO>" + strBuildSMS.ToString() + "</MOBILENO><RESULT>FAILED</RESULT><ERROR>" + Error + "</ERROR></EVENT>";
                    DatabaseLog.LogData(strUserName, "E", "FlightController.cs", "SendAvailabilitySms", XML.ToString(), strAgentID, strTerminalId, strSequnceID);
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(strUserName, "X", "FlightController.cs", "SendAvailabilitySms", ex.ToString(), strAgentID, strTerminalId, strSequnceID);
                Error = "Problem occured,while sending SMS";
                Status = "00";
            }
            return Json(new { Error = Error, Status = Status });

        }

        public ActionResult Changeservicemeals(bool Flag, string Origin, string Destination)
        {
            string Status = string.Empty;
            string Error = string.Empty;
            string Result = string.Empty;
            ArrayList RetArray = new ArrayList();
            RetArray.Add("");
            int res = 0;
            DataTable dtservice = new DataTable();
            DataTable dt = new DataTable();
            Base.ServiceUtility Serv = new Base.ServiceUtility();
            try
            {
                dtservice = (DataTable)Session["dtservicemeal"];
                if (Origin != "" && Destination != "")
                {
                    var qry = (from p in dtservice.AsEnumerable()
                               where p["Segment"].ToString() == Origin + "->" + Destination
                               select new
                               {
                                   Segment = p["Segment"],
                                   Segment_Code = p["Segment_Code"],
                                   MealCode = p["MealCode"],
                                   MealDesc = p["MealDesc"],
                                   MealAmt = p["MealAmt"],
                                   SegRef = p["SegRef"],
                                   Itinref = p["Itinref"],
                                   MyRef = p["MyRef"],
                                   Addsegorg = p["Addsegorg"],
                                   ServiceMeal = p["ServiceMeal"]
                               });
                    dt = Serv.ConvertToDataTable(qry);
                }
                else
                {
                    dt = dtservice;
                }
                Result = Bll.buildmealdropdown(dt, Flag);
            }
            catch (Exception ex)
            {
            }
            return Json(new { Error = Error, Status = Status, Result = Result });
        }
    }
}