using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.IO;
using STSTRAVRAYS.Models;
using STSTRAVRAYS.Rays_service;
using System.Net;
using STSTRAVRAYS.InplantService;
using Newtonsoft.Json;
using System.Xml;
using System.Web.Hosting;

namespace STSTRAVRAYS.Controllers
{
    public class LoginController : Controller
    {

        Inplantservice inplantservice = new Inplantservice();
        string strAgentLogoPath = ConfigurationManager.AppSettings["AgentLogoPath"].ToString().Trim();
        string strPlatform = ConfigurationManager.AppSettings["PLATFORM"].ToString();
        public ActionResult Login(string TermID, string UsrName, string Password, string EnvironmentType)
        {

            ViewBag.Title = System.Web.Configuration.WebConfigurationManager.AppSettings["Appname"] + " | Login";
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            ViewBag.version = ConfigurationManager.AppSettings["APPVersion"].ToString();

            if (ConfigurationManager.AppSettings["TerminalType"].ToString() == "T" && ConfigurationManager.AppSettings["DesktopAccess"].ToString() == "Y")
            {
                string tdid = Request.QueryString["TDID"] != null && Request.QueryString["TDID"] != "" ? Request.QueryString["TDID"].ToString() : "";

                if (tdid != "")
                {
                    Session["Webcheckkeytdid"] = tdid;
                    ViewBag.Checkwebkey = "Y";


                    string xmldata = "";
                    xmldata = "<EVENT><REQUEST>WEBSECURITYCHECK</REQUEST>" +
                                     "<Querystring>" + tdid + "</Querystring>" +
                                       "</EVENT>";

                    inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                    DatabaseLog.LogData("", "T", "LOGINCONTROLLER", "WEBAPPSSECCHECK~" + ConfigurationManager.AppSettings["TerminalType"].ToString(), xmldata, "", "", "0");
                    bool result = inplantservice.WEBAPPSSECCHECK(ref tdid);

                    xmldata = "<EVENT><REQUEST>WEBSECURITYCHECK</REQUEST>" +
                                "<ReferenceString>" + tdid + "</ReferenceString>" +
                                 "<BOOLEN>" + result + "</BOOLEN>" +
                                  "</EVENT>";

                    DatabaseLog.LogData("", "T", "LOGINCONTROLLER", "WEBAPPSSECCHECK~" + ConfigurationManager.AppSettings["TerminalType"].ToString(), xmldata, "", "", "0");
                    if (result)
                    {
                        ViewBag.Webcheckkey = tdid;
                        Session["SecurityWebkey"] = tdid.Split('|')[0];
                        ViewBag.usernamepasswor = tdid;
                    }
                    else
                    {
                        ViewBag.Webcheckkey = "";
                    }
                    return View("Login_T4");
                }
                else
                {
                    DatabaseLog.LogData("", "T", "LOGINCONTROLLER", "WEBAPPSSECCHECK~" + ConfigurationManager.AppSettings["TerminalType"].ToString(), "No Query string", "", "", "0");
                    ViewBag.Checkwebkey = "N";
                    return RedirectToAction("SessionExp", "Redirect");
                }
            }

            if (Session["terminalid"] != null && Session["terminalid"] != "" || Session["agencyname"] != null && Session["agencyname"] != "" || Session["username"] != null && Session["username"] != "")
            {
                if (ConfigurationManager.AppSettings["apptheme"].ToString().ToUpper() == "THEME2")
                {
                    return RedirectToAction("Homeflight", "Home");
                }
                else if (ConfigurationManager.AppSettings["Producttype"].ToString().ToUpper() == "JOD")
                {
                    return RedirectToAction("Flights", "Flights");
                }
                else if (ConfigurationManager.AppSettings["Producttype"].ToString().ToUpper() == "RIYA")
                {
                    return RedirectToAction("HomeMaster", "Home");
                }
                else if (ConfigurationManager.AppSettings["Producttype"].ToString().ToUpper() == "RBOA")
                {
                    return RedirectToAction("Home", "Home");
                }
                else
                {
                    return RedirectToAction("HomeMaster", "Home");
                }
            }
            else
            {
                if (EnvironmentType != null && EnvironmentType == "M")
                {
                    string mobReturn = string.Empty;
                    Loginsubmit(TermID, UsrName, Password, EnvironmentType);
                    Session.Add("EnvironmentType", EnvironmentType);
                    if (ConfigurationManager.AppSettings["apptheme"].ToString().ToUpper() == "THEME2")
                    {
                        return RedirectToAction("Homeflight", "Home");
                    }
                    else if (ConfigurationManager.AppSettings["Producttype"].ToString().ToUpper() == "JOD")
                    {
                        return RedirectToAction("Flights", "Flights");
                    }
                    else if (ConfigurationManager.AppSettings["Producttype"].ToString().ToUpper() == "RBOA")
                    {
                        return RedirectToAction("Home", "Home");
                    }
                    else
                    {
                        return RedirectToAction("HomeMaster", "Home");
                    }
                }
                if (ConfigurationManager.AppSettings["Producttype"].ToString().ToUpper() == "JOD")
                {
                    return RedirectToAction("Flights", "Flights");
                }
                else if (ConfigurationManager.AppSettings["Producttype"].ToString().ToUpper() == "RUAE")
                {
                    Session.Clear();
                    return View("~/Views/Login/Login_T3.cshtml");
                }
                else if (ConfigurationManager.AppSettings["Producttype"].ToString().ToUpper() == "RIYA" && ConfigurationManager.AppSettings["TerminalType"].ToString().ToUpper() == "T")
                {
                    Session.Clear();
                    return View("~/Views/Login/Login_T4.cshtml");
                }
                else if (ConfigurationManager.AppSettings["Producttype"].ToString().ToUpper() == "RIYA")
                {
                    Session.Clear();
                    return View("~/Views/Login/RiyaLogin.cshtml");
                }
                else if (ConfigurationManager.AppSettings["Producttype"].ToString().ToUpper() == "RBOA")
                {
                    Session.Clear();
                    return View("~/Views/Login/BOALogin.cshtml");
                }
                else
                {
                    Session.Clear();
                    return View("~/Views/Login/Login.cshtml");
                }

            }

        }

        public ActionResult Logins()//--?
        {
            if (ConfigurationManager.AppSettings["COUNTRY"].ToString() == "AE")
            {
                StringBuilder sb = new StringBuilder();
                string terms = System.IO.File.ReadAllText(Server.MapPath("/Content/FILES/TermsandCondition.html"));
                sb.Append(terms);
                ViewBag.termsrule = sb.ToString();
                StringBuilder sbap = new StringBuilder();
                string policy = System.IO.File.ReadAllText(Server.MapPath("/Content/FILES/Privacypolicy.html"));
                sbap.Append(policy);
                ViewBag.privacyrule = sbap.ToString();
                // footeridae.Visible = true;
                //footeridoae.Visible = false;
            }
            else
            {
                // footeridae.Visible = false;
                //footeridoae.Visible = true;
            }

            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            // if (!IsPostBack)
            // {
            //get_login.Attributes.Add("onclick", " return CheckVal();");

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            //  Version.InnerText = version.ToString();

            // }
            return View("~/Views/Login/Login.cshtml");
        }

