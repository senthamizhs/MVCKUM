using System;
using System.Collections.Generic;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using STSTRAVRAYS.Rays_service;
using System.Configuration;
using Newtonsoft.Json;

namespace STSTRAVRAYS.Models
{

    [DataContract]
    public class AgentDetails2
    {
        [JsonProperty(Required = Required.Always)]
        public string AgentId
        {
            get;
            set;
        }
        [JsonProperty(Required = Required.Always)]
        public string TerminalId
        {
            get;
            set;
        }
        [JsonProperty(Required = Required.Always)]
        public string UserName
        {
            get;
            set;
        }
        [JsonProperty(Required = Required.Always)]
        public string AppType
        {
            get;
            set;
        }
        [JsonProperty]
        public string Version
        {
            get;
            set;
        }
        [JsonProperty(Required = Required.Always)]
        public string Environment
        {
            get;
            set;
        }

    }

    [DataContract]
    public class CancellRQ
    {
        [DataMember]
        public AgentDetails2 Agent { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public List<string> RoomPnr { get; set; }
        [DataMember]
        public string Flag { get; set; }
        [DataMember]
        public List<string> PenaltyAmt { get; set; }
    }
    [DataContract]
    public class CancellRS
    {
        [DataMember]
        public string Error { get; set; }
        [DataMember]
        public string ResultCode { get; set; }
        [DataMember]
        public string Seq { get; set; }
        [DataMember]
        public List<string> PenaltyAmt { get; set; }
        [DataMember]
        public string Status { get; set; }
    }




}
namespace Seat_rays
{
    #region GetSeatMap_RQ
    [DataContract]
    public class GetSeatMap_RQ
    {
        [JsonProperty]
        public AgentDetails AgentDetail
        {
            get;
            set;
        }
        [JsonProperty]
        public List<RQFlights> FlightsDetails
        {
            get;
            set;
        }
        //[JsonProperty]
        //public Segment SegmentDetails
        //{
        //    get;
        //    set;
        //}
        [JsonProperty]
        public TripDetails TripDetails { get; set; }
        [JsonProperty]
        public String Platform
        {
            get;
            set;
        }
        [JsonProperty]
        public String Stock
        {
            get;
            set;
        }
        //[JsonProperty]
        //public string PassengerDetails
        //{
        //    get;
        //    set;
        //}
        [JsonProperty]
        public List<ReqPassDetail> PassengerDetails
        {
            get;
            set;
        }


    }



    [DataContract]
    public class Gettuneins_RQ
    {
        [JsonProperty]
        public AgentDetails AgentDetail
        {
            get;
            set;
        }
        [JsonProperty]
        public List<RQFlights> FlightsDetails
        {
            get;
            set;
        }
        //[JsonProperty]
        //public Segment SegmentDetails
        //{
        //    get;
        //    set;
        //}
        [JsonProperty]
        public TripDetails TripDetails { get; set; }
        [JsonProperty]
        public String Platform
        {
            get;
            set;
        }
        [JsonProperty]
        public String Stock
        {
            get;
            set;
        }
        //[JsonProperty]
        //public string PassengerDetails
        //{
        //    get;
        //    set;
        //}
        [JsonProperty]
        public List<TuneReqPassDetail> PassengerDetails
        {
            get;
            set;
        }

    }
    #endregion


    [DataContract]
    public class ReqPassDetail
    {
        [JsonProperty]
        public string Title
        {
            get;
            set;
        }
        [JsonProperty]
        public string Firstname
        {
            get;
            set;
        }
        [JsonProperty]
        public string Lastname
        {
            get;
            set;
        }
        [JsonProperty]
        public string PaxType
        {
            get;
            set;
        }
        [JsonProperty]
        public string PAXTYPENO
        {
            get;
            set;
        }

    }

    [DataContract]
    public class TuneReqPassDetail
    {
        [JsonProperty]
        public string Title
        {
            get;
            set;
        }
        [JsonProperty]
        public string Firstname
        {
            get;
            set;
        }
        [JsonProperty]
        public string Lastname
        {
            get;
            set;
        }
        [JsonProperty]
        public string PaxType
        {
            get;
            set;
        }
        [JsonProperty]
        public string PAXTYPENO
        {
            get;
            set;
        }

        [JsonProperty]
        public string Gender
        {
            get;
            set;
        }

        [JsonProperty]
        public string DOB
        {
            get;
            set;
        }

        [JsonProperty]
        public string PassportNo
        {
            get;
            set;
        }

        [JsonProperty]
        public string ExpiryDate
        {
            get;
            set;
        }
        [JsonProperty]
        public string IssCountry
        {
            get;
            set;
        }

    }


    [DataContract]
    public class TripDetails
    {

        [JsonProperty]
        public string Orgin { get; set; }
        [JsonProperty]
        public string Destination { get; set; }
        [JsonProperty]
        public string Adultcount { get; set; }
        [JsonProperty]
        public string Childcount { get; set; }
        [JsonProperty]
        public string Infantcount { get; set; }
        //[JsonProperty]
        //public string Fromdate { get; set; }
        //[JsonProperty]
        //public string Todate { get; set; }
        [JsonProperty]
        public string Triptype { get; set; }
        [JsonProperty]
        public string Segmenttype { get; set; }
        //[JsonProperty]
        //public string Specialflag { get; set; }
        //[JsonProperty]
        //public string Hostsearch { get; set; }
        //[JsonProperty]
        //public string Classtype { get; set; }
        //[JsonProperty]
        //public string CRSid { get; set; }
        //[JsonProperty]
        //public string Multicity { get; set; }
    }

    #region RS_Seatmap
    public class GetSeat_RS
    {
        [JsonProperty]
        public string Resultcode
        {
            get;
            set;
        }
        [JsonProperty]
        public List<RS_Seatmap> SeatmapsDetails
        {
            get;
            set;
        }
        [JsonProperty]
        public string Error
        {
            get;
            set;
        }
    }

    [DataContract]
    public class RS_Seatmap
    {

        [JsonProperty]
        public string SegRefNo
        {
            get;
            set;
        }
        [JsonProperty]
        public string SEATNAME
        {
            get;
            set;
        }
        [JsonProperty]
        public string SEATAMOUNT
        {
            get;
            set;
        }
        [JsonProperty]
        public string PaxRefNo
        {
            get;
            set;
        }
        [JsonProperty]
        public string PaxType
        {
            get;
            set;
        }
        [JsonProperty]
        public string SeatRef
        {
            get;
            set;
        }
        [JsonProperty]
        public string SectorDet
        {
            get;
            set;
        }
        [JsonProperty]
        public string SeatrefAPI
        {
            get;
            set;
        }
    }
    #endregion




