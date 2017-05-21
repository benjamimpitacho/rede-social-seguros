using System.Web;
using System.Web.Optimization;

namespace InsuranceWebsite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            /*
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            */
            
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      /*"~/Content/bookpost/jquery.1.11.1.min.js",*/
                      "~/Content/bookpost/bootstrap.min.js",
                      "~/Content/bookpost/custom.js"));

            bundles.Add(new ScriptBundle("~/Scripts/jsGrid").Include(
                      /*"~/Content/bookpost/jquery.1.11.1.min.js",*/
                      "~/Content/jsGrid/jsgrid.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bookpost/css/bootstrap.min.css",
                      "~/Content/bookpost/css/animate.min.css",
                      "~/Content/bookpost/css/font-awesome.min.css",
                      "~/Content/bookpost/css/timeline.css",
                      "~/Content/Site.css"/*,
                      "~/Content/Facebook/css/AtR00FcnuUe.css",
                      "~/Content/Facebook/css/gNs_8jOawWE.css",
                      "~/Content/Facebook/css/AvCSouKPVJu.css",
                      "~/Content/Facebook/css/LoCNWJ5Kj3W.css",
                      "~/Content/Facebook/css/MebYOZ-gq9c.css",
                      "~/Content/Facebook/css/n_s5vQ0E5ir.css",
                      "~/Content/Facebook/css/tigqm6m_uPB.css"*/));

            bundles.Add(new StyleBundle("~/Content/jsGrid").Include(
                      "~/Content/jsGrid/jsgrid.min.css",
                      "~/Content/jsGrid/jsgrid-theme.min.css"));
        }
    }
}
