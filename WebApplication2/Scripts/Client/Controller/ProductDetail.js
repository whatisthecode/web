if ("undefined" !== typeof app) {
    app.controller("ProductDetailController", function ($scope, ProductDetail, CONFIG, Helper, $routeParams, $cookieStore) {
        $scope.view = {
            "productDetail": null,
            "currentCategory": $routeParams.category,
            "selectedProducts": []
        };
        
        getProductDetail = function (productId) {
            ProductDetail.getProductDetail(productId, function (response) {
                if (response) {
                    $scope.view.productDetail = response.results;
                    $scope.view.productDetail.attributes[0].value = Helper.addCommasToMoney($scope.view.productDetail.attributes[0].value);
                }
            }, function (err) {
                if (err) {
                    console.log(err);
                }
            });
        };


        $scope.addProductToCart = function (product) {
            $scope.view.selectedProducts = $cookieStore.get("selectedProducts");
            if ($scope.view.selectedProducts === undefined) {
                var selectedProducts = [];
                selectedProducts.push(product.id);
                $cookieStore.put("selectedProducts", selectedProducts);
            } else {
                if (Helper.checkItemExistInArray($scope.view.selectedProducts, product.id) === false) {   
                    $scope.view.selectedProducts.push(product.id);
                    $cookieStore.put("selectedProducts", $scope.view.selectedProducts);
                }
            }

        };

        viewOninit = function () {
            getProductDetail($routeParams.product);
        };

        viewOninit();
        
    });
}