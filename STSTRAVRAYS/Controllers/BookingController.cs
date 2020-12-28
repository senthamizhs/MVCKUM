using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using STSTRAVRAYS.Models;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Data;
using STSTRAVRAYS.Rays_service;
using System.Globalization;
using System.Runtime.Caching;
using System.Net;

namespace STSTRAVRAYS.Controllers
{
    public class BookingController : Controller
    {
        string strBranchCredit = ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"] != null ? ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"].ToString() : "";
        string HideComissionFor = ConfigurationManager.AppSettings["MINUSCOMISSIONFOR"].ToString();
        string HideComission = "N";

        string strAgentID = string.Empty;
        string strTerminalId = string.Empty;
        string strUserName = string.Empty;
        string strBranchId = string.Empty;
        string sequnceID = string.Empty;
        string Ipaddress = string.Empty;
        string strURLpath = string.Empty;
        string strURLpathA = string.Empty;
        string strURLpathT = string.Empty;
        string strBranchAllowCredit = ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"] != null ? ConfigurationManager.AppSettings["BRANCHALLOWCREDITBOOKING"].ToString().ToUpper() : "";

        Base.ServiceUtility Serv = new Base.ServiceUtility();

        RaysService _RAYS_SERVICE = new RaysService();

        public ActionResult BookingRequest(string ContactDet, string PaxDet, bool BlockTicket, string PaymentMode, string TourCode, string TKey, string service,
            string queue, string Seatvalue, string SegmentType, string mulreq, string Bargainfare, string booktype, string gstdeta, string otherssr, string Appcurrency,
            string ReBook, string strRebook, string Rebookpnr, string Reselect, string SpecialFareBookHitFlag, string ClientID, string BranchID, string GroupID,
            string reqcont, string MailID, string MobileNumber, string passthrow, string ontourcode, string rttourcode, string Insdetails, string MONumber,
            string AllowDuplicateBooking, string CreditshellPNRDetails, string strPaxAddlInfo, string strCashPaymentDet) //her mulreq=="Y" means Multiple select request... (like Domestic Multicity...) //, string Appcurrency, string strRebook, string Reselect added by Rajesh
        {
            #region UsageLog
            string PageName = "Booking Flight";
            try
            {
                string strUsgLogRes = Base.Commonlog(PageName, Session["UniqueIduser"] == null ? "" : Session["UniqueIduser"].ToString(), "BOOK");
            }
            catch (Exception e) { }
            #endregion
            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");  //added by sri-rebooking response send
            Array_Book.Add("");  //added by sri-multicity domastic request
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            int FlightResponse = 1;
            int PaxResponse = 2;
            int GrossFareRespose = 3;
            int ErrorRespose = 4;
            int Ticketingaccess = 5;
            int bookingresponse = 6;
            int rebookingalert = 7;
            int rebookingresultcode = 8;
            int rebookingresponse = 9;
            int multicity_domastic = 10;
            int multicity_domastic_check = 11;
            int paymentgateway_return = 12;

            bool AppStatus = false;

            DataSet dsBookFlight = new DataSet();
            DataSet dsBookingDetails = new DataSet();
            DataTable dtBookingResponse = new DataTable();
            DataTable dtBookingResponse_bookingreq = new DataTable();
            string strResponse = "";
            string airportCategory = "I";
            string ValKey = TKey;
            string bestbuy = string.Empty;
            string lstrpaxref = "";
            string BookFlightdetails = string.Empty;
            string AllPaxdetails = string.Empty;
            string xmldata = string.Empty;
            string[] passri = null;
            CRMDATA _CRMDATA = new CRMDATA();

            string[] torc = ontourcode.Split('|');

            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strClientType = Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : "";
            string strClientID = string.Empty; string strClientTerminalID = string.Empty;
            string strProduct = ConfigurationManager.AppSettings["Producttype"].ToString();
            strUserName = Session["username"] != null && Session["username"].ToString() != "" ? Session["username"].ToString() : "";
            try
            {
                string strAgentLogin = Session["IsAgentLogin"] != null && Session["IsAgentLogin"].ToString() != "" ? Session["IsAgentLogin"].ToString() : "N";
                if (strAgentLogin == "Y" && !string.IsNullOrEmpty(HideComissionFor) && (HideComissionFor.Contains("B2B") || HideComissionFor.Contains("BOA") || HideComissionFor == "ALL"))
                {
                    HideComission = "Y";
                }
                else if (strAgentLogin != "Y" && !string.IsNullOrEmpty(HideComissionFor) && (HideComissionFor.Contains("B2C") || HideComissionFor == "ALL"))
                {
                    HideComission = "Y";
                }
                RQRS.PriceItenaryRS _PriceItenaryRS = new RQRS.PriceItenaryRS();
                List<RQRS.PaxDetails> PaxDetail = new List<RQRS.PaxDetails>();
                RQRS.PaxDetails paxDet = new RQRS.PaxDetails();
                List<List<Travrays_modal.FlightDetails>> lstoflstOnwardAvail = new List<List<Travrays_modal.FlightDetails>>();


                if (strProduct == "RIYA" && (strTerminalType == "T" ? strClientType == "RI" : strAgentType == "RI") && (MONumber == null || MONumber == ""))
                {
                    Array_Book[error] = "Please enter MO Number";
                    ViewBag.GrossFareRespose = "Please enter MO Number";
                    string LgDetails = "<REQUEST><PRODUCT>" + strProduct + "</PRODUCT><TERMINALAPP>" + ConfigurationManager.AppSettings["TerminalType"].ToString() + "</TERMINALAPP><CLIENTTYPE>" + strClientType + "</CLIENTTYPE><CONTACTDET>" + ContactDet + "</CONTACTDET><PAXDET>" + PaxDet + "</PAXDET><PAYMENTMODE>" + PaymentMode + "</PAYMENTMODE><REBOOK>" + ReBook + "</REBOOK><REBOOK>" + ReBook + "</REBOOK><STRREBOOK>" + strRebook + "</STRREBOOK><ALLOWDUPLICATEBOOKING>" + AllowDuplicateBooking + "</ALLOWDUPLICATEBOOKING></REQUEST>";
                    DatabaseLog.LogData(strUserName, "E", "BookFlight", "MONumber empty return", LgDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    return PartialView("_BookingSuccess", "");
                }

                if ((PaymentMode == "P" || PaymentMode == "B") && BlockTicket == true)
                {
                    Array_Book[error] = "Invalid paymentmode selected , Please select Topup account for block PNR";
                    ViewBag.GrossFareRespose = "Invalid paymentmode selected , Please select Topup account for block PNR";
                    string LgDetails = "<REQUEST><PAYMENTMODE>" + PaymentMode + "</PAYMENTMODE><BLOCKTICKET>" + BlockTicket + "</BLOCKTICKET></REQUEST>";
                    DatabaseLog.LogData(strUserName, "E", "BookFlight", "Invalid Payment Mode for BlockPNR", LgDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    return PartialView("_BookingSuccess", "");
                }

                if (strRebook == null)
                {
                    #region multicity domastic condition-maintain session key--write by srinath-2018-11-
                    if (mulreq == "Y" && SegmentType == "D")
                    {

                        if (Session["Priceresponse" + TKey] != null && Session["Priceresponse" + TKey].ToString() != "")
                        {
                            _PriceItenaryRS = JsonConvert.DeserializeObject<RQRS.PriceItenaryRS>(Session["Priceresponse" + TKey].ToString());
                        }
                    }

                    else
                    {  //(else condition normal booking request)
                        if (Session["Priceresponse"] != null && Session["Priceresponse"].ToString() != "")
                        {
                            _PriceItenaryRS = JsonConvert.DeserializeObject<RQRS.PriceItenaryRS>(Session["Priceresponse"].ToString());
                        }
                    }
                    #endregion multicity end
                }

                Array_Book[multicity_domastic] = TKey;
                if (mulreq == "Y")
                {
                    Session.Add("AppBookStatus", true); //For Temporary for Domestic Multicity...ll remove after Pending Track functin is called...
                }

                if (Session["AppBookStatus"] != null && Session["AppBookStatus"].ToString() != "")
                {
                    AppStatus = (bool)Session["AppBookStatus"];
                }
                if (Session["ticketing"].ToString() != "" && Session["ticketing"] != null)
                {
                    Array_Book[Ticketingaccess] = Session["ticketing"].ToString();
                }
                string TotAmt = string.Empty;
                int adultCount = 0;
                int childCount = 0;
                int infantCount = 0;


                int adtcnt = 0;
                int chdcnt = 0;
                int infcnt = 0;
                string paxcount_rebo = "";
                int ALLpaxcount_rebo = 0;
                int loopconts = 0;
                Array_Book[multicity_domastic_check] = "0";
                ClientID = ClientID != null && ClientID != "" ? ClientID : Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";

                string sSelectDataforSpecialFareBooking = "sSelectDataforSpecialFareBooking" + ValKey;
                string sSelectDataforBooking = "sSelectDataforBooking" + ValKey;
                if (Session["BookStart" + ValKey] != null)
                {
                    if (Session["BookStart" + ValKey].ToString() == "true")
                    {
                        Array_Book[error] = "Booking initiated. Please wait for your transaction in processing...";
                        // return Array_Book;
                        //return Json(new { Status = "", Message = "", Result = Array_Book });
                        ViewBag.GrossFareRespose = "Booking initiated. Please wait for your transaction in processing...";
                        return PartialView("_BookingSuccess", "");
                    }
                }
                Session.Add("BookStart" + ValKey, "true");


                if (mulreq == "Y" && SegmentType == "D")     //multicity domastic sri
                {
                    Array_Book[multicity_domastic_check] = "1";
                    if (Session["dtBookreq_table" + ValKey] != null)
                    {
                        dtBookingResponse_bookingreq = (DataTable)Session["dtBookreq_table" + ValKey];
                    }
                }
                else if (Session["dtBookreq_table" + ValKey] != null)
                {
                    dtBookingResponse_bookingreq = (DataTable)Session["dtBookreq_table" + ValKey];
                }
                else
                {
                    if (Session["dtBookreq_table"] != null)
                    {
                        dtBookingResponse_bookingreq = (DataTable)Session["dtBookreq_table"];
                    }
                }


                string roundtrip_doub = Session["roundtripflg_SESS" + ValKey] != null && Session["roundtripflg_SESS" + ValKey].ToString() != "" ? Session["roundtripflg_SESS" + ValKey].ToString() : ""; //1-roun


                bool Rebook = string.IsNullOrEmpty(ReBook) ? false : Convert.ToBoolean(ReBook);
                string lstrTrackid = string.Empty;
                string PaxCount = Session["PaxCount" + ValKey].ToString();

                if (mulreq != "Y")
                {
                    dtBookingResponse = (DataTable)Session["Response" + ValKey];
                }
                bestbuy = Session["BestBuy" + ValKey].ToString();


                string Farelist = (Session["StudentArmyFareFlag"] != null && Session["StudentArmyFareFlag"] != "") ? Session["StudentArmyFareFlag"].ToString() : "falseSPLITSRCASfalseSPLITSRCASfalse";

                #region Duplicate Booking validate

                if (AppStatus == false && (ReBook.ToString().ToUpper() == "FALSE" && AllowDuplicateBooking.ToString().ToUpper() == "FALSE"))
                {
                    string[] exitpaxdetails, exitIpax;
                    string LggDetails = "";
                    if (PaxDet.Contains("|"))
                    {
                        exitpaxdetails = PaxDet.Split('|');
                        exitIpax = Regex.Split(exitpaxdetails[0], "SPLITSCRIPT");
                        LggDetails = "<BOOKING_REQUEST><FIRSTNAME>" + exitIpax[1] + "</FIRSTNAME><LASTNAME>" + exitIpax[2] + "</LASTNAME><ORIGIN>" + dtBookingResponse.Rows[0]["ORIGIN"]
                       + "</ORIGIN><DESTINATION>" + dtBookingResponse.Rows[0]["DESTINATION"] +
                     "</DESTINATION><TRAVELDATE>" + dtBookingResponse.Rows[0]["DEPARTUREDATE"] + "</TRAVELDATE><FLIGHTNO>" + dtBookingResponse.Rows[0]["DEPARTUREDATE"] + "</FLIGHTNO><APPSTATUS>" + AppStatus + "</APPSTATUS><REBOOKFLAG>" + ReBook + "</REBOOKFLAG><DUPLICATEBOOKING>" + AllowDuplicateBooking + "</DUPLICATEBOOKING></BOOKING_REQUEST>";
                    }

                    Array_Book[error] = "Booking process already initiated please check with your previous booking details";
                    ViewBag.GrossFareRespose = "Booking process already initiated please check with your previous booking details";
                    DatabaseLog.LogData(Session["username"].ToString(), "E", "BookFlight", "BOOKING REQUEST - DUPLICATE BOOKING TRUE", LggDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    Session.Add("BookStart" + ValKey, "false");
                    return PartialView("_BookingSuccess", "");
                }

                #endregion

                #region Booking Request formation
                if (dtBookingResponse != null && dsBookingDetails.Tables.Count == 0)
                {
                    dsBookingDetails.Tables.Clear();
                    dsBookingDetails.Tables.Add(dtBookingResponse.Copy());
                }

                else
                {
                    Array_Book[error] = "Booking Response not avaliable";
                    return PartialView("_BookingSuccess", "");
                }

                /***/
                List<RQRS.FFNumber> FFnumberDetail = new List<RQRS.FFNumber>();
                RQRS.FFNumber FFdet = new RQRS.FFNumber();
                RQRS.FOPDetails _FOPDetails = new RQRS.FOPDetails();

                RQRS.BookingRquest Booking = new RQRS.BookingRquest();
                List<RQRS.ItineraryFlights> ItineraryDetail = new List<RQRS.ItineraryFlights>();
                List<RQRS.Flights> FlightDetail = new List<RQRS.Flights>();
                RQRS.CBT_Credentials _CBT_Credentials = new RQRS.CBT_Credentials();
                RQRS.ERP_Attribute _ERP_Attribute = new RQRS.ERP_Attribute();
                List<RQRS.ERP_Attribute> _LST_ERP_Attribute = new List<RQRS.ERP_Attribute>();
                RQRS.PAYMENT_INFO _CashPaymentInfo = new RQRS.PAYMENT_INFO();

                List<RQRS.Dealcode> _DealCodelist = new List<RQRS.Dealcode>();
                RQRS.Dealcode _Dealcode = new RQRS.Dealcode();
                /****/
                DatabaseLog.LogData(strUserName, "B", "BookFlight", "BOOKING FORMATION", ContactDet + " - Pax Details - " + PaxDet.ToString() + "-" + BlockTicket.ToString() + "-" + PaymentMode + "-" + TourCode + "-" + TKey, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                /****/
                Dictionary<int, string> SSR_Val = new Dictionary<int, string>();

                #region cash payment info details
                if (PaymentMode == "H")
                {
                    if (string.IsNullOrEmpty(strCashPaymentDet))
                    {
                        Array_Book[error] = "<b>Cash Payment Details</b> is mandatory for Booking for payment mode Cash";
                        ViewBag.GrossFareRespose = "<b>Cash Payment Details</b> is mandatory for Booking for payment mode Cash";
                        string LgDetails = "<REQUEST><PAYMENTMODE>" + PaymentMode + "</PAYMENTMODE><strCashPaymentDet>" + strCashPaymentDet + "</strCashPaymentDet></REQUEST>";
                        DatabaseLog.LogData(strUserName, "E", "BookFlight", "Cash payment details", LgDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        return PartialView("_BookingSuccess", "");
                    }
                    else
                    {
                        string[] strCashPaymentDetails = Regex.Split(strCashPaymentDet, "SPLITCASH");
                        _CashPaymentInfo.CUSTOMER_MOBILENO = strCashPaymentDetails[0];
                        _CashPaymentInfo.CUSTOMER_EMAILID = strCashPaymentDetails[1];
                        _CashPaymentInfo.BOOKING_REFERENCE = strCashPaymentDetails[2];
                        _CashPaymentInfo.EMPLOYEE_CODE = strCashPaymentDetails[3];
                        _CashPaymentInfo.EMPLOYEE_NAME = strCashPaymentDetails[4];
                        _CashPaymentInfo.EMPLOYEE_EMAILID = strCashPaymentDetails[5];
                        _CashPaymentInfo.EMPLOYEE_BRANCH = strCashPaymentDetails[7];
                        _CashPaymentInfo.EXPECT_PAYMENT = strCashPaymentDetails[8];
                        _CashPaymentInfo.PAYMENT_MODE = strCashPaymentDetails[9];
                        _CashPaymentInfo.PAN_CARD = strCashPaymentDetails[10];
                        _CashPaymentInfo.REMARKS = strCashPaymentDetails[11];
                        _CashPaymentInfo.BOOKED_BY = strUserName;
                    }
                }
                #endregion

                #region Paxdetails

                string[] paxdetails, Ipax;
                if (PaxDet.Contains("|"))
                {

                    PaxDet = PaxDet.Remove(PaxDet.Length - 1);
                    paxdetails = PaxDet.Split('|');
                    for (int ip = 0; ip < paxdetails.Length; ip++)
                    {
                        if (paxdetails[ip].Contains("SPLITSCRIPT"))
                        {
                            paxDet = new RQRS.PaxDetails();
                            Ipax = Regex.Split(paxdetails[ip], "SPLITSCRIPT");
                            paxDet.Age = "";
                            paxDet.Title = Ipax[0];
                            //paxDet.FirstName = Regex.Replace(Ipax[1], @"[^0-9a-zA-Z\\s]+$", ""); //Ipax[1];
                            //paxDet.LastName = Regex.Replace(Ipax[2], @"[^0-9a-zA-Z\\s]+$", ""); //Ipax[2];
                            paxDet.FirstName = Regex.Replace(Ipax[1], @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+", "").Replace("'\'", "").Replace("/", ""); //Ipax[1];
                            paxDet.LastName = Regex.Replace(Ipax[2], @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+", "").Replace("'\'", "").Replace("/", ""); //Ipax[1];
                            paxDet.Gender = Ipax[3] == "M" ? "Male" : "Female";

                            if (SegmentType == "D" && strProduct == "JOD")
                            {

                                paxDet.PassportNo = "";
                                paxDet.PassportExpiry = "";
                            }
                            else
                            {

                                paxDet.PassportNo = (Ipax[5].Trim() != "" ? Ipax[5] + "|" + Ipax[7] : "");
                                paxDet.PassportExpiry = (Ipax[5].Trim() != "" ? Ipax[6] : "");
                            }
                            paxDet.DOB = Ipax[4];
                            paxDet.MailID = ConfigurationManager.AppSettings["BookingAPIMailid"].ToString() == "" ? ((MailID == null || MailID == "") ? ConfigurationManager.AppSettings["Bookingagntmailid"].ToString() : MailID) : ConfigurationManager.AppSettings["BookingAPIMailid"].ToString();
                            paxDet.Mobnumber = MobileNumber;
                            paxDet.PlaceOfBirth = "";
                            var Paxref = Ipax[8].Split(':');
                            if (Paxref[0].Trim().ToUpper() == "ADULT")
                            {
                                paxDet.PaxType = "ADT";
                                lstrpaxref += Convert.ToString(ip + 1) + "|";
                                adtcnt++;
                            }
                            if (Paxref[0].Trim().ToUpper() == "CHILD")
                            {
                                paxDet.PaxType = "CHD";
                                lstrpaxref += Convert.ToString(ip + 1) + "|";
                                chdcnt++;
                            }
                            if (Paxref[0].Trim().ToUpper() == "INFANT")
                            {
                                paxDet.PaxType = "INF";
                                paxDet.InfantRef = Ipax[9].Trim();
                                infcnt++;
                            }
                            else
                            {
                                paxDet.InfantRef = "";
                            }
                            paxDet.PaxRefNumber = Convert.ToString(ip + 1);
                            SSR_Val.Add((ip + 1), Ipax[10]);

                            paxDet.CorpRefID = "";
                            paxDet.EMP_ID = "";
                            paxDet.EmpCostCenter = "0";
                            paxDet.PaxInfoType = "G";
                            paxDet.Nationality = Ipax.Length > 11 ? Ipax[12] : "";
                            paxDet.PassportIssuedDate = (Ipax.Length > 12 && Ipax[13] != null) ? Ipax[13] : "";
                            paxDet.Assignee = (Ipax.Length > 12 && Ipax[11] != null) ? Ipax[11] : "";
                            PaxDetail.Add(paxDet);
                        }

                    }
                }
                else
                {

                }
                string Chd_chk = string.Empty;
                string Inf_chk = string.Empty;

                Chd_chk = chdcnt > 0 ? "|" + chdcnt : "|0";
                Inf_chk = infcnt > 0 ? "|" + infcnt : "|0";

                paxcount_rebo = adtcnt + Chd_chk + Inf_chk;
                ALLpaxcount_rebo = Convert.ToInt32(adtcnt) + Convert.ToInt32(chdcnt) + Convert.ToInt32(infcnt);
                Session.Add("paxcount_rebo", paxcount_rebo);

                #endregion

                #region Passegner Additional Details
                if (strPaxAddlInfo != "")
                {
                    string[] strPaxAddlDet = Regex.Split(strPaxAddlInfo, "SPLITSCRIPTPAX");
                    for (int _count = 0; _count < strPaxAddlDet.Length - 1; _count++)
                    {
                        string[] strSinglePaxAddlDet = Regex.Split(strPaxAddlDet[_count], "SPLITSCRIPT");
                        PaxDetail[_count].MailID = strSinglePaxAddlDet[1].ToString();
                        PaxDetail[_count].Mobnumber = strSinglePaxAddlDet[2].ToString();
                        PaxDetail[_count].ReasonForTravel = strSinglePaxAddlDet[3].ToString();
                        PaxDetail[_count].State = strSinglePaxAddlDet[4].ToString();
                        PaxDetail[_count].City = strSinglePaxAddlDet[5].ToString();
                        PaxDetail[_count].PostalCode = strSinglePaxAddlDet[6].ToString();
                        PaxDetail[_count].Address = strSinglePaxAddlDet[7].ToString();
                        PaxDetail[_count].IsCovid19TestDone = strSinglePaxAddlDet[8].ToString();
                        PaxDetail[_count].CountryCodeToFly = Base.GetCountryCode(Session["BaseDest" + ValKey].ToString());
                    }
                }
                #endregion

                DataSet dstemp = new DataSet();
                int qrycnt = 1;

                string[] artAirCata = new string[] { };
                if (mulreq != "Y")
                {
                    var qry = (from p in dsBookingDetails.Tables["TrackFareDetails"].AsEnumerable()
                               select p["AIRLINECATEGORY"].ToString()).Distinct();

                    dstemp = dsBookingDetails.Copy();
                    if (qry.Count() > 0)
                    {
                        artAirCata = qry.ToArray();
                    }
                    qrycnt = qry.Count();
                }
                string Token = string.Empty;
                string lstrsegrefnum = string.Empty;

                if (Session["TripType" + ValKey].ToString() == "R")
                {
                    qrycnt = 1;
                }

                for (int index = 0; index < qrycnt; index++)
                {
                    if (mulreq != "Y")
                    {
                        var erp = from q in dstemp.Tables["TrackFareDetails"].AsEnumerable()
                                  where (q["AIRLINECATEGORY"].ToString().Trim().ToUpper() ==
                                  artAirCata[index].ToString().Trim().ToUpper())
                                  select q;
                        DataTable dttemp = erp.CopyToDataTable();
                        dttemp.TableName = "TrackFareDetails";
                        dsBookingDetails = new DataSet();
                        dsBookingDetails.Tables.Add(dttemp.Copy());
                    }
                    string lstrtotseg = "";
                    RQRS.ItineraryFlights itindetail = new RQRS.ItineraryFlights();


                    DataSet _bookflightds = new DataSet();
                    DataTable _bookflightdt = new DataTable();
                    RQRS.Flights flightdet = new RQRS.Flights();
                    string[] lststr = new string[] { };
                    //var linq_book = new { };

                    var groupedDatacnt = 0;
                    if (mulreq != "Y")
                    {
                        var groupedData = from b in dsBookingDetails.Tables["TrackFareDetails"].AsEnumerable()
                                          group b by b.Field<string>("itinRef") into g
                                          let count = g.Count()
                                          select new
                                          {
                                              //ChargeTag = g.Key,
                                              Count = g.Key,
                                          };
                        groupedDatacnt = groupedData.Count();
                    }

                    int loop_count = mulreq == "Y" ? Convert.ToInt32("1") : (Session["TripType" + ValKey].ToString().Trim() == "Y" || Session["TripType" + ValKey].ToString().Trim() == "M") ? 1 : groupedDatacnt;
                    loop_count = (Session["TripType" + ValKey].ToString().Trim() == "M") ? loop_count : Session["Specialflagfare" + ValKey].ToString() == "Y" || Session["Specialflagfare" + ValKey].ToString() == "P" ? 1 : loop_count;
                    int segcnt = 0;
                    if (roundtrip_doub == "1")
                        loop_count = 1;
                    loopconts = 1;
                    for (int i = 0; i < loop_count; i++)
                    {
                        if (mulreq != "Y" || (mulreq == "Y" && Session["Response" + reqcont + ValKey] != null))
                        {
                            if (mulreq == "Y")
                            {
                                dtBookingResponse = (DataTable)Session["Response" + reqcont + ValKey];

                                dsBookingDetails.Tables.Clear();
                                dsBookingDetails.Tables.Add(dtBookingResponse.Copy());

                                var qry = (from p in dsBookingDetails.Tables["TrackFareDetails"].AsEnumerable()
                                           select p["AIRLINECATEGORY"].ToString()).Distinct();
                                dstemp = dsBookingDetails.Copy();
                                if (qry.Count() > 0)
                                {
                                    artAirCata = qry.ToArray();
                                }

                                var erp = from q in dstemp.Tables["TrackFareDetails"].AsEnumerable()
                                          where (q["AIRLINECATEGORY"].ToString().Trim().ToUpper() ==
                                          artAirCata[0].ToString().Trim().ToUpper())  //artAirCata[0] is For Temporary prupose... wanna check in future...
                                          select q;
                                DataTable dttemp = erp.CopyToDataTable();
                                dttemp.TableName = "TrackFareDetails";
                                dsBookingDetails = new DataSet();
                                dsBookingDetails.Tables.Add(dttemp.Copy());
                            }

                            if (Session["TripType" + ValKey].ToString().Trim() == "Y" || Session["Specialflagfare" + ValKey].ToString().Trim() == "Y" || Session["Specialflagfare" + ValKey].ToString().Trim() == "P" || (Session["TripType" + ValKey].ToString().Trim() == "M" && mulreq != "Y"))
                            {
                                var linq_book = (from _bookfl in dsBookingDetails.Tables["TrackFareDetails"].AsEnumerable()
                                                     // where _bookfl["itinRef"].ToString().Trim() == i.ToString()
                                                 select _bookfl);
                                _bookflightdt = new DataTable();
                                _bookflightds = new DataSet();
                                _bookflightdt = linq_book.CopyToDataTable();
                                _bookflightds.Tables.Add(_bookflightdt);
                            }
                            else
                            {
                                var refFlg = mulreq == "Y" ? 0 : (Session["TripType" + ValKey].ToString().Trim() == "R" && index > 0) ? 1 : i; // index //Need to check for Roundtrip (both same and different airline category)...
                                var linq_book = (from _bookfl in dsBookingDetails.Tables["TrackFareDetails"].AsEnumerable()
                                                 where _bookfl["itinRef"].ToString().Trim() == refFlg.ToString()
                                                 select _bookfl);
                                _bookflightdt = new DataTable();
                                _bookflightds = new DataSet();
                                _bookflightdt = linq_book.CopyToDataTable();
                                _bookflightds.Tables.Add(_bookflightdt);
                            }



                            if (_bookflightds.Tables[0].Rows.Count > 0)
                            {
                                _FOPDetails = new RQRS.FOPDetails();
                                itindetail = new RQRS.ItineraryFlights();
                                FlightDetail = new List<RQRS.Flights>();

                                if (passthrow != null && passthrow != "")
                                {
                                    passri = passthrow.Split('~');
                                }

                                _FOPDetails.CardName = passri != null && passri.Length > 4 ? passri[0] : "";
                                _FOPDetails.CardNumber = passri != null && passri.Length > 4 ? passri[2] : "";
                                _FOPDetails.CardType = passri != null && passri.Length > 4 ? passri[1] : "";
                                _FOPDetails.CVV_Number = passri != null && passri.Length > 4 ? passri[5] : "";
                                _FOPDetails.ExpiryDate = passri != null && passri.Length > 4 ? passri[3] + "" + passri[4] : "";

                                lststr = _bookflightds.Tables[0].Rows[0]["stocktype"].ToString().Split('|');

                                #region rebooking function sri
                                if (Session[sSelectDataforSpecialFareBooking] != null && Session[sSelectDataforSpecialFareBooking] != "" && SpecialFareBookHitFlag == "S")
                                {
                                    if (strRebook != null && strRebook != "null" && strRebook != "")
                                    {
                                        _PriceItenaryRS = (RQRS.PriceItenaryRS)JsonConvert.DeserializeObject(strRebook, typeof(RQRS.PriceItenaryRS));
                                        System.Web.HttpContext.Current.Session[sSelectDataforSpecialFareBooking] = _PriceItenaryRS;
                                        xmldata = "<REBOOKING_INITIATE>" + JsonConvert.SerializeObject(_PriceItenaryRS) + "</REBOOKING_INITIATE>";
                                        DatabaseLog.LogData(System.Web.HttpContext.Current.Session["Loginusermailid"] != null ? System.Web.HttpContext.Current.Session["Loginusermailid"].ToString() : "", "T", "BOOKINGPAGE", "Rebooking", xmldata, System.Web.HttpContext.Current.Session["POS_ID"].ToString(), System.Web.HttpContext.Current.Session["POS_TID"].ToString(), System.Web.HttpContext.Current.Session["sequenceid"].ToString());
                                    }

                                    else
                                    {
                                        _PriceItenaryRS = (RQRS.PriceItenaryRS)Session[sSelectDataforSpecialFareBooking];
                                        xmldata = "<BOOKING_INITIATE>" + JsonConvert.SerializeObject(_PriceItenaryRS) + "</BOOKING_INITIATE>";
                                        DatabaseLog.LogData(System.Web.HttpContext.Current.Session["Loginusermailid"] != null ? System.Web.HttpContext.Current.Session["Loginusermailid"].ToString() : "", "T", "BOOKINGPAGE", "BOOKINGREQ", xmldata, System.Web.HttpContext.Current.Session["POS_ID"].ToString(), System.Web.HttpContext.Current.Session["POS_TID"].ToString(), System.Web.HttpContext.Current.Session["sequenceid"].ToString());
                                    }


                                }
                                else
                                {
                                    if (Session[sSelectDataforBooking] != null && Session[sSelectDataforBooking] != "")
                                    {
                                        if (strRebook != null && strRebook != "null" && strRebook != "")
                                        {
                                            _PriceItenaryRS = (RQRS.PriceItenaryRS)JsonConvert.DeserializeObject(strRebook, typeof(RQRS.PriceItenaryRS));
                                            System.Web.HttpContext.Current.Session[sSelectDataforBooking] = _PriceItenaryRS;
                                            xmldata = "<REBOOKING_INITIATE>" + JsonConvert.SerializeObject(_PriceItenaryRS) + "</REBOOKING_INITIATE>";
                                            DatabaseLog.LogData(System.Web.HttpContext.Current.Session["Loginusermailid"] != null ? System.Web.HttpContext.Current.Session["Loginusermailid"].ToString() : "", "T", "BOOKINGPAGE", "Rebooking", xmldata, System.Web.HttpContext.Current.Session["POS_ID"].ToString(), System.Web.HttpContext.Current.Session["POS_TID"].ToString(), System.Web.HttpContext.Current.Session["sequenceid"].ToString());
                                        }
                                        else if (Reselect != null && Reselect != "null" && Reselect != "")
                                        {
                                            _PriceItenaryRS = (RQRS.PriceItenaryRS)JsonConvert.DeserializeObject(Reselect, typeof(RQRS.PriceItenaryRS));
                                            System.Web.HttpContext.Current.Session[sSelectDataforBooking] = _PriceItenaryRS;
                                            xmldata = "<RESELECTBOOKING_INITIATE>" + JsonConvert.SerializeObject(_PriceItenaryRS) + "</RESELECTBOOKING_INITIATE>";
                                            DatabaseLog.LogData(System.Web.HttpContext.Current.Session["Loginusermailid"] != null ? System.Web.HttpContext.Current.Session["Loginusermailid"].ToString() : "", "T", "BOOKINGPAGE", "Reselectbooking", xmldata, System.Web.HttpContext.Current.Session["POS_ID"].ToString(), System.Web.HttpContext.Current.Session["POS_TID"].ToString(), System.Web.HttpContext.Current.Session["sequenceid"].ToString());
                                        }
                                        else
                                        {
                                            _PriceItenaryRS = (RQRS.PriceItenaryRS)Session[sSelectDataforBooking];
                                            xmldata = "<BOOKING_INITIATE>" + JsonConvert.SerializeObject(_PriceItenaryRS) + "</BOOKING_INITIATE>";
                                            DatabaseLog.LogData(System.Web.HttpContext.Current.Session["Loginusermailid"] != null ? System.Web.HttpContext.Current.Session["Loginusermailid"].ToString() : "", "T", "BOOKINGPAGE", "BOOKINGREQ", xmldata, System.Web.HttpContext.Current.Session["POS_ID"].ToString(), System.Web.HttpContext.Current.Session["POS_TID"].ToString(), System.Web.HttpContext.Current.Session["sequenceid"].ToString());
                                        }
                                    }
                                }

                                #endregion rebooking function end

                                #region round trip start
                                if (roundtrip_doub == "1" && dtBookingResponse_bookingreq != null)
                                {


                                    if (_PriceItenaryRS.PriceItenarys.Count > 0)
                                    {
                                        for (var N = 0; _PriceItenaryRS.PriceItenarys.Count > N; N++)
                                        {
                                            var FDD = from _Flightsd in _PriceItenaryRS.PriceItenarys[N].PriceRS[0].Flights.AsEnumerable() //_PriceItenaryRS.PriceRS[0].Flights.AsEnumerable()//ABCD
                                                      where _Flightsd.FareId != "" //&& _Fares.FlightId != ""
                                                      select new Travrays_modal.FlightDetails
                                                      {
                                                          FlightNumber = _Flightsd.FlightNumber,
                                                          Origin = _Flightsd.Origin,
                                                          Destination = _Flightsd.Destination,
                                                          DepartureDate = _Flightsd.DepartureDateTime.Split(' ')[0] + " " + _Flightsd.DepartureDateTime.Split(' ')[1] + " " + _Flightsd.DepartureDateTime.Split(' ')[2],
                                                          DepartureTime = _Flightsd.DepartureDateTime.Split(' ')[3],
                                                          ArrivalDate = _Flightsd.ArrivalDateTime.Split(' ')[0] + " " + _Flightsd.ArrivalDateTime.Split(' ')[1] + " " + _Flightsd.ArrivalDateTime.Split(' ')[2],
                                                          ArrivalTime = _Flightsd.ArrivalDateTime.Split(' ')[3],
                                                          FlyingTime = _Flightsd.FlyingTime,
                                                          Class = _Flightsd.Class,//_Flights["Class"] + "-" + _Flights["FareBasisCode"],
                                                          Cabin = (_Flightsd.Cabin != null && _Flightsd.Cabin.ToString() != "" ? _Flightsd.Cabin : (Session["segmclass" + ValKey] != null && Session["segmclass" + ValKey].ToString() != "" ? Session["segmclass" + ValKey].ToString() : "E")),
                                                          AvailSeat = _Flightsd.AvailSeat,
                                                          CarrierCode = _Flightsd.PlatingCarrier,//_Flights["CarrierCode"],
                                                          CNX = _Flightsd.CNX,
                                                          ConnectionFlag = _Flightsd.ConnectionFlag,
                                                          FareBasisCode = _Flightsd.FareBasisCode,
                                                          FareId = _Flightsd.FareId,
                                                          PlatingCarrier = _Flightsd.PlatingCarrier,
                                                          ReferenceToken = _Flightsd.ReferenceToken,
                                                          SegRef = _Flightsd.SegRef,
                                                          Stops = _Flightsd.Stops,
                                                          via = _Flightsd.Via,
                                                          Refund = _Flightsd.Refundable,
                                                          ItinRef = _Flightsd.ItinRef,
                                                          Airlinecategory = _Flightsd.AirlineCategory,
                                                          ClassCarrierCode = _Flightsd.Class + "-" + _Flightsd.FareBasisCode,
                                                          FareType = _PriceItenaryRS.PriceItenarys[N].PriceRS[0].Fares[0].FareType,
                                                          StartTerminal = _Flightsd.StartTerminal,
                                                          EndTerminal = _Flightsd.EndTerminal,
                                                          FareTypeDescription = _Flightsd.FareTypeDescription,
                                                          Operatingcarrier = _Flightsd.OperatingCarrier,
                                                          JourneyTime = _Flightsd.JourneyTime,
                                                          PromoCodeDesc = _Flightsd.PromoCodeDesc,
                                                          PromoCode = _Flightsd.PromoCode

                                                      };
                                            List<List<Travrays_modal.FlightDetails>> lstoflstOnwardAvailsnew = new List<List<Travrays_modal.FlightDetails>>();
                                            lstoflstOnwardAvailsnew = FDD.GroupBy(u => u.ItinRef).Select(grp => grp.ToList()).ToList();
                                            lstoflstOnwardAvail.Add(lstoflstOnwardAvailsnew[0]);
                                        }
                                    }

                                    #region rebook request formation roundtrip
                                    if (strRebook != null && strRebook != "")
                                    {
                                        List<RQRS.Payment> PaymentDetail_s = new List<RQRS.Payment>();
                                        RQRS.Payment PaymentDet_s = new RQRS.Payment();
                                        PaymentDet_s = new RQRS.Payment();
                                        FlightDetail = new List<RQRS.Flights>();
                                        itindetail = new RQRS.ItineraryFlights();
                                        // DataTable dtmine = myqry.CopyToDataTable();
                                        for (int m = 0; i < lstoflstOnwardAvail.Count; m++)
                                        {
                                            _FOPDetails = new RQRS.FOPDetails();

                                            if (passthrow != null && passthrow != "")
                                            {
                                                passri = passthrow.Split('~');
                                            }

                                            _FOPDetails.CardName = passri != null && passri.Length > 4 ? passri[0] : "";
                                            _FOPDetails.CardNumber = passri != null && passri.Length > 4 ? passri[2] : "";
                                            _FOPDetails.CardType = passri != null && passri.Length > 4 ? passri[1] : "";
                                            _FOPDetails.CVV_Number = passri != null && passri.Length > 4 ? passri[5] : "";
                                            _FOPDetails.ExpiryDate = passri != null && passri.Length > 4 ? passri[3] + "" + passri[4] : "";

                                            for (int j = 0; j < lstoflstOnwardAvail[m].Count; j++)
                                            {
                                                flightdet.AirlineCategory = lstoflstOnwardAvail[m][j].Airlinecategory;
                                                flightdet.ArrivalDateTime = lstoflstOnwardAvail[m][j].ArrivalDate + " " + lstoflstOnwardAvail[m][j].ArrivalTime;
                                                flightdet.Class = lstoflstOnwardAvail[m][j].Class;//
                                                flightdet.CNX = lstoflstOnwardAvail[m][j].CNX;//"N";
                                                flightdet.DepartureDateTime = lstoflstOnwardAvail[m][j].DepartureDate + " " + lstoflstOnwardAvail[m][j].DepartureTime;
                                                flightdet.Destination = lstoflstOnwardAvail[m][j].Destination;
                                                flightdet.FareBasisCode = lstoflstOnwardAvail[m][j].FareBasisCode;
                                                flightdet.StartTerminal = lstoflstOnwardAvail[m][j].StartTerminal;
                                                flightdet.EndTerminal = lstoflstOnwardAvail[m][j].EndTerminal;
                                                flightdet.FlightNumber = lstoflstOnwardAvail[m][j].FlightNumber;//"";
                                                flightdet.FlyingTime = lstoflstOnwardAvail[m][j].FlyingTime;//"";
                                                flightdet.Origin = lstoflstOnwardAvail[m][j].Origin;//"";
                                                flightdet.PlatingCarrier = lstoflstOnwardAvail[m][j].PlatingCarrier;//"";
                                                flightdet.ReferenceToken = lstoflstOnwardAvail[m][j].ReferenceToken;//"";
                                                flightdet.SegRef = lstoflstOnwardAvail[m][j].SegRef;// "";
                                                lstrtotseg += lstoflstOnwardAvail[m][j].SegRef;// ""; + "|";
                                                flightdet.SegmentDetails = "";

                                                flightdet.JourneyTime = lstoflstOnwardAvail[m][j].JourneyTime;
                                                flightdet.OtherFares = "";
                                                flightdet.MultiClass = "";
                                                flightdet.Via = lstoflstOnwardAvail[m][j].via;
                                                flightdet.RBDCode = lstoflstOnwardAvail[m][j].Class;//"";
                                                flightdet.FareId = lstoflstOnwardAvail[m][j].FareId;//"SG0";
                                                flightdet.OfflineFlag = 0;
                                                flightdet.ConnectionFlag = lstoflstOnwardAvail[m][j].ConnectionFlag;//"N";
                                                flightdet.TransactionFlag = true;// (allowtransfee == "Y") ? true : false;
                                                flightdet.Cabin = lstoflstOnwardAvail[m][j].Cabin;
                                                flightdet.Refundable = lstoflstOnwardAvail[m][j].Refund;// "";
                                                flightdet.ItinRef = lstoflstOnwardAvail[m][j].ItinRef;//"";
                                                flightdet.OperatingCarrier = lstoflstOnwardAvail[m][j].Operatingcarrier;
                                                flightdet.PromoCodeDesc = lstoflstOnwardAvail[m][j].PromoCodeDesc;
                                                flightdet.PromoCode = lstoflstOnwardAvail[m][j].PromoCode;
                                                FlightDetail.Add(flightdet);

                                                itindetail.HostID = j.ToString();
                                                itindetail.FlightDetails = FlightDetail;
                                                itindetail.FOPDetails = _FOPDetails;
                                                itindetail.PaymentMode = PaymentMode;
                                                itindetail.FareType = _PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].FareType;
                                                itindetail.FareTypeDescription = _PriceItenaryRS.PriceItenarys[i].PriceRS[0].Flights[0].FareTypeDescription;
                                                itindetail.FFNumber = FFnumberDetail;
                                                itindetail.Stock = _PriceItenaryRS.PriceItenarys[i].PriceRS[0].FareCheck.Stocktype;// lststr.Count() > 1 ? (Session["TripType" + ValKey].ToString().Trim() == "R" && index > 0) ? lststr[1] : lststr[i] : lststr[0];//drrFlights["stocktype"].ToString();// Session["Stock" + ValKey].ToString();stocktype
                                                itindetail.Payment = PaymentDetail_s;
                                                itindetail.Pricingcode = "";

                                                Decimal amt = 0;
                                                for (int NN = 0; NN < _PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription.Count; NN++)
                                                {
                                                    if (HideComission == "Y")
                                                    {
                                                        amt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)
                                                            + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFAMOUNT) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFGST)
                                                            - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].Incentive) - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].PLBAmount) 
                                                            - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].Discount)) * Base.ServiceUtility.ConvertToDecimal(paxcount_rebo.Split('|')[NN]));
                                                    }
                                                    else
                                                    {
                                                        amt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)
                                                        + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFAMOUNT) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFGST)) * Base.ServiceUtility.ConvertToDecimal(paxcount_rebo.Split('|')[NN]));
                                                    }
                                                }

                                                PaymentDet_s.Amount = Convert.ToString(amt);
                                                PaymentDet_s.SegRefNumber = "1";
                                                Session.Add("SEGREF", lstoflstOnwardAvail[m][j].SegRef);
                                                PaymentDetail_s.Add(PaymentDet_s);
                                                Session.Add("rebookingstock", itindetail.Stock);
                                                itindetail.Select_Token = _PriceItenaryRS.PriceItenarys[0].Token;

                                                itindetail.SeatsSSR = null;
                                                itindetail.BaggSSR = null;
                                                itindetail.MealsSSR = null;
                                                itindetail.OtherSSR = null;

                                                itindetail.AllowDupBooking = (AllowDuplicateBooking.ToString().ToUpper() == "TRUE" ? true : false);
                                                itindetail.TourCode = torc[i];
                                                itindetail.isStudentFare = Convert.ToBoolean(Regex.Split(Farelist, "SPLITSRCAS")[0]);
                                                itindetail.isArmyFare = Convert.ToBoolean(Regex.Split(Farelist, "SPLITSRCAS")[1]);
                                                itindetail.isSnrCitizenFare = Convert.ToBoolean(Regex.Split(Farelist, "SPLITSRCAS")[2]);
                                                itindetail.isLabourFare = _PriceItenaryRS.PriceItenarys[i].PriceRS[0].Flights[0].FareTypeDescription.ToString().ToUpper() == "LABOUR FARE" ? true : false;
                                                itindetail.CSPNR = (!string.IsNullOrEmpty(CreditshellPNRDetails) ? CreditshellPNRDetails.Split('|')[1].ToString() :"");
                                                itindetail.CSPNR_Amount = (!string.IsNullOrEmpty(CreditshellPNRDetails) ? CreditshellPNRDetails.Split('|')[0].ToString() : "");
                                                itindetail.ALLOW_RQ = (!string.IsNullOrEmpty(_PriceItenaryRS.PriceItenarys[i].ALLOW_RQ) ? _PriceItenaryRS.PriceItenarys[i].ALLOW_RQ.ToString() : "");
                                            }
                                        }
                                        ItineraryDetail.Add(itindetail);
                                    }


                                    #endregion roundtrip rebooking

                                    #region direct booking request formation start


                                    else
                                    {
                                        string[] qry = (from p in dtBookingResponse_bookingreq.AsEnumerable()
                                                        select p["itinRef"].ToString()).Distinct().ToArray();
                                        for (int indexmine = 0; indexmine < qry.Length; indexmine++)
                                        {

                                            var myqry = from p in dtBookingResponse_bookingreq.AsEnumerable()
                                                        where p["itinRef"].ToString().Trim() == qry[indexmine].ToString().Trim()
                                                        select p;

                                            if (myqry.Count() > 0)
                                            {
                                                List<RQRS.Payment> PaymentDetail_s = new List<RQRS.Payment>();
                                                RQRS.Payment PaymentDet_s = new RQRS.Payment();
                                                PaymentDet_s = new RQRS.Payment();
                                                FlightDetail = new List<RQRS.Flights>();
                                                itindetail = new RQRS.ItineraryFlights();
                                                DataTable dtmine = myqry.CopyToDataTable();
                                                foreach (DataRow drrFlights in dtmine.Rows)
                                                {
                                                    flightdet = new RQRS.Flights();
                                                    _FOPDetails = new RQRS.FOPDetails();
                                                    if (passthrow != null && passthrow != "")
                                                    {
                                                        passri = passthrow.Split('~');
                                                    }

                                                    _FOPDetails.CardName = passri != null && passri.Length > 4 ? passri[0] : "";
                                                    _FOPDetails.CardNumber = passri != null && passri.Length > 4 ? passri[2] : "";
                                                    _FOPDetails.CardType = passri != null && passri.Length > 4 ? passri[1] : "";
                                                    _FOPDetails.CVV_Number = passri != null && passri.Length > 4 ? passri[5] : "";
                                                    _FOPDetails.ExpiryDate = passri != null && passri.Length > 4 ? passri[3] + "" + passri[4] : "";


                                                    flightdet.AirlineCategory = drrFlights["AIRLINECATEGORY"].ToString();
                                                    flightdet.ArrivalDateTime = drrFlights["ARRIVALDATE"].ToString();
                                                    flightdet.Class = drrFlights["CLASS"].ToString();
                                                    flightdet.CNX = drrFlights["CNXTYPE"].ToString();
                                                    flightdet.DepartureDateTime = drrFlights["DEPARTUREDATE"].ToString();
                                                    flightdet.Destination = drrFlights["DESTINATION"].ToString();
                                                    flightdet.FareBasisCode = drrFlights["FAREBASISCODE"].ToString();
                                                    flightdet.StartTerminal = drrFlights["ORGTERMINAL"].ToString();
                                                    flightdet.EndTerminal = drrFlights["DESTERMINAL"].ToString();
                                                    flightdet.FlightNumber = drrFlights["FLIGHTNUMBER"].ToString();
                                                    flightdet.FlyingTime = drrFlights["FLYINGTIME"].ToString();
                                                    flightdet.Origin = drrFlights["ORIGIN"].ToString();
                                                    flightdet.PlatingCarrier = drrFlights["PLATINGCARRIER"].ToString();
                                                    flightdet.ReferenceToken = drrFlights["Token"].ToString();
                                                    flightdet.SegRef = drrFlights["SegRef"].ToString();
                                                    lstrtotseg += drrFlights["SegRef"].ToString() + "|";
                                                    flightdet.SegmentDetails = "";

                                                    flightdet.JourneyTime = drrFlights["JourneyTime"].ToString();
                                                    flightdet.OtherFares = "";
                                                    flightdet.MultiClass = "";
                                                    flightdet.Via = drrFlights["Via"] != null && drrFlights["Via"].ToString() != "" ? drrFlights["Via"].ToString() : "";
                                                    flightdet.RBDCode = drrFlights["CLASS"].ToString();
                                                    flightdet.FareId = drrFlights["Faresid"].ToString();
                                                    flightdet.OfflineFlag = Convert.ToInt16(drrFlights["OFFLINEFLAG"].ToString().Trim());
                                                    flightdet.ConnectionFlag = (dtmine.Rows.Count > 1) ? "S" : "N";
                                                    flightdet.TransactionFlag = false;// (allowtransfee == "Y") ? true : false;
                                                    flightdet.Cabin = drrFlights["Cabin"].ToString() != null && drrFlights["Cabin"].ToString() != "" ? drrFlights["Cabin"].ToString() : (Session["segmclass" + ValKey] != null && Session["segmclass" + ValKey].ToString() != "" ? Session["segmclass" + ValKey].ToString() : "E");
                                                    flightdet.Refundable = "";
                                                    flightdet.OperatingCarrier = string.IsNullOrEmpty(drrFlights["Operatincarrier"].ToString()) ? "" : drrFlights["Operatincarrier"].ToString();
                                                    flightdet.PromoCodeDesc = string.IsNullOrEmpty(drrFlights["PromoCodeDesc"].ToString()) ? "" : drrFlights["PromoCodeDesc"].ToString();
                                                    flightdet.PromoCode = string.IsNullOrEmpty(drrFlights["PromoCode"].ToString()) ? "" : drrFlights["PromoCode"].ToString();
                                                    if (mulreq == "Y")
                                                    {
                                                        flightdet.ItinRef = segcnt.ToString();
                                                    }
                                                    else
                                                        flightdet.ItinRef = segcnt.ToString(); // drrFlights["itinRef"].ToString();

                                                    FlightDetail.Add(flightdet);

                                                    Token = drrFlights["TokenBookingKKK"].ToString();  //Session["TokenBooking" + ValKey].ToString();// Session["TokenBooking" + ValKey].ToString(); //string.IsNullOrEmpty(drrFlights["TokenBooking"].ToString()) ? "" : (drrFlights["TokenBooking"].ToString() + "|");



                                                    itindetail.HostID = indexmine.ToString();
                                                    itindetail.FlightDetails = FlightDetail;
                                                    itindetail.FOPDetails = _FOPDetails;
                                                    itindetail.PaymentMode = PaymentMode;
                                                    itindetail.FareType = drrFlights["FareType"].ToString();
                                                    itindetail.FareTypeDescription = drrFlights["FareTypeDescription"].ToString();
                                                    itindetail.FFNumber = FFnumberDetail;
                                                    itindetail.Stock = drrFlights["stocktype"].ToString();   // lststr.Count() > 1 ? (Session["TripType" + ValKey].ToString().Trim() == "R" && index > 0) ? lststr[1] : lststr[i] : lststr[0];//drrFlights["stocktype"].ToString();// Session["Stock" + ValKey].ToString();stocktype
                                                    itindetail.Payment = PaymentDetail_s;
                                                    itindetail.Pricingcode = "";




                                                    //    Decimal amt = Convert.ToDecimal(drrFlights["GrossAmount"].ToString()) + Convert.ToDecimal(drrFlights["MarkUp"].ToString());


                                                    Decimal amt = 0;// Convert.ToDecimal(drrFlights["GrossAmount"].ToString()) + Convert.ToDecimal(drrFlights["MarkUp"].ToString());
                                                    for (int NN = 0; NN < _PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription.Count; NN++)
                                                    {
                                                        if (HideComission == "Y")
                                                        {
                                                            amt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)
                                                                + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFAMOUNT) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFGST)
                                                                + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ServiceFee) - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].Incentive) 
                                                                - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].PLBAmount) - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].Discount)) * Base.ServiceUtility.ConvertToDecimal(paxcount_rebo.Split('|')[NN]));
                                                        }
                                                        else
                                                        {
                                                            amt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)
                                                            + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFAMOUNT) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFGST)
                                                            + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ServiceFee)) * Base.ServiceUtility.ConvertToDecimal(paxcount_rebo.Split('|')[NN]));
                                                        }
                                                    }

                                                    PaymentDet_s.Amount = Convert.ToString(amt);




                                                    PaymentDet_s.SegRefNumber = "1";
                                                    Session.Add("SEGREF", drrFlights["SegRef"].ToString());
                                                    PaymentDetail_s.Add(PaymentDet_s);


                                                    Session.Add("rebookingstock", itindetail.Stock);
                                                    if (mulreq == "Y")
                                                    {
                                                        itindetail.Select_Token = Session["token" + reqcont + ValKey].ToString();
                                                    }
                                                    else
                                                    {
                                                        itindetail.Select_Token = Token.TrimEnd('|');
                                                    }
                                                    itindetail.AllowDupBooking = (AllowDuplicateBooking.ToString().ToUpper() == "TRUE" ? true : false);
                                                    //#endregion
                                                    itindetail.SeatsSSR = null;
                                                    itindetail.BaggSSR = null;
                                                    itindetail.MealsSSR = null;
                                                    itindetail.OtherSSR = null;
                                                    itindetail.TourCode = torc[i];
                                                    itindetail.isStudentFare = Convert.ToBoolean(Regex.Split(Farelist, "SPLITSRCAS")[0]);
                                                    itindetail.isArmyFare = Convert.ToBoolean(Regex.Split(Farelist, "SPLITSRCAS")[1]);
                                                    itindetail.isSnrCitizenFare = Convert.ToBoolean(Regex.Split(Farelist, "SPLITSRCAS")[2]);
                                                    itindetail.isLabourFare = drrFlights["FareTypeDescription"].ToString().ToUpper() == "LABOUR FARE" ? true : false;
                                                    itindetail.CSPNR = (!string.IsNullOrEmpty(CreditshellPNRDetails) ? CreditshellPNRDetails.Split('|')[1].ToString() : "");
                                                    itindetail.CSPNR_Amount = (!string.IsNullOrEmpty(CreditshellPNRDetails) ? CreditshellPNRDetails.Split('|')[0].ToString() : "");
                                                    itindetail.ALLOW_RQ = (!string.IsNullOrEmpty(_PriceItenaryRS.PriceItenarys[i].ALLOW_RQ) ? _PriceItenaryRS.PriceItenarys[i].ALLOW_RQ.ToString() : "");
                                                }

                                                ItineraryDetail.Add(itindetail);
                                            }

                                        }
                                    }


                                    #endregion direct roundtrip booking end
                                }

                                #endregion roundtrip ends

                                else
                                {
                                    List<RQRS.Payment> PaymentDetail_s = new List<RQRS.Payment>();
                                    RQRS.Payment PaymentDet_s = new RQRS.Payment();
                                    if (strRebook != null && strRebook != "")
                                    {
                                        var FDD = from _Flightsd in _PriceItenaryRS.PriceItenarys[0].PriceRS[0].Flights.AsEnumerable()
                                                      //join _Fares in _PriceItenaryRS.PriceRS[0].Fares.AsEnumerable()
                                                      //on _Flightsd.FareId equals _Fares.FlightId
                                                  where _Flightsd.FareId != "" //&& _Fares.FlightId != ""
                                                  select new Travrays_modal.FlightDetails
                                                  {
                                                      FlightNumber = _Flightsd.FlightNumber,
                                                      Origin = _Flightsd.Origin,
                                                      //OriginName = _Flights,
                                                      Destination = _Flightsd.Destination,
                                                      //DestinationName = _Dcity["AN"].ToString(),
                                                      DepartureDate = _Flightsd.DepartureDateTime.Split(' ')[0] + " " + _Flightsd.DepartureDateTime.Split(' ')[1] + " " + _Flightsd.DepartureDateTime.Split(' ')[2],
                                                      DepartureTime = _Flightsd.DepartureDateTime.Split(' ')[3],
                                                      ArrivalDate = _Flightsd.ArrivalDateTime.Split(' ')[0] + " " + _Flightsd.ArrivalDateTime.Split(' ')[1] + " " + _Flightsd.ArrivalDateTime.Split(' ')[2],
                                                      ArrivalTime = _Flightsd.ArrivalDateTime.Split(' ')[3],
                                                      FlyingTime = _Flightsd.FlyingTime,
                                                      Class = _Flightsd.Class,//_Flights["Class"] + "-" + _Flights["FareBasisCode"],
                                                      Cabin = (_Flightsd.Cabin != null && _Flightsd.Cabin != "" ? _Flightsd.Cabin : (Session["segmclass" + ValKey] != null && Session["segmclass" + ValKey].ToString() != "" ? Session["segmclass" + ValKey].ToString() : "E")),
                                                      AvailSeat = _Flightsd.AvailSeat,
                                                      //BG = //_Flightsd["BaggageInfo"],
                                                      //BP = //_Flightsd["BreakPoint"],
                                                      CarrierCode = _Flightsd.PlatingCarrier,//_Flights["CarrierCode"],
                                                      CNX = _Flightsd.CNX,
                                                      ConnectionFlag = _Flightsd.ConnectionFlag,
                                                      //CRSID = _Flightsd.cr,
                                                      FareBasisCode = _Flightsd.FareBasisCode,
                                                      FareId = _Flightsd.FareId,
                                                      // Faredescription = _Fares.Faredescription,
                                                      PlatingCarrier = _Flightsd.PlatingCarrier,
                                                      ReferenceToken = _Flightsd.ReferenceToken,
                                                      SegRef = _Flightsd.SegRef,
                                                      //VIA = _Flightsd.via
                                                      Stops = _Flightsd.Stops,
                                                      via = _Flightsd.Via,
                                                      Refund = _Flightsd.Refundable,
                                                      //Stock = "RST_" + field.RoundtripDomesticFlag,//_availRes.Stock,
                                                      ItinRef = _Flightsd.ItinRef,
                                                      Airlinecategory = _Flightsd.AirlineCategory,
                                                      ClassCarrierCode = _Flightsd.Class + "-" + _Flightsd.FareBasisCode,
                                                      FareType = _PriceItenaryRS.PriceItenarys[0].PriceRS[0].Fares[0].FareType,
                                                      StartTerminal = _Flightsd.StartTerminal,
                                                      EndTerminal = _Flightsd.EndTerminal,
                                                      FareTypeDescription = _Flightsd.FareTypeDescription,
                                                      //FRRN = _Fares.Faredescription[0].
                                                      Operatingcarrier = _Flightsd.OperatingCarrier,
                                                      JourneyTime = _Flightsd.JourneyTime,
                                                      PromoCodeDesc = _Flightsd.PromoCodeDesc,
                                                      PromoCode = _Flightsd.PromoCode
                                                  };

                                        lstoflstOnwardAvail = FDD.GroupBy(u => u.ItinRef).Select(grp => grp.ToList()).ToList();

                                        string farettype = string.Empty;
                                        string Faretypedes = string.Empty;
                                        for (int N = 0; N < lstoflstOnwardAvail.Count; N++)
                                        {
                                            for (int j = 0; j < lstoflstOnwardAvail[N].Count; j++)
                                            {

                                                flightdet = new RQRS.Flights();
                                                _FOPDetails = new RQRS.FOPDetails();

                                                if (passthrow != null && passthrow != "")
                                                {
                                                    passri = passthrow.Split('~');
                                                }

                                                _FOPDetails.CardName = passri != null && passri.Length > 4 ? passri[0] : "";
                                                _FOPDetails.CardNumber = passri != null && passri.Length > 4 ? passri[2] : "";
                                                _FOPDetails.CardType = passri != null && passri.Length > 4 ? passri[1] : "";
                                                _FOPDetails.CVV_Number = passri != null && passri.Length > 4 ? passri[5] : "";
                                                _FOPDetails.ExpiryDate = passri != null && passri.Length > 4 ? passri[3] + "" + passri[4] : "";

                                                flightdet.AirlineCategory = lstoflstOnwardAvail[N][j].Airlinecategory;
                                                flightdet.ArrivalDateTime = lstoflstOnwardAvail[N][j].ArrivalDate + " " + lstoflstOnwardAvail[N][j].ArrivalTime;
                                                flightdet.Class = lstoflstOnwardAvail[N][j].Class;//
                                                flightdet.CNX = lstoflstOnwardAvail[N][j].CNX;//"N";
                                                flightdet.DepartureDateTime = lstoflstOnwardAvail[N][j].DepartureDate + " " + lstoflstOnwardAvail[N][j].DepartureTime;//"25 May 2016 09:05";
                                                flightdet.Destination = lstoflstOnwardAvail[N][j].Destination;//"DEL";
                                                flightdet.FareBasisCode = lstoflstOnwardAvail[N][j].FareBasisCode;//"FHBO";
                                                flightdet.StartTerminal = lstoflstOnwardAvail[N][j].StartTerminal;
                                                flightdet.EndTerminal = lstoflstOnwardAvail[N][j].EndTerminal;
                                                flightdet.FlightNumber = lstoflstOnwardAvail[N][j].FlightNumber;//"";
                                                flightdet.FlyingTime = lstoflstOnwardAvail[N][j].FlyingTime;//"";
                                                flightdet.Origin = lstoflstOnwardAvail[N][j].Origin;//"";
                                                flightdet.PlatingCarrier = lstoflstOnwardAvail[N][j].PlatingCarrier;//"";
                                                flightdet.ReferenceToken = lstoflstOnwardAvail[N][j].ReferenceToken;//"";
                                                flightdet.SegRef = lstoflstOnwardAvail[N][j].SegRef;// "";
                                                lstrtotseg += lstoflstOnwardAvail[N][j].SegRef + "|";
                                                flightdet.SegmentDetails = "";
                                                Session.Add("SEGREF", lstoflstOnwardAvail[N][j].SegRef);
                                                flightdet.JourneyTime = lstoflstOnwardAvail[N][j].JourneyTime;
                                                flightdet.OtherFares = "";
                                                flightdet.MultiClass = "";
                                                flightdet.Via = lstoflstOnwardAvail[N][j].via;
                                                flightdet.RBDCode = lstoflstOnwardAvail[N][j].Class;
                                                flightdet.FareId = lstoflstOnwardAvail[N][j].FareId;//"SG0";
                                                flightdet.OfflineFlag = 0;
                                                flightdet.ConnectionFlag = lstoflstOnwardAvail[N][j].ConnectionFlag;
                                                flightdet.TransactionFlag = false;// (allowtransfee == "Y") ? true : false;
                                                flightdet.Cabin = lstoflstOnwardAvail[N][j].Cabin;
                                                flightdet.Refundable = lstoflstOnwardAvail[N][j].Refund;
                                                flightdet.OperatingCarrier = lstoflstOnwardAvail[N][j].Operatingcarrier;
                                                flightdet.PromoCodeDesc = lstoflstOnwardAvail[N][j].PromoCodeDesc;
                                                flightdet.PromoCode = lstoflstOnwardAvail[N][j].PromoCode;
                                                farettype = lstoflstOnwardAvail[N][j].FareType;
                                                Faretypedes = lstoflstOnwardAvail[N][j].FareTypeDescription;
                                                if (mulreq == "Y")
                                                {
                                                    flightdet.ItinRef = segcnt.ToString();
                                                }
                                                else
                                                    flightdet.ItinRef = segcnt.ToString(); // drrFlights["itinRef"].ToString();
                                                FlightDetail.Add(flightdet);
                                            }

                                        }


                                        Token = Session["TokenBooking" + ValKey].ToString(); //string.IsNullOrEmpty(drrFlights["TokenBooking"].ToString()) ? "" : (drrFlights["TokenBooking"].ToString() + "|");
                                        // Token =drrFlights["TokenBookingKKK"].ToString();
                                        itindetail.HostID = mulreq == "Y" ? segcnt.ToString() : "" + (index);
                                        itindetail.FlightDetails = FlightDetail;
                                        itindetail.FOPDetails = _FOPDetails;
                                        itindetail.PaymentMode = PaymentMode;
                                        itindetail.FareType = farettype;
                                        itindetail.FareTypeDescription = Faretypedes;
                                        Decimal amt = 0;// Convert.ToDecimal(drrFlights["GrossAmount"].ToString()) + Convert.ToDecimal(drrFlights["MarkUp"].ToString());
                                        for (int NN = 0; NN < _PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription.Count; NN++)
                                        {
                                            if (HideComission == "Y")
                                            {
                                                amt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)
                                                    + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFAMOUNT) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFGST)
                                                    + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ServiceFee) - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].Incentive)
                                                    - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].PLBAmount) - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].Discount)) * Base.ServiceUtility.ConvertToDecimal(paxcount_rebo.Split('|')[NN]));
                                            }
                                            else
                                            {
                                                amt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)
                                                        + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFAMOUNT) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFGST)
                                                        + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ServiceFee)) * Base.ServiceUtility.ConvertToDecimal(paxcount_rebo.Split('|')[NN]));
                                            }
                                        }

                                        PaymentDet_s.Amount = Convert.ToString(amt);
                                        PaymentDet_s.SegRefNumber = "1";

                                        PaymentDetail_s.Add(PaymentDet_s);
                                        itindetail.Payment = PaymentDetail_s;
                                        itindetail.Pricingcode = "";

                                        itindetail.FFNumber = FFnumberDetail;
                                        itindetail.Stock = lststr.Count() > 1 ? (Session["TripType" + ValKey].ToString().Trim() == "R" && index > 0) ? lststr[1] : lststr[i] : lststr[0];//drrFlights["stocktype"].ToString();// Session["Stock" + ValKey].ToString();stocktype
                                        Session.Add("rebookingstock", itindetail.Stock);
                                        if (mulreq == "Y")
                                        {
                                            itindetail.Select_Token = _PriceItenaryRS.PriceItenarys[0].Token;
                                        }
                                        else
                                        {
                                            itindetail.Select_Token = _PriceItenaryRS.PriceItenarys[0].Token;
                                        }
                                        itindetail.AllowDupBooking = (AllowDuplicateBooking.ToString().ToUpper() == "TRUE" ? true : false);
                                        itindetail.SeatsSSR = null;
                                        itindetail.BaggSSR = null;
                                        itindetail.MealsSSR = null;
                                        itindetail.OtherSSR = null;
                                        itindetail.TourCode = torc[i];
                                    }
                                    else
                                    {
                                        Decimal itenmanul = 0;
                                        foreach (DataRow drrFlights in _bookflightds.Tables[0].Rows)
                                        {
                                            flightdet = new RQRS.Flights();
                                            _FOPDetails = new RQRS.FOPDetails();
                                            if (passthrow != null && passthrow != "")
                                            {
                                                passri = passthrow.Split('~');
                                            }

                                            _FOPDetails.CardName = passri != null && passri.Length > 4 ? passri[0] : "";
                                            _FOPDetails.CardNumber = passri != null && passri.Length > 4 ? passri[2] : "";
                                            _FOPDetails.CardType = passri != null && passri.Length > 4 ? passri[1] : "";
                                            _FOPDetails.CVV_Number = passri != null && passri.Length > 4 ? passri[5] : "";
                                            _FOPDetails.ExpiryDate = passri != null && passri.Length > 4 ? passri[3] + "" + passri[4] : "";

                                            PaymentDetail_s = new List<RQRS.Payment>();
                                            flightdet.AirlineCategory = drrFlights["AIRLINECATEGORY"].ToString();
                                            flightdet.ArrivalDateTime = drrFlights["ARRIVALDATE"].ToString();
                                            flightdet.Class = drrFlights["CLASS"].ToString();
                                            flightdet.CNX = drrFlights["CNXTYPE"].ToString();
                                            flightdet.DepartureDateTime = drrFlights["DEPARTUREDATE"].ToString();
                                            flightdet.Destination = drrFlights["DESTINATION"].ToString();
                                            flightdet.FareBasisCode = drrFlights["FAREBASISCODE"].ToString();
                                            flightdet.StartTerminal = drrFlights["ORGTERMINAL"].ToString();
                                            flightdet.EndTerminal = drrFlights["DESTERMINAL"].ToString();
                                            flightdet.FlightNumber = drrFlights["FLIGHTNUMBER"].ToString();
                                            flightdet.FlyingTime = drrFlights["FLYINGTIME"].ToString();
                                            flightdet.Origin = drrFlights["ORIGIN"].ToString();
                                            flightdet.PlatingCarrier = drrFlights["PLATINGCARRIER"].ToString();
                                            flightdet.ReferenceToken = drrFlights["Token"].ToString();
                                            flightdet.SegRef = drrFlights["SegRef"].ToString();
                                            lstrtotseg += drrFlights["SegRef"].ToString() + "|";
                                            flightdet.SegmentDetails = "";

                                            flightdet.JourneyTime = drrFlights["JourneyTime"].ToString();
                                            flightdet.OtherFares = "";
                                            flightdet.MultiClass = "";
                                            flightdet.Via = drrFlights["Via"] != null && drrFlights["Via"].ToString() != "" ? drrFlights["Via"].ToString() : "";
                                            flightdet.RBDCode = drrFlights["CLASS"].ToString();
                                            flightdet.FareId = drrFlights["Faresid"].ToString();
                                            flightdet.OfflineFlag = Convert.ToInt16(drrFlights["OFFLINEFLAG"].ToString().Trim());
                                            flightdet.ConnectionFlag = (dsBookingDetails.Tables["TrackFareDetails"].Rows.Count > 1) ? "S" : "N";
                                            flightdet.TransactionFlag = false;// (allowtransfee == "Y") ? true : false;
                                            flightdet.Cabin = drrFlights["Cabin"].ToString() != null && drrFlights["Cabin"].ToString() != "" ? drrFlights["Cabin"].ToString() : (Session["segmclass" + ValKey] != null && Session["segmclass" + ValKey].ToString() != "" ? Session["segmclass" + ValKey].ToString() : "E");
                                            flightdet.Refundable = "";
                                            flightdet.OperatingCarrier = string.IsNullOrEmpty(drrFlights["Operatincarrier"].ToString()) ? "" : drrFlights["Operatincarrier"].ToString();
                                            flightdet.PromoCodeDesc = string.IsNullOrEmpty(drrFlights["PromoCodeDesc"].ToString()) ? "" : drrFlights["PromoCodeDesc"].ToString();
                                            flightdet.PromoCode = string.IsNullOrEmpty(drrFlights["PromoCode"].ToString()) ? "" : drrFlights["PromoCode"].ToString();
                                            if (mulreq == "Y")
                                            {
                                                flightdet.ItinRef = segcnt.ToString();
                                            }
                                            else
                                                if (drrFlights["CNXTYPE"].ToString() == "N")
                                            {
                                                flightdet.ItinRef = drrFlights["itinRef"].ToString(); // drrFlights["itinRef"].ToString();
                                                itenmanul++;
                                            }
                                            else
                                            {
                                                flightdet.ItinRef = drrFlights["itinRef"].ToString(); // drrFlights["itinRef"].ToString();
                                            }


                                            FlightDetail.Add(flightdet);

                                            // Token = Session["TokenBooking" + ValKey].ToString(); //string.IsNullOrEmpty(drrFlights["TokenBooking"].ToString()) ? "" : (drrFlights["TokenBooking"].ToString() + "|");
                                            Token = drrFlights["TokenBookingKKK"].ToString();
                                            itindetail.HostID = mulreq == "Y" ? segcnt.ToString() : "" + (index);
                                            itindetail.FlightDetails = FlightDetail;
                                            itindetail.FOPDetails = _FOPDetails;
                                            itindetail.PaymentMode = PaymentMode;
                                            itindetail.FareType = drrFlights["FareType"] != null && drrFlights["FareType"].ToString() != "" ? drrFlights["FareType"].ToString() : "";
                                            itindetail.FareTypeDescription = drrFlights["FareTypeDescription"] != null && drrFlights["FareTypeDescription"] != "" ? drrFlights["FareTypeDescription"].ToString() : "";
                                            Decimal amt = 0;// Convert.ToDecimal(drrFlights["GrossAmount"].ToString()) + Convert.ToDecimal(drrFlights["MarkUp"].ToString());
                                            string[] grosssplt = drrFlights["GrossAmount"].ToString().Split('|');
                                            string[] mrkupplt = drrFlights["MarkUp"].ToString().Split('|');
                                            //    amt = Convert.ToDecimal(Session["totamt" + ValKey]);

                                            for (int NN = 0; NN < _PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription.Count; NN++)
                                            {
                                                if (HideComission == "Y")
                                                {
                                                    amt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)
                                                        + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFAMOUNT) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFGST)
                                                        + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ServiceFee) - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].Incentive)
                                                        - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].PLBAmount) - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].Discount)) * Base.ServiceUtility.ConvertToDecimal(paxcount_rebo.Split('|')[NN]));
                                                }
                                                else
                                                {
                                                    amt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)
                                                        + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFAMOUNT) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].SFGST)
                                                        + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[i].PriceRS[0].Fares[0].Faredescription[NN].ServiceFee)) * Base.ServiceUtility.ConvertToDecimal(paxcount_rebo.Split('|')[NN]));
                                                }
                                            }

                                            PaymentDet_s.Amount = Convert.ToString(amt);
                                            PaymentDet_s.SegRefNumber = "1";
                                            Session.Add("SEGREF", drrFlights["SegRef"].ToString());
                                            PaymentDetail_s.Add(PaymentDet_s);
                                            itindetail.Payment = PaymentDetail_s;
                                            itindetail.Pricingcode = "";

                                            itindetail.FFNumber = FFnumberDetail;
                                            itindetail.Stock = lststr.Count() > 1 ? (Session["TripType" + ValKey].ToString().Trim() == "R" && index > 0) ? lststr[1] : lststr[i] : lststr[0];//drrFlights["stocktype"].ToString();// Session["Stock" + ValKey].ToString();stocktype
                                            Session.Add("rebookingstock", itindetail.Stock);

                                            if (mulreq == "Y")
                                            {
                                                itindetail.Select_Token = Session["token" + reqcont + ValKey].ToString();
                                            }
                                            else
                                            {
                                                itindetail.Select_Token = Token.TrimEnd('|');
                                            }
                                            itindetail.AllowDupBooking = (AllowDuplicateBooking.ToString().ToUpper() == "TRUE" ? true : false);
                                            itindetail.SeatsSSR = null;
                                            itindetail.BaggSSR = null;
                                            itindetail.MealsSSR = null;
                                            itindetail.OtherSSR = null;
                                            itindetail.TourCode = torc[i];
                                            itindetail.isStudentFare = Convert.ToBoolean(Regex.Split(Farelist, "SPLITSRCAS")[0]);
                                            itindetail.isArmyFare = Convert.ToBoolean(Regex.Split(Farelist, "SPLITSRCAS")[1]);
                                            itindetail.isSnrCitizenFare = Convert.ToBoolean(Regex.Split(Farelist, "SPLITSRCAS")[2]);
                                            itindetail.isLabourFare = drrFlights["FareTypeDescription"].ToString().ToUpper() == "LABOUR FARE" ? true : false;
                                            itindetail.CSPNR = (!string.IsNullOrEmpty(CreditshellPNRDetails) ? CreditshellPNRDetails.Split('|')[1].ToString() : "");
                                            itindetail.CSPNR_Amount = (!string.IsNullOrEmpty(CreditshellPNRDetails) ? CreditshellPNRDetails.Split('|')[0].ToString() : "");
                                            itindetail.ALLOW_RQ = (!string.IsNullOrEmpty(_PriceItenaryRS.PriceItenarys[i].ALLOW_RQ) ? _PriceItenaryRS.PriceItenarys[i].ALLOW_RQ.ToString() : "");
                                        }
                                    }
                                    ItineraryDetail.Add(itindetail);
                                }

                            }
                            segcnt++;
                        }
                    }
                }

                #region Seat_map

                List<RQRS.SeatsSSR> SeatsSSR = new List<RQRS.SeatsSSR>();

                DataTable seattable = new DataTable();

                seattable = (DataTable)Session["Response" + ValKey];

                Seatvalue = Seatvalue.TrimEnd('$'); decimal SeatAmunt = 0;

                if (Seatvalue != "" && Seatvalue != null)
                {

                    if (Seatvalue.Contains('$'))
                    {

                        string[] seat_response = Seatvalue.Split('$');
                        for (var sm = 0; sm < seat_response.Length; sm++)
                        {
                            if (seat_response[sm] != "")
                            {
                                string[] splitres = seat_response[sm].Split('~');
                                RQRS.SeatsSSR seatmapdet = new RQRS.SeatsSSR();
                                int segref = Convert.ToInt16(splitres[1]);

                                int paxid = Convert.ToInt16(splitres[0]);
                                string Describ = splitres[4].ToString();
                                string seatno = splitres[2].ToString();
                                string seatamt = splitres[3].ToString();
                                string seatref = splitres[5].ToString();
                                seatmapdet.Orgin = splitres[7];// dtBookingResponse.Rows[segref - 1]["Origin"].ToString();
                                seatmapdet.Destination = splitres[8];// dtBookingResponse.Rows[segref - 1]["Destination"].ToString();
                                seatmapdet.PaxRefNumber = paxid.ToString();
                                seatmapdet.SeatAmount = seatamt;
                                //seatmapdet.SeatCode = seatno;
                                seatmapdet.SeatCode = seatref.Replace("DoTr", ".");
                                seatmapdet.SegRefNumber = segref.ToString();
                                seatmapdet.Description = Describ;
                                seatmapdet.Itinref = splitres[6];// dtBookingResponse.Rows[sm]["itinRef"].ToString();
                                seatmapdet.SeatrefAPI = splitres[9] ?? "";
                                SeatsSSR.Add(seatmapdet);
                                SeatAmunt += Convert.ToDecimal(seatamt);
                            }
                        }

                    }
                    else
                    {
                        RQRS.SeatsSSR seatmapdet = new RQRS.SeatsSSR();
                        string[] splitres = Seatvalue.Split('~');
                        int segref = Convert.ToInt16(splitres[1]);
                        int paxid = Convert.ToInt16(splitres[0]);
                        string Describ = splitres[4].ToString();
                        string seatno = splitres[2].ToString();
                        string seatamt = splitres[3].ToString();
                        string seatref = splitres[5].ToString();
                        seatmapdet.Orgin = splitres[7];//dtBookingResponse.Rows[segref - 1]["Origin"].ToString();
                        seatmapdet.Destination = splitres[8];// dtBookingResponse.Rows[segref - 1]["Destination"].ToString();
                        seatmapdet.PaxRefNumber = paxid.ToString();
                        seatmapdet.SeatAmount = seatamt;
                        // seatmapdet.SeatCode = seatno;
                        seatmapdet.SeatCode = seatref.Replace("DoTr", ".");
                        seatmapdet.SegRefNumber = segref.ToString();
                        seatmapdet.Description = Describ;
                        seatmapdet.Itinref = splitres[6];//"0";
                        seatmapdet.SeatrefAPI = splitres[9] ?? "";
                        SeatsSSR.Add(seatmapdet);
                        SeatAmunt += Convert.ToDecimal(seatamt);
                    }
                }
                #endregion

                #region OtherSSR //otherssr

                List<RQRS.OtherSSR> OtherSSR = new List<RQRS.OtherSSR>();

                otherssr = otherssr.TrimEnd('~'); decimal OtherSSRAMOUNT = 0;

                if (otherssr != "" && otherssr != null)
                {

                    if (otherssr.Contains('~'))
                    {

                        string[] Other_SSR = otherssr.Split('~');
                        for (var sm = 0; sm < Other_SSR.Length; sm++)
                        {
                            if (Other_SSR[sm] != null && Other_SSR[sm].IndexOf("0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0") == -1)
                            {
                                string[] splitres = Regex.Split(Other_SSR[sm], "WEbMeaLWEB");
                                // string[] splitres = Other_SSR[sm].Split('WEbMeaLWEB');
                                RQRS.OtherSSR OthersssR = new RQRS.OtherSSR();
                                //int segref = Convert.ToInt16(splitres[2]);
                                string segref = splitres[2].ToString();
                                string Describ = splitres[4].ToString();
                                string ssramt = splitres[3].ToString();
                                string SSRcode = splitres[0].ToString();
                                OthersssR.PaxRefNumber = splitres[6].ToString();
                                OthersssR.Orgin = splitres[4].ToString();
                                OthersssR.Destination = splitres[5].ToString().Replace('/', ' ');
                                OthersssR.Amount = ssramt;
                                OthersssR.SSRCode = SSRcode;
                                OthersssR.SegRefNumber = segref.ToString();
                                OthersssR.Description = splitres[0].ToString().Split('|')[0];
                                OthersssR.Itinref = splitres[1];
                                OtherSSRAMOUNT += Convert.ToDecimal(ssramt);
                                OtherSSR.Add(OthersssR);
                            }
                        }

                    }
                    else
                    {
                        if (otherssr.IndexOf("0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0") == -1)
                        {
                            string[] splitres = Regex.Split(otherssr, "WEbMeaLWEB");
                            RQRS.OtherSSR OthersssR = new RQRS.OtherSSR();
                            //int segref = Convert.ToInt16(splitres[2]);
                            string segref = splitres[2].ToString();
                            string Describ = splitres[4].ToString();
                            string ssramt = splitres[3].ToString();
                            string SSRcode = splitres[0].ToString();
                            OthersssR.PaxRefNumber = splitres[6].ToString();
                            OthersssR.Orgin = splitres[4].ToString();
                            OthersssR.Destination = splitres[5].ToString().Replace('/', ' ');
                            OthersssR.Amount = ssramt;
                            OthersssR.SSRCode = SSRcode;
                            OthersssR.SegRefNumber = segref.ToString();
                            OthersssR.Description = splitres[0].ToString().Split('|')[0];
                            OthersssR.Itinref = splitres[1];
                            OtherSSRAMOUNT += Convert.ToDecimal(ssramt);
                            OtherSSR.Add(OthersssR);
                        }
                    }
                }

                #endregion



                Booking.Token = ""; // Token.TrimEnd('|');
                Booking.TourCode = "";//txtTourCode.Text.Trim();

                if (mulreq == "Y" && SegmentType == "D")
                {
                    Booking.TripType = "O"; //MULTI city domastic booking pass oneway request

                }
                else
                {
                    //Booking.TripType = (Session["TripType" + ValKey].ToString() == "Y") ? "R" : Session["TripType" + ValKey].ToString();
                    // Booking.TripType = Session["TripType" + ValKey].ToString();
                    Booking.TripType = (strProduct == "RIYA" && SegmentType == "I" && Session["TripType" + ValKey].ToString() == "Y") ? "R" : (strProduct != "RIYA" && Session["TripType" + ValKey].ToString() == "Y" ? "R" : Session["TripType" + ValKey].ToString());
                }


                Booking.SegmentType = SegmentType;
                Booking.TerminalType = (Session["Bookapptype"] != null && Session["Bookapptype"].ToString() != "") ? Session["Bookapptype"].ToString() : strTerminalType;


                /** Loading Pax details **/

                #region SSR_DETAILS
                DataTable Baggage_s = (DataTable)Session["Baggageselect" + ValKey];
                List<RQRS.MealsSSR> LstMealsDetail_s = new List<RQRS.MealsSSR>();

                RQRS.MealsSSR MealsDet_s = new RQRS.MealsSSR();

                List<RQRS.BaggSSR> LstBaggDetail_s = new List<RQRS.BaggSSR>();
                RQRS.BaggSSR BaggDet_s = new RQRS.BaggSSR();
                Decimal MealAm_s = 0, BagAm_s = 0;
                string lstrpaxbagg_s = "";
                string lstrSegbagg_s = "";
                //bool mealflag = false;
                bool baggflag_s = false;
                string roundtip_double_itnref = "";

                #region ssrs
                try
                {
                    foreach (var drpass in SSR_Val)
                    {
                        //int desc = 0;
                        string MealCode = drpass.Value;
                        string PaxRefNumber = drpass.Key.ToString();
                        string lstrbaggsegref = "";
                        string lstrbaggitinref = "";
                        if (MealCode.Contains('~'))
                        {
                            string[] MealSplit = MealCode.Split('~');

                            string lstrsegref = "";
                            for (int incr = 0; incr < MealSplit.Length; incr++)
                            {
                                lstrsegref = "";
                                baggflag_s = false;
                                if (MealSplit[incr].Contains("SpLiTSSR"))
                                {
                                    string[] Me = Regex.Split(MealSplit[incr], "SpLiTSSR");
                                    if (Session["_Meals" + ValKey].ToString() == "True")
                                    {
                                        if (Me[0] != "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" && Me[0] != "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0")
                                        {
                                            if (Me[0].Contains("WEbMeaLWEB"))
                                            {
                                                string[] Refmeal = Regex.Split(Me[0], "WEbMeaLWEB");
                                                MealsDet_s = new RQRS.MealsSSR();
                                                MealsDet_s.MealCode = Refmeal[0].ToString().Replace("@", "|").Replace("'", "");
                                                MealsDet_s.MealAmount = Refmeal[1].ToString();
                                                MealsDet_s.PaxRefNumber = PaxRefNumber;
                                                MealsDet_s.SegRefNumber = Refmeal[2].ToString();
                                                MealsDet_s.Orgin = Regex.Split(Refmeal[4].ToString(), "MEALSRSPLITbaGG")[1];
                                                MealsDet_s.Destination = Regex.Split(Refmeal[4].ToString(), "MEALSRSPLITbaGG")[2];
                                                MealsDet_s.Itinref = roundtip_double_itnref != null && roundtip_double_itnref != "" ? roundtip_double_itnref.Replace("'", "") : Refmeal[3].ToString().Replace("'", "");
                                                LstMealsDetail_s.Add(MealsDet_s);
                                                MealAm_s += Convert.ToDecimal(Refmeal[1]);
                                                lstrsegref = Refmeal[2].ToString();

                                            }
                                        }
                                    }
                                    if (Session["_Baggage" + ValKey].ToString() == "True")
                                    {
                                        if (Me[1] != "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0" && Me[1] != "0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0")
                                        {
                                            if (Me[1].Contains("WEbMeaLWEB"))
                                            {
                                                //string[] Refmeal = Me[1].Split('-');
                                                string[] Refmeal = Regex.Split(Me[1], "WEbMeaLWEB");
                                                BaggDet_s = new RQRS.BaggSSR();
                                                BaggDet_s.BaggCode = Refmeal[0].ToString().Replace("@", "|").Replace("'", "");
                                                BaggDet_s.BaggAmount = Refmeal[1].ToString();
                                                BaggDet_s.PaxRefNumber = PaxRefNumber;
                                                // BaggDet.SegRefNumber = Refmeal[2].ToString();
                                                BaggDet_s.SegRefNumber = Regex.Split(Refmeal[2].ToString(), "BAggSPLITbaGG")[0];
                                                BaggDet_s.Orgin = Regex.Split(Refmeal[2].ToString(), "BAggSPLITbaGG")[1];
                                                BaggDet_s.Destination = Regex.Split(Refmeal[2].ToString(), "BAggSPLITbaGG")[2];
                                                BaggDet_s.Description = Regex.Split(Refmeal[2].ToString(), "BAggSPLITbaGG")[3];
                                                BaggDet_s.Itinref = roundtip_double_itnref != null && roundtip_double_itnref != "" ? roundtip_double_itnref.Replace("'", "") : Refmeal[3].ToString().Replace("'", "");
                                                try
                                                {
                                                    var qrybaggMsg = (from m in Baggage_s.AsEnumerable()
                                                                      where m["SegRef"].ToString().Trim() == Regex.Split(Refmeal[2].ToString(), "BAggSPLITbaGG")[0] && m["Itinref"].ToString().Trim() == Refmeal[3].ToString().Replace("'", "") && m["BaggageCode"].ToString().Trim() == Refmeal[0].ToString().Replace("'", "")
                                                                      select new
                                                                      {
                                                                          BaggageMsg = m["BaggageText"]
                                                                      }).Distinct();
                                                    if (qrybaggMsg.Count() > 0)
                                                    {
                                                        DataTable dtbaggmsg = Serv.ConvertToDataTable(qrybaggMsg);
                                                        BaggDet_s.BaggageText = dtbaggmsg.Rows[0]["BaggageMsg"].ToString();
                                                    }
                                                    else
                                                    {
                                                        BaggDet_s.BaggageText = "";
                                                    }

                                                }
                                                catch (Exception bagg)
                                                {
                                                    BaggDet_s.BaggageText = "";
                                                }

                                                lstrSegbagg_s += (lstrpaxbagg_s.Contains(PaxRefNumber) ? (Refmeal[2].ToString() + "|") :
                                                    ((lstrSegbagg_s.Trim() == "" ? "" : "~") + PaxRefNumber + "-" + (Refmeal[2].ToString() + "|")));
                                                lstrpaxbagg_s += (lstrpaxbagg_s.Contains(PaxRefNumber) ? "" : (PaxRefNumber + "|"));
                                                LstBaggDetail_s.Add(BaggDet_s);
                                                BagAm_s += Convert.ToDecimal(Refmeal[1]);
                                                lstrbaggsegref += Regex.Split(Refmeal[2].ToString(), "BAggSPLITbaGG")[0] + "|";
                                                lstrbaggitinref += roundtip_double_itnref != null && roundtip_double_itnref != "" ? roundtip_double_itnref.Replace("'", "") : Refmeal[3].ToString().Replace("'", "") + "|";
                                                baggflag_s = true;
                                            }
                                        }
                                    }
                                    if (Me[2] != "")
                                    {
                                        lstrsegref = "";
                                        if (Regex.Split(Me[2], "WEbMeaLWEB").Length == 4 && Regex.Split(Me[2], "WEbMeaLWEB")[2] != "" && Regex.Split(Me[2], "WEbMeaLWEB")[0] != "undefined")
                                        {
                                            //string[] Refmeal = Me[2].Split('-');
                                            string[] Refmeal = Regex.Split(Me[2], "WEbMeaLWEB");
                                            FFdet = new RQRS.FFNumber();
                                            FFdet.AirlineCode = Refmeal[0].ToString();
                                            FFdet.FlyerNumber = Refmeal[2].ToString();
                                            FFdet.PaxRefNumber = PaxRefNumber;
                                            FFdet.SegRefNumber = Refmeal[1].ToString();
                                            FFdet.Itinref = Refmeal[3].ToString();
                                            lstrsegref = Refmeal[1].ToString();
                                            FFnumberDetail.Add(FFdet);
                                        }
                                    }
                                }
                            }
                        }


                        if (lstrpaxref.Contains(PaxRefNumber) && Baggage_s.Rows.Count > 0)
                        {
                            var qrybagg = (from p in Baggage_s.AsEnumerable()
                                           where Convert.ToDouble(p["BaggageAmt"]).ToString().Trim() == "0" && p["BaggageCode"].ToString().Trim() != "0" &&
                                           (!lstrbaggsegref.ToUpper().Contains(p["SegRef"].ToString().Trim()) || (!lstrbaggitinref.ToUpper().Contains(p["Itinref"].ToString().Trim())))
                                           orderby p["BaggageDesc"] descending
                                           select p).Distinct();

                            if (qrybagg.Count() > 0)
                            {
                                DataTable dtbagg = qrybagg.CopyToDataTable();
                                baggflag_s = true;
                                string lstrgr = "";
                                foreach (DataRow item in dtbagg.Rows)
                                {
                                    if (lstrgr.Contains(item["SegRef"].ToString()))
                                        continue;
                                    BaggDet_s = new RQRS.BaggSSR();
                                    BaggDet_s.BaggCode = item["BaggageCode"].ToString().Replace("@", "|").Replace("'", "");
                                    BaggDet_s.BaggAmount = item["BaggageAmt"].ToString().Trim();
                                    BaggDet_s.PaxRefNumber = PaxRefNumber;
                                    BaggDet_s.SegRefNumber = item["SegRef"].ToString();
                                    lstrgr += item["SegRef"].ToString() + "|";
                                    BaggDet_s.Orgin = Regex.Split(item["Addsegorg"].ToString(), "BAggSPLITbaGG")[1];
                                    BaggDet_s.Destination = Regex.Split(item["Addsegorg"].ToString(), "BAggSPLITbaGG")[2];
                                    BaggDet_s.Description = Regex.Split(item["Addsegorg"].ToString(), "BAggSPLITbaGG")[3];
                                    BaggDet_s.Itinref = roundtip_double_itnref != null && roundtip_double_itnref != "" ? roundtip_double_itnref.Replace("'", "") : item["ItinRef"].ToString().Replace("'", "");
                                    LstBaggDetail_s.Add(BaggDet_s);
                                    BagAm_s += Convert.ToDecimal(item["BaggageAmt"].ToString().Trim());
                                }
                            }
                        }


                    }


                    lstrpaxbagg_s = lstrpaxbagg_s.TrimEnd('|');
                    lstrSegbagg_s = lstrSegbagg_s.TrimEnd('|');
                    lstrpaxref = lstrpaxref.TrimEnd('|');
                }
                catch (Exception meal)
                {
                    DatabaseLog.LogData(strUserName, "X", "FormBookingRequest Meals_Baggage", "BOOKING meals baggage", meal.ToString() + " | " + meal.StackTrace.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                #endregion

                #endregion

                List<RQRS.Payment> PaymentDetail = new List<RQRS.Payment>();

                RQRS.Payment PaymentDet = new RQRS.Payment();
                Decimal TotAmnt = 0;  // BagAm + MealAm + Convert.ToDecimal(Session["totamt" + ValKey]);
                if (PaymentMode.ToUpper().Trim() == "P")
                {
                    TotAmnt = 0;
                    for (int j = 0; j < _PriceItenaryRS.PriceItenarys.Count; j++)
                    {
                        for (int NN = 0; NN < _PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription.Count; NN++)
                        {
                            string strPaxCunt = Base.ServiceUtility.SplitPaxCount(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].Paxtype, paxcount_rebo);
                            if (HideComission == "Y")
                            {
                                TotAmnt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)
                                    + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].SFAMOUNT) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].SFGST)
                                    + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].ServiceFee) - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].Incentive)
                                    - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].PLBAmount) - Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].Discount)) * Base.ServiceUtility.ConvertToDecimal(strPaxCunt));
                            }
                            else
                            {
                                TotAmnt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)
                                    + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].SFAMOUNT) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].SFGST)
                                    + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[NN].ServiceFee)) * Base.ServiceUtility.ConvertToDecimal(strPaxCunt));
                            }
                        }
                    }

                    decimal InsAmount = 0;
                    if (PaymentMode.ToUpper().Trim() == "P" && strProduct == "RIYA" && Insdetails != "")
                    {
                        InsAmount = ALLpaxcount_rebo * Convert.ToDecimal((ConfigurationManager.AppSettings["BAJAJETRAVELINS"].ToString()).ToString());
                    }

                    TotAmnt = BagAm_s + MealAm_s + OtherSSRAMOUNT + SeatAmunt + TotAmnt + InsAmount;
                    // TotAmnt = TotAmnt - (string.IsNullOrEmpty(service) ? 0 : Convert.ToDecimal(service));
                    PaymentDet.Amount = Convert.ToString(TotAmnt);
                    PaymentDet.SegRefNumber = "1";

                    PaymentDetail.Add(PaymentDet);
                }

                if (booktype == "BF")
                {
                    PaymentDet.Amount = Bargainfare;
                    PaymentDet.SegRefNumber = "1";
                    PaymentDetail.Add(PaymentDet);
                }

                #region Contactdetails
                List<RQRS.ContactDetails> ContacDetail = new List<RQRS.ContactDetails>();
                string[] ContactDt = Regex.Split(ContactDet, "SPLITSCRIPT");
                RQRS.ContactDetails contactDet = new RQRS.ContactDetails();
                try
                {
                    if (ContactDt.Length > 0)
                    {
                        contactDet.Title = ContactDt[0];
                        if (ContactDt[1] != null && ContactDt[1] != "")
                        {
                            contactDet.ContactNumber = ContactDt[1];
                        }
                        else
                        {
                            contactDet.ContactNumber = "";
                        }
                        contactDet.EmailID = (ContactDt[2] != null && ContactDt[2] != "") ? ContactDt[2] : ConfigurationManager.AppSettings["Bookingagntmailid"].ToString();
                        //contactDet.Firstname = ContactDt[3] != null && ContactDt[3] != "" ? Regex.Replace(ContactDt[3], @"[^0-9a-zA-Z\\s]+$", "") : Regex.Replace(ContactDt[4], @"[^0-9a-zA-Z\\s]+$", "");// ContactDt[4];
                        contactDet.Firstname = ContactDt[3] != null && ContactDt[3] != "" ? Regex.Replace(ContactDt[3], @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+", "") : Regex.Replace(ContactDt[4], @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+", "");// ContactDt[4];
                        // contactDet.Lastname = Regex.Replace(ContactDt[4], @"[^0-9a-zA-Z\\s]+$", "");// ContactDt[4];
                        contactDet.Lastname = Regex.Replace(ContactDt[4], @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+", "");// ContactDt[4];
                        contactDet.Remarks = ContactDt[6];
                        Session.Add("remarks_ss", ContactDt[6]);
                        contactDet.AgencyName = Session["agencyname"].ToString();
                        contactDet.CountryCode = "";
                        contactDet.StateID = ContactDt[7];
                        contactDet.CityId = ContactDt[8] != null && ContactDt[8] != "" ? ContactDt[8] : "";
                        contactDet.CountryID = ConfigurationManager.AppSettings["COUNTRY"].ToString();
                        contactDet.Address = ContactDt[10];
                        contactDet.Pincode = "";

                        if (ContactDt[11].ToString() == "")
                            contactDet.PhoneAgency = "";
                        else
                            contactDet.PhoneAgency = ConfigurationManager.AppSettings["Countryphonecode"].ToString() + "|" + ContactDt[11].Trim('+');
                        if (ContactDt[12].ToString() == "")
                            contactDet.PhoneBusiness = "";
                        else
                            contactDet.PhoneBusiness = ConfigurationManager.AppSettings["Countryphonecode"].ToString() + "|" + ContactDt[12].Trim('+');
                        if (ContactDt[13].ToString() == "")
                            contactDet.PhoneHome = "";
                        else
                            contactDet.PhoneHome = ConfigurationManager.AppSettings["Countryphonecode"].ToString() + "|" + ContactDt[13].Trim('+');
                        contactDet.UserName = ContactDt[14];
                        ContacDetail.Add(contactDet);

                        ViewBag.contactdet = ContactDt[2] + "~" + ContactDt[4] + "~" + ContactDt[1] + "~" + PaymentMode;
                        Session.Add("contactdet", ContactDt[2] + "~" + ContactDt[4] + "~" + ContactDt[1] + "~" + PaymentMode);
                    }
                }
                catch (Exception ee)
                {
                    DatabaseLog.LogData(strUserName, "X", "FormBookingRequest", "BOOKING ContacDetail", ee.ToString() + " | " + ee.StackTrace.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }

                #endregion

                string[] PaxCounts = PaxCount.Split('|');
                Session.Add("PaxCounts" + ValKey, PaxCounts);
                if (PaxCounts.Length > 0)
                {
                    adultCount = Convert.ToInt16(PaxCounts[0]);
                    childCount = Convert.ToInt16(PaxCounts[1]);
                    infantCount = Convert.ToInt16(PaxCounts[2]);
                }
                if (strTerminalType == "T" && ClientID != "")
                {
                    strClientID = ClientID.Substring(0, (ClientID.Length - 2));
                    strClientTerminalID = ClientID;
                }
                else
                {
                    strClientID = Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                    strClientTerminalID = Session["POS_TID"] != null && Session["POS_TID"] != "" ? Session["POS_TID"].ToString() : "";
                }

                RQRS.AgentDetails agentDet = new RQRS.AgentDetails();
                agentDet.AgentId = strClientID;
                agentDet.Agenttype = strTerminalType == "T" ? strClientType : strAgentType;
                agentDet.AirportID = SegmentType;
                agentDet.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();

                agentDet.AppType = "B2B";
                agentDet.BOAID = strClientID;
                agentDet.BOAterminalID = strClientTerminalID;
                agentDet.BranchID = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                Session.Add("ClientBranchID", agentDet.BranchID);
                agentDet.IssuingBranchID = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                agentDet.ClientID = strClientID;
                agentDet.CoOrdinatorID = "";
                agentDet.Environment = (Session["Bookapptype"] != null && Session["Bookapptype"].ToString() != "") ? Session["Bookapptype"].ToString() : strTerminalType; ;
                agentDet.Platform = "B"; //ABCD
                agentDet.ProjectID = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"] != "" ? Session["PRODUCT_CODE"].ToString() : "";
                agentDet.TerminalId = strClientTerminalID;
                agentDet.UserName = strUserName;
                agentDet.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                agentDet.COST_CENTER = "";
                agentDet.EMP_ID = "";
                agentDet.UID = Session["UniqueIduser"] != null ? Session["UniqueIduser"].ToString() : "";
                agentDet.Group_ID = GroupID != null && GroupID != "" ? GroupID : "";
                agentDet.Ipaddress = Session["ipaddress"].ToString();
                Booking.PaxCount = paxcount_rebo;
                Booking.Agent = agentDet;


                _CBT_Credentials.ApprovedBy = "";
                _CBT_Credentials.Chargeabilty = "";
                _CBT_Credentials.CorpEmpCode = "";
                _CBT_Credentials.CostCenter = "";
                _CBT_Credentials.CustLocation = "";
                _CBT_Credentials.Department = "";
                _CBT_Credentials.EmpCode = "";
                _CBT_Credentials.PersonalBooking = "";
                _CBT_Credentials.ProjectCode = "";
                _CBT_Credentials.TRNumber = "";
                _CBT_Credentials.VesselName = "";
                _CBT_Credentials.ERP_Attributes = _LST_ERP_Attribute;
                Booking.CBT_Input = _CBT_Credentials;
                Booking.Dealcode = _DealCodelist;



                #region GSTDetails
                if (gstdeta != null && gstdeta != "")
                {
                    string[] splgstdeta = gstdeta.Split('~');
                    RQRS.GST_DETAIS GST_DETAIS = new RQRS.GST_DETAIS();

                    GST_DETAIS.GSTAddress = splgstdeta[2];
                    GST_DETAIS.GSTCityCode = "";
                    GST_DETAIS.GSTCompanyName = splgstdeta[1];
                    GST_DETAIS.GSTEmailID = splgstdeta[3];
                    GST_DETAIS.GSTMobileNumber = splgstdeta[4];
                    GST_DETAIS.GSTNumber = splgstdeta[0];
                    GST_DETAIS.GSTPincode = "";
                    GST_DETAIS.GSTStateCode = "";
                    Booking.GstDetails = GST_DETAIS;
                }
                else
                {
                    RQRS.GST_DETAIS GST_DETAIS = new RQRS.GST_DETAIS();
                    GST_DETAIS.GSTAddress = "";
                    GST_DETAIS.GSTCityCode = "";
                    GST_DETAIS.GSTCompanyName = "";
                    GST_DETAIS.GSTEmailID = "";
                    GST_DETAIS.GSTMobileNumber = "";
                    GST_DETAIS.GSTNumber = "";
                    GST_DETAIS.GSTPincode = "";
                    GST_DETAIS.GSTStateCode = "";
                    Booking.GstDetails = GST_DETAIS;
                }
                #endregion

                for (int itf = 0; itf < ItineraryDetail.Count(); itf++) //Added by saranraj on 20170731...
                {
                    List<RQRS.MealsSSR> LstMealsDetailsub = new List<RQRS.MealsSSR>();
                    LstMealsDetailsub = ItineraryDetail.Count() > 1 ? LstMealsDetail_s.Where(l => l.Itinref == itf.ToString()).ToList() : LstMealsDetail_s;

                    if (Session["TripType" + ValKey].ToString() == "R")
                    {
                        for (int indx = 0; indx < LstMealsDetailsub.Count; indx++)
                        {
                            LstMealsDetailsub[indx].Itinref = "0";
                        }
                    }

                    ItineraryDetail[itf].MealsSSR = LstMealsDetailsub;

                    List<RQRS.BaggSSR> LstBaggDetailsub = new List<RQRS.BaggSSR>();
                    LstBaggDetailsub = ItineraryDetail.Count() > 1 ? LstBaggDetail_s.Where(l => l.Itinref == itf.ToString()).ToList() : LstBaggDetail_s;

                    if (Session["TripType" + ValKey].ToString() == "R")
                    {
                        for (int indx = 0; indx < LstBaggDetailsub.Count; indx++)
                        {
                            LstBaggDetailsub[indx].Itinref = "0";
                        }
                    }

                    ItineraryDetail[itf].BaggSSR = LstBaggDetailsub;

                    List<RQRS.SeatsSSR> SeatsSSRsub = new List<RQRS.SeatsSSR>();
                    SeatsSSRsub = ItineraryDetail.Count() > 1 ? SeatsSSR.Where(l => l.Itinref == itf.ToString()).ToList() : SeatsSSR;

                    if (Session["TripType" + ValKey].ToString() == "R")
                    {
                        for (int indx = 0; indx < SeatsSSRsub.Count; indx++)
                        {
                            SeatsSSRsub[indx].Itinref = "0";
                        }
                    }
                    ItineraryDetail[itf].SeatsSSR = SeatsSSRsub;



                    //ss
                    List<RQRS.OtherSSR> OtherSsrsr = new List<RQRS.OtherSSR>();
                    OtherSsrsr = ItineraryDetail.Count() > 1 ? OtherSSR.Where(l => l.Itinref == itf.ToString()).ToList() : OtherSSR;

                    if (Session["TripType" + ValKey].ToString() == "R")
                    {
                        for (int indx = 0; indx < OtherSsrsr.Count; indx++)
                        {
                            OtherSsrsr[indx].Itinref = "0";
                        }
                    }
                    ItineraryDetail[itf].OtherSSR = OtherSsrsr;
                    //ens
                }

                Booking.ItineraryFlights = ItineraryDetail;
                Booking.FFNumber = FFnumberDetail;
                Booking.MealsSSR = null; //commented by saranraj on 20170731...
                Booking.BaggSSR = null;   //commented by saranraj on 20170731...
                Booking.SeatsSSR = null;       //commented by saranraj on 20170731...   
                Booking.OtherSSR = OtherSSR;  //commented by sri on 20170731...
                Booking.PaxDetails = PaxDetail;
                Booking.Payment = PaymentDetail;
                Booking.AddressDetails = contactDet;
                Booking.Adult = adultCount;
                Booking.Child = childCount;
                Booking.Infant = infantCount;
                Booking.PaymentMode = PaymentMode;
                Booking.PaymentrefID = "";
                Booking.QueueFlag = "";
                Booking.PAYMENT_INFO = _CashPaymentInfo;
                if (HideComission == "Y")
                {
                    Booking.NetFareOnly = true;
                }
                else
                {
                    Booking.NetFareOnly = false;
                }
                // Booking.QueueFlag = queue.ToUpper();
                Booking.BestBuyOption = bestbuy.ToUpper() == "TRUE" ? true : false;
                if (airportCategory.Trim().ToUpper() == "I")
                {
                    Booking.BaseOrigin = Session["BaseOrgin" + ValKey].ToString();
                    Booking.BaseDestination = Session["BaseDest" + ValKey].ToString();
                    Booking.SegmentClass = Session["segmclass" + ValKey].ToString();// "E";
                }
                else
                {
                    Booking.BaseOrigin = Session["BaseOrgin" + ValKey].ToString();
                    Booking.BaseDestination = Session["BaseDest" + ValKey].ToString();
                    Booking.SegmentClass = Session["segmclass" + ValKey].ToString();
                }

                if (strRebook != null && strRebook != "null" && strRebook != "" && Rebookpnr != null && Rebookpnr != "null" && Rebookpnr != "")
                {
                    Booking.ItineraryFlights[0].IsAlreadyBooked = true;
                }

                Booking.ReBook = Rebook;
                Booking.TrackId = lstrTrackid;

                ViewBag.BlockPnreState = BlockTicket;

                if (BlockTicket == true)
                {
                    Booking.BlockPNR = true;
                }
                else if (PaymentMode == "P")
                {
                    Booking.BlockPNR = false;
                    Booking.Track_Create = true;
                }
                else
                {
                    Booking.BlockPNR = false;
                }

                if (booktype == "BF")
                {
                    Booking.Booking_Type = "BARGAIN";
                }




                #region Mo NO

                Booking.MONumber = (MONumber != null && MONumber != "") ? MONumber.ToUpper() : "";

                #endregion

                #endregion

                #region    For Asyncresponseupdateafterbooking

                if (BlockTicket == true)
                {
                    if (Session["BLOCKREQ"] != null && Session["BLOCKREQ"].ToString() != "")
                    {
                        Session["BLOCKREQ"] = true;
                    }
                    else
                    {
                        Session.Add("BLOCKREQ", true);
                    }
                }


                #endregion

                int bookingtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                string Query = mulreq == "Y" ? "InvokeBooking" : "InvokeBooking";
                string ReqTime = string.Empty;
                string LstrDetails = string.Empty;
                System.IO.StringWriter strREQUEST = new System.IO.StringWriter();
                DataSet dsrequest = new DataSet();
                string request = Newtonsoft.Json.JsonConvert.SerializeObject(Booking).ToString();

                Session.Add("TKTFLAG", "DTKT");
                #region Log

                if (ConfigurationManager.AppSettings["Splavailagent"].ToString() != "" && ConfigurationManager.AppSettings["Splavailagent"].ToString().Contains(Session["POS_TID"].ToString()))
                {
                    strURLpath = ConfigurationManager.AppSettings["Spl_APPS_SELECT_URL"].ToString();
                }
                else
                {
                    strURLpath = ConfigurationManager.AppSettings["APPS_SELECT_URL"].ToString();
                }

                ReqTime = DateTime.Now.ToString("yyyyMMdd");
                DateTime.Now.TimeOfDay.ToString().Replace(':', '_');

                LstrDetails = "<BOOKING_REQUEST><AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><URL>[<![CDATA[" + strURLpath
                + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (bookingtimeout).ToString() +
                "</TIMEOUT><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><REQUEST>[<![CDATA[" + request + "]]>]</REQUEST><MONUMBER>" + MONumber + "</MONUMBER><INSURANCEREQ>" + Insdetails + "</INSURANCEREQ><ONWARDTOURCODE>" + ontourcode
                + "</ONWARDTOURCODE><RETTOURCODE>" + rttourcode + "</RETTOURCODE></BOOKING_REQUEST>";


                dsrequest = Serv.convertJsonStringToDataSet(request, "");
                dsrequest.WriteXml(strREQUEST);
                DatabaseLog.LogData(strUserName, "B", "BookFlight", "BOOKING REQUEST", LstrDetails + strREQUEST.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                #endregion

                MyWebClient client = new MyWebClient();
                client.LintTimeout = bookingtimeout;
                client.Headers["Content-type"] = "application/json";
                string PaymentTrackid = string.Empty;
                string Paymentpath = string.Empty;

                string Path = string.Empty;
                string PathNEW = string.Empty;

                if (!string.IsNullOrEmpty(PaymentMode) && PaymentMode.ToString().ToUpper().Trim() == "P")
                {
                    #region paymentgateway write

                    Session.Add("BajajInsDetails", Insdetails);
                    Session.Add("BajajPaxdet", PaxDet);
                    Session.Add("BajajContactdet", ContactDet);
                    Session.Add("BajajGstdetails", gstdeta);
                    Session.Add("PGagentID", ClientID != null && ClientID != "" ? ClientID : Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "");
                    Session.Add("PGterminalID", ClientID != null && ClientID != "" ? ClientID + "01" : Session["POS_TID"] != null && Session["POS_TID"] != "" ? Session["POS_TID"].ToString() : "");
                    Session.Add("PGPosID", Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "");
                    Session.Add("PGUserName", strUserName);
                    Session.Add("PGBranchId", BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "");
                    Session.Add("PGTripType", Session["TripType" + ValKey].ToString());
                    Session.Add("PGReferenceID", TKey);
                    Session.Add("PGCompanyID", ClientID != null && ClientID != "" ? ClientID : Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "");
                    Session.Add("mulreq", mulreq);

                    string strAgentID = Session["agentid"] != null && Session["agentid"] != "" ? Session["agentid"].ToString() : "";
                    string CLIENT_ID = ClientID != null && ClientID != "" ? ClientID : Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                    string BRANCH_ID = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";

                    byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                    strResponse = System.Text.Encoding.ASCII.GetString(data);
                    RQRS.BookingRS _BookingResPGtrack = JsonConvert.DeserializeObject<RQRS.BookingRS>(strResponse);

                    ReqTime = DateTime.Now.ToString("yyyyMMdd");

                    LstrDetails = "<BOOKING_RESPONSE><URL>[<![CDATA[" + strURLpath + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (bookingtimeout).ToString() +
                                   "</TIMEOUT><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE>[<![CDATA[" + strResponse + "]]>]</RESPONSE></BOOKING_RESPONSE>";

                    DatabaseLog.LogData(strUserName, "B", "BookFlight", "BOOKING PG-TRACK RESPONSE", LstrDetails + strResponse.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                    Path = "";
                    if (_BookingResPGtrack.ResultCode == "1" && (_BookingResPGtrack.Error == null || _BookingResPGtrack.Error == ""))
                    {
                        ViewBag.ResultCode = "4";
                        PaymentTrackid = _BookingResPGtrack.TrackId != null && _BookingResPGtrack.TrackId.ToString() != "" ? _BookingResPGtrack.TrackId : "";
                        Session.Add("BookingREquestdetails" + PaymentTrackid.Split('|')[0], Booking);
                        Session["Checkduplicatebooking_" + Booking.ItineraryFlights[0].Select_Token] = Booking.ItineraryFlights[0].Select_Token;
                        string strPGusername = string.Empty;
                        if (ConfigurationManager.AppSettings["APP_HOSTING"] == "BSA")
                            strPGusername = Session["username"].ToString().Contains("@") ? Session["username"].ToString().Replace("@", "") : Session["username"].ToString();
                        else
                            strPGusername = Session["username"].ToString();

                        PathNEW = strClientID + "@" + strClientTerminalID + "@" + strTerminalType + "@" + strPGusername + "@" + Session["ipAddress"].ToString() +
                       "@" + Session["sequenceid"].ToString() + "@" + PaymentTrackid + "@" + "B" + "@" + "AIR" + "@" + TotAmnt + "@" + ConfigurationManager.AppSettings["PGRESPONSEURL"].ToString() + "@" + CLIENT_ID + "@" + BRANCH_ID;
                        Path = ConfigurationManager.AppSettings["PGREQUESTURL"].ToString() + "?AGENTID=" + strClientID + "&PGKEY=" + Base.EncryptKEy(PathNEW, "SKV" + strClientID.ToString().ToUpper().Trim());
                        Paymentpath = PathNEW;
                        ViewBag.paymentUrl = Path;
                        Array_Book[paymentgateway_return] = Path;
                        return Json(new { Status = "", Message = "", Result = Array_Book });
                    }
                    else if (_BookingResPGtrack.Error != "")
                    {
                        Array_Book[error] = "0";
                        ViewBag.ResultCode = "0";
                        Array_Book[GrossFareRespose] = _BookingResPGtrack.Error; ;
                        ViewBag.GrossFareRespose = _BookingResPGtrack.Error + "-(#01)";
                        Session.Add("BookStart" + ValKey, "false");
                        Session["AppBookStatus"] = false;
                        return PartialView("_BookingSuccess", "");
                    }
                    else
                    {
                        Array_Book[error] = "0";
                        ViewBag.ResultCode = "0";
                        Array_Book[GrossFareRespose] = _BookingResPGtrack.Error; ;
                        ViewBag.GrossFareRespose = _BookingResPGtrack.Error + "-(#01)";
                        Session["AppBookStatus"] = false;
                        Session.Add("BookStart" + ValKey, "false");
                        return PartialView("_BookingSuccess", "");
                    }

                    #endregion
                }
                else
                {
                    #region Booking Request Send.........
                    byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                    strResponse = System.Text.Encoding.ASCII.GetString(data);
                }

                RQRS.BookingRS _BookingRes = JsonConvert.DeserializeObject<RQRS.BookingRS>(strResponse);
                Session["AppBookStatus"] = false;
                #region Log

                strREQUEST = new System.IO.StringWriter();

                ReqTime = DateTime.Now.ToString("yyyyMMdd");
                DateTime.Now.TimeOfDay.ToString().Replace(':', '_');

                LstrDetails = "<BOOKING_RESPONSE><AIRPORTCATEGORY>" + strResponse + "</AIRPORTCATEGORY><URL>[<![CDATA[" + strURLpath
                + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (bookingtimeout).ToString() +
                "</TIMEOUT><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE>[<![CDATA[" + strResponse + "]]>]</RESPONSE></BOOKING_RESPONSE>";


                dsrequest = Serv.convertJsonStringToDataSet(strResponse, "");
                dsrequest.WriteXml(strREQUEST);

                ViewBag.Airlinetrackid_tune = dsrequest.Tables[0].Rows[0]["TRK"];


                DatabaseLog.LogData(strUserName, "B", "BookFlight", "BOOKING RESPONSE", LstrDetails + strREQUEST.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                #endregion
                RaysService _rays_servers = new RaysService();
                #region SERVICE URL BRANCH BASED -- STS195
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (Booking.Agent.BranchID != "" && strBranchCredit.Contains(Booking.Agent.BranchID)))
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
                #endregion
                if (string.IsNullOrEmpty(strResponse))
                {
                    string date = Base.LoadServerdatetime();

                    string lstrBkingdetail = (BlockTicket ? "<BLOKING_RESPONSE>" : "<BOOKING_RESPONSE>") +
                               ("<STATUS>FAILED</STATUS>") +
                              ("<RESPONSE>Null</RESPONSE>") +
                               "<AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE_TIME>" + date +
                           "</RESPONSE_TIME>" + (BlockTicket ? "</BLOKING_RESPONSE>" : "</BOOKING_RESPONSE>");

                    //_rays_servers.Apps_Booking_Response_Receive_StatusAsync("", Session["POS_ID"].ToString() + " ( " + Session["agencyname"].ToString() + " )", Session["POS_TID"].ToString(),
                    //  strUserName, "W", Session["ipAddress"].ToString(), Session["sequenceid"].ToString(),
                    //  "", lstrBkingdetail, "FormBookingRequest", null);
                    string refstring = string.Empty;

                    _rays_servers.Apps_Booking_Response_Receive_Status("", Session["POS_ID"].ToString() + " ( " + Session["agencyname"].ToString() + " )", Session["POS_TID"].ToString(),
                      strUserName, "W", Session["ipAddress"].ToString(), Session["sequenceid"].ToString(),
                     ref refstring, lstrBkingdetail, "FormBookingRequest");

                    Session.Add("BookStart" + ValKey, "false");
                    Array_Book[error] = "Problem occured while booking. Please Contact Customercare.";
                    if (ConfigurationManager.AppSettings["SENDPENDINGMAIL"].ToString().ToUpper().Trim() == "Y")
                    {
                        Sendmailforbookingstatus(strResponse, request, _PriceItenaryRS.PriceItenarys,"Failed");
                    }
                    //return Array_Book;
                    //return Json(new { Status = "", Message = "", Result = Array_Book });
                    return PartialView("_BookingSuccess", "");
                }
                else
                {
                    Array_Book[bookingresponse] = strResponse;
                    ViewBag.bookingresponse = strResponse;
                    var RetVal = JsonConvert.DeserializeObject<RQRS.BookingRS>(strResponse);

                    string RetVale = JsonConvert.SerializeObject(RetVal);
                    dsBookFlight = Serv.convertJsonStringToDataSet(RetVale, "");

                    if (RetVal != null)
                    {
                        string lstrBookingdetail = "";
                        try
                        {
                            lstrBookingdetail = (BlockTicket ? "<BLOKING_RESPONSE>" : "<BOOKING_RESPONSE>") +
                              ((RetVal != null && (RetVal.ResultCode == "1")) ?
                              ("<STATUS>SUCCESS</STATUS>") :
                              ((RetVal != null && (RetVal.ResultCode == "2")) ?
                              ("<STATUS>FARECHANGE</STATUS>") : ("<STATUS>FAILED</STATUS>"))) +
                              ((RetVal != null && RetVal.ResultCode != null && RetVal.ResultCode.ToString().Trim() != "") ?
                              ("<ResultCode>" + RetVal.ResultCode.ToString().Trim() + "</ResultCode>") : ("<ResultCode>Empty/Null</ResultCode>")) +
                              ((RetVal != null && RetVal.Sqe != null && RetVal.Sqe.ToString().Trim() != "") ?
                              ("<Sqe>" + RetVal.Sqe.ToString().Trim() + "</Sqe>") : ("<Sqe>Empty/Null</Sqe>")) +
                              ((RetVal != null && RetVal.Error != null && RetVal.Error.ToString().Trim() != "") ?
                              ("<Error>" + RetVal.Error.ToString().Trim() + "</Error>") : ("<Error>Empty/Null</Error>")) +
                              ((RetVal != null && RetVal.TrackId != null && RetVal.TrackId.ToString().Trim() != "") ?
                              ("<TrackId>" + RetVal.TrackId.ToString().Trim() + "</TrackId>") : ("<TrackId>Empty/Null</TrackId>")) +
                              (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].SPNR != null ?
                              ("<SPNR>" + RetVal.PnrDetails[0].SPNR.ToString().Trim() + "</SPNR>") : "") +
                              (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].AIRLINEPNR != null ?
                              ("<AIRLINEPNR>" + RetVal.PnrDetails[0].AIRLINEPNR.ToString().Trim() + "</AIRLINEPNR>") : "") +
                              (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].CRSPNR != null ?
                              ("<CRSPNR>" + RetVal.PnrDetails[0].CRSPNR.ToString().Trim() + "</CRSPNR>") : "") +
                              (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].GROSSFARE != null ?
                              ("<GROSSFARE>" + RetVal.PnrDetails[0].GROSSFARE.ToString().Trim() + "</GROSSFARE>") : "") +
                              (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].TICKETINGCARRIER != null ?
                              ("<TICKETINGCARRIER>" + RetVal.PnrDetails[0].TICKETINGCARRIER.ToString().Trim() + "</TICKETINGCARRIER>") : "") +
                              (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].TRIPTYPE != null ?
                              ("<TRIPTYPE>" + RetVal.PnrDetails[0].TRIPTYPE.ToString().Trim() + "</TRIPTYPE>") : "") +
                              (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].OFFICEID != null ?
                              ("<OFFICEID>" + RetVal.PnrDetails[0].OFFICEID.ToString().Trim() + "</OFFICEID>") : "") +
                              (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].OFFLINEFLAG != null ?
                              ("<OFFLINEFLAG>" + RetVal.PnrDetails[0].OFFLINEFLAG.ToString().Trim() + "</OFFLINEFLAG>") : "") +
                              (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 ?
                              ("<PnrDetailsCount>" + RetVal.PnrDetails.Count().ToString().Trim() + "</PnrDetailsCount>") : "") +
                              "<AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><URL>[<![CDATA[" + Session["travraysws_url"] + "/" +
                              Session["travraysws_vdir"] + "/Rays.svc/" + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (bookingtimeout).ToString() +
                          "</TIMEOUT><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE_TIME>" +
                         ReqTime +
                          "</RESPONSE_TIME>" + (BlockTicket ? "</BLOKING_RESPONSE>" : "</BOOKING_RESPONSE>");
                        }
                        catch
                        {

                        }
                        string result = RetVal.ResultCode; //dsBookFlight.Tables["rootNode"].Rows[0]["BRC"].ToString();


                        #region result code 1-sri
                        if (result == "1" || result == "5" || result=="10")
                        {
                            if (strProduct == "RBOA" && strTerminalType == "T" && Session["ENQCALLID"] != null && Session["ENQCALLID"].ToString() != "")
                            {

                                try
                                {
                                    string chhcallid = Session["chkselectcallid"] != null ? Session["chkselectcallid"].ToString() : "";
                                    string Callid = Session["ENQCALLID"] != null ? Session["ENQCALLID"].ToString() : "";
                                    string[] enqcllid = Callid.Split('-');
                                    if (chhcallid != "")
                                    {

                                        string[] splitcllid = chhcallid.Split('_');
                                        for (int i = 0; i < splitcllid.Length - 1; i++)
                                        {
                                            _CRMDATA.Call_ID += splitcllid[i] + ",";
                                        }
                                    }

                                    _CRMDATA.Call_ID += enqcllid[1] + ",";
                                    _CRMDATA.ENQUIRY_ID = enqcllid[0];
                                    string SelctedcallID = Session["chkselectcallid"] != null ? Session["chkselectcallid"].ToString() : "";
                                    if (SelctedcallID == "")
                                    {
                                        _CRMDATA.SaveDetails = "SYS";
                                    }
                                    else
                                    {
                                        _CRMDATA.SaveDetails = "USR";
                                    }
                                    _CRMDATA.Status = "5";
                                    _CRMDATA.Username = strUserName;

                                    var Insert = DependencyResolver.Current.GetService<CRMController>();
                                    JsonResult updateresults = Insert.UpdateEnquiry(_CRMDATA);
                                }
                                catch (Exception ex)
                                {
                                    DatabaseLog.LogData(strUserName, "X", "Search", "UpdateEnquiry_REQ", ex.ToString(), Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "", strTerminalId, "0");
                                }
                            }

                            #region Bajajinsurancebooking request
                            bool Insres = false;
                            if (strProduct == "RIYA" && Insdetails != null && Insdetails != "" && RetVal.Error.Contains("SUPRKMHx1007086:") == false)
                            {
                                Insres = false;

                                if (ConfigurationManager.AppSettings["Etravelversion"].ToString().ToUpper() != "NEW")
                                {
                                    ViewBag.Insresponse = BookbajajInsuranceOld(RetVale, Insdetails, PaxDet, ContactDet, ClientID, PaymentMode);
                                }
                                else
                                {
                                    ViewBag.Insresponse = BookbajajInsurance(RetVale, Insdetails, PaxDetail, ContactDet, ClientID, PaymentMode, agentDet.BranchID, "", gstdeta);//2143
                                }
                            }
                            else
                            {
                                ViewBag.Insresponse = "";
                            }
                            #endregion

                            string airwaysName = "";
                            if (Session["BaseDest" + ValKey].ToString() != null && Session["BaseDest" + ValKey].ToString() != "")
                            {
                                string Citycode = Session["BaseDest" + ValKey].ToString();
                                DataSet dsAirways = new DataSet();
                                string strAirways = Server.MapPath("~/XML/CityAirport_Lst.xml");
                                dsAirways.ReadXml(strAirways);

                                var qryAirlineName = from p in dsAirways.Tables
                                                   ["AIR"].AsEnumerable()
                                                     where p.Field<string>
                                                   ("ID") == Citycode
                                                     select p;
                                DataView dvAirlineCode = qryAirlineName.AsDataView();
                                if (dvAirlineCode.Count == 0)
                                    airwaysName = Citycode;
                                else
                                {
                                    DataTable dtAilineCode = new DataTable();
                                    dtAilineCode = qryAirlineName.CopyToDataTable();
                                    airwaysName = dtAilineCode.Rows[0]["CN"].ToString().Split('-')[0];
                                }
                            }


                            #region Offlinevisarequest

                            if (strProduct == "RIYA" && ConfigurationManager.AppSettings["Offlinevisadestinationcity"] != "" && RetVal.Error.Contains("SUPRKMHx1007086:") == false && BlockTicket == false
                            && ConfigurationManager.AppSettings["Offlinevisadestinationcity"].Contains(Session["BaseDest" + ValKey].ToString()) == true && Session["PaxCount" + ValKey] != null)
                            {
                                string QrystringVisa = string.Empty;
                                ViewBag.Visaoffline = "YES";
                                string[] strPaxCounts = PaxCount.Split('|');
                                if (strPaxCounts.Length > 0)
                                {
                                    adultCount = Convert.ToInt16(strPaxCounts[0]);
                                    childCount = Convert.ToInt16(strPaxCounts[1]);
                                    infantCount = Convert.ToInt16(strPaxCounts[2]);
                                }
                                int paxcnt = adultCount + childCount + infantCount;
                                string today = DateTime.Now.ToString("dd/MM/yyyy");
                                QrystringVisa = "tid=" + (Session["terminalid"].ToString()) + "&agentid=" + (Session["agentid"].ToString())
                                              + "&IPA=" + (Session["ipAddress"].ToString()) + "&username=" + (strUserName)
                                              + "&seq=" + (Session["sequenceid"].ToString())
                                              + "&pax=" + (paxcnt) + "&PNR=" + (RetVal.PnrDetails[0].SPNR.ToString());
                                string QueryString = "SECKEY=" + Base.EncryptKEy(QrystringVisa, "VISAQRY" + today);
                                ViewBag.VisaQuerystringURL = QueryString;
                                ViewBag.ThaiVisa = "NO";
                                ViewBag.ThaiVisaQuerystringURL = "";
                            }
                            else if (strProduct == "RIYA" && airwaysName.ToUpper().ToString() == "TH" && RetVal.Error.Contains("SUPRKMHx1007086:") == false && BlockTicket == false)
                            {
                                string QrystringVisa = string.Empty;
                                ViewBag.ThaiVisa = "YES";
                                string today = DateTime.Now.ToString("dd/MM/yyyy");
                                QrystringVisa = "tid=" + (Session["terminalid"].ToString()) + "&agentid=" + (Session["agentid"].ToString())
                                              + "&IPA=" + (Session["ipAddress"].ToString()) + "&username=" + (strUserName)
                                              + "&seq=" + (Session["sequenceid"].ToString() + "&Branchid=" + (Session["branchid"].ToString())
                                              + "&Agenttype=" + (Session["agenttype"].ToString()) + "&Terminaltype=" + (Session["TerminalType"].ToString()));

                                string QueryString = "SECKEY=" + Base.EncryptKEy(QrystringVisa, "THAIVISA" + today);
                                ViewBag.ThaiVisaQuerystringURL = QueryString;
                                ViewBag.Visaoffline = "NO";
                                ViewBag.VisaQuerystringURL = "";
                            }
                            else
                            {
                                ViewBag.Visaoffline = "NO";
                                ViewBag.VisaQuerystringURL = "";
                                ViewBag.ThaiVisa = "NO";
                                ViewBag.ThaiVisaQuerystringURL = "";
                            }

                            #endregion
                            if (RetVal.Error.Contains("SUPRKMHx1007086:"))
                            {
                                Array_Book[error] = result;

                                ViewBag.ResultCode = result;
                                Array_Book[ErrorRespose] = Regex.Split(RetVal.Error, "SUPRKMHx1007086:")[1];
                                ViewBag.Booktext = Regex.Split(RetVal.Error, "SUPRKMHx1007086:")[1];

                            }
                            else
                            {
                                Array_Book[error] = result;
                                ViewBag.ResultCode = result;
                                Array_Book[ErrorRespose] = "";
                                ViewBag.Booktext = "";
                            }


                            Session.Add("dsBookFlight" + ValKey, dsBookFlight);


                            var Tkt = from Ticket in RetVal.PnrDetails.AsEnumerable()
                                      group Ticket by new { Ticket.AIRLINEPNR, Ticket.ORIGIN, Ticket.DESTINATION }
                                          into grpTicketlst
                                      select new RQRS.PnrDetails
                                      {
                                          SPNR = grpTicketlst.FirstOrDefault().SPNR,
                                          CRSPNR = grpTicketlst.FirstOrDefault().CRSPNR,
                                          //SupPNR = Ticket.SupplierPNR != null ? Ticket.SupplierPNR : "N/A",
                                          AIRLINEPNR = grpTicketlst.FirstOrDefault().AIRLINEPNR,
                                          FLIGHTNO = grpTicketlst.FirstOrDefault().FLIGHTNO,
                                          AIRLINECODE = grpTicketlst.FirstOrDefault().AIRLINECODE,
                                          CLASS = grpTicketlst.FirstOrDefault().CLASS,
                                          ORIGIN = grpTicketlst.FirstOrDefault().ORIGIN,//Utilities.AirportcityName(Ticket.ORIGIN),
                                          DESTINATION = grpTicketlst.FirstOrDefault().DESTINATION,//Utilities.AirportcityName(Ticket.DESTINATION),
                                          DEPARTUREDATE = grpTicketlst.FirstOrDefault().DEPARTUREDATE,
                                          ARRIVALDATE = grpTicketlst.FirstOrDefault().ARRIVALDATE,
                                          BLOCKTIMELIMIT = grpTicketlst.FirstOrDefault().BLOCKTIMELIMIT,
                                          GROSSFARE = Convert.ToString((Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().GROSSFARE != null && grpTicketlst.FirstOrDefault().GROSSFARE != "") ? grpTicketlst.FirstOrDefault().GROSSFARE : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SERVICECHARGE != null && grpTicketlst.FirstOrDefault().SERVICECHARGE != "") ? grpTicketlst.FirstOrDefault().SERVICECHARGE : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().MARKUP != null && grpTicketlst.FirstOrDefault().MARKUP != "") ? grpTicketlst.FirstOrDefault().MARKUP : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().CLIENTMARKUP != null && grpTicketlst.FirstOrDefault().CLIENTMARKUP != "") ? grpTicketlst.FirstOrDefault().CLIENTMARKUP : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SEATSAMOUNT != null && grpTicketlst.FirstOrDefault().SEATSAMOUNT != "") ? grpTicketlst.FirstOrDefault().SEATSAMOUNT : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().ADDMARKUP != null && grpTicketlst.FirstOrDefault().ADDMARKUP != "") ? grpTicketlst.FirstOrDefault().ADDMARKUP : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().BAGGAGE != null && grpTicketlst.FirstOrDefault().BAGGAGE != "") ? grpTicketlst.FirstOrDefault().BAGGAGE : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().MEALSAMOUNT != null && grpTicketlst.FirstOrDefault().MEALSAMOUNT != "") ? grpTicketlst.FirstOrDefault().MEALSAMOUNT : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SFAMOUNT != null && grpTicketlst.FirstOrDefault().SFAMOUNT != "") ? grpTicketlst.FirstOrDefault().SFAMOUNT : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SFGST != null && grpTicketlst.FirstOrDefault().SFGST != "") ? grpTicketlst.FirstOrDefault().SFGST : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT != null && grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT != "") ? grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT : "0"))),
                                          NetFare = Convert.ToString((Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().GROSSFARE != null && grpTicketlst.FirstOrDefault().GROSSFARE != "") ? grpTicketlst.FirstOrDefault().GROSSFARE : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SERVICECHARGE != null && grpTicketlst.FirstOrDefault().SERVICECHARGE != "") ? grpTicketlst.FirstOrDefault().SERVICECHARGE : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().MARKUP != null && grpTicketlst.FirstOrDefault().MARKUP != "") ? grpTicketlst.FirstOrDefault().MARKUP : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().CLIENTMARKUP != null && grpTicketlst.FirstOrDefault().CLIENTMARKUP != "") ? grpTicketlst.FirstOrDefault().CLIENTMARKUP : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SEATSAMOUNT != null && grpTicketlst.FirstOrDefault().SEATSAMOUNT != "") ? grpTicketlst.FirstOrDefault().SEATSAMOUNT : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().ADDMARKUP != null && grpTicketlst.FirstOrDefault().ADDMARKUP != "") ? grpTicketlst.FirstOrDefault().ADDMARKUP : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().BAGGAGE != null && grpTicketlst.FirstOrDefault().BAGGAGE != "") ? grpTicketlst.FirstOrDefault().BAGGAGE : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().MEALSAMOUNT != null && grpTicketlst.FirstOrDefault().MEALSAMOUNT != "") ? grpTicketlst.FirstOrDefault().MEALSAMOUNT : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SFAMOUNT != null && grpTicketlst.FirstOrDefault().SFAMOUNT != "") ? grpTicketlst.FirstOrDefault().SFAMOUNT : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SFGST != null && grpTicketlst.FirstOrDefault().SFGST != "") ? grpTicketlst.FirstOrDefault().SFGST : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT != null && grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT != "") ? grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT : "0"))
                                                 - ((Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().DISCOUNT != null && grpTicketlst.FirstOrDefault().DISCOUNT != "") ? grpTicketlst.FirstOrDefault().DISCOUNT : "0"))
                                                 + (Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().INCENTIVE != null && grpTicketlst.FirstOrDefault().INCENTIVE != "") ? grpTicketlst.FirstOrDefault().INCENTIVE : "0"))
                                                 + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SERVICECHARGE != null && grpTicketlst.FirstOrDefault().SERVICECHARGE != "") ? grpTicketlst.FirstOrDefault().SERVICECHARGE : "0")
                                                 + (Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().COMMISSION != null && grpTicketlst.FirstOrDefault().COMMISSION != "") ? grpTicketlst.FirstOrDefault().COMMISSION : "0"))))
                                      };


                            Array_Book[FlightResponse] = JsonConvert.SerializeObject(Tkt);

                            ViewBag.FlightResponse = JsonConvert.SerializeObject(Tkt.ToList());

                            string TotalFare = "";
                            if ((Session["TripType" + ValKey].ToString().ToUpper() == "R") || mulreq == "Y") //mulreq="Y" - Domestic Multicity...
                            {

                                int index = 0;
                                string lstrpax = "";
                                var Paxdet = from FPass in RetVal.PnrDetails.AsEnumerable()
                                                 // where ((index >= (RetVal.PnrDetails.Count / 2) ? false : true) &&
                                                 // (lstrpax.Contains(FPass.SEQNO) ? false : true))

                                             group FPass by FPass.SEQNO
                                                 into grpfilterlst
                                             select new RQRS.PnrDetails
                                             {

                                                 TITLE = grpfilterlst.FirstOrDefault().TITLE,
                                                 FIRSTNAME = grpfilterlst.FirstOrDefault().FIRSTNAME,
                                                 LASTNAME = grpfilterlst.FirstOrDefault().LASTNAME,
                                                 PAXTYPE = grpfilterlst.FirstOrDefault().PAXTYPE,
                                                 DATEOFBIRTH = grpfilterlst.FirstOrDefault().DATEOFBIRTH,
                                                 TICKETNO = grpfilterlst.Select(a => a.TICKETNO).Distinct().Aggregate((a, b) => a + " / " + b),
                                                 USERTRACKID = grpfilterlst.Select(a => a.USERTRACKID).Distinct().Aggregate((a, b) => a + " / " + b),
                                                 GROSSFARE = Convert.ToString((Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().GROSSFARE != null && grpfilterlst.FirstOrDefault().GROSSFARE != "") ? grpfilterlst.FirstOrDefault().GROSSFARE : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SERVICECHARGE != null && grpfilterlst.FirstOrDefault().SERVICECHARGE != "") ? grpfilterlst.FirstOrDefault().SERVICECHARGE : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().MARKUP != null && grpfilterlst.FirstOrDefault().MARKUP != "") ? grpfilterlst.FirstOrDefault().MARKUP : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().CLIENTMARKUP != null && grpfilterlst.FirstOrDefault().CLIENTMARKUP != "") ? grpfilterlst.FirstOrDefault().CLIENTMARKUP : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SEATSAMOUNT != null && grpfilterlst.FirstOrDefault().SEATSAMOUNT != "") ? grpfilterlst.FirstOrDefault().SEATSAMOUNT : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().ADDMARKUP != null && grpfilterlst.FirstOrDefault().ADDMARKUP != "") ? grpfilterlst.FirstOrDefault().ADDMARKUP : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().BAGGAGE != null && grpfilterlst.FirstOrDefault().BAGGAGE != "") ? grpfilterlst.FirstOrDefault().BAGGAGE : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().MEALSAMOUNT != null && grpfilterlst.FirstOrDefault().MEALSAMOUNT != "") ? grpfilterlst.FirstOrDefault().MEALSAMOUNT : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SFAMOUNT != null && grpfilterlst.FirstOrDefault().SFAMOUNT != "") ? grpfilterlst.FirstOrDefault().SFAMOUNT : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SFGST != null && grpfilterlst.FirstOrDefault().SFGST != "") ? grpfilterlst.FirstOrDefault().SFGST : "0")
                                                 + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().OTHER_SSR_AMOUNT != null && grpfilterlst.FirstOrDefault().OTHER_SSR_AMOUNT != "") ? grpfilterlst.FirstOrDefault().OTHER_SSR_AMOUNT : "0")

                                                 )),
                                                 SERVICECHARGE = ((grpfilterlst.FirstOrDefault().SERVICECHARGE != null && grpfilterlst.FirstOrDefault().SERVICECHARGE != "") ? grpfilterlst.FirstOrDefault().SERVICECHARGE : "0"),
                                                 MARKUP = grpfilterlst.FirstOrDefault().CLIENTMARKUP,
                                                 test = index++,
                                                 pax = faresplit(grpfilterlst.FirstOrDefault().SEQNO, ref lstrpax),
                                                 SPNR = grpfilterlst.Select(a => a.SPNR).Distinct().Aggregate((a, b) => a + " / " + b)
                                             };

                                Array_Book[PaxResponse] = JsonConvert.SerializeObject(Paxdet);
                                ViewBag.PaxResponse = JsonConvert.SerializeObject(Paxdet.ToList());
                                lstrpax = "";
                                index = 0;
                                var PaxFare = (from FPass in RetVal.PnrDetails.AsEnumerable()
                                                  //    where ((index >= (RetVal.PnrDetails.Count / 2) ? false : true) 
                                                  //    &&
                                                  //(lstrpax.Contains(FPass.SEQNO) ? false : true))
                                              select new
                                              {
                                                  GROSSFARE = ((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0")

                                                      )),
                                                  NetFare = ((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0")))
                                                      - ((Base.ServiceUtility.ConvertToDecimal((FPass.DISCOUNT != null && FPass.DISCOUNT != "") ? FPass.DISCOUNT : "0"))
                                                      + (Base.ServiceUtility.ConvertToDecimal((FPass.INCENTIVE != null && FPass.INCENTIVE != "") ? FPass.INCENTIVE : "0"))
                                                      + (Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"))
                                                      + (Base.ServiceUtility.ConvertToDecimal((FPass.COMMISSION != null && FPass.COMMISSION != "") ? FPass.COMMISSION : "0"))),//
                                                  pax = faresplit(FPass.SEQNO, ref lstrpax),
                                                  pnrs = FPass.SPNR,
                                                  index = index++
                                              }).ToList();

                                //TotalFare = PaxFare.GroupBy(ax => ax.pnrs).Select(Grss => Convert.ToDouble(Grss.FirstOrDefault().GROSSFARE)).ToArray().Sum().ToString(); //Commented by saranraj on 20170705 to solve multipax sum...
                                TotalFare = PaxFare.GroupBy(ax => new { ax.pnrs, ax.pax }).Select(Grss => Convert.ToDouble(Grss.FirstOrDefault().GROSSFARE)).Sum().ToString(); //Added by saranraj...
                                string TotalNetFare = PaxFare.GroupBy(ax => new { ax.pnrs, ax.pax }).Select(Grss => Convert.ToDouble(Grss.FirstOrDefault().NetFare)).Sum().ToString();

                                //Array_Book[FlightResponse] = JsonConvert.SerializeObject(Tkt);
                                Array_Book[GrossFareRespose] = JsonConvert.SerializeObject(TotalFare);

                                ViewBag.GrossFareRespose = JsonConvert.SerializeObject(TotalFare);

                                ViewBag.NetFareResponse = JsonConvert.SerializeObject(TotalNetFare);
                            }
                            else
                            {
                                string lstrpax = "";
                                var Paxdet = (from FPass in RetVal.PnrDetails.AsEnumerable()
                                              where ((lstrpax.Contains(FPass.SEQNO) ? false : true))
                                              select new RQRS.PnrDetails
                                              {
                                                  TITLE = FPass.TITLE,
                                                  FIRSTNAME = FPass.FIRSTNAME,
                                                  LASTNAME = FPass.LASTNAME,
                                                  PAXTYPE = FPass.PAXTYPE,
                                                  DATEOFBIRTH = FPass.DATEOFBIRTH,
                                                  TICKETNO = FPass.TICKETNO,
                                                  USERTRACKID = FPass.USERTRACKID,
                                                  GROSSFARE = Convert.ToString(((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0"))
                                                     + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                     + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                     + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                                                     + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                     + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                                                     + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                     + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                                                     + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                                                     + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                                                     + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0"))
                                                     ),
                                                  SERVICECHARGE = ((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"),
                                                  MARKUP = FPass.CLIENTMARKUP,
                                                  pax = faresplit(FPass.SEQNO, ref lstrpax),
                                                  SPNR = FPass.SPNR

                                              }).ToList();

                                Array_Book[PaxResponse] = JsonConvert.SerializeObject(Paxdet);
                                ViewBag.PaxResponse = JsonConvert.SerializeObject(Paxdet.ToList());

                                lstrpax = "";
                                var PaxFare = (from FPass in RetVal.PnrDetails.AsEnumerable()
                                              where ((lstrpax.Contains(FPass.SEQNO) ? false : true))
                                              select new
                                              {
                                                  GROSSFARE = ((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0"))
                                                     ),
                                                  NetFare = ((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                                                      + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0"))) 
                                                      - ((Base.ServiceUtility.ConvertToDecimal((FPass.DISCOUNT != null && FPass.DISCOUNT != "") ? FPass.DISCOUNT : "0"))
                                                      + (Base.ServiceUtility.ConvertToDecimal((FPass.INCENTIVE != null && FPass.INCENTIVE != "") ? FPass.INCENTIVE : "0"))
                                                      + (Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"))
                                                      + (Base.ServiceUtility.ConvertToDecimal((FPass.COMMISSION != null && FPass.COMMISSION != "") ? FPass.COMMISSION : "0"))),//
                                                  pax = faresplit(FPass.SEQNO, ref lstrpax),
                                                  pnrs = FPass.SPNR,
                                              }).ToList();

                                TotalFare = PaxFare.Select(Grss => Convert.ToDouble(Grss.GROSSFARE)).ToArray().Sum().ToString();
                                string TotalNetFare = Convert.ToDouble(PaxFare.Select(Grss => Convert.ToDouble(Grss.NetFare)).ToArray().Sum()).ToString();

                                Array_Book[GrossFareRespose] = JsonConvert.SerializeObject(TotalFare);
                                ViewBag.GrossFareRespose = JsonConvert.SerializeObject(TotalFare);
                                ViewBag.NetFareResponse = JsonConvert.SerializeObject(TotalNetFare);
                            }

                            DatabaseLog.LogData(strUserName, "B", "BookFlight Result", "BOOKING Array Book", "Result Code - " + result + " BOOKING_RESPONSE", Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                            #region result code 1 or 5---ERP push
                            if ((result == "1" || result == "5") && ConfigurationManager.AppSettings["STS_ERP_PUSH"].ToString().ToUpper() == "Y" && RetVal.Error.Contains("SUPRKMHx1007086:") == false && BlockTicket == false)
                            {
                                try
                                {
                                    string ERPPUSHDETAILS = string.Empty;

                                    var ERPPushDet = (from Ticket_F in _BookingRes.PnrDetails
                                                      group Ticket_F by new { Ticket_F.SPNR } into GetTotalFare
                                                      select new
                                                      {
                                                          SPNR = GetTotalFare.ToArray().Distinct().FirstOrDefault().SPNR,
                                                          USERNAME = GetTotalFare.ToArray().Distinct().FirstOrDefault().FIRSTNAME + " " + GetTotalFare.ToArray().Distinct().FirstOrDefault().LASTNAME,
                                                      }).ToList();

                                    if (ERPPushDet != null && ERPPushDet.Count > 0)
                                    {
                                        for (int i = 0; i < ERPPushDet.Count; i++)
                                        {
                                            ERPPUSHDETAILS += ERPPushDet[i].SPNR + "~";
                                        }
                                        _rays_servers.ERPPUSHFORBOOKING(ClientID, ERPPUSHDETAILS, "", "AIR", "S", "", ERPPushDet[0].USERNAME, "", "");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DatabaseLog.LogData(strUserName, "X", "FromBookingRequest", "BOOKING ERP-PUSH FUNCTION", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                }
                            }
                            #endregion

                            if (result == "5")
                            {
                                string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error + "( " + RetVal.PnrDetails[0].DESTINATION.ToString() + " - " + RetVal.PnrDetails[0].ORIGIN.ToString() + " ) (#01)" : "( " + RetVal.PnrDetails[0].ORIGIN.ToString() + " - " + RetVal.PnrDetails[0].DESTINATION.ToString() + " ) - SUCCESS <br />( " + RetVal.PnrDetails[0].DESTINATION.ToString() + " - " + RetVal.PnrDetails[0].ORIGIN.ToString() + " ) - FAILED (#03)";
                                ViewBag.Returnfailederrormsg = msg;
                            }
                        }

                        #endregion result code 1
                        #region result code 2 -sri

                        else if (result == "2")
                        {
                            string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error : "Unable to process your request. please contact customer care.";
                            Array_Book[error] = result;
                            ViewBag.ResultCode = RetVal.ResultCode;
                            Array_Book[GrossFareRespose] = msg;
                            ViewBag.GrossFareRespose = msg + "-(#01)";
                            DatabaseLog.LogData(strUserName, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - " + result + " BOOKING_RESPONSE_MESSAGE_" + msg + "", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                        }
                        #endregion result code 2 -sri
                        #region result code 3-sri

                        else if (result == "3")
                        {
                            string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error : "Unable to process your request. please contact customer care";
                            Array_Book[error] = result;
                            ViewBag.ResultCode = result;
                            Array_Book[GrossFareRespose] = msg;
                            ViewBag.GrossFareRespose = msg + "-(#01)";
                            DatabaseLog.LogData(strUserName, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - " + result + " BOOKING_RESPONSE_MESSAGE_" + msg + "", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                        }

                        #endregion result code 3-sri
                        #region result code 5----round trip double grid --onwared success and retrun failed

                        else if (result == "5")
                        {
                            //string msg = "( " + RetVal.PnrDetails[0].ORIGIN.ToString() + " - " + RetVal.PnrDetails[0].DESTINATION.ToString() + " ) - SUCCESS <br />( " + RetVal.PnrDetails[0].DESTINATION.ToString() + " - " + RetVal.PnrDetails[0].ORIGIN.ToString() + " ) - FAILED";
                            string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error + "( " + RetVal.PnrDetails[0].DESTINATION.ToString() + " - " + RetVal.PnrDetails[0].ORIGIN.ToString() + " ) (#01)" : "( " + RetVal.PnrDetails[0].ORIGIN.ToString() + " - " + RetVal.PnrDetails[0].DESTINATION.ToString() + " ) - SUCCESS <br />( " + RetVal.PnrDetails[0].DESTINATION.ToString() + " - " + RetVal.PnrDetails[0].ORIGIN.ToString() + " ) - FAILED (#03)";
                            Array_Book[error] = result;
                            ViewBag.ResultCode = result;
                            Array_Book[GrossFareRespose] = msg;
                            ViewBag.GrossFareRespose = msg + "-(#01)";

                        }
                        #endregion
                        #region Rebooking TravRays by Rajesh

                        else if (result == "7" && RetVal.Error == "REBOOK")
                        {
                            Array_Book[bookingresponse] = strResponse;
                            Session["AppBookStatus"] = false;
                            Session.Add("BookStart" + ValKey, "false");
                            DataTable dtBaggageSelect = new DataTable();
                            DataTable dtmealseSelect = new DataTable();
                            DataTable dtSelectResponse = new DataTable();
                            DataTable dtothersdt = new DataTable();
                            DataTable dtbookdet = new DataTable();
                            DataTable dtbaggout = new DataTable();
                            string strErrtemp = string.Empty;
                            string stock = Session["rebookingstock"] != null && Session["rebookingstock"] != "" ? Session["rebookingstock"].ToString() : "";
                            string msg = string.Empty;
                            decimal TotalpayamountRebook = 0;
                            string TotalFareRebook = string.Empty;
                            if (RetVal.PriceItineraryRs == null)
                            {
                                Array_Book[error] = "Problem occured while get Rebooking.Please contact customer care(#03)";
                            }

                            Array_Book[rebookingresultcode] = "7";

                            Base.ServiceUtility.GridResponseforSelect(RetVal.PriceItineraryRs, ref dtSelectResponse, ref strErrtemp, ref dtBaggageSelect, ref dtmealseSelect, ref dtothersdt, ref dtbookdet, ref dtbaggout);

                            if (dtbookdet != null)
                            {
                                //Session.Add("dtBookreq_table", dtBookreq);
                                Session["dtBookreq_table"] = dtbookdet;
                            }
                            if (dtSelectResponse != null)
                            {
                                dtSelectResponse.TableName = "TrackFareDetails";
                                Session["Response" + ValKey] = dtSelectResponse;
                            }


                            if (dtSelectResponse != null && dtSelectResponse.Rows.Count > 0)
                            {

                                if (mulreq == "Y" && Session["TripType" + ValKey].ToString().Trim() == "R")
                                {
                                    int Rebk = 0;
                                    if (_BookingRes.PnrDetails != null && _BookingRes.PnrDetails.Count > 0 && (_BookingRes.PnrDetails[0].USERTRACKID == _BookingRes.TrackId.Split('|')[0].ToString()))//If pnr detail comes means onward booked  and return rebooking........
                                    {
                                        msg += "Onward booked successfully " + " (" + ((Booking.ItineraryFlights[0].FlightDetails[0].Origin) + "-" + (Booking.ItineraryFlights[0].FlightDetails[Booking.ItineraryFlights[0].FlightDetails.Count - 1].Destination)) + ")<br/>";
                                        Rebk = 1;
                                    }
                                    else if (Booking.ItineraryFlights[0].IsAlreadyBooked == true)// Second time rebooking initiate...
                                    {
                                        Rebk = 1;
                                    }
                                    else//onward rebooking........
                                    {
                                        Rebk = 0;
                                    }
                                    decimal Reqamt = 0;
                                    for (int NN = 0; NN < _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Fares[0].Faredescription.Count; NN++)
                                    {
                                        TotalpayamountRebook += ((Base.ServiceUtility.ConvertToDecimal(_BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)) * Convert.ToInt32(paxcount_rebo.Split('|')[NN]));
                                        Reqamt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[Rebk].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[Rebk].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)) * Convert.ToInt32(paxcount_rebo.Split('|')[NN]));
                                    }
                                    TotalpayamountRebook += Booking.ItineraryFlights[Rebk].MealsSSR.Sum(n => Base.ServiceUtility.ConvertToDecimal(n.MealAmount)) + Booking.ItineraryFlights[Rebk].BaggSSR.Sum(n => Base.ServiceUtility.ConvertToDecimal(n.BaggAmount)) + Booking.ItineraryFlights[Rebk].SeatsSSR.Sum(n => Base.ServiceUtility.ConvertToDecimal(n.SeatAmount));
                                    Reqamt += Booking.ItineraryFlights[Rebk].MealsSSR.Sum(n => Base.ServiceUtility.ConvertToDecimal(n.MealAmount)) + Booking.ItineraryFlights[Rebk].BaggSSR.Sum(n => Base.ServiceUtility.ConvertToDecimal(n.BaggAmount)) + Booking.ItineraryFlights[Rebk].SeatsSSR.Sum(n => Base.ServiceUtility.ConvertToDecimal(n.SeatAmount));
                                    if (Reqamt != TotalpayamountRebook)
                                    {
                                        msg += "Fare has been revised from  :" + (Appcurrency != null && Appcurrency != "" ? Appcurrency : ConfigurationManager.AppSettings["currency"].ToString()) + " "
                                      + (Base.ServiceUtility.ConvertToDecimal(Reqamt.ToString())
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))).ToString() + " to :"
                                      + (Appcurrency != null && Appcurrency != "" ? Appcurrency : ConfigurationManager.AppSettings["currency"].ToString()) + " "
                                      + (Base.ServiceUtility.ConvertToDecimal(SplitFareByAdultPax(TotalpayamountRebook.ToString()).ToString())
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))).ToString()
                                      + " for " + dtSelectResponse.Rows[0]["Origin"].ToString() + "-" + dtSelectResponse.Rows[dtSelectResponse.Rows.Count - 1]["Destination"].ToString() + "<br/>";
                                    }
                                    for (var N = 0; _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights.Count > N; N++)
                                    {
                                        if ((Booking.ItineraryFlights[Rebk].FlightDetails[N].FlightNumber.ToString() != _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].FlightNumber.ToString()))
                                        {
                                            msg += "Flight has been revised from:" + Booking.ItineraryFlights[Rebk].FlightDetails[N].FlightNumber.ToString() + " to :" + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].FlightNumber.ToString() + " for " + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].Origin.ToString() + "-" + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].Destination.ToString() + "<br/>";
                                        }
                                        if ((Booking.ItineraryFlights[Rebk].FlightDetails[N].FareBasisCode.ToString() != _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].FareBasisCode.ToString()) && (Booking.ItineraryFlights[Rebk].FlightDetails[N].Class.ToString() != _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].Class.ToString()))
                                        {
                                            msg += "Class has been revised from:" + Booking.ItineraryFlights[Rebk].FlightDetails[N].FareBasisCode.ToString() + " to :" + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].FareBasisCode.ToString() + " for " + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].Origin.ToString() + "-" + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].Destination.ToString() + "<br/>";
                                        }
                                        if ((Booking.ItineraryFlights[Rebk].FlightDetails[N].DepartureDateTime.ToString() != _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].DepartureDateTime.ToString()))
                                        {
                                            msg += "Departure date and time has been revised from:" + Booking.ItineraryFlights[Rebk].FlightDetails[N].DepartureDateTime.ToString() + " to :" + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].DepartureDateTime.ToString() + " for " + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].Origin.ToString() + "-" + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].Destination.ToString() + "<br/>";
                                        }
                                        if ((Booking.ItineraryFlights[Rebk].FlightDetails[N].ArrivalDateTime.ToString() != _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].ArrivalDateTime.ToString()))
                                        {
                                            msg += "Arrival date and time has been revised from:" + Booking.ItineraryFlights[Rebk].FlightDetails[N].ArrivalDateTime.ToString() + " to :" + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].ArrivalDateTime.ToString() + " for " + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].Origin.ToString() + "-" + _BookingRes.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Flights[N].Destination.ToString() + "<br/>";
                                        }
                                    }
                                    if (msg != null && msg != "")
                                    {

                                        xmldata = "<ReBOOKING_Returnval>" + msg + "</ReBOOKING_Returnval>";
                                        DatabaseLog.LogData(strUserName, "T", "BOOKINGREQ", "Rebooking" + Session["TripType" + ValKey].ToString().Trim() + "~" + stock, xmldata, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                        if (Rebk == 0)
                                        {
                                            _BookingRes.PriceItineraryRs.PriceItenarys.Add(_PriceItenaryRS.PriceItenarys[1]);
                                        }
                                        else
                                        {
                                            List<RQRS.PriceItenary> _ItineraryFlightsListRebook = new List<RQRS.PriceItenary>();
                                            RQRS.PriceItenary _LstItineraryFlightsListRebook = new RQRS.PriceItenary();
                                            _ItineraryFlightsListRebook.Add(_LstItineraryFlightsListRebook);
                                            _BookingRes.PriceItineraryRs.PriceItenarys.Insert(0, _PriceItenaryRS.PriceItenarys[1]);
                                            var TktRebook = from Ticket in _BookingRes.PnrDetails.AsEnumerable()
                                                            select new
                                                            {
                                                                SPNR = Ticket.SPNR,
                                                                SupPNR = Ticket.SupplierPNR != null ? Ticket.SupplierPNR : "N/A",
                                                                CRSPNR = Ticket.CRSPNR,
                                                                ARILINEPNR = Ticket.AIRLINEPNR,
                                                                FLIGHTNO = Ticket.FLIGHTNO,
                                                                AIRLINECODE = Ticket.AIRLINECODE,
                                                                CLASS = Ticket.CLASS,
                                                                ORIGIN = Base.Utilities.AirportcityName(Ticket.ORIGIN),
                                                                DESTINATION = Base.Utilities.AirportcityName(Ticket.DESTINATION),
                                                                DEPARTUREDATE = Ticket.DEPARTUREDATE,
                                                                ARRIVALDATE = Ticket.ARRIVALDATE,
                                                                GROSSFARE = Ticket.GROSSFARE
                                                            };
                                            BookFlightdetails = JsonConvert.SerializeObject(TktRebook);

                                            string lstrpax = "";
                                            var Paxdet = from FPass in _BookingRes.PnrDetails.AsEnumerable()
                                                         where ((lstrpax.Contains(FPass.SEQNO) ? false : true))
                                                         select new
                                                         {
                                                             First = FPass.FIRSTNAME,
                                                             LASTNAME = FPass.LASTNAME,
                                                             PAXTYPE = FPass.PAXTYPE.Trim().ToUpper() == "ADT" ? "Adult" : FPass.PAXTYPE.Trim().ToUpper() == "CHD" ? "Child" : FPass.PAXTYPE.Trim().ToUpper() == "INF" ? "Infant" : FPass.PAXTYPE,
                                                             DATEOFBIRTH = FPass.DATEOFBIRTH,
                                                             TICKETNO = FPass.TICKETNO,
                                                             USERTRACKID = FPass.USERTRACKID,
                                                             GROSSFARE = ((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0")
                                                                  )),
                                                             ServiceCharge = ((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"),
                                                             markup = FPass.MARKUP,
                                                             SEATAMD = FPass.SEATSAMOUNT,
                                                             pax = faresplit(FPass.SEQNO, ref lstrpax)

                                                         };
                                            AllPaxdetails = JsonConvert.SerializeObject(Paxdet);

                                            var Tkt_FA = from Ticket_F in _BookingRes.PnrDetails
                                                         group Ticket_F by new { Ticket_F.TICKETNO } into GetTotalFare
                                                         select new
                                                         {
                                                             GROSSFARE = (Base.ServiceUtility.ConvertToDecimal((((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().GROSSFARE != null && ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().GROSSFARE != "") ? ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().GROSSFARE : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().SERVICECHARGE != null && ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().SERVICECHARGE != "") ? ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().SERVICECHARGE : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().MARKUP != null && ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().MARKUP != "") ? GetTotalFare.FirstOrDefault().MARKUP : "0")
                                                                         + Base.ServiceUtility.ConvertToDecimal((((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().ADDMARKUP != null && ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().ADDMARKUP != "") ? ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().ADDMARKUP : "0")
                                                                         + Base.ServiceUtility.ConvertToDecimal((((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().CLIENTMARKUP != null && ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().CLIENTMARKUP != "") ? ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().CLIENTMARKUP : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().BAGGAGE != null && ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().BAGGAGE != "") ? ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().BAGGAGE : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().SFAMOUNT != null && ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().SFAMOUNT != "") ? ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().SFAMOUNT : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().SFGST != null && ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().SFGST != "") ? ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().SFGST : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().MEALSAMOUNT != null && ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().MEALSAMOUNT != "") ? ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().MEALSAMOUNT : "0")
                                                                        + Base.ServiceUtility.ConvertToDecimal((((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().OTHER_SSR_AMOUNT != null && ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().OTHER_SSR_AMOUNT != "") ? ((GetTotalFare.ToArray()).Distinct()).FirstOrDefault().OTHER_SSR_AMOUNT : "0")
                                                                         )
                                                         };

                                            double TotalFareRe = 0;
                                            TotalFareRe = Convert.ToDouble(Tkt_FA.Sum(Grs => Grs.GROSSFARE));
                                            TotalFareRebook = TotalFareRe.ToString();
                                            string RebookTicketdetails = string.Empty;
                                            string PurposeofMeetingRebook = string.Empty;
                                            string SPnrRebook = _BookingRes.PnrDetails[0].SPNR.ToString();
                                            string SectorRebook = _BookingRes.PnrDetails[0].ORIGIN.ToString() + "-" + _BookingRes.PnrDetails[0].DESTINATION.ToString();
                                            string remarks_ss = Session["remarks_ss"] != null && Session["remarks_ss"] != "" ? Session["remarks_ss"].ToString() : "";
                                            string reason = remarks_ss;
                                            string purpose = remarks_ss;
                                            PurposeofMeetingRebook = purpose;
                                        }
                                        xmldata = "<ReBOOKING_REQ>" + JsonConvert.SerializeObject(_BookingRes) + "</ReBOOKING_REQ>";
                                        DatabaseLog.LogData(strUserName, "T", "BOOKINGREQ", "Rebooking" + Session["TripType" + ValKey].ToString().Trim() + "~" + stock, xmldata, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                        return Json(new { status = "7", Errormsg = msg, success = JsonConvert.SerializeObject(_BookingRes.PriceItineraryRs), BookFlightdetails = BookFlightdetails, AllPaxdetails = AllPaxdetails, AllPnrdt = JsonConvert.SerializeObject(_BookingRes.PnrDetails), Rebkamount = TotalFareRebook });
                                    }
                                    if (msg == null || msg == "")
                                    {
                                        xmldata = "<ReBOOKING_status>" + msg + "</ReBOOKING_status>";
                                        DatabaseLog.LogData(strUserName, "T", "BOOKINGREQ", "Rebooking" + Session["TripType" + ValKey].ToString().Trim() + "~" + stock, xmldata, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                        return Json(new { status = "0", Errormsg = "Fare has been revised.Problem occured while get Rebooking.Please contact customer care(#03)", success = "" });
                                    }

                                }
                                else
                                {

                                    decimal Reqamt = 0;
                                    for (int NN = 0; NN < RetVal.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Fares[0].Faredescription.Count; NN++)
                                    {
                                        TotalpayamountRebook += ((Base.ServiceUtility.ConvertToDecimal(RetVal.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(RetVal.PriceItineraryRs.PriceItenarys[0].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)) * Convert.ToInt32(PaxCount.Split('|')[NN]));
                                        Reqamt += ((Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[0].PriceRS[0].Fares[0].Faredescription[NN].GrossAmount) + Base.ServiceUtility.ConvertToDecimal(_PriceItenaryRS.PriceItenarys[0].PriceRS[0].Fares[0].Faredescription[NN].ClientMarkup)) * Convert.ToInt32(PaxCount.Split('|')[NN]));
                                    }

                                    if (Reqamt != TotalpayamountRebook)
                                    {
                                        msg += "Fare has been revised from  :" + (Appcurrency != null && Appcurrency != "" ? Appcurrency : ConfigurationManager.AppSettings["currency"].ToString()) + " "
                                      + (Base.ServiceUtility.ConvertToDecimal(Reqamt.ToString())
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))).ToString() + " to :"
                                      + (Appcurrency != null && Appcurrency != "" ? Appcurrency : ConfigurationManager.AppSettings["currency"].ToString()) + " "
                                      + (Base.ServiceUtility.ConvertToDecimal(SplitFareByAdultPax(TotalpayamountRebook.ToString()).ToString())
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["SFGST"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["SFGST"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Sftax"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Sftax"].ToString()).ToString() : "0"))
                                      + Base.ServiceUtility.ConvertToDecimal((dtSelectResponse.Rows[0]["Servicefee"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicefee"].ToString()).ToString() : "0"))).ToString()
                                      + " for " + dtSelectResponse.Rows[0]["Origin"].ToString() + "-" + dtSelectResponse.Rows[dtSelectResponse.Rows.Count - 1]["Destination"].ToString() + Environment.NewLine;
                                    }
                                    for (var N = 0; RetVal.PriceItineraryRs.PriceItenarys.Count > N; N++)
                                    {
                                        for (var i = 0; RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights.Count > i; i++)
                                        {
                                            if ((Booking.ItineraryFlights[N].FlightDetails[i].FlightNumber.ToString() != RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].FlightNumber.ToString()))
                                            {
                                                msg += "Flight has been revised from:" + Booking.ItineraryFlights[N].FlightDetails[i].FlightNumber.ToString() + " to :" + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].FlightNumber.ToString() + " for " + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].Origin.ToString() + "-" + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].Destination.ToString() + Environment.NewLine;
                                            }
                                            if ((Booking.ItineraryFlights[N].FlightDetails[i].FareBasisCode.ToString() != RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].FareBasisCode.ToString()) && (Booking.ItineraryFlights[N].FlightDetails[i].Class.ToString() != RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].Class.ToString()))
                                            {
                                                msg += "Class has been revised from:" + Booking.ItineraryFlights[N].FlightDetails[i].FareBasisCode.ToString() + " to :" + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].FareBasisCode.ToString() + " for " + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].Origin.ToString() + "-" + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].Destination.ToString() + Environment.NewLine;
                                            }
                                            if ((Booking.ItineraryFlights[N].FlightDetails[i].DepartureDateTime.ToString() != RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].DepartureDateTime.ToString()))
                                            {
                                                msg += "Departure date and time has been revised from:" + Booking.ItineraryFlights[N].FlightDetails[i].DepartureDateTime.ToString() + " to :" + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].DepartureDateTime.ToString() + " for " + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].Origin.ToString() + "-" + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].Destination.ToString() + Environment.NewLine;
                                            }
                                            if ((Booking.ItineraryFlights[N].FlightDetails[i].ArrivalDateTime.ToString() != RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].ArrivalDateTime.ToString()))
                                            {
                                                msg += "Arrival date and time has been revised from:" + Booking.ItineraryFlights[N].FlightDetails[i].ArrivalDateTime.ToString() + " to :" + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].ArrivalDateTime.ToString() + " for " + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].Origin.ToString() + "-" + RetVal.PriceItineraryRs.PriceItenarys[N].PriceRS[0].Flights[i].Destination.ToString() + Environment.NewLine;
                                            }
                                        }
                                    }
                                    if (msg == null || msg == "")
                                    {
                                        xmldata = "<ReBOOKING_status>" + msg + "</ReBOOKING_status>";
                                        DatabaseLog.LogData(strUserName, "T", "BOOKINGREQ", "Rebooking" + Session["TripType" + ValKey].ToString().Trim() + "~" + stock, xmldata, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                        Array_Book[error] = "Fare has been revised.Problem occured while get Rebooking.Please contact customer care(#03)";
                                        msg = "Fare has been revised.Problem occured while get Rebooking.Please contact customer care(#03)";
                                    }

                                    //string msg = msg;
                                    Array_Book[rebookingresponse] = RetVal.PriceItineraryRs != null ? JsonConvert.SerializeObject(RetVal.PriceItineraryRs) : "";
                                    Array_Book[error] = result;
                                    ViewBag.ResultCode = RetVal.ResultCode;
                                    Array_Book[GrossFareRespose] = msg;
                                    ViewBag.GrossFareRespose = msg + "-(#01)";
                                    DatabaseLog.LogData(strUserName, "B", "BookFlight Result", "REBOOKING RESPONSE", "Result Code - " + result + " REBOOKING_RESPONSE_MESSAGE_" + msg + "", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                                }
                            }
                            return Json(new { Status = "", Message = "", Result = Array_Book });

                        }
                        #region Duplicate booking confirmation alert
                        else if (RetVal.ResultCode == "8")
                        {
                            string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error + Environment.NewLine + "Do you wish to continue booking with same details?" : "The booking was already exists with same passenger" + Environment.NewLine + "Do you wish to continue booking with same details?";
                            Session.Add("BookStart" + ValKey, "false");
                            Session["AppBookStatus"] = false;
                            Array_Book[error] = RetVal.ResultCode;
                            ViewBag.ResultCode = RetVal.ResultCode;
                            ViewBag.bookingresponse = strResponse;
                            Array_Book[GrossFareRespose] = msg;
                            ViewBag.GrossFareRespose = msg + "-(#01)";
                            DatabaseLog.LogData(strUserName, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - " + result + " BOOKING_RESPONSE_MESSAGE_" + msg + "", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        }
                        #endregion
                        #endregion
                        else
                        {
                            string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error : "Problem occured while booking. please contact support team.";
                            Array_Book[error] = RetVal.ResultCode;
                            ViewBag.ResultCode = RetVal.ResultCode;
                            Array_Book[GrossFareRespose] = msg;
                            ViewBag.GrossFareRespose = msg + "-(#01)";
                            DatabaseLog.LogData(strUserName, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - " + result + " BOOKING_RESPONSE_MESSAGE_" + msg + "", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        }
                        if (ConfigurationManager.AppSettings["SENDPENDINGMAIL"].ToString().ToUpper().Trim() == "Y" && result != "1" && result != "3" && result != "7")
                        {
                            Sendmailforbookingstatus(strResponse, request, _PriceItenaryRS.PriceItenarys,(result == "2" ? "Failed" : "Pending"));
                        }
                    }
                    else
                    {
                        if (ConfigurationManager.AppSettings["SENDPENDINGMAIL"].ToString().ToUpper().Trim() == "Y")
                        {
                            Sendmailforbookingstatus(strResponse, request, _PriceItenaryRS.PriceItenarys,"Failed");
                        }
                        string date = Base.LoadServerdatetime();
                        string lstrBkingdetail = (BlockTicket ? "<BLOKING_RESPONSE>" : "<BOOKING_RESPONSE>") +
                                   ("<STATUS>FAILED</STATUS>") +
                                  ("<RESPONSE>Null</RESPONSE>") +
                                   "<AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE_TIME>" + date +
                               "</RESPONSE_TIME>" + (BlockTicket ? "</BLOKING_RESPONSE>" : "</BOOKING_RESPONSE>");
                    }
                    
                    Session.Add("BookStart" + ValKey, "false");
                }
                #endregion               
            }
            catch (Exception ex)
            {
                Session.Add("BookStart" + ValKey, "false");
                DatabaseLog.LogData(strUserName, "X", "FromBookingRequest", "BOOKING REQUEST FUNCTION", ex.ToString() + ex.StackTrace.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                if (ex is WebException)
                {
                    strResponse = "The operation has timed out." + Environment.NewLine + "Your booking might be confirmed please check with booked history or contact support team";
                    Array_Book[error] = strResponse;
                }
                else if (ex.Message.ToString().Contains("Unable to connect to the remote server"))
                {
                    strResponse = ex.Message.ToString() + Environment.NewLine + "Please contact to the customercare";
                    Array_Book[error] = strResponse;

                }
                else if (ex.Message.ToUpper().Contains("THE OPERATION HAS TIMED OUT"))
                {
                    strResponse = "The operation has timed out." + Environment.NewLine + "Please contact to the customercare";
                    Array_Book[error] = strResponse;
                }
                else
                {
                    strResponse = "Problem Occured while booking the ticket." + Environment.NewLine + "Please contact customercare";
                    Array_Book[error] = strResponse;
                }
                ViewBag.ResultCode = "0";
                ViewBag.GrossFareRespose = strResponse + "-(#05)";
            }

            return PartialView("_BookingSuccess", "");
        }

        private decimal SplitFareByAdultPax(string amount)
        {
            decimal adultFare = 0;
            try
            {
                amount = amount == null || amount == "" ? "0" : amount;
                string[] fareSplitUp = amount.Contains(':') ? amount.Split(':') : amount.Split('|');
                adultFare = Convert.ToDecimal(fareSplitUp[0].ToString(), CultureInfo.InvariantCulture);

                return adultFare;
            }
            catch (Exception ex)
            {
                // DatabaseLog.LogData(Session["Loginusermailid"] != null ? Session["Loginusermailid"].ToString() : "", "X", "BookingController", "SplitFareByAdultPax", ex.ToString(), Session["CompanyID"] != null && Session["CompanyID"].ToString() != "" ? Session["CompanyID"].ToString() : "", Session["CompanyID"] != null && Session["CompanyID"].ToString() != "" ? Session["CompanyID"].ToString() : "", "0", Session["IPAddress"] != null && Session["IPAddress"].ToString() != "" ? Session["IPAddress"].ToString() : "");
                DatabaseLog.LogData(strUserName, "X", "FromBookingRequest", "BOOKING REQUEST FUNCTION", ex.ToString() + ex.StackTrace.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                return adultFare;
            }
        }

        public ActionResult PendingTrack(string PaxDet, string TKey, string service, string Totalamount, string mulreq)
        {
            ArrayList Array_Pending = new ArrayList();
            Array_Pending.Add("");
            Array_Pending.Add("");
            int error = 0;
            int TrackCheck = 1;

            try
            {
                string GstrERROR = string.Empty;
                bool AgentTrack = false;
                string[] Ipax;
                DataSet dsExistPaxDetail = new DataSet();
                DataTable dtBookingResponse = new DataTable();
                if (mulreq != "Y")
                {
                    if (Session["Response" + TKey] != null)
                    {
                        dtBookingResponse = (DataTable)Session["Response" + TKey];
                    }
                }
                else
                {
                    int loop_count = Convert.ToInt32(ConfigurationManager.AppSettings["MulMaxRowCnt"]);
                    for (int i = 0; i < loop_count; i++)
                    {
                        if (Session["Response" + i + TKey] != null)
                        {
                            dtBookingResponse = (DataTable)Session["Response" + i + TKey];
                            break;
                        }
                    }
                }
                DataTable Dt = new DataTable("PaxDeatail");

                Dt.Columns.Add("FirstName");
                Dt.Columns.Add("LastName");
                Dt.Columns.Add("PassPort");
                Dt.Columns.Add("Orgin");
                Dt.Columns.Add("Destination");
                Dt.Columns.Add("TravelDate");

                string totalfare = (Base.ServiceUtility.ConvertToDecimal(Totalamount) - Base.ServiceUtility.ConvertToDecimal(service)).ToString("0");

                if (dtBookingResponse != null && dtBookingResponse.Rows.Count > 0 && dtBookingResponse.Rows[0]["AIRLINECATEGORY"].ToString() == "FSC" && Session["alowcredit"].ToString() == "Y")
                {
                    GstrERROR = "B|" + totalfare;
                }
                else
                {
                    GstrERROR = "T|" + totalfare;
                }
                if (PaxDet != "")
                {
                    if (PaxDet.Contains("SPLITSCRIPT"))
                    {
                        Ipax = Regex.Split(PaxDet, "SPLITSCRIPT");
                        DataRow Dr = Dt.NewRow();
                        //Dr["FirstName"] = Regex.Replace(Ipax[1], @"[^0-9a-zA-Z\\s]+$", "");// Ipax[1]; @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+"
                        //Dr["LastName"] = Regex.Replace(Ipax[2], @"[^0-9a-zA-Z\\s]+$", "");// Ipax[2];
                        Dr["FirstName"] = Regex.Replace(Ipax[1], @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+", "");// Ipax[1]; 
                        Dr["LastName"] = Regex.Replace(Ipax[2], @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+", "");// Ipax[2];
                        Dr["PassPort"] = Ipax[5];
                        Dr["Orgin"] = dtBookingResponse.Rows[0]["ORIGIN"];
                        Dr["Destination"] = dtBookingResponse.Rows[0]["DESTINATION"];
                        Dr["TravelDate"] = DateTime.ParseExact(dtBookingResponse.Rows[0]["DEPARTUREDATE"].ToString().Trim(), "dd MMM yyyy HH:mm", null).ToString("yyyyMMdd");
                        Dt.Rows.Add(Dr);
                        dsExistPaxDetail.Tables.Add(Dt);
                    }
                }

                Rays_service.RaysService _rays_servers = new Rays_service.RaysService();
                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                AgentTrack = _rays_servers.Agent_Pending_Track_Check(Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["username"].ToString(), Session["TerminalType"].ToString(),
                    Session["ipAddress"].ToString(), "PENDING", Session["sequenceid"].ToString(), ref GstrERROR, dsExistPaxDetail, "BookFlight",
                    "Agent Track Pending Request");
                if (AgentTrack == false)
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "E", "Agent pending track check", "PendingTrack", (GstrERROR != string.Empty) ? (GstrERROR) : "Problem occured while Checking agent pending track details", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    Array_Pending[error] = (GstrERROR != string.Empty) ? (GstrERROR) : "Problem occured while Checking agent pending track details";
                    Array_Pending[TrackCheck] = AgentTrack;
                    // return Array_Pending;
                    return Json(new { Status = "", Message = "", Result = Array_Pending });
                }
                else
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "E", "Agent pending track check", "PendingTrack", (GstrERROR != string.Empty) ? (GstrERROR) : "No PENDING TRACK", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    Session.Add("AppBookStatus", true);
                    Array_Pending[error] = GstrERROR;
                    Array_Pending[TrackCheck] = AgentTrack;
                    // return Array_Pending;
                    return Json(new { Status = "", Message = "", Result = Array_Pending });

                }
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Agent pending track check", "PendingTrack", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                Array_Pending[error] = "Problem occured while check pending track (#05).";
                Array_Pending[TrackCheck] = false;
                return Json(new { Status = "", Message = "Problem occured while check pending track (#05).", Result = Array_Pending });
            }
            // return Array_Pending;
            return Json(new { Status = "", Message = "", Result = Array_Pending });
        }

        public ActionResult CheckMOnumber(string MONumber, string ClientID)
        {
            ArrayList Array_Pending = new ArrayList();
            Array_Pending.Add("");
            Array_Pending.Add("");
            int error = 0;
            int TrackCheck = 1;
            string stroutput = string.Empty;
            string strErrormsg = string.Empty;
            try
            {
                string Stragentid = ClientID == "" ? Session["POS_ID"].ToString() : ClientID;
                string StrTerminalid = ClientID == "" ? Session["POS_TID"].ToString() : ClientID + "01";
                string Strusername = Session["username"].ToString();
                string IPaddress = Session["ipAddress"].ToString();
                string Seqno = Session["sequenceid"].ToString();

                RaysService _rays = new RaysService();
                _rays.Url = ConfigurationManager.AppSettings["SERVICEURI"].ToString();
                bool resposne = _rays.CheckMoNumber(Stragentid, MONumber, strTerminalId, strUserName, Session["TerminalType"].ToString(), IPaddress, Convert.ToDecimal(Seqno), ref stroutput, ref strErrormsg, "CheckMonumber");

                string XML = "<REQUEST><AGENTID>" + Stragentid + "</AGENTID><MONUMBER>" + MONumber + "</MONUMBER><TERMINALID>" + strTerminalId
                    + "</TERMINALID><TERMINALTYPE>" + Session["TerminalType"].ToString() + "</TERMINALTYPE><IPADDRESS>" + IPaddress + "</IPADDRESS><SEQNO>" + Seqno + "</SEQNO></REQUEST><RESPONSE><OUTPUTREF>" + stroutput
                    + "</OUTPUTREF><ERRORMSG>" + strErrormsg + "</ERRORMSG><RESPONSE>" + resposne + "</RESPONSE><PAGENAME>CheckMonumber</PAGENAME></RESPONSE>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "MONumber Check", "MONumber Check", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                if (!string.IsNullOrEmpty(stroutput))
                {
                    if (stroutput.ToUpper() == "FALSE")
                    {
                        Array_Pending[error] = "MO No. Already Exist,Please enter another MO No.";
                        Array_Pending[TrackCheck] = "0";
                    }
                    else
                    {
                        Array_Pending[TrackCheck] = "1";
                    }
                }
                else
                {
                    Array_Pending[error] = "Unable to validate MO No. Please contact support team";
                    Array_Pending[TrackCheck] = "0";
                }

            }
            catch (Exception ex)
            {
                Array_Pending[error] = "Problem occurred while validate MO No. Please contact support team";
                Array_Pending[TrackCheck] = "0";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "MONumber Check", "MONumber Check", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            return Json(new { Status = "", Message = "", Result = Array_Pending });
        }

        public string BookbajajInsuranceOld(string responseval, string Insdetails, string Paxdet, string Contactdet, string ClientID, string PaymentMode)
        {
            string response = string.Empty;
            string[] paxdetails, Ipax, paxinfo, contactdet, Icont, Addressdetails;
            RaysService _raysservice = new RaysService();
            DataTable dtPassDetails = new DataTable();
            string SPNR = string.Empty;
            string Air_PNR = string.Empty;
            string CRSPNR = string.Empty;
            string Mobileno = string.Empty;
            try
            {
                DataTable dtFormPaxForIns = new DataTable();
                dtFormPaxForIns.Columns.Add("TITLE");
                dtFormPaxForIns.Columns.Add("FIRSTNAME");
                dtFormPaxForIns.Columns.Add("LASTNAME");
                dtFormPaxForIns.Columns.Add("DOB");
                dtFormPaxForIns.Columns.Add("GENDER");
                dtFormPaxForIns.Columns.Add("PASSPORT");
                dtFormPaxForIns.Columns.Add("Relation");
                dtFormPaxForIns.Columns.Add("ASSIGNEE");
                dtFormPaxForIns.Columns.Add("PaxRef");

                DataSet dsInsuranceDet = new DataSet();

                //Generate usertrackid
                DataTable dtAgentDetails = new DataTable("AGENTDETAILS");
                dtAgentDetails.Columns.Add("AGENTID");
                dtAgentDetails.Columns.Add("TerminalID");
                dtAgentDetails.Columns.Add("Location");
                dtAgentDetails.Columns.Add("UserID");
                dtAgentDetails.Columns.Add("AgentType");

                DataTable dtCredentails = new DataTable("CREDENTIALS");
                dtCredentails.Columns.Add("UserName");
                dtCredentails.Columns.Add("Password");
                dtCredentails.Columns.Add("TrackID");

                DataTable dtPlanDetails = new DataTable();
                dtPlanDetails.Columns.Add("FamilyFlag");
                dtPlanDetails.Columns.Add("TravelPlan");
                dtPlanDetails.Columns.Add("AreaPlan");
                dtPlanDetails.Columns.Add("FromDate");
                dtPlanDetails.Columns.Add("ToDate");

                DataTable dtContactDetails = new DataTable();
                dtContactDetails.Columns.Add("Telephone");
                dtContactDetails.Columns.Add("Contact");
                dtContactDetails.Columns.Add("E-Mail");
                dtContactDetails.Columns.Add("Address");
                dtContactDetails.Columns.Add("City");
                dtContactDetails.Columns.Add("PostCode");
                dtContactDetails.Columns.Add("FaxCode");

                if (Paxdet.Contains('|'))
                {
                    Paxdet = Paxdet.Remove(Paxdet.Length - 1);
                    paxdetails = Paxdet.Split('|');
                    for (int i = 0; i < paxdetails.Length; i++)
                    {
                        if (paxdetails[i].Contains("SPLITSCRIPT"))
                        {
                            Ipax = Regex.Split(paxdetails[i], "SPLITSCRIPT");
                            DataRow dRowFormPaxForIns = dtFormPaxForIns.NewRow();
                            dRowFormPaxForIns["TITLE"] = Ipax[0].ToString();
                            //dRowFormPaxForIns["FIRSTNAME"] = Regex.Replace(Ipax[1], @"[^0-9a-zA-Z\\s]+$", "");// Ipax[1].ToString();
                            //dRowFormPaxForIns["LASTNAME"] = Regex.Replace(Ipax[2], @"[^0-9a-zA-Z\\s]+$", ""); //Ipax[2].ToString();
                            dRowFormPaxForIns["FIRSTNAME"] = Regex.Replace(Ipax[1], @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+", "");// Ipax[1].ToString();
                            dRowFormPaxForIns["LASTNAME"] = Regex.Replace(Ipax[2], @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+", ""); //Ipax[2].ToString();
                            dRowFormPaxForIns["DOB"] = Ipax[4].ToString();
                            dRowFormPaxForIns["GENDER"] = Ipax[3] == "M" ? "Male" : "Female";
                            dRowFormPaxForIns["PASSPORT"] = string.Empty;
                            dRowFormPaxForIns["PaxRef"] = Convert.ToString(i + 1);
                            dRowFormPaxForIns["ASSIGNEE"] = Ipax[11].ToString();

                            dtFormPaxForIns.Rows.Add(dRowFormPaxForIns);
                        }
                    }
                }
                else
                {
                    if (Paxdet.Contains("SPLITSCRIPT"))
                    {
                        Ipax = Regex.Split(Paxdet, "SPLITSCRIPT");
                        DataRow dRowFormPaxForIns = dtFormPaxForIns.NewRow();
                        dRowFormPaxForIns["TITLE"] = Ipax[0].ToString();
                        //dRowFormPaxForIns["FIRSTNAME"] = Regex.Replace(Ipax[1], @"[^0-9a-zA-Z\\s]+$", ""); //Ipax[1].ToString();
                        //dRowFormPaxForIns["LASTNAME"] = Regex.Replace(Ipax[2], @"[^0-9a-zA-Z\\s]+$", "");//Ipax[2].ToString();
                        dRowFormPaxForIns["FIRSTNAME"] = Regex.Replace(Ipax[1], @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+", ""); //Ipax[1].ToString();
                        dRowFormPaxForIns["LASTNAME"] = Regex.Replace(Ipax[2], @"[ ](?=[ ])_-|[^A-Za-z0-9 ]+", "");//Ipax[2].ToString();
                        dRowFormPaxForIns["DOB"] = Ipax[4].ToString();
                        dRowFormPaxForIns["GENDER"] = Ipax[3] == "M" ? "Male" : "Female";
                        dRowFormPaxForIns["PASSPORT"] = string.Empty;
                        dRowFormPaxForIns["PaxRef"] = Convert.ToString(1);
                        dRowFormPaxForIns["ASSIGNEE"] = Ipax[11].ToString();

                        dtFormPaxForIns.Rows.Add(dRowFormPaxForIns);
                    }
                }

                Addressdetails = Regex.Split(Insdetails, "SplItInS");

                if (Contactdet.Contains('|'))
                {
                    Contactdet = Contactdet.Remove(Contactdet.Length - 1);
                    contactdet = Contactdet.Split('|');
                    for (int i = 0; i < contactdet.Length; i++)
                    {
                        if (contactdet[i].Contains("SPLITSCRIPT"))
                        {
                            Icont = Regex.Split(contactdet[i], "SPLITSCRIPT");
                            DataRow dRowFormPaxForIns = dtContactDetails.NewRow();
                            Mobileno = Icont[1].ToString();
                            dRowFormPaxForIns["Telephone"] = "";
                            dRowFormPaxForIns["Contact"] = Icont[1].ToString();
                            dRowFormPaxForIns["E-Mail"] = Icont[2].ToString();
                            dRowFormPaxForIns["Address"] = Addressdetails[1].ToString();
                            dRowFormPaxForIns["City"] = Addressdetails[0].ToString();
                            dRowFormPaxForIns["PostCode"] = Addressdetails[2].ToString();
                            dRowFormPaxForIns["FaxCode"] = "";
                            dtContactDetails.Rows.Add(dRowFormPaxForIns);
                        }
                    }
                }
                else
                {
                    if (Contactdet.Contains("SPLITSCRIPT"))
                    {
                        Icont = Regex.Split(Contactdet, "SPLITSCRIPT");
                        DataRow dRowFormPaxForIns = dtContactDetails.NewRow();
                        Mobileno = Icont[1].ToString();
                        dRowFormPaxForIns["Telephone"] = "";
                        dRowFormPaxForIns["Contact"] = Icont[1].ToString();
                        dRowFormPaxForIns["E-Mail"] = Icont[2].ToString();
                        dRowFormPaxForIns["Address"] = Addressdetails[1].ToString();
                        dRowFormPaxForIns["City"] = Addressdetails[0].ToString();
                        dRowFormPaxForIns["PostCode"] = Addressdetails[2].ToString();
                        dRowFormPaxForIns["FaxCode"] = "";
                        dtContactDetails.Rows.Add(dRowFormPaxForIns);
                    }
                }

                string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();

                dtAgentDetails.Rows.Add(string.IsNullOrEmpty(ClientID) ? Session["POS_ID"].ToString() : ClientID, string.IsNullOrEmpty(ClientID) ? Session["POS_TID"].ToString() : ClientID + "01", Addressdetails[0].ToString(), Session["POS_TID"].ToString(), (strTerminalType == "T") ? (Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : Session["agenttype"].ToString()) : Session["agenttype"].ToString());

                RQRS.BookingRS resp_data = JsonConvert.DeserializeObject<RQRS.BookingRS>(responseval);

                string MainDepartureDate = string.Empty;
                if (resp_data != null)
                {
                    MainDepartureDate = resp_data.PnrDetails[0].DEPARTUREDATE;
                    SPNR = resp_data.PnrDetails[0].SPNR;
                    Air_PNR = resp_data.PnrDetails[0].AIRLINEPNR;
                    CRSPNR = resp_data.PnrDetails[0].CRSPNR;
                }

                DateTime policyToDate = Convert.ToDateTime(MainDepartureDate).AddDays(29);
                DateTime dtDT = Convert.ToDateTime(MainDepartureDate);
                string strDT = dtDT.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                string strPT = policyToDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                dtPlanDetails.Rows.Add("N", "E-Travel", "Within India Only", strDT, strPT);

                var qryInsurance = from p in dtFormPaxForIns.AsEnumerable()
                                   select new
                                   {
                                       TITLE = p.Field<string>("TITLE"),
                                       FIRSTNAME = p.Field<string>("firstName"),
                                       LASTNAME = p.Field<string>("LastName"),
                                       DOB = (DateTime.ParseExact(p.Field<string>("DOB").ToString(), "dd/MM/yyyy", null)).ToString("dd-MMM-yyyy").ToUpper(),
                                       GENDER = p.Field<string>("Gender").Contains("Male") ? "M" : p.Field<string>("Gender").Contains("Female") ? "F" : p.Field<string>("Gender"),
                                       PASSPORT = p.Field<string>("PASSPORT"),
                                       Relation = "      ",
                                       ASSIGNEE = string.Empty,
                                       PaxRef = p.Field<string>("PaxRef")
                                   };
                dtPassDetails = Serv.ConvertToDataTable(qryInsurance);
                dtPassDetails.Columns.Add("TRACKID");

                dtPlanDetails.TableName = "PLANDETAILS";
                dtPassDetails.TableName = "PAXDETAILS";
                dtContactDetails.TableName = "CONTACTDETAILS";
                dtAgentDetails.TableName = "AGENTDETAILS";

                dsInsuranceDet.Tables.Add(dtPassDetails.Copy());
                dsInsuranceDet.Tables.Add(dtAgentDetails.Copy());
                dsInsuranceDet.Tables.Add(dtPlanDetails.Copy());
                dsInsuranceDet.Tables.Add(dtContactDetails.Copy());
                dsInsuranceDet.Tables.Add(dtCredentails.Copy());
                string strResult = string.Empty;
                string strInsurrancerror = string.Empty;
                string strRturnMsg = string.Empty;
                DataSet dsAPIResponse = new DataSet();
                DataSet dsIssued = new DataSet();

                string reqtime = "Bookreqtime" + DateTime.Now;

                _raysservice.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString(); string strAGENT_TDSPERCENTAGE = (Session["AGN_TDSPERCENTAGE"] != null && Session["AGN_TDSPERCENTAGE"] != "") ? Session["AGN_TDSPERCENTAGE"].ToString() : "";
                bool Return = _raysservice.IssuePolicy(Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(), Session["TERMINALTYPE"].ToString(), Convert.ToDecimal(Session["sequenceid"].ToString()), dsInsuranceDet, SPNR, CRSPNR, Air_PNR, "", "", "BookFlight", Mobileno, ref strResult, ref strInsurrancerror, ref strRturnMsg, ref dsAPIResponse, MainDepartureDate, ref dsIssued, PaymentMode, strAGENT_TDSPERCENTAGE);

                string restime = "Bookrestime" + DateTime.Now;

                string Reqdet = "<REQUEST><REQTIME>" + reqtime + "</REQTIME><POSID>" + Session["POS_ID"].ToString() + "</POSID><POSTID>" + Session["POS_TID"].ToString() + "</POSTID><USERNAME>" + Session["username"].ToString()
                             + "</USERNAME><IPADDRESS>" + Session["ipAddress"].ToString() + "</IPADDRESS><TERMINALTYPE>" + Session["TERMINALTYPE"].ToString() + "</TERMINALTYPE><DATASETREQ>" + dsInsuranceDet.GetXml() + "</DATASETREQ><SPNR>" + SPNR + "</SPNR><CRSPNR>" + CRSPNR + "</CRSPNR><AIRPNR>" + Air_PNR
                             + "</AIRPNR><MOBILENO>" + Mobileno + "</MOBILENO><PAYMENTMODE>" + PaymentMode + "</PAYMENTMODE><DEPTDATE>" + MainDepartureDate + "</DEPTDATE><TDSPERCENT>" + strAGENT_TDSPERCENTAGE + "</TDSPERCENT></REQUEST><RESPONSE><RESTIME>" + restime + "</RESTIME><STRRESULT>" + strResult + "</STRRESULT><STRINSURRANCERROR>" + strInsurrancerror + "</STRINSURRANCERROR><STRRTURNMSG>" +
                             strRturnMsg + "</STRRTURNMSG><DSAPIRESPONSE>" + dsAPIResponse.GetXml() + "</DSAPIRESPONSE><DSISSUED>" + dsIssued.GetXml() + "</DSISSUED></RESPONSE>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "BajajInsurancebooking", "BajajInsurancebooking", Reqdet.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                if (Return == false)
                {
                    response = !string.IsNullOrEmpty(strInsurrancerror) ? strInsurrancerror : !string.IsNullOrEmpty(strRturnMsg) ? strRturnMsg : "";
                }
                else
                {
                    response = "SUCCESS";
                }

            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "BajajInsurancebooking", "BajajInsurancebooking", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            return response;
        }

        public string BookbajajInsurance(string responseval, string Insdetails, List<RQRS.PaxDetails> lstPaxDetails, string Contactdet, string ClientID, string PaymentMode
            , string strBranchID, string strPGTrackID, string gstdeta)
        {
            string response = string.Empty;
            string[] paxdetails, Ipax, paxinfo, contactdet, Icont, Addressdetails;
            RaysService _raysservice = new RaysService();
            DataTable dtPassDetails = new DataTable();
            string SPNR = string.Empty;
            string Air_PNR = string.Empty;
            string CRSPNR = string.Empty;
            string Mobileno = string.Empty;
            string strAirlineTrackID = string.Empty;
            string strAgentID = string.Empty;
            string strTerminalID = string.Empty;
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strAgentType = (strTerminalType == "T") ? (Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : Session["agenttype"].ToString()) : Session["agenttype"].ToString();
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string strSequenceID = Session["username"] != null ? Session["sequenceid"].ToString() : DateTime.Now.ToString("ddMMyyyy");
            string strAgentLocation = string.Empty;
            if (strTerminalType == "T" && ClientID != "")
            {
                strAgentID = ClientID.Substring(0, (ClientID.Length - 2));
                strTerminalID = ClientID;
            }
            else
            {
                strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                strTerminalID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            }
            try
            {
                #region UsageLog
                string PageName = "Book BajajInsurance";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "BOOK");
                }
                catch (Exception e) { }
                #endregion
                RQRS.BookingRS resp_data = JsonConvert.DeserializeObject<RQRS.BookingRS>(responseval);
                string MainDepartureDate = string.Empty;
                if (resp_data != null)
                {
                    MainDepartureDate = resp_data.PnrDetails[0].DEPARTUREDATE;
                    SPNR = resp_data.PnrDetails[0].SPNR;
                    Air_PNR = resp_data.PnrDetails[0].AIRLINEPNR;
                    CRSPNR = resp_data.PnrDetails[0].CRSPNR;
                    strAirlineTrackID = resp_data.TrackId;
                }

                string strResult = string.Empty;
                string strRturnMsg = string.Empty;

                #region New Common insurance

                string strParentTrackID = "INSA" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + strTerminalID;

                Insurance_Common.IssuePolicyRQ _IssuePolicyRQ = new Insurance_Common.IssuePolicyRQ();
                Insurance_Common.IssuePolicyRS _IssuePolicyRS = new Insurance_Common.IssuePolicyRS();
                Insurance_Common.AgentDetails _AgentDetails = new Insurance_Common.AgentDetails();
                Insurance_Common.PlanDetails _PlanDetails = new Insurance_Common.PlanDetails();
                Insurance_Common.FlightDetails _FlightDetails = new Insurance_Common.FlightDetails();
                Insurance_Common.GST_Details _GSTDetails = new Insurance_Common.GST_Details();
                Insurance_Common.ContactDetails _ContactDetails = new Insurance_Common.ContactDetails();
                Insurance_Common.StudentDetails _StudentDetails = new Insurance_Common.StudentDetails();
                Insurance_Common.StudentSponsorDetails _StudentSponsorDetails = new Insurance_Common.StudentSponsorDetails();
                Insurance_Common.Payment _Payment = new Insurance_Common.Payment();
                Insurance_Common.Credentials _Credentials = new Insurance_Common.Credentials();
                List<Insurance_Common.Passengerdetails> _lstPassengerdetails = new List<Insurance_Common.Passengerdetails>();
                Insurance_Common.Passengerdetails _PassengerDetails = new Insurance_Common.Passengerdetails();
                List<Insurance_Common.ERP_Attribute> _LST_ERP_Attribute = new List<Insurance_Common.ERP_Attribute>();
                Insurance_Common.ERP_Attribute _ERP_Attribute = new Insurance_Common.ERP_Attribute();

                string strAgencyContactNo = string.Empty;
                string strAgencyEmaiID = string.Empty;
                try
                {
                    Addressdetails = Regex.Split(Insdetails, "SplItInS");
                    string AgencyDetails = Session["AgencyDetailsformail"].ToString();
                    string[] strAgencyDetails = AgencyDetails.Split('~');
                    strAgencyEmaiID = strAgencyDetails.Length > 2 ? strAgencyDetails[1].ToString() : "";
                    strAgentLocation = Addressdetails != null && Addressdetails.Length > 0 ? Addressdetails[0] : "";
                    strAgencyContactNo = strAgencyDetails.Length > 4 ? strAgencyDetails[3].ToString() : "";
                }
                catch (Exception)
                {
                }

                _AgentDetails.AgentId = strAgentID;
                _AgentDetails.TerminalId = strTerminalID;
                _AgentDetails.PosId = strAgentID;
                _AgentDetails.PostId = strTerminalID;
                _AgentDetails.BranchId = strBranchID;
                _AgentDetails.IssuingBranchId = strBranchID;
                _AgentDetails.ClientID = strAgentID;
                _AgentDetails.WinyatraId = "";
                _AgentDetails.AppType = "B2B";
                _AgentDetails.BOAID = strAgentID;
                _AgentDetails.BOATerminalID = strTerminalID;
                _AgentDetails.UserName = strUserName;
                _AgentDetails.Version = "";
                _AgentDetails.Environment = strTerminalType;
                _AgentDetails.AgentType = strAgentType;
                _AgentDetails.ProductType = "";
                _AgentDetails.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                _AgentDetails.ContactNo = strAgencyContactNo;
                _AgentDetails.Emailid = strAgencyEmaiID;

                DateTime policyToDate = Convert.ToDateTime(MainDepartureDate).AddDays(29);
                DateTime dtDT = Convert.ToDateTime(MainDepartureDate);
                string strDT = dtDT.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);// dtDT.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
                string strPT = policyToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture); // policyToDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

                _PlanDetails.PlanName = "E-Travel";
                _PlanDetails.PlanCode = "E-Travel";
                _PlanDetails.PlanType = "E-Travel";
                _PlanDetails.TravelType = "Individual";
                _PlanDetails.PlanDescription = "Within India - Domestic";
                _PlanDetails.FromDate = strDT;
                _PlanDetails.ToDate = strPT;
                _PlanDetails.FamilyFlag = "N";
                _PlanDetails.MinAge = "18";
                _PlanDetails.MaxAge = "65";
                _PlanDetails.MinDays = "1";
                _PlanDetails.MaxDays = "30";
                _PlanDetails.IsPermanentSameasCommAddr = "true";
                _PlanDetails.IsInsuredOnImmigrantVisa = "";
                _PlanDetails.IsTravelInvolvesSportingActivities = "false";
                _PlanDetails.SportsActivitiesID = "";
                _PlanDetails.PreExistDiseaseID = "NO";
                _PlanDetails.IsSufferingFromPEMC = "false";
                _PlanDetails.SeniorCitizenPlanID = "";
                _PlanDetails.AddOnBnifitsOpted = "false";
                _PlanDetails.NoOfYears = "";
                _PlanDetails.PolicyLink = "";
                _PlanDetails.PlanDescCode = "Within India - Domestic";
                _PlanDetails.BenefitCode = "01001";
                _PlanDetails.NoOfUnits = "1";
                _PlanDetails.Retoken = ""; ;
                _PlanDetails.Stock = "BAJAJ ETRAVEL";
                _PlanDetails.PlanTypeCode = "E-Travel";
                _PlanDetails.CoverageTypeID = "";
                _PlanDetails.MaxDaysPerTrip = "";

                _IssuePolicyRQ.Supplierid = "";

                //ContactDetails
                Addressdetails = Regex.Split(Insdetails, "SplItInS");
                if (Contactdet.Contains('|'))
                {
                    Contactdet = Contactdet.Remove(Contactdet.Length - 1);
                    contactdet = Contactdet.Split('|');
                    for (int i = 0; i < contactdet.Length; i++)
                    {
                        if (contactdet[i].Contains("SPLITSCRIPT"))
                        {
                            Icont = Regex.Split(contactdet[i], "SPLITSCRIPT");
                            Mobileno = Icont[1].ToString();
                            _ContactDetails.PrimaryAddressDetails = Addressdetails[1].ToString();
                            _ContactDetails.SecondaryAddressDetails = Addressdetails[1].ToString();
                            _ContactDetails.Location = "";
                            _ContactDetails.CityName = Addressdetails[0].ToString();
                            _ContactDetails.CountryName = "";
                            _ContactDetails.StateName = "";
                            _ContactDetails.ContactNumber = Icont[1].ToString();
                            _ContactDetails.PostCode = Addressdetails[2].ToString();
                            _ContactDetails.EmailID = Icont[2].ToString();
                            _ContactDetails.TelephoneNumber = "";
                            _ContactDetails.CityID = "";
                            _ContactDetails.StateID = "";
                            _ContactDetails.DistrictID = "";
                            _ContactDetails.District = "";
                            _ContactDetails.ContactName = "";
                        }
                    }
                }
                else
                {
                    if (Contactdet.Contains("SPLITSCRIPT"))
                    {
                        Icont = Regex.Split(Contactdet, "SPLITSCRIPT");
                        Mobileno = Icont[1].ToString();
                        _ContactDetails.PrimaryAddressDetails = Addressdetails[1].ToString();
                        _ContactDetails.SecondaryAddressDetails = Addressdetails[1].ToString();
                        _ContactDetails.Location = "";
                        _ContactDetails.CityName = Addressdetails[0].ToString();
                        _ContactDetails.CountryName = "";
                        _ContactDetails.StateName = "";
                        _ContactDetails.ContactNumber = Icont[1].ToString();
                        _ContactDetails.PostCode = Addressdetails[2].ToString();
                        _ContactDetails.EmailID = Icont[2].ToString();
                        _ContactDetails.TelephoneNumber = "";
                        _ContactDetails.CityID = "";
                        _ContactDetails.StateID = "";
                        _ContactDetails.DistrictID = "";
                        _ContactDetails.District = "";
                        _ContactDetails.ContactName = "";
                    }
                }
                _PlanDetails.ContactDetails = _ContactDetails;
                //ContactDetails

                _FlightDetails.ReturnCarrierCode = "";
                _FlightDetails.ReturnDateTime = "";
                _FlightDetails.ReturnFlightNumber = "";
                _FlightDetails.AirlinePNR = "";
                _FlightDetails.FlightNumber = "";
                _FlightDetails.CarrierCode = "";
                _FlightDetails.Origin = "";
                _FlightDetails.Destination = "";
                _FlightDetails.DepartureDateTime = "";
                _FlightDetails.DepCountryCode = "";
                _FlightDetails.ArrvCountryCode = "";
                _FlightDetails.DepCurrencyCode = "";
                _FlightDetails.Airline_SPNR = SPNR;

                if (gstdeta != null && gstdeta != "")
                {
                    string[] splgstdeta = gstdeta.Split('~');
                    _GSTDetails.GSTNumber = splgstdeta[0];
                    _GSTDetails.GSTCompanyName = splgstdeta[1];
                    _GSTDetails.GSTAddress = splgstdeta[2];
                    _GSTDetails.GSTEmailID = splgstdeta[3];
                    _GSTDetails.GSTMobileNumber = splgstdeta[4];
                    _GSTDetails.GSTCityCode = "";
                    _GSTDetails.GSTStateCode = "";
                    _GSTDetails.GSTPincode = "";
                }
                else
                {
                    _GSTDetails.GSTNumber = "";
                    _GSTDetails.GSTCompanyName = "";
                    _GSTDetails.GSTAddress = "";
                    _GSTDetails.GSTEmailID = "";
                    _GSTDetails.GSTMobileNumber = "";
                    _GSTDetails.GSTCityCode = "";
                    _GSTDetails.GSTStateCode = "";
                    _GSTDetails.GSTPincode = "";
                }

                _StudentDetails.UniversityName = "";
                _StudentDetails.UniversityCountryId = "";
                _StudentDetails.UniversityStateName = "";
                _StudentDetails.CityName = "";
                _StudentDetails.PhoneNumber = "";
                _StudentDetails.MobileNumber = "";
                _StudentDetails.EmailId = "";
                _StudentDetails.Fax = "";
                _StudentDetails.CourseDuration = "";
                _StudentDetails.NoOfSems = "";

                _StudentSponsorDetails.SponsorName = "";
                _StudentSponsorDetails.Address1 = "";
                _StudentSponsorDetails.Pincode = "";
                _StudentSponsorDetails.CountryID = "";
                _StudentSponsorDetails.MobileNo = "";

                DataTable dtInsCredential = (DataTable)Session["INSURANCEDETAILS"];
                if (dtInsCredential != null && dtInsCredential.Rows.Count > 0)
                {
                    _Payment.PaymentMode = PaymentMode;
                    _Payment.BaseFare = string.IsNullOrEmpty(dtInsCredential.Rows[0]["BaseFare"].ToString()) ? "0" : dtInsCredential.Rows[0]["BaseFare"].ToString();
                    _Payment.ServiceTax = string.IsNullOrEmpty(dtInsCredential.Rows[0]["ServiceTax"].ToString()) ? "0" : dtInsCredential.Rows[0]["ServiceTax"].ToString();
                    _Payment.GrossFare = string.IsNullOrEmpty(dtInsCredential.Rows[0]["GrossFare"].ToString()) ? "0" : dtInsCredential.Rows[0]["GrossFare"].ToString();
                    _Payment.Commission = "0";
                    _Payment.Incentive = "0";
                    _Payment.Markup = "0";
                    _Payment.Servicecharge = "0";
                    _Payment.Flatfare = "0";
                    _Payment.Breakup = string.IsNullOrEmpty(dtInsCredential.Rows[0]["Breakup"].ToString()) ? "0" : dtInsCredential.Rows[0]["Breakup"].ToString();
                    _Payment.TDSfare = "0";
                    // _PlanDetails.Stock = string.IsNullOrEmpty(dtInsCredential.Rows[0]["Stock"].ToString()) ? "0" : dtInsCredential.Rows[0]["Stock"].ToString();
                    _IssuePolicyRQ.Supplierid = string.IsNullOrEmpty(dtInsCredential.Rows[0]["Supplierid"].ToString()) ? "0" : dtInsCredential.Rows[0]["Supplierid"].ToString();
                }

                _Credentials.Agency_Name = "";
                _Credentials.Branch_Name = "";
                _Credentials.Branch_ID = strBranchID;
                _Credentials.City_Name = strAgentLocation;
                _Credentials.State_Name = "";
                _Credentials.PCC = "";

                #region Dynamic ERP Attributes
                //_ERP_Attribute = new Insurance_Common.ERP_Attribute();
                //_ERP_Attribute.AttributesName = "";
                //_ERP_Attribute.AttributesValue = "";
                //_ERP_Attribute.PaxRefNumber = "";
                //_LST_ERP_Attribute.Add(_ERP_Attribute);
                #endregion

                _IssuePolicyRQ.AgentDetails = _AgentDetails;
                _IssuePolicyRQ.PlanDetails = _PlanDetails;
                _IssuePolicyRQ.FlightDetails = _FlightDetails;
                _IssuePolicyRQ.GST_Details = _GSTDetails;
                _IssuePolicyRQ.StudentDetails = _StudentDetails;
                _IssuePolicyRQ.StudentSponsorDetails = _StudentSponsorDetails;
                _IssuePolicyRQ.Credentials = _Credentials;
                _IssuePolicyRQ.Payment = _Payment;
                _IssuePolicyRQ.Stock = "BAJAJ ETRAVEL";
                _IssuePolicyRQ.Category = "TRAVEL";
                _IssuePolicyRQ.MONumber = "";
                _IssuePolicyRQ.SelectToken = "";
                _IssuePolicyRQ.PgTrackid = strPGTrackID;
                _IssuePolicyRQ.Trackid = "";
                _IssuePolicyRQ.TrackCreate = "true";
                _IssuePolicyRQ.MultiRequest = "true";
                _IssuePolicyRQ.BookingType = "Airline";
                _IssuePolicyRQ.TrackPending = "false";
                _IssuePolicyRQ.ParentTrackid = strParentTrackID;
                _IssuePolicyRQ.AirlineTrackid = strAirlineTrackID;
                _IssuePolicyRQ.AirlineSPNR = SPNR;
                _IssuePolicyRQ.ERP_Attributes = _LST_ERP_Attribute;

                //Multirequest using Passenger Details
                for (int i = 0; i < lstPaxDetails.Count; i++)
                {
                    #region Pax Details
                    List<Insurance_Common.Passengerdetails> _lstSinglePassengerdetails = new List<Insurance_Common.Passengerdetails>();
                    _PassengerDetails = new Insurance_Common.Passengerdetails();

                    _PassengerDetails.PaxRefNumber = "1";
                    _PassengerDetails.Title = lstPaxDetails[i].Title;
                    _PassengerDetails.FirstName = lstPaxDetails[i].FirstName;
                    _PassengerDetails.LastName = lstPaxDetails[i].LastName;
                    _PassengerDetails.DOB = lstPaxDetails[i].DOB;
                    _PassengerDetails.Gender = lstPaxDetails[i].Gender;
                    _PassengerDetails.PaxType = lstPaxDetails[i].PaxType;
                    _PassengerDetails.PassportNo = lstPaxDetails[i].PassportNo;
                    _PassengerDetails.Mobnumber = lstPaxDetails[i].Mobnumber;
                    _PassengerDetails.EMailID = lstPaxDetails[i].MailID;
                    _PassengerDetails.Assignee = lstPaxDetails[i].Assignee;
                    _PassengerDetails.Age = Math.Round((DateTime.Now - (DateTime.ParseExact(lstPaxDetails[i].DOB, "dd/MM/yyyy", null))).TotalDays / 365.242199).ToString();
                    _PassengerDetails.Relation = "";

                    _PassengerDetails.IsVisitingUSACanada = "";
                    _PassengerDetails.VisitingCountriesID = "";
                    _PassengerDetails.IsResidingInIndia = "";
                    _PassengerDetails.PassportIssuingCountry = "";
                    _PassengerDetails.NomineeID = "";
                    _PassengerDetails.NomineeName = "";
                    _PassengerDetails.PermanentResidenceCountry = "";
                    _PassengerDetails.IsAddOnCover = "";
                    _PassengerDetails.PreExistingIllness = "";
                    _PassengerDetails.SufferingSince = "";
                    _PassengerDetails.strOccupationID = "";
                    _PassengerDetails.strIsIndianCitizen = "";
                    _PassengerDetails.Ridercode = "";
                    _PassengerDetails.RiderNames = "";
                    _PassengerDetails.RiderPercent = "";
                    _PassengerDetails.RiderAmount = "";
                    _PassengerDetails.Nationality = "";
                    _PassengerDetails.AirlinePaxRefNumber = (i + 1).ToString();
                    _lstSinglePassengerdetails.Add(_PassengerDetails);

                    #endregion

                    _IssuePolicyRQ.PlanDetails.Passengerdetails = _lstSinglePassengerdetails;

                    string strURLpath = ConfigurationManager.AppSettings["INSURANCEAPIURL"].ToString();
                    string strMethod = "IssuePolicy";
                    int bookingtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                    string strRequest = JsonConvert.SerializeObject(_IssuePolicyRQ).ToString();

                    string ReqTime = "IssuePolicy_REQ" + DateTime.Now;
                    string XmlData = "<IssuePolicy" + SPNR + "_REQ" + (i + 1).ToString() + "><URL>[<![CDATA[" + strURLpath + strMethod + "]]>]</URL><TIMEOUT>" +
                        (bookingtimeout).ToString() + "</TIMEOUT><JSON>" + strRequest + "</JSON><TIME>" + ReqTime + "</TIME></IssuePolicy" + SPNR + "_REQ" + (i + 1).ToString() + ">";

                    DatabaseLog.LogData(strUserName, "E", "BajajInsurancebooking", "BajajInsurancebooking", XmlData, strAgentID, strTerminalID, strSequenceID);

                    MyWebClient client = new MyWebClient();
                    client.LintTimeout = bookingtimeout;
                    client.Headers["Content-type"] = "application/json";

                    byte[] data = client.UploadData(strURLpath + strMethod, "POST", System.Text.Encoding.ASCII.GetBytes(strRequest));
                    string strResponse = System.Text.Encoding.ASCII.GetString(data);


                    string ResTime = "IssuePolicy_RES" + DateTime.Now;
                    XmlData = "<IssuePolicy" + SPNR + "_RES" + (i + 1).ToString() + "><URL>[<![CDATA[" + strURLpath + "]]>]</URL><TIMEOUT>" +
                        (bookingtimeout).ToString() + "</TIMEOUT><JSON><![CDATA[" + strResponse + "]]></JSON><TIME>" + ResTime + "</TIME></IssuePolicy" + SPNR + "_RES" + (i + 1).ToString() + ">";

                    DatabaseLog.LogData(strUserName, "E", "BajajInsurancebooking", "BajajInsurancebooking", XmlData, strAgentID, strUserName, strSequenceID);

                    if (!string.IsNullOrEmpty(strResponse))
                    {
                        _IssuePolicyRS = JsonConvert.DeserializeObject<Insurance_Common.IssuePolicyRS>(strResponse);

                        if (_IssuePolicyRS.Status.ResultCode == "1")
                        {
                            strRturnMsg += _IssuePolicyRS.IssuePolicyDetails.PolicyNumber;
                            //dtnew.Rows.Add(_IssuePolicyRS.PlanDetails.Passengerdetails[0].FirstName.Trim(), _IssuePolicyRS.PlanDetails.Passengerdetails[0].LastName.Trim(), _IssuePolicyRS.IssuePolicyDetails.PolicyNumber);
                            _IssuePolicyRQ.TrackPending = "false";
                        }
                        else
                        {
                            _IssuePolicyRQ.TrackPending = "true";
                            string str = (string.IsNullOrEmpty(_IssuePolicyRS.Status.ErrorMessage) ? "Unable To Process the insurance Request" : _IssuePolicyRS.Status.ErrorMessage);
                            strRturnMsg += str;
                        }
                    }
                    else
                    {
                        _IssuePolicyRQ.TrackPending = "true";
                        strRturnMsg += "Unable To Process the insurance Request.";
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(strUserName, "X", "BajajInsurancebooking", "BajajInsurancebooking", ex.ToString(), strAgentID, strTerminalID, strSequenceID);
            }
            return response;
        }

        public ActionResult Asyncbookingresponse(string Bookresponse, string Responseflag)
        {
            ArrayList resultarray = new ArrayList();
            resultarray.Add("");
            resultarray.Add("");
            string airportCategory = "I";
            string ReqTime = DateTime.Now.ToString("yyyyMMdd");
            int error = 0;
            int responsestate = 1;
            bool BlockTicket = false;

            string ReturnError = string.Empty;

            string stu = string.Empty;
            string msg = string.Empty;

            if (Session["BLOCKREQ"] != null && Session["BLOCKREQ"].ToString() != "")
            {
                BlockTicket = true;
            }


            try
            {
                RaysService _rays_servers = new RaysService();
                string strBranchID = (Session["branchid"] != null && Session["branchid"].ToString() != "" ? Session["branchid"].ToString() : "");
                string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"] != "") ? Session["TKTFLAG"].ToString() : "DTKT";
                string strClientBranchID = Session["ClientBranchID"] != null && Session["ClientBranchID"].ToString() != "" ? Session["ClientBranchID"].ToString() : "";

                #region SERVICE URL BRANCH BASED -- STS115
                if (strBranchCredit != "")
                {
                    if (strBranchCredit == "ALL" || (strClientBranchID != "" && strBranchCredit.Contains(strClientBranchID)))
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
                #endregion

                string Query = "InvokeBooking";
                int bookingtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);

                ReturnError = string.IsNullOrEmpty(Responseflag) ? "" : Responseflag;

                if (Bookresponse != "NULL" && Bookresponse != null && Bookresponse != "")
                {
                    var RetVal = JsonConvert.DeserializeObject<STSTRAVRAYS.Models.RQRS.BookingRS>(Bookresponse);

                    string lstrBookingdetail = (BlockTicket ? "<BLOKING_RESPONSE>" : "<BOOKING_RESPONSE>") +
                                           ((RetVal != null && (RetVal.ResultCode == "1")) ?
                                           ("<STATUS>SUCCESS</STATUS>") :
                                           ((RetVal != null && (RetVal.ResultCode == "2")) ?
                                           ("<STATUS>FARECHANGE</STATUS>") : ("<STATUS>FAILED</STATUS>"))) +
                                           ((RetVal != null && RetVal.ResultCode != null && RetVal.ResultCode.ToString().Trim() != "") ?
                                           ("<ResultCode>" + RetVal.ResultCode.ToString().Trim() + "</ResultCode>") : ("<ResultCode>Empty/Null</ResultCode>")) +
                                           ((RetVal != null && RetVal.Sqe != null && RetVal.Sqe.ToString().Trim() != "") ?
                                           ("<Sqe>" + RetVal.Sqe.ToString().Trim() + "</Sqe>") : ("<Sqe>Empty/Null</Sqe>")) +
                                           ((RetVal != null && RetVal.Error != null && RetVal.Error.ToString().Trim() != "") ?
                                           ("<Error>" + RetVal.Error.ToString().Trim() + "</Error>") : ("<Error>Empty/Null</Error>")) +
                                           ((RetVal != null && RetVal.TrackId != null && RetVal.TrackId.ToString().Trim() != "") ?
                                           ("<TrackId>" + RetVal.TrackId.ToString().Trim() + "</TrackId>") : ("<TrackId>Empty/Null</TrackId>")) +
                                           (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].SPNR != null ?
                                           ("<SPNR>" + RetVal.PnrDetails[0].SPNR.ToString().Trim() + "</SPNR>") : "") +
                                           (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].AIRLINEPNR != null ?
                                           ("<AIRLINEPNR>" + RetVal.PnrDetails[0].AIRLINEPNR.ToString().Trim() + "</AIRLINEPNR>") : "") +
                                           (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].CRSPNR != null ?
                                           ("<CRSPNR>" + RetVal.PnrDetails[0].CRSPNR.ToString().Trim() + "</CRSPNR>") : "") +
                                           (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].GROSSFARE != null ?
                                           ("<GROSSFARE>" + RetVal.PnrDetails[0].GROSSFARE.ToString().Trim() + "</GROSSFARE>") : "") +
                                           (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].TICKETINGCARRIER != null ?
                                           ("<TICKETINGCARRIER>" + RetVal.PnrDetails[0].TICKETINGCARRIER.ToString().Trim() + "</TICKETINGCARRIER>") : "") +
                                           (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].TRIPTYPE != null ?
                                           ("<TRIPTYPE>" + RetVal.PnrDetails[0].TRIPTYPE.ToString().Trim() + "</TRIPTYPE>") : "") +
                                           (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].OFFICEID != null ?
                                           ("<OFFICEID>" + RetVal.PnrDetails[0].OFFICEID.ToString().Trim() + "</OFFICEID>") : "") +
                                           (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].OFFLINEFLAG != null ?
                                           ("<OFFLINEFLAG>" + RetVal.PnrDetails[0].OFFLINEFLAG.ToString().Trim() + "</OFFLINEFLAG>") : "") +
                                           (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 ?
                                           ("<PnrDetailsCount>" + RetVal.PnrDetails.Count().ToString().Trim() + "</PnrDetailsCount>") : "") +
                                           "<AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><URL>[<![CDATA[" + Session["travraysws_url"] + "/" +
                                             Session["travraysws_vdir"] + "/Rays.svc/" + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (bookingtimeout).ToString() +
                                       "</TIMEOUT><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE_TIME>" +
                                       ReqTime +
                                        "</RESPONSE_TIME>" + (BlockTicket ? "</BLOKING_RESPONSE>" : "</BOOKING_RESPONSE>");

                    _rays_servers.Apps_Booking_Response_Receive_Status((RetVal != null && RetVal.TrackId != null &&
                               RetVal.TrackId.ToString().Trim() != "") ? RetVal.TrackId.Trim() : "", Session["POS_ID"].ToString() + " ( " + Session["agencyname"].ToString() + " )", Session["POS_TID"].ToString(),
                         Session["username"].ToString(), "W", Session["ipAddress"].ToString(), Session["sequenceid"].ToString(),
                         ref ReturnError, lstrBookingdetail, "FormBookingRequest");

                    DatabaseLog.LogData(Session["username"].ToString(), "E", "Bookingresponseupdate", "Bookingresponseupdate", "SUCCESS", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                }
                else
                {
                    string lstrBkingdetail = (BlockTicket ? "<BLOKING_RESPONSE>" : "<BOOKING_RESPONSE>") +
                                  ("<STATUS>FAILED</STATUS>") +
                                 ("<RESPONSE>Null</RESPONSE>") +
                                  "<AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME>" + (BlockTicket ? "</BLOKING_RESPONSE>" : "</BOOKING_RESPONSE>");
                    DatabaseLog.LogData(Session["username"].ToString(), "E", "Bookingresponseupdate", "Bookingresponseupdate", "SUCCESS", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }
                resultarray[responsestate] = "Success";

            }
            catch (Exception ex)
            {
                stu = "0";
                msg = "";
                resultarray[error] = "Error";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Bookingresponseupdate", "Bookingresponseupdate", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }

            return Json(new { Status = "", Message = "", Result = resultarray });

        }

        private string faresplit(string lstrSeq, ref string lstrpax)
        {
            lstrpax = lstrpax + "|" + lstrSeq;
            return lstrSeq;
        }

        public ActionResult Get_contactdetails_grid(string MobNo, string EmailId)
        {
            RaysService _rays_servers = new RaysService();

            string strTerminalId = string.Empty;
            string strUserName = string.Empty;
            string strAgentID = string.Empty;
            string terminalType = string.Empty;
            string strResponse = string.Empty;
            string Ipaddress = string.Empty;
            string sequnceID = string.Empty;

            ArrayList array_topup = new ArrayList();
            StringBuilder AgentTopup = new StringBuilder();
            StringBuilder stroptions = new StringBuilder();
            array_topup.Add("");
            array_topup.Add("");
            array_topup.Add("");
            array_topup.Add("");
            int Error = 0;
            int response = 1;
            int single_row = 2;
            int rows = 3;
            string strErrorMsg = string.Empty;
            DataSet daGetContact = new DataSet();

            string stu = string.Empty;
            string msg = string.Empty;


            try
            {
                strTerminalId = Session["POS_TID"].ToString();//"TIQAJ010000101";// 
                strUserName = Session["username"].ToString();//"sathees";//
                strAgentID = Session["POS_ID"].ToString();//"TIQAJ0100001";//
                terminalType = "W";
                strResponse = "";
                Ipaddress = Session["ipAddress"].ToString();//"1";// 
                sequnceID = Session["sequenceid"].ToString();//"1";//


                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"];
                daGetContact = _rays_servers.Fetch_ContactDetails(MobNo, EmailId, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["username"].ToString(), Session["ipAddress"].ToString(), "W", Convert.ToDecimal(Session["sequenceid"].ToString()), ref strErrorMsg, "", "", Session["agenttype"].ToString());


                if (daGetContact != null && daGetContact.Tables.Count > 0)// && daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows.Count > 0)
                {
                    if (daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows.Count > 1)
                    {

                        array_topup[response] = JsonConvert.SerializeObject(daGetContact).ToString();
                        array_topup[rows] = daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows.Count;
                        msg = "";
                        stu = "1";
                    }
                    else if (daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows.Count == 1)
                    {
                        array_topup[single_row] = daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["CONTACT_TITLE"].ToString().ToUpper() + "SPlIT"
                            + daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["FIRST_NAME"].ToString() + "SPlIT"
                            + daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["LAST_NAME"].ToString() + "SPlIT"
                            + daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["MOBILE_NO"].ToString() + "SPlIT"
                            + (daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["BUSINESS NO"].ToString().Contains('|') ? daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["BUSINESS NO"].ToString().Split('|')[1] : daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["BUSINESS NO"].ToString()) + "SPlIT"
                            + (daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["HOME NO"].ToString().Contains('|') ? daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["HOME NO"].ToString().Split('|')[1] : daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["HOME NO"].ToString()) + "SPlIT"
                            + daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["EMAIL_ID"].ToString() + "SPlIT"
                            + daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["ADDRESS"].ToString() + "SPlIT"
                            + daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["STATE_ID"].ToString() + "SPlIT"
                            + daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["CITY_ID"].ToString() + "SPlIT"
                            + daGetContact.Tables["P_FETCH_CONTACT_DETAILS"].Rows[0]["PINCODE"].ToString();

                        msg = "";
                        stu = "1";
                    }
                    else
                    {
                        array_topup[Error] = "Contact details not found.";
                        msg = "Contact details not found.";
                        stu = "0";
                    }
                }
                else
                {
                    array_topup[Error] = "Contact details not found.";
                    msg = "Contact details not found.";
                    stu = "0";
                }

            }
            catch (Exception ex)
            {
                msg = "Problem Occured.Please contact customercare.";
                stu = "0";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Get_contactdetails_grid", "Get_contactdetails_grid", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                array_topup[Error] = "Problem Occured.Please contact customercare";

            }
            //return array_topup;
            return Json(new { Status = stu, Message = msg, Result = array_topup });
        }

        #region Get Passenger Details New
        public ActionResult GetPassengerDetails(string strMobileNo, string strEmailID, string strPaxType, string strAction)
        {
            RaysService RaysService = new RaysService();
            string strStatus = string.Empty;
            string strMessage = string.Empty;
            string strLogData = string.Empty;
            ArrayList arrGetPassenger = new ArrayList();
            arrGetPassenger.Add("");
            arrGetPassenger.Add("");
            arrGetPassenger.Add("");
            int Error = 0;
            int Response = 1;
            int ResponseCount = 2;
            string strAgentID = string.Empty;
            string strTeriminalID = string.Empty;
            string strUsername = string.Empty;
            string strSequence = string.Empty;
            string strIPAddress = string.Empty;
            string strTerminalType = string.Empty;
            string strErrorMsg = string.Empty;
            string strAdult = string.Empty;
            string strChild = string.Empty;
            string strInfant = string.Empty;
            string strPaxList = string.Empty;
            DataSet dsPassengerDetails = new DataSet();
            try
            {
                strAgentID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
                strTeriminalID = Session["POS_TID"] != null && Session["POS_TID"].ToString() != "" ? Session["POS_TID"].ToString() : "";
                strUsername = Session["username"] != null && Session["username"].ToString() != "" ? Session["username"].ToString() : "";
                strSequence = Session["sequenceid"] != null && Session["sequenceid"].ToString() != "" ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyMMdd");
                strIPAddress = Session["ipAddress"] != null && Session["ipAddress"].ToString() != "" ? Session["ipAddress"].ToString() : "";

                if (strAgentID == "" || strTeriminalID == "" || strUsername == "" || strSequence == "" || strIPAddress == "")
                {
                    return new JsonResult()
                    {
                        Data = Json(new { Status = "-1", Message = "Session Expired.", Result = "" })
                    };
                }

                RaysService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                if (strAction.ToUpper() == "DELETE")
                {
                    strErrorMsg = "DELETE";
                    strMobileNo = strMobileNo.TrimEnd(',');
                }
                strAdult = (Convert.ToInt32(strPaxType.Split(',')[0]) != 0) ? "ADT" : "";
                strChild = (Convert.ToInt32(strPaxType.Split(',')[0]) != 0) ? "CHD" : "";
                strInfant = (Convert.ToInt32(strPaxType.Split(',')[0]) != 0) ? "INF" : "";
                strPaxList = strAdult + "," + strChild + "," + strInfant;

                strLogData = "<REQUEST>" + "<strMobileNo>"+ strMobileNo + "</strMobileNo>" + "<strEmailID>" + strEmailID + "</strEmailID>"
                                + "<strPaxList>" + strPaxList + "</strPaxList>" + "<strPaxList>" + strPaxList + "</strPaxList>"
                                + "<strAgentID>" + strAgentID + "</strAgentID>" + "<strTeriminalID>" + strTeriminalID + "</strTeriminalID>"
                                + "<strUsername>" + strUsername + "</strUsername>" + "<strIPAddress>" + strIPAddress + "</strIPAddress>"
                                + "<strTerminalType>" + strTerminalType + "</strTerminalType>" + "<strSequence>" + strSequence + "</strSequence>"
                                + "<strErrorMsg>" + strErrorMsg + "</strErrorMsg>" + "<AirlineCode>" + "" + "</AirlineCode>"
                                + "<AirportCategory>" + "" + "</AirportCategory>" + "</REQUEST>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "BookingController.cs", "GetPassengerDetails~REQ", strLogData, strAgentID, strTeriminalID, strSequence);

                dsPassengerDetails = RaysService.Fetch_PassengerDetails(strMobileNo, strEmailID, strPaxList, "", strAgentID, strTeriminalID, strUsername,
                                        strIPAddress, strTerminalType, Convert.ToDecimal(strSequence), ref strErrorMsg, "BookingController.cs", "GetPassengerDetails", "");

                if (dsPassengerDetails!= null && dsPassengerDetails.Tables.Count > 0 && dsPassengerDetails.Tables[0].Rows.Count > 0)
                {
                    arrGetPassenger[Response] = JsonConvert.SerializeObject(dsPassengerDetails);// pass_details.ToString();
                    arrGetPassenger[ResponseCount] = dsPassengerDetails.Tables["P_FETCH_PASSENGERDETAILS"].Rows.Count;
                    arrGetPassenger[Error] = strErrorMsg;
                    strStatus = "1";
                    strMessage = strErrorMsg;
                }
                else if (!string.IsNullOrEmpty(strErrorMsg) && strErrorMsg.ToUpper().Trim() == "DELETED SUCCESSFULLY")
                {
                    arrGetPassenger[Error] = strErrorMsg;
                    strStatus = "2";
                    strMessage = strErrorMsg;
                }
                else
                {
                    arrGetPassenger[Error] = strErrorMsg;
                    strStatus = "0";
                    strMessage = strErrorMsg;
                }
                strLogData += "<RESPONSE>" + "<STATUS>"+ strStatus + "</STATUS>" + "<ERROR>" + strErrorMsg + "</ERROR>" + "</RESPONSE>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "BookingController.cs", "GetPassengerDetails~RES", strLogData, strAgentID, strTeriminalID, strSequence);
            }
            catch (Exception ex)
            {
                strLogData += "<ERROR>" + ex.ToString() + "</ERROR>";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "BookingController.cs", "GetPassengerDetails~ERR", strLogData, strAgentID, strTeriminalID, strSequence);
                arrGetPassenger[Error] = "Problem Occured.Please contact customercare";
                strStatus = "0";
                strMessage = "Problem Occured.Please contact customercare";
            }
            return new JsonResult()
            {
                Data = Json(new { Status = strStatus, Message = strMessage, Result = arrGetPassenger }),
                MaxJsonLength = 2147483647
            };
        }
        #endregion

        public ActionResult InsertServiceAmount(string AdultAmount, string ChildAmount, string InfantAmount, string SPNR, string Adultmarkup, string Childmarkup, string Infantmarkup)
        {
            ArrayList Charge = new ArrayList();
            int Array_Result = 1;
            int Array_Error = 0;
            Charge.Add("");
            Charge.Add("");
            string output = string.Empty;
            string agentId = string.Empty;
            string terminalId = string.Empty;
            string strTerminalType = string.Empty;
            string loginUsername = string.Empty;
            string ipAddress = string.Empty;
            string sequenceId = string.Empty;
            string strBranchID = string.Empty;
            string strTKTFLAG = (Session["TKTFLAG"] != null && Session["TKTFLAG"].ToString() != "") ? Session["TKTFLAG"].ToString() : "";
            try
            {
                agentId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                terminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                strTerminalType = Session["TerminalType"] != null ? Session["TerminalType"].ToString() : "";
                loginUsername = Session["username"] != null ? Session["username"].ToString() : "";
                ipAddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                sequenceId = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyMMdd");
                strBranchID = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                RaysService _rays_servers = new RaysService();
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

                Adultmarkup = Adultmarkup == "undefined" ? "0" : Adultmarkup;
                Childmarkup = Childmarkup == "undefined" ? "0" : Childmarkup;
                Infantmarkup = Infantmarkup == "undefined" ? "0" : Infantmarkup;

                output = _rays_servers.Insert_ServiceCharge_AddlMgmtFee_Amount(AdultAmount.ToString(), ChildAmount.ToString(), InfantAmount.ToString(), Adultmarkup.ToString(),
                                        Childmarkup.ToString(), Infantmarkup.ToString(), SPNR, agentId, terminalId, loginUsername, strTerminalType, ipAddress, Convert.ToDecimal(sequenceId), "Insertservicecharge");

                #region LOG

                string LstrDetails = "<INSERT_SERVICECHARGE><ADULTAMOUNT>" + AdultAmount.ToString() + "</ADULTAMOUNT><CHILDAMOUNT>" + ChildAmount.ToString() +
                    "</CHILDAMOUNT><INFANTAMOUNT>" + InfantAmount.ToString() + "</INFANTAMOUNT><ADULTMARKUP>" + Adultmarkup.ToString() + "</ADULTMARKUP><CHILDAMOUNT>" + ChildAmount.ToString() +
                    "</CHILDAMOUNT><INFANTAMOUNT>" + InfantAmount.ToString() + "</INFANTAMOUNT><OUTPUT>" + output.ToString() +
                    "</OUTPUT><SPNR>" + SPNR.ToString() + "</SPNR></INSERT_SERVICECHARGE>";

                DatabaseLog.LogData(Session["username"].ToString(), "T", "INSERT_SERVICECHARGE_AFTER_BOOKING", "InsertServiceAmount", LstrDetails, agentId.ToString(), terminalId.ToString() + "(" + terminalId.ToString() + ")", sequenceId.ToString());

                #endregion

                if (output != "0")
                {
                    Charge[Array_Result] = ConfigurationManager.AppSettings["Producttype"] != "RIYA" ? "Service charge updated successfully" : "Management fee updated successfully";
                }
                else
                {
                    Charge[Array_Error] = "Problem occurred while update the details";
                }
                return Json(new { Status = "", Message = "", Result = Charge });
            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "INSERT_SERVICECHARGE_AFTER_BOOKING", "InsertServiceAmount", ex.ToString(), agentId, terminalId + "(" + Session["Availterminal"].ToString() + ")", sequenceId);

            }
            //return Charge;
            return Json(new { Status = "", Message = "", Result = Charge });
        }

        public ActionResult Bookinurance(string requeststr)
        {



            string ipAddress = string.Empty;
            string strUserName = string.Empty;
            string str_error_ref = string.Empty;
            Session["WEB"] = "WEBAPPS";
            DataSet dsSession = new DataSet();
            Session.Add("ipAddress", ipAddress);
            Session.Add("strSequencId", "123");
            RaysService _raysservice = new RaysService();
            _raysservice.Url = ConfigurationManager.AppSettings["serviceuri"].ToString();

            DataSet dsFlightDetails = new DataSet();

            string strResponse = "";
            ArrayList booking_Response = new ArrayList();
            DataSet dataset = new DataSet();

            booking_Response.Add("");
            booking_Response.Add("");
            booking_Response.Add("");
            booking_Response.Add("");
            booking_Response.Add("");
            booking_Response.Add("");
            booking_Response.Add("");
            booking_Response.Add("");
            booking_Response.Add("");
            booking_Response.Add("");
            int confirmdetails = 1;
            int ResultCode = 0;
            int fare = 2;
            int sequence = 3;
            int spnr = 4;
            int TrackId = 5;
            int SuccessResponse = 7;
            int FailedResponse = 8;
            int currency_ins = 9;
            string Query = "InvokePolicy";
            string LstrDetails = string.Empty;
            string ReqTime = string.Empty;

            int Availtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
            strURLpathT = strURLpathT.Replace("host", ConfigurationManager.AppSettings["bookinsurnace_URL"].ToString());
            strURLpathT = strURLpathT.Replace("Service", ConfigurationManager.AppSettings["bookinsurnace_VDIR"].ToString());

            MyWebClient client = new MyWebClient();
            client.Headers["Content-type"] = "application/json";
            client.LintTimeout = Availtimeout;

            ReqTime = DateTime.Now.ToString("yyyyMMdd");
            DateTime.Now.TimeOfDay.ToString().Replace(':', '_');



            LstrDetails = "<BOOKING_REQUEST><REQUEST>" + requeststr + "</REQUEST><URL>[<![CDATA[" + Session["TRAVRAYSTWSA_URL"] + "/" + Session["TRAVRAYSTWSA_VDIR"] + "/Tune.svc/"
           + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (Availtimeout).ToString() +
           "</TIMEOUT><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><REQUEST>[<![CDATA[" + requeststr + "]]>]</REQUEST></BOOKING_REQUEST>";
            DatabaseLog.LogData(Session["username"].ToString(), "E", "Tuneabooking", "BOOKING REQUEST", LstrDetails + requeststr.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

            byte[] data = client.UploadData(strURLpathT + Query, "POST", Encoding.ASCII.GetBytes(requeststr));
            strResponse = System.Text.Encoding.ASCII.GetString(data);

            LstrDetails = "<BOOKING_RESPONSE><RESPONSE>" + strResponse + "</RESPONSE><URL>[<![CDATA[" + strURLpathA + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (Availtimeout).ToString() +
           "</TIMEOUT><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE>[<![CDATA[" + strResponse + "]]>]</RESPONSE></BOOKING_RESPONSE>";
            DatabaseLog.LogData(Session["username"].ToString(), "E", "Tuneabooking", "BOOKING RESPONSE", LstrDetails + strResponse.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            if (string.IsNullOrEmpty(strResponse))
            {
                string empty_res = "(#-1)";
                booking_Response[ResultCode] = -1;
                booking_Response[FailedResponse] = "Problem occured. Please Contact Customercare." + empty_res;
            }
            else
            {
                var RetVal = JsonConvert.DeserializeObject<STSTRAVRAYS.Models.RQRS.IssuePolicyResponse>(strResponse);
                string RetVale = JsonConvert.SerializeObject(RetVal);
                dataset = Serv.convertJsonStringToDataSet(RetVale, "");


                if (RetVal.ResultCode != null)
                {
                    if (RetVal.ResultCode == "1")
                    {
                        booking_Response[fare] = RetVal.Fare.GrossAmount;
                        booking_Response[sequence] = RetVal.Seq;
                        booking_Response[spnr] = RetVal.SPNR;
                        booking_Response[TrackId] = RetVal.TrackId;

                        var ConfirmDetails = from Ticket in RetVal.ConfirmDetails.AsEnumerable()
                                             select new
                                             {
                                                 FirstName = Ticket.FirstName,
                                                 PolicyNo = Ticket.PolicyNo,
                                                 DOB = Ticket.DOB,
                                                 PolicyURLLink = Ticket.PolicyVoucher,
                                                 Purchasedate = Ticket.PurchaseDate,
                                                 ItineraryID = Ticket.InsurencePNR,
                                                 AirlinePNR = Ticket.AirlinePNR

                                             };
                        booking_Response[currency_ins] = ConfigurationManager.AppSettings["currency"].ToString();
                        booking_Response[SuccessResponse] = "1";
                        booking_Response[confirmdetails] = JsonConvert.SerializeObject(ConfirmDetails);
                    }
                    else
                    {
                        string empty_res = "(#" + RetVal.ResultCode + ")";
                        booking_Response[ResultCode] = RetVal.ResultCode;
                        booking_Response[FailedResponse] = RetVal.Error + empty_res;

                    }

                }

            }


            return Json(new { Status = "", Message = "", Result = booking_Response });
        }

        public ActionResult PGRedirectpage()
        {
            string PaymentId, Result = string.Empty, Auth, Reference, Amount = string.Empty, Postdate, Trackid = string.Empty, TransacID = string.Empty, Udf1, Udf2, Udf3;

            ArrayList Array_Book = new ArrayList(2);
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            Array_Book.Add("");
            int error = 0;
            int FlightResponse = 1;
            int PaxResponse = 2;
            int GrossFareRespose = 3;
            int ErrorRespose = 4;
            int Ticketingaccess = 5;
            int bookingresponse = 6;


            string ipAddress = ControllerContext.HttpContext.Request.UserHostName.ToString();
            string Gateway = string.Empty;
            string[] strFlagSession = null;
            string[] udf5 = null;
            string strflag = string.Empty;
            string strPGType = string.Empty;
            string TransId = string.Empty;
            string strBankRefNo = string.Empty;
            bool BlockTicket = false;
            DataSet dsTripUpdate = new DataSet();
            DataSet dsBookFlight = new DataSet();
            string Query = "InvokeBooking";
            string strTrackstatus = string.Empty;
            string strConfirmStatus = string.Empty;
            string strAirStatus = string.Empty;
            string Appcurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
            string AirTrackID = string.Empty;
            decimal AirlineFare = 0;
            string XMLdata = string.Empty;
            string Tocostcenter = string.Empty;
            string PaymentTrackid = string.Empty;
            string bookUserFLag = string.Empty;
            string Sector1 = string.Empty;
            string Sector2 = string.Empty;
            string Sector3 = string.Empty;
            string Sector4 = string.Empty;
            string Sector5 = string.Empty;
            string airportCategory = "I";
            string ValKey = (Session["PGReferenceID"] != null && Session["PGReferenceID"] != "") ? Session["PGReferenceID"].ToString() : "";
            RQRS.PriceItenaryRS _PriceItenaryRS = new RQRS.PriceItenaryRS();

            string strUsername = Session["username"] != null ? Session["username"].ToString() : "";
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strTerminalID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null ? Session["TERMINALTYPE"].ToString() : "";
            string strSequenceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : DateTime.Now.ToString("yyMMdd");
            try
            {
                string PaxCount = Session["PaxCount" + ValKey].ToString();
                InplantService.Inplantservice Inplant_wsdl = new InplantService.Inplantservice();
                Inplant_wsdl.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                Rays_service.RaysService RayService = new Rays_service.RaysService();
                RayService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();

                string Gatewaycharge = string.Empty;
                string serviceTAX = string.Empty;
                string strAmount = "0.00";
                ViewBag.Bookingdetails = "";
                ViewBag.Paxdetails = "";
                ViewData["SPNR"] = "";
                string strErrorMsg = string.Empty;
                string Passengername = string.Empty;
                string LoguserID = string.Empty;

                int bookingtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                DataTable dtLogdetails = new DataTable();

                LoguserID = "";
                Sector1 = Session["Sector1"] != null ? Session["Sector1"].ToString() : "";
                Sector2 = Session["Sector2"] != null ? Session["Sector2"].ToString() : "";
                Sector3 = Session["Sector3"] != null ? Session["Sector3"].ToString() : "";
                Sector4 = Session["Sector4"] != null ? Session["Sector4"].ToString() : "";
                Sector5 = Session["Sector5"] != null ? Session["Sector5"].ToString() : "";

                if (ConfigurationManager.AppSettings["Splavailagent"].ToString() != "" && ConfigurationManager.AppSettings["Splavailagent"].ToString().Contains(strTerminalID))
                {
                    strURLpath = ConfigurationManager.AppSettings["Spl_APPS_SELECT_URL"].ToString();
                }
                else
                {
                    strURLpath = ConfigurationManager.AppSettings["APPS_SELECT_URL"].ToString();
                }


                //if (ConfigurationManager.AppSettings["TMC"].ToString() == "RIYA")
                //{
                XMLdata = "<THREAD_REQUEST><URL>" + Request.Url + "</URL><DATA>" + Request.QueryString.ToString().Replace("&","SPILT") + "</DATA></THREAD_REQUEST>";
                DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGRIDIRECTURL_Fetch_PG_Payment_Details", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);
                Hashtable FetchPaymentTrack_Details = new Hashtable();

                string strFlagTemp = string.Empty;


                bool JsonSequence = Inplant_wsdl.Fetch_PG_Payment_Details(Request.QueryString["trackid"].ToString(), "", ref strFlagTemp, ref strErrorMsg);

                XMLdata = "<THREAD_RESPONSE><STEP>1</STEP><DATA>" + strFlagTemp + "</DATA></THREAD_RESPONSE>";

                DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGRIDIRECTURL_Fetch_PG_Payment_Details", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);


                if (JsonSequence == true)
                {
                    strFlagTemp = strFlagTemp;
                }
                //}
                //if (strFlagTemp != null)
                if (ConfigurationManager.AppSettings["PaymentGatewayOLDNEWURL"].ToString().ToUpper().Trim() == "OLD")
                {
                    strFlagSession = strFlagTemp.Split('@');
                }
                else
                {
                    strFlagSession = strFlagTemp.Split('|');
                }
                Result = Request.QueryString["result"];
                strPGType = strFlagSession[11].ToString();
                PaymentTrackid = strFlagSession[8].ToString();

                DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PG_INPUT_RESPONSE", strFlagTemp.ToString(), strAgentID, strTerminalID, strSequenceID);

                if (strFlagSession == null || strFlagSession.Length == 0 || string.IsNullOrEmpty(strFlagSession[0]))
                {
                }
                XMLdata = "<THREAD_RESPONSE><STEP>2</STEP><DATA>" + strFlagTemp + "</DATA></THREAD_RESPONSE>";

                DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGRIDIRECTURL_Fetch_PG_Payment_Details", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);
                if (Request["udf5"] != null && !string.IsNullOrEmpty(Request["udf5"].ToString()))
                {
                    if (ConfigurationManager.AppSettings["PaymentGatewayOLDNEWURL"].ToString().ToUpper().Trim() == "OLD")
                    {
                        udf5 = Request["udf5"].ToString().Split('/');
                        Gatewaycharge = udf5[3].ToString();
                        serviceTAX = udf5[4].ToString();
                    }
                    else
                    {
                        Gatewaycharge = strFlagSession[9];
                        serviceTAX = strFlagSession[10];
                    }
                    XMLdata = "<THREAD_RESPONSE><STEP>3</STEP></THREAD_RESPONSE>";
                    DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGRIDIRECTURL_Fetch_PG_Payment_Details", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);

                }
                if (ConfigurationManager.AppSettings["PaymentGatewayOLDNEWURL"].ToString().ToUpper().Trim() == "OLD")
                {
                    strflag = strFlagSession[14].Split('@')[0];
                }
                else
                {
                    strflag = strFlagSession[14].Split('|')[0];
                }
                XMLdata = "<THREAD_RESPONSE><STEP>4</STEP></THREAD_RESPONSE>";
                DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGRIDIRECTURL_Fetch_PG_Payment_Details", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);

                //if (strPGType.ToString().ToUpper().Trim() == "C" || strPGType.ToString().ToUpper().Trim() == "A" || strPGType.ToString().ToUpper().Trim() == "N")
                //{
                strAmount = Convert.ToDecimal(Request.QueryString["amt"]).ToString();

                //string Gatewaycharges = strFlagSession[9] != null && strFlagSession[9] != "" ? strFlagSession[9] : "0";

                Trackid = Request.QueryString["trackid"];
                PaymentId = Request.QueryString["paymentId"];
                Auth = Request.QueryString["auth"] != null ? Request.QueryString["auth"].ToString() : "";
                Reference = Request.QueryString["tranid"] != null ? Request.QueryString["tranid"].ToString() : "";
                TransId = Reference;
                strBankRefNo = Request.QueryString["Bankref"] != null ? Request.QueryString["Bankref"].ToString() : "";

                string remarks = "REDIRECT=" + ipAddress + "/PaymentResult.aspx?paymentId=" + PaymentId.ToString().Trim() +
                                                  "&result=" + Result.ToString().ToUpper().Trim() + "&amt=" + strAmount.ToString().Trim() +
                                                 "&trackid=" + Trackid.ToString().Trim() + "&Authid=" + Auth.ToString().Trim();

                XMLdata = "<REQUEST>PGDIRECTCONTROLLER Verified" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</REQUEST>" +
                         "<Trackid>" + Trackid.ToString().Trim() + "</Trackid>" +
                          "<PaymentId>" + PaymentId.ToString().Trim() + "</PaymentId>" +
                         "<strAmount>" + strAmount.ToString().Trim() + "</strAmount>";


                DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGRIDIRECTVERIFIED", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);

                XMLdata = "<THREAD_RESPONSE><STEP>5</STEP></THREAD_RESPONSE>";
                bool status = Inplant_wsdl.Check_Payment_Gateway_Track(Trackid.ToString().Trim(),
                                      "V", PaymentId.ToString().Trim(), strAmount.ToString().Trim(), ref strErrorMsg);

                XMLdata = "<Response>PGDIRECTCONTROLLER Verified" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</Response>" +
                         "<Trackid>" + Trackid.ToString().Trim() + "</Trackid>" +
                          "<PaymentId>" + PaymentId.ToString().Trim() + "</PaymentId>" +
                         "<strAmount>" + strAmount.ToString().Trim() + "</strAmount>" +
                          "<status>" + status.ToString() + "</status>";
                DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGRIDIRECTVERIFIED", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);

                if (status == true)
                {
                    if (!string.IsNullOrEmpty(Result) && (Result.ToString().Trim().ToUpper() == "TRUE" || Result.ToString().Trim().ToUpper() == "CAPTURED" || Result.ToString().Trim().ToUpper() == "APPROVED" || Result.ToString().Trim().ToUpper() == "TRANSACTION SUCCESSFUL"))
                    {
                        XMLdata = "<THREAD_RESPONSE><STEP>5</STEP></THREAD_RESPONSE>";
                        string XMLdataremarks = "<THREAD_REQUEST><URL>[<![CDATA[" + strURLpath + "]]>]</URL><DATA>" +
                               "[<![CDATA[" + remarks + "]]>]</DATA></THREAD_REQUEST>";

                        XMLdata = "<REQUEST>PGDIRECTCONTROLLER Success" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</REQUEST>" +
                         "<Trackid>" + Trackid.ToString().Trim() + "</Trackid>" +
                          "<remarks>" + XMLdataremarks + "</remarks>" +
                         "<Reference>" + Reference + "</Reference>" +
                         "<Result>" + Result.ToString().Trim().ToUpper() + "</Result>";

                        DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGRIDIRECTSUCCESS", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);


                        status = Inplant_wsdl.Update_Payment_Gateway_Track(Trackid.ToString().Trim(),
                                       "S", remarks, Trackid, ipAddress.ToString().Trim(), 0, Reference, Result.ToString().Trim().ToUpper(), ref strErrorMsg);
                        Trackid = Request.QueryString["trackid"] + "|" + Gatewaycharge + "|" + serviceTAX + "|" + Request.QueryString["paymentId"];

                        XMLdata = "<RESPONSE>PGDIRECTCONTROLLER Success" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "</RESPONSE>" +
                  "<Trackid>" + Trackid.ToString().Trim() + "</Trackid>" +
                   "<remarks>" + XMLdataremarks + "</remarks>" +
                  "<Reference>" + Reference + "</Reference>" +
                  "<Result>" + Result.ToString().Trim().ToUpper() + "</Result>" +
                   "<status>" + status + "</status>";
                        DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGRIDIRECTSUCCESS", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);

                    }
                    else
                    {

                        DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGRIDIRECTURL", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);
                        return Json(new { status = "", Errormsg = "", success = "" });
                    }
                }
                else
                {

                    DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGRIDIRECTURL", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);
                    return Json(new { status = "", Errormsg = "", success = "" });
                }
                // }

                if (!string.IsNullOrEmpty(Trackid))
                {
                    if (!string.IsNullOrEmpty(Result) && (Result.ToString().Trim().ToUpper() == "TRUE" || Result.ToString().Trim().ToUpper() == "CAPTURED" || Result.ToString().Trim().ToUpper() == "APPROVED" || Result.ToString().Trim().ToUpper() == "TRANSACTION SUCCESSFUL"))
                    {
                        #region Step-2 Redirect URL
                        XMLdata = "<EVENT><STEP>2</STEP><TRACKID>" + Trackid.ToString() + "</TRACKID></EVENT>";
                        DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGTRACKID", XMLdata.ToString(), strAgentID, strTerminalID, strSequenceID);

                        #endregion

                        #region for track fetch in pg booking new
                        //DataSet dsParam = new DataSet();
                        //byte[] myparm = Base.Utilities.ConvertDataSetToByteArray(dsParam);
                        //byte[] byteGetTrack = RayService.fetchAirlineTrackDetails_BOA(Trackid, Convert.ToBase64String(myparm), strUsername, "",
                        //    strSequenceID, strTerminalID, strAgentID);

                        //DataSet ds_res = Base.Decompress(byteGetTrack);//Base.URL_LIST.convertJsonStringToDataSet(strResponse, "");

                        ////ds_res = DbEntities.FetchBrBookedHistory(SPNR, CRSPNR, AirPnr, Clientid, "", FromDate, ToDate, POS_TID, UserName, PaymentMode, "", "", "", "", "", "", "", "", "", "");
                        //if (ds_res != null && ds_res.Tables.Count != 0 && ds_res.Tables[0].Rows.Count > 0)
                        //{

                        //    string xmlData = JsonConvert.SerializeObject(ds_res);

                        //    DataSet dsXmldet = new DataSet();
                        //    dsXmldet.ReadXml(new XmlTextReader(new StringReader(ds_res.Tables["P_FETCH_AIRLINE_TRACKID_DETAIlS_BOA"].Rows[0]["PAX_DETAILS"].ToString())));
                        //    string strBookingDet = dsXmldet.Tables["HEADER"].Rows[0]["INPUT_REQUEST"].ToString();//JsonConvert.SerializeObject(dsXmldet);
                        //    RQRS.BookingRquest _BookingReq = JsonConvert.DeserializeObject<RQRS.BookingRquest>(strBookingDet);
                        //    Session.Add("username", _BookingReq.Agent.UserName);
                        //    Session.Add("POS_ID", _BookingReq.Agent.AgentId);
                        //    Session.Add("POS_TID", _BookingReq.Agent.TerminalId);
                        //    Session.Add("TERMINALTYPE", _BookingReq.Agent.Environment);
                        //    Session.Add("sequenceid", _BookingReq.Agent.Sequence);
                        //    strUsername = _BookingReq.Agent.UserName;
                        //    strAgentID = _BookingReq.Agent.AgentId;
                        //    strTerminalID = _BookingReq.Agent.TerminalId;
                        //    strTerminalType = _BookingReq.Agent.Environment;
                        //    strSequenceID = _BookingReq.Agent.Sequence;
                        //    ViewBag.PaxMobileNo = _BookingReq.PaxDetails[0].Mobnumber;
                        //    ViewBag.PaxEmail = _BookingReq.PaxDetails[0].MailID;

                        //    string PGagentID = (Session["PGagentID"] != null && Session["PGagentID"] != "") ? Session["PGagentID"].ToString() : "";
                        //    string PGterminalID = (Session["PGterminalID"] != null && Session["PGterminalID"] != "") ? Session["PGterminalID"].ToString() : "";
                        //    string PGPosID = (Session["PGPosID"] != null && Session["PGPosID"] != "") ? Session["PGPosID"].ToString() : "";
                        //    string PGUserName = (Session["PGUserName"] != null && Session["PGUserName"] != "") ? Session["PGagentID"].ToString() : "";
                        //    string PGBranchId = (Session["PGBranchId"] != null && Session["PGBranchId"] != "") ? Session["PGBranchId"].ToString() : "";
                        //    string ReferenceID = (Session["PGReferenceID"] != null && Session["PGReferenceID"] != "") ? Session["PGReferenceID"].ToString() : "";

                        //    string CompanyID = (Session["PGCompanyID"] != null && Session["PGCompanyID"] != "") ? Session["PGCompanyID"].ToString() : "";
                        //    string terminalType = "W";
                        //    string Errormsg = string.Empty;
                        //    string BookFlightdetails = string.Empty;
                        //    string AllPaxdetails = string.Empty;
                        //    string TotalFaredt = string.Empty;
                        //    string TripType = (Session["PGTripType"] != null && Session["PGTripType"] != "") ? Session["PGTripType"].ToString() : "";

                        //    bool insStatus = false;

                        //    //if (Session["BookingREquestdetails" + PaymentTrackid.Split('~')[0]] != null)
                        //    //{
                        //    //    RQRS.BookingRquest _BookingReq = (RQRS.BookingRquest)Session["BookingREquestdetails" + PaymentTrackid.Split('~')[0]];
                        //    _BookingReq.PaymentrefID = Trackid.ToString().Split('|')[0];
                        //    _BookingReq.Track_Create = false;

                        //    if (_BookingReq.ItineraryFlights.Count > 0)
                        //    {
                        //        for (var N = 0; _BookingReq.ItineraryFlights.Count > N; N++)
                        //        {
                        //            _BookingReq.ItineraryFlights[N].TrackId = PaymentTrackid.Split('~')[N];
                        //            Tocostcenter = _BookingReq.CBT_Input.CostCenter;
                        //        }
                        //    }

                        //    string strjson = JsonConvert.SerializeObject(_BookingReq);
                        //    string BookingDetails_LOG = "";
                        //    string strResponse = string.Empty;
                        //    BookingDetails_LOG = strjson;

                        //    #region Log for Booking Request
                        //    string LstrDetails = string.Empty;
                        //    string ReqTime = "";
                        //    if (Base.AvailLog)
                        //    {
                        //        ReqTime = "PGBOOKINGREQEST" + DateTime.Now;
                        //        LstrDetails = "<URL>[<![CDATA[" + strURLpath + "]]>]</URL><DATA>" +
                        //             "[<![CDATA[" + Request.QueryString.ToString() + "]]>]</DATA><JSON>" + strjson + "</JSON><TIME>" + ReqTime + "</TIME>";

                        //        DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PG~BRQ~" + Trackid + "", LstrDetails.ToString(), strAgentID, strTerminalID, strSequenceID);

                        //    }
                        //    #endregion


                        //    MyWebClient Client = new MyWebClient();
                        //    Client.LintTimeout = bookingtimeout;
                        //    Client.Headers["Content-type"] = "application/json";


                        //    byte[] byteGetLogin = Client.UploadData(strURLpath + "/" + "InvokeBooking", "POST", Encoding.ASCII.GetBytes(strjson));
                        //    strResponse = Encoding.ASCII.GetString(byteGetLogin);

                        //    #region Log for BOOKING Response
                        //    string xmldata = string.Empty;
                        //    xmldata = "<EVENT><RESPONSE>Flight BOOKING </RESPONSE>";
                        //    xmldata += "<BOOKING_RESPONSE>" + strResponse.ToString() + "</BOOKING_RESPONSE>";
                        //    xmldata += "<BOOKING_REQUEST>" + BookingDetails_LOG.ToString() + "</BOOKING_REQUEST>";
                        //    xmldata += "</EVENT>";
                        //    DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PG~BRS~", xmldata.ToString(), strAgentID, strTerminalID, strSequenceID);

                        //    #endregion

                        //    #region Booking Response

                        //    RQRS.BookingRS _BookingRes = JsonConvert.DeserializeObject<RQRS.BookingRS>(strResponse);
                        //    AirTrackID = _BookingRes.TrackId;
                        //    ViewBag.pgPgError = _BookingRes.Error != null && _BookingRes.Error != "" ? _BookingRes.Error : "Payment received successfully.Problem occured while booking.(#01)";
                        //    string result = _BookingRes.ResultCode;
                        //    if (_BookingRes.ResultCode == "1")
                        //    {
                        //        string stu = "1";
                        //        strTrackstatus = "S";
                        //        strConfirmStatus = "S";
                        //        strAirStatus = "S";
                        //        xmldata = "";
                        //        #region Log for BOOKING Response
                        //        xmldata = "<EVENT><RESPONSE>Flight BOOKING </RESPONSE>";
                        //        xmldata += "<DATA>" + strResponse + "</DATA>";
                        //        xmldata += "<FETCH_FLIGHT_BOOKING_RESULT>" + strResponse.ToString() + "</FETCH_FLIGHT_BOOKING_RESULT>";
                        //        xmldata += "<BOOKINGDETAILS>" + BookingDetails_LOG.ToString() + "</BOOKINGDETAILS>";
                        //        DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PG~BRS~", xmldata.ToString(), strAgentID, strTerminalID, strSequenceID);
                        //        #endregion

                        //    }
                        //    else if (_BookingRes.ResultCode == "3")
                        //    {
                        //        #region  Clear All Cache element
                        //        foreach (var element in MemoryCache.Default)
                        //        {
                        //            MemoryCache.Default.Remove(element.Key);
                        //        }
                        //        #endregion

                        //        string stu = "3";
                        //        strTrackstatus = "P";
                        //        strConfirmStatus = "P";
                        //        strAirStatus = "P";
                        //        string err = _BookingRes.Error;
                        //        xmldata = string.Empty;
                        //        xmldata += "<EVENT><DATA>" + err + "</DATA></EVENT>";

                        //        DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGBOOKINGRESPONSE", xmldata.ToString(), strAgentID, strTerminalID, strSequenceID);

                        //        // return Json(new { status = stu, Errormsg = err });
                        //        dsTripUpdate = Inplant_wsdl.UpdateTrackDetails(AirTrackID, "", "", "", "", Session["SessstrTripID"] != null ? Session["SessstrTripID"].ToString() : "", CompanyID, LoguserID, Session["strTripTrackID"] != null ? Session["strTripTrackID"].ToString() : "", strTrackstatus, "P", AirlineFare, Passengername, "", "AIR", "S", "", "", "", "");
                        //    }
                        //    else
                        //    {
                        //        string stu = "0";
                        //        strTrackstatus = "P";
                        //        strConfirmStatus = "P";
                        //        strAirStatus = "P";
                        //        string err = "";
                        //        if (_BookingRes.Error == "" && _BookingRes.Error == null)
                        //        {
                        //            err = "Problem occured while booking. please contact support team (#01).";
                        //        }
                        //        else
                        //        {
                        //            err = _BookingRes.Error + "(#01).";
                        //        }

                        //        xmldata = "";
                        //        #region Log for BOOKING Response
                        //        xmldata = "<EVENT><RESPONSE>FLIGHT BOOKING </RESPONSE>";
                        //        xmldata += "<DATA>" + strResponse + "</DATA>";
                        //        xmldata += "<FETCH_FLIGHT_BOOKING_RESULT>" + strResponse.ToString() + "</FETCH_FLIGHT_BOOKING_RESULT>";
                        //        xmldata += "<BOOKINGTAILS>" + BookingDetails_LOG.ToString() + "</BOOKINGDETAILS>";

                        //        if (_BookingRes.Error.ToString() != "")
                        //        {
                        //            xmldata += "<DATA>" + _BookingRes.Error + "</DATA>";
                        //        }
                        //        else
                        //        {
                        //            xmldata += "<DATA>Failed</DATA>";
                        //        }
                        //        xmldata += "</EVENT>";
                        //        #endregion
                        //        DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGBOOKINGRESPONSE", xmldata.ToString(), strAgentID, strTerminalID, strSequenceID);

                        //        dsTripUpdate = Inplant_wsdl.UpdateTrackDetails(AirTrackID, "", "", "", "", Session["SessstrTripID"] != null ? Session["SessstrTripID"].ToString() : "", CompanyID, LoguserID, Session["strTripTrackID"] != null ? Session["strTripTrackID"].ToString() : "", strTrackstatus, "P", AirlineFare, Passengername, "", "AIR", "S", "", "", "", "");

                        //    }

                        //    #region ERPPUSH
                        //    if ((_BookingRes.ResultCode == "1" || _BookingRes.ResultCode == "5") && ConfigurationManager.AppSettings["STS_ERP_PUSH"].ToString().ToUpper() == "Y")
                        //    {
                        //        try
                        //        {
                        //            string ERPPUSHDETAILS = string.Empty;

                        //            var ERPPushDet = (from Ticket_F in _BookingRes.PnrDetails
                        //                              group Ticket_F by new { Ticket_F.SPNR } into GetTotalFare
                        //                              select new
                        //                              {
                        //                                  SPNR = GetTotalFare.ToArray().Distinct().FirstOrDefault().SPNR,
                        //                                  USERNAME = GetTotalFare.ToArray().Distinct().FirstOrDefault().FIRSTNAME + " " + GetTotalFare.ToArray().Distinct().FirstOrDefault().LASTNAME,
                        //                              }).ToList();
                        //            if (ERPPushDet != null && ERPPushDet.Count > 0)
                        //            {
                        //                for (int i = 0; i < ERPPushDet.Count; i++)
                        //                {
                        //                    ERPPUSHDETAILS += ERPPushDet[i].SPNR + "~";
                        //                }
                        //                RayService.ERPPUSHFORBOOKING(CompanyID, ERPPUSHDETAILS, "", "AIR", "S", "", ERPPushDet[0].USERNAME, ControllerContext.HttpContext.Request.UserHostName.ToString(), "");
                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            DatabaseLog.LogData(strUsername, "X", "FromBookingRequest", "BOOKING ERP-PUSH FUNCTION", ex.ToString(), strAgentID, Session["POS_TID"].ToString(), strSequenceID);
                        //        }

                        //    }
                        //    #endregion

                        //    if (string.IsNullOrEmpty(strResponse))
                        //    {
                        //        string date = Base.LoadServerdatetime();

                        //        string lstrBkingdetail = (BlockTicket ? "<BLOKING_RESPONSE>" : "<BOOKING_RESPONSE>") +
                        //                   ("<STATUS>FAILED</STATUS>") +
                        //                  ("<RESPONSE>Null</RESPONSE>") +
                        //                   "<AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE_TIME>" + date +
                        //               "</RESPONSE_TIME>" + (BlockTicket ? "</BLOKING_RESPONSE>" : "</BOOKING_RESPONSE>");

                        //        string refstring = string.Empty;
                        //        RayService.Apps_Booking_Response_Receive_Status("", strAgentID, Session["POS_TID"].ToString(),
                        //          strUsername, Session["TerminalType"].ToString(), Session["ipAddress"].ToString(), strSequenceID,
                        //         ref refstring, lstrBkingdetail, "FormBookingRequest");


                        //        Session.Add("BookStart" + ValKey, "false");
                        //        Array_Book[error] = "Problem occured while booking. Please Contact Customercare.";
                        //        //return Array_Book;
                        //        //return Json(new { Status = "", Message = "", Result = Array_Book });
                        //        return PartialView("_BookingSuccess", "");
                        //    }
                        //    else
                        //    {
                        //        Array_Book[bookingresponse] = strResponse;
                        //        ViewBag.bookingresponse = strResponse;
                        //        var RetVal = JsonConvert.DeserializeObject<RQRS.BookingRS>(strResponse);

                        //        string RetVale = JsonConvert.SerializeObject(RetVal);
                        //        dsBookFlight = Serv.convertJsonStringToDataSet(RetVale, "");

                        //        if (RetVal != null)
                        //        {
                        //            string lstrBookingdetail = "";
                        //            try
                        //            {
                        //                lstrBookingdetail = (BlockTicket ? "<BLOKING_RESPONSE>" : "<BOOKING_RESPONSE>") +
                        //                  ((RetVal != null && (RetVal.ResultCode == "1")) ?
                        //                  ("<STATUS>SUCCESS</STATUS>") :
                        //                  ((RetVal != null && (RetVal.ResultCode == "2")) ?
                        //                  ("<STATUS>FARECHANGE</STATUS>") : ("<STATUS>FAILED</STATUS>"))) +
                        //                  ((RetVal != null && RetVal.ResultCode != null && RetVal.ResultCode.ToString().Trim() != "") ?
                        //                  ("<ResultCode>" + RetVal.ResultCode.ToString().Trim() + "</ResultCode>") : ("<ResultCode>Empty/Null</ResultCode>")) +
                        //                  ((RetVal != null && RetVal.Sqe != null && RetVal.Sqe.ToString().Trim() != "") ?
                        //                  ("<Sqe>" + RetVal.Sqe.ToString().Trim() + "</Sqe>") : ("<Sqe>Empty/Null</Sqe>")) +
                        //                  //((RetVal != null && RetVal.Stock != null && RetVal.Stock.ToString().Trim() != "") ?
                        //                  //("<STOCK>" + RetVal.Stock.ToString().Trim() + "</STOCK>") : ("<STOCK>Empty/Null</STOCK>")) +
                        //                  ((RetVal != null && RetVal.Error != null && RetVal.Error.ToString().Trim() != "") ?
                        //                  ("<Error>" + RetVal.Error.ToString().Trim() + "</Error>") : ("<Error>Empty/Null</Error>")) +
                        //                  ((RetVal != null && RetVal.TrackId != null && RetVal.TrackId.ToString().Trim() != "") ?
                        //                  ("<TrackId>" + RetVal.TrackId.ToString().Trim() + "</TrackId>") : ("<TrackId>Empty/Null</TrackId>")) +
                        //                  (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].SPNR != null ?
                        //                  ("<SPNR>" + RetVal.PnrDetails[0].SPNR.ToString().Trim() + "</SPNR>") : "") +
                        //                  (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].AIRLINEPNR != null ?
                        //                  ("<AIRLINEPNR>" + RetVal.PnrDetails[0].AIRLINEPNR.ToString().Trim() + "</AIRLINEPNR>") : "") +
                        //                  (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].CRSPNR != null ?
                        //                  ("<CRSPNR>" + RetVal.PnrDetails[0].CRSPNR.ToString().Trim() + "</CRSPNR>") : "") +
                        //                  (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].GROSSFARE != null ?
                        //                  ("<GROSSFARE>" + RetVal.PnrDetails[0].GROSSFARE.ToString().Trim() + "</GROSSFARE>") : "") +
                        //                  (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].TICKETINGCARRIER != null ?
                        //                  ("<TICKETINGCARRIER>" + RetVal.PnrDetails[0].TICKETINGCARRIER.ToString().Trim() + "</TICKETINGCARRIER>") : "") +
                        //                  (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].TRIPTYPE != null ?
                        //                  ("<TRIPTYPE>" + RetVal.PnrDetails[0].TRIPTYPE.ToString().Trim() + "</TRIPTYPE>") : "") +
                        //                  (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].OFFICEID != null ?
                        //                  ("<OFFICEID>" + RetVal.PnrDetails[0].OFFICEID.ToString().Trim() + "</OFFICEID>") : "") +
                        //                  (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].OFFLINEFLAG != null ?
                        //                  ("<OFFLINEFLAG>" + RetVal.PnrDetails[0].OFFLINEFLAG.ToString().Trim() + "</OFFLINEFLAG>") : "") +
                        //                  (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 ?
                        //                  ("<PnrDetailsCount>" + RetVal.PnrDetails.Count().ToString().Trim() + "</PnrDetailsCount>") : "") +
                        //                  "<AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><URL>[<![CDATA[" + Session["travraysws_url"] + "/" +
                        //                 Session["travraysws_vdir"] + "/Rays.svc/" + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (bookingtimeout).ToString() +
                        //              "</TIMEOUT><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE_TIME>" +
                        //             ReqTime +
                        //              "</RESPONSE_TIME>" + (BlockTicket ? "</BLOKING_RESPONSE>" : "</BOOKING_RESPONSE>");
                        //            }
                        //            catch
                        //            {

                        //            }
                        //            //   string result = _BookingRes.ResultCode; //dsBookFlight.Tables["rootNode"].Rows[0]["BRC"].ToString();


                        //            #region result code 1-sri
                        //            if (result == "1" || result == "5")
                        //            {
                        //                bool Insres = false;
                        //                if (ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA" && Session["BajajInsDetails"] != null && Session["BajajInsDetails"].ToString() != "" && RetVal.Error.Contains("SUPRKMHx1007086:") == false)
                        //                {
                        //                    string Insdetails = Session["BajajInsDetails"].ToString();
                        //                    string ClientID = Session["PGagentID"].ToString();
                        //                    string PaxDet = Session["BajajPaxdet"].ToString();
                        //                    string ContactDet = Session["BajajContactdet"].ToString();
                        //                    string gstdeta = Session["BajajGstdetails"].ToString();
                        //                    Insres = false;

                        //                    if (ConfigurationManager.AppSettings["Etravelversion"].ToString().ToUpper() != "NEW")
                        //                    {
                        //                        ViewBag.Insresponse = BookbajajInsuranceOld(RetVale, Insdetails, PaxDet, ContactDet, ClientID, "P");
                        //                    }
                        //                    else
                        //                    {
                        //                        ViewBag.Insresponse = BookbajajInsurance(RetVale, Insdetails, _BookingReq.PaxDetails, ContactDet, ClientID, "P", PGBranchId, _BookingReq.PaymentrefID, gstdeta);//4650
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    ViewBag.Insresponse = "";
                        //                }
                        //                #region Offlinevisarequest
                        //                if (ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA" && ConfigurationManager.AppSettings["Offlinevisadestinationcity"] != "" && RetVal.Error.Contains("SUPRKMHx1007086:") == false && BlockTicket == false)
                        //                {
                        //                    string QrystringVisa = string.Empty;
                        //                    if (ConfigurationManager.AppSettings["Offlinevisadestinationcity"].Contains(Session["BaseDest" + ValKey].ToString()) == true && Session["PaxCount" + ValKey] != null)
                        //                    {
                        //                        string PaxCount = Session["PaxCount" + ValKey].ToString();
                        //                        int adultCount = 0;
                        //                        int childCount = 0;
                        //                        int infantCount = 0;
                        //                        ViewBag.Visaoffline = "YES";
                        //                        string[] strPaxCounts = PaxCount.Split('|');
                        //                        if (strPaxCounts.Length > 0)
                        //                        {
                        //                            adultCount = Convert.ToInt16(strPaxCounts[0]);
                        //                            childCount = Convert.ToInt16(strPaxCounts[1]);
                        //                            infantCount = Convert.ToInt16(strPaxCounts[2]);
                        //                        }
                        //                        int paxcnt = adultCount + childCount + infantCount;
                        //                        string today = DateTime.Now.ToString("dd/MM/yyyy");
                        //                        QrystringVisa = "tid=" + (Session["terminalid"].ToString()) + "&agentid=" + (Session["agentid"].ToString())
                        //                                      + "&IPA=" + (Session["ipAddress"].ToString()) + "&username=" + (strUsername)
                        //                                      + "&seq=" + (strSequenceID)
                        //                                      + "&pax=" + (paxcnt) + "&PNR=" + (RetVal.PnrDetails[0].SPNR.ToString());
                        //                        string QueryString = "SECKEY=" + Base.EncryptKEy(QrystringVisa, "VISAQRY" + today);
                        //                        ViewBag.VisaQuerystringURL = QueryString;
                        //                    }
                        //                    else
                        //                    {
                        //                        ViewBag.Visaoffline = "NO";
                        //                        ViewBag.VisaQuerystringURL = "";
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    ViewBag.Visaoffline = "NO";
                        //                    ViewBag.VisaQuerystringURL = "";
                        //                }
                        //                #endregion

                        //                if (RetVal.Error.Contains("SUPRKMHx1007086:"))
                        //                {
                        //                    Array_Book[error] = result;

                        //                    ViewBag.ResultCode = result;
                        //                    Array_Book[ErrorRespose] = Regex.Split(RetVal.Error, "SUPRKMHx1007086:")[1];
                        //                    ViewBag.Booktext = Regex.Split(RetVal.Error, "SUPRKMHx1007086:")[1];

                        //                }
                        //                else
                        //                {
                        //                    Array_Book[error] = result;
                        //                    ViewBag.ResultCode = result;
                        //                    Array_Book[ErrorRespose] = "";
                        //                    ViewBag.Booktext = "";
                        //                }

                        //                Session.Add("dsBookFlight" + ValKey, dsBookFlight);
                        //                var Tkt = from Ticket in RetVal.PnrDetails.AsEnumerable()
                        //                          group Ticket by new { Ticket.AIRLINEPNR, Ticket.ORIGIN, Ticket.DESTINATION }
                        //                              into grpTicketlst
                        //                          select new RQRS.PnrDetails
                        //                          {
                        //                              SPNR = grpTicketlst.FirstOrDefault().SPNR,
                        //                              CRSPNR = grpTicketlst.FirstOrDefault().CRSPNR,
                        //                              //SupPNR = Ticket.SupplierPNR != null ? Ticket.SupplierPNR : "N/A",
                        //                              AIRLINEPNR = grpTicketlst.FirstOrDefault().AIRLINEPNR,
                        //                              FLIGHTNO = grpTicketlst.FirstOrDefault().FLIGHTNO,
                        //                              AIRLINECODE = grpTicketlst.FirstOrDefault().AIRLINECODE,
                        //                              CLASS = grpTicketlst.FirstOrDefault().CLASS,
                        //                              ORIGIN = grpTicketlst.FirstOrDefault().ORIGIN,//Utilities.AirportcityName(Ticket.ORIGIN),
                        //                              DESTINATION = grpTicketlst.FirstOrDefault().DESTINATION,//Utilities.AirportcityName(Ticket.DESTINATION),
                        //                              DEPARTUREDATE = grpTicketlst.FirstOrDefault().DEPARTUREDATE,
                        //                              ARRIVALDATE = grpTicketlst.FirstOrDefault().ARRIVALDATE
                        //                          };


                        //                Array_Book[FlightResponse] = JsonConvert.SerializeObject(Tkt);
                        //                ViewBag.FlightResponse = JsonConvert.SerializeObject(Tkt.ToList());

                        //                string TotalFare = "";
                        //                if (_BookingReq.TripType.ToUpper() == "R" || Session["mulreq"] == "Y") //mulreq="Y" - Domestic Multicity...
                        //                {

                        //                    int index = 0;
                        //                    string lstrpax = "";
                        //                    var Paxdet = from FPass in RetVal.PnrDetails.AsEnumerable()
                        //                                     // where ((index >= (RetVal.PnrDetails.Count / 2) ? false : true) &&
                        //                                     // (lstrpax.Contains(FPass.SEQNO) ? false : true))

                        //                                 group FPass by FPass.SEQNO
                        //                                     into grpfilterlst
                        //                                 select new RQRS.PnrDetails
                        //                                 {

                        //                                     TITLE = grpfilterlst.FirstOrDefault().TITLE,
                        //                                     FIRSTNAME = grpfilterlst.FirstOrDefault().FIRSTNAME,
                        //                                     LASTNAME = grpfilterlst.FirstOrDefault().LASTNAME,
                        //                                     PAXTYPE = grpfilterlst.FirstOrDefault().PAXTYPE,
                        //                                     DATEOFBIRTH = grpfilterlst.FirstOrDefault().DATEOFBIRTH,
                        //                                     TICKETNO = grpfilterlst.Select(a => a.TICKETNO).Distinct().Aggregate((a, b) => a + " / " + b),
                        //                                     USERTRACKID = grpfilterlst.Select(a => a.USERTRACKID).Distinct().Aggregate((a, b) => a + " / " + b),
                        //                                     GROSSFARE = Convert.ToString((Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().GROSSFARE != null && grpfilterlst.FirstOrDefault().GROSSFARE != "") ? grpfilterlst.FirstOrDefault().GROSSFARE : "0")
                        //                                        + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SERVICECHARGE != null && grpfilterlst.FirstOrDefault().SERVICECHARGE != "") ? grpfilterlst.FirstOrDefault().SERVICECHARGE : "0")
                        //                                        + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().MARKUP != null && grpfilterlst.FirstOrDefault().MARKUP != "") ? grpfilterlst.FirstOrDefault().MARKUP : "0")
                        //                                        + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().CLIENTMARKUP != null && grpfilterlst.FirstOrDefault().CLIENTMARKUP != "") ? grpfilterlst.FirstOrDefault().CLIENTMARKUP : "0")
                        //                                        + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SEATSAMOUNT != null && grpfilterlst.FirstOrDefault().SEATSAMOUNT != "") ? grpfilterlst.FirstOrDefault().SEATSAMOUNT : "0")
                        //                                        + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().ADDMARKUP != null && grpfilterlst.FirstOrDefault().ADDMARKUP != "") ? grpfilterlst.FirstOrDefault().ADDMARKUP : "0")
                        //                                        + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().BAGGAGE != null && grpfilterlst.FirstOrDefault().BAGGAGE != "") ? grpfilterlst.FirstOrDefault().BAGGAGE : "0")
                        //                                        + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().MEALSAMOUNT != null && grpfilterlst.FirstOrDefault().MEALSAMOUNT != "") ? grpfilterlst.FirstOrDefault().MEALSAMOUNT : "0"))
                        //                                        + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SFAMOUNT != null && grpfilterlst.FirstOrDefault().SFAMOUNT != "") ? grpfilterlst.FirstOrDefault().SFAMOUNT : "0")
                        //                                        + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SFGST != null && grpfilterlst.FirstOrDefault().SFGST != "") ? grpfilterlst.FirstOrDefault().SFGST : "0")
                        //                                        + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().OTHER_SSR_AMOUNT != null && grpfilterlst.FirstOrDefault().OTHER_SSR_AMOUNT != "") ? grpfilterlst.FirstOrDefault().OTHER_SSR_AMOUNT : "0")
                        //                                      ),
                        //                                     SERVICECHARGE = ((grpfilterlst.FirstOrDefault().SERVICECHARGE != null && grpfilterlst.FirstOrDefault().SERVICECHARGE != "") ? grpfilterlst.FirstOrDefault().SERVICECHARGE : "0"),
                        //                                     MARKUP = grpfilterlst.FirstOrDefault().CLIENTMARKUP,
                        //                                     test = index++,
                        //                                     pax = faresplit(grpfilterlst.FirstOrDefault().SEQNO, ref lstrpax),
                        //                                     SPNR = grpfilterlst.Select(a => a.SPNR).Distinct().Aggregate((a, b) => a + " / " + b)
                        //                                 };

                        //                    Array_Book[PaxResponse] = JsonConvert.SerializeObject(Paxdet);
                        //                    ViewBag.PaxResponse = JsonConvert.SerializeObject(Paxdet.ToList());
                        //                    lstrpax = "";
                        //                    index = 0;
                        //                    var PaxFare = from FPass in RetVal.PnrDetails.AsEnumerable()
                        //                                      //    where ((index >= (RetVal.PnrDetails.Count / 2) ? false : true) 
                        //                                      //    &&
                        //                                      //(lstrpax.Contains(FPass.SEQNO) ? false : true))
                        //                                  select new
                        //                                  {
                        //                                      GROSSFARE = ((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                        //                                                    + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                        //                                                    + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                        //                                                    + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                        //                                                    + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                        //                                                    + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                        //                                                    + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                        //                                                    + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0"))
                        //                                                    + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                        //                                                    + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                        //                                                    + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0")
                        //                                                    ),
                        //                                      pax = faresplit(FPass.SEQNO, ref lstrpax),
                        //                                      pnrs = FPass.SPNR,
                        //                                      index = index++
                        //                                  };

                        //                    //TotalFare = PaxFare.GroupBy(ax => ax.pnrs).Select(Grss => Convert.ToDouble(Grss.FirstOrDefault().GROSSFARE)).ToArray().Sum().ToString(); //Commented by saranraj on 20170705 to solve multipax sum...
                        //                    TotalFare = PaxFare.GroupBy(ax => new { ax.pnrs, ax.pax }).Select(Grss => Convert.ToDouble(Grss.FirstOrDefault().GROSSFARE)).Sum().ToString(); //Added by saranraj...


                        //                    //Array_Book[FlightResponse] = JsonConvert.SerializeObject(Tkt);
                        //                    Array_Book[GrossFareRespose] = JsonConvert.SerializeObject(TotalFare);

                        //                    ViewBag.GrossFareRespose = JsonConvert.SerializeObject(TotalFare);

                        //                }
                        //                else
                        //                {
                        //                    string lstrpax = "";
                        //                    var Paxdet = (from FPass in RetVal.PnrDetails.AsEnumerable()
                        //                                  where ((lstrpax.Contains(FPass.SEQNO) ? false : true))
                        //                                  select new RQRS.PnrDetails
                        //                                  {
                        //                                      TITLE = FPass.TITLE,
                        //                                      FIRSTNAME = FPass.FIRSTNAME,
                        //                                      LASTNAME = FPass.LASTNAME,
                        //                                      PAXTYPE = FPass.PAXTYPE,
                        //                                      DATEOFBIRTH = FPass.DATEOFBIRTH,
                        //                                      TICKETNO = FPass.TICKETNO,
                        //                                      USERTRACKID = FPass.USERTRACKID,
                        //                                      GROSSFARE = Convert.ToString(((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0"))
                        //                                         + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                        //                                         + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                        //                                         + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                        //                                         + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                        //                                         + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                        //                                         + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                        //                                         + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                        //                                         + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                        //                                         + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                        //                                         + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0")
                        //                                        )),
                        //                                      SERVICECHARGE = ((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"),
                        //                                      MARKUP = FPass.CLIENTMARKUP,
                        //                                      pax = faresplit(FPass.SEQNO, ref lstrpax),
                        //                                      SPNR = FPass.SPNR

                        //                                  }).ToList();

                        //                    Array_Book[PaxResponse] = JsonConvert.SerializeObject(Paxdet);
                        //                    ViewBag.PaxResponse = JsonConvert.SerializeObject(Paxdet.ToList());

                        //                    lstrpax = "";
                        //                    var PaxFare = from FPass in RetVal.PnrDetails.AsEnumerable()
                        //                                  where ((lstrpax.Contains(FPass.SEQNO) ? false : true))
                        //                                  select new
                        //                                  {
                        //                                      GROSSFARE = ((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                        //                                          + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                        //                                          + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                        //                                          + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                        //                                          + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                        //                                          + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                        //                                          + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                        //                                          + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                        //                                          + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                        //                                          + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                        //                                          + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0"))
                        //                                        ),
                        //                                      pax = faresplit(FPass.SEQNO, ref lstrpax),
                        //                                      pnrs = FPass.SPNR,
                        //                                  };

                        //                    TotalFare = PaxFare.Select(Grss => Convert.ToDouble(Grss.GROSSFARE)).ToArray().Sum().ToString();


                        //                    Array_Book[GrossFareRespose] = JsonConvert.SerializeObject(TotalFare);
                        //                    ViewBag.GrossFareRespose = JsonConvert.SerializeObject(TotalFare);
                        //                }
                        //            }

                        //            #endregion result code 1
                        //            #region result code 2 -sri

                        //            else if (result == "2")
                        //            {
                        //                string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error : "Unable to process your request. please contact customer care.";
                        //                Array_Book[error] = result;
                        //                ViewBag.ResultCode = RetVal.ResultCode;
                        //                Array_Book[GrossFareRespose] = msg;
                        //                ViewBag.GrossFareRespose = msg + "-(#01)";
                        //                DatabaseLog.LogData(strUsername, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - " + result + " BOOKING_RESPONSE_MESSAGE_" + msg + "", strAgentID, strTerminalID, strSequenceID);

                        //            }
                        //            #endregion result code 2 -sri
                        //            #region result code 3-sri

                        //            else if (result == "3")
                        //            {
                        //                string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error : "Unable to process your request. please contact customer care";
                        //                Array_Book[error] = result;
                        //                ViewBag.ResultCode = result;
                        //                Array_Book[GrossFareRespose] = msg;
                        //                ViewBag.GrossFareRespose = msg + "-(#01)";
                        //                DatabaseLog.LogData(strUsername, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - " + result + " BOOKING_RESPONSE_MESSAGE_" + msg + "", strAgentID, strTerminalID, strSequenceID);

                        //            }

                        //            #endregion result code 3-sri
                        //            #region result code 5----round trip double grid --onwared success and retrun failed
                        //            else if (result == "5")
                        //            {
                        //                string msg = "( " + RetVal.PnrDetails[0].ORIGIN.ToString() + " - " + RetVal.PnrDetails[0].DESTINATION.ToString() + " ) - SUCCESS <br />( " + RetVal.PnrDetails[0].DESTINATION.ToString() + " - " + RetVal.PnrDetails[0].ORIGIN.ToString() + " ) - FAILED";
                        //                Array_Book[error] = result;
                        //                ViewBag.ResultCode = result;
                        //                Array_Book[GrossFareRespose] = msg;
                        //                ViewBag.GrossFareRespose = msg + "-(#01)";

                        //            }
                        //            #endregion
                        //            else
                        //            {
                        //                string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error : "Problem occured while booking. please contact support team.";
                        //                Array_Book[error] = RetVal.ResultCode;
                        //                ViewBag.ResultCode = RetVal.ResultCode;
                        //                Array_Book[GrossFareRespose] = msg;
                        //                ViewBag.GrossFareRespose = msg + "-(#01)";
                        //                DatabaseLog.LogData(strUsername, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - " + result + " BOOKING_RESPONSE_MESSAGE_" + msg + "", strAgentID, strTerminalID, strSequenceID);
                        //            }

                        //        }
                        //        else
                        //        {
                        //            string date = Base.LoadServerdatetime();
                        //            string lstrBkingdetail = (BlockTicket ? "<BLOKING_RESPONSE>" : "<BOOKING_RESPONSE>") +
                        //                       ("<STATUS>FAILED</STATUS>") +
                        //                      ("<RESPONSE>Null</RESPONSE>") +
                        //                       "<AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE_TIME>" + date +
                        //                   "</RESPONSE_TIME>" + (BlockTicket ? "</BLOKING_RESPONSE>" : "</BOOKING_RESPONSE>");
                        //        }

                        //        Session.Add("BookStart" + ValKey, "false");
                        //    }

                        //    #endregion
                        //}
                        //else
                        //{
                        //    string msg = "Unable to process your request. please contact customer care";
                        //    Array_Book[error] = "03";
                        //    ViewBag.ResultCode = "03";
                        //    Array_Book[GrossFareRespose] = msg;
                        //    ViewBag.GrossFareRespose = msg + "-(#01)";
                        //    DatabaseLog.LogData(strUsername, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - 03" + " BOOKING_RESPONSE_MESSAGE_" + msg + "", strAgentID, strTerminalID, strSequenceID);
                        //}
                        #endregion

                        #region old
                        string PGagentID = (Session["PGagentID"] != null && Session["PGagentID"] != "") ? Session["PGagentID"].ToString() : "";
                        string PGterminalID = (Session["PGterminalID"] != null && Session["PGterminalID"] != "") ? Session["PGterminalID"].ToString() : "";
                        string PGPosID = (Session["PGPosID"] != null && Session["PGPosID"] != "") ? Session["PGPosID"].ToString() : "";
                        string PGUserName = (Session["PGUserName"] != null && Session["PGUserName"] != "") ? Session["PGagentID"].ToString() : "";
                        string PGBranchId = (Session["PGBranchId"] != null && Session["PGBranchId"] != "") ? Session["PGBranchId"].ToString() : "";
                        string ReferenceID = (Session["PGReferenceID"] != null && Session["PGReferenceID"] != "") ? Session["PGReferenceID"].ToString() : "";

                        string CompanyID = (Session["PGCompanyID"] != null && Session["PGCompanyID"] != "") ? Session["PGCompanyID"].ToString() : "";
                        string terminalType = "W";
                        string Errormsg = string.Empty;
                        string BookFlightdetails = string.Empty;
                        string AllPaxdetails = string.Empty;
                        string TotalFaredt = string.Empty;
                        string TripType = (Session["PGTripType"] != null && Session["PGTripType"] != "") ? Session["PGTripType"].ToString() : "";

                        bool insStatus = false;

                        if (Session["BookingREquestdetails" + PaymentTrackid.Split('~')[0]] != null)
                        {
                            RQRS.BookingRquest _BookingReq = (RQRS.BookingRquest)Session["BookingREquestdetails" + PaymentTrackid.Split('~')[0]];
                            _BookingReq.PaymentrefID = Trackid.ToString().Split('|')[0];
                            _BookingReq.Track_Create = false;
                            ViewBag.PaxMobileNo = _BookingReq.PaxDetails[0].Mobnumber;
                            ViewBag.PaxEmail = _BookingReq.PaxDetails[0].MailID;
                            if (_BookingReq.ItineraryFlights.Count > 0)
                            {
                                for (var N = 0; _BookingReq.ItineraryFlights.Count > N; N++)
                                {
                                    _BookingReq.ItineraryFlights[N].TrackId = PaymentTrackid.Split('~')[N];
                                    Tocostcenter = _BookingReq.CBT_Input.CostCenter;
                                }
                            }

                            string strjson = JsonConvert.SerializeObject(_BookingReq);
                            string BookingDetails_LOG = "";
                            string strResponse = string.Empty;
                            BookingDetails_LOG = strjson;

                            #region Log for Booking Request
                            string LstrDetails = string.Empty;
                            string ReqTime = "";
                            if (Base.AvailLog)
                            {
                                ReqTime = "PGBOOKINGREQEST" + DateTime.Now;
                                LstrDetails = "<URL>[<![CDATA[" + strURLpath + "]]>]</URL><DATA>" +
                                     "[<![CDATA[" + Request.QueryString.ToString() + "]]>]</DATA><JSON>" + strjson + "</JSON><TIME>" + ReqTime + "</TIME>";

                                DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PG~BRQ~" + Trackid + "", LstrDetails.ToString(), strAgentID, strTerminalID, strSequenceID);

                            }
                            #endregion


                            MyWebClient Client = new MyWebClient();
                            Client.LintTimeout = bookingtimeout;
                            Client.Headers["Content-type"] = "application/json";


                            byte[] byteGetLogin = Client.UploadData(strURLpath + "/" + "InvokeBooking", "POST", Encoding.ASCII.GetBytes(strjson));
                            strResponse = Encoding.ASCII.GetString(byteGetLogin);

                            #region Log for BOOKING Response
                            string xmldata = string.Empty;
                            xmldata = "<EVENT><RESPONSE>Flight BOOKING </RESPONSE>";
                            xmldata += "<BOOKING_RESPONSE>" + strResponse.ToString() + "</BOOKING_RESPONSE>";
                            xmldata += "<BOOKING_REQUEST>" + BookingDetails_LOG.ToString() + "</BOOKING_REQUEST>";
                            xmldata += "</EVENT>";
                            DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PG~BRS~", xmldata.ToString(), strAgentID, strTerminalID, strSequenceID);

                            #endregion

                            #region Booking Response

                            RQRS.BookingRS _BookingRes = JsonConvert.DeserializeObject<RQRS.BookingRS>(strResponse);
                            AirTrackID = _BookingRes.TrackId;
                            ViewBag.pgPgError = _BookingRes.Error != null && _BookingRes.Error != "" ? _BookingRes.Error : "Payment received successfully.Problem occured while booking.(#01)";
                            string result = _BookingRes.ResultCode;
                            if (_BookingRes.ResultCode == "1")
                            {
                                string stu = "1";
                                strTrackstatus = "S";
                                strConfirmStatus = "S";
                                strAirStatus = "S";
                                xmldata = "";
                                #region Log for BOOKING Response
                                xmldata = "<EVENT><RESPONSE>Flight BOOKING </RESPONSE>";
                                xmldata += "<DATA>" + strResponse + "</DATA>";
                                xmldata += "<FETCH_FLIGHT_BOOKING_RESULT>" + strResponse.ToString() + "</FETCH_FLIGHT_BOOKING_RESULT>";
                                xmldata += "<BOOKINGDETAILS>" + BookingDetails_LOG.ToString() + "</BOOKINGDETAILS>";
                                DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PG~BRS~", xmldata.ToString(), strAgentID, strTerminalID, strSequenceID);
                                #endregion

                            }
                            else if (_BookingRes.ResultCode == "3")
                            {
                                #region  Clear All Cache element
                                foreach (var element in MemoryCache.Default)
                                {
                                    MemoryCache.Default.Remove(element.Key);
                                }
                                #endregion

                                string stu = "3";
                                strTrackstatus = "P";
                                strConfirmStatus = "P";
                                strAirStatus = "P";
                                string err = _BookingRes.Error;
                                xmldata = string.Empty;
                                xmldata += "<EVENT><DATA>" + err + "</DATA></EVENT>";

                                DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGBOOKINGRESPONSE", xmldata.ToString(), strAgentID, strTerminalID, strSequenceID);

                                // return Json(new { status = stu, Errormsg = err });
                                dsTripUpdate = Inplant_wsdl.UpdateTrackDetails(AirTrackID, "", "", "", "", Session["SessstrTripID"] != null ? Session["SessstrTripID"].ToString() : "", CompanyID, LoguserID, Session["strTripTrackID"] != null ? Session["strTripTrackID"].ToString() : "", strTrackstatus, "P", AirlineFare, Passengername, "", "AIR", "S", "", "", "", "");
                            }
                            else
                            {
                                string stu = "0";
                                strTrackstatus = "P";
                                strConfirmStatus = "P";
                                strAirStatus = "P";
                                string err = "";
                                if (_BookingRes.Error == "" && _BookingRes.Error == null)
                                {
                                    err = "Problem occured while booking. please contact support team (#01).";
                                }
                                else
                                {
                                    err = _BookingRes.Error + "(#01).";
                                }

                                xmldata = "";
                                #region Log for BOOKING Response
                                xmldata = "<EVENT><RESPONSE>FLIGHT BOOKING </RESPONSE>";
                                xmldata += "<DATA>" + strResponse + "</DATA>";
                                xmldata += "<FETCH_FLIGHT_BOOKING_RESULT>" + strResponse.ToString() + "</FETCH_FLIGHT_BOOKING_RESULT>";
                                xmldata += "<BOOKINGTAILS>" + BookingDetails_LOG.ToString() + "</BOOKINGDETAILS>";

                                if (_BookingRes.Error.ToString() != "")
                                {
                                    xmldata += "<DATA>" + _BookingRes.Error + "</DATA>";
                                }
                                else
                                {
                                    xmldata += "<DATA>Failed</DATA>";
                                }
                                xmldata += "</EVENT>";
                                #endregion
                                DatabaseLog.LogData(strUsername, "E", "PGDIRECTCONTROLLER", "PGBOOKINGRESPONSE", xmldata.ToString(), strAgentID, strTerminalID, strSequenceID);

                                dsTripUpdate = Inplant_wsdl.UpdateTrackDetails(AirTrackID, "", "", "", "", Session["SessstrTripID"] != null ? Session["SessstrTripID"].ToString() : "", CompanyID, LoguserID, Session["strTripTrackID"] != null ? Session["strTripTrackID"].ToString() : "", strTrackstatus, "P", AirlineFare, Passengername, "", "AIR", "S", "", "", "", "");

                            }

                            #region ERPPUSH
                            if ((_BookingRes.ResultCode == "1" || _BookingRes.ResultCode == "5") && ConfigurationManager.AppSettings["STS_ERP_PUSH"].ToString().ToUpper() == "Y")
                            {
                                try
                                {
                                    string ERPPUSHDETAILS = string.Empty;

                                    var ERPPushDet = (from Ticket_F in _BookingRes.PnrDetails
                                                      group Ticket_F by new { Ticket_F.SPNR } into GetTotalFare
                                                      select new
                                                      {
                                                          SPNR = GetTotalFare.ToArray().Distinct().FirstOrDefault().SPNR,
                                                          USERNAME = GetTotalFare.ToArray().Distinct().FirstOrDefault().FIRSTNAME + " " + GetTotalFare.ToArray().Distinct().FirstOrDefault().LASTNAME,
                                                      }).ToList();
                                    if (ERPPushDet != null && ERPPushDet.Count > 0)
                                    {
                                        for (int i = 0; i < ERPPushDet.Count; i++)
                                        {
                                            ERPPUSHDETAILS += ERPPushDet[i].SPNR + "~";
                                        }
                                        RayService.ERPPUSHFORBOOKING(CompanyID, ERPPUSHDETAILS, "", "AIR", "S", "", ERPPushDet[0].USERNAME, ControllerContext.HttpContext.Request.UserHostName.ToString(), "");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DatabaseLog.LogData(strUsername, "X", "FromBookingRequest", "BOOKING ERP-PUSH FUNCTION", ex.ToString(), strAgentID, Session["POS_TID"].ToString(), strSequenceID);
                                }

                            }
                            #endregion

                            if (string.IsNullOrEmpty(strResponse))
                            {
                                string date = Base.LoadServerdatetime();

                                string lstrBkingdetail = (BlockTicket ? "<BLOKING_RESPONSE>" : "<BOOKING_RESPONSE>") +
                                           ("<STATUS>FAILED</STATUS>") +
                                          ("<RESPONSE>Null</RESPONSE>") +
                                           "<AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE_TIME>" + date +
                                       "</RESPONSE_TIME>" + (BlockTicket ? "</BLOKING_RESPONSE>" : "</BOOKING_RESPONSE>");

                                string refstring = string.Empty;
                                RayService.Apps_Booking_Response_Receive_Status("", strAgentID, Session["POS_TID"].ToString(),
                                  strUsername, Session["TerminalType"].ToString(), Session["ipAddress"].ToString(), strSequenceID,
                                 ref refstring, lstrBkingdetail, "FormBookingRequest");


                                Session.Add("BookStart" + ValKey, "false");
                                Array_Book[error] = "Problem occured while booking. Please Contact Customercare.";
                                if (ConfigurationManager.AppSettings["SENDPENDINGMAIL"].ToString().ToUpper().Trim() == "Y")
                                {
                                    Sendmailforbookingstatus(strResponse, strjson, _PriceItenaryRS.PriceItenarys,"Failed");
                                }
                                //return Array_Book;
                                //return Json(new { Status = "", Message = "", Result = Array_Book });
                                return PartialView("_BookingSuccess", "");
                            }
                            else
                            {
                                Array_Book[bookingresponse] = strResponse;
                                ViewBag.bookingresponse = strResponse;
                                var RetVal = JsonConvert.DeserializeObject<RQRS.BookingRS>(strResponse);

                                string RetVale = JsonConvert.SerializeObject(RetVal);
                                dsBookFlight = Serv.convertJsonStringToDataSet(RetVale, "");

                                if (RetVal != null)
                                {
                                    string lstrBookingdetail = "";
                                    try
                                    {
                                        lstrBookingdetail = (BlockTicket ? "<BLOKING_RESPONSE>" : "<BOOKING_RESPONSE>") +
                                          ((RetVal != null && (RetVal.ResultCode == "1")) ?
                                          ("<STATUS>SUCCESS</STATUS>") :
                                          ((RetVal != null && (RetVal.ResultCode == "2")) ?
                                          ("<STATUS>FARECHANGE</STATUS>") : ("<STATUS>FAILED</STATUS>"))) +
                                          ((RetVal != null && RetVal.ResultCode != null && RetVal.ResultCode.ToString().Trim() != "") ?
                                          ("<ResultCode>" + RetVal.ResultCode.ToString().Trim() + "</ResultCode>") : ("<ResultCode>Empty/Null</ResultCode>")) +
                                          ((RetVal != null && RetVal.Sqe != null && RetVal.Sqe.ToString().Trim() != "") ?
                                          ("<Sqe>" + RetVal.Sqe.ToString().Trim() + "</Sqe>") : ("<Sqe>Empty/Null</Sqe>")) +
                                          //((RetVal != null && RetVal.Stock != null && RetVal.Stock.ToString().Trim() != "") ?
                                          //("<STOCK>" + RetVal.Stock.ToString().Trim() + "</STOCK>") : ("<STOCK>Empty/Null</STOCK>")) +
                                          ((RetVal != null && RetVal.Error != null && RetVal.Error.ToString().Trim() != "") ?
                                          ("<Error>" + RetVal.Error.ToString().Trim() + "</Error>") : ("<Error>Empty/Null</Error>")) +
                                          ((RetVal != null && RetVal.TrackId != null && RetVal.TrackId.ToString().Trim() != "") ?
                                          ("<TrackId>" + RetVal.TrackId.ToString().Trim() + "</TrackId>") : ("<TrackId>Empty/Null</TrackId>")) +
                                          (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].SPNR != null ?
                                          ("<SPNR>" + RetVal.PnrDetails[0].SPNR.ToString().Trim() + "</SPNR>") : "") +
                                          (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].AIRLINEPNR != null ?
                                          ("<AIRLINEPNR>" + RetVal.PnrDetails[0].AIRLINEPNR.ToString().Trim() + "</AIRLINEPNR>") : "") +
                                          (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].CRSPNR != null ?
                                          ("<CRSPNR>" + RetVal.PnrDetails[0].CRSPNR.ToString().Trim() + "</CRSPNR>") : "") +
                                          (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].GROSSFARE != null ?
                                          ("<GROSSFARE>" + RetVal.PnrDetails[0].GROSSFARE.ToString().Trim() + "</GROSSFARE>") : "") +
                                          (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].TICKETINGCARRIER != null ?
                                          ("<TICKETINGCARRIER>" + RetVal.PnrDetails[0].TICKETINGCARRIER.ToString().Trim() + "</TICKETINGCARRIER>") : "") +
                                          (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].TRIPTYPE != null ?
                                          ("<TRIPTYPE>" + RetVal.PnrDetails[0].TRIPTYPE.ToString().Trim() + "</TRIPTYPE>") : "") +
                                          (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].OFFICEID != null ?
                                          ("<OFFICEID>" + RetVal.PnrDetails[0].OFFICEID.ToString().Trim() + "</OFFICEID>") : "") +
                                          (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 && RetVal.PnrDetails[0].OFFLINEFLAG != null ?
                                          ("<OFFLINEFLAG>" + RetVal.PnrDetails[0].OFFLINEFLAG.ToString().Trim() + "</OFFLINEFLAG>") : "") +
                                          (RetVal.PnrDetails != null && RetVal.PnrDetails.Count() > 0 ?
                                          ("<PnrDetailsCount>" + RetVal.PnrDetails.Count().ToString().Trim() + "</PnrDetailsCount>") : "") +
                                          "<AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><URL>[<![CDATA[" + Session["travraysws_url"] + "/" +
                                         Session["travraysws_vdir"] + "/Rays.svc/" + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (bookingtimeout).ToString() +
                                      "</TIMEOUT><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE_TIME>" +
                                     ReqTime +
                                      "</RESPONSE_TIME>" + (BlockTicket ? "</BLOKING_RESPONSE>" : "</BOOKING_RESPONSE>");
                                    }
                                    catch
                                    {

                                    }

                                    #region result code 1-sri
                                    if (result == "1" || result == "5")
                                    {
                                        bool Insres = false;
                                        if (ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA" && Session["BajajInsDetails"] != null && Session["BajajInsDetails"].ToString() != "" && RetVal.Error.Contains("SUPRKMHx1007086:") == false)
                                        {
                                            string Insdetails = Session["BajajInsDetails"].ToString();
                                            string ClientID = Session["PGagentID"].ToString();
                                            string PaxDet = Session["BajajPaxdet"].ToString();
                                            string ContactDet = Session["BajajContactdet"].ToString();
                                            string gstdeta = Session["BajajGstdetails"].ToString();
                                            Insres = false;

                                            if (ConfigurationManager.AppSettings["Etravelversion"].ToString().ToUpper() != "NEW")
                                            {
                                                ViewBag.Insresponse = BookbajajInsuranceOld(RetVale, Insdetails, PaxDet, ContactDet, ClientID, "P");
                                            }
                                            else
                                            {
                                                ViewBag.Insresponse = BookbajajInsurance(RetVale, Insdetails, _BookingReq.PaxDetails, ContactDet, ClientID, "P", PGBranchId, _BookingReq.PaymentrefID, gstdeta);//4650
                                            }
                                        }
                                        else
                                        {
                                            ViewBag.Insresponse = "";
                                        }

                                        string airwaysName = "";
                                        if (Session["BaseDest" + ValKey].ToString() != null && Session["BaseDest" + ValKey].ToString() != "")
                                        {
                                            string Citycode = Session["BaseDest" + ValKey].ToString();
                                            DataSet dsAirways = new DataSet();
                                            string strAirways = Server.MapPath("~/XML/CityAirport_Lst.xml");
                                            dsAirways.ReadXml(strAirways);

                                            var qryAirlineName = from p in dsAirways.Tables
                                                               ["AIR"].AsEnumerable()
                                                                 where p.Field<string>
                                                               ("ID") == Citycode
                                                                 select p;
                                            DataView dvAirlineCode = qryAirlineName.AsDataView();
                                            if (dvAirlineCode.Count == 0)
                                                airwaysName = Citycode;
                                            else
                                            {
                                                DataTable dtAilineCode = new DataTable();
                                                dtAilineCode = qryAirlineName.CopyToDataTable();
                                                airwaysName = dtAilineCode.Rows[0]["CN"].ToString().Split('-')[0];
                                            }

                                        }

                                        #region Offlinevisarequest
                                        if (ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA" && ConfigurationManager.AppSettings["Offlinevisadestinationcity"] != "" && RetVal.Error.Contains("SUPRKMHx1007086:") == false && BlockTicket == false
                                            && ConfigurationManager.AppSettings["Offlinevisadestinationcity"].Contains(Session["BaseDest" + ValKey].ToString()) == true && Session["PaxCount" + ValKey] != null)
                                        {
                                            string QrystringVisa = string.Empty;
                                            int adultCount = 0;
                                            int childCount = 0;
                                            int infantCount = 0;
                                            ViewBag.Visaoffline = "YES";
                                            string[] strPaxCounts = PaxCount.Split('|');
                                            if (strPaxCounts.Length > 0)
                                            {
                                                adultCount = Convert.ToInt16(strPaxCounts[0]);
                                                childCount = Convert.ToInt16(strPaxCounts[1]);
                                                infantCount = Convert.ToInt16(strPaxCounts[2]);
                                            }
                                            int paxcnt = adultCount + childCount + infantCount;
                                            string today = DateTime.Now.ToString("dd/MM/yyyy");
                                            QrystringVisa = "tid=" + (Session["terminalid"].ToString()) + "&agentid=" + (Session["agentid"].ToString())
                                                          + "&IPA=" + (Session["ipAddress"].ToString()) + "&username=" + (Session["username"].ToString())
                                                          + "&seq=" + (Session["sequenceid"].ToString())
                                                          + "&pax=" + (paxcnt) + "&PNR=" + (RetVal.PnrDetails[0].SPNR.ToString());
                                            string QueryString = "SECKEY=" + Base.EncryptKEy(QrystringVisa, "VISAQRY" + today);
                                            ViewBag.VisaQuerystringURL = QueryString;
                                            ViewBag.ThaiVisa = "NO";
                                            ViewBag.ThaiVisaQuerystringURL = "";
                                        }
                                        else if (ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA" && airwaysName.ToUpper().ToString() == "TH" && RetVal.Error.Contains("SUPRKMHx1007086:") == false && BlockTicket == false)
                                        {
                                            string QrystringVisa = string.Empty;
                                            ViewBag.ThaiVisa = "YES";
                                            string today = DateTime.Now.ToString("dd/MM/yyyy");
                                            QrystringVisa = "tid=" + (Session["terminalid"].ToString()) + "&agentid=" + (Session["agentid"].ToString())
                                                          + "&IPA=" + (Session["ipAddress"].ToString()) + "&username=" + (Session["username"].ToString())
                                                          + "&seq=" + (Session["sequenceid"].ToString());

                                            string QueryString = "SECKEY=" + Base.EncryptKEy(QrystringVisa, "THAIVISA" + today);
                                            ViewBag.ThaiVisaQuerystringURL = QueryString;
                                            ViewBag.Visaoffline = "NO";
                                            ViewBag.VisaQuerystringURL = "";
                                        }
                                        else
                                        {
                                            ViewBag.Visaoffline = "NO";
                                            ViewBag.VisaQuerystringURL = "";
                                            ViewBag.ThaiVisa = "NO";
                                            ViewBag.ThaiVisaQuerystringURL = "";
                                        }
                                        #endregion

                                        if (RetVal.Error.Contains("SUPRKMHx1007086:"))
                                        {
                                            Array_Book[error] = result;

                                            ViewBag.ResultCode = result;
                                            Array_Book[ErrorRespose] = Regex.Split(RetVal.Error, "SUPRKMHx1007086:")[1];
                                            ViewBag.Booktext = Regex.Split(RetVal.Error, "SUPRKMHx1007086:")[1];

                                        }
                                        else
                                        {
                                            Array_Book[error] = result;
                                            ViewBag.ResultCode = result;
                                            Array_Book[ErrorRespose] = "";
                                            ViewBag.Booktext = "";
                                        }

                                        Session.Add("dsBookFlight" + ValKey, dsBookFlight);
                                        var Tkt = from Ticket in RetVal.PnrDetails.AsEnumerable()
                                                  group Ticket by new { Ticket.AIRLINEPNR, Ticket.ORIGIN, Ticket.DESTINATION }
                                                      into grpTicketlst
                                                  select new RQRS.PnrDetails
                                                  {
                                                      SPNR = grpTicketlst.FirstOrDefault().SPNR,
                                                      CRSPNR = grpTicketlst.FirstOrDefault().CRSPNR,
                                                      //SupPNR = Ticket.SupplierPNR != null ? Ticket.SupplierPNR : "N/A",
                                                      AIRLINEPNR = grpTicketlst.FirstOrDefault().AIRLINEPNR,
                                                      FLIGHTNO = grpTicketlst.FirstOrDefault().FLIGHTNO,
                                                      AIRLINECODE = grpTicketlst.FirstOrDefault().AIRLINECODE,
                                                      CLASS = grpTicketlst.FirstOrDefault().CLASS,
                                                      ORIGIN = grpTicketlst.FirstOrDefault().ORIGIN,//Utilities.AirportcityName(Ticket.ORIGIN),
                                                      DESTINATION = grpTicketlst.FirstOrDefault().DESTINATION,//Utilities.AirportcityName(Ticket.DESTINATION),
                                                      DEPARTUREDATE = grpTicketlst.FirstOrDefault().DEPARTUREDATE,
                                                      ARRIVALDATE = grpTicketlst.FirstOrDefault().ARRIVALDATE,
                                                      GROSSFARE = Convert.ToString((Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().GROSSFARE != null && grpTicketlst.FirstOrDefault().GROSSFARE != "") ? grpTicketlst.FirstOrDefault().GROSSFARE : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SERVICECHARGE != null && grpTicketlst.FirstOrDefault().SERVICECHARGE != "") ? grpTicketlst.FirstOrDefault().SERVICECHARGE : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().MARKUP != null && grpTicketlst.FirstOrDefault().MARKUP != "") ? grpTicketlst.FirstOrDefault().MARKUP : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().CLIENTMARKUP != null && grpTicketlst.FirstOrDefault().CLIENTMARKUP != "") ? grpTicketlst.FirstOrDefault().CLIENTMARKUP : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SEATSAMOUNT != null && grpTicketlst.FirstOrDefault().SEATSAMOUNT != "") ? grpTicketlst.FirstOrDefault().SEATSAMOUNT : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().ADDMARKUP != null && grpTicketlst.FirstOrDefault().ADDMARKUP != "") ? grpTicketlst.FirstOrDefault().ADDMARKUP : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().BAGGAGE != null && grpTicketlst.FirstOrDefault().BAGGAGE != "") ? grpTicketlst.FirstOrDefault().BAGGAGE : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().MEALSAMOUNT != null && grpTicketlst.FirstOrDefault().MEALSAMOUNT != "") ? grpTicketlst.FirstOrDefault().MEALSAMOUNT : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SFAMOUNT != null && grpTicketlst.FirstOrDefault().SFAMOUNT != "") ? grpTicketlst.FirstOrDefault().SFAMOUNT : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SFGST != null && grpTicketlst.FirstOrDefault().SFGST != "") ? grpTicketlst.FirstOrDefault().SFGST : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT != null && grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT != "") ? grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT : "0"))),
                                                      NetFare = Convert.ToString((Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().GROSSFARE != null && grpTicketlst.FirstOrDefault().GROSSFARE != "") ? grpTicketlst.FirstOrDefault().GROSSFARE : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SERVICECHARGE != null && grpTicketlst.FirstOrDefault().SERVICECHARGE != "") ? grpTicketlst.FirstOrDefault().SERVICECHARGE : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().MARKUP != null && grpTicketlst.FirstOrDefault().MARKUP != "") ? grpTicketlst.FirstOrDefault().MARKUP : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().CLIENTMARKUP != null && grpTicketlst.FirstOrDefault().CLIENTMARKUP != "") ? grpTicketlst.FirstOrDefault().CLIENTMARKUP : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SEATSAMOUNT != null && grpTicketlst.FirstOrDefault().SEATSAMOUNT != "") ? grpTicketlst.FirstOrDefault().SEATSAMOUNT : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().ADDMARKUP != null && grpTicketlst.FirstOrDefault().ADDMARKUP != "") ? grpTicketlst.FirstOrDefault().ADDMARKUP : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().BAGGAGE != null && grpTicketlst.FirstOrDefault().BAGGAGE != "") ? grpTicketlst.FirstOrDefault().BAGGAGE : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().MEALSAMOUNT != null && grpTicketlst.FirstOrDefault().MEALSAMOUNT != "") ? grpTicketlst.FirstOrDefault().MEALSAMOUNT : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SFAMOUNT != null && grpTicketlst.FirstOrDefault().SFAMOUNT != "") ? grpTicketlst.FirstOrDefault().SFAMOUNT : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SFGST != null && grpTicketlst.FirstOrDefault().SFGST != "") ? grpTicketlst.FirstOrDefault().SFGST : "0")
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT != null && grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT != "") ? grpTicketlst.FirstOrDefault().OTHER_SSR_AMOUNT : "0"))
                                                                    - ((Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().DISCOUNT != null && grpTicketlst.FirstOrDefault().DISCOUNT != "") ? grpTicketlst.FirstOrDefault().DISCOUNT : "0"))
                                                                    + (Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().INCENTIVE != null && grpTicketlst.FirstOrDefault().INCENTIVE != "") ? grpTicketlst.FirstOrDefault().INCENTIVE : "0"))
                                                                    + Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().SERVICECHARGE != null && grpTicketlst.FirstOrDefault().SERVICECHARGE != "") ? grpTicketlst.FirstOrDefault().SERVICECHARGE : "0")
                                                                    + (Base.ServiceUtility.ConvertToDecimal((grpTicketlst.FirstOrDefault().COMMISSION != null && grpTicketlst.FirstOrDefault().COMMISSION != "") ? grpTicketlst.FirstOrDefault().COMMISSION : "0"))))
                                                  };


                                        Array_Book[FlightResponse] = JsonConvert.SerializeObject(Tkt);
                                        ViewBag.FlightResponse = JsonConvert.SerializeObject(Tkt.ToList());

                                        string TotalFare = "";
                                        if (_BookingReq.TripType.ToUpper() == "R" || Session["mulreq"] == "Y") //mulreq="Y" - Domestic Multicity...
                                        {

                                            int index = 0;
                                            string lstrpax = "";
                                            var Paxdet = from FPass in RetVal.PnrDetails.AsEnumerable()
                                                             // where ((index >= (RetVal.PnrDetails.Count / 2) ? false : true) &&
                                                             // (lstrpax.Contains(FPass.SEQNO) ? false : true))

                                                         group FPass by FPass.SEQNO
                                                             into grpfilterlst
                                                         select new RQRS.PnrDetails
                                                         {

                                                             TITLE = grpfilterlst.FirstOrDefault().TITLE,
                                                             FIRSTNAME = grpfilterlst.FirstOrDefault().FIRSTNAME,
                                                             LASTNAME = grpfilterlst.FirstOrDefault().LASTNAME,
                                                             PAXTYPE = grpfilterlst.FirstOrDefault().PAXTYPE,
                                                             DATEOFBIRTH = grpfilterlst.FirstOrDefault().DATEOFBIRTH,
                                                             TICKETNO = grpfilterlst.Select(a => a.TICKETNO).Distinct().Aggregate((a, b) => a + " / " + b),
                                                             USERTRACKID = grpfilterlst.Select(a => a.USERTRACKID).Distinct().Aggregate((a, b) => a + " / " + b),
                                                             GROSSFARE = Convert.ToString((Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().GROSSFARE != null && grpfilterlst.FirstOrDefault().GROSSFARE != "") ? grpfilterlst.FirstOrDefault().GROSSFARE : "0")
                                                                + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SERVICECHARGE != null && grpfilterlst.FirstOrDefault().SERVICECHARGE != "") ? grpfilterlst.FirstOrDefault().SERVICECHARGE : "0")
                                                                + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().MARKUP != null && grpfilterlst.FirstOrDefault().MARKUP != "") ? grpfilterlst.FirstOrDefault().MARKUP : "0")
                                                                + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().CLIENTMARKUP != null && grpfilterlst.FirstOrDefault().CLIENTMARKUP != "") ? grpfilterlst.FirstOrDefault().CLIENTMARKUP : "0")
                                                                + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SEATSAMOUNT != null && grpfilterlst.FirstOrDefault().SEATSAMOUNT != "") ? grpfilterlst.FirstOrDefault().SEATSAMOUNT : "0")
                                                                + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().ADDMARKUP != null && grpfilterlst.FirstOrDefault().ADDMARKUP != "") ? grpfilterlst.FirstOrDefault().ADDMARKUP : "0")
                                                                + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().BAGGAGE != null && grpfilterlst.FirstOrDefault().BAGGAGE != "") ? grpfilterlst.FirstOrDefault().BAGGAGE : "0")
                                                                + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().MEALSAMOUNT != null && grpfilterlst.FirstOrDefault().MEALSAMOUNT != "") ? grpfilterlst.FirstOrDefault().MEALSAMOUNT : "0"))
                                                                + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SFAMOUNT != null && grpfilterlst.FirstOrDefault().SFAMOUNT != "") ? grpfilterlst.FirstOrDefault().SFAMOUNT : "0")
                                                                + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().SFGST != null && grpfilterlst.FirstOrDefault().SFGST != "") ? grpfilterlst.FirstOrDefault().SFGST : "0")
                                                                + Base.ServiceUtility.ConvertToDecimal((grpfilterlst.FirstOrDefault().OTHER_SSR_AMOUNT != null && grpfilterlst.FirstOrDefault().OTHER_SSR_AMOUNT != "") ? grpfilterlst.FirstOrDefault().OTHER_SSR_AMOUNT : "0")
                                                              ),
                                                             SERVICECHARGE = ((grpfilterlst.FirstOrDefault().SERVICECHARGE != null && grpfilterlst.FirstOrDefault().SERVICECHARGE != "") ? grpfilterlst.FirstOrDefault().SERVICECHARGE : "0"),
                                                             MARKUP = grpfilterlst.FirstOrDefault().CLIENTMARKUP,
                                                             test = index++,
                                                             pax = faresplit(grpfilterlst.FirstOrDefault().SEQNO, ref lstrpax),
                                                             SPNR = grpfilterlst.Select(a => a.SPNR).Distinct().Aggregate((a, b) => a + " / " + b)
                                                         };

                                            Array_Book[PaxResponse] = JsonConvert.SerializeObject(Paxdet);
                                            ViewBag.PaxResponse = JsonConvert.SerializeObject(Paxdet.ToList());
                                            lstrpax = "";
                                            index = 0;
                                            var PaxFare = (from FPass in RetVal.PnrDetails.AsEnumerable()
                                                              //    where ((index >= (RetVal.PnrDetails.Count / 2) ? false : true) 
                                                              //    &&
                                                              //(lstrpax.Contains(FPass.SEQNO) ? false : true))
                                                          select new
                                                          {
                                                              GROSSFARE = ((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0"))
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0")
                                                                            ),
                                                              NetFare = ((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0")))
                                                                            - ((Base.ServiceUtility.ConvertToDecimal((FPass.DISCOUNT != null && FPass.DISCOUNT != "") ? FPass.DISCOUNT : "0"))
                                                                            + (Base.ServiceUtility.ConvertToDecimal((FPass.INCENTIVE != null && FPass.INCENTIVE != "") ? FPass.INCENTIVE : "0"))
                                                                            + (Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"))
                                                                            + (Base.ServiceUtility.ConvertToDecimal((FPass.COMMISSION != null && FPass.COMMISSION != "") ? FPass.COMMISSION : "0"))),//
                                                              pax = faresplit(FPass.SEQNO, ref lstrpax),
                                                              pnrs = FPass.SPNR,
                                                              index = index++
                                                          }).ToList();

                                            //TotalFare = PaxFare.GroupBy(ax => ax.pnrs).Select(Grss => Convert.ToDouble(Grss.FirstOrDefault().GROSSFARE)).ToArray().Sum().ToString(); //Commented by saranraj on 20170705 to solve multipax sum...
                                            TotalFare = PaxFare.GroupBy(ax => new { ax.pnrs, ax.pax }).Select(Grss => Convert.ToDouble(Grss.FirstOrDefault().GROSSFARE)).Sum().ToString(); //Added by saranraj...
                                            string TotalNetFare = PaxFare.GroupBy(ax => new { ax.pnrs, ax.pax }).Select(Grss => Convert.ToDouble(Grss.FirstOrDefault().NetFare)).Sum().ToString();

                                            //Array_Book[FlightResponse] = JsonConvert.SerializeObject(Tkt);
                                            Array_Book[GrossFareRespose] = JsonConvert.SerializeObject(TotalFare);

                                            ViewBag.GrossFareRespose = JsonConvert.SerializeObject(TotalFare);
                                            ViewBag.NetFareResponse = JsonConvert.SerializeObject(TotalNetFare);
                                        }
                                        else
                                        {
                                            string lstrpax = "";
                                            var Paxdet = (from FPass in RetVal.PnrDetails.AsEnumerable()
                                                          where ((lstrpax.Contains(FPass.SEQNO) ? false : true))
                                                          select new RQRS.PnrDetails
                                                          {
                                                              TITLE = FPass.TITLE,
                                                              FIRSTNAME = FPass.FIRSTNAME,
                                                              LASTNAME = FPass.LASTNAME,
                                                              PAXTYPE = FPass.PAXTYPE,
                                                              DATEOFBIRTH = FPass.DATEOFBIRTH,
                                                              TICKETNO = FPass.TICKETNO,
                                                              USERTRACKID = FPass.USERTRACKID,
                                                              GROSSFARE = Convert.ToString(((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0"))
                                                                 + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                                 + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                                 + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                                                                 + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                                 + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                                                                 + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                                 + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                                                                 + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                                                                 + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                                                                 + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0")
                                                                )),
                                                              SERVICECHARGE = ((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"),
                                                              MARKUP = FPass.CLIENTMARKUP,
                                                              pax = faresplit(FPass.SEQNO, ref lstrpax),
                                                              SPNR = FPass.SPNR

                                                          }).ToList();

                                            Array_Book[PaxResponse] = JsonConvert.SerializeObject(Paxdet);
                                            ViewBag.PaxResponse = JsonConvert.SerializeObject(Paxdet.ToList());

                                            lstrpax = "";
                                            var PaxFare = (from FPass in RetVal.PnrDetails.AsEnumerable()
                                                          where ((lstrpax.Contains(FPass.SEQNO) ? false : true))
                                                          select new
                                                          {
                                                              GROSSFARE = ((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                                  + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                                  + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                                  + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                                  + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                                                                  + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                                                                  + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                                  + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                                                                  + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                                                                  + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                                                                  + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0"))
                                                                ),
                                                              NetFare = ((Base.ServiceUtility.ConvertToDecimal((FPass.GROSSFARE != null && FPass.GROSSFARE != "") ? FPass.GROSSFARE : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.MARKUP != null && FPass.MARKUP != "") ? FPass.MARKUP : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.ADDMARKUP != null && FPass.ADDMARKUP != "") ? FPass.ADDMARKUP : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.CLIENTMARKUP != null && FPass.CLIENTMARKUP != "") ? FPass.CLIENTMARKUP : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SEATSAMOUNT != null && FPass.SEATSAMOUNT != "") ? FPass.SEATSAMOUNT : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.BAGGAGE != null && FPass.BAGGAGE != "") ? FPass.BAGGAGE : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.MEALSAMOUNT != null && FPass.MEALSAMOUNT != "") ? FPass.MEALSAMOUNT : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SFGST != null && FPass.SFGST != "") ? FPass.SFGST : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.SFAMOUNT != null && FPass.SFAMOUNT != "") ? FPass.SFAMOUNT : "0")
                                                                            + Base.ServiceUtility.ConvertToDecimal((FPass.OTHER_SSR_AMOUNT != null && FPass.OTHER_SSR_AMOUNT != "") ? FPass.OTHER_SSR_AMOUNT : "0")))
                                                                            - ((Base.ServiceUtility.ConvertToDecimal((FPass.DISCOUNT != null && FPass.DISCOUNT != "") ? FPass.DISCOUNT : "0"))
                                                                            + (Base.ServiceUtility.ConvertToDecimal((FPass.INCENTIVE != null && FPass.INCENTIVE != "") ? FPass.INCENTIVE : "0"))
                                                                            + (Base.ServiceUtility.ConvertToDecimal((FPass.SERVICECHARGE != null && FPass.SERVICECHARGE != "") ? FPass.SERVICECHARGE : "0"))
                                                                            + (Base.ServiceUtility.ConvertToDecimal((FPass.COMMISSION != null && FPass.COMMISSION != "") ? FPass.COMMISSION : "0"))),//
                                                              pax = faresplit(FPass.SEQNO, ref lstrpax),
                                                              pnrs = FPass.SPNR,
                                                          }).ToList();

                                            TotalFare = PaxFare.Select(Grss => Convert.ToDouble(Grss.GROSSFARE)).ToArray().Sum().ToString();
                                            string TotalNetFare = PaxFare.Select(Grss => Convert.ToDouble(Grss.NetFare)).ToArray().Sum().ToString();

                                            Array_Book[GrossFareRespose] = JsonConvert.SerializeObject(TotalFare);
                                            ViewBag.GrossFareRespose = JsonConvert.SerializeObject(TotalFare);
                                            ViewBag.NetFareResponse = JsonConvert.SerializeObject(TotalNetFare);
                                        }
                                        if (result == "5")
                                        {
                                            string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error + "( " + RetVal.PnrDetails[0].DESTINATION.ToString() + " - " + RetVal.PnrDetails[0].ORIGIN.ToString() + " ) (#01)" : "( " + RetVal.PnrDetails[0].ORIGIN.ToString() + " - " + RetVal.PnrDetails[0].DESTINATION.ToString() + " ) - SUCCESS <br />( " + RetVal.PnrDetails[0].DESTINATION.ToString() + " - " + RetVal.PnrDetails[0].ORIGIN.ToString() + " ) - FAILED (#03)";
                                            ViewBag.Returnfailederrormsg = msg;
                                        }
                                    }

                                    #endregion result code 1
                                    #region result code 2 -sri

                                    else if (result == "2")
                                    {
                                        string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error : "Unable to process your request. please contact customer care.";
                                        Array_Book[error] = result;
                                        ViewBag.ResultCode = RetVal.ResultCode;
                                        Array_Book[GrossFareRespose] = msg;
                                        ViewBag.GrossFareRespose = msg + "-(#01)";
                                        DatabaseLog.LogData(strUsername, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - " + result + " BOOKING_RESPONSE_MESSAGE_" + msg + "", strAgentID, strTerminalID, strSequenceID);

                                    }
                                    #endregion result code 2 -sri
                                    #region result code 3-sri

                                    else if (result == "3")
                                    {
                                        string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error : "Unable to process your request. please contact customer care";
                                        Array_Book[error] = result;
                                        ViewBag.ResultCode = result;
                                        Array_Book[GrossFareRespose] = msg;
                                        ViewBag.GrossFareRespose = msg + "-(#01)";
                                        DatabaseLog.LogData(strUsername, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - " + result + " BOOKING_RESPONSE_MESSAGE_" + msg + "", strAgentID, strTerminalID, strSequenceID);

                                    }

                                    #endregion result code 3-sri
                                    #region result code 5----round trip double grid --onwared success and retrun failed
                                    else if (result == "5")
                                    {
                                        string msg = "( " + RetVal.PnrDetails[0].ORIGIN.ToString() + " - " + RetVal.PnrDetails[0].DESTINATION.ToString() + " ) - SUCCESS <br />( " + RetVal.PnrDetails[0].DESTINATION.ToString() + " - " + RetVal.PnrDetails[0].ORIGIN.ToString() + " ) - FAILED";
                                        Array_Book[error] = result;
                                        ViewBag.ResultCode = result;
                                        Array_Book[GrossFareRespose] = msg;
                                        ViewBag.GrossFareRespose = msg + "-(#01)";

                                    }
                                    #endregion
                                    else
                                    {
                                        string msg = RetVal.Error != null && RetVal.Error != "" ? RetVal.Error : "Problem occured while booking. please contact support team.";
                                        Array_Book[error] = RetVal.ResultCode;
                                        ViewBag.ResultCode = RetVal.ResultCode;
                                        Array_Book[GrossFareRespose] = msg;
                                        ViewBag.GrossFareRespose = msg + "-(#01)";
                                        DatabaseLog.LogData(strUsername, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - " + result + " BOOKING_RESPONSE_MESSAGE_" + msg + "", strAgentID, strTerminalID, strSequenceID);
                                    }
                                    if (ConfigurationManager.AppSettings["SENDPENDINGMAIL"].ToString().ToUpper().Trim() == "Y" && result != "1" && result != "3" && result != "7")
                                    {
                                        Sendmailforbookingstatus(strResponse, strjson, _PriceItenaryRS.PriceItenarys,(result == "2" ? "Failed" : "Pending"));
                                    }
                                }
                                else
                                {
                                    if (ConfigurationManager.AppSettings["SENDPENDINGMAIL"].ToString().ToUpper().Trim() == "Y")
                                    {
                                        Sendmailforbookingstatus(strResponse, strjson, _PriceItenaryRS.PriceItenarys,"Failed");
                                    }
                                    string date = Base.LoadServerdatetime();
                                    string lstrBkingdetail = (BlockTicket ? "<BLOKING_RESPONSE>" : "<BOOKING_RESPONSE>") +
                                               ("<STATUS>FAILED</STATUS>") +
                                              ("<RESPONSE>Null</RESPONSE>") +
                                               "<AIRPORTCATEGORY>" + airportCategory + "</AIRPORTCATEGORY><REQUEST_TIME>" + ReqTime + "</REQUEST_TIME><RESPONSE_TIME>" + date +
                                           "</RESPONSE_TIME>" + (BlockTicket ? "</BLOKING_RESPONSE>" : "</BOOKING_RESPONSE>");
                                }
                                
                                Session.Add("BookStart" + ValKey, "false");
                            }

                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        string msg = "Unable to process your request. please contact customer care";
                        Array_Book[error] = "03";
                        ViewBag.ResultCode = "03";
                        Array_Book[GrossFareRespose] = msg;
                        ViewBag.GrossFareRespose = msg + "-(#01)";
                        DatabaseLog.LogData(strUsername, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - 03" + " BOOKING_RESPONSE_MESSAGE_" + msg + "", strAgentID, strTerminalID, strSequenceID);
                    }
                }
                else
                {
                    string msg = "Unable to process your request. please contact customer care";
                    Array_Book[error] = "03";
                    ViewBag.ResultCode = "03";
                    Array_Book[GrossFareRespose] = msg;
                    ViewBag.GrossFareRespose = msg + "-(#01)";
                    DatabaseLog.LogData(strUsername, "B", "BookFlight Result", "BOOKING RESPONSE", "Result Code - 03" + " BOOKING_RESPONSE_MESSAGE_" + msg + "", strAgentID, strTerminalID, strSequenceID);
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(Result) && Result.ToString().Trim().ToUpper() == "TRUE" || Result.ToString().Trim().ToUpper() == "CAPTURED" || Result.ToString().Trim().ToUpper() == "APPROVED" || Result.ToString().Trim().ToUpper() == "TRANSACTION SUCCESSFUL")
                {
                    ViewBag.pgPgError = "<i class='fa fa-check-circle-o' style='color:green;padding-right: 15px;'></i><span  style='color:green;'>Payment received successfully ( " + Request.QueryString["trackid"].ToString() + " ).</span></br><i class='fa fa-times-circle-o' style='padding-right: 15px;'></i><i style='color:red;'>Problem occured while booking (#05)</i>";
                }
                else
                {
                    ViewBag.pgPgError = "Problem occured while Payment Received(#05)";
                }

                DatabaseLog.LogData(strUsername, "X", "PGDIRECTCONTROLLER", "PGRedirectpage", ex.ToString(), strAgentID, strTerminalID, strSequenceID);

            }
            return View("~/Views/Redirect/Airline_Payment.cshtml");

        }

        public void Sendmailforbookingstatus(string Bookresponse, string BookingReq, List<RQRS.PriceItenary> PriceItenarys, string strResult)
        {
            StringBuilder StrMailcontent = new StringBuilder();
            string MailId = string.Empty;
            try
            {
                RQRS.BookingRS Responsevalue = JsonConvert.DeserializeObject<RQRS.BookingRS>(Bookresponse);
                RQRS.BookingRquest Bookingrequest = JsonConvert.DeserializeObject<RQRS.BookingRquest>(BookingReq);
                string strFares = JsonConvert.SerializeObject(PriceItenarys);
                string strAgentLogin = Session["IsAgentLogin"] != null && Session["IsAgentLogin"].ToString() != "" ? Session["IsAgentLogin"].ToString() : "N";
                string strCustomerLogin = Session["CustomerLogin"] != null && Session["CustomerLogin"].ToString() != "" ? Session["CustomerLogin"].ToString() : "N";

                StrMailcontent.Append("");
                StrMailcontent.Append("<div style=\"font-family: sans-serif;\">");
                StrMailcontent.Append("<h4 style=\"color: #2e3246; font-size: 14px;\">Dear Roundtrip Support Team,</h4>");
                StrMailcontent.Append("<p style=\"font-size: 13px; color: #1f264c; margin-top: 0; font-weight: 600;\">There is a " + (!string.IsNullOrEmpty(strResult) ? strResult : "Pending") + " Booking which needs urgent action from your side </p>");
                StrMailcontent.Append("<br />");
                StrMailcontent.Append("<div class=\"Airline_details\" style=\"width: 100%; float: left;\">");
                StrMailcontent.Append("<div class=\"Tband\" style=\"font-weight: 600; font-size: 18px;margin-bottom: 20px;\">");
                StrMailcontent.Append("<span style=\"color:#d60015;border-bottom: 2px solid #d60015;padding-bottom: 8px;\">Booking</span><span style=\"color:#170079;border-bottom: 2px solid #170079;padding-bottom: 8px;\"> Details</span>");
                StrMailcontent.Append("</div>");
                StrMailcontent.Append("<table style=\"width:70%;\">");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:45%;\">Attempted on</td>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:10%;\">:</td>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:45%;\">" + DateTime.Now.ToString("dddd, dd MMMM yyyy") + " at " + DateTime.Now.ToString("hh:mm tt") + "</td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:45%;\">By </td>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:10%;\">:</td>");
                string strAttemptedBy = strAgentLogin == "Y" ? Bookingrequest.AddressDetails.AgencyName : (strCustomerLogin == "Y" ? Bookingrequest.AddressDetails.UserName : (Bookingrequest.PaxDetails[0].Title + ". " + Bookingrequest.PaxDetails[0].FirstName + " " + Bookingrequest.PaxDetails[0].LastName));
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:45%;\">" + strAttemptedBy + "</td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:45%;\">Transaction ID</td>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:10%;\">:</td>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:45%;\">" + Responsevalue.TrackId + "</td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:45%;\">Pax E-Mail</td>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:10%;\">:</td>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:45%;\">" + Bookingrequest.PaxDetails[0].MailID + "</td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:45%;\">Pax Contact Number</td>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:10%;\">:</td>");
                StrMailcontent.Append("<td style=\"font-size: 15px;color: #1f264c;margin-top: 0;padding: 8px 0;width:45%;\">" + Bookingrequest.PaxDetails[0].Mobnumber + "</td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("</table>");
                StrMailcontent.Append("</div>");

                StrMailcontent.Append("<div class=\"Airline_details\" style=\"width: 100%; float: left;margin-top:20px;\">");
                StrMailcontent.Append("<div class=\"Tband\" style=\"font-weight: 600; font-size: 18px;margin-bottom: 25px;\">");
                StrMailcontent.Append("<span style=\"color:#d60015;border-bottom: 2px solid #d60015;padding-bottom: 8px;\">Flight</span><span style=\"color:#170079;border-bottom: 2px solid #170079;padding-bottom: 8px;\"> Details</span>");
                StrMailcontent.Append("</div>");

                StrMailcontent.Append("<table class=\"TravelTable\" style=\"border-collapse: collapse; width: 100%; margin: 10px auto; margin-bottom: 15px; font-size: 14px;text-align:center;\">");
                StrMailcontent.Append("<thead>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;font-size: 14px;\">Orgin </th>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;font-size: 14px;\">Destination </th>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;font-size: 14px;\">Dep.Date &amp; Time </th>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;font-size: 14px;\">Arr.Date &amp; Time </th>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;font-size: 14px;\">Flight No. </th>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("</thead>");
                StrMailcontent.Append("<tbody>");
                for (int i = 0; i < Bookingrequest.ItineraryFlights.Count; i++)
                {
                    List<RQRS.Flights> lstFlightDetails = Bookingrequest.ItineraryFlights[i].FlightDetails;
                    for (int j = 0; j < lstFlightDetails.Count; j++)
                    {
                        StrMailcontent.Append("<tr>");
                        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" + Base.Utilities.AirportcityName(lstFlightDetails[j].Origin) + " (" + lstFlightDetails[j].Origin + ")</td>");
                        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" + Base.Utilities.AirportcityName(lstFlightDetails[j].Destination) + " (" + lstFlightDetails[j].Destination + ")</td>");
                        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" + lstFlightDetails[j].DepartureDateTime + "</td>");
                        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" + lstFlightDetails[j].ArrivalDateTime + "</td>");
                        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" + lstFlightDetails[j].FlightNumber + "</td>");
                        StrMailcontent.Append("</tr>");
                    }
                }
                StrMailcontent.Append("</tbody>");
                StrMailcontent.Append("</table>");

                StrMailcontent.Append("<div class=\"Tband\" style=\"font-weight: 600; font-size: 18px;margin-bottom: 25px;\">");
                StrMailcontent.Append("<span style=\"color:#d60015;border-bottom: 2px solid #d60015;padding-bottom: 8px;\">Pax & Fare</span><span style=\"color:#170079;border-bottom: 2px solid #170079;padding-bottom: 8px;\"> Details</span>");
                StrMailcontent.Append("</div>");

                StrMailcontent.Append("<table class=\"TravelTable\" style=\"border-collapse: collapse; width: 100%; margin: 10px auto; margin-bottom: 15px; font-size: 14px;\">");
                StrMailcontent.Append("<thead>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 900; padding: 8px; background-color: #f1f1f1;font-size: 12px;\">Passenger Name </th>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 900; padding: 8px; background-color: #f1f1f1;font-size: 12px;\">Basic fare</th>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 900; padding: 8px; background-color: #f1f1f1;font-size: 12px;\">Tax&nbsp;&amp;Others</th>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 900; padding: 8px; background-color: #f1f1f1;font-size: 12px;\">Meal</th>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 900; padding: 8px; background-color: #f1f1f1;font-size: 12px;\">Seat</th>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 900; padding: 8px; background-color: #f1f1f1;font-size: 12px;\">Baggage</th>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 900; padding: 8px; background-color: #f1f1f1;font-size: 12px;\">Other SSR</th>");
                StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 900; padding: 8px; background-color: #f1f1f1;font-size: 12px;\">Total</th>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("</thead>");

                StrMailcontent.Append("<tbody style=\"text-align:right;\">");
                int totalBasic = 0, totalTax = 0, totalMeals = 0, totalSeat = 0, totalBagg = 0, totalOtherSSR = 0;
                for (int i = 0; i < Bookingrequest.PaxDetails.Count; i++)
                {
                    int basicfare = 0, taxamount = 0, mealsamt = 0, seatamt = 0, baggamt = 0, otherSSRamt = 0;
                    RQRS.PaxDetails lstPaxDetails = Bookingrequest.PaxDetails[i];
                    string strPaxType = string.Empty;
                    strPaxType = lstPaxDetails.PaxType.ToUpper() == "ADT" ? "(Adult)" : (lstPaxDetails.PaxType.ToUpper() == "CHD" ? "(Child)" : (lstPaxDetails.PaxType.ToUpper() == "INF" ? "(Infant)" : ""));
                    StrMailcontent.Append("<tr>");
                    StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;text-align:left;\">"
                        + lstPaxDetails.Title + ". " + lstPaxDetails.FirstName + " " + lstPaxDetails.LastName + " " + strPaxType + "</td>");

                    for (int j = 0; j < PriceItenarys.Count; j++)
                    {
                        RQRS.Faredescription _Fares = PriceItenarys[j].PriceRS[0].Fares[0].Faredescription[i];
                        basicfare += Convert.ToInt32(_Fares.BaseAmount);
                        taxamount += Convert.ToInt32(_Fares.TotalTaxAmount) + Convert.ToInt32(_Fares.Servicecharge)
                            + Convert.ToInt32(_Fares.ClientMarkup);
                    }

                    for (int j = 0; j < Bookingrequest.ItineraryFlights.Count; j++)
                    {
                        List<RQRS.MealsSSR> lstMealsDetails = Bookingrequest.ItineraryFlights[j].MealsSSR;
                        List<int> qryMamt = (from _meals in lstMealsDetails.AsEnumerable()
                                             where _meals.PaxRefNumber == (i + 1).ToString()
                                             select Convert.ToInt32(_meals.MealAmount)).ToList();
                        mealsamt += qryMamt.Sum();
                        List<RQRS.BaggSSR> lstBaggDetails = Bookingrequest.ItineraryFlights[j].BaggSSR;
                        List<int> qryBamt = (from _meals in lstBaggDetails.AsEnumerable()
                                             where _meals.PaxRefNumber == (i + 1).ToString()
                                             select Convert.ToInt32(_meals.BaggAmount)).ToList();
                        baggamt += qryBamt.Sum();
                        List<RQRS.SeatsSSR> lstSeatDetails = Bookingrequest.ItineraryFlights[j].SeatsSSR;
                        List<int> qrySamt = (from _meals in lstSeatDetails.AsEnumerable()
                                             where _meals.PaxRefNumber == (i + 1).ToString()
                                             select Convert.ToInt32(_meals.SeatAmount)).ToList();
                        seatamt += qrySamt.Sum();
                        List<RQRS.OtherSSR> lstOtherSSRDetails = Bookingrequest.ItineraryFlights[j].OtherSSR;
                        List<int> qryamt = (from _meals in lstOtherSSRDetails.AsEnumerable()
                                            where _meals.PaxRefNumber == (i + 1).ToString()
                                            select Convert.ToInt32(_meals.Amount)).ToList();
                        otherSSRamt += qryamt.Sum();
                    }

                    StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" + basicfare + "</td>");
                    StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" + taxamount + "</td>");
                    StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" + mealsamt + "</td>");
                    StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" + seatamt + "</td>");
                    StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" + baggamt + "</td>");
                    StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" + otherSSRamt + "</td>");
                    StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 12px;\">" +
                        (basicfare + taxamount + mealsamt + seatamt + baggamt + otherSSRamt) + "</td>");
                    totalBasic += basicfare;
                    totalTax += taxamount;
                    totalMeals += mealsamt;
                    totalSeat += seatamt;
                    totalBagg += baggamt;
                    totalOtherSSR += otherSSRamt;

                    StrMailcontent.Append("</tr>");
                }
                StrMailcontent.Append("</tbody>");
                StrMailcontent.Append("</table>");

                StrMailcontent.Append("</div>");

                #region Passenger details commented by  STS-195
                //StrMailcontent.Append("<div class=\"Tband\" style=\"font-weight: 600; font-size: 18px;margin-bottom: 25px;\">");
                //StrMailcontent.Append("<span style=\"color:#d60015;border-bottom: 2px solid #d60015;padding-bottom: 8px;\">Passenger </span><span style=\"color:#170079;border-bottom: 2px solid #170079;padding-bottom: 8px;\"> Details</span>");
                //StrMailcontent.Append("</div>");

                //StrMailcontent.Append("<table class=\"TravelTable\" style=\"border-collapse: collapse; width: 100%; margin: 10px auto; margin-bottom: 15px; font-size: 14px;\">");
                //StrMailcontent.Append("<thead>");
                //StrMailcontent.Append("<tr>");
                //StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;\">Passenger Name </th>");
                //StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;\">D.O.B</th>");
                //StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;\">Gender</th>");
                //if (Bookingrequest.SegmentType == "I")
                //{
                //    StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;\">Passport No.</th>");
                //    StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;\">Passport Exp.</th>");
                //    StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;\">Nationality</th>");
                //}
                //StrMailcontent.Append("</tr>");
                //StrMailcontent.Append("</thead>");
                //StrMailcontent.Append("<tbody style=\"text-align:right;\">");
                //for (int i = 0; i < Bookingrequest.PaxDetails.Count; i++)
                //{
                //    RQRS.PaxDetails lstPaxDetails = Bookingrequest.PaxDetails[i];
                //    string strPaxType = string.Empty;
                //    strPaxType = lstPaxDetails.PaxType.ToUpper() == "ADT" ? "(Adult)" : (lstPaxDetails.PaxType.ToUpper() == "CHD" ? "(Child)" : (lstPaxDetails.PaxType.ToUpper() == "INF" ? "(Infant)" : ""));
                //    StrMailcontent.Append("<tr>");
                //    StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;text-align:left;\">"
                //        + lstPaxDetails.Title + ". " + lstPaxDetails.FirstName + " " + lstPaxDetails.LastName + " " + strPaxType + "</td>");
                //    StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">"+lstPaxDetails.DOB+"</td>");
                //    StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">"+lstPaxDetails.Gender+"</td>");
                //    if (Bookingrequest.SegmentType == "I")
                //    {
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">"
                //            + lstPaxDetails.PassportNo.Split('|')[0] + " (" + Base.GetCountryName(lstPaxDetails.PassportNo.Split('|')[1].ToString()) + ")</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">"+lstPaxDetails.PassportExpiry+"</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">"+Base.GetCountryName(lstPaxDetails.Nationality)+"</td>");
                //    }
                //    StrMailcontent.Append("</tr>");
                //}
                //StrMailcontent.Append("</tbody>");
                //StrMailcontent.Append("</table>");

                //StrMailcontent.Append("</div>");
                #endregion

                #region Addons Commented BY STS-195
                //StrMailcontent.Append("<div class=\"Airline_details\" style=\"width: 100%; float: left;margin-top:20px;\">");
                //StrMailcontent.Append("<div class=\"Tband\" style=\"font-weight: 600; font-size: 18px;margin-bottom: 25px;\">");
                //StrMailcontent.Append("<span style=\"color:#d60015;border-bottom: 2px solid #d60015;padding-bottom: 8px;\">Add-ons</span><span style=\"color:#170079;border-bottom: 2px solid #170079;padding-bottom: 8px;\"> Details</span>");
                //StrMailcontent.Append("</div>");

                //StrMailcontent.Append("<table class=\"TravelTable\" style=\"border-collapse: collapse; width: 100%; margin: 10px auto; margin-bottom: 15px; font-size: 14px;text-align:center;\">");
                //StrMailcontent.Append("<thead>");
                //StrMailcontent.Append("<tr>");
                //StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;font-size: 17px;\">Pax Ref. No. </th>");
                //StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;font-size: 17px;\">Sector </th>");
                //StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;font-size: 17px;\">SSR Type </th>");
                //StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;font-size: 17px;\">SSR Desc.</th>");
                //StrMailcontent.Append("<th style=\"border: 1px #b7b5b5 solid; font-weight: 600; padding: 8px; background-color: #f1f1f1;font-size: 17px;\">SSR Amount </th>");
                //StrMailcontent.Append("</tr>");
                //StrMailcontent.Append("</thead>");
                //StrMailcontent.Append("<tbody>");

                //int TotalMealsAmt = 0,TotalBaggAmt = 0,TotalSeatAmt = 0,TotalOtherSSRAmt = 0;
                //for (int i = 0; i < Bookingrequest.ItineraryFlights.Count; i++)
                //{
                //    var qryMealsdet = (from _SSR in Bookingrequest.ItineraryFlights[i].MealsSSR.AsEnumerable()
                //                     select new
                //                     {
                //                         PAXREF = _SSR.PaxRefNumber,
                //                         SECTOR = _SSR.Orgin + " - " + _SSR.Destination,
                //                         TYPE = "MEALS",
                //                         DESC = _SSR.MealCode.Split('|')[0],
                //                         FARE = _SSR.MealAmount,
                //                     }).ToList();

                //    var qryBaggdet = (from _SSR in Bookingrequest.ItineraryFlights[i].BaggSSR.AsEnumerable()
                //                       select new
                //                       {
                //                           PAXREF = _SSR.PaxRefNumber,
                //                           SECTOR = _SSR.Orgin + " - " + _SSR.Destination,
                //                           TYPE = "BAGGAGE",
                //                           DESC = _SSR.BaggCode.Split('|')[0],
                //                           FARE = _SSR.BaggAmount,
                //                       }).ToList();

                //    var qrySeatdet = (from _SSR in Bookingrequest.ItineraryFlights[i].SeatsSSR.AsEnumerable()
                //                       select new
                //                       {
                //                           PAXREF = _SSR.PaxRefNumber,
                //                           SECTOR = _SSR.Orgin + " - " + _SSR.Destination,
                //                           TYPE = "SEAT",
                //                           DESC = _SSR.SeatCode,
                //                           FARE = _SSR.SeatAmount,
                //                       }).ToList();

                //    var qryOtherdet = (from _SSR in Bookingrequest.ItineraryFlights[i].OtherSSR.AsEnumerable()
                //                       select new
                //                       {
                //                           PAXREF = _SSR.PaxRefNumber,
                //                           SECTOR = _SSR.Orgin + " - " + _SSR.Destination,
                //                           TYPE = "OTHER",
                //                           DESC = _SSR.SSRCode.Split('|')[0],
                //                           FARE = _SSR.Amount,
                //                       }).ToList();



                //    #region Meals SSR
                //    for (int j = 0; j < Bookingrequest.ItineraryFlights[i].MealsSSR.Count; j++)
                //    {
                //        RQRS.MealsSSR MealsSSR = Bookingrequest.ItineraryFlights[i].MealsSSR[j];
                //        StrMailcontent.Append("<tr>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + MealsSSR.PaxRefNumber + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + MealsSSR.Orgin + " - " + MealsSSR.Destination + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">Meals</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + MealsSSR.Description + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + MealsSSR.MealAmount + "</td>");
                //        TotalMealsAmt += Convert.ToInt32(MealsSSR.MealAmount);
                //        StrMailcontent.Append("</tr>");
                //    }
                //    #endregion
                //    #region Baggage SSR
                //    for (int j = 0; j < Bookingrequest.ItineraryFlights[i].BaggSSR.Count; j++)
                //    {
                //        RQRS.BaggSSR BaggSSR = Bookingrequest.ItineraryFlights[i].BaggSSR[j];
                //        StrMailcontent.Append("<tr>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + BaggSSR.PaxRefNumber + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + BaggSSR.Orgin + " - " + BaggSSR.Destination + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">Baggage</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + BaggSSR.Description + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + BaggSSR.BaggAmount + "</td>");
                //        TotalBaggAmt += Convert.ToInt32(BaggSSR.BaggAmount);
                //        StrMailcontent.Append("</tr>");
                //    }
                //    #endregion
                //    #region Seat SSR
                //    for (int j = 0; j < Bookingrequest.ItineraryFlights[i].SeatsSSR.Count; j++)
                //    {
                //        RQRS.SeatsSSR SeatSSR = Bookingrequest.ItineraryFlights[i].SeatsSSR[j];
                //        StrMailcontent.Append("<tr>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + SeatSSR.PaxRefNumber + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + SeatSSR.Orgin + " - " + SeatSSR.Destination + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">Seats</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + SeatSSR.Description + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + SeatSSR.SeatAmount + "</td>");
                //        TotalSeatAmt += Convert.ToInt32(SeatSSR.SeatAmount);
                //        StrMailcontent.Append("</tr>");
                //    }
                //    #endregion
                //    #region Other SSR
                //    for (int j = 0; j < Bookingrequest.ItineraryFlights[i].MealsSSR.Count; j++)
                //    {
                //        RQRS.OtherSSR OtherSSR = Bookingrequest.ItineraryFlights[i].OtherSSR[j];
                //        StrMailcontent.Append("<tr>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + OtherSSR.PaxRefNumber + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + OtherSSR.Orgin + " - " + OtherSSR.Destination + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">Other SSR</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + OtherSSR.Description + "</td>");
                //        StrMailcontent.Append("<td style=\"padding: 8px; border: 1px #CCC solid;color: #1f264c;font-weight: 600;font-size: 13px;\">" + OtherSSR.Amount + "</td>");
                //        TotalOtherSSRAmt += Convert.ToInt32(OtherSSR.Amount);
                //        StrMailcontent.Append("</tr>");
                //    }
                //    #endregion
                //}

                //StrMailcontent.Append("</tbody>");
                //StrMailcontent.Append("</table>");
                #endregion

                StrMailcontent.Append("<table class=\"TravelTable\" style=\"border-collapse: collapse; width: 30%; float: right; margin: 10px auto; margin-bottom: 15px; font-size: 14px;\">");
                StrMailcontent.Append("<tbody style=\"letter-spacing: 1px;color:#000;\">");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"padding: 6px;font-weight: 900;\">Basic Fare</td>");
                StrMailcontent.Append("<td style=\"text-align: right; padding: 6px;\">" + totalBasic + "</td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"padding: 6px;font-weight: 900;\">Tax &amp; Others </td>");
                StrMailcontent.Append("<td style=\"text-align: right; padding: 6px;\">" + totalTax + "</td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"padding: 6px;font-weight: 900;\">Baggage Charge </td>");
                StrMailcontent.Append("<td style=\"text-align: right; padding: 6px;\">" + totalBagg + "</td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"padding: 6px;font-weight: 900;\">Meal Charge </td>");
                StrMailcontent.Append("<td style=\"text-align: right; padding: 6px;\">" + totalMeals + "</td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"padding: 6px;font-weight: 900;\">Seat Charge </td>");
                StrMailcontent.Append("<td style=\"text-align: right; padding: 6px;\">" + totalSeat + "</td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"padding: 6px;font-weight: 900;\">Other SSR Charge </td>");
                StrMailcontent.Append("<td style=\"text-align: right; padding: 6px;\">" + totalOtherSSR + "</td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("<tr>");
                StrMailcontent.Append("<td style=\"text-align: center; font-size: 18px; padding: 6px;border-top: 2px solid #383737;border-bottom: 2px solid #383737;\"><b>Gross Total</b> </td>");
                StrMailcontent.Append("<td style=\"text-align: right; font-size: 18px; padding: 6px;border-top: 2px solid #383737;border-bottom: 2px solid #383737;\"><b>"
                    + (totalBasic + totalTax + totalBagg + totalMeals + totalSeat + totalOtherSSR) + "</b></td>");
                StrMailcontent.Append("</tr>");
                StrMailcontent.Append("</tbody>");
                StrMailcontent.Append("</table>");

                StrMailcontent.Append("<br />");
                StrMailcontent.Append("<p style=\"width: 100%; float: left; font-size: 13px; color: #1f264c; margin-top: 30px;margin-bottom: 10px; font-weight: 600;\">Pls take necessary actions to resolve this booking</p>");
                StrMailcontent.Append("<p style=\"width: 100%; float: left; font-size: 13px; color: #e10000; margin-top: 10px;margin-bottom: 30px; font-weight: 600;\">NOTE : This is an auto generated email and please do not reply back.</p>");
                StrMailcontent.Append("</div>");
                string strPendingMailID = ConfigurationManager.AppSettings["PENDINGMAILID"] != null ? ConfigurationManager.AppSettings["PENDINGMAILID"].ToString() : "";
                if (strPendingMailID != "")
                {
                    Base.AsyncSendMail("NO", "", strPendingMailID, false, "Booking " + (!string.IsNullOrEmpty(strResult) ? strResult : "Pending"), StrMailcontent.ToString(), 0);//, Bookingrequest.PaxDetails[0].MailID
                }

            }
            catch (Exception ex)
            {
                DatabaseLog.LogData(Session["username"].ToString(), "X", "BookingController", "Sendmail Bookingstatus", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
        }

        public ActionResult Get_CreditShellPNR(string CsAirlinePNR, string strClientID, string strBranchID, string strPaxCount)
        {

            string strResult = string.Empty;
            string strErrorMsg = string.Empty;
            string strResultCode = string.Empty;
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strClientTerminalID = string.Empty;
            int TotalPaxCount = 0, AdultCount = 0, ChildCount = 0, InfantCount = 0;

            string refError = string.Empty;
            string xmlResData = string.Empty;
            string strpnr = string.Empty;
            byte[] dsPNR_det = new byte[] { };
            byte[] dsDet_res = new byte[] { };
            DataSet dsViewPNR = new DataSet();
            DataSet dsDetails = new DataSet();
            string strAgentID = Session["AGENTID"] != null ? Session["AGENTID"].ToString() : "";
            string strTerminalID = Session["TERMINALID"] != null ? Session["TERMINALID"].ToString() : "";
            string strUserName = Session["USERNAME"] != null ? Session["USERNAME"].ToString() : "";
            string strIPAddress = Session["IPADDRESS"] != null ? Session["IPADDRESS"].ToString() : "";
            string strTerminalType = Session["TERMINALTYPE"] != null ? Session["TERMINALTYPE"].ToString() : "";
            string strBranchId = Session["BRANCHID"] != null ? Session["BRANCHID"].ToString() : "";
            string strSeqId = Session["SEQUENCEID"] != null ? Session["SEQUENCEID"].ToString() : DateTime.Now.ToString("ddMMyyyy");
            string strPosId = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPosTId = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strOfficeID = string.Empty;
            try
            {
                RQRS.PriceItenaryRS _PriceItenaryRS = new RQRS.PriceItenaryRS();
                List<RQRS.AvailabilityRS> _AvailRS = new List<RQRS.AvailabilityRS>();

                if (!string.IsNullOrEmpty(strPaxCount))
                {
                    AdultCount = Convert.ToInt32(strPaxCount.Split('@')[0]);
                    ChildCount = Convert.ToInt32(strPaxCount.Split('@')[1]);
                    InfantCount = Convert.ToInt32(strPaxCount.Split('@')[2]);
                    TotalPaxCount = AdultCount + ChildCount + InfantCount;
                }

                if (Session["Priceresponse"] != null && Session["Priceresponse"].ToString() != "")
                {
                    _PriceItenaryRS = JsonConvert.DeserializeObject<RQRS.PriceItenaryRS>(Session["Priceresponse"].ToString());
                    _AvailRS = _PriceItenaryRS.PriceItenarys[0].PriceRS;
                }

                if (strTerminalType == "T" && strClientID != "")
                {
                    strClientID = strClientID.Substring(0, (strClientID.Length - 2));
                    strClientTerminalID = strClientID;
                }
                else
                {
                    strClientID = Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                    strClientTerminalID = Session["POS_TID"] != null && Session["POS_TID"] != "" ? Session["POS_TID"].ToString() : "";
                }

                _RAYS_SERVICE.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                xmlResData = "<EVENT><REQ>GetCSDetails</REQ>"
                + "<strAgentID>" + strClientID + "</strAgentID>"
                + "<strSpnr></strSpnr>"
                + "<strAirpnr>" + CsAirlinePNR + "</strAirpnr>"
                + "<strTerminalID>" + strClientTerminalID + "</strTerminalID>"
                + "<strUserName>" + strUserName + "</strUserName>"
                + "<strTerminalType>" + strTerminalType + "</strTerminalType>"
                + "<AVAILABILITYOFFICEID>" + (string.IsNullOrEmpty(_AvailRS[0].OfficeId) ? "EMPTY" : _AvailRS[0].OfficeId) + "</AVAILABILITYOFFICEID>"
                + "</EVENT>";
                DatabaseLog.LogData(strUserName, "E", "ActionController", "GetCSPNRDetails-REQ", xmlResData.ToString(), strClientID, strClientTerminalID, strSeqId);                            

                bool res = _RAYS_SERVICE.Fetch_PNR_Verification_Details_NewByte(strClientID, "", CsAirlinePNR, "", "", strClientTerminalID, strUserName, strIPAddress,
                    strTerminalType, Convert.ToDecimal(strSeqId), ref dsPNR_det, ref dsDet_res, ref refError, ref strErrorMsg, "Action", "GetCSDetails", strAgentType, strClientTerminalID);
                dsViewPNR = Base.Decompress(dsPNR_det);
              
                xmlResData = "<EVENT><RES>GetCSDetails</RES>"
                + "<PNRRESPONSE>" + dsViewPNR.GetXml() + "</PNRRESPONSE>"
                + "<ERROR1>" + refError.ToString() + "</ERROR1>"
                + "<ERROR2>" + strErrorMsg.ToString() + "</ERROR2>"
                + "<AVAILABILITYOFFICEID>" + (string.IsNullOrEmpty(_AvailRS[0].OfficeId) ? "EMPTY" : _AvailRS[0].OfficeId) + "</AVAILABILITYOFFICEID>"
                + "</EVENT>";
                DatabaseLog.LogData(strUserName, "E", "ActionController", "GetCSPNRDetails-RES", xmlResData.ToString(), strPosId, strPosTId, strSeqId);

                string strAircode = string.Empty; string strFareType = string.Empty; string strRefToken = string.Empty;
                if (dsViewPNR != null && dsViewPNR.Tables.Count > 0)
                {                    
                    strAircode = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["PLATING_CARRIER"].ToString();
                    if (strAircode != "6E" && strAircode != "SG")
                    {
                        strResultCode = "00";
                        strErrorMsg = "The given PNR not allowed for Credit shell booking.";
                        return new JsonResult()
                        {
                            Data = Json(new { Statuscode = strResultCode, Message = strErrorMsg, Result = strResult }),
                            MaxJsonLength = 2147483647
                        };
                    }
                    strFareType = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["SPECIAL_TRIP"] != null ? dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["SPECIAL_TRIP"].ToString() : "";
                    strRefToken = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["Token"] != null ? dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["Token"].ToString() : "";
                    strOfficeID = dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TCK_BOOKED_OFFICEID"] != null ? dsViewPNR.Tables["P_FETCH_PNR_DETAILS_BOA"].Rows[0]["TCK_BOOKED_OFFICEID"].ToString() : "";
                }
                else
                {
                    strResultCode = "00";
                    strErrorMsg = "Please enter valid PNR!.";
                    return new JsonResult()
                    {
                        Data = Json(new { Statuscode = strResultCode, Message = strErrorMsg, Result = strResult }),
                        MaxJsonLength = 2147483647
                    };
                    //strAircode = _AvailRS[0].Flights[0].OperatingCarrier;
                    //strFareType = _AvailRS[0].Fares[0].FareType;
                    //strRefToken = _AvailRS[0].Flights[0].ReferenceToken;
                }

                RQRS.AgentDetails _Agentdet = new RQRS.AgentDetails();
                RQRS.GetCSPNRDetailsRQ _GetcsReq = new RQRS.GetCSPNRDetailsRQ();
                _Agentdet.AgentId = strClientID;
                _Agentdet.Agenttype = strTerminalType == "T" ? strAgentType : strAgentType;
                _Agentdet.AirportID = "";//SegmentType;
                _Agentdet.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();

                _Agentdet.AppType = "B2B";
                _Agentdet.BOAID = strClientID;
                _Agentdet.BOAterminalID = strClientTerminalID;
                _Agentdet.BranchID = strBranchID != null && strBranchID != "" ? strBranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                _Agentdet.IssuingBranchID = strBranchID != null && strBranchID != "" ? strBranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                _Agentdet.ClientID = strClientID;
                _Agentdet.CoOrdinatorID = "";
                _Agentdet.Environment = (Session["Bookapptype"] != null && Session["Bookapptype"].ToString() != "") ? Session["Bookapptype"].ToString() : strTerminalType; ;
                _Agentdet.Platform = "B"; 
                _Agentdet.ProjectID = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"] != "" ? Session["PRODUCT_CODE"].ToString() : "";
                _Agentdet.TerminalId = strClientTerminalID;
                _Agentdet.UserName = Session["username"].ToString();
                _Agentdet.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                _Agentdet.COST_CENTER = "";
                _Agentdet.EMP_ID = "";
                _Agentdet.UID = "";
                _Agentdet.Group_ID = "";
                _Agentdet.Ipaddress = Session["ipaddress"].ToString();  

                _GetcsReq.Agent = _Agentdet;
                _GetcsReq.AirlineCode = strAircode;
                _GetcsReq.CSPNR = CsAirlinePNR;
                _GetcsReq.FareType = strFareType;
                _GetcsReq.FareTypeDescription = strFareType; // _AvailRS[0].Flights[0].FareTypeDescription;
                _GetcsReq.RefToken = strRefToken; // _AvailRS[0].Flights[0].ReferenceToken;

                if (string.IsNullOrEmpty(_AvailRS[0].OfficeId) || string.IsNullOrEmpty(strOfficeID) || strOfficeID.Trim().ToUpper() != _AvailRS[0].OfficeId.Trim().ToUpper())
                {
                    strResultCode = "00";
                    strErrorMsg = "The given Credit shell PNR not allowed for this booking. Please check your fare type.";
                    return new JsonResult()
                    {
                        Data = Json(new { Statuscode = strResultCode, Message = strErrorMsg, Result = strResult }),
                        MaxJsonLength = 2147483647
                    };
                }
                _GetcsReq.PNROfficeID = strOfficeID;

                string request = JsonConvert.SerializeObject(_GetcsReq).ToString();
                string strUrl = ConfigurationManager.AppSettings["APPS_URL"].ToString();
                string Query = "GetCSPNRDetails";
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                xmlResData = "<EVENT>" + request + "</EVENT>";
                DatabaseLog.LogData(strUserName, "E", "ActionController", "GetCSPNRDetailsAPI-REQ", xmlResData.ToString(), strClientID, strClientTerminalID, strSeqId);
                byte[] data = client.UploadData(strUrl + Query, "POST", Encoding.ASCII.GetBytes(request));
                string strResponse = Encoding.ASCII.GetString(data);
                //string strResponse = System.IO.File.ReadAllText("C:\\Users\\Deepa\\Downloads\\GetCSPNRDetails\\Response.txt");
                RQRS.GetCSPNRDetailsRS RetVal = JsonConvert.DeserializeObject<RQRS.GetCSPNRDetailsRS>(strResponse);                
                xmlResData = "<EVENT>"+ strResponse + "</EVENT>";
                DatabaseLog.LogData(strUserName, "E", "ActionController", "GetCSPNRDetailsAPI-RES", xmlResData.ToString(), strClientID, strClientTerminalID, strSeqId);
                if (RetVal.Status.ResultCode == "1")
                {
                    if (RetVal.Passengers.Count >= TotalPaxCount)
                    {
                        int adult = RetVal.Passengers.Count(pax => pax.PaxType == "ADT");
                        int child = RetVal.Passengers.Count(pax => pax.PaxType == "CHD");
                        int infant = RetVal.Passengers.Count(pax => pax.PaxType == "INF");
                        if (adult >= AdultCount && child >= ChildCount && infant >= InfantCount)
                        {
                            RetVal.PhoneNumber = RetVal.PhoneNumber.Contains("+91") ? RetVal.PhoneNumber.Replace("+91", "") : RetVal.PhoneNumber;
                            strResultCode = "01";
                            strErrorMsg = "Credit Balance Available for the Given PNR is <b> " + RetVal.CSAmount + "</b>";
                            strResult = JsonConvert.SerializeObject(RetVal);
                        }
                        else
                        {
                            strResultCode = "00";
                            strErrorMsg = "Passengers count mismatch from the given credit shell PNR";
                        }
                    }
                    else
                    {
                        strResultCode = "00";
                        strErrorMsg = "Passengers count mismatch from the given credit shell PNR";
                    }
                }
                else
                {
                    strResultCode = "00";
                    strErrorMsg = !string.IsNullOrEmpty(RetVal.Status.ErrorToCus) ? (RetVal.Status.ErrorToCus + ".(#01)") : "Unable to get PNR details. Please contact support team.(#03)";
                }
            }
            catch (Exception ex)
            {
                strResultCode = "00";
                strErrorMsg = "Unable to get PNR details. Please contact support team.(#05)";
            }
            return new JsonResult()
            {
                Data = Json(new { Statuscode = strResultCode, Message = strErrorMsg, Result = strResult }),
                MaxJsonLength = 2147483647
            };
        }
    }
}