        public ActionResult Loginsubmit(string tr_id, string NAME, string PWD, string Environment)
        {
            string sessionid = System.Web.HttpContext.Current.Session.SessionID;
            Session["UniqueSessionid"] = sessionid + "-" + DateTime.Now.ToString("HHmmss");
            ArrayList array_login = new ArrayList();
            array_login.Add("");
            int ss = 0;
            DataSet dsLogin = new DataSet();
            string strErrorMsg = string.Empty;
            string IPAddress = Base.GetComputer_IP();
            string strLoginDetails = string.Empty;

            string stu = string.Empty;
            string msg = string.Empty;
            string mobstu = string.Empty;
            string stragnId = string.Empty;
            string Terminal_type = string.Empty;
            string agenttype = string.Empty;
            string modalname = string.Empty;
            string terminal_app = Environment;
            Session.Add("EnvironmentType", Environment);
            try
            {
                if (Environment != "W" && Environment != "T")
                {
                    Session.Add("TERMINALTYPE", Environment);
                    Session.Add("Bookapptype", Environment);
                }
                else
                {
                    Session.Add("TERMINALTYPE", Environment);
                    //Session.Add("Bookapptype", (Session["Ismobilebrowser"] != null && Session["Ismobilebrowser"].ToString() != "" && Session["Ismobilebrowser"].ToString() == "1") ? "Z" : terminal_app);
                }

                inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                string prodct = ConfigurationManager.AppSettings["Producttype"].ToString();

                string XMLData = "<REQUEST><TERMINALID>" + tr_id + "</TERMINALID><USERNAME>" + NAME + "</USERNAME><PWD>" + PWD + "</PWD></REQUEST>";
                DatabaseLog.LogData(NAME, "E", "LoginController", "Login REQ", XMLData.ToString(), tr_id, tr_id, DateTime.Now.ToString("yyMMdd"));


                if (Environment == "T")
                {
                    tr_id = "";
                }

                char Terminal = Environment.ToCharArray()[0];
                if (Environment == "M")
                {
                    dsLogin = inplantservice.Fetch_Login_Details("U", NAME.Trim(), PWD, Base.GetComputer_IP(), tr_id.Trim(), Terminal, "", "", ref strErrorMsg, "", "", strPlatform);
                }
                else
                {
                    dsLogin = inplantservice.Fetch_Login_Details("U", NAME.Trim(), PWD, Base.GetComputer_IP(), tr_id.Trim(), Terminal, "", "", ref strErrorMsg, "", "", strPlatform);
                }

                if (strErrorMsg.ToUpper().Contains("ERROR~"))
                {
                    msg = strErrorMsg.Split('~')[1].Trim() == "" ? "Unable to Login (#03)" : strErrorMsg.Split('~')[1].Trim() + " (#01).";
                    XMLData = "<RESPONSE><RESULT>FALID</RESULT><ERROR>" + strErrorMsg + "</ERROR></RESPONSE>";
                    DatabaseLog.LogData(NAME, "E", "LoginController", "Login RES", XMLData.ToString(), tr_id, tr_id, DateTime.Now.ToString("yyMMdd"));
                    stu = "00";
                    mobstu = "M00";
                    return Json(new { Status = stu, Message = msg, Result = "", mobStatus = mobstu, agenttyp = agenttype }, JsonRequestBehavior.AllowGet);
                }

                string ipAddress = Base.GetComputer_IP();
                if (dsLogin != null && dsLogin.Tables.Count > 0 && dsLogin.Tables[0].Rows.Count > 0)
                {

                    XMLData = "<RESPONSE><RESULT>SUCCESS</RESULT><ERROR>" + strErrorMsg + "</ERROR></RESPONSE>";
                    DatabaseLog.LogData(NAME, "E", "LoginController", "Login RES", XMLData.ToString(), tr_id, tr_id, DateTime.Now.ToString("yyMMdd"));

                    //System.IO.File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath("~/XML/LOGIN.xml"), dsLogin.GetXml().ToString());

                    var _ds = dsLogin.Tables[0].Rows[0];
                    var _ds_table1 = dsLogin.Tables[1].Rows[0];
                    Session.Add("BRANCH_ACCESS", _ds["BRANCH_ACCESS"].ToString());
                    Session.Add("agencyname", _ds["AGENCY_NAME"].ToString());
                    Session.Add("agentid", _ds["LGN_AGENT_ID"].ToString().Trim().ToUpper());
                    Session.Add("terminalid", _ds["LGN_TERMINAL_ID"].ToString().Trim().ToUpper());
                    stragnId = _ds["LGN_AGENT_ID"].ToString().Trim().ToUpper();
                    Session.Add("username", NAME.Trim());
                    Session.Add("PWD", PWD);
                    Session.Add("POS_ID", _ds["LGN_AGENT_ID"]);
                    Session.Add("POS_TID", _ds["LGN_TERMINAL_ID"]);
                    Session.Add("Insure", _ds["INSURANCE"].ToString().ToUpper());
                    Session.Add("privilage", _ds["LGN_TERMINAL_PREVILAGE"].ToString());
                    Session.Add("agencyname", _ds["AGENCY_NAME"].ToString());
                    Session.Add("branchid", _ds["BCH_BRANCH_ID"].ToString());
                    Session.Add("DEFAULT_AGENTID", (dsLogin.Tables[0].Columns.Contains("DEFAULT_AGENTID") && _ds["DEFAULT_AGENTID"] != null ? _ds["DEFAULT_AGENTID"].ToString() : ""));
                    if (dsLogin.Tables[0].Columns.Contains("PROCESSING_IMAGE") && _ds["PROCESSING_IMAGE"] != null && _ds["PROCESSING_IMAGE"].ToString() != "")
                    {
                        Session.Add("ProcessingImage", ConfigurationManager.AppSettings["AgentLogoPath"].ToString() + "/Process/" + _ds["PROCESSING_IMAGE"].ToString());
                    }
                    string strWebHomeURL = "CLIENTID=" + _ds["LGN_AGENT_ID"] + "&TERMINALID=" + _ds["LGN_TERMINAL_ID"] + "&USERNAME=" + NAME + "&PWD=" + PWD + "&TERMINALTYPE=" + terminal_app + "&BRANCHID=" + _ds["BCH_BRANCH_ID"] + "&TERMINALPREVILAGE=" + _ds["LGN_TERMINAL_PREVILAGE"] + "&IPADDRESS=" + ipAddress + "&SEQUENCEID=" + _ds["SEQUENCE_ID"].ToString() + "";

                    string today = DateTime.Now.ToString("dd/MM/yyyy");
                    string Querystring = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["WEBHOMEURL"]) ? ConfigurationManager.AppSettings["WEBHOMEURL"].ToString() + "?SECKEY=" + Base.EncryptKEy(strWebHomeURL, "RIYA" + today) : "";

                    Session["WEBHOMEURL"] = Querystring.ToString();
                    string Passreset = string.Empty;
                    if ((prodct.ToString().ToUpper() == "RIYA" || prodct.ToString().ToUpper() == "RBOA") && (dsLogin.Tables[0].Columns.Contains("PASSWORDRESET") && _ds["PASSWORDRESET"].ToString().Trim() == "1"))
                    {
                        msg = "Password Same as riya123";
                        array_login[ss] = msg;
                        modalname = "modal-changespassword";
                        string date = Base.LoadServerdatetime();
                        return Json(new { Status = "05", Message = "", Result = "", Datee = date, AgnNm = Session["agencyname"].ToString(), agenttyp = "", Username = Session["username"].ToString(), TerminalId = Session["terminalid"].ToString() });
                    }
                    else
                    {

                        //);   //For test only

                        Session.Add("travraysws_url", ConfigurationManager.AppSettings["APPS_SELECT_URL"].ToString());
                        Session.Add("travraysws_vdir", ConfigurationManager.AppSettings["APPS_URL"].ToString());
                        Session.Add("Allowappcurrency", dsLogin.Tables[0].Columns.Contains("Currency") && dsLogin.Tables[0].Rows[0]["Currency"].ToString().Trim() != "" ? dsLogin.Tables[0].Rows[0]["Currency"].ToString().Trim() : ConfigurationManager.AppSettings["currency"].ToString());
                        Session.Add("ALLOW_RETRIEVE", dsLogin.Tables[0].Columns.Contains("ALLOW_RETRIEVE") ? dsLogin.Tables[0].Rows[0]["ALLOW_RETRIEVE"].ToString().Trim() : "");
                        Session.Add("sequenceid", _ds["SEQUENCE_ID"].ToString());

                        #region Threadkeyvalues
                        Session.Add("threadkey", _ds["Thread"].ToString());
                        Session.Add("multithreadkey", _ds["MultiThread"].ToString());
                        Session.Add("fscthreadkey", _ds["FscThread"].ToString());
                        Session.Add("lccthreadkey", _ds["LccThread"].ToString());
                        Session.Add("nearthread", _ds["NearThread"].ToString());
                        if (_ds["NewThreadC"].ToString() != "" && _ds["NewThreadC"].ToString() != null)
                        {
                            Session.Add("multithread", _ds["NewThreadC"].ToString());
                        }
                        else
                        {
                            Session.Add("multithread", "");
                        }
                        if (dsLogin.Tables[0].Columns.Contains("doublethread") == true && _ds["doublethread"].ToString() != "" && _ds["doublethread"].ToString() != null)
                        {
                            Session.Add("doublethread", _ds["doublethread"].ToString());
                        }
                        else
                        {
                            Session.Add("doublethread", ConfigurationManager.AppSettings["Doublethread"].ToString());
                        }
                        Session.Add("SFCthread", _ds["SFC"].ToString());
                        #endregion

                        #region Domesticthreadykey

                        Session.Add("threadkey_dom", dsLogin.Tables[0].Columns.Contains("ThreadD") ? _ds["ThreadD"].ToString() : _ds["Thread"].ToString());
                        Session.Add("multithreadkey_dom", dsLogin.Tables[0].Columns.Contains("MultiThreadD") ? _ds["MultiThreadD"].ToString() : _ds["MultiThread"].ToString());
                        Session.Add("fscthreadkey_dom", dsLogin.Tables[0].Columns.Contains("FscThreadD") ? _ds["FscThreadD"].ToString() : _ds["FscThread"].ToString());
                        Session.Add("lccthreadkey_dom", dsLogin.Tables[0].Columns.Contains("LccThreadD") ? _ds["LccThreadD"].ToString() : _ds["LccThread"].ToString());
                        Session.Add("nearthread_dom", dsLogin.Tables[0].Columns.Contains("NearThread_dom") ? _ds["NearThread_dom"].ToString() : _ds["NearThread"].ToString());
                        Session.Add("newthread_dom", dsLogin.Tables[0].Columns.Contains("NewThreadD") ? _ds["NewThreadD"].ToString() : _ds["NewThreadC"].ToString());
                        Session.Add("SFCthread_dom", dsLogin.Tables[0].Columns.Contains("SFCD") ? _ds["SFCD"].ToString() : _ds["SFC"].ToString());

                        #endregion

                        #region NewFareThread
                        Session.Add("StudentThreadkey", (dsLogin.Tables[0].Columns.Contains("STUDENTFARE")) ? ((_ds["STUDENTFARE"] != null) ? _ds["STUDENTFARE"].ToString() : "") : "");
                        Session.Add("ArmyThreadkey", (dsLogin.Tables[0].Columns.Contains("ARMEDTFARE")) ? ((_ds["ARMEDTFARE"] != null) ? _ds["ARMEDTFARE"].ToString() : "") : "");
                        Session.Add("SRCitizenThreadkey", (dsLogin.Tables[0].Columns.Contains("SRCITIZENTFARE")) ? ((_ds["SRCITIZENTFARE"] != null) ? _ds["SRCITIZENTFARE"].ToString() : "") : "");

                        Session.Add("StudentfareMsg", (dsLogin.Tables[0].Columns.Contains("STUDENTFAREDISPMSG")) ? ((_ds["STUDENTFAREDISPMSG"] != null) ? _ds["STUDENTFAREDISPMSG"].ToString() : "") : "");
                        Session.Add("ArmyfareMsg", (dsLogin.Tables[0].Columns.Contains("ARMEDTFAREDISPMSG")) ? ((_ds["ARMEDTFAREDISPMSG"] != null) ? _ds["ARMEDTFAREDISPMSG"].ToString() : "") : "");
                        Session.Add("SRCitizenfareMsg", (dsLogin.Tables[0].Columns.Contains("SRCITIZENTFAREDISPMSG")) ? ((_ds["SRCITIZENTFAREDISPMSG"] != null) ? _ds["SRCITIZENTFAREDISPMSG"].ToString() : "") : "");
                        #endregion
                        Session.Add("CORPORATETHREAD", dsLogin.Tables[0].Columns.Contains("CORPORATETHREAD") ? _ds["CORPORATETHREAD"].ToString() : "");
                        Session.Add("RETAILTHREAD", dsLogin.Tables[0].Columns.Contains("RETAILTHREAD") ? _ds["RETAILTHREAD"].ToString() : "");
                        Session.Add("VIATHREAD", dsLogin.Tables[0].Columns.Contains("VIATHREAD") ? _ds["VIATHREAD"].ToString() : "");

                        Session.Add("MACADDRESS", dsLogin.Tables[0].Columns.Contains("TRAIN_MAC_ID") ? string.IsNullOrEmpty(dsLogin.Tables[0].Rows[0]["TRAIN_MAC_ID"].ToString()) ? "" : dsLogin.Tables[0].Rows[0]["TRAIN_MAC_ID"].ToString() : "");
                        Session.Add("IRCTCUSERNAME", dsLogin.Tables[0].Columns.Contains("TRAIN_USER_ID") ? string.IsNullOrEmpty(dsLogin.Tables[0].Rows[0]["TRAIN_USER_ID"].ToString()) ? "" : dsLogin.Tables[0].Rows[0]["TRAIN_USER_ID"].ToString() : "");
                        Session.Add("ipAddress", IPAddress);
                        Session.Add("ticketing", _ds["ALLOW_TICKETING"].ToString());
                        Session.Add("block", _ds["ALLOW_BLOCKPNR"].ToString());
                        Session.Add("multiclass", Environment.Trim() == "T" ? "Y" : _ds["ALLOW_MULTICLASS"].ToString());
                        string strTripConfig = ConfigurationManager.AppSettings["APP_HOSTING"].ToString() == "BSA" ? (_ds["TRIPCONFIG"].ToString() + "|MD") : _ds["TRIPCONFIG"].ToString();
                        Session.Add("tripConfig", strTripConfig);
                        Session.Add("airline", _ds["AIRLINE"].ToString());
                        Session.Add("hotel", _ds["HOTEL"].ToString());
                        Session.Add("bus", _ds["BUS"].ToString());
                        Session.Add("train", _ds["TRAIN"].ToString());
                        Session.Add("Mobrech", _ds["MOB"].ToString());
                        Session.Add("Visa", _ds["VISA"].ToString());
                        Session.Add("Activity", (dsLogin.Tables[0].Columns.Contains("ACT") == true ? _ds["ACT"].ToString() : ""));
                        Session.Add("MISC", (dsLogin.Tables[0].Columns.Contains("MISC") == true ? _ds["MISC"].ToString() : "N"));
                        Session.Add("CAR", (dsLogin.Tables[0].Columns.Contains("CAR") == true ? _ds["CAR"].ToString() : "N"));
                        Session.Add("TITOS", (dsLogin.Tables[0].Columns.Contains("TITOS") == true ? _ds["TITOS"].ToString() : "N"));
                        Session.Add("ANCILLARY", (dsLogin.Tables[0].Columns.Contains("ANCILLARY") == true ? _ds["ANCILLARY"].ToString() : "N"));
                        Session.Add("THEMEPARK", (dsLogin.Tables[0].Columns.Contains("THEMEPARK") == true ? _ds["THEMEPARK"].ToString() : "N"));
                        Session.Add("QTICKETING", (dsLogin.Tables[0].Columns.Contains("VIEW_Q_TICKETING") == true ? _ds["VIEW_Q_TICKETING"].ToString() : "N"));
                        Session.Add("UTILITY", (dsLogin.Tables[0].Columns.Contains("MUA") ? _ds["MUA"].ToString() : "N"));
                        Session.Add("MONEYTRANSFER", dsLogin.Tables[0].Columns.Contains("MTF") ? _ds["MTF"].ToString() : "N");
                        Session.Add("ALLOWGDS", dsLogin.Tables[0].Columns.Contains("ALLOW_GDS") ? _ds["ALLOW_GDS"].ToString() : "N");
                        Session.Add("CRUISES", dsLogin.Tables[0].Columns.Contains("CRUISES") ? _ds["CRUISES"].ToString() : "N");//dsLogin.Tables[0].Columns.Contains("CRS") ? _ds["CRS"].ToString() : "N"

                        Session.Add("Farecalander", _ds["ALLOW_FARECALENDER"].ToString());
                        Session.Add("cntrlpanelrits", _ds["CTRLRIGHTS"].ToString());
                        Session.Add("touchpointentry", "1");
                        Session.Add("alowcredit", _ds["ALLOW_CREDIT"].ToString());
                        Session.Add("mailfromadd", _ds["MailFrom"].ToString());
                        Session.Add("pg_charge", _ds["PG_SERVICE_CHARGE"].ToString());
                        Session.Add("pg_Commonservice", _ds["PG_common_servicecharge"].ToString());
                        Session.Add("PRODUCT_CODE", _ds["PRODUCT_CODE"].ToString());

                        Session.Add("ALLOW_CREDIT", dsLogin.Tables[0].Columns.Contains("ALLOW_CREDIT") && dsLogin.Tables[0].Rows[0]["ALLOW_CREDIT"].ToString().Trim() != "" ? dsLogin.Tables[0].Rows[0]["ALLOW_CREDIT"].ToString().Trim() : "N");
                        Session.Add("ALLOW_PG", dsLogin.Tables[0].Columns.Contains("ALLOW_PG") && dsLogin.Tables[0].Rows[0]["ALLOW_PG"].ToString().Trim() != "" ? dsLogin.Tables[0].Rows[0]["ALLOW_PG"].ToString().Trim() : "N");
                        Session.Add("ALLOW_TOPUP", dsLogin.Tables[0].Columns.Contains("ALLOW_TOPUP") && dsLogin.Tables[0].Rows[0]["ALLOW_TOPUP"].ToString().Trim() != "" ? dsLogin.Tables[0].Rows[0]["ALLOW_TOPUP"].ToString().Trim() : "N");
                        Session.Add("ALLOW_PASSTHROUGH", dsLogin.Tables[0].Columns.Contains("ALLOW_PASSTHROUGH") && dsLogin.Tables[0].Rows[0]["ALLOW_PASSTHROUGH"].ToString().Trim() != "" ? dsLogin.Tables[0].Rows[0]["ALLOW_PASSTHROUGH"].ToString().Trim() : "N");
                        Session.Add("ALLOW_CORP_FARE", dsLogin.Tables[0].Columns.Contains("ALLOW_CORP_FARE") && dsLogin.Tables[0].Rows[0]["ALLOW_CORP_FARE"].ToString().Trim() != "" ? dsLogin.Tables[0].Rows[0]["ALLOW_CORP_FARE"].ToString().Trim() : "N");
                        Session.Add("ALLOW_TICKET_TOCANCEL", dsLogin.Tables[0].Columns.Contains("ALLOW_TICKETTOCANCEL") ? dsLogin.Tables[0].Rows[0]["ALLOW_TICKETTOCANCEL"].ToString().Trim() : "N");
                        if (dsLogin.Tables[0].Columns.Contains("pgminamt"))
                        { Session.Add("payfeeprecent", _ds["pgminamt"].ToString()); }
                        else
                        { Session.Add("payfeeprecent", ConfigurationManager.AppSettings["payfeeprecent"]); }

                        if (!string.IsNullOrEmpty(_ds["VIEW_COMMISSION"].ToString()))
                        {
                            string[] Com = _ds["VIEW_COMMISSION"].ToString().Split('|');
                            Session.Add("commission", Com[2] == "1" ? "Y" : "N");
                        }
                        else
                        {
                            Session.Add("commission", "N");
                        }

                        #region Credit Shell View Balance Access
                        DataSet dsProductAccess = new DataSet();
                        if (System.IO.File.Exists(Server.MapPath("~/XML/ProductAccess.xml")))
                        {
                            dsProductAccess.ReadXml(Server.MapPath("~/XML/ProductAccess.xml"));
                            if (dsProductAccess != null && dsProductAccess.Tables.Count > 0 && dsProductAccess.Tables.Contains("CREDITSHELLPNRVIEW") && dsProductAccess.Tables["CREDITSHELLPNRVIEW"].Rows.Count > 0 &&
                                (dsProductAccess.Tables["CREDITSHELLPNRVIEW"].Rows[0]["AGENTID"].ToString().Contains(_ds["LGN_AGENT_ID"].ToString().Trim().ToUpper()) || dsProductAccess.Tables["CREDITSHELLPNRVIEW"].Rows[0]["AGENTID"].ToString().Trim().ToUpper().Contains("ALL")))
                            {
                                Session.Add("CREDITSHELL", "Y");
                            }
                            else
                            {
                                Session.Add("CREDITSHELL", "N");
                            }
                        }
                        else
                        {
                            Session.Add("CREDITSHELL", "N");
                        }
                        #endregion

                        Session.Add("GMTTIME", ConfigurationManager.AppSettings["GMTTIME"].ToString());
                        Session.Add("MasteragentId", _ds["LGN_AGENT_ID"].ToString());
                        Session.Add("Agentsublogopath", "");
                        string strLogodata = dsLogin.Tables[0].Columns.Contains("CLT_LOGIN_LOGO_TYPE") && _ds["CLT_LOGIN_LOGO_TYPE"] != null && _ds["CLT_LOGIN_LOGO_TYPE"].ToString() != "" ? _ds["CLT_LOGIN_LOGO_TYPE"].ToString() : "";
                        if (!string.IsNullOrEmpty(strLogodata))
                        {
                            string strlogoagentid = strLogodata.Contains("-") ? strLogodata.Split('-')[1].ToString() : "";
                            Session.Add("Agentmainlogopath", strlogoagentid != "" ? (strAgentLogoPath + "/LoginLogo/" + strlogoagentid + ".png") : "");
                            Session.Add("Agentmainlogotype", strLogodata.Contains("-") ? strLogodata.Split('-')[0].ToString() : "");
                        }
                        else
                        {
                            Session.Add("Agentmainlogopath", "");
                            Session.Add("Agentmainlogotype", "");
                        }

                        agenttype = _ds["AGENT_TYPE"].ToString();
                        Session.Add("RightText", _ds["RIGHTTEXT"].ToString());
                        Session.Add("HomeText", _ds["HOMETEXT"].ToString());
                        Session.Add("TopText", _ds["UPPERTEXT"].ToString());
                        Session.Add("StandardText", _ds["STANDARDTEXT"].ToString());

                        Session.Add("AllowTab", string.IsNullOrEmpty(_ds["VIEW_COMMISSION"].ToString()) ? "0|0|0|0|0|0|0|0" : _ds["VIEW_COMMISSION"].ToString());
                        Session.Add("AGN_TDSPERCENTAGE", (dsLogin.Tables[0].Columns.Contains("TDS_PERCENTAGE") == true) ? _ds["TDS_PERCENTAGE"].ToString() : "");
                        Session.Add("privilage", _ds["LGN_TERMINAL_PREVILAGE"].ToString());
                        Session.Add("agenttype", _ds["AGENT_TYPE"].ToString());
                        Session.Add("agencyaddress", _ds["AGENCY_NAMEADDRESS"].ToString());
                        Session.Add("multicity", _ds["ALLOW_MULTICITY"].ToString());
                        Session.Add("blockpnr", _ds["BlockPnrCategories"].ToString());
                        Session.Add("creditacc", _ds["CreditAccCategories"].ToString());
                        Session.Add("accessimage", _ds["AGN_ACCESS_IMG"].ToString());   //_ds["AGN_ACCESS_IMG"].ToString());
                        Session.Add("AgencyPaymentDetails", _ds["AgencyPaymentDetails"].ToString());
                        Session.Add("Agreement", _ds["AGREEMENT"].ToString());

                        Session.Add("AgencyDetailsformail", dsLogin.Tables[0].Columns.Contains("Agency_Details") ? _ds["Agency_Details"].ToString() : "");

                        Session.Add("BranchagentId", dsLogin.Tables[0].Columns.Contains("Branchagentid") ? _ds["Branchagentid"].ToString() : "");
                        Session.Add("Agentsalesperson", (dsLogin.Tables[0].Columns.Contains("SALESMAN1") ? _ds["SALESMAN1"].ToString() : "") + "|" + (dsLogin.Tables[0].Columns.Contains("SALESMAN2") ? _ds["SALESMAN2"].ToString() : "") + "|" + (dsLogin.Tables[0].Columns.Contains("SALESMAN3") ? _ds["SALESMAN3"].ToString() : ""));
                        Session.Add("Branchcountry", dsLogin.Tables[0].Columns.Contains("BCH_COUNTRY") ? (_ds["BCH_COUNTRY"].ToString() == "" ? ConfigurationManager.AppSettings["Logincountry"].ToString() : _ds["BCH_COUNTRY"].ToString()) : ConfigurationManager.AppSettings["Logincountry"].ToString());

                        agenttype = _ds["AGENT_TYPE"] != null ? _ds["AGENT_TYPE"].ToString() : "";
                        Session.Add("Allowreports", (dsLogin.Tables.Count > 2 && dsLogin.Tables[2].Columns.Contains("WEB_RIGHTS")) ? dsLogin.Tables[2].Rows[0]["WEB_RIGHTS"].ToString() : "");

                        /* Agent Commission and markup Rights*/
                        Session.Add("Agent_CommissionRef", dsLogin.Tables[0].Columns.Contains("Commission") ? _ds["Commission"].ToString() : "");
                        Session.Add("Agent_MarkupRef", dsLogin.Tables[0].Columns.Contains("Markup") ? _ds["Markup"].ToString() : "");
                        Session.Add("Agent_ServiceTaxRef", dsLogin.Tables[0].Columns.Contains("ServiceTax") ? _ds["ServiceTax"].ToString() : "");
                        Session.Add("Agent_ServiceRef", dsLogin.Tables[0].Columns.Contains("ServiceCharge") ? _ds["ServiceCharge"].ToString() : "");
                        /* Agent Commission and markup Rights*/

                        Session.Add("AgentTerminalCount", dsLogin.Tables[0].Columns.Contains("AgentTerminalCount") ? _ds["AgentTerminalCount"].ToString() : "1");
                        Session.Add("ShowCommission", dsLogin.Tables[0].Columns.Contains("SHOWCOMMISSION") ? _ds["SHOWCOMMISSION"].ToString() : "1");
                        Session.Add("AllowEtravel", dsLogin.Tables[0].Columns.Contains("ETRAVELINSURANCE") ? _ds["ETRAVELINSURANCE"].ToString() : "1");
                        Session.Add("TICKETAUTOEMAILFORMAT", dsLogin.Tables[0].Columns.Contains("Ticketformat") ? (dsLogin.Tables[0].Rows[0]["Ticketformat"].ToString() == "P" ? "PDF" : "HTML") : ConfigurationManager.AppSettings["TicketAutoEmailFormat"].ToString());

                        Session.Add("ALLOW_DOM1G", dsLogin.Tables[0].Columns.Contains("DOM_1G_AIRLINES") && dsLogin.Tables[0].Rows[0]["DOM_1G_AIRLINES"].ToString().Trim() != "" ? dsLogin.Tables[0].Rows[0]["DOM_1G_AIRLINES"].ToString().Trim() : "");
                        Session.Add("ALLOW_INT1G", dsLogin.Tables[0].Columns.Contains("INT_1G_AIRLINES") && dsLogin.Tables[0].Rows[0]["INT_1G_AIRLINES"].ToString().Trim() != "" ? dsLogin.Tables[0].Rows[0]["INT_1G_AIRLINES"].ToString().Trim() : "");
                        Session.Add("ALLOW_DOM1S", dsLogin.Tables[0].Columns.Contains("DOM_1S_AIRLINES") ? dsLogin.Tables[0].Rows[0]["DOM_1S_AIRLINES"].ToString().Trim() : ConfigurationManager.AppSettings["SaberAirlines"].ToString());
                        Session.Add("ALLOW_INT1S", dsLogin.Tables[0].Columns.Contains("INT_1S_AIRLINES") ? dsLogin.Tables[0].Rows[0]["INT_1S_AIRLINES"].ToString().Trim() : ConfigurationManager.AppSettings["SaberAirlines"].ToString());
                        Session.Add("Labourthread", dsLogin.Tables[0].Columns.Contains("Labourthread") ? dsLogin.Tables[0].Rows[0]["Labourthread"].ToString().Trim() : ConfigurationManager.AppSettings["Labourthread"].ToString());
                        Session.Add("LabourfareAirline", dsLogin.Tables[0].Columns.Contains("LabourfareAirline") ? dsLogin.Tables[0].Rows[0]["LabourfareAirline"].ToString().Trim() : ConfigurationManager.AppSettings["LabourfareAirline"].ToString());
                        Session.Add("AgentAlterMail", dsLogin.Tables[0].Columns.Contains("AgentAlterMail") ? dsLogin.Tables[0].Rows[0]["AgentAlterMail"].ToString().Trim() : "");
                        Session.Add("ALLOW_TICKET_TOCANCEL", dsLogin.Tables[0].Columns.Contains("ALLOW_TICKET_TOCANCEL") ? dsLogin.Tables[0].Rows[0]["ALLOW_TICKET_TOCANCEL"].ToString().Trim() : "");
                        Session.Add("Agent_FSCCommission", dsLogin.Tables[0].Columns.Contains("FSCCommission") ? dsLogin.Tables[0].Rows[0]["FSCCommission"].ToString().Trim() : "");
                        string strAgentEmailid = dsLogin.Tables[0].Columns.Contains("AGENT_EMAIL_ID") ? dsLogin.Tables[0].Rows[0]["AGENT_EMAIL_ID"].ToString() : (dsLogin.Tables[0].Columns.Contains("EMAIL_ID") ? dsLogin.Tables[0].Rows[0]["EMAIL_ID"].ToString() : "");
                        Session.Add("AGENT_MAILID", strAgentEmailid);
                        if (Environment != "M" && ConfigurationManager.AppSettings["agreementread"].ToString().ToUpper() == "YES")
                        {
                            if (_ds["AGREEMENT"].ToString() != "" && _ds["AGREEMENT"].ToString() == "0")
                            {
                                string date = Base.LoadServerdatetime();
                                return Json(new { Status = "02", Message = "", Result = "", Datee = date, AgnNm = Session["agencyname"].ToString(), agenttyp = agenttype }); //02 - Firsttime user...
                            }
                        }

                        if (_ds["AGENT_TYPE"].ToString() == "DL" && dsLogin.Tables[2] != null && dsLogin.Tables[2].Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt = dsLogin.Tables[2].Copy();
                            DataSet agentdetdataset = new DataSet();
                            agentdetdataset.Tables.Add(dt);
                            Session.Add("agentdetdataset", agentdetdataset);
                        }
                        Session.Add("IsAgentLogin", "Y");
                        //***************** For Agreement Read **************** //
                        string u = Request.ServerVariables["HTTP_USER_AGENT"];
                        stu = "01";
                        mobstu = "M01";
                    }
                }
                else
                {
                    stu = "00";
                    mobstu = "M02";
                    msg = "Invalid Username or Password!";


                    array_login[ss] = "Invalid UserName or password";
                    //string log = "<UserName>" + NAME + "</UserName>";
                    //log += "<PWD>" + PWD + "</PWD>";
                    //log += "<tr_id>" + tr_id + "</tr_id>";
                    //log += "<LoginResponse>" + array_login[ss] + "</LoginResponse>";
                    // DatabaseLog.LogLogin("E", "Login", "Invalid UserName or password", log, ipAddress, tr_id.ToUpper(), 0, NAME);
                }

                //      InsertBrowserCap(tr_id, NAME);

                if (Session["agenttype"] != null && Session["agenttype"].ToString() == ConfigurationManager.AppSettings["ConsoleAgent"].ToString() && ConfigurationManager.AppSettings["Powerlogin"].ToString().Contains(Session["POS_ID"].ToString()) && ConfigurationManager.AppSettings["Powerterminal"].ToString().Contains(Session["POS_TID"].ToString()))
                {
                    Session.Add("Allowpoweruser", "NO");
                }
                else
                {
                    Session.Add("Allowpoweruser", "NO");
                }

            }
            catch (Exception ee)
            {
                if (ee.Message == "Unable to connect to the remote server")
                {
                    mobstu = "M03";
                    msg = "Please check the connectivity. (#05)";
                    string ipAddress = Base.GetComputer_IP();
                    string display = ee.ToString();
                    // ClientScript.RegisterStartupScript(this.GetType(), "Invalid input", "alert('Please check the connectivity');", true);
                    DatabaseLog.LogLogin("X", "Login", "Login", display, ipAddress, tr_id.ToUpper(), 0, NAME);
                }
                else if (ee.Message == "Operation has been timeout")
                {
                    mobstu = "M04";
                    msg = "Operation has been timeout. (#05)";
                    string ipAddress = Base.GetComputer_IP();
                    string display = ee.ToString();
                    //ClientScript.RegisterStartupScript(this.GetType(), "Invalid input", "alert('Operation timeout');", true);
                    DatabaseLog.LogLogin("X", "Login", "Login", display, ipAddress, tr_id.ToUpper(), 0, NAME);
                }
                else
                {
                    mobstu = "M05";
                    string ipAddress = Base.GetComputer_IP();
                    string display = ee.ToString();
                    msg = "Problem occured while login. Please contact customer care. (#05).";
                    //ClientScript.RegisterStartupScript(this.GetType(), "Invalid input", "alert('Problem occured while login. Please contact customer care.');", true);
                    DatabaseLog.LogLogin("X", "Login", "Login", display, ipAddress, tr_id.ToUpper(), 0, NAME);
                }
                stu = "00";
            }
            return Json(new { Status = stu, Message = msg, Result = stragnId, mobStatus = mobstu, agenttyp = agenttype }, JsonRequestBehavior.AllowGet);
        }

