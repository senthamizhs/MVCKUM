using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using System.Runtime.Serialization;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Net;
using System.Text.RegularExpressions;
using STSTRAVRAYS.Models;
using System.IO.Compression;

namespace STSTRAVRAYS.Controllers
{
    public class RescheduleController : Controller
    {
        //
        // GET: /Reschedule/
        #region rescheule process
        string IpaddressGloab = "";
        string strBranchCredit = "";

        public static Hashtable AirlineCode = new Hashtable();

        public string LoadServerdatetime()
        {
            // double GMT = ConfigurationManager.AppSettings["GMTTIME"].ToString() == "" ? 0 : Convert.ToDouble(ConfigurationManager.AppSettings["GMTTIME"].ToString());
            double GMT = Convert.ToDouble(Session["GMTTIME"] != null ? Session["GMTTIME"].ToString() : ConfigurationManager.AppSettings["GMTTIME"].ToString());
            DateTime dateTime = DateTime.Now.AddMinutes(GMT);//.Now.Date;
            return dateTime.ToString("dd/MM/yyyy");
        }

        public static Byte[] Compress(DataSet dataset)
        {
            Byte[] data;
            MemoryStream mem = new MemoryStream();
            GZipStream zip = new GZipStream(mem, CompressionMode.Compress);
            dataset.WriteXml(zip, XmlWriteMode.WriteSchema);
            zip.Close();
            data = mem.ToArray();
            mem.Close();
            return data;
        }

        public static DataSet Decompress(Byte[] data)
        {
            try
            {
                MemoryStream mem = new MemoryStream(data);
                GZipStream zip = new GZipStream(mem, CompressionMode.Decompress);
                DataSet dataset = new DataSet();
                dataset.ReadXml(zip, XmlReadMode.ReadSchema);
                zip.Close();
                mem.Close();
                return dataset;
            }
            catch (Exception ex)
            {
                string Message = ex.ToString();
                string Description = "<ERROR><DESC>" + Message + "</DESC></ERROR>";
                return null;
            }
        }

        public string airlineNameRES(string airlineCode)
        {
            try
            {
                if (AirlineCode.ContainsKey(airlineCode))
                {
                    var foo = AirlineCode[airlineCode];
                    return foo.ToString();
                }
                else
                {
                    DataSet dsAirways = new DataSet();
                    dsAirways.ReadXml(Server.MapPath("~/XML/AirlineNames.xml").ToString());
                    string airwaysName = "";
                    var qryAirlineName = from p in dsAirways.Tables
                                       ["AIRLINEDET"].AsEnumerable()
                                         where p.Field<string>
                                       ("_CODE") == airlineCode
                                         select p;
                    DataView dvAirlineCode = qryAirlineName.AsDataView();
                    if (dvAirlineCode.Count == 0)
                        airwaysName = airlineCode;
                    else
                    {
                        DataTable dtAilineCode = new DataTable();
                        dtAilineCode = qryAirlineName.CopyToDataTable();
                        airwaysName = dtAilineCode.Rows[0]

                        ["_NAME"].ToString();
                        AirlineCode.Add(airlineCode, airwaysName);
                    }
                    return airwaysName;
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "airlinename", "Base");
                return string.Empty;
            }
        }

        public string CultureInfoToConvertDatetimeFormatPnrVarify(DateTime dateTimeTemp, string format)
        {
            CultureInfo cii = new CultureInfo("en-GB", true);
            string requiredDate = dateTimeTemp.ToString(format, cii);
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //Get date and time in US Mountain Standard Time 
            DateTime dateTime = TimeZoneInfo.ConvertTime(dateTimeTemp, timeZoneInfo);
            requiredDate = dateTime.ToString(format);
            return requiredDate;
        }

        public DataTable ConvertToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names   
            PropertyInfo[] oProps = null;
            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow   
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public DataSet convertJsonStringToDataSet(string jsonString, string strFunction)
        {
            DataSet dsResultset = new DataSet();
            try
            {
                XmlDocument _xmlDoc = new XmlDocument();
                jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
                _xmlDoc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
                dsResultset.ReadXml(new XmlNodeReader(_xmlDoc));
                return dsResultset;
            }
            catch (Exception)
            {
                return dsResultset;
            }
        }

        private static Decimal ConvertToDecimal(string Data)
        {
            CultureInfo invariant = CultureInfo.InvariantCulture;
            return Convert.ToDecimal(Convert.ToDecimal(Data, invariant).ToString("0", invariant), invariant);


        }

        public static double CovertToDouble(string value)
        {
            double result = 0;
            CultureInfo invariant = CultureInfo.InvariantCulture;
            result = string.IsNullOrEmpty(value) ? 0 : Convert.ToDouble(value, invariant);
            return result;

        }

