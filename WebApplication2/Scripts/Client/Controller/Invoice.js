if ("undefined" !== typeof app) {
    app.controller("InvoiceController", function ($scope, CONFIG, ProductDetail, Helper, Invoice, $cookieStore) {
        console.log("This check out");
        $scope.view = {
            products: [],
            total: "0",
            userData: ""
        };

        $scope.createInvoice = function () {
            var req = {
                "products": $scope.view.products,
                "buyer": 6,
                "total": $scope.view.total
            };
            Invoice.createInvoice(req, function (response) {
                if (response) {
                    console.log(response);
                }
            }, function (err) {
                if (err) {
                    console.log(err);
                }
            });
        };

        viewInit = function () {
            $scope.userData = JSON.parse(localStorage.getItem("userData"));
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