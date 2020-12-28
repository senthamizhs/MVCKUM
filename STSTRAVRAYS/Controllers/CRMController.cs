using Newtonsoft.Json;
using STSTRAVRAYS.Models;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.Reflection;
using STSTRAVRAYS.Rays_service;
namespace STSTRAVRAYS.Controllers
{
    public class CRMController : Controller
    {
        RaysService _RaysService = new RaysService();
        string strServiceURL = ConfigurationManager.AppSettings["ServiceURI"].ToString();

        public ActionResult CRMHomepage()//--?
        {
            Object _obj = new Object();
            _obj = Session["UserName"];
            if (_obj==null)
            {
                return RedirectToAction("SessionExp", "Redirect");
            }
            ViewBag.ServerDateTime = Base.LoadServerdatetime();
            return View();
        }

        public ActionResult UpdateRemindersStu(string strBranchid, string strClientid, string strCallid) //--?
        {
            string stu = string.Empty, msg = string.Empty, result = string.Empty;
            string strGetMobileNo = string.Empty,
                 strUserName = string.Empty,
                 strIpAddress = string.Empty,
                 strSequenceId = string.Empty,
                 strTerminalType = string.Empty,
                 strTerminalId = string.Empty,
                 strPosId = string.Empty,
                 PageFlag = string.Empty;

            try
            {

                Object _obj = new Object();
                _obj = Session["UserName"];
                if (_obj == null)
                {
                    return Json(new { Status = "-1", Message = "", Results = "" });
                }


                strUserName = Session["UserName"] != null ? Session["UserName"].ToString() : "";
                strIpAddress = ControllerContext.HttpContext.Request.UserHostAddress;
                strSequenceId = "0";
                strTerminalType = "W";
                strTerminalId = Session["TerminalID"] != null ? Session["TerminalID"].ToString() : "";
                strPosId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                PageFlag = "U";

                _RaysService.Url = strServiceURL;

                DatabaseLog.LogData(strUserName, "E", "CRM", "Fetchenquirydetails_REQ", "<REQUEST><CALLID>" + strCallid + "</CALLID></REQUEST>", strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");


                Byte[] Response = _RaysService.Reminders(strBranchid, strUserName, strIpAddress, Convert.ToDecimal(strSequenceId), strPosId, strTerminalType, strTerminalId, strClientid, PageFlag, strCallid,""); 


                DataSet _ResponseData = new DataSet();
                if (Response.Length > 0)
                {


                    _ResponseData = Base.Decompress(Response);
                }

                DatabaseLog.LogData(strUserName, "E", "CRM", "Fetchenquirydetails_RES", JsonConvert.SerializeObject(_ResponseData).ToString(), strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");
                if (_ResponseData != null && _ResponseData.Tables.Count > 0 && _ResponseData.Tables[0].Rows.Count > 0)
                {
                    stu = "01";
                    msg = "";
                    result = JsonConvert.SerializeObject(_ResponseData).ToString();
                }
                else
                {
                    stu = "02";
                    msg = "Problem occured while update reminder OFF. Please contact support team (#03).";
                    result = "";
                }

            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Problem occured while update reminder OFF. Please contact support team (#05).";
                result = "";
                DatabaseLog.LogData(strUserName, "X", "CRM", "Fetchenquirydetails_RES", ex.ToString(), strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");
            }

            return Json(new { Status = stu, Message = msg, Results = result });
        }

        public ActionResult EnquiryDetails(string ClientID, string EnquiryID, string EnquiryStatus)//--?
        {

            string stu = string.Empty,
                msg = string.Empty,
                result = string.Empty,
                strGetMobileNo = string.Empty,
                strGetEmailId = string.Empty,
                CusName = string.Empty,
                strUserName = string.Empty,
                strIpAddress = string.Empty,
                strSequenceId = string.Empty,
                strTerminalType = string.Empty,
                strTerminalId = string.Empty,
                strPosId = string.Empty,
                PageFlag = string.Empty,
                pstrRef = string.Empty,
                status = string.Empty;


            try
            {

                Object _obj = new Object();
                _obj = Session["UserName"];
                if (_obj == null)
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }


                strPosId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strTerminalId = Session["TerminalID"] != null ? Session["TerminalID"].ToString() : "";
                _RaysService.Url = strServiceURL;

                string xml = "<REQUEST><CLIENTID>" + ClientID + "</CLIENTID><ENQUIRYID>" + EnquiryID + "</ENQUIRYID><ENQUIRYSTATUS>" + EnquiryStatus + "</ENQUIRYSTATUS></REQUEST>";
                DatabaseLog.LogData(Session["UserName"] != null ? Session["UserName"].ToString() : "", "E", "CRM", "EnquiryDetails_REQ", xml, strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");

                Byte[] Response = _RaysService.GET_CRM_CALL_DETAILS(strTerminalId, ClientID, strPosId, EnquiryID, EnquiryStatus);

                DataSet _ResponseData = Base.Decompress(Response);

                xml = "<RESPONSE><CLIENTID>" + ClientID + "<CLIENTID><JSONRESPONSE>" + JsonConvert.SerializeObject(_ResponseData).ToString() + "</JSONRESPONSE><ENQUIRYID>" + EnquiryID + "</ENQUIRYID><ENQUIRYSTATUS>" + EnquiryStatus + "</ENQUIRYSTATUS></RESPONSE>";
                DatabaseLog.LogData(Session["UserName"] != null ? Session["UserName"].ToString() : "", "E", "CRM", "EnquiryDetails_RES", xml, strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");

                if (_ResponseData != null && _ResponseData.Tables.Count > 0)
                {
                    stu = "01";
                    result = JsonConvert.SerializeObject(_ResponseData).ToString();

                }
                else
                {
                    stu = "02";
                    msg = "Problem occured while fetch records. Please contact support team (#02).";
                }

            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Problem occured while fetch records. Please contact support team (#05).";
                DatabaseLog.LogData(Session["UserName"] != null ? Session["UserName"].ToString() : "", "X", "CRM", "EnquiryDetails_RES", ex.ToString(), strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");
            }
            return Json(new { Status = stu, Message = msg, Result = result });
        }

        public ActionResult Fetchenquirydetails(string strclientID, string strBranchId)//--?
        {

            string stu = string.Empty,
                msg = string.Empty,
                result = string.Empty,
                strGetMobileNo = string.Empty,
                strGetEmailId = string.Empty,
                CusName = string.Empty,
                strUserName = string.Empty,
                strIpAddress = string.Empty,
                strSequenceId = string.Empty,
                strTerminalType = string.Empty,
                strTerminalId = string.Empty,
                strPosId = string.Empty,
                PageFlag = string.Empty,
                pstrRef = string.Empty,
                status = string.Empty;


            try
            {
                Object _obj = new Object();
                _obj = Session["UserName"];
                if (_obj == null)
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }


                strGetMobileNo = Session["PhoneNo"] != null ? Session["PhoneNo"].ToString() : "";
                strGetEmailId = Session["Loginusermailid"] != null ? Session["Loginusermailid"].ToString() : "";
                CusName = Session["UserName"] != null ? Session["UserName"].ToString() : "";
                strUserName = Session["UserName"] != null ? Session["UserName"].ToString() : "";
                strIpAddress = ControllerContext.HttpContext.Request.UserHostAddress;
                strSequenceId = "0";
                strTerminalType = "W";
                strTerminalId = Session["TerminalID"] != null ? Session["TerminalID"].ToString() : "";
                strPosId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                PageFlag = "C";
                status = "1";
                _RaysService.Url = strServiceURL;


                strGetMobileNo = "";
                strGetEmailId = "";

                string xml = "<REQUEST><CLIENTID>" + strclientID + "</CLIENTID><STRBRANCHID>" + strBranchId + "</STRBRANCHID></REQUEST>";
                DatabaseLog.LogData(CusName, "E", "CRM", "Fetchenquirydetails_REQ", xml, strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");


                Byte[] Response = _RaysService.GetPassenger(strclientID, strGetMobileNo, strGetEmailId, strclientID, strBranchId, "", strIpAddress, Convert.ToDecimal(strSequenceId), strTerminalType, strTerminalId,
                                             strPosId, PageFlag, ref pstrRef, status);

               DataSet _ResponseData = Base.Decompress(Response);

               xml = "<RESPONSE><CLIENTID>" + strclientID + "<CLIENTID><JSONRESPONSE>" + JsonConvert.SerializeObject(_ResponseData).ToString() + "</JSONRESPONSE><STRBRANCHID>" + strBranchId + "</STRBRANCHID></RESPONSE>";
               DatabaseLog.LogData(CusName, "E", "CRM", "Fetchenquirydetails_RES", xml, strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");


                if (_ResponseData != null && _ResponseData.Tables.Count > 0)
                {
                    stu = "01";
                    result = JsonConvert.SerializeObject(_ResponseData).ToString();
                }
                else
               {
                   stu = "02";
                    msg = "No records fount .";
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Problem occured while fetch records. Please contact support team (#05).";
                DatabaseLog.LogData(CusName, "X", "CRM", "Fetchenquirydetails_RES", ex.ToString(), strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");
            }
            return Json(new { Status = stu, Message = msg, Result = result });
        }

        public JsonResult InsertEnquiry()
        {
            DataSet DsClientDetails = new DataSet();
            DataSet dsInsertRecord = new DataSet();
            DataTable dtInsertRecord = new DataTable();

            string strUserName = string.Empty,
            strIpAddress = string.Empty,
            strSequenceId = string.Empty,
            strPosId = string.Empty,
            strTerminalId = string.Empty,
            strTerminalType = string.Empty, strBranchId = string.Empty;
            string Response = string.Empty;
            string stu = string.Empty, msg = string.Empty, result = string.Empty;
            string companyid = string.Empty;
            string userid = string.Empty;
            try
            {
                string sessionval = Session["insertxml"] != null ? Session["insertxml"].ToString() : "";
                if (sessionval == "")
                {
                    return Json(new { Status = "00", Message = "Session Expired", Result = "" });
                }
                CRMDATA _CRMDATA = JsonConvert.DeserializeObject<CRMDATA>(sessionval);
                strBranchId = _CRMDATA.BranchId;
                strSequenceId = "0";
                strTerminalType = "W";
                strUserName = System.Web.HttpContext.Current.Session["UserName"] != null ? System.Web.HttpContext.Current.Session["UserName"].ToString() : "";
                strIpAddress = System.Web.HttpContext.Current.Session["IPAddress"] != null ? System.Web.HttpContext.Current.Session["IPAddress"].ToString() : "";
                strPosId = System.Web.HttpContext.Current.Session["POS_ID"] != null ? System.Web.HttpContext.Current.Session["POS_ID"].ToString() : "";
                strTerminalId = System.Web.HttpContext.Current.Session["TerminalID"] != null ? System.Web.HttpContext.Current.Session["TerminalID"].ToString() : "";
                companyid = System.Web.HttpContext.Current.Session["CompanyID"] != null ? System.Web.HttpContext.Current.Session["CompanyID"].ToString() : "";
                userid = System.Web.HttpContext.Current.Session["UserID"] != null ? System.Web.HttpContext.Current.Session["UserID"].ToString() : "";
                dtInsertRecord.TableName = "CRMDATA";
                dtInsertRecord.Columns.Add("ENQUIRY_ID");

                dtInsertRecord.Columns.Add("PAX_MOBILE_NO");
                dtInsertRecord.Columns.Add("PAX_EMAIL_ID");
                dtInsertRecord.Columns.Add("CLIENT_TYPE");
                dtInsertRecord.Columns.Add("CLIENT_CODE");
                dtInsertRecord.Columns.Add("COORDINATOR_ID");
                dtInsertRecord.Columns.Add("PRODUCT");
                dtInsertRecord.Columns.Add("AIRPORT_TYPE");
                dtInsertRecord.Columns.Add("CALL_TYPE");
                dtInsertRecord.Columns.Add("CUST_TYPE");
                dtInsertRecord.Columns.Add("LANGUAGE");
                dtInsertRecord.Columns.Add("CUST_RATING");
                dtInsertRecord.Columns.Add("FORWARD_DEPT");
                dtInsertRecord.Columns.Add("CALL_FROMDATE");
                dtInsertRecord.Columns.Add("CALL_TODATE");
                dtInsertRecord.Columns.Add("CALL_FROM");
                dtInsertRecord.Columns.Add("CALL_TO");
                dtInsertRecord.Columns.Add("REMAINDER_DATE");
                dtInsertRecord.Columns.Add("CALL_CATEGORY");
                dtInsertRecord.Columns.Add("REMARKS");
                dtInsertRecord.Columns.Add("XML");
                dtInsertRecord.Columns.Add("MAIL_XML");
                dtInsertRecord.Columns.Add("POS_ID");
                dtInsertRecord.Columns.Add("TERMINAL_ID");
                dtInsertRecord.Columns.Add("ORIGIN");
                dtInsertRecord.Columns.Add("DESTINATION");
                dtInsertRecord.Columns.Add("DEPARTURE_DATE");
                dtInsertRecord.Columns.Add("ARRIVAL_DATE");
                dtInsertRecord.Columns.Add("ENQUIRY_DETAILS");
                dtInsertRecord.Columns.Add("ATTENDED_BY");
                dtInsertRecord.Columns.Add("TRIP_TYPE");
                dtInsertRecord.Columns.Add("TRIP_STYLE");
                dtInsertRecord.Columns.Add("ADULT_COUNT");
                dtInsertRecord.Columns.Add("CHILD_COUNT");
                dtInsertRecord.Columns.Add("INFANT_COUNT");
                dtInsertRecord.Columns.Add("TRIP_CLASS");
                dtInsertRecord.Columns.Add("SEQ_NO");
                dtInsertRecord.Columns.Add("SAVE_DETAILS");

                DataRow drrow = dtInsertRecord.NewRow();
                drrow["ENQUIRY_ID"] = _CRMDATA.ENQUIRY_ID;

                drrow["PAX_MOBILE_NO"] = _CRMDATA.PAX_MOBILE_NO;
                drrow["PAX_EMAIL_ID"] = _CRMDATA.PAX_EMAIL_ID;
                drrow["CLIENT_TYPE"] = "";
                drrow["CUST_TYPE"] = "0";
                drrow["FORWARD_DEPT"] = "0";
                drrow["CLIENT_CODE"] = _CRMDATA.CLIENT_CODE;
                drrow["COORDINATOR_ID"] = _CRMDATA.COORDINATOR_ID;
                drrow["PRODUCT"] = _CRMDATA.PRODUCT;
                drrow["AIRPORT_TYPE"] = _CRMDATA.AIRPORT_TYPE;
                drrow["CALL_TYPE"] = _CRMDATA.CALL_TYPE;
                drrow["CUST_RATING"] = "0";
                drrow["CALL_FROMDATE"] = DateTime.ParseExact(_CRMDATA.CALL_FROMDATE, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                drrow["CALL_TODATE"] = DateTime.ParseExact(_CRMDATA.CALL_TODATE, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                drrow["CALL_FROM"] = _CRMDATA.CALL_FROMDATE;
                drrow["CALL_TO"] = _CRMDATA.CALL_TO;
                drrow["REMAINDER_DATE"] = _CRMDATA.REMAINDER_DATE;
                drrow["CALL_CATEGORY"] = _CRMDATA.CALL_CATEGORY;
                drrow["REMARKS"] = _CRMDATA.REMARKS;
                drrow["XML"] = _CRMDATA.Xml;
                drrow["MAIL_XML"] = "";
                drrow["POS_ID"] = strPosId;
                drrow["TERMINAL_ID"] = strTerminalId;
                drrow["ORIGIN"] = _CRMDATA.Orgion;
                drrow["DESTINATION"] = _CRMDATA.Desti;
                drrow["DEPARTURE_DATE"] = _CRMDATA.CALL_FROMDATE;
                drrow["ARRIVAL_DATE"] = _CRMDATA.CALL_TODATE;
                drrow["ATTENDED_BY"] = _CRMDATA.Username;
                drrow["TRIP_TYPE"] = _CRMDATA.triptype;
                drrow["TRIP_STYLE"] = _CRMDATA.AIRPORT_TYPE; ;
                drrow["ADULT_COUNT"] = _CRMDATA.ADTcnt;
                drrow["CHILD_COUNT"] = _CRMDATA.CHDcnt;
                drrow["INFANT_COUNT"] = _CRMDATA.INFcnt;
                drrow["TRIP_CLASS"] = _CRMDATA.tripClass;
                drrow["SEQ_NO"] = strSequenceId;
                drrow["SAVE_DETAILS"] = _CRMDATA.SaveDetails;

                dtInsertRecord.Rows.Add(drrow);

                DsClientDetails.Tables.Add(dtInsertRecord.Copy());
                dsInsertRecord = DsClientDetails.Copy();
                byte[] picbyte = Base.Utilities.ConvertDataSetToByteArray(dsInsertRecord);
                string StrRequst = string.Empty;
                StrRequst = Convert.ToBase64String(picbyte);

                string xml = "<REQUEST>" + JsonConvert.SerializeObject(dsInsertRecord).ToString() + "</REQUEST>";
                // DatabaseLog.LogData(strUserName, "E", "CRM", "InsertEnquiry_REQ", xml, companyid, userid, strSequenceId, strIpAddress);


                _RaysService.Url = strServiceURL;
                Response = _RaysService.InsertEnquiry(StrRequst, strBranchId, strUserName, strIpAddress, Convert.ToDecimal(strSequenceId), strPosId, strTerminalId, strTerminalType);
                Session.Add("ENQCALLID", Response);
                xml = "<RESPONSE>" + Response + "</RESPONSE>";
                //  DatabaseLog.LogData(strUserName, "E", "CRM", "InsertEnquiry_REQ", xml, companyid, userid, strSequenceId, strIpAddress);


                if (!string.IsNullOrEmpty(Response))
                {
                    stu = "01";
                    result = Response;

                }
                else
                {

                    stu = "02";
                    result = Response;
                    msg = "Problem occured while inserting enquiry , Please contact customer care.(#03)";
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                result = Response;
                msg = "Problem occured while inserting enquiry , Please contact customer care.(#05)";
                //  DatabaseLog.LogData(strUserName, "X", "CRM", "InsertEnquiry_REQ", ex.ToString(), companyid, userid, strSequenceId, strIpAddress);
            }

            return Json(new { Status = stu, Message = msg, Result = result });
        }

        public JsonResult UpdateEnquiry(CRMDATA _CRMDATA)
        {

            DataSet DsClientDetails = new DataSet();
            DataSet dsInsertRecord = new DataSet();
            DataTable dtInsertRecord = new DataTable();
            //  DataRow drERPS_Attributes = dtInsertRecord.NewRow();
            string stu = string.Empty, msg = string.Empty, result = string.Empty;
            string Response = string.Empty;
            string strBranchId = string.Empty,
                strUserName = string.Empty,
                strIpAddress = string.Empty,
                strSequenceId = string.Empty,
                strPosId = string.Empty,
                strTerminalId = string.Empty,
                strTerminalType = string.Empty;
            string companyid = string.Empty;
            string userid = string.Empty;
            try
            {

                Object _obj = new Object();
                _obj = System.Web.HttpContext.Current.Session["UserName"];
                if (_obj == null)
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }


                strSequenceId = "0";
                strTerminalType = "W";

                strUserName = System.Web.HttpContext.Current.Session["UserName"] != null ? System.Web.HttpContext.Current.Session["UserName"].ToString() : "";
                strIpAddress = System.Web.HttpContext.Current.Session["IPAddress"] != null ? System.Web.HttpContext.Current.Session["IPAddress"].ToString() : "";
                strPosId = System.Web.HttpContext.Current.Session["POS_ID"] != null ? System.Web.HttpContext.Current.Session["POS_ID"].ToString() : "";
                strTerminalId = System.Web.HttpContext.Current.Session["TerminalID"] != null ? System.Web.HttpContext.Current.Session["TerminalID"].ToString() : "";
                companyid = System.Web.HttpContext.Current.Session["CompanyID"] != null ? System.Web.HttpContext.Current.Session["CompanyID"].ToString() : "";
                userid = System.Web.HttpContext.Current.Session["UserID"] != null ? System.Web.HttpContext.Current.Session["UserID"].ToString() : "";



                dtInsertRecord.TableName = "CRMDATA";
                dtInsertRecord.Columns.Add("CALL_ID");
                dtInsertRecord.Columns.Add("ENQUIRY_ID");
                dtInsertRecord.Columns.Add("STATUS");
                dtInsertRecord.Columns.Add("PAX_MOBILE_NO");
                dtInsertRecord.Columns.Add("PAX_EMAIL_ID");
                dtInsertRecord.Columns.Add("CLIENT_TYPE");
                dtInsertRecord.Columns.Add("CLIENT_CODE");
                dtInsertRecord.Columns.Add("COORDINATOR_ID");
                dtInsertRecord.Columns.Add("PRODUCT");
                dtInsertRecord.Columns.Add("AIRPORT_TYPE");
                dtInsertRecord.Columns.Add("CALL_TYPE");
                dtInsertRecord.Columns.Add("CUST_TYPE");
                dtInsertRecord.Columns.Add("LANGUAGE");
                dtInsertRecord.Columns.Add("CUST_RATING");
                dtInsertRecord.Columns.Add("FORWARD_DEPT");
                dtInsertRecord.Columns.Add("CALL_FROMDATE");
                dtInsertRecord.Columns.Add("CALL_TODATE");
                dtInsertRecord.Columns.Add("CALL_FROM");
                dtInsertRecord.Columns.Add("CALL_TO");
                dtInsertRecord.Columns.Add("REMAINDER_DATE");
                dtInsertRecord.Columns.Add("CALL_CATEGORY");
                dtInsertRecord.Columns.Add("REMARKS");
                dtInsertRecord.Columns.Add("XML");
                dtInsertRecord.Columns.Add("MAIL_XML");
                dtInsertRecord.Columns.Add("POS_ID");
                dtInsertRecord.Columns.Add("TERMINAL_ID");
                dtInsertRecord.Columns.Add("ORIGIN");
                dtInsertRecord.Columns.Add("DESTINATION");
                dtInsertRecord.Columns.Add("DEPARTURE_DATE");
                dtInsertRecord.Columns.Add("ARRIVAL_DATE");
                dtInsertRecord.Columns.Add("ENQUIRY_DETAILS");
                dtInsertRecord.Columns.Add("ATTENDED_BY");
                dtInsertRecord.Columns.Add("TRIP_TYPE");
                dtInsertRecord.Columns.Add("TRIP_STYLE");
                dtInsertRecord.Columns.Add("ADULT_COUNT");
                dtInsertRecord.Columns.Add("CHILD_COUNT");
                dtInsertRecord.Columns.Add("INFANT_COUNT");
                dtInsertRecord.Columns.Add("TRIP_CLASS");
                dtInsertRecord.Columns.Add("SEQ_NO");
                dtInsertRecord.Columns.Add("SAVE_DETAILS");

                DataRow drrow = dtInsertRecord.NewRow();
                string[] Callid = _CRMDATA.Call_ID.Split(',');
                for (int i = 0; i < Callid.Length - 1; i++)
                {
                    drrow = dtInsertRecord.NewRow();
                    drrow["CALL_ID"] = Callid[i];

                    drrow["ENQUIRY_ID"] = _CRMDATA.ENQUIRY_ID;
                    drrow["PAX_MOBILE_NO"] = _CRMDATA.PAX_MOBILE_NO;
                    drrow["PAX_EMAIL_ID"] = _CRMDATA.PAX_EMAIL_ID;
                    drrow["CLIENT_TYPE"] = "";
                    drrow["STATUS"] = _CRMDATA.Status;
                    drrow["CLIENT_CODE"] = _CRMDATA.CLIENT_CODE;

                    drrow["COORDINATOR_ID"] = _CRMDATA.COORDINATOR_ID;
                    drrow["PRODUCT"] = _CRMDATA.PRODUCT;
                    drrow["AIRPORT_TYPE"] = _CRMDATA.AIRPORT_TYPE;
                    drrow["CALL_TYPE"] = _CRMDATA.CALL_TYPE;
                    drrow["CUST_RATING"] = "";
                    drrow["CALL_FROMDATE"] = _CRMDATA.CALL_FROMDATE;
                    drrow["CALL_TODATE"] = _CRMDATA.CALL_TODATE;
                    drrow["CALL_FROM"] = _CRMDATA.CALL_FROMDATE;
                    drrow["CALL_TO"] = _CRMDATA.CALL_TO;
                    drrow["REMAINDER_DATE"] = _CRMDATA.REMAINDER_DATE;
                    drrow["CALL_CATEGORY"] = _CRMDATA.CALL_CATEGORY;
                    drrow["REMARKS"] = _CRMDATA.REMARKS;
                    drrow["XML"] = "";
                    drrow["MAIL_XML"] = "";
                    drrow["POS_ID"] = strPosId;
                    drrow["TERMINAL_ID"] = strTerminalId;
                    drrow["ORIGIN"] = "";
                    drrow["DESTINATION"] = "";
                    drrow["DEPARTURE_DATE"] = "";
                    drrow["ARRIVAL_DATE"] = "";
                    drrow["ATTENDED_BY"] = _CRMDATA.Username;
                    drrow["TRIP_TYPE"] = "";
                    drrow["TRIP_STYLE"] = "";
                    drrow["ADULT_COUNT"] = "";
                    drrow["CHILD_COUNT"] = "";
                    drrow["INFANT_COUNT"] = "";
                    drrow["TRIP_CLASS"] = "";
                    drrow["SEQ_NO"] = strSequenceId;
                    drrow["SAVE_DETAILS"] = _CRMDATA.SaveDetails;

                    dtInsertRecord.Rows.InsertAt(drrow, 0);
                }

                //   dtInsertRecord.Rows.Add(drrow);

                DsClientDetails.Tables.Add(dtInsertRecord.Copy());
                dsInsertRecord = DsClientDetails.Copy();
                byte[] picbyte = Base.Utilities.ConvertDataSetToByteArray(dsInsertRecord);
                string StrRequst = string.Empty;
                StrRequst = Convert.ToBase64String(picbyte);

                string xml = "<REQUEST>" + JsonConvert.SerializeObject(dsInsertRecord).ToString() + "</REQUEST>";

                DatabaseLog.LogData(strUserName, "E", "Search", "UpdateEnquiry_REQ", xml, strPosId, strTerminalId, strSequenceId);

                _RaysService.Url = strServiceURL;
                Response = _RaysService.UpdateEnquiryStatus(StrRequst, "", "", "", Convert.ToDecimal("0"), "", "", "");

                xml = "<RESPONSE>" + Response + "</RESPONSE>";
                //  DatabaseLog.LogData(Session["UserName"] != null ? Session["UserName"].ToString() : "", "E", "CRM", "UpdateEnquiry_REQ", xml, Session["CompanyID"] != null ? Session["CompanyID"].ToString() : "", Session["UserID"] != null ? Session["UserID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0", Session["IPAddress"] != null ? Session["IPAddress"].ToString() : "");
                DatabaseLog.LogData(strUserName, "E", "Search", "UpdateEnquiry_REQ", xml, strPosId, strTerminalId, strSequenceId);

                if (!string.IsNullOrEmpty(Response))
                {
                    stu = "01";
                    result = Response;
                }
                else
                {
                    stu = "02";
                    result = Response;
                    msg = "Problem occured while inserting enquiry , Please contact customer care.(#03)";
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                result = Response;
                msg = "Problem occured while updating enquiry , Please contact customer care.(#05)";
                //  DatabaseLog.LogData(Session["UserName"] != null ? Session["UserName"].ToString() : "", "X", "CRM", "UpdateEnquiry_ERR", ex.ToString(), Session["CompanyID"] != null ? Session["CompanyID"].ToString() : "", Session["UserID"] != null ? Session["UserID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0", Session["IPAddress"] != null ? Session["IPAddress"].ToString() : "");
                //  DatabaseLog.LogData(strUserName, "E", "CRM", "UpdateEnquiry_REQ", ex.ToString(), companyid, userid, strSequenceId, strIpAddress);
                DatabaseLog.LogData(strUserName, "X", "Search", "UpdateEnquiry_REQ", ex.ToString(), strPosId, strTerminalId, strSequenceId);
            }

            return Json(new { Status = stu, Message = msg, Result = result });
        }

        public ActionResult Top5Reminders()//--?
        {
            string stu = string.Empty,
                   msg = string.Empty,
                   result = string.Empty;


            string strBranchId = string.Empty,
              strUserName = string.Empty,
              strIpAddress = string.Empty,
              strSequenceId = string.Empty,
              strPosId = string.Empty,
              strTerminalId = string.Empty,
              strTerminalType = string.Empty;

            try
            {
                Object _obj = new Object();
                _obj = Session["UserName"];
                if (_obj == null)
                {
                    return Json(new { Status = "-1", Message = "", Results = ""});
                }

                strUserName = Session["UserName"] != null ? Session["UserName"].ToString() : "";
                strIpAddress = ControllerContext.HttpContext.Request.UserHostAddress;
                strSequenceId = "0";
                strPosId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strTerminalId = Session["TerminalID"] != null ? Session["TerminalID"].ToString() : "";
                strTerminalType = "W";

                _RaysService.Url = strServiceURL;

                DatabaseLog.LogData(strUserName, "E", "CRM", "Top5Reminders_REQ", "", strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");

                Byte[] Response = _RaysService.Top5Reminder("", strUserName, strIpAddress, Convert.ToDecimal("0"), strPosId, strTerminalId, strTerminalType);
                DataSet _ResponseData = Base.Decompress(Response);

                DatabaseLog.LogData(strUserName, "E", "CRM", "Top5Reminders_RES", JsonConvert.SerializeObject(_ResponseData).ToString(), strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");

                if (_ResponseData != null && _ResponseData.Tables.Count>0)
                {
                    stu = "01";
                    msg = "";
                    result = JsonConvert.SerializeObject(_ResponseData).ToString();
                }
                else
                {
                    stu = "02";
                    msg = "Problem occured while fetching top 5 reminder details,Please contact customer care.(#03)";
                    result = "";
                }

            }
            catch (Exception ex)
            {
                stu = "05";
                msg = "Problem occured while fetching top 5 reminder details,Please contact customer care.(#05)";
                result = "";
                DatabaseLog.LogData(strUserName, "X", "CRM", "UpdateEnquiry_ERR", ex.ToString(), strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");
            }
            return Json(new { Status = stu, Message = msg, Results = result });
        }

        public ActionResult GetAvailability(string ClientID, string EnquiryID, string CallID)//--?
        {
            string stu = string.Empty, msg = string.Empty, result = string.Empty;
            string strGetMobileNo = string.Empty,
                 strUserName = string.Empty,
                 strIpAddress = string.Empty,
                 strSequenceId = string.Empty,
                 strTerminalType = string.Empty,
                 strTerminalId = string.Empty,
                 strPosId = string.Empty,
                 PageFlag = string.Empty;
            string strjsonkeyval = string.Empty;
            try
            {
                Object _obj = new Object();
                _obj = Session["UserName"];
                if (_obj == null)
                {
                    return Json(new { Status = "-1", Message = "", Results = "", KV = "" });
                }

                strUserName = Session["UserName"] != null ? Session["UserName"].ToString() : "";
                strIpAddress = ControllerContext.HttpContext.Request.UserHostAddress;
                strSequenceId = "0";
                strTerminalType = "W";
                strTerminalId = Session["TerminalID"] != null ? Session["TerminalID"].ToString() : "";
                strPosId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                PageFlag = "U";

                _RaysService.Url = strServiceURL;

                DatabaseLog.LogData(strUserName, "E", "CRM", "GetAvailability_REQ", "<REQUEST><CALLID>" + CallID + "</CALLID></REQUEST>", strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");


                Byte[] Response = _RaysService.FETCH_CRM_AVAILABILITY(CallID, ClientID, strIpAddress,"", Convert.ToDecimal(strSequenceId),  strTerminalId,strTerminalType, strPosId);


                DataSet _ResponseData = new DataSet();
                if (Response.Length > 0)
                {


                    _ResponseData = Base.Decompress(Response);
                }

                DatabaseLog.LogData(strUserName, "E", "CRM", "GetAvailability_RES", JsonConvert.SerializeObject(_ResponseData).ToString(), strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");
                if (_ResponseData != null && _ResponseData.Tables.Count > 0 && _ResponseData.Tables[0].Rows.Count > 0)
                {
                    stu = "01";
                    msg = "";
                    result = JsonConvert.SerializeObject(_ResponseData).ToString();
                    FlightDetailsShot op = new FlightDetailsShot();
                    var keyvalcombo = (from p in op.GetType().GetProperties().AsEnumerable()
                                       select new KeyValCombo
                                       {
                                           CD = p.GetCustomAttribute<DataMemberAttribute>().Name,
                                           FN = p.Name
                                       });
                  strjsonkeyval = JsonConvert.SerializeObject(keyvalcombo);
                   
                }
                else
                {

                    stu = "02";
                    msg = "Problem occured while update reminder OFF. Please contact support team (#03).";
                    result = "";
                }

            }
            catch (Exception ex)
            {

                stu = "00";
                msg = "Problem occured while update reminder OFF. Please contact support team (#05).";
                result = "";
                DatabaseLog.LogData(strUserName, "X", "CRM", "GetAvailability_RES", ex.ToString(), strPosId, Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "0");

            }

            return Json(new { Status = stu, Message = msg, Results = result, KV = strjsonkeyval });
        }

        public JsonResult InsertEnquirydata(CRMDATA _CRMDATA)//--?
        {

            DataSet DsClientDetails = new DataSet();
            DataSet dsInsertRecord = new DataSet();
            DataTable dtInsertRecord = new DataTable();


            string strUserName = string.Empty,
                strIpAddress = string.Empty,
                strSequenceId = string.Empty,
                strPosId = string.Empty,
                strTerminalId = string.Empty,
                strTerminalType = string.Empty, strBranchId = string.Empty;
            string Response = string.Empty;
            string stu = string.Empty, msg = string.Empty, result = string.Empty;
            string companyid = string.Empty;
            string userid = string.Empty;
            try
            {

                Object _obj = new Object();
                _obj = Session["UserName"];
                if (_obj == null)
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }

                strBranchId = _CRMDATA.BranchId;
                strSequenceId = "0";
                strTerminalType = "W";
                strUserName = System.Web.HttpContext.Current.Session["UserName"] != null ? System.Web.HttpContext.Current.Session["UserName"].ToString() : "";
                strIpAddress = System.Web.HttpContext.Current.Session["IPAddress"] != null ? System.Web.HttpContext.Current.Session["IPAddress"].ToString() : "";
                strPosId = System.Web.HttpContext.Current.Session["POS_ID"] != null ? System.Web.HttpContext.Current.Session["POS_ID"].ToString() : "";
                strTerminalId = System.Web.HttpContext.Current.Session["TerminalID"] != null ? System.Web.HttpContext.Current.Session["TerminalID"].ToString() : "";
                companyid = System.Web.HttpContext.Current.Session["CompanyID"] != null ? System.Web.HttpContext.Current.Session["CompanyID"].ToString() : "";
                userid = System.Web.HttpContext.Current.Session["POS_TID"] != null ? System.Web.HttpContext.Current.Session["POS_TID"].ToString() : "";
                dtInsertRecord.TableName = "CRMDATA";
                dtInsertRecord.Columns.Add("ENQUIRY_ID");

                dtInsertRecord.Columns.Add("PAX_MOBILE_NO");
                dtInsertRecord.Columns.Add("PAX_EMAIL_ID");
                dtInsertRecord.Columns.Add("CLIENT_TYPE");
                dtInsertRecord.Columns.Add("CLIENT_CODE");
                dtInsertRecord.Columns.Add("COORDINATOR_ID");
                dtInsertRecord.Columns.Add("PRODUCT");
                dtInsertRecord.Columns.Add("AIRPORT_TYPE");
                dtInsertRecord.Columns.Add("CALL_TYPE");
                dtInsertRecord.Columns.Add("CUST_TYPE");
                dtInsertRecord.Columns.Add("LANGUAGE");
                dtInsertRecord.Columns.Add("CUST_RATING");
                dtInsertRecord.Columns.Add("FORWARD_DEPT");
                dtInsertRecord.Columns.Add("CALL_FROMDATE");
                dtInsertRecord.Columns.Add("CALL_TODATE");
                dtInsertRecord.Columns.Add("CALL_FROM");
                dtInsertRecord.Columns.Add("CALL_TO");
                dtInsertRecord.Columns.Add("REMAINDER_DATE");
                dtInsertRecord.Columns.Add("CALL_CATEGORY");
                dtInsertRecord.Columns.Add("REMARKS");
                dtInsertRecord.Columns.Add("XML");
                dtInsertRecord.Columns.Add("MAIL_XML");
                dtInsertRecord.Columns.Add("POS_ID");
                dtInsertRecord.Columns.Add("TERMINAL_ID");
                dtInsertRecord.Columns.Add("ORIGIN");
                dtInsertRecord.Columns.Add("DESTINATION");
                dtInsertRecord.Columns.Add("DEPARTURE_DATE");
                dtInsertRecord.Columns.Add("ARRIVAL_DATE");
                dtInsertRecord.Columns.Add("ENQUIRY_DETAILS");
                dtInsertRecord.Columns.Add("ATTENDED_BY");
                dtInsertRecord.Columns.Add("TRIP_TYPE");
                dtInsertRecord.Columns.Add("TRIP_STYLE");
                dtInsertRecord.Columns.Add("ADULT_COUNT");
                dtInsertRecord.Columns.Add("CHILD_COUNT");
                dtInsertRecord.Columns.Add("INFANT_COUNT");
                dtInsertRecord.Columns.Add("TRIP_CLASS");
                dtInsertRecord.Columns.Add("SEQ_NO");
                dtInsertRecord.Columns.Add("SAVE_DETAILS");

                DataRow drrow = dtInsertRecord.NewRow();
                drrow["ENQUIRY_ID"] = _CRMDATA.ENQUIRY_ID;

                drrow["PAX_MOBILE_NO"] = _CRMDATA.PAX_MOBILE_NO;
                drrow["PAX_EMAIL_ID"] = _CRMDATA.PAX_EMAIL_ID;
                drrow["CLIENT_TYPE"] = "";
                drrow["CUST_TYPE"] = "0";
                drrow["FORWARD_DEPT"] = "0";


                drrow["CLIENT_CODE"] = _CRMDATA.CLIENT_CODE;

                drrow["COORDINATOR_ID"] = _CRMDATA.COORDINATOR_ID;
                drrow["PRODUCT"] = _CRMDATA.PRODUCT;
                drrow["AIRPORT_TYPE"] = _CRMDATA.AIRPORT_TYPE;
                drrow["CALL_TYPE"] = _CRMDATA.CALL_TYPE;
                drrow["CUST_RATING"] = "0";
                drrow["CALL_FROMDATE"] = _CRMDATA.CALL_FROMDATE;
                drrow["CALL_TODATE"] = _CRMDATA.CALL_TODATE;
                drrow["CALL_FROM"] = _CRMDATA.CALL_FROMDATE;
                drrow["CALL_TO"] = _CRMDATA.CALL_TO;
                drrow["REMAINDER_DATE"] = _CRMDATA.REMAINDER_DATE;
                drrow["CALL_CATEGORY"] = _CRMDATA.CALL_CATEGORY;
                drrow["REMARKS"] = _CRMDATA.REMARKS;
                drrow["XML"] = _CRMDATA.Xml;
                drrow["MAIL_XML"] = "";
                drrow["POS_ID"] = strPosId;
                drrow["TERMINAL_ID"] = strTerminalId;
                drrow["ORIGIN"] = "";
                drrow["DESTINATION"] = "";
                drrow["DEPARTURE_DATE"] = "0";
                drrow["ARRIVAL_DATE"] = "";
                drrow["ATTENDED_BY"] = _CRMDATA.Username;
                drrow["TRIP_TYPE"] = "";
                drrow["TRIP_STYLE"] = "";
                drrow["ADULT_COUNT"] = _CRMDATA.ADTcnt;
                drrow["CHILD_COUNT"] = _CRMDATA.CHDcnt;
                drrow["INFANT_COUNT"] = _CRMDATA.INFcnt;
                drrow["TRIP_CLASS"] = "";
                drrow["SEQ_NO"] = strSequenceId;
                drrow["SAVE_DETAILS"] = _CRMDATA.SaveDetails;

                dtInsertRecord.Rows.Add(drrow);

                DsClientDetails.Tables.Add(dtInsertRecord.Copy());
                dsInsertRecord = DsClientDetails.Copy();
                byte[] picbyte = Base.Utilities.ConvertDataSetToByteArray(dsInsertRecord);
                string StrRequst = string.Empty;
                StrRequst = Convert.ToBase64String(picbyte);

                string xml = "<REQUEST>" + JsonConvert.SerializeObject(dsInsertRecord).ToString() + "</REQUEST>";
                DatabaseLog.LogData(strUserName, "E", "CRM", "InsertEnquiry_REQ", xml, strPosId, userid, strSequenceId);


                _RaysService.Url = strServiceURL;
                Response = _RaysService.InsertEnquiry(StrRequst, strBranchId, strUserName, strIpAddress, Convert.ToDecimal(strSequenceId), strPosId, strTerminalId, strTerminalType);

                xml = "<RESPONSE>" + Response + "</RESPONSE>";
                DatabaseLog.LogData(strUserName, "E", "CRM", "InsertEnquiry_REQ", xml, strPosId, userid, strSequenceId);


                if (!string.IsNullOrEmpty(Response))
                {
                    stu = "01";
                    result = Response;
                }
                else
                {

                    stu = "02";
                    result = Response;
                    msg = "Problem occured while inserting enquiry , Please contact customer care.(#03)";
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                result = Response;
                msg = "Problem occured while inserting enquiry , Please contact customer care.(#05)";
                DatabaseLog.LogData(strUserName, "X", "CRM", "InsertEnquiry_REQ", ex.ToString(), strPosId, userid, strSequenceId);
            }

            return Json(new { Status = stu, Message = msg, Result = result });
        }

    }
}
