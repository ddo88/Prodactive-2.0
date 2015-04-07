using System.Web.Optimization;

namespace Zeitgeist.Web
{
    public class BundleConfig
    {
        // Para obtener más información acerca de Bundling, consulte http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/all").Include(
                "~/Scripts/bootstrap.min.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/themejs").Include(
                "~/Content/theme/js/jquery-ui.custom.min.js",
                "~/Content/theme/js/jquery.easypiechart.min.js",
                "~/Content/theme/js/jquery.sparkline.min.js",
                "~/Content/theme/js/jquery.flot.min.js",
                "~/Content/theme/js/jquery.flot.pie.min.js",
                "~/Content/theme/js/jquery.flot.resize.min.js",
                "~/Content/theme/js/ace-elements.min.js",
                "~/Content/theme/js/ace.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/themecss").Include(
                "~/Content/theme/css/font-awesome.min.css",
                "~/Content/theme/css/ace-fonts.css",
                "~/Content/theme/css/ace.min.css",
                "~/Content/theme/css/ace-rtl.min.css",
                "~/Content/theme/css/jquery-ui.custom.min.css",
                "~/Content/theme/css/jquery-ui.min.css"
                ));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de creación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));


            /*Chameleon Forms*/

            bundles.Add(new ScriptBundle("~/bundles/chameleon-bootstrapjs").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.validate.unobtrusive.twitterbootstrap.js"
            ));
            bundles.Add(new StyleBundle("~/bundles/chameleon-bootstrapcss").Include(
                "~/Content/bootstrap.css",
                "~/Content/chameleonforms-twitterbootstrap.css"
            ));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //    "~/Content/themes/base/jquery.ui.core.css",
            //    "~/Content/themes/base/jquery.ui.resizable.css",
            //    "~/Content/themes/base/jquery.ui.selectable.css",
            //    "~/Content/themes/base/jquery.ui.accordion.css",
            //    "~/Content/themes/base/jquery.ui.autocomplete.css",
            //    "~/Content/themes/base/jquery.ui.button.css",
            //    "~/Content/themes/base/jquery.ui.dialog.css",
            //    "~/Content/themes/base/jquery.ui.slider.css",
            //    "~/Content/themes/base/jquery.ui.tabs.css",
            //    "~/Content/themes/base/jquery.ui.datepicker.css",
            //    "~/Content/themes/base/jquery.ui.progressbar.css",
            //    "~/Content/themes/base/jquery.ui.theme.css"));
            //BundleTable.EnableOptimizations = true;
        }
    }
}