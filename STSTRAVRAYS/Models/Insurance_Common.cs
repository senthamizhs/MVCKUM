using System.Collections.Generic;
using System.Runtime.Serialization;

namespace STSTRAVRAYS.Models
{
    public class Insurance_Common
    {
        #region IssuePolicyRQ & IssuePolicyRS
        [DataContract]
        public class IssuePolicyRQ
        {
            [DataMember]
            public AgentDetails AgentDetails
            {
                get;
                set;
            }
            [DataMember]
            public PlanDetails PlanDetails
            {
                get;
                set;
            }

            [DataMember]
            public FlightDetails FlightDetails
            {
                get;
                set;
            }
            [DataMember]
            public GST_Details GST_Details
            {
                get;
                set;
            }
            [DataMember]
            public Payment Payment
            {
                get;
                set;
            }
            [DataMember]
            public StudentDetails StudentDetails
            {
                get;
                set;
            }
            [DataMember]
            public StudentSponsorDetails StudentSponsorDetails
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
            public string Stock
            {
                get;
                set;
            }
            [DataMember]
            public string MONumber
            {
                get;
                set;
            }
            [DataMember]
            public string SelectToken
            {
                get;
                set;
            }
            [DataMember]
            public string Trackid
            {
                get;
                set;
            }
            [DataMember]
            public string PgTrackid
            {
                get;
                set;
            }
            [DataMember]
            public string TrackCreate
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
            public List<ERP_Attribute> ERP_Attributes
            {
                get;
                set;
            }
            [DataMember]
            public Credentials Credentials
            {
                get;
                set;
            }
            [DataMember]
            public string MultiRequest
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
            public string TrackPending { get; set; }
            [DataMember]
            public string AirlineTrackid
            {
                get;
                set;
            }
            [DataMember]
            public string AirlineSPNR
            {
                get;
                set;
            }
            [DataMember]
            public string Supplierid
            {
                get;
                set;
            }
        }

        [DataContract]
        public class IssuePolicyRS
        {
            [DataMember]
            public Status Status
            {
                get;
                set;
            }
            [DataMember]
            public PlanDetails PlanDetails
            {
                get;
                set;
            }
            [DataMember]
            public IssuePolicyDetails IssuePolicyDetails
            {
                get;
                set;
            }
            [DataMember]
            public string Trackid
            {
                get;
                set;
            }
        }
        #endregion

        #region AgentDetails
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
            public string TerminalId
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
            public string PostId
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
            [DataMember]
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
            public string BOATerminalID
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
            public string ProductType
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
            public string Emailid
            {
                get;
                set;
            }
            [DataMember]
            public string ContactNo
            {
                get;
                set;
            }
        }
        #endregion

        #region PlanDetails
        [DataContract]
        public class PlanDetails
        {
            [DataMember]
            public string PlanName { get; set; }
            [DataMember]
            public string PlanCode { get; set; }
            [DataMember]//Gold, Silver, Standard
            public string PlanType { get; set; }
            [DataMember]//GOLD
            public string PlanTypeCode { get; set; }
            [DataMember]//Individual, Student
            public string TravelType { get; set; }
            [DataMember]//World Wide Excluding America
            public string PlanDescription { get; set; }
            [DataMember]
            public string NoOfUnits { get; set; }
            [DataMember]
            public string BenefitCode { get; set; }
            [DataMember]
            public string PlanDescCode { get; set; }
            [DataMember]
            public string FromDate { get; set; }
            [DataMember]
            public string ToDate { get; set; }
            [DataMember]
            public string PolicyLink { get; set; }
            [DataMember]
            public string FamilyFlag { get; set; }
            [DataMember]
            public string MinAge { get; set; }
            [DataMember]
            public string MaxAge { get; set; }
            [DataMember]
            public string MinDays { get; set; }
            [DataMember]
            public string MaxDays { get; set; }
            [DataMember]
            public string IsPermanentSameasCommAddr { get; set; }
            [DataMember]
            public string IsInsuredOnImmigrantVisa { get; set; }
            [DataMember]
            public string IsTravelInvolvesSportingActivities { get; set; }
            [DataMember]
            public string SportsActivitiesID { get; set; }
            [DataMember]
            public string PreExistDiseaseID { get; set; }
            [DataMember]
            public string IsSufferingFromPEMC { get; set; }
            [DataMember]
            public string SeniorCitizenPlanID { get; set; }
            [DataMember]
            public string AddOnBnifitsOpted { get; set; }
            [DataMember]
            public string NoOfYears { get; set; }
            [DataMember]
            public string CoverageTypeID { get; set; }
            [DataMember]
            public string MaxDaysPerTrip { get; set; }
            [DataMember]
            public List<Passengerdetails> Passengerdetails
            {
                get;
                set;
            }
            [DataMember]
            public ContactDetails ContactDetails { get; set; }
            [DataMember]
            public string Stock { get; set; }
            [DataMember]
            public string Retoken { get; set; }
        }

