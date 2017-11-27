using System.Web;
using System.Web.Optimization;

namespace WebApplication2
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/client_js").Include(
                        "~/Assets/JS/jquery-1.10.2.min.js",
                        "~/Assets/JS/bootstrap.min.js",
                        "~/Assets/JS/respond.js",
                        "~/Assets/JS/Client/jquery.cookie.js",
                        "~/Assets/JS/Client/waypoints.min.js",
                        "~/Assets/JS/modernizr-2.6.2.js",
                        "~/Assets/JS/Client/bootstrap-hover-dropdown.js",
                        "~/Assets/JS/Client/owl.carousel.min.js",
                        "~/Assets/JS/Client/front.js",
                        "~/Scripts/Client/app.js",
                        "~/Scripts/Client/app.config.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                        "~/Assets/JS/angular.min.js",
                        "~/Assets/JS/angular-route.min.js",
                        "~/Assets/JS/angular-cookies.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/client_controller").Include(
                       "~/Scripts/Client/Shared/Top.js",
                       "~/Scripts/Client/Shared/Error.js",
                       "~/Scripts/Client/Controller/Index.js",
                       "~/Scripts/Client/Controller/Cart.js",
                       "~/Scripts/Client/Controller/Product.js",
                       "~/Scripts/Client/Controller/ProductDetail.js",
                       "~/Scripts/Client/Controller/AccountConfirm.js",
                       "~/Scripts/Client/Controller/SetPassword.js",
                       "~/Scripts/Client/Controller/Login.js",
                       "~/Scripts/Client/Controller/Register.js"));
            bundles.Add(new ScriptBundle("~/bundles/client_service").Include(
                        "~/Scripts/Client/Service/Helper.js",
                        "~/Scripts/Client/Service/Oauth2.js",
                        "~/Scripts/Client/Service/API.js",
                        "~/Scripts/Client/Service/Login.js",
                        "~/Scripts/Client/Service/User.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Assets/JS/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Assets/JS/bootstrap.js",
                      "~/Assets/JS/respond.js"));

            bundles.Add(new StyleBundle("~/Content/client_template").Include(
                      "~/Assets/CSS/font-awesome.css",
                      "~/Assets/CSS/bootstrap.min.css",
                      "~/Assets/CSS/Client/animate.min.css",
                      "~/Assets/CSS/Client/owl.carousel.css",
                      "~/Assets/CSS/Client/owl.theme.css",
                      "~/Assets/CSS/Client/style.default.css",
                      "~/Assets/CSS/Client/custom.css"));
        }
    }
}
