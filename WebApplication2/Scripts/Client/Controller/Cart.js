﻿if ("undefined" !== typeof app) {
    app.controller("CartController", function ($scope, $cookieStore, ProductDetail, Helper, CONFIG) {
        $scope.products = [];
        $scope.selectedProducts = [];
        $scope.totalInvoice = 0;
        $scope.selectedProductsLength = 0;

        viewOninit = function () {
            $scope.selectedProducts = $cookieStore.get("selectedProducts");
            console.log($scope.selectedProducts);
            if ($scope.selectedProducts !== undefined || $scope.selectedProducts.length > 0) {
                getAllProducts();
                $scope.selectedProductsLength = $scope.selectedProducts.length.toString();
                $(".table-responsive").show();
            }
            else {
                $(".table-responsive").hide();
            }
        };

        getAllProducts = function () {
            for (var i = 0; i < $scope.selectedProducts.length; i++) {
                ProductDetail.getProductDetail($scope.selectedProducts[i], function (response) {
                    if (response) {
                        response.results["amount"] = 1;
                        response.results["productTotal"] = productTotal(response.results.attributes[0].value, response.results.amount, response.results.attributes[2].value);
                        response.results.attributes[0].value = Helper.addCommasToMoney(response.results.attributes[0].value);
                        $scope.products.push(response.results);
                        console.log($scope.products);
                    }
                }, function (err) {
                    if (err) {
                        console.log(err);
                    }
                });
            }
        };

        $scope.amountChange = function (product) {
            $("input.amount").on("keydown keyup", function (e) {
                console.log(e.keyCode);
                if (e.keyCode === 189 || e.keyCode === 187 ||
                    e.keyCode === 190 || e.keyCode === 110 ||
                    e.keyCode === 107 || e.keyCode === 109 ||
                    e.keyCode === 111) {
                    e.preventDefault();
                }
            });
            console.log(product.amount);
            if (Helper.notEmpty(product.amount) === false) {
                validator.prototype.showWarning("#errors", "checkAmountEmpty" + product.id, "Xin nhập số lượng sản phẩm cần mua");
            }
            else {
                validator.prototype.hideWarning("#errors", "checkAmountEmpty" + product.id, "Xin nhập số lượng sản phẩm cần mua");
            }
            if (validateInputAmount(product.attributes[1].value, product.amount) === false) {
                validator.prototype.showWarning("#errors", "checkAmount" + product.id, "Số lượng sản phẩm cần mua vượt quá số lượng sản phẩm được bán");
            }
            else {
                validator.prototype.hideWarning("#errors", "checkAmount" + product.id, "Số lượng sản phẩm cần mua vượt quá số lượng sản phẩm được bán");
            }
            product.productTotal = productTotal(product.attributes[0].value, product.amount, product.attributes[2].value);
            $scope.totalInvoice = sumInvoice($scope.products);
            return product;
        };

        productTotal = function (price, amount, discount) {
            return Helper.addCommasToMoney((parseInt(Helper.removeCommas(price)) - parseInt(Helper.removeCommas(discount))) * parseInt(amount).toString());
        };

        sumInvoice = function (products) {
            var tong = 0;
            for (var i = 0; i < products.length; i++) {
                tong = tong + parseInt(Helper.removeCommas(products[i].productTotal));
            }
            return Helper.addCommasToMoney(tong);
        };


        $scope.removeSelectedProduct = function (productId) {
            for (var i = 0; i < $scope.selectedProducts.length; i++) {
                if ($scope.selectedProducts[i] === productId) {
                    $scope.selectedProducts.splice(i, 1);
                    $cookieStore.put("selectedProducts", $scope.selectedProducts);
                }

            }
            for (var j = 0; j < $scope.products.length; j) {
                if ($scope.products[j].id === productId)
                    $scope.products.splice(j, 1);
            }
        };

        validateInputAmount = function (productAmount, buyAmount) {
            if (parseInt(productAmount) >= parseInt(buyAmount))
                return true;
            else
                return false;
        };

        $scope.checkout = function () {
            if ($("#errors").children().length > 0) {
                console.log("Have errors");
                return;
            }
            else {
                var checkOuts = [];
                for (var i = 0; i < $scope.products.length; i++) {
                    if ($scope.products[i].amount > 0) {
                        var checkOut = {
                            "amount": $scope.products[i].amount,
                            "productId": $scope.products[i].id
                        };
                        checkOuts.push(checkOut);
                    }
                }
                $cookieStore.put("checkOuts", checkOuts);
                window.location.href = "";
            }
        };
            

        viewOninit();
        setTimeout(function () {
            $scope.totalInvoice = sumInvoice($scope.products);
            console.log($scope.totalInvoice);
        }, 200);
    });
}