        [DataContract]
        public class PremiumPlanDetails
        {
            [DataMember]
            public string PlanName { get; set; }
            [DataMember]
            public string PlanCode { get; set; }
            [DataMember]//Gold, Silver, Standard
            public string PlanType { get; set; }
            [DataMember]//Individual, Student
            public string TravelType { get; set; }
            [DataMember]//World Wide Excluding America
            public string PlanDescription { get; set; }
            [DataMember]
            public string FromDate { get; set; }
            [DataMember]
            public string ToDate { get; set; }
            [DataMember]
            public string FamilyFlag { get; set; }
            [DataMember]
            public string MinAge { get; set; }
            [DataMember]
            public string MaxAge { get; set; }
            [DataMember]
            public string MinDays { get; set; }
            [DataMember]
            public string MaxDays { get; set; }
            [DataMember]
            public string TotalPremium { get; set; }
            [DataMember]
            public string Currency { get; set; }
        }
        #endregion

        #region Status
        [DataContract]
        public class Status
        {
            [DataMember]
            public string ResultCode { get; set; }
            [DataMember]
            public string ErrorMessage { get; set; }
            [DataMember]
            public string APIError { get; set; }
            [DataMember]
            public string TrackID { get; set; }

        }
        #endregion

        #region FlightDetails
        [DataContract]
        public class FlightDetails
        {
            [DataMember]
            public string FlightNumber { get; set; }
            [DataMember]
            public string CarrierCode { get; set; }
            [DataMember]
            public string ReturnFlightNumber { get; set; }
            [DataMember]
            public string ReturnCarrierCode { get; set; }
            [DataMember]
            public string Origin { get; set; }
            [DataMember]
            public string Destination { get; set; }
            [DataMember]
            public string DepartureDateTime { get; set; }
            [DataMember]
            public string ReturnDateTime { get; set; }
            [DataMember]
            public string DepCountryCode { get; set; }
            [DataMember]
            public string ArrvCountryCode { get; set; }
            [DataMember]
            public string DepCurrencyCode { get; set; }
            [DataMember]
            public string AirlinePNR { get; set; }
            [DataMember]
            public string Airline_SPNR { get; set; }
        }
        #endregion

        #region Passengerdetails
        [DataContract]
        public class Passengerdetails
        {
            //[DataMember]
            //public string Salutation { get; set; }
            [DataMember]
            public string PaxRefNumber { get; set; }
            [DataMember]
            public string Title { get; set; }
            [DataMember]
            public string FirstName { get; set; }
            [DataMember]
            public string LastName { get; set; }
            [DataMember]
            public string DOB { get; set; }
            [DataMember]
            public string Gender { get; set; }
            [DataMember]
            public string PaxType { get; set; }
            [DataMember]
            public string PassportNo { get; set; }
            [DataMember]
            public string Mobnumber { get; set; }
            [DataMember]
            public string EMailID { get; set; }
            [DataMember]
            public string Age { get; set; }
            [DataMember]
            public string Relation { get; set; }
            [DataMember]
            public string Assignee { get; set; }
            [DataMember]
            public string IsVisitingUSACanada { get; set; }
            [DataMember]
            public string VisitingCountriesID { get; set; }
            [DataMember]
            public string IsResidingInIndia { get; set; }
            [DataMember]
            public string PassportIssuingCountry { get; set; }
            [DataMember]
            public string NomineeID { get; set; }
            [DataMember]
            public string NomineeName { get; set; }
            [DataMember]
            public string PermanentResidenceCountry { get; set; }
            [DataMember]
            public string IsAddOnCover { get; set; }
            [DataMember]
            public string PreExistingIllness { get; set; }
            [DataMember]
            public string SufferingSince { get; set; }
            [DataMember]
            public string strOccupationID { get; set; }
            [DataMember]
            public string strIsIndianCitizen { get; set; }
            [DataMember]
            public string BaseFare { get; set; }
            [DataMember]
            public string GrossFare { get; set; }
            [DataMember]
            public string ServiceTax { get; set; }
            [DataMember]
            public string BreakUp { get; set; }
            [DataMember]
            public string Ridercode { get; set; }
            [DataMember]
            public string RiderPercent { get; set; }
            [DataMember]
            public string RiderNames { get; set; }
            [DataMember]
            public string RiderAmount { get; set; }
            [DataMember]
            public string Nationality { get; set; }
            [DataMember]
            public string AirlinePaxRefNumber { get; set; }

        }
        #endregion

