using System.Web;
using System.Web.Optimization;

namespace Web_RailWay
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //JS
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
            "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/Ajax").Include(
            "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/creative").Include(
                "~/Scripts/creative.js"));

            bundles.Add(new ScriptBundle("~/bundles/easing").Include(
                "~/Scripts/jquery.easing.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fittext").Include(
                "~/Scripts/jquery.fittext.js"));

            bundles.Add(new ScriptBundle("~/bundles/wow").Include(
                "~/Scripts/wow.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetime")
                .Include(
                 "~/Scripts/datetime/moment.min.js"
                , "~/Scripts/datetime/jquery.daterangepicker.js"
                , "~/Scripts/helpers/lib-datetime.js"
                ));


            //CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Creative/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/font-awesome.min.css",
                        "~/Content/animate.min.css",
                        "~/Content/creative.css"));

            bundles.Add(new StyleBundle("~/jquery-ui/css").Include(
                        "~/Content/jquery-ui/jquery-ui.css",
                        "~/Content/jquery-ui/jquery-ui.structure.css",
                        "~/Content/jquery-ui/jquery-ui.theme.css"));

            bundles.Add(new StyleBundle("~/datetime/css").Include("~/Content/datetime/daterangepicker.css"));
        }
    }
}
