if ("undefined" !== typeof app) {
    app.controller("TopController", function ($scope, Helper, Login) {
        $scope.isLogin = localStorage.getItem("token") ? true : false;
        $scope.username = JSON.parse(localStorage.getItem("username")) || "";
        $scope.login = function () {
            window.open(location.origin + "/login", "_self");
        };
        $scope.logout = function () {
            window.open(location.origin + "/logout", "_self");
        }
        $scope.info = function () {
            window.open(location.origin + "/dashboard/info", "_self");
        }
    });
}