        #region ContactDetails
        [DataContract]
        public class ContactDetails
        {
            [DataMember]
            public string PrimaryAddressDetails { get; set; }
            [DataMember]
            public string SecondaryAddressDetails { get; set; }
            [DataMember]
            public string Location { get; set; }
            [DataMember]
            public string CityName { get; set; }
            [DataMember]
            public string CityID { get; set; }
            [DataMember]
            public string CountryName { get; set; }
            [DataMember]
            public string StateName { get; set; }
            [DataMember]
            public string StateID { get; set; }
            [DataMember]
            public string DistrictID { get; set; }
            [DataMember]
            public string District { get; set; }
            [DataMember]
            public string ContactNumber { get; set; }
            [DataMember]
            public string PostCode { get; set; }
            [DataMember]
            public string EmailID { get; set; }
            [DataMember]
            public string TelephoneNumber { get; set; }
            [DataMember]
            public string ContactName { get; set; }
        }
        #endregion

        #region IssuePolicyDetails
        [DataContract]
        public class IssuePolicyDetails
        {
            [DataMember]
            public string TrackId { get; set; }
            [DataMember]
            public string PolicyNumber { get; set; }
            [DataMember]
            public string SPNR { get; set; }
            [DataMember]
            public string IssuePolicyStatus { get; set; }
            [DataMember]
            public string TotalPremium { get; set; }
            [DataMember]
            public string BaseAmount { get; set; }
            [DataMember]
            public string PlanName { get; set; }
            [DataMember]
            public string FromDate { get; set; }
            [DataMember]
            public string ToDate { get; set; }
            [DataMember]
            public string Mobnumber { get; set; }
            [DataMember]
            public string EmailID { get; set; }
            [DataMember]
            public string Address { get; set; }
            [DataMember]
            public string ServiceTax { get; set; }
            [DataMember]
            public string AirlinePNR { get; set; }
        }
        #endregion

        #region GST_Details
        [DataContract]
        public class GST_Details
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

        #endregion

        #region StudentDetails
        [DataContract]
        public class StudentDetails
        {
            [DataMember]
            public string UniversityName
            {
                get;
                set;
            }
            [DataMember]
            public string UniversityCountryId
            {
                get;
                set;
            }
            [DataMember]
            public string UniversityStateName
            {
                get;
                set;
            }
            [DataMember]
            public string CityName
            {
                get;
                set;
            }
            [DataMember]
            public string PhoneNumber
            {
                get;
                set;
            }
            [DataMember]
            public string MobileNumber
            {
                get;
                set;
            }
            [DataMember]
            public string EmailId
            {
                get;
                set;
            }
            [DataMember]
            public string Fax
            {
                get;
                set;
            }
            [DataMember]
            public string CourseDuration
            {
                get;
                set;
            }
            [DataMember]
            public string NoOfSems
            {
                get;
                set;
            }
        }
        #endregion

        #region StudentSponsorDetails
        [DataContract]
        public class StudentSponsorDetails
        {
            [DataMember]
            public string SponsorName
            {
                get;
                set;
            }
            [DataMember]
            public string Address1
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
            [DataMember]
            public string CountryID
            {
                get;
                set;
            }
            [DataMember]
            public string MobileNo
            {
                get;
                set;
            }
        }
        #endregion

        #region PaymentDetails
        [DataContract]
        public class Payment
        {
            [DataMember]
            public string PaymentMode { get; set; }
            [DataMember]
            public string BaseFare { get; set; }
            [DataMember]
            public string ServiceTax { get; set; }
            [DataMember]
            public string GrossFare { get; set; }
            [DataMember]
            public string Commission { get; set; }
            [DataMember]
            public string Incentive { get; set; }
            [DataMember]
            public string Markup { get; set; }
            [DataMember]
            public string Servicecharge { get; set; }
            [DataMember]
            public string Flatfare { get; set; }
            [DataMember]
            public string Breakup { get; set; }
            [DataMember]
            public string TDSfare { get; set; }
        }
        #endregion

        #region Credentials
        [DataContract]
        public class Credentials
        {
            [DataMember]
            public string Agency_Name { get; set; }
            [DataMember]
            public string Branch_Name { get; set; }
            [DataMember]
            public string Branch_ID { get; set; }
            [DataMember]
            public string City_Name { get; set; }
            [DataMember]
            public string State_Name { get; set; }
            [DataMember]
            public string PCC { get; set; }
        }

        #endregion

        [DataContract]
        public class ERP_Attribute
        {
            [DataMember]
            public string PaxRefNumber { get; set; }
            [DataMember]
            public string AttributesName { get; set; }
            [DataMember]
            public string AttributesValue { get; set; }
        }

    }
}