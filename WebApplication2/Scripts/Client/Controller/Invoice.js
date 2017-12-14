if ("undefined" !== typeof app) {
    app.controller("InvoiceController", function ($scope, CONFIG, ProductDetail, Helper, Invoice, $cookieStore, User) {
        console.log("This check out");
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
                }
            }, function (err) {
                if (err) {
                    console.log(err.data.status);
                    validator.prototype.showWarning("#Errors", "#checkLogin", err.data.status);
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