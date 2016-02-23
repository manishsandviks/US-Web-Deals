using System.Web.Mvc;

namespace deals.earlymoments.com.Areas.Max
{
    public class MaxAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Max";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "offers",
               "max/offers/{id}",
               new { controller = "Home", action = "offers", id = UrlParameter.Optional }
           );

            context.MapRoute(
                "Max_default",
                "Max/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}