using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace STSTRAVRAYS.Models
{
    public class GdsRQRS
    {
        public class RQRS_ancillary
        {

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
                [DataMember]
                public string Group_ID
                {
                    get;
                    set;
                }
            }

            #region FareRule Request
            [DataContract]
            public class FareRuleRQ
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
                public string Stock
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
                public string FetchType
                {
                    get;
                    set;
                }
                [DataMember]
                public string TicketNo
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
                public bool OflineFlag
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
                public string FareType
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
                public string PromoCode
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




            #endregion

            #region FareRule Response
            [DataContract]
            public class FareRuleRS
            {

                [DataMember(Name = "FRL")]
                public FareRule FareRule
                {
                    get;
                    set;
                }
                [DataMember(Name = "STS")]
                public Status Status
                {
                    get;
                    set;
                }
            }


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
            }



            [DataContract]
            public class FareRule
            {
                [DataMember(Name = "FAR")]
                public string Fare_Rule { get; set; }
                [DataMember(Name = "FRTx")]
                public string FareRuleText { get; set; }
            }
            #endregion

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



            }
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
                [DataMember(IsRequired = true, Name = "CAT")]
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
                [DataMember(Name = "UDF")]
                public string UserDefineFlag
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
                public string Platform
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
                [DataMember(Name = "FN")]
                public string FlightNo
                {
                    get;
                    set;
                }
                [DataMember(Name = "VIA")]
                public string Connection
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

            #region Avail Response

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
                [DataMember(Name = "HFR")]
                public List<Fares> Fares
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
                [DataMember(Name = "UDF")]
                public string UserDefineFlag
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


            [DataContract]
            public class Faredescription
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
                [DataMember(Name = "PLB")]
                public string PLBAmount
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
            public class Taxes
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

            [DataContract]
            public class Flights
            {
                [DataMember(Name = "FRI")]
                public string FareId
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
                [DataMember(Name = "CNF")]
                public string ConnectionFlag
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
                [DataMember(Name = "SGD")]
                public string SegmentDetails
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
                [DataMember(Name = "VIA")]
                public string Via
                {
                    get;
                    set;
                }

            }

            [DataContract]
            public class Fares
            {
                [DataMember(Name = "FDC")]
                public List<Faredescription> Faredescription
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
                public List<GdsRQRS.Faredescription> Faredescription { get; set; }
                public string PlatingCarrier { get; set; }
                public string ReferenceToken { get; set; }
                public string SegRef { get; set; }
                public string Stops { get; set; }
                //public string Via { get; set; }
                public string BaseAmount { get; set; }
                public string GrossAmount { get; set; }
                public string TotalTaxAmount { get; set; }
                public string Commission { get; set; }
                public List<GdsRQRS.Taxes> Taxes { get; set; }
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
            

        }
        public class AgentDetails
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
            [DataMember]
            public string EMP_ID
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
        }
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
            public List<Flights> FlightDetails
            {
                get;
                set;
            }
            [DataMember(Name = "FRD")]
            public List<Fares> FareDetails
            {
                get;
                set;
            }
        }
        [DataContract]
        public class Flights
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
        public class Fares
        {
            [DataMember(Name = "FDC")]
            public List<Faredescription> Faredescription
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
        public class Faredescription
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
        public class Taxes
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
            [DataMember]
            public String TripType
            {
                get;
                set;
            }
        }
        [DataContract]
        public class SegmentDetails
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
            public string FareTypeDescription
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

        #region Credentials
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
        }
        #endregion

    }
   
}