if ("undefined" !== typeof angular) {
    var app = angular.module('TMDT-client', ['ngCookies', 'ngRoute', 'ui.bootstrap']).config(function ($locationProvider, $routeProvider) {
        $locationProvider.html5Mode(true);
        $routeProvider.otherwise("/notfound").when("/", {
            controller: "IndexController",
            templateUrl: "./Scripts/Client/View/index.html"
        }).when("/cart", {
            controller: "CartController",
            templateUrl: "./Scripts/Client/View/cart.html"
        }).when("/notfound", {
            controller: "ErrorController",
            templateUrl: "./Scripts/Client/View/error.html"
        }).when("/login", {
            controller: "ErrorController",
            templateUrl: "./Scripts/Client/View/error.html"
        }).when("/account/confirm", {
            controller: "AccountConfirmController",
            templateUrl: "./Scripts/Client/View/confirm-email.html"
        }).when("/account/set-password", {
            controller: "SetPasswordController",
            templateUrl: "./Scripts/Client/View/set-password.html"
        }).when("/register", {
            controller: "RegisterController",
             templateUrl: "./Scripts/Client/View/register.html"
        }).when("/product/check-out", {
            controller: "InvoiceController",
            templateUrl: "./Scripts/Client/View/check-out.html"
        }).when("/:category", {
            controller: "ProductController",
            templateUrl: "./Scripts/Client/View/product.html"
        }).when("/:category/:product", {
            controller: "ProductDetailController",
            templateUrl: "./Scripts/Client/View/product-detail.html"
        });
    });
}