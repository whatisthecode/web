if ("undefined" !== typeof app) {
    app.controller("ProductController", function ($scope, Product, CONFIG, Helper) {
        $scope.products = null;

        getProduct = function () {
            Product.getProduct(function (response) {
                if (response) {
                    $scope.products = response;
                    console.log($scope.products);
                }
            }, function (err) {
                if (err) {
                    console.log(err);
                }
            });
        };
        getProduct();
    });
}