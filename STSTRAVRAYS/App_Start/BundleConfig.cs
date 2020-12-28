using System.Web.Optimization;

namespace STSTRAVRAYS
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/CommonAllJS").Include(
                       "~/js/bootstrap.min.js",
                       "~/js/AppsJS/WebValidation.js",
                       "~/js/raysModal.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/LayoutCommonJS").Include(
                       "~/js/Component.js",
                       "~/js/webslidemenu.js",
                       "~/js/chosen.jquery.js",
                       "~/js/jquery.xml2json.js",
                       //"~/js/breakingews.js",
                       "~/js/jquery.blockUI.js",
                       //"~/js/jquery.countTo.js",
                       "~/js/moment.min.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/LayoutOtherJS").Include(
                       "~/js/Pushme.js",
                       "~/js/holder.js",
                       "~/js/daterangepicker.js",
                       //"~/js/d3.v3.js",
                       "~/js/Slider/camera.js",
                       //"~/js/rickshaw.min.js",
                       "~/js/maniac.js"
                       ));

            bundles.Add(new StyleBundle("~/Content/CommonAllCSS").Include(
                "~/Content/CSS/Component.css",
                "~/Content/CSS/Colors.css",
                "~/Content/CSS/bootstrap.css",
                "~/Content/CSS/font-awesome.css",
                "~/Content/CSS/raysModal.css"
                ));

            bundles.Add(new StyleBundle("~/Content/LayoutCommonCSS").Include(
               "~/Content/CSS/webslidemenu.css",
               "~/Content/CSS/ionicons.css",
               //"~/Content/CSS/Search/jquerycalender.css",
               "~/Content/CSS/animate.css",
               "~/Content/CSS/slider.css",
               "~/Content/CSS/chosen.css"
               //"~/Content/CSS/homecss.css"
               ));

            bundles.Add(new StyleBundle("~/Content/LayoutOtherCSS").Include(
              //"~/Content/CSS/rickshaw.min.css",
              "~/Content/CSS/daterangepicker-bs3.css",
              "~/Content/CSS/material.css",
              "~/Content/CSS/style.css",
              "~/Content/CSS/plugins.css",
              "~/Content/CSS/helpers.css",
              "~/Content/CSS/responsive.css"
              ));
        }
    }
}