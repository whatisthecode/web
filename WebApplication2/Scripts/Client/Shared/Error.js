if ("undefined" !== typeof app) {
    app.controller("ErrorController", function ($scope, $location) {
        $scope.errorType = 0;
        if ($location.path() === "/404") {
            $scope.errorType = 404;
        }
        else {
            $scope.errorType = 403;
        }
    });
}