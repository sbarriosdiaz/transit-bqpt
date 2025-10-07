////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Web.Optimization;

namespace Bqpt.ExternalUI
{
    public class BundleConfig
    {
        protected BundleConfig()
        {
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/template-support/jquery/jquery-{version}.js"
                        ));

            // jQuery Validate
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/template-support/jquery/jquery.validate*"));

            // Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/jsbootstrap").Include(
                      "~/Scripts/template-support/bs4/umd/popper.js",
                      "~/Scripts/template-support/bs4/bootstrap.js"));

            bundles.Add(new StyleBundle("~/content/css-front").Include(
                    "~/Content/external-template-support/*.css"));

            // CSS style (bootstrap/template)
            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                      "~/Content/template-support/bs4/bootstrap.css",
                      "~/Content/template-support/css/style.css",
                      "~/Content/template-support/css/bs4-toggle.min.css",
                      "~/Scripts/template-support/select2/css/select2.css",
                      "~/Scripts/template-support/select2-bootstrap-css/select2-bootstrap.css",
                      "~/Content/template-support/bs4/docs.css",
                      "~/Content/template-support/css/jasny-bootstrap.css",
                      "~/Content/template-support/css/toastr.css",
                      "~/Content/template-support/icheck/minimal/minimal.css",
                      "~/Scripts/template-support/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css",
                      "~/Content/template-support/css/custom.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/bundles/fa").Include(
                      "~/Content/template-support/fa/css/all.css", new CssRewriteUrlTransform()));

            // Template script
            bundles.Add(new ScriptBundle("~/bundles/jstemplate").Include(
                      "~/Scripts/application-framework/jquery.metisMenu.js",
                      "~/Scripts/application-framework/jquery.slimscroll.js",
                      "~/Scripts/application-framework/pace.min.js",
                      "~/Scripts/application-framework/select2.js",
                      "~/Scripts/application-framework/toastr.js",
                      "~/Scripts/application-framework/jquery.blockUI.js",
                      "~/Scripts/application-framework/jquery.textareaCounter.plugin.js",
                      "~/Scripts/application-framework/jquery.mask.js",
                      "~/Scripts/application-framework/sweetalert.min.js",
                      "~/Scripts/application-framework/jquery.userTimeout.js",
                      "~/Scripts/application-framework/jquery.number.js",
                      "~/Scripts/application-framework/jquery.form.js",
                      "~/Scripts/application-framework/moment.js",
                      "~/Scripts/application-framework/globalAjaxLoader.js",
                      "~/Scripts/application-framework/bootstrap-datetimepicker.js",
                      "~/Scripts/application-framework/bootstrap-editable.js",
                      "~/Scripts/application-framework/clipboard.js",
                      "~/Scripts/application-framework/bs4-toggle.min.js",
                      "~/Scripts/template-support/axios/axios.min.js",
                      "~/Scripts/template-support/tooling/template-support.js"));

            // dataTables css styles
            bundles.Add(new StyleBundle("~/bundles/dtstyles").Include(
                      "~/Scripts/template-support/dataTables/css/dataTables.bootstrap4.css",
                      "~/Scripts/template-support/dataTables/css/buttons.bootstrap4.css"));

            // dataTables
            bundles.Add(new ScriptBundle("~/bundles/dtjs").Include(
                      "~/Scripts/template-support/dataTables/js/jquery.dataTables.js",
                      "~/Scripts/template-support/dataTables/js/dataTables.bootstrap4.js",
                      "~/Scripts/template-support/dataTables/js/dataTables.buttons.js",
                      "~/Scripts/template-support/dataTables/js/buttons.bootstrap4.js",
                      "~/Scripts/template-support/dataTables/js/jszip.min.js",
                      "~/Scripts/template-support/dataTables/js/pdfmake.min.js",
                      "~/Scripts/template-support/dataTables/js/vfs_fonts.js",
                      "~/Scripts/template-support/dataTables/js/buttons.html5.js",
                      "~/Scripts/template-support/dataTables/js/buttons.print.js"
                     ));

            // summernote JS
            bundles.Add(new ScriptBundle("~/bundles/snjs").Include(
                    "~/Scripts/template-support/summernote/summernote-bs4.js",
                    "~/Scripts/template-support/summernote/summernote-cleaner.js"));

            //summernote css
            bundles.Add(new StyleBundle("~/bundles/snstyles").Include(
                      "~/Content/template-support/summernote/summernote-bs4.css", new CssRewriteUrlTransform()));

            // custom JS apps for View Engine
            bundles.Add(new ScriptBundle("~/bundles/jsengine").Include(
                        "~/Scripts/dist/*.es5.js"
                        ));

            // set to true when in PROD stage, is false for DEV
            BundleTable.EnableOptimizations = true;
        }
    }
}