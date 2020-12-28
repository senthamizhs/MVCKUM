using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace STSTRAVRAYS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: "agent-registration", url: "agent-registration", defaults: new { controller = "Login", action = "Registration" });
            routes.MapRoute(name: "agent-registration/thank-you", url: "agent-registration/thank-you", defaults: new { controller = "Login", action = "Thankyou" });
            routes.MapRoute(name: "About", url: "About", defaults: new { controller = "B2C", action = "About" });
            routes.MapRoute(name: "Contact-us", url: "Contactus", defaults: new { controller = "B2C", action = "Contactus_RT" });
            routes.MapRoute(name: "Disclaimer", url: "Disclaimer", defaults: new { controller = "B2C", action = "Disclaimer" });
            routes.MapRoute(name: "Privacy-policy", url: "Privacypolicy", defaults: new { controller = "B2C", action = "Privacypolicy" });
            routes.MapRoute(name: "terms-and-conditions", url: "termsandconditions", defaults: new { controller = "B2C", action = "termsandconditions" });
            routes.MapRoute(name: "User-agreement", url: "Useragreement", defaults: new { controller = "B2C", action = "Useragreement" });
            routes.MapRoute(name: "products-and-serivces", url: "productsandserivces", defaults: new { controller = "B2C", action = "productsandserivces" });
            routes.MapRoute(name: "E-Ticket", url: "E-Ticket", defaults: new { controller = "B2C", action = "PrintETicket" });
            routes.MapRoute(name: "EmergencyUpdate", url: "COVID-19-Updates", defaults: new { controller = "Redirect", action = "EmergencyUpdate" });
            routes.MapRoute(name: "EmergencyEvacuation", url: "EmergencyEvacuation", defaults: new { controller = "OtherProduct", action = "EmergencyEvacuation" });
            routes.MapRoute(name: "become-an-agent", url: "become-an-agent", defaults: new { controller = "Login", action = "RTRegistration" });

            if (System.Configuration.ConfigurationManager.AppSettings["APP_HOSTING"] != null && System.Configuration.ConfigurationManager.AppSettings["APP_HOSTING"].ToString() == "BSA")
            {
                routes.MapRoute(name: "Default",url: "{controller}/{action}/{id}", defaults: new { controller = "Flights", action = "Flights", id = UrlParameter.Optional });
            }
            else
            {
                routes.MapRoute(name: "Default",url: "{controller}/{action}/{id}",defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional });
            }
            
        }
    }
}