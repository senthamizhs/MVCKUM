using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace STSTRAVRAYS.Models
{
    public class BLL
    {
        STSTRAVRAYS.Models.Base.ServiceUtility Serv = new STSTRAVRAYS.Models.Base.ServiceUtility();

        //public ArrayList LoadFlightdetails(string ValKey, int td_Adult, int td_child, int td_infant, double sercharge, string Mealamount, string baggageamount, string Seatamount, string ViewStateSSR, DataTable getflightdetails)//--?
        //{

        //    ArrayList array_Response = new ArrayList();


        //    array_Response.Add("");
        //    array_Response.Add("");
        //    array_Response.Add("");
        //    array_Response.Add("");
        //    array_Response.Add("");
        //    array_Response.Add("");
        //    array_Response.Add("");

        //    int hdnaircatogory = 0;
        //    int AIRLINECATEGORY = 1;
        //    int hdnpassportexpdate = 2;
        //    int Fare = 3;
        //    int Load_Details = 4;
        //    int Load_amountdetail = 5;
        //    int hdntotalserv = 6;

        //    string imgurl = ConfigurationManager.AppSettings["FlightUrl"].ToString();
        //    var GrossBuildfirst = "";
        //    var grossBuildSecond1 = "";
        //    var grossBuildSecond2 = "";
        //    var grossBuildSecond3 = "";
        //    string bustbuy = string.Empty;
        //    string bustbuyfareoldfare = string.Empty;
        //    string bestbuyoldmarkup = string.Empty;


        //    var totcom = "";
        //    var totinst = "";


        //    var adult_count = td_Adult;
        //    var child_count = td_child;
        //    var infant_count = td_infant;
        //    double tSCh = 0;
        //    double tmarkup = 0;
        //    //DataTable getflightdetails = new DataTable();
        //    StringBuilder strBuilderPNR = new StringBuilder();
        //    StringBuilder strBuilderamount = new StringBuilder();
        //    // StringBuilder GrossBuildfirst = new StringBuilder();
        //    var tablealigncount = 0;

        //    array_Response[hdnaircatogory] = getflightdetails.Rows[0]["AIRLINECATEGORY"].ToString();
        //    if (getflightdetails != null && getflightdetails.Rows.Count > 0)

        //        try
        //        {
        //            bustbuy = HttpContext.Current.Session["BestBuy" + ValKey].ToString();
        //            bustbuyfareoldfare = HttpContext.Current.Session["RequestFare" + ValKey].ToString();
        //            bestbuyoldmarkup = HttpContext.Current.Session["Requestmarkup" + ValKey].ToString();

        //            array_Response[AIRLINECATEGORY] = getflightdetails.Rows[0]["AIRLINECATEGORY"].ToString();


        //            if (getflightdetails.Rows[0]["GrossAmount"].ToString().Contains('|'))
        //            {
        //                string[] amount = getflightdetails.Rows[0]["GrossAmount"].ToString().Split('|');
        //                //string[] baseamount = getflightdetails.Rows[0]["BaseAmount"].ToString().Split('|');
        //                string[] taxamount = getflightdetails.Rows[0]["TotalTaxAmount"].ToString().Split('|');
        //                string[] paxtype = getflightdetails.Rows[0]["PaxType"].ToString().Split('|');
        //                string[] tot_txbreakup = getflightdetails.Rows[0]["TAXBREAKUP"].ToString().Split('|');
        //                string[] commSplit = getflightdetails.Rows[0]["Commission"].ToString().Split('|');
        //                string[] insentSplit = getflightdetails.Rows[0]["Incentive"].ToString().Split('|');
        //                string[] ServSplit = getflightdetails.Rows[0]["ServiceCharge"].ToString().Split('|');
        //                string[] Markup = getflightdetails.Rows[0]["Markup"].ToString().Split('|');

        //                string[] adult_taxSplitFirst = tot_txbreakup[0].Split('/');

        //                grossBuildSecond1 += "<div class='TotalGrossFareHead'> Adult Fare Details</div>";
        //                var GAmount = Base.ServiceUtility.CovertToDouble(amount[0]) * adult_count;
        //                var servamt = Base.ServiceUtility.CovertToDouble(ServSplit[0]) * adult_count;
        //                var Mrkamt = Base.ServiceUtility.CovertToDouble(Markup[0]) * adult_count;
        //                var grossAmount = GAmount + servamt + Mrkamt;
        //                var addcomm = Base.ServiceUtility.CovertToDouble(commSplit[0]) * adult_count;
        //                var addins = Base.ServiceUtility.CovertToDouble(insentSplit[0]) * adult_count;
        //                totcom = addcomm.ToString();
        //                totinst = addins.ToString();

        //                var Grsscmk = (Base.ServiceUtility.CovertToDouble(Markup[0]) + Base.ServiceUtility.CovertToDouble(amount[0]) + Base.ServiceUtility.CovertToDouble(ServSplit[0])).ToString();

        //                for (int i = 0; i < adult_taxSplitFirst.Length; i++)
        //                {
        //                    string[] tot_adultFARE = adult_taxSplitFirst[i].Split(':');
        //                    grossBuildSecond1 += "<div style='width:98%'><div class='totalGrossFareSpan0'>" + tot_adultFARE[0].ToString() + "</div><div class='totalGrossFareSpan1'>" + tot_adultFARE[1] + " x " + adult_count + "</div><div class='totalGrossFareSpan2'>" + Base.ServiceUtility.CovertToDouble(tot_adultFARE[1]) * adult_count + "</div></div>";
        //                }
        //                tSCh = Base.ServiceUtility.CovertToDouble(ServSplit[0]) * adult_count;
        //                tmarkup = Base.ServiceUtility.CovertToDouble(Markup[0]) * adult_count;
        //                if (Markup[0].ToString() != "0")
        //                {
        //                    grossBuildSecond1 += "<div style='width:98%'><div class='totalGrossFareSpan0'>SC</div><div class='totalGrossFareSpan1'>" + Markup[0] + " x " + adult_count + "</div><div class='totalGrossFareSpan2'>" + (Base.ServiceUtility.CovertToDouble(Markup[0]) * adult_count).ToString() + "</div></div>";
        //                }
        //                if (ServSplit[0].ToString() != "0")
        //                {
        //                    grossBuildSecond1 += "<div style='width:98%'><div class='totalGrossFareSpan0'>Service&nbsp;Charge</div><div class='totalGrossFareSpan1'>" + ServSplit[0] + " x " + adult_count + "</div><div class='totalGrossFareSpan2'>" + (Base.ServiceUtility.CovertToDouble(ServSplit[0]) * adult_count).ToString() + "</div></div>";
        //                    sercharge += Base.ServiceUtility.CovertToDouble(ServSplit[0]) * adult_count;
        //                }
        //                //grossBuildSecond1 += "<div style='width:98%'><div class='totalGrossFareSpan0'>Gross&nbsp;Amount</div><div class='totalGrossFareSpan1'>" + Grsscmk + " x " + adult_count + "</div><div class='totalGrossFareSpan2'>" + (Base.ServiceUtility.CovertToDouble(grossAmount.ToString()) + (Base.ServiceUtility.CovertToDouble(Markup[0]) * adult_count) + (Base.ServiceUtility.CovertToDouble(ServSplit[0]) * adult_count)).ToString() + "</div></div>";
        //                grossBuildSecond1 += "<div style='width:98%'><div class='totalGrossFareSpan0'>Gross&nbsp;Amount</div><div class='totalGrossFareSpan1'>" + Grsscmk + " x " + adult_count + "</div><div class='totalGrossFareSpan2'>" + grossAmount.ToString() + "</div></div>";
        //                if (paxtype.Length == 2)
        //                {

        //                    tablealigncount = 2;

        //                    string getpaxtype = paxtype[1] == "INF" ? "Infant Fare Details" : "Child Fare Details";
        //                    var countOfpax = paxtype[1] == "INF" ? infant_count : child_count;
        //                    //string getpaxtype = paxtype[1] == "CHD" ? "Child Fare Details" : "Infant Fare Details";
        //                    //var countOfpax = paxtype[1] == "CHD" ? child_count : infant_count;
        //                    string[] child_taxSplitFirst = tot_txbreakup[1].Split('/');
        //                    var TSCh = Base.ServiceUtility.CovertToDouble(ServSplit[1]) * countOfpax;
        //                    var Tmarkup = Base.ServiceUtility.CovertToDouble(Markup[1]) * countOfpax;
        //                    var chdGrossamount = Base.ServiceUtility.CovertToDouble(amount[1]) * countOfpax;
        //                    var totgfsck = (Base.ServiceUtility.CovertToDouble(ServSplit[1]) + Base.ServiceUtility.CovertToDouble(Markup[1]) + Base.ServiceUtility.CovertToDouble(amount[1])).ToString();
        //                    tSCh += TSCh;
        //                    tmarkup += Tmarkup;
        //                    var addcomm2 = Base.ServiceUtility.CovertToDouble(commSplit[1]) * countOfpax;
        //                    var addins2 = Base.ServiceUtility.CovertToDouble(insentSplit[1]) * countOfpax;
        //                    totcom = (Convert.ToDecimal(totcom) + Convert.ToDecimal(addcomm2)).ToString();
        //                    totinst = (Convert.ToDecimal(totinst) + Convert.ToDecimal(addins2)).ToString();
        //                    grossBuildSecond2 += "<div class='TotalGrossFareHead'>" + getpaxtype + "</div>";
        //                    // grossBuildSecond2 += "<div class='BodyContext'>";
        //                    for (int i = 0; i < child_taxSplitFirst.Length; i++)
        //                    {
        //                        string[] tot_childtFARE = child_taxSplitFirst[i].Split(':');
        //                        grossBuildSecond2 += "<div style='width:98%'><div class='totalGrossFareSpan0'>" + tot_childtFARE[0].ToString() + "</div><div class='totalGrossFareSpan1'>" + tot_childtFARE[1] + " x " + countOfpax + "</div><div class='totalGrossFareSpan2'>" + Base.ServiceUtility.CovertToDouble(tot_childtFARE[1]) * countOfpax + "</div></div>";
        //                    }
        //                    if (Markup[1].ToString() != "0")
        //                    {
        //                        grossBuildSecond2 += "<div style='width:98%'><div class='totalGrossFareSpan0'>SC</div><div class='totalGrossFareSpan1'>" + Markup[1] + " x " + countOfpax + "</div><div class='totalGrossFareSpan2'>" + Tmarkup.ToString() + "</div></div>";
        //                    }
        //                    if (ServSplit[1].ToString() != "0")
        //                    {
        //                        grossBuildSecond2 += "<div style='width:98%'><div class='totalGrossFareSpan0'>Service&nbsp;Charge</div><div class='totalGrossFareSpan1'>" + ServSplit[1] + " x " + countOfpax + "</div><div class='totalGrossFareSpan2'>" + TSCh.ToString() + "</div></div>";
        //                        sercharge += TSCh;
        //                    }

        //                    grossBuildSecond2 += "<div style='width:98%'><div class='totalGrossFareSpan0'>Gross&nbsp;Amount</div><div class='totalGrossFareSpan1'>" + totgfsck.ToString() + " x " + countOfpax + "</div><div class='totalGrossFareSpan2'>" + (Base.ServiceUtility.CovertToDouble(chdGrossamount.ToString()) + (Base.ServiceUtility.CovertToDouble(Markup[1]) * countOfpax) + (Base.ServiceUtility.CovertToDouble(ServSplit[1]) * countOfpax)).ToString() + "</div></div>";


        //                }
        //                else if (paxtype.Length == 3)
        //                {
        //                    tablealigncount = 3;
        //                    if (paxtype[1] == "CHD" || paxtype[1] != "")//[1] != "")
        //                    {


        //                        var chdGrossamount = Base.ServiceUtility.CovertToDouble(amount[1]) * child_count;
        //                        var addcomm3 = Base.ServiceUtility.CovertToDouble(commSplit[1]) * child_count;
        //                        var addins3 = Base.ServiceUtility.CovertToDouble(insentSplit[1]) * child_count;
        //                        totcom = (Convert.ToDecimal(totcom) + Convert.ToDecimal(addcomm3)).ToString();
        //                        totinst = (Convert.ToDecimal(totinst) + Convert.ToDecimal(addins3)).ToString();
        //                        var CSCh = Base.ServiceUtility.CovertToDouble(ServSplit[1]) * child_count;
        //                        var CmCh = Base.ServiceUtility.CovertToDouble(Markup[1]) * child_count;
        //                        tSCh += CSCh;
        //                        tmarkup += CmCh;
        //                        chdGrossamount += CSCh;
        //                        chdGrossamount += CmCh;
        //                        var totamtscmc = (Base.ServiceUtility.CovertToDouble(amount[1]) + Base.ServiceUtility.CovertToDouble(ServSplit[1]) + Base.ServiceUtility.CovertToDouble(Markup[1])).ToString();
        //                        string[] child_taxSplitFirst = tot_txbreakup[1].Split('/');
        //                        grossBuildSecond2 += "<div class='TotalGrossFareHead'>Child Fare Details</div>";
        //                        //grossBuildSecond2 += "<div class='BodyContext'>";

        //                        for (int i = 0; i < child_taxSplitFirst.Length; i++)
        //                        {
        //                            string[] tot_childtFARE = child_taxSplitFirst[i].Split(':');
        //                            grossBuildSecond2 += "<div style='width:98%'><div class='totalGrossFareSpan0'>" + tot_childtFARE[0].ToString() + "</div><div class='totalGrossFareSpan1'>" + tot_childtFARE[1] + " x " + child_count + "</div><div class='totalGrossFareSpan2'>" + Convert.ToDecimal(tot_childtFARE[1]) * child_count + "</div></div>";
        //                        }
        //                        if (Markup[1].ToString() != "0")
        //                        {
        //                            grossBuildSecond2 += "<div style='width:98%'><div class='totalGrossFareSpan0'>SC</div><div class='totalGrossFareSpan1'>" + Markup[1] + " x " + child_count + "</div><div class='totalGrossFareSpan2'>" + CmCh.ToString() + "</div></div>";
        //                        }
        //                        if (ServSplit[1].ToString() != "0")
        //                        {
        //                            grossBuildSecond2 += "<div style='width:98%'><div class='totalGrossFareSpan0'>Service&nbsp;Charge</div><div class='totalGrossFareSpan1'>" + ServSplit[1] + " x " + child_count + "</div><div class='totalGrossFareSpan2'>" + CSCh.ToString() + "</div></div>";
        //                            sercharge += CSCh;
        //                        }
        //                        grossBuildSecond2 += "<div style='width:98%'><div class='totalGrossFareSpan0'>Gross&nbsp;Amount</div><div class='totalGrossFareSpan1'>" + totamtscmc.ToString() + " x " + child_count + "</div><div class='totalGrossFareSpan2'>" + chdGrossamount.ToString() + "</div></div>";
        //                    }
        //                    if (paxtype[2] == "INF" || paxtype[2] != "")// || paxtype[2] == "INF")//[2] != "")
        //                    {
        //                        string[] Infant_taxSplitFirst = tot_txbreakup[2].Split('/');
        //                        var infGrossamount = Base.ServiceUtility.CovertToDouble(amount[2]) * infant_count;
        //                        var ISCh = Base.ServiceUtility.CovertToDouble(ServSplit[2]) * infant_count;
        //                        var ImCh = Base.ServiceUtility.CovertToDouble(Markup[2]) * infant_count;
        //                        var addcomm4 = Base.ServiceUtility.CovertToDouble(commSplit[2]) * infant_count;
        //                        var addins4 = Base.ServiceUtility.CovertToDouble(insentSplit[2]) * infant_count;
        //                        var inftotscmc = (Base.ServiceUtility.CovertToDouble(amount[2]) + Base.ServiceUtility.CovertToDouble(ServSplit[2]) + Base.ServiceUtility.CovertToDouble(Markup[2])).ToString();

        //                        totcom = (Convert.ToDecimal(totcom) + Convert.ToDecimal(addcomm4)).ToString();
        //                        totinst = (Convert.ToDecimal(totinst) + Convert.ToDecimal(addins4)).ToString();
        //                        tSCh += ISCh;
        //                        tmarkup += ImCh;
        //                        infGrossamount += ISCh;
        //                        infGrossamount += ImCh;
        //                        grossBuildSecond3 += "<div  class='TotalGrossFareHead'>Infant Fare Details</div>";
        //                        for (int i = 0; i < Infant_taxSplitFirst.Length; i++)
        //                        {
        //                            string[] tot_InfantFARE = Infant_taxSplitFirst[i].Split(':');
        //                            grossBuildSecond3 += "<div style='width:98%'><div class='totalGrossFareSpan0'>" + tot_InfantFARE[0].ToString() + "</div><div class='totalGrossFareSpan1'>" + tot_InfantFARE[1] + " x " + infant_count + "</div><div class='totalGrossFareSpan2'>" + Convert.ToDecimal(tot_InfantFARE[1]) * infant_count + "</div></div>";
        //                        }
        //                        if (Markup[2].ToString() != "0")
        //                        {
        //                            grossBuildSecond3 += "<div style='width:98%'><div class='totalGrossFareSpan0'>SC</div><div class='totalGrossFareSpan1'>" + Markup[2] + " x " + infant_count + "</div><div class='totalGrossFareSpan2'>" + ImCh.ToString() + "</div></div>";
        //                        }
        //                        if (ServSplit[2].ToString() != "0")
        //                        {
        //                            grossBuildSecond3 += "<div style='width:98%'><div class='totalGrossFareSpan0'>Service&nbsp;Charge</div><div class='totalGrossFareSpan1'>" + ServSplit[2] + " x " + infant_count + "</div><div class='totalGrossFareSpan2'>" + ISCh.ToString() + "</div></div>";
        //                            sercharge += ISCh;
        //                        }
        //                        grossBuildSecond3 += "<div style='width:98%'><div class='totalGrossFareSpan0'>Gross&nbsp;Amount</div><div class='totalGrossFareSpan1'>" + inftotscmc.ToString() + " x " + infant_count + "</div><div class='totalGrossFareSpan2'>" + infGrossamount.ToString() + "</div></div>";
        //                    }

        //                }


        //            }
        //            else
        //            {
        //                tablealigncount = 0;
        //                var adtgross = Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["GrossAmount"].ToString()) * adult_count;
        //                var totcommision = Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["Commission"].ToString()) * adult_count;
        //                var totincentive = Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["Incentive"].ToString()) * adult_count;
        //                tSCh = Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["ServiceCharge"].ToString()) * adult_count;
        //                tmarkup = Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["MarkUp"].ToString()) * adult_count;
        //                var totscmk = (Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["MarkUp"].ToString()) + Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["ServiceCharge"].ToString()) + Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["GrossAmount"].ToString())).ToString();

        //                adtgross = adtgross + tSCh + tmarkup;
        //                totcom = totcommision.ToString();
        //                totinst = totincentive.ToString();
        //                string[] tot_txbreakup = getflightdetails.Rows[0]["TAXBREAKUP"].ToString().Split('|');
        //                string[] adult_taxSplitFirst = tot_txbreakup[0].Split('/');
        //                string ServSplit = getflightdetails.Rows[0]["ServiceCharge"].ToString();
        //                GrossBuildfirst += "<div class='TotalGrossFareHead'>Adult Fare Details</div>";
        //                for (int i = 0; i < adult_taxSplitFirst.Length; i++)
        //                {
        //                    string[] tot_adultFARE = adult_taxSplitFirst[i].Split(':');
        //                    GrossBuildfirst += "<div style='width:98%'><div class='totalGrossFareSpan0'>" + tot_adultFARE[0].ToString() + "</div><div class='totalGrossFareSpan1'>" + tot_adultFARE[1] + " x " + adult_count + "</div><div class='totalGrossFareSpan2'>" + Base.ServiceUtility.CovertToDouble(tot_adultFARE[1]) * adult_count + "</div></div>";
        //                }
        //                if (getflightdetails.Rows[0]["MarkUp"].ToString() != "0")
        //                {
        //                    GrossBuildfirst += "<div style='width:98%'><div class='totalGrossFareSpan0'>SC</div><div class='totalGrossFareSpan1'>" + getflightdetails.Rows[0]["MarkUp"].ToString() + " x " + adult_count + "</div><div class='totalGrossFareSpan2'>" + Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["MarkUp"].ToString()) * adult_count + "</div></div>";
        //                }
        //                if (getflightdetails.Rows[0]["ServiceCharge"].ToString() != "0")
        //                {
        //                    GrossBuildfirst += "<div style='width:98%'><div class='totalGrossFareSpan0'>Service&nbsp;Charge</div><div class='totalGrossFareSpan1'>" + getflightdetails.Rows[0]["ServiceCharge"].ToString() + " x " + adult_count + "</div><div class='totalGrossFareSpan2'>" + Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["ServiceCharge"].ToString()) * adult_count + "</div></div>";
        //                    sercharge += Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["ServiceCharge"].ToString()) * adult_count;
        //                }
        //                GrossBuildfirst += "<div style='width:98%'><div class='totalGrossFareSpan0'>Gross&nbsp;Amount</div><div class='totalGrossFareSpan1'>" + totscmk.ToString() + " x " + adult_count + "</div><div class='totalGrossFareSpan2'>" + adtgross.ToString() + "</div></div>";
        //            }
        //            //STax_build += "</table>";

        //            strBuilderPNR.Append("<table class='CSSTableGenerator' width='100%'>");

        //            strBuilderPNR.Append("<thead>");
        //            strBuilderPNR.Append("<tr>");
        //            strBuilderPNR.Append("<th>Airlines</th>");
        //            strBuilderPNR.Append("<th data-hide=\"phone,tablet\">Sector</th>");
        //            strBuilderPNR.Append("<th data-hide=\"phone,tablet\">Departure</th>");
        //            strBuilderPNR.Append("<th data-hide=\"phone,tablet\">Arrival</th>");
        //            strBuilderPNR.Append("<th data-hide=\"phone,tablet\">Class</th>");
        //            strBuilderPNR.Append("<th>Fare</th>");
        //            strBuilderPNR.Append("</tr>");
        //            strBuilderPNR.Append("</thead>");


        //            double GR = 0, Cham = 0, inft = 0, tot = 0, oldgr = 0, oldtot = 0, oldCham = 0, oldinft = 0;


        //            for (int i = 0; i < getflightdetails.Rows.Count; i++)
        //            {

        //                string[] Ipax = Regex.Split(getflightdetails.Rows[i]["stocktype"].ToString(), "SPLIT");
        //                array_Response[hdnpassportexpdate] = DateTime.ParseExact(getflightdetails.Rows[getflightdetails.Rows.Count - 1]["ArrivalDate"].ToString(), @"dd MMM yyyy HH:mm", null).ToString("dd/MM/yyyy");

        //                var strFlightBuild = "";
        //                strFlightBuild += "<table width='100%'>";
        //                strFlightBuild += "<tr><td width='5%'></td><td>Airline&nbsp;Name</td><td>:</td><td>" + getflightdetails.Rows[i]["AIRLINENAME"].ToString() + "</td></tr>";
        //                strFlightBuild += "<tr><td width='5%'></td><td>Departure&nbsp;Airport</td> <td>:</td><td>" + Base.Utilities.AirportcityName(getflightdetails.Rows[i]["Origin"].ToString()) + "</td></tr>";
        //                strFlightBuild += "<tr><td width='5%'></td><td>Arrival&nbsp;Airport</td><td>:</td><td>" + Base.Utilities.AirportcityName(getflightdetails.Rows[i]["DESTINATION"].ToString()) + "</td></tr>";

        //                strFlightBuild += "<tr><td width='5%'></td><td>Arrival&nbsp;On</td><td>:</td><td>" + (getflightdetails.Rows[i]["ArrivalDate"].ToString() != "" ? getflightdetails.Rows[i]["ArrivalDate"].ToString() : "N/A") + "</td></tr>";
        //                strFlightBuild += "<tr><td width='5%'></td><td>Operated&nbsp;by</td><td>:</td><td>" + (getflightdetails.Rows[i]["PlatingCarrier"].ToString() != "" ? getflightdetails.Rows[i]["PlatingCarrier"].ToString() : "N/A") + "</td></tr>";
        //                strFlightBuild += "<tr><td width='5%'></td><td>Arr.Terminal</td><td>:</td><td>" + (getflightdetails.Rows[i]["DESTERMINAL"].ToString() != "" ? getflightdetails.Rows[i]["DESTERMINAL"].ToString() : "N/A") + "</td></tr>";
        //                strFlightBuild += "<tr><td width='5%'></td><td>Dept.Terminal</td><td>:</td><td>" + (getflightdetails.Rows[i]["ORGTERMINAL"].ToString() != "" ? getflightdetails.Rows[i]["ORGTERMINAL"].ToString() : "N/A") + "</td></tr>";
        //                //connflightpop += "<tr><td width='5%'></td><td>Baggage</td><td>:</td><td><label id='baggmul'>" + SegmentDetails[5].replace("\r\n", "") + "</label> </td></tr>";
        //                // strFlightBuild += "<tr><td width='5%'></td><td>Baggage</td><td>:</td><td><label>" + (getflightdetails.Rows[i]["Baggage"].ToString() != "" ? getflightdetails.Rows[i]["Baggage"].ToString() : "N/A") + "</label></td></tr>";
        //                strFlightBuild += "<tr><td width='5%'></td><td>Journey&nbsp;Time</td><td>:</td><td>" + (getflightdetails.Rows[i]["JourneyTime"].ToString() != "" ? getflightdetails.Rows[i]["JourneyTime"].ToString() : "N/A") + "</td></tr>";
        //                strFlightBuild += "<tr><td width='5%'></td><td>Flying&nbsp;Time</td><td>:</td><td>" + (getflightdetails.Rows[i]["FlyingTime"].ToString() != "" ? getflightdetails.Rows[i]["FlyingTime"].ToString() : "N/A") + "</td></tr>";
        //                strFlightBuild += "<tr><td width='5%'></td><td>Stock Type</td><td>:</td><td>" + Ipax[0] + " </td></tr>";
        //                strFlightBuild += "</table>";

        //                strBuilderPNR.Append("<tr>");
        //                strBuilderPNR.Append("<td><span cla='FlightSpan' > <img rel=\"" + strFlightBuild + "\" class='FlightTip'   src=\"" + imgurl + getflightdetails.Rows[i]["FLIGHT"].ToString().Substring(0, 2) + ".png" + "\"> <br>  " + getflightdetails.Rows[i]["FLIGHT"] + " </span></td>");
        //                strBuilderPNR.Append("<td>" + getflightdetails.Rows[i]["STOPS"] + "</td>");
        //                strBuilderPNR.Append("<td>" + getflightdetails.Rows[i]["DEPARTURE"] + "</td>");
        //                strBuilderPNR.Append("<td>" + getflightdetails.Rows[i]["ARRIVAL"] + "</td>");
        //                strBuilderPNR.Append("<td>" + getflightdetails.Rows[i]["Class"] + "-" + getflightdetails.Rows[i]["FAREBASISCODE"] + "</td>");

        //                if (getflightdetails.Rows[i]["GrossAmount"].ToString().Contains('|'))
        //                {

        //                    string[] Amount = getflightdetails.Rows[i]["GrossAmount"].ToString().Split('|');
        //                    string[] Paxtype = getflightdetails.Rows[0]["PaxType"].ToString().Split('|');
        //                    string[] markup = getflightdetails.Rows[0]["markUp"].ToString().Split('|');
        //                    string[] oldorgamount = getflightdetails.Rows[i]["originaloldgross"].ToString().Split('|');
        //                    string[] oldorgmarkup = getflightdetails.Rows[0]["originaloldmarkup"].ToString().Split('|');


        //                    if (Amount.Length == 1)
        //                    {
        //                        GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
        //                        oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
        //                        tot = GR;
        //                        oldtot = oldgr;
        //                    }

        //                    if (Amount.Length == 2)
        //                    {
        //                        if (Paxtype[1] == "ADT")
        //                        {
        //                            if (td_infant > 0)
        //                                td_child = td_infant;

        //                            GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
        //                            oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
        //                            Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
        //                            oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
        //                            tot = GR + Cham;

        //                            oldtot = oldgr + oldCham;
        //                        }
        //                        if (Paxtype[1] == "CHD")
        //                        {
        //                            GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
        //                            oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
        //                            Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
        //                            oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
        //                            tot = GR + Cham;
        //                            oldtot = oldgr + oldCham;
        //                        }
        //                        if (Paxtype[1] == "INF")
        //                        {
        //                            GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
        //                            oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
        //                            inft = (td_infant * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_infant * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
        //                            oldinft = (td_infant * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_infant * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
        //                            tot = GR + inft;
        //                            oldtot = oldgr + oldinft;
        //                        }
        //                    }
        //                    if (Amount.Length == 3)
        //                    {

        //                        GR = (td_Adult * Base.ServiceUtility.CovertToDouble(Amount[0])) + (td_Adult * Base.ServiceUtility.CovertToDouble(markup[0].ToString()));
        //                        oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgamount[0].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(oldorgmarkup[0].ToString()));
        //                        Cham = (td_child * Base.ServiceUtility.CovertToDouble(Amount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(markup[1].ToString()));
        //                        oldCham = (td_child * Base.ServiceUtility.CovertToDouble(oldorgamount[1])) + (td_child * Base.ServiceUtility.CovertToDouble(oldorgmarkup[1].ToString()));
        //                        inft = (td_infant * Base.ServiceUtility.CovertToDouble(Amount[2])) + (td_infant * Base.ServiceUtility.CovertToDouble(markup[2].ToString()));
        //                        oldinft = (td_infant * Base.ServiceUtility.CovertToDouble(oldorgamount[2])) + (td_infant * Base.ServiceUtility.CovertToDouble(oldorgmarkup[2].ToString()));

        //                        tot = GR + Cham + inft;
        //                        oldtot = oldgr + oldCham + oldinft;


        //                    }

        //                    if (i == 0)
        //                    {
        //                        if (totinst != "0" && totinst != "" && totinst != null)
        //                        {
        //                            var total = Convert.ToInt16(tot) - Convert.ToInt16(totinst);
        //                            strBuilderPNR.Append("<td rowspan='" + getflightdetails.Rows.Count + "' >" + "<p class='Bestbuyfare' >&nbsp;" + tot + "</p>" + "<p class=' TotalFareTooltip' ><a href='#' id='btngrosfare'>" + total + "</a></p></td>");
        //                        }
        //                        else
        //                        {
        //                            strBuilderPNR.Append("<td rowspan='" + getflightdetails.Rows.Count + "' >" + (bustbuy.ToUpper() == "TRUE" ? "<p class='Bestbuyfare' >&nbsp;" + oldtot + "</p>" : "") + "<p class=' TotalFareTooltip' ><a href='#' id='btngrosfare'>" + tot + "</a></p></td>");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    GR = (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[i]["GrossAmount"].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["Markup"].ToString()));
        //                    oldgr = (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[i]["originaloldgross"].ToString())) + (td_Adult * Base.ServiceUtility.CovertToDouble(getflightdetails.Rows[0]["originaloldmarkup"].ToString()));
        //                    tot = GR;
        //                    oldtot = oldgr;

        //                    if (i == 0)
        //                    {
        //                        if (totinst != "0" && totinst != "" && totinst != null)
        //                        {
        //                            var total = Convert.ToInt16(tot) - Convert.ToInt16(totinst);
        //                            strBuilderPNR.Append("<td rowspan='" + getflightdetails.Rows.Count + "' >" + "<p class='Bestbuyfare' >&nbsp;" + tot + "</p>" + "<p class=' TotalFareTooltip' ><a href='#' id='btngrosfare'>" + total + "</a></p></td>");
        //                        }
        //                        else
        //                        {
        //                            strBuilderPNR.Append("<td rowspan='" + getflightdetails.Rows.Count + "'>" + (bustbuy.ToUpper() == "TRUE" ? "<p class='Bestbuyfare' >" + oldtot + "</p>&nbsp;" : "") + "<p class='TotalFareTooltip'><a href='#' id='btngrosfare'  >" + tot + " </a></p></td>");
        //                        }

        //                    }


        //                }


        //                strBuilderPNR.Append("</tr>");

        //            }

        //            var OldT = tot;
        //            tot = tot + tSCh;
        //            strBuilderPNR.Replace(OldT.ToString(), tot.ToString());

        //            if (bustbuy.ToUpper() == "TRUE")
        //            {
        //                var newOldT = oldtot;
        //                oldtot = oldtot + tSCh;
        //                strBuilderPNR.Replace(newOldT.ToString(), oldtot.ToString());
        //            }
        //            HttpContext.Current.Session.Add("totamt" + ValKey, tot);
        //            array_Response[Fare] = tot.ToString(); // ConfigurationManager.AppSettings["currency"].ToString() + " " + 

        //            strBuilderPNR.Append("</table>");
        //            if (tablealigncount == 0)
        //            {
        //                strBuilderPNR.Append("<div id='TotalGrossFareSpan2' style='display:none;'>");
        //                strBuilderPNR.Append("<div id='TotalGrossFareSpan'><div   class='TotalFareTooltipdiv' style='width: 40%;'>" + GrossBuildfirst + "</div></div>");
        //                strBuilderPNR.Append("<div class='totalGrossFarediv2'>" + (totinst != "0" ? "<span class='spancommssion'>Discount:" + totinst.ToString() + "</span>" : "") + (totcom != "0" ? "<span class='spancommssion'>Earnings:" + totcom + "</span>" : "") + "<span>Total Gross Fare:" + tot + "</span></div></div>");
        //            }
        //            else if (tablealigncount == 2)
        //            {
        //                strBuilderPNR.Append("<div id='TotalGrossFareSpan2' style='display:none;'>");
        //                strBuilderPNR.Append("<div id='TotalGrossFareSpan'><div   class='TotalFareTooltipdiv' style='margin-left: 50px; width: 45%;'>" + grossBuildSecond2 + "</div><div   class='TotalFareTooltipdiv' style='width: 45%;'>" + grossBuildSecond1 + "</div></div>");
        //                strBuilderPNR.Append("<div class='totalGrossFarediv2'>" + (totinst != "0" ? "<span class='spancommssion'>Discount:" + totinst.ToString() + "</span>" : "") + (totcom != "0" ? "<span class='spancommssion'>Earnings:" + totcom + "</span>" : "") + "<span>Total Gross Fare:" + tot + "</span></div></div>");

        //            }
        //            else
        //            {
        //                strBuilderPNR.Append("<div id='TotalGrossFareSpan2' style='display:none;'>");
        //                strBuilderPNR.Append("<div id='TotalGrossFareSpan'><div  class='TotalFareTooltipdiv' style='margin-left: 15px;width: 31%;'>" + grossBuildSecond3 + "</div><div   class='TotalFareTooltipdiv' style='margin-left: 15px;width: 32%;'>" + grossBuildSecond2 + "</div><div class='TotalFareTooltipdiv' style='width: 32%;'>" + grossBuildSecond1 + "</div></div>");
        //                strBuilderPNR.Append("<div class='totalGrossFarediv2'>" + (totinst != "0" ? "<span class='spancommssion'>Discount:" + totinst.ToString() + "</span>" : "") + (totcom != "0" ? "<span class='spancommssion'>Earnings:" + totcom + "</span>" : "") + "<span>Total Gross Fare:" + tot + "</span></div></div>");
        //            }



        //            HttpContext.Current.Session.Add("PaxCount" + ValKey, td_Adult + "|" + td_child + "|" + td_infant);

        //            array_Response[Load_Details] = strBuilderPNR.ToString();
        //            double meal_amount = 0;
        //            double Baggage_amount = 0;
        //            double seatamount = 0;
        //            double total_amopunt = Base.ServiceUtility.CovertToDouble(HttpContext.Current.Session["totamt" + ValKey].ToString());

        //            if (Mealamount != "")
        //            {

        //                meal_amount = Base.ServiceUtility.CovertToDouble(Mealamount);
        //                Baggage_amount = Base.ServiceUtility.CovertToDouble(baggageamount);
        //                seatamount = Base.ServiceUtility.CovertToDouble(Seatamount);
        //                total_amopunt = Base.ServiceUtility.CovertToDouble(HttpContext.Current.Session["totamt" + ValKey].ToString()) + meal_amount + Baggage_amount + seatamount;
        //            }



        //            strBuilderamount.Append("<table width='30%' style='float:right' class='faretable'  cellspacing='0'  >");
        //            strBuilderamount.Append("<tr><th colspan='3' style='text-align:center'><b>Fare Details</b></th></tr>");
        //            strBuilderamount.Append("<tr><td style='color:#0083db'><b>Ticket Amount</b></td><td>  :</td><td style='text-align:right'><label id='Grossamount'>" + HttpContext.Current.Session["totamt" + ValKey].ToString() + "</label></td><tr>");
        //            if (ViewStateSSR != null && ViewStateSSR.ToString().ToUpper() == "FALSE")
        //            {
        //                strBuilderamount.Append("<tr id='TextMealamount' style='display:none' ><td style='color:#0083db'><b>Meal Amount </b> </td><td>  :</td><td style='text-align:right'><label id='mealamount'>" + meal_amount.ToString() + "</label></td><tr>");
        //                strBuilderamount.Append("<tr id='Baggageamount' style='display:none' ><td style='color:#0083db'><b>Baggage Amount</b></td><td> :</td><td style='text-align:right'><label id='BaggageAmnt'>" + Baggage_amount.ToString() + "</label></td><tr>");
        //            }
        //            else
        //            {
        //                strBuilderamount.Append("<tr id='TextMealamount' ><td style='color:#0083db'><b>Meal Amount </b> </td><td>  :</td><td style='text-align:right'><label id='mealamount'>" + meal_amount.ToString() + "</label></td><tr>");
        //                strBuilderamount.Append("<tr id='Baggageamount' ><td style='color:#0083db'><b>Baggage Amount</b></td><td> :</td><td style='text-align:right'><label id='BaggageAmnt'>" + Baggage_amount.ToString() + "</label></td><tr>");
        //            }
        //            if (getflightdetails != null && getflightdetails.Rows[0]["seatavail"].ToString().Trim() == "TRUE")
        //            {
        //                strBuilderamount.Append("<tr id='TextSeatamount' ><td style='color:#0083db'><b>Seat Selection Amount </b> </td><td>  :</td><td style='text-align:right'><label id='seatamount'>" + seatamount.ToString() + "</label></td><tr>");
        //            }
        //            else
        //            {
        //                strBuilderamount.Append("<tr id='TextSeatamount' style='display:none' ><td style='color:#0083db'><b>Seat Selection Amount </b> </td><td>  :</td><td style='text-align:right'><label id='seatamount'>" + seatamount.ToString() + "</label></td><tr>");
        //            }
        //            strBuilderamount.Append("<tr><td style='color:#6c1e14'><b>Total Amount</b> </td><td> :</td><td style='text-align:right;font-weight: bold; '><label id='totalAmnt'>" + total_amopunt.ToString() + "</label></td><tr>");
        //            // strBuilderamount.Append("")
        //            strBuilderamount.Append("</table>");
        //            array_Response[Load_amountdetail] = strBuilderamount.ToString();
        //            array_Response[hdntotalserv] = sercharge.ToString();


        //        }
        //        catch (Exception ex)
        //        {
        //            DatabaseLog.LogData(HttpContext.Current.Session["username"].ToString(), "X", "Booking Load", "LoadFlightdetails", ex.ToString(), HttpContext.Current.Session["POS_ID"].ToString(), HttpContext.Current.Session["POS_TID"].ToString(), HttpContext.Current.Session["sequenceid"].ToString());
        //        }

        //    return array_Response;

        //}

        public ArrayList Create_Meals_Bagg(DataTable Ssr_meals, string ValKey, DataTable OtherssrDt, DataTable dtBagout, string strAdtcnt, string strChdcnt)
        {
            ArrayList RetArray = new ArrayList();

            strAdtcnt = strAdtcnt != null && strAdtcnt != "" ? strAdtcnt.Trim() : "0";
            strChdcnt = strChdcnt != null && strChdcnt != "" ? strChdcnt.Trim() : "0";

            RetArray.Add(""); // 1
            RetArray.Add(""); // 2
            RetArray.Add(""); // 3
            RetArray.Add(""); // 4
            RetArray.Add(""); // 5
            RetArray.Add(""); // 6
            RetArray.Add(""); // 7
            RetArray.Add(""); // 8
            RetArray.Add(""); // 9
            RetArray.Add(""); // 10
            RetArray.Add(""); // 11

            RetArray.Add(""); // 12
            RetArray.Add(""); // 13
            RetArray.Add(""); // 14

            RetArray.Add(""); // 15
            RetArray.Add(""); // 16
            RetArray.Add(""); // 17

            RetArray.Add(""); // 18

            RetArray.Add(""); // 19
            RetArray.Add(""); // 20
            RetArray.Add(""); // 21
            RetArray.Add(""); // 22
            RetArray.Add(""); // 23
            RetArray.Add(""); // 24
            RetArray.Add(""); // 25
            RetArray.Add(""); // 26

            int Sector = 0;
            int Meals_headding = 1;
            int baggage = 2;
            int Baggage_headding = 3;
            int ViewStateSSR = 4;
            int frqno = 5;
            int Fre_flyr_no = 6;
            int FRQ = 7;
            int seatmap = 8;
            int seatmap_headding = 9;
            int seatselect = 10;

            int insurancemap = 11;
            int insurance_headding = 12;
            int insuranceselect = 13;

            int Otherssrheading = 14;
            int Otherssrselect = 15;

            int Otherssrspmaxheading = 16;
            int Otherssrspmaxselect = 17;
            int Otherssrprcheckinheading = 18;
            int Otherssrprcheckinselect = 19;
            int Otherssrbagoutheading = 20;
            int Otherssrbagoutselect = 21;

            int Meals = 22;
            int Baggage = 23;
            int FullFQT = 24;
            int OtherSSR = 25;

            string Strservicemeal = string.Empty;
            bool flag = false;

            StringBuilder sBuild = new StringBuilder();
            StringBuilder SpicemaxBuild = new StringBuilder();
            StringBuilder strOtherSSR = new StringBuilder();
            try
            {
                DataSet mealscount = new DataSet();
                StringBuilder mealsdetails = new StringBuilder();

                DataSet ds = FormSSR(true, Ssr_meals, ValKey);
                if (ds != null && ds.Tables.Count > 0 &&
                   ds.Tables.Contains("Meals") && ds.Tables["Meals"].Rows.Count > 0)
                {

                    sBuild = new StringBuilder();
                    var qry = (from p in ds.Tables["Meals"].AsEnumerable()
                               select p["MyRef"].ToString()).ToArray().Distinct();//MyRef

                    if (qry.Count() > 0)
                    {
                        sBuild.Append("<table id='meals' width='100%'>");
                        int index = 0;
                        foreach (var lstrref in qry)
                        {
                            DataTable dt = Linq(ds.Tables["Meals"], lstrref.ToString().Trim());
                            if (dt.Rows.Count != 0)
                            {
                                string strclass = dt.Rows[0]["Segment"].ToString().Replace("->","CLS");
                                sBuild.Append("<tr><td style='width: 45%;' id='mealpre" + index + "'>" + dt.Rows[0]["Segment"].ToString() + "</td><td style='width:75%;'><select style='width: 100%;' class='ddlclass "+strclass+"'  id='Meals" + index + "'  >" + buildmealdropdown(dt, flag) + "</select></td></tr>");//data-primselectmeal=" + index + "
                                index++;
                            }
                        }
                        sBuild.Append("</table>");
                        RetArray[Sector] = sBuild.ToString();
                    }

                    //Full Meals
                    sBuild = new StringBuilder();
                    if (qry.Count() > 0)
                    {

                        for (int adtcnt = 1; adtcnt <= Convert.ToDecimal(strAdtcnt); adtcnt++)
                        {
                            sBuild.Append("<div class='crdshdw col-sm-12 col10 m-b-15 pad-btm-10'><label style='margin-bottom: 10px;min-width:100px;' > Adult " + adtcnt + "</label>");
                            sBuild.Append("<table id='tblMeals' width='100%'>");
                            int index = 0;
                            foreach (var lstrref in qry)
                            {
                                DataTable dt = Linq(ds.Tables["Meals"], lstrref.ToString().Trim());
                                if (dt.Rows.Count != 0)
                                {
                                    string strclass = dt.Rows[0]["Segment"].ToString().Replace("->", "CLS") + "CLS" + adtcnt;
                                    sBuild.Append("<tr class='clsAdtMeals'>");
                                    sBuild.Append("<td style='margin-bottom: 10px;min-width:100px;width:45%;' id='MealSegAdt_" + adtcnt + "_" + index + "'>" + dt.Rows[0]["Segment"].ToString() + "</td>");
                                    sBuild.Append("<td style='width:75%;'><select data-primemeal='" + strclass + "' class='ddlclass ' id='MealNameAdt_" + adtcnt + "_" + index + "'  >" + buildmealdropdown(dt, flag) + "</select></td></tr>");
                                    index++;
                                }
                            }
                            sBuild.Append("</table></div>");
                        }

                        for (int chdcnt = 1; chdcnt <= Convert.ToDecimal(strChdcnt); chdcnt++)
                        {
                            sBuild.Append("<div class='crdshdw col-sm-12 col10 m-b-15 pad-btm-10'><label style='margin-bottom: 10px;min-width:100px;' > Child " + chdcnt + "</label>");
                            sBuild.Append("<table id='tblMeals' width='100%'>");
                            int index = 0;
                            foreach (var lstrref in qry)
                            {
                                DataTable dt = Linq(ds.Tables["Meals"], lstrref.ToString().Trim());
                                if (dt.Rows.Count != 0)
                                {
                                    string strclass = dt.Rows[0]["Segment"].ToString().Replace("->", "CLS") + "CLS" + (Convert.ToDecimal(strAdtcnt) + chdcnt);
                                    sBuild.Append("<tr class='clsChdMeals'>");
                                    sBuild.Append("<td style='margin-bottom: 10px;min-width: 100px;width: 45%;' id='MealSegChd_" + chdcnt + "_" + index + "'>" + dt.Rows[0]["Segment"].ToString() + "</td>");
                                    sBuild.Append("<td style='width:75%;'><select data-primemeal='" + strclass + "' class='ddlclass ' id='MealNameChd_" + chdcnt + "_" + index + "'  >" + buildmealdropdown(dt, flag) + "</select></td></tr>");
                                    index++;
                                }
                            }
                            sBuild.Append("</table></div>");
                        }

                        RetArray[Meals] = sBuild.ToString();
                    }
                    //Full Meals
                    HttpContext.Current.Session.Add("_Meals" + ValKey, true);
                    RetArray[ViewStateSSR] = true;
                    RetArray[Meals_headding] = true;

                }
                else
                {
                    HttpContext.Current.Session.Add("_Meals" + ValKey, false);
                    RetArray[Meals_headding] = false;
                    RetArray[ViewStateSSR] = false;
                }

                if (ds != null && ds.Tables.Count > 0 &&
                  ds.Tables.Contains("Baggage") && ds.Tables["Baggage"].Rows.Count > 0)
                {
                    sBuild = new StringBuilder();
                    var qry = (from p in ds.Tables["Baggage"].AsEnumerable()
                               select p["MyRef"].ToString()).ToArray().Distinct();

                    if (qry.Count() > 0)
                    {
                        sBuild.Append("<table id='Baggagein'>");
                        int index = 0;
                        foreach (var lstrref in qry)
                        {
                            DataTable dt = Linq(ds.Tables["Baggage"], lstrref.ToString().Trim());

                            if (dt.Rows.Count != 0)
                            {

                                sBuild.Append("<tr><td style='width: 45%;'>" +
                                   dt.Rows[0]["Segment"].ToString() +
                                   "</td><td style='width:75%;'><select class='ddlclass' id='Baggage" + index + "' style='width:100%'  onchange='ShowIntBaggage(this.id)'  >" +
                                   buildBaggagedropdown(dt) + "</select></td></tr>");
                                index++;
                            }
                        }
                        sBuild.Append("</table>");
                        RetArray[baggage] = sBuild.ToString();
                    }

                    //Full Baggage
                    sBuild = new StringBuilder();
                    if (qry.Count() > 0)
                    {

                        for (int adtcnt = 1; adtcnt <= Convert.ToDecimal(strAdtcnt); adtcnt++)
                        {
                            sBuild.Append("<div class='crdshdw col-sm-12 col10 m-b-15 pad-btm-10'><label style = 'width: 45%;' > Adult " + adtcnt + " </label>");
                            sBuild.Append("<table id='tblBaggage'>");
                            int index = 0;
                            foreach (var lstrref in qry)
                            {
                                DataTable dt = Linq(ds.Tables["Baggage"], lstrref.ToString().Trim());
                                if (dt.Rows.Count != 0)
                                {
                                    sBuild.Append("<tr>");
                                    sBuild.Append("<td style='width: 45%;'>" + dt.Rows[0]["Segment"].ToString() + "</td>");
                                    sBuild.Append("<td style='width:75%;'><select class='ddlclass' onchange='ShowIntBaggage(this.id)'  id='BaggageNameAdt_" + adtcnt + "_" + index + "'>" + buildBaggagedropdown(dt) + "</select></td></tr>");
                                    index++;
                                }
                            }
                            sBuild.Append("</table></div>");
                        }
                        for (int chdcnt = 1; chdcnt <= Convert.ToDecimal(strChdcnt); chdcnt++)
                        {
                            sBuild.Append("<div class='crdshdw col-sm-12 col10 m-b-15 pad-btm-10'><label style = 'width: 45%;' > Child " + chdcnt + " </label>");
                            sBuild.Append("<table id='tblBaggage'>");
                            int index = 0;
                            foreach (var lstrref in qry)
                            {
                                DataTable dt = Linq(ds.Tables["Baggage"], lstrref.ToString().Trim());
                                if (dt.Rows.Count != 0)
                                {
                                    sBuild.Append("<td style='width: 45%;'>" + dt.Rows[0]["Segment"].ToString() + "</td>");
                                    sBuild.Append("<td style='width:75%;'><select class='ddlclass' onchange='ShowIntBaggage(this.id)'  id='BaggageNameChd_" + chdcnt + "_" + index + "'>" + buildBaggagedropdown(dt) + "</select></td></tr>");
                                    index++;
                                }
                            }
                            sBuild.Append("</table></div>");
                        }

                        RetArray[Baggage] = sBuild.ToString();
                    }
                    //Full Baggage

                    HttpContext.Current.Session.Add("_Baggage" + ValKey, true);
                    RetArray[ViewStateSSR] = true;
                    RetArray[Baggage_headding] = true;
                }
                else
                {
                    HttpContext.Current.Session.Add("_Baggage" + ValKey, false);
                    RetArray[Baggage_headding] = false;
                }


                if (Ssr_meals != null && Ssr_meals.Rows.Count > 0 && Ssr_meals.Columns.Contains("FQT") && Ssr_meals.Rows[0]["FQT"].ToString().ToUpper() == "TRUE")
                {
                    RetArray[frqno] = Ssr_meals.Rows[0]["FQT"].ToString().ToUpper();
                    sBuild = new StringBuilder();
                    var qrySelectDistictAirline = (from p in Ssr_meals.AsEnumerable()
                                                   select new
                                                   {
                                                       AirlineName = p["FLIGHTNUMBER"].ToString().Substring(0, 2).ToString().Trim(),
                                                       itinRef = p["itinRef"].ToString().Trim(),
                                                   }).Distinct();

                    DataTable dtTemp = Serv.ConvertToDataTable(qrySelectDistictAirline);// ();

                    sBuild.Append("<table id='frequentin'>");
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        sBuild.Append("<tr>");
                        sBuild.Append("<td><label id='adulttype' ></label></td>");
                        sBuild.Append("<td><label id='flight_code' >" + Base.Utilities.AirlineName(dtTemp.Rows[i]["AirlineName"].ToString()) + "</label></td>");
                        sBuild.Append("<td><input type='hidden' id='_segno" + i + "' value='" + dtTemp.Rows[i]["AirlineName"].ToString() + "WEbMeaLWEB" + "0" + "'  /><img id='Imagecode' src='" + ConfigurationManager.AppSettings["FlightUrl"] + dtTemp.Rows[i]["AirlineName"].ToString() + ".png" + "'style='padding: 5px;box-shadow: 0px 0px 4px 1px #ddd;' /></label></td>");
                        sBuild.Append("<td><input class='hgtfix' type='text' maxlength='15' id='Freq_flyer" + i + "'  /></td>");
                        sBuild.Append("</tr>");
                    }

                    sBuild.Append("</table>");
                    sBuild.Append("<table style='float:right;'><tr><td><label style='color:red;'>(Without airline code)</label></td></tr></table>");

                    RetArray[Fre_flyr_no] = sBuild.ToString();

                    //Full FQT
                    sBuild = new StringBuilder();

                    for (int adtcnt = 1; adtcnt <= Convert.ToDecimal(strAdtcnt); adtcnt++)
                    {
                        sBuild.Append("<div class='crdshdw col-sm-12 col10 m-b-15 pad-btm-10 col-xs-12'><label><label> Adult " + adtcnt + "</label></label>");
                        sBuild.Append("<table id='tblFFN' style='width: 100%;float: left;'>");
                        for (int i = 0; i < dtTemp.Rows.Count; i++)
                        {
                            sBuild.Append("<tr style='width: 100%;float: left;margin-bottom:10px;'>");
                            sBuild.Append("<td style='width: 30%;float: left;'><label id='flight_code' style='font-size: 15px;color: #333;'>" + Base.Utilities.AirlineName(dtTemp.Rows[i]["AirlineName"].ToString()) + "</label></td>");
                            sBuild.Append("<td style='float:left;'><input type='hidden' id='_segno_" + strAdtcnt + "_" + i + "' value='" + dtTemp.Rows[i]["AirlineName"].ToString() + "WEbMeaLWEB" + "0" + "'  /><img id='Imagecode' src='" + ConfigurationManager.AppSettings["FlightUrl"] + dtTemp.Rows[i]["AirlineName"].ToString() + ".png" + "'style='padding: 5px;box-shadow: 0px 0px 4px 1px #ddd;' /></label></td>");
                            sBuild.Append("<td style='width: 48%;float: right;margin-right:15px;'><input class='hgtfix' type='text' maxlength='15' id='Freq_flyer_" + strAdtcnt + "_" + i + "'  /><input type='hidden' id='ItRef_Freq_flyer_" + strAdtcnt + "_" + i + "' value="+ dtTemp.Rows[i]["itinRef"].ToString() + " /></td>");
                            sBuild.Append("</tr>");
                        }
                        sBuild.Append("</table></div>");
                    }
                    sBuild.Append("<table style='float:right;'><tr><td><label style='color:red;font-size:12px;'>(Without airline code)</label></td></tr></table>");

                    RetArray[FullFQT] = sBuild.ToString();
                    //Full FQT

                    HttpContext.Current.Session.Add("_FREQNO" + ValKey, true);
                    RetArray[FRQ] = true;
                }
                else
                {
                    RetArray[frqno] = (string.IsNullOrEmpty(Ssr_meals.Rows[0]["FQT"].ToString()) ? "FALSE" : Ssr_meals.Rows[0]["FQT"].ToString().ToUpper());
                    HttpContext.Current.Session.Add("_FREQNO" + ValKey, false);
                    RetArray[FRQ] = false;
                }
                if (Ssr_meals != null && Ssr_meals.Rows.Count > 0 && Ssr_meals.Columns.Contains("seatavail") == true && Ssr_meals.Rows[0]["seatavail"].ToString().ToUpper() == "TRUE")
                {
                    StringBuilder seatBuild = new StringBuilder();
                    seatBuild.Append("<table id='seatsel'>");
                    for (int i = 0; i < Ssr_meals.Rows.Count; i++)
                    {
                        seatBuild.Append("<tr style='line-height: 30px;'><td style='width:45%;'>" + Ssr_meals.Rows[i]["Origin"].ToString() + " ->" + Ssr_meals.Rows[i]["Destination"].ToString() + "</td><td style='text-align: left;' id='adultseat_" + i + "'>Not Selected</td><td><span style='visibility: hidden;'>test</span></td></tr>");

                    }
                    seatBuild.Append("</table>");
                    RetArray[seatmap] = seatBuild.ToString();

                    RetArray[seatselect] = true;
                    RetArray[seatmap_headding] = true;
                }
                else
                {
                    RetArray[seatselect] = false;
                    RetArray[seatmap_headding] = false;
                }
                string SS = "";
                if (ConfigurationManager.AppSettings["Book_ins"].ToString() == "Y")
                {
                    SS = "TRUE";
                }
                else
                {
                    SS = "FALSE";
                }
                // Ssr_meals.Rows[0]["airinsavail"].ToString().ToUpper() = "TRUE";
                //&& Ssr_meals.Rows[0]["airinsavail"].ToString().ToUpper() == "TRUE"
                if (Ssr_meals != null && Ssr_meals.Rows.Count > 0 && Ssr_meals.Columns.Contains("airinsavail") == true && SS == "TRUE")
                {
                    StringBuilder insBuild = new StringBuilder();
                    insBuild.Append("<table id='insurancebok'>");
                    //for (int i = 0; i < Ssr_meals.Rows.Count; i++)
                    //{
                    insBuild.Append("<tr style='line-height: 30px;'><td id='insplantitle' style='width:45%;'>Plan Title</td><td id='insplanamount' style='text-align: left;'>N/A</td><td style='text-align: center;'><input type='button' style='visibility: hidden;' value='Reset' class='btn btn-md btn-primary width100per' id='resetinsbtn' onclick='javascript: resetinsurancefun();' /></td></tr>");

                    // }
                    insBuild.Append("</table>");
                    RetArray[insurancemap] = insBuild.ToString();

                    RetArray[insuranceselect] = true;
                    RetArray[insurance_headding] = true;
                }
                #region Otherssr

                int TotalPaxCount = int.Parse(strAdtcnt) + int.Parse(strChdcnt);

                int paxcount = 0;
                string[] strPaxArray = new string[TotalPaxCount];
                for (int adtcnt = 1; adtcnt <= Convert.ToDecimal(strAdtcnt); adtcnt++)
                {
                    strPaxArray[paxcount] = "Adult " + adtcnt;
                    paxcount++;
                }
                for (int chdcnt = 1; chdcnt <= Convert.ToDecimal(strChdcnt); chdcnt++)
                {
                    strPaxArray[paxcount] = "Child " + chdcnt;
                    paxcount++;
                }

                if (Ssr_meals != null && Ssr_meals.Rows.Count > 0 && Ssr_meals.Columns.Contains("OtherSSR") && Ssr_meals.Rows[0]["OtherSSR"].ToString().ToUpper() == "TRUE")
                {
                    DataTable spmaxdt = new DataTable();
                    DataTable prioritydt = new DataTable();
                    DataTable bagoutdt = new DataTable();
                    DataTable spmaindt = new DataTable();
                    DataTable buildssr = new DataTable();
                    DataTable buildssrbugout = new DataTable();
                    if (OtherssrDt != null && OtherssrDt.Rows.Count > 0)
                    {
                        //var OtherSSRmain = (from _lsiti in OtherssrDt.AsEnumerable()
                        //                  select
                        //                     _lsiti["OtherSSR_Type"]).Distinct();

                        var OtherSSRmain = (from _lsiti in OtherssrDt.AsEnumerable()
                                            select new
                                            {
                                                OthrSSRDetails = _lsiti["OtherSSR_Type"],
                                            }).Distinct();

                        spmaindt = Serv.ConvertToDataTable(OtherSSRmain);
                        StringBuilder otherssrmain_spmax = new StringBuilder();
                        StringBuilder otherssr_bagout = new StringBuilder();

                        if (dtBagout != null && dtBagout.Rows.Count > 0)
                        {
                            var OtherssrBuggout = (from _lsiti in dtBagout.AsEnumerable()
                                                   select new
                                                   {
                                                       Segment = _lsiti["Segment"],
                                                       Segment_Code = _lsiti["Segment_Code"],
                                                       OthrSSR_Code = _lsiti["OthrSSR_Code"],
                                                       OthrSSR_Desc = _lsiti["OthrSSR_Desc"],
                                                       OthrSSR_Details = _lsiti["OthrSSR_Details"],
                                                       OtherSSR_orgin = _lsiti["OtherSSR_orgin"],
                                                       OtherSSR_destina = _lsiti["OtherSSR_destina"],
                                                       OthrSSR_Amt = _lsiti["OthrSSR_Amt"],
                                                       SegRef = _lsiti["SegRef"],
                                                       OtherSSR_Type = _lsiti["OtherSSR_Type"],
                                                       Itinref = _lsiti["Itinref"],
                                                       MyRef = _lsiti["MyRef"],
                                                   });
                            buildssrbugout = Serv.ConvertToDataTable(OtherssrBuggout);
                            if (buildssrbugout != null && buildssrbugout.Rows.Count > 0)
                            {
                                int check0 = 0;
                                for (var i = 0; i < buildssrbugout.Rows.Count; i++)
                                {
                                    check0++;
                                    otherssr_bagout.Append("<table id='other_ssrbagout' style='width: 100%'>");
                                    otherssr_bagout.Append("<tr style='line-height: 30px;'><td style='width: 45%;'>" + buildssrbugout.Rows[i]["OthrSSR_Details"].ToString() + "</td>");
                                    otherssr_bagout.Append("<td style=''><div class='radio'><input  id='othssrid_" + i + "' name='" + buildssrbugout.Rows[i]["OtherSSR_orgin"] + "_" + buildssrbugout.Rows[i]["OtherSSR_destina"] + "' class='bagoutfirclss' value='" + buildssrbugout.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + buildssrbugout.Rows[i]["Itinref"] + "WEbMeaLWEB" + buildssrbugout.Rows[i]["SegRef"] + "WEbMeaLWEB" + buildssrbugout.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + buildssrbugout.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + buildssrbugout.Rows[i]["OtherSSR_destina"] + "' type='radio'><label class='radio-label' for='othssrid_" + i + "' ></label></div> </td><td style='text-align: left;'><span>" + buildssrbugout.Rows[i]["OthrSSR_Amt"] + "&nbsp;" + HttpContext.Current.Session["App_currency"] + "&nbsp;(" + buildssrbugout.Rows[i]["Segment"] + ")</span></td></tr>");
                                    otherssr_bagout.Append("</table>");
                                }
                                RetArray[Otherssrbagoutselect] = otherssr_bagout.ToString();

                                //FULL BAGOUT
                                otherssr_bagout = new StringBuilder();


                                for (int paxcnt = 1; paxcnt <= Convert.ToDecimal(TotalPaxCount); paxcnt++)
                                {
                                    otherssr_bagout.Append("<div class='crdshdw col-sm-12 col10 m-b-15 pad-btm-10'><label style = 'width: 45%;' > " + strPaxArray[paxcnt - 1] + " </label>");
                                    otherssr_bagout.Append("<table id='tblbagout' style='width: 100%'>");
                                    for (var i = 0; i < buildssrbugout.Rows.Count; i++)
                                    {
                                        otherssr_bagout.Append("<tr style='line-height: 30px;'>");
                                        otherssr_bagout.Append("<td style='width: 65%;'>" + buildssrbugout.Rows[i]["OthrSSR_Details"].ToString() + "</td>");
                                        otherssr_bagout.Append("<td><div class='radio'><input id='othssrid_" + paxcnt + "_" + i + "' name='" + buildssrbugout.Rows[i]["OtherSSR_orgin"] + "_" + buildssrbugout.Rows[i]["OtherSSR_destina"] + paxcnt + "' class='bagoutfirclss_" + paxcnt + "' value='" + buildssrbugout.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + buildssrbugout.Rows[i]["Itinref"] + "WEbMeaLWEB" + buildssrbugout.Rows[i]["SegRef"] + "WEbMeaLWEB" + buildssrbugout.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + buildssrbugout.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + buildssrbugout.Rows[i]["OtherSSR_destina"] + "' type='radio'><label class='radio-label' for='othssrid_" + paxcnt + "_" + i + "' ></label></div> </td><td style='text-align: left;'><span>" + buildssrbugout.Rows[i]["OthrSSR_Amt"] + "&nbsp;" + HttpContext.Current.Session["App_currency"] + "&nbsp;(" + buildssrbugout.Rows[i]["Segment"] + ")</span></td></tr>");
                                    }
                                    otherssr_bagout.Append("</table></div>");
                                }
                             
                                RetArray[Otherssrbagoutheading] = true;
                                strOtherSSR.Append(otherssr_bagout.ToString());
                            }
                        }
                        int check1 = 0, otherssrcnt = 0;
                        if (spmaindt != null && spmaindt.Rows.Count > 0)
                        {
                            for (int paxcnt = 1; paxcnt <= Convert.ToDecimal(TotalPaxCount); paxcnt++)
                            {
                                strOtherSSR.Append("<div class='crdshdw col-sm-12 col10 m-b-15 pad-btm-10'><label style = 'width: 10%;' > " + strPaxArray[paxcnt - 1] + " </label>");
                                for (int m = 0; m < spmaindt.Rows.Count; m++)
                                {
                                    string str = spmaindt.Rows[m]["OthrSSRDetails"].ToString();
                                    if (str.ToUpper() != "SPICEMAX")
                                    {
                                        var Spicemaxx = (from _lsiti in OtherssrDt.AsEnumerable()
                                                         where (_lsiti["OtherSSR_Type"].ToString() == spmaindt.Rows[m]["OthrSSRDetails"].ToString() && _lsiti["OtherSSR_Type"].ToString().ToUpper() != "SPICEMAX")
                                                         select new
                                                         {
                                                             Segment = _lsiti["Segment"],
                                                             Segment_Code = _lsiti["Segment_Code"],
                                                             OthrSSR_Code = _lsiti["OthrSSR_Code"],
                                                             OthrSSR_Desc = _lsiti["OthrSSR_Desc"],
                                                             OthrSSR_Details = _lsiti["OthrSSR_Details"],
                                                             OtherSSR_orgin = _lsiti["OtherSSR_orgin"],
                                                             OtherSSR_destina = _lsiti["OtherSSR_destina"],
                                                             OthrSSR_Amt = _lsiti["OthrSSR_Amt"],
                                                             SegRef = _lsiti["SegRef"],
                                                             OtherSSR_Type = _lsiti["OtherSSR_Type"],
                                                             Itinref = _lsiti["Itinref"],
                                                             MyRef = _lsiti["MyRef"],
                                                         });
                                        buildssr = Serv.ConvertToDataTable(Spicemaxx);
                                    }
                                    else
                                    {
                                        var Spicemaxx = (from _lsiti in OtherssrDt.AsEnumerable()
                                                         where (_lsiti["OtherSSR_Type"].ToString() == spmaindt.Rows[m]["OthrSSRDetails"].ToString())
                                                         select new
                                                         {
                                                             Segment = _lsiti["Segment"],
                                                             Segment_Code = _lsiti["Segment_Code"],
                                                             OthrSSR_Code = _lsiti["OthrSSR_Code"],
                                                             OthrSSR_Desc = _lsiti["OthrSSR_Desc"],
                                                             OthrSSR_Details = _lsiti["OthrSSR_Details"],
                                                             OtherSSR_orgin = _lsiti["OtherSSR_orgin"],
                                                             OtherSSR_destina = _lsiti["OtherSSR_destina"],
                                                             OthrSSR_Amt = _lsiti["OthrSSR_Amt"],
                                                             SegRef = _lsiti["SegRef"],
                                                             OtherSSR_Type = _lsiti["OtherSSR_Type"],
                                                             Itinref = _lsiti["Itinref"],
                                                             MyRef = _lsiti["MyRef"],
                                                         });
                                        buildssr = Serv.ConvertToDataTable(Spicemaxx);
                                    }
                                    var qry = (from p in buildssr.AsEnumerable()
                                               select p["MyRef"].ToString()).ToArray().Distinct();
                                    int index = 0, spicemaxcnt = 0;
                                    if (str.ToUpper() == "SPICEMAX")
                                    {
                                        if (paxcnt == 1)
                                        {
                                            foreach (var lstrref in qry)
                                            {
                                                DataTable dt = Linq(buildssr, lstrref.ToString().Trim());
                                                SpicemaxBuild.Append("<table id='other_ssrPcheck' style='width:100%;'>");
                                                SpicemaxBuild.Append("<tr style='line-height: 30px;'><td style='width: 45%;'>" + dt.Rows[0]["OtherSSR_Type"].ToString() + "</td><td class='fl'>" + buildOtherSSR(dt, index) + "</td></tr>");
                                                SpicemaxBuild.Append("</table>");
                                                index++;
                                            }
                                            RetArray[Otherssrprcheckinselect] = SpicemaxBuild.ToString();//177
                                        }

                                        //FULL SPICEMAX
                                        StringBuilder strotherssr = new StringBuilder();
                                        strotherssr.Append("<table id='other_ssrPcheck' style='width:100%;'>");
                                        foreach (var lstrref in qry)
                                        {
                                            DataTable dt = Linq(buildssr, lstrref.ToString().Trim());
                                            //strotherssr.Append("<tr style='line-height: 30px;'><td style='width: 45%;'>" + strPaxArray[paxcnt - 1] + "</td>");
                                            strotherssr.Append("<tr style='line-height: 30px;'><td style='width: 65%;'>" + dt.Rows[0]["OtherSSR_Type"].ToString() + "</td><td class='fl'>" + buildSpiceMax(dt, paxcnt, spicemaxcnt) + "</td></tr>");
                                            spicemaxcnt++;
                                        }
                                        strotherssr.Append("</table>");
                                        strOtherSSR.Append(strotherssr);
                                        //FULL SPICEMAX
                                        RetArray[Otherssrprcheckinheading] = true;
                                    }

                                    if (str.ToUpper() != "SPICEMAX")
                                    {

                                        //for (var i = 0; i < buildssr.Rows.Count; i++)
                                        //{
                                        //    otherssrmain_spmax.Append("<table id='other_ssrspmax' style='width: 100%'>");
                                        //    otherssrmain_spmax.Append("<tr style='line-height: 30px;'><td style='width: 45%;'>" + buildssr.Rows[i]["OthrSSR_Details"].ToString() + "</td>");
                                        //    otherssrmain_spmax.Append("<td class='fl'><input type='checkbox' class='othSpicemaxcls' style='display:none;' id=othssridspicemax_" + (check1) + " value='" + buildssr.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + buildssr.Rows[i]["Itinref"] + "WEbMeaLWEB" + buildssr.Rows[i]["SegRef"] + "WEbMeaLWEB" + buildssr.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_destina"] + "'/><label style='border: 2px solid #00afe1;' class='cbx' for='othssridspicemax_" + (check1) + "'></label><label class='lbl' for='othssridspicemax_" + (check1) + "'></label></td><td style='text-align: left;float: left; margin-left: 5px; '><span>" + buildssr.Rows[i]["OthrSSR_Amt"] + "&nbsp;" + HttpContext.Current.Session["App_currency"] + "&nbsp;(" + buildssr.Rows[i]["Segment"] + ")</span></td></tr>");
                                        //    otherssrmain_spmax.Append("</table>");
                                        //    check1++;
                                        //}

                                        if (paxcnt == 1)
                                        {
                                            for (var i = 0; i < buildssr.Rows.Count; i++)
                                            {
                                                otherssrmain_spmax.Append("<table id='other_ssrspmax' style='width: 100%'>");
                                                otherssrmain_spmax.Append("<tr style='line-height: 30px;'><td style='width: 45%;'>" + buildssr.Rows[i]["OthrSSR_Details"].ToString() + "</td>");
                                                if (str.ToUpper() == "6E PRIME")
                                                {
                                                    otherssrmain_spmax.Append("<td class='fl'><input type='checkbox' onclick=Servicemealclick(this.id,'0','','')  class='othSpicemaxcls clsprime'  style='display:none;' id=othssridspicemax_" + (check1) + " value='" + buildssr.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + buildssr.Rows[i]["Itinref"] + "WEbMeaLWEB" + buildssr.Rows[i]["SegRef"] + "WEbMeaLWEB" + buildssr.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_destina"] + "'/><label style'border: 2px solid #00afe1;' class='cbx' for='othssridspicemax_" + (check1) + "'></label><label class='lbl' for='othssridspicemax_" + (check1) + "'></label></td><td style='text-align: left;float: left; margin-left: 5px; '><span>" + buildssr.Rows[i]["OthrSSR_Amt"] + "&nbsp;" + HttpContext.Current.Session["App_currency"] + "&nbsp;(" + buildssr.Rows[i]["Segment"] + ")</span></td></tr>");//data-primmeal=" + i + "
                                                }
                                                else if (str.ToUpper() == "FAST FORWARD")
                                                {
                                                    //string strclass = buildssr.Rows[i]["OtherSSR_orgin"] + "CLS" + buildssr.Rows[i]["OtherSSR_destina"];
                                                    otherssrmain_spmax.Append("<td class='fl'><input type='checkbox'  class='othSpicemaxcls ' style='display:none;' id=othssridspicemax_" + (check1) + " value='" + buildssr.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + buildssr.Rows[i]["Itinref"] + "WEbMeaLWEB" + buildssr.Rows[i]["SegRef"] + "WEbMeaLWEB" + buildssr.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_destina"] + "'/><label style'border: 2px solid #00afe1;' class='cbx' for='othssridspicemax_" + (check1) + "'></label><label class='lbl' for='othssridspicemax_" + (check1) + "'></label></td><td style='text-align: left;float: left; margin-left: 5px; '><span>" + buildssr.Rows[i]["OthrSSR_Amt"] + "&nbsp;" + HttpContext.Current.Session["App_currency"] + "&nbsp;(" + buildssr.Rows[i]["Segment"] + ")</span></td></tr>");//" + strclass + "
                                                    otherssrmain_spmax.Append("<tr><td colspan='3' style='font-weight: bold;'>(With Fast Forward service, you can enjoy priority check-in and priority baggage handling service.)</td><td></td></tr>");
                                                }
                                                else
                                                {
                                                    otherssrmain_spmax.Append("<td class='fl'><input type='checkbox' class='othSpicemaxcls' style='display:none;' id=othssridspicemax_" + (check1) + " value='" + buildssr.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + buildssr.Rows[i]["Itinref"] + "WEbMeaLWEB" + buildssr.Rows[i]["SegRef"] + "WEbMeaLWEB" + buildssr.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_destina"] + "'/><label style='border: 2px solid #00afe1;' class='cbx' for='othssridspicemax_" + (check1) + "'></label><label class='lbl' for='othssridspicemax_" + (check1) + "'></label></td><td style='text-align: left;float: left; margin-left: 5px; '><span>" + buildssr.Rows[i]["OthrSSR_Amt"] + "&nbsp;" + HttpContext.Current.Session["App_currency"] + "&nbsp;(" + buildssr.Rows[i]["Segment"] + ")</span></td></tr>");
                                                }
                                                otherssrmain_spmax.Append("</table>");
                                                check1++;
                                            }
                                            RetArray[Otherssrspmaxselect] = otherssrmain_spmax.ToString();//177
                                        }
                                        //OTHER SSR
                                        StringBuilder strotherssr = new StringBuilder();
                                        strotherssr.Append("<table id='other_ssrspmax' class=" + (str.ToUpper() == "6E PRIME" ? "clstblprime" : "clstblotherssr") + " style='width: 100%'>");
                                        for (var i = 0; i < buildssr.Rows.Count; i++)
                                        {
                                            strotherssr.Append("<tr style='line-height: 30px;'>");
                                            strotherssr.Append("<td style='width: 65%;'>" + buildssr.Rows[i]["OthrSSR_Details"].ToString() + "</td>");
                                            if (str.ToUpper() == "6E PRIME")
                                            {
                                                strotherssr.Append("<td class='fl'><input type='checkbox' class='clsOtherSSR clsPrimeFull clsOtherSSR_" + paxcnt + "' data-primmeal=" + i + " style='display:none;' id=othssridspicemax_" + paxcnt + "_" + otherssrcnt + " value='" + buildssr.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + buildssr.Rows[i]["Itinref"] + "WEbMeaLWEB" + buildssr.Rows[i]["SegRef"] + "WEbMeaLWEB" + buildssr.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_destina"] + "'/><label style'border: 2px solid #00afe1;' class='cbx' for='othssridspicemax_" + paxcnt + "_" + otherssrcnt + "'></label><label class='lbl' for='othssridspicemax_" + paxcnt + "_" + otherssrcnt + "'></label></td><td style='text-align: left;float: left; margin-left: 5px; '><span>" + buildssr.Rows[i]["OthrSSR_Amt"] + "&nbsp;" + HttpContext.Current.Session["App_currency"] + "&nbsp;(" + buildssr.Rows[i]["Segment"] + ")</span></td></tr>");
                                            }
                                            else if (str.ToUpper() == "FAST FORWARD")
                                            {
                                                //string strclass = buildssr.Rows[i]["OtherSSR_orgin"] + "CLS" + buildssr.Rows[i]["OtherSSR_destina"];
                                                strotherssr.Append("<td class='fl'><input type='checkbox'  class='clsOtherSSR clsOtherSSR_" + paxcnt + "' style='display:none;' id=othssridspicemax_" + paxcnt + "_" + otherssrcnt + " value='" + buildssr.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + buildssr.Rows[i]["Itinref"] + "WEbMeaLWEB" + buildssr.Rows[i]["SegRef"] + "WEbMeaLWEB" + buildssr.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_destina"] + "'/><label style'border: 2px solid #00afe1;' class='cbx' for='othssridspicemax_" + paxcnt + "_" + otherssrcnt + "'></label><label class='lbl' for='othssridspicemax_" + paxcnt + "_" + otherssrcnt + "'></label></td><td style='text-align: left;float: left; margin-left: 5px; '><span>" + buildssr.Rows[i]["OthrSSR_Amt"] + "&nbsp;" + HttpContext.Current.Session["App_currency"] + "&nbsp;(" + buildssr.Rows[i]["Segment"] + ")</span></td></tr>");// " + strclass + paxcnt + "
                                            }
                                            else
                                            {
                                                strotherssr.Append("<td class='fl'><input type='checkbox' class='clsOtherSSR clsOtherSSR_" + paxcnt + "' style='display:none;' id=othssridspicemax_" + paxcnt + "_" + otherssrcnt + " value='" + buildssr.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + buildssr.Rows[i]["Itinref"] + "WEbMeaLWEB" + buildssr.Rows[i]["SegRef"] + "WEbMeaLWEB" + buildssr.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + buildssr.Rows[i]["OtherSSR_destina"] + "'/><label style='border: 2px solid #00afe1;' class='cbx' for='othssridspicemax_" + paxcnt + "_" + otherssrcnt + "'></label><label class='lbl' for='othssridspicemax_" + paxcnt + "_" + otherssrcnt + "'></label></td><td style='text-align: left;float: left; margin-left: 5px; '><span>" + buildssr.Rows[i]["OthrSSR_Amt"] + "&nbsp;" + HttpContext.Current.Session["App_currency"] + "&nbsp;(" + buildssr.Rows[i]["Segment"] + ")</span></td></tr>");
                                            }
                                            otherssrcnt++;
                                        }
                                        strotherssr.Append("</table>");
                                        strOtherSSR.Append(strotherssr);
                                        //OTHER SSR

                                        RetArray[Otherssrspmaxheading] = true;
                                    }
                                }
                                strOtherSSR.Append("</div>");
                            }
                        }
                        RetArray[OtherSSR] = strOtherSSR;

                    }
                    else
                    {
                        RetArray[Otherssrheading] = false;
                        RetArray[Otherssrselect] = "";
                    }
                }
                else
                {
                    RetArray[Otherssrheading] = false;
                    RetArray[Otherssrselect] = "";
                }
                #endregion

            }
            catch (Exception ex)
            {

                DatabaseLog.LogData(HttpContext.Current.Session["username"].ToString(), "X", "Booking Tickets", "Create_Meals_Bagg - Building Meals and Baggage and SSR Forming Details", ex.ToString(), HttpContext.Current.Session["POS_ID"].ToString(), HttpContext.Current.Session["POS_TID"].ToString(), HttpContext.Current.Session["sequenceid"].ToString());
            }

            return RetArray;

        } // END OF CREATE MEALS

        private DataTable Linq(DataTable ds, string segment)
        {
            var qry = from p in ds.AsEnumerable()
                      where p["MyRef"].ToString().Trim() == segment
                      select p;
            return qry.CopyToDataTable();
        }

        public string buildmealdropdown(DataTable dt, bool Flag)
        {

            StringBuilder sBuild = new StringBuilder();
            try
            {
                DataRow dr = dt.NewRow();
                dr["MealCode"] = "0";
                dr["MealAmt"] = "0";
                dr["SegRef"] = "0";
                dr["ItinRef"] = "0";
                dr["MealDesc"] = "--select--";
                dr["Addsegorg"] = "0";

                string strflag = Flag.ToString();

                dt.Rows.InsertAt(dr, 0);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        sBuild.Append("<option style='position:absolute;overflow:hidden;width:98%;' value='" + dt.Rows[i]["MealCode"] + "WEbMeaLWEB" + dt.Rows[i]["MealAmt"] + "WEbMeaLWEB" + dt.Rows[i]["SegRef"] + "WEbMeaLWEB" + dt.Rows[i]["ItinRef"] + "WEbMeaLWEB" + dt.Rows[i]["Addsegorg"] + "' >" + dt.Rows[i]["MealDesc"] + "</option>");
                    }
                    else
                    {
                        if (strflag.ToUpper() == dt.Rows[i]["ServiceMeal"].ToString().ToUpper())
                        {
                            sBuild.Append("<option style='position:absolute;overflow:hidden;width:98%;' value=\"'" + dt.Rows[i]["MealCode"] + "WEbMeaLWEB" + dt.Rows[i]["MealAmt"] + "WEbMeaLWEB" + dt.Rows[i]["SegRef"] + "WEbMeaLWEB" + dt.Rows[i]["ItinRef"] + "WEbMeaLWEB" + dt.Rows[i]["Addsegorg"] + "'\" >" + dt.Rows[i]["MealDesc"] + "</option>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "buildmealdropdown", "BLLCS");
            }
            return sBuild.ToString();
        }

        public string buildBaggagedropdown(DataTable dt)
        {

            StringBuilder sBuild = new StringBuilder();
            try
            {

                DataRow dr = dt.NewRow();
                dr["BaggageCode"] = "0";
                dr["BaggageAmt"] = "0";
                dr["Addsegorg"] = "0";
                dr["ItinRef"] = "0";
                dr["BaggageDesc"] = "--select--";
                dt.Rows.InsertAt(dr, 0);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        sBuild.Append("<option value='" + dt.Rows[i]["BaggageCode"] + "WEbMeaLWEB" + dt.Rows[i]["BaggageAmt"] + "WEbMeaLWEB" + dt.Rows[i]["Addsegorg"] + "WEbMeaLWEB" + dt.Rows[i]["ItinRef"] + "' >" + dt.Rows[i]["BaggageDesc"] + "</option>");
                    }
                    else
                    {
                        sBuild.Append("<option data-connectbaggage=\" '" + dt.Rows[i]["BaggageText"] + "'\" value=\"'" + dt.Rows[i]["BaggageCode"] + "WEbMeaLWEB" + dt.Rows[i]["BaggageAmt"] + "WEbMeaLWEB" + dt.Rows[i]["Addsegorg"] + "WEbMeaLWEB" + dt.Rows[i]["ItinRef"] + "'\" >" + dt.Rows[i]["BaggageDesc"] + "</option>");

                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "buildBaggagedropdown", "BLLCS");
            }
            return sBuild.ToString();
        }

        public string buildOtherSSR(DataTable dt, int m)
        {
            StringBuilder SSRBuild = new StringBuilder();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SSRBuild.Append("<input type='checkbox' class='otherpriorit' style='display:none;' id=othssridpriority_" + (i + m) + " value='" + dt.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + dt.Rows[i]["Itinref"] + "WEbMeaLWEB" + dt.Rows[i]["SegRef"] + "WEbMeaLWEB" + dt.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + dt.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + dt.Rows[i]["OtherSSR_destina"] + "'/><label style'border: 2px solid #00afe1;' class='cbx' for='othssridpriority_" + (i + m) + "'></label><label class='lbl' for='othssridpriority_" + (i + m) + "'></label></td><td style='text-align: left;float: left; margin-left: 5px; '><span>" + dt.Rows[i]["OthrSSR_Amt"] + "&nbsp;" + HttpContext.Current.Session["App_currency"] + "&nbsp;(" + dt.Rows[i]["Segment"] + ")</span>");
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "buildOtherSSR", "BLLCS");
            }
            return SSRBuild.ToString();
        }

        public string buildSpiceMax(DataTable dt, int paxcount, int m)
        {
            StringBuilder SSRBuild = new StringBuilder();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SSRBuild.Append("<input type='checkbox' class='clsspicemax clsspicemax_" + paxcount + "' style='display:none;' id=othssridpriority_" + paxcount + "_" + (i + m) + " value='" + dt.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + dt.Rows[i]["Itinref"] + "WEbMeaLWEB" + dt.Rows[i]["SegRef"] + "WEbMeaLWEB" + dt.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + dt.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + dt.Rows[i]["OtherSSR_destina"] + "'/><label style'border: 2px solid #00afe1;' class='cbx' for='othssridpriority_" + paxcount + "_" + (i + m) + "'></label><label class='lbl' for='othssridpriority_" + paxcount + "_" + (i + m) + "'></label></td><td style='text-align: left;float: left; margin-left: 5px; '><span>" + dt.Rows[i]["OthrSSR_Amt"] + "&nbsp;" + HttpContext.Current.Session["App_currency"] + "&nbsp;(" + dt.Rows[i]["Segment"] + ")</span>");
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "buildOtherSSR", "BLLCS");
            }
            return SSRBuild.ToString();
        }

        //public string formbaggout_otherssr(DataTable dt, string SSRType)//--?
        //{
        //    StringBuilder sBuild = new StringBuilder();
        //    try
        //    {
        //        //DataRow dr = dt.NewRow();
        //        //dr["OthrSSR_Code"] = "0";
        //        //dr["Itinref"] = "0";
        //        //dr["SegRef"] = "0";
        //        //dr["OthrSSR_Amt"] = "0";
        //        //dr["OthrSSR_Details"] = "--select--";
        //        //dt.Rows.InsertAt(dr, 0);
        //        sBuild.Append("<option value='0WEbMeaLWEB0WEbMeaLWEB0WEbMeaLWEB0' >--select--</option>");
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (SSRType == dt.Rows[i]["OtherSSR_Type"].ToString())
        //            {
        //                sBuild.Append("<option value='" + dt.Rows[i]["OthrSSR_Code"].ToString() + "WEbMeaLWEB" + dt.Rows[i]["Itinref"] + "WEbMeaLWEB" + dt.Rows[i]["SegRef"] + "WEbMeaLWEB" + dt.Rows[i]["OthrSSR_Amt"] + "WEbMeaLWEB" + dt.Rows[i]["OtherSSR_orgin"] + "WEbMeaLWEB" + dt.Rows[i]["OtherSSR_destina"] + "' >" + dt.Rows[i]["OthrSSR_Details"] + " (" + dt.Rows[i]["OthrSSR_Amt"] + ")</option>");
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DatabaseLog.foldererrorlog(ex.ToString(), "buildotherssr", "BLLCS");
        //    }
        //    return sBuild.ToString();
        //}

        private DataSet FormSSR(bool status, DataTable dtssr, string ValKey)
        {
            try
            {
                DataTable dtMeals = new DataTable("Meals");
                dtMeals.Columns.Add("Segment");
                dtMeals.Columns.Add("Segment_Code");
                dtMeals.Columns.Add("MealCode");
                dtMeals.Columns.Add("MealDesc");
                dtMeals.Columns.Add("MealAmt");
                dtMeals.Columns.Add("PaxType");
                dtMeals.Columns.Add("SegRef");
                dtMeals.Columns.Add("MyRef");
                dtMeals.Columns.Add("ItinRef");

                DataTable dtBaggage = new DataTable("Baggage");
                dtBaggage.Columns.Add("Segment");
                dtBaggage.Columns.Add("Segment_Code");
                dtBaggage.Columns.Add("BaggageCode");
                dtBaggage.Columns.Add("BaggageDesc");
                dtBaggage.Columns.Add("BaggageAmt");
                dtBaggage.Columns.Add("PaxType");
                dtBaggage.Columns.Add("SegRef");
                dtBaggage.Columns.Add("MyRef");
                dtBaggage.Columns.Add("Itinref");

                DataTable dtFQT = new DataTable("FQT");
                dtFQT.Columns.Add("AirlineCode");
                int INDEX = 0;
                //bool Mealstatus = true;
                //bool Baggtatus = true;
                foreach (DataRow dr in dtssr.Rows)
                {



                    string lstrSegment = (dr["Origin"].ToString().Trim()) + " --> " +
                       (dr["Destination"].ToString().Trim());// string.Empty;

                    lstrSegment = lstrSegment.Trim().TrimEnd('>').TrimEnd('-').TrimEnd('-').Trim();
                    string lstrmeal = dr["Meal"].ToString() == null ? "" : dr["Meal"].ToString().Trim();
                    string lstrBagg = dr["Baggage"].ToString() == null ? "" : dr["Baggage"].ToString().Trim();
                    string lstrAirLines = "AI";
                    string lstrALlowFQT = dr["FQT"].ToString() == null ? "" : dr["FQT"].ToString().Trim();


                    dtMeals = (DataTable)HttpContext.Current.Session["dtmealseSelect" + ValKey];

                    dtBaggage = (DataTable)HttpContext.Current.Session["Baggageselect" + ValKey];


                    if (!string.IsNullOrEmpty(lstrALlowFQT) && !string.IsNullOrEmpty(lstrAirLines) &&
                        lstrALlowFQT.ToUpper() == "Y" && status)
                    {

                        for (int incrm = 0; incrm < lstrAirLines.Split(',').Length; incrm++)
                        {
                            if (string.IsNullOrEmpty(lstrAirLines.Split(',')[incrm].Trim()))
                                continue;
                            DataRow drb = dtFQT.NewRow();
                            drb["AirlineCode"] = lstrAirLines.Split(',')[incrm];
                            dtFQT.Rows.Add(drb);
                        }
                    }

                    INDEX++;
                }
                DataSet dsresult = new DataSet();
                dsresult.Tables.Add(dtMeals);
                dsresult.Tables.Add(dtBaggage);
                dsresult.Tables.Add(dtFQT);
                return dsresult;
            }
            catch (Exception ex)
            {
                DatabaseLog.foldererrorlog(ex.ToString(), "FormSSR", "BLLCS");
                return null;
            }
        }

    } /*END OF BLL*/
}