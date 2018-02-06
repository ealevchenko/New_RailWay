using System.Web.Mvc;

namespace Web_RailWay.Areas.RW
{
    public class RWAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RW";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RW_default",
                "RW/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Web_RailWay.Areas.RW.Controllers" }
            );
        }
    }
}