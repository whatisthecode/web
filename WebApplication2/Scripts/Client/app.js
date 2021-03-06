﻿if ("undefined" !== typeof angular) {
    var app = angular.module('TMDT-client', ['ngCookies', 'ngRoute', 'ui.bootstrap']).config(function ($locationProvider, $routeProvider) {
        $locationProvider.html5Mode(true);
        $routeProvider.otherwise({
            controller: function () {
                window.open(location.origin + "/notfound", "_self");
            },
            template : ``
        }).when("/", {
            controller: "IndexController",
            templateUrl: "./Scripts/Client/View/index.html"
        }).when("/check-out", {
            controller: "InvoiceController",
            templateUrl: "./Scripts/Client/View/check-out.html"
        }).when("/cart", {
            controller: "CartController",
            templateUrl: "./Scripts/Client/View/cart.html"
        }).when("/account/confirm", {
            controller: "AccountConfirmController",
            templateUrl: "./Scripts/Client/View/confirm-email.html"
        }).when("/account/set-password", {
            controller: "SetPasswordController",
            templateUrl: "./Scripts/Client/View/set-password.html"
        }).when("/register", {
            controller: "RegisterController",
             templateUrl: "./Scripts/Client/View/register.html"
        }).when("/:category", {
            controller: "ProductController",
            templateUrl: "./Scripts/Client/View/product.html"
        }).when("/:category/:product", {
            controller: "ProductDetailController",
            templateUrl: "./Scripts/Client/View/product-detail.html"
        });
    });
}