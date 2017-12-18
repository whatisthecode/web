if ("undefined" !== typeof app) {
    app.controller("HeaderController", function ($scope, Helper, Product) {
        $scope.keyword = "";
        $scope.itemSearch = null;
        $scope.search = function (keyword) {
            console.log(keyword);
            if (Helper.notEmpty(keyword)) {
                Product.searchProduct(keyword, function (response) {
                    $("div#results ul").show();
                    $scope.itemSearch = response;
                }, function (err) {
                    console.log(err);
                });
            }
            else
            {
                $("div#results ul").hide();
            }
        };

        $scope.viewDetail = function (categoryId, productId) {
            var cate = null;
            switch (categoryId)
            {
                case 1:
                    cate = "MOUSE";
                    break;
                case 2:
                    cate = "KEYBOARD";
                    break;
                default:
                    break;
            }
            window.location.href = cate + "/" + productId;
        };
    });
}