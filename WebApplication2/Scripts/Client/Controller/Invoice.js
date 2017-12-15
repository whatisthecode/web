if ("undefined" !== typeof app) {
    app.controller("InvoiceController", function ($scope, CONFIG, ProductDetail, Helper, Invoice, $cookieStore, User) {
        $scope.view = {
            products: [],
            total: "0",
            userData: ""
        };

        $scope.createInvoice = function () {
            var req = {
                "products": $scope.view.products,
                "buyer": $scope.view.userData.userInfo.id,
                "total": $scope.view.total
            };
            Invoice.createInvoice(req, function (response) {
                if (response) {
                    console.log(response);
                    $cookieStore.remove("checkOuts");
                    $cookieStore.remove("selectedProducts");
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
            $scope.view.products = checkOuts;
            console.log(checkOuts);
            var sum = 0;
            for (var i = 0; i < checkOuts.length; i++)
            {
                sum = sum + Number(Helper.removeCommas(checkOuts[i].price));
            }
            $scope.view.total = Helper.addCommasToMoney(sum);
        };

        viewInit();
    });
}