    //[DataContract]
    //public class AgentDetails
    //{
    //    [JsonProperty]
    //    public string AgentID
    //    {
    //        get;
    //        set;
    //    }
    //    [JsonProperty]
    //    public string TerminalID
    //    {
    //        get;
    //        set;
    //    }
    //    [JsonProperty]
    //    public string UserName
    //    {
    //        get;
    //        set;
    //    }
    //    [JsonProperty]
    //    public string Version
    //    {
    //        get;
    //        set;
    //    }
    //    [JsonProperty]
    //    public string Environment
    //    {
    //        get;
    //        set;
    //    }
    //    [JsonProperty]
    //    public string AppType
    //    {
    //        get;
    //        set;
    //    }
    //    [JsonProperty]
    //    public string BranchID
    //    {
    //        get;
    //        set;
    //    }
    //    [JsonProperty]
    //    public string BOAID
    //    {
    //        get;
    //        set;
    //    }

    //}
    [DataContract]
    public class AgentDetails
    {
        [DataMember]
        public string AgentId
        {
            get;
            set;
        }
        [DataMember]
        public string PostId
        {
            get;
            set;
        }
        [DataMember]
        public string PosId
        {
            get;
            set;
        }
        [DataMember]
        public string TerminalId
        {
            get;
            set;
        }
        [DataMember]
        public string BranchId
        {
            get;
            set;
        }
        [DataMember(Name = "CID")]
        public string ClientID
        {
            get;
            set;
        }
        [DataMember]
        public string WinyatraId
        {
            get;
            set;
        }
        [DataMember]
        public string AppType
        {
            get;
            set;
        }
        [DataMember]
        public string IssuingPosId
        {
            get;
            set;
        }
        [DataMember]
        public string IssuingPosTId
        {
            get;
            set;
        }
        [DataMember]
        public string IssuingBranchId
        {
            get;
            set;
        }
        [DataMember]
        public string BOAID
        {
            get;
            set;
        }
        [DataMember]
        public string BOAterminalID
        {
            get;
            set;
        }
        [DataMember]
        public string UserTrackId
        {
            get;
            set;
        }
        [DataMember]
        public string UserName
        {
            get;
            set;
        }
        [DataMember]
        public string Version
        {
            get;
            set;
        }
        [DataMember]
        public string Environment
        {
            get;
            set;
        }
        [DataMember]
        public string AgentType
        {
            get;
            set;
        }
        [DataMember]
        public string ProductType { get; set; }
        [DataMember]
        public string APPCurrency
        {
            get;
            set;
        }
    }

    [DataContract]
    public class RQFlights
    {
        [JsonProperty]
        public string CarrierCode
        {
            get;
            set;
        }
        [JsonProperty]
        public string FlightNumber
        {
            get;
            set;
        }
        [JsonProperty]
        public string Origin
        {
            get;
            set;
        }
        [JsonProperty]
        public string Destination
        {
            get;
            set;
        }
        [JsonProperty]
        public string StartTerminal
        {
            get;
            set;
        }
        [JsonProperty]
        public string EndTerminal
        {
            get;
            set;
        }
        [JsonProperty]
        public string DepartureDateTime
        {
            get;
            set;
        }
        [JsonProperty]
        public string ArrivalDateTime
        {
            get;
            set;
        }
        [JsonProperty]
        public string Class
        {
            get;
            set;
        }
        [JsonProperty]
        public string Cabin
        {
            get;
            set;
        }
        [JsonProperty]
        public string FareBasisCode
        {
            get;
            set;
        }
        [JsonProperty]
        public string AirlineCategory
        {
            get;
            set;
        }
        [JsonProperty]
        public string PlatingCarrier
        {
            get;
            set;
        }
        [JsonProperty]
        public string ReferenceToken
        {
            get;
            set;
        }
        [JsonProperty]
        public string SegRef
        {
            get;
            set;
        }
        [JsonProperty]
        public string ItinRef
        {
            get;
            set;
        }
        [JsonProperty]
        public string FareID
        {
            get;
            set;
        }
        [JsonProperty]
        public string SeatAvailFlag { get; set; }
        [JsonProperty]
        public string FareType { get; set; }
    }



    [DataContract]
    public class Segment
    {
        [JsonProperty]
        public string BaseOrigin
        {
            get;
            set;
        }
        [JsonProperty]
        public string BaseDestination
        {
            get;
            set;
        }
        [JsonProperty]
        public string SegmentType
        {
            get;
            set;
        }
        [JsonProperty]
        public int Adult
        {
            get;
            set;
        }
        [JsonProperty]
        public int Child
        {
            get;
            set;
        }
        [JsonProperty]
        public int Infant
        {
            get;
            set;
        }
        [JsonProperty]
        public string TripType
        {
            get;
            set;
        }
    }
    [DataContract]
    public class Status
    {
        [JsonProperty("RSC")]
        public string ResultCode
        {
            get;
            set;
        }
        [JsonProperty("ERR")]
        public string Error
        {
            get;
            set;
        }
        [JsonProperty("SEQ")]
        public String SequenceID
        {
            get;
            set;
        }
    }

}

namespace multiclass
{
    #region MultiClass_RQ
    [XmlRoot(ElementName = "MultiClass_RQ")]
    [DataContract]
    public class MultiClass_RQ
    {

