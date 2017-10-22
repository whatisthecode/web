if ("undefined" !== typeof app) {
    app.controller("IndexController", function ($scope) {
        $scope.$on('$viewContentLoaded', function () {
            if (typeof carousels === "function") {
                carousels();
            }
        }); 
    });
}