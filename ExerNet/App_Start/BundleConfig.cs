using System.Web;
using System.Web.Optimization;

namespace Exernet
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Script/jquery.js",
                        "~/Script/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*" ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/markdownjs").Include(
                        "~/Content/bootstrap-markdown/bootstrap-markdown.js",
                        "~/Scripts/MarkdownDeepLib.min.js",
                        "~/Scripts/markdown.js",
                        "~/Scripts/to-markdown.js"));

            bundles.Add(new StyleBundle("~/Content/lightcss").Include(
                      "~/Content/bootstrap-united.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/darkcss").Include(
                      "~/Content/bootstrap-superhero.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/colorcss").Include(
                      "~/Content/bootstrap-amelia.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/markdown").Include(
               "~/Content/bootstrap-markdown/bootstrap-markdown.min.css"
               ));
        }
    }
}
