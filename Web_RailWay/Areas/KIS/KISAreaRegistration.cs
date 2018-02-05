using System.Web.Mvc;

namespace Web_RailWay.Areas.KIS
{
    public class KISAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "KIS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "KIS_default",
                "KIS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Web_RailWay.Areas.KIS.Controllers" }
            );
        }
    }
}