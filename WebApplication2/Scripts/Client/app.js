if ("undefined" !== typeof angular) {
    var app = angular.module('TMDT-client', ['ngCookies', 'ngRoute']).config(function ($locationProvider, $routeProvider) {
        $locationProvider.html5Mode(true);
        $routeProvider.otherwise("/404").when("/", {
            controller: "IndexController",
            templateUrl: "./Scripts/Client/View/index.html"
        }).when("/404", {
            controller: "ErrorController",
            templateUrl: "./Scripts/Client/View/error.html"
        });
    });
}