using System.Web;
using System.Web.Optimization;

namespace EMS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/angular.min.js",
                      "~/Assets/app.js",
                      "~/Assets/common.js",
                      "~/Assets/menu.js",
                      "~/Assets/querymodule.js",
                      "~/Assets/user.js",
                      "~/Assets/role.js",
                      "~/Assets/enquirydataentry.js",
                      "~/Assets/assignenquiry.js",
                      "~/Assets/callhistory.js",
                      "~/Assets/flatowners.js",
					  "~/Assets/roleMenuMapping.js",
                      "~/Assets/lovcategory.js",
                      "~/Assets/lov.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
