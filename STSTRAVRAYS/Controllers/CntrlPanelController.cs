using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STSTRAVRAYS.Models;
using STSTRAVRAYS.Rays_service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace STSTRAVRAYS.Controllers
{
    [CustomFilter]
    public class CntrlPanelController : LoginController
    {
        Base.ServiceUtility Serv = new Base.ServiceUtility();
        string strBranchCredit = ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"] != null ? ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"].ToString().ToUpper() : "";
        string strPlatform = ConfigurationManager.AppSettings["PLATFORM"].ToString();
        public ActionResult Changepassword()
        {
            if (Session["agentid"] == null || Session["agentid"].ToString() == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            #region UsageLog
            string PageName = "Change Password";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e) { }
            #endregion

            return View("~/Views/CntrlPanel/Changepassword.cshtml");
        }
        public ActionResult BookingCommApproval()
        {
            try
            {
                ViewBag.ServerDateTime = Base.LoadServerdatetime();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult Agentdetail()
        {
            if (Session["agentid"] == null || Session["agentid"].ToString() == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            #region UsageLog
            string PageName = "AgentDetails";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e) { }
            #endregion
            imageview();
            return View("~/Views/CntrlPanel/Agentdetail.cshtml");
        }

        public ActionResult Outstanding()
        {
            #region UsageLog
            string PageName = "Agent Outstanding";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e) { }
            #endregion
            if (Request.QueryString["SECKEY"] != null && Request.QueryString["SECKEY"] != "")
            {
                string Encquery = Request.QueryString["SECKEY"];
                string today = DateTime.Now.ToString("dd/MM/yyyy");
                string Querystring = Base.DecryptKEY(Encquery, "RIYA" + today);

                string[] keyval = new string[20];
                string[] Query = Querystring.Split('&');
                string strTerminalType = Request.QueryString["TERMINALTYPE"] != null && Request.QueryString["TERMINALTYPE"] != "" ? Request.QueryString["TERMINALTYPE"] : "";

                if (Query.Length > 1)
                {
                    Session.Add("POS_ID", Query[0].Split('=')[1].Trim());
                    Session.Add("POS_TID", Query[1].Split('=')[1].Trim());
                    Session.Add("username", Query[2].Split('=')[1].Trim());
                    Session.Add("agenttype", Query[4].Split('=')[1].Trim());
                    Session.Add("sequenceid", Query[5].Split('=')[1].Trim());
                    Session.Add("branchid", Query[6].Split('=')[1].Trim());
                }
                Session.Add("TERMINALTYPE", strTerminalType.ToUpper().Trim());
                if (Session["SECKEY"] != null && Session["SECKEY"].ToString() != "")
                {
                    Session["SECKEY"] = Request.QueryString["SECKEY"].ToString();
                }
                else
                {
                    Session.Add("SECKEY", Request.QueryString["SECKEY"]);
                }
            }
            if (Session["POS_ID"] == null || Session["POS_ID"].ToString() == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            return View("~/Views/CntrlPanel/_Outstanding.cshtml");
        }

        public ActionResult Customerprofile()
        {
            #region UsageLog
            string PageName = "Customer profile";
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
            return View("~/Views/CntrlPanel/Customerprofile.cshtml");
        }

        public ActionResult Transaction()
        {
            #region UsageLog
            string PageName = "Agent Transaction Report";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e) { }
            #endregion

            if (Request.QueryString["SECKEY"] != null && Request.QueryString["SECKEY"] != "")
            {
                string Encquery = Request.QueryString["SECKEY"];
                string today = DateTime.Now.ToString("dd/MM/yyyy");
                string Querystring = Base.DecryptKEY(Encquery, "RIYA" + today);

                string[] keyval = new string[20];
                string[] Query = Querystring.Split('&');

                if (Querystring.ToString() == "0")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                }

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

            if (Session["agentid"] == null || Session["agentid"].ToString() == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }

            ViewBag.ServerDateTime = Base.LoadServerdatetime();
            string product = "ALL|All~Air|Airline";
            if (Session["bus"].ToString() == "Y")
            {
                product += "~BUS|Bus";
            }
            if (Session["hotel"].ToString() == "Y")
            {
                product += "~HOT|Hotel";
            }
            if (Session["Insure"].ToString() == "Y")
            {
                product += "~INS|Insurance";
            }
            if (Session["CAR"].ToString() == "Y")
            {
                product += "~CAR|Car";
            }
            if (Session["TITOS"].ToString() == "Y")
            {
                product += "~TOS|Titos";
            }
            if (Session["train"].ToString() == "Y")
            {
                product += "~TRN|Train";
            }
            if (Session["THEMEPARK"].ToString() == "Y")
            {
                product += "~TMP|ThemePark";
            }
            if (ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA")
            {
                product += "~BRL|Brill Voice";
            }
            if (Session["Mobrech"].ToString() == "Y")
            {
                product += "~MOB|M-Recharge";
            }
            //product += "~TOP|Topup";
            if (Session["Visa"].ToString() == "Y")
            {
                product += "~VSA|Visa";
            }
            if (Session["CRUISES"] != null && Session["CRUISES"] != "" && Session["CRUISES"].ToString() == "Y")
            {
                product += "~CRU|Cruises";
            }
            ViewBag.hdn_product_list_for_sales = product;
            Session["TKTFLAG"] = Request.QueryString["Flag"] != null && Request.QueryString["Flag"].Trim() != "" ? "QTKT" : "DTKT";
            return View("~/Views/CntrlPanel/ClientTransaction.cshtml");

        }

        public ActionResult Smartscore()
        {
            #region UsageLog
            string PageName = "Smart Score";
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
            ViewBag.currentdate = Base.LoadServerdatetime();
            return View("~/Views/CntrlPanel/Smartscore.cshtml");
        }

        public ActionResult BulkMail()
        {
            #region UsageLog
            string PageName = "Bulk Mail";
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
            return View("~/Views/CntrlPanel/BulkMail.cshtml");
        }

        #region Get  DashBoard Report
        public ActionResult newdashboardreport(string TYPE, string YEAR, string MONTH, string SEG, string AGENTID)
        {
            InplantService.Inplantservice _inplantserice = new InplantService.Inplantservice();
            string strResult = string.Empty;
            string departureDate = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            int response = 1;
            int topvalue = 2;
            int count = 3;
            int Title = 4;
            int Total = 5;
            // string BASED = "book";
            string strErrorMsg = "";
            string DayType = string.Empty;
            string Totsale = "";
            string xml = string.Empty;

            byte[] dsViewPNR_compress = new byte[] { };

            DataSet my_ds = new DataSet();
            DataTable dtstatuscount1 = new DataTable();
            DataTable dtstatuscount = new DataTable();
            Hashtable my_param = new Hashtable();
            try
            {
                #region UsageLog
                string PageName = "Smart Score";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                _inplantserice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                my_ds = _inplantserice.newdashboardreportweb(Session["POS_ID"].ToString(), TYPE, YEAR, MONTH, SEG, ref strErrorMsg);

                if (my_ds != null && my_ds.Tables.Count > 0)
                {
                    if (SEG == "SALEWISE")
                    {
                        if (my_ds.Tables[0].Rows != null && TYPE == "D")
                        {
                            var qryMarge = (from input in my_ds.Tables[0].AsEnumerable()

                                            select new
                                            {
                                                Booked = input["DAY"],
                                                Total = input["SALES"],

                                            });
                            dtstatuscount = Serv.ConvertToDataTable(qryMarge);
                            var qryMarge1 = (from input in my_ds.Tables[0].AsEnumerable()
                                             where (input["SALES"].ToString()).Trim() != ""
                                             orderby input["SALES"] descending
                                             select new
                                             {
                                                 Booked = input["DAY"],
                                                 Total = input["SALES"],

                                             });
                            dtstatuscount1 = Serv.ConvertToDataTable(qryMarge1);

                            DayType = "DAYWISE SALES";
                        }
                        if (my_ds.Tables[0].Rows != null && TYPE == "M")
                        {
                            var qryMarge = (from input in my_ds.Tables[0].AsEnumerable()

                                            select new
                                            {
                                                // Booked = input["Booked"],
                                                Booked = input["MONTH"],
                                                Total = input["SALES"],

                                            });
                            dtstatuscount = Serv.ConvertToDataTable(qryMarge);
                            var qryMarge1 = (from input in my_ds.Tables[0].AsEnumerable()
                                             where (input["SALES"].ToString()).Trim() != ""
                                             orderby input["SALES"] descending
                                             select new
                                             {
                                                 //Booked = input["Booked"],
                                                 Booked = input["MONTH"],
                                                 Total = input["SALES"],

                                             });
                            dtstatuscount1 = Serv.ConvertToDataTable(qryMarge1);

                            DayType = "MONTHWISE SALES";

                        }
                        Totsale = "TOTAL SALES";
                    }
                    else
                    {
                        if (my_ds.Tables[0].Rows != null && TYPE == "D")
                        {
                            var qryMarge = (from input in my_ds.Tables[0].AsEnumerable()

                                            select new
                                            {
                                                //Booked = input["NET AMOUNT"].ToString(),
                                                Booked = input["DAY"],
                                                Total = input["SEGMENT COUNT"],

                                            });
                            dtstatuscount = Serv.ConvertToDataTable(qryMarge);
                            var qryMarge1 = (from input in my_ds.Tables[0].AsEnumerable()
                                             where (input["SEGMENT COUNT"].ToString()).Trim() != ""
                                             orderby input["SEGMENT COUNT"] descending
                                             select new
                                             {
                                                 // Booked = input["NET AMOUNT"].ToString(),
                                                 Booked = input["DAY"],
                                                 Total = input["SEGMENT COUNT"],

                                             });
                            dtstatuscount1 = Serv.ConvertToDataTable(qryMarge1);

                            DayType = "DAYWISE SEGMENT";
                        }
                        if (my_ds.Tables[0].Rows != null && TYPE == "M")
                        {
                            var qryMarge = (from input in my_ds.Tables[0].AsEnumerable()

                                            select new
                                            {
                                                // Booked = input["Booked"],
                                                Booked = input["MONTH"],
                                                Total = input["SEGMENT COUNT"],

                                            });
                            dtstatuscount = Serv.ConvertToDataTable(qryMarge);
                            var qryMarge1 = (from input in my_ds.Tables[0].AsEnumerable()
                                             where (input["SEGMENT COUNT"].ToString()).Trim() != ""
                                             orderby input["SEGMENT COUNT"] descending
                                             select new
                                             {
                                                 //Booked = input["Booked"],
                                                 Booked = input["MONTH"],
                                                 Total = input["SEGMENT COUNT"],

                                             });
                            dtstatuscount1 = Serv.ConvertToDataTable(qryMarge1);

                            DayType = "MONTHWISE SEGMENT";

                        }
                        Totsale = "TOTAL SEGMENT";
                    }
                    if (dtstatuscount1.Rows.Count > 0)
                    {
                        Array_Book[topvalue] = dtstatuscount1.Rows[0]["Total"];
                        Array_Book[count] = my_ds.Tables[0].Rows.Count;
                        Array_Book[response] = JsonConvert.SerializeObject(dtstatuscount); // JsonConvert.SerializeObject(my_ds);
                        Array_Book[Title] = DayType;
                        Array_Book[Total] = Totsale;
                    }
                    else
                    {
                        Array_Book[error] = "No Record(s) Found";
                    }
                }
                else
                {
                    Array_Book[error] = "No Record(s) Found";
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Dashboard", "newdashboardreport", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                Array_Book[error] = "Problem Occured.Please contact customercare (#05).";
            }
            //return Array_Book;
            return Json(new { Status = "", Message = "", Result = Array_Book });
        }
        #endregion

        public ActionResult Requestbuilddashboard(string YEAR, string MONTH, string AGENTID)
        {
            STSTRAVRAYS.Rays_service.RaysService _rays_servers = new STSTRAVRAYS.Rays_service.RaysService();
            string strResult = string.Empty;
            string departureDate = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");

            byte[] dsViewPNR_compress = new byte[] { };
            int error = 0;
            int response = 1;

            string strErrorMsg = "";
            DataSet my_ds = new DataSet();
            Hashtable my_param = new Hashtable();
            string xml = string.Empty;
            try
            {
                #region UsageLog
                string PageName = "Smart Score";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                xml = "<YEAR>" + YEAR + "</YEAR><MONTH>" + MONTH + "</MONTH><AGENTID>" + Session["POS_ID"] + "</AGENTID><DISPLAY_TYPE>M</DISPLAY_TYPE>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "Dashboard", "Requestbuilddashboard", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                //my_ds = _rays_servers.newdashboardweb(Session["agentid"].ToString(), YEAR, MONTH, ref strErrorMsg);
                dsViewPNR_compress = _rays_servers.newdashboard(YEAR, MONTH);

                if (dsViewPNR_compress != null)
                {
                    my_ds = Base.Decompress(dsViewPNR_compress);
                }

                if (my_ds != null && my_ds.Tables.Count > 0 && my_ds.Tables[0].Rows.Count > 0)
                {
                    var qryMarge1 = (from input in my_ds.Tables[0].AsEnumerable()
                                     where (input["NET AMOUNT"].ToString()).Trim() != ""
                                     orderby input["NET AMOUNT"] descending
                                     select new
                                     {
                                         MONTH = input["MONTH"].ToString(),
                                         NETAMOUNT = input["NET AMOUNT"],
                                         COMMISION = input["EARNING AMT"],
                                         SERVICE = input["SERVICE CHARGE"],
                                     }).Take(10);
                    DataTable dtstatuscount1 = Serv.ConvertToDataTable(qryMarge1);
                    Array_Book[response] = JsonConvert.SerializeObject(dtstatuscount1);
                }
                else
                {
                    Array_Book[error] = "No Record(s) Found";
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Dashboard", "Requestbuilddashboard", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                Array_Book[error] = "Problem Occured.Please contact customercare (#05).";
            }
            return Json(new { Status = "", Message = "", Result = Array_Book });
        }

        #region Get  DashBoard
        public ActionResult Requestdaywiseseg(string YEAR, string DATEWISE, string ORIGIN, string DESTINATION, string AGENTID)
        {


            InplantService.Inplantservice _inplantservice = new InplantService.Inplantservice();
            string strResult = string.Empty;
            string departureDate = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");

            int error = 0;
            int response = 1;

            byte[] dsViewPNR_compress = new byte[] { };

            string strErrorMsg = "";
            DataSet my_ds = new DataSet();
            Hashtable my_param = new Hashtable();
            string xml = string.Empty;
            try
            {
                xml = "<ORGIN>" + ORIGIN + "</ORGIN><DESTINATION>" + DESTINATION + "</DESTINATION><AGENTID>" + Session["POS_ID"] + "</AGENTID><DATEWISE>" + DATEWISE + "</DATEWISE>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "Dashboard", "Requestdaywiseseg", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();

                AGENTID = Session["POS_ID"].ToString();
                dsViewPNR_compress = _inplantservice.Requestdaywiseseg(AGENTID, YEAR, DATEWISE, ORIGIN, DESTINATION, strErrorMsg);

                if (dsViewPNR_compress != null)
                {
                    my_ds = Base.Decompress(dsViewPNR_compress);
                }

                if (my_ds != null && my_ds.Tables.Count > 0 && my_ds.Tables[0].Rows.Count > 0)
                {
                    Array_Book[response] = JsonConvert.SerializeObject(my_ds.Tables[0]);
                }
                else
                {
                    Array_Book[error] = "No Record(s) Found";
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Dashboard", "Requestdaywiseseg", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                Array_Book[error] = "Problem Occured.Please contact customercare (#05).";
            }
            //return Array_Book;
            return Json(new { Status = "", Message = "", Result = Array_Book });
        }
        #endregion

        #region change password
        public ActionResult change_password(string oldpasswd, string newpasswd)
        {
            #region UsageLog
            string PageName = "Change Password";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "UPDATE");
            }
            catch (Exception e)
            {

            }
            #endregion
            ArrayList result = new ArrayList(2);
            RaysService _RaysService = new RaysService();
            DataSet ds_res = new DataSet();
            DataSet ds_rr = new DataSet();

            string dsLogin = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;
            string strErrorMsg = string.Empty;

            result.Add("");
            result.Add("");

            int error = 0;
            int responce = 1;
            try
            {
                strAgentID = Session["POS_ID"].ToString();
                strTerminalId = Session["POS_TID"].ToString();
                strUserName = Session["username"].ToString();
                Ipaddress = Session["ipAddress"] != null && Session["ipAddress"] != "" ? Session["ipAddress"].ToString() : "";
                sequnceID = Session["sequenceid"] != null && Session["sequenceid"] != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");

                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                dsLogin = _RaysService.changePassword(strAgentID, strTerminalId, strUserName, oldpasswd, newpasswd, "W", Ipaddress, Convert.ToDecimal(sequnceID), ref strErrorMsg, "ChangePassword", "CHANGE_PWD");

                if (dsLogin == "1")
                {
                    result[responce] = "Your password is sucessfully changed!";
                }
                else
                {
                    result[error] = "Old password wrong!";
                    string log = "<Oldpassword>" + oldpasswd + "</Oldpassword>";
                    log += "<newpassword>" + newpasswd + "</newpassword>";
                    log += "<tr_id>" + strTerminalId + "</tr_id>";
                    DatabaseLog.LogData(Session["username"].ToString(), "E", "Change Password", "CHANGE PWD Click Event", log.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }

            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Change Password", "CHANGE PWD Click Event", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                result[error] = "Problem occured.Please contact customer support";
            }

            return Json(new { Status = "", Message = "", Result = result });
        }
        #endregion

        #region agent transation report //--? why need 3 functions here,retrievebooking,home
        public ActionResult RequestTopupFunction(string fromdate, string todate, string sPNR, string option, string product, string AccountType, string AgentId, string Currencycode, string Terminal_ID)
        {
            STSTRAVRAYS.Rays_service.RaysService _rays_servers = new STSTRAVRAYS.Rays_service.RaysService();

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
                #region UsageLog
                string PageName = "Agent Transaction Report";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                DatabaseLog.LogData(Session["username"].ToString(), "E", "AgentTopUp", "RequestTopupFunction", "<EVENT><FROMDATE>" + fromdate + "</FROMDATE><TODATE>" + todate + "</TODATE><SPNR>" + sPNR + "</SPNR><OPTION>" + option + "</OPTION><PRODUCT>" + product + "</PRODUCT><ACCOUNTTYPE>" + AccountType + "</ACCOUNTTYPE></EVENT>", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
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
                Byte[] lstrBute = null;

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

                //_rays_servers.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();//AccountType == "C" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                if (strBranchCredit != "")
                {
                    if ((strBranchCredit == "ALL" || strBranchCredit.Contains(strBranchID)) && AccountType == "C")
                        _rays_servers.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                    else if ((strBranchCredit == "ALL" || strBranchCredit.Contains(strBranchID)) && AccountType == "T")
                        _rays_servers.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                    else
                        _rays_servers.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                }
                else
                {
                    _rays_servers.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                }
                //dsAgentDetails = _rays_servers.Fetch_Transsaction(sPNR.ToString(), strAgentID.ToString(), fromDate.ToString(), toDate.ToString(),
                //    terminalUser, "", AccountType.ToString(), strAgentID.ToString(), strTerminalId.ToString(), strUserName.ToString(),
                //    Ipaddress.ToString(), terminalType.ToString(), Convert.ToDecimal(sequnceID.ToString()), product.ToString(), option.ToString(),
                //    ref strErrorMsg, "ControlPanel-Topup or Credit Account", "bgwAgentCommission_DoWork", ref lstrref, ref lstrBute);

                dsViewPNR_compress = _rays_servers.Fetch_Transsactionbyte(sPNR.ToString(), strAgentID.ToString(), fromDate.ToString(), toDate.ToString(),
                   terminalUser, "", AccountType.ToString(), strAgentID.ToString(), strTerminalId.ToString(), strUserName.ToString(),
                   Ipaddress.ToString(), terminalType.ToString(), Convert.ToDecimal(sequnceID.ToString()), product.ToString(), option.ToString(),
                   ref strErrorMsg, "ControlPanel-Topup or Credit Account", "bgwAgentCommission_DoWork", lstrref, Compressed, Currencycode, "");

                if (dsViewPNR_compress != null)
                {
                    dsAgentDetails = Base.Decompress(dsViewPNR_compress);
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
                DatabaseLog.LogData(Session["username"].ToString(), "X", "AgentTopUp", "RequestTopupFunction", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                array_topup[Error] = "Problem Occured.Please contact customercare";
            }
            return Json(new { Status = "", Message = "", Result = array_topup });
        }

        public ActionResult AgentTransactionReport(string fromdate, string todate, string sPNR, string strTransType, string product, string strPaymentmode, string AgentId, string Terminal_ID, string strMigratedAgent, string BranchId, string Corptype, string OrderBy)
        {
            STSTRAVRAYS.Rays_service.RaysService _rays_servers = new STSTRAVRAYS.Rays_service.RaysService();

            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalType = string.Empty;
            string strResponse = string.Empty;
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;
            byte[] decompress = new byte[] { };
            ArrayList array_topup = new ArrayList();
            array_topup.Add("");
            array_topup.Add("");
            int Error = 0;
            int responseBookedHistory = 1;
            DataSet dsAgentDetails = new DataSet();
            try
            {
                #region UsageLog
                string PageName = "Agent Transaction Report";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                DatabaseLog.LogData(Session["username"].ToString(), "E", "AgentTopUp", "RequestTopupFunction", "<EVENT><FROMDATE>" + fromdate + "</FROMDATE><TODATE>" + todate + "</TODATE><SPNR>" + sPNR + "</SPNR><strTransType>" + strTransType + "</strTransType><PRODUCT>" + product + "</PRODUCT><strPaymentmode>" + strPaymentmode + "</strPaymentmode></EVENT>", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                strTerminalType = (Session["TerminalType"] != null && Session["TerminalType"].ToString() != "" ? Session["TerminalType"].ToString() : "");
                strTerminalId = strTerminalType == "T" ? "" : Session["POS_TID"].ToString();
                strUserName = Session["username"].ToString();
                strAgentID = strTerminalType == "T" ? AgentId : Session["POS_ID"].ToString();
                Ipaddress = Session["ipAddress"].ToString();
                sequnceID = Session["sequenceid"].ToString();
                string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");
                string fromDate = fromdate;
                string toDate = todate;
                string strCurrency = ConfigurationManager.AppSettings["currency"].ToString();
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

                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                string strErrorMsg = string.Empty;
                if (strTerminalType.ToString().ToUpper().Trim() == "T")
                {
                    dsViewPNR_compress = _rays_servers.ClientFetchtransctiondetails("", fromDate.ToString(), toDate.ToString(), "", BranchId,
                       strPaymentmode, strTransType, strUserName.ToString(), strTerminalId.ToString(), product, Ipaddress.ToString(), Convert.ToDecimal(sequnceID.ToString()),
                       Session["POS_TID"].ToString(), Corptype, OrderBy, strAgentID, strCurrency, "");
                }
                else
                {
                    dsViewPNR_compress = _rays_servers.Fetch_Transsactionbyte(sPNR, strAgentID.ToString(), fromDate.ToString(), toDate.ToString(),
                     strTerminalId, BranchId, strPaymentmode.ToString(), strAgentID.ToString(), strTerminalId.ToString(), strUserName.ToString(),
                     Ipaddress.ToString(), strTerminalType.ToString(), Convert.ToDecimal(sequnceID.ToString()), product.ToString(), strTransType.ToString(),
                     ref strErrorMsg, "ClientTransaction", "ClientTransaction", "", Compressed, strCurrency, "");
                }
                if (dsViewPNR_compress != null)
                {
                    dsAgentDetails = Base.Decompress(dsViewPNR_compress);
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
                DatabaseLog.LogData(Session["username"].ToString(), "X", "AgentTopUp", "RequestTopupFunction", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                array_topup[Error] = "Problem Occured.Please contact customercare";
            }
            return Json(new { Status = "", Message = "", Result = array_topup });
        }
        #endregion

        #region agent transaction export to excel code
        public ActionResult btnExportExcell(string cal_fromdate, string cal_todate, string Heading)
        {
            StringBuilder AgentTopup = new StringBuilder();

            ArrayList exporttoexcel = new ArrayList();

            exporttoexcel.Add("");
            int status = 0;

            try
            {
                #region UsageLog
                string PageName = "Agent Transaction Report";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "EXCEL");
                }
                catch (Exception e) { }
                #endregion
                DataSet dsSaleDetails = (DataSet)Session["TOPUPREPORT"];


                AgentTopup.Append(" <table data-sort='false' style='overflow:scroll;' class='Bookedhistory' data-page-size='15' >");
                AgentTopup.Append("<thead>  <tr>");
                for (int col = 0; col < dsSaleDetails.Tables[0].Columns.Count; col++)
                {
                    if (col == 0)
                    { AgentTopup.Append("<th align='center' style='width:15%;' data-toggle='true' class='viewDetailsTD'>" + dsSaleDetails.Tables[0].Columns[col].ColumnName + "</th>"); }
                    else
                    { AgentTopup.Append("<th align='center' style='width:5%;' data-hide='phone,tablet' >" + dsSaleDetails.Tables[0].Columns[col].ColumnName + "</th>"); }
                }


                AgentTopup.Append("</tr></thead><tbody>");


                for (int i = 0; i < dsSaleDetails.Tables[0].Rows.Count; i++)
                {
                    AgentTopup.Append("<tr style='border-bottom:1px;'>");
                    for (int colnm = 0; colnm < dsSaleDetails.Tables[0].Columns.Count; colnm++)
                    {
                        if (dsSaleDetails.Tables[0].Columns[colnm].DataType.ToString().ToUpper().Trim() == "SYSTEM.DECIMAL")
                            AgentTopup.Append("<td style='text-align:right'>" + dsSaleDetails.Tables[0].Rows[i][colnm] + "</td>");
                        else
                            AgentTopup.Append("<td style='text-align:left'>" + dsSaleDetails.Tables[0].Rows[i][colnm] + "</td>");
                    }
                    AgentTopup.Append("</tr>");
                }
                AgentTopup.Append("</tbody>");
                AgentTopup.Append("</table>");
                string dsTopup = AgentTopup.ToString();//.InnerHtml;
                string fromdate = cal_fromdate;// ViewState["fromdate"].ToString();
                string todate = cal_todate;// ViewState["todate"].ToString();
                string attachment = "attachment; filename= " + Heading.ToString() + " " + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                Response.Write("<HTML><HEAD>");
                Response.Write("<style>TD {font-family:Verdana; font-size: 11px;} </style>");
                Response.Write("</HEAD><BODY>");
                Response.Write("<TABLE border='1' style='width:950px'>");
                Response.Write("<TR><TD style='font-family: Verdana; font-size: 13px; font-weight: bold' align='center'> Airline Sales&nbsp;Report </TD></TR>");
                Response.Write("<TR><TD align='center' style='font-family: Verdana; font-weight: bold'> From Date : " + fromdate.ToString() + " &nbsp;&nbsp;&nbsp;&nbsp; To Date : " + todate.ToString() + "</TD></TR>");
                Response.Write("<TR><TD>");
                Response.Write(dsTopup);
                Response.Write("</TR></TD>");
                Response.Write("</TABLE>");
                Response.Write("</BODY></HTML>");
                Response.Flush();
                Response.End();
                //  HttpContext.Current.Response.End();
                // HttpContext.cu
                exporttoexcel[status] = "Export To Excel Success";
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "AgentTopUp", "RequestTopupFunction", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                exporttoexcel[status] = "Problem Occured.Please contact customercare";

            }
            return View();
        }
        #endregion

        #region agent txt update
        public ActionResult agenttextupdate(string btn, string text)
        {
            #region UsageLog
            string PageName = "Agent Details";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "UPDATE");
            }
            catch (Exception e)
            {

            }
            #endregion

            RaysService Rays_service = new RaysService();
            ArrayList result = new ArrayList();
            result.Add("");
            result.Add("");
            result.Add("");
            int error = 0; ;
            int resupdate = 1;
            int resprev = 2;
            string output = string.Empty;
            byte[] dsViewPNR_compress = new byte[] { };
            string strErrorMsg = string.Empty;
            DataSet preoutput = new DataSet();
            try
            {

                string strTerminalId = Session["POS_TID"].ToString();
                string strUserName = Session["username"].ToString();
                string strAgentID = Session["POS_ID"].ToString();
                string Ipaddress = Session["ipAddress"].ToString();
                string sequnceID = Session["sequenceid"].ToString();

                if (btn == "u")
                {
                    Rays_service.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    output = Rays_service.changeAgentText(Session["POS_ID"].ToString(), text, Session["POS_TID"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(), "W", Convert.ToDecimal(Session["sequenceid"].ToString()), ref strErrorMsg, "AgentText", "AgentTextUpdate");
                    result[resupdate] = "Updated Successfully";
                }
                else if (btn == "p")
                {
                    Rays_service.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    //preoutput = Rays_service.DownloadAgentLogo(Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(), "W", Convert.ToDecimal(Session["sequenceid"].ToString()), ref strErrorMsg, "AgentText", "AgentTextPreview");

                    dsViewPNR_compress = Rays_service.DownloadAgentLogoByte(Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(), "W", Convert.ToDecimal(Session["sequenceid"].ToString()), ref strErrorMsg, "AgentText", "AgentTextPreview");
                    if (dsViewPNR_compress != null)
                    {
                        preoutput = Base.Decompress(dsViewPNR_compress);
                    }

                    if (preoutput.Tables.Count > 0 && preoutput.Tables[0].Rows.Count > 0)
                    {

                        if (preoutput.Tables[0].Rows[0]["AGN_AGENT_TEXT"].ToString() != null && preoutput.Tables[0].Rows[0]["AGN_AGENT_TEXT"].ToString() != "")
                        {

                            result[resprev] = preoutput.Tables[0].Rows[0]["AGN_AGENT_TEXT"].ToString();// JsonConvert.SerializeObject(details);
                        }
                        else
                        {
                            result[error] = "No text found.";
                        }
                    }
                    else
                    {
                        result[error] = "No text found.";
                    }

                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Agent_Text", "Agent_Text_Updatse", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                result[error] = "Error Occured while Update Text.";

            }
            return Json(new { Status = "", Message = "", Result = result });

        }
        #endregion

        #region agent logo update
        [HttpPost]
        public ActionResult UploadFiles()
        {

            #region UsageLog
            string PageName = "Agent Details";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "UPDATE");
            }
            catch (Exception e)
            {

            }
            #endregion
            string output = string.Empty;
            Hashtable my_ref = new Hashtable();
            Hashtable my_param = new Hashtable();
            StringWriter strwrt = new StringWriter();
            string logopathelocaho = string.Empty;
            string StartupPath = "";

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileExt1 = System.IO.Path.GetExtension(file.FileName);
                    string fname;
                    if (fileExt1 == ".png")
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file1 = files[i];

                            string fileExt = System.IO.Path.GetExtension(file1.FileName);
                            var fileName = Session["POS_ID"].ToString() + ".png";

                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file1.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file1.FileName;
                            }

                            fname = Path.Combine(Server.MapPath(@"~/PDF/AgentLogo/"), fileName);
                            StartupPath = fname.ToString();
                            file1.SaveAs(fname);
                            DatabaseLog.LogData(Session["username"].ToString(), "E", "Agent_logo", "Agent_update_check", fname, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                        }
                    }
                    else
                    {
                        return Json(new { Status = "", Path = logopathelocaho, Text = "Please select png image files to upload" });
                    }

                    string AgentID = (Session["POS_ID"] != null && Session["POS_ID"] != "") ? Session["POS_ID"].ToString() : "";
                    string strTerminalId = (Session["POS_TID"] != null && Session["POS_TID"] != "") ? Session["POS_TID"].ToString() : "";
                    string strUserName = (Session["username"] != null && Session["username"] != "") ? Session["username"].ToString() : "";
                    string TerminalType = (Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "") ? Session["TERMINALTYPE"].ToString() : "";
                    string Ipaddress = (Session["ipAddress"] != null && Session["ipAddress"] != "") ? Session["ipAddress"].ToString() : "";
                    string sequnceID = (Session["sequenceid"] != null && Session["sequenceid"] != "") ? Session["sequenceid"].ToString() : "";
                    string filename = AgentID;
                    string strErrorMsg = string.Empty;

                    FileStream fs;
                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                    byte[] picbyte = new byte[fs.Length];
                    picbyte = ReadBitmap2ByteArray(StartupPath);
                    fs.Close();
                    System.IO.File.Delete(StartupPath);

                    string Result = changeAgentLogo_Send(AgentID, picbyte, strTerminalId.ToString(), strUserName.ToString(), Ipaddress,
                       TerminalType.ToString(), Convert.ToDecimal(sequnceID), ref strErrorMsg, "CntrlPanelContriller", "UploadFiles").InnerText.ToString();

                    if (Result == "1")
                    {
                        return Json(new { Status = "", Path = logopathelocaho, Text = "File Uploaded Successfully!" });
                    }
                    else
                    {
                        return Json(new { Status = "", Path = logopathelocaho, Text = "Unable to update image !" });
                    }

                }

                catch (Exception ex)
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "Agent_logo", "Agent_update_check", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    return Json(new { Status = "", Path = logopathelocaho, Text = "Unable to update image" });
                }
            }
            else
            {
                return Json(new { Status = "", Path = logopathelocaho, Text = "No files selected." });
            }
        }
        #endregion

        private XmlDocument changeAgentLogo_Send(string AgentID, byte[] picbyte, string TerminalID, string usrname, string IP,
         string TerminalType, decimal SequenceID, ref string strErrorMsg, string Pagename, string Functionname)
        {
            HttpWebRequest serverRequest = CreateRequestObject();

            byte[] requestBytes = changeAgentLogo(AgentID, picbyte, TerminalID, usrname, IP,
                     TerminalType, SequenceID, ref strErrorMsg, Pagename, Functionname);

            // Send request to the server
            Stream stream = serverRequest.GetRequestStream();
            stream.Write(requestBytes, 0, requestBytes.Length);
            stream.Close();

            // Receive response
            Stream receiveStream = null;
            HttpWebResponse webResponse;

            try
            {
                webResponse = (HttpWebResponse)serverRequest.GetResponse();
                receiveStream = webResponse.GetResponseStream();
            }
            catch (WebException exception)
            {
                //this.SetErrorMessage(exception);

                if (exception.Response != null)
                {
                    // Although the request failed, we can still get a response that might
                    // contain a better error message.
                    receiveStream = exception.Response.GetResponseStream();
                }
                else
                {
                    return null;
                }
            }

            // Read output stream
            StreamReader streamReader = new StreamReader(receiveStream, Encoding.UTF8);
            string result = streamReader.ReadToEnd();

            // Remove SOAP elements
            XmlDocument filteredDocument = GetResponseDocument(result);

            return filteredDocument;
        }

        private byte[] changeAgentLogo(string AgentID, byte[] picbyte, string TerminalID, string usrname, string IP,
                    string TerminalType, decimal SequenceID, ref string strErrorMsg, string Pagename, string Functionname)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            builder.Append("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            builder.Append("<soap:Body>");
            builder.Append("<changeAgentLogo1 xmlns=\"http://tempuri.org/\">");
            builder.Append("<agentID>" + AgentID + "</agentID>");
            builder.Append("<updatedImage>" + Convert.ToBase64String(picbyte) + "</updatedImage>");
            builder.Append("<terminalID>" + TerminalID + "</terminalID>");
            builder.Append("<userName>" + usrname + "</userName>");
            builder.Append("<ipAddress>" + IP + "</ipAddress>");
            builder.Append("<terminalType>" + TerminalType + "</terminalType>");
            builder.Append("<sequenceId>" + SequenceID + "</sequenceId>");
            builder.Append("<strErrorMsg>" + strErrorMsg + "</strErrorMsg>");
            builder.Append("<lstrPageName>" + Pagename + "</lstrPageName>");
            builder.Append("<lstrFunName>" + Functionname + "</lstrFunName>");
            builder.Append("</changeAgentLogo1>");
            builder.Append(" </soap:Body>");
            builder.Append("</soap:Envelope>");

            // Convert the SOAP envelope into a byte array
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] requestBytes = encoding.GetBytes(builder.ToString());

            return requestBytes;
        }

        private HttpWebRequest CreateRequestObject()
        {

            string URL = ConfigurationManager.AppSettings["ServiceURI"].ToString();
            HttpWebRequest serverRequest = (HttpWebRequest)WebRequest.Create(URL);

            serverRequest.Method = "POST";
            serverRequest.ContentType = "text/xml";

            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            serverRequest.AutomaticDecompression = DecompressionMethods.GZip;

            return serverRequest;
        }

        private XmlDocument GetResponseDocument(string result)
        {
            XmlDocument responseXmlDocument = new XmlDocument();
            XmlDocument filteredDocument = null;
            responseXmlDocument.LoadXml(result);

            XmlNode filteredResponse = responseXmlDocument.SelectSingleNode("//*[local-name()='Body']/*");

            filteredDocument = new XmlDocument();
            filteredDocument.LoadXml(filteredResponse.OuterXml);
            return filteredDocument;
        }

        private byte[] ReadBitmap2ByteArray(string fileName)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                using (Bitmap image = new Bitmap(fileName))
                {
                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception)
            {
            }
            return stream.ToArray();
        }

        private XmlDocument DeleteAgentLogo_Send(string AgentID, string TerminalID, string UserName, string IpAddress, string TerminalType,
                   decimal SequenceId, ref string strErrorMsg, string refstrLogo, string Pagename, string FunctionName)
        {
            HttpWebRequest serverRequest = CreateRequestObject();

            byte[] requestBytes = DeleteAgentLogo(AgentID, TerminalID, UserName, IpAddress, TerminalType,
                     SequenceId, ref strErrorMsg, refstrLogo, Pagename, FunctionName);

            // Send request to the server
            Stream stream = serverRequest.GetRequestStream();
            stream.Write(requestBytes, 0, requestBytes.Length);
            stream.Close();

            // Receive response
            Stream receiveStream = null;
            HttpWebResponse webResponse;

            try
            {
                webResponse = (HttpWebResponse)serverRequest.GetResponse();
                receiveStream = webResponse.GetResponseStream();
            }
            catch (WebException exception)
            {
                //this.SetErrorMessage(exception);

                if (exception.Response != null)
                {
                    // Although the request failed, we can still get a response that might
                    // contain a better error message.
                    receiveStream = exception.Response.GetResponseStream();
                }
                else
                {
                    return null;
                }
            }

            // Read output stream
            StreamReader streamReader = new StreamReader(receiveStream, Encoding.UTF8);
            string result = streamReader.ReadToEnd();

            // Remove SOAP elements
            XmlDocument filteredDocument = GetResponseDocument(result);

            return filteredDocument;
        }

        private byte[] DeleteAgentLogo(string AgentID, string TerminalID, string UserName, string IpAddress, string TerminalType,
                 decimal SequenceId, ref string strErrorMsg, string refstrLogo, string Pagename, string FunctionName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            builder.Append("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            builder.Append("<soap:Body>");
            builder.Append("<DeleteAgentLogo xmlns=\"http://tempuri.org/\">");
            builder.Append("<agentID>" + AgentID + "</agentID>");
            builder.Append("<terminalID>" + TerminalID + "</terminalID>");
            builder.Append("<userName>" + UserName + "</userName>");
            builder.Append("<ipAddress>" + IpAddress + "</ipAddress>");
            builder.Append("<terminalType>" + TerminalType + "</terminalType>");
            builder.Append("<sequenceId>" + SequenceId + "</sequenceId>");
            builder.Append("<strErrorMsg>" + strErrorMsg + "</strErrorMsg>");
            builder.Append("<strMsg>" + refstrLogo + "</strMsg>");
            builder.Append("<lstrPageName>" + Pagename + "</lstrPageName>");
            builder.Append("<lstrFunName>" + FunctionName + "</lstrFunName>");
            builder.Append("</DeleteAgentLogo>");
            builder.Append(" </soap:Body>");
            builder.Append("</soap:Envelope>");

            // Convert the SOAP envelope into a byte array
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] requestBytes = encoding.GetBytes(builder.ToString());

            return requestBytes;
        }

        protected string imageview()
        {
            string patt = "";

            try
            {
                string cach = "";
                try
                {
                    cach = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
                }
                catch
                {

                }

                String searchFolder = Server.MapPath(@"~/PDF/AgentLogo/");
                var filters = new String[] { "png" };
                var files = GetFilesFrom(searchFolder, filters, false);
                if (files.Length > 0)
                {
                    string value1 = Array.Find(files, element => element.StartsWith(searchFolder + Session["POS_ID"].ToString() + ".png", StringComparison.Ordinal));
                    string result = Path.GetFileName(value1);
                    string applicationimageload = ConfigurationManager.AppSettings["AgentLogoPath"].ToString() + "/AgentLogo/" + Session["POS_ID"].ToString() + ".png";
                    patt = applicationimageload.ToString();
                    ViewBag.Message = applicationimageload.ToString();
                }
                else
                {
                    string applicationimageload = ConfigurationManager.AppSettings["AgentLogoPath"].ToString() + "/AgentLogo/" + "default.png";
                    patt = applicationimageload.ToString();
                    ViewBag.Message = applicationimageload.ToString();

                }

            }
            catch (Exception)
            {

            }
            return patt;

        }

        public static String[] GetFilesFrom(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
            }
            return filesFound.ToArray();
        }

        [HttpPost]
        public ActionResult btn_Delete_Click(string btn)
        {

            string output = string.Empty;
            Hashtable my_ref = new Hashtable();
            Hashtable my_param = new Hashtable();
            StringWriter strwrt = new StringWriter();
            string defaultlogo = string.Empty;
            string AgentID = (Session["POS_ID"] != null && Session["POS_ID"] != "") ? Session["POS_ID"].ToString() : "";
            string strTerminalId = (Session["POS_TID"] != null && Session["POS_TID"] != "") ? Session["POS_TID"].ToString() : "";
            string strUserName = (Session["username"] != null && Session["username"] != "") ? Session["username"].ToString() : "";
            string TerminalType = (Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "") ? Session["TERMINALTYPE"].ToString() : "";
            string Ipaddress = (Session["ipAddress"] != null && Session["ipAddress"] != "") ? Session["ipAddress"].ToString() : "";
            string sequnceID = (Session["sequenceid"] != null && Session["sequenceid"] != "") ? Session["sequenceid"].ToString() : "";
            string strErrorMsg = string.Empty;
            try
            {
                string refstrLogo = string.Empty;
                string Result = DeleteAgentLogo_Send(AgentID, strTerminalId, strUserName, Ipaddress, TerminalType,
                      Convert.ToDecimal(sequnceID), ref strErrorMsg, refstrLogo, "CntrlPanel", "btn_Delete_Click").InnerText.ToString();
                return Json(new { Status = "", Path = defaultlogo, Text = Result });
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Agent_logo", "Agent_logo_Delete", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                return Json(new { Status = "", Path = defaultlogo, Text = "Error Occured while Delete File." });
            }

        }

        #region customer profile management
        public ActionResult getalldetails(string fstname, string lstname, string cntno, string mail)
        {
            ArrayList result = new ArrayList(2);
            result.Add("");
            result.Add("");
            result.Add("");
            int error = 0;
            int response = 1;
            //int hide=0;
            byte[] dsViewPNR_compress = new byte[] { };
            DataSet ds = new DataSet();
            RaysService _rays_servers = new RaysService();
            string Error = "";
            try
            {
                #region UsageLog
                string PageName = "Customer profile";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                //ds = _rays_servers.Fetch_CustomerProfile_Details(fstname.ToString(), lstname.ToString(), cntno.ToString(), mail.ToString(), Session["agentid"].ToString(), "", "", "", "W", 0, ref Error, "CPM", "GET CPM");

                dsViewPNR_compress = _rays_servers.Fetch_CustomerProfile_Details_Byte(fstname.ToString(), lstname.ToString(), cntno.ToString(), mail.ToString(), "", "", Session["POS_ID"].ToString(), "", "", "", "W", 0, ref Error, "CPM", "GET CPM");

                if (dsViewPNR_compress != null)
                {
                    ds = Base.Decompress(dsViewPNR_compress);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    Session.Add("CPM", ds.Tables[0]);
                    result[response] = JsonConvert.SerializeObject(ds.Tables[0]);
                    var RetVal = (from v in ds.Tables[0].AsEnumerable()
                                  select new
                                  {
                                      Title = v["Pax Title"].ToString(),
                                      Firstname = v["Pax First Name"].ToString() + "~" + v["Row"].ToString(),
                                      Lastname = v["Pax last Name"].ToString(),
                                      MobileNumber = v["MOBILE_NO"].ToString(),
                                      EmailID = v["EMAIL_ID"].ToString(),
                                      row = v["Row"].ToString(),
                                  }).Distinct();
                    result[response] = JsonConvert.SerializeObject(RetVal);

                }

                else { result[error] = "No Records Found"; }

                StringWriter strres = new StringWriter();
                if (ds != null)
                    ds.WriteXml(strres);

                string LstrDetails = "<REQ><cntno>" + cntno + "</cntno>" +
                "<fstname>" + fstname + "</fstname>" + "<lstname>" + lstname + "</lstname>" +
                "<mail>" + mail + "</mail></REQ>" +
                "<RESPONSE>" + ((Base.ReqLog) ? strres.ToString() : "") + "</RESPONSE>";

                DatabaseLog.LogData(Session["username"].ToString(), "T", "CPM", "getalldetails", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());


            }
            catch (Exception e)
            {

                DatabaseLog.LogData(Session["username"].ToString(), "X", "CPM getalldetails", "Get CPM Details", e.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            return Json(new { Status = "", Message = "", Result = result });
        }

        #endregion

        #region customer profile detailsview in popup

        public ActionResult GettingFullDetails(string row)
        {

            ArrayList result = new ArrayList(2);
            result.Add("");
            result.Add("");
            result.Add("");
            int error = 0;
            int response = 1;
            DataSet ds = new DataSet();
            DataTable ddd = new DataTable();
            RaysService _rays_servers = new RaysService();
            //string Error = "";
            try
            {
                if (Session["CPM"] != null && Session["CPM"].ToString() != "")
                {
                    ds = new DataSet();
                    ddd = (DataTable)Session["CPM"];

                    if (ddd.Rows.Count > 0)
                    {
                        result[response] = JsonConvert.SerializeObject(ddd);
                        var RetVal = (from v in ddd.AsEnumerable()
                                      where v["Row"].ToString().Trim() == row
                                      select new
                                      {
                                          MobileNumber = v["MOBILE_NO"].ToString(),
                                          EmailID = v["EMAIL_ID"].ToString(),
                                          AgentNumber = v["AGENT NO"].ToString(),
                                          BusinessNumber = v["BUSINESS NO"].ToString(),
                                          HomeNumber = v["HOME NO"].ToString(),
                                          PaxType = v["Pax Type"].ToString(),
                                          Title = v["Pax Title"].ToString(),
                                          FirstName = v["Pax First Name"].ToString(),
                                          LastName = v["Pax last Name"].ToString(),
                                          Address = v["ADDRESS"].ToString(),
                                          Country = Base.Utilities.AirportcityName(v["COUNTRY_ID"].ToString()),
                                          Emirate = Emirates(v["STATE_ID"].ToString()),
                                          Location = Location(v["CITY_ID"].ToString()),
                                          Remarks = v["REMARKS"].ToString(),
                                          Pincode = v["PINCODE"].ToString(),
                                          PassportNo = v["Passport No"].ToString(),
                                          PassportExpiry = v["pax Expiry Date"].ToString()
                                      }).Distinct();
                        result[response] = JsonConvert.SerializeObject(RetVal);
                    }


                    else
                    {
                        result[error] = "No Records Found";
                    }

                    StringWriter strres = new StringWriter();
                    if (ddd != null)
                        ddd.WriteXml(strres);

                    string LstrDetails = "<REQ><row>" + row + "</row></REQ>" +

                    "<arrayresponse>" + result[response].ToString() + "</arrayresponse>" +
                    "<RESPONSE>" + ((Base.ReqLog) ? strres.ToString() : "") + "</RESPONSE>";

                    DatabaseLog.LogData(Session["username"].ToString(), "T", "CPM", "GettingFullDetails", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                }
            }
            catch (Exception e)
            {

                DatabaseLog.LogData(Session["username"].ToString(), "X", "GettingFullDetails for cpm", "CPM GettingFullDetails", e.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }

            return Json(new { Status = "", Message = "", Result = result });

        }
        #endregion

        #region customer profile name link

        public static Hashtable AirlineCode1 = new Hashtable();
        public static string Emirates(string countrycodeCode)
        {


            try
            {
                if (AirlineCode1.ContainsKey(countrycodeCode))
                {
                    var foo = AirlineCode1[countrycodeCode];
                    return foo.ToString();
                }
                else
                {
                    DataSet dsAirways = new DataSet();
                    dsAirways.ReadXml(HostingEnvironment.MapPath("~/XML/State.xml").ToString());
                    string StateName = "";
                    var qryAirlineName = from p in dsAirways.Tables

                                       ["EMIRATESDET"].AsEnumerable()
                                         where p.Field<string>

                                       ("ID") == countrycodeCode
                                         select p;
                    DataView dvAirlineCode = qryAirlineName.AsDataView();
                    if (dvAirlineCode.Count == 0)
                        StateName = countrycodeCode;
                    else
                    {
                        DataTable dtAilineCode = new DataTable();
                        dtAilineCode = qryAirlineName.CopyToDataTable();
                        StateName = dtAilineCode.Rows[0]
                        ["Name"].ToString();
                        AirlineCode1.Add(countrycodeCode, StateName);
                    }
                    return StateName;
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "Emirates", "Newavailservice");
                return string.Empty;
            }
        }

        public static Hashtable AirlineCode2 = new Hashtable();
        public static string Location(string countrycodeCode)
        {


            try
            {
                if (AirlineCode2.ContainsKey(countrycodeCode))
                {
                    var foo = AirlineCode2[countrycodeCode];
                    return foo.ToString();
                }
                else
                {
                    DataSet dsAirways = new DataSet();
                    dsAirways.ReadXml(HostingEnvironment.MapPath("~/XML/Location.xml").ToString());
                    string LocationName = "";
                    var qryAirlineName = from p in dsAirways.Tables

                                       ["LOCATIONDET"].AsEnumerable()
                                         where p.Field<string>

                                       ("ID") == countrycodeCode
                                         select p;
                    DataView dvAirlineCode = qryAirlineName.AsDataView();
                    if (dvAirlineCode.Count == 0)
                        LocationName = countrycodeCode;
                    else
                    {
                        DataTable dtAilineCode = new DataTable();
                        dtAilineCode = qryAirlineName.CopyToDataTable();
                        LocationName = dtAilineCode.Rows[0]

                        ["Name"].ToString();
                        AirlineCode2.Add(countrycodeCode, LocationName);
                    }
                    return LocationName;
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "location", "Newavailservice");
                return string.Empty;
            }
        }

        #endregion

        #region export to excel by srinath

        public ActionResult ExportData_agenttopup()
        {

            ArrayList encrypt = new ArrayList();
            encrypt.Add("");
            int status = 0;
            DateTime dt = DateTime.Now;
            string toDate = dt.ToString("yyyy-MM-ddTHH:mm");//yyyy-MM-ddTHH:mm:sszzz
            DataSet ds = new DataSet();
            ds = (DataSet)Session["TOPUPREPORT"];
            if (ds.Tables[0].Columns.Contains("No"))
                ds.Tables[0].Columns.Remove("No");
            if (ds.Tables[0].Columns.Contains("rownumber"))
                ds.Tables[0].Columns.Remove("rownumber");
            if (ds.Tables[0].Columns.Contains("Ref No."))
                ds.Tables[0].Columns.Remove("Ref No.");

            string reportname = Session["Reportname"] != null && Session["Reportname"] != "" ? Session["Reportname"].ToString() : "Reports";
            GridView gv = new GridView();
            gv.DataSource = ds.Tables[0];
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + reportname + "_" + toDate + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            // Response.Write(@"<style> td { mso-number-format:\@; } </style>");
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            encrypt[status] = "SUCCESS";
            return RedirectToAction("Temp");
        }

        #endregion

        public ActionResult agentnameandid()
        {
            ArrayList agnt = new ArrayList();
            agnt.Add("");
            agnt.Add("");
            string error = "";
            string agentdet = "";
            string airlinedeta = "";
            try
            {
                DataSet airline_det = new DataSet();
                string airlinexml = Server.MapPath("~/XML/AirlineNames.xml");
                airline_det.ReadXml(airlinexml);

                if (airline_det != null && airline_det.Tables.Count > 0 && airline_det.Tables[0].Rows.Count > 0)
                {
                    var airlinedeta_det = from p in airline_det.Tables[0].AsEnumerable()
                                          select new
                                          {
                                              _CODE = p["_CODE"],
                                              _NAME = p["_NAME"]
                                          };
                    airlinedeta = JsonConvert.SerializeObject(airlinedeta_det);
                }
                else
                {
                    error = "No agents found";
                }
            }
            catch (Exception)
            {
                error = "Unable to load agent details";
            }
            return Json(new { Status = "", Error = error, Result = agentdet, BARRES = airlinedeta });
        }

        #region Bulk Mail

        public ActionResult CompanyName(string Loginsession, string useridflag, string companyides)
        {
            StringBuilder strCostid = new StringBuilder();
            StringBuilder strUserid = new StringBuilder();
            ArrayList arysctid = new ArrayList();
            arysctid.Add("");
            arysctid.Add("");
            arysctid.Add("");
            arysctid.Add("");
            arysctid.Add("");
            int error = 0;
            int costcenterid = 1;
            int Totalresult = 4;
            int costname = 2;
            int userid = 3;
            string strUser_id = string.Empty;
            string strcompany = string.Empty;
            string strCompanyID = string.Empty;

            string IPaddress = string.Empty;
            DataSet Ds_set = new DataSet();
            string Branchname = string.Empty;
            string AgentID = "";
            string strRef = "";
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string sequnceID = (Session["sequenceid"] != null && Session["sequenceid"] != "") ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string xmldata = string.Empty;
            InplantService.Inplantservice _inplantserice = new InplantService.Inplantservice();
            try
            {
                AgentID = Session["agentid"].ToString();
                string password = System.Web.HttpContext.Current.Session["Password"] == null ? "" : System.Web.HttpContext.Current.Session["Password"].ToString();

                _inplantserice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();

                xmldata = string.Empty;
                xmldata = "<EVENT><REQUEST>CompanyName</REQUEST>" +
                          "<USERNAME>" + strUserName + "</USERNAME>" +
                          "<PASSWORD>" + password + "</PASSWORD>" +
                          "<TERMINALID>" + strTerminalId + "</TERMINALID>" +
                          "<IPADDRESS>" + Ipaddress + "</IPADDRESS>" +
                          "</EVENT>";
                DatabaseLog.LogData(strUserName, "E", "CntrlPanel", "CompanyName", xmldata, strAgentID, strTerminalId, sequnceID);

                byte[] byecltdetails = _inplantserice.Fetch_All_Agent_Branch_DetailsByte(Ipaddress, strUserName, password, strTerminalId, ref strRef, strPlatform);
                Ds_set = Base.Decompress(byecltdetails);

                xmldata = string.Empty;
                xmldata = "<EVENT><RESPONSE>CompanyName</RESPONSE>" +
                              "<Ds_set>" + JsonConvert.SerializeObject(Ds_set) + "</Ds_set>" +
                             "</EVENT>";
                DatabaseLog.LogData(strUserName, "E", "CntrlPanel", "CompanyName", xmldata, strAgentID, strTerminalId, sequnceID);

                if (Ds_set != null && Ds_set.Tables.Count > 1)//&& Ds_set.Tables[0].Rows.Count > 0
                {
                    if (Ds_set.Tables["MasterBOABranchName"] != null)
                    {
                        Branchname = JsonConvert.SerializeObject(Ds_set.Tables["MasterBOABranchName"]).ToString();
                    }
                    foreach (DataRow DrRows in Ds_set.Tables["MasterClientDetials"].Rows)
                    {
                        strcompany += DrRows["CLT_CLIENT_ID"] + "~" + DrRows["CLT_CLIENT_NAME"] + "|";
                    }
                    arysctid[costcenterid] = strcompany;

                    if (Ds_set.Tables.Count > 1)
                    {
                        var LinqGrp = (from _gplst in Ds_set.Tables[1].AsEnumerable()
                                       orderby _gplst["CLT_CLIENT_NAME"] ascending
                                       select new
                                       {
                                           CLT_BRANCH_ID = _gplst["CLT_BRANCH_ID"],
                                           BRH_BRANCH_NAME = _gplst["BRH_BRANCH_NAME"],
                                           CLT_CLIENT_ID = _gplst["CLT_CLIENT_ID"],
                                           CLT_CLIENT_NAME = _gplst["CLT_CLIENT_NAME"],
                                           CLT_CLIENT_TERMINAL_COUNT = _gplst["CLT_CLIENT_TERMINAL_COUNT"]
                                       }).ToList();

                        arysctid[Totalresult] = JsonConvert.SerializeObject(LinqGrp).ToString();
                    }
                    arysctid[costname] = strCostid.ToString();
                }
                arysctid[userid] = strUserid.ToString();
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString().ToUpper().Trim().Contains("THE OPERATION HAS TIMED OUT"))
                {
                    arysctid[error] = "THE OPERATION HAS TIMED OUT";
                }
                else if (ex.Message.ToString() == "Unable to connect the remote server")
                {
                    arysctid[error] = "Unable to connect the remote server";
                }
                else
                {
                    arysctid[error] = "Problem Occured While Fetch Company Name.Please contact support team (#05)";
                }
                DatabaseLog.LogData(strUserName, "X", "CntrlPanel", "CompanyName", ex.ToString(), strAgentID, strTerminalId, sequnceID);
                return Json(new { status = "00", Errormsg = arysctid[error], arrfetchdata = arysctid, BranchName = Branchname });
            }

            return Json(new { status = "01", Errormsg = "", arrfetchdata = arysctid, BranchName = Branchname });
        }

        public JsonResult UploadFile()
        {
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string sequnceID = (Session["sequenceid"] != null && Session["sequenceid"] != "") ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string _imgname = string.Empty;
            string Foldername = string.Empty;
            var _comPathss = "";
            var _comPathimg = "";
            string xmldata = string.Empty;
            var _comPath = "";
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var ImgCount = System.Web.HttpContext.Current.Request["ImgCount"];
                    for (int N = 0; int.Parse(ImgCount) > N; N++)
                    {
                        var pic = System.Web.HttpContext.Current.Request.Files["MyImages" + N];
                        var MyData = System.Web.HttpContext.Current.Request["MyData" + N];
                        if (pic != null && pic.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(pic.FileName);
                            var _ext = Path.GetExtension(pic.FileName);
                            Foldername = MyData.Split('|')[0];

                            if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Images/BulkMail")))
                            {
                                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Images/BulkMail"));
                            }
                            DateTime dt = DateTime.Now;
                            string toDate = dt.ToString("yyyyMMddHHmmss");
                            _comPath = System.Web.HttpContext.Current.Server.MapPath("~/Images/BulkMail" + "/") + _imgname + N + toDate + _ext;

                            ViewBag.Msg = _comPath;
                            var path = _comPath;
                            pic.SaveAs(path);
                        }
                        _comPathss += _comPath + "*";
                    }
                    _comPathimg += _comPathss + "|";
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(strUserName, "X", "CntrlPanel", "UploadFile", ex.ToString(), strAgentID, strTerminalId, sequnceID);
                return Json(Convert.ToString("00"), JsonRequestBehavior.AllowGet);
            }
            return Json(Convert.ToString(_comPathimg), JsonRequestBehavior.AllowGet);
        }
        public ActionResult InsertBulkMail(string[] strCompanyId, string strEmpMailId, string strCCMailId,
             string strBmlCcMailId, string strSubject, string strMailContent, string strAttachment, string strBranchId)
        {

            string xmldata = string.Empty;
            string stu = string.Empty;
            string err = string.Empty;
            string msg = string.Empty;
            string strEmpId = string.Empty;
            string strToMailId = string.Empty;
            string strCreatedBy = Session["username"] != null ? Session["username"].ToString() : "";
            string strErrorMsg = string.Empty;
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string sequenceID = (Session["sequenceid"] != null && Session["sequenceid"].ToString() != "") ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string TerminalType = (Session["TERMINALTYPE"] != null) ? Session["TERMINALTYPE"].ToString() : "";
            InplantService.Inplantservice _inplantserice = new InplantService.Inplantservice();
            string strError = string.Empty;
            try
            {
                #region UsageLog
                string PageName = "Bulk Mail";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "INSERT");
                }
                catch (Exception e) { }
                #endregion
                string[] straryEmplyess = strEmpMailId.Split(',');
                string dsTemp = "";
                _inplantserice.Url = ConfigurationManager.AppSettings["in_rays_service"].ToString();
                for (int empcnt = 0; empcnt < straryEmplyess.Length; empcnt++)
                {
                    if (straryEmplyess[empcnt] != "")
                    {
                        strEmpMailId = straryEmplyess[empcnt];
                        string strCmpyID = straryEmplyess[empcnt].Split('~').Length > 2 ? straryEmplyess[empcnt].Split('~')[2] : strCompanyId[0];

                        xmldata = "<EVENT><REQUEST>InsertBulkMail</REQUEST>" +
                                  "<COMPANY_ID>" + strCompanyId + "</COMPANY_ID>" +
                                  "<EMP_ID>" + strEmpId + "</EMP_ID>" +
                                  "<EMP_MAIL_ID>" + strEmpMailId + "</EMP_MAIL_ID>" +
                                  "<TO_MAIL_ID>" + strToMailId + "</TO_MAIL_ID>" +
                                  "<CC_MAIL_ID>" + strCCMailId + "</CC_MAIL_ID>" +
                                  "<BCC_MAIL_ID>" + strBmlCcMailId + "</BCC_MAIL_ID>" +
                                  "<SUBJECT>" + strSubject + "</SUBJECT>" +
                                  "<MAIL_CONTENT>" + strMailContent + "</MAIL_CONTENT>" +
                                  "<CREATED_BY>" + strCreatedBy + "</CREATED_BY>" +
                                  "<ATTACHEMENT>" + strAttachment + "</ATTACHEMENT>" +
                                  "<GROUPID>" + strBranchId + "</GROUPID>" +
                                  "</EVENT>";

                        DatabaseLog.LogData(strCreatedBy, "E", "CntrlPanel", "InsertBulkMail", xmldata, strAgentID, strTerminalId, sequenceID);

                        string GroupID = "";
                        dsTemp = _inplantserice.FetchBulkMail(strCmpyID, strEmpId, strEmpMailId, strCCMailId,
                            strBmlCcMailId, strSubject, strMailContent, strCreatedBy, strAttachment, GroupID, strAgentID,
                            strTerminalId, sequenceID, Ipaddress, TerminalType, strBranchId, ref strError);

                        xmldata = "<EVENT><RESPONSE>InsertBulkMail</RESPONSE>" + JsonConvert.SerializeObject(dsTemp) + "</EVENT>";
                        DatabaseLog.LogData(strCreatedBy, "E", "CntrlPanel", "InsertBulkMail", xmldata, strAgentID, strTerminalId, sequenceID);

                        if (strError == "SUCCESS" && Convert.ToInt32(dsTemp) > 0)
                        {
                            stu = "01";
                        }
                        else
                        {
                            err = "Unable to perform your request , please try after some times later";
                        }
                    }
                }

                if (stu == "01")
                {
                    err = "Your mail request successfully processing will receieve email shortly";
                }
                else
                {
                    err = "Unable to perform your request , please try after some times later";
                }
            }
            catch (Exception ex)
            {
                stu = "0";
                string errormgs = string.Empty;
                if (ex.Message.ToString() == "The operation has timed out")
                    errormgs = ex.Message.ToString();
                else if (ex.Message.ToString() == "Unable to connect the remote server")
                    errormgs = "unable to connect the remote server";
                else
                    errormgs = ex.ToString();
                xmldata += "<Exception>" + ex.ToString() + "</Exception>";
                DatabaseLog.LogData(strCreatedBy, "X", "CntrlPanel", "InsertBulkMail", xmldata, strAgentID, strTerminalId, sequenceID);

            }
            return Json(new { status = stu, Errormsg = err, message = msg });
        }
        public ActionResult Fetchagentmailid(string Agents, string Branch)
        {
            string xmldata = string.Empty;
            string stu = string.Empty;
            string err = string.Empty;
            string msg = string.Empty;
            DataSet ds_set = new DataSet();
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strErrorMsg = string.Empty;
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string sequenceID = (Session["sequenceid"] != null && Session["sequenceid"] != "") ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string TerminalType = (Session["TERMINALTYPE"] != null) ? Session["TERMINALTYPE"].ToString() : "";
            InplantService.Inplantservice _inplantserice = new InplantService.Inplantservice();
            bool insertmail = false;
            try
            {
                _inplantserice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                xmldata = string.Empty;
                xmldata = "<EVENT><REQUEST>Bulkmail-Request</REQUEST>" +
                          "<AGENTID>" + strAgentID + "</AGENTID>" +
                          "<TERMINALID>" + strTerminalId + "</TERMINALID>" +
                          "<SEQUENCEID>" + sequenceID + "</SEQUENCEID>" +
                          "<IPADDRESS>" + Ipaddress + "</IPADDRESS>" +
                          "<TERMINALTYPE>" + TerminalType + "</TERMINALTYPE>" +
                          "<AGENTS>" + Agents + "</AGENTS>" +
                          "<BRANCH>" + Branch + "</BRANCH>" +
                          "</EVENT>";
                DatabaseLog.LogData(strUserName, "E", "CntrlPanel", "InsertMail", xmldata, strAgentID, strTerminalId, sequenceID);

                if (Agents != "" && Agents.ToUpper() != "ALL")
                {
                    insertmail = _inplantserice.InsertMail(strAgentID, strTerminalId, sequenceID, Ipaddress, TerminalType, Agents, ref ds_set);
                    if (ds_set != null && ds_set.Tables.Count > 0 && ds_set.Tables[0].Rows.Count > 0 && insertmail == true)
                    {
                        xmldata = "<EVENT><RESPONSE>InsertMail</RESPONSE><RESULT>SUCCESS</RESULT></EVENT>";
                        stu = "01";
                        msg = JsonConvert.SerializeObject(ds_set);
                    }
                    else
                    {
                        xmldata = "<EVENT><RESPONSE>InsertMail</RESPONSE><RESULT>FAILED</RESULT></EVENT>";
                        stu = "00";
                        msg = "Not Inserted";
                    }
                }
                else
                {
                    stu = "01";
                    msg = "ALL";
                }
                xmldata = string.Empty;
                xmldata = "<EVENT><REQUEST>Bulkmail-Response</REQUEST>" +
                            "<AGENTID>" + Agents + "</AGENTID>" +
                             "<BRANCH>" + Branch + "</BRANCH>" +
                            "<RESPONSEDATA>" + ds_set.GetXml() + "</RESPONSEDATA>" +
                            "<RESULT>" + insertmail + "</RESULT>" +
                          "</EVENT>";
                DatabaseLog.LogData(strUserName, "E", "CntrlPanel", "InsertMail", xmldata, strAgentID, strTerminalId, sequenceID);
            }
            catch (Exception ex)
            {
                stu = "0";
                msg = ex.ToString();
                DatabaseLog.LogData(strUserName, "X", "CntrlPanel", "InsertMail", ex.ToString(), strAgentID, strTerminalId, sequenceID);
            }
            return Json(new { status = stu, Errormsg = err, message = msg });
        }
        #endregion

        public ActionResult AgentBalance()
        {
            if (Session["agentid"] == null || Session["agentid"].ToString() == "" || Session["TERMINALTYPE"] == null || Session["TERMINALTYPE"].ToString() != "T")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            #region UsageLog
            string PageName = "Update Agentbalance";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e) { }
            #endregion
            string todaydate = Base.LoadServerdatetime();
            ViewBag.todaydate = todaydate;
            ViewBag.username = Session["username"].ToString();
            return View();//"~/Views/CntrlPanel/gentbalance.cshtml"
        }

        public ActionResult GetClientBalance(string strAgentId)
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

                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                string ReqTime = DateTime.Now.ToString();
                string REQXML = "<EVENT><REQUEST>GETCLIENTBALANCE</REQUEST><REQTIME>" + ReqTime + "</REQTIME><AGENTID>" + strAgentid + "</AGENTID></EVENT>";
                DatabaseLog.LogData(strUserName, "E", "CntrlPanelController", "GetClientBalance", REQXML.ToString(), strPosID, strPosTID, strSequenceId);

                string strCurrencyCode = ConfigurationManager.AppSettings["currency"].ToString();
                dsclien_compress = _rays_servers.Fetch_Client_Credit_Balance_New(strAgentid, "", strTerminalID, strUserName, strIPaddress, strSequenceId, "", strCurrencyCode);
                if (dsclien_compress != null)
                {
                    dsData = Base.Decompress(dsclien_compress);
                }
                string ResTime = DateTime.Now.ToString();
                string RESXML = "<EVENT><RESPONSE>GETCLIENTBALANCE</RESPONSE><RESTIME>" + ResTime + "</RESTIME><RES>" + dsData.GetXml() + "</RES></EVENT>";
                DatabaseLog.LogData(strUserName, "E", "CntrlPanelController", "GetClientBalance", RESXML.ToString(), strPosID, strPosTID, strSequenceId);

                if (dsData != null && dsData.Tables.Count != 0 && dsData.Tables[0].Rows.Count != 0)
                {
                    strData = JsonConvert.SerializeObject(dsData).ToString();
                    Session["CltBranchID"] = dsData.Tables[0].Rows[0]["CLT_BRANCH_ID"].ToString().Trim();
                    Session["CltBalFlag"] = dsData.Tables[0].Rows[0]["CLT_BALANCE_CHECK"].ToString().Trim();
                    Session["CltClientID"] = dsData.Tables[0].Rows[0]["CLT_CLIENT_ID"].ToString().Trim();
                    strStatus = "01";
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
                strMsg = "Problem occured while fetch Agent balance. Please contact support team.";
                DatabaseLog.LogData(strUserName, "X", "CntrlPanelController", "GetClientBalance", ex.ToString(), strPosID, strPosTID, strSequenceId);
            }
            return Json(new { Status = strStatus, Message = strMsg, Result = strData });
        }

        public ActionResult ClientBalanceCheckforTopup(string strAgentID, string strAmountType, string strAmount, string strsign)//--? not used anywhere in application
        {
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            int response = 1;
            string Output = string.Empty;

            string strResult = "";
            string strStatus = "";
            string strError = "";
            bool Response = false;
            string strSequenceId = DateTime.Now.ToString("yyyyMMddhhmmss");
            string strTerminalID = Session["LgnTerminalID"] != null && Session["LgnTerminalID"] != "" ? Session["LgnTerminalID"].ToString() : "";
            string strUsername = Session["UserName"] != null && Session["UserName"] != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : strSequenceId;
            string strLgAgentID = Session["LgnAgentID"] != null && Session["LgnAgentID"].ToString() != "" ? Session["LgnAgentID"].ToString() : "";
            string strErrormsg = "";
            try
            {
                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();

                Response = _rays_servers.ClientBalanceCheckforTopup(strAgentID, strAmount, strsign, strAmountType, strTerminalID, strUsername,
                    IPAddress, strSequenceId, ref strErrormsg);

                if (Response == true)
                {
                    strStatus = "01";
                    strResult = strErrormsg;
                }
                else
                {
                    strStatus = "00";
                    strResult = "";

                }
            }
            catch (Exception ex)
            {
                strStatus = "00";
                strError = "";
                strResult = "";
                DatabaseLog.LogData(strUsername, "X", "BTR-HomeController", "ClientBalanceCheckforTopup", ex.ToString(), strLgAgentID, strTerminalID, strSequenceId);
            }
            return Json(new { Status = strStatus, Error = strError, Results = strResult });
        }

        public ActionResult GetClientCredit(string strBranchname, string strBranchId, string strAgencyname, string strAgentID, string strAcctype, string strPaymode,
            string strReqby, string strAmount, string strRemark, string strCreditType, string strChequeNO, string strCheqeDate, string strBankName, string strBankBranch,
            string strEnteredBy, string StrIcustID, string StrGroupID)//, string strBalanceCheck
        {
            string strResult = "";
            string strStatus = "";
            string strError = "";
            string strAUTHORIZEDBY = strReqby;
            string strSequenceId = DateTime.Now.ToString("yyyyMMddhhmmss");
            string strBranchID = Session["CltBranchID"] != null && Session["CltBranchID"].ToString() != "" ? Session["CltBranchID"].ToString() : strBranchId;
            string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strCltBalFlag = Session["CltBalFlag"] != null && Session["CltBalFlag"].ToString() != "" ? Session["CltBalFlag"].ToString() : "C";
            strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : strSequenceId;
            string strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            try
            {
                if (Session["agentid"] == null || Session["agentid"].ToString() == "")
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }

                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                string strCurrencyCode = ConfigurationManager.AppSettings["currency"].ToString();
                DataSet dsCappingDetails = new DataSet();
                dsCappingDetails.Tables.Add();
                dsCappingDetails.Tables[0].Columns.Add("CLIENTID");
                dsCappingDetails.Tables[0].Columns.Add("CAPPING_AMT");
                dsCappingDetails.Tables[0].Columns.Add("LimitMode");
                dsCappingDetails.Tables[0].Columns.Add("USERNAME");
                dsCappingDetails.Tables[0].Columns.Add("IP");
                dsCappingDetails.Tables[0].Columns.Add("SEQUENCENO");
                dsCappingDetails.Tables[0].Columns.Add("REMARKS");
                dsCappingDetails.Tables[0].Columns.Add("REQUESTEDBY");
                dsCappingDetails.Tables[0].Columns.Add("ICUSTID");
                dsCappingDetails.Tables[0].Columns.Add("CURRENCY");
                dsCappingDetails.Tables[0].Columns.Add("PAYMENTMODE");
                dsCappingDetails.Tables[0].Columns.Add("ChequeNO");
                dsCappingDetails.Tables[0].Columns.Add("CheqeDate");
                dsCappingDetails.Tables[0].Columns.Add("BankName");
                dsCappingDetails.Tables[0].Columns.Add("BranchName");
                dsCappingDetails.Tables[0].Columns.Add("ReceivedBy");
                dsCappingDetails.Tables[0].Columns.Add("AgencyName");
                dsCappingDetails.Tables[0].Columns.Add("BranchID");
                dsCappingDetails.Tables[0].Columns.Add("CLT_BALANCE_CHECK");
                dsCappingDetails.Tables[0].Columns.Add("CSB_GROUP");
                dsCappingDetails.Tables[0].Columns.Add("POSID");

                dsCappingDetails.Tables[0].Rows.Add(strAgentID, strAmount, strCreditType, strUserName, IPAddress, strSequenceId, strRemark, strAUTHORIZEDBY.Trim(), StrIcustID,
                    strCurrencyCode, strPaymode, strChequeNO, strCheqeDate, strBankName, strBankBranch, strEnteredBy, strBranchname, strBranchId, strCltBalFlag, StrGroupID, strBranchId);//strBalanceCheck

                string ReqTime = DateTime.Now.ToString();
                string REQXML = "<EVENT><REQUEST>GETCLIENTCREDIT</REQUEST><REQTIME>" + ReqTime + "</REQTIME>" + dsCappingDetails.GetXml() + "</EVENT>";
                DatabaseLog.LogData(strUserName, "E", "CntrlPanelController", "GetClientCredit", REQXML.ToString(), strPosID, strPosTID, strSequenceId);

                strResult = _rays_servers.UpdateClientCreditbalance("", strAgentID, strAmount, strCreditType, strRemark, strAUTHORIZEDBY, StrIcustID, strPosTID,
                        strUserName, IPAddress, strSequenceId, strCurrencyCode, dsCappingDetails);
                strStatus = "01";
                string ResTime = DateTime.Now.ToString();
                string RESXML = "<EVENT><RESPONSE>GETCLIENTCREDIT</RESPONSE><RESTIME>" + ResTime + "</RESTIME><RES>" + (strResult != null ? strResult.ToString() : "FAILED") + "</RES></EVENT>";
                DatabaseLog.LogData(strUserName, "E", "CntrlPanelController", "GetClientCredit", RESXML.ToString(), strPosID, strPosTID, strSequenceId);
            }
            catch (Exception ex)
            {
                strStatus = "00";
                strError = "Exception Caught";
                strResult = "Problem occured while Top-Up";
                DatabaseLog.LogData(strUserName, "X", "CntrlPanelController", "GetClientCredit", ex.ToString(), strPosID, strPosTID, strSequenceId);
            }
            return Json(new { Status = strStatus, Error = strError, Results = strResult });
        }

        #region Airline PNR Wise Sales
        public ActionResult AirlinePNRWiseSales()
        {
            #region UsageLog
            string PageName = "Airline PNR Wise Sales";
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
            ViewBag.ServerDateTime = Base.LoadServerdatetime();
            return View("~/Views/CntrlPanel/AirlinePNRWiseSales.cshtml");
        }

        #region For Supplier Dropdown
        public ActionResult FetchSupplierDropdown(string strAgntId)
        {
            ArrayList ary = new ArrayList();
            ary.Add("");
            ary.Add("");
            int resultCode = 0;
            int Result = 1;
            int Errmsg = 2;
            string strStatus = string.Empty;
            string strMsg = string.Empty;
            string xml = string.Empty;
            string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");
            string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            byte[] dsSupplier = new byte[] { };
            DataSet dsset = new DataSet();
            string strTermId = "";
            try
            {
                if (strAgentId == null || strAgentId == "")
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }
                string ReqTime = DateTime.Now.ToString();
                xml = "<EVENT><REQUEST>FETCHSUPPLIERDROPDOWN</REQUEST><REQTIME>" + ReqTime + "</REQTIME>" +
                    "<AGENTID>" + strAgentId + "</AGENTID>" +
                    "<USERNAME>" + strUserName + "</USERNAME>" +
                    "<IPADDRESS>" + IPAddress + "</IPADDRESS>" +
                    "<SEQUENCEID>" + strSequenceId + "</SEQUENCEID>" +
                    "<TERMINALID>" + strTerminalID + "</TERMINALID></EVENT>";

                DatabaseLog.LogData(strUserName, "E", "CntrlPanelController", "FetchSupplierDropdown", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                dsSupplier = _rays_servers.FETCH_BOA_OWN_SUPP_DETAILS(strAgntId, strUserName, IPAddress, strSequenceId, strTermId);
                if (dsSupplier != null)
                {
                    dsset = Base.Decompress(dsSupplier);
                    if (dsset != null && dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
                    {
                        xml = "<EVENT><RESPONSE>FETCHSUPPLIERDROPDOWN</RESPONSE>SUCCESS<REQTIME>" + ReqTime + "</REQTIME><RESULT>" +
                            JsonConvert.SerializeObject(dsset).ToString() + "</RESULT></EVENT>";
                        if (dsset != null && dsset.Tables.Count > 0 && dsset.Tables[2].Rows.Count > 0)
                        {
                            DataTable dtsupplierName = dsset.Tables[2].Copy();
                            var LinqQry = (from p in dtsupplierName.AsEnumerable()
                                           where p["CCR_PRODUCT_CODE"].ToString() == "AIR"
                                           select new
                                           {
                                               NAME = p["CCR_SUPPLIER_NAME"].ToString(),
                                               ID = p["CCR_SUPPLIER_ID"].ToString()
                                           }).Distinct();
                            if (LinqQry.Count() > 0)
                            {
                                string strconv = JsonConvert.SerializeObject(LinqQry).ToString();
                                strStatus = "01";
                                ary[Result] = strconv;
                            }
                        }
                    }
                    else
                    {
                        strStatus = "00";
                        strMsg = "No Records Found";
                        ary[Errmsg] = strMsg;
                        xml = "<EVENT><RESPONSE>FETCHSUPPLIERDROPDOWN</RESPONSE>FAILED<REQTIME>" + ReqTime + "</REQTIME><RESULT>" +
                            "No Records Found" + " </RESULT></EVENT>";
                    }
                }
                else
                {
                    strStatus = "00";
                    strMsg = "No Records Found";
                    ary[Errmsg] = strMsg;
                    xml = "<EVENT><RESPONSE>FETCHSUPPLIERDROPDOWN</RESPONSE>FAILED<REQTIME>" + ReqTime + "</REQTIME><RESULT>" +
                        "No Records Found" + " </RESULT></EVENT>";
                }
                DatabaseLog.LogData(strUserName, "E", "CntrlPanelController", "FetchSupplierDropdown", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            catch (Exception ex)
            {
                ary[resultCode] = "00";
                xml = "<EXCEPTION>FETCHSUPPLIERDROPDOWN</EXCEPTION>" + ex.ToString();
                DatabaseLog.LogData(strUserName, "X", "CntrlPanelController", "FetchSupplierDropdown", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            return Json(new { Error = strMsg, Status = strStatus, results = ary });
        }
        #endregion
        public ActionResult FetchAirlinePNRSalesData(string strFrmDate, string strTodate, string strBranch, string strAgentType, string strAcntType, string strAirlineName, string strSupplier,
            string strAirCategory, string strAirportId, string strclassType, string strBookingType, string strStatus, string strUserName, string strRiyapnr, string strAgencyID, string strAgencyName,
            string strPrdctType)
        {
            ArrayList arry = new ArrayList();
            arry.Add("");
            arry.Add("");
            arry.Add("");
            int Error = 0;
            int Result = 1;
            int ResultCode = 2;
            string status = "";
            string strErrMsg = "";
            string xml = string.Empty;
            string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");
            string UserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            byte[] dsSupplier = new byte[] { };
            DataSet dsset = new DataSet();
            try
            {
                if (strAgentId == null || strAgentId == "")
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }
                if (strFrmDate != "" && strTodate != "" && strFrmDate != null && strTodate != null)
                {
                    strFrmDate = DateTime.ParseExact(strFrmDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    strTodate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                }
                #region UsageLog
                string PageName = "Airline PNR Wise Sales";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                string ReqTime = DateTime.Now.ToString();
                xml = "<EVENT><REQUEST>FetchAirlinePNRSalesData</REQUEST><REQTIME>" + ReqTime + "</REQTIME>" +
                    "<AGENCYNAME>" + strAgencyName + "</AGENCYNAME>" +
                    "<FROMDATE>" + strFrmDate + "</FROMDATE>" +
                    "<TODATE>" + strTodate + "</TODATE>" +
                    "<PAYMENTMODE>" + strAcntType + "</PAYMENTMODE>" +
                    "<AIRLINECATEGORY>" + strAirCategory + "</AIRLINECATEGORY>" +
                    "<AIRLINENAME>" + strAirlineName + "</AIRLINENAME>" +
                    "<USERNAME>" + strUserName + "</USERNAME>" +
                    "<IPADDRESS>" + IPAddress + "</IPADDRESS>" +
                    "<SEQUENCEID>" + strSequenceId + "</SEQUENCEID>" +
                    "<PRODUCTTYPE>" + strPrdctType + "</PRODUCTTYPE>" +
                    "<CLASSTYPE>" + strclassType + "</CLASSTYPE>" + "<PAGENAME>CntrlPanelController</PAGENAME>" + "<FUNCTION>FetchAirlinePNRSalesData</FUNCTION>" +
                    "<STATUS>" + strStatus + "</STATUS>" + "<AIRPORTID>" + strAirportId + "</AIRPORTID>" + "<TERMINALID>" + strTerminalID + "</TERMINALID>" +
                    "<SUPPLIER>" + strSupplier + "</SUPPLIER>" + "<AGENCYID>" + strAgencyID + "</AGENCYID>" + "<BOOKINGTYPE>" + strBookingType + "</BOOKINGTYPE>" +
                    "<SPNR>" + strRiyapnr + "</SPNR>" + "<AGENTTYPE>" + strAgentType + "</AGENTTYPE><FLAG>PNRWISE</FLAG></EVENT>";

                DatabaseLog.LogData(strUserName, "E", "CntrlPanelController", "FetchAirlinePNRSalesData", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = strAcntType != "C" ? ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                //_rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                dsSupplier = _rays_servers.FETCH_PNRWISE_CONSOLIDATE_SALES(strAgencyName, "", strFrmDate, strTodate, strAcntType, strAirCategory, strAirlineName, strUserName,
                   IPAddress, strSequenceId, strPrdctType, strclassType, "", "", "CntrlPanelController", "", "", "", "FetchAirlinePNRSalesData", "", strStatus, strAirportId, strTerminalID,
                   strSupplier, strAgencyID, strBookingType, "", strRiyapnr, strAgentType, "", "PNRWISE", "", "");
                if (dsSupplier != null)
                {
                    dsset = Base.Decompress(dsSupplier);
                    if (dsset != null && dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
                    {
                        if (dsset.Tables[0].Columns.Contains("ORD_BY"))
                        {
                            dsset.Tables[0].Columns.Remove("ORD_BY");
                        }
                        xml = "<EVENT><RESPONSE>FetchAirlinePNRSalesData</RESPONSE>SUCCESS<REQTIME>" + ReqTime + "</REQTIME><RESULT>SUCCESS</RESULT></EVENT>";
                        if (dsset != null && dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
                        {
                            string strconv = JsonConvert.SerializeObject(dsset.Tables[0]).ToString();
                            status = "01";
                            arry[Result] = strconv;
                            Session.Add("AIRLINEPNRWISESALES", dsset.Tables[0]);
                        }
                    }
                    else
                    {
                        status = "00";
                        strErrMsg = "No Records Found";
                        arry[Error] = strErrMsg;
                        xml = "<EVENT><RESPONSE>FetchAirlinePNRSalesData</RESPONSE>FAILED<REQTIME>" + ReqTime + "</REQTIME><RESULT>" +
                            "No Records Found" + " </RESULT></EVENT>";
                    }
                }
                DatabaseLog.LogData(strUserName, "E", "CntrlPanelController", "FetchAirlinePNRSalesData", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            catch (Exception ex)
            {
                status = "00";
                arry[ResultCode] = "00";
                xml = "<EXCEPTION>FetchAirlinePNRSalesData</EXCEPTION>" + ex.ToString();
                DatabaseLog.LogData(strUserName, "X", "CntrlPanelController", "FetchAirlinePNRSalesData", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            var temp = Json(new { Status = status, Error = strErrMsg, Results = arry });
            temp.MaxJsonLength = int.MaxValue;
            return temp;
        }

        public ActionResult AirlinePNRwisesales_Export()
        {
            string logxmlData = string.Empty;
            string strAgentId = (Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "";
            string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string strIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strPagename = "CntrlPanelController";
            string strFunctionName = "Export";
            try
            {
                if (strAgentId == "" || strUserName == "" || strIPAddress == "" || strSequenceId == "" || strTerminalID == "")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                }
                DateTime datetime = DateTime.Now;
                string toDate = datetime.ToString("yyyy-MM-ddTHH:mm");
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                dt = (DataTable)Session["AIRLINEPNRWISESALES"];
                ds.Tables.Add(dt.Copy());
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=AirlinePNRWiseSales_" + toDate + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Write("<HTML><HEAD>");
                Response.Write("<style>TD {font-family:Verdana; font-size: 11px;} </style>");
                Response.Write("</HEAD><BODY>");
                Response.Write("<TABLE border='1' style='width:950px'>");

                Response.Write("<TR>");
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    Response.Write("<TD style='font-weight:bold;background-color:#245b9d; color:#fff;font-size: 12px;'>" +
                    (ds.Tables[0].Columns[i].ToString().Contains('_') ? ds.Tables[0].Columns[i].ToString().Replace('_', ' ') : ds.Tables[0].Columns[i].ToString()) + "</TD>");
                }
                Response.Write("</TR>");
                foreach (DataRow datarow in ds.Tables[0].Rows)
                {
                    Response.Write("<TR style='height:25px'>");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (true)
                        {
                            Response.Write(@"<TD>");// style='mso-number-format:General;'
                            Response.Write(datarow[j].ToString().Replace("\n", "?").Replace("\r", "?").Replace("\t", "?").Replace("\r\n", "?").Replace(" ", "?").Replace("?", " "));
                            Response.Write("</TD>");
                        }
                    }
                    Response.Write("</TR>");
                }
                Response.Write("</TABLE>");
                Response.Write("</BODY></HTML>");
                Response.Flush();
                Response.End();
            }
            catch (Exception Ex)
            {
                logxmlData = "<EVENT><REQUEST>Export</REQUEST>"
                      + "<EXCEPTION>" + Ex.Message.ToString() + "</EXCEPTION>"
                      + "</EVENT>";
                DatabaseLog.LogData(strUserName, "X", strPagename, strFunctionName + "~ERR", logxmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            return View();
        }
        #endregion

        #region AgentDetailsReport
        public ActionResult AgentDetailsReport()
        {
            #region UsageLog
            string PageName = "Agent Details";
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
            ViewBag.ServerDateTime = Base.LoadServerdatetime();
            return View("~/Views/CntrlPanel/AgentDetailsReport.cshtml");
        }
        public ActionResult FetchAgentDetailsReport(string strfromdate, string strtodate, string strBranch, string strState, string strAgntType, string strAvailStatus, string strStatus,
          string strAgencyname, string strAgencyId, string strSalesInchrge)
        {
            ArrayList arry = new ArrayList();
            arry.Add("");
            arry.Add("");
            arry.Add("");
            int Error = 0;
            int Result = 1;
            int ResultCode = 2;
            string status = "";
            string strErrmsg = "";
            string xml = string.Empty;
            string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");
            string UserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";

            DataSet dsset = new DataSet();
            try
            {
                if (strAgentId == null || strAgentId == "")
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }
                if (strfromdate != "" && strtodate != "" && strfromdate != null && strtodate != null)
                {
                    strfromdate = DateTime.ParseExact(strfromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    strtodate = DateTime.ParseExact(strtodate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                }
                #region UsageLog
                string PageName = "Agent Details";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                string ReqTime = DateTime.Now.ToString();
                xml = "<EVENT><REQUEST>FetchAgentDetailsReport</REQUEST><REQTIME>" + ReqTime + "</REQTIME>" +
                    "<BRANCH>" + strBranch + "</BRANCH>" +
                    "<FROMDATE>" + strfromdate + "</FROMDATE>" +
                    "<TODATE>" + strtodate + "</TODATE>" +
                    "<STATE>" + strState + "</STATE>" +
                    "<AGENTTYPE>" + strAgntType + "</AGENTTYPE>" +
                    "<AVAILABILITYSTATUS>" + strAvailStatus + "</AVAILABILITYSTATUS>" +
                    "<STATUS>" + strStatus + "</STATUS>" +
                  "<AGENCYNAME>" + strAgencyId + "</AGENCYNAME>" +
                    "<SALESINCHARGE>" + strSalesInchrge + "</SALESINCHARGE>" +
                    "<USERNAME>" + UserName + "</USERNAME>" +
                    "<IPADDRESS>" + IPAddress + "</IPADDRESS>" +
                    "<SEQUENCEID>" + strSequenceId + "</SEQUENCEID>" +
                    "<TERMINALID>" + strTerminalID + "</TERMINALID>" +
                  "</EVENT>";

                DatabaseLog.LogData(UserName, "E", "CntrlPanelController", "FetchAgentDetailsReport", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                dsset = _rays_servers.selectclientDetails_new(strBranch, strAgencyname, strAgencyId, strAgntType, strStatus, "1", UserName, IPAddress, strSequenceId, "", strTerminalID,
                    strSalesInchrge, strfromdate, strtodate, strState, "");


                if (dsset != null && dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
                {
                    xml = "<EVENT><RESPONSE>FetchAgentDetailsReport</RESPONSE>SUCCESS<REQTIME>" + ReqTime + "</REQTIME><RESULT>" +
                        JsonConvert.SerializeObject(dsset).ToString() + "</RESULT></EVENT>";
                    if (dsset != null && dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
                    {
                        string strconv = JsonConvert.SerializeObject(dsset.Tables[0]).ToString();
                        arry[ResultCode] = "01";
                        status = "01";
                        arry[Result] = strconv;
                        Session["AGNTDETREPORTS"] = dsset;
                    }
                }
                else
                {
                    status = "00";
                    strErrmsg = "No Records Found";
                    arry[ResultCode] = "00";
                    arry[Error] = strErrmsg;
                    xml = "<EVENT><RESPONSE>FetchAgentDetailsReport</RESPONSE>FAILED<REQTIME>" + ReqTime + "</REQTIME><RESULT>" +
                        "No Records Found" + " </RESULT></EVENT>";
                }

                DatabaseLog.LogData(UserName, "E", "CntrlPanelController", "FetchAgentDetailsReport", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            catch (Exception ex)
            {
                status = "00";
                arry[ResultCode] = "00";
                xml = "<EXCEPTION>FetchAgentDetailsReport</EXCEPTION>" + ex.ToString();
                DatabaseLog.LogData(UserName, "X", "CntrlPanelController", "FetchAgentDetailsReport", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            return Json(new { Status = status, Error = strErrmsg, Results = arry });
        }


        public ActionResult ExportToExcel(string RepFlag)
        {

            DateTime dt = DateTime.Now;
            string toDate = dt.ToString("yyyyMMddHHmmss");//yyyy-MM-ddTHH:mm:sszzz
            DataSet ds = new DataSet();
            ds = (DataSet)Session["AGNTDETREPORTS"];
            string str_attachment = "";
            if (RepFlag == "AGENTDETAILS")
            {
                str_attachment = "attachment; filename=AgentDetails" + toDate + ".xls";
            }
            else if (RepFlag == "AGENTPRODUCTIVE")
            {
                str_attachment = "attachment; filename=AgentProductiveDetails" + toDate + ".xls";
            }
            Response.ClearContent();
            Response.AddHeader("content-disposition", str_attachment);
            Response.Buffer = true;
            Response.ContentType = "application/ms-excel";
            Response.Write("<HTML><HEAD>");
            Response.Write("<style>TD {font-family:Verdana; font-size: 11px;} </style>");
            Response.Write("</HEAD><BODY>");
            Response.Write("<TABLE border='1' style='width:950px'>");

            Response.Write("<TR>");
            if (ds.Tables[0].Columns.Contains("Edit"))
            {
                ds.Tables[0].Columns.Remove("Edit");
            }
            if (ds.Tables[0].Columns.Contains("View"))
            {
                ds.Tables[0].Columns.Remove("View");
            }
            if (ds.Tables[0].Columns.Contains("Add Terminal"))
            {
                ds.Tables[0].Columns.Remove("Add Terminal");
            }
            if (ds.Tables[0].Columns.Contains("Group Name"))
            {
                ds.Tables[0].Columns.Remove("Group Name");
            }
            if (ds.Tables[0].Columns.Contains("Action"))
            {
                ds.Tables[0].Columns.Remove("Action");
            }
            if (ds.Tables[0].Columns.Contains("AGENTCOUNT"))
            {
                ds.Tables[0].Columns.Remove("AGENTCOUNT");
            }
            if (ds.Tables[0].Columns.Contains("Block List Agent"))
            {
                ds.Tables[0].Columns.Remove("Block List Agent");
            }
            if (ds.Tables[0].Columns.Contains("Allowed Module"))
            {
                ds.Tables[0].Columns.Remove("Allowed Module");
            }
            if (ds.Tables[0].Columns.Contains("LockThread"))
            {
                ds.Tables[0].Columns.Remove("LockThread");
            }
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                Response.Write("<TD style='font-weight:bold;background-color:#245b9d; color:#fff;font-size: 12px;'>" + ds.Tables[0].Columns[i].ToString().Replace('_', ' ') + "</TD>");
            }
            Response.Write("</TR>");

            foreach (DataRow datarow in ds.Tables[0].Rows)
            {
                Response.Write("<TR style='height:25px'>");
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    Response.Write(@"<TD>");// style='mso-number-format:General;'
                    Response.Write(datarow[j].ToString().Replace("\n", "?").Replace("\r", "?").Replace("\t", "?").Replace("\r\n", "?").Replace(" ", "?").Replace("?", " "));
                    Response.Write("</TD>");
                }
                Response.Write("</TR>");
            }
            Response.Write("</TABLE>");
            Response.Write("</BODY></HTML>");

            Response.Flush();
            Response.End();

            return View();
        }

        #endregion

        #region Agent Productive Analysis
        public ActionResult AgentProductiveAnalysis()
        {
            #region UsageLog
            string PageName = "Agent Productive Analysis";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e) { }
            #endregion

            string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");

            if (strAgentId == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }

            ViewBag.ServerDateTime = Base.LoadServerdatetime();
            return View("~/Views/CntrlPanel/AgentProductiveAnalysis.cshtml");
        }

        public ActionResult FetchAgentProductiveData(string strBookingFrmDate, string strBookingTodate, string strActiveFrom, string strActiveTo, string strState,
            string strBranch, string strSalesman, string strCustomerNo, string strModule, string strAgencyId, string strAgentType, string strAgencyName)
        {
            ArrayList arry = new ArrayList();
            arry.Add("");
            arry.Add("");
            int Error = 0;
            int Result = 1;
            string status = string.Empty;
            string strErrMsg = "";
            string xml = string.Empty;
            string strAgentId = ((Session["agentid"] != null && Session["agentid"].ToString() != "") ? Session["agentid"].ToString() : "");
            string UserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strTerminalType = (Session["terminaltype"] != null && Session["terminaltype"].ToString() != "") ? Session["terminaltype"].ToString() : "";
            byte[] dsSupplier = new byte[] { };
            DataSet dsset = new DataSet();
            try
            {
                if (strAgentId == "" || strTerminalID == "" || strTerminalType == "")
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }
                if (strBookingFrmDate != "" && strBookingTodate != "" && strBookingFrmDate != null && strBookingTodate != null)
                {
                    strBookingFrmDate = DateTime.ParseExact(strBookingFrmDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    strBookingTodate = DateTime.ParseExact(strBookingTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                }
                if (strActiveFrom != null && strActiveFrom != "" && strActiveTo != null && strActiveTo != "")
                {
                    strActiveFrom = DateTime.ParseExact(strActiveFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    strActiveTo = DateTime.ParseExact(strActiveTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                }
                #region UsageLog
                string PageName = "Agent Productive Analysis";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                string ReqTime = DateTime.Now.ToString();
                xml = "<EVENT><REQUEST>FetchAgentProductiveData</REQUEST><REQTIME>" + ReqTime + "</REQTIME>" +
                    "<BOOKINGFROMDATE>" + strBookingFrmDate + "</BOOKINGFROMDATE>" +
                    "<BOOKINGTODATE>" + strBookingTodate + "</BOOKINGTODATE>" +
                    "<ACTIVEFROMDATE>" + strActiveFrom + "</ACTIVEFROMDATE>" +
                    "<ACTIVETO>" + strActiveTo + "</ACTIVETO>" +
                    "<STATE>" + strState + "</STATE>" +
                    "<BRANCH>" + strBranch + "</BRANCH>" +
                    "<SALESMAN>" + strSalesman + "</SALESMAN>" +
                    "<CUSTOMERID>" + strCustomerNo + "</CUSTOMERID>" +
                    "<MODULE>" + strModule + "</MODULE>" +
                    "<AGENCYID>" + strAgencyId + "</AGENCYID>" +
                    "<AGENTID>" + strAgentId + "</AGENTID>" + "<PAGENAME>CntrlPanelController</PAGENAME>" + "<FUNCTION>FetchAgentProductiveData</FUNCTION>" +
                    "<USERNAME>" + UserName + "</USERNAME>" + "<IPADDRESS>" + IPAddress + "</IPADDRESS>" + "<TERMINALID>" + strTerminalID + "</TERMINALID>" +
                    "<SEQUENCEID>" + strSequenceId + "</SEQUENCEID>" + "<AGENTTYPE>" + strAgentType + "</AGENTTYPE>" + "<AGENCYNAME>" + strAgencyName + "</AGENCYNAME>" +
                    "<TERMINALTYPE>" + strTerminalType + "</TERMINALTYPE>" +
                    "</EVENT>";

                DatabaseLog.LogData(UserName, "E", "CntrlPanelController", "FetchAgentProductiveData", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                dsSupplier = _rays_servers.FETCH_AGENT_PRODUCTIVE_ANALYSIS(strBookingFrmDate, strBookingTodate, strActiveFrom, strActiveTo, strState, strBranch, strSalesman, strCustomerNo, strModule,
                    strAgencyId, strAgentId, "CntrlPanelController", "FetchAgentProductiveData", UserName, IPAddress, strTerminalID, strSequenceId, strAgentType, strAgencyName, strTerminalType);
                if (dsSupplier != null)
                {
                    dsset = Base.Decompress(dsSupplier);
                    if (dsset != null && dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
                    {
                        if (dsset.Tables[0].Columns.Contains("ORD_BY"))
                        {
                            dsset.Tables[0].Columns.Remove("ORD_BY");
                        }
                        xml = "<EVENT><RESPONSE>FetchAgentProductiveData</RESPONSE>SUCCESS<REQTIME>" + ReqTime + "</REQTIME><RESULT>SUCCESS</RESULT></EVENT>";
                        if (dsset != null && dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
                        {
                            string strconv = JsonConvert.SerializeObject(dsset.Tables[0]).ToString();
                            status = "01";
                            Session["AGNTDETREPORTS"] = dsset;
                            arry[Result] = strconv;
                        }
                    }
                    else
                    {
                        status = "00";
                        strErrMsg = "No Records Found";
                        arry[Error] = strErrMsg;
                        xml = "<EVENT><RESPONSE>FetchAgentProductiveData</RESPONSE>FAILED<REQTIME>" + ReqTime + "</REQTIME><RESULT>" +
                            "No Records Found" + " </RESULT></EVENT>";
                    }
                }
                DatabaseLog.LogData(UserName, "E", "CntrlPanelController", "FetchAgentProductiveData", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            catch (Exception ex)
            {
                status = "00";
                arry[Error] = "00";
                xml = "<EXCEPTION>FetchAgentProductiveData</EXCEPTION>" + ex.ToString();
                DatabaseLog.LogData(UserName, "X", "CntrlPanelController", "FetchAgentProductiveData", xml.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            var temp = Json(new { Status = status, Error = strErrMsg, Results = arry });
            temp.MaxJsonLength = int.MaxValue;
            return temp;

        }
        #endregion

        public ActionResult UserCreation()
        {
            #region UsageLog
            string PageName = "User Creation";
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
            return View("~/Views/CntrlPanel/UserCreation.cshtml");
        }

        public ActionResult InsertUserDetails(string strFlag, string strUsername, string strPassword, string strcontactno, string stremailid, string strterminalprivilege,
            string strstatus, string strremarks, string strTerminalid)
        {
            string strResult = "";
            string strStatus = "";
            string strError = "";
            string strSequenceId = DateTime.Now.ToString("yyyyMMddhhmmss");
            string strBranchID = Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "";
            string strUserName = Session["username"] != null && Session["username"].ToString() != "" ? Session["username"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : strSequenceId;
            string strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
            string strTerminalID = Session["terminalid"] != null ? Session["terminalid"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null ? Session["TERMINALTYPE"].ToString() : "";
            string strUrl = string.Empty;
            string strResultcode = string.Empty;
            string strLogData = string.Empty;
            string strMessage = "";
            try
            {
                if (strAgentID.ToString() == "" || strUserName == "" || strTerminalID.ToString() == "")
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }
                if (strFlag == "I")
                {
                    strTerminalid = strterminalprivilege == "S" ? strAgentID + "01" : strAgentID + "02";
                }
                strUrl = ConfigurationManager.AppSettings["RCSUPPORTSERVICEURL"].ToString();
                strLogData = "<REQUEST><URL>" + strUrl + "</URL>" + "<AGENTID>" + strAgentID + "</AGENTID>"
                    + "<TERMINALID>" + strTerminalid + "</TERMINALID>" + "<USERNAME>" + strUsername + "</USERNAME>"
                    + "<IPADDRESS>" + IPAddress + "</IPADDRESS>" + "<SEQUENCEID>" + strSequenceId + "</SEQUENCEID>"
                    + "<USERMOBILE>" + strcontactno + "</USERMOBILE>" + "<USEREMAIL>" + stremailid + "</USEREMAIL>"
                    + "<PASSWORD>" + strPassword + "</PASSWORD><FLAG>" + strFlag + "</FLAG>" + "<LOGINTERMINALID>" + strTerminalID + "</LOGINTERMINALID>"
                    + "<LOGINUSERNAME>" + strUserName + "</LOGINUSERNAME></REQUEST>";
                DatabaseLog.LogData(strUserName, "E", "CntrlPanelController", "InsertUserDetails~REQ", strLogData, strAgentID, strTerminalID, strSequenceId);

                Hashtable hsParam = new Hashtable();
                hsParam.Add("strFlag", strFlag);
                hsParam.Add("strAgentID", strAgentID);
                hsParam.Add("strTerminalID", strTerminalid);
                hsParam.Add("strUserName", strUsername);
                hsParam.Add("strPassword", strPassword);
                hsParam.Add("strLoginUsername", strUserName);
                hsParam.Add("strIpAddress", IPAddress);
                hsParam.Add("strContactNo", strcontactno);
                hsParam.Add("strEmailid", stremailid);
                hsParam.Add("strTerminalPrivilege", strterminalprivilege);
                hsParam.Add("strStatus", strstatus);
                hsParam.Add("strRemarks", strremarks);
                hsParam.Add("strTerminalType", strTerminalType);
                hsParam.Add("strSequenceID", strSequenceId);
                hsParam.Add("strErrorMsg", "");
                hsParam.Add("strResult", "");
                hsParam.Add("strResultcode", "");

                JObject objJsonres = Base.callWebMethod("InsertUpdateUserDetails", hsParam, ref strMessage);

                if (objJsonres != null)
                {
                    bool JsonBook = (bool)objJsonres["InsertUpdateUserDetailsResult"];
                    strResult = (string)objJsonres["strResult"];
                    strError = (string)objJsonres["strErrorMsg"];
                    strResultcode = (string)objJsonres["strResultcode"];
                    if (JsonBook == true && strResultcode != "00")
                    {
                        strStatus = strResultcode;
                        strResult = (string)objJsonres["strResult"];
                    }
                    else
                    {
                        strStatus = strResultcode;
                        strResult = (string)objJsonres["strResult"];
                    }
                }
                else
                {
                    strStatus = "00";
                    strResult = !string.IsNullOrEmpty(strMessage) ? strMessage : "Problem occured while Processing your request.Please contact customer care.(#03).";
                }

                strLogData = "<RESPONSE><URL>" + strUrl + "</URL><STATUS>" + strStatus + "</STATUS>" +
                    "<MESSAGE>" + strMessage + "</MESSAGE><RESULT>" + strResult + "</RESULT></RESPONSE>";
                DatabaseLog.LogData(strUserName, "E", "CntrlPanelController", "InsertUserDetails~RES", strLogData, strAgentID, strTerminalID, strSequenceId);
            }
            catch (Exception ex)
            {
                strStatus = "00";
                strError = "Problem occured while Processing your request.Please contact customer care.(#5)";
                DatabaseLog.LogData(strUserName, "X", "CntrlPanelController", "InsertUserDetails", ex.ToString(), strAgentID, strTerminalID, strSequenceId);
            }
            return Json(new { Status = strStatus, Error = strError, Results = strResult });
        }

        #region Added by Murugesan-177
        public ActionResult FetchApprovedCommission(string strAgencyname, string strAgentId, string strStatus, string strRiyapnr, string strFromdate, string strTodate, string strRemarks,string strFlag)
        {
            string strResultCode = string.Empty;
            string strError = string.Empty;
            string strResult = string.Empty;
            string strLogData = string.Empty;
            string strRefmsg = string.Empty;
            string strPagename = "CntrlPanelController";
            string strSequenceId = DateTime.Now.ToString("yyyyMMddhhmmss");
            string strBranchID = Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "";
            string strUserName = Session["username"] != null && Session["username"].ToString() != "" ? Session["username"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : strSequenceId;
            string strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
            string strTerminalID = Session["terminalid"] != null ? Session["terminalid"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null ? Session["TERMINALTYPE"].ToString() : "";
            try
            {
                RaysService _RaysService = new RaysService();
                _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                if (strAgentID.ToString() == "" || strUserName == "" || strTerminalID.ToString() == "")
                {
                    return Json(new { Status = "-1", Error = "", Results = "" });
                }
                strLogData = "<REQUEST><URL>" + ConfigurationManager.AppSettings["ServiceURI"].ToString() + "</URL>" + "<AGENTID>" + strAgentID + "</AGENTID>"
                           + "<TERMINALID>" + strTerminalID + "</TERMINALID>" + "<USERNAME>" + strUserName + "</USERNAME>"
                           + "<IPADDRESS>" + IPAddress + "</IPADDRESS>" + "<SEQUENCEID>" + strSequenceId + "</SEQUENCEID>"
                           + "<TERMINALTYPE>" + strTerminalType + "</TERMINALTYPE>" + "<BRANCHID>" + strBranchID + "</BRANCHID>"
                           + "<AGENCYNAME>" + strAgencyname + "</AGENCYNAME><AGENTIDFETCH>" + strAgentId + "</AGENTIDFETCH>" + "<STATUS>" + strTerminalID + "</STATUS>"
                           + "<REMARKS>" + strRemarks + "</REMARKS>" + "<FLAG>" + strFlag + "</FLAG>"
                           + "<RIYAPNR>" + strRiyapnr + "</RIYAPNR><FROMDATE>" + strFromdate + "</FROMDATE>" + "<TODATE>" + strTodate + "</TODATE></REQUEST>";

                DatabaseLog.LogData(strUserName, "E", strPagename, "FetchApprovedCommissionREQ", strLogData, strAgentID, strTerminalID, strSequenceId);
                string res = _RaysService.FetchUpdateCommissionOnUserRights_RBOA(strAgentID, strTerminalID, strUserName, IPAddress, strTerminalType, strSequenceId, strPagename, strFlag,
                                                                               strRiyapnr, strAgentId, strRemarks, strStatus, ref strRefmsg, strFromdate, strTodate, "", "");
                strLogData = "<RESPONSE><MESSAGE>" + res + "</MESSAGE>" + "<RESULT>" + strRefmsg + "</RESULT></RESPONSE>";

               
                if (res != null && res != "")
                {
                    strResultCode = "0";
                    strError = res;
                    strResult = "";
                }
                else
                {
                    if (strFlag.ToUpper().Trim() == "FETCH_DISP")
                    {
                        DataSet dsset = Serv.convertJsonStringToDataSet(strRefmsg, "");
                        var qry = from data in dsset.Tables[0].AsEnumerable()
                                  select new
                                  {
                                      PAX_TYPE = data["PAX_TYPE"],
                                      PAX_NAME = data["PAX_NAME"],
                                      TCK_ORIGIN_CITY_ID = data["TCK_ORIGIN_CITY_ID"],
                                      TCK_DESTINATION_CITY_ID = data["TCK_DESTINATION_CITY_ID"],
                                      CAC_ACTUAL_COMMISSION = data["CAC_ACTUAL_COMMISSION"],
                                      CAC_NEW_COMMISSION = data["CAC_NEW_COMMISSION"],
                                      CAC_ACTUAL_INCENTIVE = data["CAC_ACTUAL_INCENTIVE"],
                                      CAC_NEW_INCENTIVE = data["CAC_NEW_INCENTIVE"],
                                      CAC_ACTUAL_PLB = data["CAC_ACTUAL_PLB"],
                                      CAC_NEW_PLB = data["CAC_NEW_PLB"],
                                      CAC_ACTUAL_SERVICE_TAX = data["CAC_ACTUAL_SERVICE_TAX"],
                                      CAC_NEW_SERVICE_TAX = data["CAC_NEW_SERVICE_TAX"],
                                      CAC_ACTUAL_TDS = data["CAC_ACTUAL_TDS"],
                                      CAC_NEW_TDS = data["CAC_NEW_TDS"],
                                      AGENCYNAME= data["AGENCY1"],
                                      AUTHERIZED = data["CAC_AUTHORIZED_BY1"],
                                      BOOKEDUSER = data["TCK_USER_NAME1"],
                                      STATUS = data["CAC_STATUS1"],
                                      PNR = data["CAC_S_PNR1"]                                     

                                  };
                        DataTable dtData = Serv.ConvertToDataTable(qry);
                        strRefmsg = JsonConvert.SerializeObject(dtData);
                    }
                    strResultCode = "1";
                    strError = "";
                    strResult = strRefmsg;
                }
                DatabaseLog.LogData(strUserName, "E", strPagename, "FetchApprovedCommissionRES", strLogData, strAgentID, strTerminalID, strSequenceId);

            }
            catch (Exception ex)
            {
                strResultCode = "0";
                strError = "Unable to Process,Please contact support Team.";
                strResult = "";
                DatabaseLog.LogData(strUserName, "X", strPagename, "FetchApprovedCommission", ex.ToString(), strAgentID, strTerminalID, strSequenceId);
            }
            return Json(new { Status = strResultCode, Error = strError, Results = strResult });
        }
        #endregion
    }
}