        [DataMember]
        public AgentDetails AgentDetail
        {
            get;
            set;
        }
        [DataMember]
        public List<RQFlights> FlightsDetails
        {
            get;
            set;
        }
        [DataMember]
        public Segment SegmentDetails
        {
            get;
            set;
        }
        [DataMember]
        public String Platform
        {
            get;
            set;
        }
        [DataMember]
        public String Stock
        {
            get;
            set;
        }
    }
    [DataContract]
    public class AgentDetails
    {
        [DataMember]
        public string AgentID
        {
            get;
            set;
        }
        [DataMember]
        public string TerminalID
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        [DataMember]
        public string Version
        {
            get;
            set;
        }
        [DataMember]
        public string Environment
        {
            get;
            set;
        }
        [DataMember]
        public string AppType
        {
            get;
            set;
        }
        [DataMember]
        public string BranchID
        {
            get;
            set;
        }
        [DataMember]
        public string BOAID
        {
            get;
            set;
        }
        [DataMember]
        public string BOATreminalID
        {
            get;
            set;
        }
        [DataMember]
        public string ClientID
        {
            get;
            set;
        }
        [DataMember]
        public string CoOrdinatorID
        {
            get;
            set;
        }
        [DataMember]
        public string ProductID
        {
            get;
            set;
        }
        [DataMember]
        public string APPCurrency
        {
            get;
            set;
        }
        [DataMember]
        public string Airportid
        {
            get;
            set;
        }
        [DataMember]
        public string APIUSE
        {
            get;
            set;
        }
    }
    [DataContract]
    public class Segment
    {
        [DataMember]
        public string BaseOrigin
        {
            get;
            set;
        }
        [DataMember]
        public string BaseDestination
        {
            get;
            set;
        }
        [DataMember]
        public string SegmentType
        {
            get;
            set;
        }
        [DataMember]
        public int Adult
        {
            get;
            set;
        }
        [DataMember]
        public int Child
        {
            get;
            set;
        }
        [DataMember]
        public int Infant
        {
            get;
            set;
        }
        [DataMember]
        public string TripType
        {
            get;
            set;
        }
    }
    [DataContract]
    public class RQFlights
    {
        [DataMember]
        public string CarrierCode
        {
            get;
            set;
        }
        [DataMember]
        public string FlightNumber
        {
            get;
            set;
        }
        [DataMember]
        public string Origin
        {
            get;
            set;
        }
        [DataMember]
        public string Destination
        {
            get;
            set;
        }
        [DataMember]
        public string StartTerminal
        {
            get;
            set;
        }
        [DataMember]
        public string EndTerminal
        {
            get;
            set;
        }
        [DataMember]
        public string DepartureDateTime
        {
            get;
            set;
        }
        [DataMember]
        public string ArrivalDateTime
        {
            get;
            set;
        }
        [DataMember]
        public string Class
        {
            get;
            set;
        }
        [DataMember]
        public string Cabin
        {
            get;
            set;
        }
        [DataMember]
        public string FareBasisCode
        {
            get;
            set;
        }
        [DataMember]
        public string AirlineCategory
        {
            get;
            set;
        }
        [DataMember]
        public string PlatingCarrier
        {
            get;
            set;
        }
        [DataMember]
        public string ReferenceToken
        {
            get;
            set;
        }
        [DataMember]
        public string SegRef
        {
            get;
            set;
        }
        [DataMember]
        public string ItinRef
        {
            get;
            set;
        }
        [DataMember]
        public string FareID
        {
            get;
            set;
        }
        [DataMember]
        public string SeatAvailFlag
        {
            get;
            set;
        }
    }
    #endregion

    #region MultiClass_RS
    //[XmlRoot(ElementName = "MultiClass_RS")]
    [DataContract]
    public class MultiClass_RS
    {
        [JsonProperty("STU")]
        public Status Status
        {
            get;
            set;
        }
        [JsonProperty("FLD")]
        public List<Flights> FlightDetails
        {
            get;
            set;
        }
        [JsonProperty("FRD")]
        public List<Fares> FareDetails
        {
            get;
            set;
        }
    }
    [DataContract]
    public class Status
    {
        [JsonProperty("RSC")]
        public string ResultCode
        {
            get;
            set;
        }
        [JsonProperty("ERR")]
        public string Error
        {
            get;
            set;
        }
        [JsonProperty("SEQ")]
        public String SequenceID
        {
            get;
            set;
        }
    }
    [DataContract]
    public class Flights
    {
        [JsonProperty("CAC")]
        public string CarrierCode
        {
            get;
            set;
        }
        [JsonProperty("FNO")]
        public string FlightNumber
        {
            get;
            set;
        }
        [JsonProperty("ORG")]
        public string Origin
        {
            get;
            set;
        }
        [JsonProperty("DES")]
        public string Destination
        {
            get;
            set;
        }
        [JsonProperty("STL")]
        public string StartTerminal
        {
            get;
            set;
        }
        [JsonProperty("DTL")]
        public string EndTerminal
        {
            get;
            set;
        }
        [JsonProperty("SDT")]
        public string DepartureDateTime
        {
            get;
            set;
        }
        [JsonProperty("EDT")]
        public string ArrivalDateTime
        {
            get;
            set;
        }
        [JsonProperty("CLS")]
        public string Class
        {
            get;
            set;
        }
        [JsonProperty("CAB")]
        public string Cabin
        {
            get;
            set;
        }
        [JsonProperty("FBC")]
        public string FareBasisCode
        {
            get;
            set;
        }
        [JsonProperty("STP")]
        public string Stops
        {
            get;
            set;
        }
        [JsonProperty("ALC")]
        public string AirlineCategory
        {
            get;
            set;
        }
        [JsonProperty("PLT")]
        public string PlatingCarrier
        {
            get;
            set;
        }
        [JsonProperty("RFT")]
        public string ReferenceToken
        {
            get;
            set;
        }
        [JsonProperty("SEG")]
        public string SegRef
        {
            get;
            set;
        }
        [JsonProperty("ITN")]
        public string ItinRef
        {
            get;
            set;
        }
        [JsonProperty("FRI")]
        public string FareID
        {
            get;
            set;
        }
        [JsonProperty("RFB")]
        public string Refundable
        {
            get;
            set;
        }
        [JsonProperty("BAG")]
        public string Baggage
        {
            get;
            set;
        }
        [JsonProperty("MEL")]
        public string Meals
        {
            get;
            set;
        }
        [JsonProperty("OBF")]
        public string Otherbenfit
        {
            get;
            set;
        }
    }
    [DataContract]
    public class Fares
    {
        [JsonProperty("FDC")]
        public List<Faredescription> Faredescription
        {
            get;
            set;
        }
        [JsonProperty("CUR")]
        public string Currency
        {
            get;
            set;
        }
        [JsonProperty("ACU")]
        public string APICurrency
        {
            get;
            set;
        }
        [JsonProperty("ROE")]
        public string ROEValue
        {
            get;
            set;
        }
        [JsonProperty("FID")]
        public string FlightID
        {
            get;
            set;
        }
    }
    [DataContract]
    public class Faredescription
    {

