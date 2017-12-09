if ("undefined" !== typeof app) {
    app.controller("ProductController", function ($scope, Product, CONFIG, Helper, $routeParams) {
        $scope.products = [];
        $scope.pageSize = 10;
        $scope.pageIndex = 1;
        $scope.filter = "id";
        $scope.rowCount = null;
        
        getProduct = function (categoryId, pageIndex, pageSize, filter) {
            Product.getProduct(categoryId, pageIndex, pageSize, filter, function (response) {
                if (response)
                {
                    console.log(response);
                    $scope.products = response.results;
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

        var categoryId = 1;
        getProduct(categoryId, $scope.pageIndex, $scope.pageSize, $scope.filter);

        $scope.pageChanged = function (pageSize, pageIndex, filter)
        {
            console.log(categoryId);
            console.log(pageSize);
            console.log(pageIndex);
            console.log(filter);
            getProduct(categoryId, pageIndex, pageSize, filter);
        }

        $scope.rowChanged = function (pageIndex, pageSize, filter) 
        {
            getProduct(categoryId, pageIndex, pageSize, filter);
            $(".btn-pageSize").removeClass("btn-primary");
            $("#" + pageSize).addClass("btn-primary");
        }

        console.log($routeParams.category);
    });
}