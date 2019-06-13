using System.Web.Mvc;

namespace Web_RailWay.Areas.TD
{
    public class TDAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TD";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TD_default",
                "TD/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Web_RailWay.Areas.TD.Controllers" }
            );
        }
    }
}