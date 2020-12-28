using STSTRAVRAYS.Models;
using STSTRAVRAYS.Rays_service;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using STSTRAVRAYS.InplantService;

namespace STSTRAVRAYS.Controllers
{
    public class AfterBookingController : Controller
    {
        string strBranchCredit = ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"] != null ? ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"].ToString() : "";
        RaysService _rays_servers = new RaysService();
        Base.ServiceUtility Serv = new Base.ServiceUtility();
        Inplantservice _INPLANTSERVICE = new Inplantservice();

        #region PRINT TICKET
        public ActionResult PrintTickets(string strSPNRNO, string single, string Page)
        {
            #region UsageLog
            string PageName = "Print Ticket";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
            }
            catch (Exception e) { }
            #endregion

            string str_tickref = string.Empty;
            Page = Page.Contains('@') ? Page.Split('@')[0] : Page;

            string strUserName = Session["username"] != null && Session["username"].ToString() != "" ? Session["username"].ToString() : "";
            string strTerminalId = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strAgentID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string strSequnceID = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyMMdd");
            string getAgentType = Session["agenttype"] != null && Session["agenttype"].ToString() != "" ? Session["agenttype"].ToString() : "";
            string getprevilage = Session["privilage"] != null && Session["privilage"].ToString() != "" ? Session["privilage"].ToString() : "";
            string Ipaddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string TerminalApp = Session["TerminalType"] != null && Session["TerminalType"].ToString() != "" ? Session["TerminalType"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"].ToString() != "") ? Session["TKTFLAG"].ToString() : "DTKT";
            string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");
            string strClientBranchID = Session["ClientBranchID"] != null && Session["ClientBranchID"].ToString() != "" ? Session["ClientBranchID"].ToString() : "";
            string strAgentLogin = Session["IsAgentLogin"] != null && Session["IsAgentLogin"].ToString() != "" ? Session["IsAgentLogin"].ToString() : "N";
            string strCustomerLogin = Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() != "" ? Session["CustomerLogin"].ToString() : "N";
            string strPlatform = string.Empty;

            DataSet dsViewPNR = new DataSet();
            DataSet dsFareDispDet = new DataSet();
            byte[] dsViewPNR_compress = new byte[] { };
            byte[] dsFareDispDet_compress = new byte[] { };
            DataSet dspdf = new DataSet();
            string strResult = string.Empty;
            string MessageToCustomer = string.Empty;
            try
            {
                if (Session["POS_ID"] == null || Session["POS_ID"].ToString() == "" || strTKTFLAG == "")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                }

                strPlatform = strAgentLogin != "Y" ? "B2C" : "B2B";
                ViewBag.Page = Page;
                ViewBag.hdnsingle = single;
                Session.Add("pagename", Page);

                if (strSPNRNO != null && strSPNRNO.Contains("|"))
                {
                    string ErrorMsg_PNR = "ERROR";
                    string strErrorMsg = "ERROR";
                    bool result = false;
                    string SPNRNO = strSPNRNO;
                    string strWfare = "YES";
                    string strWlogo = "YES";
                    string strWbreakup = "YES";
                    string strWSF = "YES";
                    if (strSPNRNO.Contains("|"))
                    {
                        SPNRNO = strSPNRNO.Split('|')[0].ToString();
                        strWfare = strSPNRNO.Split('|')[1].ToString().ToUpper();
                        ViewBag.Wfare = strSPNRNO.Split('|')[1].ToString();
                        ViewBag.hdnSpnr_U = strSPNRNO;
                        ViewBag.Without_farechack = SPNRNO;
                        if (strSPNRNO.Split('|').Length > 2)
                        {
                            strWlogo = strSPNRNO.Split('|')[2].ToString().ToUpper();
                        }
                        if (strSPNRNO.Split('|').Length > 3)
                        {
                            strWbreakup = strSPNRNO.Split('|')[3].ToString().ToUpper();
                            strWSF = strSPNRNO.Split('|')[4].ToString().ToUpper();
                        }
                    }
                    ViewBag.SPNR = strSPNRNO;

                    var TicketCopy = GetTicketCopy(strSPNRNO, single);

                    if (single.ToUpper() != "YES")
                    {
                        ViewBag.hdnSpnr = strSPNRNO;
                        str_tickref = Session["printtkt"].ToString();
                        ViewBag.printdetails = str_tickref;
                    }

                    string strSpnr = SPNRNO.Contains('/') ? SPNRNO.Split('/')[0].ToString().Trim() : SPNRNO;

                    result = _rays_servers.Fetch_PNR_Verification_Details_NewByte(strAgentID, strSpnr, "", "", "",
                                strTerminalId, strUserName, Ipaddress, TerminalApp, Convert.ToDecimal(strSequnceID), ref dsViewPNR_compress,
                                ref dsFareDispDet_compress, ref ErrorMsg_PNR, ref strErrorMsg, "FetchPnrdetails", "FetchPnrdetails", getAgentType, strTerminalId);

                    dsViewPNR = Base.Decompress(dsViewPNR_compress);
                    dsFareDispDet = Base.Decompress(dsFareDispDet_compress);
                    string strCustEmailID = ((dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Columns.Contains("EMAIL_ID") && dsViewPNR.Tables[0].Rows.Count > 0 && dsViewPNR.Tables[0].Rows[0]["EMAIL_ID"].ToString().Trim() != "") ? dsViewPNR.Tables[0].Rows[0]["EMAIL_ID"].ToString() : "");
                    ViewBag.UserMailID = ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("EMAIL_ID") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["EMAIL_ID"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["EMAIL_ID"].ToString() : (strCustEmailID));

                    if (strSPNRNO.Contains('|') && strSPNRNO.Split('|')[1].ToString().ToUpper() == "YES")// && pagerigts != "B" && '@strIsAgentLogin' == 'Y'
                    {
                        string adt1 = "1";
                        string Chd1 = "0";
                        string inf1 = "0";
                        decimal adtvalue = 0;
                        decimal chdvalue = 0;
                        decimal infvalue = 0;

                        double adt_count = 1;
                        double chd_count = 0;
                        double inf_count = 0;

                        decimal adt_mkpvalue = 0;
                        decimal chd_mkpvalue = 0;
                        decimal inf_mkpvalue = 0;
                        if (dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsViewPNR.Tables[0].Rows)
                            {
                                adtvalue += ((dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("ADULT") ||
                                dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("ADT")) && (!string.IsNullOrEmpty(dr["SERVICECHARGE"].ToString().Trim()))
                                && dr["SERVICECHARGE"].ToString().Trim() != "0") ? Base.ServiceUtility.ConvertToDecimal(dr["SERVICECHARGE"].ToString().Trim()) : 0;

                                chdvalue += ((dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("CHILD") ||
                                dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("CHD")) && (!string.IsNullOrEmpty(dr["SERVICECHARGE"].ToString().Trim()))
                                && dr["SERVICECHARGE"].ToString().Trim() != "0") ? Base.ServiceUtility.ConvertToDecimal(dr["SERVICECHARGE"].ToString().Trim()) : 0;

                                infvalue += ((dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("INFANT") ||
                                dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("INF")) && (!string.IsNullOrEmpty(dr["SERVICECHARGE"].ToString().Trim()))
                                && dr["SERVICECHARGE"].ToString().Trim() != "0") ? Base.ServiceUtility.ConvertToDecimal(dr["SERVICECHARGE"].ToString().Trim()) : 0;

                                adt_mkpvalue += ((dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("ADULT") ||
                                dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("ADT")) && (!string.IsNullOrEmpty(dr["PAI_TKT_MANAGEMENT_FEE"].ToString().Trim()))
                                && dr["PAI_TKT_MANAGEMENT_FEE"].ToString().Trim() != "0") ? Base.ServiceUtility.ConvertToDecimal(dr["PAI_TKT_MANAGEMENT_FEE"].ToString().Trim()) : 0;

                                chd_mkpvalue += ((dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("CHILD") ||
                                dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("CHD")) && (!string.IsNullOrEmpty(dr["PAI_TKT_MANAGEMENT_FEE"].ToString().Trim()))
                                && dr["PAI_TKT_MANAGEMENT_FEE"].ToString().Trim() != "0") ? Base.ServiceUtility.ConvertToDecimal(dr["PAI_TKT_MANAGEMENT_FEE"].ToString().Trim()) : 0;

                                inf_mkpvalue += ((dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("INFANT") ||
                                dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("INF")) && (!string.IsNullOrEmpty(dr["PAI_TKT_MANAGEMENT_FEE"].ToString().Trim()))
                                && dr["PAI_TKT_MANAGEMENT_FEE"].ToString().Trim() != "0") ? Base.ServiceUtility.ConvertToDecimal(dr["PAI_TKT_MANAGEMENT_FEE"].ToString().Trim()) : 0;

                                ViewBag.Txtadultcnt = "*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + dsViewPNR.Tables[0].Rows[0]["ADULTCOUNT"].ToString();
                                ViewBag.Txtchildcnt = "*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + dsViewPNR.Tables[0].Rows[0]["CHILDCOUNT"].ToString();
                                ViewBag.Txtinfantcnt = "*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + dsViewPNR.Tables[0].Rows[0]["INFANTCOUNT"].ToString();

                                adt_count = Convert.ToDouble(dsViewPNR.Tables[0].Rows[0]["ADULTCOUNT"].ToString());
                                chd_count = Convert.ToDouble(dsViewPNR.Tables[0].Rows[0]["CHILDCOUNT"].ToString());
                                inf_count = Convert.ToDouble(dsViewPNR.Tables[0].Rows[0]["INFANTCOUNT"].ToString());

                                ViewBag.grpbooking = dr["PASSENGER_TYPE"].ToString().Trim().ToUpper() == "GROUP" ? true : false;
                                ViewBag.Groupcount = ViewBag.grpbooking == true ? "*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Convert.ToInt32((Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["ADULTCOUNT"].ToString()) +
                                    Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["CHILDCOUNT"].ToString()) +
                                    Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["INFANTCOUNT"].ToString()))) : "0";

