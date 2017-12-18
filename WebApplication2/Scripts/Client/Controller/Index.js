if ("undefined" !== typeof app) {
    app.controller("IndexController", function ($scope, CONFIG, Product, Helper, $cookieStore) {
        $scope.$on('$viewContentLoaded', function () {
            if (typeof carousels === "function") {
                carousels();
            }
        });

        $scope.products1 = [];

        getProduct = function (pageIndex, pageSize, filter) {
            Product.getProducts(pageIndex, pageSize, filter, function (response) {
                if (response) {

                    for (var i = 0; i < response.items.length; i++) {
                        response.items[i].attributes[0].value = Helper.addCommasToMoney(response.items[i].attributes[0].value);
                    }
                    $scope.products1 = response.items;
                }
            }, function (err) {
                if (err) {
                    console.log(err);
                }
            });
        };

        viewInit = function () {
            getProduct(1, 10, null);
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

        viewInit();
    });
}