using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STSTRAVRAYS.Models;
using STSTRAVRAYS.Rays_service;
using STSTRAVRAYS.InplantService;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Web;

namespace STSTRAVRAYS.Controllers
{
    public class B2CController : LoginController
    {

        RaysService _RaysService = new RaysService();
        Mailref.MyService _MailService = new Mailref.MyService();
        Inplantservice _InplantService = new Inplantservice();
        Base.ServiceUtility Serv = new Base.ServiceUtility();

        string strPortNo = ConfigurationManager.AppSettings["PortNo"].ToString();
        string strMailusername = ConfigurationManager.AppSettings["MailUsername"].ToString();
        string strNetworkusername = ConfigurationManager.AppSettings["NetworkUsername"].ToString();
        string strMailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
        string strHostAddress = ConfigurationManager.AppSettings["HostAddress"].ToString();
        bool blnEnableSSL = ConfigurationManager.AppSettings["EnableSsl"].ToString() == "false" ? false : true;

        string strLogoUrl = ConfigurationManager.AppSettings["LogoUrl"].ToString();
        string strProductType = ConfigurationManager.AppSettings["Producttype"].ToString();

        string strProductName = ConfigurationManager.AppSettings["Appname"].ToString();

        #region static pages
        public ActionResult About()
        {
            try
            {
                #region UsageLog
                string PageName = "About";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
                }
                catch (Exception e) { }
                #endregion
                string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");
                string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
                string strIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
                string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
                if (strAgentId == "" || strUserName == "" || strIPAddress == "" || strSequenceId == "" || strTerminalID == "")
                {
                    //bool msg = LoginController.BSA_AssignSession();
                    return RedirectToAction("SessionExp", "Redirect");
                }
            }
            catch (Exception ex) { }
            return View();
        }
        public ActionResult Contactus_RT()
        {
            try
            {
                #region UsageLog
                string PageName = "Contact Us";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
                }
                catch (Exception e) { }
                #endregion
                string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");
                string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
                string strIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
                string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
                if (strAgentId == "" || strUserName == "" || strIPAddress == "" || strSequenceId == "" || strTerminalID == "")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                    //bool msg = LoginController.BSA_AssignSession();
                }
            }
            catch (Exception ex) { }
            return View();
        }
        public ActionResult Disclaimer()
        {
            try
            {
                #region UsageLog
                string PageName = "Disclaimer";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
                }
                catch (Exception e) { }
                #endregion
                string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");
                string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
                string strIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
                string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
                if (strAgentId == "" || strUserName == "" || strIPAddress == "" || strSequenceId == "" || strTerminalID == "")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                    //bool msg = LoginController.BSA_AssignSession();
                }
            }
            catch (Exception ex) { }
            return View();
        }
        public ActionResult Privacypolicy()
        {
            try
            {
                #region UsageLog
                string PageName = "Privacy Policy";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
                }
                catch (Exception e) { }
                #endregion
                string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");
                string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
                string strIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
                string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
                if (strAgentId == "" || strUserName == "" || strIPAddress == "" || strSequenceId == "" || strTerminalID == "")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                    //bool msg = LoginController.BSA_AssignSession();
                }
            }
            catch (Exception ex) { }
            return View();
        }
        public ActionResult termsandconditions()
        {
            try
            {
                #region UsageLog
                string PageName = "Terms & Conditions";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
                }
                catch (Exception e) { }
                #endregion
                string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");
                string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
                string strIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
                string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
                if (strAgentId == "" || strUserName == "" || strIPAddress == "" || strSequenceId == "" || strTerminalID == "")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                    //bool msg = LoginController.BSA_AssignSession();
                }
            }
            catch (Exception ex) { }
            return View();
        }
        public ActionResult Useragreement()
        {
            try
            {
                #region UsageLog
                string PageName = "User Agreement";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
                }
                catch (Exception e) { }
                #endregion
                string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");
                string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
                string strIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
                string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
                if (strAgentId == "" || strUserName == "" || strIPAddress == "" || strSequenceId == "" || strTerminalID == "")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                    //bool msg = LoginController.BSA_AssignSession();
                }
            }
            catch (Exception ex) { }
            return View();
        }
        public ActionResult productsandserivces()
        {
            try
            {
                #region UsageLog
                string PageName = "Products & Services";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
                }
                catch (Exception e) { }
                #endregion
                string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");
                string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
                string strIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
                string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
                if (strAgentId == "" || strUserName == "" || strIPAddress == "" || strSequenceId == "" || strTerminalID == "")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                    //bool msg = LoginController.BSA_AssignSession();
                }
            }
            catch (Exception ex) { }
            return View();
        }
        #endregion

        #region Registration
        public JsonResult submitRegistration(string strTitle, string strFirstname, string strLastname, string strEmail, string strContact,
            string strPassword, string strDOB, string strFlag)
        {
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strLogData = string.Empty;

            string strLogUsername = string.Empty;
            string strLogAgentID = string.Empty;
            string strLogTerminalID = string.Empty;
            string strLogIPAddress = string.Empty;
            string strLogSequence = string.Empty;
            string strLogTerminalType = string.Empty;
            try
            {
                strFlag = !string.IsNullOrEmpty(strFlag) ? strFlag : "";
                strLogSequence = DateTime.Now.ToString("yyMMdd");
                strLogIPAddress = Base.GetComputer_IP();
                strLogUsername = strEmail;
                strLogTerminalType = "G";
                string strBranchID = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["B2C_BRANCH_ID"]) ? ConfigurationManager.AppSettings["B2C_BRANCH_ID"].ToString() : "";
                string strCurrencyFlag = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["currency"]) ? ConfigurationManager.AppSettings["currency"].ToString() : "";

                #region DataSet for Customer Registration
                DataSet dsRegistration = new DataSet();
                DataTable dtRegistration = new DataTable();
                dtRegistration.Columns.Add("IP");//1
                dtRegistration.Columns.Add("SEQUENCE");//2
                dtRegistration.Columns.Add("USERID");//3
                dtRegistration.Columns.Add("FLAG");//4
                dtRegistration.Columns.Add("BRANCH");//5
                dtRegistration.Columns.Add("AGENCYID");//6
                dtRegistration.Columns.Add("AGENCYNAME");//7
                dtRegistration.Columns.Add("PASSWORD");//8
                dtRegistration.Columns.Add("MOBILE");//9
                dtRegistration.Columns.Add("AGENTTYPE");//10
                dtRegistration.Columns.Add("TITLE");//11
                dtRegistration.Columns.Add("FIRSTNAME");//12
                dtRegistration.Columns.Add("LASTNAME");//13
                dtRegistration.Columns.Add("DOB");//14
                dtRegistration.Columns.Add("COUNTRY");//15
                dtRegistration.Columns.Add("STATE");//16
                dtRegistration.Columns.Add("CITY");//17
                dtRegistration.Columns.Add("ADDRESS1");//18
                dtRegistration.Columns.Add("ADDRESS2");//19
                dtRegistration.Columns.Add("EMAIL");//20
                dtRegistration.Columns.Add("ALTEREMAIL");//21
                dtRegistration.Columns.Add("FAX");//22
                dtRegistration.Columns.Add("TERMINALCOUNT");//23
                dtRegistration.Columns.Add("PHONE");//24
                dtRegistration.Columns.Add("STATUS");//25
                dtRegistration.Columns.Add("RCODE");//26
                dtRegistration.Columns.Add("PAYMENTMODE");//27
                dtRegistration.Columns.Add("Groupname");//28
                dtRegistration.Columns.Add("CLT_GROUP_ID");//29
                dtRegistration.Columns.Add("Newflag");//30
                dtRegistration.Columns.Add("AGENT_CURRENCY");//31
                dtRegistration.Columns.Add("getCBTReportsAccess");//32
                dtRegistration.Columns.Add("CLIENTLOGO");//33
                dtRegistration.Columns.Add("ICUSTID");
                dtRegistration.Columns.Add("IATANO");
                dtRegistration.Columns.Add("JOINDATE");
                dtRegistration.Columns.Add("CURRENTDEPOSIT");
                dtRegistration.Columns.Add("SALESMAN");
                dtRegistration.Columns.Add("SALESMAN1");
                dtRegistration.Columns.Add("SALESMAN2");
                dtRegistration.Columns.Add("LATITUDE");
                dtRegistration.Columns.Add("LONGITUDE");
                dtRegistration.Columns.Add("AG_LOGO");
                dtRegistration.Columns.Add("PGSERVICECHARGE");
                dtRegistration.Columns.Add("JOINTYPE");
                dtRegistration.Columns.Add("SSD_REF_ID");
                dtRegistration.Columns.Add("Balancecheck");
                dtRegistration.Columns.Add("PAN_NO");

                dtRegistration.Rows.Add("", 0, "", "CREATE", strBranchID, "", strEmail, strPassword, strContact, "CS",
                    strTitle, strFirstname, strLastname, strDOB, "", "", "", "", "", strEmail,
                    "", "", "", "", "", "", "P", "", "", "", strCurrencyFlag, "", "", "", "", "", "", "", "", "", "", "", (object)DBNull.Value, "", "", "", "", "");

                dsRegistration.Tables.Add(dtRegistration);
                #endregion

                #region Registration
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                strLogData = "<REGISTRATION>" + dsRegistration.GetXml() + "</REGISTRATION>";
                DatabaseLog.LogData(strLogUsername, "E", "B2CController", "Registration~Req", strLogData, "", "", strLogSequence);

                string[] strResponse = _RaysService.SetCustomer_Con(dsRegistration);//SetCustomer_Con

                strLogData = "<RESPONSE>" + JsonConvert.SerializeObject(strResponse) + "</RESPONSE>";
                DatabaseLog.LogData(strLogUsername, "E", "B2CController", "Registration~Res", strLogData, "", "", strLogSequence);
                #endregion  

                if (strResponse[0].ToString().Contains("CREATED"))
                {
                    StringBuilder sbTicket = new StringBuilder();
                    string strTicket = string.Empty;
                    string RetVal = string.Empty;

                    _MailService.Url = ConfigurationManager.AppSettings["Mailurl"].ToString();

                    sbTicket.Append("<div><br />");
                    sbTicket.Append("<table cellpadding=5 cellspacing=5>");
                    sbTicket.Append("<tr><td style='color:#072562;font-weight: 600;'>Dear " + strFirstname + " " + strLastname + ",</td></tr>");
                    sbTicket.Append("<tr><td style='color:#072562;'>Warm welcome to our family. Your Registration has done Successfully.</td></tr>");
                    sbTicket.Append("<tr><td style='color:#072562;'>Here is your Login Credentials<br/>");
                    sbTicket.Append("Username : " + strEmail + "<br/>");
                    sbTicket.Append("password : " + strPassword + "</td></tr>");
                    sbTicket.Append("<tr><td style='color:#072562;'>You can change the password from My Profile --> Change Password</td></tr>");
                    sbTicket.Append("<tr><td style='color:#072562;'>Thank you for being a part of us. </td></tr>");
                    sbTicket.Append("<tr style='height:50px;'><td></td>");
                    sbTicket.Append("</tr>");
                    sbTicket.Append("<tr><td>Thanks and Regards,</td></tr>");

                    if (strProductType == "ROUNDTRIP")
                    {
                        sbTicket.Append("<tr><td style='padding-top: 0;'>");
                        sbTicket.Append("<p style='font-size:15px;line-height:1.5;margin-top: 0;'>");
                        sbTicket.Append("<a href='https://roundtrip.in' style='text-decoration:none;font-size:20px' target='_blank' data-saferedirecturl='https://www.google.com/url?q=https://roundtrip.in&amp;source=gmail&amp;ust=1606817883903000&amp;usg=AFQjCNHrMYweQ7rl65mpn2J279vPumqeiQ'>");
                        sbTicket.Append("<b><span style='color:#d60015'> Round </span><span style='color:#170079'> trip.in</span></b></a>");
                        sbTicket.Append("<br> 83, Sydenhams Road, Periamet");
                        sbTicket.Append("<br> Chennai – 600003.");
                        sbTicket.Append("</p>");
                        sbTicket.Append("</td></tr>");
                    }
                    else
                    {
                        sbTicket.Append("<tr><td>Support Team</td></tr>");
                    }
                    sbTicket.Append("</table>");
                    sbTicket.Append("</div><br /><br />");
                    strTicket = sbTicket.ToString();

                    strLogData = "<MAIL>" + strTicket + "</MAIL>";

                    DatabaseLog.LogData(strLogUsername, "E", "B2CController", "Registration~Email~Req", strLogData, "", "", strLogSequence);

                    string strSubject = strProductName + "Registraion - " + strFirstname + " " + strLastname;

                    RetVal = _MailService.SendMailSingleTicket("", "", strLogUsername, strLogIPAddress, strLogTerminalType, strLogSequence,
                                strEmail, "", strSubject, strTicket, "", "", strMailusername, strMailPassword, strHostAddress, strPortNo,
                                blnEnableSSL, strMailusername, "");

                    strLogData = "<EVENT><MAIL>" + strEmail + "</MAIL>"
                           + "<username>" + strLogUsername + "</username>"
                           + "<ipAddress>" + strLogIPAddress + "</ipAddress>"
                           + "<sequenceid>" + strLogSequence + "</sequenceid>"
                           + "<MailUsername>" + strMailusername + "</MailUsername>"
                           + "<MailPassword>" + strMailPassword + "</MailPassword>"
                           + "<HostAddress>" + strHostAddress + "</HostAddress>"
                           + "<MailPort>" + strPortNo + "</MailPort>"
                           + "<ssl>" + blnEnableSSL + "</ssl>"
                           + "<MailFrom>" + strMailusername + "</MailFrom>"
                           + "</RESPONSE>" + RetVal + "</RESPONSE>"
                           + "</EVENT>";

                    DatabaseLog.LogData(strLogUsername, "E", "B2CController", "Registration~Email~Res", strLogData, "", "", strLogSequence);

                    strStatus = "01";
                    strMessage = "Registered Successfully.";
                    if (strFlag == "Y")
                    {
                        strStatus = "03";
                        strMessage = strEmail + "|" + strPassword;
                        B2C_Class.GoogleSubmitRegistration _googlesubmitregistration = new B2C_Class.GoogleSubmitRegistration();
                        _googlesubmitregistration.Message = strMessage;
                        _googlesubmitregistration.ResultCode = strStatus;
                        return Json(_googlesubmitregistration);
                    }

                }
                else
                {
                    if (strFlag == "Y")
                    {
                        strStatus = "03";
                        strMessage = strEmail + "|" + strPassword;
                        B2C_Class.GoogleSubmitRegistration _googlesubmitregistration = new B2C_Class.GoogleSubmitRegistration();
                        _googlesubmitregistration.Message = strMessage;
                        _googlesubmitregistration.ResultCode = strStatus;
                        return Json(_googlesubmitregistration);
                    }
                    strStatus = "00";
                    strMessage = strResponse[0].ToString() != "" ? strResponse[0].ToString() : "Failed to Register. Please contact support team (#03)";
                }
            }

            catch (Exception ex)
            {
                strStatus = "00";
                strMessage = "Problem Occured while registration (#05).";
                strLogData = "<ERROR>" + ex.ToString() + "</ERROR>";
                DatabaseLog.LogData(strLogUsername, "E", "B2CController", "Registration~Err", strLogData, "", "", strLogSequence);
            }
            return Json(new { Result = strStatus, Message = strMessage }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Login
        public JsonResult GetLoginDetails_B2C(string strUsername, string strPassword, string strTerminalType)
        {
            string strLogUsername = string.Empty;
            string strLogAgentID = string.Empty;
            string strLogTerminalID = string.Empty;
            string strLogIPAddress = string.Empty;
            string strLogSequence = string.Empty;
            string strLogTerminalType = string.Empty;

            string strResponse = string.Empty;
            string strLogData = string.Empty;
            string strStatus = string.Empty;
            string strMessage = string.Empty;

            DataSet dsLogin = new DataSet();
            string JsonloginRes = string.Empty;
            string strUserFName = string.Empty;
            string strUserLName = string.Empty;
            string strProfilePIC = string.Empty;

            string strQueryString = string.Empty;

            try
            {
                strLogSequence = DateTime.Now.ToString("yyMMdd");
                strLogUsername = strUsername;
                strTerminalType = (!string.IsNullOrEmpty(strTerminalType) ? strTerminalType : "W");
                strLogTerminalType = strTerminalType;
                strLogIPAddress = Base.GetComputer_IP();
                strLogAgentID = ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString().Substring(0, 12);
                strLogTerminalID = ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString();


                #region Login
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                strLogData = "<EVENT><USERNAME>" + strUsername + "</USERNAME><PASSWORD>" + strPassword + "</PASSWORD></EVENT>";
                DatabaseLog.LogData("", "E", "B2CController", "GetLoginDetails_B2C~REQ", strLogData, "", "", "");

                dsLogin = _RaysService.GET_LOGIN_DETAILS("", strUsername, strPassword, "W", "", "", "");

                strLogData = "<EVENT><RESPONSE>" + dsLogin.GetXml() + "</RESPONSE></EVENT>";
                DatabaseLog.LogData("", "E", "B2CController", "GetLoginDetails_B2C~RES", strLogData, "", "", "");
                string strTerminalID = string.Empty, strAgentID = string.Empty;
                if (dsLogin != null && dsLogin.Tables.Count > 0 && dsLogin.Tables[0].Rows.Count > 0)
                {
                    if (dsLogin.Tables[0].Columns.Contains("ERROR"))
                    {
                        strStatus = "00";// dsLogin.Tables[0].Rows[0]["ERROR"].ToString();
                        strResponse = dsLogin.Tables[0].Rows[0]["Result"].ToString();
                    }
                    else
                    {
                        strStatus = "01";
                        strTerminalID = dsLogin.Tables[0].Rows[0]["LGN_TERMINAL_ID"].ToString().Trim().ToUpper();
                        strAgentID = dsLogin.Tables[0].Rows[0]["LGN_AGENT_ID"].ToString().Trim().ToUpper();
                        strResponse = strTerminalID;
                        System.Web.HttpContext.Current.Session.Add("CustomerLogin", "Y");
                        bool msg = BSA_AssignSession(strTerminalID, strUsername, strPassword, dsLogin, strTerminalType);
                        strProfilePIC = ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png";
                        strProfilePIC = Base.ChangeToLanIP(strProfilePIC);


                        try
                        {
                            if (ConfigurationManager.AppSettings["DashboardProfile"] != null && ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("PDF/B2CUserLogo"))
                            {
                                var filePath = Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("~/PDF/B2CUserLogo/"), "*.*")
                                                 .Where(v => Path.GetFileName(v) == strAgentID + Path.GetExtension(Path.GetFileName(v))).ToList();
                                if (filePath.Count == 0)
                                {
                                    strProfilePIC = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                                }
                                else
                                {
                                    strProfilePIC = ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png";
                                }
                            }
                            else
                            {
                                string strProfilePath = ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("|") ? (ConfigurationManager.AppSettings["DashboardProfile"].ToString().Split('|')[1] + strAgentID + ".png") : (ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png");

                                bool CheckURl = Base.CheckUrlStatus(strProfilePath);

                                if (CheckURl == true)
                                {
                                    strProfilePIC = ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("|") ? ConfigurationManager.AppSettings["DashboardProfile"].ToString().Split('|')[0] + strAgentID + ".png" : ConfigurationManager.AppSettings["DashboardProfile"].ToString()+ strAgentID + ".png";
                                }
                                else
                                {
                                    strProfilePIC = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            strProfilePIC = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                        }

                        strUserFName = dsLogin.Tables[0].Rows[0]["LGN_AGENT_FIRSTNAME"].ToString().Trim();
                        strUserLName = dsLogin.Tables[0].Rows[0]["LGN_AGENT_LASTNAME"].ToString().Trim();
                        string strMobileNO = dsLogin.Tables[0].Rows[0]["LGN_MOBILE_NO"].ToString().Trim().ToUpper();
                        System.Web.HttpContext.Current.Session.Add("USERPROFILEPIC", strProfilePIC);
                        string agenttype = dsLogin.Tables[0].Columns.Contains("AGENT_TYPE") ? dsLogin.Tables[0].Rows[0]["AGENT_TYPE"].ToString() : dsLogin.Tables[0].Rows[0]["AGN_TYPE"].ToString();
                        string branchid = dsLogin.Tables[0].Columns.Contains("BCH_BRANCH_ID") ? dsLogin.Tables[0].Rows[0]["BCH_BRANCH_ID"].ToString() : dsLogin.Tables[0].Rows[0]["AGN_BRANCHID"].ToString();
                        string privilage = dsLogin.Tables[0].Rows[0]["LGN_TERMINAL_PREVILAGE"].ToString();
                        System.Web.HttpContext.Current.Session.Add("USERFNAME", strUserFName);
                        System.Web.HttpContext.Current.Session.Add("USERLNAME", strUserLName);
                        System.Web.HttpContext.Current.Session.Add("USERMOBILENO", strMobileNO);
                        System.Web.HttpContext.Current.Session.Add("USERNAME", strUsername);
                        System.Web.HttpContext.Current.Session.Add("USERTITLE", dsLogin.Tables[0].Rows[0]["LGN_AGENT_TITLE"].ToString().Trim());
                        System.Web.HttpContext.Current.Session.Add("USERADDRESS", dsLogin.Tables[0].Rows[0]["LGN_ADDRESS_1"].ToString().Trim());
                        System.Web.HttpContext.Current.Session.Add("USERCITY", dsLogin.Tables[0].Rows[0]["LGN_CITY_ID"].ToString().Trim());
                        System.Web.HttpContext.Current.Session.Add("USERCOUNTRY", dsLogin.Tables[0].Rows[0]["LGN_COUNTRY_ID"].ToString().Trim());
                        System.Web.HttpContext.Current.Session.Add("USERPASSPORTNO", dsLogin.Tables[0].Rows[0]["PASSPORT_NO"].ToString().Trim());
                        System.Web.HttpContext.Current.Session.Add("USEROB", dsLogin.Tables[0].Rows[0]["DOB"].ToString().Trim());
                        System.Web.HttpContext.Current.Session.Add("USERPASSPORT_EXP", dsLogin.Tables[0].Rows[0]["LGN_PASSPORT_EXPIRY_DATE"].ToString().Trim());
                        System.Web.HttpContext.Current.Session.Add("USERPASS_ISSU_COUNTRY", dsLogin.Tables[0].Rows[0]["LGN_ISSUED_COUNTRY"].ToString().Trim());
                        System.Web.HttpContext.Current.Session.Add("USERPINCODE", dsLogin.Tables[0].Rows[0]["LGN_PINCODE"].ToString().Trim());
                        strLogSequence = dsLogin.Tables[0].Columns.Contains("SEQUENCE_ID") ? dsLogin.Tables[0].Rows[0]["SEQUENCE_ID"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                        #region Querystring Formation
                        strQueryString = "AGENTID=" + strAgentID + "&TERMINALID=" + strTerminalID + "&USERNAME=" + strUsername + "&PWD=" + strPassword +
                            "&AGENYTYPE=" + agenttype + "&SEQUENCEID=" + strLogSequence + "&BRANCHID=" + branchid + "&TERMINALPREVILAGE=" + privilage +
                            "&TERMINALTYPE=" + strTerminalType + "&IPADDRESS=" + strLogIPAddress;
                        string strKey = ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString();
                        strQueryString = "SECKEY=" + Base.EncryptKEy(strQueryString, strKey);
                        #endregion
                        strResponse = "Login Successfull";
                    }
                }
                else
                {
                    strStatus = "00";
                    strResponse = "unable to login. please try again later";
                }
                if (strTerminalType.ToUpper().Trim() == "M")
                {
                    string strBannerURL = ConfigurationManager.AppSettings["MOBILEADVERTISEMENT"].ToString();
                    MobileLoginRQRS _MobileLoginRQRS = new MobileLoginRQRS();
                    _MobileLoginRQRS.URLQuerystring = strQueryString;
                    _MobileLoginRQRS.BannerURL = strBannerURL;
                    _MobileLoginRQRS.FlightURL = ConfigurationManager.AppSettings["AirlineURL"];
                    _MobileLoginRQRS.HotelURL = "";
                    _MobileLoginRQRS.HolidaysURL = "";
                    _MobileLoginRQRS.VisaURL = "";
                    _MobileLoginRQRS.Username = strUserFName + " " + strUserLName;
                    _MobileLoginRQRS.MyBookingsURL = ConfigurationManager.AppSettings["AirlineURL"] + "/B2C/DashBoard?FLAG=B&";
                    _MobileLoginRQRS.MyCancellations = ConfigurationManager.AppSettings["AirlineURL"] + "/B2C/DashBoard?FLAG=C&";
                    _MobileLoginRQRS.MyUpcomingTrips = ConfigurationManager.AppSettings["AirlineURL"] + "/B2C/DashBoard?FLAG=U&";
                    _MobileLoginRQRS.MySupport = ConfigurationManager.AppSettings["AirlineURL"] + "/B2C/DashBoard?FLAG=S&";
                    _MobileLoginRQRS.ProfileImage = strProfilePIC;
                    strLogData = "<QUERY_STRING>" + JsonConvert.SerializeObject(_MobileLoginRQRS) + "</QUERY_STRING>";
                    DatabaseLog.LogData(strLogUsername, "E", "B2CController", "Mobile Login~QueryString", strLogData, "", "", strLogSequence);
                    JsonloginRes = JsonConvert.SerializeObject(_MobileLoginRQRS);
                }

                #endregion
            }
            catch (Exception ex)
            {
                strStatus = "00";
                strMessage = "Problem Occured while login (#05).";
                strLogData = "<ERROR>" + ex.ToString() + "</ERROR>";
                DatabaseLog.LogData(strLogUsername, "E", "B2CController", "GetLoginDetails_B2C~Err", strLogData, "", "", strLogSequence);
            }
            return Json(new { USERNAME = strUsername, Status = strStatus, Result = strResponse, MobileLogin = JsonloginRes }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Google Registration or Signin
        public ActionResult GoogleIntegration()
        {
            string strToken = string.Empty;
            strToken = "01";
            if (Request.QueryString["code"] != null && Request.QueryString["code"] != "" && (Session["AgainRedirectGoogleINtflag"] == null || Session["AgainRedirectGoogleINtflag"].ToString() == ""))
            {
                strToken = GetToken(Request.QueryString["code"].ToString());
            }
            else
            {
                string clientid = System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleIntClientid"];
                string clientsecret = System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleClientSecret"];
                string redirection_url = System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleReturnUrl"];
                string url = System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleRedirectUrl"] + "?scope=" + System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleScope"] + "&include_granted_scopes=true&redirect_uri=" + redirection_url + "&response_type=code&client_id=" + clientid + "";
                Response.Redirect(url);
                Session["AgainRedirectGoogleINtflag"] = "";
            }
            if (strToken != "01")
            {
                Session.Add("GoogleLoginFail", "TRUE");
            }
            return View();
        }

        public string GetToken(string code)
        {
            string clientid = System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleIntClientid"];
            string clientsecret = System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleClientSecret"];
            //your redirection url  
            string redirection_url = System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleReturnUrl"];
            string url = System.Web.Configuration.WebConfigurationManager.AppSettings["Googleaccurl"];

            string poststring = "grant_type=authorization_code&code=" + code + "&client_id=" + clientid + "&client_secret=" + clientsecret + "&scope=https://www.googleapis.com/auth/userinfo.email&redirect_uri=" + redirection_url + "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            UTF8Encoding utfenc = new UTF8Encoding();
            byte[] bytes = utfenc.GetBytes(poststring);
            Stream outputstream = null;
            try
            {
                request.ContentLength = bytes.Length;
                outputstream = request.GetRequestStream();
                outputstream.Write(bytes, 0, bytes.Length);
            }
            catch { }
            var response = (HttpWebResponse)request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            string responseFromServer = streamReader.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            B2C_Class.Tokenclass obj = js.Deserialize<B2C_Class.Tokenclass>(responseFromServer);
            // hdnvalue.Value = obj.id_token;
            string returnstr = GetuserProfile(obj.access_token, obj.id_token);
            return returnstr;
        }

        public string GetuserProfile(string accesstoken, string idtoken)
        {

            string url = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + accesstoken + "";
            //string url = "https://www.googleapis.com/plus/v1/people/userId=" + idtoken;
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            B2C_Class.Userclass userinfo = js.Deserialize<B2C_Class.Userclass>(responseFromServer);
            B2C_Class.Registration reg = new B2C_Class.Registration();
            //ViewBag.GoogelIneresult = userinfo.email + "|" + userinfo.name;
            reg.txt_EmailID = userinfo.email;
            reg.Flagcheck = "Y";
            reg.txtfname = "";
            reg.slct_title = userinfo.gender != null && userinfo.gender == "male" ? "Mr." : userinfo.gender != null ? "Mrs." : "Mr";
            reg.txtlname = userinfo.name != null && userinfo.name.Split(' ').Length > 1 ? userinfo.name.Split(' ')[1] : " ";
            reg.Password = "Password@123";
            reg.slct_Countrycode = "91";
            reg.txtmobileNo = "9876543210";
            reg.Address = "Chennai";
            reg.City = "Chennai";

            string returnstr = SocialNetworkLogin(reg, "Gmail");
            return returnstr;
        }

        public string SocialNetworkLogin(B2C_Class.Registration reg, string Page)
        {
            var Registrationsubmit = DependencyResolver.Current.GetService<B2CController>();

            JsonResult gooobjRegistration = Registrationsubmit.submitRegistration(reg.slct_title, reg.txtfname, reg.txtlname, reg.txt_EmailID, reg.txtmobileNo, reg.Password, reg.dobirth, reg.Flagcheck);
            B2C_Class.GoogleSubmitRegistration _objRegResponce = (B2C_Class.GoogleSubmitRegistration)gooobjRegistration.Data;

            var Checkloginsubmit = DependencyResolver.Current.GetService<B2CController>();
            JsonResult _loginsubmit = Checkloginsubmit.GetLoginDetails_B2C(_objRegResponce.Message.Split('|')[0], _objRegResponce.Message.Split('|')[1], ConfigurationManager.AppSettings["TerminalType"].ToString());

            string result = JsonConvert.SerializeObject(_loginsubmit.Data);//(((JsonResult)(_loginsubmit)).Data).ToString();

            B2C_Class.GoogleLogincheck _objLoginResponce = new B2C_Class.GoogleLogincheck();
            _objLoginResponce = JsonConvert.DeserializeObject<B2C_Class.GoogleLogincheck>(result.ToString());

            if (_objLoginResponce.Status.ToString() == "01")
            {
                Session["AgainRedirectGoogleINtflag"] = "N";
            }
            return _objLoginResponce.Status.ToString();
        }
        #endregion

        #region E-Ticket Print
        public ActionResult PrintETicket()
        {
            if (ConfigurationManager.AppSettings["APP_HOSTING"].ToString() != "BSA")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            return View();
        }

        public ActionResult GetETicket(string strPNR, string strContactNo, string strMailID, string strLastName)
        {
            string strStatus = string.Empty;
            string strResult = string.Empty;
            string strMessage = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalID = string.Empty;
            string strUsername = string.Empty;
            string strTerminalType = string.Empty;
            string strIPAddress = string.Empty;
            string strSequenceID = string.Empty;
            string strRefErrorMsg = string.Empty;
            string strAgentType = string.Empty;
            string strGetTimeZone = string.Empty;
            string strPrevilage = string.Empty;

            bool strResponse = false;

            string strTicketCopy = string.Empty;
            DataSet dsETicket = new DataSet();
            DataSet dspdf = new DataSet();

            string RequestedmailID = string.Empty;
            string BookingCompanyID = string.Empty;
            string BookingCompanyTicket = string.Empty;
            string strrefCompanyName = string.Empty;
            string strErrorMsg = string.Empty;

            try
            {

                strAgentID = ((Session["POS_ID"] != null && Session["POS_ID"].ToString() != "") ? Session["POS_ID"].ToString() : "");
                strUsername = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
                strSequenceID = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
                strAgentID = (Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() == "Y") ? strAgentID : ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString().Substring(0, 12);
                strTerminalID = (Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() == "Y") ? strUsername : ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString();
                strTerminalType = ConfigurationManager.AppSettings["TerminalType"].ToString();
                strUsername = (Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() == "Y") ? strTerminalID : ConfigurationManager.AppSettings["BSA_USERNAME"].ToString();
                strSequenceID = (Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() == "Y") ? strSequenceID : DateTime.Now.ToString("yyyyMMdd");
                strIPAddress = Base.GetComputer_IP();
                strAgentType = Session["agenttype"] != null && Session["agenttype"].ToString() != "" ? Session["agenttype"].ToString() : "";
                strGetTimeZone = ConfigurationManager.AppSettings["Servertimezone"].ToString();
                strPrevilage = (Session["privilage"] != null && Session["privilage"].ToString() != "") ? Session["privilage"].ToString() : "";
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                int Ticketcount = 0;

                if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "RIYA")
                {
                    strResponse = _InplantService.PrintTicket_Riya(strAgentID, strPNR, "", "", false, true, true, true, true, false, strTerminalID, strUsername, strIPAddress, strTerminalType, Convert.ToDecimal(strSequenceID), ref Ticketcount, ref strResult, ref strTicketCopy, new string[] { });
                }
                else if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "TRAVRAYS")
                {
                    strResponse = _RaysService.PrintTicket_Travrays(strAgentID, strPNR, "", "", strGetTimeZone, false, true, true, true, strTerminalID, strUsername, strIPAddress, strTerminalType, Convert.ToDecimal(strSequenceID), strPrevilage, strAgentType, "MT", ref strTicketCopy, ref strErrorMsg, new string[] { }, ref dspdf);
                }
                else if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "ROUNDTRIP")
                {
                    strResponse = _RaysService.PrintTicket_RoundTrip(strAgentID, strPNR, "", "", false, true, true, true, true, false, strTerminalID,
                            strUsername, strIPAddress, strTerminalType, Convert.ToDecimal(strSequenceID), ref Ticketcount, ref strTicketCopy,
                            ref strRefErrorMsg, new string[] { }, "B2C", strMailID, strContactNo, strLastName);
                }
                else
                {
                    strMessage = "Unable to Fetch E-Ticket.Please try again later!.";
                }

                if (strResponse == false)
                {
                    strStatus = "00";
                    strResult = strRefErrorMsg;
                    Session.Add("pdfsession" + strPNR, "");
                    strMessage = string.IsNullOrEmpty(strMessage) ? "No Records found" : strMessage;
                }
                else
                {
                    strStatus = "01";
                    strResult = strTicketCopy;
                    Session.Add("pdfsession" + strPNR, strTicketCopy);
                    strMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                strStatus = "05";
                strResult = ex.ToString();
                Session.Add("pdfsession" + strPNR, "");
                strMessage = "Unable to retrieve PNR details. Try again later.";
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage });
        }
        #endregion

        #region DASHBOARD
        //[CustomFilter]
        public ActionResult DashBoard()
        {
            if (Request["FLAG"] != null && Request["FLAG"].ToString() != "" && Request["SECKEY"] != null && Request["SECKEY"].ToString() != "")
            {
                ViewBag.Flag = Request.QueryString["FLAG"].ToString();
                string strB2CUserName = string.Empty, strB2CPassword = string.Empty, strB2CTerminalType = string.Empty;
                string Encquery = Request.QueryString["SECKEY"];
                string strKey = ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString();
                string strQueryString = Base.DecryptKEY(Encquery, strKey);
                string[] QueryString = strQueryString.ToString().Split('&');
                for (int i = 0; i < QueryString.Length; i++)
                {
                    string[] strData = QueryString[i].Split('=');
                    if (strData[0] == "USERNAME")
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
                var Login = GetLoginDetails_B2C(strB2CUserName, strB2CPassword, strB2CTerminalType);
                if (Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() != "" && Session["CustomerLogin"].ToString() == "N")
                {
                    DatabaseLog.LogData("", "E", "B2CController", "Mobile Dashboard", "", "", "", "");
                    return RedirectToAction("SessionExp", "Redirect");
                }
            }
            string strCustomerLogin = Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() != "" ? Session["CustomerLogin"].ToString() : "N";
            if (strCustomerLogin == "N")
            {
                return RedirectToAction("Flights", "Flights");
            }
            else
            {
                ViewBag.ServerDateTime = Base.LoadServerdatetime();
                return View();
            }
        }
        #endregion

        #region MyBookings B2C
        [CustomFilter]
        public ActionResult FetchMyBooking(string strType, string strFromdate, string strToDate, string strSPNR)
        {
            string strStatus = string.Empty;
            string strResult = string.Empty;
            string strMessage = string.Empty;
            string strLogData = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalID = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string strSequenceid = string.Empty;
            string strUserEmailID = string.Empty;
            string strUserMobileNo = string.Empty;
            string strUrl = string.Empty;
            string strTravelFromDate = string.Empty;
            string strTravelToDate = string.Empty;
            string strTerminalType = string.Empty;
            Hashtable hsParam = new Hashtable();
            try
            {
                strAgentID = (Session["POS_ID"] != null && Session["POS_ID"].ToString() != "") ? Session["POS_ID"].ToString() : "";
                strTerminalID = (Session["POS_TID"] != null && Session["POS_TID"].ToString() != "") ? Session["POS_TID"].ToString() : "";
                strUserName = (Session["username"] != null && Session["username"].ToString() != "") ? Session["username"].ToString() : "";
                Ipaddress = (Session["ipAddress"] != null && Session["ipAddress"].ToString() != "") ? Session["ipAddress"].ToString() : "";
                strSequenceid = (Session["sequenceid"] != null && Session["sequenceid"].ToString() != "") ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                strUserMobileNo = Session["USERMOBILENO"] != null && Session["USERMOBILENO"].ToString() != "" ? Session["USERMOBILENO"].ToString() : "";
                strTerminalType = Session["TerminalType"] != null && Session["TerminalType"].ToString() != "" ? Session["TerminalType"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();

                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<REQUEST><AGENTID>" + strAgentID + "</AGENTID>"
                    + "<TERMINALID>" + strTerminalID + "</TERMINALID>" + "<USERNAME>" + strUserName + "</USERNAME>"
                    + "<IPADDRESS>" + Ipaddress + "</IPADDRESS>" + "<SEQUENCEID>" + strSequenceid + "</SEQUENCEID>"
                    + "<USERMOBILE>" + strUserMobileNo + "</USERMOBILE>" + "<USEREMAIL>" + strUserName + "</USEREMAIL>"
                    + "<TYPE>" + strType + "</TYPE>" + "<REQUEST>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "FetchMyBooking~REQ", strLogData, strAgentID, strTerminalID, strSequenceid);

                if (strType.Contains("U"))
                {
                    strTravelFromDate = DateTime.Now.ToString("yyyyMMdd");
                    strTravelToDate = DateTime.Now.AddMonths(1).ToString("yyyyMMdd");
                    strFromdate = "";
                    strToDate = "";
                }
                else if ((string.IsNullOrEmpty(strFromdate) || string.IsNullOrEmpty(strToDate)) && !strType.Contains("U"))
                {
                    strTravelFromDate = "";
                    strTravelToDate = "";
                    strFromdate = DateTime.Now.AddMonths(-1).ToString("yyyyMMdd");
                    strToDate = DateTime.Now.ToString("yyyyMMdd");
                }
                else
                {
                    strFromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    strToDate = DateTime.ParseExact(strToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                }

                hsParam.Add("strSPNR", !string.IsNullOrEmpty(strSPNR) ? strSPNR : "");
                hsParam.Add("strToDate", strToDate);
                hsParam.Add("strFromdate", strFromdate);
                hsParam.Add("strAgentID", strAgentID);
                hsParam.Add("strTerminalID", strTerminalID);
                hsParam.Add("strUserName", strUserName);
                hsParam.Add("strSequenceID", strSequenceid);
                hsParam.Add("strIPAddress", Ipaddress);
                hsParam.Add("strTerminalType", strTerminalType);
                hsParam.Add("strUserEmailID", strUserName);
                hsParam.Add("strUserMobileNo", strUserMobileNo);
                hsParam.Add("strResult", "");
                hsParam.Add("strErrorMessage", "");
                hsParam.Add("strTravelFromdate", strTravelFromDate);
                hsParam.Add("strTravelToDate", strTravelToDate);
                hsParam.Add("strCRSPNR", "");
                hsParam.Add("strAirlinePNR", "");
                hsParam.Add("strStatus", "");
                hsParam.Add("strPaymentMode", "");
                hsParam.Add("strAirline", "");
                hsParam.Add("strFirstName", "");
                hsParam.Add("strLastName", "");
                hsParam.Add("strAirport", "");

                JObject objJsonSeq = Base.callWebMethod("FetchMyBookings", hsParam, ref strMessage);

                if (objJsonSeq != null)
                {
                    bool JsonBook = (bool)objJsonSeq["FetchMyBookingsResult"];
                    if (JsonBook == true)
                    {
                        strResult = (string)objJsonSeq["strResult"];
                        strMessage = (string)objJsonSeq["strErrorMessage"];

                        DataSet dsResult = JsonConvert.DeserializeObject<DataSet>(strResult);
                        if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                        {
                            strStatus = "01";
                            string DepatureDate = Convert.ToDateTime(dsResult.Tables[0].Rows[0]["DEPATURE_DATE"].ToString()).ToString("dddd dd/MM/yyyy HH:mm tt");
                            var qryMyBookings = (from _DtMyBookings in dsResult.Tables[0].AsEnumerable()
                                                 orderby _DtMyBookings["BOOKED_DATE"] descending
                                                 where strType == "U" ? _DtMyBookings["TICKET_STATUS"].ToString().ToUpper() == "LIVE" :
                                                 strType == "C" ? _DtMyBookings["TICKET_STATUS"].ToString().ToUpper().Contains("CANCELLED") :
                                                 _DtMyBookings["TICKET_STATUS"].ToString() != ""
                                                 select new
                                                 {
                                                     S_PNR = _DtMyBookings["S_PNR"],
                                                     Airline_PNR = _DtMyBookings["AIRLINE_PNR"],
                                                     CRS_PNR = _DtMyBookings["CRS_PNR"],
                                                     Status = _DtMyBookings["TICKET_STATUS"],
                                                     TripType = _DtMyBookings["TRIP_TYPE"].ToString().ToUpper(),
                                                     TotalPassenger = _DtMyBookings["TOTAL_PASSENGERS"],
                                                     Depature = Convert.ToDateTime(_DtMyBookings["DEPATURE_DATE"].ToString()).ToString("dddd dd/MM/yyyy HH:mm tt"),
                                                     DepatureDay = Convert.ToDateTime(_DtMyBookings["DEPATURE_DATE"].ToString()).ToString("dddd"),
                                                     DepatureDate = Convert.ToDateTime(_DtMyBookings["DEPATURE_DATE"].ToString()).ToString("dd/MM/yyyy"),
                                                     DepatureTime = Convert.ToDateTime(_DtMyBookings["DEPATURE_DATE"].ToString()).ToString("HH:mm tt"),
                                                     Arrival = Convert.ToDateTime(_DtMyBookings["ARRIVAL_DATE"].ToString()).ToString("dddd dd/MM/yyyy HH:mm tt"),
                                                     ArrivalDay = Convert.ToDateTime(_DtMyBookings["ARRIVAL_DATE"].ToString()).ToString("dddd"),
                                                     ArrivalDate = Convert.ToDateTime(_DtMyBookings["ARRIVAL_DATE"].ToString()).ToString("dd/MM/yyyy"),
                                                     ArrivalTime = Convert.ToDateTime(_DtMyBookings["ARRIVAL_DATE"].ToString()).ToString("HH:mm tt"),
                                                     FlightNo = _DtMyBookings["FLIGHT_NO"],
                                                     OriginCode = _DtMyBookings["ORIGIN_CITY"].ToString().Trim(),
                                                     Origin = Base.Utilities.AirportcityName(_DtMyBookings["ORIGIN_CITY"].ToString().Trim()),
                                                     DestinationCode = _DtMyBookings["DESINATION_CITY"].ToString().Trim(),
                                                     Destination = Base.Utilities.AirportcityName(_DtMyBookings["DESINATION_CITY"].ToString().Trim()),
                                                     AirlineCode = _DtMyBookings["AIRLINE_CODE"],
                                                     AirlineName = Base.Utilities.AirlineName(_DtMyBookings["AIRLINE_CODE"].ToString().Trim()),
                                                     Airport_Type = _DtMyBookings["AIRPORT_ID"],
                                                     Payment_Mode = _DtMyBookings["PAYMENT_MODE"],
                                                     Cabin = (_DtMyBookings["CABIN"].ToString() == "E" ? "Economy" : (_DtMyBookings["CABIN"].ToString() == "F" ? "First Class" : (_DtMyBookings["CABIN"].ToString() == "B" ? "Business" : (_DtMyBookings["CABIN"].ToString() == "P" ? "Premium Economy" : _DtMyBookings["CABIN"].ToString())))),
                                                     JourneyTime = _DtMyBookings["JOURNEY_TIME"].ToString(),
                                                     GrossFare = _DtMyBookings["TOTAL_FARE"],
                                                     Segment_Ref = _DtMyBookings["SEGMENT_REF"],
                                                     Trip_No = _DtMyBookings["TRIP_NO"],
                                                     Dep_sort = Convert.ToDateTime(_DtMyBookings["DEPATURE_DATE"].ToString()),
                                                 }).ToList();
                            if (strType == "UD")
                            {
                                qryMyBookings = (from _qryMyBooking in qryMyBookings
                                                 orderby _qryMyBooking.Dep_sort ascending
                                                 select _qryMyBooking).ToList();
                                var qryMyBooking = qryMyBookings.Take(5).ToList();
                                if (qryMyBooking.Count > 0)
                                {
                                    strResult = JsonConvert.SerializeObject(qryMyBooking);
                                }
                                else
                                {
                                    strStatus = "00";
                                    strMessage = "No Records found.";
                                }
                            }
                            else
                            {
                                if (strType == "U")
                                {
                                    qryMyBookings = (from _qryMyBooking in qryMyBookings
                                                     orderby _qryMyBooking.Dep_sort ascending
                                                     select _qryMyBooking).ToList();
                                }
                                var qryMyBooking = qryMyBookings.GroupBy(grp => grp.S_PNR).Select(grp => grp.GroupBy(trp => trp.Trip_No).Select(trp => trp.ToList()).ToList()).ToList();
                                if (qryMyBooking.Count > 0)
                                {
                                    strResult = JsonConvert.SerializeObject(qryMyBooking);
                                }
                                else
                                {
                                    strStatus = "00";
                                    strMessage = "No Records found.";
                                }
                            }
                        }
                        else
                        {
                            strStatus = "00";
                            strMessage = "No Records found.";
                        }
                    }
                    else
                    {
                        strMessage = (string)objJsonSeq["strErrorMessage"];
                        strStatus = "00";
                        strMessage = !string.IsNullOrEmpty(strMessage) ? strMessage : "Unable to Request Details. Please contact support team (#01).";
                    }
                }
                else
                {
                    strStatus = "00";
                    strResult = strMessage;
                }

                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<RESPONSE>" + strResult + "<RESPONSE>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "FetchMyBooking~RES", strLogData, strAgentID, strTerminalID, strSequenceid);
            }
            catch (Exception ex)
            {
                strStatus = "05";
                strResult = ex.ToString();
                strMessage = "Unable to Fetch Request details. please contact customer care.";
                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<ERROR>" + strResult + "<ERROR>";
                DatabaseLog.LogData(strUserName, "X", "B2CController", "FetchMyBooking~Err", strLogData, strAgentID, strTerminalID, strSequenceid);
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage });
        }

        [CustomFilter]
        public ActionResult FetchMyViewPNR(string strSPNR)
        {
            string strStatus = string.Empty;
            string strResult = string.Empty;
            string strMessage = string.Empty;
            string strLogData = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalID = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string strSequenceid = string.Empty;
            string strTerminalType = string.Empty;
            string strAgentType = string.Empty;
            string strRefError = string.Empty;
            string strRefErrorMsg = string.Empty;
            byte[] byt_ViewPNR_Details = new byte[] { };
            byte[] byt_ViewPNR_DisplayDetails = new byte[] { };
            DataSet dsViewPNR_Details = new DataSet();
            DataSet dsViewPNR_DispDetails = new DataSet();
            bool MyResult = false;
            string strPaxDetails = string.Empty;
            string strPaymentDetails = string.Empty;
            string strTicketDetails = string.Empty;
            try
            {
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                strAgentID = (Session["POS_ID"] != null && Session["POS_ID"].ToString() != "") ? Session["POS_ID"].ToString() : "";
                strTerminalID = (Session["POS_TID"] != null && Session["POS_TID"].ToString() != "") ? Session["POS_TID"].ToString() : "";
                strUserName = (Session["username"] != null && Session["username"].ToString() != "") ? Session["username"].ToString() : "";
                Ipaddress = (Session["ipAddress"] != null && Session["ipAddress"].ToString() != "") ? Session["ipAddress"].ToString() : "";
                strSequenceid = (Session["sequenceid"] != null && Session["sequenceid"].ToString() != "") ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                strTerminalType = Session["TerminalType"] != null && Session["TerminalType"].ToString() != "" ? Session["TerminalType"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                strAgentType = Session["agenttype"] != null && Session["agenttype"].ToString() != "" ? Session["agenttype"].ToString() : "";

                strLogData = "<URL>[< ![CDATA[" + _RaysService.Url + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                   + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<REQUEST><AGENTID>" + strAgentID + "</AGENTID>"
                   + "<TERMINALID>" + strTerminalID + "</TERMINALID>" + "<USERNAME>" + strUserName + "</USERNAME>"
                   + "<IPADDRESS>" + Ipaddress + "</IPADDRESS>" + "<SEQUENCEID>" + strSequenceid + "</SEQUENCEID>"
                   + "<strSPNR>" + strSPNR + "</strSPNR>" + "<strAgentType>" + strAgentType + "</strAgentType>" + "<REQUEST>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "FetchMyViewPNR~REQ", strLogData, strAgentID, strTerminalID, strSequenceid);

                MyResult = _RaysService.Fetch_PNR_Verification_Details_NewByte(strAgentID, strSPNR, "", "", "", strTerminalID, strUserName, Ipaddress, strTerminalType,
                    Convert.ToDecimal(strSequenceid), ref byt_ViewPNR_Details, ref byt_ViewPNR_DisplayDetails, ref strRefError, ref strRefErrorMsg, "FetchMyViewPNR",
                    "FetchMyViewPNR", strAgentType, strTerminalID);

                if (MyResult == true)
                {
                    dsViewPNR_Details = Base.Decompress(byt_ViewPNR_Details);
                    dsViewPNR_DispDetails = Base.Decompress(byt_ViewPNR_DisplayDetails);
                    JsonConvert.SerializeObject(dsViewPNR_Details);
                    if (dsViewPNR_Details != null && dsViewPNR_Details.Tables.Count > 0 && dsViewPNR_Details.Tables[0].Rows.Count > 0
                        && dsViewPNR_DispDetails != null && dsViewPNR_DispDetails.Tables.Count > 0 && dsViewPNR_DispDetails.Tables[0].Rows.Count > 0)
                    {
                        #region Fare Details
                        var qryPaymentDetails = (from _dtPayment in dsViewPNR_DispDetails.Tables[0].AsEnumerable()
                                                 select new
                                                 {
                                                     BASIC_FARE = Convert.ToDecimal(_dtPayment["BASICFAREAMOUNT"]),
                                                     TAX_FARE = Convert.ToDecimal(_dtPayment["TAXAMOUNTCLT"]),
                                                     BAGGAGE_FARE = Convert.ToDecimal(_dtPayment["Baggage_Amt"]),
                                                     MEALS_FARE = Convert.ToDecimal(_dtPayment["Meals_Amt"]),
                                                     SEAT_FARE = Convert.ToDecimal(_dtPayment["Seat_Amt"]),
                                                     GROSS_FARE = Convert.ToDecimal(_dtPayment["GROSSAMOUNTCLT"]),
                                                 }).ToList();
                        strPaymentDetails = JsonConvert.SerializeObject(qryPaymentDetails);
                        #endregion

                        #region TIcketDetails
                        var qryTicketDetails = (from _dtTicket in dsViewPNR_Details.Tables[0].AsEnumerable()
                                                where _dtTicket["PAX_REF_NO"].ToString() == "1"
                                                select new
                                                {
                                                    S_PNR = _dtTicket["S_PNR"].ToString().Trim(),
                                                    Airline_PNR = _dtTicket["AIRLINE_PNR"].ToString().Trim(),
                                                    Status = _dtTicket["TICKET_STATUS"].ToString().ToUpper().Trim(),
                                                    AirlineName = _dtTicket["PLATING_CARRIER_NAME"].ToString().Trim(),
                                                    AirlineCode = _dtTicket["PLATING_CARRIER"].ToString().Trim(),
                                                    FlightNO = _dtTicket["FLIGHT_NO"].ToString().Trim(),
                                                    Origin = Base.Utilities.AirportcityName(_dtTicket["ORIGIN"].ToString().Trim()),
                                                    OriginCode = _dtTicket["ORIGIN"].ToString().Trim(),
                                                    Destination = Base.Utilities.AirportcityName(_dtTicket["DESTINATION"].ToString().Trim()),
                                                    DestinationCode = _dtTicket["DESTINATION"].ToString().Trim(),
                                                    TotalPassenger = Convert.ToInt32(_dtTicket["AdultCount"].ToString().Trim())
                                                                        + Convert.ToInt32(_dtTicket["ChildCount"].ToString().Trim())
                                                                        + Convert.ToInt32(_dtTicket["InfantCount"].ToString().Trim()),
                                                    DepatureDay = Convert.ToDateTime(DateTime.ParseExact(_dtTicket["DEPTDT"].ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)).ToString("dddd"),
                                                    DepatureDate = Convert.ToDateTime(DateTime.ParseExact(_dtTicket["DEPTDT"].ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)).ToString("dd/MM/yyyy"),
                                                    DepatureTime = Convert.ToDateTime(DateTime.ParseExact(_dtTicket["DEPTDT"].ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)).ToString("HH:mm tt"),
                                                    ArrivalDay = Convert.ToDateTime(DateTime.ParseExact(_dtTicket["ARRIVALDT"].ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)).ToString("dddd"),
                                                    ArrivalDate = Convert.ToDateTime(DateTime.ParseExact(_dtTicket["ARRIVALDT"].ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)).ToString("dd/MM/yyyy"),
                                                    ArrivalTime = Convert.ToDateTime(DateTime.ParseExact(_dtTicket["ARRIVALDT"].ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)).ToString("HH:mm tt"),
                                                    Class = _dtTicket["CLASS_NAME"].ToString().Trim(),
                                                    JourneyTime = Convert.ToInt32(_dtTicket["TCK_FLYING_HOURS"].ToString().Trim()),
                                                    TRIP_ID = _dtTicket["TCK_TRIP_NO"].ToString().Trim(),
                                                }).ToList();
                        var qryTicketDetail = qryTicketDetails.GroupBy(grp => grp.TRIP_ID).Select(grp => grp.ToList()).ToList();
                        strTicketDetails = JsonConvert.SerializeObject(qryTicketDetail);
                        #endregion

                        #region Passenger Details
                        var qryPaxDetails = (from _dtPax in dsViewPNR_Details.Tables[0].AsEnumerable()
                                             select new
                                             {
                                                 TICKET_NO = _dtPax["TICKET_NO"].ToString().Trim(),
                                                 PASSENGER_TYPE = _dtPax["PASSENGER_TYPE"].ToString().Trim(),
                                                 PASSENGER_NAME = _dtPax["PASSENGER_NAME"].ToString().Trim(),
                                                 STATUS = _dtPax["TICKET_STATUS"].ToString().ToUpper().Trim(),
                                                 MEALS = _dtPax["MEALS"].ToString().Trim(),
                                                 BAGGAGE = _dtPax["BAGGAGE"].ToString().Trim(),
                                                 SEAT = _dtPax["SEAT"].ToString().Trim(),
                                                 PAX_REF_NO = _dtPax["PAX_REF_NO"].ToString().Trim(),
                                             }).Distinct().ToList();
                        var qryPaxDetail = qryPaxDetails.GroupBy(PaxRef => PaxRef.PAX_REF_NO).Select(grp => grp.ToList()).ToList();
                        strPaxDetails = JsonConvert.SerializeObject(qryPaxDetail);
                        #endregion

                        strStatus = "01";
                        strMessage = "";
                        strResult = strTicketDetails;
                    }
                    else
                    {
                        strStatus = "00";
                        strResult = strRefError + " ~ERROR~ " + strRefErrorMsg;
                        strMessage = "Unable to Fetch Request details. please contact customer care.";
                    }
                }
                else
                {
                    strStatus = "00";
                    strResult = strRefError + " ~ERROR~ " + strRefErrorMsg;
                    strMessage = "Unable to Fetch Request details. please contact customer care.";
                }

                strLogData = "<URL>[< ![CDATA[" + _RaysService.Url + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                        + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<RESPONSE>" + strResult + "<RESPONSE>"
                        + "<VIEWPNRDETAILS>"
                        + (dsViewPNR_Details != null && dsViewPNR_Details.Tables.Count > 0 ? dsViewPNR_Details.Tables[0].Rows.Count.ToString() : "NO DATA")
                        + "</VIEWPNRDETAILS>"
                        + "<VIEWPNR_DISPLAY>"
                        + (dsViewPNR_DispDetails != null && dsViewPNR_DispDetails.Tables.Count > 0 ? dsViewPNR_DispDetails.Tables[0].Rows.Count.ToString() : "NO DATA")
                        + "</VIEWPNR_DISPLAY>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "FetchMyViewPNR~RES", strLogData, strAgentID, strTerminalID, strSequenceid);
            }
            catch (Exception ex)
            {
                strStatus = "05";
                strResult = ex.ToString();
                strMessage = "Unable to Fetch Request details. please contact customer care.";
                strLogData = "<URL>[< ![CDATA[" + _RaysService.Url + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<ERROR>" + strResult + "<ERROR>";
                DatabaseLog.LogData(strUserName, "X", "B2CController", "FetchMyViewPNR~Err", strLogData, strAgentID, strTerminalID, strSequenceid);
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage, strPaymentDetails = strPaymentDetails, strPaxDetails = strPaxDetails });
        }
        #endregion

        #region MyTravellers B2C
        [CustomFilter]
        public ActionResult FetchMyTravellers(B2C_Class.MyTravellers _MyTravellers)
        {
            string strStatus = string.Empty;
            string strResult = string.Empty;
            string strMessage = string.Empty;
            string strLogData = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string strSequenceid = string.Empty;
            string strUserEmailID = string.Empty;
            string strUserMobileNo = string.Empty;
            string strUrl = string.Empty;
            Hashtable hsParam = new Hashtable();
            try
            {
                strAgentID = (Session["POS_ID"] != null && Session["POS_ID"].ToString() != "") ? Session["POS_ID"].ToString() : "";
                strTerminalId = (Session["POS_TID"] != null && Session["POS_TID"].ToString() != "") ? Session["POS_TID"].ToString() : "";
                strUserName = (Session["username"] != null && Session["username"].ToString() != "") ? Session["username"].ToString() : "";
                Ipaddress = (Session["ipAddress"] != null && Session["ipAddress"].ToString() != "") ? Session["ipAddress"].ToString() : "";
                strSequenceid = (Session["sequenceid"] != null && Session["sequenceid"].ToString() != "") ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                strUserMobileNo = Session["USERMOBILENO"] != null && Session["USERMOBILENO"].ToString() != "" ? Session["USERMOBILENO"].ToString() : "";
                strUserEmailID = Session["USEREMAILID"] != null && Session["USEREMAILID"].ToString() != "" ? Session["USEREMAILID"].ToString() : "";

                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<REQUEST><AGENTID>" + strAgentID + "</AGENTID>"
                    + "<TERMINALID>" + strTerminalId + "</TERMINALID>" + "<USERNAME>" + strUserName + "</USERNAME>"
                    + "<IPADDRESS>" + Ipaddress + "</IPADDRESS>" + "<SEQUENCEID>" + strSequenceid + "</SEQUENCEID>"
                    + "<USERMOBILE>" + strUserMobileNo + "</USERMOBILE>" + "<USEREMAIL>" + strUserEmailID + "</USEREMAIL>"
                    + "<REQUEST>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "FetchMyTravellers~REQ", strLogData, strAgentID, strTerminalId, strSequenceid);

                hsParam.Add("strAgentID", strAgentID);
                hsParam.Add("strTerminalID", strTerminalId);
                hsParam.Add("strUserName", strUserName);
                hsParam.Add("strSequenceID", strSequenceid);
                hsParam.Add("strIPAddress", Ipaddress);
                hsParam.Add("strTerminalType", "W");
                hsParam.Add("strUserEmailID", strUserName);
                hsParam.Add("strUserMobileNo", strUserMobileNo);
                hsParam.Add("strResult", "");
                hsParam.Add("strErrorMessage", "");
                hsParam.Add("_MyTravellers", JsonConvert.SerializeObject(_MyTravellers));

                JObject objJsonSeq = Base.callWebMethod("MyTravellers", hsParam, ref strMessage);

                if (objJsonSeq != null)
                {
                    bool JsonBook = (bool)objJsonSeq["MyTravellersResult"];
                    if (JsonBook == true)
                    {
                        strResult = (string)objJsonSeq["strResult"];
                        strMessage = (string)objJsonSeq["strErrorMessage"];

                        DataSet dsResult = JsonConvert.DeserializeObject<DataSet>(strResult);
                        if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                        {
                            strStatus = "01";
                            strResult = JsonConvert.SerializeObject(dsResult.Tables[0]);
                        }
                        else
                        {
                            strStatus = "00";
                            strResult = "No Records found.";
                        }
                    }
                    else
                    {
                        strStatus = "00";
                        strResult = "Unable to Request Details. Please contact support team (#01).";
                    }
                }
                else
                {
                    strStatus = "00";
                    strResult = strMessage;
                }

                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<RESPONSE>" + strResult + "<RESPONSE>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "FetchMyTravellers~RES", strLogData, strAgentID, strTerminalId, strSequenceid);
            }
            catch (Exception ex)
            {
                strStatus = "05";
                strResult = ex.ToString();
                strMessage = "Unable to Fetch Request details. please contact customer care.";
                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<ERROR>" + strResult + "<ERROR>";
                DatabaseLog.LogData(strUserName, "X", "B2CController", "FetchMyTravellers~Err", strLogData, strAgentID, strTerminalId, strSequenceid);
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage });
        }
        #endregion

        #region MyProfile B2C
        [CustomFilter]
        [HttpPost]
        public ActionResult UpdateMyProfile()
        {
            string strStatus = string.Empty;
            string strResult = string.Empty;
            string strMessage = string.Empty;
            string strLogData = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string strSequenceid = string.Empty;
            string strTitle = string.Empty;
            string strFirstname = string.Empty;
            string strLastname = string.Empty;
            string strEmail = string.Empty;
            string strContact = string.Empty;
            string strDOB = string.Empty;
            string strCountry = string.Empty;
            string strPassportNo = string.Empty;
            string strPassportExp = string.Empty;
            string strAddress = string.Empty;
            string strCity = string.Empty;
            string strPincode = string.Empty;
            string strUrl = string.Empty;
            string strImageData = string.Empty;
            string strPassWord = string.Empty;
            string strProfileUrl = string.Empty;
            try
            {
                strAgentID = (Session["POS_ID"] != null && Session["POS_ID"].ToString() != "") ? Session["POS_ID"].ToString() : "";
                strTerminalId = (Session["POS_TID"] != null && Session["POS_TID"].ToString() != "") ? Session["POS_TID"].ToString() : "";
                strUserName = (Session["username"] != null && Session["username"].ToString() != "") ? Session["username"].ToString() : "";
                Ipaddress = (Session["ipAddress"] != null && Session["ipAddress"].ToString() != "") ? Session["ipAddress"].ToString() : "";
                strSequenceid = (Session["sequenceid"] != null && Session["sequenceid"].ToString() != "") ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                strPassWord = (Session["PWD"] != null && Session["PWD"].ToString() != "") ? Session["PWD"].ToString() : "";
                strUrl = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                #region DataSet for Customer Profile Update
                strTitle = Request["TITLE"].ToString();
                strFirstname = Request["FIRSTNAME"].ToString();
                strLastname = Request["LASTNAME"].ToString();
                strEmail = Request["EMAIL"].ToString();
                strContact = Request["MOBILENO"].ToString();
                strDOB = Request["DOB"].ToString();
                strCountry = Request["PASSPORTCOUNTRY"].ToString();
                strPassportNo = Request["PASSPORTNO"].ToString();
                strPassportExp = Request["PASSPORTEXP"].ToString();
                strAddress = Request["ADDRESS"].ToString();
                strCity = Request["CITY"].ToString();
                strPincode = Request["PINCODE"].ToString();

                HttpFileCollectionBase files = Request.Files;
                if (files.Count > 0)
                {
                    HttpPostedFileBase file = files[0];
                    for (int i = 0; i < files.Count; i++)
                    {
                        string fname = string.Empty;
                        HttpPostedFileBase file1 = files[i];
                        Stream stream = file1.InputStream;
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
                        if (!Directory.Exists(Server.MapPath(@"~/PDF/B2CUserLogo/")))
                        {
                            Directory.CreateDirectory(Server.MapPath(@"~/PDF/B2CUserLogo/"));
                        }
                        fname = Path.Combine(Server.MapPath(@"~/PDF/B2CUserLogo/"), fname);
                        file1.SaveAs(fname);
                        FileStream fs = new FileStream(fname.ToString() + "", FileMode.Open, FileAccess.Read);
                        byte[] Image = new byte[fs.Length];
                        Image = Base.ReadBitmap2ByteArray(fname.ToString() + "");
                        fs.Close();
                        System.IO.File.Delete(fname.ToString() + "");
                        strImageData = Convert.ToBase64String(Image);
                    }
                }

                DataSet dsRegistration = new DataSet();
                DataTable dtRegistration = new DataTable();
                dtRegistration.Columns.Add("IP");//1
                dtRegistration.Columns.Add("SEQUENCE");//2
                dtRegistration.Columns.Add("USERID");//3
                dtRegistration.Columns.Add("FLAG");//4
                dtRegistration.Columns.Add("BRANCH");//5
                dtRegistration.Columns.Add("AGENCYID");//6
                dtRegistration.Columns.Add("AGENCYNAME");//7
                dtRegistration.Columns.Add("PASSWORD");//8
                dtRegistration.Columns.Add("MOBILE");//9
                dtRegistration.Columns.Add("AGENTTYPE");//10
                dtRegistration.Columns.Add("TITLE");//11
                dtRegistration.Columns.Add("FIRSTNAME");//12
                dtRegistration.Columns.Add("LASTNAME");//13
                dtRegistration.Columns.Add("DOB");//14
                dtRegistration.Columns.Add("COUNTRY");//15
                dtRegistration.Columns.Add("STATE");//16
                dtRegistration.Columns.Add("CITY");//17
                dtRegistration.Columns.Add("ADDRESS1");//18
                dtRegistration.Columns.Add("ADDRESS2");//19
                dtRegistration.Columns.Add("EMAIL");//20
                dtRegistration.Columns.Add("ALTEREMAIL");//21
                dtRegistration.Columns.Add("FAX");//22
                dtRegistration.Columns.Add("TERMINALCOUNT");//23
                dtRegistration.Columns.Add("PHONE");//24
                dtRegistration.Columns.Add("STATUS");//25
                dtRegistration.Columns.Add("RCODE");//26
                dtRegistration.Columns.Add("PAYMENTMODE");//27
                dtRegistration.Columns.Add("Groupname");//28
                dtRegistration.Columns.Add("CLT_GROUP_ID");//29
                dtRegistration.Columns.Add("Newflag");//30
                dtRegistration.Columns.Add("AGENT_CURRENCY");//31
                dtRegistration.Columns.Add("getCBTReportsAccess");//32
                dtRegistration.Columns.Add("CLIENTLOGO");//33
                dtRegistration.Columns.Add("ICUSTID");
                dtRegistration.Columns.Add("IATANO");
                dtRegistration.Columns.Add("JOINDATE");
                dtRegistration.Columns.Add("CURRENTDEPOSIT");
                dtRegistration.Columns.Add("SALESMAN");
                dtRegistration.Columns.Add("SALESMAN1");
                dtRegistration.Columns.Add("SALESMAN2");
                dtRegistration.Columns.Add("LATITUDE");
                dtRegistration.Columns.Add("LONGITUDE");
                dtRegistration.Columns.Add("AG_LOGO");
                dtRegistration.Columns.Add("PGSERVICECHARGE");
                dtRegistration.Columns.Add("JOINTYPE");
                dtRegistration.Columns.Add("SSD_REF_ID");
                dtRegistration.Columns.Add("Balancecheck");
                dtRegistration.Columns.Add("PAN_NO");
                dtRegistration.Columns.Add("PASSPORTFLAG");
                dtRegistration.Columns.Add("PASSPORTNUMBER");
                dtRegistration.Columns.Add("PASSEXPDATE");
                dtRegistration.Columns.Add("PASSISSUEDCOUNTRY");
                dtRegistration.Columns.Add("PINCODE");
                dtRegistration.Columns.Add("FFN");
                dtRegistration.Columns.Add("TERMINALFLAG");

                strDOB = DateTime.ParseExact(strDOB, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                if (!string.IsNullOrEmpty(strPassportExp))
                {
                    strPassportExp = DateTime.ParseExact(strPassportExp, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                }

                dtRegistration.Rows.Add("", 0, "", "UPDATE", "", strAgentID, strEmail, strPassWord, strContact, "CS",
                    strTitle, strFirstname, strLastname, strDOB, "", "", strCity, strAddress, "", strEmail,
                    "", "", "", "", "", "", "P", "", "", "", "INR", "", strImageData, "", "", "", "", "", "",
                    "", "", "", (object)DBNull.Value, "", "", "", "", "", "Y", strPassportNo, strPassportExp,
                    strCountry, strPincode, "", "W");

                dsRegistration.Tables.Add(dtRegistration);
                #endregion

                #region Profile Update
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<REQUEST>" + dsRegistration.GetXml() + "<REQUEST>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "UpdateMyProfile~REQ", strLogData, strAgentID, strTerminalId, strSequenceid);

                string[] strResponse = _RaysService.SetCustomer_Con(dsRegistration);

                try
                {
                    if (ConfigurationManager.AppSettings["DashboardProfile"] != null && ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("PDF/B2CUserLogo"))
                    {
                        var filePath = Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("~/PDF/B2CUserLogo/"), "*.*")
                                         .Where(v => Path.GetFileName(v) == strAgentID + Path.GetExtension(Path.GetFileName(v))).ToList();
                        if (filePath.Count == 0)
                        {
                            strProfileUrl = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                        }
                        else
                        {
                            strProfileUrl = ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png";
                        }
                    }
                    else
                    {
                        string strProfilePath = ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("|") ? (ConfigurationManager.AppSettings["DashboardProfile"].ToString().Split('|')[1] + strAgentID + ".png") : (ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png");

                        bool CheckURl = Base.CheckUrlStatus(strProfilePath);

                        if (CheckURl == true)
                        {
                            strProfileUrl = ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("|") ? ConfigurationManager.AppSettings["DashboardProfile"].ToString().Split('|')[0] + strAgentID + ".png" : ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png";
                        }
                        else
                        {
                            strProfileUrl = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                        }

                    }

                }
                catch (Exception ex)
                {
                    strProfileUrl = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                }

                if (strResponse[0].Contains("CUSTOMER UPDATED"))
                {
                    strStatus = "01";
                    strResult = strResponse[0];
                    strMessage = strResponse[0];
                    System.Web.HttpContext.Current.Session.Add("USERFNAME", strFirstname);
                    System.Web.HttpContext.Current.Session.Add("USERLNAME", strLastname);
                    System.Web.HttpContext.Current.Session.Add("USERMOBILENO", strContact);
                    System.Web.HttpContext.Current.Session.Add("USERNAME", strEmail);
                    System.Web.HttpContext.Current.Session.Add("USERTITLE", strTitle);
                    System.Web.HttpContext.Current.Session.Add("USERADDRESS", strAddress);
                    System.Web.HttpContext.Current.Session.Add("USERCITY", strCity);
                    System.Web.HttpContext.Current.Session.Add("USERPASSPORTNO", strPassportNo);
                    System.Web.HttpContext.Current.Session.Add("USEROB", DateTime.ParseExact(strDOB, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"));
                    System.Web.HttpContext.Current.Session.Add("USERPASSPORT_EXP", DateTime.ParseExact(strPassportExp, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy"));
                    System.Web.HttpContext.Current.Session.Add("USERPASS_ISSU_COUNTRY", strCountry);
                    System.Web.HttpContext.Current.Session.Add("USERPINCODE", strPincode);
                    System.Web.HttpContext.Current.Session.Add("USERPROFILEPIC", strProfileUrl);
                }
                else
                {
                    strStatus = "00";
                    strResult = strResponse[0];
                    strMessage = "Unable to update details.please contact customer care.";
                }

                strLogData = "<RESPONSE>" + JsonConvert.SerializeObject(strResponse) + "</RESPONSE>";
                #endregion  

                DatabaseLog.LogData(strUserName, "E", "B2CController", "UpdateMyProfile~RES", strLogData, strAgentID, strTerminalId, strSequenceid);
            }
            catch (Exception ex)
            {
                strStatus = "05";
                strResult = ex.ToString();
                strMessage = "Unable to update details.please contact customer care.";
                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<ERROR>" + strResult + "<ERROR>";
                DatabaseLog.LogData(strUserName, "X", "B2CController", "UpdateMyProfile~Err", strLogData, strAgentID, strTerminalId, strSequenceid);
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage });
        }

        //[CustomFilter]
        //[HttpPost]
        //For Mobile Update Profile
        public JsonResult UpdateMyProfile_Mobile()
        {

            string strStatus = string.Empty;
            string strResult = string.Empty;
            string strMessage = string.Empty;
            string strProfileUrl = string.Empty;
            string strLogData = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string strSequenceid = string.Empty;
            string strTitle = string.Empty;
            string strFirstname = string.Empty;
            string strLastname = string.Empty;
            string strEmail = string.Empty;
            string strContact = string.Empty;
            string strDOB = string.Empty;
            string strCountry = string.Empty;
            string strPassportNo = string.Empty;
            string strPassportExp = string.Empty;
            string strAddress = string.Empty;
            string strCity = string.Empty;
            string strPincode = string.Empty;
            string strImage = string.Empty;
            string strUrl = string.Empty;
            string strImageData = string.Empty;
            string strPassWord = string.Empty;

            try
            {

                strUrl = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                #region DataSet for Customer Profile Update
                strTitle = Request.Form["TITLE"];
                strFirstname = Request.Form["FIRSTNAME"];
                strLastname = Request.Form["LASTNAME"];
                strEmail = Request.Form["EMAIL"];
                strContact = Request.Form["MOBILENO"];
                strDOB = Request.Form["DOB"];
                strCountry = Request.Form["PASSPORTCOUNTRY"];
                strPassportNo = Request.Form["PASSPORTNO"];
                strPassportExp = Request.Form["PASSPORTEXP"];
                strAddress = Request.Form["ADDRESS"];
                strCity = Request.Form["CITY"];
                strPincode = Request.Form["PINCODE"];

                strAgentID = Request.Form["AGENTID"];
                strTerminalId = Request.Form["TERMINALID"];
                strUserName = Request.Form["USERNAME"];
                Ipaddress = Request.Form["IPADDRESS"];
                strSequenceid = Request.Form["SEQNO"];
                strPassWord = Request.Form["PASSWORD"];


                HttpFileCollectionBase files = Request.Files;
                if (files.Count > 0)
                {
                    HttpPostedFileBase file = files[0];
                    string ImageName = "";
                    for (int i = 0; i < files.Count; i++)
                    {
                        string fname = string.Empty;
                        HttpPostedFileBase file1 = files[i];
                        Stream stream = file1.InputStream;
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
                            string Imgextension = Path.GetExtension(file1.FileName);
                            string ImageNamewoe = DateTime.Now.ToString("yyyyMMddHHmmss");

                            ImageName = ImageNamewoe + Imgextension;

                            //  fname = file1.FileName;
                        }
                        var filePath = Server.MapPath("~/PDF/B2CUserLogo/" + ImageName);

                        if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/PDF/B2CUserLogo/")))
                        {
                            Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/PDF/B2CUserLogo/"));
                        }

                        file1.SaveAs(filePath);

                        FileStream fs = new FileStream(filePath.ToString() + "", FileMode.Open, FileAccess.Read);
                        byte[] Image = new byte[fs.Length];
                        Image = Base.ReadBitmap2ByteArray(filePath.ToString() + "");
                        fs.Close();
                        System.IO.File.Delete(filePath.ToString() + "");
                        strImageData = Convert.ToBase64String(Image);
                    }
                }
                string strCurrencyFlag = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["currency"]) ? ConfigurationManager.AppSettings["currency"].ToString() : "";
                DataSet dsRegistration = new DataSet();
                DataTable dtRegistration = new DataTable();
                dtRegistration.Columns.Add("IP");//1
                dtRegistration.Columns.Add("SEQUENCE");//2
                dtRegistration.Columns.Add("USERID");//3
                dtRegistration.Columns.Add("FLAG");//4
                dtRegistration.Columns.Add("BRANCH");//5
                dtRegistration.Columns.Add("AGENCYID");//6
                dtRegistration.Columns.Add("AGENCYNAME");//7
                dtRegistration.Columns.Add("PASSWORD");//8
                dtRegistration.Columns.Add("MOBILE");//9
                dtRegistration.Columns.Add("AGENTTYPE");//10
                dtRegistration.Columns.Add("TITLE");//11
                dtRegistration.Columns.Add("FIRSTNAME");//12
                dtRegistration.Columns.Add("LASTNAME");//13
                dtRegistration.Columns.Add("DOB");//14
                dtRegistration.Columns.Add("COUNTRY");//15
                dtRegistration.Columns.Add("STATE");//16
                dtRegistration.Columns.Add("CITY");//17
                dtRegistration.Columns.Add("ADDRESS1");//18
                dtRegistration.Columns.Add("ADDRESS2");//19
                dtRegistration.Columns.Add("EMAIL");//20
                dtRegistration.Columns.Add("ALTEREMAIL");//21
                dtRegistration.Columns.Add("FAX");//22
                dtRegistration.Columns.Add("TERMINALCOUNT");//23
                dtRegistration.Columns.Add("PHONE");//24
                dtRegistration.Columns.Add("STATUS");//25
                dtRegistration.Columns.Add("RCODE");//26
                dtRegistration.Columns.Add("PAYMENTMODE");//27
                dtRegistration.Columns.Add("Groupname");//28
                dtRegistration.Columns.Add("CLT_GROUP_ID");//29
                dtRegistration.Columns.Add("Newflag");//30
                dtRegistration.Columns.Add("AGENT_CURRENCY");//31
                dtRegistration.Columns.Add("getCBTReportsAccess");//32
                dtRegistration.Columns.Add("CLIENTLOGO");//33
                dtRegistration.Columns.Add("ICUSTID");
                dtRegistration.Columns.Add("IATANO");
                dtRegistration.Columns.Add("JOINDATE");
                dtRegistration.Columns.Add("CURRENTDEPOSIT");
                dtRegistration.Columns.Add("SALESMAN");
                dtRegistration.Columns.Add("SALESMAN1");
                dtRegistration.Columns.Add("SALESMAN2");
                dtRegistration.Columns.Add("LATITUDE");
                dtRegistration.Columns.Add("LONGITUDE");
                dtRegistration.Columns.Add("AG_LOGO");
                dtRegistration.Columns.Add("PGSERVICECHARGE");
                dtRegistration.Columns.Add("JOINTYPE");
                dtRegistration.Columns.Add("SSD_REF_ID");
                dtRegistration.Columns.Add("Balancecheck");
                dtRegistration.Columns.Add("PAN_NO");
                dtRegistration.Columns.Add("PASSPORTFLAG");
                dtRegistration.Columns.Add("PASSPORTNUMBER");
                dtRegistration.Columns.Add("PASSEXPDATE");
                dtRegistration.Columns.Add("PASSISSUEDCOUNTRY");
                dtRegistration.Columns.Add("PINCODE");
                dtRegistration.Columns.Add("FFN");
                dtRegistration.Columns.Add("TERMINALFLAG");

                strDOB = DateTime.ParseExact(strDOB, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                if (!string.IsNullOrEmpty(strPassportExp))
                {
                    strPassportExp = DateTime.ParseExact(strPassportExp, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                }

                dtRegistration.Rows.Add("", 0, "", "UPDATE", "", strAgentID, strEmail, strPassWord, strContact, "CS",
                    strTitle, strFirstname, strLastname, strDOB, "", "", strCity, strAddress, "", strEmail,
                    "", "", "", "", "", "", "P", "", "", "", strCurrencyFlag, "", strImageData, "", "", "", "", "", "",
                    "", "", "", (object)DBNull.Value, "", "", "", "", "", "Y", strPassportNo, strPassportExp,
                    strCountry, strPincode, "", "W");

                dsRegistration.Tables.Add(dtRegistration);
                #endregion

                #region Registration
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<REQUEST>" + dsRegistration.GetXml() + "<REQUEST>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "Mobile_UpdateMyProfile~REQ", strLogData, strAgentID, strTerminalId, strSequenceid);

                string[] strResponse = _RaysService.SetCustomer_Con(dsRegistration);

                if (strResponse[0].Contains("CUSTOMER UPDATED"))
                {
                    strStatus = "01";
                    strResult = strResponse[0];
                    strMessage = strResponse[0];
                    strProfileUrl = ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png";
                    strProfileUrl = Base.ChangeToLanIP(strProfileUrl);

                    try
                    {
                        if (ConfigurationManager.AppSettings["DashboardProfile"] != null && ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("PDF/B2CUserLogo"))
                        {
                            var filePath = Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("~/PDF/B2CUserLogo/"), "*.*")
                                             .Where(v => Path.GetFileName(v) == strAgentID + Path.GetExtension(Path.GetFileName(v))).ToList();
                            if (filePath.Count == 0)
                            {
                                strProfileUrl = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                            }
                            else
                            {
                                strProfileUrl = ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png";
                            }
                        }
                        else
                        {
                            string strProfilePath = ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("|") ? (ConfigurationManager.AppSettings["DashboardProfile"].ToString().Split('|')[1] + strAgentID + ".png") : (ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png");

                            bool CheckURl = Base.CheckUrlStatus(strProfilePath);

                            if (CheckURl == true)
                            {
                                strProfileUrl = ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("|") ? ConfigurationManager.AppSettings["DashboardProfile"].ToString().Split('|')[0] + strAgentID + ".png" : ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png";
                            }
                            else
                            {
                                strProfileUrl = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        strProfileUrl = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                    }

                    System.Web.HttpContext.Current.Session.Add("USERFNAME", strFirstname);
                    System.Web.HttpContext.Current.Session.Add("USERLNAME", strLastname);
                    System.Web.HttpContext.Current.Session.Add("USERMOBILENO", strContact);
                    System.Web.HttpContext.Current.Session.Add("USERNAME", strEmail);
                    System.Web.HttpContext.Current.Session.Add("USERTITLE", strTitle);
                    System.Web.HttpContext.Current.Session.Add("USERADDRESS", strAddress);
                    System.Web.HttpContext.Current.Session.Add("USERCITY", strCity);
                    System.Web.HttpContext.Current.Session.Add("USERPASSPORTNO", strPassportNo);
                    System.Web.HttpContext.Current.Session.Add("USEROB", strDOB != "" ? DateTime.ParseExact(strDOB, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") : "");
                    System.Web.HttpContext.Current.Session.Add("USERPASSPORT_EXP", strPassportExp != "" ? DateTime.ParseExact(strPassportExp, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") : "");
                    System.Web.HttpContext.Current.Session.Add("USERPASS_ISSU_COUNTRY", strCountry);
                    System.Web.HttpContext.Current.Session.Add("USERPINCODE", strPincode);
                }
                else
                {
                    strStatus = "00";
                    strResult = strResponse[0];
                    strMessage = "Unable to update details.please contact customer care.";
                }

                strLogData = "<RESPONSE>" + JsonConvert.SerializeObject(strResponse) + "</RESPONSE>";
                #endregion  

                DatabaseLog.LogData(strUserName, "E", "B2CController", "Mobile_UpdateMyProfile~RES", strLogData, strAgentID, strTerminalId, strSequenceid);
            }
            catch (Exception ex)
            {
                strStatus = "05";
                strResult = ex.ToString();
                strMessage = "Unable to update details.please contact customer care.";
                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<ERROR>" + strResult + "<ERROR>";
                DatabaseLog.LogData(strUserName, "X", "B2CController", "Mobile_UpdateMyProfile~Err", strLogData, strAgentID, strTerminalId, strSequenceid);
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage, ProfileUrl = strProfileUrl }, JsonRequestBehavior.AllowGet);

        }
        //For Mobile Fetch Profile
        public JsonResult MyProfile_Mobile(string strUsername, string strPassword, string strTerminalType)
        {
            string strLogUsername = string.Empty;
            string strLogAgentID = string.Empty;
            string strLogTerminalID = string.Empty;
            string strLogIPAddress = string.Empty;
            string strLogSequence = string.Empty;
            string strLogTerminalType = string.Empty;

            string strResponse = string.Empty;
            string strLogData = string.Empty;
            string strStatus = string.Empty;
            string strMessage = string.Empty;

            DataSet dsLogin = new DataSet();

            string strTerminalID = string.Empty, strAgentID = string.Empty;
            try
            {
                strLogSequence = DateTime.Now.ToString("yyMMdd");
                strLogUsername = strUsername;
                strTerminalType = (!string.IsNullOrEmpty(strTerminalType) ? strTerminalType : "W");
                strLogTerminalType = strTerminalType;
                strLogIPAddress = Base.GetComputer_IP();
                strLogAgentID = ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString().Substring(0, 12);
                strLogTerminalID = ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString();


                #region Login
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                dsLogin = _RaysService.GET_LOGIN_DETAILS("", strUsername, strPassword, "W", "", "", "");

                if (dsLogin != null && dsLogin.Tables.Count > 0 && dsLogin.Tables[0].Rows.Count > 0)
                {
                    if (dsLogin.Tables[0].Columns.Contains("ERROR"))
                    {
                        strStatus = "00";// dsLogin.Tables[0].Rows[0]["ERROR"].ToString();
                        strResponse = dsLogin.Tables[0].Rows[0]["Result"].ToString();
                    }
                    else
                    {
                        strStatus = "01";
                        strTerminalID = dsLogin.Tables[0].Rows[0]["LGN_TERMINAL_ID"].ToString().Trim().ToUpper();
                        strAgentID = dsLogin.Tables[0].Rows[0]["LGN_AGENT_ID"].ToString().Trim().ToUpper();

                        string strProfilePIC = dsLogin.Tables[0].Rows[0]["LGN_PROFILE_PIC"].ToString().Trim();
                        string strUserFName = dsLogin.Tables[0].Rows[0]["LGN_AGENT_FIRSTNAME"].ToString().Trim();
                        string strUserLName = dsLogin.Tables[0].Rows[0]["LGN_AGENT_LASTNAME"].ToString().Trim();
                        string strMobileNO = dsLogin.Tables[0].Rows[0]["LGN_MOBILE_NO"].ToString().Trim().ToUpper();
                        strLogSequence = dsLogin.Tables[0].Columns.Contains("SEQUENCE_ID") ? dsLogin.Tables[0].Rows[0]["SEQUENCE_ID"].ToString() : DateTime.Now.ToString("yyyyMMdd");

                        MobileProfileRQRS _MobileProfileRQRS = new MobileProfileRQRS();
                        _MobileProfileRQRS.strAgentID = strAgentID;
                        _MobileProfileRQRS.strTerminalID = strTerminalID;
                        _MobileProfileRQRS.strUsername = strUsername;
                        _MobileProfileRQRS.strPassword = strPassword;
                        //  _MobileProfileRQRS.strProfilePIC = strProfilePIC;
                        _MobileProfileRQRS.strUserFName = strUserFName;
                        _MobileProfileRQRS.strUserLName = strUserLName;
                        _MobileProfileRQRS.strMobileNo = strMobileNO;
                        _MobileProfileRQRS.strUserTitle = dsLogin.Tables[0].Rows[0]["LGN_AGENT_TITLE"].ToString().Trim();
                        _MobileProfileRQRS.strAddress = dsLogin.Tables[0].Rows[0]["LGN_ADDRESS_1"].ToString().Trim();
                        _MobileProfileRQRS.strCity = dsLogin.Tables[0].Rows[0]["LGN_CITY_ID"].ToString().Trim();
                        _MobileProfileRQRS.strCountry = dsLogin.Tables[0].Rows[0]["LGN_COUNTRY_ID"].ToString().Trim();
                        _MobileProfileRQRS.strPassportNo = dsLogin.Tables[0].Rows[0]["PASSPORT_NO"].ToString().Trim();
                        _MobileProfileRQRS.strDOB = dsLogin.Tables[0].Rows[0]["DOB"].ToString().Trim();
                        _MobileProfileRQRS.strPassportExpireDate = dsLogin.Tables[0].Rows[0]["LGN_PASSPORT_EXPIRY_DATE"].ToString().Trim();
                        _MobileProfileRQRS.strIssuedCountry = dsLogin.Tables[0].Rows[0]["LGN_ISSUED_COUNTRY"].ToString().Trim();
                        _MobileProfileRQRS.strPincode = dsLogin.Tables[0].Rows[0]["LGN_PINCODE"].ToString().Trim();
                        _MobileProfileRQRS.strSeqNo = strLogSequence;
                        _MobileProfileRQRS.strAgnType = dsLogin.Tables[0].Rows[0]["AGN_TYPE"].ToString().Trim();
                        _MobileProfileRQRS.strBranchId = dsLogin.Tables[0].Rows[0]["AGN_BRANCHID"].ToString().Trim();
                        string strProfileUrl = ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png";
                        strProfileUrl = Base.ChangeToLanIP(strProfileUrl);
                        try
                        {
                            if (ConfigurationManager.AppSettings["DashboardProfile"] != null && ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("PDF/B2CUserLogo"))
                            {
                                var filePath = Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath("~/PDF/B2CUserLogo/"), "*.*")
                                                 .Where(v => Path.GetFileName(v) == strAgentID + Path.GetExtension(Path.GetFileName(v))).ToList();
                                if (filePath.Count == 0)
                                {
                                    strProfileUrl = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                                }
                                else
                                {
                                    strProfileUrl = ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png";
                                }
                            }
                            else
                            {
                                string strProfilePath = ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("|") ? (ConfigurationManager.AppSettings["DashboardProfile"].ToString().Split('|')[1] + strAgentID + ".png") : (ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png");

                                bool CheckURl = Base.CheckUrlStatus(strProfilePath);

                                if (CheckURl == true)
                                {
                                    strProfileUrl = ConfigurationManager.AppSettings["DashboardProfile"].ToString().Contains("|") ? ConfigurationManager.AppSettings["DashboardProfile"].ToString().Split('|')[0] + strAgentID + ".png" : ConfigurationManager.AppSettings["DashboardProfile"].ToString() + strAgentID + ".png";
                                }
                                else
                                {
                                    strProfileUrl = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            strProfileUrl = (strLogoUrl + strProductType + "/DASHBOARD/profile_img.png");
                        }
                        _MobileProfileRQRS.strProfilePIC = strProfileUrl;

                        strLogData = "<QUERY_STRING>" + JsonConvert.SerializeObject(_MobileProfileRQRS) + "</QUERY_STRING>";
                        DatabaseLog.LogData(strLogUsername, "E", "B2CController", "MyProfileRes_Mobile", strLogData, "", "", strLogSequence);
                        strResponse = JsonConvert.SerializeObject(_MobileProfileRQRS);
                    }
                }
                else
                {
                    strStatus = "00";
                    strResponse = "unable to get profile details. please contact support team";
                }
                #endregion
            }
            catch (Exception ex)
            {
                strStatus = "00";
                strMessage = "Problem Occured while get Profile details(#05).";
                strLogData = "<ERROR>" + ex.ToString() + "</ERROR>";
                DatabaseLog.LogData(strLogUsername, "E", "B2CController", "GetProfileDetails~Err", strLogData, "", "", strLogSequence);
            }
            return Json(new { Status = strStatus, Response = strResponse }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Reset Password B2C
        public ActionResult UpdateMyPassword(string strOldPassword, string strNewPassword)
        {
            string strStatus = string.Empty;
            string strResult = string.Empty;
            string strMessage = string.Empty;
            string strLogData = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string strTerminalType = string.Empty;
            string strSequenceid = string.Empty;
            string strUserEmailID = string.Empty;
            string strUserMobileNo = string.Empty;
            string strUrl = string.Empty;
            Hashtable hsParam = new Hashtable();
            try
            {
                strAgentID = (Session["POS_ID"] != null && Session["POS_ID"].ToString() != "") ? Session["POS_ID"].ToString() : "";
                strTerminalId = (Session["POS_TID"] != null && Session["POS_TID"].ToString() != "") ? Session["POS_TID"].ToString() : "";
                strUserName = (Session["username"] != null && Session["username"].ToString() != "") ? Session["username"].ToString() : "";
                Ipaddress = (Session["ipAddress"] != null && Session["ipAddress"].ToString() != "") ? Session["ipAddress"].ToString() : "";
                strSequenceid = (Session["sequenceid"] != null && Session["sequenceid"].ToString() != "") ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
                strUserMobileNo = Session["USERMOBILENO"] != null && Session["USERMOBILENO"].ToString() != "" ? Session["USERMOBILENO"].ToString() : "";
                strUserEmailID = Session["USEREMAILID"] != null && Session["USEREMAILID"].ToString() != "" ? Session["USEREMAILID"].ToString() : "";
                strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : "";
                strUrl = ConfigurationManager.AppSettings["RCSUPPORTSERVICEURL"].ToString();
                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<REQUEST><AGENTID>" + strAgentID + "</AGENTID>"
                    + "<TERMINALID>" + strTerminalId + "</TERMINALID>" + "<USERNAME>" + strUserName + "</USERNAME>"
                    + "<IPADDRESS>" + Ipaddress + "</IPADDRESS>" + "<SEQUENCEID>" + strSequenceid + "</SEQUENCEID>"
                    + "<USERMOBILE>" + strUserMobileNo + "</USERMOBILE>" + "<USEREMAIL>" + strUserEmailID + "</USEREMAIL>"
                    + "<strOldPassword>" + strOldPassword + "</strOldPassword>" + "<strNewPassword>" + strNewPassword + "</strNewPassword>"
                    + "<REQUEST>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "UpdatePassword~REQ", strLogData, strAgentID, strTerminalId, strSequenceid);

                hsParam.Add("strAgentID", strAgentID);
                hsParam.Add("strTerminalID", strTerminalId);
                hsParam.Add("strUserName", strUserName);
                hsParam.Add("strSequenceID", strSequenceid);
                hsParam.Add("strIPAddress", Ipaddress);
                hsParam.Add("strTerminalType", strTerminalType);
                hsParam.Add("strUserEmailID", strUserName);
                hsParam.Add("strUserMobileNo", strUserMobileNo);
                hsParam.Add("strResult", "");
                hsParam.Add("strErrorMessage", "");
                hsParam.Add("strOldPassword", strOldPassword);
                hsParam.Add("strNewPassword", strNewPassword);

                JObject objJsonSeq = Base.callWebMethod("UpdatePassword", hsParam, ref strMessage);

                if (objJsonSeq != null)
                {
                    bool JsonBook = (bool)objJsonSeq["UpdatePasswordResult"];
                    strResult = (string)objJsonSeq["strResult"];
                    strMessage = (string)objJsonSeq["strErrorMessage"];
                    if (JsonBook == true)
                    {
                        strStatus = "01";
                        strResult = (string)objJsonSeq["strResult"];
                    }
                    else
                    {
                        strStatus = "00";
                        strResult = "Unable to update password. Please contact customer care (#00).";
                    }
                }
                else
                {
                    strStatus = "00";
                    strResult = !string.IsNullOrEmpty(strMessage) ? strMessage : "Unable to update password. Please contact customer care (#03).";
                }

                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<RESPONSE>" + strResult + "<RESPONSE>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "UpdatePassword~RES", strLogData, strAgentID, strTerminalId, strSequenceid);

            }
            catch (Exception ex)
            {
                strStatus = "05";
                strResult = ex.ToString();
                strMessage = "Unable to update password. Please contact customer care (#05).";
                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<ERROR>" + strResult + "<ERROR>";
                DatabaseLog.LogData(strUserName, "X", "B2CController", "UpdatePassword~Err", strLogData, strAgentID, strTerminalId, strSequenceid);
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage });
        }

        //For Mobile Change Password
        public JsonResult UpdateMyPassword_Mobile(string strOldPassword, string strNewPassword, string strAgentID, string strTerminalId, string strUserName, string Ipaddress, string strSequenceid,
            string strUserMobileNo, string strUserEmailID, string strTerminalType, string strAgentType, string strBranchId, string strPrevillage)
        {
            string strStatus = string.Empty;
            string strResult = string.Empty;
            string strMessage = string.Empty;
            string strLogData = string.Empty;

            string strQueryString = string.Empty;

            string strUrl = string.Empty;
            Hashtable hsParam = new Hashtable();
            try
            {
                strUrl = ConfigurationManager.AppSettings["RCSUPPORTSERVICEURL"].ToString();
                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<REQUEST><AGENTID>" + strAgentID + "</AGENTID>"
                    + "<TERMINALID>" + strTerminalId + "</TERMINALID>" + "<USERNAME>" + strUserName + "</USERNAME>"
                    + "<IPADDRESS>" + Ipaddress + "</IPADDRESS>" + "<SEQUENCEID>" + strSequenceid + "</SEQUENCEID>"
                    + "<USERMOBILE>" + strUserMobileNo + "</USERMOBILE>" + "<USEREMAIL>" + strUserEmailID + "</USEREMAIL>"
                    + "<strOldPassword>" + strOldPassword + "</strOldPassword>" + "<strNewPassword>" + strNewPassword + "</strNewPassword>"
                    + "<REQUEST>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "UpdatePassword_Mobile~REQ", strLogData, strAgentID, strTerminalId, strSequenceid);

                hsParam.Add("strAgentID", strAgentID);
                hsParam.Add("strTerminalID", strTerminalId);
                hsParam.Add("strUserName", strUserName);
                hsParam.Add("strSequenceID", strSequenceid);
                hsParam.Add("strIPAddress", Ipaddress);
                hsParam.Add("strTerminalType", strTerminalType);
                hsParam.Add("strUserEmailID", strUserName);
                hsParam.Add("strUserMobileNo", strUserMobileNo);
                hsParam.Add("strResult", "");
                hsParam.Add("strErrorMessage", "");
                hsParam.Add("strOldPassword", strOldPassword);
                hsParam.Add("strNewPassword", strNewPassword);

                JObject objJsonSeq = Base.callWebMethod("UpdatePassword", hsParam, ref strMessage);

                if (objJsonSeq != null)
                {
                    bool JsonBook = (bool)objJsonSeq["UpdatePasswordResult"];
                    strResult = (string)objJsonSeq["strResult"];
                    strMessage = (string)objJsonSeq["strErrorMessage"];
                    if (JsonBook == true)
                    {
                        strStatus = "01";
                        strResult = (string)objJsonSeq["strResult"];

                        #region Querystring Formation
                        strQueryString = "AGENTID=" + strAgentID + "&TERMINALID=" + strTerminalId + "&USERNAME=" + strUserName + "&PWD=" + strNewPassword +
                            "&AGENYTYPE=" + strAgentType + "&SEQUENCEID=" + strSequenceid + "&BRANCHID=" + strBranchId + "&TERMINALPREVILAGE=" + strPrevillage +
                            "&TERMINALTYPE=" + strTerminalType + "&IPADDRESS=" + Ipaddress;
                        string strKey = ConfigurationManager.AppSettings["BSA_TERMINALID"].ToString();
                        strQueryString = "SECKEY=" + Base.EncryptKEy(strQueryString, strKey);
                        #endregion
                    }
                    else
                    {
                        strStatus = "00";
                        strResult = "Unable to update password. Please contact customer care (#00).";
                    }
                }
                else
                {
                    strStatus = "00";
                    strResult = !string.IsNullOrEmpty(strMessage) ? strMessage : "Unable to update password. Please contact customer care (#03).";
                }

                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<RESPONSE>" + strResult + "<RESPONSE>";
                DatabaseLog.LogData(strUserName, "E", "B2CController", "UpdatePassword_Mobile~RES", strLogData, strAgentID, strTerminalId, strSequenceid);

            }
            catch (Exception ex)
            {
                strStatus = "05";
                strResult = ex.ToString();
                strMessage = "Unable to update password. Please contact customer care (#05).";
                strLogData = "<URL>[< ![CDATA[" + strUrl + "]] >]<URL>" + "<STATUS>" + strStatus + "<STATUS>"
                    + "<MESSAGE>" + strMessage + "<MESSSAGE>" + "<ERROR>" + strResult + "<ERROR>";
                DatabaseLog.LogData(strUserName, "X", "B2CController", "UpdatePassword_Mobile~Err", strLogData, strAgentID, strTerminalId, strSequenceid);
            }
            return Json(new { Status = strStatus, Result = strResult, Message = strMessage, QueryString = strQueryString });
        }
        #endregion

        #region Forget Password OTP
        public ActionResult GetOTP_B2C(string MailID)
        {
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strErrMessage = string.Empty;
            string strTerminalType = string.Empty;
            string strIPAddress = string.Empty;
            string strSequenceID = DateTime.Now.ToString("yyyyMMdd");
            string strLogData = string.Empty;
            string strResposse = string.Empty;
            try
            {
                string strUrl = ConfigurationManager.AppSettings["RCSUPPORTSERVICEURL"].ToString();
                Hashtable hsParam = new Hashtable();
                hsParam.Add("strMailID", MailID);
                hsParam.Add("strFlag", "I");
                hsParam.Add("strStatus", "");
                hsParam.Add("strMessage", "");

                strLogData = "<EVENT><REQUEST>" + JsonConvert.SerializeObject(hsParam).ToString() + "</REQUEST></EVENT>";
                DatabaseLog.LogData(MailID, "E", "B2CController", "GetOTP_B2C~REQ", strLogData, "", "", strSequenceID);

                JObject objJsonSeq = Base.callWebMethod("B2C_GENERATE_OTP", hsParam, ref strErrMessage);

                if (objJsonSeq != null)
                {
                    bool JsonBook = (bool)objJsonSeq["B2C_GENERATE_OTPResult"];
                    strStatus = (string)objJsonSeq["strStatus"];
                    strMessage = (string)objJsonSeq["strMessage"];
                    if (strStatus != "01")
                    {
                        strStatus = "00";
                    }
                }
                else
                {
                    strStatus = "00";
                    strMessage = "Unable to Send OTP.";
                }

                strLogData = "<EVENT><STATUS>" + strStatus + "</STATUS>"
                    + "<MESSAGE>" + strMessage + "</MESSSAGE><ERRORMESSAGE>" + strErrMessage + "</ERRORMESSAGE></EVENT>";
                DatabaseLog.LogData("", "E", "B2CController", "GetOTP_B2C~RES", strLogData, "", "", strSequenceID);
            }
            catch (Exception ex)
            {
                strStatus = "05";
                strMessage = "unable to generate OTP. Please contact support team.";
                strLogData = "<EVENT><ERROR>" + ex.ToString() + "</ERROR></EVENT>";
                DatabaseLog.LogData("", "X", "B2CController", "GetOTP_B2C~ERR", strLogData, "", "", strSequenceID);
            }
            return Json(new { Status = strStatus, Message = strMessage });
        }
        public ActionResult ForgetPassword_B2C(string MailID, string OTP, string Password,string strFlag)
        {
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strErrMessage = string.Empty;
            string strIPAddress = string.Empty;
            string strSequenceID = DateTime.Now.ToString("yyyyMMdd");
            string strLogData = string.Empty;
            string strResposse = string.Empty;
            strFlag = strFlag != null ? strFlag : "";


            try
            {
                string strUrl = ConfigurationManager.AppSettings["RCSUPPORTSERVICEURL"].ToString();

                Hashtable hsParam = new Hashtable();

                hsParam.Add("strMailID", MailID);
                hsParam.Add("strPassword", Password);
                hsParam.Add("strOTP", OTP);
                hsParam.Add("strFlag", strFlag);
                hsParam.Add("strStatus", "");
                hsParam.Add("strMessage", "");

                strLogData = "<EVENT><REQUEST>" + JsonConvert.SerializeObject(hsParam).ToString() + "</REQUEST></EVENT>";
                DatabaseLog.LogData("", "E", "B2CController", "ForgetPassword_B2C~REQ", strLogData, "", "", strSequenceID);

                JObject objJsonSeq = Base.callWebMethod("B2C_VERIFY_OTP", hsParam, ref strErrMessage);

                if (objJsonSeq != null)
                {
                    bool JsonBook = (bool)objJsonSeq["B2C_VERIFY_OTPResult"];
                    strStatus = (string)objJsonSeq["strStatus"];
                    strMessage = (string)objJsonSeq["strMessage"];
                    if (strStatus != "01")
                    {
                        strStatus = "00";
                    }
                }
                else
                {
                    strStatus = "00";
                    strMessage = strFlag=="V"? "Unable to verify OTP.":"Unable to update password.";
                }

                strLogData = "<EVENT>" + "<STATUS>" + strStatus + "</STATUS>"
                  + "<MESSAGE>" + strMessage + "</MESSSAGE><ERRORMESSAGE>" + strErrMessage + "</ERRORMESSAGE></EVENT>";

                DatabaseLog.LogData("", "E", "B2CController", "ForgetPassword_B2C~RES", strLogData, "", "", strSequenceID);



            }
            catch (Exception ex)
            {
                strStatus = "05";
                strMessage = "unable to generate OTP. Please contact support team.";
                strLogData = "<EVENT><ERROR>" + ex.ToString() + "</ERROR></EVENT>";
                DatabaseLog.LogData("", "X", "B2CController", "ForgetPassword_B2C~ERR", strLogData, "", "", strSequenceID);
            }
            return Json(new { Status = strStatus, Message = strMessage});
        }
        #endregion
    }
}
