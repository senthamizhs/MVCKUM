using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.IO.Compression;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace STSTRAVRAYS.Models
{
    public class MobileLoginRQRS
    {
        public string URLQuerystring { get; set; }
        public string BannerURL { get; set; }
        public string FlightURL { get; set; }
        public string HotelURL { get; set; }
        public string HolidaysURL { get; set; }
        public string VisaURL { get; set; }
        public string Username { get; set; }
        public string MyBookingsURL { get; set; }
        public string MyUpcomingTrips { get; set; }
        public string MyCancellations { get; set; }
        public string MySupport { get; set; }
        public string ProfileImage { get; set; }
    }

    public class MobileProfileRQRS
    {
        public string strAgentID { get; set; }
        public string strTerminalID { get; set; }
        public string strUsername { get; set; }
        public string strPassword { get; set; }
        public string strProfilePIC { get; set; }
        public string strUserFName { get; set; }
        public string strUserLName { get; set; }
        public string strMobileNo { get; set; }
        public string strUserTitle { get; set; }
        public string strAddress { get; set; }
        public string strCity { get; set; }
        public string strCountry { get; set; }
        public string strPassportNo { get; set; }
        public string strDOB { get; set; }
        public string strPassportExpireDate { get; set; }
        public string strIssuedCountry { get; set; }
        public string strPincode { get; set; }
        public string strSeqNo { get; set; }

        public string strAgnType { get; set; }
        public string strBranchId { get; set; }
    }

    public abstract class Base : System.Web.UI.Page
    {
        public static bool AvailLog = true;
        public static bool ReqLog = true;
        public static bool ResLog = true;

        #region Server & Lan IP changes
        public static string ChangeToLanIP(string strContent)
        {
            string strData = strContent;
            try
            {
                string[] ArrayServerLan_IP = new string[] { };
                if (ConfigurationManager.AppSettings["SERVERLAN_IP"] != "")
                    ArrayServerLan_IP = ConfigurationManager.AppSettings["SERVERLAN_IP"].ToString().TrimEnd(',').Split(',');
                for (int j = 0; j < ArrayServerLan_IP.Length; j++)
                {
                    string[] strkeyvaluepair = ArrayServerLan_IP[j].Split('|');
                    strData = strData.Replace(strkeyvaluepair[0].ToString(), strkeyvaluepair[1].ToString());
                }
            }
            catch (Exception ex)
            {
                strData = strContent;
            }
            return strData;
        }
        #endregion

        #region Check Status of the url -- checking file exist for the url
        public static bool CheckUrlStatus(string strUrl)
        {
            bool MyResult = false;
            try
            {
                //WebRequest req = WebRequest.Create(strUrl);
                //WebResponse res = req.GetResponse();

                HttpWebRequest request = WebRequest.Create(strUrl) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.Close();
                MyResult = (response.StatusCode == HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                MyResult = false;
            }
            return MyResult;
        }
        #endregion

        #region Calculate Time Diff b/w two dates (note: start date should be less than end date)
        public static string CalcTimeDiff(string strStartDate, string strEndDate)
        {
            string strDateDiff = string.Empty;
            try
            {
                DateTime StartDate = Convert.ToDateTime(strStartDate);
                DateTime EndDate = Convert.ToDateTime(strEndDate);
                TimeSpan DateDiff = EndDate - StartDate;
                strDateDiff = DateDiff.Hours + " hrs :" + DateDiff.Minutes + " mins";
            }
            catch (Exception ex)
            {
                string Error = ex.ToString();
                strDateDiff = "";
            }
            return strDateDiff;
        }
        #endregion

        public static string RandomOTPGeneration(ref string GenerateOTP)
        {
            GenerateOTP = string.Empty;
            try
            {
                string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
                string numbers = "1234567890";

                string characters = alphabets + small_alphabets + numbers;

                string otp = string.Empty;
                int length = 6;
                for (int i = 0; i < length; i++)
                {
                    string character = string.Empty;
                    do
                    {
                        int index = new Random().Next(0, characters.Length);
                        character = characters.ToCharArray()[index].ToString();
                    } while (otp.IndexOf(character) != -1);
                    otp += character;
                }
                GenerateOTP = otp;
            }
            catch (Exception ex)
            {


            }

            return GenerateOTP;

        }

        public static JObject callWebMethod(string JsonMethodName, Hashtable hsInput, ref string strErrorMsg)
        {
            var rqLogin = JsonConvert.SerializeObject(hsInput);
            string strRCUrl = ConfigurationManager.AppSettings["RCSUPPORTSERVICEURL"].ToString();
            WebClient wcLogin = new WebClient();
            wcLogin.Headers["Content-Type"] = "application/json";
            byte[] byteGetLogin = wcLogin.UploadData(strRCUrl + "/" + JsonMethodName, "POST", Encoding.ASCII.GetBytes(rqLogin));
            JObject jbResult = (JObject)JsonConvert.DeserializeObject(ASCIIEncoding.ASCII.GetString(byteGetLogin));
            return jbResult;
        }

        public static bool AsyncSendMail(string pdf, string PrinTicket1, string txtEmailID, bool chksingletkt, string Spnr, string ticket, int TktCount)
        {
            string strPortNo = ConfigurationManager.AppSettings["PortNo"].ToString();
            string strMailusername = ConfigurationManager.AppSettings["MailUsername"].ToString();
            string strNetworkusername = ConfigurationManager.AppSettings["NetworkUsername"].ToString();
            string strMailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
            string strHostAddress = ConfigurationManager.AppSettings["HostAddress"].ToString();
            string msg = string.Empty;
            string XMl = string.Empty;
            string strExtension = string.Empty;
            string Function = string.Empty;
            string RetVal = string.Empty;
            string strCCMailID = string.Empty;

            string strAgentID = (HttpContext.Current.Session["POS_ID"].ToString() != null && HttpContext.Current.Session["POS_ID"].ToString() != "") ? HttpContext.Current.Session["POS_ID"].ToString() : "";
            string strTerminalId = (HttpContext.Current.Session["POS_TID"].ToString() != null && HttpContext.Current.Session["POS_TID"].ToString() != "") ? HttpContext.Current.Session["POS_TID"].ToString() : "";
            string strUserName = (HttpContext.Current.Session["username"].ToString() != null && HttpContext.Current.Session["username"].ToString() != "") ? HttpContext.Current.Session["username"].ToString() : "";
            string Ipaddress = (HttpContext.Current.Session["ipAddress"].ToString() != null && HttpContext.Current.Session["ipAddress"].ToString() != "") ? HttpContext.Current.Session["ipAddress"].ToString() : "";
            string strSequenceid = (HttpContext.Current.Session["sequenceid"].ToString() != null && HttpContext.Current.Session["sequenceid"].ToString() != "") ? HttpContext.Current.Session["sequenceid"].ToString() : DateTime.Now.ToString("yyyyMMdd");
            string AgentAltermail = HttpContext.Current.Session["AgentAlterMail"] != null ? HttpContext.Current.Session["AgentAlterMail"].ToString() : "";
            string MailFrom = HttpContext.Current.Session["mailfromadd"] != null && HttpContext.Current.Session["mailfromadd"].ToString() != "" ? HttpContext.Current.Session["mailfromadd"].ToString() : "";

            Mailref.MyService pp = new Mailref.MyService();
            pp.Url = ConfigurationManager.AppSettings["mailurl"].ToString();
            try
            {
                if (pdf.ToUpper() == "YES")
                    strExtension = ".pdf";
                else
                    strExtension = ".html";

                bool ssl = ConfigurationManager.AppSettings["EnableSsl"].ToString() == "false" ? false : true;
                if (chksingletkt == true && TktCount > 1)
                    Function = "Single Ticket Copy";
                else
                    Function = "Combined Ticket Copy";

                strCCMailID = txtEmailID.Contains("MAILCCSPLIT") ? Regex.Split(txtEmailID, "MAILCCSPLIT")[1] : "";
                txtEmailID = txtEmailID.Contains("MAILCCSPLIT") ? Regex.Split(txtEmailID, "MAILCCSPLIT")[0] : txtEmailID;
                string filename = chksingletkt == true ? Spnr + "-" : Spnr;
                strCCMailID = AgentAltermail.TrimEnd(',') + "," + strCCMailID.TrimEnd(',');
                strCCMailID = strCCMailID.TrimEnd(',');
                RetVal = pp.SendMailSingleTicket(strAgentID, strTerminalId, strUserName, Ipaddress, "W",
                        strSequenceid, txtEmailID, strCCMailID, Spnr, ticket, filename, PrinTicket1, strMailusername,
                        strMailPassword, strHostAddress, strPortNo, ssl, MailFrom, strExtension);

                XMl = "<EVENT><FROMMAIL>" + txtEmailID + "</FROMMAIL><METHOD>" + Function + "</METHOD>"
                        + "<agentid>" + (HttpContext.Current.Session["agentid"] != null ? HttpContext.Current.Session["agentid"].ToString() : "") + "</agentid>"
                        + "<terminalid>" + (HttpContext.Current.Session["terminalid"] != null ? HttpContext.Current.Session["terminalid"].ToString() : "") + "</terminalid>"
                        + "<username>" + strUserName + "</username>" + "<ipAddress>" + Ipaddress + "</ipAddress>"
                        + "<sequenceid>" + strSequenceid + "</sequenceid>" + "<Spnr>" + Spnr + "</Spnr>" + "<ssl>" + ssl + "</ssl>"
                        + "<MailUsername>" + strMailusername + "</MailUsername>" + "<MailPassword>" + strMailPassword + "</MailPassword>"
                        + "<HostAddress>" + strHostAddress + "</HostAddress>" + "<MailPort>" + strPortNo + "</MailPort>"
                        + "<MailFrom>" + MailFrom + "</MailFrom>" + "<EXTension>" + strExtension + "</EXTension><RESPONSE>" + RetVal + "</RESPONSE></EVENT>";
                if (RetVal.ToUpper() == "MAIL SENT")
                {
                    msg = "Your mail has been sent successfully";
                    XMl = "<EVENT><RESPONSE><FUNCTION>pdforhtml_Click</FUNCTION><RESULT>" + Spnr + "<RESULT></RESPONSE></EVENT>";
                    DatabaseLog.LogData(strUserName, "E", "AfterBookingController.cs", "TicketMailResponse - " + Spnr, XMl.ToString(), strAgentID, strTerminalId, strSequenceid);
                    return true;
                }
                else
                {
                    msg = "<EMAIL>" + txtEmailID + "</EMAIL><ERROR>" + RetVal.ToString() + "</ERROR>";
                    XMl = "<EVENT><RESPONSE><FUNCTION>pdforhtml_Click</FUNCTION><RESULT>" + msg + "<RESULT></RESPONSE></EVENT>";
                    DatabaseLog.LogData(strUserName, "E", "AfterBookingController.cs", "TicketMailResponse - " + Spnr, XMl.ToString(), strAgentID, strTerminalId, strSequenceid);
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = "<EMAIL>" + txtEmailID + "</EMAIL><ERROR>" + ex.ToString() + "</ERROR>";
                DatabaseLog.LogData(strUserName, "E", "AfterBookingController.cs", "btnEmail_pdforhtml_Click", msg, strAgentID, strTerminalId, strSequenceid);
                return false;
            }
        }

        public static string LoadServerdatetime()
        {
            double GMT = Convert.ToDouble(HttpContext.Current.Session["GMTTIME"] != null ? HttpContext.Current.Session["GMTTIME"].ToString() : ConfigurationManager.AppSettings["GMTTIME"].ToString());
            DateTime dateTime = DateTime.Now.AddMinutes(GMT);
            return dateTime.ToString("dd/MM/yyyy");
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

        public static string DecryptKEY(string cryptTxt, string key)
        {
            try
            {
                cryptTxt = cryptTxt.Replace(" ", "+").Replace("%20", "+");
                byte[] bytesBuff = Convert.FromBase64String(cryptTxt);
                using (Aes aes = Aes.Create())
                {
                    Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    aes.Key = crypto.GetBytes(32);
                    aes.IV = crypto.GetBytes(16);
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cStream.Write(bytesBuff, 0, bytesBuff.Length);
                            cStream.Close();
                        }
                        cryptTxt = Encoding.Unicode.GetString(mStream.ToArray());
                    }
                }
            }
            catch (Exception)
            {
                cryptTxt = "0";
                return cryptTxt;
            }
            return cryptTxt;
        }

        public static string EncryptKEy(string inText, string key)
        {
            byte[] bytesBuff = Encoding.Unicode.GetBytes(inText);
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytesBuff, 0, bytesBuff.Length);
                        cStream.Close();
                    }
                    inText = Convert.ToBase64String(mStream.ToArray());
                }
            }
            return inText;
        }

        public static string cacheServerdatetime()
        {
            double GMT = Convert.ToDouble(ConfigurationManager.AppSettings["GMTTIME"].ToString());
            DateTime dateTime = DateTime.Now.AddMinutes(GMT);//.Now.Date;
            int myInt = int.Parse(ConfigurationManager.AppSettings["flgMemCachedays"].ToString());
            dateTime = dateTime.AddDays(myInt);
            return dateTime.ToString("dd/MM/yyyy");
        }

        public class ServiceUtility
        {
            public static string CultureInfoToConvertDatetimeFormatPnrVarify(DateTime dateTimeTemp, string format)
            {
                CultureInfo cii = new CultureInfo("en-GB", true);
                string requiredDate = dateTimeTemp.ToString(format, cii);
                TimeZoneInfo timeZoneInfo;
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime dateTime = TimeZoneInfo.ConvertTime(dateTimeTemp, timeZoneInfo);
                requiredDate = dateTime.ToString(format);
                return requiredDate;
            }

            public static string SplitPaxCount(string strPaxType, string strsplitpax)
            {
                string strpaxcount = string.Empty;
                string[] strPaxSplit = strsplitpax.Split('|');
                if (strPaxSplit.Length <= 3 && strPaxType.ToString().ToUpper().Trim() == "ADT")
                    strpaxcount = strPaxSplit[0].ToString();
                else if (strPaxSplit.Length <= 3 && strPaxType.ToString().ToUpper().Trim() == "CHD")
                    strpaxcount = strPaxSplit[1].ToString();
                else if (strPaxSplit.Length <= 3 && strPaxType.ToString().ToUpper().Trim() == "INF")
                    strpaxcount = strPaxSplit[2].ToString();

                return strpaxcount;
            }

            //public static void GridResponse(RQRS.AvailabilityRS _availResponse, ref DataTable dttemp)//--?
            //{
            //    try
            //    {
            //        ServiceUtility Serv = new ServiceUtility();
            //        if (_availResponse.ResultCode == "1")
            //        {

            //        }


            //        var MergeFlights = from FareTable in _availResponse.Fares.AsEnumerable()
            //                           from TaxTable in FareTable.Faredescription.Where(e => e.Paxtype.Equals("ADT")).AsEnumerable()
            //                           select new
            //                           {
            //                               BaseAmount = TaxTable.BaseAmount,
            //                               Commission = TaxTable.Discount,
            //                               Discount = TaxTable.Commission,
            //                               GrossAmount = TaxTable.GrossAmount,
            //                               Incentive = TaxTable.Incentive,
            //                               PaxType = TaxTable.Paxtype,
            //                               ServiceTax = TaxTable.ServiceTax,
            //                               Servicecharge = TaxTable.Servicecharge,
            //                               TDS = TaxTable.TDS,
            //                               TotalTaxAmount = TaxTable.TotalTaxAmount,
            //                               TransactionFee = TaxTable.TransactionFee,

            //                               FlightId = FareTable.FlightId,
            //                               GrossAmountRef = TaxTable.GrossAmount,
            //                               BreakUp = String.Join("/", TaxTable.Taxes.Select(TaxData => TaxData.Code.ToString() + ":" + TaxData.Amount).ToArray())
            //                           };


            //        //int indexValue = 0;

            //        var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
            //                          join FareData in MergeFlights.AsEnumerable()
            //                          on Flight.FareId equals FareData.FlightId
            //                          select new
            //                          {
            //                              Image = groupbyRwB(Flight.FlightNumber.Substring(0, 2)),//1
            //                              Flight = Flight.FlightNumber + "\n" + airlineName(Flight.FlightNumber.Substring(0, 2)),//,2
            //                              DEPARTURE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "DepartureDate"),//3
            //                              ARRIVAL = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "ArrivalDate"),//4
            //                              Duration = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "Duration"),//5
            //                              CLASS = Flight.Class + " - " + Flight.FareBasisCode,// + (Flight.Cabin.Contains(',') ? ("," + Flight.Cabin.Replace(Flight.Class + ",", "").ToString()) : ""),//6
            //                              Rules = "Fare Rule",//7
            //                              BasicFare = ConvertToDecimal(FareData.BaseAmount),//8
            //                              BasicFareComm = ConvertToDecimal(FareData.BaseAmount) + "\nComm : " + FareData.Commission,//9
            //                              TAXBREAKUP = FareData.BreakUp,//10
            //                              GrossAmount = calculateServiceGross(FareData.GrossAmount, (FareData.Servicecharge ?? "0")),//11
            //                              GrossAmountTrans = calculateTransGross(FareData.GrossAmount, FareData.TransactionFee ?? "0", FareData.Servicecharge ?? "0"),//12

            //                              Detail = Flight.ConnectionFlag == "S" ? "Detail" : "0",//13
            //                              AIRCATEGORY = Flight.AirlineCategory,//14
            //                              SERVICE_CHARGE = FareData.Servicecharge,//15
            //                              DISCOUNT = FareData.Discount,//16
            //                              AIRCRAFT = Flight.SegmentDetails,//17

            //                              TRANSACTIONFEE = FareData.TransactionFee ?? "0",//18
            //                              CABIN = Flight.Cabin,//19
            //                              OFFLINEFLAG = Flight.OfflineFlag,//20
            //                              OFFLINE = Flight.OfflineIndicator,//21
            //                              Commission = FareData.Commission,//22

            //                              XML = (Flight.ConnectionFlag == "S" ? formxml(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), FareData.BaseAmount,
            //                                        FareData.Commission, FareData.Servicecharge, FareData.GrossAmount, FareData.TransactionFee, FareData.Discount, FareData.BreakUp) : ""),//23
            //                              Flightid = FareData.FlightId,//24

            //                              Origin = Flight.Origin,
            //                              Destination = Flight.Destination,
            //                              Token = Flight.ReferenceToken,
            //                              PlatingCarrier = Flight.PlatingCarrier,
            //                              flyingTime = Flight.FlyingTime,
            //                              TIMERANGE_ARRIVAL = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "ArrivalTime"),//
            //                              TIMERANGE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "DepartureTime"),//

            //                              CNX = Flight.CNX,
            //                              Status = "O",//25

            //                              ConnectingFlag = Flight.ConnectionFlag,
            //                              JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//
            //                              CLASSRef = Flight.Class + " - " + Flight.FareBasisCode,
            //                              AllowMulticlass = Flight.MultiClass,
            //                              MulticlassFare = "",
            //                              MulticlassTransFare = "",
            //                              MulticlassBaseFare = "",
            //                              MulticlassBaseCommFare = "",
            //                              MulticlassToken = "",
            //                              Multiclassdetail = "",
            //                              MulticlassTaxBreak = "",
            //                              Multiclass = "",
            //                              SegRef = Flight.SegRef,
            //                              MULTISTOPS = _availResponse.Flights.IndexOf(Flight),//26
            //                          };

            //        var qryflights = from q in FareFlights.AsEnumerable()
            //                         where q.Duration != ""
            //                         select q;

            //        DataTable ldtFare = Serv.ConvertToDataTable(qryflights);

            //        ldtFare.DefaultView.Sort = "GrossAmount ASC,Flightid ASC, MULTISTOPS ASC";
            //        ldtFare = ldtFare.DefaultView.ToTable();

            //        dttemp.BeginLoadData();
            //        dttemp.Merge(ldtFare.Copy());
            //        dttemp.AcceptChanges();
            //        dttemp.EndLoadData();

            //    }
            //    catch (Exception)
            //    {
            //        return;
            //    }

            //}

            private static string sector(string sector, ref string prev, ref int lindex)
            {
                int index = lindex;
                if (sector.ToUpper().Trim() != prev.ToUpper().Trim())
                {
                    index = index + 1;
                    prev = sector;
                    lindex = index;
                }
                return sector;
            }

            public static void GridResponseforSelect(RQRS.PriceItenaryRS _PriceResponse, ref DataTable dttemp, ref string strErrtemp,
                ref DataTable dtbagg, ref DataTable dtMeal, ref DataTable odt, ref DataTable dtbookreq, ref DataTable dtbagout)
            {
                try
                {
                    ServiceUtility Serv = new ServiceUtility();

                    #region Multi Response

                    if (_PriceResponse.PriceItenarys.Count > 1)
                    {

                        int MealsCount = 0;
                        int BaggageCount = 0;
                        int otherservi = 0;

                        for (int myind = 0; myind < _PriceResponse.PriceItenarys.Count; myind++)
                        {


                            List<RQRS.PriceItenary> lstrPriceItenary = _PriceResponse.PriceItenarys;
                            List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[myind].PriceRS;

                            RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];
                            string Myitinref = myind.ToString();
                            var MergeFlights = from FareData in _availResponse.Fares.AsEnumerable()
                                               from TaxTable in FareData.Faredescription.AsEnumerable()
                                               orderby TaxTable.Paxtype ascending
                                               select new
                                               {
                                                   BaseAmount = (ConvertToDecimal(TaxTable.BaseAmount ?? "0") + ConvertToDecimal(TaxTable.AddMarkup ?? "0")).ToString(),
                                                   Commission = ((ConvertToDecimal(TaxTable.Discount ?? "0") +
                                                   ConvertToDecimal(TaxTable.Incentive ?? "0") + ConvertToDecimal(TaxTable.PLBAmount ?? "0")) - ConvertToDecimal(TaxTable.ServiceTax ?? "0")).ToString(),// TaxTable.Discount,
                                                   Discount = TaxTable.Commission ?? "0",
                                                   GrossAmount = (ConvertToDecimal(TaxTable.AddMarkup ?? "0") + ConvertToDecimal(TaxTable.GrossAmount ?? "0")).ToString(),
                                                   originaloldgross = (string.IsNullOrEmpty(TaxTable.OldFare) ? 0 : ConvertToDecimal(TaxTable.OldFare)).ToString(),
                                                   Incentive = "0",
                                                   PaxType = TaxTable.Paxtype,
                                                   ServiceTax = TaxTable.ServiceTax,
                                                   Servicecharge = TaxTable.Servicecharge,
                                                   TDS = TaxTable.TDS,
                                                   TotalTaxAmount = TaxTable.TotalTaxAmount,
                                                   TransactionFee = TaxTable.TransactionFee,

                                                   FlightId = FareData.FlightId,
                                                   GrossAmountRef = (ConvertToDecimal(TaxTable.AddMarkup ?? "0") + ConvertToDecimal(TaxTable.GrossAmount ?? "0")).ToString(),
                                                   BreakUp = String.Join("/", TaxTable.Taxes.Select(TaxData => TaxData.Code.ToString() + ":"
                                                   + (TaxData.Code.ToString().Trim().ToUpper() == "BF" ?
                                                   (ConvertToDecimal(TaxTable.AddMarkup ?? "0") + ConvertToDecimal(TaxData.Amount ?? "0")).ToString() :
                                                   TaxData.Amount)).ToArray()),
                                                   markup = ((string.IsNullOrEmpty(TaxTable.ClientMarkup) ? 0 : ConvertToDecimal(TaxTable.ClientMarkup))).ToString(),

                                                   ExtraMrkUp = (string.IsNullOrEmpty(TaxTable.AddMarkup) ? 0 : ConvertToDecimal(TaxTable.AddMarkup)).ToString(),
                                                   originaloldmarkup = (string.IsNullOrEmpty(TaxTable.OldMarkup) ? 0 : ConvertToDecimal(TaxTable.OldMarkup)).ToString(),
                                                   Sftax = TaxTable.SFAMOUNT ?? "0",
                                                   SFGST = TaxTable.SFGST ?? "0",
                                                   Servicefee = TaxTable.ServiceFee ?? "0",
                                                   FareType = FareData.FareType
                                               };


                            var GroupFares = from FareData in MergeFlights.AsEnumerable()
                                             group FareData by
                                             new
                                             {
                                                 Id = FareData.FlightId,
                                             }
                                                 into FareGrp
                                             select new
                                             {
                                                 BaseAmount = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.BaseAmount) ? "0" : Fares.BaseAmount.ToString()).ToArray(), "JOIN-|"),
                                                 Commission = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.Commission) ? "0" : Fares.Commission).ToArray(), "JOIN-|"),
                                                 Discount = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.Discount) ? "0" : Fares.Discount).ToArray(), "JOIN-|"),
                                                 GrossAmount = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.GrossAmount) ? "0" : Fares.GrossAmount.ToString()).ToArray(), "JOIN-|"),
                                                 originaloldgross = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.originaloldgross) ? "0" : Fares.originaloldgross.ToString()).ToArray(), "JOIN-|"),
                                                 Incentive = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.Incentive) ? "0" : Fares.Incentive).ToArray(), "JOIN-|"),
                                                 PaxType = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.PaxType.ToString()).ToArray(), "JOIN-|"),
                                                 ServiceTax = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.ServiceTax) ? "0" : Fares.ServiceTax).ToArray(), "JOIN-|"),
                                                 Servicecharge = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.Servicecharge) ? "0" : Fares.Servicecharge).ToArray(), "JOIN-|"),
                                                 TDS = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.TDS) ? "" : Fares.TDS).ToArray(), "JOIN-|"),
                                                 TotalTaxAmount = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.TotalTaxAmount) ? "0" : Fares.TotalTaxAmount.ToString()).ToArray(), "JOIN-|"),
                                                 TransactionFee = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.TransactionFee) ? "0" : Fares.TransactionFee).ToArray(), "JOIN-|"),
                                                 FlightId = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.FlightId) ? "0" : Fares.FlightId.ToString()).ToArray(), "ANYONE"),
                                                 GrossAmountRef = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.GrossAmount) ? "0" : Fares.GrossAmount.ToString()).ToArray(), "ANYONE"),
                                                 BreakUp = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.BreakUp) ? "0" : Fares.BreakUp.ToString()).ToArray(), "JOIN-|"),
                                                 Markup = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.markup) ? "0" : Fares.markup).ToArray(), "JOIN-|"),

                                                 ExtraMrkUp = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.ExtraMrkUp.ToString()).ToArray(), "JOIN-|"),
                                                 originaloldmarkup = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.originaloldmarkup) ? "0" : Fares.originaloldmarkup).ToArray(), "JOIN-|"),
                                                 Sftax = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.Sftax) ? "0" : Fares.Sftax).ToArray(), "JOIN-|"),
                                                 SFGST = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.SFGST) ? "0" : Fares.SFGST).ToArray(), "JOIN-|"),
                                                 Servicefee = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.Servicefee) ? "0" : Fares.Servicefee).ToArray(), "JOIN-|"),
                                                 FareType = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.FareType) ? "0" : Fares.FareType).ToArray(), "JOIN-|"),
                                             };

                            #region Meals and Baggage

                            int indexValue = 0;

                            _availResponse.Meal = _availResponse.Meal == null ? new List<RQRS.Meals>() : _availResponse.Meal;
                            var Mealdata = from meal in _availResponse.Meal.AsEnumerable()
                                           group meal by
                                           new
                                           {
                                               meal.SegRef,
                                           }
                                               into mealfare
                                           select new
                                           {
                                               MealCode = GroupByProcessOneCol((mealfare.Select(Meals => Meals.Code.ToString().Replace("|", "@"))).ToArray(), "JOIN-|"),
                                               MealDesc = GroupByProcessOneCol(mealfare.Select(Meals => Meals.Description.ToString()).ToArray(), "JOIN-|"),
                                               MealAmt = GroupByProcessOneCol(mealfare.Select(Meals => Meals.Amount.ToString()).ToArray(), "JOIN-|"),
                                               SegRef = ((GroupByProcessOneCol(mealfare.Select(Meals => Meals.SegRef.ToString()).ToArray(), "ANYONE").Trim())).ToString(),
                                               Seg = GroupByProcessOneCol(mealfare.Select(Meals => Meals.SegRef.ToString()).ToArray(), "JOIN-|").Trim().ToString(),
                                               itinref = Myitinref,
                                               Myitinref = GroupByProcessOneCol(mealfare.Select(Meals => (string.IsNullOrEmpty(Meals.Itinref) ? "" : Meals.Itinref.ToString())).ToArray(), "JOIN-|").Trim().ToString(),
                                           };

                            _availResponse.Bagg = _availResponse.Bagg == null ? new List<RQRS.Bagg>() : _availResponse.Bagg;

                            var Baggdata = from Baggage in _availResponse.Bagg.AsEnumerable()
                                           group Baggage by
                                           new
                                           {
                                               Baggage.SegRef,
                                           }
                                               into Baggfare
                                           select new
                                           {
                                               BaggCode = GroupByProcessOneCol((Baggfare.Select(Meals => Meals.Code.ToString().Replace("|", "@"))).ToArray(), "JOIN-|"),
                                               BaggDesc = GroupByProcessOneCol(Baggfare.Select(Meals => Meals.Description.ToString()).ToArray(), "JOIN-|"),
                                               BaggAmt = GroupByProcessOneCol(Baggfare.Select(Meals => Meals.Amount.ToString()).ToArray(), "JOIN-|"),
                                               SegRef = ((GroupByProcessOneCol(Baggfare.Select(Meals => Meals.SegRef.ToString()).ToArray(), "ANYONE").Trim())).ToString(),
                                               Seg = GroupByProcessOneCol(Baggfare.Select(Meals => Meals.SegRef.ToString()).ToArray(), "JOIN-|").Trim().ToString(),
                                               Itinref = Myitinref,
                                               Myitinref = GroupByProcessOneCol(Baggfare.Select(Meals => (string.IsNullOrEmpty(Meals.Itinref) ? "" : Meals.Itinref.ToString())).ToArray(), "JOIN-|").Trim().ToString(),
                                           };

                            string lstrmyref = "";
                            int indexplus = 0;
                            var result = _availResponse.Bagg.AsEnumerable().Select(org => org.Orgin + org.Destination).Distinct().ToArray().Select((val, index) => new { val, index });
                            var Baggdatatemp = (from Baggage in _availResponse.Bagg.AsEnumerable()
                                                select new
                                                {
                                                    Segment = Baggage.Orgin + "->" + Baggage.Destination,
                                                    Segment_Code = sector(Baggage.Orgin + "->" + Baggage.Destination, ref lstrmyref, ref indexplus),
                                                    BaggageCode = Baggage.Code.Replace("|", "@"),
                                                    BaggageDesc = Baggage.Description + (" (" + Baggage.Amount + " )"),
                                                    BaggageAmt = Baggage.Amount,
                                                    SegRef = Baggage.SegRef,
                                                    Myitinref = (string.IsNullOrEmpty(Baggage.Itinref) ? "" : Baggage.Itinref),
                                                    Itinref = Myitinref,
                                                    MyRef = result.Where(id => id.val.Equals(Baggage.Orgin + Baggage.Destination)).FirstOrDefault().index + BaggageCount,
                                                    Addsegorg = Baggage.SegRef + "BAggSPLITbaGG" + Baggage.Orgin + "BAggSPLITbaGG" + Baggage.Destination + "BAggSPLITbaGG" + Baggage.Description + "BAggSPLITbaGG" + (string.IsNullOrEmpty(Baggage.Itinref) ? "" : Baggage.Itinref),
                                                    BaggageText = Baggage.BaggageText
                                                }).Distinct();

                            if (Baggdatatemp.Count() > 0)
                            {
                                if (dtbagg != null && dtbagg.Columns.Count > 0)
                                    dtbagg.Merge(Serv.ConvertToDataTable(Baggdatatemp));
                                else
                                    dtbagg = Serv.ConvertToDataTable(Baggdatatemp);
                                dtbagg.TableName = "Baggage";
                                BaggageCount = dtbagg.Rows.Count;
                            }


                            //DataTable dtMeal = new DataTable();

                            lstrmyref = "";
                            indexplus = 0;
                            //autoid = 0;
                            result = _availResponse.Meal.AsEnumerable().Select(org => org.Orgin + org.Destination).Distinct().ToArray().Select((val, index) => new { val, index });
                            var Mealdatatemp = (from Baggage in _availResponse.Meal.AsEnumerable()
                                                select new
                                                {
                                                    Segment = Baggage.Orgin + "->" + Baggage.Destination,
                                                    Segment_Code = sector(Baggage.Orgin + "->" + Baggage.Destination, ref lstrmyref, ref indexplus),
                                                    MealCode = Baggage.Code.Replace("|", "@"),
                                                    MealDesc = Baggage.Description + (" (" + Baggage.Amount + " )"),
                                                    MealAmt = Baggage.Amount,
                                                    SegRef = Baggage.SegRef,
                                                    Itinref = Myitinref,
                                                    Myitinref = (string.IsNullOrEmpty(Baggage.Itinref) ? "" : Baggage.Itinref),
                                                    MyRef = result.Where(id => id.val.Equals(Baggage.Orgin + Baggage.Destination)).FirstOrDefault().index + MealsCount,
                                                    Addsegorg = Baggage.SegRef + "MEALSRSPLITbaGG" + Baggage.Orgin + "MEALSRSPLITbaGG" + Baggage.Destination + "MEALSRSPLITbaGG" + Baggage.Description + "MEALSRSPLITbaGG" + (string.IsNullOrEmpty(Baggage.Itinref) ? "" : Baggage.Itinref),
                                                    ServiceMeal = Baggage.IsBundleServiceMeal
                                                }).Distinct();

                            if (Mealdatatemp.Count() > 0)
                            {
                                if (dtMeal != null && dtMeal.Columns.Count > 0)
                                    dtMeal.Merge(Serv.ConvertToDataTable(Mealdatatemp));
                                else
                                    dtMeal = Serv.ConvertToDataTable(Mealdatatemp);
                                dtMeal.TableName = "Meals";

                                MealsCount = dtMeal.Rows.Count;
                                // dsSSR.Tables.Add(dtMeal);
                            }

                            //DataTable odt = new DataTable();
                            lstrmyref = "";
                            indexplus = 0;
                            //autoid = 0;

                            _availResponse.OtherService = _availResponse.OtherService == null ? new List<RQRS.Other>() : _availResponse.OtherService;
                            result = _availResponse.OtherService.AsEnumerable().Select(org => org.Orgin + org.Destination).Distinct().ToArray().Select((val, index) => new { val, index });
                            var Otherssrtemp = (from Othrssr in _availResponse.OtherService.AsEnumerable()
                                                    //  where (Othrssr.SSRType == "SPICEMAX" || Othrssr.SSRType == "BAGOUT" || Othrssr.SSRType == "PRIORITY_CHECK_IN")
                                                where ((!string.IsNullOrEmpty(Othrssr.SSRType)) && Othrssr.SSRType.ToUpper() != "BAGOUT")
                                                select new
                                                {
                                                    Segment = Othrssr.Orgin + "->" + Othrssr.Destination,
                                                    Segment_Code = sector(Othrssr.Orgin + "->" + Othrssr.Destination, ref lstrmyref, ref indexplus),
                                                    //OthrSSR_Code = Othrssr.SSRCode,
                                                    OthrSSR_Code = Othrssr.SSRCode,
                                                    OthrSSR_Desc = Othrssr.Description + (" (" + Othrssr.Amount + " )"),
                                                    OthrSSR_Details = Othrssr.Description,
                                                    OtherSSR_orgin = Othrssr.Orgin,
                                                    OtherSSR_destina = Othrssr.Destination,
                                                    OthrSSR_Amt = Othrssr.Amount,
                                                    SegRef = Othrssr.SegRef,
                                                    OtherSSR_Type = Othrssr.SSRType,
                                                    Itinref = Myitinref,
                                                    Myitinref = (string.IsNullOrEmpty(Othrssr.Itinref) ? "" : Othrssr.Itinref),
                                                    MyRef = result.Where(id => id.val.Equals(Othrssr.Orgin + Othrssr.Destination)).FirstOrDefault().index + otherservi,
                                                }).Distinct();

                            if (Otherssrtemp.Count() > 0)
                            {
                                if (odt != null && odt.Columns.Count > 0)
                                    odt.Merge(Serv.ConvertToDataTable(Otherssrtemp));
                                else
                                    odt = Serv.ConvertToDataTable(Otherssrtemp);
                                odt.TableName = "OtherSSR";

                                otherservi = odt.Rows.Count;
                                // dsSSR.Tables.Add(dtMeal);
                            }

                            var Otherssrbagouttemp = (from Othrssr in _availResponse.OtherService.AsEnumerable()//177
                                                      where ((!string.IsNullOrEmpty(Othrssr.SSRType)) && Othrssr.SSRType.ToUpper() == "BAGOUT")
                                                      select new
                                                      {
                                                          Segment = Othrssr.Orgin + "->" + Othrssr.Destination,
                                                          Segment_Code = sector(Othrssr.Orgin + "->" + Othrssr.Destination, ref lstrmyref, ref indexplus),
                                                          OthrSSR_Code = Othrssr.SSRCode,
                                                          OthrSSR_Desc = Othrssr.Description + (" (" + Othrssr.Amount + " )"),
                                                          OthrSSR_Details = Othrssr.Description,
                                                          OtherSSR_orgin = Othrssr.Orgin,
                                                          OtherSSR_destina = Othrssr.Destination,
                                                          OthrSSR_Amt = Othrssr.Amount,
                                                          SegRef = Othrssr.SegRef,
                                                          OtherSSR_Type = Othrssr.SSRType,
                                                          Itinref = Myitinref,
                                                          Myitinref = (string.IsNullOrEmpty(Othrssr.Itinref) ? "" : Othrssr.Itinref),
                                                          MyRef = result.Where(id => id.val.Equals(Othrssr.Orgin + Othrssr.Destination)).FirstOrDefault().index,
                                                      }).Distinct();

                            if (Otherssrbagouttemp.Count() > 0)
                            {
                                dtbagout = Serv.ConvertToDataTable(Otherssrbagouttemp);
                                dtbagout.TableName = "OtherSSRBAGOUT";
                            }

                            #endregion

                            DataTable ldtFare = new DataTable();
                            if ((_availResponse.Meal.Count == 0 || Mealdata == null) &&
                             (_availResponse.Bagg.Count == 0 || Baggdata == null))
                            {
                                #region no meals and no baggage
                                var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                  join FareData in GroupFares.AsEnumerable()
                                                  on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                  select new
                                                  {
                                                      AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                      DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                      ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                      CNXType = Flight.CNX.ToString().Trim(),
                                                      Class = Flight.Class,
                                                      refund = Flight.Refundable,
                                                      Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                      Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                      FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                      FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                      CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                      FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                      JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                      Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                      PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                      Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                      SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                      MULTICLASSAMOUNT = "",
                                                      BaseAmount = FareData.BaseAmount,
                                                      TAXBREAKUP = FareData.BreakUp,
                                                      Commission = FareData.Commission,
                                                      Discount = FareData.Discount,
                                                      GrossAmount = FareData.GrossAmount,
                                                      originaloldgross = FareData.originaloldgross,
                                                      GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                      Incentive = FareData.Incentive,
                                                      PaxType = FareData.PaxType,
                                                      ServiceTax = FareData.ServiceTax,
                                                      Servicecharge = FareData.Servicecharge,
                                                      TDS = FareData.TDS,
                                                      TotalTaxAmount = FareData.TotalTaxAmount,
                                                      TransactionFee = FareData.TransactionFee,

                                                      MarkUp = FareData.Markup,
                                                      //OLDmarkup=FareData.OLDmarkup,
                                                      ExtraMrkUp = FareData.ExtraMrkUp,
                                                      originaloldmarkup = FareData.originaloldmarkup,

                                                      FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                      JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),

                                                      CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                      OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                      MEAL = "",
                                                      Baggage = "",

                                                      FQT = Flight.AllowFQT,
                                                      Faresid = (indexValue++).ToString(),
                                                      FlightId = FareData.FlightId.ToString(),
                                                      AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                      ORGTERMINAL = Flight.StartTerminal,
                                                      DESTERMINAL = Flight.EndTerminal,
                                                      OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                      NumberOfStops = "",
                                                      Via = Flight.Via,
                                                      RefNo = "",
                                                      ToSell = "",
                                                      CNX = "",
                                                      ARRIVALTIME = "",
                                                      DEPARTURETIME = "",
                                                      AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                      FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                      DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                      ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                      STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                      ViaFlights = "",
                                                      Itinref = Myitinref,
                                                      Myitinref = Flight.ItinRef.ToString().Trim(),
                                                      mealAvail = false,// _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                      BaggAvail = false,//_availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                      DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                      PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                      stocktype = _availResponse.FareCheck.Stocktype,
                                                      seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                      airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                          (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                      OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                      TokenBookingKKK = lstrPriceItenary[myind].Token,
                                                      FareTypeDescription = Flight.FareTypeDescription,
                                                      Sftax = FareData.Sftax ?? "0",
                                                      SFGST = FareData.SFGST ?? "0",
                                                      Servicefee = FareData.Servicefee ?? "0",
                                                      FareType = FareData.FareType.Split('|')[0],
                                                      Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                      Promocodedesc = Flight.PromoCodeDesc,
                                                      Promocode = Flight.PromoCode
                                                  };

                                ldtFare = Serv.ConvertToDataTable(FareFlights);
                                #endregion
                            }
                            else if ((_availResponse.Meal.Count == 0 || Mealdata == null) &&
                                     (Baggdata != null && _availResponse.Bagg.Count > 0))
                            {
                                #region only baggage


                                if (Baggdata.AsEnumerable().First().SegRef == "0")
                                {
                                    #region Common Baggage
                                    var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                      join FareData in GroupFares.AsEnumerable()
                                                      on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                      join SsrBaggData in Baggdata.AsEnumerable()
                                                     on 0 equals 0 into SSRBagg
                                                      from Baggage in SSRBagg.DefaultIfEmpty()
                                                      select new
                                                      {
                                                          AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                          DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                          ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                          CNXType = Flight.CNX.ToString().Trim(),
                                                          Class = Flight.Class,
                                                          refund = Flight.Refundable,
                                                          Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                          Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                          FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                          FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                          CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                          FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                          JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                          Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                          PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                          Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                          SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                          MULTICLASSAMOUNT = "",
                                                          BaseAmount = FareData.BaseAmount,
                                                          TAXBREAKUP = FareData.BreakUp,
                                                          Commission = FareData.Commission,
                                                          Discount = FareData.Discount,
                                                          GrossAmount = FareData.GrossAmount,
                                                          originaloldgross = FareData.originaloldgross,
                                                          GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                          Incentive = FareData.Incentive,
                                                          PaxType = FareData.PaxType,
                                                          ServiceTax = FareData.ServiceTax,
                                                          Servicecharge = FareData.Servicecharge,
                                                          TDS = FareData.TDS,
                                                          TotalTaxAmount = FareData.TotalTaxAmount,
                                                          TransactionFee = FareData.TransactionFee,
                                                          MarkUp = FareData.Markup,
                                                          //OLDmarkup=FareData.OLDmarkup,
                                                          ExtraMrkUp = FareData.ExtraMrkUp,
                                                          originaloldmarkup = FareData.originaloldmarkup,
                                                          FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                          JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),

                                                          CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                          OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                          MEAL = "",
                                                          Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                          Baggage.BaggAmt + "~" + Baggage.Seg),

                                                          FQT = Flight.AllowFQT,
                                                          Faresid = (indexValue++).ToString(),
                                                          FlightId = FareData.FlightId.ToString(),
                                                          // AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                          AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                          ORGTERMINAL = Flight.StartTerminal,
                                                          DESTERMINAL = Flight.EndTerminal,
                                                          OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                          NumberOfStops = "",
                                                          Via = Flight.Via,
                                                          RefNo = "",
                                                          ToSell = "",
                                                          CNX = "",
                                                          ARRIVALTIME = "",
                                                          DEPARTURETIME = "",
                                                          AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                          FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                          DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                          ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                          STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                          ViaFlights = "",
                                                          Itinref = Myitinref,
                                                          Myitinref = Flight.ItinRef.ToString().Trim(),
                                                          mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                          BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,

                                                          DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                          PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                          stocktype = _availResponse.FareCheck.Stocktype,
                                                          seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                          airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                          (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                          OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                          TokenBookingKKK = lstrPriceItenary[myind].Token,
                                                          FareTypeDescription = Flight.FareTypeDescription,
                                                          Sftax = FareData.Sftax ?? "0",
                                                          SFGST = FareData.SFGST ?? "0",
                                                          Servicefee = FareData.Servicefee ?? "0",
                                                          FareType = FareData.FareType.Split('|')[0],
                                                          Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                          Promocodedesc = Flight.PromoCodeDesc,
                                                          Promocode = Flight.PromoCode
                                                      };

                                    ldtFare = Serv.ConvertToDataTable(FareFlights);
                                    #endregion

                                }
                                else
                                {
                                    #region Individual Baggage

                                    var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                      join FareData in GroupFares.AsEnumerable()
                                                      on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                      join SsrBaggData in Baggdata.AsEnumerable()
                                                     on Flight.SegRef equals SsrBaggData.SegRef into SSRBagg
                                                      from Baggage in SSRBagg.DefaultIfEmpty()
                                                      select new
                                                      {
                                                          AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                          DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                          ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                          CNXType = Flight.CNX.ToString().Trim(),
                                                          Class = Flight.Class,
                                                          refund = Flight.Refundable,
                                                          Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                          Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                          FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                          FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                          CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                          FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                          JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                          Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                          PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                          Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                          SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                          MULTICLASSAMOUNT = "",
                                                          BaseAmount = FareData.BaseAmount,
                                                          TAXBREAKUP = FareData.BreakUp,
                                                          Commission = FareData.Commission,
                                                          Discount = FareData.Discount,
                                                          GrossAmount = FareData.GrossAmount,
                                                          originaloldgross = FareData.originaloldgross,
                                                          GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                          Incentive = FareData.Incentive,
                                                          PaxType = FareData.PaxType,
                                                          ServiceTax = FareData.ServiceTax,
                                                          Servicecharge = FareData.Servicecharge,
                                                          TDS = FareData.TDS,
                                                          TotalTaxAmount = FareData.TotalTaxAmount,
                                                          TransactionFee = FareData.TransactionFee,

                                                          FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                          JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                          CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                          OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                          MEAL = "",
                                                          Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                          Baggage.BaggAmt + "~" + Baggage.Seg),

                                                          FQT = Flight.AllowFQT,
                                                          Faresid = (indexValue++).ToString(),
                                                          FlightId = FareData.FlightId.ToString(),
                                                          //AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                          AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                          ORGTERMINAL = Flight.StartTerminal,
                                                          DESTERMINAL = Flight.EndTerminal,
                                                          OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                          NumberOfStops = "",
                                                          Via = Flight.Via,
                                                          RefNo = "",
                                                          ToSell = "",
                                                          CNX = "",
                                                          ARRIVALTIME = "",
                                                          DEPARTURETIME = "",
                                                          AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                          FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                          DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                          ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                          STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                          ViaFlights = "",
                                                          Itinref = Myitinref,
                                                          Myitinref = Flight.ItinRef.ToString().Trim(),
                                                          mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                          BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                          DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                          PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                          MarkUp = FareData.Markup,
                                                          //OLDmarkup=FareData.OLDmarkup,
                                                          ExtraMrkUp = FareData.ExtraMrkUp,
                                                          originaloldmarkup = FareData.originaloldmarkup,
                                                          stocktype = _availResponse.FareCheck.Stocktype,
                                                          seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                          airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                          (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                          OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                          TokenBookingKKK = lstrPriceItenary[myind].Token,
                                                          FareTypeDescription = Flight.FareTypeDescription,
                                                          Sftax = FareData.Sftax ?? "0",
                                                          SFGST = FareData.SFGST ?? "0",
                                                          Servicefee = FareData.Servicefee ?? "0",
                                                          FareType = FareData.FareType.Split('|')[0],
                                                          Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                          Promocodedesc = Flight.PromoCodeDesc,
                                                          Promocode = Flight.PromoCode
                                                      };
                                    ldtFare = Serv.ConvertToDataTable(FareFlights);

                                    #endregion

                                }

                                #endregion

                            }
                            else if ((_availResponse.Bagg.Count == 0 || Baggdata == null) &&
                                     (Mealdata != null && _availResponse.Meal.Count > 0))
                            {
                                #region only Meals
                                if (Mealdata.AsEnumerable().First().SegRef == "0")
                                {
                                    #region common meals for all segment

                                    var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                      join FareData in GroupFares.AsEnumerable()
                                                      on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                      join SsrMealData in Mealdata.AsEnumerable()
                                                      on 0 equals 0 into SSRMeal
                                                      // join SsrBaggData in Baggdata.AsEnumerable()
                                                      //on Flight.SegRef equals SsrBaggData.SegRef into SSRBagg
                                                      from Meal in SSRMeal.DefaultIfEmpty()
                                                          //from Baggage in SSRBagg.DefaultIfEmpty()
                                                      select new
                                                      {
                                                          AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                          DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                          ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                          CNXType = Flight.CNX.ToString().Trim(),
                                                          Class = Flight.Class,
                                                          refund = Flight.Refundable,
                                                          Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                          Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                          FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                          FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                          CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                          FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                          JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                          Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                          PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                          Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                          SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                          MULTICLASSAMOUNT = "",
                                                          BaseAmount = FareData.BaseAmount,
                                                          TAXBREAKUP = FareData.BreakUp,
                                                          Commission = FareData.Commission,
                                                          Discount = FareData.Discount,
                                                          GrossAmount = FareData.GrossAmount,
                                                          originaloldgross = FareData.originaloldgross,
                                                          GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                          Incentive = FareData.Incentive,
                                                          PaxType = FareData.PaxType,
                                                          ServiceTax = FareData.ServiceTax,
                                                          Servicecharge = FareData.Servicecharge,
                                                          TDS = FareData.TDS,
                                                          TotalTaxAmount = FareData.TotalTaxAmount,
                                                          TransactionFee = FareData.TransactionFee,
                                                          MarkUp = FareData.Markup,
                                                          //OLDmarkup=FareData.OLDmarkup,
                                                          ExtraMrkUp = FareData.ExtraMrkUp,
                                                          originaloldmarkup = FareData.originaloldmarkup,
                                                          FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                          JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),

                                                          CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                          OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                          MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                          Meal.MealAmt + "~" + Meal.Seg),
                                                          //Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                          //Baggage.BaggAmt + "~" + Baggage.Seg),
                                                          Baggage = "",

                                                          FQT = Flight.AllowFQT,
                                                          Faresid = (indexValue++).ToString(),
                                                          FlightId = FareData.FlightId.ToString(),
                                                          //AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                          AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                          ORGTERMINAL = Flight.StartTerminal,
                                                          DESTERMINAL = Flight.EndTerminal,
                                                          OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                          NumberOfStops = "",
                                                          Via = Flight.Via,
                                                          RefNo = "",
                                                          ToSell = "",
                                                          CNX = "",
                                                          ARRIVALTIME = "",
                                                          DEPARTURETIME = "",
                                                          AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                          FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                          DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                          ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                          STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                          ViaFlights = "",
                                                          Itinref = Myitinref,
                                                          Myitinref = Flight.ItinRef.ToString().Trim(),
                                                          mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                          BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                          DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                          PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                          stocktype = _availResponse.FareCheck.Stocktype,
                                                          seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                          airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                          (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                          OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                          TokenBookingKKK = lstrPriceItenary[myind].Token,
                                                          FareTypeDescription = Flight.FareTypeDescription,
                                                          Sftax = FareData.Sftax ?? "0",
                                                          SFGST = FareData.SFGST ?? "0",
                                                          Servicefee = FareData.Servicefee ?? "0",
                                                          FareType = FareData.FareType.Split('|')[0],
                                                          Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                          Promocodedesc = Flight.PromoCodeDesc,
                                                          Promocode = Flight.PromoCode
                                                      };

                                    ldtFare = Serv.ConvertToDataTable(FareFlights);
                                    #endregion
                                }
                                else
                                {

                                    #region Individual meal for every segment

                                    var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                      join FareData in GroupFares.AsEnumerable()
                                                      on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                      join SsrMealData in Mealdata.AsEnumerable()
                                                      on Flight.SegRef equals SsrMealData.SegRef into SSRMeal
                                                      from Meal in SSRMeal.DefaultIfEmpty()
                                                      select new
                                                      {
                                                          AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                          DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                          ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                          CNXType = Flight.CNX.ToString().Trim(),
                                                          Class = Flight.Class,
                                                          refund = Flight.Refundable,
                                                          Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                          Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                          FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                          FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                          CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                          FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                          JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                          Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                          PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                          Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                          SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                          MULTICLASSAMOUNT = "",
                                                          BaseAmount = FareData.BaseAmount,
                                                          TAXBREAKUP = FareData.BreakUp,
                                                          Commission = FareData.Commission,
                                                          Discount = FareData.Discount,
                                                          GrossAmount = FareData.GrossAmount,
                                                          originaloldgross = FareData.originaloldgross,
                                                          GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                          Incentive = FareData.Incentive,
                                                          PaxType = FareData.PaxType,
                                                          ServiceTax = FareData.ServiceTax,
                                                          Servicecharge = FareData.Servicecharge,
                                                          TDS = FareData.TDS,
                                                          TotalTaxAmount = FareData.TotalTaxAmount,
                                                          TransactionFee = FareData.TransactionFee,

                                                          FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                          JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                          CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                          OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                          MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                          Meal.MealAmt + "~" + Meal.Seg),
                                                          Baggage = "",

                                                          FQT = Flight.AllowFQT,
                                                          Faresid = (indexValue++).ToString(),
                                                          FlightId = FareData.FlightId.ToString(),
                                                          //AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                          AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                          ORGTERMINAL = Flight.StartTerminal,
                                                          DESTERMINAL = Flight.EndTerminal,
                                                          OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                          NumberOfStops = "",
                                                          Via = Flight.Via,
                                                          RefNo = "",
                                                          ToSell = "",
                                                          CNX = "",
                                                          ARRIVALTIME = "",
                                                          DEPARTURETIME = "",
                                                          AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                          FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                          DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                          ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                          STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                          ViaFlights = "",
                                                          Itinref = Myitinref,
                                                          Myitinref = string.IsNullOrEmpty(Flight.ItinRef) ? "" : Flight.ItinRef.ToString().Trim(),
                                                          mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                          BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                          MarkUp = FareData.Markup,
                                                          // OLDmarkup=FareData.OLDmarkup,
                                                          ExtraMrkUp = FareData.ExtraMrkUp,
                                                          originaloldmarkup = FareData.originaloldmarkup,
                                                          DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                          PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                          stocktype = _availResponse.FareCheck.Stocktype,
                                                          seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                          airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                          (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                          OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                          TokenBookingKKK = lstrPriceItenary[myind].Token,
                                                          FareTypeDescription = Flight.FareTypeDescription,
                                                          Sftax = FareData.Sftax ?? "0",
                                                          SFGST = FareData.SFGST ?? "0",
                                                          Servicefee = FareData.Servicefee ?? "0",
                                                          FareType = FareData.FareType.Split('|')[0],
                                                          Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                          Promocodedesc = Flight.PromoCodeDesc,
                                                          Promocode = Flight.PromoCode
                                                      };
                                    ldtFare = Serv.ConvertToDataTable(FareFlights);
                                    // ldtFare = ConvertToDataTable(FareFlights);

                                    #endregion

                                }
                                #endregion

                            }
                            else
                            {
                                #region meals and baggage
                                if (Mealdata.AsEnumerable().First().SegRef == "0" && Baggdata.AsEnumerable().First().SegRef == "0")
                                {
                                    #region Common Bagg & Meal For all segment

                                    var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                      join FareData in GroupFares.AsEnumerable()
                                                      on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                      join SsrMealData in Mealdata.AsEnumerable()
                                                      on 0 equals 0 into SSRMeal
                                                      join SsrBaggData in Baggdata.AsEnumerable()
                                                      on 0 equals 0 into SSRBagg
                                                      from Meal in SSRMeal.DefaultIfEmpty()
                                                      from Baggage in SSRBagg.DefaultIfEmpty()
                                                      select new
                                                      {
                                                          AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                          DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                          ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                          CNXType = Flight.CNX.ToString().Trim(),
                                                          Class = Flight.Class,
                                                          refund = Flight.Refundable,
                                                          Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                          Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                          FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                          FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                          CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                          FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                          JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                          Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                          PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                          Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                          SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                          MULTICLASSAMOUNT = "",
                                                          BaseAmount = FareData.BaseAmount,
                                                          TAXBREAKUP = FareData.BreakUp,
                                                          Commission = FareData.Commission,
                                                          Discount = FareData.Discount,
                                                          GrossAmount = FareData.GrossAmount,
                                                          originaloldgross = FareData.originaloldgross,
                                                          GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                          Incentive = FareData.Incentive,
                                                          PaxType = FareData.PaxType,
                                                          ServiceTax = FareData.ServiceTax,
                                                          Servicecharge = FareData.Servicecharge,
                                                          TDS = FareData.TDS,
                                                          TotalTaxAmount = FareData.TotalTaxAmount,
                                                          TransactionFee = FareData.TransactionFee,

                                                          FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                          JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                          CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                          OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                          MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                          Meal.MealAmt + "~" + Meal.Seg),
                                                          Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                          Baggage.BaggAmt + "~" + Baggage.Seg),

                                                          FQT = Flight.AllowFQT,
                                                          Faresid = (indexValue++).ToString(),
                                                          FlightId = FareData.FlightId.ToString(),
                                                          AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                          ORGTERMINAL = Flight.StartTerminal,
                                                          DESTERMINAL = Flight.EndTerminal,
                                                          OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                          NumberOfStops = "",
                                                          Via = Flight.Via,
                                                          RefNo = "",
                                                          ToSell = "",
                                                          CNX = "",
                                                          ARRIVALTIME = "",
                                                          DEPARTURETIME = "",
                                                          AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                          FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                          DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                          ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                          STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                          ViaFlights = "",
                                                          Itinref = Myitinref,
                                                          Myitinref = string.IsNullOrEmpty(Flight.ItinRef) ? "" : Flight.ItinRef.ToString().Trim(),
                                                          mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                          BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                          MarkUp = FareData.Markup,
                                                          // OLDmarkup=FareData.OLDmarkup,
                                                          ExtraMrkUp = FareData.ExtraMrkUp,
                                                          originaloldmarkup = FareData.Markup,
                                                          DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                          PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                          stocktype = _availResponse.FareCheck.Stocktype,
                                                          seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                          airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                          (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                          OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                          TokenBookingKKK = lstrPriceItenary[myind].Token,
                                                          FareTypeDescription = Flight.FareTypeDescription,
                                                          Sftax = FareData.Sftax ?? "0",
                                                          SFGST = FareData.SFGST ?? "0",
                                                          Servicefee = FareData.Servicefee ?? "0",
                                                          FareType = FareData.FareType.Split('|')[0],
                                                          Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                          Promocodedesc = Flight.PromoCodeDesc,
                                                          Promocode = Flight.PromoCode
                                                      };
                                    ldtFare = Serv.ConvertToDataTable(FareFlights);
                                    // ldtFare = ConvertToDataTable(FareFlights);
                                    #endregion

                                }
                                else if (Mealdata.AsEnumerable().First().SegRef == "0" && Baggdata.AsEnumerable().First().SegRef != "0")
                                {
                                    #region Common Meal & Separate Baggage

                                    var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                      join FareData in GroupFares.AsEnumerable()
                                                      on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                      join SsrMealData in Mealdata.AsEnumerable()
                                                       on 0 equals 0 into SSRMeal
                                                      join SsrBaggData in Baggdata.AsEnumerable()
                                                     on Flight.SegRef equals SsrBaggData.SegRef into SSRBagg
                                                      from Meal in SSRMeal.DefaultIfEmpty()
                                                      from Baggage in SSRBagg.DefaultIfEmpty()
                                                      select new
                                                      {
                                                          AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                          DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                          ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                          CNXType = Flight.CNX.ToString().Trim(),
                                                          Class = Flight.Class,
                                                          refund = Flight.Refundable,
                                                          Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                          Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                          FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                          FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                          CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                          FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                          JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                          Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                          PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                          Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                          SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                          MULTICLASSAMOUNT = "",
                                                          BaseAmount = FareData.BaseAmount,
                                                          TAXBREAKUP = FareData.BreakUp,
                                                          Commission = FareData.Commission,
                                                          Discount = FareData.Discount,
                                                          GrossAmount = FareData.GrossAmount,
                                                          originaloldgross = FareData.originaloldgross,
                                                          GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                          Incentive = FareData.Incentive,
                                                          PaxType = FareData.PaxType,
                                                          ServiceTax = FareData.ServiceTax,
                                                          Servicecharge = FareData.Servicecharge,
                                                          TDS = FareData.TDS,
                                                          TotalTaxAmount = FareData.TotalTaxAmount,
                                                          TransactionFee = FareData.TransactionFee,

                                                          FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                          JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                          CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                          OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                          MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                          Meal.MealAmt + "~" + Meal.Seg),
                                                          Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                          Baggage.BaggAmt + "~" + Baggage.Seg),

                                                          FQT = Flight.AllowFQT,
                                                          Faresid = (indexValue++).ToString(),
                                                          FlightId = FareData.FlightId.ToString(),
                                                          AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                          ORGTERMINAL = Flight.StartTerminal,
                                                          DESTERMINAL = Flight.EndTerminal,
                                                          OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                          NumberOfStops = "",
                                                          Via = Flight.Via,
                                                          RefNo = "",
                                                          ToSell = "",
                                                          CNX = "",
                                                          ARRIVALTIME = "",
                                                          DEPARTURETIME = "",
                                                          AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                          FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                          DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                          ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                          STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                          ViaFlights = "",
                                                          Itinref = Myitinref,
                                                          Myitinref = string.IsNullOrEmpty(Flight.ItinRef) ? "" : Flight.ItinRef.ToString().Trim(),
                                                          mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                          BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                          MarkUp = FareData.Markup,
                                                          // OLDmarkup=FareData.OLDmarkup,
                                                          ExtraMrkUp = FareData.ExtraMrkUp,
                                                          originaloldmarkup = FareData.originaloldmarkup,
                                                          DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                          PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                          stocktype = _availResponse.FareCheck.Stocktype,
                                                          seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                          airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                          (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                          OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                          TokenBookingKKK = lstrPriceItenary[myind].Token,
                                                          FareTypeDescription = Flight.FareTypeDescription,
                                                          Sftax = FareData.Sftax ?? "0",
                                                          SFGST = FareData.SFGST ?? "0",
                                                          Servicefee = FareData.Servicefee ?? "0",
                                                          FareType = FareData.FareType.Split('|')[0],
                                                          Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                          Promocodedesc = Flight.PromoCodeDesc,
                                                          Promocode = Flight.PromoCode
                                                      };
                                    ldtFare = Serv.ConvertToDataTable(FareFlights);
                                    // ldtFare = ConvertToDataTable(FareFlights);
                                    #endregion
                                }
                                else if (Mealdata.AsEnumerable().First().SegRef != "0" && Baggdata.AsEnumerable().First().SegRef == "0")
                                {
                                    #region Common Baggage  & Separate Meal

                                    var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                      join FareData in GroupFares.AsEnumerable()
                                                      on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                      join SsrMealData in Mealdata.AsEnumerable()
                                                       on Flight.SegRef equals SsrMealData.SegRef into SSRMeal
                                                      join SsrBaggData in Baggdata.AsEnumerable()
                                                      on 0 equals 0 into SSRBagg
                                                      from Meal in SSRMeal.DefaultIfEmpty()
                                                      from Baggage in SSRBagg.DefaultIfEmpty()
                                                      select new
                                                      {
                                                          AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                          DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                          ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                          CNXType = Flight.CNX.ToString().Trim(),
                                                          Class = Flight.Class,
                                                          refund = Flight.Refundable,
                                                          Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                          Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                          FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                          FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                          CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                          FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                          JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                          Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                          PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                          Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                          SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                          MULTICLASSAMOUNT = "",
                                                          BaseAmount = FareData.BaseAmount,
                                                          TAXBREAKUP = FareData.BreakUp,
                                                          Commission = FareData.Commission,
                                                          Discount = FareData.Discount,
                                                          GrossAmount = FareData.GrossAmount,
                                                          originaloldgross = FareData.originaloldgross,
                                                          GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                          Incentive = FareData.Incentive,
                                                          PaxType = FareData.PaxType,
                                                          ServiceTax = FareData.ServiceTax,
                                                          Servicecharge = FareData.Servicecharge,
                                                          TDS = FareData.TDS,
                                                          TotalTaxAmount = FareData.TotalTaxAmount,
                                                          TransactionFee = FareData.TransactionFee,

                                                          FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                          JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                          CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                          OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                          MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                          Meal.MealAmt + "~" + Meal.Seg),
                                                          Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                          Baggage.BaggAmt + "~" + Baggage.Seg),

                                                          FQT = Flight.AllowFQT,
                                                          Faresid = (indexValue++).ToString(),
                                                          FlightId = FareData.FlightId.ToString(),
                                                          AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                          ORGTERMINAL = Flight.StartTerminal,
                                                          DESTERMINAL = Flight.EndTerminal,
                                                          OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                          NumberOfStops = "",
                                                          Via = Flight.Via,
                                                          RefNo = "",
                                                          ToSell = "",
                                                          CNX = "",
                                                          ARRIVALTIME = "",
                                                          DEPARTURETIME = "",
                                                          AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                          FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                          DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                          ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                          STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                          ViaFlights = "",
                                                          Itinref = Myitinref,
                                                          Myitinref = string.IsNullOrEmpty(Flight.ItinRef) ? "" : Flight.ItinRef.ToString().Trim(),
                                                          mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                          BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                          MarkUp = FareData.Markup,
                                                          // OLDmarkup=FareData.OLDmarkup,
                                                          ExtraMrkUp = FareData.ExtraMrkUp,
                                                          originaloldmarkup = FareData.originaloldmarkup,
                                                          DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                          PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                          stocktype = _availResponse.FareCheck.Stocktype,
                                                          seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                          airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                          (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                          OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                          TokenBookingKKK = lstrPriceItenary[myind].Token,
                                                          FareTypeDescription = Flight.FareTypeDescription,
                                                          Sftax = FareData.Sftax ?? "0",
                                                          SFGST = FareData.SFGST ?? "0",
                                                          Servicefee = FareData.Servicefee ?? "0",
                                                          FareType = FareData.FareType.Split('|')[0],
                                                          Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                          Promocodedesc = Flight.PromoCodeDesc,
                                                          Promocode = Flight.PromoCode
                                                      };
                                    ldtFare = Serv.ConvertToDataTable(FareFlights);
                                    //ldtFare = ConvertToDataTable(FareFlights);
                                    #endregion
                                }
                                else
                                {
                                    #region individual meal and Baggage

                                    var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                      join FareData in GroupFares.AsEnumerable()
                                                      on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                      join SsrMealData in Mealdata.AsEnumerable()
                                                      on Flight.SegRef equals SsrMealData.SegRef into SSRMeal
                                                      join SsrBaggData in Baggdata.AsEnumerable()
                                                     on Flight.SegRef equals SsrBaggData.SegRef into SSRBagg
                                                      from Meal in SSRMeal.DefaultIfEmpty()
                                                      from Baggage in SSRBagg.DefaultIfEmpty()
                                                      select new
                                                      {
                                                          AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                          DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                          ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                          CNXType = Flight.CNX.ToString().Trim(),
                                                          Class = Flight.Class,
                                                          refund = Flight.Refundable,
                                                          Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                          Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                          FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                          FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                          CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                          FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                          JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                          Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                          PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                          Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                          SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                          MULTICLASSAMOUNT = "",
                                                          BaseAmount = FareData.BaseAmount,
                                                          TAXBREAKUP = FareData.BreakUp,
                                                          Commission = FareData.Commission,
                                                          Discount = FareData.Discount,
                                                          GrossAmount = FareData.GrossAmount,
                                                          originaloldgross = FareData.originaloldgross,
                                                          GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                          Incentive = FareData.Incentive,
                                                          PaxType = FareData.PaxType,
                                                          ServiceTax = FareData.ServiceTax,
                                                          Servicecharge = FareData.Servicecharge,
                                                          TDS = FareData.TDS,
                                                          TotalTaxAmount = FareData.TotalTaxAmount,
                                                          TransactionFee = FareData.TransactionFee,

                                                          FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                          JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                          CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                          OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                          MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                          Meal.MealAmt + "~" + Meal.Seg),
                                                          Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                          Baggage.BaggAmt + "~" + Baggage.Seg),

                                                          FQT = Flight.AllowFQT,
                                                          Faresid = (indexValue++).ToString(),
                                                          FlightId = FareData.FlightId.ToString(),
                                                          //AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                          AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                          ORGTERMINAL = Flight.StartTerminal,
                                                          DESTERMINAL = Flight.EndTerminal,
                                                          OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                          NumberOfStops = "",
                                                          Via = Flight.Via,
                                                          RefNo = "",
                                                          ToSell = "",
                                                          CNX = "",
                                                          ARRIVALTIME = "",
                                                          DEPARTURETIME = "",
                                                          AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                          FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                          DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                          ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                          STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                          ViaFlights = "",
                                                          Itinref = Myitinref,
                                                          Myitinref = string.IsNullOrEmpty(Flight.ItinRef) ? "" : Flight.ItinRef.ToString().Trim(),
                                                          mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                          BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                          MarkUp = FareData.Markup,
                                                          // OLDmarkup=FareData.OLDmarkup,
                                                          ExtraMrkUp = FareData.ExtraMrkUp,
                                                          originaloldmarkup = FareData.originaloldmarkup,
                                                          DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                          PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                          stocktype = _availResponse.FareCheck.Stocktype,
                                                          seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                          airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                          (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                          OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                          TokenBookingKKK = lstrPriceItenary[myind].Token,
                                                          FareTypeDescription = Flight.FareTypeDescription,
                                                          Sftax = FareData.Sftax ?? "0",
                                                          SFGST = FareData.SFGST ?? "0",
                                                          Servicefee = FareData.Servicefee ?? "0",
                                                          FareType = FareData.FareType.Split('|')[0],
                                                          Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                          Promocodedesc = Flight.PromoCodeDesc,
                                                          Promocode = Flight.PromoCode
                                                      };
                                    ldtFare = Serv.ConvertToDataTable(FareFlights);
                                    // ldtFare = ConvertToDataTable(FareFlights);

                                    #endregion
                                }

                                #endregion
                            }
                            dttemp.BeginLoadData();
                            dttemp.Merge(ldtFare.Copy());
                            dttemp.AcceptChanges();
                            dttemp.EndLoadData();

                            if (myind + 1 == _PriceResponse.PriceItenarys.Count)
                            {
                                dtbookreq = dttemp.Copy();
                                DataTable dtmytemp = new DataTable();
                                var temp = (from p in dttemp.AsEnumerable()
                                            where p["SegRef"].ToString() == "1"
                                            select p).Distinct();

                                if (temp.Count() > 0)
                                {
                                    dtmytemp = new DataTable();
                                    dtmytemp = temp.CopyToDataTable();
                                }
                                DataRow drd = dttemp.Rows[0];


                                drd["TAXBREAKUP"] = CombineNewtax((string.Join("/", dtmytemp.AsEnumerable().Select(e => e["TAXBREAKUP"].ToString().Trim().Split('|')[0]).ToArray())),
                                    (string.Join("/", dtmytemp.AsEnumerable().Select(e => (e["TAXBREAKUP"].ToString().Trim().Split('|').Length > 1 ? e["TAXBREAKUP"].ToString().Trim().Split('|')[1] : "KKK")).ToArray())),
                                (string.Join("/", dtmytemp.AsEnumerable().Select(e => (e["TAXBREAKUP"].ToString().Trim().Split('|').Length > 2 ? e["TAXBREAKUP"].ToString().Trim().Split('|')[2] : "KKK")).ToArray())));


                                string pstrrr = "MarkUp,ExtraMrkUp,originaloldmarkup,BaseAmount,Commission,Discount,GrossAmount,originaloldgross,Incentive,ServiceTax,Servicecharge,TDS,TotalTaxAmount,TransactionFee,Sftax,SFGST,Servicefee";

                                for (int index = 0; index < pstrrr.Split(',').Length; index++)
                                {

                                    string colname = pstrrr.Split(',')[index];
                                    drd[colname] = CombineAmount((string.Join("/", dtmytemp.AsEnumerable().Select(e => e[colname].ToString().Trim().Split('|')[0]).ToArray())),
                                  (string.Join("/", dtmytemp.AsEnumerable().Select(e => (e[colname].ToString().Trim().Split('|').Length > 1 ? e[colname].ToString().Trim().Split('|')[1] : "KKK")).ToArray())),
                              (string.Join("/", dtmytemp.AsEnumerable().Select(e => (e[colname].ToString().Trim().Split('|').Length > 2 ? e[colname].ToString().Trim().Split('|')[2] : "KKK")).ToArray())));

                                }
                                dttemp.AcceptChanges();
                            }
                        }
                    }
                    #endregion

                    #region Single Response

                    else if (_PriceResponse.PriceItenarys.Count == 1)
                    {
                        List<RQRS.PriceItenary> lstrPriceItenary = _PriceResponse.PriceItenarys;
                        List<RQRS.AvailabilityRS> lstrAvailResp = lstrPriceItenary[0].PriceRS;

                        RQRS.AvailabilityRS _availResponse = lstrAvailResp[0];

                        var MergeFlights = from FareData in _availResponse.Fares.AsEnumerable()
                                           from TaxTable in FareData.Faredescription.AsEnumerable()
                                           orderby TaxTable.Paxtype ascending
                                           select new
                                           {
                                               BaseAmount = (ConvertToDecimal(TaxTable.BaseAmount ?? "0") + ConvertToDecimal(TaxTable.AddMarkup ?? "0")).ToString(),
                                               Commission = ((ConvertToDecimal(TaxTable.Discount ?? "0") +
                                                   ConvertToDecimal(TaxTable.Incentive ?? "0") + ConvertToDecimal(TaxTable.PLBAmount ?? "0")) - ConvertToDecimal(TaxTable.ServiceTax ?? "0")).ToString(),
                                               Discount = TaxTable.Commission,
                                               GrossAmount = (ConvertToDecimal(TaxTable.AddMarkup ?? "0") + ConvertToDecimal(TaxTable.GrossAmount ?? "0")).ToString(),
                                               originaloldgross = (string.IsNullOrEmpty(TaxTable.OldFare) ? 0 : ConvertToDecimal(TaxTable.OldFare)).ToString(),
                                               Incentive = TaxTable.Incentive,
                                               PaxType = TaxTable.Paxtype,
                                               ServiceTax = TaxTable.ServiceTax,
                                               Servicecharge = TaxTable.Servicecharge,
                                               TDS = TaxTable.TDS,
                                               TotalTaxAmount = TaxTable.TotalTaxAmount,
                                               TransactionFee = TaxTable.TransactionFee,

                                               FlightId = FareData.FlightId,
                                               GrossAmountRef = (ConvertToDecimal(TaxTable.AddMarkup ?? "0") + ConvertToDecimal(TaxTable.GrossAmount ?? "0")).ToString(),
                                               BreakUp = String.Join("/", TaxTable.Taxes.Select(TaxData => TaxData.Code.ToString() + ":"
                                               + (TaxData.Code.ToString().Trim().ToUpper() == "BF" ?
                                               (ConvertToDecimal(TaxTable.AddMarkup ?? "0") + ConvertToDecimal(TaxData.Amount ?? "0")).ToString() :
                                               TaxData.Amount)).ToArray()),
                                               markup = ((string.IsNullOrEmpty(TaxTable.ClientMarkup) ? 0 : ConvertToDecimal(TaxTable.ClientMarkup))).ToString(),

                                               ExtraMrkUp = (string.IsNullOrEmpty(TaxTable.AddMarkup) ? 0 : ConvertToDecimal(TaxTable.AddMarkup)).ToString(),
                                               originaloldmarkup = (string.IsNullOrEmpty(TaxTable.OldMarkup) ? 0 : ConvertToDecimal(TaxTable.OldMarkup)).ToString(),
                                               Sftax = (string.IsNullOrEmpty(TaxTable.SFAMOUNT) ? 0 : ConvertToDecimal(TaxTable.SFAMOUNT)).ToString(),
                                               SFGST = (string.IsNullOrEmpty(TaxTable.SFGST) ? 0 : ConvertToDecimal(TaxTable.SFGST)).ToString(),
                                               Servicefee = (string.IsNullOrEmpty(TaxTable.ServiceFee) ? 0 : ConvertToDecimal(TaxTable.ServiceFee)).ToString(),
                                               FareType = FareData.FareType
                                           };


                        var GroupFares = from FareData in MergeFlights.AsEnumerable()
                                         group FareData by
                                         new
                                         {
                                             Id = FareData.FlightId,
                                         }
                                             into FareGrp
                                         select new
                                         {
                                             BaseAmount = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.BaseAmount) ? "0" : Fares.BaseAmount.ToString()).ToArray(), "JOIN-|"),
                                             Commission = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.Commission) ? "0" : Fares.Commission).ToArray(), "JOIN-|"),
                                             Discount = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.Discount) ? "0" : Fares.Discount).ToArray(), "JOIN-|"),
                                             GrossAmount = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.GrossAmount) ? "0" : Fares.GrossAmount.ToString()).ToArray(), "JOIN-|"),
                                             originaloldgross = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.originaloldgross) ? "0" : Fares.originaloldgross.ToString()).ToArray(), "JOIN-|"),
                                             Incentive = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.Incentive) ? "0" : Fares.Incentive).ToArray(), "JOIN-|"),
                                             PaxType = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.PaxType.ToString()).ToArray(), "JOIN-|"),
                                             ServiceTax = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.ServiceTax) ? "0" : Fares.ServiceTax).ToArray(), "JOIN-|"),
                                             Servicecharge = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.Servicecharge) ? "0" : Fares.Servicecharge).ToArray(), "JOIN-|"),
                                             TDS = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.TDS) ? "" : Fares.TDS).ToArray(), "JOIN-|"),
                                             TotalTaxAmount = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.TotalTaxAmount) ? "0" : Fares.TotalTaxAmount.ToString()).ToArray(), "JOIN-|"),
                                             TransactionFee = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.TransactionFee) ? "0" : Fares.TransactionFee).ToArray(), "JOIN-|"),
                                             FlightId = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.FlightId) ? "0" : Fares.FlightId.ToString()).ToArray(), "ANYONE"),
                                             GrossAmountRef = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.GrossAmount) ? "0" : Fares.GrossAmount.ToString()).ToArray(), "ANYONE"),
                                             BreakUp = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.BreakUp) ? "0" : Fares.BreakUp.ToString()).ToArray(), "JOIN-|"),
                                             Markup = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.markup) ? "0" : Fares.markup).ToArray(), "JOIN-|"),

                                             ExtraMrkUp = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.ExtraMrkUp.ToString()).ToArray(), "JOIN-|"),
                                             originaloldmarkup = GroupByProcessOneCol(FareGrp.Select(Fares => string.IsNullOrEmpty(Fares.originaloldmarkup) ? "0" : Fares.originaloldmarkup).ToArray(), "JOIN-|"),
                                             Sftax = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Sftax.ToString()).ToArray(), "JOIN-|"),
                                             SFGST = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.SFGST.ToString()).ToArray(), "JOIN-|"),
                                             Servicefee = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Servicefee.ToString()).ToArray(), "JOIN-|"),
                                             FareType = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.FareType.ToString()).ToArray(), "JOIN-|"),

                                         };

                        #region Meals and Baggage

                        int indexValue = 0;

                        _availResponse.Meal = _availResponse.Meal == null ? new List<RQRS.Meals>() : _availResponse.Meal;
                        var Mealdata = from meal in _availResponse.Meal.AsEnumerable()
                                       group meal by
                                       new
                                       {
                                           meal.SegRef,
                                       }
                                           into mealfare
                                       select new
                                       {
                                           MealCode = GroupByProcessOneCol((mealfare.Select(Meals => Meals.Code.ToString().Replace("|", "@"))).ToArray(), "JOIN-|"),
                                           MealDesc = GroupByProcessOneCol(mealfare.Select(Meals => Meals.Description.ToString()).ToArray(), "JOIN-|"),
                                           MealAmt = GroupByProcessOneCol(mealfare.Select(Meals => Meals.Amount.ToString()).ToArray(), "JOIN-|"),
                                           SegRef = ((GroupByProcessOneCol(mealfare.Select(Meals => Meals.SegRef.ToString()).ToArray(), "ANYONE").Trim())).ToString(),
                                           Seg = GroupByProcessOneCol(mealfare.Select(Meals => Meals.SegRef.ToString()).ToArray(), "JOIN-|").Trim().ToString(),
                                           itinref = GroupByProcessOneCol(mealfare.Select(Meals => (string.IsNullOrEmpty(Meals.Itinref) ? "" : Meals.Itinref.ToString())).ToArray(), "JOIN-|").Trim().ToString(),
                                       };

                        _availResponse.Bagg = _availResponse.Bagg == null ? new List<RQRS.Bagg>() : _availResponse.Bagg;

                        var Baggdata = from Baggage in _availResponse.Bagg.AsEnumerable()
                                       group Baggage by
                                       new
                                       {
                                           Baggage.SegRef,
                                       }
                                           into Baggfare
                                       select new
                                       {
                                           BaggCode = GroupByProcessOneCol((Baggfare.Select(Meals => Meals.Code.ToString().Replace("|", "@"))).ToArray(), "JOIN-|"),
                                           BaggDesc = GroupByProcessOneCol(Baggfare.Select(Meals => Meals.Description.ToString()).ToArray(), "JOIN-|"),
                                           BaggAmt = GroupByProcessOneCol(Baggfare.Select(Meals => Meals.Amount.ToString()).ToArray(), "JOIN-|"),
                                           SegRef = ((GroupByProcessOneCol(Baggfare.Select(Meals => Meals.SegRef.ToString()).ToArray(), "ANYONE").Trim())).ToString(),
                                           Seg = GroupByProcessOneCol(Baggfare.Select(Meals => Meals.SegRef.ToString()).ToArray(), "JOIN-|").Trim().ToString(),
                                           Itinref = GroupByProcessOneCol(Baggfare.Select(Meals => (string.IsNullOrEmpty(Meals.Itinref) ? "" : Meals.Itinref.ToString())).ToArray(), "JOIN-|").Trim().ToString(),
                                       };


                        string lstrmyref = "";
                        int indexplus = 0;
                        var result = _availResponse.Bagg.AsEnumerable().Select(org => org.Orgin + org.Destination).Distinct().ToArray().Select((val, index) => new { val, index });
                        var Baggdatatemp = (from Baggage in _availResponse.Bagg.AsEnumerable()
                                            select new
                                            {
                                                Segment = Baggage.Orgin + "->" + Baggage.Destination,
                                                Segment_Code = sector(Baggage.Orgin + "->" + Baggage.Destination, ref lstrmyref, ref indexplus),
                                                BaggageCode = Baggage.Code.Replace("|", "@"),
                                                BaggageDesc = Baggage.Description + (" (" + Baggage.Amount + " )"),
                                                BaggageAmt = Baggage.Amount,
                                                SegRef = Baggage.SegRef,
                                                Itinref = (string.IsNullOrEmpty(Baggage.Itinref) ? "" : Baggage.Itinref),
                                                MyRef = result.Where(id => id.val.Equals(Baggage.Orgin + Baggage.Destination)).FirstOrDefault().index,
                                                Addsegorg = Baggage.SegRef + "BAggSPLITbaGG" + Baggage.Orgin + "BAggSPLITbaGG" + Baggage.Destination + "BAggSPLITbaGG" + Baggage.Description + "BAggSPLITbaGG" + (string.IsNullOrEmpty(Baggage.Itinref) ? "" : Baggage.Itinref),
                                                BaggageText = Baggage.BaggageText
                                            }).Distinct();

                        if (Baggdatatemp.Count() > 0)
                        {
                            dtbagg = Serv.ConvertToDataTable(Baggdatatemp);
                            dtbagg.TableName = "Baggage";
                        }


                        //DataTable dtMeal = new DataTable();

                        lstrmyref = "";
                        indexplus = 0;
                        //autoid = 0;
                        result = _availResponse.Meal.AsEnumerable().Select(org => org.Orgin + org.Destination).Distinct().ToArray().Select((val, index) => new { val, index });
                        var Mealdatatemp = (from Baggage in _availResponse.Meal.AsEnumerable()
                                            select new
                                            {
                                                Segment = Baggage.Orgin + "->" + Baggage.Destination,
                                                Segment_Code = sector(Baggage.Orgin + "->" + Baggage.Destination, ref lstrmyref, ref indexplus),
                                                MealCode = Baggage.Code.Replace("|", "@"),
                                                MealDesc = Baggage.Description + (" (" + Baggage.Amount + " )"),
                                                MealAmt = Baggage.Amount,
                                                SegRef = Baggage.SegRef,
                                                Itinref = (string.IsNullOrEmpty(Baggage.Itinref) ? "" : Baggage.Itinref),
                                                MyRef = result.Where(id => id.val.Equals(Baggage.Orgin + Baggage.Destination)).FirstOrDefault().index,
                                                Addsegorg = Baggage.SegRef + "MEALSRSPLITbaGG" + Baggage.Orgin + "MEALSRSPLITbaGG" + Baggage.Destination + "MEALSRSPLITbaGG" + Baggage.Description + "MEALSRSPLITbaGG" + (string.IsNullOrEmpty(Baggage.Itinref) ? "" : Baggage.Itinref),
                                                ServiceMeal = Baggage.IsBundleServiceMeal
                                            }).Distinct();

                        if (Mealdatatemp.Count() > 0)
                        {
                            dtMeal = Serv.ConvertToDataTable(Mealdatatemp);
                            dtMeal.TableName = "Meals";
                            // dsSSR.Tables.Add(dtMeal);
                        }

                        //DataTable odt = new DataTable();
                        lstrmyref = "";
                        indexplus = 0;
                        //autoid = 0;

                        _availResponse.OtherService = _availResponse.OtherService == null ? new List<RQRS.Other>() : _availResponse.OtherService;
                        result = _availResponse.OtherService.AsEnumerable().Select(org => org.Orgin + org.Destination).Distinct().ToArray().Select((val, index) => new { val, index });
                        var Otherssrtemp = (from Othrssr in _availResponse.OtherService.AsEnumerable()
                                                // where (Othrssr.SSRType == "SPICEMAX" || Othrssr.SSRType == "BAGOUT" || Othrssr.SSRType == "PRIORITY_CHECK_IN")
                                            where ((!string.IsNullOrEmpty(Othrssr.SSRType)) && Othrssr.SSRType.ToUpper() != "BAGOUT")
                                            select new
                                            {
                                                Segment = Othrssr.Orgin + "->" + Othrssr.Destination,
                                                Segment_Code = sector(Othrssr.Orgin + "->" + Othrssr.Destination, ref lstrmyref, ref indexplus),
                                                //OthrSSR_Code = Othrssr.SSRCode,
                                                OthrSSR_Code = Othrssr.SSRCode,
                                                OthrSSR_Desc = Othrssr.Description + (" (" + Othrssr.Amount + " )"),
                                                OthrSSR_Details = Othrssr.Description,
                                                OtherSSR_orgin = Othrssr.Orgin,
                                                OtherSSR_destina = Othrssr.Destination,
                                                OthrSSR_Amt = Othrssr.Amount,
                                                SegRef = Othrssr.SegRef,
                                                OtherSSR_Type = Othrssr.SSRType,
                                                Itinref = (string.IsNullOrEmpty(Othrssr.Itinref) ? "" : Othrssr.Itinref),
                                                MyRef = result.Where(id => id.val.Equals(Othrssr.Orgin + Othrssr.Destination)).FirstOrDefault().index,
                                            }).Distinct();

                        if (Otherssrtemp.Count() > 0)
                        {
                            odt = Serv.ConvertToDataTable(Otherssrtemp);
                            odt.TableName = "OtherSSR";
                            // dsSSR.Tables.Add(dtMeal);
                        }

                        var Otherssrbagouttemp = (from Othrssr in _availResponse.OtherService.AsEnumerable()//177
                                                  where ((!string.IsNullOrEmpty(Othrssr.SSRType)) && Othrssr.SSRType.ToUpper() == "BAGOUT")
                                                  select new
                                                  {
                                                      Segment = Othrssr.Orgin + "->" + Othrssr.Destination,
                                                      Segment_Code = sector(Othrssr.Orgin + "->" + Othrssr.Destination, ref lstrmyref, ref indexplus),
                                                      OthrSSR_Code = Othrssr.SSRCode,
                                                      OthrSSR_Desc = Othrssr.Description + (" (" + Othrssr.Amount + " )"),
                                                      OthrSSR_Details = Othrssr.Description,
                                                      OtherSSR_orgin = Othrssr.Orgin,
                                                      OtherSSR_destina = Othrssr.Destination,
                                                      OthrSSR_Amt = Othrssr.Amount,
                                                      SegRef = Othrssr.SegRef,
                                                      OtherSSR_Type = Othrssr.SSRType,
                                                      Itinref = (string.IsNullOrEmpty(Othrssr.Itinref) ? "" : Othrssr.Itinref),
                                                      MyRef = result.Where(id => id.val.Equals(Othrssr.Orgin + Othrssr.Destination)).FirstOrDefault().index,
                                                  }).Distinct();

                        if (Otherssrbagouttemp.Count() > 0)
                        {
                            dtbagout = Serv.ConvertToDataTable(Otherssrbagouttemp);
                            dtbagout.TableName = "OtherSSRBAGOUT";
                        }

                        #endregion


                        DataTable ldtFare = new DataTable();
                        if ((_availResponse.Meal.Count == 0 || Mealdata == null) &&
                         (_availResponse.Bagg.Count == 0 || Baggdata == null))
                        {
                            #region no meals and no baggage
                            var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                              join FareData in GroupFares.AsEnumerable()
                                              //on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                              on Flight.FareId equals FareData.FlightId
                                              select new
                                              {
                                                  AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                  DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                  ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                  CNXType = Flight.CNX.ToString().Trim(),
                                                  Class = Flight.Class,
                                                  refund = Flight.Refundable,
                                                  Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                  Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                  FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                  FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                  CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                  FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                  JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                  Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                  PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                  Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                  SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                  MULTICLASSAMOUNT = "",
                                                  BaseAmount = FareData.BaseAmount,
                                                  TAXBREAKUP = FareData.BreakUp,
                                                  Commission = FareData.Commission,
                                                  Discount = FareData.Discount,
                                                  GrossAmount = FareData.GrossAmount,
                                                  originaloldgross = FareData.originaloldgross,
                                                  GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                  Incentive = FareData.Incentive,
                                                  PaxType = FareData.PaxType,
                                                  ServiceTax = FareData.ServiceTax,
                                                  Servicecharge = FareData.Servicecharge,
                                                  TDS = FareData.TDS,
                                                  TotalTaxAmount = FareData.TotalTaxAmount,
                                                  TransactionFee = FareData.TransactionFee,

                                                  MarkUp = FareData.Markup,
                                                  //OLDmarkup=FareData.OLDmarkup,
                                                  ExtraMrkUp = FareData.ExtraMrkUp,
                                                  originaloldmarkup = FareData.originaloldmarkup,

                                                  FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                  JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),

                                                  CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                  OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                  MEAL = "",
                                                  Baggage = "",

                                                  FQT = Flight.AllowFQT,
                                                  Faresid = (indexValue++).ToString(),
                                                  FlightId = FareData.FlightId.ToString(),
                                                  AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                  ORGTERMINAL = Flight.StartTerminal,
                                                  DESTERMINAL = Flight.EndTerminal,
                                                  OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                  NumberOfStops = "",
                                                  Via = Flight.Via,
                                                  RefNo = "",
                                                  ToSell = "",
                                                  CNX = "",
                                                  ARRIVALTIME = "",
                                                  DEPARTURETIME = "",
                                                  AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                  FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                  DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                  ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                  STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                  ViaFlights = "",
                                                  itinRef = Flight.ItinRef.ToString().Trim(),
                                                  mealAvail = false,// _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                  BaggAvail = false,//_availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                  DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                  PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                  stocktype = _availResponse.FareCheck.Stocktype,
                                                  seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                  airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                      (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                  OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                  TokenBookingKKK = lstrPriceItenary[0].Token,
                                                  FareTypeDescription = Flight.FareTypeDescription,
                                                  Sftax = FareData.Sftax ?? "0",
                                                  SFGST = FareData.SFGST ?? "0",
                                                  Servicefee = FareData.Servicefee ?? "0",
                                                  FareType = FareData.FareType.Split('|')[0],
                                                  Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                  Promocodedesc = Flight.PromoCodeDesc,
                                                  Promocode = Flight.PromoCode
                                              };

                            ldtFare = Serv.ConvertToDataTable(FareFlights);
                            #endregion
                        }
                        else if ((_availResponse.Meal.Count == 0 || Mealdata == null) &&
                                 (Baggdata != null && _availResponse.Bagg.Count > 0))
                        {
                            #region only baggage


                            if (Baggdata.AsEnumerable().First().SegRef == "0")
                            {
                                #region Common Baggage
                                var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                  join FareData in GroupFares.AsEnumerable()
                                                 // on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                 on Flight.FareId equals FareData.FlightId
                                                  join SsrBaggData in Baggdata.AsEnumerable()
                                                 on 0 equals 0 into SSRBagg
                                                  from Baggage in SSRBagg.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                      DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                      ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                      CNXType = Flight.CNX.ToString().Trim(),
                                                      Class = Flight.Class,
                                                      refund = Flight.Refundable,
                                                      Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                      Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                      FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                      FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                      CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                      FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                      JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                      Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                      PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                      Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                      SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                      MULTICLASSAMOUNT = "",
                                                      BaseAmount = FareData.BaseAmount,
                                                      TAXBREAKUP = FareData.BreakUp,
                                                      Commission = FareData.Commission,
                                                      Discount = FareData.Discount,
                                                      GrossAmount = FareData.GrossAmount,
                                                      originaloldgross = FareData.originaloldgross,
                                                      GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                      Incentive = FareData.Incentive,
                                                      PaxType = FareData.PaxType,
                                                      ServiceTax = FareData.ServiceTax,
                                                      Servicecharge = FareData.Servicecharge,
                                                      TDS = FareData.TDS,
                                                      TotalTaxAmount = FareData.TotalTaxAmount,
                                                      TransactionFee = FareData.TransactionFee,
                                                      MarkUp = FareData.Markup,
                                                      //OLDmarkup=FareData.OLDmarkup,
                                                      ExtraMrkUp = FareData.ExtraMrkUp,
                                                      originaloldmarkup = FareData.originaloldmarkup,
                                                      FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                      JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),

                                                      CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                      OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                      MEAL = "",
                                                      Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                      Baggage.BaggAmt + "~" + Baggage.Seg),

                                                      FQT = Flight.AllowFQT,
                                                      Faresid = (indexValue++).ToString(),
                                                      FlightId = FareData.FlightId.ToString(),
                                                      // AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                      AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                      ORGTERMINAL = Flight.StartTerminal,
                                                      DESTERMINAL = Flight.EndTerminal,
                                                      OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                      NumberOfStops = "",
                                                      Via = Flight.Via,
                                                      RefNo = "",
                                                      ToSell = "",
                                                      CNX = "",
                                                      ARRIVALTIME = "",
                                                      DEPARTURETIME = "",
                                                      AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                      FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                      DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                      ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                      STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                      ViaFlights = "",
                                                      itinRef = Flight.ItinRef.ToString().Trim(),
                                                      mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                      BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,

                                                      DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                      PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                      stocktype = _availResponse.FareCheck.Stocktype,
                                                      seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                      airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                      (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                      OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                      TokenBookingKKK = lstrPriceItenary[0].Token,
                                                      FareTypeDescription = Flight.FareTypeDescription,
                                                      Sftax = FareData.Sftax ?? "0",
                                                      SFGST = FareData.SFGST ?? "0",
                                                      Servicefee = FareData.Servicefee ?? "0",
                                                      FareType = FareData.FareType.Split('|')[0],
                                                      Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                      Promocodedesc = Flight.PromoCodeDesc,
                                                      Promocode = Flight.PromoCode
                                                  };

                                ldtFare = Serv.ConvertToDataTable(FareFlights);
                                #endregion

                            }
                            else
                            {
                                #region Individual Baggage

                                var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                  join FareData in GroupFares.AsEnumerable()
                                                 // on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                 on Flight.FareId equals FareData.FlightId
                                                  join SsrBaggData in Baggdata.AsEnumerable()
                                                 on Flight.SegRef equals SsrBaggData.SegRef into SSRBagg
                                                  from Baggage in SSRBagg.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                      DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                      ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                      CNXType = Flight.CNX.ToString().Trim(),
                                                      Class = Flight.Class,
                                                      refund = Flight.Refundable,
                                                      Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                      Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                      FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                      FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                      CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                      FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                      JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                      Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                      PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                      Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                      SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                      MULTICLASSAMOUNT = "",
                                                      BaseAmount = FareData.BaseAmount,
                                                      TAXBREAKUP = FareData.BreakUp,
                                                      Commission = FareData.Commission,
                                                      Discount = FareData.Discount,
                                                      GrossAmount = FareData.GrossAmount,
                                                      originaloldgross = FareData.originaloldgross,
                                                      GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                      Incentive = FareData.Incentive,
                                                      PaxType = FareData.PaxType,
                                                      ServiceTax = FareData.ServiceTax,
                                                      Servicecharge = FareData.Servicecharge,
                                                      TDS = FareData.TDS,
                                                      TotalTaxAmount = FareData.TotalTaxAmount,
                                                      TransactionFee = FareData.TransactionFee,

                                                      FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                      JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                      CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                      OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                      MEAL = "",
                                                      Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                      Baggage.BaggAmt + "~" + Baggage.Seg),

                                                      FQT = Flight.AllowFQT,
                                                      Faresid = (indexValue++).ToString(),
                                                      FlightId = FareData.FlightId.ToString(),
                                                      //AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                      AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                      ORGTERMINAL = Flight.StartTerminal,
                                                      DESTERMINAL = Flight.EndTerminal,
                                                      OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                      NumberOfStops = "",
                                                      Via = Flight.Via,
                                                      RefNo = "",
                                                      ToSell = "",
                                                      CNX = "",
                                                      ARRIVALTIME = "",
                                                      DEPARTURETIME = "",
                                                      AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                      FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                      DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                      ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                      STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                      ViaFlights = "",
                                                      itinRef = Flight.ItinRef.ToString().Trim(),
                                                      mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                      BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                      DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                      PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                      MarkUp = FareData.Markup,
                                                      //OLDmarkup=FareData.OLDmarkup,
                                                      ExtraMrkUp = FareData.ExtraMrkUp,
                                                      originaloldmarkup = FareData.originaloldmarkup,
                                                      stocktype = _availResponse.FareCheck.Stocktype,
                                                      seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                      airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                      (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                      OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                      TokenBookingKKK = lstrPriceItenary[0].Token,
                                                      FareTypeDescription = Flight.FareTypeDescription,
                                                      Sftax = FareData.Sftax ?? "0",
                                                      SFGST = FareData.SFGST ?? "0",
                                                      Servicefee = FareData.Servicefee ?? "0",
                                                      FareType = FareData.FareType.Split('|')[0],
                                                      Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                      Promocodedesc = Flight.PromoCodeDesc,
                                                      Promocode = Flight.PromoCode
                                                  };
                                ldtFare = Serv.ConvertToDataTable(FareFlights);

                                #endregion

                            }

                            #endregion

                        }
                        else if ((_availResponse.Bagg.Count == 0 || Baggdata == null) &&
                                 (Mealdata != null && _availResponse.Meal.Count > 0))
                        {
                            #region only Meals
                            if (Mealdata.AsEnumerable().First().SegRef == "0")
                            {
                                #region common meals for all segment

                                var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                  join FareData in GroupFares.AsEnumerable()
                                                 // on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                 on Flight.FareId equals FareData.FlightId
                                                  join SsrMealData in Mealdata.AsEnumerable()
                                                  on 0 equals 0 into SSRMeal
                                                  // join SsrBaggData in Baggdata.AsEnumerable()
                                                  //on Flight.SegRef equals SsrBaggData.SegRef into SSRBagg
                                                  from Meal in SSRMeal.DefaultIfEmpty()
                                                      //from Baggage in SSRBagg.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                      DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                      ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                      CNXType = Flight.CNX.ToString().Trim(),
                                                      Class = Flight.Class,
                                                      refund = Flight.Refundable,
                                                      Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                      Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                      FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                      FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                      CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                      FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                      JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                      Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                      PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                      Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                      SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                      MULTICLASSAMOUNT = "",
                                                      BaseAmount = FareData.BaseAmount,
                                                      TAXBREAKUP = FareData.BreakUp,
                                                      Commission = FareData.Commission,
                                                      Discount = FareData.Discount,
                                                      GrossAmount = FareData.GrossAmount,
                                                      originaloldgross = FareData.originaloldgross,
                                                      GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                      Incentive = FareData.Incentive,
                                                      PaxType = FareData.PaxType,
                                                      ServiceTax = FareData.ServiceTax,
                                                      Servicecharge = FareData.Servicecharge,
                                                      TDS = FareData.TDS,
                                                      TotalTaxAmount = FareData.TotalTaxAmount,
                                                      TransactionFee = FareData.TransactionFee,
                                                      MarkUp = FareData.Markup,
                                                      //OLDmarkup=FareData.OLDmarkup,
                                                      ExtraMrkUp = FareData.ExtraMrkUp,
                                                      originaloldmarkup = FareData.originaloldmarkup,
                                                      FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                      JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),

                                                      CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                      OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                      MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                      Meal.MealAmt + "~" + Meal.Seg),
                                                      //Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                      //Baggage.BaggAmt + "~" + Baggage.Seg),
                                                      Baggage = "",

                                                      FQT = Flight.AllowFQT,
                                                      Faresid = (indexValue++).ToString(),
                                                      FlightId = FareData.FlightId.ToString(),
                                                      //AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                      AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                      ORGTERMINAL = Flight.StartTerminal,
                                                      DESTERMINAL = Flight.EndTerminal,
                                                      OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                      NumberOfStops = "",
                                                      Via = Flight.Via,
                                                      RefNo = "",
                                                      ToSell = "",
                                                      CNX = "",
                                                      ARRIVALTIME = "",
                                                      DEPARTURETIME = "",
                                                      AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                      FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                      DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                      ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                      STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                      ViaFlights = "",
                                                      itinRef = Flight.ItinRef.ToString().Trim(),
                                                      mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                      BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                      DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                      PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                      stocktype = _availResponse.FareCheck.Stocktype,
                                                      seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                      airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                      (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                      OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                      TokenBookingKKK = lstrPriceItenary[0].Token,
                                                      FareTypeDescription = Flight.FareTypeDescription,
                                                      Sftax = FareData.Sftax ?? "0",
                                                      SFGST = FareData.SFGST ?? "0",
                                                      Servicefee = FareData.Servicefee ?? "0",
                                                      FareType = FareData.FareType.Split('|')[0],
                                                      Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                      Promocodedesc = Flight.PromoCodeDesc,
                                                      Promocode = Flight.PromoCode
                                                  };

                                ldtFare = Serv.ConvertToDataTable(FareFlights);
                                #endregion
                            }
                            else
                            {

                                #region Individual meal for every segment

                                var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                  join FareData in GroupFares.AsEnumerable()
                                                  on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                  join SsrMealData in Mealdata.AsEnumerable()
                                                  on Flight.SegRef equals SsrMealData.SegRef into SSRMeal
                                                  from Meal in SSRMeal.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                      DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                      ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                      CNXType = Flight.CNX.ToString().Trim(),
                                                      Class = Flight.Class,
                                                      refund = Flight.Refundable,
                                                      Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                      Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                      FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                      FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                      CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                      FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                      JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                      Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                      PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                      Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                      SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                      MULTICLASSAMOUNT = "",
                                                      BaseAmount = FareData.BaseAmount,
                                                      TAXBREAKUP = FareData.BreakUp,
                                                      Commission = FareData.Commission,
                                                      Discount = FareData.Discount,
                                                      GrossAmount = FareData.GrossAmount,
                                                      originaloldgross = FareData.originaloldgross,
                                                      GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                      Incentive = FareData.Incentive,
                                                      PaxType = FareData.PaxType,
                                                      ServiceTax = FareData.ServiceTax,
                                                      Servicecharge = FareData.Servicecharge,
                                                      TDS = FareData.TDS,
                                                      TotalTaxAmount = FareData.TotalTaxAmount,
                                                      TransactionFee = FareData.TransactionFee,

                                                      FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                      JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                      CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                      OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                      MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                      Meal.MealAmt + "~" + Meal.Seg),
                                                      Baggage = "",

                                                      FQT = Flight.AllowFQT,
                                                      Faresid = (indexValue++).ToString(),
                                                      FlightId = FareData.FlightId.ToString(),
                                                      //AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                      AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                      ORGTERMINAL = Flight.StartTerminal,
                                                      DESTERMINAL = Flight.EndTerminal,
                                                      OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                      NumberOfStops = "",
                                                      Via = Flight.Via,
                                                      RefNo = "",
                                                      ToSell = "",
                                                      CNX = "",
                                                      ARRIVALTIME = "",
                                                      DEPARTURETIME = "",
                                                      AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                      FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                      DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                      ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                      STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                      ViaFlights = "",
                                                      itinRef = string.IsNullOrEmpty(Flight.ItinRef) ? "" : Flight.ItinRef.ToString().Trim(),
                                                      mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                      BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                      MarkUp = FareData.Markup,
                                                      // OLDmarkup=FareData.OLDmarkup,
                                                      ExtraMrkUp = FareData.ExtraMrkUp,
                                                      originaloldmarkup = FareData.originaloldmarkup,
                                                      DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                      PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                      stocktype = _availResponse.FareCheck.Stocktype,
                                                      seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                      airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                      (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                      OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                      TokenBookingKKK = lstrPriceItenary[0].Token,
                                                      FareTypeDescription = Flight.FareTypeDescription,
                                                      Sftax = FareData.Sftax ?? "0",
                                                      SFGST = FareData.SFGST ?? "0",
                                                      Servicefee = FareData.Servicefee ?? "0",
                                                      FareType = FareData.FareType.Split('|')[0],
                                                      Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                      Promocodedesc = Flight.PromoCodeDesc,
                                                      Promocode = Flight.PromoCode
                                                  };
                                ldtFare = Serv.ConvertToDataTable(FareFlights);
                                // ldtFare = ConvertToDataTable(FareFlights);

                                #endregion

                            }
                            #endregion

                        }
                        else
                        {
                            #region meals and baggage
                            if (Mealdata.AsEnumerable().First().SegRef == "0" && Baggdata.AsEnumerable().First().SegRef == "0")
                            {
                                #region Common Bagg & Meal For all segment

                                var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                  join FareData in GroupFares.AsEnumerable()
                                                 // on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                 on Flight.FareId equals FareData.FlightId
                                                  join SsrMealData in Mealdata.AsEnumerable()
                                                  on 0 equals 0 into SSRMeal
                                                  join SsrBaggData in Baggdata.AsEnumerable()
                                                  on 0 equals 0 into SSRBagg
                                                  from Meal in SSRMeal.DefaultIfEmpty()
                                                  from Baggage in SSRBagg.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                      DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                      ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                      CNXType = Flight.CNX.ToString().Trim(),
                                                      Class = Flight.Class,
                                                      refund = Flight.Refundable,
                                                      Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                      Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                      FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                      FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                      CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                      FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                      JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                      Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                      PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                      Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                      SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                      MULTICLASSAMOUNT = "",
                                                      BaseAmount = FareData.BaseAmount,
                                                      TAXBREAKUP = FareData.BreakUp,
                                                      Commission = FareData.Commission,
                                                      Discount = FareData.Discount,
                                                      GrossAmount = FareData.GrossAmount,
                                                      originaloldgross = FareData.originaloldgross,
                                                      GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                      Incentive = FareData.Incentive,
                                                      PaxType = FareData.PaxType,
                                                      ServiceTax = FareData.ServiceTax,
                                                      Servicecharge = FareData.Servicecharge,
                                                      TDS = FareData.TDS,
                                                      TotalTaxAmount = FareData.TotalTaxAmount,
                                                      TransactionFee = FareData.TransactionFee,

                                                      FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                      JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                      CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                      OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                      MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                      Meal.MealAmt + "~" + Meal.Seg),
                                                      Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                      Baggage.BaggAmt + "~" + Baggage.Seg),

                                                      FQT = Flight.AllowFQT,
                                                      Faresid = (indexValue++).ToString(),
                                                      FlightId = FareData.FlightId.ToString(),
                                                      AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                      ORGTERMINAL = Flight.StartTerminal,
                                                      DESTERMINAL = Flight.EndTerminal,
                                                      OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                      NumberOfStops = "",
                                                      Via = Flight.Via,
                                                      RefNo = "",
                                                      ToSell = "",
                                                      CNX = "",
                                                      ARRIVALTIME = "",
                                                      DEPARTURETIME = "",
                                                      AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                      FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                      DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                      ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                      STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                      ViaFlights = "",
                                                      itinRef = string.IsNullOrEmpty(Flight.ItinRef) ? "" : Flight.ItinRef.ToString().Trim(),
                                                      mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                      BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                      MarkUp = FareData.Markup,
                                                      // OLDmarkup=FareData.OLDmarkup,
                                                      ExtraMrkUp = FareData.ExtraMrkUp,
                                                      originaloldmarkup = FareData.Markup,
                                                      DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                      PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                      stocktype = _availResponse.FareCheck.Stocktype,
                                                      seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                      airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                      (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                      OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                      TokenBookingKKK = lstrPriceItenary[0].Token,
                                                      FareTypeDescription = Flight.FareTypeDescription,
                                                      Sftax = FareData.Sftax ?? "0",
                                                      SFGST = FareData.SFGST ?? "0",
                                                      Servicefee = FareData.Servicefee ?? "0",
                                                      FareType = FareData.FareType.Split('|')[0],
                                                      Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                      Promocodedesc = Flight.PromoCodeDesc,
                                                      Promocode = Flight.PromoCode
                                                  };
                                ldtFare = Serv.ConvertToDataTable(FareFlights);
                                // ldtFare = ConvertToDataTable(FareFlights);
                                #endregion

                            }
                            else if (Mealdata.AsEnumerable().First().SegRef == "0" && Baggdata.AsEnumerable().First().SegRef != "0")
                            {
                                #region Common Meal & Separate Baggage

                                var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                  join FareData in GroupFares.AsEnumerable()
                                                 // on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                 on Flight.FareId equals FareData.FlightId
                                                  join SsrMealData in Mealdata.AsEnumerable()
                                                   on 0 equals 0 into SSRMeal
                                                  join SsrBaggData in Baggdata.AsEnumerable()
                                                 on Flight.SegRef equals SsrBaggData.SegRef into SSRBagg
                                                  from Meal in SSRMeal.DefaultIfEmpty()
                                                  from Baggage in SSRBagg.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                      DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                      ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                      CNXType = Flight.CNX.ToString().Trim(),
                                                      Class = Flight.Class,
                                                      refund = Flight.Refundable,
                                                      Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                      Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                      FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                      FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                      CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                      FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                      JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                      Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                      PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                      Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                      SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                      MULTICLASSAMOUNT = "",
                                                      BaseAmount = FareData.BaseAmount,
                                                      TAXBREAKUP = FareData.BreakUp,
                                                      Commission = FareData.Commission,
                                                      Discount = FareData.Discount,
                                                      GrossAmount = FareData.GrossAmount,
                                                      originaloldgross = FareData.originaloldgross,
                                                      GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                      Incentive = FareData.Incentive,
                                                      PaxType = FareData.PaxType,
                                                      ServiceTax = FareData.ServiceTax,
                                                      Servicecharge = FareData.Servicecharge,
                                                      TDS = FareData.TDS,
                                                      TotalTaxAmount = FareData.TotalTaxAmount,
                                                      TransactionFee = FareData.TransactionFee,

                                                      FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                      JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                      CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                      OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                      MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                      Meal.MealAmt + "~" + Meal.Seg),
                                                      Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                      Baggage.BaggAmt + "~" + Baggage.Seg),

                                                      FQT = Flight.AllowFQT,
                                                      Faresid = (indexValue++).ToString(),
                                                      FlightId = FareData.FlightId.ToString(),
                                                      AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                      ORGTERMINAL = Flight.StartTerminal,
                                                      DESTERMINAL = Flight.EndTerminal,
                                                      OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                      NumberOfStops = "",
                                                      Via = Flight.Via,
                                                      RefNo = "",
                                                      ToSell = "",
                                                      CNX = "",
                                                      ARRIVALTIME = "",
                                                      DEPARTURETIME = "",
                                                      AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                      FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                      DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                      ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                      STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                      ViaFlights = "",
                                                      itinRef = string.IsNullOrEmpty(Flight.ItinRef) ? "" : Flight.ItinRef.ToString().Trim(),
                                                      mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                      BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                      MarkUp = FareData.Markup,
                                                      // OLDmarkup=FareData.OLDmarkup,
                                                      ExtraMrkUp = FareData.ExtraMrkUp,
                                                      originaloldmarkup = FareData.originaloldmarkup,
                                                      DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                      PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                      stocktype = _availResponse.FareCheck.Stocktype,
                                                      seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                      airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                      (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                      OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                      TokenBookingKKK = lstrPriceItenary[0].Token,
                                                      FareTypeDescription = Flight.FareTypeDescription,
                                                      Sftax = FareData.Sftax ?? "0",
                                                      SFGST = FareData.SFGST ?? "0",
                                                      Servicefee = FareData.Servicefee ?? "0",
                                                      FareType = FareData.FareType.Split('|')[0],
                                                      Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                      Promocodedesc = Flight.PromoCodeDesc,
                                                      Promocode = Flight.PromoCode
                                                  };
                                ldtFare = Serv.ConvertToDataTable(FareFlights);
                                // ldtFare = ConvertToDataTable(FareFlights);
                                #endregion
                            }
                            else if (Mealdata.AsEnumerable().First().SegRef != "0" && Baggdata.AsEnumerable().First().SegRef == "0")
                            {
                                #region Common Baggage  & Separate Meal

                                var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                  join FareData in GroupFares.AsEnumerable()
                                                  //on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                  on Flight.FareId equals FareData.FlightId
                                                  join SsrMealData in Mealdata.AsEnumerable()
                                                   on Flight.SegRef equals SsrMealData.SegRef into SSRMeal
                                                  join SsrBaggData in Baggdata.AsEnumerable()
                                                  on 0 equals 0 into SSRBagg
                                                  from Meal in SSRMeal.DefaultIfEmpty()
                                                  from Baggage in SSRBagg.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                      DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                      ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                      CNXType = Flight.CNX.ToString().Trim(),
                                                      Class = Flight.Class,
                                                      refund = Flight.Refundable,
                                                      Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                      Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                      FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                      FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                      CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                      FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                      JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                      Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                      PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                      Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                      SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                      MULTICLASSAMOUNT = "",
                                                      BaseAmount = FareData.BaseAmount,
                                                      TAXBREAKUP = FareData.BreakUp,
                                                      Commission = FareData.Commission,
                                                      Discount = FareData.Discount,
                                                      GrossAmount = FareData.GrossAmount,
                                                      originaloldgross = FareData.originaloldgross,
                                                      GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                      Incentive = FareData.Incentive,
                                                      PaxType = FareData.PaxType,
                                                      ServiceTax = FareData.ServiceTax,
                                                      Servicecharge = FareData.Servicecharge,
                                                      TDS = FareData.TDS,
                                                      TotalTaxAmount = FareData.TotalTaxAmount,
                                                      TransactionFee = FareData.TransactionFee,

                                                      FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                      JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                      CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                      OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                      MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                      Meal.MealAmt + "~" + Meal.Seg),
                                                      Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                      Baggage.BaggAmt + "~" + Baggage.Seg),

                                                      FQT = Flight.AllowFQT,
                                                      Faresid = (indexValue++).ToString(),
                                                      FlightId = FareData.FlightId.ToString(),
                                                      AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                      ORGTERMINAL = Flight.StartTerminal,
                                                      DESTERMINAL = Flight.EndTerminal,
                                                      OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                      NumberOfStops = "",
                                                      Via = Flight.Via,
                                                      RefNo = "",
                                                      ToSell = "",
                                                      CNX = "",
                                                      ARRIVALTIME = "",
                                                      DEPARTURETIME = "",
                                                      AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                      FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                      DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                      ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                      STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                      ViaFlights = "",
                                                      itinRef = string.IsNullOrEmpty(Flight.ItinRef) ? "" : Flight.ItinRef.ToString().Trim(),
                                                      mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                      BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                      MarkUp = FareData.Markup,
                                                      // OLDmarkup=FareData.OLDmarkup,
                                                      ExtraMrkUp = FareData.ExtraMrkUp,
                                                      originaloldmarkup = FareData.originaloldmarkup,
                                                      DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                      PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                      stocktype = _availResponse.FareCheck.Stocktype,
                                                      seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                      airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                      (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                      OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                      TokenBookingKKK = lstrPriceItenary[0].Token,
                                                      FareTypeDescription = Flight.FareTypeDescription,
                                                      Sftax = FareData.Sftax ?? "0",
                                                      SFGST = FareData.SFGST ?? "0",
                                                      Servicefee = FareData.Servicefee ?? "0",
                                                      FareType = FareData.FareType.Split('|')[0],
                                                      Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                      Promocodedesc = Flight.PromoCodeDesc,
                                                      Promocode = Flight.PromoCode
                                                  };
                                ldtFare = Serv.ConvertToDataTable(FareFlights);
                                //ldtFare = ConvertToDataTable(FareFlights);
                                #endregion
                            }
                            else
                            {
                                #region individual meal and Baggage

                                var FareFlights = from Flight in _availResponse.Flights.AsEnumerable()
                                                  join FareData in GroupFares.AsEnumerable()
                                                  //on Flight.FareId.Replace(Flight.FareId, "") equals FareData.FlightId.Replace(FareData.FlightId, "")
                                                  on Flight.FareId equals FareData.FlightId
                                                  join SsrMealData in Mealdata.AsEnumerable()
                                                  on Flight.SegRef equals SsrMealData.SegRef into SSRMeal
                                                  join SsrBaggData in Baggdata.AsEnumerable()
                                                 on Flight.SegRef equals SsrBaggData.SegRef into SSRBagg
                                                  from Meal in SSRMeal.DefaultIfEmpty()
                                                  from Baggage in SSRBagg.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      AIRLINECATEGORY = Flight.AirlineCategory,//1
                                                      DepartureDate = Flight.DepartureDateTime.ToString().Trim(),
                                                      ArrivalDate = Flight.ArrivalDateTime.ToString().Trim(),//4
                                                      CNXType = Flight.CNX.ToString().Trim(),
                                                      Class = Flight.Class,
                                                      refund = Flight.Refundable,
                                                      Cabin = string.IsNullOrEmpty(Flight.Cabin) ? "" : Flight.Cabin.ToString().Trim(),
                                                      Destination = string.IsNullOrEmpty(Flight.Destination) ? "" : Flight.Destination.ToString().Trim(),
                                                      FAREBASISCODE = string.IsNullOrEmpty(Flight.FareBasisCode) ? "" : Flight.FareBasisCode.ToString().Trim(),
                                                      FlightNumber = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim(),
                                                      CarrierCode = string.IsNullOrEmpty(Flight.FlightNumber) ? "" : Flight.FlightNumber.ToString().Trim().Substring(0, 2),
                                                      FlyingTimeRef = string.IsNullOrEmpty(Flight.FlyingTime) ? "" : Flight.FlyingTime.ToString().Trim(),
                                                      JourneyTimeRef = string.IsNullOrEmpty(Flight.JourneyTime) ? "" : Flight.JourneyTime.ToString().Trim(),
                                                      Origin = string.IsNullOrEmpty(Flight.Origin) ? "" : Flight.Origin.ToString().Trim(),
                                                      PlatingCarrier = string.IsNullOrEmpty(Flight.PlatingCarrier) ? "" : Flight.PlatingCarrier.ToString().Trim(),
                                                      Token = string.IsNullOrEmpty(Flight.ReferenceToken) ? "" : Flight.ReferenceToken.ToString().Trim(),
                                                      SegRef = string.IsNullOrEmpty(Flight.SegRef) ? "" : Flight.SegRef.ToString().Trim(),
                                                      MULTICLASSAMOUNT = "",
                                                      BaseAmount = FareData.BaseAmount,
                                                      TAXBREAKUP = FareData.BreakUp,
                                                      Commission = FareData.Commission,
                                                      Discount = FareData.Discount,
                                                      GrossAmount = FareData.GrossAmount,
                                                      originaloldgross = FareData.originaloldgross,
                                                      GrossAmountRef = string.IsNullOrEmpty(FareData.GrossAmountRef) ? 0 : ConvertToDecimal(FareData.GrossAmountRef),
                                                      Incentive = FareData.Incentive,
                                                      PaxType = FareData.PaxType,
                                                      ServiceTax = FareData.ServiceTax,
                                                      Servicecharge = FareData.Servicecharge,
                                                      TDS = FareData.TDS,
                                                      TotalTaxAmount = FareData.TotalTaxAmount,
                                                      TransactionFee = FareData.TransactionFee,

                                                      FlyingTime = string.IsNullOrEmpty(Flight.FlyingTime) ? 0 : Convert.ToDouble((Flight.FlyingTime.ToString().Trim())),
                                                      JourneyTime = string.IsNullOrEmpty(Flight.JourneyTime) ? 0 : Convert.ToDouble((Flight.JourneyTime.ToString().Trim())),//p.Field<string>("JourneyTime"),


                                                      CONNECTINGFLAG = Flight.ConnectionFlag.ToString().Trim(),
                                                      OFFLINE = Convert.ToBoolean(Flight.OfflineIndicator.ToString().Trim()) == false ? "0" : "1",
                                                      MEAL = Meal == null ? "" : (Meal.MealCode + "~" + Meal.MealDesc + "~" +
                                                      Meal.MealAmt + "~" + Meal.Seg),
                                                      Baggage = Baggage == null ? "" : (Baggage.BaggCode + "~" + Baggage.BaggDesc + "~" +
                                                      Baggage.BaggAmt + "~" + Baggage.Seg),

                                                      FQT = Flight.AllowFQT,
                                                      Faresid = (indexValue++).ToString(),
                                                      FlightId = FareData.FlightId.ToString(),
                                                      //AIRCRAFTTYPE = Flight.SegmentDetails.ToString().Trim(),
                                                      AIRCRAFTTYPE = GroupByRow(_availResponse.Flights, _availResponse.Flights.IndexOf(Flight), "via"),// Flight.SegmentDetails,//17
                                                      ORGTERMINAL = Flight.StartTerminal,
                                                      DESTERMINAL = Flight.EndTerminal,
                                                      OFFLINEFLAG = Flight.OfflineFlag.ToString().Trim(),
                                                      NumberOfStops = "",
                                                      Via = Flight.Via,
                                                      RefNo = "",
                                                      ToSell = "",
                                                      CNX = "",
                                                      ARRIVALTIME = "",
                                                      DEPARTURETIME = "",
                                                      AIRLINENAME = Utilities.AirlineName(Flight.FlightNumber.ToString().Trim().Substring(0, 2)),
                                                      FLIGHT = Flight.FlightNumber.ToString().Trim(),
                                                      DEPARTURE = Flight.DepartureDateTime.ToString().Trim(),
                                                      ARRIVAL = Flight.ArrivalDateTime.ToString().Trim(),
                                                      STOPS = Flight.Origin.ToString().Trim() + " --> " + Flight.Destination.ToString().Trim(),
                                                      ViaFlights = "",
                                                      itinRef = string.IsNullOrEmpty(Flight.ItinRef) ? "" : Flight.ItinRef.ToString().Trim(),
                                                      mealAvail = _availResponse.Meal == null || _availResponse.Meal.Count == 0 ? false : true,
                                                      BaggAvail = _availResponse.Bagg == null || _availResponse.Bagg.Count == 0 ? false : true,
                                                      MarkUp = FareData.Markup,
                                                      // OLDmarkup=FareData.OLDmarkup,
                                                      ExtraMrkUp = FareData.ExtraMrkUp,
                                                      originaloldmarkup = FareData.originaloldmarkup,
                                                      DOBMAND = string.IsNullOrEmpty(_PriceResponse.DOBMandatory) ? "TRUE" : _PriceResponse.DOBMandatory.ToUpper(),
                                                      PASSMAND = string.IsNullOrEmpty(_PriceResponse.PassportMandatory) ? "TRUE" : _PriceResponse.PassportMandatory.ToUpper(),
                                                      stocktype = _availResponse.FareCheck.Stocktype,
                                                      seatavail = string.IsNullOrEmpty(_PriceResponse.AllowSeatMap) ? "FALSE" : _PriceResponse.AllowSeatMap.ToUpper(),
                                                      airinsavail = _PriceResponse.Bargainflag == null || _PriceResponse.Bargainflag.Trim() == "" || _PriceResponse.Bargainflag.Split('|').Length < 4 ? "FALSE" :
                                                      (string.IsNullOrEmpty(_PriceResponse.Bargainflag.ToString().Split('|')[4]) ? "FALSE" : "TRUE"),
                                                      OtherSSR = (_availResponse.OtherService == null || _availResponse.OtherService.Count() == 0) ? "FALSE" : "TRUE",
                                                      TokenBookingKKK = lstrPriceItenary[0].Token,
                                                      FareTypeDescription = Flight.FareTypeDescription,
                                                      Sftax = FareData.Sftax ?? "0",
                                                      SFGST = FareData.SFGST ?? "0",
                                                      Servicefee = FareData.Servicefee ?? "0",
                                                      FareType = FareData.FareType.Split('|')[0],
                                                      Operatincarrier = string.IsNullOrEmpty(Flight.OperatingCarrier) ? "" : Flight.OperatingCarrier.ToString().Trim(),
                                                      Promocodedesc = Flight.PromoCodeDesc,
                                                      Promocode = Flight.PromoCode
                                                  };
                                ldtFare = Serv.ConvertToDataTable(FareFlights);
                                // ldtFare = ConvertToDataTable(FareFlights);

                                #endregion
                            }

                            #endregion
                        }
                        dttemp.BeginLoadData();
                        dttemp.Merge(ldtFare.Copy());
                        dttemp.AcceptChanges();
                        dttemp.EndLoadData();

                        dtbookreq = dttemp.Copy();
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    strErrtemp = ex.ToString();
                    return; // null;
                }

            }

            public static string CombineAmount(string Adtamount, string ChdAmount, string InfAmount)
            {
                string TAXBKPA = Adtamount;
                string TAXBKPC = ChdAmount;
                string TAXBKPI = InfAmount;
                double Amount = 0;
                string Result = "";
                try
                {
                    string INput = Adtamount;
                    #region Adtbreakup

                    if (!TAXBKPA.Contains("KKK"))
                    {
                        INput = Adtamount;

                        Amount = 0;
                        foreach (string item in INput.Split('/'))
                        {
                            Amount += (Convert.ToDouble((string.IsNullOrEmpty(item.Trim()) ? "0" : item)));

                        }


                        Result = Amount.ToString();
                    }
                    #endregion
                    #region Chdbreakup

                    if (!TAXBKPC.Contains("KKK"))
                    {
                        INput = ChdAmount;

                        Amount = 0;
                        foreach (string item in INput.Split('/'))
                        {
                            Amount += (Convert.ToDouble((string.IsNullOrEmpty(item.Trim()) ? "0" : item)));

                        }


                        Result += "|" + Amount;
                    }
                    #endregion
                    #region InfBreakup

                    if ((!TAXBKPI.Contains("KKK")))
                    {
                        INput = InfAmount;

                        Amount = 0;
                        foreach (string item in INput.Split('/'))
                        {
                            Amount += (Convert.ToDouble((string.IsNullOrEmpty(item.Trim()) ? "0" : item)));

                        }
                        Result += "|" + Amount;
                    }
                    #endregion
                    return Result;
                }
                catch (Exception ex)
                {
                    return TAXBKPA + "|" + TAXBKPC + "|" + TAXBKPI;
                }
            }

            public static string CombineNewtax(string Brackup, string CHdBrackup, string InBrackup)
            {
                string TAXBKPA = Brackup;
                string TAXBKPC = CHdBrackup;
                string TAXBKPI = InBrackup;
                try
                {
                    string Result = "";
                    #region AdtBreakup

                    string INput = Brackup;

                    Hashtable hshList = new Hashtable();
                    foreach (string item in INput.Split('/'))
                    {
                        if (item.Split(':').Count() != 2 || string.IsNullOrEmpty(item.Split(':')[0]) || string.IsNullOrEmpty(item.Split(':')[1]) || item.Split(':')[1].ToString().Equals("0"))
                            continue;

                        string Amount = string.Empty;
                        if (hshList.ContainsKey(item.Split(':')[0]))
                        {
                            Amount = (Convert.ToDouble((item.Split(':')[1])) + Convert.ToDouble(hshList[item.Split(':')[0]])).ToString();
                            hshList[item.Split(':')[0]] = Amount;
                        }
                        else
                            hshList[item.Split(':')[0]] = item.Split(':')[1];
                    }
                    Brackup = string.Join("/", hshList.Keys.Cast<object>()
                                             .Select(x => x.ToString() + ":" + hshList[x].ToString())
                                             .ToArray());
                    if (string.IsNullOrEmpty(Brackup))
                        Brackup = TAXBKPA;

                    Result = Brackup;

                    #endregion
                    #region Chdbreakup

                    if (!TAXBKPC.Contains("KKK"))
                    {
                        INput = CHdBrackup;

                        hshList = new Hashtable();
                        foreach (string item in INput.Split('/'))
                        {
                            if (item.Split(':').Count() != 2 || string.IsNullOrEmpty(item.Split(':')[0]) || string.IsNullOrEmpty(item.Split(':')[1]) || item.Split(':')[1].ToString().Equals("0"))
                                continue;

                            string Amount = string.Empty;
                            if (hshList.ContainsKey(item.Split(':')[0]))
                            {
                                Amount = (Convert.ToDouble((item.Split(':')[1])) + Convert.ToDouble(hshList[item.Split(':')[0]])).ToString();
                                hshList[item.Split(':')[0]] = Amount;
                            }
                            else
                                hshList[item.Split(':')[0]] = item.Split(':')[1];
                        }
                        CHdBrackup = string.Join("/", hshList.Keys.Cast<object>()
                                                 .Select(x => x.ToString() + ":" + hshList[x].ToString())
                                                 .ToArray());
                        if (string.IsNullOrEmpty(CHdBrackup))
                            CHdBrackup = TAXBKPC;

                        Result += "|" + CHdBrackup;
                    }
                    #endregion
                    #region InfBreakup

                    if (!TAXBKPI.Contains("KKK"))
                    {
                        INput = InBrackup;

                        hshList = new Hashtable();
                        foreach (string item in INput.Split('/'))
                        {
                            if (item.Split(':').Count() != 2 || string.IsNullOrEmpty(item.Split(':')[0]) || string.IsNullOrEmpty(item.Split(':')[1]) || item.Split(':')[1].ToString().Equals("0"))
                                continue;

                            string Amount = string.Empty;
                            if (hshList.ContainsKey(item.Split(':')[0]))
                            {
                                Amount = (Convert.ToDouble((item.Split(':')[1])) + Convert.ToDouble(hshList[item.Split(':')[0]])).ToString();
                                hshList[item.Split(':')[0]] = Amount;
                            }
                            else
                                hshList[item.Split(':')[0]] = item.Split(':')[1];
                        }
                        InBrackup = string.Join("/", hshList.Keys.Cast<object>()
                                                 .Select(x => x.ToString() + ":" + hshList[x].ToString())
                                                 .ToArray());
                        if (string.IsNullOrEmpty(InBrackup))
                            InBrackup = TAXBKPI;

                        Result += "|" + InBrackup;
                    }
                    #endregion
                    return Result;
                }
                catch (Exception ex)
                {
                    return TAXBKPA + "|" + TAXBKPC + "|" + TAXBKPI;
                }
            }

            //public static string CombineTaxBreakUp(string Brackup)//--?
            //{
            //    try
            //    {
            //        string newbreakup = "";
            //        string TAXBKP = Brackup;
            //        for (int indexmine = 0; indexmine < Brackup.Split('|').Length; indexmine++)
            //        {
            //            string INput = Brackup.Split('|')[indexmine];
            //            Hashtable hshList = new Hashtable();
            //            foreach (string item in INput.Split('/'))
            //            {
            //                if (item.Split(':').Count() != 2 || string.IsNullOrEmpty(item.Split(':')[0]) || string.IsNullOrEmpty(item.Split(':')[1]) || item.Split(':')[1].ToString().Equals("0"))
            //                    continue;

            //                string Amount = string.Empty;
            //                if (hshList.ContainsKey(item.Split(':')[0]))
            //                {
            //                    Amount = (Convert.ToDouble((item.Split(':')[1])) + Convert.ToDouble(hshList[item.Split(':')[0]])).ToString();
            //                    hshList[item.Split(':')[0]] = Amount;
            //                }
            //                else
            //                {
            //                    hshList.Add(item.Split(':')[0], item.Split(':')[1]);
            //                }
            //                // hshList[item.Split(':')[0]] = item.Split(':')[1];
            //            }
            //            newbreakup += string.Join("/", hshList.Keys.Cast<object>()
            //                                     .Select(x => x.ToString() + ":" + hshList[x].ToString())
            //                                     .ToArray()) + "|";

            //        }


            //        Brackup = (string.IsNullOrEmpty(newbreakup.Trim().TrimEnd('|'))) ? TAXBKP : newbreakup.Trim().TrimEnd('|');
            //        return Brackup;
            //    }
            //    catch (Exception ex)
            //    {

            //        return Brackup;
            //    }
            //}

            public static Decimal ConvertToDecimal(string Data)
            {
                if (Data != "")
                {
                    CultureInfo invariant = CultureInfo.InvariantCulture;
                    return ConfigurationManager.AppSettings["ToDecimal"].ToString() == "Y" ?
                        Convert.ToDecimal(Convert.ToDecimal(Data, invariant).ToString("0.00", invariant), invariant) :
                        Convert.ToDecimal(Convert.ToDecimal(Data, invariant).ToString("0", invariant), invariant);
                }
                else
                {
                    return ConfigurationManager.AppSettings["ToDecimal"].ToString() == "Y" ? Convert.ToDecimal("0.00") : Convert.ToDecimal("0");
                }
            }

            public static double CovertToDouble(string value)
            {
                double result = 0;
                CultureInfo invariant = CultureInfo.InvariantCulture;
                result = string.IsNullOrEmpty(value) ? 0 : Convert.ToDouble(value, invariant);
                return result;
            }

            private static string GroupByRow(List<RQRS.Flights> _Flights, int lindex, string ColumnName)
            {
                try
                {
                    int ind = lindex;
                    if (_Flights[lindex].CNX == "Y")
                    {
                        return "";
                    }
                    else if (_Flights[lindex].CNX == "N")
                    {
                        if (ColumnName == "Duration")
                        {
                            string lstr = string.Empty;
                            do
                            {
                                lstr = _Flights[lindex].Origin + " -->" + lstr;
                                lindex--;
                            } while (lindex >= 0 && (_Flights.Count > lindex ? _Flights[lindex].CNX.Trim() == "Y" : true));

                            lindex = (lindex < 0) ? 0 : lindex;
                            string lstrDest = _Flights[lindex].Destination.ToString().Trim();
                            if (_Flights[lindex].CNX.Trim() == "Y")
                            {
                                for (int inde = lindex; inde < _Flights.Count; inde++)
                                {
                                    if (_Flights[inde].CNX.Trim() == "N")
                                    {
                                        lstrDest = _Flights[inde].Destination.ToString().Trim();
                                        lindex = inde;
                                        break;
                                    }
                                }
                            }
                            lstr = (lstr + lstrDest);
                            lstr = "Number of Stops : 0" + "\n" + lstr + "\n" + (_Flights[lindex].JourneyTime != null ? totalJourney(_Flights[lindex].JourneyTime.Trim()) : "");

                            return lstr;
                        }
                        else if (ColumnName == "ArrivalDate")
                        {
                            string lstr = string.Empty;
                            //do
                            //{
                            //    lindex--;
                            //} while (lindex >= 0 && _Flights[lindex].CNX.Trim() == "Y");

                            //lindex = lindex + 1;
                            //lstr = _Flights[lindex].ArrivalDateTime.Substring(0, 11) + "\n" + _Flights[lindex].ArrivalDateTime.Substring(11);
                            lstr = _Flights[lindex].ArrivalDateTime.ToString();
                            return lstr;
                        }
                        else if (ColumnName == "DepartureDate")
                        {
                            string lstr = string.Empty;

                            do
                            {
                                lindex--;
                            } while (lindex >= 0 && _Flights[lindex].CNX.Trim() == "Y");

                            lindex = lindex + 1;
                            lstr = _Flights[lindex].DepartureDateTime.ToString();
                            //lstr = _Flights[lindex].DepartureDateTime.Substring(0, 11) + "\n" + _Flights[lindex].DepartureDateTime.Substring(11);
                            return lstr;
                        }
                        else if (ColumnName == "ArrivalTime")
                        {
                            return _Flights[lindex].ArrivalDateTime.ToString().Split(' ')[3].ToString().TrimEnd('\n').Replace(":", "");
                        }
                        else if (ColumnName == "DepartureTime")
                        {
                            do
                            {
                                lindex--;
                            } while (lindex >= 0 && _Flights[lindex].CNX.Trim() == "Y");

                            lindex = lindex + 1;
                            return _Flights[lindex].DepartureDateTime.ToString().Split(' ')[3].ToString().TrimEnd('\n').Replace(":", "");
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception)
                {
                }
                return "";
            }

            public static string totalJourney(string time1)
            {
                try
                {
                    if (string.IsNullOrEmpty(time1) ||
                       time1 == "0")
                        return string.Empty;
                    int hrs = 0;
                    int min = 0;
                    int Mins = 0;
                    int.TryParse(time1, out Mins);
                    hrs = Mins / 60;
                    min = Mins % 60;
                    string totalJourneyHours = string.Empty;
                    TimeSpan tpJouyHours = new TimeSpan(hrs, min, 0);
                    TimeSpan FlightTime = new TimeSpan();
                    if (!TimeSpan.TryParse(time1, out FlightTime))
                    {
                        // The format of pairing.FlightTime.Insert(2, ":") is invalid
                        // Maybe you want to print it to know why
                    }
                    totalJourneyHours = (tpJouyHours.Days == 0 ? "" : tpJouyHours.Days + "day ") + tpJouyHours.Hours + "hrs  " + tpJouyHours.Minutes + "min";
                    return totalJourneyHours;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            private static decimal calculateTransGrossMclass(string gross, string trans, string service, string transfee, string Sfamt, string sfgst, string Servfee)
            {
                decimal AMt = 0;
                AMt = ConvertToDecimal(gross.Split('|')[0].ToString()) +
                    ConvertToDecimal(trans.Split('|')[0].ToString()) +
                    ConvertToDecimal(service.Split('|')[0].ToString()) +
                    ConvertToDecimal(transfee.Split('|')[0].ToString()) +
                    ConvertToDecimal(Sfamt.Split('|')[0].ToString()) +
                    ConvertToDecimal(sfgst.Split('|')[0].ToString()) +
                    ConvertToDecimal(Servfee.Split('|')[0].ToString());
                return AMt;
            }

            private static decimal calculateServiceGrossMclass(string gross, string Service, string transfee, string Sfamt, string sfgst, string Servfee)
            {
                decimal AMt = 0;
                AMt = ConvertToDecimal(gross.Split('|')[0].ToString()) +
                    ConvertToDecimal(Service.Split('|')[0].ToString()) +
                    ConvertToDecimal(transfee.Split('|')[0].ToString()) +
                    ConvertToDecimal(Sfamt.Split('|')[0].ToString()) +
                    ConvertToDecimal(sfgst.Split('|')[0].ToString()) +
                    ConvertToDecimal(Servfee.Split('|')[0].ToString());
                return AMt;
            }

            #region GroupByProcess for linqQuery..........

            public static string GroupByProcessOneCol(string[] pstr_Columns, string pstrAction)
            {
                string strValue = string.Empty;
                try
                {
                    if (pstrAction.Equals("ANYONE"))
                    {
                        strValue = pstr_Columns[0];
                    }
                    else if (pstrAction.StartsWith("JOIN"))
                    {
                        pstrAction = pstrAction.Replace("JOIN-", "");
                        strValue = string.Join(pstrAction, pstr_Columns);
                    }
                    else if (pstrAction.StartsWith("UNIQUE"))
                    {
                        pstrAction = pstrAction.Replace("UNIQUE-", "");
                        strValue = string.Join(pstrAction, (pstr_Columns.Distinct()).ToArray());

                    }
                    else if (pstrAction.StartsWith("MATRIX"))
                    {
                        List<Array> lstReturnValue = new List<Array>();
                        for (int i = 0; i < pstr_Columns.Length; i++)
                        {
                            lstReturnValue.Add(pstr_Columns[i].Split('|'));
                        }

                        ArrayList arrData = new ArrayList();

                        return (strValue.TrimEnd('}')).TrimEnd(']');
                    }
                    return strValue;
                }
                catch (Exception)
                {
                    //Database.LogData(
                    //"X", "Baseclass", "GroupByProcessOneCol", ex.ToString(),"");
                    return strValue;
                }
            }

            #endregion

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

            public DataTable getMultiClass(multiclass.MultiClass_RS _MultiClass, string FlightId)
            {
                try
                {
                    var _Fare = from Combine in
                                    (from Flighdata in _MultiClass.FlightDetails.AsEnumerable()
                                     join FareData in _MultiClass.FareDetails.AsEnumerable()
                                     on Flighdata.FareID equals FareData.FlightID
                                     from FareDesc in FareData.Faredescription.AsEnumerable().Where(e => e.PaxType.Equals("ADT")).AsEnumerable()
                                     select new
                                     {
                                         FlightNum = Flighdata.FlightNumber + "\n" + Utilities.AirlineName(Flighdata.FlightNumber.Substring(0, 2)),
                                         Class = Flighdata.Class,// + "-" + Flighdata.FareBasisCode

                                         Orgin = Flighdata.Origin,
                                         Destinations = Flighdata.Destination,
                                         Depttime = Flighdata.DepartureDateTime,
                                         ArrTime = Flighdata.ArrivalDateTime,
                                         PlatingCarrier = Flighdata.PlatingCarrier,
                                         AirlineCategory = Flighdata.AirlineCategory,
                                         AirlineCodeflightnum = Flighdata.CarrierCode + " " + (Flighdata.FlightNumber).Split(' ').Last(),
                                         ItinRef = Flighdata.ItinRef,
                                         Stocks = Flighdata.AirlineCategory,

                                         Token = Flighdata.ReferenceToken,
                                         BasicFare = ConvertToDecimal(FareDesc.BaseAmount),//8
                                         BasicFareComm = ConvertToDecimal(FareDesc.BaseAmount) + "\nComm : " + CovertToDouble((FareDesc.Discount ?? "0") + (FareDesc.Incentive ?? "0") + (FareDesc.PLB ?? "0")),//9
                                         GrossAmount = calculateServiceGrossMclass(FareDesc.GrossAmount, (FareDesc.Servicecharge ?? "0"), FareDesc.TransactionFee ?? "0", FareDesc.SFAMOUNT ?? "0", FareDesc.SFGST ?? "0", FareDesc.ServiceFee ?? "0"),//11
                                         GrossAmountTrans = calculateTransGrossMclass(FareDesc.GrossAmount, FareDesc.TransactionFee ?? "0", FareDesc.Servicecharge ?? "0", FareDesc.ClientMarkup ?? "0", FareDesc.SFAMOUNT ?? "0", FareDesc.SFGST ?? "0", FareDesc.ServiceFee ?? "0"),//12
                                         GrossAmountTranssel = FareDesc.GrossAmount ?? "0",
                                         TaxAmount = FareDesc.TotalTaxAmount ?? "0",
                                         TransactionFee = FareDesc.TransactionFee ?? "0",// FareDesc.Markup,// FareDesc.TransactionFee ?? "0",
                                         ServiceTax = FareDesc.ServiceTax ?? "0",
                                         TDS = FareDesc.TDS ?? "0",
                                         ServiceCharge = FareDesc.Servicecharge ?? "0",
                                         PaxType = FareDesc.PaxType,
                                         Commision = (CovertToDouble((FareDesc.Discount ?? "0")) + CovertToDouble((FareDesc.Incentive ?? "0")) + CovertToDouble((FareDesc.PLB ?? "0"))), //+ (FareDesc.pl ?? "0"),
                                         Discount = FareDesc.Commission ?? "0",
                                         Incentive = "0",
                                         BreakUp = String.Join("/", FareDesc.Taxes.Select(TaxData => TaxData.Code.ToString() + ":" + TaxData.Amount).ToArray()),
                                         ClientMarkup = FareDesc.ClientMarkup ?? "0",// FareDesc.Markup,
                                         Aircraft = "",//
                                         Farebasiscode = Flighdata.FareBasisCode,
                                         Baggage = Flighdata.Baggage ?? "",
                                         Meals = Flighdata.Meals ?? "",
                                         Refundable = Flighdata.Refundable ?? "",
                                         Stops = Flighdata.Stops ?? "",
                                         StartTerminal = Flighdata.StartTerminal ?? "",
                                         EndTerminal = Flighdata.EndTerminal ?? "",
                                         //Class=Flighdata.Class,
                                         Otherbenfit = (string.IsNullOrEmpty(Flighdata.Otherbenfit) ? "N/A" : Flighdata.Otherbenfit),
                                         IncentiveGrosFare = (FareDesc.Incentive ?? "0") == "0" ? "" : (calculateServiceGrossMclass(FareDesc.GrossAmount,
                                        (FareDesc.Servicecharge == null || FareDesc.Servicecharge.ToString().Trim() == "" ? "0"
                                        : FareDesc.Servicecharge), "0", (FareDesc.SFAMOUNT ?? "0"), (FareDesc.SFGST ?? "0"), FareDesc.ServiceFee ?? "0") - ConvertToDecimal(FareDesc.Incentive.Split('|')[0].ToString())).ToString(),//12,

                                         IncentiveGrosFareTrans = FareDesc.Incentive == "0" ? "" : (calculateTransGrossMclass(FareDesc.GrossAmount, FareDesc.TransactionFee ?? "0",
                                         (FareDesc.Servicecharge == null || FareDesc.Servicecharge.ToString().Trim() == "" ? "0" :
                                         FareDesc.Servicecharge), FareDesc.ClientMarkup ?? "0", (FareDesc.SFAMOUNT ?? "0"), (FareDesc.SFGST ?? "0"), FareDesc.ServiceFee ?? "0") - ConvertToDecimal(FareDesc.Incentive.Split('|')[0].ToString())).ToString(),//13
                                         Sftax = FareDesc.SFAMOUNT ?? "0",
                                         Sfgst = FareDesc.SFGST ?? "0",
                                         Servfee = FareDesc.ServiceFee ?? "0"
                                     }).AsEnumerable()
                                group Combine by new
                                {
                                    Id = Combine.FlightNum
                                }
                                    into FareGrp
                                select new
                                {
                                    FlightNum = FareGrp.Select(Fares => Fares.FlightNum.ToString()).ToArray()[0],
                                    FlightId = FlightId,
                                    Class = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Class.ToString()).ToArray(), "JOIN-}"),

                                    Orgin = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Orgin.ToString()).ToArray(), "JOIN-}"),
                                    Destinations = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Destinations.ToString()).ToArray(), "JOIN-}"),
                                    Depttime = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Depttime.ToString()).ToArray(), "JOIN-}"),
                                    ArrTime = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.ArrTime.ToString()).ToArray(), "JOIN-}"),
                                    PlatingCarrier = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.PlatingCarrier.ToString()).ToArray(), "JOIN-}"),
                                    AirlineCategory = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.AirlineCategory.ToString()).ToArray(), "JOIN-}"),
                                    AirlineCodeflightnum = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.AirlineCodeflightnum.ToString()).ToArray(), "JOIN-}"),
                                    ItinRef = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.ItinRef.ToString()).ToArray(), "JOIN-}"),
                                    Stocks = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Stocks.ToString()).ToArray(), "JOIN-}"),

                                    Token = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Token.ToString()).ToArray(), "JOIN-TOKENSPILIT"),
                                    BasicFare = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.BasicFare.ToString()).ToArray(), "JOIN-}"),
                                    BasicFareComm = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.BasicFareComm.ToString()).ToArray(), "JOIN-}"),
                                    GrossAmount = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.GrossAmount.ToString()).ToArray(), "JOIN-}"),
                                    GrossAmountTrans = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.GrossAmountTrans.ToString()).ToArray(), "JOIN-}"),
                                    GrossAmountTranssel = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.GrossAmountTranssel.ToString()).ToArray(), "JOIN-}"),
                                    TaxAmount = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.TaxAmount.ToString()).ToArray(), "JOIN-}"),
                                    TransactionFee = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.TransactionFee.ToString()).ToArray(), "JOIN-}"),
                                    ServiceTax = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.ServiceTax.ToString()).ToArray(), "JOIN-}"),
                                    TDS = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.TDS.ToString()).ToArray(), "JOIN-}"),
                                    Farebasecode = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Farebasiscode.ToString()).ToArray(), "JOIN-}"),
                                    ServiceCharge = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.ServiceCharge.ToString()).ToArray(), "JOIN-}"),
                                    PaxType = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.PaxType.ToString()).ToArray(), "ANYONE"),
                                    Commision = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Commision.ToString()).ToArray(), "JOIN-}"),
                                    Discount = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Discount.ToString()).ToArray(), "JOIN-}"),
                                    Incentive = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Incentive.ToString()).ToArray(), "JOIN-}"),
                                    BreakUp = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.BreakUp.ToString()).ToArray(), "JOIN-}"),
                                    ClientMarkup = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.ClientMarkup.ToString()).ToArray(), "JOIN-}"),
                                    Aircraft = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Aircraft.ToString()).ToArray(), "JOIN-}"),
                                    Baggage = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Baggage.ToString()).ToArray(), "JOIN-}"),
                                    Meals = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Meals.ToString()).ToArray(), "JOIN-}"),
                                    Refundable = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Refundable.ToString()).ToArray(), "JOIN-}"),
                                    Stops = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Stops.ToString()).ToArray(), "JOIN-}"),
                                    Otherbenfit = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Otherbenfit.ToString()).ToArray(), "JOIN-}"),
                                    StartTerminal = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.StartTerminal.ToString()).ToArray(), "JOIN-}"),
                                    EndTerminal = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.EndTerminal.ToString()).ToArray(), "JOIN-}"),
                                    //IncentiveGrosFareTrans = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.IncentiveGrosFareTrans.ToString()).ToArray(), "JOIN-}"),
                                    IncentiveGrosFare = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.IncentiveGrosFare.ToString()).ToArray(), "JOIN-}"),
                                    Sftax = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Sftax.ToString()).ToArray(), "JOIN-}"),
                                    Sfgst = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Sfgst.ToString()).ToArray(), "JOIN-}"),
                                    Servfee = GroupByProcessOneCol(FareGrp.Select(Fares => Fares.Servfee.ToString()).ToArray(), "JOIN-}"),
                                };

                    return ConvertToDataTable(_Fare);
                    //return null;
                }
                catch (Exception ex)
                {
                    DatabaseLog.foldererrorlog(ex.ToString(), "getmulticlass", "Base");
                    //LogData("X", "Version.cs", "getmulticlass", ex.ToString());
                    return null;
                }
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
        }

        public static string GetComputer_IP()
        {
            string IPAddress = string.Empty;

            String strHostName = HttpContext.Current.Request.UserHostAddress.ToString();

            IPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();

            string sIPAddress = null;

            sIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(sIPAddress))
                sIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];


            return IPAddress;
        }

        public class Utilities
        {
            public static Hashtable AirlineCode = new Hashtable();
            public static string AirportcityName(string Citycode)
            {
                try
                {
                    if (AirlineCode.ContainsKey(Citycode))
                    {
                        string foo = AirlineCode[Citycode].ToString();
                        if (ConfigurationManager.AppSettings["APP_HOSTING"].ToString() == "BSA")
                            return foo.Split('-')[0];
                        else
                            return foo;
                    }
                    else
                    {
                        DataSet dsAirways = new DataSet();
                        dsAirways.ReadXml(System.Web.Hosting.HostingEnvironment.MapPath("~/XML/CityAirport_Lst.xml").ToString());
                        string airwaysName = "";
                        var qryAirlineName = from p in dsAirways.Tables["AIR"].AsEnumerable()
                                             where p.Field<string>("ID") == Citycode
                                             select p;
                        DataView dvAirlineCode = qryAirlineName.AsDataView();
                        if (dvAirlineCode.Count == 0)
                            airwaysName = Citycode;
                        else
                        {
                            DataTable dtAilineCode = new DataTable();
                            dtAilineCode = qryAirlineName.CopyToDataTable();
                            airwaysName = dtAilineCode.Rows[0]["AN"].ToString().Split('-')[0];
                            AirlineCode.Add(Citycode, airwaysName);
                        }
                        if (ConfigurationManager.AppSettings["APP_HOSTING"].ToString() == "BSA")
                            return airwaysName.Split('-')[0];
                        else
                            return airwaysName;
                    }
                }
                catch (Exception ex)
                {
                    return Citycode;
                }
            }

            public static Hashtable Airlinename = new Hashtable();
            public static string AirlineName(string Airlinecode)
            {
                try
                {
                    if (Airlinename.ContainsKey(Airlinecode))
                    {
                        var foo = Airlinename[Airlinecode];
                        return foo.ToString();
                    }
                    else
                    {
                        DataSet dsAirways = new DataSet();
                        dsAirways.ReadXml(System.Web.Hosting.HostingEnvironment.MapPath("~/XML/AirlineNames.xml").ToString());
                        string airwaysName = "";
                        var qryAirlineName = from p in dsAirways.Tables
                                           ["AIRLINEDET"].AsEnumerable()
                                             where p.Field<string>
                                           ("_CODE") == Airlinecode
                                             select p;
                        DataView dvAirlineCode = qryAirlineName.AsDataView();
                        if (dvAirlineCode.Count == 0)
                            airwaysName = Airlinecode;
                        else
                        {
                            DataTable dtAilineCode = new DataTable();
                            dtAilineCode = qryAirlineName.CopyToDataTable();
                            airwaysName = dtAilineCode.Rows[0]["AIRLINEDET"].ToString();
                            Airlinename.Add(Airlinecode, airwaysName);
                        }
                        return airwaysName;
                    }
                }
                catch (Exception ex)
                {
                    return Airlinecode;
                }
            }

            public static string Getdayvaluebydate(string Datevalue)
            {
                string returndate = string.Empty;
                try
                {
                    if (ConfigurationManager.AppSettings["APP_HOSTING"].ToString() == "BSA")
                    {
                        string deptDay = Convert.ToDateTime(Datevalue.Trim()).DayOfWeek.ToString().Trim();
                        returndate = deptDay + " " + Datevalue;
                    }
                    else
                    {
                        string deptDay = Convert.ToDateTime(Datevalue.Trim()).DayOfWeek.ToString().Trim().Substring(0, 3);
                        returndate = Datevalue + ", " + deptDay;
                    }
                }
                catch (Exception ex)
                {
                    returndate = Datevalue;
                }
                return returndate;
            }

            public static string BookingpageMousehoverinfo(string Flightno, string Origin, string Destination, string Operatingcarrier, string Segmentdetails, string Stock, string Via)//--?
            {
                string strPopup = string.Empty;
                try
                {
                    strPopup += "<table  width='100%'>";
                    strPopup += "<tr><td width='5%'></td><td>Airline Name</td><td>:</td><td>" + AirlineName(Flightno.Split(' ')[0]) + "</td></tr>";
                    strPopup += "<tr><td width='5%'></td><td>Departure Airport</td> <td>:</td><td>" + AirportcityName(Origin) + "</td></tr>";
                    strPopup += "<tr><td width='5%'></td><td>Arrival Airport</td><td>:</td><td>" + AirportcityName(Destination) + "</td></tr>";
                    strPopup += "<tr><td width='5%'></td><td>Operated by</td><td>:</td><td>" + Operatingcarrier + "</td></tr>";
                    strPopup += "<tr><td width='5%'></td><td>Depa.Terminal</td><td>:</td><td>" + (Segmentdetails.Replace("\r", "").Split('\n')[2].Split(':')[1] == "" ? "N/A" : Segmentdetails.Replace("\r", "").Split('\n')[2].Split(':')[1]) + "</td></tr>";
                    strPopup += "<tr><td width='5%'></td><td>Arr.Terminal</td><td>:</td><td>" + (Segmentdetails.Replace("\r", "").Split('\n')[3].Split(':')[1] == "" ? "N/A" : Segmentdetails.Replace("\r", "").Split('\n')[3].Split(':')[1]) + "</td></tr>";
                    strPopup += "<tr><td width='5%'></td><td>Equipment Code</td><td>:</td><td>Airbus " + (Segmentdetails.Replace("\r", "").Split('\n')[0].Split(':')[1] == "" ? "N/A" : Segmentdetails.Replace("\r", "").Split('\n')[0].Split(':')[1]) + "</td></tr>";
                    strPopup += "<tr><td width='5%'></td><td>Baggage</td><td>:</td><td><span id='baggmul'>" + (Segmentdetails.Replace("\r", "").Split('\n')[4].Split(':')[1] == "" ? "N/A" : Segmentdetails.Replace("\r", "").Split('\n')[4].Split(':')[1]) + "</span> </td></tr>";
                    strPopup += "<tr><td width='5%'></td><td>Stock Type </td><td>:</td><td>" + Stock + " </td></tr>";
                    strPopup += "<tr><td width='5%'></td><td>Via </td><td>:</td><td>" + (Via == "" ? "N/A" : Via) + " </td></tr>";
                    strPopup += "</table>";
                }
                catch (Exception ex)
                {
                    strPopup = "";
                }
                return strPopup;
            }

            public static byte[] ConvertDataSetToByteArray(DataSet dataSet)
            {
                Byte[] data;
                MemoryStream mem = new MemoryStream();
                GZipStream zip = new GZipStream(mem, CompressionMode.Compress);
                dataSet.WriteXml(zip, XmlWriteMode.WriteSchema);
                zip.Close();
                data = mem.ToArray();
                mem.Close();
                return data;
            }

            public static string AllowControls(string strcontrolname)
            {
                string displayclass = string.Empty;
                try
                {
                    string strAllowControl = System.Configuration.ConfigurationManager.AppSettings["AllowControls"].ToString();
                    if (strAllowControl.IndexOf(strcontrolname) > -1)
                    {
                        displayclass = "clsenable"; /*Display enable class name*/
                    }
                    else
                    {
                        displayclass = "clsdiable"; /*Display disable class name*/
                    }

                }
                catch (Exception ex)
                {
                    displayclass = "clsdiable"; /*Display disable class name*/
                }
                return displayclass;
            }
        }

        #region UsageLog

        public class UsageDetails
        {
            public string BRANCH { get; set; }
            public string GROUP_ID { get; set; }
            public string TERMINAL_ID { get; set; }
            public string CLIENT_ID { get; set; }
            public string USERNAME { get; set; }
            public string PRODUCT { get; set; }
            public string PLATFORM { get; set; }
            public string UNIQUEID { get; set; }
            public string PAGE_NAME { get; set; }
            public string CREATED_DATE { get; set; }
            public string IP_ADDRESS { get; set; }
            public string SEQUENCE_ID { get; set; }
            public string APP_TYPE { get; set; }
            public string PRODUCT_TYPE { get; set; }
            public string PRODUCT_COUNTRY { get; set; }
            public string ACTION_TYPE { get; set; }
        }

        public static string Commonlog(string PAGENAME, string UNIQUEID, string Action)
        {
            string strResponse = "";
            try
            {
                string StrInput = "";
                UsageDetails _LocationLogDetails = new UsageDetails();

                _LocationLogDetails.PRODUCT = ConfigurationManager.AppSettings["Producttype"] == "RBOA" ? "Riya BOA" : ConfigurationManager.AppSettings["Appname"].ToString().ToUpper();
                _LocationLogDetails.PLATFORM = (HttpContext.Current.Session["TERMINALTYPE"].ToString() == "T" ? "TDK" : "B2B");
                _LocationLogDetails.CLIENT_ID = HttpContext.Current.Session["POS_ID"].ToString();
                _LocationLogDetails.TERMINAL_ID = HttpContext.Current.Session["POS_TID"].ToString();
                _LocationLogDetails.USERNAME = HttpContext.Current.Session["username"].ToString();
                _LocationLogDetails.BRANCH = HttpContext.Current.Session["branchid"].ToString();
                _LocationLogDetails.SEQUENCE_ID = HttpContext.Current.Session["sequenceid"].ToString();
                _LocationLogDetails.GROUP_ID = "";
                _LocationLogDetails.PAGE_NAME = PAGENAME;
                _LocationLogDetails.APP_TYPE = HttpContext.Current.Session["Bookapptype"].ToString();
                _LocationLogDetails.IP_ADDRESS = HttpContext.Current.Session["ipAddress"].ToString();
                _LocationLogDetails.UNIQUEID = UNIQUEID;
                _LocationLogDetails.PRODUCT_COUNTRY = ConfigurationManager.AppSettings["COUNTRY"].ToString();
                _LocationLogDetails.PRODUCT_TYPE = "AIRLINE";
                _LocationLogDetails.ACTION_TYPE = Action;

                StrInput = JsonConvert.SerializeObject(_LocationLogDetails);
                string LogQuery = "Insert_Usage_LogDetails";
                string TIMEOUTMINUTS = ConfigurationManager.AppSettings["AVAIL_TIMEOUT"].ToString();
                string strURLpathA = ConfigurationManager.AppSettings["STaskiesUsageLogURL"] + LogQuery;

                if (ConfigurationManager.AppSettings["STaskiesUsageLogURL"].ToString() != "")
                {
                    MyWebClient Client = new MyWebClient();
                    Client.Headers["Content-Type"] = "application/json";
                    Client.LintTimeout = 5000; // 5 SECONDS
                    byte[] byteGetLogin = Client.UploadData(strURLpathA, "POST", Encoding.ASCII.GetBytes(StrInput));
                    strResponse = Encoding.ASCII.GetString(byteGetLogin);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    string path = ConfigurationManager.AppSettings["Folderlogpath"].ToString();
                    string Folder_name = DateTime.Now.ToString("yyyyMMdd");
                    string Folderpath = path + "\\" + Folder_name;
                    if (!Directory.Exists(Folderpath))
                    {
                        Directory.CreateDirectory(Folderpath);
                    }
                    string filename = "Terminal" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    string Savedpath = Folderpath + "\\" + filename + ".txt";

                    if (!File.Exists(Savedpath))
                    {
                        StreamWriter sw = File.CreateText(Savedpath);

                        sw.WriteLine("DATE :" + DateTime.Now.ToString() + "============================================================================="
                             + "===================================================================="
                             + "=================================================================================================================\r\n");
                        sw.WriteLine((("Exeption :" + ex.ToString())));
                        sw.WriteLine("\r\n============================================================================="
                           + "===================================================================="
                           + "=================================================================================================================");
                        sw.Flush();
                        sw.Close();
                    }
                    else if (File.Exists(Savedpath))
                    {
                        StreamWriter sw = File.AppendText(Savedpath);
                        sw.WriteLine("DATE :" + DateTime.Now.ToString() + "============================================================================="
                             + "===================================================================="
                             + "=================================================================================================================\r\n");
                        sw.WriteLine((("EXEPTION :" + ex.ToString())));
                        sw.WriteLine("\r\n============================================================================="
                           + "===================================================================="
                           + "=================================================================================================================");
                        sw.Flush();
                        sw.Close();
                    }
                }
                catch
                {
                }
            }
            return strResponse;
        }

        #endregion

        public static byte[] ReadBitmap2ByteArray(string fileName)
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

        #region Fetch Country Using Airport ID
        private static Hashtable CountryCodes = new Hashtable();
        public static string GetCountryCode(string strAirportID)
        {
            string strCountryCode = string.Empty;
            try
            {
                if (CountryCodes.ContainsKey(strAirportID))
                {
                    strCountryCode = CountryCodes[strAirportID].ToString();
                }
                else
                {
                    DataSet DsAirports = new DataSet();
                    DsAirports.ReadXml(HttpContext.Current.Server.MapPath("~/XML/CityAirport_Lst.xml").ToString());
                    var strCode = (from _Code in DsAirports.Tables["AIR"].AsEnumerable()
                                   where _Code.Field<string>("ID") == strAirportID
                                   select _Code.Field<string>("CN")).ToList();
                    if (strCode.Count > 0)
                    {
                        strCountryCode = strCode[0].ToString();
                        CountryCodes.Add(strAirportID, strCountryCode);
                    }
                }
            }
            catch (Exception ex)
            {
                strCountryCode = string.Empty;
            }
            return strCountryCode;
        }
        #endregion
    }

    public class B2C_Class
    {
        public class Tokenclass
        {
            public string access_token
            {
                get;
                set;
            }
            public string token_type
            {
                get;
                set;
            }
            public int expires_in
            {
                get;
                set;
            }
            public string refresh_token
            {
                get;
                set;
            }
            public string id_token
            {
                get;
                set;
            }
        }

        public class Userclass
        {
            public string id
            {
                get;
                set;
            }
            public string name
            {
                get;
                set;
            }
            public string given_name
            {
                get;
                set;
            }
            public string family_name
            {
                get;
                set;
            }
            public string link
            {
                get;
                set;
            }
            public string picture
            {
                get;
                set;
            }
            public string gender
            {
                get;
                set;
            }
            public string email
            {
                get;
                set;
            }
            public string verified_email
            {
                get;
                set;
            }
            public string locale
            {
                get;
                set;
            }
        }

        public class Registration
        {
            public string slct_title { get; set; }
            public string txtfname { get; set; }
            public string txtlname { get; set; }
            public string slct_Countrycode { get; set; }
            public string txtmobileNo { get; set; }
            public string txt_EmailID { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string Code { get; set; }
            public string Password { get; set; }
            public string Updatetype { get; set; }
            public string Oldpassword { get; set; }
            public string Newpassword { get; set; }
            public string Flagcheck { get; set; }
            public string passportnum { get; set; }
            public string dobirth { get; set; }
            public string expirydate { get; set; }
            public string issuedcountry { get; set; }
            public string usernamelog { get; set; }
            public string Currency { get; set; }


        }

        public class GoogleSubmitRegistration
        {
            public string ResultCode { get; set; }
            public string Message { get; set; }
        }

        public class GoogleLogincheck
        {
            public string Result { get; set; }
            public string Status { get; set; }
            public string USERNAME { get; set; }
        }

        #region B2C TRAVELLER DETAILS
        public class MyTravellers
        {
            public string PaxType { get; set; }
            public string MailId { get; set; }
            public string ParentTravelerCode { get; set; }
            public string Title { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            public string Dob { get; set; }
            public string MobileNo { get; set; }
            public string Address { get; set; }
            public string PassportNo { get; set; }
            public string IssuedCountry { get; set; }
            public string PassportExpiryDate { get; set; }
            public string PassportImage { get; set; }
            public string Nationality { get; set; }
            public string CreatedBy { get; set; }
            public string UserMailid { get; set; }
            public string Flag { get; set; }

        }
        #endregion
    }

    public class CustomFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string strAgentId = HttpContext.Current.Session["agentid"] != null && HttpContext.Current.Session["agentid"].ToString() != "" ? HttpContext.Current.Session["agentid"].ToString() : "";
            string strUserName = HttpContext.Current.Session["UserName"] != null && HttpContext.Current.Session["UserName"].ToString() != "" ? HttpContext.Current.Session["UserName"].ToString() : "";
            string strIPAddress = HttpContext.Current.Session["ipAddress"] != null && HttpContext.Current.Session["ipAddress"].ToString() != "" ? HttpContext.Current.Session["ipAddress"].ToString() : "";
            string strSequenceId = HttpContext.Current.Session["sequenceid"] != null && HttpContext.Current.Session["sequenceid"].ToString() != "" ? HttpContext.Current.Session["sequenceid"].ToString() : "";
            string strPOS_ID = HttpContext.Current.Session["POS_ID"] != null && HttpContext.Current.Session["POS_ID"].ToString() != "" ? HttpContext.Current.Session["POS_ID"].ToString() : "";
            string strPOS_TID = HttpContext.Current.Session["POS_TID"] != null && HttpContext.Current.Session["POS_TID"].ToString() != "" ? HttpContext.Current.Session["POS_TID"].ToString() : "";
            string strTerminalID = HttpContext.Current.Session["terminalid"] != null && HttpContext.Current.Session["terminalid"].ToString() != "" ? HttpContext.Current.Session["terminalid"].ToString() : "";
            string strTerminalType = HttpContext.Current.Session["TERMINALTYPE"] != null && HttpContext.Current.Session["TERMINALTYPE"].ToString() != "" ? HttpContext.Current.Session["TERMINALTYPE"].ToString() : "";
            if (strAgentId == "" || strUserName == "" || strIPAddress == "" || strTerminalID == "" || strTerminalType == "" || strPOS_ID == "" || strPOS_TID == "")
            {
                HttpContext.Current.Session.Clear();
                filterContext.HttpContext.Response.Redirect("/Redirect/SessionExp", true);
            }
        }
    }

    public class WebconfigValuesLst
    {
        public string Keys { get; set; }
        public string Values { get; set; }
    }

    public class CustomFilterAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string strAdminUserIP = HttpContext.Current.Request.UserHostAddress.ToString();
            if (HttpContext.Current.Session["AdminLogin"] == null || HttpContext.Current.Session["AdminLogin"].ToString() == "")
            {
                HttpContext.Current.Session.Clear();
                filterContext.HttpContext.Response.Redirect("/Redirect/SessionExp", true);
            }
        }
    }

}