        public static bool CheckFileExistFrmWeb(string Path)
        {
            try
            {
                WebRequest request = HttpWebRequest.Create(Path);
                request.Method = "HEAD"; // Just get the document headers, not the data.
                request.Credentials = System.Net.CredentialCache.DefaultCredentials;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    return (response.StatusCode == HttpStatusCode.OK);
            }
            catch (WebException)
            {
                return false;
            }
        }

        public ActionResult Insert_agent_agrement()
        {
            string stu = string.Empty;
            string msg = string.Empty;

            RaysService _RaysService = new RaysService();
            bool ds_res = false;
            string AgentID = string.Empty;
            string terminalId = string.Empty;
            string userName = string.Empty;
            string Ipaddress = string.Empty;
            string seqid = string.Empty;
            try
            {
                if (Session["agentid"] == null)
                {
                    stu = "-1";
                    msg = "session Expired.";
                    return Json(new { Status = stu, Message = msg, Result = "" });
                }

                AgentID = Session["POS_ID"].ToString();
                terminalId = Session["POS_TID"].ToString();
                userName = Session["username"].ToString();
                Ipaddress = Session["ipAddress"].ToString();
                seqid = Session["sequenceid"].ToString();
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                ds_res = _RaysService.INSERT_UPDATE_AGENT_AGREEMENT(AgentID, terminalId, userName, "W", Ipaddress, seqid);
                Session.Add("Agreement", "1");
                stu = "01";
                DatabaseLog.LogData(Session["username"].ToString(), "T", "New Agent Agreement Submit", "Insert_agent_agrement", AgentID, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

            }
            catch (Exception ex)
            {
                stu = "00";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "New Agent Agreement", "Insert_agent_agrement", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }

            return Json(new { Status = stu, Message = msg, Result = ds_res });
        }

        public ActionResult Logout(string termID, string agnID)
        {
            string stu = string.Empty;
            string msg = string.Empty;
            string res = string.Empty;

            string Terminalid = string.Empty;
            string Username = string.Empty;

            RaysService _RaysService = new RaysService();
            try
            {

                Terminalid = termID + "~W";
                Username = agnID;

                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                res = _RaysService.Insert_Update_Login_Status(Terminalid, Username, "2", "", ref msg, 0, "");
                Session.Clear();
                Session.Abandon();
                stu = "01";
            }
            catch (Exception)
            {
                msg = "Internal problem occured (#05).";
                stu = "00";
            }
            return Json(new { Status = stu, Message = msg, Result = res });
        }

        public ActionResult Logoutpopup()
        {
            #region UsageLOg
            string PageName = "Logout";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "LOGOUT");
            }
            catch (Exception e) { }
            #endregion
            Session.Clear();
            if (ConfigurationManager.AppSettings["APP_HOSTING"] != null && ConfigurationManager.AppSettings["APP_HOSTING"].ToString() == "BSA")
            {
                string strTerminalID = ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString();
                string strUsername = ConfigurationManager.AppSettings["BSA_USERNAME"].ToString();
                string strPassword = ConfigurationManager.AppSettings["BSA_PASSWORD"].ToString();
                string strTerminalType = ConfigurationManager.AppSettings["TerminalType"].ToString();
                string path = Server.MapPath("~/XML/LOGIN.xml");
                string LoginDetXml = System.IO.File.ReadAllText(path);
                DataSet dsBCI_LOGIN = new DataSet();
                dsBCI_LOGIN.ReadXml(new XmlTextReader(new StringReader(LoginDetXml)));
                bool msg = BSA_AssignSession(strTerminalID, strUsername, strPassword, dsBCI_LOGIN, strTerminalType);
                return RedirectToAction("Flights", "Flights");
            }
            else
            {
                if (ConfigurationManager.AppSettings["TerminalType"].ToString() == "T" && ConfigurationManager.AppSettings["DesktopAccess"].ToString() == "Y")
                    return RedirectToAction("SessionExp", "Redirect", new { Logout = "Y" });
                else
                    return RedirectToAction("Login", "Login");
            }
        }

        #region download_setup
        public ActionResult Download()
        {
            return View();
        }

        public ActionResult Download_submit(string tr_id, string NAME, string PWD, string Environment)
        {
            // LogDataBaseClass logObject = new LogDataBaseClass();

            ArrayList array_login = new ArrayList();
            array_login.Add("");
            array_login.Add("");
            int download_err = 0;
            int download_suc = 1;
            Inplantservice _inplantservice = new Inplantservice();


            DataSet dsLogin = new DataSet();
            string strErrorMsg = string.Empty;
            string ipAddress = Base.GetComputer_IP();
            string strLoginDetails = string.Empty;

            if (tr_id == "" || tr_id == null)
            {
                array_login[download_err] = "Please enter Terminal id!";
            }
            else if (NAME == "" || NAME == null)
            {
                array_login[download_err] = "Please enter User name!";
            }
            else if (PWD == "" || PWD == null)
            {
                array_login[download_err] = "Please enter Password!";
            }
            try
            {

                string desc = "<DownloadRequest><TerminalID>" + tr_id + "</TerminalID><UserName>" + NAME +
                    "</UserName><txt_passwd>" + PWD + "</txt_passwd></DownloadRequest>";
                DatabaseLog.LogLogin("D", "SetupDownload", "Download", desc, ipAddress, tr_id, 0, NAME);
                string strResult = string.Empty;
                string departureDate = string.Empty;
                ArrayList Array_Book = new ArrayList(2);
                Array_Book.Add("");
                Array_Book.Add("");
                DataSet my_ds = new DataSet();
                Hashtable my_param = new Hashtable();

                my_param.Add("TERMINAL_ID", tr_id);
                my_param.Add("NAME", NAME);
                my_param.Add("PWD", PWD);
                try
                {
                    _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                    dsLogin = _inplantservice.fetchdownloaddetails(tr_id, NAME, PWD);
                    // dsLogin = DBHandler.ExecSelectProcedure("P_FETCH_GETDOWNLOADDETAILS", my_param, ref strErrorMsg);
                    DatabaseLog.LogLogin("D", "SetupDownload", dsLogin.GetXml(), desc, ipAddress, tr_id, 0, NAME);
                    if (strErrorMsg.ToUpper().Contains("ERROR~"))
                    {
                        string str = strErrorMsg.Split('~')[1].Trim() == "" ? "Unable to Login" : strErrorMsg.Split('~')[1].Trim();
                        desc = "<DownloadResponse><STATUS>" + str + "</STATUS><TerminalID>" + tr_id + "</TerminalID><UserName>" + NAME +
                                "</UserName><txt_passwd>" + PWD + "</txt_passwd></DownloadResponse>";
                        DatabaseLog.LogLogin("D", "SetupDownload", str, desc, ipAddress, tr_id, 0, NAME);
                    }
                }
                catch
                {

                }
                if (dsLogin != null && dsLogin.Tables.Count > 0 && dsLogin.Tables[0].Rows.Count > 0)
                {
                    var _ds = dsLogin.Tables[0].Rows[0];
                    string strError = string.Empty;
                    string strSeqno = string.Empty;
                    if (_ds["ASSIGNED_TERMINAL"].ToString() != "" && _ds["ASSIGNED_TERMINAL"].ToString().Contains("C"))
                    {
                        string Clientfilename = ConfigurationManager.AppSettings["ClientDownloadfilename"].ToString();
                        array_login[download_suc] = Clientfilename;
                        desc = "<DownloadResponse><STATUS>Downloaded Successfully</STATUS><TerminalID>" + tr_id + "</TerminalID><UserName>" + NAME +
               "</UserName><txt_passwd>" + PWD + "</txt_passwd><Download_path>" + Clientfilename + "</Download_path></DownloadResponse>";

                        DatabaseLog.LogLogin("D", "SetupDownload", "Download-Success", desc, ipAddress, tr_id, 0, NAME);

                    }
                    else
                    {
                        array_login[download_err] = "Unauthorized to download the Application.Please Contact Customer Care. ";
                        desc = "<DownloadResponse><STATUS>Download RESTRICTED</STATUS><TerminalID>" + tr_id + "</TerminalID><UserName>" + NAME +
             "</UserName><txt_passwd>" + PWD + "</txt_passwd></DownloadResponse>";

                        DatabaseLog.LogLogin("D", "SetupDownload", "Download-Download RESTRICTED", desc, ipAddress, tr_id, 0, NAME);
                    }
                }
                else
                {
                    array_login[download_err] = "Invalid UserName or password";
                    desc = "<DownloadResponse><STATUS>INVALID USER</STATUS><TerminalID>" + tr_id + "</TerminalID><UserName>" + NAME +
             "</UserName><txt_passwd>" + PWD + "</txt_passwd></DownloadResponse>";

                    DatabaseLog.LogLogin("D", "SetupDownload", "Download-INVALID USER", desc, ipAddress, tr_id, 0, NAME);
                }
            }
            catch (Exception ee)
            {
                ipAddress = Base.GetComputer_IP();
                string display = ee.Message;
                if (display != "Thread was being aborted")
                {
                    DatabaseLog.LogLogin("D", "SetupDownload", "Download-EX", display, ipAddress, tr_id, 0, NAME);
                }
            }
            return Json(new { Status = "", Message = "", Result = array_login });
        }
        #endregion

        public ActionResult Registrationsubmit(string agencyname, string address, string cityname, string mobileno, string emailadd, string contctperson)
        {
            string stu = string.Empty;
            string msg = string.Empty;
            string stragnId = string.Empty;
            try
            {

                if (IsValidEmail(emailadd) == true)
                {
                    string IPAddress = Base.GetComputer_IP();
                    STSTRAVRAYS.Mailref.MyService _mailservice = new STSTRAVRAYS.Mailref.MyService();

                    string XMLL = "<REQUEST><AGENCYNAME>" + agencyname + "</AGENCYNAME><ADDRESS>" + address + "</ADDRESS><CITYNAME>" + cityname + "</CITYNAME><MOBILENO>" + mobileno
                               + "</MOBILENO><EMAILADD>" + emailadd + "</EMAILADD><CONTACTPERSON>" + contctperson + "</CONACTPERSON></REQUEST>";


                    DatabaseLog.LogData("AgentReg", "S", "Email Registration Request", "Email Agent Registration Request", XMLL.ToString(), "Newagentid", "NewagenttId", "101");

                    StringBuilder builder = new StringBuilder();
                    builder.Append("<div>Dear Team,</div><br/>");
                    builder.Append("<div>Greetings from TravRays !!!</div><br/><br/>");
                    builder.Append("<table id='tblInnerActivityStatic1' style='border:1px solid #ccc; margin-bottom:20px;' >");
                    builder.Append("<tr style='background-color: #f9f9f9;'>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px; font-weight:bold;background-color: #7dcdff !important;'>Agency Name</td>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;'>" + agencyname + "</td>");
                    builder.Append("<tr style='background-color: #f9f9f9;'>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px; font-weight:bold;background-color: #7dcdff !important;''>Address</td>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;'>" + address + "</td>");
                    builder.Append("<tr style='background-color: #f9f9f9;'>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px; background-color: #7dcdff !important;font-weight:bold;''>City Name</td>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;'>" + cityname + "</td>");
                    builder.Append("<tr style='background-color: #f9f9f9;'>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px; background-color: #7dcdff !important;font-weight:bold;''>Contact Person</td>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;'>" + contctperson + "</td>");
                    //builder.Append("<tr style='background-color: #f9f9f9;'>");
                    //builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px; font-weight:bold;background-color: #7dcdff !important;''>Phone No</td>");
                    //builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;'>" + phoneno + "</td>");
                    builder.Append("<tr style='background-color: #f9f9f9;'>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;background-color: #7dcdff !important; font-weight:bold;''>Mobile No</td>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;'>" + mobileno + "</td>");
                    //builder.Append("<tr style='background-color: #f9f9f9;'>");
                    //builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px; font-weight:bold;background-color: #7dcdff !important;''>Office Manager Name</td>");
                    //builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;'>" + officemngrname + "</td>");
                    //builder.Append("<tr style='background-color: #f9f9f9;'>");
                    //builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;background-color: #7dcdff !important; font-weight:bold;''>Office Contact No</td>");
                    //builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;'>" + officecontact + "</td>");
                    builder.Append("<tr style='background-color: #f9f9f9;'>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px; font-weight:bold;background-color: #7dcdff !important;''>Email Address</td>");
                    builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;'>" + emailadd + "</td>");
                    //builder.Append("<tr style='background-color: #f9f9f9;'>");
                    //builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;background-color: #7dcdff !important; font-weight:bold;''>Fax No</td>");
                    //builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;'>" + (faxno == "" ? "N/A" : faxno) + "</td>");
                    //builder.Append("<tr style='background-color: #f9f9f9;'>");
                    //builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px; font-weight:bold;background-color: #7dcdff !important;''>IATA No</td>");
                    //builder.Append("<td style='text-align:center; border:1px solid #ddd; padding:10px;'>" + (iatano == "" ? "N/A" : iatano) + "</td></tr>");
                    builder.Append("</table>");
                    builder.Append("<div>Thanks and Regards,</div><br/>");
                    builder.Append("<div>TravRays Team</div>");
                    _mailservice.Url = ConfigurationManager.AppSettings["Mailurl"].ToString();
                    string tomailId = ConfigurationManager.AppSettings["Newagentmail"].ToString();
                    string Mailfrom = ConfigurationManager.AppSettings["NetworkUsername"].ToString();
                    string lstr = _mailservice.SendMailSingleTicket("", "", "Travrays Egypt", IPAddress, "W", "1",
                                 tomailId, "", "TravRays-New Agency Request", builder.ToString(), "", "", ConfigurationManager.AppSettings["MailUsername"].ToString(),
                                 ConfigurationManager.AppSettings["MailPassword"].ToString(), ConfigurationManager.AppSettings["HostAddress"].ToString(),
                                 ConfigurationManager.AppSettings["PortNo"].ToString(), (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"].ToString())),
                                 Mailfrom, ".htm");

                    //string XML = "<REQUEST><AGENCYNAME>" + agencyname + "</AGENCYNAME><ADDRESS>" + address + "</ADDRESS><CITYNAME>" + cityname + "</CITYNAME><PHONENO>" + phoneno
                    //    + "</PHONENO><MOBILENO>" + mobileno + "</MOBILENO><OFFICEMANAGERNAME>" + officemngrname + "</OFFICEMANAGERNAME><OFFICECONTACT>" + officecontact
                    //    + "</OFFICECONTACT><EMAILADD>" + emailadd + "</EMAILADD><FAXNO>" + faxno + "</FAXNO><IATANO>" + iatano + "</IATANO></REQUEST><RESPONSE>[<![CDATA[" + lstr + "]]>]</RESPONSE>";

                    string XML = "<REQUEST><AGENCYNAME>" + agencyname + "</AGENCYNAME><ADDRESS>" + address + "</ADDRESS><CITYNAME>" + cityname + "</CITYNAME><MOBILENO>" + mobileno
                                + "</MOBILENO><EMAILADD>" + emailadd + "</EMAILADD><CONTACTPERSON>" + contctperson + "</CONACTPERSON></REQUEST><RESPONSE>[<![CDATA[" + lstr + "]]>]</RESPONSE>";


                    DatabaseLog.LogData("AgentReg", "S", "Email Registration", "Email Agent Registration", XML.ToString(), "Newagentid", "NewagenttId", "101");

                    if (lstr.ToUpper() == "MAIL SENT")
                    {
                        stu = "01";
                        msg = "Thank you for registering with us. Our support team will contact you at the earliest.";
                    }
                    else
                    {
                        stu = "00";
                        msg = "Unable to process your request please contact support team (#01)";
                    }
                }
                else
                {
                    stu = "02";
                    msg = "Please enter valid mail-id";
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Unable to process your request please contact support team (#02)";
                string Requestparam = "<ERROR><REQUEST><AGENCYNAME>" + agencyname + "</AGENCYNAME><ADDRESS>" + address + "</ADDRESS><CITYNAME>" + cityname + "</CITYNAME><CONTACTPERSON>"
                    + contctperson + "</CONACTPERSON><MOBILENO>" + mobileno + "</MOBILENO><EMAILADD>" + emailadd + "</EMAILADD></REQUEST><EXCEPTION>[<![CDATA[" + ex.ToString() + "]]>]</EXCEPTION></ERROR>";
                DatabaseLog.LogData("AgentReg", "X", "Email Registration", "Email Agent Registration", Requestparam.ToString(), "Newagentid", "NewagenttId", "101");
            }
            return Json(new { Status = stu, Message = msg });
        }

        public class LocationLogDetails
        {
            public string BRANCH { get; set; }
            public string GROUP_ID { get; set; }
            public string AGENT_ID { get; set; }
            public string TERMINAL_ID { get; set; }
            public string CLIENT_ID { get; set; }
            public string USERNAME { get; set; }
            public string PASSWORD { get; set; }
            public string PRODUCT_CODE { get; set; }
            public string PLATFORM { get; set; }
            public string UNIQUEID { get; set; }
            public string STATUS { get; set; }
            public string CREATED_DATE { get; set; }
            public string BROWSER { get; set; }
            public string IP { get; set; }
            public string ISP { get; set; }
            public string LATITUDE { get; set; }
            public string LONGITUDE { get; set; }
            public string CITY { get; set; }
            public string COUNTRY { get; set; }
            public string STATE { get; set; }
            public string REMARKS { get; set; }
            public string PC_NAME { get; set; }
            public string CLIENT_NAME { get; set; }
            public string USER_ID { get; set; }
            public string SEQUENCE_ID { get; set; }

            public string PRODUCTNAME { get; set; }
            public string TERMINALTYPE { get; set; }
            public string USERTYPE { get; set; }
            //  public string LOGOUT_TIME { get; set; }
            //public int id { get; set; }
        }

        public ActionResult Commonlog(string CLIENT_ID, string USERNAME, string PASSWORD, string PRODUCT_CODE, string PLATFORM, string STATUS, string BROWSER, string IP, string ISP, string LATITUDE, string LONGITUDE, string CITY, string COUNTRY, string STATE, string REMARKS)
        {
            string StrInput = "";
            string strResponse = "";
            string Platform = "";
            string terminal_app = "";
            try
            {

                LocationLogDetails _LocationLogDetails = new LocationLogDetails();
                if (STATUS.ToString().ToUpper() == "SUCCESS")
                {
                    _LocationLogDetails.AGENT_ID = Session["POS_ID"].ToString();
                    _LocationLogDetails.CLIENT_ID = Session["POS_ID"].ToString();
                    _LocationLogDetails.TERMINAL_ID = Session["POS_TID"].ToString();
                    _LocationLogDetails.USER_ID = Session["POS_TID"].ToString();
                    _LocationLogDetails.PASSWORD = PASSWORD;
                    _LocationLogDetails.USERNAME = Session["username"].ToString();
                    _LocationLogDetails.BRANCH = Session["branchid"].ToString();
                    _LocationLogDetails.CLIENT_NAME = Session["agencyname"].ToString();
                    terminal_app = (Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "") ? (Session["TERMINALTYPE"].ToString() == "W" && PLATFORM.Contains("Mobile")) ? "Z" : Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                    Session.Add("Bookapptype", terminal_app);
                }
                else
                {
                    _LocationLogDetails.AGENT_ID = "";
                    _LocationLogDetails.CLIENT_ID = "";
                    _LocationLogDetails.TERMINAL_ID = CLIENT_ID;
                    _LocationLogDetails.PASSWORD = PASSWORD;
                    _LocationLogDetails.USERNAME = USERNAME;
                    _LocationLogDetails.BRANCH = "";
                    _LocationLogDetails.CLIENT_NAME = "";
                    _LocationLogDetails.USER_ID = "";
                }


                _LocationLogDetails.BROWSER = BROWSER;
                _LocationLogDetails.CITY = CITY;
                _LocationLogDetails.COUNTRY = COUNTRY;
                _LocationLogDetails.CREATED_DATE = "";
                _LocationLogDetails.GROUP_ID = "";
                _LocationLogDetails.IP = IP;
                _LocationLogDetails.ISP = ISP;
                _LocationLogDetails.LATITUDE = LATITUDE;
                _LocationLogDetails.LONGITUDE = LONGITUDE;
                _LocationLogDetails.PLATFORM = (Session["TERMINALTYPE"].ToString() == "T" ? "TDK" : "B2B");
                _LocationLogDetails.PRODUCT_CODE = ConfigurationManager.AppSettings["Producttype"] == "RBOA" ? "BOA" : "B2B";
                _LocationLogDetails.REMARKS = REMARKS;
                _LocationLogDetails.STATE = STATE;
                _LocationLogDetails.STATUS = STATUS;
                _LocationLogDetails.UNIQUEID = "";
                _LocationLogDetails.PC_NAME = PLATFORM.Contains("Mobile") ? "Mobile" : "Desktop";


                _LocationLogDetails.PRODUCTNAME = ConfigurationManager.AppSettings["Producttype"] == "RBOA" ? "Riya BOA" : ConfigurationManager.AppSettings["Appname"].ToString();
                _LocationLogDetails.TERMINALTYPE = (Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" && Session["TERMINALTYPE"].ToString() != "") ? (Session["TERMINALTYPE"].ToString() == "W" && PLATFORM.Contains("Mobile")) ? "Z" : Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                _LocationLogDetails.USERTYPE = "";
                _LocationLogDetails.SEQUENCE_ID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";

                StrInput = JsonConvert.SerializeObject(_LocationLogDetails);

                if (ConfigurationManager.AppSettings["STaskiesLoginLogURL"].ToString() != "")
                {
                    MyWebClient Client = new MyWebClient();
                    Client.Headers["Content-Type"] = "application/json";
                    Client.LintTimeout = 7000; // 7 SECONDS
                    byte[] byteGetLogin = Client.UploadData(System.Configuration.ConfigurationManager.AppSettings["STaskiesLoginLogURL"], "POST", Encoding.ASCII.GetBytes(StrInput));
                    strResponse = Encoding.ASCII.GetString(byteGetLogin);
                }

            }
            catch (Exception ex)
            {
                //DatabaseLog.LogData(Session["username"].ToString(), "X", "Commonlog", "Commonlog - Insert", ex.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
            }
            return View();
        }

        protected bool IsValidEmail(string sEmail)//--?
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex orgxEmail = new System.Text.RegularExpressions.Regex(strRegex);
            return orgxEmail.IsMatch(sEmail);
        }

        public ActionResult ForgotPasswordGenerateOTP(string strOTPtype, string strTerminalid, string strUsername, string strOTPname, string strTerminalType, string Type)
        {
            string Status = string.Empty;
            string Error = string.Empty;
            string Results = string.Empty;
            string ErrorMsg = string.Empty;
            string IpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            string dcSequenceId = DateTime.Now.ToString("yyMMdd");
            string AgentID = strTerminalid.Remove(strTerminalid.Length - 2);
            string logxmlData = string.Empty;
            string strError = string.Empty;
            string strResultcode = string.Empty;
            string strMessage = string.Empty;
            string Functionname = (Type.ToUpper().ToString() == "FIRSTLOGIN") ? "First Login Generate OTP" : "Forget Password Generate OTP";
            try
            {
                string ReqTime = DateTime.Now.ToString();
                string Producttype = ConfigurationManager.AppSettings["Producttype"].ToString();
                string description = Producttype + "-ForgetPassword";
                logxmlData = "<EVENT><REQ>" + Functionname + "</REQ><REQTIME>" + ReqTime + "</REQTIME><PAGENAME>LoginController</PAGENAME><AgentId>" + AgentID + "</AgentId><TerminalId>" +
                    strTerminalid + "</TerminalId><Username>" + strUsername + "</Username></EVENT>";
                DatabaseLog.LogData(strUsername, "S", "LoginController", Functionname, logxmlData.ToString(), AgentID, strTerminalid, dcSequenceId);

                Rays_service.RaysService _rays_servers = new Rays_service.RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                bool result = _rays_servers.GenerateOTPforForgetPassword(AgentID, strTerminalType, strTerminalid, strUsername, strOTPtype, description, strOTPname, "F",
                   IpAddress, dcSequenceId, ref strResultcode, ref strMessage, ref strError);

                string ResTime = DateTime.Now.ToString();
                logxmlData = "<EVENT><RES>" + Functionname + "</RES><RESTIME>" + ResTime + "</RESTIME><RESULT>" + result.ToString() + "</RESULT><RCODE>" + strResultcode + "</RCODE><Message>" + strMessage + "</Message></EVENT>";
                DatabaseLog.LogData(strUsername, "S", "LoginController.cs", Functionname, logxmlData.ToString(), AgentID, strTerminalid, dcSequenceId);
                if (result == true)
                {
                    if (strResultcode == "1")
                    {
                        Status = "01";
                        Error = "";
                        Results = "SUCCESS";
                    }
                    else
                    {
                        Status = "00";
                        Error = strMessage;
                        Results = "Failed";
                    }
                }
                else
                {
                    Status = "00";
                    Error = strMessage;
                    Results = "Failed";
                }

            }
            catch (Exception ex)
            {
                Status = "00";
                if (ex is WebException)
                    Error = "The operation has timed out.";
                else if (ex.Message.ToString().ToUpper().Contains("THE UNDERLYING CONNECTION WAS CLOSED") || (ex.Message.ToString().ToUpper().Contains("THE REMOTE NAME COULD NOT BE RESOLVED")))
                    Error = "Please check the connectivity";
                else if (ex.Message.ToString().ToUpper().Contains("UNABLE TO CONNECT TO THE REMOTE SERVER"))
                    Error = ex.Message.ToString();
                else
                    Error = "Unable to process your request. Please contact support team (#05).";
                logxmlData = "<ERROR><ERRORMESSAGE>" + ex.ToString() + "</ERRORMESSAGE></ERROR>";
                DatabaseLog.LogData(strUsername, "X", "LoginController.cs", Functionname, logxmlData.ToString(), AgentID, strTerminalid, dcSequenceId);

            }
            return Json(new { status = Status, errMsg = Error, Result = Results });
        }


        public ActionResult AgentChangePassword(string strOTPtype, string strTerminalid, string strUsername, string strOTP, string strPassword, string strTerminaltype, string description, string OTPFor)
        {
            string Status = string.Empty;
            string Results = string.Empty;
            string Error = string.Empty;
            string Resultcode = string.Empty;
            string MessageToCustomer = string.Empty;
            string strErrorMsg = string.Empty;
            string IpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            string dcSequenceId = DateTime.Now.ToString("yyMMdd");
            string AgentID = strTerminalid.Remove(strTerminalid.Length - 2);
            string logxmlData = string.Empty;
            try
            {

                string ReqTime = DateTime.Now.ToString();
                string Producttype = ConfigurationManager.AppSettings["Producttype"].ToString();
                description = Producttype + "-ForgetPassword";
                logxmlData = "<EVENT><REQ>AgentChangePassword</REQ><REQTIME>" + ReqTime + "</REQTIME><PAGENAME>LoginController</PAGENAME><AgentId>" + AgentID + "</AgentId><TerminalId>" +
                    strTerminalid + "<TerminalId><Username>" + strUsername + "</Username><OTP>" + strOTP + "</OTP><NewPasswor>" + strPassword + "</></EVENT>";
                DatabaseLog.LogData(strUsername, "S", "LoginController.cs", "AgentChangePassword", logxmlData.ToString(), AgentID, strTerminalid, dcSequenceId);

                Rays_service.RaysService _rays_servers = new Rays_service.RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                bool result = _rays_servers.VerifyOTPwithPasswordUpdate(AgentID, strTerminaltype, strTerminalid, strUsername, strOTPtype, description, OTPFor, IpAddress,
                    dcSequenceId, strPassword, strTerminaltype, strOTP, ref MessageToCustomer, ref strErrorMsg);

                string ResTime = DateTime.Now.ToString();
                logxmlData = "<EVENT><RES>GenerateOTP</RES><RESTIME>" + ResTime + "</RESTIME><RESULT>" + result.ToString() + "</RESULT><RCODE>" + Resultcode + "</RCODE><Message>" + MessageToCustomer + "</Message></EVENT>";
                DatabaseLog.LogData(strUsername, "S", "LoginController.cs", "AgentChangePassword", logxmlData.ToString(), AgentID, strTerminalid, dcSequenceId);
                if (result == true)
                {
                    Status = "01";
                    Error = MessageToCustomer != null && MessageToCustomer != "" ? MessageToCustomer : "Password updated successfully";
                    Results = "SUCCESS";
                }
                else
                {
                    Status = "00";
                    Error = (MessageToCustomer != "") ? MessageToCustomer : "Unable to update password.Please contact support team(#01)";
                    Results = "Failed";
                }

            }
            catch (Exception ex)
            {
                Status = "00";
                if (ex.Message.ToString().ToUpper().Trim().Contains("THE OPERATION HAS TIMED OUT"))
                    Error = ex.Message.ToString();
                else if (ex.Message.ToString().ToUpper().Contains("THE UNDERLYING CONNECTION WAS CLOSED") || (ex.Message.ToString().ToUpper().Contains("THE REMOTE NAME COULD NOT BE RESOLVED")))
                    Error = "Please check the connectivity";
                else if (ex.Message.ToString().ToUpper().Contains("UNABLE TO CONNECT TO THE REMOTE SERVER"))
                    Error = ex.Message.ToString();
                else
                    Error = "Unable to process your request. Please contact support team (#05).";
                logxmlData = "<ERROR><ERRORMESSAGE>" + ex.ToString() + "*" + ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line")).ToString() + "</ERRORMESSAGE></ERROR>";
                DatabaseLog.LogData(strUsername, "X", "LoginController.cs", "AgentChangePassword", logxmlData.ToString(), AgentID, strTerminalid, dcSequenceId);

            }
            return Json(new { status = Status, errMsg = Error, Result = Results });
        }
        public ActionResult checksessionKey(string sessionKeys)
        {

            string stu = string.Empty;
            string msg = string.Empty;
            string results = string.Empty;
            try
            {
                Inplantservice _inplantservice = new Inplantservice();
                _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                string xmldata = "";
                xmldata = "<EVENT><REQUEST>WEBSECURITYCHECK</REQUEST>" +
                                 "<Querystring>" + sessionKeys + "</Querystring>" +
                                   "</EVENT>";


                DatabaseLog.LogData("", "T", "LOGINCONTROLLER", "WEBAPPS_SESSION_CHECK~", xmldata, "", "", "0");
                bool result = _inplantservice.WEBAPPSSESSIONCHECK(ref sessionKeys);

                if (result.Equals(true))
                {
                    stu = "01";
                    results = sessionKeys;
                }
                else
                {
                    stu = "00";
                    msg = "Unable to login. Click on the icon from your desktop to restart the application";
                }
                xmldata = "<EVENT><REQUEST>WEBSECURITYCHECK</REQUEST>" +
                            "<ReferenceString>" + sessionKeys + "</ReferenceString>" +
                             "<BOOLEN>" + result + "</BOOLEN>" +
                              "</EVENT>";

                DatabaseLog.LogData("", "T", "LOGINCONTROLLER", "WEBAPPS_SESSION_CHECK~", xmldata, "", "", "0");

            }
            catch (Exception ex)
            {
                stu = "-01";
                msg = "Problem occured while login,Please contact customer care-(#05)";
                DatabaseLog.LogData("", "X", "LOGINCONTROLLER", "WEBAPPS_SESSION_CHECK", ex.ToString(), "", "", "0");
            }

            return Json(new { Status = stu, Message = msg, Results = results });
        }

        #region AgentRegistration

        public ActionResult AgentRequest()
        {
            return View();
        }

        public ActionResult Registration()
        {
            #region UsageLog
            string PageName = "Agent Registration";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e) { }
            #endregion
            return View();
        }

        public ActionResult RTRegistration()
        {
            #region UsageLog
            string PageName = "Agent Registration";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e) { }
            #endregion
            return View();
        }

        public ActionResult Thankyou()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgentRegistration()
        {
            string Status = string.Empty;
            string Error = string.Empty;
            string Results = string.Empty;
            string strMessage = string.Empty;
            string strResultcode = string.Empty;
            string strStatus = string.Empty;
            string logxmlData = string.Empty;
            string FirstName = string.Empty;
            string LastName = string.Empty;
            string Address1 = string.Empty;
            string Address2 = string.Empty;
            string StateName = string.Empty;
            string CityName = string.Empty;
            string Pincode = string.Empty;
            string MobileNumber = string.Empty;
            string EmailId = string.Empty;
            string LandLine = string.Empty;
            string ProofType = string.Empty;
            string strProof = string.Empty;
            string fname = string.Empty;
            byte[] Proof = new byte[] { };
            string dcSequenceId = DateTime.Now.ToString("yyMMdd");
            try
            {
                #region UsageLog
                string PageName = "Agent Registration";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "INSERT");
                }
                catch (Exception e) { }
                #endregion

                FirstName = Request["FirstName"].ToString();
                LastName = Request["LastName"].ToString();
                Address1 = Request["Address1"].ToString();
                Address2 = Request["Address2"].ToString();
                StateName = Request["StateName"].ToString();
                CityName = Request["CityName"].ToString();
                Pincode = Request["Pincode"].ToString();
                MobileNumber = Request["MobileNumber"].ToString();
                EmailId = Request["EmailId"].ToString();
                LandLine = Request["LandLine"].ToString();
                ProofType = Request["ProofType"] != null ? Request["ProofType"].ToString() : "";

                HttpFileCollectionBase files = Request.Files;
                if (files.Count > 0)
                {
                    HttpPostedFileBase file = files[0];
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file1 = files[i];
                        System.IO.Stream stream = file1.InputStream;
                        System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                        decimal size = Math.Round(((decimal)file1.ContentLength / (decimal)1024), 2);
                        if (size > 800)
                        {
                            return Json(new { status = "00", errMsg = "Image size should be <strong>less than 800KB</strong> ", Result = "Image size should be <strong>less than 800KB</strong> " });
                        }
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file1.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file1.FileName;
                        }
                        fname = Path.Combine(Server.MapPath(@"~/PDF/B2CUserLogo/"), fname);
                        file1.SaveAs(fname);
                        FileStream fs = new FileStream(fname.ToString() + "", FileMode.Open, FileAccess.Read);
                        Proof = new byte[fs.Length];
                        Proof = Base.ReadBitmap2ByteArray(fname.ToString() + "");
                        fs.Close();
                        System.IO.File.Delete(fname.ToString() + "");
                        strProof = Convert.ToBase64String(Proof);
                    }
                }
                string ReqTime = DateTime.Now.ToString();
                logxmlData = "<EVENT><REQ>Registration</REQ><REQTIME>" + ReqTime + "</REQTIME><PAGENAME>LoginController</PAGENAME><FNAME>" + FirstName + "</FNAME><LNAME>" + LastName + "</LNAME><ADD1>" + Address1 + "</ADD1><ADD2>" + Address2 + "</ADD2><STATE>" + StateName + "</STATE><CITY>" + CityName
                    + "</CITY><PIN>" + Pincode + "</PIN><MOB>" + MobileNumber + "</MOB><EMAIL>" + EmailId + "</EMAIL>"
                    + "<PROOFTYPE>" + ProofType + "</PROOFTYPE>" + "</EVENT>";
                DatabaseLog.LogData(FirstName, "E", "LoginController.cs", "AgentRegistration - Request ", logxmlData.ToString(), "", "", dcSequenceId);
                RaysService _RaysService = new RaysService();
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                bool Response = _RaysService.AgentRegistration(FirstName, LastName, LandLine, MobileNumber, Address1, Address2, StateName, CityName, EmailId, Pincode, ref strMessage, ref strResultcode, ref strStatus, ProofType, strProof);

                string ResTime = DateTime.Now.ToString();
                logxmlData = "<EVENT><RES>Registration</RES><RESTIME>" + ResTime + "</RESTIME><RESULT>" + Response + "</RESULT></EVENT>";
                DatabaseLog.LogData(FirstName, "E", "LoginController.cs", "AgentRegistration - Response", logxmlData.ToString(), "", "", dcSequenceId);
                if (Response == true)
                {
                    Status = strStatus;
                    Error = strMessage;
                    Results = strResultcode;
                    if (ConfigurationManager.AppSettings["Appname"].ToString().ToUpper() == "ROUNDTRIP")
                    {
                        #region state city name
                        Hashtable HsStatecode = new Hashtable();
                        Hashtable HsCitycode = new Hashtable();
                        string strStatename = "";
                        string strCityname = "";
                        DataSet dsstate = new DataSet();
                        DataSet dscity = new DataSet();
                        dsstate.ReadXml(HostingEnvironment.MapPath(("~/XML/State.xml").ToString()));
                        dscity.ReadXml(HostingEnvironment.MapPath(("~/XML/City.xml").ToString()));

                        var qryStateName = (from p in dsstate.Tables["STATE"].AsEnumerable()
                                            where p.Field<string>("STATE_ID") == StateName
                                            select p);
                        DataView dvstateCode = qryStateName.AsDataView();
                        if (dvstateCode.Count == 0)
                            strStatename = StateName;
                        else
                        {
                            DataTable dtstateCode = new DataTable();
                            dtstateCode = qryStateName.CopyToDataTable();
                            strStatename = dtstateCode.Rows[0]["STATE_NAME"].ToString();
                            HsStatecode.Add(StateName, strStatename);
                        }
                        //City Name 
                        var qryCityName = (from p in dscity.Tables["CITY"].AsEnumerable()
                                           where p.Field<string>("CITY_ID") == CityName
                                           select p);
                        DataView dvCityCode = qryStateName.AsDataView();
                        if (dvCityCode.Count == 0)
                            strCityname = StateName;
                        else
                        {
                            DataTable dtCityCode = new DataTable();
                            dtCityCode = qryCityName.CopyToDataTable();
                            strCityname = dtCityCode.Rows[0]["CITY_NAME"].ToString();
                            HsCitycode.Add(CityName, strCityname);
                        }
                        #endregion
                        string customercontact = ConfigurationManager.AppSettings["Callcenterno"].ToString();
                        Mailref.MyService _mailservice = new Mailref.MyService();
                        StringBuilder sbTicket = new StringBuilder();
                        string stragentmail = string.Empty;
                        sbTicket.Append("<div>");
                        sbTicket.Append("<p><b>Dear " + FirstName + ",</b></p><br/>");
                        sbTicket.Append("<p>Thank you for registering with Roundtrip.in LLP </p>");
                        sbTicket.Append("<p>We will evaluate you  application and revert back shortly to your registered email ID. You can reach out to us as <b>" + customercontact + "</b> or write to us at <b>support@roundtrip.in</b> </p><br/>");
                        sbTicket.Append("<p style=\"font-size:14px;margin-bottom: 0;\">Thanks and Regards,</p>");
                        sbTicket.Append("<p style=\"font-size: 15px;line-height:1.5;margin-top: 5px;\"><a href=\"https://roundtrip.in\" style=\"text-decoration: none;font-size: 20px;\"><b><span style=\"color:#d60015;\">Round</span><span style=\"color:#170079;\">trip.in</span></b> </a><br> 83, Sydenhams Road, Periamet<br> Chennai – 600003.</p>");
                        sbTicket.Append("</div><br />");

                        stragentmail = sbTicket.ToString();

                        _mailservice.Url = ConfigurationManager.AppSettings["Mailurl"].ToString();
                        string Mailfrom = ConfigurationManager.AppSettings["Supportmail"].ToString();
                        string strLogIPAddress = Base.GetComputer_IP();
                        string strPortNo = ConfigurationManager.AppSettings["PortNo"].ToString();
                        string strMailusername = ConfigurationManager.AppSettings["MailUsername"].ToString();
                        string strNetworkusername = ConfigurationManager.AppSettings["NetworkUsername"].ToString();
                        string strMailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
                        string strHostAddress = ConfigurationManager.AppSettings["HostAddress"].ToString();
                        bool blnEnableSSL = ConfigurationManager.AppSettings["EnableSsl"].ToString() == "false" ? false : true;

                        string strAgentMail = _mailservice.SendMailSingleTicket("", "", FirstName, strLogIPAddress, "W", dcSequenceId,
                                    EmailId, "", "Registrantion Successfull", stragentmail, "", "", strMailusername, strMailPassword, strHostAddress, strPortNo,
                                    blnEnableSSL, strMailusername, "");

                        StringBuilder sbclient = new StringBuilder();
                        string strclientmail = string.Empty;
                        sbclient.Append("<div>");
                        sbclient.Append("<h4 style=\"color: #2e3246; font-size: 14px;\">Dear Roundtrip Support Team,</h4>");
                        sbclient.Append("<p>There is new Agency registration with the following details. Please action as necessary </p>");
                        sbclient.Append("<span style=\"color:#d60015;border-bottom: 2px solid #d60015;padding-bottom: 8px;\">Agent</span><span style=\"color:#170079;border-bottom: 2px solid #170079;padding-bottom: 8px;\"> Details</span>");
                        sbclient.Append("<p>First Name : " + FirstName + "</p>");
                        sbclient.Append("<p>Last Name  : " + LastName + "</p>");
                        sbclient.Append("<p>Landline   : " + LandLine + "</p>");
                        sbclient.Append("<p>Mobile     : " + MobileNumber + "</p>");
                        sbclient.Append("<p>Email  Id  : " + EmailId + "</p>");
                        sbclient.Append("<p>Address    : " + Address1 + "</p>");
                        sbclient.Append("<p>LandMark   : " + Address2 + "</p>");
                        sbclient.Append("<p>Country    : INDIA</p>");
                        sbclient.Append("<p>State      : " + strStatename + "</p>");
                        sbclient.Append("<p>City       : " + strCityname + "</p>");
                        sbclient.Append("<p>Postlcode  : " + Pincode + "</p>");
                        sbclient.Append("<p>Proof Type : " + ProofType + "</p>");
                        sbclient.Append("</div><br /><br />");

                        strclientmail = sbclient.ToString();
                        string clientmailresult = _mailservice.SendMailSingleTicket("", "", FirstName, strLogIPAddress, "W", dcSequenceId,
                                    Mailfrom, "", "Agent Request", strclientmail, "", "", strMailusername, strMailPassword, strHostAddress, strPortNo,
                                    blnEnableSSL, strMailusername, "");
                    }
                }
                else
                {
                    Status = strStatus;
                    Error = !string.IsNullOrEmpty(strMessage) ? strMessage : "Unable to process your Request.. Please contact support team.";
                    Results = strResultcode;
                }
            }
            catch (Exception ex)
            {
                Status = "00";
                if (ex.Message.ToString().ToUpper().Trim().Contains("THE OPERATION HAS TIMED OUT"))
                    Error = ex.Message.ToString();
                else if (ex.Message.ToString().ToUpper().Contains("THE UNDERLYING CONNECTION WAS CLOSED") || (ex.Message.ToString().ToUpper().Contains("THE REMOTE NAME COULD NOT BE RESOLVED")))
                    Error = "Please check the connectivity";
                else if (ex.Message.ToString().ToUpper().Contains("UNABLE TO CONNECT TO THE REMOTE SERVER"))
                    Error = ex.Message.ToString();
                else
                    Error = "Unable to process your request. Please contact support team (#05).";
                logxmlData = "<ERROR><ERRORMESSAGE>" + ex.ToString() + "*" + ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line")).ToString() + "</ERRORMESSAGE></ERROR>";
                DatabaseLog.LogData(FirstName, "S", "LoginController.cs", "AgentRegistration", logxmlData.ToString(), "", "", dcSequenceId);
            }
            return Json(new { status = Status, errMsg = Error, Result = Results });
        }

        #endregion

        #region Contact Us 
        public ActionResult Contactus()
        {
            return View();
        }

        #endregion

        #region change password
        public ActionResult UpdatePassword(string strOldpwd, string strNewpwd, string strTerminalId, string strUsername)
        {

            ArrayList result = new ArrayList(2);
            RaysService _RaysService = new RaysService();
            DataSet ds_res = new DataSet();
            DataSet ds_rr = new DataSet();

            string dsLogin = string.Empty;
            string Ipaddress = Base.GetComputer_IP();
            string sequnceID = DateTime.Now.ToString("ddMMyyyyhhmm");
            string strErrorMsg = string.Empty;
            string strAgentId = (strTerminalId.Length > 12) ? strTerminalId.Substring(0, 12).ToString().Trim() : "";

            result.Add("");
            result.Add("");

            int error = 0;
            int responce = 1;
            try
            {
                #region UsageLog
                string PageName = "Change Password";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "UPDATE");
                }
                catch (Exception e) { }
                #endregion
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                dsLogin = _RaysService.changePassword(strAgentId, strTerminalId, strUsername, strOldpwd, strNewpwd, "W", Ipaddress, Convert.ToDecimal(sequnceID), ref strErrorMsg, "LoginController.cs", "UpdatePassword");

                if (dsLogin == "1")
                {
                    result[responce] = "Your password is sucessfully Updated!";
                }
                else
                {
                    result[error] = "Old password wrong!";
                    string log = "<Oldpassword>" + strOldpwd + "</Oldpassword>";
                    log += "<newpassword>" + strNewpwd + "</newpassword>";
                    log += "<tr_id>" + strTerminalId + "</tr_id>";
                    DatabaseLog.LogData(Session["username"].ToString(), "E", "LoginController.cs", "UpdatePassword", log.ToString(), "", strTerminalId, sequnceID);
                }

            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "LoginController.cs", "UpdatePassword", ex.ToString(), "", strTerminalId, sequnceID);
                result[error] = "Problem occured.Please contact customer support";
            }

            return Json(new { Status = "", Message = "", Result = result });
        }
        #endregion

        public bool BSA_AssignSession(string strTerminalID, string strUsername, string strPassword, DataSet dsBCI_LOGIN, string strTerminalType)
        {
            try
            {

                string strTerminaltype = !string.IsNullOrEmpty(strTerminalType) ? strTerminalType : ConfigurationManager.AppSettings["TerminalType"].ToString();
                string IPAddress = Base.GetComputer_IP();
                string agenttype = string.Empty;

                //Loginsubmit(strTerminalID, strUsername, strPassword, strTerminaltype);

                System.Web.HttpContext.Current.Session.Add("agencyname", dsBCI_LOGIN.Tables[0].Rows[0]["AGENCY_NAME"].ToString());
                System.Web.HttpContext.Current.Session.Add("agentid", dsBCI_LOGIN.Tables[0].Rows[0]["LGN_AGENT_ID"].ToString().Trim().ToUpper());
                System.Web.HttpContext.Current.Session.Add("terminalid", strTerminalID);
                System.Web.HttpContext.Current.Session.Add("username", strUsername);
                System.Web.HttpContext.Current.Session.Add("PWD", strPassword);
                System.Web.HttpContext.Current.Session.Add("POS_ID", dsBCI_LOGIN.Tables[0].Rows[0]["LGN_AGENT_ID"]);
                System.Web.HttpContext.Current.Session.Add("POS_TID", dsBCI_LOGIN.Tables[0].Rows[0]["LGN_TERMINAL_ID"]);
                System.Web.HttpContext.Current.Session.Add("privilage", dsBCI_LOGIN.Tables[0].Rows[0]["LGN_TERMINAL_PREVILAGE"].ToString());
                System.Web.HttpContext.Current.Session.Add("branchid", dsBCI_LOGIN.Tables[0].Columns.Contains("BCH_BRANCH_ID") ? dsBCI_LOGIN.Tables[0].Rows[0]["BCH_BRANCH_ID"].ToString() : dsBCI_LOGIN.Tables[0].Rows[0]["AGN_BRANCHID"].ToString());
                System.Web.HttpContext.Current.Session.Add("TERMINALTYPE", strTerminaltype);
                System.Web.HttpContext.Current.Session.Add("travraysws_url", ConfigurationManager.AppSettings["APPS_SELECT_URL"].ToString());
                System.Web.HttpContext.Current.Session.Add("travraysws_vdir", ConfigurationManager.AppSettings["APPS_URL"].ToString());
                System.Web.HttpContext.Current.Session.Add("Allowappcurrency", dsBCI_LOGIN.Tables[0].Columns.Contains("Currency") && dsBCI_LOGIN.Tables[0].Rows[0]["Currency"].ToString().Trim() != "" ? dsBCI_LOGIN.Tables[0].Rows[0]["Currency"].ToString().Trim() : ConfigurationManager.AppSettings["currency"].ToString());
                System.Web.HttpContext.Current.Session.Add("ALLOW_RETRIEVE", "N");//dsBCI_LOGIN.Tables[0].Columns.Contains("ALLOW_RETRIEVE") ? dsBCI_LOGIN.Tables[0].Rows[0]["ALLOW_RETRIEVE"].ToString().Trim() : "");
                System.Web.HttpContext.Current.Session.Add("sequenceid", dsBCI_LOGIN.Tables[0].Columns.Contains("SEQUENCE_ID") ? dsBCI_LOGIN.Tables[0].Rows[0]["SEQUENCE_ID"].ToString() : DateTime.Now.ToString("yyyyMMdd"));
                agenttype = dsBCI_LOGIN.Tables[0].Columns.Contains("AGENT_TYPE") ? dsBCI_LOGIN.Tables[0].Rows[0]["AGENT_TYPE"].ToString() : dsBCI_LOGIN.Tables[0].Rows[0]["AGN_TYPE"].ToString();
                System.Web.HttpContext.Current.Session.Add("accessimage", "");   //dsBCI_LOGIN.Tables[0].Rows[0]["AGN_ACCESS_IMG"].ToString());
                System.Web.HttpContext.Current.Session.Add("multicity", "N");
                System.Web.HttpContext.Current.Session.Add("ticketing", "Y");
                System.Web.HttpContext.Current.Session.Add("block", "N");
                System.Web.HttpContext.Current.Session.Add("ALLOW_CREDIT", "N");
                System.Web.HttpContext.Current.Session.Add("ALLOW_PG", "Y");
                string strAllowTopup = dsBCI_LOGIN.Tables[0].Columns.Contains("PAYMENT_RIGHTS") && dsBCI_LOGIN.Tables[0].Rows[0]["PAYMENT_RIGHTS"].ToString().Trim() != "" && dsBCI_LOGIN.Tables[0].Rows[0]["PAYMENT_RIGHTS"].ToString().Trim().Contains("T") ? "Y" : "N";
                System.Web.HttpContext.Current.Session.Add("ALLOW_TOPUP", System.Web.HttpContext.Current.Session["CustomerLogin"] != null && System.Web.HttpContext.Current.Session["CustomerLogin"].ToString() == "Y" ? strAllowTopup : "N");
                System.Web.HttpContext.Current.Session.Add("ALLOW_PASSTHROUGH", "N");

                #region Credit Shell View Balance Access
                DataSet dsProductAccess = new DataSet();
                if (System.IO.File.Exists(Server.MapPath("~/XML/ProductAccess.xml")))
                {
                    dsProductAccess.ReadXml(Server.MapPath("~/XML/ProductAccess.xml"));
                    if (dsProductAccess != null && dsProductAccess.Tables.Count > 0 && dsProductAccess.Tables.Contains("CREDITSHELLPNRVIEW") && dsProductAccess.Tables["CREDITSHELLPNRVIEW"].Rows.Count > 0 &&
                        (dsProductAccess.Tables["CREDITSHELLPNRVIEW"].Rows[0]["AGENTID"].ToString().Contains(dsBCI_LOGIN.Tables[0].Rows[0]["LGN_AGENT_ID"].ToString().Trim().ToUpper()) || dsProductAccess.Tables["CREDITSHELLPNRVIEW"].Rows[0]["AGENTID"].ToString().Trim().ToUpper().Contains("ALL")))
                    {
                        System.Web.HttpContext.Current.Session.Add("CREDITSHELL", "Y");
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session.Add("CREDITSHELL", "N");
                    }
                }
                else
                {
                    System.Web.HttpContext.Current.Session.Add("CREDITSHELL", "N");
                }
                #endregion

                string path = System.Web.HttpContext.Current.Server.MapPath("~/XML/LOGIN.xml");
                string LoginDetXml = System.IO.File.ReadAllText(path);
                dsBCI_LOGIN = new DataSet();
                dsBCI_LOGIN.ReadXml(new XmlTextReader(new StringReader(LoginDetXml)));

                System.Web.HttpContext.Current.Session.Add("blockpnr", dsBCI_LOGIN.Tables[0].Rows[0]["BlockPnrCategories"].ToString());
                System.Web.HttpContext.Current.Session.Add("creditacc", dsBCI_LOGIN.Tables[0].Rows[0]["CreditAccCategories"].ToString());
                System.Web.HttpContext.Current.Session.Add("pg_Commonservice", dsBCI_LOGIN.Tables[0].Rows[0]["PG_common_servicecharge"].ToString());
                System.Web.HttpContext.Current.Session.Add("PRODUCT_CODE", dsBCI_LOGIN.Tables[0].Rows[0]["PRODUCT_CODE"].ToString());
                System.Web.HttpContext.Current.Session.Add("tripConfig", dsBCI_LOGIN.Tables[0].Rows[0]["TRIPCONFIG"].ToString());// + "|MD"
                #region Threadkeyvalues
                System.Web.HttpContext.Current.Session.Add("threadkey", dsBCI_LOGIN.Tables[0].Rows[0]["Thread"].ToString());
                System.Web.HttpContext.Current.Session.Add("multithreadkey", dsBCI_LOGIN.Tables[0].Rows[0]["MultiThread"].ToString());
                System.Web.HttpContext.Current.Session.Add("fscthreadkey", dsBCI_LOGIN.Tables[0].Rows[0]["FscThread"].ToString());
                System.Web.HttpContext.Current.Session.Add("lccthreadkey", dsBCI_LOGIN.Tables[0].Rows[0]["LccThread"].ToString());
                System.Web.HttpContext.Current.Session.Add("nearthread", dsBCI_LOGIN.Tables[0].Rows[0]["NearThread"].ToString());
                if (dsBCI_LOGIN.Tables[0].Rows[0]["NewThreadC"].ToString() != "" && dsBCI_LOGIN.Tables[0].Rows[0]["NewThreadC"].ToString() != null)
                {
                    System.Web.HttpContext.Current.Session.Add("multithread", dsBCI_LOGIN.Tables[0].Rows[0]["NewThreadC"].ToString());
                }
                else
                {
                    System.Web.HttpContext.Current.Session.Add("multithread", "");
                }
                if (dsBCI_LOGIN.Tables[0].Columns.Contains("doublethread") == true && dsBCI_LOGIN.Tables[0].Rows[0]["doublethread"].ToString() != "" && dsBCI_LOGIN.Tables[0].Rows[0]["doublethread"].ToString() != null)
                {
                    System.Web.HttpContext.Current.Session.Add("doublethread", dsBCI_LOGIN.Tables[0].Rows[0]["doublethread"].ToString());
                }
                else
                {
                    System.Web.HttpContext.Current.Session.Add("doublethread", ConfigurationManager.AppSettings["Doublethread"].ToString());
                }
                System.Web.HttpContext.Current.Session.Add("SFCthread", dsBCI_LOGIN.Tables[0].Rows[0]["SFC"].ToString());
                #endregion

                #region Domesticthreadykey
                System.Web.HttpContext.Current.Session.Add("threadkey_dom", dsBCI_LOGIN.Tables[0].Columns.Contains("ThreadD") ? dsBCI_LOGIN.Tables[0].Rows[0]["ThreadD"].ToString() : dsBCI_LOGIN.Tables[0].Rows[0]["Thread"].ToString());
                System.Web.HttpContext.Current.Session.Add("multithreadkey_dom", dsBCI_LOGIN.Tables[0].Columns.Contains("MultiThreadD") ? dsBCI_LOGIN.Tables[0].Rows[0]["MultiThreadD"].ToString() : dsBCI_LOGIN.Tables[0].Rows[0]["MultiThread"].ToString());
                System.Web.HttpContext.Current.Session.Add("fscthreadkey_dom", dsBCI_LOGIN.Tables[0].Columns.Contains("FscThreadD") ? dsBCI_LOGIN.Tables[0].Rows[0]["FscThreadD"].ToString() : dsBCI_LOGIN.Tables[0].Rows[0]["FscThread"].ToString());
                System.Web.HttpContext.Current.Session.Add("lccthreadkey_dom", dsBCI_LOGIN.Tables[0].Columns.Contains("LccThreadD") ? dsBCI_LOGIN.Tables[0].Rows[0]["LccThreadD"].ToString() : dsBCI_LOGIN.Tables[0].Rows[0]["LccThread"].ToString());
                System.Web.HttpContext.Current.Session.Add("nearthread_dom", dsBCI_LOGIN.Tables[0].Columns.Contains("NearThread_dom") ? dsBCI_LOGIN.Tables[0].Rows[0]["NearThread_dom"].ToString() : dsBCI_LOGIN.Tables[0].Rows[0]["NearThread"].ToString());
                System.Web.HttpContext.Current.Session.Add("newthread_dom", dsBCI_LOGIN.Tables[0].Columns.Contains("NewThreadD") ? dsBCI_LOGIN.Tables[0].Rows[0]["NewThreadD"].ToString() : dsBCI_LOGIN.Tables[0].Rows[0]["NewThreadC"].ToString());
                System.Web.HttpContext.Current.Session.Add("SFCthread_dom", dsBCI_LOGIN.Tables[0].Columns.Contains("SFCD") ? dsBCI_LOGIN.Tables[0].Rows[0]["SFCD"].ToString() : dsBCI_LOGIN.Tables[0].Rows[0]["SFC"].ToString());
                #endregion

                #region NewFareThread
                System.Web.HttpContext.Current.Session.Add("StudentThreadkey", (dsBCI_LOGIN.Tables[0].Columns.Contains("STUDENTFARE")) ? ((dsBCI_LOGIN.Tables[0].Rows[0]["STUDENTFARE"] != null) ? dsBCI_LOGIN.Tables[0].Rows[0]["STUDENTFARE"].ToString() : "") : "");
                System.Web.HttpContext.Current.Session.Add("ArmyThreadkey", (dsBCI_LOGIN.Tables[0].Columns.Contains("ARMEDTFARE")) ? ((dsBCI_LOGIN.Tables[0].Rows[0]["ARMEDTFARE"] != null) ? dsBCI_LOGIN.Tables[0].Rows[0]["ARMEDTFARE"].ToString() : "") : "");
                System.Web.HttpContext.Current.Session.Add("SRCitizenThreadkey", (dsBCI_LOGIN.Tables[0].Columns.Contains("SRCITIZENTFARE")) ? ((dsBCI_LOGIN.Tables[0].Rows[0]["SRCITIZENTFARE"] != null) ? dsBCI_LOGIN.Tables[0].Rows[0]["SRCITIZENTFARE"].ToString() : "") : "");
                System.Web.HttpContext.Current.Session.Add("StudentfareMsg", (dsBCI_LOGIN.Tables[0].Columns.Contains("STUDENTFAREDISPMSG")) ? ((dsBCI_LOGIN.Tables[0].Rows[0]["STUDENTFAREDISPMSG"] != null) ? dsBCI_LOGIN.Tables[0].Rows[0]["STUDENTFAREDISPMSG"].ToString() : "") : "");
                System.Web.HttpContext.Current.Session.Add("ArmyfareMsg", (dsBCI_LOGIN.Tables[0].Columns.Contains("ARMEDTFAREDISPMSG")) ? ((dsBCI_LOGIN.Tables[0].Rows[0]["ARMEDTFAREDISPMSG"] != null) ? dsBCI_LOGIN.Tables[0].Rows[0]["ARMEDTFAREDISPMSG"].ToString() : "") : "");
                System.Web.HttpContext.Current.Session.Add("SRCitizenfareMsg", (dsBCI_LOGIN.Tables[0].Columns.Contains("SRCITIZENTFAREDISPMSG")) ? ((dsBCI_LOGIN.Tables[0].Rows[0]["SRCITIZENTFAREDISPMSG"] != null) ? dsBCI_LOGIN.Tables[0].Rows[0]["SRCITIZENTFAREDISPMSG"].ToString() : "") : "");
                #endregion

                System.Web.HttpContext.Current.Session.Add("CORPORATETHREAD", dsBCI_LOGIN.Tables[0].Columns.Contains("CORPORATETHREAD") ? dsBCI_LOGIN.Tables[0].Rows[0]["CORPORATETHREAD"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("RETAILTHREAD", dsBCI_LOGIN.Tables[0].Columns.Contains("RETAILTHREAD") ? dsBCI_LOGIN.Tables[0].Rows[0]["RETAILTHREAD"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("VIATHREAD", dsBCI_LOGIN.Tables[0].Columns.Contains("VIATHREAD") ? dsBCI_LOGIN.Tables[0].Rows[0]["VIATHREAD"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("MACADDRESS", dsBCI_LOGIN.Tables[0].Columns.Contains("TRAIN_MAC_ID") ? string.IsNullOrEmpty(dsBCI_LOGIN.Tables[0].Rows[0]["TRAIN_MAC_ID"].ToString()) ? "" : dsBCI_LOGIN.Tables[0].Rows[0]["TRAIN_MAC_ID"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("IRCTCUSERNAME", dsBCI_LOGIN.Tables[0].Columns.Contains("TRAIN_USER_ID") ? string.IsNullOrEmpty(dsBCI_LOGIN.Tables[0].Rows[0]["TRAIN_USER_ID"].ToString()) ? "" : dsBCI_LOGIN.Tables[0].Rows[0]["TRAIN_USER_ID"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("ipAddress", IPAddress);
                System.Web.HttpContext.Current.Session.Add("multiclass", strTerminaltype.Trim() == "T" ? "Y" : dsBCI_LOGIN.Tables[0].Rows[0]["ALLOW_MULTICLASS"].ToString());
                System.Web.HttpContext.Current.Session.Add("airline", dsBCI_LOGIN.Tables[0].Rows[0]["AIRLINE"].ToString());
                System.Web.HttpContext.Current.Session.Add("hotel", dsBCI_LOGIN.Tables[0].Rows[0]["HOTEL"].ToString());
                System.Web.HttpContext.Current.Session.Add("bus", dsBCI_LOGIN.Tables[0].Rows[0]["BUS"].ToString());
                System.Web.HttpContext.Current.Session.Add("train", dsBCI_LOGIN.Tables[0].Rows[0]["TRAIN"].ToString());
                System.Web.HttpContext.Current.Session.Add("Mobrech", dsBCI_LOGIN.Tables[0].Rows[0]["MOB"].ToString());
                System.Web.HttpContext.Current.Session.Add("Visa", dsBCI_LOGIN.Tables[0].Rows[0]["VISA"].ToString());
                System.Web.HttpContext.Current.Session.Add("Activity", (dsBCI_LOGIN.Tables[0].Columns.Contains("ACT") == true ? dsBCI_LOGIN.Tables[0].Rows[0]["ACT"].ToString() : ""));
                System.Web.HttpContext.Current.Session.Add("MISC", (dsBCI_LOGIN.Tables[0].Columns.Contains("MISC") == true ? dsBCI_LOGIN.Tables[0].Rows[0]["MISC"].ToString() : "N"));
                System.Web.HttpContext.Current.Session.Add("CAR", (dsBCI_LOGIN.Tables[0].Columns.Contains("CAR") == true ? dsBCI_LOGIN.Tables[0].Rows[0]["CAR"].ToString() : "N"));
                System.Web.HttpContext.Current.Session.Add("TITOS", (dsBCI_LOGIN.Tables[0].Columns.Contains("TITOS") == true ? dsBCI_LOGIN.Tables[0].Rows[0]["TITOS"].ToString() : "N"));
                System.Web.HttpContext.Current.Session.Add("ANCILLARY", (dsBCI_LOGIN.Tables[0].Columns.Contains("ANCILLARY") == true ? dsBCI_LOGIN.Tables[0].Rows[0]["ANCILLARY"].ToString() : "N"));
                System.Web.HttpContext.Current.Session.Add("THEMEPARK", (dsBCI_LOGIN.Tables[0].Columns.Contains("THEMEPARK") == true ? dsBCI_LOGIN.Tables[0].Rows[0]["THEMEPARK"].ToString() : "N"));
                System.Web.HttpContext.Current.Session.Add("QTICKETING", (dsBCI_LOGIN.Tables[0].Columns.Contains("VIEW_Q_TICKETING") == true ? dsBCI_LOGIN.Tables[0].Rows[0]["VIEW_Q_TICKETING"].ToString() : "N"));
                System.Web.HttpContext.Current.Session.Add("UTILITY", (dsBCI_LOGIN.Tables[0].Columns.Contains("MUA") ? dsBCI_LOGIN.Tables[0].Rows[0]["MUA"].ToString() : "N"));
                System.Web.HttpContext.Current.Session.Add("MONEYTRANSFER", dsBCI_LOGIN.Tables[0].Columns.Contains("MTF") ? dsBCI_LOGIN.Tables[0].Rows[0]["MTF"].ToString() : "N");
                System.Web.HttpContext.Current.Session.Add("ALLOWGDS", dsBCI_LOGIN.Tables[0].Columns.Contains("ALLOW_GDS") ? dsBCI_LOGIN.Tables[0].Rows[0]["ALLOW_GDS"].ToString() : "N");
                System.Web.HttpContext.Current.Session.Add("Farecalander", dsBCI_LOGIN.Tables[0].Rows[0]["ALLOW_FARECALENDER"].ToString());
                System.Web.HttpContext.Current.Session.Add("cntrlpanelrits", dsBCI_LOGIN.Tables[0].Rows[0]["CTRLRIGHTS"].ToString());
                System.Web.HttpContext.Current.Session.Add("touchpointentry", "1");
                System.Web.HttpContext.Current.Session.Add("alowcredit", dsBCI_LOGIN.Tables[0].Rows[0]["ALLOW_CREDIT"].ToString());
                System.Web.HttpContext.Current.Session.Add("mailfromadd", dsBCI_LOGIN.Tables[0].Rows[0]["MailFrom"].ToString());
                System.Web.HttpContext.Current.Session.Add("pg_charge", dsBCI_LOGIN.Tables[0].Rows[0]["PG_SERVICE_CHARGE"].ToString());

                System.Web.HttpContext.Current.Session.Add("ALLOW_CORP_FARE", "N");
                if (dsBCI_LOGIN.Tables[0].Columns.Contains("pgminamt"))
                {
                    System.Web.HttpContext.Current.Session.Add("payfeeprecent", dsBCI_LOGIN.Tables[0].Rows[0]["pgminamt"].ToString());
                }
                else
                {
                    System.Web.HttpContext.Current.Session.Add("payfeeprecent", ConfigurationManager.AppSettings["payfeeprecent"]);
                }
                if (!string.IsNullOrEmpty(dsBCI_LOGIN.Tables[0].Rows[0]["VIEW_COMMISSION"].ToString()))
                {
                    string[] Com = dsBCI_LOGIN.Tables[0].Rows[0]["VIEW_COMMISSION"].ToString().Split('|');
                    System.Web.HttpContext.Current.Session.Add("commission", Com[2] == "1" ? "Y" : "N");
                }
                else
                {
                    System.Web.HttpContext.Current.Session.Add("commission", "N");
                }
                System.Web.HttpContext.Current.Session.Add("GMTTIME", ConfigurationManager.AppSettings["GMTTIME"].ToString());
                System.Web.HttpContext.Current.Session.Add("MasteragentId", dsBCI_LOGIN.Tables[0].Rows[0]["LGN_AGENT_ID"].ToString());
                System.Web.HttpContext.Current.Session.Add("Agentsublogopath", "");
                System.Web.HttpContext.Current.Session.Add("Agentmainlogopath", strAgentLogoPath + "/LoginLogo/" + "/logo.png?");
                System.Web.HttpContext.Current.Session.Add("AGN_TDSPERCENTAGE", (dsBCI_LOGIN.Tables[0].Columns.Contains("TDS_PERCENTAGE") == true) ? dsBCI_LOGIN.Tables[0].Rows[0]["TDS_PERCENTAGE"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("privilage", dsBCI_LOGIN.Tables[0].Rows[0]["LGN_TERMINAL_PREVILAGE"].ToString());
                System.Web.HttpContext.Current.Session.Add("agenttype", agenttype);
                System.Web.HttpContext.Current.Session.Add("agencyaddress", dsBCI_LOGIN.Tables[0].Rows[0]["AGENCY_NAMEADDRESS"].ToString());
                System.Web.HttpContext.Current.Session.Add("AgencyPaymentDetails", dsBCI_LOGIN.Tables[0].Rows[0]["AgencyPaymentDetails"].ToString());
                System.Web.HttpContext.Current.Session.Add("Agreement", dsBCI_LOGIN.Tables[0].Rows[0]["AGREEMENT"].ToString());
                System.Web.HttpContext.Current.Session.Add("AgencyDetailsformail", dsBCI_LOGIN.Tables[0].Columns.Contains("Agency_Details") ? dsBCI_LOGIN.Tables[0].Rows[0]["Agency_Details"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("BranchagentId", dsBCI_LOGIN.Tables[0].Columns.Contains("Branchagentid") ? dsBCI_LOGIN.Tables[0].Rows[0]["Branchagentid"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("Agentsalesperson", (dsBCI_LOGIN.Tables[0].Columns.Contains("SALESMAN1") ? dsBCI_LOGIN.Tables[0].Rows[0]["SALESMAN1"].ToString() : "") + "|" + (dsBCI_LOGIN.Tables[0].Columns.Contains("SALESMAN2") ? dsBCI_LOGIN.Tables[0].Rows[0]["SALESMAN2"].ToString() : "") + "|" + (dsBCI_LOGIN.Tables[0].Columns.Contains("SALESMAN3") ? dsBCI_LOGIN.Tables[0].Rows[0]["SALESMAN3"].ToString() : ""));
                System.Web.HttpContext.Current.Session.Add("Branchcountry", dsBCI_LOGIN.Tables[0].Columns.Contains("BCH_COUNTRY") ? (dsBCI_LOGIN.Tables[0].Rows[0]["BCH_COUNTRY"].ToString() == "" ? ConfigurationManager.AppSettings["Logincountry"].ToString() : dsBCI_LOGIN.Tables[0].Rows[0]["BCH_COUNTRY"].ToString()) : ConfigurationManager.AppSettings["Logincountry"].ToString());
                System.Web.HttpContext.Current.Session.Add("Allowreports", (dsBCI_LOGIN.Tables.Count > 2 && dsBCI_LOGIN.Tables[2].Columns.Contains("WEB_RIGHTS")) ? dsBCI_LOGIN.Tables[2].Rows[0]["WEB_RIGHTS"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("Agent_CommissionRef", dsBCI_LOGIN.Tables[0].Columns.Contains("Commission") ? dsBCI_LOGIN.Tables[0].Rows[0]["Commission"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("Agent_MarkupRef", dsBCI_LOGIN.Tables[0].Columns.Contains("Markup") ? dsBCI_LOGIN.Tables[0].Rows[0]["Markup"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("Agent_ServiceTaxRef", dsBCI_LOGIN.Tables[0].Columns.Contains("ServiceTax") ? dsBCI_LOGIN.Tables[0].Rows[0]["ServiceTax"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("Agent_ServiceRef", dsBCI_LOGIN.Tables[0].Columns.Contains("ServiceCharge") ? dsBCI_LOGIN.Tables[0].Rows[0]["ServiceCharge"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("AgentTerminalCount", dsBCI_LOGIN.Tables[0].Columns.Contains("AgentTerminalCount") ? dsBCI_LOGIN.Tables[0].Rows[0]["AgentTerminalCount"].ToString() : "1");
                System.Web.HttpContext.Current.Session.Add("ShowCommission", dsBCI_LOGIN.Tables[0].Columns.Contains("SHOWCOMMISSION") ? dsBCI_LOGIN.Tables[0].Rows[0]["SHOWCOMMISSION"].ToString() : "1");
                System.Web.HttpContext.Current.Session.Add("ALLOW_DOM1G", dsBCI_LOGIN.Tables[0].Columns.Contains("DOM_1G_AIRLINES") && dsBCI_LOGIN.Tables[0].Rows[0]["DOM_1G_AIRLINES"].ToString().Trim() != "" ? dsBCI_LOGIN.Tables[0].Rows[0]["DOM_1G_AIRLINES"].ToString().Trim() : "");
                System.Web.HttpContext.Current.Session.Add("ALLOW_INT1G", dsBCI_LOGIN.Tables[0].Columns.Contains("INT_1G_AIRLINES") && dsBCI_LOGIN.Tables[0].Rows[0]["INT_1G_AIRLINES"].ToString().Trim() != "" ? dsBCI_LOGIN.Tables[0].Rows[0]["INT_1G_AIRLINES"].ToString().Trim() : "");
                System.Web.HttpContext.Current.Session.Add("ALLOW_DOM1S", dsBCI_LOGIN.Tables[0].Columns.Contains("DOM_1S_AIRLINES") ? dsBCI_LOGIN.Tables[0].Rows[0]["DOM_1S_AIRLINES"].ToString().Trim() : ConfigurationManager.AppSettings["SaberAirlines"].ToString());
                System.Web.HttpContext.Current.Session.Add("ALLOW_INT1S", dsBCI_LOGIN.Tables[0].Columns.Contains("INT_1S_AIRLINES") ? dsBCI_LOGIN.Tables[0].Rows[0]["INT_1S_AIRLINES"].ToString().Trim() : ConfigurationManager.AppSettings["SaberAirlines"].ToString());
                System.Web.HttpContext.Current.Session.Add("Labourthread", dsBCI_LOGIN.Tables[0].Columns.Contains("Labourthread") ? dsBCI_LOGIN.Tables[0].Rows[0]["Labourthread"].ToString().Trim() : ConfigurationManager.AppSettings["Labourthread"].ToString());
                System.Web.HttpContext.Current.Session.Add("LabourfareAirline", dsBCI_LOGIN.Tables[0].Columns.Contains("LabourfareAirline") ? dsBCI_LOGIN.Tables[0].Rows[0]["LabourfareAirline"].ToString().Trim() : ConfigurationManager.AppSettings["LabourfareAirline"].ToString());
                System.Web.HttpContext.Current.Session.Add("AgentAlterMail", dsBCI_LOGIN.Tables[0].Columns.Contains("AgentAlterMail") ? dsBCI_LOGIN.Tables[0].Rows[0]["AgentAlterMail"].ToString().Trim() : "");
                System.Web.HttpContext.Current.Session.Add("ALLOW_TICKET_TOCANCEL", dsBCI_LOGIN.Tables[0].Columns.Contains("ALLOW_TICKET_TOCANCEL") ? dsBCI_LOGIN.Tables[0].Rows[0]["ALLOW_TICKET_TOCANCEL"].ToString().Trim() : "");
                System.Web.HttpContext.Current.Session.Add("Agent_FSCCommission", dsBCI_LOGIN.Tables[0].Columns.Contains("FSCCommission") ? dsBCI_LOGIN.Tables[0].Rows[0]["FSCCommission"].ToString().Trim() : "");
                string strAgentEmailid = dsBCI_LOGIN.Tables[0].Columns.Contains("AGENT_EMAIL_ID") ? dsBCI_LOGIN.Tables[0].Rows[0]["AGENT_EMAIL_ID"].ToString() : (dsBCI_LOGIN.Tables[0].Columns.Contains("EMAIL_ID") ? dsBCI_LOGIN.Tables[0].Rows[0]["EMAIL_ID"].ToString() : "");
                System.Web.HttpContext.Current.Session.Add("AGENT_MAILID", strAgentEmailid);
                if (dsBCI_LOGIN.Tables[0].Columns.Contains("PROCESSING_IMAGE") && dsBCI_LOGIN.Tables[0].Rows[0]["PROCESSING_IMAGE"] != null && dsBCI_LOGIN.Tables[0].Rows[0]["PROCESSING_IMAGE"].ToString() != "")
                {
                    Session.Add("ProcessingImage", ConfigurationManager.AppSettings["AgentLogoPath"].ToString() + "/Process/" + dsBCI_LOGIN.Tables[0].Rows[0]["PROCESSING_IMAGE"].ToString());
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData("", "X", "LOGINCONTROLLER", "B2C Session Assign", "<EVENT>" + ex.ToString() + "</EVENT>", "", "", "0");
            }
            return true;
        }
    }
}

