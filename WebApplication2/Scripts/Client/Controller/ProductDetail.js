if ("undefined" !== typeof app) {
    app.controller("ProductDetailController", function ($scope, ProductDetail, CONFIG, Helper, $routeParams) {
        $scope.view = {
            "productDetail": null
        }
        
        getProductDetail = function (productId) {
            ProductDetail.getProductDetail(productId, function (response) {
                if (response)
                {
                    $scope.view.productDetail = response.results;
                    $scope.view.productDetail.attributes[0].value = Helper.addCommasToMoney($scope.view.productDetail.attributes[0].value);
                    console.log($scope.view.productDetail);
                }
            }, function (err) {
                if (err) {
                    console.log(err);
                }
            });
        }

        getProductDetail($routeParams.product);
    });
}