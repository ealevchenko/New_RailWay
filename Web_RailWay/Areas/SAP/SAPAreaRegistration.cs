using System.Web.Mvc;

namespace Web_RailWay.Areas.SAP
{
    public class SAPAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SAP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SAP_default",
                "SAP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}