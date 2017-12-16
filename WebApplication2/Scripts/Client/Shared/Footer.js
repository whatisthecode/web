if ("undefined" !== typeof app) {
    app.controller("FooterController", function ($scope, Helper, Category) {
        $scope.cateProducts = null;

        getCategoryType = function (id) {
            Category.getCategoryByType(id, function (success) {
                if (success) {
                    switch (id) {
                        case 1: $scope.cateBrands = success.results;
                            break;
                        case 2: $scope.cateProducts = success.results;
                            console.log($scope.cateProducts);
                            break;
                        case 3: $scope.cateAttrs = success.results;
                            break;
                        default:
                            break;
                    }
                }
            }, function (err) {
                if (err) {
                    console.log(err);
                }
            });
        };

        getCategoryType(2);
    });
}