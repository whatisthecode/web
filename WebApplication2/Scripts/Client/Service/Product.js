if ("undefined" !== typeof app) {
    app.factory('Product', function (API, Helper, Oauth2) {
        var apiName = {
            "getProductByCategory": "api/category/$1/products/?pageIndex=$2&pageSize=$3&order=$4",
            "getProducts": "api/products/?pageIndex=$1&pageSize=$2&order=$3"
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
            },
            getProducts: function (pageIndex, pageSize, orderBy, success, fail) {
                var headers = {
                    'Content-Type': "application/json"
                };
                var api = Helper.fixUrlAPI(apiName.getProducts, [pageIndex, pageSize, orderBy]);
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