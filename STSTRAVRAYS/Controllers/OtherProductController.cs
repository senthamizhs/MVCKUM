using System;
using System.Configuration;
using System.Web.Mvc;
using STSTRAVRAYS.Models;
using System.Data;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace STSTRAVRAYS.Controllers
{
    public class OtherProductController : Controller
    {
        public ActionResult OtherProduct(string Flag)
        {

            string strQuerystring = string.Empty;
            string Comingsoon = ConfigurationManager.AppSettings["COOMINGSOON"].ToString();
            string strAgentId = Session["agentid"] != null ? Session["agentid"].ToString() : "";
            string strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString() : "";
            string strUsername = Session["username"] != null ? Session["username"].ToString() : "";
            string strSequenceNo = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";

            string strIpaddres = string.Empty;
            string str_credit = string.Empty;
            string strTerminaltype = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strPrivilage = Session["privilage"] != null ? Session["privilage"].ToString() : "";
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strUserID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";

            string strCstnameID = string.Empty;
            string strPOS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";  // LOGIN AGENT ID
            string strPOS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : ""; // LOGIN TERMINAL ID
            string strBranch = Session["branchid"] != null ? Session["branchid"].ToString() : "";
            string Credit_train = string.Empty;
            string strMacaddress = Session["MACADDRESS"] != null ? Session["MACADDRESS"].ToString() : "";
            string strIRCTCUserName = Session["IRCTCUSERNAME"] != null ? Session["IRCTCUSERNAME"].ToString() : "";
            string strPassword = Session["PWD"] != null ? Session["PWD"].ToString() : "";
            string strAllowMUA = Session["UTILITY"] != null ? Session["UTILITY"].ToString() : "";
            string strAllowMTF = Session["MONEYTRANSFER"] != null ? Session["MONEYTRANSFER"].ToString() : "";
            string strSPNR_ANC = Session["PNRFLAG_ANC"] != null ? Session["PNRFLAG_ANC"].ToString() : "";
            string strAgentTerminalCount = Session["AGENTTERMINALCOUNT"] != null ? Session["AGENTTERMINALCOUNT"].ToString() : "";
            strIpaddres = Base.GetComputer_IP();
            str_credit = Session["creditacc"] != null ? Session["creditacc"].ToString() : "";
            string strSalesreportQuerystring = string.Empty;
            try
            {

                if (strPOS_ID == "" || strPOS_TID == "")
                {
                    return View("~/Views/Redirect/SessionExp.cshtml");
                }

                if (Flag == "BUS" || Flag == "BMB")
                    strQuerystring = "CLIENTID=" + strPOS_ID + "&TERMINALID=" + strTerminalId + "&USERNAME=" + strUsername + "&PWD=" + strPassword + "&CLIENTTYPE=" + strAgentType + "&SEQUENCEID=" + strSequenceNo + "&BRANCHID=" + strBranch + "&TERMINALPREVILAGE=" + strPrivilage;
                else if (Flag == "CAR" || Flag == "TMP" || Flag == "TRN" || Flag == "TRNMB")
                    strQuerystring = "CLIENTID=" + strPOS_ID + "&TERMINALID=" + strPOS_TID + "&USERNAME=" + strUsername + "&PWD=" + strPassword + "&CLIENTTYPE=" + strAgentType + "&SEQUENCEID=" + strSequenceNo + "&BRANCHID=" + strBranch + "&TERMINALPREVILAGE=" + strPrivilage;
                else if (Flag == "INS" || Flag == "IVP" || Flag == "ICL" || Flag == "IBH" || Flag == "TOS" || Flag == "TTC" || Flag == "CRU" || Flag == "BTK" || Flag.StartsWith("MB") || Flag == "RTAC")
                    strQuerystring = "CLIENTID=" + strPOS_ID + "&TERMINALID=" + strPOS_TID + "&USERNAME=" + strUsername + "&PWD=" + strPassword + "&CLIENTTYPE=" + strAgentType + "&SEQUENCEID=" + strSequenceNo + "&BRANCHID=" + strBranch + "&TERMINALPREVILAGE=" + strPrivilage + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres;
                else if (Flag == "ANC" || Flag == "BRL")
                    strQuerystring = "CLIENTID=" + strPOS_ID + "&TERMINALID=" + strPOS_TID + "&USERNAME=" + strUsername + "&PWD=" + strPassword + "&CLIENTTYPE=" + strAgentType + "&SEQUENCEID=" + strSequenceNo + "&BRANCHID=" + strBranch + "&TERMINALPREVILAGE=" + strPrivilage + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres;
                else if (Flag == "ANCV")
                    strQuerystring = "CLIENTID=" + strPOS_ID + "&TERMINALID=" + strPOS_TID + "&USERNAME=" + strUsername + "&PWD=" + strPassword + "&CLIENTTYPE=" + strAgentType + "&SEQUENCEID=" + strSequenceNo + "&BRANCHID=" + strBranch + "&TERMINALPREVILAGE=" + strPrivilage + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres + "&PNRFLAG=" + strSPNR_ANC;
                else if (Flag == "MUA")
                    strQuerystring = "AGENTID=" + strPOS_ID + "&TERMINALID=" + strPOS_TID + "&TERMINALTYPE=" + strTerminaltype + "&USERNAME=" + strUsername + "&PASSWORD=" + strPassword + "&AGENT_TYPE=" + strAgentType + "&IPADDRESS=" + strIpaddres + "&SEQUENCEID=" + strSequenceNo + "&BRANCHID=" + strBranch + "&ALLOWRIGHTS=" + strAllowMTF + "|" + strAllowMUA;
                else if (Flag == "PKG" || Flag == "PKGMB")
                    strQuerystring = "CLIENTID=" + strPOS_ID + "&TERMINALID=" + strPOS_TID + "&USERNAME=" + strUsername + "&PWD=" + strPassword + "&CLIENTTYPE=" + strAgentType + "&SEQUENCEID=" + strSequenceNo + "&BRANCHID=" + strBranch + "&TERMINALPREVILAGE=" + strPrivilage + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres;
                else if (Flag == "CSPNR")
                    strQuerystring = "CLIENTID=" + strPOS_ID + "&TERMINALID=" + strPOS_TID + "&USERNAME=" + strUsername + "&PWD=" + strPassword + "&TERMINALTYPE=" + strTerminaltype + "&BRANCHID=" + strBranch + "&TERMINALPREVILAGE=" + strPrivilage + "&IPADDRESS=" + strIpaddres + "&SEQUENCEID=" + strSequenceNo + "";
                else if (Flag == "WATUP" || Flag == "WABD" || Flag == "WASC" || Flag == "WAAE" || Flag == "WAAS")
                    strQuerystring = "CLIENTID=" + strPOS_ID + "&TERMINALID=" + strPOS_TID + "&USERNAME=" + strUsername + "&PWD=" + strPassword + "&TERMINALTYPE=" + strTerminaltype + " &BRANCHID=" + strBranch + "&TERMINALPREVILAGE=" + strPrivilage + "&IPADDRESS=" + strIpaddres + "&SEQUENCEID=" + strSequenceNo + "";
                else if (Flag == "ASR")
                    strSalesreportQuerystring = "AGENTID=" + strPOS_ID + "&TERMINALID=" + strPOS_TID + "&TERMINALPREVILAGE=" + strPrivilage + "&TERMINALTYPE=" + strTerminaltype + "&LOGINUSERNAME=" + strUsername + "&SEQUENCEID=" + strSequenceNo + "&BRANCHID=" + strBranch + "&AGENT_TYPE=" + strAgentType + "&TERMINALCOUNT=" + strAgentTerminalCount;

                string today = DateTime.Now.ToString("dd/MM/yyyy");
                string Querystring = "SECKEY=" + Base.EncryptKEy(strQuerystring, "RIYA" + today);

                ViewBag.HotelQuerystring = "tid=" + strTerminalId + "&agentid=" + strAgentId + "&username=" + strUsername + "&seq=" + strSequenceNo + "&IPA=" + strIpaddres + "&credit=" + str_credit + "&typ=" + strTerminaltype + "&privilage=" + strPrivilage + "&AGNTYPE="
                                           + strAgentType + "&userid=" + strUserID + "&companyId=" + strPOS_ID + "&costcenter=" + strCstnameID + "&strflag=" + "TDK&pos_tid=" + strPOS_TID + "&pos_id=" + strPOS_ID + "&branchid=" + strBranch + "&AGNTYPE=TB" + "&Referenceid=";

                #region BUS

                if (Flag == "BUS")
                {
                    if (ConfigurationManager.AppSettings["BUSURL"].ToString() != "")
                    {
                        string strBusurl = ConfigurationManager.AppSettings["BUSURL"].ToString();
                        if (ConfigurationManager.AppSettings["NewQuerystring"] != "" && ConfigurationManager.AppSettings["NewQuerystring"].ToString().Contains("BUS"))
                        {
                            ViewBag.OtherProductQSURL = strBusurl + "?" + Querystring + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres + "&POSTID=" + strPOS_TID + "&POSID=" + strPOS_ID + "&USERID= " + "&TRIPFLAG=" + "&TRIPDEST=" + "&ARIVEDATE=" + "&TRIPPAXCNT=" + "&COORDINATRAVEL=" + "&PLATFORM=B2B";
                        }
                        else if (ConfigurationManager.AppSettings["Producttype"] == "RIYA" || ConfigurationManager.AppSettings["Producttype"] == "JOD")
                        {
                            ViewBag.OtherProductQSURL = strBusurl + "?" + Querystring + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres + "&POSTID=" + strPOS_TID + "&POSID=" + strPOS_ID;
                        }
                        else
                        {
                            ViewBag.OtherProductQSURL = strBusurl + "?" + ViewBag.HotelQuerystring;
                        }
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "BMB")
                {
                    if (ConfigurationManager.AppSettings["BUSMB"].ToString() != "")
                    {
                        string strBusMBurl = ConfigurationManager.AppSettings["BUSMB"].ToString();
                        if (ConfigurationManager.AppSettings["NewQuerystring"] != "" && ConfigurationManager.AppSettings["NewQuerystring"].ToString().Contains("BUS"))
                        {
                            ViewBag.OtherProductQSURL = strBusMBurl + "?" + Querystring + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres + "&POSTID=" + strPOS_TID + "&POSID=" + strPOS_ID + "&USERID= " + "&TRIPFLAG=" + "&TRIPDEST=" + "&ARIVEDATE=" + "&TRIPPAXCNT=" + "&COORDINATRAVEL=" + "&PLATFORM=B2B";
                        }
                        else if (ConfigurationManager.AppSettings["Producttype"] == "RIYA" || ConfigurationManager.AppSettings["Producttype"] == "JOD")
                        {
                            ViewBag.OtherProductQSURL = strBusMBurl + "?" + Querystring + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres + "&POSTID=" + strPOS_TID + "&POSID=" + strPOS_ID;
                        }
                        else
                        {
                            ViewBag.OtherProductQSURL = strBusMBurl + "?" + ViewBag.HotelQuerystring;
                        }
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region INSURANCE

                else if (Flag == "I" || Flag == "INS")
                {
                    if (ConfigurationManager.AppSettings["INSURL"].ToString() != "")
                    {
                        string strINSURL = ConfigurationManager.AppSettings["INSURL"].ToString();
                        if (ConfigurationManager.AppSettings["Producttype"] == "RIYA")
                        {
                            ViewBag.OtherProductQSURL = strINSURL + "?" + Querystring;
                        }
                        else
                        {
                            ViewBag.OtherProductQSURL = strINSURL + "?" + ViewBag.HotelQuerystring; //Querystring // ViewBag.HotelQuerystring
                        }
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "IVP")
                {
                    if (ConfigurationManager.AppSettings["INSURL"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["INSURL_IVP"].ToString();
                        if (ConfigurationManager.AppSettings["Producttype"] == "RIYA")
                        {
                            ViewBag.OtherProductQSURL = strInsuranceurl + "?" + Querystring;
                        }
                        else
                        {
                            ViewBag.OtherProductQSURL = strInsuranceurl + ViewBag.HotelQuerystring;
                        }
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "IBH")
                {
                    if (ConfigurationManager.AppSettings["INSURL"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["INSURL_IBH"].ToString();
                        if (ConfigurationManager.AppSettings["Producttype"] == "RIYA")
                        {
                            ViewBag.OtherProductQSURL = strInsuranceurl + "?" + Querystring;
                        }
                        else
                        {
                            ViewBag.OtherProductQSURL = strInsuranceurl + ViewBag.HotelQuerystring;
                        }
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "ICL")
                {
                    if (ConfigurationManager.AppSettings["INSURL"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["INSURL_ICL"].ToString();
                        if (ConfigurationManager.AppSettings["Producttype"] == "RIYA")
                        {
                            ViewBag.OtherProductQSURL = strInsuranceurl + "?" + Querystring;
                        }
                        else
                        {
                            ViewBag.OtherProductQSURL = strInsuranceurl + ViewBag.HotelQuerystring;
                        }
                    }
                    else
                    {

                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }

                #endregion

                #region MOBILERECHARGE

                else if (Flag == "M")
                {
                    if (ConfigurationManager.AppSettings["mobileUrl_B"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["mobileUrl_B"].ToString();
                        ViewBag.OtherProductQSURL = strInsuranceurl + ViewBag.HotelQuerystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "MBH")
                {
                    if (ConfigurationManager.AppSettings["mobileUrl_B"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["mobileUrl_BBH"].ToString();
                        ViewBag.OtherProductQSURL = strInsuranceurl + ViewBag.HotelQuerystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }

                #endregion

                #region VISA

                else if (Flag == "V")
                {
                    if (ConfigurationManager.AppSettings["Visa"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["Visa"].ToString();
                        ViewBag.OtherProductQSURL = strInsuranceurl + "?" + ViewBag.HotelQuerystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "VH")
                {
                    if (ConfigurationManager.AppSettings["Visa"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["Visahistory"].ToString();
                        ViewBag.OtherProductQSURL = strInsuranceurl + "?" + ViewBag.HotelQuerystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }

                #endregion

                #region ACTIVITY

                else if (Flag == "A")
                {
                    if (ConfigurationManager.AppSettings["Activity_A"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["Activity_A"].ToString();
                        ViewBag.OtherProductQSURL = strInsuranceurl + "?" + ViewBag.HotelQuerystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "AH")
                {
                    if (ConfigurationManager.AppSettings["Activity_AH"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["Activity_AH"].ToString();
                        ViewBag.OtherProductQSURL = strInsuranceurl + "?" + ViewBag.HotelQuerystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }

                #endregion

                #region TRAIN

                /*Train*/
                else if (Flag == "TRN")
                {
                    if (ConfigurationManager.AppSettings["TRAINURL"].ToString() != "")
                    {
                        string strTrainUrl = ConfigurationManager.AppSettings["TRAINURL"].ToString();
                        DataSet dsTrainAgents = new DataSet();
                        dsTrainAgents.ReadXml(Server.MapPath("~/XML/URL_XML.xml"));
                        if (dsTrainAgents != null && dsTrainAgents.Tables.Contains("TRAIN") && dsTrainAgents.Tables["TRAIN"].Rows.Count > 0 &&
                            dsTrainAgents.Tables["TRAIN"].Rows[0]["AGENTID"].ToString().Contains(strAgentId))
                        {
                            strTrainUrl = dsTrainAgents.Tables["TRAIN"].Rows[0]["TRAINURL"].ToString();
                        }
                        ViewBag.OtherProductQSURL = strTrainUrl + "?" + Querystring + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres + "&CREDIT=N&MACID=" + strMacaddress + "&IRCTCUSERNAME=" + strIRCTCUserName + "&PRODUCTTYPE = B2B";

                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "TRNMB")
                {
                    if (ConfigurationManager.AppSettings["TRAINURL_TBH"].ToString() != "")
                    {
                        string strTrainUrl = ConfigurationManager.AppSettings["TRAINURL_TBH"].ToString();
                        ViewBag.OtherProductQSURL = strTrainUrl + "?" + Querystring + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres + "&CREDIT=N&MACID=" + strMacaddress + "&IRCTCUSERNAME=" + strIRCTCUserName + "&PRODUCTTYPE = B2B";
                    }
                    else
                    {

                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                /*Train*/
                #endregion

                #region CAR
                else if (Flag == "CAR")
                {
                    if (ConfigurationManager.AppSettings["CARURL"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["CARURL"].ToString();
                        ViewBag.OtherProductQSURL = strInsuranceurl + "?" + Querystring + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region ANCILLARYSERVIE
                else if (Flag == "ANC" || Flag == "ANCV")
                {
                    if (ConfigurationManager.AppSettings["ANCILARYSERVICE"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["ANCILARYSERVICE"].ToString();
                        ViewBag.OtherProductQSURL = strInsuranceurl + "?" + strQuerystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }

                #endregion

                #region THEMEPARK

                /* Theme Park */
                else if (Flag == "TMP")
                {
                    if (ConfigurationManager.AppSettings["THEMEPARKURL"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["THEMEPARKURL"].ToString();
                        ViewBag.OtherProductQSURL = strInsuranceurl + "?" + Querystring + "&TERMINALTYPE=" + strTerminaltype + "&IPADDRESS=" + strIpaddres;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region Brillvoice

                /* Brill Booking */
                else if (Flag == "BRL")
                {
                    if (ConfigurationManager.AppSettings["BRILLVOICEURL"].ToString() != "")
                    {
                        string strBrillVoiceurl = ConfigurationManager.AppSettings["BRILLVOICEURL"].ToString();
                        ViewBag.OtherProductQSURL = strBrillVoiceurl + "?" + strQuerystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }

                #endregion

                #region Titos

                /* Titos Booking */
                else if (Flag == "TOS")
                {
                    if (ConfigurationManager.AppSettings["TITOSURL"].ToString() != "")
                    {
                        string strTOSurl = ConfigurationManager.AppSettings["TITOSURL"].ToString();
                        ViewBag.OtherProductQSURL = strTOSurl + "?" + Querystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                /* Titos Booking */

                #endregion

                #region Hotel

                else if (Flag == "H")
                {
                    if (ConfigurationManager.AppSettings["HotelUrl_H"].ToString() != "")
                    {
                        string strhotelurl = ConfigurationManager.AppSettings["HotelUrl_H"].ToString();
                        ViewBag.OtherProductQSURL = strhotelurl + ViewBag.HotelQuerystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "HVP")
                {
                    if (ConfigurationManager.AppSettings["HotelUrl_H"].ToString() != "")
                    {
                        string strhotelVIEWPNRurl = ConfigurationManager.AppSettings["HotelUrl_HVP"].ToString();
                        ViewBag.OtherProductQSURL = strhotelVIEWPNRurl + ViewBag.HotelQuerystring + "&Tab=1";
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "HBH")
                {
                    if (ConfigurationManager.AppSettings["HotelUrl_H"].ToString() != "")
                    {
                        string strhotelBOOKEDHISTORYRurl = ConfigurationManager.AppSettings["HotelUrl_HBH"].ToString();
                        ViewBag.OtherProductQSURL = strhotelBOOKEDHISTORYRurl + ViewBag.HotelQuerystring + "&Tab=0";
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "HCL")
                {
                    if (ConfigurationManager.AppSettings["HotelUrl_H"].ToString() != "")
                    {
                        string strhotelCANCELLATIONSurl = ConfigurationManager.AppSettings["HotelUrl_HCL"].ToString();
                        ViewBag.OtherProductQSURL = strhotelCANCELLATIONSurl + ViewBag.HotelQuerystring + "&Tab=2";
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region UTILITYSERVIES
                else if (Flag == "MUA")
                {
                    if (ConfigurationManager.AppSettings["UTILITYSERVICE"].ToString() != "")
                    {
                        string strUtilityurl = ConfigurationManager.AppSettings["UTILITYSERVICE"].ToString();

                        DataSet dsTrainAgents = new DataSet();
                        dsTrainAgents.ReadXml(Server.MapPath("~/XML/URL_XML.xml"));
                        if (dsTrainAgents != null && dsTrainAgents.Tables.Contains("UTILITY") && dsTrainAgents.Tables["UTILITY"].Rows.Count > 0 &&
                            (dsTrainAgents.Tables["UTILITY"].Rows[0]["AGENTID"].ToString().Contains(strAgentId) || dsTrainAgents.Tables["UTILITY"].Rows[0]["AGENTID"].ToString() == "ALL"))
                        {
                            strUtilityurl = dsTrainAgents.Tables["UTILITY"].Rows[0]["UTILITYURL"].ToString();
                        }
                        ViewBag.OtherProductQSURL = strUtilityurl + "?" + Querystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region HOLIDAY PACKAGES
                else if (Flag == "PKG")
                {
                    if (ConfigurationManager.AppSettings["PackageBooking"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["PackageBooking"].ToString();
                        ViewBag.OtherProductQSURL = strInsuranceurl + "?" + Querystring + "&pkgtype=fixed";
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                else if (Flag == "PKGMB")
                {
                    if (ConfigurationManager.AppSettings["PackageManageBooking"].ToString() != "")
                    {
                        string strInsuranceurl = ConfigurationManager.AppSettings["PackageManageBooking"].ToString();
                        ViewBag.OtherProductQSURL = strInsuranceurl + "?" + ViewBag.HotelQuerystring + "&pkgtype=fixed";
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region Credit Shell PNR VIEW BALANCE
                else if (Flag == "CSPNR")
                {
                    if (ConfigurationManager.AppSettings["ACTIONURL"].ToString() != "")
                    {
                        string strUrl = ConfigurationManager.AppSettings["ACTIONURL"] != null ? ConfigurationManager.AppSettings["ACTIONURL"].ToString() : "";
                        ViewBag.OtherProductQSURL = strUrl + "Action/PnrDetails?" + Querystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region Ticket to cancel
                else if (Flag == "TTC")
                {
                    if (ConfigurationManager.AppSettings["ACTIONURL"].ToString() != "")
                    {
                        string strUrl = ConfigurationManager.AppSettings["ACTIONURL"] != null ? ConfigurationManager.AppSettings["ACTIONURL"].ToString() : "";
                        ViewBag.OtherProductQSURL = strUrl + "Action/ToCancel?" + Querystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region Retrieve Accounting
                else if (Flag == "RTAC")
                {
                    if (ConfigurationManager.AppSettings["ACTIONURL"].ToString() != "")
                    {
                        string strUrl = ConfigurationManager.AppSettings["ACTIONURL"] != null ? ConfigurationManager.AppSettings["ACTIONURL"].ToString() : "";
                        ViewBag.OtherProductQSURL = strUrl + "RetrieveBooking/RetrieveAccounting?" + Querystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region Booking Track
                else if (Flag == "BTK")
                {
                    if (ConfigurationManager.AppSettings["ACTIONURL"].ToString() != "")
                    {
                        string strUrl = ConfigurationManager.AppSettings["ACTIONURL"] != null ? ConfigurationManager.AppSettings["ACTIONURL"].ToString() : "";
                        ViewBag.OtherProductQSURL = strUrl + "Action/BookingTrack?" + Querystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region Cruises
                else if (Flag == "CRU")
                {
                    if (ConfigurationManager.AppSettings["CRUISES_URL"].ToString() != "")
                    {
                        string strUrl = ConfigurationManager.AppSettings["CRUISES_URL"] != null ? ConfigurationManager.AppSettings["CRUISES_URL"].ToString() : "";
                        ViewBag.OtherProductQSURL = strUrl + "?" + Querystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region Manual Booking
                else if (Flag.StartsWith("MB"))
                {
                    if (ConfigurationManager.AppSettings["ACTIONURL"].ToString() != "")
                    {
                        string strUrl = ConfigurationManager.AppSettings["ACTIONURL"] != null ? ConfigurationManager.AppSettings["ACTIONURL"].ToString() : "";
                        if (Flag == "MBAIR")
                        {
                            ViewBag.OtherProductQSURL = strUrl + "ManualTicket/AirTicket?" + Querystring;
                        }
                        else if (Flag == "MBBUS")
                        {
                            ViewBag.OtherProductQSURL = strUrl + "ManualTicket/BusTicket?" + Querystring;
                        }
                        else if (Flag == "MBCAR")
                        {
                            ViewBag.OtherProductQSURL = strUrl + "ManualTicket/CarTicket?" + Querystring;
                        }
                        else if (Flag == "MBHTL")
                        {
                            ViewBag.OtherProductQSURL = strUrl + "ManualTicket/HotelTicket?" + Querystring;
                        }
                        else if (Flag == "MBTRN")
                        {
                            ViewBag.OtherProductQSURL = strUrl + "ManualTicket/TrainTicket?" + Querystring;
                        }
                        else if (Flag == "MBINS")
                        {
                            ViewBag.OtherProductQSURL = strUrl + "ManualTicket/InsuranceTicket?" + Querystring;
                        }
                        else if (Flag == "MBOTR")
                        {
                            ViewBag.OtherProductQSURL = strUrl + "ManualTicket/OtherTicket?" + Querystring;
                        }
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region Agent MyAccount Details
                else if (Flag.StartsWith("WA"))
                {
                    if (ConfigurationManager.AppSettings["ACTIONURL"].ToString() != "")
                    {
                        string strUrl = ConfigurationManager.AppSettings["ACTIONURL"] != null ? ConfigurationManager.AppSettings["ACTIONURL"].ToString() : "";
                        if (Flag == "WATUP") // AGENR TOPUP URL
                        {
                            ViewBag.OtherProductQSURL = strUrl + "Home/AgentTopup?" + Querystring;
                        }
                        else if (Flag == "WABD") // BANK DETAILS
                        {
                            ViewBag.OtherProductQSURL = strUrl + "Home/BankDetail?" + Querystring;
                        }
                        else if (Flag == "WASC") // AGENT SERVICE CHARGE
                        {
                            ViewBag.OtherProductQSURL = strUrl + "Home/ServiceCharge?" + Querystring;
                        }
                        else if (Flag == "WAAE") // AGENT EARING
                        {
                            ViewBag.OtherProductQSURL = strUrl + "Home/AgentEarning?" + Querystring;
                        }
                        else if (Flag == "WAAS") // Agent Support
                        {
                            ViewBag.OtherProductQSURL = strUrl + "Home/AgentSupport?" + Querystring;
                        }

                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }
                #endregion

                #region AgentSalesReport
                else if (Flag == "ASR")
                {
                    if (ConfigurationManager.AppSettings["AGENTSALESUMMARYURL"] != null && ConfigurationManager.AppSettings["AGENTSALESUMMARYURL"].ToString() != "")
                    {
                        string Sales_Querystring = ConfigurationManager.AppSettings["AGENTSALESUMMARYURL"].ToString() + "?SECKEY=" + Base.EncryptKEy(strSalesreportQuerystring, "VKRIYA" + DateTime.Now.ToString("yyyyMMdd"));
                        ViewBag.OtherProductQSURL = Sales_Querystring;
                    }
                    else
                    {
                        ViewBag.OtherProductQSURL = Comingsoon;
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return View();
        }

        public ActionResult Checktopup()
        {
            string strCstnameID = string.Empty;
            string strAgentPOS_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";  // LOGIN AGENT ID
            string strAgentPOS_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : ""; // LOGIN TERMINAL ID
            string strBranch = Session["branchid"] != null ? Session["branchid"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strSequenceNo = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            string strPrivilage = Session["privilage"] != null ? Session["privilage"].ToString() : "";
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strPassword = Session["PWD"] != null ? Session["PWD"].ToString() : "";
            string strIPAddress = Base.GetComputer_IP();
            string strReturnURl = ConfigurationManager.AppSettings["HOMEURL"].ToString();
            try
            {
                if (strAgentPOS_ID == "" || strUserName == "" || strTerminalType == "")
                {
                    return View("~/Views/Redirect/SessionExp.cshtml");
                }

                string PARAMETER = "AGENTID=" + strAgentPOS_ID + "&TERMINALID=" + strAgentPOS_TID + "&TERMINALTYPE=" + strTerminalType + "&USERNAME=" + strUserName + "&PASSWORD=" + strPassword + "&AGENT_TYPE=" + strAgentType + "&IPADDRESS=" + strIPAddress + "&SEQUENCEID=" + strSequenceNo + "&BRANCHID=" + strBranch + "&PREVELIGE=" + strPrivilage;
                ViewBag.Checktopup = ConfigurationManager.AppSettings["CHECKAUTOTOPUPURL"].ToString() + "?SECKEY=" + Base.EncryptKEy(PARAMETER, "RIYA" + DateTime.Now.ToString("dd/MM/yyyy"));// +DateTime.Now.ToString("dd/MM/yyyy")


                return View("~/Views/OtherProduct/Checktopup.cshtml");
            }
            catch (Exception ex)
            {
                return View("~/Views/Redirect/SessionExp.cshtml");
            }

        }

        #region Emergency Evacuation
        public ActionResult EmergencyEvacuation()
        {
            return View();
        }


        public ActionResult INSERT_FOI_DETAILS(string UAE_PERSON_NAME, string GENDER, string DEPENDENTTYPE, string DEPENDENTNAME, string DOB, string PROFESSION, string EDUCATION,
           string LANGUAGES, string BIRTHCOUNTRY, string MARTIAL, string WHATSAPPNO, string UAE_CONTACT_NO, string UAE_EMAILID, string IND_PERSON_NAME,
           string IND_CONTACT_NO, string IND_ADDRESS, string IND_STATE, string IND_CITY, string ORIGIN, string DESTINATION, string URGENTREASON,
           string NEAREST_AIRPORT, string TRAVEL_PRIORITY, string NATIONALITY, string BIRTHPLACE, string paxlistname, string Quarntine, string CovidTest, string CovidResult)
        {
            string strStatus = "0";
            string StrErrormsg = "";
            string seqno = "";
            string strsuccmailid = "";
            string strLogData = "";
            string strUserName = (Session["username"] != null && Session["username"].ToString() != "") ? Session["username"].ToString() : "";


            DataSet dstResult = new DataSet();
            string ErrorMessage = string.Empty;
            string strres = string.Empty;
            //bool my_result = false;

            MARTIAL = MARTIAL != null ? MARTIAL : "";
            GENDER = GENDER != null ? GENDER : "";
            DEPENDENTTYPE = DEPENDENTTYPE != null ? DEPENDENTTYPE : "";
            LANGUAGES = LANGUAGES != null ? LANGUAGES : "";
            GENDER = GENDER != null ? GENDER : "";
            URGENTREASON = URGENTREASON != null ? URGENTREASON : "";
            DEPENDENTNAME = DEPENDENTNAME != null ? DEPENDENTNAME : "";
            PROFESSION = PROFESSION != null ? PROFESSION : "";
            EDUCATION = EDUCATION != null ? EDUCATION : "";
            IND_PERSON_NAME = IND_PERSON_NAME != null ? IND_PERSON_NAME : "";
            IND_CONTACT_NO = IND_CONTACT_NO != null ? IND_CONTACT_NO : "";
            IND_ADDRESS = IND_ADDRESS != null ? IND_ADDRESS : "";
            IND_STATE = IND_STATE != null ? IND_STATE : "";
            IND_CITY = IND_CITY != null ? IND_CITY : "";

            string strPassword = Base.RandomOTPGeneration(ref strres);
            strPassword = strPassword != "" ? strPassword : "ge42sf";

            string Titile = "fetchUserRightsLoad";
            string strXMLEvent = string.Empty;
            Hashtable my_param = new Hashtable();
            string Msg = "";
            try
            {

                my_param.Clear();
                string IP_ADDRESS = ControllerContext.HttpContext.Request.UserHostAddress;
                my_param.Add("UAE_PERSON_NAME", UAE_PERSON_NAME);
                my_param.Add("UAE_CONTACT_NO", UAE_CONTACT_NO);
                my_param.Add("UAE_EMAILID", UAE_EMAILID);
                my_param.Add("UAE_EMIRATES", "");
                my_param.Add("UAE_PO_NO", strPassword);
                my_param.Add("UAE_RESIDENCEPHONE", "");
                my_param.Add("UAE_WORKINGPHONE", "");
                my_param.Add("UAE_MOBILENO", "");
                my_param.Add("IND_PERSON_NAME", IND_PERSON_NAME);
                my_param.Add("IND_CONTACT_NO", IND_CONTACT_NO);
                my_param.Add("IND_ADDRESS", IND_ADDRESS);
                my_param.Add("IND_STATE", IND_STATE);
                my_param.Add("IND_CITY", IND_CITY);
                my_param.Add("Y_H_P_TICKETS", "");
                my_param.Add("WILLING_TO_TRAVEL", "");
                my_param.Add("SERVICE_OPEN_TO_FLY", "");
                my_param.Add("PLACE_TO_TRAVEL", BIRTHPLACE);
                my_param.Add("ASSISTATED_WITH_FLIGHT_BY_EMAIL", "");
                my_param.Add("WILLING_TO_PAY", "");
                my_param.Add("SPECIAL_FLIGHT", "");
                my_param.Add("SPECIAL_FLIGHT_CHARGES", "0");
                my_param.Add("WHERE_PLACED_NOW", Quarntine);
                my_param.Add("OWN_ACCOMIDATION", CovidTest);
                my_param.Add("EXPECTING_ASSITANCE", CovidResult);
                my_param.Add("CREATED_BY", strUserName);
                my_param.Add("IP_ADDRESS", IP_ADDRESS);


                my_param.Add("DOB", DOB);
                my_param.Add("DEPENDENTTYPE", DEPENDENTTYPE);
                my_param.Add("DEPENDENTNAME", DEPENDENTNAME);
                my_param.Add("PROFESSION", paxlistname);
                my_param.Add("EDUCATION", EDUCATION);
                my_param.Add("LANGUAGES", LANGUAGES);
                my_param.Add("BIRTHCOUNTRY", BIRTHCOUNTRY);
                my_param.Add("MARTIAL", MARTIAL);
                my_param.Add("WHATSAPPNO", WHATSAPPNO);
                my_param.Add("ORIGIN", ORIGIN);
                my_param.Add("DESTINATION", DESTINATION);
                my_param.Add("TRAVEL_PRIORITY", TRAVEL_PRIORITY);
                my_param.Add("URGENTREASON", URGENTREASON);
                my_param.Add("NEAREST_AIRPORT", NEAREST_AIRPORT);
                my_param.Add("GENDER", GENDER);
                my_param.Add("strResult", "");
                my_param.Add("strErrorMessage", "");

                strLogData = "<URL> " + JsonConvert.SerializeObject(my_param) + "<URL>";
                DatabaseLog.LogData(strUserName, "E", "OtherproductController", "Insert_foi_details", strLogData, "", "", "");

                JObject objJsonSeq = Base.callWebMethod("Covid19InsertRegister", my_param, ref ErrorMessage);
                string strResult = "";
                if (objJsonSeq != null)
                {
                    bool JsonBook = (bool)objJsonSeq["Covid19InsertRegisterResult"];
                    if (JsonBook == true)
                    {
                        strResult = (string)objJsonSeq["strResult"];
                        dstResult = JsonConvert.DeserializeObject<DataSet>(strResult);

                        seqno = dstResult.Tables[0].Rows[0]["Column1"].ToString();

                        if (dstResult.Tables[0].Columns.Contains("Column2"))
                        {
                            return Json(new { Status = "0", Errormsg = dstResult.Tables[0].Rows[0]["Column2"].ToString(), Referenceid = seqno, Mailid = strsuccmailid });
                        }
                        else
                        {
                            strStatus = "1";
                            bool flgmail = SendMail(UAE_EMAILID, strPassword, my_param);
                            strsuccmailid = flgmail == true ? UAE_EMAILID : "";
                        }
                    }
                    else
                    {
                        strStatus = "0";
                        StrErrormsg = "Unable to process your request. Please contact support team (#03)";
                    }
                }
                else
                {
                    strStatus = "0";
                    StrErrormsg = "Unable to process your request. Please contact support team (#03)";
                }

                strLogData = "<STATUS>" + strStatus + "<STATUS>"
                    + "<RESPONSE>" + strResult + "<RESPONSE> <ERRORMESSAGE> " + ErrorMessage + "</ERRORMESSAGE>";
                DatabaseLog.LogData(strUserName, "E", "OtherProductController", "Insert_Foi_details~RES", strLogData, "", "", "");


            }
            catch (Exception ex)
            {
                strStatus = "0";
                StrErrormsg = "Unable to process your request. Please contact support team (#05)";
                DatabaseLog.LogData(strUserName, "E", "OtherProductController", "Insert_Foi_details~X", ex.ToString(), "", "", "");

                Msg = "Insert Failed";
                //InsertLog("X", strUserName, "Service", Titile, ex.Message.ToString(), "");
                //   DatabaseLog.LogData(strUserName, "X", "Base", "fetchUserRightsLoadWEB", ex.ToString(), struserID, "", sequenceID, ipAddress);
            }
            return Json(new { Status = strStatus, Errormsg = StrErrormsg, Referenceid = seqno, Mailid = strsuccmailid });
        }

        public bool SendMail(string strSendId, string strPassword, Hashtable my_param)
        {
            string strStatus = string.Empty;
            string strMsg = string.Empty;
            bool boolres = false;
            Mailref.MyService _MailService = new Mailref.MyService();
            string strUserName = (Session["username"] != null && Session["username"].ToString() != "") ? Session["username"].ToString() : "";
            try
            {
                string stringBuilder = string.Empty;
                string strres = "";
                string strSubject = string.Empty;
                string MailUsername = string.Empty;
                string MailPassword = string.Empty;
                strPassword = strPassword != "" ? strPassword : "ge42sf";

                bool EnableSsl = false;

                string strErrmsg = string.Empty;
                string strMaildId = strSendId;

                #region MailContent
                string imgurl = ConfigurationManager.AppSettings["LogoUrl"] + "logo.png?v1.0";

                stringBuilder += "<div>";
                //stringBuilder += "<p class='MsoNormal'>Post Registration :  <u></u><u></u></p>";
                stringBuilder += "<p class='MsoNormal'><u></u> <u></u></p>";

                stringBuilder += "<p class='MsoNormal'><u></u> <u></u></p>";
                stringBuilder += "<p class='MsoNormal'>You have successfully completed your Registration process.<span style='color:#31708f'>  We will soon get back to you.</span></p>";
                stringBuilder += "<img src=" + imgurl + " />";
                stringBuilder += "<p class='MsoNormal'><span lang='EN-GB'><u></u> <u></u></span> Roundtrip.in</p>";
                stringBuilder += "<p class='MsoNormal'><span lang='EN-GB'><u></u> <u></u></span> #83, Sydenhams Road, 2nd Floor, Opp. Nehru Stadium Gate 1, Periamet, Chennai- 600003.</p>";
                //stringBuilder += "<p class='MsoNormal'><span> </span><span lang='EN-GB'#83, Sydenhams Road, 2nd Floor, Opp. Nehru Stadium Gate 1, Periamet, Chennai- 600003.<u></u><u></u></span></p>";
                stringBuilder += "<p class='MsoNormal'><span lang='EN-GB'><u></u> <u></u></span> support@roundtrip.in </p>";

                stringBuilder += "<p class='MsoNormal'><span lang='EN-GB'><u></u> <u></u></span>044-4000 0005</p>";

                stringBuilder += "<p class='MsoNormal'><span lang='EN-GB'>Stay Safe, we all are ";
                stringBuilder += "there with you.<u></u><u></u></span></p>";


                stringBuilder += "</div>";
                #endregion


                strSubject = "Registered Successfully";
                //   strSendId = "sirancheevishan1@gmail.com";


                _MailService.Url = ConfigurationManager.AppSettings["Mailurl"].ToString();

                string strPortNo = ConfigurationManager.AppSettings["PortNo"].ToString();
                string strMailusername = ConfigurationManager.AppSettings["MailUsername"].ToString();
                string strNetworkusername = ConfigurationManager.AppSettings["NetworkUsername"].ToString();
                string strMailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
                string strHostAddress = ConfigurationManager.AppSettings["HostAddress"].ToString();
                bool blnEnableSSL = ConfigurationManager.AppSettings["EnableSsl"].ToString() == "false" ? false : true;



                string retval = _MailService.SendMailSingleTicket("", "", "", "", "", "",
                                strSendId, "", strSubject, stringBuilder, "", "", strMailusername, strMailPassword, strHostAddress, strPortNo,
                                blnEnableSSL, strMailusername, "");

                stringBuilder = "";
                //To Send register details
                stringBuilder += "<div id='card' style='position: relative;top: 110px;width: 520px;display: block;margin: auto;text-align: left;border: 1px solid #eee' class='animated fadeIn'>";
                stringBuilder += "<div id='upper-side' style=' padding: 0.1em;background-color: #8BC34A;display: block;color: #fff;border-top-right-radius: 8px;border-top-left-radius: 8px;text-align:center'>";
                stringBuilder += " <h3 id='status'> Registered Details </h3>";
                stringBuilder += " </div>";

                stringBuilder += "  <div id='lower-side' style='padding:10px'>";
                stringBuilder += "  <div id='message' style='padding:10px'> <div style='width: 200px;float:left'> Name </div> : " + my_param["UAE_PERSON_NAME"] + " </div>";
                stringBuilder += "  <div id='message' style='padding:10px'> <div style='width: 200px;float:left'> Email ID </div>: " + my_param["UAE_EMAILID"] + " </div>";
                stringBuilder += "  <div id='message' style='padding:10px'> <div style='width: 200px;float:left'> Contact No. </div>: " + my_param["UAE_CONTACT_NO"] + " </div>";
                stringBuilder += "  <div id='message' style='padding:10px'> <div style='width: 200px;float:left'> Current Address </div>: " + my_param["IND_ADDRESS"] + " </div>";
                stringBuilder += "  <div id='message' style='padding:10px'> <div style='width: 200px;float:left'> Destination Address </div>: " + my_param["PLACE_TO_TRAVEL"] + " </div>";
                stringBuilder += "  <div id='message' style='padding:10px'> <div style='width: 200px;float:left'> Sector </div>: " + my_param["ORIGIN"] + " </div>";
                stringBuilder += "  <div id='message' style='padding:10px'> <div style='width: 200px;float:left'> Adult </div>: " + my_param["PROFESSION"].ToString().Split('|')[0] + " </div>";
                stringBuilder += "  <div id='message' style='padding:10px'> <div style='width: 200px;float:left'> Child </div>: " + (my_param["PROFESSION"].ToString().Split('|')[1].Trim() != "" ? my_param["PROFESSION"].ToString().Split('|')[1].Trim() : "0") + " </div>";
                stringBuilder += "  <div id='message' style='padding:10px'> <div style='width: 200px;float:left'> Infant </div>: " + (my_param["PROFESSION"].ToString().Split('|')[2].Trim() != "" ? my_param["PROFESSION"].ToString().Split('|')[2].Trim() : "0") + " </div>";
                stringBuilder += "  <div id='message' style='padding:10px'> <div style='width: 200px;float:left'> Reason for Travel </div>: " + my_param["TRAVEL_PRIORITY"] + " </div>";
                stringBuilder += "  <div id='message' style='padding:10px;margin-bottom:10px;'>  <div style='width: 200px;float:left'>Have you been quarrantined in last two months </div>: " + (my_param["WHERE_PLACED_NOW"].ToString().Trim() == "Y" ? "Yes" : "No") + " </div>";
                stringBuilder += "  <div id='message' style='padding:10px;margin-bottom:10px;'> <div style='width: 200px;float:left'> Have you undergone COVID Test </div>: " + (my_param["OWN_ACCOMIDATION"].ToString().Trim() == "Y" ? "Yes" : "No") + " </div>";
                if (my_param["OWN_ACCOMIDATION"].ToString().Trim() == "Y")
                    stringBuilder += "  <div id='message' style='padding:10px;margin-bottom:10px'> <div style='width: 200px;float:left'> COVID-19 Test result </div>: " + my_param["EXPECTING_ASSITANCE"] + " </div>";
                stringBuilder += "</div>";
                stringBuilder += "</div>";

                retval = _MailService.SendMailSingleTicket("", "", "", "", "", "",
                               "Support@roundtrip.in", "Balaji@roundtrip.in,postproduction@shreyastechsolutions.com", strSubject, stringBuilder, "", "", strMailusername, strMailPassword,
                               strHostAddress, strPortNo,
                               blnEnableSSL, strMailusername, "");


                DatabaseLog.LogData(strUserName, "E", "OtherProductController", "Sendmail", retval, "", "", "");

                boolres = true;
                if (boolres == true)
                {
                    strStatus = "01";
                    strMsg = "Message sent Sucessfully.";
                }
                else
                {
                    strStatus = "01";
                    strMsg = "Unable to sent Mail.";
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(strUserName, "E", "OtherProductController", "Sendmail-X", ex.ToString(), "", "", "");
                strStatus = "01";
                strMsg = "Unable to sent Mail.";
                boolres = false;
            }
            return boolres;
        }
        #endregion
    }
}
