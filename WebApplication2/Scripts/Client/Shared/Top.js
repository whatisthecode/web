if ("undefined" !== typeof app) {
    app.controller("TopController", function ($scope) {
        $scope.login = function () {
            location.reload();
        };
    });
}