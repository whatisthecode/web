if ("undefined" !== typeof app) {
    app.controller("IndexController", function ($scope, CONFIG, Product, Helper) {
        $scope.$on('$viewContentLoaded', function () {
            if (typeof carousels === "function") {
                carousels();
            }
        });

    });
}