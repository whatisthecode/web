if ("undefined" !== typeof app) {
    app.controller("InvoiceController", function ($scope, CONFIG, ProductDetail, Helper, Invoice, $cookieStore, User) {
        $scope.view = {
            products: [],
            total: "0",
            userData: "",
            show: true
        };

        $scope.createInvoice = function () {
            var req = {
                "products": $scope.view.products,
                "buyer": $scope.view.userData.userInfo.id,
                "total": $scope.view.total
            };
            console.log(req);
            Invoice.createInvoice(req, function (response) {
                if (response) {
                    console.log(response);
                    $cookieStore.remove("checkOuts");
                    $cookieStore.remove("selectedProducts");
                    $scope.view.show = false;
                }
            }, function (err) {
                if (err) {
                    console.log(err.data.status);
                    switch (err.data.code)
                    {
                        case "409":
                            {
                                validator.prototype.showWarning("#errors", "checkLogin", "Bạn không thể mua sản phẩm của chính mình!");
                            }
                            break;
                        default:
                            console.log(err);
                            break;
                    }
                    
                }
            });
        };

        getUserInfo = function () {
            User.getUserInfo(function (response) {
                if (response) {
                    $scope.view.userData = response.results;
                    console.log($scope.view.userData);
                }
            }, function (err) {
                if (err) {
                    console.log(err);
                }
            });
        };

        $scope.removeInvoice = function () {
            $cookieStore.remove("checkOuts");
            $cookieStore.remove("selectedProducts");
            window.location.href = "/";
        };

        viewInit = function () {
            getUserInfo();
            var checkOuts = $cookieStore.get("checkOuts");
            if (!Helper.notEmpty(checkOuts))
            {
                setTimeout(function () {
                    alert("Bạn chưa có sản phẩm nào để thanh toán.");
                }, 1000);
                setTimeout(function () {
                    window.location.href = "/";
                }, 2000);
            }
            $scope.view.products = checkOuts;
            var sum = 0;
            for (var i = 0; i < checkOuts.length; i++)
            {
                sum = sum + Number(Helper.removeCommas(checkOuts[i].price));
            }
            $scope.view.total = Helper.addCommasToMoney(sum);
            $scope.view.show = true;
        };

        viewInit();
    });
}