if ("undefined" !== typeof app) {
    app.factory('Product', function (API, Helper, Oauth2) {
        var apiName = {
            "getProductByCategory": "api/category/$1/products/index=$2size=$3filter=$4"
        };
        return {
            getProduct: function (categoryId, pageSize, pageIndex, orderBy, success, fail) {
                var headers = {
                    'Content-Type': "application/json"
                };
                var api = Helper.fixUrlAPI(apiName.getProductByCategory, [categoryId, pageSize, pageIndex, orderBy]);
                API.request(false, "get", api, headers, null).then(function (result) {
                    success(result.data.results);
                    fail(null);
                }, function (error) {
                    success(null);
                    fail(error);
                });
            }
        };
    });
}