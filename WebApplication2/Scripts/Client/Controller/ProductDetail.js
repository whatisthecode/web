if ("undefined" !== typeof app) {
    app.controller("ProductDetailController", function ($scope, ProductDetail, CONFIG, Helper, $routeParams, $cookieStore, Category) {
        $scope.view = {
            "productDetail": null,
            "currentCategory": $routeParams.category,
            "selectedProducts": []
        };
        $scope.cateBrands = null;
        $scope.cateProducts = null;
        $scope.cateAttrs = null;
        $scope.categoryName = null;
        
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
            if (Helper.notEmpty($scope.view.selectedProducts) === false) {
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

        breadCrumb = function () {
            switch ($routeParams.category) {
                case "KEYBOARD":
                    $scope.categoryName = "Bàn phím";
                    break;
                case "MOUSE":
                    $scope.categoryName = "Chuột";
                    break;
                default:
                    break;
            }
        };

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

        viewOninit = function () {
            getProductDetail($routeParams.product);
            getCategoryType(1);
            getCategoryType(2);
            getCategoryType(3);
            breadCrumb();
        };

        viewOninit();
        
    });
}