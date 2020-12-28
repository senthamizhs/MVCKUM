using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using STSTRAVRAYS.Models;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Runtime.Caching;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.SessionState;
using System.Globalization;
using STSTRAVRAYS.Rays_service;
namespace STSTRAVRAYS.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class SearchController : Controller
    {

        string lstra = string.Empty;
        string strPlatform = ConfigurationManager.AppSettings["PLATFORM"] != null ? ConfigurationManager.AppSettings["PLATFORM"].ToString() : "";

        MemoryCache memCache = MemoryCache.Default;
        RaysService _rays_servers = new RaysService();
        Dictionary<string, string> dictThreadRes = new Dictionary<string, string>();

        string strURLpath = string.Empty;
        string strURLpathA = string.Empty;
        string strProductType = ConfigurationManager.AppSettings["Producttype"].ToString().ToUpper().Trim();
        string strAppType = System.Web.HttpContext.Current.Session["IsAgentLogin"] != null && System.Web.HttpContext.Current.Session["IsAgentLogin"].ToString() != "Y" ? "B2C" : "B2B";
        Base.ServiceUtility Serv = new Base.ServiceUtility();

        async Task<string> AsyncFlightAvail(RQRS.Itineary Itn, string Service_url)
        {
            string content = "";
            string TIMEOUTMINUTS = ConfigurationManager.AppSettings["AVAIL_TIMEOUT"].ToString();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.Timeout = TimeSpan.FromMinutes(Convert.ToInt32(TIMEOUTMINUTS));
                    var ret = await httpClient.PostAsJsonAsync(Service_url, Itn);
                    content = await ret.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {

            }
            return content;
        }

        [AsyncTimeout(180000)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "TimedOut")]
        public async Task<ActionResult> FlightSearch(string strfromCity, string strtoCity, string strDepartureDate,
            string strRetDate, string strAdults, string strChildrens, string strInfants, string strTrip, string Airline, string FCategory,
            string Class, string MultiCity, string HSearch, string DirectSearch, string segmenttype, string roundtripflg, string availagent,
            string availterminal, string Agencyname, string UID, string AppCurrency, string fltnumkey, string fltsegmentkey, string ClientID,
            string BranchID, string GroupID, string mulfltnumvia, string reqker, string Promothread, string Threadspl, bool StudentFare,
            bool ArmyFare, bool SnrcitizenFare, bool LabourFare, string strBookletFare)
        {
            //FCategory = "FSC";
            ArrayList array_Response = new ArrayList();
            string serverdate = Base.cacheServerdatetime();
            //DateTime addcacheday=
            array_Response.Add(""); //for array concept_groupfare
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");

            array_Response.Add(""); //for memcache ccheck

            // int airliefilter = 0;
            int AvailSession = 1;
            int Error = 2;
            int FlightSearchError = 3;
            int JsonResponse = 4;
            int threadnoresponse = 5;
            int availsideflat = 6; //Added by saranraj on 20170516...
            int memcachechck = 7; //added by srinath on 20171213
            int RetavailforRTspl = 8;
            int available_agent = 9;
            int concept_groupfare = 0; //added by srinath on 20180312
            string stu = string.Empty;
            string msg = string.Empty;

            string availsetflag = string.Empty;

            StringBuilder strBuliderRes = new StringBuilder();
            StringBuilder strTaxBuild = new StringBuilder();

            bool Hostsearch = false;
            bool blnBookletFare = false;

            string availsetcount = string.Empty;
            bool cache_availtruefalse = false;
            string threadnoresponse_arg = string.Empty;
            string strAvailSession_arg = string.Empty;

            int totcachemin = 0;
            string gotoflg = string.Empty;
            string strResponse = string.Empty;
            string ReqTime = "";
            byte[] data = null;
            RQRS.AvailabilityRS _availRes = new RQRS.AvailabilityRS();
            CRMDATA _CRMDATA = new CRMDATA();
            string strjsondata = string.Empty;

            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strClientType = Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : "";

            try
            {

                if (Session["agentid"] == null)
                {
                    stu = "-1";
                    msg = "session Expired.";
                    array_Response[concept_groupfare] = "";// array_Response[AvailSession] + "sri" + array_Response[Error] + "sri" + array_Response[FlightSearchError] + "sri" + array_Response[JsonResponse] + "sri" + array_Response[threadnoresponse] + "sri" + array_Response[availsideflat] + "sri" + array_Response[memcachechck];
                    return Json(new { Status = stu, Message = msg, Result = array_Response });
                }

                if (Threadspl == "Y" && ConfigurationManager.AppSettings["RTsplthread"].ToString() != "" && ConfigurationManager.AppSettings["RTsplthread"].ToString().Contains(FCategory) == false)
                {
                    string XML = "<REQUEST><TRIP>" + strTrip + "</TRIP><RTSPLTHREAD>" + ConfigurationManager.AppSettings["RTsplthread"].ToString() + "</RTSPLTHREAD><CATEGORY>" + FCategory + "</CATEGORY></REQUEST>";
                    DatabaseLog.LogData(Session["username"].ToString(), "E", "SearchController", "BlockavailforRTSpecial", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    msg = "No flights available";
                    return Json(new { Status = stu, Message = msg, Result = array_Response });
                }

                #region session assign-problem in asyn hit-sri
                if (reqker.ToString() == "0")
                {
                    #region UsageLog
                    string PageName = "Search Availability";
                    try
                    {
                        string strUsgLogRes = Base.Commonlog(PageName, UID.Split('$').Length > 0 ? UID.Split('$')[0] : UID, "SEARCH");
                    }
                    catch (Exception e) { }
                    #endregion

                    if (Session["roundtripflg_SESS"] == null)
                    {
                        Session.Add("roundtripflg_SESS", roundtripflg);
                    }
                    if (availagent != null && availagent != "")
                    {
                        Session["Availagentid"] = availagent;
                    }
                    else
                    {
                        Session["Availagentid"] = Session["agentid"].ToString();
                    }
                    if (availterminal != null && availterminal != "")
                    {
                        Session["Availterminal"] = availterminal;
                    }
                    else
                    {
                        Session["Availterminal"] = Session["terminalid"].ToString();
                    }
                    if (Agencyname != null && Agencyname != "")
                    {
                        Session["Availagent"] = Agencyname;
                    }
                    else
                    {
                        Session["Availagent"] = Session["agencyname"].ToString();
                    }
                    if (AppCurrency != null && AppCurrency != "")
                    {
                        if (Session["App_currency"] == null)
                        {
                            Session["App_currency"] = AppCurrency;

                        }
                    }
                    Session["segmenttype"] = segmenttype;
                    System.Web.HttpContext.Current.Session["UniqueIduser"] = null;
                    //Session.Add("UniqueIduser", (UID.Contains('$') ? UID.Split('$')[0] : UID));
                    Session.Add("UniqueIduser", UID.Split('$').Length > 0 ? UID.Split('$')[0] : UID);
                }
                #endregion

                string strClientID = string.Empty; string strClientTerminalID = string.Empty;
                if (strTerminalType == "T" && ClientID != "")
                {
                    strClientID = ClientID.Substring(0, (ClientID.Length - 2));
                    strClientTerminalID = ClientID;
                    array_Response[available_agent] = strClientID;
                }
                else
                {
                    strClientID = Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                    strClientTerminalID = Session["POS_TID"] != null && Session["POS_TID"] != "" ? Session["POS_TID"].ToString() : "";
                    array_Response[available_agent] = Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                }

                if (!string.IsNullOrEmpty(strBookletFare))
                {
                    blnBookletFare = Convert.ToBoolean(strBookletFare);
                    Session.Add("ISBOOKLETFARE", blnBookletFare);
                }
                if (blnBookletFare == true && ConfigurationManager.AppSettings["AllowBookletAgent"] != null && ConfigurationManager.AppSettings["AllowBookletAgent"].ToString() != "" &&
                    ConfigurationManager.AppSettings["AllowBookletAgent"].ToString() != "ALL" && !ConfigurationManager.AppSettings["AllowBookletAgent"].ToString().Contains(strClientID))
                {
                    array_Response[Error] = "Booklet fare currently not available.";
                    array_Response[concept_groupfare] = "";
                    return Json(new { Status = "", Message = "", Result = array_Response });
                }

                string strOrigin = string.Empty;
                string strDestination = string.Empty;
                string strDepdate = string.Empty;
                string strArrdate = string.Empty;
                string strTripType = string.Empty;
                string muticount = string.Empty;
                string Faretype = string.Empty;

                string str_agentid = string.Empty;
                int NoofAdt = Convert.ToInt32(strAdults == "" ? "1" : strAdults);
                int NoofChd = Convert.ToInt32(strChildrens == "" ? "0" : strChildrens);
                int NoofInf = Convert.ToInt32(strInfants == "" ? "0" : strInfants);
                availsetcount = segmenttype.Split('_')[1].ToString();
                array_Response[availsideflat] = availsetcount;//Added by saranraj on 20170516 for left side or right side avail binding...
                segmenttype = segmenttype.Split('_')[0].ToString();

                totcachemin = ConfigurationManager.AppSettings["flgMemCacheDuration"] != null ?
                          Convert.ToInt32(ConfigurationManager.AppSettings["flgMemCacheDuration"].ToString()) : 20;
                RQRS.AvailabilityRequests RequestAvail = new RQRS.AvailabilityRequests();
                List<RQRS.AvailabilityRequests> avail = new List<RQRS.AvailabilityRequests>();

                RQRS.PromoCodeRS _PromocodeRsdata = new RQRS.PromoCodeRS();
                List<RQRS.Promocodes> Prcode = new List<RQRS.Promocodes>();
                RQRS.Promocodes Promocodes = new RQRS.Promocodes();
                if (!string.IsNullOrEmpty(Promothread) && Promothread.Length > 2)
                {
                    Faretype = (Promothread.Split('~')[3].ToString() == "U" ? "M" : Promothread.Split('~')[3].ToString());
                }
                else
                {
                    // FCategory = (Airline.Contains("UK") && ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA" && FCategory == "1A") ? "1S" : FCategory;
                    if (FCategory.Length > 2 && Session["CORPORATETHREAD"].ToString().Contains(FCategory) == true)
                    {
                        Faretype = FCategory.Substring(FCategory.Length - 1);
                        //FCategory = FCategory.Substring(0, 2);
                    }
                    else
                    {
                        Faretype = "N";
                    }
                }

                if (FCategory == ConfigurationManager.AppSettings["Air_Thread"].ToString() && segmenttype == "I" && ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA" && strTrip != "M")
                {
                    Faretype = "P";
                }

                if (avail.Count > 0)
                {
                    avail.Clear();
                }
                //int trip = 2;
                if (HSearch != "" && HSearch != null)
                {
                    Hostsearch = Convert.ToBoolean(HSearch);
                }

                #region Labourfare
                bool Islabourfare = false;
                if (LabourFare == true)
                {
                    string[] Labourthread = Session["Labourthread"].ToString().Split('|');
                    string[] Labourairlines = Session["LabourfareAirline"].ToString().Split(',');
                    string Lbrairline = FCategory.Contains("PR_THREAD") ? Regex.Split(FCategory, "PR_THREAD")[1].ToString() : "";
                    FCategory = Regex.Split(FCategory, "PR_THREAD")[0].ToString();

                    if ((Array.IndexOf(Labourthread, FCategory) > -1))
                    {
                        string clntid = (ClientID != "" && strTerminalType == "T" ? strClientID : Session["pos_id"].ToString());
                        string terminalid = (ClientID != "" && strTerminalType == "T" ? strClientTerminalID : Session["POS_TID"].ToString());
                        strDepdate = DateTime.ParseExact(strDepartureDate, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                        InplantService.Inplantservice _rays = new InplantService.Inplantservice();
                        _rays.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                        string Promoerr = string.Empty;
                        string classreq = string.Empty;
                        string treq = string.Empty;
                        string Airline_lst = string.Empty;
                        DataSet dsPricingCode = new DataSet();
                        dsPricingCode = _rays.FetchPricingCodeDetails(clntid, terminalid, strfromCity, strtoCity, segmenttype, strTrip, "", Class, FCategory, "0", Session["username"].ToString(), "W", Convert.ToDecimal(Session["sequenceid"]), Session["ipaddress"].ToString(), ref Promoerr, Lbrairline, strDepdate, "L", "");

                        string Xmldata = "<REQUEST><CLIENTID>" + clntid + "</CLIENTID><POSTID>" + terminalid + "</POSTID><ORIGIN>" + strOrigin + "</ORIGIN><DESTINATION>" + strDestination
                                + "</DESTINATION><SEGMENTTYPE>" + segmenttype + "</SEGMENTTYPE><TRIPTYPE>" + strTrip + "</TRIPTYPE><TRAVELDATE>" + strDepdate + "</TRAVELDATE><CLASS>" + Class + "</CLASS><STOCKTYPE>" + FCategory
                                + "</STOCKTYPE><REQCOUNT>0</REQCOUNT><USERNAME>" + Session["username"].ToString() + "</USERNAME></REQUEST><RESPONSE><DATA>" + dsPricingCode.GetXml() + "</DATA><ERROR>" + Promoerr + "</ERROR></RESPONSE>";

                        DatabaseLog.LogData(Session["username"].ToString(), "E", "Fetchpromocode", "Fetchpromocode-Flightsearch", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        if (dsPricingCode != null && dsPricingCode.Tables.Count > 0 && dsPricingCode.Tables[0].Rows.Count > 0)
                        {
                            for (var i = 0; i < dsPricingCode.Tables[0].Rows.Count; i++)
                            {
                                if ((Lbrairline.Contains(dsPricingCode.Tables[0].Rows[i]["TC_AIRLINE"].ToString())))
                                {
                                    Promocodes = new RQRS.Promocodes();
                                    Promocodes.CodeType = dsPricingCode.Tables[0].Rows[i]["TC_TYPE"].ToString();
                                    Promocodes.Flight = dsPricingCode.Tables[0].Rows[i]["TC_AIRLINE"].ToString();
                                    Promocodes.OfficeId = dsPricingCode.Tables[0].Rows[i]["FTC_TICKET_OFFICE_ID"].ToString();
                                    Promocodes.PromoCode = dsPricingCode.Tables[0].Rows[i]["TC_CODE"].ToString();
                                    Promocodes.PromoCodeDesc = dsPricingCode.Tables[0].Columns.Contains("FTC_SHOW_DESC") == true ? dsPricingCode.Tables[0].Rows[i]["FTC_SHOW_DESC"].ToString() : "";
                                    Prcode.Add(Promocodes);
                                    Airline_lst += dsPricingCode.Tables[0].Rows[i]["TC_AIRLINE"].ToString() + ",";
                                    Islabourfare = (Array.IndexOf(Labourairlines, Lbrairline) > -1) ? true : false;
                                }
                            }
                            Airline = Airline_lst.TrimEnd(',');
                        }
                    }
                    else
                    {
                        msg = "No fares available";
                        return Json(new { Status = stu, Message = msg, Result = array_Response });
                    }
                }

                #endregion

                if (strTrip == "M" && (segmenttype == "I" || blnBookletFare == true))
                {
                    string fltnumtrim = mulfltnumvia == null ? "" : mulfltnumvia.Contains('|') ? mulfltnumvia.ToString().TrimEnd('|') : mulfltnumvia.ToString();
                    string[] aryOrigin = strfromCity.Split(',');


                    string[] aryDestination = strtoCity.Split(',');
                    string[] aryDepart = strDepartureDate.Split(',');
                    string[] splttotcnt = fltnumtrim.Split('|');
                    strOrigin = strfromCity.Split(',')[0];
                    strDestination = strDepartureDate.Split(',')[aryDestination.Length - 1];
                    for (int MD = 0; MD < aryOrigin.Length; MD++)
                    {
                        RequestAvail = new RQRS.AvailabilityRequests();
                        RequestAvail.DepartureStation = aryOrigin[MD];
                        RequestAvail.ArrivalStation = aryDestination[MD];
                        RequestAvail.FlightDate = DateTime.ParseExact(aryDepart[MD], @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                        RequestAvail.FareclassOption = Class;
                        //RequestAvail.Connection = "";
                        //RequestAvail.FlightNo = "";
                        RequestAvail.Connection = splttotcnt[MD].Split(',').Length > 1 ? splttotcnt[MD].Split(',')[1].ToString() : "";
                        RequestAvail.FlightNo = splttotcnt[MD].Split(',').Length > 0 ? splttotcnt[MD].Split(',')[0].ToString() : "";
                        RequestAvail.FareType = Faretype;// "N";
                        RequestAvail.isStudentFare = StudentFare;
                        RequestAvail.isArmyFare = ArmyFare;
                        RequestAvail.isSnrCitizenFare = SnrcitizenFare;
                        RequestAvail.isLabourFare = Islabourfare;
                        avail.Add(RequestAvail);
                    }
                }
                else
                {
                    strOrigin = strfromCity;
                    strDestination = strtoCity;
                    strDepdate = DateTime.ParseExact(strDepartureDate, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                    RequestAvail.DepartureStation = strOrigin;
                    RequestAvail.ArrivalStation = strDestination;
                    RequestAvail.FlightDate = strDepdate;

                    RequestAvail.FlightNo = fltnumkey.Contains(",") ? fltnumkey.Split(',')[0].ToString() : fltnumkey;
                    RequestAvail.Connection = fltsegmentkey.Contains(",") ? fltsegmentkey.Split(',')[0].ToString() : fltsegmentkey;

                    if (strTrip == "R" && Threadspl != "Y")
                    {
                        strArrdate = DateTime.ParseExact(strRetDate, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                        RequestAvail.FlightNo = fltnumkey.Contains(",") ? fltnumkey.Split(',')[1].ToString() : fltnumkey;
                        RequestAvail.Connection = fltsegmentkey.Contains(",") ? fltsegmentkey.Split(',')[1].ToString() : fltsegmentkey;
                    }
                    RequestAvail.FareclassOption = Class;
                    //RequestAvail.Connection = "";
                    //RequestAvail.FlightNo = "";
                    RequestAvail.FareType = Faretype;// "N";
                    RequestAvail.isStudentFare = StudentFare;
                    RequestAvail.isArmyFare = ArmyFare;
                    RequestAvail.isSnrCitizenFare = SnrcitizenFare;
                    RequestAvail.isLabourFare = Islabourfare;
                    avail.Add(RequestAvail);

                    if (strTrip == "Y" || (strTrip == "R" && Threadspl == "Y")) //Roundtrip Special... by saranraj on 20170523...
                    {
                        strArrdate = DateTime.ParseExact(strRetDate, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                        RequestAvail = new RQRS.AvailabilityRequests();
                        RequestAvail.DepartureStation = strDestination;
                        RequestAvail.ArrivalStation = strOrigin;
                        RequestAvail.FlightDate = strArrdate;
                        RequestAvail.FareclassOption = Class;


                        RequestAvail.FlightNo = fltnumkey.Contains(",") ? fltnumkey.Split(',')[1].ToString() : fltnumkey.ToString();
                        RequestAvail.Connection = fltsegmentkey.Contains(",") ? fltsegmentkey.Split(',')[1].ToString() : fltsegmentkey.ToString();

                        RequestAvail.FareType = Faretype;
                        RequestAvail.isStudentFare = StudentFare;
                        RequestAvail.isArmyFare = ArmyFare;
                        RequestAvail.isSnrCitizenFare = SnrcitizenFare;
                        RequestAvail.isLabourFare = Islabourfare;
                        avail.Add(RequestAvail);
                    }
                }

                string Agentcomm = (Session["Agent_CommissionRef"] != null && Session["Agent_CommissionRef"].ToString() != "" ? Session["Agent_CommissionRef"].ToString() : "");
                string AgentFSCcomm = (Session["Agent_FSCCommission"] != null && Session["Agent_FSCCommission"].ToString() != "" ? Session["Agent_FSCCommission"].ToString() : "");
                string Agentmark = (Session["Agent_MarkupRef"] != null && Session["Agent_MarkupRef"].ToString() != "" ? Session["Agent_MarkupRef"].ToString() : "");
                string Agentserviceref = (Session["Agent_ServiceRef"] != null && Session["Agent_ServiceRef"].ToString() != "" ? Session["Agent_ServiceRef"].ToString() : "");
                string Agentservicetaxref = (Session["Agent_ServiceTaxRef"] != null && Session["Agent_ServiceTaxRef"].ToString() != "" ? Session["Agent_ServiceTaxRef"].ToString() : "");
                string Agent_TDS = (Session["AGN_TDSPERCENTAGE"] != null && Session["AGN_TDSPERCENTAGE"].ToString() != "" ? Session["AGN_TDSPERCENTAGE"].ToString() : "");

                RQRS.AgentDetails agentDet = new RQRS.AgentDetails();
                agentDet.AgentId = Session["agentid"] != null && Session["agentid"] != "" ? Session["agentid"].ToString() : "";
                str_agentid = Session["Availagentid"] != null && Session["Availagentid"] != "" ? Session["Availagentid"].ToString() : "";
                agentDet.Agenttype = strTerminalType == "T" ? strClientType : strAgentType;
                agentDet.AirportID = segmenttype;
                agentDet.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                agentDet.AppType = strAppType;
                agentDet.BOAID = strClientID;
                agentDet.BOAterminalID = strClientTerminalID;
                agentDet.BranchID = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                agentDet.IssuingBranchID = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                agentDet.ClientID = strClientID;
                agentDet.CoOrdinatorID = "";
                agentDet.Environment = strTerminalType;
                agentDet.Platform = "B"; //ABCD
                agentDet.ProjectID = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"] != "" ? Session["PRODUCT_CODE"].ToString() : "";
                agentDet.TerminalId = Session["Availterminal"] != null && Session["Availterminal"] != "" ? Session["Availterminal"].ToString() : "";
                agentDet.UserName = Session["username"] != null && Session["username"] != "" ? Session["username"].ToString() : "";
                agentDet.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                agentDet.COST_CENTER = "";
                agentDet.Group_ID = GroupID != null && GroupID != "" ? GroupID : "";
                agentDet.EMP_ID = "";
                agentDet.UID = UID.Split('$').Length > 0 ? UID.Split('$')[0] : UID;
                agentDet.TID = UID.Split('$').Length > 1 ? UID.Split('$')[1] : UID;
                if (strTerminalType == "W")
                {
                    agentDet.COMMISSION_REF = (ConfigurationManager.AppSettings["Producttype"] == "RIYA" || ConfigurationManager.AppSettings["Producttype"] == "RBOA") ? (FCategory.Contains("1A") || FCategory.Contains("1S") || FCategory.Contains("1G") || FCategory.Contains("UAI")) ? AgentFSCcomm : Agentcomm : Agentcomm;
                    agentDet.MARKUP_REF = Agentmark;
                    agentDet.AGENT_SERVICE_FEE_REF = Agentserviceref;
                    agentDet.SERVICE_TAX_REF = "1";/* Value hardcoded for tax*/
                    agentDet.SERVICE_TAX = Agentservicetaxref;
                    agentDet.TDS_TAX = Agent_TDS;
                }


                #region Passengers
                /// Pax details adding in list
                RQRS.Passengers lstrPax = new RQRS.Passengers();
                lstrPax.PaxCount = (NoofAdt + NoofChd + NoofInf).ToString();

                List<RQRS.PaxTypeRefs> lstrReferencePax = new List<RQRS.PaxTypeRefs>();
                if (NoofAdt > 0)
                {
                    RQRS.PaxTypeRefs RefAdt = new RQRS.PaxTypeRefs();
                    RefAdt.Type = "ADT";
                    RefAdt.Quantity = NoofAdt.ToString();
                    lstrReferencePax.Add(RefAdt);
                }
                if (NoofChd > 0)
                {
                    RQRS.PaxTypeRefs RefChd = new RQRS.PaxTypeRefs();
                    RefChd.Type = "CHD";
                    RefChd.Quantity = NoofChd.ToString();
                    lstrReferencePax.Add(RefChd);
                }
                if (NoofInf > 0)
                {
                    RQRS.PaxTypeRefs RefInf = new RQRS.PaxTypeRefs();
                    RefInf.Type = "INF";
                    RefInf.Quantity = NoofInf.ToString();
                    lstrReferencePax.Add(RefInf);
                }
                #endregion
                lstrPax.PaxTypeRef = lstrReferencePax;

                #region FlightOption

                string airlinecode = Airline.Trim().TrimEnd(',');

                string[] lstrArra = new string[] { };
                if (Airline == "")
                { lstrArra = new string[] { }; }
                else
                {
                    if (airlinecode.Contains(","))
                    {
                        lstrArra = airlinecode.Split(',');
                    }
                    else
                    {
                        lstrArra = Airline.TrimEnd(',').Split(',');
                    }
                }
                #endregion

                #region Pricingcodefetch
                #region For WYairline
                if (FCategory == ConfigurationManager.AppSettings["Air_Thread"].ToString() && segmenttype == "I" && ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA" && strTrip != "M")
                {
                    Airline = "WY";
                    string WYOfficeId = strfromCity == "LKO" ? "LKOVS3194" : (strfromCity == "HYD" ? "HYDVS3328" : (strfromCity == "MAA" ? "MAAVS31US" : (strfromCity == "COK" ? "COKVS3234" : "")));

                    Promocodes = new RQRS.Promocodes();
                    Promocodes.CodeType = "";
                    Promocodes.Flight = "WY";
                    Promocodes.OfficeId = WYOfficeId;
                    Promocodes.PromoCode = "";
                    Promocodes.PromoCodeDesc = "";
                    Prcode.Add(Promocodes);
                    FCategory = "1A";
                    Faretype = "P";
                }
                #endregion
                else if (blnBookletFare == true && ConfigurationManager.AppSettings["BookletThread"] != null && ConfigurationManager.AppSettings["BookletThread"].ToString() != "")
                {
                    try
                    {
                        string[] strBookletPromo = ConfigurationManager.AppSettings["BookletThread"].ToString().Split('|');
                        for (int _blk = 0; _blk < strBookletPromo.Length; _blk++)
                        {
                            string[] strcode = strBookletPromo[_blk].Split('~');
                            if (strcode[0] == FCategory)
                            {
                                string[] strbkltair = strcode[1].Split(',');
                                for (int _blkair = 0; _blkair < strbkltair.Length; _blkair++)
                                {
                                    Promocodes = new RQRS.Promocodes();
                                    Promocodes.CodeType = strcode[4].ToString();
                                    Promocodes.Flight = strbkltair[_blkair].ToString();
                                    Promocodes.OfficeId = strcode[3].ToString();
                                    Promocodes.PromoCode = strcode[2].ToString();
                                    Promocodes.PromoCodeDesc = "";
                                    Prcode.Add(Promocodes);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else if (!string.IsNullOrEmpty(Promothread))
                {
                    Promocodes = new RQRS.Promocodes();
                    Promocodes.CodeType = Promothread.Split('~')[3].ToString();
                    Promocodes.Flight = Promothread.Split('~')[0].ToString();
                    Promocodes.OfficeId = Promothread.Split('~')[2].ToString();
                    Promocodes.PromoCode = Promothread.Split('~')[1].ToString();
                    Promocodes.PromoCodeDesc = "";
                    Prcode.Add(Promocodes);
                }
                else
                {
                    try
                    {

                        if (LabourFare == false)
                        {
                            //// PRICING DALIY STATIC DATA
                            DataSet dsPricingCode = new DataSet(); DataSet dstPricingCode = new DataSet(); string strDynamicName = DateTime.Now.ToString("yyMMddhh");

                            string[] Promothread_lst = new string[] { };
                            string[] Addl_Promothread_lst = new string[] { };

                            Promothread_lst = ConfigurationManager.AppSettings["Promocodethread"].ToString().Split(',');
                            Addl_Promothread_lst = ConfigurationManager.AppSettings["Addl_Promocodethread"].ToString().Split(',');

                            string Checkcategory = ConfigurationManager.AppSettings["Producttype"].ToString() == "UAE" ? FCategory.Contains("COUNT") == true ? Regex.Split(FCategory, "COUNT")[0].ToString() : FCategory : FCategory;
                            if (ConfigurationManager.AppSettings["Promocodethread"].ToString() != "" && (Array.IndexOf(Promothread_lst, Checkcategory) > -1) && strTrip != "M")
                            {
                                string clntid = (ClientID != "" && strTerminalType == "T" ? strClientID : Session["pos_id"].ToString());
                                string terminalid = (ClientID != "" && strTerminalType == "T" ? strClientTerminalID : Session["POS_TID"].ToString());
                                InplantService.Inplantservice _rays = new InplantService.Inplantservice();
                                _rays.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                                string Promoerr = string.Empty;
                                string classreq = string.Empty;
                                string treq = string.Empty;
                                string Airline_lst = string.Empty;
                                string Catcount = ConfigurationManager.AppSettings["Producttype"].ToString() == "UAE" ? FCategory.Contains("COUNT") == true ? Regex.Split(FCategory, "COUNT")[1].ToString() : "0" : "0";
                                FCategory = ConfigurationManager.AppSettings["Producttype"].ToString() == "UAE" ? FCategory.Contains("COUNT") == true ? Regex.Split(FCategory, "COUNT")[0].ToString() : FCategory : FCategory;

                                dsPricingCode = _rays.FetchPricingCodeDetails(clntid, terminalid, strOrigin, strDestination, segmenttype, strTrip, "", Class, FCategory, Catcount, Session["username"].ToString(), "W", Convert.ToDecimal(Session["sequenceid"]), Session["ipaddress"].ToString(), ref Promoerr, Airline, strDepdate, "", "");

                                string Xmldata = "<REQUEST><CLIENTID>" + clntid + "</CLIENTID><POSTID>" + terminalid + "</POSTID><ORIGIN>" + strOrigin + "</ORIGIN><DESTINATION>" + strDestination
                                        + "</DESTINATION><SEGMENTTYPE>" + segmenttype + "</SEGMENTTYPE><TRIPTYPE>" + strTrip + "</TRIPTYPE><TRAVELDATE>" + strDepdate + "</TRAVELDATE><CLASS>" + Class + "</CLASS><STOCKTYPE>" + FCategory
                                        + "</STOCKTYPE><REQCOUNT>0</REQCOUNT><USERNAME>" + Session["username"].ToString() + "</USERNAME></REQUEST><RESPONSE><DATA>" + dsPricingCode.GetXml() + "</DATA><ERROR>" + Promoerr + "</ERROR></RESPONSE>";

                                DatabaseLog.LogData(Session["username"].ToString(), "E", "Fetchpromocode", "Fetchpromocode-Flightsearch", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                if (dsPricingCode != null && dsPricingCode.Tables.Count > 0 && dsPricingCode.Tables[0].Rows.Count > 0)
                                {
                                    for (var i = 0; i < dsPricingCode.Tables[0].Rows.Count; i++)
                                    {
                                        if (Airline == "" || Airline.Contains(dsPricingCode.Tables[0].Rows[i]["TC_AIRLINE"].ToString()))
                                        {
                                            Promocodes = new RQRS.Promocodes();
                                            Promocodes.CodeType = dsPricingCode.Tables[0].Rows[i]["TC_TYPE"].ToString();
                                            Promocodes.Flight = dsPricingCode.Tables[0].Rows[i]["TC_AIRLINE"].ToString();
                                            Promocodes.OfficeId = dsPricingCode.Tables[0].Rows[i]["FTC_TICKET_OFFICE_ID"].ToString();
                                            Promocodes.PromoCode = dsPricingCode.Tables[0].Rows[i]["TC_CODE"].ToString();
                                            Promocodes.PromoCodeDesc = dsPricingCode.Tables[0].Columns.Contains("FTC_SHOW_DESC") == true ? dsPricingCode.Tables[0].Rows[i]["FTC_SHOW_DESC"].ToString() : "";
                                            Prcode.Add(Promocodes);
                                            if (Catcount != "0")
                                            {
                                                Airline_lst += dsPricingCode.Tables[0].Rows[i]["TC_AIRLINE"].ToString() + ",";
                                            }
                                        }
                                    }
                                    lstrArra = Airline_lst.TrimEnd(',').Split(',');
                                }
                            }

                            string CheckAddlcategory = ConfigurationManager.AppSettings["Producttype"].ToString() == "UAE" ? FCategory.Contains("COUNT") == true && FCategory.Contains("PR_THREAD") == true ? Regex.Split(Regex.Split(FCategory, "PR_THREAD")[0], "COUNT")[0].ToString() : FCategory.Contains("COUNT") == false && FCategory.Contains("PR_THREAD") == true ? Regex.Split(FCategory, "PR_THREAD")[0] : FCategory : Regex.Split(FCategory, "PR_THREAD")[0];

                            if ((ConfigurationManager.AppSettings["Addl_Promocodethread"].ToString() != "" && FCategory.Contains("PR_THREAD") && (Array.IndexOf(Addl_Promothread_lst, CheckAddlcategory) > -1)) && (Array.IndexOf(Promothread_lst, CheckAddlcategory) == -1) && strTrip != "M")
                            {
                                string clntid = (ClientID != "" && strTerminalType == "T" ? strClientID : Session["pos_id"].ToString());
                                string terminalid = (ClientID != "" && strTerminalType == "T" ? strClientTerminalID : Session["POS_TID"].ToString());
                                InplantService.Inplantservice _rays = new InplantService.Inplantservice();
                                _rays.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                                string Promoerr = string.Empty;
                                string classreq = string.Empty;
                                string treq = string.Empty;
                                string Airline_lst = string.Empty;

                                string catcount = ConfigurationManager.AppSettings["Producttype"].ToString() == "UAE" ? FCategory.Contains("COUNT") == true ? Regex.Split(Regex.Split(FCategory, "PR_THREAD")[0], "COUNT")[1].ToString() : "0" : "0";
                                string OfficeId = Regex.Split(FCategory, "PR_THREAD")[1];
                                FCategory = ConfigurationManager.AppSettings["Producttype"].ToString() == "UAE" ? FCategory.Contains("COUNT") == true ? Regex.Split(Regex.Split(FCategory, "PR_THREAD")[0], "COUNT")[0].ToString() : Regex.Split(FCategory, "PR_THREAD")[0] : Regex.Split(FCategory, "PR_THREAD")[0];

                                dsPricingCode = _rays.FetchPricingCodeDetails(clntid, terminalid, strOrigin, strDestination, segmenttype, strTrip, "", Class, FCategory, catcount, Session["username"].ToString(), "W", Convert.ToDecimal(Session["sequenceid"]), Session["ipaddress"].ToString(), ref Promoerr, Airline, strDepdate, "", "");

                                string Xmldata = "<REQUEST><CLIENTID>" + clntid + "</CLIENTID><POSTID>" + terminalid + "</POSTID><ORIGIN>" + strOrigin + "</ORIGIN><DESTINATION>" + strDestination
                                        + "</DESTINATION><SEGMENTTYPE>" + segmenttype + "</SEGMENTTYPE><TRIPTYPE>" + strTrip + "</TRIPTYPE><TRAVELDATE>" + strDepdate + "</TRAVELDATE><CLASS>" + Class + "</CLASS><STOCKTYPE>" + FCategory
                                        + "</STOCKTYPE><REQCOUNT>0</REQCOUNT><USERNAME>" + Session["username"].ToString() + "</USERNAME></REQUEST><RESPONSE><DATA>" + dsPricingCode.GetXml() + "</DATA><ERROR>" + Promoerr + "</ERROR></RESPONSE>";

                                DatabaseLog.LogData(Session["username"].ToString(), "E", "Fetchpromocode", "Fetchpromocode-Flightsearch", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                                if (dsPricingCode != null && dsPricingCode.Tables.Count > 0 && dsPricingCode.Tables[0].Rows.Count > 0)
                                {
                                    //string OfficeId = Regex.Split(FCategory, "PR_THREAD")[1];
                                    for (var i = 0; i < dsPricingCode.Tables[0].Rows.Count; i++)
                                    {
                                        if ((Airline == "" || Airline.Contains(dsPricingCode.Tables[0].Rows[i]["TC_AIRLINE"].ToString())) && OfficeId == dsPricingCode.Tables[0].Rows[i]["FTC_TICKET_OFFICE_ID"].ToString())
                                        {
                                            Promocodes = new RQRS.Promocodes();
                                            Promocodes.CodeType = dsPricingCode.Tables[0].Rows[i]["TC_TYPE"].ToString();
                                            Promocodes.Flight = dsPricingCode.Tables[0].Rows[i]["TC_AIRLINE"].ToString();
                                            Promocodes.OfficeId = dsPricingCode.Tables[0].Rows[i]["FTC_TICKET_OFFICE_ID"].ToString();
                                            Promocodes.PromoCode = dsPricingCode.Tables[0].Rows[i]["TC_CODE"].ToString();
                                            Promocodes.PromoCodeDesc = dsPricingCode.Tables[0].Columns.Contains("FTC_SHOW_DESC") == true ? dsPricingCode.Tables[0].Rows[i]["FTC_SHOW_DESC"].ToString() : "";
                                            Prcode.Add(Promocodes);
                                            Airline_lst += dsPricingCode.Tables[0].Rows[i]["TC_AIRLINE"].ToString() + ",";
                                        }
                                    }
                                }
                                else
                                {
                                    msg = "No flights available";
                                    return Json(new { Status = stu, Message = msg, Result = array_Response });
                                }
                                lstrArra = Airline_lst.TrimEnd(',').Split(',');
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string error = "<URL>[<![CDATA[" + ex.ToString() + "]]>]</URL>";
                        DatabaseLog.LogData(Session["username"].ToString(), "X", "SearchController", "Fetchpromocode-Flightsearch", error.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    }
                }
                #endregion

                //Prcode = new List<RQRS.Promocodes>();

                if (FCategory.Contains("PR_THREAD") == true)
                {
                    msg = "No flights available";
                    return Json(new { Status = stu, Message = msg, Result = array_Response });
                }

                // FCategory = ConfigurationManager.AppSettings["Producttype"].ToString() == "RIYA" && airlinecode.Contains("UK") && FCategory == "1S" ? "1A" : FCategory;

                string Trip_type = (Threadspl == "Y" || (strTrip == "Y" && segmenttype == "D" && strProductType == "RIYA") ? "Y" : strTrip == "R" ? "O" : (strTrip == "M" && segmenttype == "I") ? "M" : (strTrip == "Y" ? "R" : "O"));


                RQRS.Itineary itin = new RQRS.Itineary()
                {
                    Agent = agentDet,
                    AvailabilityRequest = avail,
                    Category = (FCategory.Length > 2 && Session["CORPORATETHREAD"].ToString().Contains(FCategory) == true) ? FCategory.Substring(0, 2) : FCategory, //"RIYA",
                    Currencycode = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString(), //"INR",
                    //TripType = strTrip == "R" ? "O" : (strTrip == "M" && segmenttype == "I") ? "M" : (strTrip == "Y" ? "R" : "O"),//strTrip, //For RoundTrip Special TripType ll go as "Y" otherwise "O"  ... by sarnarj on 20170523...
                    TripType = Trip_type,
                    SegmentType = segmenttype,
                    Passenger = lstrPax,
                    HostSearch = Convert.ToBoolean(Hostsearch),
                    Stock = "RST_" + availsetcount + "~" + Airline,
                    FlightOption = lstrArra,
                    Commission = "Y",
                    DiscountFlag = "N",
                    PromoCodes = Prcode,
                    //IsBookletFare = blnBookletFare,
                };

                #region Sending Request to server..............
                try
                {
                    string memCacheKey = string.Empty;
                    string request = JsonConvert.SerializeObject(itin).ToString();
                    string Query = "InvokeAvailability";
                    string LstrDetails = string.Empty;

                    int Availtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);

                    if (ConfigurationManager.AppSettings["Splavailagent"].ToString() != "" && ConfigurationManager.AppSettings["Splavailagent"].ToString().Contains(Session["POS_TID"].ToString()))
                    {
                        strURLpathA = ConfigurationManager.AppSettings["Spl_APPS_URL"].ToString();
                    }
                    else
                    {
                        strURLpathA = ConfigurationManager.AppSettings["APPS_URL"].ToString();
                    }

                    #region Log


                    if (Base.AvailLog)
                    {
                        ReqTime = "AvailabityReqest" + DateTime.Now;

                        LstrDetails = "<THREAD_REQUEST><URL>[<![CDATA[" + strURLpathA + "]]>]</URL><QUERY>" + Query + "</QUERY><CATEGORY>"
                              + itin.TripType + " ~ " + itin.Category + " ~ " +
                              ((string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) +
                              (string.IsNullOrEmpty(strDestination) ? "" : strDestination)) +
                              "</CATEGORY><JSON>" + request + "</JSON><TIME>" + ReqTime + "</TIME>";

                        StringWriter strREQUEST = new StringWriter();
                        if (Base.ReqLog)
                        {
                            DataSet dsrequest = new DataSet();
                            dsrequest = Serv.convertJsonStringToDataSet(request, "");
                            if (dsrequest != null)
                                dsrequest.WriteXml(strREQUEST);
                        }

                        DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "InternationalFlights",
                            ((itin.TripType + " ~TRQ~" + itin.Category + " ~ " +
                            (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) +
                            (string.IsNullOrEmpty(strDestination) ? "" : strDestination))
                            + ":THREAD REQUEST" + (Airline == null ? "" : " -FLIGHTOPTION :" + Airline)),
                            LstrDetails + ((Base.ReqLog) ? strREQUEST.ToString() : "") + "</THREAD_REQUEST>", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                        if (roundtripflg == "1")
                        {

                            DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "InternationalFlights",
                                ((itin.TripType + "R" + " ~TRQ~" + itin.Category + " ~ " +
                                (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) +
                                (string.IsNullOrEmpty(strDestination) ? "" : strDestination))
                                + ":THREAD REQUEST" + (Airline == null ? "" : " -FLIGHTOPTION :" + Airline)),
                                LstrDetails + ((Base.ReqLog) ? strREQUEST.ToString() : "") + "</THREAD_REQUEST>", Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        }

                    }



                    #endregion

                    strResponse = await AsyncFlightAvail(itin, strURLpathA + Query);
                    if (strProductType == "RBOA" && strTerminalType == "T")
                    {
                        string qrystringval = Session["SelectClient"] != null ? Session["SelectClient"].ToString() : "";
                        string Enqids = Session["Enqidsameera"] != null ? Session["Enqidsameera"].ToString() : "";
                        string CallID = Session["callidsameera"] != null ? Session["callidsameera"].ToString() : "";

                        _CRMDATA.ENQUIRY_ID = Enqids;
                        _CRMDATA.PAX_MOBILE_NO = Session["Mobileno"] != null ? Session["Mobileno"].ToString() : "";
                        _CRMDATA.PAX_EMAIL_ID = "";
                        _CRMDATA.CLIENT_CODE = ClientID != null && ClientID != "" ? ClientID : Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                        _CRMDATA.COORDINATOR_ID = "";
                        _CRMDATA.PRODUCT = "AIR";
                        _CRMDATA.AIRPORT_TYPE = "I";
                        _CRMDATA.CALL_TYPE = "Open";
                        _CRMDATA.Orgion = strfromCity;
                        _CRMDATA.Desti = strtoCity;
                        _CRMDATA.CALL_FROMDATE = strDepartureDate;
                        if (strTrip == "O")
                        {
                            _CRMDATA.CALL_TODATE = strDepartureDate;
                        }
                        else
                        {
                            _CRMDATA.CALL_TODATE = strRetDate;
                        }

                        _CRMDATA.REMAINDER_DATE = null;
                        _CRMDATA.CALL_CATEGORY = "1";
                        _CRMDATA.REMARKS = "";
                        _CRMDATA.BranchId = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                        _CRMDATA.Username = Session["UserName"] != null ? Session["UserName"].ToString() : "";
                        _CRMDATA.ADTcnt = strAdults;
                        _CRMDATA.CHDcnt = strChildrens;
                        _CRMDATA.INFcnt = strInfants;
                        _CRMDATA.triptype = strTrip;
                        _CRMDATA.tripClass = Class;

                        if (qrystringval == "")
                        {
                            _CRMDATA.SaveDetails = "SYS";
                        }
                        else
                        {
                            _CRMDATA.SaveDetails = "USR";
                        }

                    }


                    if (string.IsNullOrEmpty(strResponse))
                    {
                        #region log
                        string lstrtime = "AvailabityResponse" + DateTime.Now;
                        LstrDetails = "<THREAD_RESPONSE><THREAD>" + (itin.TripType + " ~ " + itin.Category + " ~ " +
                            (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) +
                            (string.IsNullOrEmpty(strDestination) ? "" : strDestination)) + "</THREAD><REqTime>" + ReqTime + "</REqTime><ResTIME>" + lstrtime + "</ResTIME>" +
                            "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" + "Null Or Empty" + "</THREAD_RESPONSE>";

                        DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "InternationalFlights", (itin.TripType + " ~TRSN~" + itin.Category + " ~ " +
                            (string.IsNullOrEmpty(strOrigin) ? "" : strOrigin) + (string.IsNullOrEmpty(strDestination) ? "" :
                            strDestination)) + " :THREAD RESPONSE NULL", LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        #endregion
                        array_Response[Error] = "Problem occured while Searching Flight";
                        array_Response[concept_groupfare] = ""; //array_Response[AvailSession] + "sri" + array_Response[Error] + "sri" + array_Response[FlightSearchError] + "sri" + array_Response[JsonResponse] + "sri" + array_Response[threadnoresponse] + "sri" + array_Response[availsideflat] + "sri" + array_Response[memcachechck];

                        return Json(new { Status = "", Message = "", Result = array_Response });
                    }

                    afterAvail:
                    _availRes = JsonConvert.DeserializeObject<RQRS.AvailabilityRS>(strResponse);
                    if (_availRes.ResultCode != null && _availRes.ResultCode == "1" && (memCache.Get(memCacheKey) == null || memCache.Get(memCacheKey) == ""))
                    {
                        memCache.Add(memCacheKey, strResponse, DateTimeOffset.UtcNow.AddMinutes(totcachemin));
                    }

                    if (_availRes.ResultCode != null)
                    {
                        if (_availRes.ResultCode == "1")
                        {
                            #region Log
                            string lstrtime = "AvailabityResponse" + DateTime.Now;
                            StringWriter strres = new StringWriter();
                            if (Base.ReqLog)
                            {
                                DataSet dsresponse = new DataSet();
                                dsresponse = Serv.convertJsonStringToDataSet(strResponse, "");
                                if (dsresponse != null)
                                    dsresponse.WriteXml(strres);
                            }
                            Base.ReqLog = false;
                            LstrDetails = "<THREAD_RESPONSE><THREAD>" + (itin.Category + " ~TRS~" +
                              (string.IsNullOrEmpty(strOrigin) ? "" : strOrigin) +
                              (string.IsNullOrEmpty(strDestination) ? "" : strDestination)) + "</THREAD><REqTime>" + ReqTime + "</REqTime><ResTIME>" +
                              lstrtime + "</ResTIME>" + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                           "<Flights>" + (_availRes == null || _availRes.Flights == null ? "NULL" : _availRes.Flights.Count.ToString()) + "</Flights>" +
                           "<Fares>" + (_availRes == null || _availRes.Fares == null ? "NULL" : _availRes.Fares.Count.ToString()) + "</Fares>" +
                            "<Stock>" + (_availRes == null || _availRes.Stock == null ? "NULL" : _availRes.Stock.ToString()) + "</Stock>" +
                           "<Bagg>" + (_availRes == null || _availRes.Bagg == null ? "NULL" : _availRes.Bagg.Count.ToString()) + "</Bagg>" +
                           "<Meal>" + (_availRes == null || _availRes.Meal == null ? "NULL" : _availRes.Meal.Count.ToString()) + "</Meal>" +
                         ((Base.ReqLog) ? strres.ToString() : "") + "</THREAD_RESPONSE>";

                            DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "InternationalFlights", (itin.TripType + " ~TRS~ " + itin.Category + " ~ " +
                                (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) + (string.IsNullOrEmpty(strDestination) ? "" :
                                strDestination)) + " :THREAD RESPONSE SUCCESS", LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                            #endregion
                            string strairlinenamejson = string.Empty;
                            DataSet dsAirways = new DataSet();
                            if (Session["dsAirportlst"] == null || Session["dsAirportlst"].ToString() == "")
                            {
                                string strPathcityall = Server.MapPath("~/XML/CityAirport_Lst.xml");
                                dsAirways.ReadXml(strPathcityall);
                                strairlinenamejson = JsonConvert.SerializeObject(dsAirways);
                                Session.Add("dsAirportlst", strairlinenamejson);
                            }
                            else
                            {
                                strairlinenamejson = Session["dsAirportlst"].ToString();
                                dsAirways = JsonConvert.DeserializeObject<DataSet>(strairlinenamejson);
                            }

                            if (Threadspl != "Y")
                            {

                                string strAvailSession = "AVAILFLIGHTSINT_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                string[] strKeySession = strAvailSession.Split('_');
                                strAvailSession_arg = strAvailSession;
                                stu = "1";
                                msg = "";

                                var FlightsDatatemp = from _Flights in _availRes.Flights.AsEnumerable()
                                                      join _Fares in _availRes.Fares.AsEnumerable()
                                                      on _Flights.FareId equals _Fares.FlightId
                                                      from _FareData in _Fares.Faredescription.Where(e => e.Paxtype.Equals("ADT")).AsEnumerable()
                                                      select new
                                                      {
                                                          FC = findflightcount(_availRes.Flights, _Flights.FareId, (strTrip == "Y" ? "2" : "1")),
                                                          Org = _Flights.Origin,
                                                          Des = _Flights.Destination,
                                                          Dep = _Flights.DepartureDateTime,
                                                          Arr = _Flights.ArrivalDateTime,
                                                          Dur = _Flights.JourneyTime,
                                                          Cls = _Flights.Class.Trim() + '-' + _Flights.FareBasisCode.Trim(),
                                                          Cab = _Flights.Cabin.TrimEnd(','),
                                                          BFare = _FareData.BaseAmount,
                                                          GFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) +
                                                          Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ?
                                                          _FareData.ClientMarkup : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ?
                                                          _FareData.Servicecharge : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ?
                                                          _FareData.SFAMOUNT : "0")
                                                            + Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ?
                                                          _FareData.SFGST : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ?
                                                          _FareData.ServiceFee : "0")).ToString(),

                                                          WTMFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount)
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ?
                                                          _FareData.Servicecharge : "0")).ToString("0"),


                                                          WTSCANDNETFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount)
                                                           + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ?
                                                           _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null &&
                                                           _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0") + Base.ServiceUtility.CovertToDouble((
                                                           _FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ? _FareData.SFAMOUNT : "0") +
                                                           Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ? _FareData.SFGST : "0")
                                                           + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ?
                                                          _FareData.ServiceFee : "0")).ToString(),

                                                          NETFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount)
                                                           + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ?
                                                           _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ?
                                                          _FareData.ServiceFee : "0") + Base.ServiceUtility.CovertToDouble((_FareData.ServiceTax != null && _FareData.ServiceTax != "") ?
                                                          _FareData.ServiceTax : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ?
                                                          _FareData.SFAMOUNT : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ?
                                                          _FareData.SFGST : "0") + (ConfigurationManager.AppSettings["AVAILFORMAT"].ToString() == "NAT" ? Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ?
                                                          _FareData.Servicecharge : "0") : 0) -
                                                           Base.ServiceUtility.CovertToDouble((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString()))).ToString(),

                                                          Con = _Flights.CNX,
                                                          Fno = _Flights.FlightNumber.TrimEnd(),
                                                          Ref = _Flights.ReferenceToken,

                                                          //Conurl = imgurl + _Flights.FlightNumber.Substring(0, 2) + ".png?" + ConfigurationManager.AppSettings["flightimageversion"].ToString(),
                                                          //ur = imgurl + _Flights.PlatingCarrier + ".png?" + ConfigurationManager.AppSettings["flightimageversion"].ToString(),

                                                          Inva = _Flights.Origin + "SpLitPResna" + _Flights.Destination + "SpLitPResna" + _Flights.DepartureDateTime + "SpLitPResna" + _Flights.ArrivalDateTime + "SpLitPResna" + _Flights.JourneyTime + "SpLitPResna" + _Flights.Class.Trim() + "SpLitPResna" + strAdults + "SpLitPResna" + strChildrens + //upto index 7
                                                          "SpLitPResna" + strInfants + "SpLitPResna" + _Flights.PlatingCarrier + "SpLitPResna" + _Flights.RBDCode + "SpLitPResna" + _Flights.TransactionFlag + "SpLitPResna" + _Flights.ReferenceToken + "SpLitPResna" + _FareData.BaseAmount + "SpLitPResna" + _FareData.GrossAmount + //upto index 14
                                                          "SpLitPResna" + _Flights.ConnectionFlag + "SpLitPResna" + _Flights.AirlineCategory + "SpLitPResna" + _Flights.FareBasisCode + "SpLitPResna" + _Flights.FlightNumber + "SpLitPResna" + _Flights.ItinRef + "SpLitPResna" + _Flights.CNX + "SpLitPResna" + _Flights.Cabin.TrimEnd(',') + //upto index 21
                                                          "SpLitPResna" + _Flights.FlyingTime + "SpLitPResna" + _availRes.FareCheck.Stocktype + "SpLitPResna" + _Flights.SegmentDetails + "SpLitPResna" + _Flights.FareId + "SpLitPResna" + _FareData.Servicecharge + "SpLitPResna" + _FareData.Incentive + "SpLitPResna" + _FareData.ClientMarkup + "SpLitPResna" + _Flights.SegRef
                                                          + "SpLitPResna" + _Flights.AvailSeat + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ? _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ? _FareData.ServiceFee : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ? _FareData.SFAMOUNT : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ? _FareData.SFGST : "0")).ToString() //+ Base.ServiceUtility.CovertToDouble((_FareData.ServiceTax != null && _FareData.ServiceTax != "") ? _FareData.ServiceTax : "0")
                                                          + "SpLitPResna" + _Flights.Refundable.ToUpper() + "SpLitPResna" + _Flights.Via + "SpLitPResna" + _Flights.FareTypeDescription + "SpLitPResna" + _Fares.FareType + "SpLitPResna" + _Flights.OperatingCarrier + "SpLitPResna" + _Flights.PromoCodeDesc + "SpLitPResna" + _Flights.PromoCode,

                                                          tax = _FareData.Taxes,
                                                          acat = _Flights.AirlineCategory + "SpLitPResna" + _Flights.Origin + "SpLitPResna" + _Flights.Destination + "SpLitPResna" + _Flights.DepartureDateTime + "SpLitPResna" + _Flights.ReferenceToken + "SpLitPResna" + _Flights.PlatingCarrier + "SpLitPResna" + _availRes.FareCheck.Stocktype,

                                                          nonstop = _Flights.Via,
                                                          com = ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")) - Convert.ToDecimal(_FareData.ServiceTax ?? "0")).ToString()),
                                                          fly = _Flights.FlyingTime,
                                                          seg = _Flights.SegmentDetails,
                                                          Bagg = aircraft(_availRes.Flights, _availRes.Flights.IndexOf(_Flights)),
                                                          plt = _Flights.PlatingCarrier,
                                                          offFlag = _Flights.OfflineFlag,
                                                          Stops = _Flights.Stops,
                                                          Seats = _Flights.AvailSeat,
                                                          iti = _Flights.ItinRef,
                                                          multiclass = _Flights.MultiClass,
                                                          flightid = _Fares.FlightId,
                                                          StkType = _availRes.FareCheck.Stocktype,
                                                          Service = _FareData.Servicecharge,
                                                          Markup = _FareData.ClientMarkup,
                                                          Refund = _Flights.Refundable.ToUpper(),
                                                          connflightid = _Flights.ConnectionFlag,
                                                          bestbuy = _FareData.BestBuyOption.ToString().ToUpper(), //TRUE - for Special fare button enable...
                                                          taxbreakupold = string.Join("|", _FareData.Taxes.Select(t => t.Code + ":" + t.Amount).ToArray()) + (_FareData.Servicecharge != "0" ? "|Serv.Chrg:" + _FareData.Servicecharge : ""),
                                                          mclassresult = ((_Flights.MultiClass != null && _Flights.MultiClass != "" && _Flights.MultiClass != "0") ? _Flights.FlightNumber + "SpLitPResna" + _Fares.FlightId + "SpLitPResna" + _Flights.Class.Trim() + "SpLitPResna" + _Flights.ReferenceToken + "SpLitPResna" + _FareData.BaseAmount + "SpLitPResna" + Base.ServiceUtility.CovertToDouble((_FareData.BaseAmount != null && _FareData.BaseAmount != "") ? _FareData.BaseAmount : "0") + "\nComm : " +
                                                          ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString())) + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.TransactionFee != null && _FareData.TransactionFee != "") ? _FareData.TransactionFee : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ? _FareData.SFGST : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ? _FareData.SFAMOUNT : "0")).ToString("0") + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ? _FareData.ClientMarkup : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ? _FareData.SFAMOUNT : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ? _FareData.SFGST : "0") + Base.ServiceUtility.CovertToDouble((_FareData.TransactionFee != null && _FareData.TransactionFee != "") ? _FareData.TransactionFee : "0")).ToString("0") + "SpLitPResna" + _FareData.Taxes + "SpLitPResna" + _FareData.ClientMarkup + "SpLitPResna" + _FareData.ServiceTax + "SpLitPResna" + _FareData.TDS + "SpLitPResna" + _FareData.Servicecharge + //
                                                                        "SpLitPResna" + _FareData.Paxtype + "SpLitPResna" +
                                                                        ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString())) + "SpLitPResna" +
                                                                ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString())) + "SpLitPResna" + "0" + "SpLitPResna" + string.Join("/", _FareData.Taxes.Select(t => t.Code + ":" + t.Amount).ToArray()) + "SpLitPResna" + _Flights.Baggage + "SpLitPResna" + _Flights.FareBasisCode + "SpLitPResna" + "N/A" + "SpLitPResna" + _FareData.GrossAmount + "SpLitPResna" + _FareData.SFAMOUNT + "SpLitPResna" + _FareData.SFGST + "SpLitPResna" + _FareData.ServiceFee : ""),
                                                          //"SpLitPResna" + _FareData.Paxtype + "SpLitPResna" + _FareData.Commission + "SpLitPResna" + _FareData.Discount + "SpLitPResna" + _FareData.Incentive + "SpLitPResna" + string.Join("/", _FareData.Taxes.Select(t => t.Code + ":" + t.Amount).ToArray()) + "SpLitPResna" + _Flights.Baggage + "SpLitPResna" + _Flights.FareBasisCode + "SpLitPResna" + (string.IsNullOrEmpty(_Flights.Otherbenfit) ? "N/A" : _Flights.Otherbenfit) + "SpLitPResna" + _FareData.GrossAmount,
                                                          mclassinva = ((_Flights.MultiClass != null && _Flights.MultiClass != "" && _Flights.MultiClass != "0") ? _Flights.Origin + "SpLitPResna" + _Flights.Destination + "SpLitPResna" + _Flights.DepartureDateTime + "SpLitPResna" + _Flights.ArrivalDateTime + "SpLitPResna" + _Flights.JourneyTime + "SpLitPResnaclasssSpLitPResna" + strAdults + "SpLitPResna" + strChildrens +
                                                                       "SpLitPResna" + strInfants + "SpLitPResna" + _Flights.PlatingCarrier + "SpLitPResnaRBDSpLitPResna" + false + "SpLitPResnaTokenSpLitPResnaBaseAmntSpLitPResnaGrossAmntSpLitPResna" + _Flights.ConnectionFlag +
                                                                       "SpLitPResna" + _Flights.AirlineCategory + "SpLitPResnaFareBasisCodeSpLitPResna" + _Flights.FlightNumber + "SpLitPResna" + _Flights.ItinRef + "SpLitPResna" + _Flights.CNX + "SpLitPResna" + _Flights.Cabin.TrimEnd(',') +
                                                                       "SpLitPResna" + _Flights.FlyingTime + "SpLitPResna" + _availRes.FareCheck.Stocktype + "SpLitPResna" + _Flights.SegmentDetails + "SpLitPResnaSTUfareID" + "SpLitPResnaSTUservcharg" + "SpLitPResnaSTUincentive" + "SpLitPResnaSTUmarkup" + "SpLitPResnaSTUsegref" + "SpLitPResna" + _Flights.AvailSeat + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ? _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0")
                                                             + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ? _FareData.ServiceFee : "0")).ToString() + "SpLitPResna" + _Flights.Refundable.ToUpper() + "SpLitPResna" + _Flights.Via + "SpLitPResna" + _Flights.FareTypeDescription + "SpLitPResna" + _Fares.FareType + "SpLitPResna" + _Flights.OperatingCarrier + "SpLitPResna" + _Flights.PromoCodeDesc + "SpLitPResna" + _Flights.PromoCode : ""),

                                                          SinglePNR = strTrip + "~" + _Fares.FareType + "~" + _availRes.FareCheck.Stocktype + "~" + _Flights.FareTypeDescription,   //hide by srinath_28_10_check
                                                          availsr_id = "",
                                                          bindside_6_arg = availsetcount,
                                                          AF = _availRes.CacheAvail,
                                                          availbilitysession_1_arg = strAvailSession_arg,
                                                          FareType = _Flights.FareTypeDescription,
                                                          AIF = _Flights.AvailInfo, //Availablity Information
                                                          MFR = _Flights.FareRuleInfo, //Min Fare Rule
                                                          Servfee = _FareData.ServiceFee,
                                                          SFtax = _FareData.SFAMOUNT,
                                                          SFGST = _FareData.SFGST,
                                                          Opt_carrier = _Flights.OperatingCarrier,
                                                          Availagent = array_Response[available_agent],
                                                          PromocodeDesc = _Flights.PromoCodeDesc
                                                      };



                                string lstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                string[] FLIGHTID = (from sathees in FlightsDatatemp.AsEnumerable()
                                                     where (sathees.Seats.Trim() != "" && lstr.Contains(sathees.Seats))
                                                     select sathees.flightid).ToArray();

                                string lstid = string.Join(",", FLIGHTID);

                                var FlightsData = (from sathees in FlightsDatatemp.AsEnumerable()
                                                   where (!lstid.Contains(sathees.flightid))
                                                   select sathees);

                                array_Response[JsonResponse] = JsonConvert.SerializeObject(FlightsData);


                                if (DirectSearch != null && DirectSearch != "")
                                {
                                    if (Convert.ToBoolean(DirectSearch))
                                    {
                                        var qryflights = (from q in FlightsData.AsEnumerable()
                                                          where (q.FC == "0")
                                                          select q);
                                        array_Response[JsonResponse] = JsonConvert.SerializeObject(qryflights);
                                    }

                                    Session.Add("DirEctFlightSearch", DirectSearch);
                                }
                                else
                                {
                                    Session.Add("DirEctFlightSearch", false);
                                }

                                Session.Add("Triptyeper", (strTrip == "Y" ? "2" : "1")); //coz for Roundtrip Special only we have to minus 2 otherwise minus 1...  

                                Session["ValueKey"] = null;
                                Session["ValueKey"] = strKeySession[1].ToString().Trim();
                                string TCnt = strAdults + "," + strChildrens + "," + strInfants;
                                Session.Add("TravellerCount" + strKeySession[1].ToString().Trim(), TCnt);

                                Session["strAvailSession"] = null;
                                Session[strAvailSession] = _availRes;
                                array_Response[AvailSession] = strAvailSession;
                                stu = "1";
                                msg = "";
                                dictThreadRes = new Dictionary<string, string>();
                                dictThreadRes.Add("RST_" + strTrip + "_" + "", strResponse);//strResponse

                            }
                            #region ForRoundtripspl LCC avail
                            else
                            {

                                #region For onward ItinRef 0

                                string strAvailSession = "AVAILFLIGHTSINT_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                string[] strKeySession = strAvailSession.Split('_');
                                strAvailSession_arg = strAvailSession;
                                stu = "1";
                                msg = "";

                                var FlightsDatatemp = from _Flights in _availRes.Flights.AsEnumerable()
                                                      join _Fares in _availRes.Fares.AsEnumerable()
                                                      on _Flights.FareId equals _Fares.FlightId
                                                      from _FareData in _Fares.Faredescription.Where(e => e.Paxtype.Equals("ADT")).AsEnumerable()
                                                      where _Flights.ItinRef == "0"
                                                      select new
                                                      {
                                                          FC = findflightcount(_availRes.Flights, _Flights.FareId, (strTrip == "Y" ? "2" : "1")),
                                                          Org = _Flights.Origin,
                                                          Des = _Flights.Destination,
                                                          Dep = _Flights.DepartureDateTime,
                                                          Arr = _Flights.ArrivalDateTime,
                                                          Dur = _Flights.JourneyTime,
                                                          Cls = _Flights.Class.Trim() + '-' + _Flights.FareBasisCode.Trim(),
                                                          Cab = _Flights.Cabin.TrimEnd(','),
                                                          BFare = _FareData.BaseAmount,
                                                          GFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) +
                                                          Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ?
                                                          _FareData.ClientMarkup : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ?
                                                          _FareData.Servicecharge : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ?
                                                          _FareData.SFAMOUNT : "0")
                                                            + Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ?
                                                          _FareData.SFGST : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ?
                                                          _FareData.ServiceFee : "0")).ToString(),

                                                          WTMFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount)
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ?
                                                          _FareData.Servicecharge : "0")).ToString("0"),


                                                          WTSCANDNETFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount)
                                                           + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ?
                                                           _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null &&
                                                           _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0") + Base.ServiceUtility.CovertToDouble((
                                                           _FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ? _FareData.SFAMOUNT : "0") +
                                                           Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ? _FareData.SFGST : "0")
                                                           + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ?
                                                          _FareData.ServiceFee : "0")).ToString(),

                                                          NETFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount)
                                                           + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ?
                                                           _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ?
                                                          _FareData.ServiceFee : "0") + Base.ServiceUtility.CovertToDouble((_FareData.ServiceTax != null && _FareData.ServiceTax != "") ?
                                                          _FareData.ServiceTax : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ?
                                                          _FareData.SFAMOUNT : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ?
                                                          _FareData.SFGST : "0") + (ConfigurationManager.AppSettings["AVAILFORMAT"].ToString() == "NAT" ? Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ?
                                                           _FareData.Servicecharge : "0") : 0) -
                                                           Base.ServiceUtility.CovertToDouble((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString()))).ToString(),

                                                          Con = _Flights.CNX,
                                                          Fno = _Flights.FlightNumber.TrimEnd(),
                                                          Ref = _Flights.ReferenceToken,
                                                          Inva = _Flights.Origin + "SpLitPResna" + _Flights.Destination + "SpLitPResna" + _Flights.DepartureDateTime + "SpLitPResna" + _Flights.ArrivalDateTime + "SpLitPResna" + _Flights.JourneyTime + "SpLitPResna" + _Flights.Class.Trim() + "SpLitPResna" + strAdults + "SpLitPResna" + strChildrens + //upto index 7
                                                          "SpLitPResna" + strInfants + "SpLitPResna" + _Flights.PlatingCarrier + "SpLitPResna" + _Flights.RBDCode + "SpLitPResna" + _Flights.TransactionFlag + "SpLitPResna" + _Flights.ReferenceToken + "SpLitPResna" + _FareData.BaseAmount + "SpLitPResna" + _FareData.GrossAmount + //upto index 14
                                                          "SpLitPResna" + _Flights.ConnectionFlag + "SpLitPResna" + _Flights.AirlineCategory + "SpLitPResna" + _Flights.FareBasisCode + "SpLitPResna" + _Flights.FlightNumber + "SpLitPResna" + _Flights.ItinRef + "SpLitPResna" + _Flights.CNX + "SpLitPResna" + _Flights.Cabin.TrimEnd(',') + //upto index 21
                                                          "SpLitPResna" + _Flights.JourneyTime + "SpLitPResna" + _availRes.FareCheck.Stocktype + "SpLitPResna" + _Flights.SegmentDetails + "SpLitPResna" + _Flights.FareId + "SpLitPResna" + _FareData.Servicecharge + "SpLitPResna" + _FareData.Incentive + "SpLitPResna" + _FareData.ClientMarkup + "SpLitPResna" + _Flights.SegRef
                                                          + "SpLitPResna" + _Flights.AvailSeat + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ? _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ? _FareData.ServiceFee : "0")).ToString() + "SpLitPResna" + _Flights.Refundable.ToUpper() + "SpLitPResna" + _Flights.Via + "SpLitPResna" + _Flights.FareTypeDescription + "SpLitPResna" + _Fares.FareType + "SpLitPResna" + _Flights.OperatingCarrier + "SpLitPResna" + _Flights.PromoCodeDesc + "SpLitPResna" + _Flights.PromoCode,

                                                          tax = _FareData.Taxes,
                                                          acat = _Flights.AirlineCategory + "SpLitPResna" + _Flights.Origin + "SpLitPResna" + _Flights.Destination + "SpLitPResna" + _Flights.DepartureDateTime + "SpLitPResna" + _Flights.ReferenceToken + "SpLitPResna" + _Flights.PlatingCarrier + "SpLitPResna" + _availRes.FareCheck.Stocktype,

                                                          nonstop = _Flights.Via,
                                                          com = ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")) - Convert.ToDecimal(_FareData.ServiceTax ?? "0")).ToString()),
                                                          fly = _Flights.FlyingTime,
                                                          seg = _Flights.SegmentDetails,
                                                          Bagg = aircraft(_availRes.Flights, _availRes.Flights.IndexOf(_Flights)),
                                                          plt = _Flights.PlatingCarrier,
                                                          offFlag = _Flights.OfflineFlag,
                                                          Stops = _Flights.Stops,
                                                          Seats = _Flights.AvailSeat,
                                                          iti = _Flights.ItinRef,
                                                          multiclass = _Flights.MultiClass,
                                                          flightid = _Fares.FlightId,
                                                          StkType = _availRes.FareCheck.Stocktype,
                                                          Service = _FareData.Servicecharge,
                                                          Markup = _FareData.ClientMarkup,
                                                          Refund = _Flights.Refundable.ToUpper(),
                                                          connflightid = _Flights.ConnectionFlag,
                                                          bestbuy = _FareData.BestBuyOption.ToString().ToUpper(), //TRUE - for Special fare button enable...
                                                          taxbreakupold = string.Join("|", _FareData.Taxes.Select(t => t.Code + ":" + t.Amount).ToArray()) + (_FareData.Servicecharge != "0" ? "|Serv.Chrg:" + _FareData.Servicecharge : ""),
                                                          mclassresult = ((_Flights.MultiClass != null && _Flights.MultiClass != "" && _Flights.MultiClass != "0") ? _Flights.FlightNumber + "SpLitPResna" + _Fares.FlightId + "SpLitPResna" + _Flights.Class.Trim() + "SpLitPResna" + _Flights.ReferenceToken + "SpLitPResna" + _FareData.BaseAmount + "SpLitPResna" + Base.ServiceUtility.CovertToDouble((_FareData.BaseAmount != null && _FareData.BaseAmount != "") ? _FareData.BaseAmount : "0") + "\nComm : " +
                                                          ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString())) + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.TransactionFee != null && _FareData.TransactionFee != "") ? _FareData.TransactionFee : "0")).ToString("0") + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ? _FareData.ClientMarkup : "0")
                                                          + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0") + Base.ServiceUtility.CovertToDouble((_FareData.TransactionFee != null && _FareData.TransactionFee != "") ? _FareData.TransactionFee : "0")).ToString("0") + "SpLitPResna" + _FareData.Taxes + "SpLitPResna" + _FareData.ClientMarkup + "SpLitPResna" + _FareData.ServiceTax + "SpLitPResna" + _FareData.TDS + "SpLitPResna" + _FareData.Servicecharge + //
                                                                        "SpLitPResna" + _FareData.Paxtype + "SpLitPResna" +
                                                                        ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString())) + "SpLitPResna" +
                                                                ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString())) + "SpLitPResna" + "0" + "SpLitPResna" + string.Join("/", _FareData.Taxes.Select(t => t.Code + ":" + t.Amount).ToArray()) + "SpLitPResna" + _Flights.Baggage + "SpLitPResna" + _Flights.FareBasisCode + "SpLitPResna" + "N/A" + "SpLitPResna" + _FareData.GrossAmount + "SpLitPResna" + _FareData.SFAMOUNT + "SpLitPResna" + _FareData.SFGST + "SpLitPResna" + _FareData.ServiceFee : ""),
                                                          //"SpLitPResna" + _FareData.Paxtype + "SpLitPResna" + _FareData.Commission + "SpLitPResna" + _FareData.Discount + "SpLitPResna" + _FareData.Incentive + "SpLitPResna" + string.Join("/", _FareData.Taxes.Select(t => t.Code + ":" + t.Amount).ToArray()) + "SpLitPResna" + _Flights.Baggage + "SpLitPResna" + _Flights.FareBasisCode + "SpLitPResna" + (string.IsNullOrEmpty(_Flights.Otherbenfit) ? "N/A" : _Flights.Otherbenfit) + "SpLitPResna" + _FareData.GrossAmount,
                                                          mclassinva = ((_Flights.MultiClass != null && _Flights.MultiClass != "" && _Flights.MultiClass != "0") ? _Flights.Origin + "SpLitPResna" + _Flights.Destination + "SpLitPResna" + _Flights.DepartureDateTime + "SpLitPResna" + _Flights.ArrivalDateTime + "SpLitPResna" + _Flights.JourneyTime + "SpLitPResnaclasssSpLitPResna" + strAdults + "SpLitPResna" + strChildrens +
                                                                       "SpLitPResna" + strInfants + "SpLitPResna" + _Flights.PlatingCarrier + "SpLitPResnaRBDSpLitPResna" + false + "SpLitPResnaTokenSpLitPResnaBaseAmntSpLitPResnaGrossAmntSpLitPResna" + _Flights.ConnectionFlag +
                                                                       "SpLitPResna" + _Flights.AirlineCategory + "SpLitPResnaFareBasisCodeSpLitPResna" + _Flights.FlightNumber + "SpLitPResna" + _Flights.ItinRef + "SpLitPResna" + _Flights.CNX + "SpLitPResna" + _Flights.Cabin.TrimEnd(',') +
                                                                       "SpLitPResna" + _Flights.JourneyTime + "SpLitPResna" + _availRes.FareCheck.Stocktype + "SpLitPResna" + _Flights.SegmentDetails + "SpLitPResnaSTUfareID" + "SpLitPResnaSTUservcharg" + "SpLitPResnaSTUincentive" + "SpLitPResnaSTUmarkup" + "SpLitPResnaSTUsegref" + "SpLitPResna" + _Flights.AvailSeat + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ? _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0")
                                                             + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ? _FareData.ServiceFee : "0")).ToString() + "SpLitPResna" + _Flights.Refundable.ToUpper() + "SpLitPResna" + _Flights.Via + "SpLitPResna" + _Flights.FareTypeDescription + "SpLitPResna" + _Fares.FareType + "SpLitPResna" + _Flights.OperatingCarrier + "SpLitPResna" + _Flights.PromoCodeDesc + "SpLitPResna" + _Flights.PromoCode : ""),

                                                          SinglePNR = strTrip + "~" + _Fares.FareType + "~" + _availRes.FareCheck.Stocktype + "~" + _Flights.FareTypeDescription,   //hide by srinath_28_10_check
                                                          availsr_id = "",
                                                          bindside_6_arg = "1",
                                                          AF = _availRes.CacheAvail,
                                                          availbilitysession_1_arg = strAvailSession_arg,
                                                          FareType = _Flights.FareTypeDescription,
                                                          AIF = _Flights.AvailInfo, //Availablity Information
                                                          MFR = _Flights.FareRuleInfo, //Min Fare Rule
                                                          Servfee = _FareData.ServiceFee,
                                                          SFtax = _FareData.SFAMOUNT,
                                                          SFGST = _FareData.SFGST,
                                                          Opt_carrier = _Flights.OperatingCarrier,
                                                          Availagent = array_Response[available_agent],
                                                          PromocodeDesc = _Flights.PromoCodeDesc
                                                      };



                                string lstr = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                string[] FLIGHTID = (from sathees in FlightsDatatemp.AsEnumerable()
                                                     where (sathees.Seats.Trim() != "" && lstr.Contains(sathees.Seats))
                                                     select sathees.flightid).ToArray();

                                string lstid = string.Join(",", FLIGHTID);

                                var FlightsData = (from sathees in FlightsDatatemp.AsEnumerable()
                                                   where (!lstid.Contains(sathees.flightid))
                                                   select sathees);

                                array_Response[JsonResponse] = JsonConvert.SerializeObject(FlightsData);


                                if (DirectSearch != null && DirectSearch != "")
                                {
                                    if (Convert.ToBoolean(DirectSearch))
                                    {
                                        var qryflights = (from q in FlightsData.AsEnumerable()
                                                          where (q.FC == "0")
                                                          select q);
                                        array_Response[JsonResponse] = JsonConvert.SerializeObject(qryflights);
                                    }

                                    Session.Add("DirEctFlightSearch", DirectSearch);
                                }
                                else
                                {
                                    Session.Add("DirEctFlightSearch", false);
                                }

                                Session["ValueKey"] = null;
                                Session["ValueKey"] = strKeySession[1].ToString().Trim();
                                string TCnt = strAdults + "," + strChildrens + "," + strInfants;
                                Session.Add("TravellerCount" + strKeySession[1].ToString().Trim(), TCnt);

                                Session["strAvailSession"] = null;
                                Session[strAvailSession] = _availRes;
                                array_Response[AvailSession] = strAvailSession;
                                stu = "1";
                                msg = "";
                                dictThreadRes = new Dictionary<string, string>();
                                dictThreadRes.Add("RST_" + strTrip + "_" + "", strResponse);//strResponse


                                #endregion

                                #region Retward ItinRef 1

                                string strAvailSession_ret = "AVAILFLIGHTSINT_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                string[] strKeySession_ret = strAvailSession_ret.Split('_');
                                strAvailSession_arg = strAvailSession_ret;
                                stu = "1";
                                msg = "";

                                var FlightsDatatemp_ret = from _Flights in _availRes.Flights.AsEnumerable()
                                                          join _Fares in _availRes.Fares.AsEnumerable()
                                                          on _Flights.FareId equals _Fares.FlightId
                                                          from _FareData in _Fares.Faredescription.Where(e => e.Paxtype.Equals("ADT")).AsEnumerable()
                                                          where _Flights.ItinRef == "1"
                                                          select new
                                                          {
                                                              FC = findflightcount(_availRes.Flights, _Flights.FareId, (strTrip == "Y" ? "2" : "1")),
                                                              Org = _Flights.Origin,
                                                              Des = _Flights.Destination,
                                                              Dep = _Flights.DepartureDateTime,
                                                              Arr = _Flights.ArrivalDateTime,
                                                              Dur = _Flights.JourneyTime,
                                                              Cls = _Flights.Class.Trim() + '-' + _Flights.FareBasisCode.Trim(),
                                                              Cab = _Flights.Cabin.TrimEnd(','),
                                                              BFare = _FareData.BaseAmount,
                                                              GFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) +
                                                              Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ?
                                                              _FareData.ClientMarkup : "0")
                                                              + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ?
                                                              _FareData.Servicecharge : "0")
                                                              + Base.ServiceUtility.CovertToDouble((_FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ?
                                                              _FareData.SFAMOUNT : "0")
                                                                + Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ?
                                                              _FareData.SFGST : "0")
                                                              + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ?
                                                              _FareData.ServiceFee : "0")).ToString(),

                                                              WTMFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount)
                                                              + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ?
                                                              _FareData.Servicecharge : "0")).ToString("0"),


                                                              WTSCANDNETFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount)
                                                               + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ?
                                                               _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null &&
                                                               _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0") + Base.ServiceUtility.CovertToDouble((
                                                               _FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ? _FareData.SFAMOUNT : "0") +
                                                               Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ? _FareData.SFGST : "0")
                                                               + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ?
                                                              _FareData.ServiceFee : "0")).ToString(),

                                                              NETFare = (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount)
                                                               + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ?
                                                               _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ?
                                                              _FareData.ServiceFee : "0") + Base.ServiceUtility.CovertToDouble((_FareData.ServiceTax != null && _FareData.ServiceTax != "") ?
                                                              _FareData.ServiceTax : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFAMOUNT != null && _FareData.SFAMOUNT != "") ?
                                                              _FareData.SFAMOUNT : "0") + Base.ServiceUtility.CovertToDouble((_FareData.SFGST != null && _FareData.SFGST != "") ?
                                                              _FareData.SFGST : "0") + (ConfigurationManager.AppSettings["AVAILFORMAT"].ToString() == "NAT" ? Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ?
                                                              _FareData.Servicecharge : "0") : 0) -
                                                               Base.ServiceUtility.CovertToDouble((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                    Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString()))).ToString(),

                                                              Con = _Flights.CNX,
                                                              Fno = _Flights.FlightNumber.TrimEnd(),
                                                              Ref = _Flights.ReferenceToken,
                                                              Inva = _Flights.Origin + "SpLitPResna" + _Flights.Destination + "SpLitPResna" + _Flights.DepartureDateTime + "SpLitPResna" + _Flights.ArrivalDateTime + "SpLitPResna" + _Flights.JourneyTime + "SpLitPResna" + _Flights.Class.Trim() + "SpLitPResna" + strAdults + "SpLitPResna" + strChildrens + //upto index 7
                                                              "SpLitPResna" + strInfants + "SpLitPResna" + _Flights.PlatingCarrier + "SpLitPResna" + _Flights.RBDCode + "SpLitPResna" + _Flights.TransactionFlag + "SpLitPResna" + _Flights.ReferenceToken + "SpLitPResna" + _FareData.BaseAmount + "SpLitPResna" + _FareData.GrossAmount + //upto index 14
                                                              "SpLitPResna" + _Flights.ConnectionFlag + "SpLitPResna" + _Flights.AirlineCategory + "SpLitPResna" + _Flights.FareBasisCode + "SpLitPResna" + _Flights.FlightNumber + "SpLitPResna" + _Flights.ItinRef + "SpLitPResna" + _Flights.CNX + "SpLitPResna" + _Flights.Cabin.TrimEnd(',') + //upto index 21
                                                              "SpLitPResna" + _Flights.JourneyTime + "SpLitPResna" + _availRes.FareCheck.Stocktype + "SpLitPResna" + _Flights.SegmentDetails + "SpLitPResna" + _Flights.FareId + "SpLitPResna" + _FareData.Servicecharge + "SpLitPResna" + _FareData.Incentive + "SpLitPResna" + _FareData.ClientMarkup + "SpLitPResna" + _Flights.SegRef
                                                              + "SpLitPResna" + _Flights.AvailSeat + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ? _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0")
                                                              + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ? _FareData.ServiceFee : "0")).ToString() + "SpLitPResna" + _Flights.Refundable.ToUpper() + "SpLitPResna" + _Flights.Via + "SpLitPResna" + _Flights.FareTypeDescription + "SpLitPResna" + _Fares.FareType + "SpLitPResna" + _Flights.OperatingCarrier + "SpLitPResna" + _Flights.PromoCodeDesc + "SpLitPResna" + _Flights.PromoCode,

                                                              tax = _FareData.Taxes,
                                                              acat = _Flights.AirlineCategory + "SpLitPResna" + _Flights.Origin + "SpLitPResna" + _Flights.Destination + "SpLitPResna" + _Flights.DepartureDateTime + "SpLitPResna" + _Flights.ReferenceToken + "SpLitPResna" + _Flights.PlatingCarrier + "SpLitPResna" + _availRes.FareCheck.Stocktype,

                                                              nonstop = _Flights.Via,
                                                              com = ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                    Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")) - Convert.ToDecimal(_FareData.ServiceTax ?? "0")).ToString()),
                                                              fly = _Flights.FlyingTime,
                                                              seg = _Flights.SegmentDetails,
                                                              Bagg = aircraft(_availRes.Flights, _availRes.Flights.IndexOf(_Flights)),
                                                              plt = _Flights.PlatingCarrier,
                                                              offFlag = _Flights.OfflineFlag,
                                                              Stops = _Flights.Stops,
                                                              Seats = _Flights.AvailSeat,
                                                              iti = _Flights.ItinRef,
                                                              multiclass = _Flights.MultiClass,
                                                              flightid = _Fares.FlightId,
                                                              StkType = _availRes.FareCheck.Stocktype,
                                                              Service = _FareData.Servicecharge,
                                                              Markup = _FareData.ClientMarkup,
                                                              Refund = _Flights.Refundable.ToUpper(),
                                                              // urlcnt = imgurl + "MultiCarrier" + ".png",
                                                              connflightid = _Flights.ConnectionFlag,
                                                              bestbuy = _FareData.BestBuyOption.ToString().ToUpper(), //TRUE - for Special fare button enable...
                                                              taxbreakupold = string.Join("|", _FareData.Taxes.Select(t => t.Code + ":" + t.Amount).ToArray()) + (_FareData.Servicecharge != "0" ? "|Serv.Chrg:" + _FareData.Servicecharge : ""),
                                                              mclassresult = ((_Flights.MultiClass != null && _Flights.MultiClass != "" && _Flights.MultiClass != "0") ? _Flights.FlightNumber + "SpLitPResna" + _Fares.FlightId + "SpLitPResna" + _Flights.Class.Trim() + "SpLitPResna" + _Flights.ReferenceToken + "SpLitPResna" + _FareData.BaseAmount + "SpLitPResna" + Base.ServiceUtility.CovertToDouble((_FareData.BaseAmount != null && _FareData.BaseAmount != "") ? _FareData.BaseAmount : "0") + "\nComm : " +
                                                              ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                    Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString())) + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0")
                                                              + Base.ServiceUtility.CovertToDouble((_FareData.TransactionFee != null && _FareData.TransactionFee != "") ? _FareData.TransactionFee : "0")).ToString("0") + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ? _FareData.ClientMarkup : "0")
                                                              + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0") + Base.ServiceUtility.CovertToDouble((_FareData.TransactionFee != null && _FareData.TransactionFee != "") ? _FareData.TransactionFee : "0")).ToString("0") + "SpLitPResna" + _FareData.Taxes + "SpLitPResna" + _FareData.ClientMarkup + "SpLitPResna" + _FareData.ServiceTax + "SpLitPResna" + _FareData.TDS + "SpLitPResna" + _FareData.Servicecharge + //
                                                                            "SpLitPResna" + _FareData.Paxtype + "SpLitPResna" +
                                                                            ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                    Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString())) + "SpLitPResna" +
                                                                    ((Convert.ToDecimal(Convert.ToDecimal(_FareData.Discount ?? "0") +
                                                                    Convert.ToDecimal(_FareData.PLBAmount ?? "0") + Convert.ToDecimal(_FareData.Incentive ?? "0")).ToString())) + "SpLitPResna" + "0" + "SpLitPResna" + string.Join("/", _FareData.Taxes.Select(t => t.Code + ":" + t.Amount).ToArray()) + "SpLitPResna" + _Flights.Baggage + "SpLitPResna" + _Flights.FareBasisCode + "SpLitPResna" + "N/A" + "SpLitPResna" + _FareData.GrossAmount + "SpLitPResna" + _FareData.SFAMOUNT + "SpLitPResna" + _FareData.SFGST + "SpLitPResna" + _FareData.ServiceFee : ""),
                                                              //"SpLitPResna" + _FareData.Paxtype + "SpLitPResna" + _FareData.Commission + "SpLitPResna" + _FareData.Discount + "SpLitPResna" + _FareData.Incentive + "SpLitPResna" + string.Join("/", _FareData.Taxes.Select(t => t.Code + ":" + t.Amount).ToArray()) + "SpLitPResna" + _Flights.Baggage + "SpLitPResna" + _Flights.FareBasisCode + "SpLitPResna" + (string.IsNullOrEmpty(_Flights.Otherbenfit) ? "N/A" : _Flights.Otherbenfit) + "SpLitPResna" + _FareData.GrossAmount,
                                                              mclassinva = ((_Flights.MultiClass != null && _Flights.MultiClass != "" && _Flights.MultiClass != "0") ? _Flights.Origin + "SpLitPResna" + _Flights.Destination + "SpLitPResna" + _Flights.DepartureDateTime + "SpLitPResna" + _Flights.ArrivalDateTime + "SpLitPResna" + _Flights.JourneyTime + "SpLitPResnaclasssSpLitPResna" + strAdults + "SpLitPResna" + strChildrens +
                                                                       "SpLitPResna" + strInfants + "SpLitPResna" + _Flights.PlatingCarrier + "SpLitPResnaRBDSpLitPResna" + false + "SpLitPResnaTokenSpLitPResnaBaseAmntSpLitPResnaGrossAmntSpLitPResna" + _Flights.ConnectionFlag +
                                                                       "SpLitPResna" + _Flights.AirlineCategory + "SpLitPResnaFareBasisCodeSpLitPResna" + _Flights.FlightNumber + "SpLitPResna" + _Flights.ItinRef + "SpLitPResna" + _Flights.CNX + "SpLitPResna" + _Flights.Cabin.TrimEnd(',') +
                                                                       "SpLitPResna" + _Flights.JourneyTime + "SpLitPResna" + _availRes.FareCheck.Stocktype + "SpLitPResna" + _Flights.SegmentDetails + "SpLitPResnaSTUfareID" + "SpLitPResnaSTUservcharg" + "SpLitPResnaSTUincentive" + "SpLitPResnaSTUmarkup" + "SpLitPResnaSTUsegref" + "SpLitPResna" + _Flights.AvailSeat + "SpLitPResna" + (Base.ServiceUtility.CovertToDouble(_FareData.GrossAmount) + Base.ServiceUtility.CovertToDouble((_FareData.ClientMarkup != null && _FareData.ClientMarkup != "") ? _FareData.ClientMarkup : "0") + Base.ServiceUtility.CovertToDouble((_FareData.Servicecharge != null && _FareData.Servicecharge != "") ? _FareData.Servicecharge : "0")
                                                             + Base.ServiceUtility.CovertToDouble((_FareData.ServiceFee != null && _FareData.ServiceFee != "") ? _FareData.ServiceFee : "0")).ToString() + "SpLitPResna" + _Flights.Refundable.ToUpper() + "SpLitPResna" + _Flights.Via + "SpLitPResna" + _Flights.FareTypeDescription + "SpLitPResna" + _Fares.FareType + "SpLitPResna" + _Flights.OperatingCarrier + "SpLitPResna" + _Flights.PromoCodeDesc + "SpLitPResna" + _Flights.PromoCode : ""),

                                                              SinglePNR = strTrip + "~" + _Fares.FareType + "~" + _availRes.FareCheck.Stocktype + "~" + _Flights.FareTypeDescription,   //hide by srinath_28_10_check
                                                              availsr_id = "",
                                                              bindside_6_arg = "2",
                                                              AF = _availRes.CacheAvail,
                                                              availbilitysession_1_arg = strAvailSession_arg,
                                                              FareType = _Flights.FareTypeDescription,
                                                              AIF = _Flights.AvailInfo, //Availablity Information
                                                              MFR = _Flights.FareRuleInfo, //Min Fare Rule
                                                              Servfee = _FareData.ServiceFee,
                                                              SFtax = _FareData.SFAMOUNT,
                                                              SFGST = _FareData.SFGST,
                                                              Opt_carrier = _Flights.OperatingCarrier,
                                                              Availagent = array_Response[available_agent],
                                                              PromocodeDesc = _Flights.PromoCodeDesc
                                                          };



                                string lstr_ret = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                string[] FLIGHTID_ret = (from sathees in FlightsDatatemp_ret.AsEnumerable()
                                                         where (sathees.Seats.Trim() != "" && lstr.Contains(sathees.Seats))
                                                         select sathees.flightid).ToArray();

                                string lstid_ret = string.Join(",", FLIGHTID);

                                var FlightsData_ret = (from sathees in FlightsDatatemp_ret.AsEnumerable()
                                                       where (!lstid.Contains(sathees.flightid))
                                                       select sathees);

                                array_Response[RetavailforRTspl] = JsonConvert.SerializeObject(FlightsData_ret);


                                if (DirectSearch != null && DirectSearch != "")
                                {
                                    if (Convert.ToBoolean(DirectSearch))
                                    {
                                        var qryflights = (from q in FlightsData_ret.AsEnumerable()
                                                          where (q.FC == "0")
                                                          select q);
                                        array_Response[RetavailforRTspl] = JsonConvert.SerializeObject(qryflights);
                                    }

                                    Session.Add("DirEctFlightSearch", DirectSearch);
                                }
                                else
                                {
                                    Session.Add("DirEctFlightSearch", false);
                                }

                                Session["ValueKey"] = null;
                                Session["ValueKey"] = strKeySession_ret[1].ToString().Trim();
                                string TCnt_ret = strAdults + "," + strChildrens + "," + strInfants;
                                Session.Add("TravellerCount" + strKeySession_ret[1].ToString().Trim(), TCnt);

                                Session["strAvailSession"] = null;
                                Session[strAvailSession] = _availRes;
                                array_Response[AvailSession] = strAvailSession;
                                stu = "1";
                                msg = "";
                                dictThreadRes = new Dictionary<string, string>();
                                dictThreadRes.Add("RST_" + strTrip + "_" + "", strResponse);//strResponse

                                #endregion

                            }
                            #endregion


                            #region CRM Avail Insert
                            if (dictThreadRes != null && dictThreadRes.Count != 0)
                            {
                                lstra = "<RESULT>";
                                int indexincr = 1;
                                lstra += "<BranchID></BranchID>";
                                lstra += "<Triptype>" + strTripType + "</Triptype>";
                                lstra += "<Airporttype>I</Airporttype>";
                                lstra += "<BoaClientID>" + ClientID + "</BoaClientID>";
                                lstra += "<ORG>" + strOrigin + "</ORG>";
                                lstra += "<DES>" + strDestination + "</DES>";
                                lstra += "<Request>" + strAdults + "~" + strChildrens + "~" + strInfants + "~" + strTripType + "~" + "I" + "~" + Class + "~" + "~" + strOrigin + "~" + strDestination + "</Request>";
                                foreach (string item in dictThreadRes.Values)
                                {
                                    lstra += "<RESPONSE" + indexincr + ">" + item + "</RESPONSE" + indexincr + ">";
                                    indexincr++;
                                }
                                lstra += "</RESULT>";

                            }
                            _CRMDATA.Xml = lstra;

                            Session.Add("insertxml", JsonConvert.SerializeObject(_CRMDATA));
                            #endregion

                        }

                        else
                        {
                            #region log

                            string lstrtime = "AvailabityResponse" + DateTime.Now;



                            string error = (_availRes.Error != null &&
                             (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ?
                             ((string.IsNullOrEmpty(_availRes.Sqe) ?
                             _availRes.Error.ToString() :
                             (_availRes.Sqe + " : " + _availRes.Error.ToString()))) : "";

                            LstrDetails = "<THREAD_RESPONSE><THREAD>" + (itin.Category + " ~TRSF~" +
                                (string.IsNullOrEmpty(strOrigin) ? "" : strOrigin) +
                                (string.IsNullOrEmpty(strDestination) ? "" : strDestination)) + "</THREAD><REQTIME>" + ReqTime + "</REQTIME><RESTIME>" +
                           lstrtime + "</RESTIME>" + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                           ((Base.ReqLog) ? strResponse.ToString() : "") + "</THREAD_RESPONSE>";

                            DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "InternationalFlights", (itin.TripType + " ~TRSF~ " + itin.Category + " ~ " +
                                (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) + (string.IsNullOrEmpty(strDestination) ? "" :
                                strDestination)) + " :THREAD RESPONSE FAIL - " + error, LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());


                            #endregion
                            array_Response[FlightSearchError] = _availRes.Error != null && _availRes.Error != "" ? _availRes.Error : "No flights available";
                            msg = _availRes.Error != null && _availRes.Error != "" ? _availRes.Error : "No flights available";
                            if (_availRes.Error != null && _availRes.Error != "")
                            {
                                array_Response[threadnoresponse] = FCategory + " -> " + array_Response[FlightSearchError];
                                threadnoresponse_arg = FCategory + " -> " + array_Response[FlightSearchError];
                            }
                            stu = "0";
                            msg = msg + "(" + itin.Category + ")";



                        }

                        //availsetflag = _availRes.Stock.Split('_')[1].Split('~')[0];
                    }
                    else
                    {
                        #region log

                        string lstrtime = "AvailabityResponse" + DateTime.Now;



                        string error = (_availRes.Error != null &&
                         (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ?
                         ((string.IsNullOrEmpty(_availRes.Sqe) ?
                         _availRes.Error.ToString() :
                         (_availRes.Sqe + " : " + _availRes.Error.ToString()))) : "";

                        LstrDetails = "<THREAD_RESPONSE><THREAD>" + (itin.Category + " ~TRSF~" +
                            (string.IsNullOrEmpty(strOrigin) ? "" : strOrigin) +
                            (string.IsNullOrEmpty(strDestination) ? "" : strDestination)) + "</THREAD><REqTime>" + ReqTime + "</REqTime><ResTIME>" +
                       lstrtime + "</ResTIME>" + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                       ((Base.ReqLog) ? strResponse.ToString() : "") + "</THREAD_RESPONSE>";



                        DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "InternationalFlights", (itin.TripType + " ~TRSF~ " + itin.Category + " ~ " +
                            (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) + (string.IsNullOrEmpty(strOrigin) ? "" :
                            strOrigin)) + " :THREAD RESPONSE FAIL - " + error, LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());


                        #endregion

                        array_Response[Error] = "Problem occured while Searching Flight";

                        stu = "0";
                        msg = "Problem occured while Searching Flight";
                        // return array_Response;

                    }

                    array_Response[concept_groupfare] = ""; //array_Response[AvailSession] + "sri" + array_Response[Error] + "sri" + array_Response[FlightSearchError] + "sri" + array_Response[JsonResponse] + "sri" + array_Response[threadnoresponse] + "sri" + array_Response[availsideflat] + "sri" + array_Response[memcachechck];
                    // return array_Response;
                    return Json(new { Status = stu, Message = msg, Result = array_Response });
                }

                catch (Exception Second_Ex)
                {
                    stu = "0";
                    msg = "";
                    string Xmldata = "<EVENT><DeptDate>[<![CDATA[" + strDepartureDate + "]]>]</DeptDate><RetDate>[<![CDATA[" + strRetDate + "]]>]</RetDate><DirectSearch>[<![CDATA[" + DirectSearch + "</DirectSearch><Error>[<![CDATA[" + Second_Ex.ToString() + "]]>]</Error></EVENT>";
                    DatabaseLog.LogData_wasc(Session["username"].ToString(), "X", "FlightSearch", "Search Request", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    //  return array_Response;
                    return Json(new { Status = stu, Message = msg, Result = array_Response, Availset = availsetflag });

                }
                #endregion
            }
            catch (Exception First_Ex)
            {
                stu = "0";
                msg = "";
                string Xmldata = "<EVENT><DeptDate>[<![CDATA[" + strDepartureDate + "]]>]</DeptDate><RetDate>[<![CDATA[" + strRetDate + "]]>]</RetDate><DirectSearch>[<![CDATA[" + DirectSearch + "]]>]</DirectSearch><Error>[<![CDATA[" + First_Ex.ToString() + "]]>]</Error></EVENT>";
                DatabaseLog.LogData_wasc(Session["username"].ToString(), "X", "FlightSearch", "Search Request", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            array_Response[concept_groupfare] = ""; //array_Response[AvailSession] + "sri" + array_Response[Error] + "sri" + array_Response[FlightSearchError] + "sri" + array_Response[JsonResponse] + "sri" + array_Response[threadnoresponse] + "sri" + array_Response[availsideflat] + "sri" + array_Response[memcachechck];
            return Json(new { Status = stu, Message = msg, Result = array_Response });

        }

        private string findflightcount(List<RQRS.Flights> _Flights, string lindex, string countt)
        {
            string count = string.Empty;
            try
            {
                var FlightsDatatemp = from Q in _Flights.AsEnumerable()
                                      where Q.FareId == lindex
                                      select new
                                      {

                                      };
                count = (FlightsDatatemp.Count() - Convert.ToInt16(countt)).ToString();

            }
            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "findflightcount", "Newavailservice");
            }
            return count;
        }

        private string aircraft(List<RQRS.Flights> _Flights, int lindex)
        {
            string TempsegmentDetails = "";
            do
            {
                TempsegmentDetails = (_Flights[lindex].SegmentDetails.Split('\n').Length > 4 &&
                    _Flights[lindex].SegmentDetails.Split('\n')[4].Split(':').Length > 1 ?
                    _Flights[lindex].SegmentDetails.Split('\n')[4].Split(':')[1] : "") + " / " + TempsegmentDetails;
                lindex--;
            } while (lindex >= 0 && (_Flights.Count > lindex ? _Flights[lindex].CNX.Trim() == "Y" : true));
            TempsegmentDetails = TempsegmentDetails.Trim().TrimEnd('/');
            return TempsegmentDetails;
        }

        public string form_multiclass_avail(string availmulticlass, string flight_no)
        {
            try
            {
                if (availmulticlass != null && availmulticlass.Trim() != "")
                {
                    multiclass.MultiClass_RS _availRes = JsonConvert.DeserializeObject<multiclass.MultiClass_RS>(availmulticlass);
                    if (_availRes.Status.ResultCode == "1")
                    {
                        DataTable dt = Serv.getMultiClass(_availRes, flight_no);
                        availmulticlass = dt.Rows[0]["FlightNum"] + "SpLitPResna" + dt.Rows[0]["FlightId"] + "SpLitPResna" + dt.Rows[0]["Class"] + "SpLitPResna" + dt.Rows[0]["Token"] + "SpLitPResna" + dt.Rows[0]["BasicFare"] + "SpLitPResna" + dt.Rows[0]["BasicFareComm"] + "SpLitPResna" + dt.Rows[0]["GrossAmount"] + "SpLitPResna" + dt.Rows[0]["GrossAmountTrans"] + "SpLitPResna" + dt.Rows[0]["TaxAmount"] + "SpLitPResna" + dt.Rows[0]["MarkUp"] + "SpLitPResna" + dt.Rows[0]["ServiceTax"] + "SpLitPResna" + dt.Rows[0]["TDS"] + "SpLitPResna" + dt.Rows[0]["ServiceCharge"] +
                           "SpLitPResna" + dt.Rows[0]["PaxType"] + "SpLitPResna" + dt.Rows[0]["Commision"] + "SpLitPResna" + dt.Rows[0]["Discount"] + "SpLitPResna" + dt.Rows[0]["Incentive"] + "SpLitPResna" + dt.Rows[0]["Breakup"] + "SpLitPResna" + dt.Rows[0]["WTMAMT"] + "SpLitPResna" + dt.Rows[0]["baggagedetails"];
                    }
                    else
                    {
                        availmulticlass = "";
                    }
                }
                else
                {
                    availmulticlass = "";
                }

                return availmulticlass;
            }

            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "formmclassavail", "Newavailservice");
                return string.Empty;
            }
        }

        public ActionResult FormmulticlasssRequest(string FullFlag, string Flightid, string SClass, string availmulti, string Base_orgin, string Base_destin, string Trip_type, string Cabin, string Flightstock, string ClientID, string Segmenttype)
        {
            ArrayList MC_response = new ArrayList();
            DataTable dtSelectResponse = new DataTable();
            DataTable dtMultiClassResponse = new DataTable();
            MC_response.Add("");
            MC_response.Add("");
            MC_response.Add("");
            MC_response.Add("");
            int error = 0;
            int response = 1;
            int AvailSession = 2;
            int InvaFlag = 3;

            string AgentID = string.Empty;
            string TerminalID = string.Empty;
            string UserName = string.Empty;
            string adtcount = string.Empty;
            string chdcount = string.Empty;
            string infcount = string.Empty;
            string stock = string.Empty;
            string strResponse = string.Empty;
            string Origin = string.Empty;
            string Destination = string.Empty;
            string depttime = string.Empty;
            string ArrTime = string.Empty;
            string PlatingCarrier = string.Empty;
            string ConnectionFlag = string.Empty;
            string AirlineCategory = string.Empty;
            DataTable sess_class = new DataTable();
            DataTable session_tab = new DataTable();

            string Flight_id = string.Empty;
            int segrf = 0;

            RQRS.MultiClass_RQ mreq = new RQRS.MultiClass_RQ();
            RQRS.AgentDetailsMulticlass _agent = new RQRS.AgentDetailsMulticlass();
            List<RQRS.RQFlightsnew> _fdet = new List<RQRS.RQFlightsnew>();
            RQRS.RQFlightsnew _flightdet = new RQRS.RQFlightsnew();
            RQRS.Segment _segment = new RQRS.Segment();

            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strClientType = Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : "";

            string strClientID = string.Empty; string strClientTerminalID = string.Empty;
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

            try
            {
                if (Session["agentid"] == null)
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }
                #region UsageLog
                string PageName = "Get Multiclass";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion
                string str_agentid = Session["Availagentid"].ToString();
                _agent.AgentID = strClientID;
                _agent.TerminalID = strClientTerminalID;
                _agent.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                _agent.AppType = "B2B";
                _agent.Environment = "B";
                _agent.UserName = Session["username"].ToString();
                _agent.AgentType = strTerminalType == "T" ? strClientType : strAgentType;
                _agent.BOAID = strClientID;
                _agent.BOATreminalID = strClientTerminalID;
                _agent.CoOrdinatorID = "";// ConfigurationManager.AppSettings["CoOrdinatorID"].ToString();
                _agent.Airportid = Segmenttype;//ConfigurationManager.AppSettings["AirportID"].ToString();                
                _agent.BranchID = Session["branchid"].ToString();
                _agent.ClientID = strClientID;
                _agent.Group_ID = Session["GroupID"] != null && Session["GroupID"].ToString() != "" ? Session["GroupID"].ToString() : "";// Session["PRODUCT_CODE"].ToString();
                _agent.ProjectId = Session["PRODUCT_CODE"].ToString();
                _agent.ProductID = Session["PRODUCT_CODE"].ToString();
                _agent.Platform = "B";
                _agent.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();

                string segementdetails = string.Empty;

                #region input_req
                string[] ArrFliDet = null;
                string[] FlightCount = null;
                string[] clsCabin = null;
                string[] cbncls = null;

                if (FullFlag.Contains("SpLITSaTIS"))
                {
                    FlightCount = Regex.Split(FullFlag.TrimEnd(), "SpLITSaTIS");
                    clsCabin = Regex.Split(Cabin, "SpLITSaTIS");

                    for (int iF = 0; iF < FlightCount.Length; iF++)
                    {
                        if (FlightCount[iF] != null && FlightCount[iF] != "")
                        {
                            if (clsCabin[iF].Contains("-") == true)
                            {
                                cbncls = Regex.Split(clsCabin[iF], "-");
                            }
                            else
                            {
                                cbncls = Regex.Split(clsCabin[iF], "");
                            }



                            if (FlightCount[iF].Contains("SpLitPResna"))
                            {
                                segrf++;
                                ArrFliDet = Regex.Split(FlightCount[iF], "SpLitPResna");

                                adtcount = ArrFliDet[6];
                                chdcount = ArrFliDet[7];
                                infcount = ArrFliDet[8];

                                Flight_id = Flightid;
                                Origin = ArrFliDet[0];
                                Destination = ArrFliDet[1];
                                depttime = ArrFliDet[2];
                                ArrTime = ArrFliDet[3];
                                PlatingCarrier = ArrFliDet[9];
                                ConnectionFlag = "";
                                AirlineCategory = ArrFliDet[16];


                                _flightdet = new RQRS.RQFlightsnew();
                                _flightdet.AirlineCategory = ArrFliDet[16];
                                _flightdet.ArrivalDateTime = ArrFliDet[3];
                                _flightdet.Cabin = SClass;//cbncls[1];
                                _flightdet.CarrierCode = (ArrFliDet[18]).Split(' ')[0].ToString();

                                _flightdet.Class = cbncls[0] != "" ? cbncls[0] : cbncls[1]; //ArrFliDet[5];

                                _flightdet.DepartureDateTime = ArrFliDet[2];
                                _flightdet.Destination = ArrFliDet[1];
                                _flightdet.EndTerminal = "";

                                _flightdet.FareBasisCode = cbncls[1];

                                _flightdet.FareType = ArrFliDet[35];
                                _flightdet.FareTypeDescription = "";
                                _flightdet.PromoCode = "";


                                _flightdet.FareID = Flightid;
                                _flightdet.FlightNumber = (ArrFliDet[18]);
                                _flightdet.ItinRef = ArrFliDet[19];
                                _flightdet.Origin = ArrFliDet[0];
                                _flightdet.PlatingCarrier = ArrFliDet[9];
                                _flightdet.ReferenceToken = ArrFliDet[12];
                                _flightdet.SegRef = segrf.ToString();
                                _flightdet.StartTerminal = "";
                                _flightdet.SeatAvailFlag = "";  //Added by saranraj on 20170717...


                                stock = ArrFliDet[23];
                                _fdet.Add(_flightdet);

                                //segment
                                segementdetails += ArrFliDet[24] + "%";
                            }
                        }
                    }

                }
                else
                {
                    if (FullFlag.Contains("SpLitPResna"))
                    {
                        clsCabin = Regex.Split(Cabin, "SpLITSaTIS");

                        if (clsCabin[0].Contains("-") == true)
                        {
                            cbncls = Regex.Split(clsCabin[0], "-");
                        }
                        else
                        {
                            cbncls = Regex.Split(clsCabin[0], "");
                        }

                        ArrFliDet = Regex.Split(FullFlag, "SpLitPResna");
                        adtcount = ArrFliDet[6];
                        chdcount = ArrFliDet[7];
                        infcount = ArrFliDet[8];

                        Flight_id = Flightid;
                        Origin = ArrFliDet[0];
                        Destination = ArrFliDet[1];
                        depttime = ArrFliDet[2];
                        ArrTime = ArrFliDet[3];
                        PlatingCarrier = ArrFliDet[9];
                        ConnectionFlag = "";
                        AirlineCategory = ArrFliDet[16];


                        _flightdet = new RQRS.RQFlightsnew();
                        _flightdet.AirlineCategory = ArrFliDet[16];
                        _flightdet.ArrivalDateTime = ArrFliDet[3];
                        _flightdet.Cabin = SClass;
                        _flightdet.CarrierCode = (ArrFliDet[18]).Split(' ')[0].ToString();
                        _flightdet.Class = cbncls[0] != "" ? cbncls[0] : cbncls[1];//ArrFliDet[17];//ArrFliDet[5];
                        _flightdet.DepartureDateTime = ArrFliDet[2];
                        _flightdet.Destination = ArrFliDet[1];
                        _flightdet.EndTerminal = "";
                        _flightdet.FareBasisCode = cbncls[1];
                        _flightdet.FareID = Flightid;
                        _flightdet.FlightNumber = (ArrFliDet[18]);

                        _flightdet.FareType = ArrFliDet[35];
                        _flightdet.FareTypeDescription = "";
                        _flightdet.PromoCode = "";

                        _flightdet.ItinRef = ArrFliDet[19];
                        _flightdet.Origin = ArrFliDet[0];
                        _flightdet.PlatingCarrier = ArrFliDet[9];
                        _flightdet.ReferenceToken = ArrFliDet[12];
                        _flightdet.SegRef = "1";
                        _flightdet.StartTerminal = "";
                        _flightdet.SeatAvailFlag = "";  //Added by saranraj on 20170717...

                        stock = ArrFliDet[23];
                        _fdet.Add(_flightdet);
                        segementdetails += ArrFliDet[24] + "%";
                    }
                }

                #endregion

                _segment.Adult = Convert.ToInt16(adtcount);
                _segment.BaseDestination = Base_destin;
                _segment.BaseOrigin = Base_orgin;
                _segment.Child = Convert.ToInt16(chdcount);
                _segment.Infant = Convert.ToInt16(infcount);
                _segment.TripType = (Trip_type == "Y") ? "R" : Trip_type == "R" ? "O" : Trip_type; //because For Roundtrip we select multiclass as seperate oneway...

                _segment.SegmentType = Segmenttype;
                Session.Add("SegmentType_SE", Segmenttype);
                mreq.AgentDetail = _agent;
                mreq.FlightsDetails = _fdet;
                mreq.Platform = "T";
                mreq.SegmentDetails = _segment;
                mreq.Stock = stock;

                string request = JsonConvert.SerializeObject(mreq).ToString();
                string Query = "GetMultiClassFare";
                int hostchecktimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);

                MyWebClient client = new MyWebClient();
                client.LintTimeout = hostchecktimeout;
                client.Headers["Content-type"] = "application/json";

                #region Log

                StringWriter strWriter = new StringWriter();
                DataSet dsrequest = new DataSet();
                dsrequest = Serv.convertJsonStringToDataSet(request, "");
                dsrequest.WriteXml(strWriter);
                string LstrDetails = "<SELECT_REQUEST><URL>[<![CDATA[" + ConfigurationManager.AppSettings["Multiclas"].ToString()
                    + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (hostchecktimeout).ToString() + "</TIMEOUT><TIME>"
                    + "</TIME>" + strWriter.ToString() + "</SELECT_REQUEST>";
                DatabaseLog.LogData(Session["username"].ToString(), "S", "Formmulticlass", "multiclasss-Request", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                #endregion

                /****/
                strURLpath = ConfigurationManager.AppSettings["Multiclas"].ToString();
                byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                strResponse = System.Text.Encoding.ASCII.GetString(data);
                /****/
                #region Log

                string ResTime = "AvailabityReqest" + DateTime.Now;
                LstrDetails = "<AVAILRESPONSE><AIRPORTCATEGORY>" + strResponse + "</AIRPORTCATEGORY><URL>[<![CDATA[" + strURLpath + "/Rays.svc/"
                   + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (hostchecktimeout).ToString() +
                   "</TIMEOUT><RESPONSE_TIME>" + ResTime + "</RESPONSE_TIME></AVAILRESPONSE>";


                DatabaseLog.LogData(Session["username"].ToString(), "T", "Formmulticlass", "Multiclass Response - ", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());


                #endregion

                multiclass.MultiClass_RS _availRes = JsonConvert.DeserializeObject<multiclass.MultiClass_RS>(strResponse);
                if (_availRes.Status.ResultCode == "1")
                {
                    DataTable dt = Serv.getMultiClass(_availRes, Flight_id);
                    var response_mc = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows.Count > 1)
                        {
                            dtMultiClassResponse.Merge(dt);//0                              //1                                     //2                                         //3                                 //4                                         //5                                             //6                                             //7                                         //8                                     //9                                         //10                                    //11                                //12
                            response_mc += dt.Rows[i]["FlightNum"] + "SpLitPResna" + dt.Rows[i]["FlightId"] + "SpLitPResna" + dt.Rows[i]["Class"] + "SpLitPResna" + dt.Rows[i]["Token"] + "SpLitPResna" + dt.Rows[i]["BasicFare"] + "SpLitPResna" + dt.Rows[i]["BasicFareComm"] + "SpLitPResna" + dt.Rows[i]["GrossAmount"] + "SpLitPResna" + dt.Rows[i]["GrossAmountTrans"] + "SpLitPResna" + dt.Rows[i]["TaxAmount"] + "SpLitPResna" + dt.Rows[i]["ClientMarkup"] + "SpLitPResna" + dt.Rows[i]["ServiceTax"] + "SpLitPResna" + dt.Rows[0]["TDS"] + "SpLitPResna" + dt.Rows[i]["ServiceCharge"] + //
                                   "SpLitPResna" + dt.Rows[i]["PaxType"] + "SpLitPResna" + dt.Rows[i]["Commision"] + "SpLitPResna" + dt.Rows[i]["Discount"] + "SpLitPResna" + dt.Rows[i]["Incentive"] + "SpLitPResna" + dt.Rows[i]["Breakup"] + "SpLitPResna" + dt.Rows[i]["Baggage"] + "SpLitPResna" + dt.Rows[i]["Farebasecode"] + "SpLitPResna" + dt.Rows[i]["Otherbenfit"] + "SpLitPResna" + dt.Rows[i]["GrossAmountTranssel"] + "SpLitPResna" + dt.Rows[i]["Sftax"] + "SpLitPResna" + dt.Rows[i]["Sfgst"] + "SpLitPResna" + dt.Rows[i]["Servfee"] + "SpLITSaTIS";//+ "SpLitPResna" + dt.Rows[0]["WTMAMT"]
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      //13                                            //14                                    //15                                        //16                                    //17                                        //18                                    //19                                        //20                                        //21                                                //22                                 //23                                   //24                           
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      //MC_response[InvaFlag] += dt.Rows[i]["Orgin"] + "SpLitPResna" + dt.Rows[i]["Destinations"] + "SpLitPResna" + dt.Rows[i]["Depttime"] + "SpLitPResna" + dt.Rows[i]["ArrTime"] + "SpLitPResna" + ArrFliDet[4] + "SpLitPResnaclasssSpLitPResna" + adtcount + "SpLitPResna" + chdcount + "SpLitPResna" + infcount + "SpLitPResna" + dt.Rows[i]["PlatingCarrier"] + "SpLitPResnaRBDSpLitPResna" + false + "SpLitPResnaTokenSpLitPResnaBaseAmntSpLitPResnaGrossAmntSpLitPResna" + ConnectionFlag + "SpLitPResna" + dt.Rows[i]["AirlineCategory"] + "SpLitPResnaFareBasisCodeSpLitPResna" + dt.Rows[i]["AirlineCodeflightnum"] + "SpLitPResna" + dt.Rows[i]["ItinRef"] + "SpLitPResna" + ArrFliDet[20] + "SpLitPResna" + ArrFliDet[21] + "SpLitPResna" + ArrFliDet[22] + "SpLitPResna" + dt.Rows[i]["Stocks"] + "SpLITSaTIS";
                            if (i == dt.Rows.Count - 1)
                            {
                                MC_response[InvaFlag] += dt.Rows[i]["Orgin"] + "SpLitPResna" + dt.Rows[i]["Destinations"] + "SpLitPResna" + dt.Rows[i]["Depttime"] + "SpLitPResna" + dt.Rows[i]["ArrTime"] + "SpLitPResna" + ArrFliDet[4] + "SpLitPResnaclasssSpLitPResna" + adtcount + "SpLitPResna" + chdcount +
                                    "SpLitPResna" + infcount + "SpLitPResna" + dt.Rows[i]["PlatingCarrier"] + "SpLitPResnaRBDSpLitPResna" + false + "SpLitPResnaTokenSpLitPResnaBaseAmntSpLitPResnaGrossAmntSpLitPResna" + ConnectionFlag +
                                    "SpLitPResna" + dt.Rows[i]["AirlineCategory"] + "SpLitPResnaFareBasisCodeSpLitPResna" + dt.Rows[i]["AirlineCodeflightnum"] + "SpLitPResna" + dt.Rows[i]["ItinRef"] + "SpLitPResna" + "N" + "SpLitPResna" + ArrFliDet[21] +
                                    "SpLitPResna" + ArrFliDet[22] + "SpLitPResna" + Flightstock + "SpLitPResna" + Regex.Split(segementdetails.Split('%')[i], "Baggage :")[0] + "Baggage : " + dt.Rows[i]["Baggage"] + "SpLitPResnaSTUfareID" + "SpLitPResnaSTUservcharg" + "SpLitPResnaSTUincentive" + "SpLitPResnaSTUmarkup" + "SpLitPResnaSTUsegref" + "SpLitPResna" + ArrFliDet[30] + "SpLitPResnagrosswithmark" + "SpLitPResna" + ArrFliDet[32] + "SpLitPResna" + ArrFliDet[33] + "SpLitPResna" + ArrFliDet[34] + "SpLitPResna" + ArrFliDet[35] + "SpLitPResna" + ArrFliDet[36] + "SpLitPResna" + ArrFliDet[37] + "SpLitPResna" + ArrFliDet[38] + "SpLITSaTIS";
                            }
                            else
                            {
                                MC_response[InvaFlag] += dt.Rows[i]["Orgin"] + "SpLitPResna" + dt.Rows[i]["Destinations"] + "SpLitPResna" + dt.Rows[i]["Depttime"] + "SpLitPResna" + dt.Rows[i]["ArrTime"] + "SpLitPResna" + ArrFliDet[4] + "SpLitPResnaclasssSpLitPResna" + adtcount + "SpLitPResna" + chdcount +
                                    "SpLitPResna" + infcount + "SpLitPResna" + dt.Rows[i]["PlatingCarrier"] + "SpLitPResnaRBDSpLitPResna" + false + "SpLitPResnaTokenSpLitPResnaBaseAmntSpLitPResnaGrossAmntSpLitPResna" + ConnectionFlag +
                                    "SpLitPResna" + dt.Rows[i]["AirlineCategory"] + "SpLitPResnaFareBasisCodeSpLitPResna" + dt.Rows[i]["AirlineCodeflightnum"] + "SpLitPResna" + dt.Rows[i]["ItinRef"] + "SpLitPResna" + "Y" + "SpLitPResna" + ArrFliDet[21] +
                                    "SpLitPResna" + ArrFliDet[22] + "SpLitPResna" + Flightstock + "SpLitPResna" + Regex.Split(segementdetails.Split('%')[i], "Baggage :")[0] + "Baggage : " + dt.Rows[i]["Baggage"] + "SpLitPResnaSTUfareID" + "SpLitPResnaSTUservcharg" + "SpLitPResnaSTUincentive" + "SpLitPResnaSTUmarkup" + "SpLitPResnaSTUsegref" + "SpLitPResna" + ArrFliDet[30] + "SpLitPResnagrosswithmark" + "SpLitPResna" + ArrFliDet[32] + "SpLitPResna" + ArrFliDet[33] + "SpLitPResna" + ArrFliDet[34] + "SpLitPResna" + ArrFliDet[35] + "SpLitPResna" + ArrFliDet[36] + "SpLitPResna" + ArrFliDet[37] + "SpLitPResna" + ArrFliDet[38] + "SpLITSaTIS";
                            }
                        }
                        else
                        {
                            dtMultiClassResponse.Merge(dt);//0                              //1                                     //2                                         //3                                 //4                                         //5                                             //6                                             //7                                         //8                                     //9                                         //10                                    //11                                //12
                            response_mc += dt.Rows[i]["FlightNum"] + "SpLitPResna" + dt.Rows[i]["FlightId"] + "SpLitPResna" + dt.Rows[i]["Class"] + "SpLitPResna" + dt.Rows[i]["Token"] + "SpLitPResna" + dt.Rows[i]["BasicFare"] + "SpLitPResna" + dt.Rows[i]["BasicFareComm"] + "SpLitPResna" + dt.Rows[i]["GrossAmount"] + "SpLitPResna" + dt.Rows[i]["GrossAmountTrans"] + "SpLitPResna" + dt.Rows[i]["TaxAmount"] + "SpLitPResna" + dt.Rows[i]["ClientMarkup"] + "SpLitPResna" + dt.Rows[i]["ServiceTax"] + "SpLitPResna" + dt.Rows[0]["TDS"] + "SpLitPResna" + dt.Rows[i]["ServiceCharge"] + //
                                   "SpLitPResna" + dt.Rows[i]["PaxType"] + "SpLitPResna" + dt.Rows[i]["Commision"] + "SpLitPResna" + dt.Rows[i]["Discount"] + "SpLitPResna" + dt.Rows[i]["Incentive"] + "SpLitPResna" + dt.Rows[i]["Breakup"] + "SpLitPResna" + dt.Rows[i]["Baggage"] + "SpLitPResna" + dt.Rows[i]["Farebasecode"] + "SpLitPResna" + dt.Rows[i]["Otherbenfit"] + "SpLitPResna" + dt.Rows[i]["GrossAmountTranssel"] + "SpLitPResna" + dt.Rows[i]["Sftax"] + "SpLitPResna" + dt.Rows[i]["Sfgst"] + "SpLitPResna" + dt.Rows[i]["Servfee"];//+ "SpLitPResna" + dt.Rows[0]["WTMAMT"]                
                            MC_response[InvaFlag] += dt.Rows[i]["Orgin"] + "SpLitPResna" + dt.Rows[i]["Destinations"] + "SpLitPResna" + dt.Rows[i]["Depttime"] + "SpLitPResna" + dt.Rows[i]["ArrTime"] + "SpLitPResna" + ArrFliDet[4] + "SpLitPResnaclasssSpLitPResna" + adtcount + "SpLitPResna" + chdcount +
                                "SpLitPResna" + infcount + "SpLitPResna" + dt.Rows[i]["PlatingCarrier"] + "SpLitPResnaRBDSpLitPResna" + false + "SpLitPResnaTokenSpLitPResnaBaseAmntSpLitPResnaGrossAmntSpLitPResna" + ConnectionFlag +
                                "SpLitPResna" + dt.Rows[i]["AirlineCategory"] + "SpLitPResnaFareBasisCodeSpLitPResna" + dt.Rows[i]["AirlineCodeflightnum"] + "SpLitPResna" + dt.Rows[i]["ItinRef"] + "SpLitPResna" + ArrFliDet[20] + "SpLitPResna" + ArrFliDet[21] +
                                "SpLitPResna" + ArrFliDet[22] + "SpLitPResna" + Flightstock + "SpLitPResna" + Regex.Split(segementdetails.Split('%')[0], "Baggage :")[0] + "Baggage : " + dt.Rows[i]["Baggage"] + "SpLitPResnaSTUfareID" + "SpLitPResnaSTUservcharg" + "SpLitPResnaSTUincentive" + "SpLitPResnaSTUmarkup" + "SpLitPResnaSTUsegref" + "SpLitPResna" + ArrFliDet[30] + "SpLitPResnagrosswithmark" + "SpLitPResna" + ArrFliDet[32] + "SpLitPResna" + ArrFliDet[33] + "SpLitPResna" + ArrFliDet[34] + "SpLitPResna" + ArrFliDet[35] + "SpLitPResna" + ArrFliDet[36] + "SpLitPResna" + ArrFliDet[37] + "SpLitPResna" + ArrFliDet[38];
                        }
                    }
                    MC_response[AvailSession] = response_mc;
                }
                else
                {
                    if (_availRes.Status.Error != "" && _availRes.Status.Error != null)
                    {
                        MC_response[error] = _availRes.Status.Error;
                    }
                    else
                    {
                        MC_response[error] = "No Class are available";
                    }
                }
            }
            catch (Exception ex)
            {
                MC_response[error] = "Problem occurred while getting class.";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Multiclass", "FormmulticlassRequest", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                return Json(new { Status = "", Message = "", Result = MC_response });
            }
            return Json(new { Status = "", Message = "", Result = MC_response });
            // return MC_response;
        }

        #region Get Class Fare Details

        public ActionResult GetClassfare(string FullFlag, string Flightid, string SClass, string availmulti, string Base_orgin, string Base_destin, string Trip_type, string Cabin, string sementrow, string ClientID)
        {
            ArrayList MC_response = new ArrayList();
            DataTable dtSelectResponse = new DataTable();
            DataTable dtMultiClassResponse = new DataTable();
            MC_response.Add("");
            MC_response.Add("");
            MC_response.Add("");
            MC_response.Add("");
            int error = 0;
            int response = 1;
            //int AvailSession = 2;
            int resultflag = 3;

            string AgentID = string.Empty;
            string TerminalID = string.Empty;
            string UserName = string.Empty;
            string adtcount = string.Empty;
            string chdcount = string.Empty;
            string infcount = string.Empty;
            string stock = string.Empty;
            string strResponse = string.Empty;
            string Origin = string.Empty;
            string Destination = string.Empty;
            string depttime = string.Empty;
            string ArrTime = string.Empty;
            string PlatingCarrier = string.Empty;
            string ConnectionFlag = string.Empty;
            string AirlineCategory = string.Empty;
            DataTable sess_class = new DataTable();
            DataTable session_tab = new DataTable();
            string stu = string.Empty;

            string Flight_id = string.Empty;
            int segrf = 0;

            multiclass.Class_RQ creq = new multiclass.Class_RQ();

            multiclass.AgentDetails _agent = new multiclass.AgentDetails();
            List<multiclass.SegmentDetails> _fdet = new List<multiclass.SegmentDetails>();

            RQRS.Class_RQ MulticlassReq = new RQRS.Class_RQ();

            List<RQRS.Promocodes> Prcode = new List<RQRS.Promocodes>();
            RQRS.Promocodes Promocodes = new RQRS.Promocodes();
            // multiclass.Segment _segment = new multiclass.Segment();
            RQRS.AgentDetailsMulticlass Agent = new RQRS.AgentDetailsMulticlass();
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strClientType = Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : "";

            string strClientID = string.Empty; string strClientTerminalID = string.Empty;
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


            try
            {
                if (Session["agentid"] == null)
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }

                Agent.AgentID = Session["agentid"].ToString();// ConfigurationManager.AppSettings["AgentId"].ToString();
                Agent.TerminalID = Session["terminalid"].ToString(); //ConfigurationManager.AppSettings["TerminalId"].ToString();
                Agent.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                Agent.AppType = "B2B";
                Agent.Environment = "B";
                Agent.UserName = Session["UserName"].ToString();// ConfigurationManager.AppSettings["UserName"].ToString();                
                Agent.AppType = strTerminalType == "T" ? strClientType : strAgentType;
                Agent.BOAID = strClientID;//Session["POS_ID"].ToString(); // ConfigurationManager.AppSettings["BOAID"].ToString();
                Agent.BOATreminalID = strClientTerminalID; // Session["POS_TID"].ToString();// ConfigurationManager.AppSettings["BOAterminalID"].ToString();
                Agent.CoOrdinatorID = "";// ConfigurationManager.AppSettings["CoOrdinatorID"].ToString();
                Agent.Airportid = "";//Session["sAirporttype"] != null ? Session["sAirporttype"].ToString() : "";//ConfigurationManager.AppSettings["AirportID"].ToString();                
                Agent.BranchID = Session["branchid"].ToString();
                Agent.ClientID = strClientID;
                Agent.Group_ID = Session["GroupID"] != null && Session["GroupID"].ToString() != "" ? Session["GroupID"].ToString() : "";
                Agent.ProjectId = Session["PRODUCT_CODE"].ToString();
                Agent.ProductID = Session["PRODUCT_CODE"].ToString();
                Agent.Platform = "B";
                Agent.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();

                List<RQRS.SegmentDetailss> _lstsegdt = new List<RQRS.SegmentDetailss>();
                RQRS.SegmentDetailss _segdt = new RQRS.SegmentDetailss();
                RQRS.PaxInfos paxdt = new RQRS.PaxInfos();

                string[] ArrFliDet = null;
                string[] FlightCount = null;
                if (FullFlag.Contains("SpLITSaTIS"))
                {
                    FlightCount = Regex.Split(FullFlag, "SpLITSaTIS");
                    for (int iF = 0; iF < FlightCount.Length; iF++)
                    {
                        if (FlightCount[iF].Contains("SpLitPResna"))
                        {
                            segrf++;
                            ArrFliDet = Regex.Split(FlightCount[iF], "SpLitPResna"); //FlightCount[iF].Split('~');
                            _segdt = new RQRS.SegmentDetailss();

                            adtcount = ArrFliDet[6];
                            chdcount = ArrFliDet[7];
                            infcount = ArrFliDet[8];
                            paxdt.ADT = Convert.ToInt32(adtcount.ToString());
                            paxdt.CHD = Convert.ToInt32(chdcount.ToString());
                            paxdt.INF = Convert.ToInt32(infcount.ToString());

                            Flight_id = Flightid;
                            Origin = ArrFliDet[0];
                            Destination = ArrFliDet[1];
                            depttime = ArrFliDet[2];
                            ArrTime = ArrFliDet[3];
                            PlatingCarrier = ArrFliDet[9];
                            ConnectionFlag = "";
                            AirlineCategory = ArrFliDet[16];


                            multiclass.SegmentDetails _SegmentDetails = new multiclass.SegmentDetails();

                            _segdt.AirlineCategory = ArrFliDet[16];
                            _segdt.ArrivalDateTime = ArrFliDet[3];
                            _segdt.Cabin = SClass.Split('|');//cbncls[1];
                            _segdt.CarrierCode = (ArrFliDet[18]).Split(' ')[0].ToString();
                            _segdt.Class = Cabin;//cbncls[1];//ArrFliDet[5];
                            _segdt.DepartureDateTime = ArrFliDet[2];
                            _segdt.Destination = ArrFliDet[1];
                            _segdt.FareBasisCode = Cabin;
                            _segdt.FlightNumber = (ArrFliDet[18]).Split(' ')[1].ToString();
                            _segdt.Origin = ArrFliDet[0];
                            _segdt.PlatingCarrier = ArrFliDet[9];
                            _segdt.ReferenceToken = ArrFliDet[12];
                            _segdt.ItinRef = Convert.ToInt32(ArrFliDet[19]);
                            _segdt.SegRef = segrf.ToString();
                            _segdt.FareType = ArrFliDet[35];
                            _segdt.FareTypeDescription = "";

                            MulticlassReq.Stock = ArrFliDet[23];
                            _lstsegdt.Add(_segdt);

                        }
                    }

                }
                else
                {
                    if (FullFlag.Contains("SpLitPResna"))
                    {
                        ArrFliDet = Regex.Split(FullFlag, "SpLitPResna");
                        _segdt = new RQRS.SegmentDetailss();
                        adtcount = ArrFliDet[6];
                        chdcount = ArrFliDet[7];
                        infcount = ArrFliDet[8];

                        paxdt.ADT = Convert.ToInt32(adtcount.ToString());
                        paxdt.CHD = Convert.ToInt32(chdcount.ToString());
                        paxdt.INF = Convert.ToInt32(infcount.ToString());

                        Flight_id = Flightid;
                        Origin = ArrFliDet[0];
                        Destination = ArrFliDet[1];
                        depttime = ArrFliDet[2];
                        ArrTime = ArrFliDet[3];
                        PlatingCarrier = ArrFliDet[9];
                        ConnectionFlag = "";
                        AirlineCategory = ArrFliDet[16];


                        //multiclass.SegmentDetails _SegmentDetails = new multiclass.SegmentDetails();
                        _segdt.AirlineCategory = ArrFliDet[16];
                        _segdt.ArrivalDateTime = ArrFliDet[3];
                        _segdt.Cabin = SClass.Split('|');
                        _segdt.CarrierCode = (ArrFliDet[18]).Split(' ')[0].ToString();
                        _segdt.Class = Cabin;//ArrFliDet[17];//ArrFliDet[5];
                        _segdt.DepartureDateTime = ArrFliDet[2];
                        _segdt.Destination = ArrFliDet[1];
                        _segdt.FareBasisCode = Cabin;
                        _segdt.FlightNumber = (ArrFliDet[18]).Split(' ')[1].ToString();
                        _segdt.Origin = ArrFliDet[0];
                        _segdt.PlatingCarrier = ArrFliDet[9];
                        _segdt.ReferenceToken = ArrFliDet[12];
                        _segdt.ItinRef = Convert.ToInt32(ArrFliDet[19]);
                        _segdt.SegRef = "1";
                        _segdt.FareType = ArrFliDet[35];
                        _segdt.FareTypeDescription = "";

                        MulticlassReq.Stock = ArrFliDet[23];

                        _lstsegdt.Add(_segdt);
                    }
                }

                creq.AgentDetail = _agent;
                // creq.FlightsDetails = _fdet;
                creq.Platform = "T";
                creq.Segments = _fdet;
                creq.Stock = stock;

                MulticlassReq.AgentDetail = Agent;
                MulticlassReq.Segments = _lstsegdt;
                MulticlassReq.TripType = Trip_type;

                MulticlassReq.PaxDetails = paxdt;

                string request = JsonConvert.SerializeObject(MulticlassReq).ToString();
                string Query = "GetMultiClass";
                int hostchecktimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);

                MyWebClient client = new MyWebClient();
                client.LintTimeout = hostchecktimeout;
                client.Headers["Content-type"] = "application/json";

                #region Log

                StringWriter strWriter = new StringWriter();
                DataSet dsrequest = new DataSet();
                dsrequest = Serv.convertJsonStringToDataSet(request, "");
                dsrequest.WriteXml(strWriter);

                string LstrDetails = "<SELECT_REQUEST><URL>[<![CDATA[" + Session["travraysws_url"] + "/" + Session["travraysws_vdir"] + "/Rays.svc/"
                    + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (hostchecktimeout).ToString() + "</TIMEOUT><TIME>"
                    + "</TIME>" + strWriter.ToString() + "</SELECT_REQUEST>";


                DatabaseLog.LogData(Session["username"].ToString(), "S", "Formmulticlass", "multiclasss-Request", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                #endregion

                /****/
                strURLpath = ConfigurationManager.AppSettings["Multiclas"].ToString();
                if (availmulti == "" || availmulti == "null" || availmulti == "undefined")
                {

                    byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                    strResponse = System.Text.Encoding.ASCII.GetString(data);
                    /****/
                    #region Log

                    string ResTime = "AvailabityReqest" + DateTime.Now;
                    LstrDetails = "<AVAILRESPONSE><AIRPORTCATEGORY>" + strResponse + "</AIRPORTCATEGORY><URL>[<![CDATA[" + strURLpath + "/Rays.svc/"
                       + "]]>]</URL><QUERY>" + Query + "</QUERY><TIMEOUT>" + (hostchecktimeout).ToString() +
                       "</TIMEOUT><RESPONSE_TIME>" + ResTime + "</RESPONSE_TIME></AVAILRESPONSE>";


                    DatabaseLog.LogData(Session["username"].ToString(), "T", "Formmulticlass", "Multiclass Response - ", LstrDetails, Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());


                    #endregion

                    multiclass.Class_RS _availClassRes = JsonConvert.DeserializeObject<multiclass.Class_RS>(strResponse);


                    if (_availClassRes.Status.ResultCode == "1")
                    {
                        string jsoresults = JsonConvert.SerializeObject(_availClassRes).ToString();
                        MC_response[resultflag] = jsoresults;
                        MC_response[response] = "1";
                    }
                    else
                    {
                        MC_response[response] = "0";
                        if (_availClassRes.Status.Error != "")
                        {
                            MC_response[error] = _availClassRes.Status.Error.ToString();
                        }
                        else
                        {
                            MC_response[error] = "No Class Available";
                        }

                    }

                }
                else
                {
                    MC_response[response] = availmulti;
                    MC_response[resultflag] = Origin + "SpLitPResna" + Destination + "SpLitPResna" + depttime + "SpLitPResna" + ArrTime + "SpLitPResna" + ArrFliDet[4] + "SpLitPResna" + SClass + "SpLitPResna" + adtcount + "SpLitPResna" + chdcount +
                        "SpLitPResna" + infcount + "SpLitPResna" + PlatingCarrier + "SpLitPResnaRBDSpLitPResna" + false + "SpLitPResnaTokenSpLitPResnaBaseAmntSpLitPResnaGrossAmntSpLitPResna" + ConnectionFlag +
                        "SpLitPResna" + AirlineCategory + "SpLitPResnaFareBasisCodeSpLitPResna" + ArrFliDet[18] + "SpLitPResna" + ArrFliDet[19] + "SpLitPResna" + ArrFliDet[20] + "SpLitPResna" + ArrFliDet[21] +
                        "SpLitPResna" + ArrFliDet[22] + "SpLitPResna" + ArrFliDet[23] + "SpLitPResna" + ArrFliDet[24] + "SpLitPResnaSTUfareID" + "SpLitPResnaSTUservcharg" + "SpLitPResnaSTUincentive" + "SpLitPResnaSTUmarkup" + "SpLitPResnaSTUsegref";
                }
            }
            catch (Exception ex)
            {
                MC_response[response] = "0";
                MC_response[error] = "Problem occurred while getting class.";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Multiclass", "FormmulticlassRequest", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                return Json(new { Status = "", Message = "", Result = MC_response });
            }

            //return MC_response;
            return Json(new { Status = "", Message = "", Result = MC_response });
        }

        #endregion

        #region CAllendar Fare
        public ActionResult CalendarFare(string origin, string destination, string depdate, string retdate, string triptype)
        {
            string getTerminalType = string.Empty;// "W";
            string getIpAddress = string.Empty;// "1";
            string getSequenceId = string.Empty;// "1";
            string getTerminalID = string.Empty;// "TIQAJ010000103";
            string getUserName = string.Empty;// "sathees";

            string stu = string.Empty;
            string msg = string.Empty;
            string res = string.Empty;
            string lstrlowfare = string.Empty;
            try
            {
                if (Session["agentid"] == null)
                {
                    return Json(new { Status = "-1", Message = "", Result = "" });
                }

                if (Session["Availterminal"] == null || Session["Availterminal"].ToString() == "")
                {
                    Session.Add("Availterminal", Session["terminalid"].ToString());
                }
                else
                {
                    Session["Availterminal"] = Session["terminalid"].ToString();
                }

                Rays_service.RaysService _rays_servers = new Rays_service.RaysService();

                string lstrerror = "";
                string org = origin.Split('(')[1].Split(')')[0];// txtStartingCityCode.Text.Trim();
                string dest = destination.Split('(')[1].Split(')')[0];// txtReachingCityCode.Text.Trim();
                string originname = Base.Utilities.AirportcityName(origin);
                string destinationname = Base.Utilities.AirportcityName(destination);
                string getTriptType = triptype;
                string Depdate = depdate.ToString().ToUpper().Replace("%2F", "/").Trim();
                string Retdate = retdate.ToString().ToUpper().Replace("%2F", "/").Trim();
                string Date = DateTime.ParseExact(Depdate.ToString().Trim(), @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                string EndDate = (DateTime.ParseExact(Depdate.ToString().Trim(), @"dd\/MM\/yyyy", null).Date.AddDays(30)).ToString("yyyyMMdd");

                getTerminalID = Session["Availterminal"].ToString();
                getTerminalType = "W";
                getIpAddress = Session["ipAddress"].ToString();
                getSequenceId = Session["sequenceid"].ToString();
                getUserName = Session["username"].ToString();
                if (Retdate.ToString().Trim() == "")
                {
                    Retdate = Depdate;
                }
                TimeSpan tp = DateTime.ParseExact(Retdate.ToString().Trim(), @"dd\/MM\/yyyy", null).Date.Subtract(DateTime.ParseExact(Depdate, @"dd\/MM\/yyyy", null).Date);
                int diff = tp.Days;


                string xml = "<EVENT><DATE>" + Date + "</DATE><EndDate>" + EndDate + "</EndDate><org>" + org + "</org><dest>" + dest + "</dest><getTriptType>" + getTriptType + "</getTriptType><diff>" + diff + "</diff><getTerminalID>" + getTerminalID + "</getTerminalID></EVENT>";

                DatabaseLog.LogData(Session["username"].ToString(), "E", "SearchController", "CalendarFare", xml.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                DataSet dscalfare = _rays_servers.FetchCalenderAvailabilty(Date, EndDate, org, dest, getTriptType, diff, getTerminalID, getUserName,
                   getTerminalType, getIpAddress, Convert.ToDecimal(getSequenceId), ref lstrerror);

                if (dscalfare != null && dscalfare.Tables.Count > 0)
                {
                    DatabaseLog.LogData(Session["username"].ToString(), "E", "SearchController", "CalendarFare", "SUCCESS", Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                    //DatabaseLog.LogData(Session["username"].ToString(), "E", "SearchController", "CalendarFare", "RESULT", Session["agentid"].ToString(), Session["terminalid"].ToString(), Session["sequenceid"].ToString());

                    int index = 0;
                    string lstrAirline = "";
                    var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;

                    getTriptType = getTriptType.ToUpper();

                    #region LINQ's
                    var qry = from p in dscalfare.Tables[0].AsEnumerable()
                              group p by new
                              {
                                  Id = cal.GetWeekOfYear((DateTime)p["DATES"], CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                              }
                                  into gRP
                              select new
                              {
                                  Monday1 = getTriptType == "O" ? GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  "Monday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline) :

                                  GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  gRP.Select(DatesFare => (DateTime)DatesFare["DATESRET"]).ToArray(), "Monday", ref index,
                                 gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline),

                                  // Monday = lstrAirline,

                                  Tuesday1 = getTriptType == "O" ? GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  "Tuesday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline) :

                                  GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  gRP.Select(DatesFare => (DateTime)DatesFare["DATESRET"]).ToArray(), "Tuesday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline),

                                  //Tuesday = lstrAirline,

                                  Wednesday1 = getTriptType == "O" ? GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  "Wednesday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline) :

                                  GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  gRP.Select(DatesFare => (DateTime)DatesFare["DATESRET"]).ToArray(), "Wednesday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline),

                                  // Wednesday = lstrAirline,

                                  Thursday1 = getTriptType == "O" ? GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  "Thursday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline) :

                                  GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  gRP.Select(DatesFare => (DateTime)DatesFare["DATESRET"]).ToArray(), "Thursday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline),

                                  //Thursday = lstrAirline,

                                  Friday1 = getTriptType == "O" ? GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  "Friday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline) :

                                  GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  gRP.Select(DatesFare => (DateTime)DatesFare["DATESRET"]).ToArray(), "Friday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline),

                                  //Friday = lstrAirline,

                                  Saturday1 = getTriptType == "O" ? GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  "Saturday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline) :

                                  GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  gRP.Select(DatesFare => (DateTime)DatesFare["DATESRET"]).ToArray(), "Saturday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline),

                                  //Saturday = lstrAirline,

                                  Sunday1 = getTriptType == "O" ? GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  "Sunday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline) :

                                 GrpCalFare(gRP.Select(DatesFare => (DateTime)DatesFare["DATES"]).ToArray(),
                                  gRP.Select(DatesFare => (DateTime)DatesFare["DATESRET"]).ToArray(), "Sunday", ref index,
                                  gRP.Select(DatesFare => DatesFare["Deptime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["ArrTime"].ToString()).ToArray(),
                                  gRP.Select(DatesFare => (decimal)DatesFare["fare"]).ToArray(),
                                  gRP.Select(DatesFare => DatesFare["airline"].ToString()).ToArray(), ref lstrAirline),

                                  //Sunday = lstrAirline,

                              };
                    #endregion

                    try
                    {
                        var qry1 = (from p in dscalfare.Tables[0].AsEnumerable()
                                    where p["Fare"].ToString() != "" && p["Fare"].ToString() != "0"
                                    && p["Fare"].ToString() != "0.00"
                                    orderby p["Fare"] ascending
                                    select p["Fare"]).FirstOrDefault();

                        lstrlowfare = qry1.ToString();
                    }
                    catch (Exception)
                    {

                    }

                    if (qry.Count() > 0)
                    {
                        DataTable dt = Serv.ConvertToDataTable(qry);
                        StringBuilder strcalfare = new StringBuilder();
                        strcalfare.Append("<table class='Calender'  style='border-collapse:collapse'>");
                        strcalfare.Append("<tr class='Head' ><th colspan='7' >" + originname + " To " + destinationname + "</th></tr>");
                        strcalfare.Append("<tr class='Header'><th>Mon<span class='hidden-xs'>day</span></th><th>Tue<span class='hidden-xs'>sday</span></th><th>Wed<span class='hidden-xs'>nesday</span></th><th>Thu<span class='hidden-xs'>rsday</span></th><th>Fri<span class='hidden-xs'>day</span></th><th>Sat<span class='hidden-xs'>urday</span></th><th>Sun<span class='hidden-xs'>day</span></th></tr>");
                        //foreach (DataRow dr in dt.Rows)
                        int column = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strcalfare.Append("<tr>");
                            foreach (DataColumn dtc in dt.Columns)
                            {
                                string colname = dtc.ColumnName;
                                if (colname.Contains("1"))
                                {

                                    if (dt.Rows[i][colname].ToString() != "")
                                    {
                                        if (dt.Rows[i][colname].ToString() != "")
                                        {
                                            if (lstrlowfare != "" && dt.Rows[i][colname].ToString().Contains(lstrlowfare))
                                            {
                                                string Val = "";
                                                if (dt.Rows[i][colname].ToString().Split('~').Length > 4)
                                                { Val = dt.Rows[i][colname].ToString().Split('~')[4]; }
                                                else if (dt.Rows[i][colname].ToString().Split('~').Length > 2)
                                                { Val = dt.Rows[i][colname].ToString().Split('~')[2]; }

                                                strcalfare.Append("<td class='LowFare' >" +
                                                    ((dt.Rows[i][colname].ToString().Contains('~')) ? ((Val.ToString().Split('|')[1].Contains('-') ? Val.ToString().Split('|')[1].Split('-')[0] : Val.ToString().Split('|')[1]) != Depdate.ToString() ?
                                                   "<a href=# class='Fareothers' onclick='javascript:Getavailability(" + column + ")' >" : "<a href=# class='Fareothers11' onclick='javascript:Getavailability(" + column + ")' >")
                                                   + (((Val.ToString().Split('|')[1].Contains('-') ? Val.ToString().Split('|')[1].Split('-')[0] : Val.ToString().Split('|')[1]) != Depdate.ToString() ? dt.Rows[i][colname].ToString().Split('~')[0] : dt.Rows[i][colname].ToString().Split('~')[0].Replace(" ", "&nbsp;&nbsp;")) +
                                                   "</a>" + (Val.ToString().Split('|')[0] != "" ? "<span class='Passname' title='" + dt.Rows[i][colname].ToString().Split('~')[1] + "<br />"
                                                   + dt.Rows[i][colname].ToString().Split('~')[2] + "'><img style='float: right;' src='" + ConfigurationManager.AppSettings["FlightUrl"] +
                                               Val.ToString().Split('|')[0] + ".png" + "'>" : "") + "<br /><p class='Fareothers' style='display:none'>" +
                                                   (dt.Rows[i][colname].ToString().Split('~').Length > 1 ?
                                                   dt.Rows[i][colname].ToString().Split('~')[1] : "") +
                                                   "</p><br /><p class='Fareothers' style='display:none'>" + (dt.Rows[i][colname].ToString().Split('~').Length > 2 ?
                                                   dt.Rows[i][colname].ToString().Split('~')[2] : "") +
                                                   "</p><br /><p class='FareRule1' style='cursor:pointer' onclick='javascript:Getavailability(" + column + ")' >" + (dt.Rows[i][colname].ToString().Split('~').Length > 3 ?
                                                   dt.Rows[i][colname].ToString().Split('~')[3] : "")) : dt.Rows[i][colname].ToString()) +
                                                   "</p> </span><input type='hidden' value='" + Val + "' id='AvailValue" + column + "' /></td>");
                                            }
                                            else
                                            {
                                                string Val = "";
                                                if (dt.Rows[i][colname].ToString().Split('~').Length > 4)
                                                { Val = dt.Rows[i][colname].ToString().Split('~')[4]; }
                                                else if (dt.Rows[i][colname].ToString().Split('~').Length > 2)
                                                { Val = dt.Rows[i][colname].ToString().Split('~')[2]; }
                                                strcalfare.Append("<td>" +
                                                    ((dt.Rows[i][colname].ToString().Contains('~')) ? ((Val.ToString().Split('|')[1].Contains('-') ? Val.ToString().Split('|')[1].Split('-')[0] : Val.ToString().Split('|')[1]) != Depdate.ToString() ?
                                                    "<a href=# class='Fareothers' onclick='javascript:Getavailability(" + column + ")'  >" : "<a href=# class='Fareothers11' onclick='javascript:Getavailability(" + column + ")'  >")
                                                    + (((Val.ToString().Split('|')[1].Contains('-') ? Val.ToString().Split('|')[1].Split('-')[0] : Val.ToString().Split('|')[1]) != Depdate.ToString() ? dt.Rows[i][colname].ToString().Split('~')[0] : dt.Rows[i][colname].ToString().Split('~')[0].Replace(" ", "&nbsp;&nbsp;")) +
                                                    "</a>" + (Val.ToString().Split('|')[0] != "" ? "<span class='Passname' title='" + dt.Rows[i][colname].ToString().Split('~')[1] + "<br />"
                                                    + dt.Rows[i][colname].ToString().Split('~')[2] + "'><img style='float: right;' src='" + ConfigurationManager.AppSettings["FlightUrl"] +
                                                    Val.Split('|')[0] + ".png" + "'>" : "") + "<br /><p class='Fareothers' style='display:none'>" +
                                                    (dt.Rows[i][colname].ToString().Split('~').Length > 1 ?
                                                    dt.Rows[i][colname].ToString().Split('~')[1] : "") +
                                                    "</p><br /><p class='Fareothers' style='display:none'>" + (dt.Rows[i][colname].ToString().Split('~').Length > 2 ?
                                                    dt.Rows[i][colname].ToString().Split('~')[2] : "") +
                                                    "</p><br /><a href=# class='FareRule1' onclick='javascript:Getavailability(" + column + ")' >" + (dt.Rows[i][colname].ToString().Split('~').Length > 3 ?
                                                    dt.Rows[i][colname].ToString().Split('~')[3] : "")) : dt.Rows[i][colname].ToString()) +
                                                    "</a></span><input type='hidden' value='" + Val + "' id='AvailValue" + column + "' /></td>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strcalfare.Append("<td></td>");
                                    }

                                }

                                else
                                {

                                    strcalfare.Append("<td style='display:none'></td>");
                                }
                                column++;
                            }
                            strcalfare.Append("</tr>");

                        }
                        strcalfare.Append("</table>");
                        strcalfare.Append("<div class='divNote' ><p >Note: TravRays provides calender availability for the given period to see cheapest fare easily. Fares might be changed at the time of booking. All fares are in currency " + Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString() + ".</p></div>");

                        stu = "01";
                        res = strcalfare.ToString(); //JsonConvert.SerializeObject(dt);
                    }
                }
                else
                {
                    stu = "00";
                    msg = "No records found.";
                }
            }
            catch (Exception ex)
            {
                stu = "00";
                msg = "Problem occured while processing (#05).";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "SearchController", "CalendarFare", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
            }
            return Json(new { Status = stu, Message = msg, Result = res, lowFr = lstrlowfare });
        }

        public string GrpCalFare(DateTime[] date, string day, ref int index, string[] deptime, string[] arrtime, decimal[] fare,
         string[] airline, ref string Airline)
        {
            string returnvalue = "";
            if (index != 0 && date.Length > index && date[index].DayOfWeek.ToString().ToUpper() != day.ToUpper())
            {
                for (int ldate = index; ldate < date.Length; ldate++)
                {
                    index = ldate;
                    if (date[index].DayOfWeek.ToString().ToUpper() == day.ToUpper())
                        break;
                }
            }
            if (date.Length > index && date[index].DayOfWeek.ToString().ToUpper() == day.ToUpper())
            {
                returnvalue = fare[index] == 0 ? (date[index].ToString("dd MMM") + "~N/A") :
                    (date[index].ToString("dd MMM") + "~Dept  :  " + deptime[index] + "~Arr    :  "
                    + arrtime[index] + "~Fare  :  " + (fare[index]) + " ");
                Airline = (fare[index] == 0 ? "" : airline[index]) + "|" + date[index].ToString("dd/MM/yyyy");
                index++;
            }
            index = day == "Sunday" ? 0 : index;
            Airline = string.IsNullOrEmpty(returnvalue) ? "" : "~" + Airline;
            returnvalue = returnvalue + Airline;
            return returnvalue;
        }

        public string GrpCalFare(DateTime[] date, DateTime[] Rdate, string day, ref int index, string[] deptime, string[] arrtime, decimal[] fare,
           string[] airline, ref string Airline)
        {
            string returnvalue = "";
            if (index != 0 && date.Length > index && date[index].DayOfWeek.ToString().ToUpper() != day.ToUpper())
            {
                for (int ldate = index; ldate < date.Length; ldate++)
                {
                    index = ldate;
                    if (date[index].DayOfWeek.ToString().ToUpper() == day.ToUpper())
                        break;
                }
            }
            if (date.Length > index && date[index].DayOfWeek.ToString().ToUpper() == day.ToUpper())
            {
                returnvalue = fare[index] == 0 ? (date[index].ToString("dd MMM") +
                    ("-" + Rdate[index].ToString("dd MMM")) + "~N/A") :
                    (date[index].ToString("dd MMM") + ("-" + Rdate[index].ToString("dd MMM"))
                    + "~Dept  :  " + deptime[index] + "~Arr    :  "
                    + arrtime[index] + "~Fare  :  " + (fare[index]) + " ");
                Airline = (fare[index] == 0 ? "" : airline[index]) + "|" + date[index].ToString("dd/MM/yyyy") + "-" + Rdate[index].ToString("dd/MM/yyyy");
                index++;
            }
            index = day == "Sunday" ? 0 : index;
            Airline = string.IsNullOrEmpty(returnvalue) ? "" : "~" + Airline;
            returnvalue = returnvalue + Airline;
            return returnvalue;
        }

        #endregion

        public ActionResult GetCorporateCode(string AirlineCodes, string strClientId)
        {
            ArrayList arryAvail = new ArrayList();
            arryAvail.Add("");
            arryAvail.Add("");
            arryAvail.Add("");
            arryAvail.Add("");
            arryAvail.Add("");
            arryAvail.Add("");
            arryAvail.Add("");
            int Array_Error = 0;
            int Corpcode = 1;
            int Retailcode = 2;
            int SMEFare = 3;
            int MarineFare = 4;
            int SpecFare = 5;
            DataTable dtcorp = new DataTable();

            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString().ToUpper().Trim() : "";
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            string strBranchId = Session["branchid"] != null ? Session["branchid"].ToString() : "";
            string TerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strresult = string.Empty;
            string StrResponse = string.Empty;
            string strErrorMsg = string.Empty;
            string strResultcode = string.Empty;

            string strErrMsg = string.Empty;
            try
            {
                if (strAgentID == "" || strTerminalId == "" || strUserName == "" || strPosID == "" || strPosTID == "")
                {
                    strErrMsg = "Session Expire!.";
                    return Json(new { Error = strErrMsg, Status = "-1", Results = arryAvail });
                }


                string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
                string strClientType = Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : "";

                DataSet dsCorpCode = new DataSet();
                STSTRAVRAYS.Rays_service.RaysService WebService = new STSTRAVRAYS.Rays_service.RaysService();
                WebService.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                if (TerminalType == "T" && strClientId != "")
                {
                    strAgentID = strClientId.Substring(0, (strClientId.Length - 2));
                    strTerminalId = strClientId;
                }
                string Xmldata = "<REQUEST><CLIENTID>" + strAgentID + "</CLIENTID><TERMINALID>" + strTerminalId + "</TERMINALID><POSTID>" + Session["POS_TID"].ToString() + "</POSTID><USERNAME>" + Session["username"].ToString() + "</USERNAME></REQUEST><RESPONSE><URL>" + WebService.Url + "</URL></RESPONSE>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "SearchController.cs", "GetCorporateCode", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());


                bool JsonSelect = WebService.Getcorporatecode(strAgentID, strTerminalId, strUserName, Ipaddress, Convert.ToDecimal(sequnceID), TerminalType, ref strresult, ref strErrorMsg);


                DatabaseLog.LogData(Session["username"].ToString(), "E", "SearchController.cs", "GetCorporateCode", StrResponse, Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                if (JsonSelect == true)
                {
                    StrResponse = strresult;
                    if (!string.IsNullOrEmpty(StrResponse))
                        dsCorpCode = Newtonsoft.Json.JsonConvert.DeserializeObject<DataSet>(StrResponse);
                }
                else
                {
                    if (!string.IsNullOrEmpty(strErrorMsg)) { strErrMsg = strErrorMsg; }
                    else
                    {
                        strErrMsg = "Unable to fetch corporate code.Please contact support team";
                    }

                    return Json(new { Error = strErrMsg, Status = "00", Results = arryAvail });
                }


                if (dsCorpCode == null || dsCorpCode.Tables.Count <= 0 || dsCorpCode.Tables[0].Rows.Count <= 0)
                {
                    strErrMsg = "No corporate code is assigned. Please contact support team";
                    return Json(new { Error = strErrMsg, Status = "00", Results = arryAvail });
                }

                string strcorp = "0"; string strRetail = "0"; string strSME = "0"; string strMarine = "0";

                StringBuilder CorporateDetailsDesign = new StringBuilder();
                int TempCount = 0;
                var qrycorpAll = (from p in dsCorpCode.Tables[0].AsEnumerable()
                                  select new
                                  {
                                      CORPORATE_RETAIL = p.Field<string>("CORPORATE_RETAIL")
                                  }).Distinct();
                if (qrycorpAll.Count() > 0)
                {

                    string strPath = Server.MapPath("~/XML/AirlineNames.xml");
                    DataSet dsAirways = new DataSet(); // ((DataSet)Session["MASTERALLDATA"]);
                    DataSet dsAirways1 = new DataSet();
                    dsAirways1.ReadXml(strPath);

                    var availaircode = (from p in dsAirways1.Tables[0].AsEnumerable()
                                        select new
                                        {
                                            AIRLINE_CODE = p.Field<string>("_CODE"),
                                            AIRLINE_NAME = p.Field<string>("_NAME")
                                        }).Distinct();

                    DataTable Allaircodes = Serv.ConvertToDataTable(availaircode);
                    Allaircodes.TableName = "AIRLINES";
                    dsAirways.Tables.Add(Allaircodes);

                    DataTable dtCorpAirlineAll = Serv.ConvertToDataTable(qrycorpAll);
                    for (int AirCorpAll = 0; AirCorpAll < dtCorpAirlineAll.Rows.Count; AirCorpAll++)
                    {
                        CorporateDetailsDesign = new StringBuilder();
                        string NameOfCorporate = string.Empty;
                        string NameOfAir = string.Empty;
                        string TypeOfName = string.Empty;
                        if (dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "C")
                        {
                            NameOfCorporate = "gdvCorporateDetails_ddlCorporateName_";
                            NameOfAir = "gdvCorporateDetails_Airline_";
                            TypeOfName = "Corporate Name"; strcorp = "1";
                        }
                        else if (dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "R" || dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "RC")
                        {
                            NameOfCorporate = "gdvRetailsDetails_ddlCorporateName_";
                            NameOfAir = "gdvRetailsDetails_Airline_";
                            TypeOfName = "Retail Name"; strRetail = "1";
                        }
                        else if (dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "U" || dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "UC" || dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "M" || dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "MC")
                        {
                            NameOfCorporate = "gdvSMEDetails_ddlCorporateName_";
                            NameOfAir = "gdvSMEDetails_Airline_";
                            TypeOfName = "SME Name"; strSME = "1";
                        }
                        else if (dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "G" || dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "GC")
                        {
                            NameOfCorporate = "gdvMarineDetails_ddlCorporateName_";
                            NameOfAir = "gdvMarineDetails_Airline_";
                            TypeOfName = "Marine Name"; strMarine = "1";
                        }
                        string[] strAir = AirlineCodes.Split(',');
                        int AvailCorp = 0;
                        if (!string.IsNullOrEmpty(AirlineCodes) && strAir.Length > 0 && dsCorpCode != null && dsCorpCode.Tables.Count > 0 && dsCorpCode.Tables[0].Rows.Count > 0)
                        {
                            Session["dsCorpCode"] = dsCorpCode;
                            CorporateDetailsDesign.Append("<table id='" + NameOfCorporate.Split('_')[0] + "' cellpadding='2' class='gdvCorporateDetails' width='100%'>");
                            CorporateDetailsDesign.Append("	<thead>");
                            CorporateDetailsDesign.Append("<tr> ");
                            CorporateDetailsDesign.Append("<th data-class='expand' data-sort-ignore='true'>");
                            CorporateDetailsDesign.Append("	<span title='table sorted by this column on load'>Airline</span>");
                            CorporateDetailsDesign.Append("</th>");
                            CorporateDetailsDesign.Append("<th data-hide='phone,tablet' data-sort-ignore='true' style='padding-left: 100px;' >" + TypeOfName + " ");
                            CorporateDetailsDesign.Append("</th>");
                            CorporateDetailsDesign.Append("</tr>");
                            CorporateDetailsDesign.Append("</thead>");
                            CorporateDetailsDesign.Append("<tbody>");
                            int Index = 0;
                            for (int AirCorpCode = 0; AirCorpCode < strAir.Length; AirCorpCode++)
                            {
                                if (!string.IsNullOrEmpty(strAir[AirCorpCode]))
                                {
                                    var qrycorp = (from p in dsCorpCode.Tables[0].AsEnumerable()
                                                   where p.Field<string>("CORP_AIRLINE").ToUpper().Trim() == strAir[AirCorpCode].ToUpper().Trim()
                                                    && p.Field<string>("CORPORATE_RETAIL").ToUpper().Trim() == dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim()
                                                   select p).Distinct();

                                    if (qrycorp.Count() > 0)
                                    {
                                        dtcorp = qrycorp.CopyToDataTable();
                                        int CorpCodeTemp = 0;
                                        for (int Aircorporate = 0; Aircorporate < dtcorp.Rows.Count; Aircorporate++)
                                        {
                                            if (!string.IsNullOrEmpty(dtcorp.Rows[Aircorporate]["CORP_CODE"].ToString()) && !string.IsNullOrEmpty(dtcorp.Rows[Aircorporate]["CORP_NAME"].ToString()))
                                            {
                                                string officeid = ((dtcorp.Columns.Contains("OFFICEID") == true) ? dtcorp.Rows[Aircorporate]["OFFICE_ID"].ToString() : (dtcorp.Columns.Contains("Office_ID") == true) ? dtcorp.Rows[Aircorporate]["OFFICE_ID"].ToString() : "");
                                                if (CorpCodeTemp == 0)
                                                {
                                                    CorpCodeTemp = 1;
                                                    AvailCorp = 1;
                                                    CorporateDetailsDesign.Append("<tr>");
                                                    CorporateDetailsDesign.Append("<td style='width:30%'><span style='display: none;' id='" + NameOfAir + Index + "'>" + strAir[AirCorpCode].ToUpper().Trim() + "</span><span id='1" + NameOfAir + Index + "'>" + airlineName(strAir[AirCorpCode].ToUpper().Trim(), dsAirways) + "</span></td>");
                                                    CorporateDetailsDesign.Append("<td align='center'>");
                                                    CorporateDetailsDesign.Append("<select style='width:200px;height: 35px;margin-top: 10px;' name='select' id='" + NameOfCorporate + Index + "'>");
                                                    CorporateDetailsDesign.Append("<option selected='selected' value='0'>--select--</option>");
                                                }
                                                CorporateDetailsDesign.Append("<option value='" + dtcorp.Rows[Aircorporate]["CORP_CODE"].ToString() + '~' + officeid + '~' + dtcorp.Rows[Aircorporate]["CORPORATE_RETAIL"].ToString() + "'>" + dtcorp.Rows[Aircorporate]["CORP_NAME"].ToString() + "</option>");
                                                TempCount = 1;
                                            }
                                        }
                                        if (CorpCodeTemp == 1)
                                            CorporateDetailsDesign.Append("</select></td></tr>");
                                        Index++;
                                    }
                                }
                            }
                            CorporateDetailsDesign.Append("</tbody>");
                            CorporateDetailsDesign.Append("</table>");
                        }
                        else if (dsCorpCode != null && dsCorpCode.Tables.Count > 0 && dsCorpCode.Tables[0].Rows.Count > 0)
                        {
                            dtcorp = dsCorpCode.Tables[0];
                            Session["dsCorpCode"] = dsCorpCode;
                            var qrycorpAir = (from p in dsCorpCode.Tables[0].AsEnumerable()
                                              where p.Field<string>("CORPORATE_RETAIL").ToUpper().Trim() == dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim()
                                              select new
                                              {
                                                  CORP_AIRLINE = p.Field<string>("CORP_AIRLINE")
                                              }).Distinct();
                            if (qrycorpAir.Count() > 0)
                            {
                                DataTable dtCorpAirline = Serv.ConvertToDataTable(qrycorpAir);

                                CorporateDetailsDesign.Append("<table id='" + NameOfCorporate.Split('_')[0] + "' cellpadding='2' class='gdvCorporateDetails' width='100%'>");
                                CorporateDetailsDesign.Append("<thead>");
                                CorporateDetailsDesign.Append("<tr>");
                                CorporateDetailsDesign.Append("<th data-class='expand' data-sort-ignore='true'>");
                                CorporateDetailsDesign.Append("<span title='table sorted by this column on load'>Airline</span>");
                                CorporateDetailsDesign.Append("</th>");
                                CorporateDetailsDesign.Append("<th data-hide='phone,tablet' data-sort-ignore='true' style='padding-left: 100px;'>" + TypeOfName + "   ");
                                CorporateDetailsDesign.Append("</th>");
                                CorporateDetailsDesign.Append("</tr>");
                                CorporateDetailsDesign.Append("</thead>");
                                CorporateDetailsDesign.Append("<tbody>");
                                int Index = 0;
                                for (int AirCorpCode = 0; AirCorpCode < dtCorpAirline.Rows.Count; AirCorpCode++)
                                {
                                    if (!string.IsNullOrEmpty(dtCorpAirline.Rows[AirCorpCode]["CORP_AIRLINE"].ToString().Trim()))
                                    {
                                        var qrycorp = (from p in dsCorpCode.Tables[0].AsEnumerable()
                                                       where p.Field<string>("CORP_AIRLINE").ToUpper().Trim() == dtCorpAirline.Rows[AirCorpCode]["CORP_AIRLINE"].ToString().ToUpper().Trim()
                                                       && p.Field<string>("CORPORATE_RETAIL").ToUpper().Trim() == dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim()
                                                       select p).Distinct();

                                        if (qrycorp.Count() > 0)
                                        {
                                            dtcorp = qrycorp.CopyToDataTable();
                                            int CorpCodeTemp = 0;
                                            for (int Aircorporate = 0; Aircorporate < dtcorp.Rows.Count; Aircorporate++)
                                            {
                                                if (!string.IsNullOrEmpty(dtcorp.Rows[Aircorporate]["CORP_CODE"].ToString()) && !string.IsNullOrEmpty(dtcorp.Rows[Aircorporate]["CORP_NAME"].ToString()))
                                                {
                                                    string officeid = ((dtcorp.Columns.Contains("OFFICEID") == true) ? dtcorp.Rows[Aircorporate]["OFFICE_ID"].ToString() : (dtcorp.Columns.Contains("Office_ID") == true) ? dtcorp.Rows[Aircorporate]["OFFICE_ID"].ToString() : "");
                                                    if (CorpCodeTemp == 0)
                                                    {
                                                        CorpCodeTemp = 1;
                                                        AvailCorp = 1;
                                                        CorporateDetailsDesign.Append("<tr>");
                                                        CorporateDetailsDesign.Append("<td style='width:30%'><span style='display: none;' id='" + NameOfAir + Index + "'>" + dtCorpAirline.Rows[AirCorpCode]["CORP_AIRLINE"].ToString().ToUpper().Trim() + "</span><span id='1" + NameOfAir + Index + "'>" + airlineName(dtCorpAirline.Rows[AirCorpCode]["CORP_AIRLINE"].ToString().Trim(), dsAirways) + "</span></td>");
                                                        CorporateDetailsDesign.Append("<td align='center' >");
                                                        CorporateDetailsDesign.Append("<select style='width:200px;height: 35px;margin-top: 10px;' name='select' id='" + NameOfCorporate + Index + "'>");
                                                        CorporateDetailsDesign.Append("<option selected='selected' value='0'>--select--</option>");
                                                    }
                                                    CorporateDetailsDesign.Append("<option value='" + dtcorp.Rows[Aircorporate]["CORP_CODE"].ToString() + '~' + officeid + '~' + dtcorp.Rows[Aircorporate]["CORPORATE_RETAIL"].ToString() + "'>" + dtcorp.Rows[Aircorporate]["CORP_NAME"].ToString() + "</option>");
                                                    TempCount = 1;
                                                }
                                            }
                                            if (CorpCodeTemp == 1)
                                                CorporateDetailsDesign.Append("</select></td></tr>");
                                            Index++;
                                        }
                                    }
                                }
                                CorporateDetailsDesign.Append("</tbody>");
                                CorporateDetailsDesign.Append("</table>");
                            }
                        }
                        if (dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "C" && AvailCorp == 1)
                            arryAvail[Corpcode] = CorporateDetailsDesign.ToString();
                        else if ((dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "R" || dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "RC") && AvailCorp == 1)
                            arryAvail[Retailcode] = CorporateDetailsDesign.ToString();
                        if ((dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "U" || dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "UC" || dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "M" || dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "MC") && AvailCorp == 1)
                            arryAvail[SMEFare] = CorporateDetailsDesign.ToString();
                        else if ((dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "G" || dtCorpAirlineAll.Rows[AirCorpAll]["CORPORATE_RETAIL"].ToString().ToUpper().Trim() == "GC") && AvailCorp == 1)
                            arryAvail[MarineFare] = CorporateDetailsDesign.ToString();

                        strResultcode = "01";
                    }
                }

                if (TempCount == 0)
                {
                    strErrMsg = "No corporate code is assigned. Please contact support team";
                    return Json(new { Error = strErrMsg, Status = strResultcode, Results = arryAvail });
                }


            }
            catch (Exception ex)
            {
                strErrMsg = "Unable to get corporate fare credentials(#05)";
                strResultcode = "00";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "SearchController.cs", "GetCorporateCode", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
            }
            return Json(new { Error = strErrMsg, Status = strResultcode, Results = arryAvail });
        }

        public ActionResult GetLabourthread(string Origincode, string Destinationcity, string strDepartureDate, string strRetDate, string strTrip, string Class, string segmenttype, string AppCurrency, string ClientID, string Airlinecode)
        {
            ArrayList arryAvail = new ArrayList();
            arryAvail.Add("");
            arryAvail.Add("");
            arryAvail.Add("");
            arryAvail.Add("");
            int Resultcode = 0;
            int Threadkey = 1;
            int FSCThread = 2;
            int ErrorMsg = 3;
            DataSet dsPricingCode = new DataSet();
            string strAgentID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strTerminalId = Session["POS_TID"] != null ? Session["POS_TID"].ToString().ToUpper().Trim() : "";
            string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
            string Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
            string sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
            string strBranchId = Session["branchid"] != null ? Session["branchid"].ToString() : "";
            string TerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
            string strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
            string strresult = string.Empty;
            string StrResponse = string.Empty;
            string strErrorMsg = string.Empty;
            string strResultcode = string.Empty;
            string Returnthread = string.Empty;
            string strErrMsg = string.Empty;
            string strDepdate = string.Empty;
            try
            {
                string Labourthrd = Session["Labourthread"].ToString();
                string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
                string strClientType = Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : "";

                STSTRAVRAYS.InplantService.Inplantservice _inplantservice = new STSTRAVRAYS.InplantService.Inplantservice();
                _inplantservice.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                if (TerminalType == "T" && ClientID != "")
                {
                    ClientID = ClientID.Substring(0, (ClientID.Length - 2));
                    strTerminalId = ClientID;
                }
                else
                {
                    ClientID = Session["POS_ID"].ToString();
                    strTerminalId = Session["POS_TID"].ToString();
                }

                InplantService.Inplantservice _rays = new InplantService.Inplantservice();
                _rays.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                strDepdate = DateTime.ParseExact(strDepartureDate, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");

                for (var i = 0; i < Labourthrd.Split('|').Count(); i++)
                {
                    dsPricingCode = _rays.FetchPricingCodeDetails(ClientID, strTerminalId, Origincode, Destinationcity, segmenttype, strTrip, "", Class, Labourthrd.Split('|')[i], "0", Session["username"].ToString(), "W", Convert.ToDecimal(Session["sequenceid"]), Session["ipaddress"].ToString(), ref strErrorMsg, Airlinecode, strDepdate, "L", "");

                    string Xmldata = "<REQUEST><CLIENTID>" + ClientID + "</CLIENTID><POSTID>" + strTerminalId + "</POSTID><ORIGIN>" + Origincode + "</ORIGIN><DESTINATION>" + Destinationcity
                                       + "</DESTINATION><SEGMENTTYPE>" + segmenttype + "</SEGMENTTYPE><TRIPTYPE>" + strTrip + "</TRIPTYPE><CLASS>" + Class + "</CLASS><STOCKTYPE>" + Labourthrd.Split('|')[i]
                                       + "</STOCKTYPE><USERNAME>" + Session["username"].ToString() + "</USERNAME></REQUEST><RESPONSE><DATA>" + dsPricingCode.GetXml() + "</DATA><ERROR>" + strErrorMsg + "</ERROR></RESPONSE>";

                    DatabaseLog.LogData(Session["username"].ToString(), "E", "GetLabourcode", "GetLabourcode", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());


                    if (dsPricingCode != null && dsPricingCode.Tables.Count > 0 && dsPricingCode.Tables[0].Rows.Count > 0)
                    {
                        var Codedetails = (from code in dsPricingCode.Tables[0].AsEnumerable()
                                           group code by code["TC_AIRLINE"].ToString() into c
                                           select c);

                        if (Codedetails.Count() > 0) /* Default Thread came from Threadkey so need to add from additional */
                        {
                            DataTable dtd = new DataTable();
                            dtd = Serv.ConvertToDataTable(Codedetails);
                            for (int j = 0; j < dtd.Rows.Count; j++)
                            {
                                Returnthread += Labourthrd.Split('|')[i] + "PR_THREAD" + dtd.Rows[j]["Key"].ToString() + "|";
                            }
                        }
                    }

                }

                if (Returnthread != null && Returnthread != "")
                {
                    arryAvail[Resultcode] = "1";
                    arryAvail[Threadkey] = Returnthread.TrimEnd('|');
                    arryAvail[FSCThread] = Returnthread.TrimEnd('|');
                }
                else
                {
                    arryAvail[Resultcode] = "0";
                    arryAvail[ErrorMsg] = "There is no labour fare available for this sector";
                }
            }
            catch (Exception ex)
            {
                arryAvail[ErrorMsg] = "Unable to get labour fare for this sector(#05)";
                arryAvail[Resultcode] = "0";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "SearchController.cs", "GetLabourCode", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
            }
            return Json(new { Results = arryAvail });
        }

        public ActionResult GetPricingCode(string Origincode, string Destinationcity, string strDepartureDate, string strRetDate, string strTrip, string Class, string segmenttype, string AppCurrency, string ClientID, string Airlinecode)
        {
            ArrayList array_Response = new ArrayList();
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            int error = 0;
            int Threadkey = 1;
            int D_Threadkey = 2;
            int FSCthread = 3;
            int D_Fscthread = 4;
            int Mcitythread = 5;
            DataSet dsPricingCode = new DataSet();
            string Returnthread = string.Empty;
            string Stat = "0";
            string strDepdate = string.Empty;
            string OriginCity = string.Empty;
            string DestCity = string.Empty;
            try
            {
                string Air_Thread = ConfigurationManager.AppSettings["Air_Thread"].ToString();
                string Promothread = ConfigurationManager.AppSettings["Addl_Promocodethread"].ToString();
                string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                strDepdate = DateTime.ParseExact(strDepartureDate, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                string strClientID = string.Empty; string strClientTerminalID = string.Empty;
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
                //if (Airlinecode == "")
                //{
                OriginCity = strTrip == "M" ? Origincode.Split(',')[0] : Origincode;
                DestCity = strTrip == "M" ? Destinationcity.Split(',')[Destinationcity.Split(',').Length - 1] : Destinationcity;

                if (Promothread != "")
                {
                    for (int i = 0; i < Promothread.Split(',').Length; i++)
                    {
                        if (Promothread.Split(',')[i] != "" && ((Session["threadkey"].ToString().Contains(Promothread.Split(',')[i]) && segmenttype == "I") || (Session["threadkey_dom"].ToString().Contains(Promothread.Split(',')[i]) && segmenttype == "D")))
                        {
                            string clntid = (ClientID != "" && strTerminalType == "T" ? strClientID : Session["pos_id"].ToString());
                            string terminalid = (ClientID != "" && strTerminalType == "T" ? strClientTerminalID : Session["POS_TID"].ToString());
                            InplantService.Inplantservice _rays = new InplantService.Inplantservice();
                            _rays.Url = ConfigurationManager.AppSettings["TOPUP_APPS_INPLANT_SERVICE"].ToString();
                            string Promoerr = string.Empty;
                            string classreq = string.Empty;
                            string treq = string.Empty;
                            string Category = Promothread.Split(',')[i].ToString();

                            dsPricingCode = _rays.FetchPricingCodeDetails(clntid, terminalid, OriginCity, DestCity, segmenttype, strTrip, "", Class, Category, "0", Session["username"].ToString(), "W", Convert.ToDecimal(Session["sequenceid"]), Session["ipaddress"].ToString(), ref Promoerr, Airlinecode, strDepdate, "", "");

                            string Xmldata = "<REQUEST><CLIENTID>" + clntid + "</CLIENTID><POSTID>" + terminalid + "</POSTID><ORIGIN>" + Origincode + "</ORIGIN><DESTINATION>" + Destinationcity
                                    + "</DESTINATION><SEGMENTTYPE>" + segmenttype + "</SEGMENTTYPE><TRIPTYPE>" + strTrip + "</TRIPTYPE><CLASS>" + Class + "</CLASS><STOCKTYPE>" + Category
                                    + "</STOCKTYPE><USERNAME>" + Session["username"].ToString() + "</USERNAME></REQUEST><RESPONSE><DATA>" + dsPricingCode.GetXml() + "</DATA><ERROR>" + Promoerr + "</ERROR></RESPONSE>";

                            DatabaseLog.LogData(Session["username"].ToString(), "E", "GetPricingCode", "GetPricingCode-Flightsearch", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                            if (dsPricingCode != null && dsPricingCode.Tables.Count > 0 && dsPricingCode.Tables[0].Rows.Count > 0)
                            {
                                var Codedetails = (from code in dsPricingCode.Tables[0].AsEnumerable()
                                                   group code by code["FTC_TICKET_OFFICE_ID"].ToString() into c
                                                   select c);

                                if (Codedetails.Count() > 0) /* Default Thread came from Threadkey so need to add from additional */
                                {
                                    DataTable dtd = new DataTable();
                                    dtd = Serv.ConvertToDataTable(Codedetails);
                                    for (int j = 0; j < dtd.Rows.Count; j++)
                                    {
                                        Returnthread += Promothread.Split(',')[i] + "PR_THREAD" + dtd.Rows[j]["Key"].ToString() + "|";
                                    }
                                }
                            }
                        }
                    }
                }
                Stat = "1";
                if (strTrip != "M")
                {
                    array_Response[Threadkey] = (Session["threadkey"] != null ? Session["threadkey"].ToString() + ((Air_Thread != "") ? "|" + Air_Thread + "" : "") : "") + (Returnthread != "" ? "|" + Returnthread.TrimEnd('|') : "");
                    array_Response[D_Threadkey] = Session["threadkey_dom"].ToString() + (Returnthread != "" ? "|" + Returnthread.TrimEnd('|') : "");
                    array_Response[FSCthread] = Session["fscthreadkey"].ToString() + (Returnthread != "" ? "|" + Returnthread.TrimEnd('|') : "");
                    array_Response[D_Fscthread] = Session["fscthreadkey_dom"].ToString() + (Returnthread != "" ? "|" + Returnthread.TrimEnd('|') : "");
                }
                else
                {
                    array_Response[Mcitythread] = Session["multithreadkey"].ToString() + (Returnthread != "" ? "|" + Returnthread.TrimEnd('|') : "");
                }
                //}
                //else
                //{
                //    Stat = "1";
                //    array_Response[Threadkey] = (Session["threadkey"] != null ? Session["threadkey"].ToString() + ((Air_Thread != "") ? "|" + Air_Thread + "" : "") : "");
                //    array_Response[D_Threadkey] = Session["threadkey_dom"].ToString();
                //    array_Response[FSCthread] = Session["fscthreadkey"].ToString();
                //    array_Response[D_Fscthread] = Session["fscthreadkey_dom"].ToString();
                //}
            }
            catch (Exception ex)
            {
                Stat = "0";
                string Xmldata = "<EVENT>[<![CDATA[" + ex.ToString() + "]]>]</EVENT>";
                DatabaseLog.LogData_wasc(Session["username"].ToString(), "X", "GetPricingCode", "GetPricingCode-List", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            return Json(new { Status = Stat, Message = "", Result = array_Response });
        }

        public ActionResult Getpricingcodecount(string Origincode, string Destinationcity, string strDepartureDate, string strRetDate, string strTrip, string Class, string segmenttype, string AppCurrency, string ClientID, string Airlinecode)
        {
            ArrayList array_Response = new ArrayList();
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            int error = 0;
            int Threadkey = 1;
            int D_Threadkey = 2;
            int FSCthread = 3;
            int D_Fscthread = 4;
            DataSet dstPricingCode = new DataSet();
            DataSet dsPricingCode = new DataSet();
            string Returnthread = string.Empty;
            string Stat = "0";
            try
            {
                string Promothread = ConfigurationManager.AppSettings["Addl_Promocodethread"].ToString();
                string Dir_Promothread = ConfigurationManager.AppSettings["Promocodethread"].ToString();
                string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                string strClientID = string.Empty; string strClientTerminalID = string.Empty;
                string strDepdate = DateTime.ParseExact(strDepartureDate, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
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
                //if (Airlinecode == "")
                //{
                string add_thread = string.Empty;
                string Additionalthread = string.Empty;
                if (Promothread != "")
                {
                    for (int i = 0; i < Promothread.Split(',').Length; i++)
                    {
                        if (Promothread.Split(',')[i] != "" && ((Session["threadkey"].ToString().Contains(Promothread.Split(',')[i]) && segmenttype == "I") || (Session["threadkey_dom"].ToString().Contains(Promothread.Split(',')[i]) && segmenttype == "D")))
                        {
                            string clntid = (ClientID != "" && strTerminalType == "T" ? strClientID : Session["pos_id"].ToString());
                            string terminalid = (ClientID != "" && strTerminalType == "T" ? strClientTerminalID : Session["POS_TID"].ToString());
                            InplantService.Inplantservice _rays = new InplantService.Inplantservice();
                            _rays.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                            string Promoerr = string.Empty;
                            string classreq = string.Empty;
                            string treq = string.Empty;
                            string Category = Promothread.Split(',')[i].ToString();
                            dsPricingCode = _rays.FetchCodeCountDetails(clntid, terminalid, Origincode, Destinationcity, segmenttype, strTrip, "", Class, Category, "0", Session["username"].ToString(), "W", Convert.ToDecimal(Session["sequenceid"]), Session["ipaddress"].ToString(), ref Promoerr, Airlinecode);
                            string Xmldata = "<REQUEST><CLIENTID>" + clntid + "</CLIENTID><POSTID>" + terminalid + "</POSTID><ORIGIN>" + Origincode + "</ORIGIN><DESTINATION>" + Destinationcity
                                    + "</DESTINATION><SEGMENTTYPE>" + segmenttype + "</SEGMENTTYPE><TRIPTYPE>" + strTrip + "</TRIPTYPE><CLASS>" + Class + "</CLASS><TRAVELDATE>" + strDepdate + "</TRAVELDATE><STOCKTYPE>" + Category
                                    + "</STOCKTYPE><USERNAME>" + Session["username"].ToString() + "</USERNAME></REQUEST><RESPONSE><DATA>" + dsPricingCode.GetXml() + "</DATA><ERROR>" + Promoerr + "</ERROR></RESPONSE>";

                            DatabaseLog.LogData(Session["username"].ToString(), "E", "GetPricingCode", "GetPricingCode-Flightsearch", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                            if (dsPricingCode != null && dsPricingCode.Tables.Count > 0 && dsPricingCode.Tables[0].Rows.Count > 0 && dsPricingCode.Tables[0].Columns.Contains("TOT_COUNT") == true)
                            {
                                for (int k = 0; k < dsPricingCode.Tables[0].Rows.Count; k++)
                                {
                                    //int tot_count = Convert.ToInt16(dsPricingCode.Tables[0].Rows[k]["TOT_COUNT"]);
                                    int tot_count = Convert.ToInt32(Math.Ceiling((Convert.ToDouble(dsPricingCode.Tables[0].Rows[k]["TOT_COUNT"]) / 5)));
                                    for (int l = 0; l < tot_count; l++)
                                    {
                                        add_thread += Category + "COUNT" + l + "PR_THREAD" + dsPricingCode.Tables[0].Rows[k]["FTC_TICKET_OFFICE_ID"].ToString() + "|";
                                    }
                                }

                            }
                        }
                    }
                }
                if (Dir_Promothread != "")
                {
                    for (int i = 0; i < Dir_Promothread.Split(',').Length; i++)
                    {
                        if (Dir_Promothread.Split(',')[i] != "" && ((Session["threadkey"].ToString().Contains(Dir_Promothread.Split(',')[i]) && segmenttype == "I")))
                        {
                            string clntid = (ClientID != "" && strTerminalType == "T" ? strClientID : Session["pos_id"].ToString());
                            string terminalid = (ClientID != "" && strTerminalType == "T" ? strClientTerminalID : Session["POS_TID"].ToString());
                            InplantService.Inplantservice _rays = new InplantService.Inplantservice();
                            _rays.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
                            string Promoerr = string.Empty;
                            string classreq = string.Empty;
                            string treq = string.Empty;
                            string Category = Dir_Promothread.Split(',')[i].ToString();
                            dsPricingCode = _rays.FetchCodeCountDetails(clntid, terminalid, Origincode, Destinationcity, segmenttype, strTrip, "", Class, Category, "0", Session["username"].ToString(), "W", Convert.ToDecimal(Session["sequenceid"]), Session["ipaddress"].ToString(), ref Promoerr, Airlinecode);
                            string Xmldata = "<REQUEST><CLIENTID>" + clntid + "</CLIENTID><POSTID>" + terminalid + "</POSTID><ORIGIN>" + Origincode + "</ORIGIN><DESTINATION>" + Destinationcity
                                    + "</DESTINATION><SEGMENTTYPE>" + segmenttype + "</SEGMENTTYPE><TRIPTYPE>" + strTrip + "</TRIPTYPE><CLASS>" + Class + "</CLASS><TRAVELDATE>" + strDepdate + "</TRAVELDATE><STOCKTYPE>" + Category
                                    + "</STOCKTYPE><USERNAME>" + Session["username"].ToString() + "</USERNAME></REQUEST><RESPONSE><DATA>" + dsPricingCode.GetXml() + "</DATA><ERROR>" + Promoerr + "</ERROR></RESPONSE>";

                            DatabaseLog.LogData(Session["username"].ToString(), "E", "GetPricingCode", "GetPricingCode-Flightsearch", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                            if (dsPricingCode != null && dsPricingCode.Tables.Count > 0 && dsPricingCode.Tables[0].Rows.Count > 0 && dsPricingCode.Tables[0].Columns.Contains("TOT_COUNT") == true && Convert.ToInt16(dsPricingCode.Tables[0].Rows[0]["TOT_COUNT"]) > 5)
                            {
                                //int sepcount = Convert.ToInt16(Convert.ToInt16(dsPricingCode.Tables[0].Rows[0]["TOT_COUNT"]) % 5);
                                int sepcount = Convert.ToInt32(Math.Ceiling((Convert.ToDouble(dsPricingCode.Tables[0].Rows[0]["TOT_COUNT"]) / 5))) - 1;
                                for (int j = 1; j <= sepcount; j++)
                                {
                                    Additionalthread += Category + "COUNT" + j + "|";
                                }
                            }
                        }
                    }
                }

                Returnthread = add_thread + Additionalthread;

                Stat = "1";
                array_Response[Threadkey] = (Session["threadkey"] != null ? Session["threadkey"].ToString() : "") + (Returnthread != "" ? "|" + Returnthread.TrimEnd('|') : "");
                array_Response[D_Threadkey] = Session["threadkey_dom"].ToString() + (Returnthread != "" ? "|" + Returnthread.TrimEnd('|') : "");
                array_Response[FSCthread] = Session["fscthreadkey"].ToString() + (Returnthread != "" ? "|" + Returnthread.TrimEnd('|') : "");
                array_Response[D_Fscthread] = Session["fscthreadkey_dom"].ToString() + (Returnthread != "" ? "|" + Returnthread.TrimEnd('|') : "");
                //}
                //else
                //{
                //    Stat = "1";
                //    array_Response[Threadkey] = (Session["threadkey"] != null ? Session["threadkey"].ToString() : "");
                //    array_Response[D_Threadkey] = Session["threadkey_dom"].ToString();
                //    array_Response[FSCthread] = Session["fscthreadkey"].ToString();
                //    array_Response[D_Fscthread] = Session["fscthreadkey_dom"].ToString();
                //}
            }
            catch (Exception ex)
            {
                Stat = "0";
                string Xmldata = "<EVENT>[<![CDATA[" + ex.ToString() + "]]>]</EVENT>";
                DatabaseLog.LogData_wasc(Session["username"].ToString(), "X", "Getpricingcodecount", "Getpricingcodecount-Error", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
            }
            return Json(new { Status = Stat, Message = "", Result = array_Response });
        }

        private string airlineName(string airlineCode, DataSet dsAirways)
        {

            string airwaysName = "";
            var qryAirlineName = from p in dsAirways.Tables["AIRLINES"].AsEnumerable()
                                 where p.Field<string>("AIRLINE_CODE") == airlineCode
                                 select p;
            DataView dvAirlineCode = qryAirlineName.AsDataView();
            if (dvAirlineCode.Count == 0)
                airwaysName = airlineCode;
            else
            {
                DataTable dtAilineCode = new DataTable();
                dtAilineCode = qryAirlineName.CopyToDataTable();
                airwaysName = dtAilineCode.Rows[0]["AIRLINE_NAME"].ToString();
            }
            return airwaysName;
        }

        #region GDS Search
        BLL Bll = new BLL();
        public async Task<string> AsyncFlightAvailGDS(GdsRQRS.RQRS_ancillary.Itineary Itn, string Service_url)
        {
            string content = "";
            string TIMEOUTMINUTS = ConfigurationManager.AppSettings["AVAIL_TIMEOUT"].ToString();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.Timeout = TimeSpan.FromMinutes(Convert.ToInt32(TIMEOUTMINUTS));
                    var ret = await httpClient.PostAsJsonAsync(Service_url, Itn);
                    content = await ret.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {

            }
            return content;
        }

        [AsyncTimeout(180000)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "TimedOut")]
        public async Task<ActionResult> GdsFlightSearch(string strfromCity, string strtoCity, string strDepartureDate,
            string strRetDate, string strAdults, string strChildrens, string strInfants, string TripType, string Airline, string FCategory,
            string Class, string availagent, string availterminal, string Agencyname, string ClientID, string BranchID, string GroupID, string segmenttype, string UID, string reqker)
        {
            ArrayList array_Response = new ArrayList();
            string serverdate = Base.cacheServerdatetime();
            //DateTime addcacheday=
            array_Response.Add(""); //for array concept_groupfare   mugi
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add(""); //for memcache ccheck

            int AvailSession = 1;
            int Error = 2;
            int FlightSearchError = 3;
            int JsonResponse = 4;
            int threadnoresponse = 5;
            int availsideflat = 6;
            int memcachechck = 7;
            int concept_groupfare = 0;
            string stu = string.Empty;
            string msg = string.Empty;
            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strClientType = Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : "";

            try
            {

                if (Session["agentid"] == null)
                {
                    stu = "-1";
                    msg = "session Expired.";
                    array_Response[concept_groupfare] = array_Response[AvailSession] + "sri" + array_Response[Error] + "sri" + array_Response[FlightSearchError] + "sri" + array_Response[JsonResponse] + "sri" + array_Response[threadnoresponse] + "sri" + array_Response[availsideflat] + "sri" + array_Response[memcachechck];
                    return Json(new { Status = stu, Message = msg, Result = array_Response });
                }
                #region UsageLog
                string PageName = "GDS Search Availability";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, UID.Split('$').Length > 0 ? UID.Split('$')[0] : UID, "SEARCH");
                }
                catch (Exception e) { }
                #endregion
                if (TripType == "R")
                {
                    Session.Add("roundtripflg_SESS", "1");
                }
                else
                {
                    Session.Add("roundtripflg_SESS", "");
                }
                #region session assign-problem in asyn hit-sri
                if (reqker.ToString() == "0")
                {
                    if (availagent != null && availagent != "")
                    {
                        Session["Availagentid"] = availagent;
                    }
                    else
                    {
                        Session["Availagentid"] = Session["agentid"].ToString();
                    }
                    if (availterminal != null && availterminal != "")
                    {
                        Session["Availterminal"] = availterminal;
                    }
                    else
                    {
                        Session["Availterminal"] = Session["terminalid"].ToString();
                    }
                    if (Agencyname != null && Agencyname != "")
                    {
                        Session["Availagent"] = Agencyname;
                    }
                    else
                    {
                        Session["Availagent"] = Session["agencyname"].ToString();
                    }

                    System.Web.HttpContext.Current.Session["UniqueIduser"] = null;
                    Session.Add("UniqueIduser", UID);
                }
                #endregion
                string availsetcount = string.Empty;
                string strOrigin = string.Empty;
                string strDestination = string.Empty;
                string strDepdate = string.Empty;
                string strArrdate = string.Empty;
                string strTripType = string.Empty;
                string muticount = string.Empty;
                string gotoflg = string.Empty;
                string strResponse = string.Empty;
                string ReqTime = "";
                int totcachemin = 0;
                bool Hostsearch = false;
                bool HSearch = false;
                bool cache_availtruefalse = false;
                byte[] data = null;
                string strAvailSession_arg = string.Empty;
                string promocodegdscontent = string.Empty;

                GdsRQRS.RQRS_ancillary.AvailabilityRS _availRes = new GdsRQRS.RQRS_ancillary.AvailabilityRS();
                string str_agentid = string.Empty;
                int NoofAdt = Convert.ToInt32(strAdults == "" ? "1" : strAdults);
                int NoofChd = Convert.ToInt32(strChildrens == "" ? "0" : strChildrens);
                int NoofInf = Convert.ToInt32(strInfants == "" ? "0" : strInfants);
                availsetcount = segmenttype.ToString();

                GdsRQRS.RQRS_ancillary.AgentDetails _agentdetails = new GdsRQRS.RQRS_ancillary.AgentDetails();
                totcachemin = ConfigurationManager.AppSettings["flgMemCacheDuration"] != null ?
                          Convert.ToInt32(ConfigurationManager.AppSettings["flgMemCacheDuration"].ToString()) : 20;
                GdsRQRS.RQRS_ancillary.AvailabilityRequests RequestAvail = new GdsRQRS.RQRS_ancillary.AvailabilityRequests();
                List<GdsRQRS.RQRS_ancillary.AvailabilityRequests> avail = new List<GdsRQRS.RQRS_ancillary.AvailabilityRequests>();
                List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>> lstoflstOnwardAvail = new List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>>();

                RQRS.PromoCodeRS _PromocodeRsdata = new RQRS.PromoCodeRS();
                List<RQRS.Promocodes> Prcode = new List<RQRS.Promocodes>();
                RQRS.Promocodes Promocodes = new RQRS.Promocodes();

                strDepdate = DateTime.ParseExact(strDepartureDate, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                RequestAvail.DepartureStation = strfromCity.Split('(')[1].Split(')')[0];
                RequestAvail.ArrivalStation = strtoCity.Split('(')[1].Split(')')[0];
                RequestAvail.FlightDate = strDepdate;

                RequestAvail.FlightNo = "";
                RequestAvail.Connection = "";
                RequestAvail.FareclassOption = Class;
                RequestAvail.FareType = "N";
                avail.Add(RequestAvail);
                if (TripType == "R")
                {
                    RequestAvail = new GdsRQRS.RQRS_ancillary.AvailabilityRequests();
                    strArrdate = DateTime.ParseExact(strRetDate, @"dd\/MM\/yyyy", null).ToString("yyyyMMdd");
                    RequestAvail.DepartureStation = strtoCity.Split('(')[1].Split(')')[0];
                    RequestAvail.ArrivalStation = strfromCity.Split('(')[1].Split(')')[0];
                    RequestAvail.FlightDate = strArrdate;

                    RequestAvail.FlightNo = "";
                    RequestAvail.Connection = "";
                    RequestAvail.FareclassOption = Class;
                    RequestAvail.FareType = "N";
                    avail.Add(RequestAvail);
                }

                _agentdetails.AgentType = strTerminalType == "T" ? strClientType : strAgentType;
                _agentdetails.Airportid = segmenttype;
                _agentdetails.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                _agentdetails.AppType = strAppType;
                _agentdetails.BOAID = Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                _agentdetails.BOATreminalID = Session["POS_TID"] != null && Session["POS_TID"] != "" ? Session["POS_TID"].ToString() : "";
                _agentdetails.BranchID = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                _agentdetails.ClientID = ClientID != null && ClientID != "" ? ClientID : Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";
                _agentdetails.CoOrdinatorID = "";
                _agentdetails.Environment = "B";
                _agentdetails.Platform = "B";
                _agentdetails.ProjectId = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"] != "" ? Session["PRODUCT_CODE"].ToString() : "";
                _agentdetails.TerminalID = Session["POS_TID"] != null && Session["POS_TID"] != "" ? Session["POS_TID"].ToString() : "";
                _agentdetails.UserName = Session["username"] != null && Session["username"] != "" ? Session["username"].ToString() : "";
                _agentdetails.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                _agentdetails.Group_ID = GroupID != null && GroupID != "" ? GroupID : "";
                _agentdetails.AgentID = Session["POS_ID"] != null && Session["POS_ID"] != "" ? Session["POS_ID"].ToString() : "";

                #region Passengers
                /// Pax details adding in list
                GdsRQRS.RQRS_ancillary.Passengers lstrPax = new GdsRQRS.RQRS_ancillary.Passengers();
                lstrPax.PaxCount = (NoofAdt + NoofChd + NoofInf).ToString();

                List<GdsRQRS.RQRS_ancillary.PaxTypeRefs> lstrReferencePax = new List<GdsRQRS.RQRS_ancillary.PaxTypeRefs>();
                if (NoofAdt > 0)
                {
                    GdsRQRS.RQRS_ancillary.PaxTypeRefs RefAdt = new GdsRQRS.RQRS_ancillary.PaxTypeRefs();
                    RefAdt.Type = "ADT";
                    RefAdt.Quantity = NoofAdt.ToString();
                    lstrReferencePax.Add(RefAdt);
                }
                if (NoofChd > 0)
                {
                    GdsRQRS.RQRS_ancillary.PaxTypeRefs RefChd = new GdsRQRS.RQRS_ancillary.PaxTypeRefs();
                    RefChd.Type = "CHD";
                    RefChd.Quantity = NoofChd.ToString();
                    lstrReferencePax.Add(RefChd);
                }
                if (NoofInf > 0)
                {
                    GdsRQRS.RQRS_ancillary.PaxTypeRefs RefInf = new GdsRQRS.RQRS_ancillary.PaxTypeRefs();
                    RefInf.Type = "INF";
                    RefInf.Quantity = NoofInf.ToString();
                    lstrReferencePax.Add(RefInf);
                }
                #endregion
                lstrPax.PaxTypeRef = lstrReferencePax;

                #region FlightOption

                string airlinecode = Airline.Trim().TrimEnd(',');

                string[] lstrArra = new string[] { };
                if (Airline == "")
                { lstrArra = new string[] { }; }
                else
                {
                    if (airlinecode.Contains(","))
                    {
                        lstrArra = airlinecode.Split(',');
                    }
                    else
                    {
                        lstrArra = Airline.TrimEnd(',').Split(',');
                    }
                }
                #endregion

                //Prcode = new List<RQRS.Promocodes>();

                GdsRQRS.RQRS_ancillary.Itineary itin = new GdsRQRS.RQRS_ancillary.Itineary()
                {
                    Agent = _agentdetails,
                    AvailabilityRequest = avail,
                    Category = ConfigurationManager.AppSettings["GDSSTOCK"].ToString(), //"RIYA",
                    Currencycode = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString(), //"INR",
                    TripType = TripType,
                    SegmentType = segmenttype,
                    Passenger = lstrPax,
                    HostSearch = Convert.ToBoolean(Hostsearch),
                    Stock = ConfigurationManager.AppSettings["GDSSTOCK"].ToString(),// "RST_" + availsetcount + "~" + Airline,
                    FlightOption = lstrArra,
                    Platform = "R",
                    UserDefineFlag = "1",
                    //Commission = "Y",
                    //DiscountFlag = "N",
                    //PromoCodes = Prcode,
                };

                #region Sending Request to server..............
                try
                {
                    string memCacheKey = string.Empty;
                    #region MemCache
                    //memCacheKey = itin.TripType + "~" + segmenttype + "~" + FCategory + "~" + Airline + "~" + Class + "~" + strAdults + "~" + strChildrens + "~" + strInfants + "~" + AppCurrency + "~" + fltnumkey + "~" + fltsegmentkey + "~";// JsonConvert.SerializeObject(field);
                    //for (int i = 0; i < avail.Count; i++)
                    //{
                    //    string returndte = "";  // avail[i].FlightReturnDate != null && avail[i].FlightReturnDate != "" ? avail[i].FlightReturnDate.Replace("/", "") : avail[i].FlightDate.Replace("/", "");
                    //    memCacheKey += avail[i].DepartureStation + "~" + avail[i].ArrivalStation + "~" + avail[i].FlightDate.Replace("/", "") + "~" + returndte + "~";
                    //}
                    //memCacheKey += str_agentid; //To Avoid differnet client have same availability... (fare ll change based on client ID )... by saranraj on 20170306...
                    //bool daycache = false;

                    //if (TripType != "M" && TripType != "MD" && TripType != "MI")
                    //{
                    //    if (DateTime.ParseExact(strDepartureDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(serverdate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    //    {
                    //        daycache = true;
                    //    }
                    //}
                    //if (System.Configuration.ConfigurationManager.AppSettings["flgClearMemCache"] != null &&
                    //    System.Configuration.ConfigurationManager.AppSettings["flgClearMemCache"].ToString().Trim() == "Y" && memCache.Contains(memCacheKey) || HSearch == true || daycache == true)
                    //{
                    //    memCache.Remove(memCacheKey);
                    //}

                    //var res = memCache.Get(memCacheKey);
                    //if (res != null)
                    //{
                    //    array_Response[memcachechck] = true;
                    //    cache_availtruefalse = true;
                    //}

                    //if (TripType != "M" && TripType != "MD" && TripType != "MI")
                    //{
                    //    if (res != null)
                    //    {
                    //        strResponse = res.ToString();
                    //        gotoflg = "1";
                    //        // goto afterAvail;
                    //    }
                    //}

                    #endregion
                    /** Serializeing object from request **/
                    string request = JsonConvert.SerializeObject(itin).ToString();
                    string Query = "AirMultiAvail";
                    string LstrDetails = string.Empty;

                    int Availtimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);
                    strURLpathA = ConfigurationManager.AppSettings["GDSSEARCH"].ToString();


                    #region Log

                    if (Base.AvailLog)
                    {
                        ReqTime = "AvailabityReqest" + DateTime.Now;

                        LstrDetails = "<THREAD_REQUEST><URL>[<![CDATA[" + strURLpathA + "]]>]</URL><QUERY>" + Query + "</QUERY><CATEGORY>"
                              + itin.TripType + " ~ " + itin.Category + " ~ " +
                              ((string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) +
                              (string.IsNullOrEmpty(strDestination) ? "" : strDestination)) +
                              "</CATEGORY><JSON>" + request + "</JSON><TIME>" + ReqTime + "</TIME>";

                        StringWriter strREQUEST = new StringWriter();
                        if (Base.ReqLog)
                        {
                            DataSet dsrequest = new DataSet();
                            dsrequest = Serv.convertJsonStringToDataSet(request, "");
                            if (dsrequest != null)
                                dsrequest.WriteXml(strREQUEST);
                        }

                        DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "SearchController",
                            ((itin.TripType + " ~TRQ~" + itin.Category + " ~ " +
                            (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) +
                            (string.IsNullOrEmpty(strDestination) ? "" : strDestination))
                            + ":THREAD REQUEST" + (Airline == null ? "" : " -FLIGHTOPTION :" + Airline)),
                            LstrDetails + ((Base.ReqLog) ? strREQUEST.ToString() : "") + "</THREAD_REQUEST>", Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                        //if (roundtripflg == "1")
                        //{

                        //    DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "InternationalFlights",
                        //        ((itin.TripType + "R" + " ~TRQ~" + itin.Category + " ~ " +
                        //        (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) +
                        //        (string.IsNullOrEmpty(strDestination) ? "" : strDestination))
                        //        + ":THREAD REQUEST" + (Airline == null ? "" : " -FLIGHTOPTION :" + Airline)),
                        //        LstrDetails + ((Base.ReqLog) ? strREQUEST.ToString() : "") + "</THREAD_REQUEST>", Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                        //}

                    }



                    #endregion

                    strResponse = await AsyncFlightAvailGDS(itin, strURLpathA + Query);

                    string str = "";

                    if (string.IsNullOrEmpty(strResponse))
                    {
                        #region log
                        string lstrtime = "AvailabityResponse" + DateTime.Now;
                        LstrDetails = "<THREAD_RESPONSE><THREAD>" + (itin.TripType + " ~ " + itin.Category + " ~ " +
                            (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) +
                            (string.IsNullOrEmpty(strDestination) ? "" : strDestination)) + "</THREAD><REqTime>" + ReqTime + "</REqTime><ResTIME>" + lstrtime + "</ResTIME>" +
                            "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" + "Null Or Empty" + "</THREAD_RESPONSE>";

                        DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "SearchController", (itin.TripType + " ~TRSN~" + itin.Category + " ~ " +
                            (string.IsNullOrEmpty(strOrigin) ? "" : strOrigin) + (string.IsNullOrEmpty(strDestination) ? "" :
                            strDestination)) + " :THREAD RESPONSE NULL", LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                        #endregion
                        array_Response[Error] = "Problem occured while Searching Flight";
                        array_Response[concept_groupfare] = array_Response[AvailSession] + "sri" + array_Response[Error] + "sri" + array_Response[FlightSearchError] + "sri" + array_Response[JsonResponse] + "sri" + array_Response[threadnoresponse] + "sri" + array_Response[availsideflat] + "sri" + array_Response[memcachechck];

                        return Json(new { Status = "", Message = "", Result = array_Response });
                    }

                    afterAvail:
                    _availRes = JsonConvert.DeserializeObject<GdsRQRS.RQRS_ancillary.AvailabilityRS>(strResponse);
                    if (_availRes.ResultCode != null && _availRes.ResultCode == "1" && (memCache.Get(memCacheKey) == null || memCache.Get(memCacheKey) == ""))
                    {
                        memCache.Add(memCacheKey, strResponse, DateTimeOffset.UtcNow.AddMinutes(totcachemin));
                    }

                    if (_availRes.ResultCode != null)
                    {
                        if (_availRes.ResultCode == "1")
                        {

                            #region Log
                            string lstrtime = "AvailabityResponse" + DateTime.Now;
                            StringWriter strres = new StringWriter();
                            if (Base.ReqLog)
                            {
                                DataSet dsresponse = new DataSet();
                                dsresponse = Serv.convertJsonStringToDataSet(strResponse, "");
                                if (dsresponse != null)
                                    dsresponse.WriteXml(strres);
                            }
                            LstrDetails = "<THREAD_RESPONSE><THREAD>" + (itin.Category + " ~TRS~" +
                              (string.IsNullOrEmpty(strOrigin) ? "" : strOrigin) +
                              (string.IsNullOrEmpty(strDestination) ? "" : strDestination)) + "</THREAD><REqTime>" + ReqTime + "</REqTime><ResTIME>" +
                              lstrtime + "</ResTIME>" + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                           "<Flights>" + (_availRes == null || _availRes.Flights == null ? "NULL" : _availRes.Flights.Count.ToString()) + "</Flights>" +
                           "<Fares>" + (_availRes == null || _availRes.Fares == null ? "NULL" : _availRes.Fares.Count.ToString()) + "</Fares>" +
                           ((Base.ReqLog) ? strres.ToString() : "") + "</THREAD_RESPONSE>";

                            DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "SearchController", (itin.TripType + " ~TRS~ " + itin.Category + " ~ " +
                                (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) + (string.IsNullOrEmpty(strDestination) ? "" :
                                strDestination)) + " :THREAD RESPONSE SUCCESS", LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());

                            #endregion
                            string strairlinenamejson = string.Empty;
                            DataSet dsAirways = new DataSet();
                            DataSet dsArilineNames = new DataSet();
                            if (Session["dsAirportlst"] == null || Session["dsAirportlst"].ToString() == "")
                            {
                                string strPathcityall = Server.MapPath("~/XML/CityAirport_Lst.xml");
                                dsAirways.ReadXml(strPathcityall);
                                strairlinenamejson = JsonConvert.SerializeObject(dsAirways);
                                Session.Add("dsAirportlst", strairlinenamejson);

                            }
                            else
                            {
                                strairlinenamejson = Session["dsAirportlst"].ToString();
                                dsAirways = JsonConvert.DeserializeObject<DataSet>(strairlinenamejson);
                            }

                            string strPathcity = Server.MapPath("~/XML/AirlineNames.xml");
                            dsArilineNames.ReadXml(strPathcity);

                            string strAvailSession = "AVAILFLIGHTSINT_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            string[] strKeySession = strAvailSession.Split('_');
                            strAvailSession_arg = strAvailSession;
                            stu = "1";
                            msg = "";

                            var FlightsDatatemp = from _Flights in _availRes.Flights.AsEnumerable()


                                                  join _Ocity in dsAirways.Tables[0].AsEnumerable()
                                                  on _Flights.Origin equals _Ocity["ID"]

                                                  join _Dcity in dsAirways.Tables[0].AsEnumerable()
                                                  on _Flights.Destination equals _Dcity["ID"]
                                                  join _AName in dsArilineNames.Tables[0].AsEnumerable()
                                                  on _Flights.PlatingCarrier equals _AName["_CODE"]

                                                  //where _Flights.ItinRef != "" && _AName["AIRPORT_ID"].ToString() == FCategory
                                                  //from _FareData in _Fares.Faredescription.Where(e => e.Paxtype.Equals("ADT")).AsEnumerable()

                                                  select new GdsRQRS.RQRS_ancillary.FlightDetails_GDS
                                                  {
                                                      FlightNumber = _Flights.FlightNumber.TrimEnd(),
                                                      Origin = _Flights.Origin,
                                                      Destination = _Flights.Destination,
                                                      OriginName = _Ocity["AN"].ToString(),
                                                      DestinationName = _Dcity["AN"].ToString(),
                                                      DepartureDate = _Flights.DepartureDateTime.Split(' ')[0] + " " + _Flights.DepartureDateTime.Split(' ')[1] + " " + _Flights.DepartureDateTime.Split(' ')[2],
                                                      ArrivalDate = _Flights.ArrivalDateTime.Split(' ')[0] + " " + _Flights.ArrivalDateTime.Split(' ')[1] + " " + _Flights.ArrivalDateTime.Split(' ')[2],
                                                      DepartureTime = _Flights.DepartureDateTime.Split(' ')[3],
                                                      ArrivalTime = _Flights.ArrivalDateTime.Split(' ')[3],
                                                      FlyingTime = _Flights.FlyingTime,
                                                      Class = _Flights.Class.Trim() + '-' + _Flights.FareBasisCode.Trim(),
                                                      Cabin = _Flights.Cabin.TrimEnd(','),
                                                      CNX = _Flights.CNXType,
                                                      CarrierCode = _Flights.CarrierCode,
                                                      FareBasisCode = _Flights.FareBasisCode,
                                                      FareId = _Flights.FareId,
                                                      PlatingCarrier = _Flights.PlatingCarrier,
                                                      ReferenceToken = _Flights.ReferenceToken,
                                                      SegRef = _Flights.SegRef,
                                                      Stops = _Flights.Stops,
                                                      Seat = _Flights.Seat,
                                                      Refund = _Flights.Refundable,
                                                      Stock = itin.Stock,
                                                      ItinRef = _Flights.ItinRef,
                                                      FareSegRef = _availRes.UserDefineFlag,
                                                      MulticlassAirlinecategory = _Flights.AirlineCategory,
                                                      Airlinecategory = _Flights.AirlineCategory,
                                                      STK = _availRes.FareCheck.Stocktype,
                                                      StartTerminal = _Flights.StartTerminal,
                                                      EndTerminal = _Flights.EndTerminal,
                                                      AirlineName = _AName["_NAME"].ToString(),
                                                      Promocodecontent = promocodegdscontent
                                                  };
                            lstoflstOnwardAvail = FlightsDatatemp.GroupBy(u => u.FareId).Select(grp => grp.ToList()).ToList();
                            array_Response[JsonResponse] = JsonConvert.SerializeObject(lstoflstOnwardAvail);

                        }
                        else
                        {
                            #region log
                            string lstrtime = "GdsAvailabityResponse" + DateTime.Now;
                            string error = (_availRes.Error != null &&
                             (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ?
                             ((string.IsNullOrEmpty(_availRes.Sqe) ?
                             _availRes.Error.ToString() :
                             (_availRes.Sqe + " : " + _availRes.Error.ToString()))) : "";
                            LstrDetails = "<THREAD_RESPONSE><THREAD>" + (itin.Category + " ~TRSF~" +
                                (string.IsNullOrEmpty(strOrigin) ? "" : strOrigin) +
                                (string.IsNullOrEmpty(strDestination) ? "" : strDestination)) + "</THREAD><REqTime>" + ReqTime + "</REqTime><ResTIME>" +
                           lstrtime + "</ResTIME>" + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                           ((Base.ReqLog) ? strResponse.ToString() : "") + "</THREAD_RESPONSE>";
                            DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "Search Controller", (itin.TripType + " ~TRSF~ " + itin.Category + " ~ " +
                                (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) + (string.IsNullOrEmpty(strOrigin) ? "" :
                                strOrigin)) + " :THREAD RESPONSE FAIL - " + error, LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                            #endregion
                            _availRes = JsonConvert.DeserializeObject<GdsRQRS.RQRS_ancillary.AvailabilityRS>(strResponse);
                            stu = _availRes.Status.ResultCode;
                            msg = _availRes.Status.Error;
                        }
                    }
                    else
                    {
                        #region log

                        string lstrtime = "GdsAvailabityResponse" + DateTime.Now;

                        string error = (_availRes.Error != null &&
                         (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ?
                         ((string.IsNullOrEmpty(_availRes.Sqe) ?
                         _availRes.Error.ToString() :
                         (_availRes.Sqe + " : " + _availRes.Error.ToString()))) : "";

                        LstrDetails = "<THREAD_RESPONSE><THREAD>" + (itin.Category + " ~TRSF~" +
                            (string.IsNullOrEmpty(strOrigin) ? "" : strOrigin) +
                            (string.IsNullOrEmpty(strDestination) ? "" : strDestination)) + "</THREAD><REqTime>" + ReqTime + "</REqTime><ResTIME>" +
                       lstrtime + "</ResTIME>" + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                       ((Base.ReqLog) ? strResponse.ToString() : "") + "</THREAD_RESPONSE>";

                        DatabaseLog.LogData_wasc(Session["username"].ToString(), "T", "Search Controller", (itin.TripType + " ~TRSF~ " + itin.Category + " ~ " +
                            (string.IsNullOrEmpty(strOrigin) ? "" : (strOrigin + "~")) + (string.IsNullOrEmpty(strOrigin) ? "" :
                            strOrigin)) + " :THREAD RESPONSE FAIL - " + error, LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());


                        #endregion
                        _availRes = JsonConvert.DeserializeObject<GdsRQRS.RQRS_ancillary.AvailabilityRS>(strResponse);
                        stu = _availRes.Status.ResultCode;
                        msg = _availRes.Status.Error;

                    }

                }
                catch (Exception ex)
                {
                    stu = "0";
                    msg = "";
                    string Xmldata = "<EVENT><DeptDate>[<![CDATA[" + strDepartureDate + "]]>]</DeptDate><RetDate>[<![CDATA[" + strRetDate + "]]>]</RetDate><Error>[<![CDATA[" + ex.ToString() + "]]>]</Error></EVENT>";
                    DatabaseLog.LogData_wasc(Session["username"].ToString(), "X", "SearchController", "GdsFlightSearch", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                    return Json(new { Status = stu, Message = msg, Result = array_Response, Availset = "" });
                }

            }

            catch (Exception Second_Ex)
            {
                stu = "0";
                msg = "";
                string Xmldata = "<EVENT><DeptDate>[<![CDATA[" + strDepartureDate + "]]>]</DeptDate><RetDate>[<![CDATA[" + strRetDate + "]]>]</RetDate><Error>[<![CDATA[" + Second_Ex.ToString() + "]]>]</Error></EVENT>";
                DatabaseLog.LogData_wasc(Session["username"].ToString(), "X", "SearchController", "GdsFlightSearch", Xmldata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString() + "(" + Session["Availterminal"].ToString() + ")", Session["sequenceid"].ToString());
                return Json(new { Status = stu, Message = msg, Result = array_Response, Availset = "" });

            }
            return Json(new { Status = stu, Message = msg, Result = array_Response });
            #endregion
        }

        public ActionResult GetClassFare_GDS(string selectflight, string[] Classid, string ClientID, string BranchID, string Airporttype, string SearchDet)
        {
            string CompanyID = string.Empty;
            string Userid = string.Empty;
            string Username = string.Empty;
            string Ipaddress = string.Empty;
            string SequenceID = "0";
            string xmldata = string.Empty;
            string strResult = string.Empty;
            string MulticlassAvail = string.Empty;
            string connectingflag = string.Empty;

            string strAgentID = string.Empty;
            string strTerminalID = string.Empty;
            string strUserName = string.Empty;
            string strPOSID = string.Empty;
            string strCliendId = string.Empty;
            string PromocodeGDS = Session["GDSPromocode"] != null ? Session["GDSPromocode"].ToString().TrimEnd('|') : "";
            string RetailfareGDS = Session["RetailfarePromocode"] != null ? Session["RetailfarePromocode"].ToString().TrimEnd('|') : "";
            string[] PromocodeGdsary = PromocodeGDS.Split('|');
            string[] Retailpromoary = RetailfareGDS.Split('|');

            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strClientType = Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : "";


            try
            {

                if (Session["POS_TID"] == null)
                {
                    return Json(new { status = "-1", Errormsg = "Your session has expired.", Empdata = "" });
                }
                #region UsageLog
                string PageName = "GDS GetClassfare";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "FETCH");
                }
                catch (Exception e) { }
                #endregion

                strCliendId = Session["CompanyID"] != null && Session["CompanyID"].ToString() != "" ? Session["CompanyID"].ToString() : "";
                strPOSID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : "";
                strAgentID = Session["AGENTID"] != null && Session["AGENTID"].ToString() != "" ? Session["AGENTID"].ToString() : "";
                strTerminalID = Session["TERMINALID"] != null && Session["TERMINALID"].ToString() != "" ? Session["TERMINALID"].ToString() : "";
                strUserName = Session["UserName"] != null && Session["UserName"].ToString() != "" ? Session["UserName"].ToString() : "";

                selectflight = selectflight.Replace(@"\", "");


                List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>> lst = new List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>>();
                lst = (List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>>)JsonConvert.DeserializeObject(selectflight, typeof(List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>>));

                List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>> lstoflstOnwardAvail = new List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>>();

                #region GDS RQRS Object Creation
                GdsRQRS.MultiClass_RQ MulticlassReq = new GdsRQRS.MultiClass_RQ();
                GdsRQRS.AgentDetails AgnDet_GDS = new GdsRQRS.AgentDetails();
                List<GdsRQRS.RQFlights> _lstflight = new List<GdsRQRS.RQFlights>();
                GdsRQRS.Segment _seg = new GdsRQRS.Segment();

                GdsRQRS.MultiClass_RS MulticlassRes = new GdsRQRS.MultiClass_RS();
                GdsRQRS.Status _status = new GdsRQRS.Status();
                List<GdsRQRS.Flights> _lstfltRS = new List<GdsRQRS.Flights>();
                List<GdsRQRS.Fares> _lstfrsRS = new List<GdsRQRS.Fares>();
                #endregion

                AgnDet_GDS.AgentID = strAgentID;
                AgnDet_GDS.TerminalID = strTerminalID;
                AgnDet_GDS.AgentType = strTerminalType == "T" ? strClientType : strAgentType;
                AgnDet_GDS.AppType = "B2B";
                AgnDet_GDS.UserName = strUserName;
                AgnDet_GDS.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                AgnDet_GDS.ClientID = Session["POS_ID"] != null && Session["POS_ID"].ToString() != "" ? Session["POS_ID"].ToString() : ""; ;
                AgnDet_GDS.Group_ID = Session["GroupID"] != null && Session["GroupID"] != "" ? Session["GroupID"].ToString() : "";
                AgnDet_GDS.BOAID = strPOSID;
                AgnDet_GDS.BOATreminalID = Session["POS_TID"] != null && Session["POS_TID"] != "" ? Session["POS_TID"].ToString() : "";
                AgnDet_GDS.CoOrdinatorID = "";
                AgnDet_GDS.Airportid = Airporttype;
                AgnDet_GDS.Environment = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                AgnDet_GDS.BranchID = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : ""; //Session["BRANCH_ID"] != null && Session["BRANCH_ID"].ToString() != "" ? Session["BRANCH_ID"].ToString() : "";
                AgnDet_GDS.Platform = "B"; //ABCD
                AgnDet_GDS.ProductID = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"] != "" ? Session["PRODUCT_CODE"].ToString() : ""; //ABCD
                AgnDet_GDS.ProjectId = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"] != "" ? Session["PRODUCT_CODE"].ToString() : ""; //ABCD
                AgnDet_GDS.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();


                string BaseOrigin = string.Empty;
                string BaseDestination = string.Empty;

                string[] arySearchdet = SearchDet.Split('~');
                _seg.Adult = Convert.ToInt32(arySearchdet[0]);
                _seg.Child = Convert.ToInt32(arySearchdet[1]);
                _seg.Infant = Convert.ToInt32(arySearchdet[2]);

                _seg.TripType = arySearchdet[3];
                _seg.SegmentType = arySearchdet[4];

                _seg.BaseOrigin = arySearchdet[6].Split('(')[1].Split(')')[0];
                _seg.BaseDestination = arySearchdet[7].Split('(')[1].Split(')')[0];

                if (lst.Count > 0)
                {
                    var cnt = 0;
                    for (var ii = 0; lst.Count > ii; ii++) //Segment count... (if roundtrip...)
                    {
                        for (var i = 0; lst[ii].Count > i; i++) //Flight count... (if connecting flight...)
                        {
                            GdsRQRS.RQFlights _fltRQ = new GdsRQRS.RQFlights();

                            _fltRQ.Class = Classid[cnt];
                            _fltRQ.AirlineCategory = lst[ii][i].Airlinecategory;
                            _fltRQ.ArrivalDateTime = lst[ii][i].ArrivalDate + " " + lst[ii][i].ArrivalTime;
                            _fltRQ.Cabin = "";
                            _fltRQ.CarrierCode = lst[ii][i].CarrierCode;
                            _fltRQ.DepartureDateTime = lst[ii][i].DepartureDate + " " + lst[ii][i].DepartureTime;
                            _fltRQ.Destination = lst[ii][i].Destination;
                            _fltRQ.EndTerminal = lst[ii][i].EndTerminal;
                            _fltRQ.FareBasisCode = lst[ii][i].FareBasisCode;
                            _fltRQ.FareID = lst[ii][i].FareId;
                            _fltRQ.FlightNumber = (lst[ii][i].FlightNumber).Contains(' ') ? lst[ii][i].FlightNumber.Split(' ')[1] : lst[ii][i].FlightNumber;
                            _fltRQ.ItinRef = _seg.TripType == "M" ? ii.ToString() : lst[ii][i].ItinRef;
                            _fltRQ.Origin = lst[ii][i].Origin;
                            _fltRQ.PlatingCarrier = lst[ii][i].PlatingCarrier;
                            _fltRQ.ReferenceToken = lst[ii][i].ReferenceToken;
                            _fltRQ.SeatAvailFlag = "";
                            _fltRQ.SegRef = lst[ii][i].SegRef;
                            _fltRQ.StartTerminal = lst[ii][i].StartTerminal;
                            _fltRQ.FareType = "N";
                            _fltRQ.PromoCode = "";

                            _lstflight.Add(_fltRQ);
                            cnt++;
                        }
                    }
                }

                MulticlassReq.AgentDetail = AgnDet_GDS;
                MulticlassReq.FlightsDetails = _lstflight;
                MulticlassReq.SegmentDetails = _seg;
                MulticlassReq.Platform = ""; //R
                MulticlassReq.Stock = lst[0][0].Stock;

                string StrInput = JsonConvert.SerializeObject(MulticlassReq);

                string useridunique = Session["UniqueIduser"] != null ? Session["UniqueIduser"].ToString() : ""; // by vijai for log purpose 07042018

                #region Request Log
                string ReqTime = "GetClassFare_Gds" + DateTime.Now;
                string LstrDetails = "<THREAD_REQUEST><FUNCTION>GDS FARE REQUEST</FUNCTION><QUERY>" + StrInput + "</QUERY><TIME>" + ReqTime + "</TIME></THREAD_REQUEST>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "Search Controler", "GetClassFare_Gds", LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                #endregion
                strURLpathA = ConfigurationManager.AppSettings["GDSSEARCH"].ToString();
                MyWebClient Client = new MyWebClient();
                Client.Headers["Content-Type"] = "application/json";
                byte[] byteGetLogin = Client.UploadData(strURLpathA + "/" + "AirMultiFare", "POST", Encoding.ASCII.GetBytes(StrInput));
                string strResponse = Encoding.ASCII.GetString(byteGetLogin);

                #region Response Log
                ReqTime = "GetClassFare_GDS" + DateTime.Now;
                LstrDetails = "<THREAD_RESPONSE><FUNCTION>GDS FARE RESPONSE</FUNCTION><QUERY>" + strResponse + "</QUERY><TIME>" + ReqTime + "</TIME></THREAD_RESPONSE>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "Search Controler", "GetClassFare_Gds", LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                #endregion

                MulticlassRes = JsonConvert.DeserializeObject<GdsRQRS.MultiClass_RS>(strResponse);

                if (MulticlassRes.Status.ResultCode.ToString() == "0")
                {
                    return Json(new { status = "03", Errormsg = "Unable to fetching class details.Please contact support team-#03", Result = "" });
                }
                else if (MulticlassRes.Status.ResultCode.ToString() == "1")
                {
                    //strResult = JsonConvert.SerializeObject(MulticlassRes);

                    var FDD = from _Flights in MulticlassRes.FlightDetails.AsEnumerable()

                              join _Fares in MulticlassRes.FareDetails.AsEnumerable()
                              on _Flights.FareID equals _Fares.FlightID

                              where _Flights.FareID != "" && _Fares.FlightID != "" && _Flights.ItinRef != ""
                              select new GdsRQRS.RQRS_ancillary.FlightDetails_GDS
                              {
                                  FlightNumber = _Flights.PlatingCarrier + " " + _Flights.FlightNumber,
                                  Origin = _Flights.Origin,
                                  OriginName = _Flights.Origin,
                                  Destination = _Flights.Destination,
                                  DestinationName = _Flights.Destination,
                                  DepartureDate = _Flights.DepartureDateTime.Split(' ')[0] + " " + _Flights.DepartureDateTime.Split(' ')[1] + " " + _Flights.DepartureDateTime.Split(' ')[2],
                                  DepartureTime = _Flights.DepartureDateTime.Split(' ')[3],
                                  ArrivalDate = _Flights.ArrivalDateTime.Split(' ')[0] + " " + _Flights.ArrivalDateTime.Split(' ')[1] + " " + _Flights.ArrivalDateTime.Split(' ')[2],
                                  ArrivalTime = _Flights.ArrivalDateTime.Split(' ')[3],
                                  FlyingTime = _Flights.FlyingTime,
                                  Class = _Flights.Class + "-" + _Flights.FareBasisCode,
                                  ClassType = _Flights.Class,
                                  Cabin = _Flights.Cabin,
                                  AvailSeat = _Flights.Seat,
                                  BaggageInfo = "Aircraft Type:" + "Journey Time:" + _Flights.FlyingTime + "Start Terminal:" + _Flights.StartTerminal + "EndTerminal:" + _Flights.EndTerminal + "Baggage:" + _Flights.Baggage,
                                  CarrierCode = _Flights.PlatingCarrier,
                                  CNX = _Flights.CNXType,
                                  ConnectionFlag = _Flights.CNXType,
                                  FareBasisCode = _Flights.FareBasisCode,
                                  FareId = _Flights.FareID,
                                  Faredescription = _Fares.Faredescription,
                                  PlatingCarrier = _Flights.PlatingCarrier,
                                  ReferenceToken = _Flights.ReferenceToken,
                                  ItinRef = _Flights.ItinRef,
                                  SegRef = _Flights.SegRef,
                                  Stops = _Flights.Stops,
                                  Stock = MulticlassReq.Stock,
                                  via = "",
                                  Refund = _Flights.Refundable,
                                  BaseAmount = _Fares.Faredescription[0].BaseAmount,
                                  GrossAmount = (Convert.ToInt64(_Fares.Faredescription[0].GrossAmount) + Convert.ToInt64(_Fares.Faredescription[0].ClientMarkup != "" ? _Fares.Faredescription[0].ClientMarkup : "0")).ToString(),
                                  TotalTaxAmount = _Fares.Faredescription[0].TotalTaxAmount,
                                  Commission = _Fares.Faredescription[0].Commission,
                                  Taxes = _Fares.Faredescription[0].Taxes,
                                  MultiClassAmount = "",
                                  MulticlassAirlinecategory = _Flights.AirlineCategory,
                                  Airlinecategory = _Flights.AirlineCategory,
                                  ClassCarrierCode = _Flights.Class + "-" + _Flights.FareBasisCode,
                                  STK = MulticlassReq.Stock,//_Flights.AirlineCategory,
                                  FareType = "",
                                  StartTerminal = _Flights.StartTerminal,
                                  EndTerminal = _Flights.EndTerminal,
                                  JourneyTime = _Flights.FlyingTime,
                                  MulticlassAvail = "",
                                  AirlineName = _Flights.CarrierCode + " " + _Flights.FlightNumber,
                              };

                    lstoflstOnwardAvail = FDD.GroupBy(u => u.FareId).Select(grp => grp.ToList()).ToList();

                    strResult = JsonConvert.SerializeObject(lstoflstOnwardAvail);
                }
            }
            catch (Exception ex)
            {
                string errormgs = string.Empty;
                if (ex.Message.ToString() == "The operation has timed out")
                    errormgs = ex.Message.ToString();
                else if (ex.Message.ToString() == "Unable to connect the remote server")
                    errormgs = "unable to connect the remote server.";
                else
                    errormgs = "Problem occured While load class details.Please contact support team (#05).";
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Search Controler", "GdsSelectedFltDetails", ex.Message.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                return Json(new { status = "00", Errormsg = errormgs, Result = "" });
            }
            return Json(new { status = "01", Errormsg = "", Result = strResult });
        }

        public ActionResult GdsSelectedFltDetails(string selectflight, string strJsonOthertxt, string searchDet, string Adult, string Child, string Infant,
        string TripType, string TotalPaxCount, string Refundableoption, string onwardid, string returnid, string button_id, string CurrencyCode,
        string ClientID, string Personalbooking, string Specialflagfare, string AllowSpecialFare, string TwoOnewayflag, string Multicabin, string QuerystringTripid)
        {
            ArrayList array_Response = new ArrayList();
            DataSet dataSet = new DataSet();
            DataTable dtBaggageSelect = new DataTable();
            DataTable dtmealseSelect = new DataTable();
            DataTable dtOtherSsrsel = new DataTable();
            DataTable dtBookreq = new DataTable();
            DataTable dtOtherbaggout = new DataTable();
            array_Response.Add("");
            array_Response.Add("");
            array_Response.Add("");
            string TokenBooking = "";
            Base.ServiceUtility Serv = new Base.ServiceUtility();
            RQRS.PriceItenaryRS _availRes = new RQRS.PriceItenaryRS();
            DataTable dtSelectResponse = new DataTable();
            RaysService _rays_servers = new RaysService();
            _rays_servers.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
            InplantService.Inplantservice _inplantservice = new InplantService.Inplantservice();
            _inplantservice.Url = ConfigurationManager.AppSettings["IN_RAYS_SERVICE"].ToString();
            string strAirlinsecatagory = string.Empty;
            string refuncs = string.Empty;
            int ResultCode = 0;
            int ErrorMsg = 1;
            string Returnval = "";
            string[] det = searchDet.Split('~');
            string BaseOrgin = det[6].Split('(')[1].Split(')')[0];
            string BaseDestination = det[7].Split('(')[1].Split(')')[0];
            string SegmentType = det[4];
            string Trip = det[3];
            string strAdults = det[0];
            string strChildrens = det[1];
            string strInfants = det[2];
            string SClass = det[5];
            string BranchID = string.Empty;
            string GroupID = string.Empty;
            string RtripComparFlg = string.Empty;
            string AlterQueue = string.Empty;
            string FullFlag = string.Empty;
            string oldfaremarkup = string.Empty;
            string Nfarecheck = string.Empty;
            string Deaprt = string.Empty;
            string Arrive = string.Empty;
            string DEPTDATE = string.Empty;
            string ARRDATE = string.Empty;
            string StkType = string.Empty;
            string Class = string.Empty;
            Session.Add("domseg", SegmentType);

            string strTerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            string strAgentType = Session["agenttype"] != null ? Session["agenttype"].ToString() : "";
            string strClientType = Session["CLIENT_TYPE"] != null ? Session["CLIENT_TYPE"].ToString() : "";

            try
            {
                string strAgentID = Session["agentid"] != null ? Session["agentid"].ToString() : "";
                string strTerminalId = Session["terminalid"] != null ? Session["terminalid"].ToString().ToUpper().Trim() : "";
                string strUserName = Session["username"] != null ? Session["username"].ToString() : "";
                string Ipaddress = Session["ipAddress"] != null ? Session["ipAddress"].ToString() : "";
                string sequnceID = Session["sequenceid"] != null ? Session["sequenceid"].ToString() : "";
                string strBranchId = Session["branchid"] != null ? Session["branchid"].ToString() : "";
                string TerminalType = Session["TERMINALTYPE"] != null && Session["TERMINALTYPE"].ToString() != "" ? Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
                string strPosID = Session["POS_ID"] != null ? Session["POS_ID"].ToString() : "";
                string strPosTID = Session["POS_TID"] != null ? Session["POS_TID"].ToString() : "";
                string ValKey = "";
                string ClientID_x = ClientID != null && ClientID != "" ? ClientID : Session["POS_ID"].ToString();
                string balfetch = "";
                string agninfo = string.Empty;
                string depttime = string.Empty;
                string ArrTime = string.Empty;
                string CNX = string.Empty;
                string FareID = string.Empty;
                string FareCode = string.Empty;
                string AirlineCategory = string.Empty;

                RQRS.PriceItineary PriceIti = new RQRS.PriceItineary();
                List<RQRS.Itineraries> ListItenar = new List<RQRS.Itineraries>();
                List<RQRS.Flights> FlightsDet = new List<RQRS.Flights>();
                RQRS.Itineraries itinflights = new RQRS.Itineraries();
                RQRS.SegmentDetails SegDet = new RQRS.SegmentDetails();
                RQRS.Flights Itindet = new RQRS.Flights();

                #region UsageLog
                string PageName = "GDS Select Flight";
                try
                {
                    string strUsgLogRes = Base.Commonlog(PageName, "", "SELECT");
                }
                catch (Exception e) { }
                #endregion
                selectflight = selectflight.Replace(@"\", "");

                List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>> lst = new List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>>();
                lst = (List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>>)JsonConvert.DeserializeObject(selectflight, typeof(List<List<GdsRQRS.RQRS_ancillary.FlightDetails_GDS>>));

                int flightcounts = 0;

                string[] TerminalDetails = new string[] { };
                string[] ArrFliDet, FlightCount, IFCount;
                IFCount = Regex.Split(FullFlag, "SpLiTWeB");
                ListItenar = new List<RQRS.Itineraries>();
                string rtsItinref = "0";

                Itindet = new RQRS.Flights();
                itinflights = new RQRS.Itineraries();
                FlightsDet = new List<RQRS.Flights>();


                if (lst.Count > 0)
                {
                    var cnt = 0;
                    for (var ii = 0; lst.Count > ii; ii++)
                    {

                        itinflights = new RQRS.Itineraries();
                        FlightsDet = new List<RQRS.Flights>();
                        // flightcounts++;
                        for (var i = 0; lst[ii].Count > i; i++) //Flight count... (if connecting flight...)
                        {
                            flightcounts++;
                            Itindet = new RQRS.Flights();
                            Itindet.AirlineCategory = lst[ii][i].Airlinecategory;
                            Itindet.ArrivalDateTime = lst[ii][i].ArrivalDate + " " + lst[ii][i].ArrivalTime;
                            Itindet.DepartureDateTime = lst[ii][i].DepartureDate + " " + lst[ii][i].DepartureTime;
                            Itindet.Destination = lst[ii][i].Destination;
                            Itindet.FareBasisCode = lst[ii][i].FareBasisCode;
                            Itindet.FlightNumber = lst[ii][i].FlightNumber;
                            Itindet.Origin = lst[ii][i].Origin;
                            Itindet.PlatingCarrier = lst[ii][i].PlatingCarrier;
                            Itindet.Class = lst[ii][i].ClassType;
                            Itindet.Cabin = lst[ii][i].ClassType;
                            Itindet.RBDCode = lst[ii][i].ClassType;
                            Itindet.ReferenceToken = lst[ii][i].ReferenceToken;
                            Itindet.OfflineFlag = 0;
                            Itindet.ConnectionFlag = lst[ii][i].ConnectionFlag;
                            Itindet.TransactionFlag = false;
                            Itindet.ItinRef = TripType == "M" ? ii.ToString() : lst[ii][i].ItinRef;
                            Itindet.SegRef = lst[ii][i].SegRef;
                            Itindet.CNX = lst[ii][i].CNX;
                            Itindet.FareId = lst[ii][i].SegRef;
                            FlightsDet.Add(Itindet);

                        }

                        itinflights.BaseAmount = lst[ii][0].Faredescription[0].BaseAmount;
                        itinflights.GrossAmount = lst[ii][0].Faredescription[0].GrossAmount;
                        itinflights.FlightDetails = FlightsDet;
                        itinflights.Stock = ConfigurationManager.AppSettings["GDSSTOCK"].ToString();
                        itinflights.FareType = "N";
                        itinflights.FareTypeDescription = "";

                        ListItenar.Add(itinflights);

                    }

                }

                Deaprt = lst[0][0].DepartureTime;
                Arrive = lst[0][0].ArrivalTime;
                DEPTDATE = lst[0][0].DepartureDate;
                ARRDATE = lst[0][0].ArrivalDate;
                StkType = ConfigurationManager.AppSettings["GDSSTOCK"].ToString();
                Class = lst[0][0].ClassType;

                //itinflights.BaseAmount = lst[0][0].Faredescription[0].BaseAmount;
                //itinflights.GrossAmount = lst[0][0].Faredescription[0].GrossAmount;
                //itinflights.FlightDetails = FlightsDet;
                //itinflights.Stock = ConfigurationManager.AppSettings["GDSSTOCK"].ToString();
                //itinflights.FareType = "N";
                //itinflights.FareTypeDescription = "";

                //ListItenar.Add(itinflights);

                SegDet.BaseOrigin = BaseOrgin;
                SegDet.BaseDestination = BaseDestination;
                SegDet.Adult = strAdults;
                SegDet.Child = strChildrens != null && strChildrens != "" ? strChildrens.Trim() : "0";
                SegDet.Infant = strInfants != null && strInfants != "" ? strInfants.Trim() : "0";
                SegDet.SegmentType = SegmentType;
                SegDet.ClassType = lst[0][0].ClassType;
                SegDet.BookingType = "B2B";
                SegDet.TripType = (Trip == "Y") ? "R" : Trip;
                SegDet.AppType = "W";
                SegDet.SinglePNR = RtripComparFlg;
                SegDet.RTSpecial = Specialflagfare == "Y" ? "Y" : "";



                string strClientID = string.Empty; string strClientTerminalID = string.Empty;
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


                RQRS.AgentDetails agent = new RQRS.AgentDetails();
                agent.AgentId = Session["agentid"] != null && Session["agentid"] != "" ? Session["agentid"].ToString() : "";
                agent.Agenttype = strTerminalType == "T" ? strClientType : strAgentType;
                agent.AirportID = SegmentType;

                agent.AppType = "B2B";
                agent.APPCurrency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                agent.BOAID = strClientID;
                agent.BOAterminalID = strClientTerminalID;
                agent.BranchID = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                agent.IssuingBranchID = BranchID != null && BranchID != "" ? BranchID : Session["branchid"] != null && Session["branchid"] != "" ? Session["branchid"].ToString() : "";
                agent.ClientID = strClientID;
                agent.CoOrdinatorID = "";
                agent.Environment = "B";
                agent.Platform = "B";
                agent.ProjectID = Session["PRODUCT_CODE"] != null && Session["PRODUCT_CODE"] != "" ? Session["PRODUCT_CODE"].ToString() : "";
                agent.TerminalId = Session["terminalid"] != null && Session["terminalid"] != "" ? Session["terminalid"].ToString() : "";
                agent.UserName = Session["username"].ToString();
                agent.Version = ConfigurationManager.AppSettings["APIVersion"].ToString();
                agent.COST_CENTER = "";
                agent.EMP_ID = "";
                agent.UID = Session["UniqueIduser"] != null ? Session["UniqueIduser"].ToString() : "";
                agent.Group_ID = GroupID != null && GroupID != "" ? GroupID : "";

                PriceIti.AgentDetails = agent;
                PriceIti.BestBuyOption = false;
                PriceIti.Stock = ConfigurationManager.AppSettings["GDSSTOCK"].ToString();
                PriceIti.AlterQueue = AlterQueue;
                PriceIti.SegmnetDetails = SegDet;
                PriceIti.ItinearyDetails = ListItenar;



                ViewBag.txt_UserName = Session["username"].ToString();
                string[] PP = Session["agencyaddress"].ToString().Split('~');
                if (PP.Count() >= 4)
                {
                    ViewBag.txt_AgnNo = PP[4];
                }
                string request = JsonConvert.SerializeObject(PriceIti).ToString();
                if (Session["Pricerequest"] != null && Session["Pricerequest"] != "")
                {
                    Session["Pricerequest"] = request.ToString();
                }
                else
                {
                    Session.Add("Pricerequest", request);
                }
                string Query = "InvokeHostCheck";
                int hostchecktimeout = Convert.ToInt32(ConfigurationManager.AppSettings["hostchecktimeout"]);

                MyWebClient client = new MyWebClient();
                client.LintTimeout = hostchecktimeout;
                client.Headers["Content-type"] = "application/json";
                #region Log

                string ReqTime = "SelectReqest" + DateTime.Now;
                StringWriter strWriter = new StringWriter();
                DataSet dsrequest = new DataSet();
                dsrequest = Serv.convertJsonStringToDataSet(request, "");
                dsrequest.WriteXml(strWriter);

                if (ConfigurationManager.AppSettings["Splavailagent"].ToString() != "" && ConfigurationManager.AppSettings["Splavailagent"].ToString().Contains(Session["POS_TID"].ToString()))
                {
                    strURLpath = ConfigurationManager.AppSettings["Spl_APPS_SELECT_URL"].ToString();
                }
                else
                {
                    strURLpath = ConfigurationManager.AppSettings["APPS_SELECT_URL"].ToString();
                }
                string lstrCat = Trip + " ~SRQ~" + PriceIti.Stock.ToString()
                    + "~" + BaseOrgin + "~" + BaseDestination;
                string LstrDetails = "<SELECT_REQUEST><URL>[<![CDATA[" + strURLpath
                    + "]]>]</URL><QUERY>" + Query + "</QUERY><REQTIME>" + ReqTime + "</REQTIME><TIMEOUT>" + (hostchecktimeout).ToString() + "</TIMEOUT>" + ((Base.ReqLog) ?
                    strWriter.ToString() : request) + "<JSON>" + request + "</JSON></SELECT_REQUEST>";
                DatabaseLog.LogData(Session["username"].ToString(), "E", "Search Controler", "GdsSelectedFltDetails", LstrDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                #endregion



                byte[] data = client.UploadData(strURLpath + Query, "POST", Encoding.ASCII.GetBytes(request));
                string strResponse = System.Text.Encoding.ASCII.GetString(data);


                if (string.IsNullOrEmpty(strResponse))
                {

                    #region log
                    string lstrCata = Trip + " ~SRN~" + PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;
                    DatabaseLog.LogData(Session["username"].ToString(), "E", "Search Controler", "GdsSelectedFltDetails", lstrCata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                    #endregion

                }
                else
                {
                    DataSet dsselect = new DataSet();
                    dsselect = Serv.convertJsonStringToDataSet(strResponse, "");
                    DataTable dsselect1 = new DataTable();
                    dsselect1 = dsselect.Tables[0];
                    Session.Add("Dobmand" + ValKey, dsselect1);
                    Session.Add("valkey", ValKey);
                    string Erroralert = string.Empty;
                    _availRes = JsonConvert.DeserializeObject<RQRS.PriceItenaryRS>(strResponse);
                    if (_availRes.ResultCode == "1" || _availRes.ResultCode == "2")
                    {

                        #region GST Details and Balance in a single method (Added on 20190226 by STS185)
                        DataSet dsRequest = new DataSet();
                        dsselect = Serv.convertJsonStringToDataSet(request, "");

                        string strRequestDetails = JsonConvert.SerializeObject(dsselect);
                        DataTable dtGSTExistingDetails = new DataTable();
                        DataSet dsGSTExistingDetails = new DataSet();
                        string gstMandatory = string.Empty;
                        string strDispInfoMessage = string.Empty;
                        DataTable getdtGSTRegisterDetails = new DataTable();
                        DataTable getBTACardtype = new DataTable();
                        DataTable getPaxtitle = new DataTable();

                        string getGSTMandatory = string.Empty;
                        bool getLastNameMandatory = false;
                        bool getAllowLCCBlockPNR = false;

                        STSTRAVRAYS.Rays_service.RaysService _rayservice = new STSTRAVRAYS.Rays_service.RaysService();
                        _rayservice.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
                        try
                        {
                            #region Log For GST

                            string ReqTime1 = "GSTRequest" + DateTime.Now;

                            string lstrCat1 = Trip + " ~SRQ~" + PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;
                            string LstrDetails1 = "<SELECT_REQUEST_GST><URL>[<![CDATA[" + Session["travraysws_url"] + "/" + Session["travraysws_vdir"] + "/Rays.svc/"
                                + "]]>]</URL><QUERY>" + Query + "</QUERY><REQTIME>" + ReqTime + "</REQTIME><TIMEOUT>" + (hostchecktimeout).ToString() + "</TIMEOUT>" + ((Base.ReqLog) ?
                                strWriter.ToString() : request) + "<JSON>" + request + "</JSON></SELECT_REQUEST_GST>";
                            DatabaseLog.LogData(Session["username"].ToString(), "E", "Search Controler", "GdsSelectedFltDetails", LstrDetails1.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                            #endregion


                            string referrormsg = string.Empty;
                            _rayservice.FecthGSTandPopupMsgForClient(strClientID, strClientTerminalID, strRequestDetails,
                                strUserName, "", Ipaddress, sequnceID.ToString(), ref gstMandatory, ref getLastNameMandatory, ref dsGSTExistingDetails, ref strDispInfoMessage, strTerminalType, ref referrormsg, "");
                            if (dsGSTExistingDetails != null && dsGSTExistingDetails.Tables.Count > 0)
                            {
                                getdtGSTRegisterDetails = dsGSTExistingDetails.Tables.Contains("GSTDETAILS") && dsGSTExistingDetails.Tables["GSTDETAILS"].Rows.Count > 0 ? dsGSTExistingDetails.Tables["GSTDETAILS"].Copy() : null;
                                getBTACardtype = dsGSTExistingDetails.Tables.Contains("P_LOAD_MANUAL_GST2") && dsGSTExistingDetails.Tables["P_LOAD_MANUAL_GST2"].Rows.Count > 0 ? dsGSTExistingDetails.Tables["P_LOAD_MANUAL_GST2"].Copy() : null;
                                if (getdtGSTRegisterDetails != null && dsGSTExistingDetails.Tables["GSTDETAILS"].Rows.Count > 0)
                                {
                                    ViewData["GSTDETAILS"] = JsonConvert.SerializeObject(getdtGSTRegisterDetails);
                                }
                                else
                                {
                                    ViewData["GSTDETAILS"] = "";
                                }

                                if (dsGSTExistingDetails.Tables.Contains("P_FETCH_POS_CREDIT_BALANCE") == true && dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows.Count != 0)
                                {
                                    string Allowbokingtype = dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CLT_ALLOW_ACC_TYPE"].ToString().Trim();

                                    if (Allowbokingtype.Contains("C"))
                                    {
                                        Session["ALLOW_CREDIT"] = "Y";
                                    }
                                    else
                                    {
                                        Session["ALLOW_CREDIT"] = "N";
                                    }
                                    if (Allowbokingtype.Contains("T"))
                                    {
                                        Session["ALLOW_TOPUP"] = "Y";
                                    }
                                    else
                                    {
                                        Session["ALLOW_TOPUP"] = "N";
                                    }
                                    if (Allowbokingtype.Contains("P"))
                                    {
                                        Session["ALLOW_PG"] = "Y";
                                    }
                                    else
                                    {
                                        Session["ALLOW_PG"] = "N";
                                    }


                                    if (Allowbokingtype.Contains("B"))
                                    {
                                        Session["ALLOW_PAS"] = "Y";
                                    }
                                    else
                                    {
                                        Session["ALLOW_PAS"] = "N";
                                    }
                                    for (int i = 0; i < dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows.Count; i++)
                                    {
                                        string ccur = dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[i]["CLT_ALLOW_ACC_TYPE"].ToString().Trim();
                                        string appcurrency = dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[i]["CCD_CURRENCY_CODE"].ToString().Trim();
                                        string[] ss = ccur.Split('|');
                                        if (ss.Length > 0)
                                        {
                                            ViewBag.CltTopBalance = (dsGSTExistingDetails.Tables.Contains("P_FETCH_POS_CREDIT_BALANCE") && dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows.Count > 0) ? dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CLT_OPENING_BALANCE"].ToString() : "";
                                            ViewBag.CltCreditBalance = (dsGSTExistingDetails.Tables.Contains("P_FETCH_POS_CREDIT_BALANCE") && dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows.Count > 0) ? dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[0]["CREDITBALANCE"].ToString() : "";

                                            for (int j = 0; j < ss.Length; j++)
                                            {
                                                if (ss[j] == "C")
                                                {
                                                    balfetch += "Credit : " + appcurrency + " " + dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[i]["CREDITBALANCE"].ToString() + "|";
                                                }
                                                if (ss[j] == "T")
                                                {
                                                    balfetch += "Top-up : " + appcurrency + " " + dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE"].Rows[i]["CLT_OPENING_BALANCE"].ToString() + "|";
                                                }

                                            }
                                        }
                                    }
                                    if (dsGSTExistingDetails.Tables.Count > 8 && dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE6"].Rows.Count > 0)
                                    {
                                        agninfo = dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE6"].Rows[0]["CLT_CLIENT_ID"].ToString() + "~" + dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE6"].Rows[0]["CLT_CLIENT_NAME"].ToString() + "~" + dsGSTExistingDetails.Tables["P_FETCH_POS_CREDIT_BALANCE6"].Rows[0]["CLT_WINYATRA_ID"].ToString();
                                    }
                                }

                                ViewBag.agninfo = agninfo;
                                ViewBag.balsfet = balfetch;

                            }
                            else
                            {
                                #region log
                                string lstrCata = Trip + " ~SRN~" +
                                   PriceIti.Stock.ToString()
                               + "~" + BaseOrgin + "~" + BaseDestination;

                                DatabaseLog.LogData(Session["username"].ToString(), "E", "Search Controler", "GdsSelectedFltDetails", lstrCata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            getdtGSTRegisterDetails = null;
                            getLastNameMandatory = true;
                        }

                        if (!string.IsNullOrEmpty(strDispInfoMessage.ToString().Trim()))
                            // MessageBox.Show(strDispInfoMessage, Common.MSGBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            if (dtSelectResponse != null && dtSelectResponse.Rows.Count != 0)
                                getAllowLCCBlockPNR = dtSelectResponse.Columns.Contains("AllowBlockPNR") ? (dtSelectResponse.Rows[0]["AllowBlockPNR"].ToString().Trim().ToUpper() == "TRUE" ? true : false) : false;
                        #endregion




                        List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;
                        List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;


                        Session.Add("sSelectDataforBooking" + ValKey, _availRes);
                        Session.Add("sStrAirlinsecatagory" + ValKey, strAirlinsecatagory);

                        RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                        bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                        TokenBooking = lstrPriceItenary[0].Token;
                        _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                        _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                        _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                        _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                        _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;
                        //if (Multireqst == "Y")
                        //   {
                        //       Session.Add("token" + reqcnt + ValKey, TokenBooking);
                        //   }

                        //if ((PriceIti.ItinearyDetails != null) && (PriceIti.ItinearyDetails.Count > 0) && lstrPriceItenary[0].PriceRS != null &&
                        //                                    (lstrPriceItenary[0].PriceRS.Count > 0) &&
                        //    (flightcounts == lstrPriceItenary[0].PriceRS[0].Flights.Count() ||
                        //   lstrPriceItenary.Count > 1))
                        if ((PriceIti.ItinearyDetails != null) && (PriceIti.ItinearyDetails.Count > 0) && lstrPriceItenary[0].PriceRS != null &&
                                                             (lstrPriceItenary[0].PriceRS.Count > 0) &&
                             (flightcounts == lstrPriceItenary[0].PriceRS[0].Flights.Count() ||
                            lstrPriceItenary.Count > 1))
                        {
                            ViewBag.Bookticketoption = Session["ticketing"] != null ? Session["ticketing"].ToString().Trim() : "N";

                            if (lstrPriceItenary.Count > 1)
                            {
                                List<RQRS.Flights> FlightDetail = new List<RQRS.Flights>();
                                List<RQRS.Flights> FlightDetailfin = new List<RQRS.Flights>();
                                List<List<RQRS.Flights>> Lstlstflgtdetails = new List<List<RQRS.Flights>>();

                                for (int indx = 0; indx < lstrPriceItenary.Count; indx++)
                                {
                                    FlightDetail = new List<RQRS.Flights>();
                                    FlightDetail = lstrPriceItenary[indx].PriceRS[0].Flights;

                                    for (int myindx = 0; myindx < FlightDetail.Count(); myindx++)
                                    {
                                        RQRS.Flights flightdet = new RQRS.Flights();
                                        flightdet = FlightDetail[myindx];
                                        FlightDetailfin.Add(flightdet);
                                    }
                                    if (indx == 0)
                                    {
                                        Lstlstflgtdetails = FlightDetail.GroupBy(v => v.ItinRef).Select(j => j.ToList()).ToList();
                                    }
                                    else
                                    {
                                        Lstlstflgtdetails.AddRange(FlightDetail.GroupBy(v => v.ItinRef).Select(j => j.ToList()).ToList());
                                    }
                                }
                                ViewBag.FltDets = JsonConvert.SerializeObject(FlightDetailfin);
                                ViewBag.LstofLstFltDetails = JsonConvert.SerializeObject(Lstlstflgtdetails);
                                ViewBag.FltDets = JsonConvert.SerializeObject(FlightDetailfin);
                            }
                            else
                            {
                                ViewBag.FltDets = JsonConvert.SerializeObject(lstrPriceItenary[0].PriceRS[0].Flights);
                                ViewBag.LstofLstFltDetails = JsonConvert.SerializeObject(lstrPriceItenary[0].PriceRS[0].Flights.GroupBy(v => v.ItinRef).Select(j => j.ToList()).ToList());
                            }
                            string str_Allow_Block_Pnr = (_availRes.AllowBlockPNR != null && _availRes.AllowBlockPNR != "" && _availRes.AllowBlockPNR.ToString().ToUpper() == "TRUE" ? "Y" : "N");

                            ViewBag.Blockticketoption = Session["block"] != null && Session["block"].ToString().Trim() == "Y" && str_Allow_Block_Pnr == "Y" ? "Y" : "N";
                            ViewBag.hdnbestbuyallow = _availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[3] : "N";  //added by Rajesh
                            ViewBag.hdnbargainflag = _availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[2] : "N";  //added by Rajesh
                            ViewBag.hdnallowtune = _availRes.Bargainflag != null && _availRes.Bargainflag != "" ? _availRes.Bargainflag.ToString().Split('|')[4] : "N";  //added by Rajesh
                            ViewBag.hdnallowtune = ConfigurationManager.AppSettings["Book_ins"].ToString();
                            ViewBag.hdnDObMand = _availRes.DOBMandatory != null && _availRes.DOBMandatory != "" ? _availRes.DOBMandatory.ToString().ToUpper() : "TRUE";
                            ViewBag.hdnPassmand = _availRes.PassportMandatory != null && _availRes.PassportMandatory != "" ? _availRes.PassportMandatory.ToString().ToUpper() : "TRUE";

                            ViewBag.shoqcommitons = Session["commission"] != null ? Session["commission"].ToString().Trim() : "N";

                            string strErrtemp = "";
                            string GrossAmnt = "";
                            #region log


                            StringWriter strresWriter = new StringWriter();
                            DataSet dsresponse1 = new DataSet();
                            dsresponse1 = Serv.convertJsonStringToDataSet(strResponse, "");
                            dsresponse1.WriteXml(strresWriter);

                            string lstrtime = "SelectResponse" + DateTime.Now;

                            string lstrCata = Trip + " ~SRS~" +
                           PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;

                            string LstrRespDetails = "<SELECT_RESPONSE><REQTIME>" + ReqTime + "</REQTIME><RESPONSETIME>" + lstrtime + "</RESPONSETIME>"
                                 + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                           "<Flights>" + (_availResponse == null || _availResponse.Flights == null ? "NULL" : _availResponse.Flights.Count.ToString()) + "</Flights>" +
                           "<Fares>" + (_availResponse == null || _availResponse.Fares == null ? "NULL" : _availResponse.Fares.Count.ToString()) + "</Fares>" +
                           "<Bagg>" + (_availResponse == null || _availResponse.Bagg == null ? "NULL" : _availResponse.Bagg.Count.ToString()) + "</Bagg>" +
                           "<Meal>" + (_availResponse == null || _availResponse.Meal == null ? "NULL" : _availResponse.Meal.Count.ToString()) + "</Meal>" +
                           "<DOBMandatory>" + (_availRes == null || _availRes.DOBMandatory == null ? "NULL" : _availRes.DOBMandatory.ToString()) + "</DOBMandatory>" +
                           "<PassportMandatory>" + (_availRes == null || _availRes.PassportMandatory == null ? "NULL" : _availRes.PassportMandatory.ToString()) + "</PassportMandatory>"
                                + strresWriter.ToString() + "</SELECT_RESPONSE>";

                            DatabaseLog.LogData(Session["username"].ToString(), "E", "Search Controler", "GdsSelectedFltDetails", LstrRespDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                            # endregion
                            string[] GAmnt;

                            Session.Add("BestBuy" + ValKey, bestbuy.ToString());
                            Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                            Session.Add("Requestmarkup" + ValKey, (oldfaremarkup.ToString() != "" && oldfaremarkup.ToString() != null ? oldfaremarkup : "0"));

                            if (bestbuy == true)
                            {
                                ViewBag.privtfareoldgros = (string.IsNullOrEmpty((_availResponse.Fares[0].Faredescription[0].OldFare).ToString()) ? "0" : _availResponse.Fares[0].Faredescription[0].OldFare);
                            }

                            dtBaggageSelect = new DataTable();
                            dtmealseSelect = new DataTable();
                            dtOtherSsrsel = new DataTable();
                            dtOtherbaggout = new DataTable();
                            Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse, ref strErrtemp,
                              ref dtBaggageSelect, ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtOtherbaggout);


                            if (Trip == "M" && SegmentType == "D")
                            {
                                Session.Add("dtBookreq_table" + ValKey, dtBookreq);
                            }
                            else
                            {
                                Session.Add("dtBookreq_table", dtBookreq);
                            }

                            if (dtSelectResponse.Rows.Count > 0)
                            {
                                if (lstrPriceItenary.Count() == 1)
                                {
                                    var temp = (from p in dtSelectResponse.AsEnumerable()
                                                group p by p["SegRef"].ToString() into g
                                                select g.FirstOrDefault()).Distinct();

                                    if (temp.Count() > 0)
                                    {
                                        dtSelectResponse = new DataTable();
                                        dtSelectResponse = temp.CopyToDataTable();
                                    }
                                }
                            }
                            //End...

                            if (!string.IsNullOrEmpty(strErrtemp))
                            {
                                Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                                array_Response[ResultCode] = 0;
                                ViewBag.resultcode = "0";
                                array_Response[ErrorMsg] = "Unable to select flight";
                                ViewBag.ErrorMsg = "Unable to select flight-(#03)";

                            }
                            if (dtSelectResponse != null
                                          && dtSelectResponse.Columns.Count > 1
                                          && dtSelectResponse.Rows.Count > 0)
                            {

                                if (TerminalDetails.Count() == dtSelectResponse.Rows.Count)
                                {
                                    for (int index = 0; index < dtSelectResponse.Rows.Count; index++)
                                    {
                                        dtSelectResponse.Rows[index]["DESTERMINAL"] = TerminalDetails[index].Split('\n').Length > 3 ?
                        (TerminalDetails[index].Split('\n')[3].Split(':').Length > 1 ?
                        TerminalDetails[index].Split('\n')[3].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";

                                        dtSelectResponse.Rows[index]["ORGTERMINAL"] = TerminalDetails[index].Split('\n').Length > 2 ?
                       (TerminalDetails[index].Split('\n')[2].Split(':').Length > 1 ?
                       TerminalDetails[index].Split('\n')[2].Split(':')[1].Trim().TrimEnd('\r').Trim() : "N/A") : "N/A";
                                        dtSelectResponse.AcceptChanges();
                                    }

                                }
                                GrossAmnt = dtSelectResponse.Rows[0]["GrossAmount"].ToString();
                                if (dtSelectResponse.Rows[0]["GrossAmount"].ToString().Contains("|"))
                                {
                                    GAmnt = GrossAmnt.Split('|');
                                    GrossAmnt = GAmnt[0];
                                }

                                if (bestbuy == false)
                                {
                                    var linq_Grs = (from _lsiti in PriceIti.ItinearyDetails.AsEnumerable()
                                                    group _lsiti by _lsiti.GrossAmount
                                                        into Grsfare
                                                    select new
                                                    {
                                                        GrsAmount = (Grsfare.Sum(A => Convert.ToDecimal(A.GrossAmount)))
                                                    }).ToList().Sum(A => A.GrsAmount);

                                    decimal checkdiff = (Base.ServiceUtility.ConvertToDecimal(GrossAmnt) - Base.ServiceUtility.ConvertToDecimal(linq_Grs.ToString()));
                                    decimal limitroundvalue = (checkdiff < 0 ? (-1 * checkdiff) : checkdiff);
                                    decimal checklimit = Base.ServiceUtility.ConvertToDecimal(ConfigurationManager.AppSettings["Farechecklimit"].ToString());

                                    if ((Convert.ToDouble(GrossAmnt) == Convert.ToDouble(linq_Grs.ToString()) || checklimit >= limitroundvalue) && _availRes.ResultCode == "1") //if (GrossAmnt == linq_Grs.ToString() && _availRes.ResultCode == "1")
                                    {

                                        dtSelectResponse.TableName = "TrackFareDetails";
                                        //if (Multireqst == "Y")
                                        //{
                                        //    Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                        //}
                                        //else
                                        //{
                                        //    Session.Add("Response" + ValKey, dtSelectResponse);
                                        //}
                                        Session.Add("Response" + ValKey, dtSelectResponse);//177


                                        Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                        Session.Add("BaseDest" + ValKey, BaseDestination);
                                        Session.Add("TripType" + ValKey, Trip);
                                        Session.Add("Specialflagfare" + ValKey, Specialflagfare);
                                        Session.Add("TokenBooking" + ValKey, TokenBooking);
                                        Session.Add("Deaprt" + ValKey, Deaprt);
                                        Session.Add("Arrive" + ValKey, Arrive);
                                        Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                        Session.Add("RetDate" + ValKey, ARRDATE);
                                        Session.Add("Stock" + ValKey, StkType);
                                        Session.Add("segmclass" + ValKey, Class);
                                        Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                        Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                        ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);

                                        if (Nfarecheck == "FALSE" && _availResponse.FareCheck.CheckFlag == "0")
                                        {
                                            string Currency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                                            Returnval = "Fare has been revised from :" + Currency + (Base.ServiceUtility.CovertToDouble(PriceIti.ItinearyDetails[0].GrossAmount.ToString())
                                           + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                            + " to :" + Currency +
                                            (Base.ServiceUtility.CovertToDouble(SplitFareByAdultPax(dtSelectResponse.Rows[0]["GROSSAMOUNT"].ToString()).ToString())
                                           + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                           + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString()
                                           + Environment.NewLine + " Do you want to continue the flight selection");

                                            array_Response[ResultCode] = 2;
                                            ViewBag.resultcode = "2";
                                            ViewBag.status = "02";
                                            array_Response[ErrorMsg] = Returnval;
                                            //array_Response[ErrorMsg1] = Returnval;
                                            ViewBag.ErrorMsg = Returnval;
                                        }
                                        else if (_availRes.PriceItenarys[0].Error != null && _availRes.PriceItenarys[0].Error != "")
                                        {
                                            array_Response[ResultCode] = 2;
                                            ViewBag.resultcode = "2";
                                            ViewBag.status = "02";
                                            array_Response[ErrorMsg] = _availRes.PriceItenarys[0].Error;
                                            //array_Response[ErrorMsg1] = Returnval;
                                            ViewBag.ErrorMsg = _availRes.PriceItenarys[0].Error;

                                        }
                                        else
                                        {
                                            array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                            ViewBag.resultcode = _availRes.ResultCode.ToString();
                                            ViewBag.status = _availRes.ResultCode.ToString();
                                            array_Response[ErrorMsg] = _availRes.Error;
                                            ViewBag.ErrorMsg = _availRes.Error + "-(#01)";
                                        }


                                    }

                                    else
                                    {

                                        dtSelectResponse.TableName = "TrackFareDetails";
                                        //if (Multireqst == "Y")
                                        //{
                                        //    Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                        //}
                                        //else
                                        //{
                                        //    Session.Add("Response" + ValKey, dtSelectResponse);
                                        //}

                                        Session.Add("Response" + ValKey, dtSelectResponse);
                                        Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                        Session.Add("BaseDest" + ValKey, BaseDestination);
                                        Session.Add("TripType" + ValKey, Trip);
                                        Session.Add("Specialflagfare" + ValKey, Specialflagfare);
                                        Session.Add("TokenBooking" + ValKey, TokenBooking);
                                        Session.Add("Deaprt" + ValKey, Deaprt);
                                        Session.Add("Arrive" + ValKey, Arrive);
                                        Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                        Session.Add("RetDate" + ValKey, ARRDATE);
                                        Session.Add("segmclass" + ValKey, Class);
                                        Session.Add("Stock" + ValKey, StkType);
                                        Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                        Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);
                                        ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);

                                        var linq_Grss = (from _lsiti in PriceIti.ItinearyDetails.AsEnumerable()
                                                         group _lsiti by _lsiti.GrossAmount
                                                             into Grsfare
                                                         select new
                                                         {
                                                             GrsAmount = (Grsfare.Sum(A => Convert.ToDecimal(A.GrossAmount)))
                                                         }).ToList().Sum(A => A.GrsAmount);

                                        if (Convert.ToDouble(GrossAmnt) != Convert.ToDouble(linq_Grs.ToString())) //  if (GrossAmnt != Convert.ToDecimal(linq_Grs).ToString("0.00"))                                           
                                        {
                                            string Currency = Session["App_currency"] != null && Session["App_currency"] != "" ? Session["App_currency"].ToString() : ConfigurationManager.AppSettings["currency"].ToString();
                                            Returnval = "Fare has been revised from :" + Currency + " "
                                                + (Base.ServiceUtility.CovertToDouble(linq_Grss.ToString())
                                                + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                                + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString() + " to :"
                                                + Currency + " "
                                                + (Base.ServiceUtility.CovertToDouble(SplitFareByAdultPax(dtSelectResponse.Rows[0]["GROSSAMOUNT"].ToString()).ToString())
                                                + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["Servicecharge"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["Servicecharge"].ToString()).ToString() : "0"))
                                                + Base.ServiceUtility.CovertToDouble((dtSelectResponse.Rows[0]["MarkUp"].ToString() != "" ? SplitFareByAdultPax(dtSelectResponse.Rows[0]["MarkUp"].ToString()).ToString() : "0"))).ToString()
                                                + Environment.NewLine + " Do you want to continue the flight selection";
                                            ViewBag.ErrorMsg = Returnval;

                                            Erroralert += (Erroralert != "" ? Environment.NewLine + Returnval : Returnval);
                                        }

                                        if (_availRes.Error != null && _availRes.Error != "")
                                        {
                                            Returnval = _availRes.Error;
                                            ViewBag.ErrorMsg = Returnval;
                                        }
                                        //Returnval = _availRes.PriceItenarys[0].Error;
                                        array_Response[ResultCode] = 2;
                                        ViewBag.resultcode = "2";
                                        ViewBag.status = "02";
                                        array_Response[ErrorMsg] = Returnval;

                                        //array_Response[ErrorMsg1] = Returnval;
                                    }
                                }
                                else
                                {

                                    dtSelectResponse.TableName = "TrackFareDetails";
                                    //if (Multireqst == "Y")
                                    //{
                                    //    Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                                    //}
                                    //else
                                    //{
                                    //    Session.Add("Response" + ValKey, dtSelectResponse);
                                    //}

                                    Session.Add("Response" + ValKey, dtSelectResponse);
                                    Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                                    Session.Add("BaseDest" + ValKey, BaseDestination);
                                    Session.Add("TripType" + ValKey, Trip);
                                    Session.Add("Specialflagfare" + ValKey, Specialflagfare);
                                    Session.Add("TokenBooking" + ValKey, TokenBooking);
                                    Session.Add("Deaprt" + ValKey, Deaprt);
                                    Session.Add("Arrive" + ValKey, Arrive);
                                    Session.Add("DepartureDate" + ValKey, DEPTDATE);
                                    Session.Add("RetDate" + ValKey, ARRDATE);
                                    Session.Add("Stock" + ValKey, StkType);
                                    Session.Add("segmclass" + ValKey, Class);
                                    Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                                    Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);

                                    ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);
                                    array_Response[ResultCode] = _availRes.ResultCode.ToString();
                                    ViewBag.resultcode = _availRes.ResultCode.ToString();
                                    ViewBag.status = _availRes.ResultCode.ToString();
                                    array_Response[ErrorMsg] = _availRes.Error;
                                    ViewBag.ErrorMsg = _availRes.Error + "-(#01)";
                                }

                                int IfCount = dtSelectResponse.Rows.Count;
                                int fCount = 0;

                                foreach (var Li in PriceIti.ItinearyDetails[0].FlightDetails)
                                {
                                    if (fCount <= IfCount)
                                    {
                                        if (dtSelectResponse.Rows[fCount]["FareBasisCode"].ToString() != Li.FareBasisCode && ConfigurationManager.AppSettings["Revisedclassalert"].ToString().Contains(StkType) == false)
                                        {
                                            Returnval = "Class has been revised from :" + Li.FareBasisCode + " to :" + dtSelectResponse.Rows[0]["FareBasisCode"].ToString() + Environment.NewLine + " Do you want to continue the flight selection";
                                            array_Response[ResultCode] = 2;
                                            ViewBag.resultcode = "2";
                                            ViewBag.status = "02";
                                            ViewBag.ErrorMsg = Returnval;
                                            array_Response[ErrorMsg] = Returnval;
                                        }
                                        fCount++;
                                    }
                                }
                            }
                            else
                            {

                                Returnval = "Problem occured while select a flight";
                                ViewBag.ErrorMsg = "Problem occured while select a flight";
                                array_Response[ResultCode] = 0;
                                ViewBag.resultcode = "0";
                                ViewBag.status = "00";
                                ViewBag.ErrorMsg = Returnval;
                                array_Response[ErrorMsg] = Returnval;
                            }

                        }
                        else
                        {
                            string XML = "<EVENT><URL>[<![CDATA[" + strResponse + "]]>]</URL></EVENT>";
                            DatabaseLog.LogData(Session["username"].ToString(), "E", "Search Controler", "GdsSelectedFltDetails", XML.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                            array_Response[ResultCode] = 0;
                            ViewBag.resultcode = "0";
                            ViewBag.status = "00";
                            array_Response[ErrorMsg] = "Selected flight not available";
                            ViewBag.ErrorMsg = "Selected flight not available" + "-(#03)";
                        }

                    }
                    else if (_availRes.ResultCode == "3")
                    {
                        string strErrtemp = "";
                        List<RQRS.PriceItenary> lstrPriceItenary = _availRes.PriceItenarys;
                        List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;
                        RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                        bool bestbuy = _availResponse.Fares[0].Faredescription[0].BestBuyOption;

                        TokenBooking = lstrPriceItenary[0].Token;
                        _availRes.PassportMandatory = lstrPriceItenary[0].PassportMandatory;
                        _availRes.DOBMandatory = lstrPriceItenary[0].DOBMandatory;
                        _availRes.AllowSeatMap = lstrPriceItenary[0].AllowSeatMap;
                        _availRes.AllowBlockPNR = lstrPriceItenary[0].AllowBlockPNR;
                        _availRes.Bargainflag = lstrPriceItenary[0].Bargainflag;

                        #region log


                        StringWriter strresWriter = new StringWriter();
                        DataSet dsresponse = new DataSet();
                        dsresponse = Serv.convertJsonStringToDataSet(strResponse, "");
                        dsresponse.WriteXml(strresWriter);

                        string lstrtime = "SelectResponse" + DateTime.Now;

                        string lstrCata = Trip + " ~SRS~" +
                       PriceIti.Stock.ToString() + "~" + BaseOrgin + "~" + BaseDestination;

                        string LstrRespDetails = "<SELECT_RESPONSE><REQTIME>" + ReqTime + "</REQTIME><RESPONSETIME>" + lstrtime + "</RESPONSETIME>"
                             + "<data>" + (data == null ? "NULL" : data.Length.ToString()) + "</data>" +
                       "<Flights>" + (_availResponse == null || _availResponse.Flights == null ? "NULL" : _availResponse.Flights.Count.ToString()) + "</Flights>" +
                       "<Fares>" + (_availResponse == null || _availResponse.Fares == null ? "NULL" : _availResponse.Fares.Count.ToString()) + "</Fares>" +
                       "<Bagg>" + (_availResponse == null || _availResponse.Bagg == null ? "NULL" : _availResponse.Bagg.Count.ToString()) + "</Bagg>" +
                       "<Meal>" + (_availResponse == null || _availResponse.Meal == null ? "NULL" : _availResponse.Meal.Count.ToString()) + "</Meal>" +
                       "<DOBMandatory>" + (_availRes == null || _availRes.DOBMandatory == null ? "NULL" : _availRes.DOBMandatory.ToString()) + "</DOBMandatory>" +
                       "<PassportMandatory>" + (_availRes == null || _availRes.PassportMandatory == null ? "NULL" : _availRes.PassportMandatory.ToString()) + "</PassportMandatory>"
                            + strresWriter.ToString() + "</SELECT_RESPONSE>";

                        DatabaseLog.LogData(Session["username"].ToString(), "E", "Search Controler", "GdsSelectedFltDetails", LstrRespDetails.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());

                        # endregion

                        Session.Add("BestBuy" + ValKey, bestbuy.ToString());
                        Session.Add("RequestFare" + ValKey, PriceIti.ItinearyDetails[0].GrossAmount.ToString());
                        Session.Add("Requestmarkup" + ValKey, (oldfaremarkup.ToString() != "" && oldfaremarkup.ToString() != null ? oldfaremarkup : "0"));


                        dtBaggageSelect = new DataTable();
                        dtmealseSelect = new DataTable();
                        dtOtherSsrsel = new DataTable();
                        dtOtherbaggout = new DataTable();
                        Base.ServiceUtility.GridResponseforSelect(_availRes, ref dtSelectResponse, ref strErrtemp,
                            ref dtBaggageSelect, ref dtmealseSelect, ref dtOtherSsrsel, ref dtBookreq, ref dtOtherbaggout);

                        if (!string.IsNullOrEmpty(strErrtemp))
                        {
                            DatabaseLog.LogData(Session["username"].ToString(), "E", "Search Controler", "GdsSelectedFltDetails", strErrtemp.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                            Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                            array_Response[ResultCode] = 0;
                            ViewBag.resultcode = "0";
                            array_Response[ErrorMsg] = "Unable to select flight-(#03).";
                            ViewBag.ErrorMsg = "Unable to select flight" + "-(#03).";
                            ViewBag.status = "00";
                        }
                        if (dtSelectResponse != null
                                   && dtSelectResponse.Columns.Count > 1
                                   && dtSelectResponse.Rows.Count > 0)
                        {

                            dtSelectResponse.TableName = "TrackFareDetails";
                            //if (Multireqst == "Y")
                            //{
                            //    Session.Add("Response" + reqcnt + ValKey, dtSelectResponse);
                            //}
                            //else
                            //{
                            //    Session.Add("Response" + ValKey, dtSelectResponse);
                            //}

                            ViewBag.Response = JsonConvert.SerializeObject(dtSelectResponse);

                            Session.Add("BaseOrgin" + ValKey, BaseOrgin);
                            Session.Add("BaseDest" + ValKey, BaseDestination);
                            Session.Add("TripType" + ValKey, Trip);
                            Session.Add("Specialflagfare" + ValKey, Specialflagfare);
                            Session.Add("TokenBooking" + ValKey, TokenBooking);
                            Session.Add("Deaprt" + ValKey, Deaprt);
                            Session.Add("Arrive" + ValKey, Arrive);
                            Session.Add("DepartureDate" + ValKey, DEPTDATE);
                            Session.Add("RetDate" + ValKey, ARRDATE);
                            Session.Add("Stock" + ValKey, StkType);
                            Session.Add("segmclass" + ValKey, Class);
                            Session.Add("Baggageselect" + ValKey, dtBaggageSelect);
                            Session.Add("dtmealseSelect" + ValKey, dtmealseSelect);

                            array_Response[ResultCode] = _availRes.ResultCode.ToString();
                            ViewBag.resultcode = _availRes.ResultCode.ToString();
                            ViewBag.status = _availRes.ResultCode.ToString();
                            array_Response[ErrorMsg] = _availRes.Error;
                            ViewBag.ErrorMsg = _availRes.Error;
                        }
                        else
                        {
                            Returnval = "Problem occured while select a flight";
                            array_Response[ResultCode] = 0;
                            ViewBag.resultcode = "0";
                            ViewBag.status = "00";
                            ViewBag.ErrorMsg = Returnval;
                            array_Response[ErrorMsg] = Returnval;
                        }

                    }
                    else
                    {
                        Returnval = (_availRes.Error != null && (!string.IsNullOrEmpty(_availRes.Error.ToString()))) ? _availRes.Error.ToString() : "Unable to select flight";
                        array_Response[ResultCode] = 0;
                        ViewBag.resultcode = "0";
                        array_Response[ErrorMsg] = Returnval;
                        ViewBag.ErrorMsg = Returnval + "-(#01)";
                        string lstrCata = Trip + " ~SRN~" +
                        PriceIti.Stock.ToString()
                           + "~" + BaseOrgin + "~" + BaseDestination;
                        DatabaseLog.LogData(Session["username"].ToString(), "E", "Search Controler", "GdsSelectedFltDetails", lstrCata.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                        ViewBag.status = "00";
                    }

                    ViewBag.Totslpaxcount = (Adult + Child + Infant);
                    ViewBag.Adultcount = Adult;
                    ViewBag.Childcount = Child;
                    ViewBag.Infantcount = Infant;


                    List<DataRow> _listonewaydata = new List<DataRow>();
                    List<DataRow> _listroundtripdata = new List<DataRow>();
                    List<int> _listTotaldata = new List<int>();
                    ViewBag.Trips = Trip;

                    ViewBag.Sector = "";
                    ViewBag.baggage = "";
                    ViewBag.Fre_flyr_no = "";
                    ViewBag.seatmappopup = "";
                    ViewBag.Meals = "";
                    ViewBag.BaggageSSR = "";
                    ViewBag.FFN = "";
                    ViewBag.OtherSSR = "";
                    ViewBag.Insurancedata = "";
                    if (dtSelectResponse != null && dtSelectResponse.Rows.Count > 0)
                    {
                        loadsessionvalues(dtSelectResponse, Convert.ToInt32(ViewBag.Adultcount), Convert.ToInt32(ViewBag.Childcount), Convert.ToInt32(ViewBag.Infantcount), ValKey);
                        ArrayList SSRRetArray = Bll.Create_Meals_Bagg(dtSelectResponse, ValKey, dtOtherSsrsel, dtOtherbaggout, strAdults, strChildrens);


                        ViewBag.Sector = SSRRetArray[0].ToString();
                        ViewBag.baggage = SSRRetArray[2].ToString();
                        ViewBag.Fre_flyr_no = SSRRetArray[6].ToString();
                        ViewBag.frqno = SSRRetArray[5].ToString();
                        ViewBag.seatmappopup = SSRRetArray[8].ToString();
                        ViewBag.Insurancedata = SSRRetArray[11].ToString();
                        ViewBag.Otherssrdata = SSRRetArray[15].ToString();
                        ViewBag.Otherssrspicemax = SSRRetArray[17].ToString();
                        ViewBag.Otherssrprcheckinselect = SSRRetArray[19].ToString();
                        ViewBag.Otherssrbagoutselect = SSRRetArray[21].ToString();
                        ViewBag.Meals = SSRRetArray[22].ToString();
                        ViewBag.BaggageSSR = SSRRetArray[23].ToString();
                        ViewBag.FFN = SSRRetArray[24].ToString();
                        ViewBag.OtherSSR = SSRRetArray[25].ToString();
                        Session.Add("SSR", SSRRetArray[4].ToString());
                        Session.Add("FRQ", SSRRetArray[7].ToString());

                    }

                    string td_Adult, td_child, td_infant = string.Empty;

                    td_Adult = ViewBag.Adultcount;
                    td_child = ViewBag.Childcount;
                    td_infant = ViewBag.Infantcount;

                    Session.Add("PaxCount" + ValKey, td_Adult + "|" + td_child + "|" + td_infant);

                    ViewBag.ValKey = ValKey;
                    ViewBag.ServerDateTime = Base.LoadServerdatetime();
                }

            }
            catch (Exception ex)
            {
                ViewBag.status = "00";
                Returnval = ex.Message;
                DatabaseLog.LogData(Session["username"].ToString(), "X", "Search Controler", "GdsSelectedFltDetails", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                array_Response[ResultCode] = 0;
                ViewBag.resultcode = "0";
                array_Response[ErrorMsg] = Returnval;
                ViewBag.ErrorMsg = Returnval + "-(#05)";
            }
            ViewBag.Multireqst = "N";
            string Multireqst = "";
            if (Multireqst == "Y")
            {
                return Json(new
                {
                    status = ViewBag.status,
                    Message = ViewBag.ErrorMsg,
                    Result = ViewBag.Response,
                    adtcount = ViewBag.Adultcount,
                    chdcount = ViewBag.Childcount,
                    infcount = ViewBag.Infantcount,
                    shoqcommitons = ViewBag.shoqcommitons,
                    hdnDObMand = ViewBag.hdnDObMand,
                    hdnPassmand = ViewBag.hdnPassmand,
                    Totslpaxcount = ViewBag.Totslpaxcount,
                    ValKey = ViewBag.ValKey,
                    resultcode = ViewBag.resultcode
                    //token = TokenBooking
                });
            }
            else
            {
                if (System.Configuration.ConfigurationManager.AppSettings["APP_HOSTING"] == "BSA")
                {
                    return PartialView("_AvailSelect_BSA", "");
                }
                else
                {
                    return PartialView("_AvailSelect", "");
                }
            }

        }

        private decimal SplitFareByAdultPax(string amount)
        {
            decimal adultFare = 0;
            try
            {
                string[] fareSplitUp = amount.Split('|');
                adultFare = Convert.ToDecimal(fareSplitUp[0].ToString(), CultureInfo.InvariantCulture);

                return adultFare;
            }
            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "SplitFareByAdultPax", "Newavailservice");
                return adultFare;
            }
        }

        public void loadsessionvalues(DataTable getflightdetails, int td_Adult, int td_child, int td_infant, string ValKey)
        {
            //var totcom = "";
            //var totinst = "";
            double tSCh = 0;
            //double tmarkup = 0;
            string bustbuy = string.Empty;
            string bustbuyfareoldfare = string.Empty;
            string bestbuyoldmarkup = string.Empty;

            if (getflightdetails.Rows[0]["GrossAmount"].ToString().Contains('|'))
            {
                string[] paxtype = getflightdetails.Rows[0]["PaxType"].ToString().Split('|');
                string[] ServSplit = getflightdetails.Rows[0]["ServiceCharge"].ToString().Split('|');

                tSCh = Base.ServiceUtility.CovertToDouble(ServSplit[0]) * td_Adult;
                if (paxtype.Length == 2)
                {
                    var countOfpax = paxtype[1] == "INF" ? td_infant : td_child;
                    var TSCh = Base.ServiceUtility.CovertToDouble(ServSplit[1]) * countOfpax;
                    tSCh += TSCh;
                }
                else if (paxtype.Length == 3)
                {
                    if (paxtype[1] == "CHD" || paxtype[1] != "")
                    {
                        var CSCh = Base.ServiceUtility.CovertToDouble(ServSplit[1]) * td_child;
                        tSCh += CSCh;
                    }
                    if (paxtype[2] == "INF" || paxtype[2] != "")
                    {
                        var ISCh = Base.ServiceUtility.CovertToDouble(ServSplit[2]) * td_infant;
                        tSCh += ISCh;
                    }
                }

            }
            else
            {
                tSCh = Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["ServiceCharge"].ToString()) * td_Adult;
            }
            double GR = 0, Cham = 0, inft = 0, tot = 0, oldgr = 0, oldtot = 0, oldCham = 0, oldinft = 0;



            for (int i = 0; i < getflightdetails.Rows.Count; i++)
            {
                if (getflightdetails.Rows[i]["GrossAmount"].ToString().Contains('|'))
                {

                    string[] Amount = getflightdetails.Rows[i]["GrossAmount"].ToString().Split('|');
                    string[] Paxtype = getflightdetails.Rows[0]["PaxType"].ToString().Split('|');
                    string[] markup = getflightdetails.Rows[0]["markUp"].ToString().Split('|');
                    string[] oldorgamount = getflightdetails.Rows[i]["originaloldgross"].ToString().Split('|');
                    string[] oldorgmarkup = getflightdetails.Rows[0]["originaloldmarkup"].ToString().Split('|');


                    if (Amount.Length == 1)
                    {
                        GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                        oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                        tot = GR;
                        oldtot = oldgr;
                    }

                    if (Amount.Length == 2)
                    {
                        if (Paxtype[1] == "ADT")
                        {
                            if (td_infant > 0)
                                td_child = td_infant;

                            GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                            oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                            Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                            oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                            tot = GR + Cham;

                            oldtot = oldgr + oldCham;
                        }
                        if (Paxtype[1] == "CHD")
                        {
                            GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                            oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                            Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                            oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                            tot = GR + Cham;
                            oldtot = oldgr + oldCham;
                        }
                        if (Paxtype[1] == "INF")
                        {
                            GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                            oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                            inft = (td_infant * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_infant * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                            oldinft = (td_infant * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_infant * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                            tot = GR + inft;
                            oldtot = oldgr + oldinft;
                        }
                    }
                    if (Amount.Length == 3)
                    {

                        GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
                        oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
                        Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
                        oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
                        inft = (td_infant * Base.ServiceUtility.CovertToDouble(Amount[2])) + (td_infant * Base.ServiceUtility.CovertToDouble(markup[2].ToString()));
                        oldinft = (td_infant * Base.ServiceUtility.CovertToDouble(oldorgamount[2])) + (td_infant * Base.ServiceUtility.CovertToDouble(oldorgmarkup[2].ToString()));

                        tot = GR + Cham + inft;
                        oldtot = oldgr + oldCham + oldinft;


                    }

                }
                else
                {
                    GR = (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[i]["GrossAmount"].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["Markup"].ToString()));
                    oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[i]["originaloldgross"].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["originaloldmarkup"].ToString()));
                    tot = GR;
                    oldtot = oldgr;
                }
            }

            var OldT = tot;
            tot = tot + tSCh;
            if (bustbuy.ToUpper() == "TRUE")
            {
                var newOldT = oldtot;
                oldtot = oldtot + tSCh;

            }
            Session.Add("totamt" + ValKey, tot);
        }

        #endregion

        [CustomFilter]
        public ActionResult Availability()
        {
            try
            {
                ViewBag.maxdate = DateTime.Today.AddMonths(13).ToString("dd/MM/yyyy");

                string Air_Thread = ConfigurationManager.AppSettings["Air_Thread"].ToString();

                ViewBag.threadkey = Session["threadkey"] != null ? Session["threadkey"].ToString() + ((Air_Thread != "") ? "|" + Air_Thread + "" : "") : "";
                ViewBag.newthreadkey = Session["multithread"] != null ? Session["multithread"].ToString() : "";
                ViewBag.lccthreadkey = Session["lccthreadkey"] != null ? Session["lccthreadkey"].ToString() : "";
                ViewBag.fscthreadkey = Session["fscthreadkey"] != null ? Session["fscthreadkey"].ToString() : "";
                ViewBag.doublethread = Session["doublethread"] != null ? Session["doublethread"].ToString() : "";
                ViewBag.hdn_allowtrip = Session["tripConfig"] != null ? Session["tripConfig"].ToString() : "";
                ViewBag.hdnMultiThread = Session["multithreadkey"] != null ? Session["multithreadkey"].ToString() : "";

                ViewBag.threadkey_dom = Session["threadkey_dom"] != null ? Session["threadkey_dom"].ToString() : "";
                ViewBag.newthreadkey_dom = Session["newthread_dom"] != null ? Session["newthread_dom"].ToString() : "";
                ViewBag.lccthreadkey_dom = Session["lccthreadkey_dom"] != null ? Session["lccthreadkey_dom"].ToString() : "";
                ViewBag.fscthreadkey_dom = Session["fscthreadkey_dom"] != null ? Session["fscthreadkey_dom"].ToString() : "";
                ViewBag.hdnMultiThread_dom = Session["multithreadkey_dom"] != null ? Session["multithreadkey_dom"].ToString() : "";
                ViewBag.viathread = Session["VIATHREAD"] != null ? Session["VIATHREAD"].ToString() : "";

                ViewBag.hdn_checkflag = ConfigurationManager.AppSettings["Checktripflag"].ToString();
                ViewBag.hdn_country = ConfigurationManager.AppSettings["COUNTRY"].ToString();
                ViewBag.hdntrip_country = ConfigurationManager.AppSettings["trip_country"].ToString();
                ViewBag.hdnproductname = ConfigurationManager.AppSettings["Appname"].ToString();
                //ViewBag.hdnproductname = ConfigurationManager.AppSettings["Appname"].ToString();
                ViewBag.hdnsAllowmulticlass = Session["multiclass"].ToString();

                /*New for student,army,sr.citizen fare*/
                ViewBag.studentthreadkey = Session["StudentThreadkey"] != null ? Session["StudentThreadkey"].ToString() : "";
                ViewBag.armythreadkey = Session["ArmyThreadkey"] != null ? Session["ArmyThreadkey"].ToString() : "";
                ViewBag.srcitizenthreadkey = Session["SRCitizenThreadkey"] != null ? Session["SRCitizenThreadkey"].ToString() : "";

                ViewBag.studentfareMessage = Session["StudentfareMsg"] != null ? Session["StudentfareMsg"].ToString() : "";
                ViewBag.armyfareMessage = Session["ArmyfareMsg"] != null ? Session["ArmyfareMsg"].ToString() : "";
                ViewBag.srcitizenfareMessage = Session["SRCitizenfareMsg"] != null ? Session["SRCitizenfareMsg"].ToString() : "";

                string sub = Session["POS_ID"].ToString().Substring(0, 5);
                ViewBag.hdnBaseorgin = sub.ToString().Substring(2, 3);
                Session.Add("Serverdatetime", Base.LoadServerdatetime());

                ViewBag.currentdate = Base.LoadServerdatetime();
                string Errormsg = string.Empty;

                try
                {
                    string _Version = System.IO.File.ReadAllText(Server.MapPath("~/XML/VERSION/Scriptversion.txt"));
                    ViewBag.fileversion = string.IsNullOrEmpty(_Version) ? DateTime.Now.ToString("yyMMdd") : _Version;
                }
                catch (Exception ex)
                {
                    ViewBag.fileversion = DateTime.Now.ToString("yyMMdd");
                    DatabaseLog.LogData(Session["username"].ToString(), "X", "Fetchscriptversion", "Fetchscriptversion-Request filter", ex.ToString(), Session["POS_ID"].ToString(), Session["POS_TID"].ToString(), Session["sequenceid"].ToString());
                }

                ViewBag.maxdate = DateTime.Today.AddMonths(13).ToString("dd/MM/yyyy");
            }
            catch (Exception ex) { }
            return View();
        }
    }
}
