using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STSTRAVRAYS.Models;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace STSTRAVRAYS.Controllers
{
    public class ReportController : Controller
    {
        private static string strServiceURL = ConfigurationManager.AppSettings["RCSUPPORTSERVICEURL"].ToString();
        private string strLoginClientID = string.Empty;
        private string strLoginTerminalID = string.Empty;
        private string strLoginUserName = string.Empty;
        private string strLoginSeqID = string.Empty;
        private string strLoginIpAddress = string.Empty;
        private string strLoginTerminalType = string.Empty;
        private string strLogData = string.Empty;

        #region common Response Format
        private string Status = string.Empty;
        private string Error = string.Empty;
        private string Result = string.Empty;
        // return Json(new { Status = Status, Error = Error, Result = Result });
        #endregion

        #region Common WebMethod Call Function
        public JObject CallWebMethod(string URL, string Request, ref string Error)
        {
            JObject jbResult = new JObject();
            try
            {
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                byte[] byteGetData = client.UploadData(URL, "POST", Encoding.ASCII.GetBytes(Request));
                jbResult = (JObject)JsonConvert.DeserializeObject(Encoding.ASCII.GetString(byteGetData));
            }
            catch (Exception ex)
            {
                jbResult = null;
                Error = ex.ToString();
            }
            return jbResult;
        }
        #endregion

        #region User Cash Payment details report
        public ActionResult CashPaymentdetails()
        {
            strLoginClientID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            strLoginTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            strLoginTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : "";
            strLoginUserName = Session["USERNAME"] != null && Session["USERNAME"].ToString() != "" ? Session["USERNAME"].ToString() : "";
            strLoginIpAddress = Session["IPADDRESS"] != null && Session["IPADDRESS"].ToString() != "" ? Session["IPADDRESS"].ToString() : "";
            strLoginSeqID = Session["SEQUENCEID"] != null && Session["SEQUENCEID"].ToString() != "" ? Session["SEQUENCEID"].ToString() : DateTime.Now.ToString("yyyymmddhhmmss");

            if (strLoginClientID == "" || strLoginTerminalID == "" || strLoginTerminalType == "" || strLoginUserName == "" || strLoginIpAddress == "")
            {
                return RedirectToAction("SessionExp", "Redirect");
            }

            return View();
        }

        public ActionResult GetCashPaymentDetailReport(string strFromDate, string strToDate, string strBranch, string strProduct)
        {
            strLoginClientID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            strLoginTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            strLoginTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : "";
            strLoginUserName = Session["USERNAME"] != null && Session["USERNAME"].ToString() != "" ? Session["USERNAME"].ToString() : "";
            strLoginIpAddress = Session["IPADDRESS"] != null && Session["IPADDRESS"].ToString() != "" ? Session["IPADDRESS"].ToString() : "";
            strLoginSeqID = Session["SEQUENCEID"] != null && Session["SEQUENCEID"].ToString() != "" ? Session["SEQUENCEID"].ToString() : DateTime.Now.ToString("yyyymmddhhmmss");
            Hashtable MyParam = new Hashtable();
            string strRequest = string.Empty;
            string strResponse = string.Empty;
            string strError = string.Empty;
            JObject JResult = new JObject();
            try
            {
                if (strLoginClientID == "" || strLoginTerminalID == "" || strLoginTerminalType == "" || strLoginUserName == "" || strLoginIpAddress == "")
                {
                    return RedirectToAction("SessionExp", "Redirect");
                }

                if (string.IsNullOrEmpty(strFromDate) || string.IsNullOrEmpty(strToDate))
                {
                    Status = "00";
                    Error = "From date & to date cannnot be empty.";
                    Result = "";
                }
                else if (string.IsNullOrEmpty(strProduct))
                {
                    Status = "00";
                    Error = "Product type cannot be empty.please select product type.";
                    Result = "";
                }
                else
                {
                    strFromDate = DateTime.ParseExact(strFromDate, "dd/mm/yyyy", CultureInfo.InvariantCulture).ToString("yyyymmdd");
                    strToDate = DateTime.ParseExact(strToDate, "dd/mm/yyyy", CultureInfo.InvariantCulture).ToString("yyyymmdd");
                    MyParam.Add("strFromDate", strFromDate);
                    MyParam.Add("strToDate", strToDate);
                    MyParam.Add("strBranch", strBranch);
                    MyParam.Add("strProduct", strProduct);
                    MyParam.Add("strLogUserName", strLoginUserName);
                    MyParam.Add("strLogCliendID", strLoginClientID);
                    MyParam.Add("strLogTerminalID", strLoginTerminalID);
                    MyParam.Add("strLogTerminalType", strLoginTerminalType);
                    MyParam.Add("strLogIPAddress", strLoginIpAddress);
                    MyParam.Add("strLogSeqID", strLoginSeqID);
                    MyParam.Add("strResult", "");
                    MyParam.Add("strError", "");

                    strRequest = JsonConvert.SerializeObject(MyParam);

                    strLogData = "<URL>" + strServiceURL + "</URL>" + "<FUNCTION></FUNCTION>" + "<Request>" + strRequest + "</Request>";
                    DatabaseLog.LogData(strLoginUserName, "E", "ReportController", "GetCashPaymentDetailReport~REQ", strLogData, strLoginClientID, strLoginTerminalID, strLoginSeqID);

                    JResult = CallWebMethod(strServiceURL + "/" + "GetCashPaymentDetailReport", strRequest, ref strError);

                    if (JResult != null)
                    {
                        bool MyResult = (bool)JResult["GetCashPaymentDetailReportResult"];
                        Result = (string)JResult["strResult"];
                        DataSet dsSet = new DataSet();
                        dsSet = JsonConvert.DeserializeObject<DataSet>(Result);
                        Session.Add("EXPORTCASHPAYMENTDATA", dsSet);
                        Error = (string)JResult["strError"];
                        Status = MyResult == true ? "01" : "00";
                    }
                    else
                    {
                        Status = "00";
                        Error = !string.IsNullOrEmpty(strError) ? strError : "Unable to process your request.please contact support team(#03)";
                        Result = "";
                    }
                    strLogData = "<URL>" + strServiceURL + "</URL>" + "<STATUS>" + Status + "</STATUS>" + "<ERROR>" + Error + "</ERROR>"
                            + "<JSON>" + Result + "</JSON>" + "<RESULT>" + Result + "</RESULT>";
                    DatabaseLog.LogData(strLoginUserName, "X", "ReportController", "GetCashPaymentDetailReport~RES", strLogData, strLoginClientID, strLoginTerminalID, strLoginSeqID);
                }
            }
            catch (Exception ex)
            {
                Status = "05";
                Error = "Unable to process your request.please contact support team(#05).";
                Result = "";
                strLogData = "<URL>" + strServiceURL + "</URL>" + "<STATUS>" + Status + "</STATUS>" + "<ERROR>" + Error + "</ERROR>"
                        + "<EXCEPTION>" + ex.ToString() + "</EXCEPTION>" + "<RESULT>" + Result + "</RESULT>";
                DatabaseLog.LogData(strLoginUserName, "X", "ReportController", "GetCashPaymentDetailReport~ERR", strLogData, strLoginClientID, strLoginTerminalID, strLoginSeqID);
            }
            return Json(new { Status = Status, Error = Error, Result = Result });
        }
        #endregion

        #region Common Export Function
        public void ExportData()
        {
            strLoginClientID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
            strLoginTerminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
            strLoginTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : "";
            strLoginUserName = Session["USERNAME"] != null && Session["USERNAME"].ToString() != "" ? Session["USERNAME"].ToString() : "";
            strLoginIpAddress = Session["IPADDRESS"] != null && Session["IPADDRESS"].ToString() != "" ? Session["IPADDRESS"].ToString() : "";
            strLoginSeqID = Session["SEQUENCEID"] != null && Session["SEQUENCEID"].ToString() != "" ? Session["SEQUENCEID"].ToString() : DateTime.Now.ToString("yyyymmddhhmmss");

            try
            {
                DateTime datetime = DateTime.Now;
                string toDate = datetime.ToString("yyyy-MM-ddTHH:mm");
                DataSet ds = new DataSet();
                ds = (DataSet)Session["EXPORTCASHPAYMENTDATA"];
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=CashPaymentDetailReport_" + toDate + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Write("<HTML><HEAD>");
                Response.Write("<style>TD {font-family:Verdana; font-size: 11px;} </style>");
                Response.Write("</HEAD><BODY>");
                Response.Write("<TABLE border='1' style='width:950px'>");

                Response.Write("<TR>");
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    Response.Write("<TD style='font-weight:bold;background-color:#245b9d; color:#fff;font-size: 12px;'>" +
                    (ds.Tables[0].Columns[i].ToString().Contains("_") ? ds.Tables[0].Columns[i].ToString().Replace('_', ' ') : ds.Tables[0].Columns[i].ToString()) + "</TD>");
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
            catch (Exception ex)
            {
                strLogData = "<EXCEPTION>" + ex.ToString() + "</EXCEPTION>";
                DatabaseLog.LogData(strLoginUserName, "X", "ReportController", "ExportData~ERR", strLogData, strLoginClientID, strLoginTerminalID, strLoginSeqID);
            }
        }
        #endregion
    }
}
