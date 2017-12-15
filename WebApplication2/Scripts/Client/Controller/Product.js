﻿if ("undefined" !== typeof app) {
    app.controller("ProductController", function ($scope, Product, CONFIG, Helper, $routeParams, $cookieStore) {
        $scope.products = [];
        $scope.pageSize = 10;
        $scope.pageIndex = 1;
        $scope.filter = "name";
        $scope.asceding = false;
        $scope.rowCount = null;
        $scope.currentPage = "";

        var categoryId = null;
        getProduct = function (pageIndex, pageSize, filter) {
            switch ($routeParams.category) {
                case "ban-phim":
                    categoryId = 1;
                    $scope.currentPage = "Bàn Phím";
                    break;
                case "chuot":
                    categoryId = 2;
                    $scope.currentPage = "Chuột";
                    break;
                default:
                    break;
            }
            Product.getProduct(categoryId, pageIndex, pageSize, filter, function (response) {
                if (response)
                {
                    
                    for (var i = 0; i < response.items.length; i++)
                    {
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
        };

        getProduct($scope.pageIndex, $scope.pageSize, $scope.filter);

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
    });
}