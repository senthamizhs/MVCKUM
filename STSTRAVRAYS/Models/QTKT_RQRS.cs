using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace QUEUERQRS
{

    #region QTRetrieve
    [DataContract]
    public class QueueRetrieveRQ
    {
        [DataMember]
        public AgentDetails AgentDetail
        {
            get;
            set;
        }
        [DataMember]
        public PNRDetail PNR
        {
            get;
            set;
        }

    }
    public class PNRDetail
    {
        [DataMember]
        public string CRSPNR
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
        public string QUEUENUMBER
        {
            get;
            set;
        }
        [DataMember]
        public string AUTOQUEUEFLAG
        {
            get;
            set;
        }
        [DataMember]
        public string QUEUEID
        {
            get;
            set;
        }

    }

    [DataContract]
    public class QueueRetrieveRS
    {
        [DataMember]
        public ResultCode Result
        {
            get;
            set;
        }
        [DataMember]
        public List<PassengerPNRDetail> PassengerPNRDetails
        {
            get;
            set;
        }
        [DataMember]
        public GSTDETAILS GstDetails
        {
            get;
            set;
        }
        [DataMember]
        public string IsFare
        {
            get;
            set;
        }
    }
    #endregion

    #region QTTicketing
    [DataContract]
    public class QueueTicketingRQ
    {
        [DataMember]
        public AgentDetails AgentDetail
        {
            get;
            set;
        }
        [DataMember]
        public List<TicketPnrDetail> TicketPnrDetails
        {
            get;
            set;
        }
        [DataMember]
        public PNRDetails PNRDetail
        {
            get;
            set;
        }
        [DataMember]
        public Payment PaymentDetail
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
        public GSTDETAILS GstDetails
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
        public CBT_Credentials CBT_Input
        {
            get;
            set;
        }
        [DataMember]
        public string TicketingMode
        {
            get;
            set;
        }
        [DataMember]
        public bool IsGST
        {
            get;
            set;
        }
        [DataMember]
        public bool ISAllFare
        {
            get;
            set;
        }
        [DataMember]
        public List<Earning> Earnings
        {
            get; 
            set; 
        }
        [DataMember]
        public bool ISReeBook
        {
            get;
            set;
        }
        [DataMember]
        public bool ISPNRFOP
        {
            get;
            set;
        }
        [DataMember]
        public PAYMENT_INFO PAYMENT_INFO
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
        public string InfantRef
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
    }
    [DataContract]
    public class FFNumber
    {
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
    }

    [DataContract]
    public class QueueTicketingRS
    {
        [DataMember]
        public ResultCode Result
        {
            get;
            set;
        }
        [DataMember]
        public List<PassengerPNRDetail> PassengerPNRDetails
        {
            get;
            set;
        }
    }

    [DataContract]
    public class PNRDetails
    {
        [DataMember]
        public string CRSPNR
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
        public string CORPORATECODE
        {
            get;
            set;
        }
        [DataMember]
        public string TOURCODE
        {
            get;
            set;
        }
        [DataMember]
        public string BOOKINGAMOUNT
        {
            get;
            set;
        }
        [DataMember]
        public string QUEUENUMBER
        {
            get;
            set;
        }
        [DataMember]
        public string ADULT
        {
            get;
            set;
        }
        [DataMember]
        public string CHILD
        {
            get;
            set;
        }
        [DataMember]
        public string INFANT
        {
            get;
            set;
        }
        [DataMember]
        public string SEGMENTTYPE
        {
            get;
            set;
        }
        [DataMember]
        public string FAREQUALIFIER
        {
            get;
            set;
        }
        [DataMember]
        public string OBCTax
        {
            get;
            set;
        }
        [DataMember]
        public string MARKUP { get; set; }
        [DataMember]
        public string EARNINGS { get; set; }
        [DataMember]
        public string PLBAmount
        {
            get;
            set;
        }
        [DataMember]
        public string Endorsement
        {
            get;
            set;
        }
    }

    [DataContract]
    public class Payment
    {
        [DataMember]
        public string PAYMENTMODE
        {
            get;
            set;
        }
        [DataMember]
        public string CARDTYPE
        {
            get;
            set;
        }
        [DataMember]
        public string CARDNUMBER
        {
            get;
            set;
        }
        [DataMember]
        public string EXPIRYDATE
        {
            get;
            set;
        }
        [DataMember]
        public string CVVNUMBER
        {
            get;
            set;
        }
        [DataMember]
        public string CARDNAME
        {
            get;
            set;
        }
        [DataMember]
        public string REFERANSEID
        {
            get;
            set;
        }
        [DataMember]
        public string PASSENGER_CONTACTNO
        {
            get;
            set;
        }
        [DataMember]
        public string PaymentInfo
        {
            get;
            set;
        }

    }

    [DataContract]
    public class TicketPnrDetail
    {
        [DataMember]
        public string AIRLINEPNR
        {
            get;
            set;
        }
        [DataMember]
        public string CARRIERCODE
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
        public string CLASS
        {
            get;
            set;
        }
        [DataMember]
        public string FAREBASISCODE
        {
            get;
            set;
        }

        [DataMember]
        public string TSTREFERENCE
        {
            get;
            set;
        }
        [DataMember]
        public string SEGMENTREFERENCE
        {
            get;
            set;
        }
        [DataMember]
        public string PAXREFERENCE
        {
            get;
            set;
        }
        [DataMember]
        public string SEGMENTNO
        {
            get;
            set;
        }
        [DataMember]
        public string PAXNO
        {
            get;
            set;
        }
        [DataMember]
        public string PLATINGCARRIER
        {
            get;
            set;
        }
        [DataMember]
        public string TSTCOUNT
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
        [DataMember]
        public string AttributesName { get; set; }
        [DataMember]
        public string AttributesValue { get; set; }
    }

    #endregion

    #region QTFarePricing
    [DataContract]
    public class QueueTicketingFarePricingRQ
    {
        [DataMember]
        public AgentDetails AgentDetail
        {
            get;
            set;
        }
        [DataMember]
        public FarePricePNRDetail PricePNRDetail
        {
            get;
            set;
        }
        [DataMember]
        public List<PricingSegmentDetails> PriceSegment
        {
            get;
            set;
        }
        [DataMember]
        public GSTDETAILS GstDetails
        {
            get;
            set;
        }
        [DataMember]
        public Payment PaymentDetail
        {
            get;
            set;
        }
        [DataMember]
        public bool ISAllFare
        {
            get;
            set;
        }
    }

    [DataContract]
    public class QueueTicketingFarePricingRS
    {
        [DataMember]
        public ResultCode Result
        {
            get;
            set;
        }
        [DataMember]
        public string PricingToken
        {
            get;
            set;
        }
        [DataMember]
        public List<PricingDetail> FarePriceDetails
        {
            get;
            set;
        }
        public List<OBCDetails> OBCDetails
        {
            get;
            set;
        }
        [DataMember]
        public string SupplierCommission
        {
            get;
            set;
        }
        //[DataMember]
        //public OBCDetails OBCDetails
        //{
        //    get;
        //    set;
        //}
    }

    [DataContract]
    public class OBCDetails
    {
        //[DataMember]
        //public string OBCAmount
        //{
        //    get;
        //    set;
        //}
        //[DataMember]
        //public string CardNumber
        //{
        //    get;
        //    set;
        //}
        //[DataMember]
        //public string CardName
        //{
        //    get;
        //    set;
        //}
        //[DataMember]
        //public string ISview
        //{
        //    get;
        //    set;
        //}

        [DataMember]
        public string OBCAmount
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
        [DataMember]
        public string CardNumber
        {
            get;
            set;
        }
        [DataMember]
        public string CardMode
        {
            get;
            set;
        }
        [DataMember]
        public string ISview
        {
            get;
            set;
        }

    }

    [DataContract]
    public class FarePricePNRDetail
    {
        [DataMember]
        public string CRSPNR
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
        public string CORPORATECODE
        {
            get;
            set;
        }
        [DataMember]
        public string TOURCODE
        {
            get;
            set;
        }
        [DataMember]
        public string QUEUENUMBER
        {
            get;
            set;
        }
        [DataMember]
        public string BOOKINGAMOUNT
        {
            get;
            set;
        }
        [DataMember]
        public string ADULT
        {
            get;
            set;
        }
        [DataMember]
        public string CHILD
        {
            get;
            set;
        }
        [DataMember]
        public string INFANT
        {
            get;
            set;
        }
        [DataMember]
        public string SEGMENTTYPE
        {
            get;
            set;
        }
        [DataMember]
        public string AUTOQUEUEFLAG
        {
            get;
            set;
        }
        [DataMember]
        public string QUEUEID
        {
            get;
            set;
        }
    }
    [DataContract]
    public class PricingDetail
    {

        [DataMember]
        public string CRSPNR
        {
            get;
            set;
        }
        [DataMember]
        public string GROSSFARE
        {
            get;
            set;
        }
        [DataMember]
        public string ADTBASEFARE
        {
            get;
            set;
        }
        [DataMember]
        public string CHDBASEFARE
        {
            get;
            set;
        }
        [DataMember]
        public string INFBASEFARE
        {
            get;
            set;
        }

        [DataMember]
        public string ADTGROSSFARE
        {
            get;
            set;
        }
        [DataMember]
        public string CHDGROSSFARE
        {
            get;
            set;
        }
        [DataMember]
        public string INFGROSSFARE
        {
            get;
            set;
        }
        [DataMember]
        public string ADTTAXBREAKUP
        {
            get;
            set;
        }
        [DataMember]
        public string CHDTAXBREAKUP
        {
            get;
            set;
        }
        [DataMember]
        public string INFTAXBREAKUP
        {
            get;
            set;
        }
        [DataMember]
        public string DIFFERENCEAMOUNT
        {
            get;
            set;
        }
        [DataMember]
        public string CURRENCY
        {
            get;
            set;
        }
        [DataMember]
        public string CORPORATECODE
        {
            get;
            set;
        }
        [DataMember]
        public string FAREBASISCODE
        {
            get;
            set;
        }
        [DataMember]
        public string CLASS
        {
            get;
            set;
        }
        [DataMember]
        public string ADTALLOWBAGGAGE
        {
            get;
            set;
        }
        [DataMember]
        public string CHDALLOWBAGGAGE
        {
            get;
            set;
        }
        [DataMember]
        public string INFALLOWBAGGAGE
        {
            get;
            set;
        }
        [DataMember]
        public string FREETEXT
        {
            get;
            set;
        }
        [DataMember]
        public string REFTOKEN
        {
            get;
            set;
        }
        [DataMember]
        public string FAREQUALIFIER
        {
            get;
            set;
        }
        [DataMember]
        public string MARKUP
        {
            get;
            set;
        }
        [DataMember]
        public string EARNINGS
        {
            get;
            set;
        }
        [DataMember]
        public string SERVICECHARGE
        {
            get;
            set;
        }
        [DataMember]
        public string INCENTIVE
        {
            get;
            set;
        }
        [DataMember]
        public string TDS_AMT
        {
            get;
            set;
        }
        [DataMember]
        public string PLB_AMT
        {
            get;
            set;
        }
        [DataMember]
        public string EARNINGS_REF_ID
        {
            get;
            set;
        }
    }

    [DataContract]
    public class PricingSegmentDetails
    {

        [DataMember]
        public string CARRIERCODE
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
        public string CLASS
        {
            get;
            set;
        }
        [DataMember]
        public string FAREBASISCODE
        {
            get;
            set;
        }
        [DataMember]
        public string PLATINGCARRIER
        {
            get;
            set;
        }
        [DataMember]
        public string SEGMENTREFERENCE
        {
            get;
            set;
        }
        [DataMember]
        public string TSTREFERENCE
        {
            get;
            set;
        }
        [DataMember]
        public string PAXTSTREFERENCE
        {
            get;
            set;
        }
    }

    #endregion

    [DataContract]
    public class PassengerPNRDetail
    {
        [DataMember]
        public string SPNR
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
        public string PAXTYPE
        {
            get;
            set;
        }
        [DataMember]
        public string TITLE
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
        public string DATEOFBIRTH
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
        public string TICKETINGCARRIER
        {
            get;
            set;
        }
        [DataMember]
        public string AIRLINECATEGORY
        {
            get;
            set;
        }
        [DataMember]
        public string USERTRACKID
        {
            get;
            set;
        }
        [DataMember]
        public string OFFICEID
        {
            get;
            set;
        }
        [DataMember]
        public string QUEUINGOFFICEID
        {
            get;
            set;
        }
        [DataMember]
        public string CLASS
        {
            get;
            set;
        }
        [DataMember]
        public string FAREBASISCODE
        {
            get;
            set;
        }
        [DataMember]
        public string BASEAMT
        {
            get;
            set;
        }
        [DataMember]
        public string TOTALTAXAMT
        {
            get;
            set;
        }
        [DataMember]
        public string GROSSAMT
        {
            get;
            set;
        }
        [DataMember]
        public string TAXBREAKUP
        {
            get;
            set;
        }
        [DataMember]
        public string MARKUP
        {
            get;
            set;
        }
        [DataMember]
        public string EARNINGS
        {
            get;
            set;
        }
        [DataMember]
        public string SERVICECHARGE
        {
            get;
            set;
        }
        [DataMember]
        public string INCENTIVE
        {
            get;
            set;
        }
        [DataMember]
        public string TDS_AMT
        {
            get;
            set;
        }
        [DataMember]
        public string PLB_AMT
        {
            get;
            set;
        }
        [DataMember]
        public string FQTV
        {
            get;
            set;
        }
        [DataMember]
        public string MEALS
        {
            get;
            set;
        }
        [DataMember]
        public string CURRENCY
        {
            get;
            set;
        }
        [DataMember]
        public string BAGGAGE
        {
            get;
            set;
        }
        [DataMember]
        public string TOURCODE
        {
            get;
            set;
        }
        [DataMember]
        public string SEGMENTNO
        {
            get;
            set;
        }
        [DataMember]
        public string REFERENCE
        {
            get;
            set;
        }
        [DataMember]
        public string PAXNO
        {
            get;
            set;
        }
        [DataMember]
        public string LOGINTYPE
        {
            get;
            set;
        }
        [DataMember]
        public string PAYMENTINFO
        {
            get;
            set;
        }
        [DataMember]
        public string TSTREFERENCE
        {
            get;
            set;
        }
        [DataMember]
        public string SEGMENTREFERENCE
        {
            get;
            set;
        }
        [DataMember]
        public string PAXREFERENCE
        {
            get;
            set;
        }
        [DataMember]
        public string TSTCOUNT
        {
            get;
            set;
        }
        [DataMember]
        public string PASSPORTNO
        {
            get;
            set;
        }
        [DataMember]
        public string ISSUED_COUNTRY
        {
            get;
            set;
        }
        [DataMember]
        public string PASSPORT_EXPAIRY
        {
            get;
            set;
        }
        [DataMember]
        public string PLB 
        { 
            get; 
            set; 
        }
        [DataMember]
        public string EARNINGS_REF_ID
        {
            get;
            set;
        }
    }

    #region Common
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
        [DataMember]
        public string AppType
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
        public string BranchID
        {
            get;
            set;
        }
        [DataMember]
        public string ProductType
        {
            get;
            set;
        }
        [DataMember]
        public string PNROfficeId
        {
            get;
            set;
        }
        [DataMember]
        public string TicketingOfficeId
        {
            get;
            set;
        }
        [DataMember]
        public string BOATerminalID
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
        public string Version
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
        public string AgentType
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
        public string IssuingBranchID
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
        public string COST_CENTER
        {
            get;
            set;
        }
        [DataMember]
        public string Ipaddress
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
        public string ProjectID
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
        public string BOATreminalID
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
        public string Airportid
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


    }

    [DataContract]
    public class GSTDETAILS
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
    public class ResultCode
    {
        [DataMember]
        public string Code
        {
            get;
            set;
        }
        [DataMember]
        public string ErrorDescription
        {
            get;
            set;
        }
        [DataMember]
        public string ErrorDisplay
        {
            get;
            set;
        }
        [DataMember]
        public string RebookingFare
        {
            get;
            set;
        }
        [DataMember]
        public string OPCAmount
        {
            get;
            set;
        }
    }

    #endregion


    #region RetrivePNR Accounting

    public class RetrivePNRBOA_RQ
    {
        // public RetrivePNRBOA_RQ();

        public AgentDetails AgentDetail { get; set; }
        public string AirlineCategory { get; set; }
        public string AirlinePNR { get; set; }
        public string BookPCC { get; set; }
        public string BookSupp { get; set; }
        public string CRSID { get; set; }
        public string CRSPNR { get; set; }
        public string FareType { get; set; }
        public string Platform { get; set; }
        public bool Ticketing { get; set; }
        public string TicketPCC { get; set; }
        public string TicketSupp { get; set; }
        public string TrackID { get; set; }
    }


    #endregion

    public class RetriveResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }
        public string SessionKey { get; set; }
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
        public string PlatForm
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
        public string CorporateCode
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

        [DataMember]
        public FareRule FareRule
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
    }
    [DataContract]
    public class Status
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
        public String SequenceID
        {
            get;
            set;
        }
    }
    [DataContract]
    public class FareRule
    {
        [DataMember]
        public string Fare_Rule { get; set; }
        [DataMember]
        public string FareRuleText { get; set; }
    }
    #endregion

    [DataContract]
    public class Earning
    {
        [DataMember]
        public string PAXNO
        {
            get;
            set;
        }
        [DataMember]
        public string SEGMENTNO 
        { 
            get; 
            set; 
        }
        [DataMember]
        public string OBCTax
        {
            get;
            set;
        }
        [DataMember]
        public string MARKUP
        {
            get;
            set;
        }
        [DataMember]
        public string SERVICE_FEE
        {
            get;
            set;
        }
        [DataMember]
        public string COMM
        {
            get;
            set;
        }
        [DataMember]
        public string PLBAmount
        {
            get;
            set;
        }
        [DataMember]
        public string INCENTIVE
        {
            get;
            set;
        }
        [DataMember]
        public string MARKUP_ON_FARE
        {
            get;
            set;
        }
        [DataMember]
        public string MARKUP_ON_TAX
        {
            get;
            set;
        }
        [DataMember]
        public string NEW_COMM { get; set; }
        [DataMember]
        public string NEW_PLBAmount { get; set; }
        [DataMember]
        public string NEW_INCENTIVE { get; set; }
        [DataMember]
        public string NEW_MARKUP_ON_FARE { get; set; }
        [DataMember]
        public string NEW_MARKUP_ON_TAX { get; set; }
        [DataMember]
        public string NEW_SERVICE_FEE { get; set; }
        [DataMember]
        public string DIFF_FLAG { get; set; }
        [DataMember]
        public string APPROVED_BY { get; set; }
    }

    #region Cash Payment info details

    [DataContract]
    public class PAYMENT_INFO
    {
        [DataMember]
        public string EMPLOYEE_CODE
        {
            get;
            set;
        }
        [DataMember]
        public string EMPLOYEE_NAME
        {
            get;
            set;
        }
        [DataMember]
        public string EMPLOYEE_EMAILID
        {
            get;
            set;
        }
        [DataMember]
        public string EMPLOYEE_BRANCH
        {
            get;
            set;
        }
        [DataMember]
        public string CUSTOMER_MOBILENO
        {
            get;
            set;
        }
        [DataMember]
        public string CUSTOMER_EMAILID
        {
            get;
            set;
        }
        [DataMember]
        public string BOOKING_REFERENCE
        {
            get;
            set;
        }
        [DataMember]
        public string EXPECT_PAYMENT
        {
            get;
            set;
        }
        [DataMember]
        public string PAYMENT_MODE
        {
            get;
            set;
        }
        [DataMember]
        public string PAN_CARD
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
        public string BOOKED_BY
        {
            get;
            set;
        }
    }
    #endregion
}



