using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace STSTRAVRAYS.Models
{
    public class OnlineserviceRQRS
    {
        

        #region New

      

        #region GetClasses_RQ

        [XmlRoot(ElementName = "Class_RQ")]
        [DataContract]
        public class Class_RQ
        {
            [DataMember]
            public AgentDetailsMulticlass AgentDetail
            {
                get;
                set;
            }
            [DataMember]
            public List<SegmentDetailss> Segments
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
            [DataMember]
            public String TripType
            {
                get;
                set;
            }
        }

        [DataContract]
        public class SegmentDetailss
        {
            [DataMember(IsRequired = true)]
            public string Origin
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string Destination
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string CarrierCode
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
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
            [DataMember(IsRequired = true)]
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
            [DataMember(IsRequired = true)]
            public int ItinRef
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string SegRef
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
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
            [DataMember]
            public string FareTypeDescription
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

        [DataContract]
        public class AgentDetailsMulticlass
        {
            [DataMember]
            public string AgentID
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
            public string TerminalID
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
            [DataMember(IsRequired = true)]
            public string Version
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string Environment
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
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
            [DataMember(IsRequired = true)]
            public string ProjectId
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string Platform
            {
                get;
                set;
            }
            [DataMember]
            public string[] APIUSE
            {
                get;
                set;
            }
            public string Group_ID
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
            [DataMember(Name = "STU")]
            public Status Status
            {
                get;
                set;
            }
            [DataMember(Name = "ACS")]
            public List<AvailDetails> AvailDetails
            {
                get;
                set;
            }
        }
        [DataContract]
        public class AvailDetails
        {
            [DataMember(Name = "ORG")]
            public string Origin
            {
                get;
                set;
            }
            [DataMember(Name = "DST")]
            public string Destination
            {
                get;
                set;
            }
            [DataMember(Name = "CAC")]
            public string CarrierCode
            {
                get;
                set;
            }
            [DataMember(Name = "FNO")]
            public string FlightNumber
            {
                get;
                set;
            }
            [DataMember(Name = "SRF")]
            public string SegRef
            {
                get;
                set;
            }
            [DataMember(Name = "IRF")]
            public string ItinRef
            {
                get;
                set;
            }
            [DataMember(Name = "CLS")]
            public List<Classes> Classes
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Classes
        {
            [DataMember(Name = "CLA")]
            public string Class
            {
                get;
                set;
            }
            [DataMember(Name = "SAT")]
            public string Seats
            {
                get;
                set;
            }
            [DataMember(Name = "FBC")]
            public string FareBasisCode
            {
                get;
                set;
            }
            [DataMember(Name = "CBN")]
            public string Cabin
            {
                get;
                set;
            }
        }
        #endregion



        #region MultiClass_RQ
        [XmlRoot(ElementName = "MultiClass_RQ")]
        [DataContract]
        public class MultiClass_RQ
        {
            [DataMember]
            public AgentDetailsMulticlass AgentDetail
            {
                get;
                set;
            }
            [DataMember]
            public List<RQFlightsnew> FlightsDetails
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
        public class RQFlightsnew
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
            [DataMember]
            public string FareType
            {
                get;
                set;
            }
            [DataMember]
            public string FareTypeDescription
            {
                get;
                set;
            }
            [DataMember]
            public string PromoCode
            {
                get;
                set;
            }
        }
        #endregion

        #region MultiClass_RS
        [XmlRoot(ElementName = "MultiClass_RS")]
        [DataContract]
        public class MultiClass_RS
        {
            [DataMember(Name = "STU")]
            public Status Status
            {
                get;
                set;
            }
            [DataMember(Name = "FLD")]
            public List<Flightsnew> FlightDetails
            {
                get;
                set;
            }
            [DataMember(Name = "FRD")]
            public List<Faresnew> FareDetails
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Flightsnew
        {
            [DataMember(Name = "CAC")]
            public string CarrierCode
            {
                get;
                set;
            }
            [DataMember(Name = "FNO")]
            public string FlightNumber
            {
                get;
                set;
            }
            [DataMember(Name = "ORG")]
            public string Origin
            {
                get;
                set;
            }
            [DataMember(Name = "DES")]
            public string Destination
            {
                get;
                set;
            }
            [DataMember(Name = "STL")]
            public string StartTerminal
            {
                get;
                set;
            }
            [DataMember(Name = "DTL")]
            public string EndTerminal
            {
                get;
                set;
            }
            [DataMember(Name = "SDT")]
            public string DepartureDateTime
            {
                get;
                set;
            }
            [DataMember(Name = "EDT")]
            public string ArrivalDateTime
            {
                get;
                set;
            }
            [DataMember(Name = "CLS")]
            public string Class
            {
                get;
                set;
            }
            [DataMember(Name = "CAB")]
            public string Cabin
            {
                get;
                set;
            }
            [DataMember(Name = "FBC")]
            public string FareBasisCode
            {
                get;
                set;
            }
            [DataMember(Name = "STP")]
            public string Stops
            {
                get;
                set;
            }
            [DataMember(Name = "ALC")]
            public string AirlineCategory
            {
                get;
                set;
            }
            [DataMember(Name = "PLT")]
            public string PlatingCarrier
            {
                get;
                set;
            }
            [DataMember(Name = "RFT")]
            public string ReferenceToken
            {
                get;
                set;
            }
            [DataMember(Name = "SEG")]
            public string SegRef
            {
                get;
                set;
            }
            [DataMember(Name = "ITN")]
            public string ItinRef
            {
                get;
                set;
            }
            [DataMember(Name = "FRI")]
            public string FareID
            {
                get;
                set;
            }
            [DataMember(Name = "RFB")]
            public string Refundable
            {
                get;
                set;
            }
            [DataMember(Name = "BAG")]
            public string Baggage
            {
                get;
                set;
            }
            [DataMember(Name = "MEL")]
            public string Meals
            {
                get;
                set;
            }
            [DataMember(Name = "SET")]
            public string Seat
            {
                get;
                set;
            }
            [DataMember(Name = "CNX")]
            public string CNXType
            {
                get;
                set;
            }
            [DataMember(Name = "FYT")]
            public string FlyingTime
            {
                get;
                set;
            }
            [DataMember(Name = "FDC")]
            public string FareDescription
            {
                get;
                set;
            }

        }
        [DataContract]
        public class Faresnew
        {
            [DataMember(Name = "FDC")]
            public List<Faredescriptionnew> Faredescription
            {
                get;
                set;
            }
            [DataMember(Name = "CUR")]
            public string Currency
            {
                get;
                set;
            }
            [DataMember(Name = "ACU")]
            public string APICurrency
            {
                get;
                set;
            }
            [DataMember(Name = "ROE")]
            public string ROEValue
            {
                get;
                set;
            }
            [DataMember(Name = "FID")]
            public string FlightID
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Faredescriptionnew
        {

            [DataMember(Name = "PTY")]
            public string PaxType
            {
                get;
                set;
            }
            [DataMember(Name = "BFA")]
            public string BaseAmount
            {
                get;
                set;
            }
            [DataMember(Name = "TTA")]
            public string TotalTaxAmount
            {
                get;
                set;
            }
            [DataMember(Name = "GRA")]
            public string GrossAmount
            {
                get;
                set;
            }
            [DataMember(Name = "ABU")]
            public string APIBreakup
            {
                get;
                set;
            }
            [DataMember(Name = "COM")]
            public string Commission
            {
                get;
                set;
            }
            [DataMember(Name = "INC")]
            public string Incentive
            {
                get;
                set;
            }
            [DataMember(Name = "SVC")]
            public string Servicecharge
            {
                get;
                set;
            }
            [DataMember(Name = "STA")]
            public string ServiceTax
            {
                get;
                set;
            }
            [DataMember(Name = "TDS")]
            public string TDS
            {
                get;
                set;
            }
            [DataMember(Name = "DSC")]
            public string Discount
            {
                get;
                set;
            }
            [DataMember(Name = "TSF")]
            public string TransactionFee
            {
                get;
                set;
            }
            [DataMember(Name = "MRK")]
            public string Markup
            {
                get;
                set;
            }
            [DataMember(Name = "CMKP")]
            public string ClientMarkup
            {
                get;
                set;
            }
            [DataMember]
            public string ServiceFee
            {
                get;
                set;
            }
            [DataMember(Name = "AMK")]
            public string AddMarkup
            {
                get;
                set;
            }
            [DataMember(Name = "AGD")]
            public string AgnDeal
            {
                get;
                set;
            }
            [DataMember(Name = "OLF")]
            public string OldFare
            {
                get;
                set;
            }
            [DataMember(Name = "OLM")]
            public string OldMarkup
            {
                get;
                set;
            }
            [DataMember(Name = "BBO")]
            public bool BestBuyOption
            {
                get;
                set;
            }
            [DataMember(Name = "TAX")]
            public List<Taxesnew> Taxes
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Taxesnew
        {
            [DataMember(Name = "TAC")]
            public string Code
            {
                get;
                set;
            }
            [DataMember(Name = "TAM")]
            public string Amount
            {
                get;
                set;
            }
        }
        #endregion


        #region Multiclass_Request

        [XmlRoot(ElementName = "Multiclass_Request")]
        [DataContract]
        public class Multiclass_Request
        {
            [DataMember]
            public MLCAgentDetails AgentDetail
            {
                get;
                set;
            }
            [DataMember]
            public List<SegmentDetailsNEW> Segments
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
            public string Platform
            {
                get;
                set;
            }
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
        }
        [DataContract]
        public class MLCAgentDetails
        {
            [DataMember]
            public string AgentID
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
            public string TerminalID
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
            [DataMember(IsRequired = true)]
            public string Version
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string Environment
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
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
            public string Airportid
            {
                get;
                set;
            }
            [DataMember]
            public string[] APIUSE
            {
                get;
                set;
            }
        }
        [DataContract]
        public class SegmentDetailsNEW
        {
            [DataMember(IsRequired = true)]
            public string Origin
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string Destination
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string CarrierCode
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
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
            [DataMember(IsRequired = true)]
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
            [DataMember(IsRequired = true)]
            public int ItinRef
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string SegRef
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
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

        #endregion

        #region Multiclass_Response
        [XmlRoot(ElementName = "Multiclass_Response")]
        [DataContract]
        public class Multiclass_Response
        {
            [DataMember(Name = "STU")]
            public Status Status
            {
                get;
                set;
            }
            [DataMember(Name = "ACS")]
            public List<AvailDetails> AvailDetails
            {
                get;
                set;
            }
        }
        //[DataContract] ABCD
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


        #endregion



        #region Balance_RQ
        [DataContract]
        public class Balance_RQ
        {
            [DataMember(IsRequired = true)]
            public string Platform
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string[] CRSID
            {
                get;
                set;
            }
            [DataMember]
            public AgentDetailsBalance AgentDetail
            {
                get;
                set;
            }
        }
        #endregion

        #region Balance_RS
        [DataContract]
        public class Balance_RS
        {
            [DataMember(Name = "STU")]
            public Status Status
            {
                get;
                set;
            }
            [DataMember(Name = "BAL")]
            public string Balance
            {
                get;
                set;
            }
        }
        #endregion

        #region PromoCodeFetching

        public class PromoCodeRQ
        {
            [DataMember]
            public AgentDetails Agent //ABCD
            {
                get;
                set;
            }
        }
        public class PromoCodeRS
        {
            public class Promocodes
            {
                [DataMember]
                public string Promocode
                {
                    get;
                    set;
                }
                [DataMember]
                public string Airline
                {
                    get;
                    set;
                }
                [DataMember]
                public string PromocodeType
                {
                    get;
                    set;
                }
            }
            [DataMember]
            public List<Promocodes> _Promocodes
            {
                get;
                set;
            }
            [DataMember]
            public string ErrorCode
            {
                get;
                set;
            }
            [DataMember]
            public string ErrorDesc
            {
                get;
                set;
            }
        }

        #endregion

        #region Avail Request

        [XmlRoot(ElementName = "Itinerary")]
        [DataContract]
        public class Itineary
        {
            [DataMember]
            public Credential Credentials
            {
                get;
                set;
            }
            [DataMember(Name = "CAT")]
            public string Category
            {
                get;
                set;
            }
            [DataMember(Name = "AGD")]
            public AgentDetails Agent
            {
                get;
                set;
            }
            [DataMember(Name = "AVR")]
            public List<AvailabilityRequests> AvailabilityRequest
            {
                get;
                set;
            }
            [DataMember(Name = "PRCD")]
            public List<Promocodes> PromoCodes
            {
                get;
                set;
            }
            [DataMember(Name = "PSG")]
            public Passengers Passenger
            {
                get;
                set;
            }
            [DataMember(Name = "CYC")]
            public string Currencycode
            {
                get;
                set;
            }
            [XmlArrayItem("CarrierId", typeof(string))]
            [XmlArray("FlightOption")]
            [DataMember(Name = "FLO")]
            public string[] FlightOption
            {
                get;
                set;
            }

            [DataMember(Name = "TRP")]
            public string TripType
            {
                get;
                set;
            }
            [DataMember(Name = "SEG")]
            public string SegmentType
            {
                get;
                set;
            }
            [DataMember(Name = "HOS")]
            public bool HostSearch
            {
                get;
                set;
            }
            [DataMember(Name = "MRF")]
            public bool MoreFlights
            {
                get;
                set;
            }
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
            [DataMember(Name = "NTY")]
            public string Nationality
            {
                get;
                set;
            }

            [DataMember(Name = "WTL")]
            public bool ISWaitList
            {
                get;
                set;
            }
            [DataMember(Name = "DSC")]
            public bool Discount
            {
                get;
                set;
            }
            [DataMember(Name = "DCF")]
            public string DiscountFlag
            {
                get;
                set;
            }

            [DataMember(Name = "CMF")]
            public string Commission
            {
                get;
                set;
            }

            [DataMember(Name = "DFT")]
            public bool DirectFlight
            {
                get;
                set;
            }

            [DataMember(Name = "FCG")]
            public string FlightCategory
            {
                get;
                set;
            }
        }
        [DataContract]
        public class AvailabilityRequests
        {
            [DataMember(Name = "DSN")]
            public string DepartureStation
            {
                get;
                set;
            }
            [DataMember(Name = "ASN")]
            public string ArrivalStation
            {
                get;
                set;
            }
            [DataMember(Name = "FLD")]
            public string FlightDate
            {
                get;
                set;
            }
            [DataMember(Name = "FCO")]
            public string FareclassOption
            {
                get;
                set;
            }
            [DataMember(Name = "FTE")]
            public string FareType
            {
                get;
                set;
            }
            [DataMember]
            public string FlightNo
            {
                get;
                set;
            }
            [DataMember]
            public string Connection//DXB,MAA,BOM,DEL,ETC,,.
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Passengers
        {
            [DataMember(Name = "PXC")]
            public string PaxCount
            {
                get;
                set;
            }
            [DataMember(Name = "PXR")]
            public List<PaxTypeRefs> PaxTypeRef
            {
                get;
                set;
            }
        }
        [DataContract]
        public class PaxTypeRefs
        {
            [DataMember(Name = "PXT")]
            public string Type
            {
                get;
                set;
            }
            [DataMember(Name = "PXQ")]
            public string Quantity
            {
                get;
                set;
            }
        }
        [DataContract]
        public class AgentDetails
        {
            [DataMember(Name = "AGI")]
            public string AgentId
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
            [DataMember(Name = "TRI")]
            public string TerminalId
            {
                get;
                set;
            }
            [DataMember(Name = "UN")]
            public string UserName
            {
                get;
                set;
            }
            [DataMember(Name = "APP")]
            public string AppType
            {
                get;
                set;
            }
            [DataMember(Name = "VER")]
            public string Version
            {
                get;
                set;
            }
            [DataMember(Name = "ENV")]
            public string Environment
            {
                get;
                set;
            }
            [DataMember(Name = "BID")]
            public string BOAID
            {
                get;
                set;
            }
            [DataMember(Name = "BTID")]
            public string BOAterminalID
            {
                get;
                set;
            }
            [DataMember(Name = "AGTYP")]
            public string Agenttype
            {
                get;
                set;
            }
            [DataMember(Name = "CORID")]
            public string CoOrdinatorID
            {
                get;
                set;
            }
            [DataMember(Name = "SID")]
            public string AirportID
            {
                get;
                set;
            }
            [DataMember(Name = "BRID")]
            public string BranchID
            {
                get;
                set;
            }
            [DataMember(Name = "IBRID")]
            public string IssuingBranchID
            {
                get;
                set;
            }
            [DataMember(Name = "EMID")]
            public string EMP_ID
            {
                get;
                set;
            }
            [DataMember(Name = "CTCTR")]
            public string COST_CENTER
            {
                get;
                set;
            }
            [DataMember(Name = "IP")]
            public string Ipaddress
            {
                get;
                set;
            }
            [DataMember(Name = "seq")]
            public string Sequence
            {
                get;
                set;
            }
            [DataMember(Name = "BRG")]
            public string Bargain_Cred
            {
                get;
                set;
            }
            [DataMember(Name = "PBK")]
            public string Personal_Booking
            {
                get;
                set;
            }
            [DataMember(Name = "GFL")]
            public string GST_FLAG
            {
                get;
                set;
            }
            [DataMember]
            public string Platform//A->Agent ,B->BOA,C->CBT
            {
                get;
                set;
            }
            [DataMember]
            public string ProjectID
            {
                get;
                set;
            }
            [DataMember]
            public string UID
            {
                get;
                set;
            }
            [DataMember]
            public string Group_ID
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
            public string ClientName
            {
                get;
                set;
            }
        }

        [DataContract]
        public class Promocodes
        {
            [DataMember(Name = "CDE")]
            public string PromoCode
            {
                get;
                set;
            }
            [DataMember(Name = "FLT")]
            public string Flight
            {
                get;
                set;
            }
            [DataMember(Name = "CDTY")]
            public string CodeType
            {
                get;
                set;
            }
            [DataMember]
            public string OfficeId
            {
                get;
                set;
            }
        }

        #endregion

        #region Avail Response

        [DataContract]
        public class Status
        {
            [DataMember(Name = "RSC")]
            public string ResultCode
            {
                get;
                set;
            }
            [DataMember(Name = "ERR")]
            public string Error
            {
                get;
                set;
            }
            [DataMember(Name = "SEQ")]
            public String SequenceID
            {
                get;
                set;
            }
            [DataMember(Name = "EEC")]
            public String ErrorToCus
            {
                get;
                set;
            }
        }
        [XmlRoot(ElementName = "Itinerary")]
        [DataContract]
        public class AvailabilityRS
        {
            [DataMember(Name = "Er")]
            public string Error
            {
                get;
                set;
            }
            [DataMember(Name = "DEr")]
            public string DisplayError
            {
                get;
                set;
            }
            [DataMember(Name = "STA")]
            public Status Status
            {
                get;
                set;
            }
            [DataMember(Name = "RC")]
            public string ResultCode
            {
                get;
                set;
            }
            [DataMember(Name = "SEQ")]
            public string Sqe
            {
                get;
                set;
            }
            [DataMember(Name = "FC")]
            public HostCheck FareCheck
            {
                get;
                set;
            }
            [DataMember(Name = "CC")]
            public bool IsCurChange
            {
                get;
                set;
            }
            [XmlElement("Flights")]
            [DataMember(Name = "FL")]
            public List<Flights> Flights
            {
                get;
                set;
            }
            [XmlElement("HiddenFlights")]
            [DataMember(Name = "HFL")]
            public List<Flights> HiddenFlights
            {
                get;
                set;
            }
            [XmlElement("Fares")]
            [DataMember(Name = "FR")]
            public List<Fares> Fares
            {
                get;
                set;
            }
            [XmlElement("Meal")]
            [DataMember(Name = "ML")]
            public List<Meals> Meal
            {
                get;
                set;
            }
            [XmlElement("Bagg")]
            [DataMember(Name = "BG")]
            public List<Bagg> Bagg
            {
                get;
                set;
            }
            [XmlElement("Other")]
            [DataMember(Name = "OSSR")]
            public List<Meals> OtherSSR
            {
                get;
                set;
            }
            [XmlElement("HiddenFares")]
            [DataMember(Name = "HFR")]
            public List<Fares> HiddenFares
            {
                get;
                set;
            }
            [DataMember(Name = "TK")]
            public string Token
            {
                get;
                set;
            }
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
            [DataMember]
            public string ServiceFee
            {
                get;
                set;
            }
            //[DataMember]
            //public bool OfflineFlag
            //{
            //    get;
            //    set;
            //}

        }
        [DataContract]
        public class Flights
        {
            [DataMember(Name = "FCUR")]
            public string Meals
            {
                get;
                set;
            }
            [DataMember(Name = "RFN")]
            public string Refundable
            {
                get;
                set;
            }
            [DataMember(Name = "FBAG")]
            public string Baggage
            {
                get;
                set;
            }
            [DataMember(Name = "FN")]
            public string FlightNumber
            {
                get;
                set;
            }
            [DataMember(Name = "ORG")]
            public string Origin
            {
                get;
                set;
            }
            [DataMember(Name = "DES")]
            public string Destination
            {
                get;
                set;
            }
            [DataMember(Name = "STE")]
            public string StartTerminal
            {
                get;
                set;
            }
            [DataMember(Name = "DTE")]
            public string EndTerminal
            {
                get;
                set;
            }
            [DataMember(Name = "SDT")]
            public string DepartureDateTime
            {
                get;
                set;
            }
            [DataMember(Name = "EDT")]
            public string ArrivalDateTime
            {
                get;
                set;
            }
            [DataMember(Name = "CL")]
            public string Class
            {
                get;
                set;
            }
            [DataMember(Name = "CAB")]
            public string Cabin
            {
                get;
                set;
            }
            [DataMember(Name = "RBD")]
            public string RBDCode
            {
                get;
                set;
            }
            [DataMember(Name = "FBC")]
            public string FareBasisCode
            {
                get;
                set;
            }
            [DataMember(Name = "STP")]
            public string Stops
            {
                get;
                set;
            }
            [DataMember(Name = "VIA")]
            public string Via
            {
                get;
                set;
            }
            [DataMember(Name = "ALC")]
            public string AirlineCategory
            {
                get;
                set;
            }
            [DataMember(Name = "CNX")]
            public string CNX
            {
                get;
                set;
            }
            [DataMember(Name = "PC")]
            public string PlatingCarrier
            {
                get;
                set;
            }
            [DataMember(Name = "SGD")]
            public string SegmentDetails
            {
                get;
                set;
            }
            [DataMember(Name = "FYT")]
            public string FlyingTime
            {
                get;
                set;
            }
            [DataMember(Name = "JYT")]
            public string JourneyTime
            {
                get;
                set;
            }
            [DataMember(Name = "RTK")]
            public string ReferenceToken
            {
                get;
                set;
            }
            [DataMember(Name = "SGR")]
            public string SegRef
            {
                get;
                set;
            }
            [DataMember(Name = "ITN")]
            public string ItinRef
            {
                get;
                set;
            }
            [DataMember(Name = "VIAITN")]
            public string ViaItinRef
            {
                get;
                set;
            }
            [DataMember(Name = "CNF")]
            public string ConnectionFlag
            {
                get;
                set;
            }
            [DataMember(Name = "OFF")]
            public int OfflineFlag
            {
                get;
                set;
            }
            [DataMember(Name = "FRI")]
            public string FareId
            {
                get;
                set;
            }
            [DataMember(Name = "OFI")]
            public bool OfflineIndicator
            {
                get;
                set;
            }

            [DataMember(Name = "TNF")]
            public bool TransactionFlag
            {
                get;
                set;
            }
            [DataMember(Name = "OFR")]
            public string OtherFares
            {
                get;
                set;
            }
            [DataMember(Name = "MCL")]
            public string MultiClass
            {
                get;
                set;
            }
            [DataMember(Name = "FQT")]
            public bool AllowFQT
            {
                get;
                set;
            }
            [DataMember(Name = "AVS")]
            public string AvailSeat
            {
                get;
                set;
            }
            [DataMember(Name = "PDE")]
            public string PromoCode
            {
                get;
                set;
            }
            [DataMember(Name = "FRD")]
            public string FareTypeDescription
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Fares
        {
            [XmlElement("Faredescription")]
            [DataMember(Name = "FRD")]
            public List<Faredescription> Faredescription
            {
                get;
                set;
            }
            [DataMember(Name = "MCA")]
            public string MultiClassAmount
            {
                get;
                set;
            }
            [DataMember(Name = "CCY")]
            public string Currency
            {
                get;
                set;
            }
            [DataMember(Name = "API_CCY")]
            public string API_Currency
            {
                get;
                set;
            }
            [DataMember(Name = "ROE")]
            public string ROE_Value
            {
                get;
                set;
            }
            [DataMember(Name = "FLI")]
            public string FlightId
            {
                get;
                set;
            }
            [DataMember(Name = "FRID")]
            public string FareType
            {
                get;
                set;
            }
            [DataMember(Name = "TFR")]
            public string Tripfare
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Faredescription
        {

            [DataMember(Name = "PAX")]
            public string Paxtype
            {
                get;
                set;
            }
            [DataMember(Name = "BA")]
            public string BaseAmount
            {
                get;
                set;
            }
            [DataMember(Name = "TA")]
            public string TotalTaxAmount
            {
                get;
                set;
            }
            [DataMember(Name = "GA")]
            public string GrossAmount
            {
                get;
                set;
            }
            [DataMember(Name = "BRK")]
            public string API_Breakup
            {
                get;
                set;
            }
            [DataMember(Name = "CM")]
            public string Commission
            {
                get;
                set;
            }
            [DataMember(Name = "SCM")]
            public string SupplierCommission
            {
                get;
                set;
            }
            [DataMember(Name = "IC")]
            public string Incentive
            {
                get;
                set;
            }
            [DataMember(Name = "PLB")]
            public string PLB
            {
                get;
                set;
            }
            [DataMember(Name = "SC")]
            public string Servicecharge
            {
                get;
                set;
            }
            [DataMember(Name = "ST")]
            public string ServiceTax
            {
                get;
                set;
            }
            [DataMember(Name = "TDS")]
            public string TDS
            {
                get;
                set;
            }
            [DataMember(Name = "DIS")]
            public string Discount
            {
                get;
                set;
            }
            [DataMember(Name = "TNF")]
            public string TransactionFee
            {
                get;
                set;
            }
            [DataMember(Name = "MKP")]
            public string Markup
            {
                get;
                set;
            }
            [DataMember(Name = "HDMKP")]
            public string HIDDMARKUP
            {
                get;
                set;
            }
            [DataMember(Name = "ADDMKP")]
            public string AddMarkup
            {
                get;
                set;
            }
            [DataMember(Name = "ADE")]
            public string AgnDeal
            {
                get;
                set;
            }
            [DataMember(Name = "OLDFR")]
            public string OldFare
            {
                get;
                set;
            }
            [DataMember(Name = "OLDMKP")]
            public string OldMarkup
            {
                get;
                set;
            }
            [DataMember(Name = "BBO")]
            public bool BestBuyOption
            {
                get;
                set;
            }
            [DataMember(Name = "CMKP")]
            public string ClientMarkup
            {
                get;
                set;
            }
            [DataMember(Name = "TXDI")]
            public string Taxdifference
            {
                get;
                set;
            }
            [DataMember(Name = "BSDI")]
            public string Basedifference
            {
                get;
                set;
            }
            //[XmlElement("Tax")]
            [DataMember]
            public List<Taxes> Taxes
            {
                get;
                set;
            }
            [DataMember(Name = "RBD")]
            public string RBD
            {
                get;
                set;
            }
            [DataMember]
            public string ServiceFee
            {
                get;
                set;
            }

            public static Faredescription operator +(Faredescription _Faredescription1, Faredescription _Faredescription2)
            {
                Faredescription result = new Faredescription();
                try
                {
                    result.API_Breakup = "";// string.Join("/", (_Faredescription1.API_Breakup.TrimEnd('/') + _Faredescription2.API_Breakup.TrimEnd('/')).Split('/').GroupBy(e => e.Split(':')[0]).Select(f => f.Key + ":" + f.Select(g => Convert.ToDouble(g.Split(':')[1])).Sum()).ToArray());
                    result.BaseAmount = (Convert.ToDouble(_Faredescription1.BaseAmount) + Convert.ToDouble(_Faredescription2.BaseAmount)).ToString();
                    result.Commission = (Convert.ToDouble(_Faredescription1.Commission) + Convert.ToDouble(_Faredescription2.Commission)).ToString();
                    result.SupplierCommission = (Convert.ToDouble(_Faredescription1.SupplierCommission) + Convert.ToDouble(_Faredescription2.SupplierCommission)).ToString();
                    result.Discount = (Convert.ToDouble(_Faredescription1.Discount) + Convert.ToDouble(_Faredescription2.Discount)).ToString();
                    result.GrossAmount = (Convert.ToDouble(_Faredescription1.GrossAmount) + Convert.ToDouble(_Faredescription2.GrossAmount)).ToString();
                    result.Incentive = (Convert.ToDouble(_Faredescription1.Incentive) + Convert.ToDouble(_Faredescription2.Incentive)).ToString();
                    result.Markup = (Convert.ToDouble(_Faredescription1.Markup) + Convert.ToDouble(_Faredescription2.Markup)).ToString();
                    result.HIDDMARKUP = (Convert.ToDouble(_Faredescription1.HIDDMARKUP) + Convert.ToDouble(_Faredescription2.HIDDMARKUP)).ToString();
                    result.ClientMarkup = (Convert.ToDouble(_Faredescription1.ClientMarkup) + Convert.ToDouble(_Faredescription2.ClientMarkup)).ToString();
                    result.Paxtype = _Faredescription1.Paxtype;
                    result.Servicecharge = (Convert.ToDouble(_Faredescription1.Servicecharge) + Convert.ToDouble(_Faredescription2.Servicecharge)).ToString();
                    result.ServiceTax = (Convert.ToDouble(_Faredescription1.ServiceTax) + Convert.ToDouble(_Faredescription2.ServiceTax)).ToString();
                    result.Taxes = new List<Taxes>();
                    result.Taxes.AddRange(_Faredescription1.Taxes);
                    result.Taxes.AddRange(_Faredescription2.Taxes);
                    result.Taxes = result.Taxes.GroupBy(f => f.Code).Select(r => new Taxes() { Code = r.Key, Amount = r.Select(e => Convert.ToDouble(e.Amount)).Sum().ToString() }).ToList();
                    result.TDS = (Convert.ToDouble(_Faredescription1.TDS) + Convert.ToDouble(_Faredescription2.TDS)).ToString();
                    result.TotalTaxAmount = (Convert.ToDouble(_Faredescription1.TotalTaxAmount) + Convert.ToDouble(_Faredescription2.TotalTaxAmount)).ToString();
                    result.TransactionFee = (Convert.ToDouble(_Faredescription1.TransactionFee) + Convert.ToDouble(_Faredescription2.TransactionFee)).ToString();
                    result.AddMarkup = (Convert.ToDouble(_Faredescription1.AddMarkup) + Convert.ToDouble(_Faredescription2.AddMarkup)).ToString();
                    result.Taxdifference = (Convert.ToDouble(_Faredescription1.Taxdifference) + Convert.ToDouble(_Faredescription2.Taxdifference)).ToString();
                    result.Basedifference = (Convert.ToDouble(_Faredescription1.Basedifference) + Convert.ToDouble(_Faredescription2.Basedifference)).ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    result.Paxtype = ex.Message.ToString();
                    return result;
                }
            }

        }
        [DataContract]
        public class Taxes
        {
            [DataMember(Name = "COD")]
            public string Code
            {
                get;
                set;
            }
            [DataMember(Name = "AMT")]
            public string Amount
            {
                get;
                set;
            }
            public static Taxes operator +(Taxes Taxes1, Taxes Taxes2)
            {
                return new Taxes() { Code = Taxes1.Code, Amount = (Convert.ToDouble(Taxes1.Amount) + Convert.ToDouble(Taxes2.Amount)).ToString() };
            }
        }
        [DataContract]
        public class HostCheck
        {
            [DataMember(Name = "STK")]
            public string Stocktype
            {
                get;
                set;
            }
            [DataMember(Name = "CHF")]
            public string CheckFlag
            {
                get;
                set;
            }
            [DataMember(Name = "TCK")]
            public string TicketType
            {
                get;
                set;
            }
        }

        #endregion


        #region FarePrice Request
        [DataContract]
        public class PriceItineary
        {
            [DataMember]
            public AgentDetails AgentDetails
            {
                get;
                set;
            }
            [DataMember]
            public Credential Credentials
            {
                get;
                set;
            }
            [DataMember(Name = "VIAF")]
            public bool ViaFlag
            {
                get;
                set;
            }
            [DataMember]
            public List<Itineraries> ItinearyDetails
            {
                get;
                set;
            }
            [DataMember]
            public SegmentDetails SegmnetDetails
            {
                get;
                set;
            }
            [DataMember]
            public string Token
            {
                get;
                set;
            }
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
            [DataMember]
            public bool BestBuyOption
            {
                get;
                set;
            }
            [DataMember]
            public string AlterQueue
            {
                get;
                set;
            }
            [DataMember]
            public string Nationality
            {
                get;
                set;
            }
            [DataMember]
            public string Currency
            {
                get;
                set;
            }
            [DataMember]
            public string SequenceID
            {
                get;
                set;
            }
            [DataMember]
            public string TerminalType
            {
                get;
                set;
            }
            [DataMember]
            public string IpAddress
            {
                get;
                set;
            }
            [DataMember]
            public bool IsDiscount
            {
                get;
                set;
            }
            [DataMember]
            public string DiscountFlag
            {
                get;
                set;
            }
            [DataMember(Name = "WTL")]
            public bool ISWaitList
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Itineraries
        {
            [DataMember]
            public List<Flights> FlightDetails
            {
                get;
                set;
            }
            [DataMember]
            public string BaseAmount
            {
                get;
                set;
            }
            [DataMember]
            public string GrossAmount
            {
                get;
                set;
            }
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
            [DataMember]
            public string Pricingcode
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
            [DataMember]
            public string FareTypeDescription
            {
                get;
                set;
            }
        }
        [DataContract]
        public class SegmentDetails
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
            public string TripType
            {
                get;
                set;
            }
            [DataMember]
            public string RTSpecial
            {
                get;
                set;
            }
            [DataMember]
            public string ClassType
            {
                get;
                set;
            }
            [DataMember]
            public string BookingType
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
            public string Adult
            {
                get;
                set;
            }
            [DataMember]
            public string Child
            {
                get;
                set;
            }
            [DataMember]
            public string Infant
            {
                get;
                set;
            }
        }

        #endregion

        #region FarePrice Response
        public class PriceItenaryRS
        {
            [DataMember]
            public List<PriceItenary> PriceItenarys
            {
                get;
                set;
            }
            [DataMember]
            public string Error
            {
                get;
                set;
            }
            [DataMember]
            public string ResultCode
            {
                get;
                set;
            }
            [DataMember]
            public string Sqe
            {
                get;
                set;
            }
        }
        [DataContract]
        public class PriceItenary
        {
            [DataMember(Name = "ASM")]
            public string AllowSeatMap
            {
                get;
                set;
            }

            [DataMember(Name = "ABPNR")]
            public string AllowBlockPNR
            {
                get;
                set;
            }
            [DataMember(Name = "DOBMand")]
            public string DOBMandatory
            {
                get;
                set;
            }
            [DataMember(Name = "PASMand")]
            public string PassportMandatory
            {
                get;
                set;
            }
            [DataMember(Name = "PEr")]
            public string Error
            {
                get;
                set;
            }
            [DataMember(Name = "PRC")]
            public string ResultCode
            {
                get;
                set;
            }
            [DataMember(Name = "SEQ")]
            public string Sqe
            {
                get;
                set;
            }
            [DataMember(Name = "PTK")]
            public string Token
            {
                get;
                set;
            }
            [DataMember(Name = "PKCD")]
            public string Pricingcode
            {
                get;
                set;
            }
            [DataMember(Name = "BRFG")]
            public string Bargainflag
            {
                get;
                set;
            }
            [DataMember(Name = "PKCDTP")]
            public string PricingcodeType
            {
                get;
                set;
            }
            [DataMember(Name = "TDCODE")]
            public List<Dealcodes> Tdcode
            {
                get;
                set;
            }
            [DataMember(Name = "PRS")]
            public List<AvailabilityRS> PriceRS
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Dealcodes
        {
            [DataMember(Name = "DealFlag")]
            public string DealFlag
            {
                get;
                set;
            }
            [DataMember(Name = "DealCode")]
            public string DealCode
            {
                get;
                set;
            }
            [DataMember(Name = "DealTypeName")]
            public string DealTypeName
            {
                get;
                set;
            }
        }
        #endregion


        #region Booking Request
        [DataContract]
        public class BookingRquest
        {
            [DataMember]
            public AgentDetails Agent
            {
                get;
                set;
            }
            [DataMember]
            public bool Viaflag
            {
                get;
                set;
            }
            [DataMember]
            public Credential Credentials
            {
                get;
                set;
            }
            [DataMember]
            public string Token
            {
                get;
                set;
            }
            [DataMember]
            public string Genraltkn
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
            public List<ItineraryFlights> ItineraryFlights
            {
                get;
                set;
            }
            [DataMember]
            public List<PaxDetails> PaxDetails
            {
                get;
                set;
            }
            [DataMember]
            public List<FFNumber> FFNumber
            {
                get;
                set;
            }
            [DataMember]
            public List<MealsSSR> MealsSSR
            {
                get;
                set;
            }
            [DataMember]
            public List<BaggSSR> BaggSSR
            {
                get;
                set;
            }
            [DataMember]
            public List<SeatsSSR> SeatsSSR
            {
                get;
                set;
            }
            [DataMember]
            public List<Payment> Payment
            {
                get;
                set;
            }
            [DataMember]
            public List<Dealcode> Dealcode
            {
                get;
                set;
            }
            [DataMember]
            public ContactDetails AddressDetails
            {
                get;
                set;
            }
            [DataMember]
            public GST_DETAIS GstDetails
            {
                get;
                set;
            }
            [DataMember]
            public CBT_Credentials CBT_Input
            {
                get;
                set;
            }


            [DataMember]
            public FOPDetails FOPDetails
            {
                get;
                set;
            }
            [DataMember]
            public string PaxCount
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
            [DataMember]
            public string Currency
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
            public string TerminalType
            {
                get;
                set;
            }
            [DataMember]
            public string TourCode
            {
                get;
                set;
            }
            [DataMember]
            public bool BlockPNR
            {
                get;
                set;
            }
            [DataMember]
            public string PaymentMode
            {
                get;
                set;
            }
            [DataMember]
            public string PaymentrefID
            {
                get;
                set;
            }
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
            public bool ReBook
            {
                get;
                set;
            }
            [DataMember]
            public bool Multi_Track_Create
            {
                get;
                set;
            }
            [DataMember]
            public string TrackId
            {
                get;
                set;
            }
            [DataMember]
            public bool Track_Create
            {
                get;
                set;
            }
            [DataMember]
            public string SegmentClass
            {
                get;
                set;
            }
            [DataMember]
            public bool BestBuyOption
            {
                get;
                set;
            }
            [DataMember]
            public string QueueFlag
            {
                get;
                set;
            }
            [DataMember]
            public string Booking_Type
            {
                get;
                set;
            }

        }
        [DataContract]
        public class CBT_Credentials
        {
            [DataMember]
            public string EmpCode
            {
                get;
                set;
            }

            [DataMember]
            public string Department
            {
                get;
                set;
            }

            [DataMember]
            public string Chargeabilty
            {
                get;
                set;
            }

            [DataMember]
            public string ProjectCode
            {
                get;
                set;
            }

            [DataMember]
            public string CustLocation
            {
                get;
                set;
            }

            [DataMember]
            public string ApprovedBy
            {
                get;
                set;
            }

            [DataMember]
            public string PersonalBooking
            {
                get;
                set;
            }

            [DataMember]
            public string TRNumber
            {
                get;
                set;
            }
            [DataMember]
            public string VesselName
            {
                get;
                set;
            }
            [DataMember]
            public string Budgetcode
            {
                get;
                set;
            }
            [DataMember]
            public string CostCenter
            {
                get;
                set;
            }
            [DataMember]
            public string CorpEmpCode
            {
                get;
                set;
            }
            [DataMember]//Add by hari..
            public List<ERP_Attribute> ERP_Attributes
            {
                get;
                set;
            }


        }
        public class ERP_Attribute
        {
            [DataMember(IsRequired = true)]
            public string PaxRefNumber { get; set; }
            [DataMember]
            public string AttributesName { get; set; }
            [DataMember]
            public string AttributesValue { get; set; }
        }


        [DataContract]
        public class GST_DETAIS
        {
            [DataMember]
            public string GSTNumber
            {
                get;
                set;
            }
            [DataMember]
            public string GSTCompanyName
            {
                get;
                set;
            }
            [DataMember]
            public string GSTAddress
            {
                get;
                set;
            }
            [DataMember]
            public string GSTEmailID
            {
                get;
                set;
            }
            [DataMember]
            public string GSTMobileNumber
            {
                get;
                set;
            }

            [DataMember]
            public string GSTCityCode
            {
                get;
                set;
            }
            [DataMember]
            public string GSTStateCode
            {
                get;
                set;
            }
            [DataMember]
            public string GSTPincode
            {
                get;
                set;
            }


        }
        [DataContract]
        public class FOPDetails
        {
            [DataMember]
            public string CardNumber
            {
                get;
                set;
            }
            [DataMember]
            public string CVV_Number
            {
                get;
                set;
            }
            [DataMember]
            public string ExpiryDate
            {
                get;
                set;
            }
            [DataMember]
            public string CardName
            {
                get;
                set;
            }
            [DataMember]
            public string CardType
            {
                get;
                set;
            }
        }
        [DataContract]
        public class FFNumber
        {
            [DataMember]
            public string SegRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string PaxRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string AirlineCode
            {
                get;
                set;
            }
            [DataMember]
            public string FlyerNumber
            {
                get;
                set;
            }
            [DataMember]
            public string Itinref
            {
                get;
                set;
            }
        }
        [DataContract]
        public class PaxDetails
        {
            [DataMember]
            public string PaxRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string Title
            {
                get;
                set;
            }
            [DataMember]
            public string FirstName
            {
                get;
                set;
            }
            [DataMember]
            public string LastName
            {
                get;
                set;
            }
            [DataMember]
            public string Age
            {
                get;
                set;
            }
            [DataMember]
            public string DOB
            {
                get;
                set;
            }
            [DataMember]
            public string Gender
            {
                get;
                set;
            }
            [DataMember]
            public string PaxType
            {
                get;
                set;
            }
            [DataMember]
            public string PassportNo
            {
                get;
                set;
            }
            [DataMember]
            public string PassportExpiry
            {
                get;
                set;
            }
            [DataMember]
            public string PlaceOfBirth
            {
                get;
                set;
            }

            [DataMember]
            public string InfantRef
            {
                get;
                set;
            }
            [DataMember]
            public string Mobnumber
            {
                get;
                set;
            }

            [DataMember]
            public string MailID
            {
                get;
                set;
            }
            [DataMember]
            public string ProofType
            {
                get;
                set;
            }
            [DataMember]
            public bool WheelChair
            {
                get;
                set;
            }
            [DataMember]
            public string Berth
            {
                get;
                set;
            }
            [DataMember]
            public string CorpRefID
            {
                get;
                set;
            }
            [DataMember]
            public string EmpCostCenter
            {
                get;
                set;
            }
            [DataMember]
            public string PaxInfoType
            {
                get;
                set;
            }
            [DataMember]
            public string EMP_ID
            {
                get;
                set;
            }

        }
        [DataContract]
        public class MealsSSR
        {
            [DataMember]
            public string SegRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string PaxRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string MealCode
            {
                get;
                set;
            }
            [DataMember]
            public string MealAmount
            {
                get;
                set;
            }
            [DataMember]
            public string Orgin
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
            public string Description
            {
                get;
                set;
            }
            [DataMember]
            public string Itinref
            {
                get;
                set;
            }
            [DataMember]
            public string markup
            {
                get;
                set;
            }
        }
        [DataContract]
        public class BaggSSR
        {
            [DataMember]
            public string SegRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string PaxRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string BaggCode
            {
                get;
                set;
            }
            [DataMember]
            public string BaggAmount
            {
                get;
                set;
            }
            [DataMember]
            public string Description
            {
                get;
                set;
            }
            [DataMember]
            public string Amount
            {
                get;
                set;
            }
            [DataMember]
            public string Orgin
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
            public string Itinref
            {
                get;
                set;
            }
            [DataMember]
            public string markup
            {
                get;
                set;
            }
        }
        [DataContract]
        public class SeatsSSR
        {
            [DataMember]
            public string SegRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string PaxRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string SeatCode
            {
                get;
                set;
            }
            [DataMember]
            public string SeatAmount
            {
                get;
                set;
            }
            [DataMember]
            public string Orgin
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
            public string Description
            {
                get;
                set;
            }
            [DataMember]
            public string Itinref
            {
                get;
                set;
            }
            [DataMember]
            public string markup
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Payment
        {
            [DataMember]
            public string SegRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string Amount
            {
                get;
                set;
            }
            [DataMember]
            public string Totalmrkup
            {
                get;
                set;
            }
            [DataMember]
            public string Token
            {
                get;
                set;
            }
        }
        [DataContract]
        public class ItineraryFlights
        {
            [DataMember]
            public string Pricingcode
            {
                get;
                set;
            }
            [DataMember]
            public string Select_Token
            {
                get;
                set;
            }
            [DataMember]
            public bool IsAlreadyBooked
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
            [DataMember]
            public string FareTypeDescription
            {
                get;
                set;
            }
            [DataMember]
            public string ParentTrackid
            {
                get;
                set;
            }
            [DataMember]
            public string TrackId
            {
                get;
                set;
            }
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
            [DataMember]
            public string HostID
            {
                get;
                set;
            }
            [DataMember]
            public List<Flights> FlightDetails
            {
                get;
                set;
            }
            [DataMember]
            public string PaymentMode
            {
                get;
                set;
            }
            [DataMember]
            public FOPDetails FOPDetails
            {
                get;
                set;
            }
            [DataMember]
            public string IssueTicket
            {
                get;
                set;
            }
            [DataMember]
            public List<FFNumber> FFNumber
            {
                get;
                set;
            }
            [DataMember]
            public List<MealsSSR> MealsSSR
            {
                get;
                set;
            }
            [DataMember]
            public List<BaggSSR> BaggSSR
            {
                get;
                set;
            }
            [DataMember]
            public List<SeatsSSR> SeatsSSR
            {
                get;
                set;
            }
            [DataMember]
            public List<Payment> Payment
            {
                get;
                set;
            }
            [DataMember]
            public List<Dealcodes> Dealcode
            {
                get;
                set;
            }
            [DataMember]
            public List<ERP_Attribute> ERP_Attributes
            {
                get;
                set;
            }
            [DataMember]
            public string OSEntry
            {
                get;
                set;
            }
        }
        [DataContract]
        public class ContactDetails
        {
            [DataMember]
            public string Firstname
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
            public string AgencyName
            {
                get;
                set;
            }
            [DataMember]
            public string CountryCode
            {
                get;
                set;
            }
            [DataMember]
            public string ContactNumber
            {
                get;
                set;
            }
            [DataMember]
            public string EmailID
            {
                get;
                set;
            }
            [DataMember]
            public string Address
            {
                get;
                set;
            }
            [DataMember]
            public string Lastname
            {
                get;
                set;
            }
            [DataMember]
            public string Title
            {
                get;
                set;
            }
            [DataMember]
            public string Remarks
            {
                get;
                set;
            }
            [DataMember]
            public string CountryID
            {
                get;
                set;
            }
            [DataMember]
            public string StateID
            {
                get;
                set;
            }
            [DataMember]
            public string CityId
            {
                get;
                set;
            }
            [DataMember]
            public string PhoneAgency
            {
                get;
                set;
            }
            [DataMember]
            public string PhoneHome
            {
                get;
                set;
            }
            [DataMember]
            public string PhoneBusiness
            {
                get;
                set;
            }
            [DataMember]
            public string Pincode
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Credential
        {
            [DataMember]
            public string UserName
            {
                get;
                set;
            }
            [DataMember]
            public string UserID
            {
                get;
                set;
            }

            [DataMember]
            public string Password
            {
                get;
                set;
            }
            [DataMember]
            public string IP
            {
                get;
                set;
            }
            [DataMember]
            public string SeqID
            {
                get;
                set;
            }
            [DataMember]
            public string TerminalType
            {
                get;
                set;
            }
            [DataMember]
            public string LogType
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
            public string Key
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Dealcode
        {
            [DataMember(Name = "DealFlag")]
            public string DealFlag
            {
                get;
                set;
            }
            [DataMember(Name = "DealCode")]
            public string DealCode
            {
                get;
                set;
            }
            //[DataMember(Name = "DealTypeName")]
            //public string DealTypeName
            //{
            //    get;
            //    set;
            //}
        }
        #endregion

        #region Booking Response
        [DataContract]
        public class BookingRS
        {
            [DataMember(Name = "BEr")]
            public string Error
            {
                get;
                set;
            }
            [DataMember(Name = "BRC")]
            public string ResultCode
            {
                get;
                set;
            }
            [DataMember(Name = "SEQ")]
            public string Sqe
            {
                get;
                set;
            }
            [DataMember(Name = "BPNR")]
            public List<PnrDetails> PnrDetails
            {
                get;
                set;
            }
            [DataMember(Name = "RPC")]
            public PriceItenaryRS PriceItineraryRs
            {
                get;
                set;
            }

            [DataMember(Name = "BAV")]
            public AvailabilityRS AvailRs
            {
                get;
                set;
            }

            [DataMember(Name = "TRK")]
            public string TrackId
            {
                get;
                set;
            }
        }
        [DataContract]
        public class PnrDetails
        {
            [DataMember(Name = "PSID")]
            public string SPNR
            {
                get;
                set;
            }
            [DataMember(Name = "PAID")]
            public string AIRLINEPNR
            {
                get;
                set;
            }
            [DataMember(Name = "SPID")]
            public string SupplierPNR
            {
                get;
                set;
            }
            [DataMember(Name = "PAC")]
            public string AIRLINECODE
            {
                get;
                set;
            }
            [DataMember(Name = "PFN")]
            public string FLIGHTNO
            {
                get;
                set;
            }
            [DataMember(Name = "PORG")]
            public string ORIGIN
            {
                get;
                set;
            }
            [DataMember(Name = "PDES")]
            public string DESTINATION
            {
                get;
                set;
            }
            [DataMember(Name = "PTIT")]
            public string TITLE
            {
                get;
                set;
            }
            [DataMember(Name = "PLTN")]
            public string LASTNAME
            {
                get;
                set;
            }
            [DataMember(Name = "PFTN")]
            public string FIRSTNAME
            {
                get;
                set;
            }
            [DataMember(Name = "PPAX")]
            public string PAXTYPE
            {
                get;
                set;
            }
            [DataMember(Name = "PDEP")]
            public string DEPARTUREDATE
            {
                get;
                set;
            }
            [DataMember(Name = "PARR")]
            public string ARRIVALDATE
            {
                get;
                set;
            }
            [DataMember(Name = "PDOB")]
            public string DATEOFBIRTH
            {
                get;
                set;
            }
            [DataMember(Name = "PTKT")]
            public string TICKETNO
            {
                get;
                set;
            }
            [DataMember(Name = "PCRS")]
            public string CRSPNR
            {
                get;
                set;
            }
            [DataMember(Name = "PCLS")]
            public string CLASS
            {
                get;
                set;
            }
            [DataMember(Name = "PFBC")]
            public string FAREBASISCODE
            {
                get;
                set;
            }
            [DataMember(Name = "PBF")]
            public string BASICFARE
            {
                get;
                set;
            }
            [DataMember(Name = "PGF")]
            public string GROSSFARE
            {
                get;
                set;
            }
            [DataMember(Name = "PTX")]
            public string TAXFARE
            {
                get;
                set;
            }
            [DataMember(Name = "PTA")]
            public string TAXAMOUNT
            {
                get;
                set;
            }
            [DataMember(Name = "PFR")]
            public string FAREID
            {
                get;
                set;
            }
            [DataMember(Name = "PCM")]
            public string COMMISSION
            {
                get;
                set;
            }
            [DataMember(Name = "PIC")]
            public string INCENTIVE
            {
                get;
                set;
            }
            [DataMember(Name = "PSC")]
            public string SERVICECHARGE
            {
                get;
                set;
            }
            [DataMember(Name = "PST")]
            public string SERVICETAX
            {
                get;
                set;
            }
            [DataMember(Name = "PMKP")]
            public string MARKUP
            {
                get;
                set;
            }
            [DataMember(Name = "HDMKP")]
            public string HIDDMARKUP
            {
                get;
                set;
            }
            [DataMember(Name = "ADDMKP")]
            public string ADDMARKUP
            {
                get;
                set;
            }
            [DataMember(Name = "PTF")]
            public string TRANSACTIONFEE
            {
                get;
                set;
            }
            [DataMember(Name = "PDIS")]
            public string DISCOUNT
            {
                get;
                set;
            }
            [DataMember(Name = "PTDS")]
            public string TDS
            {
                get;
                set;
            }
            [DataMember(Name = "PMLA")]
            public string MEALSAMOUNT
            {
                get;
                set;
            }
            [DataMember(Name = "PALC")]
            public string AIRLINECATEGORY
            {
                get;
                set;
            }
            [DataMember(Name = "PUT")]
            public string USERTRACKID
            {
                get;
                set;
            }
            [DataMember(Name = "POFC")]
            public string OFFICEID
            {
                get;
                set;
            }
            [DataMember(Name = "PTC")]
            public string TICKETINGCARRIER
            {
                get;
                set;
            }
            [DataMember(Name = "PFQT")]
            public string FQTV
            {
                get;
                set;
            }
            [DataMember(Name = "PML")]
            public string MEALS
            {
                get;
                set;
            }
            [DataMember(Name = "PBG")]
            public string BAGGAGE
            {
                get;
                set;
            }
            [DataMember(Name = "PREF")]
            public string REFERENCE
            {
                get;
                set;
            }
            [DataMember(Name = "PSEQ")]
            public string SEQNO
            {
                get;
                set;
            }
            [DataMember(Name = "PITN")]
            public string ITINREF
            {
                get;
                set;
            }
            [DataMember(Name = "PTRP")]
            public string TRIPTYPE
            {
                get;
                set;
            }
            [DataMember(Name = "PTK")]
            public string TOKEN
            {
                get;
                set;
            }
            [DataMember(Name = "POFF")]
            public int OFFLINEFLAG
            {
                get;
                set;
            }
            [DataMember(Name = "PSTA")]
            public string SEATSAMOUNT
            {
                get;
                set;
            }
            [DataMember(Name = "CMKP")]
            public string CLIENTMARKUP
            {
                get;
                set;
            }
        }
        #endregion

        #region Block Ticket Request
        [DataContract]
        public class TicketingRQ
        {
            [DataMember]
            public string CRSPNR
            {
                get;
                set;
            }
            [DataMember]
            public string AIRLINEPNR
            {
                get;
                set;
            }
            [DataMember]
            public string PlaitingCarrier
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
            public string Payment
            {
                get;
                set;
            }
            [DataMember]
            public string TerminalType
            {
                get;
                set;
            }
            [DataMember]
            public bool BlockFlag
            {
                get;
                set;
            }
            [DataMember]
            public AgentDetails Agent
            {
                get;
                set;
            }
            [DataMember]
            public string PaymentMode
            {
                get;
                set;
            }
            [DataMember]
            public string PaymentrefID
            {
                get;
                set;
            }

            [DataMember]
            public string SPNR
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
            public string Category
            {
                get;
                set;
            }
            [DataMember]
            public string HostID
            {
                get;
                set;
            }
            [DataMember]
            public string RawData
            {
                get;
                set;
            }
        }
        #endregion

        #region HexwarePushData
        [DataContract]
        public class BookingDetails
        {
            [DataMember]
            public string Authorizationkey { get; set; }
            [DataMember]
            public string EmployeeID { get; set; }
            [DataMember]
            public string RequestID { get; set; }
            [DataMember]
            public string TripID { get; set; }
            [DataMember]
            public string RequestType { get; set; }
            [DataMember]
            public string CorporateID { get; set; }
            [DataMember]
            public string SPNR { get; set; }


        }
        #endregion

        [DataContract]
        public class PaxDetailsHuntman
        {
            [DataMember]
            public string PaxRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string Title
            {
                get;
                set;
            }
            [DataMember]
            public string FirstName
            {
                get;
                set;
            }
            [DataMember]
            public string LastName
            {
                get;
                set;
            }
            [DataMember]
            public string Age
            {
                get;
                set;
            }
            [DataMember]
            public string DOB
            {
                get;
                set;
            }
            [DataMember]
            public string Gender
            {
                get;
                set;
            }
            [DataMember]
            public string PaxType
            {
                get;
                set;
            }
            [DataMember]
            public string PassportNo
            {
                get;
                set;
            }
            [DataMember]
            public string PassportExpiry
            {
                get;
                set;
            }
            [DataMember]
            public string PlaceOfBirth
            {
                get;
                set;
            }
            [DataMember]
            public string InfantRef
            {
                get;
                set;
            }
            [DataMember]
            public string Mobnumber
            {
                get;
                set;
            }
            [DataMember]
            public string MailID
            {
                get;
                set;
            }
            [DataMember]
            public string Berth
            {
                get;
                set;
            }
            [DataMember]
            public string Meals
            {
                get;
                set;
            }
        }
        public class Expecedt
        {
            [DataMember]
            public List<ADULTPASSDETAILss> EVENTss
            {
                get;
                set;
            }
        }
        [DataContract]
        public class ADULTPASSDETAILss
        {
            [DataMember]
            public string Date
            {
                get;
                set;
            }
            [DataMember]
            public string AccountHead
            {
                get;
                set;
            }
            [DataMember]
            public string Subgroup
            {
                get;
                set;
            }
            [DataMember]
            public string Description
            {
                get;
                set;
            }
            [DataMember]
            public string Amount
            {
                get;
                set;
            }
        }
        public class PaxDetailsHotel
        {
            [DataMember]
            public EVENT EVENT
            {
                get;
                set;
            }
        }
        [DataContract]
        public class EVENT
        {
            [DataMember]
            public ADULTPASSDETAIL ADULTPASSDETAIL
            {
                get;
                set;
            }
        }
        [DataContract]
        public class ADULTPASSDETAIL
        {
            [DataMember]
            public string PAXTYPE
            {
                get;
                set;
            }
            [DataMember]
            public string PAXTITLE
            {
                get;
                set;
            }
            [DataMember]
            public string FIRSTNAME
            {
                get;
                set;
            }
            [DataMember]
            public string LASTNAME
            {
                get;
                set;
            }
            [DataMember]
            public string GENDER
            {
                get;
                set;
            }
            [DataMember]
            public string AGE
            {
                get;
                set;
            }
            [DataMember]
            public string LANDLINE
            {
                get;
                set;
            }
            [DataMember]
            public string MOBILE
            {
                get;
                set;
            }
            [DataMember]
            public string EMAIL
            {
                get;
                set;
            }
            [DataMember]
            public string NO_OF_ROOMS
            {
                get;
                set;
            }
            [DataMember]
            public string ADT_PER_ROOM
            {
                get;
                set;
            }
            [DataMember]
            public string CHD_PER_ROOM
            {
                get;
                set;
            }
            [DataMember]
            public string CHILDAGE
            {
                get;
                set;
            }
            [DataMember]
            public string SPECIALREQUEST
            {
                get;
                set;
            }
            [DataMember]
            public string REMARKS
            {
                get;
                set;
            }
            [DataMember]
            public string PAXCOUNT
            {
                get;
                set;
            }
            [DataMember]
            public string COUNTRYCODE
            {
                get;
                set;
            }
            [DataMember]
            public string COUNTRYNAME
            {
                get;
                set;
            }
            [DataMember]
            public string STATE
            {
                get;
                set;
            }
            [DataMember]
            public string CITY
            {
                get;
                set;
            }
            [DataMember]
            public string ADDRESS
            {
                get;
                set;
            }
        }



        #region FareRule Request & Responce
        [DataContract]
        public class FareRuleRQ
        {
            [DataMember]
            public AgentDetails Agent //ABCD
            {
                get;
                set;
            }
            [DataMember]
            public List<Flights> FlightDetails
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
            [DataMember]
            public string PlatingCarrier
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
            [DataMember]
            public string Cabin
            {
                get;
                set;
            }
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
            public string Stock
            {
                get;
                set;
            }

        }
        [DataContract]
        public class FareRuleRS
        {
            [JsonProperty("FLRC")]
            public string ResultCode
            {
                get;
                set;
            }
            [DataMember(Name = "SEQ")]
            public string Sqe
            {
                get;
                set;
            }
            [JsonProperty("FLRr")]
            public string Error
            {
                get;
                set;
            }
            [JsonProperty("FLTx")]
            public string FareRuleText
            {
                get;
                set;
            }
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
        }
        #endregion

        #region  Read PNR
        [DataContract]
        public class ReadPnrRQ
        {
            [DataMember]
            public AgentDetails Agent //ABCD
            {
                get;
                set;
            }
            [DataMember]
            public ContactDetails AddressDetails
            {
                get;
                set;
            }
            [DataMember]
            public string CRSPnr
            {
                get;
                set;
            }
            [DataMember]
            public string AirlinePnr
            {
                get;
                set;
            }
            [DataMember]
            public string FirstName
            {
                get;
                set;
            }
            [DataMember]
            public string LastName
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
            public string CRSID
            {
                get;
                set;
            }
            [DataMember]
            public bool TicketPnr
            {
                get;
                set;
            }
            [DataMember]
            public string TerminalType
            {
                get;
                set;
            }
            [DataMember]
            public string PaymentMode
            {
                get;
                set;
            }
        }
        [DataContract]
        public class ReadPnrRS
        {

            [DataMember]
            public string ResultCode
            {
                get;
                set;
            }
            [DataMember]
            public string Error
            {
                get;
                set;
            }
            [DataMember]
            public string PnrDetails
            {
                get;
                set;
            }
        }
        #endregion

        #region MultiClass Avail
        [DataContract]
        public class MultiClassRQ
        {
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
            [DataMember]
            public AgentDetails Agent //ABCD
            {
                get;
                set;
            }
            [DataMember]
            public List<Flights> Flight
            {
                get;
                set;
            }
            [DataMember]
            public int AdultCount
            {
                get;
                set;
            }
            [DataMember]
            public int ChildCount
            {
                get;
                set;
            }
            [DataMember]
            public int InfantCount
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
            [DataMember]
            public string SegmentType
            {
                get;
                set;
            }
        }
        [DataContract]
        public class MultiClassRS
        {
            [JsonProperty("MCRC")]
            public string ResultCode
            {
                get;
                set;
            }
            [JsonProperty("SEQ")]
            public string Sqe
            {
                get;
                set;
            }
            [JsonProperty("MCEr")]
            public string Error
            {
                get;
                set;
            }
            [XmlElement("Flights")]
            [JsonProperty("MCFL")]
            public List<Flights> Flights
            {
                get;
                set;
            }
            [XmlElement("Fares")]
            [JsonProperty("MCFR")]
            public List<Fares> Fares
            {
                get;
                set;
            }

        }
        #endregion

        #region Meals
        [DataContract]
        public class Meals
        {

            [JsonProperty("MOG")]
            public string Orgin
            {
                get;
                set;
            }
            [JsonProperty("MDS")]
            public string Destination
            {
                get;
                set;
            }
            [JsonProperty("MLS")]
            public string SegRef
            {
                get;
                set;
            }
            [JsonProperty("MLC")]
            public string Code
            {
                get;
                set;
            }
            [JsonProperty("MLD")]
            public string Description
            {
                get;
                set;
            }
            [JsonProperty("MLA")]
            public string Amount
            {
                get;
                set;
            }
            [JsonProperty("URL")]
            public string Url
            {
                get;
                set;
            }
            [JsonProperty("CGY")]
            public string Category
            {
                get;
                set;
            }

            [DataMember(Name = "MITN")]
            public string Itinref
            {
                get;
                set;
            }
        }
        #endregion

        #region Baggage

        [DataContract]
        public class Bagg
        {

            [JsonProperty("BOG")]
            public string Orgin
            {
                get;
                set;
            }
            [JsonProperty("BDS")]
            public string Destination
            {
                get;
                set;
            }
            [JsonProperty("BGS")]
            public string SegRef
            {
                get;
                set;
            }
            [JsonProperty("BGC")]
            public string Code
            {
                get;
                set;
            }
            [JsonProperty("BGD")]
            public string Description
            {
                get;
                set;
            }
            [JsonProperty("BGA")]
            public string Amount
            {
                get;
                set;
            }
            [DataMember(Name = "BITN")]
            public string Itinref
            {
                get;
                set;
            }
        }
        #endregion

        #region RescheduleAvailRQ
        //[DataContract]
        //public class RescheduleAvailRQ
        //{
        //    [JsonProperty("CAT")]
        //    public string Category
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("AGD")]
        //    public AgentDetails Agent //ABCD
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("AVR")]
        //    public List<AvailabilityRequests> AvailabilityRequest
        //    {
        //        get;
        //        set;
        //    }

        //    [JsonProperty("PSG")]
        //    public Passengers Passenger
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("CYC")]
        //    public string Currencycode
        //    {
        //        get;
        //        set;
        //    }
        //    [XmlArrayItem("CarrierId", typeof(string))]
        //    [XmlArray("FlightOption")]
        //    [JsonProperty("FLO")]
        //    public string[] FlightOption
        //    {
        //        get;
        //        set;
        //    }

        //    [JsonProperty("TRP")]
        //    public string TripType
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("SEG")]
        //    public string SegmentType
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("HOS")]
        //    public bool HostSearch
        //    {
        //        get;
        //        set;
        //    }
        //}
        #endregion

        #region RescheduleAvailRS
        //[DataContract]
        //public class RescheduleAvailRS
        //{
        //    [JsonProperty("Er")]
        //    public string Error
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("RC")]
        //    public string ResultCode
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("SEQ")]
        //    public string Sqe
        //    {
        //        get;
        //        set;
        //    }
        //    [XmlElement("Flights")]
        //    [JsonProperty("FL")]
        //    public List<ResFlight> ResFlights
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("TK")]
        //    public string Token
        //    {
        //        get;
        //        set;
        //    }
        //}
        //[DataContract]
        //public class ResFlight
        //{
        //    [JsonProperty("FN")]
        //    public string FlightNumber
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("SDT")]
        //    public string DepartureDateTime
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("EDT")]
        //    public string ArrivalDateTime
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("CL")]
        //    public string Class
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("ALC")]
        //    public string AirlineCategory
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("CNX")]
        //    public string CNX
        //    {
        //        get;
        //        set;
        //    }
        //    [JsonProperty("RTK")]
        //    public string ReferenceToken
        //    {
        //        get;
        //        set;
        //    }
        //}
        #endregion

        #region RQ_SeatMap

        [DataContract]
        public class GetSeatMap_RQ
        {
            [JsonProperty]
            public AgentDetailsSeat AgentDetail
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
            [JsonProperty]
            public List<ReqPassDetail> PassengerDetails
            {
                get;
                set;
            }
        }

        [DataContract]
        public class AgentDetailsSeat
        {
            [JsonProperty]
            public string AgentId
            {
                get;
                set;
            }
            [JsonProperty]
            public string TerminalId
            {
                get;
                set;
            }
            [JsonProperty]
            public string UserName
            {
                get;
                set;
            }
            [JsonProperty]
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
            [JsonProperty]
            public string Environment
            {
                get;
                set;
            }
            [JsonProperty]
            public string ClientID
            {
                get;
                set;
            }
            [JsonProperty]
            public string BOAID
            {
                get;
                set;
            }
            [JsonProperty]
            public string BOAterminalID
            {
                get;
                set;
            }
            [JsonProperty]
            public string Agenttype
            {
                get;
                set;
            }
            [JsonProperty]
            public string CoOrdinatorID
            {
                get;
                set;
            }
            [JsonProperty]
            public string AirportID
            {
                get;
                set;
            }
            [JsonProperty]
            public string BranchID
            {
                get;
                set;
            }
            [JsonProperty]
            public string IssuingBranchID
            {
                get;
                set;
            }
            [JsonProperty]
            public string EMP_ID
            {
                get;
                set;
            }
            [JsonProperty]
            public string COST_CENTER
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string ProjectId
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string Platform
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
            [DataMember]
            public string[] APIUSE
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
        }



        [DataContract]
        public class AgentDetailsBalance
        {
            [JsonProperty]
            public string AgentID
            {
                get;
                set;
            }
            [JsonProperty]
            public string TerminalID
            {
                get;
                set;
            }
            [JsonProperty]
            public string UserName
            {
                get;
                set;
            }
            [JsonProperty]
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
            [JsonProperty]
            public string Environment
            {
                get;
                set;
            }
            [JsonProperty]
            public string ClientID
            {
                get;
                set;
            }
            [JsonProperty]
            public string BOAID
            {
                get;
                set;
            }
            [JsonProperty]
            public string BOATreminalID
            {
                get;
                set;
            }
            [JsonProperty]
            public string AgentType
            {
                get;
                set;
            }
            [JsonProperty]
            public string CoOrdinatorID
            {
                get;
                set;
            }
            [JsonProperty]
            public string Airportid
            {
                get;
                set;
            }
            [JsonProperty]
            public string BranchID
            {
                get;
                set;
            }
            //[JsonProperty]
            //public string IssuingBranchID
            //{
            //    get;
            //    set;
            //}
            //[JsonProperty]
            //public string EMP_ID
            //{
            //    get;
            //    set;
            //}
            ////[JsonProperty]
            ////public string COST_CENTER
            ////{
            ////    get;
            ////    set;
            ////}
            [JsonProperty]
            public string APIUSE
            {
                get;
                set;
            }
        }



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
            [JsonProperty]
            public string Triptype { get; set; }
            [JsonProperty]
            public string Segmenttype { get; set; }
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
            [DataMember]
            public string FareType
            {
                get;
                set;
            }
            [DataMember]
            public string FareTypeDescription
            {
                get;
                set;
            }
        }

        #endregion

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

        }
        #endregion

        [DataContract]
        public class GetSeatMap_RQs
        {
            [JsonProperty]
            public AgentDetailss AGENTDETAILS
            {
                get;
                set;
            }
            [JsonProperty]
            public List<RQFlightss> FLIGHTDETAILS
            {
                get;
                set;
            }
            [JsonProperty]
            public TripDetailss SEARCHINPUT { get; set; }
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
            [JsonProperty]
            public List<ReqPassDetails> PASSENGER_DETAILS
            {
                get;
                set;
            }
        }

        [DataContract]
        public class AgentDetailss
        {
            [JsonProperty]
            public string AgentId
            {
                get;
                set;
            }
            [JsonProperty]
            public string TerminalId
            {
                get;
                set;
            }
            [JsonProperty]
            public string UserName
            {
                get;
                set;
            }
            [JsonProperty]
            public string AppType
            {
                get;
                set;
            }
            [JsonProperty]
            public string Appversion
            {
                get;
                set;
            }
            [JsonProperty]
            public string TERMINALTYPE
            {
                get;
                set;
            }
            [JsonProperty]
            public string ClientID
            {
                get;
                set;
            }
            [JsonProperty]
            public string BOAID
            {
                get;
                set;
            }
            [JsonProperty]
            public string BOAterminalID
            {
                get;
                set;
            }
            [JsonProperty]
            public string Agenttype
            {
                get;
                set;
            }
            [JsonProperty]
            public string CoOrdinatorID
            {
                get;
                set;
            }
            [JsonProperty]
            public string AirportID
            {
                get;
                set;
            }
            [JsonProperty]
            public string BranchID
            {
                get;
                set;
            }
            [JsonProperty]
            public string IssuingBranchID
            {
                get;
                set;
            }

            [JsonProperty]
            public string HEADERBRANCHID
            {
                get;
                set;
            }
            [JsonProperty]
            public string EMP_ID
            {
                get;
                set;
            }
            [JsonProperty]
            public string COST_CENTER
            {
                get;
                set;
            }
            [JsonProperty]
            public string SEQUENCEID
            {
                get;
                set;
            }
        }

        [DataContract]
        public class ReqPassDetails
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
        public class TripDetailss
        {

            [JsonProperty]
            public string Orgin { get; set; }
            [JsonProperty]
            public string Destination { get; set; }
            [JsonProperty]
            public string Adult { get; set; }
            [JsonProperty]
            public string Child { get; set; }
            [JsonProperty]
            public string Infant { get; set; }
            [JsonProperty]
            public string Triptype { get; set; }
            [JsonProperty]
            public string Segtype { get; set; }
            [JsonProperty]
            public string FROMDATE { get; set; }
            [JsonProperty]
            public string TODATE { get; set; }
            [JsonProperty]
            public string SPLFLAG { get; set; }
            [JsonProperty]
            public string HOSTSEARCH { get; set; }
            [JsonProperty]
            public string CLASSTYPE { get; set; }
            [JsonProperty]
            public string CRSID { get; set; }
            [JsonProperty]
            public string MULTICITY { get; set; }



        }

        [DataContract]
        public class RQFlightss
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
            public string FlightToken
            {
                get;
                set;
            }
            [DataMember]
            public string SegNo
            {
                get;
                set;
            }
            [DataMember]
            public string ItryNo
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
            [DataMember]
            public string FareType
            {
                get;
                set;
            }
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
            public string BreakPoint
            {
                get;
                set;
            }
            [DataMember]
            public string Departure
            {
                get;
                set;
            }
            [DataMember]
            public string Arrival
            {
                get;
                set;
            }
        }


        #region FareCheck

        [DataContract]
        public class FareCheckRQ
        {
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
            [DataMember]
            public AgentDetails Agent //ABCD
            {
                get;
                set;
            }
            [DataMember]
            public List<Flights> Flight
            {
                get;
                set;
            }
            [DataMember]
            public int AdultCount
            {
                get;
                set;
            }
            [DataMember]
            public int ChildCount
            {
                get;
                set;
            }
            [DataMember]
            public int InfantCount
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
            [DataMember]
            public string SegmentType
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
            [DataMember]
            public string ClassType
            {
                get;
                set;
            }
            [DataMember]
            public string Category
            {
                get;
                set;
            }

        }

        [DataContract]
        public class FareCheckRS
        {
            [DataMember(Name = "MCRC")]
            public string ResultCode
            {
                get;
                set;
            }
            [DataMember(Name = "SEQ")]
            public string Sqe
            {
                get;
                set;
            }
            [DataMember(Name = "MCEr")]
            public string Error
            {
                get;
                set;
            }
            [XmlElement("Flights")]
            [DataMember(Name = "MCFL")]
            public List<Flights> Flights
            {
                get;
                set;
            }
            [XmlElement("Fares")]
            [DataMember(Name = "MCFR")]
            public List<Fares> Fares
            {
                get;
                set;
            }

        }

        #endregion

        #endregion

        #region Cancellation_RQ
        [DataContract]
        public class CancellationRQ
        {
            [DataMember(IsRequired = true)]
            public String Platform
            {
                get;
                set;
            }

            [DataMember(IsRequired = true)]
            public String CRSID
            {
                get;
                set;
            }

            [DataMember(IsRequired = true)]
            public String Stock
            {
                get;
                set;
            }

            [DataMember]
            public String CRSPNR
            {
                get;
                set;
            }

            [DataMember]
            public String AirlinePNR
            {
                get;
                set;
            }

            [DataMember(IsRequired = true)]
            public String AirlineCategory
            {
                get;
                set;
            }

            [DataMember]
            public String BookingTrackID
            {
                get;
                set;
            }

            [DataMember]
            public String BookPCC
            {
                get;
                set;
            }

            [DataMember]
            public String BookSupp
            {
                get;
                set;
            }

            [DataMember]
            public String TicketPCC
            {
                get;
                set;
            }

            [DataMember]
            public String TicketSupp
            {
                get;
                set;
            }

            [DataMember(IsRequired = true)]
            public String CancellationType
            {
                get;
                set;
            }//Partial-->P, Full -->F

            [DataMember(IsRequired = true)]
            public bool IsCancellation
            {
                get;
                set;
            }//true --> To Cancel, false --> To fare check

            [DataMember(IsRequired = true)]
            public AgentDetailscancel AgentDetail
            {
                get;
                set;
            }

            [DataMember]
            public String BookingAmount
            {
                get;
                set;
            }

            [DataMember]
            public SegmentDetailsCancel Segments
            {
                get;
                set;
            }
        }
        [DataContract]
        public class SegmentDetailsCancel
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
            public string DepartureDate
            {
                get;
                set;
            }
            [DataMember]
            public string ArrivalDate
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
            [DataMember]
            public String FareType
            {
                get;
                set;
            }
        }

        [DataContract]
        public class AgentDetailscancel
        {
            [DataMember]
            public string AgentID
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
            public string TerminalID
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
            [DataMember(IsRequired = true)]
            public string Version
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string Environment
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
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
            [DataMember(IsRequired = true)]
            public string ProjectId
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string Platform
            {
                get;
                set;
            }
            [DataMember]
            public string OfficeId
            {
                get;
                set;
            }
            [DataMember]
            public string PromotionCode
            {
                get;
                set;
            }
            [DataMember]
            public string EMP_ID
            {
                get;
                set;
            }
            [DataMember]
            public string[] APIUSE
            {
                get;
                set;
            }
            [DataMember]
            public string Group_ID
            {
                get;
                set;
            }

        }

        #endregion

        #region Cancellation_RS
        [DataContract]
        public class CancellationRS
        {
            [DataMember]
            public Status Status
            {
                get;
                set;
            }
            [DataMember]
            public String TotalBookingAmount
            {
                get;
                set;
            }
            [DataMember]
            public String RefundAmount
            {
                get;
                set;
            }
            [DataMember]
            public String PenaltyAmount
            {
                get;
                set;
            }
            [DataMember]
            public List<SSRDetails> SSRDetail
            {
                get;
                set;
            }
            [DataContract]
            public class SSRDetails
            {
                [DataMember]
                public string Orgin
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
                public string Code
                {
                    get;
                    set;
                }
                [DataMember]
                public string Description
                {
                    get;
                    set;
                }
                [DataMember]
                public string Amount
                {
                    get;
                    set;
                }
                [DataMember]
                public string PaxRef
                {
                    get;
                    set;
                }
                [DataMember]
                public string SSRAvailType
                {
                    get;
                    set;
                }
            }
        }
        #endregion



        #region Reschedule Request & Response
        [DataContract]
        public class RescheduleAvailRQ
        {
            [DataMember]
            public Credential Credentials
            {
                get;
                set;
            }
            [DataMember]
            public string Category
            {
                get;
                set;
            }
            [DataMember]
            public AgentDetails Agent
            {
                get;
                set;
            }
            [DataMember]
            public List<AvailabilityRequests> AvailabilityRequest
            {
                get;
                set;
            }
            [DataMember]
            public List<Promocodes> PromoCodes
            {
                get;
                set;
            }
            [DataMember]
            public Passengers Passenger
            {
                get;
                set;
            }
            [DataMember]
            public string Currencycode
            {
                get;
                set;
            }
            [XmlArrayItem("CarrierId", typeof(string))]
            [XmlArray("FlightOption")]
            [DataMember]
            public string[] FlightOption
            {
                get;
                set;
            }

            [DataMember]
            public string CRSPNR
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
            [DataMember]
            public string SegmentType
            {
                get;
                set;
            }
            [DataMember]
            public bool HostSearch
            {
                get;
                set;
            }
            [DataMember]
            public bool MoreFlights
            {
                get;
                set;
            }
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
            [DataMember]
            public string Nationality
            {
                get;
                set;
            }

            [DataMember]
            public bool ISWaitList
            {
                get;
                set;
            }
            [DataMember]
            public bool Discount
            {
                get;
                set;
            }
            [DataMember]
            public string DiscountFlag
            {
                get;
                set;
            }

            [DataMember]
            public string Commission
            {
                get;
                set;
            }

            [DataMember]
            public bool DirectFlight
            {
                get;
                set;
            }

            [DataMember]
            public string FlightCategory
            {
                get;
                set;
            }
            [DataMember]
            public string RequestTime
            {
                get;
                set;
            }
            [DataMember]
            public List<TicketNumberDetails> TicketNumberDetails
            {
                get;
                set;
            }
            //[DataMember]
            //public L2BInsert Insert_L2B { get; set; }
        }
        [DataContract]
        public class TicketNumberDetails
        {
            [DataMember]
            public string PaxRefNo
            {
                get;
                set;
            }
            [DataMember]
            public string PaxType
            {
                get;
                set;
            }
            [DataMember]
            public string SegNo
            {
                get;
                set;
            }
            [DataMember]
            public string TicketNumber
            {
                get;
                set;
            }
        }
        [DataContract]
        public class RescheduleAvailRS
        {
            [DataMember]
            public string Error
            {
                get;
                set;
            }
            [DataMember]
            public string DisplayError
            {
                get;
                set;
            }
            [DataMember]
            public Status Status
            {
                get;
                set;
            }
            [DataMember]
            public string ResultCode
            {
                get;
                set;
            }
            [DataMember]
            public string Sqe
            {
                get;
                set;
            }
            [DataMember]
            public HostCheck FareCheck
            {
                get;
                set;
            }
            [DataMember]
            public bool IsCurChange
            {
                get;
                set;
            }
            [XmlElement("ResFlights")]
            [DataMember]
            public List<ResFlight> ResFlights
            {
                get;
                set;
            }
            [XmlElement("Fares")]
            [DataMember]
            public List<Fares> Fares
            {
                get;
                set;
            }
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
        }
        [DataContract]
        public class ResFlight
        {
            [DataMember]
            public string Meals
            {
                get;
                set;
            }
            [DataMember]
            public string Refundable
            {
                get;
                set;
            }
            [DataMember]
            public string Baggage
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
            public string RBDCode
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
            public string Stops
            {
                get;
                set;
            }
            [DataMember]
            public string Via
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
            public string CNX
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
            public string SegmentDetails
            {
                get;
                set;
            }
            [DataMember]
            public string FlyingTime
            {
                get;
                set;
            }
            [DataMember]
            public string JourneyTime
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
            public string ViaItinRef
            {
                get;
                set;
            }
            [DataMember]
            public string ConnectionFlag
            {
                get;
                set;
            }
            [DataMember]
            public int OfflineFlag
            {
                get;
                set;
            }
            [DataMember]
            public string FareId
            {
                get;
                set;
            }
            [DataMember]
            public bool OfflineIndicator
            {
                get;
                set;
            }
            [DataMember]
            public bool TransactionFlag
            {
                get;
                set;
            }
            [DataMember]
            public string OtherFares
            {
                get;
                set;
            }
            [DataMember]
            public string MultiClass
            {
                get;
                set;
            }
            [DataMember]
            public bool AllowFQT
            {
                get;
                set;
            }
            [DataMember]
            public string AvailSeat
            {
                get;
                set;
            }
            [DataMember]
            public string PromoCode
            {
                get;
                set;
            }
            [DataMember]
            public string FareTypeDescription
            {
                get;
                set;
            }
        }
        #endregion

        #region ReScheduleRs
        public class GetReSchedule_RS
        {
            [DataMember]
            public Status Status
            {
                get;
                set;
            }
            //[DataMember]
            //public string ActionType
            //{
            //    get;
            //    set;
            //}
            [DataMember]
            public decimal Balancedue
            {
                get;
                set;
            }
            [DataMember]
            public decimal Penalty
            {
                get;
                set;
            }
            [DataMember]
            public bool RetryFlag
            {
                get;
                set;
            }
            [DataMember]
            public decimal FareDifference
            {
                get;
                set;
            }
            [DataMember]
            public decimal OldBookingAmount
            {
                get;
                set;
            }
            [DataMember]
            public decimal NewBookingAmount
            {
                get;
                set;
            }
            [DataMember]
            public decimal TotalAdditionalAmount
            {
                get;
                set;
            }
            [DataMember(Name = "FLD")]
            public List<FlightsReRS> FLD
            {
                get;
                set;
            }
            [DataMember(Name = "FRD")]
            public List<FaresRERS> FRD
            {
                get;
                set;
            }
            [DataMember]
            public string AlertMsg
            {
                get;
                set;
            }
            [DataMember]
            public List<PassengerDetails> PassengerDetails
            {
                get;
                set;
            }

        }
        #endregion
        [DataContract]
        public class PassengerDetails
        {
            [DataMember]
            public string PaxRefNumber
            {
                get;
                set;
            }
            [DataMember]
            public string Title
            {
                get;
                set;
            }
            [DataMember]
            public string FirstName
            {
                get;
                set;
            }
            [DataMember]
            public string LastName
            {
                get;
                set;
            }
            [DataMember]
            public string DOB
            {
                get;
                set;
            }
            [DataMember]
            public string PaxType
            {
                get;
                set;
            }
            [DataMember]
            public string AIRLINECODE
            {
                get;
                set;
            }
            [DataMember]
            public string FLIGHTNO
            {
                get;
                set;
            }
            [DataMember]
            public string ORIGIN
            {
                get;
                set;
            }
            [DataMember]
            public string DESTINATION
            {
                get;
                set;
            }
            [DataMember]
            public string PLATTINGCARRIER
            {
                get;
                set;
            }
            [DataMember]
            public string DEPARTUREDATE
            {
                get;
                set;
            }
            [DataMember]
            public string ARRIVALDATE
            {
                get;
                set;
            }
            [DataMember]
            public string DEPARTURETIME
            {
                get;
                set;
            }
            [DataMember]
            public string ARRIVALTIME
            {
                get;
                set;
            }
            [DataMember]
            public string AIRLINEPNR
            {
                get;
                set;
            }
            [DataMember]
            public string CRSPNR
            {
                get;
                set;
            }
            [DataMember]
            public string TICKETNO
            {
                get;
                set;
            }
            [DataMember]
            public string SEGREF
            {
                get;
                set;
            }


        }

        [DataContract]
        public class FlightsReRS
        {
            [DataMember(Name = "CAC")]
            public string CarrierCode
            {
                get;
                set;
            }
            [DataMember(Name = "FNO")]
            public string FlightNumber
            {
                get;
                set;
            }
            [DataMember(Name = "ORG")]
            public string Origin
            {
                get;
                set;
            }
            [DataMember(Name = "DES")]
            public string Destination
            {
                get;
                set;
            }
            [DataMember(Name = "STL")]
            public string StartTerminal
            {
                get;
                set;
            }
            [DataMember(Name = "DTL")]
            public string EndTerminal
            {
                get;
                set;
            }
            [DataMember(Name = "SDT")]
            public string DepartureDateTime
            {
                get;
                set;
            }
            [DataMember(Name = "EDT")]
            public string ArrivalDateTime
            {
                get;
                set;
            }
            [DataMember(Name = "CLS")]
            public string Class
            {
                get;
                set;
            }
            [DataMember(Name = "CAB")]
            public string Cabin
            {
                get;
                set;
            }
            [DataMember(Name = "FBC")]
            public string FareBasisCode
            {
                get;
                set;
            }
            [DataMember(Name = "STP")]
            public string Stops
            {
                get;
                set;
            }
            [DataMember(Name = "ALC")]
            public string AirlineCategory
            {
                get;
                set;
            }
            [DataMember(Name = "PLT")]
            public string PlatingCarrier
            {
                get;
                set;
            }
            [DataMember(Name = "RFT")]
            public string ReferenceToken
            {
                get;
                set;
            }
            [DataMember(Name = "SEG")]
            public string SegRef
            {
                get;
                set;
            }
            [DataMember(Name = "ITN")]
            public string ItinRef
            {
                get;
                set;
            }
            [DataMember(Name = "FRI")]
            public string FareID
            {
                get;
                set;
            }
            [DataMember(Name = "RFB")]
            public string Refundable
            {
                get;
                set;
            }
            [DataMember(Name = "BAG")]
            public string Baggage
            {
                get;
                set;
            }
            [DataMember(Name = "MEL")]
            public string Meals
            {
                get;
                set;
            }
            [DataMember(Name = "SET")]
            public string Seat
            {
                get;
                set;
            }
            [DataMember(Name = "CNX")]
            public string CNXType
            {
                get;
                set;
            }
            [DataMember(Name = "FYT")]
            public string FlyingTime
            {
                get;
                set;
            }
            [DataMember(Name = "FDC")]
            public string FareDescription
            {
                get;
                set;
            }

        }
        [DataContract]
        public class FaresRERS
        {
            [DataMember(Name = "FDC")]
            public List<FaredescriptionRERS> Faredescription
            {
                get;
                set;
            }
            [DataMember(Name = "CUR")]
            public string Currency
            {
                get;
                set;
            }
            [DataMember(Name = "ACU")]
            public string APICurrency
            {
                get;
                set;
            }
            [DataMember(Name = "ROE")]
            public string ROEValue
            {
                get;
                set;
            }
            [DataMember(Name = "FID")]
            public string FlightID
            {
                get;
                set;
            }
        }
        [DataContract]
        public class FaredescriptionRERS
        {

            [DataMember(Name = "PTY")]
            public string PaxType
            {
                get;
                set;
            }
            [DataMember(Name = "BFA")]
            public string BaseAmount
            {
                get;
                set;
            }
            [DataMember(Name = "TTA")]
            public string TotalTaxAmount
            {
                get;
                set;
            }
            [DataMember(Name = "GRA")]
            public string GrossAmount
            {
                get;
                set;
            }
            [DataMember(Name = "ABU")]
            public string APIBreakup
            {
                get;
                set;
            }
            [DataMember(Name = "COM")]
            public string Commission
            {
                get;
                set;
            }
            [DataMember(Name = "INC")]
            public string Incentive
            {
                get;
                set;
            }
            [DataMember(Name = "SVC")]
            public string Servicecharge
            {
                get;
                set;
            }
            [DataMember(Name = "STA")]
            public string ServiceTax
            {
                get;
                set;
            }
            [DataMember(Name = "TDS")]
            public string TDS
            {
                get;
                set;
            }
            [DataMember(Name = "DSC")]
            public string Discount
            {
                get;
                set;
            }
            [DataMember(Name = "TSF")]
            public string TransactionFee
            {
                get;
                set;
            }
            [DataMember(Name = "MRK")]
            public string Markup
            {
                get;
                set;
            }
            [DataMember]
            public string ServiceFee { get; set; }
            [DataMember(Name = "HID")]
            public string HIDDMARKUP
            {
                get;
                set;
            }
            [DataMember(Name = "CMKP")]
            public string ClientMarkup
            {
                get;
                set;
            }
            [DataMember(Name = "AMK")]
            public string AddMarkup
            {
                get;
                set;
            }
            [DataMember(Name = "AGD")]
            public string AgnDeal
            {
                get;
                set;
            }
            [DataMember(Name = "OLF")]
            public string OldFare
            {
                get;
                set;
            }
            [DataMember(Name = "OLM")]
            public string OldMarkup
            {
                get;
                set;
            }
            [DataMember(Name = "BBO")]
            public bool BestBuyOption
            {
                get;
                set;
            }
            [DataMember(Name = "TAX")]
            public List<Taxes> Taxes
            {
                get;
                set;
            }


        }

        [DataContract]
        public class GetReSchedule_RQ
        {
            [DataMember]
            public AgentDetailsRES Agent
            {
                get;
                set;
            }
            [DataMember]
            public decimal PaymentAmount
            {
                get;
                set;
            }
            [DataMember]
            public List<ReSchedule> ReSchedule
            {
                get;
                set;
            }
            [DataMember]
            public List<TicketNumberDetails> TicketNumberDetails
            {
                get;
                set;
            }
            [DataMember]
            public List<PaxDetails> PassengerDetails
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
            public string ResheduleMethod//ALL,JOURNEY
            {
                get;
                set;
            }
            [DataMember]
            public string Stock
            {
                get;
                set;
            }
            [DataMember]
            public string OfficeId
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
            [DataMember]
            public string BookingTrackId
            {
                get;
                set;
            }
            [DataMember]
            public string AirlinePNR
            {
                get;
                set;
            }
            [DataMember]
            public string CRSPNR
            {
                get;
                set;
            }
            [DataMember]
            public string AirlineCategory //LCC, FSC
            {
                get;
                set;
            }
            [DataMember]
            public string ProcessType//RESCHEDULE,CHECKFARE
            {
                get;
                set;
            }
            [DataMember]
            public string PaymentMode
            {
                get;
                set;
            }
        }
        [DataContract]
        public class AgentDetailsRES
        {
            [DataMember]
            public string AgentID
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
            public string TerminalID
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
            [DataMember(IsRequired = true)]
            public string Version
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
            public string Environment
            {
                get;
                set;
            }
            [DataMember(IsRequired = true)]
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
            public string Airportid
            {
                get;
                set;
            }
            [DataMember]
            public string ProjectId
            {
                get;
                set;
            }
            [DataMember]
            public string Platform  // A for Agent, B for BOA, C for CBT
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
            public string[] APIUSE
            {
                get;
                set;
            }
            [DataMember]
            public string OfficeId
            {
                get;
                set;
            }
            [DataMember]
            public string PromotionCode
            {
                get;
                set;
            }
            [DataMember]
            public string EMP_ID
            {
                get;
                set;
            }
            [DataMember]
            public string Group_ID
            {
                get;
                set;
            }

        }
        [DataContract]
        public class ReSchedule
        {
            [DataMember]
            public string Type//NOCHANGE,SECOTRCHANGE,FLIGHT,PASSENGER
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
            public string PlatingCarrier
            {
                get;
                set;
            }
            [DataMember]
            public string FareTypeDescription
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

            //Reschedule Flight Details
            [DataMember]
            public string NEWFlightNumber
            {
                get;
                set;
            }
            [DataMember]
            public string NEWOrigin
            {
                get;
                set;
            }
            [DataMember]
            public string NEWDestination
            {
                get;
                set;
            }
            [DataMember]
            public string NEWDepartureDateTime
            {
                get;
                set;
            }
            [DataMember]
            public string NEWArrivalDateTime
            {
                get;
                set;
            }
            [DataMember]
            public string NEWClass
            {
                get;
                set;
            }
            [DataMember]
            public string NEWCabin
            {
                get;
                set;
            }
            [DataMember]
            public string NEWFareBasisCode
            {
                get;
                set;
            }
            [DataMember]
            public string NEWPlatingCarrier
            {
                get;
                set;
            }
            [DataMember]
            public string NEWFareTypeDescription
            {
                get;
                set;
            }
            [DataMember]
            public string NEWReferenceToken
            {
                get;
                set;
            }
        }
        [DataContract]
        public class FaresRES
        {
            [XmlElement("Faredescription")]
            [DataMember]
            public List<Faredescription> Faredescription
            {
                get;
                set;
            }
            [DataMember]
            public string MultiClassAmount
            {
                get;
                set;
            }
            [DataMember]
            public string Currency
            {
                get;
                set;
            }
            [DataMember]
            public string FlightId
            {
                get;
                set;
            }
        }
        [DataContract]
        public class FaredescriptionRES
        {
            [DataMember]
            public string Paxtype
            {
                get;
                set;
            }
            [DataMember]
            public string BaseAmount
            {
                get;
                set;
            }
            [DataMember]
            public string TotalTaxAmount
            {
                get;
                set;
            }
            [DataMember]
            public string GrossAmount
            {
                get;
                set;
            }
            [DataMember]
            public string Commission
            {
                get;
                set;
            }
            [DataMember]
            public string Incentive
            {
                get;
                set;
            }
            [DataMember]
            public string Servicecharge
            {
                get;
                set;
            }
            [DataMember]
            public string ServiceTax
            {
                get;
                set;
            }
            [DataMember]
            public string TDS
            {
                get;
                set;
            }
            [DataMember]
            public string Discount
            {
                get;
                set;
            }
            [DataMember]
            public string TransactionFee
            {
                get;
                set;
            }

            // [XmlElement("Tax")]
            [DataMember]
            public List<TaxesRES> Taxes
            {
                get;
                set;
            }
        }
        [DataContract]
        public class TaxesRES
        {
            [DataMember]
            public string Code
            {
                get;
                set;
            }
            [DataMember]
            public string Amount
            {
                get;
                set;
            }
        }
        public class FlightDetailsRES
        {
            public string FlightNumber { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string OriginName { get; set; }
            public string DestinationName { get; set; }
            public string DepartureDate { get; set; }
            public string DepartureTime { get; set; }
            public string ArrivalDate { get; set; }
            public string ArrivalTime { get; set; }
            public string FlyingTime { get; set; }
            public string Class { get; set; }
            public string ClassType { get; set; }
            public string Cabin { get; set; }
            public string AvailSeat { get; set; }
            public string CarrierCode { get; set; }
            public string CNX { get; set; }
            public string ConnectionFlag { get; set; }
            public string FareBasisCode { get; set; }
            public string FareId { get; set; }
            public List<FaredescriptionRES> Faredescription { get; set; }
            public string PlatingCarrier { get; set; }
            public string ReferenceToken { get; set; }
            public string SegRef { get; set; }
            public string Stops { get; set; }
            public string Via { get; set; }
            public string BaseAmount { get; set; }
            public string GrossAmount { get; set; }
            public string TotalTaxAmount { get; set; }
            public string Commission { get; set; }
            public List<TaxesRES> Taxes { get; set; }
            public string MultiClassAmount { get; set; }
            public string Refund { get; set; }
            public string Stock { get; set; }
            public string ItinRef { get; set; }
            public string Airlinecategory { get; set; }
            public string MulticlassAirlinecategory { get; set; }
            public string Duration { get; set; }
            public string ClassCarrierCode { get; set; }
            public string FareType { get; set; }
            public string StartTerminal { get; set; }
            public string EndTerminal { get; set; }
            public string JourneyTime { get; set; }
            public string MulticlassAvail { get; set; }
            public string FareTypeDescription { get; set; }
            //Future use
            public string BaggageInfo { get; set; }
            public string BreakPoint { get; set; }
            public string CRSID { get; set; }
            public string via { get; set; }
            public string FareSegRef { get; set; }
            public string STK { get; set; }
            //brindha added for Compliance & NonCompliance
            public string ComplianceStatus { get; set; }
            public string ComplianceStatusflag { get; set; }
            public string COMPARABLEFLAG { get; set; }
            public string AirlineName { get; set; }
        }

        public class SearchModel
        {
            public class SearchRQ
            {
                public string OriginCode { get; set; }
                public string DestinationCode { get; set; }
                public string DepDate { get; set; }
                public string ReturnDate { get; set; }
                public string AdultCount { get; set; }
                public string ChildCount { get; set; }
                public string InfantCount { get; set; }
                public string TripType { get; set; }
                public string Class { get; set; }
                public bool HostSearch { get; set; }
                public string Airporttype { get; set; }
                public string Airline { get; set; }
                public string Corporateflag { get; set; }
                public string Airlinecategory { get; set; }
                public string CorporateCode { get; set; }
                public string RoundtripDomesticFlag { get; set; }
                public string Product { get; set; }
                public string seqID { get; set; }
                public string ClientID { get; set; }
                public string BranchID { get; set; }
                public string FareTypeFlag { get; set; }
                public string InitialCommflag { get; set; }
                public string personalbookingflag { get; set; }
                public string GroupID { get; set; }
                public string Threadlength { get; set; }
                public string Rescount { get; set; }
                public string Flightno { get; set; }
                public string via { get; set; }
            }

            #region RDBBlockClass by vijai 09082018
            public List<RBDBlockClass> lstBlockClasses { get; set; }

            [DataContract]
            public class RBDBlockClass
            {
                public string AirlineName { get; set; }
                public string ClassName { get; set; }
            }
            #endregion

            public List<List<FlightDetails>> lstlstFlightDetail { get; set; }

            public List<List<List<FlightDetails>>> lstlstFlightDetailslist { get; set; }



            [DataContract]
            public class FlightDetailsShot
            {
                [DataMember(Name = "FN")]
                public string FlightNumber { get; set; }
                [DataMember(Name = "O")]
                public string Origin { get; set; }
                [DataMember(Name = "D")]
                public string Destination { get; set; }
                [DataMember(Name = "ON")]
                public string OriginName { get; set; }
                [DataMember(Name = "DN")]
                public string DestinationName { get; set; }
                [DataMember(Name = "DD")]
                public string DepartureDate { get; set; }
                [DataMember(Name = "DT")]
                public string DepartureTime { get; set; }
                [DataMember(Name = "AD")]
                public string ArrivalDate { get; set; }
                [DataMember(Name = "AT")]
                public string ArrivalTime { get; set; }
                [DataMember(Name = "FTM")]
                public string FlyingTime { get; set; }
                [DataMember(Name = "C")]
                public string Class { get; set; }
                [DataMember(Name = "CT")]
                public string ClassType { get; set; }
                [DataMember(Name = "CB")]
                public string Cabin { get; set; }
                [DataMember(Name = "AS")]
                public string AvailSeat { get; set; }
                [DataMember(Name = "CC")]
                public string CarrierCode { get; set; }
                [DataMember(Name = "CX")]
                public string CNX { get; set; }
                [DataMember(Name = "CF")]
                public string ConnectionFlag { get; set; }
                [DataMember(Name = "FC")]
                public string FareBasisCode { get; set; }
                [DataMember(Name = "FI")]
                public string FareId { get; set; }
                [DataMember(Name = "FD")]
                public List<OnlineserviceRQRS.Faredescription> Faredescription { get; set; }
                [DataMember(Name = "PC")]
                public string PlatingCarrier { get; set; }
                [DataMember(Name = "RT")]
                public string ReferenceToken { get; set; }
                [DataMember(Name = "SR")]
                public string SegRef { get; set; }
                [DataMember(Name = "S")]
                public string Stops { get; set; }
                [DataMember(Name = "V")]
                public string Via { get; set; }
                [DataMember(Name = "BA")]
                public string BaseAmount { get; set; }
                [DataMember(Name = "GA")]
                public string GrossAmount { get; set; }
                [DataMember(Name = "TA")]
                public string TotalTaxAmount { get; set; }
                [DataMember(Name = "CMS")]
                public string Commission { get; set; }
                [DataMember(Name = "T")]
                public List<OnlineserviceRQRS.Taxes> Taxes { get; set; }
                [DataMember(Name = "MA")]
                public string MultiClassAmount { get; set; }
                [DataMember(Name = "R")]
                public string Refund { get; set; }
                [DataMember(Name = "STCK")]
                public string Stock { get; set; }
                [DataMember(Name = "IR")]
                public string ItinRef { get; set; }
                [DataMember(Name = "AC")]
                public string Airlinecategory { get; set; }
                [DataMember(Name = "MAC")]
                public string MulticlassAirlinecategory { get; set; }
                [DataMember(Name = "DU")]
                public string Duration { get; set; }
                [DataMember(Name = "CCC")]
                public string ClassCarrierCode { get; set; }
                [DataMember(Name = "FT")]
                public string FareType { get; set; }
                [DataMember(Name = "ST")]
                public string StartTerminal { get; set; }
                [DataMember(Name = "ET")]
                public string EndTerminal { get; set; }
                [DataMember(Name = "JT")]
                public string JourneyTime { get; set; }
                [DataMember(Name = "MCA")]
                public string MulticlassAvail { get; set; }
                [DataMember(Name = "FTPO")]
                public string FareTypeDescription { get; set; }
                //Future use
                [DataMember(Name = "BI")]
                public string BaggageInfo { get; set; }
                [DataMember(Name = "BP")]
                public string BreakPoint { get; set; }
                [DataMember(Name = "CRS")]
                public string CRSID { get; set; }
                [DataMember(Name = "VA")]
                public string via { get; set; }
                [DataMember(Name = "FSR")]
                public string FareSegRef { get; set; }
                [DataMember(Name = "SK")]
                public string STK { get; set; }
                //brindha added for Compliance & NonCompliance
                [DataMember(Name = "CS")]
                public string ComplianceStatus { get; set; }
                [DataMember(Name = "CSF")]
                public string ComplianceStatusflag { get; set; }
                [DataMember(Name = "CMF")]
                public string COMPARABLEFLAG { get; set; }
                [DataMember(Name = "AN")]
                public string AirlineName { get; set; }
                [DataMember(Name = "SF")]
                public string ServiceFee { get; set; }

                //RQRS.Faredescription start...
                [DataMember(Name = "PAX")]
                public string Paxtype { get; set; }
                [DataMember(Name = "BRK")]
                public string API_Breakup { get; set; }
                //wanna add other key values if want in future...
                //RQRS.Faredescription End...

                //RQRS.Taxes start...
                [DataMember(Name = "COD")]
                public string Code { get; set; }
                [DataMember(Name = "AMT")]
                public string Amount { get; set; }
                //RQRS.Taxes End...
                [DataMember(Name = "DPDT")]
                public string Depdate { get; set; }
                [DataMember(Name = "ARDT")]
                public string Arrdate { get; set; }
                [DataMember(Name = "Inva")]
                public string Inva { get; set; }
                [DataMember(Name = "NTF")]
                public string NETFare { get; set; }
                [DataMember(Name = "FATX")]
                public string FARETYPETEXT { get; set; }
            }

            public class FlightDetails
            {
                public string FlightNumber { get; set; }
                public string Origin { get; set; }
                public string Destination { get; set; }
                public string OriginName { get; set; }
                public string DestinationName { get; set; }
                public string DepartureDate { get; set; }
                public string DepartureTime { get; set; }
                public string ArrivalDate { get; set; }
                public string ArrivalTime { get; set; }
                public string FlyingTime { get; set; }
                public string Class { get; set; }
                public string ClassType { get; set; }
                public string Cabin { get; set; }
                public string AvailSeat { get; set; }
                public string CarrierCode { get; set; }
                public string CNX { get; set; }
                public string ConnectionFlag { get; set; }
                public string FareBasisCode { get; set; }
                public string FareId { get; set; }
                public List<RQRS.Faredescription> Faredescription { get; set; }
                public string PlatingCarrier { get; set; }
                public string ReferenceToken { get; set; }
                public string SegRef { get; set; }
                public string Stops { get; set; }
                public string Via { get; set; }
                public string BaseAmount { get; set; }
                public string GrossAmount { get; set; }
                public string TotalTaxAmount { get; set; }
                public string Commission { get; set; }
                public List<RQRS.Taxes> Taxes { get; set; }
                public string MultiClassAmount { get; set; }
                public string Refund { get; set; }
                public string Stock { get; set; }
                public string ItinRef { get; set; }
                public string Airlinecategory { get; set; }
                public string MulticlassAirlinecategory { get; set; }
                public string Duration { get; set; }
                public string ClassCarrierCode { get; set; }
                public string FareType { get; set; }
                public string StartTerminal { get; set; }
                public string EndTerminal { get; set; }
                public string JourneyTime { get; set; }
                public string MulticlassAvail { get; set; }
                public string FareTypeDescription { get; set; }
                //Future use
                public string BaggageInfo { get; set; }
                public string BreakPoint { get; set; }
                public string CRSID { get; set; }
                public string via { get; set; }
                public string FareSegRef { get; set; }
                public string STK { get; set; }
                //brindha added for Compliance & NonCompliance
                public string ComplianceStatus { get; set; }
                public string ComplianceStatusflag { get; set; }
                public string COMPARABLEFLAG { get; set; }
                public string AirlineName { get; set; }

                public string Inva { get; set; }
            }

            #region GDS Avail
            public class FlightDetails_GDS
            {
                public string FlightNumber { get; set; }
                public string Origin { get; set; }
                public string Destination { get; set; }
                public string OriginName { get; set; }
                public string DestinationName { get; set; }
                public string DepartureDate { get; set; }
                public string DepartureTime { get; set; }
                public string ArrivalDate { get; set; }
                public string ArrivalTime { get; set; }
                public string FlyingTime { get; set; }
                public string Class { get; set; }
                public string ClassType { get; set; }
                public string Cabin { get; set; }
                public string AvailSeat { get; set; }
                public string CarrierCode { get; set; }
                public string CNX { get; set; }
                public string ConnectionFlag { get; set; }
                public string FareBasisCode { get; set; }
                public string FareId { get; set; }
                public List<RQRS.Faredescription> Faredescription { get; set; }
                public string PlatingCarrier { get; set; }
                public string ReferenceToken { get; set; }
                public string SegRef { get; set; }
                public string Stops { get; set; }
                //public string Via { get; set; }
                public string BaseAmount { get; set; }
                public string GrossAmount { get; set; }
                public string TotalTaxAmount { get; set; }
                public string Commission { get; set; }
                public List<RQRS.Taxes> Taxes { get; set; }
                public string MultiClassAmount { get; set; }
                public string Refund { get; set; }
                public string Stock { get; set; }
                public string ItinRef { get; set; }
                public string Airlinecategory { get; set; }
                public string MulticlassAirlinecategory { get; set; }
                public string Duration { get; set; }
                public string ClassCarrierCode { get; set; }
                public string FareType { get; set; }
                public string StartTerminal { get; set; }
                public string EndTerminal { get; set; }
                public string JourneyTime { get; set; }
                public string MulticlassAvail { get; set; }

                //Future use
                public string BaggageInfo { get; set; }
                public string BreakPoint { get; set; }
                public string CRSID { get; set; }
                public string via { get; set; }
                public string FareSegRef { get; set; }
                public string STK { get; set; }
                //brindha added for Compliance & NonCompliance
                public string ComplianceStatus { get; set; }
                public string ComplianceStatusflag { get; set; }
                public string COMPARABLEFLAG { get; set; }
                public string AirlineName { get; set; }
                public string Promocodecontent { get; set; }

                //Added for GDS Search...
                public string Seat { get; set; }
            }
            #endregion


            public class FlightDetailsCommission
            {
                public string FlightNumber { get; set; }
                public string Origin { get; set; }
                public string Destination { get; set; }
                public string OriginName { get; set; }
                public string DestinationName { get; set; }
                public string DepartureDate { get; set; }
                public string DepartureTime { get; set; }
                public string ArrivalDate { get; set; }
                public string ArrivalTime { get; set; }
                public string FlyingTime { get; set; }
                public string Class { get; set; }
                public string ClassType { get; set; }
                public string Cabin { get; set; }
                public string AvailSeat { get; set; }
                public string CarrierCode { get; set; }
                public string CNX { get; set; }
                public string ConnectionFlag { get; set; }
                public string FareBasisCode { get; set; }
                public string FareId { get; set; }
                public List<RQRS.Faredescription> Faredescription { get; set; }
                public string PlatingCarrier { get; set; }
                public string ReferenceToken { get; set; }
                public string SegRef { get; set; }
                public string Stops { get; set; }
                public string Via { get; set; }
                public string BaseAmount { get; set; }
                public string GrossAmount { get; set; }
                public string TotalTaxAmount { get; set; }
                public List<SearchModel.Taxes> Taxes { get; set; }
                public string MultiClassAmount { get; set; }
                public string Refund { get; set; }
                public string Stock { get; set; }
                public string ItinRef { get; set; }
                public string Airlinecategory { get; set; }
                public string MulticlassAirlinecategory { get; set; }
                public string Duration { get; set; }
                public string ClassCarrierCode { get; set; }
                public string FareType { get; set; }
                public string StartTerminal { get; set; }
                public string EndTerminal { get; set; }
                public string JourneyTime { get; set; }
                public string MulticlassAvail { get; set; }

                //Future use
                public string BaggageInfo { get; set; }
                public string BreakPoint { get; set; }
                public string CRSID { get; set; }
                public string via { get; set; }
                public string FareSegRef { get; set; }
                public string STK { get; set; }
                //brindha added for Compliance & NonCompliance
                public string ComplianceStatus { get; set; }
                public string ComplianceStatusflag { get; set; }
                public string COMPARABLEFLAG { get; set; }
                public string AirlineName { get; set; }
            }

            public class AvailCommission
            {
                public string Avil_Rowindex { get; set; }
                public decimal Avail_Commission { get; set; }
                public decimal Avail_Netfare { get; set; }
            }

            public class Taxes
            {
                public string Code
                {
                    get;
                    set;
                }

                public string Amount
                {
                    get;
                    set;
                }
            }

            public class KeyValCombo
            {
                public string CD
                {
                    get;
                    set;
                }

                public string FN
                {
                    get;
                    set;
                }
            }


            public class ViewModel
            {
                public IEnumerable<FlightDetails> FlightDetailsOnward { get; set; }
                public IEnumerable<FlightDetailsShot> FlightDetailsReturn { get; set; }
                public IEnumerable<SearchRQ> FlightSearchRQ { get; set; }
                public IEnumerable<List<List<FlightDetails>>> lstFlightDetailOnward { get; set; }
                public List<insertMeetingDets> Pax_Passengers { get; set; }
                public List<insertExpenses> Meeting_Expense { get; set; }

                public IEnumerable<List<preparedairline>> preparedairlineList { get; set; }

            }
            public class SelectModel
            {
                public List<List<FlightDetails>> SelectedFlights { get; set; }
                public List<List<List<FlightDetails>>> UnSelectedFlights { get; set; }
                public List<insertMeetingDets> Pax_Passengers { get; set; }
                public List<insertExpenses> Meeting_Expense { get; set; }


            }

            public class preparedairline
            {

                public string CODE { get; set; }
                public string NAME { get; set; }
                public string CATEGORY { get; set; }

            }


            //brindha


            public class SendAvailMailModal
            {
                public string AvailflightContent { get; set; }
                public string frommailid { get; set; }
                public string tomailid { get; set; }
                public string txtmailsubject { get; set; }
                public string txtmailbodytext { get; set; }
                public string clientid { get; set; }

            }

            public class SendExcelModal
            {
                public string Availvalues { get; set; }


            }

            [DataContract]
            public class SEGMENTS
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
                public string DepartureDate
                {
                    get;
                    set;
                }
                [DataMember]
                public string Time
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

            }

            [DataContract]
            public class TravelRequestDetails
            {
                [DataMember]
                public string Authorizationkey { get; set; }
                [DataMember]
                public string EmployeeID { get; set; }
                [DataMember]
                public string RequestID { get; set; }
                [DataMember]
                public string TripType { get; set; }
                [DataMember]
                public string TravelType { get; set; }
                [DataMember]
                public string TravelerCategory { get; set; }
                [DataMember]
                public string ProcessingLocation { get; set; }
                [DataMember]
                public string RequestType { get; set; }
                [DataMember]
                public string Currency { get; set; }
                [DataMember]
                public List<SEGMENTS> TravelRequest
                {
                    get;
                    set;
                }


            }

            public class MISReport
            {
                public string strFromDate { get; set; }
                public string strToDate { get; set; }
                public string corporatecode { get; set; }
                public string CCcode { get; set; }
                public string Empcode { get; set; }
                public string Status { get; set; }
                public string Airportyppe { get; set; }
                public string strloginseesion { get; set; }
                //public string strl2code { get; set; }
                //public string strl3code { get; set; }
                //public string strl4code { get; set; }
                //public string strl5code { get; set; }
                public string strlevel { get; set; }
                public string strCompanyId { get; set; }
                public string Producttype { get; set; }
                public string columnnames { get; set; }
                public string ReqType { get; set; }
                public string PassengerName { get; set; }
                public string columnINSnames { get; set; }

            }



            public class DeptwiseReport
            {
                public string Fromdate { get; set; }
                public string Todatedate { get; set; }
                public string CompanyId { get; set; }
                public string department { get; set; }
            }


            public class FetchCompanyLevel
            {
                public string strLoginsession { get; set; }
                public string strcorporatecode { get; set; }
                public string Usertype { get; set; }
            }

            public class FetchCompanyLevelupdate
            {
                public string strLoginsession { get; set; }
                public string strcorporatecode { get; set; }
                public string strlevel { get; set; }
            }

            public class FlightBookedHistory
            {
                public string Loginsession { get; set; }
                public string strFromdate { get; set; }
                public string strTodate { get; set; }
                public string strSPNR { get; set; }
                public string strAirPNR { get; set; }
                public string strSearchBy { get; set; }
                public string strFname { get; set; }
                public string strLname { get; set; }
                public string strPayMode { get; set; }
                public string strBookedDate { get; set; }
                public string FlaG { get; set; }
            }

            public class MeetingDetails_
            {
                public string COMPANYID { get; set; }
                public string USERID { get; set; }
                public string USERNAME { get; set; }
                public string APPROVERMAILID { get; set; }
                public string TCKTMAILID { get; set; }
                public string USERMAILID { get; set; }
                public string APPROVERNAME { get; set; }
                public string SECONDAPPROVER { get; set; }
                public string ALLOWREPORTS { get; set; }
                public string COMPANYNAME { get; set; }
                public string APPROVERCOSTCENTER { get; set; }
                public string COSTCENTERS { get; set; }
                public string COSTCENTERSID { get; set; }
                public string COSTCENTERNAME { get; set; }
                public string FIRSTNAME { get; set; }
                public string LASTNAME { get; set; }
                public string TITLE { get; set; }
                public string GENDER { get; set; }
                public string DOB { get; set; }
                public string DEPARTMENT { get; set; }
                public string USD_NAME_FIRST_APPROEVER { get; set; }
                public string USD_NAME_SECOND_APPROEVER { get; set; }
                public string COSTCENTER_CODE { get; set; }
                public string LOCATION { get; set; }
                public string L1_CODE { get; set; }
                public string L2_CODE { get; set; }
                public string L3_CODE { get; set; }
                public string L4_CODE { get; set; }
                public string L5_CODE { get; set; }
                public string FS_BOOKINGTOOL { get; set; }
                public string EXEMPID { get; set; }
                public string EXPENSE_OPTION { get; set; }
                public string USD_SUPER_USER { get; set; }
                public string USD_APPROVAL_PROCESS_REQ { get; set; }
                public string COM_MEETING_MANDATORY { get; set; }
                public string PASSPORT_NUMBER { get; set; }
                public string PASSPORT_EXP_DATE { get; set; }
                public string PASSPORT_ISS_COUNTRY { get; set; }
                public string COM_REASON_TRAVEL_MAND { get; set; }//Added by saranraj on 20170209...
                public string singleapprovermail { get; set; }//Added by saranraj on 20170217...
                public string COSTCENTERMANAGER { get; set; }
                public string COSTCENTERMAILID { get; set; }
                public string LOWLEVELEMP { get; set; }
                public string Meals { get; set; }
                public string OFFLINEAPPROVAL { get; set; }
                public string EMPBOOKINGTYPE { get; set; }
                public string TRIPPRODUCTS { get; set; }
                public string USD_NAME_THIRD_APPROEVER { get; set; }
                public string USD_NAME_FOURTH_APPROEVER { get; set; }
                public string USD_NAME_FIFTH_APPROEVER { get; set; }
                public string THIRDAPPROVER { get; set; }
                public string FOURTHAPPROVER { get; set; }
                public string FIFTHAPPROVER { get; set; }
                public string APPROVERTYPE { get; set; }

            }

            public class CostCenterDetails_
            {
                public string COST_CENTER { get; set; }//Added by saranraj on 20170217...
                public string COST_MAILID { get; set; }//Added by saranraj on 20170217...

            }

            public class insertMeetingDet
            {
                public string selectedFlight { get; set; }
                public string UnselectedFlight { get; set; }
                public string MeetingDetailsSLTFlight { get; set; }
                public string ApproverDetails { get; set; }
                public string PassengerDetails { get; set; }
                public string Reasonforchosinghightcostflight { get; set; }
                public string Reasonfortravel { get; set; }
                public string CompanyID { get; set; }
                public string USERID { get; set; }
                public string USERNAME { get; set; }
                public string USERMAILID { get; set; }
                public string COMPANYNAME { get; set; }
                public string TCKTMAILID { get; set; }
                public string LOCATION { get; set; }
                public string L1_CODE { get; set; }
                public string L2_CODE { get; set; }
                public string L3_CODE { get; set; }
                public string L4_CODE { get; set; }
                public string L5_CODE { get; set; }
                public string FS_BOOKINGTOOL { get; set; }
                public string EXEMPID { get; set; }
                public string DEPARTMENT { get; set; }
                public string EXPENSE_OPTION { get; set; }
                public string SUGGESTEDFARE { get; set; }
                public string PAXCOUNTS { get; set; }
                public string PersonalBookingoption { get; set; }
                public string TicketFormat { get; set; }
                public string CartREFID { get; set; }
                public string SecretaryReq_Option { get; set; }
                public string SecretaryReq_UserID { get; set; }
                public string Producttype { get; set; }
                public string LoginCBT_Credentials { get; set; }
                public string Remarks { get; set; }
                public string TripName { get; set; }
                public string COSTCENTERMANAGER { get; set; }
                public string COSTCENTERMAILID { get; set; }
                public string COSTMANGROPTION { get; set; }
                public string LOWLEVELEMP { get; set; }
                public string DEBITECOST { get; set; }
                public string SELECTEDFARE { get; set; }
                public string NEWUSERDETAILS { get; set; }
                public string PackageId { get; set; }
                public string AgentName { get; set; }
                public string Emailid { get; set; }
                public string AgentMobileNo { get; set; }
                public string EMPBOOKINGTYPE { get; set; }
                public string TRIPPRODUCTS { get; set; }
                public string Paxdetails { get; set; }
                public string DepartureDate { get; set; }
                public string strpolicyst { get; set; }
                public string Purposeoftravel { get; set; }
                public string Imagetripid { get; set; }
                public string selectedHotel { get; set; }
                public string selectedCar { get; set; }
                public string selectedRail { get; set; }
                public string TicketDetailsEntryFlag { get; set; }
                public string UserFlag { get; set; }
                public string SectorSpecification { get; set; }
                public string ExceptionApprovarMailFlag { get; set; }
                public string FAREREASONHTL { get; set; }//Added by Anbarasu on 12/7/18
                public string Htlpaxdet { get; set; }
                public string Gstdetails { get; set; }
            }


            public List<hotelliet> hotelde { get; set; }
            public List<htlpaxdetail> htlpaxdetaillst { get; set; }

            public OfflineGST GSTOFFline { get; set; }

            public class htlpaxdetail
            {
                public string Hotelandroom { get; set; }
                public string Type { get; set; }
                public string Title { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string Gender { get; set; }
                public string DOB { get; set; }
                public string PASSPORTNO { get; set; }
                public string PASSPORTEXPIRYDATE { get; set; }
                public string ISSUEDCOUNTRY { get; set; }

            }

            public class OfflineGST
            {
                public string GstState { get; set; }
                public string GstNumber { get; set; }
                public string GstCompanyname { get; set; }
                public string GstAdd { get; set; }
                public string GstEmailids { get; set; }
                public string Gstmobileno { get; set; }

            }
            public class hotelliet
            {
                public string Hotflag { get; set; }
                public string Segment { get; set; }
                public string City { get; set; }
                public string Checkin { get; set; }
                public string Checkout { get; set; }
                public string Adult { get; set; }
                public string NoofRoom { get; set; }
                public string NearestLocations { get; set; }
                public string PreferredHotel { get; set; }
                public string Roompreference { get; set; }

                public string Aggmaild { get; set; }
                public string AggSupplier { get; set; }
                public string GstState { get; set; }
                public string GstNumber { get; set; }
                public string GstCompanyname { get; set; }
                public string GstAdd { get; set; }
                public string GstEmailids { get; set; }
                public string Gstmobileno { get; set; }


            }
            public class TripBookedHistory
            {
                public string Loginsession { get; set; }
                public string strFromdate { get; set; }
                public string strTodate { get; set; }
                public string strTripId { get; set; }
                public string FlaG { get; set; }
            }
            //Seatmaps Model

            public List<BookingresponsePaxdetails> Bookingresponse_Paxdetails { get; set; }

            public class BookingresponsePaxdetails
            {
                public string First { get; set; }
                public string LASTNAME { get; set; }
                public string PAXTYPE { get; set; }
                public string DATEOFBIRTH { get; set; }
                public string TICKETNO { get; set; }
                public string USERTRACKID { get; set; }
                public double GROSSFARE { get; set; }
                public string ServiceCharge { get; set; }
                public string markup { get; set; }
                public double SEATAMD { get; set; }
                public string pax { get; set; }
            }

            public class Seatmaps
            {
                public string valkey { get; set; }
                public string Paxcount { get; set; }
                public string Paxname { get; set; }
                public string Totalpaxcount { get; set; }
                public string Adultnamedetails { get; set; }
                public string Childnamedetails { get; set; }
                public string ClientID { get; set; }
                public string Currencyflag { get; set; }
                public string Domesticmulflagcnt { get; set; }
                public string Sessionkeys { get; set; }

            }



            public class CREDENTIALS
            {
                public string ORIGIN { get; set; }
                public string DESTINATION { get; set; }
                public string DEPARTURE { get; set; }
                public string RETURN { get; set; }
                public string TRIPTYPE { get; set; }
                public string RTSTYPE { get; set; } //RoundTrip Special Type (LCC or FSC ) by saranraj on 20160907

                public string TMCSESSIONDATA { get; set; }//Riya TMC Session data by udhaya 20160907

                public string PASSENGERS { get; set; }
                //public string CHILD { get; set; }
                //public string INFANT { get; set; }
                public string PAXCOUNT { get; set; }
                public string CLASSTYPE { get; set; }
                public string L1_CODE { get; set; }
                public string L2_CODE { get; set; }
                public string L3_CODE { get; set; }
                public string L4_CODE { get; set; }
                public string L5_CODE { get; set; }
                public string COM_BOOKINGTYPE { get; set; }
                public string COM_CODE { get; set; }
                public string COSTCENTERFLAG { get; set; }
                public string TO_COSTCENTER { get; set; }
                public string BOOKTYPE { get; set; }
                public string PURPOSE_OF_MEETING { get; set; }
                public string REASON { get; set; }
                public string USD_DEPARTMENT { get; set; }
                public string USD_LOCATION { get; set; }
                public string CLIENTNAME { get; set; }
                public string EMPEXISTCODE { get; set; }
                public string DEFAULTCOSTCENTER { get; set; }
                public string LOWESTFAREFLAG { get; set; }
                public string EXPENSE_OPTION { get; set; }
                public string PAXCOUNTS { get; set; }
                public string AGENTID { get; set; }
                public string TERMINALID { get; set; }
                public string POS_ID { get; set; }
                public string BRANCH_ID { get; set; }
                public string USERNAME { get; set; }
                public string CLIENT_ID { get; set; }
                public string PERSONALBOOKINGOPTION { get; set; }
                public string TICKETFORMAT { get; set; }
                public string COMPLIANCESTATUS { get; set; }
                public string ComplianceStatusflag { get; set; }
                public string COMPARABLEFLAG { get; set; }
                public string MULTICART { get; set; }
                public string LoginCBT_Credentials { get; set; }
                public string CompanyName { get; set; }
                public string POLICY { get; set; }
                public string SECTORSPECFICATION { get; set; }
                public string CITYID { get; set; }

            }
            public List<insertMeetingDets> PASSENGERSDET { get; set; }


            public class insertExpenses
            {
                public string Date { get; set; }
                public string AccountHead { get; set; }
                public string Subgroup { get; set; }
                public string Description { get; set; }
                public string Amount { get; set; }
            }

            public class insertMeetingDets
            {

                public string PaxType { get; set; }
                public string PaxTitle { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string Gender { get; set; }
                public string DOB { get; set; }
            }


        }

        [DataContract]
        public class FlightDetailsShot
        {
            [DataMember(Name = "FN")]
            public string FlightNumber { get; set; }
            [DataMember(Name = "O")]
            public string Origin { get; set; }
            [DataMember(Name = "D")]
            public string Destination { get; set; }
            [DataMember(Name = "ON")]
            public string OriginName { get; set; }
            [DataMember(Name = "DN")]
            public string DestinationName { get; set; }
            [DataMember(Name = "DD")]
            public string DepartureDate { get; set; }
            [DataMember(Name = "DT")]
            public string DepartureTime { get; set; }
            [DataMember(Name = "AD")]
            public string ArrivalDate { get; set; }
            [DataMember(Name = "AT")]
            public string ArrivalTime { get; set; }
            [DataMember(Name = "FTM")]
            public string FlyingTime { get; set; }
            [DataMember(Name = "C")]
            public string Class { get; set; }
            [DataMember(Name = "CT")]
            public string ClassType { get; set; }
            [DataMember(Name = "CB")]
            public string Cabin { get; set; }
            [DataMember(Name = "AS")]
            public string AvailSeat { get; set; }
            [DataMember(Name = "CC")]
            public string CarrierCode { get; set; }
            [DataMember(Name = "CX")]
            public string CNX { get; set; }
            [DataMember(Name = "CF")]
            public string ConnectionFlag { get; set; }
            [DataMember(Name = "FC")]
            public string FareBasisCode { get; set; }
            [DataMember(Name = "FI")]
            public string FareId { get; set; }
            [DataMember(Name = "FD")]
            public List<RQRS.Faredescription> Faredescription { get; set; }
            [DataMember(Name = "PC")]
            public string PlatingCarrier { get; set; }
            [DataMember(Name = "RT")]
            public string ReferenceToken { get; set; }
            [DataMember(Name = "SR")]
            public string SegRef { get; set; }
            [DataMember(Name = "S")]
            public string Stops { get; set; }
            [DataMember(Name = "V")]
            public string Via { get; set; }
            [DataMember(Name = "BA")]
            public string BaseAmount { get; set; }
            [DataMember(Name = "GA")]
            public string GrossAmount { get; set; }
            [DataMember(Name = "TA")]
            public string TotalTaxAmount { get; set; }
            [DataMember(Name = "CMS")]
            public string Commission { get; set; }
            [DataMember(Name = "T")]
            public List<RQRS.Taxes> Taxes { get; set; }
            [DataMember(Name = "MA")]
            public string MultiClassAmount { get; set; }
            [DataMember(Name = "R")]
            public string Refund { get; set; }
            [DataMember(Name = "STCK")]
            public string Stock { get; set; }
            [DataMember(Name = "IR")]
            public string ItinRef { get; set; }
            [DataMember(Name = "AC")]
            public string Airlinecategory { get; set; }
            [DataMember(Name = "MAC")]
            public string MulticlassAirlinecategory { get; set; }
            [DataMember(Name = "DU")]
            public string Duration { get; set; }
            [DataMember(Name = "CCC")]
            public string ClassCarrierCode { get; set; }
            [DataMember(Name = "FT")]
            public string FareType { get; set; }
            [DataMember(Name = "ST")]
            public string StartTerminal { get; set; }
            [DataMember(Name = "ET")]
            public string EndTerminal { get; set; }
            [DataMember(Name = "JT")]
            public string JourneyTime { get; set; }
            [DataMember(Name = "MCA")]
            public string MulticlassAvail { get; set; }
            [DataMember(Name = "FTPO")]
            public string FareTypeDescription { get; set; }
            //Future use
            [DataMember(Name = "BI")]
            public string BaggageInfo { get; set; }
            [DataMember(Name = "BP")]
            public string BreakPoint { get; set; }
            [DataMember(Name = "CRS")]
            public string CRSID { get; set; }
            [DataMember(Name = "VA")]
            public string via { get; set; }
            [DataMember(Name = "FSR")]
            public string FareSegRef { get; set; }
            [DataMember(Name = "SK")]
            public string STK { get; set; }
            //brindha added for Compliance & NonCompliance
            [DataMember(Name = "CS")]
            public string ComplianceStatus { get; set; }
            [DataMember(Name = "CSF")]
            public string ComplianceStatusflag { get; set; }
            [DataMember(Name = "CMF")]
            public string COMPARABLEFLAG { get; set; }
            [DataMember(Name = "AN")]
            public string AirlineName { get; set; }
            [DataMember(Name = "SF")]
            public string ServiceFee { get; set; }

            //RQRS.Faredescription start...
            [DataMember(Name = "PAX")]
            public string Paxtype { get; set; }
            [DataMember(Name = "BRK")]
            public string API_Breakup { get; set; }
            //wanna add other key values if want in future...
            //RQRS.Faredescription End...

            //RQRS.Taxes start...
            [DataMember(Name = "COD")]
            public string Code { get; set; }
            [DataMember(Name = "AMT")]
            public string Amount { get; set; }
            //RQRS.Taxes End...
            [DataMember(Name = "DPDT")]
            public string Depdate { get; set; }
            [DataMember(Name = "ARDT")]
            public string Arrdate { get; set; }
        }


    }
}