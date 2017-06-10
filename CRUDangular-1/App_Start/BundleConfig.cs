using System.Web;
using System.Web.Optimization;

namespace CRUDangular_1
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
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/site1.css"));

            bundles.Add(new ScriptBundle("~/bundles/mini-spa/script").Include("~/Scripts/jquery.min.js",
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-route.min.js",
                "~/Scripts/angular-material/angular-material.min.js",
                "~/Scripts/angular-aria/angular-aria.min.js",
                "~/Scripts/angular-animate/angular-animate.min.js",
                "~/Scripts/angular-ui/ui-bootstrap.min.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                "~/Scripts/ngDialog.min.js",
                "~/Scripts/select.min.js",
                "~/Scripts/dirPagination.js",
                "~/Scripts/angular-file-upload.min.js",
                "~/Scripts/angular-bootstrap-lightbox.min.js",
                "~/Scripts/ng-table.min.js",
                "~/Scripts/FileSaver.min.js",
                "~/app/AngularForm.js",
                "~/app/DataService.js",
                 "~/app/ValidationDirective.js", 
                "~/app/TutorForm/tfcontroller.js",
                "~/app/TutorForm/tfDirective.js",
                "~/app/QualForm/qfcontroller.js",
                "~/app/QualForm/qfDirective.js",
                "~/app/ExpeForm/expeController.js",
                "~/app/itemForm/ifController.js",
                "~/app/itemForm/ifDirective.js"));

            bundles.Add(new StyleBundle("~/bundles/mini-spa/style").Include("~/Content/bootstrap.min.css",
                      "~/Content/app.css",
                      "~/Content/ngDialog.css",
                      "~/Content/ngDialog-theme-default.css",
                      "~/Content/ngDialog.min.css",
                      "~/Content/select.min.css",
                      "~/Content/ng-table.min.css",
                     // "~/Content/angular-material.css"
                    //  "~/Content/angular-material.layouts.css"
                     "~/Content/angular-material.layouts.min.css",
                     "~/Content/angular-bootstrap-lightbox.min.css",
                      "~/Content/angular-material.min.css"


                ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
