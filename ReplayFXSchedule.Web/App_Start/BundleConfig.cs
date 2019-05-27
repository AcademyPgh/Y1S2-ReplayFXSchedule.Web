using System.Web;
using System.Web.Optimization;

namespace ReplayFXSchedule.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/umd/popper.js", 
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                "~/Scripts/dashboard/now-ui-dashboard.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/dashboard_plugins").Include(
                "~/Scripts/dashboard/plugins/moment.min.js",
                "~/Scripts/dashboard/plugins/bootstrap-notify.js",
                "~/Scripts/dashboard/plugins/bootstrap-selectpicker.js",
                "~/Scripts/dashboard/plugins/bootstrap-switch.js",
                "~/Scripts/dashboard/plugins/bootstrap-tagsinput.js",
                "~/Scripts/dashboard/plugins/chartjs.min.js",
                "~/Scripts/dashboard/plugins/fullcalendar.min.js",
                "~/Scripts/dashboard/plugins/jasny-bootstrap.min.js",
                "~/Scripts/dashboard/plugins/jquery-jvectormap.js",
                "~/Scripts/dashboard/plugins/jquery.bootstrap-wizard.js",
                "~/Scripts/dashboard/plugins/jquery.dataTables.min.js",
                "~/Scripts/dashboard/plugins/jquery.validate.min.js",
                "~/Scripts/dashboard/plugins/nouislider.min.js",
                "~/Scripts/dashboard/plugins/perfect-scrollbar.jquery.min.js",
                "~/Scripts/dashboard/plugins/sweetalert2.min.js",
                "~/Scripts/dashboard/plugins/bootstrap-datetimepicker.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/screens").Include(
                "~/Scripts/screens/jquery.marquee.min.js",
                "~/Scripts/screens/handlebars-v4.1.0.js",
                "~/Scripts/screens/slick.min.js",
                "~/Scripts/moment.js"
                ));

            bundles.Add(new StyleBundle("~/Content/screens").Include(
                "~/Content/normalize.css",
                "~/Conetnt/slick.css",
                "~/Content/screens.css"
                ));
        }
    }
}