        [JsonProperty("PTY")]
        public string PaxType
        {
            get;
            set;
        }
        [JsonProperty("BFA")]
        public string BaseAmount
        {
            get;
            set;
        }
        [JsonProperty("TTA")]
        public string TotalTaxAmount
        {
            get;
            set;
        }
        [JsonProperty("GRA")]
        public string GrossAmount
        {
            get;
            set;
        }
        [JsonProperty("ABU")]
        public string APIBreakup
        {
            get;
            set;
        }
        [JsonProperty("COM")]
        public string Commission
        {
            get;
            set;
        }
        [JsonProperty("INC")]
        public string Incentive
        {
            get;
            set;
        }
        [JsonProperty("SVC")]
        public string Servicecharge
        {
            get;
            set;
        }
        [JsonProperty("STA")]
        public string ServiceTax
        {
            get;
            set;
        }
        [JsonProperty("TDS")]
        public string TDS
        {
            get;
            set;
        }
        [JsonProperty("DSC")]
        public string Discount
        {
            get;
            set;
        }
        [JsonProperty("TSF")]
        public string TransactionFee
        {
            get;
            set;
        }
        [JsonProperty("MRK")]
        public string Markup
        {
            get;
            set;
        }
        public string ServiceFee { get; set; }
        [JsonProperty("HID")]
        public string HIDDMARKUP
        {
            get;
            set;
        }
        [JsonProperty("CMKP")]
        public string ClientMarkup
        {
            get;
            set;
        }
        [JsonProperty("AMK")]
        public string AddMarkup
        {
            get;
            set;
        }
        [JsonProperty("SFA")]
        public string SFAMOUNT
        {
            get;
            set;
        }
        [JsonProperty("SFG")]
        public string SFGST
        {
            get;
            set;
        }
        [JsonProperty("AGD")]
        public string AgnDeal
        {
            get;
            set;
        }
        [JsonProperty("OLF")]
        public string OldFare
        {
            get;
            set;
        }
        [JsonProperty("OLM")]
        public string OldMarkup
        {
            get;
            set;
        }
        [JsonProperty("BBO")]
        public bool BestBuyOption
        {
            get;
            set;
        }
        [JsonProperty("PLB")]
        public string PLB
        {
            get;
            set;
        }
        [JsonProperty("TAX")]
        public List<Taxes> Taxes
        {
            get;
            set;
        }
    }
    [DataContract]
    public class Taxes
    {
        [JsonProperty("TAC")]
        public string Code
        {
            get;
            set;
        }
        [JsonProperty("TAM")]
        public string Amount
        {
            get;
            set;
        }
    }
    #endregion

    /*23/1/2016 by udhaya*/

    #region GetClasses_RQ
    [XmlRoot(ElementName = "Class_RQ")]
    [DataContract]
    public class Class_RQ
    {
        [DataMember]
        public AgentDetails AgentDetail
        {
            get;
            set;
        }
        [DataMember]
        public List<SegmentDetails> Segments
        {
            get;
            set;
        }
        [DataMember]
        public PaxInfos PaxDetails
        {
            get;
            set;
        }
        [DataMember]
        public String Platform
        {
            get;
            set;
        }
        [DataMember]
        public String Stock
        {
            get;
            set;
        }
    }
    [DataContract]
    public class SegmentDetails
    {
        [DataMember]
        public string Origin
        {
            get;
            set;
        }
        [DataMember]
        public string Destination
        {
            get;
            set;
        }
        [DataMember]
        public string CarrierCode
        {
            get;
            set;
        }
        [DataMember]
        public string FlightNumber
        {
            get;
            set;
        }
        [DataMember]
        public string DepartureDateTime
        {
            get;
            set;
        }
        [DataMember]
        public string ArrivalDateTime
        {
            get;
            set;
        }
        [DataMember]
        public string AirlineCategory
        {
            get;
            set;
        }
        [DataMember]
        public string[] Cabin
        {
            get;
            set;
        }
        [DataMember]
        public string Class
        {
            get;
            set;
        }
        [DataMember]
        public string FareBasisCode
        {
            get;
            set;
        }
        [DataMember]
        public string ReferenceToken
        {
            get;
            set;
        }
        [DataMember]
        public string ItinRef
        {
            get;
            set;
        }
        [DataMember]
        public string SegRef
        {
            get;
            set;
        }
        [DataMember]
        public string PlatingCarrier
        {
            get;
            set;
        }
        [DataMember]
        public string FareType
        {
            get;
            set;
        }
    }

    [DataContract]
    public class PaxInfos
    {
        [DataMember]
        public int ADT
        {
            get;
            set;
        }
        [DataMember]
        public int CHD
        {
            get;
            set;
        }
        [DataMember]
        public int INF
        {
            get;
            set;
        }
    }

    #endregion

    #region GetClasses_RS
    [XmlRoot(ElementName = "Class_RS")]
    [DataContract]
    public class Class_RS
    {
        [JsonProperty("STU")]
        public Status Status
        {
            get;
            set;
        }
        [JsonProperty("ACS")]
        public List<AvailDetails> AvailDetails
        {
            get;
            set;
        }

    }
    //[DataContract]
    //public class Status
    //{
    //    [DataMember(Name = "RSC")]
    //    public string ResultCode
    //    {
    //        get;
    //        set;
    //    }
    //    [DataMember(Name = "ERR")]
    //    public string Error
    //    {
    //        get;
    //        set;
    //    }
    //    [DataMember(Name = "SEQ")]
    //    public String SequenceID
    //    {
    //        get;
    //        set;
    //    }
    //}
    [DataContract]
    public class AvailDetails
    {
        [JsonProperty("ORG")]
        public string Origin
        {
            get;
            set;
        }
        [JsonProperty("DST")]
        public string Destination
        {
            get;
            set;
        }
        [JsonProperty("CAC")]
        public string CarrierCode
        {
            get;
            set;
        }
        [JsonProperty("FNO")]
        public string FlightNumber
        {
            get;
            set;
        }
        [JsonProperty("CLS")]
        public List<Classes> Classes
        {
            get;
            set;
        }
    }
    [DataContract]
    public class Classes
    {
        [JsonProperty("CLA")]
        public string Class
        {
            get;
            set;
        }
        [JsonProperty("SAT")]
        public string Seats
        {
            get;
            set;
        }
        [JsonProperty("FBC")]
        public string FareBasisCode
        {
            get;
            set;
        }
    }
    #endregion

    /*23/1/2016*/
}



public class DatabaseLog
{

