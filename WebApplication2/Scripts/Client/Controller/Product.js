if ("undefined" !== typeof app) {
    app.controller("ProductController", function ($scope, Product, CONFIG, Helper, $routeParams, $cookieStore, Category) {
        $scope.products = [];
        $scope.pageSize = 10;
        $scope.pageIndex = 1;
        $scope.filter = "name";
        $scope.asceding = false;
        $scope.rowCount = null;
        $scope.currentPage = "";
        $scope.cateBrands = null;
        $scope.cateProducts = null;
        $scope.cateAttrs = null;

        var categoryId = null;
        getProduct = function (pageIndex, pageSize, filter) {
            Category.getCategoryByCode($routeParams.category, function (response) {
                console.log(response);
                categoryId = response.id;
                $scope.currentPage = response.name;

                Product.getProduct(categoryId, pageIndex, pageSize, filter, function (response) {
                    if (response) {

                        for (var i = 0; i < response.items.length; i++) {
                            response.items[i].attributes[0].value = Helper.addCommasToMoney(response.items[i].attributes[0].value);
                        }
                        $scope.products = response.items;
                        $scope.pageSize = response.pageSize;
                        $scope.pageIndex = response.currentPage;
                        $scope.rowCount = response.rowCount;
                    }
                }, function (err) {
                    if (err) {
                        console.log(err);
                    }
                });

            }, function (err) {
                console.log(err);
            });
        };

        $scope.pageChanged = function (pageSize, pageIndex, filter) {
            getProduct(pageIndex, pageSize, filter);
        };

        $scope.rowChanged = function (pageIndex, pageSize, filter, asceding) {
            if (filter === "priceUp")
                asceding = true;
            else
                asceding = false;
            getProduct(pageIndex, pageSize, filter, asceding);
            $(".btn-pageSize").removeClass("btn-primary");
            $("#" + pageSize).addClass("btn-primary");
        };

        $scope.viewDetail = function (productId) {
            window.location.href = $routeParams.category + "/" + productId;
        };

        var selectedProducts = [];
        $scope.addProductToCart = function (product) {
            selectedProducts = $cookieStore.get("selectedProducts");
            if (Helper.notEmpty(selectedProducts) === false) {
                var selectedProducts = [];
                selectedProducts.push(product.id);
                $cookieStore.put("selectedProducts", selectedProducts);
            } else {
                if (Helper.checkItemExistInArray(selectedProducts, product.id) === false) {
                    selectedProducts.push(product.id);
                    $cookieStore.put("selectedProducts", selectedProducts);
                }
            }

        };

        getCategoryType = function (id) {
            Category.getCategoryByType(id, function (success) {
                if (success) {
                    console.log(success);
                    switch (id)
                    {
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

        

        viewInit = function() {
            getProduct($scope.pageIndex, $scope.pageSize, $scope.filter);
            getCategoryType(1);
            getCategoryType(2);
            getCategoryType(3);
        };
        viewInit();
    });
}