        public static string Faretypetext(string faretype, string faretypeDescription)
        {
            string Faretypetext = string.Empty;
            try
            {
                if (faretype == "R")
                    Faretypetext = "Retail Fare";
                else if (faretype == "C")
                    Faretypetext = "Corporate Fare";
                else if (faretype == "M")
                    Faretypetext = "SME Fare";
                else if (faretype == "U")
                    Faretypetext = "Flat Fare";
                else if (faretype == "G")
                    Faretypetext = "Marine Fare";
                else if (faretypeDescription == "C")
                    Faretypetext = "Corporate Fare";
                else if (faretypeDescription == "R")
                    Faretypetext = "Retail Fare";
                else if (faretypeDescription == "E")
                    Faretypetext = "Ecoupon Fare";
                else if (faretypeDescription == "F")
                    Faretypetext = "Flexi Fare";
                else if (faretypeDescription == "U")
                    Faretypetext = "Flat Fare";
                else if (faretypeDescription == "M")
                    Faretypetext = "Normal SME Fare";
                else if (faretypeDescription == "S")
                    Faretypetext = "Special  Fare";
                else if (faretypeDescription == "CF")
                    Faretypetext = "Corporate Flexi Fare";
                else if (faretypeDescription == "I")
                    Faretypetext = "Corporate SME fare";
                else if (faretypeDescription == "J")
                    Faretypetext = "Retail SME fare";
                else if (faretypeDescription == "Z")
                    Faretypetext = "Corporate Flexi Fare";
                else
                    Faretypetext = "Normal Fare";

            }
            catch (Exception)
            {

                return Faretypetext;
            }
            return Faretypetext;
        }
        public ActionResult get_reschedule_process(string spnr, string airlinepnr, string crspnr, string strPaymentmode)
        {
            //#region UsageLog
            //string PageName = "Reschedule";
            //try
            //{
            //    string strUsgLogRes = Commonlog(PageName, "", "FETCH");
            //}
            //catch (Exception e)
            //{

            //}
            //#endregion
            Rays_service.RaysService _rays_servers = new Rays_service.RaysService();

            ArrayList arrayViewPNR = new ArrayList();
            ArrayList arrayaddPNR = new ArrayList();
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            arrayViewPNR.Add("");
            int Error = 0;
            int Response = 1;
            int pnr_details = 2;
            int contact_details = 3;
            int modify_response = 4;
            int pax_details = 5;
            int resheduletype = 6;
            StringBuilder strBuilderPNR = new StringBuilder();
            StringBuilder strBuilderPNR1 = new StringBuilder();
            StringBuilder ViewTotal = new StringBuilder();
            string getAgentType = string.Empty;
            string getprevilage = string.Empty;
            ViewBag.ServerDateTime = LoadServerdatetime();
            // int Result = 2;
            try
            {
                string strAgentID = Session["POS_ID"].ToString();
                string strTerminalId = Session["POS_TID"].ToString();
                string strUserName = Session["username"].ToString();
                string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");
                // string strAppType = "B2B";
                string sequnceID = Session["sequenceid"].ToString();// "0";
                string terminalType = Session["TerminalType"] != null ? Session["TerminalType"].ToString() : "";
                string Ipaddress = Session["ipAddress"].ToString();
                //getprevilage = Session["privilage"].ToString();
                //getAgentType = "";
                string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"].ToString() != "") ? Session["TKTFLAG"].ToString() : "DTKT";
                #region
                string ConsoleAgent = "";// "";
                if (getprevilage.ToString() == "S")
                {
                    strTerminalId = string.Empty;
                }
                else
                {
                    strTerminalId = Session["POS_TID"].ToString();
                }
                if (getAgentType == ConsoleAgent)
                {
                    strAgentID = string.Empty;
                    strTerminalId = string.Empty;
                }
                if (terminalType == "T")
                {
                    strAgentID = string.Empty;
                }
                #endregion


                StringBuilder strpassdetails = new StringBuilder();
                DataSet dsViewPNR = new DataSet();
                DataSet dsFareDispDet = new DataSet();
                DataSet dsPassDetails = new DataSet();

                byte[] dsViewPNR_compress = new byte[] { };
                byte[] dsFareDispDet_compress = new byte[] { };



                string refError = "ERROR";
                string strErrorMsg = "ERROR";
                string PayMode = string.Empty;
                bool result = false;
                DataSet dsDisplayDet = new DataSet();


                _rays_servers.Url = ConfigurationManager.AppSettings["serviceuri"].ToString();
                result = _rays_servers.Fetch_PNR_Verification_Details_NewByte(strAgentID, spnr, airlinepnr, crspnr, "", strTerminalId, strUserName, "", "", Convert.ToDecimal(sequnceID), ref dsViewPNR_compress, ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "FetchreschedulePnr", "FetchPNRdetailsforReschedule", getAgentType, strTerminalId);


                dsViewPNR = Decompress(dsViewPNR_compress);
                dsDisplayDet = Decompress(dsFareDispDet_compress);

                if (dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                {
                    string strClientBranchID = string.Empty;
                    strClientBranchID = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("TAI_ISSUING_BRANCH_ID") ? dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TAI_ISSUING_BRANCH_ID"].ToString() : "";
                    Session.Add("ClientBranchID", strClientBranchID);
                    string StockType = Regex.Split(dsViewPNR.Tables[0].Rows[0]["StockType"].ToString(), "SPLIT")[0].ToString();
                    string Aircategory = dsViewPNR.Tables[0].Rows[0]["AIRLINE_CATEGORY"].ToString();
                    arrayViewPNR[resheduletype] = (ConfigurationManager.AppSettings["Online_RescheduleAirline"].ToString().Contains(Aircategory) || ConfigurationManager.AppSettings["Online_RescheduleAirline"].ToString().Contains(StockType)) ? "ONLINE" : "OFFLINE";
                }

                if (result == false)
                {
                    if (dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                    {
                        string record = JsonConvert.SerializeObject(dsViewPNR);
                        arrayViewPNR[modify_response] = record;
                    }
                    else
                    {
                        arrayViewPNR[Error] = "Please enter valid PNR No.";
                    }
                }
                else
                {
                    if (dsViewPNR != null && dsViewPNR.Tables[0].Rows.Count > 0)
                    {
                        if (dsViewPNR != null)
                        {
                            string S_PNR = dsViewPNR.Tables[0].Rows[0]["S_PNR"].ToString();
                            string txt_AirPNR = dsViewPNR.Tables[0].Rows[0]["AIRLINE_PNR"].ToString();
                            string txt_CSRPNR = dsViewPNR.Tables[0].Rows[0]["CRS_PNR"].ToString();
                            arrayViewPNR[pnr_details] = S_PNR + "|" + txt_AirPNR + "|" + txt_CSRPNR;

                        }

                        if (dsViewPNR.Tables[0].Rows[0]["PAYMENT_MODE"].ToString() == "T" || dsViewPNR.Tables[0].Rows[0]["PAYMENT_MODE"].ToString() == "Topup")
                        {
                            PayMode = "Topup";
                        }
                        else if (dsViewPNR.Tables[0].Rows[0]["PAYMENT_MODE"].ToString() == "C" || dsViewPNR.Tables[0].Rows[0]["PAYMENT_MODE"].ToString().ToUpper() == "CREDIT")
                        {
                            PayMode = "Credit";
                        }
                        else if (dsViewPNR.Tables[0].Rows[0]["PAYMENT_MODE"].ToString() == "P")
                        {
                            PayMode = "Payment Gateway";
                        }

                        string user_name = ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("NAME") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["NAME"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["NAME"].ToString().TrimStart('.') : "N/A");
                        string Contact_No = ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("MOBILE_NO") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["MOBILE_NO"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["MOBILE_NO"].ToString() : "N/A");
                        string email_id = ((dsViewPNR.Tables.Contains("ContactDetails") && dsViewPNR.Tables["ContactDetails"].Columns.Contains("EMAIL_ID") && dsViewPNR.Tables["ContactDetails"].Rows.Count > 0 && dsViewPNR.Tables["ContactDetails"].Rows[0]["EMAIL_ID"].ToString().Trim() != "") ? dsViewPNR.Tables["ContactDetails"].Rows[0]["EMAIL_ID"].ToString() : "N/A");
                        string imageurl = ConfigurationManager.AppSettings["FlightUrl"] + dsViewPNR.Tables[0].Rows[0]["PLATING_CARRIER"].ToString() + ".png";
                        string img_alt = dsViewPNR.Tables[0].Rows[0]["PLATING_CARRIER"].ToString();
                        string img_airlinename = airlineNameRES(dsViewPNR.Tables[0].Rows[0]["PLATING_CARRIER"].ToString());
                        string booked_terminal = dsViewPNR.Tables[0].Rows[0]["TERMINAL_ID"] + "(" + dsViewPNR.Tables[0].Rows[0]["USER_NAME"] + ")";
                        string p_mode = PayMode;
                        string issue_Date = dsViewPNR.Tables[0].Rows[0]["BOOKED_DATE"].ToString();

                        arrayViewPNR[contact_details] = user_name + "|" + Contact_No + "|" + email_id + "|" + imageurl + "|" + img_alt + "|" + img_airlinename + "|" + booked_terminal + "|" + p_mode + "|" + issue_Date;

                        if (dsViewPNR.Tables[0].Rows.Count > 0)
                        {

                            if (dsViewPNR.Tables[0].AsEnumerable().Any(v => v.Field<string>("TICKET_STATUS_FLAG") == "C"))
                            {
                                var PNRREFNO = (from input in dsViewPNR.Tables[0].AsEnumerable()
                                                where input["SEGMENT_NO"].ToString() == "1"
                                                select new
                                                {
                                                    PAX_REF_NO = input["PAX_REF_NO"].ToString(),
                                                    PAX_TYPE = input["PASSENGER_TYPE"].ToString().ToUpper(),
                                                    PASSENGER_NAME = input["PASSENGER_NAME"].ToString(),
                                                    TICKET_NO = input["TICKET_NO"].ToString(),
                                                    STATUS = input["TICKET_STATUS"].ToString(),
                                                    GROSSFARE = input["GROSSFARE"].ToString(),
                                                    NET_AMOUNT = input["NET_AMOUNT"].ToString(),
                                                }).Distinct();
                                DataTable dtpaxcount = ConvertToDataTable(PNRREFNO);
                                string Reschdulepaxdetails = JsonConvert.SerializeObject(dtpaxcount);

                                arrayViewPNR[pax_details] = Reschdulepaxdetails;

                                var qryAirlineMarge = (from input in dsViewPNR.Tables[0].AsEnumerable()
                                                       where input["PAX_REF_NO"].ToString() == "1"
                                                       select new
                                                       {
                                                           SPNR = input["S_PNR"].ToString(),
                                                           FLIGHT_NO = input["FLIGHT_NO"].ToString(),
                                                           ORIGIN = input["ORIGINCODE"].ToString(),
                                                           DESTINATION = input["DESTINATIONCODE"].ToString(),
                                                           DEPT_DATE = CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["DEPT_DATE"].ToString()), "dd/MM/yyyy HH:mm"),
                                                           CLASS_ID = input["CLASS_ID"].ToString(),
                                                           GROSSFARE = input["GROSSFARE"].ToString(),
                                                           STATUS = input["TICKET_STATUS"].ToString(),
                                                           PAX_REF_NO = input["PAX_REF_NO"].ToString(),
                                                           SEGMENT_NO = input["SEGMENT_NO"].ToString(),
                                                           ARRIVAL_DATE = CultureInfoToConvertDatetimeFormatPnrVarify(Convert.ToDateTime(input["ARRIVAL_DATE"].ToString()), "dd/MM/yyyy HH:mm"),
                                                           TRIPNO = input["TCK_TRIP_NO"].ToString(),
                                                           PLATING_CARRIER = input["PLATING_CARRIER"].ToString(),
                                                           SEG_TYPE = input["AIRPORTID"].ToString(),
                                                       });

                                DataTable dtsegcount = ConvertToDataTable(qryAirlineMarge);
                                string reschedule_proce = JsonConvert.SerializeObject(dtsegcount);
                                arrayViewPNR[Response] = reschedule_proce;
                            }
                            else if (dsViewPNR.Tables[0].AsEnumerable().All(v => v.Field<string>("TICKET_STATUS_FLAG") == "F"))
                            {
                                arrayViewPNR[Error] = "Ticket already Cancelled. (#03)";
                            }
                            else if (dsViewPNR.Tables[0].AsEnumerable().All(v => v.Field<string>("TICKET_STATUS_FLAG") == "R"))
                            {
                                arrayViewPNR[Error] = "Ticket already Rescheduled. (#03)";
                            }
                            else if (dsViewPNR.Tables[0].AsEnumerable().All(v => v.Field<string>("TICKET_STATUS_FLAG") == "D"))
                            {
                                arrayViewPNR[Error] = "Ticket already requested for Cancellation. Please contact support team. (#03)";
                            }
                            else if (dsViewPNR.Tables[0].AsEnumerable().All(v => v.Field<string>("TICKET_STATUS_FLAG") == "S"))
                            {
                                arrayViewPNR[Error] = "Ticket already requested for Reschedule. Please contact support team. (#03)";
                            }
                            else
                            {
                                arrayViewPNR[Error] = "Kindly check the PNR status (#03)";
                            }


                        }
                    }
                    else
                    {
                        string display = "Please enter Valid PNR No";
                        arrayViewPNR[Error] = display;
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Reschedule", "RequestReschedule", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                string display = "Problem Occured while fetching details. Please contact customer care.";
                arrayViewPNR[Error] = display;

            }

            return Json(new { Status = "", Message = "", Result = arrayViewPNR });
        }

        public ActionResult Rescheduleavailablity(string Tripref, string RescheduleDate, string Arrivdate, string Spnr, string Cabin, string Origin, string Dest, string Triptype)
        {
            ArrayList availabile = new ArrayList(2);
            availabile.Add("");
            availabile.Add("");
            availabile.Add("");
            availabile.Add("");
            availabile.Add("");
            availabile.Add("");
            int error = 0;
            int response = 1;
            int returnval = 2;
            int baseorigin = 3;
            int basedesination = 4;
            int segcount = 5;
            string triptype = string.Empty;
            Rays_service.RaysService _rays_servers = new Rays_service.RaysService();;
            string jsonresultsReschedule = string.Empty;
            RescheduleDate = RescheduleDate.Replace('-', '/');
            string strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
            string strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString() : "";
            string strUserName = Session["username"] != null && Session["username"] != "" ? Session["username"].ToString() : "";
            string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");
            string strsequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyMMdd");
            string strTerminalType = Session["TerminalType"] != null ? Session["TerminalType"].ToString() : "";
            string Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string strClientType = Session["agenttype"] != null && Session["agenttype"] != "" ? "" : "";

            string strClient_ID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strClient_TID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";

            try
            {


                if (strTerminalId == "" || strUserName == "" || strAgentID == "")
                {
                    availabile[error] = "session has been expired!.";
                    return Json(new { Status = "-1", Message = "session has been expired!", Result = availabile });
                }
                string cancelledErrorMsg = string.Empty;
                string strErrorMsg = string.Empty;
                string strResult = string.Empty;
                string flighdetails = "";
                DataTable dtMulticityAvail = new DataTable();
                DataSet dsAvail = new DataSet();
                DataSet dsReSchedule = new DataSet();
                DataSet dsFareDispDet = new DataSet();
                DataTable dt_avail = new DataTable();
                string refError = "";
                bool result;
                string AirportType = string.Empty;
                string TripType = string.Empty;
                // string PaxType = string.Empty;
                #region Request Forming to Get Availability..............


                string OnwardSegmentNo = string.Empty;
                string ReturnSegmentNo = string.Empty;
                string OriginCity = string.Empty;
                string DestinationCity = string.Empty;
                string Cabin_class = string.Empty;
                string Flightno = string.Empty;
                string PayMode = string.Empty;
                DataSet dsDisplayDet = new DataSet();
                DataSet dsViewPNR = new DataSet();

                byte[] dsViewPNR_compress = new byte[] { };
                byte[] dsFareDispDet_compress = new byte[] { };


                OnlineserviceRQRS.RescheduleAvailRQ Itnery = new OnlineserviceRQRS.RescheduleAvailRQ();
                List<OnlineserviceRQRS.AvailabilityRequests> avail = new List<OnlineserviceRQRS.AvailabilityRequests>();
                OnlineserviceRQRS.AvailabilityRequests ReuqestAvail = new OnlineserviceRQRS.AvailabilityRequests();

                OnlineserviceRQRS.PromoCodeRS _PromocodeRsdata = new OnlineserviceRQRS.PromoCodeRS();
                List<OnlineserviceRQRS.Promocodes> Prcode = new List<OnlineserviceRQRS.Promocodes>();
                OnlineserviceRQRS.Promocodes Promocodes = new OnlineserviceRQRS.Promocodes();

                OnlineserviceRQRS.TicketNumberDetails Ticktdet = new OnlineserviceRQRS.TicketNumberDetails();
                List<OnlineserviceRQRS.TicketNumberDetails> _tktno = new List<OnlineserviceRQRS.TicketNumberDetails>();

                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                result = _rays_servers.Fetch_PNR_Verification_Details_NewByte(strClient_ID, Spnr, "", "", "",
                            strClient_TID, strUserName, Session["ipAddress"].ToString(), strTerminalType, Convert.ToDecimal(strsequnceID), ref dsViewPNR_compress,
                        ref dsFareDispDet_compress, ref refError, ref strErrorMsg, "FetchPnrforReschedule", "FetchPnrdetailsforrescheduleavailability", strClientType, strClient_TID);

                dsReSchedule = Decompress(dsViewPNR_compress);
                dsFareDispDet = Decompress(dsFareDispDet_compress);

                if (dsReSchedule != null && dsReSchedule.Tables.Count > 0 && dsReSchedule.Tables[0].Rows.Count > 0)
                {
                    Promocodes.CodeType = "";
                    Promocodes.Flight = "";
                    Promocodes.PromoCode = "";
                    Promocodes.OfficeId = "";
                    Prcode.Add(Promocodes);

                    #region Complete Method

                    var qry = (from p in dsReSchedule.Tables[0].AsEnumerable()
                               where p["TCK_TRIP_NO"].ToString() == Tripref
                               orderby p["SEGMENT_NO"]
                               select new
                               {
                                   SEGMENT_NO = p["SEGMENT_NO"],
                               }).Distinct();
                    if (qry.Count() > 0)
                    {
                        DataTable dtSegNo = ConvertToDataTable(qry);
                        OnwardSegmentNo = dtSegNo.Rows[0]["SEGMENT_NO"].ToString();
                    }

                    var qryR = (from p in dsReSchedule.Tables[0].AsEnumerable()
                                where (Tripref == "0" ? p["TCK_TRIP_NO"].ToString() == "0" : p["TCK_TRIP_NO"].ToString() == "1")
                                orderby p["SEGMENT_NO"]
                                select new
                                {
                                    SEGMENT_NO = p["SEGMENT_NO"]
                                }).Distinct();
                    if (qryR.Count() > 0)
                    {
                        DataTable dtSegNo = ConvertToDataTable(qryR);
                        int rowcount = Convert.ToInt16(dtSegNo.Rows.Count);
                        availabile[segcount] = rowcount;
                        // ReturnSegmentNo = (Tripref == "1" ? dtSegNo.Rows[0][0].ToString() : dtSegNo.Rows[rowcount - 1][0].ToString());
                        ReturnSegmentNo = dtSegNo.Rows[rowcount - 1][0].ToString();
                    }

                    var origin_code = (from p in dsReSchedule.Tables[0].AsEnumerable()
                                       where p["SEGMENT_NO"].ToString() == OnwardSegmentNo
                                       select new
                                       {
                                           ORIGINCODE = p["ORIGINCODE"],
                                       }).Distinct();
                    if (origin_code.Count() > 0)
                    {
                        DataTable dtOrgin = ConvertToDataTable(origin_code);
                        OriginCity = dtOrgin.Rows[0]["ORIGINCODE"].ToString();
                    }
                    var destination_code = (from p in dsReSchedule.Tables[0].AsEnumerable()
                                            where p["SEGMENT_NO"].ToString() == ReturnSegmentNo
                                            select new
                                            {
                                                DESTINATIONCODE = p["DESTINATIONCODE"],
                                            }).Distinct();
                    if (destination_code.Count() > 0)
                    {
                        DataTable dtOrgin = ConvertToDataTable(destination_code);
                        DestinationCity = dtOrgin.Rows[0]["DESTINATIONCODE"].ToString();
                    }

                    var flight_no = (from p in dsReSchedule.Tables[0].AsEnumerable()
                                     where (Tripref == "0" ? p["SEGMENT_NO"].ToString() == OnwardSegmentNo : p["SEGMENT_NO"].ToString() == ReturnSegmentNo)
                                     select new
                                     {
                                         FLIGHT_NO = p["FLIGHT_NO"],
                                     }).Distinct();
                    if (destination_code.Count() > 0)
                    {
                        DataTable flightno = ConvertToDataTable(flight_no);
                        Flightno = flightno.Rows[0]["FLIGHT_NO"].ToString();
                    }

                    #endregion
                }

                OriginCity = Origin;
                DestinationCity = Dest;
                Ticktdet.TicketNumber = dsReSchedule.Tables[0].Rows[0]["TICKET_NO"].ToString().Trim();
                Ticktdet.PaxRefNo = dsReSchedule.Tables[0].Rows[0]["PAX_REF_NO"].ToString().Trim();
                Ticktdet.PaxType = dsReSchedule.Tables[0].Rows[0]["PASSENGER_TYPE"].ToString().Trim();
                Ticktdet.SegNo = dsReSchedule.Tables[0].Rows[0]["SEGMENT_NO"].ToString().Trim();

                _tktno.Add(Ticktdet);

                availabile[baseorigin] = OriginCity.Trim();
                availabile[basedesination] = DestinationCity.Trim();

                AirportType = dsReSchedule.Tables[0].Rows[0]["AIRPORTID"].ToString().Trim();
                TripType = dsReSchedule.Tables[0].Rows[0]["TRIP_DESC"].ToString();
                if (AirportType == "I" && TripType == "M")
                {
                    ReuqestAvail.DepartureStation = OriginCity.Trim();
                    ReuqestAvail.ArrivalStation = DestinationCity.Trim();
                    ReuqestAvail.FlightDate = DateTime.ParseExact(RescheduleDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    ReuqestAvail.FareclassOption = Cabin.Trim();
                    ReuqestAvail.FareType = dsReSchedule.Tables[0].Rows[0]["SPECIAL_TRIP"].ToString();
                    avail.Add(ReuqestAvail);
                }
                else
                {
                    ReuqestAvail.DepartureStation = OriginCity.Trim();
                    ReuqestAvail.ArrivalStation = DestinationCity.Trim();
                    ReuqestAvail.FlightDate = DateTime.ParseExact(RescheduleDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    ReuqestAvail.FareclassOption = Cabin.Trim();
                    ReuqestAvail.FareType = dsReSchedule.Tables[0].Rows[0]["SPECIAL_TRIP"].ToString(); //"N";
                    avail.Add(ReuqestAvail);
                }
                if (Triptype == "R")
                {
                    ReuqestAvail = new OnlineserviceRQRS.AvailabilityRequests();
                    ReuqestAvail.DepartureStation = DestinationCity.Trim();
                    ReuqestAvail.ArrivalStation = OriginCity.Trim();
                    ReuqestAvail.FlightDate = DateTime.ParseExact(Arrivdate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                    ReuqestAvail.FareclassOption = Cabin.Trim();
                    ReuqestAvail.FareType = dsReSchedule.Tables[0].Rows[0]["SPECIAL_TRIP"].ToString(); //"N";
                    avail.Add(ReuqestAvail);
                }
                Itnery.AvailabilityRequest = avail;

                OnlineserviceRQRS.AgentDetails agentDet = new OnlineserviceRQRS.AgentDetails();
                agentDet.AgentId = strAgentID;
                agentDet.TerminalId = strTerminalId;
                agentDet.UserName = strUserName;
                agentDet.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();

                agentDet.Agenttype = strClientType;
                agentDet.AirportID = dsReSchedule.Tables[0].Rows[0]["AIRPORTID"].ToString().Trim();
                agentDet.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                agentDet.AppType = "B2B";
                agentDet.BOAID = dsReSchedule.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["POS_ID"].ToString().Trim();
                //agentDet.BOAID = Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                agentDet.BOAterminalID = dsReSchedule.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["POS_TERMINAL_ID"].ToString().Trim();
                //agentDet.BOAterminalID = Session["POS_TID"] != null && Session["POS_TID"] != "" ? Session["POS_TID"].ToString() : "";
                agentDet.BranchID = dsReSchedule.Tables[0].Rows[0]["TAI_ISSUING_BRANCH_ID"].ToString().Trim();//strBranchID;
                agentDet.IssuingBranchID = dsReSchedule.Tables[0].Rows[0]["TAI_ISSUING_BRANCH_ID"].ToString().Trim();
                // agentDet.IssuingBranchID = Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                agentDet.ClientID = dsReSchedule.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["CLIENT_ID"].ToString().Trim();
                agentDet.CoOrdinatorID = "";
                agentDet.Environment = strTerminalType;
                agentDet.Platform = "B";
                agentDet.ProjectID = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"] != "" ? Session["PRODUCT_CODE"].ToString() : "";

                agentDet.COST_CENTER = "";
                agentDet.Group_ID = "";
                agentDet.EMP_ID = "";
                agentDet.UID = "";

                Itnery.Agent = agentDet;
                #region Log
                StringWriter strres = new StringWriter();

                if (dsReSchedule != null)
                    dsReSchedule.WriteXml(strres);



                #endregion
                //string cate = dsReSchedule.Tables[0].Columns.Contains("StockType") && dsReSchedule.Tables[0].Rows[0]["StockType"].ToString() != ""
                //    && dsReSchedule.Tables[0].Rows[0]["StockType"].ToString().Length > 3 && dsReSchedule.Tables[0].Rows[0]["StockType"].ToString().Contains("SPLIT") ? Regex.Split(dsReSchedule.Tables[0].Rows[0]["StockType"].ToString(), "SPLIT")[0] : "UAI";

                //Itnery.Category = dsReSchedule.Tables[0].Columns.Contains("StockType") && dsReSchedule.Tables[0].Rows[0]["StockType"].ToString() != ""
                //    && dsReSchedule.Tables[0].Rows[0]["StockType"].ToString().Length > 3 && dsReSchedule.Tables[0].Rows[0]["StockType"].ToString().Contains("SPLIT") ? Regex.Split(dsReSchedule.Tables[0].Rows[0]["StockType"].ToString(), "SPLIT")[0] : "UAI";

                Itnery.Category = dsReSchedule.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("CRS_ID") ? dsReSchedule.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["CRS_ID"].ToString().Trim().ToUpper() : "";
                Itnery.Stock = dsReSchedule.Tables["P_FETCH_PNR_DETAILS_BOA"].Columns.Contains("CRS_ID") ? dsReSchedule.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["CRS_ID"].ToString().Trim().ToUpper() : "";
                Itnery.TripType = Triptype;
                Itnery.SegmentType = AirportType;

                #region Passengers

                OnlineserviceRQRS.Passengers lstrPax = new OnlineserviceRQRS.Passengers();
                List<OnlineserviceRQRS.PaxTypeRefs> lstrReferencePax = new List<OnlineserviceRQRS.PaxTypeRefs>();
                if (Convert.ToInt16(dsReSchedule.Tables[0].Rows[0]["AdultCount"].ToString().Trim()) > 0)
                {
                    OnlineserviceRQRS.PaxTypeRefs RefAdt = new OnlineserviceRQRS.PaxTypeRefs();
                    RefAdt.Type = "ADT";
                    RefAdt.Quantity = dsReSchedule.Tables[0].Rows[0]["AdultCount"].ToString().Trim();
                    lstrReferencePax.Add(RefAdt);
                }
                else if (Convert.ToInt16(dsReSchedule.Tables[0].Rows[0]["ChildCount"].ToString().Trim()) > 0)
                {
                    OnlineserviceRQRS.PaxTypeRefs RefChd = new OnlineserviceRQRS.PaxTypeRefs();
                    RefChd.Type = "CHD";
                    RefChd.Quantity = dsReSchedule.Tables[0].Rows[0]["ChildCount"].ToString().Trim();
                    lstrReferencePax.Add(RefChd);
                }
                else if (Convert.ToInt16(dsReSchedule.Tables[0].Rows[0]["InfantCount"].ToString().Trim()) > 0)
                {
                    OnlineserviceRQRS.PaxTypeRefs RefInf = new OnlineserviceRQRS.PaxTypeRefs();
                    RefInf.Type = "INF";
                    RefInf.Quantity = dsReSchedule.Tables[0].Rows[0]["InfantCount"].ToString().Trim();
                    lstrReferencePax.Add(RefInf);
                }
                lstrPax.PaxCount = (Convert.ToInt16(dsReSchedule.Tables[0].Rows[0]["AdultCount"].ToString().Trim()) + Convert.ToInt16(dsReSchedule.Tables[0].Rows[0]["ChildCount"].ToString().Trim()) + Convert.ToInt16(dsReSchedule.Tables[0].Rows[0]["InfantCount"].ToString().Trim())).ToString();
                lstrPax.PaxTypeRef = lstrReferencePax;

                #endregion

                #region FlightOption
                string[] lstrArra = new string[] { };
                #endregion

                if (Flightno != null && Flightno != "")
                {
                    string lstrflightOption = Flightno.Substring(0, 2);// string.Empty;
                    lstrArra = lstrflightOption.Split(',');
                }
                //if (dsReSchedule.Tables[0].Rows[0]["AIRCATEGORY"].ToString().ToUpper().Trim() == "FSC")
                //{
                //    Itnery.Stock = dsReSchedule.Tables[0].Rows[0]["CRS_ID"].ToString();
                //}
                //else
                //{
                //    Itnery.Stock = "RST_" + "1" + "~" + cate;
                //}
                Itnery.Currencycode = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();

                Itnery.Passenger = lstrPax;
                Itnery.FlightOption = string.IsNullOrEmpty(Flightno) ? null : lstrArra;
                Itnery.Category = dsReSchedule.Tables[0].Rows[0]["CRS_ID"].ToString();
                Itnery.PromoCodes = Prcode;
                Itnery.Agent = agentDet;
                Itnery.CRSPNR = dsReSchedule.Tables[0].Rows[0]["CRS_PNR"].ToString();
                Itnery.TicketNumberDetails = _tktno;
                #endregion

                #region send request
                dtMulticityAvail = new DataTable();

                triptype = dsReSchedule.Tables[0].Rows[0]["TRIP_DESC"].ToString();


                string request = JsonConvert.SerializeObject(Itnery).ToString();
                string Query = "InvokeRescheduleAvail";
                string LstrDetails = string.Empty;
                string TIMEOUTMINUTS = "4";//ConfigurationManager.AppSettings["Booking_Timeout"].ToString();
                int Availtimeout = Convert.ToInt32(TIMEOUTMINUTS) * 60 * 1000;

                string strURLpathARes = ConfigurationManager.AppSettings["APPS_URL"].ToString();

                /// Sending Request for avaliablity...
                 MyWebClient client = new MyWebClient();
                client.Headers["Content-type"] = "application/json";
                client.LintTimeout = Convert.ToInt32(TIMEOUTMINUTS) * 60 * 1000;

                string LstrDetailsreq = "<SELECT_REQUEST><URL>[<![CDATA[" + strURLpathARes + "]]>]</URL><QUERY>"
                    + Query + "</QUERY><TIMEOUT>" + (Availtimeout).ToString() + "</TIMEOUT><DATASET>" + strres + "</DATASET><JSON>" + request + "</JSON></SELECT_REQUEST>";
                DatabaseLog.LogData(strUserName, "T", "Rescheduleavail", "Reschedule Request", LstrDetailsreq, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), strsequnceID);

                byte[] data = client.UploadData(strURLpathARes + Query, "POST", Encoding.ASCII.GetBytes(request));
                string strResponse = Encoding.ASCII.GetString(data);

                string Responsedet = "<SELECT_REQUEST><URL>[<![CDATA[" + strURLpathARes + "]]>]</URL><QUERY>"
                    + Query + "</QUERY><TIMEOUT>" + (Availtimeout).ToString() + "</TIMEOUT><JSON>" + strResponse + "</JSON></SELECT_REQUEST>";
                DatabaseLog.LogData(strUserName, "T", "Rescheduleavail", "Reschedule-Avail Response", Responsedet, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), strsequnceID);

                DataSet dsRescheduleInfo = new DataSet();

                if (string.IsNullOrEmpty(strResponse))
                {
                    availabile[error] = "Problem occured while getting availability";
                }
                else
                {
                    DataSet dsAvailable = new DataSet();
                    dsAvailable = convertJsonStringToDataSet(strResponse, "");
                    OnlineserviceRQRS.RescheduleAvailRS _availRes = JsonConvert.DeserializeObject<OnlineserviceRQRS.RescheduleAvailRS>(strResponse);

                    //online reschedule request format

                    if (_availRes != null && _availRes.ResultCode == "1")
                    {
                        DataSet dsArilineNames = new DataSet();
                        string strPathcityall = Server.MapPath("~/XML/AirlineNames.xml");
                        dsArilineNames.ReadXml(strPathcityall);

                        string strjsonkeyval = string.Empty;
                        OnlineserviceRQRS.FlightDetailsShot op = new OnlineserviceRQRS.FlightDetailsShot();
                        var keyvalcombo = (from p in op.GetType().GetProperties().AsEnumerable()
                                           select new OnlineserviceRQRS.SearchModel.KeyValCombo
                                           {
                                               CD = p.GetCustomAttribute<DataMemberAttribute>().Name,
                                               FN = p.Name
                                           });
                        strjsonkeyval = JsonConvert.SerializeObject(keyvalcombo);
                        availabile[returnval] = strjsonkeyval;
                        List<List<OnlineserviceRQRS.SearchModel.FlightDetailsShot>> lstoflstOnwardAvail2;
                        int loopCnt = 1;
                        if (triptype == "Y" && AirportType == "LCC")
                        {
                            loopCnt = 2;
                        }

                        for (int i = 0; i < loopCnt; i++)
                        {
                            if (i == 1)
                            {
                                lstoflstOnwardAvail2 = new List<List<OnlineserviceRQRS.SearchModel.FlightDetailsShot>>();
                            }

                            var FDDnew = from _Flights in _availRes.ResFlights.AsEnumerable()

                                         join _Fares in _availRes.Fares.AsEnumerable()
                                         on _Flights.FareId equals _Fares.FlightId

                                         join _AName in dsArilineNames.Tables[0].AsEnumerable()
                                         on _Flights.PlatingCarrier equals _AName["_CODE"]

                                         where _Flights.FareId != "" && _Fares.FlightId != "" && (loopCnt > 1 ? _Flights.ItinRef == i.ToString() : _Flights.ItinRef != "")
                                          && _AName["AIRPORT_ID"].ToString() == AirportType

                                         select new OnlineserviceRQRS.SearchModel.FlightDetailsShot
                                         {
                                             FlightNumber = _Flights.FlightNumber,
                                             Origin = _Flights.Origin,
                                             // OriginName = _city["AN"].ToString(),
                                             Destination = _Flights.Destination,
                                             // DestinationName = _Dcity["AN"].ToString(),
                                             Depdate = DateTime.ParseExact(_Flights.DepartureDateTime.Split(' ')[0] + "/" + _Flights.DepartureDateTime.Split(' ')[1] + "/" + _Flights.DepartureDateTime.Split(' ')[2], @"dd\/MMM\/yyyy", null).ToString("dd/MM/yyyy"),
                                             Arrdate = DateTime.ParseExact(_Flights.ArrivalDateTime.Split(' ')[0] + "/" + _Flights.ArrivalDateTime.Split(' ')[1] + "/" + _Flights.ArrivalDateTime.Split(' ')[2], @"dd\/MMM\/yyyy", null).ToString("dd/MM/yyyy"),
                                             DepartureDate = _Flights.DepartureDateTime.Split(' ')[0] + " " + _Flights.DepartureDateTime.Split(' ')[1] + " " + _Flights.DepartureDateTime.Split(' ')[2],
                                             DepartureTime = _Flights.DepartureDateTime.Split(' ')[3],
                                             ArrivalDate = _Flights.ArrivalDateTime.Split(' ')[0] + " " + _Flights.ArrivalDateTime.Split(' ')[1] + " " + _Flights.ArrivalDateTime.Split(' ')[2],
                                             ArrivalTime = _Flights.ArrivalDateTime.Split(' ')[3],
                                             FlyingTime = _Flights.FlyingTime,
                                             Class = _Flights.Class + "-" + _Flights.FareBasisCode,
                                             ClassType = Cabin,
                                             AvailSeat = _Flights.AvailSeat,
                                             BaggageInfo = _Flights.SegmentDetails,
                                             CarrierCode = _Flights.PlatingCarrier,
                                             CNX = _Flights.CNX,
                                             ConnectionFlag = _Flights.ConnectionFlag,
                                             FareBasisCode = _Flights.FareBasisCode,
                                             FareId = _Flights.FareId,
                                             Faredescription = _Fares.Faredescription, //_Fares.Faredescription,
                                             PlatingCarrier = _Flights.PlatingCarrier,
                                             ReferenceToken = _Flights.ReferenceToken,
                                             SegRef = _Flights.SegRef,
                                             Stops = _Flights.Stops,
                                             via = _Flights.Via,
                                             Refund = _Flights.Refundable,
                                             Stock = loopCnt > 1 ? "RST_" + (i + 1) : _availRes.Stock.Split('~')[0],
                                             ItinRef = _Flights.ItinRef,
                                             BaseAmount = _Fares.Faredescription[0].BaseAmount,
                                             GrossAmount = _Fares.Faredescription[0].GrossAmount,//(Convert.ToInt64(_Fares.Faredescription[0].GrossAmount) + Convert.ToInt64(_Fares.Faredescription[0].ClientMarkup != "" ? _Fares.Faredescription[0].ClientMarkup : "0")).ToString(),
                                             TotalTaxAmount = _Fares.Faredescription[0].TotalTaxAmount,
                                             Commission = _Fares.Faredescription[0].Commission,
                                             Taxes = _Fares.Faredescription[0].Taxes,
                                             MultiClassAmount = _Fares.MultiClassAmount,
                                             MulticlassAirlinecategory = _Flights.AirlineCategory,
                                             Airlinecategory = _Flights.FlightNumber.Substring(0, 2).Trim() + _Fares.FareType.Trim(),
                                             ClassCarrierCode = _Flights.Class + "-" + _Flights.FareBasisCode,
                                             STK = _availRes.FareCheck.Stocktype,
                                             FareType = _Fares.FareType,
                                             StartTerminal = _Flights.StartTerminal,
                                             EndTerminal = _Flights.EndTerminal,
                                             JourneyTime = _Flights.JourneyTime,
                                             MulticlassAvail = _Flights.MultiClass,
                                             FareTypeDescription = _Flights.FareTypeDescription != null && _Flights.FareTypeDescription != "" ? _Flights.FareTypeDescription : "",
                                             AirlineName = _AName["_NAME"].ToString(),
                                             FARETYPETEXT = Faretypetext(_Fares.FareType, _Flights.FareTypeDescription),
                                             NETFare = (CovertToDouble(_Fares.Faredescription[0].GrossAmount)
                                                  + CovertToDouble((_Fares.Faredescription[0].ClientMarkup != null && _Fares.Faredescription[0].ClientMarkup != "") ? _Fares.Faredescription[0].ClientMarkup : "0")
                                                  + CovertToDouble((_Fares.Faredescription[0].ServiceFee != null && _Fares.Faredescription[0].ServiceFee != "") ? _Fares.Faredescription[0].ServiceFee : "0")
                                                  - CovertToDouble((Convert.ToDecimal(Convert.ToDecimal(_Fares.Faredescription[0].Discount ?? "0") +
                                                  Convert.ToDecimal(_Fares.Faredescription[0].PLB ?? "0") + Convert.ToDecimal(_Fares.Faredescription[0].Incentive ?? "0")).ToString()))).ToString(),
                                             //ServiceFee = _availRes.ServiceFee != null ? _availRes.ServiceFee : "",
                                             //             0                                   1                                       2                                           3                                           4                                          
                                             Inva = _Flights.Origin + "SpLitPResna" + _Flights.Destination + "SpLitPResna" + _Flights.DepartureDateTime + "SpLitPResna" + _Flights.ArrivalDateTime + "SpLitPResna" + _Flights.Class.Trim() + "SpLitPResna"
                                                       //                       5                                     6                                   7                                         8                                         9                              
                                                       + _Flights.PlatingCarrier + "SpLitPResna" + _Flights.RBDCode + "SpLitPResna" + _Flights.ReferenceToken + "SpLitPResna" + _Flights.ConnectionFlag + "SpLitPResna" + _Flights.AirlineCategory + "SpLitPResna"
                                                       //                      10                                      11                                      12                             13                                   14                                             15                                           16                             17                              18
                                                       + _Flights.FareBasisCode + "SpLitPResna" + _Flights.FlightNumber + "SpLitPResna" + _Flights.ItinRef + "SpLitPResna" + _Flights.CNX + "SpLitPResna" + _Flights.Cabin.TrimEnd(',') + "SpLitPResna" + _availRes.FareCheck.Stocktype + "SpLitPResna" + _Flights.FareId + "SpLitPResna" + _Flights.SegRef + "SpLitPResna" + _Fares.FareType,
                                         };

                            var jsonresults = FDDnew.GroupBy(s => s.FareId).Select(n => n.ToList()).ToList();
                            jsonresultsReschedule = JsonConvert.SerializeObject(jsonresults);
                            availabile[response] = jsonresultsReschedule;
                        }
                    }
                    else
                    {
                        availabile[error] = _availRes != null && _availRes.Error != null && _availRes.Error != "" ? _availRes.Error + " (01)" : "No Flights Available(03)";
                        return Json(new { Status = "", Message = "", Result = availabile });
                    }
                }
                #endregion
            }
            catch (WebException webEx)
            {
                DatabaseLog.LogData(strUserName, "X", "reschedule filight availability", "filight availability", webEx.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), strsequnceID);
                availabile[error] = "Problem occured while getting availability";
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(strUserName, "X", "reschedule filight availability", "reschedule filight availability", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), strsequnceID);
                availabile[error] = "Problem occured while getting availability";
            }
            return Json(new { Status = "", Message = "", Result = availabile });
        }

        #region Onlinereschedule
        public ActionResult Onlinereschedule(string Paxddetails, string Ticketdetails, string Remarks, string Contactdet, string farediff, string farechange,
            string RescheduleFlag, string SPNR, string AirlinePNR, string CRSPNR, string OnwardAvailjson, string ReturnAvailjson, string Flag, string strPaymentmode)
        {
            Rays_service.RaysService _rays_servers = new Rays_service.RaysService();;
            string strResult = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            int resultcode = 1;
            int responsemsg = 2;
            int responsedata = 3;
            bool result = false;
            DataSet dsViewPNR = new DataSet();
            DataSet dsDisplayDet = new DataSet();
            string refError = string.Empty;
            string refstrErrorMsg = string.Empty;
            string passtype = string.Empty;
            string strAgentID = Session["agentid"].ToString();
            string strTerminalId = Session["terminalid"].ToString();
            string strUserName = Session["username"].ToString();
            string sequnceID = Session["sequenceid"].ToString();
            string Ipaddress = Session["ipAddress"].ToString();
            string PosId = Session["POS_ID"].ToString();
            string Postid = Session["POS_TID"].ToString();
            string getAgentType = "";

            string strErrormsg = string.Empty;
            string segno = string.Empty;
            DataSet completedst = new DataSet();
            string updatestatus = string.Empty;
            bool onlineflg = false;
            DataSet pantyamt = new DataSet();
            string Strresponse = string.Empty;
            string checkflag = string.Empty;

            byte[] dsViewPNR_compress = new byte[] { };
            byte[] dsFareDispDet_compress = new byte[] { };
            string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"].ToString() != "") ? Session["TKTFLAG"].ToString() : "DTKT";
            string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");

            try
            {
                //#region UsageLog
                //string PageName = "Reschedule";
                //try
                //{
                //    string strUsgLogRes = Commonlog(PageName, "", "UPDATE");
                //}
                //catch (Exception e)
                //{

                //}
                //#endregion
                string XML = "<REQUEST><PAXDETAILS>" + Paxddetails + "</PAXDETAILS><TICKETDETAILS>" + Ticketdetails + "</TICKETDETAILS><REMARKS>" + Remarks + "</REMARKS><CONTACTDET>" + Contactdet + "</CONTACTDET><ALLOWFAREDIFF>" + farediff +
                             "</ALLOWFAREDIFF><ALLOWPENALTY>" + farechange + "</ALLOWPENALTY><TYPE>" + RescheduleFlag + "</TYPE><SPNR>" + SPNR + "</SPNR><AIRLINEPNR>" + AirlinePNR + "</AIRLINEPNR><CRSPNR>" + CRSPNR + "</CRSPNR><ONWARDAVAILJSON>" + OnwardAvailjson + "</ONWARDAVAILJSON><RETURNAVAILJSON>" + ReturnAvailjson + "</RETURNAVAILJSON><FLAG>" + Flag + "</FLAG></REQUEST>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "Reschedule online", "Reschedule online details", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                //_rays_servers.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                #region SERVICE URL BRANCH BASED -- STS195
                string strClientBranchID = Session["ClientBranchID"] != null && Session["ClientBranchID"].ToString() != "" ? Session["ClientBranchID"].ToString() : "";

                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                #endregion
                result = _rays_servers.Fetch_PNR_Verification_Details_NewByte(PosId, SPNR, AirlinePNR, CRSPNR, "", Postid, strUserName, "", "W", Convert.ToDecimal(sequnceID), ref dsViewPNR_compress, ref dsFareDispDet_compress, ref refError, ref refstrErrorMsg, "Reschedule online", "Reschedule online details", getAgentType, strTerminalId);

                //if ((strProduct == "RIYA" || strProduct == "RBOA") && result == false && string.IsNullOrEmpty(SPNR))
                //{
                //    _rays_servers.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
                //    result = _rays_servers.Fetch_PNR_Verification_Details_NewByte(PosId, SPNR, AirlinePNR, CRSPNR, "", Postid, strUserName, "", "W", Convert.ToDecimal(sequnceID), ref dsViewPNR_compress, ref dsFareDispDet_compress, ref refError, ref refstrErrorMsg, "Reschedule online", "Reschedule online details", getAgentType, strTerminalId);
                //}

                if (dsViewPNR_compress != null)
                {
                    dsViewPNR = Decompress(dsViewPNR_compress);
                }
                if (dsFareDispDet_compress != null)
                {
                    dsDisplayDet = Decompress(dsFareDispDet_compress);
                }


                if (dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                {
                    string StockType = Regex.Split(dsViewPNR.Tables[0].Rows[0]["StockType"].ToString(), "SPLIT")[0].ToString();
                    string Aircategory = dsViewPNR.Tables[0].Rows[0]["AIRLINE_CATEGORY"].ToString();
                    checkflag = (ConfigurationManager.AppSettings["Online_RescheduleAirline"].ToString().Contains(Aircategory) || ConfigurationManager.AppSettings["Online_RescheduleAirline"].ToString().Contains(StockType)) ? "ONLINE" : "OFFLINE";
                }


                if (result == true && dsViewPNR != null && dsViewPNR.Tables.Count > 0 && dsViewPNR.Tables[0].Rows.Count > 0)
                {
                    #region Create table detail

                    #region rescheduleticketdet
                    DataTable dtInsertTicketInfo = new DataTable();
                    dtInsertTicketInfo.Columns.Add("PNR");
                    dtInsertTicketInfo.Columns.Add("AGENTID");
                    dtInsertTicketInfo.Columns.Add("TERMINALID");
                    dtInsertTicketInfo.Columns.Add("FLIGHTNO");
                    dtInsertTicketInfo.Columns.Add("ORIGIN");
                    dtInsertTicketInfo.Columns.Add("DESTINATION");
                    dtInsertTicketInfo.Columns.Add("DEPARTURE_DATE");
                    dtInsertTicketInfo.Columns.Add("ARRIVAL_DATE");
                    dtInsertTicketInfo.Columns.Add("ADULT");
                    dtInsertTicketInfo.Columns.Add("CHILD");
                    dtInsertTicketInfo.Columns.Add("INFANT");
                    dtInsertTicketInfo.Columns.Add("USERNAME");
                    dtInsertTicketInfo.Columns.Add("AIRPORT_ID");
                    dtInsertTicketInfo.Columns.Add("AIRPNR");
                    dtInsertTicketInfo.Columns.Add("CRSPNR");
                    dtInsertTicketInfo.Columns.Add("SEGMENT_NO");
                    dtInsertTicketInfo.Columns.Add("EXIST_S_SPNR");
                    dtInsertTicketInfo.Columns.Add("BOOKINGTYPE");
                    dtInsertTicketInfo.Columns.Add("PAX_REF_NO");
                    dtInsertTicketInfo.Columns.Add("CLASS");
                    dtInsertTicketInfo.Columns.Add("FAREBASIS");
                    dtInsertTicketInfo.Columns.Add("CLIENTID");
                    dtInsertTicketInfo.Columns.Add("COORDINATORCODE");
                    dtInsertTicketInfo.Columns.Add("BOABRANCHID");
                    dtInsertTicketInfo.Columns.Add("BOAPOSID");
                    dtInsertTicketInfo.Columns.Add("BOATERMINALID");
                    dtInsertTicketInfo.Columns.Add("FORMOFPAYMENT");
                    dtInsertTicketInfo.Columns.Add("STOCKTYPE");
                    dtInsertTicketInfo.Columns.Add("EMPLOYEEID");
                    dtInsertTicketInfo.Columns.Add("ADDSECTOR");
                    #endregion

                    #region Reschedulepaxdetils

                    DataTable dtPassengerInfoDetails = new DataTable();
                    dtPassengerInfoDetails.Columns.Add("AGENTID");
                    dtPassengerInfoDetails.Columns.Add("Passengername");
                    dtPassengerInfoDetails.Columns.Add("PASSTYPE");
                    dtPassengerInfoDetails.Columns.Add("PNR");
                    dtPassengerInfoDetails.Columns.Add("AIRPNR");
                    dtPassengerInfoDetails.Columns.Add("CRSPNR");
                    dtPassengerInfoDetails.Columns.Add("TICKETNO");
                    dtPassengerInfoDetails.Columns.Add("SEGMENT_NO");
                    dtPassengerInfoDetails.Columns.Add("PAX_REF_NO");
                    dtPassengerInfoDetails.Columns.Add("SUPPILER_PENALITY");
                    dtPassengerInfoDetails.Columns.Add("AGENT_PENALITY");
                    dtPassengerInfoDetails.Columns.Add("FARE_DIFFERENCE");
                    dtPassengerInfoDetails.Columns.Add("FARE_ID");
                    dtPassengerInfoDetails.Columns.Add("BAGGAGE");
                    dtPassengerInfoDetails.Columns.Add("BAGGAGE_AMT");
                    dtPassengerInfoDetails.Columns.Add("MEALS");
                    dtPassengerInfoDetails.Columns.Add("MEALS_AMT");
                    dtPassengerInfoDetails.Columns.Add("SEAT");
                    dtPassengerInfoDetails.Columns.Add("SEAT_AMT");
                    dtPassengerInfoDetails.Columns.Add("WC");
                    dtPassengerInfoDetails.Columns.Add("WC_AMT");
                    dtPassengerInfoDetails.Columns.Add("OK");
                    dtPassengerInfoDetails.Columns.Add("OK_AMT");
                    dtPassengerInfoDetails.Columns.Add("MARKUP");
                    dtPassengerInfoDetails.Columns.Add("CLIENTPENALTY");
                    dtPassengerInfoDetails.Columns.Add("CURRENCY");
                    dtPassengerInfoDetails.Columns.Add("YQOLD");
                    dtPassengerInfoDetails.Columns.Add("YQ");
                    #endregion

                    #region Agentdetail
                    DataTable Dtagent = new DataTable();
                    Dtagent.TableName = "AgentDetails";
                    Dtagent.Columns.Add("AgentId");
                    Dtagent.Columns.Add("TerminalId");
                    Dtagent.Columns.Add("Version");
                    Dtagent.Columns.Add("AppType");
                    Dtagent.Columns.Add("UserName");
                    Dtagent.Columns.Add("Agenttype");
                    Dtagent.Columns.Add("BOAID");
                    Dtagent.Columns.Add("BOAterminalID");
                    Dtagent.Columns.Add("CoOrdinatorID");
                    Dtagent.Columns.Add("AirportID");
                    Dtagent.Columns.Add("BranchID");
                    Dtagent.Columns.Add("IssuingBranchID");
                    Dtagent.Columns.Add("ProjectID");
                    Dtagent.Columns.Add("ProductID");
                    Dtagent.Columns.Add("ClientID");
                    Dtagent.Columns.Add("Environment");
                    Dtagent.Columns.Add("Group_ID");
                    Dtagent.Columns.Add("APPCurrency");
                    Dtagent.Columns.Add("EMP_ID");
                    Dtagent.Columns.Add("Platform");
                    #endregion

                    #region Rescheduledetails
                    DataTable DtRecsheduledt = new DataTable();
                    DtRecsheduledt.TableName = "Rescheduledetails";
                    DtRecsheduledt.Columns.Add("Reschedt");
                    DtRecsheduledt.Columns.Add("Availjson");
                    #endregion

                    #region RescheduleInfo
                    DataTable dataTable = new DataTable();
                    DataRow drBook;
                    dataTable.Columns.Add("SEQNO");
                    dataTable.Columns.Add("PNR_NO");
                    dataTable.Columns.Add("PASS_TYPE");
                    dataTable.Columns.Add("TITLE");
                    dataTable.Columns.Add("FIRST_NAME");
                    dataTable.Columns.Add("LAST_NAME");
                    dataTable.Columns.Add("DATE");
                    dataTable.Columns.Add("CLASS");
                    dataTable.Columns.Add("FLIGHT_NO");
                    dataTable.Columns.Add("FARE_DIFF");
                    dataTable.Columns.Add("FARE_CHANGE");
                    dataTable.Columns.Add("CONTACT_NO");
                    dataTable.Columns.Add("ORIGIN");
                    dataTable.Columns.Add("DESTINATION");
                    dataTable.Columns.Add("EXIST_FLIGHT_NO");
                    dataTable.Columns.Add("TICKET_NO");
                    dataTable.Columns.Add("PAX_REF_NO");
                    dataTable.Columns.Add("SEGMENT_NO");
                    dataTable.Columns.Add("TRIP_TYPE");
                    dataTable.Columns.Add("PLATING_CARRIER");
                    dataTable.Columns.Add("BASE_ORIGIN");
                    dataTable.Columns.Add("BASE_DESTINATION");
                    dataTable.Columns.Add("BASE_DEPT_DATE");
                    dataTable.Columns.Add("BASE_ARRIVAL_DATE");
                    dataTable.Columns.Add("BASE_CLASS");
                    dataTable.Columns.Add("DATETEMP");
                    dataTable.Columns.Add("SPNR");
                    dataTable.Columns.Add("AIRPNR");
                    dataTable.Columns.Add("CRSPNR");
                    dataTable.Columns.Add("RESDATE");
                    dataTable.Columns.Add("RESARRDATE");
                    dataTable.Columns.Add("RESCLASS");
                    dataTable.Columns.Add("RESFLIGHTNO");
                    dataTable.Columns.Add("RDL_RESCHEDULE_MODE");
                    dataTable.Columns.Add("RDL_FARBASIS_ID");
                    dataTable.Columns.Add("ADDSECTOR");
                    #endregion

                    #region Newflightdetails

                    DataTable dtnewflight = new DataTable();
                    dtnewflight.Columns.Add("FLIGHT_NO");
                    dtnewflight.Columns.Add("ORIGINCODE");
                    dtnewflight.Columns.Add("DESTINATIONCODE");
                    dtnewflight.Columns.Add("DEPT_DATE");
                    dtnewflight.Columns.Add("ARRIVAL_DATE");
                    dtnewflight.Columns.Add("CLASS_ID");
                    dtnewflight.Columns.Add("CLASS_TYPE");
                    dtnewflight.Columns.Add("FARE_BASIS");
                    dtnewflight.Columns.Add("PLATING_CARRIER");
                    dtnewflight.Columns.Add("SPECIAL_TRIP");
                    dtnewflight.Columns.Add("REFERENCE_TOKEN");
                    dtnewflight.Columns.Add("SEGMENT_NO");
                    dtnewflight.Columns.Add("RescheduleGrossFare");

                    #endregion

                    #endregion

                    #region Inserttablevalues

                    Dtagent.Rows.Add(dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), dsViewPNR.Tables[0].Rows[0]["TERMINAL_ID"].ToString(), ConfigurationManager.AppSettings["APIVersion"].ToString(), "B2B",
                       dsViewPNR.Tables[0].Rows[0]["USER_NAME"].ToString(), "", "", "", "", dsViewPNR.Tables[0].Rows[0]["AIRPORTID"].ToString(),
                       dsViewPNR.Tables[0].Rows[0]["TAI_ISSUING_BRANCH_ID"].ToString(), dsViewPNR.Tables[0].Rows[0]["TAI_ISSUING_BRANCH_ID"].ToString(), Session["PRODUCT_CODE"].ToString(), Session["PRODUCT_CODE"].ToString(), dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), "",
                        "", "", "", "B");

                    Ticketdetails = Ticketdetails.TrimEnd('|');
                    string[] arrval = Ticketdetails.Split('|');

                    Paxddetails = Paxddetails.TrimEnd('|');
                    string[] arrval_pax = Paxddetails.Split('|');

                    if (arrval.Length > 0)
                    {
                        for (int j = 0; j < arrval_pax.Length; j++)
                        {
                            string[] paxdetsplit = arrval_pax[j].Split(',');
                            for (int i = 0; i < arrval.Length; i++)
                            {
                                string[] ticketdetsplit = arrval[i].Split(',');

                                var qry = from p in dsViewPNR.Tables[0].AsEnumerable()
                                          where p["PAX_REF_NO"].ToString() == paxdetsplit[7].Trim() && p["SEGMENT_NO"].ToString() == ticketdetsplit[8].Trim()
                                          select p;
                                DataView dv = qry.AsDataView();
                                DataTable dtData = new DataTable();
                                if (dv.Count > 0)
                                {
                                    dtData = qry.CopyToDataTable();
                                }

                                drBook = dataTable.NewRow();

                                passtype = paxdetsplit[2];//dtData.Rows[0]["PASSENGER_TYPE"].ToString();
                                if (passtype.ToUpper() == "ADULT" || passtype.ToUpper() == "ADT" || passtype.ToUpper() == "A")
                                {
                                    passtype = "A";
                                }
                                else if (passtype.ToUpper() == "CHILD" || passtype.ToUpper() == "CHD" || passtype.ToUpper() == "C")
                                {
                                    passtype = "C";
                                }
                                else
                                {
                                    passtype = "I";
                                }

                                drBook["SEQNO"] = "";
                                drBook["PNR_NO"] = dsViewPNR.Tables[0].Rows[0]["S_PNR"].ToString();
                                drBook["PASS_TYPE"] = passtype;
                                drBook["TITLE"] = paxdetsplit[3].Split(' ')[0];//dtData.Rows[0]["PASSENGER_TITLE"].ToString();
                                drBook["FIRST_NAME"] = paxdetsplit[3].Split(' ')[1];
                                drBook["LAST_NAME"] = paxdetsplit[3].Split(' ')[2];
                                drBook["DATE"] = converttime(ticketdetsplit[11].Trim().ToString());  //splitSubDatas[3].Trim().ToString();
                                drBook["CLASS"] = ticketdetsplit.Length > 6 ? ticketdetsplit[5].ToString() : "";
                                drBook["FLIGHT_NO"] = ticketdetsplit.Length > 7 ? ticketdetsplit[6].ToString() : "";
                                drBook["FARE_DIFF"] = farediff;
                                drBook["FARE_CHANGE"] = farechange;
                                drBook["CONTACT_NO"] = Contactdet;// dtData.Rows[0]["CONTACT_NO"].ToString();
                                drBook["ORIGIN"] = ticketdetsplit.Length > 1 ? ticketdetsplit[0].ToString() : "";
                                drBook["DESTINATION"] = ticketdetsplit.Length > 2 ? ticketdetsplit[1].ToString() : "";
                                drBook["EXIST_FLIGHT_NO"] = ticketdetsplit.Length > 11 ? ticketdetsplit[10].ToString() : "";
                                drBook["TICKET_NO"] = paxdetsplit[9];
                                drBook["PAX_REF_NO"] = paxdetsplit[7];
                                drBook["SEGMENT_NO"] = ticketdetsplit[8].ToString();

                                drBook["TRIP_TYPE"] = dsViewPNR.Tables[0].Rows[0]["TRIP_DESC"].ToString();
                                drBook["PLATING_CARRIER"] = dsViewPNR.Tables[0].Rows[0]["PLATING_CARRIER"].ToString();
                                drBook["BASE_ORIGIN"] = ticketdetsplit.Length > 1 ? ticketdetsplit[0].ToString() : "";
                                drBook["BASE_DESTINATION"] = ticketdetsplit.Length > 2 ? ticketdetsplit[1].ToString() : "";
                                drBook["BASE_DEPT_DATE"] = dtData != null && dtData.Rows.Count > 0 ? converttimewithAMPM(dtData.Rows[0]["DEPTDT"].ToString()) : converttimewithAMPM(ticketdetsplit[13].ToString());// converttimewithAMPM(ticketdetsplit[22].ToString()); //dtData.Rows[0]["DEPT_DATE"].ToString();
                                drBook["BASE_ARRIVAL_DATE"] = dtData != null && dtData.Rows.Count > 0 ? converttimewithAMPM(dtData.Rows[0]["ARRIVALDT"].ToString()) : converttimewithAMPM(ticketdetsplit[14].ToString());// converttimewithAMPM(ticketdetsplit[23].ToString()); //dtData.Rows[0]["ARRIVAL_DATE"].ToString();//converttime(dtData.Rows[0]["ARRIVAL_DATE"].ToString());
                                drBook["BASE_CLASS"] = dsViewPNR.Tables[0].Rows[0]["CLASS_ID"].ToString();
                                drBook["DATETEMP"] = converttime(ticketdetsplit[11].ToString());  //splitSubDatas[5].Trim().ToString();
                                drBook["SPNR"] = SPNR;
                                drBook["AIRPNR"] = AirlinePNR;
                                drBook["CRSPNR"] = CRSPNR;

                                drBook["RESDATE"] = ticketdetsplit[13].ToString();
                                drBook["RESARRDATE"] = ticketdetsplit[14].ToString();
                                drBook["RESCLASS"] = ticketdetsplit[12].ToString();
                                drBook["RESFLIGHTNO"] = ticketdetsplit[6].ToString();
                                drBook["RDL_RESCHEDULE_MODE"] = "O";
                                drBook["ADDSECTOR"] = ticketdetsplit[19].ToString();
                                dataTable.Rows.Add(drBook);

                                if (passtype.ToUpper() == "ADULT" || passtype.ToUpper() == "ADT" || passtype.ToUpper() == "A")
                                {
                                    passtype = "ADT";
                                }
                                else if (passtype.ToUpper() == "CHILD" || passtype.ToUpper() == "CHD" || passtype.ToUpper() == "C")
                                {
                                    passtype = "CHD";
                                }
                                else
                                {
                                    passtype = "INF";
                                }
                                if (i == 0)
                                {
                                    segno = ticketdetsplit[8].ToString();
                                    dtInsertTicketInfo.Rows.Add(SPNR, dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), dsViewPNR.Tables[0].Rows[0]["TERMINAL_ID"].ToString(), drBook["RESFLIGHTNO"], drBook["BASE_ORIGIN"], drBook["BASE_DESTINATION"], converttimewithAMPM(drBook["RESDATE"].ToString()) + " " + drBook["RESDATE"].ToString().Split(' ')[1],
                                                                converttimewithAMPM(drBook["RESARRDATE"].ToString()) + " " + drBook["RESARRDATE"].ToString().Split(' ')[1], dsViewPNR.Tables[0].Rows[0]["AdultCount"].ToString(), dsViewPNR.Tables[0].Rows[0]["ChildCount"].ToString(), dsViewPNR.Tables[0].Rows[0]["InfantCount"].ToString(),
                                                                dsViewPNR.Tables[0].Rows[0]["USER_NAME"].ToString(), dsViewPNR.Tables[0].Rows[0]["AIRPORTID"].ToString(), AirlinePNR, drBook["CRSPNR"], drBook["SEGMENT_NO"], SPNR, "", drBook["PAX_REF_NO"], drBook["CLASS"],
                                                                "", dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), "", dsViewPNR.Tables[0].Rows[0]["TAI_ISSUING_BRANCH_ID"].ToString(), "", "", "", dsViewPNR.Tables[0].Rows[0]["StockType"].ToString(), "", ticketdetsplit[19].ToString());
                                }
                                else if (segno != ticketdetsplit[8].ToString())
                                {
                                    segno = ticketdetsplit[8].ToString();
                                    dtInsertTicketInfo.Rows.Add(SPNR, dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), dsViewPNR.Tables[0].Rows[0]["TERMINAL_ID"].ToString(), drBook["RESFLIGHTNO"], drBook["BASE_ORIGIN"], drBook["BASE_DESTINATION"], converttimewithAMPM(drBook["RESDATE"].ToString()) + " " + drBook["RESDATE"].ToString().Split(' ')[1],
                                                                converttimewithAMPM(drBook["RESARRDATE"].ToString()) + " " + drBook["RESARRDATE"].ToString().Split(' ')[1], dsViewPNR.Tables[0].Rows[0]["AdultCount"].ToString(), dsViewPNR.Tables[0].Rows[0]["ChildCount"].ToString(), dsViewPNR.Tables[0].Rows[0]["InfantCount"].ToString(),
                                                                dsViewPNR.Tables[0].Rows[0]["USER_NAME"].ToString(), dsViewPNR.Tables[0].Rows[0]["AIRPORTID"].ToString(), AirlinePNR, drBook["CRSPNR"], drBook["SEGMENT_NO"], SPNR, "", drBook["PAX_REF_NO"], drBook["CLASS"],
                                                                "", dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), "", dsViewPNR.Tables[0].Rows[0]["TAI_ISSUING_BRANCH_ID"].ToString(), "", "", "", dsViewPNR.Tables[0].Rows[0]["StockType"].ToString(), "", ticketdetsplit[19].ToString());
                                }
                                if (dtData != null && dtData.Rows.Count > 0)
                                {
                                    dtPassengerInfoDetails.Rows.Add(dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), dtData.Rows[0]["FIRST_NAME"] + " " + dtData.Rows[0]["LAST_NAME"], passtype, drBook["SPNR"], drBook["AIRPNR"], drBook["CRSPNR"], drBook["TICKET_NO"], segno, drBook["PAX_REF_NO"], dtData.Rows[0]["SUPPILER_PENALITY"].ToString(),
                                                                    dtData.Rows[0]["AGENT_PENALITY"], dtData.Rows[0]["FARE_DIFFERENCE"], dtData.Rows[0]["FARE_ID"], dtData.Rows[0]["BAGGAGE"], "0", dtData.Rows[0]["MEALS"], "0", dtData.Rows[0]["SEAT"],
                                                                    "0", dtData.Rows[0]["WCR"], "0", dtData.Rows[0]["OKTBD"], "0", "0", dtData.Rows[0]["AGENT_PENALITY"], dtData.Rows[0]["Currency"], dtData.Rows[0]["YQ_FARE"], "0");

                                    // dataTable.Rows.Add(drBook);
                                }
                                else
                                {
                                    dtPassengerInfoDetails.Rows.Add(dsViewPNR.Tables[0].Rows[0]["AGENT_ID"].ToString(), drBook["FIRST_NAME"] + " " + drBook["LAST_NAME"], passtype, drBook["SPNR"], drBook["AIRPNR"], drBook["CRSPNR"], drBook["TICKET_NO"], segno, drBook["PAX_REF_NO"], "",
                                                                  "0", "0", "", "", "0", "", "0", "",
                                                                  "0", "0", "0", "0", "0", "0", "0", dsViewPNR.Tables[0].Rows[0]["Currency"], "0", "0");
                                }
                            }
                        }
                    }



                    #region Rescheduleflightdetails

                    int countno = 0;
                    int Segmentno = 0;
                    if (OnwardAvailjson != "")
                    {
                        List<RQRS_Onlinereschedule.FlightDetailsRES> _Flights = new List<RQRS_Onlinereschedule.FlightDetailsRES>();
                        _Flights = (List<RQRS_Onlinereschedule.FlightDetailsRES>)JsonConvert.DeserializeObject(OnwardAvailjson, typeof(List<RQRS_Onlinereschedule.FlightDetailsRES>));

                        for (int i = 0; i < _Flights.Count; i++)
                        {
                            Segmentno = i;
                            countno = i + 1;
                            int Totalamount = 0;
                            string Strpaxcount = dsViewPNR.Tables[0].Rows[0]["AdultCount"].ToString() + "|" + dsViewPNR.Tables[0].Rows[0]["ChildCount"].ToString() + "|" + dsViewPNR.Tables[0].Rows[0]["InfantCount"].ToString();
                            if (_Flights[i].Faredescription != null)
                            {
                                for (var N = 0; _Flights[i].Faredescription.Count > N; N++)
                                {
                                    Totalamount += ((Convert.ToInt32(_Flights[i].Faredescription[N].GrossAmount)) * (Convert.ToInt32(Strpaxcount.Split('|')[N].ToString())));
                                }
                            }
                            dtnewflight.Rows.Add(_Flights[Segmentno].FlightNumber.ToString(),
                                _Flights[Segmentno].Origin.ToString().Trim(), _Flights[Segmentno].Destination, _Flights[Segmentno].DepartureDate.ToString().Contains(":") ? _Flights[Segmentno].DepartureDate.ToString() : _Flights[Segmentno].DepartureDate.ToString() + " " + _Flights[Segmentno].DepartureTime.ToString(),

                                _Flights[Segmentno].ArrivalDate.ToString().Contains(":") ? _Flights[Segmentno].ArrivalDate.ToString() : _Flights[Segmentno].ArrivalDate.ToString() + " " + _Flights[Segmentno].ArrivalTime.ToString(),
                                 _Flights[Segmentno].Class.Split('-')[0].ToString(), _Flights[Segmentno].ClassType.ToString(), _Flights[Segmentno].Class.Split('-')[1].ToString(),
                                 _Flights[Segmentno].PlatingCarrier.ToString(), dsViewPNR.Tables[0].Rows[0]["TRIP_DESC"].ToString(), _Flights[Segmentno].ReferenceToken.ToString(), countno.ToString(), Totalamount.ToString());
                        }
                    }

                    if (ReturnAvailjson != "")
                    {
                        List<RQRS_Onlinereschedule.FlightDetailsRES> _Flights = new List<RQRS_Onlinereschedule.FlightDetailsRES>();
                        _Flights = (List<RQRS_Onlinereschedule.FlightDetailsRES>)JsonConvert.DeserializeObject(ReturnAvailjson, typeof(List<RQRS_Onlinereschedule.FlightDetailsRES>));

                        for (int i = 0; i < _Flights.Count; i++)
                        {
                            Segmentno = i;
                            countno = countno + 1;
                            int Totalamount = 0;
                            string Strpaxcount = dsViewPNR.Tables[0].Rows[0]["AdultCount"].ToString() + "|" + dsViewPNR.Tables[0].Rows[0]["ChildCount"].ToString() + "|" + dsViewPNR.Tables[0].Rows[0]["InfantCount"].ToString();
                            if (_Flights[i].Faredescription != null)
                            {
                                for (var N = 0; _Flights[i].Faredescription.Count > N; N++)
                                {
                                    Totalamount += ((Convert.ToInt32(_Flights[i].Faredescription[N].GrossAmount)) * (Convert.ToInt32(Strpaxcount.Split('|')[N].ToString())));
                                }
                            }
                            dtnewflight.Rows.Add(_Flights[Segmentno].FlightNumber.ToString(),
                                _Flights[Segmentno].Origin.ToString().Trim(), _Flights[Segmentno].Destination, _Flights[Segmentno].DepartureDate.ToString().Contains(":") ? _Flights[Segmentno].DepartureDate.ToString() : _Flights[Segmentno].DepartureDate.ToString() + " " + _Flights[Segmentno].DepartureTime.ToString(),

                                _Flights[Segmentno].ArrivalDate.ToString().Contains(":") ? _Flights[Segmentno].ArrivalDate.ToString() : _Flights[Segmentno].ArrivalDate.ToString() + " " + _Flights[Segmentno].ArrivalTime.ToString(),
                                 _Flights[Segmentno].Class.Split('-')[0].ToString(), _Flights[Segmentno].ClassType.ToString(), _Flights[Segmentno].Class.Split('-')[1].ToString(),
                                 _Flights[Segmentno].PlatingCarrier.ToString(), dsViewPNR.Tables[0].Rows[0]["TRIP_DESC"].ToString(), _Flights[Segmentno].ReferenceToken.ToString(), countno.ToString(), Totalamount.ToString());
                        }
                    }
                    #endregion

                    completedst.Tables.Add(dataTable);
                    completedst.Tables.Add(dtnewflight);
                    string strReschedule = JsonConvert.SerializeObject(completedst);

                    #endregion
                    onlineflg = (RescheduleFlag == "ONLINE" ? true : false);
                    onlineflg = false; // For temporary
                    if (onlineflg == false)
                    {
                        updatestatus = _rays_servers.Fetch_Insert_Reshedule_Info(completedst, Remarks, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), strUserName, Ipaddress, "W", Convert.ToDecimal(sequnceID), "S", ref strErrormsg, "OfflineReshedule", "OfflineReshedule", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), ref onlineflg, ref pantyamt, ref Strresponse);


                        Array_Book[error] = strErrormsg != null && strErrormsg != "" ? strErrormsg : "Unable to update Reschedule request (#03)";

                        return Json(new { Status = "", Message = "", Results = Array_Book });
                    }

                    if (Flag == "CHECKFARE")
                    {

                        updatestatus = _rays_servers.Fetch_Insert_Reshedule_Info(completedst, Remarks, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), strUserName, Ipaddress, "W", Convert.ToDecimal(sequnceID), "S", ref strErrormsg, "Onlinereschedule", "Onlinereschedule-FetchPenalty", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), ref onlineflg, ref pantyamt, ref Strresponse);

                        string XMLresponse = "<REQUEST><DATAS>" + completedst.GetXml() + "</DATAS><POSID>" + Session["POS_ID"].ToString() + "</POSID><POS_TID>" + Session["POS_TID"].ToString() + "</POS_TID><STRUSERNAME>" + strUserName + "</STRUSERNAME><IPADDRESS>" + Ipaddress +
                         "</IPADDRESS><FLAG>" + Flag + "</FLAG><REQTYPE>" + onlineflg + "</REQTYPE></REQUEST><RESPONSE><ERRORMSG>" + strErrormsg + "</ERRORMSG><PENALTY>" + pantyamt.GetXml() + "</PENALTY><STRRESPONSE>" + Strresponse + "</STRRESPONSE></RESPONSE>";

                        DatabaseLog.LogData(Session["username"].ToString(), "E", "Reschedule online", "Reschedule online details", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                        Session.Add("RESH_RQRS", Strresponse.ToString());
                        string panaltyamt = string.Empty;
                        if (updatestatus == "1")
                        {
                            if (pantyamt != null && pantyamt.Tables.Count > 0)
                            {
                                Array_Book[error] = strErrormsg != "" ? strErrormsg : "";
                                string resutls = JsonConvert.SerializeObject(pantyamt);
                                Array_Book[resultcode] = "1";
                                Array_Book[responsedata] = resutls;// (pantyamt.Tables[0].Rows[0]["Penalty"] != null ? pantyamt.Tables[0].Rows[0]["Penalty"] : "0") + "|" + (pantyamt.Tables[0].Rows[0]["FareDifference"] != null ? pantyamt.Tables[0].Rows[0]["FareDifference"].ToString() : "0");
                                Array_Book[responsemsg] = pantyamt.Tables[0].Columns.Contains("AlertMsg") ? pantyamt.Tables[0].Rows[0]["AlertMsg"].ToString() : pantyamt.Tables[1].Columns.Contains("AlertMsg") ? pantyamt.Tables[1].Rows[0]["AlertMsg"].ToString() : "";
                                Session.Add("CancelAmount", pantyamt.Tables[0].Rows[0]["Penalty"] != null ? pantyamt.Tables[0].Rows[0]["Penalty"] : "");
                                Session.Add("Faredifff", pantyamt.Tables[0].Rows[0]["FareDifference"] != null ? pantyamt.Tables[0].Rows[0]["FareDifference"].ToString() : "");
                                return Json(new { status = "", message = "", Results = Array_Book });
                            }
                            else
                            {
                                Array_Book[resultcode] = "0";
                                Array_Book[error] = strErrormsg != "" ? strErrormsg : "Unable to process online request" + Environment.NewLine + "Your request has been registered offline, please contact customer care";
                                return Json(new { status = "", message = "", Results = Array_Book });
                            }
                        }
                        else
                        {
                            Array_Book[resultcode] = "0";
                            Array_Book[error] = strErrormsg != "" ? strErrormsg : "Problem occured while update ToBeReschedule status.Please contact customer care(#03)";
                            return Json(new { status = "", message = "", Results = Array_Book });
                        }

                    }
                    else if (Flag == "CONFIRM_RESCHEDULE")
                    {
                        DataSet Paxdet = new DataSet();
                        dtPassengerInfoDetails.TableName = "Passenger";
                        Paxdet.Tables.Add(dtPassengerInfoDetails);
                        dtPassengerInfoDetails.AcceptChanges();

                        DataSet TKTdet = new DataSet();
                        TKTdet.Tables.Add(dtInsertTicketInfo);
                        dtInsertTicketInfo.AcceptChanges();
                        dtInsertTicketInfo.TableName = "Ticket";

                        string errorresult = string.Empty;
                        for (int i = 0; i < Paxdet.Tables.Count; i++)
                        {
                            Paxdet.Tables[0].Rows[i]["SUPPILER_PENALITY"] = Session["CancelAmount"] != null ? Session["CancelAmount"].ToString() : "0";
                            Paxdet.Tables[0].Rows[i]["FARE_DIFFERENCE"] = Session["Faredifff"] != null ? Session["Faredifff"].ToString() : "0";
                        }

                        string Errorconv = string.Empty;
                        DataSet dtRaysResult = new DataSet();
                        DataSet ssrlast = new DataSet();
                        string Ticketdet = Convert.ToBase64String(Compress(TKTdet));
                        string Passengerdet = Convert.ToBase64String(Compress(Paxdet));
                        byte[] resutlval = _rays_servers.GetReschdulepopup_BOA(completedst.Tables[0].Rows[0]["SPNR"].ToString(), "4", completedst.Tables[0].Rows[0]["ORIGIN"].ToString(), completedst.Tables[0].Rows[0]["DESTINATION"].ToString(), completedst.Tables[0].Rows[0]["RESFLIGHTNO"].ToString(), strUserName, Ipaddress, sequnceID);
                        dtRaysResult = Decompress(resutlval);

                        DataSet ssrdt = new DataSet();

                        DataTable SSRdet = dtRaysResult.Tables[4].Copy();
                        SSRdet.TableName = "SSR Details";
                        ssrdt.Tables.Add(SSRdet);
                        SSRdet.AcceptChanges();
                        string getres = Convert.ToBase64String(Compress(ssrdt));

                        DataSet Pnrinfo = new DataSet();
                        DataTable dtPnrinfo = new DataTable();
                        dtPnrinfo.Columns.Add("PNR");
                        dtPnrinfo.Columns.Add("PAXNO");
                        string paxrefno = string.Empty;
                        string resflightno = string.Empty;
                        string clssres = string.Empty;
                        string resdate = string.Empty;
                        string NewSpnr = string.Empty;

                        int totalcount = Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["AdultCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["ChildCount"]) + Convert.ToInt32(dsViewPNR.Tables[0].Rows[0]["InfantCount"]);

                        for (int i = 0; i < completedst.Tables[0].Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                NewSpnr = _rays_servers.GenerateSPNR();
                                paxrefno += completedst.Tables[0].Rows[i]["PAX_REF_NO"].ToString() + ",";
                            }
                            //else if (completedst.Tables[0].Rows[i]["RESFLIGHTNO"].ToString() != completedst.Tables[0].Rows[i - 1]["RESFLIGHTNO"].ToString() && completedst.Tables[0].Rows[i]["PAX_REF_NO"].ToString() != completedst.Tables[0].Rows[i - 1]["PAX_REF_NO"].ToString())
                            //{
                            //    string NewSpnrmultipax = _rays_servers.GenerateSPNR();
                            //    dtPnrinfo.Rows.Add(NewSpnrmultipax, completedst.Tables[0].Rows[i]["PAX_REF_NO"].ToString());
                            //}
                            else if (completedst.Tables[0].Rows[i]["PAX_REF_NO"].ToString() != completedst.Tables[0].Rows[i - 1]["PAX_REF_NO"].ToString())
                            {
                                paxrefno += completedst.Tables[0].Rows[i]["PAX_REF_NO"].ToString() + ",";
                            }
                        }
                        string[] paxary = paxrefno.Split(',');
                        dtPnrinfo.Rows.Add(NewSpnr, paxrefno.TrimEnd(','));
                        dtPnrinfo.TableName = "NewPNRS";
                        Pnrinfo.Tables.Add(dtPnrinfo);
                        Pnrinfo.AcceptChanges();

                        string Newpnrres = Convert.ToBase64String(Compress(Pnrinfo));

                        bool onlineflag = true;
                        string onliereq = Session["RESH_RQRS"] != null ? Session["RESH_RQRS"].ToString() : "";

                        string TicketResult = _rays_servers.Reschedule_TicketPassengerDetails_new_BOA(Ticketdet, Passengerdet, Newpnrres, strUserName, Ipaddress, sequnceID, "", "F", ref errorresult, "Onlinereschedule", "Rescheduleprocess-Confirm", getres, "", strTerminalId, onlineflag, onliereq);

                        string rescheduleres = "<REQUEST><DATAS>" + Pnrinfo.GetXml() + "</DATAS><TICKETDETAILS>" + TKTdet.GetXml() + "</TICKETDETAILS><PAXDETAILS>" + Paxdet.GetXml() + "</PAXDETAILS><SSRDETAILS>" + ssrdt.GetXml() + "</SSRDETAILS><USERNAME>" + strUserName + "</USERNAME><IPADDRESS>" + Ipaddress +
                        "</IPADDRESS><ONLIEREQUEST>" + onliereq + "</ONLIEREQUEST><REQTYPE>" + onlineflag + "</REQTYPE></REQUEST><RESPONSE><ERRORMSG>" + errorresult + "</ERRORMSG><RES>" + getres + "</RES><TicketResult>" + TicketResult + "</TicketResult></RESPONSE>";

                        DatabaseLog.LogData(Session["username"].ToString(), "E", "Reschedule online", "Reschedule online Confirm", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                        string pnrtxt = ConfigurationManager.AppSettings["TMC"].ToString() == "RIYA" ? "R PNR" : "S PNR";
                        if (TicketResult != "FAILED IN ONLINE")
                        {
                            Array_Book[resultcode] = "1";
                            Array_Book[error] = "Your ticket has been rescheduled successfully" + Environment.NewLine + "Your new " + pnrtxt + ":" + NewSpnr;
                            return Json(new { status = "", message = "", Results = Array_Book });
                        }
                        else
                        {
                            Array_Book[resultcode] = "0";
                            Array_Book[error] = errorresult != "" ? errorresult : "Unable to process your online request" + Environment.NewLine + "Your request has been registered offline, please contact customer care";
                            return Json(new { status = "", message = "", Results = Array_Book });
                        }
                    }
                    else if (Flag == "CANCELREQUEST")
                    {
                        string seqnores = Session["ReshSEQno"] != null ? Session["ReshSEQno"].ToString() : "";
                        DataSet paxdet = new DataSet();
                        string outputres = _rays_servers.Update_Reschedule_Status("cancelled", dsViewPNR.Tables[0].Rows[0]["ORIGIN_CODE"].ToString(), dsViewPNR.Tables[0].Rows[0]["DESTINATION_CODE"].ToString(), SPNR, seqnores, Remarks, paxdet, "", strUserName, Ipaddress, sequnceID, "OnlineReschedule", "Cancelpanaltyamount");

                        string rescheduleres = "<CANCELREQUEST><SEQNO>" + seqnores + "</SEQNO><ORIGIN>" + dsViewPNR.Tables[0].Rows[0]["ORIGIN_CODE"].ToString() + "</ORIGIN><DESTINATION>" + dsViewPNR.Tables[0].Rows[0]["DESTINATION_CODE"].ToString() + "</DESTINATION><SPNR>" + SPNR + "</SPNR><PAXDETAILS>" + paxdet.GetXml()
                                                + "</PAXDETAILS><USERNAME>" + strUserName + "</USERNAME><IPADDRESS>" + Ipaddress + "</IPADDRESS><OUTPUTRESPONSE>" + outputres + "</OUTPUTRESPONSE></CANCELREQUEST>";
                        DatabaseLog.LogData(Session["username"].ToString(), "E", "Reschedule online", "Reschedule online cancelrequest", rescheduleres.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                        Array_Book[resultcode] = "1";
                        Array_Book[error] = "Your reshedule request cancelled successfully";
                        return Json(new { status = "", message = "", Results = Array_Book });
                    }
                }

            }
            catch (Exception ex)
            {
                Array_Book[error] = "Problem occured in Reschedule request ";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Availpage", "btnConfirmReSchedule_Click", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }

            return Json(new { Status = "", Message = "", Results = Array_Book });
        }

        public ActionResult Reschedulefarerule(string Availjson)
        {
            ArrayList available = new ArrayList();
            available.Add("");
            available.Add("");
            available.Add("");
            int response = 1;
            int error = 0;
            int resultval = 2;
            try
            {
                RQRS_ancillary.FareRuleRQ _FareRuleRQ = new RQRS_ancillary.FareRuleRQ();
                RQRS_ancillary.AgentDetails Agent = new RQRS_ancillary.AgentDetails();
                RQRS_ancillary.RQFlights _Flts = new RQRS_ancillary.RQFlights();
                List<RQRS_ancillary.RQFlights> _lstFlts = new List<RQRS_ancillary.RQFlights>();
                RQRS_ancillary.FareRuleRS _FareRuleRS = new RQRS_ancillary.FareRuleRS();

                Agent.AgentID = Session["agentid"].ToString();
                Agent.AgentType = Session["agenttype"] != null && Session["agenttype"] != "" ? "" : "";
                Agent.Airportid = "I";
                Agent.AppType = "B2B";
                Agent.BOAID = Session["POS_ID"].ToString();
                Agent.BOATreminalID = Session["POS_TID"].ToString();
                Agent.BranchID = Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                Agent.ClientID = Session["POS_TID"].ToString();
                Agent.CoOrdinatorID = "";
                Agent.Environment = "W";
                Agent.TerminalID = Session["terminalid"].ToString();
                Agent.UserName = Session["username"].ToString();
                Agent.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                Agent.ProjectId = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"] != "" ? Session["PRODUCT_CODE"].ToString() : "";
                Agent.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["CurrencyFlag"].ToString();
                Agent.Platform = "B";
                Agent.ProductID = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"] != "" ? Session["PRODUCT_CODE"].ToString() : "";

                string Stock = string.Empty;
                string BaseOrg = string.Empty;
                string Basedes = string.Empty;
                string Cabin = string.Empty;

                string[] SplitAirCategory = Regex.Split(Availjson, "SpLitPResna");

                _Flts = new RQRS_ancillary.RQFlights();
                _Flts.AirlineCategory = SplitAirCategory[9];
                _Flts.Origin = SplitAirCategory[0].Trim();
                _Flts.Destination = SplitAirCategory[1].Trim();
                _Flts.DepartureDateTime = SplitAirCategory[2];
                _Flts.ReferenceToken = SplitAirCategory[7];
                _Flts.FlightNumber = SplitAirCategory[11];
                _Flts.PlatingCarrier = SplitAirCategory[5];
                _Flts.ArrivalDateTime = SplitAirCategory[3];
                _Flts.Class = SplitAirCategory[4];
                _Flts.CarrierCode = SplitAirCategory[5];
                _Flts.Cabin = Cabin;
                _Flts.FareType = "";
                _Flts.SeatAvailFlag = "";
                _Flts.PromoCode = "";
                _Flts.StartTerminal = "";
                _Flts.EndTerminal = "";
                //_Flts.CNX = SplitAirCategory[20];
                //_Flts.ConnectionFlag = SplitAirCategory[15];
                _Flts.FareBasisCode = SplitAirCategory[10];
                _Flts.FareID = SplitAirCategory[16];
                _Flts.ItinRef = SplitAirCategory[12];
                _Flts.SegRef = SplitAirCategory[17];

                Stock = SplitAirCategory[15];
                BaseOrg = SplitAirCategory[0];
                Basedes = SplitAirCategory[1];
                _lstFlts.Add(_Flts);

                string Triptypeflag = "O";
                RQRS_ancillary.Segment Segment = new RQRS_ancillary.Segment();
                Segment.BaseOrigin = BaseOrg.Trim();
                Segment.BaseDestination = Basedes.Trim();
                Segment.Adult = 1;
                Segment.Child = 0;
                Segment.Infant = 0;
                Segment.SegmentType = "I";
                Segment.TripType = Triptypeflag;

                _FareRuleRQ.FlightsDetails = _lstFlts;
                _FareRuleRQ.SegmentDetails = Segment;
                _FareRuleRQ.Stock = Stock;
                _FareRuleRQ.AirlinePNR = "";
                _FareRuleRQ.CRSPNR = "";
                _FareRuleRQ.FareType = "N";
                _FareRuleRQ.FetchType = "";
                _FareRuleRQ.TicketNo = "";
                _FareRuleRQ.CRSID = "";

                _FareRuleRQ.AgentDetail = Agent;

                available[resultval] = SplitAirCategory[9];

                string request = Newtonsoft.Json.JsonConvert.SerializeObject(_FareRuleRQ).ToString();
                string Query = "GetFareRule";
                 MyWebClient client = new MyWebClient();
                string TIMEOUTMINUTS = ConfigurationManager.AppSettings["Booking_Timeout"].ToString();
                client.LintTimeout = Convert.ToInt32(TIMEOUTMINUTS) * 60 * 1000;
                client.Headers["Content-type"] = "application/json";
                string strurl = ConfigurationManager.AppSettings["FareRule"].ToString();

                StringWriter strWriter = new StringWriter();
                DataSet dsrequest = new DataSet();
                dsrequest = convertJsonStringToDataSet(request, "");
                dsrequest.WriteXml(strWriter);

                string Reqtime = "FareruleRequest" + DateTime.Now;

                string Reqdetails = "<GET_FARE_RULE_REQUEST><URL>[<![CDATA[" + strurl + "]]>]</URL><QUERY>" + Query + "</QUERY><REQTIME>" + Reqtime + "</REQTIME><JSONREQUEST>[<![CDATA[" + request + "]]>]</JSONREQUEST>" + strWriter.ToString() + "</GET_FARE_RULE_REQUEST>";
                DatabaseLog.LogData(Session["username"].ToString(), "F", "Reschedule-Flightrule", "GetFlighFareRule-Request", Reqdetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                byte[] data = client.UploadData(strurl + Query, "POST", Encoding.ASCII.GetBytes(request));
                string strResponse = System.Text.Encoding.ASCII.GetString(data);

                string ResTime = "FareruleResponse" + DateTime.Now;
                StringWriter strWriterres = new StringWriter();
                DataSet dsresponse = new DataSet();
                dsresponse = convertJsonStringToDataSet(strResponse, "");
                dsresponse.WriteXml(strWriterres);
                string Resdetails = "<GET_FARE_RULE_RESPONSE><URL>[<![CDATA[" + strurl + "]]>]</URL><QUERY>" + Query + "</QUERY><RESTIME>" + ResTime + "</RESTIME><JSONRESPONSE>[<![CDATA[" + strResponse + "]]>]</JSONRESPONSE>" + strWriterres.ToString() + "</GET_FARE_RULE_RESPONSE>";
                DatabaseLog.LogData(Session["username"].ToString(), "F", "Reschedule-Flightrule", "GetFlighFareRule-Response", Resdetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                if (string.IsNullOrEmpty(strResponse))
                {
                    available[error] = "Unable to get fare rule details";
                }
                else
                {
                    _FareRuleRS = JsonConvert.DeserializeObject<RQRS_ancillary.FareRuleRS>(strResponse);
                    if (_FareRuleRS.Status.ResultCode == "1")
                    {
                        available[response] = _FareRuleRS.FareRule.FareRuleText != null && _FareRuleRS.FareRule.FareRuleText != "" ? _FareRuleRS.FareRule.FareRuleText : "Unable to get fare rule details";
                    }
                    else
                    {
                        available[error] = _FareRuleRS.Status.Error != null && _FareRuleRS.Status.Error != "" ? _FareRuleRS.Status.Error : "Unable to get fare rule details";
                    }
                }

            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "reschedule farerule", "reschedule farerule", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                available[error] = "Problem occured while getting farerule";
            }
            return Json(new { Status = "", Message = "", Result = available });
        }




        #region ConvertTime with AM and PM
        public string converttimewithAMPM(string datetime)
        {

            string finaltime = string.Empty;
            string finaldatetime = string.Empty;
            string strArrDate = string.Empty;
            try
            {
                string[] splitdate = datetime.Split(' ');
                string dateonly = string.Empty;
                dateonly = splitdate.Length > 0 ? splitdate[0] : datetime;

                strArrDate = DateTime.ParseExact(dateonly, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                strArrDate = strArrDate + " " + splitdate[1] + " " + splitdate[2];
                //string[] date = strArrDate.Split(' ');
                //string[] time = date[1].Split(':');
                //finaltime = time[0] + ':' + time[1];
                //finaldatetime = date[0] + ' ' + finaltime;

            }
            catch (Exception ex)
            {

            }
            return strArrDate;

        }
        #endregion


        #endregion

        #region Offlinereshedule

        public string Offlinereschedulerequest(string Paxddetails, string Ticketdetails, string Remarks, string Contactdet, string farediff, string farechange, string SPNR, string AirlinePNR, string CRSPNR, DataSet dsViewPNR, string strPaymentmode)
        {
            Rays_service.RaysService _rays_servers = new Rays_service.RaysService();;
            string strResult = string.Empty;
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            string returnmessage = string.Empty;
            DataSet dsDisplayDet = new DataSet();
            string refError = string.Empty;
            string refstrErrorMsg = string.Empty;
            try
            {
                //#region UsageLog
                //string PageName = "Reschedule";
                //try
                //{
                //    string strUsgLogRes = Commonlog(PageName, "", "UPDATE");
                //}
                //catch (Exception e)
                //{

                //}
                //#endregion
                string strAgentID = Session["agentid"].ToString();
                string strTerminalId = Session["terminalid"].ToString();
                string strUserName = Session["username"].ToString();
                string sequnceID = Session["sequenceid"].ToString();
                string Ipaddress = Session["ipAddress"].ToString();
                string PosId = Session["POS_ID"].ToString();
                string Postid = Session["POS_TID"].ToString();
                string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"].ToString() != "") ? Session["TKTFLAG"].ToString() : "DTKT";
                string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");
                DataSet dsSequenceNo = new DataSet();
                DataTable dataTable = new DataTable();
                DataRow drBook;
                dataTable.Columns.Add("SEQNO");
                dataTable.Columns.Add("PNR_NO");
                dataTable.Columns.Add("PASS_TYPE");
                dataTable.Columns.Add("TITLE");
                dataTable.Columns.Add("FIRST_NAME");
                dataTable.Columns.Add("LAST_NAME");
                dataTable.Columns.Add("DATE");
                dataTable.Columns.Add("CLASS");
                dataTable.Columns.Add("FLIGHT_NO");
                dataTable.Columns.Add("FARE_DIFF");
                dataTable.Columns.Add("FARE_CHANGE");
                dataTable.Columns.Add("CONTACT_NO");
                dataTable.Columns.Add("ORIGIN");
                dataTable.Columns.Add("DESTINATION");
                dataTable.Columns.Add("EXIST_FLIGHT_NO");
                dataTable.Columns.Add("TICKET_NO");
                dataTable.Columns.Add("PAX_REF_NO");
                dataTable.Columns.Add("SEGMENT_NO");

                dataTable.Columns.Add("TRIP_TYPE");
                dataTable.Columns.Add("PLATING_CARRIER");
                dataTable.Columns.Add("BASE_ORIGIN");
                dataTable.Columns.Add("BASE_DESTINATION");
                dataTable.Columns.Add("BASE_DEPT_DATE");
                dataTable.Columns.Add("BASE_ARRIVAL_DATE");
                dataTable.Columns.Add("BASE_CLASS");

                dataTable.Columns.Add("DATETEMP");
                dataTable.Columns.Add("SPNR");
                dataTable.Columns.Add("AIRPNR");
                dataTable.Columns.Add("CRSPNR");

                dataTable.Columns.Add("RESDATE");
                dataTable.Columns.Add("RESARRDATE");
                dataTable.Columns.Add("RESCLASS");
                dataTable.Columns.Add("RESFLIGHTNO");
                dataTable.Columns.Add("RDL_RESCHEDULE_MODE");
                dataTable.Columns.Add("RDL_FARBASIS_ID");

                Ticketdetails = Ticketdetails.TrimEnd('|');
                string[] arrval = Ticketdetails.Split('|');

                Paxddetails = Paxddetails.TrimEnd('|');
                string[] arrval_pax = Paxddetails.Split('|');

                if (arrval.Length > 0)
                {
                    for (int j = 0; j < arrval_pax.Length; j++)
                    {
                        string[] paxdetsplit = arrval_pax[j].Split(',');
                        for (int i = 0; i < arrval.Length; i++)
                        {
                            string[] ticketdetsplit = arrval[i].Split(',');

                            var qry = from p in dsViewPNR.Tables[0].AsEnumerable()
                                      where p["PAX_REF_NO"].ToString() == paxdetsplit[7].Trim() && p["SEGMENT_NO"].ToString() == ticketdetsplit[8].Trim()
                                      select p;
                            DataView dv = qry.AsDataView();
                            DataTable dtData = new DataTable();
                            if (dv.Count > 0)
                            {
                                dtData = qry.CopyToDataTable();
                            }

                            drBook = dataTable.NewRow();

                            string passtype = dtData.Rows[0]["PASSENGER_TYPE"].ToString();
                            if (passtype.ToUpper() == "ADULT" || passtype.ToUpper() == "ADT" || passtype.ToUpper() == "A")
                            {
                                passtype = "A";
                            }
                            else if (passtype.ToUpper() == "CHILD" || passtype.ToUpper() == "CHD" || passtype.ToUpper() == "C")
                            {
                                passtype = "C";
                            }
                            else
                            {
                                passtype = "I";
                            }

                            drBook["SEQNO"] = "";
                            drBook["PNR_NO"] = dtData.Rows[0]["S_PNR"].ToString();
                            drBook["PASS_TYPE"] = passtype;
                            drBook["TITLE"] = dtData.Rows[0]["PASSENGER_TITLE"].ToString();
                            drBook["FIRST_NAME"] = dtData.Rows[0]["FIRST_NAME"].ToString();
                            drBook["LAST_NAME"] = dtData.Rows[0]["LAST_NAME"].ToString();
                            drBook["DATE"] = converttime(ticketdetsplit[11].Trim().ToString());  //splitSubDatas[3].Trim().ToString();
                            drBook["CLASS"] = ticketdetsplit.Length > 6 ? ticketdetsplit[5].ToString() : "";
                            drBook["FLIGHT_NO"] = ticketdetsplit.Length > 7 ? ticketdetsplit[6].ToString() : "";
                            drBook["FARE_DIFF"] = farediff;
                            drBook["FARE_CHANGE"] = farechange;
                            drBook["CONTACT_NO"] = Contactdet;// dtData.Rows[0]["CONTACT_NO"].ToString();
                            drBook["ORIGIN"] = ticketdetsplit.Length > 1 ? ticketdetsplit[0].ToString() : "";
                            drBook["DESTINATION"] = ticketdetsplit.Length > 2 ? ticketdetsplit[1].ToString() : "";
                            drBook["EXIST_FLIGHT_NO"] = ticketdetsplit.Length > 11 ? ticketdetsplit[10].ToString() : "";
                            drBook["TICKET_NO"] = dtData.Rows[0]["TICKET_NO"].ToString();
                            drBook["PAX_REF_NO"] = dtData.Rows[0]["PAX_REF_NO"].ToString();
                            drBook["SEGMENT_NO"] = dtData.Rows[0]["SEGMENT_NO"].ToString();

                            drBook["TRIP_TYPE"] = dtData.Rows[0]["TRIP_DESC"].ToString();
                            drBook["PLATING_CARRIER"] = dtData.Rows[0]["PLATING_CARRIER"].ToString();
                            drBook["BASE_ORIGIN"] = dtData.Rows[0]["ORIGIN_CODE"].ToString().Trim();
                            drBook["BASE_DESTINATION"] = dtData.Rows[0]["DESTINATION_CODE"].ToString().Trim();
                            drBook["BASE_DEPT_DATE"] = converttimewithAMPM(dtData.Rows[0]["DEPTDT"].ToString());// converttimewithAMPM(ticketdetsplit[22].ToString()); //dtData.Rows[0]["DEPT_DATE"].ToString();
                            drBook["BASE_ARRIVAL_DATE"] = converttimewithAMPM(dtData.Rows[0]["ARRIVALDT"].ToString());// converttimewithAMPM(ticketdetsplit[23].ToString()); //dtData.Rows[0]["ARRIVAL_DATE"].ToString();//converttime(dtData.Rows[0]["ARRIVAL_DATE"].ToString());
                            drBook["BASE_CLASS"] = dtData.Rows[0]["CLASS_ID"].ToString();
                            drBook["DATETEMP"] = converttime(ticketdetsplit[11].ToString());  //splitSubDatas[5].Trim().ToString();
                            drBook["SPNR"] = SPNR;
                            drBook["AIRPNR"] = AirlinePNR;
                            drBook["CRSPNR"] = CRSPNR;

                            drBook["RESDATE"] = (ticketdetsplit[13] != null && ticketdetsplit[13] != "" && ticketdetsplit[13] != "undefined") ? ticketdetsplit[13].ToString() : ticketdetsplit[4].ToString() + " 00:00";
                            drBook["RESARRDATE"] = (ticketdetsplit[14] != null && ticketdetsplit[14] != "" && ticketdetsplit[14] != "undefined") ? ticketdetsplit[14].ToString() : ticketdetsplit[4].ToString() + " 00:00";
                            drBook["RESCLASS"] = ticketdetsplit[12].ToString();
                            drBook["RESFLIGHTNO"] = ticketdetsplit[6].ToString();
                            drBook["RDL_RESCHEDULE_MODE"] = "";
                            drBook["RDL_FARBASIS_ID"] = "";
                            dataTable.Rows.Add(drBook);
                        }
                    }
                }

                string strSeqNo = string.Empty;
                string output = string.Empty;
                string strErrorMsg = string.Empty;
                dsSequenceNo.Tables.Add(dataTable);
                strErrorMsg = string.Empty;

                bool offlinereq = false;        //** for offline request send as false **//
                DataSet dst = new DataSet();
                string responsestrng = string.Empty;
                #region SERVICE URL BRANCH BASED -- STS195
                string strClientBranchID = Session["ClientBranchID"] != null && Session["ClientBranchID"].ToString() != "" ? Session["ClientBranchID"].ToString() : "";
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (strClientBranchID != "" && strBranchCredit.Contains(strClientBranchID)))
                    {
                        _rays_servers.Url = ConfigurationManager.AppSettings["TOPUP_APPS_RAYS_SERVICE"].ToString();
                    }
                    else
                    {
                        _rays_servers.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                    }
                }
                else
                {
                    _rays_servers.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                }
                #endregion
                //_rays_servers.Url = strTKTFLAG == "QTKT" ? ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString() : ConfigurationManager.AppSettings["ServiceURI"].ToString();
                output = _rays_servers.Fetch_Insert_Reshedule_Info(dsSequenceNo, Remarks, PosId, Postid, strUserName, Ipaddress,
                                        "W", Convert.ToDecimal(sequnceID), "S", ref strErrorMsg, "PNRVerification", "btnConfirmReSchedule_Click", "", "", ref offlinereq, ref dst, ref responsestrng);

                #region Log
                StringWriter strWriter = new StringWriter();

                dsSequenceNo.WriteXml(strWriter);
                string LstrDetails = "<OFFLINERESCHEDULE><DETAILS>" + strWriter.ToString() + "</DETAILS><OUTPUT>" + output.ToString() +
                   "</OUTPUT><POSID></POSID><POSTID></POSTID><USERNAME>" + strUserName + "</USERNAME><IPADDRESS>" + Ipaddress + "</IPADDRESS><REFERROR>" + strErrorMsg
                   + "</REFERROR><OFFLINEREQUEST>" + offlinereq + "</OFFLINEREQUEST><DATAS>" + dst.GetXml() + "</DATAS><RESPONSESTRING>" + responsestrng + "</RESPONSESTRING></OFFLINERESCHEDULE>";
                DatabaseLog.LogData(Session["username"].ToString(), "T", "Offlinereschedule", "Offlinereschedulerequest", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                #endregion

                if (output == "1")
                {
                    returnmessage = strErrorMsg; //"Your Reschedule request has been completed Sucessfully" + Environment.NewLine + "Your reschedule request id was" + (strErrorMsg != null && strErrorMsg != "" ? strErrorMsg : "");
                }
                else if (output == "0")
                {
                    returnmessage = "Unable to update Reschedule request (#01)";
                }
            }
            catch (Exception ex)
            {
                returnmessage = "Problem occured in Reschedule request (#03)";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Availpage", "btnConfirmReSchedule_Click", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }

            return returnmessage;
        }

        #endregion


     

        private string converttime(string datetime)
        {

            string finaltime = string.Empty;
            string finaldatetime = string.Empty;
            string strArrDate = string.Empty;
            try
            {
                strArrDate = DateTime.ParseExact(datetime, "dd MMM yyyy HH:mm", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm");

                string[] date = strArrDate.Split(' ');
                string[] time = date[1].Split(':');
                finaltime = time[0] + ':' + time[1];
                finaldatetime = date[0] + ' ' + finaltime;

            }
            catch (Exception ex)
            {

            }
            return finaldatetime;

        }
        #endregion

        public ActionResult GetReschdulepopup_BOA(string Strspnr, string Orgcode, string Descode, string Rsdlno)
        {
            string strStatus = string.Empty;
            string strMsg = string.Empty;
            string strResult = string.Empty;
            string refError = string.Empty;
            string strErrorMsg = string.Empty;
            string xmlResData = string.Empty;
            string strFaretype = string.Empty;
            string strpnr = string.Empty;
            byte[] Getdetails = new byte[] { };
            byte[] dsDet_res = new byte[] { };
            DataSet dsViewPNR = new DataSet();
            DataSet dsDetails = new DataSet();
            string strAgentID = Session["AGENTID"] != null ? Session["AGENTID"].ToString() : "";

            string strTerminalID = Session["TERMINALID"] != null ? Session["TERMINALID"].ToString() : "";
            string strUserName = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "";
            string strIPAddress = Session["IPADDRESS"] != null ? Session["IPADDRESS"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null ? Session["TERMINALTYPE"].ToString() : "";
            string strAgentType = Session["CLIENTTYPE"] != null ? Session["CLIENTTYPE"].ToString() : "";
            string strBranchId = Session["BRANCHID"] != null ? Session["BRANCHID"].ToString() : "";
            string strSeqId = Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : DateTime.Now.ToString("ddMMyyyy");
            string strPosId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPosTId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strPlatform = Session["PLATFORM"] != null ? Session["PLATFORM"].ToString() : "";
            string strRefToken = string.Empty;
            string strOfficeID = string.Empty;

            try
            {
                Rays_service.RaysService _rays_servers = new Rays_service.RaysService();;

                _rays_servers.Url = ConfigurationManager.AppSettings["serviceuri"].ToString();
                string Logxml = "<EVENT><REQ><Strspnr>" + Strspnr + "</Strspnr><FLAG>4</FLAG><Orgcode>" + Orgcode + "</Orgcode><Descode>" + Descode + "</Descode><Rsdlno>" + Rsdlno + "</Rsdlno><strUserName></strUserName></REQ>";
                Getdetails = _rays_servers.GetReschdulepopup_BOA(Strspnr, "4", Orgcode, Descode, Rsdlno.Trim(), strUserName, strIPAddress, strSeqId);
                dsDetails = Decompress(Getdetails);
                Session["Olddata_" + Strspnr] = dsDetails;
                Logxml += "<RES>" + JsonConvert.SerializeObject(dsDetails).ToString() + "</RES></EVENT>";
                if (dsDetails.Tables.Count > 0 && dsDetails.Tables[0].Rows.Count > 0)
                {
                    strResult = JsonConvert.SerializeObject(dsDetails).ToString();
                    strStatus = "03";
                }
                DatabaseLog.LogData(strUserName, "E", "ActionController", "WEB_TOBE_RESHEDULE_STEP_2", Logxml.ToString(), strPosId, strPosTId, strSeqId);
            }
            catch (Exception ec)
            {
                DatabaseLog.LogData(strUserName, "X", "ActionController", "WEB_TOBE_RESHEDULE_STEP_2-EXP", ec.ToString(), strPosId, strPosTId, strSeqId);
                strStatus = "05";
                strErrorMsg = "No data found(05)";
            }

            return Json(new { Status = strStatus, Message = strErrorMsg, Result = strResult });
        }

        public ActionResult Reschedule_TicketPassengerDetails_new_BOA(string Paxdetails, string Ticketdata, string spnr, string SSRinfo, string Paxcount,
            string RescheduleStatus, string OnlineFlag)
        {
            string strStatus = string.Empty;
            string strMsg = string.Empty;
            string strResult = string.Empty;
            string refError = string.Empty;
            string strErrorMsg = string.Empty;
            string xmlResData = string.Empty;
            string strFaretype = string.Empty;
            string strpnr = string.Empty;
            byte[] Getdetails = new byte[] { };
            byte[] dsDet_res = new byte[] { };
            DataSet dsViewPNR = new DataSet();
            DataSet dsDetails = new DataSet();
            string strAgentID = Session["AGENTID"] != null ? Session["AGENTID"].ToString() : "";

            string strTerminalID = Session["TERMINALID"] != null ? Session["TERMINALID"].ToString() : "";
            string strUserName = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "";
            string strIPAddress = Session["IPADDRESS"] != null ? Session["IPADDRESS"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null ? Session["TERMINALTYPE"].ToString() : "";
            string strAgentType = Session["CLIENTTYPE"] != null ? Session["CLIENTTYPE"].ToString() : "";
            string strBranchId = Session["BRANCHID"] != null ? Session["BRANCHID"].ToString() : "";
            string strSeqId = Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : DateTime.Now.ToString("ddMMyyyy");
            string strPosId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPosTId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strPlatform = Session["PLATFORM"] != null ? Session["PLATFORM"].ToString() : "";


            try
            {

                Rays_service.RaysService _rays_servers = new Rays_service.RaysService();;
                _rays_servers.Url = ConfigurationManager.AppSettings["serviceuri"].ToString();
                string Newspnr = _rays_servers.GenerateSPNR();
                string pnr = string.Empty;
                DataSet Newpnrdt = new DataSet();
                DataSet Paxdt = new DataSet();
                DataSet Newflt = new DataSet();
                DataSet Oldds = new DataSet();
                StringReader Txtxml = new StringReader(Paxdetails);
                XmlTextReader Xmlconvert = new XmlTextReader(Txtxml);
                Paxdt.ReadXml(Xmlconvert);
                Paxdt.AcceptChanges();

                Txtxml = new StringReader(Ticketdata);
                Xmlconvert = new XmlTextReader(Txtxml);
                Newflt.ReadXml(Xmlconvert);
                DataSet Dsssr = new DataSet();
                string SSR = string.Empty;
                if (SSRinfo == "")
                {
                    SSR = "<event><SSR><SSR_PAX_REF_NO></SSR_PAX_REF_NO><SSR_SEGMENT_NO></SSR_SEGMENT_NO><SSR_SSR_FLAG></SSR_SSR_FLAG><SSR_SSR_NAME></SSR_SSR_NAME><SSR_SSR_AMOUNT>0</SSR_SSR_AMOUNT></SSR></event>";
                }
                else
                {
                    SSR = SSRinfo;
                }
                Txtxml = new StringReader(SSR);
                Xmlconvert = new XmlTextReader(Txtxml);
                Dsssr.ReadXml(Xmlconvert);
                Oldds = (DataSet)Session["Olddata_" + spnr];

                Byte[] MyParam1 = Compress(Paxdt);
                Byte[] MyParam = Compress(Newflt);
                Byte[] MyParam3 = Compress(Dsssr);
                string mystring = string.Empty;
                byte[] dsViewPNR_compress = new byte[] { };
                byte[] dsFareDispDet_compress = new byte[] { };



                for (int i = 0; i < Convert.ToInt32(Paxcount); i++)
                {

                    pnr += "<NewPNRS><PNR>" + Newspnr + "</PNR><PAXNO>" + Paxdt.Tables[0].Rows[i]["PAX_REF_NO"].ToString() + "</PAXNO></NewPNRS>";

                }
                pnr = "<EVENT>" + pnr + "</EVENT>";
                Txtxml = new StringReader(pnr);
                Xmlconvert = new XmlTextReader(Txtxml);

                Newpnrdt.ReadXml(Xmlconvert);
                Newpnrdt.AcceptChanges();
                Byte[] MyParam2 = Compress(Newpnrdt);

                string logxml = "<event>";

                logxml += "<REQ><NEWFLT>" + Ticketdata + "</NEWFLT>";
                logxml += "<PAXDETAIL>" + Paxdetails + "</PAXDETAIL>";
                logxml += "<NEWPNR>" + pnr + "</NEWPNR>";
                logxml += "<SSR>" + SSR + "</SSR>";
                logxml += "<ONLINERESHREQ></ONLINERESHREQ><ONLINEFLAG>true</ONLINEFLAG></REQ>";

                RescheduleStatus = (RescheduleStatus != null && RescheduleStatus != "") ? RescheduleStatus : "F";
                string onliereq = "";
                bool boolonlineflag = false;
                if (OnlineFlag == "Y")
                {
                    onliereq = Session["RESH_RQRS"] != null ? Session["RESH_RQRS"].ToString() : "";
                    boolonlineflag = true;
                }


                strResult = _rays_servers.Reschedule_TicketPassengerDetails_new_BOA(Convert.ToBase64String(MyParam), Convert.ToBase64String(MyParam1),
                    Convert.ToBase64String(MyParam2), strUserName, strIPAddress, strSeqId, "", RescheduleStatus, ref mystring, "Web_action",
                    "GetTicketcancelpopup_BOA", Convert.ToBase64String(MyParam3), strAgentType, strTerminalID, boolonlineflag, onliereq);
                strResult = mystring;
                strStatus = "03";
                if (mystring.Contains("Successfully"))
                {
                    strResult = mystring + " - " + Newspnr;
                    strStatus = "01";
                }

                logxml += "<RES><OUTPUT>" + strResult + "</OUTPUT><ERRORMSG>" + mystring + "</ERRORMSG></RES></event>";
                DatabaseLog.LogData(strUserName, "E", "ActionController", "WEB_TOBE_RESHEDULE_STEP_3", logxml.ToString(), strPosId, strPosTId, strSeqId);

            }
            catch (Exception ec)
            {

                strStatus = "05";
                strErrorMsg = "Problem Occured. Please contact customercare(05)";
                DatabaseLog.LogData(strUserName, "X", "ActionController", "WEB_TOBE_RESHEDULE_STEP_3-EXP", ec.ToString(), strPosId, strPosTId, strSeqId);
            }

            return Json(new { Status = strStatus, Message = strErrorMsg, Result = strResult });
        }

    }
}