                                ViewBag.Grp_amt = ViewBag.grpbooking == true ? Base.ServiceUtility.ConvertToDecimal(dr["SERVICECHARGE"].ToString().Trim()) : 0;

                                if (dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("CHILD") ||
                                    dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("CHD"))
                                    Chd1 = "1";
                                if (dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("INFANT") ||
                                     dr["PASSENGER_TYPE"].ToString().Trim().ToUpper().Contains("INF"))
                                    inf1 = "1";
                            }
                        }

                        decimal pax_adtvalue = 0;
                        decimal pax_chdvalue = 0;
                        decimal pax_infvalue = 0;
                        if (Convert.ToDouble(adt_count) != 0)
                        { pax_adtvalue = Convert.ToDecimal(adtvalue) / Convert.ToDecimal(adt_count); }
                        if (Convert.ToDouble(chd_count) != 0)
                        {
                            pax_chdvalue = Convert.ToDecimal(chdvalue) / Convert.ToDecimal(chd_count);
                        }
                        if (Convert.ToDouble(inf_count) != 0)
                        {
                            pax_infvalue = Convert.ToDecimal(infvalue) / Convert.ToDecimal(inf_count);
                        }

                        ViewBag.txtadult = pax_adtvalue.ToString();
                        ViewBag.txtChild = pax_chdvalue.ToString();
                        ViewBag.txtinfant = pax_infvalue.ToString();

                        ViewBag.txtadult_mkp = adt_mkpvalue.ToString();
                        ViewBag.txtChild_mkp = chd_mkpvalue.ToString();
                        ViewBag.txtinfant_mkp = inf_mkpvalue.ToString();

                        ViewBag.hdnadultfare_ = pax_adtvalue.ToString();
                        ViewBag.hdnchildfare_ = pax_chdvalue.ToString();
                        ViewBag.hdninffare_ = pax_infvalue.ToString();

                        ViewBag.Allowadultinput = adt1;
                        ViewBag.Allowchildinput = Chd1;
                        ViewBag.allowinfadult = inf1;

                    }
                    if (single.ToUpper() == "YES" && dsViewPNR.Tables.Count > 0)
                    {
                        var passlist = "";
                        var sing1 = (from p in dsViewPNR.Tables[0].AsEnumerable()
                                     select new
                                     {
                                         PaxRef = p["PAX_REF_NO"].ToString().Trim(),
                                         PaxName = p["PASSENGER_NAME"].ToString().Trim(),
                                     }).Distinct();
                        DataTable sintable1 = Serv.ConvertToDataTable(sing1);

                        if (sintable1.Rows.Count > 1)
                        {

                            passlist += "<table id='Paxrefdetail' class='ContactDetails' style='background-color: #fff; border:1px solid #CCC; '><tbody>";
                            passlist += "<tr height: 37px; background-color: #fcb322;color:#000;border: 0; width:37%;>";
                            passlist += "<th >";
                            passlist += "<div class='cntr'>";
                            passlist += "<input style='display: none;' name='chkallrefbox' id='checkedallpaxxref' checked class='cb' type='checkbox' onclick='checkallpaxxno()' />";
                            passlist += " <label class='cbx' for='checkedallpaxxref' style='border: 1px solid #fff !important;'></label>";
                            passlist += "<label class='lbl' for='checkedallpaxxref'>ALL&nbsp;&nbsp;Passenger List</label>";
                            passlist += "</div>";
                            passlist += "</th></tr>";
                            if (dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                            {
                                var sing = (from p in dsViewPNR.Tables[0].AsEnumerable()
                                            select new
                                            {
                                                PaxRef = p["PAX_REF_NO"].ToString().Trim(),
                                                PaxName = p["PASSENGER_NAME"].ToString().Trim(),
                                            }).Distinct();
                                DataTable sintable = Serv.ConvertToDataTable(sing);
                                passlist += "<tr><td align='center'><div class='someClass' ><div class='submenu'><ul id='Submenu'>";
                                for (var i = 0; i < sintable.Rows.Count; i++)
                                {
                                    if (i % 2 != 0)
                                    {
                                        passlist += "<li class='var_navNew-G'>";
                                        passlist += "<div class='cntr'>";
                                        passlist += "<input style='display: none;' name='chkrefbox' id='" + sintable.Rows[i]["PaxRef"] + "' checked class='cb Singleprintlist' data-val='" + sintable.Rows[i]["PaxRef"] + "' type='checkbox' />";
                                        passlist += " <label class='cbx' for='" + sintable.Rows[i]["PaxRef"] + "' style='border: 1px solid #fff !important;'></label>";
                                        passlist += "<label class='lbl' for='" + sintable.Rows[i]["PaxRef"] + "'>" + sintable.Rows[i]["PaxName"] + "</label>";

                                        passlist += "</div>";
                                        passlist += "</li>";
                                    }
                                    else
                                    {
                                        passlist += "<li class='var_navNew-B'>";
                                        passlist += "<div class='cntr'>";
                                        passlist += "<input style='display: none;' name='chkrefbox' id='" + sintable.Rows[i]["PaxRef"] + "' checked class='cb Singleprintlist' data-val='" + sintable.Rows[i]["PaxRef"] + "' type='checkbox'  />";
                                        passlist += " <label class='cbx' for='" + sintable.Rows[i]["PaxRef"] + "' style='border: 1px solid #fff !important;'></label>";
                                        passlist += "<label class='lbl' for='" + sintable.Rows[i]["PaxRef"] + "'>" + sintable.Rows[i]["PaxName"] + "</label>";
                                        passlist += "</div>";
                                        passlist += "</li>";
                                    }
                                }
                            }
                            passlist += "</ul></div></div></td></tr>";
                            passlist += " <tr><td style='text-align:center;padding-bottom: 5px;'><input style='width: 60% !important;margin:10px;' class='btn btn-md btn-primary' type='button' id='passlistcnfrm' value='Submit' onclick='Paxrefno()'/></td></tr>";
                            passlist += "</table>";
                            ViewBag.singpasswflist = passlist.ToString();
                        }
                    }
                }
                else
                {
                    DatabaseLog.LogData(strUserName, "E", "AfterBookingController.cs", "PrintTickets~NoPNR", "<EVENT>PNR EMPTY</EVENT>", strAgentID, strTerminalId, strSequnceID);
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(strUserName, "X", "AfterBookingController.cs", "PrintTickets Ex-2", ex.ToString(), strAgentID, strTerminalId, strSequnceID);
                return RedirectToAction("SessionExp", "Redirect");
            }
            return View();
        }

        public ActionResult ticket_print(string SPNRNO, string single, string paxrefno)
        {
            string str_tick = string.Empty;
            Session.Add("hdnpaxprint", paxrefno);
            string str = Session["pagename"].ToString();
            PrintTickets(SPNRNO, single, str);
            str_tick = Session["printtkt"].ToString();
            return Json(new { Status = "", Message = "", Result = str_tick });
        }
        #endregion

        #region SEND MAIL TECKET COPY
        public ActionResult btnEmail_pdforhtml_Click(string strSPNRNO, string txtEmailID, string pdf, string strMobNumber, string strSingle, string Subject,
            string strPaymentmode, bool blntktmailflag)
        {
            ArrayList Charge = new ArrayList();
            int Array_Result = 1;
            int Array_Error = 0;
            Charge.Add("");
            Charge.Add("");

            string stu = string.Empty;
            string msg = string.Empty;
            string xml = string.Empty;
            string strAgentID = (Session["POS_ID"].ToString() != null || Session["POS_ID"].ToString() != "") ? Session["POS_ID"].ToString() : "";
            string strTerminalId = (Session["POS_TID"].ToString() != null || Session["POS_TID"].ToString() != "") ? Session["POS_TID"].ToString() : "";
            string strUserName = (Session["username"].ToString() != null || Session["username"].ToString() != "") ? Session["username"].ToString() : "";
            string Ipaddress = (Session["ipAddress"].ToString() != null || Session["ipAddress"].ToString() != "") ? Session["ipAddress"].ToString() : "";
            string getprevilage = (Session["privilage"].ToString() != null || Session["privilage"].ToString() != "") ? Session["privilage"].ToString() : "";
            string getAgentType = (Session["agenttype"].ToString() != null || Session["agenttype"].ToString() != "") ? Session["agenttype"].ToString() : "";
            string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"].ToString() != "") ? Session["TKTFLAG"].ToString() : "DTKT";
            string strSequenceid = (Session["sequenceid"] != null && Session["sequenceid"].ToString() != "") ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string strClientBranchID = Session["ClientBranchID"] != null && Session["ClientBranchID"].ToString() != "" ? Session["ClientBranchID"].ToString() : "";
            string strAgentLogin = Session["IsAgentLogin"] != null && Session["IsAgentLogin"].ToString() != "" ? Session["IsAgentLogin"].ToString() : "N";
            string strCustomerLogin = Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() != "" ? Session["CustomerLogin"].ToString() : "N";
            string strPlatform = string.Empty;
            string ticket = string.Empty;
            string TickectCopy = string.Empty;
            try
            {
                strPlatform = strAgentLogin != "Y" ? "B2C" : "B2B";
                string SPNRNO = strSPNRNO;
                string strWfare = "YES";
                string strWlogo = "YES";
                string strWbreakup = "YES";
                string strWSF = "YES";
                bool blnWfare = false;
                bool blnWlogo = false;
                bool blnWbreakup = false;
                bool blnWSF = false;
                bool chksingletkt = false;
                int TktCount = 0;
                if (strSPNRNO.Contains("|"))
                {
                    SPNRNO = strSPNRNO.Split('|')[0].ToString();
                    strWfare = strSPNRNO.Split('|')[1].ToString().ToUpper();
                    if (strSPNRNO.Split('|').Length > 3)
                    {
                        strWlogo = strSPNRNO.Split('|')[2].ToString().ToUpper();
                        strWbreakup = strSPNRNO.Split('|')[3].ToString().ToUpper();
                        strWSF = strSPNRNO.Split('|')[4].ToString().ToUpper();
                    }
                }
                ViewBag.SPNR = strSPNRNO;
                if (strWfare.ToUpper().Trim() == "YES")
                {
                    blnWfare = true;
                }
                if (strWlogo.ToUpper().Trim() == "YES")
                {
                    blnWlogo = true;
                }
                if (strWbreakup.ToUpper().Trim() == "YES")
                {
                    blnWbreakup = true;
                }
                if (strWSF.ToUpper().Trim() == "YES")
                {
                    blnWSF = true;
                }
                if (strSingle.ToUpper().Trim() == "YES")
                {
                    chksingletkt = true;
                }

                string XMl = "<EVENT><REQUEST><FUNCTION>pdforhtml_Click</FUNCTION><MAILDTO>" + txtEmailID + "</MAILDTO><PNR>" + SPNRNO + "</PNR><WITHFARE>" + strWfare + "</WITHFARE><WITHLOGO>"
                    + strWlogo + "</WITHLOGO><WITHBREAKUP>" + blnWbreakup + "</WITHBREAKUP><WITHSF>" + blnWSF + "</WITHSF><SINGLETKT>" + strSingle + "</SINGLETKT><PDF>" + pdf + "</PDF></REQUEST></EVENT>";
                DatabaseLog.LogData(strUserName, "E", "AfterBookingController.cs", "TicketMailReq - " + strSPNRNO, XMl.ToString(), strAgentID, strTerminalId, strSequenceid);

                #region SERVICE URL BRANCH BASED -- STS115
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (strClientBranchID != "" && strBranchCredit.Contains(strClientBranchID)))
                    {
                        _rays_servers.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                        _INPLANTSERVICE.Url = ConfigurationManager.AppSettings["TOPUP_APPS_INPLANT_SERVICE"].ToString();
                    }
                    else
                    {
                        _rays_servers.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                        _INPLANTSERVICE.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString() : ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                    }
                }
                else
                {
                    _rays_servers.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    _INPLANTSERVICE.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString() : ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                }
                #endregion         

                if (ConfigurationManager.AppSettings["SENDSMS"].ToString() == "Y" && !string.IsNullOrEmpty(strMobNumber))
                {
                    if (SPNRNO.Contains("/"))
                    {
                        for (int k = 0; k < SPNRNO.Split('/').Length; k++)
                        {
                            Sendsms(strSPNRNO.Split('/')[k].Trim(), strMobNumber, "B", strPaymentmode);
                        }
                    }
                    else
                    {
                        Sendsms(strSPNRNO.Trim(), strMobNumber, "B", strPaymentmode);
                    }
                }
                StringBuilder StrTicket = new StringBuilder();

                if (ConfigurationManager.AppSettings["Producttype"].ToString() == "ROUNDTRIP")
                {
                    StrTicket.Append("<div class=\"Thanku\">");
                    StrTicket.Append("<h4 style=\"color: #2e3246;\">Dear Customer,</h4>");
                    StrTicket.Append("<p style=\"margin-bottom: 20px;font-size: 18px;font-weight: 600;color: #4CAF50;\">Thank you for booking your ticket with us.</p>");
                    StrTicket.Append("<ul style=\"color: #565656;margin: 0px;\">");
                    StrTicket.Append("<li><p style=\"color: #072562;font-size: 15px;margin: 5px 0px;\">Please find enclosed the details of your trip with the PNR.</p></li>");
                    StrTicket.Append("<li><p style=\"color: #072562;font-size: 15px;margin: 5px 0px;\">Please be kind enough to carry the e-ticket with a valid photo identification proof.</p></li>");
                    StrTicket.Append("</ul>");
                    StrTicket.Append("<p style=\"font-size: 15px;margin-top: 30px;\">If you may require any further assistance, please be in touch with the contact details provided in the ticket copy.</p><br>");
                    StrTicket.Append("<p style=\"font-size: 16px;font-weight: 600;color: #2e3246;margin-top: 0px;\">Wishing you a pleasant and comfortable trip.</p>");
                    StrTicket.Append("<br>");
                    StrTicket.Append("<p style=\"font-size:14px;margin-bottom: 0;\">Thanks and Regards,</p>");
                    StrTicket.Append("<p style=\"font-size: 15px;line-height:1.5;margin-top: 5px;\"><a href=\"https://roundtrip.in\" style=\"text-decoration: none;font-size: 20px;\"><b><span style=\"color:#d60015;\">Round</span><span style=\"color:#170079;\">trip.in</span></b> </a><br> 83, Sydenhams Road, Periamet<br> Chennai – 600003.</p>");
                    StrTicket.Append("</div>");

                }
                else
                {
                    StrTicket.Append("<div class=\"Thanku\">");
                    StrTicket.Append("<h4 style=\"color: #2e3246;\">Dear Customer,</h4>");
                    StrTicket.Append("<p style=\"margin-bottom: 20px;font-size: 16px;font-weight: 600;color: #4CAF50;\">Thank you for booking your ticket with us.</p>");
                    StrTicket.Append("<ul style=\"color: #565656;margin: 0px;\">");
                    StrTicket.Append("<li><p style=\"color: #072562;font-size: 14px;margin: 5px 0px;\">Please find enclosed the details of your trip with the PNR.</p></li>");
                    StrTicket.Append("<li><p style=\"color: #072562;font-size: 14px;margin: 5px 0px;\">Please be kind enough to carry the e-ticket with a valid photo identification proof.</p></li>");
                    StrTicket.Append("</ul>");
                    StrTicket.Append("<p style=\"font-size: 14px;margin-top: 30px;\">If you may require any further assistance, please be in touch with the contact details provided in the ticket copy.</p>");
                    StrTicket.Append("<p style=\"font-size: 16px;font-weight: 600;color: #2e3246;margin-top: 0px;\">Wishing you a pleasant and comfortable trip.</p>");
                    StrTicket.Append("<br>");
                    StrTicket.Append("<br>");
                    StrTicket.Append("<label style='color:red;font-size:14px;font-weight: 600'>NOTE : This is an auto generated email and please do not reply back.</label><br /><br />");
                    StrTicket.Append("</div>");

                    //StrTicket.Append("<div>Dear Customer,<br />");

                    //StrTicket.Append("<div style='font-size:14px;color:#33a7e7'>Thank you for booking your ticket with us.<br />");
                    //StrTicket.Append("Please find enclosed the details of your trip with the PNR.<br />");
                    //StrTicket.Append("Please be kind enough to carry the e-ticket with a valid photo identification proof.<br />");
                    //StrTicket.Append("If you may require any further assistance, please be in touch with the contact details provided in the ticket copy.<br /></div>");
                    //StrTicket.Append("<label style='font-size:12px;color:#33a7e7'>Wishing you a pleasant and comfortable trip.</label><br />");
                    //StrTicket.Append("<label style='color:red;font-size:12px'>NOTE : This is an auto generated email and please do not reply back.</label><br /><br />");

                    //StrTicket.Append("</div><br /><br />");
                }


                ticket = StrTicket.ToString();

                string PrinTicket1 = string.Empty;
                string MessageToCustomer = string.Empty;
                string printpaxnumber = (Session["hdnpaxprint"] != null && Session["hdnpaxprint"].ToString() != "") ? Session["hdnpaxprint"].ToString() : "";
                if (printpaxnumber != null && printpaxnumber != "")
                {
                    printpaxnumber = printpaxnumber.Split('|')[0].TrimEnd(',');
                }
                string[] paxdetails = (printpaxnumber != null && printpaxnumber != "") ? printpaxnumber.Split(',') : new string[] { };
                string StFlag = strSingle.ToString().ToUpper() == "YES" ? "ST" : "MT";

                string GetTimeZone = ConfigurationManager.AppSettings["Servertimezone"].ToString();
                string TerminalApp = Session["TerminalType"] != null && Session["TerminalType"].ToString() != "" ? Session["TerminalType"].ToString() : "";
                DataSet dspdf = new DataSet();

                if (SPNRNO.Contains("/"))
                {
                    for (int i = 0; i < SPNRNO.Split('/').Count(); i++)
                    {
                        if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "RIYA")
                        {
                            _INPLANTSERVICE.PrintTicket_Riya(strAgentID, SPNRNO.Split('/')[i].Trim(), "", "", chksingletkt, blnWlogo, blnWfare, blnWbreakup, blnWSF, blntktmailflag, strTerminalId, strUserName, Ipaddress, "W", Convert.ToDecimal(strSequenceid), ref TktCount, ref TickectCopy, ref MessageToCustomer, paxdetails);
                            PrinTicket1 = TickectCopy;
                        }
                        else if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "TRAVRAYS")
                        {
                            bool ret_tickt = _rays_servers.PrintTicket_Travrays(strAgentID, SPNRNO.Split('/')[i].ToString().Trim(), "", "", GetTimeZone, blntktmailflag, blnWfare, blnWSF, blnWSF, strTerminalId, strUserName, Ipaddress, TerminalApp, Convert.ToDecimal(strSequenceid), getprevilage, getAgentType, StFlag, ref TickectCopy, ref MessageToCustomer, paxdetails, ref dspdf);
                            if (ret_tickt == true && TickectCopy != "")
                                PrinTicket1 = TickectCopy;
                        }
                        else if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "ROUNDTRIP")
                        {
                            bool ret_tickt = _rays_servers.PrintTicket_RoundTrip(strAgentID, SPNRNO.Split('/')[i].ToString().Trim(), "", "", chksingletkt, blnWlogo, blnWfare, blnWbreakup, blnWSF, blntktmailflag, strTerminalId,
                                    strUserName, Ipaddress, TerminalApp, Convert.ToDecimal(strSequenceid), ref TktCount, ref TickectCopy,
                                     ref MessageToCustomer, paxdetails, strPlatform, "", "", "");
                            if (ret_tickt == true && TickectCopy != "")
                                PrinTicket1 = TickectCopy;
                        }

                        if (PrinTicket1 != "" && txtEmailID.Trim() != "")
                        {
                            bool blnmail = Base.AsyncSendMail(pdf, PrinTicket1, txtEmailID, chksingletkt, SPNRNO.Split('/')[i].Trim(), ticket, Convert.ToInt32(TktCount));
                            if (blnmail)
                            {
                                stu = "1";
                                msg = "Your mail has been sent successfully";
                                Charge[Array_Result] = "Your mail has been sent successfully";
                            }
                            else
                            {
                                stu = "0";
                                msg = "Mail sending failure. Please contact customer care";
                                Charge[Array_Error] = "Mail sending failure. Please contact customer care";
                            }
                            xml = "<RESPONSE>" + "<EMAIL>" + txtEmailID + "</EMAIL>" + "<PRINT>" + (string.IsNullOrEmpty(PrinTicket1) ? "EMPTY" : "NOT EMPTY") + "</PRINT>"
                                    + "<RESULT>" + blnmail + "</RESULT>" + "</RESPONSE>";
                        }
                        else
                        {
                            stu = "0";
                            msg = "Mail sending failure. Please contact customer care";
                            xml = "<RESPONSE>" + "<EMAIL>" + txtEmailID + "</EMAIL>" + "<PRINT>" + (string.IsNullOrEmpty(PrinTicket1) ? "EMPTY" : "NOT EMPTY") + "</PRINT>" + "</RESPONSE>";
                        }
                        DatabaseLog.LogData(strUserName, "E", "AfterBookingController.cs", "btnEmail_pdforhtml_Click - " + SPNRNO.Split('/')[i].Trim(), xml, strAgentID, strTerminalId, strSequenceid);
                    }
                }
                else
                {
                    if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "RIYA")
                    {
                        _INPLANTSERVICE.PrintTicket_Riya(strAgentID, SPNRNO.ToString().Trim(), "", "", chksingletkt, blnWlogo, blnWfare, blnWbreakup, blnWSF, blntktmailflag, strTerminalId, strUserName, Ipaddress, "W", Convert.ToDecimal(strSequenceid), ref TktCount, ref TickectCopy, ref MessageToCustomer, paxdetails);
                        PrinTicket1 = TickectCopy;
                    }
                    else if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "TRAVRAYS")
                    {
                        bool ret_tickt = _rays_servers.PrintTicket_Travrays(strAgentID, SPNRNO.ToString().Trim(), "", "", GetTimeZone, blntktmailflag, blnWfare, blnWSF, blnWSF, strTerminalId, strUserName, Ipaddress, TerminalApp, Convert.ToDecimal(strSequenceid), getprevilage, getAgentType, StFlag, ref TickectCopy, ref MessageToCustomer, paxdetails, ref dspdf);
                        if (ret_tickt == true && TickectCopy != "")
                            PrinTicket1 = TickectCopy;
                    }
                    else if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "ROUNDTRIP")
                    {
                        bool ret_tickt = _rays_servers.PrintTicket_RoundTrip(strAgentID, SPNRNO.ToString().Trim(), "", "", chksingletkt, blnWlogo, blnWfare, blnWbreakup, blnWSF, blntktmailflag, strTerminalId,
                                strUserName, Ipaddress, TerminalApp, Convert.ToDecimal(strSequenceid), ref TktCount, ref TickectCopy,
                                 ref MessageToCustomer, paxdetails, strPlatform, "", "", "");
                        if (ret_tickt == true && TickectCopy != "")
                            PrinTicket1 = TickectCopy;
                    }

                    if (PrinTicket1 != "" && txtEmailID.Trim() != "")
                    {
                        bool blnmail = Base.AsyncSendMail(pdf, PrinTicket1, txtEmailID, chksingletkt, SPNRNO.Trim(), ticket, Convert.ToInt32(TktCount));
                        if (blnmail)
                        {
                            stu = "1";
                            msg = "Your mail has been sent successfully";
                            Charge[Array_Result] = "Your mail has been sent successfully";
                        }
                        else
                        {
                            stu = "0";
                            msg = "Mail sending failure. Please contact customer care";
                            Charge[Array_Error] = "Mail sending failure. Please contact customer care";
                        }
                        xml = "<RESPONSE>" + "<EMAIL>" + txtEmailID + "</EMAIL>" + "<PRINT>" + (string.IsNullOrEmpty(PrinTicket1) ? "EMPTY" : "NOT EMPTY") + "</PRINT>"
                                + "<RESULT>" + blnmail + "</RESULT>" + "</RESPONSE>";
                    }
                    else
                    {
                        stu = "0";
                        msg = "Mail sending failure. Please contact customer care";
                        xml = "<RESPONSE>" + "<EMAIL>" + txtEmailID + "</EMAIL>" + "<PRINT>" + (string.IsNullOrEmpty(PrinTicket1) ? "EMPTY" : "NOT EMPTY") + "</PRINT>" + "</RESPONSE>";
                    }
                    DatabaseLog.LogData(strUserName, "E", "AfterBookingController.cs", "TicketMailRes - " + SPNRNO.Trim(), xml, strAgentID, strTerminalId, strSequenceid);
                }
            }
            catch (Exception ex)
            {
                stu = "0";
                msg = "Mail sending failure. Please contact customer care";
                Charge[Array_Error] = "Mail Sending is Failure..";
                DatabaseLog.LogData(strUserName, "X", "AfterBookingController.cs", "TicketMailErr", ex.ToString(), strAgentID, strTerminalId, strSequenceid);
            }
            return Json(new { Status = stu, Message = msg, Result = Charge });
        }
        #endregion

        #region PDF CONVERSION
        [WebMethod(Description = "pdfTktconverter", EnableSession = true)]
        public ActionResult pdfTktconverter(string strPNR)
        {

            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strResult = string.Empty;
            string strLogData = string.Empty;
            string strUserName = Session["username"] != null  ? Session["username"].ToString() : "";
            string strAgentID = Session["POS_ID"] != null  ? Session["POS_ID"].ToString() : "";
            string strTerminalID = Session["POS_TID"] != null  ? Session["POS_TID"].ToString() : "";
            string strSequesnceID = Session["sequenceid"] != null  ? Session["sequenceid"].ToString() : "";
            try
            {
                if (strPNR == null || strPNR == "")
                {
                    strStatus = "-1"; strMessage = "";
                    return Json(new { Status = strStatus, Message = strMessage, Result = strResult });
                }

                strPNR = strPNR.Contains('/') ? strPNR.Split('/')[0].Trim() : strPNR;
                string strPDFContent = Session["pdfsession" + strPNR].ToString();
                strPDFContent = Base.ChangeToLanIP(strPDFContent);
                string pfname = HttpContext.Server.MapPath("~/PDF/" + strPNR + ".pdf");

                if (System.IO.File.Exists(pfname) && Request.Browser.IsMobileDevice)
                {
                    string BarCodePath = HttpContext.Server.MapPath("~/PDF/" + strPNR.Split('/')[0].Trim() + ".pdf"); //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PDF\\" + strRiyaPNR + ".pdf");
                    System.IO.File.Delete(BarCodePath);
                }
                if (!System.IO.File.Exists(pfname))
                {
                    var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                    var pdfBytes = htmlToPdf.GeneratePdf(strPDFContent);
                    System.IO.File.WriteAllBytes(pfname, pdfBytes);
                    string strurl = ConfigurationManager.AppSettings["DOMAIN_URL"].ToString() + "/PDF/" + strPNR.Split('/')[0].Trim() + ".pdf";
                    strStatus = "00";
                    strResult = strurl;
                }
                else
                {
                    string BarCodePath = HttpContext.Server.MapPath("~/PDF/" + strPNR.Split('/')[0].Trim() + ".pdf"); //Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PDF\\" + strRiyaPNR + ".pdf");
                    System.IO.File.Delete(BarCodePath);
                    strStatus = "01";
                    strMessage = "Unable to Convert PDF";
                }

                strLogData = "<EVENT><PNR>" + strPNR + "</PNR><STATUS>" + strStatus + "</STATUS><MESSAGE>" + strMessage + "</MESSAGE><RESULT>" + strResult + "</RESULT></EVENT>";
                DatabaseLog.LogData(strUserName, "E", "pdfTktconverter", "pdfTktconverter ", strLogData, strAgentID, strTerminalID, strSequesnceID);
            }
            catch (Exception ex)
            {
                strStatus = "05";
                strMessage = "Problem occured while converting. Please try again later(#05).";
                strLogData = "<EVENT><PNR>" + strPNR + "</PNR><ERRMSG>" + ex.ToString() + "</ERRMSG></EVENT>";
                DatabaseLog.LogData(strUserName, "X", "pdfTktconverter", "pdfTktconverter ", strLogData, strAgentID, strTerminalID, strSequesnceID);
            }
            return Json(new { Status = strStatus, Message = strMessage, Result = strResult });
        }
        #endregion

        #region WORD CONVERTION
        public void wordTktconverter()
        {
            string strAgentID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string strTerminalId = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strUserName = Session["username"] != null && Session["username"].ToString() != "" ? Session["username"].ToString() : "";
            string Ipaddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string getprevilage = Session["privilage"] != null && Session["privilage"].ToString() != "" ? Session["privilage"].ToString() : "";
            string strTerminalType = Session["TerminalType"] != null ? Session["TerminalType"].ToString() : "";

            if (getprevilage.ToString() == "S")
                strTerminalId = string.Empty;
            else
                strTerminalId = Session["POS_TID"].ToString();

            if ("T" == strTerminalType)
            {
                strAgentID = string.Empty;
                strTerminalId = string.Empty;
            }
            try
            {
                if (Session["printtkt"] != null && Session["printtkt"].ToString() != "")
                {
                    object sdss = Session["printtkt"].ToString();
                    StringBuilder strHTMLContent = new StringBuilder();
                    strHTMLContent.Append(sdss);
                    iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
                    Response.ContentType = "application/vnd.ms-word";
                    Response.AddHeader("content-disposition", "attachment;filename=Ticketcopy.doc");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Output.Write(strHTMLContent.ToString());
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Print Word", "Word Downlod Error", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

            }
        }
        #endregion

        #region SEND SMS BOOKING AND AFTER BOOKING
        public ActionResult Sendsms(string strSPNRNO, string MobileNumber, string Flag, string strPaymentmode)
        {
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strSequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0";
            string AgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string strTerminalType = Session["TerminalType"] != null ? Session["TerminalType"].ToString() : "";
            string strClientBranchID = Session["ClientBranchID"] != null && Session["ClientBranchID"].ToString() != "" ? Session["ClientBranchID"].ToString() : "";
            string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"].ToString() != "") ? Session["TKTFLAG"].ToString() : "DTKT";
            string strAgentLogin = Session["IsAgentLogin"] != null && Session["IsAgentLogin"].ToString() != "" ? Session["IsAgentLogin"].ToString() : "N";
            string strCustomerLogin = Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() != "" ? Session["CustomerLogin"].ToString() : "N";

            byte[] dsViewPNR_compress = new byte[] { };
            byte[] dsFareDispDet_compress = new byte[] { };
            DataSet Ds_Tab = new DataSet();
            StringBuilder strBuildSMS = new StringBuilder();
            string strPlatform = string.Empty;
            string smsAirlineName = string.Empty;
            string smsFlight = string.Empty;
            string smsSPNR = string.Empty;
            string smsDepDate = string.Empty;
            string smsOrigin = string.Empty;
            string smsDepTime = string.Empty;
            string smsDestination = string.Empty;
            string xml = string.Empty;
            string Status = string.Empty;
            string Error = string.Empty;
            string refstring = string.Empty;
            try
            {
                #region SERVICE URL BRANCH BASED -- STS195
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (strClientBranchID != "" && strBranchCredit.Contains(strClientBranchID)))
                    {
                        _rays_servers.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                        _INPLANTSERVICE.Url = ConfigurationManager.AppSettings["TOPUP_APPS_INPLANT_SERVICE"].ToString();
                    }
                    else
                    {
                        _rays_servers.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                        _INPLANTSERVICE.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString() : ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                    }
                }
                else
                {
                    _rays_servers.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    _INPLANTSERVICE.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_INPLANT_SERVICE"].ToString() : ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                }
                #endregion

                string refError = "ERROR";
                string strErrorMsg = "ERROR";

                xml = "<EVENT><TOSMS>" + MobileNumber + "</TOSMS>"
                      + "<agentid>" + strAgentID + "</agentid>"
                      + "<terminalid>" + strTerminalId + "</terminalid>"
                      + "<username>" + strUserName + "</username>"
                      + "<ipAddress>" + Ipaddress + "</ipAddress>"
                      + "<sequenceid>" + strSequnceID + "</sequenceid>"
                      + "<Spnr>" + strSPNRNO + "</Spnr></EVENT>";

                DatabaseLog.LogData(strUserName, "E", "AfterBookingController", "SMS Ticket - Load", xml.ToString(), strAgentID, strTerminalId, strSequnceID);

                bool res = _rays_servers.Fetch_PNR_Verification_Details_NewByte(strAgentID, strSPNRNO, "", "", "", strTerminalId, strUserName, Ipaddress, "W", Convert.ToDecimal(strSequnceID), ref dsViewPNR_compress, ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "Fetchviewpnr", "Fetchviewpnrdetails", AgentType, strTerminalId);

                if (res == true)
                {
                    Ds_Tab = Base.Decompress(dsViewPNR_compress);
                    int segCount = 0;
                    var qryTemp = (from p in Ds_Tab.Tables[0].AsEnumerable()
                                   where p["TICKET_STATUS"].ToString().ToUpper() != "CANCELLED"
                                   select new
                                   {
                                       FLIGHT = p["FLIGHT_NO"].ToString().Substring(3).ToString().Trim(),
                                       DEPARTURE_DATE = Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(p.Field<DateTime>("DEPT_DATE").ToString()), "dd/MM/yyyy"),
                                       DEPARURE_TIME = Base.ServiceUtility.CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(p.Field<DateTime>("DEPT_DATE").ToString()), "HH:mm"),
                                       AIRLINECODE = p["FLIGHT_NO"].ToString().Substring(0, 2),
                                       AIRLINEPNR = p["AIRLINE_PNR"].ToString(),
                                       DESTINATION = p["DESTINATIONCODE"].ToString(),
                                       ORIGIN = p["ORIGINCODE"].ToString()
                                   }).Distinct();

                    foreach (var qry in qryTemp)
                    {
                        if (segCount == 0)
                        {
                            smsFlight = qry.FLIGHT.ToString().Trim();
                            smsDepDate = qry.DEPARTURE_DATE.ToString();
                            smsOrigin = qry.ORIGIN.ToString().Trim();
                            smsDepTime = qry.DEPARURE_TIME.ToString();
                            smsAirlineName = qry.AIRLINECODE;
                            smsSPNR = qry.AIRLINEPNR.ToString();
                        }
                        if (segCount == (qryTemp.Count() - 1))
                        {
                            smsDestination = qry.DESTINATION.ToString().Trim();
                        }
                        segCount++;
                    }

                    if (ConfigurationManager.AppSettings["Appname"].ToString().ToUpper() == "ROUNDTRIP")
                    {
                        strBuildSMS.Append("Your booking from " + smsOrigin + " to " + smsDestination + " has been confirmed. PNR " + smsSPNR + " Airline " +
                                    smsAirlineName + " Flight No " + smsFlight + " Dep " + smsDepDate + " Time " + smsDepTime + ". Thank you for booking with us. PLEASE NOTE- WEB CHECKIN IS NOW MANDATORY");
                    }
                    else
                    {
                        strBuildSMS.Append("TICKET CONFIRMATION: ");
                        strBuildSMS.Append(" Flight " + smsFlight + " Date-" + smsDepDate + " " + smsOrigin + "-" +
                                   smsDestination + " Dep:" + smsDepTime + "hrs " + smsAirlineName + " REF-" + smsSPNR +
                                   ".");
                        strBuildSMS.Append(" Have a pleasant journey.");
                    }
                    if (qryTemp.Count() == 0)
                    {
                        return Json(new { Error = "Confirmed ticket only allow to send SMS", Status = "00" });
                    }

                    strPlatform = strAgentLogin != "Y" ? "B2C" : "B2B";
                    bool smsres = _rays_servers.Send_SMS_Riya(strAgentID, strTerminalId, MobileNumber, strBuildSMS.ToString(), ref refstring, strUserName, "AIR", Ipaddress, strTerminalType, Convert.ToDecimal(strSequnceID == "" ? "0" : strSequnceID), ref refError, strPlatform);
                    xml = "<EVENT><SMSRESPONSE>" + smsres + "</SMSRESPONSE></EVENT>";
                    DatabaseLog.LogData(strUserName, "E", "AfterBookingController", "SMS Ticket - FETCH", xml.ToString(), strAgentID, strTerminalId, strSequnceID);
                    if (Flag == "V")
                    {
                        if (smsres == true)
                        {
                            Status = "01";
                            Error = "SMS send successfully.";
                        }
                        else
                        {
                            Status = "00";
                            Error = "Unable to send SMS.";
                        }
                    }
                }
                else
                {
                    Status = "00";
                    Error = "Unable to send SMS.";
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Sms Ticket", "Send SMS - EXCCEPTIon", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                Error = "Problem occured,while sending SMS";
                Status = "05";
            }
            return Json(new { Error = Error, Status = Status });
        }
        #endregion

        #region TICKETCOPY DOWNLOAD HTML
        public ActionResult downloadhtml(string strPNR)
        {
            ArrayList arr = new ArrayList();
            int result = 1;
            int error = 0;
            arr.Add("");
            arr.Add("");
            int TktCount = 0;
            string strResult = string.Empty;
            string MessageToCustomer = string.Empty;
            string[] paxdetails = new string[] { };
            string strAgentID = Session["POS_ID"].ToString();
            string strTerminalId = Session["POS_TID"].ToString();
            string strUserName = Session["username"].ToString();
            string Ipaddress = Session["ipAddress"].ToString();
            string getprevilage = Session["privilage"].ToString();
            string getAgentType = Session["agenttype"].ToString();
            string ProductType = ConfigurationManager.AppSettings["Producttype"].ToString();

            string TerminalApp = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            if (getprevilage.ToString() == "S")
                strTerminalId = string.Empty;
            else
                strTerminalId = Session["POS_TID"].ToString();

            if ("T" == TerminalApp)
            {
                strAgentID = string.Empty;
                strTerminalId = string.Empty;
            }
            try
            {
                if (Session["printtkt"] != null && Session["printtkt"].ToString() != "")
                {
                    arr[result] = Session["printtkt"].ToString();
                }
                else
                {
                    if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "RIYA")
                    {
                        _INPLANTSERVICE.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                        _INPLANTSERVICE.PrintTicket_Riya(strAgentID, strPNR.ToString().Trim(), "", "", false, false, false, false, false, false, strTerminalId, strUserName, Ipaddress, TerminalApp, Convert.ToDecimal(Session["sequenceid"].ToString()), ref TktCount, ref strResult, ref MessageToCustomer, paxdetails);
                        arr[result] = strResult;
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Download HTML", "Download HTML", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            return Json(new { Status = "", Message = "", Result = arr });
        }
        #endregion

        #region GET TICKET COPY
        public ActionResult GetTicketCopy(string strSPNRNO, string single)
        {
            bool MyResult = true;
            string str_tickref = string.Empty;
            DataSet dspdf = new DataSet();
            string strResult = string.Empty;
            string MessageToCustomer = string.Empty;
            string strPlatform = string.Empty;
            string strStatus = string.Empty;

            string strUserName = Session["username"] != null && Session["username"].ToString() != "" ? Session["username"].ToString() : "";
            string strTerminalId = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strAgentID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string strSequnceID = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyMMdd");
            string getAgentType = Session["agenttype"] != null && Session["agenttype"].ToString() != "" ? Session["agenttype"].ToString() : "";
            string getprevilage = Session["privilage"] != null && Session["privilage"].ToString() != "" ? Session["privilage"].ToString() : "";
            string Ipaddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strTerminalType = Session["TerminalType"] != null && Session["TerminalType"].ToString() != "" ? Session["TerminalType"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strClientBranchID = Session["ClientBranchID"] != null && Session["ClientBranchID"].ToString() != "" ? Session["ClientBranchID"].ToString() : "";
            string strAgentLogin = Session["IsAgentLogin"] != null && Session["IsAgentLogin"].ToString() != "" ? Session["IsAgentLogin"].ToString() : "N";

            try
            {
                strPlatform = strAgentLogin != "Y" ? "B2C" : "B2B";
                string printpaxnumber = (Session["hdnpaxprint"] != null && Session["hdnpaxprint"].ToString() != "") ? Session["hdnpaxprint"].ToString() : "";
                if (printpaxnumber != null && printpaxnumber != "")
                {
                    printpaxnumber = printpaxnumber.Split('|')[0].TrimEnd(',');
                }

                ViewBag.hdnsingle = single;
                if (strSPNRNO != null && strSPNRNO.Contains("|"))
                {
                    string[] paxdetails = (printpaxnumber != null && printpaxnumber != "") ? printpaxnumber.Split(',') : new string[] { };
                    int TktCount = 0;
                    string strErrorMsg = "ERROR";
                    string SPNRNO = strSPNRNO;
                    string strWfare = "YES";
                    string strWlogo = "YES";
                    string strWbreakup = "YES";
                    string strWSF = "YES";
                    bool blnWfare = false;
                    bool blnWlogo = false;
                    bool blnWbreakup = false;
                    bool blnWSF = false;
                    bool chksingletkt = false;
                    if (strSPNRNO.Contains("|"))
                    {
                        SPNRNO = strSPNRNO.Split('|')[0].ToString();
                        strWfare = strSPNRNO.Split('|')[1].ToString().ToUpper();
                        ViewBag.Wfare = strSPNRNO.Split('|')[1].ToString();
                        ViewBag.hdnSpnr_U = strSPNRNO;
                        ViewBag.Without_farechack = SPNRNO;
                        if (strSPNRNO.Split('|').Length > 2)
                        {
                            strWlogo = strSPNRNO.Split('|')[2].ToString().ToUpper();
                        }
                        if (strSPNRNO.Split('|').Length > 3)
                        {
                            strWbreakup = strSPNRNO.Split('|')[3].ToString().ToUpper();
                            strWSF = strSPNRNO.Split('|')[4].ToString().ToUpper();
                        }
                    }
                    ViewBag.SPNR = strSPNRNO;
                    if (strWfare.ToUpper().Trim() == "YES")
                    {
                        blnWfare = true;
                    }
                    if (strWlogo.ToUpper().Trim() == "YES")
                    {
                        blnWlogo = true;
                    }
                    if (strWbreakup.ToUpper().Trim() == "YES")
                    {
                        blnWbreakup = true;
                    }
                    if (strWSF.ToUpper().Trim() == "YES")
                    {
                        blnWSF = true;
                    }
                    if (single.ToUpper().Trim() == "YES")
                    {
                        chksingletkt = true;
                    }
                    if (getprevilage.ToString() == "S")
                        strTerminalId = string.Empty;

                    if (strTerminalType == "T")
                    {
                        strAgentID = string.Empty;
                        strTerminalId = string.Empty;
                    }

                    #region SERVICE URL BRANCH BASED -- STS195
                    if (strBranchCredit != "")
                    {
                        if (strBranchCredit == "ALL" || (strClientBranchID != "" && strBranchCredit.Contains(strClientBranchID)))
                        {
                            _rays_servers.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                            _INPLANTSERVICE.Url = ConfigurationManager.AppSettings["TOPUP_APPS_INPLANT_SERVICE"].ToString();
                        }
                        else
                        {
                            _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                            _INPLANTSERVICE.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                        }
                    }
                    else
                    {
                        _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                        _INPLANTSERVICE.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                    }
                    #endregion


                    if (SPNRNO != null && SPNRNO != "")
                    {
                        SPNRNO = SPNRNO.Trim();
                    }
                    string[] strPNR = SPNRNO.Split('/');
                    string Appendticket = string.Empty;
                    for (int i = 0; i < strPNR.Length; i++)
                    {
                        if (strPNR[i].ToString().Trim() != "")
                        {
                            if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "RIYA")
                            {
                                MyResult = _INPLANTSERVICE.PrintTicket_Riya(strAgentID, strPNR[i].ToString().Trim(), "", "", chksingletkt, blnWlogo, blnWfare, blnWbreakup, blnWSF, false, strTerminalId, strUserName, Ipaddress, strTerminalType, Convert.ToDecimal(strSequnceID), ref TktCount, ref strResult, ref MessageToCustomer, paxdetails);
                            }
                            else if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "TRAVRAYS")
                            {
                                string StFlag = single.ToString().ToUpper() == "YES" ? "ST" : "MT";
                                string GetTimeZone = ConfigurationManager.AppSettings["Servertimezone"].ToString();
                                MyResult = _rays_servers.PrintTicket_Travrays(strAgentID, strPNR[i].ToString().Trim(), "", "", GetTimeZone, false, blnWfare, false, blnWSF, strTerminalId, strUserName, Ipaddress, strTerminalType, Convert.ToDecimal(strSequnceID), getprevilage, getAgentType, StFlag, ref strResult, ref strErrorMsg, paxdetails, ref dspdf);
                            }
                            else if (ConfigurationManager.AppSettings["PRINTTICKETFORMAT"].ToString() == "ROUNDTRIP")
                            {
                                MyResult = _rays_servers.PrintTicket_RoundTrip(strAgentID, strPNR[i].ToString().Trim(), "", "", chksingletkt, blnWlogo, blnWfare, blnWbreakup, blnWSF, false, strTerminalId,
                                        strUserName, Ipaddress, strTerminalType, Convert.ToDecimal(strSequnceID), ref TktCount, ref strResult,
                                         ref MessageToCustomer, paxdetails, strPlatform, "", "", "");
                            }
                            if (i < strPNR.Length - 1)
                            {
                                strResult += "<p style=\"page-break-before: always;\"></p>";
                            }
                            Appendticket += strResult;
                        }
                    }
                    string status = string.IsNullOrEmpty(Appendticket) ? "EMPTY" : "SUCCESS";
                    string XMl = "<EVENT><REQUEST><FUNCTION>PrintTickets</FUNCTION><PNR>" + SPNRNO + "</PNR><WITHFARE>" + blnWfare + "</WITHFARE><WITHLOGO>" + blnWlogo + "</WITHLOGO><WITHBREAKUP>" + blnWbreakup + "</WITHBREAKUP><WITHSF>" + blnWSF + "</WITHSF><SINGLETKT>" + single + "</SINGLETKT></REQUEST><RESPONSE>" + status + "</RESPONSE></EVENT>";
                    DatabaseLog.LogData(strUserName, "E", "GetTicketCopy", "GetTicketCopy~" + SPNRNO, XMl.ToString(), strAgentID, strTerminalId, strSequnceID);
                    Session.Add("pdfsession" + strPNR[0].ToString().Trim(), Appendticket);
                    Session.Add("printtkt", Appendticket);
                    strStatus = Appendticket.Replace("<p style=\"page-break-before: always;\"></p>", "") != "" ? "01" : "00";
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(strUserName, "X", "GetTicketCopy", "GetTicketCopy~ERR", ex.ToString(), strAgentID, strTerminalId, Session["sequenceid"].ToString());
                strStatus = "00";
            }
            return Json(new { Status = strStatus });
        }
        #endregion

    }
}