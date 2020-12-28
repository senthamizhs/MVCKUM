using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Text;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Net;
using STSTRAVRAYS.Models;
using STSTRAVRAYS.InplantService;
using System.Web.UI;
using System.Web.UI.WebControls;
using STSTRAVRAYS.Rays_service;
using RestSharp;


namespace STSTRAVRAYS.Controllers
{
    public class VisaController : Controller
    {
        //
        // GET: /VisaOffline/

        public ActionResult Index()
        {
            return View();
        }


        #region Visa Uae Form
        public ActionResult VisaOfflinecreation()
        {
            #region UsageLog
            string PageName = "Visa Offline Creation";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "");
            }
            catch (Exception e) { }
            #endregion
            string Encquery = Request.QueryString["SECKEY"];
            ViewBag.ServerDateTime = Base.LoadServerdatetime();
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            string DecryptQry = Base.DecryptKEY(Encquery, "VISAQRY" + today);
            string[] querystring = DecryptQry.Split('&');
            string strQryAgentID = querystring[1] != null && querystring[1].ToString() != "" ? querystring[1].Split('=')[1].ToString() : "";
            string strQryTerminalID = querystring[0] != null && querystring[0].ToString() != "" ? querystring[0].Split('=')[1].ToString() : "";
            string strQryIPAddress = querystring[2] != null && querystring[2].ToString() != "" ? querystring[2].Split('=')[1].ToString() : "";
            string strQryUserName = querystring[3] != null && querystring[3].ToString() != "" ? querystring[3].Split('=')[1].ToString() : "";
            string strQrySequenceID = querystring[4] != null && querystring[4].ToString() != "" ? querystring[4].Split('=')[1].ToString() : "";
            Session["POS_ID"] = strQryAgentID;
            Session["POS_TID"] = strQryTerminalID;
            Session["ipAddress"] = strQryIPAddress;
            Session["UserName"] = strQryUserName;
            Session["sequenceid"] = strQrySequenceID;
            if (strQryAgentID == null || strQryAgentID == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }

            string strVisapaxcount = querystring[5] != null && querystring[5].ToString() != "" ? querystring[5].Split('=')[1].ToString() : "";
            string strVisaPNR = querystring[6] != null && querystring[6].ToString() != "" ? querystring[6].Split('=')[1].ToString() : "";
            ViewBag.VisaPaxCount = strVisapaxcount;
            Session.Remove("SPNR");
            Session.Add("SPNR", strVisaPNR);
            return View("~/Views/Visa/VisaOfflinecreation.cshtml");
        }

        public ActionResult VisaThankyou()
        { return View(); }

        public ActionResult Visa()
        {
            string strQryAgentID = string.Empty;
            string strQryTerminalID = string.Empty;
            string strQryIPAddress = string.Empty;
            string strQryUserName = string.Empty;
            string strQrySequenceID = string.Empty;
            string strVisapaxcount = string.Empty;
            string strVisaPNR = string.Empty;

            string Encquery = Request.QueryString["SECKEY"] != null && Request.QueryString["SECKEY"].ToString() != "" ? Request.QueryString["SECKEY"].ToString() : "";
            ViewBag.ServerDateTime = Base.LoadServerdatetime();
            if (Encquery != null && Encquery != "")
            {
                string today = DateTime.Now.ToString("dd/MM/yyyy");
                string DecryptQry = Base.DecryptKEY(Encquery, "VISAQRY" + today);

                string[] querystring = DecryptQry.Split('&');
                strQryAgentID = querystring[1] != null && querystring[1].ToString() != "" ? querystring[1].Split('=')[1].ToString() : "";
                strQryTerminalID = querystring[0] != null && querystring[0].ToString() != "" ? querystring[0].Split('=')[1].ToString() : "";
                strQryIPAddress = querystring[2] != null && querystring[2].ToString() != "" ? querystring[2].Split('=')[1].ToString() : "";
                strQryUserName = querystring[3] != null && querystring[3].ToString() != "" ? querystring[3].Split('=')[1].ToString() : "";
                strQrySequenceID = querystring[4] != null && querystring[4].ToString() != "" ? querystring[4].Split('=')[1].ToString() : "";
                strVisapaxcount = querystring[5] != null && querystring[5].ToString() != "" ? querystring[5].Split('=')[1].ToString() : "";
                strVisaPNR = querystring[6] != null && querystring[6].ToString() != "" ? querystring[6].Split('=')[1].ToString() : "";
                // strVisapaxcount = "0";
            }
            else
            {
                strQryAgentID = Session["agentid"] != null && Session["agentid"].ToString() != "" ? Session["agentid"].ToString() : "";
                strQryTerminalID = Session["terminalid"] != null && Session["terminalid"].ToString() != "" ? Session["terminalid"].ToString() : "";
                strQryIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
                strQryUserName = Session["username"] != null && Session["username"].ToString() != "" ? Session["username"].ToString() : "";
                strQrySequenceID = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "";
                strVisapaxcount = "0";
            }
            Session["POS_ID"] = strQryAgentID;
            Session["POS_TID"] = strQryTerminalID;
            Session["ipAddress"] = strQryIPAddress;
            Session["UserName"] = strQryUserName;
            Session["sequenceid"] = strQrySequenceID;
            if (strQryAgentID == null || strQryAgentID == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            ViewBag.VisaPaxCount = strVisapaxcount;
            Session.Remove("SPNR");
            Session.Add("SPNR", strVisaPNR);
            return View("~/Views/Visa/Visa.cshtml");
        }

        public ActionResult InsertVisaOffline()
        {
            ArrayList aryy = new ArrayList();
            aryy.Add("");
            aryy.Add("");
            int Error = 0;
            int Result = 1;
            string status = string.Empty;
            string strErrorMsg = string.Empty;
            string xmlData = string.Empty;
            string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string UserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strTerminalType = (Session["terminaltype"] != null && Session["terminaltype"].ToString() != "") ? Session["terminaltype"].ToString() : "";
            string StartupPath = string.Empty;
            string imgpath = string.Empty;
            string strFileName = string.Empty;
            string statuscode = string.Empty;
            string strVisaPNR = Session["SPNR"] != null && Session["SPNR"].ToString() != "" ? Session["SPNR"].ToString() : "";
            string strVisaTrackID = string.Empty;
            try
            {
                DataSet dsset = new DataSet();
                InplantService.Inplantservice _inplantserice = new InplantService.Inplantservice();
                _inplantserice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                FileStream fs;
                string Date = DateTime.Now.ToString("yyyyMMddHHmmss");
                byte[] PassportFirstPage = new byte[] { };
                byte[] PassportLastPge = new byte[] { };
                byte[] psportPhoto = new byte[] { };
                byte[] PassportReturn = new byte[] { };
                byte[] passportObservation = new byte[] { };
                byte[] psprtMrgeCertifcte = new byte[] { };
                byte[] psprtBirthCertifcte = new byte[] { };
                string strPaxCount = System.Web.HttpContext.Current.Request["PaxCount"];
                int PaxCount = Convert.ToInt32(strPaxCount);
                string Results = string.Empty;
                string TableName = "T_T_SPNR"; string columnName = "VSA_OFFLINE_SEQNO"; string strvisa = "VSA"; string strLength = "7";
                string strRefID = string.Empty;
                for (var j = 1; j <= PaxCount; j++)
                {
                    xmlData = "<EVENT><REQUEST>InsertVisaOffline</REQUEST>"
               + "<TABLENAME>" + TableName + "</TABLENAME>"
               + "<COLUMNNAME>" + columnName + "</COLUMNNAME>"
               + "<STRING>" + strvisa + "</STRING>"
               + "<LENGTH>" + strLength + "</LENGTH>"
               + "</EVENT>";
                    DatabaseLog.LogData(UserName, "E", "VisaController", "InsertVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                    strRefID = _inplantserice.GenerateSeqNoForVisa(strAgentId, strTerminalID, UserName, IPAddress, strTerminalType, Convert.ToDecimal(strSequenceId),
                       ref strErrorMsg, "VisaController.cs", "InsertVisaOffline", TableName, columnName, strvisa, strLength, "");
                    xmlData = "<EVENT><RESPONSE>InsertVisaOffline</RESPONSE>"
                     + "<RESULT>" + strRefID + "</RESULT>"
                     + "</EVENT>";
                    DatabaseLog.LogData(UserName, "E", "VisaController", "InsertVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                    string strfirstName = System.Web.HttpContext.Current.Request["strFirstName_" + j];
                    string strmiddleName = System.Web.HttpContext.Current.Request["strmiddleName_" + j];
                    string strlastName = System.Web.HttpContext.Current.Request["strlastName_" + j];
                    string strmobileNumber = System.Web.HttpContext.Current.Request["strmobileNumber_" + j];
                    string strEmailId = System.Web.HttpContext.Current.Request["strEmailId_" + j];
                    string strPassportNum = System.Web.HttpContext.Current.Request["strPassportNum_" + j];
                    string strmaritalstatus = System.Web.HttpContext.Current.Request["strmaritalstatus_" + j];
                    string strreligion = System.Web.HttpContext.Current.Request["strreligion_" + j];
                    string streducation = System.Web.HttpContext.Current.Request["streducation_" + j];
                    string strvisitpurpose = System.Web.HttpContext.Current.Request["strvisitpurpose_" + j];
                    string strduration = System.Web.HttpContext.Current.Request["strduration_" + j];
                    string strprofession = System.Web.HttpContext.Current.Request["strprofession_" + j];
                    string strRPNR = strVisaPNR; //System.Web.HttpContext.Current.Request["strRPNR_"+j];
                    // string strRefID = Date;//System.Web.HttpContext.Current.Request["strRefID"];
                    string strPaxTitle = System.Web.HttpContext.Current.Request["strPaxTitle_" + j];
                    string strFlag = System.Web.HttpContext.Current.Request["strFlag_" + j];
                    strVisaTrackID = strTerminalID + DateTime.Now.ToString("yyMMddHHmmss");
                    xmlData = "<EVENT><REQUEST>InsertVisaOffline</REQUEST>" +
                         "<FIRSTNAME>" + strfirstName + "</FIRSTNAME>" +
                     "<MIDDLENAME>" + strmiddleName + "</MIDDLENAME>" +
                     "<LASTNAME>" + strlastName + "</LASTNAME>" +
                     "<MOBILENUMBER>" + strmobileNumber + "</MOBILENUMBER>" +
                     "<EMAILID>" + strEmailId + "</EMAILID>" +
                     "<PASSPORTNUM>" + strPassportNum + "</PASSPORTNUM>" +
                     "<MARITALSTATUS>" + strmaritalstatus + "</MARITALSTATUS>" +
                     "<RELIGION>" + strreligion + "</RELIGION>" +
                     "<EDUCATION>" + streducation + "</EDUCATION>" +
                     "<VISITPURPOSE>" + strvisitpurpose + "</VISITPURPOSE>" +
                     "<DURATION>" + strduration + "</DURATION>" +
                     "<PROFESSION>" + strprofession + "</PROFESSION>" +
                     "<AGENTID>" + strAgentId + "</AGENTID>" +
                     "<TERMINALID>" + strTerminalID + "</TERMINALID>" +
                     "<IPADDRESS>" + IPAddress + "</IPADDRESS>" +
                     "<SEQUENCEID>" + strSequenceId + "</SEQUENCEID>" +
                     "<USERNAME>" + UserName + "</USERNAME>" +
                     "<TERMINALTYPE>" + strTerminalType + "</TERMINALTYPE>" +
                     "<RPNR>" + strRPNR + "</RPNR>" +
                     "<REFID>" + strRefID + "</REFID>" +
                     "<PAXTITLE>" + strPaxTitle + "</PAXTITLE>" +
                     "<FLAG>" + strFlag + "</FLAG>" +
                     "<TRACKID>" + strVisaTrackID + "</TRACKID>" +
                     "</EVENT>";
                    DatabaseLog.LogData(UserName, "E", "VisaController", "InsertVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                    HttpFileCollectionBase files = Request.Files;
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file1 = files[i];
                            string fileExt1 = System.IO.Path.GetExtension(file1.FileName);
                            string fname;
                            if (fileExt1 == ".jpg" || fileExt1 == ".png")
                            {
                                //strFileName = Request.Form[i];
                                strFileName = files.AllKeys[i];
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
                                fname = Path.Combine(Server.MapPath(@"~/PDF/AgentLogo/"), fname);
                                file1.SaveAs(fname);
                                if (strFileName == "PassportFirst_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    PassportFirstPage = new byte[fs.Length];
                                    PassportFirstPage = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "PassportLast_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    PassportLastPge = new byte[fs.Length];
                                    PassportLastPge = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "PassportPhoto_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psportPhoto = new byte[fs.Length];
                                    psportPhoto = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "PassportReturn_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    PassportReturn = new byte[fs.Length];
                                    PassportReturn = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "passportobservation_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    passportObservation = new byte[fs.Length];
                                    passportObservation = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "passportmrge_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psprtMrgeCertifcte = new byte[fs.Length];
                                    psprtMrgeCertifcte = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "passportbirth_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psprtBirthCertifcte = new byte[fs.Length];
                                    psprtBirthCertifcte = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                System.IO.File.Delete(fname);
                                DatabaseLog.LogData(Session["username"].ToString(), "E", "VisaContoller", "InsertVisaOffline", fname, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                            }
                            else
                            {
                                status = "00";
                                aryy[Error] = "Please select png/jpg files to upload";
                                xmlData = "<EVENT><RESPONSE>InsertVisaOffline</RESPONSE>" +
                                         "<RESULT>Please select png image files to upload</RESULT></EVENT>";
                                DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                                return Json(new { Status = status, Path = "", Results = aryy });
                            }
                        }
                    }
                    else
                    {
                        status = "00";
                        aryy[Error] = "Please select png image files to upload";
                        xmlData = "<EVENT><RESPONSE>InsertVisaOffline</RESPONSE>" +
                    "<RESULT>Please select png image files to upload</RESULT></EVENT>";
                        DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                        return Json(new { Status = status, Path = "", Results = aryy });
                    }
                    Results = GettingImgRequest(strAgentId, PassportFirstPage, PassportLastPge, psportPhoto, PassportReturn, passportObservation, psprtMrgeCertifcte, psprtBirthCertifcte, strTerminalID.ToString(), UserName.ToString(), IPAddress,
                    strTerminalType.ToString(), Convert.ToDecimal(strSequenceId), ref strErrorMsg, "VisaOfflineContoller", "InsertVisaOffline", strfirstName, strmiddleName, strlastName, strmobileNumber, strEmailId,
                    strPassportNum, strmaritalstatus, strreligion, streducation, strvisitpurpose, strduration, strprofession, strRPNR, strRefID, strPaxTitle, strFlag, ref statuscode, strVisaTrackID).InnerText.ToString();
                }
                if (statuscode != "00" && Results != "0")
                {
                    status = "01";
                    aryy[Result] = "Your visa request submited successfully" + "\n" + "Your Reference ID:" + strVisaTrackID.ToString();
                    xmlData = "<EVENT><RESPONSE>InsertVisaOffline</RESPONSE>" +
                    "<RESULT>Your visa request submited successfully" + "\n" + "Your Reference ID:" + strVisaTrackID.ToString() + "</RESULT></EVENT>";
                    DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                    if (ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA")
                    {
                        string strAgentmailToID = Session["AgencyDetailsformail"].ToString().Split('~').Length > 5 ? Session["AgencyDetailsformail"].ToString().Split('~')[1] : "";
                        string strVisaToMailID = ConfigurationManager.AppSettings["VisaTOMailID"].ToString();
                        string strMailUserName = ConfigurationManager.AppSettings["MailUsername"].ToString();
                        string strMailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
                        string strHostAddress = ConfigurationManager.AppSettings["HostAddress"].ToString();
                        string strPortNo = ConfigurationManager.AppSettings["PortNo"].ToString();
                        bool ssl = ConfigurationManager.AppSettings["EnableSsl"].ToString() == "false" ? false : true;
                        string MailFrom = ConfigurationManager.AppSettings["NetworkUsername"].ToString();

                        string RetVal = string.Empty;
                        STSTRAVRAYS.Mailref.MyService pp = new STSTRAVRAYS.Mailref.MyService();
                        pp.Url = ConfigurationManager.AppSettings["mailurl"].ToString();
                        if (strAgentmailToID != null && strAgentmailToID != "")
                        {
                            StringBuilder strBuild = new StringBuilder();
                            strBuild.Append("<div><div style='text-align:center;font-size:20px;'><strong>INDEMNITY BOND</strong></div>");
                            strBuild.Append("<ul><li>We hereby agree and undertake to Riya Travel & Tours Pvt. Ltd that we shall be responsible for any penal/overstay/or any other liabilities for absconding and/or overstay visa applicants, if the criteria, checks & procedures are not followed and/or adhered to by us meticulously, and/or as per the instructions given by Riya Travel & Tours Pvt. Ltd in writing from time to time.</li>");
                            strBuild.Append("<li>We hereby further agree that we shall and will at the instance of Riya Travel & Tours Pvt. Ltd for any overstay/penal liability, remit the said amount to Riya Travel & Tours Pvt. Ltd within 72 hours of intimation about such overstay/penal liability.</li></ul></div>");

                            RetVal = pp.SendMailSingleTicket(Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(),
                                      "W", Session["sequenceid"].ToString(), strAgentmailToID, "", strRefID,
                                    strBuild.ToString(), "VisaTermsAndConditions", "", strMailUserName, strMailPassword, strHostAddress, strPortNo, ssl, MailFrom, ".html");

                            xmlData = "<EVENT><RESPONSE>InsertVisaOffline-ForAgentMail</RESPONSE>"
                                + "<MAILRESPONSE>" + RetVal + "</MAILRESPONSE>"
                                + "</EVENT>";
                            DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                        }
                        if (strVisaToMailID != null && strVisaToMailID != "")
                        {
                            string strAgencyName = Session["agencyname"] != null && Session["agencyname"].ToString() != "" ? Session["agencyname"].ToString() : "";
                            StringBuilder strBuild = new StringBuilder();
                            strBuild.Append("<div><div style='text-align:center;font-size:20px;'><strong>INDEMNITY BOND</strong></div>");
                            strBuild.Append("<div>Agency Name:" + strAgencyName + "</div>");
                            strBuild.Append("<div>Agent ID:" + strAgentId + "</div>");
                            strBuild.Append("<ul><li>We hereby agree and undertake to Riya Travel & Tours Pvt. Ltd that we shall be responsible for any penal/overstay/or any other liabilities for absconding and/or overstay visa applicants, if the criteria, checks & procedures are not followed and/or adhered to by us meticulously, and/or as per the instructions given by Riya Travel & Tours Pvt. Ltd in writing from time to time.</li>");
                            strBuild.Append("<li>We hereby further agree that we shall and will at the instance of Riya Travel & Tours Pvt. Ltd for any overstay/penal liability, remit the said amount to Riya Travel & Tours Pvt. Ltd within 72 hours of intimation about such overstay/penal liability.</li></ul></div>");

                            RetVal = pp.SendMailSingleTicket(Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(),
                                    "W", Session["sequenceid"].ToString(), strVisaToMailID, "", strRefID,
                                  strBuild.ToString(), "VisaTermsAndConditions", "", strMailUserName, strMailPassword, strHostAddress, strPortNo, ssl, MailFrom, ".html");
                            xmlData = "<EVENT><RESPONSE>InsertVisaOffline-ForMail</RESPONSE>"
                                   + "<MAILRESPONSE>" + RetVal + "</MAILRESPONSE>"
                                   + "</EVENT>";
                            DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                        }
                    }
                }
                else
                {
                    status = "00";
                    aryy[Error] = "Unable to update image";
                    xmlData = "<EVENT><RESPONSE>InsertVisaOffline</RESPONSE>" +
                    "<RESULT>Unable to update image</RESULT></EVENT>";
                    DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                }

            }
            catch (Exception ex)
            {
                status = "00";
                aryy[Error] = ex.ToString();
                DatabaseLog.LogData(UserName.ToString(), "X", "VisaController", "InsertVisaOffline", ex.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            return Json(new { Status = status, Path = imgpath, Results = aryy });
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

        private XmlDocument GettingImgRequest(string AgentID, byte[] picbytefirst, byte[] picbytelast, byte[] picbytephoto, byte[] picbytereturn, byte[] picbyteObs, byte[] picbyteMrge,
            byte[] picbyteBirth, string TerminalID, string usrname, string IP, string TerminalType, decimal SequenceID, ref string strErrorMsg, string Pagename, string Functionname,
            string strfirstName, string strmiddleName, string strlastName, string strmobileNumber, string strEmailId, string strPassportNum, string strmaritalstatus, string strreligion,
            string streducation, string strvisitpurpose, string strduration, string strprofession, string strRPNR, string strRefID, string strPaxTitle, string strFlag, ref string strStatus, string strVisaTrackID)
        {
            HttpWebRequest serverRequest = CreateRequestObject();

            byte[] requestBytes = SendImgRequest(AgentID, picbytefirst, picbytelast, picbytephoto, picbytereturn, picbyteObs, picbyteMrge, picbyteBirth, TerminalID, usrname, IP,
                     TerminalType, SequenceID, ref strErrorMsg, Pagename, Functionname, strfirstName, strmiddleName, strlastName, strmobileNumber, strEmailId, strPassportNum,
                    strmaritalstatus, strreligion, streducation, strvisitpurpose, strduration, strprofession, strRPNR, strRefID, strPaxTitle, strFlag, strStatus, strVisaTrackID);

            // Send request to the server
            Stream stream = serverRequest.GetRequestStream();
            stream.Write(requestBytes, 0, requestBytes.Length);
            stream.Close();

            // Receive response
            Stream receiveStream = null;
            HttpWebResponse webResponse;
            XmlDocument filteredDocument = new XmlDocument();
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
                    strStatus = "00";
                    //filteredDocument = FAILED;
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
            filteredDocument = GetResponseDocument(result);

            return filteredDocument;
        }

        private HttpWebRequest CreateRequestObject()
        {

            string URL = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
            HttpWebRequest serverRequest = (HttpWebRequest)WebRequest.Create(URL);

            serverRequest.Method = "POST";
            serverRequest.ContentType = "text/xml";

            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;
            serverRequest.AutomaticDecompression = DecompressionMethods.GZip;

            return serverRequest;
        }

        private byte[] SendImgRequest(string AgentID, byte[] picbytefirst, byte[] picbytelast, byte[] picbytephoto, byte[] picbytereturn, byte[] picbyteObs, byte[] picbyteMrge, byte[] picbyteBirth, string TerminalID, string usrname, string IP,
                 string TerminalType, decimal SequenceID, ref string strErrorMsg, string Pagename, string Functionname, string strfirstName, string strmiddleName, string strlastName, string strmobileNumber, string strEmailId, string strPassportNum,
                 string strmaritalstatus, string strreligion, string streducation, string strvisitpurpose, string strduration, string strprofession, string strRPNR, string strRefID, string strPaxTitle, string strFlag, string statuscode, string strVisaTrackID)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            builder.Append("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            builder.Append("<soap:Body>");
            builder.Append("<InsertVisaOffline xmlns=\"http://tempuri.org/\">");
            builder.Append("<agentID>" + AgentID + "</agentID>");
            builder.Append("<strPassportFirstPage>" + Convert.ToBase64String(picbytefirst) + "</strPassportFirstPage>");
            builder.Append("<strPassportLastPage>" + Convert.ToBase64String(picbytelast) + "</strPassportLastPage>");
            builder.Append("<strPhoto>" + Convert.ToBase64String(picbytephoto) + "</strPhoto>");
            builder.Append("<strTicketCopy>" + Convert.ToBase64String(picbytereturn) + "</strTicketCopy>");
            builder.Append("<PassportObservImage>" + Convert.ToBase64String(picbyteObs) + "</PassportObservImage>");
            builder.Append("<strMarriageCertificateImage>" + Convert.ToBase64String(picbyteMrge) + "</strMarriageCertificateImage>");
            builder.Append("<strBirtCertfificateImage>" + Convert.ToBase64String(picbyteBirth) + "</strBirtCertfificateImage>");
            builder.Append("<FirstName>" + strfirstName + "</FirstName>");
            builder.Append("<MiddleName>" + strmiddleName + "</MiddleName>");
            builder.Append("<LastName>" + strlastName + "</LastName>");
            builder.Append("<MobileNumber>" + strmobileNumber + "</MobileNumber>");
            builder.Append("<EmailId>" + strEmailId + "</EmailId>");
            builder.Append("<PassportNo>" + strPassportNum + "</PassportNo>");
            builder.Append("<MaritalStatus>" + strmaritalstatus + "</MaritalStatus>");
            builder.Append("<Religion>" + strreligion + "</Religion>");
            builder.Append("<Education>" + streducation + "</Education>");
            builder.Append("<VisitPurpose>" + strvisitpurpose + "</VisitPurpose>");
            builder.Append("<Duration>" + strduration + "</Duration>");
            builder.Append("<Profession>" + strprofession + "</Profession>");
            builder.Append("<terminalID>" + TerminalID + "</terminalID>");
            builder.Append("<userName>" + usrname + "</userName>");
            builder.Append("<ipAddress>" + IP + "</ipAddress>");
            builder.Append("<terminalType>" + TerminalType + "</terminalType>");
            builder.Append("<sequenceId>" + SequenceID + "</sequenceId>");
            builder.Append("<strErrorMsg>" + strErrorMsg + "</strErrorMsg>");
            builder.Append("<lstrPageName>" + Pagename + "</lstrPageName>");
            builder.Append("<lstrFunName>" + Functionname + "</lstrFunName>");
            builder.Append("<PNR>" + strRPNR + "</PNR>");
            builder.Append("<RefID>" + strRefID + "</RefID>");
            builder.Append("<PaxTitle>" + strPaxTitle + "</PaxTitle>");
            builder.Append("<Flag>" + strFlag + "</Flag>");
            builder.Append("<strVODTracID>" + strVisaTrackID + "</strVODTracID>");
            builder.Append("</InsertVisaOffline>");
            builder.Append(" </soap:Body>");
            builder.Append("</soap:Envelope>");

            // Convert the SOAP envelope into a byte array
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] requestBytes = encoding.GetBytes(builder.ToString());

            return requestBytes;
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
        #endregion

        #region Thai Visa
        public ActionResult ThaiVisa()
        {
            #region UsageLog
            string PageName = "Thai Visa";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "");
            }
            catch (Exception e) { }
            #endregion

            string strQryAgentID = string.Empty;
            string strQryTerminalID = string.Empty;
            string strQryIPAddress = string.Empty;
            string strQryUserName = string.Empty;
            string strQrySequenceID = string.Empty;
            string strVisapaxcount = string.Empty;
            string strVisaPNR = string.Empty;
            string strBranchid = string.Empty;
            string strAgenttype = string.Empty;
            string strTerminaltype = string.Empty;

            string Encquery = Request.QueryString["SECKEY"] != null && Request.QueryString["SECKEY"].ToString() != "" ? Request.QueryString["SECKEY"].ToString() : "";
            ViewBag.ServerDateTime = Base.LoadServerdatetime();
            if (Encquery != null && Encquery != "")
            {
                string today = DateTime.Now.ToString("dd/MM/yyyy");
                string DecryptQry = Base.DecryptKEY(Encquery, "THAIVISA" + today);

                string[] querystring = DecryptQry.Split('&');
                strQryAgentID = querystring[1] != null && querystring[1].ToString() != "" ? querystring[1].Split('=')[1].ToString() : "";
                strQryTerminalID = querystring[0] != null && querystring[0].ToString() != "" ? querystring[0].Split('=')[1].ToString() : "";
                strQryIPAddress = querystring[2] != null && querystring[2].ToString() != "" ? querystring[2].Split('=')[1].ToString() : "";
                strQryUserName = querystring[3] != null && querystring[3].ToString() != "" ? querystring[3].Split('=')[1].ToString() : "";
                strQrySequenceID = querystring[4] != null && querystring[4].ToString() != "" ? querystring[4].Split('=')[1].ToString() : "";
                strBranchid = querystring[5] != null && querystring[5].ToString() != "" ? querystring[5].Split('=')[1].ToString() : "";
                strAgenttype = querystring[6] != null && querystring[6].ToString() != "" ? querystring[6].Split('=')[1].ToString() : "";
                strTerminaltype = querystring[7] != null && querystring[7].ToString() != "" ? querystring[7].Split('=')[1].ToString() : "";

            }
            else
            {
                strQryAgentID = Session["agentid"] != null && Session["agentid"].ToString() != "" ? Session["agentid"].ToString() : "";
                strQryTerminalID = Session["terminalid"] != null && Session["terminalid"].ToString() != "" ? Session["terminalid"].ToString() : "";
                strQryIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
                strQryUserName = Session["username"] != null && Session["username"].ToString() != "" ? Session["username"].ToString() : "";
                strQrySequenceID = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "";
                strBranchid = Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "";
                strAgenttype = Session["agenttype"] != null && Session["agenttype"].ToString() != "" ? Session["agenttype"].ToString() : "";
                strTerminaltype = Session["TerminalType"] != null && Session["TerminalType"].ToString() != "" ? Session["TerminalType"].ToString() : "";
                if (Request.QueryString["PAXCOUNT"] != null && Request.QueryString["PAXCOUNT"] != "")
                {
                    ViewBag.VisaPaxCount = Request.QueryString["PAXCOUNT"].ToString();
                }
                if (Request.QueryString["VISAFARE"] != null && Request.QueryString["VISAFARE"] != "")
                {
                    ViewBag.VisaFareAmt = Request.QueryString["VISAFARE"].ToString();
                }

            }
            Session["POS_ID"] = strQryAgentID;
            Session["POS_TID"] = strQryTerminalID;
            Session["ipAddress"] = strQryIPAddress;
            Session["UserName"] = strQryUserName;
            Session["sequenceid"] = strQrySequenceID;
            Session["sequenceid"] = strQrySequenceID;
            Session.Add("branchid", strBranchid);
            Session.Add("agenttype", strAgenttype);
            Session.Add("TerminalType", strTerminaltype);
            if (strQryAgentID == null || strQryAgentID == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }

            return View("~/Views/Visa/ThaiVisa.cshtml");

            //string Encquery = Request.QueryString["SECKEY"];
            //string today = DateTime.Now.ToString("dd/MM/yyyy");
            //string DecryptQry = Base.DecryptKEY(Encquery, "THAIVISA" + today);
            //string[] querystring = DecryptQry.Split('&');
            //string strQryAgentID = querystring[1] != null && querystring[1].ToString() != "" ? querystring[1].Split('=')[1].ToString() : "";
            //string strQryTerminalID = querystring[0] != null && querystring[0].ToString() != "" ? querystring[0].Split('=')[1].ToString() : "";
            //string strQryIPAddress = querystring[2] != null && querystring[2].ToString() != "" ? querystring[2].Split('=')[1].ToString() : "";
            //string strQryUserName = querystring[3] != null && querystring[3].ToString() != "" ? querystring[3].Split('=')[1].ToString() : "";
            //string strQrySequenceID = querystring[4] != null && querystring[4].ToString() != "" ? querystring[4].Split('=')[1].ToString() : "";
            //string strBranchid = querystring[5] != null && querystring[5].ToString() != "" ? querystring[5].Split('=')[1].ToString() : "";
            //string strAgenttype = querystring[6] != null && querystring[6].ToString() != "" ? querystring[6].Split('=')[1].ToString() : "";
            //string strTerminaltype = querystring[7] != null && querystring[7].ToString() != "" ? querystring[7].Split('=')[1].ToString() : "";
            //Session["POS_ID"] = strQryAgentID;
            //Session["POS_TID"] = strQryTerminalID;
            //Session["ipAddress"] = strQryIPAddress;
            //Session["UserName"] = strQryUserName;
            //Session["sequenceid"] = strQrySequenceID;
            //Session.Add("branchid", strBranchid);
            //Session.Add("agenttype", strAgenttype);
            //Session.Add("TerminalType", strTerminaltype);


            //string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            //string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            //string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            //string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
            //string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            //if (strAgentId == "" || strTerminalID == "" || strUserName == "")
            //{
            //    return RedirectToAction("SessionExp", "Redirect");
            //}
            //ViewBag.ServerDateTime = Base.LoadServerdatetime();
            // return View("~/Views/Visa/ThaiVisa.cshtml");
        }


        public ActionResult InsertThaiVisa()
        {
            ArrayList ary = new ArrayList();
            ary.Add("");
            ary.Add("");
            int Result = 1;
            int Error = 0;
            string status = string.Empty;
            string strErrorMsg = string.Empty;
            string xmlData = string.Empty;
            string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strTerminalType = (Session["TerminalType"] != null && Session["TerminalType"].ToString() != "") ? Session["TerminalType"].ToString() : "";
            string strBranchID = Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "";
            string strCurrency = Session["App_currency"] != null && Session["App_currency"].ToString() != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
            // string strUniqueID = Session["UniqueIduser"] != null ? Session["UniqueIduser"].ToString() : "";
            //  string strProductCode = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"].ToString() != "" ? Session["PRODUCT_CODE"].ToString() : "";
            string strEnvironment = (Session["TerminalType"] != null && Session["TerminalType"].ToString() != "") ? Session["TerminalType"].ToString() : strTerminalType;
            string strFileName = string.Empty;
            string StartupPath = string.Empty;
            string imgpath = string.Empty;
            string statuscode = string.Empty;
            string strURLpath = string.Empty;
            string strResponse = "";
            Double strAmount = 0;
            try
            {
                RQRS.Booking_RQ bookingrq = new RQRS.Booking_RQ();
                List<RQRS.Applicants> lstapplicants = new List<RQRS.Applicants>();
                List<RQRS.DocumentList> _lstDocuments = new List<RQRS.DocumentList>();
                RQRS.DocumentList _DocList = new RQRS.DocumentList();
                RQRS.Applicants _Applicants = new RQRS.Applicants();
                RQRS.ApplicantDetails _Appdetails = new RQRS.ApplicantDetails();
                RQRS.PassportDetails _PassDetails = new RQRS.PassportDetails();
                RQRS.TravelDetails _TravelDetails = new RQRS.TravelDetails();
                RQRS.AccommodationDetails _AccomodationDetails = new RQRS.AccommodationDetails();
                RQRS.AgentDetails _AgentDetail = new RQRS.AgentDetails();
                DataSet dsset = new DataSet();
                FileStream fs;
                byte[] bytConvrtPassPrt = new byte[] { };
                //byte[] bytPassBioPage = new byte[] { };
                //byte[] bytPassPhoto = new byte[] { };
                //byte[] bytPassArrflightTckt = new byte[] { };
                //byte[] bytPassDeptflightTckt = new byte[] { };
                //byte[] bytPassAccomodation = new byte[] { };
                //byte[] bytPassInviteLetter = new byte[] { };
                //byte[] bytPassAddrProof = new byte[] { };
                //byte[] bytPassID = new byte[] { };
                //byte[] bytPassAddDoc1 = new byte[] { };
                //byte[] bytPassAddDoc2 = new byte[] { };
                string strPaxCount = System.Web.HttpContext.Current.Request["strPassengerCnt"];
                int PaxCount = Convert.ToInt32(strPaxCount);
                Session.Add("strPaxCount", Convert.ToString(PaxCount));
                string Results = string.Empty;
                string strArrAirport = ""; string strArrDate = ""; string strArrTime = ""; string strArrFlightNo = ""; string strArrAirlines = ""; string strArrPNR = "";
                string strDeptDate = ""; string strDeptTime = ""; string strDeptFlightNo = ""; string strDeptAirlines = ""; string strDeptPNR = ""; string strPurposeofVisit = "";
                string strBoardedCountry = ""; string strNextCity = ""; string strFirstTimeTravel = ""; string strTypofFlight = ""; string strLengthofStay = ""; string strMinorTravel = "";
                string strMinorFamilyName = ""; string strMinorGivenName = ""; string strMinorGender = ""; string strMinorDOB = ""; string strPlaceofBirth = ""; string strPlaceofStay = "";
                string strOwnerName = ""; string strCity = ""; string strDistrict = ""; string strSubDistrict = ""; string strAccPincode = ""; string strStreetAddr = ""; string strRefDetails = "";
                string strFlag = ""; string strPriority = "";
                strArrAirport = System.Web.HttpContext.Current.Request["strArrAirport_1"];
                strArrDate = System.Web.HttpContext.Current.Request["strArrDate_1"];
                strArrTime = System.Web.HttpContext.Current.Request["strArrTime_1"];
                strArrFlightNo = System.Web.HttpContext.Current.Request["strArrFlightNo_1"];
                strArrAirlines = System.Web.HttpContext.Current.Request["strArrAirlines_1"];
                strArrPNR = System.Web.HttpContext.Current.Request["strArrPNR_1"];
                strDeptDate = System.Web.HttpContext.Current.Request["strDeptDate_1"];
                strDeptTime = System.Web.HttpContext.Current.Request["strDeptTime_1"];
                strDeptFlightNo = System.Web.HttpContext.Current.Request["strDeptFlightNo_1"];
                strDeptAirlines = System.Web.HttpContext.Current.Request["strDeptAirlines_1"];
                strDeptPNR = System.Web.HttpContext.Current.Request["strDeptPNR_1"];
                strPurposeofVisit = System.Web.HttpContext.Current.Request["strPurposeofVisit_1"];
                strBoardedCountry = System.Web.HttpContext.Current.Request["strBoardedCountry_1"];
                strNextCity = System.Web.HttpContext.Current.Request["strNextCity_1"];
                strFirstTimeTravel = System.Web.HttpContext.Current.Request["strFirstTimeTravel_1"];
                strTypofFlight = System.Web.HttpContext.Current.Request["strTypofFlight_1"];
                strLengthofStay = System.Web.HttpContext.Current.Request["strLengthofStay_1"];
                strMinorTravel = System.Web.HttpContext.Current.Request["strMinorTravel_1"];
                strMinorFamilyName = System.Web.HttpContext.Current.Request["strMinorFamilyName_1"];
                strMinorGivenName = System.Web.HttpContext.Current.Request["strMinorGivenName_1"];
                strMinorGender = System.Web.HttpContext.Current.Request["strMinorGender_1"];
                strMinorDOB = System.Web.HttpContext.Current.Request["strMinorDOB_1"];
                strPlaceofBirth = System.Web.HttpContext.Current.Request["strPlaceofBirth_1"];
                strPlaceofStay = System.Web.HttpContext.Current.Request["strPlaceofStay_1"];
                strOwnerName = System.Web.HttpContext.Current.Request["strOwnerName_1"];
                strCity = System.Web.HttpContext.Current.Request["strCity_1"];
                strDistrict = System.Web.HttpContext.Current.Request["strDistrict_1"];
                strSubDistrict = System.Web.HttpContext.Current.Request["strSubDistrict_1"];
                strAccPincode = System.Web.HttpContext.Current.Request["strAccPincode_1"];
                strStreetAddr = System.Web.HttpContext.Current.Request["strStreetAddr_1"];
                strRefDetails = System.Web.HttpContext.Current.Request["strRefDetails_1"];
                strFlag = System.Web.HttpContext.Current.Request["strflag_1"];
                strPriority = System.Web.HttpContext.Current.Request["strPriority_1"];
                string strPlcOfStyName = System.Web.HttpContext.Current.Request["strPlcOfStyName_1"];
                string strCityName = System.Web.HttpContext.Current.Request["strCityName_1"];
                string strDistrictName = System.Web.HttpContext.Current.Request["strDistrictName_1"];
                string strSubDistrictName = System.Web.HttpContext.Current.Request["strSubDistrictName_1"];
                CultureInfo cii = new CultureInfo("en-GB", true);
                cii.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                if (strArrDate != null && strArrDate != "")
                {
                    strArrDate = Convert.ToDateTime(strArrDate.Trim(), cii).ToString("yyyy-MM-dd");
                }
                if (strDeptDate != null && strDeptDate != "")
                {
                    strDeptDate = Convert.ToDateTime(strDeptDate.Trim(), cii).ToString("yyyy-MM-dd");
                }

                _TravelDetails = new RQRS.TravelDetails();
                _AccomodationDetails = new RQRS.AccommodationDetails();
                for (var j = 1; j <= PaxCount; j++)
                {
                    _Appdetails = new RQRS.ApplicantDetails();
                    _PassDetails = new RQRS.PassportDetails();
                    _lstDocuments = new List<RQRS.DocumentList>();
                    _AgentDetail = new RQRS.AgentDetails();
                    _Applicants = new RQRS.Applicants();
                    string strTitle = System.Web.HttpContext.Current.Request["strTitle_" + j];
                    string strfirstName = System.Web.HttpContext.Current.Request["strFirstName_" + j];
                    string strmiddleName = System.Web.HttpContext.Current.Request["strMiddleName_" + j];
                    string strlastName = System.Web.HttpContext.Current.Request["strLastName_" + j];
                    string strEmailID = System.Web.HttpContext.Current.Request["strEmailID_" + j];
                    string strMobileNumber = System.Web.HttpContext.Current.Request["strMobileNumber_" + j];
                    string strNationality = System.Web.HttpContext.Current.Request["strNationality_" + j];
                    string strPassportNumber = System.Web.HttpContext.Current.Request["strPassportNumber_" + j];
                    string strPassIssueDate = System.Web.HttpContext.Current.Request["strPassIssueDate_" + j];
                    string strPassExpDate = System.Web.HttpContext.Current.Request["strPassExpDate_" + j];
                    string strPassDOB = System.Web.HttpContext.Current.Request["strPassDOB_" + j];
                    string strGender = System.Web.HttpContext.Current.Request["strGender_" + j];
                    string strPassType = System.Web.HttpContext.Current.Request["strPassType_" + j];
                    string strPermanentAddr = System.Web.HttpContext.Current.Request["strPermanentAddr_" + j];
                    string strState = System.Web.HttpContext.Current.Request["strState_" + j];
                    string strYearlyIncome = System.Web.HttpContext.Current.Request["strYearlyIncome_" + j];
                    string strPlcofIssue = System.Web.HttpContext.Current.Request["strPlcofIssue_" + j];
                    string strOccupation = System.Web.HttpContext.Current.Request["strOccupation_" + j];
                    strAmount += System.Web.HttpContext.Current.Request["strAmount_" + j] != null && System.Web.HttpContext.Current.Request["strAmount_" + j] != "" ? Convert.ToDouble(System.Web.HttpContext.Current.Request["strAmount_" + j]) : 0;
                    string strPassFileSize = "";
                    string strNationalityName = System.Web.HttpContext.Current.Request["strNationalityName_" + j];
                    string strGenderName = System.Web.HttpContext.Current.Request["strGenderName_" + j];
                    string strTitleName = System.Web.HttpContext.Current.Request["strTitleName_" + j];
                    string strPassportTypName = System.Web.HttpContext.Current.Request["strPassportTypName_" + j];
                    string strOccupationName = System.Web.HttpContext.Current.Request["strOccupationName_" + j];

                    #region RQRS

                    strPassExpDate = Convert.ToDateTime(strPassExpDate.Trim(), cii).ToString("yyyy-MM-dd");
                    strPassIssueDate = Convert.ToDateTime(strPassIssueDate.Trim(), cii).ToString("yyyy-MM-dd");
                    strPassDOB = Convert.ToDateTime(strPassDOB.Trim(), cii).ToString("yyyy-MM-dd");

                    if (strMinorDOB != null && strMinorDOB != "")
                    {
                        strMinorDOB = Convert.ToDateTime(strMinorDOB.Trim(), cii).ToString("yyyy-MM-dd");
                    }
                    _Appdetails.PaymentReceiptRefNumber = "";
                    _Appdetails.ApplicantName = strfirstName + strlastName;
                    _Appdetails.Email = strEmailID;
                    _Appdetails.Mobile = strMobileNumber;
                    _Appdetails.IsPriority = strPriority != null && strPriority != "" ? Convert.ToInt32(strPriority) : 0;
                    _Appdetails.ISDCode = "+91";
                    _Appdetails.NationalityId = Convert.ToInt32(strNationality);
                    _Applicants.ApplicantDetails = _Appdetails;
                    _Appdetails.Nationality = strNationalityName;
                    //Passenger Details (Passport)
                    _PassDetails.GivenName = strfirstName;
                    _PassDetails.SurName = strlastName;
                    _PassDetails.MiddleName = strmiddleName;
                    _PassDetails.PassportNumber = strPassportNumber;
                    _PassDetails.PassportExpiryDate = strPassExpDate;
                    _PassDetails.PassportIssueDate = strPassIssueDate;
                    _PassDetails.DateOfBirth = strPassDOB;
                    _PassDetails.ResidencyId = Convert.ToInt32(strNationality);
                    _PassDetails.GenderId = Convert.ToInt32(strGender);
                    _PassDetails.SalutationId = Convert.ToInt32(strTitle);
                    _PassDetails.PassportTypeId = Convert.ToInt32(strPassType);
                    _PassDetails.PermanentAddress = strPermanentAddr;
                    _PassDetails.State = strState;
                    _PassDetails.YearlyIncome = Convert.ToInt32(strYearlyIncome);
                    _PassDetails.PlaceOfIssue = strPlcofIssue;
                    _PassDetails.OccupationId = Convert.ToInt32(strOccupation);
                    _PassDetails.Gender = strGenderName;
                    _PassDetails.Salutation = strTitleName;
                    _PassDetails.PassportType = strPassportTypName;
                    _PassDetails.Occupation = strOccupationName;
                    _Applicants.PassportDetails = _PassDetails;
                    #endregion


                    HttpFileCollectionBase files = Request.Files;
                    if (files.Count > 0)
                    {
                        var count = 0;
                        if (files.Count == 1)
                        {
                            count = files.Count;
                        }
                        else
                        {
                            count = files.Count - 1;
                        }
                        for (int i = 0; i < count; i++)
                        {
                            _DocList = new RQRS.DocumentList();
                            HttpPostedFileBase file1 = files[i];
                            string fileExt1 = System.IO.Path.GetExtension(file1.FileName);
                            string fname;
                            string filename = "";
                            if (fileExt1 == ".jpg" || fileExt1 == ".pdf")
                            {
                                strFileName = files.AllKeys[i];
                                string fileExt = System.IO.Path.GetExtension(file1.FileName);
                                var fileName = Session["POS_ID"].ToString() + fileExt;
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = file1.FileName.Split(new char[] { '\\' });
                                    filename = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    filename = file1.FileName;
                                }

                                fname = Path.Combine(Server.MapPath(@"~/Content/FILES/VISA/VisaImages/"), filename);
                                file1.SaveAs(fname);
                                if (strFileName == "PassportCover_" + j)
                                {
                                    if (fileExt1 == ".jpg" || fileExt1 == ".jpeg")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = ReadBitmap2ByteArray(StartupPath);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["PassCoverfileSize_" + j];
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select .jpg/jpeg files to upload";
                                    }
                                }
                                else if (strFileName == "imgPassBio_" + j)
                                {
                                    if (fileExt1 == ".jpg" || fileExt1 == ".jpeg")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = ReadBitmap2ByteArray(StartupPath);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["PassBiofileSize_" + j];
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select .jpg/jpeg files to upload";
                                    }
                                }
                                else if (strFileName == "imgPassPhoto_" + j)
                                {
                                    if (fileExt1 == ".jpg" || fileExt1 == ".jpeg")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = ReadBitmap2ByteArray(StartupPath);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["PassPhotofileSize_" + j];
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select .jpg/jpeg files to upload";
                                    }
                                }
                                else if (strFileName == "pdfPassArrivalFlightTckt_" + j)
                                {
                                    if (fileExt1 == ".pdf")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = System.IO.File.ReadAllBytes(StartupPath);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["PassArrfileSize_" + j];
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select pdf files to upload";
                                    }
                                }
                                else if (strFileName == "pdfDeptFlightTckt_" + j)
                                {
                                    if (fileExt1 == ".pdf")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = System.IO.File.ReadAllBytes(StartupPath);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["PassDeptfileSize_" + j];
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select pdf files to upload";
                                    }
                                }
                                else if (strFileName == "pdfPassAccomodation_" + j)
                                {
                                    if (fileExt1 == ".pdf")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = System.IO.File.ReadAllBytes(StartupPath);
                                        // System.IO.File.WriteAllBytes(StartupPath, bytPassAccomodation);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["PassAccfileSize_" + j];
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select pdf files to upload";
                                    }
                                }
                                else if (strFileName == "pdfInvitationLettr_" + j)
                                {
                                    if (fileExt1 == ".pdf")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = System.IO.File.ReadAllBytes(StartupPath);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["PassInvitefileSize_" + j];
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select pdf files to upload";
                                    }
                                }
                                else if (strFileName == "pdfHouseReg_" + j)
                                {
                                    if (fileExt1 == ".pdf")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = System.IO.File.ReadAllBytes(StartupPath);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["PassHousefileSize_" + j];
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select pdf files to upload";
                                    }
                                }
                                else if (strFileName == "pdfIDCard_" + j)
                                {
                                    if (fileExt1 == ".pdf")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = System.IO.File.ReadAllBytes(StartupPath);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["PassIDfileSize_" + j];
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select pdf files to upload";
                                    }
                                }
                                else if (strFileName == "imgAddDoct1_" + j)
                                {
                                    if (fileExt1 == ".pdf")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = System.IO.File.ReadAllBytes(StartupPath);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["PassDoc1fileSize_" + j];
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select pdf files to upload";
                                    }
                                }
                                else if (strFileName == "imgAddDoct2_" + j)
                                {
                                    if (fileExt1 == ".pdf")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = System.IO.File.ReadAllBytes(StartupPath);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["PassDoc2fileSize_" + j];
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select pdf files to upload";
                                    }
                                }
                                System.IO.File.Delete(fname);
                                //List of Documents
                                _DocList.DocumentListId = i;
                                _DocList.FileContent = Convert.ToBase64String(bytConvrtPassPrt);
                                _DocList.FileName = filename;
                                _DocList.MimeType = fileExt1;
                                _DocList.FileSize = strPassFileSize;
                                DatabaseLog.LogData(strUserName, "E", "VisaContoller", "InsertThaiVisa", fname, strAgentId, strTerminalID, strSequenceId);
                            }
                            else
                            {
                                status = "00";
                                ary[Error] = "Please select jpg/jpeg/pdf files to upload";
                                xmlData = "<EVENT><RESPONSE>InsertThaiVisa</RESPONSE>" +
                                         "<RESULT>Please select png image files to upload</RESULT></EVENT>";
                                DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertThaiVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                                return Json(new { Status = status, Path = "", Results = ary });
                            }
                            _lstDocuments.Add(_DocList);
                        }
                        _Applicants.DocumentList = _lstDocuments;
                    }
                    lstapplicants.Add(_Applicants);
                }


                //_TravelDetails 
                _TravelDetails.ArrivalAirportId = strArrAirport;
                _TravelDetails.ArrivalDate = strArrDate;
                _TravelDetails.ArrivalTime = strArrTime;
                _TravelDetails.ArrivalFlightNumber = strArrFlightNo;
                _TravelDetails.ArrivalAirlinesId = Convert.ToInt32(strArrAirlines);
                _TravelDetails.ArrivalPNRNumber = strArrPNR;
                _TravelDetails.DepartureDate = (string.IsNullOrEmpty(strDeptDate) || string.IsNullOrEmpty(strDeptTime) || string.IsNullOrEmpty(strDeptFlightNo)) ? strArrDate : strDeptDate;
                _TravelDetails.DepartureTime = (string.IsNullOrEmpty(strDeptDate) || string.IsNullOrEmpty(strDeptTime) || string.IsNullOrEmpty(strDeptFlightNo)) ? strArrTime : strDeptTime;
                _TravelDetails.DepartureFlightNumber = (string.IsNullOrEmpty(strDeptDate) || string.IsNullOrEmpty(strDeptTime) || string.IsNullOrEmpty(strDeptFlightNo)) ? strArrFlightNo : strDeptFlightNo;
                _TravelDetails.DepartureAirlinesId = (string.IsNullOrEmpty(strDeptDate) || string.IsNullOrEmpty(strDeptTime) || string.IsNullOrEmpty(strDeptFlightNo)) ? strArrAirlines : strDeptAirlines;
                _TravelDetails.DeparturePNRNumber = (string.IsNullOrEmpty(strDeptDate) || string.IsNullOrEmpty(strDeptTime) || string.IsNullOrEmpty(strDeptFlightNo)) ? strArrPNR : strDeptPNR;
                _TravelDetails.PurposeOfVisit = Convert.ToInt32(strPurposeofVisit);
                _TravelDetails.BoardedCountry = strBoardedCountry;
                _TravelDetails.NextCity = strNextCity;
                _TravelDetails.FirstTimeTraveller = strFirstTimeTravel;
                _TravelDetails.TypeOfFlight = strTypofFlight;
                _TravelDetails.LengthOfStay = strLengthofStay;
                _TravelDetails.IsMinorTravelingOnSamePassport = Convert.ToInt32(strMinorTravel);
                _TravelDetails.MinorFamilyName = strMinorFamilyName;
                _TravelDetails.MinorGivenName = strMinorGivenName;
                _TravelDetails.MinorGender = strMinorGender != null && strMinorGender != "" && strMinorGender != "undefined" ? Convert.ToInt32(strMinorGender) : 1;
                _TravelDetails.MinorDateOfBirth = strMinorDOB;
                _TravelDetails.MinorPlaceOfBirth = strPlaceofBirth;

                //_AccomodationDetails
                _AccomodationDetails.PlaceOfStayId = Convert.ToInt32(strPlaceofStay);
                _AccomodationDetails.AccommodationOrOwnerName = strOwnerName;
                _AccomodationDetails.CityId = Convert.ToInt32(strCity);
                _AccomodationDetails.DistrictId = Convert.ToInt32(strDistrict);
                _AccomodationDetails.SubDistrictId = Convert.ToInt32(strSubDistrict);
                _AccomodationDetails.PinCode = strAccPincode;
                _AccomodationDetails.StreetAddress = strStreetAddr;
                _AccomodationDetails.ReferenceDetails = strRefDetails;
                _AccomodationDetails.PlaceOfStay = strPlcOfStyName;
                _AccomodationDetails.City = strCityName;
                _AccomodationDetails.District = strDistrictName;
                _AccomodationDetails.SubDistrict = strSubDistrictName;
                //_AgentDetail
                _AgentDetail.AgentId = strAgentId;
                _AgentDetail.ClientID = strAgentId;
                _AgentDetail.TerminalId = strTerminalID;
                _AgentDetail.UserName = strUserName;
                _AgentDetail.AppType = "B2B";
                _AgentDetail.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                _AgentDetail.Environment = strEnvironment;
                _AgentDetail.BOAID = strAgentId;
                _AgentDetail.BOAterminalID = strTerminalID;
                _AgentDetail.Agenttype = strTerminalType;
                _AgentDetail.CoOrdinatorID = "";
                _AgentDetail.AirportID = "";
                _AgentDetail.BranchID = strBranchID;
                _AgentDetail.IssuingBranchID = strBranchID;
                _AgentDetail.EMP_ID = "";
                _AgentDetail.COST_CENTER = "";
                _AgentDetail.Ipaddress = IPAddress;
                _AgentDetail.Sequence = strSequenceId;
                _AgentDetail.Bargain_Cred = "";
                _AgentDetail.Personal_Booking = "";
                _AgentDetail.GST_FLAG = "";
                _AgentDetail.Platform = "B";
                _AgentDetail.ProjectID = "";
                _AgentDetail.UID = "";
                _AgentDetail.Group_ID = "";
                _AgentDetail.APPCurrency = strCurrency;
                _AgentDetail.TID = "";
                _AgentDetail.COMMISSION_REF = "";
                _AgentDetail.SERVICE_TAX_REF = "";
                _AgentDetail.SERVICE_TAX = "";
                _AgentDetail.TDS_TAX = "";
                _AgentDetail.MARKUP_REF = "";
                _AgentDetail.AGENT_SERVICE_FEE_REF = "";


                bookingrq.AgentDetail = _AgentDetail;
                bookingrq.BatchReference = "";
                bookingrq.PaymentBatchReference = strTerminalID;
                bookingrq.NoOfApplicants = PaxCount.ToString();
                bookingrq.Amount = strAmount.ToString();
                bookingrq.TravelDetails = _TravelDetails;
                bookingrq.AccommodationDetails = _AccomodationDetails;
                bookingrq.Applicants = lstapplicants;
                bookingrq.PaymentMode = "P";
                bookingrq.TrackID = "";
                bookingrq.Platform = "";
                bookingrq.Stock = "THAI";
                bookingrq.blCreateTrack = true;

                int Amount = Convert.ToInt32(strAmount);
                #region APIRequest & Response for Generate TrackID
                int bookingtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                string Query = "GetVisaBooking";
                MyWebClient client = new MyWebClient();
                client.LintTimeout = bookingtimeout;
                client.Headers["Content-type"] = "application/json";

                strURLpath = ConfigurationManager.AppSettings["VISAAPIURL"];
                string request = Newtonsoft.Json.JsonConvert.SerializeObject(bookingrq).ToString();
                Session.Add("strThaiVisaReq", request);
                string ReqTime = "";
                ReqTime = "InsertThaiVisa" + DateTime.Now;
                xmlData = "<EVENT><REQUEST>InsertThaiVisa -GenerateTrackID Request</REQUEST>" +
                    "<REQDATA>" + request.ToString() + "</REQDATA>" +
                    "</EVENT>";
                DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertThaiVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                strResponse = System.Text.Encoding.ASCII.GetString(data);
                RQRS.Booking_RS _BookingResponse = JsonConvert.DeserializeObject<RQRS.Booking_RS>(strResponse);
                xmlData = "<EVENT><RESPONSE>InsertThaiVisa-GenerateTrackID Response</RESPONSE>" +
                    "<RESDATA>" + strResponse.ToString() + "</RESDATA>" +
                    "</EVENT>";
                DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertThaiVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                #endregion
                string strPayUrl = "";
                string strPathPayment = "";
                string strpaymentTrackID = "";
                string strPaymentPath = "";
                #region PGRequest
                if (_BookingResponse.Status.ResultCode == "1" && (_BookingResponse.TrackID != null && _BookingResponse.TrackID != ""))
                {
                    strpaymentTrackID = (_BookingResponse.TrackID != null && _BookingResponse.TrackID.ToString() != "") ? _BookingResponse.TrackID : "";
                    Session.Add("strTrackID", strpaymentTrackID);
                    strPathPayment = strAgentId + "@" + strTerminalID + "@" + strTerminalType + "@" + strUserName + "@" + IPAddress + "@" + strSequenceId + "@" +
                        strpaymentTrackID + "@" + "B" + "@" + "VISA" + "@" + Amount + "@" + ConfigurationManager.AppSettings["VISAPGRESPONSEURL"].ToString() + "@" +
                        strAgentId + "@" + strBranchID; /*PGRESPONSEURL*/
                    xmlData = "<EVENT><REQUEST>InsertThaiVisa-PG Request</REQUEST>" + "<REQDATA>" + strPathPayment + "</REQDATA>" + "</EVENT>";
                    DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertThaiVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                    strPayUrl = ConfigurationManager.AppSettings["VISAPGREQUESTURL"].ToString() + "?AGENTID=" + strAgentId + "&URL=" + ConfigurationManager.AppSettings["VISAPGRESPONSEURL"].ToString() + "&PGKEY=" + Base.EncryptKEy(strPathPayment, "SKV" + strAgentId.ToString().ToUpper().Trim());
                    strPaymentPath = strPathPayment;
                    status = "01";
                    ViewBag.PaymentURL = strPayUrl;
                    ary[Result] = strPayUrl;
                    return Json(new { Status = status, Message = "", Result = ary });
                }
                else
                {
                    ary[Error] = "Problem Occured While Booking.Please Contact Customercare.";
                    xmlData = "<EVENT><REQUEST>InsertThaiVisa-PG Request</REQUEST>" + "<RESDATE>FAILED</RESDATE>" + "</EVENT>";
                    DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertThaiVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                }
                #endregion
            }
            catch (Exception ex)
            {
                status = "00";
                ary[Error] = ex.ToString();
                DatabaseLog.LogData(strUserName.ToString(), "X", "VisaController", "InsertThaiVisa", ex.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            return Json(new { Status = status, Path = imgpath, Results = ary });
        }

        public ActionResult PGRedirectpage()
        {
            ArrayList ary = new ArrayList(2);
            ary.Add("");
            ary.Add("");
            int Error = 0;
            int Result = 1;
            string strURLpath = "";
            string strResponse = "";
            string Status = string.Empty;
            string strResult = string.Empty;
            string xmlData = string.Empty;
            string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strTerminalType = (Session["terminaltype"] != null && Session["terminaltype"].ToString() != "") ? Session["terminaltype"].ToString() : "";
            string TrackID = (Session["strTrackID"] != null && Session["strTrackID"].ToString() != "") ? Session["strTrackID"].ToString() : "";
            string strPaxCount = Session["strPaxCount"] != null ? Session["strPaxCount"].ToString() : "";
            int PaxCount = Convert.ToInt32(strPaxCount);
            try
            {
                InplantService.Inplantservice Inplant_wsdl = new InplantService.Inplantservice();
                Inplant_wsdl.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();

                string strErrorMsg = string.Empty;
                int bookingtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                string strTrackID = Request.QueryString["paymentId"];
                string strPaymentID = Request.QueryString["trackid"];
                string Query = "GetVisaBooking";
                MyWebClient client = new MyWebClient();
                client.LintTimeout = bookingtimeout;
                client.Headers["Content-type"] = "application/json";
                string strRequest = Session["strThaiVisaReq"] != null && Session["strThaiVisaReq"].ToString() != "" ? Session["strThaiVisaReq"].ToString() : "";
                RQRS.Booking_RQ bookingrq = JsonConvert.DeserializeObject<RQRS.Booking_RQ>(strRequest);
                RQRS.Booking_RS _BookingResponse = new RQRS.Booking_RS();
                List<RQRS.ReferenceList> _lstRefList = new List<RQRS.ReferenceList>();
                RQRS.ReferenceList _RefList = new RQRS.ReferenceList();
                RQRS.ApplicantDetails lstApplDetails = new RQRS.ApplicantDetails();
                List<RQRS.ApplicantDetails> _lstAppDet = new List<RQRS.ApplicantDetails>();
                bookingrq.BatchReference = strPaymentID;
                bookingrq.TrackID = strTrackID;
                bookingrq.blCreateTrack = false;
                string strPGResult = "";
                string strError = Request.QueryString["ErrorText"];
                string strAmount = Request.QueryString["amt"] != null && Request.QueryString["amt"] != "" ? Convert.ToDecimal(Request.QueryString["amt"]).ToString() : "0";
                for (var i = 0; i < PaxCount; i++)
                {
                    lstApplDetails = new RQRS.ApplicantDetails();
                    bookingrq.Applicants[i].ApplicantDetails.PaymentReceiptRefNumber = strPaymentID;
                    lstApplDetails = bookingrq.Applicants[i].ApplicantDetails;
                    _lstAppDet.Add(lstApplDetails);
                }


                strURLpath = ConfigurationManager.AppSettings["VISAAPIURL"];
                ViewBag.strPassengerDetail = _lstAppDet;
                //FetchPg details
                string strFlagTemp = string.Empty;
                bool JsonSequence = Inplant_wsdl.Fetch_PG_Payment_Details(strPaymentID, "", ref strFlagTemp, ref strErrorMsg);
                xmlData = "<THREAD_RESPONSE><DATA>" + strFlagTemp + "</DATA></THREAD_RESPONSE>";
                DatabaseLog.LogData(strUserName, "E", "VisaController", "PGRedirectpage-Fetch_PG_Track", xmlData.ToString(), strAgentId, strTerminalID, strSequenceId);

                //Check Pg
                if (JsonSequence == true)
                {
                    strPGResult = Request.QueryString["result"];
                }
                xmlData = "<REQUEST>PGRedirectpage-Verified_Request" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</REQUEST>" +
                        "<Trackid>" + strPaymentID.ToString().Trim() + "</Trackid>" +
                         "<PaymentId>" + strPaymentID.ToString().Trim() + "</PaymentId>" +
                        "<strAmount>" + strAmount.ToString().Trim() + "</strAmount>";
                DatabaseLog.LogData(strUserName, "E", "VisaController", "PGRedirectpage-Verified", xmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                string remarks = "CAPTURED-" + strPaymentID;
                bool status = Inplant_wsdl.Check_Payment_Gateway_Track(strPaymentID.ToString().Trim(),
                                      "V", strPaymentID.ToString().Trim(), strAmount.ToString().Trim(), ref strErrorMsg);

                xmlData = "<Response>PGRedirectpage Verified_Response" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</Response>" +
                         "<Trackid>" + strPaymentID.ToString().Trim() + "</Trackid>" +
                          "<PaymentId>" + strPaymentID.ToString().Trim() + "</PaymentId>" +
                         "<strAmount>" + strAmount.ToString().Trim() + "</strAmount>" +
                          "<status>" + status.ToString() + "</status>";
                DatabaseLog.LogData(strUserName, "E", "VisaController", "PGRedirectpage-Verified", xmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                //Update track Id
                if (status == true)
                {
                    if (!string.IsNullOrEmpty(strPGResult) && (strPGResult.ToString().Trim().ToUpper() == "TRUE" || strPGResult.ToString().Trim().ToUpper() == "CAPTURED" || strPGResult.ToString().Trim().ToUpper() == "APPROVED" || Result.ToString().Trim().ToUpper() == "TRANSACTION SUCCESSFUL"))
                    {
                        string XMLdataremarks = "<THREAD_REQUEST><URL>[<![CDATA[" + strURLpath + "]]>]</URL><DATA>" +
                               "[<![CDATA[" + remarks + "]]>]</DATA></THREAD_REQUEST>";

                        xmlData = "<REQUEST>PGRedirectpage_Update_Request" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</REQUEST>" +
                         "<Trackid>" + strPaymentID.ToString().Trim() + "</Trackid>" +
                          "<remarks>" + XMLdataremarks + "</remarks>" +
                         "<Reference>" + strPaymentID + "</Reference>" +
                         "<Result>" + strPGResult.ToString().Trim().ToUpper() + "</Result>";
                        DatabaseLog.LogData(strUserName, "E", "VisaController", "PGRedirectpage-Update", xmlData.ToString(), strAgentId, strTerminalID, strSequenceId);

                        status = Inplant_wsdl.Update_Payment_Gateway_Track(strPaymentID.ToString().Trim(),
                                       "S", remarks, strPaymentID, IPAddress.ToString().Trim(), 0, strPaymentID, strPGResult.ToString().Trim().ToUpper(), ref strErrorMsg);

                        xmlData = "<RESPONSE>PGRedirectpage_Update_Response_Success" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</RESPONSE>" +
                  "<Trackid>" + strPaymentID.ToString().Trim() + "</Trackid>" +
                   "<remarks>" + XMLdataremarks + "</remarks>" +
                  "<Reference>" + strPaymentID + "</Reference>" +
                  "<Result>" + strPGResult.ToString().Trim().ToUpper() + "</Result>" +
                   "<status>" + status + "</status>";
                        DatabaseLog.LogData(strUserName, "E", "VisaController", "PGRedirectpage-Update", xmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                    }
                    else
                    {
                        DatabaseLog.LogData(strUserName, "E", "VisaController", "PGRedirectpage-CheckPGFailed", xmlData.ToString(), strAgentId, strTerminalID, strSequenceId);

                    }
                }
                else
                {
                    DatabaseLog.LogData(strUserName, "E", "VisaController", "PGRedirectpage-AlreadyUpdated", xmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                }
                if (!string.IsNullOrEmpty(strTrackID))
                {
                    if (!string.IsNullOrEmpty(strPGResult) && (strPGResult.ToString().Trim().ToUpper() == "TRUE" || strPGResult.ToString().Trim().ToUpper() == "CAPTURED" || strPGResult.ToString().Trim().ToUpper() == "APPROVED" || Result.ToString().Trim().ToUpper() == "TRANSACTION SUCCESSFUL") && strPaymentID != null && strPaymentID != "")
                    {
                        ViewBag.strPayment = strPGResult;

                        string request = Newtonsoft.Json.JsonConvert.SerializeObject(bookingrq).ToString();
                        xmlData = "<EVENT><REQUEST>PGRedirectpage</REQUEST>" +
                            "<REQDATA>" + request.ToString() + "</REQDATA></EVENT>";
                        DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "PGRedirectpage", xmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                        byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                        strResponse = System.Text.Encoding.ASCII.GetString(data);
                        xmlData = "<EVENT><RESPONSE>PGRedirectpage</RESPONSE>" +
                           "<RESDATA>" + strResponse.ToString() + "</RESDATA></EVENT>";
                        DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "PGRedirectpage", xmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                        _BookingResponse = JsonConvert.DeserializeObject<RQRS.Booking_RS>(strResponse);
                        ViewBag.strBookingResponse = _BookingResponse;
                        strResult = _BookingResponse.TrackID;
                        ViewBag.strPaxCount = strPaxCount;
                        ViewBag.strResultCode = _BookingResponse.Status.ResultCode;

                        if (_BookingResponse.Status.ResultCode == "1" && (strResult != null && strResult != ""))
                        {
                            Status = "01";
                            ary[Result] = strResult;
                            ViewBag.strBooking = "SUCCESS";
                            ViewBag.strBookErr = "";
                            _RefList.PaymentReceiptRefNumber = strPaymentID;
                            _RefList.WebReferenceNumber = _BookingResponse.ReferenceList[0].WebReferenceNumber != null && _BookingResponse.ReferenceList[0].WebReferenceNumber != "" ? _BookingResponse.ReferenceList[0].WebReferenceNumber : "";
                            _lstRefList.Add(_RefList);
                            ViewBag.strBookingResponse = _BookingResponse;
                        }
                        else
                        {
                            ary[Error] = "FAILED";
                            ViewBag.strBooking = "FAILED";
                            _BookingResponse.TrackID = strTrackID;
                            _RefList.PaymentReceiptRefNumber = strPaymentID;
                            _RefList.WebReferenceNumber = "";
                            _lstRefList.Add(_RefList);
                            _BookingResponse.ReferenceList = _lstRefList;
                            ViewBag.strBookingResponse = _BookingResponse;
                            if (_BookingResponse.Status.Error == null || _BookingResponse.Status.Error == "")
                            {
                                ViewBag.strBookErr = "Problem occured while booking. please contact support team (#01).";
                            }
                            else
                            {
                                ViewBag.strBookErr = _BookingResponse.Status.Error + "(#01).";
                            }
                        }
                        ViewBag.PGErrorMsg = "";
                    }
                    else
                    {
                        ary[Error] = "FAILED";
                        ViewBag.strBooking = "FAILED";
                        _BookingResponse.TrackID = strTrackID;
                        _RefList.PaymentReceiptRefNumber = strPaymentID;
                        _RefList.WebReferenceNumber = "";
                        _lstRefList.Add(_RefList);
                        _BookingResponse.ReferenceList = _lstRefList;
                        ViewBag.strBookingResponse = _BookingResponse;
                        ViewBag.strBookErr = "";
                        ViewBag.PGErrorMsg = strError + "(#01).";
                        ViewBag.strPayment = strPGResult.ToString().ToUpper();
                    }
                }
                else
                {
                    ViewBag.strPayment = strPGResult.ToString().ToUpper();
                    _BookingResponse.TrackID = strTrackID;
                    _RefList.PaymentReceiptRefNumber = "";
                    _RefList.WebReferenceNumber = "";//_BookingResponse.ReferenceList[0].WebReferenceNumber != null && _BookingResponse.ReferenceList[0].WebReferenceNumber != "" ? _BookingResponse.ReferenceList[0].WebReferenceNumber:"";
                    _lstRefList.Add(_RefList);
                    _BookingResponse.ReferenceList = _lstRefList;
                    ViewBag.strBookingResponse = _BookingResponse;
                    ViewBag.PGErrorMsg = "Unable to Process your Request.Please Contact Support Team";
                }
                ViewBag.BookExp = "";
            }
            catch (Exception ex)
            {
                Status = "00";
                ary[Error] = ex.ToString();
                ViewBag.PGErrorMsg = "Problem occured while Payment Received(#05)";
                DatabaseLog.LogData(strUserName.ToString(), "X", "VisaController", "PGRedirectpage", ex.ToString(), strAgentId, strTerminalID, strSequenceId);

            }
            return View("~/Views/Visa/PGRedirectpage.cshtml");
        }
        public ActionResult ManageBooking()
        {
            #region UsageLog
            string PageName = "View PNR";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, "", "");
            }
            catch (Exception e) { }
            #endregion
            string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            if ((strAgentId == null || strAgentId == "") && (strTerminalID == null || strTerminalID == ""))
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            ViewBag.tabflag = Request.QueryString["Tab"] != null && Request.QueryString["Tab"].Trim() != "" ? Request.QueryString["Tab"].Trim() : "0";
            if (Request.QueryString["FLAG"] != null && Request.QueryString["FLAG"] != "")
            {
                ViewBag.FlagKeys = Request.QueryString["FLAG"].ToString();
            }
            ViewBag.ServerDateTime = Base.LoadServerdatetime();
            return View("~/Views/Visa/ManageBooking.cshtml");
        }

        public ActionResult FetchVisaBookedHistory(string strRiyaPNR, string strFromDate, string strToDate, string strStatus)
        {
            ArrayList arr = new ArrayList();
            arr.Add("");
            arr.Add("");
            int Result = 1;
            int Error = 0;
            string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strTerminalType = (Session["terminaltype"] != null && Session["terminaltype"].ToString() != "") ? Session["terminaltype"].ToString() : "";
            string Status = string.Empty;
            DataSet dsset = new DataSet();
            string XmlData = string.Empty;
            string PageName = "VisaController.cs";
            try
            {
                Inplantservice Inplntsvc = new Inplantservice();
                Inplntsvc.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                XmlData = "<EVENT><REQUEST>FetchVisaBookedHistory</REQUEST>" +
                    "<AGENTID>" + strAgentId + "</AGENTID>" +
                    "<TERMINALID>" + strTerminalID + "</TERMINALID>" +
                    "<USERNAME>" + strUserName + "</USERNAME>" +
                    "<IPADDRESS>" + IPAddress + "</IPADDRESS>" +
                    "<SEQUENCEID>" + strSequenceId + "</SEQUENCEID>" +
                    "<TERMINALTYPE>" + strTerminalType + "</TERMINALTYPE>" +
                    "<RIYAPNR>" + strRiyaPNR + "</RIYAPNR>" +
                    "<FROMDATE>" + strFromDate + "</FROMDATE>" +
                    "<TODATE>" + strToDate + "</TODATE>" +
                    "<PAGENAME>" + PageName + "</PAGENAME>" + "</EVENT>";
                DatabaseLog.LogData(strUserName, "E", "VisaController", "FetchVisaBookedHistory", XmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                dsset = Inplntsvc.FetchThaiVisaBookedHistory(strAgentId, strTerminalID, strUserName, IPAddress, strSequenceId, strTerminalType, strRiyaPNR, strFromDate, strToDate, PageName, strStatus);
                if (dsset != null && dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
                {
                    Session.Add("strTHBookedHistory", dsset);
                    string strResult = JsonConvert.SerializeObject(dsset);
                    arr[Result] = strResult;
                    XmlData = "<EVENTS><RESPONSE>FetchVisaBookedHistory</RESPONSE>"
                        + "<RESULTDATA>" + strResult + "</RESULTDATA></EVENTS>";
                    DatabaseLog.LogData(strUserName, "E", "VisaController", "FetchVisaBookedHistory", XmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                }
                else
                {
                    XmlData = "<EVENTS><RESPONSE>FetchVisaBookedHistory</RESPONSE>"
                        + "<RESULTDATA>" + "No Records Found" + "</RESULTDATA></EVENTS>";
                    DatabaseLog.LogData(strUserName, "E", "VisaController", "FetchVisaBookedHistory", XmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                    arr[Error] = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                Status = "00";
                arr[Error] = ex.ToString();
                ViewBag.PGErrorMsg = "Problem occured while Fetching Booked History(#05)";
                DatabaseLog.LogData(strUserName.ToString(), "X", "VisaController", "FetchVisaBookedHistory", ex.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            return Json(new { Status = Status, Results = arr });
        }

        public ActionResult FetchVisaViewPNR(string strRiyaPNR, string strRefNo)
        {
            ArrayList ary = new ArrayList();
            ary.Add("");
            ary.Add("");
            int Result = 1;
            int Error = 0;
            string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strTerminalType = (Session["terminaltype"] != null && Session["terminaltype"].ToString() != "") ? Session["terminaltype"].ToString() : "";
            string Status = string.Empty;
            DataSet dsset = new DataSet();
            string strPageName = "VisaController.cs";
            string XmlData = string.Empty;
            try
            {
                Inplantservice Inplntsvc = new Inplantservice();
                Inplntsvc.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                XmlData = "<EVENTS><REQUEST>FetchVisaViewPNR</REQUEST>"
                    + "<AGENTID>" + strAgentId + "</AGENTID>"
                    + "<TERMINALID>" + strTerminalID + "</TERMINALID>"
                    + "<USERNAME>" + strUserName + "</USERNAME>"
                    + "<IPADDRESS>" + IPAddress + "</IPADDRESS>"
                    + "<SEQUENCEID>" + strSequenceId + "</SEQUENCEID>"
                    + "<TERMINALTYPE>" + strTerminalType + "</TERMINALTYPE>"
                    + "<RIYAPNR>" + strRiyaPNR + "</RIYAPNR>"
                    + "<REFERENCENO>" + strRefNo + "</REFERENCENO>"
                    + "<PAGENAME>" + strPageName + "</PAGENAME></EVENTS>";
                DatabaseLog.LogData(strUserName, "E", "VisaController", "FetchVisaViewPNR", XmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                dsset = Inplntsvc.FetchThaiVisaViewPNR(strAgentId, strTerminalID, strUserName, IPAddress, strSequenceId, strTerminalType, strRiyaPNR, strRefNo, strPageName);
                if (dsset != null && dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
                {
                    string strResult = JsonConvert.SerializeObject(dsset);
                    ary[Result] = strResult;
                    XmlData = "<EVENTS><RESPONSE>FetchVisaViewPNR</RESPONSE>"
                       + "<RESULT>" + strResult + "</RESULT></EVENTS>";
                    DatabaseLog.LogData(strUserName, "E", "VisaController", "FetchVisaViewPNR", XmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                }
                else
                {
                    XmlData = "<EVENTS><RESPONSE>FetchVisaViewPNR</RESPONSE>"
                        + "<RESULT>No Records Found</RESULT></EVENTS>";
                    DatabaseLog.LogData(strUserName, "E", "VisaController", "FetchVisaViewPNR", XmlData.ToString(), strAgentId, strTerminalID, strSequenceId);
                    ary[Error] = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                Status = "00";
                ary[Error] = ex.ToString();
                ViewBag.PGErrorMsg = "Problem occured while Fetching View PNR(#05)";
                DatabaseLog.LogData(strUserName.ToString(), "X", "VisaController", "FetchVisaViewPNR", ex.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            return Json(new { Status = Status, Results = ary });
        }
        public ActionResult GetVisaReq()
        {
            string strError = string.Empty;
            string strResponse = string.Empty;
            string request = string.Empty;
            string strURLpath = ConfigurationManager.AppSettings["VISAAPIURL"].ToString().Trim().Replace("\n\n", "");
            try
            {
                int bookingtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                string Query = "GetVisaBooking";
                MyWebClient client = new MyWebClient();
                client.LintTimeout = bookingtimeout;
                client.Headers["Content-type"] = "applicatio";



                request = System.IO.File.ReadAllText(Server.MapPath(@"~/VISA/VisaReq.txt"));
                byte[] data = client.UploadData(strURLpath + "/" + Query, "POST", Encoding.UTF8.GetBytes(request));
                strResponse = System.Text.Encoding.ASCII.GetString(data);
            }
            catch (Exception ex)
            {

                strError = ex.ToString();
            }

            return Json(new { Status = "00", ErrMsg = strError, Results = strResponse + "---" + request + "--" + strURLpath }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVisaGetsampleReq()
        {
            string strError = string.Empty;
            string strResponse = string.Empty;
            string request = string.Empty;
            string strURLpath = ConfigurationManager.AppSettings["VISAAPIURL"].ToString().Trim().Replace("\n\n", "");
            try
            {
                RQRS.Booking_Sample_RQ bookingrq = new RQRS.Booking_Sample_RQ();
                RQRS.AgentDetails _AgentDetail = new RQRS.AgentDetails();
                _AgentDetail.AgentId = "";
                _AgentDetail.ClientID = "";
                _AgentDetail.TerminalId = "";
                _AgentDetail.UserName = "";
                _AgentDetail.AppType = "B2B";
                _AgentDetail.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                _AgentDetail.Environment = "";
                _AgentDetail.BOAID = "";
                _AgentDetail.BOAterminalID = "";
                _AgentDetail.Agenttype = "";
                _AgentDetail.CoOrdinatorID = "";
                _AgentDetail.AirportID = "";
                _AgentDetail.BranchID = "";
                _AgentDetail.IssuingBranchID = "";
                _AgentDetail.EMP_ID = "";
                _AgentDetail.COST_CENTER = "";
                _AgentDetail.Ipaddress = "";
                _AgentDetail.Sequence = "";
                _AgentDetail.Bargain_Cred = "";
                _AgentDetail.Personal_Booking = "";
                _AgentDetail.GST_FLAG = "";
                _AgentDetail.Platform = "B";

                bookingrq.AgentDetail = _AgentDetail;



                int bookingtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                string Query = "GetSampleFunction";
                MyWebClient client = new MyWebClient();
                client.LintTimeout = bookingtimeout;
                client.Headers["Content-type"] = "application/json";

                request = Newtonsoft.Json.JsonConvert.SerializeObject(bookingrq).ToString();
                byte[] data = client.UploadData(strURLpath + "/" + Query, "POST", Encoding.ASCII.GetBytes(request));
                strResponse = System.Text.Encoding.ASCII.GetString(data);
            }
            catch (Exception ex)
            {

                strError = ex.ToString();
            }

            return Json(new { Status = "00", ErrMsg = strError, Results = strResponse + "---" + request + "---" + strURLpath }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVisasampleReq()
        {
            string strError = string.Empty;
            string strResponse = string.Empty;

            string strURLpath = string.Empty;
            try
            {
                string Query = "GetVisaBooking";

                strURLpath = ConfigurationManager.AppSettings["VISAAPIURL"].ToString().Trim().Replace("\n\n", "");

                var client = new RestClient(strURLpath + "/" + Query);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddParameter("application/json; charset=utf-8", System.IO.File.ReadAllText(Server.MapPath(@"~/VISA/VisaReq.txt")), ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                IRestResponse response = client.Execute(request);
                strResponse = response.Content.ToString();

            }
            catch (Exception ex)
            {

                strError = ex.ToString();
            }

            return Json(new { Status = "00", ErrMsg = strError, Results = strResponse + "---" + strURLpath }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVisahttpReq()
        {
            string strError = string.Empty;
            string strResponse = string.Empty;
            string request = string.Empty;
            string strURLpath = ConfigurationManager.AppSettings["VISAAPIURL"].ToString().Trim().Replace("\n\n", "");
            try
            {
                int bookingtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                string Query = "GetVisaBooking";
                MyWebClient client = new MyWebClient();
                client.LintTimeout = bookingtimeout;
                client.Headers["Content-type"] = "application/json";

                // strURLpath = ConfigurationManager.AppSettings["VISAAPIURL"].ToString();
                request = System.IO.File.ReadAllText(Server.MapPath(@"~/VISA/VisaReq.txt"));


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(strURLpath + "/" + Query);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = System.IO.File.ReadAllText(Server.MapPath(@"~/VISA/VisaReq.txt"));

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }

                //byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                //strResponse = System.Text.Encoding.ASCII.GetString(data);
            }
            catch (WebException wex)
            {

                strError = wex.ToString() + wex.Status + wex.StackTrace;
            }
            catch (Exception ex)
            {

                strError = ex.ToString();
            }

            return Json(new { Status = "00", ErrMsg = strError, Results = strResponse + "---" + request + "---" + strURLpath }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFareRule()
        {
            string strError = string.Empty;
            string strResponse = string.Empty;
            string request = string.Empty;
            string strURLpath = string.Empty;
            try
            {
                RQRS_ancillary.FareRuleRQ _FareRuleRQ = new RQRS_ancillary.FareRuleRQ();
                RQRS_ancillary.AgentDetails Agent = new RQRS_ancillary.AgentDetails();
                RQRS_ancillary.RQFlights _Flts = new RQRS_ancillary.RQFlights();
                List<RQRS_ancillary.RQFlights> _lstFlts = new List<RQRS_ancillary.RQFlights>();

                Agent.AgentID = "";
                Agent.AgentType = "";
                Agent.Airportid = "";
                Agent.AppType = "B2B";
                Agent.BOAID = "";
                Agent.BOATreminalID = "";
                Agent.BranchID = Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "";
                Agent.ClientID = "";
                Agent.CoOrdinatorID = "";
                Agent.Environment = "W";
                Agent.TerminalID = "";
                Agent.UserName = "";
                Agent.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                Agent.ProjectId = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"].ToString() != "" ? Session["PRODUCT_CODE"].ToString() : "";
                Agent.APPCurrency = Session["App_currency"] != null && Session["App_currency"].ToString() != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                Agent.Platform = "B";
                Agent.ProductID = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"].ToString() != "" ? Session["PRODUCT_CODE"].ToString() : "";

                _Flts = new RQRS_ancillary.RQFlights();
                _Flts.AirlineCategory = "LCC";
                _Flts.Origin = "BOM";
                _Flts.Destination = "DEL";
                _Flts.DepartureDateTime = "";
                _Flts.ReferenceToken = "";
                _Flts.FlightNumber = "";
                _Flts.PlatingCarrier = "";
                _Flts.ArrivalDateTime = "";
                _Flts.Class = "";
                _Flts.FareBasisCode = "";
                _Flts.FareID = "";
                _Flts.ItinRef = "";
                _Flts.ReferenceToken = "";
                _Flts.SegRef = "";
                _Flts.CarrierCode = "";
                _Flts.Cabin = "";
                _Flts.SeatAvailFlag = "";
                _Flts.PromoCode = "";
                _Flts.StartTerminal = "";
                _Flts.EndTerminal = "";
                _Flts.FareType = "";

                _lstFlts.Add(_Flts);

                RQRS_ancillary.Segment Segment = new RQRS_ancillary.Segment();
                Segment.BaseOrigin = "BOM";
                Segment.BaseDestination = "DEL";

                Segment.Adult = 1;
                Segment.Child = 0;
                Segment.Infant = 0;

                Segment.SegmentType = "I";
                Segment.TripType = "O";

                _FareRuleRQ.FlightsDetails = _lstFlts;
                _FareRuleRQ.SegmentDetails = Segment;
                _FareRuleRQ.Stock = "6E";

                _FareRuleRQ.AirlinePNR = "";
                _FareRuleRQ.CRSPNR = "";

                _FareRuleRQ.FareType = "N";
                _FareRuleRQ.FetchType = "";
                _FareRuleRQ.TicketNo = "";
                _FareRuleRQ.CRSID = "";



                _FareRuleRQ.AgentDetail = Agent;

                request = Newtonsoft.Json.JsonConvert.SerializeObject(_FareRuleRQ).ToString();
                string Query = "GetFareRule";
                MyWebClient client = new MyWebClient();
                client.LintTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                client.Headers["Content-type"] = "application/json";


                strURLpath = ConfigurationManager.AppSettings["FareRuleAncillaryURL"].ToString();
                byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));

                strResponse = System.Text.Encoding.ASCII.GetString(data);
            }
            catch (Exception ex)
            {
                strError = ex.ToString();
            }

            return Json(new { Status = "00", ErrMsg = strError, Results = strResponse + "---" + request + "---" + strURLpath }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVisaPDF(string strSPNR)
        {
            ArrayList aryy = new ArrayList();
            aryy.Add("");
            aryy.Add("");
            int Result = 1;
            int Error = 0;
            string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strTerminalType = (Session["terminaltype"] != null && Session["terminaltype"].ToString() != "") ? Session["terminaltype"].ToString() : "";
            string strBranchID = Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "";
            string strCurrency = Session["App_currency"] != null && Session["App_currency"].ToString() != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
            string strUniqueID = Session["UniqueIduser"] != null ? Session["UniqueIduser"].ToString() : "";
            string strProductCode = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"].ToString() != "" ? Session["PRODUCT_CODE"].ToString() : "";
            string strEnvironment = (Session["Bookapptype"] != null && Session["Bookapptype"].ToString() != "") ? Session["Bookapptype"].ToString() : strTerminalType;
            string Status = string.Empty;
            string XmlData = string.Empty;
            string strURLpath = string.Empty;
            try
            {
                RQRS.BookedPDF_RQ bookingrq = new RQRS.BookedPDF_RQ();
                RQRS.AgentDetails _AgentDetail = new RQRS.AgentDetails();
                List<RQRS.ReferenceList> _lstRefList = new List<RQRS.ReferenceList>();
                RQRS.ReferenceList _RefList = new RQRS.ReferenceList();
                RQRS.BookedPDF_RS bookingrs = new RQRS.BookedPDF_RS();
                _AgentDetail.AgentId = strAgentId;
                _AgentDetail.ClientID = strAgentId;
                _AgentDetail.TerminalId = strTerminalID;
                _AgentDetail.UserName = strUserName;
                _AgentDetail.AppType = "B2B";
                _AgentDetail.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                _AgentDetail.Environment = strEnvironment;
                _AgentDetail.BOAID = strAgentId;
                _AgentDetail.BOAterminalID = strTerminalID;
                _AgentDetail.Agenttype = strTerminalType;
                _AgentDetail.CoOrdinatorID = "";
                _AgentDetail.AirportID = "";
                _AgentDetail.BranchID = strBranchID;
                _AgentDetail.IssuingBranchID = strBranchID;
                _AgentDetail.EMP_ID = "";
                _AgentDetail.COST_CENTER = "";
                _AgentDetail.Ipaddress = IPAddress;
                _AgentDetail.Sequence = strSequenceId;
                _AgentDetail.Bargain_Cred = "";
                _AgentDetail.Personal_Booking = "";
                _AgentDetail.GST_FLAG = "";
                _AgentDetail.Platform = "B";
                _AgentDetail.ProjectID = strProductCode;
                _AgentDetail.UID = strUniqueID;
                _AgentDetail.Group_ID = "";
                _AgentDetail.APPCurrency = strCurrency;
                _AgentDetail.TID = "";
                _AgentDetail.COMMISSION_REF = "";
                _AgentDetail.SERVICE_TAX_REF = "";
                _AgentDetail.SERVICE_TAX = "";
                _AgentDetail.TDS_TAX = "";
                _AgentDetail.MARKUP_REF = "";
                _AgentDetail.AGENT_SERVICE_FEE_REF = "";

                //Reference list:
                _RefList.PaxRef = "";
                _RefList.PaymentReceiptRefNumber = "";
                _RefList.VISA_PDF = "";
                _RefList.WebReferenceNumber = "";
                _lstRefList.Add(_RefList);
                bookingrq.AgentDetail = _AgentDetail;
                bookingrq.TrackID = "";
                bookingrq.Platform = "";
                bookingrq.Stock = "THAI";
                bookingrq.S_PNR = strSPNR;
                bookingrq.ReferenceList = _lstRefList;
                int bookingtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                MyWebClient client = new MyWebClient();
                string strResponse = string.Empty;
                client.LintTimeout = bookingtimeout;
                client.Headers["Content-type"] = "application/json";
                string Query = "GetVisaPDF";
                strURLpath = ConfigurationManager.AppSettings["VISAAPIURL"];
                string request = Newtonsoft.Json.JsonConvert.SerializeObject(bookingrq).ToString();
                string ReqTime = "";
                ReqTime = "GetVisaPDF" + DateTime.Now;
                XmlData = "<EVENT><REQUEST>GetVisaPDF Request</REQUEST>" +
                    "<REQDATA>" + request.ToString() + "</REQDATA>" +
                    "</EVENT>";
                DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "GetVisaPDF", XmlData, strAgentId, strTerminalID, strSequenceId);
                byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                strResponse = System.Text.Encoding.ASCII.GetString(data);
                RQRS.BookedPDF_RS _BookingResponse = JsonConvert.DeserializeObject<RQRS.BookedPDF_RS>(strResponse);
                XmlData = "<EVENT><RESPONSE>GetVisaPDF Response</RESPONSE>" +
                   "<RESDATA>" + strResponse.ToString() + "</RESDATA>" +
                   "</EVENT>";
                DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "GetVisaPDF", XmlData, strAgentId, strTerminalID, strSequenceId);
                _RefList = new RQRS.ReferenceList();
                string strVisaPdf = "";
                if (_BookingResponse.Status.ResultCode == "1" && _BookingResponse.TrackID != null && _BookingResponse.TrackID != "")
                {
                    strVisaPdf = _BookingResponse.ReferenceList[0].VISA_PDF;
                }
                else
                {
                    aryy[Error] = _BookingResponse.Status.Error != null && _BookingResponse.Status.Error != "" ? _BookingResponse.Status.Error : "Problem Occured While Fetching Visa PDF";
                }
            }
            catch (Exception ex)
            {
                Status = "00";
                aryy[Error] = ex.ToString();
                ViewBag.PGErrorMsg = "Problem occured while Fetching Visa PDF(#05)";
                DatabaseLog.LogData(strUserName.ToString(), "X", "VisaController", "GetVisaPDF", ex.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            return Json(new { Status = Status, Results = aryy }, JsonRequestBehavior.AllowGet);
        }

        //Export Excel for Thai Visa Booked History
        public ActionResult ExportExcelThBookH()
        {

            ArrayList encrypt = new ArrayList();
            encrypt.Add("");
            int status = 0;
            DateTime dt = DateTime.Now;
            string toDate = dt.ToString("yyyy-MM-ddTHH:mm");//yyyy-MM-ddTHH:mm:sszzz
            DataSet ds = new DataSet();
            ds = (DataSet)Session["strTHBookedHistory"];
            GridView gv = new GridView();
            gv.DataSource = ds.Tables[0];
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=BookedHistory_" + toDate + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            encrypt[status] = "SUCCESS";
            return RedirectToAction("Temp");
        }
        #endregion

        #region
        public ActionResult InsertCommonVisaOffline()
        {
            ArrayList aryy = new ArrayList();
            aryy.Add("");
            aryy.Add("");
            int Error = 0;
            int Result = 1;
            string status = string.Empty;
            string strErrorMsg = string.Empty;
            string xmlData = string.Empty;
            string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string UserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strTerminalType = (Session["terminaltype"] != null && Session["terminaltype"].ToString() != "") ? Session["terminaltype"].ToString() : "";
            string StartupPath = string.Empty;
            string imgpath = string.Empty;
            string strFileName = string.Empty;
            string statuscode = string.Empty;
            string strVisaPNR = Session["SPNR"] != null && Session["SPNR"].ToString() != "" ? Session["SPNR"].ToString() : "";
            string strVisaTrackID = string.Empty;
            bool result = false;
            try
            {
                DataSet dsset = new DataSet();
                InplantService.Inplantservice _inplantserice = new InplantService.Inplantservice();
                _inplantserice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();

                FileStream fs;
                string Date = DateTime.Now.ToString("yyyyMMddHHmmss");
                byte[] PassportFirstPage = new byte[] { };
                byte[] PassportLastPge = new byte[] { };
                byte[] psportPhoto = new byte[] { };
                //byte[] psprtMrgeCertifcte = new byte[] { };
                //byte[] psprtBirthCertifcte = new byte[] { };
                byte[] psPancard = new byte[] { };
                byte[] psHotelBookingtckt = new byte[] { };
                byte[] psAirBookingtckt = new byte[] { };
                byte[] psDepartureFlighttckt = new byte[] { };
                byte[] psInvitationLetter = new byte[] { };
                byte[] psAddressProof = new byte[] { };
                byte[] psIDCardOfHost = new byte[] { };
                byte[] psAddDocument1 = new byte[] { };
                byte[] psAddDocument2 = new byte[] { };
                string strPaxCount = System.Web.HttpContext.Current.Request["PaxCount"];
                int PaxCount = Convert.ToInt32(strPaxCount);
                string Results = string.Empty;
                string TableName = "T_T_SPNR"; string columnName = "VSA_OFFLINE_SEQNO"; string strvisa = "VSA"; string strLength = "7";
                string strRefID = string.Empty;

                DataTable dtPaxInfo = new DataTable();
                dtPaxInfo.Columns.Add("FIRSTNAME");
                dtPaxInfo.Columns.Add("MIDDLENAME");
                dtPaxInfo.Columns.Add("LASTNAME");
                dtPaxInfo.Columns.Add("MOBILENO");
                dtPaxInfo.Columns.Add("EMAILID");
                dtPaxInfo.Columns.Add("MARITALSTATUS");
                dtPaxInfo.Columns.Add("RELIGION");
                dtPaxInfo.Columns.Add("PANCARD");
                dtPaxInfo.Columns.Add("DATEOFTRAVEL");
                dtPaxInfo.Columns.Add("DATEOFBIRTH");
                dtPaxInfo.Columns.Add("GENDER");
                dtPaxInfo.Columns.Add("COUNTRYOFBIRTH");
                dtPaxInfo.Columns.Add("MOTHERNAME");
                dtPaxInfo.Columns.Add("FATHERNAME");
                dtPaxInfo.Columns.Add("PLACEOFBIRTH");
                dtPaxInfo.Columns.Add("SPOUSENAME");
                dtPaxInfo.Columns.Add("PROFESSION");
                dtPaxInfo.Columns.Add("PASSPORTNUMBER");
                dtPaxInfo.Columns.Add("PASSPORTISSUEDATE");
                dtPaxInfo.Columns.Add("PASSPORTEXPDATE");
                dtPaxInfo.Columns.Add("FLAG");
                dtPaxInfo.Columns.Add("DESTINATION");
                dtPaxInfo.Columns.Add("VISATYPE");
                dtPaxInfo.Columns.Add("REFID");
                dtPaxInfo.Columns.Add("VISATRACKID");
                dtPaxInfo.Columns.Add("PNR");
                dtPaxInfo.Columns.Add("VISAFARE");

                DataTable dtPaxDocument = new DataTable();
                dtPaxDocument.Columns.Add("PASSPORTFIRSTPAGE");
                dtPaxDocument.Columns.Add("PASSPORTLASTPGE");
                dtPaxDocument.Columns.Add("PHOTO");
                dtPaxDocument.Columns.Add("PANCARD");
                dtPaxDocument.Columns.Add("HOTELBOOKINGTCKT");
                dtPaxDocument.Columns.Add("AIRBOOKINGTCKT");
                dtPaxDocument.Columns.Add("DEPARTUREFLIGHTTCKT");
                dtPaxDocument.Columns.Add("INVITATIONLETTER");
                dtPaxDocument.Columns.Add("ADDRESSPROOF");
                dtPaxDocument.Columns.Add("IDCARDOFHOST");
                dtPaxDocument.Columns.Add("ADDDOCUMENT1");
                dtPaxDocument.Columns.Add("ADDDOCUMENT2");
                dtPaxDocument.Columns.Add("MARRIAGECERTIFICATE");
                dtPaxDocument.Columns.Add("BIRTHCERTIFICATE");

                for (var j = 1; j <= PaxCount; j++)
                {
                    xmlData = "<EVENT><REQUEST>InsertCommonVisaOffline</REQUEST>"
               + "<TABLENAME>" + TableName + "</TABLENAME>"
               + "<COLUMNNAME>" + columnName + "</COLUMNNAME>"
               + "<STRING>" + strvisa + "</STRING>"
               + "<LENGTH>" + strLength + "</LENGTH>"
               + "</EVENT>";
                    DatabaseLog.LogData(UserName, "E", "VisaController", "InsertVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                    strRefID = _inplantserice.GenerateSeqNoForVisa(strAgentId, strTerminalID, UserName, IPAddress, strTerminalType, Convert.ToDecimal(strSequenceId),
                       ref strErrorMsg, "VisaController.cs", "InsertVisaOffline", TableName, columnName, strvisa, strLength, "");
                    xmlData = "<EVENT><RESPONSE>InsertCommonVisaOffline</RESPONSE>"
                     + "<RESULT>" + strRefID + "</RESULT>"
                     + "</EVENT>";
                    DatabaseLog.LogData(UserName, "E", "VisaController", "InsertCommonVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                    string strfirstName = System.Web.HttpContext.Current.Request["strFirstName_" + j];
                    string strmiddleName = System.Web.HttpContext.Current.Request["strmiddleName_" + j];
                    string strlastName = System.Web.HttpContext.Current.Request["strlastName_" + j];
                    string strmobileNumber = System.Web.HttpContext.Current.Request["strmobileNumber_" + j];
                    string strEmailId = System.Web.HttpContext.Current.Request["strEmailId_" + j];
                    string strPassportNum = System.Web.HttpContext.Current.Request["strPassportNum_" + j];
                    string strmaritalstatus = System.Web.HttpContext.Current.Request["strmaritalstatus_" + j];
                    string strreligion = System.Web.HttpContext.Current.Request["strreligion_" + j];
                    string strpancard = System.Web.HttpContext.Current.Request["strpancard_" + j];
                    string strDateoftravel = System.Web.HttpContext.Current.Request["strDateoftravel_" + j];
                    string strDateofbirth = System.Web.HttpContext.Current.Request["strDateofbirth_" + j];
                    string strGender = System.Web.HttpContext.Current.Request["strGender_" + j];
                    string strcountryofbirth = System.Web.HttpContext.Current.Request["strcountryofbirth_" + j];
                    string strmothername = System.Web.HttpContext.Current.Request["strmothername_" + j];
                    string strfathername = System.Web.HttpContext.Current.Request["strfathername_" + j];
                    string strplaceofbirth = System.Web.HttpContext.Current.Request["strplaceofbirth_" + j];
                    string strspousename = System.Web.HttpContext.Current.Request["strspousename_" + j];
                    string strprofession = System.Web.HttpContext.Current.Request["strprofession_" + j];
                    string strRPNR = strVisaPNR;
                    string strPassportIssuedate = System.Web.HttpContext.Current.Request["strPassportIssuedate_" + j];
                    string strPassportExpdate = System.Web.HttpContext.Current.Request["strPassportExpdate_" + j];
                    string strFlag = System.Web.HttpContext.Current.Request["strFlag_" + j];

                    string strDestination = System.Web.HttpContext.Current.Request["strDestination"];
                    string strVisaType = System.Web.HttpContext.Current.Request["strVisaType"];
                    string strVisaFare = System.Web.HttpContext.Current.Request["strVisaFare"];
                    strVisaTrackID = strTerminalID + DateTime.Now.ToString("yyMMddHHmmss");

                    dtPaxInfo.Rows.Add(strfirstName, strmiddleName, strlastName, strmobileNumber, strEmailId, strmaritalstatus, strreligion, strpancard, strDateoftravel,
                        strDateofbirth, strGender, strcountryofbirth, strmothername, strfathername, strplaceofbirth, strspousename, strprofession, strPassportNum,
                       strPassportIssuedate, strPassportExpdate, strFlag, strDestination, strVisaType, strRefID, strVisaTrackID, "", strVisaFare);

                    xmlData = "<EVENT><REQUEST>InsertCommonVisaOffline</REQUEST>" +
                         "<FIRSTNAME>" + strfirstName + "</FIRSTNAME>" +
                     "<MIDDLENAME>" + strmiddleName + "</MIDDLENAME>" +
                     "<LASTNAME>" + strlastName + "</LASTNAME>" +
                     "<MOBILENUMBER>" + strmobileNumber + "</MOBILENUMBER>" +
                     "<EMAILID>" + strEmailId + "</EMAILID>" +
                     "<PASSPORTNUM>" + strPassportNum + "</PASSPORTNUM>" +
                     "<MARITALSTATUS>" + strmaritalstatus + "</MARITALSTATUS>" +
                     "<RELIGION>" + strreligion + "</RELIGION>" +
                     "<PANCARD>" + strpancard + "</PANCARD>" +
                     "<DATEOFTRAVEL>" + strDateoftravel + "</DATEOFTRAVEL>" +
                     "<DATEOFBIRTH>" + strDateofbirth + "</DATEOFBIRTH>" +
                     "<PROFESSION>" + strprofession + "</PROFESSION>" +
                     "<AGENTID>" + strAgentId + "</AGENTID>" +
                     "<TERMINALID>" + strTerminalID + "</TERMINALID>" +
                     "<IPADDRESS>" + IPAddress + "</IPADDRESS>" +
                     "<SEQUENCEID>" + strSequenceId + "</SEQUENCEID>" +
                     "<USERNAME>" + UserName + "</USERNAME>" +
                     "<TERMINALTYPE>" + strTerminalType + "</TERMINALTYPE>" +
                     "<RPNR>" + strRPNR + "</RPNR>" +
                     "<REFID>" + strRefID + "</REFID>" +
                     "<FLAG>" + strFlag + "</FLAG>" +
                     "<TRACKID>" + strVisaTrackID + "</TRACKID>" +
                     "</EVENT>";
                    DatabaseLog.LogData(UserName, "E", "VisaController", "InsertCommonVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                    HttpFileCollectionBase files = Request.Files;
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file1 = files[i];
                            string fileExt1 = System.IO.Path.GetExtension(file1.FileName);
                            string fname;
                            if (fileExt1 == ".jpg" || fileExt1 == ".png")
                            {
                                //strFileName = Request.Form[i];
                                strFileName = files.AllKeys[i];
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
                                fname = Path.Combine(Server.MapPath(@"~/PDF/AgentLogo/"), fname);
                                file1.SaveAs(fname);
                                if (strFileName == "PassportFirst_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    PassportFirstPage = new byte[fs.Length];
                                    PassportFirstPage = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "PassportLast_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    PassportLastPge = new byte[fs.Length];
                                    PassportLastPge = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "PassportPhoto_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psportPhoto = new byte[fs.Length];
                                    psportPhoto = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "Pancard_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psPancard = new byte[fs.Length];
                                    psPancard = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "HotelBookTckt_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psHotelBookingtckt = new byte[fs.Length];
                                    psHotelBookingtckt = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "AirBookingTckt_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psAirBookingtckt = new byte[fs.Length];
                                    psAirBookingtckt = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "DepFlightTckt_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psDepartureFlighttckt = new byte[fs.Length];
                                    psDepartureFlighttckt = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "InvitaionLetter_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psInvitationLetter = new byte[fs.Length];
                                    psInvitationLetter = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "AddressProof_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psAddressProof = new byte[fs.Length];
                                    psAddressProof = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "IDCardproof_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psIDCardOfHost = new byte[fs.Length];
                                    psIDCardOfHost = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "AddDocument1_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psAddDocument1 = new byte[fs.Length];
                                    psAddDocument1 = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }
                                else if (strFileName == "AddDocument2_" + j)
                                {
                                    StartupPath = fname.ToString();
                                    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                    psAddDocument2 = new byte[fs.Length];
                                    psAddDocument2 = ReadBitmap2ByteArray(StartupPath);
                                    fs.Close();
                                }

                                //else if (strFileName == "passportmrge_" + j)
                                //{
                                //    StartupPath = fname.ToString();
                                //    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                //    psprtMrgeCertifcte = new byte[fs.Length];
                                //    psprtMrgeCertifcte = ReadBitmap2ByteArray(StartupPath);
                                //    fs.Close();
                                //}
                                //else if (strFileName == "passportbirth_" + j)
                                //{
                                //    StartupPath = fname.ToString();
                                //    fs = new FileStream(StartupPath + "", FileMode.Open, FileAccess.Read);
                                //    psprtBirthCertifcte = new byte[fs.Length];
                                //    psprtBirthCertifcte = ReadBitmap2ByteArray(StartupPath);
                                //    fs.Close();
                                //}
                                System.IO.File.Delete(fname);
                                DatabaseLog.LogData(Session["username"].ToString(), "E", "VisaContoller", "InsertCommonVisaOffline", fname, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                            }
                            else
                            {
                                status = "00";
                                aryy[Error] = "Please select png/jpg files to upload";
                                xmlData = "<EVENT><RESPONSE>InsertCommonVisaOffline</RESPONSE>" +
                                         "<RESULT>Please select png image files to upload</RESULT></EVENT>";
                                DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertCommonVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                                return Json(new { Status = status, Path = "", Results = aryy });
                            }
                        }

                        dtPaxDocument.Rows.Add(Convert.ToBase64String(PassportFirstPage), Convert.ToBase64String(PassportLastPge), Convert.ToBase64String(psportPhoto),
                             Convert.ToBase64String(psPancard), Convert.ToBase64String(psHotelBookingtckt), Convert.ToBase64String(psAirBookingtckt), Convert.ToBase64String(psDepartureFlighttckt),
                                 Convert.ToBase64String(psAddressProof), Convert.ToBase64String(psIDCardOfHost), Convert.ToBase64String(psAddDocument1), Convert.ToBase64String(psAddDocument2));
                    }
                    else
                    {
                        status = "00";
                        aryy[Error] = "Please select png image files to upload";
                        xmlData = "<EVENT><RESPONSE>InsertVisaOffline</RESPONSE>" +
                    "<RESULT>Please select png image files to upload</RESULT></EVENT>";
                        DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertCommonVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                        return Json(new { Status = status, Path = "", Results = aryy });
                    }
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dtPaxInfo);
                ds.Tables.Add(dtPaxDocument);
                string jsonVisaInput = Newtonsoft.Json.JsonConvert.SerializeObject(ds);

                // Results = _inplantserice.InsertCommonVisaOffline(strAgentId, strTerminalID.ToString(), strTerminalType.ToString(), IPAddress, UserName.ToString(),
                //   strSequenceId, jsonVisaInput,"VisaController", "InsertCommonVisaOffline", ref strErrorMsg,ref result);

                if (result == true && Results != "0")
                {
                    status = "01";
                    aryy[Result] = "Your visa request submited successfully" + "\n" + "Your Reference ID:" + strVisaTrackID.ToString();
                    xmlData = "<EVENT><RESPONSE>InsertVisaOffline</RESPONSE>" +
                    "<RESULT>Your visa request submited successfully" + "\n" + "Your Reference ID:" + strVisaTrackID.ToString() + "</RESULT></EVENT>";
                    DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertCommonVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);

                    //if (ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA")
                    //{
                    //    string strAgentmailToID = Session["AgencyDetailsformail"].ToString().Split('~').Length > 5 ? Session["AgencyDetailsformail"].ToString().Split('~')[1] : "";
                    //    string strVisaToMailID = ConfigurationManager.AppSettings["VisaTOMailID"].ToString();
                    //    string strMailUserName = ConfigurationManager.AppSettings["MailUsername"].ToString();
                    //    string strMailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
                    //    string strHostAddress = ConfigurationManager.AppSettings["HostAddress"].ToString();
                    //    string strPortNo = ConfigurationManager.AppSettings["PortNo"].ToString();
                    //    bool ssl = ConfigurationManager.AppSettings["EnableSsl"].ToString() == "false" ? false : true;
                    //    string MailFrom = ConfigurationManager.AppSettings["NetworkUsername"].ToString();

                    //    string RetVal = string.Empty;
                    //    STSTRAVRAYS.Mailref.MyService pp = new STSTRAVRAYS.Mailref.MyService();
                    //    pp.Url = ConfigurationManager.AppSettings["mailurl"].ToString();
                    //    if (strAgentmailToID != null && strAgentmailToID != "")
                    //    {
                    //        StringBuilder strBuild = new StringBuilder();
                    //        strBuild.Append("<div><div style='text-align:center;font-size:20px;'><strong>INDEMNITY BOND</strong></div>");
                    //        //strBuild.Append("<ul><li>We hereby agree and undertake to Riya Travel & Tours Pvt. Ltd that we shall be responsible for any penal/overstay/or any other liabilities for absconding and/or overstay visa applicants, if the criteria, checks & procedures are not followed and/or adhered to by us meticulously, and/or as per the instructions given by Riya Travel & Tours Pvt. Ltd in writing from time to time.</li>");
                    //        //strBuild.Append("<li>We hereby further agree that we shall and will at the instance of Riya Travel & Tours Pvt. Ltd for any overstay/penal liability, remit the said amount to Riya Travel & Tours Pvt. Ltd within 72 hours of intimation about such overstay/penal liability.</li></ul></div>");
                    //        strBuild.Append("<ul><li>We hereby agree and undertake to Riya Travel & Tours Pvt. Ltd that we shall be responsible for any penal/overstay/or any other liabilities for absconding and/or overstay visa applicants, if the criteria, checks & procedures are not followed and/or adhered to by us meticulously, and/or as per the instructions given by Riya Travel & Tours Pvt. Ltd in writing from time to time.</li>");
                    //        strBuild.Append("<li>We hereby further agree that we shall and will at the instance of Riya Travel & Tours Pvt. Ltd for any overstay/penal liability, remit the said amount of upto AED 5600 to Riya Travel & Tours Pvt. Ltd within 72 hours of intimation about such overstay/penal liability.</li></ul></div>");

                    //        RetVal = pp.SendMailSingleTicket(Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(),
                    //                  "W", Session["sequenceid"].ToString(), strAgentmailToID, "", strRefID,
                    //                strBuild.ToString(), "VisaTermsAndConditions", "", strMailUserName, strMailPassword, strHostAddress, strPortNo, ssl, MailFrom, ".html");

                    //        xmlData = "<EVENT><RESPONSE>InsertCommonVisaOffline-ForAgentMail</RESPONSE>"
                    //            + "<MAILRESPONSE>" + RetVal + "</MAILRESPONSE>"
                    //            + "</EVENT>";
                    //        DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertCommonVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                    //    }
                    //    if (strVisaToMailID != null && strVisaToMailID != "")
                    //    {
                    //        string strAgencyName = Session["agencyname"] != null && Session["agencyname"].ToString() != "" ? Session["agencyname"].ToString() : "";
                    //        StringBuilder strBuild = new StringBuilder();
                    //        strBuild.Append("<div><div style='text-align:center;font-size:20px;'><strong>INDEMNITY BOND</strong></div>");
                    //        strBuild.Append("<div>Agency Name:" + strAgencyName + "</div>");
                    //        strBuild.Append("<div>Agent ID:" + strAgentId + "</div>");
                    //        strBuild.Append("<ul><li>We hereby agree and undertake to Riya Travel & Tours Pvt. Ltd that we shall be responsible for any penal/overstay/or any other liabilities for absconding and/or overstay visa applicants, if the criteria, checks & procedures are not followed and/or adhered to by us meticulously, and/or as per the instructions given by Riya Travel & Tours Pvt. Ltd in writing from time to time.</li>");
                    //        strBuild.Append("<li>We hereby further agree that we shall and will at the instance of Riya Travel & Tours Pvt. Ltd for any overstay/penal liability, remit the said amount of upto AED 5600 to Riya Travel & Tours Pvt. Ltd within 72 hours of intimation about such overstay/penal liability.</li></ul></div>");

                    //        RetVal = pp.SendMailSingleTicket(Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(),
                    //                "W", Session["sequenceid"].ToString(), strVisaToMailID, "", strRefID,
                    //              strBuild.ToString(), "VisaTermsAndConditions", "", strMailUserName, strMailPassword, strHostAddress, strPortNo, ssl, MailFrom, ".html");
                    //        xmlData = "<EVENT><RESPONSE>InsertCommonVisaOffline-ForMail</RESPONSE>"
                    //               + "<MAILRESPONSE>" + RetVal + "</MAILRESPONSE>"
                    //               + "</EVENT>";
                    //        DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertCommonVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                    //    }
                    //}
                }
                else
                {
                    status = "00";
                    aryy[Error] = "Unable to insert visa request.Please contact support team(#1)";
                    xmlData = "<EVENT><RESPONSE>InsertVisaOffline</RESPONSE>" +
                    "<RESULT>Unable to insert visa request</RESULT></EVENT>";
                    DatabaseLog.LogData(UserName.ToString(), "E", "VisaController", "InsertCommonVisaOffline", xmlData, strAgentId, strTerminalID, strSequenceId);
                }

            }
            catch (Exception ex)
            {
                status = "00";
                aryy[Error] = "Unable to insert visa details.Please contact support team(#2)";
                DatabaseLog.LogData(UserName.ToString(), "X", "VisaController", "InsertCommonVisaOffline", ex.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            return Json(new { Status = status, Path = imgpath, Results = aryy });
        }


        #endregion

        public ActionResult FetchVisaSearchDetails(string Flag)
        {
            string status = string.Empty;
            ArrayList aryy = new ArrayList();
            aryy.Add("");
            aryy.Add("");
            int Error = 0;
            int result = 1;
            string strErrorMsg = string.Empty;
            string xmlData = string.Empty;
            string strInputValue = string.Empty;
            bool response = false;
            string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string UserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strTerminalType = (Session["terminaltype"] != null && Session["terminaltype"].ToString() != "") ? Session["terminaltype"].ToString() : "";
            try
            {
                DataSet dsresult = new DataSet();
                RaysService _rays_servers = new RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                DataTable dtInput = new DataTable();
                dtInput.Columns.Add("FLAG");
                DataRow drr = dtInput.NewRow();
                drr["FLAG"] = "SELECT";
                dtInput.Rows.Add(drr);
                strInputValue = Newtonsoft.Json.JsonConvert.SerializeObject(dtInput);
                xmlData = "<EVENT><REQUEST>FetchVisaSearchDetails</REQUEST>"
                + "<INPUTVALUE>" + strInputValue + "</INPUTVALUE>"
                + "<IPADDRESS>" + IPAddress + "</IPADDRESS>"
                + "<TERMINALID>" + strTerminalID + "</TERMINALID>"
                + "<USERNAME>" + UserName + "</USERNAME>"
                + "<TERMINALTYPE>" + strTerminalType + "<TERMINALTYPE>"
                + "</EVENT>";
                DatabaseLog.LogData(UserName, "E", "VisaController", "FetchVisaSearchDetailsReq", xmlData, strAgentId, strTerminalID, strSequenceId);
                response = _rays_servers.P_INSERT_UPDATE_VISA(strInputValue, IPAddress, strTerminalID, UserName, ref dsresult, strTerminalType, ref strErrorMsg);

                if (response == true && dsresult != null && dsresult.Tables.Count > 0 && dsresult.Tables[0].Rows.Count > 0)
                {
                    aryy[result] = Newtonsoft.Json.JsonConvert.SerializeObject(dsresult);
                    status = "01";
                    aryy[Error] = "";
                }
                else
                {
                    status = "00";
                    aryy[Error] = strErrorMsg;
                }
                xmlData = string.Empty;
                xmlData = "<EVENT><RESPONSE>FetchVisaSearchDetailsRes</RESPONSE>"
                + "<RESULT>" + aryy[result] + "</RESULT>"
                + "<STATUS>" + status + "</STATUS>"
                + "<ERRORMSG>" + aryy[Error] + "</ERRORMSG>"
                + "</EVENT>";
                DatabaseLog.LogData(UserName, "E", "VisaController", "FetchVisaSearchDetailsRes", xmlData, strAgentId, strTerminalID, strSequenceId);
            }
            catch (Exception ex)
            {
                status = "00";
                aryy[Error] = "Unable to Fetch Visa Search details.Please Contact Support Team";
                DatabaseLog.LogData(UserName.ToString(), "X", "VisaController", "FetchVisaSearchDetails", ex.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            return Json(new { Status = status, Results = aryy });
        }

        //public ActionResult FetchPaxVisaDetails(string Fromdate,string Todate)
        //{
        //    string status = string.Empty;
        //    ArrayList aryy = new ArrayList();
        //    aryy.Add("");
        //    aryy.Add("");
        //    int Error = 0;
        //    int result = 1;
        //    string strErrorMsg = string.Empty;
        //    string xmlData = string.Empty;
        //    string strInputValue = string.Empty;
        //    bool response = false;
        //    string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
        //    string UserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
        //    string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
        //    string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
        //    string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
        //    string strTerminalType = (Session["terminaltype"] != null && Session["terminaltype"].ToString() != "") ? Session["terminaltype"].ToString() : "";
        //    try
        //    {
        //        string strresult = string.Empty;
        //        RaysService _rays_servers = new RaysService();
        //        _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
        //        DataTable dtInput = new DataTable();
        //        dtInput.Columns.Add("FLAG");
        //        DataRow drr = dtInput.NewRow();
        //        drr["FLAG"] = "SELECT";
        //        dtInput.Rows.Add(drr);
        //        strInputValue = Newtonsoft.Json.JsonConvert.SerializeObject(dtInput);
        //        xmlData = "<EVENT><REQUEST>FetchPaxVisaDetails</REQUEST>"
        //        + "<INPUTVALUE>" + strInputValue + "</INPUTVALUE>"
        //        + "<IPADDRESS>" + IPAddress + "</IPADDRESS>"
        //        + "<TERMINALID>" + strTerminalID + "</TERMINALID>"
        //        + "<USERNAME>" + UserName + "</USERNAME>"
        //        + "<TERMINALTYPE>" + strTerminalType + "<TERMINALTYPE>"
        //        + "</EVENT>";
        //        DatabaseLog.LogData(UserName, "E", "VisaController", "FetchPaxVisaDetailsReq", xmlData, strAgentId, strTerminalID, strSequenceId);
        //        response = _rays_servers.FetchPaxVisaDetails(Fromdate, Todate, strAgentId, IPAddress, strTerminalID, UserName, strTerminalType, strSequenceId, ref strresult, ref strErrorMsg);

        //        if (response == true && strresult != null && strresult != "" && strErrorMsg == "")
        //        {
        //            aryy[result] = strresult;
        //            status = "01";
        //            aryy[Error] = "";
        //        }
        //        else
        //        {
        //            status = "00";
        //            aryy[Error] = strErrorMsg;
        //        }
        //        xmlData = string.Empty;
        //        xmlData = "<EVENT><RESPONSE>FetchPaxVisaDetails</RESPONSE>"
        //        + "<RESULT>" + aryy[result] + "</RESULT>"
        //        + "<STATUS>" + status + "</STATUS>"
        //        + "<ERRORMSG>" + aryy[Error] + "</ERRORMSG>"
        //        + "</EVENT>";
        //        DatabaseLog.LogData(UserName, "E", "VisaController", "FetchPaxVisaDetailsRes", xmlData, strAgentId, strTerminalID, strSequenceId);
        //    }
        //    catch (Exception ex)
        //    {
        //        status = "00";
        //        aryy[Error] = ex.ToString();
        //        DatabaseLog.LogData(UserName.ToString(), "X", "VisaController", "FetchPaxVisaDetails", ex.ToString(), strAgentId, strTerminalID, strSequenceId);
        //    }
        //    return Json(new { Status = status, Results = aryy });
        //}

        #region Insert_Common_Visa
       
        public ActionResult InsertCommonVisa()
        {
            ArrayList ary = new ArrayList();
            ary.Add("");
            ary.Add("");
            int Result = 1;
            int Error = 0;
            string status = string.Empty;
            string strErrorMsg = string.Empty;
            string xmlData = string.Empty;
            string strAgentId = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            string strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";
            string IPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";
            string strSequenceId = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : "0";
            string strTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            string strTerminalType = (Session["TerminalType"] != null && Session["TerminalType"].ToString() != "") ? Session["TerminalType"].ToString() : "";
            string strBranchID = Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "";
            string strCurrency = Session["App_currency"] != null && Session["App_currency"].ToString() != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
            string strEnvironment = (Session["TerminalType"] != null && Session["TerminalType"].ToString() != "") ? Session["TerminalType"].ToString() : strTerminalType;
            string strFileName = string.Empty;
            string StartupPath = string.Empty;
            string imgpath = string.Empty;
            string statuscode = string.Empty;
            string strURLpath = string.Empty;
            string strResponse = "";
            Double strAmount = 0;
            try
            {
                if (strAgentId == "")
                {
                   return Json(new { Status = "-01", Message = "", Result = ary });
                }
                InplantService.Inplantservice _inplantserice = new InplantService.Inplantservice();
                _inplantserice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();

                RQRS.Booking_RQ bookingrq = new RQRS.Booking_RQ();
                List<RQRS.Applicants> lstapplicants = new List<RQRS.Applicants>();
                List<RQRS.DocumentList> _lstDocuments = new List<RQRS.DocumentList>();
                RQRS.DocumentList _DocList = new RQRS.DocumentList();
                RQRS.Applicants _Applicants = new RQRS.Applicants();
                RQRS.ApplicantDetails _Appdetails = new RQRS.ApplicantDetails();
                RQRS.PassportDetails _PassDetails = new RQRS.PassportDetails();
                RQRS.TravelDetails _TravelDetails = new RQRS.TravelDetails();
                RQRS.AccommodationDetails _AccomodationDetails = new RQRS.AccommodationDetails();
                RQRS.AgentDetails _AgentDetail = new RQRS.AgentDetails();
                DataSet dsset = new DataSet();
                FileStream fs;
                byte[] bytConvrtPassPrt = new byte[] { };

                string strPaxCount = System.Web.HttpContext.Current.Request["PaxCount"];
                string strVisaType = System.Web.HttpContext.Current.Request["strVisaType"];
                string strDestination = System.Web.HttpContext.Current.Request["strDestination"];
                string strVisaCode = System.Web.HttpContext.Current.Request["strVisaCode"];

                int PaxCount = Convert.ToInt32(strPaxCount);
                Session.Add("strPaxCount", Convert.ToString(PaxCount));
                string Results = string.Empty;
                string TableName = "T_T_SPNR"; string columnName = "VSA_OFFLINE_SEQNO"; string strvisa = "VSA"; string strLength = "7";
                string strRefID = string.Empty;
                string strDateoftravel = string.Empty;

                CultureInfo cii = new CultureInfo("en-GB", true);
                cii.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

                _TravelDetails = new RQRS.TravelDetails();
                _AccomodationDetails = new RQRS.AccommodationDetails();
                int Passcount = 0;
                for (var j = 1; j <= PaxCount; j++)
                {
                    _Appdetails = new RQRS.ApplicantDetails();
                    _PassDetails = new RQRS.PassportDetails();
                    _lstDocuments = new List<RQRS.DocumentList>();
                    _AgentDetail = new RQRS.AgentDetails();
                    _Applicants = new RQRS.Applicants();
                    strRefID = string.Empty;

                    xmlData = "<EVENT><REQUEST>InsertCommonVisa-RefID</REQUEST>"
                       + "<TABLENAME>" + TableName + "</TABLENAME>"
                       + "<COLUMNNAME>" + columnName + "</COLUMNNAME>"
                       + "<STRING>" + strvisa + "</STRING>"
                       + "<LENGTH>" + strLength + "</LENGTH>"
                       + "</EVENT>";
                    DatabaseLog.LogData(strUserName, "E", "VisaController", "InsertCommonVisa_RefidReq", xmlData, strAgentId, strTerminalID, strSequenceId);
                    strRefID = _inplantserice.GenerateSeqNoForVisa(strAgentId, strTerminalID, strUserName, IPAddress, strTerminalType, Convert.ToDecimal(strSequenceId),
                       ref strErrorMsg, "VisaController.cs", "InsertVisaOffline", TableName, columnName, strvisa, strLength, "");
                    xmlData = "<EVENT><RESPONSE>InsertCommonVisa-RefID</RESPONSE>"
                     + "<RESULT>" + strRefID + "</RESULT>"
                     + "</EVENT>";
                    DatabaseLog.LogData(strUserName, "E", "VisaController", "InsertCommonVisa_RefidRes", xmlData, strAgentId, strTerminalID, strSequenceId);

                    string strTitle = System.Web.HttpContext.Current.Request["strTitle_" + j];
                    string strfirstName = System.Web.HttpContext.Current.Request["strFirstName_" + j];
                    string strmiddleName = System.Web.HttpContext.Current.Request["strmiddleName_" + j];
                    string strlastName = System.Web.HttpContext.Current.Request["strlastName_" + j];
                    string strEmailID = System.Web.HttpContext.Current.Request["strEmailId_" + j];
                    string strMobileNumber = System.Web.HttpContext.Current.Request["strmobileNumber_" + j];
                    string strNationality = System.Web.HttpContext.Current.Request["strcountryofbirth_" + j];
                    string strPassportNumber = System.Web.HttpContext.Current.Request["strPassportNum_" + j];
                    string strPassIssueDate = System.Web.HttpContext.Current.Request["strPassportIssuedate_" + j];
                    string strPassExpDate = System.Web.HttpContext.Current.Request["strPassportExpdate_" + j];
                    string strPassDOB = System.Web.HttpContext.Current.Request["strDateofbirth_" + j];
                    string strGender = System.Web.HttpContext.Current.Request["strGender_" + j];

                    string strOccupation = System.Web.HttpContext.Current.Request["strprofession_" + j];
                    strAmount = System.Web.HttpContext.Current.Request["strVisaFare"] != null && System.Web.HttpContext.Current.Request["strVisaFare"] != "" ? Convert.ToDouble(System.Web.HttpContext.Current.Request["strVisaFare"]) : 0;
                    string strPassFileSize = "";
                    string strNationalityName = System.Web.HttpContext.Current.Request["strcountryofbirth_" + j];
                    string strGenderName = System.Web.HttpContext.Current.Request["strGender_" + j];
                    string strOccupationName = System.Web.HttpContext.Current.Request["strprofession_" + j];
                    //kapil
                    string strMaritalStatus = System.Web.HttpContext.Current.Request["strmaritalstatus_" + j];
                    string strReligion = System.Web.HttpContext.Current.Request["strreligion_" + j];
                    string strPanNumber = System.Web.HttpContext.Current.Request["strpancard_" + j];
                    strDateoftravel = System.Web.HttpContext.Current.Request["strDateoftravel_" + j];
                    string strMotherName = System.Web.HttpContext.Current.Request["strmothername_" + j];
                    string strFatherName = System.Web.HttpContext.Current.Request["strfathername_" + j];
                    string strPlaceofbirth = System.Web.HttpContext.Current.Request["strplaceofbirth_" + j];
                    string strSpouseName = System.Web.HttpContext.Current.Request["strspousename_" + j];
                    string strPassIssueCountry = System.Web.HttpContext.Current.Request["strPassIssueCountry_" + j];

                    #region RQRS

                    strPassExpDate = strPassExpDate != "" ? Convert.ToDateTime(strPassExpDate.Trim(), cii).ToString("yyyy-MM-dd") : "";
                    strPassIssueDate = strPassIssueDate != "" ? Convert.ToDateTime(strPassIssueDate.Trim(), cii).ToString("yyyy-MM-dd") : "";
                    strPassDOB = strPassDOB != "" ? Convert.ToDateTime(strPassDOB.Trim(), cii).ToString("yyyy-MM-dd") : "";
                    strDateoftravel = strDateoftravel != "" ? Convert.ToDateTime(strDateoftravel.Trim(), cii).ToString("yyyy-MM-dd") : "";


                    _Appdetails.PaymentReceiptRefNumber = "";
                    _Appdetails.ApplicantName = strfirstName + strlastName;
                    _Appdetails.Email = strEmailID;
                    _Appdetails.Mobile = strMobileNumber;
                    _Appdetails.IsPriority = 0;
                    _Appdetails.ISDCode = "+91";
                    _Appdetails.NationalityId = 0;
                    _Applicants.ApplicantDetails = _Appdetails;
                    _Appdetails.Nationality = strNationalityName;

                    //Passenger Details (Passport)
                    _PassDetails.Title = strTitle;
                    _PassDetails.GivenName = strfirstName;
                    _PassDetails.SurName = strlastName;
                    _PassDetails.MiddleName = strmiddleName;
                    _PassDetails.PassportNumber = strPassportNumber;
                    _PassDetails.PassportExpiryDate = strPassExpDate;
                    _PassDetails.PassportIssueDate = strPassIssueDate;
                    _PassDetails.DateOfBirth = strPassDOB;
                    _PassDetails.ResidencyId = 0;
                    _PassDetails.GenderId = 0;
                    _PassDetails.SalutationId = 0;
                    _PassDetails.PassportTypeId = 0;
                    _PassDetails.PermanentAddress = "";
                    _PassDetails.State = "";
                    _PassDetails.YearlyIncome = 0;
                    _PassDetails.PlaceOfIssue = strPassIssueCountry;
                    _PassDetails.OccupationId = 0;
                    _PassDetails.Gender = strGenderName;
                    _PassDetails.Salutation = "";
                    _PassDetails.PassportType = "";
                    _PassDetails.Occupation = strOccupationName;
                    _PassDetails.Maritalstatus = strMaritalStatus;
                    _PassDetails.Religion = strReligion;
                    _PassDetails.PanNumber = strPanNumber;
                    _PassDetails.Date_of_travel = strDateoftravel;
                    _PassDetails.MotherName = strMotherName;
                    _PassDetails.FatherName = strFatherName;
                    _PassDetails.Place_of_birth = strPlaceofbirth;
                    _PassDetails.SpouseName = strSpouseName;
                    _PassDetails.RefID = strRefID;
                    _PassDetails.VisaType = strVisaType;
                    _PassDetails.Destination = strDestination;
                    _PassDetails.VOD_VISA_TYPEID = strVisaCode;

                    _Applicants.PassportDetails = _PassDetails;
                    #endregion

                   
                    HttpFileCollectionBase files = Request.Files;
                    if (files.Count > 0)
                    {
                        var count = 0;
                        count = files.Count;
                        for (int i = 0; i < count; i++)
                        {
                            _DocList = new RQRS.DocumentList();
                            if (count <= Passcount) { Passcount--; }
                            HttpPostedFileBase file1 = files[Passcount];//i
                            string fileExt1 = System.IO.Path.GetExtension(file1.FileName);
                            string fname;
                            string filename = "";
                            if (fileExt1 == ".jpg" || fileExt1 == ".pdf")
                            {

                                strFileName = files.AllKeys[Passcount];
                                string fileExt = System.IO.Path.GetExtension(file1.FileName);
                                var fileName = strAgentId + fileExt;
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = file1.FileName.Split(new char[] { '\\' });
                                    filename = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    filename = file1.FileName;
                                }

                                fname = Path.Combine(Server.MapPath(@"~/Content/FILES/VISA/DOCUMENTS/"), filename);
                                file1.SaveAs(fname);
                                bytConvrtPassPrt = new byte[] { };

                                if (strFileName == "imguploaddoc_" + j + "_" + i)//Convert.ToInt32(i)+ Convert.ToInt32(passlastCount) //(i- passlastCount)
                                {
                                    if (fileExt1 == ".jpg" || fileExt1 == ".jpeg" || fileExt1 == ".pdf")
                                    {
                                        StartupPath = fname.ToString();
                                        fs = new FileStream(StartupPath, FileMode.Open, FileAccess.Read);
                                        bytConvrtPassPrt = new byte[fs.Length];
                                        bytConvrtPassPrt = ReadBitmap2ByteArray(StartupPath);
                                        fs.Close();
                                        strPassFileSize = System.Web.HttpContext.Current.Request["imguploaddocsize_" + j + "_" + i];
                                        if (files.Count > i) { Passcount++; }


                                        _DocList.DocumentListId = i;
                                        _DocList.FileContent = Convert.ToBase64String(bytConvrtPassPrt);
                                        _DocList.FileName = filename;
                                        _DocList.MimeType = fileExt1;
                                        _DocList.FileSize = strPassFileSize;
                                        _DocList.RefID = strRefID;

                                        _lstDocuments.Add(_DocList);
                                        System.IO.File.Delete(StartupPath);
                                    }
                                    else
                                    {
                                        ary[Error] = "Please Select .jpg/jpeg/pdf files to upload";
                                    }
                                }

                                
                                DatabaseLog.LogData(strUserName, "E", "VisaContoller", "InsertCommonVisa", fname, strAgentId, strTerminalID, strSequenceId);
                            }
                            else
                            {
                                status = "00";
                                ary[Error] = "Please select jpg/jpeg/pdf files to upload";
                                xmlData = "<EVENT><RESPONSE>InsertCommonVisa</RESPONSE>" +
                                         "<RESULT>Please select jpg/jpeg/pdf files to upload</RESULT></EVENT>";
                                DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertCommonVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                                return Json(new { Status = status, Message = "", Result = ary });
                            }
                        }
                        _Applicants.DocumentList = _lstDocuments;
                    }
                    lstapplicants.Add(_Applicants);
                }


                //_TravelDetails 
                _TravelDetails.ArrivalAirportId = "";
                _TravelDetails.ArrivalDate = "";
                _TravelDetails.ArrivalTime = "";
                _TravelDetails.ArrivalFlightNumber = "";
                _TravelDetails.ArrivalAirlinesId = 0;
                _TravelDetails.ArrivalPNRNumber = "";
                _TravelDetails.DepartureDate = strDateoftravel;
                _TravelDetails.DepartureTime = "";
                _TravelDetails.DepartureFlightNumber = "";
                _TravelDetails.DepartureAirlinesId = "0";
                _TravelDetails.DeparturePNRNumber = "";
                _TravelDetails.PurposeOfVisit = 0;
                _TravelDetails.BoardedCountry = "";
                _TravelDetails.NextCity = "";
                _TravelDetails.FirstTimeTraveller = "";
                _TravelDetails.TypeOfFlight = "";
                _TravelDetails.LengthOfStay = "";
                _TravelDetails.IsMinorTravelingOnSamePassport = 0;
                _TravelDetails.MinorFamilyName = "";
                _TravelDetails.MinorGivenName = "";
                _TravelDetails.MinorGender = 0;
                _TravelDetails.MinorDateOfBirth = "";
                _TravelDetails.MinorPlaceOfBirth = "";

                //_AccomodationDetails
                _AccomodationDetails.PlaceOfStayId = 0;
                _AccomodationDetails.AccommodationOrOwnerName = "";
                _AccomodationDetails.CityId = 0;
                _AccomodationDetails.DistrictId = 0;
                _AccomodationDetails.SubDistrictId = 0;
                _AccomodationDetails.PinCode = "";
                _AccomodationDetails.StreetAddress = "";
                _AccomodationDetails.ReferenceDetails = "";
                _AccomodationDetails.PlaceOfStay = "";
                _AccomodationDetails.City = "";
                _AccomodationDetails.District = "";
                _AccomodationDetails.SubDistrict = "";

                //_AgentDetail
                _AgentDetail.AgentId = strAgentId;
                _AgentDetail.ClientID = strAgentId;
                _AgentDetail.TerminalId = strTerminalID;
                _AgentDetail.UserName = strUserName;
                _AgentDetail.AppType = "B2B";
                _AgentDetail.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                _AgentDetail.Environment = strEnvironment;
                _AgentDetail.BOAID = strAgentId;
                _AgentDetail.BOAterminalID = strTerminalID;
                _AgentDetail.Agenttype = strTerminalType;
                _AgentDetail.CoOrdinatorID = "";
                _AgentDetail.AirportID = "";
                _AgentDetail.BranchID = strBranchID;
                _AgentDetail.IssuingBranchID = strBranchID;
                _AgentDetail.EMP_ID = "";
                _AgentDetail.COST_CENTER = "";
                _AgentDetail.Ipaddress = IPAddress;
                _AgentDetail.Sequence = strSequenceId;
                _AgentDetail.Bargain_Cred = "";
                _AgentDetail.Personal_Booking = "";
                _AgentDetail.GST_FLAG = "";
                _AgentDetail.Platform = "B";
                _AgentDetail.ProjectID = "";
                _AgentDetail.UID = "";
                _AgentDetail.Group_ID = "";
                _AgentDetail.APPCurrency = strCurrency;

                bookingrq.AgentDetail = _AgentDetail;
                bookingrq.BatchReference = "";
                bookingrq.PaymentBatchReference = strTerminalID;
                bookingrq.NoOfApplicants = PaxCount.ToString();
                bookingrq.Amount = strAmount.ToString();
                bookingrq.TravelDetails = _TravelDetails;
                bookingrq.AccommodationDetails = _AccomodationDetails;
                bookingrq.Applicants = lstapplicants;
                bookingrq.PaymentMode = "T";
                bookingrq.TrackID = "";
                bookingrq.Platform = "";
                bookingrq.Stock = "OFFLINE";
                bookingrq.blCreateTrack = true;

                int Amount = Convert.ToInt32(strAmount);
                #region APIRequest & Response for Generate TrackID
                int bookingtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                string Query = "GetVisaBooking";
               //MyWebClient client = new MyWebClient();
                //client.LintTimeout = bookingtimeout;
                //client.Headers["Content-type"] = "application/json";

                //strURLpath = ConfigurationManager.AppSettings["VISAAPIURL"];
                string request = Newtonsoft.Json.JsonConvert.SerializeObject(bookingrq).ToString();
                strURLpath = ConfigurationManager.AppSettings["VISAAPIURL"].ToString().Trim();
                xmlData = "<EVENT><REQUEST>InsertCommonVisa-GenerateTrackID Request</REQUEST><URL>" + strURLpath + "</URL>" +
                    "<REQDATA>" + request.ToString() + "</REQDATA>" +
                    "</EVENT>";
                DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertCommonVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                //byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                //strResponse = System.Text.Encoding.ASCII.GetString(data);                               


                var client = new RestClient(strURLpath + "/" + Query);
                client.Timeout = -1;
                var requestparameter = new RestRequest(Method.POST);
                requestparameter.AddParameter("application/json; charset=utf-8", request, ParameterType.RequestBody);
                requestparameter.RequestFormat = DataFormat.Json;

                IRestResponse response = client.Execute(requestparameter);

                xmlData = "<EVENT><RESPONSE>InsertCommonVisa-GenerateTrackID Response</RESPONSE>" +
                    "<RESDATA>" + response.Content.ToString() + "</RESDATA>" +
                    "<STATUSDESCRIPTION>" + response.StatusDescription.ToString() + "</STATUSDESCRIPTION>" +
                    "</EVENT>";
                DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertCommonVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                #endregion
                if (response.StatusDescription.ToString().ToUpper().Trim() == "OK")
                {
                    strResponse = response.Content.ToString();

                    RQRS.Booking_RS _BookingResponse = JsonConvert.DeserializeObject<RQRS.Booking_RS>(strResponse);

                    if (_BookingResponse.Status.ResultCode == "1" && (_BookingResponse.TrackID != null && _BookingResponse.TrackID != ""))
                    {
                        status = "01";
                        ary[Result] = "Your visa request submited successfully." + "\n" + "Your Reference ID:" + _BookingResponse.TrackID.ToString();
                        xmlData = "<EVENT><RESPONSE>InsertCommonVisa</RESPONSE>" +
                        "<RESULT>Your visa request submited successfully" + "\n" + "Your Reference ID:" + _BookingResponse.TrackID.ToString() + "</RESULT></EVENT>";
                        DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertCommonVisa", xmlData, strAgentId, strTerminalID, strSequenceId);

                        if (ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA")
                        {
                            string strAgentmailToID = Session["AgencyDetailsformail"].ToString().Split('~').Length > 5 ? Session["AgencyDetailsformail"].ToString().Split('~')[1] : "";
                            string strVisaToMailID = ConfigurationManager.AppSettings["VisaTOMailID"].ToString();
                            string strMailUserName = ConfigurationManager.AppSettings["MailUsername"].ToString();
                            string strMailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
                            string strHostAddress = ConfigurationManager.AppSettings["HostAddress"].ToString();
                            string strPortNo = ConfigurationManager.AppSettings["PortNo"].ToString();
                            bool ssl = ConfigurationManager.AppSettings["EnableSsl"].ToString() == "false" ? false : true;
                            string MailFrom = ConfigurationManager.AppSettings["NetworkUsername"].ToString();

                            string RetVal = string.Empty;
                            STSTRAVRAYS.Mailref.MyService pp = new STSTRAVRAYS.Mailref.MyService();
                            pp.Url = ConfigurationManager.AppSettings["Mailurl"].ToString();
                            if (strAgentmailToID != null && strAgentmailToID != "")
                            {
                                StringBuilder strBuild = new StringBuilder();
                                strBuild.Append("<div><div style='text-align:center;font-size:20px;'><strong>INDEMNITY BOND</strong></div>");
                                //strBuild.Append("<ul><li>We hereby agree and undertake to Riya Travel & Tours Pvt. Ltd that we shall be responsible for any penal/overstay/or any other liabilities for absconding and/or overstay visa applicants, if the criteria, checks & procedures are not followed and/or adhered to by us meticulously, and/or as per the instructions given by Riya Travel & Tours Pvt. Ltd in writing from time to time.</li>");
                                //strBuild.Append("<li>We hereby further agree that we shall and will at the instance of Riya Travel & Tours Pvt. Ltd for any overstay/penal liability, remit the said amount to Riya Travel & Tours Pvt. Ltd within 72 hours of intimation about such overstay/penal liability.</li></ul></div>");
                                strBuild.Append("<ul><li>We hereby agree and undertake to Riya Travel & Tours Pvt. Ltd that we shall be responsible for any penal/overstay/or any other liabilities for absconding and/or overstay visa applicants, if the criteria, checks & procedures are not followed and/or adhered to by us meticulously, and/or as per the instructions given by Riya Travel & Tours Pvt. Ltd in writing from time to time.</li>");
                                strBuild.Append("<li>We hereby further agree that we shall and will at the instance of Riya Travel & Tours Pvt. Ltd for any overstay/penal liability, remit the said amount of upto AED 5600 to Riya Travel & Tours Pvt. Ltd within 72 hours of intimation about such overstay/penal liability.</li></ul></div>");

                                RetVal = pp.SendMailSingleTicket(Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(),
                                          "W", Session["sequenceid"].ToString(), strAgentmailToID, "", strRefID,
                                        strBuild.ToString(), "VisaTermsAndConditions", "", strMailUserName, strMailPassword, strHostAddress, strPortNo, ssl, MailFrom, ".html");

                                xmlData = "<EVENT><RESPONSE>InsertCommonVisa-ForAgentMail</RESPONSE>"
                                    + "<MAILRESPONSE>" + RetVal + "</MAILRESPONSE>"
                                    + "</EVENT>";
                                DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertCommonVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                            }
                            if (strVisaToMailID != null && strVisaToMailID != "")
                            {
                                string strAgencyName = Session["agencyname"] != null && Session["agencyname"].ToString() != "" ? Session["agencyname"].ToString() : "";
                                StringBuilder strBuild = new StringBuilder();
                                strBuild.Append("<div><div style='text-align:center;font-size:20px;'><strong>INDEMNITY BOND</strong></div>");
                                strBuild.Append("<div>Agency Name:" + strAgencyName + "</div>");
                                strBuild.Append("<div>Agent ID:" + strAgentId + "</div>");
                                strBuild.Append("<ul><li>We hereby agree and undertake to Riya Travel & Tours Pvt. Ltd that we shall be responsible for any penal/overstay/or any other liabilities for absconding and/or overstay visa applicants, if the criteria, checks & procedures are not followed and/or adhered to by us meticulously, and/or as per the instructions given by Riya Travel & Tours Pvt. Ltd in writing from time to time.</li>");
                                strBuild.Append("<li>We hereby further agree that we shall and will at the instance of Riya Travel & Tours Pvt. Ltd for any overstay/penal liability, remit the said amount of upto AED 5600 to Riya Travel & Tours Pvt. Ltd within 72 hours of intimation about such overstay/penal liability.</li></ul></div>");

                                RetVal = pp.SendMailSingleTicket(Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(),
                                        "W", Session["sequenceid"].ToString(), strVisaToMailID, "", strRefID,
                                      strBuild.ToString(), "VisaTermsAndConditions", "", strMailUserName, strMailPassword, strHostAddress, strPortNo, ssl, MailFrom, ".html");
                                xmlData = "<EVENT><RESPONSE>InsertCommonVisa-ForMail</RESPONSE>"
                                       + "<MAILRESPONSE>" + RetVal + "</MAILRESPONSE>"
                                       + "</EVENT>";
                                DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertCommonVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                            }
                        }
                        return Json(new { Status = status, Message = "", Result = ary });
                    }
                    else
                    {
                        ary[Error] = "Problem Occured While Booking.Please Contact Support Team(#1).";
                        xmlData = "<EVENT><RESPONSE>InsertCommonVisa-Request</RESPONSE>" + "<RESDATE>FAILED</RESDATE>" + "</EVENT>";
                        DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertCommonVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                    }
                }
                else
                {
                    ary[Error] = response.StatusDescription.ToString();
                    xmlData = "<EVENT><RESPONSE>InsertCommonVisa-Request</RESPONSE>" + "<RESDATE>FAILED</RESDATE>" + "</EVENT>";
                    DatabaseLog.LogData(strUserName.ToString(), "E", "VisaController", "InsertCommonVisa", xmlData, strAgentId, strTerminalID, strSequenceId);
                }

            }
            catch (Exception ex)
            {
                status = "00";
                // ary[Error] = ex.ToString();
                ary[Error] = "Problem Occured While Booking.Please Contact Support Team(#2).";
                DatabaseLog.LogData(strUserName.ToString(), "X", "VisaController", "InsertCommonVisa", ex.ToString(), strAgentId, strTerminalID, strSequenceId);
            }
            return Json(new { Status = status, Message = "", Result = ary });
        }
        #endregion

    }
}