    private static string terminalType = "W";
    public static void LogData(string userName, string logType, string pageName, string function, string description, string agentID, string terminalID, string sequenceID)
    {


        try
        {

            sequenceID = string.IsNullOrEmpty(sequenceID) ? DateTime.Now.ToString("yymmddhhmmss") : sequenceID;
            description = (logType.ToUpper() == "X") ? "<![CDATA[" + description + "]]>" : description;
            string rootnode = logType.ToUpper() == "X" ? "<ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "<INSTALATION>" : "<EVENT>"));
            string rootnodeEnd = logType.ToUpper() == "X" ? "</ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "</INSTALATION>" : "</EVENT>"));
            string xmlData = string.Empty;

            xmlData = rootnode + description + rootnodeEnd;
            xmlData = logType.ToUpper() == "P" ? description : xmlData;
            LogException((string.IsNullOrEmpty(agentID)) ? "" : agentID.ToString(), (string.IsNullOrEmpty(terminalID)) ? "" : terminalID.ToString(), (string.IsNullOrEmpty(userName)) ? "" : userName.ToString(), HttpContext.Current.Session["ipAddress"] == null ? HttpContext.Current.Request.UserHostAddress.ToString() : HttpContext.Current.Session["ipAddress"].ToString(), (string.IsNullOrEmpty(terminalType)) ? "W" : terminalType.ToString(), (string.IsNullOrEmpty(logType)) ? "" : logType.ToString(), xmlData.ToString(), Convert.ToDecimal(sequenceID), (string.IsNullOrEmpty(pageName)) ? "" : pageName.ToString(), (string.IsNullOrEmpty(function)) ? "" : function.ToString());
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
                string filename = (terminalID != null && terminalID != "") ? terminalID : "Terminal" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string Savedpath = Folderpath + "\\" + filename + ".txt";

                if (!File.Exists(Savedpath))
                {
                    StreamWriter sw = File.CreateText(Savedpath);

                    sw.WriteLine("DATE :" + DateTime.Now.ToString() + "============================================================================="
                         + "===================================================================="
                         + "=================================================================================================================\r\n");
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty("")) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + "" + "\r\n"))
                        + ((string.IsNullOrEmpty("W")) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + "W" + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(pageName)) ? "" : ("PAGENAME :".PadLeft(15, ' ') + pageName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(function)) ? "" : ("FUNCTION :".PadLeft(15, ' ') + function.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(description)) ? "" : ("DESCRIPTION :".PadLeft(15, ' ') + description.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("Exeption :" + ex.ToString())));
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
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty("")) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + "" + "\r\n"))
                        + ((string.IsNullOrEmpty("W")) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + "W" + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(pageName)) ? "" : ("PAGENAME :".PadLeft(15, ' ') + pageName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(function)) ? "" : ("FUNCTION :".PadLeft(15, ' ') + function.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(description)) ? "" : ("DESCRIPTION :".PadLeft(15, ' ') + description.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("EXEPTION :" + ex.ToString())));
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
    }

    public static void PGlogdata(string userName, string logType, string pageName, string function, string description, string agentID, string terminalID, string sequenceID, string Terminaltype)
    {


        try
        {


            string rootnode = logType.ToUpper() == "X" ? "<ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "<INSTALATION>" : "<EVENT>"));
            string rootnodeEnd = logType.ToUpper() == "X" ? "</ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "</INSTALATION>" : "</EVENT>"));
            string xmlData = string.Empty;

            xmlData = rootnode + description + rootnodeEnd;
            xmlData = logType.ToUpper() == "P" ? description : xmlData;
            LogException((string.IsNullOrEmpty(agentID)) ? "" : agentID.ToString(), (string.IsNullOrEmpty(terminalID)) ? "" : terminalID.ToString(), (string.IsNullOrEmpty(userName)) ? "" : userName.ToString(), (HttpContext.Current.Session["ipAddress"]==null) ? HttpContext.Current.Request.UserHostAddress.ToString() : HttpContext.Current.Session["ipAddress"].ToString(), (string.IsNullOrEmpty(Terminaltype)) ? "W" : Terminaltype, (string.IsNullOrEmpty(logType)) ? "" : logType.ToString(), xmlData.ToString(), Convert.ToDecimal(sequenceID), (string.IsNullOrEmpty(pageName)) ? "" : pageName.ToString(), (string.IsNullOrEmpty(function)) ? "" : function.ToString());
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
                string filename = (terminalID != null && terminalID != "") ? terminalID : "Terminal" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string Savedpath = Folderpath + "\\" + filename + ".txt";

                if (!File.Exists(Savedpath))
                {
                    StreamWriter sw = File.CreateText(Savedpath);

                    sw.WriteLine("DATE :" + DateTime.Now.ToString() + "============================================================================="
                         + "===================================================================="
                         + "=================================================================================================================\r\n");
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty("")) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + "" + "\r\n"))
                        + ((string.IsNullOrEmpty("W")) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + "W" + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(pageName)) ? "" : ("PAGENAME :".PadLeft(15, ' ') + pageName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(function)) ? "" : ("FUNCTION :".PadLeft(15, ' ') + function.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(description)) ? "" : ("DESCRIPTION :".PadLeft(15, ' ') + description.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("Exeption :" + ex.ToString())));
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
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty("")) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + "" + "\r\n"))
                        + ((string.IsNullOrEmpty("W")) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + "W" + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(pageName)) ? "" : ("PAGENAME :".PadLeft(15, ' ') + pageName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(function)) ? "" : ("FUNCTION :".PadLeft(15, ' ') + function.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(description)) ? "" : ("DESCRIPTION :".PadLeft(15, ' ') + description.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("EXEPTION :" + ex.ToString())));
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
    }

    public static void LogException(string agentID, string terminalID, string userName, string ipAddress, string terminalType, string logType, string logData, decimal sequenceID, string PageName, string Function)
    {
        try
        {
            RaysService S_SERVER_DOWNLOAD = new RaysService();
            S_SERVER_DOWNLOAD.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
            string insStatus = string.Empty;

            terminalType = HttpContext.Current.Session["TERMINALTYPE"] != null && HttpContext.Current.Session["TERMINALTYPE"].ToString() != "" ? HttpContext.Current.Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            ipAddress = ConfigurationManager.AppSettings["SERVERIPADDRESS"] != null ? ipAddress + "--" + ConfigurationManager.AppSettings["SERVERIPADDRESS"].ToString() : ipAddress;

            S_SERVER_DOWNLOAD.Insert_Detailed_LogDetails(agentID, terminalID, userName, ipAddress, terminalType, logType, logData, sequenceID, PageName, Function);

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
                string filename = (terminalID != null && terminalID != "") ? terminalID : "Terminal" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string Savedpath = Folderpath + "\\" + filename + ".txt";

                if (!File.Exists(Savedpath))
                {

                    StreamWriter sw = File.CreateText(Savedpath);
                    sw.WriteLine("DATE :" + DateTime.Now.ToString() + "============================================================================="
                         + "===================================================================="
                         + "=================================================================================================================\r\n");
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(ipAddress)) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + ipAddress.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalType)) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + terminalType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(logData)) ? "" : ("LOGDATA :".PadLeft(15, ' ') + logData.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("EXEPTION :" + ex.ToString())));
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
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(ipAddress)) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + ipAddress.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(terminalType)) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + terminalType.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(logData)) ? "" : ("LOGDATA :".PadLeft(15, ' ') + logData.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                         + (("EXEPTION :" + ex.ToString())));
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
    }

    public static void TPlogdata(string userName, string logType, string pageName, string function, string description, string agentID, string terminalID, string sequenceID)
    {
        try
        {
            string rootnode = logType.ToUpper() == "X" ? "<ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "<INSTALATION>" : "<EVENT>"));
            string rootnodeEnd = logType.ToUpper() == "X" ? "</ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "</INSTALATION>" : "</EVENT>"));
            string xmlData = string.Empty;

            xmlData = rootnode + description + rootnodeEnd;
            xmlData = logType.ToUpper() == "P" ? description : xmlData;
            TPLogException((string.IsNullOrEmpty(agentID)) ? "" : agentID.ToString(), (string.IsNullOrEmpty(terminalID)) ? "" : terminalID.ToString(), (string.IsNullOrEmpty(userName)) ? "" : userName.ToString(), (string.IsNullOrEmpty(HttpContext.Current.Session["ipAddress"].ToString())) ? "" : HttpContext.Current.Session["ipAddress"].ToString(), (string.IsNullOrEmpty("W")) ? "W" : "W", (string.IsNullOrEmpty(logType)) ? "" : logType.ToString(), xmlData.ToString(), Convert.ToDecimal(sequenceID), (string.IsNullOrEmpty(pageName)) ? "" : pageName.ToString(), (string.IsNullOrEmpty(function)) ? "" : function.ToString());
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
                string filename = (terminalID != null && terminalID != "") ? terminalID : "Terminal" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string Savedpath = Folderpath + "\\" + filename + ".txt";
                if (!File.Exists(Savedpath))
                {
                    StreamWriter sw = File.CreateText(Savedpath);

                    sw.WriteLine("DATE :" + DateTime.Now.ToString() + "============================================================================="
                         + "===================================================================="
                         + "=================================================================================================================\r\n");
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty("")) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + "" + "\r\n"))
                        + ((string.IsNullOrEmpty("W")) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + "W" + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(pageName)) ? "" : ("PAGENAME :".PadLeft(15, ' ') + pageName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(function)) ? "" : ("FUNCTION :".PadLeft(15, ' ') + function.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(description)) ? "" : ("DESCRIPTION :".PadLeft(15, ' ') + description.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("Exeption :" + ex.ToString())));
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
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty("")) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + "" + "\r\n"))
                        + ((string.IsNullOrEmpty("W")) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + "W" + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(pageName)) ? "" : ("PAGENAME :".PadLeft(15, ' ') + pageName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(function)) ? "" : ("FUNCTION :".PadLeft(15, ' ') + function.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(description)) ? "" : ("DESCRIPTION :".PadLeft(15, ' ') + description.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("EXEPTION :" + ex.ToString())));
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
    }

    public static void TPLogException(string agentID, string terminalID, string userName, string ipAddress, string terminalType, string logType, string logData, decimal sequenceID, string PageName, string Function)
    {
        try
        {
            RaysService S_SERVER_DOWNLOAD = new RaysService();
            S_SERVER_DOWNLOAD.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
            string insStatus = string.Empty;
            terminalType = HttpContext.Current.Session["TERMINALTYPE"] != null && HttpContext.Current.Session["TERMINALTYPE"].ToString() != "" ? HttpContext.Current.Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            ipAddress = ConfigurationManager.AppSettings["SERVERIPADDRESS"] != null ? ipAddress + "--" + ConfigurationManager.AppSettings["SERVERIPADDRESS"].ToString() : ipAddress;
            insStatus = S_SERVER_DOWNLOAD.Insert_Pre_Request_LogDetails(agentID, terminalID, userName, ipAddress, terminalType, logType, logData, sequenceID, PageName, Function);

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
                string filename = (terminalID != null && terminalID != "") ? terminalID : "Terminal" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string Savedpath = Folderpath + "\\" + filename + ".txt";

                if (!File.Exists(Savedpath))
                {

                    StreamWriter sw = File.CreateText(Savedpath);
                    sw.WriteLine("DATE :" + DateTime.Now.ToString() + "============================================================================="
                         + "===================================================================="
                         + "=================================================================================================================\r\n");
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(ipAddress)) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + ipAddress.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalType)) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + terminalType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(logData)) ? "" : ("LOGDATA :".PadLeft(15, ' ') + logData.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("EXEPTION :" + ex.ToString())));
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
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(ipAddress)) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + ipAddress.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(terminalType)) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + terminalType.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(logData)) ? "" : ("LOGDATA :".PadLeft(15, ' ') + logData.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                         + (("EXEPTION :" + ex.ToString())));
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
    }



    public static void LogLogin(string logType, string pageName, string function, string description, string ipAddress, string terminalID, decimal sequenceID, string userName)
    {

        string agentID = "";
        //string terminalID = GlobalVar.getTerminalID;        
        string terminalType = "W";
        try
        {


            string rootnode = logType.ToUpper() == "X" ? "<ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "<INSTALATION>" : "<EVENT>"));
            string rootnodeEnd = logType.ToUpper() == "X" ? "</ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "</INSTALATION>" : "</EVENT>"));
            string xmlData = string.Empty;

            xmlData = rootnode + description + rootnodeEnd;
            xmlData = logType.ToUpper() == "P" ? description : xmlData;
            RaysService S_SERVER_DOWNLOAD = new RaysService();
            S_SERVER_DOWNLOAD.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
            terminalType = HttpContext.Current.Session["TERMINALTYPE"] != null && HttpContext.Current.Session["TERMINALTYPE"].ToString() != "" ? HttpContext.Current.Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            ipAddress = ConfigurationManager.AppSettings["SERVERIPADDRESS"] != null ? ipAddress + "--" + ConfigurationManager.AppSettings["SERVERIPADDRESS"].ToString() : ipAddress;
            string insStatus = string.Empty;

            insStatus = S_SERVER_DOWNLOAD.Insert_Detailed_LogDetails(agentID, terminalID, userName, ipAddress, terminalType, logType, xmlData, sequenceID, pageName, function);

        }
        catch (Exception ex)
        {
            try
            {

                string folderName = DateTime.Now.ToString("yyyyMMdd") +
                    DateTime.Now.TimeOfDay.ToString().Replace(':', '_');
                string Description = "<Missing_Log>";
                Description += "<TIME>" + DateTime.Now.ToString() + "</TIME>";
                Description += "<LOGTYPE>" + logType + "</LOGTYPE>";
                Description += "<LOGDATA>" + description + "</LOGDATA>";
                Description += "<pageName>" + pageName + "</pageName>";
                Description += "<function>" + function + "</function>";
                Description += "<EXEPTION>[<![CDATA[" + ex.ToString() + "]]>]</EXEPTION>";
                Description += "</Missing_Log>";

                if (!Directory.Exists("Log"))
                {
                    Directory.CreateDirectory("Log");
                    DirectoryInfo info = new DirectoryInfo("Log");
                    info.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                }

                string path = "Log/" + folderName + ".xml";
                if (!File.Exists(path))
                {
                    StreamWriter sw = File.CreateText(path);
                    sw.WriteLine(Description);
                    sw.Flush();
                    sw.Close();
                }
                else if (File.Exists(path))
                {
                    StreamWriter sw = File.AppendText(path);
                    sw.WriteLine(Description);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch
            {
            }
        }
    }



    public static void foldererrorlog(string Errormsg, string Function, string Pagename)
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
            string filename = "Errorlog" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string Savedpath = Folderpath + "\\" + filename + ".txt";

            if (!File.Exists(Savedpath))
            {
                StreamWriter sw = File.CreateText(Savedpath);

                sw.WriteLine("DATE :" + DateTime.Now.ToString() + "============================================================================="
                     + "===================================================================="
                     + "=================================================================================================================\r\n");
                sw.WriteLine(((string.IsNullOrEmpty("W")) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + "W" + "\r\n"))
                    + ((string.IsNullOrEmpty("X")) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + "X" + "\r\n"))
                    + ((string.IsNullOrEmpty(Pagename)) ? "" : ("PAGENAME :".PadLeft(15, ' ') + Pagename.ToString() + "\r\n"))
                    + ((string.IsNullOrEmpty(Function)) ? "" : ("FUNCTION :".PadLeft(15, ' ') + Function.ToString() + "\r\n"))
                    + ((string.IsNullOrEmpty(Errormsg)) ? "" : ("DESCRIPTION :".PadLeft(15, ' ') + Errormsg.ToString() + "\r\n")));
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
                sw.WriteLine(((string.IsNullOrEmpty("W")) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + "W" + "\r\n"))
                    + ((string.IsNullOrEmpty("X")) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + "X" + "\r\n"))
                    + ((string.IsNullOrEmpty(Pagename)) ? "" : ("PAGENAME :".PadLeft(15, ' ') + Pagename.ToString() + "\r\n"))
                    + ((string.IsNullOrEmpty(Function)) ? "" : ("FUNCTION :".PadLeft(15, ' ') + Function.ToString() + "\r\n"))
                    + ((string.IsNullOrEmpty(Errormsg)) ? "" : ("DESCRIPTION :".PadLeft(15, ' ') + Errormsg.ToString() + "\r\n")));
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


    public static void LogData_wasc(string userName, string logType, string pageName, string function, string description, string agentID, string terminalID, string sequenceID)
    {


        try
        {


            string rootnode = logType.ToUpper() == "X" ? "<ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "<INSTALATION>" : "<EVENT>"));
            string rootnodeEnd = logType.ToUpper() == "X" ? "</ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "</INSTALATION>" : "</EVENT>"));
            string xmlData = string.Empty;

            xmlData = rootnode + description + rootnodeEnd;
            xmlData = logType.ToUpper() == "P" ? description : xmlData;
            LogException_wasc((string.IsNullOrEmpty(agentID)) ? "" : agentID.ToString(), (string.IsNullOrEmpty(terminalID)) ? "" : terminalID.ToString(), (string.IsNullOrEmpty(userName)) ? "" : userName.ToString(), (string.IsNullOrEmpty(HttpContext.Current.Session["ipAddress"].ToString())) ? "" : HttpContext.Current.Session["ipAddress"].ToString(), (string.IsNullOrEmpty(terminalType)) ? "W" : terminalType.ToString(), (string.IsNullOrEmpty(logType)) ? "" : logType.ToString(), xmlData.ToString(), Convert.ToDecimal(sequenceID), (string.IsNullOrEmpty(pageName)) ? "" : pageName.ToString(), (string.IsNullOrEmpty(function)) ? "" : function.ToString());
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
                string filename = (terminalID != null && terminalID != "") ? terminalID : "Terminal" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string Savedpath = Folderpath + "\\" + filename + ".txt";

                if (!File.Exists(Savedpath))
                {
                    StreamWriter sw = File.CreateText(Savedpath);

                    sw.WriteLine("DATE :" + DateTime.Now.ToString() + "============================================================================="
                         + "===================================================================="
                         + "=================================================================================================================\r\n");
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty("")) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + "" + "\r\n"))
                        + ((string.IsNullOrEmpty("W")) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + "W" + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(pageName)) ? "" : ("PAGENAME :".PadLeft(15, ' ') + pageName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(function)) ? "" : ("FUNCTION :".PadLeft(15, ' ') + function.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(description)) ? "" : ("DESCRIPTION :".PadLeft(15, ' ') + description.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("Exeption :" + ex.ToString())));
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
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty("")) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + "" + "\r\n"))
                        + ((string.IsNullOrEmpty("W")) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + "W" + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(pageName)) ? "" : ("PAGENAME :".PadLeft(15, ' ') + pageName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(function)) ? "" : ("FUNCTION :".PadLeft(15, ' ') + function.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(description)) ? "" : ("DESCRIPTION :".PadLeft(15, ' ') + description.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("EXEPTION :" + ex.ToString())));
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
    }

    public static void LogException_wasc(string agentID, string terminalID, string userName, string ipAddress, string terminalType, string logType, string logData, decimal sequenceID, string PageName, string Function)
    {
        try
        {
            RaysService S_SERVER_DOWNLOAD = new RaysService();
            //Trays.S_SERVER_WSDL.RaysService S_SERVER_DOWNLOAD = new Trays.S_SERVER_WSDL.RaysService();
            S_SERVER_DOWNLOAD.Url = ConfigurationManager.AppSettings["ServiceURI"].ToString();
            string insStatus = string.Empty;
            terminalType = HttpContext.Current.Session["TERMINALTYPE"] != null && HttpContext.Current.Session["TERMINALTYPE"].ToString() != "" ? HttpContext.Current.Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            ipAddress = ConfigurationManager.AppSettings["SERVERIPADDRESS"] != null ? ipAddress + "--" + ConfigurationManager.AppSettings["SERVERIPADDRESS"].ToString() : ipAddress;

            insStatus = S_SERVER_DOWNLOAD.Insert_Detailed_LogDetails(agentID, terminalID, userName, ipAddress, terminalType, logType, logData, sequenceID, PageName, Function);

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
                string filename = (terminalID != null && terminalID != "") ? terminalID : "Terminal" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string Savedpath = Folderpath + "\\" + filename + ".txt";

                if (!File.Exists(Savedpath))
                {

                    StreamWriter sw = File.CreateText(Savedpath);
                    sw.WriteLine("DATE :" + DateTime.Now.ToString() + "============================================================================="
                         + "===================================================================="
                         + "=================================================================================================================\r\n");
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(ipAddress)) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + ipAddress.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalType)) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + terminalType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(logData)) ? "" : ("LOGDATA :".PadLeft(15, ' ') + logData.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("EXEPTION :" + ex.ToString())));
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
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(ipAddress)) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + ipAddress.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(terminalType)) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + terminalType.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(logData)) ? "" : ("LOGDATA :".PadLeft(15, ' ') + logData.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                         + (("EXEPTION :" + ex.ToString())));
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
    }

    public static void Retrievetcktlog(string userName, string logType, string PageName, string Function, string logData, string agentID, string terminalID, string sequenceID)
    {
        string ipAddress = string.Empty;
        try
        {
            string rootnode = logType.ToUpper() == "X" ? "<ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "<INSTALATION>" : "<EVENT>"));
            string rootnodeEnd = logType.ToUpper() == "X" ? "</ERROR>" : (logType.ToUpper() == "T" ? "" : (logType.ToUpper() == "I" ? "</INSTALATION>" : "</EVENT>"));
            string xmlData = string.Empty;
            xmlData = rootnode + logData + rootnodeEnd;

            agentID= (string.IsNullOrEmpty(agentID)) ? "" : agentID.ToString();
            terminalID= (string.IsNullOrEmpty(terminalID)) ? "" : terminalID.ToString();
            userName=(string.IsNullOrEmpty(userName)) ? "" : userName.ToString();
            ipAddress = HttpContext.Current.Session["ipAddress"] == null ? HttpContext.Current.Request.UserHostAddress.ToString() : HttpContext.Current.Session["ipAddress"].ToString();
            terminalType = HttpContext.Current.Session["TERMINALTYPE"] != null && HttpContext.Current.Session["TERMINALTYPE"].ToString() != "" ? HttpContext.Current.Session["TERMINALTYPE"].ToString() : ConfigurationManager.AppSettings["TerminalType"].ToString();
            ipAddress = ConfigurationManager.AppSettings["SERVERIPADDRESS"] != null ? ipAddress + "--" + ConfigurationManager.AppSettings["SERVERIPADDRESS"].ToString() : ipAddress;
            logType =(string.IsNullOrEmpty(logType)) ? "" : logType.ToString();
            PageName = (string.IsNullOrEmpty(PageName)) ? "" : PageName.ToString();
            Function = (string.IsNullOrEmpty(Function)) ? "" : Function.ToString();

            RaysService S_SERVER_DOWNLOAD = new RaysService();
            S_SERVER_DOWNLOAD.Url = ConfigurationManager.AppSettings["QTKT_APPS_RAYS_SERVICE"].ToString();
            string insStatus = string.Empty;
            S_SERVER_DOWNLOAD.Insert_Detailed_LogDetails(agentID, terminalID, userName, ipAddress, terminalType, logType, xmlData, Convert.ToDecimal(sequenceID), PageName, Function);

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
                string filename = (terminalID != null && terminalID != "") ? terminalID : "Terminal" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string Savedpath = Folderpath + "\\" + filename + ".txt";

                if (!File.Exists(Savedpath))
                {

                    StreamWriter sw = File.CreateText(Savedpath);
                    sw.WriteLine("DATE :" + DateTime.Now.ToString() + "============================================================================="
                         + "===================================================================="
                         + "=================================================================================================================\r\n");
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(ipAddress)) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + ipAddress.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(terminalType)) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + terminalType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                        + ((string.IsNullOrEmpty(logData)) ? "" : ("LOGDATA :".PadLeft(15, ' ') + logData.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                        + (("EXEPTION :" + ex.ToString())));
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
                    sw.WriteLine(((string.IsNullOrEmpty(agentID)) ? "" : ("AGENTID :".PadLeft(15, ' ') + agentID.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(terminalID)) ? "" : ("TERMINAL ID :".PadLeft(15, ' ') + terminalID.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(userName)) ? "" : ("USERNAME :".PadLeft(15, ' ') + userName.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(ipAddress)) ? "" : ("IPADDRESS :".PadLeft(15, ' ') + ipAddress.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(terminalType)) ? "" : ("TERMINALTYPE :".PadLeft(15, ' ') + terminalType.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(logType)) ? "" : ("LOGTYPE :".PadLeft(15, ' ') + logType.ToString() + "\r\n"))
                         + ((string.IsNullOrEmpty(logData)) ? "" : ("LOGDATA :".PadLeft(15, ' ') + logData.ToString() + "\r\n"))
                        + (("SEQUENCEID :".PadLeft(15, ' ') + sequenceID.ToString() + "\r\n"))
                         + (("EXEPTION :" + ex.ToString())));
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
    }
}


