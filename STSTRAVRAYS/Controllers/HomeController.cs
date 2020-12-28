using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using STSTRAVRAYS.Models;
using System.Web.UI;
using System.Data;
using STSTRAVRAYS.Rays_service;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Net;

namespace STSTRAVRAYS.Controllers
{
    public class HomeController : Controller
    {
        string strAgentID = string.Empty;
        string strTerminalId = string.Empty;
        string strUserName = string.Empty;
        string strBranchId = string.Empty;
        string sequnceID = string.Empty;
        string Ipaddress = string.Empty;
        string agentId = string.Empty;
        string terminalId = string.Empty;
        string terminalType = string.Empty;
        string ipAddress = string.Empty;
        string sequenceId = string.Empty;
        RaysService _rays_servers = new RaysService();
        Base.ServiceUtility Serv = new Base.ServiceUtility();
        string strBranchCredit = ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"] != null ? ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"].ToString().ToUpper() : "";
        string strPlatform = ConfigurationManager.AppSettings["PLATFORM"].ToString();
        string strLogoUrl = ConfigurationManager.AppSettings["LogoUrl"].ToString();
        string strProduct = ConfigurationManager.AppSettings["Producttype"].ToString();
        string strAppHosting = ConfigurationManager.AppSettings["APP_HOSTING"].ToString();

