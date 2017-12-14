if ("undefined" !== typeof app) {
    app.controller("InvoiceController", function ($scope, CONFIG, ProductDetail, Helper, Invoice, $cookieStore) {
        console.log("This check out");
        $scope.view = {
            products: [],
            total: "0"
        };

        $scope.createInvoice = function () {
            var req = {
                "products": $scope.view.products,
                "buyer": "",
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