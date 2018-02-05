using System.Web.Mvc;

namespace Web_RailWay.Areas.MT
{
    public class MTAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MT_default",
                "MT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Web_RailWay.Areas.MT.Controllers" }
            );
        }
    }
}