        #region HomePage
        public ActionResult HomeMaster()
        {
            #region UsageLog
            string PageName = "Home Page";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "VISIT");
            }
            catch (Exception e)
            {
            }
            #endregion
            try
            {
                ViewBag.loadpage = (Session["WEBHOMEURL"] != null && Session["WEBHOMEURL"] != "") ? Session["WEBHOMEURL"] : "";
                if (Session["agentid"] == null || Session["agentid"].ToString() == "")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                }
                ViewBag.ServerDateTime = Base.LoadServerdatetime();

                string str_terminalid = string.Empty;
                string str_agentid = string.Empty;
                string str_username = string.Empty;
                string str_sequenceNo = string.Empty;
                string str_ipaddres = string.Empty;
                string str_credit = string.Empty;
                string str_terminaltype = string.Empty;
                string str_privilage = string.Empty;
                string str_agentType = string.Empty;
                string strUserid = string.Empty;
                string strCompanyid = string.Empty;
                string strCstnameID = string.Empty;
                string POS_TID = string.Empty;
                string POS_ID = string.Empty;
                string strBranch = string.Empty;
                string Credit_train = string.Empty;
                string Password = string.Empty;

                str_terminalid = Session["terminalid"] != null ? Session["terminalid"].ToString() : "";
                str_agentid = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                str_username = Session["username"] != null ? Session["username"].ToString() : "";
                str_sequenceNo = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
                str_ipaddres = Base.GetComputer_IP();
                str_credit = Session["creditacc"] != null ? Session["creditacc"].ToString() : "";
                str_terminaltype = Session["terminaltype"] != null ? Session["terminaltype"].ToString() : "";
                str_privilage = Session["privilage"] != null ? Session["privilage"].ToString() : "";
                str_agentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
                strUserid = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strCompanyid = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                POS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                POS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strBranch = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                Password = Session["PWD"] != null ? Session["PWD"].ToString() : "";

                ViewBag.txt_gatewayenterby = Session["username"].ToString();
                if (Session["AgencyPaymentDetails"] != null && Session["AgencyPaymentDetails"].ToString() != "")
                {
                    string[] AgencyPaymentDetails = Session["AgencyPaymentDetails"].ToString().Split('~');
                    ViewBag.txt_gatwayfirstname = AgencyPaymentDetails[0].Replace("\r", "").Replace("\n", "");
                    ViewBag.txt_gatwaylastname = AgencyPaymentDetails[1].Replace("\r", "").Replace("\n", "");
                    ViewBag.txt_gatwaymailid = AgencyPaymentDetails[2].Replace("\r", "").Replace("\n", "");
                    ViewBag.Txt_gatewayaddress1 = AgencyPaymentDetails[3].Replace("\r", "").Replace("\n", "");
                    ViewBag.txt_gatewayaddress2 = AgencyPaymentDetails[4].Replace("\r", "").Replace("\n", "");
                    ViewBag.Txt_gatewaycity = AgencyPaymentDetails[5].Replace("\r", "").Replace("\n", "");
                }

                ViewBag.hdn_agnmobno = Session["agencyaddress"].ToString().Split('~')[4];
                ViewBag.hdn_currency_code = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();

                ViewBag.hdnmaxpercalc = Session["payfeeprecent"].ToString();
                ViewBag.hdntransactionpercent = Session["pg_charge"].ToString();//.Split('|')[0];
                ViewBag.hdn_PGCommoncharge = Session["pg_Commonservice"].ToString();//.Split('|')[1];


                ViewBag.agencyname = Session["agencyname"].ToString();
                ViewBag.agenmail = Session["agencyaddress"].ToString().Split('~')[2];

                DataSet vatstatus = new DataSet();

                if (vatstatus != null && vatstatus.Tables.Count > 0 && vatstatus.Tables[0].Rows.Count > 0)
                {
                    ViewBag.hdn_vatagentid = str_agentid;
                    ViewBag.hdn_vatagentName = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_COMPANY_NAME"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_COMPANY_NAME"].ToString();
                    ViewBag.hdn_vatfullname = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_FULL_NAME"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_FULL_NAME"].ToString();
                    ViewBag.hdn_vatagentmail = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_MAIL"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_MAIL"].ToString();
                    ViewBag.hdn_vatagentcontact = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_CONTACT_NO"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_CONTACT_NO"].ToString();
                    ViewBag.hdn_vatregno = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_REG_NO"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_REG_NO"].ToString();
                    ViewBag.hdn_vatinvoice = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_INVOICE_PAYMENT"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_INVOICE_PAYMENT"].ToString();
                    ViewBag.hdn_vatstatus = "1";
                }
                else
                {
                    string[] AgencyPaymentDetails = Session["AgencyPaymentDetails"].ToString().Split('~');
                    string txt_gatwayfirstname = AgencyPaymentDetails[0].Replace("\r", "").Replace("\n", "");
                    string txt_gatwaylastname = AgencyPaymentDetails[1].Replace("\r", "").Replace("\n", "");
                    string txt_gatwaymailid = AgencyPaymentDetails[2].Replace("\r", "").Replace("\n", "");
                    ViewBag.hdn_vatstatus = "1";
                    ViewBag.hdn_vatagentid = str_agentid;
                    ViewBag.hdn_vatagentName = Session["agencyname"].ToString();
                    ViewBag.hdn_vatfullname = txt_gatwayfirstname + " " + txt_gatwaylastname;
                    ViewBag.hdn_vatagentmail = txt_gatwaymailid;
                    ViewBag.hdn_vatagentcontact = Session["agencyaddress"].ToString().Split('~')[4];
                    ViewBag.hdn_vatregno = "";
                    ViewBag.hdn_vatinvoice = "";
                }
                string strAgentPOS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";  // LOGIN AGENT ID
                string strAgentPOS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : ""; // LOGIN TERMINAL ID
                string strBranchid = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
                string strSequenceNo = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyMMddHHmm"); ;
                string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
                string strPrivilage = Session["privilage"] != null ? Session["privilage"].ToString() : "";
                string strAgentTerminalCount = Session["AgentTerminalCount"] != null ? Session["AgentTerminalCount"].ToString() : "";
                string strResultCode = string.Empty;
                string strErrorMsg = string.Empty;
                string strResult = string.Empty;
                if (strTerminalType == "W" && strProduct == "RBOA")
                {
                    return View("~/Views/Home/B2B_Home.cshtml");
                }
                else if (strTerminalType == "T" && strProduct == "RBOA")
                {
                    ViewBag.currentdate = Base.LoadServerdatetime();
                    return View("~/Views/Home/HomeBoard.cshtml");
                }
                else if (strTerminalType == "T" && (strAppHosting == "BSA" || strAppHosting == "BOA"))
                {
                    return View("~/Views/Home/HomeBoard.cshtml");
                }
            }
            catch (Exception Ex)
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            return View("~/Views/Home/HomeMaster.cshtml");
        }

        public ActionResult Home()
        {
            if (Session["agentid"] == null || Session["agentid"].ToString() == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            ViewBag.ServerDateTime = Base.LoadServerdatetime();

            string str_terminalid = string.Empty;
            string str_agentid = string.Empty;
            string str_username = string.Empty;
            string str_sequenceNo = string.Empty;
            string str_ipaddres = string.Empty;
            string str_credit = string.Empty;
            string str_terminaltype = string.Empty;
            string str_privilage = string.Empty;
            string str_agentType = string.Empty;
            string strUserid = string.Empty;
            string strCompanyid = string.Empty;
            string strCstnameID = string.Empty;
            string POS_TID = string.Empty;
            string POS_ID = string.Empty;
            string strBranch = string.Empty;
            string Credit_train = string.Empty;
            string Macaddress = string.Empty;
            string irctcusername = string.Empty;
            string Password = string.Empty;

            str_terminalid = Session["terminalid"] != null ? Session["terminalid"].ToString() : "";
            str_agentid = Session["agentid"] != null ? Session["agentid"].ToString() : "";
            str_username = Session["username"] != null ? Session["username"].ToString() : "";
            str_sequenceNo = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            str_ipaddres = Base.GetComputer_IP();
            str_credit = Session["creditacc"] != null ? Session["creditacc"].ToString() : "";
            str_terminaltype = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            str_privilage = Session["privilage"] != null ? Session["privilage"].ToString() : "";
            str_agentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            strUserid = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            strCompanyid = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            POS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            POS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            strBranch = Session["branchid"] != null ? Session["branchid"].ToString() : "";
            Password = Session["PWD"] != null ? Session["PWD"].ToString() : "";

            Macaddress = Session["Macaddress"] != null ? Session["Macaddress"].ToString() : "";
            irctcusername = Session["irctcusername"] != null ? Session["irctcusername"].ToString() : "";

            ViewBag.HotelQuerystring = "tid=" + str_terminalid + "&agentid="
                + str_agentid + "&username=" + str_username + "&seq=" + str_sequenceNo + "&IPA="
                + str_ipaddres + "&credit=" + str_credit + "&typ=" + str_terminaltype + "&privilage="
                + str_privilage + "&AGNTYPE=" + str_agentType + "&userid=" + strUserid + "&companyId=" + strCompanyid + "&costcenter=" + strCstnameID + "&strflag=" + "TDK&pos_tid=" + POS_TID + "&pos_id=" + POS_ID + "&branchid=" + strBranch + "&AGNTYPE=TB" + "&Referenceid=";

            ViewBag.Otherproductstring = "CLIENTID=" + str_agentid + "&TERMINALID="
               + str_terminalid + "&USERNAME=" + str_username + "&PWD=" + Password + "&SEQUENCEID="
               + str_sequenceNo + "&BRANCHID=" + strBranch + "&TERMINALPREVILAGE=" + str_privilage + "&TERMINALTYPE="
               + str_terminaltype + "&IPADDRESS=" + str_ipaddres + "";

            ViewBag.Otherproductstring2 = "CLIENTID=" + str_agentid + "&TERMINALID="
               + str_terminalid + "&USERNAME=" + str_username + "&PWD=" + Password + "&SEQUENCEID="
               + str_sequenceNo + "&BRANCHID=" + strBranch + "&TERMINALPREVILAGE=" + str_privilage + "";


            string today = DateTime.Now.ToString("dd/MM/yyyy");
            string Querystring = "SECKEY=" + Base.EncryptKEy(ViewBag.HotelQuerystring, "RIYA" + today);
            string RiyaQuerystring = "SECKEY=" + Base.EncryptKEy(ViewBag.Otherproductstring, "RIYA" + today);
            string RiyaQuerystring2 = "SECKEY=" + Base.EncryptKEy(ViewBag.Otherproductstring2, "RIYA" + today) + "&TERMINALTYPE=" + str_terminaltype + "&IPADDRESS=" + str_ipaddres + "";
            Session.Add("OtherProductLoad", Querystring);
            Session.Add("RiyaOtherProductLoad", RiyaQuerystring);
            Session.Add("RiyaOtherProductLoad2", RiyaQuerystring2);

            ViewBag.txt_gatewayenterby = Session["username"].ToString();
            if (Session["AgencyPaymentDetails"] != null && Session["AgencyPaymentDetails"].ToString() != "")
            {
                string[] AgencyPaymentDetails = Session["AgencyPaymentDetails"].ToString().Split('~');
                ViewBag.txt_gatwayfirstname = AgencyPaymentDetails[0].Replace("\r", "").Replace("\n", "");
                ViewBag.txt_gatwaylastname = AgencyPaymentDetails[1].Replace("\r", "").Replace("\n", "");
                ViewBag.txt_gatwaymailid = AgencyPaymentDetails[2].Replace("\r", "").Replace("\n", "");
                ViewBag.Txt_gatewayaddress1 = AgencyPaymentDetails[3].Replace("\r", "").Replace("\n", "");
                ViewBag.txt_gatewayaddress2 = AgencyPaymentDetails[4].Replace("\r", "").Replace("\n", "");
                ViewBag.Txt_gatewaycity = AgencyPaymentDetails[5].Replace("\r", "").Replace("\n", "");
            }

            ViewBag.hdn_agnmobno = Session["agencyaddress"].ToString().Split('~')[4];
            ViewBag.hdn_currency_code = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();

            ViewBag.hdnmaxpercalc = Session["payfeeprecent"].ToString();
            ViewBag.hdntransactionpercent = Session["pg_charge"].ToString();//.Split('|')[0];
            ViewBag.hdn_PGCommoncharge = Session["pg_Commonservice"].ToString();//.Split('|')[1];


            ViewBag.agencyname = Session["agencyname"].ToString();
            ViewBag.agenmail = Session["agencyaddress"].ToString().Split('~')[2];
            //ViewBag.mobileno = Session["agencyaddress"].ToString().Split('~')[4];

            DataSet vatstatus = new DataSet();
            if (strProduct == "UAE")
            {
                vatstatus = fetch_vat_registration(str_agentid);
            }

            if (vatstatus != null && vatstatus.Tables.Count > 0 && vatstatus.Tables[0].Rows.Count > 0)
            {
                ViewBag.hdn_vatagentid = str_agentid;
                ViewBag.hdn_vatagentName = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_COMPANY_NAME"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_COMPANY_NAME"].ToString();
                ViewBag.hdn_vatfullname = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_FULL_NAME"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_FULL_NAME"].ToString();
                ViewBag.hdn_vatagentmail = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_MAIL"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_MAIL"].ToString();
                ViewBag.hdn_vatagentcontact = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_CONTACT_NO"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_CONTACT_NO"].ToString();
                ViewBag.hdn_vatregno = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_REG_NO"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_REG_NO"].ToString();
                ViewBag.hdn_vatinvoice = string.IsNullOrEmpty(vatstatus.Tables[0].Rows[0]["VAT_INVOICE_PAYMENT"].ToString()) ? "" : vatstatus.Tables[0].Rows[0]["VAT_INVOICE_PAYMENT"].ToString();
                ViewBag.hdn_vatstatus = "1";
            }
            else
            {
                string[] AgencyPaymentDetails = Session["AgencyPaymentDetails"].ToString().Split('~');
                string txt_gatwayfirstname = AgencyPaymentDetails[0].Replace("\r", "").Replace("\n", "");
                string txt_gatwaylastname = AgencyPaymentDetails[1].Replace("\r", "").Replace("\n", "");
                string txt_gatwaymailid = AgencyPaymentDetails[2].Replace("\r", "").Replace("\n", "");
                ViewBag.hdn_vatstatus = "0";
                ViewBag.hdn_vatagentid = str_agentid;
                ViewBag.hdn_vatagentName = Session["agencyname"].ToString();
                ViewBag.hdn_vatfullname = txt_gatwayfirstname + " " + txt_gatwaylastname;
                ViewBag.hdn_vatagentmail = txt_gatwaymailid;
                ViewBag.hdn_vatagentcontact = Session["agencyaddress"].ToString().Split('~')[4];
                ViewBag.hdn_vatregno = "";
                ViewBag.hdn_vatinvoice = "";
            }

            string strAgentPOS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";  // LOGIN AGENT ID
            string strAgentPOS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : ""; // LOGIN TERMINAL ID
            string strBranchid = Session["branchid"] != null ? Session["branchid"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strSequenceNo = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyMMddHHmm"); ;
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strPrivilage = Session["privilage"] != null ? Session["privilage"].ToString() : "";
            string strAgentTerminalCount = Session["AgentTerminalCount"] != null ? Session["AgentTerminalCount"].ToString() : "";

            string strResultCode = string.Empty;
            string strErrorMsg = string.Empty;
            string strResult = string.Empty;


            if (str_terminaltype == "W" && strProduct == "RBOA")
            {
                return View("~/Views/Home/B2B_Home.cshtml");
            }
            else if (str_terminaltype == "T" && strProduct == "RBOA")
            {
                ViewBag.currentdate = Base.LoadServerdatetime();
                return View("~/Views/Home/HomeBoard.cshtml");
            }
            return View("~/Views/Home/HomeMaster.cshtml");
        }

        public ActionResult Homeflight()
        {
            if (Session["agentid"] == null || Session["agentid"].ToString() == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            ViewBag.ServerDateTime = Base.LoadServerdatetime();

            string str_terminalid = string.Empty;
            string str_agentid = string.Empty;
            string str_username = string.Empty;
            string str_sequenceNo = string.Empty;
            string str_ipaddres = string.Empty;
            string str_credit = string.Empty;
            string str_terminaltype = string.Empty;
            string str_privilage = string.Empty;
            string str_agentType = string.Empty;

            str_terminalid = Session["terminalid"] != null ? Session["terminalid"].ToString() : "";
            str_agentid = Session["agentid"] != null ? Session["agentid"].ToString() : "";
            str_username = Session["username"] != null ? Session["username"].ToString() : "";
            str_sequenceNo = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            str_ipaddres = Base.GetComputer_IP();
            str_credit = Session["creditacc"] != null ? Session["creditacc"].ToString() : "";
            str_terminaltype = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            str_privilage = Session["privilage"] != null ? Session["privilage"].ToString() : "";
            str_agentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";

            ViewBag.HotelQuerystring = "tid=" + str_terminalid + "&agentid="
                + str_agentid + "&username=" + str_username + "&seq=" + str_sequenceNo + "&IPA="
                + str_ipaddres + "&credit=" + str_credit + "&typ=" + str_terminaltype + "&privilage="
                + str_privilage + "&AGNTYPE=" + str_agentType;
            Session.Add("OtherProductLoad", ViewBag.HotelQuerystring);
            //Session.Add("InsuranceProductLoad", ViewBag.InsuranceQuerystring);
            ViewBag.agencyname = Session["agencyname"].ToString();
            ViewBag.agenmail = Session["agencyaddress"].ToString().Split('~')[2];
            //ViewBag.mobileno = Session["agencyaddress"].ToString().Split('~')[4];

            ViewBag.txt_gatewayenterby = Session["username"].ToString();
            if (Session["AgencyPaymentDetails"] != null && Session["AgencyPaymentDetails"].ToString() != "")
            {

                string[] AgencyPaymentDetails = Session["AgencyPaymentDetails"].ToString().Split('~');

                ViewBag.txt_gatwayfirstname = AgencyPaymentDetails[0].Replace("\r", "").Replace("\n", "");
                ViewBag.txt_gatwaylastname = AgencyPaymentDetails[1].Replace("\r", "").Replace("\n", "");
                ViewBag.txt_gatwaymailid = AgencyPaymentDetails[2].Replace("\r", "").Replace("\n", "");
                ViewBag.Txt_gatewayaddress1 = AgencyPaymentDetails[3].Replace("\r", "").Replace("\n", "");
                ViewBag.txt_gatewayaddress2 = AgencyPaymentDetails[4].Replace("\r", "").Replace("\n", "");
                ViewBag.Txt_gatewaycity = AgencyPaymentDetails[5].Replace("\r", "").Replace("\n", "");

            }
            ViewBag.hdn_agnmobno = Session["agencyaddress"].ToString().Split('~')[4];
            ViewBag.hdn_currency_code = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();

            ViewBag.hdnmaxpercalc = Session["payfeeprecent"].ToString();
            ViewBag.hdntransactionpercent = Session["pg_charge"].ToString();//.Split('|')[0];
            ViewBag.hdn_PGCommoncharge = Session["pg_Commonservice"].ToString();//.Split('|')[1];

            return RedirectToAction("Flights", "Flights");
        }

        public ActionResult Redirectpaymentpage()
        {
            ViewBag.loadpage = "YES";
            return View("~/Views/Home/HomeMaster.cshtml");
        }
        #endregion

        #region Agent Balance
        public ActionResult BalanceCheck(string AgentId)
        {
            #region UsageLog
            string PageName = "Balance Check";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
            }
            catch (Exception e)
            {
            }
            #endregion
            string strErrorMsg = string.Empty;
            string strPOSID = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalID = string.Empty;
            string strUserName = string.Empty;
            string strCliendId = string.Empty;
            string strTerminaltype = string.Empty;

            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            int response = 1;
            int threadresponse = 2;
            string stu = string.Empty;
            string msg = string.Empty;
            try
            {
                strPOSID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
                strAgentID = Session["AGENTID"] != null && Session["AGENTID"].ToString() != "" ? Session["AGENTID"].ToString() : "";
                strTerminalID = Session["TERMINALID"] != null && Session["TERMINALID"].ToString() != "" ? Session["TERMINALID"].ToString() : "";
                strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
                strCliendId = Session["CompanyID"] != null && Session["CompanyID"].ToString() != "" ? Session["CompanyID"].ToString() : "";
                strTerminaltype = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TerminalType"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                string balance = string.Empty;
                DataSet dataSet = new DataSet();
                string strBalFetch = "";
                string topupAmount = string.Empty, creditAmount = string.Empty;
                int Topup = 0; int Outstanding = 0; int TotalOutstanding = 0;
                #region Topup Balance Amount check

                string agentid = (AgentId != null && AgentId != "") ? AgentId : Session["POS_ID"].ToString();

                dataSet = _rays_servers.Fetch_Client_Credit_Balance_DetailsWEB(agentid, Session["username"].ToString(), Session["ipAddress"].ToString(), Session["sequenceid"].ToString(), strPlatform);
                StringWriter strREQUEST = new StringWriter();

                if (dataSet != null)
                    dataSet.WriteXml(strREQUEST);

                DatabaseLog.LogData(Session["username"].ToString(), "T", "Availpage", "BalanceCheck", strREQUEST.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
                if (dataSet != null && dataSet.Tables.Count != 0 && dataSet.Tables[0].Rows.Count != 0)
                {
                    Session["CLIENT_TYPE"] = (dataSet.Tables[0].Columns.Contains("CLT_CORP_TYPE") && dataSet.Tables[0].Rows[0]["CLT_CORP_TYPE"] != null) ? dataSet.Tables[0].Rows[0]["CLT_CORP_TYPE"].ToString().Trim() : "";
                    Session["Client_AgentId"] = AgentId != null && AgentId != "" ? AgentId : Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        string ccur = dataSet.Tables[0].Columns.Contains("CLT_ALLOW_ACC_TYPE") ? dataSet.Tables[0].Rows[i]["CLT_ALLOW_ACC_TYPE"].ToString().Trim() : "";
                        string appcurrency = dataSet.Tables[0].Columns.Contains("CCD_CURRENCY_CODE") ? dataSet.Tables[0].Rows[i]["CCD_CURRENCY_CODE"].ToString().Trim() : "";
                        string[] ss = ccur.Split('|');
                        if (ss.Length > 0)
                        {
                            string strTopup = string.Empty; string strCredit = string.Empty; string strOutstanding = string.Empty;
                            for (int j = 0; j < ss.Length; j++)
                            {
                                if (strProduct == "UAE")
                                {
                                    if (ss[j] == "T")
                                    {
                                        Session["ALLOW_TOPUP"] = "Y";
                                        Topup = Convert.ToInt32((dataSet.Tables[0].Columns.Contains("CLT_OPENING_BALANCE") ? dataSet.Tables[0].Rows[i]["CLT_OPENING_BALANCE"] : "0"));
                                        strTopup = "<div><div class=''><img src='" + strLogoUrl + "/" + strProduct + "/AddMoney.gif'></div><div class='fl' style='line-height:32px;'>Active Balance</span><span class='ft' style='line-height:32px;'> : </span></div><div class='ft clr-gr' style='line-height:32px;'><span>" + appcurrency + "</span><span>  " + Topup + "</div></div>|";
                                    }
                                    if (ss[j] == "C")
                                    {
                                        Session["ALLOW_CREDIT"] = "Y";
                                        strCredit = "<div><div class='spritepage'><span class='sprit_creditcrd'></span></div><div class='fl'>Credit</span><span class='ft'> : </span></div><div class='ft clr-gr'><span>" + appcurrency + "</span><span> " + (dataSet.Tables[0].Columns.Contains("CREDITBALANCE") ? dataSet.Tables[0].Rows[i]["CREDITBALANCE"] : "0") + "</div></div>|";
                                    }
                                    if (dataSet.Tables[0].Columns.Contains("OUTSTANDINGAMOUNT") && Convert.ToDouble(dataSet.Tables[0].Rows[0]["OUTSTANDINGAMOUNT"]) != 0)
                                    {
                                        Outstanding = Convert.ToInt32(dataSet.Tables[0].Rows[0]["OUTSTANDINGAMOUNT"]);
                                        TotalOutstanding = Topup - Outstanding;
                                        strOutstanding = "<div><div class='spritepage'><span class='sprit_outstand'></span></div><div class='fl'>OutStanding</span><span class='ft'> : </span></div><div class='ft clr-rdp'><span>" + appcurrency + "</span><span> " + TotalOutstanding + "</div></div>|";
                                    }
                                }
                                else
                                {

                                    if (ss[j] == "T")
                                    {
                                        Session["ALLOW_TOPUP"] = "Y";
                                        strTopup = "<div><div class=''><img src='" + strLogoUrl + "/" + strProduct + "/AddMoney.gif'></div><div class='fl' style='line-height:32px;'>Topup</span><span class='ft' style='line-height:32px;'> : </span></div><div class='ft clr-gr' style='line-height:32px;'><span>" + appcurrency + "</span><span>  " + (dataSet.Tables[0].Columns.Contains("CLT_OPENING_BALANCE") ? dataSet.Tables[0].Rows[i]["CLT_OPENING_BALANCE"].ToString() : "0") + "</div></div>|";
                                    }
                                    if (ss[j] == "C")
                                    {
                                        Session["ALLOW_CREDIT"] = "Y";
                                        strCredit = "<div><div class='spritepage'><span class='sprit_creditcrd'></span></div><div class='fl'>Credit</span><span class='ft'> : </span></div><div class='ft clr-gr'><span>" + appcurrency + "</span><span> " + (dataSet.Tables[0].Columns.Contains("CREDITBALANCE") ? dataSet.Tables[0].Rows[i]["CREDITBALANCE"] : "0") + "</div></div>|";
                                    }

                                    if (dataSet.Tables[0].Columns.Contains("OUTSTANDINGAMOUNT") && Convert.ToDouble(dataSet.Tables[0].Rows[0]["OUTSTANDINGAMOUNT"]) != 0)
                                    {
                                        strOutstanding = "<div><div class='spritepage'><span class='sprit_outstand'></span></div><div class='fl'>OutStanding</span><span class='ft'> : </span></div><div class='ft clr-rdp' style='text-decoration: underline;cursor: pointer;' onclick='clickOutstand();'><span>" + appcurrency + "</span><span> " + dataSet.Tables[0].Rows[0]["OUTSTANDINGAMOUNT"].ToString() + "</div></div>|";
                                    }
                                }
                                if (ss[j] == "B")
                                {
                                    Session["ALLOW_PASSTHROUGH"] = "Y";
                                }
                                if (ss[j] == "P")
                                {
                                    Session["ALLOW_PG"] = "Y";
                                }
                            }

                            strBalFetch = strTopup + strCredit + strOutstanding;
                        }
                    }
                    if (strTerminaltype == "T")
                    {
                        if (dataSet != null && dataSet.Tables.Count > 8 && dataSet.Tables[8].Rows.Count > 0)
                        {
                            Array_Book[threadresponse] = JsonConvert.SerializeObject(dataSet.Tables[8]).ToString();
                            if (Session["threadkey"] == null)
                            {
                                Session.Add("threadkey", dataSet.Tables[8].Columns.Contains("Thread") ? dataSet.Tables[8].Rows[0]["Thread"].ToString() : "");
                            }
                            else
                            {
                                Session["threadkey"] = dataSet.Tables[8].Columns.Contains("Thread") ? dataSet.Tables[8].Rows[0]["Thread"].ToString() : "";
                            }
                            if (Session["fscthreadkey"] == null)
                            {
                                Session.Add("fscthreadkey", dataSet.Tables[8].Columns.Contains("FscThread") ? dataSet.Tables[8].Rows[0]["FscThread"].ToString() : "");
                            }
                            else
                            {
                                Session["fscthreadkey"] = dataSet.Tables[8].Columns.Contains("FscThread") ? dataSet.Tables[8].Rows[0]["FscThread"].ToString() : "";
                            }
                            if (Session["LccThread"] == null)
                            {
                                Session.Add("LccThread", dataSet.Tables[8].Columns.Contains("LccThread") ? dataSet.Tables[8].Rows[0]["LccThread"].ToString() : "");
                            }
                            else
                            {
                                Session["LccThread"] = dataSet.Tables[8].Columns.Contains("LccThread") ? dataSet.Tables[8].Rows[0]["LccThread"].ToString() : "";
                            }
                            if (Session["threadkey_dom"] == null)
                            {
                                Session.Add("threadkey_dom", dataSet.Tables[8].Columns.Contains("ThreadD") ? dataSet.Tables[8].Rows[0]["ThreadD"].ToString() : "");
                            }
                            else
                            {
                                Session["threadkey_dom"] = dataSet.Tables[8].Columns.Contains("ThreadD") ? dataSet.Tables[8].Rows[0]["ThreadD"].ToString() : "";
                            }
                            if (Session["fscthreadkey_dom"] == null)
                            {
                                Session.Add("fscthreadkey_dom", dataSet.Tables[8].Columns.Contains("FscThreadD") ? dataSet.Tables[8].Rows[0]["FscThreadD"].ToString() : "");
                            }
                            else
                            {
                                Session["fscthreadkey_dom"] = dataSet.Tables[8].Columns.Contains("FscThreadD") ? dataSet.Tables[8].Rows[0]["FscThreadD"].ToString() : "";
                            }
                            if (Session["lccthreadkey_dom"] == null)
                            {
                                Session.Add("lccthreadkey_dom", dataSet.Tables[8].Columns.Contains("LccThreadD") ? dataSet.Tables[8].Rows[0]["LccThreadD"].ToString() : "");
                            }
                            else
                            {
                                Session["lccthreadkey_dom"] = dataSet.Tables[8].Columns.Contains("LccThreadD") ? dataSet.Tables[8].Rows[0]["LccThreadD"].ToString() : "";
                            }
                            if (Session["CORPORATETHREAD"] == null)
                            {
                                Session.Add("CORPORATETHREAD", dataSet.Tables[8].Columns.Contains("CORPORATETHREAD") ? dataSet.Tables[8].Rows[0]["CORPORATETHREAD"].ToString() : "");
                            }
                            else
                            {
                                Session["CORPORATETHREAD"] = dataSet.Tables[8].Columns.Contains("CORPORATETHREAD") ? dataSet.Tables[8].Rows[0]["CORPORATETHREAD"].ToString() : "";
                            }
                            if (Session["RETAILTHREAD"] == null)
                            {
                                Session.Add("RETAILTHREAD", dataSet.Tables[8].Columns.Contains("RETAILTHREAD") ? dataSet.Tables[8].Rows[0]["RETAILTHREAD"].ToString() : "");
                            }
                            else
                            {
                                Session["RETAILTHREAD"] = dataSet.Tables[8].Columns.Contains("RETAILTHREAD") ? dataSet.Tables[8].Rows[0]["RETAILTHREAD"].ToString() : "";
                            }
                            if (Session["SFCthread_dom"] == null)
                            {
                                Session.Add("SFCthread_dom", dataSet.Tables[8].Columns.Contains("SFCD") ? dataSet.Tables[8].Rows[0]["SFCD"].ToString() : "");
                            }
                            else
                            {
                                Session["SFCthread_dom"] = dataSet.Tables[8].Columns.Contains("SFCD") ? dataSet.Tables[8].Rows[0]["SFCD"].ToString() : "";
                            }
                            if (dataSet.Tables[8].Columns.Contains("TDS_PERCENTAGE") == true)
                            {
                                if (Session["AGN_TDSPERCENTAGE"] == null || Session["AGN_TDSPERCENTAGE"].ToString() == "")
                                {
                                    Session.Add("AGN_TDSPERCENTAGE", (dataSet.Tables[8].Rows[0]["TDS_PERCENTAGE"].ToString()));
                                }
                                else
                                {
                                    Session["AGN_TDSPERCENTAGE"] = dataSet.Tables[8].Columns.Contains("TDS_PERCENTAGE") ? dataSet.Tables[8].Rows[0]["TDS_PERCENTAGE"].ToString() : "";
                                }
                            }


                            /* Agent Commission and markup Rights*/
                            if (Session["Agent_CommissionRef"] == null)
                            {
                                Session.Add("Agent_CommissionRef", dataSet.Tables[8].Columns.Contains("Commission") ? dataSet.Tables[8].Rows[0]["Commission"].ToString() : "0");
                            }
                            else
                            {
                                Session["Agent_CommissionRef"] = dataSet.Tables[8].Columns.Contains("Commission") ? dataSet.Tables[8].Rows[0]["Commission"].ToString() : "0";
                            }
                            if (Session["Agent_MarkupRef"] == null)
                            {
                                Session.Add("Agent_MarkupRef", dataSet.Tables[8].Columns.Contains("Markup") ? dataSet.Tables[8].Rows[0]["Markup"].ToString() : "0");
                            }
                            else
                            {
                                Session["Agent_MarkupRef"] = dataSet.Tables[8].Columns.Contains("Markup") ? dataSet.Tables[8].Rows[0]["Markup"].ToString() : "0";
                            }
                            if (Session["Agent_ServiceTaxRef"] == null)
                            {
                                Session.Add("Agent_ServiceTaxRef", dataSet.Tables[8].Columns.Contains("ServiceTax") ? dataSet.Tables[8].Rows[0]["ServiceTax"].ToString() : "0");
                            }
                            else
                            {
                                Session["Agent_ServiceTaxRef"] = dataSet.Tables[8].Columns.Contains("ServiceTax") ? dataSet.Tables[8].Rows[0]["ServiceTax"].ToString() : "0";
                            }
                            if (Session["Agent_ServiceRef"] == null)
                            {
                                Session.Add("Agent_ServiceRef", dataSet.Tables[8].Columns.Contains("ServiceCharge") ? dataSet.Tables[8].Rows[0]["ServiceCharge"].ToString() : "0");
                            }
                            else
                            {
                                Session["Agent_ServiceRef"] = dataSet.Tables[8].Columns.Contains("ServiceCharge") ? dataSet.Tables[8].Rows[0]["ServiceCharge"].ToString() : "0";
                            }
                            /* Agent Commission and markup Rights*/
                        }
                    }

                    Array_Book[error] = 0;
                    Array_Book[response] = strBalFetch;
                    stu = "01";
                }
                #endregion
            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Problem occured while fetch Agent balance. Please contact support team.";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Availpage", "BalanceCheck", ex.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
            }
            return Json(new { Status = stu, Message = msg, Result = Array_Book });
        }

        public ActionResult BalanceCheckBOA(string AgentId, string strBranchID)
        {
            #region UsageLog
            string PageName = "Balance Check";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
            }
            catch (Exception e)
            {
            }
            #endregion
            string strErrorMsg = string.Empty;
            string strPOSID = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalID = string.Empty;
            string strUserName = string.Empty;
            string strCliendId = string.Empty;

            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            int response = 1;
            int threadresponse = 2;
            string stu = string.Empty;
            string msg = string.Empty;
            try
            {
                strPOSID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
                strAgentID = Session["AGENTID"] != null && Session["AGENTID"].ToString() != "" ? Session["AGENTID"].ToString() : "";
                strTerminalID = Session["TERMINALID"] != null && Session["TERMINALID"].ToString() != "" ? Session["TERMINALID"].ToString() : "";
                strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
                strCliendId = Session["CompanyID"] != null && Session["CompanyID"].ToString() != "" ? Session["CompanyID"].ToString() : "";

                RaysService _rays_servers = new RaysService();
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || strBranchCredit.Contains(strBranchID))
                    {
                        _rays_servers.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                    }
                    else
                    {
                        _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    }
                }
                else
                {
                    _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                }

                string balance = string.Empty;
                DataSet dataSet = new DataSet();
                string strBalFetch = "";
                string topupAmount = string.Empty, creditAmount = string.Empty;
                int Topup = 0; int Outstanding = 0; int TotalOutstanding = 0;
                #region Topup Balance Amount check

                string agentid = (AgentId != null && AgentId != "") ? AgentId : Session["POS_ID"].ToString();

                dataSet = _rays_servers.Fetch_Client_Credit_Balance_DetailsWEB(agentid, Session["username"].ToString(), Session["ipAddress"].ToString(), Session["sequenceid"].ToString(), strPlatform);
                StringWriter strREQUEST = new StringWriter();

                if (dataSet != null)
                    dataSet.WriteXml(strREQUEST);

                DatabaseLog.LogData(Session["username"].ToString(), "T", "Availpage", "BalanceCheck", strREQUEST.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
                if (dataSet != null && dataSet.Tables.Count != 0 && dataSet.Tables[0].Rows.Count != 0)
                {
                    Session["CLIENT_TYPE"] = (dataSet.Tables[0].Columns.Contains("CLT_CORP_TYPE") && dataSet.Tables[0].Rows[0]["CLT_CORP_TYPE"] != null) ? dataSet.Tables[0].Rows[0]["CLT_CORP_TYPE"].ToString().Trim() : "";
                    Session["Client_AgentId"] = AgentId != null && AgentId != "" ? AgentId : Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        string ccur = dataSet.Tables[0].Columns.Contains("CLT_ALLOW_ACC_TYPE") ? dataSet.Tables[0].Rows[i]["CLT_ALLOW_ACC_TYPE"].ToString().Trim() : "";
                        string appcurrency = dataSet.Tables[0].Columns.Contains("CCD_CURRENCY_CODE") ? dataSet.Tables[0].Rows[i]["CCD_CURRENCY_CODE"].ToString().Trim() : "";
                        string[] ss = ccur.Split('|');
                        if (ss.Length > 0)
                        {
                            string strTopup = string.Empty; string strCredit = string.Empty; string strOutstanding = string.Empty;
                            for (int j = 0; j < ss.Length; j++)
                            {
                                if (strProduct == "UAE")
                                {
                                    if (ss[j] == "T")
                                    {
                                        Session["ALLOW_TOPUP"] = "Y";
                                        Topup = Convert.ToInt32((dataSet.Tables[0].Columns.Contains("CLT_OPENING_BALANCE") ? dataSet.Tables[0].Rows[i]["CLT_OPENING_BALANCE"] : "0"));
                                        strTopup = "<div><div class=''><img src='" + strLogoUrl + "/" + strProduct + "/AddMoney.gif'></div><div class='fl' style='line-height:32px;'>Active Balance</span><span class='ft' style='line-height:32px;'> : </span></div><div class='ft clr-gr' style='line-height:32px;'><span>" + appcurrency + "</span><span>  " + Topup + "</div></div>|";
                                    }
                                    if (ss[j] == "C")
                                    {
                                        Session["ALLOW_CREDIT"] = "Y";
                                        strCredit = "<div><div class='spritepage'><span class='sprit_creditcrd'></span></div><div class='fl'>Credit</span><span class='ft'> : </span></div><div class='ft clr-gr'><span>" + appcurrency + "</span><span> " + (dataSet.Tables[0].Columns.Contains("CREDITBALANCE") ? dataSet.Tables[0].Rows[i]["CREDITBALANCE"] : "0") + "</div></div>|";
                                    }
                                    if (dataSet.Tables[0].Columns.Contains("OUTSTANDINGAMOUNT") && Convert.ToDouble(dataSet.Tables[0].Rows[0]["OUTSTANDINGAMOUNT"]) != 0)
                                    {
                                        Outstanding = Convert.ToInt32(dataSet.Tables[0].Rows[0]["OUTSTANDINGAMOUNT"]);
                                        TotalOutstanding = Topup - Outstanding;
                                        strOutstanding = "<div><div class='spritepage'><span class='sprit_outstand'></span></div><div class='fl'>OutStanding</span><span class='ft'> : </span></div><div class='ft clr-rdp'><span>" + appcurrency + "</span><span> " + TotalOutstanding + "</div></div>|";
                                    }
                                }
                                else
                                {

                                    if (ss[j] == "T")
                                    {
                                        Session["ALLOW_TOPUP"] = "Y";
                                        strTopup = "<div><div class=''><img src='" + strLogoUrl + "/" + strProduct + "/AddMoney.gif'></div><div class='fl' style='line-height:32px;'>Topup</span><span class='ft' style='line-height:32px;'> : </span></div><div class='ft clr-gr' style='line-height:32px;'><span>" + appcurrency + "</span><span>  " + (dataSet.Tables[0].Columns.Contains("CLT_OPENING_BALANCE") ? dataSet.Tables[0].Rows[i]["CLT_OPENING_BALANCE"].ToString() : "0") + "</div></div>|";
                                    }
                                    if (ss[j] == "C")
                                    {
                                        Session["ALLOW_CREDIT"] = "Y";
                                        strCredit = "<div><div class='spritepage'><span class='sprit_creditcrd'></span></div><div class='fl'>Credit</span><span class='ft'> : </span></div><div class='ft clr-gr'><span>" + appcurrency + "</span><span> " + (dataSet.Tables[0].Columns.Contains("CREDITBALANCE") ? dataSet.Tables[0].Rows[i]["CREDITBALANCE"] : "0") + "</div></div>|";
                                    }

                                    if (dataSet.Tables[0].Columns.Contains("OUTSTANDINGAMOUNT") && Convert.ToDouble(dataSet.Tables[0].Rows[0]["OUTSTANDINGAMOUNT"]) != 0)
                                    {
                                        strOutstanding = "<div><div class='spritepage'><span class='sprit_outstand'></span></div><div class='fl'>OutStanding</span><span class='ft'> : </span></div><div class='ft clr-rdp' style='text-decoration: underline;cursor: pointer;' onclick='clickOutstand();'><span>" + appcurrency + "</span><span> " + dataSet.Tables[0].Rows[0]["OUTSTANDINGAMOUNT"].ToString() + "</div></div>|";
                                    }
                                }
                                if (ss[j] == "B")
                                {
                                    Session["ALLOW_PASSTHROUGH"] = "Y";
                                }
                                if (ss[j] == "P")
                                {
                                    Session["ALLOW_PG"] = "Y";
                                }
                            }

                            strBalFetch = strTopup + strCredit + strOutstanding;
                        }
                    }
                    if (ConfigurationManager.AppSettings["TerminalType"].ToString() == "T")
                    {
                        if (dataSet != null && dataSet.Tables.Count > 8 && dataSet.Tables[8].Rows.Count > 0)
                        {
                            Array_Book[threadresponse] = JsonConvert.SerializeObject(dataSet.Tables[8]).ToString();
                            if (Session["threadkey"] == null)
                            {
                                Session.Add("threadkey", dataSet.Tables[8].Columns.Contains("Thread") ? dataSet.Tables[8].Rows[0]["Thread"].ToString() : "");
                            }
                            else
                            {
                                Session["threadkey"] = dataSet.Tables[8].Columns.Contains("Thread") ? dataSet.Tables[8].Rows[0]["Thread"].ToString() : "";
                            }
                            if (Session["fscthreadkey"] == null)
                            {
                                Session.Add("fscthreadkey", dataSet.Tables[8].Columns.Contains("FscThread") ? dataSet.Tables[8].Rows[0]["FscThread"].ToString() : "");
                            }
                            else
                            {
                                Session["fscthreadkey"] = dataSet.Tables[8].Columns.Contains("FscThread") ? dataSet.Tables[8].Rows[0]["FscThread"].ToString() : "";
                            }
                            if (Session["LccThread"] == null)
                            {
                                Session.Add("LccThread", dataSet.Tables[8].Columns.Contains("LccThread") ? dataSet.Tables[8].Rows[0]["LccThread"].ToString() : "");
                            }
                            else
                            {
                                Session["LccThread"] = dataSet.Tables[8].Columns.Contains("LccThread") ? dataSet.Tables[8].Rows[0]["LccThread"].ToString() : "";
                            }
                            if (Session["threadkey_dom"] == null)
                            {
                                Session.Add("threadkey_dom", dataSet.Tables[8].Columns.Contains("ThreadD") ? dataSet.Tables[8].Rows[0]["ThreadD"].ToString() : "");
                            }
                            else
                            {
                                Session["threadkey_dom"] = dataSet.Tables[8].Columns.Contains("ThreadD") ? dataSet.Tables[8].Rows[0]["ThreadD"].ToString() : "";
                            }
                            if (Session["fscthreadkey_dom"] == null)
                            {
                                Session.Add("fscthreadkey_dom", dataSet.Tables[8].Columns.Contains("FscThreadD") ? dataSet.Tables[8].Rows[0]["FscThreadD"].ToString() : "");
                            }
                            else
                            {
                                Session["fscthreadkey_dom"] = dataSet.Tables[8].Columns.Contains("FscThreadD") ? dataSet.Tables[8].Rows[0]["FscThreadD"].ToString() : "";
                            }
                            if (Session["lccthreadkey_dom"] == null)
                            {
                                Session.Add("lccthreadkey_dom", dataSet.Tables[8].Columns.Contains("LccThreadD") ? dataSet.Tables[8].Rows[0]["LccThreadD"].ToString() : "");
                            }
                            else
                            {
                                Session["lccthreadkey_dom"] = dataSet.Tables[8].Columns.Contains("LccThreadD") ? dataSet.Tables[8].Rows[0]["LccThreadD"].ToString() : "";
                            }
                            if (Session["CORPORATETHREAD"] == null)
                            {
                                Session.Add("CORPORATETHREAD", dataSet.Tables[8].Columns.Contains("CORPORATETHREAD") ? dataSet.Tables[8].Rows[0]["CORPORATETHREAD"].ToString() : "");
                            }
                            else
                            {
                                Session["CORPORATETHREAD"] = dataSet.Tables[8].Columns.Contains("CORPORATETHREAD") ? dataSet.Tables[8].Rows[0]["CORPORATETHREAD"].ToString() : "";
                            }
                            if (Session["RETAILTHREAD"] == null)
                            {
                                Session.Add("RETAILTHREAD", dataSet.Tables[8].Columns.Contains("RETAILTHREAD") ? dataSet.Tables[8].Rows[0]["RETAILTHREAD"].ToString() : "");
                            }
                            else
                            {
                                Session["RETAILTHREAD"] = dataSet.Tables[8].Columns.Contains("RETAILTHREAD") ? dataSet.Tables[8].Rows[0]["RETAILTHREAD"].ToString() : "";
                            }
                            if (Session["SFCthread_dom"] == null)
                            {
                                Session.Add("SFCthread_dom", dataSet.Tables[8].Columns.Contains("SFCD") ? dataSet.Tables[8].Rows[0]["SFCD"].ToString() : "");
                            }
                            else
                            {
                                Session["SFCthread_dom"] = dataSet.Tables[8].Columns.Contains("SFCD") ? dataSet.Tables[8].Rows[0]["SFCD"].ToString() : "";
                            }
                            if (dataSet.Tables[8].Columns.Contains("TDS_PERCENTAGE") == true)
                            {
                                if (Session["AGN_TDSPERCENTAGE"] == null || Session["AGN_TDSPERCENTAGE"].ToString() == "")
                                {
                                    Session.Add("AGN_TDSPERCENTAGE", (dataSet.Tables[8].Rows[0]["TDS_PERCENTAGE"].ToString()));
                                }
                                else
                                {
                                    Session["AGN_TDSPERCENTAGE"] = dataSet.Tables[8].Columns.Contains("TDS_PERCENTAGE") ? dataSet.Tables[8].Rows[0]["TDS_PERCENTAGE"].ToString() : "";
                                }
                            }


                            /* Agent Commission and markup Rights*/
                            if (Session["Agent_CommissionRef"] == null)
                            {
                                Session.Add("Agent_CommissionRef", dataSet.Tables[8].Columns.Contains("Commission") ? dataSet.Tables[8].Rows[0]["Commission"].ToString() : "0");
                            }
                            else
                            {
                                Session["Agent_CommissionRef"] = dataSet.Tables[8].Columns.Contains("Commission") ? dataSet.Tables[8].Rows[0]["Commission"].ToString() : "0";
                            }
                            if (Session["Agent_MarkupRef"] == null)
                            {
                                Session.Add("Agent_MarkupRef", dataSet.Tables[8].Columns.Contains("Markup") ? dataSet.Tables[8].Rows[0]["Markup"].ToString() : "0");
                            }
                            else
                            {
                                Session["Agent_MarkupRef"] = dataSet.Tables[8].Columns.Contains("Markup") ? dataSet.Tables[8].Rows[0]["Markup"].ToString() : "0";
                            }
                            if (Session["Agent_ServiceTaxRef"] == null)
                            {
                                Session.Add("Agent_ServiceTaxRef", dataSet.Tables[8].Columns.Contains("ServiceTax") ? dataSet.Tables[8].Rows[0]["ServiceTax"].ToString() : "0");
                            }
                            else
                            {
                                Session["Agent_ServiceTaxRef"] = dataSet.Tables[8].Columns.Contains("ServiceTax") ? dataSet.Tables[8].Rows[0]["ServiceTax"].ToString() : "0";
                            }
                            if (Session["Agent_ServiceRef"] == null)
                            {
                                Session.Add("Agent_ServiceRef", dataSet.Tables[8].Columns.Contains("ServiceCharge") ? dataSet.Tables[8].Rows[0]["ServiceCharge"].ToString() : "0");
                            }
                            else
                            {
                                Session["Agent_ServiceRef"] = dataSet.Tables[8].Columns.Contains("ServiceCharge") ? dataSet.Tables[8].Rows[0]["ServiceCharge"].ToString() : "0";
                            }
                            /* Agent Commission and markup Rights*/
                        }
                    }

                    Array_Book[error] = 0;
                    Array_Book[response] = strBalFetch;
                    stu = "01";
                }
                #endregion
            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Problem occured while fetch Agent balance. Please contact support team.";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Availpage", "BalanceCheck", ex.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
            }
            return Json(new { Status = stu, Message = msg, Result = Array_Book });
        }

        #endregion

        #region Agent support//--? new to verify whether used
        public ActionResult callback(string PhoneNocallback, string Emailidcallback, string TypeofIssue, string FacingFrom, string PrefferdTimeCBR, string IssueDesc, string RemarkscallbackReq, string lstragentmnmo, string Product, string Riyapnr)
        {

            string strResult = string.Empty;
            string departureDate = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            int response = 1;
            string Output = string.Empty;
            string AgentID = string.Empty;
            string TerminalID = string.Empty;
            string UserName = string.Empty;
            string strErrorMsg = string.Empty;
            string strRefno = string.Empty;
            string strBranch = string.Empty;

            string stu = string.Empty;
            string msg = string.Empty;
            string strAgentMob = string.Empty;
            string strAgentEmail = string.Empty;

            try
            {
                AgentID = Session["POS_ID"].ToString();
                TerminalID = Session["POS_TID"].ToString();
                UserName = Session["username"].ToString();
                Ipaddress = Session["ipAddress"].ToString();
                sequnceID = Session["sequenceid"].ToString();
                terminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                strBranch = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                strAgentMob = (PhoneNocallback != null && PhoneNocallback != "") ? PhoneNocallback : Session["agencyaddress"].ToString().Split('~')[4];
                strAgentEmail = (Emailidcallback != null && Emailidcallback != "") ? Emailidcallback : Session["agencyaddress"].ToString().Split('~')[2];
                PrefferdTimeCBR = (PrefferdTimeCBR != null && PrefferdTimeCBR != "") ? PrefferdTimeCBR : "00:00-24:00";

                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                string fromDate = (FacingFrom != "") ? DateTime.ParseExact(FacingFrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm") : DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                string XML = "<EVENT><AGENTID>" + Session["agentid"].ToString() + "</AGENTID><TERMINALID>" + Session["terminalid"].ToString() + "</TERMINALID><USERNAME>" + Session["username"].ToString() + "</USERNAME><MOBNO>" +
                    lstragentmnmo + "</MOBNO><TERMINALMNO>" + strAgentMob + " </TERMINALMNO><EMAILID>" + strAgentEmail + " </EMAILID><TYPE>" + TypeofIssue + " </TYPE><FDATE>" + fromDate + " </FDATE><PREFERTIME>" +
                    PrefferdTimeCBR + "</PREFERTIME><DESC>" + IssueDesc + " </DESC><REMARKS>" + RemarkscallbackReq + "</REMARKS></EVENT>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "Fetchallagentdetails", "Fetchallagent Details", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                bool result = _rays_servers.MUA_updateagentstatus("", RemarkscallbackReq, "", AgentID, TerminalID, strBranch, terminalType, TypeofIssue, Product, Riyapnr, IssueDesc, PrefferdTimeCBR, fromDate,
                                                    UserName, strAgentEmail, strAgentMob, "I", UserName, Ipaddress, sequnceID, ref strErrorMsg, ref strRefno, "HomeController", "callback", "", "", "");

                if (result == true)
                {
                    stu = "01";
                    Array_Book[response] = strRefno;
                }
                else
                {
                    stu = "00";
                    msg = (strErrorMsg != "") ? strErrorMsg : "Unable to send your request, please contact customer care(#01)";
                    Array_Book[error] = msg;
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Problem occurred while send your request, please contact customer care.";
                Array_Book[error] = msg;
                DatabaseLog.LogData(Session["username"].ToString(), "X", "insert_callback ", "callback_request_webhome", ex.Message.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            //return Array_Book;
            return Json(new { Status = stu, Message = msg, Result = Array_Book });
        }
        #endregion

        public DataSet fetch_vat_registration(string agentid)
        {
            InplantService.Inplantservice _inplantservice = new InplantService.Inplantservice();
            DataSet dst = new DataSet();
            string Errormsg = string.Empty;
            try
            {
                Hashtable hst = new Hashtable();
                hst.Add("AGENTID", agentid);
                hst.Add("COMPANY_NAME", "");
                hst.Add("FULL_NAME", "");
                hst.Add("CONTACT_NO", "");
                hst.Add("EMAILID", "");
                hst.Add("REG_NO", "");
                hst.Add("INVOICE_PAYMENT", "");
                hst.Add("REMARKS", "");
                hst.Add("CREATEDBY", "");
                hst.Add("FLAG", "S");
                // dst = DBHandler.ExecProcedureReturnsDataset("P_INSERT_VAT_MASTERDETAILS", hst);
                _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                dst = _inplantservice.Insert_vat_registration("", agentid, "", "", "", "", "", "", "", Session["username"].ToString(), Session["POS_TID"].ToString(), Session["ipaddress"].ToString(), Session["sequenceid"].ToString(), ref Errormsg);

            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "vat_registration ", "vat_registration", ex.Message.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            return dst;
        }

        public ActionResult ExportDatabookedhistory_sum()
        {

            ArrayList encrypt = new ArrayList();
            encrypt.Add("");
            int status = 0;
            DateTime dt = DateTime.Now;
            string toDate = dt.ToString("yyyy-MM-ddTHH:mm");//yyyy-MM-ddTHH:mm:sszzz
            DataSet ds = new DataSet();
            ds = (DataSet)Session["Airline_bookedhistorydsBookedHistory"];
            GridView gv = new GridView();
            gv.DataSource = ds.Tables[0];
            gv.DataBind();
            // string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            // TableCell cell = new TableCell();

            Response.ClearContent();
            Response.Buffer = true;
            //if (field.Status != null && field.Status == "SavingsReport")
            //{
            //    Response.AddHeader("content-disposition", "attachment; filename=SavingsReport_" + toDate + ".xls");
            //}
            //else
            //{
            //    Response.AddHeader("content-disposition", "attachment; filename=MISReport_" + toDate + ".xls");
            //}
            Response.AddHeader("content-disposition", "attachment; filename=BookedHistory_" + toDate + ".xls");
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
            //return PartialView("Address_Book_Partial_View_FileName_Here_Without_Extension");
            //return View(); //Json(new { Status = "", Message = "", Result = encrypt });
            return RedirectToAction("Temp");
        }

        public ActionResult GetClientData()
        {
            DataSet Ds_set = new DataSet();
            string results = "";
            string POS_ID = "";
            string POS_TID = "";
            string Username = "";
            string AgentID = "";
            string strRef = "";
            string BranchDet = "";
            string Coordinatordetails = "";
            string stu = "";
            string error = "";
            string result = "";
            string ipAddress = "";
            string productlst = "";
            string password = string.Empty;
            byte[] dsclien_compress = new byte[] { };
            try
            {
                if (Session["POS_TID"] == null)
                {
                    return Json(new { Status = "-1", Error = error, Result = result, BARRES = BranchDet, CoordinatorDet = Coordinatordetails });
                }

                POS_ID = System.Web.HttpContext.Current.Session["POS_ID"] != null ? System.Web.HttpContext.Current.Session["POS_ID"].ToString() : "";
                string BranchId = Session["branchid"].ToString();
                POS_TID = System.Web.HttpContext.Current.Session["POS_TID"] != null ? System.Web.HttpContext.Current.Session["POS_TID"].ToString() : "";
                Username = System.Web.HttpContext.Current.Session["UserName"] != null ? System.Web.HttpContext.Current.Session["UserName"].ToString() : "";
                AgentID = "";
                ipAddress = System.Web.HttpContext.Current.Session["ipAddress"] == null ? "" : System.Web.HttpContext.Current.Session["ipAddress"].ToString();
                password = System.Web.HttpContext.Current.Session["Password"] != null ? System.Web.HttpContext.Current.Session["Password"].ToString() : "";
                InplantService.Inplantservice _inplantservice = new InplantService.Inplantservice();

                _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                dsclien_compress = _inplantservice.Fetch_All_Agent_Branch_DetailsByte(ipAddress, Username, password, POS_TID, ref strRef, strPlatform);

                if (dsclien_compress != null)
                {
                    Ds_set = Base.Decompress(dsclien_compress);
                }

                if (Ds_set != null && Ds_set.Tables[0].Rows.Count > 0)
                {

                    var Clients = (from Key in Ds_set.Tables["MasterClientDetials"].AsEnumerable()
                                   orderby Key["CLT_CLIENT_NAME"] ascending
                                   select new
                                   {
                                       CLTNAME = (Key["CLT_CLIENT_NAME"]) + "~~" + Key["CLT_CLIENT_ID"],
                                       CLTCODE = Key["CLT_CLIENT_ID"],
                                       BID = Key["CLT_BRANCH_ID"],
                                       BNAME = Key["BRH_BRANCH_NAME"],
                                   }).Distinct();

                    if (Ds_set.Tables.Contains("P_FETCH_CORPORATE_IMPLANT2") && Ds_set.Tables["P_FETCH_CORPORATE_IMPLANT2"].Columns.Contains("PRD_PRODUCT_NAME"))
                    {
                        var Product = (from p in Ds_set.Tables["P_FETCH_CORPORATE_IMPLANT2"].AsEnumerable()
                                       orderby p["PRD_PRODUCT_NAME"] ascending
                                       select new
                                       {
                                           PNAME = p["PRD_PRODUCT_NAME"],
                                           PID = p["PRD_PRODUCT_ID"],
                                       }).Distinct();

                        if (Product.Count() > 0)
                        {
                            productlst = JsonConvert.SerializeObject(Product).ToString();
                        }
                    }

                    if (Clients.Count() > 0)
                    {
                        stu = "01";
                        error = "";
                        result = JsonConvert.SerializeObject(Clients).ToString();
                    }
                }
                else
                {
                    stu = "00";
                    error = "";
                    result = "No Client Names found.";
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                error = ex.ToString();
                result = "Unable to fetch agent details (#05).";
            }
            return new JsonResult()
            {
                Data = Json(new { Status = stu, Error = error, Result = result, BARRES = BranchDet, CoordinatorDet = Coordinatordetails, ProductList = productlst }),//result,
                MaxJsonLength = 2147483647
            };
            //return Json(new { Status = stu, Error = error, Result = result, BARRES = BranchDet, CoordinatorDet = Coordinatordetails }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClientdetails()
        {
            DataSet Ds_set = new DataSet();
            string results = "";
            string POS_ID = "";
            string POS_TID = "";
            string Username = "";
            string AgentID = "";
            string strRef = "";
            string BranchDet = "";
            string Coordinatordetails = "";
            string stu = "";
            string error = "";
            string result = "";
            string ipAddress = "";
            string password = string.Empty;
            byte[] dsclien_compress = new byte[] { };
            try
            {
                if (Session["POS_TID"] == null)
                {
                    return Json(new { Status = "-1", Error = error, Result = result, BARRES = BranchDet, CoordinatorDet = Coordinatordetails });
                }

                POS_ID = System.Web.HttpContext.Current.Session["POS_ID"] != null ? System.Web.HttpContext.Current.Session["POS_ID"].ToString() : "";
                string BranchId = Session["branchid"].ToString();
                POS_TID = System.Web.HttpContext.Current.Session["POS_TID"] != null ? System.Web.HttpContext.Current.Session["POS_TID"].ToString() : "";
                Username = System.Web.HttpContext.Current.Session["UserName"] != null ? System.Web.HttpContext.Current.Session["UserName"].ToString() : "";
                AgentID = "";
                ipAddress = System.Web.HttpContext.Current.Session["ipAddress"] == null ? "" : System.Web.HttpContext.Current.Session["ipAddress"].ToString();
                password = System.Web.HttpContext.Current.Session["Password"] != null ? System.Web.HttpContext.Current.Session["Password"].ToString() : "";
                InplantService.Inplantservice _inplantservice = new InplantService.Inplantservice();

                _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                dsclien_compress = _inplantservice.Fetch_All_Agent_Branch_DetailsByte(ipAddress, Username, password, POS_TID, ref strRef, strPlatform);

                if (dsclien_compress != null)
                {
                    Ds_set = Base.Decompress(dsclien_compress);
                }

                if (Ds_set != null && Ds_set.Tables[0].Rows.Count > 0)
                {

                    var Clients = (from Key in Ds_set.Tables["MasterClientDetials"].AsEnumerable()
                                   where ((Ds_set.Tables["MasterClientDetials"].Columns.Contains("MODULERIGHTS") && strProduct == "RBOA") ? Key["MODULERIGHTS"].ToString().Contains("B") : Key["CLT_CLIENT_ID"] != "")
                                   orderby Key["CLT_CLIENT_NAME"] ascending
                                   select new
                                   {
                                       CLTNAME = ((Key["CLT_WINYATRA_ID"] != null && Key["CLT_WINYATRA_ID"].ToString().Trim() != "") ? Key["CLT_CLIENT_NAME"] : Key["CLT_CLIENT_NAME"]) + "~~" + Key["CLT_CLIENT_ID"],//+ "-(" + Key["CLT_WINYATRA_ID"] + ")"
                                       CLTCODE = Key["CLT_CLIENT_ID"],
                                       BID = Key["CLT_BRANCH_ID"],
                                       BNAME = Key["BRH_BRANCH_NAME"],
                                       CNT = Key["CLT_CLIENT_TERMINAL_COUNT"],
                                       TML = Ds_set.Tables["MasterClientDetials"].Columns.Contains("Terminalaccess") ? Key["Terminalaccess"].ToString() : "ALL"//terminal access -- TMLACCESS
                                   }).Distinct();

                    if (Clients.Count() > 0)
                    {
                        stu = "01";
                        error = "";
                        result = JsonConvert.SerializeObject(Clients).ToString();
                        // Session.Add("Subagent", results); // REMOVED#STS105
                    }
                }
                else
                {
                    stu = "00";
                    error = "";
                    result = "No Client Names found.";
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                error = ex.ToString();
                result = "Unable to fetch agent details (#05).";
            }
            return new JsonResult()
            {
                Data = Json(new { Status = stu, Error = error, Result = result, BARRES = BranchDet, CoordinatorDet = Coordinatordetails }),//result,
                MaxJsonLength = 2147483647
            };
        }

        public ActionResult GetDashboardData(string frmDt, string toDt, string Branchid, string flag, string ClientID, string Loginname, string Sector, string Flowtype)
        {
            string StrInput = string.Empty;
            string strResponse = string.Empty;

            string stu = string.Empty;
            string msg = string.Empty;
            string res = string.Empty;
            string username = Session["UserName"] != null ? Session["UserName"].ToString() : "";
            string UserID = Session["UserID"] != null ? Session["UserID"].ToString() : "";
            string CompanyID = Session["CompanyID"] != null ? Session["CompanyID"].ToString() : "";
            string strIPAddress = ControllerContext.HttpContext.Request.UserHostAddress;
            string xmldata = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(frmDt) || !string.IsNullOrEmpty(toDt))
                {
                    frmDt = DateTime.ParseExact(frmDt, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    toDt = DateTime.ParseExact(toDt, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                }

                ClientID = ClientID != null && ClientID != "" ? ClientID : CompanyID;
                Loginname = Loginname != null && Loginname != "" ? Loginname : username;

                DataSet dsData = new DataSet();
                STSTRAVRAYS.Rays_service.RaysService _rays = new STSTRAVRAYS.Rays_service.RaysService();
                _rays.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                xmldata = "<EVENT><REQUEST>Login</REQUEST>" +
                              "<Fromdate>" + frmDt + "</Fromdate>" +
                              "<Todate>" + toDt + "</Todate>" +
                               "<BranchID>" + Branchid + "</BranchID>" +
                                 "<ClientID>" + ClientID + "</ClientID>" +
                                  "<Loginname>" + Loginname + "</Loginname>" +
                                   "<Sector>" + Sector + "</Sector>" +
                                   "<Flowtype>" + Flowtype + "</Flowtype>" +
                              "<APPTYPE>B2B</APPTYPE>" +
                             "</EVENT>";
                DatabaseLog.LogData(username, "E", "HomeController.cs", "GetDashboardData", xmldata.ToString(), UserID, CompanyID, "");
                dsData = _rays.P_FETCH_SMARTVIEW_MASTER(frmDt, toDt, Branchid, flag, ClientID, Loginname, Sector, Flowtype, username);
                if (dsData != null && dsData.Tables.Count > 0)
                {
                    stu = "01";
                    res = JsonConvert.SerializeObject(dsData);
                    //res = System.IO.File.ReadAllText(Server.MapPath("~/XML/Sampledet.txt"));
                    DatabaseLog.LogData(username, "E", "HomeController.cs", "GetDashboardData", res.ToString(), UserID, CompanyID, "");
                }
                else
                {
                    stu = "00";
                    msg = "Problem occured while fetch records. Please contact support team (#01).";
                    DatabaseLog.LogData(username, "E", "HomeController.cs", "GetDashboardData", msg, UserID, CompanyID, "");
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Problem occured while fetch records. Please contact support team (#05).";
                DatabaseLog.LogData(username, "X", "HomeController.cs", "GetDashboardData", ex.ToString(), UserID, CompanyID, "");
            }
            //return Json(new { status = stu, message = msg, results = res });
            return new JsonResult()
            {
                Data = Json(new { Status = stu, Error = msg, results = res }),
                MaxJsonLength = 2147483647
            };
        }

        public ActionResult Getcalendarbookedcount(string LoginID, string Fromdate, string Todate, string Approvercountflag)
        {
            ArrayList Arrbookedcount = new ArrayList();
            Arrbookedcount.Add("");
            Arrbookedcount.Add("");
            int result = 1;
            int error = 0;
            DataTable dtLogdetails = new DataTable();
            string xmldata = string.Empty;
            string CompanyID = string.Empty;
            string UserID = string.Empty;
            string UserName = string.Empty;
            string IPaddress = string.Empty;
            string SequenceID = "0";
            string srcbtsuperuser = string.Empty;
            string TotalOutput = string.Empty;
            string status = "";
            string errormsg = "";
            try
            {

                if (Session["POS_TID"] == null)
                {
                    return Json(new { status = "03", Errormsg = "Your session has been expired.", DSBDetails = "" });
                }

                string strsuperuser = string.Empty;
                string strflag = string.Empty;
                string Companyname = string.Empty;
                string Bookedcount = string.Empty;
                string usertype = "";

                UserName = Session["UserName"] != null ? Session["UserName"].ToString() : "";
                UserID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                CompanyID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strflag = "TDK";
                IPaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";

                string fromDtStr = DateTime.ParseExact(Fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                string toDtStr = DateTime.ParseExact(Todate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");

                DataSet dsprofile = new DataSet();
                InplantService.Inplantservice _inplant = new InplantService.Inplantservice();
                _inplant.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();

                #region RequestLog
                string ReqTime = "DashBoradcalendarReqTime:" + DateTime.Now;

                xmldata = "<EVENT><REQUEST>GETCALENDARBOOKEDCOUNT</REQUEST>" +
                                "<USERID>" + UserID + "</USERID>" +
                                "<TODATE>" + toDtStr + "</TODATE>" +
                                "<FLAG>" + strflag + "</FLAG>" +
                                "<FROMDATE>" + fromDtStr + "</FROMDATE>" +
                                "<COMPANYID>" + CompanyID + "</COMPANYID>" +
                                "<APPTYPE>B2B</APPTYPE>" +
                               "</EVENT>";
                string LstrDetails = "<GETCALENDARBOOKEDCOUNT_REQUEST><QUERY>" + xmldata + "</QUERY><REQTIME>" + ReqTime + "</REQTIME><APPTYPE>B2B</APPTYPE></GETCALENDARBOOKEDCOUNT_REQUEST>";

                DatabaseLog.LogData(Session["username"] != null ? Session["username"].ToString() : "", "T", "HOMECONTROLLER", "DashBoradcalendarReq", LstrDetails, CompanyID.ToString(), UserID.ToString(), SequenceID);
                #endregion

                //_inplant.
                dsprofile = _inplant.DashBoradDetails(UserID.ToString().ToUpper().Trim(), fromDtStr, toDtStr, strflag, CompanyID, usertype, Approvercountflag, IPaddress);

                //  var groupedresult = from CountDetails in  dsprofile.
                if (dsprofile != null && dsprofile.Tables.Count > 0)
                {
                    TotalOutput = JsonConvert.SerializeObject(dsprofile);
                    status = "01";
                    errormsg = "";
                }
                else
                {
                    status = "00";
                    errormsg = "No Records Found (#03)";
                }


                StringWriter strWriter = new StringWriter();
                dsprofile.WriteXml(strWriter);

                #region Response Log
                LstrDetails = string.Empty;
                string ResTime = "DashBoradcalendarResTime" + DateTime.Now;

                xmldata = "<EVENT><RESPONSE>GETCALENDARBOOKEDCOUNT</RESPONSE>" +
                                "<RESPONSEDATA>" + strWriter + "</RESPONSEDATA>" +
                               "</EVENT>";
                LstrDetails = "<GETCALENDARBOOKEDCOUNT_RESPONSE><QUERY>" + xmldata + "</QUERY><RESTIME>" + ResTime + "</RESTIME><APPTYPE>B2B</APPTYPE></GETCALENDARBOOKEDCOUNT_RESPONSE>";

                DatabaseLog.LogData(Session["username"] != null ? Session["username"].ToString() : "", "T", "HOMECONTROLLER", "DashBoradcalendarRes", LstrDetails, Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "", Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", SequenceID);
                #endregion
            }
            catch (Exception ex)
            {
                string errormgs = string.Empty;
                if (ex.Message.ToString() == "The operation has timed out")
                    errormgs = ex.Message.ToString();
                else if (ex.Message.ToString() == "Unable to connect to the remote server")
                    errormgs = "unable to connect the remote server.";
                else
                    errormgs = "Problem occured While Getcalendarbookedcount.Please contact support team (#05).";
                Arrbookedcount[error] = errormgs.ToString();
                xmldata = "<ERROR>" + ex.Message.ToString() + "</ERROR>";
                DatabaseLog.LogData(Session["username"] != null ? Session["username"].ToString() : "", "x", "HomeController", "Getcalendarbookedcount", ex.ToString(), UserID.ToString(), CompanyID.ToString(), SequenceID);
                return Json(new { status = "00", Errormsg = "Problem occured while loading calendar events.", DSBDetails = "" });
            }
            return Json(new { status = "01", Errormsg = errormsg, DSBDetails = TotalOutput });
        }

        public ActionResult Getcalendarbookedflightdetails(string BookedDate, string Producttype)
        {
            string strbookeddt = string.Empty;
            DataTable dtLogdetails = new DataTable();
            string xmldata = string.Empty;
            string CompanyID = string.Empty;
            string UserID = string.Empty;
            string UserName = string.Empty;
            string IPaddress = string.Empty;
            string SequenceID = "0";
            try
            {
                if (Session["POS_TID"] == null)
                {
                    return Json(new { status = "00", Errormsg = "Session", DSBDetails = "" });
                }
                dtLogdetails = (DataTable)Session["SLoginDetails"];

                string strsuperuser = string.Empty;
                string strflag = string.Empty;
                string Companyname = string.Empty;
                string Bookedcount = string.Empty;
                string Apptype = "TDK";

                UserName = Session["UserName"] != null ? Session["UserName"].ToString() : ""; //dtLogdetails.Rows[0]["TDK_NAME"].ToString().ToUpper().Trim();
                UserID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                strflag = "";
                IPaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                //CompanyID = Session["POS_ID"] != null  ? Session["POS_ID"].ToString() : "";
                DataSet dsbookflights = new DataSet();
                InplantService.Inplantservice _inplant = new InplantService.Inplantservice();
                _inplant.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();

                #region RequestLog

                string strlog_details = string.Empty;
                string ReqTime = "GetBookedFlightREQTime:" + DateTime.Now;
                //strlog_details = UserID.ToString() + '|' + UserID.ToString() + '|' + toDtStr + '|' + strflag + '|' + CompanyID + '|' + Convert.ToDecimal(0);
                xmldata = "<EVENT><REQUEST>Getcalendarbookedflightdetails</REQUEST>" +
                                "<USERID>" + UserID + "</USERID>" +
                                "<COMPANYNAME>" + Companyname + "</COMPANYNAME>" +
                                "<USERNAME>" + UserName + "</USERNAME>" +
                                "<FLAG>" + strflag + "</FLAG>" +
                                "<COMPANYID>" + CompanyID + "</COMPANYID>" +
                               "</EVENT>";

                string LstrDetails = "<GetBookedFlight_REQUEST><QUERY>" + xmldata + "</QUERY><REQTIME>" + ReqTime + "</REQTIME></GetBookedFlight_REQUEST>";

                DatabaseLog.LogData(UserName, "T", "HomeController", "Getcalendarbookedflightdetails", LstrDetails, UserID.ToString(), CompanyID.ToString(), SequenceID);
                #endregion
                //_inplant.
                dsbookflights = _inplant.FetchBookedTicketDetails(UserID.ToString().ToUpper().Trim(), BookedDate, CompanyID, Apptype, strflag, IPaddress);

                #region ResponseLog
                string ResTime = "GetBookedFlightRESTime:" + DateTime.Now;
                xmldata = string.Empty;
                xmldata = "<EVENT><RESPONSE>Getcalendarbookedflightdetails</RESPONSE><RESTIME>" + ResTime + "</RESTIME>";
                if (Producttype.Trim() == "Airline")
                {
                    if (dsbookflights != null && dsbookflights.Tables.Count > 0 && dsbookflights.Tables[0].Rows.Count > 0)
                    {
                        var qryval = from p in dsbookflights.Tables[0].AsEnumerable()
                                     select new
                                     {
                                         Img_Logo = p["Flightno"].ToString(),//ImageLogo(p["PLATINGCARRIER"].ToString(), p["Flightno"].ToString()),
                                         Dep_city = p["ORIGINCITY"].ToString().Trim(),//Utilities.AirportcityName(p["ORIGINCITY"].ToString().Trim()),
                                         Dest_city = p["DESTINATIONCITY"].ToString().Trim(),//Utilities.AirportcityName(p["DESTINATIONCITY"].ToString().Trim()),
                                         Dep_Time = p["DEPARTURE_DATE"],
                                         Dest_Time = p["ARIVEL_DATE"],
                                         Travellername = p["USD_NAME"],
                                         Producttype = p["PRODUCT_TYPE"],
                                         Company = p["COMPANY"],
                                         PNR = p["PNR"],
                                         TRAVELLERTYPE = p["TRAVELLERTYPE"],
                                         UniqId = ""
                                     };
                        if (qryval.Count() > 0)
                        {
                            strbookeddt = JsonConvert.SerializeObject(qryval);
                        }
                        xmldata += "<RESULT>SUCCESS</RESULT>";
                    }
                    else
                    {
                        xmldata += "<RESULT>FAILURE</RESULT>";
                    }
                }
                else if (Producttype.Trim() == "Hotel")
                {
                    if (dsbookflights != null && dsbookflights.Tables.Count > 1 && dsbookflights.Tables[1].Rows.Count > 0)
                    {
                        var qryval = from p in dsbookflights.Tables[1].AsEnumerable()
                                     select new
                                     {
                                         hOTELNAME = p["hOTELNAME"].ToString().Trim(),
                                         CITYNAME = p["CITYNAME"].ToString().Trim(),
                                         LOCATION = p["LOCATION"].ToString().Trim(),
                                         CHECKINDATE = p["CHECKINDATE"],
                                         CHECKOUTDATE = p["CHECKOUTDATE"],
                                         USERNAME = p["USERNAME"],
                                         Producttype = p["PRODUCT_TYPE"],
                                         Company = p["COMPANY"],
                                         PNR = p["PNR"],
                                         UniqId = ""

                                     };
                        if (qryval.Count() > 0)
                        {
                            strbookeddt = JsonConvert.SerializeObject(qryval);
                        }
                        xmldata += "<RESULT>SUCCESS</RESULT>";
                    }
                    else
                    {
                        xmldata += "<RESULT>FAILURE</RESULT>";
                    }
                }
                else if (Producttype.Trim() == "Rail")
                {
                    if (dsbookflights != null && dsbookflights.Tables.Count > 2 && dsbookflights.Tables[2].Rows.Count > 0)
                    {
                        var qryval = from p in dsbookflights.Tables[2].AsEnumerable()
                                     select new
                                     {
                                         ORIGIN = p["ORIGIN"].ToString().Trim(),
                                         DESTINATION = p["DESTINATION"].ToString().Trim(),
                                         JOURNEYDATE = p["JOURNEY DATE"],
                                         DEPARTURETIME = p["DEPARTURE TIME"],
                                         ARRIVALTIME = p["ARRIVAL TIME"],
                                         TRAINNO = p["TRAIN NO"],
                                         USERNAME = p["USERNAME"],
                                         TRAINNAME = p["TRAINNAME"],
                                         Company = p["COMPANY"],
                                         PNR = p["PNR"],
                                         UniqId = ""


                                     };
                        if (qryval.Count() > 0)
                        {
                            strbookeddt = JsonConvert.SerializeObject(qryval);
                        }
                        xmldata += "<RESULT>SUCCESS</RESULT>";
                    }
                    else
                    {
                        xmldata += "<RESULT>FAILURE</RESULT>";
                    }
                }
                else if (Producttype.Trim() == "Bus")
                {
                    if (dsbookflights != null && dsbookflights.Tables.Count > 3 && dsbookflights.Tables[3].Rows.Count > 0)
                    {
                        var qryval = from p in dsbookflights.Tables[3].AsEnumerable()
                                     select new
                                     {
                                         ORIGIN = p["ORIGINCITY"].ToString().Trim(),
                                         DESTINATION = p["DESTINATIONCITY"].ToString().Trim(),
                                         DEPARTUREDATE = p["DEPARTURE_DATE"],
                                         ARRIVALDATE = p["ARRIVAL_DATE"],
                                         BUSTYPE = p["BUSTYPE"],
                                         USERNAME = p["USD_NAME"],
                                         BUSNAME = p["BUS NAME"],
                                         Company = p["COMPANY"],
                                         PNR = p["PNR"],
                                         UniqId = ""
                                     };
                        if (qryval.Count() > 0)
                        {
                            strbookeddt = JsonConvert.SerializeObject(qryval);
                        }
                        xmldata += "<RESULT>SUCCESS</RESULT>";
                    }
                    else
                    {
                        xmldata += "<RESULT>FAILURE</RESULT>";
                    }
                }
                xmldata += "</EVENT>";
                DatabaseLog.LogData(UserName, "T", "HomeController", "Getcalendarbookedflightdetails", xmldata, UserID.ToString(), CompanyID.ToString(), SequenceID);
                #endregion
            }

            catch (Exception ex)
            {
                string errormgs = string.Empty;
                if (ex.Message.ToString() == "The operation has timed out")
                    errormgs = ex.Message.ToString();
                else if (ex.Message.ToString() == "Unable to connect the remote server")
                    errormgs = "unable to connect the remote server.";
                else
                    errormgs = "Problem occured While Getcalendarbookedflightdetails .Please contact support team (#05).";
                xmldata = "<ERROR>" + ex.ToString() + "</ERROR>";
                DatabaseLog.LogData(UserName, "x", "HomeController", "Getcalendarbookedflightdetails", xmldata, UserID.ToString(), CompanyID.ToString(), SequenceID);
                return Json(new { status = "00", Errormsg = errormgs, DSBDetails = strbookeddt });
            }
            return Json(new { status = "01", Errormsg = "", DSBDetails = strbookeddt });
        }

        public ActionResult Last5Transaction(string strClientID, string strBranchID)
        {
            string stu = string.Empty,
                   msg = string.Empty,
                   result = string.Empty;
            string strBranchid = string.Empty;
            string strUserid = string.Empty;
            string stripAddress = string.Empty;
            string strSequenceid = string.Empty;
            string strPosid = string.Empty;
            string strPartycode = string.Empty;
            string strCoordinatorCode = string.Empty;
            string strTerminaltype = string.Empty;
            string strTerminalId = string.Empty;
            DataSet _udk_result = new DataSet();

            try
            {
                strBranchid = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                strUserid = Session["agentid"].ToString() != "" ? Session["agentid"].ToString() : "";
                stripAddress = Session["agentid"].ToString() != "" ? Session["agentid"].ToString() : "";
                strSequenceid = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
                strTerminaltype = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();

                if (strTerminaltype == "T" && strClientID != "")
                {
                    strClientID = strClientID.Substring(0, (strClientID.Length - 2));
                    strTerminalId = strClientID;
                }

                strPosid = strClientID != null && strClientID != "" ? strClientID : Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                strPartycode = strClientID != null && strClientID != "" ? strClientID : Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                RaysService _RaysService = new RaysService();
                #region SERVICE URL BRANCH BASED -- STS115
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || strBranchCredit.Contains(strBranchID))
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                    }
                    else
                    {
                        _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    }
                }
                else
                {
                    _RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                }
                #endregion

                string xml = "<REQUEST><POSID>" + strPosid + "</POSID><CLIENTID>" + strPartycode + "</CLIENTID></REQUEST>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "Homecontroller", "Last5Transaction", xml.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());

                _udk_result = _RaysService.FetchLast5Trans(strBranchid, strUserid, stripAddress, Convert.ToDecimal(strSequenceid), strPosid, strPartycode, strCoordinatorCode, strTerminaltype, strTerminalId);

                DatabaseLog.LogData(Session["username"].ToString(), "E", "Homecontroller", "Last5Transaction", "<RESULT><JSON>" + JsonConvert.SerializeObject(_udk_result).ToString() + "</JSON></RESULT>", Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());


                if (_udk_result != null && _udk_result.Tables.Count > 0 && _udk_result.Tables[0].Rows.Count > 0)
                {
                    stu = "01";
                    msg = "";
                    result = JsonConvert.SerializeObject(_udk_result).ToString();
                }
                else
                {
                    stu = "02";
                    msg = "No records found.";
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Problem occure while fetch last transaction detaisl,Please contact customer care.(05)";
                //   DatabaseLog.LogData(Session["Loginusermailid"] != null ? Session["Loginusermailid"].ToString() : "", "X", "Last5Transaction", "", ex.ToString(), Session["CompanyID"] != null && Session["CompanyID"].ToString() != "" ? Session["CompanyID"].ToString() : "", Session["CompanyID"] != null && Session["CompanyID"].ToString() != "" ? Session["CompanyID"].ToString() : "", "0", Session["IPAddress"] != null && Session["IPAddress"].ToString() != "" ? Session["IPAddress"].ToString() : "");
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Homecontroller", "Last5Transaction", ex.ToString(), Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());
            }
            return Json(new { Status = stu, Message = msg, Results = result });
        }

        #region _AGENT OUTSTANDING

        public ActionResult OutstandingReport(string strDate, string strAgentID, string strTerminalID, string strUserName, string strAgentType, string strSequenceId, string strBranchID, string strTerminalType, string strIPAddress)
        {

            ArrayList outstanding = new ArrayList();
            outstanding.Add("");
            outstanding.Add("");
            outstanding.Add("");
            outstanding.Add("");
            string sts = string.Empty;
            string msg = string.Empty;
            DataSet dsOutstandingReport = new DataSet();
            string strFlag = "N";
            string strResult = string.Empty;
            string strErrorMsg = string.Empty;
            string strIpAddress = "";
            int Error = 0;
            int Maildata1 = 1;
            int Maildata2 = 2;
            int OutstandingAmt = 3;
            STSTRAVRAYS.Rays_service.RaysService _rays = new STSTRAVRAYS.Rays_service.RaysService();
            strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strBranchId = Session["branchid"] != null ? Session["branchid"].ToString() : "";
            strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            strSequenceId = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            strTerminalID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            strIpAddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            try
            {
                #region UsageLog
                string PageName = "Agent Outstanding";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                strSequenceId = (strSequenceId != "" && strSequenceId != null ? strSequenceId : "0");
                CultureInfo cii = new CultureInfo("en-GB", true);
                cii.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                strDate = Convert.ToDateTime(strDate.Trim(), cii).ToString("yyyyMMdd");
                // decimal dcSequenceId = Convert.ToDecimal(strSequenceId.ToString().ToUpper().Trim());

                _rays.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                bool Result = _rays.Fetch_TopupStatementDetails(strUserName, strDate, strAgentType, strBranchId, "", strAgentID, strFlag, strTerminalID, strIpAddress, strTerminalType, strSequenceId, ref strResult, ref strErrorMsg);

                if (Result == true && string.IsNullOrEmpty(strErrorMsg))
                {
                    if (!string.IsNullOrEmpty(strResult))
                    {
                        dsOutstandingReport = JsonConvert.DeserializeObject<DataSet>(strResult);
                        if (dsOutstandingReport != null && dsOutstandingReport.Tables.Count > 0)
                        {
                            Session.Add("outstandingstatement", strResult);
                            var Query = from p in dsOutstandingReport.Tables[1].AsEnumerable()
                                        select new
                                        {
                                            Date = p.Field<string>("Date"),
                                            Delay_Days = p["Delay_Days"],
                                            OD_Amount = p["OD_Amount"],
                                            Receipt_Amount = p["Receipt_Amount"],
                                            Outstanding_Amount = p["Outstanding_Amount"],
                                            RefID = p["RefID"]
                                        };
                            outstanding[Maildata1] = JsonConvert.SerializeObject(dsOutstandingReport.Tables[0]);
                            outstanding[Maildata2] = JsonConvert.SerializeObject(Serv.ConvertToDataTable(Query));
                            var sum = dsOutstandingReport.Tables[1].AsEnumerable().Sum(dr => Convert.ToDecimal(dr["Outstanding_Amount"]));
                            outstanding[OutstandingAmt] = sum.ToString();
                            sts = "01";
                        }
                        else
                        {
                            sts = "00";
                            msg = "No Records Found(#01)!.";
                        }
                    }
                    else
                    {
                        sts = "00";
                        msg = "No Records Found(#01)!.";
                    }
                }
                else
                {
                    sts = "00";
                    msg = (!string.IsNullOrEmpty(strErrorMsg)) ? strErrorMsg + "(#03)" : "Unable to fetch the details(#01)!.";
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(strUserName, "X", "Homecontroller", "outstandingreport", ex.ToString(), strAgentID, strTerminalID, strSequenceId);
                return Json(new { Status = "00", Message = "problem occured while getting details-#05", Result = "" });
            }
            return Json(new { Status = sts, Message = msg, Result = outstanding });

        }

        public ActionResult ClEAROUTSTANDINGENCRYPTURL(string strAgentID, string strTerminalID, string strUserName, string strTerminalType, string dcSequenceId, string strIPAddress, string strReceiptID, string strReceiptAmt)
        {
            ArrayList _ArryURl = new ArrayList();
            _ArryURl.Add("");
            _ArryURl.Add("");
            int ERROR = 0;
            int RESULT = 1;
            string sts = string.Empty;
            string msg = string.Empty;
            string strErrorMsg = string.Empty;

            STSTRAVRAYS.Rays_service.RaysService _raysService = new STSTRAVRAYS.Rays_service.RaysService();
            strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            strTerminalID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strUsername = Session["username"] != null ? Session["username"].ToString() : "";
            strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strSequenceId = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            strIPAddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";

            try
            {
                #region UsageLog
                string PageName = "Agent Outstanding";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "UPDATE");
                }
                catch (Exception e) { }
                #endregion
                strSequenceId = (strSequenceId != "" && strSequenceId != null ? strSequenceId : "0");
                decimal strdcSequenceId = Convert.ToDecimal(strSequenceId.ToString().ToUpper().Trim());
                object obj = Session["outstandingstatement"];
                string strClrOutsandReport = (obj != null && obj != "") ? Session["outstandingstatement"].ToString() : "";

                if (strClrOutsandReport == "")
                {
                    msg = "Session Expried!.";
                    return Json(new { status = "-1", Message = msg, Result = "" });
                }

                DataSet dsOutstandingReport = JsonConvert.DeserializeObject<DataSet>(strClrOutsandReport);

                string xml = "<Table>";
                for (int i = 0; i < strReceiptID.Split('|').Length; i++)
                {

                    if (dsOutstandingReport != null && dsOutstandingReport.Tables.Count > 0)
                    {
                        DataTable dtTemp = new DataTable();


                        var Query = from p in dsOutstandingReport.Tables[1].AsEnumerable()
                                    where p["RefID"].ToString().ToUpper().Trim() == strReceiptID.Split('|')[i].ToString().ToUpper().Trim()
                                    select new
                                    {
                                        ReceiptID = p["RefID"],
                                        ReceiptAmt = p["Outstanding_Amount"],
                                    };

                        if (Query.Count() > 0)
                        {
                            foreach (var order in Query)
                            {
                                xml += "<OutstandingDetails><OutstandingRefID>" + order.ReceiptID + "</OutstandingRefID>";
                                xml += "<OutstandingAmt>" + order.ReceiptAmt + "</OutstandingAmt></OutstandingDetails>";
                            }
                        }

                    }
                }
                xml += "</Table>";
                string strTrackID = "OST" + strTerminalId + DateTime.Now.ToString("yyyyMMhhHHmmss");
                _raysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                bool Result = _raysService.Insert_ClearOutStandingtrackREQ(strAgentID, strTerminalID, strUserName, strTerminalType, strTrackID, xml, strIPAddress, strdcSequenceId, ref strErrorMsg);
                if (Result == true && string.IsNullOrEmpty(strErrorMsg))
                {
                    string PARAMETER = string.Empty;
                    if (!string.IsNullOrEmpty(strReceiptID))
                    {
                        PARAMETER = strAgentID + "@" + strTerminalID + "@" + strTerminalType + "@" + strUsername + "@" + strIPAddress + "@" + strSequenceId + "@" + strTrackID + "@O@OST@" + strReceiptAmt + "@@";
                    }

                    _ArryURl[RESULT] = ConfigurationManager.AppSettings["PGURL"].ToString() + "?AGENTID=" + strAgentID + "&PGKEY=" + Base.EncryptKEy(PARAMETER, "SKV" + strAgentID.ToString().ToUpper().Trim()) + "&RETURNURL=";
                    sts = "01";
                }
                else
                {
                    sts = "00";
                    msg = (!string.IsNullOrEmpty(strErrorMsg)) ? strErrorMsg + "(#03)" : "problem occured while clear outstanding!.(#01)";
                }
            }
            catch (Exception ex)
            {
                sts = "00";
                msg = "Unable to problem occured URL encrypt(#05)";
                DatabaseLog.LogData(strUsername, "X", "Homecontroller", "ClEAROUTSTANDINGENCRYPTURL", ex.ToString(), strAgentID, strTerminalID, Session["SequenceId"].ToString());
            }
            return Json(new { Status = sts, Message = msg, Result = _ArryURl });
        }

        public void btnExportExcel()
        {
            StringBuilder Outstanding = new StringBuilder();
            ArrayList exporttoexcel = new ArrayList();

            exporttoexcel.Add("");
            int status = 0;

            try
            {
                string strClrOutsandReport = Session["outstandingstatement"].ToString();
                DataSet dsOutstandingReport = JsonConvert.DeserializeObject<DataSet>(strClrOutsandReport);

                DataTable dt = new DataTable();
                var qrytable = (from p in dsOutstandingReport.Tables[0].AsEnumerable()
                                select new
                                {

                                    TopUpDate = p.Field<string>("Date"),
                                    ReceiptDate = p.Field<string>("Receipt Date"),
                                    ReceiptID = p.Field<string>("Receipt ID"),
                                    OD_Amount = p.Field<double>("OD Amount"),
                                    Receipt_Amount = p.Field<double>("Receipt Amount"),
                                    Outstanding_Amount = p.Field<double>("Outstanding Amount"),
                                    Remarks = p.Field<string>("Remarks"),
                                    Entered_By = p.Field<string>("Entered By")

                                });
                dt = Serv.ConvertToDataTable(qrytable);
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=OustandingReport_" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Homecontroller", "btnExportExcel", ex.ToString(), Session["strAgentId"].ToString(), Session["strTerminalId"].ToString(), Session["strSequenceId"].ToString());
                exporttoexcel[status] = "Problem Occured.Please contact customercare";

            }


        }

        #endregion

        #region Airline Name Format Dynamic Changes
        [HttpPost]
        public ActionResult AirlineNameFormat()
        {
            string strErrormsg = "";
            string stu = string.Empty;
            string msg = string.Empty;
            DataSet strResult = new DataSet();
            string agentID = string.Empty;
            string terminalID = string.Empty;
            string userName = string.Empty;
            string ipAddress = string.Empty;
            string terminalType = string.Empty;
            string sequenceID = string.Empty;
            StringBuilder airlinename = new StringBuilder();
            string stairline = string.Empty;
            string Xml = string.Empty;
            try
            {
                #region UsageLog
                string PageName = "Airlinenameformat";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                userName = Session["username"] != null ? Session["username"].ToString() : "";
                terminalType = Session["terminaltype"] != null ? Session["terminaltype"].ToString() : "";
                sequenceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
                agentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                terminalID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                ipAddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                sequenceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0";

                InplantService.Inplantservice _inplantServ = new InplantService.Inplantservice();
                _inplantServ.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                #region RequestLog
                Xml = "<EVENT><REQUEST>AirlineNameFormat</REQUEST>" +
                                "<AGENTID>" + agentID + "</AGENTID>" +
                                "<TERMINALID>" + terminalID + "</TERMINALID>" +
                                "<USERNAME>" + userName + "</USERNAME>" +
                                "<IPADDRESS>" + ipAddress + "</IPADDRESS>" +
                                "<TERMINALTYPE>" + terminalType + "</TERMINALTYPE>" +
                                "<SEQUENCEID>" + sequenceID + "</SEQUENCEID>" +
                               "</EVENT>";
                DatabaseLog.LogData(userName, "E", "HomeController", "AirlineNameFormat -Request", Xml.ToString(), agentID, terminalID, sequenceID);

                #endregion
                bool airname = _inplantServ.Fetch_AirlineNameFormat(agentID, terminalID, userName, ipAddress, terminalType, Convert.ToDecimal(sequenceID), "", ref strErrormsg, ref strResult);

                #region ResponseLog
                string xmldata = "";
                xmldata = "<EVENT><RESPONSE>AirlineNameFormat</RESPONSE>" +
                                "<RESULT>" + airname + "</RESULT>" +
                              "</EVENT>";
                DatabaseLog.LogData(userName, "E", "HomeController", "AirlineNameFormat -Response", xmldata.ToString(), agentID, terminalID, sequenceID);
                #endregion
                if (airname == true && string.IsNullOrEmpty(strErrormsg) && strResult != null && strResult.Tables.Count > 0)
                {
                    airlinename.Append("<p style='font-size: 14px;font-weight: 500;color: #616161;font-family: sans-serif,Open Sans; margin-bottom:5%;'> Above information is just for your guideline and is subject to change by the Airline without any prior notice. We will try to keep this section updated regularly, Riya Travel and Tours (I) Pvt Ltd undertakes no liability for loss that you may suffer due to some information mentioned here and changed by the airline </p>");
                    airlinename.Append("<div class='row0 headname'>");
                    airlinename.Append("<div class='col-sm-1 col-xs-2' style='border-right: 1px solid #ddd;border-left: 1px solid #ddd;border-top: 1px solid #ddd;font-size: 13px;font-weight: 600;color: #333;text-transform: uppercase;text-align:center;padding: 8px 10px;border-bottom: 1px solid #ddd;background-color: #f1f1f1;height: 46px;'><span>AIRLINE CODE</span></div>");
                    airlinename.Append("<div class='col-sm-5 col-xs-4' style='border-right: 1px solid #ddd;border-left: 1px solid #ddd;border-top: 1px solid #ddd;font-size: 13px;font-weight: 600;color: #333;text-transform: uppercase;text-align:center;padding: 8px 10px;border-bottom: 1px solid #ddd;background-color: #f1f1f1;height: 46px;'><span>AIRLINE NAME</span></div>");
                    airlinename.Append("<div class='col-sm-2 col-xs-2' style='border-right: 1px solid #ddd;border-left: 1px solid #ddd;border-top: 1px solid #ddd;font-size: 13px;font-weight: 600;color: #333;text-transform: uppercase;text-align:center;padding: 8px 10px;border-bottom: 1px solid #ddd;background-color: #f1f1f1;height: 46px;'><span>FIRST NAME</span></div>");
                    airlinename.Append("<div class='col-sm-2 col-xs-2' style='border-right: 1px solid #ddd;border-left: 1px solid #ddd;border-top: 1px solid #ddd;font-size: 13px;font-weight: 600;color: #333;text-transform: uppercase;text-align:center;padding: 8px 10px;border-bottom: 1px solid #ddd;background-color: #f1f1f1;height: 46px;'><span>LAST NAME</span></div>");
                    airlinename.Append("<div class='col-sm-2 col-xs-2' style='border-right: 1px solid #ddd;border-left: 1px solid #ddd;border-top: 1px solid #ddd;font-size: 13px;font-weight: 600;color: #333;text-transform: uppercase;text-align:center;padding: 8px 10px;border-bottom: 1px solid #ddd;background-color: #f1f1f1;height: 46px;'><span>REMARKS</span></div>");
                    airlinename.Append("</div>");
                    for (int i = 0; i < strResult.Tables[0].Rows.Count; i++)
                    {
                        airlinename.Append("<div class='row0 innerdets'>");
                        airlinename.Append("<div class='col-sm-1 col-xs-2' style='padding: 8px 10px;border-right: 1px solid #ddd;height:35px;border-left: 1px solid #ddd;font-size: 13px;font-weight: 500;color: #333;border-bottom: 1px solid #ddd;white-space: nowrap;' id=aAirlinecode><span>" + strResult.Tables[0].Rows[i]["ANF_AIRLINE_CODE"].ToString() + "</span></div>");
                        airlinename.Append("<div class='col-sm-5 col-xs-4' style='padding: 8px 10px;border-right: 1px solid #ddd;height:35px;border-left: 1px solid #ddd;border-bottom: 1px solid #ddd;font-size: 13px;font-weight: 500;color: #333;white-space: nowrap;' id=aAirlineName><span>" + strResult.Tables[0].Rows[i]["ANF_AIRLINE_NAME"].ToString() + "</span></div>");
                        airlinename.Append("<div class='col-sm-2 col-xs-2' style='padding: 8px 10px;border-right: 1px solid #ddd;height:35px;border-left: 1px solid #ddd;border-bottom: 1px solid #ddd;font-size: 13px;font-weight: 500;color: #333;white-space: nowrap;' id=aFirstName><span>" + strResult.Tables[0].Rows[i]["ANF_FIRST_NAME"].ToString() + "</span></div>");
                        airlinename.Append("<div class='col-sm-2 col-xs-2' style='padding: 8px 10px;border-right: 1px solid #ddd;height:35px;border-left: 1px solid #ddd;border-bottom: 1px solid #ddd;font-size: 13px;font-weight: 500;color: #333;white-space: nowrap;' id=aLastName><span>" + strResult.Tables[0].Rows[i]["ANF_LAST_NAME"].ToString() + "</span></div>");
                        airlinename.Append("<div class='col-sm-2 col-xs-2' style='padding: 8px 10px;border-right: 1px solid #ddd;height:35px;border-left: 1px solid #ddd;border-bottom: 1px solid #ddd;font-size: 13px;font-weight: 500;color: #333;white-space: nowrap;' id=aLastName><span>" + (strResult.Tables[0].Columns.Contains("ANF_REMARKS") ? strResult.Tables[0].Rows[i]["ANF_REMARKS"].ToString() : "") + "</span></div>");
                        airlinename.Append("</div>");
                    }

                    stairline = airlinename.ToString();
                    stu = "01";
                }
                else
                {
                    stu = "00";
                    msg = (!string.IsNullOrEmpty(strErrormsg)) ? strErrormsg + "(#03)" : "Unable to fetch the details(#01)!.";
                }

            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(userName, "X", "Homecontroller", "AirlineNameFormat -Error", ex.ToString(), agentID, terminalID, sequenceID);
                return Json(new { Status = "00", Message = "problem occured while getting details-#05", Result = "" });
            }
            return Json(new { Status = stu, Message = msg, strbuild = stairline });
        }
        #endregion

        #region GetEmployeeDetails // For Cash Payment Mode Booking
        public ActionResult GetEmployeeDetails(string strEmployeeCode)
        {
            string strStatus = string.Empty, strMessage = string.Empty, strResult = string.Empty;
            string strClientID = string.Empty, strTerminalID = string.Empty, strUserName = string.Empty;
            string strIPAddress = string.Empty, strSeqID = string.Empty;
            string strRequest = string.Empty, strResponse = string.Empty, LogData = string.Empty;
            string strApiURL = string.Empty, strApiKey = string.Empty, strApiPassword = string.Empty;
            try
            {
                strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
                strUserName = Session["username"] != null && Session["username"].ToString() != "" ? Session["username"].ToString() : "";
                strClientID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
                strIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
                strSeqID = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyymmddhhmmss");

                strApiURL = ConfigurationManager.AppSettings["RIYAEMP_API_URL"].ToString();
                strApiKey = ConfigurationManager.AppSettings["RIYAEMP_API_KEY"].ToString();
                strApiPassword = ConfigurationManager.AppSettings["RIYAEMP_API_PASSWORD"].ToString();

                strRequest = "client=STS&api_key=" + strApiKey + "&api_password=" + strApiPassword + "&emp_code=" + strEmployeeCode + "";

                LogData = "<REQUEST><strRequest>" + strRequest + "</strRequest><strApiURL>" + strApiURL + "</strApiURL></REQUEST>";
                DatabaseLog.LogData(strUserName, "E", "Homecontroller", "GetEmployeeDetails~REQ", LogData, strClientID, strTerminalID, strSeqID);

                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/x-www-form-urlencoded";
                byte[] data = client.UploadData(strApiURL, "POST", Encoding.ASCII.GetBytes(strRequest));
                strResponse = Encoding.ASCII.GetString(data);

                if (!string.IsNullOrEmpty(strResponse))
                {
                    DataSet dsSet = new DataSet();
                    dsSet = Serv.convertJsonStringToDataSet(strResponse, "");
                    if (dsSet != null && dsSet.Tables.Count > 0 && dsSet.Tables[0].Rows.Count > 0 && dsSet.Tables[0].Rows[0]["Status"].ToString() == "1")
                    {
                        strStatus = "01";
                        strMessage = "";
                        strResult = strResponse;
                    }
                    else
                    {
                        strStatus = "00";
                        strMessage = dsSet.Tables[0].Rows[0]["message"] != null ? dsSet.Tables[0].Rows[0]["message"].ToString() : "No Records Found.";
                        strResult = "";
                    }
                }
                else
                {
                    strStatus = "03";
                    strMessage = "No Records Found.";
                    strResult = "";
                }
                LogData = "<RESPONSE><strStatus>" + strStatus + "</strStatus><strMessage>" + strMessage + "</strMessage>"
                        + "<strResult>" + strResult + "</strResult><Json>" + strResponse + "</Json></RESPONSE>";
                DatabaseLog.LogData(strUserName, "E", "Homecontroller", "GetEmployeeDetails~RES", LogData, strClientID, strTerminalID, strSeqID);
            }
            catch (Exception ex)
            {
                strStatus = "05";
                strMessage = "Problem occured while fetching employee detail.please contact support team.";
                strResult = "";
                DatabaseLog.LogData(strUserName, "X", "Homecontroller", "GetEmployeeDetails~ERR", ex.ToString(), strClientID, strTerminalID, strSeqID);
            }
            return Json(new { Status = strStatus, Message = strMessage, Result = strResult });
        }
        #endregion
    }
}