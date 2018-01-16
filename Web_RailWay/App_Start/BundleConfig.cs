using System.Web;
using System.Web.Optimization;

namespace Web_RailWay
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //-----------------------------------------------------------------------
            //JS
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.2.1.min.js"));
                        //"~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            "~/Scripts/jquery.validate.min.js"));
            //"~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
            "~/Scripts/jquery-ui-1.12.1.min.js"));
            //"~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/Ajax").Include(
            "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      //"~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap.min.js",
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
            // Плагин таблицы
            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
                "~/Scripts/DataTables/jquery.dataTables.min.js",
                "~/Scripts/DataTables/dataTables.jqueryui.min.js"
                ));

            //-----------------------------------------------------------------------
            //CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        //"~/Content/bootstrap.css",
                        "~/Content/bootstrap.min.css",
                        "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Creative/css").Include(
                //"~/Content/bootstrap.css",
                "~/Content/bootstrap.min.css",
                        "~/Content/font-awesome.min.css",
                        "~/Content/animate.min.css",
                        "~/Content/creative.css"));

            bundles.Add(new StyleBundle("~/jquery-ui/css").Include(
                        "~/Content/jquery-ui.css",
                        "~/Content/jquery-ui.structure.css",
                        "~/Content/jquery-ui.theme.css"));

            bundles.Add(new StyleBundle("~/datetime/css").Include("~/Content/datetime/daterangepicker.css"));
            // Плагин таблицы
            bundles.Add(new StyleBundle("~/DataTables/css").Include(
                "~/Content/DataTables/css/jquery.dataTables.min.css",
                "~/Content/DataTables/css/datatables.css"));

                //"~/Content/table/reports-dataTables.css"));
        }
    }
}
